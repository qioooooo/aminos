using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200001E RID: 30
	[ComVisible(false)]
	public sealed class SharedProperty
	{
		// Token: 0x0600005B RID: 91 RVA: 0x000020D0 File Offset: 0x000010D0
		internal SharedProperty(ISharedProperty prop)
		{
			this._x = prop;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000020DF File Offset: 0x000010DF
		// (set) Token: 0x0600005D RID: 93 RVA: 0x000020EC File Offset: 0x000010EC
		public object Value
		{
			get
			{
				return this._x.Value;
			}
			set
			{
				this._x.Value = value;
			}
		}

		// Token: 0x0400001C RID: 28
		private ISharedProperty _x;
	}
}
