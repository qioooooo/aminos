using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.XmlConfiguration;

namespace System.Xml.Schema
{
	public sealed class XmlSchemaValidator
	{
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

		public XmlResolver XmlResolver
		{
			set
			{
				this.xmlResolver = value;
			}
		}

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

		public void ValidateElement(string localName, string namespaceUri, XmlSchemaInfo schemaInfo)
		{
			this.ValidateElement(localName, namespaceUri, schemaInfo, null, null, null, null);
		}

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

		public object ValidateAttribute(string localName, string namespaceUri, string attributeValue, XmlSchemaInfo schemaInfo)
		{
			if (attributeValue == null)
			{
				throw new ArgumentNullException("attributeValue");
			}
			return this.ValidateAttribute(localName, namespaceUri, null, attributeValue, schemaInfo);
		}

		public object ValidateAttribute(string localName, string namespaceUri, XmlValueGetter attributeValue, XmlSchemaInfo schemaInfo)
		{
			if (attributeValue == null)
			{
				throw new ArgumentNullException("attributeValue");
			}
			return this.ValidateAttribute(localName, namespaceUri, attributeValue, null, schemaInfo);
		}

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

		public void GetUnspecifiedDefaultAttributes(ArrayList defaultAttributes)
		{
			if (defaultAttributes == null)
			{
				throw new ArgumentNullException("defaultAttributes");
			}
			this.CheckStateTransition(ValidatorState.Attribute, "GetUnspecifiedDefaultAttributes");
			this.GetUnspecifiedDefaultAttributes(defaultAttributes, false);
		}

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

		public void ValidateText(string elementValue)
		{
			if (elementValue == null)
			{
				throw new ArgumentNullException("elementValue");
			}
			this.ValidateText(elementValue, null);
		}

		public void ValidateText(XmlValueGetter elementValue)
		{
			if (elementValue == null)
			{
				throw new ArgumentNullException("elementValue");
			}
			this.ValidateText(null, elementValue);
		}

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

		public void ValidateWhitespace(string elementValue)
		{
			if (elementValue == null)
			{
				throw new ArgumentNullException("elementValue");
			}
			this.ValidateWhitespace(elementValue, null);
		}

		public void ValidateWhitespace(XmlValueGetter elementValue)
		{
			if (elementValue == null)
			{
				throw new ArgumentNullException("elementValue");
			}
			this.ValidateWhitespace(null, elementValue);
		}

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

		public object ValidateEndElement(XmlSchemaInfo schemaInfo)
		{
			return this.InternalValidateEndElement(schemaInfo, null);
		}

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

		public void EndValidation()
		{
			if (this.validationStack.Length > 1)
			{
				throw new InvalidOperationException(Res.GetString("Sch_InvalidEndValidation"));
			}
			this.CheckStateTransition(ValidatorState.Finish, XmlSchemaValidator.MethodNames[11]);
			this.CheckForwardRefs();
		}

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

		internal XmlSchemaSet SchemaSet
		{
			get
			{
				return this.schemaSet;
			}
		}

		internal XmlSchemaValidationFlags ValidationFlags
		{
			get
			{
				return this.validationFlags;
			}
		}

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

		internal XmlSchemaContentProcessing CurrentProcessContents
		{
			get
			{
				return this.processContents;
			}
		}

		internal void SetDtdSchemaInfo(SchemaInfo dtdSchemaInfo)
		{
			this.dtdSchemaInfo = dtdSchemaInfo;
			this.checkEntity = true;
		}

		private bool StrictlyAssessed
		{
			get
			{
				return (this.processContents == XmlSchemaContentProcessing.Strict || this.processContents == XmlSchemaContentProcessing.Lax) && this.context.ElementDecl != null && !this.context.ValidationSkipped;
			}
		}

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

		internal string GetConcatenatedValue()
		{
			return this.textValue.ToString();
		}

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

		private string GetTypeName(SchemaDeclBase decl)
		{
			string text = decl.SchemaType.QualifiedName.ToString();
			if (text.Length == 0)
			{
				text = decl.Datatype.TypeCodeString;
			}
			return text;
		}

