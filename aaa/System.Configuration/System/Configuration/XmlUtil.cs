using System;
using System.Configuration.Internal;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020000B3 RID: 179
	internal sealed class XmlUtil : IDisposable, IConfigErrorInfo
	{
		// Token: 0x060006A8 RID: 1704 RVA: 0x0001DEA9 File Offset: 0x0001CEA9
		private static int GetPositionOffset(XmlNodeType nodeType)
		{
			return XmlUtil.s_positionOffset[(int)nodeType];
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0001DEB2 File Offset: 0x0001CEB2
		internal XmlUtil(Stream stream, string name, bool readToFirstElement)
			: this(stream, name, readToFirstElement, new ConfigurationSchemaErrors())
		{
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0001DEC4 File Offset: 0x0001CEC4
		internal XmlUtil(Stream stream, string name, bool readToFirstElement, ConfigurationSchemaErrors schemaErrors)
		{
			try
			{
				this._streamName = name;
				this._stream = stream;
				this._reader = new XmlTextReader(this._stream);
				this._reader.XmlResolver = null;
				this._schemaErrors = schemaErrors;
				this._lastLineNumber = 1;
				this._lastLinePosition = 1;
				if (readToFirstElement)
				{
					this._reader.WhitespaceHandling = WhitespaceHandling.None;
					bool flag = false;
					while (!flag && this._reader.Read())
					{
						XmlNodeType nodeType = this._reader.NodeType;
						if (nodeType != XmlNodeType.Element)
						{
							switch (nodeType)
							{
							case XmlNodeType.Comment:
							case XmlNodeType.DocumentType:
								continue;
							case XmlNodeType.Document:
								break;
							default:
								if (nodeType == XmlNodeType.XmlDeclaration)
								{
									continue;
								}
								break;
							}
							throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_element"), this);
						}
						flag = true;
					}
				}
			}
			catch
			{
				this.ReleaseResources();
				throw;
			}
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0001DF98 File Offset: 0x0001CF98
		private void ReleaseResources()
		{
			if (this._reader != null)
			{
				this._reader.Close();
				this._reader = null;
			}
			else if (this._stream != null)
			{
				this._stream.Close();
			}
			this._stream = null;
			if (this._cachedStringWriter != null)
			{
				this._cachedStringWriter.Close();
				this._cachedStringWriter = null;
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0001DFF5 File Offset: 0x0001CFF5
		public void Dispose()
		{
			this.ReleaseResources();
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0001DFFD File Offset: 0x0001CFFD
		public string Filename
		{
			get
			{
				return this._streamName;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x0001E005 File Offset: 0x0001D005
		public int LineNumber
		{
			get
			{
				return this.Reader.LineNumber;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x0001E014 File Offset: 0x0001D014
		internal int TrueLinePosition
		{
			get
			{
				return this.Reader.LinePosition - XmlUtil.GetPositionOffset(this.Reader.NodeType);
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x0001E03F File Offset: 0x0001D03F
		internal XmlTextReader Reader
		{
			get
			{
				return this._reader;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0001E047 File Offset: 0x0001D047
		internal ConfigurationSchemaErrors SchemaErrors
		{
			get
			{
				return this._schemaErrors;
			}
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0001E04F File Offset: 0x0001D04F
		internal void ReadToNextElement()
		{
			while (this._reader.Read())
			{
				if (this._reader.MoveToContent() == XmlNodeType.Element)
				{
					return;
				}
			}
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0001E070 File Offset: 0x0001D070
		internal void SkipToNextElement()
		{
			this._reader.Skip();
			this._reader.MoveToContent();
			while (!this._reader.EOF && this._reader.NodeType != XmlNodeType.Element)
			{
				this._reader.Read();
				this._reader.MoveToContent();
			}
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0001E0C9 File Offset: 0x0001D0C9
		internal void StrictReadToNextElement(ExceptionAction action)
		{
			while (this._reader.Read())
			{
				if (this._reader.NodeType == XmlNodeType.Element)
				{
					return;
				}
				this.VerifyIgnorableNodeType(action);
			}
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001E0F0 File Offset: 0x0001D0F0
		internal void StrictSkipToNextElement(ExceptionAction action)
		{
			this._reader.Skip();
			while (!this._reader.EOF && this._reader.NodeType != XmlNodeType.Element)
			{
				this.VerifyIgnorableNodeType(action);
				this._reader.Read();
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0001E130 File Offset: 0x0001D130
		internal void StrictSkipToOurParentsEndElement(ExceptionAction action)
		{
			int depth = this._reader.Depth;
			while (this._reader.Depth >= depth)
			{
				this._reader.Skip();
			}
			while (!this._reader.EOF && this._reader.NodeType != XmlNodeType.EndElement)
			{
				this.VerifyIgnorableNodeType(action);
				this._reader.Read();
			}
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001E198 File Offset: 0x0001D198
		internal void VerifyIgnorableNodeType(ExceptionAction action)
		{
			XmlNodeType nodeType = this._reader.NodeType;
			if (nodeType != XmlNodeType.Comment && nodeType != XmlNodeType.EndElement)
			{
				ConfigurationException ex = new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_element"), this);
				this.SchemaErrors.AddError(ex, action);
			}
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001E1D8 File Offset: 0x0001D1D8
		internal void VerifyNoUnrecognizedAttributes(ExceptionAction action)
		{
			if (this._reader.MoveToNextAttribute())
			{
				this.AddErrorUnrecognizedAttribute(action);
			}
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001E1EE File Offset: 0x0001D1EE
		internal bool VerifyRequiredAttribute(object o, string attrName, ExceptionAction action)
		{
			if (o == null)
			{
				this.AddErrorRequiredAttribute(attrName, action);
				return false;
			}
			return true;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001E200 File Offset: 0x0001D200
		internal void AddErrorUnrecognizedAttribute(ExceptionAction action)
		{
			ConfigurationErrorsException ex = new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_attribute", new object[] { this._reader.Name }), this);
			this.SchemaErrors.AddError(ex, action);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0001E244 File Offset: 0x0001D244
		internal void AddErrorRequiredAttribute(string attrib, ExceptionAction action)
		{
			ConfigurationErrorsException ex = new ConfigurationErrorsException(SR.GetString("Config_missing_required_attribute", new object[]
			{
				attrib,
				this._reader.Name
			}), this);
			this.SchemaErrors.AddError(ex, action);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0001E28C File Offset: 0x0001D28C
		internal void AddErrorReservedAttribute(ExceptionAction action)
		{
			ConfigurationErrorsException ex = new ConfigurationErrorsException(SR.GetString("Config_reserved_attribute", new object[] { this._reader.Name }), this);
			this.SchemaErrors.AddError(ex, action);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0001E2D0 File Offset: 0x0001D2D0
		internal void AddErrorUnrecognizedElement(ExceptionAction action)
		{
			ConfigurationErrorsException ex = new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_element"), this);
			this.SchemaErrors.AddError(ex, action);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0001E2FC File Offset: 0x0001D2FC
		internal void VerifyAndGetNonEmptyStringAttribute(ExceptionAction action, out string newValue)
		{
			if (!string.IsNullOrEmpty(this._reader.Value))
			{
				newValue = this._reader.Value;
				return;
			}
			newValue = null;
			ConfigurationException ex = new ConfigurationErrorsException(SR.GetString("Empty_attribute", new object[] { this._reader.Name }), this);
			this.SchemaErrors.AddError(ex, action);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0001E360 File Offset: 0x0001D360
		internal void VerifyAndGetBooleanAttribute(ExceptionAction action, bool defaultValue, out bool newValue)
		{
			if (this._reader.Value == "true")
			{
				newValue = true;
				return;
			}
			if (this._reader.Value == "false")
			{
				newValue = false;
				return;
			}
			newValue = defaultValue;
			ConfigurationErrorsException ex = new ConfigurationErrorsException(SR.GetString("Config_invalid_boolean_attribute", new object[] { this._reader.Name }), this);
			this.SchemaErrors.AddError(ex, action);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001E3DA File Offset: 0x0001D3DA
		internal bool CopyOuterXmlToNextElement(XmlUtilWriter utilWriter, bool limitDepth)
		{
			this.CopyElement(utilWriter);
			return this.CopyReaderToNextElement(utilWriter, limitDepth);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001E3EC File Offset: 0x0001D3EC
		internal bool SkipChildElementsAndCopyOuterXmlToNextElement(XmlUtilWriter utilWriter)
		{
			bool isEmptyElement = this._reader.IsEmptyElement;
			int lineNumber = this._reader.LineNumber;
			this.CopyXmlNode(utilWriter);
			if (!isEmptyElement)
			{
				while (this._reader.NodeType != XmlNodeType.EndElement)
				{
					if (this._reader.NodeType == XmlNodeType.Element)
					{
						this._reader.Skip();
						if (this._reader.NodeType == XmlNodeType.Whitespace)
						{
							this._reader.Skip();
						}
					}
					else
					{
						this.CopyXmlNode(utilWriter);
					}
				}
				if (this._reader.LineNumber != lineNumber)
				{
					utilWriter.AppendSpacesToLinePosition(this.TrueLinePosition);
				}
				this.CopyXmlNode(utilWriter);
			}
			return this.CopyReaderToNextElement(utilWriter, true);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001E498 File Offset: 0x0001D498
		internal bool CopyReaderToNextElement(XmlUtilWriter utilWriter, bool limitDepth)
		{
			bool flag = true;
			int num;
			if (limitDepth)
			{
				if (this._reader.NodeType == XmlNodeType.EndElement)
				{
					return true;
				}
				num = this._reader.Depth;
			}
			else
			{
				num = 0;
			}
			while (this._reader.NodeType != XmlNodeType.Element && this._reader.Depth >= num)
			{
				flag = this.CopyXmlNode(utilWriter);
				if (!flag)
				{
					break;
				}
			}
			return flag;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0001E4F4 File Offset: 0x0001D4F4
		internal bool SkipAndCopyReaderToNextElement(XmlUtilWriter utilWriter, bool limitDepth)
		{
			if (!utilWriter.IsLastLineBlank)
			{
				this._reader.Skip();
				return this.CopyReaderToNextElement(utilWriter, limitDepth);
			}
			int num;
			if (limitDepth)
			{
				num = this._reader.Depth;
			}
			else
			{
				num = 0;
			}
			this._reader.Skip();
			int lineNumber = this._reader.LineNumber;
			while (!this._reader.EOF)
			{
				if (this._reader.NodeType != XmlNodeType.Whitespace)
				{
					if (this._reader.LineNumber > lineNumber)
					{
						utilWriter.SeekToLineStart();
						utilWriter.AppendWhiteSpace(lineNumber + 1, 1, this.LineNumber, this.TrueLinePosition);
					}
					IL_00C3:
					while (!this._reader.EOF && this._reader.NodeType != XmlNodeType.Element && this._reader.Depth >= num)
					{
						this.CopyXmlNode(utilWriter);
					}
					return !this._reader.EOF;
				}
				this._reader.Read();
			}
			goto IL_00C3;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001E5E0 File Offset: 0x0001D5E0
		private void CopyElement(XmlUtilWriter utilWriter)
		{
			int depth = this._reader.Depth;
			bool isEmptyElement = this._reader.IsEmptyElement;
			this.CopyXmlNode(utilWriter);
			while (this._reader.Depth > depth)
			{
				this.CopyXmlNode(utilWriter);
			}
			if (!isEmptyElement)
			{
				this.CopyXmlNode(utilWriter);
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0001E630 File Offset: 0x0001D630
		internal bool CopyXmlNode(XmlUtilWriter utilWriter)
		{
			string text = null;
			int num = -1;
			int num2 = -1;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			if (utilWriter.TrackPosition)
			{
				num3 = this._reader.LineNumber;
				num4 = this._reader.LinePosition;
				num5 = utilWriter.LineNumber;
				num6 = utilWriter.LinePosition;
			}
			XmlNodeType xmlNodeType = this._reader.NodeType;
			if (xmlNodeType == XmlNodeType.Whitespace)
			{
				utilWriter.Write(this._reader.Value);
			}
			else if (xmlNodeType == XmlNodeType.Element)
			{
				text = (this._reader.IsEmptyElement ? "/>" : ">");
				num = this._reader.LineNumber;
				num2 = this._reader.LinePosition + this._reader.Name.Length;
				utilWriter.Write('<');
				utilWriter.Write(this._reader.Name);
				while (this._reader.MoveToNextAttribute())
				{
					int lineNumber = this._reader.LineNumber;
					int linePosition = this._reader.LinePosition;
					utilWriter.AppendRequiredWhiteSpace(num, num2, lineNumber, linePosition);
					int num7 = utilWriter.Write(this._reader.Name);
					num7 += utilWriter.Write('=');
					num7 += utilWriter.AppendAttributeValue(this._reader);
					num = lineNumber;
					num2 = linePosition + num7;
				}
			}
			else if (xmlNodeType == XmlNodeType.EndElement)
			{
				text = ">";
				num = this._reader.LineNumber;
				num2 = this._reader.LinePosition + this._reader.Name.Length;
				utilWriter.Write("</");
				utilWriter.Write(this._reader.Name);
			}
			else if (xmlNodeType == XmlNodeType.Comment)
			{
				utilWriter.AppendComment(this._reader.Value);
			}
			else if (xmlNodeType == XmlNodeType.Text)
			{
				utilWriter.AppendEscapeTextString(this._reader.Value);
			}
			else if (xmlNodeType == XmlNodeType.XmlDeclaration)
			{
				text = "?>";
				num = this._reader.LineNumber;
				num2 = this._reader.LinePosition + 3;
				utilWriter.Write("<?xml");
				while (this._reader.MoveToNextAttribute())
				{
					int lineNumber2 = this._reader.LineNumber;
					int linePosition2 = this._reader.LinePosition;
					utilWriter.AppendRequiredWhiteSpace(num, num2, lineNumber2, linePosition2);
					int num8 = utilWriter.Write(this._reader.Name);
					num8 += utilWriter.Write('=');
					num8 += utilWriter.AppendAttributeValue(this._reader);
					num = lineNumber2;
					num2 = linePosition2 + num8;
				}
				this._reader.MoveToElement();
			}
			else if (xmlNodeType == XmlNodeType.SignificantWhitespace)
			{
				utilWriter.Write(this._reader.Value);
			}
			else if (xmlNodeType == XmlNodeType.ProcessingInstruction)
			{
				utilWriter.AppendProcessingInstruction(this._reader.Name, this._reader.Value);
			}
			else if (xmlNodeType == XmlNodeType.EntityReference)
			{
				utilWriter.AppendEntityRef(this._reader.Name);
			}
			else if (xmlNodeType == XmlNodeType.CDATA)
			{
				utilWriter.AppendCData(this._reader.Value);
			}
			else if (xmlNodeType == XmlNodeType.DocumentType)
			{
				int num9 = utilWriter.Write("<!DOCTYPE");
				utilWriter.AppendRequiredWhiteSpace(this._lastLineNumber, this._lastLinePosition + num9, this._reader.LineNumber, this._reader.LinePosition);
				utilWriter.Write(this._reader.Name);
				string text2 = null;
				if (this._reader.HasValue)
				{
					text2 = this._reader.Value;
				}
				num = this._reader.LineNumber;
				num2 = this._reader.LinePosition + this._reader.Name.Length;
				if (this._reader.MoveToFirstAttribute())
				{
					utilWriter.AppendRequiredWhiteSpace(num, num2, this._reader.LineNumber, this._reader.LinePosition);
					string name = this._reader.Name;
					utilWriter.Write(name);
					utilWriter.AppendSpace();
					utilWriter.AppendAttributeValue(this._reader);
					this._reader.MoveToAttribute(0);
					if (name == "PUBLIC")
					{
						this._reader.MoveToAttribute(1);
						utilWriter.AppendSpace();
						utilWriter.AppendAttributeValue(this._reader);
						this._reader.MoveToAttribute(1);
					}
				}
				if (text2 != null && text2.Length > 0)
				{
					utilWriter.Write(" [");
					utilWriter.Write(text2);
					utilWriter.Write(']');
				}
				utilWriter.Write('>');
			}
			bool flag = this._reader.Read();
			xmlNodeType = this._reader.NodeType;
			if (text != null)
			{
				int positionOffset = XmlUtil.GetPositionOffset(xmlNodeType);
				int lineNumber3 = this._reader.LineNumber;
				int num10 = this._reader.LinePosition - positionOffset - text.Length;
				utilWriter.AppendWhiteSpace(num, num2, lineNumber3, num10);
				utilWriter.Write(text);
			}
			if (utilWriter.TrackPosition)
			{
				this._lastLineNumber = num3 - num5 + utilWriter.LineNumber;
				if (num5 == utilWriter.LineNumber)
				{
					this._lastLinePosition = num4 - num6 + utilWriter.LinePosition;
				}
				else
				{
					this._lastLinePosition = utilWriter.LinePosition;
				}
			}
			return flag;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001EB58 File Offset: 0x0001DB58
		private string RetrieveFullOpenElementTag()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("<");
			stringBuilder.Append(this._reader.Name);
			while (this._reader.MoveToNextAttribute())
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(this._reader.Name);
				stringBuilder.Append("=");
				stringBuilder.Append('"');
				stringBuilder.Append(this._reader.Value);
				stringBuilder.Append('"');
			}
			stringBuilder.Append(">");
			return stringBuilder.ToString();
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0001EBFC File Offset: 0x0001DBFC
		internal string UpdateStartElement(XmlUtilWriter utilWriter, string updatedStartElement, bool needsChildren, int linePosition, int indent)
		{
			string text = null;
			bool flag = false;
			string name = this._reader.Name;
			if (this._reader.IsEmptyElement)
			{
				if (updatedStartElement == null && needsChildren)
				{
					updatedStartElement = this.RetrieveFullOpenElementTag();
				}
				flag = updatedStartElement != null;
			}
			if (updatedStartElement == null)
			{
				this.CopyXmlNode(utilWriter);
			}
			else
			{
				string text2 = "</" + name + ">";
				string text3 = updatedStartElement + text2;
				string text4 = XmlUtil.FormatXmlElement(text3, linePosition, indent, true);
				int num = text4.LastIndexOf('\n') + 1;
				string text5;
				if (flag)
				{
					text = text4.Substring(num);
					text5 = text4.Substring(0, num);
				}
				else
				{
					text5 = text4.Substring(0, num - 2);
				}
				utilWriter.Write(text5);
				this._reader.Read();
			}
			return text;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001ECBD File Offset: 0x0001DCBD
		private void ResetCachedStringWriter()
		{
			if (this._cachedStringWriter == null)
			{
				this._cachedStringWriter = new StringWriter(new StringBuilder(64), CultureInfo.InvariantCulture);
				return;
			}
			this._cachedStringWriter.GetStringBuilder().Length = 0;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001ECF0 File Offset: 0x0001DCF0
		internal string CopySection()
		{
			this.ResetCachedStringWriter();
			WhitespaceHandling whitespaceHandling = this._reader.WhitespaceHandling;
			this._reader.WhitespaceHandling = WhitespaceHandling.All;
			XmlUtilWriter xmlUtilWriter = new XmlUtilWriter(this._cachedStringWriter, false);
			this.CopyElement(xmlUtilWriter);
			this._reader.WhitespaceHandling = whitespaceHandling;
			if (whitespaceHandling == WhitespaceHandling.None && this.Reader.NodeType == XmlNodeType.Whitespace)
			{
				this._reader.Read();
			}
			xmlUtilWriter.Flush();
			return ((StringWriter)xmlUtilWriter.Writer).ToString();
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001ED74 File Offset: 0x0001DD74
		internal static string FormatXmlElement(string xmlElement, int linePosition, int indent, bool skipFirstIndent)
		{
			XmlParserContext xmlParserContext = new XmlParserContext(null, null, null, XmlSpace.Default, Encoding.Unicode);
			XmlTextReader xmlTextReader = new XmlTextReader(xmlElement, XmlNodeType.Element, xmlParserContext);
			StringWriter stringWriter = new StringWriter(new StringBuilder(64), CultureInfo.InvariantCulture);
			XmlUtilWriter xmlUtilWriter = new XmlUtilWriter(stringWriter, false);
			bool flag = false;
			bool flag2 = false;
			int num = 0;
			while (xmlTextReader.Read())
			{
				XmlNodeType nodeType = xmlTextReader.NodeType;
				int num2;
				if (flag2)
				{
					xmlUtilWriter.Flush();
					num2 = num - ((StringWriter)xmlUtilWriter.Writer).GetStringBuilder().Length;
				}
				else
				{
					num2 = 0;
				}
				XmlNodeType xmlNodeType = nodeType;
				if (xmlNodeType <= XmlNodeType.CDATA)
				{
					if (xmlNodeType == XmlNodeType.Element || xmlNodeType == XmlNodeType.CDATA)
					{
						goto IL_0091;
					}
				}
				else if (xmlNodeType == XmlNodeType.Comment || xmlNodeType == XmlNodeType.EndElement)
				{
					goto IL_0091;
				}
				IL_00CA:
				flag2 = false;
				switch (nodeType)
				{
				case XmlNodeType.Element:
				{
					xmlUtilWriter.Write('<');
					xmlUtilWriter.Write(xmlTextReader.Name);
					num2 += xmlTextReader.Name.Length + 2;
					int attributeCount = xmlTextReader.AttributeCount;
					for (int i = 0; i < attributeCount; i++)
					{
						bool flag3;
						if (num2 > 60)
						{
							xmlUtilWriter.AppendIndent(linePosition, indent, xmlTextReader.Depth - 1, true);
							num2 = indent;
							flag3 = false;
							xmlUtilWriter.Flush();
							num = ((StringWriter)xmlUtilWriter.Writer).GetStringBuilder().Length;
						}
						else
						{
							flag3 = true;
						}
						xmlTextReader.MoveToNextAttribute();
						xmlUtilWriter.Flush();
						int length = ((StringWriter)xmlUtilWriter.Writer).GetStringBuilder().Length;
						if (flag3)
						{
							xmlUtilWriter.AppendSpace();
						}
						xmlUtilWriter.Write(xmlTextReader.Name);
						xmlUtilWriter.Write('=');
						xmlUtilWriter.AppendAttributeValue(xmlTextReader);
						xmlUtilWriter.Flush();
						num2 += ((StringWriter)xmlUtilWriter.Writer).GetStringBuilder().Length - length;
					}
					xmlTextReader.MoveToElement();
					if (xmlTextReader.IsEmptyElement)
					{
						xmlUtilWriter.Write(" />");
					}
					else
					{
						xmlUtilWriter.Write('>');
					}
					break;
				}
				case XmlNodeType.Text:
					xmlUtilWriter.AppendEscapeTextString(xmlTextReader.Value);
					flag2 = true;
					break;
				case XmlNodeType.CDATA:
					xmlUtilWriter.AppendCData(xmlTextReader.Value);
					break;
				case XmlNodeType.EntityReference:
					xmlUtilWriter.AppendEntityRef(xmlTextReader.Name);
					break;
				case XmlNodeType.ProcessingInstruction:
					xmlUtilWriter.AppendProcessingInstruction(xmlTextReader.Name, xmlTextReader.Value);
					break;
				case XmlNodeType.Comment:
					xmlUtilWriter.AppendComment(xmlTextReader.Value);
					break;
				case XmlNodeType.SignificantWhitespace:
					xmlUtilWriter.Write(xmlTextReader.Value);
					break;
				case XmlNodeType.EndElement:
					xmlUtilWriter.Write("</");
					xmlUtilWriter.Write(xmlTextReader.Name);
					xmlUtilWriter.Write('>');
					break;
				}
				flag = true;
				skipFirstIndent = false;
				continue;
				IL_0091:
				if (skipFirstIndent || flag2)
				{
					goto IL_00CA;
				}
				xmlUtilWriter.AppendIndent(linePosition, indent, xmlTextReader.Depth, flag);
				if (flag)
				{
					xmlUtilWriter.Flush();
					num = ((StringWriter)xmlUtilWriter.Writer).GetStringBuilder().Length;
					goto IL_00CA;
				}
				goto IL_00CA;
			}
			xmlUtilWriter.Flush();
			return ((StringWriter)xmlUtilWriter.Writer).ToString();
		}

		// Token: 0x04000400 RID: 1024
		private const int MAX_LINE_WIDTH = 60;

		// Token: 0x04000401 RID: 1025
		private static readonly int[] s_positionOffset = new int[]
		{
			0, 1, -1, 0, 9, 1, -1, 2, 4, -1,
			10, -1, -1, 0, 0, 2, -1, 2
		};

		// Token: 0x04000402 RID: 1026
		private Stream _stream;

		// Token: 0x04000403 RID: 1027
		private string _streamName;

		// Token: 0x04000404 RID: 1028
		private XmlTextReader _reader;

		// Token: 0x04000405 RID: 1029
		private StringWriter _cachedStringWriter;

		// Token: 0x04000406 RID: 1030
		private ConfigurationSchemaErrors _schemaErrors;

		// Token: 0x04000407 RID: 1031
		private int _lastLineNumber;

		// Token: 0x04000408 RID: 1032
		private int _lastLinePosition;
	}
}
