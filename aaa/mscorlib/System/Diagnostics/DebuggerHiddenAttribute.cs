using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002A1 RID: 673
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
	[Serializable]
	public sealed class DebuggerHiddenAttribute : Attribute
	{
	}
}
