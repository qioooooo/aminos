using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x0200044F RID: 1103
	internal sealed class ParsedAttributeCollection : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06003473 RID: 13427 RVA: 0x000E36A0 File Offset: 0x000E26A0
		internal ParsedAttributeCollection()
		{
			this._filterTable = new ListDictionary(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x06003474 RID: 13428 RVA: 0x000E36B8 File Offset: 0x000E26B8
		private IDictionary AllFiltersDictionary
		{
			get
			{
				if (this._allFiltersDictionary == null)
				{
					this._allFiltersDictionary = new ListDictionary(StringComparer.OrdinalIgnoreCase);
					foreach (object obj in this._filterTable.Values)
					{
						FilteredAttributeDictionary filteredAttributeDictionary = (FilteredAttributeDictionary)obj;
						foreach (object obj2 in ((IEnumerable)filteredAttributeDictionary))
						{
							DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
							this._allFiltersDictionary[Util.CreateFilteredName(filteredAttributeDictionary.Filter, dictionaryEntry.Key.ToString())] = dictionaryEntry.Value;
						}
					}
				}
				return this._allFiltersDictionary;
			}
		}

		// Token: 0x06003475 RID: 13429 RVA: 0x000E379C File Offset: 0x000E279C
		public void AddFilteredAttribute(string filter, string name, string value)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (filter == null)
			{
				filter = string.Empty;
			}
			if (this._allFiltersDictionary != null)
			{
				this._allFiltersDictionary.Add(Util.CreateFilteredName(filter, name), value);
			}
			FilteredAttributeDictionary filteredAttributeDictionary = (FilteredAttributeDictionary)this._filterTable[filter];
			if (filteredAttributeDictionary == null)
			{
				filteredAttributeDictionary = new FilteredAttributeDictionary(this, filter);
				this._filterTable[filter] = filteredAttributeDictionary;
			}
			filteredAttributeDictionary.Data.Add(name, value);
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x000E3828 File Offset: 0x000E2828
		public void ClearFilter(string filter)
		{
			if (filter == null)
			{
				filter = string.Empty;
			}
			if (this._allFiltersDictionary != null)
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in this._allFiltersDictionary.Keys)
				{
					string text = (string)obj;
					string text3;
					string text2 = Util.ParsePropertyDeviceFilter(text, out text3);
					if (StringUtil.EqualsIgnoreCase(text2, filter))
					{
						arrayList.Add(text);
					}
				}
				foreach (object obj2 in arrayList)
				{
					string text4 = (string)obj2;
					this._allFiltersDictionary.Remove(text4);
				}
			}
			this._filterTable.Remove(filter);
		}

		// Token: 0x06003477 RID: 13431 RVA: 0x000E3918 File Offset: 0x000E2918
		public ICollection GetFilteredAttributeDictionaries()
		{
			return this._filterTable.Values;
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x000E3928 File Offset: 0x000E2928
		public void RemoveFilteredAttribute(string filter, string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("name");
			}
			if (filter == null)
			{
				filter = string.Empty;
			}
			if (this._allFiltersDictionary != null)
			{
				this._allFiltersDictionary.Remove(Util.CreateFilteredName(filter, name));
			}
			FilteredAttributeDictionary filteredAttributeDictionary = (FilteredAttributeDictionary)this._filterTable[filter];
			if (filteredAttributeDictionary != null)
			{
				filteredAttributeDictionary.Data.Remove(name);
			}
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x000E3990 File Offset: 0x000E2990
		public void ReplaceFilteredAttribute(string filter, string name, string value)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("name");
			}
			if (filter == null)
			{
				filter = string.Empty;
			}
			if (this._allFiltersDictionary != null)
			{
				this._allFiltersDictionary[Util.CreateFilteredName(filter, name)] = value;
			}
			FilteredAttributeDictionary filteredAttributeDictionary = (FilteredAttributeDictionary)this._filterTable[filter];
			if (filteredAttributeDictionary == null)
			{
				filteredAttributeDictionary = new FilteredAttributeDictionary(this, filter);
				this._filterTable[filter] = filteredAttributeDictionary;
			}
			filteredAttributeDictionary.Data[name] = value;
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x0600347A RID: 13434 RVA: 0x000E3A0C File Offset: 0x000E2A0C
		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x0600347B RID: 13435 RVA: 0x000E3A0F File Offset: 0x000E2A0F
		bool IDictionary.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BB6 RID: 2998
		object IDictionary.this[object key]
		{
			get
			{
				return this.AllFiltersDictionary[key];
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				string text2;
				string text = Util.ParsePropertyDeviceFilter(key.ToString(), out text2);
				this.ReplaceFilteredAttribute(text, text2, value.ToString());
			}
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x0600347E RID: 13438 RVA: 0x000E3A57 File Offset: 0x000E2A57
		ICollection IDictionary.Keys
		{
			get
			{
				return this.AllFiltersDictionary.Keys;
			}
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x0600347F RID: 13439 RVA: 0x000E3A64 File Offset: 0x000E2A64
		ICollection IDictionary.Values
		{
			get
			{
				return this.AllFiltersDictionary.Values;
			}
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x000E3A74 File Offset: 0x000E2A74
		void IDictionary.Add(object key, object value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (value == null)
			{
				value = string.Empty;
			}
			string text2;
			string text = Util.ParsePropertyDeviceFilter(key.ToString(), out text2);
			this.AddFilteredAttribute(text, text2, value.ToString());
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x000E3AB5 File Offset: 0x000E2AB5
		bool IDictionary.Contains(object key)
		{
			return this.AllFiltersDictionary.Contains(key);
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x000E3AC3 File Offset: 0x000E2AC3
		void IDictionary.Clear()
		{
			this.AllFiltersDictionary.Clear();
			this._filterTable.Clear();
		}

		// Token: 0x06003483 RID: 13443 RVA: 0x000E3ADB File Offset: 0x000E2ADB
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return this.AllFiltersDictionary.GetEnumerator();
		}

		// Token: 0x06003484 RID: 13444 RVA: 0x000E3AE8 File Offset: 0x000E2AE8
		void IDictionary.Remove(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			string text2;
			string text = Util.ParsePropertyDeviceFilter(key.ToString(), out text2);
			this.RemoveFilteredAttribute(text, text2);
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x06003485 RID: 13445 RVA: 0x000E3B19 File Offset: 0x000E2B19
		int ICollection.Count
		{
			get
			{
				return this.AllFiltersDictionary.Count;
			}
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x06003486 RID: 13446 RVA: 0x000E3B26 File Offset: 0x000E2B26
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.AllFiltersDictionary.IsSynchronized;
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x06003487 RID: 13447 RVA: 0x000E3B33 File Offset: 0x000E2B33
		object ICollection.SyncRoot
		{
			get
			{
				return this.AllFiltersDictionary.SyncRoot;
			}
		}

		// Token: 0x06003488 RID: 13448 RVA: 0x000E3B40 File Offset: 0x000E2B40
		void ICollection.CopyTo(Array array, int index)
		{
			this.AllFiltersDictionary.CopyTo(array, index);
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x000E3B4F File Offset: 0x000E2B4F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.AllFiltersDictionary.GetEnumerator();
		}

		// Token: 0x040024C0 RID: 9408
		private IDictionary _filterTable;

		// Token: 0x040024C1 RID: 9409
		private IDictionary _allFiltersDictionary;
	}
}
