namespace Midi;

using System.Diagnostics.CodeAnalysis;

/// <summary>
///		Status bytes are eight-bit binary numbers in which the Most Significant Bit (MSB) is set (binary 1).
///		Status bytes serve to identify the message type, that is, the purpose of the Data bytes which follow it.
///		Except for Real-Time messages, new Status bytes will always command a receiver to adopt a new status,
///		even if the last message was not completed.
/// </summary>
public sealed record MidiStatusByte {
	public static readonly byte[] AllStatusBytes = Enumerable.Range(
		MidiByte.StatusRange.Start.Value,
		MidiByte.StatusRange.End.Value
	).Select(x => (byte)x).ToArray();

	#region Channel Status Bytes

	public static readonly MidiStatusByte NoteOff = new(0x80, highOnly: true);
	public static readonly MidiStatusByte NoteOn = new(0x90, highOnly: true);
	public static readonly MidiStatusByte ControlChange = new(0xB0, highOnly: true);
	public static readonly MidiStatusByte ProgramChange = new(0xC0, highOnly: true);

	#endregion

	#region System Realtime Status Bytes (MIDI 1.0 Detailed Specification 4.2.1 - Page 30)

	public static readonly MidiStatusByte SongSelect = new(0xF3);
	public static readonly MidiStatusByte TimingClock = new(0xF8);
	public static readonly MidiStatusByte Start = new(0xFA);
	public static readonly MidiStatusByte Continue = new(0xFB);
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
		Value = value;
		HighOnly = highOnly;

		if (!values.Add(this)) {
			throw new ArgumentException($"Status byte {value} has already been registered.", nameof(value));
		}
	}

	public byte Value { get; }
	public bool HighOnly { get; }

	/// <returns>
	///		True if the byte is a status byte, otherwise false if it is a data byte.
	/// </returns>
	public static bool IsStatusByte(byte value) {
		return MidiByte.IsStatusByte(value);
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
