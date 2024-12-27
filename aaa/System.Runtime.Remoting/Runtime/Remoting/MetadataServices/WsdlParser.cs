using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Xml;

namespace System.Runtime.Remoting.MetadataServices
{
	// Token: 0x0200007B RID: 123
	internal class WsdlParser
	{
		// Token: 0x0600039A RID: 922 RVA: 0x00011AB8 File Offset: 0x00010AB8
		internal WsdlParser(TextReader input, string outputDir, ArrayList outCodeStreamList, string locationURL, bool bWrappedProxy, string proxyNamespace)
		{
			this._XMLReader = null;
			this._readerStreamsWsdl = new WsdlParser.ReaderStream(locationURL);
			this._readerStreamsWsdl.InputStream = input;
			this._writerStreams = null;
			this._outputDir = outputDir;
			this._outCodeStreamList = outCodeStreamList;
			this._bWrappedProxy = bWrappedProxy;
			if (proxyNamespace == null || proxyNamespace.Length == 0)
			{
				this._proxyNamespace = "InteropNS";
			}
			else
			{
				this._proxyNamespace = proxyNamespace;
			}
			if (outputDir == null)
			{
				outputDir = ".";
			}
			int length = outputDir.Length;
			if (length > 0)
			{
				char c = outputDir[length - 1];
				if (c != '\\' && c != '/')
				{
					this._outputDir += '\\';
				}
			}
			this._URTNamespaces = new ArrayList();
			this._blockDefault = SchemaBlockType.ALL;
			this._primedNametable = WsdlParser.CreatePrimedNametable();
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00011BD4 File Offset: 0x00010BD4
		internal string SchemaNamespaceString
		{
			get
			{
				string text = null;
				switch (this._xsdVersion)
				{
				case XsdVersion.V1999:
					text = WsdlParser.s_schemaNamespaceString1999;
					break;
				case XsdVersion.V2000:
					text = WsdlParser.s_schemaNamespaceString2000;
					break;
				case XsdVersion.V2001:
					text = WsdlParser.s_schemaNamespaceString;
					break;
				}
				return text;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00011C15 File Offset: 0x00010C15
		internal string ProxyNamespace
		{
			get
			{
				return this._proxyNamespace;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00011C1D File Offset: 0x00010C1D
		// (set) Token: 0x0600039E RID: 926 RVA: 0x00011C25 File Offset: 0x00010C25
		internal int ProxyNamespaceCount
		{
			get
			{
				return this._proxyNamespaceCount;
			}
			set
			{
				this._proxyNamespaceCount = value;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00011C2E File Offset: 0x00010C2E
		internal XmlTextReader XMLReader
		{
			get
			{
				return this._XMLReader;
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00011C38 File Offset: 0x00010C38
		private bool SkipXmlElement()
		{
			this._XMLReader.Skip();
			XmlNodeType xmlNodeType = this._XMLReader.MoveToContent();
			while (xmlNodeType == XmlNodeType.EndElement)
			{
				this._XMLReader.Read();
				xmlNodeType = this._XMLReader.MoveToContent();
				if (xmlNodeType == XmlNodeType.None)
				{
					break;
				}
			}
			return xmlNodeType != XmlNodeType.None;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00011C88 File Offset: 0x00010C88
		private bool ReadNextXmlElement()
		{
			this._XMLReader.Read();
			XmlNodeType xmlNodeType = this._XMLReader.MoveToContent();
			while (xmlNodeType == XmlNodeType.EndElement)
			{
				this._XMLReader.Read();
				xmlNodeType = this._XMLReader.MoveToContent();
				if (xmlNodeType == XmlNodeType.None)
				{
					break;
				}
			}
			return xmlNodeType != XmlNodeType.None;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00011CD8 File Offset: 0x00010CD8
		private WsdlParser.URTComplexType ParseComplexType(WsdlParser.URTNamespace parsingNamespace, string typeName)
		{
			if (typeName == null)
			{
				typeName = this.LookupAttribute(WsdlParser.s_nameString, null, true);
			}
			WsdlParser.URTNamespace urtnamespace = null;
			this.ParseQName(ref typeName, parsingNamespace, out urtnamespace);
			WsdlParser.URTComplexType urtcomplexType = urtnamespace.LookupComplexType(typeName);
			if (urtcomplexType == null)
			{
				urtcomplexType = new WsdlParser.URTComplexType(typeName, urtnamespace.Name, urtnamespace.Namespace, urtnamespace.EncodedNS, this._blockDefault, false, typeName != null, this, urtnamespace);
				urtnamespace.AddComplexType(urtcomplexType);
			}
			string text = this.LookupAttribute(WsdlParser.s_baseString, null, false);
			if (!WsdlParser.MatchingStrings(text, WsdlParser.s_emptyString))
			{
				string text2 = this.ParseQName(ref text, parsingNamespace);
				urtcomplexType.Extends(text, text2);
			}
			if (urtcomplexType.Fields.Count > 0)
			{
				this.SkipXmlElement();
			}
			else
			{
				int depth = this._XMLReader.Depth;
				this.ReadNextXmlElement();
				int num = 0;
				while (this._XMLReader.Depth > depth)
				{
					string localName = this._XMLReader.LocalName;
					if (WsdlParser.MatchingStrings(localName, WsdlParser.s_elementString))
					{
						this.ParseElementField(urtnamespace, urtcomplexType, num);
						num++;
					}
					else if (WsdlParser.MatchingStrings(localName, WsdlParser.s_attributeString))
					{
						this.ParseAttributeField(urtnamespace, urtcomplexType);
					}
					else
					{
						if (WsdlParser.MatchingStrings(localName, WsdlParser.s_allString))
						{
							urtcomplexType.BlockType = SchemaBlockType.ALL;
						}
						else if (WsdlParser.MatchingStrings(localName, WsdlParser.s_sequenceString))
						{
							urtcomplexType.BlockType = SchemaBlockType.SEQUENCE;
						}
						else if (WsdlParser.MatchingStrings(localName, WsdlParser.s_choiceString))
						{
							urtcomplexType.BlockType = SchemaBlockType.CHOICE;
						}
						else if (WsdlParser.MatchingStrings(localName, WsdlParser.s_complexContentString))
						{
							urtcomplexType.BlockType = SchemaBlockType.ComplexContent;
						}
						else
						{
							if (WsdlParser.MatchingStrings(localName, WsdlParser.s_restrictionString))
							{
								this.ParseRestrictionField(urtnamespace, urtcomplexType);
								continue;
							}
							this.SkipXmlElement();
							continue;
						}
						this.ReadNextXmlElement();
					}
				}
			}
			return urtcomplexType;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00011E88 File Offset: 0x00010E88
		private WsdlParser.URTSimpleType ParseSimpleType(WsdlParser.URTNamespace parsingNamespace, string typeName)
		{
			if (typeName == null)
			{
				typeName = this.LookupAttribute(WsdlParser.s_nameString, null, true);
			}
			string text = this.LookupAttribute(WsdlParser.s_enumTypeString, WsdlParser.s_wsdlSudsNamespaceString, false);
			WsdlParser.URTSimpleType urtsimpleType = parsingNamespace.LookupSimpleType(typeName);
			if (urtsimpleType == null)
			{
				urtsimpleType = new WsdlParser.URTSimpleType(typeName, parsingNamespace.Name, parsingNamespace.Namespace, parsingNamespace.EncodedNS, typeName != null, this);
				string text2 = this.LookupAttribute(WsdlParser.s_baseString, null, false);
				if (!WsdlParser.MatchingStrings(text2, WsdlParser.s_emptyString))
				{
					string text3 = this.ParseQName(ref text2, parsingNamespace);
					urtsimpleType.Extends(text2, text3);
				}
				parsingNamespace.AddSimpleType(urtsimpleType);
				int depth = this._XMLReader.Depth;
				this.ReadNextXmlElement();
				while (this._XMLReader.Depth > depth)
				{
					string localName = this._XMLReader.LocalName;
					if (WsdlParser.MatchingStrings(localName, WsdlParser.s_restrictionString))
					{
						this.ParseRestrictionField(parsingNamespace, urtsimpleType);
					}
					else
					{
						this.SkipXmlElement();
					}
				}
			}
			else
			{
				this.SkipXmlElement();
			}
			if (text != null)
			{
				urtsimpleType.EnumType = text;
			}
			return urtsimpleType;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00011F84 File Offset: 0x00010F84
		private void ParseEnumeration(WsdlParser.URTSimpleType parsingSimpleType, int enumFacetNum)
		{
			if (this._XMLReader.IsEmptyElement)
			{
				string text = this.LookupAttribute(WsdlParser.s_valueString, null, true);
				parsingSimpleType.IsEnum = true;
				parsingSimpleType.AddFacet(new WsdlParser.EnumFacet(text, enumFacetNum));
				return;
			}
			throw new SUDSParserException(CoreChannel.GetResourceString("Remoting_Suds_EnumMustBeEmpty"));
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00011FD0 File Offset: 0x00010FD0
		private void ParseElementField(WsdlParser.URTNamespace parsingNamespace, WsdlParser.URTComplexType parsingComplexType, int fieldNum)
		{
			string text = this.LookupAttribute(WsdlParser.s_nameString, null, true);
			string text2 = this.LookupAttribute(WsdlParser.s_minOccursString, null, false);
			string text3 = this.LookupAttribute(WsdlParser.s_maxOccursString, null, false);
			bool flag = false;
			if (WsdlParser.MatchingStrings(text2, WsdlParser.s_zeroString))
			{
				flag = true;
			}
			bool flag2 = false;
			string text4 = null;
			if (!WsdlParser.MatchingStrings(text3, WsdlParser.s_emptyString) && !WsdlParser.MatchingStrings(text3, WsdlParser.s_oneString))
			{
				if (WsdlParser.MatchingStrings(text3, WsdlParser.s_unboundedString))
				{
					text4 = string.Empty;
				}
				else
				{
					text4 = text3;
				}
				flag2 = true;
			}
			string text5;
			string text6;
			bool flag3;
			bool flag4;
			if (this._XMLReader.IsEmptyElement)
			{
				text5 = this.LookupAttribute(WsdlParser.s_typeString, null, false);
				this.ResolveTypeAttribute(ref text5, out text6, out flag3, out flag4);
				this.ReadNextXmlElement();
			}
			else
			{
				text6 = parsingNamespace.Namespace;
				text5 = parsingNamespace.GetNextAnonymousName();
				flag4 = false;
				flag3 = true;
				int depth = this._XMLReader.Depth;
				this.ReadNextXmlElement();
				while (this._XMLReader.Depth > depth)
				{
					string localName = this._XMLReader.LocalName;
					if (WsdlParser.MatchingStrings(localName, WsdlParser.s_complexTypeString))
					{
						WsdlParser.URTComplexType urtcomplexType = this.ParseComplexType(parsingNamespace, text5);
						if (urtcomplexType.IsEmittableFieldType)
						{
							text6 = urtcomplexType.FieldNamespace;
							text5 = urtcomplexType.FieldName;
							flag4 = urtcomplexType.PrimitiveField;
							parsingNamespace.RemoveComplexType(urtcomplexType);
						}
					}
					else if (WsdlParser.MatchingStrings(localName, WsdlParser.s_simpleTypeString))
					{
						WsdlParser.URTSimpleType urtsimpleType = this.ParseSimpleType(parsingNamespace, text5);
						if (urtsimpleType.IsEmittableFieldType)
						{
							text6 = urtsimpleType.FieldNamespace;
							text5 = urtsimpleType.FieldName;
							flag4 = urtsimpleType.PrimitiveField;
							parsingNamespace.RemoveSimpleType(urtsimpleType);
						}
					}
					else
					{
						this.SkipXmlElement();
					}
				}
			}
			parsingComplexType.AddField(new WsdlParser.URTField(text, text5, text6, this, flag4, flag3, false, flag, flag2, text4, parsingNamespace));
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00012188 File Offset: 0x00011188
		private void ParseAttributeField(WsdlParser.URTNamespace parsingNamespace, WsdlParser.URTComplexType parsingComplexType)
		{
			string text = this.LookupAttribute(WsdlParser.s_nameString, null, true);
			bool flag = false;
			string text2 = this.LookupAttribute(WsdlParser.s_minOccursString, null, false);
			if (WsdlParser.MatchingStrings(text2, WsdlParser.s_zeroString))
			{
				flag = true;
			}
			string text3;
			string text4;
			bool flag2;
			bool flag3;
			if (this._XMLReader.IsEmptyElement)
			{
				text3 = this.LookupAttribute(WsdlParser.s_typeString, null, true);
				this.ResolveTypeAttribute(ref text3, out text4, out flag2, out flag3);
				this.ReadNextXmlElement();
				if (WsdlParser.MatchingStrings(text3, WsdlParser.s_idString) && this.MatchingSchemaStrings(text4))
				{
					parsingComplexType.IsStruct = false;
					return;
				}
			}
			else
			{
				text4 = parsingNamespace.Namespace;
				text3 = parsingNamespace.GetNextAnonymousName();
				flag3 = false;
				flag2 = true;
				int depth = this._XMLReader.Depth;
				this.ReadNextXmlElement();
				while (this._XMLReader.Depth > depth)
				{
					string localName = this._XMLReader.LocalName;
					if (WsdlParser.MatchingStrings(localName, WsdlParser.s_simpleTypeString))
					{
						WsdlParser.URTSimpleType urtsimpleType = this.ParseSimpleType(parsingNamespace, text3);
						if (urtsimpleType.IsEmittableFieldType)
						{
							text4 = urtsimpleType.FieldNamespace;
							text3 = urtsimpleType.FieldName;
							flag3 = urtsimpleType.PrimitiveField;
							parsingNamespace.RemoveSimpleType(urtsimpleType);
						}
					}
					else
					{
						this.SkipXmlElement();
					}
				}
			}
			parsingComplexType.AddField(new WsdlParser.URTField(text, text3, text4, this, flag3, flag2, true, flag, false, null, parsingNamespace));
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x000122C4 File Offset: 0x000112C4
		private void ParseRestrictionField(WsdlParser.URTNamespace parsingNamespace, WsdlParser.BaseType parsingType)
		{
			string text = this.LookupAttribute(WsdlParser.s_baseString, null, true);
			this.ParseQName(ref text, parsingNamespace);
			int depth = this._XMLReader.Depth;
			this.ReadNextXmlElement();
			int num = 0;
			while (this._XMLReader.Depth > depth)
			{
				string localName = this._XMLReader.LocalName;
				if (WsdlParser.MatchingStrings(localName, WsdlParser.s_attributeString))
				{
					string text2 = this.LookupAttribute(WsdlParser.s_refString, null, true);
					string text3 = this.ParseQName(ref text2, parsingNamespace);
					if (WsdlParser.MatchingStrings(text3, WsdlParser.s_soapEncodingString) && WsdlParser.MatchingStrings(text2, WsdlParser.s_arrayTypeString))
					{
						WsdlParser.URTComplexType urtcomplexType = (WsdlParser.URTComplexType)parsingType;
						string text4 = this.LookupAttribute(WsdlParser.s_arrayTypeString, WsdlParser.s_wsdlNamespaceString, true);
						WsdlParser.URTNamespace urtnamespace = null;
						this.ParseQName(ref text4, null, out urtnamespace);
						urtcomplexType.AddArray(text4, urtnamespace);
						urtnamespace.AddComplexType(urtcomplexType);
						urtcomplexType.IsPrint = false;
					}
				}
				else if (WsdlParser.MatchingStrings(localName, WsdlParser.s_enumerationString))
				{
					WsdlParser.URTSimpleType urtsimpleType = (WsdlParser.URTSimpleType)parsingType;
					this.ParseEnumeration(urtsimpleType, num);
					num++;
				}
				else
				{
					this.SkipXmlElement();
				}
				this.ReadNextXmlElement();
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x000123E4 File Offset: 0x000113E4
		private void ParseElementDecl(WsdlParser.URTNamespace parsingNamespace)
		{
			string text = this.LookupAttribute(WsdlParser.s_nameString, null, true);
			string name = parsingNamespace.Name;
			string text2 = this.LookupAttribute(WsdlParser.s_typeString, null, false);
			string name2;
			bool flag2;
			if (this._XMLReader.IsEmptyElement)
			{
				bool flag;
				this.ResolveTypeAttribute(ref text2, out name2, out flag, out flag2);
				this.ReadNextXmlElement();
			}
			else
			{
				name2 = parsingNamespace.Name;
				text2 = parsingNamespace.GetNextAnonymousName();
				bool flag = true;
				flag2 = false;
				int depth = this._XMLReader.Depth;
				this.ReadNextXmlElement();
				while (this._XMLReader.Depth > depth)
				{
					string localName = this._XMLReader.LocalName;
					if (WsdlParser.MatchingStrings(localName, WsdlParser.s_complexTypeString))
					{
						this.ParseComplexType(parsingNamespace, text2);
					}
					else if (WsdlParser.MatchingStrings(localName, WsdlParser.s_simpleTypeString))
					{
						this.ParseSimpleType(parsingNamespace, text2);
					}
					else
					{
						this.SkipXmlElement();
					}
				}
			}
			parsingNamespace.AddElementDecl(new WsdlParser.ElementDecl(text, name, text2, name2, flag2));
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x000124C8 File Offset: 0x000114C8
		private void ResolveTypeNames(ref string typeNS, ref string typeName, out bool bEmbedded, out bool bPrimitive)
		{
			bEmbedded = true;
			bool flag = false;
			if (WsdlParser.MatchingStrings(typeNS, WsdlParser.s_wsdlSoapNamespaceString))
			{
				if (WsdlParser.MatchingStrings(typeName, WsdlParser.s_referenceString))
				{
					bEmbedded = false;
				}
				else if (WsdlParser.MatchingStrings(typeName, WsdlParser.s_arrayString))
				{
					flag = true;
				}
			}
			if (!bEmbedded || flag)
			{
				typeName = this.LookupAttribute(WsdlParser.s_refTypeString, WsdlParser.s_wsdlSudsNamespaceString, true);
				typeNS = this.ParseQName(ref typeName);
			}
			bPrimitive = this.IsPrimitiveType(typeNS, typeName);
			if (bPrimitive)
			{
				typeName = this.MapSchemaTypesToCSharpTypes(typeName);
				bEmbedded = false;
				return;
			}
			if (WsdlParser.MatchingStrings(typeName, WsdlParser.s_urTypeString) && this.MatchingSchemaStrings(typeNS))
			{
				typeName = WsdlParser.s_objectString;
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00012570 File Offset: 0x00011570
		private WsdlParser.URTNamespace ParseNamespace()
		{
			string text = this.LookupAttribute(WsdlParser.s_targetNamespaceString, null, false);
			bool flag = false;
			if (WsdlParser.MatchingStrings(text, WsdlParser.s_emptyString) && WsdlParser.MatchingStrings(this._XMLReader.LocalName, WsdlParser.s_sudsString) && this._parsingInput.UniqueNS == null)
			{
				text = this._parsingInput.TargetNS;
				flag = true;
			}
			WsdlParser.URTNamespace urtnamespace = this.LookupNamespace(text);
			if (urtnamespace == null)
			{
				urtnamespace = new WsdlParser.URTNamespace(text, this);
			}
			if (flag)
			{
				this._parsingInput.UniqueNS = urtnamespace;
			}
			this.ReadNextXmlElement();
			return urtnamespace;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x000125F8 File Offset: 0x000115F8
		private void ParseReaderStreamLocation(WsdlParser.ReaderStream reader, WsdlParser.ReaderStream currentReaderStream)
		{
			string text = reader.Location;
			int num = text.IndexOf(':');
			if (num == -1)
			{
				if (currentReaderStream == null || currentReaderStream.Location == null)
				{
					throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_Import"), new object[] { reader.Location }));
				}
				if (currentReaderStream.Uri == null)
				{
					currentReaderStream.Uri = new Uri(currentReaderStream.Location);
				}
				Uri uri = new Uri(currentReaderStream.Uri, text);
				reader.Uri = uri;
				text = uri.ToString();
				num = text.IndexOf(':');
				if (num == -1)
				{
					return;
				}
				reader.Location = text;
			}
			string text2 = text.Substring(0, num).ToLower(CultureInfo.InvariantCulture);
			string text3 = text.Substring(num + 1);
			if (text2 == "file")
			{
				reader.InputStream = new StreamReader(text3);
				return;
			}
			if (text2.StartsWith("http", StringComparison.Ordinal))
			{
				WebRequest webRequest = WebRequest.Create(text);
				WebResponse response = webRequest.GetResponse();
				Stream responseStream = response.GetResponseStream();
				reader.InputStream = new StreamReader(responseStream);
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00012714 File Offset: 0x00011714
		private void ParseImport()
		{
			this.LookupAttribute(WsdlParser.s_namespaceString, null, true);
			string text = this.LookupAttribute(WsdlParser.s_locationString, null, false);
			if (text != null && text.Length > 0)
			{
				WsdlParser.ReaderStream readerStream = new WsdlParser.ReaderStream(text);
				this.ParseReaderStreamLocation(readerStream, (WsdlParser.ReaderStream)this._currentReaderStack.Peek());
				WsdlParser.ReaderStream.GetReaderStream(this._readerStreamsWsdl, readerStream);
			}
			this.ReadNextXmlElement();
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0001277C File Offset: 0x0001177C
		internal void Parse()
		{
			WsdlParser.ReaderStream readerStream = this._readerStreamsWsdl;
			do
			{
				this._XMLReader = new XmlTextReader(readerStream.InputStream, this._primedNametable);
				this._XMLReader.WhitespaceHandling = WhitespaceHandling.None;
				this._XMLReader.XmlResolver = null;
				this.ParseInput(readerStream);
				readerStream = WsdlParser.ReaderStream.GetNextReaderStream(readerStream);
			}
			while (readerStream != null);
			this.StartWsdlResolution();
			if (this._writerStreams != null)
			{
				WsdlParser.WriterStream.Close(this._writerStreams);
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x000127EC File Offset: 0x000117EC
		private void ParseInput(WsdlParser.ReaderStream input)
		{
			this._parsingInput = input;
			try
			{
				this.ReadNextXmlElement();
				string localName = this._XMLReader.LocalName;
				if (this.MatchingNamespace(WsdlParser.s_wsdlNamespaceString) && WsdlParser.MatchingStrings(localName, WsdlParser.s_definitionsString))
				{
					this._currentReaderStack.Push(input);
					this.ParseWsdl();
					this._currentReaderStack.Pop();
				}
				else if (this.MatchingNamespace(WsdlParser.s_wsdlNamespaceString) && WsdlParser.MatchingStrings(localName, WsdlParser.s_typesString))
				{
					this._currentReaderStack.Push(input);
					this.ParseWsdlTypes();
					this._currentReaderStack.Pop();
				}
				else
				{
					if (!this.MatchingSchemaNamespace() || !WsdlParser.MatchingStrings(localName, WsdlParser.s_schemaString))
					{
						throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_UnknownElementAtRootLevel"), new object[] { localName }));
					}
					this._currentReaderStack.Push(input);
					this.ParseSchema();
					this._currentReaderStack.Pop();
				}
			}
			finally
			{
				WsdlParser.WriterStream.Flush(this._writerStreams);
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00012904 File Offset: 0x00011904
		private void ParseWsdl()
		{
			int depth = this._XMLReader.Depth;
			this._parsingInput.Name = this.LookupAttribute(WsdlParser.s_nameString, null, false);
			this._parsingInput.TargetNS = this.LookupAttribute(WsdlParser.s_targetNamespaceString, null, false);
			WsdlParser.URTNamespace urtnamespace = this.ParseNamespace();
			while (this._XMLReader.Depth > depth)
			{
				string localName = this._XMLReader.LocalName;
				if (this.MatchingNamespace(WsdlParser.s_wsdlNamespaceString))
				{
					if (WsdlParser.MatchingStrings(localName, WsdlParser.s_typesString))
					{
						this.ParseWsdlTypes();
						continue;
					}
					if (WsdlParser.MatchingStrings(localName, WsdlParser.s_messageString))
					{
						this.ParseWsdlMessage();
						continue;
					}
					if (WsdlParser.MatchingStrings(localName, WsdlParser.s_portTypeString))
					{
						this.ParseWsdlPortType();
						continue;
					}
					if (WsdlParser.MatchingStrings(localName, WsdlParser.s_bindingString))
					{
						this.ParseWsdlBinding(urtnamespace);
						continue;
					}
					if (WsdlParser.MatchingStrings(localName, WsdlParser.s_serviceString))
					{
						this.ParseWsdlService();
						continue;
					}
					if (WsdlParser.MatchingStrings(localName, WsdlParser.s_importString))
					{
						this.ParseImport();
						continue;
					}
				}
				this.SkipXmlElement();
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00012A09 File Offset: 0x00011A09
		private void StartWsdlResolution()
		{
			this.ResolveWsdl();
			this.Resolve();
			this.PruneNamespaces();
			this.PrintCSC();
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00012A24 File Offset: 0x00011A24
		private void PruneNamespaces()
		{
			ArrayList arrayList = new ArrayList(10);
			for (int i = 0; i < this._URTNamespaces.Count; i++)
			{
				WsdlParser.URTNamespace urtnamespace = (WsdlParser.URTNamespace)this._URTNamespaces[i];
				if (urtnamespace.bReferenced)
				{
					arrayList.Add(urtnamespace);
				}
			}
			this._URTNamespaces = arrayList;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00012A78 File Offset: 0x00011A78
		[Conditional("_LOGGING")]
		private void DumpWsdl()
		{
			foreach (object obj in this.wsdlMessages)
			{
				((WsdlParser.IDump)((DictionaryEntry)obj).Value).Dump();
			}
			foreach (object obj2 in this.wsdlPortTypes)
			{
				((WsdlParser.IDump)((DictionaryEntry)obj2).Value).Dump();
			}
			foreach (object obj3 in this.wsdlBindings)
			{
				WsdlParser.WsdlBinding wsdlBinding = (WsdlParser.WsdlBinding)obj3;
				wsdlBinding.Dump();
			}
			foreach (object obj4 in this.wsdlServices)
			{
				WsdlParser.WsdlService wsdlService = (WsdlParser.WsdlService)obj4;
				wsdlService.Dump();
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00012BD8 File Offset: 0x00011BD8
		private void ParseWsdlTypes()
		{
			int depth = this._XMLReader.Depth;
			this.ReadNextXmlElement();
			this._currentSchemaReaderStack.Push(this._currentReaderStack.Peek());
			while (this._XMLReader.Depth > depth)
			{
				string localName = this._XMLReader.LocalName;
				if (this.MatchingSchemaNamespace() && WsdlParser.MatchingStrings(localName, WsdlParser.s_schemaString))
				{
					this.ParseSchema();
					if (this._readerStreamsXsd != null)
					{
						this.ParseImportedSchemaController();
					}
				}
				else
				{
					this.SkipXmlElement();
				}
			}
			this._currentSchemaReaderStack.Pop();
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00012C68 File Offset: 0x00011C68
		private void ParseSchemaIncludeElement()
		{
			this.ParseSchemaImportElement(false);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00012C71 File Offset: 0x00011C71
		private void ParseSchemaImportElement()
		{
			this.ParseSchemaImportElement(true);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00012C7C File Offset: 0x00011C7C
		private void ParseSchemaImportElement(bool bImport)
		{
			if (bImport)
			{
				this.LookupAttribute(WsdlParser.s_namespaceString, null, true);
			}
			string text = this.LookupAttribute(WsdlParser.s_schemaLocationString, null, false);
			if (text != null && text.Length > 0)
			{
				if (this._readerStreamsXsd == null)
				{
					this._readerStreamsXsd = new WsdlParser.ReaderStream(text);
					this.ParseReaderStreamLocation(this._readerStreamsXsd, (WsdlParser.ReaderStream)this._currentSchemaReaderStack.Peek());
				}
				else
				{
					WsdlParser.ReaderStream readerStream = new WsdlParser.ReaderStream(text);
					this.ParseReaderStreamLocation(readerStream, (WsdlParser.ReaderStream)this._currentSchemaReaderStack.Peek());
					WsdlParser.ReaderStream.GetReaderStream(this._readerStreamsWsdl, readerStream);
				}
			}
			this.ReadNextXmlElement();
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00012D1C File Offset: 0x00011D1C
		internal void ParseImportedSchemaController()
		{
			WsdlParser.CreatePrimedNametable();
			WsdlParser.ReaderStream readerStream = this._readerStreamsXsd;
			XmlTextReader xmlreader = this._XMLReader;
			WsdlParser.ReaderStream parsingInput = this._parsingInput;
			do
			{
				this._XMLReader = new XmlTextReader(readerStream.InputStream, this._primedNametable);
				this._XMLReader.WhitespaceHandling = WhitespaceHandling.None;
				this._XMLReader.XmlResolver = null;
				this._parsingInput = readerStream;
				this.ParseImportedSchema(readerStream);
				readerStream = WsdlParser.ReaderStream.GetNextReaderStream(readerStream);
			}
			while (readerStream != null);
			this._readerStreamsXsd = null;
			this._XMLReader = xmlreader;
			this._parsingInput = parsingInput;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00012DA0 File Offset: 0x00011DA0
		private void ParseImportedSchema(WsdlParser.ReaderStream input)
		{
			try
			{
				string localName = this._XMLReader.LocalName;
				this._currentSchemaReaderStack.Push(input);
				this.ReadNextXmlElement();
				this.ParseSchema();
				this._currentSchemaReaderStack.Pop();
			}
			finally
			{
				WsdlParser.WriterStream.Flush(this._writerStreams);
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00012DFC File Offset: 0x00011DFC
		private void ParseWsdlMessage()
		{
			WsdlParser.WsdlMessage wsdlMessage = new WsdlParser.WsdlMessage();
			wsdlMessage.name = this.LookupAttribute(WsdlParser.s_nameString, null, true);
			wsdlMessage.nameNs = this._parsingInput.TargetNS;
			int depth = this._XMLReader.Depth;
			this.ReadNextXmlElement();
			while (this._XMLReader.Depth > depth)
			{
				string localName = this._XMLReader.LocalName;
				if (WsdlParser.MatchingStrings(localName, WsdlParser.s_partString))
				{
					WsdlParser.WsdlMessagePart wsdlMessagePart = new WsdlParser.WsdlMessagePart();
					wsdlMessagePart.name = this.LookupAttribute(WsdlParser.s_nameString, null, true);
					wsdlMessagePart.nameNs = this._parsingInput.TargetNS;
					wsdlMessagePart.element = this.LookupAttribute(WsdlParser.s_elementString, null, false);
					wsdlMessagePart.typeName = this.LookupAttribute(WsdlParser.s_typeString, null, false);
					if (wsdlMessagePart.element != null)
					{
						wsdlMessagePart.elementNs = this.ParseQName(ref wsdlMessagePart.element);
					}
					if (wsdlMessagePart.typeName != null)
					{
						wsdlMessagePart.typeNameNs = this.ParseQName(ref wsdlMessagePart.typeName);
					}
					wsdlMessage.parts.Add(wsdlMessagePart);
					this.ReadNextXmlElement();
				}
				else
				{
					this.SkipXmlElement();
				}
			}
			this.wsdlMessages[wsdlMessage.name] = wsdlMessage;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00012F2C File Offset: 0x00011F2C
		private void ParseWsdlPortType()
		{
			WsdlParser.WsdlPortType wsdlPortType = new WsdlParser.WsdlPortType();
			wsdlPortType.name = this.LookupAttribute(WsdlParser.s_nameString, null, true);
			int depth = this._XMLReader.Depth;
			this.ReadNextXmlElement();
			while (this._XMLReader.Depth > depth)
			{
				string localName = this._XMLReader.LocalName;
				if (WsdlParser.MatchingStrings(localName, WsdlParser.s_operationString))
				{
					WsdlParser.WsdlPortTypeOperation wsdlPortTypeOperation = new WsdlParser.WsdlPortTypeOperation();
					wsdlPortTypeOperation.name = this.LookupAttribute(WsdlParser.s_nameString, null, true);
					wsdlPortTypeOperation.nameNs = this.ParseQName(ref wsdlPortTypeOperation.nameNs);
					wsdlPortTypeOperation.parameterOrder = this.LookupAttribute(WsdlParser.s_parameterOrderString, null, false);
					this.ParseWsdlPortTypeOperationContent(wsdlPortType, wsdlPortTypeOperation);
					wsdlPortType.operations.Add(wsdlPortTypeOperation);
				}
				else
				{
					this.SkipXmlElement();
				}
			}
			this.wsdlPortTypes[wsdlPortType.name] = wsdlPortType;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00013000 File Offset: 0x00012000
		private void ParseWsdlPortTypeOperationContent(WsdlParser.WsdlPortType portType, WsdlParser.WsdlPortTypeOperation portTypeOperation)
		{
			int depth = this._XMLReader.Depth;
			this.ReadNextXmlElement();
			while (this._XMLReader.Depth > depth)
			{
				string localName = this._XMLReader.LocalName;
				if (WsdlParser.MatchingStrings(localName, WsdlParser.s_inputString))
				{
					WsdlParser.WsdlPortTypeOperationContent wsdlPortTypeOperationContent = new WsdlParser.WsdlPortTypeOperationContent();
					wsdlPortTypeOperationContent.element = this.Atomize("input");
					wsdlPortTypeOperationContent.name = this.LookupAttribute(WsdlParser.s_nameString, null, false);
					if (WsdlParser.MatchingStrings(wsdlPortTypeOperationContent.name, WsdlParser.s_emptyString))
					{
						wsdlPortTypeOperationContent.name = this.Atomize(portTypeOperation.name + "Request");
						if (portType.sections.ContainsKey(wsdlPortTypeOperationContent.name))
						{
							throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_DuplicatePortTypesOperationName"), new object[] { portTypeOperation.name }));
						}
						portType.sections[wsdlPortTypeOperationContent.name] = portTypeOperation;
						portType.sections[portTypeOperation.name] = portTypeOperation;
					}
					else
					{
						if (portType.sections.ContainsKey(wsdlPortTypeOperationContent.name))
						{
							throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_DuplicatePortSectionName"), new object[] { wsdlPortTypeOperationContent.name }));
						}
						portType.sections[wsdlPortTypeOperationContent.name] = portTypeOperation;
					}
					wsdlPortTypeOperationContent.message = this.LookupAttribute(WsdlParser.s_messageString, null, true);
					wsdlPortTypeOperationContent.messageNs = this.ParseQName(ref wsdlPortTypeOperationContent.message);
					portTypeOperation.contents.Add(wsdlPortTypeOperationContent);
					this.ReadNextXmlElement();
				}
				else if (WsdlParser.MatchingStrings(localName, WsdlParser.s_outputString))
				{
					WsdlParser.WsdlPortTypeOperationContent wsdlPortTypeOperationContent2 = new WsdlParser.WsdlPortTypeOperationContent();
					wsdlPortTypeOperationContent2.element = this.Atomize("output");
					wsdlPortTypeOperationContent2.name = this.LookupAttribute(WsdlParser.s_nameString, null, false);
					wsdlPortTypeOperationContent2.nameNs = this.ParseQName(ref wsdlPortTypeOperationContent2.name);
					if (WsdlParser.MatchingStrings(wsdlPortTypeOperationContent2.name, WsdlParser.s_emptyString))
					{
						wsdlPortTypeOperationContent2.name = this.Atomize(portTypeOperation.name + "Response");
					}
					if (!portType.sections.ContainsKey(wsdlPortTypeOperationContent2.name))
					{
						portType.sections[wsdlPortTypeOperationContent2.name] = portTypeOperation;
					}
					wsdlPortTypeOperationContent2.message = this.LookupAttribute(WsdlParser.s_messageString, null, true);
					wsdlPortTypeOperationContent2.messageNs = this.ParseQName(ref wsdlPortTypeOperationContent2.message);
					portTypeOperation.contents.Add(wsdlPortTypeOperationContent2);
					this.ReadNextXmlElement();
				}
				else
				{
					this.SkipXmlElement();
				}
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00013288 File Offset: 0x00012288
		private void ParseWsdlBinding(WsdlParser.URTNamespace inparsingNamespace)
		{
			WsdlParser.WsdlBinding wsdlBinding = new WsdlParser.WsdlBinding();
			wsdlBinding.name = this.LookupAttribute(WsdlParser.s_nameString, null, true);
			wsdlBinding.type = this.LookupAttribute(WsdlParser.s_typeString, null, true);
			wsdlBinding.typeNs = this.ParseQName(ref wsdlBinding.type);
			WsdlParser.URTNamespace urtnamespace = this.LookupNamespace(wsdlBinding.typeNs);
			if (urtnamespace == null)
			{
				urtnamespace = new WsdlParser.URTNamespace(wsdlBinding.typeNs, this);
			}
			wsdlBinding.parsingNamespace = urtnamespace;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			int depth = this._XMLReader.Depth;
			this.ReadNextXmlElement();
			while (this._XMLReader.Depth > depth)
			{
				string localName = this._XMLReader.LocalName;
				if (this.MatchingNamespace(WsdlParser.s_wsdlSoapNamespaceString) && WsdlParser.MatchingStrings(localName, WsdlParser.s_bindingString))
				{
					flag = true;
					WsdlParser.WsdlBindingSoapBinding wsdlBindingSoapBinding = new WsdlParser.WsdlBindingSoapBinding();
					wsdlBindingSoapBinding.style = this.LookupAttribute(WsdlParser.s_styleString, null, true);
					if (wsdlBindingSoapBinding.style == "rpc")
					{
						flag2 = true;
					}
					wsdlBindingSoapBinding.transport = this.LookupAttribute(WsdlParser.s_transportString, null, true);
					wsdlBinding.soapBinding = wsdlBindingSoapBinding;
					this.ReadNextXmlElement();
				}
				else
				{
					if (this.MatchingNamespace(WsdlParser.s_wsdlSudsNamespaceString))
					{
						flag4 = true;
						if (WsdlParser.MatchingStrings(localName, WsdlParser.s_classString) || WsdlParser.MatchingStrings(localName, WsdlParser.s_structString))
						{
							WsdlParser.WsdlBindingSuds wsdlBindingSuds = new WsdlParser.WsdlBindingSuds();
							wsdlBindingSuds.elementName = localName;
							wsdlBindingSuds.typeName = this.LookupAttribute(WsdlParser.s_typeString, null, true);
							wsdlBindingSuds.ns = this.ParseQName(ref wsdlBindingSuds.typeName);
							wsdlBindingSuds.extendsTypeName = this.LookupAttribute(WsdlParser.s_extendsString, null, false);
							string text = this.LookupAttribute(WsdlParser.s_rootTypeString, null, false);
							wsdlBindingSuds.sudsUse = this.ProcessSudsUse(text, localName);
							if (!WsdlParser.MatchingStrings(wsdlBindingSuds.extendsTypeName, WsdlParser.s_emptyString))
							{
								wsdlBindingSuds.extendsNs = this.ParseQName(ref wsdlBindingSuds.extendsTypeName);
							}
							this.ParseWsdlBindingSuds(wsdlBindingSuds);
							wsdlBinding.suds.Add(wsdlBindingSuds);
							continue;
						}
						if (WsdlParser.MatchingStrings(localName, WsdlParser.s_interfaceString))
						{
							WsdlParser.WsdlBindingSuds wsdlBindingSuds2 = new WsdlParser.WsdlBindingSuds();
							wsdlBindingSuds2.elementName = localName;
							wsdlBindingSuds2.typeName = this.LookupAttribute(WsdlParser.s_typeString, null, true);
							wsdlBindingSuds2.ns = this.ParseQName(ref wsdlBindingSuds2.typeName);
							string text2 = this.LookupAttribute(WsdlParser.s_rootTypeString, null, false);
							wsdlBindingSuds2.sudsUse = this.ProcessSudsUse(text2, localName);
							this.ParseWsdlBindingSuds(wsdlBindingSuds2);
							wsdlBinding.suds.Add(wsdlBindingSuds2);
							continue;
						}
					}
					else if (this.MatchingNamespace(WsdlParser.s_wsdlNamespaceString) && WsdlParser.MatchingStrings(localName, WsdlParser.s_operationString))
					{
						WsdlParser.WsdlBindingOperation wsdlBindingOperation = new WsdlParser.WsdlBindingOperation();
						wsdlBindingOperation.name = this.LookupAttribute(WsdlParser.s_nameString, null, true);
						wsdlBindingOperation.nameNs = this._parsingInput.TargetNS;
						this.ParseWsdlBindingOperation(wsdlBindingOperation, ref flag2, ref flag3);
						wsdlBinding.operations.Add(wsdlBindingOperation);
						continue;
					}
					this.SkipXmlElement();
				}
			}
			if ((flag && flag2 && flag3) || flag4)
			{
				this.wsdlBindings.Add(wsdlBinding);
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00013598 File Offset: 0x00012598
		private void ParseWsdlBindingSuds(WsdlParser.WsdlBindingSuds suds)
		{
			int depth = this._XMLReader.Depth;
			this.ReadNextXmlElement();
			while (this._XMLReader.Depth > depth)
			{
				string localName = this._XMLReader.LocalName;
				if (WsdlParser.MatchingStrings(localName, WsdlParser.s_implementsString) || WsdlParser.MatchingStrings(localName, WsdlParser.s_extendsString))
				{
					WsdlParser.WsdlBindingSudsImplements wsdlBindingSudsImplements = new WsdlParser.WsdlBindingSudsImplements();
					wsdlBindingSudsImplements.typeName = this.LookupAttribute(WsdlParser.s_typeString, null, true);
					wsdlBindingSudsImplements.ns = this.ParseQName(ref wsdlBindingSudsImplements.typeName);
					suds.implements.Add(wsdlBindingSudsImplements);
					this.ReadNextXmlElement();
				}
				else if (WsdlParser.MatchingStrings(localName, WsdlParser.s_nestedTypeString))
				{
					WsdlParser.WsdlBindingSudsNestedType wsdlBindingSudsNestedType = new WsdlParser.WsdlBindingSudsNestedType();
					wsdlBindingSudsNestedType.name = this.LookupAttribute(WsdlParser.s_nameString, null, true);
					wsdlBindingSudsNestedType.typeName = this.LookupAttribute(WsdlParser.s_typeString, null, true);
					wsdlBindingSudsNestedType.ns = this.ParseQName(ref wsdlBindingSudsNestedType.typeName);
					suds.nestedTypes.Add(wsdlBindingSudsNestedType);
					this.ReadNextXmlElement();
				}
				else
				{
					this.SkipXmlElement();
				}
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x000136A0 File Offset: 0x000126A0
		private WsdlParser.SudsUse ProcessSudsUse(string use, string elementName)
		{
			WsdlParser.SudsUse sudsUse = WsdlParser.SudsUse.Class;
			if (use == null || use.Length == 0)
			{
				use = elementName;
			}
			if (WsdlParser.MatchingStrings(use, WsdlParser.s_interfaceString))
			{
				sudsUse = WsdlParser.SudsUse.Interface;
			}
			else if (WsdlParser.MatchingStrings(use, WsdlParser.s_classString))
			{
				sudsUse = WsdlParser.SudsUse.Class;
			}
			else if (WsdlParser.MatchingStrings(use, WsdlParser.s_structString))
			{
				sudsUse = WsdlParser.SudsUse.Struct;
			}
			else if (WsdlParser.MatchingStrings(use, WsdlParser.s_ISerializableString))
			{
				sudsUse = WsdlParser.SudsUse.ISerializable;
			}
			else if (WsdlParser.MatchingStrings(use, WsdlParser.s_marshalByRefString))
			{
				sudsUse = WsdlParser.SudsUse.MarshalByRef;
			}
			else if (WsdlParser.MatchingStrings(use, WsdlParser.s_delegateString))
			{
				sudsUse = WsdlParser.SudsUse.Delegate;
			}
			else if (WsdlParser.MatchingStrings(use, WsdlParser.s_servicedComponentString))
			{
				sudsUse = WsdlParser.SudsUse.ServicedComponent;
			}
			return sudsUse;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00013734 File Offset: 0x00012734
		private void ParseWsdlBindingOperation(WsdlParser.WsdlBindingOperation op, ref bool bRpcBinding, ref bool bSoapEncoded)
		{
			int depth = this._XMLReader.Depth;
			bool flag = false;
			bool flag2 = false;
			WsdlParser.WsdlBindingOperationSection wsdlBindingOperationSection = null;
			this.ReadNextXmlElement();
			while (this._XMLReader.Depth > depth)
			{
				string localName = this._XMLReader.LocalName;
				if (this.MatchingNamespace(WsdlParser.s_wsdlSudsNamespaceString) && WsdlParser.MatchingStrings(localName, WsdlParser.s_methodString))
				{
					op.methodAttributes = this.LookupAttribute(WsdlParser.s_attributesString, null, true);
					this.ReadNextXmlElement();
				}
				else if (this.MatchingNamespace(WsdlParser.s_wsdlSoapNamespaceString) && WsdlParser.MatchingStrings(localName, WsdlParser.s_operationString))
				{
					WsdlParser.WsdlBindingSoapOperation wsdlBindingSoapOperation = new WsdlParser.WsdlBindingSoapOperation();
					wsdlBindingSoapOperation.soapAction = this.LookupAttribute(WsdlParser.s_soapActionString, null, false);
					wsdlBindingSoapOperation.style = this.LookupAttribute(WsdlParser.s_styleString, null, false);
					if (wsdlBindingSoapOperation.style == "rpc")
					{
						bRpcBinding = true;
					}
					op.soapOperation = wsdlBindingSoapOperation;
					this.ReadNextXmlElement();
				}
				else
				{
					if (this.MatchingNamespace(WsdlParser.s_wsdlNamespaceString))
					{
						if (WsdlParser.MatchingStrings(localName, WsdlParser.s_inputString))
						{
							flag = true;
							wsdlBindingOperationSection = this.ParseWsdlBindingOperationSection(op, localName, ref bSoapEncoded);
							continue;
						}
						if (WsdlParser.MatchingStrings(localName, WsdlParser.s_outputString))
						{
							flag2 = true;
							this.ParseWsdlBindingOperationSection(op, localName, ref bSoapEncoded);
							continue;
						}
						if (WsdlParser.MatchingStrings(localName, WsdlParser.s_faultString))
						{
							this.ParseWsdlBindingOperationSection(op, localName, ref bSoapEncoded);
							continue;
						}
					}
					this.SkipXmlElement();
				}
			}
			if (wsdlBindingOperationSection != null && flag && !flag2)
			{
				wsdlBindingOperationSection.name = op.name;
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000138A8 File Offset: 0x000128A8
		private WsdlParser.WsdlBindingOperationSection ParseWsdlBindingOperationSection(WsdlParser.WsdlBindingOperation op, string inputElementName, ref bool bSoapEncoded)
		{
			bool flag = false;
			WsdlParser.WsdlBindingOperationSection wsdlBindingOperationSection = new WsdlParser.WsdlBindingOperationSection();
			op.sections.Add(wsdlBindingOperationSection);
			wsdlBindingOperationSection.name = this.LookupAttribute(WsdlParser.s_nameString, null, false);
			if (WsdlParser.MatchingStrings(wsdlBindingOperationSection.name, WsdlParser.s_emptyString))
			{
				if (WsdlParser.MatchingStrings(inputElementName, WsdlParser.s_inputString))
				{
					flag = true;
					wsdlBindingOperationSection.name = this.Atomize(op.name + "Request");
				}
				else if (WsdlParser.MatchingStrings(inputElementName, WsdlParser.s_outputString))
				{
					wsdlBindingOperationSection.name = this.Atomize(op.name + "Response");
				}
			}
			wsdlBindingOperationSection.elementName = inputElementName;
			int depth = this._XMLReader.Depth;
			this.ReadNextXmlElement();
			while (this._XMLReader.Depth > depth)
			{
				string localName = this._XMLReader.LocalName;
				if (this.MatchingNamespace(WsdlParser.s_wsdlSoapNamespaceString))
				{
					if (WsdlParser.MatchingStrings(localName, WsdlParser.s_bodyString))
					{
						WsdlParser.WsdlBindingSoapBody wsdlBindingSoapBody = new WsdlParser.WsdlBindingSoapBody();
						wsdlBindingOperationSection.extensions.Add(wsdlBindingSoapBody);
						wsdlBindingSoapBody.parts = this.LookupAttribute(WsdlParser.s_partsString, null, false);
						wsdlBindingSoapBody.use = this.LookupAttribute(WsdlParser.s_useString, null, true);
						if (wsdlBindingSoapBody.use == "encoded")
						{
							bSoapEncoded = true;
						}
						wsdlBindingSoapBody.encodingStyle = this.LookupAttribute(WsdlParser.s_encodingStyleString, null, false);
						wsdlBindingSoapBody.namespaceUri = this.LookupAttribute(WsdlParser.s_namespaceString, null, false);
						this.ReadNextXmlElement();
						continue;
					}
					if (WsdlParser.MatchingStrings(localName, WsdlParser.s_headerString))
					{
						WsdlParser.WsdlBindingSoapHeader wsdlBindingSoapHeader = new WsdlParser.WsdlBindingSoapHeader();
						wsdlBindingOperationSection.extensions.Add(wsdlBindingSoapHeader);
						wsdlBindingSoapHeader.message = this.LookupAttribute(WsdlParser.s_messageString, null, true);
						wsdlBindingSoapHeader.messageNs = this.ParseQName(ref wsdlBindingSoapHeader.message);
						wsdlBindingSoapHeader.part = this.LookupAttribute(WsdlParser.s_partString, null, true);
						wsdlBindingSoapHeader.use = this.LookupAttribute(WsdlParser.s_useString, null, true);
						wsdlBindingSoapHeader.encodingStyle = this.LookupAttribute(WsdlParser.s_encodingStyleString, null, false);
						wsdlBindingSoapHeader.namespaceUri = this.LookupAttribute(WsdlParser.s_namespaceString, null, false);
						this.ReadNextXmlElement();
						continue;
					}
					if (WsdlParser.MatchingStrings(localName, WsdlParser.s_faultString))
					{
						WsdlParser.WsdlBindingSoapFault wsdlBindingSoapFault = new WsdlParser.WsdlBindingSoapFault();
						wsdlBindingOperationSection.extensions.Add(wsdlBindingSoapFault);
						wsdlBindingSoapFault.name = this.LookupAttribute(WsdlParser.s_nameString, null, true);
						wsdlBindingSoapFault.use = this.LookupAttribute(WsdlParser.s_useString, null, true);
						wsdlBindingSoapFault.encodingStyle = this.LookupAttribute(WsdlParser.s_encodingStyleString, null, false);
						wsdlBindingSoapFault.namespaceUri = this.LookupAttribute(WsdlParser.s_namespaceString, null, false);
						this.ReadNextXmlElement();
						continue;
					}
				}
				this.SkipXmlElement();
			}
			if (flag)
			{
				return wsdlBindingOperationSection;
			}
			return null;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00013B5C File Offset: 0x00012B5C
		private void ParseWsdlService()
		{
			WsdlParser.WsdlService wsdlService = new WsdlParser.WsdlService();
			wsdlService.name = this.LookupAttribute(WsdlParser.s_nameString, null, true);
			int depth = this._XMLReader.Depth;
			this.ReadNextXmlElement();
			while (this._XMLReader.Depth > depth)
			{
				string localName = this._XMLReader.LocalName;
				if (this.MatchingNamespace(WsdlParser.s_wsdlNamespaceString) && WsdlParser.MatchingStrings(localName, WsdlParser.s_portString))
				{
					WsdlParser.WsdlServicePort wsdlServicePort = new WsdlParser.WsdlServicePort();
					wsdlServicePort.name = this.LookupAttribute(WsdlParser.s_nameString, null, true);
					wsdlServicePort.nameNs = this.ParseQName(ref wsdlServicePort.nameNs);
					wsdlServicePort.binding = this.LookupAttribute(WsdlParser.s_bindingString, null, true);
					wsdlServicePort.bindingNs = this.ParseQName(ref wsdlServicePort.binding);
					this.ParseWsdlServicePort(wsdlServicePort);
					wsdlService.ports[wsdlServicePort.binding] = wsdlServicePort;
				}
				else
				{
					this.SkipXmlElement();
				}
			}
			this.wsdlServices.Add(wsdlService);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00013C50 File Offset: 0x00012C50
		private void ParseWsdlServicePort(WsdlParser.WsdlServicePort port)
		{
			int depth = this._XMLReader.Depth;
			this.ReadNextXmlElement();
			while (this._XMLReader.Depth > depth)
			{
				string localName = this._XMLReader.LocalName;
				if (this.MatchingNamespace(WsdlParser.s_wsdlSoapNamespaceString) && WsdlParser.MatchingStrings(localName, WsdlParser.s_addressString))
				{
					if (port.locations == null)
					{
						port.locations = new ArrayList(10);
					}
					port.locations.Add(this.LookupAttribute(WsdlParser.s_locationString, null, true));
					this.ReadNextXmlElement();
				}
				else
				{
					this.SkipXmlElement();
				}
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00013CE4 File Offset: 0x00012CE4
		private void ResolveWsdl()
		{
			if (this.wsdlBindings.Count == 0)
			{
				throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_RpcBindingsMissing"), new object[0]));
			}
			foreach (object obj in this.wsdlBindings)
			{
				WsdlParser.WsdlBinding wsdlBinding = (WsdlParser.WsdlBinding)obj;
				if (wsdlBinding.soapBinding != null)
				{
					if (wsdlBinding.suds != null && wsdlBinding.suds.Count > 0)
					{
						bool flag = true;
						using (IEnumerator enumerator2 = wsdlBinding.suds.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								object obj2 = enumerator2.Current;
								WsdlParser.WsdlBindingSuds wsdlBindingSuds = (WsdlParser.WsdlBindingSuds)obj2;
								if (WsdlParser.MatchingStrings(wsdlBindingSuds.elementName, WsdlParser.s_classString) || WsdlParser.MatchingStrings(wsdlBindingSuds.elementName, WsdlParser.s_structString))
								{
									this.ResolveWsdlClass(wsdlBinding, wsdlBindingSuds, flag);
									flag = false;
								}
								else
								{
									if (!WsdlParser.MatchingStrings(wsdlBindingSuds.elementName, WsdlParser.s_interfaceString))
									{
										throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_CantResolveElementInNS"), new object[]
										{
											wsdlBindingSuds.elementName,
											WsdlParser.s_wsdlSudsNamespaceString
										}));
									}
									this.ResolveWsdlInterface(wsdlBinding, wsdlBindingSuds);
								}
							}
							continue;
						}
					}
					this.ResolveWsdlClass(wsdlBinding, null, true);
				}
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00013E88 File Offset: 0x00012E88
		private void ResolveWsdlClass(WsdlParser.WsdlBinding binding, WsdlParser.WsdlBindingSuds suds, bool bFirstSuds)
		{
			WsdlParser.URTNamespace urtnamespace;
			WsdlParser.URTComplexType urtcomplexType;
			if (suds != null)
			{
				urtnamespace = this.AddNewNamespace(suds.ns);
				urtcomplexType = urtnamespace.LookupComplexType(suds.typeName);
				if (urtcomplexType == null)
				{
					urtcomplexType = new WsdlParser.URTComplexType(suds.typeName, urtnamespace.Name, urtnamespace.Namespace, urtnamespace.EncodedNS, this._blockDefault, false, false, this, urtnamespace);
					urtnamespace.AddComplexType(urtcomplexType);
				}
				if (WsdlParser.MatchingStrings(suds.elementName, WsdlParser.s_structString))
				{
					urtcomplexType.IsValueType = true;
				}
				urtcomplexType.SudsUse = suds.sudsUse;
				if (suds.sudsUse == WsdlParser.SudsUse.MarshalByRef || suds.sudsUse == WsdlParser.SudsUse.ServicedComponent)
				{
					urtcomplexType.IsSUDSType = true;
					if (this._bWrappedProxy)
					{
						urtcomplexType.SUDSType = SUDSType.ClientProxy;
					}
					else
					{
						urtcomplexType.SUDSType = SUDSType.MarshalByRef;
					}
					if (suds.extendsTypeName != null && suds.extendsTypeName.Length > 0)
					{
						WsdlParser.URTNamespace urtnamespace2 = this.AddNewNamespace(suds.extendsNs);
						WsdlParser.URTComplexType urtcomplexType2 = urtnamespace2.LookupComplexType(suds.extendsTypeName);
						if (urtcomplexType2 == null)
						{
							urtcomplexType2 = new WsdlParser.URTComplexType(suds.extendsTypeName, urtnamespace2.Name, urtnamespace2.Namespace, urtnamespace2.EncodedNS, this._blockDefault, true, false, this, urtnamespace2);
							urtnamespace2.AddComplexType(urtcomplexType2);
						}
						else
						{
							urtcomplexType2.IsSUDSType = true;
						}
						if (this._bWrappedProxy)
						{
							urtcomplexType2.SUDSType = SUDSType.ClientProxy;
						}
						else
						{
							urtcomplexType2.SUDSType = SUDSType.MarshalByRef;
						}
						urtcomplexType2.SudsUse = suds.sudsUse;
					}
				}
				using (IEnumerator enumerator = suds.nestedTypes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						WsdlParser.WsdlBindingSudsNestedType wsdlBindingSudsNestedType = (WsdlParser.WsdlBindingSudsNestedType)obj;
						this.ResolveWsdlNestedType(binding, suds, wsdlBindingSudsNestedType);
					}
					goto IL_0222;
				}
			}
			urtnamespace = this.AddNewNamespace(binding.typeNs);
			string text = binding.name;
			int num = binding.name.IndexOf("Binding");
			if (num > 0)
			{
				text = binding.name.Substring(0, num);
			}
			urtcomplexType = urtnamespace.LookupComplexTypeEqual(text);
			if (urtcomplexType == null)
			{
				urtcomplexType = new WsdlParser.URTComplexType(text, urtnamespace.Name, urtnamespace.Namespace, urtnamespace.EncodedNS, this._blockDefault, true, false, this, urtnamespace);
				urtnamespace.AddComplexType(urtcomplexType);
			}
			else
			{
				urtcomplexType.IsSUDSType = true;
			}
			if (this._bWrappedProxy)
			{
				urtcomplexType.SUDSType = SUDSType.ClientProxy;
			}
			else
			{
				urtcomplexType.SUDSType = SUDSType.MarshalByRef;
			}
			urtcomplexType.SudsUse = WsdlParser.SudsUse.MarshalByRef;
			IL_0222:
			urtcomplexType.ConnectURLs = this.ResolveWsdlAddress(binding);
			if (suds != null)
			{
				if (!WsdlParser.MatchingStrings(suds.extendsTypeName, WsdlParser.s_emptyString))
				{
					urtcomplexType.Extends(suds.extendsTypeName, suds.extendsNs);
				}
				foreach (object obj2 in suds.implements)
				{
					WsdlParser.WsdlBindingSudsImplements wsdlBindingSudsImplements = (WsdlParser.WsdlBindingSudsImplements)obj2;
					urtcomplexType.Implements(wsdlBindingSudsImplements.typeName, wsdlBindingSudsImplements.ns, this);
				}
			}
			if (bFirstSuds && (urtcomplexType.SudsUse == WsdlParser.SudsUse.MarshalByRef || urtcomplexType.SudsUse == WsdlParser.SudsUse.ServicedComponent || urtcomplexType.SudsUse == WsdlParser.SudsUse.Delegate || urtcomplexType.SudsUse == WsdlParser.SudsUse.Interface))
			{
				ArrayList arrayList = this.ResolveWsdlMethodInfo(binding);
				foreach (object obj3 in arrayList)
				{
					WsdlParser.WsdlMethodInfo wsdlMethodInfo = (WsdlParser.WsdlMethodInfo)obj3;
					if (wsdlMethodInfo.inputMethodName != null && wsdlMethodInfo.outputMethodName != null)
					{
						WsdlParser.RRMethod rrmethod = new WsdlParser.RRMethod(wsdlMethodInfo, urtcomplexType);
						rrmethod.AddRequest(wsdlMethodInfo.methodName, wsdlMethodInfo.methodNameNs);
						rrmethod.AddResponse(wsdlMethodInfo.methodName, wsdlMethodInfo.methodNameNs);
						urtcomplexType.AddMethod(rrmethod);
					}
					else
					{
						if (wsdlMethodInfo.inputMethodName == null)
						{
							throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_WsdlInvalidMessage"), new object[] { wsdlMethodInfo.methodName }));
						}
						WsdlParser.OnewayMethod onewayMethod = new WsdlParser.OnewayMethod(wsdlMethodInfo, urtcomplexType);
						urtcomplexType.AddMethod(onewayMethod);
						onewayMethod.AddMessage(wsdlMethodInfo.methodName, wsdlMethodInfo.methodNameNs);
					}
				}
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0001428C File Offset: 0x0001328C
		private void ResolveWsdlInterface(WsdlParser.WsdlBinding binding, WsdlParser.WsdlBindingSuds suds)
		{
			WsdlParser.URTNamespace parsingNamespace = binding.parsingNamespace;
			WsdlParser.URTNamespace urtnamespace = this.AddNewNamespace(suds.ns);
			WsdlParser.URTInterface urtinterface = urtnamespace.LookupInterface(suds.typeName);
			if (urtinterface == null)
			{
				urtinterface = new WsdlParser.URTInterface(suds.typeName, urtnamespace.Name, urtnamespace.Namespace, urtnamespace.EncodedNS, this);
				urtnamespace.AddInterface(urtinterface);
			}
			if (suds.extendsTypeName != null)
			{
				urtinterface.Extends(suds.extendsTypeName, suds.extendsNs, this);
			}
			foreach (object obj in suds.implements)
			{
				WsdlParser.WsdlBindingSudsImplements wsdlBindingSudsImplements = (WsdlParser.WsdlBindingSudsImplements)obj;
				urtinterface.Extends(wsdlBindingSudsImplements.typeName, wsdlBindingSudsImplements.ns, this);
			}
			ArrayList arrayList = this.ResolveWsdlMethodInfo(binding);
			foreach (object obj2 in arrayList)
			{
				WsdlParser.WsdlMethodInfo wsdlMethodInfo = (WsdlParser.WsdlMethodInfo)obj2;
				if (wsdlMethodInfo.inputMethodName != null && wsdlMethodInfo.outputMethodName != null)
				{
					WsdlParser.RRMethod rrmethod = new WsdlParser.RRMethod(wsdlMethodInfo, null);
					rrmethod.AddRequest(wsdlMethodInfo.methodName, wsdlMethodInfo.methodNameNs);
					rrmethod.AddResponse(wsdlMethodInfo.methodName, wsdlMethodInfo.methodNameNs);
					urtinterface.AddMethod(rrmethod);
				}
				else
				{
					if (wsdlMethodInfo.inputMethodName == null)
					{
						throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_WsdlInvalidMessage"), new object[] { wsdlMethodInfo.methodName }));
					}
					WsdlParser.OnewayMethod onewayMethod = new WsdlParser.OnewayMethod(wsdlMethodInfo.methodName, wsdlMethodInfo.soapAction, null);
					onewayMethod.AddMessage(wsdlMethodInfo.methodName, wsdlMethodInfo.methodNameNs);
					urtinterface.AddMethod(onewayMethod);
				}
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0001446C File Offset: 0x0001346C
		private void ResolveWsdlNestedType(WsdlParser.WsdlBinding binding, WsdlParser.WsdlBindingSuds suds, WsdlParser.WsdlBindingSudsNestedType nested)
		{
			string typeName = suds.typeName;
			string ns = nested.ns;
			string name = nested.name;
			string typeName2 = nested.typeName;
			if (suds.ns != ns)
			{
				throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_CantResolveNestedTypeNS"), new object[] { suds.typeName, suds.ns }));
			}
			WsdlParser.URTNamespace urtnamespace = this.AddNewNamespace(suds.ns);
			WsdlParser.URTComplexType urtcomplexType = urtnamespace.LookupComplexType(suds.typeName);
			if (urtcomplexType == null)
			{
				throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_CantResolveNestedType"), new object[] { suds.typeName, suds.ns }));
			}
			WsdlParser.BaseType baseType = urtnamespace.LookupType(nested.typeName);
			if (baseType == null)
			{
				baseType = urtnamespace.LookupComplexType(nested.typeName);
				if (baseType == null)
				{
					baseType = new WsdlParser.URTComplexType(nested.typeName, urtnamespace.Name, urtnamespace.Namespace, urtnamespace.EncodedNS, this._blockDefault, false, false, this, urtnamespace);
					urtnamespace.AddComplexType((WsdlParser.URTComplexType)baseType);
				}
			}
			baseType.bNestedType = true;
			baseType.NestedTypeName = nested.name;
			baseType.FullNestedTypeName = nested.typeName;
			baseType.OuterTypeName = suds.typeName;
			urtcomplexType.AddNestedType(baseType);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x000145B8 File Offset: 0x000135B8
		private ArrayList ResolveWsdlAddress(WsdlParser.WsdlBinding binding)
		{
			ArrayList arrayList = null;
			if (this._bWrappedProxy)
			{
				foreach (object obj in this.wsdlServices)
				{
					WsdlParser.WsdlService wsdlService = (WsdlParser.WsdlService)obj;
					WsdlParser.WsdlServicePort wsdlServicePort = (WsdlParser.WsdlServicePort)wsdlService.ports[binding.name];
					if (wsdlServicePort != null)
					{
						arrayList = wsdlServicePort.locations;
						break;
					}
					if (arrayList != null)
					{
						break;
					}
				}
			}
			return arrayList;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00014640 File Offset: 0x00013640
		private ArrayList ResolveWsdlMethodInfo(WsdlParser.WsdlBinding binding)
		{
			ArrayList arrayList = new ArrayList(10);
			Hashtable hashtable = new Hashtable(3);
			for (int i = 0; i < binding.operations.Count; i++)
			{
				bool flag = false;
				bool flag2 = false;
				WsdlParser.WsdlBindingOperation wsdlBindingOperation = (WsdlParser.WsdlBindingOperation)binding.operations[i];
				if (wsdlBindingOperation.soapOperation != null)
				{
					WsdlParser.WsdlMethodInfo wsdlMethodInfo = new WsdlParser.WsdlMethodInfo();
					wsdlMethodInfo.methodName = wsdlBindingOperation.name;
					wsdlMethodInfo.methodNameNs = wsdlBindingOperation.nameNs;
					wsdlMethodInfo.methodAttributes = wsdlBindingOperation.methodAttributes;
					this.AddNewNamespace(wsdlBindingOperation.nameNs);
					WsdlParser.WsdlBindingSoapOperation soapOperation = wsdlBindingOperation.soapOperation;
					if (wsdlMethodInfo.methodName.StartsWith("get_", StringComparison.Ordinal) && wsdlMethodInfo.methodName.Length > 4)
					{
						flag = true;
					}
					else if (wsdlMethodInfo.methodName.StartsWith("set_", StringComparison.Ordinal) && wsdlMethodInfo.methodName.Length > 4)
					{
						flag2 = true;
					}
					if (flag || flag2)
					{
						bool flag3 = false;
						string text = wsdlMethodInfo.methodName.Substring(4);
						WsdlParser.WsdlMethodInfo wsdlMethodInfo2 = (WsdlParser.WsdlMethodInfo)hashtable[text];
						if (wsdlMethodInfo2 == null)
						{
							hashtable[text] = wsdlMethodInfo;
							arrayList.Add(wsdlMethodInfo);
							wsdlMethodInfo2 = wsdlMethodInfo;
							wsdlMethodInfo.propertyName = text;
							wsdlMethodInfo.bProperty = true;
							flag3 = true;
						}
						if (flag)
						{
							wsdlMethodInfo2.bGet = true;
							wsdlMethodInfo2.soapActionGet = soapOperation.soapAction;
						}
						else
						{
							wsdlMethodInfo2.bSet = true;
							wsdlMethodInfo2.soapActionSet = soapOperation.soapAction;
						}
						if (!flag3)
						{
							goto IL_0959;
						}
					}
					else
					{
						arrayList.Add(wsdlMethodInfo);
					}
					wsdlMethodInfo.soapAction = soapOperation.soapAction;
					WsdlParser.WsdlPortType wsdlPortType = (WsdlParser.WsdlPortType)this.wsdlPortTypes[binding.type];
					if (wsdlPortType == null || wsdlPortType.operations.Count != binding.operations.Count)
					{
						throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_WsdlInvalidPortType"), new object[] { binding.type }));
					}
					WsdlParser.WsdlPortTypeOperation wsdlPortTypeOperation = null;
					foreach (object obj in wsdlBindingOperation.sections)
					{
						WsdlParser.WsdlBindingOperationSection wsdlBindingOperationSection = (WsdlParser.WsdlBindingOperationSection)obj;
						if (WsdlParser.MatchingStrings(wsdlBindingOperationSection.elementName, WsdlParser.s_inputString))
						{
							wsdlPortTypeOperation = (WsdlParser.WsdlPortTypeOperation)wsdlPortType.sections[wsdlBindingOperationSection.name];
							if (wsdlPortTypeOperation == null)
							{
								int num = wsdlBindingOperationSection.name.LastIndexOf("Request");
								if (num > 0)
								{
									string text2 = wsdlBindingOperationSection.name.Substring(0, num);
									wsdlPortTypeOperation = (WsdlParser.WsdlPortTypeOperation)wsdlPortType.sections[text2];
								}
							}
							if (wsdlPortTypeOperation != null && wsdlPortTypeOperation.parameterOrder != null && wsdlPortTypeOperation.parameterOrder.Length > 0)
							{
								wsdlMethodInfo.paramNamesOrder = wsdlPortTypeOperation.parameterOrder.Split(new char[] { ' ' });
							}
							using (IEnumerator enumerator2 = wsdlBindingOperationSection.extensions.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									object obj2 = enumerator2.Current;
									WsdlParser.WsdlBindingSoapBody wsdlBindingSoapBody = (WsdlParser.WsdlBindingSoapBody)obj2;
									if (wsdlBindingSoapBody.namespaceUri != null || wsdlBindingSoapBody.namespaceUri.Length > 0)
									{
										wsdlMethodInfo.inputMethodNameNs = wsdlBindingSoapBody.namespaceUri;
									}
								}
								continue;
							}
						}
						if (WsdlParser.MatchingStrings(wsdlBindingOperationSection.elementName, WsdlParser.s_outputString))
						{
							foreach (object obj3 in wsdlBindingOperationSection.extensions)
							{
								WsdlParser.WsdlBindingSoapBody wsdlBindingSoapBody2 = (WsdlParser.WsdlBindingSoapBody)obj3;
								if (wsdlBindingSoapBody2.namespaceUri != null || wsdlBindingSoapBody2.namespaceUri.Length > 0)
								{
									wsdlMethodInfo.outputMethodNameNs = wsdlBindingSoapBody2.namespaceUri;
								}
							}
						}
					}
					if (wsdlPortTypeOperation != null)
					{
						foreach (object obj4 in wsdlPortTypeOperation.contents)
						{
							WsdlParser.WsdlPortTypeOperationContent wsdlPortTypeOperationContent = (WsdlParser.WsdlPortTypeOperationContent)obj4;
							if (WsdlParser.MatchingStrings(wsdlPortTypeOperationContent.element, WsdlParser.s_inputString))
							{
								wsdlMethodInfo.inputMethodName = wsdlPortTypeOperationContent.message;
								if (wsdlMethodInfo.inputMethodNameNs == null)
								{
									wsdlMethodInfo.inputMethodNameNs = wsdlPortTypeOperationContent.messageNs;
								}
								WsdlParser.WsdlMessage wsdlMessage = (WsdlParser.WsdlMessage)this.wsdlMessages[wsdlPortTypeOperationContent.message];
								if (wsdlMessage == null)
								{
									throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_WsdlMissingMessage"), new object[] { wsdlPortTypeOperationContent.message }));
								}
								if (wsdlMessage.parts != null)
								{
									wsdlMethodInfo.inputNames = new string[wsdlMessage.parts.Count];
									wsdlMethodInfo.inputNamesNs = new string[wsdlMessage.parts.Count];
									wsdlMethodInfo.inputElements = new string[wsdlMessage.parts.Count];
									wsdlMethodInfo.inputElementsNs = new string[wsdlMessage.parts.Count];
									wsdlMethodInfo.inputTypes = new string[wsdlMessage.parts.Count];
									wsdlMethodInfo.inputTypesNs = new string[wsdlMessage.parts.Count];
									for (int j = 0; j < wsdlMessage.parts.Count; j++)
									{
										wsdlMethodInfo.inputNames[j] = ((WsdlParser.WsdlMessagePart)wsdlMessage.parts[j]).name;
										wsdlMethodInfo.inputNamesNs[j] = ((WsdlParser.WsdlMessagePart)wsdlMessage.parts[j]).nameNs;
										this.AddNewNamespace(wsdlMethodInfo.inputNamesNs[j]);
										wsdlMethodInfo.inputElements[j] = ((WsdlParser.WsdlMessagePart)wsdlMessage.parts[j]).element;
										wsdlMethodInfo.inputElementsNs[j] = ((WsdlParser.WsdlMessagePart)wsdlMessage.parts[j]).elementNs;
										this.AddNewNamespace(wsdlMethodInfo.inputElementsNs[j]);
										wsdlMethodInfo.inputTypes[j] = ((WsdlParser.WsdlMessagePart)wsdlMessage.parts[j]).typeName;
										wsdlMethodInfo.inputTypesNs[j] = ((WsdlParser.WsdlMessagePart)wsdlMessage.parts[j]).typeNameNs;
										this.AddNewNamespace(wsdlMethodInfo.inputTypesNs[j]);
										if (wsdlMethodInfo.bProperty && wsdlMethodInfo.inputTypes[j] != null && wsdlMethodInfo.propertyType == null)
										{
											wsdlMethodInfo.propertyType = wsdlMethodInfo.inputTypes[j];
											wsdlMethodInfo.propertyNs = wsdlMethodInfo.inputTypesNs[j];
											this.AddNewNamespace(wsdlMethodInfo.propertyNs);
										}
									}
								}
							}
							else
							{
								if (!WsdlParser.MatchingStrings(wsdlPortTypeOperationContent.element, WsdlParser.s_outputString))
								{
									throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_WsdlInvalidPortType"), new object[] { wsdlPortTypeOperationContent.element }));
								}
								wsdlMethodInfo.outputMethodName = wsdlPortTypeOperationContent.message;
								if (wsdlMethodInfo.outputMethodNameNs == null)
								{
									wsdlMethodInfo.outputMethodNameNs = wsdlPortTypeOperationContent.messageNs;
								}
								WsdlParser.WsdlMessage wsdlMessage2 = (WsdlParser.WsdlMessage)this.wsdlMessages[wsdlPortTypeOperationContent.message];
								if (wsdlMessage2 == null)
								{
									throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_WsdlMissingMessage"), new object[] { wsdlPortTypeOperationContent.message }));
								}
								if (wsdlMessage2.parts != null)
								{
									wsdlMethodInfo.outputNames = new string[wsdlMessage2.parts.Count];
									wsdlMethodInfo.outputNamesNs = new string[wsdlMessage2.parts.Count];
									wsdlMethodInfo.outputElements = new string[wsdlMessage2.parts.Count];
									wsdlMethodInfo.outputElementsNs = new string[wsdlMessage2.parts.Count];
									wsdlMethodInfo.outputTypes = new string[wsdlMessage2.parts.Count];
									wsdlMethodInfo.outputTypesNs = new string[wsdlMessage2.parts.Count];
									for (int k = 0; k < wsdlMessage2.parts.Count; k++)
									{
										wsdlMethodInfo.outputNames[k] = ((WsdlParser.WsdlMessagePart)wsdlMessage2.parts[k]).name;
										wsdlMethodInfo.outputNamesNs[k] = ((WsdlParser.WsdlMessagePart)wsdlMessage2.parts[k]).nameNs;
										this.AddNewNamespace(wsdlMethodInfo.outputNamesNs[k]);
										wsdlMethodInfo.outputElements[k] = ((WsdlParser.WsdlMessagePart)wsdlMessage2.parts[k]).element;
										wsdlMethodInfo.outputElementsNs[k] = ((WsdlParser.WsdlMessagePart)wsdlMessage2.parts[k]).elementNs;
										this.AddNewNamespace(wsdlMethodInfo.outputElementsNs[k]);
										wsdlMethodInfo.outputTypes[k] = ((WsdlParser.WsdlMessagePart)wsdlMessage2.parts[k]).typeName;
										wsdlMethodInfo.outputTypesNs[k] = ((WsdlParser.WsdlMessagePart)wsdlMessage2.parts[k]).typeNameNs;
										this.AddNewNamespace(wsdlMethodInfo.outputTypesNs[k]);
										if (wsdlMethodInfo.bProperty && wsdlMethodInfo.outputTypes[k] != null && wsdlMethodInfo.propertyType == null)
										{
											wsdlMethodInfo.propertyType = wsdlMethodInfo.outputTypes[k];
											wsdlMethodInfo.propertyNs = wsdlMethodInfo.outputTypesNs[k];
											this.AddNewNamespace(wsdlMethodInfo.outputTypesNs[k]);
										}
									}
								}
							}
						}
					}
				}
				IL_0959:;
			}
			return arrayList;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00015020 File Offset: 0x00014020
		private void ParseSchema()
		{
			int depth = this._XMLReader.Depth;
			WsdlParser.URTNamespace urtnamespace = this.ParseNamespace();
			while (this._XMLReader.Depth > depth)
			{
				string localName = this._XMLReader.LocalName;
				if (WsdlParser.MatchingStrings(localName, WsdlParser.s_complexTypeString))
				{
					this.ParseComplexType(urtnamespace, null);
				}
				else if (WsdlParser.MatchingStrings(localName, WsdlParser.s_simpleTypeString))
				{
					this.ParseSimpleType(urtnamespace, null);
				}
				else if (WsdlParser.MatchingStrings(localName, WsdlParser.s_schemaString))
				{
					this.ParseSchema();
				}
				else if (WsdlParser.MatchingStrings(localName, WsdlParser.s_elementString))
				{
					this.ParseElementDecl(urtnamespace);
				}
				else if (WsdlParser.MatchingStrings(localName, WsdlParser.s_importString))
				{
					this.ParseSchemaImportElement();
				}
				else if (WsdlParser.MatchingStrings(localName, WsdlParser.s_includeString))
				{
					this.ParseSchemaIncludeElement();
				}
				else
				{
					this.SkipXmlElement();
				}
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x000150F0 File Offset: 0x000140F0
		private void Resolve()
		{
			for (int i = 0; i < this._URTNamespaces.Count; i++)
			{
				((WsdlParser.URTNamespace)this._URTNamespaces[i]).ResolveElements(this);
			}
			for (int j = 0; j < this._URTNamespaces.Count; j++)
			{
				((WsdlParser.URTNamespace)this._URTNamespaces[j]).ResolveTypes(this);
			}
			for (int k = 0; k < this._URTNamespaces.Count; k++)
			{
				((WsdlParser.URTNamespace)this._URTNamespaces[k]).ResolveMethods();
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00015184 File Offset: 0x00014184
		private string LookupAttribute(string attrName, string attrNS, bool throwExp)
		{
			string text = WsdlParser.s_emptyString;
			bool flag;
			if (attrNS != null)
			{
				flag = this._XMLReader.MoveToAttribute(attrName, attrNS);
			}
			else
			{
				flag = this._XMLReader.MoveToAttribute(attrName);
			}
			if (flag)
			{
				text = this.Atomize(this._XMLReader.Value.Trim());
			}
			this._XMLReader.MoveToElement();
			if (!flag && throwExp)
			{
				throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_AttributeNotFound"), new object[]
				{
					attrName,
					this.XMLReader.LineNumber,
					this.XMLReader.LinePosition,
					this.XMLReader.Name
				}));
			}
			return text;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0001523E File Offset: 0x0001423E
		private void ResolveTypeAttribute(ref string typeName, out string typeNS, out bool bEmbedded, out bool bPrimitive)
		{
			if (WsdlParser.MatchingStrings(typeName, WsdlParser.s_emptyString))
			{
				typeName = WsdlParser.s_objectString;
				typeNS = this.SchemaNamespaceString;
				bEmbedded = true;
				bPrimitive = false;
				return;
			}
			typeNS = this.ParseQName(ref typeName);
			this.ResolveTypeNames(ref typeNS, ref typeName, out bEmbedded, out bPrimitive);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00015279 File Offset: 0x00014279
		private string ParseQName(ref string qname)
		{
			return this.ParseQName(ref qname, null);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00015284 File Offset: 0x00014284
		private string ParseQName(ref string qname, WsdlParser.URTNamespace defaultNS)
		{
			WsdlParser.URTNamespace urtnamespace = null;
			return this.ParseQName(ref qname, defaultNS, out urtnamespace);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000152A0 File Offset: 0x000142A0
		private string ParseQName(ref string qname, WsdlParser.URTNamespace defaultNS, out WsdlParser.URTNamespace returnNS)
		{
			returnNS = null;
			if (qname == null || qname.Length == 0)
			{
				return null;
			}
			int num = qname.IndexOf(":");
			string text;
			if (num == -1)
			{
				returnNS = defaultNS;
				if (defaultNS == null)
				{
					text = this._XMLReader.LookupNamespace("");
				}
				else
				{
					text = defaultNS.Name;
				}
			}
			else
			{
				string text2 = qname.Substring(0, num);
				qname = this.Atomize(qname.Substring(num + 1));
				text = this._XMLReader.LookupNamespace(text2);
			}
			text = this.Atomize(text);
			WsdlParser.URTNamespace urtnamespace = this.LookupNamespace(text);
			if (urtnamespace == null)
			{
				urtnamespace = new WsdlParser.URTNamespace(text, this);
			}
			returnNS = urtnamespace;
			return text;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0001533C File Offset: 0x0001433C
		private bool Qualify(string typeNS, string curNS)
		{
			return !this.MatchingSchemaStrings(typeNS) && !WsdlParser.MatchingStrings(typeNS, WsdlParser.s_soapNamespaceString) && !WsdlParser.MatchingStrings(typeNS, WsdlParser.s_wsdlSoapNamespaceString) && !WsdlParser.MatchingStrings(typeNS, "System") && !WsdlParser.MatchingStrings(typeNS, curNS);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001537A File Offset: 0x0001437A
		private bool MatchingNamespace(string elmNS)
		{
			return WsdlParser.MatchingStrings(this._XMLReader.NamespaceURI, elmNS);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00015394 File Offset: 0x00014394
		private bool MatchingSchemaNamespace()
		{
			if (this.MatchingNamespace(WsdlParser.s_schemaNamespaceString))
			{
				return true;
			}
			if (this.MatchingNamespace(WsdlParser.s_schemaNamespaceString1999))
			{
				this._xsdVersion = XsdVersion.V1999;
				return true;
			}
			if (this.MatchingNamespace(WsdlParser.s_schemaNamespaceString2000))
			{
				this._xsdVersion = XsdVersion.V2000;
				return true;
			}
			if (this.MatchingNamespace(WsdlParser.s_schemaNamespaceString))
			{
				this._xsdVersion = XsdVersion.V2001;
				return true;
			}
			return false;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x000153F4 File Offset: 0x000143F4
		internal static string IsValidUrl(string value)
		{
			if (value == null)
			{
				return "\"\"";
			}
			WsdlParser.vsb.Length = 0;
			WsdlParser.vsb.Append("@\"");
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] == '"')
				{
					WsdlParser.vsb.Append("\"\"");
				}
				else
				{
					WsdlParser.vsb.Append(value[i]);
				}
			}
			WsdlParser.vsb.Append("\"");
			return WsdlParser.vsb.ToString();
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001547F File Offset: 0x0001447F
		private static bool IsCSharpKeyword(string value)
		{
			if (WsdlParser.cSharpKeywords == null)
			{
				WsdlParser.InitKeywords();
			}
			return WsdlParser.cSharpKeywords.ContainsKey(value);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00015498 File Offset: 0x00014498
		private static void InitKeywords()
		{
			Hashtable hashtable = new Hashtable(75);
			object obj = new object();
			hashtable["abstract"] = obj;
			hashtable["base"] = obj;
			hashtable["bool"] = obj;
			hashtable["break"] = obj;
			hashtable["byte"] = obj;
			hashtable["case"] = obj;
			hashtable["catch"] = obj;
			hashtable["char"] = obj;
			hashtable["checked"] = obj;
			hashtable["class"] = obj;
			hashtable["const"] = obj;
			hashtable["continue"] = obj;
			hashtable["decimal"] = obj;
			hashtable["default"] = obj;
			hashtable["delegate"] = obj;
			hashtable["do"] = obj;
			hashtable["double"] = obj;
			hashtable["else"] = obj;
			hashtable["enum"] = obj;
			hashtable["event"] = obj;
			hashtable["exdouble"] = obj;
			hashtable["exfloat"] = obj;
			hashtable["explicit"] = obj;
			hashtable["extern"] = obj;
			hashtable["false"] = obj;
			hashtable["finally"] = obj;
			hashtable["fixed"] = obj;
			hashtable["float"] = obj;
			hashtable["for"] = obj;
			hashtable["foreach"] = obj;
			hashtable["goto"] = obj;
			hashtable["if"] = obj;
			hashtable["implicit"] = obj;
			hashtable["in"] = obj;
			hashtable["int"] = obj;
			hashtable["interface"] = obj;
			hashtable["internal"] = obj;
			hashtable["is"] = obj;
			hashtable["lock"] = obj;
			hashtable["long"] = obj;
			hashtable["namespace"] = obj;
			hashtable["new"] = obj;
			hashtable["null"] = obj;
			hashtable["object"] = obj;
			hashtable["operator"] = obj;
			hashtable["out"] = obj;
			hashtable["override"] = obj;
			hashtable["private"] = obj;
			hashtable["protected"] = obj;
			hashtable["public"] = obj;
			hashtable["readonly"] = obj;
			hashtable["ref"] = obj;
			hashtable["return"] = obj;
			hashtable["sbyte"] = obj;
			hashtable["sealed"] = obj;
			hashtable["short"] = obj;
			hashtable["sizeof"] = obj;
			hashtable["static"] = obj;
			hashtable["string"] = obj;
			hashtable["struct"] = obj;
			hashtable["switch"] = obj;
			hashtable["this"] = obj;
			hashtable["throw"] = obj;
			hashtable["true"] = obj;
			hashtable["try"] = obj;
			hashtable["typeof"] = obj;
			hashtable["uint"] = obj;
			hashtable["ulong"] = obj;
			hashtable["unchecked"] = obj;
			hashtable["unsafe"] = obj;
			hashtable["ushort"] = obj;
			hashtable["using"] = obj;
			hashtable["virtual"] = obj;
			hashtable["void"] = obj;
			hashtable["while"] = obj;
			WsdlParser.cSharpKeywords = hashtable;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00015840 File Offset: 0x00014840
		private static bool IsValidLanguageIndependentIdentifier(string ident)
		{
			int i = 0;
			while (i < ident.Length)
			{
				char c = ident[i];
				switch (char.GetUnicodeCategory(c))
				{
				case UnicodeCategory.UppercaseLetter:
				case UnicodeCategory.LowercaseLetter:
				case UnicodeCategory.TitlecaseLetter:
				case UnicodeCategory.ModifierLetter:
				case UnicodeCategory.OtherLetter:
				case UnicodeCategory.NonSpacingMark:
				case UnicodeCategory.SpacingCombiningMark:
				case UnicodeCategory.DecimalDigitNumber:
				case UnicodeCategory.ConnectorPunctuation:
					i++;
					break;
				case UnicodeCategory.EnclosingMark:
				case UnicodeCategory.LetterNumber:
				case UnicodeCategory.OtherNumber:
				case UnicodeCategory.SpaceSeparator:
				case UnicodeCategory.LineSeparator:
				case UnicodeCategory.ParagraphSeparator:
				case UnicodeCategory.Control:
				case UnicodeCategory.Format:
				case UnicodeCategory.Surrogate:
				case UnicodeCategory.PrivateUse:
				case UnicodeCategory.DashPunctuation:
				case UnicodeCategory.OpenPunctuation:
				case UnicodeCategory.ClosePunctuation:
				case UnicodeCategory.InitialQuotePunctuation:
				case UnicodeCategory.FinalQuotePunctuation:
				case UnicodeCategory.OtherPunctuation:
				case UnicodeCategory.MathSymbol:
				case UnicodeCategory.CurrencySymbol:
				case UnicodeCategory.ModifierSymbol:
				case UnicodeCategory.OtherSymbol:
				case UnicodeCategory.OtherNotAssigned:
					return false;
				default:
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000158FC File Offset: 0x000148FC
		internal static void CheckValidIdentifier(string ident)
		{
			if (!WsdlParser.IsValidLanguageIndependentIdentifier(ident))
			{
				throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_WsdlInvalidStringSyntax"), new object[] { ident }));
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00015938 File Offset: 0x00014938
		internal static string IsValidCSAttr(string identifier)
		{
			string text = WsdlParser.IsValidCS(identifier);
			if (text.Length > 0 && text[0] == '@')
			{
				return text.Substring(1);
			}
			return text;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001596C File Offset: 0x0001496C
		internal static string IsValidCS(string identifier)
		{
			if (identifier == null || identifier.Length == 0 || identifier == " ")
			{
				return identifier;
			}
			string text = identifier;
			int num = identifier.IndexOf('[');
			string text2 = null;
			if (num > -1)
			{
				text2 = identifier.Substring(num);
				identifier = identifier.Substring(0, num);
				foreach (char c in text2)
				{
					if (c != ' ' && c != ',')
					{
						switch (c)
						{
						case '[':
						case ']':
							break;
						default:
							throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_WsdlInvalidStringSyntax"), new object[] { identifier }));
						}
					}
				}
			}
			string[] array = identifier.Split(new char[] { '.' });
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < array.Length; j++)
			{
				if (j > 0)
				{
					stringBuilder.Append(".");
				}
				if (WsdlParser.IsCSharpKeyword(array[j]))
				{
					stringBuilder.Append("@");
					flag = true;
				}
				WsdlParser.CheckValidIdentifier(array[j]);
				stringBuilder.Append(array[j]);
			}
			if (flag)
			{
				if (text2 != null)
				{
					stringBuilder.Append(text2);
				}
				return stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00015AAE File Offset: 0x00014AAE
		private static bool MatchingStrings(string left, string right)
		{
			return left == right;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00015AB4 File Offset: 0x00014AB4
		private bool MatchingSchemaStrings(string left)
		{
			if (WsdlParser.MatchingStrings(left, WsdlParser.s_schemaNamespaceString1999))
			{
				this._xsdVersion = XsdVersion.V1999;
				return true;
			}
			if (WsdlParser.MatchingStrings(left, WsdlParser.s_schemaNamespaceString2000))
			{
				this._xsdVersion = XsdVersion.V2000;
				return true;
			}
			if (WsdlParser.MatchingStrings(left, WsdlParser.s_schemaNamespaceString))
			{
				this._xsdVersion = XsdVersion.V2001;
				return true;
			}
			return false;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00015B04 File Offset: 0x00014B04
		internal string Atomize(string str)
		{
			return this._XMLReader.NameTable.Add(str);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00015B18 File Offset: 0x00014B18
		private string MapSchemaTypesToCSharpTypes(string xsdType)
		{
			string text = xsdType;
			int num = xsdType.IndexOf('[');
			if (num != -1)
			{
				text = xsdType.Substring(0, num);
			}
			string text2 = SudsConverter.MapXsdToClrTypes(text);
			if (text2 == null)
			{
				throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_CantResolveTypeInNS"), new object[]
				{
					xsdType,
					WsdlParser.s_schemaNamespaceString
				}));
			}
			if (num != -1)
			{
				text2 += xsdType.Substring(num);
			}
			return text2;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00015B88 File Offset: 0x00014B88
		private bool IsPrimitiveType(string typeNS, string typeName)
		{
			bool flag = false;
			if (this.MatchingSchemaStrings(typeNS) && !WsdlParser.MatchingStrings(typeName, WsdlParser.s_urTypeString))
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00015BB0 File Offset: 0x00014BB0
		private WsdlParser.URTNamespace LookupNamespace(string name)
		{
			for (int i = 0; i < this._URTNamespaces.Count; i++)
			{
				WsdlParser.URTNamespace urtnamespace = (WsdlParser.URTNamespace)this._URTNamespaces[i];
				if (WsdlParser.MatchingStrings(urtnamespace.Name, name))
				{
					return urtnamespace;
				}
			}
			return null;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00015BF8 File Offset: 0x00014BF8
		internal WsdlParser.URTNamespace AddNewNamespace(string ns)
		{
			if (ns == null)
			{
				return null;
			}
			WsdlParser.URTNamespace urtnamespace = this.LookupNamespace(ns);
			if (urtnamespace == null)
			{
				urtnamespace = new WsdlParser.URTNamespace(ns, this);
			}
			if (!urtnamespace.IsSystem)
			{
				urtnamespace.bReferenced = true;
			}
			return urtnamespace;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00015C2D File Offset: 0x00014C2D
		internal void AddNamespace(WsdlParser.URTNamespace xns)
		{
			this._URTNamespaces.Add(xns);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00015C3C File Offset: 0x00014C3C
		private void PrintCSC()
		{
			int num = 0;
			for (int i = 0; i < this._URTNamespaces.Count; i++)
			{
				WsdlParser.URTNamespace urtnamespace = (WsdlParser.URTNamespace)this._URTNamespaces[i];
				if (!urtnamespace.IsEmpty && urtnamespace.UrtType == UrtType.Interop)
				{
					if (num == 0)
					{
						urtnamespace.EncodedNS = this._proxyNamespace;
					}
					else
					{
						urtnamespace.EncodedNS = this._proxyNamespace + num;
					}
					num++;
				}
			}
			for (int j = 0; j < this._URTNamespaces.Count; j++)
			{
				WsdlParser.URTNamespace urtnamespace2 = (WsdlParser.URTNamespace)this._URTNamespaces[j];
				if (!urtnamespace2.IsEmpty && urtnamespace2.UrtType != UrtType.UrtSystem && urtnamespace2.UrtType != UrtType.Xsd && urtnamespace2.UrtType != UrtType.None)
				{
					string text = (urtnamespace2.IsURTNamespace ? urtnamespace2.AssemName : urtnamespace2.EncodedNS);
					int num2 = text.IndexOf(',');
					if (num2 > -1)
					{
						text = text.Substring(0, num2);
					}
					string text2 = "";
					WsdlParser.WriterStream writerStream = WsdlParser.WriterStream.GetWriterStream(ref this._writerStreams, this._outputDir, text, ref text2);
					if (text2.Length > 0)
					{
						this._outCodeStreamList.Add(text2);
					}
					urtnamespace2.PrintCSC(writerStream);
				}
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00015D84 File Offset: 0x00014D84
		internal UrtType IsURTExportedType(string name, out string ns, out string assemName)
		{
			UrtType urtType = UrtType.None;
			ns = null;
			assemName = null;
			if (this.MatchingSchemaStrings(name))
			{
				urtType = UrtType.Xsd;
			}
			else
			{
				if (SoapServices.IsClrTypeNamespace(name))
				{
					SoapServices.DecodeXmlNamespaceForClrTypeNamespace(name, out ns, out assemName);
					if (assemName == null)
					{
						assemName = typeof(string).Assembly.GetName().Name;
						urtType = UrtType.UrtSystem;
					}
					else
					{
						urtType = UrtType.UrtUser;
					}
				}
				if (urtType == UrtType.None)
				{
					ns = name;
					assemName = ns;
					urtType = UrtType.Interop;
				}
				ns = this.Atomize(ns);
				assemName = this.Atomize(assemName);
			}
			return urtType;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00015E00 File Offset: 0x00014E00
		internal string GetTypeString(string curNS, bool bNS, WsdlParser.URTNamespace urtNS, string typeName, string typeNS)
		{
			WsdlParser.URTComplexType urtcomplexType = urtNS.LookupComplexType(typeName);
			string text;
			if (urtcomplexType != null && urtcomplexType.IsArray())
			{
				if (urtcomplexType.GetArray() == null)
				{
					urtcomplexType.ResolveArray();
				}
				string array = urtcomplexType.GetArray();
				WsdlParser.URTNamespace arrayNS = urtcomplexType.GetArrayNS();
				StringBuilder stringBuilder = new StringBuilder(50);
				if (arrayNS.EncodedNS != null && this.Qualify(urtNS.EncodedNS, arrayNS.EncodedNS))
				{
					stringBuilder.Append(WsdlParser.IsValidCSAttr(arrayNS.EncodedNS));
					stringBuilder.Append('.');
				}
				stringBuilder.Append(WsdlParser.IsValidCSAttr(array));
				text = stringBuilder.ToString();
			}
			else
			{
				string text2;
				if (urtNS.UrtType == UrtType.Interop)
				{
					text2 = urtNS.EncodedNS;
				}
				else
				{
					text2 = typeNS;
				}
				if (bNS && this.Qualify(text2, curNS))
				{
					StringBuilder stringBuilder2 = new StringBuilder(50);
					if (text2 != null)
					{
						stringBuilder2.Append(WsdlParser.IsValidCSAttr(text2));
						stringBuilder2.Append('.');
					}
					stringBuilder2.Append(WsdlParser.IsValidCSAttr(typeName));
					text = stringBuilder2.ToString();
				}
				else
				{
					text = typeName;
				}
			}
			int num = text.IndexOf('+');
			if (num > 0)
			{
				if (bNS)
				{
					text = text.Replace('+', '.');
				}
				else
				{
					text = text.Substring(0, num);
				}
			}
			return text;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00015F2C File Offset: 0x00014F2C
		private static XmlNameTable CreatePrimedNametable()
		{
			NameTable nameTable = new NameTable();
			WsdlParser.s_emptyString = nameTable.Add(string.Empty);
			WsdlParser.s_complexTypeString = nameTable.Add("complexType");
			WsdlParser.s_simpleTypeString = nameTable.Add("simpleType");
			WsdlParser.s_elementString = nameTable.Add("element");
			WsdlParser.s_enumerationString = nameTable.Add("enumeration");
			WsdlParser.s_encodingString = nameTable.Add("encoding");
			WsdlParser.s_attributeString = nameTable.Add("attribute");
			WsdlParser.s_attributesString = nameTable.Add("attributes");
			WsdlParser.s_allString = nameTable.Add("all");
			WsdlParser.s_sequenceString = nameTable.Add("sequence");
			WsdlParser.s_choiceString = nameTable.Add("choice");
			WsdlParser.s_minOccursString = nameTable.Add("minOccurs");
			WsdlParser.s_maxOccursString = nameTable.Add("maxOccurs");
			WsdlParser.s_unboundedString = nameTable.Add("unbounded");
			WsdlParser.s_oneString = nameTable.Add("1");
			WsdlParser.s_zeroString = nameTable.Add("0");
			WsdlParser.s_nameString = nameTable.Add("name");
			WsdlParser.s_typeString = nameTable.Add("type");
			WsdlParser.s_baseString = nameTable.Add("base");
			WsdlParser.s_valueString = nameTable.Add("value");
			WsdlParser.s_interfaceString = nameTable.Add("interface");
			WsdlParser.s_serviceString = nameTable.Add("service");
			WsdlParser.s_extendsString = nameTable.Add("extends");
			WsdlParser.s_addressesString = nameTable.Add("addresses");
			WsdlParser.s_addressString = nameTable.Add("address");
			WsdlParser.s_uriString = nameTable.Add("uri");
			WsdlParser.s_implementsString = nameTable.Add("implements");
			WsdlParser.s_nestedTypeString = nameTable.Add("nestedType");
			WsdlParser.s_requestString = nameTable.Add("request");
			WsdlParser.s_responseString = nameTable.Add("response");
			WsdlParser.s_requestResponseString = nameTable.Add("requestResponse");
			WsdlParser.s_messageString = nameTable.Add("message");
			WsdlParser.s_locationString = nameTable.Add("location");
			WsdlParser.s_schemaLocationString = nameTable.Add("schemaLocation");
			WsdlParser.s_importString = nameTable.Add("import");
			WsdlParser.s_onewayString = nameTable.Add("oneway");
			WsdlParser.s_includeString = nameTable.Add("include");
			WsdlParser.s_refString = nameTable.Add("ref");
			WsdlParser.s_refTypeString = nameTable.Add("refType");
			WsdlParser.s_referenceString = nameTable.Add("Reference");
			WsdlParser.s_objectString = nameTable.Add("Object");
			WsdlParser.s_urTypeString = nameTable.Add("anyType");
			WsdlParser.s_arrayString = nameTable.Add("Array");
			WsdlParser.s_sudsString = nameTable.Add("suds");
			WsdlParser.s_methodString = nameTable.Add("method");
			WsdlParser.s_useString = nameTable.Add("use");
			WsdlParser.s_rootTypeString = nameTable.Add("rootType");
			WsdlParser.s_soapString = nameTable.Add("soap");
			WsdlParser.s_serviceDescString = nameTable.Add("serviceDescription");
			WsdlParser.s_schemaString = nameTable.Add("schema");
			WsdlParser.s_targetNamespaceString = nameTable.Add("targetNamespace");
			WsdlParser.s_namespaceString = nameTable.Add("namespace");
			WsdlParser.s_idString = nameTable.Add("ID");
			WsdlParser.s_soapActionString = nameTable.Add("soapAction");
			WsdlParser.s_schemaNamespaceString1999 = nameTable.Add(SudsConverter.Xsd1999);
			WsdlParser.s_instanceNamespaceString1999 = nameTable.Add(SudsConverter.Xsi1999);
			WsdlParser.s_schemaNamespaceString2000 = nameTable.Add(SudsConverter.Xsd2000);
			WsdlParser.s_instanceNamespaceString2000 = nameTable.Add(SudsConverter.Xsi2000);
			WsdlParser.s_schemaNamespaceString = nameTable.Add(SudsConverter.Xsd2001);
			WsdlParser.s_instanceNamespaceString = nameTable.Add(SudsConverter.Xsi2001);
			WsdlParser.s_soapNamespaceString = nameTable.Add("urn:schemas-xmlsoap-org:soap.v1");
			WsdlParser.s_sudsNamespaceString = nameTable.Add("urn:schemas-xmlsoap-org:soap-sdl-2000-01-25");
			WsdlParser.s_serviceNamespaceString = nameTable.Add("urn:schemas-xmlsoap-org:sdl.2000-01-25");
			WsdlParser.s_definitionsString = nameTable.Add("definitions");
			WsdlParser.s_wsdlNamespaceString = nameTable.Add("http://schemas.xmlsoap.org/wsdl/");
			WsdlParser.s_wsdlSoapNamespaceString = nameTable.Add("http://schemas.xmlsoap.org/wsdl/soap/");
			WsdlParser.s_wsdlSudsNamespaceString = nameTable.Add("http://www.w3.org/2000/wsdl/suds");
			WsdlParser.s_enumTypeString = nameTable.Add("enumType");
			WsdlParser.s_typesString = nameTable.Add("types");
			WsdlParser.s_partString = nameTable.Add("part");
			WsdlParser.s_portTypeString = nameTable.Add("portType");
			WsdlParser.s_operationString = nameTable.Add("operation");
			WsdlParser.s_inputString = nameTable.Add("input");
			WsdlParser.s_outputString = nameTable.Add("output");
			WsdlParser.s_bindingString = nameTable.Add("binding");
			WsdlParser.s_classString = nameTable.Add("class");
			WsdlParser.s_structString = nameTable.Add("struct");
			WsdlParser.s_ISerializableString = nameTable.Add("ISerializable");
			WsdlParser.s_marshalByRefString = nameTable.Add("MarshalByRefObject");
			WsdlParser.s_delegateString = nameTable.Add("Delegate");
			WsdlParser.s_servicedComponentString = nameTable.Add("ServicedComponent");
			WsdlParser.s_comObjectString = nameTable.Add("__ComObject");
			WsdlParser.s_portString = nameTable.Add("port");
			WsdlParser.s_styleString = nameTable.Add("style");
			WsdlParser.s_transportString = nameTable.Add("transport");
			WsdlParser.s_encodedString = nameTable.Add("encoded");
			WsdlParser.s_faultString = nameTable.Add("fault");
			WsdlParser.s_bodyString = nameTable.Add("body");
			WsdlParser.s_partsString = nameTable.Add("parts");
			WsdlParser.s_headerString = nameTable.Add("header");
			WsdlParser.s_encodingStyleString = nameTable.Add("encodingStyle");
			WsdlParser.s_restrictionString = nameTable.Add("restriction");
			WsdlParser.s_complexContentString = nameTable.Add("complexContent");
			WsdlParser.s_soapEncodingString = nameTable.Add("http://schemas.xmlsoap.org/soap/encoding/");
			WsdlParser.s_arrayTypeString = nameTable.Add("arrayType");
			WsdlParser.s_parameterOrderString = nameTable.Add("parameterOrder");
			return nameTable;
		}

		// Token: 0x040002D5 RID: 725
		private static StringBuilder vsb = new StringBuilder();

		// Token: 0x040002D6 RID: 726
		private static Hashtable cSharpKeywords;

		// Token: 0x040002D7 RID: 727
		private XmlTextReader _XMLReader;

		// Token: 0x040002D8 RID: 728
		private ArrayList _URTNamespaces;

		// Token: 0x040002D9 RID: 729
		private WsdlParser.ReaderStream _parsingInput;

		// Token: 0x040002DA RID: 730
		internal bool _bWrappedProxy;

		// Token: 0x040002DB RID: 731
		private string _proxyNamespace;

		// Token: 0x040002DC RID: 732
		private int _proxyNamespaceCount;

		// Token: 0x040002DD RID: 733
		private WsdlParser.ReaderStream _readerStreamsWsdl;

		// Token: 0x040002DE RID: 734
		private WsdlParser.ReaderStream _readerStreamsXsd;

		// Token: 0x040002DF RID: 735
		private string _outputDir;

		// Token: 0x040002E0 RID: 736
		private ArrayList _outCodeStreamList;

		// Token: 0x040002E1 RID: 737
		private WsdlParser.WriterStream _writerStreams;

		// Token: 0x040002E2 RID: 738
		private SchemaBlockType _blockDefault;

		// Token: 0x040002E3 RID: 739
		private XsdVersion _xsdVersion;

		// Token: 0x040002E4 RID: 740
		private Hashtable wsdlMessages = new Hashtable(10);

		// Token: 0x040002E5 RID: 741
		private Hashtable wsdlPortTypes = new Hashtable(10);

		// Token: 0x040002E6 RID: 742
		private ArrayList wsdlBindings = new ArrayList(10);

		// Token: 0x040002E7 RID: 743
		private ArrayList wsdlServices = new ArrayList(10);

		// Token: 0x040002E8 RID: 744
		private Stack _currentReaderStack = new Stack(5);

		// Token: 0x040002E9 RID: 745
		private Stack _currentSchemaReaderStack = new Stack(5);

		// Token: 0x040002EA RID: 746
		private XmlNameTable _primedNametable;

		// Token: 0x040002EB RID: 747
		private static string s_emptyString;

		// Token: 0x040002EC RID: 748
		private static string s_complexTypeString;

		// Token: 0x040002ED RID: 749
		private static string s_simpleTypeString;

		// Token: 0x040002EE RID: 750
		private static string s_elementString;

		// Token: 0x040002EF RID: 751
		private static string s_enumerationString;

		// Token: 0x040002F0 RID: 752
		private static string s_encodingString;

		// Token: 0x040002F1 RID: 753
		private static string s_attributeString;

		// Token: 0x040002F2 RID: 754
		private static string s_attributesString;

		// Token: 0x040002F3 RID: 755
		private static string s_allString;

		// Token: 0x040002F4 RID: 756
		private static string s_sequenceString;

		// Token: 0x040002F5 RID: 757
		private static string s_choiceString;

		// Token: 0x040002F6 RID: 758
		private static string s_minOccursString;

		// Token: 0x040002F7 RID: 759
		private static string s_maxOccursString;

		// Token: 0x040002F8 RID: 760
		private static string s_unboundedString;

		// Token: 0x040002F9 RID: 761
		private static string s_oneString;

		// Token: 0x040002FA RID: 762
		private static string s_zeroString;

		// Token: 0x040002FB RID: 763
		private static string s_nameString;

		// Token: 0x040002FC RID: 764
		private static string s_enumTypeString;

		// Token: 0x040002FD RID: 765
		private static string s_typeString;

		// Token: 0x040002FE RID: 766
		private static string s_baseString;

		// Token: 0x040002FF RID: 767
		private static string s_valueString;

		// Token: 0x04000300 RID: 768
		private static string s_interfaceString;

		// Token: 0x04000301 RID: 769
		private static string s_serviceString;

		// Token: 0x04000302 RID: 770
		private static string s_extendsString;

		// Token: 0x04000303 RID: 771
		private static string s_addressesString;

		// Token: 0x04000304 RID: 772
		private static string s_addressString;

		// Token: 0x04000305 RID: 773
		private static string s_uriString;

		// Token: 0x04000306 RID: 774
		private static string s_implementsString;

		// Token: 0x04000307 RID: 775
		private static string s_nestedTypeString;

		// Token: 0x04000308 RID: 776
		private static string s_requestString;

		// Token: 0x04000309 RID: 777
		private static string s_responseString;

		// Token: 0x0400030A RID: 778
		private static string s_requestResponseString;

		// Token: 0x0400030B RID: 779
		private static string s_messageString;

		// Token: 0x0400030C RID: 780
		private static string s_locationString;

		// Token: 0x0400030D RID: 781
		private static string s_schemaLocationString;

		// Token: 0x0400030E RID: 782
		private static string s_importString;

		// Token: 0x0400030F RID: 783
		private static string s_includeString;

		// Token: 0x04000310 RID: 784
		private static string s_onewayString;

		// Token: 0x04000311 RID: 785
		private static string s_refString;

		// Token: 0x04000312 RID: 786
		private static string s_refTypeString;

		// Token: 0x04000313 RID: 787
		private static string s_referenceString;

		// Token: 0x04000314 RID: 788
		private static string s_arrayString;

		// Token: 0x04000315 RID: 789
		private static string s_objectString;

		// Token: 0x04000316 RID: 790
		private static string s_urTypeString;

		// Token: 0x04000317 RID: 791
		private static string s_methodString;

		// Token: 0x04000318 RID: 792
		private static string s_sudsString;

		// Token: 0x04000319 RID: 793
		private static string s_useString;

		// Token: 0x0400031A RID: 794
		private static string s_rootTypeString;

		// Token: 0x0400031B RID: 795
		private static string s_soapString;

		// Token: 0x0400031C RID: 796
		private static string s_serviceDescString;

		// Token: 0x0400031D RID: 797
		private static string s_schemaString;

		// Token: 0x0400031E RID: 798
		private static string s_targetNamespaceString;

		// Token: 0x0400031F RID: 799
		private static string s_namespaceString;

		// Token: 0x04000320 RID: 800
		private static string s_idString;

		// Token: 0x04000321 RID: 801
		private static string s_soapActionString;

		// Token: 0x04000322 RID: 802
		private static string s_instanceNamespaceString;

		// Token: 0x04000323 RID: 803
		private static string s_schemaNamespaceString;

		// Token: 0x04000324 RID: 804
		private static string s_instanceNamespaceString1999;

		// Token: 0x04000325 RID: 805
		private static string s_schemaNamespaceString1999;

		// Token: 0x04000326 RID: 806
		private static string s_instanceNamespaceString2000;

		// Token: 0x04000327 RID: 807
		private static string s_schemaNamespaceString2000;

		// Token: 0x04000328 RID: 808
		private static string s_soapNamespaceString;

		// Token: 0x04000329 RID: 809
		private static string s_sudsNamespaceString;

		// Token: 0x0400032A RID: 810
		private static string s_serviceNamespaceString;

		// Token: 0x0400032B RID: 811
		private static string s_definitionsString;

		// Token: 0x0400032C RID: 812
		private static string s_wsdlNamespaceString;

		// Token: 0x0400032D RID: 813
		private static string s_wsdlSoapNamespaceString;

		// Token: 0x0400032E RID: 814
		private static string s_wsdlSudsNamespaceString;

		// Token: 0x0400032F RID: 815
		private static string s_typesString;

		// Token: 0x04000330 RID: 816
		private static string s_partString;

		// Token: 0x04000331 RID: 817
		private static string s_portTypeString;

		// Token: 0x04000332 RID: 818
		private static string s_operationString;

		// Token: 0x04000333 RID: 819
		private static string s_inputString;

		// Token: 0x04000334 RID: 820
		private static string s_outputString;

		// Token: 0x04000335 RID: 821
		private static string s_bindingString;

		// Token: 0x04000336 RID: 822
		private static string s_classString;

		// Token: 0x04000337 RID: 823
		private static string s_structString;

		// Token: 0x04000338 RID: 824
		private static string s_ISerializableString;

		// Token: 0x04000339 RID: 825
		private static string s_marshalByRefString;

		// Token: 0x0400033A RID: 826
		private static string s_delegateString;

		// Token: 0x0400033B RID: 827
		private static string s_servicedComponentString;

		// Token: 0x0400033C RID: 828
		private static string s_comObjectString;

		// Token: 0x0400033D RID: 829
		private static string s_portString;

		// Token: 0x0400033E RID: 830
		private static string s_styleString;

		// Token: 0x0400033F RID: 831
		private static string s_transportString;

		// Token: 0x04000340 RID: 832
		private static string s_encodedString;

		// Token: 0x04000341 RID: 833
		private static string s_faultString;

		// Token: 0x04000342 RID: 834
		private static string s_bodyString;

		// Token: 0x04000343 RID: 835
		private static string s_partsString;

		// Token: 0x04000344 RID: 836
		private static string s_headerString;

		// Token: 0x04000345 RID: 837
		private static string s_encodingStyleString;

		// Token: 0x04000346 RID: 838
		private static string s_restrictionString;

		// Token: 0x04000347 RID: 839
		private static string s_complexContentString;

		// Token: 0x04000348 RID: 840
		private static string s_soapEncodingString;

		// Token: 0x04000349 RID: 841
		private static string s_arrayTypeString;

		// Token: 0x0400034A RID: 842
		private static string s_parameterOrderString;

		// Token: 0x0200007C RID: 124
		internal class ReaderStream
		{
			// Token: 0x060003E7 RID: 999 RVA: 0x0001654C File Offset: 0x0001554C
			internal ReaderStream(string location)
			{
				this._location = location;
				this._name = string.Empty;
				this._targetNS = string.Empty;
				this._uniqueNS = null;
				this._reader = null;
				this._next = null;
			}

			// Token: 0x170000DA RID: 218
			// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00016586 File Offset: 0x00015586
			// (set) Token: 0x060003E9 RID: 1001 RVA: 0x0001658E File Offset: 0x0001558E
			internal string Location
			{
				get
				{
					return this._location;
				}
				set
				{
					this._location = value;
				}
			}

			// Token: 0x170000DB RID: 219
			// (set) Token: 0x060003EA RID: 1002 RVA: 0x00016597 File Offset: 0x00015597
			internal string Name
			{
				set
				{
					this._name = value;
				}
			}

			// Token: 0x170000DC RID: 220
			// (get) Token: 0x060003EB RID: 1003 RVA: 0x000165A0 File Offset: 0x000155A0
			// (set) Token: 0x060003EC RID: 1004 RVA: 0x000165A8 File Offset: 0x000155A8
			internal string TargetNS
			{
				get
				{
					return this._targetNS;
				}
				set
				{
					this._targetNS = value;
				}
			}

			// Token: 0x170000DD RID: 221
			// (get) Token: 0x060003ED RID: 1005 RVA: 0x000165B1 File Offset: 0x000155B1
			// (set) Token: 0x060003EE RID: 1006 RVA: 0x000165B9 File Offset: 0x000155B9
			internal WsdlParser.URTNamespace UniqueNS
			{
				get
				{
					return this._uniqueNS;
				}
				set
				{
					this._uniqueNS = value;
				}
			}

			// Token: 0x170000DE RID: 222
			// (get) Token: 0x060003EF RID: 1007 RVA: 0x000165C2 File Offset: 0x000155C2
			// (set) Token: 0x060003F0 RID: 1008 RVA: 0x000165CA File Offset: 0x000155CA
			internal TextReader InputStream
			{
				get
				{
					return this._reader;
				}
				set
				{
					this._reader = value;
				}
			}

			// Token: 0x170000DF RID: 223
			// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000165D3 File Offset: 0x000155D3
			// (set) Token: 0x060003F2 RID: 1010 RVA: 0x000165DB File Offset: 0x000155DB
			internal Uri Uri
			{
				get
				{
					return this._uri;
				}
				set
				{
					this._uri = value;
				}
			}

			// Token: 0x060003F3 RID: 1011 RVA: 0x000165E4 File Offset: 0x000155E4
			internal static void GetReaderStream(WsdlParser.ReaderStream inputStreams, WsdlParser.ReaderStream newStream)
			{
				WsdlParser.ReaderStream readerStream = inputStreams;
				while (!(readerStream._location == newStream.Location))
				{
					WsdlParser.ReaderStream readerStream2 = readerStream;
					readerStream = readerStream._next;
					if (readerStream == null)
					{
						readerStream2._next = newStream;
						return;
					}
				}
			}

			// Token: 0x060003F4 RID: 1012 RVA: 0x0001661C File Offset: 0x0001561C
			internal static WsdlParser.ReaderStream GetNextReaderStream(WsdlParser.ReaderStream input)
			{
				return input._next;
			}

			// Token: 0x0400034B RID: 843
			private string _location;

			// Token: 0x0400034C RID: 844
			private string _name;

			// Token: 0x0400034D RID: 845
			private string _targetNS;

			// Token: 0x0400034E RID: 846
			private WsdlParser.URTNamespace _uniqueNS;

			// Token: 0x0400034F RID: 847
			private TextReader _reader;

			// Token: 0x04000350 RID: 848
			private WsdlParser.ReaderStream _next;

			// Token: 0x04000351 RID: 849
			private Uri _uri;
		}

		// Token: 0x0200007D RID: 125
		internal class WriterStream
		{
			// Token: 0x060003F5 RID: 1013 RVA: 0x00016624 File Offset: 0x00015624
			private WriterStream(string fileName, TextWriter writer)
			{
				this._fileName = fileName;
				this._writer = writer;
			}

			// Token: 0x170000E0 RID: 224
			// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0001663A File Offset: 0x0001563A
			internal TextWriter OutputStream
			{
				get
				{
					return this._writer;
				}
			}

			// Token: 0x060003F7 RID: 1015 RVA: 0x00016642 File Offset: 0x00015642
			internal bool GetWrittenTo()
			{
				return this._bWrittenTo;
			}

			// Token: 0x060003F8 RID: 1016 RVA: 0x0001664A File Offset: 0x0001564A
			internal void SetWrittenTo()
			{
				this._bWrittenTo = true;
			}

			// Token: 0x060003F9 RID: 1017 RVA: 0x00016653 File Offset: 0x00015653
			internal static void Flush(WsdlParser.WriterStream writerStream)
			{
				while (writerStream != null)
				{
					writerStream._writer.Flush();
					writerStream = writerStream._next;
				}
			}

			// Token: 0x060003FA RID: 1018 RVA: 0x00016670 File Offset: 0x00015670
			internal static WsdlParser.WriterStream GetWriterStream(ref WsdlParser.WriterStream outputStreams, string outputDir, string fileName, ref string completeFileName)
			{
				WsdlParser.WriterStream writerStream;
				for (writerStream = outputStreams; writerStream != null; writerStream = writerStream._next)
				{
					if (writerStream._fileName == fileName)
					{
						return writerStream;
					}
				}
				string text = fileName;
				if (text.EndsWith(".exe", StringComparison.Ordinal) || text.EndsWith(".dll", StringComparison.Ordinal))
				{
					text = text.Substring(0, text.Length - 4);
				}
				string text2 = outputDir + text + ".cs";
				completeFileName = text2;
				TextWriter textWriter = new StreamWriter(text2, false, new UTF8Encoding(false));
				writerStream = new WsdlParser.WriterStream(fileName, textWriter);
				writerStream._next = outputStreams;
				outputStreams = writerStream;
				return writerStream;
			}

			// Token: 0x060003FB RID: 1019 RVA: 0x000166FC File Offset: 0x000156FC
			internal static void Close(WsdlParser.WriterStream outputStreams)
			{
				for (WsdlParser.WriterStream writerStream = outputStreams; writerStream != null; writerStream = writerStream._next)
				{
					writerStream._writer.Close();
				}
			}

			// Token: 0x04000352 RID: 850
			private string _fileName;

			// Token: 0x04000353 RID: 851
			private TextWriter _writer;

			// Token: 0x04000354 RID: 852
			private WsdlParser.WriterStream _next;

			// Token: 0x04000355 RID: 853
			private bool _bWrittenTo;
		}

		// Token: 0x0200007E RID: 126
		[Serializable]
		internal enum URTParamType
		{
			// Token: 0x04000357 RID: 855
			IN,
			// Token: 0x04000358 RID: 856
			OUT,
			// Token: 0x04000359 RID: 857
			REF
		}

		// Token: 0x0200007F RID: 127
		internal class URTParam
		{
			// Token: 0x060003FC RID: 1020 RVA: 0x00016724 File Offset: 0x00015724
			internal URTParam(string name, string typeName, string typeNS, string encodedNS, WsdlParser.URTParamType pType, bool bEmbedded, WsdlParser parser, WsdlParser.URTNamespace urtNamespace)
			{
				this._name = name;
				this._typeName = typeName;
				this._typeNS = typeNS;
				this._encodedNS = encodedNS;
				this._pType = pType;
				this._embeddedParam = bEmbedded;
				this._parser = parser;
				this._urtNamespace = urtNamespace;
			}

			// Token: 0x060003FD RID: 1021 RVA: 0x00016774 File Offset: 0x00015774
			public override bool Equals(object obj)
			{
				WsdlParser.URTParam urtparam = (WsdlParser.URTParam)obj;
				return this._pType == urtparam._pType && WsdlParser.MatchingStrings(this._typeName, urtparam._typeName) && WsdlParser.MatchingStrings(this._typeNS, urtparam._typeNS);
			}

			// Token: 0x060003FE RID: 1022 RVA: 0x000167BF File Offset: 0x000157BF
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x170000E1 RID: 225
			// (get) Token: 0x060003FF RID: 1023 RVA: 0x000167C7 File Offset: 0x000157C7
			// (set) Token: 0x06000400 RID: 1024 RVA: 0x000167CF File Offset: 0x000157CF
			internal WsdlParser.URTParamType ParamType
			{
				get
				{
					return this._pType;
				}
				set
				{
					this._pType = value;
				}
			}

			// Token: 0x170000E2 RID: 226
			// (get) Token: 0x06000401 RID: 1025 RVA: 0x000167D8 File Offset: 0x000157D8
			internal string Name
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x170000E3 RID: 227
			// (get) Token: 0x06000402 RID: 1026 RVA: 0x000167E0 File Offset: 0x000157E0
			internal string TypeName
			{
				get
				{
					return this._typeName;
				}
			}

			// Token: 0x170000E4 RID: 228
			// (get) Token: 0x06000403 RID: 1027 RVA: 0x000167E8 File Offset: 0x000157E8
			internal string TypeNS
			{
				get
				{
					return this._typeNS;
				}
			}

			// Token: 0x06000404 RID: 1028 RVA: 0x000167F0 File Offset: 0x000157F0
			internal string GetTypeString(string curNS, bool bNS)
			{
				return this._parser.GetTypeString(curNS, bNS, this._urtNamespace, this._typeName, this._encodedNS);
			}

			// Token: 0x06000405 RID: 1029 RVA: 0x00016811 File Offset: 0x00015811
			internal void PrintCSC(StringBuilder sb, string curNS)
			{
				sb.Append(WsdlParser.URTParam.PTypeString[(int)this._pType]);
				sb.Append(this.GetTypeString(curNS, true));
				sb.Append(' ');
				sb.Append(WsdlParser.IsValidCS(this._name));
			}

			// Token: 0x06000406 RID: 1030 RVA: 0x00016850 File Offset: 0x00015850
			internal void PrintCSC(StringBuilder sb)
			{
				sb.Append(WsdlParser.URTParam.PTypeString[(int)this._pType]);
				sb.Append(WsdlParser.IsValidCS(this._name));
			}

			// Token: 0x0400035A RID: 858
			private static string[] PTypeString = new string[] { "", "out ", "ref " };

			// Token: 0x0400035B RID: 859
			private string _name;

			// Token: 0x0400035C RID: 860
			private string _typeName;

			// Token: 0x0400035D RID: 861
			private string _typeNS;

			// Token: 0x0400035E RID: 862
			private string _encodedNS;

			// Token: 0x0400035F RID: 863
			private WsdlParser.URTParamType _pType;

			// Token: 0x04000360 RID: 864
			private bool _embeddedParam;

			// Token: 0x04000361 RID: 865
			private WsdlParser.URTNamespace _urtNamespace;

			// Token: 0x04000362 RID: 866
			private WsdlParser _parser;
		}

		// Token: 0x02000080 RID: 128
		[Flags]
		internal enum MethodPrintEnum
		{
			// Token: 0x04000364 RID: 868
			PrintBody = 1,
			// Token: 0x04000365 RID: 869
			InterfaceMethods = 2,
			// Token: 0x04000366 RID: 870
			InterfaceInClass = 4
		}

		// Token: 0x02000081 RID: 129
		[Flags]
		internal enum MethodFlags
		{
			// Token: 0x04000368 RID: 872
			None = 0,
			// Token: 0x04000369 RID: 873
			Public = 1,
			// Token: 0x0400036A RID: 874
			Protected = 2,
			// Token: 0x0400036B RID: 875
			Override = 4,
			// Token: 0x0400036C RID: 876
			New = 8,
			// Token: 0x0400036D RID: 877
			Virtual = 16,
			// Token: 0x0400036E RID: 878
			Internal = 32
		}

		// Token: 0x02000082 RID: 130
		internal abstract class URTMethod
		{
			// Token: 0x06000408 RID: 1032 RVA: 0x000168AA File Offset: 0x000158AA
			internal static bool FlagTest(WsdlParser.MethodPrintEnum flag, WsdlParser.MethodPrintEnum target)
			{
				return (flag & target) == target;
			}

			// Token: 0x06000409 RID: 1033 RVA: 0x000168B5 File Offset: 0x000158B5
			internal static bool MethodFlagsTest(WsdlParser.MethodFlags flag, WsdlParser.MethodFlags target)
			{
				return (flag & target) == target;
			}

			// Token: 0x0600040A RID: 1034 RVA: 0x000168C0 File Offset: 0x000158C0
			internal URTMethod(string name, string soapAction, string methodAttributes, WsdlParser.URTComplexType complexType)
			{
				this._methodName = name;
				this._soapAction = soapAction;
				this._methodType = null;
				this._complexType = complexType;
				name.IndexOf('.');
				this._methodFlags = WsdlParser.MethodFlags.None;
				if (methodAttributes != null && methodAttributes.Length > 0)
				{
					string[] array = methodAttributes.Split(new char[] { ' ' });
					foreach (string text in array)
					{
						if (text == "virtual")
						{
							this._methodFlags |= WsdlParser.MethodFlags.Virtual;
						}
						if (text == "new")
						{
							this._methodFlags |= WsdlParser.MethodFlags.New;
						}
						if (text == "override")
						{
							this._methodFlags |= WsdlParser.MethodFlags.Override;
						}
						if (text == "public")
						{
							this._methodFlags |= WsdlParser.MethodFlags.Public;
						}
						if (text == "protected")
						{
							this._methodFlags |= WsdlParser.MethodFlags.Protected;
						}
						if (text == "internal")
						{
							this._methodFlags |= WsdlParser.MethodFlags.Internal;
						}
					}
				}
			}

			// Token: 0x170000E5 RID: 229
			// (get) Token: 0x0600040B RID: 1035 RVA: 0x000169FF File Offset: 0x000159FF
			internal string Name
			{
				get
				{
					return this._methodName;
				}
			}

			// Token: 0x170000E6 RID: 230
			// (get) Token: 0x0600040C RID: 1036 RVA: 0x00016A07 File Offset: 0x00015A07
			internal string SoapAction
			{
				get
				{
					return this._soapAction;
				}
			}

			// Token: 0x170000E7 RID: 231
			// (get) Token: 0x0600040D RID: 1037 RVA: 0x00016A0F File Offset: 0x00015A0F
			// (set) Token: 0x0600040E RID: 1038 RVA: 0x00016A17 File Offset: 0x00015A17
			internal WsdlParser.MethodFlags MethodFlags
			{
				get
				{
					return this._methodFlags;
				}
				set
				{
					this._methodFlags = value;
				}
			}

			// Token: 0x0600040F RID: 1039 RVA: 0x00016A20 File Offset: 0x00015A20
			internal string GetTypeString(string curNS, bool bNS)
			{
				if (this._methodType == null)
				{
					return "void";
				}
				return this._methodType.GetTypeString(curNS, bNS);
			}

			// Token: 0x170000E8 RID: 232
			// (get) Token: 0x06000410 RID: 1040 RVA: 0x00016A3D File Offset: 0x00015A3D
			protected WsdlParser.URTParam MethodType
			{
				get
				{
					return this._methodType;
				}
			}

			// Token: 0x06000411 RID: 1041 RVA: 0x00016A45 File Offset: 0x00015A45
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x06000412 RID: 1042 RVA: 0x00016A50 File Offset: 0x00015A50
			public override bool Equals(object obj)
			{
				WsdlParser.URTMethod urtmethod = (WsdlParser.URTMethod)obj;
				if (WsdlParser.MatchingStrings(this._methodName, urtmethod._methodName) && this._params.Count == urtmethod._params.Count)
				{
					for (int i = 0; i < this._params.Count; i++)
					{
						if (!this._params[i].Equals(urtmethod._params[i]))
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}

			// Token: 0x06000413 RID: 1043 RVA: 0x00016AC9 File Offset: 0x00015AC9
			internal WsdlParser.MethodFlags GetMethodFlags(MethodInfo method)
			{
				return this._methodFlags;
			}

			// Token: 0x06000414 RID: 1044 RVA: 0x00016AD4 File Offset: 0x00015AD4
			internal void AddParam(WsdlParser.URTParam newParam)
			{
				int i = 0;
				while (i < this._params.Count)
				{
					WsdlParser.URTParam urtparam = (WsdlParser.URTParam)this._params[i];
					if (WsdlParser.MatchingStrings(urtparam.Name, newParam.Name))
					{
						if (urtparam.ParamType == WsdlParser.URTParamType.IN && newParam.ParamType == WsdlParser.URTParamType.OUT && WsdlParser.MatchingStrings(urtparam.TypeName, newParam.TypeName) && WsdlParser.MatchingStrings(urtparam.TypeNS, newParam.TypeNS))
						{
							urtparam.ParamType = WsdlParser.URTParamType.REF;
							return;
						}
						throw new SUDSParserException(CoreChannel.GetResourceString("Remoting_Suds_DuplicateParameter"));
					}
					else
					{
						i++;
					}
				}
				int num = -1;
				if (this._paramNamesOrder != null)
				{
					for (int j = 0; j < this._paramNamesOrder.Length; j++)
					{
						if (this._paramNamesOrder[j] == newParam.Name)
						{
							num = j;
							break;
						}
					}
					if (num == -1)
					{
						this._methodType = newParam;
						return;
					}
					this._params.Add(newParam);
					this._paramPosition.Add(num);
					return;
				}
				else
				{
					if (this._methodType == null && newParam.ParamType == WsdlParser.URTParamType.OUT)
					{
						this._methodType = newParam;
						return;
					}
					this._params.Add(newParam);
					return;
				}
			}

			// Token: 0x06000415 RID: 1045 RVA: 0x00016BF6 File Offset: 0x00015BF6
			internal void ResolveMethodAttributes()
			{
				if (!WsdlParser.URTMethod.MethodFlagsTest(this._methodFlags, WsdlParser.MethodFlags.Override) && !WsdlParser.URTMethod.MethodFlagsTest(this._methodFlags, WsdlParser.MethodFlags.New))
				{
					this.FindMethodAttributes();
				}
			}

			// Token: 0x06000416 RID: 1046 RVA: 0x00016C1C File Offset: 0x00015C1C
			private void FindMethodAttributes()
			{
				if (this._complexType == null)
				{
					return;
				}
				ArrayList arrayList = this._complexType.Inherit;
				Type type = null;
				if (arrayList == null)
				{
					arrayList = new ArrayList();
					if (this._complexType.SUDSType == SUDSType.ClientProxy)
					{
						type = typeof(RemotingClientProxy);
					}
					else if (this._complexType.SudsUse == WsdlParser.SudsUse.MarshalByRef)
					{
						type = typeof(MarshalByRefObject);
					}
					else if (this._complexType.SudsUse == WsdlParser.SudsUse.ServicedComponent)
					{
						type = typeof(MarshalByRefObject);
					}
					if (type == null)
					{
						return;
					}
					while (type != null)
					{
						arrayList.Add(type);
						type = type.BaseType;
					}
					this._complexType.Inherit = arrayList;
				}
				BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				bool flag = WsdlParser.URTMethod.MethodFlagsTest(this._methodFlags, WsdlParser.MethodFlags.Virtual);
				bool flag2 = false;
				for (int i = 0; i < arrayList.Count; i++)
				{
					type = (Type)arrayList[i];
					MethodInfo[] array = null;
					try
					{
						MethodInfo method = type.GetMethod(this.Name, bindingFlags);
						if (method != null)
						{
							array = new MethodInfo[] { method };
						}
					}
					catch
					{
						array = type.GetMethods(bindingFlags);
					}
					if (array != null)
					{
						MethodInfo[] array2 = array;
						int j = 0;
						while (j < array2.Length)
						{
							MethodBase methodBase = array2[j];
							if (methodBase != null && methodBase.Name == this.Name && (methodBase.IsFamily || methodBase.IsPublic || methodBase.IsAssembly) && this.IsSignature(methodBase))
							{
								flag2 = true;
								if (!methodBase.IsPublic)
								{
									if (methodBase.IsAssembly)
									{
										this._methodFlags &= ~WsdlParser.MethodFlags.Public;
										this._methodFlags |= WsdlParser.MethodFlags.Internal;
									}
									else if (methodBase.IsFamily)
									{
										this._methodFlags &= ~WsdlParser.MethodFlags.Public;
										this._methodFlags |= WsdlParser.MethodFlags.Protected;
									}
								}
								if (methodBase.IsFinal)
								{
									this._methodFlags |= WsdlParser.MethodFlags.New;
									break;
								}
								if (methodBase.IsVirtual && flag)
								{
									this._methodFlags |= WsdlParser.MethodFlags.Override;
									break;
								}
								this._methodFlags |= WsdlParser.MethodFlags.New;
								break;
							}
							else
							{
								j++;
							}
						}
					}
					if (flag2)
					{
						return;
					}
				}
			}

			// Token: 0x06000417 RID: 1047 RVA: 0x00016E54 File Offset: 0x00015E54
			private bool IsSignature(MethodBase baseInfo)
			{
				ParameterInfo[] parameters = baseInfo.GetParameters();
				if (this._params.Count != parameters.Length)
				{
					return false;
				}
				bool flag = true;
				for (int i = 0; i < parameters.Length; i++)
				{
					WsdlParser.URTParam urtparam = (WsdlParser.URTParam)this._params[i];
					if (urtparam.GetTypeString(null, true) != parameters[i].ParameterType.FullName)
					{
						flag = false;
						break;
					}
				}
				return flag;
			}

			// Token: 0x06000418 RID: 1048 RVA: 0x00016EC0 File Offset: 0x00015EC0
			internal void PrintSignature(StringBuilder sb, string curNS)
			{
				for (int i = 0; i < this._params.Count; i++)
				{
					if (i != 0)
					{
						sb.Append(", ");
					}
					((WsdlParser.URTParam)this._params[i]).PrintCSC(sb, curNS);
				}
			}

			// Token: 0x06000419 RID: 1049 RVA: 0x00016F0C File Offset: 0x00015F0C
			internal virtual void PrintCSC(TextWriter textWriter, string indentation, string namePrefix, string curNS, WsdlParser.MethodPrintEnum methodPrintEnum, bool bURTType, string bodyPrefix, StringBuilder sb)
			{
				sb.Length = 0;
				sb.Append(indentation);
				if (this.Name == "Finalize")
				{
					return;
				}
				if (WsdlParser.URTMethod.FlagTest(methodPrintEnum, WsdlParser.MethodPrintEnum.InterfaceInClass))
				{
					sb.Append("public ");
				}
				else if (WsdlParser.URTMethod.MethodFlagsTest(this._methodFlags, WsdlParser.MethodFlags.Public))
				{
					sb.Append("public ");
				}
				else if (WsdlParser.URTMethod.MethodFlagsTest(this._methodFlags, WsdlParser.MethodFlags.Protected))
				{
					sb.Append("protected ");
				}
				else if (WsdlParser.URTMethod.MethodFlagsTest(this._methodFlags, WsdlParser.MethodFlags.Internal))
				{
					sb.Append("internal ");
				}
				if (WsdlParser.URTMethod.MethodFlagsTest(this._methodFlags, WsdlParser.MethodFlags.Override))
				{
					sb.Append("override ");
				}
				else if (WsdlParser.URTMethod.MethodFlagsTest(this._methodFlags, WsdlParser.MethodFlags.Virtual))
				{
					sb.Append("virtual ");
				}
				if (WsdlParser.URTMethod.MethodFlagsTest(this._methodFlags, WsdlParser.MethodFlags.New))
				{
					sb.Append("new ");
				}
				sb.Append(WsdlParser.IsValidCSAttr(this.GetTypeString(curNS, true)));
				if (WsdlParser.URTMethod.FlagTest(methodPrintEnum, WsdlParser.MethodPrintEnum.InterfaceInClass))
				{
					sb.Append(" ");
				}
				else
				{
					sb.Append(WsdlParser.IsValidCSAttr(namePrefix));
				}
				if (this._wsdlMethodInfo.bProperty)
				{
					sb.Append(WsdlParser.IsValidCS(this._wsdlMethodInfo.propertyName));
				}
				else
				{
					sb.Append(WsdlParser.IsValidCS(this._methodName));
					sb.Append('(');
					if (this._params.Count > 0)
					{
						((WsdlParser.URTParam)this._params[0]).PrintCSC(sb, curNS);
						for (int i = 1; i < this._params.Count; i++)
						{
							sb.Append(", ");
							((WsdlParser.URTParam)this._params[i]).PrintCSC(sb, curNS);
						}
					}
					sb.Append(')');
				}
				if (this._wsdlMethodInfo.bProperty && WsdlParser.URTMethod.FlagTest(methodPrintEnum, WsdlParser.MethodPrintEnum.InterfaceMethods))
				{
					sb.Append("{");
					if (this._wsdlMethodInfo.bGet)
					{
						sb.Append(" get; ");
					}
					if (this._wsdlMethodInfo.bSet)
					{
						sb.Append(" set; ");
					}
					sb.Append("}");
				}
				else if (!WsdlParser.URTMethod.FlagTest(methodPrintEnum, WsdlParser.MethodPrintEnum.PrintBody))
				{
					sb.Append(';');
				}
				textWriter.WriteLine(sb);
				if (this._wsdlMethodInfo.bProperty && WsdlParser.URTMethod.FlagTest(methodPrintEnum, WsdlParser.MethodPrintEnum.PrintBody))
				{
					this.PrintPropertyBody(textWriter, indentation, sb, bodyPrefix);
					return;
				}
				if (WsdlParser.URTMethod.FlagTest(methodPrintEnum, WsdlParser.MethodPrintEnum.PrintBody))
				{
					sb.Length = 0;
					sb.Append(indentation);
					sb.Append('{');
					textWriter.WriteLine(sb);
					string text = indentation + "    ";
					if (bodyPrefix == null)
					{
						for (int j = 0; j < this._params.Count; j++)
						{
							WsdlParser.URTParam urtparam = (WsdlParser.URTParam)this._params[j];
							if (urtparam.ParamType == WsdlParser.URTParamType.OUT)
							{
								sb.Length = 0;
								sb.Append(text);
								sb.Append(WsdlParser.IsValidCS(urtparam.Name));
								sb.Append(" = ");
								sb.Append(WsdlParser.URTMethod.ValueString(urtparam.GetTypeString(curNS, true)));
								sb.Append(';');
								textWriter.WriteLine(sb);
							}
						}
						sb.Length = 0;
						sb.Append(text);
						sb.Append("return");
						string text2 = WsdlParser.URTMethod.ValueString(this.GetTypeString(curNS, true));
						if (text2 != null)
						{
							sb.Append('(');
							sb.Append(text2);
							sb.Append(')');
						}
						sb.Append(';');
					}
					else
					{
						sb.Length = 0;
						sb.Append(text);
						if (WsdlParser.URTMethod.ValueString(this.GetTypeString(curNS, true)) != null)
						{
							sb.Append("return ");
						}
						this.PrintMethodName(sb, bodyPrefix, this._methodName);
						sb.Append('(');
						if (this._params.Count > 0)
						{
							((WsdlParser.URTParam)this._params[0]).PrintCSC(sb);
							for (int k = 1; k < this._params.Count; k++)
							{
								sb.Append(", ");
								((WsdlParser.URTParam)this._params[k]).PrintCSC(sb);
							}
						}
						sb.Append(");");
					}
					textWriter.WriteLine(sb);
					textWriter.Write(indentation);
					textWriter.WriteLine('}');
				}
			}

			// Token: 0x0600041A RID: 1050 RVA: 0x0001739B File Offset: 0x0001639B
			private void PrintSoapAction(string action, StringBuilder sb)
			{
				sb.Append("[SoapMethod(SoapAction=");
				sb.Append(WsdlParser.IsValidUrl(action));
				sb.Append(")]");
			}

			// Token: 0x0600041B RID: 1051 RVA: 0x000173C4 File Offset: 0x000163C4
			private void PrintPropertyBody(TextWriter textWriter, string indentation, StringBuilder sb, string bodyPrefix)
			{
				sb.Length = 0;
				sb.Append(indentation);
				sb.Append('{');
				textWriter.WriteLine(sb);
				string text = indentation + "    ";
				sb.Length = 0;
				sb.Append(text);
				if (this._wsdlMethodInfo.bGet)
				{
					sb.Length = 0;
					sb.Append(text);
					this.PrintSoapAction(this._wsdlMethodInfo.soapActionGet, sb);
					textWriter.WriteLine(sb);
					sb.Length = 0;
					sb.Append(text);
					sb.Append("get{return ");
					this.PrintMethodName(sb, bodyPrefix, this._wsdlMethodInfo.propertyName);
					sb.Append(";}");
					textWriter.WriteLine(sb);
				}
				if (this._wsdlMethodInfo.bSet)
				{
					if (this._wsdlMethodInfo.bGet)
					{
						textWriter.WriteLine();
					}
					sb.Length = 0;
					sb.Append(text);
					this.PrintSoapAction(this._wsdlMethodInfo.soapActionSet, sb);
					textWriter.WriteLine(sb);
					sb.Length = 0;
					sb.Append(text);
					sb.Append("set{");
					this.PrintMethodName(sb, bodyPrefix, this._wsdlMethodInfo.propertyName);
					sb.Append("= value;}");
					textWriter.WriteLine(sb);
				}
				sb.Length = 0;
				sb.Append(indentation);
				sb.Append('}');
				textWriter.WriteLine(sb);
			}

			// Token: 0x0600041C RID: 1052 RVA: 0x0001752C File Offset: 0x0001652C
			private void PrintMethodName(StringBuilder sb, string bodyPrefix, string name)
			{
				int num = name.LastIndexOf('.');
				if (num < 0)
				{
					sb.Append(bodyPrefix);
					sb.Append(WsdlParser.IsValidCS(name));
					return;
				}
				string text = name.Substring(0, num);
				string text2 = name.Substring(num + 1);
				if (bodyPrefix == null)
				{
					sb.Append("(");
					sb.Append(WsdlParser.IsValidCS(text));
					sb.Append(")");
					sb.Append(WsdlParser.IsValidCS(text2));
					return;
				}
				sb.Append("((");
				sb.Append(WsdlParser.IsValidCS(text));
				sb.Append(") _tp).");
				sb.Append(WsdlParser.IsValidCS(text2));
			}

			// Token: 0x0600041D RID: 1053 RVA: 0x000175D8 File Offset: 0x000165D8
			internal static string ValueString(string paramType)
			{
				string text;
				if (paramType == "void")
				{
					text = null;
				}
				else if (paramType == "bool")
				{
					text = "false";
				}
				else if (paramType == "string")
				{
					text = "null";
				}
				else if (paramType == "sbyte" || paramType == "byte" || paramType == "short" || paramType == "ushort" || paramType == "int" || paramType == "uint" || paramType == "long" || paramType == "ulong")
				{
					text = "1";
				}
				else if (paramType == "float" || paramType == "exfloat")
				{
					text = "(float)1.0";
				}
				else if (paramType == "double" || paramType == "exdouble")
				{
					text = "1.0";
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder(50);
					stringBuilder.Append('(');
					stringBuilder.Append(WsdlParser.IsValidCS(paramType));
					stringBuilder.Append(") (Object) null");
					text = stringBuilder.ToString();
				}
				return text;
			}

			// Token: 0x0600041E RID: 1054
			internal abstract void ResolveTypes(WsdlParser parser);

			// Token: 0x0600041F RID: 1055 RVA: 0x00017710 File Offset: 0x00016710
			protected void ResolveWsdlParams(WsdlParser parser, string targetNS, string targetName, bool bRequest, WsdlParser.WsdlMethodInfo wsdlMethodInfo)
			{
				this._wsdlMethodInfo = wsdlMethodInfo;
				this._paramNamesOrder = this._wsdlMethodInfo.paramNamesOrder;
				int num;
				if (this._wsdlMethodInfo.bProperty)
				{
					num = 1;
				}
				else if (bRequest)
				{
					num = wsdlMethodInfo.inputNames.Length;
				}
				else
				{
					num = wsdlMethodInfo.outputNames.Length;
				}
				for (int i = 0; i < num; i++)
				{
					string text = null;
					string text2 = null;
					string text3 = null;
					string text4;
					string text5;
					WsdlParser.URTParamType urtparamType;
					if (this._wsdlMethodInfo.bProperty)
					{
						text4 = wsdlMethodInfo.propertyType;
						text5 = wsdlMethodInfo.propertyNs;
						urtparamType = WsdlParser.URTParamType.OUT;
					}
					else if (bRequest && !this._wsdlMethodInfo.bProperty)
					{
						text = wsdlMethodInfo.inputElements[i];
						text2 = wsdlMethodInfo.inputElementsNs[i];
						text3 = wsdlMethodInfo.inputNames[i];
						string text6 = wsdlMethodInfo.inputNamesNs[i];
						text4 = wsdlMethodInfo.inputTypes[i];
						text5 = wsdlMethodInfo.inputTypesNs[i];
						urtparamType = WsdlParser.URTParamType.IN;
					}
					else
					{
						text = wsdlMethodInfo.outputElements[i];
						text2 = wsdlMethodInfo.outputElementsNs[i];
						text3 = wsdlMethodInfo.outputNames[i];
						string text7 = wsdlMethodInfo.outputNamesNs[i];
						text4 = wsdlMethodInfo.outputTypes[i];
						text5 = wsdlMethodInfo.outputTypesNs[i];
						urtparamType = WsdlParser.URTParamType.OUT;
					}
					string text8;
					string text9;
					if (text == null || text.Length == 0)
					{
						text8 = text4;
						text9 = text5;
					}
					else
					{
						text8 = text;
						text9 = text2;
					}
					WsdlParser.URTNamespace urtnamespace = parser.LookupNamespace(text9);
					if (urtnamespace == null)
					{
						throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_CantResolveSchemaNS"), new object[] { text9, text8 }));
					}
					WsdlParser.URTComplexType urtcomplexType = urtnamespace.LookupComplexType(text8);
					if (urtcomplexType != null && urtcomplexType.IsArray())
					{
						if (urtcomplexType.GetArray() == null)
						{
							urtcomplexType.ResolveArray();
						}
						string array = urtcomplexType.GetArray();
						WsdlParser.URTNamespace arrayNS = urtcomplexType.GetArrayNS();
						this.AddParam(new WsdlParser.URTParam(text3, array, arrayNS.Name, arrayNS.EncodedNS, urtparamType, true, parser, arrayNS));
					}
					else if (urtnamespace.UrtType == UrtType.Xsd)
					{
						string text10 = parser.MapSchemaTypesToCSharpTypes(text8);
						this.AddParam(new WsdlParser.URTParam(text3, text10, urtnamespace.Namespace, urtnamespace.EncodedNS, urtparamType, true, parser, urtnamespace));
					}
					else
					{
						string text11;
						if (urtcomplexType != null)
						{
							text11 = urtcomplexType.Name;
						}
						else
						{
							WsdlParser.URTSimpleType urtsimpleType = urtnamespace.LookupSimpleType(text8);
							if (urtsimpleType != null)
							{
								text11 = urtsimpleType.Name;
							}
							else
							{
								text11 = text8;
							}
						}
						this.AddParam(new WsdlParser.URTParam(text3, text11, urtnamespace.Namespace, urtnamespace.EncodedNS, urtparamType, true, parser, urtnamespace));
					}
				}
			}

			// Token: 0x0400036F RID: 879
			private string _methodName;

			// Token: 0x04000370 RID: 880
			private string _soapAction;

			// Token: 0x04000371 RID: 881
			private WsdlParser.URTParam _methodType;

			// Token: 0x04000372 RID: 882
			internal WsdlParser.URTComplexType _complexType;

			// Token: 0x04000373 RID: 883
			protected string[] _paramNamesOrder;

			// Token: 0x04000374 RID: 884
			protected ArrayList _params = new ArrayList();

			// Token: 0x04000375 RID: 885
			protected ArrayList _paramPosition = new ArrayList();

			// Token: 0x04000376 RID: 886
			private WsdlParser.MethodFlags _methodFlags;

			// Token: 0x04000377 RID: 887
			private WsdlParser.WsdlMethodInfo _wsdlMethodInfo;
		}

		// Token: 0x02000083 RID: 131
		internal class RRMethod : WsdlParser.URTMethod
		{
			// Token: 0x06000420 RID: 1056 RVA: 0x00017985 File Offset: 0x00016985
			internal RRMethod(WsdlParser.WsdlMethodInfo wsdlMethodInfo, WsdlParser.URTComplexType complexType)
				: base(wsdlMethodInfo.methodName, wsdlMethodInfo.soapAction, wsdlMethodInfo.methodAttributes, complexType)
			{
				this._wsdlMethodInfo = wsdlMethodInfo;
				this._requestElementName = null;
				this._requestElementNS = null;
				this._responseElementName = null;
				this._responseElementNS = null;
			}

			// Token: 0x06000421 RID: 1057 RVA: 0x000179C3 File Offset: 0x000169C3
			internal void AddRequest(string name, string ns)
			{
				this._requestElementName = name;
				this._requestElementNS = ns;
			}

			// Token: 0x06000422 RID: 1058 RVA: 0x000179D3 File Offset: 0x000169D3
			internal void AddResponse(string name, string ns)
			{
				this._responseElementName = name;
				this._responseElementNS = ns;
			}

			// Token: 0x06000423 RID: 1059 RVA: 0x000179E4 File Offset: 0x000169E4
			internal override void ResolveTypes(WsdlParser parser)
			{
				base.ResolveWsdlParams(parser, this._requestElementNS, this._requestElementName, true, this._wsdlMethodInfo);
				base.ResolveWsdlParams(parser, this._responseElementNS, this._responseElementName, false, this._wsdlMethodInfo);
				if (this._paramNamesOrder != null)
				{
					object[] array = new object[this._params.Count];
					for (int i = 0; i < this._params.Count; i++)
					{
						array[(int)this._paramPosition[i]] = this._params[i];
					}
					this._params = new ArrayList(array);
				}
				base.ResolveMethodAttributes();
			}

			// Token: 0x06000424 RID: 1060 RVA: 0x00017A88 File Offset: 0x00016A88
			internal override void PrintCSC(TextWriter textWriter, string indentation, string namePrefix, string curNS, WsdlParser.MethodPrintEnum methodPrintEnum, bool bURTType, string bodyPrefix, StringBuilder sb)
			{
				if (base.Name == "Finalize")
				{
					return;
				}
				bool flag = false;
				if (base.SoapAction != null)
				{
					flag = true;
				}
				if ((flag || !bURTType) && !this._wsdlMethodInfo.bProperty)
				{
					sb.Length = 0;
					sb.Append(indentation);
					sb.Append("[SoapMethod(");
					if (flag)
					{
						sb.Append("SoapAction=");
						sb.Append(WsdlParser.IsValidUrl(base.SoapAction));
					}
					if (!bURTType)
					{
						if (flag)
						{
							sb.Append(",");
						}
						sb.Append("ResponseXmlElementName=");
						sb.Append(WsdlParser.IsValidUrl(this._responseElementName));
						if (base.MethodType != null)
						{
							sb.Append(", ReturnXmlElementName=");
							sb.Append(WsdlParser.IsValidUrl(base.MethodType.Name));
						}
						sb.Append(", XmlNamespace=");
						sb.Append(WsdlParser.IsValidUrl(this._wsdlMethodInfo.inputMethodNameNs));
						sb.Append(", ResponseXmlNamespace=");
						sb.Append(WsdlParser.IsValidUrl(this._wsdlMethodInfo.outputMethodNameNs));
					}
					sb.Append(")]");
					textWriter.WriteLine(sb);
				}
				base.PrintCSC(textWriter, indentation, namePrefix, curNS, methodPrintEnum, bURTType, bodyPrefix, sb);
			}

			// Token: 0x04000378 RID: 888
			private string _requestElementName;

			// Token: 0x04000379 RID: 889
			private string _requestElementNS;

			// Token: 0x0400037A RID: 890
			private string _responseElementName;

			// Token: 0x0400037B RID: 891
			private string _responseElementNS;

			// Token: 0x0400037C RID: 892
			private WsdlParser.WsdlMethodInfo _wsdlMethodInfo;
		}

		// Token: 0x02000084 RID: 132
		internal class OnewayMethod : WsdlParser.URTMethod
		{
			// Token: 0x06000425 RID: 1061 RVA: 0x00017BE5 File Offset: 0x00016BE5
			internal OnewayMethod(string name, string soapAction, WsdlParser.URTComplexType complexType)
				: base(name, soapAction, null, complexType)
			{
				this._messageElementName = null;
				this._messageElementNS = null;
			}

			// Token: 0x06000426 RID: 1062 RVA: 0x00017BFF File Offset: 0x00016BFF
			internal OnewayMethod(WsdlParser.WsdlMethodInfo wsdlMethodInfo, WsdlParser.URTComplexType complexType)
				: base(wsdlMethodInfo.methodName, wsdlMethodInfo.soapAction, wsdlMethodInfo.methodAttributes, complexType)
			{
				this._wsdlMethodInfo = wsdlMethodInfo;
				this._messageElementName = null;
				this._messageElementNS = null;
			}

			// Token: 0x06000427 RID: 1063 RVA: 0x00017C2F File Offset: 0x00016C2F
			internal void AddMessage(string name, string ns)
			{
				this._messageElementName = name;
				this._messageElementNS = ns;
			}

			// Token: 0x06000428 RID: 1064 RVA: 0x00017C40 File Offset: 0x00016C40
			internal override void ResolveTypes(WsdlParser parser)
			{
				base.ResolveWsdlParams(parser, this._messageElementNS, this._messageElementName, true, this._wsdlMethodInfo);
				if (this._paramNamesOrder != null)
				{
					object[] array = new object[this._params.Count];
					for (int i = 0; i < this._params.Count; i++)
					{
						array[(int)this._paramPosition[i]] = this._params[i];
					}
					this._params = new ArrayList(array);
				}
				base.ResolveMethodAttributes();
			}

			// Token: 0x06000429 RID: 1065 RVA: 0x00017CC8 File Offset: 0x00016CC8
			internal override void PrintCSC(TextWriter textWriter, string indentation, string namePrefix, string curNS, WsdlParser.MethodPrintEnum methodPrintEnum, bool bURTType, string bodyPrefix, StringBuilder sb)
			{
				if (base.Name == "Finalize")
				{
					return;
				}
				bool flag = false;
				if (base.SoapAction != null)
				{
					flag = true;
				}
				if (!flag && bURTType)
				{
					textWriter.Write(indentation);
					textWriter.WriteLine("[OneWay]");
				}
				else
				{
					sb.Length = 0;
					sb.Append(indentation);
					sb.Append("[OneWay, SoapMethod(");
					if (flag)
					{
						sb.Append("SoapAction=");
						sb.Append(WsdlParser.IsValidUrl(base.SoapAction));
					}
					if (!bURTType)
					{
						if (flag)
						{
							sb.Append(",");
						}
						sb.Append("XmlNamespace=");
						sb.Append(WsdlParser.IsValidUrl(this._wsdlMethodInfo.inputMethodNameNs));
					}
					sb.Append(")]");
					textWriter.WriteLine(sb);
				}
				base.PrintCSC(textWriter, indentation, namePrefix, curNS, methodPrintEnum, bURTType, bodyPrefix, sb);
			}

			// Token: 0x0400037D RID: 893
			private string _messageElementName;

			// Token: 0x0400037E RID: 894
			private string _messageElementNS;

			// Token: 0x0400037F RID: 895
			private WsdlParser.WsdlMethodInfo _wsdlMethodInfo;
		}

		// Token: 0x02000085 RID: 133
		internal abstract class BaseInterface
		{
			// Token: 0x0600042A RID: 1066 RVA: 0x00017DB4 File Offset: 0x00016DB4
			internal BaseInterface(string name, string urlNS, string ns, string encodedNS, WsdlParser parser)
			{
				this._name = name;
				this._urlNS = urlNS;
				this._namespace = ns;
				this._encodedNS = encodedNS;
				this._parser = parser;
			}

			// Token: 0x170000E9 RID: 233
			// (get) Token: 0x0600042B RID: 1067 RVA: 0x00017DE1 File Offset: 0x00016DE1
			internal string Name
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x170000EA RID: 234
			// (get) Token: 0x0600042C RID: 1068 RVA: 0x00017DE9 File Offset: 0x00016DE9
			internal string UrlNS
			{
				get
				{
					return this._urlNS;
				}
			}

			// Token: 0x170000EB RID: 235
			// (get) Token: 0x0600042D RID: 1069 RVA: 0x00017DF1 File Offset: 0x00016DF1
			internal string Namespace
			{
				get
				{
					return this._namespace;
				}
			}

			// Token: 0x170000EC RID: 236
			// (get) Token: 0x0600042E RID: 1070 RVA: 0x00017DF9 File Offset: 0x00016DF9
			internal bool IsURTInterface
			{
				get
				{
					return this._namespace == this._encodedNS;
				}
			}

			// Token: 0x0600042F RID: 1071 RVA: 0x00017E0C File Offset: 0x00016E0C
			internal string GetName(string curNS)
			{
				string text;
				if (this._parser.Qualify(this._namespace, curNS))
				{
					StringBuilder stringBuilder = new StringBuilder(this._encodedNS, 50);
					stringBuilder.Append('.');
					stringBuilder.Append(WsdlParser.IsValidCS(this._name));
					text = stringBuilder.ToString();
				}
				else
				{
					text = this._name;
				}
				return text;
			}

			// Token: 0x06000430 RID: 1072
			internal abstract void PrintClassMethods(TextWriter textWriter, string indentation, string curNS, ArrayList printedIFaces, bool bProxy, StringBuilder sb);

			// Token: 0x04000380 RID: 896
			private string _name;

			// Token: 0x04000381 RID: 897
			private string _urlNS;

			// Token: 0x04000382 RID: 898
			private string _namespace;

			// Token: 0x04000383 RID: 899
			private string _encodedNS;

			// Token: 0x04000384 RID: 900
			private WsdlParser _parser;
		}

		// Token: 0x02000086 RID: 134
		internal class SystemInterface : WsdlParser.BaseInterface
		{
			// Token: 0x06000431 RID: 1073 RVA: 0x00017E68 File Offset: 0x00016E68
			internal SystemInterface(string name, string urlNS, string ns, WsdlParser parser, string assemName)
				: base(name, urlNS, ns, ns, parser)
			{
				string text = ns + '.' + name;
				Assembly assembly;
				if (assemName == null)
				{
					assembly = typeof(string).Assembly;
				}
				else
				{
					assembly = Assembly.LoadWithPartialName(assemName, null);
				}
				if (assembly == null)
				{
					throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_AssemblyNotFound"), new object[] { assemName }));
				}
				this._type = assembly.GetType(text, true);
			}

			// Token: 0x06000432 RID: 1074 RVA: 0x00017EEC File Offset: 0x00016EEC
			internal override void PrintClassMethods(TextWriter textWriter, string indentation, string curNS, ArrayList printedIFaces, bool bProxy, StringBuilder sb)
			{
				int i;
				for (i = 0; i < printedIFaces.Count; i++)
				{
					if (printedIFaces[i] is WsdlParser.SystemInterface)
					{
						WsdlParser.SystemInterface systemInterface = (WsdlParser.SystemInterface)printedIFaces[i];
						if (systemInterface._type == this._type)
						{
							return;
						}
					}
				}
				printedIFaces.Add(this);
				BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
				ArrayList arrayList = new ArrayList();
				sb.Length = 0;
				arrayList.Add(this._type);
				i = 0;
				int num = 1;
				while (i < num)
				{
					Type type = (Type)arrayList[i];
					MethodInfo[] methods = type.GetMethods(bindingFlags);
					Type[] interfaces = type.GetInterfaces();
					int j = 0;
					IL_00CC:
					while (j < interfaces.Length)
					{
						for (int k = 0; k < num; k++)
						{
							if (arrayList[i] == interfaces[j])
							{
								IL_00C6:
								j++;
								goto IL_00CC;
							}
						}
						arrayList.Add(interfaces[j]);
						num++;
						goto IL_00C6;
					}
					foreach (MethodInfo methodInfo in methods)
					{
						sb.Length = 0;
						sb.Append(indentation);
						sb.Append(WsdlParser.SystemInterface.CSharpTypeString(methodInfo.ReturnType.FullName));
						sb.Append(' ');
						sb.Append(WsdlParser.IsValidCS(type.FullName));
						sb.Append('.');
						sb.Append(WsdlParser.IsValidCS(methodInfo.Name));
						sb.Append('(');
						ParameterInfo[] parameters = methodInfo.GetParameters();
						for (int m = 0; m < parameters.Length; m++)
						{
							if (m != 0)
							{
								sb.Append(", ");
							}
							ParameterInfo parameterInfo = parameters[m];
							Type type2 = parameterInfo.ParameterType;
							if (parameterInfo.IsIn)
							{
								sb.Append("in ");
							}
							else if (parameterInfo.IsOut)
							{
								sb.Append("out ");
							}
							else if (type2.IsByRef)
							{
								sb.Append("ref ");
								type2 = type2.GetElementType();
							}
							sb.Append(WsdlParser.SystemInterface.CSharpTypeString(type2.FullName));
							sb.Append(' ');
							sb.Append(WsdlParser.IsValidCS(parameterInfo.Name));
						}
						sb.Append(')');
						textWriter.WriteLine(sb);
						textWriter.Write(indentation);
						textWriter.WriteLine('{');
						string text = indentation + "    ";
						if (!bProxy)
						{
							foreach (ParameterInfo parameterInfo2 in parameters)
							{
								Type parameterType = parameterInfo2.ParameterType;
								if (parameterInfo2.IsOut)
								{
									sb.Length = 0;
									sb.Append(text);
									sb.Append(WsdlParser.IsValidCS(parameterInfo2.Name));
									sb.Append(WsdlParser.URTMethod.ValueString(WsdlParser.SystemInterface.CSharpTypeString(parameterInfo2.ParameterType.FullName)));
									sb.Append(';');
									textWriter.WriteLine(sb);
								}
							}
							sb.Length = 0;
							sb.Append(text);
							sb.Append("return");
							string text2 = WsdlParser.URTMethod.ValueString(WsdlParser.SystemInterface.CSharpTypeString(methodInfo.ReturnType.FullName));
							if (text2 != null)
							{
								sb.Append('(');
								sb.Append(text2);
								sb.Append(')');
							}
							sb.Append(';');
						}
						else
						{
							sb.Length = 0;
							sb.Append(text);
							sb.Append("return((");
							sb.Append(WsdlParser.IsValidCS(type.FullName));
							sb.Append(") _tp).");
							sb.Append(WsdlParser.IsValidCS(methodInfo.Name));
							sb.Append('(');
							if (parameters.Length > 0)
							{
								int num2 = parameters.Length - 1;
								for (int num3 = 0; num3 < parameters.Length; num3++)
								{
									ParameterInfo parameterInfo3 = parameters[0];
									Type parameterType2 = parameterInfo3.ParameterType;
									if (parameterInfo3.IsIn)
									{
										sb.Append("in ");
									}
									else if (parameterInfo3.IsOut)
									{
										sb.Append("out ");
									}
									else if (parameterType2.IsByRef)
									{
										sb.Append("ref ");
									}
									sb.Append(WsdlParser.IsValidCS(parameterInfo3.Name));
									if (num3 < num2)
									{
										sb.Append(", ");
									}
								}
							}
							sb.Append(");");
						}
						textWriter.WriteLine(sb);
						textWriter.Write(indentation);
						textWriter.WriteLine('}');
					}
					i++;
				}
			}

			// Token: 0x06000433 RID: 1075 RVA: 0x00018374 File Offset: 0x00017374
			private static string CSharpTypeString(string typeName)
			{
				string text = typeName;
				if (typeName == "System.SByte")
				{
					text = "sbyte";
				}
				else if (typeName == "System.byte")
				{
					text = "byte";
				}
				else if (typeName == "System.Int16")
				{
					text = "short";
				}
				else if (typeName == "System.UInt16")
				{
					text = "ushort";
				}
				else if (typeName == "System.Int32")
				{
					text = "int";
				}
				else if (typeName == "System.UInt32")
				{
					text = "uint";
				}
				else if (typeName == "System.Int64")
				{
					text = "long";
				}
				else if (typeName == "System.UInt64")
				{
					text = "ulong";
				}
				else if (typeName == "System.Char")
				{
					text = "char";
				}
				else if (typeName == "System.Single")
				{
					text = "float";
				}
				else if (typeName == "System.Double")
				{
					text = "double";
				}
				else if (typeName == "System.Boolean")
				{
					text = "boolean";
				}
				else if (typeName == "System.Void")
				{
					text = "void";
				}
				else if (typeName == "System.String")
				{
					text = "String";
				}
				return WsdlParser.IsValidCSAttr(text);
			}

			// Token: 0x04000385 RID: 901
			private Type _type;
		}

		// Token: 0x02000087 RID: 135
		internal class URTInterface : WsdlParser.BaseInterface
		{
			// Token: 0x06000434 RID: 1076 RVA: 0x000184C4 File Offset: 0x000174C4
			internal URTInterface(string name, string urlNS, string ns, string encodedNS, WsdlParser parser)
				: base(name, urlNS, ns, encodedNS, parser)
			{
				this._baseIFaces = new ArrayList();
				this._baseIFaceNames = new ArrayList();
				this._extendsInterface = new ArrayList();
				this._methods = new ArrayList();
				this._parser = parser;
			}

			// Token: 0x06000435 RID: 1077 RVA: 0x00018514 File Offset: 0x00017514
			internal void Extends(string baseName, string baseNS, WsdlParser parser)
			{
				this._baseIFaceNames.Add(baseName);
				this._baseIFaceNames.Add(baseNS);
				WsdlParser.URTNamespace urtnamespace = parser.AddNewNamespace(baseNS);
				WsdlParser.URTInterface urtinterface = urtnamespace.LookupInterface(baseName);
				if (urtinterface == null)
				{
					urtinterface = new WsdlParser.URTInterface(baseName, urtnamespace.Name, urtnamespace.Namespace, urtnamespace.EncodedNS, parser);
					urtnamespace.AddInterface(urtinterface);
				}
				this._extendsInterface.Add(urtinterface);
			}

			// Token: 0x06000436 RID: 1078 RVA: 0x0001857C File Offset: 0x0001757C
			internal void AddMethod(WsdlParser.URTMethod method)
			{
				this._methods.Add(method);
				method.MethodFlags = WsdlParser.MethodFlags.None;
			}

			// Token: 0x06000437 RID: 1079 RVA: 0x00018594 File Offset: 0x00017594
			internal void NewNeeded(WsdlParser.URTMethod method)
			{
				foreach (object obj in this._extendsInterface)
				{
					WsdlParser.URTInterface urtinterface = (WsdlParser.URTInterface)obj;
					urtinterface.CheckIfNewNeeded(method);
					if (WsdlParser.URTMethod.MethodFlagsTest(method.MethodFlags, WsdlParser.MethodFlags.New))
					{
						break;
					}
				}
			}

			// Token: 0x06000438 RID: 1080 RVA: 0x000185FC File Offset: 0x000175FC
			private void CheckIfNewNeeded(WsdlParser.URTMethod method)
			{
				foreach (object obj in this._methods)
				{
					WsdlParser.URTMethod urtmethod = (WsdlParser.URTMethod)obj;
					if (urtmethod.Name == method.Name)
					{
						method.MethodFlags |= WsdlParser.MethodFlags.New;
						break;
					}
				}
				if (WsdlParser.URTMethod.MethodFlagsTest(method.MethodFlags, WsdlParser.MethodFlags.New))
				{
					this.NewNeeded(method);
				}
			}

			// Token: 0x06000439 RID: 1081 RVA: 0x00018688 File Offset: 0x00017688
			internal void ResolveTypes(WsdlParser parser)
			{
				for (int i = 0; i < this._baseIFaceNames.Count; i += 2)
				{
					string text = (string)this._baseIFaceNames[i];
					string text2 = (string)this._baseIFaceNames[i + 1];
					string text3;
					string text4;
					UrtType urtType = parser.IsURTExportedType(text2, out text3, out text4);
					WsdlParser.BaseInterface baseInterface;
					if (urtType != UrtType.Interop && text3.StartsWith("System", StringComparison.Ordinal))
					{
						baseInterface = new WsdlParser.SystemInterface(text, text2, text3, this._parser, text4);
					}
					else
					{
						WsdlParser.URTNamespace urtnamespace = parser.LookupNamespace(text2);
						if (urtnamespace == null)
						{
							throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_CantResolveSchemaNS"), new object[] { text2, text }));
						}
						baseInterface = urtnamespace.LookupInterface(text);
						if (baseInterface == null)
						{
							throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_CantResolveTypeInNS"), new object[] { text, text2 }));
						}
					}
					this._baseIFaces.Add(baseInterface);
				}
				for (int j = 0; j < this._methods.Count; j++)
				{
					((WsdlParser.URTMethod)this._methods[j]).ResolveTypes(parser);
				}
			}

			// Token: 0x0600043A RID: 1082 RVA: 0x000187C4 File Offset: 0x000177C4
			internal void PrintCSC(TextWriter textWriter, string indentation, string curNS, StringBuilder sb)
			{
				bool isURTInterface = base.IsURTInterface;
				sb.Length = 0;
				sb.Append("\n");
				sb.Append(indentation);
				sb.Append("[SoapType(");
				if (this._parser._xsdVersion == XsdVersion.V1999)
				{
					sb.Append("SoapOptions=SoapOption.Option1|SoapOption.AlwaysIncludeTypes|SoapOption.XsdString|SoapOption.EmbedAll,");
				}
				else if (this._parser._xsdVersion == XsdVersion.V2000)
				{
					sb.Append("SoapOptions=SoapOption.Option2|SoapOption.AlwaysIncludeTypes|SoapOption.XsdString|SoapOption.EmbedAll,");
				}
				if (!isURTInterface)
				{
					sb.Append("XmlElementName=");
					sb.Append(WsdlParser.IsValidUrl(base.Name));
					sb.Append(", XmlNamespace=");
					sb.Append(WsdlParser.IsValidUrl(base.Namespace));
					sb.Append(", XmlTypeName=");
					sb.Append(WsdlParser.IsValidUrl(base.Name));
					sb.Append(", XmlTypeNamespace=");
					sb.Append(WsdlParser.IsValidUrl(base.Namespace));
				}
				else
				{
					sb.Append("XmlNamespace=");
					sb.Append(WsdlParser.IsValidUrl(base.UrlNS));
					sb.Append(", XmlTypeNamespace=");
					sb.Append(WsdlParser.IsValidUrl(base.UrlNS));
				}
				sb.Append(")]");
				sb.Append("[ComVisible(true)]");
				textWriter.WriteLine(sb);
				sb.Length = 0;
				sb.Append(indentation);
				sb.Append("public interface ");
				sb.Append(WsdlParser.IsValidCS(base.Name));
				if (this._baseIFaces.Count > 0)
				{
					sb.Append(" : ");
				}
				if (this._baseIFaces.Count > 0)
				{
					sb.Append(WsdlParser.IsValidCSAttr(((WsdlParser.BaseInterface)this._baseIFaces[0]).GetName(curNS)));
					for (int i = 1; i < this._baseIFaces.Count; i++)
					{
						sb.Append(", ");
						sb.Append(WsdlParser.IsValidCSAttr(((WsdlParser.BaseInterface)this._baseIFaces[i]).GetName(curNS)));
					}
				}
				textWriter.WriteLine(sb);
				textWriter.Write(indentation);
				textWriter.WriteLine('{');
				string text = indentation + "    ";
				string text2 = " ";
				for (int j = 0; j < this._methods.Count; j++)
				{
					this.NewNeeded((WsdlParser.URTMethod)this._methods[j]);
					((WsdlParser.URTMethod)this._methods[j]).PrintCSC(textWriter, text, text2, curNS, WsdlParser.MethodPrintEnum.InterfaceMethods, isURTInterface, null, sb);
				}
				textWriter.Write(indentation);
				textWriter.WriteLine('}');
			}

			// Token: 0x0600043B RID: 1083 RVA: 0x00018A74 File Offset: 0x00017A74
			internal override void PrintClassMethods(TextWriter textWriter, string indentation, string curNS, ArrayList printedIFaces, bool bProxy, StringBuilder sb)
			{
				for (int i = 0; i < printedIFaces.Count; i++)
				{
					if (printedIFaces[i] == this)
					{
						return;
					}
				}
				printedIFaces.Add(this);
				sb.Length = 0;
				sb.Append(indentation);
				if (this._methods.Count > 0)
				{
					sb.Append("// ");
					sb.Append(WsdlParser.IsValidCS(base.Name));
					sb.Append(" interface Methods");
					textWriter.WriteLine(sb);
					sb.Length = 0;
					sb.Append(' ');
					string name = base.GetName(curNS);
					sb.Append(WsdlParser.IsValidCS(name));
					sb.Append('.');
					string text = sb.ToString();
					string text2 = null;
					if (bProxy)
					{
						sb.Length = 0;
						sb.Append("((");
						sb.Append(WsdlParser.IsValidCS(name));
						sb.Append(") _tp).");
						text2 = sb.ToString();
					}
					WsdlParser.MethodPrintEnum methodPrintEnum = WsdlParser.MethodPrintEnum.PrintBody | WsdlParser.MethodPrintEnum.InterfaceInClass;
					for (int j = 0; j < this._methods.Count; j++)
					{
						((WsdlParser.URTMethod)this._methods[j]).PrintCSC(textWriter, indentation, text, curNS, methodPrintEnum, true, text2, sb);
					}
				}
				for (int k = 0; k < this._baseIFaces.Count; k++)
				{
					((WsdlParser.BaseInterface)this._baseIFaces[k]).PrintClassMethods(textWriter, indentation, curNS, printedIFaces, bProxy, sb);
				}
			}

			// Token: 0x04000386 RID: 902
			private WsdlParser _parser;

			// Token: 0x04000387 RID: 903
			private ArrayList _baseIFaces;

			// Token: 0x04000388 RID: 904
			private ArrayList _baseIFaceNames;

			// Token: 0x04000389 RID: 905
			private ArrayList _methods;

			// Token: 0x0400038A RID: 906
			private ArrayList _extendsInterface;
		}

		// Token: 0x02000088 RID: 136
		internal class URTField
		{
			// Token: 0x0600043C RID: 1084 RVA: 0x00018BF0 File Offset: 0x00017BF0
			internal URTField(string name, string typeName, string xmlNS, WsdlParser parser, bool bPrimitive, bool bEmbedded, bool bAttribute, bool bOptional, bool bArray, string arraySize, WsdlParser.URTNamespace urtNamespace)
			{
				this._name = name;
				this._typeName = typeName;
				this._parser = parser;
				string text;
				UrtType urtType = parser.IsURTExportedType(xmlNS, out this._typeNS, out text);
				if (urtType == UrtType.Interop)
				{
					this._encodedNS = urtNamespace.EncodedNS;
				}
				else
				{
					this._encodedNS = this._typeNS;
				}
				this._primitiveField = bPrimitive;
				this._embeddedField = bEmbedded;
				this._attributeField = bAttribute;
				this._optionalField = bOptional;
				this._arrayField = bArray;
				this._arraySize = arraySize;
				this._urtNamespace = urtNamespace;
			}

			// Token: 0x170000ED RID: 237
			// (get) Token: 0x0600043D RID: 1085 RVA: 0x00018C81 File Offset: 0x00017C81
			internal string TypeName
			{
				get
				{
					if (this._arrayField)
					{
						return this._typeName + "[]";
					}
					return this._typeName;
				}
			}

			// Token: 0x170000EE RID: 238
			// (get) Token: 0x0600043E RID: 1086 RVA: 0x00018CA2 File Offset: 0x00017CA2
			internal string TypeNS
			{
				get
				{
					return this._typeNS;
				}
			}

			// Token: 0x170000EF RID: 239
			// (get) Token: 0x0600043F RID: 1087 RVA: 0x00018CAA File Offset: 0x00017CAA
			internal bool IsPrimitive
			{
				get
				{
					return this._primitiveField;
				}
			}

			// Token: 0x170000F0 RID: 240
			// (get) Token: 0x06000440 RID: 1088 RVA: 0x00018CB2 File Offset: 0x00017CB2
			internal bool IsArray
			{
				get
				{
					return this._arrayField;
				}
			}

			// Token: 0x06000441 RID: 1089 RVA: 0x00018CBA File Offset: 0x00017CBA
			internal string GetTypeString(string curNS, bool bNS)
			{
				return this._parser.GetTypeString(curNS, bNS, this._urtNamespace, this.TypeName, this._typeNS);
			}

			// Token: 0x06000442 RID: 1090 RVA: 0x00018CDC File Offset: 0x00017CDC
			internal void PrintCSC(TextWriter textWriter, string indentation, string curNS, StringBuilder sb)
			{
				if (this._embeddedField)
				{
					textWriter.Write(indentation);
					textWriter.WriteLine("[SoapField(Embedded=true)]");
				}
				sb.Length = 0;
				sb.Append(indentation);
				sb.Append("public ");
				sb.Append(WsdlParser.IsValidCSAttr(this.GetTypeString(curNS, true)));
				sb.Append(' ');
				sb.Append(WsdlParser.IsValidCS(this._name));
				sb.Append(';');
				textWriter.WriteLine(sb);
			}

			// Token: 0x0400038B RID: 907
			private string _name;

			// Token: 0x0400038C RID: 908
			private string _typeName;

			// Token: 0x0400038D RID: 909
			private string _typeNS;

			// Token: 0x0400038E RID: 910
			private string _encodedNS;

			// Token: 0x0400038F RID: 911
			private bool _primitiveField;

			// Token: 0x04000390 RID: 912
			private bool _embeddedField;

			// Token: 0x04000391 RID: 913
			private bool _attributeField;

			// Token: 0x04000392 RID: 914
			private bool _optionalField;

			// Token: 0x04000393 RID: 915
			private bool _arrayField;

			// Token: 0x04000394 RID: 916
			private string _arraySize;

			// Token: 0x04000395 RID: 917
			private WsdlParser _parser;

			// Token: 0x04000396 RID: 918
			private WsdlParser.URTNamespace _urtNamespace;
		}

		// Token: 0x02000089 RID: 137
		internal abstract class SchemaFacet
		{
			// Token: 0x06000444 RID: 1092 RVA: 0x00018D6D File Offset: 0x00017D6D
			internal virtual void ResolveTypes(WsdlParser parser)
			{
			}

			// Token: 0x06000445 RID: 1093
			internal abstract void PrintCSC(TextWriter textWriter, string newIndentation, string curNS, StringBuilder sb);
		}

		// Token: 0x0200008A RID: 138
		internal class EnumFacet : WsdlParser.SchemaFacet
		{
			// Token: 0x06000446 RID: 1094 RVA: 0x00018D6F File Offset: 0x00017D6F
			internal EnumFacet(string valueString, int value)
			{
				this._valueString = valueString;
				this._value = value;
			}

			// Token: 0x06000447 RID: 1095 RVA: 0x00018D88 File Offset: 0x00017D88
			internal override void PrintCSC(TextWriter textWriter, string newIndentation, string curNS, StringBuilder sb)
			{
				sb.Length = 0;
				sb.Append(newIndentation);
				sb.Append(WsdlParser.IsValidCS(this._valueString));
				sb.Append(" = ");
				sb.Append(this._value);
				sb.Append(',');
				textWriter.WriteLine(sb);
			}

			// Token: 0x04000397 RID: 919
			private string _valueString;

			// Token: 0x04000398 RID: 920
			private int _value;
		}

		// Token: 0x0200008B RID: 139
		internal abstract class BaseType
		{
			// Token: 0x06000448 RID: 1096 RVA: 0x00018DE6 File Offset: 0x00017DE6
			internal BaseType(string name, string urlNS, string ns, string encodedNS)
			{
				this._searchName = name;
				this._name = name;
				this._urlNS = urlNS;
				this._namespace = ns;
				this._elementName = this._name;
				this._elementNS = ns;
				this._encodedNS = encodedNS;
			}

			// Token: 0x170000F1 RID: 241
			// (get) Token: 0x06000449 RID: 1097 RVA: 0x00018E25 File Offset: 0x00017E25
			// (set) Token: 0x0600044A RID: 1098 RVA: 0x00018E2D File Offset: 0x00017E2D
			internal string Name
			{
				get
				{
					return this._name;
				}
				set
				{
					this._name = value;
				}
			}

			// Token: 0x170000F2 RID: 242
			// (get) Token: 0x0600044B RID: 1099 RVA: 0x00018E36 File Offset: 0x00017E36
			// (set) Token: 0x0600044C RID: 1100 RVA: 0x00018E3E File Offset: 0x00017E3E
			internal string SearchName
			{
				get
				{
					return this._searchName;
				}
				set
				{
					this._searchName = value;
				}
			}

			// Token: 0x170000F3 RID: 243
			// (set) Token: 0x0600044D RID: 1101 RVA: 0x00018E47 File Offset: 0x00017E47
			internal string OuterTypeName
			{
				set
				{
					this._outerTypeName = value;
				}
			}

			// Token: 0x170000F4 RID: 244
			// (get) Token: 0x0600044E RID: 1102 RVA: 0x00018E50 File Offset: 0x00017E50
			// (set) Token: 0x0600044F RID: 1103 RVA: 0x00018E58 File Offset: 0x00017E58
			internal string NestedTypeName
			{
				get
				{
					return this._nestedTypeName;
				}
				set
				{
					this._nestedTypeName = value;
				}
			}

			// Token: 0x170000F5 RID: 245
			// (set) Token: 0x06000450 RID: 1104 RVA: 0x00018E61 File Offset: 0x00017E61
			internal string FullNestedTypeName
			{
				set
				{
					this._fullNestedTypeName = value;
				}
			}

			// Token: 0x170000F6 RID: 246
			// (get) Token: 0x06000451 RID: 1105 RVA: 0x00018E6A File Offset: 0x00017E6A
			// (set) Token: 0x06000452 RID: 1106 RVA: 0x00018E72 File Offset: 0x00017E72
			internal bool bNestedType
			{
				get
				{
					return this._bNestedType;
				}
				set
				{
					this._bNestedType = value;
				}
			}

			// Token: 0x170000F7 RID: 247
			// (get) Token: 0x06000453 RID: 1107 RVA: 0x00018E7B File Offset: 0x00017E7B
			// (set) Token: 0x06000454 RID: 1108 RVA: 0x00018E83 File Offset: 0x00017E83
			internal bool bNestedTypePrint
			{
				get
				{
					return this._bNestedTypePrint;
				}
				set
				{
					this._bNestedTypePrint = value;
				}
			}

			// Token: 0x170000F8 RID: 248
			// (get) Token: 0x06000455 RID: 1109 RVA: 0x00018E8C File Offset: 0x00017E8C
			internal string UrlNS
			{
				get
				{
					return this._urlNS;
				}
			}

			// Token: 0x170000F9 RID: 249
			// (get) Token: 0x06000456 RID: 1110 RVA: 0x00018E94 File Offset: 0x00017E94
			internal string Namespace
			{
				get
				{
					return this._namespace;
				}
			}

			// Token: 0x170000FA RID: 250
			// (set) Token: 0x06000457 RID: 1111 RVA: 0x00018E9C File Offset: 0x00017E9C
			internal string ElementName
			{
				set
				{
					this._elementName = value;
				}
			}

			// Token: 0x170000FB RID: 251
			// (set) Token: 0x06000458 RID: 1112 RVA: 0x00018EA5 File Offset: 0x00017EA5
			internal string ElementNS
			{
				set
				{
					this._elementNS = value;
				}
			}

			// Token: 0x170000FC RID: 252
			// (get) Token: 0x06000459 RID: 1113 RVA: 0x00018EAE File Offset: 0x00017EAE
			internal bool IsURTType
			{
				get
				{
					return this._namespace == this._encodedNS;
				}
			}

			// Token: 0x0600045A RID: 1114 RVA: 0x00018EC0 File Offset: 0x00017EC0
			internal virtual string GetName(string curNS)
			{
				string text;
				if (WsdlParser.MatchingStrings(this._namespace, curNS))
				{
					text = this._name;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder(this._encodedNS, 50);
					stringBuilder.Append('.');
					stringBuilder.Append(WsdlParser.IsValidCS(this._name));
					text = stringBuilder.ToString();
				}
				return text;
			}

			// Token: 0x0600045B RID: 1115
			internal abstract WsdlParser.MethodFlags GetMethodFlags(WsdlParser.URTMethod method);

			// Token: 0x170000FD RID: 253
			// (get) Token: 0x0600045C RID: 1116
			internal abstract bool IsEmittableFieldType { get; }

			// Token: 0x170000FE RID: 254
			// (get) Token: 0x0600045D RID: 1117
			internal abstract string FieldName { get; }

			// Token: 0x170000FF RID: 255
			// (get) Token: 0x0600045E RID: 1118
			internal abstract string FieldNamespace { get; }

			// Token: 0x17000100 RID: 256
			// (get) Token: 0x0600045F RID: 1119
			internal abstract bool PrimitiveField { get; }

			// Token: 0x04000399 RID: 921
			private string _name;

			// Token: 0x0400039A RID: 922
			private string _searchName;

			// Token: 0x0400039B RID: 923
			private string _urlNS;

			// Token: 0x0400039C RID: 924
			private string _namespace;

			// Token: 0x0400039D RID: 925
			private string _elementName;

			// Token: 0x0400039E RID: 926
			private string _elementNS;

			// Token: 0x0400039F RID: 927
			private string _encodedNS;

			// Token: 0x040003A0 RID: 928
			internal ArrayList _nestedTypes;

			// Token: 0x040003A1 RID: 929
			internal string _nestedTypeName;

			// Token: 0x040003A2 RID: 930
			internal string _fullNestedTypeName;

			// Token: 0x040003A3 RID: 931
			internal string _outerTypeName;

			// Token: 0x040003A4 RID: 932
			internal bool _bNestedType;

			// Token: 0x040003A5 RID: 933
			internal bool _bNestedTypePrint;
		}

		// Token: 0x0200008C RID: 140
		internal class SystemType : WsdlParser.BaseType
		{
			// Token: 0x06000460 RID: 1120 RVA: 0x00018F18 File Offset: 0x00017F18
			internal SystemType(string name, string urlNS, string ns, string assemName)
				: base(name, urlNS, ns, ns)
			{
				string text = ns + '.' + name;
				Assembly assembly;
				if (assemName == null)
				{
					assembly = typeof(string).Assembly;
				}
				else
				{
					assembly = Assembly.LoadWithPartialName(assemName, null);
				}
				if (assembly == null)
				{
					throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_AssemblyNotFound"), new object[] { assemName }));
				}
				this._type = assembly.GetType(text, true);
			}

			// Token: 0x06000461 RID: 1121 RVA: 0x00018F98 File Offset: 0x00017F98
			internal override WsdlParser.MethodFlags GetMethodFlags(WsdlParser.URTMethod method)
			{
				BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				for (Type type = this._type; type != null; type = type.BaseType)
				{
					MethodInfo[] methods = type.GetMethods(bindingFlags);
					for (int i = 0; i < methods.Length; i++)
					{
						WsdlParser.MethodFlags methodFlags = method.GetMethodFlags(methods[i]);
						if (methodFlags != WsdlParser.MethodFlags.None)
						{
							return methodFlags;
						}
					}
				}
				return WsdlParser.MethodFlags.None;
			}

			// Token: 0x17000101 RID: 257
			// (get) Token: 0x06000462 RID: 1122 RVA: 0x00018FE4 File Offset: 0x00017FE4
			internal override bool IsEmittableFieldType
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000102 RID: 258
			// (get) Token: 0x06000463 RID: 1123 RVA: 0x00018FE7 File Offset: 0x00017FE7
			internal override string FieldName
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17000103 RID: 259
			// (get) Token: 0x06000464 RID: 1124 RVA: 0x00018FEA File Offset: 0x00017FEA
			internal override string FieldNamespace
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17000104 RID: 260
			// (get) Token: 0x06000465 RID: 1125 RVA: 0x00018FED File Offset: 0x00017FED
			internal override bool PrimitiveField
			{
				get
				{
					return false;
				}
			}

			// Token: 0x040003A6 RID: 934
			private Type _type;
		}

		// Token: 0x0200008D RID: 141
		internal class URTSimpleType : WsdlParser.BaseType
		{
			// Token: 0x06000466 RID: 1126 RVA: 0x00018FF0 File Offset: 0x00017FF0
			internal URTSimpleType(string name, string urlNS, string ns, string encodedNS, bool bAnonymous, WsdlParser parser)
				: base(name, urlNS, ns, encodedNS)
			{
				this._baseTypeName = null;
				this._baseTypeXmlNS = null;
				this._baseType = null;
				this._fieldString = null;
				this._facets = new ArrayList();
				this._bEnum = false;
				this._bAnonymous = bAnonymous;
				this._encoding = null;
				this._parser = parser;
			}

			// Token: 0x06000467 RID: 1127 RVA: 0x0001904D File Offset: 0x0001804D
			internal void Extends(string baseTypeName, string baseTypeNS)
			{
				this._baseTypeName = baseTypeName;
				this._baseTypeXmlNS = baseTypeNS;
			}

			// Token: 0x17000105 RID: 261
			// (get) Token: 0x06000468 RID: 1128 RVA: 0x0001905D File Offset: 0x0001805D
			// (set) Token: 0x06000469 RID: 1129 RVA: 0x00019065 File Offset: 0x00018065
			internal bool IsEnum
			{
				get
				{
					return this._bEnum;
				}
				set
				{
					this._bEnum = value;
				}
			}

			// Token: 0x17000106 RID: 262
			// (set) Token: 0x0600046A RID: 1130 RVA: 0x00019070 File Offset: 0x00018070
			internal string EnumType
			{
				set
				{
					string text = value;
					this._parser.ParseQName(ref text);
					if (text != null && text.Length > 0)
					{
						this._enumType = this.MapToEnumType(this._parser.MapSchemaTypesToCSharpTypes(text));
					}
				}
			}

			// Token: 0x0600046B RID: 1131 RVA: 0x000190B4 File Offset: 0x000180B4
			private string MapToEnumType(string type)
			{
				string text;
				if (type == "Byte")
				{
					text = "byte";
				}
				else if (type == "SByte")
				{
					text = "sbyte";
				}
				else if (type == "Int16")
				{
					text = "short";
				}
				else if (type == "UInt16")
				{
					text = "ushort";
				}
				else if (type == "Int32")
				{
					text = "int";
				}
				else if (type == "UInt32")
				{
					text = "uint";
				}
				else if (type == "Int64")
				{
					text = "long";
				}
				else
				{
					if (!(type == "UInt64"))
					{
						throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_InvalidEnumType"), new object[] { type }));
					}
					text = "ulong";
				}
				return text;
			}

			// Token: 0x0600046C RID: 1132 RVA: 0x0001919B File Offset: 0x0001819B
			internal void AddFacet(WsdlParser.SchemaFacet facet)
			{
				this._facets.Add(facet);
			}

			// Token: 0x17000107 RID: 263
			// (get) Token: 0x0600046D RID: 1133 RVA: 0x000191AC File Offset: 0x000181AC
			internal override bool IsEmittableFieldType
			{
				get
				{
					if (this._fieldString == null)
					{
						if (this._bAnonymous && this._facets.Count == 0 && this._encoding != null && this._baseTypeName == "binary" && this._parser.MatchingSchemaStrings(this._baseTypeXmlNS))
						{
							this._fieldString = "byte[]";
						}
						else
						{
							this._fieldString = string.Empty;
						}
					}
					return this._fieldString != string.Empty;
				}
			}

			// Token: 0x17000108 RID: 264
			// (get) Token: 0x0600046E RID: 1134 RVA: 0x0001922B File Offset: 0x0001822B
			internal override string FieldName
			{
				get
				{
					return this._fieldString;
				}
			}

			// Token: 0x17000109 RID: 265
			// (get) Token: 0x0600046F RID: 1135 RVA: 0x00019234 File Offset: 0x00018234
			internal override string FieldNamespace
			{
				get
				{
					string text = null;
					if (this._parser._xsdVersion == XsdVersion.V1999)
					{
						text = WsdlParser.s_schemaNamespaceString1999;
					}
					else if (this._parser._xsdVersion == XsdVersion.V2000)
					{
						text = WsdlParser.s_schemaNamespaceString2000;
					}
					else if (this._parser._xsdVersion == XsdVersion.V2001)
					{
						text = WsdlParser.s_schemaNamespaceString;
					}
					return text;
				}
			}

			// Token: 0x1700010A RID: 266
			// (get) Token: 0x06000470 RID: 1136 RVA: 0x00019283 File Offset: 0x00018283
			internal override bool PrimitiveField
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06000471 RID: 1137 RVA: 0x00019286 File Offset: 0x00018286
			internal override string GetName(string curNS)
			{
				if (this._fieldString != null && this._fieldString != string.Empty)
				{
					return this._fieldString;
				}
				return base.GetName(curNS);
			}

			// Token: 0x06000472 RID: 1138 RVA: 0x000192B0 File Offset: 0x000182B0
			internal void PrintCSC(TextWriter textWriter, string indentation, string curNS, StringBuilder sb)
			{
				if (this.IsEmittableFieldType)
				{
					return;
				}
				if (base.bNestedType && !base.bNestedTypePrint)
				{
					return;
				}
				string encoding = this._encoding;
				sb.Length = 0;
				sb.Append("\n");
				sb.Append(indentation);
				sb.Append("[");
				sb.Append("Serializable, ");
				sb.Append("SoapType(");
				if (this._parser._xsdVersion == XsdVersion.V1999)
				{
					sb.Append("SoapOptions=SoapOption.Option1|SoapOption.AlwaysIncludeTypes|SoapOption.XsdString|SoapOption.EmbedAll,");
				}
				else if (this._parser._xsdVersion == XsdVersion.V2000)
				{
					sb.Append("SoapOptions=SoapOption.Option2|SoapOption.AlwaysIncludeTypes|SoapOption.XsdString|SoapOption.EmbedAll,");
				}
				sb.Append("XmlNamespace=");
				sb.Append(WsdlParser.IsValidUrl(base.UrlNS));
				sb.Append(", XmlTypeNamespace=");
				sb.Append(WsdlParser.IsValidUrl(base.UrlNS));
				sb.Append(")]");
				textWriter.WriteLine(sb);
				sb.Length = 0;
				sb.Append(indentation);
				if (this.IsEnum)
				{
					sb.Append("public enum ");
				}
				else
				{
					sb.Append("public class ");
				}
				if (this._bNestedType)
				{
					sb.Append(WsdlParser.IsValidCS(base.NestedTypeName));
				}
				else
				{
					sb.Append(WsdlParser.IsValidCS(base.Name));
				}
				if (this._baseType != null)
				{
					sb.Append(" : ");
					sb.Append(WsdlParser.IsValidCSAttr(this._baseType.GetName(curNS)));
				}
				else if (this.IsEnum && this._enumType != null && this._enumType.Length > 0)
				{
					sb.Append(" : ");
					sb.Append(WsdlParser.IsValidCSAttr(this._enumType));
				}
				textWriter.WriteLine(sb);
				textWriter.Write(indentation);
				textWriter.WriteLine('{');
				string text = indentation + "    ";
				for (int i = 0; i < this._facets.Count; i++)
				{
					((WsdlParser.SchemaFacet)this._facets[i]).PrintCSC(textWriter, text, curNS, sb);
				}
				textWriter.Write(indentation);
				textWriter.WriteLine('}');
			}

			// Token: 0x06000473 RID: 1139 RVA: 0x000194E5 File Offset: 0x000184E5
			internal override WsdlParser.MethodFlags GetMethodFlags(WsdlParser.URTMethod method)
			{
				return WsdlParser.MethodFlags.None;
			}

			// Token: 0x040003A7 RID: 935
			private string _baseTypeName;

			// Token: 0x040003A8 RID: 936
			private string _baseTypeXmlNS;

			// Token: 0x040003A9 RID: 937
			private WsdlParser.BaseType _baseType;

			// Token: 0x040003AA RID: 938
			private string _fieldString;

			// Token: 0x040003AB RID: 939
			private bool _bEnum;

			// Token: 0x040003AC RID: 940
			private bool _bAnonymous;

			// Token: 0x040003AD RID: 941
			private string _encoding;

			// Token: 0x040003AE RID: 942
			private ArrayList _facets;

			// Token: 0x040003AF RID: 943
			private string _enumType;

			// Token: 0x040003B0 RID: 944
			private WsdlParser _parser;
		}

		// Token: 0x0200008E RID: 142
		internal class URTComplexType : WsdlParser.BaseType
		{
			// Token: 0x06000474 RID: 1140 RVA: 0x000194E8 File Offset: 0x000184E8
			internal URTComplexType(string name, string urlNS, string ns, string encodedNS, SchemaBlockType blockDefault, bool bSUDSType, bool bAnonymous, WsdlParser parser, WsdlParser.URTNamespace xns)
				: base(name, urlNS, ns, encodedNS)
			{
				this._baseTypeName = null;
				this._baseTypeXmlNS = null;
				this._baseType = null;
				this._connectURLs = null;
				this._bStruct = !bSUDSType;
				this._blockType = blockDefault;
				this._bSUDSType = bSUDSType;
				this._bAnonymous = bAnonymous;
				this._fieldString = null;
				this._fields = new ArrayList();
				this._methods = new ArrayList();
				this._implIFaces = new ArrayList();
				this._implIFaceNames = new ArrayList();
				this._sudsType = SUDSType.None;
				this._parser = parser;
				int num = name.IndexOf('+');
				if (num > 0)
				{
					string text = parser.Atomize(name.Substring(0, num));
					if (xns.LookupComplexType(text) == null)
					{
						WsdlParser.URTComplexType urtcomplexType = new WsdlParser.URTComplexType(text, urlNS, ns, encodedNS, blockDefault, bSUDSType, bAnonymous, parser, xns);
						xns.AddComplexType(urtcomplexType);
					}
				}
				if (xns.UrtType == UrtType.Interop)
				{
					num = name.LastIndexOf('.');
					if (num > -1)
					{
						this._wireType = name;
						base.Name = name.Replace(".", "_");
						base.SearchName = name;
					}
				}
			}

			// Token: 0x06000475 RID: 1141 RVA: 0x00019609 File Offset: 0x00018609
			internal void AddNestedType(WsdlParser.BaseType ct)
			{
				if (this._nestedTypes == null)
				{
					this._nestedTypes = new ArrayList(10);
				}
				this._nestedTypes.Add(ct);
			}

			// Token: 0x06000476 RID: 1142 RVA: 0x0001962D File Offset: 0x0001862D
			internal void Extends(string baseTypeName, string baseTypeNS)
			{
				this._baseTypeName = baseTypeName;
				this._baseTypeXmlNS = baseTypeNS;
			}

			// Token: 0x06000477 RID: 1143 RVA: 0x00019640 File Offset: 0x00018640
			internal void Implements(string iFaceName, string iFaceNS, WsdlParser parser)
			{
				this._implIFaceNames.Add(iFaceName);
				this._implIFaceNames.Add(iFaceNS);
				WsdlParser.URTNamespace urtnamespace = parser.AddNewNamespace(iFaceNS);
				if (urtnamespace.LookupInterface(iFaceName) == null)
				{
					WsdlParser.URTInterface urtinterface = new WsdlParser.URTInterface(iFaceName, urtnamespace.Name, urtnamespace.Namespace, urtnamespace.EncodedNS, this._parser);
					urtnamespace.AddInterface(urtinterface);
				}
			}

			// Token: 0x1700010B RID: 267
			// (set) Token: 0x06000478 RID: 1144 RVA: 0x000196A0 File Offset: 0x000186A0
			internal ArrayList ConnectURLs
			{
				set
				{
					this._connectURLs = value;
				}
			}

			// Token: 0x1700010C RID: 268
			// (set) Token: 0x06000479 RID: 1145 RVA: 0x000196A9 File Offset: 0x000186A9
			internal bool IsStruct
			{
				set
				{
					this._bStruct = value;
				}
			}

			// Token: 0x1700010D RID: 269
			// (get) Token: 0x0600047A RID: 1146 RVA: 0x000196B2 File Offset: 0x000186B2
			// (set) Token: 0x0600047B RID: 1147 RVA: 0x000196BA File Offset: 0x000186BA
			internal bool IsSUDSType
			{
				get
				{
					return this._bSUDSType;
				}
				set
				{
					this._bSUDSType = value;
					this._bStruct = !value;
				}
			}

			// Token: 0x1700010E RID: 270
			// (get) Token: 0x0600047C RID: 1148 RVA: 0x000196CD File Offset: 0x000186CD
			// (set) Token: 0x0600047D RID: 1149 RVA: 0x000196D5 File Offset: 0x000186D5
			internal SUDSType SUDSType
			{
				get
				{
					return this._sudsType;
				}
				set
				{
					this._sudsType = value;
				}
			}

			// Token: 0x1700010F RID: 271
			// (get) Token: 0x0600047E RID: 1150 RVA: 0x000196DE File Offset: 0x000186DE
			// (set) Token: 0x0600047F RID: 1151 RVA: 0x000196E6 File Offset: 0x000186E6
			internal WsdlParser.SudsUse SudsUse
			{
				get
				{
					return this._sudsUse;
				}
				set
				{
					this._sudsUse = value;
				}
			}

			// Token: 0x17000110 RID: 272
			// (set) Token: 0x06000480 RID: 1152 RVA: 0x000196EF File Offset: 0x000186EF
			internal bool IsValueType
			{
				set
				{
					this._bValueType = value;
				}
			}

			// Token: 0x17000111 RID: 273
			// (set) Token: 0x06000481 RID: 1153 RVA: 0x000196F8 File Offset: 0x000186F8
			internal SchemaBlockType BlockType
			{
				set
				{
					this._blockType = value;
				}
			}

			// Token: 0x17000112 RID: 274
			// (get) Token: 0x06000482 RID: 1154 RVA: 0x00019701 File Offset: 0x00018701
			internal string WireType
			{
				get
				{
					return this._wireType;
				}
			}

			// Token: 0x17000113 RID: 275
			// (get) Token: 0x06000483 RID: 1155 RVA: 0x00019709 File Offset: 0x00018709
			// (set) Token: 0x06000484 RID: 1156 RVA: 0x00019711 File Offset: 0x00018711
			internal ArrayList Inherit
			{
				get
				{
					return this._inherit;
				}
				set
				{
					this._inherit = value;
				}
			}

			// Token: 0x06000485 RID: 1157 RVA: 0x0001971A File Offset: 0x0001871A
			internal bool IsArray()
			{
				return this._arrayType != null;
			}

			// Token: 0x06000486 RID: 1158 RVA: 0x00019727 File Offset: 0x00018727
			internal string GetArray()
			{
				return this._clrarray;
			}

			// Token: 0x06000487 RID: 1159 RVA: 0x0001972F File Offset: 0x0001872F
			internal WsdlParser.URTNamespace GetArrayNS()
			{
				return this._arrayNS;
			}

			// Token: 0x06000488 RID: 1160 RVA: 0x00019738 File Offset: 0x00018738
			internal string GetClassName()
			{
				string text;
				if (this._bNameMethodConflict)
				{
					text = "C" + base.Name;
				}
				else
				{
					text = base.Name;
				}
				return text;
			}

			// Token: 0x17000114 RID: 276
			// (get) Token: 0x06000489 RID: 1161 RVA: 0x0001976A File Offset: 0x0001876A
			// (set) Token: 0x0600048A RID: 1162 RVA: 0x00019772 File Offset: 0x00018772
			internal bool IsPrint
			{
				get
				{
					return this._bprint;
				}
				set
				{
					this._bprint = value;
				}
			}

			// Token: 0x17000115 RID: 277
			// (get) Token: 0x0600048B RID: 1163 RVA: 0x0001977C File Offset: 0x0001877C
			internal override bool IsEmittableFieldType
			{
				get
				{
					if (this._fieldString == null)
					{
						if (this._bAnonymous && this._fields.Count == 1)
						{
							WsdlParser.URTField urtfield = (WsdlParser.URTField)this._fields[0];
							if (urtfield.IsArray)
							{
								this._fieldString = urtfield.TypeName;
								return true;
							}
						}
						this._fieldString = string.Empty;
					}
					return this._fieldString != string.Empty;
				}
			}

			// Token: 0x17000116 RID: 278
			// (get) Token: 0x0600048C RID: 1164 RVA: 0x000197EA File Offset: 0x000187EA
			internal override string FieldName
			{
				get
				{
					return this._fieldString;
				}
			}

			// Token: 0x17000117 RID: 279
			// (get) Token: 0x0600048D RID: 1165 RVA: 0x000197F2 File Offset: 0x000187F2
			internal override string FieldNamespace
			{
				get
				{
					return ((WsdlParser.URTField)this._fields[0]).TypeNS;
				}
			}

			// Token: 0x17000118 RID: 280
			// (get) Token: 0x0600048E RID: 1166 RVA: 0x0001980A File Offset: 0x0001880A
			internal override bool PrimitiveField
			{
				get
				{
					return ((WsdlParser.URTField)this._fields[0]).IsPrimitive;
				}
			}

			// Token: 0x0600048F RID: 1167 RVA: 0x00019822 File Offset: 0x00018822
			internal override string GetName(string curNS)
			{
				if (this._fieldString != null && this._fieldString != string.Empty)
				{
					return this._fieldString;
				}
				return base.GetName(curNS);
			}

			// Token: 0x17000119 RID: 281
			// (get) Token: 0x06000490 RID: 1168 RVA: 0x0001984C File Offset: 0x0001884C
			internal ArrayList Fields
			{
				get
				{
					return this._fields;
				}
			}

			// Token: 0x06000491 RID: 1169 RVA: 0x00019854 File Offset: 0x00018854
			internal void AddField(WsdlParser.URTField field)
			{
				this._fields.Add(field);
			}

			// Token: 0x06000492 RID: 1170 RVA: 0x00019864 File Offset: 0x00018864
			internal void AddMethod(WsdlParser.URTMethod method)
			{
				if (method.Name == base.Name)
				{
					this._bNameMethodConflict = true;
				}
				this._methods.Add(method);
				int num = method.Name.IndexOf('.');
				if (num > 0)
				{
					method.MethodFlags = WsdlParser.MethodFlags.None;
					return;
				}
				method.MethodFlags = (method.MethodFlags |= WsdlParser.MethodFlags.Public);
			}

			// Token: 0x06000493 RID: 1171 RVA: 0x000198CC File Offset: 0x000188CC
			private WsdlParser.URTMethod GetMethod(string name)
			{
				for (int i = 0; i < this._methods.Count; i++)
				{
					WsdlParser.URTMethod urtmethod = (WsdlParser.URTMethod)this._methods[i];
					if (urtmethod.Name == name)
					{
						return urtmethod;
					}
				}
				return null;
			}

			// Token: 0x06000494 RID: 1172 RVA: 0x00019914 File Offset: 0x00018914
			internal void ResolveTypes(WsdlParser parser)
			{
				string text = null;
				string text2 = null;
				if (this.IsArray())
				{
					this.ResolveArray();
					return;
				}
				if (this.IsSUDSType && this._sudsType == SUDSType.None)
				{
					if (this._parser._bWrappedProxy)
					{
						this._sudsType = SUDSType.ClientProxy;
					}
					else
					{
						this._sudsType = SUDSType.MarshalByRef;
					}
				}
				if (this._baseTypeName != null)
				{
					UrtType urtType = parser.IsURTExportedType(this._baseTypeXmlNS, out text, out text2);
					if (urtType == UrtType.UrtSystem || text.StartsWith("System", StringComparison.Ordinal))
					{
						this._baseType = new WsdlParser.SystemType(this._baseTypeName, this._baseTypeXmlNS, text, text2);
					}
					else
					{
						WsdlParser.URTNamespace urtnamespace = parser.LookupNamespace(this._baseTypeXmlNS);
						if (urtnamespace == null)
						{
							throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_CantResolveSchemaNS"), new object[] { this._baseTypeXmlNS, this._baseTypeName }));
						}
						this._baseType = urtnamespace.LookupComplexType(this._baseTypeName);
						if (this._baseType == null)
						{
							this._baseType = new WsdlParser.SystemType(this._baseTypeName, this._baseTypeXmlNS, text, text2);
						}
					}
				}
				if (this.IsSUDSType)
				{
					if (this._parser._bWrappedProxy)
					{
						if (this._baseTypeName == null || this._baseType is WsdlParser.SystemType)
						{
							this._baseTypeName = "RemotingClientProxy";
							this._baseTypeXmlNS = SoapServices.CodeXmlNamespaceForClrTypeNamespace("System.Runtime.Remoting", "System.Runtime.Remoting");
							text = "System.Runtime.Remoting.Services";
							text2 = "System.Runtime.Remoting";
							this._baseType = new WsdlParser.SystemType(this._baseTypeName, this._baseTypeXmlNS, text, text2);
						}
					}
					else if (this._baseTypeName == null)
					{
						this._baseTypeName = "MarshalByRefObject";
						this._baseTypeXmlNS = SoapServices.CodeXmlNamespaceForClrTypeNamespace("System", null);
						text = "System";
						text2 = null;
						this._baseType = new WsdlParser.SystemType(this._baseTypeName, this._baseTypeXmlNS, text, text2);
					}
				}
				else if (this._baseType == null)
				{
					this._baseType = new WsdlParser.SystemType("Object", SoapServices.CodeXmlNamespaceForClrTypeNamespace("System", null), "System", null);
				}
				for (int i = 0; i < this._implIFaceNames.Count; i += 2)
				{
					string text3 = (string)this._implIFaceNames[i];
					string text4 = (string)this._implIFaceNames[i + 1];
					string text5;
					string text6;
					UrtType urtType2 = parser.IsURTExportedType(text4, out text5, out text6);
					WsdlParser.BaseInterface baseInterface;
					if (urtType2 == UrtType.UrtSystem)
					{
						baseInterface = new WsdlParser.SystemInterface(text3, text4, text5, parser, text6);
					}
					else
					{
						WsdlParser.URTNamespace urtnamespace2 = parser.LookupNamespace(text4);
						if (urtnamespace2 == null)
						{
							throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_CantResolveSchemaNS"), new object[] { text4, text3 }));
						}
						baseInterface = urtnamespace2.LookupInterface(text3);
						if (baseInterface == null)
						{
							throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_CantResolveTypeInNS"), new object[] { text3, text4 }));
						}
					}
					this._implIFaces.Add(baseInterface);
				}
				for (int j = 0; j < this._methods.Count; j++)
				{
					((WsdlParser.URTMethod)this._methods[j]).ResolveTypes(parser);
				}
			}

			// Token: 0x06000495 RID: 1173 RVA: 0x00019C38 File Offset: 0x00018C38
			internal void ResolveMethods()
			{
				for (int i = 0; i < this._methods.Count; i++)
				{
					WsdlParser.URTMethod urtmethod = (WsdlParser.URTMethod)this._methods[i];
				}
			}

			// Token: 0x06000496 RID: 1174 RVA: 0x00019C6D File Offset: 0x00018C6D
			internal override WsdlParser.MethodFlags GetMethodFlags(WsdlParser.URTMethod method)
			{
				return method.MethodFlags;
			}

			// Token: 0x06000497 RID: 1175 RVA: 0x00019C78 File Offset: 0x00018C78
			internal void PrintCSC(TextWriter textWriter, string indentation, string curNS, StringBuilder sb)
			{
				if (this.IsEmittableFieldType)
				{
					return;
				}
				if (base.bNestedType && !base.bNestedTypePrint)
				{
					return;
				}
				sb.Length = 0;
				sb.Append(indentation);
				if (this._baseTypeName != null)
				{
					string name = this._baseType.GetName(curNS);
					if (name == "System.Delegate" || name == "System.MulticastDelegate")
					{
						sb.Append("public delegate ");
						WsdlParser.URTMethod method = this.GetMethod("Invoke");
						if (method == null)
						{
							throw new SUDSParserException(CoreChannel.GetResourceString("Remoting_Suds_DelegateWithoutInvoke"));
						}
						string typeString = method.GetTypeString(curNS, true);
						sb.Append(WsdlParser.IsValidCSAttr(typeString));
						sb.Append(' ');
						string text = base.Name;
						int num = text.IndexOf('.');
						if (num > 0)
						{
							text = text.Substring(num + 1);
						}
						sb.Append(WsdlParser.IsValidCS(text));
						sb.Append('(');
						method.PrintSignature(sb, curNS);
						sb.Append(");");
						textWriter.WriteLine(sb);
						return;
					}
				}
				bool isURTType = base.IsURTType;
				sb.Length = 0;
				sb.Append("\n");
				sb.Append(indentation);
				sb.Append("[");
				if (this._sudsType != SUDSType.ClientProxy)
				{
					sb.Append("Serializable, ");
				}
				sb.Append("SoapType(");
				if (this._parser._xsdVersion == XsdVersion.V1999)
				{
					sb.Append("SoapOptions=SoapOption.Option1|SoapOption.AlwaysIncludeTypes|SoapOption.XsdString|SoapOption.EmbedAll,");
				}
				else if (this._parser._xsdVersion == XsdVersion.V2000)
				{
					sb.Append("SoapOptions=SoapOption.Option2|SoapOption.AlwaysIncludeTypes|SoapOption.XsdString|SoapOption.EmbedAll,");
				}
				if (!isURTType)
				{
					sb.Append("XmlElementName=");
					sb.Append(WsdlParser.IsValidUrl(this.GetClassName()));
					sb.Append(", XmlNamespace=");
					sb.Append(WsdlParser.IsValidUrl(base.Namespace));
					sb.Append(", XmlTypeName=");
					if (this.WireType != null)
					{
						sb.Append(WsdlParser.IsValidUrl(this.WireType));
					}
					else
					{
						sb.Append(WsdlParser.IsValidUrl(this.GetClassName()));
					}
					sb.Append(", XmlTypeNamespace=");
					sb.Append(WsdlParser.IsValidUrl(base.Namespace));
				}
				else
				{
					sb.Append("XmlNamespace=");
					sb.Append(WsdlParser.IsValidUrl(base.UrlNS));
					sb.Append(", XmlTypeNamespace=");
					sb.Append(WsdlParser.IsValidUrl(base.UrlNS));
					if (this.WireType != null)
					{
						sb.Append(", XmlTypeName=");
						sb.Append(WsdlParser.IsValidUrl(this.WireType));
					}
				}
				sb.Append(")]");
				sb.Append("[ComVisible(true)]");
				textWriter.WriteLine(sb);
				sb.Length = 0;
				sb.Append(indentation);
				if (this._sudsUse == WsdlParser.SudsUse.Struct)
				{
					sb.Append("public struct ");
				}
				else
				{
					sb.Append("public class ");
				}
				if (this._bNestedType)
				{
					sb.Append(WsdlParser.IsValidCS(base.NestedTypeName));
				}
				else
				{
					sb.Append(WsdlParser.IsValidCS(this.GetClassName()));
				}
				if (this._baseTypeName != null || this._sudsUse == WsdlParser.SudsUse.ISerializable || this._implIFaces.Count > 0)
				{
					sb.Append(" : ");
				}
				string text2 = null;
				bool flag = false;
				bool flag2 = this._baseTypeName == "RemotingClientProxy";
				if (flag2)
				{
					sb.Append("System.Runtime.Remoting.Services.RemotingClientProxy");
					flag = true;
				}
				else if (this._baseTypeName != null)
				{
					bool isURTType2 = this._baseType.IsURTType;
					text2 = this._baseType.GetName(curNS);
					if (text2 == "System.__ComObject")
					{
						sb.Append("System.MarshalByRefObject");
						flag = true;
					}
					else
					{
						sb.Append(WsdlParser.IsValidCSAttr(text2));
						flag = true;
					}
				}
				else if (this._sudsUse == WsdlParser.SudsUse.ISerializable)
				{
					sb.Append("System.Runtime.Serialization.ISerializable");
					flag = true;
				}
				if (this._implIFaces.Count > 0)
				{
					for (int i = 0; i < this._implIFaces.Count; i++)
					{
						if (flag)
						{
							sb.Append(", ");
						}
						sb.Append(WsdlParser.IsValidCS(((WsdlParser.BaseInterface)this._implIFaces[i]).GetName(curNS)));
						flag = true;
					}
				}
				textWriter.WriteLine(sb);
				textWriter.Write(indentation);
				textWriter.WriteLine('{');
				string text3 = indentation + "    ";
				int length = text3.Length;
				if (flag2)
				{
					this.PrintClientProxy(textWriter, indentation, curNS, sb);
				}
				if (this._methods.Count > 0)
				{
					string text4 = null;
					if (this._parser._bWrappedProxy)
					{
						sb.Length = 0;
						sb.Append("((");
						sb.Append(WsdlParser.IsValidCS(this.GetClassName()));
						sb.Append(") _tp).");
						text4 = sb.ToString();
					}
					for (int j = 0; j < this._methods.Count; j++)
					{
						((WsdlParser.URTMethod)this._methods[j]).PrintCSC(textWriter, text3, " ", curNS, WsdlParser.MethodPrintEnum.PrintBody, isURTType, text4, sb);
					}
					textWriter.WriteLine();
				}
				if (this._fields.Count > 0)
				{
					textWriter.Write(text3);
					textWriter.WriteLine("// Class Fields");
					for (int k = 0; k < this._fields.Count; k++)
					{
						((WsdlParser.URTField)this._fields[k]).PrintCSC(textWriter, text3, curNS, sb);
					}
				}
				if (this._nestedTypes != null && this._nestedTypes.Count > 0)
				{
					foreach (object obj in this._nestedTypes)
					{
						WsdlParser.BaseType baseType = (WsdlParser.BaseType)obj;
						baseType.bNestedTypePrint = true;
						if (baseType is WsdlParser.URTSimpleType)
						{
							((WsdlParser.URTSimpleType)baseType).PrintCSC(textWriter, text3, curNS, sb);
						}
						else
						{
							((WsdlParser.URTComplexType)baseType).PrintCSC(textWriter, text3, curNS, sb);
						}
						baseType.bNestedTypePrint = false;
					}
				}
				if (this._sudsUse == WsdlParser.SudsUse.ISerializable)
				{
					this.PrintISerializable(textWriter, indentation, curNS, sb, text2);
				}
				sb.Length = 0;
				sb.Append(indentation);
				sb.Append("}");
				textWriter.WriteLine(sb);
			}

			// Token: 0x06000498 RID: 1176 RVA: 0x0001A2FC File Offset: 0x000192FC
			private void PrintClientProxy(TextWriter textWriter, string indentation, string curNS, StringBuilder sb)
			{
				string text = indentation + "    ";
				string text2 = text + "    ";
				sb.Length = 0;
				sb.Append(text);
				sb.Append("// Constructor");
				textWriter.WriteLine(sb);
				sb.Length = 0;
				sb.Append(text);
				sb.Append("public ");
				sb.Append(WsdlParser.IsValidCS(this.GetClassName()));
				sb.Append("()");
				textWriter.WriteLine(sb);
				sb.Length = 0;
				sb.Append(text);
				sb.Append('{');
				textWriter.WriteLine(sb);
				if (this._connectURLs != null)
				{
					for (int i = 0; i < this._connectURLs.Count; i++)
					{
						sb.Length = 0;
						sb.Append(text2);
						if (i == 0)
						{
							sb.Append("base.ConfigureProxy(this.GetType(), ");
							sb.Append(WsdlParser.IsValidUrl((string)this._connectURLs[i]));
							sb.Append(");");
						}
						else
						{
							sb.Append("//base.ConfigureProxy(this.GetType(), ");
							sb.Append(WsdlParser.IsValidUrl((string)this._connectURLs[i]));
							sb.Append(");");
						}
						textWriter.WriteLine(sb);
					}
				}
				foreach (object obj in this._parser._URTNamespaces)
				{
					WsdlParser.URTNamespace urtnamespace = (WsdlParser.URTNamespace)obj;
					foreach (object obj2 in urtnamespace._URTComplexTypes)
					{
						WsdlParser.URTComplexType urtcomplexType = (WsdlParser.URTComplexType)obj2;
						if (urtcomplexType._sudsType != SUDSType.ClientProxy && !urtcomplexType.IsArray())
						{
							sb.Length = 0;
							sb.Append(text2);
							sb.Append("System.Runtime.Remoting.SoapServices.PreLoad(typeof(");
							sb.Append(WsdlParser.IsValidCS(urtnamespace.EncodedNS));
							if (urtnamespace.EncodedNS != null && urtnamespace.EncodedNS.Length > 0)
							{
								sb.Append(".");
							}
							sb.Append(WsdlParser.IsValidCS(urtcomplexType.Name));
							sb.Append("));");
							textWriter.WriteLine(sb);
						}
					}
				}
				foreach (object obj3 in this._parser._URTNamespaces)
				{
					WsdlParser.URTNamespace urtnamespace2 = (WsdlParser.URTNamespace)obj3;
					foreach (object obj4 in urtnamespace2._URTSimpleTypes)
					{
						WsdlParser.URTSimpleType urtsimpleType = (WsdlParser.URTSimpleType)obj4;
						if (urtsimpleType.IsEnum)
						{
							sb.Length = 0;
							sb.Append(text2);
							sb.Append("System.Runtime.Remoting.SoapServices.PreLoad(typeof(");
							sb.Append(WsdlParser.IsValidCS(urtnamespace2.EncodedNS));
							if (urtnamespace2.EncodedNS != null && urtnamespace2.EncodedNS.Length > 0)
							{
								sb.Append(".");
							}
							sb.Append(WsdlParser.IsValidCS(urtsimpleType.Name));
							sb.Append("));");
							textWriter.WriteLine(sb);
						}
					}
				}
				sb.Length = 0;
				sb.Append(text);
				sb.Append('}');
				textWriter.WriteLine(sb);
				textWriter.WriteLine();
				sb.Length = 0;
				sb.Append(text);
				sb.Append("public Object RemotingReference");
				textWriter.WriteLine(sb);
				sb.Length = 0;
				sb.Append(text);
				sb.Append("{");
				textWriter.WriteLine(sb);
				sb.Length = 0;
				sb.Append(text2);
				sb.Append("get{return(_tp);}");
				textWriter.WriteLine(sb);
				sb.Length = 0;
				sb.Append(text);
				sb.Append("}");
				textWriter.WriteLine(sb);
				textWriter.WriteLine();
			}

			// Token: 0x06000499 RID: 1177 RVA: 0x0001A798 File Offset: 0x00019798
			private void PrintISerializable(TextWriter textWriter, string indentation, string curNS, StringBuilder sb, string baseString)
			{
				string text = indentation + "    ";
				string text2 = text + "    ";
				if (baseString == null || baseString.StartsWith("System.", StringComparison.Ordinal))
				{
					sb.Length = 0;
					sb.Append(text);
					sb.Append("public System.Runtime.Serialization.SerializationInfo info;");
					textWriter.WriteLine(sb);
					sb.Length = 0;
					sb.Append(text);
					sb.Append("public System.Runtime.Serialization.StreamingContext context; \n");
					textWriter.WriteLine(sb);
				}
				sb.Length = 0;
				sb.Append(text);
				if (this._baseTypeName == null)
				{
					sb.Append("public ");
				}
				else
				{
					sb.Append("protected ");
				}
				if (this._bNestedType)
				{
					sb.Append(WsdlParser.IsValidCS(base.NestedTypeName));
				}
				else
				{
					sb.Append(WsdlParser.IsValidCS(this.GetClassName()));
				}
				sb.Append("(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)");
				if (this._baseTypeName != null)
				{
					sb.Append(" : base(info, context)");
				}
				textWriter.WriteLine(sb);
				sb.Length = 0;
				sb.Append(text);
				sb.Append("{");
				textWriter.WriteLine(sb);
				if (baseString == null || baseString.StartsWith("System.", StringComparison.Ordinal))
				{
					sb.Length = 0;
					sb.Append(text2);
					sb.Append("this.info = info;");
					textWriter.WriteLine(sb);
					sb.Length = 0;
					sb.Append(text2);
					sb.Append("this.context = context;");
					textWriter.WriteLine(sb);
				}
				sb.Length = 0;
				sb.Append(text);
				sb.Append("}");
				textWriter.WriteLine(sb);
				if (this._baseTypeName == null)
				{
					sb.Length = 0;
					sb.Append(text);
					sb.Append("public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)");
					textWriter.WriteLine(sb);
					sb.Length = 0;
					sb.Append(text);
					sb.Append("{");
					textWriter.WriteLine(sb);
					sb.Length = 0;
					sb.Append(text);
					sb.Append("}");
					textWriter.WriteLine(sb);
				}
			}

			// Token: 0x0600049A RID: 1178 RVA: 0x0001A9D0 File Offset: 0x000199D0
			internal void AddArray(string arrayType, WsdlParser.URTNamespace arrayNS)
			{
				this._arrayType = arrayType;
				this._arrayNS = arrayNS;
			}

			// Token: 0x0600049B RID: 1179 RVA: 0x0001A9E0 File Offset: 0x000199E0
			internal void ResolveArray()
			{
				if (this._clrarray != null)
				{
					return;
				}
				string text = null;
				string text2 = this._arrayType;
				int num = this._arrayType.IndexOf("[");
				if (num < 0)
				{
					throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_WsdlInvalidArraySyntax"), new object[] { this._arrayType }));
				}
				text2 = this._arrayType.Substring(0, num);
				switch (this._arrayNS.UrtType)
				{
				case UrtType.Interop:
					text = text2;
					break;
				case UrtType.UrtSystem:
				case UrtType.UrtUser:
					text = text2;
					break;
				case UrtType.Xsd:
					text = this._parser.MapSchemaTypesToCSharpTypes(text2);
					break;
				}
				this._clrarray = text + this.FilterDimensions(this._arrayType.Substring(num));
			}

			// Token: 0x0600049C RID: 1180 RVA: 0x0001AAA8 File Offset: 0x00019AA8
			private string FilterDimensions(string value)
			{
				char[] array = new char[value.Length];
				for (int i = 0; i < value.Length; i++)
				{
					if (char.IsDigit(value[i]))
					{
						array[i] = ' ';
					}
					else
					{
						array[i] = value[i];
					}
				}
				return new string(array);
			}

			// Token: 0x040003B1 RID: 945
			private string _baseTypeName;

			// Token: 0x040003B2 RID: 946
			private string _baseTypeXmlNS;

			// Token: 0x040003B3 RID: 947
			private WsdlParser.BaseType _baseType;

			// Token: 0x040003B4 RID: 948
			private ArrayList _connectURLs;

			// Token: 0x040003B5 RID: 949
			private bool _bStruct;

			// Token: 0x040003B6 RID: 950
			private SchemaBlockType _blockType;

			// Token: 0x040003B7 RID: 951
			private bool _bSUDSType;

			// Token: 0x040003B8 RID: 952
			private bool _bAnonymous;

			// Token: 0x040003B9 RID: 953
			private string _wireType;

			// Token: 0x040003BA RID: 954
			private ArrayList _inherit;

			// Token: 0x040003BB RID: 955
			private string _fieldString;

			// Token: 0x040003BC RID: 956
			private ArrayList _implIFaceNames;

			// Token: 0x040003BD RID: 957
			private ArrayList _implIFaces;

			// Token: 0x040003BE RID: 958
			private ArrayList _fields;

			// Token: 0x040003BF RID: 959
			private ArrayList _methods;

			// Token: 0x040003C0 RID: 960
			private SUDSType _sudsType;

			// Token: 0x040003C1 RID: 961
			private WsdlParser.SudsUse _sudsUse;

			// Token: 0x040003C2 RID: 962
			private bool _bValueType;

			// Token: 0x040003C3 RID: 963
			private WsdlParser _parser;

			// Token: 0x040003C4 RID: 964
			private string _arrayType;

			// Token: 0x040003C5 RID: 965
			private WsdlParser.URTNamespace _arrayNS;

			// Token: 0x040003C6 RID: 966
			private string _clrarray;

			// Token: 0x040003C7 RID: 967
			private bool _bprint = true;

			// Token: 0x040003C8 RID: 968
			private bool _bNameMethodConflict;
		}

		// Token: 0x0200008F RID: 143
		internal class ElementDecl
		{
			// Token: 0x0600049D RID: 1181 RVA: 0x0001AAF7 File Offset: 0x00019AF7
			internal ElementDecl(string elmName, string elmNS, string typeName, string typeNS, bool bPrimitive)
			{
				this._elmName = elmName;
				this._elmNS = elmNS;
				this._typeName = typeName;
				this._typeNS = typeNS;
				this._bPrimitive = bPrimitive;
			}

			// Token: 0x1700011A RID: 282
			// (get) Token: 0x0600049E RID: 1182 RVA: 0x0001AB24 File Offset: 0x00019B24
			internal string Name
			{
				get
				{
					return this._elmName;
				}
			}

			// Token: 0x1700011B RID: 283
			// (get) Token: 0x0600049F RID: 1183 RVA: 0x0001AB2C File Offset: 0x00019B2C
			internal string Namespace
			{
				get
				{
					return this._elmNS;
				}
			}

			// Token: 0x1700011C RID: 284
			// (get) Token: 0x060004A0 RID: 1184 RVA: 0x0001AB34 File Offset: 0x00019B34
			internal string TypeName
			{
				get
				{
					return this._typeName;
				}
			}

			// Token: 0x1700011D RID: 285
			// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0001AB3C File Offset: 0x00019B3C
			internal string TypeNS
			{
				get
				{
					return this._typeNS;
				}
			}

			// Token: 0x060004A2 RID: 1186 RVA: 0x0001AB44 File Offset: 0x00019B44
			internal bool Resolve(WsdlParser parser)
			{
				if (this._bPrimitive)
				{
					return true;
				}
				WsdlParser.URTNamespace urtnamespace = parser.LookupNamespace(this.TypeNS);
				if (urtnamespace == null)
				{
					throw new SUDSParserException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Suds_CantResolveSchemaNS"), new object[] { this.TypeNS, this.TypeName }));
				}
				WsdlParser.BaseType baseType = urtnamespace.LookupType(this.TypeName);
				if (baseType == null)
				{
					return false;
				}
				baseType.ElementName = this.Name;
				baseType.ElementNS = this.Namespace;
				return true;
			}

			// Token: 0x040003C9 RID: 969
			private string _elmName;

			// Token: 0x040003CA RID: 970
			private string _elmNS;

			// Token: 0x040003CB RID: 971
			private string _typeName;

			// Token: 0x040003CC RID: 972
			private string _typeNS;

			// Token: 0x040003CD RID: 973
			private bool _bPrimitive;
		}

		// Token: 0x02000090 RID: 144
		internal class URTNamespace
		{
			// Token: 0x060004A3 RID: 1187 RVA: 0x0001ABCC File Offset: 0x00019BCC
			internal URTNamespace(string name, WsdlParser parser)
			{
				this._name = name;
				this._parser = parser;
				this._nsType = parser.IsURTExportedType(name, out this._namespace, out this._assemName);
				if (this._nsType == UrtType.Interop)
				{
					this._encodedNS = WsdlParser.URTNamespace.EncodeInterop(this._namespace, parser);
				}
				else
				{
					this._encodedNS = this._namespace;
				}
				this._elmDecls = new ArrayList();
				this._URTComplexTypes = new ArrayList();
				this._numURTComplexTypes = 0;
				this._URTSimpleTypes = new ArrayList();
				this._numURTSimpleTypes = 0;
				this._URTInterfaces = new ArrayList();
				this._anonymousSeqNum = 0;
				parser.AddNamespace(this);
			}

			// Token: 0x060004A4 RID: 1188 RVA: 0x0001AC78 File Offset: 0x00019C78
			internal static string EncodeInterop(string name, WsdlParser parser)
			{
				string text = name;
				if (parser.ProxyNamespace != null && parser.ProxyNamespace.Length > 0)
				{
					string text2 = "";
					if (parser.ProxyNamespaceCount > 0)
					{
						text2 = parser.ProxyNamespaceCount.ToString(CultureInfo.InvariantCulture);
					}
					parser.ProxyNamespaceCount++;
					text = parser.ProxyNamespace + text2;
				}
				else
				{
					int num = name.IndexOf(":");
					if (num > 0)
					{
						text = text.Substring(num + 1);
					}
					if (text.StartsWith("//", StringComparison.Ordinal))
					{
						text = text.Substring(2);
					}
					text = text.Replace('/', '_');
				}
				return text;
			}

			// Token: 0x060004A5 RID: 1189 RVA: 0x0001AD19 File Offset: 0x00019D19
			internal string GetNextAnonymousName()
			{
				this._anonymousSeqNum++;
				return "AnonymousType" + this._anonymousSeqNum;
			}

			// Token: 0x060004A6 RID: 1190 RVA: 0x0001AD3E File Offset: 0x00019D3E
			internal void AddElementDecl(WsdlParser.ElementDecl elmDecl)
			{
				this._elmDecls.Add(elmDecl);
			}

			// Token: 0x060004A7 RID: 1191 RVA: 0x0001AD4D File Offset: 0x00019D4D
			internal void AddComplexType(WsdlParser.URTComplexType type)
			{
				this._URTComplexTypes.Add(type);
				this._numURTComplexTypes++;
			}

			// Token: 0x060004A8 RID: 1192 RVA: 0x0001AD6A File Offset: 0x00019D6A
			internal void AddSimpleType(WsdlParser.URTSimpleType type)
			{
				this._URTSimpleTypes.Add(type);
				this._numURTSimpleTypes++;
			}

			// Token: 0x060004A9 RID: 1193 RVA: 0x0001AD87 File Offset: 0x00019D87
			internal void AddInterface(WsdlParser.URTInterface iface)
			{
				this._URTInterfaces.Add(iface);
			}

			// Token: 0x1700011E RID: 286
			// (get) Token: 0x060004AA RID: 1194 RVA: 0x0001AD96 File Offset: 0x00019D96
			internal string Namespace
			{
				get
				{
					return this._namespace;
				}
			}

			// Token: 0x1700011F RID: 287
			// (get) Token: 0x060004AB RID: 1195 RVA: 0x0001AD9E File Offset: 0x00019D9E
			internal bool IsSystem
			{
				get
				{
					return this._namespace != null && this._namespace.StartsWith("System", StringComparison.Ordinal);
				}
			}

			// Token: 0x17000120 RID: 288
			// (get) Token: 0x060004AC RID: 1196 RVA: 0x0001ADBE File Offset: 0x00019DBE
			// (set) Token: 0x060004AD RID: 1197 RVA: 0x0001ADC6 File Offset: 0x00019DC6
			internal string EncodedNS
			{
				get
				{
					return this._encodedNS;
				}
				set
				{
					this._encodedNS = value;
				}
			}

			// Token: 0x17000121 RID: 289
			// (get) Token: 0x060004AE RID: 1198 RVA: 0x0001ADCF File Offset: 0x00019DCF
			// (set) Token: 0x060004AF RID: 1199 RVA: 0x0001ADD7 File Offset: 0x00019DD7
			internal bool bReferenced
			{
				get
				{
					return this._bReferenced;
				}
				set
				{
					this._bReferenced = value;
				}
			}

			// Token: 0x17000122 RID: 290
			// (get) Token: 0x060004B0 RID: 1200 RVA: 0x0001ADE0 File Offset: 0x00019DE0
			internal string Name
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x17000123 RID: 291
			// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0001ADE8 File Offset: 0x00019DE8
			internal string AssemName
			{
				get
				{
					return this._assemName;
				}
			}

			// Token: 0x17000124 RID: 292
			// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0001ADF0 File Offset: 0x00019DF0
			internal UrtType UrtType
			{
				get
				{
					return this._nsType;
				}
			}

			// Token: 0x17000125 RID: 293
			// (get) Token: 0x060004B3 RID: 1203 RVA: 0x0001ADF8 File Offset: 0x00019DF8
			internal bool IsURTNamespace
			{
				get
				{
					return this._namespace == this._encodedNS;
				}
			}

			// Token: 0x17000126 RID: 294
			// (get) Token: 0x060004B4 RID: 1204 RVA: 0x0001AE08 File Offset: 0x00019E08
			internal bool IsEmpty
			{
				get
				{
					return this.ComplexTypeOnlyArrayorEmpty() && this._URTInterfaces.Count == 0 && this._numURTSimpleTypes == 0;
				}
			}

			// Token: 0x060004B5 RID: 1205 RVA: 0x0001AE3C File Offset: 0x00019E3C
			internal bool ComplexTypeOnlyArrayorEmpty()
			{
				bool flag = true;
				for (int i = 0; i < this._URTComplexTypes.Count; i++)
				{
					WsdlParser.URTComplexType urtcomplexType = (WsdlParser.URTComplexType)this._URTComplexTypes[i];
					if (urtcomplexType != null && !urtcomplexType.IsArray())
					{
						flag = false;
						break;
					}
				}
				return flag;
			}

			// Token: 0x060004B6 RID: 1206 RVA: 0x0001AE84 File Offset: 0x00019E84
			internal WsdlParser.URTComplexType LookupComplexType(string typeName)
			{
				WsdlParser.URTComplexType urtcomplexType = null;
				for (int i = 0; i < this._URTComplexTypes.Count; i++)
				{
					WsdlParser.URTComplexType urtcomplexType2 = (WsdlParser.URTComplexType)this._URTComplexTypes[i];
					if (urtcomplexType2 != null && WsdlParser.MatchingStrings(urtcomplexType2.SearchName, typeName))
					{
						urtcomplexType = urtcomplexType2;
						break;
					}
				}
				return urtcomplexType;
			}

			// Token: 0x060004B7 RID: 1207 RVA: 0x0001AED4 File Offset: 0x00019ED4
			internal WsdlParser.URTComplexType LookupComplexTypeEqual(string typeName)
			{
				WsdlParser.URTComplexType urtcomplexType = null;
				for (int i = 0; i < this._URTComplexTypes.Count; i++)
				{
					WsdlParser.URTComplexType urtcomplexType2 = (WsdlParser.URTComplexType)this._URTComplexTypes[i];
					if (urtcomplexType2 != null && urtcomplexType2.SearchName == typeName)
					{
						urtcomplexType = urtcomplexType2;
						break;
					}
				}
				return urtcomplexType;
			}

			// Token: 0x060004B8 RID: 1208 RVA: 0x0001AF24 File Offset: 0x00019F24
			internal WsdlParser.URTSimpleType LookupSimpleType(string typeName)
			{
				for (int i = 0; i < this._URTSimpleTypes.Count; i++)
				{
					WsdlParser.URTSimpleType urtsimpleType = (WsdlParser.URTSimpleType)this._URTSimpleTypes[i];
					if (urtsimpleType != null && WsdlParser.MatchingStrings(urtsimpleType.Name, typeName))
					{
						return urtsimpleType;
					}
				}
				return null;
			}

			// Token: 0x060004B9 RID: 1209 RVA: 0x0001AF70 File Offset: 0x00019F70
			internal WsdlParser.BaseType LookupType(string typeName)
			{
				WsdlParser.BaseType baseType = this.LookupComplexType(typeName);
				if (baseType == null)
				{
					baseType = this.LookupSimpleType(typeName);
				}
				return baseType;
			}

			// Token: 0x060004BA RID: 1210 RVA: 0x0001AF94 File Offset: 0x00019F94
			internal void RemoveComplexType(WsdlParser.URTComplexType type)
			{
				for (int i = 0; i < this._URTComplexTypes.Count; i++)
				{
					if (this._URTComplexTypes[i] == type)
					{
						this._URTComplexTypes[i] = null;
						this._numURTComplexTypes--;
						return;
					}
				}
				throw new SUDSParserException(CoreChannel.GetResourceString("Remoting_Suds_TriedToRemoveNonexistentType"));
			}

			// Token: 0x060004BB RID: 1211 RVA: 0x0001AFF4 File Offset: 0x00019FF4
			internal void RemoveSimpleType(WsdlParser.URTSimpleType type)
			{
				for (int i = 0; i < this._URTSimpleTypes.Count; i++)
				{
					if (this._URTSimpleTypes[i] == type)
					{
						this._URTSimpleTypes[i] = null;
						this._numURTSimpleTypes--;
						return;
					}
				}
				throw new SUDSParserException(CoreChannel.GetResourceString("Remoting_Suds_TriedToRemoveNonexistentType"));
			}

			// Token: 0x060004BC RID: 1212 RVA: 0x0001B054 File Offset: 0x0001A054
			internal WsdlParser.URTInterface LookupInterface(string iFaceName)
			{
				for (int i = 0; i < this._URTInterfaces.Count; i++)
				{
					WsdlParser.URTInterface urtinterface = (WsdlParser.URTInterface)this._URTInterfaces[i];
					if (WsdlParser.MatchingStrings(urtinterface.Name, iFaceName))
					{
						return urtinterface;
					}
				}
				return null;
			}

			// Token: 0x060004BD RID: 1213 RVA: 0x0001B09C File Offset: 0x0001A09C
			internal void ResolveElements(WsdlParser parser)
			{
				for (int i = 0; i < this._elmDecls.Count; i++)
				{
					((WsdlParser.ElementDecl)this._elmDecls[i]).Resolve(parser);
				}
			}

			// Token: 0x060004BE RID: 1214 RVA: 0x0001B0D8 File Offset: 0x0001A0D8
			internal void ResolveTypes(WsdlParser parser)
			{
				for (int i = 0; i < this._URTComplexTypes.Count; i++)
				{
					if (this._URTComplexTypes[i] != null)
					{
						((WsdlParser.URTComplexType)this._URTComplexTypes[i]).ResolveTypes(parser);
					}
				}
				for (int j = 0; j < this._URTInterfaces.Count; j++)
				{
					((WsdlParser.URTInterface)this._URTInterfaces[j]).ResolveTypes(parser);
				}
			}

			// Token: 0x060004BF RID: 1215 RVA: 0x0001B150 File Offset: 0x0001A150
			internal void ResolveMethods()
			{
				for (int i = 0; i < this._URTComplexTypes.Count; i++)
				{
					if (this._URTComplexTypes[i] != null)
					{
						((WsdlParser.URTComplexType)this._URTComplexTypes[i]).ResolveMethods();
					}
				}
			}

			// Token: 0x060004C0 RID: 1216 RVA: 0x0001B198 File Offset: 0x0001A198
			internal void PrintCSC(WsdlParser.WriterStream writerStream)
			{
				TextWriter outputStream = writerStream.OutputStream;
				bool flag = false;
				if (this._numURTComplexTypes > 0)
				{
					for (int i = 0; i < this._URTComplexTypes.Count; i++)
					{
						WsdlParser.URTComplexType urtcomplexType = (WsdlParser.URTComplexType)this._URTComplexTypes[i];
						if (urtcomplexType != null && urtcomplexType.IsPrint)
						{
							flag = true;
						}
					}
				}
				if (this._numURTSimpleTypes > 0)
				{
					for (int j = 0; j < this._URTSimpleTypes.Count; j++)
					{
						WsdlParser.URTSimpleType urtsimpleType = (WsdlParser.URTSimpleType)this._URTSimpleTypes[j];
						if (urtsimpleType != null)
						{
							flag = true;
						}
					}
				}
				if (this._URTInterfaces.Count > 0)
				{
					flag = true;
				}
				if (!flag)
				{
					return;
				}
				string text = string.Empty;
				Stream baseStream = ((StreamWriter)outputStream).BaseStream;
				if (!writerStream.GetWrittenTo())
				{
					outputStream.WriteLine("using System;");
					outputStream.WriteLine("using System.Runtime.Remoting.Messaging;");
					outputStream.WriteLine("using System.Runtime.Remoting.Metadata;");
					outputStream.WriteLine("using System.Runtime.Remoting.Metadata.W3cXsd2001;");
					outputStream.WriteLine("using System.Runtime.InteropServices;");
					writerStream.SetWrittenTo();
				}
				if (this.Namespace != null && this.Namespace.Length != 0)
				{
					outputStream.Write("namespace ");
					outputStream.Write(WsdlParser.IsValidCS(this.EncodedNS));
					outputStream.WriteLine(" {");
					text = "    ";
				}
				StringBuilder stringBuilder = new StringBuilder(256);
				if (this._numURTComplexTypes > 0)
				{
					for (int k = 0; k < this._URTComplexTypes.Count; k++)
					{
						WsdlParser.URTComplexType urtcomplexType2 = (WsdlParser.URTComplexType)this._URTComplexTypes[k];
						if (urtcomplexType2 != null && urtcomplexType2.IsPrint)
						{
							urtcomplexType2.PrintCSC(outputStream, text, this._encodedNS, stringBuilder);
						}
					}
				}
				if (this._numURTSimpleTypes > 0)
				{
					for (int l = 0; l < this._URTSimpleTypes.Count; l++)
					{
						WsdlParser.URTSimpleType urtsimpleType2 = (WsdlParser.URTSimpleType)this._URTSimpleTypes[l];
						if (urtsimpleType2 != null)
						{
							urtsimpleType2.PrintCSC(outputStream, text, this._encodedNS, stringBuilder);
						}
					}
				}
				for (int m = 0; m < this._URTInterfaces.Count; m++)
				{
					((WsdlParser.URTInterface)this._URTInterfaces[m]).PrintCSC(outputStream, text, this._encodedNS, stringBuilder);
				}
				if (this.Namespace != null && this.Namespace.Length != 0)
				{
					outputStream.WriteLine('}');
				}
			}

			// Token: 0x040003CE RID: 974
			private string _name;

			// Token: 0x040003CF RID: 975
			private UrtType _nsType;

			// Token: 0x040003D0 RID: 976
			private WsdlParser _parser;

			// Token: 0x040003D1 RID: 977
			private string _namespace;

			// Token: 0x040003D2 RID: 978
			private string _encodedNS;

			// Token: 0x040003D3 RID: 979
			private string _assemName;

			// Token: 0x040003D4 RID: 980
			private int _anonymousSeqNum;

			// Token: 0x040003D5 RID: 981
			private ArrayList _elmDecls;

			// Token: 0x040003D6 RID: 982
			internal ArrayList _URTComplexTypes;

			// Token: 0x040003D7 RID: 983
			private int _numURTComplexTypes;

			// Token: 0x040003D8 RID: 984
			internal ArrayList _URTSimpleTypes;

			// Token: 0x040003D9 RID: 985
			private int _numURTSimpleTypes;

			// Token: 0x040003DA RID: 986
			private ArrayList _URTInterfaces;

			// Token: 0x040003DB RID: 987
			private bool _bReferenced;
		}

		// Token: 0x02000091 RID: 145
		internal interface IDump
		{
			// Token: 0x060004C1 RID: 1217
			void Dump();
		}

		// Token: 0x02000092 RID: 146
		internal interface INamespaces
		{
			// Token: 0x060004C2 RID: 1218
			void UsedNamespace(Hashtable namespaces);
		}

		// Token: 0x02000093 RID: 147
		internal class WsdlMessage : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004C3 RID: 1219 RVA: 0x0001B3E0 File Offset: 0x0001A3E0
			public void UsedNamespace(Hashtable namespaces)
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					((WsdlParser.INamespaces)this.parts[i]).UsedNamespace(namespaces);
				}
			}

			// Token: 0x060004C4 RID: 1220 RVA: 0x0001B41C File Offset: 0x0001A41C
			public void Dump()
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					((WsdlParser.IDump)this.parts[i]).Dump();
				}
			}

			// Token: 0x040003DC RID: 988
			internal string name;

			// Token: 0x040003DD RID: 989
			internal string nameNs;

			// Token: 0x040003DE RID: 990
			internal ArrayList parts = new ArrayList(10);
		}

		// Token: 0x02000094 RID: 148
		internal class WsdlMessagePart : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004C6 RID: 1222 RVA: 0x0001B46A File Offset: 0x0001A46A
			public void UsedNamespace(Hashtable namespaces)
			{
				if (this.nameNs != null)
				{
					namespaces[this.nameNs] = 1;
				}
				if (this.elementNs != null)
				{
					namespaces[this.elementNs] = 1;
				}
			}

			// Token: 0x060004C7 RID: 1223 RVA: 0x0001B4A0 File Offset: 0x0001A4A0
			public void Dump()
			{
			}

			// Token: 0x040003DF RID: 991
			internal string name;

			// Token: 0x040003E0 RID: 992
			internal string nameNs;

			// Token: 0x040003E1 RID: 993
			internal string element;

			// Token: 0x040003E2 RID: 994
			internal string elementNs;

			// Token: 0x040003E3 RID: 995
			internal string typeName;

			// Token: 0x040003E4 RID: 996
			internal string typeNameNs;
		}

		// Token: 0x02000095 RID: 149
		internal class WsdlPortType : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004C9 RID: 1225 RVA: 0x0001B4AC File Offset: 0x0001A4AC
			public void UsedNamespace(Hashtable namespaces)
			{
				foreach (object obj in this.operations)
				{
					WsdlParser.INamespaces namespaces2 = (WsdlParser.INamespaces)obj;
					namespaces2.UsedNamespace(namespaces);
				}
			}

			// Token: 0x060004CA RID: 1226 RVA: 0x0001B508 File Offset: 0x0001A508
			public void Dump()
			{
				foreach (object obj in this.sections)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				}
				foreach (object obj2 in this.operations)
				{
					WsdlParser.IDump dump = (WsdlParser.IDump)obj2;
					dump.Dump();
				}
			}

			// Token: 0x040003E5 RID: 997
			internal string name;

			// Token: 0x040003E6 RID: 998
			internal ArrayList operations = new ArrayList(10);

			// Token: 0x040003E7 RID: 999
			internal Hashtable sections = new Hashtable(10);
		}

		// Token: 0x02000096 RID: 150
		internal class WsdlPortTypeOperation : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004CC RID: 1228 RVA: 0x0001B5C8 File Offset: 0x0001A5C8
			public void UsedNamespace(Hashtable namespaces)
			{
				foreach (object obj in this.contents)
				{
					WsdlParser.INamespaces namespaces2 = (WsdlParser.INamespaces)obj;
					namespaces2.UsedNamespace(namespaces);
				}
			}

			// Token: 0x060004CD RID: 1229 RVA: 0x0001B624 File Offset: 0x0001A624
			public void Dump()
			{
				foreach (object obj in this.contents)
				{
					WsdlParser.IDump dump = (WsdlParser.IDump)obj;
					dump.Dump();
				}
			}

			// Token: 0x040003E8 RID: 1000
			internal string name;

			// Token: 0x040003E9 RID: 1001
			internal string nameNs;

			// Token: 0x040003EA RID: 1002
			internal string parameterOrder;

			// Token: 0x040003EB RID: 1003
			internal ArrayList contents = new ArrayList(3);
		}

		// Token: 0x02000097 RID: 151
		internal class WsdlPortTypeOperationContent : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004CF RID: 1231 RVA: 0x0001B690 File Offset: 0x0001A690
			public void UsedNamespace(Hashtable namespaces)
			{
			}

			// Token: 0x060004D0 RID: 1232 RVA: 0x0001B692 File Offset: 0x0001A692
			public void Dump()
			{
			}

			// Token: 0x040003EC RID: 1004
			internal string element;

			// Token: 0x040003ED RID: 1005
			internal string name;

			// Token: 0x040003EE RID: 1006
			internal string nameNs;

			// Token: 0x040003EF RID: 1007
			internal string message;

			// Token: 0x040003F0 RID: 1008
			internal string messageNs;
		}

		// Token: 0x02000098 RID: 152
		internal class WsdlBinding : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004D2 RID: 1234 RVA: 0x0001B69C File Offset: 0x0001A69C
			public void UsedNamespace(Hashtable namespaces)
			{
				if (this.soapBinding != null)
				{
					this.soapBinding.UsedNamespace(namespaces);
				}
				foreach (object obj in this.suds)
				{
					WsdlParser.INamespaces namespaces2 = (WsdlParser.INamespaces)obj;
					namespaces2.UsedNamespace(namespaces);
				}
				foreach (object obj2 in this.operations)
				{
					WsdlParser.INamespaces namespaces3 = (WsdlParser.INamespaces)obj2;
					namespaces3.UsedNamespace(namespaces);
				}
			}

			// Token: 0x060004D3 RID: 1235 RVA: 0x0001B758 File Offset: 0x0001A758
			public void Dump()
			{
				if (this.soapBinding != null)
				{
					this.soapBinding.Dump();
				}
				foreach (object obj in this.suds)
				{
					WsdlParser.IDump dump = (WsdlParser.IDump)obj;
					dump.Dump();
				}
				foreach (object obj2 in this.operations)
				{
					WsdlParser.IDump dump2 = (WsdlParser.IDump)obj2;
					dump2.Dump();
				}
			}

			// Token: 0x040003F1 RID: 1009
			internal WsdlParser.URTNamespace parsingNamespace;

			// Token: 0x040003F2 RID: 1010
			internal string name;

			// Token: 0x040003F3 RID: 1011
			internal string type;

			// Token: 0x040003F4 RID: 1012
			internal string typeNs;

			// Token: 0x040003F5 RID: 1013
			internal ArrayList suds = new ArrayList(10);

			// Token: 0x040003F6 RID: 1014
			internal WsdlParser.WsdlBindingSoapBinding soapBinding;

			// Token: 0x040003F7 RID: 1015
			internal ArrayList operations = new ArrayList(10);
		}

		// Token: 0x02000099 RID: 153
		internal class WsdlBindingOperation : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004D5 RID: 1237 RVA: 0x0001B838 File Offset: 0x0001A838
			public void UsedNamespace(Hashtable namespaces)
			{
				this.soapOperation.UsedNamespace(namespaces);
				foreach (object obj in this.sections)
				{
					WsdlParser.INamespaces namespaces2 = (WsdlParser.INamespaces)obj;
					namespaces2.UsedNamespace(namespaces);
				}
			}

			// Token: 0x060004D6 RID: 1238 RVA: 0x0001B8A0 File Offset: 0x0001A8A0
			public void Dump()
			{
				this.soapOperation.Dump();
				foreach (object obj in this.sections)
				{
					WsdlParser.IDump dump = (WsdlParser.IDump)obj;
					dump.Dump();
				}
			}

			// Token: 0x040003F8 RID: 1016
			internal string name;

			// Token: 0x040003F9 RID: 1017
			internal string nameNs;

			// Token: 0x040003FA RID: 1018
			internal string methodAttributes;

			// Token: 0x040003FB RID: 1019
			internal WsdlParser.WsdlBindingSoapOperation soapOperation;

			// Token: 0x040003FC RID: 1020
			internal ArrayList sections = new ArrayList(10);
		}

		// Token: 0x0200009A RID: 154
		internal class WsdlBindingOperationSection : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004D8 RID: 1240 RVA: 0x0001B91C File Offset: 0x0001A91C
			public void UsedNamespace(Hashtable namespaces)
			{
				foreach (object obj in this.extensions)
				{
					WsdlParser.INamespaces namespaces2 = (WsdlParser.INamespaces)obj;
					namespaces2.UsedNamespace(namespaces);
				}
			}

			// Token: 0x060004D9 RID: 1241 RVA: 0x0001B978 File Offset: 0x0001A978
			public void Dump()
			{
				foreach (object obj in this.extensions)
				{
					WsdlParser.IDump dump = (WsdlParser.IDump)obj;
					dump.Dump();
				}
			}

			// Token: 0x040003FD RID: 1021
			internal string name;

			// Token: 0x040003FE RID: 1022
			internal string elementName;

			// Token: 0x040003FF RID: 1023
			internal ArrayList extensions = new ArrayList(10);
		}

		// Token: 0x0200009B RID: 155
		internal class WsdlBindingSoapBinding : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004DB RID: 1243 RVA: 0x0001B9E5 File Offset: 0x0001A9E5
			public void UsedNamespace(Hashtable namespaces)
			{
			}

			// Token: 0x060004DC RID: 1244 RVA: 0x0001B9E7 File Offset: 0x0001A9E7
			public void Dump()
			{
			}

			// Token: 0x04000400 RID: 1024
			internal string style;

			// Token: 0x04000401 RID: 1025
			internal string transport;
		}

		// Token: 0x0200009C RID: 156
		internal class WsdlBindingSoapBody : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004DE RID: 1246 RVA: 0x0001B9F1 File Offset: 0x0001A9F1
			public void UsedNamespace(Hashtable namespaces)
			{
			}

			// Token: 0x060004DF RID: 1247 RVA: 0x0001B9F3 File Offset: 0x0001A9F3
			public void Dump()
			{
			}

			// Token: 0x04000402 RID: 1026
			internal string parts;

			// Token: 0x04000403 RID: 1027
			internal string use;

			// Token: 0x04000404 RID: 1028
			internal string encodingStyle;

			// Token: 0x04000405 RID: 1029
			internal string namespaceUri;
		}

		// Token: 0x0200009D RID: 157
		internal class WsdlBindingSoapHeader : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004E1 RID: 1249 RVA: 0x0001B9FD File Offset: 0x0001A9FD
			public void UsedNamespace(Hashtable namespaces)
			{
			}

			// Token: 0x060004E2 RID: 1250 RVA: 0x0001B9FF File Offset: 0x0001A9FF
			public void Dump()
			{
			}

			// Token: 0x04000406 RID: 1030
			internal string message;

			// Token: 0x04000407 RID: 1031
			internal string messageNs;

			// Token: 0x04000408 RID: 1032
			internal string part;

			// Token: 0x04000409 RID: 1033
			internal string use;

			// Token: 0x0400040A RID: 1034
			internal string encodingStyle;

			// Token: 0x0400040B RID: 1035
			internal string namespaceUri;
		}

		// Token: 0x0200009E RID: 158
		internal class WsdlBindingSoapOperation : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004E4 RID: 1252 RVA: 0x0001BA09 File Offset: 0x0001AA09
			public void UsedNamespace(Hashtable namespaces)
			{
			}

			// Token: 0x060004E5 RID: 1253 RVA: 0x0001BA0B File Offset: 0x0001AA0B
			public void Dump()
			{
			}

			// Token: 0x0400040C RID: 1036
			internal string soapAction;

			// Token: 0x0400040D RID: 1037
			internal string style;
		}

		// Token: 0x0200009F RID: 159
		internal class WsdlBindingSoapFault : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004E7 RID: 1255 RVA: 0x0001BA15 File Offset: 0x0001AA15
			public void UsedNamespace(Hashtable namespaces)
			{
			}

			// Token: 0x060004E8 RID: 1256 RVA: 0x0001BA17 File Offset: 0x0001AA17
			public void Dump()
			{
			}

			// Token: 0x0400040E RID: 1038
			internal string name;

			// Token: 0x0400040F RID: 1039
			internal string use;

			// Token: 0x04000410 RID: 1040
			internal string encodingStyle;

			// Token: 0x04000411 RID: 1041
			internal string namespaceUri;
		}

		// Token: 0x020000A0 RID: 160
		internal enum SudsUse
		{
			// Token: 0x04000413 RID: 1043
			Class,
			// Token: 0x04000414 RID: 1044
			ISerializable,
			// Token: 0x04000415 RID: 1045
			Struct,
			// Token: 0x04000416 RID: 1046
			Interface,
			// Token: 0x04000417 RID: 1047
			MarshalByRef,
			// Token: 0x04000418 RID: 1048
			Delegate,
			// Token: 0x04000419 RID: 1049
			ServicedComponent
		}

		// Token: 0x020000A1 RID: 161
		internal class WsdlBindingSuds : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004EA RID: 1258 RVA: 0x0001BA24 File Offset: 0x0001AA24
			public void UsedNamespace(Hashtable namespaces)
			{
				if (this.ns != null)
				{
					namespaces[this.ns] = 1;
				}
				if (this.extendsNs != null)
				{
					namespaces[this.extendsNs] = 1;
				}
				foreach (object obj in this.implements)
				{
					WsdlParser.INamespaces namespaces2 = (WsdlParser.INamespaces)obj;
					namespaces2.UsedNamespace(namespaces);
				}
			}

			// Token: 0x060004EB RID: 1259 RVA: 0x0001BAB4 File Offset: 0x0001AAB4
			public void Dump()
			{
				foreach (object obj in this.implements)
				{
					WsdlParser.IDump dump = (WsdlParser.IDump)obj;
					dump.Dump();
				}
				foreach (object obj2 in this.nestedTypes)
				{
					WsdlParser.IDump dump2 = (WsdlParser.IDump)obj2;
					dump2.Dump();
				}
			}

			// Token: 0x0400041A RID: 1050
			internal string elementName;

			// Token: 0x0400041B RID: 1051
			internal string typeName;

			// Token: 0x0400041C RID: 1052
			internal string ns;

			// Token: 0x0400041D RID: 1053
			internal string extendsTypeName;

			// Token: 0x0400041E RID: 1054
			internal string extendsNs;

			// Token: 0x0400041F RID: 1055
			internal WsdlParser.SudsUse sudsUse;

			// Token: 0x04000420 RID: 1056
			internal ArrayList implements = new ArrayList(10);

			// Token: 0x04000421 RID: 1057
			internal ArrayList nestedTypes = new ArrayList(10);
		}

		// Token: 0x020000A2 RID: 162
		internal class WsdlBindingSudsImplements : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004ED RID: 1261 RVA: 0x0001BB7E File Offset: 0x0001AB7E
			public void UsedNamespace(Hashtable namespaces)
			{
				if (this.ns != null)
				{
					namespaces[this.ns] = 1;
				}
			}

			// Token: 0x060004EE RID: 1262 RVA: 0x0001BB9A File Offset: 0x0001AB9A
			public void Dump()
			{
			}

			// Token: 0x04000422 RID: 1058
			internal string typeName;

			// Token: 0x04000423 RID: 1059
			internal string ns;
		}

		// Token: 0x020000A3 RID: 163
		internal class WsdlBindingSudsNestedType : WsdlParser.IDump
		{
			// Token: 0x060004F0 RID: 1264 RVA: 0x0001BBA4 File Offset: 0x0001ABA4
			public void Dump()
			{
			}

			// Token: 0x04000424 RID: 1060
			internal string name;

			// Token: 0x04000425 RID: 1061
			internal string typeName;

			// Token: 0x04000426 RID: 1062
			internal string ns;
		}

		// Token: 0x020000A4 RID: 164
		internal class WsdlService : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004F2 RID: 1266 RVA: 0x0001BBB0 File Offset: 0x0001ABB0
			public void UsedNamespace(Hashtable namespaces)
			{
				foreach (object obj in this.ports)
				{
					((WsdlParser.INamespaces)((DictionaryEntry)obj).Value).UsedNamespace(namespaces);
				}
			}

			// Token: 0x060004F3 RID: 1267 RVA: 0x0001BC14 File Offset: 0x0001AC14
			public void Dump()
			{
				foreach (object obj in this.ports)
				{
					((WsdlParser.IDump)((DictionaryEntry)obj).Value).Dump();
				}
			}

			// Token: 0x04000427 RID: 1063
			internal string name;

			// Token: 0x04000428 RID: 1064
			internal Hashtable ports = new Hashtable(10);
		}

		// Token: 0x020000A5 RID: 165
		internal class WsdlServicePort : WsdlParser.IDump, WsdlParser.INamespaces
		{
			// Token: 0x060004F5 RID: 1269 RVA: 0x0001BC8D File Offset: 0x0001AC8D
			public void UsedNamespace(Hashtable namespaces)
			{
			}

			// Token: 0x060004F6 RID: 1270 RVA: 0x0001BC90 File Offset: 0x0001AC90
			public void Dump()
			{
				if (this.locations != null)
				{
					foreach (object obj in this.locations)
					{
						string text = (string)obj;
					}
				}
			}

			// Token: 0x04000429 RID: 1065
			internal string name;

			// Token: 0x0400042A RID: 1066
			internal string nameNs;

			// Token: 0x0400042B RID: 1067
			internal string binding;

			// Token: 0x0400042C RID: 1068
			internal string bindingNs;

			// Token: 0x0400042D RID: 1069
			internal ArrayList locations;
		}

		// Token: 0x020000A6 RID: 166
		internal class WsdlMethodInfo : WsdlParser.IDump
		{
			// Token: 0x060004F8 RID: 1272 RVA: 0x0001BCF4 File Offset: 0x0001ACF4
			public void Dump()
			{
				if (this.paramNamesOrder != null)
				{
					foreach (string text in this.paramNamesOrder)
					{
					}
				}
				if (this.inputNames != null)
				{
					for (int j = 0; j < this.inputNames.Length; j++)
					{
					}
				}
				if (this.outputNames != null)
				{
					for (int k = 0; k < this.outputNames.Length; k++)
					{
					}
				}
				bool flag = this.bProperty;
			}

			// Token: 0x0400042E RID: 1070
			internal string soapAction;

			// Token: 0x0400042F RID: 1071
			internal string methodName;

			// Token: 0x04000430 RID: 1072
			internal string methodNameNs;

			// Token: 0x04000431 RID: 1073
			internal string methodAttributes;

			// Token: 0x04000432 RID: 1074
			internal string[] paramNamesOrder;

			// Token: 0x04000433 RID: 1075
			internal string inputMethodName;

			// Token: 0x04000434 RID: 1076
			internal string inputMethodNameNs;

			// Token: 0x04000435 RID: 1077
			internal string outputMethodName;

			// Token: 0x04000436 RID: 1078
			internal string outputMethodNameNs;

			// Token: 0x04000437 RID: 1079
			internal string[] inputNames;

			// Token: 0x04000438 RID: 1080
			internal string[] inputNamesNs;

			// Token: 0x04000439 RID: 1081
			internal string[] inputElements;

			// Token: 0x0400043A RID: 1082
			internal string[] inputElementsNs;

			// Token: 0x0400043B RID: 1083
			internal string[] inputTypes;

			// Token: 0x0400043C RID: 1084
			internal string[] inputTypesNs;

			// Token: 0x0400043D RID: 1085
			internal string[] outputNames;

			// Token: 0x0400043E RID: 1086
			internal string[] outputNamesNs;

			// Token: 0x0400043F RID: 1087
			internal string[] outputElements;

			// Token: 0x04000440 RID: 1088
			internal string[] outputElementsNs;

			// Token: 0x04000441 RID: 1089
			internal string[] outputTypes;

			// Token: 0x04000442 RID: 1090
			internal string[] outputTypesNs;

			// Token: 0x04000443 RID: 1091
			internal string propertyName;

			// Token: 0x04000444 RID: 1092
			internal bool bProperty;

			// Token: 0x04000445 RID: 1093
			internal bool bGet;

			// Token: 0x04000446 RID: 1094
			internal bool bSet;

			// Token: 0x04000447 RID: 1095
			internal string propertyType;

			// Token: 0x04000448 RID: 1096
			internal string propertyNs;

			// Token: 0x04000449 RID: 1097
			internal string soapActionGet;

			// Token: 0x0400044A RID: 1098
			internal string soapActionSet;
		}
	}
}
