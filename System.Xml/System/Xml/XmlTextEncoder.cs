using System;
using System.Globalization;
using System.IO;

namespace System.Xml
{
	internal class XmlTextEncoder
	{
		internal XmlTextEncoder(TextWriter textWriter)
		{
			this.textWriter = textWriter;
			this.quoteChar = '"';
			this.xmlCharType = XmlCharType.Instance;
		}

		internal char QuoteChar
		{
			set
			{
				this.quoteChar = value;
			}
		}

		internal void StartAttribute(bool cacheAttrValue)
		{
			this.inAttribute = true;
			this.cacheAttrValue = cacheAttrValue;
			if (cacheAttrValue)
			{
				if (this.attrValue == null)
				{
					this.attrValue = new BufferBuilder();
					return;
				}
				this.attrValue.Clear();
			}
		}

		internal void EndAttribute()
		{
			if (this.cacheAttrValue)
			{
				this.attrValue.Clear();
			}
			this.inAttribute = false;
			this.cacheAttrValue = false;
		}

		internal string AttributeValue
		{
			get
			{
				if (this.cacheAttrValue)
				{
					return this.attrValue.ToString();
				}
				return string.Empty;
			}
		}

		internal void WriteSurrogateChar(char lowChar, char highChar)
		{
			if (lowChar < '\udc00' || lowChar > '\udfff' || highChar < '\ud800' || highChar > '\udbff')
			{
				throw XmlConvert.CreateInvalidSurrogatePairException(lowChar, highChar);
			}
			this.textWriter.Write(highChar);
			this.textWriter.Write(lowChar);
		}

		internal unsafe void Write(char[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (0 > offset)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (0 > count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > array.Length - offset)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.cacheAttrValue)
			{
				this.attrValue.Append(array, offset, count);
			}
			int num = offset + count;
			int num2 = offset;
			char c = '\0';
			for (;;)
			{
				int num3 = num2;
				while (num2 < num && (this.xmlCharType.charProperties[c = array[num2]] & 128) != 0)
				{
					num2++;
				}
				if (num3 < num2)
				{
					this.textWriter.Write(array, num3, num2 - num3);
				}
				if (num2 == num)
				{
					break;
				}
				char c2 = c;
				if (c2 <= '"')
				{
					switch (c2)
					{
					case '\t':
						this.textWriter.Write(c);
						break;
					case '\n':
					case '\r':
						if (this.inAttribute)
						{
							this.WriteCharEntityImpl(c);
						}
						else
						{
							this.textWriter.Write(c);
						}
						break;
					case '\v':
					case '\f':
						goto IL_01C4;
					default:
						if (c2 != '"')
						{
							goto IL_01C4;
						}
						if (this.inAttribute && this.quoteChar == c)
						{
							this.WriteEntityRefImpl("quot");
						}
						else
						{
							this.textWriter.Write('"');
						}
						break;
					}
				}
				else
				{
					switch (c2)
					{
					case '&':
						this.WriteEntityRefImpl("amp");
						break;
					case '\'':
						if (this.inAttribute && this.quoteChar == c)
						{
							this.WriteEntityRefImpl("apos");
						}
						else
						{
							this.textWriter.Write('\'');
						}
						break;
					default:
						switch (c2)
						{
						case '<':
							this.WriteEntityRefImpl("lt");
							break;
						case '=':
							goto IL_01C4;
						case '>':
							this.WriteEntityRefImpl("gt");
							break;
						default:
							goto IL_01C4;
						}
						break;
					}
				}
				IL_0218:
				num2++;
				continue;
				IL_01C4:
				if (c >= '\ud800' && c <= '\udbff')
				{
					if (num2 + 1 < num)
					{
						this.WriteSurrogateChar(array[++num2], c);
						goto IL_0218;
					}
					goto IL_01EA;
				}
				else
				{
					if (c >= '\udc00' && c <= '\udfff')
					{
						goto Block_23;
					}
					this.WriteCharEntityImpl(c);
					goto IL_0218;
				}
			}
			return;
			IL_01EA:
			throw new ArgumentException(Res.GetString("Xml_SurrogatePairSplit"));
			Block_23:
			throw XmlConvert.CreateInvalidHighSurrogateCharException(c);
		}

