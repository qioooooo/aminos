﻿using System;
using System.Collections;
using System.Globalization;
using System.Xml.Schema;

namespace System.Xml
{
	internal class XmlLoader
	{
		internal void Load(XmlDocument doc, XmlReader reader, bool preserveWhitespace)
		{
			this.doc = doc;
			if (reader.GetType() == typeof(XmlTextReader))
			{
				this.reader = ((XmlTextReader)reader).Impl;
			}
			else
			{
				this.reader = reader;
			}
			this.preserveWhitespace = preserveWhitespace;
			if (doc == null)
			{
				throw new ArgumentException(Res.GetString("Xdom_Load_NoDocument"));
			}
			if (reader == null)
			{
				throw new ArgumentException(Res.GetString("Xdom_Load_NoReader"));
			}
			doc.SetBaseURI(reader.BaseURI);
			if (reader.Settings != null && reader.Settings.ValidationType == ValidationType.Schema)
			{
				doc.Schemas = reader.Settings.Schemas;
			}
			if (this.reader.ReadState != ReadState.Interactive && !this.reader.Read())
			{
				return;
			}
			this.LoadDocSequence(doc);
		}

		private void LoadDocSequence(XmlDocument parentDoc)
		{
			XmlNode xmlNode;
			while ((xmlNode = this.LoadNode(true)) != null)
			{
				parentDoc.AppendChildForLoad(xmlNode, parentDoc);
				if (!this.reader.Read())
				{
					return;
				}
			}
		}

		internal XmlNode ReadCurrentNode(XmlDocument doc, XmlReader reader)
		{
			this.doc = doc;
			this.reader = reader;
			this.preserveWhitespace = true;
			if (doc == null)
			{
				throw new ArgumentException(Res.GetString("Xdom_Load_NoDocument"));
			}
			if (reader == null)
			{
				throw new ArgumentException(Res.GetString("Xdom_Load_NoReader"));
			}
			if (reader.ReadState == ReadState.Initial)
			{
				reader.Read();
			}
			if (reader.ReadState == ReadState.Interactive)
			{
				XmlNode xmlNode = this.LoadNode(true);
				if (xmlNode.NodeType != XmlNodeType.Attribute)
				{
					reader.Read();
				}
				return xmlNode;
			}
			return null;
		}

