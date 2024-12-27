using System;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000388 RID: 904
	internal sealed class RegionIterator : BaseRegionIterator
	{
		// Token: 0x06002FEA RID: 12266 RVA: 0x002B2460 File Offset: 0x002B1860
		internal RegionIterator(XmlBoundElement rowElement)
			: base(((XmlDataDocument)rowElement.OwnerDocument).Mapper)
		{
			this.rowElement = rowElement;
			this.currentNode = rowElement;
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x002B2494 File Offset: 0x002B1894
		internal override void Reset()
		{
			this.currentNode = this.rowElement;
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002FEC RID: 12268 RVA: 0x002B24B0 File Offset: 0x002B18B0
		internal override XmlNode CurrentNode
		{
			get
			{
				return this.currentNode;
			}
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x002B24C4 File Offset: 0x002B18C4
		internal override bool Next()
		{
			ElementState elementState = this.rowElement.ElementState;
			XmlNode firstChild = this.currentNode.FirstChild;
			if (firstChild != null)
			{
				this.currentNode = firstChild;
				this.rowElement.ElementState = elementState;
				return true;
			}
			return this.NextRight();
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x002B2508 File Offset: 0x002B1908
		internal override bool NextRight()
		{
			if (this.currentNode == this.rowElement)
			{
				this.currentNode = null;
				return false;
			}
			ElementState elementState = this.rowElement.ElementState;
			XmlNode xmlNode = this.currentNode.NextSibling;
			if (xmlNode != null)
			{
				this.currentNode = xmlNode;
				this.rowElement.ElementState = elementState;
				return true;
			}
			xmlNode = this.currentNode;
			while (xmlNode != this.rowElement && xmlNode.NextSibling == null)
			{
				xmlNode = xmlNode.ParentNode;
			}
			if (xmlNode == this.rowElement)
			{
				this.currentNode = null;
				this.rowElement.ElementState = elementState;
				return false;
			}
			this.currentNode = xmlNode.NextSibling;
			this.rowElement.ElementState = elementState;
			return true;
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x002B25B4 File Offset: 0x002B19B4
		internal bool NextInitialTextLikeNodes(out string value)
		{
			ElementState elementState = this.rowElement.ElementState;
			XmlNode firstChild = this.CurrentNode.FirstChild;
			value = RegionIterator.GetInitialTextFromNodes(ref firstChild);
			if (firstChild == null)
			{
				this.rowElement.ElementState = elementState;
				return this.NextRight();
			}
			this.currentNode = firstChild;
			this.rowElement.ElementState = elementState;
			return true;
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x002B260C File Offset: 0x002B1A0C
		private static string GetInitialTextFromNodes(ref XmlNode n)
		{
			string text = null;
			if (n != null)
			{
				while (n.NodeType == XmlNodeType.Whitespace)
				{
					n = n.NextSibling;
					if (n == null)
					{
						return string.Empty;
					}
				}
				if (XmlDataDocument.IsTextLikeNode(n) && (n.NextSibling == null || !XmlDataDocument.IsTextLikeNode(n.NextSibling)))
				{
					text = n.Value;
					n = n.NextSibling;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (n != null && XmlDataDocument.IsTextLikeNode(n))
					{
						if (n.NodeType != XmlNodeType.Whitespace)
						{
							stringBuilder.Append(n.Value);
						}
						n = n.NextSibling;
					}
					text = stringBuilder.ToString();
				}
			}
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}

		// Token: 0x04001D97 RID: 7575
		private XmlBoundElement rowElement;

		// Token: 0x04001D98 RID: 7576
		private XmlNode currentNode;
	}
}