		internal unsafe void Write(char ch)
		{
			if (this.cacheAttrValue)
			{
				this.attrValue.Append(ch);
			}
			bool flag = (this.xmlCharType.charProperties[ch] & 128) != 0;
			if (flag)
			{
				this.textWriter.Write(ch);
				return;
			}
			if (ch <= '"')
			{
				switch (ch)
				{
				case '\t':
					this.textWriter.Write(ch);
					return;
				case '\n':
				case '\r':
					if (this.inAttribute)
					{
						this.WriteCharEntityImpl(ch);
						return;
					}
					this.textWriter.Write(ch);
					return;
				case '\v':
				case '\f':
					break;
				default:
					if (ch == '"')
					{
						if (this.inAttribute && this.quoteChar == ch)
						{
							this.WriteEntityRefImpl("quot");
							return;
						}
						this.textWriter.Write('"');
						return;
					}
					break;
				}
			}
			else
			{
				switch (ch)
				{
				case '&':
					this.WriteEntityRefImpl("amp");
					return;
				case '\'':
					if (this.inAttribute && this.quoteChar == ch)
					{
						this.WriteEntityRefImpl("apos");
						return;
					}
					this.textWriter.Write('\'');
					return;
				default:
					switch (ch)
					{
					case '<':
						this.WriteEntityRefImpl("lt");
						return;
					case '>':
						this.WriteEntityRefImpl("gt");
						return;
					}
					break;
				}
			}
			if (ch >= '\ud800' && ch <= '\udbff')
			{
				throw new ArgumentException(Res.GetString("Xml_InvalidSurrogateMissingLowChar"));
			}
			if (ch >= '\udc00' && ch <= '\udfff')
			{
				throw XmlConvert.CreateInvalidHighSurrogateCharException(ch);
			}
			this.WriteCharEntityImpl(ch);
		}

		internal void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			if (lowChar < '\udc00' || lowChar > '\udfff' || highChar < '\ud800' || highChar > '\udbff')
			{
				throw XmlConvert.CreateInvalidSurrogatePairException(lowChar, highChar);
			}
			int num = (int)(lowChar - '\udc00') | ((int)((int)(highChar - '\ud800') << 10) + 65536);
			if (this.cacheAttrValue)
			{
				this.attrValue.Append(highChar);
				this.attrValue.Append(lowChar);
			}
			this.textWriter.Write("&#x");
			this.textWriter.Write(num.ToString("X", NumberFormatInfo.InvariantInfo));
			this.textWriter.Write(';');
		}

		internal unsafe void Write(string text)
		{
			if (text == null)
			{
				return;
			}
			if (this.cacheAttrValue)
			{
				this.attrValue.Append(text);
			}
			int length = text.Length;
			int i = 0;
			int num = 0;
			char c = '\0';
			for (;;)
			{
				if (i >= length || (this.xmlCharType.charProperties[c = text[i]] & 128) == 0)
				{
					if (i == length)
					{
						break;
					}
					if (this.inAttribute)
					{
						if (c != '\t')
						{
							goto IL_0090;
						}
						i++;
					}
					else
					{
						if (c != '\t' && c != '\n' && c != '\r' && c != '"' && c != '\'')
						{
							goto IL_0090;
						}
						i++;
					}
				}
				else
				{
					i++;
				}
			}
			this.textWriter.Write(text);
			return;
			IL_0090:
			char[] array = new char[256];
			for (;;)
			{
				if (num < i)
				{
					this.WriteStringFragment(text, num, i - num, array);
				}
				if (i == length)
				{
					break;
				}
				char c2 = c;
				if (c2 <= '"')
				{
					switch (c2)
					{
					case '\t':
						this.textWriter.Write(c);
						break;
					case '\n':
					case '\r':
						if (this.inAttribute)
						{
							this.WriteCharEntityImpl(c);
						}
						else
						{
							this.textWriter.Write(c);
						}
						break;
					case '\v':
					case '\f':
						goto IL_01DA;
					default:
						if (c2 != '"')
						{
							goto IL_01DA;
						}
						if (this.inAttribute && this.quoteChar == c)
						{
							this.WriteEntityRefImpl("quot");
						}
						else
						{
							this.textWriter.Write('"');
						}
						break;
					}
				}
				else
				{
					switch (c2)
					{
					case '&':
						this.WriteEntityRefImpl("amp");
						break;
					case '\'':
						if (this.inAttribute && this.quoteChar == c)
						{
							this.WriteEntityRefImpl("apos");
						}
						else
						{
							this.textWriter.Write('\'');
						}
						break;
					default:
						switch (c2)
						{
						case '<':
							this.WriteEntityRefImpl("lt");
							break;
						case '=':
							goto IL_01DA;
						case '>':
							this.WriteEntityRefImpl("gt");
							break;
						default:
							goto IL_01DA;
						}
						break;
					}
				}
				IL_0230:
				i++;
				num = i;
				while (i < length)
				{
					if ((this.xmlCharType.charProperties[c = text[i]] & 128) == 0)
					{
						break;
					}
					i++;
				}
				continue;
				IL_01DA:
				if (c >= '\ud800' && c <= '\udbff')
				{
					if (i + 1 < length)
					{
						this.WriteSurrogateChar(text[++i], c);
						goto IL_0230;
					}
					goto IL_0204;
				}
				else
				{
					if (c >= '\udc00' && c <= '\udfff')
					{
						goto Block_27;
					}
					this.WriteCharEntityImpl(c);
					goto IL_0230;
				}
			}
			return;
			IL_0204:
			throw XmlConvert.CreateInvalidSurrogatePairException(text[i], c);
			Block_27:
			throw XmlConvert.CreateInvalidHighSurrogateCharException(c);
		}

