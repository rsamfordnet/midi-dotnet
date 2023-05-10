namespace Midi.Messages;

/// <summary>
/// 	<p>
/// 		Exclusive messages can contain any number of Data bytes, and can be
/// 		terminated either by an End of Exclusive (EOX) or any other Status byte (except
/// 		Real Time messages). An EOX should always be sent at the end of a System
/// 		Exclusive message. These messages include a Manufacturer's Identification (ID)
/// 		code. If a receiver does not recognize the ID code, it should ignore the following
/// 		data.
/// 	</p>
/// 	<p>
/// 		So that other users and third party developers can fully access their instruments,
/// 		manufacturers must publish the format of the System Exclusive data following
/// 		their ID code. Only the manufacturer can define or update the format following
/// 		their ID.
/// 	</p>
/// </summary>
public class MidiSysExMessage : MidiVariableLengthMessage {
	public const byte StartMarker = 0xF0;
	public const byte EndMarker = 0xF7;

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

public enum MidiManufacturerId : byte {
	Unknown = 0x00,

	/// <summary>
	/// 	Special ID 7D is reserved for non-commercial use (e.g. schools, research, etc.)
	/// 	and is not to be used on any product released to the public. Since Non-Commercial codes
	/// 	would not be seen or used by an ordinary user, there is no standard format.
	/// </summary>
	NonCommercial = 0x7D,

	/// <summary>
	///		Represents a non-real time system exclusive message.
	/// </summary>
	/// <remarks>
	///		The standardized format for both Real Time and Non-Real	Time Universal Exclusive
	///		messages is as follows:
	///			F0H [ID number] [device ID] [sub-ID#1] [sub-ID#2] ... F7H
	/// </remarks>
	UniversalNonRealTime = 0x7E,

	/// <summary>
	///		Represents a real time system exclusive message.
	/// </summary>
	/// <remarks>
	///		The standardized format for both Real Time and Non-Real	Time Universal Exclusive
	///		messages is as follows:
	///			F0H [ID number] [device ID] [sub-ID#1] [sub-ID#2] ... F7H
	/// </remarks>
	UniversalRealTime = 0x7F,
}

public enum MidiDeviceId : byte {
	Unknown = 0x00,

	/// <summary>
	///		The device ID 7F, sometimes referred to as the 'all call' device ID, is used to
	/// 	indicate that all devices should respond.
	/// </summary>
	AllCall = 0x7F,
}
