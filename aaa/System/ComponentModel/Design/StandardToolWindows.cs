using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x0200019F RID: 415
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class StandardToolWindows
	{
		// Token: 0x04000E98 RID: 3736
		public static readonly Guid ObjectBrowser = new Guid("{970d9861-ee83-11d0-a778-00a0c91110c3}");

		// Token: 0x04000E99 RID: 3737
		public static readonly Guid OutputWindow = new Guid("{34e76e81-ee4a-11d0-ae2e-00a0c90fffc3}");

		// Token: 0x04000E9A RID: 3738
		public static readonly Guid ProjectExplorer = new Guid("{3ae79031-e1bc-11d0-8f78-00a0c9110057}");

		// Token: 0x04000E9B RID: 3739
		public static readonly Guid PropertyBrowser = new Guid("{eefa5220-e298-11d0-8f78-00a0c9110057}");

		// Token: 0x04000E9C RID: 3740
		public static readonly Guid RelatedLinks = new Guid("{66dba47c-61df-11d2-aa79-00c04f990343}");

		// Token: 0x04000E9D RID: 3741
		public static readonly Guid ServerExplorer = new Guid("{74946827-37a0-11d2-a273-00c04f8ef4ff}");

		// Token: 0x04000E9E RID: 3742
		public static readonly Guid TaskList = new Guid("{4a9b7e51-aa16-11d0-a8c5-00a0c921a4d2}");

		// Token: 0x04000E9F RID: 3743
		public static readonly Guid Toolbox = new Guid("{b1e99781-ab81-11d0-b683-00aa00a3ee26}");
	}
}
