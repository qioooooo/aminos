using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000ED RID: 237
	public class XmlSignificantWhitespace : XmlCharacterData
	{
		// Token: 0x06000E90 RID: 3728 RVA: 0x0004069F File Offset: 0x0003F69F
		protected internal XmlSignificantWhitespace(string strData, XmlDocument doc)
			: base(strData, doc)
		{
			if (!doc.IsLoading && !base.CheckOnData(strData))
			{
				throw new ArgumentException(Res.GetString("Xdom_WS_Char"));
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x000406CA File Offset: 0x0003F6CA
		public override string Name
		{
			get
			{
				return this.OwnerDocument.strSignificantWhitespaceName;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x000406D7 File Offset: 0x0003F6D7
		public override string LocalName
		{
			get
			{
				return this.OwnerDocument.strSignificantWhitespaceName;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x000406E4 File Offset: 0x0003F6E4
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.SignificantWhitespace;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000E94 RID: 3732 RVA: 0x000406E8 File Offset: 0x0003F6E8
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

		// Token: 0x06000E95 RID: 3733 RVA: 0x00040755 File Offset: 0x0003F755
		public override XmlNode CloneNode(bool deep)
		{
			return this.OwnerDocument.CreateSignificantWhitespace(this.Data);
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000E96 RID: 3734 RVA: 0x00040768 File Offset: 0x0003F768
		// (set) Token: 0x06000E97 RID: 3735 RVA: 0x00040770 File Offset: 0x0003F770
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

		// Token: 0x06000E98 RID: 3736 RVA: 0x00040792 File Offset: 0x0003F792
		public override void WriteTo(XmlWriter w)
		{
			w.WriteString(this.Data);
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x000407A0 File Offset: 0x0003F7A0
		public override void WriteContentTo(XmlWriter w)
		{
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000E9A RID: 3738 RVA: 0x000407A4 File Offset: 0x0003F7A4
		internal override XPathNodeType XPNodeType
		{
			get
			{
				XPathNodeType xpathNodeType = XPathNodeType.SignificantWhitespace;
				base.DecideXPNodeTypeForTextNodes(this, ref xpathNodeType);
				return xpathNodeType;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000E9B RID: 3739 RVA: 0x000407BE File Offset: 0x0003F7BE
		internal override bool IsText
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000E9C RID: 3740 RVA: 0x000407C1 File Offset: 0x0003F7C1
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
