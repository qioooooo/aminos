using System;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Web.Security
{
	// Token: 0x02000349 RID: 841
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PassportAuthenticationEventArgs : EventArgs
	{
		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x060028C7 RID: 10439 RVA: 0x000B2BC4 File Offset: 0x000B1BC4
		// (set) Token: 0x060028C8 RID: 10440 RVA: 0x000B2BCC File Offset: 0x000B1BCC
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

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x060028C9 RID: 10441 RVA: 0x000B2BD5 File Offset: 0x000B1BD5
		public HttpContext Context
		{
			get
			{
				return this._Context;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x060028CA RID: 10442 RVA: 0x000B2BDD File Offset: 0x000B1BDD
		public PassportIdentity Identity
		{
			get
			{
				return this._Identity;
			}
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x000B2BE5 File Offset: 0x000B1BE5
		public PassportAuthenticationEventArgs(PassportIdentity identity, HttpContext context)
		{
			this._Identity = identity;
			this._Context = context;
		}

		// Token: 0x04001ED9 RID: 7897
		private IPrincipal _User;

		// Token: 0x04001EDA RID: 7898
		private HttpContext _Context;

		// Token: 0x04001EDB RID: 7899
		private PassportIdentity _Identity;
	}
}
