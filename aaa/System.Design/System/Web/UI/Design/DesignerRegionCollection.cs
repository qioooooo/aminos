using System;
using System.Collections;
using System.Design;
using System.Globalization;

namespace System.Web.UI.Design
{
	// Token: 0x0200035F RID: 863
	public class DesignerRegionCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x06002066 RID: 8294 RVA: 0x000B6835 File Offset: 0x000B5835
		public DesignerRegionCollection()
		{
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x000B683D File Offset: 0x000B583D
		public DesignerRegionCollection(ControlDesigner owner)
		{
			this._owner = owner;
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06002068 RID: 8296 RVA: 0x000B684C File Offset: 0x000B584C
		public int Count
		{
			get
			{
				return this.InternalList.Count;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06002069 RID: 8297 RVA: 0x000B6859 File Offset: 0x000B5859
		private ArrayList InternalList
		{
			get
			{
				if (this._list == null)
				{
					this._list = new ArrayList();
				}
				return this._list;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x0600206A RID: 8298 RVA: 0x000B6874 File Offset: 0x000B5874
		public bool IsFixedSize
		{
			get
			{
				return this.InternalList.IsFixedSize;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x0600206B RID: 8299 RVA: 0x000B6881 File Offset: 0x000B5881
		public bool IsReadOnly
		{
			get
			{
				return this.InternalList.IsReadOnly;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x0600206C RID: 8300 RVA: 0x000B688E File Offset: 0x000B588E
		public bool IsSynchronized
		{
			get
			{
				return this.InternalList.IsSynchronized;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x0600206D RID: 8301 RVA: 0x000B689B File Offset: 0x000B589B
		public ControlDesigner Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x0600206E RID: 8302 RVA: 0x000B68A3 File Offset: 0x000B58A3
		public object SyncRoot
		{
			get
			{
				return this.InternalList.SyncRoot;
			}
		}

		// Token: 0x170005CC RID: 1484
		public DesignerRegion this[int index]
		{
			get
			{
				return (DesignerRegion)this.InternalList[index];
			}
			set
			{
				this.InternalList[index] = value;
			}
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x000B68D2 File Offset: 0x000B58D2
		public int Add(DesignerRegion region)
		{
			return this.InternalList.Add(region);
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x000B68E0 File Offset: 0x000B58E0
		public void Clear()
		{
			this.InternalList.Clear();
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x000B68ED File Offset: 0x000B58ED
		public void CopyTo(Array array, int index)
		{
			this.InternalList.CopyTo(array, index);
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x000B68FC File Offset: 0x000B58FC
		public IEnumerator GetEnumerator()
		{
			return this.InternalList.GetEnumerator();
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x000B6909 File Offset: 0x000B5909
		public bool Contains(DesignerRegion region)
		{
			return this.InternalList.Contains(region);
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x000B6917 File Offset: 0x000B5917
		public int IndexOf(DesignerRegion region)
		{
			return this.InternalList.IndexOf(region);
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x000B6925 File Offset: 0x000B5925
		public void Insert(int index, DesignerRegion region)
		{
			this.InternalList.Insert(index, region);
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x000B6934 File Offset: 0x000B5934
		public void Remove(DesignerRegion region)
		{
			this.InternalList.Remove(region);
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x000B6942 File Offset: 0x000B5942
		public void RemoveAt(int index)
		{
			this.InternalList.RemoveAt(index);
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x0600207A RID: 8314 RVA: 0x000B6950 File Offset: 0x000B5950
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x0600207B RID: 8315 RVA: 0x000B6958 File Offset: 0x000B5958
		bool IList.IsFixedSize
		{
			get
			{
				return this.IsFixedSize;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x0600207C RID: 8316 RVA: 0x000B6960 File Offset: 0x000B5960
		bool IList.IsReadOnly
		{
			get
			{
				return this.IsReadOnly;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x0600207D RID: 8317 RVA: 0x000B6968 File Offset: 0x000B5968
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.IsSynchronized;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x0600207E RID: 8318 RVA: 0x000B6970 File Offset: 0x000B5970
		object ICollection.SyncRoot
		{
			get
			{
				return this.SyncRoot;
			}
		}

		// Token: 0x170005D2 RID: 1490
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (!(value is DesignerRegion))
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SR.GetString("WrongType"), new object[] { "DesignerRegion" }), "value");
				}
				this[index] = (DesignerRegion)value;
			}
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x000B69D8 File Offset: 0x000B59D8
		int IList.Add(object o)
		{
			if (!(o is DesignerRegion))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SR.GetString("WrongType"), new object[] { "DesignerRegion" }), "o");
			}
			return this.Add((DesignerRegion)o);
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x000B6A28 File Offset: 0x000B5A28
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x000B6A30 File Offset: 0x000B5A30
		bool IList.Contains(object o)
		{
			if (!(o is DesignerRegion))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SR.GetString("WrongType"), new object[] { "DesignerRegion" }), "o");
			}
			return this.Contains((DesignerRegion)o);
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x000B6A80 File Offset: 0x000B5A80
		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x000B6A8A File Offset: 0x000B5A8A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x000B6A94 File Offset: 0x000B5A94
		int IList.IndexOf(object o)
		{
			if (!(o is DesignerRegion))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SR.GetString("WrongType"), new object[] { "DesignerRegion" }), "o");
			}
			return this.IndexOf((DesignerRegion)o);
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x000B6AE4 File Offset: 0x000B5AE4
		void IList.Insert(int index, object o)
		{
			if (!(o is DesignerRegion))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SR.GetString("WrongType"), new object[] { "DesignerRegion" }), "o");
			}
			this.Insert(index, (DesignerRegion)o);
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x000B6B38 File Offset: 0x000B5B38
		void IList.Remove(object o)
		{
			if (!(o is DesignerRegion))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SR.GetString("WrongType"), new object[] { "DesignerRegion" }), "o");
			}
			this.Remove((DesignerRegion)o);
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x000B6B88 File Offset: 0x000B5B88
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x040017DC RID: 6108
		private ArrayList _list;

		// Token: 0x040017DD RID: 6109
		private ControlDesigner _owner;
	}
}
