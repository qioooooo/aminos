using System;
using System.Security.Permissions;

namespace System.DirectoryServices
{
	// Token: 0x02000023 RID: 35
	[Serializable]
	public sealed class DirectoryServicesPermission : ResourcePermissionBase
	{
		// Token: 0x060000FB RID: 251 RVA: 0x00005923 File Offset: 0x00004923
		public DirectoryServicesPermission()
		{
			this.SetNames();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005931 File Offset: 0x00004931
		public DirectoryServicesPermission(PermissionState state)
			: base(state)
		{
			this.SetNames();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005940 File Offset: 0x00004940
		public DirectoryServicesPermission(DirectoryServicesPermissionAccess permissionAccess, string path)
		{
			this.SetNames();
			this.AddPermissionAccess(new DirectoryServicesPermissionEntry(permissionAccess, path));
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000595C File Offset: 0x0000495C
		public DirectoryServicesPermission(DirectoryServicesPermissionEntry[] permissionAccessEntries)
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000FF RID: 255 RVA: 0x0000599A File Offset: 0x0000499A
		public DirectoryServicesPermissionEntryCollection PermissionEntries
		{
			get
			{
				if (this.innerCollection == null)
				{
					this.innerCollection = new DirectoryServicesPermissionEntryCollection(this, base.GetPermissionEntries());
				}
				return this.innerCollection;
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000059BC File Offset: 0x000049BC
		internal void AddPermissionAccess(DirectoryServicesPermissionEntry entry)
		{
			base.AddPermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000059CA File Offset: 0x000049CA
		internal new void Clear()
		{
			base.Clear();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000059D2 File Offset: 0x000049D2
		internal void RemovePermissionAccess(DirectoryServicesPermissionEntry entry)
		{
			base.RemovePermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000059E0 File Offset: 0x000049E0
		private void SetNames()
		{
			base.PermissionAccessType = typeof(DirectoryServicesPermissionAccess);
			base.TagNames = new string[] { "Path" };
		}

		// Token: 0x040001A1 RID: 417
		private DirectoryServicesPermissionEntryCollection innerCollection;
	}
}
