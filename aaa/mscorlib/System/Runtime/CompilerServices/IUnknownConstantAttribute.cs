using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005F7 RID: 1527
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[Serializable]
	public sealed class IUnknownConstantAttribute : CustomConstantAttribute
	{
		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x060037C0 RID: 14272 RVA: 0x000BBAA1 File Offset: 0x000BAAA1
		public override object Value
		{
			get
			{
				return new UnknownWrapper(null);
			}
		}
	}
}
