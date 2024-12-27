using System;
using System.Collections;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x02000091 RID: 145
	internal class CanonicalXmlNodeList : XmlNodeList, IList, ICollection, IEnumerable
	{
		// Token: 0x06000291 RID: 657 RVA: 0x0000E810 File Offset: 0x0000D810
		internal CanonicalXmlNodeList()
		{
			this.m_nodeArray = new ArrayList();
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000E823 File Offset: 0x0000D823
		public override XmlNode Item(int index)
		{
			return (XmlNode)this.m_nodeArray[index];
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000E836 File Offset: 0x0000D836
		public override IEnumerator GetEnumerator()
		{
			return this.m_nodeArray.GetEnumerator();
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000E843 File Offset: 0x0000D843
		public override int Count
		{
			get
			{
				return this.m_nodeArray.Count;
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000E850 File Offset: 0x0000D850
		public int Add(object value)
		{
			if (!(value is XmlNode))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_IncorrectObjectType"), "node");
			}
			return this.m_nodeArray.Add(value);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000E87B File Offset: 0x0000D87B
		public void Clear()
		{
			this.m_nodeArray.Clear();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000E888 File Offset: 0x0000D888
		public bool Contains(object value)
		{
			return this.m_nodeArray.Contains(value);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000E896 File Offset: 0x0000D896
		public int IndexOf(object value)
		{
			return this.m_nodeArray.IndexOf(value);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000E8A4 File Offset: 0x0000D8A4
		public void Insert(int index, object value)
		{
			if (!(value is XmlNode))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_IncorrectObjectType"), "value");
			}
			this.m_nodeArray.Insert(index, value);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000E8D0 File Offset: 0x0000D8D0
		public void Remove(object value)
		{
			this.m_nodeArray.Remove(value);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000E8DE File Offset: 0x0000D8DE
		public void RemoveAt(int index)
		{
			this.m_nodeArray.RemoveAt(index);
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000E8EC File Offset: 0x0000D8EC
		public bool IsFixedSize
		{
			get
			{
				return this.m_nodeArray.IsFixedSize;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000E8F9 File Offset: 0x0000D8F9
		public bool IsReadOnly
		{
			get
			{
				return this.m_nodeArray.IsReadOnly;
			}
		}

		// Token: 0x17000074 RID: 116
		object IList.this[int index]
		{
			get
			{
				return this.m_nodeArray[index];
			}
			set
			{
				if (!(value is XmlNode))
				{
					throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_IncorrectObjectType"), "value");
				}
				this.m_nodeArray[index] = value;
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000E940 File Offset: 0x0000D940
		public void CopyTo(Array array, int index)
		{
			this.m_nodeArray.CopyTo(array, index);
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000E94F File Offset: 0x0000D94F
		public object SyncRoot
		{
			get
			{
				return this.m_nodeArray.SyncRoot;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000E95C File Offset: 0x0000D95C
		public bool IsSynchronized
		{
			get
			{
				return this.m_nodeArray.IsSynchronized;
			}
		}

		// Token: 0x040004E6 RID: 1254
		private ArrayList m_nodeArray;
	}
}