		private XmlNode LoadNode(bool skipOverWhitespace)
		{
			XmlReader xmlReader = this.reader;
			XmlNode xmlNode = null;
			for (;;)
			{
				XmlNode xmlNode2;
				switch (xmlReader.NodeType)
				{
				case XmlNodeType.Element:
				{
					bool isEmptyElement = xmlReader.IsEmptyElement;
					XmlElement xmlElement = this.doc.CreateElement(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI);
					xmlElement.IsEmpty = isEmptyElement;
					if (xmlReader.MoveToFirstAttribute())
					{
						XmlAttributeCollection attributes = xmlElement.Attributes;
						do
						{
							XmlAttribute xmlAttribute = this.LoadAttributeNode();
							attributes.Append(xmlAttribute);
						}
						while (xmlReader.MoveToNextAttribute());
						xmlReader.MoveToElement();
					}
					if (!isEmptyElement)
					{
						if (xmlNode != null)
						{
							xmlNode.AppendChildForLoad(xmlElement, this.doc);
						}
						xmlNode = xmlElement;
						goto IL_025B;
					}
					IXmlSchemaInfo xmlSchemaInfo = xmlReader.SchemaInfo;
					if (xmlSchemaInfo != null)
					{
						xmlElement.XmlName = this.doc.AddXmlName(xmlElement.Prefix, xmlElement.LocalName, xmlElement.NamespaceURI, xmlSchemaInfo);
					}
					xmlNode2 = xmlElement;
					goto IL_0244;
				}
				case XmlNodeType.Attribute:
					xmlNode2 = this.LoadAttributeNode();
					goto IL_0244;
				case XmlNodeType.Text:
					xmlNode2 = this.doc.CreateTextNode(xmlReader.Value);
					goto IL_0244;
				case XmlNodeType.CDATA:
					xmlNode2 = this.doc.CreateCDataSection(xmlReader.Value);
					goto IL_0244;
				case XmlNodeType.EntityReference:
					xmlNode2 = this.LoadEntityReferenceNode(false);
					goto IL_0244;
				case XmlNodeType.ProcessingInstruction:
					xmlNode2 = this.doc.CreateProcessingInstruction(xmlReader.Name, xmlReader.Value);
					goto IL_0244;
				case XmlNodeType.Comment:
					xmlNode2 = this.doc.CreateComment(xmlReader.Value);
					goto IL_0244;
				case XmlNodeType.DocumentType:
					xmlNode2 = this.LoadDocumentTypeNode();
					goto IL_0244;
				case XmlNodeType.Whitespace:
					if (this.preserveWhitespace)
					{
						xmlNode2 = this.doc.CreateWhitespace(xmlReader.Value);
						goto IL_0244;
					}
					if (xmlNode == null && !skipOverWhitespace)
					{
						goto Block_13;
					}
					goto IL_025B;
				case XmlNodeType.SignificantWhitespace:
					xmlNode2 = this.doc.CreateSignificantWhitespace(xmlReader.Value);
					goto IL_0244;
				case XmlNodeType.EndElement:
				{
					if (xmlNode == null)
					{
						goto Block_7;
					}
					IXmlSchemaInfo xmlSchemaInfo = xmlReader.SchemaInfo;
					if (xmlSchemaInfo != null)
					{
						XmlElement xmlElement = xmlNode as XmlElement;
						if (xmlElement != null)
						{
							xmlElement.XmlName = this.doc.AddXmlName(xmlElement.Prefix, xmlElement.LocalName, xmlElement.NamespaceURI, xmlSchemaInfo);
						}
					}
					if (xmlNode.ParentNode == null)
					{
						return xmlNode;
					}
					xmlNode = xmlNode.ParentNode;
					goto IL_025B;
				}
				case XmlNodeType.EndEntity:
					goto IL_0178;
				case XmlNodeType.XmlDeclaration:
					xmlNode2 = this.LoadDeclarationNode();
					goto IL_0244;
				}
				break;
				IL_025B:
				if (!xmlReader.Read())
				{
					goto Block_15;
				}
				continue;
				IL_0244:
				if (xmlNode != null)
				{
					xmlNode.AppendChildForLoad(xmlNode2, this.doc);
					goto IL_025B;
				}
				return xmlNode2;
			}
			goto IL_0238;
			Block_7:
			return null;
			IL_0178:
			return null;
			Block_13:
			return null;
			IL_0238:
			throw XmlLoader.UnexpectedNodeType(xmlReader.NodeType);
			Block_15:
			if (xmlNode != null)
			{
				while (xmlNode.ParentNode != null)
				{
					xmlNode = xmlNode.ParentNode;
				}
			}
			return xmlNode;
		}

		private XmlAttribute LoadAttributeNode()
		{
			XmlReader xmlReader = this.reader;
			if (xmlReader.IsDefault)
			{
				return this.LoadDefaultAttribute();
			}
			XmlAttribute xmlAttribute = this.doc.CreateAttribute(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI);
			IXmlSchemaInfo schemaInfo = xmlReader.SchemaInfo;
			if (schemaInfo != null)
			{
				xmlAttribute.XmlName = this.doc.AddAttrXmlName(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, schemaInfo);
			}
			while (xmlReader.ReadAttributeValue())
			{
				XmlNode xmlNode;
				switch (xmlReader.NodeType)
				{
				case XmlNodeType.Text:
					xmlNode = this.doc.CreateTextNode(xmlReader.Value);
					break;
				case XmlNodeType.CDATA:
					goto IL_00EC;
				case XmlNodeType.EntityReference:
					xmlNode = this.doc.CreateEntityReference(xmlReader.LocalName);
					if (xmlReader.CanResolveEntity)
					{
						xmlReader.ResolveEntity();
						this.LoadAttributeValue(xmlNode, false);
						if (xmlNode.FirstChild == null)
						{
							xmlNode.AppendChildForLoad(this.doc.CreateTextNode(""), this.doc);
						}
					}
					break;
				default:
					goto IL_00EC;
				}
				xmlAttribute.AppendChildForLoad(xmlNode, this.doc);
				continue;
				IL_00EC:
				throw XmlLoader.UnexpectedNodeType(xmlReader.NodeType);
			}
			return xmlAttribute;
		}

