using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x0200076E RID: 1902
	[Serializable]
	public sealed class PerformanceCounterPermission : ResourcePermissionBase
	{
		// Token: 0x06003A85 RID: 14981 RVA: 0x000F9257 File Offset: 0x000F8257
		public PerformanceCounterPermission()
		{
			this.SetNames();
		}

		// Token: 0x06003A86 RID: 14982 RVA: 0x000F9265 File Offset: 0x000F8265
		public PerformanceCounterPermission(PermissionState state)
			: base(state)
		{
			this.SetNames();
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x000F9274 File Offset: 0x000F8274
		public PerformanceCounterPermission(PerformanceCounterPermissionAccess permissionAccess, string machineName, string categoryName)
		{
			this.SetNames();
			this.AddPermissionAccess(new PerformanceCounterPermissionEntry(permissionAccess, machineName, categoryName));
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x000F9290 File Offset: 0x000F8290
		public PerformanceCounterPermission(PerformanceCounterPermissionEntry[] permissionAccessEntries)
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

		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x06003A89 RID: 14985 RVA: 0x000F92CE File Offset: 0x000F82CE
		public PerformanceCounterPermissionEntryCollection PermissionEntries
		{
			get
			{
				if (this.innerCollection == null)
				{
					this.innerCollection = new PerformanceCounterPermissionEntryCollection(this, base.GetPermissionEntries());
				}
				return this.innerCollection;
			}
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x000F92F0 File Offset: 0x000F82F0
		internal void AddPermissionAccess(PerformanceCounterPermissionEntry entry)
		{
			base.AddPermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x000F92FE File Offset: 0x000F82FE
		internal new void Clear()
		{
			base.Clear();
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x000F9306 File Offset: 0x000F8306
		internal void RemovePermissionAccess(PerformanceCounterPermissionEntry entry)
		{
			base.RemovePermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x000F9314 File Offset: 0x000F8314
		private void SetNames()
		{
			base.PermissionAccessType = typeof(PerformanceCounterPermissionAccess);
			base.TagNames = new string[] { "Machine", "Category" };
		}

		// Token: 0x04003343 RID: 13123
		private PerformanceCounterPermissionEntryCollection innerCollection;
	}
}
