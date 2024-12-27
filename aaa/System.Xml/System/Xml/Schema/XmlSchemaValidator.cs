using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.XmlConfiguration;

namespace System.Xml.Schema
{
	// Token: 0x02000285 RID: 645
	public sealed class XmlSchemaValidator
	{
		// Token: 0x06001D79 RID: 7545 RVA: 0x000860E0 File Offset: 0x000850E0
		public XmlSchemaValidator(XmlNameTable nameTable, XmlSchemaSet schemas, IXmlNamespaceResolver namespaceResolver, XmlSchemaValidationFlags validationFlags)
		{
			if (nameTable == null)
			{
				throw new ArgumentNullException("nameTable");
			}
			if (schemas == null)
			{
				throw new ArgumentNullException("schemas");
			}
			if (namespaceResolver == null)
			{
				throw new ArgumentNullException("namespaceResolver");
			}
			this.nameTable = nameTable;
			this.nsResolver = namespaceResolver;
			this.validationFlags = validationFlags;
			if ((validationFlags & XmlSchemaValidationFlags.ProcessInlineSchema) != XmlSchemaValidationFlags.None || (validationFlags & XmlSchemaValidationFlags.ProcessSchemaLocation) != XmlSchemaValidationFlags.None)
			{
				this.schemaSet = new XmlSchemaSet(nameTable);
				this.schemaSet.ValidationEventHandler += schemas.GetEventHandler();
				this.schemaSet.CompilationSettings = schemas.CompilationSettings;
				this.schemaSet.XmlResolver = schemas.GetResolver();
				this.schemaSet.Add(schemas);
				this.validatedNamespaces = new Hashtable();
			}
			else
			{
				this.schemaSet = schemas;
			}
			this.Init();
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x000861C0 File Offset: 0x000851C0
		private void Init()
		{
			this.validationStack = new HWStack(10);
			this.attPresence = new Hashtable();
			this.Push(XmlQualifiedName.Empty);
			this.dummyPositionInfo = new PositionInfo();
			this.positionInfo = this.dummyPositionInfo;
			this.validationEventSender = this;
			this.currentState = ValidatorState.None;
			this.textValue = new StringBuilder(100);
			this.xmlResolver = XmlReaderSection.CreateDefaultResolver();
			this.contextQName = new XmlQualifiedName();
			this.Reset();
			this.RecompileSchemaSet();
			this.NsXs = this.nameTable.Add("http://www.w3.org/2001/XMLSchema");
			this.NsXsi = this.nameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
			this.NsXmlNs = this.nameTable.Add("http://www.w3.org/2000/xmlns/");
			this.NsXml = this.nameTable.Add("http://www.w3.org/XML/1998/namespace");
			this.xsiTypeString = this.nameTable.Add("type");
			this.xsiNilString = this.nameTable.Add("nil");
			this.xsiSchemaLocationString = this.nameTable.Add("schemaLocation");
			this.xsiNoNamespaceSchemaLocationString = this.nameTable.Add("noNamespaceSchemaLocation");
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x000862F4 File Offset: 0x000852F4
		private void Reset()
		{
			this.isRoot = true;
			this.rootHasSchema = true;
			while (this.validationStack.Length > 1)
			{
				this.validationStack.Pop();
			}
			this.startIDConstraint = -1;
			this.partialValidationType = null;
			if (this.IDs != null)
			{
				this.IDs.Clear();
			}
			if (this.ProcessSchemaHints)
			{
				this.validatedNamespaces.Clear();
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (set) Token: 0x06001D7C RID: 7548 RVA: 0x0008635F File Offset: 0x0008535F
		public XmlResolver XmlResolver
		{
			set
			{
				this.xmlResolver = value;
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06001D7D RID: 7549 RVA: 0x00086368 File Offset: 0x00085368
		// (set) Token: 0x06001D7E RID: 7550 RVA: 0x00086370 File Offset: 0x00085370
		public IXmlLineInfo LineInfoProvider
		{
			get
			{
				return this.positionInfo;
			}
			set
			{
				if (value == null)
				{
					this.positionInfo = this.dummyPositionInfo;
					return;
				}
				this.positionInfo = value;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06001D7F RID: 7551 RVA: 0x00086389 File Offset: 0x00085389
		// (set) Token: 0x06001D80 RID: 7552 RVA: 0x00086391 File Offset: 0x00085391
		public Uri SourceUri
		{
			get
			{
				return this.sourceUri;
			}
			set
			{
				this.sourceUri = value;
				this.sourceUriString = this.sourceUri.ToString();
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06001D81 RID: 7553 RVA: 0x000863AB File Offset: 0x000853AB
		// (set) Token: 0x06001D82 RID: 7554 RVA: 0x000863B3 File Offset: 0x000853B3
		public object ValidationEventSender
		{
			get
			{
				return this.validationEventSender;
			}
			set
			{
				this.validationEventSender = value;
			}
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06001D83 RID: 7555 RVA: 0x000863BC File Offset: 0x000853BC
		// (remove) Token: 0x06001D84 RID: 7556 RVA: 0x000863D5 File Offset: 0x000853D5
		public event ValidationEventHandler ValidationEventHandler
		{
			add
			{
				this.eventHandler = (ValidationEventHandler)Delegate.Combine(this.eventHandler, value);
			}
			remove
			{
				this.eventHandler = (ValidationEventHandler)Delegate.Remove(this.eventHandler, value);
			}
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x000863F0 File Offset: 0x000853F0
		public void AddSchema(XmlSchema schema)
		{
			if (schema == null)
			{
				throw new ArgumentNullException("schema");
			}
			if ((this.validationFlags & XmlSchemaValidationFlags.ProcessInlineSchema) == XmlSchemaValidationFlags.None)
			{
				return;
			}
			string text = schema.TargetNamespace;
			if (text == null)
			{
				text = string.Empty;
			}
			Hashtable schemaLocations = this.schemaSet.SchemaLocations;
			DictionaryEntry[] array = new DictionaryEntry[schemaLocations.Count];
			schemaLocations.CopyTo(array, 0);
			if (this.validatedNamespaces[text] != null && this.schemaSet.FindSchemaByNSAndUrl(schema.BaseUri, text, array) == null)
			{
				this.SendValidationEvent("Sch_ComponentAlreadySeenForNS", text, XmlSeverityType.Error);
			}
			if (schema.ErrorCount == 0)
			{
				try
				{
					this.schemaSet.Add(schema);
					this.RecompileSchemaSet();
				}
				catch (XmlSchemaException ex)
				{
					this.SendValidationEvent("Sch_CannotLoadSchema", new string[]
					{
						schema.BaseUri.ToString(),
						ex.Message
					}, ex);
				}
				foreach (object obj in schema.ImportedSchemas)
				{
					XmlSchema xmlSchema = (XmlSchema)obj;
					text = xmlSchema.TargetNamespace;
					if (text == null)
					{
						text = string.Empty;
					}
					if (this.validatedNamespaces[text] != null && this.schemaSet.FindSchemaByNSAndUrl(xmlSchema.BaseUri, text, array) == null)
					{
						this.SendValidationEvent("Sch_ComponentAlreadySeenForNS", text, XmlSeverityType.Error);
						this.schemaSet.RemoveRecursive(schema);
						break;
					}
				}
			}
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x00086574 File Offset: 0x00085574
		public void Initialize()
		{
			if (this.currentState != ValidatorState.None && this.currentState != ValidatorState.Finish)
			{
				throw new InvalidOperationException(Res.GetString("Sch_InvalidStateTransition", new string[]
				{
					XmlSchemaValidator.MethodNames[(int)this.currentState],
					XmlSchemaValidator.MethodNames[1]
				}));
			}
			this.currentState = ValidatorState.Start;
			this.Reset();
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x000865D4 File Offset: 0x000855D4
		public void Initialize(XmlSchemaObject partialValidationType)
		{
			if (this.currentState != ValidatorState.None && this.currentState != ValidatorState.Finish)
			{
				throw new InvalidOperationException(Res.GetString("Sch_InvalidStateTransition", new string[]
				{
					XmlSchemaValidator.MethodNames[(int)this.currentState],
					XmlSchemaValidator.MethodNames[1]
				}));
			}
			if (partialValidationType == null)
			{
				throw new ArgumentNullException("partialValidationType");
			}
			if (!(partialValidationType is XmlSchemaElement) && !(partialValidationType is XmlSchemaAttribute) && !(partialValidationType is XmlSchemaType))
			{
				throw new ArgumentException(Res.GetString("Sch_InvalidPartialValidationType"));
			}
			this.currentState = ValidatorState.Start;
			this.Reset();
			this.partialValidationType = partialValidationType;
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x0008666E File Offset: 0x0008566E
		public void ValidateElement(string localName, string namespaceUri, XmlSchemaInfo schemaInfo)
		{
			this.ValidateElement(localName, namespaceUri, schemaInfo, null, null, null, null);
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x00086680 File Offset: 0x00085680
		public void ValidateElement(string localName, string namespaceUri, XmlSchemaInfo schemaInfo, string xsiType, string xsiNil, string xsiSchemaLocation, string xsiNoNamespaceSchemaLocation)
		{
			if (localName == null)
			{
				throw new ArgumentNullException("localName");
			}
			if (namespaceUri == null)
			{
				throw new ArgumentNullException("namespaceUri");
			}
			this.CheckStateTransition(ValidatorState.Element, XmlSchemaValidator.MethodNames[4]);
			this.ClearPSVI();
			this.contextQName.Init(localName, namespaceUri);
			XmlQualifiedName xmlQualifiedName = this.contextQName;
			bool flag;
			object obj = this.ValidateElementContext(xmlQualifiedName, out flag);
			SchemaElementDecl schemaElementDecl = this.FastGetElementDecl(xmlQualifiedName, obj);
			this.Push(xmlQualifiedName);
			if (flag)
			{
				this.context.Validity = XmlSchemaValidity.Invalid;
			}
			if ((this.validationFlags & XmlSchemaValidationFlags.ProcessSchemaLocation) != XmlSchemaValidationFlags.None && this.xmlResolver != null)
			{
				this.ProcessSchemaLocations(xsiSchemaLocation, xsiNoNamespaceSchemaLocation);
			}
			if (this.processContents != XmlSchemaContentProcessing.Skip)
			{
				if (schemaElementDecl == null && this.partialValidationType == null)
				{
					schemaElementDecl = this.compiledSchemaInfo.GetElementDecl(xmlQualifiedName);
				}
				bool flag2 = schemaElementDecl != null;
				if (xsiType != null || xsiNil != null)
				{
					schemaElementDecl = this.CheckXsiTypeAndNil(schemaElementDecl, xsiType, xsiNil, ref flag2);
				}
				if (schemaElementDecl == null)
				{
					this.ThrowDeclNotFoundWarningOrError(flag2);
				}
			}
			this.context.ElementDecl = schemaElementDecl;
			XmlSchemaElement xmlSchemaElement = null;
			XmlSchemaType xmlSchemaType = null;
			if (schemaElementDecl != null)
			{
				this.CheckElementProperties();
				this.attPresence.Clear();
				this.context.NeedValidateChildren = this.processContents != XmlSchemaContentProcessing.Skip;
				this.ValidateStartElementIdentityConstraints();
				schemaElementDecl.ContentValidator.InitValidation(this.context);
				xmlSchemaType = schemaElementDecl.SchemaType;
				xmlSchemaElement = this.GetSchemaElement();
			}
			if (schemaInfo != null)
			{
				schemaInfo.SchemaType = xmlSchemaType;
				schemaInfo.SchemaElement = xmlSchemaElement;
				schemaInfo.IsNil = this.context.IsNill;
				schemaInfo.Validity = this.context.Validity;
			}
			if (this.ProcessSchemaHints && this.validatedNamespaces[namespaceUri] == null)
			{
				this.validatedNamespaces.Add(namespaceUri, namespaceUri);
			}
			if (this.isRoot)
			{
				this.isRoot = false;
			}
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x0008682C File Offset: 0x0008582C
		public object ValidateAttribute(string localName, string namespaceUri, string attributeValue, XmlSchemaInfo schemaInfo)
		{
			if (attributeValue == null)
			{
				throw new ArgumentNullException("attributeValue");
			}
			return this.ValidateAttribute(localName, namespaceUri, null, attributeValue, schemaInfo);
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x00086848 File Offset: 0x00085848
		public object ValidateAttribute(string localName, string namespaceUri, XmlValueGetter attributeValue, XmlSchemaInfo schemaInfo)
		{
			if (attributeValue == null)
			{
				throw new ArgumentNullException("attributeValue");
			}
			return this.ValidateAttribute(localName, namespaceUri, attributeValue, null, schemaInfo);
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x00086864 File Offset: 0x00085864
		private object ValidateAttribute(string lName, string ns, XmlValueGetter attributeValueGetter, string attributeStringValue, XmlSchemaInfo schemaInfo)
		{
			if (lName == null)
			{
				throw new ArgumentNullException("localName");
			}
			if (ns == null)
			{
				throw new ArgumentNullException("namespaceUri");
			}
			ValidatorState validatorState = ((this.validationStack.Length > 1) ? ValidatorState.Attribute : ValidatorState.TopLevelAttribute);
			this.CheckStateTransition(validatorState, XmlSchemaValidator.MethodNames[(int)validatorState]);
			object obj = null;
			this.attrValid = true;
			XmlSchemaValidity xmlSchemaValidity = XmlSchemaValidity.NotKnown;
			XmlSchemaAttribute xmlSchemaAttribute = null;
			XmlSchemaSimpleType xmlSchemaSimpleType = null;
			ns = this.nameTable.Add(ns);
			if (Ref.Equal(ns, this.NsXmlNs))
			{
				return null;
			}
			SchemaAttDef schemaAttDef = null;
			SchemaElementDecl elementDecl = this.context.ElementDecl;
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(lName, ns);
			if (this.attPresence[xmlQualifiedName] != null)
			{
				this.SendValidationEvent("Sch_DuplicateAttribute", xmlQualifiedName.ToString());
				if (schemaInfo != null)
				{
					schemaInfo.Clear();
				}
				return null;
			}
			if (!Ref.Equal(ns, this.NsXsi))
			{
				XmlSchemaObject xmlSchemaObject = ((this.currentState == ValidatorState.TopLevelAttribute) ? this.partialValidationType : null);
				AttributeMatchState attributeMatchState;
				schemaAttDef = this.compiledSchemaInfo.GetAttributeXsd(elementDecl, xmlQualifiedName, xmlSchemaObject, out attributeMatchState);
				switch (attributeMatchState)
				{
				case AttributeMatchState.AttributeFound:
					break;
				case AttributeMatchState.AnyIdAttributeFound:
				{
					if (this.wildID != null)
					{
						this.SendValidationEvent("Sch_MoreThanOneWildId", string.Empty);
						goto IL_0409;
					}
					this.wildID = schemaAttDef;
					XmlSchemaComplexType xmlSchemaComplexType = elementDecl.SchemaType as XmlSchemaComplexType;
					if (xmlSchemaComplexType.ContainsIdAttribute(false))
					{
						this.SendValidationEvent("Sch_AttrUseAndWildId", string.Empty);
						goto IL_0409;
					}
					break;
				}
				case AttributeMatchState.UndeclaredElementAndAttribute:
					if ((schemaAttDef = this.CheckIsXmlAttribute(xmlQualifiedName)) == null)
					{
						if (elementDecl == null && this.processContents == XmlSchemaContentProcessing.Strict && xmlQualifiedName.Namespace.Length != 0 && this.compiledSchemaInfo.Contains(xmlQualifiedName.Namespace))
						{
							this.attrValid = false;
							this.SendValidationEvent("Sch_UndeclaredAttribute", xmlQualifiedName.ToString());
							goto IL_0409;
						}
						if (this.processContents != XmlSchemaContentProcessing.Skip)
						{
							this.SendValidationEvent("Sch_NoAttributeSchemaFound", xmlQualifiedName.ToString(), XmlSeverityType.Warning);
							goto IL_0409;
						}
						goto IL_0409;
					}
					break;
				case AttributeMatchState.UndeclaredAttribute:
					if ((schemaAttDef = this.CheckIsXmlAttribute(xmlQualifiedName)) == null)
					{
						this.attrValid = false;
						this.SendValidationEvent("Sch_UndeclaredAttribute", xmlQualifiedName.ToString());
						goto IL_0409;
					}
					break;
				case AttributeMatchState.AnyAttributeLax:
					this.SendValidationEvent("Sch_NoAttributeSchemaFound", xmlQualifiedName.ToString(), XmlSeverityType.Warning);
					goto IL_0409;
				case AttributeMatchState.AnyAttributeSkip:
					goto IL_0409;
				case AttributeMatchState.ProhibitedAnyAttribute:
					if ((schemaAttDef = this.CheckIsXmlAttribute(xmlQualifiedName)) == null)
					{
						this.attrValid = false;
						this.SendValidationEvent("Sch_ProhibitedAttribute", xmlQualifiedName.ToString());
						goto IL_0409;
					}
					break;
				case AttributeMatchState.ProhibitedAttribute:
					this.attrValid = false;
					this.SendValidationEvent("Sch_ProhibitedAttribute", xmlQualifiedName.ToString());
					goto IL_0409;
				case AttributeMatchState.AttributeNameMismatch:
					this.attrValid = false;
					this.SendValidationEvent("Sch_SchemaAttributeNameMismatch", new string[]
					{
						xmlQualifiedName.ToString(),
						((XmlSchemaAttribute)xmlSchemaObject).QualifiedName.ToString()
					});
					goto IL_0409;
				case AttributeMatchState.ValidateAttributeInvalidCall:
					this.currentState = ValidatorState.Start;
					this.attrValid = false;
					this.SendValidationEvent("Sch_ValidateAttributeInvalidCall", string.Empty);
					goto IL_0409;
				default:
					goto IL_0409;
				}
				xmlSchemaAttribute = schemaAttDef.SchemaAttribute;
				if (elementDecl != null)
				{
					this.attPresence.Add(xmlQualifiedName, schemaAttDef);
				}
				object obj2;
				if (attributeValueGetter != null)
				{
					obj2 = attributeValueGetter();
				}
				else
				{
					obj2 = attributeStringValue;
				}
				obj = this.CheckAttributeValue(obj2, schemaAttDef);
				XmlSchemaDatatype xmlSchemaDatatype = schemaAttDef.Datatype;
				if (xmlSchemaDatatype.Variety == XmlSchemaDatatypeVariety.Union && obj != null)
				{
					XsdSimpleValue xsdSimpleValue = obj as XsdSimpleValue;
					xmlSchemaSimpleType = xsdSimpleValue.XmlType;
					xmlSchemaDatatype = xsdSimpleValue.XmlType.Datatype;
					obj = xsdSimpleValue.TypedValue;
				}
				this.CheckTokenizedTypes(xmlSchemaDatatype, obj, true);
				if (this.HasIdentityConstraints)
				{
					this.AttributeIdentityConstraints(xmlQualifiedName.Name, xmlQualifiedName.Namespace, obj, obj2.ToString(), xmlSchemaDatatype);
				}
			}
			else
			{
				lName = this.nameTable.Add(lName);
				if (Ref.Equal(lName, this.xsiTypeString) || Ref.Equal(lName, this.xsiNilString) || Ref.Equal(lName, this.xsiSchemaLocationString) || Ref.Equal(lName, this.xsiNoNamespaceSchemaLocationString))
				{
					this.attPresence.Add(xmlQualifiedName, SchemaAttDef.Empty);
				}
				else
				{
					this.attrValid = false;
					this.SendValidationEvent("Sch_NotXsiAttribute", xmlQualifiedName.ToString());
				}
			}
			IL_0409:
			if (!this.attrValid)
			{
				xmlSchemaValidity = XmlSchemaValidity.Invalid;
			}
			else if (schemaAttDef != null)
			{
				xmlSchemaValidity = XmlSchemaValidity.Valid;
			}
			if (schemaInfo != null)
			{
				schemaInfo.SchemaAttribute = xmlSchemaAttribute;
				schemaInfo.SchemaType = ((xmlSchemaAttribute == null) ? null : xmlSchemaAttribute.AttributeSchemaType);
				schemaInfo.MemberType = xmlSchemaSimpleType;
				schemaInfo.IsDefault = false;
				schemaInfo.Validity = xmlSchemaValidity;
			}
			if (this.ProcessSchemaHints && this.validatedNamespaces[ns] == null)
			{
				this.validatedNamespaces.Add(ns, ns);
			}
			return obj;
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x00086CE8 File Offset: 0x00085CE8
		public void GetUnspecifiedDefaultAttributes(ArrayList defaultAttributes)
		{
			if (defaultAttributes == null)
			{
				throw new ArgumentNullException("defaultAttributes");
			}
			this.CheckStateTransition(ValidatorState.Attribute, "GetUnspecifiedDefaultAttributes");
			this.GetUnspecifiedDefaultAttributes(defaultAttributes, false);
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x00086D0C File Offset: 0x00085D0C
		public void ValidateEndOfAttributes(XmlSchemaInfo schemaInfo)
		{
			this.CheckStateTransition(ValidatorState.EndOfAttributes, XmlSchemaValidator.MethodNames[6]);
			SchemaElementDecl elementDecl = this.context.ElementDecl;
			if (elementDecl != null && elementDecl.HasRequiredAttribute)
			{
				this.context.CheckRequiredAttribute = false;
				this.CheckRequiredAttributes(elementDecl);
			}
			if (schemaInfo != null)
			{
				schemaInfo.Validity = this.context.Validity;
			}
		}

		// Token: 0x06001D8F RID: 7567 RVA: 0x00086D65 File Offset: 0x00085D65
		public void ValidateText(string elementValue)
		{
			if (elementValue == null)
			{
				throw new ArgumentNullException("elementValue");
			}
			this.ValidateText(elementValue, null);
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x00086D7D File Offset: 0x00085D7D
		public void ValidateText(XmlValueGetter elementValue)
		{
			if (elementValue == null)
			{
				throw new ArgumentNullException("elementValue");
			}
			this.ValidateText(null, elementValue);
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x00086D98 File Offset: 0x00085D98
		private void ValidateText(string elementStringValue, XmlValueGetter elementValueGetter)
		{
			ValidatorState validatorState = ((this.validationStack.Length > 1) ? ValidatorState.Text : ValidatorState.TopLevelTextOrWS);
			this.CheckStateTransition(validatorState, XmlSchemaValidator.MethodNames[(int)validatorState]);
			if (this.context.NeedValidateChildren)
			{
				if (this.context.IsNill)
				{
					this.SendValidationEvent("Sch_ContentInNill", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
					return;
				}
				switch (this.context.ElementDecl.ContentValidator.ContentType)
				{
				case XmlSchemaContentType.TextOnly:
					if (elementValueGetter != null)
					{
						this.SaveTextValue(elementValueGetter());
						return;
					}
					this.SaveTextValue(elementStringValue);
					return;
				case XmlSchemaContentType.Empty:
					this.SendValidationEvent("Sch_InvalidTextInEmpty", string.Empty);
					return;
				case XmlSchemaContentType.ElementOnly:
				{
					string text = ((elementValueGetter != null) ? elementValueGetter().ToString() : elementStringValue);
					if (this.xmlCharType.IsOnlyWhitespace(text))
					{
						return;
					}
					ArrayList arrayList = this.context.ElementDecl.ContentValidator.ExpectedParticles(this.context, false);
					if (arrayList == null || arrayList.Count == 0)
					{
						this.SendValidationEvent("Sch_InvalidTextInElement", XmlSchemaValidator.BuildElementName(this.context.LocalName, this.context.Namespace));
						return;
					}
					this.SendValidationEvent("Sch_InvalidTextInElementExpecting", new string[]
					{
						XmlSchemaValidator.BuildElementName(this.context.LocalName, this.context.Namespace),
						XmlSchemaValidator.PrintExpectedElements(arrayList, true)
					});
					return;
				}
				case XmlSchemaContentType.Mixed:
					if (this.context.ElementDecl.DefaultValueTyped != null)
					{
						if (elementValueGetter != null)
						{
							this.SaveTextValue(elementValueGetter());
							return;
						}
						this.SaveTextValue(elementStringValue);
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x00086F3D File Offset: 0x00085F3D
		public void ValidateWhitespace(string elementValue)
		{
			if (elementValue == null)
			{
				throw new ArgumentNullException("elementValue");
			}
			this.ValidateWhitespace(elementValue, null);
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x00086F55 File Offset: 0x00085F55
		public void ValidateWhitespace(XmlValueGetter elementValue)
		{
			if (elementValue == null)
			{
				throw new ArgumentNullException("elementValue");
			}
			this.ValidateWhitespace(null, elementValue);
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x00086F70 File Offset: 0x00085F70
		private void ValidateWhitespace(string elementStringValue, XmlValueGetter elementValueGetter)
		{
			ValidatorState validatorState = ((this.validationStack.Length > 1) ? ValidatorState.Whitespace : ValidatorState.TopLevelTextOrWS);
			this.CheckStateTransition(validatorState, XmlSchemaValidator.MethodNames[(int)validatorState]);
			if (this.context.NeedValidateChildren)
			{
				if (this.context.IsNill)
				{
					this.SendValidationEvent("Sch_ContentInNill", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
				}
				switch (this.context.ElementDecl.ContentValidator.ContentType)
				{
				case XmlSchemaContentType.TextOnly:
					if (elementValueGetter != null)
					{
						this.SaveTextValue(elementValueGetter());
						return;
					}
					this.SaveTextValue(elementStringValue);
					return;
				case XmlSchemaContentType.Empty:
					this.SendValidationEvent("Sch_InvalidWhitespaceInEmpty", string.Empty);
					return;
				case XmlSchemaContentType.ElementOnly:
					break;
				case XmlSchemaContentType.Mixed:
					if (this.context.ElementDecl.DefaultValueTyped != null)
					{
						if (elementValueGetter != null)
						{
							this.SaveTextValue(elementValueGetter());
							return;
						}
						this.SaveTextValue(elementStringValue);
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x00087062 File Offset: 0x00086062
		public object ValidateEndElement(XmlSchemaInfo schemaInfo)
		{
			return this.InternalValidateEndElement(schemaInfo, null);
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x0008706C File Offset: 0x0008606C
		public object ValidateEndElement(XmlSchemaInfo schemaInfo, object typedValue)
		{
			if (typedValue == null)
			{
				throw new ArgumentNullException("typedValue");
			}
			if (this.textValue.Length > 0)
			{
				throw new InvalidOperationException(Res.GetString("Sch_InvalidEndElementCall"));
			}
			return this.InternalValidateEndElement(schemaInfo, typedValue);
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x000870A4 File Offset: 0x000860A4
		public void SkipToEndElement(XmlSchemaInfo schemaInfo)
		{
			if (this.validationStack.Length <= 1)
			{
				throw new InvalidOperationException(Res.GetString("Sch_InvalidEndElementMultiple", new object[] { XmlSchemaValidator.MethodNames[10] }));
			}
			this.CheckStateTransition(ValidatorState.SkipToEndElement, XmlSchemaValidator.MethodNames[10]);
			if (schemaInfo != null)
			{
				SchemaElementDecl elementDecl = this.context.ElementDecl;
				if (elementDecl != null)
				{
					schemaInfo.SchemaType = elementDecl.SchemaType;
					schemaInfo.SchemaElement = this.GetSchemaElement();
				}
				else
				{
					schemaInfo.SchemaType = null;
					schemaInfo.SchemaElement = null;
				}
				schemaInfo.MemberType = null;
				schemaInfo.IsNil = this.context.IsNill;
				schemaInfo.IsDefault = this.context.IsDefault;
				schemaInfo.Validity = this.context.Validity;
			}
			this.context.ValidationSkipped = true;
			this.currentState = ValidatorState.SkipToEndElement;
			this.Pop();
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x00087180 File Offset: 0x00086180
		public void EndValidation()
		{
			if (this.validationStack.Length > 1)
			{
				throw new InvalidOperationException(Res.GetString("Sch_InvalidEndValidation"));
			}
			this.CheckStateTransition(ValidatorState.Finish, XmlSchemaValidator.MethodNames[11]);
			this.CheckForwardRefs();
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x000871B8 File Offset: 0x000861B8
		public XmlSchemaParticle[] GetExpectedParticles()
		{
			if (this.currentState != ValidatorState.Start && this.currentState != ValidatorState.TopLevelTextOrWS)
			{
				if (this.context.ElementDecl != null)
				{
					ArrayList arrayList = this.context.ElementDecl.ContentValidator.ExpectedParticles(this.context, false);
					if (arrayList != null)
					{
						return arrayList.ToArray(typeof(XmlSchemaParticle)) as XmlSchemaParticle[];
					}
				}
				return XmlSchemaValidator.EmptyParticleArray;
			}
			if (this.partialValidationType == null)
			{
				ICollection values = this.schemaSet.GlobalElements.Values;
				XmlSchemaParticle[] array = new XmlSchemaParticle[values.Count];
				values.CopyTo(array, 0);
				return array;
			}
			XmlSchemaElement xmlSchemaElement = this.partialValidationType as XmlSchemaElement;
			if (xmlSchemaElement != null)
			{
				return new XmlSchemaParticle[] { xmlSchemaElement };
			}
			return XmlSchemaValidator.EmptyParticleArray;
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x00087274 File Offset: 0x00086274
		public XmlSchemaAttribute[] GetExpectedAttributes()
		{
			if (this.currentState == ValidatorState.Element || this.currentState == ValidatorState.Attribute)
			{
				SchemaElementDecl elementDecl = this.context.ElementDecl;
				ArrayList arrayList = new ArrayList();
				if (elementDecl != null)
				{
					foreach (object obj in elementDecl.AttDefs.Values)
					{
						SchemaAttDef schemaAttDef = (SchemaAttDef)obj;
						if (this.attPresence[schemaAttDef.Name] == null)
						{
							arrayList.Add(schemaAttDef.SchemaAttribute);
						}
					}
				}
				if (this.nsResolver.LookupPrefix(this.NsXsi) != null)
				{
					this.AddXsiAttributes(arrayList);
				}
				return arrayList.ToArray(typeof(XmlSchemaAttribute)) as XmlSchemaAttribute[];
			}
			if (this.currentState == ValidatorState.Start && this.partialValidationType != null)
			{
				XmlSchemaAttribute xmlSchemaAttribute = this.partialValidationType as XmlSchemaAttribute;
				if (xmlSchemaAttribute != null)
				{
					return new XmlSchemaAttribute[] { xmlSchemaAttribute };
				}
			}
			return XmlSchemaValidator.EmptyAttributeArray;
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x00087384 File Offset: 0x00086384
		internal void GetUnspecifiedDefaultAttributes(ArrayList defaultAttributes, bool createNodeData)
		{
			this.currentState = ValidatorState.Attribute;
			SchemaElementDecl elementDecl = this.context.ElementDecl;
			if (elementDecl != null && elementDecl.HasDefaultAttribute)
			{
				foreach (SchemaAttDef schemaAttDef in elementDecl.DefaultAttDefs)
				{
					if (!this.attPresence.Contains(schemaAttDef.Name) && schemaAttDef.DefaultValueTyped != null)
					{
						string text = this.nameTable.Add(schemaAttDef.Name.Namespace);
						string text2 = string.Empty;
						if (text.Length > 0)
						{
							text2 = this.GetDefaultAttributePrefix(text);
							if (text2 == null || text2.Length == 0)
							{
								this.SendValidationEvent("Sch_DefaultAttributeNotApplied", new string[]
								{
									schemaAttDef.Name.ToString(),
									XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace)
								});
								goto IL_0240;
							}
						}
						XmlSchemaDatatype xmlSchemaDatatype = schemaAttDef.Datatype;
						if (createNodeData)
						{
							ValidatingReaderNodeData validatingReaderNodeData = new ValidatingReaderNodeData();
							validatingReaderNodeData.LocalName = this.nameTable.Add(schemaAttDef.Name.Name);
							validatingReaderNodeData.Namespace = text;
							validatingReaderNodeData.Prefix = this.nameTable.Add(text2);
							validatingReaderNodeData.NodeType = XmlNodeType.Attribute;
							AttributePSVIInfo attributePSVIInfo = new AttributePSVIInfo();
							XmlSchemaInfo attributeSchemaInfo = attributePSVIInfo.attributeSchemaInfo;
							if (schemaAttDef.Datatype.Variety == XmlSchemaDatatypeVariety.Union)
							{
								XsdSimpleValue xsdSimpleValue = schemaAttDef.DefaultValueTyped as XsdSimpleValue;
								attributeSchemaInfo.MemberType = xsdSimpleValue.XmlType;
								xmlSchemaDatatype = xsdSimpleValue.XmlType.Datatype;
								attributePSVIInfo.typedAttributeValue = xsdSimpleValue.TypedValue;
							}
							else
							{
								attributePSVIInfo.typedAttributeValue = schemaAttDef.DefaultValueTyped;
							}
							attributeSchemaInfo.IsDefault = true;
							attributeSchemaInfo.Validity = XmlSchemaValidity.Valid;
							attributeSchemaInfo.SchemaType = schemaAttDef.SchemaType;
							attributeSchemaInfo.SchemaAttribute = schemaAttDef.SchemaAttribute;
							validatingReaderNodeData.RawValue = attributeSchemaInfo.XmlType.ValueConverter.ToString(attributePSVIInfo.typedAttributeValue);
							validatingReaderNodeData.AttInfo = attributePSVIInfo;
							defaultAttributes.Add(validatingReaderNodeData);
						}
						else
						{
							defaultAttributes.Add(schemaAttDef.SchemaAttribute);
						}
						this.CheckTokenizedTypes(xmlSchemaDatatype, schemaAttDef.DefaultValueTyped, true);
						if (this.HasIdentityConstraints)
						{
							this.AttributeIdentityConstraints(schemaAttDef.Name.Name, schemaAttDef.Name.Namespace, schemaAttDef.DefaultValueTyped, schemaAttDef.DefaultValueRaw, xmlSchemaDatatype);
						}
					}
					IL_0240:;
				}
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06001D9C RID: 7580 RVA: 0x000875E2 File Offset: 0x000865E2
		internal XmlSchemaSet SchemaSet
		{
			get
			{
				return this.schemaSet;
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06001D9D RID: 7581 RVA: 0x000875EA File Offset: 0x000865EA
		internal XmlSchemaValidationFlags ValidationFlags
		{
			get
			{
				return this.validationFlags;
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06001D9E RID: 7582 RVA: 0x000875F2 File Offset: 0x000865F2
		internal XmlSchemaContentType CurrentContentType
		{
			get
			{
				if (this.context.ElementDecl == null)
				{
					return XmlSchemaContentType.Empty;
				}
				return this.context.ElementDecl.ContentValidator.ContentType;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06001D9F RID: 7583 RVA: 0x00087618 File Offset: 0x00086618
		internal XmlSchemaContentProcessing CurrentProcessContents
		{
			get
			{
				return this.processContents;
			}
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x00087620 File Offset: 0x00086620
		internal void SetDtdSchemaInfo(SchemaInfo dtdSchemaInfo)
		{
			this.dtdSchemaInfo = dtdSchemaInfo;
			this.checkEntity = true;
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06001DA1 RID: 7585 RVA: 0x00087630 File Offset: 0x00086630
		private bool StrictlyAssessed
		{
			get
			{
				return (this.processContents == XmlSchemaContentProcessing.Strict || this.processContents == XmlSchemaContentProcessing.Lax) && this.context.ElementDecl != null && !this.context.ValidationSkipped;
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06001DA2 RID: 7586 RVA: 0x00087661 File Offset: 0x00086661
		private bool HasSchema
		{
			get
			{
				if (this.isRoot)
				{
					this.isRoot = false;
					if (!this.compiledSchemaInfo.Contains(this.context.Namespace))
					{
						this.rootHasSchema = false;
					}
				}
				return this.rootHasSchema;
			}
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x00087697 File Offset: 0x00086697
		internal string GetConcatenatedValue()
		{
			return this.textValue.ToString();
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x000876A4 File Offset: 0x000866A4
		private object InternalValidateEndElement(XmlSchemaInfo schemaInfo, object typedValue)
		{
			if (this.validationStack.Length <= 1)
			{
				throw new InvalidOperationException(Res.GetString("Sch_InvalidEndElementMultiple", new object[] { XmlSchemaValidator.MethodNames[9] }));
			}
			this.CheckStateTransition(ValidatorState.EndElement, XmlSchemaValidator.MethodNames[9]);
			SchemaElementDecl elementDecl = this.context.ElementDecl;
			XmlSchemaSimpleType xmlSchemaSimpleType = null;
			XmlSchemaType xmlSchemaType = null;
			XmlSchemaElement xmlSchemaElement = null;
			string text = string.Empty;
			if (elementDecl != null)
			{
				if (this.context.CheckRequiredAttribute && elementDecl.HasRequiredAttribute)
				{
					this.CheckRequiredAttributes(elementDecl);
				}
				if (!this.context.IsNill && this.context.NeedValidateChildren)
				{
					switch (elementDecl.ContentValidator.ContentType)
					{
					case XmlSchemaContentType.TextOnly:
						if (typedValue == null)
						{
							text = this.textValue.ToString();
							typedValue = this.ValidateAtomicValue(text, out xmlSchemaSimpleType);
						}
						else
						{
							typedValue = this.ValidateAtomicValue(typedValue, out xmlSchemaSimpleType);
						}
						break;
					case XmlSchemaContentType.ElementOnly:
						if (typedValue != null)
						{
							throw new InvalidOperationException(Res.GetString("Sch_InvalidEndElementCallTyped"));
						}
						break;
					case XmlSchemaContentType.Mixed:
						if (elementDecl.DefaultValueTyped != null && typedValue == null)
						{
							text = this.textValue.ToString();
							typedValue = this.CheckMixedValueConstraint(text);
						}
						break;
					}
					if (!elementDecl.ContentValidator.CompleteValidation(this.context))
					{
						XmlSchemaValidator.CompleteValidationError(this.context, this.eventHandler, this.nsResolver, this.sourceUriString, this.positionInfo.LineNumber, this.positionInfo.LinePosition, true);
						this.context.Validity = XmlSchemaValidity.Invalid;
					}
				}
				if (this.HasIdentityConstraints)
				{
					XmlSchemaType xmlSchemaType2 = ((xmlSchemaSimpleType == null) ? elementDecl.SchemaType : xmlSchemaSimpleType);
					this.EndElementIdentityConstraints(typedValue, text, xmlSchemaType2.Datatype);
				}
				xmlSchemaType = elementDecl.SchemaType;
				xmlSchemaElement = this.GetSchemaElement();
			}
			if (schemaInfo != null)
			{
				schemaInfo.SchemaType = xmlSchemaType;
				schemaInfo.SchemaElement = xmlSchemaElement;
				schemaInfo.MemberType = xmlSchemaSimpleType;
				schemaInfo.IsNil = this.context.IsNill;
				schemaInfo.IsDefault = this.context.IsDefault;
				if (this.context.Validity == XmlSchemaValidity.NotKnown && this.StrictlyAssessed)
				{
					this.context.Validity = XmlSchemaValidity.Valid;
				}
				schemaInfo.Validity = this.context.Validity;
			}
			this.Pop();
			return typedValue;
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x000878D8 File Offset: 0x000868D8
		private void ProcessSchemaLocations(string xsiSchemaLocation, string xsiNoNamespaceSchemaLocation)
		{
			bool flag = false;
			if (xsiNoNamespaceSchemaLocation != null)
			{
				flag = true;
				this.LoadSchema(string.Empty, xsiNoNamespaceSchemaLocation);
			}
			if (xsiSchemaLocation != null)
			{
				object obj;
				Exception ex = XmlSchemaValidator.dtStringArray.TryParseValue(xsiSchemaLocation, this.nameTable, this.nsResolver, out obj);
				if (ex != null)
				{
					this.SendValidationEvent(new XmlSchemaException("Sch_InvalidValueDetailed", new string[]
					{
						xsiSchemaLocation,
						XmlSchemaValidator.dtStringArray.TypeCodeString,
						ex.Message
					}, ex, null, 0, 0, null));
					return;
				}
				string[] array = (string[])obj;
				flag = true;
				try
				{
					for (int i = 0; i < array.Length - 1; i += 2)
					{
						this.LoadSchema(array[i], array[i + 1]);
					}
				}
				catch (XmlSchemaException ex2)
				{
					this.SendValidationEvent(ex2);
				}
			}
			if (flag)
			{
				this.RecompileSchemaSet();
			}
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x000879AC File Offset: 0x000869AC
		private object ValidateElementContext(XmlQualifiedName elementName, out bool invalidElementInContext)
		{
			object obj = null;
			int num = 0;
			invalidElementInContext = false;
			if (this.context.NeedValidateChildren)
			{
				if (this.context.IsNill)
				{
					this.SendValidationEvent("Sch_ContentInNill", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
					return null;
				}
				ContentValidator contentValidator = this.context.ElementDecl.ContentValidator;
				if (contentValidator.ContentType == XmlSchemaContentType.Mixed && this.context.ElementDecl.Presence == SchemaDeclBase.Use.Fixed)
				{
					this.SendValidationEvent("Sch_ElementInMixedWithFixed", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
					return null;
				}
				XmlQualifiedName xmlQualifiedName = elementName;
				bool flag = false;
				for (;;)
				{
					obj = this.context.ElementDecl.ContentValidator.ValidateElement(xmlQualifiedName, this.context, out num);
					if (obj != null)
					{
						goto IL_0115;
					}
					if (num == -2)
					{
						break;
					}
					flag = true;
					XmlSchemaElement substitutionGroupHead = this.GetSubstitutionGroupHead(xmlQualifiedName);
					if (substitutionGroupHead == null)
					{
						goto IL_0115;
					}
					xmlQualifiedName = substitutionGroupHead.QualifiedName;
				}
				this.SendValidationEvent("Sch_AllElement", elementName.ToString());
				invalidElementInContext = true;
				this.processContents = (this.context.ProcessContents = XmlSchemaContentProcessing.Skip);
				return null;
				IL_0115:
				if (flag)
				{
					XmlSchemaElement xmlSchemaElement = obj as XmlSchemaElement;
					if (xmlSchemaElement == null)
					{
						obj = null;
					}
					else if (xmlSchemaElement.RefName.IsEmpty)
					{
						this.SendValidationEvent("Sch_InvalidElementSubstitution", XmlSchemaValidator.BuildElementName(elementName), XmlSchemaValidator.BuildElementName(xmlSchemaElement.QualifiedName));
						invalidElementInContext = true;
						this.processContents = (this.context.ProcessContents = XmlSchemaContentProcessing.Skip);
					}
					else
					{
						obj = this.compiledSchemaInfo.GetElement(elementName);
						this.context.NeedValidateChildren = true;
					}
				}
				if (obj == null)
				{
					XmlSchemaValidator.ElementValidationError(elementName, this.context, this.eventHandler, this.nsResolver, this.sourceUriString, this.positionInfo.LineNumber, this.positionInfo.LinePosition, true);
					invalidElementInContext = true;
					this.processContents = (this.context.ProcessContents = XmlSchemaContentProcessing.Skip);
				}
			}
			return obj;
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x00087B98 File Offset: 0x00086B98
		private XmlSchemaElement GetSubstitutionGroupHead(XmlQualifiedName member)
		{
			XmlSchemaElement element = this.compiledSchemaInfo.GetElement(member);
			if (element != null)
			{
				XmlQualifiedName substitutionGroup = element.SubstitutionGroup;
				if (!substitutionGroup.IsEmpty)
				{
					XmlSchemaElement element2 = this.compiledSchemaInfo.GetElement(substitutionGroup);
					if (element2 != null)
					{
						if ((element2.BlockResolved & XmlSchemaDerivationMethod.Substitution) != XmlSchemaDerivationMethod.Empty)
						{
							this.SendValidationEvent("Sch_SubstitutionNotAllowed", new string[]
							{
								member.ToString(),
								substitutionGroup.ToString()
							});
							return null;
						}
						if (!XmlSchemaType.IsDerivedFrom(element.ElementSchemaType, element2.ElementSchemaType, element2.BlockResolved))
						{
							this.SendValidationEvent("Sch_SubstitutionBlocked", new string[]
							{
								member.ToString(),
								substitutionGroup.ToString()
							});
							return null;
						}
						return element2;
					}
				}
			}
			return null;
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x00087C54 File Offset: 0x00086C54
		private object ValidateAtomicValue(string stringValue, out XmlSchemaSimpleType memberType)
		{
			object obj = null;
			memberType = null;
			SchemaElementDecl elementDecl = this.context.ElementDecl;
			if (!this.context.IsNill)
			{
				if (stringValue.Length == 0 && elementDecl.DefaultValueTyped != null)
				{
					SchemaElementDecl elementDeclBeforeXsi = this.context.ElementDeclBeforeXsi;
					if (elementDeclBeforeXsi != null && elementDeclBeforeXsi != elementDecl)
					{
						Exception ex = elementDecl.Datatype.TryParseValue(elementDecl.DefaultValueRaw, this.nameTable, this.nsResolver, out obj);
						if (ex != null)
						{
							this.SendValidationEvent("Sch_InvalidElementDefaultValue", new string[]
							{
								elementDecl.DefaultValueRaw,
								XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace)
							});
						}
						else
						{
							this.context.IsDefault = true;
						}
					}
					else
					{
						this.context.IsDefault = true;
						obj = elementDecl.DefaultValueTyped;
					}
				}
				else
				{
					obj = this.CheckElementValue(stringValue);
				}
				XsdSimpleValue xsdSimpleValue = obj as XsdSimpleValue;
				XmlSchemaDatatype xmlSchemaDatatype = elementDecl.Datatype;
				if (xsdSimpleValue != null)
				{
					memberType = xsdSimpleValue.XmlType;
					obj = xsdSimpleValue.TypedValue;
					xmlSchemaDatatype = memberType.Datatype;
				}
				this.CheckTokenizedTypes(xmlSchemaDatatype, obj, false);
			}
			return obj;
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x00087D74 File Offset: 0x00086D74
		private object ValidateAtomicValue(object parsedValue, out XmlSchemaSimpleType memberType)
		{
			memberType = null;
			SchemaElementDecl elementDecl = this.context.ElementDecl;
			object obj = null;
			if (!this.context.IsNill)
			{
				SchemaDeclBase schemaDeclBase = elementDecl;
				XmlSchemaDatatype xmlSchemaDatatype = elementDecl.Datatype;
				Exception ex = xmlSchemaDatatype.TryParseValue(parsedValue, this.nameTable, this.nsResolver, out obj);
				if (ex != null)
				{
					string text = parsedValue as string;
					if (text == null)
					{
						text = XmlSchemaDatatype.ConcatenatedToString(parsedValue);
					}
					this.SendValidationEvent("Sch_ElementValueDataTypeDetailed", new string[]
					{
						XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace),
						text,
						this.GetTypeName(schemaDeclBase),
						ex.Message
					}, ex);
					return null;
				}
				if (!schemaDeclBase.CheckValue(obj))
				{
					this.SendValidationEvent("Sch_FixedElementValue", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
				}
				if (xmlSchemaDatatype.Variety == XmlSchemaDatatypeVariety.Union)
				{
					XsdSimpleValue xsdSimpleValue = obj as XsdSimpleValue;
					memberType = xsdSimpleValue.XmlType;
					obj = xsdSimpleValue.TypedValue;
					xmlSchemaDatatype = memberType.Datatype;
				}
				this.CheckTokenizedTypes(xmlSchemaDatatype, obj, false);
			}
			return obj;
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x00087E94 File Offset: 0x00086E94
		private string GetTypeName(SchemaDeclBase decl)
		{
			string text = decl.SchemaType.QualifiedName.ToString();
			if (text.Length == 0)
			{
				text = decl.Datatype.TypeCodeString;
			}
			return text;
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x00087EC8 File Offset: 0x00086EC8
		private void SaveTextValue(object value)
		{
			string text = value.ToString();
			this.textValue.Append(text);
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x00087EEC File Offset: 0x00086EEC
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
			this.context.IsDefault = false;
			this.context.CheckRequiredAttribute = true;
			this.context.ValidationSkipped = false;
			this.context.Validity = XmlSchemaValidity.NotKnown;
			this.context.NeedValidateChildren = false;
			this.context.ProcessContents = this.processContents;
			this.context.ElementDeclBeforeXsi = null;
			this.context.Constr = null;
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x00087FD4 File Offset: 0x00086FD4
		private void Pop()
		{
			ValidationState validationState = (ValidationState)this.validationStack.Pop();
			if (this.startIDConstraint == this.validationStack.Length)
			{
				this.startIDConstraint = -1;
			}
			this.context = (ValidationState)this.validationStack.Peek();
			if (validationState.Validity == XmlSchemaValidity.Invalid)
			{
				this.context.Validity = XmlSchemaValidity.Invalid;
			}
			if (validationState.ValidationSkipped)
			{
				this.context.ValidationSkipped = true;
			}
			this.processContents = this.context.ProcessContents;
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x0008805C File Offset: 0x0008705C
		private void AddXsiAttributes(ArrayList attList)
		{
			XmlSchemaValidator.BuildXsiAttributes();
			if (this.attPresence[XmlSchemaValidator.xsiTypeSO.QualifiedName] == null)
			{
				attList.Add(XmlSchemaValidator.xsiTypeSO);
			}
			if (this.attPresence[XmlSchemaValidator.xsiNilSO.QualifiedName] == null)
			{
				attList.Add(XmlSchemaValidator.xsiNilSO);
			}
			if (this.attPresence[XmlSchemaValidator.xsiSLSO.QualifiedName] == null)
			{
				attList.Add(XmlSchemaValidator.xsiSLSO);
			}
			if (this.attPresence[XmlSchemaValidator.xsiNoNsSLSO.QualifiedName] == null)
			{
				attList.Add(XmlSchemaValidator.xsiNoNsSLSO);
			}
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x000880FC File Offset: 0x000870FC
		private SchemaElementDecl FastGetElementDecl(XmlQualifiedName elementName, object particle)
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
			if (schemaElementDecl == null && this.processContents != XmlSchemaContentProcessing.Skip)
			{
				if (this.isRoot && this.partialValidationType != null)
				{
					if (this.partialValidationType is XmlSchemaElement)
					{
						XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)this.partialValidationType;
						if (elementName.Equals(xmlSchemaElement2.QualifiedName))
						{
							schemaElementDecl = xmlSchemaElement2.ElementDecl;
						}
						else
						{
							this.SendValidationEvent("Sch_SchemaElementNameMismatch", elementName.ToString(), xmlSchemaElement2.QualifiedName.ToString());
						}
					}
					else if (this.partialValidationType is XmlSchemaType)
					{
						XmlSchemaType xmlSchemaType = (XmlSchemaType)this.partialValidationType;
						schemaElementDecl = xmlSchemaType.ElementDecl;
					}
					else
					{
						this.SendValidationEvent("Sch_ValidateElementInvalidCall", string.Empty);
					}
				}
				else
				{
					schemaElementDecl = this.compiledSchemaInfo.GetElementDecl(elementName);
				}
			}
			return schemaElementDecl;
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x000881F0 File Offset: 0x000871F0
		private SchemaElementDecl CheckXsiTypeAndNil(SchemaElementDecl elementDecl, string xsiType, string xsiNil, ref bool declFound)
		{
			XmlQualifiedName xmlQualifiedName = XmlQualifiedName.Empty;
			if (xsiType != null)
			{
				object obj = null;
				Exception ex = XmlSchemaValidator.dtQName.TryParseValue(xsiType, this.nameTable, this.nsResolver, out obj);
				if (ex != null)
				{
					this.SendValidationEvent(new XmlSchemaException("Sch_InvalidValueDetailed", new string[]
					{
						xsiType,
						XmlSchemaValidator.dtQName.TypeCodeString,
						ex.Message
					}, ex, null, 0, 0, null));
				}
				else
				{
					xmlQualifiedName = obj as XmlQualifiedName;
				}
			}
			if (elementDecl != null)
			{
				if (elementDecl.IsNillable)
				{
					if (xsiNil != null)
					{
						this.context.IsNill = XmlConvert.ToBoolean(xsiNil);
						if (this.context.IsNill && elementDecl.Presence == SchemaDeclBase.Use.Fixed)
						{
							this.SendValidationEvent("Sch_XsiNilAndFixed");
						}
					}
				}
				else if (xsiNil != null)
				{
					this.SendValidationEvent("Sch_InvalidXsiNill");
				}
			}
			if (xmlQualifiedName.IsEmpty)
			{
				if (elementDecl != null && elementDecl.IsAbstract)
				{
					this.SendValidationEvent("Sch_AbstractElement", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
					elementDecl = null;
				}
			}
			else
			{
				SchemaElementDecl schemaElementDecl = this.compiledSchemaInfo.GetTypeDecl(xmlQualifiedName);
				XmlSeverityType xmlSeverityType = XmlSeverityType.Warning;
				if (this.HasSchema && this.processContents == XmlSchemaContentProcessing.Strict)
				{
					xmlSeverityType = XmlSeverityType.Error;
				}
				if (schemaElementDecl == null && xmlQualifiedName.Namespace == this.NsXs)
				{
					XmlSchemaType xmlSchemaType = DatatypeImplementation.GetSimpleTypeFromXsdType(xmlQualifiedName);
					if (xmlSchemaType == null)
					{
						xmlSchemaType = XmlSchemaType.GetBuiltInComplexType(xmlQualifiedName);
					}
					if (xmlSchemaType != null)
					{
						schemaElementDecl = xmlSchemaType.ElementDecl;
					}
				}
				if (schemaElementDecl == null)
				{
					this.SendValidationEvent("Sch_XsiTypeNotFound", xmlQualifiedName.ToString(), xmlSeverityType);
					elementDecl = null;
				}
				else
				{
					declFound = true;
					if (schemaElementDecl.IsAbstract)
					{
						this.SendValidationEvent("Sch_XsiTypeAbstract", xmlQualifiedName.ToString(), xmlSeverityType);
						elementDecl = null;
					}
					else if (elementDecl != null && !XmlSchemaType.IsDerivedFrom(schemaElementDecl.SchemaType, elementDecl.SchemaType, elementDecl.Block))
					{
						this.SendValidationEvent("Sch_XsiTypeBlockedEx", new string[]
						{
							xmlQualifiedName.ToString(),
							XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace)
						});
						elementDecl = null;
					}
					else
					{
						if (elementDecl != null)
						{
							schemaElementDecl = schemaElementDecl.Clone();
							schemaElementDecl.Constraints = elementDecl.Constraints;
							schemaElementDecl.DefaultValueRaw = elementDecl.DefaultValueRaw;
							schemaElementDecl.DefaultValueTyped = elementDecl.DefaultValueTyped;
							schemaElementDecl.Block = elementDecl.Block;
						}
						this.context.ElementDeclBeforeXsi = elementDecl;
						elementDecl = schemaElementDecl;
					}
				}
			}
			return elementDecl;
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x0008844C File Offset: 0x0008744C
		private void ThrowDeclNotFoundWarningOrError(bool declFound)
		{
			if (declFound)
			{
				this.processContents = (this.context.ProcessContents = XmlSchemaContentProcessing.Skip);
				this.context.NeedValidateChildren = false;
				return;
			}
			if (this.HasSchema && this.processContents == XmlSchemaContentProcessing.Strict)
			{
				this.processContents = (this.context.ProcessContents = XmlSchemaContentProcessing.Skip);
				this.context.NeedValidateChildren = false;
				this.SendValidationEvent("Sch_UndeclaredElement", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
				return;
			}
			this.SendValidationEvent("Sch_NoElementSchemaFound", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace), XmlSeverityType.Warning);
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x000884FE File Offset: 0x000874FE
		private void CheckElementProperties()
		{
			if (this.context.ElementDecl.IsAbstract)
			{
				this.SendValidationEvent("Sch_AbstractElement", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
			}
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x00088538 File Offset: 0x00087538
		private void ValidateStartElementIdentityConstraints()
		{
			if (this.ProcessIdentityConstraints && this.context.ElementDecl.Constraints != null)
			{
				this.AddIdentityConstraints();
			}
			if (this.HasIdentityConstraints)
			{
				this.ElementIdentityConstraints();
			}
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x00088568 File Offset: 0x00087568
		private SchemaAttDef CheckIsXmlAttribute(XmlQualifiedName attQName)
		{
			SchemaAttDef schemaAttDef = null;
			if (Ref.Equal(attQName.Namespace, this.NsXml) && (this.validationFlags & XmlSchemaValidationFlags.AllowXmlAttributes) != XmlSchemaValidationFlags.None)
			{
				if (!this.compiledSchemaInfo.Contains(this.NsXml))
				{
					this.AddXmlNamespaceSchema();
				}
				schemaAttDef = (SchemaAttDef)this.compiledSchemaInfo.AttributeDecls[attQName];
			}
			return schemaAttDef;
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x000885C8 File Offset: 0x000875C8
		private void AddXmlNamespaceSchema()
		{
			XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
			xmlSchemaSet.Add(Preprocessor.GetBuildInSchema());
			xmlSchemaSet.Compile();
			this.schemaSet.Add(xmlSchemaSet);
			this.RecompileSchemaSet();
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x00088600 File Offset: 0x00087600
		internal object CheckMixedValueConstraint(string elementValue)
		{
			SchemaElementDecl elementDecl = this.context.ElementDecl;
			if (this.context.IsNill)
			{
				return null;
			}
			if (elementValue.Length == 0)
			{
				this.context.IsDefault = true;
				return elementDecl.DefaultValueTyped;
			}
			SchemaDeclBase schemaDeclBase = elementDecl;
			if (schemaDeclBase.Presence == SchemaDeclBase.Use.Fixed && !elementValue.Equals(elementDecl.DefaultValueRaw))
			{
				this.SendValidationEvent("Sch_FixedElementValue", elementDecl.Name.ToString());
			}
			return elementValue;
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x00088674 File Offset: 0x00087674
		private void LoadSchema(string uri, string url)
		{
			XmlReader xmlReader = null;
			try
			{
				Uri uri2 = this.xmlResolver.ResolveUri(this.sourceUri, url);
				Stream stream = (Stream)this.xmlResolver.GetEntity(uri2, null, null);
				XmlReaderSettings readerSettings = this.schemaSet.ReaderSettings;
				readerSettings.CloseInput = true;
				readerSettings.XmlResolver = this.xmlResolver;
				xmlReader = XmlReader.Create(stream, readerSettings, uri2.ToString());
				this.schemaSet.Add(uri, xmlReader, this.validatedNamespaces);
				while (xmlReader.Read())
				{
				}
			}
			catch (XmlSchemaException ex)
			{
				this.SendValidationEvent("Sch_CannotLoadSchema", new string[] { uri, ex.Message }, ex);
			}
			catch (Exception ex2)
			{
				this.SendValidationEvent("Sch_CannotLoadSchema", new string[] { uri, ex2.Message }, ex2, XmlSeverityType.Warning);
			}
			finally
			{
				if (xmlReader != null)
				{
					xmlReader.Close();
				}
			}
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x00088780 File Offset: 0x00087780
		internal void RecompileSchemaSet()
		{
			if (!this.schemaSet.IsCompiled)
			{
				try
				{
					this.schemaSet.Compile();
				}
				catch (XmlSchemaException ex)
				{
					this.SendValidationEvent(ex);
				}
			}
			this.compiledSchemaInfo = this.schemaSet.CompiledInfo;
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x000887D4 File Offset: 0x000877D4
		private void ProcessTokenizedType(XmlTokenizedType ttype, string name, bool attrValue)
		{
			switch (ttype)
			{
			case XmlTokenizedType.ID:
				if (this.ProcessIdentityConstraints)
				{
					if (this.FindId(name) != null)
					{
						if (attrValue)
						{
							this.attrValid = false;
						}
						this.SendValidationEvent("Sch_DupId", name);
						return;
					}
					if (this.IDs == null)
					{
						this.IDs = new Hashtable();
					}
					this.IDs.Add(name, this.context.LocalName);
					return;
				}
				break;
			case XmlTokenizedType.IDREF:
				if (this.ProcessIdentityConstraints && this.FindId(name) == null)
				{
					this.idRefListHead = new IdRefNode(this.idRefListHead, name, this.positionInfo.LineNumber, this.positionInfo.LinePosition);
					return;
				}
				break;
			case XmlTokenizedType.IDREFS:
				break;
			case XmlTokenizedType.ENTITY:
				this.ProcessEntity(name);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x00088898 File Offset: 0x00087898
		private object CheckAttributeValue(object value, SchemaAttDef attdef)
		{
			object obj = null;
			XmlSchemaDatatype datatype = attdef.Datatype;
			string text = value as string;
			Exception ex;
			if (text != null)
			{
				ex = datatype.TryParseValue(text, this.nameTable, this.nsResolver, out obj);
				if (ex != null)
				{
					goto IL_0078;
				}
			}
			else
			{
				ex = datatype.TryParseValue(value, this.nameTable, this.nsResolver, out obj);
				if (ex != null)
				{
					goto IL_0078;
				}
			}
			if (!attdef.CheckValue(obj))
			{
				this.attrValid = false;
				this.SendValidationEvent("Sch_FixedAttributeValue", attdef.Name.ToString());
			}
			return obj;
			IL_0078:
			this.attrValid = false;
			if (text == null)
			{
				text = XmlSchemaDatatype.ConcatenatedToString(value);
			}
			this.SendValidationEvent("Sch_AttributeValueDataTypeDetailed", new string[]
			{
				attdef.Name.ToString(),
				text,
				this.GetTypeName(attdef),
				ex.Message
			}, ex);
			return null;
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x00088970 File Offset: 0x00087970
		private object CheckElementValue(string stringValue)
		{
			object obj = null;
			SchemaDeclBase elementDecl = this.context.ElementDecl;
			XmlSchemaDatatype datatype = elementDecl.Datatype;
			Exception ex = datatype.TryParseValue(stringValue, this.nameTable, this.nsResolver, out obj);
			if (ex != null)
			{
				this.SendValidationEvent("Sch_ElementValueDataTypeDetailed", new string[]
				{
					XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace),
					stringValue,
					this.GetTypeName(elementDecl),
					ex.Message
				}, ex);
				return null;
			}
			if (!elementDecl.CheckValue(obj))
			{
				this.SendValidationEvent("Sch_FixedElementValue", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
			}
			return obj;
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x00088A2C File Offset: 0x00087A2C
		private void CheckTokenizedTypes(XmlSchemaDatatype dtype, object typedValue, bool attrValue)
		{
			if (typedValue == null)
			{
				return;
			}
			XmlTokenizedType tokenizedType = dtype.TokenizedType;
			if (tokenizedType == XmlTokenizedType.ENTITY || tokenizedType == XmlTokenizedType.ID || tokenizedType == XmlTokenizedType.IDREF)
			{
				if (dtype.Variety == XmlSchemaDatatypeVariety.List)
				{
					string[] array = (string[])typedValue;
					foreach (string text in array)
					{
						this.ProcessTokenizedType(dtype.TokenizedType, text, attrValue);
					}
					return;
				}
				this.ProcessTokenizedType(dtype.TokenizedType, (string)typedValue, attrValue);
			}
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x00088A9B File Offset: 0x00087A9B
		private object FindId(string name)
		{
			if (this.IDs != null)
			{
				return this.IDs[name];
			}
			return null;
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x00088AB4 File Offset: 0x00087AB4
		private void CheckForwardRefs()
		{
			IdRefNode next;
			for (IdRefNode idRefNode = this.idRefListHead; idRefNode != null; idRefNode = next)
			{
				if (this.FindId(idRefNode.Id) == null)
				{
					this.SendValidationEvent(new XmlSchemaValidationException("Sch_UndeclaredId", idRefNode.Id, this.sourceUriString, idRefNode.LineNo, idRefNode.LinePos), XmlSeverityType.Error);
				}
				next = idRefNode.Next;
				idRefNode.Next = null;
			}
			this.idRefListHead = null;
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06001DBF RID: 7615 RVA: 0x00088B1B File Offset: 0x00087B1B
		private bool HasIdentityConstraints
		{
			get
			{
				return this.ProcessIdentityConstraints && this.startIDConstraint != -1;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06001DC0 RID: 7616 RVA: 0x00088B33 File Offset: 0x00087B33
		internal bool ProcessIdentityConstraints
		{
			get
			{
				return (this.validationFlags & XmlSchemaValidationFlags.ProcessIdentityConstraints) != XmlSchemaValidationFlags.None;
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06001DC1 RID: 7617 RVA: 0x00088B43 File Offset: 0x00087B43
		internal bool ReportValidationWarnings
		{
			get
			{
				return (this.validationFlags & XmlSchemaValidationFlags.ReportValidationWarnings) != XmlSchemaValidationFlags.None;
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06001DC2 RID: 7618 RVA: 0x00088B53 File Offset: 0x00087B53
		internal bool ProcessInlineSchema
		{
			get
			{
				return (this.validationFlags & XmlSchemaValidationFlags.ProcessInlineSchema) != XmlSchemaValidationFlags.None;
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06001DC3 RID: 7619 RVA: 0x00088B63 File Offset: 0x00087B63
		internal bool ProcessSchemaLocation
		{
			get
			{
				return (this.validationFlags & XmlSchemaValidationFlags.ProcessSchemaLocation) != XmlSchemaValidationFlags.None;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06001DC4 RID: 7620 RVA: 0x00088B73 File Offset: 0x00087B73
		internal bool ProcessSchemaHints
		{
			get
			{
				return (this.validationFlags & XmlSchemaValidationFlags.ProcessInlineSchema) != XmlSchemaValidationFlags.None || (this.validationFlags & XmlSchemaValidationFlags.ProcessSchemaLocation) != XmlSchemaValidationFlags.None;
			}
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x00088B90 File Offset: 0x00087B90
		private void CheckStateTransition(ValidatorState toState, string methodName)
		{
			if (XmlSchemaValidator.ValidStates[(int)this.currentState, (int)toState])
			{
				this.currentState = toState;
				return;
			}
			if (this.currentState == ValidatorState.None)
			{
				throw new InvalidOperationException(Res.GetString("Sch_InvalidStartTransition", new string[]
				{
					methodName,
					XmlSchemaValidator.MethodNames[1]
				}));
			}
			throw new InvalidOperationException(Res.GetString("Sch_InvalidStateTransition", new string[]
			{
				XmlSchemaValidator.MethodNames[(int)this.currentState],
				methodName
			}));
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x00088C10 File Offset: 0x00087C10
		private void ClearPSVI()
		{
			if (this.textValue != null)
			{
				this.textValue.Length = 0;
			}
			this.attPresence.Clear();
			this.wildID = null;
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x00088C38 File Offset: 0x00087C38
		private void CheckRequiredAttributes(SchemaElementDecl currentElementDecl)
		{
			Hashtable attDefs = currentElementDecl.AttDefs;
			foreach (object obj in attDefs.Values)
			{
				SchemaAttDef schemaAttDef = (SchemaAttDef)obj;
				if (this.attPresence[schemaAttDef.Name] == null && (schemaAttDef.Presence == SchemaDeclBase.Use.Required || schemaAttDef.Presence == SchemaDeclBase.Use.RequiredFixed))
				{
					this.SendValidationEvent("Sch_MissRequiredAttribute", schemaAttDef.Name.ToString());
				}
			}
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x00088CCC File Offset: 0x00087CCC
		private XmlSchemaElement GetSchemaElement()
		{
			SchemaElementDecl elementDeclBeforeXsi = this.context.ElementDeclBeforeXsi;
			SchemaElementDecl elementDecl = this.context.ElementDecl;
			if (elementDeclBeforeXsi != null && elementDeclBeforeXsi.SchemaElement != null)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)elementDeclBeforeXsi.SchemaElement.Clone();
				xmlSchemaElement.SchemaTypeName = XmlQualifiedName.Empty;
				xmlSchemaElement.SchemaType = elementDecl.SchemaType;
				xmlSchemaElement.SetElementType(elementDecl.SchemaType);
				xmlSchemaElement.ElementDecl = elementDecl;
				return xmlSchemaElement;
			}
			return elementDecl.SchemaElement;
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x00088D40 File Offset: 0x00087D40
		internal string GetDefaultAttributePrefix(string attributeNS)
		{
			IDictionary<string, string> namespacesInScope = this.nsResolver.GetNamespacesInScope(XmlNamespaceScope.All);
			string text = null;
			foreach (KeyValuePair<string, string> keyValuePair in namespacesInScope)
			{
				string text2 = this.nameTable.Add(keyValuePair.Value);
				if (Ref.Equal(text2, attributeNS))
				{
					text = keyValuePair.Key;
					if (text.Length != 0)
					{
						return text;
					}
				}
			}
			return text;
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x00088DCC File Offset: 0x00087DCC
		private void AddIdentityConstraints()
		{
			SchemaElementDecl elementDecl = this.context.ElementDecl;
			this.context.Constr = new ConstraintStruct[elementDecl.Constraints.Length];
			int num = 0;
			foreach (CompiledIdentityConstraint compiledIdentityConstraint in elementDecl.Constraints)
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
						this.SendValidationEvent("Sch_RefNotInScope", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
					}
				}
			}
			if (this.startIDConstraint == -1)
			{
				this.startIDConstraint = this.validationStack.Length - 1;
			}
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x00088F94 File Offset: 0x00087F94
		private void ElementIdentityConstraints()
		{
			SchemaElementDecl elementDecl = this.context.ElementDecl;
			string localName = this.context.LocalName;
			string @namespace = this.context.Namespace;
			for (int i = this.startIDConstraint; i < this.validationStack.Length; i++)
			{
				if (((ValidationState)this.validationStack[i]).Constr != null)
				{
					foreach (ConstraintStruct constraintStruct in ((ValidationState)this.validationStack[i]).Constr)
					{
						if (constraintStruct.axisSelector.MoveToStartElement(localName, @namespace))
						{
							constraintStruct.axisSelector.PushKS(this.positionInfo.LineNumber, this.positionInfo.LinePosition);
						}
						foreach (object obj in constraintStruct.axisFields)
						{
							LocatedActiveAxis locatedActiveAxis = (LocatedActiveAxis)obj;
							if (locatedActiveAxis.MoveToStartElement(localName, @namespace) && elementDecl != null)
							{
								if (elementDecl.Datatype == null || elementDecl.ContentValidator.ContentType == XmlSchemaContentType.Mixed)
								{
									this.SendValidationEvent("Sch_FieldSimpleTypeExpected", localName);
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

		// Token: 0x06001DCC RID: 7628 RVA: 0x000890F4 File Offset: 0x000880F4
		private void AttributeIdentityConstraints(string name, string ns, object obj, string sobj, XmlSchemaDatatype datatype)
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
									this.SendValidationEvent("Sch_FieldSingleValueExpected", name);
								}
								else
								{
									locatedActiveAxis.Ks[locatedActiveAxis.Column] = new TypedObject(obj, sobj, datatype);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x00089204 File Offset: 0x00088204
		private void EndElementIdentityConstraints(object typedValue, string stringValue, XmlSchemaDatatype datatype)
		{
			string localName = this.context.LocalName;
			string @namespace = this.context.Namespace;
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
									this.SendValidationEvent("Sch_FieldSingleValueExpected", localName);
								}
								else if (typedValue != null && stringValue.Length != 0)
								{
									locatedActiveAxis.Ks[locatedActiveAxis.Column] = new TypedObject(typedValue, stringValue, datatype);
								}
							}
							locatedActiveAxis.EndElement(localName, @namespace);
						}
						if (constraintStruct.axisSelector.EndElement(localName, @namespace))
						{
							KeySequence keySequence = constraintStruct.axisSelector.PopKS();
							switch (constraintStruct.constraint.Role)
							{
							case CompiledIdentityConstraint.ConstraintRole.Unique:
								if (keySequence.IsQualified())
								{
									if (constraintStruct.qualifiedTable.Contains(keySequence))
									{
										this.SendValidationEvent(new XmlSchemaValidationException("Sch_DuplicateKey", new string[]
										{
											keySequence.ToString(),
											constraintStruct.constraint.name.ToString()
										}, this.sourceUriString, keySequence.PosLine, keySequence.PosCol));
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
									this.SendValidationEvent(new XmlSchemaValidationException("Sch_MissingKey", constraintStruct.constraint.name.ToString(), this.sourceUriString, keySequence.PosLine, keySequence.PosCol));
								}
								else if (constraintStruct.qualifiedTable.Contains(keySequence))
								{
									this.SendValidationEvent(new XmlSchemaValidationException("Sch_DuplicateKey", new string[]
									{
										keySequence.ToString(),
										constraintStruct.constraint.name.ToString()
									}, this.sourceUriString, keySequence.PosLine, keySequence.PosCol));
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
								this.SendValidationEvent(new XmlSchemaValidationException("Sch_UnresolvedKeyref", keySequence2.ToString(), this.sourceUriString, keySequence2.PosLine, keySequence2.PosCol));
							}
						}
					}
				}
			}
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x000895E8 File Offset: 0x000885E8
		private static void BuildXsiAttributes()
		{
			if (XmlSchemaValidator.xsiTypeSO == null)
			{
				XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
				xmlSchemaAttribute.Name = "type";
				xmlSchemaAttribute.SetQualifiedName(new XmlQualifiedName("type", "http://www.w3.org/2001/XMLSchema-instance"));
				xmlSchemaAttribute.SetAttributeType(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.QName));
				Interlocked.CompareExchange<XmlSchemaAttribute>(ref XmlSchemaValidator.xsiTypeSO, xmlSchemaAttribute, null);
			}
			if (XmlSchemaValidator.xsiNilSO == null)
			{
				XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
				xmlSchemaAttribute2.Name = "nil";
				xmlSchemaAttribute2.SetQualifiedName(new XmlQualifiedName("nil", "http://www.w3.org/2001/XMLSchema-instance"));
				xmlSchemaAttribute2.SetAttributeType(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.Boolean));
				Interlocked.CompareExchange<XmlSchemaAttribute>(ref XmlSchemaValidator.xsiNilSO, xmlSchemaAttribute2, null);
			}
			if (XmlSchemaValidator.xsiSLSO == null)
			{
				XmlSchemaSimpleType builtInSimpleType = XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String);
				XmlSchemaAttribute xmlSchemaAttribute3 = new XmlSchemaAttribute();
				xmlSchemaAttribute3.Name = "schemaLocation";
				xmlSchemaAttribute3.SetQualifiedName(new XmlQualifiedName("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance"));
				xmlSchemaAttribute3.SetAttributeType(builtInSimpleType);
				Interlocked.CompareExchange<XmlSchemaAttribute>(ref XmlSchemaValidator.xsiSLSO, xmlSchemaAttribute3, null);
			}
			if (XmlSchemaValidator.xsiNoNsSLSO == null)
			{
				XmlSchemaSimpleType builtInSimpleType2 = XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String);
				XmlSchemaAttribute xmlSchemaAttribute4 = new XmlSchemaAttribute();
				xmlSchemaAttribute4.Name = "noNamespaceSchemaLocation";
				xmlSchemaAttribute4.SetQualifiedName(new XmlQualifiedName("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance"));
				xmlSchemaAttribute4.SetAttributeType(builtInSimpleType2);
				Interlocked.CompareExchange<XmlSchemaAttribute>(ref XmlSchemaValidator.xsiNoNsSLSO, xmlSchemaAttribute4, null);
			}
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x0008971C File Offset: 0x0008871C
		internal static void ElementValidationError(XmlQualifiedName name, ValidationState context, ValidationEventHandler eventHandler, object sender, string sourceUri, int lineNo, int linePos, bool getParticles)
		{
			if (context.ElementDecl != null)
			{
				ContentValidator contentValidator = context.ElementDecl.ContentValidator;
				XmlSchemaContentType contentType = contentValidator.ContentType;
				if (contentType == XmlSchemaContentType.ElementOnly || (contentType == XmlSchemaContentType.Mixed && contentValidator != ContentValidator.Mixed && contentValidator != ContentValidator.Any))
				{
					ArrayList arrayList;
					if (getParticles)
					{
						arrayList = contentValidator.ExpectedParticles(context, false);
					}
					else
					{
						arrayList = contentValidator.ExpectedElements(context, false);
					}
					if (arrayList == null || arrayList.Count == 0)
					{
						if (context.TooComplex)
						{
							XmlSchemaValidator.SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("Sch_InvalidElementContentComplex", new string[]
							{
								XmlSchemaValidator.BuildElementName(context.LocalName, context.Namespace),
								XmlSchemaValidator.BuildElementName(name),
								Res.GetString("Sch_ComplexContentModel")
							}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
							return;
						}
						XmlSchemaValidator.SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("Sch_InvalidElementContent", new string[]
						{
							XmlSchemaValidator.BuildElementName(context.LocalName, context.Namespace),
							XmlSchemaValidator.BuildElementName(name)
						}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
						return;
					}
					else
					{
						if (context.TooComplex)
						{
							XmlSchemaValidator.SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("Sch_InvalidElementContentExpectingComplex", new string[]
							{
								XmlSchemaValidator.BuildElementName(context.LocalName, context.Namespace),
								XmlSchemaValidator.BuildElementName(name),
								XmlSchemaValidator.PrintExpectedElements(arrayList, getParticles),
								Res.GetString("Sch_ComplexContentModel")
							}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
							return;
						}
						XmlSchemaValidator.SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("Sch_InvalidElementContentExpecting", new string[]
						{
							XmlSchemaValidator.BuildElementName(context.LocalName, context.Namespace),
							XmlSchemaValidator.BuildElementName(name),
							XmlSchemaValidator.PrintExpectedElements(arrayList, getParticles)
						}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
						return;
					}
				}
				else
				{
					if (contentType == XmlSchemaContentType.Empty)
					{
						XmlSchemaValidator.SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("Sch_InvalidElementInEmptyEx", new string[]
						{
							XmlSchemaValidator.QNameString(context.LocalName, context.Namespace),
							name.ToString()
						}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
						return;
					}
					if (!contentValidator.IsOpen)
					{
						XmlSchemaValidator.SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("Sch_InvalidElementInTextOnlyEx", new string[]
						{
							XmlSchemaValidator.QNameString(context.LocalName, context.Namespace),
							name.ToString()
						}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
					}
				}
			}
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x00089968 File Offset: 0x00088968
		internal static void CompleteValidationError(ValidationState context, ValidationEventHandler eventHandler, object sender, string sourceUri, int lineNo, int linePos, bool getParticles)
		{
			ArrayList arrayList = null;
			if (context.ElementDecl != null)
			{
				if (getParticles)
				{
					arrayList = context.ElementDecl.ContentValidator.ExpectedParticles(context, true);
				}
				else
				{
					arrayList = context.ElementDecl.ContentValidator.ExpectedElements(context, true);
				}
			}
			if (arrayList == null || arrayList.Count == 0)
			{
				if (context.TooComplex)
				{
					XmlSchemaValidator.SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("Sch_IncompleteContentComplex", new string[]
					{
						XmlSchemaValidator.BuildElementName(context.LocalName, context.Namespace),
						Res.GetString("Sch_ComplexContentModel")
					}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
				}
				XmlSchemaValidator.SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("Sch_IncompleteContent", XmlSchemaValidator.BuildElementName(context.LocalName, context.Namespace), sourceUri, lineNo, linePos), XmlSeverityType.Error);
				return;
			}
			if (context.TooComplex)
			{
				XmlSchemaValidator.SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("Sch_IncompleteContentExpectingComplex", new string[]
				{
					XmlSchemaValidator.BuildElementName(context.LocalName, context.Namespace),
					XmlSchemaValidator.PrintExpectedElements(arrayList, getParticles),
					Res.GetString("Sch_ComplexContentModel")
				}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
				return;
			}
			XmlSchemaValidator.SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("Sch_IncompleteContentExpecting", new string[]
			{
				XmlSchemaValidator.BuildElementName(context.LocalName, context.Namespace),
				XmlSchemaValidator.PrintExpectedElements(arrayList, getParticles)
			}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x00089ABC File Offset: 0x00088ABC
		internal static string PrintExpectedElements(ArrayList expected, bool getParticles)
		{
			if (getParticles)
			{
				string @string = Res.GetString("Sch_ContinuationString", new string[] { " " });
				XmlSchemaParticle xmlSchemaParticle = null;
				ArrayList arrayList = new ArrayList();
				StringBuilder stringBuilder = new StringBuilder();
				if (expected.Count == 1)
				{
					xmlSchemaParticle = expected[0] as XmlSchemaParticle;
				}
				else
				{
					for (int i = 1; i < expected.Count; i++)
					{
						XmlSchemaParticle xmlSchemaParticle2 = expected[i - 1] as XmlSchemaParticle;
						xmlSchemaParticle = expected[i] as XmlSchemaParticle;
						XmlQualifiedName qualifiedName = xmlSchemaParticle2.GetQualifiedName();
						if (qualifiedName.Namespace != xmlSchemaParticle.GetQualifiedName().Namespace)
						{
							arrayList.Add(qualifiedName);
							XmlSchemaValidator.PrintNamesWithNS(arrayList, stringBuilder);
							arrayList.Clear();
							stringBuilder.Append(@string);
						}
						else
						{
							arrayList.Add(qualifiedName);
						}
					}
				}
				arrayList.Add(xmlSchemaParticle.GetQualifiedName());
				XmlSchemaValidator.PrintNamesWithNS(arrayList, stringBuilder);
				return stringBuilder.ToString();
			}
			return XmlSchemaValidator.PrintNames(expected);
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x00089BBC File Offset: 0x00088BBC
		private static string PrintNames(ArrayList expected)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("'");
			stringBuilder.Append(expected[0].ToString());
			for (int i = 1; i < expected.Count; i++)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(expected[i].ToString());
			}
			stringBuilder.Append("'");
			return stringBuilder.ToString();
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x00089C30 File Offset: 0x00088C30
		private static void PrintNamesWithNS(ArrayList expected, StringBuilder builder)
		{
			XmlQualifiedName xmlQualifiedName = expected[0] as XmlQualifiedName;
			if (expected.Count == 1)
			{
				if (xmlQualifiedName.Name == "*")
				{
					XmlSchemaValidator.EnumerateAny(builder, xmlQualifiedName.Namespace);
					return;
				}
				if (xmlQualifiedName.Namespace.Length != 0)
				{
					builder.Append(Res.GetString("Sch_ElementNameAndNamespace", new object[] { xmlQualifiedName.Name, xmlQualifiedName.Namespace }));
					return;
				}
				builder.Append(Res.GetString("Sch_ElementName", new object[] { xmlQualifiedName.Name }));
				return;
			}
			else
			{
				bool flag = false;
				bool flag2 = true;
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < expected.Count; i++)
				{
					xmlQualifiedName = expected[i] as XmlQualifiedName;
					if (xmlQualifiedName.Name == "*")
					{
						flag = true;
					}
					else
					{
						if (flag2)
						{
							flag2 = false;
						}
						else
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(xmlQualifiedName.Name);
					}
				}
				if (flag)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(Res.GetString("Sch_AnyElement"));
					return;
				}
				if (xmlQualifiedName.Namespace.Length != 0)
				{
					builder.Append(Res.GetString("Sch_ElementNameAndNamespace", new object[]
					{
						stringBuilder.ToString(),
						xmlQualifiedName.Namespace
					}));
					return;
				}
				builder.Append(Res.GetString("Sch_ElementName", new object[] { stringBuilder.ToString() }));
				return;
			}
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x00089DC0 File Offset: 0x00088DC0
		private static void EnumerateAny(StringBuilder builder, string namespaces)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (namespaces == "##any" || namespaces == "##other")
			{
				stringBuilder.Append(namespaces);
			}
			else
			{
				string[] array = XmlConvert.SplitString(namespaces);
				stringBuilder.Append(array[0]);
				for (int i = 1; i < array.Length; i++)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(array[i]);
				}
			}
			builder.Append(Res.GetString("Sch_AnyElementNS", new object[] { stringBuilder.ToString() }));
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x00089E4E File Offset: 0x00088E4E
		internal static string QNameString(string localName, string ns)
		{
			if (ns.Length == 0)
			{
				return localName;
			}
			return ns + ":" + localName;
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x00089E66 File Offset: 0x00088E66
		internal static string BuildElementName(XmlQualifiedName qname)
		{
			return XmlSchemaValidator.BuildElementName(qname.Name, qname.Namespace);
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x00089E7C File Offset: 0x00088E7C
		internal static string BuildElementName(string localName, string ns)
		{
			if (ns.Length != 0)
			{
				return Res.GetString("Sch_ElementNameAndNamespace", new object[] { localName, ns });
			}
			return Res.GetString("Sch_ElementName", new object[] { localName });
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x00089EC4 File Offset: 0x00088EC4
		private void ProcessEntity(string name)
		{
			if (!this.checkEntity)
			{
				return;
			}
			SchemaEntity schemaEntity = null;
			if (this.dtdSchemaInfo != null)
			{
				schemaEntity = (SchemaEntity)this.dtdSchemaInfo.GeneralEntities[new XmlQualifiedName(name)];
			}
			if (schemaEntity == null)
			{
				this.SendValidationEvent("Sch_UndeclaredEntity", name);
				return;
			}
			if (!schemaEntity.NData.IsEmpty)
			{
				this.SendValidationEvent("Sch_UnparsedEntityRef", name);
			}
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x00089F29 File Offset: 0x00088F29
		private void SendValidationEvent(string code)
		{
			this.SendValidationEvent(code, string.Empty);
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x00089F37 File Offset: 0x00088F37
		private void SendValidationEvent(string code, string[] args)
		{
			this.SendValidationEvent(new XmlSchemaValidationException(code, args, this.sourceUriString, this.positionInfo.LineNumber, this.positionInfo.LinePosition));
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x00089F62 File Offset: 0x00088F62
		private void SendValidationEvent(string code, string arg)
		{
			this.SendValidationEvent(new XmlSchemaValidationException(code, arg, this.sourceUriString, this.positionInfo.LineNumber, this.positionInfo.LinePosition));
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x00089F90 File Offset: 0x00088F90
		private void SendValidationEvent(string code, string arg1, string arg2)
		{
			this.SendValidationEvent(new XmlSchemaValidationException(code, new string[] { arg1, arg2 }, this.sourceUriString, this.positionInfo.LineNumber, this.positionInfo.LinePosition));
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x00089FD5 File Offset: 0x00088FD5
		private void SendValidationEvent(string code, string[] args, Exception innerException, XmlSeverityType severity)
		{
			if (severity != XmlSeverityType.Warning || this.ReportValidationWarnings)
			{
				this.SendValidationEvent(new XmlSchemaValidationException(code, args, innerException, this.sourceUriString, this.positionInfo.LineNumber, this.positionInfo.LinePosition), severity);
			}
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x0008A010 File Offset: 0x00089010
		private void SendValidationEvent(string code, string[] args, Exception innerException)
		{
			this.SendValidationEvent(new XmlSchemaValidationException(code, args, innerException, this.sourceUriString, this.positionInfo.LineNumber, this.positionInfo.LinePosition), XmlSeverityType.Error);
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x0008A03D File Offset: 0x0008903D
		private void SendValidationEvent(XmlSchemaValidationException e)
		{
			this.SendValidationEvent(e, XmlSeverityType.Error);
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x0008A047 File Offset: 0x00089047
		private void SendValidationEvent(XmlSchemaException e)
		{
			this.SendValidationEvent(new XmlSchemaValidationException(e.GetRes, e.Args, e.SourceUri, e.LineNumber, e.LinePosition), XmlSeverityType.Error);
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x0008A073 File Offset: 0x00089073
		private void SendValidationEvent(string code, string msg, XmlSeverityType severity)
		{
			if (severity != XmlSeverityType.Warning || this.ReportValidationWarnings)
			{
				this.SendValidationEvent(new XmlSchemaValidationException(code, msg, this.sourceUriString, this.positionInfo.LineNumber, this.positionInfo.LinePosition), severity);
			}
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x0008A0AC File Offset: 0x000890AC
		private void SendValidationEvent(XmlSchemaValidationException e, XmlSeverityType severity)
		{
			bool flag = false;
			if (severity == XmlSeverityType.Error)
			{
				flag = true;
				this.context.Validity = XmlSchemaValidity.Invalid;
			}
			if (!flag)
			{
				if (this.ReportValidationWarnings && this.eventHandler != null)
				{
					this.eventHandler(this.validationEventSender, new ValidationEventArgs(e, severity));
				}
				return;
			}
			if (this.eventHandler != null)
			{
				this.eventHandler(this.validationEventSender, new ValidationEventArgs(e, severity));
				return;
			}
			throw e;
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x0008A11A File Offset: 0x0008911A
		internal static void SendValidationEvent(ValidationEventHandler eventHandler, object sender, XmlSchemaValidationException e, XmlSeverityType severity)
		{
			if (eventHandler != null)
			{
				eventHandler(sender, new ValidationEventArgs(e, severity));
				return;
			}
			if (severity == XmlSeverityType.Error)
			{
				throw e;
			}
		}

		// Token: 0x04001203 RID: 4611
		private const int STACK_INCREMENT = 10;

		// Token: 0x04001204 RID: 4612
		private const string Quote = "'";

		// Token: 0x04001205 RID: 4613
		private XmlSchemaSet schemaSet;

		// Token: 0x04001206 RID: 4614
		private XmlSchemaValidationFlags validationFlags;

		// Token: 0x04001207 RID: 4615
		private int startIDConstraint = -1;

		// Token: 0x04001208 RID: 4616
		private bool isRoot;

		// Token: 0x04001209 RID: 4617
		private bool rootHasSchema;

		// Token: 0x0400120A RID: 4618
		private bool attrValid;

		// Token: 0x0400120B RID: 4619
		private bool checkEntity;

		// Token: 0x0400120C RID: 4620
		private SchemaInfo compiledSchemaInfo;

		// Token: 0x0400120D RID: 4621
		private SchemaInfo dtdSchemaInfo;

		// Token: 0x0400120E RID: 4622
		private Hashtable validatedNamespaces;

		// Token: 0x0400120F RID: 4623
		private HWStack validationStack;

		// Token: 0x04001210 RID: 4624
		private ValidationState context;

		// Token: 0x04001211 RID: 4625
		private ValidatorState currentState;

		// Token: 0x04001212 RID: 4626
		private Hashtable attPresence;

		// Token: 0x04001213 RID: 4627
		private SchemaAttDef wildID;

		// Token: 0x04001214 RID: 4628
		private Hashtable IDs;

		// Token: 0x04001215 RID: 4629
		private IdRefNode idRefListHead;

		// Token: 0x04001216 RID: 4630
		private XmlQualifiedName contextQName;

		// Token: 0x04001217 RID: 4631
		private string NsXs;

		// Token: 0x04001218 RID: 4632
		private string NsXsi;

		// Token: 0x04001219 RID: 4633
		private string NsXmlNs;

		// Token: 0x0400121A RID: 4634
		private string NsXml;

		// Token: 0x0400121B RID: 4635
		private XmlSchemaObject partialValidationType;

		// Token: 0x0400121C RID: 4636
		private StringBuilder textValue;

		// Token: 0x0400121D RID: 4637
		private ValidationEventHandler eventHandler;

		// Token: 0x0400121E RID: 4638
		private object validationEventSender;

		// Token: 0x0400121F RID: 4639
		private XmlNameTable nameTable;

		// Token: 0x04001220 RID: 4640
		private IXmlLineInfo positionInfo;

		// Token: 0x04001221 RID: 4641
		private IXmlLineInfo dummyPositionInfo;

		// Token: 0x04001222 RID: 4642
		private XmlResolver xmlResolver;

		// Token: 0x04001223 RID: 4643
		private Uri sourceUri;

		// Token: 0x04001224 RID: 4644
		private string sourceUriString;

		// Token: 0x04001225 RID: 4645
		private IXmlNamespaceResolver nsResolver;

		// Token: 0x04001226 RID: 4646
		private XmlSchemaContentProcessing processContents = XmlSchemaContentProcessing.Strict;

		// Token: 0x04001227 RID: 4647
		private static XmlSchemaAttribute xsiTypeSO;

		// Token: 0x04001228 RID: 4648
		private static XmlSchemaAttribute xsiNilSO;

		// Token: 0x04001229 RID: 4649
		private static XmlSchemaAttribute xsiSLSO;

		// Token: 0x0400122A RID: 4650
		private static XmlSchemaAttribute xsiNoNsSLSO;

		// Token: 0x0400122B RID: 4651
		private string xsiTypeString;

		// Token: 0x0400122C RID: 4652
		private string xsiNilString;

		// Token: 0x0400122D RID: 4653
		private string xsiSchemaLocationString;

		// Token: 0x0400122E RID: 4654
		private string xsiNoNamespaceSchemaLocationString;

		// Token: 0x0400122F RID: 4655
		private static readonly XmlSchemaDatatype dtQName = XmlSchemaDatatype.FromXmlTokenizedTypeXsd(XmlTokenizedType.QName);

		// Token: 0x04001230 RID: 4656
		private static readonly XmlSchemaDatatype dtCDATA = XmlSchemaDatatype.FromXmlTokenizedType(XmlTokenizedType.CDATA);

		// Token: 0x04001231 RID: 4657
		private static readonly XmlSchemaDatatype dtStringArray = XmlSchemaValidator.dtCDATA.DeriveByList(null);

		// Token: 0x04001232 RID: 4658
		private static XmlSchemaParticle[] EmptyParticleArray = new XmlSchemaParticle[0];

		// Token: 0x04001233 RID: 4659
		private static XmlSchemaAttribute[] EmptyAttributeArray = new XmlSchemaAttribute[0];

		// Token: 0x04001234 RID: 4660
		private XmlCharType xmlCharType = XmlCharType.Instance;

		// Token: 0x04001235 RID: 4661
		internal static bool[,] ValidStates = new bool[,]
		{
			{
				true, true, false, false, false, false, false, false, false, false,
				false, false
			},
			{
				false, true, true, true, true, false, false, false, false, false,
				false, true
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, true
			},
			{
				false, false, false, true, true, false, false, false, false, false,
				false, true
			},
			{
				false, false, false, true, false, true, true, false, false, true,
				true, false
			},
			{
				false, false, false, false, false, true, true, false, false, true,
				true, false
			},
			{
				false, false, false, false, true, false, false, true, true, true,
				true, false
			},
			{
				false, false, false, false, true, false, false, true, true, true,
				true, false
			},
			{
				false, false, false, false, true, false, false, true, true, true,
				true, false
			},
			{
				false, false, false, true, true, false, false, true, true, true,
				true, true
			},
			{
				false, false, false, true, true, false, false, true, true, true,
				true, true
			},
			{
				false, true, false, false, false, false, false, false, false, false,
				false, false
			}
		};

		// Token: 0x04001236 RID: 4662
		private static string[] MethodNames = new string[]
		{
			"None", "Initialize", "top-level ValidateAttribute", "top-level ValidateText or ValidateWhitespace", "ValidateElement", "ValidateAttribute", "ValidateEndOfAttributes", "ValidateText", "ValidateWhitespace", "ValidateEndElement",
			"SkipToEndElement", "EndValidation"
		};
	}
}
