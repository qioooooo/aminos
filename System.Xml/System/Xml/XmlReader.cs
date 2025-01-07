using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace System.Xml
{
	[DebuggerDisplay("{debuggerDisplayProxy}")]
	public abstract class XmlReader : IDisposable
	{
		public virtual XmlReaderSettings Settings
		{
			get
			{
				return null;
			}
		}

		public abstract XmlNodeType NodeType { get; }

		public virtual string Name
		{
			get
			{
				if (this.Prefix.Length == 0)
				{
					return this.LocalName;
				}
				return this.NameTable.Add(this.Prefix + ":" + this.LocalName);
			}
		}

		public abstract string LocalName { get; }

		public abstract string NamespaceURI { get; }

		public abstract string Prefix { get; }

		public abstract bool HasValue { get; }

		public abstract string Value { get; }

		public abstract int Depth { get; }

		public abstract string BaseURI { get; }

		public abstract bool IsEmptyElement { get; }

		public virtual bool IsDefault
		{
			get
			{
				return false;
			}
		}

		public virtual char QuoteChar
		{
			get
			{
				return '"';
			}
		}

		public virtual XmlSpace XmlSpace
		{
			get
			{
				return XmlSpace.None;
			}
		}

		public virtual string XmlLang
		{
			get
			{
				return string.Empty;
			}
		}

		public virtual IXmlSchemaInfo SchemaInfo
		{
			get
			{
				return this as IXmlSchemaInfo;
			}
		}

		public virtual Type ValueType
		{
			get
			{
				return typeof(string);
			}
		}

		public virtual object ReadContentAsObject()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsObject");
			}
			return this.InternalReadContentAsString();
		}

		public virtual bool ReadContentAsBoolean()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsBoolean");
			}
			bool flag;
			try
			{
				flag = XmlConvert.ToBoolean(this.InternalReadContentAsString());
			}
			catch (FormatException ex)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", "Boolean", ex, this as IXmlLineInfo);
			}
			return flag;
		}

		public virtual DateTime ReadContentAsDateTime()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsDateTime");
			}
			DateTime dateTime;
			try
			{
				dateTime = XmlConvert.ToDateTime(this.InternalReadContentAsString(), XmlDateTimeSerializationMode.RoundtripKind);
			}
			catch (FormatException ex)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", "DateTime", ex, this as IXmlLineInfo);
			}
			return dateTime;
		}

		public virtual double ReadContentAsDouble()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsDouble");
			}
			double num;
			try
			{
				num = XmlConvert.ToDouble(this.InternalReadContentAsString());
			}
			catch (FormatException ex)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", "Double", ex, this as IXmlLineInfo);
			}
			return num;
		}

		public virtual float ReadContentAsFloat()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsFloat");
			}
			float num;
			try
			{
				num = XmlConvert.ToSingle(this.InternalReadContentAsString());
			}
			catch (FormatException ex)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", "Float", ex, this as IXmlLineInfo);
			}
			return num;
		}

		public virtual decimal ReadContentAsDecimal()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsDecimal");
			}
			decimal num;
			try
			{
				num = XmlConvert.ToDecimal(this.InternalReadContentAsString());
			}
			catch (FormatException ex)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", "Decimal", ex, this as IXmlLineInfo);
			}
			return num;
		}

		public virtual int ReadContentAsInt()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsInt");
			}
			int num;
			try
			{
				num = XmlConvert.ToInt32(this.InternalReadContentAsString());
			}
			catch (FormatException ex)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", "Int", ex, this as IXmlLineInfo);
			}
			return num;
		}

		public virtual long ReadContentAsLong()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsLong");
			}
			long num;
			try
			{
				num = XmlConvert.ToInt64(this.InternalReadContentAsString());
			}
			catch (FormatException ex)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", "Long", ex, this as IXmlLineInfo);
			}
			return num;
		}

		public virtual string ReadContentAsString()
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAsString");
			}
			return this.InternalReadContentAsString();
		}

		public virtual object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
		{
			if (!this.CanReadContentAs())
			{
				throw this.CreateReadContentAsException("ReadContentAs");
			}
			string text = this.InternalReadContentAsString();
			if (returnType == typeof(string))
			{
				return text;
			}
			object obj;
			try
			{
				obj = XmlUntypedConverter.Untyped.ChangeType(text, returnType, this as IXmlNamespaceResolver);
			}
			catch (FormatException ex)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", returnType.ToString(), ex, this as IXmlLineInfo);
			}
			catch (InvalidCastException ex2)
			{
				throw new XmlException("Xml_ReadContentAsFormatException", returnType.ToString(), ex2, this as IXmlLineInfo);
			}
			return obj;
		}

		public virtual object ReadElementContentAsObject()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsObject"))
			{
				object obj = this.ReadContentAsObject();
				this.FinishReadElementContentAsXxx();
				return obj;
			}
			return string.Empty;
		}

		public virtual object ReadElementContentAsObject(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsObject();
		}

		public virtual bool ReadElementContentAsBoolean()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsBoolean"))
			{
				bool flag = this.ReadContentAsBoolean();
				this.FinishReadElementContentAsXxx();
				return flag;
			}
			return XmlConvert.ToBoolean(string.Empty);
		}

		public virtual bool ReadElementContentAsBoolean(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsBoolean();
		}

		public virtual DateTime ReadElementContentAsDateTime()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsDateTime"))
			{
				DateTime dateTime = this.ReadContentAsDateTime();
				this.FinishReadElementContentAsXxx();
				return dateTime;
			}
			return XmlConvert.ToDateTime(string.Empty, XmlDateTimeSerializationMode.RoundtripKind);
		}

		public virtual DateTime ReadElementContentAsDateTime(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsDateTime();
		}

		public virtual double ReadElementContentAsDouble()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsDouble"))
			{
				double num = this.ReadContentAsDouble();
				this.FinishReadElementContentAsXxx();
				return num;
			}
			return XmlConvert.ToDouble(string.Empty);
		}

		public virtual double ReadElementContentAsDouble(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsDouble();
		}

		public virtual float ReadElementContentAsFloat()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsFloat"))
			{
				float num = this.ReadContentAsFloat();
				this.FinishReadElementContentAsXxx();
				return num;
			}
			return XmlConvert.ToSingle(string.Empty);
		}

		public virtual float ReadElementContentAsFloat(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsFloat();
		}

		public virtual decimal ReadElementContentAsDecimal()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsDecimal"))
			{
				decimal num = this.ReadContentAsDecimal();
				this.FinishReadElementContentAsXxx();
				return num;
			}
			return XmlConvert.ToDecimal(string.Empty);
		}

		public virtual decimal ReadElementContentAsDecimal(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsDecimal();
		}

		public virtual int ReadElementContentAsInt()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsInt"))
			{
				int num = this.ReadContentAsInt();
				this.FinishReadElementContentAsXxx();
				return num;
			}
			return XmlConvert.ToInt32(string.Empty);
		}

		public virtual int ReadElementContentAsInt(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsInt();
		}

		public virtual long ReadElementContentAsLong()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsLong"))
			{
				long num = this.ReadContentAsLong();
				this.FinishReadElementContentAsXxx();
				return num;
			}
			return XmlConvert.ToInt64(string.Empty);
		}

		public virtual long ReadElementContentAsLong(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsLong();
		}

		public virtual string ReadElementContentAsString()
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAsString"))
			{
				string text = this.ReadContentAsString();
				this.FinishReadElementContentAsXxx();
				return text;
			}
			return string.Empty;
		}

		public virtual string ReadElementContentAsString(string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAsString();
		}

		public virtual object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
		{
			if (this.SetupReadElementContentAsXxx("ReadElementContentAs"))
			{
				object obj = this.ReadContentAs(returnType, namespaceResolver);
				this.FinishReadElementContentAsXxx();
				return obj;
			}
			if (returnType != typeof(string))
			{
				return XmlUntypedConverter.Untyped.ChangeType(string.Empty, returnType, namespaceResolver);
			}
			return string.Empty;
		}

		public virtual object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver, string localName, string namespaceURI)
		{
			this.CheckElement(localName, namespaceURI);
			return this.ReadElementContentAs(returnType, namespaceResolver);
		}

		public abstract int AttributeCount { get; }

		public abstract string GetAttribute(string name);

		public abstract string GetAttribute(string name, string namespaceURI);

		public abstract string GetAttribute(int i);

		public virtual string this[int i]
		{
			get
			{
				return this.GetAttribute(i);
			}
		}

		public virtual string this[string name]
		{
			get
			{
				return this.GetAttribute(name);
			}
		}

		public virtual string this[string name, string namespaceURI]
		{
			get
			{
				return this.GetAttribute(name, namespaceURI);
			}
		}

		public abstract bool MoveToAttribute(string name);

		public abstract bool MoveToAttribute(string name, string ns);

		public virtual void MoveToAttribute(int i)
		{
			if (i < 0 || i >= this.AttributeCount)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			this.MoveToElement();
			this.MoveToFirstAttribute();
			for (int j = 0; j < i; j++)
			{
				this.MoveToNextAttribute();
			}
		}

		public abstract bool MoveToFirstAttribute();

		public abstract bool MoveToNextAttribute();

		public abstract bool MoveToElement();

		public abstract bool ReadAttributeValue();

		public abstract bool Read();

		public abstract bool EOF { get; }

		public abstract void Close();

		public abstract ReadState ReadState { get; }

		public virtual void Skip()
		{
			this.SkipSubtree();
		}

		public abstract XmlNameTable NameTable { get; }

		public abstract string LookupNamespace(string prefix);

		public virtual bool CanResolveEntity
		{
			get
			{
				return false;
			}
		}

		public abstract void ResolveEntity();

		public virtual bool CanReadBinaryContent
		{
			get
			{
				return false;
			}
		}

		public virtual int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			throw new NotSupportedException(Res.GetString("Xml_ReadBinaryContentNotSupported", new object[] { "ReadContentAsBase64" }));
		}

		public virtual int ReadElementContentAsBase64(byte[] buffer, int index, int count)
		{
			throw new NotSupportedException(Res.GetString("Xml_ReadBinaryContentNotSupported", new object[] { "ReadElementContentAsBase64" }));
		}

		public virtual int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			throw new NotSupportedException(Res.GetString("Xml_ReadBinaryContentNotSupported", new object[] { "ReadContentAsBinHex" }));
		}

		public virtual int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
		{
			throw new NotSupportedException(Res.GetString("Xml_ReadBinaryContentNotSupported", new object[] { "ReadElementContentAsBinHex" }));
		}

		public virtual bool CanReadValueChunk
		{
			get
			{
				return false;
			}
		}

		public virtual int ReadValueChunk(char[] buffer, int index, int count)
		{
			throw new NotSupportedException(Res.GetString("Xml_ReadValueChunkNotSupported"));
		}

		public virtual string ReadString()
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return string.Empty;
			}
			this.MoveToElement();
			if (this.NodeType == XmlNodeType.Element)
			{
				if (this.IsEmptyElement)
				{
					return string.Empty;
				}
				if (!this.Read())
				{
					throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
				}
				if (this.NodeType == XmlNodeType.EndElement)
				{
					return string.Empty;
				}
			}
			string text = string.Empty;
			while (XmlReader.IsTextualNode(this.NodeType))
			{
				text += this.Value;
				if (!this.Read())
				{
					break;
				}
			}
			return text;
		}

		public virtual XmlNodeType MoveToContent()
		{
			for (;;)
			{
				XmlNodeType nodeType = this.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
				case XmlNodeType.EntityReference:
					goto IL_003D;
				case XmlNodeType.Attribute:
					goto IL_0036;
				default:
					switch (nodeType)
					{
					case XmlNodeType.EndElement:
					case XmlNodeType.EndEntity:
						goto IL_003D;
					default:
						if (!this.Read())
						{
							goto Block_2;
						}
						break;
					}
					break;
				}
			}
			IL_0036:
			this.MoveToElement();
			IL_003D:
			return this.NodeType;
			Block_2:
			return this.NodeType;
		}

		public virtual void ReadStartElement()
		{
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			this.Read();
		}

		public virtual void ReadStartElement(string name)
		{
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			if (this.Name == name)
			{
				this.Read();
				return;
			}
			throw new XmlException("Xml_ElementNotFound", name, this as IXmlLineInfo);
		}

		public virtual void ReadStartElement(string localname, string ns)
		{
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			if (this.LocalName == localname && this.NamespaceURI == ns)
			{
				this.Read();
				return;
			}
			throw new XmlException("Xml_ElementNotFoundNs", new string[] { localname, ns }, this as IXmlLineInfo);
		}

		public virtual string ReadElementString()
		{
			string text = string.Empty;
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			if (!this.IsEmptyElement)
			{
				this.Read();
				text = this.ReadString();
				if (this.NodeType != XmlNodeType.EndElement)
				{
					throw new XmlException("Xml_UnexpectedNodeInSimpleContent", new string[]
					{
						this.NodeType.ToString(),
						"ReadElementString"
					}, this as IXmlLineInfo);
				}
				this.Read();
			}
			else
			{
				this.Read();
			}
			return text;
		}

		public virtual string ReadElementString(string name)
		{
			string text = string.Empty;
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			if (this.Name != name)
			{
				throw new XmlException("Xml_ElementNotFound", name, this as IXmlLineInfo);
			}
			if (!this.IsEmptyElement)
			{
				text = this.ReadString();
				if (this.NodeType != XmlNodeType.EndElement)
				{
					throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
				}
				this.Read();
			}
			else
			{
				this.Read();
			}
			return text;
		}

		public virtual string ReadElementString(string localname, string ns)
		{
			string text = string.Empty;
			if (this.MoveToContent() != XmlNodeType.Element)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			if (this.LocalName != localname || this.NamespaceURI != ns)
			{
				throw new XmlException("Xml_ElementNotFoundNs", new string[] { localname, ns }, this as IXmlLineInfo);
			}
			if (!this.IsEmptyElement)
			{
				text = this.ReadString();
				if (this.NodeType != XmlNodeType.EndElement)
				{
					throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
				}
				this.Read();
			}
			else
			{
				this.Read();
			}
			return text;
		}

		public virtual void ReadEndElement()
		{
			if (this.MoveToContent() != XmlNodeType.EndElement)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			this.Read();
		}

		public virtual bool IsStartElement()
		{
			return this.MoveToContent() == XmlNodeType.Element;
		}

		public virtual bool IsStartElement(string name)
		{
			return this.MoveToContent() == XmlNodeType.Element && this.Name == name;
		}

		public virtual bool IsStartElement(string localname, string ns)
		{
			return this.MoveToContent() == XmlNodeType.Element && this.LocalName == localname && this.NamespaceURI == ns;
		}

		public virtual bool ReadToFollowing(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(name, "name");
			}
			name = this.NameTable.Add(name);
			while (this.Read())
			{
				if (this.NodeType == XmlNodeType.Element && Ref.Equal(name, this.Name))
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool ReadToFollowing(string localName, string namespaceURI)
		{
			if (localName == null || localName.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(localName, "localName");
			}
			if (namespaceURI == null)
			{
				throw new ArgumentNullException("namespaceURI");
			}
			localName = this.NameTable.Add(localName);
			namespaceURI = this.NameTable.Add(namespaceURI);
			while (this.Read())
			{
				if (this.NodeType == XmlNodeType.Element && Ref.Equal(localName, this.LocalName) && Ref.Equal(namespaceURI, this.NamespaceURI))
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool ReadToDescendant(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(name, "name");
			}
			int num = this.Depth;
			if (this.NodeType != XmlNodeType.Element)
			{
				if (this.ReadState != ReadState.Initial)
				{
					return false;
				}
				num--;
			}
			else if (this.IsEmptyElement)
			{
				return false;
			}
			name = this.NameTable.Add(name);
			while (this.Read() && this.Depth > num)
			{
				if (this.NodeType == XmlNodeType.Element && Ref.Equal(name, this.Name))
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool ReadToDescendant(string localName, string namespaceURI)
		{
			if (localName == null || localName.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(localName, "localName");
			}
			if (namespaceURI == null)
			{
				throw new ArgumentNullException("namespaceURI");
			}
			int num = this.Depth;
			if (this.NodeType != XmlNodeType.Element)
			{
				if (this.ReadState != ReadState.Initial)
				{
					return false;
				}
				num--;
			}
			else if (this.IsEmptyElement)
			{
				return false;
			}
			localName = this.NameTable.Add(localName);
			namespaceURI = this.NameTable.Add(namespaceURI);
			while (this.Read() && this.Depth > num)
			{
				if (this.NodeType == XmlNodeType.Element && Ref.Equal(localName, this.LocalName) && Ref.Equal(namespaceURI, this.NamespaceURI))
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool ReadToNextSibling(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(name, "name");
			}
			name = this.NameTable.Add(name);
			for (;;)
			{
				this.SkipSubtree();
				XmlNodeType nodeType = this.NodeType;
				if (nodeType == XmlNodeType.Element && Ref.Equal(name, this.Name))
				{
					break;
				}
				if (nodeType == XmlNodeType.EndElement || this.EOF)
				{
					return false;
				}
			}
			return true;
		}

		public virtual bool ReadToNextSibling(string localName, string namespaceURI)
		{
			if (localName == null || localName.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(localName, "localName");
			}
			if (namespaceURI == null)
			{
				throw new ArgumentNullException("namespaceURI");
			}
			localName = this.NameTable.Add(localName);
			namespaceURI = this.NameTable.Add(namespaceURI);
			for (;;)
			{
				this.SkipSubtree();
				XmlNodeType nodeType = this.NodeType;
				if (nodeType == XmlNodeType.Element && Ref.Equal(localName, this.LocalName) && Ref.Equal(namespaceURI, this.NamespaceURI))
				{
					break;
				}
				if (nodeType == XmlNodeType.EndElement || this.EOF)
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsName(string str)
		{
			return XmlCharType.Instance.IsName(str);
		}

		public static bool IsNameToken(string str)
		{
			return XmlCharType.Instance.IsNmToken(str);
		}

		public virtual string ReadInnerXml()
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return string.Empty;
			}
			if (this.NodeType != XmlNodeType.Attribute && this.NodeType != XmlNodeType.Element)
			{
				this.Read();
				return string.Empty;
			}
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			try
			{
				this.SetNamespacesFlag(xmlTextWriter);
				if (this.NodeType == XmlNodeType.Attribute)
				{
					xmlTextWriter.QuoteChar = this.QuoteChar;
					this.WriteAttributeValue(xmlTextWriter);
				}
				if (this.NodeType == XmlNodeType.Element)
				{
					this.WriteNode(xmlTextWriter, false);
				}
			}
			finally
			{
				xmlTextWriter.Close();
			}
			return stringWriter.ToString();
		}

		private void WriteNode(XmlTextWriter xtw, bool defattr)
		{
			int num = ((this.NodeType == XmlNodeType.None) ? (-1) : this.Depth);
			while (this.Read() && num < this.Depth)
			{
				switch (this.NodeType)
				{
				case XmlNodeType.Element:
					xtw.WriteStartElement(this.Prefix, this.LocalName, this.NamespaceURI);
					xtw.QuoteChar = this.QuoteChar;
					xtw.WriteAttributes(this, defattr);
					if (this.IsEmptyElement)
					{
						xtw.WriteEndElement();
					}
					break;
				case XmlNodeType.Text:
					xtw.WriteString(this.Value);
					break;
				case XmlNodeType.CDATA:
					xtw.WriteCData(this.Value);
					break;
				case XmlNodeType.EntityReference:
					xtw.WriteEntityRef(this.Name);
					break;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.XmlDeclaration:
					xtw.WriteProcessingInstruction(this.Name, this.Value);
					break;
				case XmlNodeType.Comment:
					xtw.WriteComment(this.Value);
					break;
				case XmlNodeType.DocumentType:
					xtw.WriteDocType(this.Name, this.GetAttribute("PUBLIC"), this.GetAttribute("SYSTEM"), this.Value);
					break;
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					xtw.WriteWhitespace(this.Value);
					break;
				case XmlNodeType.EndElement:
					xtw.WriteFullEndElement();
					break;
				}
			}
			if (num == this.Depth && this.NodeType == XmlNodeType.EndElement)
			{
				this.Read();
			}
		}

		private void WriteAttributeValue(XmlTextWriter xtw)
		{
			string name = this.Name;
			while (this.ReadAttributeValue())
			{
				if (this.NodeType == XmlNodeType.EntityReference)
				{
					xtw.WriteEntityRef(this.Name);
				}
				else
				{
					xtw.WriteString(this.Value);
				}
			}
			this.MoveToAttribute(name);
		}

		public virtual string ReadOuterXml()
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return string.Empty;
			}
			if (this.NodeType != XmlNodeType.Attribute && this.NodeType != XmlNodeType.Element)
			{
				this.Read();
				return string.Empty;
			}
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			try
			{
				this.SetNamespacesFlag(xmlTextWriter);
				if (this.NodeType == XmlNodeType.Attribute)
				{
					xmlTextWriter.WriteStartAttribute(this.Prefix, this.LocalName, this.NamespaceURI);
					this.WriteAttributeValue(xmlTextWriter);
					xmlTextWriter.WriteEndAttribute();
				}
				else
				{
					xmlTextWriter.WriteNode(this, false);
				}
			}
			finally
			{
				xmlTextWriter.Close();
			}
			return stringWriter.ToString();
		}

		private void SetNamespacesFlag(XmlTextWriter xtw)
		{
			XmlTextReader xmlTextReader = this as XmlTextReader;
			if (xmlTextReader != null)
			{
				xtw.Namespaces = xmlTextReader.Namespaces;
				return;
			}
			XmlValidatingReader xmlValidatingReader = this as XmlValidatingReader;
			if (xmlValidatingReader != null)
			{
				xtw.Namespaces = xmlValidatingReader.Namespaces;
			}
		}

		public virtual XmlReader ReadSubtree()
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw new InvalidOperationException(Res.GetString("Xml_ReadSubtreeNotOnElement"));
			}
			return new XmlSubtreeReader(this);
		}

		public virtual bool HasAttributes
		{
			get
			{
				return this.AttributeCount > 0;
			}
		}

		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (this.ReadState != ReadState.Closed)
			{
				this.Close();
			}
		}

		internal virtual XmlNamespaceManager NamespaceManager
		{
			get
			{
				return null;
			}
		}

		internal static bool IsTextualNode(XmlNodeType nodeType)
		{
			return 0UL != ((ulong)XmlReader.IsTextualNodeBitmap & (ulong)(1L << (int)(nodeType & (XmlNodeType)31)));
		}

		internal static bool CanReadContentAs(XmlNodeType nodeType)
		{
			return 0UL != ((ulong)XmlReader.CanReadContentAsBitmap & (ulong)(1L << (int)(nodeType & (XmlNodeType)31)));
		}

		internal static bool HasValueInternal(XmlNodeType nodeType)
		{
			return 0UL != ((ulong)XmlReader.HasValueBitmap & (ulong)(1L << (int)(nodeType & (XmlNodeType)31)));
		}

		private void SkipSubtree()
		{
			if (this.ReadState != ReadState.Interactive)
			{
				return;
			}
			this.MoveToElement();
			if (this.NodeType == XmlNodeType.Element && !this.IsEmptyElement)
			{
				int depth = this.Depth;
				while (this.Read() && depth < this.Depth)
				{
				}
				if (this.NodeType == XmlNodeType.EndElement)
				{
					this.Read();
					return;
				}
			}
			else
			{
				this.Read();
			}
		}

		internal void CheckElement(string localName, string namespaceURI)
		{
			if (localName == null || localName.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(localName, "localName");
			}
			if (namespaceURI == null)
			{
				throw new ArgumentNullException("namespaceURI");
			}
			if (this.NodeType != XmlNodeType.Element)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString(), this as IXmlLineInfo);
			}
			if (this.LocalName != localName || this.NamespaceURI != namespaceURI)
			{
				throw new XmlException("Xml_ElementNotFoundNs", new string[] { localName, namespaceURI }, this as IXmlLineInfo);
			}
		}

		internal Exception CreateReadContentAsException(string methodName)
		{
			return XmlReader.CreateReadContentAsException(methodName, this.NodeType, this as IXmlLineInfo);
		}

		internal Exception CreateReadElementContentAsException(string methodName)
		{
			return XmlReader.CreateReadElementContentAsException(methodName, this.NodeType, this as IXmlLineInfo);
		}

		internal bool CanReadContentAs()
		{
			return XmlReader.CanReadContentAs(this.NodeType);
		}

		internal static Exception CreateReadContentAsException(string methodName, XmlNodeType nodeType, IXmlLineInfo lineInfo)
		{
			return new InvalidOperationException(XmlReader.AddLineInfo(Res.GetString("Xml_InvalidReadContentAs", new string[]
			{
				methodName,
				nodeType.ToString()
			}), lineInfo));
		}

		internal static Exception CreateReadElementContentAsException(string methodName, XmlNodeType nodeType, IXmlLineInfo lineInfo)
		{
			return new InvalidOperationException(XmlReader.AddLineInfo(Res.GetString("Xml_InvalidReadElementContentAs", new string[]
			{
				methodName,
				nodeType.ToString()
			}), lineInfo));
		}

		private static string AddLineInfo(string message, IXmlLineInfo lineInfo)
		{
			if (lineInfo != null)
			{
				message = message + " " + Res.GetString("Xml_ErrorPosition", new string[]
				{
					lineInfo.LineNumber.ToString(CultureInfo.InvariantCulture),
					lineInfo.LinePosition.ToString(CultureInfo.InvariantCulture)
				});
			}
			return message;
		}

		internal string InternalReadContentAsString()
		{
			string text = string.Empty;
			BufferBuilder bufferBuilder = null;
			do
			{
				switch (this.NodeType)
				{
				case XmlNodeType.Attribute:
					goto IL_0055;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					if (text.Length == 0)
					{
						text = this.Value;
						goto IL_0099;
					}
					if (bufferBuilder == null)
					{
						bufferBuilder = new BufferBuilder();
						bufferBuilder.Append(text);
					}
					bufferBuilder.Append(this.Value);
					goto IL_0099;
				case XmlNodeType.EntityReference:
					if (this.CanResolveEntity)
					{
						this.ResolveEntity();
						goto IL_0099;
					}
					break;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Comment:
				case XmlNodeType.EndEntity:
					goto IL_0099;
				}
				break;
				IL_0099:;
			}
			while ((this.AttributeCount != 0) ? this.ReadAttributeValue() : this.Read());
			goto IL_00B4;
			IL_0055:
			return this.Value;
			IL_00B4:
			if (bufferBuilder != null)
			{
				return bufferBuilder.ToString();
			}
			return text;
		}

		private bool SetupReadElementContentAsXxx(string methodName)
		{
			if (this.NodeType != XmlNodeType.Element)
			{
				throw this.CreateReadElementContentAsException(methodName);
			}
			bool isEmptyElement = this.IsEmptyElement;
			this.Read();
			if (isEmptyElement)
			{
				return false;
			}
			XmlNodeType nodeType = this.NodeType;
			if (nodeType == XmlNodeType.EndElement)
			{
				this.Read();
				return false;
			}
			if (nodeType == XmlNodeType.Element)
			{
				throw new XmlException("Xml_MixedReadElementContentAs", string.Empty, this as IXmlLineInfo);
			}
			return true;
		}

		private void FinishReadElementContentAsXxx()
		{
			if (this.NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("Xml_InvalidNodeType", this.NodeType.ToString());
			}
			this.Read();
		}

		internal static SchemaInfo GetDtdSchemaInfo(XmlReader reader)
		{
			XmlWrappingReader xmlWrappingReader = reader as XmlWrappingReader;
			if (xmlWrappingReader != null)
			{
				return xmlWrappingReader.DtdSchemaInfo;
			}
			XmlTextReaderImpl xmlTextReaderImpl = XmlReader.GetXmlTextReaderImpl(reader);
			if (xmlTextReaderImpl == null)
			{
				return null;
			}
			return xmlTextReaderImpl.DtdSchemaInfo;
		}

		internal static Encoding GetEncoding(XmlReader reader)
		{
			XmlTextReaderImpl xmlTextReaderImpl = XmlReader.GetXmlTextReaderImpl(reader);
			if (xmlTextReaderImpl == null)
			{
				return null;
			}
			return xmlTextReaderImpl.Encoding;
		}

		internal static ConformanceLevel GetV1ConformanceLevel(XmlReader reader)
		{
			XmlTextReaderImpl xmlTextReaderImpl = XmlReader.GetXmlTextReaderImpl(reader);
			if (xmlTextReaderImpl == null)
			{
				return ConformanceLevel.Document;
			}
			return xmlTextReaderImpl.V1ComformanceLevel;
		}

		private static XmlTextReaderImpl GetXmlTextReaderImpl(XmlReader reader)
		{
			XmlTextReaderImpl xmlTextReaderImpl = reader as XmlTextReaderImpl;
			if (xmlTextReaderImpl != null)
			{
				return xmlTextReaderImpl;
			}
			XmlTextReader xmlTextReader = reader as XmlTextReader;
			if (xmlTextReader != null)
			{
				return xmlTextReader.Impl;
			}
			XmlValidatingReaderImpl xmlValidatingReaderImpl = reader as XmlValidatingReaderImpl;
			if (xmlValidatingReaderImpl != null)
			{
				return xmlValidatingReaderImpl.ReaderImpl;
			}
			XmlValidatingReader xmlValidatingReader = reader as XmlValidatingReader;
			if (xmlValidatingReader != null)
			{
				return xmlValidatingReader.Impl.ReaderImpl;
			}
			return null;
		}

		public static XmlReader Create(string inputUri)
		{
			return XmlReader.Create(inputUri, null, null);
		}

		public static XmlReader Create(string inputUri, XmlReaderSettings settings)
		{
			return XmlReader.Create(inputUri, settings, null);
		}

		public static XmlReader Create(string inputUri, XmlReaderSettings settings, XmlParserContext inputContext)
		{
			if (inputUri == null)
			{
				throw new ArgumentNullException("inputUri");
			}
			if (inputUri.Length == 0)
			{
				throw new ArgumentException(Res.GetString("XmlConvert_BadUri"), "inputUri");
			}
			if (settings == null)
			{
				settings = new XmlReaderSettings();
			}
			XmlResolver xmlResolver = settings.GetXmlResolver();
			if (xmlResolver == null)
			{
				xmlResolver = new XmlUrlResolver();
			}
			Uri uri = xmlResolver.ResolveUri(null, inputUri);
			Stream stream = (Stream)xmlResolver.GetEntity(uri, string.Empty, typeof(Stream));
			if (stream == null)
			{
				throw new XmlException("Xml_CannotResolveUrl", inputUri);
			}
			XmlReader xmlReader;
			try
			{
				xmlReader = XmlReader.CreateReaderImpl(stream, settings, uri, uri.ToString(), inputContext, true);
			}
			catch
			{
				stream.Close();
				throw;
			}
			return xmlReader;
		}

		public static XmlReader Create(Stream input)
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			return XmlReader.CreateReaderImpl(input, xmlReaderSettings, null, string.Empty, null, xmlReaderSettings.CloseInput);
		}

		public static XmlReader Create(Stream input, XmlReaderSettings settings)
		{
			return XmlReader.Create(input, settings, string.Empty);
		}

		public static XmlReader Create(Stream input, XmlReaderSettings settings, string baseUri)
		{
			if (settings == null)
			{
				settings = new XmlReaderSettings();
			}
			return XmlReader.CreateReaderImpl(input, settings, null, baseUri, null, settings.CloseInput);
		}

		public static XmlReader Create(Stream input, XmlReaderSettings settings, XmlParserContext inputContext)
		{
			if (settings == null)
			{
				settings = new XmlReaderSettings();
			}
			return XmlReader.CreateReaderImpl(input, settings, null, string.Empty, inputContext, settings.CloseInput);
		}

		public static XmlReader Create(TextReader input)
		{
			return XmlReader.CreateReaderImpl(input, null, string.Empty, null);
		}

		public static XmlReader Create(TextReader input, XmlReaderSettings settings)
		{
			return XmlReader.Create(input, settings, string.Empty);
		}

		public static XmlReader Create(TextReader input, XmlReaderSettings settings, string baseUri)
		{
			return XmlReader.CreateReaderImpl(input, settings, baseUri, null);
		}

		public static XmlReader Create(TextReader input, XmlReaderSettings settings, XmlParserContext inputContext)
		{
			return XmlReader.CreateReaderImpl(input, settings, string.Empty, inputContext);
		}

		public static XmlReader Create(XmlReader reader, XmlReaderSettings settings)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (settings == null)
			{
				settings = new XmlReaderSettings();
			}
			return XmlReader.CreateReaderImpl(reader, settings);
		}

		internal static XmlReader CreateSqlReader(Stream input, XmlReaderSettings settings, XmlParserContext inputContext)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (settings == null)
			{
				settings = new XmlReaderSettings();
			}
			byte[] array = new byte[XmlReader.CalcBufferSize(input)];
			int num = 0;
			int num2;
			do
			{
				num2 = input.Read(array, num, array.Length - num);
				num += num2;
			}
			while (num2 > 0 && num < 2);
			XmlReader xmlReader;
			if (num >= 2 && array[0] == 223 && array[1] == 255)
			{
				if (inputContext != null)
				{
					throw new ArgumentException(Res.GetString("XmlBinary_NoParserContext"), "inputContext");
				}
				xmlReader = new XmlSqlBinaryReader(input, array, num, string.Empty, settings.CloseInput, settings);
			}
			else
			{
				xmlReader = new XmlTextReaderImpl(input, array, num, settings, null, string.Empty, inputContext, settings.CloseInput);
			}
			if (settings.ValidationType != ValidationType.None)
			{
				xmlReader = XmlReader.AddValidation(xmlReader, settings);
			}
			return xmlReader;
		}

		private static XmlReader CreateReaderImpl(Stream input, XmlReaderSettings settings, Uri baseUri, string baseUriStr, XmlParserContext inputContext, bool closeInput)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (baseUriStr == null)
			{
				baseUriStr = string.Empty;
			}
			XmlReader xmlReader = new XmlTextReaderImpl(input, null, 0, settings, baseUri, baseUriStr, inputContext, closeInput);
			if (settings.ValidationType != ValidationType.None)
			{
				xmlReader = XmlReader.AddValidation(xmlReader, settings);
			}
			return xmlReader;
		}

		private static XmlReader AddValidation(XmlReader reader, XmlReaderSettings settings)
		{
			if (settings.ValidationType == ValidationType.Schema)
			{
				reader = new XsdValidatingReader(reader, settings.GetXmlResolver_CheckConfig(), settings);
			}
			else if (settings.ValidationType == ValidationType.DTD)
			{
				reader = XmlReader.CreateDtdValidatingReader(reader, settings);
			}
			return reader;
		}

		internal static int CalcBufferSize(Stream input)
		{
			int num = 4096;
			if (input.CanSeek)
			{
				long length = input.Length;
				if (length < (long)num)
				{
					num = checked((int)length);
				}
				else if (length > 65536L)
				{
					num = 8192;
				}
			}
			return num;
		}

		private static XmlReader CreateReaderImpl(TextReader input, XmlReaderSettings settings, string baseUriStr, XmlParserContext context)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (settings == null)
			{
				settings = new XmlReaderSettings();
			}
			if (baseUriStr == null)
			{
				baseUriStr = string.Empty;
			}
			XmlReader xmlReader = new XmlTextReaderImpl(input, settings, baseUriStr, context);
			if (settings.ValidationType == ValidationType.Schema)
			{
				xmlReader = new XsdValidatingReader(xmlReader, settings.GetXmlResolver_CheckConfig(), settings);
			}
			else if (settings.ValidationType == ValidationType.DTD)
			{
				xmlReader = XmlReader.CreateDtdValidatingReader(xmlReader, settings);
			}
			return xmlReader;
		}

		private static XmlReader CreateReaderImpl(XmlReader baseReader, XmlReaderSettings settings)
		{
			XmlReader xmlReader = baseReader;
			if (settings.ValidationType == ValidationType.DTD)
			{
				xmlReader = XmlReader.CreateDtdValidatingReader(xmlReader, settings);
			}
			xmlReader = XmlReader.AddWrapper(xmlReader, settings, xmlReader.Settings);
			if (settings.ValidationType == ValidationType.Schema)
			{
				xmlReader = new XsdValidatingReader(xmlReader, settings.GetXmlResolver_CheckConfig(), settings);
			}
			return xmlReader;
		}

		private static XmlValidatingReaderImpl CreateDtdValidatingReader(XmlReader baseReader, XmlReaderSettings settings)
		{
			return new XmlValidatingReaderImpl(baseReader, settings.GetEventHandler(), (settings.ValidationFlags & XmlSchemaValidationFlags.ProcessIdentityConstraints) != XmlSchemaValidationFlags.None);
		}

		private static XmlReader AddWrapper(XmlReader baseReader, XmlReaderSettings settings, XmlReaderSettings baseReaderSettings)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			if (baseReaderSettings == null)
			{
				if (settings.ConformanceLevel != ConformanceLevel.Auto && settings.ConformanceLevel != XmlReader.GetV1ConformanceLevel(baseReader))
				{
					throw new InvalidOperationException(Res.GetString("Xml_IncompatibleConformanceLevel", new object[] { settings.ConformanceLevel.ToString() }));
				}
				if (settings.IgnoreWhitespace)
				{
					WhitespaceHandling whitespaceHandling = WhitespaceHandling.All;
					XmlTextReader xmlTextReader = baseReader as XmlTextReader;
					if (xmlTextReader != null)
					{
						whitespaceHandling = xmlTextReader.WhitespaceHandling;
					}
					else
					{
						XmlValidatingReader xmlValidatingReader = baseReader as XmlValidatingReader;
						if (xmlValidatingReader != null)
						{
							whitespaceHandling = ((XmlTextReader)xmlValidatingReader.Reader).WhitespaceHandling;
						}
					}
					if (whitespaceHandling == WhitespaceHandling.All)
					{
						flag2 = true;
						flag5 = true;
					}
				}
				if (settings.IgnoreComments)
				{
					flag3 = true;
					flag5 = true;
				}
				if (settings.IgnoreProcessingInstructions)
				{
					flag4 = true;
					flag5 = true;
				}
				if (settings.ProhibitDtd)
				{
					XmlTextReader xmlTextReader2 = baseReader as XmlTextReader;
					if (xmlTextReader2 == null)
					{
						XmlValidatingReader xmlValidatingReader2 = baseReader as XmlValidatingReader;
						if (xmlValidatingReader2 != null)
						{
							xmlTextReader2 = (XmlTextReader)xmlValidatingReader2.Reader;
						}
					}
					if (xmlTextReader2 == null || !xmlTextReader2.ProhibitDtd)
					{
						flag6 = true;
						flag5 = true;
					}
				}
			}
			else
			{
				if (settings.ConformanceLevel != baseReaderSettings.ConformanceLevel && settings.ConformanceLevel != ConformanceLevel.Auto)
				{
					throw new InvalidOperationException(Res.GetString("Xml_IncompatibleConformanceLevel", new object[] { settings.ConformanceLevel.ToString() }));
				}
				if (settings.CheckCharacters && !baseReaderSettings.CheckCharacters)
				{
					flag = true;
					flag5 = true;
				}
				if (settings.IgnoreWhitespace && !baseReaderSettings.IgnoreWhitespace)
				{
					flag2 = true;
					flag5 = true;
				}
				if (settings.IgnoreComments && !baseReaderSettings.IgnoreComments)
				{
					flag3 = true;
					flag5 = true;
				}
				if (settings.IgnoreProcessingInstructions && !baseReaderSettings.IgnoreProcessingInstructions)
				{
					flag4 = true;
					flag5 = true;
				}
				if (settings.ProhibitDtd && !baseReaderSettings.ProhibitDtd)
				{
					flag6 = true;
					flag5 = true;
				}
			}
			if (!flag5)
			{
				return baseReader;
			}
			IXmlNamespaceResolver xmlNamespaceResolver = baseReader as IXmlNamespaceResolver;
			if (xmlNamespaceResolver != null)
			{
				return new XmlCharCheckingReaderWithNS(baseReader, xmlNamespaceResolver, flag, flag2, flag3, flag4, flag6);
			}
			return new XmlCharCheckingReader(baseReader, flag, flag2, flag3, flag4, flag6);
		}

		private object debuggerDisplayProxy
		{
			get
			{
				return new XmlReader.DebuggerDisplayProxy(this);
			}
		}

		internal const int DefaultBufferSize = 4096;

		internal const int BiggerBufferSize = 8192;

		internal const int MaxStreamLengthForDefaultBufferSize = 65536;

		private static uint IsTextualNodeBitmap = 24600U;

		private static uint CanReadContentAsBitmap = 123324U;

		private static uint HasValueBitmap = 157084U;

		[DebuggerDisplay("{ToString()}")]
		private struct DebuggerDisplayProxy
		{
			internal DebuggerDisplayProxy(XmlReader reader)
			{
				this.reader = reader;
			}

			public override string ToString()
			{
				XmlNodeType nodeType = this.reader.NodeType;
				string text = nodeType.ToString();
				switch (nodeType)
				{
				case XmlNodeType.Element:
				case XmlNodeType.EntityReference:
				case XmlNodeType.EndElement:
				case XmlNodeType.EndEntity:
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						", Name=\"",
						this.reader.Name,
						'"'
					});
					break;
				}
				case XmlNodeType.Attribute:
				case XmlNodeType.ProcessingInstruction:
				{
					object obj2 = text;
					text = string.Concat(new object[]
					{
						obj2,
						", Name=\"",
						this.reader.Name,
						"\", Value=\"",
						XmlConvert.EscapeValueForDebuggerDisplay(this.reader.Value),
						'"'
					});
					break;
				}
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
				case XmlNodeType.Comment:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
				case XmlNodeType.XmlDeclaration:
				{
					object obj3 = text;
					text = string.Concat(new object[]
					{
						obj3,
						", Value=\"",
						XmlConvert.EscapeValueForDebuggerDisplay(this.reader.Value),
						'"'
					});
					break;
				}
				case XmlNodeType.DocumentType:
				{
					text = text + ", Name=\"" + this.reader.Name + "'";
					object obj4 = text;
					text = string.Concat(new object[]
					{
						obj4,
						", SYSTEM=\"",
						this.reader.GetAttribute("SYSTEM"),
						'"'
					});
					object obj5 = text;
					text = string.Concat(new object[]
					{
						obj5,
						", PUBLIC=\"",
						this.reader.GetAttribute("PUBLIC"),
						'"'
					});
					object obj6 = text;
					text = string.Concat(new object[]
					{
						obj6,
						", Value=\"",
						XmlConvert.EscapeValueForDebuggerDisplay(this.reader.Value),
						'"'
					});
					break;
				}
				}
				return text;
			}

			private XmlReader reader;
		}
	}
}
