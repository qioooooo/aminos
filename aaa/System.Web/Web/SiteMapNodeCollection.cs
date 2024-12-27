using System;
using System.Collections;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.Web
{
	// Token: 0x020000D0 RID: 208
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class SiteMapNodeCollection : IHierarchicalEnumerable, IList, ICollection, IEnumerable
	{
		// Token: 0x06000957 RID: 2391 RVA: 0x000298A4 File Offset: 0x000288A4
		public SiteMapNodeCollection()
		{
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x000298B4 File Offset: 0x000288B4
		public SiteMapNodeCollection(int capacity)
		{
			this._initialSize = capacity;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x000298CB File Offset: 0x000288CB
		public SiteMapNodeCollection(SiteMapNode value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this._initialSize = 1;
			this.List.Add(value);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x000298FD File Offset: 0x000288FD
		public SiteMapNodeCollection(SiteMapNode[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this._initialSize = value.Length;
			this.AddRangeInternal(value);
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0002992B File Offset: 0x0002892B
		public SiteMapNodeCollection(SiteMapNodeCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this._initialSize = value.Count;
			this.AddRangeInternal(value);
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x0002995C File Offset: 0x0002895C
		public virtual int Count
		{
			get
			{
				return this.List.Count;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x00029969 File Offset: 0x00028969
		public virtual bool IsSynchronized
		{
			get
			{
				return this.List.IsSynchronized;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x00029976 File Offset: 0x00028976
		public virtual object SyncRoot
		{
			get
			{
				return this.List.SyncRoot;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x00029983 File Offset: 0x00028983
		private ArrayList List
		{
			get
			{
				if (this._innerList == null)
				{
					this._innerList = new ArrayList(this._initialSize);
				}
				return this._innerList;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x000299A4 File Offset: 0x000289A4
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x000299A7 File Offset: 0x000289A7
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000303 RID: 771
		public virtual SiteMapNode this[int index]
		{
			get
			{
				return (SiteMapNode)this.List[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.List[index] = value;
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x000299DA File Offset: 0x000289DA
		public virtual int Add(SiteMapNode value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return this.List.Add(value);
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x000299F6 File Offset: 0x000289F6
		public virtual void AddRange(SiteMapNode[] value)
		{
			this.AddRangeInternal(value);
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x000299FF File Offset: 0x000289FF
		public virtual void AddRange(SiteMapNodeCollection value)
		{
			this.AddRangeInternal(value);
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x00029A08 File Offset: 0x00028A08
		private void AddRangeInternal(IList value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.List.AddRange(value);
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00029A24 File Offset: 0x00028A24
		public virtual void Clear()
		{
			this.List.Clear();
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x00029A31 File Offset: 0x00028A31
		public virtual bool Contains(SiteMapNode value)
		{
			return this.List.Contains(value);
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00029A3F File Offset: 0x00028A3F
		public virtual void CopyTo(SiteMapNode[] array, int index)
		{
			this.CopyToInternal(array, index);
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00029A49 File Offset: 0x00028A49
		internal virtual void CopyToInternal(Array array, int index)
		{
			this.List.CopyTo(array, index);
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x00029A58 File Offset: 0x00028A58
		public SiteMapDataSourceView GetDataSourceView(SiteMapDataSource owner, string viewName)
		{
			return new SiteMapDataSourceView(owner, viewName, this);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x00029A62 File Offset: 0x00028A62
		public virtual IEnumerator GetEnumerator()
		{
			return this.List.GetEnumerator();
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00029A6F File Offset: 0x00028A6F
		public SiteMapHierarchicalDataSourceView GetHierarchicalDataSourceView()
		{
			return new SiteMapHierarchicalDataSourceView(this);
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x00029A77 File Offset: 0x00028A77
		public virtual IHierarchyData GetHierarchyData(object enumeratedItem)
		{
			return enumeratedItem as IHierarchyData;
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x00029A7F File Offset: 0x00028A7F
		public virtual int IndexOf(SiteMapNode value)
		{
			return this.List.IndexOf(value);
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00029A8D File Offset: 0x00028A8D
		public virtual void Insert(int index, SiteMapNode value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.List.Insert(index, value);
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00029AAC File Offset: 0x00028AAC
		protected virtual void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is SiteMapNode))
			{
				throw new ArgumentException(SR.GetString("SiteMapNodeCollection_Invalid_Type", new object[] { value.GetType().ToString() }));
			}
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00029AF5 File Offset: 0x00028AF5
		public static SiteMapNodeCollection ReadOnly(SiteMapNodeCollection collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			return new SiteMapNodeCollection.ReadOnlySiteMapNodeCollection(collection);
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00029B0B File Offset: 0x00028B0B
		public virtual void Remove(SiteMapNode value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.List.Remove(value);
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00029B27 File Offset: 0x00028B27
		public virtual void RemoveAt(int index)
		{
			this.List.RemoveAt(index);
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x00029B35 File Offset: 0x00028B35
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x00029B3D File Offset: 0x00028B3D
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.IsSynchronized;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000978 RID: 2424 RVA: 0x00029B45 File Offset: 0x00028B45
		object ICollection.SyncRoot
		{
			get
			{
				return this.SyncRoot;
			}
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00029B4D File Offset: 0x00028B4D
		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyToInternal(array, index);
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00029B57 File Offset: 0x00028B57
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00029B5F File Offset: 0x00028B5F
		IHierarchyData IHierarchicalEnumerable.GetHierarchyData(object enumeratedItem)
		{
			return this.GetHierarchyData(enumeratedItem);
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x00029B68 File Offset: 0x00028B68
		bool IList.IsFixedSize
		{
			get
			{
				return this.IsFixedSize;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x00029B70 File Offset: 0x00028B70
		bool IList.IsReadOnly
		{
			get
			{
				return this.IsReadOnly;
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
				this.OnValidate(value);
				this[index] = (SiteMapNode)value;
			}
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00029B97 File Offset: 0x00028B97
		int IList.Add(object value)
		{
			this.OnValidate(value);
			return this.Add((SiteMapNode)value);
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00029BAC File Offset: 0x00028BAC
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x00029BB4 File Offset: 0x00028BB4
		bool IList.Contains(object value)
		{
			this.OnValidate(value);
			return this.Contains((SiteMapNode)value);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00029BC9 File Offset: 0x00028BC9
		int IList.IndexOf(object value)
		{
			this.OnValidate(value);
			return this.IndexOf((SiteMapNode)value);
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x00029BDE File Offset: 0x00028BDE
		void IList.Insert(int index, object value)
		{
			this.OnValidate(value);
			this.Insert(index, (SiteMapNode)value);
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00029BF4 File Offset: 0x00028BF4
		void IList.Remove(object value)
		{
			this.OnValidate(value);
			this.Remove((SiteMapNode)value);
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00029C09 File Offset: 0x00028C09
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x04001244 RID: 4676
		internal static SiteMapNodeCollection Empty = new SiteMapNodeCollection.ReadOnlySiteMapNodeCollection(new SiteMapNodeCollection());

		// Token: 0x04001245 RID: 4677
		private int _initialSize = 10;

		// Token: 0x04001246 RID: 4678
		private ArrayList _innerList;

		// Token: 0x020000D1 RID: 209
		private sealed class ReadOnlySiteMapNodeCollection : SiteMapNodeCollection
		{
			// Token: 0x06000988 RID: 2440 RVA: 0x00029C23 File Offset: 0x00028C23
			internal ReadOnlySiteMapNodeCollection(SiteMapNodeCollection collection)
			{
				if (collection == null)
				{
					throw new ArgumentNullException("collection");
				}
				this._internalCollection = collection;
			}

			// Token: 0x1700030A RID: 778
			// (get) Token: 0x06000989 RID: 2441 RVA: 0x00029C40 File Offset: 0x00028C40
			public override int Count
			{
				get
				{
					return this._internalCollection.Count;
				}
			}

			// Token: 0x1700030B RID: 779
			// (get) Token: 0x0600098A RID: 2442 RVA: 0x00029C4D File Offset: 0x00028C4D
			public override bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700030C RID: 780
			// (get) Token: 0x0600098B RID: 2443 RVA: 0x00029C50 File Offset: 0x00028C50
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700030D RID: 781
			// (get) Token: 0x0600098C RID: 2444 RVA: 0x00029C53 File Offset: 0x00028C53
			public override bool IsSynchronized
			{
				get
				{
					return this._internalCollection.IsSynchronized;
				}
			}

			// Token: 0x1700030E RID: 782
			// (get) Token: 0x0600098D RID: 2445 RVA: 0x00029C60 File Offset: 0x00028C60
			public override object SyncRoot
			{
				get
				{
					return this._internalCollection.SyncRoot;
				}
			}

			// Token: 0x0600098E RID: 2446 RVA: 0x00029C6D File Offset: 0x00028C6D
			public override int Add(SiteMapNode value)
			{
				throw new NotSupportedException(SR.GetString("Collection_readonly"));
			}

			// Token: 0x0600098F RID: 2447 RVA: 0x00029C7E File Offset: 0x00028C7E
			public override void AddRange(SiteMapNode[] value)
			{
				throw new NotSupportedException(SR.GetString("Collection_readonly"));
			}

			// Token: 0x06000990 RID: 2448 RVA: 0x00029C8F File Offset: 0x00028C8F
			public override void AddRange(SiteMapNodeCollection value)
			{
				throw new NotSupportedException(SR.GetString("Collection_readonly"));
			}

			// Token: 0x06000991 RID: 2449 RVA: 0x00029CA0 File Offset: 0x00028CA0
			public override void Clear()
			{
				throw new NotSupportedException(SR.GetString("Collection_readonly"));
			}

			// Token: 0x06000992 RID: 2450 RVA: 0x00029CB1 File Offset: 0x00028CB1
			public override bool Contains(SiteMapNode node)
			{
				return this._internalCollection.Contains(node);
			}

			// Token: 0x06000993 RID: 2451 RVA: 0x00029CBF File Offset: 0x00028CBF
			internal override void CopyToInternal(Array array, int index)
			{
				this._internalCollection.List.CopyTo(array, index);
			}

			// Token: 0x1700030F RID: 783
			public override SiteMapNode this[int index]
			{
				get
				{
					return this._internalCollection[index];
				}
				set
				{
					throw new NotSupportedException(SR.GetString("Collection_readonly"));
				}
			}

			// Token: 0x06000996 RID: 2454 RVA: 0x00029CF2 File Offset: 0x00028CF2
			public override IEnumerator GetEnumerator()
			{
				return this._internalCollection.GetEnumerator();
			}

			// Token: 0x06000997 RID: 2455 RVA: 0x00029CFF File Offset: 0x00028CFF
			public override int IndexOf(SiteMapNode value)
			{
				return this._internalCollection.IndexOf(value);
			}

			// Token: 0x06000998 RID: 2456 RVA: 0x00029D0D File Offset: 0x00028D0D
			public override void Insert(int index, SiteMapNode value)
			{
				throw new NotSupportedException(SR.GetString("Collection_readonly"));
			}

			// Token: 0x06000999 RID: 2457 RVA: 0x00029D1E File Offset: 0x00028D1E
			public override void Remove(SiteMapNode value)
			{
				throw new NotSupportedException(SR.GetString("Collection_readonly"));
			}

			// Token: 0x0600099A RID: 2458 RVA: 0x00029D2F File Offset: 0x00028D2F
			public override void RemoveAt(int index)
			{
				throw new NotSupportedException(SR.GetString("Collection_readonly"));
			}

			// Token: 0x04001247 RID: 4679
			private SiteMapNodeCollection _internalCollection;
		}
	}
}
