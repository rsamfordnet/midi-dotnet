namespace Midi.IO;

using System.Text;

public class MidiBinaryWriter : BinaryWriter {
	public MidiBinaryWriter(Stream input) : base(input) { }

	public void WriteMidiString(string text, bool nullTerminate) {
		Write(Encoding.ASCII.GetBytes(text));

		if (!nullTerminate) {
			return;
		}

		Write((byte)0);
	}
}
