namespace Midi.IO;

using System.Text;
using Midi.Messages;

public class MidiBinaryReader : BinaryReader {
	private bool endOfMessage;

	public MidiBinaryReader(Stream input) : base(input) { }

	public bool EndOfMessage {
		get => endOfMessage || PeekChar() == MidiSysExMessage.EndMarker;
		protected set => endOfMessage = value;
	}

	public string ReadMidiString() {
		var sb = new StringBuilder();

		for (var ch = (char)ReadByte();
			 ch is not char.MinValue and not (char)MidiSysExMessage.EndMarker;
			 ch = (char)ReadByte()) {
			_ = sb.Append(ch);
			EndOfMessage = ch == MidiSysExMessage.EndMarker;
		}

		return sb.ToString();
	}
}
