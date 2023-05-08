using System.Text;

namespace Midi.IO;

public class MidiBinaryWriter : BinaryWriter {
	public MidiBinaryWriter(Stream input) : base(input) { }

	public void WriteMidiString(string text, bool nullTerminate) {
		this.Write(Encoding.ASCII.GetBytes(text));
		if (!nullTerminate)
			return;
		this.Write((byte) 0);
	}
}
