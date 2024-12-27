using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005CA RID: 1482
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[Serializable]
	public abstract class CustomConstantAttribute : Attribute
	{
		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06003789 RID: 14217
		public abstract object Value { get; }
	}
}
