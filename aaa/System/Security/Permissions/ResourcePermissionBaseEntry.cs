using System;

namespace System.Security.Permissions
{
	// Token: 0x0200073E RID: 1854
	[Serializable]
	public class ResourcePermissionBaseEntry
	{
		// Token: 0x06003886 RID: 14470 RVA: 0x000EEC63 File Offset: 0x000EDC63
		public ResourcePermissionBaseEntry()
		{
			this.permissionAccess = 0;
			this.accessPath = new string[0];
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x000EEC7E File Offset: 0x000EDC7E
		public ResourcePermissionBaseEntry(int permissionAccess, string[] permissionAccessPath)
		{
			if (permissionAccessPath == null)
			{
				throw new ArgumentNullException("permissionAccessPath");
			}
			this.permissionAccess = permissionAccess;
			this.accessPath = permissionAccessPath;
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x06003888 RID: 14472 RVA: 0x000EECA2 File Offset: 0x000EDCA2
		public int PermissionAccess
		{
			get
			{
				return this.permissionAccess;
			}
		}

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x06003889 RID: 14473 RVA: 0x000EECAA File Offset: 0x000EDCAA
		public string[] PermissionAccessPath
		{
			get
			{
				return this.accessPath;
			}
		}

		// Token: 0x04003249 RID: 12873
		private string[] accessPath;

		// Token: 0x0400324A RID: 12874
		private int permissionAccess;
	}
}
