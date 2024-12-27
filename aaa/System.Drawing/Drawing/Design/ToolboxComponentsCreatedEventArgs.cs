using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	// Token: 0x020000FC RID: 252
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class ToolboxComponentsCreatedEventArgs : EventArgs
	{
		// Token: 0x06000DBC RID: 3516 RVA: 0x000281D4 File Offset: 0x000271D4
		public ToolboxComponentsCreatedEventArgs(IComponent[] components)
		{
			this.comps = components;
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x000281E3 File Offset: 0x000271E3
		public IComponent[] Components
		{
			get
			{
				return (IComponent[])this.comps.Clone();
			}
		}

		// Token: 0x04000B6D RID: 2925
		private readonly IComponent[] comps;
	}
}
