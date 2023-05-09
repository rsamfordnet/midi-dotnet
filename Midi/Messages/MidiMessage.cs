namespace Midi.Messages;

public abstract class MidiMessage {
	public MidiStatusByte Status { get; }
}

public class RawMidiMessage : MidiMessage, VariableDataBytes {
	public byte[] DataBytes { get; init; } = Array.Empty<byte>();
}

public interface SingleDataByte {
	public byte DataByte { get; }
}

public interface DoubleDataByte {
	public byte DataByte1 { get; }
	public byte DataByte2 { get; }
}

public interface VariableDataBytes {
	public byte[] DataBytes { get; }
}
