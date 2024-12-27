using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml.XmlConfiguration;

namespace System.Xml.Schema
{
	// Token: 0x0200021D RID: 541
	internal sealed class XdrBuilder : SchemaBuilder
	{
		// Token: 0x060019C1 RID: 6593 RVA: 0x0007BC38 File Offset: 0x0007AC38
		internal XdrBuilder(XmlReader reader, XmlNamespaceManager curmgr, SchemaInfo sinfo, string targetNamspace, XmlNameTable nameTable, SchemaNames schemaNames, ValidationEventHandler eventhandler)
		{
			this._SchemaInfo = sinfo;
			this._TargetNamespace = targetNamspace;
			this._reader = reader;
			this._CurNsMgr = curmgr;
			this.validationEventHandler = eventhandler;
			this._StateHistory = new HWStack(10);
			this._ElementDef = new XdrBuilder.ElementContent();
			this._AttributeDef = new XdrBuilder.AttributeContent();
			this._GroupStack = new HWStack(10);
			this._GroupDef = new XdrBuilder.GroupContent();
			this._NameTable = nameTable;
			this._SchemaNames = schemaNames;
			this._CurState = XdrBuilder.S_SchemaEntries[0];
			this.positionInfo = PositionInfo.GetPositionInfo(this._reader);
			this.xmlResolver = XmlReaderSection.CreateDefaultResolver();
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x0007BCF0 File Offset: 0x0007ACF0
		internal override bool ProcessElement(string prefix, string name, string ns)
		{
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(name, XmlSchemaDatatype.XdrCanonizeUri(ns, this._NameTable, this._SchemaNames));
			if (this.GetNextState(xmlQualifiedName))
			{
				this.Push();
				if (this._CurState._InitFunc != null)
				{
					this._CurState._InitFunc(this, xmlQualifiedName);
				}
				return true;
			}
			if (!this.IsSkipableElement(xmlQualifiedName))
			{
				this.SendValidationEvent("Sch_UnsupportedElement", XmlQualifiedName.ToString(name, prefix));
			}
			return false;
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x0007BD64 File Offset: 0x0007AD64
		internal override void ProcessAttribute(string prefix, string name, string ns, string value)
		{
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(name, XmlSchemaDatatype.XdrCanonizeUri(ns, this._NameTable, this._SchemaNames));
			int i = 0;
			while (i < this._CurState._Attributes.Length)
			{
				XdrBuilder.XdrAttributeEntry xdrAttributeEntry = this._CurState._Attributes[i];
				if (this._SchemaNames.TokenToQName[(int)xdrAttributeEntry._Attribute].Equals(xmlQualifiedName))
				{
					XdrBuilder.XdrBuildFunction buildFunc = xdrAttributeEntry._BuildFunc;
					if (xdrAttributeEntry._Datatype.TokenizedType == XmlTokenizedType.QName)
					{
						string text;
						XmlQualifiedName xmlQualifiedName2 = XmlQualifiedName.Parse(value, this._CurNsMgr, out text);
						xmlQualifiedName2.Atomize(this._NameTable);
						if (text.Length != 0)
						{
							if (xdrAttributeEntry._Attribute != SchemaNames.Token.SchemaType)
							{
								throw new XmlException("Xml_UnexpectedToken", "NAME");
							}
						}
						else if (this.IsGlobal(xdrAttributeEntry._SchemaFlags))
						{
							xmlQualifiedName2 = new XmlQualifiedName(xmlQualifiedName2.Name, this._TargetNamespace);
						}
						else
						{
							xmlQualifiedName2 = new XmlQualifiedName(xmlQualifiedName2.Name);
						}
						buildFunc(this, xmlQualifiedName2, text);
						return;
					}
					buildFunc(this, xdrAttributeEntry._Datatype.ParseValue(value, this._NameTable, this._CurNsMgr), string.Empty);
					return;
				}
				else
				{
					i++;
				}
			}
			if (ns == this._SchemaNames.NsXmlNs && XdrBuilder.IsXdrSchema(value))
			{
				this.LoadSchema(value);
				return;
			}
			if (!this.IsSkipableAttribute(xmlQualifiedName))
			{
				this.SendValidationEvent("Sch_UnsupportedAttribute", XmlQualifiedName.ToString(xmlQualifiedName.Name, prefix));
			}
		}

		// Token: 0x17000663 RID: 1635
		// (set) Token: 0x060019C4 RID: 6596 RVA: 0x0007BECE File Offset: 0x0007AECE
		internal XmlResolver XmlResolver
		{
			set
			{
				this.xmlResolver = value;
			}
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x0007BED8 File Offset: 0x0007AED8
		private bool LoadSchema(string uri)
		{
			if (this.xmlResolver == null)
			{
				return false;
			}
			uri = this._NameTable.Add(uri);
			if (this._SchemaInfo.TargetNamespaces.Contains(uri))
			{
				return false;
			}
			SchemaInfo schemaInfo = null;
			Uri uri2 = this.xmlResolver.ResolveUri(null, this._reader.BaseURI);
			XmlReader xmlReader = null;
			try
			{
				Uri uri3 = this.xmlResolver.ResolveUri(uri2, uri.Substring("x-schema:".Length));
				Stream stream = (Stream)this.xmlResolver.GetEntity(uri3, null, null);
				xmlReader = new XmlTextReader(uri3.ToString(), stream, this._NameTable);
				schemaInfo = new SchemaInfo();
				Parser parser = new Parser(SchemaType.XDR, this._NameTable, this._SchemaNames, this.validationEventHandler);
				parser.XmlResolver = this.xmlResolver;
				parser.Parse(xmlReader, uri);
				schemaInfo = parser.XdrSchema;
			}
			catch (XmlException ex)
			{
				this.SendValidationEvent("Sch_CannotLoadSchema", new string[] { uri, ex.Message }, XmlSeverityType.Warning);
				schemaInfo = null;
			}
			finally
			{
				if (xmlReader != null)
				{
					xmlReader.Close();
				}
			}
			if (schemaInfo != null && schemaInfo.ErrorCount == 0)
			{
				this._SchemaInfo.Add(schemaInfo, this.validationEventHandler);
				return true;
			}
			return false;
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x0007C02C File Offset: 0x0007B02C
		internal static bool IsXdrSchema(string uri)
		{
			return uri.Length >= "x-schema:".Length && string.Compare(uri, 0, "x-schema:", 0, "x-schema:".Length, StringComparison.Ordinal) == 0 && !uri.StartsWith("x-schema:#", StringComparison.Ordinal);
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x0007C06B File Offset: 0x0007B06B
		internal override bool IsContentParsed()
		{
			return true;
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x0007C06E File Offset: 0x0007B06E
		internal override void ProcessMarkup(XmlNode[] markup)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x0007C07F File Offset: 0x0007B07F
		internal override void ProcessCData(string value)
		{
			if (this._CurState._AllowText)
			{
				this._Text = value;
				return;
			}
			this.SendValidationEvent("Sch_TextNotAllowed", value);
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x0007C0A2 File Offset: 0x0007B0A2
		internal override void StartChildren()
		{
			if (this._CurState._BeginChildFunc != null)
			{
				this._CurState._BeginChildFunc(this);
			}
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x0007C0C2 File Offset: 0x0007B0C2
		internal override void EndChildren()
		{
			if (this._CurState._EndChildFunc != null)
			{
				this._CurState._EndChildFunc(this);
			}
			this.Pop();
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x0007C0E8 File Offset: 0x0007B0E8
		private void Push()
		{
			this._StateHistory.Push();
			this._StateHistory[this._StateHistory.Length - 1] = this._CurState;
			this._CurState = this._NextState;
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x0007C120 File Offset: 0x0007B120
		private void Pop()
		{
			this._CurState = (XdrBuilder.XdrEntry)this._StateHistory.Pop();
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x0007C138 File Offset: 0x0007B138
		private void PushGroupInfo()
		{
			this._GroupStack.Push();
			this._GroupStack[this._GroupStack.Length - 1] = XdrBuilder.GroupContent.Copy(this._GroupDef);
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x0007C169 File Offset: 0x0007B169
		private void PopGroupInfo()
		{
			this._GroupDef = (XdrBuilder.GroupContent)this._GroupStack.Pop();
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x0007C181 File Offset: 0x0007B181
		private static void XDR_InitRoot(XdrBuilder builder, object obj)
		{
			builder._SchemaInfo.SchemaType = SchemaType.XDR;
			builder._ElementDef._ElementDecl = null;
			builder._ElementDef._AttDefList = null;
			builder._AttributeDef._AttDef = null;
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x0007C1B3 File Offset: 0x0007B1B3
		private static void XDR_BuildRoot_Name(XdrBuilder builder, object obj, string prefix)
		{
			builder._XdrName = (string)obj;
			builder._XdrPrefix = prefix;
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x0007C1C8 File Offset: 0x0007B1C8
		private static void XDR_BuildRoot_ID(XdrBuilder builder, object obj, string prefix)
		{
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x0007C1CC File Offset: 0x0007B1CC
		private static void XDR_BeginRoot(XdrBuilder builder)
		{
			if (builder._TargetNamespace == null)
			{
				if (builder._XdrName != null)
				{
					builder._TargetNamespace = builder._NameTable.Add("x-schema:#" + builder._XdrName);
				}
				else
				{
					builder._TargetNamespace = string.Empty;
				}
			}
			builder._SchemaInfo.TargetNamespaces.Add(builder._TargetNamespace, true);
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x0007C234 File Offset: 0x0007B234
		private static void XDR_EndRoot(XdrBuilder builder)
		{
			while (builder._UndefinedAttributeTypes != null)
			{
				XmlQualifiedName xmlQualifiedName = builder._UndefinedAttributeTypes._TypeName;
				if (xmlQualifiedName.Namespace.Length == 0)
				{
					xmlQualifiedName = new XmlQualifiedName(xmlQualifiedName.Name, builder._TargetNamespace);
				}
				SchemaAttDef schemaAttDef = (SchemaAttDef)builder._SchemaInfo.AttributeDecls[xmlQualifiedName];
				if (schemaAttDef != null)
				{
					builder._UndefinedAttributeTypes._Attdef = schemaAttDef.Clone();
					builder._UndefinedAttributeTypes._Attdef.Name = xmlQualifiedName;
					builder.XDR_CheckAttributeDefault(builder._UndefinedAttributeTypes, builder._UndefinedAttributeTypes._Attdef);
				}
				else
				{
					builder.SendValidationEvent("Sch_UndeclaredAttribute", xmlQualifiedName.Name);
				}
				builder._UndefinedAttributeTypes = builder._UndefinedAttributeTypes._Next;
			}
			foreach (object obj in builder._UndeclaredElements.Values)
			{
				SchemaElementDecl schemaElementDecl = (SchemaElementDecl)obj;
				builder.SendValidationEvent("Sch_UndeclaredElement", XmlQualifiedName.ToString(schemaElementDecl.Name.Name, schemaElementDecl.Prefix));
			}
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x0007C364 File Offset: 0x0007B364
		private static void XDR_InitElementType(XdrBuilder builder, object obj)
		{
			builder._ElementDef._ElementDecl = new SchemaElementDecl();
			builder._contentValidator = new ParticleContentValidator(XmlSchemaContentType.Mixed);
			builder._contentValidator.IsOpen = true;
			builder._ElementDef._ContentAttr = 0;
			builder._ElementDef._OrderAttr = 0;
			builder._ElementDef._MasterGroupRequired = false;
			builder._ElementDef._ExistTerminal = false;
			builder._ElementDef._AllowDataType = true;
			builder._ElementDef._HasDataType = false;
			builder._ElementDef._EnumerationRequired = false;
			builder._ElementDef._AttDefList = new Hashtable();
			builder._ElementDef._MaxLength = uint.MaxValue;
			builder._ElementDef._MinLength = uint.MaxValue;
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x0007C418 File Offset: 0x0007B418
		private static void XDR_BuildElementType_Name(XdrBuilder builder, object obj, string prefix)
		{
			XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)obj;
			if (builder._SchemaInfo.ElementDecls[xmlQualifiedName] != null)
			{
				builder.SendValidationEvent("Sch_DupElementDecl", XmlQualifiedName.ToString(xmlQualifiedName.Name, prefix));
			}
			builder._ElementDef._ElementDecl.Name = xmlQualifiedName;
			builder._ElementDef._ElementDecl.Prefix = prefix;
			builder._SchemaInfo.ElementDecls.Add(xmlQualifiedName, builder._ElementDef._ElementDecl);
			if (builder._UndeclaredElements[xmlQualifiedName] != null)
			{
				builder._UndeclaredElements.Remove(xmlQualifiedName);
			}
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x0007C4AE File Offset: 0x0007B4AE
		private static void XDR_BuildElementType_Content(XdrBuilder builder, object obj, string prefix)
		{
			builder._ElementDef._ContentAttr = builder.GetContent((XmlQualifiedName)obj);
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x0007C4C7 File Offset: 0x0007B4C7
		private static void XDR_BuildElementType_Model(XdrBuilder builder, object obj, string prefix)
		{
			builder._contentValidator.IsOpen = builder.GetModel((XmlQualifiedName)obj);
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x0007C4E0 File Offset: 0x0007B4E0
		private static void XDR_BuildElementType_Order(XdrBuilder builder, object obj, string prefix)
		{
			builder._ElementDef._OrderAttr = (builder._GroupDef._Order = builder.GetOrder((XmlQualifiedName)obj));
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x0007C514 File Offset: 0x0007B514
		private static void XDR_BuildElementType_DtType(XdrBuilder builder, object obj, string prefix)
		{
			builder._ElementDef._HasDataType = true;
			string text = ((string)obj).Trim();
			if (text.Length == 0)
			{
				builder.SendValidationEvent("Sch_MissDtvalue");
				return;
			}
			XmlSchemaDatatype xmlSchemaDatatype = XmlSchemaDatatype.FromXdrName(text);
			if (xmlSchemaDatatype == null)
			{
				builder.SendValidationEvent("Sch_UnknownDtType", text);
			}
			builder._ElementDef._ElementDecl.Datatype = xmlSchemaDatatype;
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x0007C574 File Offset: 0x0007B574
		private static void XDR_BuildElementType_DtValues(XdrBuilder builder, object obj, string prefix)
		{
			builder._ElementDef._EnumerationRequired = true;
			builder._ElementDef._ElementDecl.Values = new ArrayList((string[])obj);
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x0007C59D File Offset: 0x0007B59D
		private static void XDR_BuildElementType_DtMaxLength(XdrBuilder builder, object obj, string prefix)
		{
			XdrBuilder.ParseDtMaxLength(ref builder._ElementDef._MaxLength, obj, builder);
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x0007C5B1 File Offset: 0x0007B5B1
		private static void XDR_BuildElementType_DtMinLength(XdrBuilder builder, object obj, string prefix)
		{
			XdrBuilder.ParseDtMinLength(ref builder._ElementDef._MinLength, obj, builder);
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x0007C5C8 File Offset: 0x0007B5C8
		private static void XDR_BeginElementType(XdrBuilder builder)
		{
			string text = null;
			string text2 = null;
			if (builder._ElementDef._ElementDecl.Name.IsEmpty)
			{
				text = "Sch_MissAttribute";
				text2 = "name";
			}
			else
			{
				if (builder._ElementDef._HasDataType)
				{
					if (!builder._ElementDef._AllowDataType)
					{
						text = "Sch_DataTypeTextOnly";
						goto IL_01F4;
					}
					builder._ElementDef._ContentAttr = 2;
				}
				else if (builder._ElementDef._ContentAttr == 0)
				{
					switch (builder._ElementDef._OrderAttr)
					{
					case 0:
						builder._ElementDef._ContentAttr = 3;
						builder._ElementDef._OrderAttr = 1;
						break;
					case 1:
						builder._ElementDef._ContentAttr = 3;
						break;
					case 2:
						builder._ElementDef._ContentAttr = 4;
						break;
					case 3:
						builder._ElementDef._ContentAttr = 4;
						break;
					}
				}
				bool isOpen = builder._contentValidator.IsOpen;
				XdrBuilder.ElementContent elementDef = builder._ElementDef;
				switch (builder._ElementDef._ContentAttr)
				{
				case 1:
					builder._ElementDef._ElementDecl.ContentValidator = ContentValidator.Empty;
					builder._contentValidator = null;
					break;
				case 2:
					builder._ElementDef._ElementDecl.ContentValidator = ContentValidator.TextOnly;
					builder._GroupDef._Order = 1;
					builder._contentValidator = null;
					break;
				case 3:
					if (elementDef._OrderAttr != 0 && elementDef._OrderAttr != 1)
					{
						text = "Sch_MixedMany";
						goto IL_01F4;
					}
					builder._GroupDef._Order = 1;
					elementDef._MasterGroupRequired = true;
					builder._contentValidator.IsOpen = isOpen;
					break;
				case 4:
					builder._contentValidator = new ParticleContentValidator(XmlSchemaContentType.ElementOnly);
					if (elementDef._OrderAttr == 0)
					{
						builder._GroupDef._Order = 2;
					}
					elementDef._MasterGroupRequired = true;
					builder._contentValidator.IsOpen = isOpen;
					break;
				}
				if (elementDef._ContentAttr == 3 || elementDef._ContentAttr == 4)
				{
					builder._contentValidator.Start();
					builder._contentValidator.OpenGroup();
				}
			}
			IL_01F4:
			if (text != null)
			{
				builder.SendValidationEvent(text, text2);
			}
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x0007C7D4 File Offset: 0x0007B7D4
		private static void XDR_EndElementType(XdrBuilder builder)
		{
			SchemaElementDecl elementDecl = builder._ElementDef._ElementDecl;
			if (elementDecl != null)
			{
				elementDecl.EndAddAttDef();
			}
			if (builder._UndefinedAttributeTypes != null && builder._ElementDef._AttDefList != null)
			{
				XdrBuilder.DeclBaseInfo declBaseInfo = builder._UndefinedAttributeTypes;
				XdrBuilder.DeclBaseInfo declBaseInfo2 = declBaseInfo;
				while (declBaseInfo != null)
				{
					SchemaAttDef schemaAttDef = null;
					if (declBaseInfo._ElementDecl == elementDecl)
					{
						XmlQualifiedName typeName = declBaseInfo._TypeName;
						schemaAttDef = (SchemaAttDef)builder._ElementDef._AttDefList[typeName];
						if (schemaAttDef != null)
						{
							declBaseInfo._Attdef = schemaAttDef.Clone();
							declBaseInfo._Attdef.Name = typeName;
							builder.XDR_CheckAttributeDefault(declBaseInfo, schemaAttDef);
							if (declBaseInfo == builder._UndefinedAttributeTypes)
							{
								declBaseInfo = (builder._UndefinedAttributeTypes = declBaseInfo._Next);
								declBaseInfo2 = declBaseInfo;
							}
							else
							{
								declBaseInfo2._Next = declBaseInfo._Next;
								declBaseInfo = declBaseInfo2._Next;
							}
						}
					}
					if (schemaAttDef == null)
					{
						if (declBaseInfo != builder._UndefinedAttributeTypes)
						{
							declBaseInfo2 = declBaseInfo2._Next;
						}
						declBaseInfo = declBaseInfo._Next;
					}
				}
			}
			if (builder._ElementDef._MasterGroupRequired)
			{
				builder._contentValidator.CloseGroup();
				if (!builder._ElementDef._ExistTerminal)
				{
					if (builder._contentValidator.IsOpen)
					{
						builder._ElementDef._ElementDecl.ContentValidator = ContentValidator.Any;
						builder._contentValidator = null;
					}
					else if (builder._ElementDef._ContentAttr != 3)
					{
						builder.SendValidationEvent("Sch_ElementMissing");
					}
				}
				else if (builder._GroupDef._Order == 1)
				{
					builder._contentValidator.AddStar();
				}
			}
			if (elementDecl.Datatype != null)
			{
				XmlTokenizedType tokenizedType = elementDecl.Datatype.TokenizedType;
				if (tokenizedType == XmlTokenizedType.ENUMERATION && !builder._ElementDef._EnumerationRequired)
				{
					builder.SendValidationEvent("Sch_MissDtvaluesAttribute");
				}
				if (tokenizedType != XmlTokenizedType.ENUMERATION && builder._ElementDef._EnumerationRequired)
				{
					builder.SendValidationEvent("Sch_RequireEnumeration");
				}
			}
			XdrBuilder.CompareMinMaxLength(builder._ElementDef._MinLength, builder._ElementDef._MaxLength, builder);
			elementDecl.MaxLength = (long)((ulong)builder._ElementDef._MaxLength);
			elementDecl.MinLength = (long)((ulong)builder._ElementDef._MinLength);
			if (builder._contentValidator != null)
			{
				builder._ElementDef._ElementDecl.ContentValidator = builder._contentValidator.Finish(true);
				builder._contentValidator = null;
			}
			builder._ElementDef._ElementDecl = null;
			builder._ElementDef._AttDefList = null;
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x0007CA18 File Offset: 0x0007BA18
		private static void XDR_InitAttributeType(XdrBuilder builder, object obj)
		{
			XdrBuilder.AttributeContent attributeDef = builder._AttributeDef;
			attributeDef._AttDef = new SchemaAttDef(XmlQualifiedName.Empty, null);
			attributeDef._Required = false;
			attributeDef._Prefix = null;
			attributeDef._Default = null;
			attributeDef._MinVal = 0U;
			attributeDef._MaxVal = 1U;
			attributeDef._EnumerationRequired = false;
			attributeDef._HasDataType = false;
			attributeDef._Global = builder._StateHistory.Length == 2;
			attributeDef._MaxLength = uint.MaxValue;
			attributeDef._MinLength = uint.MaxValue;
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x0007CA90 File Offset: 0x0007BA90
		private static void XDR_BuildAttributeType_Name(XdrBuilder builder, object obj, string prefix)
		{
			XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)obj;
			builder._AttributeDef._Name = xmlQualifiedName;
			builder._AttributeDef._Prefix = prefix;
			builder._AttributeDef._AttDef.Name = xmlQualifiedName;
			if (builder._ElementDef._ElementDecl != null)
			{
				if (builder._ElementDef._AttDefList[xmlQualifiedName] == null)
				{
					builder._ElementDef._AttDefList.Add(xmlQualifiedName, builder._AttributeDef._AttDef);
					return;
				}
				builder.SendValidationEvent("Sch_DupAttribute", XmlQualifiedName.ToString(xmlQualifiedName.Name, prefix));
				return;
			}
			else
			{
				xmlQualifiedName = new XmlQualifiedName(xmlQualifiedName.Name, builder._TargetNamespace);
				builder._AttributeDef._AttDef.Name = xmlQualifiedName;
				if (builder._SchemaInfo.AttributeDecls[xmlQualifiedName] == null)
				{
					builder._SchemaInfo.AttributeDecls.Add(xmlQualifiedName, builder._AttributeDef._AttDef);
					return;
				}
				builder.SendValidationEvent("Sch_DupAttribute", XmlQualifiedName.ToString(xmlQualifiedName.Name, prefix));
				return;
			}
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x0007CB8C File Offset: 0x0007BB8C
		private static void XDR_BuildAttributeType_Required(XdrBuilder builder, object obj, string prefix)
		{
			builder._AttributeDef._Required = XdrBuilder.IsYes(obj, builder);
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x0007CBA0 File Offset: 0x0007BBA0
		private static void XDR_BuildAttributeType_Default(XdrBuilder builder, object obj, string prefix)
		{
			builder._AttributeDef._Default = obj;
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x0007CBB0 File Offset: 0x0007BBB0
		private static void XDR_BuildAttributeType_DtType(XdrBuilder builder, object obj, string prefix)
		{
			XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)obj;
			builder._AttributeDef._HasDataType = true;
			builder._AttributeDef._AttDef.Datatype = builder.CheckDatatype(xmlQualifiedName.Name);
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x0007CBEC File Offset: 0x0007BBEC
		private static void XDR_BuildAttributeType_DtValues(XdrBuilder builder, object obj, string prefix)
		{
			builder._AttributeDef._EnumerationRequired = true;
			builder._AttributeDef._AttDef.Values = new ArrayList((string[])obj);
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x0007CC15 File Offset: 0x0007BC15
		private static void XDR_BuildAttributeType_DtMaxLength(XdrBuilder builder, object obj, string prefix)
		{
			XdrBuilder.ParseDtMaxLength(ref builder._AttributeDef._MaxLength, obj, builder);
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x0007CC29 File Offset: 0x0007BC29
		private static void XDR_BuildAttributeType_DtMinLength(XdrBuilder builder, object obj, string prefix)
		{
			XdrBuilder.ParseDtMinLength(ref builder._AttributeDef._MinLength, obj, builder);
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x0007CC3D File Offset: 0x0007BC3D
		private static void XDR_BeginAttributeType(XdrBuilder builder)
		{
			if (builder._AttributeDef._Name.IsEmpty)
			{
				builder.SendValidationEvent("Sch_MissAttribute");
			}
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x0007CC5C File Offset: 0x0007BC5C
		private static void XDR_EndAttributeType(XdrBuilder builder)
		{
			string text = null;
			if (builder._AttributeDef._HasDataType && builder._AttributeDef._AttDef.Datatype != null)
			{
				XmlTokenizedType tokenizedType = builder._AttributeDef._AttDef.Datatype.TokenizedType;
				if (tokenizedType == XmlTokenizedType.ENUMERATION && !builder._AttributeDef._EnumerationRequired)
				{
					text = "Sch_MissDtvaluesAttribute";
					goto IL_0164;
				}
				if (tokenizedType != XmlTokenizedType.ENUMERATION && builder._AttributeDef._EnumerationRequired)
				{
					text = "Sch_RequireEnumeration";
					goto IL_0164;
				}
				if (builder._AttributeDef._Default != null && tokenizedType == XmlTokenizedType.ID)
				{
					text = "Sch_DefaultIdValue";
					goto IL_0164;
				}
			}
			else
			{
				builder._AttributeDef._AttDef.Datatype = XmlSchemaDatatype.FromXmlTokenizedType(XmlTokenizedType.CDATA);
			}
			XdrBuilder.CompareMinMaxLength(builder._AttributeDef._MinLength, builder._AttributeDef._MaxLength, builder);
			builder._AttributeDef._AttDef.MaxLength = (long)((ulong)builder._AttributeDef._MaxLength);
			builder._AttributeDef._AttDef.MinLength = (long)((ulong)builder._AttributeDef._MinLength);
			if (builder._AttributeDef._Default != null)
			{
				builder._AttributeDef._AttDef.DefaultValueRaw = (builder._AttributeDef._AttDef.DefaultValueExpanded = (string)builder._AttributeDef._Default);
				builder.CheckDefaultAttValue(builder._AttributeDef._AttDef);
			}
			builder.SetAttributePresence(builder._AttributeDef._AttDef, builder._AttributeDef._Required);
			IL_0164:
			if (text != null)
			{
				builder.SendValidationEvent(text);
			}
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x0007CDD8 File Offset: 0x0007BDD8
		private static void XDR_InitElement(XdrBuilder builder, object obj)
		{
			if (builder._ElementDef._HasDataType || builder._ElementDef._ContentAttr == 1 || builder._ElementDef._ContentAttr == 2)
			{
				builder.SendValidationEvent("Sch_ElementNotAllowed");
			}
			builder._ElementDef._AllowDataType = false;
			builder._ElementDef._HasType = false;
			builder._ElementDef._MinVal = 1U;
			builder._ElementDef._MaxVal = 1U;
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x0007CE4C File Offset: 0x0007BE4C
		private static void XDR_BuildElement_Type(XdrBuilder builder, object obj, string prefix)
		{
			XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)obj;
			if (builder._SchemaInfo.ElementDecls[xmlQualifiedName] == null && (SchemaElementDecl)builder._UndeclaredElements[xmlQualifiedName] == null)
			{
				SchemaElementDecl schemaElementDecl = new SchemaElementDecl(xmlQualifiedName, prefix, SchemaType.DTD);
				builder._UndeclaredElements.Add(xmlQualifiedName, schemaElementDecl);
			}
			builder._ElementDef._HasType = true;
			if (builder._ElementDef._ExistTerminal)
			{
				builder.AddOrder();
			}
			else
			{
				builder._ElementDef._ExistTerminal = true;
			}
			builder._contentValidator.AddName(xmlQualifiedName, null);
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x0007CED8 File Offset: 0x0007BED8
		private static void XDR_BuildElement_MinOccurs(XdrBuilder builder, object obj, string prefix)
		{
			builder._ElementDef._MinVal = XdrBuilder.ParseMinOccurs(obj, builder);
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x0007CEEC File Offset: 0x0007BEEC
		private static void XDR_BuildElement_MaxOccurs(XdrBuilder builder, object obj, string prefix)
		{
			builder._ElementDef._MaxVal = XdrBuilder.ParseMaxOccurs(obj, builder);
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x0007CF00 File Offset: 0x0007BF00
		private static void XDR_EndElement(XdrBuilder builder)
		{
			if (builder._ElementDef._HasType)
			{
				XdrBuilder.HandleMinMax(builder._contentValidator, builder._ElementDef._MinVal, builder._ElementDef._MaxVal);
				return;
			}
			builder.SendValidationEvent("Sch_MissAttribute");
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x0007CF3C File Offset: 0x0007BF3C
		private static void XDR_InitAttribute(XdrBuilder builder, object obj)
		{
			if (builder._BaseDecl == null)
			{
				builder._BaseDecl = new XdrBuilder.DeclBaseInfo();
			}
			builder._BaseDecl._MinOccurs = 0U;
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x0007CF5D File Offset: 0x0007BF5D
		private static void XDR_BuildAttribute_Type(XdrBuilder builder, object obj, string prefix)
		{
			builder._BaseDecl._TypeName = (XmlQualifiedName)obj;
			builder._BaseDecl._Prefix = prefix;
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x0007CF7C File Offset: 0x0007BF7C
		private static void XDR_BuildAttribute_Required(XdrBuilder builder, object obj, string prefix)
		{
			if (XdrBuilder.IsYes(obj, builder))
			{
				builder._BaseDecl._MinOccurs = 1U;
			}
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x0007CF93 File Offset: 0x0007BF93
		private static void XDR_BuildAttribute_Default(XdrBuilder builder, object obj, string prefix)
		{
			builder._BaseDecl._Default = obj;
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x0007CFA4 File Offset: 0x0007BFA4
		private static void XDR_BeginAttribute(XdrBuilder builder)
		{
			if (builder._BaseDecl._TypeName.IsEmpty)
			{
				builder.SendValidationEvent("Sch_MissAttribute");
			}
			SchemaAttDef schemaAttDef = null;
			XmlQualifiedName typeName = builder._BaseDecl._TypeName;
			string prefix = builder._BaseDecl._Prefix;
			if (builder._ElementDef._AttDefList != null)
			{
				schemaAttDef = (SchemaAttDef)builder._ElementDef._AttDefList[typeName];
			}
			if (schemaAttDef == null)
			{
				XmlQualifiedName xmlQualifiedName = typeName;
				if (prefix.Length == 0)
				{
					xmlQualifiedName = new XmlQualifiedName(typeName.Name, builder._TargetNamespace);
				}
				SchemaAttDef schemaAttDef2 = (SchemaAttDef)builder._SchemaInfo.AttributeDecls[xmlQualifiedName];
				if (schemaAttDef2 != null)
				{
					schemaAttDef = schemaAttDef2.Clone();
					schemaAttDef.Name = typeName;
				}
				else if (prefix.Length != 0)
				{
					builder.SendValidationEvent("Sch_UndeclaredAttribute", XmlQualifiedName.ToString(typeName.Name, prefix));
				}
			}
			if (schemaAttDef != null)
			{
				builder.XDR_CheckAttributeDefault(builder._BaseDecl, schemaAttDef);
			}
			else
			{
				schemaAttDef = new SchemaAttDef(typeName, prefix);
				builder._UndefinedAttributeTypes = new XdrBuilder.DeclBaseInfo
				{
					_Checking = true,
					_Attdef = schemaAttDef,
					_TypeName = builder._BaseDecl._TypeName,
					_ElementDecl = builder._ElementDef._ElementDecl,
					_MinOccurs = builder._BaseDecl._MinOccurs,
					_Default = builder._BaseDecl._Default,
					_Next = builder._UndefinedAttributeTypes
				};
			}
			builder._ElementDef._ElementDecl.AddAttDef(schemaAttDef);
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x0007D116 File Offset: 0x0007C116
		private static void XDR_EndAttribute(XdrBuilder builder)
		{
			builder._BaseDecl.Reset();
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x0007D124 File Offset: 0x0007C124
		private static void XDR_InitGroup(XdrBuilder builder, object obj)
		{
			if (builder._ElementDef._ContentAttr == 1 || builder._ElementDef._ContentAttr == 2)
			{
				builder.SendValidationEvent("Sch_GroupDisabled");
			}
			builder.PushGroupInfo();
			builder._GroupDef._MinVal = 1U;
			builder._GroupDef._MaxVal = 1U;
			builder._GroupDef._HasMaxAttr = false;
			builder._GroupDef._HasMinAttr = false;
			if (builder._ElementDef._ExistTerminal)
			{
				builder.AddOrder();
			}
			builder._ElementDef._ExistTerminal = false;
			builder._contentValidator.OpenGroup();
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x0007D1B8 File Offset: 0x0007C1B8
		private static void XDR_BuildGroup_Order(XdrBuilder builder, object obj, string prefix)
		{
			builder._GroupDef._Order = builder.GetOrder((XmlQualifiedName)obj);
			if (builder._ElementDef._ContentAttr == 3 && builder._GroupDef._Order != 1)
			{
				builder.SendValidationEvent("Sch_MixedMany");
			}
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x0007D1F8 File Offset: 0x0007C1F8
		private static void XDR_BuildGroup_MinOccurs(XdrBuilder builder, object obj, string prefix)
		{
			builder._GroupDef._MinVal = XdrBuilder.ParseMinOccurs(obj, builder);
			builder._GroupDef._HasMinAttr = true;
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x0007D218 File Offset: 0x0007C218
		private static void XDR_BuildGroup_MaxOccurs(XdrBuilder builder, object obj, string prefix)
		{
			builder._GroupDef._MaxVal = XdrBuilder.ParseMaxOccurs(obj, builder);
			builder._GroupDef._HasMaxAttr = true;
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x0007D238 File Offset: 0x0007C238
		private static void XDR_EndGroup(XdrBuilder builder)
		{
			if (!builder._ElementDef._ExistTerminal)
			{
				builder.SendValidationEvent("Sch_ElementMissing");
			}
			builder._contentValidator.CloseGroup();
			if (builder._GroupDef._Order == 1)
			{
				builder._contentValidator.AddStar();
			}
			if (1 == builder._GroupDef._Order && builder._GroupDef._HasMaxAttr && builder._GroupDef._MaxVal != 4294967295U)
			{
				builder.SendValidationEvent("Sch_ManyMaxOccurs");
			}
			XdrBuilder.HandleMinMax(builder._contentValidator, builder._GroupDef._MinVal, builder._GroupDef._MaxVal);
			builder.PopGroupInfo();
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x0007D2DC File Offset: 0x0007C2DC
		private static void XDR_InitElementDtType(XdrBuilder builder, object obj)
		{
			if (builder._ElementDef._HasDataType)
			{
				builder.SendValidationEvent("Sch_DupDtType");
			}
			if (!builder._ElementDef._AllowDataType)
			{
				builder.SendValidationEvent("Sch_DataTypeTextOnly");
			}
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x0007D310 File Offset: 0x0007C310
		private static void XDR_EndElementDtType(XdrBuilder builder)
		{
			if (!builder._ElementDef._HasDataType)
			{
				builder.SendValidationEvent("Sch_MissAttribute");
			}
			builder._ElementDef._ElementDecl.ContentValidator = ContentValidator.TextOnly;
			builder._ElementDef._ContentAttr = 2;
			builder._ElementDef._MasterGroupRequired = false;
			builder._contentValidator = null;
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x0007D369 File Offset: 0x0007C369
		private static void XDR_InitAttributeDtType(XdrBuilder builder, object obj)
		{
			if (builder._AttributeDef._HasDataType)
			{
				builder.SendValidationEvent("Sch_DupDtType");
			}
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x0007D384 File Offset: 0x0007C384
		private static void XDR_EndAttributeDtType(XdrBuilder builder)
		{
			string text = null;
			if (!builder._AttributeDef._HasDataType)
			{
				text = "Sch_MissAttribute";
			}
			else if (builder._AttributeDef._AttDef.Datatype != null)
			{
				XmlTokenizedType tokenizedType = builder._AttributeDef._AttDef.Datatype.TokenizedType;
				if (tokenizedType == XmlTokenizedType.ENUMERATION && !builder._AttributeDef._EnumerationRequired)
				{
					text = "Sch_MissDtvaluesAttribute";
				}
				else if (tokenizedType != XmlTokenizedType.ENUMERATION && builder._AttributeDef._EnumerationRequired)
				{
					text = "Sch_RequireEnumeration";
				}
			}
			if (text != null)
			{
				builder.SendValidationEvent(text);
			}
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x0007D40C File Offset: 0x0007C40C
		private bool GetNextState(XmlQualifiedName qname)
		{
			if (this._CurState._NextStates != null)
			{
				for (int i = 0; i < this._CurState._NextStates.Length; i++)
				{
					if (this._SchemaNames.TokenToQName[(int)XdrBuilder.S_SchemaEntries[this._CurState._NextStates[i]]._Name].Equals(qname))
					{
						this._NextState = XdrBuilder.S_SchemaEntries[this._CurState._NextStates[i]];
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x0007D488 File Offset: 0x0007C488
		private bool IsSkipableElement(XmlQualifiedName qname)
		{
			string @namespace = qname.Namespace;
			return (@namespace != null && !Ref.Equal(@namespace, this._SchemaNames.NsXdr)) || (this._SchemaNames.TokenToQName[38].Equals(qname) || this._SchemaNames.TokenToQName[39].Equals(qname));
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x0007D4E4 File Offset: 0x0007C4E4
		private bool IsSkipableAttribute(XmlQualifiedName qname)
		{
			string @namespace = qname.Namespace;
			return (@namespace.Length != 0 && !Ref.Equal(@namespace, this._SchemaNames.NsXdr) && !Ref.Equal(@namespace, this._SchemaNames.NsDataType)) || (Ref.Equal(@namespace, this._SchemaNames.NsDataType) && this._CurState._Name == SchemaNames.Token.XdrDatatype && (this._SchemaNames.QnDtMax.Equals(qname) || this._SchemaNames.QnDtMin.Equals(qname) || this._SchemaNames.QnDtMaxExclusive.Equals(qname) || this._SchemaNames.QnDtMinExclusive.Equals(qname)));
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0007D59C File Offset: 0x0007C59C
		private int GetOrder(XmlQualifiedName qname)
		{
			int num = 0;
			if (this._SchemaNames.TokenToQName[15].Equals(qname))
			{
				num = 2;
			}
			else if (this._SchemaNames.TokenToQName[16].Equals(qname))
			{
				num = 3;
			}
			else if (this._SchemaNames.TokenToQName[17].Equals(qname))
			{
				num = 1;
			}
			else
			{
				this.SendValidationEvent("Sch_UnknownOrder", qname.Name);
			}
			return num;
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x0007D60C File Offset: 0x0007C60C
		private void AddOrder()
		{
			switch (this._GroupDef._Order)
			{
			case 1:
			case 3:
				this._contentValidator.AddChoice();
				return;
			case 2:
				this._contentValidator.AddSequence();
				return;
			}
			throw new XmlException("Xml_UnexpectedToken", "NAME");
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x0007D668 File Offset: 0x0007C668
		private static bool IsYes(object obj, XdrBuilder builder)
		{
			XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)obj;
			bool flag = false;
			if (xmlQualifiedName.Name == "yes")
			{
				flag = true;
			}
			else if (xmlQualifiedName.Name != "no")
			{
				builder.SendValidationEvent("Sch_UnknownRequired");
			}
			return flag;
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x0007D6B4 File Offset: 0x0007C6B4
		private static uint ParseMinOccurs(object obj, XdrBuilder builder)
		{
			uint num = 1U;
			if (!XdrBuilder.ParseInteger((string)obj, ref num) || (num != 0U && num != 1U))
			{
				builder.SendValidationEvent("Sch_MinOccursInvalid");
			}
			return num;
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x0007D6E8 File Offset: 0x0007C6E8
		private static uint ParseMaxOccurs(object obj, XdrBuilder builder)
		{
			uint maxValue = uint.MaxValue;
			string text = (string)obj;
			if (!text.Equals("*") && (!XdrBuilder.ParseInteger(text, ref maxValue) || (maxValue != 4294967295U && maxValue != 1U)))
			{
				builder.SendValidationEvent("Sch_MaxOccursInvalid");
			}
			return maxValue;
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x0007D729 File Offset: 0x0007C729
		private static void HandleMinMax(ParticleContentValidator pContent, uint cMin, uint cMax)
		{
			if (pContent != null)
			{
				if (cMax == 4294967295U)
				{
					if (cMin == 0U)
					{
						pContent.AddStar();
						return;
					}
					pContent.AddPlus();
					return;
				}
				else if (cMin == 0U)
				{
					pContent.AddQMark();
				}
			}
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x0007D74C File Offset: 0x0007C74C
		private static void ParseDtMaxLength(ref uint cVal, object obj, XdrBuilder builder)
		{
			if (4294967295U != cVal)
			{
				builder.SendValidationEvent("Sch_DupDtMaxLength");
			}
			if (!XdrBuilder.ParseInteger((string)obj, ref cVal) || cVal < 0U)
			{
				builder.SendValidationEvent("Sch_DtMaxLengthInvalid", obj.ToString());
			}
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x0007D782 File Offset: 0x0007C782
		private static void ParseDtMinLength(ref uint cVal, object obj, XdrBuilder builder)
		{
			if (4294967295U != cVal)
			{
				builder.SendValidationEvent("Sch_DupDtMinLength");
			}
			if (!XdrBuilder.ParseInteger((string)obj, ref cVal) || cVal < 0U)
			{
				builder.SendValidationEvent("Sch_DtMinLengthInvalid", obj.ToString());
			}
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x0007D7B8 File Offset: 0x0007C7B8
		private static void CompareMinMaxLength(uint cMin, uint cMax, XdrBuilder builder)
		{
			if (cMin != 4294967295U && cMax != 4294967295U && cMin > cMax)
			{
				builder.SendValidationEvent("Sch_DtMinMaxLength");
			}
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x0007D7D1 File Offset: 0x0007C7D1
		private static bool ParseInteger(string str, ref uint n)
		{
			return uint.TryParse(str, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out n);
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x0007D7E0 File Offset: 0x0007C7E0
		private void XDR_CheckAttributeDefault(XdrBuilder.DeclBaseInfo decl, SchemaAttDef pAttdef)
		{
			if ((decl._Default != null || pAttdef.DefaultValueTyped != null) && decl._Default != null)
			{
				pAttdef.DefaultValueRaw = (pAttdef.DefaultValueExpanded = (string)decl._Default);
				this.CheckDefaultAttValue(pAttdef);
			}
			this.SetAttributePresence(pAttdef, 1U == decl._MinOccurs);
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x0007D838 File Offset: 0x0007C838
		private void SetAttributePresence(SchemaAttDef pAttdef, bool fRequired)
		{
			if (SchemaDeclBase.Use.Fixed != pAttdef.Presence)
			{
				if (fRequired || SchemaDeclBase.Use.Required == pAttdef.Presence)
				{
					if (pAttdef.DefaultValueTyped != null)
					{
						pAttdef.Presence = SchemaDeclBase.Use.Fixed;
						return;
					}
					pAttdef.Presence = SchemaDeclBase.Use.Required;
					return;
				}
				else
				{
					if (pAttdef.DefaultValueTyped != null)
					{
						pAttdef.Presence = SchemaDeclBase.Use.Default;
						return;
					}
					pAttdef.Presence = SchemaDeclBase.Use.Implied;
				}
			}
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x0007D88C File Offset: 0x0007C88C
		private int GetContent(XmlQualifiedName qname)
		{
			int num = 0;
			if (this._SchemaNames.TokenToQName[11].Equals(qname))
			{
				num = 1;
				this._ElementDef._AllowDataType = false;
			}
			else if (this._SchemaNames.TokenToQName[12].Equals(qname))
			{
				num = 4;
				this._ElementDef._AllowDataType = false;
			}
			else if (this._SchemaNames.TokenToQName[10].Equals(qname))
			{
				num = 3;
				this._ElementDef._AllowDataType = false;
			}
			else if (this._SchemaNames.TokenToQName[13].Equals(qname))
			{
				num = 2;
			}
			else
			{
				this.SendValidationEvent("Sch_UnknownContent", qname.Name);
			}
			return num;
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x0007D93C File Offset: 0x0007C93C
		private bool GetModel(XmlQualifiedName qname)
		{
			bool flag = false;
			if (this._SchemaNames.TokenToQName[7].Equals(qname))
			{
				flag = true;
			}
			else if (this._SchemaNames.TokenToQName[8].Equals(qname))
			{
				flag = false;
			}
			else
			{
				this.SendValidationEvent("Sch_UnknownModel", qname.Name);
			}
			return flag;
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x0007D990 File Offset: 0x0007C990
		private XmlSchemaDatatype CheckDatatype(string str)
		{
			XmlSchemaDatatype xmlSchemaDatatype = XmlSchemaDatatype.FromXdrName(str);
			if (xmlSchemaDatatype == null)
			{
				this.SendValidationEvent("Sch_UnknownDtType", str);
			}
			else if (xmlSchemaDatatype.TokenizedType == XmlTokenizedType.ID && !this._AttributeDef._Global)
			{
				if (this._ElementDef._ElementDecl.IsIdDeclared)
				{
					this.SendValidationEvent("Sch_IdAttrDeclared", XmlQualifiedName.ToString(this._ElementDef._ElementDecl.Name.Name, this._ElementDef._ElementDecl.Prefix));
				}
				this._ElementDef._ElementDecl.IsIdDeclared = true;
			}
			return xmlSchemaDatatype;
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x0007DA24 File Offset: 0x0007CA24
		private void CheckDefaultAttValue(SchemaAttDef attDef)
		{
			string text = attDef.DefaultValueRaw.Trim();
			XdrValidator.CheckDefaultValue(text, attDef, this._SchemaInfo, this._CurNsMgr, this._NameTable, null, this.validationEventHandler, this._reader.BaseURI, this.positionInfo.LineNumber, this.positionInfo.LinePosition);
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0007DA7E File Offset: 0x0007CA7E
		private bool IsGlobal(int flags)
		{
			return flags == 256;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x0007DA88 File Offset: 0x0007CA88
		private void SendValidationEvent(string code, string[] args, XmlSeverityType severity)
		{
			this.SendValidationEvent(new XmlSchemaException(code, args, this._reader.BaseURI, this.positionInfo.LineNumber, this.positionInfo.LinePosition), severity);
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x0007DAB9 File Offset: 0x0007CAB9
		private void SendValidationEvent(string code)
		{
			this.SendValidationEvent(code, string.Empty);
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x0007DAC7 File Offset: 0x0007CAC7
		private void SendValidationEvent(string code, string msg)
		{
			this.SendValidationEvent(new XmlSchemaException(code, msg, this._reader.BaseURI, this.positionInfo.LineNumber, this.positionInfo.LinePosition), XmlSeverityType.Error);
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0007DAF8 File Offset: 0x0007CAF8
		private void SendValidationEvent(XmlSchemaException e, XmlSeverityType severity)
		{
			this._SchemaInfo.ErrorCount++;
			if (this.validationEventHandler != null)
			{
				this.validationEventHandler(this, new ValidationEventArgs(e, severity));
				return;
			}
			if (severity == XmlSeverityType.Error)
			{
				throw e;
			}
		}

		// Token: 0x04001027 RID: 4135
		private const int XdrSchema = 1;

		// Token: 0x04001028 RID: 4136
		private const int XdrElementType = 2;

		// Token: 0x04001029 RID: 4137
		private const int XdrAttributeType = 3;

		// Token: 0x0400102A RID: 4138
		private const int XdrElement = 4;

		// Token: 0x0400102B RID: 4139
		private const int XdrAttribute = 5;

		// Token: 0x0400102C RID: 4140
		private const int XdrGroup = 6;

		// Token: 0x0400102D RID: 4141
		private const int XdrElementDatatype = 7;

		// Token: 0x0400102E RID: 4142
		private const int XdrAttributeDatatype = 8;

		// Token: 0x0400102F RID: 4143
		private const int SchemaFlagsNs = 256;

		// Token: 0x04001030 RID: 4144
		private const int StackIncrement = 10;

		// Token: 0x04001031 RID: 4145
		private const int SchemaOrderNone = 0;

		// Token: 0x04001032 RID: 4146
		private const int SchemaOrderMany = 1;

		// Token: 0x04001033 RID: 4147
		private const int SchemaOrderSequence = 2;

		// Token: 0x04001034 RID: 4148
		private const int SchemaOrderChoice = 3;

		// Token: 0x04001035 RID: 4149
		private const int SchemaOrderAll = 4;

		// Token: 0x04001036 RID: 4150
		private const int SchemaContentNone = 0;

		// Token: 0x04001037 RID: 4151
		private const int SchemaContentEmpty = 1;

		// Token: 0x04001038 RID: 4152
		private const int SchemaContentText = 2;

		// Token: 0x04001039 RID: 4153
		private const int SchemaContentMixed = 3;

		// Token: 0x0400103A RID: 4154
		private const int SchemaContentElement = 4;

		// Token: 0x0400103B RID: 4155
		private const string x_schema = "x-schema:";

		// Token: 0x0400103C RID: 4156
		private static readonly int[] S_XDR_Root_Element = new int[] { 1 };

		// Token: 0x0400103D RID: 4157
		private static readonly int[] S_XDR_Root_SubElements = new int[] { 2, 3 };

		// Token: 0x0400103E RID: 4158
		private static readonly int[] S_XDR_ElementType_SubElements = new int[] { 4, 6, 3, 5, 7 };

		// Token: 0x0400103F RID: 4159
		private static readonly int[] S_XDR_AttributeType_SubElements = new int[] { 8 };

		// Token: 0x04001040 RID: 4160
		private static readonly int[] S_XDR_Group_SubElements = new int[] { 4, 6 };

		// Token: 0x04001041 RID: 4161
		private static readonly XdrBuilder.XdrAttributeEntry[] S_XDR_Root_Attributes = new XdrBuilder.XdrAttributeEntry[]
		{
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaName, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildRoot_Name)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaId, XmlTokenizedType.QName, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildRoot_ID))
		};

		// Token: 0x04001042 RID: 4162
		private static readonly XdrBuilder.XdrAttributeEntry[] S_XDR_ElementType_Attributes = new XdrBuilder.XdrAttributeEntry[]
		{
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaName, XmlTokenizedType.QName, 256, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildElementType_Name)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaContent, XmlTokenizedType.QName, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildElementType_Content)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaModel, XmlTokenizedType.QName, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildElementType_Model)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaOrder, XmlTokenizedType.QName, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildElementType_Order)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtType, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildElementType_DtType)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtValues, XmlTokenizedType.NMTOKENS, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildElementType_DtValues)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtMaxLength, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildElementType_DtMaxLength)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtMinLength, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildElementType_DtMinLength))
		};

		// Token: 0x04001043 RID: 4163
		private static readonly XdrBuilder.XdrAttributeEntry[] S_XDR_AttributeType_Attributes = new XdrBuilder.XdrAttributeEntry[]
		{
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaName, XmlTokenizedType.QName, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildAttributeType_Name)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaRequired, XmlTokenizedType.QName, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildAttributeType_Required)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDefault, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildAttributeType_Default)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtType, XmlTokenizedType.QName, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildAttributeType_DtType)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtValues, XmlTokenizedType.NMTOKENS, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildAttributeType_DtValues)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtMaxLength, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildAttributeType_DtMaxLength)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtMinLength, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildAttributeType_DtMinLength))
		};

		// Token: 0x04001044 RID: 4164
		private static readonly XdrBuilder.XdrAttributeEntry[] S_XDR_Element_Attributes = new XdrBuilder.XdrAttributeEntry[]
		{
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaType, XmlTokenizedType.QName, 256, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildElement_Type)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaMinOccurs, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildElement_MinOccurs)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaMaxOccurs, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildElement_MaxOccurs))
		};

		// Token: 0x04001045 RID: 4165
		private static readonly XdrBuilder.XdrAttributeEntry[] S_XDR_Attribute_Attributes = new XdrBuilder.XdrAttributeEntry[]
		{
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaType, XmlTokenizedType.QName, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildAttribute_Type)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaRequired, XmlTokenizedType.QName, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildAttribute_Required)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDefault, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildAttribute_Default))
		};

		// Token: 0x04001046 RID: 4166
		private static readonly XdrBuilder.XdrAttributeEntry[] S_XDR_Group_Attributes = new XdrBuilder.XdrAttributeEntry[]
		{
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaOrder, XmlTokenizedType.QName, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildGroup_Order)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaMinOccurs, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildGroup_MinOccurs)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaMaxOccurs, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildGroup_MaxOccurs))
		};

		// Token: 0x04001047 RID: 4167
		private static readonly XdrBuilder.XdrAttributeEntry[] S_XDR_ElementDataType_Attributes = new XdrBuilder.XdrAttributeEntry[]
		{
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtType, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildElementType_DtType)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtValues, XmlTokenizedType.NMTOKENS, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildElementType_DtValues)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtMaxLength, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildElementType_DtMaxLength)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtMinLength, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildElementType_DtMinLength))
		};

		// Token: 0x04001048 RID: 4168
		private static readonly XdrBuilder.XdrAttributeEntry[] S_XDR_AttributeDataType_Attributes = new XdrBuilder.XdrAttributeEntry[]
		{
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtType, XmlTokenizedType.QName, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildAttributeType_DtType)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtValues, XmlTokenizedType.NMTOKENS, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildAttributeType_DtValues)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtMaxLength, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildAttributeType_DtMaxLength)),
			new XdrBuilder.XdrAttributeEntry(SchemaNames.Token.SchemaDtMinLength, XmlTokenizedType.CDATA, new XdrBuilder.XdrBuildFunction(XdrBuilder.XDR_BuildAttributeType_DtMinLength))
		};

		// Token: 0x04001049 RID: 4169
		private static readonly XdrBuilder.XdrEntry[] S_SchemaEntries = new XdrBuilder.XdrEntry[]
		{
			new XdrBuilder.XdrEntry(SchemaNames.Token.Empty, XdrBuilder.S_XDR_Root_Element, null, null, null, null, false),
			new XdrBuilder.XdrEntry(SchemaNames.Token.XdrRoot, XdrBuilder.S_XDR_Root_SubElements, XdrBuilder.S_XDR_Root_Attributes, new XdrBuilder.XdrInitFunction(XdrBuilder.XDR_InitRoot), new XdrBuilder.XdrBeginChildFunction(XdrBuilder.XDR_BeginRoot), new XdrBuilder.XdrEndChildFunction(XdrBuilder.XDR_EndRoot), false),
			new XdrBuilder.XdrEntry(SchemaNames.Token.XdrElementType, XdrBuilder.S_XDR_ElementType_SubElements, XdrBuilder.S_XDR_ElementType_Attributes, new XdrBuilder.XdrInitFunction(XdrBuilder.XDR_InitElementType), new XdrBuilder.XdrBeginChildFunction(XdrBuilder.XDR_BeginElementType), new XdrBuilder.XdrEndChildFunction(XdrBuilder.XDR_EndElementType), false),
			new XdrBuilder.XdrEntry(SchemaNames.Token.XdrAttributeType, XdrBuilder.S_XDR_AttributeType_SubElements, XdrBuilder.S_XDR_AttributeType_Attributes, new XdrBuilder.XdrInitFunction(XdrBuilder.XDR_InitAttributeType), new XdrBuilder.XdrBeginChildFunction(XdrBuilder.XDR_BeginAttributeType), new XdrBuilder.XdrEndChildFunction(XdrBuilder.XDR_EndAttributeType), false),
			new XdrBuilder.XdrEntry(SchemaNames.Token.XdrElement, null, XdrBuilder.S_XDR_Element_Attributes, new XdrBuilder.XdrInitFunction(XdrBuilder.XDR_InitElement), null, new XdrBuilder.XdrEndChildFunction(XdrBuilder.XDR_EndElement), false),
			new XdrBuilder.XdrEntry(SchemaNames.Token.XdrAttribute, null, XdrBuilder.S_XDR_Attribute_Attributes, new XdrBuilder.XdrInitFunction(XdrBuilder.XDR_InitAttribute), new XdrBuilder.XdrBeginChildFunction(XdrBuilder.XDR_BeginAttribute), new XdrBuilder.XdrEndChildFunction(XdrBuilder.XDR_EndAttribute), false),
			new XdrBuilder.XdrEntry(SchemaNames.Token.XdrGroup, XdrBuilder.S_XDR_Group_SubElements, XdrBuilder.S_XDR_Group_Attributes, new XdrBuilder.XdrInitFunction(XdrBuilder.XDR_InitGroup), null, new XdrBuilder.XdrEndChildFunction(XdrBuilder.XDR_EndGroup), false),
			new XdrBuilder.XdrEntry(SchemaNames.Token.XdrDatatype, null, XdrBuilder.S_XDR_ElementDataType_Attributes, new XdrBuilder.XdrInitFunction(XdrBuilder.XDR_InitElementDtType), null, new XdrBuilder.XdrEndChildFunction(XdrBuilder.XDR_EndElementDtType), true),
			new XdrBuilder.XdrEntry(SchemaNames.Token.XdrDatatype, null, XdrBuilder.S_XDR_AttributeDataType_Attributes, new XdrBuilder.XdrInitFunction(XdrBuilder.XDR_InitAttributeDtType), null, new XdrBuilder.XdrEndChildFunction(XdrBuilder.XDR_EndAttributeDtType), true)
		};

		// Token: 0x0400104A RID: 4170
		private SchemaInfo _SchemaInfo;

		// Token: 0x0400104B RID: 4171
		private string _TargetNamespace;

		// Token: 0x0400104C RID: 4172
		private XmlReader _reader;

		// Token: 0x0400104D RID: 4173
		private PositionInfo positionInfo;

		// Token: 0x0400104E RID: 4174
		private ParticleContentValidator _contentValidator;

		// Token: 0x0400104F RID: 4175
		private XdrBuilder.XdrEntry _CurState;

		// Token: 0x04001050 RID: 4176
		private XdrBuilder.XdrEntry _NextState;

		// Token: 0x04001051 RID: 4177
		private HWStack _StateHistory;

		// Token: 0x04001052 RID: 4178
		private HWStack _GroupStack;

		// Token: 0x04001053 RID: 4179
		private string _XdrName;

		// Token: 0x04001054 RID: 4180
		private string _XdrPrefix;

		// Token: 0x04001055 RID: 4181
		private XdrBuilder.ElementContent _ElementDef;

		// Token: 0x04001056 RID: 4182
		private XdrBuilder.GroupContent _GroupDef;

		// Token: 0x04001057 RID: 4183
		private XdrBuilder.AttributeContent _AttributeDef;

		// Token: 0x04001058 RID: 4184
		private XdrBuilder.DeclBaseInfo _UndefinedAttributeTypes;

		// Token: 0x04001059 RID: 4185
		private XdrBuilder.DeclBaseInfo _BaseDecl;

		// Token: 0x0400105A RID: 4186
		private XmlNameTable _NameTable;

		// Token: 0x0400105B RID: 4187
		private SchemaNames _SchemaNames;

		// Token: 0x0400105C RID: 4188
		private XmlNamespaceManager _CurNsMgr;

		// Token: 0x0400105D RID: 4189
		private string _Text;

		// Token: 0x0400105E RID: 4190
		private ValidationEventHandler validationEventHandler;

		// Token: 0x0400105F RID: 4191
		private Hashtable _UndeclaredElements = new Hashtable();

		// Token: 0x04001060 RID: 4192
		private XmlResolver xmlResolver;

		// Token: 0x0200021E RID: 542
		private sealed class DeclBaseInfo
		{
			// Token: 0x06001A17 RID: 6679 RVA: 0x0007E120 File Offset: 0x0007D120
			internal DeclBaseInfo()
			{
				this.Reset();
			}

			// Token: 0x06001A18 RID: 6680 RVA: 0x0007E130 File Offset: 0x0007D130
			internal void Reset()
			{
				this._Name = XmlQualifiedName.Empty;
				this._Prefix = null;
				this._TypeName = XmlQualifiedName.Empty;
				this._TypePrefix = null;
				this._Default = null;
				this._Revises = null;
				this._MaxOccurs = 1U;
				this._MinOccurs = 1U;
				this._Checking = false;
				this._ElementDecl = null;
				this._Next = null;
				this._Attdef = null;
			}

			// Token: 0x04001061 RID: 4193
			internal XmlQualifiedName _Name;

			// Token: 0x04001062 RID: 4194
			internal string _Prefix;

			// Token: 0x04001063 RID: 4195
			internal XmlQualifiedName _TypeName;

			// Token: 0x04001064 RID: 4196
			internal string _TypePrefix;

			// Token: 0x04001065 RID: 4197
			internal object _Default;

			// Token: 0x04001066 RID: 4198
			internal object _Revises;

			// Token: 0x04001067 RID: 4199
			internal uint _MaxOccurs;

			// Token: 0x04001068 RID: 4200
			internal uint _MinOccurs;

			// Token: 0x04001069 RID: 4201
			internal bool _Checking;

			// Token: 0x0400106A RID: 4202
			internal SchemaElementDecl _ElementDecl;

			// Token: 0x0400106B RID: 4203
			internal SchemaAttDef _Attdef;

			// Token: 0x0400106C RID: 4204
			internal XdrBuilder.DeclBaseInfo _Next;
		}

		// Token: 0x0200021F RID: 543
		private sealed class GroupContent
		{
			// Token: 0x06001A19 RID: 6681 RVA: 0x0007E199 File Offset: 0x0007D199
			internal static void Copy(XdrBuilder.GroupContent from, XdrBuilder.GroupContent to)
			{
				to._MinVal = from._MinVal;
				to._MaxVal = from._MaxVal;
				to._Order = from._Order;
			}

			// Token: 0x06001A1A RID: 6682 RVA: 0x0007E1C0 File Offset: 0x0007D1C0
			internal static XdrBuilder.GroupContent Copy(XdrBuilder.GroupContent other)
			{
				XdrBuilder.GroupContent groupContent = new XdrBuilder.GroupContent();
				XdrBuilder.GroupContent.Copy(other, groupContent);
				return groupContent;
			}

			// Token: 0x0400106D RID: 4205
			internal uint _MinVal;

			// Token: 0x0400106E RID: 4206
			internal uint _MaxVal;

			// Token: 0x0400106F RID: 4207
			internal bool _HasMaxAttr;

			// Token: 0x04001070 RID: 4208
			internal bool _HasMinAttr;

			// Token: 0x04001071 RID: 4209
			internal int _Order;
		}

		// Token: 0x02000220 RID: 544
		private sealed class ElementContent
		{
			// Token: 0x04001072 RID: 4210
			internal SchemaElementDecl _ElementDecl;

			// Token: 0x04001073 RID: 4211
			internal int _ContentAttr;

			// Token: 0x04001074 RID: 4212
			internal int _OrderAttr;

			// Token: 0x04001075 RID: 4213
			internal bool _MasterGroupRequired;

			// Token: 0x04001076 RID: 4214
			internal bool _ExistTerminal;

			// Token: 0x04001077 RID: 4215
			internal bool _AllowDataType;

			// Token: 0x04001078 RID: 4216
			internal bool _HasDataType;

			// Token: 0x04001079 RID: 4217
			internal bool _HasType;

			// Token: 0x0400107A RID: 4218
			internal bool _EnumerationRequired;

			// Token: 0x0400107B RID: 4219
			internal uint _MinVal;

			// Token: 0x0400107C RID: 4220
			internal uint _MaxVal;

			// Token: 0x0400107D RID: 4221
			internal uint _MaxLength;

			// Token: 0x0400107E RID: 4222
			internal uint _MinLength;

			// Token: 0x0400107F RID: 4223
			internal Hashtable _AttDefList;
		}

		// Token: 0x02000221 RID: 545
		private sealed class AttributeContent
		{
			// Token: 0x04001080 RID: 4224
			internal SchemaAttDef _AttDef;

			// Token: 0x04001081 RID: 4225
			internal XmlQualifiedName _Name;

			// Token: 0x04001082 RID: 4226
			internal string _Prefix;

			// Token: 0x04001083 RID: 4227
			internal bool _Required;

			// Token: 0x04001084 RID: 4228
			internal uint _MinVal;

			// Token: 0x04001085 RID: 4229
			internal uint _MaxVal;

			// Token: 0x04001086 RID: 4230
			internal uint _MaxLength;

			// Token: 0x04001087 RID: 4231
			internal uint _MinLength;

			// Token: 0x04001088 RID: 4232
			internal bool _EnumerationRequired;

			// Token: 0x04001089 RID: 4233
			internal bool _HasDataType;

			// Token: 0x0400108A RID: 4234
			internal bool _Global;

			// Token: 0x0400108B RID: 4235
			internal object _Default;
		}

		// Token: 0x02000222 RID: 546
		// (Invoke) Token: 0x06001A1F RID: 6687
		private delegate void XdrBuildFunction(XdrBuilder builder, object obj, string prefix);

		// Token: 0x02000223 RID: 547
		// (Invoke) Token: 0x06001A23 RID: 6691
		private delegate void XdrInitFunction(XdrBuilder builder, object obj);

		// Token: 0x02000224 RID: 548
		// (Invoke) Token: 0x06001A27 RID: 6695
		private delegate void XdrBeginChildFunction(XdrBuilder builder);

		// Token: 0x02000225 RID: 549
		// (Invoke) Token: 0x06001A2B RID: 6699
		private delegate void XdrEndChildFunction(XdrBuilder builder);

		// Token: 0x02000226 RID: 550
		private sealed class XdrAttributeEntry
		{
			// Token: 0x06001A2E RID: 6702 RVA: 0x0007E1F3 File Offset: 0x0007D1F3
			internal XdrAttributeEntry(SchemaNames.Token a, XmlTokenizedType ttype, XdrBuilder.XdrBuildFunction build)
			{
				this._Attribute = a;
				this._Datatype = XmlSchemaDatatype.FromXmlTokenizedType(ttype);
				this._SchemaFlags = 0;
				this._BuildFunc = build;
			}

			// Token: 0x06001A2F RID: 6703 RVA: 0x0007E21C File Offset: 0x0007D21C
			internal XdrAttributeEntry(SchemaNames.Token a, XmlTokenizedType ttype, int schemaFlags, XdrBuilder.XdrBuildFunction build)
			{
				this._Attribute = a;
				this._Datatype = XmlSchemaDatatype.FromXmlTokenizedType(ttype);
				this._SchemaFlags = schemaFlags;
				this._BuildFunc = build;
			}

			// Token: 0x0400108C RID: 4236
			internal SchemaNames.Token _Attribute;

			// Token: 0x0400108D RID: 4237
			internal int _SchemaFlags;

			// Token: 0x0400108E RID: 4238
			internal XmlSchemaDatatype _Datatype;

			// Token: 0x0400108F RID: 4239
			internal XdrBuilder.XdrBuildFunction _BuildFunc;
		}

		// Token: 0x02000227 RID: 551
		private sealed class XdrEntry
		{
			// Token: 0x06001A30 RID: 6704 RVA: 0x0007E246 File Offset: 0x0007D246
			internal XdrEntry(SchemaNames.Token n, int[] states, XdrBuilder.XdrAttributeEntry[] attributes, XdrBuilder.XdrInitFunction init, XdrBuilder.XdrBeginChildFunction begin, XdrBuilder.XdrEndChildFunction end, bool fText)
			{
				this._Name = n;
				this._NextStates = states;
				this._Attributes = attributes;
				this._InitFunc = init;
				this._BeginChildFunc = begin;
				this._EndChildFunc = end;
				this._AllowText = fText;
			}

			// Token: 0x04001090 RID: 4240
			internal SchemaNames.Token _Name;

			// Token: 0x04001091 RID: 4241
			internal int[] _NextStates;

			// Token: 0x04001092 RID: 4242
			internal XdrBuilder.XdrAttributeEntry[] _Attributes;

			// Token: 0x04001093 RID: 4243
			internal XdrBuilder.XdrInitFunction _InitFunc;

			// Token: 0x04001094 RID: 4244
			internal XdrBuilder.XdrBeginChildFunction _BeginChildFunc;

			// Token: 0x04001095 RID: 4245
			internal XdrBuilder.XdrEndChildFunction _EndChildFunc;

			// Token: 0x04001096 RID: 4246
			internal bool _AllowText;
		}
	}
}
