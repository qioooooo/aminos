using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000F1 RID: 241
	internal class XPathNodeList : XmlNodeList
	{
		// Token: 0x06000EC4 RID: 3780 RVA: 0x00040BC4 File Offset: 0x0003FBC4
		public XPathNodeList(XPathNodeIterator nodeIterator)
		{
			this.nodeIterator = nodeIterator;
			this.list = new List<XmlNode>();
			this.done = false;
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x00040BE5 File Offset: 0x0003FBE5
		public override int Count
		{
			get
			{
				if (!this.done)
				{
					this.ReadUntil(int.MaxValue);
				}
				return this.list.Count;
			}
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00040C08 File Offset: 0x0003FC08
		private XmlNode GetNode(XPathNavigator n)
		{
			IHasXmlNode hasXmlNode = (IHasXmlNode)n;
			return hasXmlNode.GetNode();
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x00040C24 File Offset: 0x0003FC24
		internal int ReadUntil(int index)
		{
			int num = this.list.Count;
			while (!this.done && num <= index)
			{
				if (!this.nodeIterator.MoveNext())
				{
					this.done = true;
					break;
				}
				XmlNode node = this.GetNode(this.nodeIterator.Current);
				if (node != null)
				{
					this.list.Add(node);
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00040C89 File Offset: 0x0003FC89
		public override XmlNode Item(int index)
		{
			if (this.list.Count <= index)
			{
				this.ReadUntil(index);
			}
			if (index < 0 || this.list.Count <= index)
			{
				return null;
			}
			return this.list[index];
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x00040CC1 File Offset: 0x0003FCC1
		public override IEnumerator GetEnumerator()
		{
			return new XmlNodeListEnumerator(this);
		}

		// Token: 0x040009A8 RID: 2472
		private List<XmlNode> list;

		// Token: 0x040009A9 RID: 2473
		private XPathNodeIterator nodeIterator;

		// Token: 0x040009AA RID: 2474
		private bool done;

		// Token: 0x040009AB RID: 2475
		private static readonly object[] nullparams = new object[0];
	}
}
