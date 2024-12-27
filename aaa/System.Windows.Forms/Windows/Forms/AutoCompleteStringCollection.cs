using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x0200021C RID: 540
	public class AutoCompleteStringCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x17000302 RID: 770
		public string this[int index]
		{
			get
			{
				return (string)this.data[index];
			}
			set
			{
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, this.data[index]));
				this.data[index] = value;
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, value));
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x060018C8 RID: 6344 RVA: 0x0002C05F File Offset: 0x0002B05F
		public int Count
		{
			get
			{
				return this.data.Count;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x060018C9 RID: 6345 RVA: 0x0002C06C File Offset: 0x0002B06C
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x060018CA RID: 6346 RVA: 0x0002C06F File Offset: 0x0002B06F
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x060018CB RID: 6347 RVA: 0x0002C072 File Offset: 0x0002B072
		// (remove) Token: 0x060018CC RID: 6348 RVA: 0x0002C08B File Offset: 0x0002B08B
		public event CollectionChangeEventHandler CollectionChanged
		{
			add
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChanged, value);
			}
			remove
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChanged, value);
			}
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x0002C0A4 File Offset: 0x0002B0A4
		protected void OnCollectionChanged(CollectionChangeEventArgs e)
		{
			if (this.onCollectionChanged != null)
			{
				this.onCollectionChanged(this, e);
			}
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x0002C0BC File Offset: 0x0002B0BC
		public int Add(string value)
		{
			int num = this.data.Add(value);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, value));
			return num;
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x0002C0E4 File Offset: 0x0002B0E4
		public void AddRange(string[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.data.AddRange(value);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x0002C10D File Offset: 0x0002B10D
		public void Clear()
		{
			this.data.Clear();
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x0002C127 File Offset: 0x0002B127
		public bool Contains(string value)
		{
			return this.data.Contains(value);
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x0002C135 File Offset: 0x0002B135
		public void CopyTo(string[] array, int index)
		{
			this.data.CopyTo(array, index);
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x0002C144 File Offset: 0x0002B144
		public int IndexOf(string value)
		{
			return this.data.IndexOf(value);
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x0002C152 File Offset: 0x0002B152
		public void Insert(int index, string value)
		{
			this.data.Insert(index, value);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, value));
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x060018D5 RID: 6357 RVA: 0x0002C16E File Offset: 0x0002B16E
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x060018D6 RID: 6358 RVA: 0x0002C171 File Offset: 0x0002B171
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x0002C174 File Offset: 0x0002B174
		public void Remove(string value)
		{
			this.data.Remove(value);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, value));
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x0002C190 File Offset: 0x0002B190
		public void RemoveAt(int index)
		{
			string text = (string)this.data[index];
			this.data.RemoveAt(index);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, text));
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x060018D9 RID: 6361 RVA: 0x0002C1C8 File Offset: 0x0002B1C8
		public object SyncRoot
		{
			[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
			get
			{
				return this;
			}
		}

		// Token: 0x17000309 RID: 777
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

		// Token: 0x060018DC RID: 6364 RVA: 0x0002C1E3 File Offset: 0x0002B1E3
		int IList.Add(object value)
		{
			return this.Add((string)value);
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x0002C1F1 File Offset: 0x0002B1F1
		bool IList.Contains(object value)
		{
			return this.Contains((string)value);
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x0002C1FF File Offset: 0x0002B1FF
		int IList.IndexOf(object value)
		{
			return this.IndexOf((string)value);
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x0002C20D File Offset: 0x0002B20D
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (string)value);
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x0002C21C File Offset: 0x0002B21C
		void IList.Remove(object value)
		{
			this.Remove((string)value);
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x0002C22A File Offset: 0x0002B22A
		void ICollection.CopyTo(Array array, int index)
		{
			this.data.CopyTo(array, index);
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x0002C239 File Offset: 0x0002B239
		public IEnumerator GetEnumerator()
		{
			return this.data.GetEnumerator();
		}

		// Token: 0x0400122C RID: 4652
		private CollectionChangeEventHandler onCollectionChanged;

		// Token: 0x0400122D RID: 4653
		private ArrayList data = new ArrayList();
	}
}
