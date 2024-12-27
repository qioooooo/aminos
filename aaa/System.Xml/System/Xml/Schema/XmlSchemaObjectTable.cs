using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Xml.Schema
{
	// Token: 0x0200026A RID: 618
	public class XmlSchemaObjectTable
	{
		// Token: 0x06001CC2 RID: 7362 RVA: 0x00083690 File Offset: 0x00082690
		internal XmlSchemaObjectTable()
		{
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x000836AE File Offset: 0x000826AE
		internal void Add(XmlQualifiedName name, XmlSchemaObject value)
		{
			this.table.Add(name, value);
			this.entries.Add(new XmlSchemaObjectTable.XmlSchemaObjectEntry(name, value));
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x000836D0 File Offset: 0x000826D0
		internal void Insert(XmlQualifiedName name, XmlSchemaObject value)
		{
			XmlSchemaObject xmlSchemaObject = null;
			if (this.table.TryGetValue(name, out xmlSchemaObject))
			{
				this.table[name] = value;
				int num = this.FindIndexByValue(xmlSchemaObject);
				this.entries[num] = new XmlSchemaObjectTable.XmlSchemaObjectEntry(name, value);
				return;
			}
			this.Add(name, value);
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x00083720 File Offset: 0x00082720
		internal void Replace(XmlQualifiedName name, XmlSchemaObject value)
		{
			XmlSchemaObject xmlSchemaObject;
			if (this.table.TryGetValue(name, out xmlSchemaObject))
			{
				this.table[name] = value;
				int num = this.FindIndexByValue(xmlSchemaObject);
				this.entries[num] = new XmlSchemaObjectTable.XmlSchemaObjectEntry(name, value);
			}
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x00083765 File Offset: 0x00082765
		internal void Clear()
		{
			this.table.Clear();
			this.entries.Clear();
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x00083780 File Offset: 0x00082780
		internal void Remove(XmlQualifiedName name)
		{
			XmlSchemaObject xmlSchemaObject;
			if (this.table.TryGetValue(name, out xmlSchemaObject))
			{
				this.table.Remove(name);
				int num = this.FindIndexByValue(xmlSchemaObject);
				this.entries.RemoveAt(num);
			}
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x000837C0 File Offset: 0x000827C0
		private int FindIndexByValue(XmlSchemaObject xso)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].xso == xso)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06001CC9 RID: 7369 RVA: 0x000837FA File Offset: 0x000827FA
		public int Count
		{
			get
			{
				return this.table.Count;
			}
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x00083807 File Offset: 0x00082807
		public bool Contains(XmlQualifiedName name)
		{
			return this.table.ContainsKey(name);
		}

		// Token: 0x17000769 RID: 1897
		public XmlSchemaObject this[XmlQualifiedName name]
		{
			get
			{
				XmlSchemaObject xmlSchemaObject;
				if (this.table.TryGetValue(name, out xmlSchemaObject))
				{
					return xmlSchemaObject;
				}
				return null;
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06001CCC RID: 7372 RVA: 0x00083838 File Offset: 0x00082838
		public ICollection Names
		{
			get
			{
				return new XmlSchemaObjectTable.NamesCollection(this.entries, this.table.Count);
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06001CCD RID: 7373 RVA: 0x00083850 File Offset: 0x00082850
		public ICollection Values
		{
			get
			{
				return new XmlSchemaObjectTable.ValuesCollection(this.entries, this.table.Count);
			}
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x00083868 File Offset: 0x00082868
		public IDictionaryEnumerator GetEnumerator()
		{
			return new XmlSchemaObjectTable.XSODictionaryEnumerator(this.entries, this.table.Count, XmlSchemaObjectTable.EnumeratorType.DictionaryEntry);
		}

		// Token: 0x040011A3 RID: 4515
		private Dictionary<XmlQualifiedName, XmlSchemaObject> table = new Dictionary<XmlQualifiedName, XmlSchemaObject>();

		// Token: 0x040011A4 RID: 4516
		private List<XmlSchemaObjectTable.XmlSchemaObjectEntry> entries = new List<XmlSchemaObjectTable.XmlSchemaObjectEntry>();

		// Token: 0x0200026B RID: 619
		internal enum EnumeratorType
		{
			// Token: 0x040011A6 RID: 4518
			Keys,
			// Token: 0x040011A7 RID: 4519
			Values,
			// Token: 0x040011A8 RID: 4520
			DictionaryEntry
		}

		// Token: 0x0200026C RID: 620
		internal struct XmlSchemaObjectEntry
		{
			// Token: 0x06001CCF RID: 7375 RVA: 0x00083881 File Offset: 0x00082881
			public XmlSchemaObjectEntry(XmlQualifiedName name, XmlSchemaObject value)
			{
				this.qname = name;
				this.xso = value;
			}

			// Token: 0x06001CD0 RID: 7376 RVA: 0x00083891 File Offset: 0x00082891
			public XmlSchemaObject IsMatch(string localName, string ns)
			{
				if (localName == this.qname.Name && ns == this.qname.Namespace)
				{
					return this.xso;
				}
				return null;
			}

			// Token: 0x06001CD1 RID: 7377 RVA: 0x000838C1 File Offset: 0x000828C1
			public void Reset()
			{
				this.qname = null;
				this.xso = null;
			}

			// Token: 0x040011A9 RID: 4521
			internal XmlQualifiedName qname;

			// Token: 0x040011AA RID: 4522
			internal XmlSchemaObject xso;
		}

		// Token: 0x0200026D RID: 621
		internal class NamesCollection : ICollection, IEnumerable
		{
			// Token: 0x06001CD2 RID: 7378 RVA: 0x000838D1 File Offset: 0x000828D1
			internal NamesCollection(List<XmlSchemaObjectTable.XmlSchemaObjectEntry> entries, int size)
			{
				this.entries = entries;
				this.size = size;
			}

			// Token: 0x1700076C RID: 1900
			// (get) Token: 0x06001CD3 RID: 7379 RVA: 0x000838E7 File Offset: 0x000828E7
			public int Count
			{
				get
				{
					return this.size;
				}
			}

			// Token: 0x1700076D RID: 1901
			// (get) Token: 0x06001CD4 RID: 7380 RVA: 0x000838EF File Offset: 0x000828EF
			public object SyncRoot
			{
				get
				{
					return ((ICollection)this.entries).SyncRoot;
				}
			}

			// Token: 0x1700076E RID: 1902
			// (get) Token: 0x06001CD5 RID: 7381 RVA: 0x000838FC File Offset: 0x000828FC
			public bool IsSynchronized
			{
				get
				{
					return ((ICollection)this.entries).IsSynchronized;
				}
			}

			// Token: 0x06001CD6 RID: 7382 RVA: 0x0008390C File Offset: 0x0008290C
			public void CopyTo(Array array, int arrayIndex)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException("arrayIndex");
				}
				for (int i = 0; i < this.size; i++)
				{
					array.SetValue(this.entries[i].qname, arrayIndex++);
				}
			}

			// Token: 0x06001CD7 RID: 7383 RVA: 0x00083964 File Offset: 0x00082964
			public IEnumerator GetEnumerator()
			{
				return new XmlSchemaObjectTable.XSOEnumerator(this.entries, this.size, XmlSchemaObjectTable.EnumeratorType.Keys);
			}

			// Token: 0x040011AB RID: 4523
			private List<XmlSchemaObjectTable.XmlSchemaObjectEntry> entries;

			// Token: 0x040011AC RID: 4524
			private int size;
		}

		// Token: 0x0200026E RID: 622
		internal class ValuesCollection : ICollection, IEnumerable
		{
			// Token: 0x06001CD8 RID: 7384 RVA: 0x00083978 File Offset: 0x00082978
			internal ValuesCollection(List<XmlSchemaObjectTable.XmlSchemaObjectEntry> entries, int size)
			{
				this.entries = entries;
				this.size = size;
			}

			// Token: 0x1700076F RID: 1903
			// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x0008398E File Offset: 0x0008298E
			public int Count
			{
				get
				{
					return this.size;
				}
			}

			// Token: 0x17000770 RID: 1904
			// (get) Token: 0x06001CDA RID: 7386 RVA: 0x00083996 File Offset: 0x00082996
			public object SyncRoot
			{
				get
				{
					return ((ICollection)this.entries).SyncRoot;
				}
			}

			// Token: 0x17000771 RID: 1905
			// (get) Token: 0x06001CDB RID: 7387 RVA: 0x000839A3 File Offset: 0x000829A3
			public bool IsSynchronized
			{
				get
				{
					return ((ICollection)this.entries).IsSynchronized;
				}
			}

			// Token: 0x06001CDC RID: 7388 RVA: 0x000839B0 File Offset: 0x000829B0
			public void CopyTo(Array array, int arrayIndex)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException("arrayIndex");
				}
				for (int i = 0; i < this.size; i++)
				{
					array.SetValue(this.entries[i].xso, arrayIndex++);
				}
			}

			// Token: 0x06001CDD RID: 7389 RVA: 0x00083A08 File Offset: 0x00082A08
			public IEnumerator GetEnumerator()
			{
				return new XmlSchemaObjectTable.XSOEnumerator(this.entries, this.size, XmlSchemaObjectTable.EnumeratorType.Values);
			}

			// Token: 0x040011AD RID: 4525
			private List<XmlSchemaObjectTable.XmlSchemaObjectEntry> entries;

			// Token: 0x040011AE RID: 4526
			private int size;
		}

		// Token: 0x0200026F RID: 623
		internal class XSOEnumerator : IEnumerator
		{
			// Token: 0x06001CDE RID: 7390 RVA: 0x00083A1C File Offset: 0x00082A1C
			internal XSOEnumerator(List<XmlSchemaObjectTable.XmlSchemaObjectEntry> entries, int size, XmlSchemaObjectTable.EnumeratorType enumType)
			{
				this.entries = entries;
				this.size = size;
				this.enumType = enumType;
				this.currentIndex = -1;
			}

			// Token: 0x17000772 RID: 1906
			// (get) Token: 0x06001CDF RID: 7391 RVA: 0x00083A40 File Offset: 0x00082A40
			public object Current
			{
				get
				{
					if (this.currentIndex == -1)
					{
						throw new InvalidOperationException(Res.GetString("Sch_EnumNotStarted", new object[] { string.Empty }));
					}
					if (this.currentIndex >= this.size)
					{
						throw new InvalidOperationException(Res.GetString("Sch_EnumFinished", new object[] { string.Empty }));
					}
					switch (this.enumType)
					{
					case XmlSchemaObjectTable.EnumeratorType.Keys:
						return this.currentKey;
					case XmlSchemaObjectTable.EnumeratorType.Values:
						return this.currentValue;
					case XmlSchemaObjectTable.EnumeratorType.DictionaryEntry:
						return new DictionaryEntry(this.currentKey, this.currentValue);
					default:
						return null;
					}
				}
			}

			// Token: 0x06001CE0 RID: 7392 RVA: 0x00083AE8 File Offset: 0x00082AE8
			public bool MoveNext()
			{
				if (this.currentIndex >= this.size - 1)
				{
					this.currentValue = null;
					this.currentKey = null;
					return false;
				}
				this.currentIndex++;
				this.currentValue = this.entries[this.currentIndex].xso;
				this.currentKey = this.entries[this.currentIndex].qname;
				return true;
			}

			// Token: 0x06001CE1 RID: 7393 RVA: 0x00083B5C File Offset: 0x00082B5C
			public void Reset()
			{
				this.currentIndex = -1;
				this.currentValue = null;
				this.currentKey = null;
			}

			// Token: 0x040011AF RID: 4527
			private List<XmlSchemaObjectTable.XmlSchemaObjectEntry> entries;

			// Token: 0x040011B0 RID: 4528
			private XmlSchemaObjectTable.EnumeratorType enumType;

			// Token: 0x040011B1 RID: 4529
			protected int currentIndex;

			// Token: 0x040011B2 RID: 4530
			protected int size;

			// Token: 0x040011B3 RID: 4531
			protected XmlQualifiedName currentKey;

			// Token: 0x040011B4 RID: 4532
			protected XmlSchemaObject currentValue;
		}

		// Token: 0x02000270 RID: 624
		internal class XSODictionaryEnumerator : XmlSchemaObjectTable.XSOEnumerator, IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06001CE2 RID: 7394 RVA: 0x00083B73 File Offset: 0x00082B73
			internal XSODictionaryEnumerator(List<XmlSchemaObjectTable.XmlSchemaObjectEntry> entries, int size, XmlSchemaObjectTable.EnumeratorType enumType)
				: base(entries, size, enumType)
			{
			}

			// Token: 0x17000773 RID: 1907
			// (get) Token: 0x06001CE3 RID: 7395 RVA: 0x00083B80 File Offset: 0x00082B80
			public DictionaryEntry Entry
			{
				get
				{
					if (this.currentIndex == -1)
					{
						throw new InvalidOperationException(Res.GetString("Sch_EnumNotStarted", new object[] { string.Empty }));
					}
					if (this.currentIndex >= this.size)
					{
						throw new InvalidOperationException(Res.GetString("Sch_EnumFinished", new object[] { string.Empty }));
					}
					return new DictionaryEntry(this.currentKey, this.currentValue);
				}
			}

			// Token: 0x17000774 RID: 1908
			// (get) Token: 0x06001CE4 RID: 7396 RVA: 0x00083BF8 File Offset: 0x00082BF8
			public object Key
			{
				get
				{
					if (this.currentIndex == -1)
					{
						throw new InvalidOperationException(Res.GetString("Sch_EnumNotStarted", new object[] { string.Empty }));
					}
					if (this.currentIndex >= this.size)
					{
						throw new InvalidOperationException(Res.GetString("Sch_EnumFinished", new object[] { string.Empty }));
					}
					return this.currentKey;
				}
			}

			// Token: 0x17000775 RID: 1909
			// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x00083C64 File Offset: 0x00082C64
			public object Value
			{
				get
				{
					if (this.currentIndex == -1)
					{
						throw new InvalidOperationException(Res.GetString("Sch_EnumNotStarted", new object[] { string.Empty }));
					}
					if (this.currentIndex >= this.size)
					{
						throw new InvalidOperationException(Res.GetString("Sch_EnumFinished", new object[] { string.Empty }));
					}
					return this.currentValue;
				}
			}
		}
	}
}
