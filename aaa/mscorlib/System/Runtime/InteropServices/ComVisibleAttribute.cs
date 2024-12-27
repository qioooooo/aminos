using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004CC RID: 1228
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
	[ComVisible(true)]
	public sealed class ComVisibleAttribute : Attribute
	{
		// Token: 0x060030F8 RID: 12536 RVA: 0x000A8C57 File Offset: 0x000A7C57
		public ComVisibleAttribute(bool visibility)
		{
			this._val = visibility;
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x060030F9 RID: 12537 RVA: 0x000A8C66 File Offset: 0x000A7C66
		public bool Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018AC RID: 6316
		internal bool _val;
	}
}
