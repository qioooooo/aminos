using System;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	// Token: 0x020000FE RID: 254
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class ToolboxComponentsCreatingEventArgs : EventArgs
	{
		// Token: 0x06000DC2 RID: 3522 RVA: 0x000281F5 File Offset: 0x000271F5
		public ToolboxComponentsCreatingEventArgs(IDesignerHost host)
		{
			this.host = host;
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x00028204 File Offset: 0x00027204
		public IDesignerHost DesignerHost
		{
			get
			{
				return this.host;
			}
		}

		// Token: 0x04000B6E RID: 2926
		private readonly IDesignerHost host;
	}
}
