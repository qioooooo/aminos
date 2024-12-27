using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000D5 RID: 213
	public class XmlDocumentFragment : XmlNode
	{
		// Token: 0x06000D00 RID: 3328 RVA: 0x0003A00E File Offset: 0x0003900E
		protected internal XmlDocumentFragment(XmlDocument ownerDocument)
		{
			if (ownerDocument == null)
			{
				throw new ArgumentException(Res.GetString("Xdom_Node_Null_Doc"));
			}
			this.parentNode = ownerDocument;
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x0003A030 File Offset: 0x00039030
		public override string Name
		{
			get
			{
				return this.OwnerDocument.strDocumentFragmentName;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x0003A03D File Offset: 0x0003903D
		public override string LocalName
		{
			get
			{
				return this.OwnerDocument.strDocumentFragmentName;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x0003A04A File Offset: 0x0003904A
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.DocumentFragment;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x0003A04E File Offset: 0x0003904E
		public override XmlNode ParentNode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x0003A051 File Offset: 0x00039051
		public override XmlDocument OwnerDocument
		{
			get
			{
				return (XmlDocument)this.parentNode;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x0003A05E File Offset: 0x0003905E
		// (set) Token: 0x06000D07 RID: 3335 RVA: 0x0003A068 File Offset: 0x00039068
		public override string InnerXml
		{
			get
			{
				return base.InnerXml;
			}
			set
			{
				this.RemoveAll();
				XmlLoader xmlLoader = new XmlLoader();
				xmlLoader.ParsePartialContent(this, value, XmlNodeType.Element);
			}
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x0003A08C File Offset: 0x0003908C
		public override XmlNode CloneNode(bool deep)
		{
			XmlDocument ownerDocument = this.OwnerDocument;
			XmlDocumentFragment xmlDocumentFragment = ownerDocument.CreateDocumentFragment();
			if (deep)
			{
				xmlDocumentFragment.CopyChildren(ownerDocument, this, deep);
			}
			return xmlDocumentFragment;
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x0003A0B4 File Offset: 0x000390B4
		internal override bool IsContainer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x0003A0B7 File Offset: 0x000390B7
		// (set) Token: 0x06000D0B RID: 3339 RVA: 0x0003A0BF File Offset: 0x000390BF
		internal override XmlLinkedNode LastNode
		{
			get
			{
				return this.lastChild;
			}
			set
			{
				this.lastChild = value;
			}
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0003A0C8 File Offset: 0x000390C8
		internal override bool IsValidChildType(XmlNodeType type)
		{
			switch (type)
			{
			case XmlNodeType.Element:
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.EntityReference:
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				return true;
			case XmlNodeType.XmlDeclaration:
			{
				XmlNode firstChild = this.FirstChild;
				return firstChild == null || firstChild.NodeType != XmlNodeType.XmlDeclaration;
			}
			}
			return false;
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0003A140 File Offset: 0x00039140
		internal override bool CanInsertAfter(XmlNode newChild, XmlNode refChild)
		{
			return newChild.NodeType != XmlNodeType.XmlDeclaration || (refChild == null && this.LastNode == null);
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0003A15C File Offset: 0x0003915C
		internal override bool CanInsertBefore(XmlNode newChild, XmlNode refChild)
		{
			return newChild.NodeType != XmlNodeType.XmlDeclaration || refChild == null || refChild == this.FirstChild;
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0003A178 File Offset: 0x00039178
		public override void WriteTo(XmlWriter w)
		{
			this.WriteContentTo(w);
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0003A184 File Offset: 0x00039184
		public override void WriteContentTo(XmlWriter w)
		{
			foreach (object obj in this)
			{
				XmlNode xmlNode = (XmlNode)obj;
				xmlNode.WriteTo(w);
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x0003A1D8 File Offset: 0x000391D8
		internal override XPathNodeType XPNodeType
		{
			get
			{
				return XPathNodeType.Root;
			}
		}

		// Token: 0x04000928 RID: 2344
		private XmlLinkedNode lastChild;
	}
}
