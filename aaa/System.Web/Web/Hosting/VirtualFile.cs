using System;
using System.IO;
using System.Security.Permissions;

namespace System.Web.Hosting
{
	// Token: 0x0200013A RID: 314
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class VirtualFile : VirtualFileBase
	{
		// Token: 0x06000EEC RID: 3820 RVA: 0x000436B8 File Offset: 0x000426B8
		protected VirtualFile(string virtualPath)
		{
			this._virtualPath = global::System.Web.VirtualPath.Create(virtualPath);
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000EED RID: 3821 RVA: 0x000436CC File Offset: 0x000426CC
		public override bool IsDirectory
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000EEE RID: 3822
		public abstract Stream Open();
	}
}
