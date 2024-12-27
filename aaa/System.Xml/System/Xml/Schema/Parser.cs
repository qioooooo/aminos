using System;
using System.Xml.XmlConfiguration;

namespace System.Xml.Schema
{
	// Token: 0x02000204 RID: 516
	internal sealed class Parser
	{
		// Token: 0x0600186B RID: 6251 RVA: 0x0006D5C8 File Offset: 0x0006C5C8
		public Parser(SchemaType schemaType, XmlNameTable nameTable, SchemaNames schemaNames, ValidationEventHandler eventHandler)
		{
			this.schemaType = schemaType;
			this.nameTable = nameTable;
			this.schemaNames = schemaNames;
			this.eventHandler = eventHandler;
			this.xmlResolver = XmlReaderSection.CreateDefaultResolver();
			this.processMarkup = true;
			this.dummyDocument = new XmlDocument();
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x0006D620 File Offset: 0x0006C620
		public SchemaType Parse(XmlReader reader, string targetNamespace)
		{
			this.StartParsing(reader, targetNamespace);
			while (this.ParseReaderNode() && reader.Read())
			{
			}
			return this.FinishParsing();
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x0006D640 File Offset: 0x0006C640
		public void StartParsing(XmlReader reader, string targetNamespace)
		{
			this.reader = reader;
			this.positionInfo = PositionInfo.GetPositionInfo(reader);
			this.namespaceManager = reader.NamespaceManager;
			if (this.namespaceManager == null)
			{
				this.namespaceManager = new XmlNamespaceManager(this.nameTable);
				this.isProcessNamespaces = true;
			}
			else
			{
				this.isProcessNamespaces = false;
			}
			while (reader.NodeType != XmlNodeType.Element && reader.Read())
			{
			}
			this.markupDepth = int.MaxValue;
			this.schemaXmlDepth = reader.Depth;
			SchemaType schemaType = this.schemaNames.SchemaTypeFromRoot(reader.LocalName, reader.NamespaceURI);
			string text;
			if (!this.CheckSchemaRoot(schemaType, out text))
			{
				throw new XmlSchemaException(text, reader.BaseURI, this.positionInfo.LineNumber, this.positionInfo.LinePosition);
			}
			if (this.schemaType == SchemaType.XSD)
			{
				this.schema = new XmlSchema();
				this.schema.BaseUri = new Uri(reader.BaseURI, UriKind.RelativeOrAbsolute);
				this.builder = new XsdBuilder(reader, this.namespaceManager, this.schema, this.nameTable, this.schemaNames, this.eventHandler);
				return;
			}
			this.xdrSchema = new SchemaInfo();
			this.xdrSchema.SchemaType = SchemaType.XDR;
			this.builder = new XdrBuilder(reader, this.namespaceManager, this.xdrSchema, targetNamespace, this.nameTable, this.schemaNames, this.eventHandler);
			((XdrBuilder)this.builder).XmlResolver = this.xmlResolver;
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x0006D7B4 File Offset: 0x0006C7B4
		private bool CheckSchemaRoot(SchemaType rootType, out string code)
		{
			code = null;
			if (this.schemaType == SchemaType.None)
			{
				this.schemaType = rootType;
			}
			switch (rootType)
			{
			case SchemaType.None:
			case SchemaType.DTD:
				code = "Sch_SchemaRootExpected";
				if (this.schemaType == SchemaType.XSD)
				{
					code = "Sch_XSDSchemaRootExpected";
				}
				return false;
			case SchemaType.XDR:
				if (this.schemaType == SchemaType.XSD)
				{
					code = "Sch_XSDSchemaOnly";
					return false;
				}
				if (this.schemaType != SchemaType.XDR)
				{
					code = "Sch_MixSchemaTypes";
					return false;
				}
				break;
			case SchemaType.XSD:
				if (this.schemaType != SchemaType.XSD)
				{
					code = "Sch_MixSchemaTypes";
					return false;
				}
				break;
			}
			return true;
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x0006D83D File Offset: 0x0006C83D
		public SchemaType FinishParsing()
		{
			return this.schemaType;
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001870 RID: 6256 RVA: 0x0006D845 File Offset: 0x0006C845
		public XmlSchema XmlSchema
		{
			get
			{
				return this.schema;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (set) Token: 0x06001871 RID: 6257 RVA: 0x0006D84D File Offset: 0x0006C84D
		internal XmlResolver XmlResolver
		{
			set
			{
				this.xmlResolver = value;
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06001872 RID: 6258 RVA: 0x0006D856 File Offset: 0x0006C856
		public SchemaInfo XdrSchema
		{
			get
			{
				return this.xdrSchema;
			}
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x0006D860 File Offset: 0x0006C860
		public bool ParseReaderNode()
		{
			if (this.reader.Depth > this.markupDepth)
			{
				if (this.processMarkup)
				{
					this.ProcessAppInfoDocMarkup(false);
				}
				return true;
			}
			if (this.reader.NodeType == XmlNodeType.Element)
			{
				if (this.builder.ProcessElement(this.reader.Prefix, this.reader.LocalName, this.reader.NamespaceURI))
				{
					this.namespaceManager.PushScope();
					if (this.reader.MoveToFirstAttribute())
					{
						do
						{
							this.builder.ProcessAttribute(this.reader.Prefix, this.reader.LocalName, this.reader.NamespaceURI, this.reader.Value);
							if (Ref.Equal(this.reader.NamespaceURI, this.schemaNames.NsXmlNs) && this.isProcessNamespaces)
							{
								this.namespaceManager.AddNamespace((this.reader.Prefix.Length == 0) ? string.Empty : this.reader.LocalName, this.reader.Value);
							}
						}
						while (this.reader.MoveToNextAttribute());
						this.reader.MoveToElement();
					}
					this.builder.StartChildren();
					if (this.reader.IsEmptyElement)
					{
						this.namespaceManager.PopScope();
						this.builder.EndChildren();
					}
					else if (!this.builder.IsContentParsed())
					{
						this.markupDepth = this.reader.Depth;
						this.processMarkup = true;
						if (this.annotationNSManager == null)
						{
							this.annotationNSManager = new XmlNamespaceManager(this.nameTable);
							this.xmlns = this.nameTable.Add("xmlns");
						}
						this.ProcessAppInfoDocMarkup(true);
					}
				}
				else if (!this.reader.IsEmptyElement)
				{
					this.markupDepth = this.reader.Depth;
					this.processMarkup = false;
				}
			}
			else if (this.reader.NodeType == XmlNodeType.Text)
			{
				if (!this.xmlCharType.IsOnlyWhitespace(this.reader.Value))
				{
					this.builder.ProcessCData(this.reader.Value);
				}
			}
			else if (this.reader.NodeType == XmlNodeType.EntityReference || this.reader.NodeType == XmlNodeType.SignificantWhitespace || this.reader.NodeType == XmlNodeType.CDATA)
			{
				this.builder.ProcessCData(this.reader.Value);
			}
			else if (this.reader.NodeType == XmlNodeType.EndElement)
			{
				if (this.reader.Depth == this.markupDepth)
				{
					if (this.processMarkup)
					{
						XmlNodeList childNodes = this.parentNode.ChildNodes;
						XmlNode[] array = new XmlNode[childNodes.Count];
						for (int i = 0; i < childNodes.Count; i++)
						{
							array[i] = childNodes[i];
						}
						this.builder.ProcessMarkup(array);
						this.namespaceManager.PopScope();
						this.builder.EndChildren();
					}
					this.markupDepth = int.MaxValue;
				}
				else
				{
					this.namespaceManager.PopScope();
					this.builder.EndChildren();
				}
				if (this.reader.Depth == this.schemaXmlDepth)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x0006DBAC File Offset: 0x0006CBAC
		private void ProcessAppInfoDocMarkup(bool root)
		{
			XmlNode xmlNode = null;
			switch (this.reader.NodeType)
			{
			case XmlNodeType.Element:
				this.annotationNSManager.PushScope();
				xmlNode = this.LoadElementNode(root);
				return;
			case XmlNodeType.Text:
				xmlNode = this.dummyDocument.CreateTextNode(this.reader.Value);
				break;
			case XmlNodeType.CDATA:
				xmlNode = this.dummyDocument.CreateCDataSection(this.reader.Value);
				break;
			case XmlNodeType.EntityReference:
				xmlNode = this.dummyDocument.CreateEntityReference(this.reader.Name);
				break;
			case XmlNodeType.ProcessingInstruction:
				xmlNode = this.dummyDocument.CreateProcessingInstruction(this.reader.Name, this.reader.Value);
				break;
			case XmlNodeType.Comment:
				xmlNode = this.dummyDocument.CreateComment(this.reader.Value);
				break;
			case XmlNodeType.Whitespace:
			case XmlNodeType.EndEntity:
				return;
			case XmlNodeType.SignificantWhitespace:
				xmlNode = this.dummyDocument.CreateSignificantWhitespace(this.reader.Value);
				break;
			case XmlNodeType.EndElement:
				this.annotationNSManager.PopScope();
				this.parentNode = this.parentNode.ParentNode;
				return;
			}
			this.parentNode.AppendChild(xmlNode);
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x0006DCFC File Offset: 0x0006CCFC
		private XmlElement LoadElementNode(bool root)
		{
			XmlReader xmlReader = this.reader;
			bool isEmptyElement = xmlReader.IsEmptyElement;
			XmlElement xmlElement = this.dummyDocument.CreateElement(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI);
			xmlElement.IsEmpty = isEmptyElement;
			if (root)
			{
				this.parentNode = xmlElement;
			}
			else
			{
				XmlAttributeCollection attributes = xmlElement.Attributes;
				if (xmlReader.MoveToFirstAttribute())
				{
					do
					{
						if (Ref.Equal(xmlReader.NamespaceURI, this.schemaNames.NsXmlNs))
						{
							this.annotationNSManager.AddNamespace((xmlReader.Prefix.Length == 0) ? string.Empty : this.reader.LocalName, this.reader.Value);
						}
						XmlAttribute xmlAttribute = this.LoadAttributeNode();
						attributes.Append(xmlAttribute);
					}
					while (xmlReader.MoveToNextAttribute());
				}
				xmlReader.MoveToElement();
				string text = this.annotationNSManager.LookupNamespace(xmlReader.Prefix);
				if (text == null)
				{
					XmlAttribute xmlAttribute2 = this.CreateXmlNsAttribute(xmlReader.Prefix, this.namespaceManager.LookupNamespace(xmlReader.Prefix));
					attributes.Append(xmlAttribute2);
				}
				else if (text.Length == 0)
				{
					string text2 = this.namespaceManager.LookupNamespace(xmlReader.Prefix);
					if (text2 != string.Empty)
					{
						XmlAttribute xmlAttribute3 = this.CreateXmlNsAttribute(xmlReader.Prefix, text2);
						attributes.Append(xmlAttribute3);
					}
				}
				while (xmlReader.MoveToNextAttribute())
				{
					if (xmlReader.Prefix.Length != 0 && this.annotationNSManager.LookupNamespace(xmlReader.Prefix) == null)
					{
						XmlAttribute xmlAttribute4 = this.CreateXmlNsAttribute(xmlReader.Prefix, this.namespaceManager.LookupNamespace(xmlReader.Prefix));
						attributes.Append(xmlAttribute4);
					}
				}
				xmlReader.MoveToElement();
				this.parentNode.AppendChild(xmlElement);
				if (!xmlReader.IsEmptyElement)
				{
					this.parentNode = xmlElement;
				}
			}
			return xmlElement;
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x0006DECC File Offset: 0x0006CECC
		private XmlAttribute CreateXmlNsAttribute(string prefix, string value)
		{
			XmlAttribute xmlAttribute;
			if (prefix.Length == 0)
			{
				xmlAttribute = this.dummyDocument.CreateAttribute(string.Empty, this.xmlns, "http://www.w3.org/2000/xmlns/");
			}
			else
			{
				xmlAttribute = this.dummyDocument.CreateAttribute(this.xmlns, prefix, "http://www.w3.org/2000/xmlns/");
			}
			xmlAttribute.AppendChild(this.dummyDocument.CreateTextNode(value));
			this.annotationNSManager.AddNamespace(prefix, value);
			return xmlAttribute;
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x0006DF38 File Offset: 0x0006CF38
		private XmlAttribute LoadAttributeNode()
		{
			XmlReader xmlReader = this.reader;
			XmlAttribute xmlAttribute = this.dummyDocument.CreateAttribute(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI);
			while (xmlReader.ReadAttributeValue())
			{
				switch (xmlReader.NodeType)
				{
				case XmlNodeType.Text:
					xmlAttribute.AppendChild(this.dummyDocument.CreateTextNode(xmlReader.Value));
					continue;
				case XmlNodeType.EntityReference:
					xmlAttribute.AppendChild(this.LoadEntityReferenceInAttribute());
					continue;
				}
				throw XmlLoader.UnexpectedNodeType(xmlReader.NodeType);
			}
			return xmlAttribute;
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x0006DFC8 File Offset: 0x0006CFC8
		private XmlEntityReference LoadEntityReferenceInAttribute()
		{
			XmlEntityReference xmlEntityReference = this.dummyDocument.CreateEntityReference(this.reader.LocalName);
			if (!this.reader.CanResolveEntity)
			{
				return xmlEntityReference;
			}
			this.reader.ResolveEntity();
			while (this.reader.ReadAttributeValue())
			{
				XmlNodeType nodeType = this.reader.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Text:
					xmlEntityReference.AppendChild(this.dummyDocument.CreateTextNode(this.reader.Value));
					continue;
				case XmlNodeType.CDATA:
					break;
				case XmlNodeType.EntityReference:
					xmlEntityReference.AppendChild(this.LoadEntityReferenceInAttribute());
					continue;
				default:
					if (nodeType == XmlNodeType.EndEntity)
					{
						if (xmlEntityReference.ChildNodes.Count == 0)
						{
							xmlEntityReference.AppendChild(this.dummyDocument.CreateTextNode(string.Empty));
						}
						return xmlEntityReference;
					}
					break;
				}
				throw XmlLoader.UnexpectedNodeType(this.reader.NodeType);
			}
			return xmlEntityReference;
		}

		// Token: 0x04000E57 RID: 3671
		private SchemaType schemaType;

		// Token: 0x04000E58 RID: 3672
		private XmlNameTable nameTable;

		// Token: 0x04000E59 RID: 3673
		private SchemaNames schemaNames;

		// Token: 0x04000E5A RID: 3674
		private ValidationEventHandler eventHandler;

		// Token: 0x04000E5B RID: 3675
		private XmlNamespaceManager namespaceManager;

		// Token: 0x04000E5C RID: 3676
		private XmlReader reader;

		// Token: 0x04000E5D RID: 3677
		private PositionInfo positionInfo;

		// Token: 0x04000E5E RID: 3678
		private bool isProcessNamespaces;

		// Token: 0x04000E5F RID: 3679
		private int schemaXmlDepth;

		// Token: 0x04000E60 RID: 3680
		private int markupDepth;

		// Token: 0x04000E61 RID: 3681
		private SchemaBuilder builder;

		// Token: 0x04000E62 RID: 3682
		private XmlSchema schema;

		// Token: 0x04000E63 RID: 3683
		private SchemaInfo xdrSchema;

		// Token: 0x04000E64 RID: 3684
		private XmlResolver xmlResolver;

		// Token: 0x04000E65 RID: 3685
		private XmlDocument dummyDocument;

		// Token: 0x04000E66 RID: 3686
		private bool processMarkup;

		// Token: 0x04000E67 RID: 3687
		private XmlNode parentNode;

		// Token: 0x04000E68 RID: 3688
		private XmlNamespaceManager annotationNSManager;

		// Token: 0x04000E69 RID: 3689
		private string xmlns;

		// Token: 0x04000E6A RID: 3690
		private XmlCharType xmlCharType = XmlCharType.Instance;
	}
}
