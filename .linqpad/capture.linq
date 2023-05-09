<Query Kind="Program">
  <Reference Relative="..\Midi.Bedrock\bin\Debug\net7.0\Midi.Bedrock.dll">C:\dev\chaws\Midi\Midi.Bedrock\bin\Debug\net7.0\Midi.Bedrock.dll</Reference>
  <Reference Relative="..\Midi.Bedrock\bin\Debug\net7.0\Midi.dll">C:\dev\chaws\Midi\Midi.Bedrock\bin\Debug\net7.0\Midi.dll</Reference>
  <NuGetReference>SharpPcap</NuGetReference>
  <Namespace>PacketDotNet</Namespace>
  <Namespace>SharpPcap</Namespace>
  <Namespace>SharpPcap.LibPcap</Namespace>
  <Namespace>System.Buffers</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Midi.Utils</Namespace>
</Query>

async Task Main() {
	try {
		foreach (var device in LibPcapLiveDeviceList.Instance) {
			device.Open();
			device.NonBlockingMode = true;
			device.OnPacketArrival += Device_OnPacketArrival;
			device.StartCapture();
		}

		//using var device = LibPcapLiveDeviceList.Instance.Single(IsLoopbackDevice);
		//device.Open();
		//device.OnPacketArrival += Device_OnPacketArrival;
		//device.StartCapture();

		while (!QueryCancelToken.IsCancellationRequested) {
			await Task.Delay(1);
		}
	} finally {
		foreach (var device in LibPcapLiveDeviceList.Instance) {
			device.StopCapture();
			device.OnPacketArrival -= Device_OnPacketArrival;
			device.Close();
			device.Dispose();
		}
	}
}

void Device_OnPacketArrival(object s, PacketCapture e) {
	var rawCapture = e.GetPacket();
	if (rawCapture is null) {
		return;
	}
	
	var packet = rawCapture.GetPacket();
	if (packet is null) {
		return;
	}

	var tcpPacket = packet.Extract<TcpPacket>();
	if (tcpPacket is null) {
		return;
	}
	
	if (tcpPacket.SourcePort != 50_000 && tcpPacket.DestinationPort != 50_000) {
		return;
	}

	if (tcpPacket.SourcePort == 50_000) {
		Console.WriteLine(Environment.NewLine + "MIXER->CONTROLLER");
	}

	if (tcpPacket.DestinationPort == 50_000) {
		Console.WriteLine(Environment.NewLine + "MIXER<-CONTROLLER");
	}

	var data = e.Data;
	HexConverter.BytesToHex(data).Dump();
	
	//var messages = MidiByte.SplitByStatusBytes(data);
	//foreach (var message in messages) {
	//	HexConverter.BytesToHex(message).Dump();
	//}

//	for (var i = 0; i < data.Length; i++) {
//		var currentByte = data[i];
//		
//		if (MidiByteUtils.IsStatusByte(currentByte)) {
//			var message = data.Slice(0, i - 1);
//			data = data.Slice(i, data.Length - i);
//
//			HexConverter.BytesToHex(message).Dump();
//			//HexConverter.BytesToHex(message).Dump();
//			//Convert.ToHexString(message).Dump();
//			//Console.WriteLine(message.ToHexString(usePrefix: false));
//		}
//	}
}

class HexConverter {
	public static string BytesToHex(ReadOnlyMemory<byte> bytes) {
		var sb = new StringBuilder(bytes.Length * 3 - 1);

		for (var i = 0; i < bytes.Length; i++) {
			if (i > 0) {
				sb.Append('-');
			}
			sb.Append(bytes.Span[i].ToString("X2"));
		}

		return sb.ToString();
	}

	public static string BytesToHex(ReadOnlySpan<byte> bytes) {
		var sb = new StringBuilder(bytes.Length * 3 - 1);

		for (var i = 0; i < bytes.Length; i++) {
			if (i > 0) {
				sb.Append('-');
			}
			sb.Append(bytes[i].ToString("X2"));
		}

		return sb.ToString();
	}
	
	public static string BytesToHexFast(ReadOnlySpan<byte> bytes) {
		var outputLength = bytes.Length * 3 - 1;
		var chars = outputLength < 1024 ? stackalloc char[outputLength] : new char[outputLength];

		for (int i = 0, j = 0; i < bytes.Length; i++, j += 3) {
			if (i > 0) {
				chars[j - 1] = '-';
			}
			
			WriteByteAsHexFast(chars.Slice(j), bytes[i]);
		}

		return new string(chars);
	}

	private static void WriteByteAsHexFast(Span<char> buffer, byte value) {
		if (buffer.Length < 2) {
			throw new ArgumentException("Insufficient buffer size.", nameof(buffer));
		}

		byte low = (byte)(value & 0x0F);
		byte high = (byte)((value & 0xF0) >> 4);

		buffer[0] = (char)(high < 10 ? high + '0' : high - 10 + 'A');
		buffer[1] = (char)(low < 10 ? low + '0' : low - 10 + 'A');
	}
}