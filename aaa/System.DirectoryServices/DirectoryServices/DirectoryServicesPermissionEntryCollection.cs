using System;
using System.Collections;
using System.Security.Permissions;

namespace System.DirectoryServices
{
	// Token: 0x02000027 RID: 39
	[Serializable]
	public class DirectoryServicesPermissionEntryCollection : CollectionBase
	{
		// Token: 0x0600010F RID: 271 RVA: 0x00005B14 File Offset: 0x00004B14
		internal DirectoryServicesPermissionEntryCollection(DirectoryServicesPermission owner, ResourcePermissionBaseEntry[] entries)
		{
			this.owner = owner;
			for (int i = 0; i < entries.Length; i++)
			{
				base.InnerList.Add(new DirectoryServicesPermissionEntry(entries[i]));
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005B50 File Offset: 0x00004B50
		internal DirectoryServicesPermissionEntryCollection()
		{
		}

		// Token: 0x17000045 RID: 69
		public DirectoryServicesPermissionEntry this[int index]
		{
			get
			{
				return (DirectoryServicesPermissionEntry)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005B7A File Offset: 0x00004B7A
		public int Add(DirectoryServicesPermissionEntry value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005B88 File Offset: 0x00004B88
		public void AddRange(DirectoryServicesPermissionEntry[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005BBC File Offset: 0x00004BBC
		public void AddRange(DirectoryServicesPermissionEntryCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int count = value.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005BF8 File Offset: 0x00004BF8
		public bool Contains(DirectoryServicesPermissionEntry value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005C06 File Offset: 0x00004C06
		public void CopyTo(DirectoryServicesPermissionEntry[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005C15 File Offset: 0x00004C15
		public int IndexOf(DirectoryServicesPermissionEntry value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005C23 File Offset: 0x00004C23
		public void Insert(int index, DirectoryServicesPermissionEntry value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005C32 File Offset: 0x00004C32
		public void Remove(DirectoryServicesPermissionEntry value)
		{
			base.List.Remove(value);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005C40 File Offset: 0x00004C40
		protected override void OnClear()
		{
			this.owner.Clear();
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005C4D File Offset: 0x00004C4D
		protected override void OnInsert(int index, object value)
		{
			this.owner.AddPermissionAccess((DirectoryServicesPermissionEntry)value);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005C60 File Offset: 0x00004C60
		protected override void OnRemove(int index, object value)
		{
			this.owner.RemovePermissionAccess((DirectoryServicesPermissionEntry)value);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005C73 File Offset: 0x00004C73
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			this.owner.RemovePermissionAccess((DirectoryServicesPermissionEntry)oldValue);
			this.owner.AddPermissionAccess((DirectoryServicesPermissionEntry)newValue);
		}

		// Token: 0x040001AA RID: 426
		private DirectoryServicesPermission owner;
	}
}
