using System;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Web.Security
{
	// Token: 0x02000335 RID: 821
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class FormsAuthenticationEventArgs : EventArgs
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x0600283F RID: 10303 RVA: 0x000B0ED8 File Offset: 0x000AFED8
		// (set) Token: 0x06002840 RID: 10304 RVA: 0x000B0EE0 File Offset: 0x000AFEE0
		public IPrincipal User
		{
			get
			{
				return this._User;
			}
			[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
			set
			{
				this._User = value;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002841 RID: 10305 RVA: 0x000B0EE9 File Offset: 0x000AFEE9
		public HttpContext Context
		{
			get
			{
				return this._Context;
			}
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x000B0EF1 File Offset: 0x000AFEF1
		public FormsAuthenticationEventArgs(HttpContext context)
		{
			this._Context = context;
		}

		// Token: 0x04001E9B RID: 7835
		private IPrincipal _User;

		// Token: 0x04001E9C RID: 7836
		private HttpContext _Context;
	}
}
