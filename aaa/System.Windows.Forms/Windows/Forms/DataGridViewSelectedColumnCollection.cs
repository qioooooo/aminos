using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000396 RID: 918
	[ListBindable(false)]
	public class DataGridViewSelectedColumnCollection : BaseCollection, IList, ICollection, IEnumerable
	{
		// Token: 0x06003810 RID: 14352 RVA: 0x000CCB49 File Offset: 0x000CBB49
		int IList.Add(object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x06003811 RID: 14353 RVA: 0x000CCB5A File Offset: 0x000CBB5A
		void IList.Clear()
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x06003812 RID: 14354 RVA: 0x000CCB6B File Offset: 0x000CBB6B
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		// Token: 0x06003813 RID: 14355 RVA: 0x000CCB79 File Offset: 0x000CBB79
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x000CCB87 File Offset: 0x000CBB87
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x06003815 RID: 14357 RVA: 0x000CCB98 File Offset: 0x000CBB98
		void IList.Remove(object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x06003816 RID: 14358 RVA: 0x000CCBA9 File Offset: 0x000CBBA9
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06003817 RID: 14359 RVA: 0x000CCBBA File Offset: 0x000CBBBA
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06003818 RID: 14360 RVA: 0x000CCBBD File Offset: 0x000CBBBD
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A7A RID: 2682
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

		// Token: 0x0600381B RID: 14363 RVA: 0x000CCBDF File Offset: 0x000CBBDF
		void ICollection.CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x0600381C RID: 14364 RVA: 0x000CCBEE File Offset: 0x000CBBEE
		int ICollection.Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x0600381D RID: 14365 RVA: 0x000CCBFB File Offset: 0x000CBBFB
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x0600381E RID: 14366 RVA: 0x000CCBFE File Offset: 0x000CBBFE
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x000CCC01 File Offset: 0x000CBC01
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06003820 RID: 14368 RVA: 0x000CCC0E File Offset: 0x000CBC0E
		internal DataGridViewSelectedColumnCollection()
		{
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06003821 RID: 14369 RVA: 0x000CCC21 File Offset: 0x000CBC21
		protected override ArrayList List
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x17000A7F RID: 2687
		public DataGridViewColumn this[int index]
		{
			get
			{
				return (DataGridViewColumn)this.items[index];
			}
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x000CCC3C File Offset: 0x000CBC3C
		internal int Add(DataGridViewColumn dataGridViewColumn)
		{
			return this.items.Add(dataGridViewColumn);
		}

		// Token: 0x06003824 RID: 14372 RVA: 0x000CCC4A File Offset: 0x000CBC4A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Clear()
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x06003825 RID: 14373 RVA: 0x000CCC5B File Offset: 0x000CBC5B
		public bool Contains(DataGridViewColumn dataGridViewColumn)
		{
			return this.items.IndexOf(dataGridViewColumn) != -1;
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x000CCC6F File Offset: 0x000CBC6F
		public void CopyTo(DataGridViewColumn[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x000CCC7E File Offset: 0x000CBC7E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Insert(int index, DataGridViewColumn dataGridViewColumn)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x04001C53 RID: 7251
		private ArrayList items = new ArrayList();
	}
}
