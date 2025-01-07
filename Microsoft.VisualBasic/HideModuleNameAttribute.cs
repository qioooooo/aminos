using System;

namespace Microsoft.VisualBasic
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class HideModuleNameAttribute : Attribute
	{
	}
}
