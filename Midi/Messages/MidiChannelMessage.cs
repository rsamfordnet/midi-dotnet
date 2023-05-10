namespace Midi.Messages;

public class MidiChannelMessage : MidiFixedLengthMessage {
	public required MidiChannelCommandType Command { get; init; }
	public required byte Channel { get; init; }
}
