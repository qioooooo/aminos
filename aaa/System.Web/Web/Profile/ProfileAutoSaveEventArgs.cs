using System;
using System.Security.Permissions;

namespace System.Web.Profile
{
	// Token: 0x02000310 RID: 784
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProfileAutoSaveEventArgs : EventArgs
	{
		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x0600269D RID: 9885 RVA: 0x000A5882 File Offset: 0x000A4882
		public HttpContext Context
		{
			get
			{
				return this._Context;
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x0600269E RID: 9886 RVA: 0x000A588A File Offset: 0x000A488A
		// (set) Token: 0x0600269F RID: 9887 RVA: 0x000A5892 File Offset: 0x000A4892
		public bool ContinueWithProfileAutoSave
		{
			get
			{
				return this._ContinueSave;
			}
			set
			{
				this._ContinueSave = value;
			}
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x000A589B File Offset: 0x000A489B
		public ProfileAutoSaveEventArgs(HttpContext context)
		{
			this._Context = context;
		}

		// Token: 0x04001DD1 RID: 7633
		private HttpContext _Context;

		// Token: 0x04001DD2 RID: 7634
		private bool _ContinueSave = true;
	}
}
