namespace Windows.Win32.System.Com;

[AttributeUsage(AttributeTargets.Class)]
public sealed class InterfaceAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Method)]
public sealed class MethodAttribute(int index = -1) : Attribute
{
	public int Index { get; set; } = index;
}