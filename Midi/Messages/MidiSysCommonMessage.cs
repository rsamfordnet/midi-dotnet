namespace Midi.Messages;

/// <summary>
/// 	Common messages are intended for all receivers in a system regardless of channel.
/// </summary>
public class MidiSysCommonMessage : MidiVariableLengthMessage {
	public required MidiSysCommonMessageType Type { get; init; }
}
