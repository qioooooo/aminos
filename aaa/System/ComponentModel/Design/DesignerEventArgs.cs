using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x02000174 RID: 372
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class DesignerEventArgs : EventArgs
	{
		// Token: 0x06000C0F RID: 3087 RVA: 0x0002936A File Offset: 0x0002836A
		public DesignerEventArgs(IDesignerHost host)
		{
			this.host = host;
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000C10 RID: 3088 RVA: 0x00029379 File Offset: 0x00028379
		public IDesignerHost Designer
		{
			get
			{
				return this.host;
			}
		}

		// Token: 0x04000ACC RID: 2764
		private readonly IDesignerHost host;
	}
}
