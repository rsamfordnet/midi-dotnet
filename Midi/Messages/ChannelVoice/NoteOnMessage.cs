namespace Midi.Messages.ChannelVoice;

public class NoteOnMessage : MidiMessage, DoubleDataByte {
	public byte DataByte1 { get; }
	public byte DataByte2 { get; }
}
