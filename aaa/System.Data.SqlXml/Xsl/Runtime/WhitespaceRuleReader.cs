using System;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200009D RID: 157
	internal class WhitespaceRuleReader : XmlWrappingReader
	{
		// Token: 0x06000775 RID: 1909 RVA: 0x00026B14 File Offset: 0x00025B14
		public static XmlReader CreateReader(XmlReader baseReader, WhitespaceRuleLookup wsRules)
		{
			if (wsRules == null)
			{
				return baseReader;
			}
			XmlReaderSettings settings = baseReader.Settings;
			if (settings != null)
			{
				if (settings.IgnoreWhitespace)
				{
					return baseReader;
				}
			}
			else
			{
				XmlTextReader xmlTextReader = baseReader as XmlTextReader;
				if (xmlTextReader != null && xmlTextReader.WhitespaceHandling == WhitespaceHandling.None)
				{
					return baseReader;
				}
				XmlTextReaderImpl xmlTextReaderImpl = baseReader as XmlTextReaderImpl;
				if (xmlTextReaderImpl != null && xmlTextReaderImpl.WhitespaceHandling == WhitespaceHandling.None)
				{
					return baseReader;
				}
			}
			return new WhitespaceRuleReader(baseReader, wsRules);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00026B6C File Offset: 0x00025B6C
		private WhitespaceRuleReader(XmlReader baseReader, WhitespaceRuleLookup wsRules)
			: base(baseReader)
		{
			this.val = null;
			this.stkStrip = new BitStack();
			this.shouldStrip = false;
			this.preserveAdjacent = false;
			this.wsRules = wsRules;
			this.wsRules.Atomize(baseReader.NameTable);
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x00026BC3 File Offset: 0x00025BC3
		public override string Value
		{
			get
			{
				if (this.val != null)
				{
					return this.val;
				}
				return base.Value;
			}
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00026BDC File Offset: 0x00025BDC
		public override bool Read()
		{
			XmlCharType instance = XmlCharType.Instance;
			string text = null;
			this.val = null;
			while (base.Read())
			{
				XmlNodeType nodeType = base.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
					if (!base.IsEmptyElement)
					{
						this.stkStrip.PushBit(this.shouldStrip);
						this.shouldStrip = this.wsRules.ShouldStripSpace(base.LocalName, base.NamespaceURI) && base.XmlSpace != XmlSpace.Preserve;
						goto IL_012E;
					}
					goto IL_012E;
				case XmlNodeType.Attribute:
					goto IL_012E;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					if (this.preserveAdjacent)
					{
						return true;
					}
					if (!this.shouldStrip)
					{
						goto IL_012E;
					}
					if (!instance.IsOnlyWhitespace(base.Value))
					{
						if (text != null)
						{
							this.val = text + base.Value;
						}
						this.preserveAdjacent = true;
						return true;
					}
					break;
				case XmlNodeType.EntityReference:
					this.reader.ResolveEntity();
					goto IL_012E;
				default:
					switch (nodeType)
					{
					case XmlNodeType.Whitespace:
					case XmlNodeType.SignificantWhitespace:
						break;
					case XmlNodeType.EndElement:
						this.shouldStrip = this.stkStrip.PopBit();
						goto IL_012E;
					case XmlNodeType.EndEntity:
						continue;
					default:
						goto IL_012E;
					}
					break;
				}
				if (this.preserveAdjacent)
				{
					return true;
				}
				if (this.shouldStrip)
				{
					if (text == null)
					{
						text = base.Value;
						continue;
					}
					text += base.Value;
					continue;
				}
				IL_012E:
				this.preserveAdjacent = false;
				return true;
			}
			return false;
		}

		// Token: 0x04000514 RID: 1300
		private WhitespaceRuleLookup wsRules;

		// Token: 0x04000515 RID: 1301
		private BitStack stkStrip;

		// Token: 0x04000516 RID: 1302
		private bool shouldStrip;

		// Token: 0x04000517 RID: 1303
		private bool preserveAdjacent;

		// Token: 0x04000518 RID: 1304
		private string val;

		// Token: 0x04000519 RID: 1305
		private XmlCharType xmlCharType = XmlCharType.Instance;
	}
}
