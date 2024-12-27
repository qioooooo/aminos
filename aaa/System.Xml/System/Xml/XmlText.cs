using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000EE RID: 238
	public class XmlText : XmlCharacterData
	{
		// Token: 0x06000E9D RID: 3741 RVA: 0x000407D8 File Offset: 0x0003F7D8
		internal XmlText(string strData)
			: this(strData, null)
		{
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x000407E2 File Offset: 0x0003F7E2
		protected internal XmlText(string strData, XmlDocument doc)
			: base(strData, doc)
		{
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000E9F RID: 3743 RVA: 0x000407EC File Offset: 0x0003F7EC
		public override string Name
		{
			get
			{
				return this.OwnerDocument.strTextName;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x000407F9 File Offset: 0x0003F7F9
		public override string LocalName
		{
			get
			{
				return this.OwnerDocument.strTextName;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x00040806 File Offset: 0x0003F806
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.Text;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x0004080C File Offset: 0x0003F80C
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

		// Token: 0x06000EA3 RID: 3747 RVA: 0x00040874 File Offset: 0x0003F874
		public override XmlNode CloneNode(bool deep)
		{
			return this.OwnerDocument.CreateTextNode(this.Data);
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x00040887 File Offset: 0x0003F887
		// (set) Token: 0x06000EA5 RID: 3749 RVA: 0x00040890 File Offset: 0x0003F890
		public override string Value
		{
			get
			{
				return this.Data;
			}
			set
			{
				this.Data = value;
				XmlNode parentNode = this.parentNode;
				if (parentNode != null && parentNode.NodeType == XmlNodeType.Attribute)
				{
					XmlUnspecifiedAttribute xmlUnspecifiedAttribute = parentNode as XmlUnspecifiedAttribute;
					if (xmlUnspecifiedAttribute != null && !xmlUnspecifiedAttribute.Specified)
					{
						xmlUnspecifiedAttribute.SetSpecified(true);
					}
				}
			}
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x000408D0 File Offset: 0x0003F8D0
		public virtual XmlText SplitText(int offset)
		{
			XmlNode parentNode = this.ParentNode;
			int length = this.Length;
			if (offset > length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (parentNode == null)
			{
				throw new InvalidOperationException(Res.GetString("Xdom_TextNode_SplitText"));
			}
			int num = length - offset;
			string text = this.Substring(offset, num);
			this.DeleteData(offset, num);
			XmlText xmlText = this.OwnerDocument.CreateTextNode(text);
			parentNode.InsertAfter(xmlText, this);
			return xmlText;
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0004093C File Offset: 0x0003F93C
		public override void WriteTo(XmlWriter w)
		{
			w.WriteString(this.Data);
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0004094A File Offset: 0x0003F94A
		public override void WriteContentTo(XmlWriter w)
		{
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x0004094C File Offset: 0x0003F94C
		internal override XPathNodeType XPNodeType
		{
			get
			{
				return XPathNodeType.Text;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x0004094F File Offset: 0x0003F94F
		internal override bool IsText
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000EAB RID: 3755 RVA: 0x00040952 File Offset: 0x0003F952
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
