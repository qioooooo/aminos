using System;
using System.Collections;

namespace System.Windows.Forms
{
	// Token: 0x02000317 RID: 791
	internal class DataGridViewCellLinkedList : IEnumerable
	{
		// Token: 0x06003345 RID: 13125 RVA: 0x000B3E25 File Offset: 0x000B2E25
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DataGridViewCellLinkedListEnumerator(this.headElement);
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x000B3E32 File Offset: 0x000B2E32
		public DataGridViewCellLinkedList()
		{
			this.lastAccessedIndex = -1;
		}

		// Token: 0x17000909 RID: 2313
		public DataGridViewCell this[int index]
		{
			get
			{
				if (this.lastAccessedIndex == -1 || index < this.lastAccessedIndex)
				{
					DataGridViewCellLinkedListElement next = this.headElement;
					for (int i = index; i > 0; i--)
					{
						next = next.Next;
					}
					this.lastAccessedElement = next;
					this.lastAccessedIndex = index;
					return next.DataGridViewCell;
				}
				while (this.lastAccessedIndex < index)
				{
					this.lastAccessedElement = this.lastAccessedElement.Next;
					this.lastAccessedIndex++;
				}
				return this.lastAccessedElement.DataGridViewCell;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06003348 RID: 13128 RVA: 0x000B3EC5 File Offset: 0x000B2EC5
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06003349 RID: 13129 RVA: 0x000B3ECD File Offset: 0x000B2ECD
		public DataGridViewCell HeadCell
		{
			get
			{
				return this.headElement.DataGridViewCell;
			}
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x000B3EDC File Offset: 0x000B2EDC
		public void Add(DataGridViewCell dataGridViewCell)
		{
			DataGridViewCellLinkedListElement dataGridViewCellLinkedListElement = new DataGridViewCellLinkedListElement(dataGridViewCell);
			if (this.headElement != null)
			{
				dataGridViewCellLinkedListElement.Next = this.headElement;
			}
			this.headElement = dataGridViewCellLinkedListElement;
			this.count++;
			this.lastAccessedElement = null;
			this.lastAccessedIndex = -1;
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x000B3F27 File Offset: 0x000B2F27
		public void Clear()
		{
			this.lastAccessedElement = null;
			this.lastAccessedIndex = -1;
			this.headElement = null;
			this.count = 0;
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x000B3F48 File Offset: 0x000B2F48
		public bool Contains(DataGridViewCell dataGridViewCell)
		{
			int num = 0;
			DataGridViewCellLinkedListElement next = this.headElement;
			while (next != null)
			{
				if (next.DataGridViewCell == dataGridViewCell)
				{
					this.lastAccessedElement = next;
					this.lastAccessedIndex = num;
					return true;
				}
				next = next.Next;
				num++;
			}
			return false;
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x000B3F88 File Offset: 0x000B2F88
		public bool Remove(DataGridViewCell dataGridViewCell)
		{
			DataGridViewCellLinkedListElement dataGridViewCellLinkedListElement = null;
			DataGridViewCellLinkedListElement next = this.headElement;
			while (next != null && next.DataGridViewCell != dataGridViewCell)
			{
				dataGridViewCellLinkedListElement = next;
				next = next.Next;
			}
			if (next.DataGridViewCell == dataGridViewCell)
			{
				DataGridViewCellLinkedListElement next2 = next.Next;
				if (dataGridViewCellLinkedListElement == null)
				{
					this.headElement = next2;
				}
				else
				{
					dataGridViewCellLinkedListElement.Next = next2;
				}
				this.count--;
				this.lastAccessedElement = null;
				this.lastAccessedIndex = -1;
				return true;
			}
			return false;
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x000B3FF8 File Offset: 0x000B2FF8
		public int RemoveAllCellsAtBand(bool column, int bandIndex)
		{
			int num = 0;
			DataGridViewCellLinkedListElement dataGridViewCellLinkedListElement = null;
			DataGridViewCellLinkedListElement dataGridViewCellLinkedListElement2 = this.headElement;
			while (dataGridViewCellLinkedListElement2 != null)
			{
				if ((column && dataGridViewCellLinkedListElement2.DataGridViewCell.ColumnIndex == bandIndex) || (!column && dataGridViewCellLinkedListElement2.DataGridViewCell.RowIndex == bandIndex))
				{
					DataGridViewCellLinkedListElement next = dataGridViewCellLinkedListElement2.Next;
					if (dataGridViewCellLinkedListElement == null)
					{
						this.headElement = next;
					}
					else
					{
						dataGridViewCellLinkedListElement.Next = next;
					}
					dataGridViewCellLinkedListElement2 = next;
					this.count--;
					this.lastAccessedElement = null;
					this.lastAccessedIndex = -1;
					num++;
				}
				else
				{
					dataGridViewCellLinkedListElement = dataGridViewCellLinkedListElement2;
					dataGridViewCellLinkedListElement2 = dataGridViewCellLinkedListElement2.Next;
				}
			}
			return num;
		}

		// Token: 0x04001AAF RID: 6831
		private DataGridViewCellLinkedListElement lastAccessedElement;

		// Token: 0x04001AB0 RID: 6832
		private DataGridViewCellLinkedListElement headElement;

		// Token: 0x04001AB1 RID: 6833
		private int count;

		// Token: 0x04001AB2 RID: 6834
		private int lastAccessedIndex;
	}
}
