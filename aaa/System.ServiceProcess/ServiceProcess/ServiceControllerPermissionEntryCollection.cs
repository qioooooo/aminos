using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ServiceProcess
{
	// Token: 0x0200002C RID: 44
	[Serializable]
	public class ServiceControllerPermissionEntryCollection : CollectionBase
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x00004F7C File Offset: 0x00003F7C
		internal ServiceControllerPermissionEntryCollection(ServiceControllerPermission owner, ResourcePermissionBaseEntry[] entries)
		{
			this.owner = owner;
			for (int i = 0; i < entries.Length; i++)
			{
				base.InnerList.Add(new ServiceControllerPermissionEntry(entries[i]));
			}
		}

		// Token: 0x17000023 RID: 35
		public ServiceControllerPermissionEntry this[int index]
		{
			get
			{
				return (ServiceControllerPermissionEntry)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004FDA File Offset: 0x00003FDA
		public int Add(ServiceControllerPermissionEntry value)
		{
			return base.List.Add(value);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004FE8 File Offset: 0x00003FE8
		public void AddRange(ServiceControllerPermissionEntry[] value)
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

		// Token: 0x060000CD RID: 205 RVA: 0x0000501C File Offset: 0x0000401C
		public void AddRange(ServiceControllerPermissionEntryCollection value)
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

		// Token: 0x060000CE RID: 206 RVA: 0x00005058 File Offset: 0x00004058
		public bool Contains(ServiceControllerPermissionEntry value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005066 File Offset: 0x00004066
		public void CopyTo(ServiceControllerPermissionEntry[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005075 File Offset: 0x00004075
		public int IndexOf(ServiceControllerPermissionEntry value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005083 File Offset: 0x00004083
		public void Insert(int index, ServiceControllerPermissionEntry value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005092 File Offset: 0x00004092
		public void Remove(ServiceControllerPermissionEntry value)
		{
			base.List.Remove(value);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000050A0 File Offset: 0x000040A0
		protected override void OnClear()
		{
			this.owner.Clear();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000050AD File Offset: 0x000040AD
		protected override void OnInsert(int index, object value)
		{
			this.owner.AddPermissionAccess((ServiceControllerPermissionEntry)value);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000050C0 File Offset: 0x000040C0
		protected override void OnRemove(int index, object value)
		{
			this.owner.RemovePermissionAccess((ServiceControllerPermissionEntry)value);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000050D3 File Offset: 0x000040D3
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			this.owner.RemovePermissionAccess((ServiceControllerPermissionEntry)oldValue);
			this.owner.AddPermissionAccess((ServiceControllerPermissionEntry)newValue);
		}

		// Token: 0x0400020D RID: 525
		private ServiceControllerPermission owner;
	}
}
