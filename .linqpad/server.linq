<Query Kind="Program">
  <Reference Relative="..\Midi.Bedrock\bin\Debug\net7.0\Midi.Bedrock.dll">C:\dev\chaws\Midi\Midi.Bedrock\bin\Debug\net7.0\Midi.Bedrock.dll</Reference>
  <Reference Relative="..\Midi.Bedrock\bin\Debug\net7.0\Midi.dll">C:\dev\chaws\Midi\Midi.Bedrock\bin\Debug\net7.0\Midi.dll</Reference>
  <Namespace>Bedrock.Framework</Namespace>
  <Namespace>Bedrock.Framework.Protocols</Namespace>
  <Namespace>Microsoft.AspNetCore.Connections</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>Midi.Bedrock</Namespace>
  <Namespace>Midi.Messages</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

async Task Main(string[] args) {
	var services = new ServiceCollection();
	services.AddLogging(logger => {
		logger.SetMinimumLevel(LogLevel.Information);
		logger.AddConsole();
		//logger.AddSimpleConsole(options => {
		//	options.IncludeScopes = true;
		//	options.SingleLine = true;
		//	options.TimestampFormat = "HH:mm:ss ";
		//});
	});
	
	services.AddSingleton<MidiProtocol>();

	await using var serviceProvider = services.BuildServiceProvider();
	var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Server");

	var serverBuilder = new ServerBuilder(serviceProvider);
	serverBuilder.HeartBeatInterval = TimeSpan.FromSeconds(15);
	serverBuilder.UseSockets(sockets => {
		sockets.Listen(
			address: IPAddress.Any,
			port: 50_000,
			configure: builder => builder
				.UseConnectionLogging()
				.UseConnectionHandler<YamahaConnectionHandler>()
		);
	});

	var server = serverBuilder.Build();

	try {
		await server.StartAsync(QueryCancelToken);

		logger.LogInformation("Server started...");

		foreach (var ep in server.EndPoints) {
			logger.LogInformation("Listening on {EndPoint}", ep);
		}

		while (!QueryCancelToken.IsCancellationRequested) {
			await Task.Delay(1);
		}
	} finally {
		await server.StopAsync(QueryCancelToken);
		logger.LogInformation("Server stopped");
	}
}

public class YamahaConnectionHandler : ConnectionHandler {
	private readonly static TimeSpan heartbeatInterval = TimeSpan.FromSeconds(1);
	private readonly ILogger<YamahaConnectionHandler> logger;
	private readonly MidiProtocol protocol;

	public YamahaConnectionHandler(
		ILogger<YamahaConnectionHandler> logger,
		MidiProtocol protocol
	) {
		this.logger = logger;
		this.protocol = protocol;
	}

	public override async Task OnConnectedAsync(ConnectionContext connection) {
		this.logger.LogInformation($"Client connected: {connection.ConnectionId}");

		var ct = connection.ConnectionClosed;
		var reader = connection.CreateReader();
		var writer = connection.CreateWriter();

		var sw = Stopwatch.StartNew();
		await writer.WriteAsync(this.protocol, MidiMessageBuilder.Heartbeat);

		try {
			while (!ct.IsCancellationRequested) {
				var result = await reader.ReadAsync(this.protocol, ct);
				var message = result.Message;

				if (message is RawMidiMessage raw && raw.DataBytes.Length > 0) {
					this.logger.LogDebug($"Received a message of {raw.DataBytes.Length} bytes");
				}

				if (sw.Elapsed >= heartbeatInterval) {
					sw.Restart();
					await writer.WriteAsync(this.protocol, MidiMessageBuilder.Heartbeat, ct);
					this.logger.LogTrace($"Hearbeat sent to {connection.ConnectionId}");
					Console.Write('.');
				}

				//	new BinaryReader(new MemoryStream(sysExData))
				//
				//				var sysExData = result.Message.ToArray();
				//				SysexEvent sysExEvent = SysexEvent.ReadSysexEvent();
				//
				//				// Process the SysEx message
				//				ProcessSysExMessage(sysExEvent);

				reader.Advance();
			}
		} catch (Exception ex) {
			this.logger.LogError($"Error handling client: {ex.Message}");
		} finally {
			await connection.DisposeAsync();
			this.logger.LogInformation($"Client disconnected: {connection.ConnectionId}");
		}
	}

	//static void ProcessSysExMessage(SysexEvent sysExEvent) {
	//	// Handle SysEx message here
	//	this.logger.LogTrace($"Received SysEx message: {BitConverter.ToString(sysExEvent.Data)}");
	//}
}
