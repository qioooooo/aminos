using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004CE RID: 1230
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	public sealed class LCIDConversionAttribute : Attribute
	{
		// Token: 0x060030FC RID: 12540 RVA: 0x000A8C8A File Offset: 0x000A7C8A
		public LCIDConversionAttribute(int lcid)
		{
			this._val = lcid;
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x060030FD RID: 12541 RVA: 0x000A8C99 File Offset: 0x000A7C99
		public int Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018AE RID: 6318
		internal int _val;
	}
}
