using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000397 RID: 919
	[ListBindable(false)]
	public class DataGridViewSelectedRowCollection : BaseCollection, IList, ICollection, IEnumerable
	{
		// Token: 0x06003828 RID: 14376 RVA: 0x000CCC8F File Offset: 0x000CBC8F
		int IList.Add(object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x06003829 RID: 14377 RVA: 0x000CCCA0 File Offset: 0x000CBCA0
		void IList.Clear()
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x0600382A RID: 14378 RVA: 0x000CCCB1 File Offset: 0x000CBCB1
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x000CCCBF File Offset: 0x000CBCBF
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		// Token: 0x0600382C RID: 14380 RVA: 0x000CCCCD File Offset: 0x000CBCCD
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x000CCCDE File Offset: 0x000CBCDE
		void IList.Remove(object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x000CCCEF File Offset: 0x000CBCEF
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x0600382F RID: 14383 RVA: 0x000CCD00 File Offset: 0x000CBD00
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06003830 RID: 14384 RVA: 0x000CCD03 File Offset: 0x000CBD03
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A82 RID: 2690
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

		// Token: 0x06003833 RID: 14387 RVA: 0x000CCD25 File Offset: 0x000CBD25
		void ICollection.CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06003834 RID: 14388 RVA: 0x000CCD34 File Offset: 0x000CBD34
		int ICollection.Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06003835 RID: 14389 RVA: 0x000CCD41 File Offset: 0x000CBD41
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06003836 RID: 14390 RVA: 0x000CCD44 File Offset: 0x000CBD44
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06003837 RID: 14391 RVA: 0x000CCD47 File Offset: 0x000CBD47
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x000CCD54 File Offset: 0x000CBD54
		internal DataGridViewSelectedRowCollection()
		{
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06003839 RID: 14393 RVA: 0x000CCD67 File Offset: 0x000CBD67
		protected override ArrayList List
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x17000A87 RID: 2695
		public DataGridViewRow this[int index]
		{
			get
			{
				return (DataGridViewRow)this.items[index];
			}
		}

		// Token: 0x0600383B RID: 14395 RVA: 0x000CCD82 File Offset: 0x000CBD82
		internal int Add(DataGridViewRow dataGridViewRow)
		{
			return this.items.Add(dataGridViewRow);
		}

		// Token: 0x0600383C RID: 14396 RVA: 0x000CCD90 File Offset: 0x000CBD90
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Clear()
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x000CCDA1 File Offset: 0x000CBDA1
		public bool Contains(DataGridViewRow dataGridViewRow)
		{
			return this.items.IndexOf(dataGridViewRow) != -1;
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x000CCDB5 File Offset: 0x000CBDB5
		public void CopyTo(DataGridViewRow[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x000CCDC4 File Offset: 0x000CBDC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Insert(int index, DataGridViewRow dataGridViewRow)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x04001C54 RID: 7252
		private ArrayList items = new ArrayList();
	}
}
