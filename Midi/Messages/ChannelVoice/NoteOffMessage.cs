namespace Midi.Messages.ChannelVoice;

public class NoteOffMessage : MidiMessage, DoubleDataByte {
	public byte DataByte1 { get; }
	public byte DataByte2 { get; }
}
