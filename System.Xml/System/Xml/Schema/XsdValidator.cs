using System;
using System.Collections;
using System.IO;
using System.Text;

namespace System.Xml.Schema
{
	internal sealed class XsdValidator : BaseValidator
	{
		internal XsdValidator(BaseValidator validator)
			: base(validator)
		{
			this.Init();
		}

		internal XsdValidator(XmlValidatingReaderImpl reader, XmlSchemaCollection schemaCollection, ValidationEventHandler eventHandler)
			: base(reader, schemaCollection, eventHandler)
		{
			this.Init();
		}

		private void Init()
		{
			this.nsManager = this.reader.NamespaceManager;
			if (this.nsManager == null)
			{
				this.nsManager = new XmlNamespaceManager(base.NameTable);
				this.bManageNamespaces = true;
			}
			this.validationStack = new HWStack(10);
			this.textValue = new StringBuilder();
			this.attPresence = new Hashtable();
			this.schemaInfo = new SchemaInfo();
			this.checkDatatype = false;
			this.processContents = XmlSchemaContentProcessing.Strict;
			this.Push(XmlQualifiedName.Empty);
			this.NsXmlNs = base.NameTable.Add("http://www.w3.org/2000/xmlns/");
			this.NsXs = base.NameTable.Add("http://www.w3.org/2001/XMLSchema");
			this.NsXsi = base.NameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
			this.XsiType = base.NameTable.Add("type");
			this.XsiNil = base.NameTable.Add("nil");
			this.XsiSchemaLocation = base.NameTable.Add("schemaLocation");
			this.XsiNoNamespaceSchemaLocation = base.NameTable.Add("noNamespaceSchemaLocation");
			this.XsdSchema = base.NameTable.Add("schema");
		}

		public override void Validate()
		{
			if (this.IsInlineSchemaStarted)
			{
				this.ProcessInlineSchema();
				return;
			}
			XmlNodeType nodeType = this.reader.NodeType;
			switch (nodeType)
			{
			case XmlNodeType.Element:
				this.ValidateElement();
				if (this.reader.IsEmptyElement)
				{
					goto IL_006C;
				}
				return;
			case XmlNodeType.Attribute:
				return;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				break;
			default:
				switch (nodeType)
				{
				case XmlNodeType.Whitespace:
					base.ValidateWhitespace();
					return;
				case XmlNodeType.SignificantWhitespace:
					break;
				case XmlNodeType.EndElement:
					goto IL_006C;
				default:
					return;
				}
				break;
			}
			base.ValidateText();
			return;
			IL_006C:
			this.ValidateEndElement();
		}

		public override void CompleteValidation()
		{
			this.CheckForwardRefs();
		}

		public ValidationState Context
		{
			set
			{
				this.context = value;
			}
		}

		public static XmlSchemaDatatype DtQName
		{
			get
			{
				return XsdValidator.dtQName;
			}
		}

		private bool IsInlineSchemaStarted
		{
			get
			{
				return this.inlineSchemaParser != null;
			}
		}

		private void ProcessInlineSchema()
		{
			if (!this.inlineSchemaParser.ParseReaderNode())
			{
				this.inlineSchemaParser.FinishParsing();
				XmlSchema xmlSchema = this.inlineSchemaParser.XmlSchema;
				if (xmlSchema != null && xmlSchema.ErrorCount == 0)
				{
					try
					{
						SchemaInfo schemaInfo = new SchemaInfo();
						schemaInfo.SchemaType = SchemaType.XSD;
						string text = ((xmlSchema.TargetNamespace == null) ? string.Empty : xmlSchema.TargetNamespace);
						if (!base.SchemaInfo.TargetNamespaces.Contains(text) && base.SchemaCollection.Add(text, schemaInfo, xmlSchema, true) != null)
						{
							base.SchemaInfo.Add(schemaInfo, base.EventHandler);
						}
					}
					catch (XmlSchemaException ex)
					{
						base.SendValidationEvent("Sch_CannotLoadSchema", new string[]
						{
							base.BaseUri.AbsoluteUri,
							ex.Message
						}, XmlSeverityType.Error);
					}
				}
				this.inlineSchemaParser = null;
			}
		}

