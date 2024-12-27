using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004C6 RID: 1222
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event, Inherited = false)]
	[ComVisible(true)]
	public sealed class DispIdAttribute : Attribute
	{
		// Token: 0x060030EE RID: 12526 RVA: 0x000A8BDD File Offset: 0x000A7BDD
		public DispIdAttribute(int dispId)
		{
			this._val = dispId;
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x060030EF RID: 12527 RVA: 0x000A8BEC File Offset: 0x000A7BEC
		public int Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018A0 RID: 6304
		internal int _val;
	}
}
