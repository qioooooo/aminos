using System;
using System.Collections;

namespace System.Windows.Forms
{
	// Token: 0x02000376 RID: 886
	internal class DataGridViewIntLinkedList : IEnumerable
	{
		// Token: 0x06003651 RID: 13905 RVA: 0x000C2277 File Offset: 0x000C1277
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DataGridViewIntLinkedListEnumerator(this.headElement);
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x000C2284 File Offset: 0x000C1284
		public DataGridViewIntLinkedList()
		{
			this.lastAccessedIndex = -1;
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x000C2294 File Offset: 0x000C1294
		public DataGridViewIntLinkedList(DataGridViewIntLinkedList source)
		{
			int num = source.Count;
			for (int i = 0; i < num; i++)
			{
				this.Add(source[i]);
			}
		}

		// Token: 0x170009EA RID: 2538
		public int this[int index]
		{
			get
			{
				if (this.lastAccessedIndex == -1 || index < this.lastAccessedIndex)
				{
					DataGridViewIntLinkedListElement next = this.headElement;
					for (int i = index; i > 0; i--)
					{
						next = next.Next;
					}
					this.lastAccessedElement = next;
					this.lastAccessedIndex = index;
					return next.Int;
				}
				while (this.lastAccessedIndex < index)
				{
					this.lastAccessedElement = this.lastAccessedElement.Next;
					this.lastAccessedIndex++;
				}
				return this.lastAccessedElement.Int;
			}
			set
			{
				if (index != this.lastAccessedIndex)
				{
					int num = this[index];
				}
				this.lastAccessedElement.Int = value;
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06003656 RID: 13910 RVA: 0x000C2368 File Offset: 0x000C1368
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06003657 RID: 13911 RVA: 0x000C2370 File Offset: 0x000C1370
		public int HeadInt
		{
			get
			{
				return this.headElement.Int;
			}
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x000C2380 File Offset: 0x000C1380
		public void Add(int integer)
		{
			DataGridViewIntLinkedListElement dataGridViewIntLinkedListElement = new DataGridViewIntLinkedListElement(integer);
			if (this.headElement != null)
			{
				dataGridViewIntLinkedListElement.Next = this.headElement;
			}
			this.headElement = dataGridViewIntLinkedListElement;
			this.count++;
			this.lastAccessedElement = null;
			this.lastAccessedIndex = -1;
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x000C23CB File Offset: 0x000C13CB
		public void Clear()
		{
			this.lastAccessedElement = null;
			this.lastAccessedIndex = -1;
			this.headElement = null;
			this.count = 0;
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x000C23EC File Offset: 0x000C13EC
		public bool Contains(int integer)
		{
			int num = 0;
			DataGridViewIntLinkedListElement next = this.headElement;
			while (next != null)
			{
				if (next.Int == integer)
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

		// Token: 0x0600365B RID: 13915 RVA: 0x000C242C File Offset: 0x000C142C
		public int IndexOf(int integer)
		{
			if (this.Contains(integer))
			{
				return this.lastAccessedIndex;
			}
			return -1;
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x000C2440 File Offset: 0x000C1440
		public bool Remove(int integer)
		{
			DataGridViewIntLinkedListElement dataGridViewIntLinkedListElement = null;
			DataGridViewIntLinkedListElement next = this.headElement;
			while (next != null && next.Int != integer)
			{
				dataGridViewIntLinkedListElement = next;
				next = next.Next;
			}
			if (next.Int == integer)
			{
				DataGridViewIntLinkedListElement next2 = next.Next;
				if (dataGridViewIntLinkedListElement == null)
				{
					this.headElement = next2;
				}
				else
				{
					dataGridViewIntLinkedListElement.Next = next2;
				}
				this.count--;
				this.lastAccessedElement = null;
				this.lastAccessedIndex = -1;
				return true;
			}
			return false;
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x000C24B0 File Offset: 0x000C14B0
		public void RemoveAt(int index)
		{
			DataGridViewIntLinkedListElement dataGridViewIntLinkedListElement = null;
			DataGridViewIntLinkedListElement next = this.headElement;
			while (index > 0)
			{
				dataGridViewIntLinkedListElement = next;
				next = next.Next;
				index--;
			}
			DataGridViewIntLinkedListElement next2 = next.Next;
			if (dataGridViewIntLinkedListElement == null)
			{
				this.headElement = next2;
			}
			else
			{
				dataGridViewIntLinkedListElement.Next = next2;
			}
			this.count--;
			this.lastAccessedElement = null;
			this.lastAccessedIndex = -1;
		}

		// Token: 0x04001BD0 RID: 7120
		private DataGridViewIntLinkedListElement lastAccessedElement;

		// Token: 0x04001BD1 RID: 7121
		private DataGridViewIntLinkedListElement headElement;

		// Token: 0x04001BD2 RID: 7122
		private int count;

		// Token: 0x04001BD3 RID: 7123
		private int lastAccessedIndex;
	}
}
