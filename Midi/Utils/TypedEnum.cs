namespace Midi.Utils;

public abstract class TypedEnum<T> where T : struct, IComparable, IFormattable, IConvertible {
	public T Value { get; private set; }

	protected TypedEnum(T value) {
		Value = value;
	}

	public override string ToString() {
		return Value.ToString() ?? string.Empty;
	}

	public override bool Equals(object? obj) {
		return obj is TypedEnum<T> other && EqualityComparer<T>.Default.Equals(Value, other.Value);
	}

	public override int GetHashCode() {
		return Value.GetHashCode();
	}

	public static bool operator ==(TypedEnum<T> a, TypedEnum<T> b) => a is null ? b is null : a.Equals(b);
	public static bool operator !=(TypedEnum<T> a, TypedEnum<T> b) => !(a == b);
	public static implicit operator T(TypedEnum<T> typedEnum) => typedEnum.Value;
}