		private XmlAttribute LoadDefaultAttribute()
		{
			XmlReader xmlReader = this.reader;
			XmlAttribute xmlAttribute = this.doc.CreateDefaultAttribute(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI);
			IXmlSchemaInfo schemaInfo = xmlReader.SchemaInfo;
			if (schemaInfo != null)
			{
				xmlAttribute.XmlName = this.doc.AddAttrXmlName(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, schemaInfo);
			}
			this.LoadAttributeValue(xmlAttribute, false);
			XmlUnspecifiedAttribute xmlUnspecifiedAttribute = xmlAttribute as XmlUnspecifiedAttribute;
			if (xmlUnspecifiedAttribute != null)
			{
				xmlUnspecifiedAttribute.SetSpecified(false);
			}
			return xmlAttribute;
		}

		private void LoadAttributeValue(XmlNode parent, bool direct)
		{
			XmlReader xmlReader = this.reader;
			while (xmlReader.ReadAttributeValue())
			{
				XmlNodeType nodeType = xmlReader.NodeType;
				XmlNode xmlNode;
				switch (nodeType)
				{
				case XmlNodeType.Text:
					xmlNode = (direct ? new XmlText(xmlReader.Value, this.doc) : this.doc.CreateTextNode(xmlReader.Value));
					break;
				case XmlNodeType.CDATA:
					goto IL_00DD;
				case XmlNodeType.EntityReference:
					xmlNode = (direct ? new XmlEntityReference(this.reader.LocalName, this.doc) : this.doc.CreateEntityReference(this.reader.LocalName));
					if (xmlReader.CanResolveEntity)
					{
						xmlReader.ResolveEntity();
						this.LoadAttributeValue(xmlNode, direct);
						if (xmlNode.FirstChild == null)
						{
							xmlNode.AppendChildForLoad(direct ? new XmlText("") : this.doc.CreateTextNode(""), this.doc);
						}
					}
					break;
				default:
					if (nodeType != XmlNodeType.EndEntity)
					{
						goto IL_00DD;
					}
					return;
				}
				parent.AppendChildForLoad(xmlNode, this.doc);
				continue;
				IL_00DD:
				throw XmlLoader.UnexpectedNodeType(xmlReader.NodeType);
			}
		}

		private XmlEntityReference LoadEntityReferenceNode(bool direct)
		{
			XmlEntityReference xmlEntityReference = (direct ? new XmlEntityReference(this.reader.Name, this.doc) : this.doc.CreateEntityReference(this.reader.Name));
			if (this.reader.CanResolveEntity)
			{
				this.reader.ResolveEntity();
				while (this.reader.Read() && this.reader.NodeType != XmlNodeType.EndEntity)
				{
					XmlNode xmlNode = (direct ? this.LoadNodeDirect() : this.LoadNode(false));
					if (xmlNode != null)
					{
						xmlEntityReference.AppendChildForLoad(xmlNode, this.doc);
					}
				}
				if (xmlEntityReference.LastChild == null)
				{
					xmlEntityReference.AppendChildForLoad(this.doc.CreateTextNode(""), this.doc);
				}
			}
			return xmlEntityReference;
		}