		private void ValidateElement()
		{
			this.elementName.Init(this.reader.LocalName, this.reader.NamespaceURI);
			object obj = this.ValidateChildElement();
			if (this.IsXSDRoot(this.elementName.Name, this.elementName.Namespace) && this.reader.Depth > 0)
			{
				this.inlineSchemaParser = new Parser(SchemaType.XSD, base.NameTable, base.SchemaNames, base.EventHandler);
				this.inlineSchemaParser.StartParsing(this.reader, null);
				this.inlineSchemaParser.ParseReaderNode();
				return;
			}
			this.ProcessElement(obj);
		}

		private object ValidateChildElement()
		{
			object obj = null;
			int num = 0;
			if (this.context.NeedValidateChildren)
			{
				if (this.context.IsNill)
				{
					base.SendValidationEvent("Sch_ContentInNill", this.elementName.ToString());
					return null;
				}
				obj = this.context.ElementDecl.ContentValidator.ValidateElement(this.elementName, this.context, out num);
				if (obj == null)
				{
					this.processContents = (this.context.ProcessContents = XmlSchemaContentProcessing.Skip);
					if (num == -2)
					{
						base.SendValidationEvent("Sch_AllElement", this.elementName.ToString());
					}
					XmlSchemaValidator.ElementValidationError(this.elementName, this.context, base.EventHandler, this.reader, this.reader.BaseURI, base.PositionInfo.LineNumber, base.PositionInfo.LinePosition, false);
				}
			}
			return obj;
		}

		private void ProcessElement(object particle)
		{
			SchemaElementDecl schemaElementDecl = this.FastGetElementDecl(particle);
			this.Push(this.elementName);
			if (this.bManageNamespaces)
			{
				this.nsManager.PushScope();
			}
			XmlQualifiedName xmlQualifiedName;
			string text;
			this.ProcessXsiAttributes(out xmlQualifiedName, out text);
			if (this.processContents != XmlSchemaContentProcessing.Skip)
			{
				if (schemaElementDecl == null || !xmlQualifiedName.IsEmpty || text != null)
				{
					schemaElementDecl = this.ThoroughGetElementDecl(schemaElementDecl, xmlQualifiedName, text);
				}
				if (schemaElementDecl == null)
				{
					if (this.HasSchema && this.processContents == XmlSchemaContentProcessing.Strict)
					{
						base.SendValidationEvent("Sch_UndeclaredElement", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
					}
					else
					{
						base.SendValidationEvent("Sch_NoElementSchemaFound", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace), XmlSeverityType.Warning);
					}
				}
			}
			this.context.ElementDecl = schemaElementDecl;
			this.ValidateStartElementIdentityConstraints();
			this.ValidateStartElement();
			if (this.context.ElementDecl != null)
			{
				this.ValidateEndStartElement();
				this.context.NeedValidateChildren = this.processContents != XmlSchemaContentProcessing.Skip;
				this.context.ElementDecl.ContentValidator.InitValidation(this.context);
			}
		}

		private void ProcessXsiAttributes(out XmlQualifiedName xsiType, out string xsiNil)
		{
			string[] array = null;
			string text = null;
			xsiType = XmlQualifiedName.Empty;
			xsiNil = null;
			if (this.reader.Depth == 0)
			{
				this.LoadSchema(string.Empty, null);
				foreach (string text2 in this.nsManager.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml).Values)
				{
					this.LoadSchema(text2, null);
				}
			}
			if (this.reader.MoveToFirstAttribute())
			{
				do
				{
					string namespaceURI = this.reader.NamespaceURI;
					string localName = this.reader.LocalName;
					if (Ref.Equal(namespaceURI, this.NsXmlNs))
					{
						this.LoadSchema(this.reader.Value, null);
						if (this.bManageNamespaces)
						{
							this.nsManager.AddNamespace((this.reader.Prefix.Length == 0) ? string.Empty : this.reader.LocalName, this.reader.Value);
						}
					}
					else if (Ref.Equal(namespaceURI, this.NsXsi))
					{
						if (Ref.Equal(localName, this.XsiSchemaLocation))
						{
							array = (string[])XsdValidator.dtStringArray.ParseValue(this.reader.Value, base.NameTable, this.nsManager);
						}
						else if (Ref.Equal(localName, this.XsiNoNamespaceSchemaLocation))
						{
							text = this.reader.Value;
						}
						else if (Ref.Equal(localName, this.XsiType))
						{
							xsiType = (XmlQualifiedName)XsdValidator.dtQName.ParseValue(this.reader.Value, base.NameTable, this.nsManager);
						}
						else if (Ref.Equal(localName, this.XsiNil))
						{
							xsiNil = this.reader.Value;
						}
					}
				}
				while (this.reader.MoveToNextAttribute());
				this.reader.MoveToElement();
			}
			if (text != null)
			{
				this.LoadSchema(string.Empty, text);
			}
			if (array != null)
			{
				for (int i = 0; i < array.Length - 1; i += 2)
				{
					this.LoadSchema(array[i], array[i + 1]);
				}
			}
		}

