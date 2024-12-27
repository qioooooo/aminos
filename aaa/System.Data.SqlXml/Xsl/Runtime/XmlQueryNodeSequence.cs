using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000B8 RID: 184
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class XmlQueryNodeSequence : XmlQuerySequence<XPathNavigator>, IList<XPathItem>, ICollection<XPathItem>, IEnumerable<XPathItem>, IEnumerable
	{
		// Token: 0x060008F7 RID: 2295 RVA: 0x0002B397 File Offset: 0x0002A397
		public static XmlQueryNodeSequence CreateOrReuse(XmlQueryNodeSequence seq)
		{
			if (seq != null)
			{
				seq.Clear();
				return seq;
			}
			return new XmlQueryNodeSequence();
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0002B3A9 File Offset: 0x0002A3A9
		public static XmlQueryNodeSequence CreateOrReuse(XmlQueryNodeSequence seq, XPathNavigator navigator)
		{
			if (seq != null)
			{
				seq.Clear();
				seq.Add(navigator);
				return seq;
			}
			return new XmlQueryNodeSequence(navigator);
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0002B3C3 File Offset: 0x0002A3C3
		public XmlQueryNodeSequence()
		{
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0002B3CB File Offset: 0x0002A3CB
		public XmlQueryNodeSequence(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0002B3D4 File Offset: 0x0002A3D4
		public XmlQueryNodeSequence(IList<XPathNavigator> list)
			: base(list.Count)
		{
			for (int i = 0; i < list.Count; i++)
			{
				this.AddClone(list[i]);
			}
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0002B40B File Offset: 0x0002A40B
		public XmlQueryNodeSequence(XPathNavigator[] array, int size)
			: base(array, size)
		{
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0002B415 File Offset: 0x0002A415
		public XmlQueryNodeSequence(XPathNavigator navigator)
			: base(1)
		{
			this.AddClone(navigator);
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x0002B425 File Offset: 0x0002A425
		// (set) Token: 0x060008FF RID: 2303 RVA: 0x0002B43E File Offset: 0x0002A43E
		public bool IsDocOrderDistinct
		{
			get
			{
				return this.docOrderDistinct == this || base.Count <= 1;
			}
			set
			{
				this.docOrderDistinct = (value ? this : null);
			}
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0002B450 File Offset: 0x0002A450
		public XmlQueryNodeSequence DocOrderDistinct(IComparer<XPathNavigator> comparer)
		{
			if (this.docOrderDistinct != null)
			{
				return this.docOrderDistinct;
			}
			if (base.Count <= 1)
			{
				return this;
			}
			XPathNavigator[] array = new XPathNavigator[base.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = base[i];
			}
			Array.Sort<XPathNavigator>(array, 0, base.Count, comparer);
			int num = 0;
			for (int i = 1; i < array.Length; i++)
			{
				if (!array[num].IsSamePosition(array[i]))
				{
					num++;
					if (num != i)
					{
						array[num] = array[i];
					}
				}
			}
			this.docOrderDistinct = new XmlQueryNodeSequence(array, num + 1);
			this.docOrderDistinct.docOrderDistinct = this.docOrderDistinct;
			return this.docOrderDistinct;
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0002B4FA File Offset: 0x0002A4FA
		public void AddClone(XPathNavigator navigator)
		{
			base.Add(navigator.Clone());
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0002B508 File Offset: 0x0002A508
		protected override void OnItemsChanged()
		{
			this.docOrderDistinct = null;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0002B511 File Offset: 0x0002A511
		IEnumerator<XPathItem> IEnumerable<XPathItem>.GetEnumerator()
		{
			return new IListEnumerator<XPathItem>(this);
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x0002B51E File Offset: 0x0002A51E
		bool ICollection<XPathItem>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0002B521 File Offset: 0x0002A521
		void ICollection<XPathItem>.Add(XPathItem value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0002B528 File Offset: 0x0002A528
		void ICollection<XPathItem>.Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0002B52F File Offset: 0x0002A52F
		bool ICollection<XPathItem>.Contains(XPathItem value)
		{
			return base.IndexOf((XPathNavigator)value) != -1;
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0002B544 File Offset: 0x0002A544
		void ICollection<XPathItem>.CopyTo(XPathItem[] array, int index)
		{
			for (int i = 0; i < base.Count; i++)
			{
				array[index + i] = base[i];
			}
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0002B56E File Offset: 0x0002A56E
		bool ICollection<XPathItem>.Remove(XPathItem value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000158 RID: 344
		XPathItem IList<XPathItem>.this[int index]
		{
			get
			{
				if (index >= base.Count)
				{
					throw new ArgumentOutOfRangeException();
				}
				return base[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0002B594 File Offset: 0x0002A594
		int IList<XPathItem>.IndexOf(XPathItem value)
		{
			return base.IndexOf((XPathNavigator)value);
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0002B5A2 File Offset: 0x0002A5A2
		void IList<XPathItem>.Insert(int index, XPathItem value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0002B5A9 File Offset: 0x0002A5A9
		void IList<XPathItem>.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040005AB RID: 1451
		public new static readonly XmlQueryNodeSequence Empty = new XmlQueryNodeSequence();

		// Token: 0x040005AC RID: 1452
		private XmlQueryNodeSequence docOrderDistinct;
	}
}
