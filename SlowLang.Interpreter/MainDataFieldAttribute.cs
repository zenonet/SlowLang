namespace SlowLang.Interpreter;

/// <summary>
/// Shows which field in a Value deriver holds the actual value.
/// Used in EqualityOperator so you don't have to implement equality comparison
/// with a the same type.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class MainDataFieldAttribute : Attribute
{
}