		private void ValidateEndElement()
		{
			if (this.bManageNamespaces)
			{
				this.nsManager.PopScope();
			}
			if (this.context.ElementDecl != null)
			{
				if (!this.context.IsNill)
				{
					if (this.context.NeedValidateChildren && !this.context.ElementDecl.ContentValidator.CompleteValidation(this.context))
					{
						XmlSchemaValidator.CompleteValidationError(this.context, base.EventHandler, this.reader, this.reader.BaseURI, base.PositionInfo.LineNumber, base.PositionInfo.LinePosition, false);
					}
					if (this.checkDatatype && !this.context.IsNill)
					{
						string text = ((!this.hasSibling) ? this.textString : this.textValue.ToString());
						if (text.Length != 0 || this.context.ElementDecl.DefaultValueTyped == null)
						{
							this.CheckValue(text, null);
							this.checkDatatype = false;
						}
					}
				}
				if (this.HasIdentityConstraints)
				{
					this.EndElementIdentityConstraints();
				}
			}
			this.Pop();
		}

		private SchemaElementDecl FastGetElementDecl(object particle)
		{
			SchemaElementDecl schemaElementDecl = null;
			if (particle != null)
			{
				XmlSchemaElement xmlSchemaElement = particle as XmlSchemaElement;
				if (xmlSchemaElement != null)
				{
					schemaElementDecl = xmlSchemaElement.ElementDecl;
				}
				else
				{
					XmlSchemaAny xmlSchemaAny = (XmlSchemaAny)particle;
					this.processContents = xmlSchemaAny.ProcessContentsCorrect;
				}
			}
			return schemaElementDecl;
		}

		private SchemaElementDecl ThoroughGetElementDecl(SchemaElementDecl elementDecl, XmlQualifiedName xsiType, string xsiNil)
		{
			if (elementDecl == null)
			{
				elementDecl = this.schemaInfo.GetElementDecl(this.elementName);
			}
			if (elementDecl != null)
			{
				if (xsiType.IsEmpty)
				{
					if (elementDecl.IsAbstract)
					{
						base.SendValidationEvent("Sch_AbstractElement", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
						elementDecl = null;
					}
				}
				else if (xsiNil != null && xsiNil.Equals("true"))
				{
					base.SendValidationEvent("Sch_XsiNilAndType");
				}
				else
				{
					SchemaElementDecl schemaElementDecl = (SchemaElementDecl)this.schemaInfo.ElementDeclsByType[xsiType];
					if (schemaElementDecl == null && xsiType.Namespace == this.NsXs)
					{
						XmlSchemaSimpleType simpleTypeFromXsdType = DatatypeImplementation.GetSimpleTypeFromXsdType(new XmlQualifiedName(xsiType.Name, this.NsXs));
						if (simpleTypeFromXsdType != null)
						{
							schemaElementDecl = simpleTypeFromXsdType.ElementDecl;
						}
					}
					if (schemaElementDecl == null)
					{
						base.SendValidationEvent("Sch_XsiTypeNotFound", xsiType.ToString());
						elementDecl = null;
					}
					else if (!XmlSchemaType.IsDerivedFrom(schemaElementDecl.SchemaType, elementDecl.SchemaType, elementDecl.Block))
					{
						base.SendValidationEvent("Sch_XsiTypeBlockedEx", new string[]
						{
							xsiType.ToString(),
							XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace)
						});
						elementDecl = null;
					}
					else
					{
						elementDecl = schemaElementDecl;
					}
				}
				if (elementDecl != null && elementDecl.IsNillable)
				{
					if (xsiNil != null)
					{
						this.context.IsNill = XmlConvert.ToBoolean(xsiNil);
						if (this.context.IsNill && elementDecl.DefaultValueTyped != null)
						{
							base.SendValidationEvent("Sch_XsiNilAndFixed");
						}
					}
				}
				else if (xsiNil != null)
				{
					base.SendValidationEvent("Sch_InvalidXsiNill");
				}
			}
			return elementDecl;
		}

