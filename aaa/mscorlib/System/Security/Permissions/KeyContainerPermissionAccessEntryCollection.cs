using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000649 RID: 1609
	[ComVisible(true)]
	[Serializable]
	public sealed class KeyContainerPermissionAccessEntryCollection : ICollection, IEnumerable
	{
		// Token: 0x06003A83 RID: 14979 RVA: 0x000C66C5 File Offset: 0x000C56C5
		private KeyContainerPermissionAccessEntryCollection()
		{
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x000C66CD File Offset: 0x000C56CD
		internal KeyContainerPermissionAccessEntryCollection(KeyContainerPermissionFlags globalFlags)
		{
			this.m_list = new ArrayList();
			this.m_globalFlags = globalFlags;
		}

		// Token: 0x170009E9 RID: 2537
		public KeyContainerPermissionAccessEntry this[int index]
		{
			get
			{
				if (index < 0)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
				}
				if (index >= this.m_list.Count)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				return (KeyContainerPermissionAccessEntry)this.m_list[index];
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06003A86 RID: 14982 RVA: 0x000C673D File Offset: 0x000C573D
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x000C674C File Offset: 0x000C574C
		public int Add(KeyContainerPermissionAccessEntry accessEntry)
		{
			if (accessEntry == null)
			{
				throw new ArgumentNullException("accessEntry");
			}
			int num = this.m_list.IndexOf(accessEntry);
			if (num != -1)
			{
				((KeyContainerPermissionAccessEntry)this.m_list[num]).Flags &= accessEntry.Flags;
				return num;
			}
			if (accessEntry.Flags != this.m_globalFlags)
			{
				return this.m_list.Add(accessEntry);
			}
			return -1;
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x000C67B9 File Offset: 0x000C57B9
		public void Clear()
		{
			this.m_list.Clear();
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x000C67C6 File Offset: 0x000C57C6
		public int IndexOf(KeyContainerPermissionAccessEntry accessEntry)
		{
			return this.m_list.IndexOf(accessEntry);
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x000C67D4 File Offset: 0x000C57D4
		public void Remove(KeyContainerPermissionAccessEntry accessEntry)
		{
			if (accessEntry == null)
			{
				throw new ArgumentNullException("accessEntry");
			}
			this.m_list.Remove(accessEntry);
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x000C67F0 File Offset: 0x000C57F0
		public KeyContainerPermissionAccessEntryEnumerator GetEnumerator()
		{
			return new KeyContainerPermissionAccessEntryEnumerator(this);
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x000C67F8 File Offset: 0x000C57F8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new KeyContainerPermissionAccessEntryEnumerator(this);
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x000C6800 File Offset: 0x000C5800
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
			}
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (index + this.Count > array.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], index);
				index++;
			}
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x000C689A File Offset: 0x000C589A
		public void CopyTo(KeyContainerPermissionAccessEntry[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06003A8F RID: 14991 RVA: 0x000C68A4 File Offset: 0x000C58A4
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06003A90 RID: 14992 RVA: 0x000C68A7 File Offset: 0x000C58A7
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x04001E2F RID: 7727
		private ArrayList m_list;

		// Token: 0x04001E30 RID: 7728
		private KeyContainerPermissionFlags m_globalFlags;
	}
}
