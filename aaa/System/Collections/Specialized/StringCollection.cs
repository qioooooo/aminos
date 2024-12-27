using System;

namespace System.Collections.Specialized
{
	// Token: 0x0200025F RID: 607
	[Serializable]
	public class StringCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x17000462 RID: 1122
		public string this[int index]
		{
			get
			{
				return (string)this.data[index];
			}
			set
			{
				this.data[index] = value;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001506 RID: 5382 RVA: 0x00045B30 File Offset: 0x00044B30
		public int Count
		{
			get
			{
				return this.data.Count;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001507 RID: 5383 RVA: 0x00045B3D File Offset: 0x00044B3D
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x00045B40 File Offset: 0x00044B40
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x00045B43 File Offset: 0x00044B43
		public int Add(string value)
		{
			return this.data.Add(value);
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00045B51 File Offset: 0x00044B51
		public void AddRange(string[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.data.AddRange(value);
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x00045B6D File Offset: 0x00044B6D
		public void Clear()
		{
			this.data.Clear();
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x00045B7A File Offset: 0x00044B7A
		public bool Contains(string value)
		{
			return this.data.Contains(value);
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x00045B88 File Offset: 0x00044B88
		public void CopyTo(string[] array, int index)
		{
			this.data.CopyTo(array, index);
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x00045B97 File Offset: 0x00044B97
		public StringEnumerator GetEnumerator()
		{
			return new StringEnumerator(this);
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x00045B9F File Offset: 0x00044B9F
		public int IndexOf(string value)
		{
			return this.data.IndexOf(value);
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x00045BAD File Offset: 0x00044BAD
		public void Insert(int index, string value)
		{
			this.data.Insert(index, value);
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001511 RID: 5393 RVA: 0x00045BBC File Offset: 0x00044BBC
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001512 RID: 5394 RVA: 0x00045BBF File Offset: 0x00044BBF
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x00045BC2 File Offset: 0x00044BC2
		public void Remove(string value)
		{
			this.data.Remove(value);
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x00045BD0 File Offset: 0x00044BD0
		public void RemoveAt(int index)
		{
			this.data.RemoveAt(index);
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06001515 RID: 5397 RVA: 0x00045BDE File Offset: 0x00044BDE
		public object SyncRoot
		{
			get
			{
				return this.data.SyncRoot;
			}
		}

		// Token: 0x17000469 RID: 1129
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (string)value;
			}
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x00045C03 File Offset: 0x00044C03
		int IList.Add(object value)
		{
			return this.Add((string)value);
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x00045C11 File Offset: 0x00044C11
		bool IList.Contains(object value)
		{
			return this.Contains((string)value);
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x00045C1F File Offset: 0x00044C1F
		int IList.IndexOf(object value)
		{
			return this.IndexOf((string)value);
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x00045C2D File Offset: 0x00044C2D
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (string)value);
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x00045C3C File Offset: 0x00044C3C
		void IList.Remove(object value)
		{
			this.Remove((string)value);
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x00045C4A File Offset: 0x00044C4A
		void ICollection.CopyTo(Array array, int index)
		{
			this.data.CopyTo(array, index);
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x00045C59 File Offset: 0x00044C59
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.data.GetEnumerator();
		}

		// Token: 0x040011B8 RID: 4536
		private ArrayList data = new ArrayList();
	}
}
