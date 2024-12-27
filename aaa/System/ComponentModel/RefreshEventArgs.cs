using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000135 RID: 309
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class RefreshEventArgs : EventArgs
	{
		// Token: 0x06000A2F RID: 2607 RVA: 0x00023CB6 File Offset: 0x00022CB6
		public RefreshEventArgs(object componentChanged)
		{
			this.componentChanged = componentChanged;
			this.typeChanged = componentChanged.GetType();
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00023CD1 File Offset: 0x00022CD1
		public RefreshEventArgs(Type typeChanged)
		{
			this.typeChanged = typeChanged;
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x00023CE0 File Offset: 0x00022CE0
		public object ComponentChanged
		{
			get
			{
				return this.componentChanged;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000A32 RID: 2610 RVA: 0x00023CE8 File Offset: 0x00022CE8
		public Type TypeChanged
		{
			get
			{
				return this.typeChanged;
			}
		}

		// Token: 0x04000A62 RID: 2658
		private object componentChanged;

		// Token: 0x04000A63 RID: 2659
		private Type typeChanged;
	}
}
