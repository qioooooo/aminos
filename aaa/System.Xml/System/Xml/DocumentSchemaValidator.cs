using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000D7 RID: 215
	internal sealed class DocumentSchemaValidator : IXmlNamespaceResolver
	{
		// Token: 0x06000D23 RID: 3363 RVA: 0x0003A30C File Offset: 0x0003930C
		public DocumentSchemaValidator(XmlDocument ownerDocument, XmlSchemaSet schemas, ValidationEventHandler eventHandler)
		{
			this.schemas = schemas;
			this.eventHandler = eventHandler;
			this.document = ownerDocument;
			this.internalEventHandler = new ValidationEventHandler(this.InternalValidationCallBack);
			this.nameTable = this.document.NameTable;
			this.nsManager = new XmlNamespaceManager(this.nameTable);
			this.nodeValueGetter = new XmlValueGetter(this.GetNodeValue);
			this.psviAugmentation = true;
			this.NsXmlNs = this.nameTable.Add("http://www.w3.org/2000/xmlns/");
			this.NsXsi = this.nameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
			this.XsiType = this.nameTable.Add("type");
			this.XsiNil = this.nameTable.Add("nil");
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x0003A3D9 File Offset: 0x000393D9
		// (set) Token: 0x06000D25 RID: 3365 RVA: 0x0003A3E1 File Offset: 0x000393E1
		public bool PsviAugmentation
		{
			get
			{
				return this.psviAugmentation;
			}
			set
			{
				this.psviAugmentation = value;
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0003A3EC File Offset: 0x000393EC
		public bool Validate(XmlNode nodeToValidate)
		{
			XmlSchemaObject xmlSchemaObject = null;
			XmlSchemaValidationFlags xmlSchemaValidationFlags = XmlSchemaValidationFlags.AllowXmlAttributes;
			this.startNode = nodeToValidate;
			XmlNodeType nodeType = nodeToValidate.NodeType;
			switch (nodeType)
			{
			case XmlNodeType.Element:
			{
				IXmlSchemaInfo xmlSchemaInfo = nodeToValidate.SchemaInfo;
				XmlSchemaElement schemaElement = xmlSchemaInfo.SchemaElement;
				if (schemaElement != null)
				{
					if (!schemaElement.RefName.IsEmpty)
					{
						xmlSchemaObject = this.schemas.GlobalElements[schemaElement.QualifiedName];
						goto IL_0110;
					}
					xmlSchemaObject = schemaElement;
					goto IL_0110;
				}
				else
				{
					xmlSchemaObject = xmlSchemaInfo.SchemaType;
					if (xmlSchemaObject != null)
					{
						goto IL_0110;
					}
					if (nodeToValidate.ParentNode.NodeType == XmlNodeType.Document)
					{
						nodeToValidate = nodeToValidate.ParentNode;
						goto IL_0110;
					}
					xmlSchemaObject = this.FindSchemaInfo(nodeToValidate as XmlElement);
					if (xmlSchemaObject == null)
					{
						throw new XmlSchemaValidationException("XmlDocument_NoNodeSchemaInfo", null, nodeToValidate);
					}
					goto IL_0110;
				}
				break;
			}
			case XmlNodeType.Attribute:
				if (nodeToValidate.XPNodeType != XPathNodeType.Namespace)
				{
					xmlSchemaObject = nodeToValidate.SchemaInfo.SchemaAttribute;
					if (xmlSchemaObject != null)
					{
						goto IL_0110;
					}
					xmlSchemaObject = this.FindSchemaInfo(nodeToValidate as XmlAttribute);
					if (xmlSchemaObject == null)
					{
						throw new XmlSchemaValidationException("XmlDocument_NoNodeSchemaInfo", null, nodeToValidate);
					}
					goto IL_0110;
				}
				break;
			default:
				switch (nodeType)
				{
				case XmlNodeType.Document:
					xmlSchemaValidationFlags |= XmlSchemaValidationFlags.ProcessIdentityConstraints;
					goto IL_0110;
				case XmlNodeType.DocumentFragment:
					goto IL_0110;
				}
				break;
			}
			throw new InvalidOperationException(Res.GetString("XmlDocument_ValidateInvalidNodeType", null));
			IL_0110:
			this.isValid = true;
			this.CreateValidator(xmlSchemaObject, xmlSchemaValidationFlags);
			if (this.psviAugmentation)
			{
				if (this.schemaInfo == null)
				{
					this.schemaInfo = new XmlSchemaInfo();
				}
				this.attributeSchemaInfo = new XmlSchemaInfo();
			}
			this.ValidateNode(nodeToValidate);
			this.validator.EndValidation();
			return this.isValid;
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0003A558 File Offset: 0x00039558
		public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			IDictionary<string, string> namespacesInScope = this.nsManager.GetNamespacesInScope(scope);
			if (scope != XmlNamespaceScope.Local)
			{
				XmlNode xmlNode = this.startNode;
				while (xmlNode != null)
				{
					switch (xmlNode.NodeType)
					{
					case XmlNodeType.Element:
					{
						XmlElement xmlElement = (XmlElement)xmlNode;
						if (xmlElement.HasAttributes)
						{
							XmlAttributeCollection attributes = xmlElement.Attributes;
							for (int i = 0; i < attributes.Count; i++)
							{
								XmlAttribute xmlAttribute = attributes[i];
								if (Ref.Equal(xmlAttribute.NamespaceURI, this.document.strReservedXmlns))
								{
									if (xmlAttribute.Prefix.Length == 0)
									{
										if (!namespacesInScope.ContainsKey(string.Empty))
										{
											namespacesInScope.Add(string.Empty, xmlAttribute.Value);
										}
									}
									else if (!namespacesInScope.ContainsKey(xmlAttribute.LocalName))
									{
										namespacesInScope.Add(xmlAttribute.LocalName, xmlAttribute.Value);
									}
								}
							}
						}
						xmlNode = xmlNode.ParentNode;
						break;
					}
					case XmlNodeType.Attribute:
						xmlNode = ((XmlAttribute)xmlNode).OwnerElement;
						break;
					default:
						xmlNode = xmlNode.ParentNode;
						break;
					}
				}
			}
			return namespacesInScope;
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0003A670 File Offset: 0x00039670
		public string LookupNamespace(string prefix)
		{
			string text = this.nsManager.LookupNamespace(prefix);
			if (text == null)
			{
				text = this.startNode.GetNamespaceOfPrefixStrict(prefix);
			}
			return text;
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0003A69C File Offset: 0x0003969C
		public string LookupPrefix(string namespaceName)
		{
			string text = this.nsManager.LookupPrefix(namespaceName);
			if (text == null)
			{
				text = this.startNode.GetPrefixOfNamespaceStrict(namespaceName);
			}
			return text;
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x0003A6C7 File Offset: 0x000396C7
		private IXmlNamespaceResolver NamespaceResolver
		{
			get
			{
				if (this.startNode == this.document)
				{
					return this.nsManager;
				}
				return this;
			}
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0003A6E0 File Offset: 0x000396E0
		private void CreateValidator(XmlSchemaObject partialValidationType, XmlSchemaValidationFlags validationFlags)
		{
			this.validator = new XmlSchemaValidator(this.nameTable, this.schemas, this.NamespaceResolver, validationFlags);
			this.validator.SourceUri = XmlConvert.ToUri(this.document.BaseURI);
			this.validator.XmlResolver = null;
			this.validator.ValidationEventHandler += this.internalEventHandler;
			this.validator.ValidationEventSender = this;
			if (partialValidationType != null)
			{
				this.validator.Initialize(partialValidationType);
				return;
			}
			this.validator.Initialize();
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0003A76C File Offset: 0x0003976C
		private void ValidateNode(XmlNode node)
		{
			this.currentNode = node;
			switch (this.currentNode.NodeType)
			{
			case XmlNodeType.Element:
				this.ValidateElement();
				return;
			case XmlNodeType.Attribute:
			{
				XmlAttribute xmlAttribute = this.currentNode as XmlAttribute;
				this.validator.ValidateAttribute(xmlAttribute.LocalName, xmlAttribute.NamespaceURI, this.nodeValueGetter, this.attributeSchemaInfo);
				if (this.psviAugmentation)
				{
					xmlAttribute.XmlName = this.document.AddAttrXmlName(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, this.attributeSchemaInfo);
					return;
				}
				return;
			}
			case XmlNodeType.Text:
				this.validator.ValidateText(this.nodeValueGetter);
				return;
			case XmlNodeType.CDATA:
				this.validator.ValidateText(this.nodeValueGetter);
				return;
			case XmlNodeType.EntityReference:
			case XmlNodeType.DocumentFragment:
			{
				for (XmlNode xmlNode = node.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
				{
					this.ValidateNode(xmlNode);
				}
				return;
			}
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
				return;
			case XmlNodeType.Document:
			{
				XmlElement documentElement = ((XmlDocument)node).DocumentElement;
				if (documentElement == null)
				{
					throw new InvalidOperationException(Res.GetString("Xml_InvalidXmlDocument", new object[] { Res.GetString("Xdom_NoRootEle") }));
				}
				this.ValidateNode(documentElement);
				return;
			}
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				this.validator.ValidateWhitespace(this.nodeValueGetter);
				return;
			}
			throw new InvalidOperationException(Res.GetString("Xml_UnexpectedNodeType", new string[] { this.currentNode.NodeType.ToString() }));
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0003A900 File Offset: 0x00039900
		private void ValidateElement()
		{
			this.nsManager.PushScope();
			XmlElement xmlElement = this.currentNode as XmlElement;
			XmlAttributeCollection attributes = xmlElement.Attributes;
			string text = null;
			string text2 = null;
			for (int i = 0; i < attributes.Count; i++)
			{
				XmlAttribute xmlAttribute = attributes[i];
				string namespaceURI = xmlAttribute.NamespaceURI;
				string localName = xmlAttribute.LocalName;
				if (Ref.Equal(namespaceURI, this.NsXsi))
				{
					if (Ref.Equal(localName, this.XsiType))
					{
						text2 = xmlAttribute.Value;
					}
					else if (Ref.Equal(localName, this.XsiNil))
					{
						text = xmlAttribute.Value;
					}
				}
				else if (Ref.Equal(namespaceURI, this.NsXmlNs))
				{
					this.nsManager.AddNamespace((xmlAttribute.Prefix.Length == 0) ? string.Empty : xmlAttribute.LocalName, xmlAttribute.Value);
				}
			}
			this.validator.ValidateElement(xmlElement.LocalName, xmlElement.NamespaceURI, this.schemaInfo, text2, text, null, null);
			this.ValidateAttributes(xmlElement);
			this.validator.ValidateEndOfAttributes(this.schemaInfo);
			for (XmlNode xmlNode = xmlElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				this.ValidateNode(xmlNode);
			}
			this.currentNode = xmlElement;
			this.validator.ValidateEndElement(this.schemaInfo);
			if (this.psviAugmentation)
			{
				xmlElement.XmlName = this.document.AddXmlName(xmlElement.Prefix, xmlElement.LocalName, xmlElement.NamespaceURI, this.schemaInfo);
				if (this.schemaInfo.IsDefault)
				{
					XmlText xmlText = this.document.CreateTextNode(this.schemaInfo.SchemaElement.ElementDecl.DefaultValueRaw);
					xmlElement.AppendChild(xmlText);
				}
			}
			this.nsManager.PopScope();
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0003AAC8 File Offset: 0x00039AC8
		private void ValidateAttributes(XmlElement elementNode)
		{
			XmlAttributeCollection attributes = elementNode.Attributes;
			for (int i = 0; i < attributes.Count; i++)
			{
				XmlAttribute xmlAttribute = attributes[i];
				this.currentNode = xmlAttribute;
				if (!Ref.Equal(xmlAttribute.NamespaceURI, this.NsXmlNs))
				{
					this.validator.ValidateAttribute(xmlAttribute.LocalName, xmlAttribute.NamespaceURI, this.nodeValueGetter, this.attributeSchemaInfo);
					if (this.psviAugmentation)
					{
						xmlAttribute.XmlName = this.document.AddAttrXmlName(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, this.attributeSchemaInfo);
					}
				}
			}
			if (this.psviAugmentation)
			{
				if (this.defaultAttributes == null)
				{
					this.defaultAttributes = new ArrayList();
				}
				else
				{
					this.defaultAttributes.Clear();
				}
				this.validator.GetUnspecifiedDefaultAttributes(this.defaultAttributes);
				for (int j = 0; j < this.defaultAttributes.Count; j++)
				{
					XmlSchemaAttribute xmlSchemaAttribute = this.defaultAttributes[j] as XmlSchemaAttribute;
					XmlQualifiedName qualifiedName = xmlSchemaAttribute.QualifiedName;
					XmlAttribute xmlAttribute = this.document.CreateDefaultAttribute(this.GetDefaultPrefix(qualifiedName.Namespace), qualifiedName.Name, qualifiedName.Namespace);
					this.SetDefaultAttributeSchemaInfo(xmlSchemaAttribute);
					xmlAttribute.XmlName = this.document.AddAttrXmlName(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, this.attributeSchemaInfo);
					xmlAttribute.AppendChild(this.document.CreateTextNode(xmlSchemaAttribute.AttDef.DefaultValueRaw));
					attributes.Append(xmlAttribute);
					XmlUnspecifiedAttribute xmlUnspecifiedAttribute = xmlAttribute as XmlUnspecifiedAttribute;
					if (xmlUnspecifiedAttribute != null)
					{
						xmlUnspecifiedAttribute.SetSpecified(false);
					}
				}
			}
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0003AC74 File Offset: 0x00039C74
		private void SetDefaultAttributeSchemaInfo(XmlSchemaAttribute schemaAttribute)
		{
			this.attributeSchemaInfo.Clear();
			this.attributeSchemaInfo.IsDefault = true;
			this.attributeSchemaInfo.IsNil = false;
			this.attributeSchemaInfo.SchemaType = schemaAttribute.AttributeSchemaType;
			this.attributeSchemaInfo.SchemaAttribute = schemaAttribute;
			SchemaAttDef attDef = schemaAttribute.AttDef;
			if (attDef.Datatype.Variety == XmlSchemaDatatypeVariety.Union)
			{
				XsdSimpleValue xsdSimpleValue = attDef.DefaultValueTyped as XsdSimpleValue;
				this.attributeSchemaInfo.MemberType = xsdSimpleValue.XmlType;
			}
			this.attributeSchemaInfo.Validity = XmlSchemaValidity.Valid;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0003AD00 File Offset: 0x00039D00
		private string GetDefaultPrefix(string attributeNS)
		{
			IDictionary<string, string> namespacesInScope = this.NamespaceResolver.GetNamespacesInScope(XmlNamespaceScope.All);
			string text = null;
			attributeNS = this.nameTable.Add(attributeNS);
			foreach (KeyValuePair<string, string> keyValuePair in namespacesInScope)
			{
				string text2 = this.nameTable.Add(keyValuePair.Value);
				if (object.ReferenceEquals(text2, attributeNS))
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

		// Token: 0x06000D31 RID: 3377 RVA: 0x0003AD98 File Offset: 0x00039D98
		private object GetNodeValue()
		{
			return this.currentNode.Value;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0003ADA8 File Offset: 0x00039DA8
		private XmlSchemaObject FindSchemaInfo(XmlElement elementToValidate)
		{
			this.isPartialTreeValid = true;
			int num = 0;
			XmlNode xmlNode = elementToValidate.ParentNode;
			IXmlSchemaInfo xmlSchemaInfo;
			do
			{
				xmlSchemaInfo = xmlNode.SchemaInfo;
				if (xmlSchemaInfo.SchemaElement != null || xmlSchemaInfo.SchemaType != null)
				{
					break;
				}
				this.CheckNodeSequenceCapacity(num);
				this.nodeSequenceToValidate[num++] = xmlNode;
				xmlNode = xmlNode.ParentNode;
			}
			while (xmlNode != null);
			if (xmlNode == null)
			{
				num--;
				this.nodeSequenceToValidate[num] = null;
				return this.GetTypeFromAncestors(elementToValidate, null, num);
			}
			if (this.nodeSequenceToValidate == null)
			{
				XmlSchemaComplexType xmlSchemaComplexType = xmlSchemaInfo.SchemaType as XmlSchemaComplexType;
				if (xmlSchemaComplexType == null)
				{
					return null;
				}
				if (!xmlSchemaComplexType.HasWildCard && !xmlSchemaComplexType.HasDuplicateDecls)
				{
					return this.GetTypeFromParent(elementToValidate, xmlSchemaComplexType);
				}
				this.CheckNodeSequenceCapacity(num);
				this.nodeSequenceToValidate[num++] = xmlNode;
			}
			else
			{
				this.CheckNodeSequenceCapacity(num);
				this.nodeSequenceToValidate[num++] = xmlNode;
			}
			XmlSchemaObject xmlSchemaObject = xmlSchemaInfo.SchemaElement;
			if (xmlSchemaObject == null)
			{
				xmlSchemaObject = xmlSchemaInfo.SchemaType;
			}
			return this.GetTypeFromAncestors(elementToValidate, xmlSchemaObject, num);
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0003AE97 File Offset: 0x00039E97
		private XmlSchemaElement GetTypeFromParent(XmlElement elementToValidate, XmlSchemaComplexType parentSchemaType)
		{
			return parentSchemaType.LocalElements[new XmlQualifiedName(elementToValidate.LocalName, elementToValidate.NamespaceURI)] as XmlSchemaElement;
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0003AEBC File Offset: 0x00039EBC
		private void CheckNodeSequenceCapacity(int currentIndex)
		{
			if (this.nodeSequenceToValidate == null)
			{
				this.nodeSequenceToValidate = new XmlNode[4];
				return;
			}
			if (currentIndex >= this.nodeSequenceToValidate.Length - 1)
			{
				XmlNode[] array = new XmlNode[this.nodeSequenceToValidate.Length * 2];
				Array.Copy(this.nodeSequenceToValidate, 0, array, 0, this.nodeSequenceToValidate.Length);
				this.nodeSequenceToValidate = array;
			}
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0003AF18 File Offset: 0x00039F18
		private XmlSchemaAttribute FindSchemaInfo(XmlAttribute attributeToValidate)
		{
			XmlElement ownerElement = attributeToValidate.OwnerElement;
			XmlSchemaObject xmlSchemaObject = this.FindSchemaInfo(ownerElement);
			XmlSchemaComplexType complexType = this.GetComplexType(xmlSchemaObject);
			if (complexType == null)
			{
				return null;
			}
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(attributeToValidate.LocalName, attributeToValidate.NamespaceURI);
			XmlSchemaAttribute xmlSchemaAttribute = complexType.AttributeUses[xmlQualifiedName] as XmlSchemaAttribute;
			if (xmlSchemaAttribute == null)
			{
				XmlSchemaAnyAttribute attributeWildcard = complexType.AttributeWildcard;
				if (attributeWildcard != null && attributeWildcard.NamespaceList.Allows(xmlQualifiedName))
				{
					xmlSchemaAttribute = this.schemas.GlobalAttributes[xmlQualifiedName] as XmlSchemaAttribute;
				}
			}
			return xmlSchemaAttribute;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0003AFA0 File Offset: 0x00039FA0
		private XmlSchemaObject GetTypeFromAncestors(XmlElement elementToValidate, XmlSchemaObject ancestorType, int ancestorsCount)
		{
			this.validator = this.CreateTypeFinderValidator(ancestorType);
			this.schemaInfo = new XmlSchemaInfo();
			int num = ancestorsCount - 1;
			bool flag = this.AncestorTypeHasWildcard(ancestorType);
			for (int i = num; i >= 0; i--)
			{
				XmlNode xmlNode = this.nodeSequenceToValidate[i];
				XmlElement xmlElement = xmlNode as XmlElement;
				this.ValidateSingleElement(xmlElement, false, this.schemaInfo);
				if (!flag)
				{
					xmlElement.XmlName = this.document.AddXmlName(xmlElement.Prefix, xmlElement.LocalName, xmlElement.NamespaceURI, this.schemaInfo);
					flag = this.AncestorTypeHasWildcard(this.schemaInfo.SchemaElement);
				}
				this.validator.ValidateEndOfAttributes(null);
				if (i > 0)
				{
					this.ValidateChildrenTillNextAncestor(xmlNode, this.nodeSequenceToValidate[i - 1]);
				}
				else
				{
					this.ValidateChildrenTillNextAncestor(xmlNode, elementToValidate);
				}
			}
			this.ValidateSingleElement(elementToValidate, false, this.schemaInfo);
			XmlSchemaObject xmlSchemaObject;
			if (this.schemaInfo.SchemaElement != null)
			{
				xmlSchemaObject = this.schemaInfo.SchemaElement;
			}
			else
			{
				xmlSchemaObject = this.schemaInfo.SchemaType;
			}
			if (xmlSchemaObject == null)
			{
				if (this.validator.CurrentProcessContents == XmlSchemaContentProcessing.Skip)
				{
					if (this.isPartialTreeValid)
					{
						return XmlSchemaComplexType.AnyTypeSkip;
					}
				}
				else if (this.validator.CurrentProcessContents == XmlSchemaContentProcessing.Lax)
				{
					return XmlSchemaComplexType.AnyType;
				}
			}
			return xmlSchemaObject;
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0003B0E0 File Offset: 0x0003A0E0
		private bool AncestorTypeHasWildcard(XmlSchemaObject ancestorType)
		{
			XmlSchemaComplexType complexType = this.GetComplexType(ancestorType);
			return ancestorType != null && complexType.HasWildCard;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0003B100 File Offset: 0x0003A100
		private XmlSchemaComplexType GetComplexType(XmlSchemaObject schemaObject)
		{
			if (schemaObject == null)
			{
				return null;
			}
			XmlSchemaElement xmlSchemaElement = schemaObject as XmlSchemaElement;
			XmlSchemaComplexType xmlSchemaComplexType;
			if (xmlSchemaElement != null)
			{
				xmlSchemaComplexType = xmlSchemaElement.ElementSchemaType as XmlSchemaComplexType;
			}
			else
			{
				xmlSchemaComplexType = schemaObject as XmlSchemaComplexType;
			}
			return xmlSchemaComplexType;
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0003B134 File Offset: 0x0003A134
		private void ValidateSingleElement(XmlElement elementNode, bool skipToEnd, XmlSchemaInfo newSchemaInfo)
		{
			this.nsManager.PushScope();
			XmlAttributeCollection attributes = elementNode.Attributes;
			string text = null;
			string text2 = null;
			for (int i = 0; i < attributes.Count; i++)
			{
				XmlAttribute xmlAttribute = attributes[i];
				string namespaceURI = xmlAttribute.NamespaceURI;
				string localName = xmlAttribute.LocalName;
				if (Ref.Equal(namespaceURI, this.NsXsi))
				{
					if (Ref.Equal(localName, this.XsiType))
					{
						text2 = xmlAttribute.Value;
					}
					else if (Ref.Equal(localName, this.XsiNil))
					{
						text = xmlAttribute.Value;
					}
				}
				else if (Ref.Equal(namespaceURI, this.NsXmlNs))
				{
					this.nsManager.AddNamespace((xmlAttribute.Prefix.Length == 0) ? string.Empty : xmlAttribute.LocalName, xmlAttribute.Value);
				}
			}
			this.validator.ValidateElement(elementNode.LocalName, elementNode.NamespaceURI, newSchemaInfo, text2, text, null, null);
			if (skipToEnd)
			{
				this.validator.ValidateEndOfAttributes(newSchemaInfo);
				this.validator.SkipToEndElement(newSchemaInfo);
				this.nsManager.PopScope();
			}
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0003B24C File Offset: 0x0003A24C
		private void ValidateChildrenTillNextAncestor(XmlNode parentNode, XmlNode childToStopAt)
		{
			XmlNode xmlNode = parentNode.FirstChild;
			while (xmlNode != null)
			{
				if (xmlNode == childToStopAt)
				{
					return;
				}
				switch (xmlNode.NodeType)
				{
				case XmlNodeType.Element:
					this.ValidateSingleElement(xmlNode as XmlElement, true, null);
					break;
				case XmlNodeType.Attribute:
				case XmlNodeType.Entity:
				case XmlNodeType.Document:
				case XmlNodeType.DocumentType:
				case XmlNodeType.DocumentFragment:
				case XmlNodeType.Notation:
					goto IL_009A;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					this.validator.ValidateText(xmlNode.Value);
					break;
				case XmlNodeType.EntityReference:
					this.ValidateChildrenTillNextAncestor(xmlNode, childToStopAt);
					break;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Comment:
					break;
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					this.validator.ValidateWhitespace(xmlNode.Value);
					break;
				default:
					goto IL_009A;
				}
				xmlNode = xmlNode.NextSibling;
				continue;
				IL_009A:
				throw new InvalidOperationException(Res.GetString("Xml_UnexpectedNodeType", new string[] { this.currentNode.NodeType.ToString() }));
			}
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0003B330 File Offset: 0x0003A330
		private XmlSchemaValidator CreateTypeFinderValidator(XmlSchemaObject partialValidationType)
		{
			XmlSchemaValidator xmlSchemaValidator = new XmlSchemaValidator(this.document.NameTable, this.document.Schemas, this.nsManager, XmlSchemaValidationFlags.None);
			xmlSchemaValidator.ValidationEventHandler += this.TypeFinderCallBack;
			if (partialValidationType != null)
			{
				xmlSchemaValidator.Initialize(partialValidationType);
			}
			else
			{
				xmlSchemaValidator.Initialize();
			}
			return xmlSchemaValidator;
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x0003B385 File Offset: 0x0003A385
		private void TypeFinderCallBack(object sender, ValidationEventArgs arg)
		{
			if (arg.Severity == XmlSeverityType.Error)
			{
				this.isPartialTreeValid = false;
			}
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0003B398 File Offset: 0x0003A398
		private void InternalValidationCallBack(object sender, ValidationEventArgs arg)
		{
			if (arg.Severity == XmlSeverityType.Error)
			{
				this.isValid = false;
			}
			XmlSchemaValidationException ex = arg.Exception as XmlSchemaValidationException;
			ex.SetSourceObject(this.currentNode);
			if (this.eventHandler != null)
			{
				this.eventHandler(sender, arg);
				return;
			}
			if (arg.Severity == XmlSeverityType.Error)
			{
				throw ex;
			}
		}

		// Token: 0x04000931 RID: 2353
		private XmlSchemaValidator validator;

		// Token: 0x04000932 RID: 2354
		private XmlSchemaSet schemas;

		// Token: 0x04000933 RID: 2355
		private XmlNamespaceManager nsManager;

		// Token: 0x04000934 RID: 2356
		private XmlNameTable nameTable;

		// Token: 0x04000935 RID: 2357
		private ArrayList defaultAttributes;

		// Token: 0x04000936 RID: 2358
		private XmlValueGetter nodeValueGetter;

		// Token: 0x04000937 RID: 2359
		private XmlSchemaInfo attributeSchemaInfo;

		// Token: 0x04000938 RID: 2360
		private XmlSchemaInfo schemaInfo;

		// Token: 0x04000939 RID: 2361
		private ValidationEventHandler eventHandler;

		// Token: 0x0400093A RID: 2362
		private ValidationEventHandler internalEventHandler;

		// Token: 0x0400093B RID: 2363
		private XmlNode startNode;

		// Token: 0x0400093C RID: 2364
		private XmlNode currentNode;

		// Token: 0x0400093D RID: 2365
		private XmlDocument document;

		// Token: 0x0400093E RID: 2366
		private XmlNode[] nodeSequenceToValidate;

		// Token: 0x0400093F RID: 2367
		private bool isPartialTreeValid;

		// Token: 0x04000940 RID: 2368
		private bool psviAugmentation;

		// Token: 0x04000941 RID: 2369
		private bool isValid;

		// Token: 0x04000942 RID: 2370
		private string NsXmlNs;

		// Token: 0x04000943 RID: 2371
		private string NsXsi;

		// Token: 0x04000944 RID: 2372
		private string XsiType;

		// Token: 0x04000945 RID: 2373
		private string XsiNil;
	}
}
