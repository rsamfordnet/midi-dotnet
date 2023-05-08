namespace Midi.IO;

using System.Text;

public class MidiBinaryReader : BinaryReader {
	private bool endOfMessage;

	public MidiBinaryReader(Stream input) : base(input) { }

	public bool EndOfMessage {
		get => endOfMessage || PeekChar() == MidiStatusByte.SystemExclusiveEnd.Value;
		protected set => endOfMessage = value;
	}

	public string ReadMidiString() {
		var sb = new StringBuilder();

		for (var ch = (char)ReadByte();
			 ch != char.MinValue && ch != MidiStatusByte.SystemExclusiveEnd.Value;
			 ch = (char)ReadByte()) {
			_ = sb.Append(ch);
			EndOfMessage = ch == MidiStatusByte.SystemExclusiveEnd.Value;
		}

		return sb.ToString();
	}
}
