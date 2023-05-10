namespace Midi.Bedrock;

using System.Diagnostics;
using Microsoft.AspNetCore.Connections;

public class MidiConnectionHandler : ConnectionHandler {
	private static readonly TimeSpan heartbeatInterval = TimeSpan.FromSeconds(1);
	private readonly ILogger<MidiConnectionHandler> logger;
	private readonly MidiProtocol protocol;

	public MidiConnectionHandler(
		ILogger<MidiConnectionHandler> logger,
		MidiProtocol protocol
	) {
		this.logger = logger;
		this.protocol = protocol;
	}

	public override async Task OnConnectedAsync(ConnectionContext connection) {
		logger.LogInformation($"Client connected: {connection.ConnectionId}");

		var ct = connection.ConnectionClosed;
		var reader = connection.CreateReader();
		var writer = connection.CreateWriter();

		var sw = Stopwatch.StartNew();
		await writer.WriteAsync(protocol, MidiMessageBuilder.Heartbeat);

		try {
			while (!ct.IsCancellationRequested) {
				var result = await reader.ReadAsync(protocol, ct);
				var message = result.Message;

				if (message.Payload.Length > 0) {
					logger.LogDebug($"Received a message of {message.Payload.Length} bytes");
				}

				if (sw.Elapsed >= heartbeatInterval) {
					sw.Restart();
					await writer.WriteAsync(protocol, MidiMessageBuilder.Heartbeat, ct);
					logger.LogTrace($"Hearbeat sent to {connection.ConnectionId}");
					Console.Write('.');
				}

				reader.Advance();
			}
		} catch (Exception ex) {
			logger.LogError($"Error handling client: {ex.Message}");
		} finally {
			await connection.DisposeAsync();
			logger.LogInformation($"Client disconnected: {connection.ConnectionId}");
		}
	}
}
