namespace Midi.Messages.ChannelVoice;

public class ProgramChangeMessage : MidiMessage, SingleDataByte {
	public byte DataByte { get; }
}
