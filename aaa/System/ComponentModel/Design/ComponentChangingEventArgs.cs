using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x0200015E RID: 350
	[ComVisible(true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public sealed class ComponentChangingEventArgs : EventArgs
	{
		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x00028167 File Offset: 0x00027167
		public object Component
		{
			get
			{
				return this.component;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x0002816F File Offset: 0x0002716F
		public MemberDescriptor Member
		{
			get
			{
				return this.member;
			}
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00028177 File Offset: 0x00027177
		public ComponentChangingEventArgs(object component, MemberDescriptor member)
		{
			this.component = component;
			this.member = member;
		}

		// Token: 0x04000AA8 RID: 2728
		private object component;

		// Token: 0x04000AA9 RID: 2729
		private MemberDescriptor member;
	}
}
