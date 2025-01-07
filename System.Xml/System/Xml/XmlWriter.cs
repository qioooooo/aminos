using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
	public abstract class XmlWriter : IDisposable
	{
		public virtual XmlWriterSettings Settings
		{
			get
			{
				return null;
			}
		}

		public abstract void WriteStartDocument();

		public abstract void WriteStartDocument(bool standalone);

		public abstract void WriteEndDocument();

		public abstract void WriteDocType(string name, string pubid, string sysid, string subset);

		public void WriteStartElement(string localName, string ns)
		{
			this.WriteStartElement(null, localName, ns);
		}

		public abstract void WriteStartElement(string prefix, string localName, string ns);

		public void WriteStartElement(string localName)
		{
			this.WriteStartElement(null, localName, null);
		}

		public abstract void WriteEndElement();

		public abstract void WriteFullEndElement();

		public void WriteAttributeString(string localName, string ns, string value)
		{
			this.WriteStartAttribute(null, localName, ns);
			this.WriteString(value);
			this.WriteEndAttribute();
		}

		public void WriteAttributeString(string localName, string value)
		{
			this.WriteStartAttribute(null, localName, null);
			this.WriteString(value);
			this.WriteEndAttribute();
		}

		public void WriteAttributeString(string prefix, string localName, string ns, string value)
		{
			this.WriteStartAttribute(prefix, localName, ns);
			this.WriteString(value);
			this.WriteEndAttribute();
		}

		public void WriteStartAttribute(string localName, string ns)
		{
			this.WriteStartAttribute(null, localName, ns);
		}

		public abstract void WriteStartAttribute(string prefix, string localName, string ns);

		public void WriteStartAttribute(string localName)
		{
			this.WriteStartAttribute(null, localName, null);
		}

		public abstract void WriteEndAttribute();

		public abstract void WriteCData(string text);

		public abstract void WriteComment(string text);

		public abstract void WriteProcessingInstruction(string name, string text);

		public abstract void WriteEntityRef(string name);

		public abstract void WriteCharEntity(char ch);

		public abstract void WriteWhitespace(string ws);

		public abstract void WriteString(string text);

		public abstract void WriteSurrogateCharEntity(char lowChar, char highChar);

		public abstract void WriteChars(char[] buffer, int index, int count);

		public abstract void WriteRaw(char[] buffer, int index, int count);

		public abstract void WriteRaw(string data);

		public abstract void WriteBase64(byte[] buffer, int index, int count);

		public virtual void WriteBinHex(byte[] buffer, int index, int count)
		{
			BinHexEncoder.Encode(buffer, index, count, this);
		}

		public abstract WriteState WriteState { get; }

		public abstract void Close();

		public abstract void Flush();

		public abstract string LookupPrefix(string ns);

		public virtual XmlSpace XmlSpace
		{
			get
			{
				return XmlSpace.Default;
			}
		}

		public virtual string XmlLang
		{
			get
			{
				return string.Empty;
			}
		}

		public virtual void WriteNmToken(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("Xml_EmptyName"));
			}
			this.WriteString(XmlConvert.VerifyNMTOKEN(name, ExceptionType.ArgumentException));
		}

		public virtual void WriteName(string name)
		{
			this.WriteString(XmlConvert.VerifyQName(name, ExceptionType.ArgumentException));
		}

		public virtual void WriteQualifiedName(string localName, string ns)
		{
			if (ns != null && ns.Length > 0)
			{
				string text = this.LookupPrefix(ns);
				if (text == null)
				{
					throw new ArgumentException(Res.GetString("Xml_UndefNamespace", new object[] { ns }));
				}
				this.WriteString(text);
				this.WriteString(":");
			}
			this.WriteString(localName);
		}

		public virtual void WriteValue(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value, null));
		}

		public virtual void WriteValue(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.WriteString(value);
		}

		public virtual void WriteValue(bool value)
		{
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value));
		}

		public virtual void WriteValue(DateTime value)
		{
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value));
		}

		public virtual void WriteValue(double value)
		{
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value));
		}

		public virtual void WriteValue(float value)
		{
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value));
		}

		public virtual void WriteValue(decimal value)
		{
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value));
		}

		public virtual void WriteValue(int value)
		{
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value));
		}

		public virtual void WriteValue(long value)
		{
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value));
		}

		public virtual void WriteAttributes(XmlReader reader, bool defattr)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (reader.NodeType == XmlNodeType.Element || reader.NodeType == XmlNodeType.XmlDeclaration)
			{
				if (reader.MoveToFirstAttribute())
				{
					this.WriteAttributes(reader, defattr);
					reader.MoveToElement();
					return;
				}
			}
			else
			{
				if (reader.NodeType != XmlNodeType.Attribute)
				{
					throw new XmlException("Xml_InvalidPosition", string.Empty);
				}
				do
				{
					IXmlSchemaInfo schemaInfo;
					if (defattr || (!reader.IsDefault && ((schemaInfo = reader.SchemaInfo) == null || !schemaInfo.IsDefault)))
					{
						this.WriteStartAttribute(reader.Prefix, reader.LocalName, reader.NamespaceURI);
						while (reader.ReadAttributeValue())
						{
							if (reader.NodeType == XmlNodeType.EntityReference)
							{
								this.WriteEntityRef(reader.Name);
							}
							else
							{
								this.WriteString(reader.Value);
							}
						}
						this.WriteEndAttribute();
					}
				}
				while (reader.MoveToNextAttribute());
			}
		}

		public virtual void WriteNode(XmlReader reader, bool defattr)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			bool canReadValueChunk = reader.CanReadValueChunk;
			int num = ((reader.NodeType == XmlNodeType.None) ? (-1) : reader.Depth);
			do
			{
				switch (reader.NodeType)
				{
				case XmlNodeType.Element:
					this.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
					this.WriteAttributes(reader, defattr);
					if (reader.IsEmptyElement)
					{
						this.WriteEndElement();
					}
					break;
				case XmlNodeType.Text:
					if (canReadValueChunk)
					{
						if (this.writeNodeBuffer == null)
						{
							this.writeNodeBuffer = new char[1024];
						}
						int num2;
						while ((num2 = reader.ReadValueChunk(this.writeNodeBuffer, 0, 1024)) > 0)
						{
							this.WriteChars(this.writeNodeBuffer, 0, num2);
						}
					}
					else
					{
						this.WriteString(reader.Value);
					}
					break;
				case XmlNodeType.CDATA:
					this.WriteCData(reader.Value);
					break;
				case XmlNodeType.EntityReference:
					this.WriteEntityRef(reader.Name);
					break;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.XmlDeclaration:
					this.WriteProcessingInstruction(reader.Name, reader.Value);
					break;
				case XmlNodeType.Comment:
					this.WriteComment(reader.Value);
					break;
				case XmlNodeType.DocumentType:
					this.WriteDocType(reader.Name, reader.GetAttribute("PUBLIC"), reader.GetAttribute("SYSTEM"), reader.Value);
					break;
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					this.WriteWhitespace(reader.Value);
					break;
				case XmlNodeType.EndElement:
					this.WriteFullEndElement();
					break;
				}
			}
			while (reader.Read() && (num < reader.Depth || (num == reader.Depth && reader.NodeType == XmlNodeType.EndElement)));
		}

		public virtual void WriteNode(XPathNavigator navigator, bool defattr)
		{
			if (navigator == null)
			{
				throw new ArgumentNullException("navigator");
			}
			int num = 0;
			navigator = navigator.Clone();
			for (;;)
			{
				IL_0018:
				bool flag = false;
				switch (navigator.NodeType)
				{
				case XPathNodeType.Root:
					flag = true;
					break;
				case XPathNodeType.Element:
					this.WriteStartElement(navigator.Prefix, navigator.LocalName, navigator.NamespaceURI);
					if (navigator.MoveToFirstAttribute())
					{
						do
						{
							IXmlSchemaInfo schemaInfo = navigator.SchemaInfo;
							if (defattr || schemaInfo == null || !schemaInfo.IsDefault)
							{
								this.WriteStartAttribute(navigator.Prefix, navigator.LocalName, navigator.NamespaceURI);
								this.WriteString(navigator.Value);
								this.WriteEndAttribute();
							}
						}
						while (navigator.MoveToNextAttribute());
						navigator.MoveToParent();
					}
					if (navigator.MoveToFirstNamespace(XPathNamespaceScope.Local))
					{
						this.WriteLocalNamespaces(navigator);
						navigator.MoveToParent();
					}
					flag = true;
					break;
				case XPathNodeType.Text:
					this.WriteString(navigator.Value);
					break;
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
					this.WriteWhitespace(navigator.Value);
					break;
				case XPathNodeType.ProcessingInstruction:
					this.WriteProcessingInstruction(navigator.LocalName, navigator.Value);
					break;
				case XPathNodeType.Comment:
					this.WriteComment(navigator.Value);
					break;
				}
				if (flag)
				{
					if (navigator.MoveToFirstChild())
					{
						num++;
						continue;
					}
					if (navigator.NodeType == XPathNodeType.Element)
					{
						if (navigator.IsEmptyElement)
						{
							this.WriteEndElement();
						}
						else
						{
							this.WriteFullEndElement();
						}
					}
				}
				while (num != 0)
				{
					if (navigator.MoveToNext())
					{
						goto IL_0018;
					}
					num--;
					navigator.MoveToParent();
					if (navigator.NodeType == XPathNodeType.Element)
					{
						this.WriteFullEndElement();
					}
				}
				break;
			}
		}

		public void WriteElementString(string localName, string value)
		{
			this.WriteElementString(localName, null, value);
		}

		public void WriteElementString(string localName, string ns, string value)
		{
			this.WriteStartElement(localName, ns);
			if (value != null && value.Length != 0)
			{
				this.WriteString(value);
			}
			this.WriteEndElement();
		}

		public void WriteElementString(string prefix, string localName, string ns, string value)
		{
			this.WriteStartElement(prefix, localName, ns);
			if (value != null && value.Length != 0)
			{
				this.WriteString(value);
			}
			this.WriteEndElement();
		}

		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (this.WriteState != WriteState.Closed)
			{
				try
				{
					this.Close();
				}
				catch
				{
				}
			}
		}

		private void WriteLocalNamespaces(XPathNavigator nsNav)
		{
			string localName = nsNav.LocalName;
			string value = nsNav.Value;
			if (nsNav.MoveToNextNamespace(XPathNamespaceScope.Local))
			{
				this.WriteLocalNamespaces(nsNav);
			}
			if (localName.Length == 0)
			{
				this.WriteAttributeString(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/", value);
				return;
			}
			this.WriteAttributeString("xmlns", localName, "http://www.w3.org/2000/xmlns/", value);
		}

		public static XmlWriter Create(string outputFileName)
		{
			return XmlWriter.Create(outputFileName, null);
		}

		public static XmlWriter Create(string outputFileName, XmlWriterSettings settings)
		{
			if (outputFileName == null)
			{
				throw new ArgumentNullException("outputFileName");
			}
			if (settings == null)
			{
				settings = new XmlWriterSettings();
			}
			FileStream fileStream = null;
			XmlWriter xmlWriter;
			try
			{
				fileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write, FileShare.Read);
				xmlWriter = XmlWriter.CreateWriterImpl(fileStream, settings.Encoding, true, settings);
			}
			catch
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
				throw;
			}
			return xmlWriter;
		}

		public static XmlWriter Create(Stream output)
		{
			return XmlWriter.Create(output, null);
		}

		public static XmlWriter Create(Stream output, XmlWriterSettings settings)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (settings == null)
			{
				settings = new XmlWriterSettings();
			}
			return XmlWriter.CreateWriterImpl(output, settings.Encoding, settings.CloseOutput, settings);
		}

		public static XmlWriter Create(TextWriter output)
		{
			return XmlWriter.Create(output, null);
		}

		public static XmlWriter Create(TextWriter output, XmlWriterSettings settings)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (settings == null)
			{
				settings = new XmlWriterSettings();
			}
			return XmlWriter.CreateWriterImpl(output, settings);
		}

		public static XmlWriter Create(StringBuilder output)
		{
			return XmlWriter.Create(output, null);
		}

		public static XmlWriter Create(StringBuilder output, XmlWriterSettings settings)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (settings == null)
			{
				settings = new XmlWriterSettings();
			}
			return XmlWriter.CreateWriterImpl(new StringWriter(output, CultureInfo.InvariantCulture), settings);
		}

		public static XmlWriter Create(XmlWriter output)
		{
			return XmlWriter.Create(output, null);
		}

		public static XmlWriter Create(XmlWriter output, XmlWriterSettings settings)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (settings == null)
			{
				settings = new XmlWriterSettings();
			}
			return XmlWriter.AddConformanceWrapper(output, output.Settings, settings);
		}

		private static XmlWriter CreateWriterImpl(Stream output, Encoding encoding, bool closeOutput, XmlWriterSettings settings)
		{
			XmlWriter xmlWriter;
			if (encoding.CodePage == 65001)
			{
				switch (settings.OutputMethod)
				{
				case XmlOutputMethod.Xml:
					if (settings.Indent)
					{
						xmlWriter = new XmlUtf8RawTextWriterIndent(output, encoding, settings, closeOutput);
					}
					else
					{
						xmlWriter = new XmlUtf8RawTextWriter(output, encoding, settings, closeOutput);
					}
					break;
				case XmlOutputMethod.Html:
					if (settings.Indent)
					{
						xmlWriter = new HtmlUtf8RawTextWriterIndent(output, encoding, settings, closeOutput);
					}
					else
					{
						xmlWriter = new HtmlUtf8RawTextWriter(output, encoding, settings, closeOutput);
					}
					break;
				case XmlOutputMethod.Text:
					xmlWriter = new TextUtf8RawTextWriter(output, encoding, settings, closeOutput);
					break;
				case XmlOutputMethod.AutoDetect:
					xmlWriter = new XmlAutoDetectWriter(output, encoding, settings);
					break;
				default:
					return null;
				}
			}
			else
			{
				switch (settings.OutputMethod)
				{
				case XmlOutputMethod.Xml:
					if (settings.Indent)
					{
						xmlWriter = new XmlEncodedRawTextWriterIndent(output, encoding, settings, closeOutput);
					}
					else
					{
						xmlWriter = new XmlEncodedRawTextWriter(output, encoding, settings, closeOutput);
					}
					break;
				case XmlOutputMethod.Html:
					if (settings.Indent)
					{
						xmlWriter = new HtmlEncodedRawTextWriterIndent(output, encoding, settings, closeOutput);
					}
					else
					{
						xmlWriter = new HtmlEncodedRawTextWriter(output, encoding, settings, closeOutput);
					}
					break;
				case XmlOutputMethod.Text:
					xmlWriter = new TextEncodedRawTextWriter(output, encoding, settings, closeOutput);
					break;
				case XmlOutputMethod.AutoDetect:
					xmlWriter = new XmlAutoDetectWriter(output, encoding, settings);
					break;
				default:
					return null;
				}
			}
			if (settings.OutputMethod != XmlOutputMethod.AutoDetect && settings.IsQuerySpecific)
			{
				xmlWriter = new QueryOutputWriter((XmlRawWriter)xmlWriter, settings);
			}
			return new XmlWellFormedWriter(xmlWriter, settings);
		}

		private static XmlWriter CreateWriterImpl(TextWriter output, XmlWriterSettings settings)
		{
			XmlWriter xmlWriter;
			switch (settings.OutputMethod)
			{
			case XmlOutputMethod.Xml:
				if (settings.Indent)
				{
					xmlWriter = new XmlEncodedRawTextWriterIndent(output, settings);
				}
				else
				{
					xmlWriter = new XmlEncodedRawTextWriter(output, settings);
				}
				break;
			case XmlOutputMethod.Html:
				if (settings.Indent)
				{
					xmlWriter = new HtmlEncodedRawTextWriterIndent(output, settings);
				}
				else
				{
					xmlWriter = new HtmlEncodedRawTextWriter(output, settings);
				}
				break;
			case XmlOutputMethod.Text:
				xmlWriter = new TextEncodedRawTextWriter(output, settings);
				break;
			case XmlOutputMethod.AutoDetect:
				xmlWriter = new XmlAutoDetectWriter(output, settings);
				break;
			default:
				return null;
			}
			if (settings.OutputMethod != XmlOutputMethod.AutoDetect && settings.IsQuerySpecific)
			{
				xmlWriter = new QueryOutputWriter((XmlRawWriter)xmlWriter, settings);
			}
			return new XmlWellFormedWriter(xmlWriter, settings);
		}

		private static XmlWriter AddConformanceWrapper(XmlWriter baseWriter, XmlWriterSettings baseWriterSettings, XmlWriterSettings settings)
		{
			ConformanceLevel conformanceLevel = ConformanceLevel.Auto;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			if (baseWriterSettings == null)
			{
				if (settings.NewLineHandling == NewLineHandling.Replace)
				{
					flag3 = true;
					flag4 = true;
				}
				if (settings.CheckCharacters)
				{
					flag = true;
					flag4 = true;
				}
			}
			else
			{
				if (settings.ConformanceLevel != baseWriterSettings.ConformanceLevel)
				{
					conformanceLevel = settings.ConformanceLevel;
					flag4 = true;
				}
				if (settings.CheckCharacters && !baseWriterSettings.CheckCharacters)
				{
					flag = true;
					flag2 = conformanceLevel == ConformanceLevel.Auto;
					flag4 = true;
				}
				if (settings.NewLineHandling == NewLineHandling.Replace && baseWriterSettings.NewLineHandling == NewLineHandling.None)
				{
					flag3 = true;
					flag4 = true;
				}
			}
			if (flag4)
			{
				XmlWriter xmlWriter = baseWriter;
				if (conformanceLevel != ConformanceLevel.Auto)
				{
					xmlWriter = new XmlWellFormedWriter(xmlWriter, settings);
				}
				if (flag || flag3)
				{
					xmlWriter = new XmlCharCheckingWriter(xmlWriter, flag, flag2, flag3, settings.NewLineChars);
				}
				return xmlWriter;
			}
			return baseWriter;
		}

		private const int WriteNodeBufferSize = 1024;

		private char[] writeNodeBuffer;
	}
}
