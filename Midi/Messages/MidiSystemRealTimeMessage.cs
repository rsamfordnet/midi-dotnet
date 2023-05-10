namespace Midi.Messages;

/// <summary>
/// 	Real-Time messages are used for synchronization and are intended for all clock-
/// 	based units in a system. They contain Status bytes only — no Data bytes. Real-
/// 	Time messages may be sent at any time — even between bytes of a message
/// 	which has a different status. In such cases the Real-Time message is either acted
/// 	upon or ignored, after which the receiving process resumes under the previous
/// 	status.
/// </summary>
public class MidiSystemRealTimeMessage : MidiMessage {
	/// <summary>
	/// The type of real-time midi message.
	/// </summary>
	public MidiSystemRealTimeType Type { get; set; }
}