		private XmlDeclaration LoadDeclarationNode()
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			while (this.reader.MoveToNextAttribute())
			{
				string name;
				if ((name = this.reader.Name) != null)
				{
					if (!(name == "version"))
					{
						if (!(name == "encoding"))
						{
							if (name == "standalone")
							{
								text3 = this.reader.Value;
							}
						}
						else
						{
							text2 = this.reader.Value;
						}
					}
					else
					{
						text = this.reader.Value;
					}
				}
			}
			if (text == null)
			{
				XmlLoader.ParseXmlDeclarationValue(this.reader.Value, out text, out text2, out text3);
			}
			return this.doc.CreateXmlDeclaration(text, text2, text3);
		}

		private XmlDocumentType LoadDocumentTypeNode()
		{
			string text = null;
			string text2 = null;
			string value = this.reader.Value;
			string localName = this.reader.LocalName;
			while (this.reader.MoveToNextAttribute())
			{
				string name;
				if ((name = this.reader.Name) != null)
				{
					if (!(name == "PUBLIC"))
					{
						if (name == "SYSTEM")
						{
							text2 = this.reader.Value;
						}
					}
					else
					{
						text = this.reader.Value;
					}
				}
			}
			XmlDocumentType xmlDocumentType = this.doc.CreateDocumentType(localName, text, text2, value);
			SchemaInfo dtdSchemaInfo = XmlReader.GetDtdSchemaInfo(this.reader);
			if (dtdSchemaInfo != null)
			{
				this.LoadDocumentType(dtdSchemaInfo, xmlDocumentType);
			}
			else
			{
				this.ParseDocumentType(xmlDocumentType);
			}
			return xmlDocumentType;
		}

		private XmlNode LoadNodeDirect()
		{
			XmlReader xmlReader = this.reader;
			XmlNode xmlNode = null;
			for (;;)
			{
				XmlNode xmlNode2;
				switch (xmlReader.NodeType)
				{
				case XmlNodeType.Element:
				{
					bool isEmptyElement = this.reader.IsEmptyElement;
					XmlElement xmlElement = new XmlElement(this.reader.Prefix, this.reader.LocalName, this.reader.NamespaceURI, this.doc);
					xmlElement.IsEmpty = isEmptyElement;
					if (this.reader.MoveToFirstAttribute())
					{
						XmlAttributeCollection attributes = xmlElement.Attributes;
						do
						{
							XmlAttribute xmlAttribute = this.LoadAttributeNodeDirect();
							attributes.Append(xmlAttribute);
						}
						while (xmlReader.MoveToNextAttribute());
					}
					if (!isEmptyElement)
					{
						xmlNode.AppendChildForLoad(xmlElement, this.doc);
						xmlNode = xmlElement;
						goto IL_01FC;
					}
					xmlNode2 = xmlElement;
					goto IL_01E7;
				}
				case XmlNodeType.Attribute:
					xmlNode2 = this.LoadAttributeNodeDirect();
					goto IL_01E7;
				case XmlNodeType.Text:
					xmlNode2 = new XmlText(this.reader.Value, this.doc);
					goto IL_01E7;
				case XmlNodeType.CDATA:
					xmlNode2 = new XmlCDataSection(this.reader.Value, this.doc);
					goto IL_01E7;
				case XmlNodeType.EntityReference:
					xmlNode2 = this.LoadEntityReferenceNode(true);
					goto IL_01E7;
				case XmlNodeType.ProcessingInstruction:
					xmlNode2 = new XmlProcessingInstruction(this.reader.Name, this.reader.Value, this.doc);
					goto IL_01E7;
				case XmlNodeType.Comment:
					xmlNode2 = new XmlComment(this.reader.Value, this.doc);
					goto IL_01E7;
				case XmlNodeType.Whitespace:
					if (this.preserveWhitespace)
					{
						xmlNode2 = new XmlWhitespace(this.reader.Value, this.doc);
						goto IL_01E7;
					}
					goto IL_01FC;
				case XmlNodeType.SignificantWhitespace:
					xmlNode2 = new XmlSignificantWhitespace(this.reader.Value, this.doc);
					goto IL_01E7;
				case XmlNodeType.EndElement:
					if (xmlNode.ParentNode == null)
					{
						return xmlNode;
					}
					xmlNode = xmlNode.ParentNode;
					goto IL_01FC;
				case XmlNodeType.EndEntity:
					goto IL_01FC;
				}
				break;
				IL_01FC:
				if (!xmlReader.Read())
				{
					goto Block_7;
				}
				continue;
				IL_01E7:
				if (xmlNode != null)
				{
					xmlNode.AppendChildForLoad(xmlNode2, this.doc);
					goto IL_01FC;
				}
				return xmlNode2;
			}
			throw XmlLoader.UnexpectedNodeType(this.reader.NodeType);
			Block_7:
			return null;
		}

