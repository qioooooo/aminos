using System;
using System.Security.Permissions;

namespace System.ServiceProcess
{
	// Token: 0x02000028 RID: 40
	[Serializable]
	public sealed class ServiceControllerPermission : ResourcePermissionBase
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00004C4A File Offset: 0x00003C4A
		public ServiceControllerPermission()
		{
			this.SetNames();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004C58 File Offset: 0x00003C58
		public ServiceControllerPermission(PermissionState state)
			: base(state)
		{
			this.SetNames();
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004C67 File Offset: 0x00003C67
		public ServiceControllerPermission(ServiceControllerPermissionAccess permissionAccess, string machineName, string serviceName)
		{
			this.SetNames();
			this.AddPermissionAccess(new ServiceControllerPermissionEntry(permissionAccess, machineName, serviceName));
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004C84 File Offset: 0x00003C84
		public ServiceControllerPermission(ServiceControllerPermissionEntry[] permissionAccessEntries)
		{
			if (permissionAccessEntries == null)
			{
				throw new ArgumentNullException("permissionAccessEntries");
			}
			this.SetNames();
			for (int i = 0; i < permissionAccessEntries.Length; i++)
			{
				this.AddPermissionAccess(permissionAccessEntries[i]);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00004CC2 File Offset: 0x00003CC2
		public ServiceControllerPermissionEntryCollection PermissionEntries
		{
			get
			{
				if (this.innerCollection == null)
				{
					this.innerCollection = new ServiceControllerPermissionEntryCollection(this, base.GetPermissionEntries());
				}
				return this.innerCollection;
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004CE4 File Offset: 0x00003CE4
		internal void AddPermissionAccess(ServiceControllerPermissionEntry entry)
		{
			base.AddPermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004CF2 File Offset: 0x00003CF2
		internal new void Clear()
		{
			base.Clear();
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004CFA File Offset: 0x00003CFA
		internal void RemovePermissionAccess(ServiceControllerPermissionEntry entry)
		{
			base.RemovePermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004D08 File Offset: 0x00003D08
		private void SetNames()
		{
			base.PermissionAccessType = typeof(ServiceControllerPermissionAccess);
			base.TagNames = new string[] { "Machine", "Service" };
		}

		// Token: 0x04000202 RID: 514
		private ServiceControllerPermissionEntryCollection innerCollection;
	}
}
