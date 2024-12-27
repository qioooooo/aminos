using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020000B4 RID: 180
	internal class XmlUtilWriter
	{
		// Token: 0x060006CD RID: 1741 RVA: 0x0001F114 File Offset: 0x0001E114
		internal XmlUtilWriter(TextWriter writer, bool trackPosition)
		{
			this._writer = writer;
			this._trackPosition = trackPosition;
			this._lineNumber = 1;
			this._linePosition = 1;
			this._isLastLineBlank = true;
			if (this._trackPosition)
			{
				this._baseStream = ((StreamWriter)this._writer).BaseStream;
				this._lineStartCheckpoint = this.CreateStreamCheckpoint();
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x0001F174 File Offset: 0x0001E174
		internal TextWriter Writer
		{
			get
			{
				return this._writer;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x0001F17C File Offset: 0x0001E17C
		internal bool TrackPosition
		{
			get
			{
				return this._trackPosition;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x0001F184 File Offset: 0x0001E184
		internal int LineNumber
		{
			get
			{
				return this._lineNumber;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0001F18C File Offset: 0x0001E18C
		internal int LinePosition
		{
			get
			{
				return this._linePosition;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x0001F194 File Offset: 0x0001E194
		internal bool IsLastLineBlank
		{
			get
			{
				return this._isLastLineBlank;
			}
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001F19C File Offset: 0x0001E19C
		private void UpdatePosition(char ch)
		{
			switch (ch)
			{
			case '\t':
				break;
			case '\n':
				this._lineStartCheckpoint = this.CreateStreamCheckpoint();
				return;
			case '\v':
			case '\f':
				goto IL_005F;
			case '\r':
				this._lineNumber++;
				this._linePosition = 1;
				this._isLastLineBlank = true;
				return;
			default:
				if (ch != ' ')
				{
					goto IL_005F;
				}
				break;
			}
			this._linePosition++;
			return;
			IL_005F:
			this._linePosition++;
			this._isLastLineBlank = false;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001F220 File Offset: 0x0001E220
		internal int Write(string s)
		{
			if (this._trackPosition)
			{
				foreach (char c in s)
				{
					this._writer.Write(c);
					this.UpdatePosition(c);
				}
			}
			else
			{
				this._writer.Write(s);
			}
			return s.Length;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001F275 File Offset: 0x0001E275
		internal int Write(char ch)
		{
			this._writer.Write(ch);
			if (this._trackPosition)
			{
				this.UpdatePosition(ch);
			}
			return 1;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001F293 File Offset: 0x0001E293
		internal void Flush()
		{
			this._writer.Flush();
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001F2A0 File Offset: 0x0001E2A0
		internal int AppendEscapeTextString(string s)
		{
			return this.AppendEscapeXmlString(s, false, 'A');
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001F2AC File Offset: 0x0001E2AC
		internal int AppendEscapeXmlString(string s, bool inAttribute, char quoteChar)
		{
			int num = 0;
			foreach (char c in s)
			{
				bool flag = false;
				string text = null;
				if ((c < ' ' && c != '\t' && c != '\r' && c != '\n') || c > '\ufffd')
				{
					flag = true;
				}
				else
				{
					char c2 = c;
					if (c2 <= '\r')
					{
						if (c2 == '\n' || c2 == '\r')
						{
							flag = inAttribute;
						}
					}
					else if (c2 != '"')
					{
						switch (c2)
						{
						case '&':
							text = "amp";
							break;
						case '\'':
							if (inAttribute && quoteChar == c)
							{
								text = "apos";
							}
							break;
						default:
							switch (c2)
							{
							case '<':
								text = "lt";
								break;
							case '>':
								text = "gt";
								break;
							}
							break;
						}
					}
					else if (inAttribute && quoteChar == c)
					{
						text = "quot";
					}
				}
				if (flag)
				{
					num += this.AppendCharEntity(c);
				}
				else if (text != null)
				{
					num += this.AppendEntityRef(text);
				}
				else
				{
					num += this.Write(c);
				}
			}
			return num;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001F3B1 File Offset: 0x0001E3B1
		internal int AppendEntityRef(string entityRef)
		{
			this.Write('&');
			this.Write(entityRef);
			this.Write(';');
			return entityRef.Length + 2;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001F3D8 File Offset: 0x0001E3D8
		internal int AppendCharEntity(char ch)
		{
			int num = (int)ch;
			string text = num.ToString("X", CultureInfo.InvariantCulture);
			this.Write('&');
			this.Write('#');
			this.Write('x');
			this.Write(text);
			this.Write(';');
			return text.Length + 4;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0001F42D File Offset: 0x0001E42D
		internal int AppendCData(string cdata)
		{
			this.Write("<![CDATA[");
			this.Write(cdata);
			this.Write("]]>");
			return cdata.Length + 12;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001F458 File Offset: 0x0001E458
		internal int AppendProcessingInstruction(string name, string value)
		{
			this.Write("<?");
			this.Write(name);
			this.AppendSpace();
			this.Write(value);
			this.Write("?>");
			return name.Length + value.Length + 5;
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0001F498 File Offset: 0x0001E498
		internal int AppendComment(string comment)
		{
			this.Write("<!--");
			this.Write(comment);
			this.Write("-->");
			return comment.Length + 7;
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001F4C4 File Offset: 0x0001E4C4
		internal int AppendAttributeValue(XmlTextReader reader)
		{
			int num = 0;
			char c = reader.QuoteChar;
			if (c != '"' && c != '\'')
			{
				c = '"';
			}
			num += this.Write(c);
			while (reader.ReadAttributeValue())
			{
				if (reader.NodeType == XmlNodeType.Text)
				{
					num += this.AppendEscapeXmlString(reader.Value, true, c);
				}
				else
				{
					num += this.AppendEntityRef(reader.Name);
				}
			}
			return num + this.Write(c);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001F534 File Offset: 0x0001E534
		internal int AppendRequiredWhiteSpace(int fromLineNumber, int fromLinePosition, int toLineNumber, int toLinePosition)
		{
			int num = this.AppendWhiteSpace(fromLineNumber, fromLinePosition, toLineNumber, toLinePosition);
			if (num == 0)
			{
				num += this.AppendSpace();
			}
			return num;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0001F55C File Offset: 0x0001E55C
		internal int AppendWhiteSpace(int fromLineNumber, int fromLinePosition, int toLineNumber, int toLinePosition)
		{
			int num = 0;
			while (fromLineNumber++ < toLineNumber)
			{
				num += this.AppendNewLine();
				fromLinePosition = 1;
			}
			return num + this.AppendSpaces(toLinePosition - fromLinePosition);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0001F590 File Offset: 0x0001E590
		internal int AppendIndent(int linePosition, int indent, int depth, bool newLine)
		{
			int num = 0;
			if (newLine)
			{
				num += this.AppendNewLine();
			}
			int num2 = linePosition - 1 + indent * depth;
			return num + this.AppendSpaces(num2);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001F5C0 File Offset: 0x0001E5C0
		internal int AppendSpacesToLinePosition(int linePosition)
		{
			if (linePosition <= 0)
			{
				return 0;
			}
			int num = linePosition - this._linePosition;
			if (num < 0 && this.IsLastLineBlank)
			{
				this.SeekToLineStart();
			}
			return this.AppendSpaces(linePosition - this._linePosition);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001F5FC File Offset: 0x0001E5FC
		internal int AppendNewLine()
		{
			return this.Write("\r\n");
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001F60C File Offset: 0x0001E60C
		internal int AppendSpaces(int count)
		{
			int i = count;
			while (i > 0)
			{
				if (i >= 8)
				{
					this.Write(XmlUtilWriter.SPACES_8);
					i -= 8;
				}
				else if (i >= 4)
				{
					this.Write(XmlUtilWriter.SPACES_4);
					i -= 4;
				}
				else
				{
					if (i < 2)
					{
						this.Write(' ');
						break;
					}
					this.Write(XmlUtilWriter.SPACES_2);
					i -= 2;
				}
			}
			if (count <= 0)
			{
				return 0;
			}
			return count;
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001F675 File Offset: 0x0001E675
		internal int AppendSpace()
		{
			return this.Write(' ');
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0001F67F File Offset: 0x0001E67F
		internal void SeekToLineStart()
		{
			this.RestoreStreamCheckpoint(this._lineStartCheckpoint);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0001F68D File Offset: 0x0001E68D
		internal object CreateStreamCheckpoint()
		{
			return new XmlUtilWriter.StreamWriterCheckpoint(this);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0001F698 File Offset: 0x0001E698
		internal void RestoreStreamCheckpoint(object o)
		{
			XmlUtilWriter.StreamWriterCheckpoint streamWriterCheckpoint = (XmlUtilWriter.StreamWriterCheckpoint)o;
			this.Flush();
			this._lineNumber = streamWriterCheckpoint._lineNumber;
			this._linePosition = streamWriterCheckpoint._linePosition;
			this._isLastLineBlank = streamWriterCheckpoint._isLastLineBlank;
			this._baseStream.Seek(streamWriterCheckpoint._streamPosition, SeekOrigin.Begin);
			this._baseStream.SetLength(streamWriterCheckpoint._streamLength);
			this._baseStream.Flush();
		}

		// Token: 0x04000409 RID: 1033
		private const char SPACE = ' ';

		// Token: 0x0400040A RID: 1034
		private const string NL = "\r\n";

		// Token: 0x0400040B RID: 1035
		private static string SPACES_8 = new string(' ', 8);

		// Token: 0x0400040C RID: 1036
		private static string SPACES_4 = new string(' ', 4);

		// Token: 0x0400040D RID: 1037
		private static string SPACES_2 = new string(' ', 2);

		// Token: 0x0400040E RID: 1038
		private TextWriter _writer;

		// Token: 0x0400040F RID: 1039
		private Stream _baseStream;

		// Token: 0x04000410 RID: 1040
		private bool _trackPosition;

		// Token: 0x04000411 RID: 1041
		private int _lineNumber;

		// Token: 0x04000412 RID: 1042
		private int _linePosition;

		// Token: 0x04000413 RID: 1043
		private bool _isLastLineBlank;

		// Token: 0x04000414 RID: 1044
		private object _lineStartCheckpoint;

		// Token: 0x020000B5 RID: 181
		private class StreamWriterCheckpoint
		{
			// Token: 0x060006E9 RID: 1769 RVA: 0x0001F708 File Offset: 0x0001E708
			internal StreamWriterCheckpoint(XmlUtilWriter writer)
			{
				writer.Flush();
				this._lineNumber = writer._lineNumber;
				this._linePosition = writer._linePosition;
				this._isLastLineBlank = writer._isLastLineBlank;
				writer._baseStream.Flush();
				this._streamPosition = writer._baseStream.Position;
				this._streamLength = writer._baseStream.Length;
			}

			// Token: 0x04000415 RID: 1045
			internal int _lineNumber;

			// Token: 0x04000416 RID: 1046
			internal int _linePosition;

			// Token: 0x04000417 RID: 1047
			internal bool _isLastLineBlank;

			// Token: 0x04000418 RID: 1048
			internal long _streamLength;

			// Token: 0x04000419 RID: 1049
			internal long _streamPosition;
		}
	}
}
