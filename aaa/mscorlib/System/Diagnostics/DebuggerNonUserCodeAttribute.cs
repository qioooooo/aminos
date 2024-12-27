using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002A2 RID: 674
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class DebuggerNonUserCodeAttribute : Attribute
	{
	}
}
