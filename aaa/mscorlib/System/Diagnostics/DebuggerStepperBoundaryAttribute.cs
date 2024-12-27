using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002A0 RID: 672
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class DebuggerStepperBoundaryAttribute : Attribute
	{
	}
}
