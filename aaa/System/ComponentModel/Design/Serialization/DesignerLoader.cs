using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001A5 RID: 421
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class DesignerLoader
	{
		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x0002A56C File Offset: 0x0002956C
		public virtual bool Loading
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000D07 RID: 3335
		public abstract void BeginLoad(IDesignerLoaderHost host);

		// Token: 0x06000D08 RID: 3336
		public abstract void Dispose();

		// Token: 0x06000D09 RID: 3337 RVA: 0x0002A56F File Offset: 0x0002956F
		public virtual void Flush()
		{
		}
	}
}
