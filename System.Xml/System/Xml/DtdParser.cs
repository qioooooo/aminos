using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Xml
{
	internal class DtdParser
	{
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

		internal static SchemaInfo Parse(XmlTextReaderImpl tr, string baseUri, string docTypeName, string publicId, string systemId, string internalSubset)
		{
			XmlTextReaderImpl.DtdParserProxy dtdParserProxy = new XmlTextReaderImpl.DtdParserProxy(baseUri, docTypeName, publicId, systemId, internalSubset, tr);
			dtdParserProxy.Parse(false);
			return dtdParserProxy.DtdSchemaInfo;
		}

		internal string SystemID
		{
			get
			{
				return this.systemId;
			}
		}

		internal string PublicID
		{
			get
			{
				return this.publicId;
			}
		}

		internal string InternalSubset
		{
			get
			{
				return this.internalSubsetValue;
			}
		}

		internal SchemaInfo SchemaInfo
		{
			get
			{
				return this.schemaInfo;
			}
		}

		private bool ParsingInternalSubset
		{
			get
			{
				return this.externalEntitiesDepth == 0;
			}
		}

		private bool IgnoreEntityReferences
		{
			get
			{
				return this.scanningFunction == DtdParser.ScanningFunction.CondSection3;
			}
		}

		private bool SaveInternalSubsetValue
		{
			get
			{
				return this.readerAdapter.EntityStackLength == 0 && this.internalSubsetValueSb != null;
			}
		}

		private bool ParsingTopLevelMarkup
		{
			get
			{
				return this.scanningFunction == DtdParser.ScanningFunction.SubsetContent || (this.scanningFunction == DtdParser.ScanningFunction.ParamEntitySpace && this.savedScanningFunction == DtdParser.ScanningFunction.SubsetContent);
			}
		}

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

		private void ParseInternalSubset()
		{
			this.ParseSubset();
		}

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

		private DtdParser.Token ScanNameExpected()
		{
			this.ScanName();
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.Name;
		}

		private DtdParser.Token ScanQNameExpected()
		{
			this.ScanQName();
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.QName;
		}

		private DtdParser.Token ScanNmtokenExpected()
		{
			this.ScanNmtoken();
			this.scanningFunction = this.nextScaningFunction;
			return DtdParser.Token.Nmtoken;
		}

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

		private void ScanName()
		{
			this.ScanQName(false);
		}

		private void ScanQName()
		{
			this.ScanQName(this.supportNamespaces);
		}

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

		private bool ReadDataInName()
		{
			int num = this.curPos - this.tokenStartPos;
			this.curPos = this.tokenStartPos;
			bool flag = this.ReadData() != 0;
			this.tokenStartPos = this.curPos;
			this.curPos += num;
			return flag;
		}

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

		private string GetNameString()
		{
			return new string(this.chars, this.tokenStartPos, this.curPos - this.tokenStartPos);
		}

		private string GetNmtokenString()
		{
			return this.GetNameString();
		}

		private string GetValue()
		{
			if (this.stringBuilder.Length == 0)
			{
				return new string(this.chars, this.tokenStartPos, this.curPos - this.tokenStartPos - 1);
			}
			return this.stringBuilder.ToString();
		}

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

		private int ReadData()
		{
			this.SaveParsingBuffer();
			int num = this.readerAdapter.ReadData();
			this.LoadParsingBuffer();
			return num;
		}

		private void LoadParsingBuffer()
		{
			this.chars = this.readerAdapter.ParsingBuffer;
			this.charsUsed = this.readerAdapter.ParsingBufferLength;
			this.curPos = this.readerAdapter.CurrentPosition;
		}

		private void SaveParsingBuffer()
		{
			this.SaveParsingBuffer(this.curPos);
		}

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

		private bool HandleEntityReference(bool paramEntity, bool inLiteral, bool inAttribute)
		{
			this.curPos++;
			return this.HandleEntityReference(this.ScanEntityName(), paramEntity, inLiteral, inAttribute);
		}

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

		private void SendValidationEvent(int pos, XmlSeverityType severity, string code, string arg)
		{
			this.SendValidationEvent(severity, new XmlSchemaException(code, arg, this.BaseUriStr, this.LineNo, this.LinePos + (pos - this.curPos)));
		}

		private void SendValidationEvent(XmlSeverityType severity, string code, string arg)
		{
			this.SendValidationEvent(severity, new XmlSchemaException(code, arg, this.BaseUriStr, this.LineNo, this.LinePos));
		}

		private void SendValidationEvent(XmlSeverityType severity, XmlSchemaException e)
		{
			this.readerAdapter.SendValidationEvent(severity, e);
		}

		private bool IsAttributeValueType(DtdParser.Token token)
		{
			return token >= DtdParser.Token.CDATA && token <= DtdParser.Token.NOTATION;
		}

		private int LineNo
		{
			get
			{
				return this.readerAdapter.LineNo;
			}
		}

		private int LinePos
		{
			get
			{
				return this.curPos - this.readerAdapter.LineStartPosition;
			}
		}

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

		private void OnUnexpectedError()
		{
			this.Throw(this.curPos, "Xml_InternalError");
		}

		private void Throw(int curPos, string res)
		{
			this.Throw(curPos, res, string.Empty);
		}

		private void Throw(int curPos, string res, string arg)
		{
			this.curPos = curPos;
			Uri baseUri = this.readerAdapter.BaseUri;
			this.readerAdapter.Throw(new XmlException(res, arg, this.LineNo, this.LinePos, (baseUri == null) ? null : baseUri.ToString()));
		}

		private void Throw(int curPos, string res, string[] args)
		{
			this.curPos = curPos;
			Uri baseUri = this.readerAdapter.BaseUri;
			this.readerAdapter.Throw(new XmlException(res, args, this.LineNo, this.LinePos, (baseUri == null) ? null : baseUri.ToString()));
		}

		private void Throw(string res, string arg, int lineNo, int linePos)
		{
			Uri baseUri = this.readerAdapter.BaseUri;
			this.readerAdapter.Throw(new XmlException(res, arg, lineNo, linePos, (baseUri == null) ? null : baseUri.ToString()));
		}

		private void ThrowInvalidChar(int pos, char invChar)
		{
			this.Throw(pos, "Xml_InvalidCharacter", XmlException.BuildCharExceptionStr(invChar));
		}

		private void ThrowUnexpectedToken(int pos, string expectedToken)
		{
			this.ThrowUnexpectedToken(pos, expectedToken, null);
		}

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

		private const int CondSectionEntityIdsInitialSize = 2;

		private IDtdParserAdapter readerAdapter;

		private XmlNameTable nameTable;

		private SchemaInfo schemaInfo;

		private XmlCharType xmlCharType = XmlCharType.Instance;

		private string systemId = string.Empty;

		private string publicId = string.Empty;

		private bool validate;

		private bool normalize;

		private bool supportNamespaces;

		private bool v1Compat;

		private char[] chars;

		private int charsUsed;

		private int curPos;

		private DtdParser.ScanningFunction scanningFunction;

		private DtdParser.ScanningFunction nextScaningFunction;

		private DtdParser.ScanningFunction savedScanningFunction;

		private bool whitespaceSeen;

		private int tokenStartPos;

		private int colonPos;

		private BufferBuilder internalSubsetValueSb;

		private string internalSubsetValue = string.Empty;

		private int externalEntitiesDepth;

		private int currentEntityId;

		private int nextEntityId = 1;

		private int[] condSectionEntityIds;

		private bool freeFloatingDtd;

		private bool hasFreeFloatingInternalSubset;

		private BufferBuilder stringBuilder;

		private Hashtable undeclaredNotations;

		private int condSectionDepth;

		private LineInfo literalLineInfo = new LineInfo(0, 0);

		private char literalQuoteChar = '"';

		private string documentBaseUri = string.Empty;

		private string externalDtdBaseUri = string.Empty;

		private enum Token
		{
			CDATA,
			ID,
			IDREF,
			IDREFS,
			ENTITY,
			ENTITIES,
			NMTOKEN,
			NMTOKENS,
			NOTATION,
			None,
			PERef,
			AttlistDecl,
			ElementDecl,
			EntityDecl,
			NotationDecl,
			Comment,
			PI,
			CondSectionStart,
			CondSectionEnd,
			Eof,
			REQUIRED,
			IMPLIED,
			FIXED,
			QName,
			Name,
			Nmtoken,
			Quote,
			LeftParen,
			RightParen,
			GreaterThan,
			Or,
			LeftBracket,
			RightBracket,
			PUBLIC,
			SYSTEM,
			Literal,
			DOCTYPE,
			NData,
			Percent,
			Star,
			QMark,
			Plus,
			PCDATA,
			Comma,
			ANY,
			EMPTY,
			IGNORE,
			INCLUDE
		}

		private enum ScanningFunction
		{
			SubsetContent,
			Name,
			QName,
			Nmtoken,
			Doctype1,
			Doctype2,
			Element1,
			Element2,
			Element3,
			Element4,
			Element5,
			Element6,
			Element7,
			Attlist1,
			Attlist2,
			Attlist3,
			Attlist4,
			Attlist5,
			Attlist6,
			Attlist7,
			Entity1,
			Entity2,
			Entity3,
			Notation1,
			CondSection1,
			CondSection2,
			CondSection3,
			Literal,
			SystemId,
			PublicId1,
			PublicId2,
			ClosingTag,
			ParamEntitySpace,
			None
		}

		private enum LiteralType
		{
			AttributeValue,
			EntityReplText,
			SystemOrPublicID
		}

		private class UndeclaredNotation
		{
			internal UndeclaredNotation(string name, int lineNo, int linePos)
			{
				this.name = name;
				this.lineNo = lineNo;
				this.linePos = linePos;
				this.next = null;
			}

			internal string name;

			internal int lineNo;

			internal int linePos;

			internal DtdParser.UndeclaredNotation next;
		}

		private class ParseElementOnlyContent_LocalFrame
		{
			public ParseElementOnlyContent_LocalFrame(int startParentEntityIdParam)
			{
				this.startParenEntityId = startParentEntityIdParam;
				this.parsingSchema = DtdParser.Token.None;
				this.connectorEntityId = startParentEntityIdParam;
				this.contentEntityId = -1;
			}

			public int startParenEntityId;

			public DtdParser.Token parsingSchema;

			public int connectorEntityId;

			public int contentEntityId;
		}
	}
}
