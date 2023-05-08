using System.Text;

namespace Midi.IO;

public class MidiBinaryReader : BinaryReader {
	private bool endOfMessage;

	public MidiBinaryReader(Stream input) : base(input) { }

	public bool EndOfMessage {
		get => this.endOfMessage || this.PeekChar() == MidiStatusByte.SystemExclusiveEnd.Value;
		protected set => this.endOfMessage = value;
	}

	public string ReadMidiString() {
		var sb = new StringBuilder();

		for (var ch = (char) this.ReadByte();
			 ch != char.MinValue && ch != MidiStatusByte.SystemExclusiveEnd.Value;
			 ch = (char)this.ReadByte()) {
			sb.Append(ch);
			this.EndOfMessage = ch == MidiStatusByte.SystemExclusiveEnd.Value;
		}

		return sb.ToString();
	}
}
