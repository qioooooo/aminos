using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x02000320 RID: 800
	public class XmlSchemas : CollectionBase, IEnumerable<XmlSchema>, IEnumerable
	{
		// Token: 0x1700094C RID: 2380
		public XmlSchema this[int index]
		{
			get
			{
				return (XmlSchema)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x1700094D RID: 2381
		public XmlSchema this[string ns]
		{
			get
			{
				IList list = (IList)this.SchemaSet.Schemas(ns);
				if (list.Count == 0)
				{
					return null;
				}
				if (list.Count == 1)
				{
					return (XmlSchema)list[0];
				}
				throw new InvalidOperationException(Res.GetString("XmlSchemaDuplicateNamespace", new object[] { ns }));
			}
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x000B994E File Offset: 0x000B894E
		public IList GetSchemas(string ns)
		{
			return (IList)this.SchemaSet.Schemas(ns);
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x0600261F RID: 9759 RVA: 0x000B9961 File Offset: 0x000B8961
		internal SchemaObjectCache Cache
		{
			get
			{
				if (this.cache == null)
				{
					this.cache = new SchemaObjectCache();
				}
				return this.cache;
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06002620 RID: 9760 RVA: 0x000B997C File Offset: 0x000B897C
		internal Hashtable MergedSchemas
		{
			get
			{
				if (this.mergedSchemas == null)
				{
					this.mergedSchemas = new Hashtable();
				}
				return this.mergedSchemas;
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06002621 RID: 9761 RVA: 0x000B9997 File Offset: 0x000B8997
		internal Hashtable References
		{
			get
			{
				if (this.references == null)
				{
					this.references = new Hashtable();
				}
				return this.references;
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06002622 RID: 9762 RVA: 0x000B99B2 File Offset: 0x000B89B2
		internal XmlSchemaSet SchemaSet
		{
			get
			{
				if (this.schemaSet == null)
				{
					this.schemaSet = new XmlSchemaSet();
					this.schemaSet.XmlResolver = null;
					this.schemaSet.ValidationEventHandler += XmlSchemas.IgnoreCompileErrors;
				}
				return this.schemaSet;
			}
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x000B99F0 File Offset: 0x000B89F0
		internal int Add(XmlSchema schema, bool delay)
		{
			if (delay)
			{
				if (this.delayedSchemas[schema] == null)
				{
					this.delayedSchemas.Add(schema, schema);
				}
				return -1;
			}
			return this.Add(schema);
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x000B9A19 File Offset: 0x000B8A19
		public int Add(XmlSchema schema)
		{
			if (base.List.Contains(schema))
			{
				return base.List.IndexOf(schema);
			}
			return base.List.Add(schema);
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x000B9A42 File Offset: 0x000B8A42
		public int Add(XmlSchema schema, Uri baseUri)
		{
			if (base.List.Contains(schema))
			{
				return base.List.IndexOf(schema);
			}
			if (baseUri != null)
			{
				schema.BaseUri = baseUri;
			}
			return base.List.Add(schema);
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x000B9A7C File Offset: 0x000B8A7C
		public void Add(XmlSchemas schemas)
		{
			foreach (object obj in schemas)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				this.Add(xmlSchema);
			}
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x000B9AD4 File Offset: 0x000B8AD4
		public void AddReference(XmlSchema schema)
		{
			this.References[schema] = schema;
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x000B9AE3 File Offset: 0x000B8AE3
		public void Insert(int index, XmlSchema schema)
		{
			base.List.Insert(index, schema);
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x000B9AF2 File Offset: 0x000B8AF2
		public int IndexOf(XmlSchema schema)
		{
			return base.List.IndexOf(schema);
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x000B9B00 File Offset: 0x000B8B00
		public bool Contains(XmlSchema schema)
		{
			return base.List.Contains(schema);
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x000B9B0E File Offset: 0x000B8B0E
		public bool Contains(string targetNamespace)
		{
			return this.SchemaSet.Contains(targetNamespace);
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x000B9B1C File Offset: 0x000B8B1C
		public void Remove(XmlSchema schema)
		{
			base.List.Remove(schema);
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x000B9B2A File Offset: 0x000B8B2A
		public void CopyTo(XmlSchema[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x000B9B39 File Offset: 0x000B8B39
		protected override void OnInsert(int index, object value)
		{
			this.AddName((XmlSchema)value);
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x000B9B47 File Offset: 0x000B8B47
		protected override void OnRemove(int index, object value)
		{
			this.RemoveName((XmlSchema)value);
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x000B9B55 File Offset: 0x000B8B55
		protected override void OnClear()
		{
			this.schemaSet = null;
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x000B9B5E File Offset: 0x000B8B5E
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			this.RemoveName((XmlSchema)oldValue);
			this.AddName((XmlSchema)newValue);
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x000B9B78 File Offset: 0x000B8B78
		private void AddName(XmlSchema schema)
		{
			if (this.isCompiled)
			{
				throw new InvalidOperationException(Res.GetString("XmlSchemaCompiled"));
			}
			if (this.SchemaSet.Contains(schema))
			{
				this.SchemaSet.Reprocess(schema);
				return;
			}
			this.Prepare(schema);
			this.SchemaSet.Add(schema);
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x000B9BD0 File Offset: 0x000B8BD0
		private void Prepare(XmlSchema schema)
		{
			ArrayList arrayList = new ArrayList();
			string targetNamespace = schema.TargetNamespace;
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				if (xmlSchemaExternal is XmlSchemaImport && targetNamespace == ((XmlSchemaImport)xmlSchemaExternal).Namespace)
				{
					arrayList.Add(xmlSchemaExternal);
				}
			}
			foreach (object obj in arrayList)
			{
				XmlSchemaObject xmlSchemaObject2 = (XmlSchemaObject)obj;
				schema.Includes.Remove(xmlSchemaObject2);
			}
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x000B9CA8 File Offset: 0x000B8CA8
		private void RemoveName(XmlSchema schema)
		{
			this.SchemaSet.Remove(schema);
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x000B9CB7 File Offset: 0x000B8CB7
		public object Find(XmlQualifiedName name, Type type)
		{
			return this.Find(name, type, true);
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x000B9CC4 File Offset: 0x000B8CC4
		internal object Find(XmlQualifiedName name, Type type, bool checkCache)
		{
			if (!this.IsCompiled)
			{
				foreach (object obj in base.List)
				{
					XmlSchema xmlSchema = (XmlSchema)obj;
					XmlSchemas.Preprocess(xmlSchema);
				}
			}
			IList list = (IList)this.SchemaSet.Schemas(name.Namespace);
			if (list == null)
			{
				return null;
			}
			foreach (object obj2 in list)
			{
				XmlSchema xmlSchema2 = (XmlSchema)obj2;
				XmlSchemas.Preprocess(xmlSchema2);
				XmlSchemaObject xmlSchemaObject = null;
				if (typeof(XmlSchemaType).IsAssignableFrom(type))
				{
					xmlSchemaObject = xmlSchema2.SchemaTypes[name];
					if (xmlSchemaObject == null)
					{
						continue;
					}
					if (!type.IsAssignableFrom(xmlSchemaObject.GetType()))
					{
						continue;
					}
				}
				else if (type == typeof(XmlSchemaGroup))
				{
					xmlSchemaObject = xmlSchema2.Groups[name];
				}
				else if (type == typeof(XmlSchemaAttributeGroup))
				{
					xmlSchemaObject = xmlSchema2.AttributeGroups[name];
				}
				else if (type == typeof(XmlSchemaElement))
				{
					xmlSchemaObject = xmlSchema2.Elements[name];
				}
				else if (type == typeof(XmlSchemaAttribute))
				{
					xmlSchemaObject = xmlSchema2.Attributes[name];
				}
				else if (type == typeof(XmlSchemaNotation))
				{
					xmlSchemaObject = xmlSchema2.Notations[name];
				}
				if (xmlSchemaObject != null && this.shareTypes && checkCache && !this.IsReference(xmlSchemaObject))
				{
					xmlSchemaObject = this.Cache.AddItem(xmlSchemaObject, name, this);
				}
				if (xmlSchemaObject != null)
				{
					return xmlSchemaObject;
				}
			}
			return null;
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x000B9EA8 File Offset: 0x000B8EA8
		IEnumerator<XmlSchema> IEnumerable<XmlSchema>.GetEnumerator()
		{
			return new XmlSchemaEnumerator(this);
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x000B9EB0 File Offset: 0x000B8EB0
		internal static void Preprocess(XmlSchema schema)
		{
			if (!schema.IsPreprocessed)
			{
				try
				{
					XmlNameTable xmlNameTable = new NameTable();
					new Preprocessor(xmlNameTable, new SchemaNames(xmlNameTable), null)
					{
						SchemaLocations = new Hashtable()
					}.Execute(schema, schema.TargetNamespace, false);
				}
				catch (XmlSchemaException ex)
				{
					throw XmlSchemas.CreateValidationException(ex, ex.Message);
				}
			}
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x000B9F14 File Offset: 0x000B8F14
		public static bool IsDataSet(XmlSchema schema)
		{
			foreach (XmlSchemaObject xmlSchemaObject in schema.Items)
			{
				if (xmlSchemaObject is XmlSchemaElement)
				{
					XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)xmlSchemaObject;
					if (xmlSchemaElement.UnhandledAttributes != null)
					{
						foreach (XmlAttribute xmlAttribute in xmlSchemaElement.UnhandledAttributes)
						{
							if (xmlAttribute.LocalName == "IsDataSet" && xmlAttribute.NamespaceURI == "urn:schemas-microsoft-com:xml-msdata" && (xmlAttribute.Value == "True" || xmlAttribute.Value == "true" || xmlAttribute.Value == "1"))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x000BA00C File Offset: 0x000B900C
		private void Merge(XmlSchema schema)
		{
			if (this.MergedSchemas[schema] != null)
			{
				return;
			}
			IList list = (IList)this.SchemaSet.Schemas(schema.TargetNamespace);
			if (list != null && list.Count > 0)
			{
				this.MergedSchemas.Add(schema, schema);
				this.Merge(list, schema);
				return;
			}
			this.Add(schema);
			this.MergedSchemas.Add(schema, schema);
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x000BA078 File Offset: 0x000B9078
		private void AddImport(IList schemas, string ns)
		{
			foreach (object obj in schemas)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				bool flag = true;
				foreach (XmlSchemaObject xmlSchemaObject in xmlSchema.Includes)
				{
					XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
					if (xmlSchemaExternal is XmlSchemaImport && ((XmlSchemaImport)xmlSchemaExternal).Namespace == ns)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					XmlSchemaImport xmlSchemaImport = new XmlSchemaImport();
					xmlSchemaImport.Namespace = ns;
					xmlSchema.Includes.Add(xmlSchemaImport);
				}
			}
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x000BA158 File Offset: 0x000B9158
		private void Merge(IList originals, XmlSchema schema)
		{
			foreach (object obj in originals)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (schema == xmlSchema)
				{
					return;
				}
			}
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				if (xmlSchemaExternal is XmlSchemaImport)
				{
					xmlSchemaExternal.SchemaLocation = null;
					if (xmlSchemaExternal.Schema != null)
					{
						this.Merge(xmlSchemaExternal.Schema);
					}
					else
					{
						this.AddImport(originals, ((XmlSchemaImport)xmlSchemaExternal).Namespace);
					}
				}
				else if (xmlSchemaExternal.Schema == null)
				{
					if (xmlSchemaExternal.SchemaLocation != null)
					{
						throw new InvalidOperationException(Res.GetString("XmlSchemaIncludeLocation", new object[]
						{
							base.GetType().Name,
							xmlSchemaExternal.SchemaLocation
						}));
					}
				}
				else
				{
					xmlSchemaExternal.SchemaLocation = null;
					this.Merge(originals, xmlSchemaExternal.Schema);
				}
			}
			bool[] array = new bool[schema.Items.Count];
			int num = 0;
			for (int i = 0; i < schema.Items.Count; i++)
			{
				XmlSchemaObject xmlSchemaObject2 = schema.Items[i];
				XmlSchemaObject xmlSchemaObject3 = this.Find(xmlSchemaObject2, originals);
				if (xmlSchemaObject3 != null)
				{
					if (!this.Cache.Match(xmlSchemaObject3, xmlSchemaObject2, this.shareTypes))
					{
						throw new InvalidOperationException(XmlSchemas.MergeFailedMessage(xmlSchemaObject2, xmlSchemaObject3, schema.TargetNamespace));
					}
					array[i] = true;
					num++;
				}
			}
			if (num != schema.Items.Count)
			{
				XmlSchema xmlSchema2 = (XmlSchema)originals[0];
				for (int j = 0; j < schema.Items.Count; j++)
				{
					if (!array[j])
					{
						xmlSchema2.Items.Add(schema.Items[j]);
					}
				}
				xmlSchema2.IsPreprocessed = false;
				XmlSchemas.Preprocess(xmlSchema2);
			}
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x000BA378 File Offset: 0x000B9378
		private static string ItemName(XmlSchemaObject o)
		{
			if (o is XmlSchemaNotation)
			{
				return ((XmlSchemaNotation)o).Name;
			}
			if (o is XmlSchemaGroup)
			{
				return ((XmlSchemaGroup)o).Name;
			}
			if (o is XmlSchemaElement)
			{
				return ((XmlSchemaElement)o).Name;
			}
			if (o is XmlSchemaType)
			{
				return ((XmlSchemaType)o).Name;
			}
			if (o is XmlSchemaAttributeGroup)
			{
				return ((XmlSchemaAttributeGroup)o).Name;
			}
			if (o is XmlSchemaAttribute)
			{
				return ((XmlSchemaAttribute)o).Name;
			}
			return null;
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x000BA400 File Offset: 0x000B9400
		internal static XmlQualifiedName GetParentName(XmlSchemaObject item)
		{
			while (item.Parent != null)
			{
				if (item.Parent is XmlSchemaType)
				{
					XmlSchemaType xmlSchemaType = (XmlSchemaType)item.Parent;
					if (xmlSchemaType.Name != null && xmlSchemaType.Name.Length != 0)
					{
						return xmlSchemaType.QualifiedName;
					}
				}
				item = item.Parent;
			}
			return XmlQualifiedName.Empty;
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x000BA45C File Offset: 0x000B945C
		private static string GetSchemaItem(XmlSchemaObject o, string ns, string details)
		{
			if (o == null)
			{
				return null;
			}
			while (o.Parent != null && !(o.Parent is XmlSchema))
			{
				o = o.Parent;
			}
			if (ns == null || ns.Length == 0)
			{
				XmlSchemaObject xmlSchemaObject = o;
				while (xmlSchemaObject.Parent != null)
				{
					xmlSchemaObject = xmlSchemaObject.Parent;
				}
				if (xmlSchemaObject is XmlSchema)
				{
					ns = ((XmlSchema)xmlSchemaObject).TargetNamespace;
				}
			}
			string text;
			if (o is XmlSchemaNotation)
			{
				text = Res.GetString("XmlSchemaNamedItem", new object[]
				{
					ns,
					"notation",
					((XmlSchemaNotation)o).Name,
					details
				});
			}
			else if (o is XmlSchemaGroup)
			{
				text = Res.GetString("XmlSchemaNamedItem", new object[]
				{
					ns,
					"group",
					((XmlSchemaGroup)o).Name,
					details
				});
			}
			else if (o is XmlSchemaElement)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)o;
				if (xmlSchemaElement.Name == null || xmlSchemaElement.Name.Length == 0)
				{
					XmlQualifiedName parentName = XmlSchemas.GetParentName(o);
					text = Res.GetString("XmlSchemaElementReference", new object[]
					{
						xmlSchemaElement.RefName.ToString(),
						parentName.Name,
						parentName.Namespace
					});
				}
				else
				{
					text = Res.GetString("XmlSchemaNamedItem", new object[] { ns, "element", xmlSchemaElement.Name, details });
				}
			}
			else if (o is XmlSchemaType)
			{
				string text2 = "XmlSchemaNamedItem";
				object[] array = new object[4];
				array[0] = ns;
				array[1] = ((o.GetType() == typeof(XmlSchemaSimpleType)) ? "simpleType" : "complexType");
				array[2] = ((XmlSchemaType)o).Name;
				text = Res.GetString(text2, array);
			}
			else if (o is XmlSchemaAttributeGroup)
			{
				text = Res.GetString("XmlSchemaNamedItem", new object[]
				{
					ns,
					"attributeGroup",
					((XmlSchemaAttributeGroup)o).Name,
					details
				});
			}
			else if (o is XmlSchemaAttribute)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)o;
				if (xmlSchemaAttribute.Name == null || xmlSchemaAttribute.Name.Length == 0)
				{
					XmlQualifiedName parentName2 = XmlSchemas.GetParentName(o);
					return Res.GetString("XmlSchemaAttributeReference", new object[]
					{
						xmlSchemaAttribute.RefName.ToString(),
						parentName2.Name,
						parentName2.Namespace
					});
				}
				text = Res.GetString("XmlSchemaNamedItem", new object[] { ns, "attribute", xmlSchemaAttribute.Name, details });
			}
			else if (o is XmlSchemaContent)
			{
				XmlQualifiedName parentName3 = XmlSchemas.GetParentName(o);
				string text3 = "XmlSchemaContentDef";
				object[] array2 = new object[3];
				array2[0] = parentName3.Name;
				array2[1] = parentName3.Namespace;
				text = Res.GetString(text3, array2);
			}
			else if (o is XmlSchemaExternal)
			{
				string text4 = ((o is XmlSchemaImport) ? "import" : ((o is XmlSchemaInclude) ? "include" : ((o is XmlSchemaRedefine) ? "redefine" : o.GetType().Name)));
				text = Res.GetString("XmlSchemaItem", new object[] { ns, text4, details });
			}
			else if (o is XmlSchema)
			{
				text = Res.GetString("XmlSchema", new object[] { ns, details });
			}
			else
			{
				text = Res.GetString("XmlSchemaNamedItem", new object[]
				{
					ns,
					o.GetType().Name,
					null,
					details
				});
			}
			return text;
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000BA82C File Offset: 0x000B982C
		private static string Dump(XmlSchemaObject o)
		{
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.OmitXmlDeclaration = true;
			xmlWriterSettings.Indent = true;
			XmlSerializer xmlSerializer = new XmlSerializer(o.GetType());
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add("xs", "http://www.w3.org/2001/XMLSchema");
			xmlSerializer.Serialize(xmlWriter, o, xmlSerializerNamespaces);
			return stringWriter.ToString();
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x000BA894 File Offset: 0x000B9894
		private static string MergeFailedMessage(XmlSchemaObject src, XmlSchemaObject dest, string ns)
		{
			string text = Res.GetString("XmlSerializableMergeItem", new object[]
			{
				ns,
				XmlSchemas.GetSchemaItem(src, ns, null)
			});
			text = text + "\r\n" + XmlSchemas.Dump(src);
			return text + "\r\n" + XmlSchemas.Dump(dest);
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x000BA8E8 File Offset: 0x000B98E8
		internal XmlSchemaObject Find(XmlSchemaObject o, IList originals)
		{
			string text = XmlSchemas.ItemName(o);
			if (text == null)
			{
				return null;
			}
			Type type = o.GetType();
			foreach (object obj in originals)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				foreach (XmlSchemaObject xmlSchemaObject in xmlSchema.Items)
				{
					if (xmlSchemaObject.GetType() == type && text == XmlSchemas.ItemName(xmlSchemaObject))
					{
						return xmlSchemaObject;
					}
				}
			}
			return null;
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06002643 RID: 9795 RVA: 0x000BA9B4 File Offset: 0x000B99B4
		public bool IsCompiled
		{
			get
			{
				return this.isCompiled;
			}
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x000BA9BC File Offset: 0x000B99BC
		public void Compile(ValidationEventHandler handler, bool fullCompile)
		{
			if (this.isCompiled)
			{
				return;
			}
			foreach (object obj in this.delayedSchemas.Values)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				this.Merge(xmlSchema);
			}
			this.delayedSchemas.Clear();
			if (fullCompile)
			{
				this.schemaSet = new XmlSchemaSet();
				this.schemaSet.XmlResolver = null;
				this.schemaSet.ValidationEventHandler += handler;
				foreach (object obj2 in this.References.Values)
				{
					XmlSchema xmlSchema2 = (XmlSchema)obj2;
					this.schemaSet.Add(xmlSchema2);
				}
				int num = this.schemaSet.Count;
				foreach (object obj3 in base.List)
				{
					XmlSchema xmlSchema3 = (XmlSchema)obj3;
					if (!this.SchemaSet.Contains(xmlSchema3))
					{
						this.schemaSet.Add(xmlSchema3);
						num++;
					}
				}
				if (!this.SchemaSet.Contains("http://www.w3.org/2001/XMLSchema"))
				{
					this.AddReference(XmlSchemas.XsdSchema);
					this.schemaSet.Add(XmlSchemas.XsdSchema);
					num++;
				}
				if (!this.SchemaSet.Contains("http://www.w3.org/XML/1998/namespace"))
				{
					this.AddReference(XmlSchemas.XmlSchema);
					this.schemaSet.Add(XmlSchemas.XmlSchema);
					num++;
				}
				this.schemaSet.Compile();
				this.schemaSet.ValidationEventHandler -= handler;
				this.isCompiled = this.schemaSet.IsCompiled && num == this.schemaSet.Count;
				return;
			}
			try
			{
				XmlNameTable xmlNameTable = new NameTable();
				Preprocessor preprocessor = new Preprocessor(xmlNameTable, new SchemaNames(xmlNameTable), null);
				preprocessor.XmlResolver = null;
				preprocessor.SchemaLocations = new Hashtable();
				preprocessor.ChameleonSchemas = new Hashtable();
				foreach (object obj4 in this.SchemaSet.Schemas())
				{
					XmlSchema xmlSchema4 = (XmlSchema)obj4;
					preprocessor.Execute(xmlSchema4, xmlSchema4.TargetNamespace, true);
				}
			}
			catch (XmlSchemaException ex)
			{
				throw XmlSchemas.CreateValidationException(ex, ex.Message);
			}
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x000BAC84 File Offset: 0x000B9C84
		internal static Exception CreateValidationException(XmlSchemaException exception, string message)
		{
			XmlSchemaObject xmlSchemaObject = exception.SourceSchemaObject;
			if (exception.LineNumber == 0 && exception.LinePosition == 0)
			{
				throw new InvalidOperationException(XmlSchemas.GetSchemaItem(xmlSchemaObject, null, message), exception);
			}
			string text = null;
			if (xmlSchemaObject != null)
			{
				while (xmlSchemaObject.Parent != null)
				{
					xmlSchemaObject = xmlSchemaObject.Parent;
				}
				if (xmlSchemaObject is XmlSchema)
				{
					text = ((XmlSchema)xmlSchemaObject).TargetNamespace;
				}
			}
			throw new InvalidOperationException(Res.GetString("XmlSchemaSyntaxErrorDetails", new object[] { text, message, exception.LineNumber, exception.LinePosition }), exception);
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x000BAD1D File Offset: 0x000B9D1D
		internal static void IgnoreCompileErrors(object sender, ValidationEventArgs args)
		{
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x06002647 RID: 9799 RVA: 0x000BAD1F File Offset: 0x000B9D1F
		internal static XmlSchema XsdSchema
		{
			get
			{
				if (XmlSchemas.xsd == null)
				{
					XmlSchemas.xsd = XmlSchemas.CreateFakeXsdSchema("http://www.w3.org/2001/XMLSchema", "schema");
				}
				return XmlSchemas.xsd;
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06002648 RID: 9800 RVA: 0x000BAD41 File Offset: 0x000B9D41
		internal static XmlSchema XmlSchema
		{
			get
			{
				if (XmlSchemas.xml == null)
				{
					XmlSchemas.xml = XmlSchema.Read(new StringReader("<?xml version='1.0' encoding='UTF-8' ?> \r\n<xs:schema targetNamespace='http://www.w3.org/XML/1998/namespace' xmlns:xs='http://www.w3.org/2001/XMLSchema' xml:lang='en'>\r\n <xs:attribute name='lang' type='xs:language'/>\r\n <xs:attribute name='space'>\r\n  <xs:simpleType>\r\n   <xs:restriction base='xs:NCName'>\r\n    <xs:enumeration value='default'/>\r\n    <xs:enumeration value='preserve'/>\r\n   </xs:restriction>\r\n  </xs:simpleType>\r\n </xs:attribute>\r\n <xs:attribute name='base' type='xs:anyURI'/>\r\n <xs:attribute name='id' type='xs:ID' />\r\n <xs:attributeGroup name='specialAttrs'>\r\n  <xs:attribute ref='xml:base'/>\r\n  <xs:attribute ref='xml:lang'/>\r\n  <xs:attribute ref='xml:space'/>\r\n </xs:attributeGroup>\r\n</xs:schema>"), null);
				}
				return XmlSchemas.xml;
			}
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x000BAD64 File Offset: 0x000B9D64
		private static XmlSchema CreateFakeXsdSchema(string ns, string name)
		{
			XmlSchema xmlSchema = new XmlSchema();
			xmlSchema.TargetNamespace = ns;
			XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
			xmlSchemaElement.Name = name;
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			xmlSchemaElement.SchemaType = xmlSchemaComplexType;
			xmlSchema.Items.Add(xmlSchemaElement);
			return xmlSchema;
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x000BADA6 File Offset: 0x000B9DA6
		internal void SetCache(SchemaObjectCache cache, bool shareTypes)
		{
			this.shareTypes = shareTypes;
			this.cache = cache;
			if (shareTypes)
			{
				cache.GenerateSchemaGraph(this);
			}
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x000BADC0 File Offset: 0x000B9DC0
		internal bool IsReference(XmlSchemaObject type)
		{
			XmlSchemaObject xmlSchemaObject = type;
			while (xmlSchemaObject.Parent != null)
			{
				xmlSchemaObject = xmlSchemaObject.Parent;
			}
			return this.References.Contains(xmlSchemaObject);
		}

		// Token: 0x040015C3 RID: 5571
		internal const string xmlSchema = "<?xml version='1.0' encoding='UTF-8' ?> \r\n<xs:schema targetNamespace='http://www.w3.org/XML/1998/namespace' xmlns:xs='http://www.w3.org/2001/XMLSchema' xml:lang='en'>\r\n <xs:attribute name='lang' type='xs:language'/>\r\n <xs:attribute name='space'>\r\n  <xs:simpleType>\r\n   <xs:restriction base='xs:NCName'>\r\n    <xs:enumeration value='default'/>\r\n    <xs:enumeration value='preserve'/>\r\n   </xs:restriction>\r\n  </xs:simpleType>\r\n </xs:attribute>\r\n <xs:attribute name='base' type='xs:anyURI'/>\r\n <xs:attribute name='id' type='xs:ID' />\r\n <xs:attributeGroup name='specialAttrs'>\r\n  <xs:attribute ref='xml:base'/>\r\n  <xs:attribute ref='xml:lang'/>\r\n  <xs:attribute ref='xml:space'/>\r\n </xs:attributeGroup>\r\n</xs:schema>";

		// Token: 0x040015C4 RID: 5572
		private XmlSchemaSet schemaSet;

		// Token: 0x040015C5 RID: 5573
		private Hashtable references;

		// Token: 0x040015C6 RID: 5574
		private SchemaObjectCache cache;

		// Token: 0x040015C7 RID: 5575
		private bool shareTypes;

		// Token: 0x040015C8 RID: 5576
		private Hashtable mergedSchemas;

		// Token: 0x040015C9 RID: 5577
		internal Hashtable delayedSchemas = new Hashtable();

		// Token: 0x040015CA RID: 5578
		private bool isCompiled;

		// Token: 0x040015CB RID: 5579
		private static XmlSchema xsd;

		// Token: 0x040015CC RID: 5580
		private static XmlSchema xml;
	}
}
