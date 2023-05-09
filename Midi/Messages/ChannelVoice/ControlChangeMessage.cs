namespace Midi.Messages.ChannelVoice;

public class ControlChangeMessage : MidiMessage, DoubleDataByte {
	public byte DataByte1 { get; }
	public byte DataByte2 { get; }
}
