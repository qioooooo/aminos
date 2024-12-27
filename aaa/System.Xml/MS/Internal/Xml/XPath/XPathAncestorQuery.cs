using System;
using System.Xml;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000164 RID: 356
	internal sealed class XPathAncestorQuery : CacheAxisQuery
	{
		// Token: 0x06001332 RID: 4914 RVA: 0x00053377 File Offset: 0x00052377
		public XPathAncestorQuery(Query qyInput, string name, string prefix, XPathNodeType typeTest, bool matchSelf)
			: base(qyInput, name, prefix, typeTest)
		{
			this.matchSelf = matchSelf;
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0005338C File Offset: 0x0005238C
		private XPathAncestorQuery(XPathAncestorQuery other)
			: base(other)
		{
			this.matchSelf = other.matchSelf;
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x000533A4 File Offset: 0x000523A4
		public override object Evaluate(XPathNodeIterator context)
		{
			base.Evaluate(context);
			XPathNavigator xpathNavigator = null;
			XPathNavigator xpathNavigator2;
			while ((xpathNavigator2 = this.qyInput.Advance()) != null)
			{
				if (!this.matchSelf || !this.matches(xpathNavigator2) || base.Insert(this.outputBuffer, xpathNavigator2))
				{
					if (xpathNavigator == null || !xpathNavigator.MoveTo(xpathNavigator2))
					{
						xpathNavigator = xpathNavigator2.Clone();
					}
					while (xpathNavigator.MoveToParent() && (!this.matches(xpathNavigator) || base.Insert(this.outputBuffer, xpathNavigator)))
					{
					}
				}
			}
			return this;
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x00053422 File Offset: 0x00052422
		public override XPathNodeIterator Clone()
		{
			return new XPathAncestorQuery(this);
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001336 RID: 4918 RVA: 0x0005342A File Offset: 0x0005242A
		public override int CurrentPosition
		{
			get
			{
				return this.outputBuffer.Count - this.count + 1;
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001337 RID: 4919 RVA: 0x00053440 File Offset: 0x00052440
		public override QueryProps Properties
		{
			get
			{
				return base.Properties | QueryProps.Reverse;
			}
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0005344C File Offset: 0x0005244C
		public override void PrintQuery(XmlWriter w)
		{
			w.WriteStartElement(base.GetType().Name);
			if (this.matchSelf)
			{
				w.WriteAttributeString("self", "yes");
			}
			if (base.NameTest)
			{
				w.WriteAttributeString("name", (base.Prefix.Length != 0) ? (base.Prefix + ':' + base.Name) : base.Name);
			}
			if (base.TypeTest != XPathNodeType.Element)
			{
				w.WriteAttributeString("nodeType", base.TypeTest.ToString());
			}
			this.qyInput.PrintQuery(w);
			w.WriteEndElement();
		}

		// Token: 0x04000BEB RID: 3051
		private bool matchSelf;
	}
}
