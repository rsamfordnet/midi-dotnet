namespace Midi.Messages;

public abstract class MidiMessage {
	public required byte Status { get; init; }
	public required ReadOnlyMemory<byte> Payload { get; init; } = Array.Empty<byte>();
}

public abstract class MidiFixedLengthMessage : MidiMessage {
	public required byte Parameter1 { get; init; }
	public required byte Parameter2 { get; init; }
}

public abstract class MidiVariableLengthMessage : MidiMessage {
	public required ReadOnlyMemory<byte> Data { get; init; } = Array.Empty<byte>();
}
