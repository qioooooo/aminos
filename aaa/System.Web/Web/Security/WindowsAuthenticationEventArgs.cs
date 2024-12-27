using System;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Web.Security
{
	// Token: 0x02000358 RID: 856
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WindowsAuthenticationEventArgs : EventArgs
	{
		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x060029B8 RID: 10680 RVA: 0x000BA813 File Offset: 0x000B9813
		// (set) Token: 0x060029B9 RID: 10681 RVA: 0x000BA81B File Offset: 0x000B981B
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

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x060029BA RID: 10682 RVA: 0x000BA824 File Offset: 0x000B9824
		public HttpContext Context
		{
			get
			{
				return this._Context;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x060029BB RID: 10683 RVA: 0x000BA82C File Offset: 0x000B982C
		public WindowsIdentity Identity
		{
			get
			{
				return this._Identity;
			}
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x000BA834 File Offset: 0x000B9834
		public WindowsAuthenticationEventArgs(WindowsIdentity identity, HttpContext context)
		{
			this._Identity = identity;
			this._Context = context;
		}

		// Token: 0x04001F1A RID: 7962
		private IPrincipal _User;

		// Token: 0x04001F1B RID: 7963
		private HttpContext _Context;

		// Token: 0x04001F1C RID: 7964
		private WindowsIdentity _Identity;
	}
}
