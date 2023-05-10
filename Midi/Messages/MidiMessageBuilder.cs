namespace Midi.Messages;

public static class MidiMessageBuilder {
	public static readonly RawMidiMessage Empty = new() {
		Status = MidiStatusByte.Unknown.Value,
		Payload = Array.Empty<byte>(),
		Data = Array.Empty<byte>(),
	};

	public static readonly RawMidiMessage Heartbeat = new() {
		Status = MidiStatusByte.Unknown.Value,
		Payload = new byte[] { 0xF0, 0x43, 0x10, 0x3E, 0x19, 0x7F, 0xF7 },
		Data = Array.Empty<byte>(),
	};

	public static RawMidiMessage GenericHandshake(byte deviceId, GenericHandshakeType type, byte packetNumber) {
		return new() {
			Status = MidiStatusByte.Unknown.Value,
			Payload = new byte[] { 0xF0, 0x7E, deviceId, (byte)type, packetNumber, 0xF7 },
			Data = Array.Empty<byte>(),
		};
	}
}

public enum GenericHandshakeType : byte {
	/// <summary>
	/// 	This is the first handshaking flag. It means "Last data packet was received correctly. Start sending
	/// 	the next one." The packet number represents the packet being acknowledged as correct.
	/// </summary>
	ACK = 0x7F,

	/// <summary>
	/// 	This is the second handshaking flag. It means "Last data packet was received incorrectly. Please re-send."
	///		The packet number represents the packet being rejected.
	/// </summary>
	NAK = 0x7E,

	/// <summary>
	/// 	This is the third handshaking flag. It means "Abort dump." The packet number represents the
	///		packet on which the abort takes place.
	/// </summary>
	CANCEL = 0x7D,

	/// <summary>
	/// 	This is the fourth handshaking flag. It means "Do not send any more packets until told to do
	///		otherwise." This is important for systems in which the receiver (such as a computer) may need to
	///		perform other operations (such as disk access) before receiving the remainder of the dump. An ACK
	///		will continue the dump while a Cancel will abort the dump.
	/// </summary>
	WAIT = 0x7C,

	/// <summary>
	/// 	This is a new generic handshaking flag which was added for the File Dump extension, and is
	///		described fully under the File Dump heading.
	/// </summary>
	EOF = 0x7B,
}
