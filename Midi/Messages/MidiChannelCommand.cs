namespace Midi.Messages;

public class MidiChannelCommandType : TypedEnum<byte> {
	/// <summary>
	/// An invalid channel command.
	/// </summary>
	public static readonly MidiChannelCommandType Invalid = new(0x00, 0);

	/// <summary>
	/// Represents the note-off command type.
	/// </summary>
	public static readonly MidiChannelCommandType NoteOff = new(0x80, 2);

	/// <summary>
	/// Represents the note-on command type.
	/// </summary>
	public static readonly MidiChannelCommandType NoteOn = new(0x90, 2);

	/// <summary>
	/// Represents the poly pressure (after touch) command type.
	/// </summary>
	public static readonly MidiChannelCommandType PolyPressure = new(0xA0, 3);

	/// <summary>
	/// Represents the controller command type.
	/// </summary>
	public static readonly MidiChannelCommandType ControlChange = new(0xB0, 3);

	/// <summary>
	/// Represents the program change command type.
	/// </summary>
	public static readonly MidiChannelCommandType ProgramChange = new(0xC0, 2);

	/// <summary>
	/// Represents the channel pressure (after touch) command
	/// type.
	/// </summary>
	public static readonly MidiChannelCommandType ChannelPressure = new(0xD0, 2);

	/// <summary>
	/// Represents the pitch wheel command type.
	/// </summary>
	public static readonly MidiChannelCommandType PitchWheel = new(0xE0, 3);

	private MidiChannelCommandType(byte value, int parameters) : base(value) {
		Parameters = parameters;
	}

	public int Parameters { get; set; }
}