		private XmlAttribute LoadAttributeNodeDirect()
		{
			XmlReader xmlReader = this.reader;
			if (xmlReader.IsDefault)
			{
				XmlUnspecifiedAttribute xmlUnspecifiedAttribute = new XmlUnspecifiedAttribute(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI, this.doc);
				this.LoadAttributeValue(xmlUnspecifiedAttribute, true);
				xmlUnspecifiedAttribute.SetSpecified(false);
				return xmlUnspecifiedAttribute;
			}
			XmlAttribute xmlAttribute = new XmlAttribute(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI, this.doc);
			this.LoadAttributeValue(xmlAttribute, true);
			return xmlAttribute;
		}

		internal void ParseDocumentType(XmlDocumentType dtNode)
		{
			XmlDocument ownerDocument = dtNode.OwnerDocument;
			if (ownerDocument.HasSetResolver)
			{
				this.ParseDocumentType(dtNode, true, ownerDocument.GetResolver());
				return;
			}
			this.ParseDocumentType(dtNode, false, null);
		}

		private void ParseDocumentType(XmlDocumentType dtNode, bool bUseResolver, XmlResolver resolver)
		{
			this.doc = dtNode.OwnerDocument;
			XmlNameTable nameTable = this.doc.NameTable;
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
			SchemaInfo schemaInfo = DtdParser.Parse(nameTable, xmlNamespaceManager, dtNode.ParseWithNamespaces, this.doc.BaseURI, dtNode.Name, dtNode.PublicId, dtNode.SystemId, dtNode.InternalSubset, bUseResolver, resolver);
			this.LoadDocumentType(schemaInfo, dtNode);
		}

		private void LoadDocumentType(SchemaInfo schInfo, XmlDocumentType dtNode)
		{
			dtNode.DtdSchemaInfo = schInfo;
			if (schInfo != null)
			{
				this.doc.DtdSchemaInfo = schInfo;
				if (schInfo.Notations != null)
				{
					foreach (object obj in schInfo.Notations.Values)
					{
						SchemaNotation schemaNotation = (SchemaNotation)obj;
						dtNode.Notations.SetNamedItem(new XmlNotation(schemaNotation.Name.Name, schemaNotation.Pubid, schemaNotation.SystemLiteral, this.doc));
					}
				}
				if (schInfo.GeneralEntities != null)
				{
					foreach (object obj2 in schInfo.GeneralEntities.Values)
					{
						SchemaEntity schemaEntity = (SchemaEntity)obj2;
						XmlEntity xmlEntity = new XmlEntity(schemaEntity.Name.Name, schemaEntity.Text, schemaEntity.Pubid, schemaEntity.Url, schemaEntity.NData.IsEmpty ? null : schemaEntity.NData.Name, this.doc);
						xmlEntity.SetBaseURI(schemaEntity.DeclaredURI);
						dtNode.Entities.SetNamedItem(xmlEntity);
					}
				}
				if (schInfo.ParameterEntities != null)
				{
					foreach (object obj3 in schInfo.ParameterEntities.Values)
					{
						SchemaEntity schemaEntity2 = (SchemaEntity)obj3;
						XmlEntity xmlEntity2 = new XmlEntity(schemaEntity2.Name.Name, schemaEntity2.Text, schemaEntity2.Pubid, schemaEntity2.Url, schemaEntity2.NData.IsEmpty ? null : schemaEntity2.NData.Name, this.doc);
						xmlEntity2.SetBaseURI(schemaEntity2.DeclaredURI);
						dtNode.Entities.SetNamedItem(xmlEntity2);
					}
				}
				this.doc.Entities = dtNode.Entities;
				IDictionaryEnumerator enumerator4 = schInfo.ElementDecls.GetEnumerator();
				if (enumerator4 != null)
				{
					enumerator4.Reset();
					while (enumerator4.MoveNext())
					{
						SchemaElementDecl schemaElementDecl = (SchemaElementDecl)enumerator4.Value;
						if (schemaElementDecl.AttDefs != null)
						{
							IDictionaryEnumerator enumerator5 = schemaElementDecl.AttDefs.GetEnumerator();
							while (enumerator5.MoveNext())
							{
								SchemaAttDef schemaAttDef = (SchemaAttDef)enumerator5.Value;
								if (schemaAttDef.Datatype.TokenizedType == XmlTokenizedType.ID)
								{
									this.doc.AddIdInfo(this.doc.AddXmlName(schemaElementDecl.Prefix, schemaElementDecl.Name.Name, string.Empty, null), this.doc.AddAttrXmlName(schemaAttDef.Prefix, schemaAttDef.Name.Name, string.Empty, null));
									break;
								}
							}
						}
					}
				}
			}
		}

