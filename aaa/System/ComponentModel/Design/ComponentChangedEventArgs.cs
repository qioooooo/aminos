using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x0200015C RID: 348
	[ComVisible(true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public sealed class ComponentChangedEventArgs : EventArgs
	{
		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x00028122 File Offset: 0x00027122
		public object Component
		{
			get
			{
				return this.component;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000B62 RID: 2914 RVA: 0x0002812A File Offset: 0x0002712A
		public MemberDescriptor Member
		{
			get
			{
				return this.member;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x00028132 File Offset: 0x00027132
		public object NewValue
		{
			get
			{
				return this.newValue;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x0002813A File Offset: 0x0002713A
		public object OldValue
		{
			get
			{
				return this.oldValue;
			}
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00028142 File Offset: 0x00027142
		public ComponentChangedEventArgs(object component, MemberDescriptor member, object oldValue, object newValue)
		{
			this.component = component;
			this.member = member;
			this.oldValue = oldValue;
			this.newValue = newValue;
		}

		// Token: 0x04000AA4 RID: 2724
		private object component;

		// Token: 0x04000AA5 RID: 2725
		private MemberDescriptor member;

		// Token: 0x04000AA6 RID: 2726
		private object oldValue;

		// Token: 0x04000AA7 RID: 2727
		private object newValue;
	}
}
