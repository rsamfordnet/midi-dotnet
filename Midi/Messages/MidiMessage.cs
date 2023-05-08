using System.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace Midi.Messages;

public record MidiMessage(ReadOnlySequence<byte> RawBytes) {
	public static readonly MidiMessage Empty = new(ReadOnlySequence<byte>.Empty);
	public static readonly MidiMessage Heartbeat = new(
		RawBytes: new(new byte[] { 0xF0, 0x43, 0x10, 0x3E, 0x19, 0x7F })
	);
}

public record ChannelMessage(ReadOnlySequence<byte> RawBytes) : MidiMessage(RawBytes) {

}

public record ParameterChangeMessage(ReadOnlySequence<byte> RawBytes) : MidiMessage(RawBytes) {

}

public record SystemRealTimeMessage(ReadOnlySequence<byte> RawBytes) : MidiMessage(RawBytes) {

}

public record SystemExclusiveMessage(ReadOnlySequence<byte> RawBytes) : MidiMessage(RawBytes) {
	/// <summary>
	/// 	Special ID 7D is reserved for non-commercial use (e.g. schools, research, etc.)
	/// 	and is not to be used on any product released to the public. Since Non-Commercial codes
	/// 	would not be seen or used by an ordinary user, there is no standard format.
	/// </summary>
	public const byte NonCommercialId = 0x7D;

	/// <summary>
	///		Represents a non-real time system exclusive message.
	/// </summary>
	/// <remarks>
	///		The standardized format for both Real Time and Non-Real	Time Universal Exclusive
	///		messages is as follows:
	///			F0H [ID number] [device ID] [sub-ID#1] [sub-ID#2] ... F7H
	/// </remarks>
	public const byte NonRealTimeId = 0x7E;

	/// <summary>
	///		Represents a real time system exclusive message.
	/// </summary>
	/// <remarks>
	///		The standardized format for both Real Time and Non-Real	Time Universal Exclusive
	///		messages is as follows:
	///			F0H [ID number] [device ID] [sub-ID#1] [sub-ID#2] ... F7H
	/// </remarks>
	public const byte RealTimeId = 0x7F;

	public byte SysExId { get; init; }

	/// <summary>
	///		<p>
	/// 		Since System Exclusive messages are not assigned to a MIDI Channel, the Device ID (formerly referred
	/// 		to as the "channel" byte) is intended to indicate which device in the system is supposed to respond. The
	/// 		device ID 7F, sometimes referred to as the 'all call' device ID, is used to indicate that all devices should
	/// 		respond.
	///		</p>
	///		<p>
	/// 		In most cases, the Device ID should refer to the physical device being addressed (the "hunk of metal and
	/// 		plastic" is a common term that has been used), as opposed to having the same meaning as channel or
	/// 		referring to a virtual device inside a physical device. For reference, this also corresponds to old USI
	/// 		discussions that included a "Unit ID" that was supposed to be attached to one UART and set of in/out
	/// 		ports.
	///		</p>
	///		<p>
	/// 		However, there are exceptions - for example, what Device ID to use for a dual-transport tape deck and
	/// 		MMC commands? Some may feel more comfortable thinking of the Device ID as an "address" and allow
	/// 		for the possibility that a single physical unit may be powerful enough to have more than one valid
	/// 		address. (This also has more relevance as devices move from stand-alone units to cards in a computer.)
	///		</p>
	///		<p>
	/// 		Therefore, Device ID is meant to refer to a single physical device or I/O port as a default. Sophisticated
	/// 		devices - such as multi-transport tape decks, computers with card slots, or even networks of devices -
	/// 		may have more than one Device ID, and such occurrences should be explained to the user clearly in the
	/// 		manual. From one to sixteen virtual devices may be accessed at each Device ID by use of the normal
	/// 		MIDI channel numbers, depending on the capabilities of the device.
	///		</p>
	/// </summary>
	public byte DeviceId { get; init; }
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum GenericHandshakeMessageType : byte {
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

public static class SystemExclusiveMessageBuilder {
	public static SystemExclusiveMessage GenericHandshakeMessage(byte deviceId, GenericHandshakeMessageType type, byte packetNumber) {
		return new(new(new byte[] { 0xF0, 0x7E, deviceId, (byte)type, packetNumber, 0xF7 }));
	}
}
