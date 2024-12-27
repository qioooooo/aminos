using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200012C RID: 300
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class PropertyDescriptorCollection : IList, IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06000976 RID: 2422 RVA: 0x0001F740 File Offset: 0x0001E740
		public PropertyDescriptorCollection(PropertyDescriptor[] properties)
		{
			this.properties = properties;
			if (properties == null)
			{
				this.properties = new PropertyDescriptor[0];
				this.propCount = 0;
			}
			else
			{
				this.propCount = properties.Length;
			}
			this.propsOwned = true;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0001F77E File Offset: 0x0001E77E
		public PropertyDescriptorCollection(PropertyDescriptor[] properties, bool readOnly)
			: this(properties)
		{
			this.readOnly = readOnly;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0001F790 File Offset: 0x0001E790
		private PropertyDescriptorCollection(PropertyDescriptor[] properties, int propCount, string[] namedSort, IComparer comparer)
		{
			this.propsOwned = false;
			if (namedSort != null)
			{
				this.namedSort = (string[])namedSort.Clone();
			}
			this.comparer = comparer;
			this.properties = properties;
			this.propCount = propCount;
			this.needSort = true;
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x0001F7E2 File Offset: 0x0001E7E2
		public int Count
		{
			get
			{
				return this.propCount;
			}
		}

		// Token: 0x170001F8 RID: 504
		public virtual PropertyDescriptor this[int index]
		{
			get
			{
				if (index >= this.propCount)
				{
					throw new IndexOutOfRangeException();
				}
				this.EnsurePropsOwned();
				return this.properties[index];
			}
		}

		// Token: 0x170001F9 RID: 505
		public virtual PropertyDescriptor this[string name]
		{
			get
			{
				return this.Find(name, false);
			}
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0001F814 File Offset: 0x0001E814
		public int Add(PropertyDescriptor value)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			this.EnsureSize(this.propCount + 1);
			this.properties[this.propCount++] = value;
			return this.propCount - 1;
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0001F85E File Offset: 0x0001E85E
		public void Clear()
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			this.propCount = 0;
			this.cachedFoundProperties = null;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0001F87C File Offset: 0x0001E87C
		public bool Contains(PropertyDescriptor value)
		{
			return this.IndexOf(value) >= 0;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0001F88B File Offset: 0x0001E88B
		public void CopyTo(Array array, int index)
		{
			this.EnsurePropsOwned();
			Array.Copy(this.properties, 0, array, index, this.Count);
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0001F8A8 File Offset: 0x0001E8A8
		private void EnsurePropsOwned()
		{
			if (!this.propsOwned)
			{
				this.propsOwned = true;
				if (this.properties != null)
				{
					PropertyDescriptor[] array = new PropertyDescriptor[this.Count];
					Array.Copy(this.properties, 0, array, 0, this.Count);
					this.properties = array;
				}
			}
			if (this.needSort)
			{
				this.needSort = false;
				this.InternalSort(this.namedSort);
			}
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0001F910 File Offset: 0x0001E910
		private void EnsureSize(int sizeNeeded)
		{
			if (sizeNeeded <= this.properties.Length)
			{
				return;
			}
			if (this.properties == null || this.properties.Length == 0)
			{
				this.propCount = 0;
				this.properties = new PropertyDescriptor[sizeNeeded];
				return;
			}
			this.EnsurePropsOwned();
			int num = Math.Max(sizeNeeded, this.properties.Length * 2);
			PropertyDescriptor[] array = new PropertyDescriptor[num];
			Array.Copy(this.properties, 0, array, 0, this.propCount);
			this.properties = array;
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0001F988 File Offset: 0x0001E988
		public virtual PropertyDescriptor Find(string name, bool ignoreCase)
		{
			PropertyDescriptor propertyDescriptor2;
			lock (this)
			{
				PropertyDescriptor propertyDescriptor = null;
				if (this.cachedFoundProperties == null || this.cachedIgnoreCase != ignoreCase)
				{
					this.cachedIgnoreCase = ignoreCase;
					this.cachedFoundProperties = new HybridDictionary(ignoreCase);
				}
				object obj = this.cachedFoundProperties[name];
				if (obj != null)
				{
					propertyDescriptor2 = (PropertyDescriptor)obj;
				}
				else
				{
					for (int i = 0; i < this.propCount; i++)
					{
						if (ignoreCase)
						{
							if (string.Equals(this.properties[i].Name, name, StringComparison.OrdinalIgnoreCase))
							{
								this.cachedFoundProperties[name] = this.properties[i];
								propertyDescriptor = this.properties[i];
								break;
							}
						}
						else if (this.properties[i].Name.Equals(name))
						{
							this.cachedFoundProperties[name] = this.properties[i];
							propertyDescriptor = this.properties[i];
							break;
						}
					}
					propertyDescriptor2 = propertyDescriptor;
				}
			}
			return propertyDescriptor2;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0001FA7C File Offset: 0x0001EA7C
		public int IndexOf(PropertyDescriptor value)
		{
			return Array.IndexOf<PropertyDescriptor>(this.properties, value, 0, this.propCount);
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0001FA94 File Offset: 0x0001EA94
		public void Insert(int index, PropertyDescriptor value)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			this.EnsureSize(this.propCount + 1);
			if (index < this.propCount)
			{
				Array.Copy(this.properties, index, this.properties, index + 1, this.propCount - index);
			}
			this.properties[index] = value;
			this.propCount++;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0001FAFC File Offset: 0x0001EAFC
		public void Remove(PropertyDescriptor value)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			int num = this.IndexOf(value);
			if (num != -1)
			{
				this.RemoveAt(num);
			}
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0001FB2C File Offset: 0x0001EB2C
		public void RemoveAt(int index)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			if (index < this.propCount - 1)
			{
				Array.Copy(this.properties, index + 1, this.properties, index, this.propCount - index - 1);
			}
			this.properties[this.propCount - 1] = null;
			this.propCount--;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0001FB8F File Offset: 0x0001EB8F
		public virtual PropertyDescriptorCollection Sort()
		{
			return new PropertyDescriptorCollection(this.properties, this.propCount, this.namedSort, this.comparer);
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0001FBAE File Offset: 0x0001EBAE
		public virtual PropertyDescriptorCollection Sort(string[] names)
		{
			return new PropertyDescriptorCollection(this.properties, this.propCount, names, this.comparer);
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0001FBC8 File Offset: 0x0001EBC8
		public virtual PropertyDescriptorCollection Sort(string[] names, IComparer comparer)
		{
			return new PropertyDescriptorCollection(this.properties, this.propCount, names, comparer);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0001FBDD File Offset: 0x0001EBDD
		public virtual PropertyDescriptorCollection Sort(IComparer comparer)
		{
			return new PropertyDescriptorCollection(this.properties, this.propCount, this.namedSort, comparer);
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0001FBF8 File Offset: 0x0001EBF8
		protected void InternalSort(string[] names)
		{
			if (this.properties == null || this.properties.Length == 0)
			{
				return;
			}
			this.InternalSort(this.comparer);
			if (names != null && names.Length > 0)
			{
				ArrayList arrayList = new ArrayList(this.properties);
				int num = 0;
				int num2 = this.properties.Length;
				for (int i = 0; i < names.Length; i++)
				{
					for (int j = 0; j < num2; j++)
					{
						PropertyDescriptor propertyDescriptor = (PropertyDescriptor)arrayList[j];
						if (propertyDescriptor != null && propertyDescriptor.Name.Equals(names[i]))
						{
							this.properties[num++] = propertyDescriptor;
							arrayList[j] = null;
							break;
						}
					}
				}
				for (int k = 0; k < num2; k++)
				{
					if (arrayList[k] != null)
					{
						this.properties[num++] = (PropertyDescriptor)arrayList[k];
					}
				}
			}
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0001FCD8 File Offset: 0x0001ECD8
		protected void InternalSort(IComparer sorter)
		{
			if (sorter == null)
			{
				TypeDescriptor.SortDescriptorArray(this);
				return;
			}
			Array.Sort(this.properties, sorter);
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0001FCF0 File Offset: 0x0001ECF0
		public virtual IEnumerator GetEnumerator()
		{
			this.EnsurePropsOwned();
			if (this.properties.Length != this.propCount)
			{
				PropertyDescriptor[] array = new PropertyDescriptor[this.propCount];
				Array.Copy(this.properties, 0, array, 0, this.propCount);
				return array.GetEnumerator();
			}
			return this.properties.GetEnumerator();
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x0001FD45 File Offset: 0x0001ED45
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x0001FD4D File Offset: 0x0001ED4D
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x0001FD50 File Offset: 0x0001ED50
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0001FD54 File Offset: 0x0001ED54
		void IDictionary.Add(object key, object value)
		{
			PropertyDescriptor propertyDescriptor = value as PropertyDescriptor;
			if (propertyDescriptor == null)
			{
				throw new ArgumentException("value");
			}
			this.Add(propertyDescriptor);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0001FD7E File Offset: 0x0001ED7E
		void IDictionary.Clear()
		{
			this.Clear();
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0001FD86 File Offset: 0x0001ED86
		bool IDictionary.Contains(object key)
		{
			return key is string && this[(string)key] != null;
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0001FDA4 File Offset: 0x0001EDA4
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new PropertyDescriptorCollection.PropertyDescriptorEnumerator(this);
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x0001FDAC File Offset: 0x0001EDAC
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.readOnly;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x0001FDB4 File Offset: 0x0001EDB4
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.readOnly;
			}
		}

		// Token: 0x170001FF RID: 511
		object IDictionary.this[object key]
		{
			get
			{
				if (key is string)
				{
					return this[(string)key];
				}
				return null;
			}
			set
			{
				if (this.readOnly)
				{
					throw new NotSupportedException();
				}
				if (value != null && !(value is PropertyDescriptor))
				{
					throw new ArgumentException("value");
				}
				int num = -1;
				if (key is int)
				{
					num = (int)key;
					if (num < 0 || num >= this.propCount)
					{
						throw new IndexOutOfRangeException();
					}
				}
				else
				{
					if (!(key is string))
					{
						throw new ArgumentException("key");
					}
					for (int i = 0; i < this.propCount; i++)
					{
						if (this.properties[i].Name.Equals((string)key))
						{
							num = i;
							break;
						}
					}
				}
				if (num == -1)
				{
					this.Add((PropertyDescriptor)value);
					return;
				}
				this.EnsurePropsOwned();
				this.properties[num] = (PropertyDescriptor)value;
				if (this.cachedFoundProperties != null && key is string)
				{
					this.cachedFoundProperties[key] = value;
				}
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x0001FEB0 File Offset: 0x0001EEB0
		ICollection IDictionary.Keys
		{
			get
			{
				string[] array = new string[this.propCount];
				for (int i = 0; i < this.propCount; i++)
				{
					array[i] = this.properties[i].Name;
				}
				return array;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x0001FEEC File Offset: 0x0001EEEC
		ICollection IDictionary.Values
		{
			get
			{
				if (this.properties.Length != this.propCount)
				{
					PropertyDescriptor[] array = new PropertyDescriptor[this.propCount];
					Array.Copy(this.properties, 0, array, 0, this.propCount);
					return array;
				}
				return (ICollection)this.properties.Clone();
			}
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0001FF3C File Offset: 0x0001EF3C
		void IDictionary.Remove(object key)
		{
			if (key is string)
			{
				PropertyDescriptor propertyDescriptor = this[(string)key];
				if (propertyDescriptor != null)
				{
					((IList)this).Remove(propertyDescriptor);
				}
			}
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0001FF68 File Offset: 0x0001EF68
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0001FF70 File Offset: 0x0001EF70
		int IList.Add(object value)
		{
			return this.Add((PropertyDescriptor)value);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0001FF7E File Offset: 0x0001EF7E
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0001FF86 File Offset: 0x0001EF86
		bool IList.Contains(object value)
		{
			return this.Contains((PropertyDescriptor)value);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0001FF94 File Offset: 0x0001EF94
		int IList.IndexOf(object value)
		{
			return this.IndexOf((PropertyDescriptor)value);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0001FFA2 File Offset: 0x0001EFA2
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (PropertyDescriptor)value);
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x0001FFB1 File Offset: 0x0001EFB1
		bool IList.IsReadOnly
		{
			get
			{
				return this.readOnly;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0001FFB9 File Offset: 0x0001EFB9
		bool IList.IsFixedSize
		{
			get
			{
				return this.readOnly;
			}
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0001FFC1 File Offset: 0x0001EFC1
		void IList.Remove(object value)
		{
			this.Remove((PropertyDescriptor)value);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0001FFCF File Offset: 0x0001EFCF
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x17000204 RID: 516
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (this.readOnly)
				{
					throw new NotSupportedException();
				}
				if (index >= this.propCount)
				{
					throw new IndexOutOfRangeException();
				}
				if (value != null && !(value is PropertyDescriptor))
				{
					throw new ArgumentException("value");
				}
				this.EnsurePropsOwned();
				this.properties[index] = (PropertyDescriptor)value;
			}
		}

		// Token: 0x04000A10 RID: 2576
		public static readonly PropertyDescriptorCollection Empty = new PropertyDescriptorCollection(null, true);

		// Token: 0x04000A11 RID: 2577
		private IDictionary cachedFoundProperties;

		// Token: 0x04000A12 RID: 2578
		private bool cachedIgnoreCase;

		// Token: 0x04000A13 RID: 2579
		private PropertyDescriptor[] properties;

		// Token: 0x04000A14 RID: 2580
		private int propCount;

		// Token: 0x04000A15 RID: 2581
		private string[] namedSort;

		// Token: 0x04000A16 RID: 2582
		private IComparer comparer;

		// Token: 0x04000A17 RID: 2583
		private bool propsOwned = true;

		// Token: 0x04000A18 RID: 2584
		private bool needSort;

		// Token: 0x04000A19 RID: 2585
		private bool readOnly;

		// Token: 0x0200012D RID: 301
		private class PropertyDescriptorEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x060009A9 RID: 2473 RVA: 0x00020046 File Offset: 0x0001F046
			public PropertyDescriptorEnumerator(PropertyDescriptorCollection owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000205 RID: 517
			// (get) Token: 0x060009AA RID: 2474 RVA: 0x0002005C File Offset: 0x0001F05C
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x17000206 RID: 518
			// (get) Token: 0x060009AB RID: 2475 RVA: 0x0002006C File Offset: 0x0001F06C
			public DictionaryEntry Entry
			{
				get
				{
					PropertyDescriptor propertyDescriptor = this.owner[this.index];
					return new DictionaryEntry(propertyDescriptor.Name, propertyDescriptor);
				}
			}

			// Token: 0x17000207 RID: 519
			// (get) Token: 0x060009AC RID: 2476 RVA: 0x00020097 File Offset: 0x0001F097
			public object Key
			{
				get
				{
					return this.owner[this.index].Name;
				}
			}

			// Token: 0x17000208 RID: 520
			// (get) Token: 0x060009AD RID: 2477 RVA: 0x000200AF File Offset: 0x0001F0AF
			public object Value
			{
				get
				{
					return this.owner[this.index].Name;
				}
			}

			// Token: 0x060009AE RID: 2478 RVA: 0x000200C7 File Offset: 0x0001F0C7
			public bool MoveNext()
			{
				if (this.index < this.owner.Count - 1)
				{
					this.index++;
					return true;
				}
				return false;
			}

			// Token: 0x060009AF RID: 2479 RVA: 0x000200EF File Offset: 0x0001F0EF
			public void Reset()
			{
				this.index = -1;
			}

			// Token: 0x04000A1A RID: 2586
			private PropertyDescriptorCollection owner;

			// Token: 0x04000A1B RID: 2587
			private int index = -1;
		}
	}
}
