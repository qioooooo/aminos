using System;
using System.Xml.XPath;

namespace System.Xml
{
	public class XmlText : XmlCharacterData
	{
		internal XmlText(string strData)
			: this(strData, null)
		{
		}

		protected internal XmlText(string strData, XmlDocument doc)
			: base(strData, doc)
		{
		}

		public override string Name
		{
			get
			{
				return this.OwnerDocument.strTextName;
			}
		}

		public override string LocalName
		{
			get
			{
				return this.OwnerDocument.strTextName;
			}
		}

		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.Text;
			}
		}

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

		public override XmlNode CloneNode(bool deep)
		{
			return this.OwnerDocument.CreateTextNode(this.Data);
		}

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

		public override void WriteTo(XmlWriter w)
		{
			w.WriteString(this.Data);
		}

		public override void WriteContentTo(XmlWriter w)
		{
		}

		internal override XPathNodeType XPNodeType
		{
			get
			{
				return XPathNodeType.Text;
			}
		}

		internal override bool IsText
		{
			get
			{
				return true;
			}
		}

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
