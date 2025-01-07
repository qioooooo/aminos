using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	internal class HtmlEncodedRawTextWriter : XmlEncodedRawTextWriter
	{
		public HtmlEncodedRawTextWriter(TextWriter writer, XmlWriterSettings settings)
			: base(writer, settings)
		{
			this.Init(settings);
		}

		public HtmlEncodedRawTextWriter(Stream stream, Encoding encoding, XmlWriterSettings settings, bool closeOutput)
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
			if (this.trackTextContent && this.inTextContent)
			{
				base.ChangeTextContentMark(false);
			}
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
				this.bufChars[this.bufPos++] = '"';
			}
			else if (sysid != null)
			{
				base.RawText(" SYSTEM \"");
				base.RawText(sysid);
				this.bufChars[this.bufPos++] = '"';
			}
			else
			{
				this.bufChars[this.bufPos++] = ' ';
			}
			if (subset != null)
			{
				this.bufChars[this.bufPos++] = '[';
				base.RawText(subset);
				this.bufChars[this.bufPos++] = ']';
			}
			this.bufChars[this.bufPos++] = '>';
		}

		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.elementScope.Push((byte)this.currentElementProperties);
			if (ns.Length == 0)
			{
				if (this.trackTextContent && this.inTextContent)
				{
					base.ChangeTextContentMark(false);
				}
				this.currentElementProperties = (ElementProperties)HtmlEncodedRawTextWriter.elementPropertySearch.FindCaseInsensitiveString(localName);
				this.bufChars[this.bufPos++] = '<';
				base.RawText(localName);
				this.attrEndPos = this.bufPos;
				return;
			}
			this.currentElementProperties = ElementProperties.HAS_NS;
			base.WriteStartElement(prefix, localName, ns);
		}

		internal override void StartElementContent()
		{
			this.bufChars[this.bufPos++] = '>';
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
				if (this.trackTextContent && this.inTextContent)
				{
					base.ChangeTextContentMark(false);
				}
				if ((this.currentElementProperties & ElementProperties.EMPTY) == ElementProperties.DEFAULT)
				{
					this.bufChars[this.bufPos++] = '<';
					this.bufChars[this.bufPos++] = '/';
					base.RawText(localName);
					this.bufChars[this.bufPos++] = '>';
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
				if (this.trackTextContent && this.inTextContent)
				{
					base.ChangeTextContentMark(false);
				}
				if ((this.currentElementProperties & ElementProperties.EMPTY) == ElementProperties.DEFAULT)
				{
					this.bufChars[this.bufPos++] = '<';
					this.bufChars[this.bufPos++] = '/';
					base.RawText(localName);
					this.bufChars[this.bufPos++] = '>';
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
				if (this.trackTextContent && this.inTextContent)
				{
					base.ChangeTextContentMark(false);
				}
				if (this.attrEndPos == this.bufPos)
				{
					this.bufChars[this.bufPos++] = ' ';
				}
				base.RawText(localName);
				if ((this.currentElementProperties & (ElementProperties)7U) != ElementProperties.DEFAULT)
				{
					this.currentAttributeProperties = (AttributeProperties)((ElementProperties)HtmlEncodedRawTextWriter.attributePropertySearch.FindCaseInsensitiveString(localName) & this.currentElementProperties);
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
				this.bufChars[this.bufPos++] = '=';
				this.bufChars[this.bufPos++] = '"';
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
				if (this.trackTextContent && this.inTextContent)
				{
					base.ChangeTextContentMark(false);
				}
				this.bufChars[this.bufPos++] = '"';
			}
			this.inAttributeValue = false;
			this.attrEndPos = this.bufPos;
		}

		public override void WriteProcessingInstruction(string target, string text)
		{
			if (this.trackTextContent && this.inTextContent)
			{
				base.ChangeTextContentMark(false);
			}
			this.bufChars[this.bufPos++] = '<';
			this.bufChars[this.bufPos++] = '?';
			base.RawText(target);
			this.bufChars[this.bufPos++] = ' ';
			base.WriteCommentOrPi(text, 63);
			this.bufChars[this.bufPos++] = '>';
			if (this.bufPos > this.bufLen)
			{
				this.FlushBuffer();
			}
		}

		public unsafe override void WriteString(string text)
		{
			if (this.trackTextContent && !this.inTextContent)
			{
				base.ChangeTextContentMark(true);
			}
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
			if (this.trackTextContent && !this.inTextContent)
			{
				base.ChangeTextContentMark(true);
			}
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
			if (HtmlEncodedRawTextWriter.elementPropertySearch == null)
			{
				HtmlEncodedRawTextWriter.attributePropertySearch = new TernaryTreeReadOnly(HtmlTernaryTree.htmlAttributes);
				HtmlEncodedRawTextWriter.elementPropertySearch = new TernaryTreeReadOnly(HtmlTernaryTree.htmlElements);
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
			fixed (char* bufChars = this.bufChars)
			{
				char* ptr = bufChars + this.bufPos;
				char c = '\0';
				for (;;)
				{
					char* ptr2 = ptr + (long)(pSrcEnd - pSrc) * 2L / 2L;
					if (ptr2 != bufChars + this.bufLen)
					{
						ptr2 = bufChars + this.bufLen;
					}
					while (ptr < ptr2 && (this.xmlCharType.charProperties[c = *pSrc] & 128) != 0)
					{
						*(ptr++) = c;
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
								goto IL_015E;
							case '\n':
								ptr = XmlEncodedRawTextWriter.LineFeedEntity(ptr);
								goto IL_018A;
							case '\v':
							case '\f':
								break;
							case '\r':
								ptr = XmlEncodedRawTextWriter.CarriageReturnEntity(ptr);
								goto IL_018A;
							default:
								if (c2 == '"')
								{
									ptr = XmlEncodedRawTextWriter.QuoteEntity(ptr);
									goto IL_018A;
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
									ptr = XmlEncodedRawTextWriter.AmpEntity(ptr);
									goto IL_018A;
								}
								*(ptr++) = c;
								goto IL_018A;
							case '\'':
								goto IL_015E;
							default:
								switch (c2)
								{
								case '<':
								case '>':
									goto IL_015E;
								}
								break;
							}
						}
						base.EncodeChar(ref pSrc, pSrcEnd, ref ptr);
						continue;
						IL_018A:
						pSrc++;
						continue;
						IL_015E:
						*(ptr++) = c;
						goto IL_018A;
					}
					this.bufPos = (int)((long)(ptr - bufChars));
					this.FlushBuffer();
					ptr = bufChars + 1;
				}
				this.bufPos = (int)((long)(ptr - bufChars));
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
			fixed (char* bufChars = this.bufChars)
			{
				char* ptr = bufChars + this.bufPos;
				char c = '\0';
				for (;;)
				{
					char* ptr2 = ptr + (long)(pSrcEnd - pSrc) * 2L / 2L;
					if (ptr2 != bufChars + this.bufLen)
					{
						ptr2 = bufChars + this.bufLen;
					}
					while (ptr < ptr2 && (this.xmlCharType.charProperties[c = *pSrc] & 128) != 0 && c < '\u0080')
					{
						*(ptr++) = c;
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
								goto IL_0175;
							case '\n':
								ptr = XmlEncodedRawTextWriter.LineFeedEntity(ptr);
								goto IL_021C;
							case '\v':
							case '\f':
								break;
							case '\r':
								ptr = XmlEncodedRawTextWriter.CarriageReturnEntity(ptr);
								goto IL_021C;
							default:
								if (c2 == '"')
								{
									ptr = XmlEncodedRawTextWriter.QuoteEntity(ptr);
									goto IL_021C;
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
									ptr = XmlEncodedRawTextWriter.AmpEntity(ptr);
									goto IL_021C;
								}
								*(ptr++) = c;
								goto IL_021C;
							case '\'':
								goto IL_0175;
							default:
								switch (c2)
								{
								case '<':
								case '>':
									goto IL_0175;
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
								*(ptr++) = '%';
								*(ptr++) = "0123456789ABCDEF"[*ptr4 >> 4];
								*(ptr++) = "0123456789ABCDEF"[(int)(*ptr4 & 15)];
								ptr4++;
							}
						}
						continue;
						IL_021C:
						pSrc++;
						continue;
						IL_0175:
						*(ptr++) = c;
						goto IL_021C;
					}
					this.bufPos = (int)((long)(ptr - bufChars));
					this.FlushBuffer();
					ptr = bufChars + 1;
				}
				this.bufPos = (int)((long)(ptr - bufChars));
			}
		}

		private void OutputRestAmps()
		{
			this.bufChars[this.bufPos++] = 'a';
			this.bufChars[this.bufPos++] = 'm';
			this.bufChars[this.bufPos++] = 'p';
			this.bufChars[this.bufPos++] = ';';
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
