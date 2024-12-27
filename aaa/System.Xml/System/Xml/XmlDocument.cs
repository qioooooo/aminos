using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000D4 RID: 212
	public class XmlDocument : XmlNode
	{
		// Token: 0x06000C7C RID: 3196 RVA: 0x00038324 File Offset: 0x00037324
		public XmlDocument()
			: this(new XmlImplementation())
		{
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00038331 File Offset: 0x00037331
		public XmlDocument(XmlNameTable nt)
			: this(new XmlImplementation(nt))
		{
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00038340 File Offset: 0x00037340
		protected internal XmlDocument(XmlImplementation imp)
		{
			this.implementation = imp;
			this.domNameTable = new DomNameTable(this);
			XmlNameTable nameTable = this.NameTable;
			nameTable.Add(string.Empty);
			this.strDocumentName = nameTable.Add("#document");
			this.strDocumentFragmentName = nameTable.Add("#document-fragment");
			this.strCommentName = nameTable.Add("#comment");
			this.strTextName = nameTable.Add("#text");
			this.strCDataSectionName = nameTable.Add("#cdata-section");
			this.strEntityName = nameTable.Add("#entity");
			this.strID = nameTable.Add("id");
			this.strNonSignificantWhitespaceName = nameTable.Add("#whitespace");
			this.strSignificantWhitespaceName = nameTable.Add("#significant-whitespace");
			this.strXmlns = nameTable.Add("xmlns");
			this.strXml = nameTable.Add("xml");
			this.strSpace = nameTable.Add("space");
			this.strLang = nameTable.Add("lang");
			this.strReservedXmlns = nameTable.Add("http://www.w3.org/2000/xmlns/");
			this.strReservedXml = nameTable.Add("http://www.w3.org/XML/1998/namespace");
			this.strEmpty = nameTable.Add(string.Empty);
			this.baseURI = string.Empty;
			this.objLock = new object();
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x0003849F File Offset: 0x0003749F
		// (set) Token: 0x06000C80 RID: 3200 RVA: 0x000384A7 File Offset: 0x000374A7
		internal SchemaInfo DtdSchemaInfo
		{
			get
			{
				return this.schemaInfo;
			}
			set
			{
				this.schemaInfo = value;
			}
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x000384B0 File Offset: 0x000374B0
		internal static void CheckName(string name)
		{
			XmlCharType instance = XmlCharType.Instance;
			for (int i = 0; i < name.Length; i++)
			{
				if (!instance.IsNCNameChar(name[i]))
				{
					throw new XmlException("Xml_BadNameChar", XmlException.BuildCharExceptionStr(name[i]));
				}
			}
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x000384FC File Offset: 0x000374FC
		internal XmlName AddXmlName(string prefix, string localName, string namespaceURI, IXmlSchemaInfo schemaInfo)
		{
			return this.domNameTable.AddName(prefix, localName, namespaceURI, schemaInfo);
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x0003851C File Offset: 0x0003751C
		internal XmlName GetXmlName(string prefix, string localName, string namespaceURI, IXmlSchemaInfo schemaInfo)
		{
			return this.domNameTable.GetName(prefix, localName, namespaceURI, schemaInfo);
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x0003853C File Offset: 0x0003753C
		internal XmlName AddAttrXmlName(string prefix, string localName, string namespaceURI, IXmlSchemaInfo schemaInfo)
		{
			XmlName xmlName = this.AddXmlName(prefix, localName, namespaceURI, schemaInfo);
			if (!this.IsLoading)
			{
				object prefix2 = xmlName.Prefix;
				object namespaceURI2 = xmlName.NamespaceURI;
				object localName2 = xmlName.LocalName;
				if ((prefix2 == this.strXmlns || (prefix2 == this.strEmpty && localName2 == this.strXmlns)) ^ (namespaceURI2 == this.strReservedXmlns))
				{
					throw new ArgumentException(Res.GetString("Xdom_Attr_Reserved_XmlNS", new object[] { namespaceURI }));
				}
			}
			return xmlName;
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x000385BF File Offset: 0x000375BF
		internal bool AddIdInfo(XmlName eleName, XmlName attrName)
		{
			if (this.htElementIDAttrDecl == null || this.htElementIDAttrDecl[eleName] == null)
			{
				if (this.htElementIDAttrDecl == null)
				{
					this.htElementIDAttrDecl = new Hashtable();
				}
				this.htElementIDAttrDecl.Add(eleName, attrName);
				return true;
			}
			return false;
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x000385FC File Offset: 0x000375FC
		private XmlName GetIDInfoByElement_(XmlName eleName)
		{
			XmlName xmlName = this.GetXmlName(eleName.Prefix, eleName.LocalName, string.Empty, null);
			if (xmlName != null)
			{
				return (XmlName)this.htElementIDAttrDecl[xmlName];
			}
			return null;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00038638 File Offset: 0x00037638
		internal XmlName GetIDInfoByElement(XmlName eleName)
		{
			if (this.htElementIDAttrDecl == null)
			{
				return null;
			}
			return this.GetIDInfoByElement_(eleName);
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0003864C File Offset: 0x0003764C
		private WeakReference GetElement(ArrayList elementList, XmlElement elem)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in elementList)
			{
				WeakReference weakReference = (WeakReference)obj;
				if (!weakReference.IsAlive)
				{
					arrayList.Add(weakReference);
				}
				else if ((XmlElement)weakReference.Target == elem)
				{
					return weakReference;
				}
			}
			foreach (object obj2 in arrayList)
			{
				WeakReference weakReference2 = (WeakReference)obj2;
				elementList.Remove(weakReference2);
			}
			return null;
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00038718 File Offset: 0x00037718
		internal void AddElementWithId(string id, XmlElement elem)
		{
			if (this.htElementIdMap == null || !this.htElementIdMap.Contains(id))
			{
				if (this.htElementIdMap == null)
				{
					this.htElementIdMap = new Hashtable();
				}
				ArrayList arrayList = new ArrayList();
				arrayList.Add(new WeakReference(elem));
				this.htElementIdMap.Add(id, arrayList);
				return;
			}
			ArrayList arrayList2 = (ArrayList)this.htElementIdMap[id];
			if (this.GetElement(arrayList2, elem) == null)
			{
				arrayList2.Add(new WeakReference(elem));
			}
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00038798 File Offset: 0x00037798
		internal void RemoveElementWithId(string id, XmlElement elem)
		{
			if (this.htElementIdMap != null && this.htElementIdMap.Contains(id))
			{
				ArrayList arrayList = (ArrayList)this.htElementIdMap[id];
				WeakReference element = this.GetElement(arrayList, elem);
				if (element != null)
				{
					arrayList.Remove(element);
					if (arrayList.Count == 0)
					{
						this.htElementIdMap.Remove(id);
					}
				}
			}
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x000387F4 File Offset: 0x000377F4
		public override XmlNode CloneNode(bool deep)
		{
			XmlDocument xmlDocument = this.Implementation.CreateDocument();
			xmlDocument.SetBaseURI(this.baseURI);
			if (deep)
			{
				xmlDocument.ImportChildren(this, xmlDocument, deep);
			}
			return xmlDocument;
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x00038826 File Offset: 0x00037826
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.Document;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0003882A File Offset: 0x0003782A
		public override XmlNode ParentNode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x0003882D File Offset: 0x0003782D
		public virtual XmlDocumentType DocumentType
		{
			get
			{
				return (XmlDocumentType)this.FindChild(XmlNodeType.DocumentType);
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x0003883C File Offset: 0x0003783C
		internal virtual XmlDeclaration Declaration
		{
			get
			{
				if (this.HasChildNodes)
				{
					return this.FirstChild as XmlDeclaration;
				}
				return null;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000C90 RID: 3216 RVA: 0x00038860 File Offset: 0x00037860
		public XmlImplementation Implementation
		{
			get
			{
				return this.implementation;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x00038868 File Offset: 0x00037868
		public override string Name
		{
			get
			{
				return this.strDocumentName;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x00038870 File Offset: 0x00037870
		public override string LocalName
		{
			get
			{
				return this.strDocumentName;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x00038878 File Offset: 0x00037878
		public XmlElement DocumentElement
		{
			get
			{
				return (XmlElement)this.FindChild(XmlNodeType.Element);
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x00038886 File Offset: 0x00037886
		internal override bool IsContainer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x00038889 File Offset: 0x00037889
		// (set) Token: 0x06000C96 RID: 3222 RVA: 0x00038891 File Offset: 0x00037891
		internal override XmlLinkedNode LastNode
		{
			get
			{
				return this.lastChild;
			}
			set
			{
				this.lastChild = value;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0003889A File Offset: 0x0003789A
		public override XmlDocument OwnerDocument
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0003889D File Offset: 0x0003789D
		// (set) Token: 0x06000C99 RID: 3225 RVA: 0x000388BE File Offset: 0x000378BE
		public XmlSchemaSet Schemas
		{
			get
			{
				if (this.schemas == null)
				{
					this.schemas = new XmlSchemaSet(this.NameTable);
				}
				return this.schemas;
			}
			set
			{
				this.schemas = value;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x000388C7 File Offset: 0x000378C7
		internal bool CanReportValidity
		{
			get
			{
				return this.reportValidity;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x000388CF File Offset: 0x000378CF
		internal bool HasSetResolver
		{
			get
			{
				return this.bSetResolver;
			}
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x000388D7 File Offset: 0x000378D7
		internal XmlResolver GetResolver()
		{
			return this.resolver;
		}

		// Token: 0x170002EA RID: 746
		// (set) Token: 0x06000C9D RID: 3229 RVA: 0x000388E0 File Offset: 0x000378E0
		public virtual XmlResolver XmlResolver
		{
			set
			{
				if (value != null)
				{
					try
					{
						new NamedPermissionSet("FullTrust").Demand();
					}
					catch (SecurityException ex)
					{
						throw new SecurityException(Res.GetString("Xml_UntrustedCodeSettingResolver"), ex);
					}
				}
				this.resolver = value;
				if (!this.bSetResolver)
				{
					this.bSetResolver = true;
				}
				XmlDocumentType documentType = this.DocumentType;
				if (documentType != null)
				{
					documentType.DtdSchemaInfo = null;
				}
			}
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0003894C File Offset: 0x0003794C
		internal override bool IsValidChildType(XmlNodeType type)
		{
			if (type != XmlNodeType.Element)
			{
				switch (type)
				{
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Comment:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					return true;
				case XmlNodeType.DocumentType:
					if (this.DocumentType != null)
					{
						throw new InvalidOperationException(Res.GetString("Xdom_DualDocumentTypeNode"));
					}
					return true;
				case XmlNodeType.XmlDeclaration:
					if (this.Declaration != null)
					{
						throw new InvalidOperationException(Res.GetString("Xdom_DualDeclarationNode"));
					}
					return true;
				}
				return false;
			}
			if (this.DocumentElement != null)
			{
				throw new InvalidOperationException(Res.GetString("Xdom_DualDocumentElementNode"));
			}
			return true;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x000389E8 File Offset: 0x000379E8
		private bool HasNodeTypeInPrevSiblings(XmlNodeType nt, XmlNode refNode)
		{
			if (refNode == null)
			{
				return false;
			}
			XmlNode xmlNode = null;
			if (refNode.ParentNode != null)
			{
				xmlNode = refNode.ParentNode.FirstChild;
			}
			while (xmlNode != null)
			{
				if (xmlNode.NodeType == nt)
				{
					return true;
				}
				if (xmlNode == refNode)
				{
					break;
				}
				xmlNode = xmlNode.NextSibling;
			}
			return false;
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x00038A2C File Offset: 0x00037A2C
		private bool HasNodeTypeInNextSiblings(XmlNodeType nt, XmlNode refNode)
		{
			for (XmlNode xmlNode = refNode; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				if (xmlNode.NodeType == nt)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x00038A54 File Offset: 0x00037A54
		internal override bool CanInsertBefore(XmlNode newChild, XmlNode refChild)
		{
			if (refChild == null)
			{
				refChild = this.FirstChild;
			}
			if (refChild == null)
			{
				return true;
			}
			XmlNodeType nodeType = newChild.NodeType;
			if (nodeType != XmlNodeType.Element)
			{
				switch (nodeType)
				{
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Comment:
					return refChild.NodeType != XmlNodeType.XmlDeclaration;
				case XmlNodeType.Document:
					break;
				case XmlNodeType.DocumentType:
					if (refChild.NodeType != XmlNodeType.XmlDeclaration)
					{
						return !this.HasNodeTypeInPrevSiblings(XmlNodeType.Element, refChild.PreviousSibling);
					}
					break;
				default:
					if (nodeType == XmlNodeType.XmlDeclaration)
					{
						return refChild == this.FirstChild;
					}
					break;
				}
			}
			else if (refChild.NodeType != XmlNodeType.XmlDeclaration)
			{
				return !this.HasNodeTypeInNextSiblings(XmlNodeType.DocumentType, refChild);
			}
			return false;
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00038AE4 File Offset: 0x00037AE4
		internal override bool CanInsertAfter(XmlNode newChild, XmlNode refChild)
		{
			if (refChild == null)
			{
				refChild = this.LastChild;
			}
			if (refChild == null)
			{
				return true;
			}
			XmlNodeType nodeType = newChild.NodeType;
			if (nodeType != XmlNodeType.Element)
			{
				switch (nodeType)
				{
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Comment:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					return true;
				case XmlNodeType.DocumentType:
					return !this.HasNodeTypeInPrevSiblings(XmlNodeType.Element, refChild);
				}
				return false;
			}
			return !this.HasNodeTypeInNextSiblings(XmlNodeType.DocumentType, refChild.NextSibling);
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x00038B58 File Offset: 0x00037B58
		public XmlAttribute CreateAttribute(string name)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			string empty3 = string.Empty;
			XmlNode.SplitName(name, out empty, out empty2);
			this.SetDefaultNamespace(empty, empty2, ref empty3);
			return this.CreateAttribute(empty, empty2, empty3);
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00038B94 File Offset: 0x00037B94
		internal void SetDefaultNamespace(string prefix, string localName, ref string namespaceURI)
		{
			if (prefix == this.strXmlns || (prefix.Length == 0 && localName == this.strXmlns))
			{
				namespaceURI = this.strReservedXmlns;
				return;
			}
			if (prefix == this.strXml)
			{
				namespaceURI = this.strReservedXml;
			}
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00038BE4 File Offset: 0x00037BE4
		public virtual XmlCDataSection CreateCDataSection(string data)
		{
			this.fCDataNodesPresent = true;
			return new XmlCDataSection(data, this);
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x00038BF4 File Offset: 0x00037BF4
		public virtual XmlComment CreateComment(string data)
		{
			return new XmlComment(data, this);
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x00038BFD File Offset: 0x00037BFD
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		public virtual XmlDocumentType CreateDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new XmlDocumentType(name, publicId, systemId, internalSubset, this);
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00038C0A File Offset: 0x00037C0A
		public virtual XmlDocumentFragment CreateDocumentFragment()
		{
			return new XmlDocumentFragment(this);
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00038C14 File Offset: 0x00037C14
		public XmlElement CreateElement(string name)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			XmlNode.SplitName(name, out empty, out empty2);
			return this.CreateElement(empty, empty2, string.Empty);
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00038C44 File Offset: 0x00037C44
		internal void AddDefaultAttributes(XmlElement elem)
		{
			SchemaInfo dtdSchemaInfo = this.DtdSchemaInfo;
			SchemaElementDecl schemaElementDecl = this.GetSchemaElementDecl(elem);
			if (schemaElementDecl != null && schemaElementDecl.AttDefs != null)
			{
				IDictionaryEnumerator enumerator = schemaElementDecl.AttDefs.GetEnumerator();
				while (enumerator.MoveNext())
				{
					SchemaAttDef schemaAttDef = (SchemaAttDef)enumerator.Value;
					if (schemaAttDef.Presence == SchemaDeclBase.Use.Default || schemaAttDef.Presence == SchemaDeclBase.Use.Fixed)
					{
						string text = string.Empty;
						string name = schemaAttDef.Name.Name;
						string text2 = string.Empty;
						if (dtdSchemaInfo.SchemaType == SchemaType.DTD)
						{
							text = schemaAttDef.Name.Namespace;
						}
						else
						{
							text = schemaAttDef.Prefix;
							text2 = schemaAttDef.Name.Namespace;
						}
						XmlAttribute xmlAttribute = this.PrepareDefaultAttribute(schemaAttDef, text, name, text2);
						elem.SetAttributeNode(xmlAttribute);
					}
				}
			}
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00038D08 File Offset: 0x00037D08
		private SchemaElementDecl GetSchemaElementDecl(XmlElement elem)
		{
			SchemaInfo dtdSchemaInfo = this.DtdSchemaInfo;
			if (dtdSchemaInfo != null)
			{
				XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(elem.LocalName, (dtdSchemaInfo.SchemaType == SchemaType.DTD) ? elem.Prefix : elem.NamespaceURI);
				return (SchemaElementDecl)dtdSchemaInfo.ElementDecls[xmlQualifiedName];
			}
			return null;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00038D58 File Offset: 0x00037D58
		private XmlAttribute PrepareDefaultAttribute(SchemaAttDef attdef, string attrPrefix, string attrLocalname, string attrNamespaceURI)
		{
			this.SetDefaultNamespace(attrPrefix, attrLocalname, ref attrNamespaceURI);
			XmlAttribute xmlAttribute = this.CreateDefaultAttribute(attrPrefix, attrLocalname, attrNamespaceURI);
			xmlAttribute.InnerXml = attdef.DefaultValueRaw;
			XmlUnspecifiedAttribute xmlUnspecifiedAttribute = xmlAttribute as XmlUnspecifiedAttribute;
			if (xmlUnspecifiedAttribute != null)
			{
				xmlUnspecifiedAttribute.SetSpecified(false);
			}
			return xmlAttribute;
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00038D98 File Offset: 0x00037D98
		public virtual XmlEntityReference CreateEntityReference(string name)
		{
			return new XmlEntityReference(name, this);
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00038DA1 File Offset: 0x00037DA1
		public virtual XmlProcessingInstruction CreateProcessingInstruction(string target, string data)
		{
			return new XmlProcessingInstruction(target, data, this);
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00038DAB File Offset: 0x00037DAB
		public virtual XmlDeclaration CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new XmlDeclaration(version, encoding, standalone, this);
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00038DB6 File Offset: 0x00037DB6
		public virtual XmlText CreateTextNode(string text)
		{
			return new XmlText(text, this);
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00038DBF File Offset: 0x00037DBF
		public virtual XmlSignificantWhitespace CreateSignificantWhitespace(string text)
		{
			return new XmlSignificantWhitespace(text, this);
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00038DC8 File Offset: 0x00037DC8
		public override XPathNavigator CreateNavigator()
		{
			return this.CreateNavigator(this);
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x00038DD4 File Offset: 0x00037DD4
		protected internal virtual XPathNavigator CreateNavigator(XmlNode node)
		{
			switch (node.NodeType)
			{
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.SignificantWhitespace:
			{
				XmlNode xmlNode = node.ParentNode;
				if (xmlNode != null)
				{
					for (;;)
					{
						XmlNodeType xmlNodeType = xmlNode.NodeType;
						if (xmlNodeType == XmlNodeType.Attribute)
						{
							break;
						}
						if (xmlNodeType != XmlNodeType.EntityReference)
						{
							goto IL_0076;
						}
						xmlNode = xmlNode.ParentNode;
						if (xmlNode == null)
						{
							goto IL_0076;
						}
					}
					return null;
				}
				IL_0076:
				node = this.NormalizeText(node);
				break;
			}
			case XmlNodeType.EntityReference:
			case XmlNodeType.Entity:
			case XmlNodeType.DocumentType:
			case XmlNodeType.Notation:
			case XmlNodeType.XmlDeclaration:
				return null;
			case XmlNodeType.Whitespace:
			{
				XmlNode xmlNode = node.ParentNode;
				if (xmlNode != null)
				{
					for (;;)
					{
						XmlNodeType xmlNodeType = xmlNode.NodeType;
						if (xmlNodeType == XmlNodeType.Document || xmlNodeType == XmlNodeType.Attribute)
						{
							break;
						}
						if (xmlNodeType != XmlNodeType.EntityReference)
						{
							goto IL_00AB;
						}
						xmlNode = xmlNode.ParentNode;
						if (xmlNode == null)
						{
							goto IL_00AB;
						}
					}
					return null;
				}
				IL_00AB:
				node = this.NormalizeText(node);
				break;
			}
			}
			return new DocumentXPathNavigator(this, node);
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x00038E9C File Offset: 0x00037E9C
		internal static bool IsTextNode(XmlNodeType nt)
		{
			switch (nt)
			{
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				break;
			default:
				switch (nt)
				{
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					break;
				default:
					return false;
				}
				break;
			}
			return true;
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00038ED4 File Offset: 0x00037ED4
		private XmlNode NormalizeText(XmlNode n)
		{
			XmlNode xmlNode = null;
			while (XmlDocument.IsTextNode(n.NodeType))
			{
				xmlNode = n;
				n = n.PreviousSibling;
				if (n == null)
				{
					XmlNode xmlNode2 = xmlNode;
					while (xmlNode2.ParentNode != null && xmlNode2.ParentNode.NodeType == XmlNodeType.EntityReference)
					{
						if (xmlNode2.ParentNode.PreviousSibling != null)
						{
							n = xmlNode2.ParentNode.PreviousSibling;
							break;
						}
						xmlNode2 = xmlNode2.ParentNode;
						if (xmlNode2 == null)
						{
							break;
						}
					}
				}
				if (n == null)
				{
					break;
				}
				while (n.NodeType == XmlNodeType.EntityReference)
				{
					n = n.LastChild;
				}
			}
			return xmlNode;
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x00038F54 File Offset: 0x00037F54
		public virtual XmlWhitespace CreateWhitespace(string text)
		{
			return new XmlWhitespace(text, this);
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00038F5D File Offset: 0x00037F5D
		public virtual XmlNodeList GetElementsByTagName(string name)
		{
			return new XmlElementList(this, name);
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x00038F68 File Offset: 0x00037F68
		public XmlAttribute CreateAttribute(string qualifiedName, string namespaceURI)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			XmlNode.SplitName(qualifiedName, out empty, out empty2);
			return this.CreateAttribute(empty, empty2, namespaceURI);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x00038F94 File Offset: 0x00037F94
		public XmlElement CreateElement(string qualifiedName, string namespaceURI)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			XmlNode.SplitName(qualifiedName, out empty, out empty2);
			return this.CreateElement(empty, empty2, namespaceURI);
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x00038FC0 File Offset: 0x00037FC0
		public virtual XmlNodeList GetElementsByTagName(string localName, string namespaceURI)
		{
			return new XmlElementList(this, localName, namespaceURI);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x00038FCC File Offset: 0x00037FCC
		public virtual XmlElement GetElementById(string elementId)
		{
			if (this.htElementIdMap != null)
			{
				ArrayList arrayList = (ArrayList)this.htElementIdMap[elementId];
				if (arrayList != null)
				{
					foreach (object obj in arrayList)
					{
						WeakReference weakReference = (WeakReference)obj;
						XmlElement xmlElement = (XmlElement)weakReference.Target;
						if (xmlElement != null && xmlElement.IsConnected())
						{
							return xmlElement;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0003905C File Offset: 0x0003805C
		public virtual XmlNode ImportNode(XmlNode node, bool deep)
		{
			return this.ImportNodeInternal(node, deep);
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x00039068 File Offset: 0x00038068
		private XmlNode ImportNodeInternal(XmlNode node, bool deep)
		{
			if (node == null)
			{
				throw new InvalidOperationException(Res.GetString("Xdom_Import_NullNode"));
			}
			switch (node.NodeType)
			{
			case XmlNodeType.Element:
			{
				XmlNode xmlNode = this.CreateElement(node.Prefix, node.LocalName, node.NamespaceURI);
				this.ImportAttributes(node, xmlNode);
				if (deep)
				{
					this.ImportChildren(node, xmlNode, deep);
					return xmlNode;
				}
				return xmlNode;
			}
			case XmlNodeType.Attribute:
			{
				XmlNode xmlNode = this.CreateAttribute(node.Prefix, node.LocalName, node.NamespaceURI);
				this.ImportChildren(node, xmlNode, true);
				return xmlNode;
			}
			case XmlNodeType.Text:
				return this.CreateTextNode(node.Value);
			case XmlNodeType.CDATA:
				return this.CreateCDataSection(node.Value);
			case XmlNodeType.EntityReference:
				return this.CreateEntityReference(node.Name);
			case XmlNodeType.ProcessingInstruction:
				return this.CreateProcessingInstruction(node.Name, node.Value);
			case XmlNodeType.Comment:
				return this.CreateComment(node.Value);
			case XmlNodeType.DocumentType:
			{
				XmlDocumentType xmlDocumentType = (XmlDocumentType)node;
				return this.CreateDocumentType(xmlDocumentType.Name, xmlDocumentType.PublicId, xmlDocumentType.SystemId, xmlDocumentType.InternalSubset);
			}
			case XmlNodeType.DocumentFragment:
			{
				XmlNode xmlNode = this.CreateDocumentFragment();
				if (deep)
				{
					this.ImportChildren(node, xmlNode, deep);
					return xmlNode;
				}
				return xmlNode;
			}
			case XmlNodeType.Whitespace:
				return this.CreateWhitespace(node.Value);
			case XmlNodeType.SignificantWhitespace:
				return this.CreateSignificantWhitespace(node.Value);
			case XmlNodeType.XmlDeclaration:
			{
				XmlDeclaration xmlDeclaration = (XmlDeclaration)node;
				return this.CreateXmlDeclaration(xmlDeclaration.Version, xmlDeclaration.Encoding, xmlDeclaration.Standalone);
			}
			}
			throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Res.GetString("Xdom_Import"), new object[] { node.NodeType.ToString() }));
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x00039254 File Offset: 0x00038254
		private void ImportAttributes(XmlNode fromElem, XmlNode toElem)
		{
			int count = fromElem.Attributes.Count;
			for (int i = 0; i < count; i++)
			{
				if (fromElem.Attributes[i].Specified)
				{
					toElem.Attributes.SetNamedItem(this.ImportNodeInternal(fromElem.Attributes[i], true));
				}
			}
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x000392AC File Offset: 0x000382AC
		private void ImportChildren(XmlNode fromNode, XmlNode toNode, bool deep)
		{
			for (XmlNode xmlNode = fromNode.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				toNode.AppendChild(this.ImportNodeInternal(xmlNode, deep));
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x000392DB File Offset: 0x000382DB
		public XmlNameTable NameTable
		{
			get
			{
				return this.implementation.NameTable;
			}
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x000392E8 File Offset: 0x000382E8
		public virtual XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI)
		{
			return new XmlAttribute(this.AddAttrXmlName(prefix, localName, namespaceURI, null), this);
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x000392FA File Offset: 0x000382FA
		protected internal virtual XmlAttribute CreateDefaultAttribute(string prefix, string localName, string namespaceURI)
		{
			return new XmlUnspecifiedAttribute(prefix, localName, namespaceURI, this);
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x00039308 File Offset: 0x00038308
		public virtual XmlElement CreateElement(string prefix, string localName, string namespaceURI)
		{
			XmlElement xmlElement = new XmlElement(this.AddXmlName(prefix, localName, namespaceURI, null), true, this);
			if (!this.IsLoading)
			{
				this.AddDefaultAttributes(xmlElement);
			}
			return xmlElement;
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x00039337 File Offset: 0x00038337
		// (set) Token: 0x06000CC5 RID: 3269 RVA: 0x0003933F File Offset: 0x0003833F
		public bool PreserveWhitespace
		{
			get
			{
				return this.preserveWhitespace;
			}
			set
			{
				this.preserveWhitespace = value;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x00039348 File Offset: 0x00038348
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0003934B File Offset: 0x0003834B
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x00039367 File Offset: 0x00038367
		internal XmlNamedNodeMap Entities
		{
			get
			{
				if (this.entities == null)
				{
					this.entities = new XmlNamedNodeMap(this);
				}
				return this.entities;
			}
			set
			{
				this.entities = value;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x00039370 File Offset: 0x00038370
		// (set) Token: 0x06000CCA RID: 3274 RVA: 0x00039378 File Offset: 0x00038378
		internal bool IsLoading
		{
			get
			{
				return this.isLoading;
			}
			set
			{
				this.isLoading = value;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x00039381 File Offset: 0x00038381
		// (set) Token: 0x06000CCC RID: 3276 RVA: 0x00039389 File Offset: 0x00038389
		internal bool ActualLoadingStatus
		{
			get
			{
				return this.actualLoadingStatus;
			}
			set
			{
				this.actualLoadingStatus = value;
			}
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00039394 File Offset: 0x00038394
		public virtual XmlNode CreateNode(XmlNodeType type, string prefix, string name, string namespaceURI)
		{
			switch (type)
			{
			case XmlNodeType.Element:
				if (prefix != null)
				{
					return this.CreateElement(prefix, name, namespaceURI);
				}
				return this.CreateElement(name, namespaceURI);
			case XmlNodeType.Attribute:
				if (prefix != null)
				{
					return this.CreateAttribute(prefix, name, namespaceURI);
				}
				return this.CreateAttribute(name, namespaceURI);
			case XmlNodeType.Text:
				return this.CreateTextNode(string.Empty);
			case XmlNodeType.CDATA:
				return this.CreateCDataSection(string.Empty);
			case XmlNodeType.EntityReference:
				return this.CreateEntityReference(name);
			case XmlNodeType.ProcessingInstruction:
				return this.CreateProcessingInstruction(name, string.Empty);
			case XmlNodeType.Comment:
				return this.CreateComment(string.Empty);
			case XmlNodeType.Document:
				return new XmlDocument();
			case XmlNodeType.DocumentType:
				return this.CreateDocumentType(name, string.Empty, string.Empty, string.Empty);
			case XmlNodeType.DocumentFragment:
				return this.CreateDocumentFragment();
			case XmlNodeType.Whitespace:
				return this.CreateWhitespace(string.Empty);
			case XmlNodeType.SignificantWhitespace:
				return this.CreateSignificantWhitespace(string.Empty);
			case XmlNodeType.XmlDeclaration:
				return this.CreateXmlDeclaration("1.0", null, null);
			}
			throw new ArgumentException(Res.GetString("Arg_CannotCreateNode", new object[] { type }));
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x000394C7 File Offset: 0x000384C7
		public virtual XmlNode CreateNode(string nodeTypeString, string name, string namespaceURI)
		{
			return this.CreateNode(this.ConvertToNodeType(nodeTypeString), name, namespaceURI);
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x000394D8 File Offset: 0x000384D8
		public virtual XmlNode CreateNode(XmlNodeType type, string name, string namespaceURI)
		{
			return this.CreateNode(type, null, name, namespaceURI);
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x000394E4 File Offset: 0x000384E4
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		public virtual XmlNode ReadNode(XmlReader reader)
		{
			XmlNode xmlNode = null;
			try
			{
				this.IsLoading = true;
				XmlLoader xmlLoader = new XmlLoader();
				xmlNode = xmlLoader.ReadCurrentNode(this, reader);
			}
			finally
			{
				this.IsLoading = false;
			}
			return xmlNode;
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00039524 File Offset: 0x00038524
		internal XmlNodeType ConvertToNodeType(string nodeTypeString)
		{
			if (nodeTypeString == "element")
			{
				return XmlNodeType.Element;
			}
			if (nodeTypeString == "attribute")
			{
				return XmlNodeType.Attribute;
			}
			if (nodeTypeString == "text")
			{
				return XmlNodeType.Text;
			}
			if (nodeTypeString == "cdatasection")
			{
				return XmlNodeType.CDATA;
			}
			if (nodeTypeString == "entityreference")
			{
				return XmlNodeType.EntityReference;
			}
			if (nodeTypeString == "entity")
			{
				return XmlNodeType.Entity;
			}
			if (nodeTypeString == "processinginstruction")
			{
				return XmlNodeType.ProcessingInstruction;
			}
			if (nodeTypeString == "comment")
			{
				return XmlNodeType.Comment;
			}
			if (nodeTypeString == "document")
			{
				return XmlNodeType.Document;
			}
			if (nodeTypeString == "documenttype")
			{
				return XmlNodeType.DocumentType;
			}
			if (nodeTypeString == "documentfragment")
			{
				return XmlNodeType.DocumentFragment;
			}
			if (nodeTypeString == "notation")
			{
				return XmlNodeType.Notation;
			}
			if (nodeTypeString == "significantwhitespace")
			{
				return XmlNodeType.SignificantWhitespace;
			}
			if (nodeTypeString == "whitespace")
			{
				return XmlNodeType.Whitespace;
			}
			throw new ArgumentException(Res.GetString("Xdom_Invalid_NT_String", new object[] { nodeTypeString }));
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00039624 File Offset: 0x00038624
		private XmlTextReader SetupReader(XmlTextReader tr)
		{
			tr.XmlValidatingReaderCompatibilityMode = true;
			tr.EntityHandling = EntityHandling.ExpandCharEntities;
			if (this.HasSetResolver)
			{
				tr.XmlResolver = this.GetResolver();
			}
			return tr;
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0003964C File Offset: 0x0003864C
		public virtual void Load(string filename)
		{
			XmlTextReader xmlTextReader = this.SetupReader(new XmlTextReader(filename, this.NameTable));
			try
			{
				this.Load(xmlTextReader);
			}
			finally
			{
				xmlTextReader.Close();
			}
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0003968C File Offset: 0x0003868C
		public virtual void Load(Stream inStream)
		{
			XmlTextReader xmlTextReader = this.SetupReader(new XmlTextReader(inStream, this.NameTable));
			try
			{
				this.Load(xmlTextReader);
			}
			finally
			{
				xmlTextReader.Impl.Close(false);
			}
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x000396D4 File Offset: 0x000386D4
		public virtual void Load(TextReader txtReader)
		{
			XmlTextReader xmlTextReader = this.SetupReader(new XmlTextReader(txtReader, this.NameTable));
			try
			{
				this.Load(xmlTextReader);
			}
			finally
			{
				xmlTextReader.Impl.Close(false);
			}
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0003971C File Offset: 0x0003871C
		public virtual void Load(XmlReader reader)
		{
			try
			{
				this.IsLoading = true;
				this.actualLoadingStatus = true;
				this.RemoveAll();
				this.fEntRefNodesPresent = false;
				this.fCDataNodesPresent = false;
				this.reportValidity = true;
				XmlLoader xmlLoader = new XmlLoader();
				xmlLoader.Load(this, reader, this.preserveWhitespace);
			}
			finally
			{
				this.IsLoading = false;
				this.actualLoadingStatus = false;
				this.reportValidity = true;
			}
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x00039790 File Offset: 0x00038790
		public virtual void LoadXml(string xml)
		{
			XmlTextReader xmlTextReader = this.SetupReader(new XmlTextReader(new StringReader(xml), this.NameTable));
			try
			{
				this.Load(xmlTextReader);
			}
			finally
			{
				xmlTextReader.Close();
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x000397D8 File Offset: 0x000387D8
		internal Encoding TextEncoding
		{
			get
			{
				if (this.Declaration != null)
				{
					string encoding = this.Declaration.Encoding;
					if (encoding.Length > 0)
					{
						return global::System.Text.Encoding.GetEncoding(encoding);
					}
				}
				return null;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x0003980A File Offset: 0x0003880A
		// (set) Token: 0x06000CDA RID: 3290 RVA: 0x00039812 File Offset: 0x00038812
		public override string InnerXml
		{
			get
			{
				return base.InnerXml;
			}
			set
			{
				this.LoadXml(value);
			}
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0003981C File Offset: 0x0003881C
		public virtual void Save(string filename)
		{
			if (this.DocumentElement == null)
			{
				throw new XmlException("Xml_InvalidXmlDocument", Res.GetString("Xdom_NoRootEle"));
			}
			XmlDOMTextWriter xmlDOMTextWriter = new XmlDOMTextWriter(filename, this.TextEncoding);
			try
			{
				if (!this.preserveWhitespace)
				{
					xmlDOMTextWriter.Formatting = Formatting.Indented;
				}
				this.WriteTo(xmlDOMTextWriter);
			}
			finally
			{
				xmlDOMTextWriter.Flush();
				xmlDOMTextWriter.Close();
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x00039888 File Offset: 0x00038888
		public virtual void Save(Stream outStream)
		{
			XmlDOMTextWriter xmlDOMTextWriter = new XmlDOMTextWriter(outStream, this.TextEncoding);
			if (!this.preserveWhitespace)
			{
				xmlDOMTextWriter.Formatting = Formatting.Indented;
			}
			this.WriteTo(xmlDOMTextWriter);
			xmlDOMTextWriter.Flush();
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x000398C0 File Offset: 0x000388C0
		public virtual void Save(TextWriter writer)
		{
			XmlDOMTextWriter xmlDOMTextWriter = new XmlDOMTextWriter(writer);
			if (!this.preserveWhitespace)
			{
				xmlDOMTextWriter.Formatting = Formatting.Indented;
			}
			this.Save(xmlDOMTextWriter);
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x000398EC File Offset: 0x000388EC
		public virtual void Save(XmlWriter w)
		{
			XmlNode xmlNode = this.FirstChild;
			if (xmlNode == null)
			{
				return;
			}
			if (w.WriteState == WriteState.Start)
			{
				if (xmlNode is XmlDeclaration)
				{
					if (this.Standalone.Length == 0)
					{
						w.WriteStartDocument();
					}
					else if (this.Standalone == "yes")
					{
						w.WriteStartDocument(true);
					}
					else if (this.Standalone == "no")
					{
						w.WriteStartDocument(false);
					}
					xmlNode = xmlNode.NextSibling;
				}
				else
				{
					w.WriteStartDocument();
				}
			}
			while (xmlNode != null)
			{
				xmlNode.WriteTo(w);
				xmlNode = xmlNode.NextSibling;
			}
			w.Flush();
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00039985 File Offset: 0x00038985
		public override void WriteTo(XmlWriter w)
		{
			this.WriteContentTo(w);
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00039990 File Offset: 0x00038990
		public override void WriteContentTo(XmlWriter xw)
		{
			foreach (object obj in this)
			{
				XmlNode xmlNode = (XmlNode)obj;
				xmlNode.WriteTo(xw);
			}
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x000399E4 File Offset: 0x000389E4
		public void Validate(ValidationEventHandler validationEventHandler)
		{
			this.Validate(validationEventHandler, this);
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x000399F0 File Offset: 0x000389F0
		public void Validate(ValidationEventHandler validationEventHandler, XmlNode nodeToValidate)
		{
			if (this.schemas == null || this.schemas.Count == 0)
			{
				throw new InvalidOperationException(Res.GetString("XmlDocument_NoSchemaInfo"));
			}
			XmlDocument document = nodeToValidate.Document;
			if (document != this)
			{
				throw new ArgumentException(Res.GetString("XmlDocument_NodeNotFromDocument", new object[] { "nodeToValidate" }));
			}
			if (nodeToValidate == this)
			{
				this.reportValidity = false;
			}
			DocumentSchemaValidator documentSchemaValidator = new DocumentSchemaValidator(this, this.schemas, validationEventHandler);
			documentSchemaValidator.Validate(nodeToValidate);
			if (nodeToValidate == this)
			{
				this.reportValidity = true;
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000CE3 RID: 3299 RVA: 0x00039A79 File Offset: 0x00038A79
		// (remove) Token: 0x06000CE4 RID: 3300 RVA: 0x00039A92 File Offset: 0x00038A92
		public event XmlNodeChangedEventHandler NodeInserting
		{
			add
			{
				this.onNodeInsertingDelegate = (XmlNodeChangedEventHandler)Delegate.Combine(this.onNodeInsertingDelegate, value);
			}
			remove
			{
				this.onNodeInsertingDelegate = (XmlNodeChangedEventHandler)Delegate.Remove(this.onNodeInsertingDelegate, value);
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000CE5 RID: 3301 RVA: 0x00039AAB File Offset: 0x00038AAB
		// (remove) Token: 0x06000CE6 RID: 3302 RVA: 0x00039AC4 File Offset: 0x00038AC4
		public event XmlNodeChangedEventHandler NodeInserted
		{
			add
			{
				this.onNodeInsertedDelegate = (XmlNodeChangedEventHandler)Delegate.Combine(this.onNodeInsertedDelegate, value);
			}
			remove
			{
				this.onNodeInsertedDelegate = (XmlNodeChangedEventHandler)Delegate.Remove(this.onNodeInsertedDelegate, value);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000CE7 RID: 3303 RVA: 0x00039ADD File Offset: 0x00038ADD
		// (remove) Token: 0x06000CE8 RID: 3304 RVA: 0x00039AF6 File Offset: 0x00038AF6
		public event XmlNodeChangedEventHandler NodeRemoving
		{
			add
			{
				this.onNodeRemovingDelegate = (XmlNodeChangedEventHandler)Delegate.Combine(this.onNodeRemovingDelegate, value);
			}
			remove
			{
				this.onNodeRemovingDelegate = (XmlNodeChangedEventHandler)Delegate.Remove(this.onNodeRemovingDelegate, value);
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000CE9 RID: 3305 RVA: 0x00039B0F File Offset: 0x00038B0F
		// (remove) Token: 0x06000CEA RID: 3306 RVA: 0x00039B28 File Offset: 0x00038B28
		public event XmlNodeChangedEventHandler NodeRemoved
		{
			add
			{
				this.onNodeRemovedDelegate = (XmlNodeChangedEventHandler)Delegate.Combine(this.onNodeRemovedDelegate, value);
			}
			remove
			{
				this.onNodeRemovedDelegate = (XmlNodeChangedEventHandler)Delegate.Remove(this.onNodeRemovedDelegate, value);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000CEB RID: 3307 RVA: 0x00039B41 File Offset: 0x00038B41
		// (remove) Token: 0x06000CEC RID: 3308 RVA: 0x00039B5A File Offset: 0x00038B5A
		public event XmlNodeChangedEventHandler NodeChanging
		{
			add
			{
				this.onNodeChangingDelegate = (XmlNodeChangedEventHandler)Delegate.Combine(this.onNodeChangingDelegate, value);
			}
			remove
			{
				this.onNodeChangingDelegate = (XmlNodeChangedEventHandler)Delegate.Remove(this.onNodeChangingDelegate, value);
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000CED RID: 3309 RVA: 0x00039B73 File Offset: 0x00038B73
		// (remove) Token: 0x06000CEE RID: 3310 RVA: 0x00039B8C File Offset: 0x00038B8C
		public event XmlNodeChangedEventHandler NodeChanged
		{
			add
			{
				this.onNodeChangedDelegate = (XmlNodeChangedEventHandler)Delegate.Combine(this.onNodeChangedDelegate, value);
			}
			remove
			{
				this.onNodeChangedDelegate = (XmlNodeChangedEventHandler)Delegate.Remove(this.onNodeChangedDelegate, value);
			}
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00039BA8 File Offset: 0x00038BA8
		internal override XmlNodeChangedEventArgs GetEventArgs(XmlNode node, XmlNode oldParent, XmlNode newParent, string oldValue, string newValue, XmlNodeChangedAction action)
		{
			this.reportValidity = false;
			switch (action)
			{
			case XmlNodeChangedAction.Insert:
				if (this.onNodeInsertingDelegate == null && this.onNodeInsertedDelegate == null)
				{
					return null;
				}
				break;
			case XmlNodeChangedAction.Remove:
				if (this.onNodeRemovingDelegate == null && this.onNodeRemovedDelegate == null)
				{
					return null;
				}
				break;
			case XmlNodeChangedAction.Change:
				if (this.onNodeChangingDelegate == null && this.onNodeChangedDelegate == null)
				{
					return null;
				}
				break;
			}
			return new XmlNodeChangedEventArgs(node, oldParent, newParent, oldValue, newValue, action);
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00039C18 File Offset: 0x00038C18
		internal XmlNodeChangedEventArgs GetInsertEventArgsForLoad(XmlNode node, XmlNode newParent)
		{
			if (this.onNodeInsertingDelegate == null && this.onNodeInsertedDelegate == null)
			{
				return null;
			}
			string value = node.Value;
			return new XmlNodeChangedEventArgs(node, null, newParent, value, value, XmlNodeChangedAction.Insert);
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00039C4C File Offset: 0x00038C4C
		internal override void BeforeEvent(XmlNodeChangedEventArgs args)
		{
			if (args != null)
			{
				switch (args.Action)
				{
				case XmlNodeChangedAction.Insert:
					if (this.onNodeInsertingDelegate != null)
					{
						this.onNodeInsertingDelegate(this, args);
						return;
					}
					break;
				case XmlNodeChangedAction.Remove:
					if (this.onNodeRemovingDelegate != null)
					{
						this.onNodeRemovingDelegate(this, args);
						return;
					}
					break;
				case XmlNodeChangedAction.Change:
					if (this.onNodeChangingDelegate != null)
					{
						this.onNodeChangingDelegate(this, args);
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00039CB8 File Offset: 0x00038CB8
		internal override void AfterEvent(XmlNodeChangedEventArgs args)
		{
			if (args != null)
			{
				switch (args.Action)
				{
				case XmlNodeChangedAction.Insert:
					if (this.onNodeInsertedDelegate != null)
					{
						this.onNodeInsertedDelegate(this, args);
						return;
					}
					break;
				case XmlNodeChangedAction.Remove:
					if (this.onNodeRemovedDelegate != null)
					{
						this.onNodeRemovedDelegate(this, args);
						return;
					}
					break;
				case XmlNodeChangedAction.Change:
					if (this.onNodeChangedDelegate != null)
					{
						this.onNodeChangedDelegate(this, args);
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x00039D24 File Offset: 0x00038D24
		internal XmlAttribute GetDefaultAttribute(XmlElement elem, string attrPrefix, string attrLocalname, string attrNamespaceURI)
		{
			SchemaInfo dtdSchemaInfo = this.DtdSchemaInfo;
			SchemaElementDecl schemaElementDecl = this.GetSchemaElementDecl(elem);
			if (schemaElementDecl != null && schemaElementDecl.AttDefs != null)
			{
				IDictionaryEnumerator enumerator = schemaElementDecl.AttDefs.GetEnumerator();
				while (enumerator.MoveNext())
				{
					SchemaAttDef schemaAttDef = (SchemaAttDef)enumerator.Value;
					if ((schemaAttDef.Presence == SchemaDeclBase.Use.Default || schemaAttDef.Presence == SchemaDeclBase.Use.Fixed) && schemaAttDef.Name.Name == attrLocalname && ((dtdSchemaInfo.SchemaType == SchemaType.DTD && schemaAttDef.Name.Namespace == attrPrefix) || (dtdSchemaInfo.SchemaType != SchemaType.DTD && schemaAttDef.Name.Namespace == attrNamespaceURI)))
					{
						return this.PrepareDefaultAttribute(schemaAttDef, attrPrefix, attrLocalname, attrNamespaceURI);
					}
				}
			}
			return null;
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x00039DE4 File Offset: 0x00038DE4
		internal string Version
		{
			get
			{
				XmlDeclaration declaration = this.Declaration;
				if (declaration != null)
				{
					return declaration.Version;
				}
				return null;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x00039E04 File Offset: 0x00038E04
		internal string Encoding
		{
			get
			{
				XmlDeclaration declaration = this.Declaration;
				if (declaration != null)
				{
					return declaration.Encoding;
				}
				return null;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x00039E24 File Offset: 0x00038E24
		internal string Standalone
		{
			get
			{
				XmlDeclaration declaration = this.Declaration;
				if (declaration != null)
				{
					return declaration.Standalone;
				}
				return null;
			}
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x00039E44 File Offset: 0x00038E44
		internal XmlEntity GetEntityNode(string name)
		{
			if (this.DocumentType != null)
			{
				XmlNamedNodeMap xmlNamedNodeMap = this.DocumentType.Entities;
				if (xmlNamedNodeMap != null)
				{
					return (XmlEntity)xmlNamedNodeMap.GetNamedItem(name);
				}
			}
			return null;
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x00039E78 File Offset: 0x00038E78
		public override IXmlSchemaInfo SchemaInfo
		{
			get
			{
				if (this.reportValidity)
				{
					XmlElement documentElement = this.DocumentElement;
					if (documentElement != null)
					{
						switch (documentElement.SchemaInfo.Validity)
						{
						case XmlSchemaValidity.Valid:
							return XmlDocument.ValidSchemaInfo;
						case XmlSchemaValidity.Invalid:
							return XmlDocument.InvalidSchemaInfo;
						}
					}
				}
				return XmlDocument.NotKnownSchemaInfo;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x00039EC6 File Offset: 0x00038EC6
		public override string BaseURI
		{
			get
			{
				return this.baseURI;
			}
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x00039ECE File Offset: 0x00038ECE
		internal void SetBaseURI(string inBaseURI)
		{
			this.baseURI = inBaseURI;
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00039ED8 File Offset: 0x00038ED8
		internal override XmlNode AppendChildForLoad(XmlNode newChild, XmlDocument doc)
		{
			if (!this.IsValidChildType(newChild.NodeType))
			{
				throw new InvalidOperationException(Res.GetString("Xdom_Node_Insert_TypeConflict"));
			}
			if (!this.CanInsertAfter(newChild, this.LastChild))
			{
				throw new InvalidOperationException(Res.GetString("Xdom_Node_Insert_Location"));
			}
			XmlNodeChangedEventArgs insertEventArgsForLoad = this.GetInsertEventArgsForLoad(newChild, this);
			if (insertEventArgsForLoad != null)
			{
				this.BeforeEvent(insertEventArgsForLoad);
			}
			XmlLinkedNode xmlLinkedNode = (XmlLinkedNode)newChild;
			if (this.lastChild == null)
			{
				xmlLinkedNode.next = xmlLinkedNode;
			}
			else
			{
				xmlLinkedNode.next = this.lastChild.next;
				this.lastChild.next = xmlLinkedNode;
			}
			this.lastChild = xmlLinkedNode;
			xmlLinkedNode.SetParentForLoad(this);
			if (insertEventArgsForLoad != null)
			{
				this.AfterEvent(insertEventArgsForLoad);
			}
			return xmlLinkedNode;
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x00039F83 File Offset: 0x00038F83
		internal override XPathNodeType XPNodeType
		{
			get
			{
				return XPathNodeType.Root;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x00039F86 File Offset: 0x00038F86
		internal bool HasEntityReferences
		{
			get
			{
				return this.fEntRefNodesPresent;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00039F90 File Offset: 0x00038F90
		internal XmlAttribute NamespaceXml
		{
			get
			{
				if (this.namespaceXml == null)
				{
					this.namespaceXml = new XmlAttribute(this.AddAttrXmlName(this.strXmlns, this.strXml, this.strReservedXmlns, null), this);
					this.namespaceXml.Value = this.strReservedXml;
				}
				return this.namespaceXml;
			}
		}

		// Token: 0x040008FB RID: 2299
		private XmlImplementation implementation;

		// Token: 0x040008FC RID: 2300
		private DomNameTable domNameTable;

		// Token: 0x040008FD RID: 2301
		private XmlLinkedNode lastChild;

		// Token: 0x040008FE RID: 2302
		private XmlNamedNodeMap entities;

		// Token: 0x040008FF RID: 2303
		private Hashtable htElementIdMap;

		// Token: 0x04000900 RID: 2304
		private Hashtable htElementIDAttrDecl;

		// Token: 0x04000901 RID: 2305
		private SchemaInfo schemaInfo;

		// Token: 0x04000902 RID: 2306
		private XmlSchemaSet schemas;

		// Token: 0x04000903 RID: 2307
		private bool reportValidity;

		// Token: 0x04000904 RID: 2308
		private bool actualLoadingStatus;

		// Token: 0x04000905 RID: 2309
		private XmlNodeChangedEventHandler onNodeInsertingDelegate;

		// Token: 0x04000906 RID: 2310
		private XmlNodeChangedEventHandler onNodeInsertedDelegate;

		// Token: 0x04000907 RID: 2311
		private XmlNodeChangedEventHandler onNodeRemovingDelegate;

		// Token: 0x04000908 RID: 2312
		private XmlNodeChangedEventHandler onNodeRemovedDelegate;

		// Token: 0x04000909 RID: 2313
		private XmlNodeChangedEventHandler onNodeChangingDelegate;

		// Token: 0x0400090A RID: 2314
		private XmlNodeChangedEventHandler onNodeChangedDelegate;

		// Token: 0x0400090B RID: 2315
		internal bool fEntRefNodesPresent;

		// Token: 0x0400090C RID: 2316
		internal bool fCDataNodesPresent;

		// Token: 0x0400090D RID: 2317
		private bool preserveWhitespace;

		// Token: 0x0400090E RID: 2318
		private bool isLoading;

		// Token: 0x0400090F RID: 2319
		internal string strDocumentName;

		// Token: 0x04000910 RID: 2320
		internal string strDocumentFragmentName;

		// Token: 0x04000911 RID: 2321
		internal string strCommentName;

		// Token: 0x04000912 RID: 2322
		internal string strTextName;

		// Token: 0x04000913 RID: 2323
		internal string strCDataSectionName;

		// Token: 0x04000914 RID: 2324
		internal string strEntityName;

		// Token: 0x04000915 RID: 2325
		internal string strID;

		// Token: 0x04000916 RID: 2326
		internal string strXmlns;

		// Token: 0x04000917 RID: 2327
		internal string strXml;

		// Token: 0x04000918 RID: 2328
		internal string strSpace;

		// Token: 0x04000919 RID: 2329
		internal string strLang;

		// Token: 0x0400091A RID: 2330
		internal string strEmpty;

		// Token: 0x0400091B RID: 2331
		internal string strNonSignificantWhitespaceName;

		// Token: 0x0400091C RID: 2332
		internal string strSignificantWhitespaceName;

		// Token: 0x0400091D RID: 2333
		internal string strReservedXmlns;

		// Token: 0x0400091E RID: 2334
		internal string strReservedXml;

		// Token: 0x0400091F RID: 2335
		internal string baseURI;

		// Token: 0x04000920 RID: 2336
		private XmlResolver resolver;

		// Token: 0x04000921 RID: 2337
		internal bool bSetResolver;

		// Token: 0x04000922 RID: 2338
		internal object objLock;

		// Token: 0x04000923 RID: 2339
		private XmlAttribute namespaceXml;

		// Token: 0x04000924 RID: 2340
		internal static EmptyEnumerator EmptyEnumerator = new EmptyEnumerator();

		// Token: 0x04000925 RID: 2341
		internal static IXmlSchemaInfo NotKnownSchemaInfo = new XmlSchemaInfo(XmlSchemaValidity.NotKnown);

		// Token: 0x04000926 RID: 2342
		internal static IXmlSchemaInfo ValidSchemaInfo = new XmlSchemaInfo(XmlSchemaValidity.Valid);

		// Token: 0x04000927 RID: 2343
		internal static IXmlSchemaInfo InvalidSchemaInfo = new XmlSchemaInfo(XmlSchemaValidity.Invalid);
	}
}
