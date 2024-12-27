using System;
using System.Security.Permissions;

namespace System.Web.Hosting
{
	// Token: 0x02000139 RID: 313
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class VirtualFileBase : MarshalByRefObject
	{
		// Token: 0x06000EE6 RID: 3814 RVA: 0x0004368B File Offset: 0x0004268B
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x0004368E File Offset: 0x0004268E
		public virtual string Name
		{
			get
			{
				return this._virtualPath.FileName;
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000EE8 RID: 3816 RVA: 0x0004369B File Offset: 0x0004269B
		public string VirtualPath
		{
			get
			{
				return this._virtualPath.VirtualPathString;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x000436A8 File Offset: 0x000426A8
		internal VirtualPath VirtualPathObject
		{
			get
			{
				return this._virtualPath;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000EEA RID: 3818
		public abstract bool IsDirectory { get; }

		// Token: 0x040015AA RID: 5546
		internal VirtualPath _virtualPath;
	}
}
