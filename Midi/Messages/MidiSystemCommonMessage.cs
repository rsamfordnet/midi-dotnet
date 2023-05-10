namespace Midi.Messages;

/// <summary>
/// 	Common messages are intended for all receivers in a system regardless of channel.
/// </summary>
public class MidiSystemCommonMessage : MidiVariableLengthMessage {
	private readonly byte[] data;

	public MidiSystemCommonMessage(byte[] data) {
		this.data = data;
	}

	public required MidiSystemCommonMessageType Type { get; init; }
}
