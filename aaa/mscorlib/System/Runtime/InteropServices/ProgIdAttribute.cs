using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004D1 RID: 1233
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	[ComVisible(true)]
	public sealed class ProgIdAttribute : Attribute
	{
		// Token: 0x06003100 RID: 12544 RVA: 0x000A8CB1 File Offset: 0x000A7CB1
		public ProgIdAttribute(string progId)
		{
			this._val = progId;
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06003101 RID: 12545 RVA: 0x000A8CC0 File Offset: 0x000A7CC0
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018AF RID: 6319
		internal string _val;
	}
}
