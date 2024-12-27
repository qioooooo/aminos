using System;
using System.Security.Permissions;

namespace System.Web.Security
{
	// Token: 0x02000327 RID: 807
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class AnonymousIdentificationEventArgs : EventArgs
	{
		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x060027A6 RID: 10150 RVA: 0x000AD89B File Offset: 0x000AC89B
		// (set) Token: 0x060027A7 RID: 10151 RVA: 0x000AD8A3 File Offset: 0x000AC8A3
		public string AnonymousID
		{
			get
			{
				return this._AnonymousId;
			}
			set
			{
				this._AnonymousId = value;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x060027A8 RID: 10152 RVA: 0x000AD8AC File Offset: 0x000AC8AC
		public HttpContext Context
		{
			get
			{
				return this._Context;
			}
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x000AD8B4 File Offset: 0x000AC8B4
		public AnonymousIdentificationEventArgs(HttpContext context)
		{
			this._Context = context;
		}

		// Token: 0x04001E61 RID: 7777
		private string _AnonymousId;

		// Token: 0x04001E62 RID: 7778
		private HttpContext _Context;
	}
}
