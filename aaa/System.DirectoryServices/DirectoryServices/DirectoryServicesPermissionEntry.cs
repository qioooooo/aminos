using System;
using System.Security.Permissions;

namespace System.DirectoryServices
{
	// Token: 0x02000026 RID: 38
	[Serializable]
	public class DirectoryServicesPermissionEntry
	{
		// Token: 0x0600010A RID: 266 RVA: 0x00005A91 File Offset: 0x00004A91
		public DirectoryServicesPermissionEntry(DirectoryServicesPermissionAccess permissionAccess, string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			this.permissionAccess = permissionAccess;
			this.path = path;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005AB5 File Offset: 0x00004AB5
		internal DirectoryServicesPermissionEntry(ResourcePermissionBaseEntry baseEntry)
		{
			this.permissionAccess = (DirectoryServicesPermissionAccess)baseEntry.PermissionAccess;
			this.path = baseEntry.PermissionAccessPath[0];
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00005AD7 File Offset: 0x00004AD7
		public string Path
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00005ADF File Offset: 0x00004ADF
		public DirectoryServicesPermissionAccess PermissionAccess
		{
			get
			{
				return this.permissionAccess;
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005AE8 File Offset: 0x00004AE8
		internal ResourcePermissionBaseEntry GetBaseEntry()
		{
			return new ResourcePermissionBaseEntry((int)this.PermissionAccess, new string[] { this.Path });
		}

		// Token: 0x040001A8 RID: 424
		private string path;

		// Token: 0x040001A9 RID: 425
		private DirectoryServicesPermissionAccess permissionAccess;
	}
}
