using System.Diagnostics.CodeAnalysis;

namespace Midi;

/// <summary>
///		Status bytes are eight-bit binary numbers in which the Most Significant Bit (MSB) is set (binary 1).
///		Status bytes serve to identify the message type, that is, the purpose of the Data bytes which follow it.
///		Except for Real-Time messages, new Status bytes will always command a receiver to adopt a new status,
///		even if the last message was not completed.
/// </summary>
public sealed record MidiStatusByte {
	/// <remarks>
	///		MIDI 1.0 Detailed Specification 4.2.1 - Page 3 (PDF Page 8)
	/// </remarks>
	public const byte MinStatusByte = 0x80;

	#region Channel Status Bytes

	public static readonly MidiStatusByte NoteOff = new(0x80, highOnly: true);
	public static readonly MidiStatusByte NoteOn = new(0x90, highOnly: true);
	public static readonly MidiStatusByte ControlChange = new(0xB0, highOnly: true);
	public static readonly MidiStatusByte ProgramChange = new(0xC0, highOnly: true);

	#endregion

	#region System Realtime Status Bytes (MIDI 1.0 Detailed Specification 4.2.1 - Page 30)

	public static readonly MidiStatusByte SongSelect = new(0xF3);

	/// <summary>
	/// 	Clock-based MIDI systems are synchronized with this message, which is
	/// 	sent at a rate of 24 per quarter note. If Timing Clocks (F8H) are sent during
	/// 	idle time they should be sent at the current tempo setting of the transmitter
	/// 	even while it is not playing. Receivers which are synchronized to incoming
	/// 	Real Time messages (MIDI Sync mode) can thus phase lock their internal
	/// 	clocks while waiting for a Start (FAH) or Continue (FBH) command.
	/// </summary>
	public static readonly MidiStatusByte TimingClock = new(0xF8);

	/// <summary>
	///		Start (FAH) is sent when a PLAY button on the master (sequencer or drum
	///		machine) is pressed. This message commands all receivers which are
	///		synchronized to incoming Real Time messages (MIDI Sync mode) to start
	///		at the beginning of the song or sequence.
	/// </summary>
	public static readonly MidiStatusByte Start = new(0xFA);

	/// <summary>
	///		Continue (FBH) is sent when a CONTINUE button is hit.
	///		A sequence will continue from its current location upon
	///		receipt of the next Timing Clock (F8H).
	/// </summary>
	public static readonly MidiStatusByte Continue = new(0xFB);

	/// <summary>
	///		Stop (FCH) is sent when a STOP button is hit.
	///		Playback in a receiver should	stop immediately.
	/// </summary>
	public static readonly MidiStatusByte Stop = new(0xFC);
	public static readonly MidiStatusByte ActiveSensing = new(0xFE);
	public static readonly MidiStatusByte SystemReset = new(0xFF);

	#endregion

	#region System Exclusive Status Bytes

	public static readonly MidiStatusByte SystemExclusiveStart = new(0xF0);
	public static readonly MidiStatusByte SystemExclusiveEnd = new(0xF7);

	#endregion

	private static readonly HashSet<MidiStatusByte> values = new();

	private MidiStatusByte(byte value, bool highOnly = false) {
		this.Value = value;
		this.HighOnly = highOnly;

		if (!values.Add(this)) {
			throw new ArgumentException($"Status byte {value} has already been registered.", nameof(value));
		}
	}

	public byte Value { get; }
	public bool HighOnly { get; }

	/// <summary>
	///
	/// </summary>
	/// <param name="value"></param>
	/// <returns>True if the byte is a status byte, otherwise false if it is a data byte.</returns>
	public static bool IsStatusByte(byte value) {
		return value >= MinStatusByte;
	}

	public static bool TryGet(byte statusByte, [NotNullWhen(true)] out MidiStatusByte? status) {
		foreach (var value in values) {
			if (value.Value.GetHighNibble() != statusByte.GetHighNibble()) {
				continue;
			}

			if (value.HighOnly) {
				continue;
			}

			if (value.Value.GetLowNibble() != statusByte.GetLowNibble()) {
				continue;
			}

			status = value;
			return true;
		}

		status = null;
		return false;
	}
}
