using System;
using System.Collections;

namespace System.Configuration
{
	// Token: 0x02000039 RID: 57
	public class ConfigurationPropertyCollection : ICollection, IEnumerable
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002BC RID: 700 RVA: 0x00010FE6 File Offset: 0x0000FFE6
		public int Count
		{
			get
			{
				return this._items.Count;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00010FF3 File Offset: 0x0000FFF3
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002BE RID: 702 RVA: 0x00010FF6 File Offset: 0x0000FFF6
		public object SyncRoot
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002BF RID: 703 RVA: 0x00010FFE File Offset: 0x0000FFFE
		internal ConfigurationProperty DefaultCollectionProperty
		{
			get
			{
				return this[ConfigurationProperty.DefaultCollectionPropertyName];
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0001100B File Offset: 0x0001000B
		void ICollection.CopyTo(Array array, int index)
		{
			this._items.CopyTo(array, index);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0001101A File Offset: 0x0001001A
		public void CopyTo(ConfigurationProperty[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00011024 File Offset: 0x00010024
		public IEnumerator GetEnumerator()
		{
			return this._items.GetEnumerator();
		}

		// Token: 0x170000B2 RID: 178
		public ConfigurationProperty this[string name]
		{
			get
			{
				for (int i = 0; i < this._items.Count; i++)
				{
					ConfigurationProperty configurationProperty = (ConfigurationProperty)this._items[i];
					if (configurationProperty.Name == name)
					{
						return (ConfigurationProperty)this._items[i];
					}
				}
				return null;
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0001108C File Offset: 0x0001008C
		public bool Contains(string name)
		{
			for (int i = 0; i < this._items.Count; i++)
			{
				ConfigurationProperty configurationProperty = (ConfigurationProperty)this._items[i];
				if (configurationProperty.Name == name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x000110D2 File Offset: 0x000100D2
		public void Add(ConfigurationProperty property)
		{
			if (!this.Contains(property.Name))
			{
				this._items.Add(property);
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x000110F0 File Offset: 0x000100F0
		public bool Remove(string name)
		{
			for (int i = 0; i < this._items.Count; i++)
			{
				ConfigurationProperty configurationProperty = (ConfigurationProperty)this._items[i];
				if (configurationProperty.Name == name)
				{
					this._items.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00011142 File Offset: 0x00010142
		public void Clear()
		{
			this._items.Clear();
		}

		// Token: 0x04000289 RID: 649
		private ArrayList _items = new ArrayList();
	}
}
