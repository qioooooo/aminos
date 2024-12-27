using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000759 RID: 1881
	[Serializable]
	public class EventLogPermissionEntryCollection : CollectionBase
	{
		// Token: 0x06003991 RID: 14737 RVA: 0x000F44B0 File Offset: 0x000F34B0
		internal EventLogPermissionEntryCollection(EventLogPermission owner, ResourcePermissionBaseEntry[] entries)
		{
			this.owner = owner;
			for (int i = 0; i < entries.Length; i++)
			{
				base.InnerList.Add(new EventLogPermissionEntry(entries[i]));
			}
		}

		// Token: 0x17000D5A RID: 3418
		public EventLogPermissionEntry this[int index]
		{
			get
			{
				return (EventLogPermissionEntry)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06003994 RID: 14740 RVA: 0x000F450E File Offset: 0x000F350E
		public int Add(EventLogPermissionEntry value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06003995 RID: 14741 RVA: 0x000F451C File Offset: 0x000F351C
		public void AddRange(EventLogPermissionEntry[] value)
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

		// Token: 0x06003996 RID: 14742 RVA: 0x000F4550 File Offset: 0x000F3550
		public void AddRange(EventLogPermissionEntryCollection value)
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

		// Token: 0x06003997 RID: 14743 RVA: 0x000F458C File Offset: 0x000F358C
		public bool Contains(EventLogPermissionEntry value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06003998 RID: 14744 RVA: 0x000F459A File Offset: 0x000F359A
		public void CopyTo(EventLogPermissionEntry[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x000F45A9 File Offset: 0x000F35A9
		public int IndexOf(EventLogPermissionEntry value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x000F45B7 File Offset: 0x000F35B7
		public void Insert(int index, EventLogPermissionEntry value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x000F45C6 File Offset: 0x000F35C6
		public void Remove(EventLogPermissionEntry value)
		{
			base.List.Remove(value);
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x000F45D4 File Offset: 0x000F35D4
		protected override void OnClear()
		{
			this.owner.Clear();
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x000F45E1 File Offset: 0x000F35E1
		protected override void OnInsert(int index, object value)
		{
			this.owner.AddPermissionAccess((EventLogPermissionEntry)value);
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x000F45F4 File Offset: 0x000F35F4
		protected override void OnRemove(int index, object value)
		{
			this.owner.RemovePermissionAccess((EventLogPermissionEntry)value);
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x000F4607 File Offset: 0x000F3607
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			this.owner.RemovePermissionAccess((EventLogPermissionEntry)oldValue);
			this.owner.AddPermissionAccess((EventLogPermissionEntry)newValue);
		}

		// Token: 0x040032C3 RID: 12995
		private EventLogPermission owner;
	}
}
