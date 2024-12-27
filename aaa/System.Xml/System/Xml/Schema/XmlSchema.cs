using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using System.Xml.XmlConfiguration;

namespace System.Xml.Schema
{
	// Token: 0x0200022D RID: 557
	[XmlRoot("schema", Namespace = "http://www.w3.org/2001/XMLSchema")]
	public class XmlSchema : XmlSchemaObject
	{
		// Token: 0x06001A80 RID: 6784 RVA: 0x0007FE6F File Offset: 0x0007EE6F
		public static XmlSchema Read(TextReader reader, ValidationEventHandler validationEventHandler)
		{
			return XmlSchema.Read(new XmlTextReader(reader), validationEventHandler);
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x0007FE7D File Offset: 0x0007EE7D
		public static XmlSchema Read(Stream stream, ValidationEventHandler validationEventHandler)
		{
			return XmlSchema.Read(new XmlTextReader(stream), validationEventHandler);
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x0007FE8C File Offset: 0x0007EE8C
		public static XmlSchema Read(XmlReader reader, ValidationEventHandler validationEventHandler)
		{
			XmlNameTable xmlNameTable = reader.NameTable;
			Parser parser = new Parser(SchemaType.XSD, xmlNameTable, new SchemaNames(xmlNameTable), validationEventHandler);
			try
			{
				parser.Parse(reader, null);
			}
			catch (XmlSchemaException ex)
			{
				if (validationEventHandler != null)
				{
					validationEventHandler(null, new ValidationEventArgs(ex));
					return null;
				}
				throw ex;
			}
			return parser.XmlSchema;
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x0007FEEC File Offset: 0x0007EEEC
		public void Write(Stream stream)
		{
			this.Write(stream, null);
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x0007FEF8 File Offset: 0x0007EEF8
		public void Write(Stream stream, XmlNamespaceManager namespaceManager)
		{
			this.Write(new XmlTextWriter(stream, null)
			{
				Formatting = Formatting.Indented
			}, namespaceManager);
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x0007FF1C File Offset: 0x0007EF1C
		public void Write(TextWriter writer)
		{
			this.Write(writer, null);
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x0007FF28 File Offset: 0x0007EF28
		public void Write(TextWriter writer, XmlNamespaceManager namespaceManager)
		{
			this.Write(new XmlTextWriter(writer)
			{
				Formatting = Formatting.Indented
			}, namespaceManager);
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x0007FF4B File Offset: 0x0007EF4B
		public void Write(XmlWriter writer)
		{
			this.Write(writer, null);
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x0007FF58 File Offset: 0x0007EF58
		public void Write(XmlWriter writer, XmlNamespaceManager namespaceManager)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlSchema));
			XmlSerializerNamespaces xmlSerializerNamespaces;
			if (namespaceManager != null)
			{
				xmlSerializerNamespaces = new XmlSerializerNamespaces();
				bool flag = false;
				if (base.Namespaces != null)
				{
					flag = base.Namespaces.Namespaces["xs"] != null || base.Namespaces.Namespaces.ContainsValue("http://www.w3.org/2001/XMLSchema");
				}
				if (!flag && namespaceManager.LookupPrefix("http://www.w3.org/2001/XMLSchema") == null && namespaceManager.LookupNamespace("xs") == null)
				{
					xmlSerializerNamespaces.Add("xs", "http://www.w3.org/2001/XMLSchema");
				}
				using (IEnumerator enumerator = namespaceManager.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						string text = (string)obj;
						if (text != "xml" && text != "xmlns")
						{
							xmlSerializerNamespaces.Add(text, namespaceManager.LookupNamespace(text));
						}
					}
					goto IL_017A;
				}
			}
			if (base.Namespaces != null && base.Namespaces.Count > 0)
			{
				Hashtable namespaces = base.Namespaces.Namespaces;
				if (namespaces["xs"] == null && !namespaces.ContainsValue("http://www.w3.org/2001/XMLSchema"))
				{
					namespaces.Add("xs", "http://www.w3.org/2001/XMLSchema");
				}
				xmlSerializerNamespaces = base.Namespaces;
			}
			else
			{
				xmlSerializerNamespaces = new XmlSerializerNamespaces();
				xmlSerializerNamespaces.Add("xs", "http://www.w3.org/2001/XMLSchema");
				if (this.targetNs != null && this.targetNs.Length != 0)
				{
					xmlSerializerNamespaces.Add("tns", this.targetNs);
				}
			}
			IL_017A:
			xmlSerializer.Serialize(writer, this, xmlSerializerNamespaces);
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x000800F8 File Offset: 0x0007F0F8
		[Obsolete("Use System.Xml.Schema.XmlSchemaSet for schema compilation and validation. http://go.microsoft.com/fwlink/?linkid=14202")]
		public void Compile(ValidationEventHandler validationEventHandler)
		{
			SchemaInfo schemaInfo = new SchemaInfo();
			schemaInfo.SchemaType = SchemaType.XSD;
			this.CompileSchema(null, XmlReaderSection.CreateDefaultResolver(), schemaInfo, null, validationEventHandler, this.NameTable, false);
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x0008012C File Offset: 0x0007F12C
		[Obsolete("Use System.Xml.Schema.XmlSchemaSet for schema compilation and validation. http://go.microsoft.com/fwlink/?linkid=14202")]
		public void Compile(ValidationEventHandler validationEventHandler, XmlResolver resolver)
		{
			this.CompileSchema(null, resolver, new SchemaInfo
			{
				SchemaType = SchemaType.XSD
			}, null, validationEventHandler, this.NameTable, false);
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x0008015C File Offset: 0x0007F15C
		internal bool CompileSchema(XmlSchemaCollection xsc, XmlResolver resolver, SchemaInfo schemaInfo, string ns, ValidationEventHandler validationEventHandler, XmlNameTable nameTable, bool CompileContentModel)
		{
			bool flag;
			lock (this)
			{
				if (!new SchemaCollectionPreprocessor(nameTable, null, validationEventHandler)
				{
					XmlResolver = resolver
				}.Execute(this, ns, true, xsc))
				{
					flag = false;
				}
				else
				{
					SchemaCollectionCompiler schemaCollectionCompiler = new SchemaCollectionCompiler(nameTable, validationEventHandler);
					this.isCompiled = schemaCollectionCompiler.Execute(this, schemaInfo, CompileContentModel);
					this.SetIsCompiled(this.isCompiled);
					flag = this.isCompiled;
				}
			}
			return flag;
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x000801DC File Offset: 0x0007F1DC
		internal void CompileSchemaInSet(XmlNameTable nameTable, ValidationEventHandler eventHandler, XmlSchemaCompilationSettings compilationSettings)
		{
			Compiler compiler = new Compiler(nameTable, eventHandler, null, compilationSettings);
			compiler.Prepare(this, true);
			this.isCompiledBySet = compiler.Compile();
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06001A8D RID: 6797 RVA: 0x00080207 File Offset: 0x0007F207
		// (set) Token: 0x06001A8E RID: 6798 RVA: 0x0008020F File Offset: 0x0007F20F
		[DefaultValue(XmlSchemaForm.None)]
		[XmlAttribute("attributeFormDefault")]
		public XmlSchemaForm AttributeFormDefault
		{
			get
			{
				return this.attributeFormDefault;
			}
			set
			{
				this.attributeFormDefault = value;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06001A8F RID: 6799 RVA: 0x00080218 File Offset: 0x0007F218
		// (set) Token: 0x06001A90 RID: 6800 RVA: 0x00080220 File Offset: 0x0007F220
		[DefaultValue(XmlSchemaDerivationMethod.None)]
		[XmlAttribute("blockDefault")]
		public XmlSchemaDerivationMethod BlockDefault
		{
			get
			{
				return this.blockDefault;
			}
			set
			{
				this.blockDefault = value;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06001A91 RID: 6801 RVA: 0x00080229 File Offset: 0x0007F229
		// (set) Token: 0x06001A92 RID: 6802 RVA: 0x00080231 File Offset: 0x0007F231
		[DefaultValue(XmlSchemaDerivationMethod.None)]
		[XmlAttribute("finalDefault")]
		public XmlSchemaDerivationMethod FinalDefault
		{
			get
			{
				return this.finalDefault;
			}
			set
			{
				this.finalDefault = value;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06001A93 RID: 6803 RVA: 0x0008023A File Offset: 0x0007F23A
		// (set) Token: 0x06001A94 RID: 6804 RVA: 0x00080242 File Offset: 0x0007F242
		[DefaultValue(XmlSchemaForm.None)]
		[XmlAttribute("elementFormDefault")]
		public XmlSchemaForm ElementFormDefault
		{
			get
			{
				return this.elementFormDefault;
			}
			set
			{
				this.elementFormDefault = value;
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06001A95 RID: 6805 RVA: 0x0008024B File Offset: 0x0007F24B
		// (set) Token: 0x06001A96 RID: 6806 RVA: 0x00080253 File Offset: 0x0007F253
		[XmlAttribute("targetNamespace", DataType = "anyURI")]
		public string TargetNamespace
		{
			get
			{
				return this.targetNs;
			}
			set
			{
				this.targetNs = value;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06001A97 RID: 6807 RVA: 0x0008025C File Offset: 0x0007F25C
		// (set) Token: 0x06001A98 RID: 6808 RVA: 0x00080264 File Offset: 0x0007F264
		[XmlAttribute("version", DataType = "token")]
		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06001A99 RID: 6809 RVA: 0x0008026D File Offset: 0x0007F26D
		[XmlElement("include", typeof(XmlSchemaInclude))]
		[XmlElement("redefine", typeof(XmlSchemaRedefine))]
		[XmlElement("import", typeof(XmlSchemaImport))]
		public XmlSchemaObjectCollection Includes
		{
			get
			{
				return this.includes;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06001A9A RID: 6810 RVA: 0x00080275 File Offset: 0x0007F275
		[XmlElement("complexType", typeof(XmlSchemaComplexType))]
		[XmlElement("annotation", typeof(XmlSchemaAnnotation))]
		[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroup))]
		[XmlElement("group", typeof(XmlSchemaGroup))]
		[XmlElement("notation", typeof(XmlSchemaNotation))]
		[XmlElement("element", typeof(XmlSchemaElement))]
		[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
		[XmlElement("attribute", typeof(XmlSchemaAttribute))]
		public XmlSchemaObjectCollection Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001A9B RID: 6811 RVA: 0x0008027D File Offset: 0x0007F27D
		[XmlIgnore]
		public bool IsCompiled
		{
			get
			{
				return this.isCompiled || this.isCompiledBySet;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001A9C RID: 6812 RVA: 0x0008028F File Offset: 0x0007F28F
		// (set) Token: 0x06001A9D RID: 6813 RVA: 0x00080297 File Offset: 0x0007F297
		[XmlIgnore]
		internal bool IsCompiledBySet
		{
			get
			{
				return this.isCompiledBySet;
			}
			set
			{
				this.isCompiledBySet = value;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001A9E RID: 6814 RVA: 0x000802A0 File Offset: 0x0007F2A0
		// (set) Token: 0x06001A9F RID: 6815 RVA: 0x000802A8 File Offset: 0x0007F2A8
		[XmlIgnore]
		internal bool IsPreprocessed
		{
			get
			{
				return this.isPreprocessed;
			}
			set
			{
				this.isPreprocessed = value;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001AA0 RID: 6816 RVA: 0x000802B1 File Offset: 0x0007F2B1
		// (set) Token: 0x06001AA1 RID: 6817 RVA: 0x000802B9 File Offset: 0x0007F2B9
		[XmlIgnore]
		internal bool IsRedefined
		{
			get
			{
				return this.isRedefined;
			}
			set
			{
				this.isRedefined = value;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001AA2 RID: 6818 RVA: 0x000802C2 File Offset: 0x0007F2C2
		[XmlIgnore]
		public XmlSchemaObjectTable Attributes
		{
			get
			{
				if (this.attributes == null)
				{
					this.attributes = new XmlSchemaObjectTable();
				}
				return this.attributes;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001AA3 RID: 6819 RVA: 0x000802DD File Offset: 0x0007F2DD
		[XmlIgnore]
		public XmlSchemaObjectTable AttributeGroups
		{
			get
			{
				if (this.attributeGroups == null)
				{
					this.attributeGroups = new XmlSchemaObjectTable();
				}
				return this.attributeGroups;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06001AA4 RID: 6820 RVA: 0x000802F8 File Offset: 0x0007F2F8
		[XmlIgnore]
		public XmlSchemaObjectTable SchemaTypes
		{
			get
			{
				if (this.types == null)
				{
					this.types = new XmlSchemaObjectTable();
				}
				return this.types;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06001AA5 RID: 6821 RVA: 0x00080313 File Offset: 0x0007F313
		[XmlIgnore]
		public XmlSchemaObjectTable Elements
		{
			get
			{
				if (this.elements == null)
				{
					this.elements = new XmlSchemaObjectTable();
				}
				return this.elements;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06001AA6 RID: 6822 RVA: 0x0008032E File Offset: 0x0007F32E
		// (set) Token: 0x06001AA7 RID: 6823 RVA: 0x00080336 File Offset: 0x0007F336
		[XmlAttribute("id", DataType = "ID")]
		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06001AA8 RID: 6824 RVA: 0x0008033F File Offset: 0x0007F33F
		// (set) Token: 0x06001AA9 RID: 6825 RVA: 0x00080347 File Offset: 0x0007F347
		[XmlAnyAttribute]
		public XmlAttribute[] UnhandledAttributes
		{
			get
			{
				return this.moreAttributes;
			}
			set
			{
				this.moreAttributes = value;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06001AAA RID: 6826 RVA: 0x00080350 File Offset: 0x0007F350
		[XmlIgnore]
		public XmlSchemaObjectTable Groups
		{
			get
			{
				return this.groups;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06001AAB RID: 6827 RVA: 0x00080358 File Offset: 0x0007F358
		[XmlIgnore]
		public XmlSchemaObjectTable Notations
		{
			get
			{
				return this.notations;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06001AAC RID: 6828 RVA: 0x00080360 File Offset: 0x0007F360
		[XmlIgnore]
		internal XmlSchemaObjectTable IdentityConstraints
		{
			get
			{
				return this.identityConstraints;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06001AAD RID: 6829 RVA: 0x00080368 File Offset: 0x0007F368
		// (set) Token: 0x06001AAE RID: 6830 RVA: 0x00080370 File Offset: 0x0007F370
		[XmlIgnore]
		internal Uri BaseUri
		{
			get
			{
				return this.baseUri;
			}
			set
			{
				this.baseUri = value;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06001AAF RID: 6831 RVA: 0x00080379 File Offset: 0x0007F379
		[XmlIgnore]
		internal int SchemaId
		{
			get
			{
				if (this.schemaId == -1)
				{
					this.schemaId = Interlocked.Increment(ref XmlSchema.globalIdCounter);
				}
				return this.schemaId;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06001AB0 RID: 6832 RVA: 0x0008039A File Offset: 0x0007F39A
		// (set) Token: 0x06001AB1 RID: 6833 RVA: 0x000803A2 File Offset: 0x0007F3A2
		[XmlIgnore]
		internal bool IsChameleon
		{
			get
			{
				return this.isChameleon;
			}
			set
			{
				this.isChameleon = value;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06001AB2 RID: 6834 RVA: 0x000803AB File Offset: 0x0007F3AB
		[XmlIgnore]
		internal Hashtable Ids
		{
			get
			{
				return this.ids;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06001AB3 RID: 6835 RVA: 0x000803B3 File Offset: 0x0007F3B3
		[XmlIgnore]
		internal XmlDocument Document
		{
			get
			{
				if (this.document == null)
				{
					this.document = new XmlDocument();
				}
				return this.document;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06001AB4 RID: 6836 RVA: 0x000803CE File Offset: 0x0007F3CE
		// (set) Token: 0x06001AB5 RID: 6837 RVA: 0x000803D6 File Offset: 0x0007F3D6
		[XmlIgnore]
		internal int ErrorCount
		{
			get
			{
				return this.errorCount;
			}
			set
			{
				this.errorCount = value;
			}
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x000803E0 File Offset: 0x0007F3E0
		internal new XmlSchema Clone()
		{
			XmlSchema xmlSchema = new XmlSchema();
			xmlSchema.attributeFormDefault = this.attributeFormDefault;
			xmlSchema.elementFormDefault = this.elementFormDefault;
			xmlSchema.blockDefault = this.blockDefault;
			xmlSchema.finalDefault = this.finalDefault;
			xmlSchema.targetNs = this.targetNs;
			xmlSchema.version = this.version;
			xmlSchema.includes = this.includes;
			xmlSchema.Namespaces = base.Namespaces;
			xmlSchema.items = this.items;
			xmlSchema.BaseUri = this.BaseUri;
			SchemaCollectionCompiler.Cleanup(xmlSchema);
			return xmlSchema;
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x00080474 File Offset: 0x0007F474
		internal XmlSchema DeepClone()
		{
			XmlSchema xmlSchema = new XmlSchema();
			xmlSchema.attributeFormDefault = this.attributeFormDefault;
			xmlSchema.elementFormDefault = this.elementFormDefault;
			xmlSchema.blockDefault = this.blockDefault;
			xmlSchema.finalDefault = this.finalDefault;
			xmlSchema.targetNs = this.targetNs;
			xmlSchema.version = this.version;
			xmlSchema.isPreprocessed = this.isPreprocessed;
			foreach (XmlSchemaObject xmlSchemaObject in this.items)
			{
				XmlSchemaObject xmlSchemaObject2 = xmlSchemaObject.Clone();
				xmlSchema.Items.Add(xmlSchemaObject2);
			}
			foreach (XmlSchemaObject xmlSchemaObject3 in this.includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject3;
				XmlSchemaExternal xmlSchemaExternal2 = (XmlSchemaExternal)xmlSchemaExternal.Clone();
				xmlSchema.Includes.Add(xmlSchemaExternal2);
			}
			xmlSchema.Namespaces = base.Namespaces;
			xmlSchema.BaseUri = this.BaseUri;
			return xmlSchema;
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06001AB8 RID: 6840 RVA: 0x000805B4 File Offset: 0x0007F5B4
		// (set) Token: 0x06001AB9 RID: 6841 RVA: 0x000805BC File Offset: 0x0007F5BC
		[XmlIgnore]
		internal override string IdAttribute
		{
			get
			{
				return this.Id;
			}
			set
			{
				this.Id = value;
			}
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x000805C5 File Offset: 0x0007F5C5
		internal void SetIsCompiled(bool isCompiled)
		{
			this.isCompiled = isCompiled;
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x000805CE File Offset: 0x0007F5CE
		internal override void SetUnhandledAttributes(XmlAttribute[] moreAttributes)
		{
			this.moreAttributes = moreAttributes;
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x000805D7 File Offset: 0x0007F5D7
		internal override void AddAnnotation(XmlSchemaAnnotation annotation)
		{
			this.items.Add(annotation);
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06001ABD RID: 6845 RVA: 0x000805E6 File Offset: 0x0007F5E6
		internal XmlNameTable NameTable
		{
			get
			{
				if (this.nameTable == null)
				{
					this.nameTable = new NameTable();
				}
				return this.nameTable;
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06001ABE RID: 6846 RVA: 0x00080601 File Offset: 0x0007F601
		internal ArrayList ImportedSchemas
		{
			get
			{
				if (this.importedSchemas == null)
				{
					this.importedSchemas = new ArrayList();
				}
				return this.importedSchemas;
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06001ABF RID: 6847 RVA: 0x0008061C File Offset: 0x0007F61C
		internal ArrayList ImportedNamespaces
		{
			get
			{
				if (this.importedNamespaces == null)
				{
					this.importedNamespaces = new ArrayList();
				}
				return this.importedNamespaces;
			}
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x00080638 File Offset: 0x0007F638
		internal void GetExternalSchemasList(IList extList, XmlSchema schema)
		{
			if (extList.Contains(schema))
			{
				return;
			}
			extList.Add(schema);
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				if (xmlSchemaExternal.Schema != null)
				{
					this.GetExternalSchemasList(extList, xmlSchemaExternal.Schema);
				}
			}
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x000806B4 File Offset: 0x0007F6B4
		internal void AddCompiledInfo(SchemaInfo schemaInfo)
		{
			foreach (object obj in this.elements.Values)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj;
				XmlQualifiedName xmlQualifiedName = xmlSchemaElement.QualifiedName;
				schemaInfo.TargetNamespaces[xmlQualifiedName.Namespace] = true;
				if (schemaInfo.ElementDecls[xmlQualifiedName] == null)
				{
					schemaInfo.ElementDecls.Add(xmlQualifiedName, xmlSchemaElement.ElementDecl);
				}
			}
			foreach (object obj2 in this.attributes.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj2;
				XmlQualifiedName xmlQualifiedName = xmlSchemaAttribute.QualifiedName;
				schemaInfo.TargetNamespaces[xmlQualifiedName.Namespace] = true;
				if (schemaInfo.ElementDecls[xmlQualifiedName] == null)
				{
					schemaInfo.AttributeDecls.Add(xmlQualifiedName, xmlSchemaAttribute.AttDef);
				}
			}
			foreach (object obj3 in this.types.Values)
			{
				XmlSchemaType xmlSchemaType = (XmlSchemaType)obj3;
				XmlQualifiedName xmlQualifiedName = xmlSchemaType.QualifiedName;
				schemaInfo.TargetNamespaces[xmlQualifiedName.Namespace] = true;
				XmlSchemaComplexType xmlSchemaComplexType = xmlSchemaType as XmlSchemaComplexType;
				if ((xmlSchemaComplexType == null || xmlSchemaType != XmlSchemaComplexType.AnyType) && schemaInfo.ElementDeclsByType[xmlQualifiedName] == null)
				{
					schemaInfo.ElementDeclsByType.Add(xmlQualifiedName, xmlSchemaType.ElementDecl);
				}
			}
			foreach (object obj4 in this.notations.Values)
			{
				XmlSchemaNotation xmlSchemaNotation = (XmlSchemaNotation)obj4;
				XmlQualifiedName xmlQualifiedName = xmlSchemaNotation.QualifiedName;
				schemaInfo.TargetNamespaces[xmlQualifiedName.Namespace] = true;
				SchemaNotation schemaNotation = new SchemaNotation(xmlQualifiedName);
				schemaNotation.SystemLiteral = xmlSchemaNotation.System;
				schemaNotation.Pubid = xmlSchemaNotation.Public;
				if (schemaInfo.Notations[xmlQualifiedName.Name] == null)
				{
					schemaInfo.Notations.Add(xmlQualifiedName.Name, schemaNotation);
				}
			}
		}

		// Token: 0x040010B3 RID: 4275
		public const string Namespace = "http://www.w3.org/2001/XMLSchema";

		// Token: 0x040010B4 RID: 4276
		public const string InstanceNamespace = "http://www.w3.org/2001/XMLSchema-instance";

		// Token: 0x040010B5 RID: 4277
		private XmlSchemaForm attributeFormDefault;

		// Token: 0x040010B6 RID: 4278
		private XmlSchemaForm elementFormDefault;

		// Token: 0x040010B7 RID: 4279
		private XmlSchemaDerivationMethod blockDefault = XmlSchemaDerivationMethod.None;

		// Token: 0x040010B8 RID: 4280
		private XmlSchemaDerivationMethod finalDefault = XmlSchemaDerivationMethod.None;

		// Token: 0x040010B9 RID: 4281
		private string targetNs;

		// Token: 0x040010BA RID: 4282
		private string version;

		// Token: 0x040010BB RID: 4283
		private XmlSchemaObjectCollection includes = new XmlSchemaObjectCollection();

		// Token: 0x040010BC RID: 4284
		private XmlSchemaObjectCollection items = new XmlSchemaObjectCollection();

		// Token: 0x040010BD RID: 4285
		private string id;

		// Token: 0x040010BE RID: 4286
		private XmlAttribute[] moreAttributes;

		// Token: 0x040010BF RID: 4287
		private bool isCompiled;

		// Token: 0x040010C0 RID: 4288
		private bool isCompiledBySet;

		// Token: 0x040010C1 RID: 4289
		private bool isPreprocessed;

		// Token: 0x040010C2 RID: 4290
		private bool isRedefined;

		// Token: 0x040010C3 RID: 4291
		private int errorCount;

		// Token: 0x040010C4 RID: 4292
		private XmlSchemaObjectTable attributes;

		// Token: 0x040010C5 RID: 4293
		private XmlSchemaObjectTable attributeGroups = new XmlSchemaObjectTable();

		// Token: 0x040010C6 RID: 4294
		private XmlSchemaObjectTable elements = new XmlSchemaObjectTable();

		// Token: 0x040010C7 RID: 4295
		private XmlSchemaObjectTable types = new XmlSchemaObjectTable();

		// Token: 0x040010C8 RID: 4296
		private XmlSchemaObjectTable groups = new XmlSchemaObjectTable();

		// Token: 0x040010C9 RID: 4297
		private XmlSchemaObjectTable notations = new XmlSchemaObjectTable();

		// Token: 0x040010CA RID: 4298
		private XmlSchemaObjectTable identityConstraints = new XmlSchemaObjectTable();

		// Token: 0x040010CB RID: 4299
		private static int globalIdCounter = -1;

		// Token: 0x040010CC RID: 4300
		private ArrayList importedSchemas;

		// Token: 0x040010CD RID: 4301
		private ArrayList importedNamespaces;

		// Token: 0x040010CE RID: 4302
		private int schemaId = -1;

		// Token: 0x040010CF RID: 4303
		private Uri baseUri;

		// Token: 0x040010D0 RID: 4304
		private bool isChameleon;

		// Token: 0x040010D1 RID: 4305
		private Hashtable ids = new Hashtable();

		// Token: 0x040010D2 RID: 4306
		private XmlDocument document;

		// Token: 0x040010D3 RID: 4307
		private XmlNameTable nameTable;
	}
}
