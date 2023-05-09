namespace Midi.Messages.ChannelVoice;

public class PolyphonicKeyPressureMessage : MidiMessage, DoubleDataByte {
	public byte DataByte1 { get; }
	public byte DataByte2 { get; }
}
