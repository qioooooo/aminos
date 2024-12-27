using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000A0 RID: 160
	internal class PlainXmlWriter : XmlWriter
	{
		// Token: 0x06000475 RID: 1141 RVA: 0x0003C460 File Offset: 0x0003B860
		public PlainXmlWriter(bool format)
		{
			this.navigator = new TraceXPathNavigator();
			this.stack = new Stack<string>();
			this.format = format;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0003C490 File Offset: 0x0003B890
		public PlainXmlWriter()
			: this(false)
		{
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0003C4A4 File Offset: 0x0003B8A4
		public XPathNavigator ToNavigator()
		{
			return this.navigator;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0003C4B8 File Offset: 0x0003B8B8
		public override void WriteStartDocument()
		{
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0003C4C8 File Offset: 0x0003B8C8
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0003C4D8 File Offset: 0x0003B8D8
		public override void WriteStartDocument(bool standalone)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0003C4EC File Offset: 0x0003B8EC
		public override void WriteEndDocument()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0003C500 File Offset: 0x0003B900
		public override string LookupPrefix(string ns)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x0003C514 File Offset: 0x0003B914
		public override WriteState WriteState
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x0003C528 File Offset: 0x0003B928
		public override XmlSpace XmlSpace
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x0003C53C File Offset: 0x0003B93C
		public override string XmlLang
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0003C550 File Offset: 0x0003B950
		public override void WriteNmToken(string name)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0003C564 File Offset: 0x0003B964
		public override void WriteName(string name)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0003C578 File Offset: 0x0003B978
		public override void WriteQualifiedName(string localName, string ns)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0003C58C File Offset: 0x0003B98C
		public override void WriteValue(object value)
		{
			this.navigator.AddText(value.ToString());
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0003C5AC File Offset: 0x0003B9AC
		public override void WriteValue(string value)
		{
			this.navigator.AddText(value);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0003C5C8 File Offset: 0x0003B9C8
		public override void WriteBase64(byte[] buffer, int offset, int count)
		{
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0003C5D8 File Offset: 0x0003B9D8
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.navigator.AddElement(prefix, localName, ns);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0003C5F4 File Offset: 0x0003B9F4
		public override void WriteFullEndElement()
		{
			this.WriteEndElement();
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0003C608 File Offset: 0x0003BA08
		public override void WriteEndElement()
		{
			this.navigator.CloseElement();
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0003C620 File Offset: 0x0003BA20
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			this.currentAttributeName = localName;
			this.currentAttributePrefix = prefix;
			this.currentAttributeNs = ns;
			this.writingAttribute = true;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0003C64C File Offset: 0x0003BA4C
		public override void WriteEndAttribute()
		{
			this.writingAttribute = false;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0003C660 File Offset: 0x0003BA60
		public override void WriteCData(string text)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0003C674 File Offset: 0x0003BA74
		public override void WriteComment(string text)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0003C688 File Offset: 0x0003BA88
		public override void WriteProcessingInstruction(string name, string text)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0003C69C File Offset: 0x0003BA9C
		public override void WriteEntityRef(string name)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0003C6B0 File Offset: 0x0003BAB0
		public override void WriteCharEntity(char ch)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0003C6C4 File Offset: 0x0003BAC4
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0003C6D8 File Offset: 0x0003BAD8
		public override void WriteWhitespace(string ws)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0003C6EC File Offset: 0x0003BAEC
		public override void WriteString(string text)
		{
			if (this.writingAttribute)
			{
				this.navigator.AddAttribute(this.currentAttributeName, text, this.currentAttributeNs, this.currentAttributePrefix);
				return;
			}
			this.WriteValue(text);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0003C728 File Offset: 0x0003BB28
		public override void WriteChars(char[] buffer, int index, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0003C73C File Offset: 0x0003BB3C
		public override void WriteRaw(string data)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0003C750 File Offset: 0x0003BB50
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0003C764 File Offset: 0x0003BB64
		public override void WriteBinHex(byte[] buffer, int index, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0003C778 File Offset: 0x0003BB78
		public override void Close()
		{
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0003C788 File Offset: 0x0003BB88
		public override void Flush()
		{
		}

		// Token: 0x0400026B RID: 619
		private TraceXPathNavigator navigator;

		// Token: 0x0400026C RID: 620
		private Stack<string> stack;

		// Token: 0x0400026D RID: 621
		private bool writingAttribute;

		// Token: 0x0400026E RID: 622
		private string currentAttributeName;

		// Token: 0x0400026F RID: 623
		private string currentAttributePrefix;

		// Token: 0x04000270 RID: 624
		private string currentAttributeNs;

		// Token: 0x04000271 RID: 625
		private bool format;
	}
}
