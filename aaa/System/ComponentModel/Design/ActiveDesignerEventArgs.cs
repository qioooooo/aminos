using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x02000158 RID: 344
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class ActiveDesignerEventArgs : EventArgs
	{
		// Token: 0x06000B4E RID: 2894 RVA: 0x00027FDE File Offset: 0x00026FDE
		public ActiveDesignerEventArgs(IDesignerHost oldDesigner, IDesignerHost newDesigner)
		{
			this.oldDesigner = oldDesigner;
			this.newDesigner = newDesigner;
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x00027FF4 File Offset: 0x00026FF4
		public IDesignerHost OldDesigner
		{
			get
			{
				return this.oldDesigner;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x00027FFC File Offset: 0x00026FFC
		public IDesignerHost NewDesigner
		{
			get
			{
				return this.newDesigner;
			}
		}

		// Token: 0x04000A9F RID: 2719
		private readonly IDesignerHost oldDesigner;

		// Token: 0x04000AA0 RID: 2720
		private readonly IDesignerHost newDesigner;
	}
}
