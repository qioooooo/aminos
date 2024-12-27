using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000758 RID: 1880
	[Serializable]
	public class EventLogPermissionEntry
	{
		// Token: 0x0600398C RID: 14732 RVA: 0x000F4404 File Offset: 0x000F3404
		public EventLogPermissionEntry(EventLogPermissionAccess permissionAccess, string machineName)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "MachineName", machineName }));
			}
			this.permissionAccess = permissionAccess;
			this.machineName = machineName;
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x000F4451 File Offset: 0x000F3451
		internal EventLogPermissionEntry(ResourcePermissionBaseEntry baseEntry)
		{
			this.permissionAccess = (EventLogPermissionAccess)baseEntry.PermissionAccess;
			this.machineName = baseEntry.PermissionAccessPath[0];
		}

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x0600398E RID: 14734 RVA: 0x000F4473 File Offset: 0x000F3473
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
		}

		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x0600398F RID: 14735 RVA: 0x000F447B File Offset: 0x000F347B
		public EventLogPermissionAccess PermissionAccess
		{
			get
			{
				return this.permissionAccess;
			}
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x000F4484 File Offset: 0x000F3484
		internal ResourcePermissionBaseEntry GetBaseEntry()
		{
			return new ResourcePermissionBaseEntry((int)this.PermissionAccess, new string[] { this.MachineName });
		}

		// Token: 0x040032C1 RID: 12993
		private string machineName;

		// Token: 0x040032C2 RID: 12994
		private EventLogPermissionAccess permissionAccess;
	}
}
