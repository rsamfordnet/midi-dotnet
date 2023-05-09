namespace Midi.Messages.ChannelVoice;

public class ChannelPressureMessage : MidiMessage, SingleDataByte {
	public byte DataByte { get; }
}
