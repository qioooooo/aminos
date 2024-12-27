using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000F0 RID: 240
	public class XmlWhitespace : XmlCharacterData
	{
		// Token: 0x06000EB7 RID: 3767 RVA: 0x00040A8B File Offset: 0x0003FA8B
		protected internal XmlWhitespace(string strData, XmlDocument doc)
			: base(strData, doc)
		{
			if (!doc.IsLoading && !base.CheckOnData(strData))
			{
				throw new ArgumentException(Res.GetString("Xdom_WS_Char"));
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x00040AB6 File Offset: 0x0003FAB6
		public override string Name
		{
			get
			{
				return this.OwnerDocument.strNonSignificantWhitespaceName;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x00040AC3 File Offset: 0x0003FAC3
		public override string LocalName
		{
			get
			{
				return this.OwnerDocument.strNonSignificantWhitespaceName;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000EBA RID: 3770 RVA: 0x00040AD0 File Offset: 0x0003FAD0
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.Whitespace;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000EBB RID: 3771 RVA: 0x00040AD4 File Offset: 0x0003FAD4
		public override XmlNode ParentNode
		{
			get
			{
				XmlNodeType nodeType = this.parentNode.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					break;
				default:
					if (nodeType == XmlNodeType.Document)
					{
						return base.ParentNode;
					}
					switch (nodeType)
					{
					case XmlNodeType.Whitespace:
					case XmlNodeType.SignificantWhitespace:
						break;
					default:
						return this.parentNode;
					}
					break;
				}
				XmlNode xmlNode = this.parentNode.parentNode;
				while (xmlNode.IsText)
				{
					xmlNode = xmlNode.parentNode;
				}
				return xmlNode;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x00040B41 File Offset: 0x0003FB41
		// (set) Token: 0x06000EBD RID: 3773 RVA: 0x00040B49 File Offset: 0x0003FB49
		public override string Value
		{
			get
			{
				return this.Data;
			}
			set
			{
				if (base.CheckOnData(value))
				{
					this.Data = value;
					return;
				}
				throw new ArgumentException(Res.GetString("Xdom_WS_Char"));
			}
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00040B6B File Offset: 0x0003FB6B
		public override XmlNode CloneNode(bool deep)
		{
			return this.OwnerDocument.CreateWhitespace(this.Data);
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x00040B7E File Offset: 0x0003FB7E
		public override void WriteTo(XmlWriter w)
		{
			w.WriteWhitespace(this.Data);
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00040B8C File Offset: 0x0003FB8C
		public override void WriteContentTo(XmlWriter w)
		{
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x00040B90 File Offset: 0x0003FB90
		internal override XPathNodeType XPNodeType
		{
			get
			{
				XPathNodeType xpathNodeType = XPathNodeType.Whitespace;
				base.DecideXPNodeTypeForTextNodes(this, ref xpathNodeType);
				return xpathNodeType;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000EC2 RID: 3778 RVA: 0x00040BAA File Offset: 0x0003FBAA
		internal override bool IsText
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x00040BAD File Offset: 0x0003FBAD
		internal override XmlNode PreviousText
		{
			get
			{
				if (this.parentNode.IsText)
				{
					return this.parentNode;
				}
				return null;
			}
		}
	}
}
