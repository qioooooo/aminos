using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	internal class HtmlUtf8RawTextWriter : XmlUtf8RawTextWriter
	{
		public HtmlUtf8RawTextWriter(Stream stream, Encoding encoding, XmlWriterSettings settings, bool closeOutput)
			: base(stream, encoding, settings, closeOutput)
		{
			this.Init(settings);
		}

		internal override void WriteXmlDeclaration(XmlStandalone standalone)
		{
		}

		internal override void WriteXmlDeclaration(string xmldecl)
		{
		}

		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			base.RawText("<!DOCTYPE ");
			if (name == "HTML")
			{
				base.RawText("HTML");
			}
			else
			{
				base.RawText("html");
			}
			if (pubid != null)
			{
				base.RawText(" PUBLIC \"");
				base.RawText(pubid);
				if (sysid != null)
				{
					base.RawText("\" \"");
					base.RawText(sysid);
				}
				this.bufBytes[this.bufPos++] = 34;
			}
			else if (sysid != null)
			{
				base.RawText(" SYSTEM \"");
				base.RawText(sysid);
				this.bufBytes[this.bufPos++] = 34;
			}
			else
			{
				this.bufBytes[this.bufPos++] = 32;
			}
			if (subset != null)
			{
				this.bufBytes[this.bufPos++] = 91;
				base.RawText(subset);
				this.bufBytes[this.bufPos++] = 93;
			}
			this.bufBytes[this.bufPos++] = 62;
		}

		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.elementScope.Push((byte)this.currentElementProperties);
			if (ns.Length == 0)
			{
				this.currentElementProperties = (ElementProperties)HtmlUtf8RawTextWriter.elementPropertySearch.FindCaseInsensitiveString(localName);
				this.bufBytes[this.bufPos++] = 60;
				base.RawText(localName);
				this.attrEndPos = this.bufPos;
				return;
			}
			this.currentElementProperties = ElementProperties.HAS_NS;
			base.WriteStartElement(prefix, localName, ns);
		}

		internal override void StartElementContent()
		{
			this.bufBytes[this.bufPos++] = 62;
			this.contentPos = this.bufPos;
			if ((this.currentElementProperties & ElementProperties.HEAD) != ElementProperties.DEFAULT)
			{
				this.WriteMetaElement();
			}
		}

		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
			if (ns.Length == 0)
			{
				if ((this.currentElementProperties & ElementProperties.EMPTY) == ElementProperties.DEFAULT)
				{
					this.bufBytes[this.bufPos++] = 60;
					this.bufBytes[this.bufPos++] = 47;
					base.RawText(localName);
					this.bufBytes[this.bufPos++] = 62;
				}
			}
			else
			{
				base.WriteEndElement(prefix, localName, ns);
			}
			this.currentElementProperties = (ElementProperties)this.elementScope.Pop();
		}

		internal override void WriteFullEndElement(string prefix, string localName, string ns)
		{
			if (ns.Length == 0)
			{
				if ((this.currentElementProperties & ElementProperties.EMPTY) == ElementProperties.DEFAULT)
				{
					this.bufBytes[this.bufPos++] = 60;
					this.bufBytes[this.bufPos++] = 47;
					base.RawText(localName);
					this.bufBytes[this.bufPos++] = 62;
				}
			}
			else
			{
				base.WriteFullEndElement(prefix, localName, ns);
			}
			this.currentElementProperties = (ElementProperties)this.elementScope.Pop();
		}

		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			if (ns.Length == 0)
			{
				if (this.attrEndPos == this.bufPos)
				{
					this.bufBytes[this.bufPos++] = 32;
				}
				base.RawText(localName);
				if ((this.currentElementProperties & (ElementProperties)7U) != ElementProperties.DEFAULT)
				{
					this.currentAttributeProperties = (AttributeProperties)((ElementProperties)HtmlUtf8RawTextWriter.attributePropertySearch.FindCaseInsensitiveString(localName) & this.currentElementProperties);
					if ((this.currentAttributeProperties & AttributeProperties.BOOLEAN) != AttributeProperties.DEFAULT)
					{
						this.inAttributeValue = true;
						return;
					}
				}
				else
				{
					this.currentAttributeProperties = AttributeProperties.DEFAULT;
				}
				this.bufBytes[this.bufPos++] = 61;
				this.bufBytes[this.bufPos++] = 34;
			}
			else
			{
				base.WriteStartAttribute(prefix, localName, ns);
				this.currentAttributeProperties = AttributeProperties.DEFAULT;
			}
			this.inAttributeValue = true;
		}

		public override void WriteEndAttribute()
		{
			if ((this.currentAttributeProperties & AttributeProperties.BOOLEAN) != AttributeProperties.DEFAULT)
			{
				this.attrEndPos = this.bufPos;
			}
			else
			{
				if (this.endsWithAmpersand)
				{
					this.OutputRestAmps();
					this.endsWithAmpersand = false;
				}
				this.bufBytes[this.bufPos++] = 34;
			}
			this.inAttributeValue = false;
			this.attrEndPos = this.bufPos;
		}

		public override void WriteProcessingInstruction(string target, string text)
		{
			this.bufBytes[this.bufPos++] = 60;
			this.bufBytes[this.bufPos++] = 63;
			base.RawText(target);
			this.bufBytes[this.bufPos++] = 32;
			base.WriteCommentOrPi(text, 63);
			this.bufBytes[this.bufPos++] = 62;
			if (this.bufPos > this.bufLen)
			{
				this.FlushBuffer();
			}
		}

		public unsafe override void WriteString(string text)
		{
			fixed (char* ptr = text)
			{
				char* ptr2 = ptr + text.Length;
				if (this.inAttributeValue)
				{
					this.WriteHtmlAttributeTextBlock(ptr, ptr2);
				}
				else
				{
					this.WriteHtmlElementTextBlock(ptr, ptr2);
				}
			}
		}

		public override void WriteEntityRef(string name)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		public override void WriteCharEntity(char ch)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		public unsafe override void WriteChars(char[] buffer, int index, int count)
		{
			fixed (char* ptr = &buffer[index])
			{
				if (this.inAttributeValue)
				{
					base.WriteAttributeTextBlock(ptr, ptr + count);
				}
				else
				{
					base.WriteElementTextBlock(ptr, ptr + count);
				}
			}
		}

		private void Init(XmlWriterSettings settings)
		{
			if (HtmlUtf8RawTextWriter.elementPropertySearch == null)
			{
				HtmlUtf8RawTextWriter.attributePropertySearch = new TernaryTreeReadOnly(HtmlTernaryTree.htmlAttributes);
				HtmlUtf8RawTextWriter.elementPropertySearch = new TernaryTreeReadOnly(HtmlTernaryTree.htmlElements);
			}
			this.elementScope = new ByteStack(10);
			this.uriEscapingBuffer = new byte[5];
			this.currentElementProperties = ElementProperties.DEFAULT;
			this.mediaType = settings.MediaType;
			this.doNotEscapeUriAttributes = settings.DoNotEscapeUriAttributes;
		}

		protected void WriteMetaElement()
		{
			base.RawText("<META http-equiv=\"Content-Type\"");
			if (this.mediaType == null)
			{
				this.mediaType = "text/html";
			}
			base.RawText(" content=\"");
			base.RawText(this.mediaType);
			base.RawText("; charset=");
			base.RawText(this.encoding.WebName);
			base.RawText("\">");
		}

		protected unsafe void WriteHtmlElementTextBlock(char* pSrc, char* pSrcEnd)
		{
			if ((this.currentElementProperties & ElementProperties.NO_ENTITIES) != ElementProperties.DEFAULT)
			{
				base.RawText(pSrc, pSrcEnd);
				return;
			}
			base.WriteElementTextBlock(pSrc, pSrcEnd);
		}

		protected unsafe void WriteHtmlAttributeTextBlock(char* pSrc, char* pSrcEnd)
		{
			if ((this.currentAttributeProperties & (AttributeProperties)7U) != AttributeProperties.DEFAULT)
			{
				if ((this.currentAttributeProperties & AttributeProperties.BOOLEAN) != AttributeProperties.DEFAULT)
				{
					return;
				}
				if ((this.currentAttributeProperties & (AttributeProperties)5U) != AttributeProperties.DEFAULT && !this.doNotEscapeUriAttributes)
				{
					this.WriteUriAttributeText(pSrc, pSrcEnd);
					return;
				}
				this.WriteHtmlAttributeText(pSrc, pSrcEnd);
				return;
			}
			else
			{
				if ((this.currentElementProperties & ElementProperties.HAS_NS) != ElementProperties.DEFAULT)
				{
					base.WriteAttributeTextBlock(pSrc, pSrcEnd);
					return;
				}
				this.WriteHtmlAttributeText(pSrc, pSrcEnd);
				return;
			}
		}

		private unsafe void WriteHtmlAttributeText(char* pSrc, char* pSrcEnd)
		{
			if (this.endsWithAmpersand)
			{
				if ((long)(pSrcEnd - pSrc) > 0L && *pSrc != '{')
				{
					this.OutputRestAmps();
				}
				this.endsWithAmpersand = false;
			}
			fixed (byte* bufBytes = this.bufBytes)
			{
				byte* ptr = bufBytes + this.bufPos;
				char c = '\0';
				for (;;)
				{
					byte* ptr2 = ptr + (long)(pSrcEnd - pSrc);
					if (ptr2 != bufBytes + this.bufLen)
					{
						ptr2 = bufBytes + this.bufLen;
					}
					while (ptr < ptr2 && (this.xmlCharType.charProperties[c = *pSrc] & 128) != 0 && c <= '\u007f')
					{
						*(ptr++) = (byte)c;
						pSrc++;
					}
					if (pSrc >= pSrcEnd)
					{
						break;
					}
					if (ptr < ptr2)
					{
						char c2 = c;
						if (c2 <= '"')
						{
							switch (c2)
							{
							case '\t':
								goto IL_0159;
							case '\n':
								ptr = XmlUtf8RawTextWriter.LineFeedEntity(ptr);
								goto IL_0186;
							case '\v':
							case '\f':
								break;
							case '\r':
								ptr = XmlUtf8RawTextWriter.CarriageReturnEntity(ptr);
								goto IL_0186;
							default:
								if (c2 == '"')
								{
									ptr = XmlUtf8RawTextWriter.QuoteEntity(ptr);
									goto IL_0186;
								}
								break;
							}
						}
						else
						{
							switch (c2)
							{
							case '&':
								if (pSrc + 1 == pSrcEnd)
								{
									this.endsWithAmpersand = true;
								}
								else if (pSrc[1] != '{')
								{
									ptr = XmlUtf8RawTextWriter.AmpEntity(ptr);
									goto IL_0186;
								}
								*(ptr++) = (byte)c;
								goto IL_0186;
							case '\'':
								goto IL_0159;
							default:
								switch (c2)
								{
								case '<':
								case '>':
									goto IL_0159;
								}
								break;
							}
						}
						base.EncodeChar(ref pSrc, pSrcEnd, ref ptr);
						continue;
						IL_0186:
						pSrc++;
						continue;
						IL_0159:
						*(ptr++) = (byte)c;
						goto IL_0186;
					}
					this.bufPos = (int)((long)(ptr - bufBytes));
					this.FlushBuffer();
					ptr = bufBytes + 1;
				}
				this.bufPos = (int)((long)(ptr - bufBytes));
			}
		}

		private unsafe void WriteUriAttributeText(char* pSrc, char* pSrcEnd)
		{
			if (this.endsWithAmpersand)
			{
				if ((long)(pSrcEnd - pSrc) > 0L && *pSrc != '{')
				{
					this.OutputRestAmps();
				}
				this.endsWithAmpersand = false;
			}
			fixed (byte* bufBytes = this.bufBytes)
			{
				byte* ptr = bufBytes + this.bufPos;
				char c = '\0';
				for (;;)
				{
					byte* ptr2 = ptr + (long)(pSrcEnd - pSrc);
					if (ptr2 != bufBytes + this.bufLen)
					{
						ptr2 = bufBytes + this.bufLen;
					}
					while (ptr < ptr2 && (this.xmlCharType.charProperties[c = *pSrc] & 128) != 0 && c < '\u0080')
					{
						*(ptr++) = (byte)c;
						pSrc++;
					}
					if (pSrc >= pSrcEnd)
					{
						break;
					}
					if (ptr < ptr2)
					{
						char c2 = c;
						if (c2 <= '"')
						{
							switch (c2)
							{
							case '\t':
								goto IL_0168;
							case '\n':
								ptr = XmlUtf8RawTextWriter.LineFeedEntity(ptr);
								goto IL_0212;
							case '\v':
							case '\f':
								break;
							case '\r':
								ptr = XmlUtf8RawTextWriter.CarriageReturnEntity(ptr);
								goto IL_0212;
							default:
								if (c2 == '"')
								{
									ptr = XmlUtf8RawTextWriter.QuoteEntity(ptr);
									goto IL_0212;
								}
								break;
							}
						}
						else
						{
							switch (c2)
							{
							case '&':
								if (pSrc + 1 == pSrcEnd)
								{
									this.endsWithAmpersand = true;
								}
								else if (pSrc[1] != '{')
								{
									ptr = XmlUtf8RawTextWriter.AmpEntity(ptr);
									goto IL_0212;
								}
								*(ptr++) = (byte)c;
								goto IL_0212;
							case '\'':
								goto IL_0168;
							default:
								switch (c2)
								{
								case '<':
								case '>':
									goto IL_0168;
								}
								break;
							}
						}
						fixed (byte* ptr3 = this.uriEscapingBuffer)
						{
							byte* ptr4 = ptr3;
							byte* ptr5 = ptr4;
							XmlUtf8RawTextWriter.CharToUTF8(ref pSrc, pSrcEnd, ref ptr5);
							while (ptr4 < ptr5)
							{
								*(ptr++) = 37;
								*(ptr++) = (byte)"0123456789ABCDEF"[*ptr4 >> 4];
								*(ptr++) = (byte)"0123456789ABCDEF"[(int)(*ptr4 & 15)];
								ptr4++;
							}
						}
						continue;
						IL_0212:
						pSrc++;
						continue;
						IL_0168:
						*(ptr++) = (byte)c;
						goto IL_0212;
					}
					this.bufPos = (int)((long)(ptr - bufBytes));
					this.FlushBuffer();
					ptr = bufBytes + 1;
				}
				this.bufPos = (int)((long)(ptr - bufBytes));
			}
		}

		private void OutputRestAmps()
		{
			this.bufBytes[this.bufPos++] = 97;
			this.bufBytes[this.bufPos++] = 109;
			this.bufBytes[this.bufPos++] = 112;
			this.bufBytes[this.bufPos++] = 59;
		}

		private const int StackIncrement = 10;

		protected ByteStack elementScope;

		protected ElementProperties currentElementProperties;

		private AttributeProperties currentAttributeProperties;

		private bool endsWithAmpersand;

		private byte[] uriEscapingBuffer;

		private string mediaType;

		private bool doNotEscapeUriAttributes;

		protected static TernaryTreeReadOnly elementPropertySearch;

		protected static TernaryTreeReadOnly attributePropertySearch;
	}
}
