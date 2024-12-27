using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Web.UI
{
	// Token: 0x020003F6 RID: 1014
	internal sealed class FilteredAttributeDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06003214 RID: 12820 RVA: 0x000DC272 File Offset: 0x000DB272
		internal FilteredAttributeDictionary(ParsedAttributeCollection owner, string filter)
		{
			this._filter = filter;
			this._owner = owner;
			this._data = new ListDictionary(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06003215 RID: 12821 RVA: 0x000DC298 File Offset: 0x000DB298
		internal IDictionary Data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06003216 RID: 12822 RVA: 0x000DC2A0 File Offset: 0x000DB2A0
		public string Filter
		{
			get
			{
				return this._filter;
			}
		}

		// Token: 0x17000B04 RID: 2820
		public string this[string key]
		{
			get
			{
				return (string)this._data[key];
			}
			set
			{
				this._owner.ReplaceFilteredAttribute(this._filter, key, value);
			}
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x000DC2D0 File Offset: 0x000DB2D0
		public void Add(string key, string value)
		{
			this._owner.AddFilteredAttribute(this._filter, key, value);
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x000DC2E5 File Offset: 0x000DB2E5
		public void Clear()
		{
			this._owner.ClearFilter(this._filter);
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x000DC2F8 File Offset: 0x000DB2F8
		public bool Contains(string key)
		{
			return this._data.Contains(key);
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x000DC306 File Offset: 0x000DB306
		public void Remove(string key)
		{
			this._owner.RemoveFilteredAttribute(this._filter, key);
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x0600321D RID: 12829 RVA: 0x000DC31A File Offset: 0x000DB31A
		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x0600321E RID: 12830 RVA: 0x000DC31D File Offset: 0x000DB31D
		bool IDictionary.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B07 RID: 2823
		object IDictionary.this[object key]
		{
			get
			{
				if (!(key is string))
				{
					throw new ArgumentException(SR.GetString("FilteredAttributeDictionary_ArgumentMustBeString"), "key");
				}
				return this[key.ToString()];
			}
			set
			{
				if (!(key is string))
				{
					throw new ArgumentException(SR.GetString("FilteredAttributeDictionary_ArgumentMustBeString"), "key");
				}
				if (!(value is string))
				{
					throw new ArgumentException(SR.GetString("FilteredAttributeDictionary_ArgumentMustBeString"), "value");
				}
				this[key.ToString()] = value.ToString();
			}
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06003221 RID: 12833 RVA: 0x000DC3A5 File Offset: 0x000DB3A5
		ICollection IDictionary.Keys
		{
			get
			{
				return this._data.Keys;
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06003222 RID: 12834 RVA: 0x000DC3B2 File Offset: 0x000DB3B2
		ICollection IDictionary.Values
		{
			get
			{
				return this._data.Values;
			}
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x000DC3C0 File Offset: 0x000DB3C0
		void IDictionary.Add(object key, object value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!(key is string))
			{
				throw new ArgumentException(SR.GetString("FilteredAttributeDictionary_ArgumentMustBeString"), "key");
			}
			if (!(value is string))
			{
				throw new ArgumentException(SR.GetString("FilteredAttributeDictionary_ArgumentMustBeString"), "value");
			}
			if (value == null)
			{
				value = string.Empty;
			}
			this.Add(key.ToString(), value.ToString());
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x000DC431 File Offset: 0x000DB431
		bool IDictionary.Contains(object key)
		{
			if (!(key is string))
			{
				throw new ArgumentException(SR.GetString("FilteredAttributeDictionary_ArgumentMustBeString"), "key");
			}
			return this.Contains(key.ToString());
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x000DC45C File Offset: 0x000DB45C
		void IDictionary.Clear()
		{
			this.Clear();
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x000DC464 File Offset: 0x000DB464
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return this._data.GetEnumerator();
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x000DC471 File Offset: 0x000DB471
		void IDictionary.Remove(object key)
		{
			this.Remove(key.ToString());
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06003228 RID: 12840 RVA: 0x000DC47F File Offset: 0x000DB47F
		int ICollection.Count
		{
			get
			{
				return this._data.Count;
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06003229 RID: 12841 RVA: 0x000DC48C File Offset: 0x000DB48C
		bool ICollection.IsSynchronized
		{
			get
			{
				return this._data.IsSynchronized;
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x0600322A RID: 12842 RVA: 0x000DC499 File Offset: 0x000DB499
		object ICollection.SyncRoot
		{
			get
			{
				return this._data.SyncRoot;
			}
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x000DC4A6 File Offset: 0x000DB4A6
		void ICollection.CopyTo(Array array, int index)
		{
			this._data.CopyTo(array, index);
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x000DC4B5 File Offset: 0x000DB4B5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._data.GetEnumerator();
		}

		// Token: 0x040022F5 RID: 8949
		private string _filter;

		// Token: 0x040022F6 RID: 8950
		private IDictionary _data;

		// Token: 0x040022F7 RID: 8951
		private ParsedAttributeCollection _owner;
	}
}