		private void SaveTextValue(object value)
		{
			string text = value.ToString();
			this.textValue.Append(text);
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
			this.context.IsDefault = false;
			this.context.CheckRequiredAttribute = true;
			this.context.ValidationSkipped = false;
			this.context.Validity = XmlSchemaValidity.NotKnown;
			this.context.NeedValidateChildren = false;
			this.context.ProcessContents = this.processContents;
			this.context.ElementDeclBeforeXsi = null;
			this.context.Constr = null;
		}

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

		private void CheckElementProperties()
		{
			if (this.context.ElementDecl.IsAbstract)
			{
				this.SendValidationEvent("Sch_AbstractElement", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
			}
		}

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

		private void AddXmlNamespaceSchema()
		{
			XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
			xmlSchemaSet.Add(Preprocessor.GetBuildInSchema());
			xmlSchemaSet.Compile();
			this.schemaSet.Add(xmlSchemaSet);
			this.RecompileSchemaSet();
		}

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

		private object FindId(string name)
		{
			if (this.IDs != null)
			{
				return this.IDs[name];
			}
			return null;
		}

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

		private bool HasIdentityConstraints
		{
			get
			{
				return this.ProcessIdentityConstraints && this.startIDConstraint != -1;
			}
		}

		internal bool ProcessIdentityConstraints
		{
			get
			{
				return (this.validationFlags & XmlSchemaValidationFlags.ProcessIdentityConstraints) != XmlSchemaValidationFlags.None;
			}
		}

		internal bool ReportValidationWarnings
		{
			get
			{
				return (this.validationFlags & XmlSchemaValidationFlags.ReportValidationWarnings) != XmlSchemaValidationFlags.None;
			}
		}

		internal bool ProcessInlineSchema
		{
			get
			{
				return (this.validationFlags & XmlSchemaValidationFlags.ProcessInlineSchema) != XmlSchemaValidationFlags.None;
			}
		}

		internal bool ProcessSchemaLocation
		{
			get
			{
				return (this.validationFlags & XmlSchemaValidationFlags.ProcessSchemaLocation) != XmlSchemaValidationFlags.None;
			}
		}

		internal bool ProcessSchemaHints
		{
			get
			{
				return (this.validationFlags & XmlSchemaValidationFlags.ProcessInlineSchema) != XmlSchemaValidationFlags.None || (this.validationFlags & XmlSchemaValidationFlags.ProcessSchemaLocation) != XmlSchemaValidationFlags.None;
			}
		}

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

		private void ClearPSVI()
		{
			if (this.textValue != null)
			{
				this.textValue.Length = 0;
			}
			this.attPresence.Clear();
			this.wildID = null;
		}

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

		internal static string QNameString(string localName, string ns)
		{
			if (ns.Length == 0)
			{
				return localName;
			}
			return ns + ":" + localName;
		}

		internal static string BuildElementName(XmlQualifiedName qname)
		{
			return XmlSchemaValidator.BuildElementName(qname.Name, qname.Namespace);
		}

		internal static string BuildElementName(string localName, string ns)
		{
			if (ns.Length != 0)
			{
				return Res.GetString("Sch_ElementNameAndNamespace", new object[] { localName, ns });
			}
			return Res.GetString("Sch_ElementName", new object[] { localName });
		}

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

		private void SendValidationEvent(string code)
		{
			this.SendValidationEvent(code, string.Empty);
		}

		private void SendValidationEvent(string code, string[] args)
		{
			this.SendValidationEvent(new XmlSchemaValidationException(code, args, this.sourceUriString, this.positionInfo.LineNumber, this.positionInfo.LinePosition));
		}

		private void SendValidationEvent(string code, string arg)
		{
			this.SendValidationEvent(new XmlSchemaValidationException(code, arg, this.sourceUriString, this.positionInfo.LineNumber, this.positionInfo.LinePosition));
		}

		private void SendValidationEvent(string code, string arg1, string arg2)
		{
			this.SendValidationEvent(new XmlSchemaValidationException(code, new string[] { arg1, arg2 }, this.sourceUriString, this.positionInfo.LineNumber, this.positionInfo.LinePosition));
		}

		private void SendValidationEvent(string code, string[] args, Exception innerException, XmlSeverityType severity)
		{
			if (severity != XmlSeverityType.Warning || this.ReportValidationWarnings)
			{
				this.SendValidationEvent(new XmlSchemaValidationException(code, args, innerException, this.sourceUriString, this.positionInfo.LineNumber, this.positionInfo.LinePosition), severity);
			}
		}

		private void SendValidationEvent(string code, string[] args, Exception innerException)
		{
			this.SendValidationEvent(new XmlSchemaValidationException(code, args, innerException, this.sourceUriString, this.positionInfo.LineNumber, this.positionInfo.LinePosition), XmlSeverityType.Error);
		}

		private void SendValidationEvent(XmlSchemaValidationException e)
		{
			this.SendValidationEvent(e, XmlSeverityType.Error);
		}

		private void SendValidationEvent(XmlSchemaException e)
		{
			this.SendValidationEvent(new XmlSchemaValidationException(e.GetRes, e.Args, e.SourceUri, e.LineNumber, e.LinePosition), XmlSeverityType.Error);
		}

		private void SendValidationEvent(string code, string msg, XmlSeverityType severity)
		{
			if (severity != XmlSeverityType.Warning || this.ReportValidationWarnings)
			{
				this.SendValidationEvent(new XmlSchemaValidationException(code, msg, this.sourceUriString, this.positionInfo.LineNumber, this.positionInfo.LinePosition), severity);
			}
		}

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

		private const int STACK_INCREMENT = 10;

		private const string Quote = "'";

		private XmlSchemaSet schemaSet;

		private XmlSchemaValidationFlags validationFlags;

		private int startIDConstraint = -1;

		private bool isRoot;

		private bool rootHasSchema;

		private bool attrValid;

		private bool checkEntity;

		private SchemaInfo compiledSchemaInfo;

		private SchemaInfo dtdSchemaInfo;

		private Hashtable validatedNamespaces;

		private HWStack validationStack;

		private ValidationState context;

		private ValidatorState currentState;

		private Hashtable attPresence;

		private SchemaAttDef wildID;

		private Hashtable IDs;

		private IdRefNode idRefListHead;

		private XmlQualifiedName contextQName;

		private string NsXs;

		private string NsXsi;

		private string NsXmlNs;

		private string NsXml;

		private XmlSchemaObject partialValidationType;

		private StringBuilder textValue;

		private ValidationEventHandler eventHandler;

		private object validationEventSender;

		private XmlNameTable nameTable;

		private IXmlLineInfo positionInfo;

		private IXmlLineInfo dummyPositionInfo;

		private XmlResolver xmlResolver;

		private Uri sourceUri;

		private string sourceUriString;

		private IXmlNamespaceResolver nsResolver;

		private XmlSchemaContentProcessing processContents = XmlSchemaContentProcessing.Strict;

		private static XmlSchemaAttribute xsiTypeSO;

		private static XmlSchemaAttribute xsiNilSO;

		private static XmlSchemaAttribute xsiSLSO;

		private static XmlSchemaAttribute xsiNoNsSLSO;

		private string xsiTypeString;

		private string xsiNilString;

		private string xsiSchemaLocationString;

		private string xsiNoNamespaceSchemaLocationString;

		private static readonly XmlSchemaDatatype dtQName = XmlSchemaDatatype.FromXmlTokenizedTypeXsd(XmlTokenizedType.QName);

		private static readonly XmlSchemaDatatype dtCDATA = XmlSchemaDatatype.FromXmlTokenizedType(XmlTokenizedType.CDATA);

		private static readonly XmlSchemaDatatype dtStringArray = XmlSchemaValidator.dtCDATA.DeriveByList(null);

		private static XmlSchemaParticle[] EmptyParticleArray = new XmlSchemaParticle[0];

		private static XmlSchemaAttribute[] EmptyAttributeArray = new XmlSchemaAttribute[0];

		private XmlCharType xmlCharType = XmlCharType.Instance;

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

		private static string[] MethodNames = new string[]
		{
			"None", "Initialize", "top-level ValidateAttribute", "top-level ValidateText or ValidateWhitespace", "ValidateElement", "ValidateAttribute", "ValidateEndOfAttributes", "ValidateText", "ValidateWhitespace", "ValidateEndElement",
			"SkipToEndElement", "EndValidation"
		};
	}
}
