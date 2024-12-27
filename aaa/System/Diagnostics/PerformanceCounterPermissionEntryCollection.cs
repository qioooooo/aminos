using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000772 RID: 1906
	[Serializable]
	public class PerformanceCounterPermissionEntryCollection : CollectionBase
	{
		// Token: 0x06003A9C RID: 15004 RVA: 0x000F9534 File Offset: 0x000F8534
		internal PerformanceCounterPermissionEntryCollection(PerformanceCounterPermission owner, ResourcePermissionBaseEntry[] entries)
		{
			this.owner = owner;
			for (int i = 0; i < entries.Length; i++)
			{
				base.InnerList.Add(new PerformanceCounterPermissionEntry(entries[i]));
			}
		}

		// Token: 0x17000DA5 RID: 3493
		public PerformanceCounterPermissionEntry this[int index]
		{
			get
			{
				return (PerformanceCounterPermissionEntry)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06003A9F RID: 15007 RVA: 0x000F9592 File Offset: 0x000F8592
		public int Add(PerformanceCounterPermissionEntry value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x000F95A0 File Offset: 0x000F85A0
		public void AddRange(PerformanceCounterPermissionEntry[] value)
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

		// Token: 0x06003AA1 RID: 15009 RVA: 0x000F95D4 File Offset: 0x000F85D4
		public void AddRange(PerformanceCounterPermissionEntryCollection value)
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

		// Token: 0x06003AA2 RID: 15010 RVA: 0x000F9610 File Offset: 0x000F8610
		public bool Contains(PerformanceCounterPermissionEntry value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x000F961E File Offset: 0x000F861E
		public void CopyTo(PerformanceCounterPermissionEntry[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x000F962D File Offset: 0x000F862D
		public int IndexOf(PerformanceCounterPermissionEntry value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x000F963B File Offset: 0x000F863B
		public void Insert(int index, PerformanceCounterPermissionEntry value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06003AA6 RID: 15014 RVA: 0x000F964A File Offset: 0x000F864A
		public void Remove(PerformanceCounterPermissionEntry value)
		{
			base.List.Remove(value);
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x000F9658 File Offset: 0x000F8658
		protected override void OnClear()
		{
			this.owner.Clear();
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x000F9665 File Offset: 0x000F8665
		protected override void OnInsert(int index, object value)
		{
			this.owner.AddPermissionAccess((PerformanceCounterPermissionEntry)value);
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x000F9678 File Offset: 0x000F8678
		protected override void OnRemove(int index, object value)
		{
			this.owner.RemovePermissionAccess((PerformanceCounterPermissionEntry)value);
		}

		// Token: 0x06003AAA RID: 15018 RVA: 0x000F968B File Offset: 0x000F868B
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			this.owner.RemovePermissionAccess((PerformanceCounterPermissionEntry)oldValue);
			this.owner.AddPermissionAccess((PerformanceCounterPermissionEntry)newValue);
		}

		// Token: 0x04003351 RID: 13137
		private PerformanceCounterPermission owner;
	}
}
