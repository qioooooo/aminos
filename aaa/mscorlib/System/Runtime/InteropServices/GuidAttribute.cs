using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E1 RID: 1249
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
	public sealed class GuidAttribute : Attribute
	{
		// Token: 0x06003123 RID: 12579 RVA: 0x000A903D File Offset: 0x000A803D
		public GuidAttribute(string guid)
		{
			this._val = guid;
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06003124 RID: 12580 RVA: 0x000A904C File Offset: 0x000A804C
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001940 RID: 6464
		internal string _val;
	}
}
