using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000755 RID: 1877
	[Serializable]
	public sealed class EventLogPermission : ResourcePermissionBase
	{
		// Token: 0x0600397D RID: 14717 RVA: 0x000F427B File Offset: 0x000F327B
		public EventLogPermission()
		{
			this.SetNames();
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x000F4289 File Offset: 0x000F3289
		public EventLogPermission(PermissionState state)
			: base(state)
		{
			this.SetNames();
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x000F4298 File Offset: 0x000F3298
		public EventLogPermission(EventLogPermissionAccess permissionAccess, string machineName)
		{
			this.SetNames();
			this.AddPermissionAccess(new EventLogPermissionEntry(permissionAccess, machineName));
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x000F42B4 File Offset: 0x000F32B4
		public EventLogPermission(EventLogPermissionEntry[] permissionAccessEntries)
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

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06003981 RID: 14721 RVA: 0x000F42F2 File Offset: 0x000F32F2
		public EventLogPermissionEntryCollection PermissionEntries
		{
			get
			{
				if (this.innerCollection == null)
				{
					this.innerCollection = new EventLogPermissionEntryCollection(this, base.GetPermissionEntries());
				}
				return this.innerCollection;
			}
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x000F4314 File Offset: 0x000F3314
		internal void AddPermissionAccess(EventLogPermissionEntry entry)
		{
			base.AddPermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x06003983 RID: 14723 RVA: 0x000F4322 File Offset: 0x000F3322
		internal new void Clear()
		{
			base.Clear();
		}

		// Token: 0x06003984 RID: 14724 RVA: 0x000F432A File Offset: 0x000F332A
		internal void RemovePermissionAccess(EventLogPermissionEntry entry)
		{
			base.RemovePermissionAccess(entry.GetBaseEntry());
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x000F4338 File Offset: 0x000F3338
		private void SetNames()
		{
			base.PermissionAccessType = typeof(EventLogPermissionAccess);
			base.TagNames = new string[] { "Machine" };
		}

		// Token: 0x040032B7 RID: 12983
		private EventLogPermissionEntryCollection innerCollection;
	}
}