		internal unsafe void WriteRawWithSurrogateChecking(string text)
		{
			if (text == null)
			{
				return;
			}
			if (this.cacheAttrValue)
			{
				this.attrValue.Append(text);
			}
			int length = text.Length;
			int num = 0;
			char c = '\0';
			char c2;
			for (;;)
			{
				if (num >= length || ((this.xmlCharType.charProperties[c = text[num]] & 16) == 0 && c >= ' '))
				{
					if (num == length)
					{
						goto IL_00BF;
					}
					if (c >= '\ud800' && c <= '\udbff')
					{
						if (num + 1 >= length)
						{
							goto IL_008F;
						}
						c2 = text[num + 1];
						if (c2 < '\udc00' || c2 > '\udfff')
						{
							break;
						}
						num += 2;
					}
					else
					{
						if (c >= '\udc00' && c <= '\udfff')
						{
							goto Block_12;
						}
						num++;
					}
				}
				else
				{
					num++;
				}
			}
			throw XmlConvert.CreateInvalidSurrogatePairException(c2, c);
			IL_008F:
			throw new ArgumentException(Res.GetString("Xml_InvalidSurrogateMissingLowChar"));
			Block_12:
			throw XmlConvert.CreateInvalidHighSurrogateCharException(c);
			IL_00BF:
			this.textWriter.Write(text);
		}

		internal void WriteRaw(string value)
		{
			if (this.cacheAttrValue)
			{
				this.attrValue.Append(value);
			}
			this.textWriter.Write(value);
		}

		internal void WriteRaw(char[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (0 > count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (0 > offset)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count > array.Length - offset)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.cacheAttrValue)
			{
				this.attrValue.Append(array, offset, count);
			}
			this.textWriter.Write(array, offset, count);
		}

		internal void WriteCharEntity(char ch)
		{
			if (ch >= '\ud800' && ch <= '\udfff')
			{
				throw new ArgumentException(Res.GetString("Xml_InvalidSurrogateMissingLowChar"));
			}
			int num = (int)ch;
			string text = num.ToString("X", NumberFormatInfo.InvariantInfo);
			if (this.cacheAttrValue)
			{
				this.attrValue.Append("&#x");
				this.attrValue.Append(text);
				this.attrValue.Append(';');
			}
			this.WriteCharEntityImpl(text);
		}

		internal void WriteEntityRef(string name)
		{
			if (this.cacheAttrValue)
			{
				this.attrValue.Append('&');
				this.attrValue.Append(name);
				this.attrValue.Append(';');
			}
			this.WriteEntityRefImpl(name);
		}

		internal void Flush()
		{
		}

		private void WriteStringFragment(string str, int offset, int count, char[] helperBuffer)
		{
			int num = helperBuffer.Length;
			while (count > 0)
			{
				int num2 = count;
				if (num2 > num)
				{
					num2 = num;
				}
				str.CopyTo(offset, helperBuffer, 0, num2);
				this.textWriter.Write(helperBuffer, 0, num2);
				offset += num2;
				count -= num2;
			}
		}

		private void WriteCharEntityImpl(char ch)
		{
			int num = (int)ch;
			this.WriteCharEntityImpl(num.ToString("X", NumberFormatInfo.InvariantInfo));
		}

		private void WriteCharEntityImpl(string strVal)
		{
			this.textWriter.Write("&#x");
			this.textWriter.Write(strVal);
			this.textWriter.Write(';');
		}

		private void WriteEntityRefImpl(string name)
		{
			this.textWriter.Write('&');
			this.textWriter.Write(name);
			this.textWriter.Write(';');
		}

		private const int SurHighStart = 55296;

		private const int SurHighEnd = 56319;

		private const int SurLowStart = 56320;

		private const int SurLowEnd = 57343;

		private TextWriter textWriter;

		private bool inAttribute;

		private char quoteChar;

		private BufferBuilder attrValue;

		private bool cacheAttrValue;

		private XmlCharType xmlCharType;
	}
}
