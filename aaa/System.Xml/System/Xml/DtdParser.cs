using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Xml
{
	// Token: 0x020001EC RID: 492
	internal class DtdParser
	{
		// Token: 0x06001777 RID: 6007 RVA: 0x000646CC File Offset: 0x000636CC
		internal DtdParser(IDtdParserAdapter readerAdapter)
		{
			this.readerAdapter = readerAdapter;
			this.nameTable = readerAdapter.NameTable;
			this.validate = readerAdapter.DtdValidation;
			this.normalize = readerAdapter.Normalization;
			this.supportNamespaces = readerAdapter.Namespaces;
			this.v1Compat = readerAdapter.V1CompatibilityMode;
			this.schemaInfo = new SchemaInfo();
			this.schemaInfo.SchemaType = SchemaType.DTD;
			this.stringBuilder = new BufferBuilder();
			Uri baseUri = readerAdapter.BaseUri;
			if (baseUri != null)
			{
				this.documentBaseUri = baseUri.ToString();
			}
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x000647C0 File Offset: 0x000637C0
		internal DtdParser(string baseUri, string docTypeName, string publicId, string systemId, string internalSubset, IDtdParserAdapter underlyingReader)
			: this(underlyingReader)
		{
			if (docTypeName == null || docTypeName.Length == 0)
			{
				throw XmlConvert.CreateInvalidNameArgumentException(docTypeName, "docTypeName");
			}
			XmlConvert.VerifyName(docTypeName);
			int num = docTypeName.IndexOf(':');
			if (num == -1)
			{
				this.schemaInfo.DocTypeName = new XmlQualifiedName(this.nameTable.Add(docTypeName));
			}
			else
			{
				this.schemaInfo.DocTypeName = new XmlQualifiedName(this.nameTable.Add(docTypeName.Substring(0, num)), this.nameTable.Add(docTypeName.Substring(num + 1)));
			}
			if (systemId != null && systemId.Length > 0)
			{
				int num2;
				if ((num2 = this.xmlCharType.IsOnlyCharData(systemId)) >= 0)
				{
					this.ThrowInvalidChar(this.curPos, systemId[num2]);
				}
				this.systemId = systemId;
			}
			if (publicId != null && publicId.Length > 0)
			{
				int num2;
				if ((num2 = this.xmlCharType.IsPublicId(publicId)) >= 0)
				{
					this.ThrowInvalidChar(this.curPos, publicId[num2]);
				}
				this.publicId = publicId;
			}
			if (internalSubset != null && internalSubset.Length > 0)
			{
				this.readerAdapter.PushInternalDtd(baseUri, internalSubset);
				this.hasFreeFloatingInternalSubset = true;
			}
			Uri baseUri2 = this.readerAdapter.BaseUri;
			if (baseUri2 != null)
			{
				this.documentBaseUri = baseUri2.ToString();
			}
			this.freeFloatingDtd = true;
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x00064914 File Offset: 0x00063914
		internal static SchemaInfo Parse(XmlNameTable nt, XmlNamespaceManager nsManager, bool namespaces, string baseUri, string docTypeName, string publicId, string systemId, string internalSubset, bool useResolver, XmlResolver resolver)
		{
			XmlParserContext xmlParserContext = new XmlParserContext(nt, nsManager, null, null, null, null, baseUri, string.Empty, XmlSpace.None);
			XmlTextReaderImpl xmlTextReaderImpl = new XmlTextReaderImpl("", XmlNodeType.Element, xmlParserContext);
			xmlTextReaderImpl.Namespaces = namespaces;
			if (useResolver)
			{
				xmlTextReaderImpl.XmlResolver = resolver;
			}
			return DtdParser.Parse(xmlTextReaderImpl, baseUri, docTypeName, publicId, systemId, internalSubset);
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x00064964 File Offset: 0x00063964
		internal static SchemaInfo Parse(XmlTextReaderImpl tr, string baseUri, string docTypeName, string publicId, string systemId, string internalSubset)
		{
			XmlTextReaderImpl.DtdParserProxy dtdParserProxy = new XmlTextReaderImpl.DtdParserProxy(baseUri, docTypeName, publicId, systemId, internalSubset, tr);
			dtdParserProxy.Parse(false);
			return dtdParserProxy.DtdSchemaInfo;
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x0600177B RID: 6011 RVA: 0x0006498C File Offset: 0x0006398C
		internal string SystemID
		{
			get
			{
				return this.systemId;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x0600177C RID: 6012 RVA: 0x00064994 File Offset: 0x00063994
		internal string PublicID
		{
			get
			{
				return this.publicId;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x0600177D RID: 6013 RVA: 0x0006499C File Offset: 0x0006399C
		internal string InternalSubset
		{
			get
			{
				return this.internalSubsetValue;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x0600177E RID: 6014 RVA: 0x000649A4 File Offset: 0x000639A4
		internal SchemaInfo SchemaInfo
		{
			get
			{
				return this.schemaInfo;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x0600177F RID: 6015 RVA: 0x000649AC File Offset: 0x000639AC
		private bool ParsingInternalSubset
		{
			get
			{
				return this.externalEntitiesDepth == 0;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001780 RID: 6016 RVA: 0x000649B7 File Offset: 0x000639B7
		private bool IgnoreEntityReferences
		{
			get
			{
				return this.scanningFunction == DtdParser.ScanningFunction.CondSection3;
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001781 RID: 6017 RVA: 0x000649C3 File Offset: 0x000639C3
		private bool SaveInternalSubsetValue
		{
			get
			{
				return this.readerAdapter.EntityStackLength == 0 && this.internalSubsetValueSb != null;
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001782 RID: 6018 RVA: 0x000649E0 File Offset: 0x000639E0
		private bool ParsingTopLevelMarkup
		{
			get
			{
				return this.scanningFunction == DtdParser.ScanningFunction.SubsetContent || (this.scanningFunction == DtdParser.ScanningFunction.ParamEntitySpace && this.savedScanningFunction == DtdParser.ScanningFunction.SubsetContent);
			}
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x00064A04 File Offset: 0x00063A04
		internal void Parse(bool saveInternalSubset)
		{
			if (this.freeFloatingDtd)
			{
				this.ParseFreeFloatingDtd();
			}
			else
			{
				this.ParseInDocumentDtd(saveInternalSubset);
			}
			this.schemaInfo.Finish();
			if (this.validate && this.undeclaredNotations != null)
			{
				foreach (object obj in this.undeclaredNotations.Values)
				{
					DtdParser.UndeclaredNotation undeclaredNotation = (DtdParser.UndeclaredNotation)obj;
					for (DtdParser.UndeclaredNotation undeclaredNotation2 = undeclaredNotation; undeclaredNotation2 != null; undeclaredNotation2 = undeclaredNotation2.next)
					{
						this.SendValidationEvent(XmlSeverityType.Error, new XmlSchemaException("Sch_UndeclaredNotation", undeclaredNotation.name, this.BaseUriStr, undeclaredNotation.lineNo, undeclaredNotation.linePos));
					}
				}
			}
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x00064AC4 File Offset: 0x00063AC4
		private void ParseInDocumentDtd(bool saveInternalSubset)
		{
			this.LoadParsingBuffer();
			this.scanningFunction = DtdParser.ScanningFunction.QName;
			this.nextScaningFunction = DtdParser.ScanningFunction.Doctype1;
			if (this.GetToken(false) != DtdParser.Token.QName)
			{
				this.OnUnexpectedError();
			}
			this.schemaInfo.DocTypeName = this.GetNameQualified(true);
			DtdParser.Token token = this.GetToken(false);
			if (token == DtdParser.Token.SYSTEM || token == DtdParser.Token.PUBLIC)
			{
				this.ParseExternalId(token, DtdParser.Token.DOCTYPE, out this.publicId, out this.systemId);
				token = this.GetToken(false);
			}
			switch (token)
			{
			case DtdParser.Token.GreaterThan:
				goto IL_00A1;
			case DtdParser.Token.LeftBracket:
				if (saveInternalSubset)
				{
					this.SaveParsingBuffer();
					this.internalSubsetValueSb = new BufferBuilder();
				}
				this.ParseInternalSubset();
				goto IL_00A1;
			}
			this.OnUnexpectedError();
			IL_00A1:
			this.SaveParsingBuffer();
			if (this.systemId != null && this.systemId.Length > 0)
			{
				this.ParseExternalSubset();
			}
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x00064B94 File Offset: 0x00063B94
		private void ParseFreeFloatingDtd()
		{
			if (this.hasFreeFloatingInternalSubset)
			{
				this.LoadParsingBuffer();
				this.ParseInternalSubset();
				this.SaveParsingBuffer();
			}
			if (this.systemId != null && this.systemId.Length > 0)
			{
				this.ParseExternalSubset();
			}
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x00064BCC File Offset: 0x00063BCC
		private void ParseInternalSubset()
		{
			this.ParseSubset();
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00064BD4 File Offset: 0x00063BD4
		private void ParseExternalSubset()
		{
			if (!this.readerAdapter.PushExternalSubset(this.systemId, this.publicId))
			{
				return;
			}
			Uri baseUri = this.readerAdapter.BaseUri;
			if (baseUri != null)
			{
				this.externalDtdBaseUri = baseUri.ToString();
			}
			this.externalEntitiesDepth++;
			this.LoadParsingBuffer();
			this.ParseSubset();
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x00064C38 File Offset: 0x00063C38
		private void ParseSubset()
		{
			for (;;)
			{
				DtdParser.Token token = this.GetToken(false);
				int num = this.currentEntityId;
				DtdParser.Token token2 = token;
				switch (token2)
				{
				case DtdParser.Token.AttlistDecl:
					this.ParseAttlistDecl();
					break;
				case DtdParser.Token.ElementDecl:
					this.ParseElementDecl();
					break;
				case DtdParser.Token.EntityDecl:
					this.ParseEntityDecl();
					break;
				case DtdParser.Token.NotationDecl:
					this.ParseNotationDecl();
					break;
				case DtdParser.Token.Comment:
					this.ParseComment();
					break;
				case DtdParser.Token.PI:
					this.ParsePI();
					break;
				case DtdParser.Token.CondSectionStart:
					if (this.ParsingInternalSubset)
					{
						this.Throw(this.curPos - 3, "Xml_InvalidConditionalSection");
					}
					this.ParseCondSection();
					num = this.currentEntityId;
					break;
				case DtdParser.Token.CondSectionEnd:
					if (this.condSectionDepth > 0)
					{
						this.condSectionDepth--;
						if (this.validate && this.currentEntityId != this.condSectionEntityIds[this.condSectionDepth])
						{
							this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "Sch_ParEntityRefNesting", string.Empty);
						}
					}
					else
					{
						this.Throw(this.curPos - 3, "Xml_UnexpectedCDataEnd");
					}
					break;
				case DtdParser.Token.Eof:
					goto IL_01A6;
				default:
					if (token2 == DtdParser.Token.RightBracket)
					{
						goto IL_0128;
					}
					break;
				}
				if (this.currentEntityId != num)
				{
					if (this.validate)
					{
						this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "Sch_ParEntityRefNesting", string.Empty);
					}
					else if (!this.v1Compat)
					{
						this.Throw(this.curPos, "Sch_ParEntityRefNesting");
					}
				}
			}
			IL_0128:
			if (this.ParsingInternalSubset)
			{
				if (this.condSectionDepth != 0)
				{
					this.Throw(this.curPos, "Xml_UnclosedConditionalSection");
				}
				if (this.internalSubsetValueSb != null)
				{
					this.SaveParsingBuffer(this.curPos - 1);
					this.internalSubsetValue = this.internalSubsetValueSb.ToString();
					this.internalSubsetValueSb = null;
				}
				if (this.GetToken(false) != DtdParser.Token.GreaterThan)
				{
					this.ThrowUnexpectedToken(this.curPos, ">");
					return;
				}
			}
			else
			{
				this.Throw(this.curPos, "Xml_ExpectDtdMarkup");
			}
			return;
			IL_01A6:
			if (this.ParsingInternalSubset && !this.freeFloatingDtd)
			{
				this.Throw(this.curPos, "Xml_IncompleteDtdContent");
			}
			if (this.condSectionDepth != 0)
			{
				this.Throw(this.curPos, "Xml_UnclosedConditionalSection");
			}
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x00064E78 File Offset: 0x00063E78
		private void ParseAttlistDecl()
		{
			if (this.GetToken(true) == DtdParser.Token.QName)
			{
				XmlQualifiedName nameQualified = this.GetNameQualified(true);
				SchemaElementDecl schemaElementDecl = (SchemaElementDecl)this.schemaInfo.ElementDecls[nameQualified];
				if (schemaElementDecl == null)
				{
					schemaElementDecl = (SchemaElementDecl)this.schemaInfo.UndeclaredElementDecls[nameQualified];
					if (schemaElementDecl == null)
					{
						schemaElementDecl = new SchemaElementDecl(nameQualified, nameQualified.Namespace, SchemaType.DTD);
						this.schemaInfo.UndeclaredElementDecls.Add(nameQualified, schemaElementDecl);
					}
				}
				SchemaAttDef schemaAttDef = null;
				DtdParser.Token token;
				for (;;)
				{
					token = this.GetToken(false);
					if (token != DtdParser.Token.QName)
					{
						break;
					}
					XmlQualifiedName nameQualified2 = this.GetNameQualified(true);
					schemaAttDef = new SchemaAttDef(nameQualified2, nameQualified2.Namespace);
					schemaAttDef.IsDeclaredInExternal = !this.ParsingInternalSubset;
					schemaAttDef.LineNum = this.LineNo;
					schemaAttDef.LinePos = this.LinePos - (this.curPos - this.tokenStartPos);
					this.ParseAttlistType(schemaAttDef, schemaElementDecl);
					this.ParseAttlistDefault(schemaAttDef);
					if (schemaAttDef.Prefix.Length > 0 && schemaAttDef.Prefix.Equals("xml"))
					{
						if (schemaAttDef.Name.Name == "space")
						{
							if (this.v1Compat)
							{
								string text = schemaAttDef.DefaultValueExpanded.Trim();
								if (text.Equals("preserve") || text.Equals("default"))
								{
									schemaAttDef.Reserved = SchemaAttDef.Reserve.XmlSpace;
								}
							}
							else
							{
								schemaAttDef.Reserved = SchemaAttDef.Reserve.XmlSpace;
								if (schemaAttDef.Datatype.TokenizedType != XmlTokenizedType.ENUMERATION)
								{
									this.Throw("Xml_EnumerationRequired", string.Empty, schemaAttDef.LineNum, schemaAttDef.LinePos);
								}
								if (this.readerAdapter.EventHandler != null)
								{
									schemaAttDef.CheckXmlSpace(this.readerAdapter.EventHandler);
								}
							}
						}
						else if (schemaAttDef.Name.Name == "lang")
						{
							schemaAttDef.Reserved = SchemaAttDef.Reserve.XmlLang;
						}
					}
					if (schemaElementDecl.GetAttDef(schemaAttDef.Name) == null)
					{
						schemaElementDecl.AddAttDef(schemaAttDef);
					}
				}
				if (token == DtdParser.Token.GreaterThan)
				{
					if (this.v1Compat && schemaAttDef != null && schemaAttDef.Prefix.Length > 0 && schemaAttDef.Prefix.Equals("xml") && schemaAttDef.Name.Name == "space")
					{
						schemaAttDef.Reserved = SchemaAttDef.Reserve.XmlSpace;
						if (schemaAttDef.Datatype.TokenizedType != XmlTokenizedType.ENUMERATION)
						{
							this.Throw("Xml_EnumerationRequired", string.Empty, schemaAttDef.LineNum, schemaAttDef.LinePos);
						}
						if (this.readerAdapter.EventHandler != null)
						{
							schemaAttDef.CheckXmlSpace(this.readerAdapter.EventHandler);
						}
					}
					return;
				}
			}
			this.OnUnexpectedError();
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x00065110 File Offset: 0x00064110
		private void ParseAttlistType(SchemaAttDef attrDef, SchemaElementDecl elementDecl)
		{
			DtdParser.Token token = this.GetToken(true);
			if (token != DtdParser.Token.CDATA)
			{
				elementDecl.HasNonCDataAttribute = true;
			}
			if (this.IsAttributeValueType(token))
			{
				attrDef.Datatype = XmlSchemaDatatype.FromXmlTokenizedType((XmlTokenizedType)token);
				attrDef.SchemaType = XmlSchemaType.GetBuiltInSimpleType(attrDef.Datatype.TypeCode);
				DtdParser.Token token2 = token;
				if (token2 == DtdParser.Token.ID)
				{
					if (this.validate && elementDecl.IsIdDeclared)
					{
						SchemaAttDef attDef = elementDecl.GetAttDef(attrDef.Name);
						if (attDef == null || attDef.Datatype.TokenizedType != XmlTokenizedType.ID)
						{
							this.SendValidationEvent(XmlSeverityType.Error, "Sch_IdAttrDeclared", elementDecl.Name.ToString());
						}
					}
					elementDecl.IsIdDeclared = true;
					return;
				}
				if (token2 != DtdParser.Token.NOTATION)
				{
					return;
				}
				if (this.validate)
				{
					if (elementDecl.IsNotationDeclared)
					{
						this.SendValidationEvent(this.curPos - 8, XmlSeverityType.Error, "Sch_DupNotationAttribute", elementDecl.Name.ToString());
					}
					else
					{
						if (elementDecl.ContentValidator != null && elementDecl.ContentValidator.ContentType == XmlSchemaContentType.Empty)
						{
							this.SendValidationEvent(this.curPos - 8, XmlSeverityType.Error, "Sch_NotationAttributeOnEmptyElement", elementDecl.Name.ToString());
						}
						elementDecl.IsNotationDeclared = true;
					}
				}
				if (this.GetToken(true) == DtdParser.Token.LeftParen && this.GetToken(false) == DtdParser.Token.Name)
				{
					for (;;)
					{
						string nameString = this.GetNameString();
						if (this.schemaInfo.Notations[nameString] == null)
						{
							if (this.undeclaredNotations == null)
							{
								this.undeclaredNotations = new Hashtable();
							}
							DtdParser.UndeclaredNotation undeclaredNotation = new DtdParser.UndeclaredNotation(nameString, this.LineNo, this.LinePos - nameString.Length);
							DtdParser.UndeclaredNotation undeclaredNotation2 = (DtdParser.UndeclaredNotation)this.undeclaredNotations[nameString];
							if (undeclaredNotation2 != null)
							{
								undeclaredNotation.next = undeclaredNotation2.next;
								undeclaredNotation2.next = undeclaredNotation;
							}
							else
							{
								this.undeclaredNotations.Add(nameString, undeclaredNotation);
							}
						}
						if (this.validate && !this.v1Compat && attrDef.Values != null && attrDef.Values.Contains(nameString))
						{
							this.SendValidationEvent(XmlSeverityType.Error, new XmlSchemaException("Xml_AttlistDuplNotationValue", nameString, this.BaseUriStr, this.LineNo, this.LinePos));
						}
						attrDef.AddValue(nameString);
						switch (this.GetToken(false))
						{
						case DtdParser.Token.RightParen:
							return;
						case DtdParser.Token.Or:
							if (this.GetToken(false) == DtdParser.Token.Name)
							{
								continue;
							}
							break;
						}
						break;
					}
				}
			}
			else if (token == DtdParser.Token.LeftParen)
			{
				attrDef.Datatype = XmlSchemaDatatype.FromXmlTokenizedType(XmlTokenizedType.ENUMERATION);
				attrDef.SchemaType = XmlSchemaType.GetBuiltInSimpleType(attrDef.Datatype.TypeCode);
				if (this.GetToken(false) == DtdParser.Token.Nmtoken)
				{
					attrDef.AddValue(this.GetNameString());
					for (;;)
					{
						switch (this.GetToken(false))
						{
						case DtdParser.Token.RightParen:
							return;
						case DtdParser.Token.Or:
							if (this.GetToken(false) == DtdParser.Token.Nmtoken)
							{
								string nmtokenString = this.GetNmtokenString();
								if (this.validate && !this.v1Compat && attrDef.Values != null && attrDef.Values.Contains(nmtokenString))
								{
									this.SendValidationEvent(XmlSeverityType.Error, new XmlSchemaException("Xml_AttlistDuplEnumValue", nmtokenString, this.BaseUriStr, this.LineNo, this.LinePos));
								}
								attrDef.AddValue(nmtokenString);
								continue;
							}
							break;
						}
						break;
					}
				}
			}
			this.OnUnexpectedError();
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x00065428 File Offset: 0x00064428
		private void ParseAttlistDefault(SchemaAttDef attrDef)
		{
			DtdParser.Token token = this.GetToken(true);
			switch (token)
			{
			case DtdParser.Token.REQUIRED:
				attrDef.Presence = SchemaDeclBase.Use.Required;
				return;
			case DtdParser.Token.IMPLIED:
				attrDef.Presence = SchemaDeclBase.Use.Implied;
				return;
			case DtdParser.Token.FIXED:
				attrDef.Presence = SchemaDeclBase.Use.Fixed;
				if (this.GetToken(true) != DtdParser.Token.Literal)
				{
					goto IL_00D1;
				}
				break;
			default:
				if (token != DtdParser.Token.Literal)
				{
					goto IL_00D1;
				}
				break;
			}
			if (this.validate && attrDef.Datatype.TokenizedType == XmlTokenizedType.ID)
			{
				this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "Sch_AttListPresence", string.Empty);
			}
			if (attrDef.Datatype.TokenizedType != XmlTokenizedType.CDATA)
			{
				attrDef.DefaultValueExpanded = this.GetValueWithStrippedSpaces();
			}
			else
			{
				attrDef.DefaultValueExpanded = this.GetValue();
			}
			attrDef.ValueLineNum = this.literalLineInfo.lineNo;
			attrDef.ValueLinePos = this.literalLineInfo.linePos + 1;
			DtdValidator.SetDefaultTypedValue(attrDef, this.readerAdapter);
			return;
			IL_00D1:
			this.OnUnexpectedError();
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x0006550C File Offset: 0x0006450C
		private void ParseElementDecl()
		{
			if (this.GetToken(true) == DtdParser.Token.QName)
			{
				XmlQualifiedName nameQualified = this.GetNameQualified(true);
				SchemaElementDecl schemaElementDecl = (SchemaElementDecl)this.schemaInfo.ElementDecls[nameQualified];
				if (schemaElementDecl != null)
				{
					if (this.validate)
					{
						this.SendValidationEvent(this.curPos - nameQualified.Name.Length, XmlSeverityType.Error, "Sch_DupElementDecl", this.GetNameString());
					}
				}
				else
				{
					if ((schemaElementDecl = (SchemaElementDecl)this.schemaInfo.UndeclaredElementDecls[nameQualified]) != null)
					{
						this.schemaInfo.UndeclaredElementDecls.Remove(nameQualified);
					}
					else
					{
						schemaElementDecl = new SchemaElementDecl(nameQualified, nameQualified.Namespace, SchemaType.DTD);
					}
					this.schemaInfo.ElementDecls.Add(nameQualified, schemaElementDecl);
				}
				schemaElementDecl.IsDeclaredInExternal = !this.ParsingInternalSubset;
				DtdParser.Token token = this.GetToken(true);
				if (token != DtdParser.Token.LeftParen)
				{
					switch (token)
					{
					case DtdParser.Token.ANY:
						schemaElementDecl.ContentValidator = ContentValidator.Any;
						break;
					case DtdParser.Token.EMPTY:
						schemaElementDecl.ContentValidator = ContentValidator.Empty;
						break;
					default:
						goto IL_0192;
					}
				}
				else
				{
					int num = this.currentEntityId;
					DtdParser.Token token2 = this.GetToken(false);
					if (token2 != DtdParser.Token.None)
					{
						if (token2 != DtdParser.Token.PCDATA)
						{
							goto IL_0192;
						}
						ParticleContentValidator particleContentValidator = new ParticleContentValidator(XmlSchemaContentType.Mixed);
						particleContentValidator.Start();
						particleContentValidator.OpenGroup();
						this.ParseElementMixedContent(particleContentValidator, num);
						schemaElementDecl.ContentValidator = particleContentValidator.Finish(true);
					}
					else
					{
						ParticleContentValidator particleContentValidator2 = new ParticleContentValidator(XmlSchemaContentType.ElementOnly);
						particleContentValidator2.Start();
						particleContentValidator2.OpenGroup();
						this.ParseElementOnlyContent(particleContentValidator2, num);
						schemaElementDecl.ContentValidator = particleContentValidator2.Finish(true);
					}
				}
				if (this.GetToken(false) != DtdParser.Token.GreaterThan)
				{
					this.ThrowUnexpectedToken(this.curPos, ">");
				}
				return;
			}
			IL_0192:
			this.OnUnexpectedError();
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x000656B4 File Offset: 0x000646B4
		private void ParseElementOnlyContent(ParticleContentValidator pcv, int startParenEntityId)
		{
			Stack<DtdParser.ParseElementOnlyContent_LocalFrame> stack = new Stack<DtdParser.ParseElementOnlyContent_LocalFrame>();
			DtdParser.ParseElementOnlyContent_LocalFrame parseElementOnlyContent_LocalFrame = new DtdParser.ParseElementOnlyContent_LocalFrame(startParenEntityId);
			stack.Push(parseElementOnlyContent_LocalFrame);
			for (;;)
			{
				DtdParser.Token token = this.GetToken(false);
				if (token != DtdParser.Token.QName)
				{
					switch (token)
					{
					case DtdParser.Token.LeftParen:
						pcv.OpenGroup();
						if (this.validate)
						{
							parseElementOnlyContent_LocalFrame.contentEntityId = this.currentEntityId;
							if (parseElementOnlyContent_LocalFrame.connectorEntityId > parseElementOnlyContent_LocalFrame.contentEntityId)
							{
								this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "Sch_ParEntityRefNesting", string.Empty);
							}
						}
						parseElementOnlyContent_LocalFrame = new DtdParser.ParseElementOnlyContent_LocalFrame(this.currentEntityId);
						stack.Push(parseElementOnlyContent_LocalFrame);
						continue;
					case DtdParser.Token.RightParen:
						goto IL_0246;
					case DtdParser.Token.GreaterThan:
						this.Throw(this.curPos, "Xml_InvalidContentModel");
						goto IL_024C;
					default:
						goto IL_0246;
					}
				}
				else
				{
					pcv.AddName(this.GetNameQualified(true), null);
					if (this.validate)
					{
						parseElementOnlyContent_LocalFrame.contentEntityId = this.currentEntityId;
						if (parseElementOnlyContent_LocalFrame.connectorEntityId > parseElementOnlyContent_LocalFrame.contentEntityId)
						{
							this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "Sch_ParEntityRefNesting", string.Empty);
						}
					}
					this.ParseHowMany(pcv);
				}
				IL_00F8:
				DtdParser.Token token2 = this.GetToken(false);
				switch (token2)
				{
				case DtdParser.Token.RightParen:
					pcv.CloseGroup();
					if (this.validate && this.currentEntityId != parseElementOnlyContent_LocalFrame.startParenEntityId)
					{
						this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "Sch_ParEntityRefNesting", string.Empty);
					}
					this.ParseHowMany(pcv);
					break;
				case DtdParser.Token.GreaterThan:
					this.Throw(this.curPos, "Xml_InvalidContentModel");
					break;
				case DtdParser.Token.Or:
					if (parseElementOnlyContent_LocalFrame.parsingSchema == DtdParser.Token.Comma)
					{
						this.Throw(this.curPos, "Xml_InvalidContentModel");
					}
					pcv.AddChoice();
					parseElementOnlyContent_LocalFrame.parsingSchema = DtdParser.Token.Or;
					if (!this.validate)
					{
						continue;
					}
					parseElementOnlyContent_LocalFrame.connectorEntityId = this.currentEntityId;
					if (parseElementOnlyContent_LocalFrame.connectorEntityId > parseElementOnlyContent_LocalFrame.contentEntityId)
					{
						this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "Sch_ParEntityRefNesting", string.Empty);
						continue;
					}
					continue;
				default:
					if (token2 != DtdParser.Token.Comma)
					{
						goto IL_0246;
					}
					if (parseElementOnlyContent_LocalFrame.parsingSchema == DtdParser.Token.Or)
					{
						this.Throw(this.curPos, "Xml_InvalidContentModel");
					}
					pcv.AddSequence();
					parseElementOnlyContent_LocalFrame.parsingSchema = DtdParser.Token.Comma;
					if (!this.validate)
					{
						continue;
					}
					parseElementOnlyContent_LocalFrame.connectorEntityId = this.currentEntityId;
					if (parseElementOnlyContent_LocalFrame.connectorEntityId > parseElementOnlyContent_LocalFrame.contentEntityId)
					{
						this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "Sch_ParEntityRefNesting", string.Empty);
						continue;
					}
					continue;
				}
				IL_024C:
				stack.Pop();
				if (stack.Count > 0)
				{
					parseElementOnlyContent_LocalFrame = stack.Peek();
					goto IL_00F8;
				}
				break;
				IL_0246:
				this.OnUnexpectedError();
				goto IL_024C;
			}
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x0006592C File Offset: 0x0006492C
		private void ParseHowMany(ParticleContentValidator pcv)
		{
			switch (this.GetToken(false))
			{
			case DtdParser.Token.Star:
				pcv.AddStar();
				return;
			case DtdParser.Token.QMark:
				pcv.AddQMark();
				return;
			case DtdParser.Token.Plus:
				pcv.AddPlus();
				return;
			default:
				return;
			}
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x0006596C File Offset: 0x0006496C
		private void ParseElementMixedContent(ParticleContentValidator pcv, int startParenEntityId)
		{
			bool flag = false;
			int num = -1;
			int num2 = this.currentEntityId;
			for (;;)
			{
				switch (this.GetToken(false))
				{
				case DtdParser.Token.RightParen:
					goto IL_002F;
				case DtdParser.Token.Or:
					if (!flag)
					{
						flag = true;
					}
					else
					{
						pcv.AddChoice();
					}
					if (this.validate)
					{
						num = this.currentEntityId;
						if (num2 < num)
						{
							this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "Sch_ParEntityRefNesting", string.Empty);
						}
					}
					if (this.GetToken(false) == DtdParser.Token.QName)
					{
						XmlQualifiedName nameQualified = this.GetNameQualified(true);
						if (pcv.Exists(nameQualified) && this.validate)
						{
							this.SendValidationEvent(XmlSeverityType.Error, "Sch_DupElement", nameQualified.ToString());
						}
						pcv.AddName(nameQualified, null);
						if (!this.validate)
						{
							continue;
						}
						num2 = this.currentEntityId;
						if (num2 < num)
						{
							this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "Sch_ParEntityRefNesting", string.Empty);
							continue;
						}
						continue;
					}
					break;
				}
				this.OnUnexpectedError();
			}
			IL_002F:
			pcv.CloseGroup();
			if (this.validate && this.currentEntityId != startParenEntityId)
			{
				this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "Sch_ParEntityRefNesting", string.Empty);
			}
			if (this.GetToken(false) == DtdParser.Token.Star && flag)
			{
				pcv.AddStar();
				return;
			}
			if (flag)
			{
				this.ThrowUnexpectedToken(this.curPos, "*");
			}
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x00065AB4 File Offset: 0x00064AB4
		private void ParseEntityDecl()
		{
			bool flag = false;
			DtdParser.Token token = this.GetToken(true);
			if (token != DtdParser.Token.Name)
			{
				if (token != DtdParser.Token.Percent)
				{
					goto IL_024E;
				}
				flag = true;
				if (this.GetToken(true) != DtdParser.Token.Name)
				{
					goto IL_024E;
				}
			}
			XmlQualifiedName nameQualified = this.GetNameQualified(false);
			SchemaEntity schemaEntity = new SchemaEntity(nameQualified, flag);
			schemaEntity.BaseURI = this.BaseUriStr;
			schemaEntity.DeclaredURI = ((this.externalDtdBaseUri.Length == 0) ? this.documentBaseUri : this.externalDtdBaseUri);
			if (flag)
			{
				if (this.schemaInfo.ParameterEntities[nameQualified] == null)
				{
					this.schemaInfo.ParameterEntities.Add(nameQualified, schemaEntity);
				}
			}
			else if (this.schemaInfo.GeneralEntities[nameQualified] == null)
			{
				this.schemaInfo.GeneralEntities.Add(nameQualified, schemaEntity);
			}
			schemaEntity.DeclaredInExternal = !this.ParsingInternalSubset;
			schemaEntity.IsProcessed = true;
			DtdParser.Token token2 = this.GetToken(true);
			switch (token2)
			{
			case DtdParser.Token.PUBLIC:
			case DtdParser.Token.SYSTEM:
			{
				string text;
				string text2;
				this.ParseExternalId(token2, DtdParser.Token.EntityDecl, out text, out text2);
				schemaEntity.IsExternal = true;
				schemaEntity.Url = text2;
				schemaEntity.Pubid = text;
				if (this.GetToken(false) == DtdParser.Token.NData)
				{
					if (flag)
					{
						this.ThrowUnexpectedToken(this.curPos - 5, ">");
					}
					if (!this.whitespaceSeen)
					{
						this.Throw(this.curPos - 5, "Xml_ExpectingWhiteSpace", "NDATA");
					}
					if (this.GetToken(true) != DtdParser.Token.Name)
					{
						goto IL_024E;
					}
					schemaEntity.NData = this.GetNameQualified(false);
					string name = schemaEntity.NData.Name;
					if (this.schemaInfo.Notations[name] == null)
					{
						if (this.undeclaredNotations == null)
						{
							this.undeclaredNotations = new Hashtable();
						}
						DtdParser.UndeclaredNotation undeclaredNotation = new DtdParser.UndeclaredNotation(name, this.LineNo, this.LinePos - name.Length);
						DtdParser.UndeclaredNotation undeclaredNotation2 = (DtdParser.UndeclaredNotation)this.undeclaredNotations[name];
						if (undeclaredNotation2 != null)
						{
							undeclaredNotation.next = undeclaredNotation2.next;
							undeclaredNotation2.next = undeclaredNotation;
						}
						else
						{
							this.undeclaredNotations.Add(name, undeclaredNotation);
						}
					}
				}
				break;
			}
			case DtdParser.Token.Literal:
				schemaEntity.Text = this.GetValue();
				schemaEntity.Line = this.literalLineInfo.lineNo;
				schemaEntity.Pos = this.literalLineInfo.linePos;
				break;
			default:
				goto IL_024E;
			}
			if (this.GetToken(false) == DtdParser.Token.GreaterThan)
			{
				schemaEntity.IsProcessed = false;
				return;
			}
			IL_024E:
			this.OnUnexpectedError();
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x00065D18 File Offset: 0x00064D18
		private void ParseNotationDecl()
		{
			if (this.GetToken(true) != DtdParser.Token.Name)
			{
				this.OnUnexpectedError();
			}
			SchemaNotation schemaNotation = null;
			XmlQualifiedName nameQualified = this.GetNameQualified(false);
			if (this.schemaInfo.Notations[nameQualified.Name] == null)
			{
				if (this.undeclaredNotations != null)
				{
					this.undeclaredNotations.Remove(nameQualified.Name);
				}
				schemaNotation = new SchemaNotation(nameQualified);
				this.schemaInfo.Notations.Add(schemaNotation.Name.Name, schemaNotation);
			}
			else if (this.validate)
			{
				this.SendValidationEvent(this.curPos - nameQualified.Name.Length, XmlSeverityType.Error, "Sch_DupNotation", nameQualified.Name);
			}
			DtdParser.Token token = this.GetToken(true);
			if (token == DtdParser.Token.SYSTEM || token == DtdParser.Token.PUBLIC)
			{
				string text;
				string text2;
				this.ParseExternalId(token, DtdParser.Token.NOTATION, out text, out text2);
				if (schemaNotation != null)
				{
					schemaNotation.SystemLiteral = text2;
					schemaNotation.Pubid = text;
				}
			}
			else
			{
				this.OnUnexpectedError();
			}
			if (this.GetToken(false) != DtdParser.Token.GreaterThan)
			{
				this.OnUnexpectedError();
			}
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x00065E0C File Offset: 0x00064E0C
		private void ParseComment()
		{
			this.SaveParsingBuffer();
			try
			{
				if (this.SaveInternalSubsetValue)
				{
					this.readerAdapter.ParseComment(this.internalSubsetValueSb);
					this.internalSubsetValueSb.Append("-->");
				}
				else
				{
					this.readerAdapter.ParseComment(null);
				}
			}
			catch (XmlException ex)
			{
				if (!(ex.ResString == "Xml_UnexpectedEOF") || this.currentEntityId == 0)
				{
					throw;
				}
				this.SendValidationEvent(XmlSeverityType.Error, "Sch_ParEntityRefNesting", null);
			}
			this.LoadParsingBuffer();
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x00065E9C File Offset: 0x00064E9C
		private void ParsePI()
		{
			this.SaveParsingBuffer();
			if (this.SaveInternalSubsetValue)
			{
				this.readerAdapter.ParsePI(this.internalSubsetValueSb);
				this.internalSubsetValueSb.Append("?>");
			}
			else
			{
				this.readerAdapter.ParsePI(null);
			}
			this.LoadParsingBuffer();
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x00065EEC File Offset: 0x00064EEC
		private void ParseCondSection()
		{
			int num = this.currentEntityId;
			switch (this.GetToken(false))
			{
			case DtdParser.Token.IGNORE:
				if (this.GetToken(false) == DtdParser.Token.RightBracket)
				{
					if (this.validate && num != this.currentEntityId)
					{
						this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "Sch_ParEntityRefNesting", string.Empty);
					}
					if (this.GetToken(false) == DtdParser.Token.CondSectionEnd)
					{
						if (this.validate && num != this.currentEntityId)
						{
							this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "Sch_ParEntityRefNesting", string.Empty);
							return;
						}
						return;
					}
				}
				break;
			case DtdParser.Token.INCLUDE:
				if (this.GetToken(false) == DtdParser.Token.RightBracket)
				{
					if (this.validate && num != this.currentEntityId)
					{
						this.SendValidationEvent(this.curPos, XmlSeverityType.Error, "Sch_ParEntityRefNesting", string.Empty);
					}
					if (this.validate)
					{
						if (this.condSectionEntityIds == null)
						{
							this.condSectionEntityIds = new int[2];
						}
						else if (this.condSectionEntityIds.Length == this.condSectionDepth)
						{
							int[] array = new int[this.condSectionEntityIds.Length * 2];
							Array.Copy(this.condSectionEntityIds, 0, array, 0, this.condSectionEntityIds.Length);
							this.condSectionEntityIds = array;
						}
						this.condSectionEntityIds[this.condSectionDepth] = num;
					}
					this.condSectionDepth++;
					return;
				}
				break;
			}
			this.OnUnexpectedError();
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0006603C File Offset: 0x0006503C
		private void ParseExternalId(DtdParser.Token idTokenType, DtdParser.Token declType, out string publicId, out string systemId)
		{
			LineInfo lineInfo = new LineInfo(this.LineNo, this.LinePos - 6);
			publicId = null;
			systemId = null;
			if (this.GetToken(true) != DtdParser.Token.Literal)
			{
				this.ThrowUnexpectedToken(this.curPos, "\"", "'");
			}
			if (idTokenType == DtdParser.Token.SYSTEM)
			{
				systemId = this.GetValue();
				if (systemId.IndexOf('#') >= 0)
				{
					this.Throw(this.curPos - systemId.Length - 1, "Xml_FragmentId", new string[]
					{
						systemId.Substring(systemId.IndexOf('#')),
						systemId
					});
				}
				if (declType == DtdParser.Token.DOCTYPE && !this.freeFloatingDtd)
				{
					this.literalLineInfo.linePos = this.literalLineInfo.linePos + 1;
					this.readerAdapter.OnSystemId(systemId, lineInfo, this.literalLineInfo);
					return;
				}
			}
			else
			{
				publicId = this.GetValue();
				int num;
				if ((num = this.xmlCharType.IsPublicId(publicId)) >= 0)
				{
					this.ThrowInvalidChar(this.curPos - 1 - publicId.Length + num, publicId[num]);
				}
				if (declType == DtdParser.Token.DOCTYPE && !this.freeFloatingDtd)
				{
					this.literalLineInfo.linePos = this.literalLineInfo.linePos + 1;
					this.readerAdapter.OnPublicId(publicId, lineInfo, this.literalLineInfo);
					if (this.GetToken(false) == DtdParser.Token.Literal)
					{
						if (!this.whitespaceSeen)
						{
							this.Throw("Xml_ExpectingWhiteSpace", new string(this.literalQuoteChar, 1), this.literalLineInfo.lineNo, this.literalLineInfo.linePos);
						}
						systemId = this.GetValue();
						this.literalLineInfo.linePos = this.literalLineInfo.linePos + 1;
						this.readerAdapter.OnSystemId(systemId, lineInfo, this.literalLineInfo);
						return;
					}
					this.ThrowUnexpectedToken(this.curPos, "\"", "'");
					return;
				}
				else
				{
					if (this.GetToken(false) == DtdParser.Token.Literal)
					{
						if (!this.whitespaceSeen)
						{
							this.Throw("Xml_ExpectingWhiteSpace", new string(this.literalQuoteChar, 1), this.literalLineInfo.lineNo, this.literalLineInfo.linePos);
						}
						systemId = this.GetValue();
						return;
					}
					if (declType != DtdParser.Token.NOTATION)
					{
						this.ThrowUnexpectedToken(this.curPos, "\"", "'");
					}
				}
			}
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x00066280 File Offset: 0x00065280
		private DtdParser.Token GetToken(bool needWhiteSpace)
		{
			this.whitespaceSeen = false;
			for (;;)
			{
				char c = this.chars[this.curPos];
				if (c <= '\r')
				{
					if (c != '\0')
					{
						switch (c)
						{
						case '\t':
							goto IL_014D;
						case '\n':
							this.whitespaceSeen = true;
							this.curPos++;
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						case '\r':
							this.whitespaceSeen = true;
							if (this.chars[this.curPos + 1] == '\n')
							{
								if (this.normalize)
								{
									this.SaveParsingBuffer();
									this.readerAdapter.CurrentPosition++;
								}
								this.curPos += 2;
							}
							else
							{
								if (this.curPos + 1 >= this.charsUsed && !this.readerAdapter.IsEof)
								{
									goto IL_0388;
								}
								this.chars[this.curPos] = '\n';
								this.curPos++;
							}
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						}
						break;
					}
					if (this.curPos != this.charsUsed)
					{
						this.ThrowInvalidChar(this.curPos, this.chars[this.curPos]);
						goto IL_0388;
					}
					goto IL_0388;
				}
				else if (c != ' ')
				{
					if (c != '%')
					{
						break;
					}
					if (this.charsUsed - this.curPos < 2)
					{
						goto IL_0388;
					}
					if (this.xmlCharType.IsWhiteSpace(this.chars[this.curPos + 1]))
					{
						break;
					}
					if (this.IgnoreEntityReferences)
					{
						this.curPos++;
						continue;
					}
					this.HandleEntityReference(true, false, false);
					continue;
				}
				IL_014D:
				this.whitespaceSeen = true;
				this.curPos++;
				continue;
				IL_0388:
				if ((this.readerAdapter.IsEof || this.ReadData() == 0) && !this.HandleEntityEnd(false))
				{
					if (this.scanningFunction == DtdParser.ScanningFunction.SubsetContent)
					{
						return DtdParser.Token.Eof;
					}
					this.Throw(this.curPos, "Xml_IncompleteDtdContent");
				}
			}
			if (needWhiteSpace && !this.whitespaceSeen && this.scanningFunction != DtdParser.ScanningFunction.ParamEntitySpace)
			{
				this.Throw(this.curPos, "Xml_ExpectingWhiteSpace", this.ParseUnexpectedToken(this.curPos));
			}
			this.tokenStartPos = this.curPos;
			for (;;)
			{
				switch (this.scanningFunction)
				{
				case DtdParser.ScanningFunction.SubsetContent:
					goto IL_02A9;
				case DtdParser.ScanningFunction.Name:
					goto IL_0294;
				case DtdParser.ScanningFunction.QName:
					goto IL_029B;
				case DtdParser.ScanningFunction.Nmtoken:
					goto IL_02A2;
				case DtdParser.ScanningFunction.Doctype1:
					goto IL_02B0;
				case DtdParser.ScanningFunction.Doctype2:
					goto IL_02B7;
				case DtdParser.ScanningFunction.Element1:
					goto IL_02BE;
				case DtdParser.ScanningFunction.Element2:
					goto IL_02C5;
				case DtdParser.ScanningFunction.Element3:
					goto IL_02CC;
				case DtdParser.ScanningFunction.Element4:
					goto IL_02D3;
				case DtdParser.ScanningFunction.Element5:
					goto IL_02DA;
				case DtdParser.ScanningFunction.Element6:
					goto IL_02E1;
				case DtdParser.ScanningFunction.Element7:
					goto IL_02E8;
				case DtdParser.ScanningFunction.Attlist1:
					goto IL_02EF;
				case DtdParser.ScanningFunction.Attlist2:
					goto IL_02F6;
				case DtdParser.ScanningFunction.Attlist3:
					goto IL_02FD;
				case DtdParser.ScanningFunction.Attlist4:
					goto IL_0304;
				case DtdParser.ScanningFunction.Attlist5:
					goto IL_030B;
				case DtdParser.ScanningFunction.Attlist6:
					goto IL_0312;
				case DtdParser.ScanningFunction.Attlist7:
					goto IL_0319;
				case DtdParser.ScanningFunction.Entity1:
					goto IL_033C;
				case DtdParser.ScanningFunction.Entity2:
					goto IL_0343;
				case DtdParser.ScanningFunction.Entity3:
					goto IL_034A;
				case DtdParser.ScanningFunction.Notation1:
					goto IL_0320;
				case DtdParser.ScanningFunction.CondSection1:
					goto IL_0351;
				case DtdParser.ScanningFunction.CondSection2:
					goto IL_0358;
				case DtdParser.ScanningFunction.CondSection3:
					goto IL_035F;
				case DtdParser.ScanningFunction.SystemId:
					goto IL_0327;
				case DtdParser.ScanningFunction.PublicId1:
					goto IL_032E;
				case DtdParser.ScanningFunction.PublicId2:
					goto IL_0335;
				case DtdParser.ScanningFunction.ClosingTag:
					goto IL_0366;
				case DtdParser.ScanningFunction.ParamEntitySpace:
					this.whitespaceSeen = true;
					this.scanningFunction = this.savedScanningFunction;
					continue;
				}
				break;
			}
			return DtdParser.Token.None;
			IL_0294:
			return this.ScanNameExpected();
			IL_029B:
			return this.ScanQNameExpected();
			IL_02A2:
			return this.ScanNmtokenExpected();
			IL_02A9:
			return this.ScanSubsetContent();
			IL_02B0:
			return this.ScanDoctype1();
			IL_02B7:
			return this.ScanDoctype2();
			IL_02BE:
			return this.ScanElement1();
			IL_02C5:
			return this.ScanElement2();
			IL_02CC:
			return this.ScanElement3();
			IL_02D3:
			return this.ScanElement4();
			IL_02DA:
			return this.ScanElement5();
			IL_02E1:
			return this.ScanElement6();
			IL_02E8:
			return this.ScanElement7();
			IL_02EF:
			return this.ScanAttlist1();
			IL_02F6:
			return this.ScanAttlist2();
			IL_02FD:
			return this.ScanAttlist3();
			IL_0304:
			return this.ScanAttlist4();
			IL_030B:
			return this.ScanAttlist5();
			IL_0312:
			return this.ScanAttlist6();
			IL_0319:
			return this.ScanAttlist7();
			IL_0320:
			return this.ScanNotation1();
			IL_0327:
			return this.ScanSystemId();
			IL_032E:
			return this.ScanPublicId1();
			IL_0335:
			return this.ScanPublicId2();
			IL_033C:
			return this.ScanEntity1();
			IL_0343:
			return this.ScanEntity2();
			IL_034A:
			return this.ScanEntity3();
			IL_0351:
			return this.ScanCondSection1();
			IL_0358:
			return this.ScanCondSection2();
			IL_035F:
			return this.ScanCondSection3();
			IL_0366:
			return this.ScanClosingTag();
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x0006665C File Offset: 0x0006565C
		private DtdParser.Token ScanSubsetContent()
		{
			for (;;)
			{
				char c = this.chars[this.curPos];
				if (c != '<')
				{
					if (c == ']')
					{
						if (this.charsUsed - this.curPos < 2 && !this.readerAdapter.IsEof)
						{
							goto IL_0513;
						}
						if (this.chars[this.curPos + 1] != ']')
						{
							goto Block_40;
						}
						if (this.charsUsed - this.curPos < 3 && !this.readerAdapter.IsEof)
						{
							goto IL_0513;
						}
						if (this.chars[this.curPos + 1] == ']' && this.chars[this.curPos + 2] == '>')
						{
							goto Block_43;
						}
					}
					if (this.charsUsed - this.curPos != 0)
					{
						this.Throw(this.curPos, "Xml_ExpectDtdMarkup");
					}
				}
				else
				{
					char c2 = this.chars[this.curPos + 1];
					if (c2 != '!')
					{
						if (c2 == '?')
						{
							goto IL_041B;
						}
						if (this.charsUsed - this.curPos >= 2)
						{
							goto Block_38;
						}
					}
					else
					{
						char c3 = this.chars[this.curPos + 2];
						if (c3 <= 'A')
						{
							if (c3 != '-')
							{
								if (c3 == 'A')
								{
									if (this.charsUsed - this.curPos >= 9)
									{
										goto Block_22;
									}
									goto IL_0513;
								}
							}
							else
							{
								if (this.chars[this.curPos + 3] == '-')
								{
									goto Block_35;
								}
								if (this.charsUsed - this.curPos >= 4)
								{
									this.Throw(this.curPos, "Xml_ExpectDtdMarkup");
									goto IL_0513;
								}
								goto IL_0513;
							}
						}
						else if (c3 != 'E')
						{
							if (c3 != 'N')
							{
								if (c3 == '[')
								{
									goto IL_038A;
								}
							}
							else
							{
								if (this.charsUsed - this.curPos >= 10)
								{
									goto Block_28;
								}
								goto IL_0513;
							}
						}
						else if (this.chars[this.curPos + 3] == 'L')
						{
							if (this.charsUsed - this.curPos >= 9)
							{
								break;
							}
							goto IL_0513;
						}
						else if (this.chars[this.curPos + 3] == 'N')
						{
							if (this.charsUsed - this.curPos >= 8)
							{
								goto Block_17;
							}
							goto IL_0513;
						}
						else
						{
							if (this.charsUsed - this.curPos >= 4)
							{
								goto Block_21;
							}
							goto IL_0513;
						}
						if (this.charsUsed - this.curPos >= 3)
						{
							this.Throw(this.curPos + 2, "Xml_ExpectDtdMarkup");
						}
					}
				}
				IL_0513:
				if (this.ReadData() == 0)
				{
					this.Throw(this.charsUsed, "Xml_IncompleteDtdContent");
				}
			}
			if (this.chars[this.curPos + 4] != 'E' || this.chars[this.curPos + 5] != 'M' || this.chars[this.curPos + 6] != 'E' || this.chars[this.curPos + 7] != 'N' || this.chars[this.curPos + 8] != 'T')
			{
				this.Throw(this.curPos, "Xml_ExpectDtdMarkup");
			}
			this.curPos += 9;
			this.scanningFunction = DtdParser.ScanningFunction.QName;
			this.nextScaningFunction = DtdParser.ScanningFunction.Element1;
			return DtdParser.Token.ElementDecl;
			Block_17:
			if (this.chars[this.curPos + 4] != 'T' || this.chars[this.curPos + 5] != 'I' || this.chars[this.curPos + 6] != 'T' || this.chars[this.curPos + 7] != 'Y')
			{
				this.Throw(this.curPos, "Xml_ExpectDtdMarkup");
			}
			this.curPos += 8;
			this.scanningFunction = DtdParser.ScanningFunction.Entity1;
			return DtdParser.Token.EntityDecl;
			Block_21:
			this.Throw(this.curPos, "Xml_ExpectDtdMarkup");
			return DtdParser.Token.None;
			Block_22:
			if (this.chars[this.curPos + 3] != 'T' || this.chars[this.curPos + 4] != 'T' || this.chars[this.curPos + 5] != 'L' || this.chars[this.curPos + 6] != 'I' || this.chars[this.curPos + 7] != 'S' || this.chars[this.curPos + 8] != 'T')
			{
				this.Throw(this.curPos, "Xml_ExpectDtdMarkup");
			}
			this.curPos += 9;
			this.scanningFunction = DtdParser.ScanningFunction.QName;
			this.nextScaningFunction = DtdParser.ScanningFunction.Attlist1;
			return DtdParser.Token.AttlistDecl;
			Block_28:
			if (this.chars[this.curPos + 3] != 'O' || this.chars[this.curPos + 4] != 'T' || this.chars[this.curPos + 5] != 'A' || this.chars[this.curPos + 6] != 'T' || this.chars[this.curPos + 7] != 'I' || this.chars[this.curPos + 8] != 'O' || this.chars[this.curPos + 9] != 'N')
			{
				this.Throw(this.curPos, "Xml_ExpectDtdMarkup");
			}
			this.curPos += 10;
			this.scanningFunction = DtdParser.ScanningFunction.Name;
			this.nextScaningFunction = DtdParser.ScanningFunction.Notation1;
			return DtdParser.Token.NotationDecl;
			IL_038A:
			this.curPos += 3;
			this.scanningFunction = DtdParser.ScanningFunction.CondSection1;
			return DtdParser.Token.CondSectionStart;
			Block_35:
			this.curPos += 4;
			return DtdParser.Token.Comment;
			IL_041B:
			this.curPos += 2;
			return DtdParser.Token.PI;
			Block_38:
			this.Throw(this.curPos, "Xml_ExpectDtdMarkup");
			return DtdParser.Token.None;
			Block_40:
			this.curPos++;
			this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
			return DtdParser.Token.RightBracket;
			Block_43:
			this.curPos += 3;
			return DtdParser.Token.CondSectionEnd;
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x00066B9C File Offset: 0x00065B9C
		private DtdParser.Token ScanNameExpected()
		{
			this.ScanName();
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.Name;
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x00066BB2 File Offset: 0x00065BB2
		private DtdParser.Token ScanQNameExpected()
		{
			this.ScanQName();
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.QName;
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x00066BC8 File Offset: 0x00065BC8
		private DtdParser.Token ScanNmtokenExpected()
		{
			this.ScanNmtoken();
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.Nmtoken;
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x00066BE0 File Offset: 0x00065BE0
		private DtdParser.Token ScanDoctype1()
		{
			char c = this.chars[this.curPos];
			if (c <= 'P')
			{
				if (c == '>')
				{
					this.curPos++;
					this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
					return DtdParser.Token.GreaterThan;
				}
				if (c == 'P')
				{
					if (!this.EatPublicKeyword())
					{
						this.Throw(this.curPos, "Xml_ExpectExternalOrClose");
					}
					this.nextScaningFunction = DtdParser.ScanningFunction.Doctype2;
					this.scanningFunction = DtdParser.ScanningFunction.PublicId1;
					return DtdParser.Token.PUBLIC;
				}
			}
			else
			{
				if (c == 'S')
				{
					if (!this.EatSystemKeyword())
					{
						this.Throw(this.curPos, "Xml_ExpectExternalOrClose");
					}
					this.nextScaningFunction = DtdParser.ScanningFunction.Doctype2;
					this.scanningFunction = DtdParser.ScanningFunction.SystemId;
					return DtdParser.Token.SYSTEM;
				}
				if (c == '[')
				{
					this.curPos++;
					this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
					return DtdParser.Token.LeftBracket;
				}
			}
			this.Throw(this.curPos, "Xml_ExpectExternalOrClose");
			return DtdParser.Token.None;
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x00066CBC File Offset: 0x00065CBC
		private DtdParser.Token ScanDoctype2()
		{
			char c = this.chars[this.curPos];
			if (c == '>')
			{
				this.curPos++;
				this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
				return DtdParser.Token.GreaterThan;
			}
			if (c == '[')
			{
				this.curPos++;
				this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
				return DtdParser.Token.LeftBracket;
			}
			this.Throw(this.curPos, "Xml_ExpectSubOrClose");
			return DtdParser.Token.None;
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x00066D24 File Offset: 0x00065D24
		private DtdParser.Token ScanClosingTag()
		{
			if (this.chars[this.curPos] != '>')
			{
				this.ThrowUnexpectedToken(this.curPos, ">");
			}
			this.curPos++;
			this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
			return DtdParser.Token.GreaterThan;
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x00066D60 File Offset: 0x00065D60
		private DtdParser.Token ScanElement1()
		{
			for (;;)
			{
				char c = this.chars[this.curPos];
				if (c != '(')
				{
					if (c != 'A')
					{
						if (c != 'E')
						{
							goto IL_010A;
						}
						if (this.charsUsed - this.curPos >= 5)
						{
							if (this.chars[this.curPos + 1] == 'M' && this.chars[this.curPos + 2] == 'P' && this.chars[this.curPos + 3] == 'T' && this.chars[this.curPos + 4] == 'Y')
							{
								goto Block_7;
							}
							goto IL_010A;
						}
					}
					else if (this.charsUsed - this.curPos >= 3)
					{
						if (this.chars[this.curPos + 1] == 'N' && this.chars[this.curPos + 2] == 'Y')
						{
							goto Block_10;
						}
						goto IL_010A;
					}
					IL_011B:
					if (this.ReadData() == 0)
					{
						this.Throw(this.curPos, "Xml_IncompleteDtdContent");
						continue;
					}
					continue;
					IL_010A:
					this.Throw(this.curPos, "Xml_InvalidContentModel");
					goto IL_011B;
				}
				break;
			}
			this.scanningFunction = DtdParser.ScanningFunction.Element2;
			this.curPos++;
			return DtdParser.Token.LeftParen;
			Block_7:
			this.curPos += 5;
			this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
			return DtdParser.Token.EMPTY;
			Block_10:
			this.curPos += 3;
			this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
			return DtdParser.Token.ANY;
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x00066EA8 File Offset: 0x00065EA8
		private DtdParser.Token ScanElement2()
		{
			if (this.chars[this.curPos] == '#')
			{
				while (this.charsUsed - this.curPos < 7)
				{
					if (this.ReadData() == 0)
					{
						this.Throw(this.curPos, "Xml_IncompleteDtdContent");
					}
				}
				if (this.chars[this.curPos + 1] == 'P' && this.chars[this.curPos + 2] == 'C' && this.chars[this.curPos + 3] == 'D' && this.chars[this.curPos + 4] == 'A' && this.chars[this.curPos + 5] == 'T' && this.chars[this.curPos + 6] == 'A')
				{
					this.curPos += 7;
					this.scanningFunction = DtdParser.ScanningFunction.Element6;
					return DtdParser.Token.PCDATA;
				}
				this.Throw(this.curPos + 1, "Xml_ExpectPcData");
			}
			this.scanningFunction = DtdParser.ScanningFunction.Element3;
			return DtdParser.Token.None;
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x00066F9C File Offset: 0x00065F9C
		private DtdParser.Token ScanElement3()
		{
			char c = this.chars[this.curPos];
			if (c == '(')
			{
				this.curPos++;
				return DtdParser.Token.LeftParen;
			}
			if (c != '>')
			{
				this.ScanQName();
				this.scanningFunction = DtdParser.ScanningFunction.Element4;
				return DtdParser.Token.QName;
			}
			this.curPos++;
			this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
			return DtdParser.Token.GreaterThan;
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x00066FFC File Offset: 0x00065FFC
		private DtdParser.Token ScanElement4()
		{
			this.scanningFunction = DtdParser.ScanningFunction.Element5;
			char c = this.chars[this.curPos];
			DtdParser.Token token;
			switch (c)
			{
			case '*':
				token = DtdParser.Token.Star;
				break;
			case '+':
				token = DtdParser.Token.Plus;
				break;
			default:
				if (c != '?')
				{
					return DtdParser.Token.None;
				}
				token = DtdParser.Token.QMark;
				break;
			}
			if (this.whitespaceSeen)
			{
				this.Throw(this.curPos, "Xml_ExpectNoWhitespace");
			}
			this.curPos++;
			return token;
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x00067074 File Offset: 0x00066074
		private DtdParser.Token ScanElement5()
		{
			char c = this.chars[this.curPos];
			if (c <= ',')
			{
				if (c == ')')
				{
					this.curPos++;
					this.scanningFunction = DtdParser.ScanningFunction.Element4;
					return DtdParser.Token.RightParen;
				}
				if (c == ',')
				{
					this.curPos++;
					this.scanningFunction = DtdParser.ScanningFunction.Element3;
					return DtdParser.Token.Comma;
				}
			}
			else
			{
				if (c == '>')
				{
					this.curPos++;
					this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
					return DtdParser.Token.GreaterThan;
				}
				if (c == '|')
				{
					this.curPos++;
					this.scanningFunction = DtdParser.ScanningFunction.Element3;
					return DtdParser.Token.Or;
				}
			}
			this.Throw(this.curPos, "Xml_ExpectOp");
			return DtdParser.Token.None;
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00067120 File Offset: 0x00066120
		private DtdParser.Token ScanElement6()
		{
			char c = this.chars[this.curPos];
			if (c == ')')
			{
				this.curPos++;
				this.scanningFunction = DtdParser.ScanningFunction.Element7;
				return DtdParser.Token.RightParen;
			}
			if (c != '|')
			{
				this.ThrowUnexpectedToken(this.curPos, ")", "|");
				return DtdParser.Token.None;
			}
			this.curPos++;
			this.nextScaningFunction = DtdParser.ScanningFunction.Element6;
			this.scanningFunction = DtdParser.ScanningFunction.QName;
			return DtdParser.Token.Or;
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x00067198 File Offset: 0x00066198
		private DtdParser.Token ScanElement7()
		{
			this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
			if (this.chars[this.curPos] == '*' && !this.whitespaceSeen)
			{
				this.curPos++;
				return DtdParser.Token.Star;
			}
			return DtdParser.Token.None;
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x000671D0 File Offset: 0x000661D0
		private DtdParser.Token ScanAttlist1()
		{
			char c = this.chars[this.curPos];
			if (c == '>')
			{
				this.curPos++;
				this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
				return DtdParser.Token.GreaterThan;
			}
			if (!this.whitespaceSeen)
			{
				this.Throw(this.curPos, "Xml_ExpectingWhiteSpace", this.ParseUnexpectedToken(this.curPos));
			}
			this.ScanQName();
			this.scanningFunction = DtdParser.ScanningFunction.Attlist2;
			return DtdParser.Token.QName;
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x00067240 File Offset: 0x00066240
		private DtdParser.Token ScanAttlist2()
		{
			for (;;)
			{
				char c = this.chars[this.curPos];
				if (c <= 'E')
				{
					if (c == '(')
					{
						break;
					}
					switch (c)
					{
					case 'C':
						if (this.charsUsed - this.curPos >= 5)
						{
							goto Block_5;
						}
						break;
					case 'D':
						goto IL_0460;
					case 'E':
						if (this.charsUsed - this.curPos >= 9)
						{
							this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
							if (this.chars[this.curPos + 1] != 'N' || this.chars[this.curPos + 2] != 'T' || this.chars[this.curPos + 3] != 'I' || this.chars[this.curPos + 4] != 'T')
							{
								this.Throw(this.curPos, "Xml_InvalidAttributeType");
							}
							char c2 = this.chars[this.curPos + 5];
							if (c2 == 'I')
							{
								goto IL_0184;
							}
							if (c2 == 'Y')
							{
								goto IL_01CB;
							}
							this.Throw(this.curPos, "Xml_InvalidAttributeType");
						}
						break;
					default:
						goto IL_0460;
					}
				}
				else if (c != 'I')
				{
					if (c != 'N')
					{
						goto IL_0460;
					}
					if (this.charsUsed - this.curPos >= 8 || this.readerAdapter.IsEof)
					{
						switch (this.chars[this.curPos + 1])
						{
						case 'M':
							goto IL_03A2;
						case 'O':
							goto IL_0307;
						}
						this.Throw(this.curPos, "Xml_InvalidAttributeType");
					}
				}
				else if (this.charsUsed - this.curPos >= 6)
				{
					goto Block_16;
				}
				IL_0471:
				if (this.ReadData() == 0)
				{
					this.Throw(this.curPos, "Xml_IncompleteDtdContent");
					continue;
				}
				continue;
				IL_0460:
				this.Throw(this.curPos, "Xml_InvalidAttributeType");
				goto IL_0471;
			}
			this.curPos++;
			this.scanningFunction = DtdParser.ScanningFunction.Nmtoken;
			this.nextScaningFunction = DtdParser.ScanningFunction.Attlist5;
			return DtdParser.Token.LeftParen;
			Block_5:
			if (this.chars[this.curPos + 1] != 'D' || this.chars[this.curPos + 2] != 'A' || this.chars[this.curPos + 3] != 'T' || this.chars[this.curPos + 4] != 'A')
			{
				this.Throw(this.curPos, "Xml_InvalidAttributeType1");
			}
			this.curPos += 5;
			this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
			return DtdParser.Token.CDATA;
			IL_0184:
			if (this.chars[this.curPos + 6] != 'E' || this.chars[this.curPos + 7] != 'S')
			{
				this.Throw(this.curPos, "Xml_InvalidAttributeType");
			}
			this.curPos += 8;
			return DtdParser.Token.ENTITIES;
			IL_01CB:
			this.curPos += 6;
			return DtdParser.Token.ENTITY;
			Block_16:
			this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
			if (this.chars[this.curPos + 1] != 'D')
			{
				this.Throw(this.curPos, "Xml_InvalidAttributeType");
			}
			if (this.chars[this.curPos + 2] != 'R')
			{
				this.curPos += 2;
				return DtdParser.Token.ID;
			}
			if (this.chars[this.curPos + 3] != 'E' || this.chars[this.curPos + 4] != 'F')
			{
				this.Throw(this.curPos, "Xml_InvalidAttributeType");
			}
			if (this.chars[this.curPos + 5] != 'S')
			{
				this.curPos += 5;
				return DtdParser.Token.IDREF;
			}
			this.curPos += 6;
			return DtdParser.Token.IDREFS;
			IL_0307:
			if (this.chars[this.curPos + 2] != 'T' || this.chars[this.curPos + 3] != 'A' || this.chars[this.curPos + 4] != 'T' || this.chars[this.curPos + 5] != 'I' || this.chars[this.curPos + 6] != 'O' || this.chars[this.curPos + 7] != 'N')
			{
				this.Throw(this.curPos, "Xml_InvalidAttributeType");
			}
			this.curPos += 8;
			this.scanningFunction = DtdParser.ScanningFunction.Attlist3;
			return DtdParser.Token.NOTATION;
			IL_03A2:
			if (this.chars[this.curPos + 2] != 'T' || this.chars[this.curPos + 3] != 'O' || this.chars[this.curPos + 4] != 'K' || this.chars[this.curPos + 5] != 'E' || this.chars[this.curPos + 6] != 'N')
			{
				this.Throw(this.curPos, "Xml_InvalidAttributeType");
			}
			this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
			if (this.chars[this.curPos + 7] == 'S')
			{
				this.curPos += 8;
				return DtdParser.Token.NMTOKENS;
			}
			this.curPos += 7;
			return DtdParser.Token.NMTOKEN;
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x000676E0 File Offset: 0x000666E0
		private DtdParser.Token ScanAttlist3()
		{
			if (this.chars[this.curPos] == '(')
			{
				this.curPos++;
				this.scanningFunction = DtdParser.ScanningFunction.Name;
				this.nextScaningFunction = DtdParser.ScanningFunction.Attlist4;
				return DtdParser.Token.LeftParen;
			}
			this.ThrowUnexpectedToken(this.curPos, "(");
			return DtdParser.Token.None;
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x00067734 File Offset: 0x00066734
		private DtdParser.Token ScanAttlist4()
		{
			char c = this.chars[this.curPos];
			if (c == ')')
			{
				this.curPos++;
				this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
				return DtdParser.Token.RightParen;
			}
			if (c != '|')
			{
				this.ThrowUnexpectedToken(this.curPos, ")", "|");
				return DtdParser.Token.None;
			}
			this.curPos++;
			this.scanningFunction = DtdParser.ScanningFunction.Name;
			this.nextScaningFunction = DtdParser.ScanningFunction.Attlist4;
			return DtdParser.Token.Or;
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x000677AC File Offset: 0x000667AC
		private DtdParser.Token ScanAttlist5()
		{
			char c = this.chars[this.curPos];
			if (c == ')')
			{
				this.curPos++;
				this.scanningFunction = DtdParser.ScanningFunction.Attlist6;
				return DtdParser.Token.RightParen;
			}
			if (c != '|')
			{
				this.ThrowUnexpectedToken(this.curPos, ")", "|");
				return DtdParser.Token.None;
			}
			this.curPos++;
			this.scanningFunction = DtdParser.ScanningFunction.Nmtoken;
			this.nextScaningFunction = DtdParser.ScanningFunction.Attlist5;
			return DtdParser.Token.Or;
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x00067824 File Offset: 0x00066824
		private DtdParser.Token ScanAttlist6()
		{
			for (;;)
			{
				char c = this.chars[this.curPos];
				switch (c)
				{
				case '"':
					goto IL_0027;
				case '#':
					if (this.charsUsed - this.curPos >= 6)
					{
						char c2 = this.chars[this.curPos + 1];
						if (c2 == 'F')
						{
							goto IL_01E8;
						}
						if (c2 != 'I')
						{
							if (c2 == 'R')
							{
								if (this.charsUsed - this.curPos >= 9)
								{
									goto Block_5;
								}
							}
							else
							{
								this.Throw(this.curPos, "Xml_ExpectAttType");
							}
						}
						else if (this.charsUsed - this.curPos >= 8)
						{
							goto Block_12;
						}
					}
					break;
				default:
					if (c == '\'')
					{
						goto IL_0027;
					}
					this.Throw(this.curPos, "Xml_ExpectAttType");
					break;
				}
				if (this.ReadData() == 0)
				{
					this.Throw(this.curPos, "Xml_IncompleteDtdContent");
				}
			}
			IL_0027:
			this.ScanLiteral(DtdParser.LiteralType.AttributeValue);
			this.scanningFunction = DtdParser.ScanningFunction.Attlist1;
			return DtdParser.Token.Literal;
			Block_5:
			if (this.chars[this.curPos + 2] != 'E' || this.chars[this.curPos + 3] != 'Q' || this.chars[this.curPos + 4] != 'U' || this.chars[this.curPos + 5] != 'I' || this.chars[this.curPos + 6] != 'R' || this.chars[this.curPos + 7] != 'E' || this.chars[this.curPos + 8] != 'D')
			{
				this.Throw(this.curPos, "Xml_ExpectAttType");
			}
			this.curPos += 9;
			this.scanningFunction = DtdParser.ScanningFunction.Attlist1;
			return DtdParser.Token.REQUIRED;
			Block_12:
			if (this.chars[this.curPos + 2] != 'M' || this.chars[this.curPos + 3] != 'P' || this.chars[this.curPos + 4] != 'L' || this.chars[this.curPos + 5] != 'I' || this.chars[this.curPos + 6] != 'E' || this.chars[this.curPos + 7] != 'D')
			{
				this.Throw(this.curPos, "Xml_ExpectAttType");
			}
			this.curPos += 8;
			this.scanningFunction = DtdParser.ScanningFunction.Attlist1;
			return DtdParser.Token.IMPLIED;
			IL_01E8:
			if (this.chars[this.curPos + 2] != 'I' || this.chars[this.curPos + 3] != 'X' || this.chars[this.curPos + 4] != 'E' || this.chars[this.curPos + 5] != 'D')
			{
				this.Throw(this.curPos, "Xml_ExpectAttType");
			}
			this.curPos += 6;
			this.scanningFunction = DtdParser.ScanningFunction.Attlist7;
			return DtdParser.Token.FIXED;
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x00067AD4 File Offset: 0x00066AD4
		private DtdParser.Token ScanAttlist7()
		{
			char c = this.chars[this.curPos];
			if (c == '"' || c == '\'')
			{
				this.ScanLiteral(DtdParser.LiteralType.AttributeValue);
				this.scanningFunction = DtdParser.ScanningFunction.Attlist1;
				return DtdParser.Token.Literal;
			}
			this.ThrowUnexpectedToken(this.curPos, "\"", "'");
			return DtdParser.Token.None;
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x00067B24 File Offset: 0x00066B24
		private unsafe DtdParser.Token ScanLiteral(DtdParser.LiteralType literalType)
		{
			char c = this.chars[this.curPos];
			char c2 = ((literalType == DtdParser.LiteralType.AttributeValue) ? ' ' : '\n');
			int num = this.currentEntityId;
			this.literalLineInfo.Set(this.LineNo, this.LinePos);
			this.curPos++;
			this.tokenStartPos = this.curPos;
			this.stringBuilder.Length = 0;
			char c4;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[this.chars[this.curPos]] & 128) == 0 || this.chars[this.curPos] == '%')
				{
					if (this.chars[this.curPos] == c && this.currentEntityId == num)
					{
						break;
					}
					int num2 = this.curPos - this.tokenStartPos;
					if (num2 > 0)
					{
						this.stringBuilder.Append(this.chars, this.tokenStartPos, num2);
						this.tokenStartPos = this.curPos;
					}
					char c3 = this.chars[this.curPos];
					switch (c3)
					{
					case '\t':
						if (literalType == DtdParser.LiteralType.AttributeValue && this.normalize)
						{
							this.stringBuilder.Append(' ');
							this.tokenStartPos++;
						}
						this.curPos++;
						continue;
					case '\n':
						this.curPos++;
						if (this.normalize)
						{
							this.stringBuilder.Append(c2);
							this.tokenStartPos = this.curPos;
						}
						this.readerAdapter.OnNewLine(this.curPos);
						continue;
					case '\v':
					case '\f':
						break;
					case '\r':
						if (this.chars[this.curPos + 1] == '\n')
						{
							if (this.normalize)
							{
								if (literalType == DtdParser.LiteralType.AttributeValue)
								{
									this.stringBuilder.Append(this.readerAdapter.IsEntityEolNormalized ? "  " : " ");
								}
								else
								{
									this.stringBuilder.Append(this.readerAdapter.IsEntityEolNormalized ? "\r\n" : "\n");
								}
								this.tokenStartPos = this.curPos + 2;
								this.SaveParsingBuffer();
								this.readerAdapter.CurrentPosition++;
							}
							this.curPos += 2;
						}
						else
						{
							if (this.curPos + 1 == this.charsUsed)
							{
								goto IL_05D9;
							}
							this.curPos++;
							if (this.normalize)
							{
								this.stringBuilder.Append(c2);
								this.tokenStartPos = this.curPos;
							}
						}
						this.readerAdapter.OnNewLine(this.curPos);
						continue;
					default:
						switch (c3)
						{
						case '"':
						case '\'':
							break;
						case '#':
						case '$':
							goto IL_053D;
						case '%':
							if (literalType != DtdParser.LiteralType.EntityReplText)
							{
								this.curPos++;
								continue;
							}
							this.HandleEntityReference(true, true, literalType == DtdParser.LiteralType.AttributeValue);
							this.tokenStartPos = this.curPos;
							continue;
						case '&':
							if (literalType == DtdParser.LiteralType.SystemOrPublicID)
							{
								this.curPos++;
								continue;
							}
							if (this.curPos + 1 == this.charsUsed)
							{
								goto IL_05D9;
							}
							if (this.chars[this.curPos + 1] == '#')
							{
								this.SaveParsingBuffer();
								int num3 = this.readerAdapter.ParseNumericCharRef(this.SaveInternalSubsetValue ? this.internalSubsetValueSb : null);
								this.LoadParsingBuffer();
								this.stringBuilder.Append(this.chars, this.curPos, num3 - this.curPos);
								this.readerAdapter.CurrentPosition = num3;
								this.tokenStartPos = num3;
								this.curPos = num3;
								continue;
							}
							this.SaveParsingBuffer();
							if (literalType == DtdParser.LiteralType.AttributeValue)
							{
								int num4 = this.readerAdapter.ParseNamedCharRef(true, this.SaveInternalSubsetValue ? this.internalSubsetValueSb : null);
								this.LoadParsingBuffer();
								if (num4 >= 0)
								{
									this.stringBuilder.Append(this.chars, this.curPos, num4 - this.curPos);
									this.readerAdapter.CurrentPosition = num4;
									this.tokenStartPos = num4;
									this.curPos = num4;
									continue;
								}
								this.HandleEntityReference(false, true, true);
								this.tokenStartPos = this.curPos;
								continue;
							}
							else
							{
								int num5 = this.readerAdapter.ParseNamedCharRef(false, null);
								this.LoadParsingBuffer();
								if (num5 >= 0)
								{
									this.tokenStartPos = this.curPos;
									this.curPos = num5;
									continue;
								}
								this.stringBuilder.Append('&');
								this.curPos++;
								this.tokenStartPos = this.curPos;
								XmlQualifiedName xmlQualifiedName = this.ScanEntityName();
								this.VerifyEntityReference(xmlQualifiedName, false, false, false);
								continue;
							}
							break;
						default:
							switch (c3)
							{
							case '<':
								if (literalType == DtdParser.LiteralType.AttributeValue)
								{
									this.Throw(this.curPos, "Xml_BadAttributeChar", XmlException.BuildCharExceptionStr('<'));
								}
								this.curPos++;
								continue;
							case '=':
								goto IL_053D;
							case '>':
								break;
							default:
								goto IL_053D;
							}
							break;
						}
						this.curPos++;
						continue;
					}
					IL_053D:
					if (this.curPos != this.charsUsed)
					{
						c4 = this.chars[this.curPos];
						if (c4 < '\ud800' || c4 > '\udbff')
						{
							goto IL_05C8;
						}
						if (this.curPos + 1 != this.charsUsed)
						{
							this.curPos++;
							if (this.chars[this.curPos] >= '\udc00' && this.chars[this.curPos] <= '\udfff')
							{
								this.curPos++;
								continue;
							}
							goto IL_05C8;
						}
					}
					IL_05D9:
					if ((this.readerAdapter.IsEof || this.ReadData() == 0) && (literalType == DtdParser.LiteralType.SystemOrPublicID || !this.HandleEntityEnd(true)))
					{
						this.Throw(this.curPos, "Xml_UnclosedQuote");
					}
					this.tokenStartPos = this.curPos;
				}
				else
				{
					this.curPos++;
				}
			}
			if (this.stringBuilder.Length > 0)
			{
				this.stringBuilder.Append(this.chars, this.tokenStartPos, this.curPos - this.tokenStartPos);
			}
			this.curPos++;
			this.literalQuoteChar = c;
			return DtdParser.Token.Literal;
			IL_05C8:
			this.ThrowInvalidChar(this.curPos, c4);
			return DtdParser.Token.None;
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x00068150 File Offset: 0x00067150
		private XmlQualifiedName ScanEntityName()
		{
			try
			{
				this.ScanName();
			}
			catch (XmlException ex)
			{
				this.Throw("Xml_ErrorParsingEntityName", string.Empty, ex.LineNumber, ex.LinePosition);
			}
			if (this.chars[this.curPos] != ';')
			{
				this.ThrowUnexpectedToken(this.curPos, ";");
			}
			XmlQualifiedName nameQualified = this.GetNameQualified(false);
			this.curPos++;
			return nameQualified;
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x000681D0 File Offset: 0x000671D0
		private DtdParser.Token ScanNotation1()
		{
			char c = this.chars[this.curPos];
			if (c == 'P')
			{
				if (!this.EatPublicKeyword())
				{
					this.Throw(this.curPos, "Xml_ExpectExternalOrClose");
				}
				this.nextScaningFunction = DtdParser.ScanningFunction.ClosingTag;
				this.scanningFunction = DtdParser.ScanningFunction.PublicId1;
				return DtdParser.Token.PUBLIC;
			}
			if (c != 'S')
			{
				this.Throw(this.curPos, "Xml_ExpectExternalOrPublicId");
				return DtdParser.Token.None;
			}
			if (!this.EatSystemKeyword())
			{
				this.Throw(this.curPos, "Xml_ExpectExternalOrClose");
			}
			this.nextScaningFunction = DtdParser.ScanningFunction.ClosingTag;
			this.scanningFunction = DtdParser.ScanningFunction.SystemId;
			return DtdParser.Token.SYSTEM;
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x00068264 File Offset: 0x00067264
		private DtdParser.Token ScanSystemId()
		{
			if (this.chars[this.curPos] != '"' && this.chars[this.curPos] != '\'')
			{
				this.ThrowUnexpectedToken(this.curPos, "\"", "'");
			}
			this.ScanLiteral(DtdParser.LiteralType.SystemOrPublicID);
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.Literal;
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x000682C0 File Offset: 0x000672C0
		private DtdParser.Token ScanEntity1()
		{
			if (this.chars[this.curPos] == '%')
			{
				this.curPos++;
				this.nextScaningFunction = DtdParser.ScanningFunction.Entity2;
				this.scanningFunction = DtdParser.ScanningFunction.Name;
				return DtdParser.Token.Percent;
			}
			this.ScanName();
			this.scanningFunction = DtdParser.ScanningFunction.Entity2;
			return DtdParser.Token.Name;
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x00068310 File Offset: 0x00067310
		private DtdParser.Token ScanEntity2()
		{
			char c = this.chars[this.curPos];
			if (c <= '\'')
			{
				if (c == '"' || c == '\'')
				{
					this.ScanLiteral(DtdParser.LiteralType.EntityReplText);
					this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
					return DtdParser.Token.Literal;
				}
			}
			else
			{
				if (c == 'P')
				{
					if (!this.EatPublicKeyword())
					{
						this.Throw(this.curPos, "Xml_ExpectExternalOrClose");
					}
					this.nextScaningFunction = DtdParser.ScanningFunction.Entity3;
					this.scanningFunction = DtdParser.ScanningFunction.PublicId1;
					return DtdParser.Token.PUBLIC;
				}
				if (c == 'S')
				{
					if (!this.EatSystemKeyword())
					{
						this.Throw(this.curPos, "Xml_ExpectExternalOrClose");
					}
					this.nextScaningFunction = DtdParser.ScanningFunction.Entity3;
					this.scanningFunction = DtdParser.ScanningFunction.SystemId;
					return DtdParser.Token.SYSTEM;
				}
			}
			this.Throw(this.curPos, "Xml_ExpectExternalIdOrEntityValue");
			return DtdParser.Token.None;
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x000683C8 File Offset: 0x000673C8
		private DtdParser.Token ScanEntity3()
		{
			if (this.chars[this.curPos] == 'N')
			{
				while (this.charsUsed - this.curPos < 5)
				{
					if (this.ReadData() == 0)
					{
						goto IL_009A;
					}
				}
				if (this.chars[this.curPos + 1] == 'D' && this.chars[this.curPos + 2] == 'A' && this.chars[this.curPos + 3] == 'T' && this.chars[this.curPos + 4] == 'A')
				{
					this.curPos += 5;
					this.scanningFunction = DtdParser.ScanningFunction.Name;
					this.nextScaningFunction = DtdParser.ScanningFunction.ClosingTag;
					return DtdParser.Token.NData;
				}
			}
			IL_009A:
			this.scanningFunction = DtdParser.ScanningFunction.ClosingTag;
			return DtdParser.Token.None;
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x0006847C File Offset: 0x0006747C
		private DtdParser.Token ScanPublicId1()
		{
			if (this.chars[this.curPos] != '"' && this.chars[this.curPos] != '\'')
			{
				this.ThrowUnexpectedToken(this.curPos, "\"", "'");
			}
			this.ScanLiteral(DtdParser.LiteralType.SystemOrPublicID);
			this.scanningFunction = DtdParser.ScanningFunction.PublicId2;
			return DtdParser.Token.Literal;
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x000684D4 File Offset: 0x000674D4
		private DtdParser.Token ScanPublicId2()
		{
			if (this.chars[this.curPos] != '"' && this.chars[this.curPos] != '\'')
			{
				this.scanningFunction = this.nextScaningFunction;
				return DtdParser.Token.None;
			}
			this.ScanLiteral(DtdParser.LiteralType.SystemOrPublicID);
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.Literal;
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x00068528 File Offset: 0x00067528
		private DtdParser.Token ScanCondSection1()
		{
			if (this.chars[this.curPos] != 'I')
			{
				this.Throw(this.curPos, "Xml_ExpectIgnoreOrInclude");
			}
			this.curPos++;
			for (;;)
			{
				if (this.charsUsed - this.curPos >= 5)
				{
					char c = this.chars[this.curPos];
					if (c == 'G')
					{
						goto IL_0121;
					}
					if (c != 'N')
					{
						goto IL_01AA;
					}
					if (this.charsUsed - this.curPos >= 6)
					{
						break;
					}
				}
				if (this.ReadData() == 0)
				{
					this.Throw(this.curPos, "Xml_IncompleteDtdContent");
				}
			}
			if (this.chars[this.curPos + 1] == 'C' && this.chars[this.curPos + 2] == 'L' && this.chars[this.curPos + 3] == 'U' && this.chars[this.curPos + 4] == 'D' && this.chars[this.curPos + 5] == 'E' && !this.xmlCharType.IsNameChar(this.chars[this.curPos + 6]))
			{
				this.nextScaningFunction = DtdParser.ScanningFunction.SubsetContent;
				this.scanningFunction = DtdParser.ScanningFunction.CondSection2;
				this.curPos += 6;
				return DtdParser.Token.INCLUDE;
			}
			goto IL_01AA;
			IL_0121:
			if (this.chars[this.curPos + 1] == 'N' && this.chars[this.curPos + 2] == 'O' && this.chars[this.curPos + 3] == 'R' && this.chars[this.curPos + 4] == 'E' && !this.xmlCharType.IsNameChar(this.chars[this.curPos + 5]))
			{
				this.nextScaningFunction = DtdParser.ScanningFunction.CondSection3;
				this.scanningFunction = DtdParser.ScanningFunction.CondSection2;
				this.curPos += 5;
				return DtdParser.Token.IGNORE;
			}
			IL_01AA:
			this.Throw(this.curPos - 1, "Xml_ExpectIgnoreOrInclude");
			return DtdParser.Token.None;
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x00068715 File Offset: 0x00067715
		private DtdParser.Token ScanCondSection2()
		{
			if (this.chars[this.curPos] != '[')
			{
				this.ThrowUnexpectedToken(this.curPos, "[");
			}
			this.curPos++;
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.RightBracket;
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x00068758 File Offset: 0x00067758
		private unsafe DtdParser.Token ScanCondSection3()
		{
			int num = 0;
			char c2;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[this.chars[this.curPos]] & 64) == 0 || this.chars[this.curPos] == ']')
				{
					char c = this.chars[this.curPos];
					if (c <= '"')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
							this.curPos++;
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						case '\v':
						case '\f':
							goto IL_0222;
						case '\r':
							if (this.chars[this.curPos + 1] == '\n')
							{
								this.curPos += 2;
							}
							else
							{
								if (this.curPos + 1 >= this.charsUsed && !this.readerAdapter.IsEof)
								{
									goto IL_02BA;
								}
								this.curPos++;
							}
							this.readerAdapter.OnNewLine(this.curPos);
							continue;
						default:
							if (c != '"')
							{
								goto IL_0222;
							}
							break;
						}
					}
					else
					{
						switch (c)
						{
						case '&':
						case '\'':
							break;
						default:
							if (c != '<')
							{
								if (c != ']')
								{
									goto IL_0222;
								}
								if (this.charsUsed - this.curPos < 3)
								{
									goto IL_02BA;
								}
								if (this.chars[this.curPos + 1] != ']' || this.chars[this.curPos + 2] != '>')
								{
									this.curPos++;
									continue;
								}
								if (num > 0)
								{
									num--;
									this.curPos += 3;
									continue;
								}
								goto IL_020A;
							}
							else
							{
								if (this.charsUsed - this.curPos < 3)
								{
									goto IL_02BA;
								}
								if (this.chars[this.curPos + 1] != '!' || this.chars[this.curPos + 2] != '[')
								{
									this.curPos++;
									continue;
								}
								num++;
								this.curPos += 3;
								continue;
							}
							break;
						}
					}
					this.curPos++;
					continue;
					IL_0222:
					if (this.curPos != this.charsUsed)
					{
						c2 = this.chars[this.curPos];
						if (c2 < '\ud800' || c2 > '\udbff')
						{
							goto IL_02AA;
						}
						if (this.curPos + 1 != this.charsUsed)
						{
							this.curPos++;
							if (this.chars[this.curPos] >= '\udc00' && this.chars[this.curPos] <= '\udfff')
							{
								this.curPos++;
								continue;
							}
							goto IL_02AA;
						}
					}
					IL_02BA:
					if (this.readerAdapter.IsEof || this.ReadData() == 0)
					{
						if (this.HandleEntityEnd(false))
						{
							continue;
						}
						this.Throw(this.curPos, "Xml_UnclosedConditionalSection");
					}
					this.tokenStartPos = this.curPos;
				}
				else
				{
					this.curPos++;
				}
			}
			IL_020A:
			this.curPos += 3;
			this.scanningFunction = DtdParser.ScanningFunction.SubsetContent;
			return DtdParser.Token.CondSectionEnd;
			IL_02AA:
			this.ThrowInvalidChar(this.curPos, c2);
			return DtdParser.Token.None;
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x00068A61 File Offset: 0x00067A61
		private void ScanName()
		{
			this.ScanQName(false);
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x00068A6A File Offset: 0x00067A6A
		private void ScanQName()
		{
			this.ScanQName(this.supportNamespaces);
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x00068A78 File Offset: 0x00067A78
		private unsafe void ScanQName(bool isQName)
		{
			this.tokenStartPos = this.curPos;
			int num = -1;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[this.chars[this.curPos]] & 4) == 0 && this.chars[this.curPos] != ':')
				{
					if (this.curPos == this.charsUsed)
					{
						if (this.ReadDataInName())
						{
							continue;
						}
						this.Throw(this.curPos, "Xml_UnexpectedEOF", "Name");
					}
					else if (this.chars[this.curPos] != ':' || this.supportNamespaces)
					{
						this.Throw(this.curPos, "Xml_BadStartNameChar", XmlException.BuildCharExceptionStr(this.chars[this.curPos]));
					}
				}
				this.curPos++;
				for (;;)
				{
					if ((this.xmlCharType.charProperties[this.chars[this.curPos]] & 8) == 0)
					{
						if (this.chars[this.curPos] == ':')
						{
							if (isQName)
							{
								break;
							}
							this.curPos++;
						}
						else
						{
							if (this.charsUsed - this.curPos != 0)
							{
								goto IL_0182;
							}
							if (!this.ReadDataInName())
							{
								goto Block_11;
							}
						}
					}
					else
					{
						this.curPos++;
					}
				}
				if (num != -1)
				{
					this.Throw(this.curPos, "Xml_BadNameChar", XmlException.BuildCharExceptionStr(':'));
				}
				num = this.curPos - this.tokenStartPos;
				this.curPos++;
			}
			Block_11:
			if (this.tokenStartPos == this.curPos)
			{
				this.Throw(this.curPos, "Xml_UnexpectedEOF", "Name");
			}
			IL_0182:
			this.colonPos = ((num == -1) ? (-1) : (this.tokenStartPos + num));
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x00068C1C File Offset: 0x00067C1C
		private bool ReadDataInName()
		{
			int num = this.curPos - this.tokenStartPos;
			this.curPos = this.tokenStartPos;
			bool flag = this.ReadData() != 0;
			this.tokenStartPos = this.curPos;
			this.curPos += num;
			return flag;
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x00068C6C File Offset: 0x00067C6C
		private unsafe void ScanNmtoken()
		{
			this.tokenStartPos = this.curPos;
			int num;
			for (;;)
			{
				if ((this.xmlCharType.charProperties[this.chars[this.curPos]] & 8) == 0 && this.chars[this.curPos] != ':')
				{
					if (this.chars[this.curPos] != '\0')
					{
						break;
					}
					num = this.curPos - this.tokenStartPos;
					this.curPos = this.tokenStartPos;
					if (this.ReadData() == 0)
					{
						if (num > 0)
						{
							goto Block_6;
						}
						this.Throw(this.curPos, "Xml_UnexpectedEOF", "NmToken");
					}
					this.tokenStartPos = this.curPos;
					this.curPos += num;
				}
				else
				{
					this.curPos++;
				}
			}
			if (this.curPos - this.tokenStartPos == 0)
			{
				this.Throw(this.curPos, "Xml_BadNameChar", XmlException.BuildCharExceptionStr(this.chars[this.curPos]));
			}
			return;
			Block_6:
			this.tokenStartPos = this.curPos;
			this.curPos += num;
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x00068D7C File Offset: 0x00067D7C
		private bool EatPublicKeyword()
		{
			while (this.charsUsed - this.curPos < 6)
			{
				if (this.ReadData() == 0)
				{
					return false;
				}
			}
			if (this.chars[this.curPos + 1] != 'U' || this.chars[this.curPos + 2] != 'B' || this.chars[this.curPos + 3] != 'L' || this.chars[this.curPos + 4] != 'I' || this.chars[this.curPos + 5] != 'C')
			{
				return false;
			}
			this.curPos += 6;
			return true;
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x00068E18 File Offset: 0x00067E18
		private bool EatSystemKeyword()
		{
			while (this.charsUsed - this.curPos < 6)
			{
				if (this.ReadData() == 0)
				{
					return false;
				}
			}
			if (this.chars[this.curPos + 1] != 'Y' || this.chars[this.curPos + 2] != 'S' || this.chars[this.curPos + 3] != 'T' || this.chars[this.curPos + 4] != 'E' || this.chars[this.curPos + 5] != 'M')
			{
				return false;
			}
			this.curPos += 6;
			return true;
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x00068EB4 File Offset: 0x00067EB4
		private XmlQualifiedName GetNameQualified(bool canHavePrefix)
		{
			if (this.colonPos == -1)
			{
				return new XmlQualifiedName(this.nameTable.Add(this.chars, this.tokenStartPos, this.curPos - this.tokenStartPos));
			}
			if (canHavePrefix)
			{
				return new XmlQualifiedName(this.nameTable.Add(this.chars, this.colonPos + 1, this.curPos - this.colonPos - 1), this.nameTable.Add(this.chars, this.tokenStartPos, this.colonPos - this.tokenStartPos));
			}
			this.Throw(this.tokenStartPos, "Xml_ColonInLocalName", this.GetNameString());
			return null;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x00068F61 File Offset: 0x00067F61
		private string GetNameString()
		{
			return new string(this.chars, this.tokenStartPos, this.curPos - this.tokenStartPos);
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x00068F81 File Offset: 0x00067F81
		private string GetNmtokenString()
		{
			return this.GetNameString();
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x00068F89 File Offset: 0x00067F89
		private string GetValue()
		{
			if (this.stringBuilder.Length == 0)
			{
				return new string(this.chars, this.tokenStartPos, this.curPos - this.tokenStartPos - 1);
			}
			return this.stringBuilder.ToString();
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x00068FC4 File Offset: 0x00067FC4
		private string GetValueWithStrippedSpaces()
		{
			if (this.stringBuilder.Length == 0)
			{
				int num = this.curPos - this.tokenStartPos - 1;
				XmlComplianceUtil.StripSpaces(this.chars, this.tokenStartPos, ref num);
				return new string(this.chars, this.tokenStartPos, num);
			}
			return XmlComplianceUtil.StripSpaces(this.stringBuilder.ToString());
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x00069024 File Offset: 0x00068024
		private int ReadData()
		{
			this.SaveParsingBuffer();
			int num = this.readerAdapter.ReadData();
			this.LoadParsingBuffer();
			return num;
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x0006904A File Offset: 0x0006804A
		private void LoadParsingBuffer()
		{
			this.chars = this.readerAdapter.ParsingBuffer;
			this.charsUsed = this.readerAdapter.ParsingBufferLength;
			this.curPos = this.readerAdapter.CurrentPosition;
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x0006907F File Offset: 0x0006807F
		private void SaveParsingBuffer()
		{
			this.SaveParsingBuffer(this.curPos);
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x00069090 File Offset: 0x00068090
		private void SaveParsingBuffer(int internalSubsetValueEndPos)
		{
			if (this.SaveInternalSubsetValue)
			{
				int currentPosition = this.readerAdapter.CurrentPosition;
				if (internalSubsetValueEndPos - currentPosition > 0)
				{
					this.internalSubsetValueSb.Append(this.chars, currentPosition, internalSubsetValueEndPos - currentPosition);
				}
			}
			this.readerAdapter.CurrentPosition = this.curPos;
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x000690DD File Offset: 0x000680DD
		private bool HandleEntityReference(bool paramEntity, bool inLiteral, bool inAttribute)
		{
			this.curPos++;
			return this.HandleEntityReference(this.ScanEntityName(), paramEntity, inLiteral, inAttribute);
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x000690FC File Offset: 0x000680FC
		private bool HandleEntityReference(XmlQualifiedName entityName, bool paramEntity, bool inLiteral, bool inAttribute)
		{
			this.SaveParsingBuffer();
			if (paramEntity && this.ParsingInternalSubset && !this.ParsingTopLevelMarkup)
			{
				this.Throw(this.curPos - entityName.Name.Length - 1, "Xml_InvalidParEntityRef");
			}
			SchemaEntity schemaEntity = this.VerifyEntityReference(entityName, paramEntity, true, inAttribute);
			if (schemaEntity == null)
			{
				return false;
			}
			if (schemaEntity.IsProcessed)
			{
				this.Throw(this.curPos - entityName.Name.Length - 1, paramEntity ? "Xml_RecursiveParEntity" : "Xml_RecursiveGenEntity", entityName.Name);
			}
			int num = this.nextEntityId++;
			if (schemaEntity.IsExternal)
			{
				if (!this.readerAdapter.PushEntity(schemaEntity, num))
				{
					return false;
				}
				this.externalEntitiesDepth++;
			}
			else
			{
				if (schemaEntity.Text.Length == 0)
				{
					return false;
				}
				if (!this.readerAdapter.PushEntity(schemaEntity, num))
				{
					return false;
				}
			}
			this.currentEntityId = num;
			if (paramEntity && !inLiteral && this.scanningFunction != DtdParser.ScanningFunction.ParamEntitySpace)
			{
				this.savedScanningFunction = this.scanningFunction;
				this.scanningFunction = DtdParser.ScanningFunction.ParamEntitySpace;
			}
			this.LoadParsingBuffer();
			return true;
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x00069218 File Offset: 0x00068218
		private bool HandleEntityEnd(bool inLiteral)
		{
			this.SaveParsingBuffer();
			SchemaEntity schemaEntity;
			if (!this.readerAdapter.PopEntity(out schemaEntity, out this.currentEntityId))
			{
				return false;
			}
			this.LoadParsingBuffer();
			if (schemaEntity == null)
			{
				if (this.scanningFunction == DtdParser.ScanningFunction.ParamEntitySpace)
				{
					this.scanningFunction = this.savedScanningFunction;
				}
				return false;
			}
			if (schemaEntity.IsExternal)
			{
				this.externalEntitiesDepth--;
			}
			if (!inLiteral && this.scanningFunction != DtdParser.ScanningFunction.ParamEntitySpace)
			{
				this.savedScanningFunction = this.scanningFunction;
				this.scanningFunction = DtdParser.ScanningFunction.ParamEntitySpace;
			}
			return true;
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x0006929C File Offset: 0x0006829C
		private SchemaEntity VerifyEntityReference(XmlQualifiedName entityName, bool paramEntity, bool mustBeDeclared, bool inAttribute)
		{
			SchemaEntity schemaEntity;
			if (paramEntity)
			{
				schemaEntity = (SchemaEntity)this.schemaInfo.ParameterEntities[entityName];
			}
			else
			{
				schemaEntity = (SchemaEntity)this.schemaInfo.GeneralEntities[entityName];
			}
			if (schemaEntity == null)
			{
				if (paramEntity)
				{
					if (this.validate)
					{
						this.SendValidationEvent(this.curPos - entityName.Name.Length - 1, XmlSeverityType.Error, "Xml_UndeclaredParEntity", entityName.Name);
					}
				}
				else if (mustBeDeclared)
				{
					if (!this.ParsingInternalSubset)
					{
						this.SendValidationEvent(this.curPos - entityName.Name.Length - 1, XmlSeverityType.Error, "Xml_UndeclaredEntity", entityName.Name);
					}
					else
					{
						this.Throw(this.curPos - entityName.Name.Length - 1, "Xml_UndeclaredEntity", entityName.Name);
					}
				}
				return null;
			}
			if (!schemaEntity.NData.IsEmpty)
			{
				this.Throw(this.curPos - entityName.Name.Length - 1, "Xml_UnparsedEntityRef", entityName.Name);
			}
			if (inAttribute && schemaEntity.IsExternal)
			{
				this.Throw(this.curPos - entityName.Name.Length - 1, "Xml_ExternalEntityInAttValue", entityName.Name);
			}
			return schemaEntity;
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x000693D8 File Offset: 0x000683D8
		private void SendValidationEvent(int pos, XmlSeverityType severity, string code, string arg)
		{
			this.SendValidationEvent(severity, new XmlSchemaException(code, arg, this.BaseUriStr, this.LineNo, this.LinePos + (pos - this.curPos)));
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x0006940F File Offset: 0x0006840F
		private void SendValidationEvent(XmlSeverityType severity, string code, string arg)
		{
			this.SendValidationEvent(severity, new XmlSchemaException(code, arg, this.BaseUriStr, this.LineNo, this.LinePos));
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x00069431 File Offset: 0x00068431
		private void SendValidationEvent(XmlSeverityType severity, XmlSchemaException e)
		{
			this.readerAdapter.SendValidationEvent(severity, e);
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x00069440 File Offset: 0x00068440
		private bool IsAttributeValueType(DtdParser.Token token)
		{
			return token >= DtdParser.Token.CDATA && token <= DtdParser.Token.NOTATION;
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x060017D0 RID: 6096 RVA: 0x0006944F File Offset: 0x0006844F
		private int LineNo
		{
			get
			{
				return this.readerAdapter.LineNo;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x0006945C File Offset: 0x0006845C
		private int LinePos
		{
			get
			{
				return this.curPos - this.readerAdapter.LineStartPosition;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x060017D2 RID: 6098 RVA: 0x00069470 File Offset: 0x00068470
		private string BaseUriStr
		{
			get
			{
				Uri baseUri = this.readerAdapter.BaseUri;
				if (!(baseUri != null))
				{
					return string.Empty;
				}
				return baseUri.ToString();
			}
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x0006949E File Offset: 0x0006849E
		private void OnUnexpectedError()
		{
			this.Throw(this.curPos, "Xml_InternalError");
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x000694B1 File Offset: 0x000684B1
		private void Throw(int curPos, string res)
		{
			this.Throw(curPos, res, string.Empty);
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x000694C0 File Offset: 0x000684C0
		private void Throw(int curPos, string res, string arg)
		{
			this.curPos = curPos;
			Uri baseUri = this.readerAdapter.BaseUri;
			this.readerAdapter.Throw(new XmlException(res, arg, this.LineNo, this.LinePos, (baseUri == null) ? null : baseUri.ToString()));
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00069510 File Offset: 0x00068510
		private void Throw(int curPos, string res, string[] args)
		{
			this.curPos = curPos;
			Uri baseUri = this.readerAdapter.BaseUri;
			this.readerAdapter.Throw(new XmlException(res, args, this.LineNo, this.LinePos, (baseUri == null) ? null : baseUri.ToString()));
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00069560 File Offset: 0x00068560
		private void Throw(string res, string arg, int lineNo, int linePos)
		{
			Uri baseUri = this.readerAdapter.BaseUri;
			this.readerAdapter.Throw(new XmlException(res, arg, lineNo, linePos, (baseUri == null) ? null : baseUri.ToString()));
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x000695A0 File Offset: 0x000685A0
		private void ThrowInvalidChar(int pos, char invChar)
		{
			this.Throw(pos, "Xml_InvalidCharacter", XmlException.BuildCharExceptionStr(invChar));
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x000695B4 File Offset: 0x000685B4
		private void ThrowUnexpectedToken(int pos, string expectedToken)
		{
			this.ThrowUnexpectedToken(pos, expectedToken, null);
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x000695C0 File Offset: 0x000685C0
		private void ThrowUnexpectedToken(int pos, string expectedToken1, string expectedToken2)
		{
			string text = this.ParseUnexpectedToken(pos);
			if (expectedToken2 != null)
			{
				this.Throw(this.curPos, "Xml_UnexpectedTokens2", new string[] { text, expectedToken1, expectedToken2 });
				return;
			}
			this.Throw(this.curPos, "Xml_UnexpectedTokenEx", new string[] { text, expectedToken1 });
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00069620 File Offset: 0x00068620
		private string ParseUnexpectedToken(int startPos)
		{
			if (this.xmlCharType.IsNCNameChar(this.chars[startPos]))
			{
				int num = startPos + 1;
				while (this.xmlCharType.IsNCNameChar(this.chars[num]))
				{
					num++;
				}
				return new string(this.chars, startPos, num - startPos);
			}
			return new string(this.chars, startPos, 1);
		}

		// Token: 0x04000DB3 RID: 3507
		private const int CondSectionEntityIdsInitialSize = 2;

		// Token: 0x04000DB4 RID: 3508
		private IDtdParserAdapter readerAdapter;

		// Token: 0x04000DB5 RID: 3509
		private XmlNameTable nameTable;

		// Token: 0x04000DB6 RID: 3510
		private SchemaInfo schemaInfo;

		// Token: 0x04000DB7 RID: 3511
		private XmlCharType xmlCharType = XmlCharType.Instance;

		// Token: 0x04000DB8 RID: 3512
		private string systemId = string.Empty;

		// Token: 0x04000DB9 RID: 3513
		private string publicId = string.Empty;

		// Token: 0x04000DBA RID: 3514
		private bool validate;

		// Token: 0x04000DBB RID: 3515
		private bool normalize;

		// Token: 0x04000DBC RID: 3516
		private bool supportNamespaces;

		// Token: 0x04000DBD RID: 3517
		private bool v1Compat;

		// Token: 0x04000DBE RID: 3518
		private char[] chars;

		// Token: 0x04000DBF RID: 3519
		private int charsUsed;

		// Token: 0x04000DC0 RID: 3520
		private int curPos;

		// Token: 0x04000DC1 RID: 3521
		private DtdParser.ScanningFunction scanningFunction;

		// Token: 0x04000DC2 RID: 3522
		private DtdParser.ScanningFunction nextScaningFunction;

		// Token: 0x04000DC3 RID: 3523
		private DtdParser.ScanningFunction savedScanningFunction;

		// Token: 0x04000DC4 RID: 3524
		private bool whitespaceSeen;

		// Token: 0x04000DC5 RID: 3525
		private int tokenStartPos;

		// Token: 0x04000DC6 RID: 3526
		private int colonPos;

		// Token: 0x04000DC7 RID: 3527
		private BufferBuilder internalSubsetValueSb;

		// Token: 0x04000DC8 RID: 3528
		private string internalSubsetValue = string.Empty;

		// Token: 0x04000DC9 RID: 3529
		private int externalEntitiesDepth;

		// Token: 0x04000DCA RID: 3530
		private int currentEntityId;

		// Token: 0x04000DCB RID: 3531
		private int nextEntityId = 1;

		// Token: 0x04000DCC RID: 3532
		private int[] condSectionEntityIds;

		// Token: 0x04000DCD RID: 3533
		private bool freeFloatingDtd;

		// Token: 0x04000DCE RID: 3534
		private bool hasFreeFloatingInternalSubset;

		// Token: 0x04000DCF RID: 3535
		private BufferBuilder stringBuilder;

		// Token: 0x04000DD0 RID: 3536
		private Hashtable undeclaredNotations;

		// Token: 0x04000DD1 RID: 3537
		private int condSectionDepth;

		// Token: 0x04000DD2 RID: 3538
		private LineInfo literalLineInfo = new LineInfo(0, 0);

		// Token: 0x04000DD3 RID: 3539
		private char literalQuoteChar = '"';

		// Token: 0x04000DD4 RID: 3540
		private string documentBaseUri = string.Empty;

		// Token: 0x04000DD5 RID: 3541
		private string externalDtdBaseUri = string.Empty;

		// Token: 0x020001ED RID: 493
		private enum Token
		{
			// Token: 0x04000DD7 RID: 3543
			CDATA,
			// Token: 0x04000DD8 RID: 3544
			ID,
			// Token: 0x04000DD9 RID: 3545
			IDREF,
			// Token: 0x04000DDA RID: 3546
			IDREFS,
			// Token: 0x04000DDB RID: 3547
			ENTITY,
			// Token: 0x04000DDC RID: 3548
			ENTITIES,
			// Token: 0x04000DDD RID: 3549
			NMTOKEN,
			// Token: 0x04000DDE RID: 3550
			NMTOKENS,
			// Token: 0x04000DDF RID: 3551
			NOTATION,
			// Token: 0x04000DE0 RID: 3552
			None,
			// Token: 0x04000DE1 RID: 3553
			PERef,
			// Token: 0x04000DE2 RID: 3554
			AttlistDecl,
			// Token: 0x04000DE3 RID: 3555
			ElementDecl,
			// Token: 0x04000DE4 RID: 3556
			EntityDecl,
			// Token: 0x04000DE5 RID: 3557
			NotationDecl,
			// Token: 0x04000DE6 RID: 3558
			Comment,
			// Token: 0x04000DE7 RID: 3559
			PI,
			// Token: 0x04000DE8 RID: 3560
			CondSectionStart,
			// Token: 0x04000DE9 RID: 3561
			CondSectionEnd,
			// Token: 0x04000DEA RID: 3562
			Eof,
			// Token: 0x04000DEB RID: 3563
			REQUIRED,
			// Token: 0x04000DEC RID: 3564
			IMPLIED,
			// Token: 0x04000DED RID: 3565
			FIXED,
			// Token: 0x04000DEE RID: 3566
			QName,
			// Token: 0x04000DEF RID: 3567
			Name,
			// Token: 0x04000DF0 RID: 3568
			Nmtoken,
			// Token: 0x04000DF1 RID: 3569
			Quote,
			// Token: 0x04000DF2 RID: 3570
			LeftParen,
			// Token: 0x04000DF3 RID: 3571
			RightParen,
			// Token: 0x04000DF4 RID: 3572
			GreaterThan,
			// Token: 0x04000DF5 RID: 3573
			Or,
			// Token: 0x04000DF6 RID: 3574
			LeftBracket,
			// Token: 0x04000DF7 RID: 3575
			RightBracket,
			// Token: 0x04000DF8 RID: 3576
			PUBLIC,
			// Token: 0x04000DF9 RID: 3577
			SYSTEM,
			// Token: 0x04000DFA RID: 3578
			Literal,
			// Token: 0x04000DFB RID: 3579
			DOCTYPE,
			// Token: 0x04000DFC RID: 3580
			NData,
			// Token: 0x04000DFD RID: 3581
			Percent,
			// Token: 0x04000DFE RID: 3582
			Star,
			// Token: 0x04000DFF RID: 3583
			QMark,
			// Token: 0x04000E00 RID: 3584
			Plus,
			// Token: 0x04000E01 RID: 3585
			PCDATA,
			// Token: 0x04000E02 RID: 3586
			Comma,
			// Token: 0x04000E03 RID: 3587
			ANY,
			// Token: 0x04000E04 RID: 3588
			EMPTY,
			// Token: 0x04000E05 RID: 3589
			IGNORE,
			// Token: 0x04000E06 RID: 3590
			INCLUDE
		}

		// Token: 0x020001EE RID: 494
		private enum ScanningFunction
		{
			// Token: 0x04000E08 RID: 3592
			SubsetContent,
			// Token: 0x04000E09 RID: 3593
			Name,
			// Token: 0x04000E0A RID: 3594
			QName,
			// Token: 0x04000E0B RID: 3595
			Nmtoken,
			// Token: 0x04000E0C RID: 3596
			Doctype1,
			// Token: 0x04000E0D RID: 3597
			Doctype2,
			// Token: 0x04000E0E RID: 3598
			Element1,
			// Token: 0x04000E0F RID: 3599
			Element2,
			// Token: 0x04000E10 RID: 3600
			Element3,
			// Token: 0x04000E11 RID: 3601
			Element4,
			// Token: 0x04000E12 RID: 3602
			Element5,
			// Token: 0x04000E13 RID: 3603
			Element6,
			// Token: 0x04000E14 RID: 3604
			Element7,
			// Token: 0x04000E15 RID: 3605
			Attlist1,
			// Token: 0x04000E16 RID: 3606
			Attlist2,
			// Token: 0x04000E17 RID: 3607
			Attlist3,
			// Token: 0x04000E18 RID: 3608
			Attlist4,
			// Token: 0x04000E19 RID: 3609
			Attlist5,
			// Token: 0x04000E1A RID: 3610
			Attlist6,
			// Token: 0x04000E1B RID: 3611
			Attlist7,
			// Token: 0x04000E1C RID: 3612
			Entity1,
			// Token: 0x04000E1D RID: 3613
			Entity2,
			// Token: 0x04000E1E RID: 3614
			Entity3,
			// Token: 0x04000E1F RID: 3615
			Notation1,
			// Token: 0x04000E20 RID: 3616
			CondSection1,
			// Token: 0x04000E21 RID: 3617
			CondSection2,
			// Token: 0x04000E22 RID: 3618
			CondSection3,
			// Token: 0x04000E23 RID: 3619
			Literal,
			// Token: 0x04000E24 RID: 3620
			SystemId,
			// Token: 0x04000E25 RID: 3621
			PublicId1,
			// Token: 0x04000E26 RID: 3622
			PublicId2,
			// Token: 0x04000E27 RID: 3623
			ClosingTag,
			// Token: 0x04000E28 RID: 3624
			ParamEntitySpace,
			// Token: 0x04000E29 RID: 3625
			None
		}

		// Token: 0x020001EF RID: 495
		private enum LiteralType
		{
			// Token: 0x04000E2B RID: 3627
			AttributeValue,
			// Token: 0x04000E2C RID: 3628
			EntityReplText,
			// Token: 0x04000E2D RID: 3629
			SystemOrPublicID
		}

		// Token: 0x020001F0 RID: 496
		private class UndeclaredNotation
		{
			// Token: 0x060017DC RID: 6108 RVA: 0x0006967E File Offset: 0x0006867E
			internal UndeclaredNotation(string name, int lineNo, int linePos)
			{
				this.name = name;
				this.lineNo = lineNo;
				this.linePos = linePos;
				this.next = null;
			}

			// Token: 0x04000E2E RID: 3630
			internal string name;

			// Token: 0x04000E2F RID: 3631
			internal int lineNo;

			// Token: 0x04000E30 RID: 3632
			internal int linePos;

			// Token: 0x04000E31 RID: 3633
			internal DtdParser.UndeclaredNotation next;
		}

		// Token: 0x020001F1 RID: 497
		private class ParseElementOnlyContent_LocalFrame
		{
			// Token: 0x060017DD RID: 6109 RVA: 0x000696A2 File Offset: 0x000686A2
			public ParseElementOnlyContent_LocalFrame(int startParentEntityIdParam)
			{
				this.startParenEntityId = startParentEntityIdParam;
				this.parsingSchema = DtdParser.Token.None;
				this.connectorEntityId = startParentEntityIdParam;
				this.contentEntityId = -1;
			}

			// Token: 0x04000E32 RID: 3634
			public int startParenEntityId;

			// Token: 0x04000E33 RID: 3635
			public DtdParser.Token parsingSchema;

			// Token: 0x04000E34 RID: 3636
			public int connectorEntityId;

			// Token: 0x04000E35 RID: 3637
			public int contentEntityId;
		}
	}
}
