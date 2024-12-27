using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x0200015B RID: 347
	[ComVisible(true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class CommandID
	{
		// Token: 0x06000B5B RID: 2907 RVA: 0x0002804E File Offset: 0x0002704E
		public CommandID(Guid menuGroup, int commandID)
		{
			this.menuGroup = menuGroup;
			this.commandID = commandID;
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x00028064 File Offset: 0x00027064
		public virtual int ID
		{
			get
			{
				return this.commandID;
			}
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x0002806C File Offset: 0x0002706C
		public override bool Equals(object obj)
		{
			if (!(obj is CommandID))
			{
				return false;
			}
			CommandID commandID = (CommandID)obj;
			return commandID.menuGroup.Equals(this.menuGroup) && commandID.commandID == this.commandID;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x000280B0 File Offset: 0x000270B0
		public override int GetHashCode()
		{
			return (this.menuGroup.GetHashCode() << 2) | this.commandID;
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x000280DA File Offset: 0x000270DA
		public virtual Guid Guid
		{
			get
			{
				return this.menuGroup;
			}
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x000280E4 File Offset: 0x000270E4
		public override string ToString()
		{
			return this.menuGroup.ToString() + " : " + this.commandID.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x04000AA2 RID: 2722
		private readonly Guid menuGroup;

		// Token: 0x04000AA3 RID: 2723
		private readonly int commandID;
	}
}
