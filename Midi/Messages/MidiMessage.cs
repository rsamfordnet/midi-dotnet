namespace Midi.Messages;

public abstract class MidiMessage {
	public static readonly RawMidiMessage Empty = new() {
		Status = byte.MinValue,
		Data = Array.Empty<byte>(),
	};

	public required byte Status { get; init; }
}

public abstract class MidiFixedLengthMessage : MidiMessage {
	public required byte DataByte1 { get; init; }
	public required byte DataByte2 { get; init; }
}

public abstract class MidiVariableLengthMessage : MidiMessage {
	public required ReadOnlyMemory<byte> Data { get; init; } = Array.Empty<byte>();
}
