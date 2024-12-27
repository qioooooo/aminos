using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000C0 RID: 192
	internal abstract class DocumentXPathNodeIterator_ElemDescendants : XPathNodeIterator
	{
		// Token: 0x06000B66 RID: 2918 RVA: 0x00034DC7 File Offset: 0x00033DC7
		internal DocumentXPathNodeIterator_ElemDescendants(DocumentXPathNavigator nav)
		{
			this.nav = (DocumentXPathNavigator)nav.Clone();
			this.level = 0;
			this.position = 0;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x00034DEE File Offset: 0x00033DEE
		internal DocumentXPathNodeIterator_ElemDescendants(DocumentXPathNodeIterator_ElemDescendants other)
		{
			this.nav = (DocumentXPathNavigator)other.nav.Clone();
			this.level = other.level;
			this.position = other.position;
		}

		// Token: 0x06000B68 RID: 2920
		protected abstract bool Match(XmlNode node);

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x00034E24 File Offset: 0x00033E24
		public override XPathNavigator Current
		{
			get
			{
				return this.nav;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x00034E2C File Offset: 0x00033E2C
		public override int CurrentPosition
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00034E34 File Offset: 0x00033E34
		protected void SetPosition(int pos)
		{
			this.position = pos;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00034E40 File Offset: 0x00033E40
		public override bool MoveNext()
		{
			for (;;)
			{
				if (this.nav.MoveToFirstChild())
				{
					this.level++;
				}
				else
				{
					if (this.level == 0)
					{
						break;
					}
					while (!this.nav.MoveToNext())
					{
						this.level--;
						if (this.level == 0)
						{
							return false;
						}
						if (!this.nav.MoveToParent())
						{
							return false;
						}
					}
				}
				XmlNode xmlNode = (XmlNode)this.nav.UnderlyingObject;
				if (xmlNode.NodeType == XmlNodeType.Element && this.Match(xmlNode))
				{
					goto Block_5;
				}
			}
			return false;
			Block_5:
			this.position++;
			return true;
		}

		// Token: 0x040008DF RID: 2271
		private DocumentXPathNavigator nav;

		// Token: 0x040008E0 RID: 2272
		private int level;

		// Token: 0x040008E1 RID: 2273
		private int position;
	}
}
