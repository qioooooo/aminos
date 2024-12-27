using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x02000162 RID: 354
	[ComVisible(true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class ComponentRenameEventArgs : EventArgs
	{
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x000281A4 File Offset: 0x000271A4
		public object Component
		{
			get
			{
				return this.component;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x000281AC File Offset: 0x000271AC
		public virtual string OldName
		{
			get
			{
				return this.oldName;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x000281B4 File Offset: 0x000271B4
		public virtual string NewName
		{
			get
			{
				return this.newName;
			}
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x000281BC File Offset: 0x000271BC
		public ComponentRenameEventArgs(object component, string oldName, string newName)
		{
			this.oldName = oldName;
			this.newName = newName;
			this.component = component;
		}

		// Token: 0x04000AAB RID: 2731
		private object component;

		// Token: 0x04000AAC RID: 2732
		private string oldName;

		// Token: 0x04000AAD RID: 2733
		private string newName;
	}
}
