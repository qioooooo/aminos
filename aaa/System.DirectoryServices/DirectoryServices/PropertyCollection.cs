using System;
using System.Collections;
using System.DirectoryServices.Interop;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.DirectoryServices
{
	// Token: 0x0200002E RID: 46
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class PropertyCollection : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06000141 RID: 321 RVA: 0x000060A8 File Offset: 0x000050A8
		internal PropertyCollection(DirectoryEntry entry)
		{
			this.entry = entry;
			Hashtable hashtable = new Hashtable();
			this.valueTable = Hashtable.Synchronized(hashtable);
		}

		// Token: 0x1700004E RID: 78
		public PropertyValueCollection this[string propertyName]
		{
			get
			{
				if (propertyName == null)
				{
					throw new ArgumentNullException("propertyName");
				}
				string text = propertyName.ToLower(CultureInfo.InvariantCulture);
				if (this.valueTable.Contains(text))
				{
					return (PropertyValueCollection)this.valueTable[text];
				}
				PropertyValueCollection propertyValueCollection = new PropertyValueCollection(this.entry, propertyName);
				this.valueTable.Add(text, propertyValueCollection);
				return propertyValueCollection;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00006138 File Offset: 0x00005138
		public int Count
		{
			get
			{
				if (!(this.entry.AdsObject is UnsafeNativeMethods.IAdsPropertyList))
				{
					throw new NotSupportedException(Res.GetString("DSCannotCount"));
				}
				this.entry.FillCache("");
				UnsafeNativeMethods.IAdsPropertyList adsPropertyList = (UnsafeNativeMethods.IAdsPropertyList)this.entry.AdsObject;
				return adsPropertyList.PropertyCount;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000618E File Offset: 0x0000518E
		public ICollection PropertyNames
		{
			get
			{
				return new PropertyCollection.KeysCollection(this);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00006196 File Offset: 0x00005196
		public ICollection Values
		{
			get
			{
				return new PropertyCollection.ValuesCollection(this);
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000061A0 File Offset: 0x000051A0
		public bool Contains(string propertyName)
		{
			object obj;
			int ex = this.entry.AdsObject.GetEx(propertyName, out obj);
			if (ex == 0)
			{
				return true;
			}
			if (ex == -2147463155 || ex == -2147463162)
			{
				return false;
			}
			throw COMExceptionHelper.CreateFormattedComException(ex);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000061DE File Offset: 0x000051DE
		public void CopyTo(PropertyValueCollection[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000061E8 File Offset: 0x000051E8
		public IDictionaryEnumerator GetEnumerator()
		{
			if (!(this.entry.AdsObject is UnsafeNativeMethods.IAdsPropertyList))
			{
				throw new NotSupportedException(Res.GetString("DSCannotEmunerate"));
			}
			DirectoryEntry directoryEntry = this.entry.CloneBrowsable();
			directoryEntry.FillCache("");
			UnsafeNativeMethods.IAdsPropertyList adsPropertyList = (UnsafeNativeMethods.IAdsPropertyList)directoryEntry.AdsObject;
			directoryEntry.propertiesAlreadyEnumerated = true;
			return new PropertyCollection.PropertyEnumerator(this.entry, directoryEntry);
		}

		// Token: 0x17000052 RID: 82
		object IDictionary.this[object key]
		{
			get
			{
				return this[(string)key];
			}
			set
			{
				throw new NotSupportedException(Res.GetString("DSPropertySetSupported"));
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000626C File Offset: 0x0000526C
		bool IDictionary.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000626F File Offset: 0x0000526F
		bool IDictionary.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00006272 File Offset: 0x00005272
		ICollection IDictionary.Keys
		{
			get
			{
				return new PropertyCollection.KeysCollection(this);
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000627A File Offset: 0x0000527A
		void IDictionary.Add(object key, object value)
		{
			throw new NotSupportedException(Res.GetString("DSAddNotSupported"));
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000628B File Offset: 0x0000528B
		void IDictionary.Clear()
		{
			throw new NotSupportedException(Res.GetString("DSClearNotSupported"));
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000629C File Offset: 0x0000529C
		bool IDictionary.Contains(object value)
		{
			return this.Contains((string)value);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000062AA File Offset: 0x000052AA
		void IDictionary.Remove(object key)
		{
			throw new NotSupportedException(Res.GetString("DSRemoveNotSupported"));
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000062BB File Offset: 0x000052BB
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000062C3 File Offset: 0x000052C3
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000062C6 File Offset: 0x000052C6
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000062CC File Offset: 0x000052CC
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(Res.GetString("OnlyAllowSingleDimension"), "array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(Res.GetString("LessThanZero"), "index");
			}
			if (index + this.Count > array.Length || index + this.Count < index)
			{
				throw new ArgumentException(Res.GetString("DestinationArrayNotLargeEnough"));
			}
			foreach (object obj in this)
			{
				PropertyValueCollection propertyValueCollection = (PropertyValueCollection)obj;
				array.SetValue(propertyValueCollection, index);
				index++;
			}
		}

		// Token: 0x040001C2 RID: 450
		private DirectoryEntry entry;

		// Token: 0x040001C3 RID: 451
		internal Hashtable valueTable;

		// Token: 0x0200002F RID: 47
		private class PropertyEnumerator : IDictionaryEnumerator, IEnumerator, IDisposable
		{
			// Token: 0x06000156 RID: 342 RVA: 0x00006398 File Offset: 0x00005398
			public PropertyEnumerator(DirectoryEntry parent, DirectoryEntry clone)
			{
				this.entry = clone;
				this.parentEntry = parent;
			}

			// Token: 0x06000157 RID: 343 RVA: 0x000063B0 File Offset: 0x000053B0
			~PropertyEnumerator()
			{
				this.Dispose(true);
			}

			// Token: 0x06000158 RID: 344 RVA: 0x000063E0 File Offset: 0x000053E0
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06000159 RID: 345 RVA: 0x000063EF File Offset: 0x000053EF
			protected virtual void Dispose(bool disposing)
			{
				if (disposing)
				{
					this.entry.Dispose();
				}
			}

			// Token: 0x17000058 RID: 88
			// (get) Token: 0x0600015A RID: 346 RVA: 0x00006400 File Offset: 0x00005400
			public object Current
			{
				get
				{
					return this.Entry.Value;
				}
			}

			// Token: 0x17000059 RID: 89
			// (get) Token: 0x0600015B RID: 347 RVA: 0x0000641B File Offset: 0x0000541B
			public DictionaryEntry Entry
			{
				get
				{
					if (this.currentPropName == null)
					{
						throw new InvalidOperationException(Res.GetString("DSNoCurrentProperty"));
					}
					return new DictionaryEntry(this.currentPropName, new PropertyValueCollection(this.parentEntry, this.currentPropName));
				}
			}

			// Token: 0x1700005A RID: 90
			// (get) Token: 0x0600015C RID: 348 RVA: 0x00006454 File Offset: 0x00005454
			public object Key
			{
				get
				{
					return this.Entry.Key;
				}
			}

			// Token: 0x1700005B RID: 91
			// (get) Token: 0x0600015D RID: 349 RVA: 0x00006470 File Offset: 0x00005470
			public object Value
			{
				get
				{
					return this.Entry.Value;
				}
			}

			// Token: 0x0600015E RID: 350 RVA: 0x0000648C File Offset: 0x0000548C
			public bool MoveNext()
			{
				int num = 0;
				object obj;
				try
				{
					num = ((UnsafeNativeMethods.IAdsPropertyList)this.entry.AdsObject).Next(out obj);
				}
				catch (COMException ex)
				{
					num = ex.ErrorCode;
					obj = null;
				}
				if (num == 0)
				{
					if (obj != null)
					{
						this.currentPropName = ((UnsafeNativeMethods.IAdsPropertyEntry)obj).Name;
					}
					else
					{
						this.currentPropName = null;
					}
					return true;
				}
				this.currentPropName = null;
				return false;
			}

			// Token: 0x0600015F RID: 351 RVA: 0x000064FC File Offset: 0x000054FC
			public void Reset()
			{
				((UnsafeNativeMethods.IAdsPropertyList)this.entry.AdsObject).Reset();
				this.currentPropName = null;
			}

			// Token: 0x040001C4 RID: 452
			private DirectoryEntry entry;

			// Token: 0x040001C5 RID: 453
			private DirectoryEntry parentEntry;

			// Token: 0x040001C6 RID: 454
			private string currentPropName;
		}

		// Token: 0x02000030 RID: 48
		private class ValuesCollection : ICollection, IEnumerable
		{
			// Token: 0x06000160 RID: 352 RVA: 0x0000651A File Offset: 0x0000551A
			public ValuesCollection(PropertyCollection props)
			{
				this.props = props;
			}

			// Token: 0x1700005C RID: 92
			// (get) Token: 0x06000161 RID: 353 RVA: 0x00006529 File Offset: 0x00005529
			public int Count
			{
				get
				{
					return this.props.Count;
				}
			}

			// Token: 0x1700005D RID: 93
			// (get) Token: 0x06000162 RID: 354 RVA: 0x00006536 File Offset: 0x00005536
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700005E RID: 94
			// (get) Token: 0x06000163 RID: 355 RVA: 0x00006539 File Offset: 0x00005539
			public bool IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700005F RID: 95
			// (get) Token: 0x06000164 RID: 356 RVA: 0x0000653C File Offset: 0x0000553C
			public object SyncRoot
			{
				get
				{
					return ((ICollection)this.props).SyncRoot;
				}
			}

			// Token: 0x06000165 RID: 357 RVA: 0x0000654C File Offset: 0x0000554C
			public void CopyTo(Array array, int index)
			{
				foreach (object obj in this)
				{
					array.SetValue(obj, index++);
				}
			}

			// Token: 0x06000166 RID: 358 RVA: 0x000065A4 File Offset: 0x000055A4
			public virtual IEnumerator GetEnumerator()
			{
				return new PropertyCollection.ValuesEnumerator(this.props);
			}

			// Token: 0x040001C7 RID: 455
			protected PropertyCollection props;
		}

		// Token: 0x02000031 RID: 49
		private class KeysCollection : PropertyCollection.ValuesCollection
		{
			// Token: 0x06000167 RID: 359 RVA: 0x000065B1 File Offset: 0x000055B1
			public KeysCollection(PropertyCollection props)
				: base(props)
			{
			}

			// Token: 0x06000168 RID: 360 RVA: 0x000065BA File Offset: 0x000055BA
			public override IEnumerator GetEnumerator()
			{
				this.props.entry.FillCache("");
				return new PropertyCollection.KeysEnumerator(this.props);
			}
		}

		// Token: 0x02000032 RID: 50
		private class ValuesEnumerator : IEnumerator
		{
			// Token: 0x06000169 RID: 361 RVA: 0x000065DC File Offset: 0x000055DC
			public ValuesEnumerator(PropertyCollection propCollection)
			{
				this.propCollection = propCollection;
			}

			// Token: 0x17000060 RID: 96
			// (get) Token: 0x0600016A RID: 362 RVA: 0x000065F2 File Offset: 0x000055F2
			protected int CurrentIndex
			{
				get
				{
					if (this.currentIndex == -1)
					{
						throw new InvalidOperationException(Res.GetString("DSNoCurrentValue"));
					}
					return this.currentIndex;
				}
			}

			// Token: 0x17000061 RID: 97
			// (get) Token: 0x0600016B RID: 363 RVA: 0x00006614 File Offset: 0x00005614
			public virtual object Current
			{
				get
				{
					UnsafeNativeMethods.IAdsPropertyList adsPropertyList = (UnsafeNativeMethods.IAdsPropertyList)this.propCollection.entry.AdsObject;
					return this.propCollection[((UnsafeNativeMethods.IAdsPropertyEntry)adsPropertyList.Item(this.CurrentIndex)).Name];
				}
			}

			// Token: 0x0600016C RID: 364 RVA: 0x0000665D File Offset: 0x0000565D
			public bool MoveNext()
			{
				this.currentIndex++;
				if (this.currentIndex >= this.propCollection.Count)
				{
					this.currentIndex = -1;
					return false;
				}
				return true;
			}

			// Token: 0x0600016D RID: 365 RVA: 0x0000668A File Offset: 0x0000568A
			public void Reset()
			{
				this.currentIndex = -1;
			}

			// Token: 0x040001C8 RID: 456
			private int currentIndex = -1;

			// Token: 0x040001C9 RID: 457
			protected PropertyCollection propCollection;
		}

		// Token: 0x02000033 RID: 51
		private class KeysEnumerator : PropertyCollection.ValuesEnumerator
		{
			// Token: 0x0600016E RID: 366 RVA: 0x00006693 File Offset: 0x00005693
			public KeysEnumerator(PropertyCollection collection)
				: base(collection)
			{
			}

			// Token: 0x17000062 RID: 98
			// (get) Token: 0x0600016F RID: 367 RVA: 0x0000669C File Offset: 0x0000569C
			public override object Current
			{
				get
				{
					UnsafeNativeMethods.IAdsPropertyList adsPropertyList = (UnsafeNativeMethods.IAdsPropertyList)this.propCollection.entry.AdsObject;
					return ((UnsafeNativeMethods.IAdsPropertyEntry)adsPropertyList.Item(base.CurrentIndex)).Name;
				}
			}
		}
	}
}
