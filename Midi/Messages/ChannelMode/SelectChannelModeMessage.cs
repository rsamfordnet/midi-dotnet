namespace Midi.Messages.ChannelMode;

public class SelectChannelModeMessage : MidiMessage, DoubleDataByte {
	public byte DataByte1 { get; }
	public byte DataByte2 { get; }
}
