using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001B2 RID: 434
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ResolveNameEventArgs : EventArgs
	{
		// Token: 0x06000D4B RID: 3403 RVA: 0x0002AC65 File Offset: 0x00029C65
		public ResolveNameEventArgs(string name)
		{
			this.name = name;
			this.value = null;
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000D4C RID: 3404 RVA: 0x0002AC7B File Offset: 0x00029C7B
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000D4D RID: 3405 RVA: 0x0002AC83 File Offset: 0x00029C83
		// (set) Token: 0x06000D4E RID: 3406 RVA: 0x0002AC8B File Offset: 0x00029C8B
		public object Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x04000EB7 RID: 3767
		private string name;

		// Token: 0x04000EB8 RID: 3768
		private object value;
	}
}
