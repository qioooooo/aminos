using System;

namespace System.Xml
{
	// Token: 0x02000389 RID: 905
	internal sealed class TreeIterator : BaseTreeIterator
	{
		// Token: 0x06002FF1 RID: 12273 RVA: 0x002B26C0 File Offset: 0x002B1AC0
		internal TreeIterator(XmlNode nodeTop)
			: base(((XmlDataDocument)nodeTop.OwnerDocument).Mapper)
		{
			this.nodeTop = nodeTop;
			this.currentNode = nodeTop;
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x002B26F4 File Offset: 0x002B1AF4
		internal override void Reset()
		{
			this.currentNode = this.nodeTop;
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06002FF3 RID: 12275 RVA: 0x002B2710 File Offset: 0x002B1B10
		internal override XmlNode CurrentNode
		{
			get
			{
				return this.currentNode;
			}
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x002B2724 File Offset: 0x002B1B24
		internal override bool Next()
		{
			XmlNode firstChild = this.currentNode.FirstChild;
			if (firstChild != null)
			{
				this.currentNode = firstChild;
				return true;
			}
			return this.NextRight();
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x002B2750 File Offset: 0x002B1B50
		internal override bool NextRight()
		{
			if (this.currentNode == this.nodeTop)
			{
				this.currentNode = null;
				return false;
			}
			XmlNode xmlNode = this.currentNode.NextSibling;
			if (xmlNode != null)
			{
				this.currentNode = xmlNode;
				return true;
			}
			xmlNode = this.currentNode;
			while (xmlNode != this.nodeTop && xmlNode.NextSibling == null)
			{
				xmlNode = xmlNode.ParentNode;
			}
			if (xmlNode == this.nodeTop)
			{
				this.currentNode = null;
				return false;
			}
			this.currentNode = xmlNode.NextSibling;
			return true;
		}

		// Token: 0x04001D99 RID: 7577
		private XmlNode nodeTop;

		// Token: 0x04001D9A RID: 7578
		private XmlNode currentNode;
	}
}
