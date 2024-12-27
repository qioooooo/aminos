using System;
using System.Diagnostics;

namespace System.Xml
{
	// Token: 0x020000E5 RID: 229
	[DebuggerDisplay("{ToString()}")]
	internal struct DebuggerDisplayXmlNodeProxy
	{
		// Token: 0x06000DF9 RID: 3577 RVA: 0x0003E1C4 File Offset: 0x0003D1C4
		public DebuggerDisplayXmlNodeProxy(XmlNode node)
		{
			this.node = node;
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0003E1D0 File Offset: 0x0003D1D0
		public override string ToString()
		{
			XmlNodeType nodeType = this.node.NodeType;
			string text = nodeType.ToString();
			switch (nodeType)
			{
			case XmlNodeType.Element:
			case XmlNodeType.EntityReference:
				text = text + ", Name=\"" + this.node.Name + "\"";
				break;
			case XmlNodeType.Attribute:
			case XmlNodeType.ProcessingInstruction:
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					", Name=\"",
					this.node.Name,
					"\", Value=\"",
					XmlConvert.EscapeValueForDebuggerDisplay(this.node.Value),
					"\""
				});
				break;
			}
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.Comment:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
			case XmlNodeType.XmlDeclaration:
				text = text + ", Value=\"" + XmlConvert.EscapeValueForDebuggerDisplay(this.node.Value) + "\"";
				break;
			case XmlNodeType.DocumentType:
			{
				XmlDocumentType xmlDocumentType = (XmlDocumentType)this.node;
				string text3 = text;
				text = string.Concat(new string[]
				{
					text3,
					", Name=\"",
					xmlDocumentType.Name,
					"\", SYSTEM=\"",
					xmlDocumentType.SystemId,
					"\", PUBLIC=\"",
					xmlDocumentType.PublicId,
					"\", Value=\"",
					XmlConvert.EscapeValueForDebuggerDisplay(xmlDocumentType.InternalSubset),
					"\""
				});
				break;
			}
			}
			return text;
		}

		// Token: 0x0400097A RID: 2426
		private XmlNode node;
	}
}