		private void ValidateStartElement()
		{
			if (this.context.ElementDecl != null)
			{
				if (this.context.ElementDecl.IsAbstract)
				{
					base.SendValidationEvent("Sch_AbstractElement", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
				}
				this.reader.SchemaTypeObject = this.context.ElementDecl.SchemaType;
				if (this.reader.IsEmptyElement && !this.context.IsNill && this.context.ElementDecl.DefaultValueTyped != null)
				{
					this.reader.TypedValueObject = this.UnWrapUnion(this.context.ElementDecl.DefaultValueTyped);
					this.context.IsNill = true;
				}
				else
				{
					this.reader.TypedValueObject = null;
				}
				if (this.context.ElementDecl.HasRequiredAttribute || this.HasIdentityConstraints)
				{
					this.attPresence.Clear();
				}
			}
			if (this.reader.MoveToFirstAttribute())
			{
				do
				{
					if (this.reader.NamespaceURI != this.NsXmlNs && this.reader.NamespaceURI != this.NsXsi)
					{
						try
						{
							this.reader.SchemaTypeObject = null;
							XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(this.reader.LocalName, this.reader.NamespaceURI);
							bool flag = this.processContents == XmlSchemaContentProcessing.Skip;
							SchemaAttDef attributeXsd = this.schemaInfo.GetAttributeXsd(this.context.ElementDecl, xmlQualifiedName, ref flag);
							if (attributeXsd != null)
							{
								if (this.context.ElementDecl != null && (this.context.ElementDecl.HasRequiredAttribute || this.startIDConstraint != -1))
								{
									this.attPresence.Add(attributeXsd.Name, attributeXsd);
								}
								this.reader.SchemaTypeObject = attributeXsd.SchemaType;
								if (attributeXsd.Datatype != null)
								{
									this.CheckValue(this.reader.Value, attributeXsd);
								}
								if (this.HasIdentityConstraints)
								{
									this.AttributeIdentityConstraints(this.reader.LocalName, this.reader.NamespaceURI, this.reader.TypedValueObject, this.reader.Value, attributeXsd);
								}
							}
							else if (!flag)
							{
								if (this.context.ElementDecl == null && this.processContents == XmlSchemaContentProcessing.Strict && xmlQualifiedName.Namespace.Length != 0 && this.schemaInfo.Contains(xmlQualifiedName.Namespace))
								{
									base.SendValidationEvent("Sch_UndeclaredAttribute", xmlQualifiedName.ToString());
								}
								else
								{
									base.SendValidationEvent("Sch_NoAttributeSchemaFound", xmlQualifiedName.ToString(), XmlSeverityType.Warning);
								}
							}
						}
						catch (XmlSchemaException ex)
						{
							ex.SetSource(this.reader.BaseURI, base.PositionInfo.LineNumber, base.PositionInfo.LinePosition);
							base.SendValidationEvent(ex);
						}
					}
				}
				while (this.reader.MoveToNextAttribute());
				this.reader.MoveToElement();
			}
		}

		private void ValidateEndStartElement()
		{
			if (this.context.ElementDecl.HasDefaultAttribute)
			{
				foreach (SchemaAttDef schemaAttDef in this.context.ElementDecl.DefaultAttDefs)
				{
					this.reader.AddDefaultAttribute(schemaAttDef);
					if (this.HasIdentityConstraints && !this.attPresence.Contains(schemaAttDef.Name))
					{
						this.AttributeIdentityConstraints(schemaAttDef.Name.Name, schemaAttDef.Name.Namespace, this.UnWrapUnion(schemaAttDef.DefaultValueTyped), schemaAttDef.DefaultValueRaw, schemaAttDef);
					}
				}
			}
			if (this.context.ElementDecl.HasRequiredAttribute)
			{
				try
				{
					this.context.ElementDecl.CheckAttributes(this.attPresence, this.reader.StandAlone);
				}
				catch (XmlSchemaException ex)
				{
					ex.SetSource(this.reader.BaseURI, base.PositionInfo.LineNumber, base.PositionInfo.LinePosition);
					base.SendValidationEvent(ex);
				}
			}
			if (this.context.ElementDecl.Datatype != null)
			{
				this.checkDatatype = true;
				this.hasSibling = false;
				this.textString = string.Empty;
				this.textValue.Length = 0;
			}
		}

		private void LoadSchemaFromLocation(string uri, string url)
		{
			XmlReader xmlReader = null;
			try
			{
				Uri uri2 = base.XmlResolver.ResolveUri(base.BaseUri, url);
				Stream stream = (Stream)base.XmlResolver.GetEntity(uri2, null, null);
				xmlReader = new XmlTextReader(uri2.ToString(), stream, base.NameTable);
				Parser parser = new Parser(SchemaType.XSD, base.NameTable, base.SchemaNames, base.EventHandler);
				parser.XmlResolver = base.XmlResolver;
				SchemaType schemaType = parser.Parse(xmlReader, uri);
				SchemaInfo schemaInfo = new SchemaInfo();
				schemaInfo.SchemaType = schemaType;
				if (schemaType == SchemaType.XSD)
				{
					if (base.SchemaCollection.EventHandler == null)
					{
						base.SchemaCollection.EventHandler = base.EventHandler;
					}
					base.SchemaCollection.Add(uri, schemaInfo, parser.XmlSchema, true);
				}
				base.SchemaInfo.Add(schemaInfo, base.EventHandler);
				while (xmlReader.Read())
				{
				}
			}
			catch (XmlSchemaException ex)
			{
				base.SendValidationEvent("Sch_CannotLoadSchema", new string[] { uri, ex.Message }, XmlSeverityType.Error);
			}
			catch (Exception ex2)
			{
				base.SendValidationEvent("Sch_CannotLoadSchema", new string[] { uri, ex2.Message }, XmlSeverityType.Warning);
			}
			finally
			{
				if (xmlReader != null)
				{
					xmlReader.Close();
				}
			}
		}

		private void LoadSchema(string uri, string url)
		{
			if (base.XmlResolver == null)
			{
				return;
			}
			if (base.SchemaInfo.TargetNamespaces.Contains(uri) && this.nsManager.LookupPrefix(uri) != null)
			{
				return;
			}
			SchemaInfo schemaInfo = null;
			if (base.SchemaCollection != null)
			{
				schemaInfo = base.SchemaCollection.GetSchemaInfo(uri);
			}
			if (schemaInfo == null)
			{
				if (url != null)
				{
					this.LoadSchemaFromLocation(uri, url);
				}
				return;
			}
			if (schemaInfo.SchemaType != SchemaType.XSD)
			{
				throw new XmlException("Xml_MultipleValidaitonTypes", string.Empty, base.PositionInfo.LineNumber, base.PositionInfo.LinePosition);
			}
			base.SchemaInfo.Add(schemaInfo, base.EventHandler);
		}

		private bool HasSchema
		{
			get
			{
				return this.schemaInfo.SchemaType != SchemaType.None;
			}
		}

		public override bool PreserveWhitespace
		{
			get
			{
				return this.context.ElementDecl != null && this.context.ElementDecl.ContentValidator.PreserveWhitespace;
			}
		}

		private void ProcessTokenizedType(XmlTokenizedType ttype, string name)
		{
			switch (ttype)
			{
			case XmlTokenizedType.ID:
				if (this.FindId(name) != null)
				{
					base.SendValidationEvent("Sch_DupId", name);
					return;
				}
				this.AddID(name, this.context.LocalName);
				return;
			case XmlTokenizedType.IDREF:
				if (this.FindId(name) == null)
				{
					this.idRefListHead = new IdRefNode(this.idRefListHead, name, base.PositionInfo.LineNumber, base.PositionInfo.LinePosition);
					return;
				}
				break;
			case XmlTokenizedType.IDREFS:
				break;
			case XmlTokenizedType.ENTITY:
				BaseValidator.ProcessEntity(this.schemaInfo, name, this, base.EventHandler, this.reader.BaseURI, base.PositionInfo.LineNumber, base.PositionInfo.LinePosition);
				break;
			default:
				return;
			}
		}

		private void CheckValue(string value, SchemaAttDef attdef)
		{
			try
			{
				this.reader.TypedValueObject = null;
				bool flag = attdef != null;
				XmlSchemaDatatype xmlSchemaDatatype = (flag ? attdef.Datatype : this.context.ElementDecl.Datatype);
				if (xmlSchemaDatatype != null)
				{
					object obj = xmlSchemaDatatype.ParseValue(value, base.NameTable, this.nsManager, true);
					XmlTokenizedType tokenizedType = xmlSchemaDatatype.TokenizedType;
					if (tokenizedType == XmlTokenizedType.ENTITY || tokenizedType == XmlTokenizedType.ID || tokenizedType == XmlTokenizedType.IDREF)
					{
						if (xmlSchemaDatatype.Variety == XmlSchemaDatatypeVariety.List)
						{
							string[] array = (string[])obj;
							foreach (string text in array)
							{
								this.ProcessTokenizedType(xmlSchemaDatatype.TokenizedType, text);
							}
						}
						else
						{
							this.ProcessTokenizedType(xmlSchemaDatatype.TokenizedType, (string)obj);
						}
					}
					SchemaDeclBase schemaDeclBase = (flag ? attdef : this.context.ElementDecl);
					if (!schemaDeclBase.CheckValue(obj))
					{
						if (flag)
						{
							base.SendValidationEvent("Sch_FixedAttributeValue", attdef.Name.ToString());
						}
						else
						{
							base.SendValidationEvent("Sch_FixedElementValue", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
						}
					}
					if (xmlSchemaDatatype.Variety == XmlSchemaDatatypeVariety.Union)
					{
						obj = this.UnWrapUnion(obj);
					}
					this.reader.TypedValueObject = obj;
				}
			}
			catch (XmlSchemaException)
			{
				if (attdef != null)
				{
					base.SendValidationEvent("Sch_AttributeValueDataType", attdef.Name.ToString());
				}
				else
				{
					base.SendValidationEvent("Sch_ElementValueDataType", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
				}
			}
		}

		internal void AddID(string name, object node)
		{
			if (this.IDs == null)
			{
				this.IDs = new Hashtable();
			}
			this.IDs.Add(name, node);
		}

		public override object FindId(string name)
		{
			if (this.IDs != null)
			{
				return this.IDs[name];
			}
			return null;
		}

		public bool IsXSDRoot(string localName, string ns)
		{
			return Ref.Equal(ns, this.NsXs) && Ref.Equal(localName, this.XsdSchema);
		}

		private void Push(XmlQualifiedName elementName)
		{
			this.context = (ValidationState)this.validationStack.Push();
			if (this.context == null)
			{
				this.context = new ValidationState();
				this.validationStack.AddToTop(this.context);
			}
			this.context.LocalName = elementName.Name;
			this.context.Namespace = elementName.Namespace;
			this.context.HasMatched = false;
			this.context.IsNill = false;
			this.context.ProcessContents = this.processContents;
			this.context.NeedValidateChildren = false;
			this.context.Constr = null;
		}

		private void Pop()
		{
			if (this.validationStack.Length > 1)
			{
				this.validationStack.Pop();
				if (this.startIDConstraint == this.validationStack.Length)
				{
					this.startIDConstraint = -1;
				}
				this.context = (ValidationState)this.validationStack.Peek();
				this.processContents = this.context.ProcessContents;
			}
		}

		private void CheckForwardRefs()
		{
			IdRefNode next;
			for (IdRefNode idRefNode = this.idRefListHead; idRefNode != null; idRefNode = next)
			{
				if (this.FindId(idRefNode.Id) == null)
				{
					base.SendValidationEvent(new XmlSchemaException("Sch_UndeclaredId", idRefNode.Id, this.reader.BaseURI, idRefNode.LineNo, idRefNode.LinePos));
				}
				next = idRefNode.Next;
				idRefNode.Next = null;
			}
			this.idRefListHead = null;
		}

		private void ValidateStartElementIdentityConstraints()
		{
			if (this.context.ElementDecl != null)
			{
				if (this.context.ElementDecl.Constraints != null)
				{
					this.AddIdentityConstraints();
				}
				if (this.HasIdentityConstraints)
				{
					this.ElementIdentityConstraints();
				}
			}
		}

		private bool HasIdentityConstraints
		{
			get
			{
				return this.startIDConstraint != -1;
			}
		}

		private void AddIdentityConstraints()
		{
			this.context.Constr = new ConstraintStruct[this.context.ElementDecl.Constraints.Length];
			int num = 0;
			foreach (CompiledIdentityConstraint compiledIdentityConstraint in this.context.ElementDecl.Constraints)
			{
				this.context.Constr[num++] = new ConstraintStruct(compiledIdentityConstraint);
			}
			foreach (ConstraintStruct constraintStruct in this.context.Constr)
			{
				if (constraintStruct.constraint.Role == CompiledIdentityConstraint.ConstraintRole.Keyref)
				{
					bool flag = false;
					for (int k = this.validationStack.Length - 1; k >= ((this.startIDConstraint >= 0) ? this.startIDConstraint : (this.validationStack.Length - 1)); k--)
					{
						if (((ValidationState)this.validationStack[k]).Constr != null)
						{
							foreach (ConstraintStruct constraintStruct2 in ((ValidationState)this.validationStack[k]).Constr)
							{
								if (constraintStruct2.constraint.name == constraintStruct.constraint.refer)
								{
									flag = true;
									if (constraintStruct2.keyrefTable == null)
									{
										constraintStruct2.keyrefTable = new Hashtable();
									}
									constraintStruct.qualifiedTable = constraintStruct2.keyrefTable;
									break;
								}
							}
							if (flag)
							{
								break;
							}
						}
					}
					if (!flag)
					{
						base.SendValidationEvent("Sch_RefNotInScope", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
					}
				}
			}
			if (this.startIDConstraint == -1)
			{
				this.startIDConstraint = this.validationStack.Length - 1;
			}
		}

		private void ElementIdentityConstraints()
		{
			for (int i = this.startIDConstraint; i < this.validationStack.Length; i++)
			{
				if (((ValidationState)this.validationStack[i]).Constr != null)
				{
					foreach (ConstraintStruct constraintStruct in ((ValidationState)this.validationStack[i]).Constr)
					{
						if (constraintStruct.axisSelector.MoveToStartElement(this.reader.LocalName, this.reader.NamespaceURI))
						{
							constraintStruct.axisSelector.PushKS(base.PositionInfo.LineNumber, base.PositionInfo.LinePosition);
						}
						foreach (object obj in constraintStruct.axisFields)
						{
							LocatedActiveAxis locatedActiveAxis = (LocatedActiveAxis)obj;
							if (locatedActiveAxis.MoveToStartElement(this.reader.LocalName, this.reader.NamespaceURI) && this.context.ElementDecl != null)
							{
								if (this.context.ElementDecl.Datatype == null)
								{
									base.SendValidationEvent("Sch_FieldSimpleTypeExpected", this.reader.LocalName);
								}
								else
								{
									locatedActiveAxis.isMatched = true;
								}
							}
						}
					}
				}
			}
		}

		private void AttributeIdentityConstraints(string name, string ns, object obj, string sobj, SchemaAttDef attdef)
		{
			for (int i = this.startIDConstraint; i < this.validationStack.Length; i++)
			{
				if (((ValidationState)this.validationStack[i]).Constr != null)
				{
					foreach (ConstraintStruct constraintStruct in ((ValidationState)this.validationStack[i]).Constr)
					{
						foreach (object obj2 in constraintStruct.axisFields)
						{
							LocatedActiveAxis locatedActiveAxis = (LocatedActiveAxis)obj2;
							if (locatedActiveAxis.MoveToAttribute(name, ns))
							{
								if (locatedActiveAxis.Ks[locatedActiveAxis.Column] != null)
								{
									base.SendValidationEvent("Sch_FieldSingleValueExpected", name);
								}
								else if (attdef != null && attdef.Datatype != null)
								{
									locatedActiveAxis.Ks[locatedActiveAxis.Column] = new TypedObject(obj, sobj, attdef.Datatype);
								}
							}
						}
					}
				}
			}
		}

		private object UnWrapUnion(object typedValue)
		{
			XsdSimpleValue xsdSimpleValue = typedValue as XsdSimpleValue;
			if (xsdSimpleValue != null)
			{
				typedValue = xsdSimpleValue.TypedValue;
			}
			return typedValue;
		}

		private void EndElementIdentityConstraints()
		{
			for (int i = this.validationStack.Length - 1; i >= this.startIDConstraint; i--)
			{
				if (((ValidationState)this.validationStack[i]).Constr != null)
				{
					foreach (ConstraintStruct constraintStruct in ((ValidationState)this.validationStack[i]).Constr)
					{
						foreach (object obj in constraintStruct.axisFields)
						{
							LocatedActiveAxis locatedActiveAxis = (LocatedActiveAxis)obj;
							if (locatedActiveAxis.isMatched)
							{
								locatedActiveAxis.isMatched = false;
								if (locatedActiveAxis.Ks[locatedActiveAxis.Column] != null)
								{
									base.SendValidationEvent("Sch_FieldSingleValueExpected", this.reader.LocalName);
								}
								else
								{
									string text = ((!this.hasSibling) ? this.textString : this.textValue.ToString());
									if (this.reader.TypedValueObject != null && text.Length != 0)
									{
										locatedActiveAxis.Ks[locatedActiveAxis.Column] = new TypedObject(this.reader.TypedValueObject, text, this.context.ElementDecl.Datatype);
									}
								}
							}
							locatedActiveAxis.EndElement(this.reader.LocalName, this.reader.NamespaceURI);
						}
						if (constraintStruct.axisSelector.EndElement(this.reader.LocalName, this.reader.NamespaceURI))
						{
							KeySequence keySequence = constraintStruct.axisSelector.PopKS();
							switch (constraintStruct.constraint.Role)
							{
							case CompiledIdentityConstraint.ConstraintRole.Unique:
								if (keySequence.IsQualified())
								{
									if (constraintStruct.qualifiedTable.Contains(keySequence))
									{
										base.SendValidationEvent(new XmlSchemaException("Sch_DuplicateKey", new string[]
										{
											keySequence.ToString(),
											constraintStruct.constraint.name.ToString()
										}, this.reader.BaseURI, keySequence.PosLine, keySequence.PosCol));
									}
									else
									{
										constraintStruct.qualifiedTable.Add(keySequence, keySequence);
									}
								}
								break;
							case CompiledIdentityConstraint.ConstraintRole.Key:
								if (!keySequence.IsQualified())
								{
									base.SendValidationEvent(new XmlSchemaException("Sch_MissingKey", constraintStruct.constraint.name.ToString(), this.reader.BaseURI, keySequence.PosLine, keySequence.PosCol));
								}
								else if (constraintStruct.qualifiedTable.Contains(keySequence))
								{
									base.SendValidationEvent(new XmlSchemaException("Sch_DuplicateKey", new string[]
									{
										keySequence.ToString(),
										constraintStruct.constraint.name.ToString()
									}, this.reader.BaseURI, keySequence.PosLine, keySequence.PosCol));
								}
								else
								{
									constraintStruct.qualifiedTable.Add(keySequence, keySequence);
								}
								break;
							case CompiledIdentityConstraint.ConstraintRole.Keyref:
								if (constraintStruct.qualifiedTable != null && keySequence.IsQualified() && !constraintStruct.qualifiedTable.Contains(keySequence))
								{
									constraintStruct.qualifiedTable.Add(keySequence, keySequence);
								}
								break;
							}
						}
					}
				}
			}
			ConstraintStruct[] constr2 = ((ValidationState)this.validationStack[this.validationStack.Length - 1]).Constr;
			if (constr2 != null)
			{
				foreach (ConstraintStruct constraintStruct2 in constr2)
				{
					if (constraintStruct2.constraint.Role != CompiledIdentityConstraint.ConstraintRole.Keyref && constraintStruct2.keyrefTable != null)
					{
						foreach (object obj2 in constraintStruct2.keyrefTable.Keys)
						{
							KeySequence keySequence2 = (KeySequence)obj2;
							if (!constraintStruct2.qualifiedTable.Contains(keySequence2))
							{
								base.SendValidationEvent(new XmlSchemaException("Sch_UnresolvedKeyref", keySequence2.ToString(), this.reader.BaseURI, keySequence2.PosLine, keySequence2.PosCol));
							}
						}
					}
				}
			}
		}

		private const int STACK_INCREMENT = 10;

		private int startIDConstraint = -1;

		private HWStack validationStack;

		private Hashtable attPresence;

		private XmlNamespaceManager nsManager;

		private bool bManageNamespaces;

		private Hashtable IDs;

		private IdRefNode idRefListHead;

		private Parser inlineSchemaParser;

		private XmlSchemaContentProcessing processContents;

		private static readonly XmlSchemaDatatype dtCDATA = XmlSchemaDatatype.FromXmlTokenizedType(XmlTokenizedType.CDATA);

		private static readonly XmlSchemaDatatype dtQName = XmlSchemaDatatype.FromXmlTokenizedTypeXsd(XmlTokenizedType.QName);

		private static readonly XmlSchemaDatatype dtStringArray = XsdValidator.dtCDATA.DeriveByList(null);

		private string NsXmlNs;

		private string NsXs;

		private string NsXsi;

		private string XsiType;

		private string XsiNil;

		private string XsiSchemaLocation;

		private string XsiNoNamespaceSchemaLocation;

		private string XsdSchema;
	}
}
