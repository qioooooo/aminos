using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000395 RID: 917
	[ListBindable(false)]
	public class DataGridViewSelectedCellCollection : BaseCollection, IList, ICollection, IEnumerable
	{
		// Token: 0x060037F7 RID: 14327 RVA: 0x000CC9A5 File Offset: 0x000CB9A5
		int IList.Add(object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x060037F8 RID: 14328 RVA: 0x000CC9B6 File Offset: 0x000CB9B6
		void IList.Clear()
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x060037F9 RID: 14329 RVA: 0x000CC9C7 File Offset: 0x000CB9C7
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		// Token: 0x060037FA RID: 14330 RVA: 0x000CC9D5 File Offset: 0x000CB9D5
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x000CC9E3 File Offset: 0x000CB9E3
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x060037FC RID: 14332 RVA: 0x000CC9F4 File Offset: 0x000CB9F4
		void IList.Remove(object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x060037FD RID: 14333 RVA: 0x000CCA05 File Offset: 0x000CBA05
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x060037FE RID: 14334 RVA: 0x000CCA16 File Offset: 0x000CBA16
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x060037FF RID: 14335 RVA: 0x000CCA19 File Offset: 0x000CBA19
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A72 RID: 2674
		object IList.this[int index]
		{
			get
			{
				return this.items[index];
			}
			set
			{
				throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
			}
		}

		// Token: 0x06003802 RID: 14338 RVA: 0x000CCA3B File Offset: 0x000CBA3B
		void ICollection.CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06003803 RID: 14339 RVA: 0x000CCA4A File Offset: 0x000CBA4A
		int ICollection.Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06003804 RID: 14340 RVA: 0x000CCA57 File Offset: 0x000CBA57
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06003805 RID: 14341 RVA: 0x000CCA5A File Offset: 0x000CBA5A
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x000CCA5D File Offset: 0x000CBA5D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x000CCA6A File Offset: 0x000CBA6A
		internal DataGridViewSelectedCellCollection()
		{
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06003808 RID: 14344 RVA: 0x000CCA7D File Offset: 0x000CBA7D
		protected override ArrayList List
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x17000A77 RID: 2679
		public DataGridViewCell this[int index]
		{
			get
			{
				return (DataGridViewCell)this.items[index];
			}
		}

		// Token: 0x0600380A RID: 14346 RVA: 0x000CCA98 File Offset: 0x000CBA98
		internal int Add(DataGridViewCell dataGridViewCell)
		{
			return this.items.Add(dataGridViewCell);
		}

		// Token: 0x0600380B RID: 14347 RVA: 0x000CCAA8 File Offset: 0x000CBAA8
		internal void AddCellLinkedList(DataGridViewCellLinkedList dataGridViewCells)
		{
			foreach (object obj in ((IEnumerable)dataGridViewCells))
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				this.items.Add(dataGridViewCell);
			}
		}

		// Token: 0x0600380C RID: 14348 RVA: 0x000CCB04 File Offset: 0x000CBB04
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Clear()
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x000CCB15 File Offset: 0x000CBB15
		public bool Contains(DataGridViewCell dataGridViewCell)
		{
			return this.items.IndexOf(dataGridViewCell) != -1;
		}

		// Token: 0x0600380E RID: 14350 RVA: 0x000CCB29 File Offset: 0x000CBB29
		public void CopyTo(DataGridViewCell[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x0600380F RID: 14351 RVA: 0x000CCB38 File Offset: 0x000CBB38
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Insert(int index, DataGridViewCell dataGridViewCell)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x04001C52 RID: 7250
		private ArrayList items = new ArrayList();
	}
}
