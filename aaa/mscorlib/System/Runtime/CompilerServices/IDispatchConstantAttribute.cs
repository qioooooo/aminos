using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005F6 RID: 1526
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class IDispatchConstantAttribute : CustomConstantAttribute
	{
		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x060037BE RID: 14270 RVA: 0x000BBA91 File Offset: 0x000BAA91
		public override object Value
		{
			get
			{
				return new DispatchWrapper(null);
			}
		}
	}
}
