using System;
using System.Xml;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000134 RID: 308
	internal abstract class DescendantBaseQuery : BaseAxisQuery
	{
		// Token: 0x060011D6 RID: 4566 RVA: 0x0004E9EA File Offset: 0x0004D9EA
		public DescendantBaseQuery(Query qyParent, string Name, string Prefix, XPathNodeType Type, bool matchSelf, bool abbrAxis)
			: base(qyParent, Name, Prefix, Type)
		{
			this.matchSelf = matchSelf;
			this.abbrAxis = abbrAxis;
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0004EA07 File Offset: 0x0004DA07
		public DescendantBaseQuery(DescendantBaseQuery other)
			: base(other)
		{
			this.matchSelf = other.matchSelf;
			this.abbrAxis = other.abbrAxis;
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x0004EA28 File Offset: 0x0004DA28
		public override XPathNavigator MatchNode(XPathNavigator context)
		{
			if (context != null)
			{
				if (!this.abbrAxis)
				{
					throw XPathException.Create("Xp_InvalidPattern");
				}
				if (this.matches(context))
				{
					XPathNavigator xpathNavigator;
					if (this.matchSelf && (xpathNavigator = this.qyInput.MatchNode(context)) != null)
					{
						return xpathNavigator;
					}
					XPathNavigator xpathNavigator2 = context.Clone();
					while (xpathNavigator2.MoveToParent())
					{
						if ((xpathNavigator = this.qyInput.MatchNode(xpathNavigator2)) != null)
						{
							return xpathNavigator;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x0004EA94 File Offset: 0x0004DA94
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

		// Token: 0x04000B52 RID: 2898
		protected bool matchSelf;

		// Token: 0x04000B53 RID: 2899
		protected bool abbrAxis;
	}
}
