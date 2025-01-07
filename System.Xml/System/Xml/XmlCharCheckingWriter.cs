using System;
using System.Text;

namespace System.Xml
{
	internal class XmlCharCheckingWriter : XmlWrappingWriter
	{
		internal XmlCharCheckingWriter(XmlWriter baseWriter, bool checkValues, bool checkNames, bool replaceNewLines, string newLineChars)
			: base(baseWriter)
		{
			this.checkValues = checkValues;
			this.checkNames = checkNames;
			this.replaceNewLines = replaceNewLines;
			this.newLineChars = newLineChars;
			if (checkValues)
			{
				this.xmlCharType = XmlCharType.Instance;
			}
		}

		public override XmlWriterSettings Settings
		{
			get
			{
				XmlWriterSettings xmlWriterSettings = this.writer.Settings;
				if (xmlWriterSettings == null)
				{
					xmlWriterSettings = new XmlWriterSettings();
				}
				else
				{
					xmlWriterSettings = xmlWriterSettings.Clone();
				}
				if (this.checkValues)
				{
					xmlWriterSettings.CheckCharacters = true;
				}
				if (this.replaceNewLines)
				{
					xmlWriterSettings.NewLineHandling = NewLineHandling.Replace;
					xmlWriterSettings.NewLineChars = this.newLineChars;
				}
				xmlWriterSettings.ReadOnly = true;
				return xmlWriterSettings;
			}
		}

		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			if (this.checkNames)
			{
				this.ValidateQName(name);
			}
			if (this.checkValues)
			{
				int num;
				if (pubid != null && (num = this.xmlCharType.IsPublicId(pubid)) >= 0)
				{
					throw XmlConvert.CreateInvalidCharException(pubid[num]);
				}
				if (sysid != null)
				{
					this.CheckCharacters(sysid);
				}
				if (subset != null)
				{
					this.CheckCharacters(subset);
				}
			}
			if (this.replaceNewLines)
			{
				sysid = this.ReplaceNewLines(sysid);
				pubid = this.ReplaceNewLines(pubid);
				subset = this.ReplaceNewLines(subset);
			}
			this.writer.WriteDocType(name, pubid, sysid, subset);
		}

		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			if (this.checkNames)
			{
				if (localName == null || localName.Length == 0)
				{
					throw new ArgumentException(Res.GetString("Xml_EmptyLocalName"));
				}
				this.ValidateNCName(localName);
				if (prefix != null && prefix.Length > 0)
				{
					this.ValidateNCName(prefix);
				}
			}
			this.writer.WriteStartElement(prefix, localName, ns);
		}

		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			if (this.checkNames)
			{
				if (localName == null || localName.Length == 0)
				{
					throw new ArgumentException(Res.GetString("Xml_EmptyLocalName"));
				}
				this.ValidateNCName(localName);
				if (prefix != null && prefix.Length > 0)
				{
					this.ValidateNCName(prefix);
				}
			}
			this.writer.WriteStartAttribute(prefix, localName, ns);
		}

		public override void WriteCData(string text)
		{
			if (text != null)
			{
				if (this.checkValues)
				{
					this.CheckCharacters(text);
				}
				if (this.replaceNewLines)
				{
					text = this.ReplaceNewLines(text);
				}
				int num;
				while ((num = text.IndexOf("]]>", StringComparison.Ordinal)) >= 0)
				{
					this.writer.WriteCData(text.Substring(0, num + 2));
					text = text.Substring(num + 2);
				}
			}
			this.writer.WriteCData(text);
		}

		public override void WriteComment(string text)
		{
			if (text != null)
			{
				if (this.checkValues)
				{
					this.CheckCharacters(text);
					text = this.InterleaveInvalidChars(text, '-', '-');
				}
				if (this.replaceNewLines)
				{
					text = this.ReplaceNewLines(text);
				}
			}
			this.writer.WriteComment(text);
		}

		public override void WriteProcessingInstruction(string name, string text)
		{
			if (this.checkNames)
			{
				this.ValidateNCName(name);
			}
			if (text != null)
			{
				if (this.checkValues)
				{
					this.CheckCharacters(text);
					text = this.InterleaveInvalidChars(text, '?', '>');
				}
				if (this.replaceNewLines)
				{
					text = this.ReplaceNewLines(text);
				}
			}
			this.writer.WriteProcessingInstruction(name, text);
		}

		public override void WriteEntityRef(string name)
		{
			if (this.checkNames)
			{
				this.ValidateQName(name);
			}
			this.writer.WriteEntityRef(name);
		}

		public override void WriteWhitespace(string ws)
		{
			int num;
			if (this.checkNames && (num = this.xmlCharType.IsOnlyWhitespaceWithPos(ws)) != -1)
			{
				throw new ArgumentException(Res.GetString("Xml_InvalidWhitespaceCharacter", XmlException.BuildCharExceptionStr(ws[num])));
			}
			this.writer.WriteWhitespace(ws);
		}

		public override void WriteString(string text)
		{
			if (text != null)
			{
				if (this.checkValues)
				{
					this.CheckCharacters(text);
				}
				if (this.replaceNewLines && this.WriteState != WriteState.Attribute)
				{
					text = this.ReplaceNewLines(text);
				}
			}
			this.writer.WriteString(text);
		}

		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			this.writer.WriteSurrogateCharEntity(lowChar, highChar);
		}

		public override void WriteChars(char[] buffer, int index, int count)
		{
			if (this.checkValues)
			{
				this.CheckCharacters(buffer, index, count);
			}
			if (this.replaceNewLines && this.WriteState != WriteState.Attribute)
			{
				string text = this.ReplaceNewLines(buffer, index, count);
				if (text != null)
				{
					this.WriteString(text);
					return;
				}
			}
			this.writer.WriteChars(buffer, index, count);
		}

		public override void WriteNmToken(string name)
		{
			if (this.checkNames)
			{
				if (name == null || name.Length == 0)
				{
					throw new ArgumentException(Res.GetString("Xml_EmptyName"));
				}
				XmlConvert.VerifyNMTOKEN(name);
			}
			this.writer.WriteNmToken(name);
		}

		public override void WriteName(string name)
		{
			if (this.checkNames)
			{
				XmlConvert.VerifyQName(name);
			}
			this.writer.WriteName(name);
		}

		public override void WriteQualifiedName(string localName, string ns)
		{
			if (this.checkNames)
			{
				this.ValidateNCName(localName);
			}
			this.writer.WriteQualifiedName(localName, ns);
		}

		private void CheckCharacters(string str)
		{
			XmlConvert.VerifyCharData(str, ExceptionType.ArgumentException);
		}

		private void CheckCharacters(char[] data, int offset, int len)
		{
			XmlConvert.VerifyCharData(data, offset, len, ExceptionType.ArgumentException);
		}

		private void ValidateNCName(string ncname)
		{
			if (ncname.Length == 0)
			{
				throw new ArgumentException(Res.GetString("Xml_EmptyName"));
			}
			int num = ValidateNames.ParseNCName(ncname, 0);
			if (num != ncname.Length)
			{
				throw new ArgumentException(Res.GetString((num == 0) ? "Xml_BadStartNameChar" : "Xml_BadNameChar", XmlException.BuildCharExceptionStr(ncname[num])));
			}
		}

		private void ValidateQName(string name)
		{
			if (name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("Xml_EmptyName"));
			}
			int num2;
			int num = ValidateNames.ParseQName(name, 0, out num2);
			if (num != name.Length)
			{
				string text = ((num == 0 || (num2 > -1 && num == num2 + 1)) ? "Xml_BadStartNameChar" : "Xml_BadNameChar");
				throw new ArgumentException(Res.GetString(text, XmlException.BuildCharExceptionStr(name[num])));
			}
		}

		private string ReplaceNewLines(string str)
		{
			if (str == null)
			{
				return null;
			}
			StringBuilder stringBuilder = null;
			int num = 0;
			int i;
			for (i = 0; i < str.Length; i++)
			{
				char c;
				if ((c = str[i]) < ' ')
				{
					if (c == '\n')
					{
						if (this.newLineChars == "\n")
						{
							goto IL_00F7;
						}
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(str.Length + 5);
						}
						stringBuilder.Append(str, num, i - num);
					}
					else
					{
						if (c != '\r')
						{
							goto IL_00F7;
						}
						if (i + 1 < str.Length && str[i + 1] == '\n')
						{
							if (this.newLineChars == "\r\n")
							{
								i++;
								goto IL_00F7;
							}
							if (stringBuilder == null)
							{
								stringBuilder = new StringBuilder(str.Length + 5);
							}
							stringBuilder.Append(str, num, i - num);
							i++;
						}
						else
						{
							if (this.newLineChars == "\r")
							{
								goto IL_00F7;
							}
							if (stringBuilder == null)
							{
								stringBuilder = new StringBuilder(str.Length + 5);
							}
							stringBuilder.Append(str, num, i - num);
						}
					}
					stringBuilder.Append(this.newLineChars);
					num = i + 1;
				}
				IL_00F7:;
			}
			if (stringBuilder == null)
			{
				return str;
			}
			stringBuilder.Append(str, num, i - num);
			return stringBuilder.ToString();
		}

		private string ReplaceNewLines(char[] data, int offset, int len)
		{
			if (data == null)
			{
				return null;
			}
			StringBuilder stringBuilder = null;
			int num = offset;
			int num2 = offset + len;
			int i;
			for (i = offset; i < num2; i++)
			{
				char c;
				if ((c = data[i]) < ' ')
				{
					if (c == '\n')
					{
						if (this.newLineChars == "\n")
						{
							goto IL_00D6;
						}
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(len + 5);
						}
						stringBuilder.Append(data, num, i - num);
					}
					else
					{
						if (c != '\r')
						{
							goto IL_00D6;
						}
						if (i + 1 < num2 && data[i + 1] == '\n')
						{
							if (this.newLineChars == "\r\n")
							{
								goto IL_00D6;
							}
							if (stringBuilder == null)
							{
								stringBuilder = new StringBuilder(len + 5);
							}
							stringBuilder.Append(data, num, i - num);
							i++;
						}
						else
						{
							if (this.newLineChars == "\r")
							{
								goto IL_00D6;
							}
							if (stringBuilder == null)
							{
								stringBuilder = new StringBuilder(len + 5);
							}
							stringBuilder.Append(data, num, i - num);
						}
					}
					stringBuilder.Append(this.newLineChars);
					num = i + 1;
				}
				IL_00D6:;
			}
			if (stringBuilder == null)
			{
				return null;
			}
			stringBuilder.Append(data, num, i - num);
			return stringBuilder.ToString();
		}

		private string InterleaveInvalidChars(string text, char invChar1, char invChar2)
		{
			StringBuilder stringBuilder = null;
			int num = 0;
			int i;
			for (i = 0; i < text.Length; i++)
			{
				if (text[i] == invChar2 && i > 0 && text[i - 1] == invChar1)
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(text.Length + 5);
					}
					stringBuilder.Append(text, num, i - num);
					stringBuilder.Append(' ');
					num = i;
				}
			}
			if (stringBuilder != null)
			{
				stringBuilder.Append(text, num, i - num);
				if (i > 0 && text[i - 1] == invChar1)
				{
					stringBuilder.Append(' ');
				}
				return stringBuilder.ToString();
			}
			if (i != 0 && text[i - 1] == invChar1)
			{
				return text + ' ';
			}
			return text;
		}

		private bool checkValues;

		private bool checkNames;

		private bool replaceNewLines;

		private string newLineChars;

		private XmlCharType xmlCharType;
	}
}
