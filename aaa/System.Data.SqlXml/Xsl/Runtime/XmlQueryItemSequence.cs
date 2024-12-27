using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000B7 RID: 183
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class XmlQueryItemSequence : XmlQuerySequence<XPathItem>
	{
		// Token: 0x060008F0 RID: 2288 RVA: 0x0002B31B File Offset: 0x0002A31B
		public static XmlQueryItemSequence CreateOrReuse(XmlQueryItemSequence seq)
		{
			if (seq != null)
			{
				seq.Clear();
				return seq;
			}
			return new XmlQueryItemSequence();
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0002B32D File Offset: 0x0002A32D
		public static XmlQueryItemSequence CreateOrReuse(XmlQueryItemSequence seq, XPathItem item)
		{
			if (seq != null)
			{
				seq.Clear();
				seq.Add(item);
				return seq;
			}
			return new XmlQueryItemSequence(item);
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0002B347 File Offset: 0x0002A347
		public XmlQueryItemSequence()
		{
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0002B34F File Offset: 0x0002A34F
		public XmlQueryItemSequence(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0002B358 File Offset: 0x0002A358
		public XmlQueryItemSequence(XPathItem item)
			: base(1)
		{
			this.AddClone(item);
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0002B368 File Offset: 0x0002A368
		public void AddClone(XPathItem item)
		{
			if (item.IsNode)
			{
				base.Add(((XPathNavigator)item).Clone());
				return;
			}
			base.Add(item);
		}

		// Token: 0x040005AA RID: 1450
		public new static readonly XmlQueryItemSequence Empty = new XmlQueryItemSequence();
	}
}
