using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004C0 RID: 1216
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class AuthenticateEventArgs : EventArgs
	{
		// Token: 0x060039DB RID: 14811 RVA: 0x000F4D70 File Offset: 0x000F3D70
		public AuthenticateEventArgs()
			: this(false)
		{
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x000F4D79 File Offset: 0x000F3D79
		public AuthenticateEventArgs(bool authenticated)
		{
			this._authenticated = authenticated;
		}

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x060039DD RID: 14813 RVA: 0x000F4D88 File Offset: 0x000F3D88
		// (set) Token: 0x060039DE RID: 14814 RVA: 0x000F4D90 File Offset: 0x000F3D90
		public bool Authenticated
		{
			get
			{
				return this._authenticated;
			}
			set
			{
				this._authenticated = value;
			}
		}

		// Token: 0x04002649 RID: 9801
		private bool _authenticated;
	}
}
