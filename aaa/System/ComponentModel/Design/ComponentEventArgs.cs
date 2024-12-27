using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x02000160 RID: 352
	[ComVisible(true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ComponentEventArgs : EventArgs
	{
		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000B71 RID: 2929 RVA: 0x0002818D File Offset: 0x0002718D
		public virtual IComponent Component
		{
			get
			{
				return this.component;
			}
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00028195 File Offset: 0x00027195
		public ComponentEventArgs(IComponent component)
		{
			this.component = component;
		}

		// Token: 0x04000AAA RID: 2730
		private IComponent component;
	}
}
