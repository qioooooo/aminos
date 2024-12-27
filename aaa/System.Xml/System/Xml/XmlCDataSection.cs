using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000CE RID: 206
	public class XmlCDataSection : XmlCharacterData
	{
		// Token: 0x06000C4A RID: 3146 RVA: 0x00037E10 File Offset: 0x00036E10
		protected internal XmlCDataSection(string data, XmlDocument doc)
			: base(data, doc)
		{
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000C4B RID: 3147 RVA: 0x00037E1A File Offset: 0x00036E1A
		public override string Name
		{
			get
			{
				return this.OwnerDocument.strCDataSectionName;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x00037E27 File Offset: 0x00036E27
		public override string LocalName
		{
			get
			{
				return this.OwnerDocument.strCDataSectionName;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x00037E34 File Offset: 0x00036E34
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.CDATA;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x00037E38 File Offset: 0x00036E38
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
						return null;
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

		// Token: 0x06000C4F RID: 3151 RVA: 0x00037EA0 File Offset: 0x00036EA0
		public override XmlNode CloneNode(bool deep)
		{
			return this.OwnerDocument.CreateCDataSection(this.Data);
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00037EB3 File Offset: 0x00036EB3
		public override void WriteTo(XmlWriter w)
		{
			w.WriteCData(this.Data);
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00037EC1 File Offset: 0x00036EC1
		public override void WriteContentTo(XmlWriter w)
		{
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x00037EC3 File Offset: 0x00036EC3
		internal override XPathNodeType XPNodeType
		{
			get
			{
				return XPathNodeType.Text;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000C53 RID: 3155 RVA: 0x00037EC6 File Offset: 0x00036EC6
		internal override bool IsText
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000C54 RID: 3156 RVA: 0x00037EC9 File Offset: 0x00036EC9
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
