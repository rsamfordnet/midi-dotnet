namespace Midi.Messages.System;

/// <summary>
/// 	Common messages are intended for all receivers in a system regardless of channel.
/// </summary>
public class SystemCommonMessage : MidiMessage, VariableDataBytes {
	public byte StatusByte { get; }
	public byte[] DataBytes { get; } = Array.Empty<byte>();
}