		private XmlParserContext GetContext(XmlNode node)
		{
			string text = null;
			XmlSpace xmlSpace = XmlSpace.None;
			XmlDocumentType documentType = this.doc.DocumentType;
			string baseURI = this.doc.BaseURI;
			Hashtable hashtable = new Hashtable();
			XmlNameTable nameTable = this.doc.NameTable;
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
			bool flag = false;
			while (node != null && node != this.doc)
			{
				if (node is XmlElement && ((XmlElement)node).HasAttributes)
				{
					xmlNamespaceManager.PushScope();
					foreach (object obj in ((XmlElement)node).Attributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.Prefix == this.doc.strXmlns && !hashtable.Contains(xmlAttribute.LocalName))
						{
							hashtable.Add(xmlAttribute.LocalName, xmlAttribute.LocalName);
							xmlNamespaceManager.AddNamespace(xmlAttribute.LocalName, xmlAttribute.Value);
						}
						else if (!flag && xmlAttribute.Prefix.Length == 0 && xmlAttribute.LocalName == this.doc.strXmlns)
						{
							xmlNamespaceManager.AddNamespace(string.Empty, xmlAttribute.Value);
							flag = true;
						}
						else if (xmlSpace == XmlSpace.None && xmlAttribute.Prefix == this.doc.strXml && xmlAttribute.LocalName == this.doc.strSpace)
						{
							if (xmlAttribute.Value == "default")
							{
								xmlSpace = XmlSpace.Default;
							}
							else if (xmlAttribute.Value == "preserve")
							{
								xmlSpace = XmlSpace.Preserve;
							}
						}
						else if (text == null && xmlAttribute.Prefix == this.doc.strXml && xmlAttribute.LocalName == this.doc.strLang)
						{
							text = xmlAttribute.Value;
						}
					}
				}
				node = node.ParentNode;
			}
			return new XmlParserContext(nameTable, xmlNamespaceManager, (documentType == null) ? null : documentType.Name, (documentType == null) ? null : documentType.PublicId, (documentType == null) ? null : documentType.SystemId, (documentType == null) ? null : documentType.InternalSubset, baseURI, text, xmlSpace);
		}

		internal XmlNamespaceManager ParsePartialContent(XmlNode parentNode, string innerxmltext, XmlNodeType nt)
		{
			this.doc = parentNode.OwnerDocument;
			XmlParserContext context = this.GetContext(parentNode);
			this.reader = this.CreateInnerXmlReader(innerxmltext, nt, context, this.doc);
			try
			{
				this.preserveWhitespace = true;
				bool isLoading = this.doc.IsLoading;
				this.doc.IsLoading = true;
				if (nt == XmlNodeType.Entity)
				{
					while (this.reader.Read())
					{
						XmlNode xmlNode;
						if ((xmlNode = this.LoadNodeDirect()) == null)
						{
							break;
						}
						parentNode.AppendChildForLoad(xmlNode, this.doc);
					}
				}
				else
				{
					XmlNode xmlNode2;
					while (this.reader.Read() && (xmlNode2 = this.LoadNode(true)) != null)
					{
						parentNode.AppendChildForLoad(xmlNode2, this.doc);
					}
				}
				this.doc.IsLoading = isLoading;
			}
			finally
			{
				this.reader.Close();
			}
			return context.NamespaceManager;
		}

		internal void LoadInnerXmlElement(XmlElement node, string innerxmltext)
		{
			XmlNamespaceManager xmlNamespaceManager = this.ParsePartialContent(node, innerxmltext, XmlNodeType.Element);
			if (node.ChildNodes.Count > 0)
			{
				this.RemoveDuplicateNamespace(node, xmlNamespaceManager, false);
			}
		}

		internal void LoadInnerXmlAttribute(XmlAttribute node, string innerxmltext)
		{
			this.ParsePartialContent(node, innerxmltext, XmlNodeType.Attribute);
		}

		private void RemoveDuplicateNamespace(XmlElement elem, XmlNamespaceManager mgr, bool fCheckElemAttrs)
		{
			mgr.PushScope();
			XmlAttributeCollection attributes = elem.Attributes;
			int count = attributes.Count;
			if (fCheckElemAttrs && count > 0)
			{
				for (int i = count - 1; i >= 0; i--)
				{
					XmlAttribute xmlAttribute = attributes[i];
					if (xmlAttribute.Prefix == this.doc.strXmlns)
					{
						string text = mgr.LookupNamespace(xmlAttribute.LocalName);
						if (text != null)
						{
							if (xmlAttribute.Value == text)
							{
								elem.Attributes.RemoveNodeAt(i);
							}
						}
						else
						{
							mgr.AddNamespace(xmlAttribute.LocalName, xmlAttribute.Value);
						}
					}
					else if (xmlAttribute.Prefix.Length == 0 && xmlAttribute.LocalName == this.doc.strXmlns)
					{
						string defaultNamespace = mgr.DefaultNamespace;
						if (defaultNamespace != null)
						{
							if (xmlAttribute.Value == defaultNamespace)
							{
								elem.Attributes.RemoveNodeAt(i);
							}
						}
						else
						{
							mgr.AddNamespace(xmlAttribute.LocalName, xmlAttribute.Value);
						}
					}
				}
			}
			for (XmlNode xmlNode = elem.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				XmlElement xmlElement = xmlNode as XmlElement;
				if (xmlElement != null)
				{
					this.RemoveDuplicateNamespace(xmlElement, mgr, true);
				}
			}
			mgr.PopScope();
		}

		private string EntitizeName(string name)
		{
			return "&" + name + ";";
		}

		internal void ExpandEntity(XmlEntity ent)
		{
			this.ParsePartialContent(ent, this.EntitizeName(ent.Name), XmlNodeType.Entity);
		}

		internal void ExpandEntityReference(XmlEntityReference eref)
		{
			this.doc = eref.OwnerDocument;
			bool isLoading = this.doc.IsLoading;
			this.doc.IsLoading = true;
			string name;
			if ((name = eref.Name) != null)
			{
				if (name == "lt")
				{
					eref.AppendChildForLoad(this.doc.CreateTextNode("<"), this.doc);
					this.doc.IsLoading = isLoading;
					return;
				}
				if (name == "gt")
				{
					eref.AppendChildForLoad(this.doc.CreateTextNode(">"), this.doc);
					this.doc.IsLoading = isLoading;
					return;
				}
				if (name == "amp")
				{
					eref.AppendChildForLoad(this.doc.CreateTextNode("&"), this.doc);
					this.doc.IsLoading = isLoading;
					return;
				}
				if (name == "apos")
				{
					eref.AppendChildForLoad(this.doc.CreateTextNode("'"), this.doc);
					this.doc.IsLoading = isLoading;
					return;
				}
				if (name == "quot")
				{
					eref.AppendChildForLoad(this.doc.CreateTextNode("\""), this.doc);
					this.doc.IsLoading = isLoading;
					return;
				}
			}
			XmlNamedNodeMap entities = this.doc.Entities;
			foreach (object obj in entities)
			{
				XmlEntity xmlEntity = (XmlEntity)obj;
				if (Ref.Equal(xmlEntity.Name, eref.Name))
				{
					this.ParsePartialContent(eref, this.EntitizeName(eref.Name), XmlNodeType.EntityReference);
					return;
				}
			}
			if (!this.doc.ActualLoadingStatus)
			{
				eref.AppendChildForLoad(this.doc.CreateTextNode(""), this.doc);
				this.doc.IsLoading = isLoading;
				return;
			}
			this.doc.IsLoading = isLoading;
			throw new XmlException("Xml_UndeclaredParEntity", eref.Name);
		}

		private XmlReader CreateInnerXmlReader(string xmlFragment, XmlNodeType nt, XmlParserContext context, XmlDocument doc)
		{
			XmlNodeType xmlNodeType = nt;
			if (xmlNodeType == XmlNodeType.Entity || xmlNodeType == XmlNodeType.EntityReference)
			{
				xmlNodeType = XmlNodeType.Element;
			}
			XmlTextReaderImpl xmlTextReaderImpl = new XmlTextReaderImpl(xmlFragment, xmlNodeType, context);
			xmlTextReaderImpl.XmlValidatingReaderCompatibilityMode = true;
			if (doc.HasSetResolver)
			{
				xmlTextReaderImpl.XmlResolver = doc.GetResolver();
			}
			if (!doc.ActualLoadingStatus)
			{
				xmlTextReaderImpl.DisableUndeclaredEntityCheck = true;
			}
			XmlDocumentType documentType = doc.DocumentType;
			if (documentType != null)
			{
				xmlTextReaderImpl.Namespaces = documentType.ParseWithNamespaces;
				if (documentType.DtdSchemaInfo != null)
				{
					xmlTextReaderImpl.DtdSchemaInfo = documentType.DtdSchemaInfo;
				}
				else
				{
					SchemaInfo schemaInfo = DtdParser.Parse(xmlTextReaderImpl, context.BaseURI, context.DocTypeName, context.PublicId, context.SystemId, context.InternalSubset);
					documentType.DtdSchemaInfo = schemaInfo;
					xmlTextReaderImpl.DtdSchemaInfo = schemaInfo;
				}
			}
			if (nt == XmlNodeType.Entity || nt == XmlNodeType.EntityReference)
			{
				xmlTextReaderImpl.Read();
				xmlTextReaderImpl.ResolveEntity();
			}
			return xmlTextReaderImpl;
		}

		internal static void ParseXmlDeclarationValue(string strValue, out string version, out string encoding, out string standalone)
		{
			version = null;
			encoding = null;
			standalone = null;
			XmlTextReaderImpl xmlTextReaderImpl = new XmlTextReaderImpl(strValue, null);
			try
			{
				xmlTextReaderImpl.Read();
				if (xmlTextReaderImpl.MoveToAttribute("version"))
				{
					version = xmlTextReaderImpl.Value;
				}
				if (xmlTextReaderImpl.MoveToAttribute("encoding"))
				{
					encoding = xmlTextReaderImpl.Value;
				}
				if (xmlTextReaderImpl.MoveToAttribute("standalone"))
				{
					standalone = xmlTextReaderImpl.Value;
				}
			}
			finally
			{
				xmlTextReaderImpl.Close();
			}
		}

		internal static Exception UnexpectedNodeType(XmlNodeType nodetype)
		{
			return new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Res.GetString("Xml_UnexpectedNodeType"), new object[] { nodetype.ToString() }));
		}

		private XmlDocument doc;

		private XmlReader reader;

		private bool preserveWhitespace;
	}
}
