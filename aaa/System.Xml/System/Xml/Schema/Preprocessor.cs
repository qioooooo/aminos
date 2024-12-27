using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace System.Xml.Schema
{
	// Token: 0x02000207 RID: 519
	internal sealed class Preprocessor : BaseProcessor
	{
		// Token: 0x0600187A RID: 6266 RVA: 0x0006E0BE File Offset: 0x0006D0BE
		public Preprocessor(XmlNameTable nameTable, SchemaNames schemaNames, ValidationEventHandler eventHandler)
			: this(nameTable, schemaNames, eventHandler, new XmlSchemaCompilationSettings())
		{
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x0006E0CE File Offset: 0x0006D0CE
		public Preprocessor(XmlNameTable nameTable, SchemaNames schemaNames, ValidationEventHandler eventHandler, XmlSchemaCompilationSettings compilationSettings)
			: base(nameTable, schemaNames, eventHandler, compilationSettings)
		{
			this.referenceNamespaces = new Hashtable();
			this.processedExternals = new Hashtable();
			this.lockList = new SortedList();
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x0006E0FC File Offset: 0x0006D0FC
		public bool Execute(XmlSchema schema, string targetNamespace, bool loadExternals)
		{
			this.rootSchema = schema;
			this.Xmlns = base.NameTable.Add("xmlns");
			this.NsXsi = base.NameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
			this.rootSchema.ImportedSchemas.Clear();
			this.rootSchema.ImportedNamespaces.Clear();
			if (this.rootSchema.BaseUri != null && this.schemaLocations[this.rootSchema.BaseUri] == null)
			{
				this.schemaLocations.Add(this.rootSchema.BaseUri, this.rootSchema);
			}
			if (this.rootSchema.TargetNamespace != null)
			{
				if (targetNamespace == null)
				{
					targetNamespace = this.rootSchema.TargetNamespace;
				}
				else if (targetNamespace != this.rootSchema.TargetNamespace)
				{
					base.SendValidationEvent("Sch_MismatchTargetNamespaceEx", targetNamespace, this.rootSchema.TargetNamespace, this.rootSchema);
				}
			}
			else if (targetNamespace != null && targetNamespace.Length != 0)
			{
				this.rootSchema = this.GetChameleonSchema(targetNamespace, this.rootSchema);
			}
			if (loadExternals && this.xmlResolver != null)
			{
				this.LoadExternals(this.rootSchema);
			}
			this.BuildSchemaList(this.rootSchema);
			int i = 0;
			try
			{
				for (i = 0; i < this.lockList.Count; i++)
				{
					XmlSchema xmlSchema = (XmlSchema)this.lockList.GetByIndex(i);
					Monitor.Enter(xmlSchema);
					xmlSchema.IsProcessing = false;
				}
				this.rootSchemaForRedefine = this.rootSchema;
				this.Preprocess(this.rootSchema, targetNamespace, this.rootSchema.ImportedSchemas);
				if (this.redefinedList != null)
				{
					foreach (object obj in this.redefinedList)
					{
						RedefineEntry redefineEntry = (RedefineEntry)obj;
						this.PreprocessRedefine(redefineEntry);
					}
				}
			}
			finally
			{
				if (i == this.lockList.Count)
				{
					i--;
				}
				while (i >= 0)
				{
					XmlSchema xmlSchema = (XmlSchema)this.lockList.GetByIndex(i);
					xmlSchema.IsProcessing = false;
					if (xmlSchema == Preprocessor.GetBuildInSchema())
					{
						Monitor.Exit(xmlSchema);
					}
					else
					{
						xmlSchema.IsCompiledBySet = false;
						xmlSchema.IsPreprocessed = !base.HasErrors;
						Monitor.Exit(xmlSchema);
					}
					i--;
				}
			}
			this.rootSchema.IsPreprocessed = !base.HasErrors;
			return !base.HasErrors;
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x0006E374 File Offset: 0x0006D374
		private void Cleanup(XmlSchema schema)
		{
			if (schema == Preprocessor.GetBuildInSchema())
			{
				return;
			}
			schema.Attributes.Clear();
			schema.AttributeGroups.Clear();
			schema.SchemaTypes.Clear();
			schema.Elements.Clear();
			schema.Groups.Clear();
			schema.Notations.Clear();
			schema.Ids.Clear();
			schema.IdentityConstraints.Clear();
			schema.IsRedefined = false;
			schema.IsCompiledBySet = false;
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x0006E3F0 File Offset: 0x0006D3F0
		private void CleanupRedefine(XmlSchemaExternal include)
		{
			XmlSchemaRedefine xmlSchemaRedefine = include as XmlSchemaRedefine;
			xmlSchemaRedefine.AttributeGroups.Clear();
			xmlSchemaRedefine.Groups.Clear();
			xmlSchemaRedefine.SchemaTypes.Clear();
		}

		// Token: 0x17000619 RID: 1561
		// (set) Token: 0x0600187F RID: 6271 RVA: 0x0006E425 File Offset: 0x0006D425
		internal XmlResolver XmlResolver
		{
			set
			{
				this.xmlResolver = value;
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06001880 RID: 6272 RVA: 0x0006E42E File Offset: 0x0006D42E
		// (set) Token: 0x06001881 RID: 6273 RVA: 0x0006E455 File Offset: 0x0006D455
		internal XmlReaderSettings ReaderSettings
		{
			get
			{
				if (this.readerSettings == null)
				{
					this.readerSettings = new XmlReaderSettings();
					this.readerSettings.ProhibitDtd = true;
				}
				return this.readerSettings;
			}
			set
			{
				this.readerSettings = value;
			}
		}

		// Token: 0x1700061B RID: 1563
		// (set) Token: 0x06001882 RID: 6274 RVA: 0x0006E45E File Offset: 0x0006D45E
		internal Hashtable SchemaLocations
		{
			set
			{
				this.schemaLocations = value;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (set) Token: 0x06001883 RID: 6275 RVA: 0x0006E467 File Offset: 0x0006D467
		internal Hashtable ChameleonSchemas
		{
			set
			{
				this.chameleonSchemas = value;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001884 RID: 6276 RVA: 0x0006E470 File Offset: 0x0006D470
		internal XmlSchema RootSchema
		{
			get
			{
				return this.rootSchema;
			}
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x0006E478 File Offset: 0x0006D478
		private void BuildSchemaList(XmlSchema schema)
		{
			if (this.lockList.Contains(schema.SchemaId))
			{
				return;
			}
			this.lockList.Add(schema.SchemaId, schema);
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				if (xmlSchemaExternal.Schema != null)
				{
					this.BuildSchemaList(xmlSchemaExternal.Schema);
				}
			}
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x0006E510 File Offset: 0x0006D510
		private void LoadExternals(XmlSchema schema)
		{
			if (schema.IsProcessing)
			{
				return;
			}
			schema.IsProcessing = true;
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				XmlSchema xmlSchema = xmlSchemaExternal.Schema;
				if (xmlSchema != null)
				{
					Uri baseUri = xmlSchema.BaseUri;
					if (baseUri != null && this.schemaLocations[baseUri] == null)
					{
						this.schemaLocations.Add(baseUri, xmlSchema);
					}
					this.LoadExternals(xmlSchema);
				}
				else
				{
					string schemaLocation = xmlSchemaExternal.SchemaLocation;
					Uri uri = null;
					Exception ex = null;
					if (schemaLocation != null)
					{
						try
						{
							uri = this.ResolveSchemaLocationUri(schema, schemaLocation);
						}
						catch (Exception ex2)
						{
							uri = null;
							ex = ex2;
						}
					}
					if (xmlSchemaExternal.Compositor == Compositor.Import)
					{
						XmlSchemaImport xmlSchemaImport = xmlSchemaExternal as XmlSchemaImport;
						string text = ((xmlSchemaImport.Namespace != null) ? xmlSchemaImport.Namespace : string.Empty);
						if (!schema.ImportedNamespaces.Contains(text))
						{
							schema.ImportedNamespaces.Add(text);
						}
						if (text == "http://www.w3.org/XML/1998/namespace" && uri == null)
						{
							xmlSchemaExternal.Schema = Preprocessor.GetBuildInSchema();
							continue;
						}
					}
					if (uri == null)
					{
						if (schemaLocation != null)
						{
							base.SendValidationEvent(new XmlSchemaException("Sch_InvalidIncludeLocation", null, ex, xmlSchemaExternal.SourceUri, xmlSchemaExternal.LineNumber, xmlSchemaExternal.LinePosition, xmlSchemaExternal), XmlSeverityType.Warning);
						}
					}
					else if (this.schemaLocations[uri] == null)
					{
						object obj = null;
						try
						{
							obj = this.GetSchemaEntity(uri);
						}
						catch (Exception ex3)
						{
							ex = ex3;
							obj = null;
						}
						if (obj != null)
						{
							xmlSchemaExternal.BaseUri = uri;
							Type type = obj.GetType();
							if (typeof(XmlSchema).IsAssignableFrom(type))
							{
								xmlSchemaExternal.Schema = (XmlSchema)obj;
								this.schemaLocations.Add(uri, xmlSchemaExternal.Schema);
								this.LoadExternals(xmlSchemaExternal.Schema);
								continue;
							}
							XmlReader xmlReader = null;
							if (type.IsSubclassOf(typeof(Stream)))
							{
								this.readerSettings.CloseInput = true;
								this.readerSettings.XmlResolver = this.xmlResolver;
								xmlReader = XmlReader.Create((Stream)obj, this.readerSettings, uri.ToString());
							}
							else if (type.IsSubclassOf(typeof(XmlReader)))
							{
								xmlReader = (XmlReader)obj;
							}
							else if (type.IsSubclassOf(typeof(TextReader)))
							{
								this.readerSettings.CloseInput = true;
								this.readerSettings.XmlResolver = this.xmlResolver;
								xmlReader = XmlReader.Create((TextReader)obj, this.readerSettings, uri.ToString());
							}
							if (xmlReader == null)
							{
								base.SendValidationEvent("Sch_InvalidIncludeLocation", xmlSchemaExternal, XmlSeverityType.Warning);
								continue;
							}
							try
							{
								try
								{
									Parser parser = new Parser(SchemaType.XSD, base.NameTable, base.SchemaNames, base.EventHandler);
									parser.Parse(xmlReader, null);
									while (xmlReader.Read())
									{
									}
									xmlSchema = parser.XmlSchema;
									xmlSchemaExternal.Schema = xmlSchema;
									this.schemaLocations.Add(uri, xmlSchema);
									this.LoadExternals(xmlSchema);
								}
								catch (XmlSchemaException ex4)
								{
									base.SendValidationEvent("Sch_CannotLoadSchemaLocation", schemaLocation, ex4.Message, ex4.SourceUri, ex4.LineNumber, ex4.LinePosition);
								}
								catch (Exception ex5)
								{
									base.SendValidationEvent(new XmlSchemaException("Sch_InvalidIncludeLocation", null, ex5, xmlSchemaExternal.SourceUri, xmlSchemaExternal.LineNumber, xmlSchemaExternal.LinePosition, xmlSchemaExternal), XmlSeverityType.Warning);
								}
								continue;
							}
							finally
							{
								xmlReader.Close();
							}
						}
						base.SendValidationEvent(new XmlSchemaException("Sch_InvalidIncludeLocation", null, ex, xmlSchemaExternal.SourceUri, xmlSchemaExternal.LineNumber, xmlSchemaExternal.LinePosition, xmlSchemaExternal), XmlSeverityType.Warning);
					}
					else
					{
						xmlSchemaExternal.Schema = (XmlSchema)this.schemaLocations[uri];
					}
				}
			}
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x0006E968 File Offset: 0x0006D968
		internal static XmlSchema GetBuildInSchema()
		{
			if (Preprocessor.builtInSchemaForXmlNS == null)
			{
				XmlSchema xmlSchema = new XmlSchema();
				xmlSchema.TargetNamespace = "http://www.w3.org/XML/1998/namespace";
				xmlSchema.Namespaces.Add("xml", "http://www.w3.org/XML/1998/namespace");
				XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
				xmlSchemaAttribute.Name = "lang";
				xmlSchemaAttribute.SchemaTypeName = new XmlQualifiedName("language", "http://www.w3.org/2001/XMLSchema");
				xmlSchema.Items.Add(xmlSchemaAttribute);
				XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
				xmlSchemaAttribute2.Name = "base";
				xmlSchemaAttribute2.SchemaTypeName = new XmlQualifiedName("anyURI", "http://www.w3.org/2001/XMLSchema");
				xmlSchema.Items.Add(xmlSchemaAttribute2);
				XmlSchemaAttribute xmlSchemaAttribute3 = new XmlSchemaAttribute();
				xmlSchemaAttribute3.Name = "space";
				XmlSchemaSimpleType xmlSchemaSimpleType = new XmlSchemaSimpleType();
				XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = new XmlSchemaSimpleTypeRestriction();
				xmlSchemaSimpleTypeRestriction.BaseTypeName = new XmlQualifiedName("NCName", "http://www.w3.org/2001/XMLSchema");
				XmlSchemaEnumerationFacet xmlSchemaEnumerationFacet = new XmlSchemaEnumerationFacet();
				xmlSchemaEnumerationFacet.Value = "default";
				xmlSchemaSimpleTypeRestriction.Facets.Add(xmlSchemaEnumerationFacet);
				XmlSchemaEnumerationFacet xmlSchemaEnumerationFacet2 = new XmlSchemaEnumerationFacet();
				xmlSchemaEnumerationFacet2.Value = "preserve";
				xmlSchemaSimpleTypeRestriction.Facets.Add(xmlSchemaEnumerationFacet2);
				xmlSchemaSimpleType.Content = xmlSchemaSimpleTypeRestriction;
				xmlSchemaAttribute3.SchemaType = xmlSchemaSimpleType;
				xmlSchemaAttribute3.DefaultValue = "preserve";
				xmlSchema.Items.Add(xmlSchemaAttribute3);
				XmlSchemaAttributeGroup xmlSchemaAttributeGroup = new XmlSchemaAttributeGroup();
				xmlSchemaAttributeGroup.Name = "specialAttrs";
				XmlSchemaAttribute xmlSchemaAttribute4 = new XmlSchemaAttribute();
				xmlSchemaAttribute4.RefName = new XmlQualifiedName("lang", "http://www.w3.org/XML/1998/namespace");
				xmlSchemaAttributeGroup.Attributes.Add(xmlSchemaAttribute4);
				XmlSchemaAttribute xmlSchemaAttribute5 = new XmlSchemaAttribute();
				xmlSchemaAttribute5.RefName = new XmlQualifiedName("space", "http://www.w3.org/XML/1998/namespace");
				xmlSchemaAttributeGroup.Attributes.Add(xmlSchemaAttribute5);
				XmlSchemaAttribute xmlSchemaAttribute6 = new XmlSchemaAttribute();
				xmlSchemaAttribute6.RefName = new XmlQualifiedName("base", "http://www.w3.org/XML/1998/namespace");
				xmlSchemaAttributeGroup.Attributes.Add(xmlSchemaAttribute6);
				xmlSchema.Items.Add(xmlSchemaAttributeGroup);
				xmlSchema.IsPreprocessed = true;
				xmlSchema.CompileSchemaInSet(new NameTable(), null, null);
				Interlocked.CompareExchange<XmlSchema>(ref Preprocessor.builtInSchemaForXmlNS, xmlSchema, null);
			}
			return Preprocessor.builtInSchemaForXmlNS;
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x0006EB78 File Offset: 0x0006DB78
		private void BuildRefNamespaces(XmlSchema schema)
		{
			this.referenceNamespaces.Clear();
			this.referenceNamespaces.Add("http://www.w3.org/2001/XMLSchema", "http://www.w3.org/2001/XMLSchema");
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				if (xmlSchemaExternal is XmlSchemaImport)
				{
					XmlSchemaImport xmlSchemaImport = xmlSchemaExternal as XmlSchemaImport;
					string text = xmlSchemaImport.Namespace;
					if (text == null)
					{
						text = string.Empty;
					}
					if (this.referenceNamespaces[text] == null)
					{
						this.referenceNamespaces.Add(text, text);
					}
				}
			}
			string empty = schema.TargetNamespace;
			if (empty == null)
			{
				empty = string.Empty;
			}
			if (this.referenceNamespaces[empty] == null)
			{
				this.referenceNamespaces.Add(empty, empty);
			}
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x0006EC58 File Offset: 0x0006DC58
		private void ParseUri(string uri, string code, XmlSchemaObject sourceSchemaObject)
		{
			try
			{
				XmlConvert.ToUri(uri);
			}
			catch (FormatException ex)
			{
				base.SendValidationEvent(code, new string[] { uri }, ex, sourceSchemaObject);
			}
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x0006EC98 File Offset: 0x0006DC98
		private void Preprocess(XmlSchema schema, string targetNamespace, ArrayList imports)
		{
			if (schema.IsProcessing)
			{
				return;
			}
			schema.IsProcessing = true;
			string text = schema.TargetNamespace;
			if (text != null)
			{
				text = (schema.TargetNamespace = base.NameTable.Add(text));
				if (text.Length == 0)
				{
					base.SendValidationEvent("Sch_InvalidTargetNamespaceAttribute", schema);
				}
				else
				{
					this.ParseUri(text, "Sch_InvalidNamespace", schema);
				}
			}
			if (schema.Version != null)
			{
				XmlSchemaDatatype datatype = DatatypeImplementation.GetSimpleTypeFromTypeCode(XmlTypeCode.Token).Datatype;
				object obj;
				Exception ex = datatype.TryParseValue(schema.Version, null, null, out obj);
				if (ex != null)
				{
					base.SendValidationEvent("Sch_AttributeValueDataTypeDetailed", new string[] { "version", schema.Version, datatype.TypeCodeString, ex.Message }, ex, schema);
				}
				else
				{
					schema.Version = (string)obj;
				}
			}
			this.Cleanup(schema);
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				XmlSchema xmlSchema = xmlSchemaExternal.Schema;
				this.SetParent(xmlSchemaExternal, schema);
				this.PreprocessAnnotation(xmlSchemaExternal);
				string schemaLocation = xmlSchemaExternal.SchemaLocation;
				if (schemaLocation != null)
				{
					this.ParseUri(schemaLocation, "Sch_InvalidSchemaLocation", xmlSchemaExternal);
				}
				else if ((xmlSchemaExternal.Compositor == Compositor.Include || xmlSchemaExternal.Compositor == Compositor.Redefine) && xmlSchemaExternal.Schema == null)
				{
					base.SendValidationEvent("Sch_MissRequiredAttribute", "schemaLocation", xmlSchemaExternal);
				}
				switch (xmlSchemaExternal.Compositor)
				{
				case Compositor.Include:
				{
					XmlSchema schema2 = xmlSchemaExternal.Schema;
					if (schema2 == null)
					{
						continue;
					}
					break;
				}
				case Compositor.Import:
				{
					XmlSchemaImport xmlSchemaImport = xmlSchemaExternal as XmlSchemaImport;
					string @namespace = xmlSchemaImport.Namespace;
					if (@namespace == schema.TargetNamespace)
					{
						base.SendValidationEvent("Sch_ImportTargetNamespace", xmlSchemaExternal);
					}
					if (xmlSchema != null)
					{
						if (@namespace != xmlSchema.TargetNamespace)
						{
							base.SendValidationEvent("Sch_MismatchTargetNamespaceImport", @namespace, xmlSchema.TargetNamespace, xmlSchemaImport);
						}
						XmlSchema xmlSchema2 = this.rootSchemaForRedefine;
						this.rootSchemaForRedefine = xmlSchema;
						this.Preprocess(xmlSchema, @namespace, imports);
						this.rootSchemaForRedefine = xmlSchema2;
						continue;
					}
					if (@namespace == null)
					{
						continue;
					}
					if (@namespace.Length == 0)
					{
						base.SendValidationEvent("Sch_InvalidNamespaceAttribute", @namespace, xmlSchemaExternal);
						continue;
					}
					this.ParseUri(@namespace, "Sch_InvalidNamespace", xmlSchemaExternal);
					continue;
				}
				case Compositor.Redefine:
					if (xmlSchema == null)
					{
						continue;
					}
					this.CleanupRedefine(xmlSchemaExternal);
					break;
				}
				if (xmlSchema.TargetNamespace != null)
				{
					if (schema.TargetNamespace != xmlSchema.TargetNamespace)
					{
						base.SendValidationEvent("Sch_MismatchTargetNamespaceInclude", xmlSchema.TargetNamespace, schema.TargetNamespace, xmlSchemaExternal);
					}
				}
				else if (targetNamespace != null && targetNamespace.Length != 0)
				{
					xmlSchema = this.GetChameleonSchema(targetNamespace, xmlSchema);
					xmlSchemaExternal.Schema = xmlSchema;
				}
				this.Preprocess(xmlSchema, schema.TargetNamespace, imports);
			}
			this.currentSchema = schema;
			this.BuildRefNamespaces(schema);
			this.ValidateIdAttribute(schema);
			this.targetNamespace = ((targetNamespace == null) ? string.Empty : targetNamespace);
			this.SetSchemaDefaults(schema);
			this.processedExternals.Clear();
			int i = 0;
			while (i < schema.Includes.Count)
			{
				XmlSchemaExternal xmlSchemaExternal2 = (XmlSchemaExternal)schema.Includes[i];
				XmlSchema schema3 = xmlSchemaExternal2.Schema;
				if (schema3 != null)
				{
					switch (xmlSchemaExternal2.Compositor)
					{
					case Compositor.Include:
						if (this.processedExternals[schema3] == null)
						{
							this.processedExternals.Add(schema3, xmlSchemaExternal2);
							this.CopyIncludedComponents(schema3, schema);
							goto IL_04CF;
						}
						break;
					case Compositor.Import:
					{
						if (schema3 == this.rootSchema)
						{
							goto IL_04CF;
						}
						XmlSchemaImport xmlSchemaImport2 = xmlSchemaExternal2 as XmlSchemaImport;
						string text2 = ((xmlSchemaImport2.Namespace != null) ? xmlSchemaImport2.Namespace : string.Empty);
						if (!imports.Contains(schema3))
						{
							imports.Add(schema3);
						}
						if (!this.rootSchema.ImportedNamespaces.Contains(text2))
						{
							this.rootSchema.ImportedNamespaces.Add(text2);
							goto IL_04CF;
						}
						goto IL_04CF;
					}
					case Compositor.Redefine:
						if (this.redefinedList == null)
						{
							this.redefinedList = new ArrayList();
						}
						this.redefinedList.Add(new RedefineEntry(xmlSchemaExternal2 as XmlSchemaRedefine, this.rootSchemaForRedefine));
						if (this.processedExternals[schema3] == null)
						{
							this.processedExternals.Add(schema3, xmlSchemaExternal2);
							this.CopyIncludedComponents(schema3, schema);
							goto IL_04CF;
						}
						break;
					default:
						goto IL_04CF;
					}
				}
				else
				{
					if (xmlSchemaExternal2.Compositor != Compositor.Redefine)
					{
						goto IL_04CF;
					}
					XmlSchemaRedefine xmlSchemaRedefine = xmlSchemaExternal2 as XmlSchemaRedefine;
					if (xmlSchemaRedefine.BaseUri == null)
					{
						foreach (XmlSchemaObject xmlSchemaObject2 in xmlSchemaRedefine.Items)
						{
							if (!(xmlSchemaObject2 is XmlSchemaAnnotation))
							{
								base.SendValidationEvent("Sch_RedefineNoSchema", xmlSchemaRedefine);
								break;
							}
						}
						goto IL_04CF;
					}
					goto IL_04CF;
				}
				IL_04D7:
				i++;
				continue;
				IL_04CF:
				this.ValidateIdAttribute(xmlSchemaExternal2);
				goto IL_04D7;
			}
			ArrayList arrayList = new ArrayList();
			foreach (XmlSchemaObject xmlSchemaObject3 in schema.Items)
			{
				this.SetParent(xmlSchemaObject3, schema);
				if (xmlSchemaObject3 is XmlSchemaAttribute)
				{
					XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)xmlSchemaObject3;
					this.PreprocessAttribute(xmlSchemaAttribute);
					base.AddToTable(schema.Attributes, xmlSchemaAttribute.QualifiedName, xmlSchemaAttribute);
				}
				else if (xmlSchemaObject3 is XmlSchemaAttributeGroup)
				{
					XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)xmlSchemaObject3;
					this.PreprocessAttributeGroup(xmlSchemaAttributeGroup);
					base.AddToTable(schema.AttributeGroups, xmlSchemaAttributeGroup.QualifiedName, xmlSchemaAttributeGroup);
				}
				else if (xmlSchemaObject3 is XmlSchemaComplexType)
				{
					XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)xmlSchemaObject3;
					this.PreprocessComplexType(xmlSchemaComplexType, false);
					base.AddToTable(schema.SchemaTypes, xmlSchemaComplexType.QualifiedName, xmlSchemaComplexType);
				}
				else if (xmlSchemaObject3 is XmlSchemaSimpleType)
				{
					XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)xmlSchemaObject3;
					this.PreprocessSimpleType(xmlSchemaSimpleType, false);
					base.AddToTable(schema.SchemaTypes, xmlSchemaSimpleType.QualifiedName, xmlSchemaSimpleType);
				}
				else if (xmlSchemaObject3 is XmlSchemaElement)
				{
					XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)xmlSchemaObject3;
					this.PreprocessElement(xmlSchemaElement);
					base.AddToTable(schema.Elements, xmlSchemaElement.QualifiedName, xmlSchemaElement);
				}
				else if (xmlSchemaObject3 is XmlSchemaGroup)
				{
					XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup)xmlSchemaObject3;
					this.PreprocessGroup(xmlSchemaGroup);
					base.AddToTable(schema.Groups, xmlSchemaGroup.QualifiedName, xmlSchemaGroup);
				}
				else if (xmlSchemaObject3 is XmlSchemaNotation)
				{
					XmlSchemaNotation xmlSchemaNotation = (XmlSchemaNotation)xmlSchemaObject3;
					this.PreprocessNotation(xmlSchemaNotation);
					base.AddToTable(schema.Notations, xmlSchemaNotation.QualifiedName, xmlSchemaNotation);
				}
				else if (xmlSchemaObject3 is XmlSchemaAnnotation)
				{
					this.PreprocessAnnotation(xmlSchemaObject3 as XmlSchemaAnnotation);
				}
				else
				{
					base.SendValidationEvent("Sch_InvalidCollection", xmlSchemaObject3);
					arrayList.Add(xmlSchemaObject3);
				}
			}
			foreach (object obj2 in arrayList)
			{
				XmlSchemaObject xmlSchemaObject4 = (XmlSchemaObject)obj2;
				schema.Items.Remove(xmlSchemaObject4);
			}
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x0006F424 File Offset: 0x0006E424
		private void CopyIncludedComponents(XmlSchema includedSchema, XmlSchema schema)
		{
			foreach (object obj in includedSchema.Elements.Values)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj;
				base.AddToTable(schema.Elements, xmlSchemaElement.QualifiedName, xmlSchemaElement);
			}
			foreach (object obj2 in includedSchema.Attributes.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj2;
				base.AddToTable(schema.Attributes, xmlSchemaAttribute.QualifiedName, xmlSchemaAttribute);
			}
			foreach (object obj3 in includedSchema.Groups.Values)
			{
				XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup)obj3;
				base.AddToTable(schema.Groups, xmlSchemaGroup.QualifiedName, xmlSchemaGroup);
			}
			foreach (object obj4 in includedSchema.AttributeGroups.Values)
			{
				XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)obj4;
				base.AddToTable(schema.AttributeGroups, xmlSchemaAttributeGroup.QualifiedName, xmlSchemaAttributeGroup);
			}
			foreach (object obj5 in includedSchema.SchemaTypes.Values)
			{
				XmlSchemaType xmlSchemaType = (XmlSchemaType)obj5;
				base.AddToTable(schema.SchemaTypes, xmlSchemaType.QualifiedName, xmlSchemaType);
			}
			foreach (object obj6 in includedSchema.Notations.Values)
			{
				XmlSchemaNotation xmlSchemaNotation = (XmlSchemaNotation)obj6;
				base.AddToTable(schema.Notations, xmlSchemaNotation.QualifiedName, xmlSchemaNotation);
			}
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x0006F67C File Offset: 0x0006E67C
		private void PreprocessRedefine(RedefineEntry redefineEntry)
		{
			XmlSchemaRedefine redefine = redefineEntry.redefine;
			XmlSchema schema = redefine.Schema;
			this.currentSchema = Preprocessor.GetParentSchema(redefine);
			this.SetSchemaDefaults(this.currentSchema);
			if (schema.IsRedefined)
			{
				base.SendValidationEvent("Sch_MultipleRedefine", redefine, XmlSeverityType.Warning);
				return;
			}
			schema.IsRedefined = true;
			XmlSchema schemaToUpdate = redefineEntry.schemaToUpdate;
			ArrayList arrayList = new ArrayList();
			this.GetIncludedSet(schema, arrayList);
			string text = ((schemaToUpdate.TargetNamespace == null) ? string.Empty : schemaToUpdate.TargetNamespace);
			foreach (XmlSchemaObject xmlSchemaObject in redefine.Items)
			{
				this.SetParent(xmlSchemaObject, redefine);
				if (xmlSchemaObject is XmlSchemaGroup)
				{
					XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup)xmlSchemaObject;
					this.PreprocessGroup(xmlSchemaGroup);
					xmlSchemaGroup.QualifiedName.SetNamespace(text);
					if (redefine.Groups[xmlSchemaGroup.QualifiedName] != null)
					{
						base.SendValidationEvent("Sch_GroupDoubleRedefine", xmlSchemaGroup);
					}
					else
					{
						base.AddToTable(redefine.Groups, xmlSchemaGroup.QualifiedName, xmlSchemaGroup);
						XmlSchemaGroup xmlSchemaGroup2 = (XmlSchemaGroup)schemaToUpdate.Groups[xmlSchemaGroup.QualifiedName];
						XmlSchema parentSchema = Preprocessor.GetParentSchema(xmlSchemaGroup2);
						if (xmlSchemaGroup2 == null || (parentSchema != schema && !arrayList.Contains(parentSchema)))
						{
							base.SendValidationEvent("Sch_ComponentRedefineNotFound", "<group>", xmlSchemaGroup.QualifiedName.ToString(), xmlSchemaGroup);
						}
						else
						{
							xmlSchemaGroup.Redefined = xmlSchemaGroup2;
							schemaToUpdate.Groups.Insert(xmlSchemaGroup.QualifiedName, xmlSchemaGroup);
							this.CheckRefinedGroup(xmlSchemaGroup);
						}
					}
				}
				else if (xmlSchemaObject is XmlSchemaAttributeGroup)
				{
					XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)xmlSchemaObject;
					this.PreprocessAttributeGroup(xmlSchemaAttributeGroup);
					xmlSchemaAttributeGroup.QualifiedName.SetNamespace(text);
					if (redefine.AttributeGroups[xmlSchemaAttributeGroup.QualifiedName] != null)
					{
						base.SendValidationEvent("Sch_AttrGroupDoubleRedefine", xmlSchemaAttributeGroup);
					}
					else
					{
						base.AddToTable(redefine.AttributeGroups, xmlSchemaAttributeGroup.QualifiedName, xmlSchemaAttributeGroup);
						XmlSchemaAttributeGroup xmlSchemaAttributeGroup2 = (XmlSchemaAttributeGroup)schemaToUpdate.AttributeGroups[xmlSchemaAttributeGroup.QualifiedName];
						XmlSchema parentSchema2 = Preprocessor.GetParentSchema(xmlSchemaAttributeGroup2);
						if (xmlSchemaAttributeGroup2 == null || (parentSchema2 != schema && !arrayList.Contains(parentSchema2)))
						{
							base.SendValidationEvent("Sch_ComponentRedefineNotFound", "<attributeGroup>", xmlSchemaAttributeGroup.QualifiedName.ToString(), xmlSchemaAttributeGroup);
						}
						else
						{
							xmlSchemaAttributeGroup.Redefined = xmlSchemaAttributeGroup2;
							schemaToUpdate.AttributeGroups.Insert(xmlSchemaAttributeGroup.QualifiedName, xmlSchemaAttributeGroup);
							this.CheckRefinedAttributeGroup(xmlSchemaAttributeGroup);
						}
					}
				}
				else if (xmlSchemaObject is XmlSchemaComplexType)
				{
					XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)xmlSchemaObject;
					this.PreprocessComplexType(xmlSchemaComplexType, false);
					xmlSchemaComplexType.QualifiedName.SetNamespace(text);
					if (redefine.SchemaTypes[xmlSchemaComplexType.QualifiedName] != null)
					{
						base.SendValidationEvent("Sch_ComplexTypeDoubleRedefine", xmlSchemaComplexType);
					}
					else
					{
						base.AddToTable(redefine.SchemaTypes, xmlSchemaComplexType.QualifiedName, xmlSchemaComplexType);
						XmlSchemaType xmlSchemaType = (XmlSchemaType)schemaToUpdate.SchemaTypes[xmlSchemaComplexType.QualifiedName];
						XmlSchema parentSchema3 = Preprocessor.GetParentSchema(xmlSchemaType);
						if (xmlSchemaType == null || (parentSchema3 != schema && !arrayList.Contains(parentSchema3)))
						{
							base.SendValidationEvent("Sch_ComponentRedefineNotFound", "<complexType>", xmlSchemaComplexType.QualifiedName.ToString(), xmlSchemaComplexType);
						}
						else if (xmlSchemaType is XmlSchemaComplexType)
						{
							xmlSchemaComplexType.Redefined = xmlSchemaType;
							schemaToUpdate.SchemaTypes.Insert(xmlSchemaComplexType.QualifiedName, xmlSchemaComplexType);
							this.CheckRefinedComplexType(xmlSchemaComplexType);
						}
						else
						{
							base.SendValidationEvent("Sch_SimpleToComplexTypeRedefine", xmlSchemaComplexType);
						}
					}
				}
				else if (xmlSchemaObject is XmlSchemaSimpleType)
				{
					XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)xmlSchemaObject;
					this.PreprocessSimpleType(xmlSchemaSimpleType, false);
					xmlSchemaSimpleType.QualifiedName.SetNamespace(text);
					if (redefine.SchemaTypes[xmlSchemaSimpleType.QualifiedName] != null)
					{
						base.SendValidationEvent("Sch_SimpleTypeDoubleRedefine", xmlSchemaSimpleType);
					}
					else
					{
						base.AddToTable(redefine.SchemaTypes, xmlSchemaSimpleType.QualifiedName, xmlSchemaSimpleType);
						XmlSchemaType xmlSchemaType2 = (XmlSchemaType)schemaToUpdate.SchemaTypes[xmlSchemaSimpleType.QualifiedName];
						XmlSchema parentSchema4 = Preprocessor.GetParentSchema(xmlSchemaType2);
						if (xmlSchemaType2 == null || (parentSchema4 != schema && !arrayList.Contains(parentSchema4)))
						{
							base.SendValidationEvent("Sch_ComponentRedefineNotFound", "<simpleType>", xmlSchemaSimpleType.QualifiedName.ToString(), xmlSchemaSimpleType);
						}
						else if (xmlSchemaType2 is XmlSchemaSimpleType)
						{
							xmlSchemaSimpleType.Redefined = xmlSchemaType2;
							schemaToUpdate.SchemaTypes.Insert(xmlSchemaSimpleType.QualifiedName, xmlSchemaSimpleType);
							this.CheckRefinedSimpleType(xmlSchemaSimpleType);
						}
						else
						{
							base.SendValidationEvent("Sch_ComplexToSimpleTypeRedefine", xmlSchemaSimpleType);
						}
					}
				}
			}
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x0006FB28 File Offset: 0x0006EB28
		private void GetIncludedSet(XmlSchema schema, ArrayList includesList)
		{
			if (includesList.Contains(schema))
			{
				return;
			}
			includesList.Add(schema);
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				if ((xmlSchemaExternal.Compositor == Compositor.Include || xmlSchemaExternal.Compositor == Compositor.Redefine) && xmlSchemaExternal.Schema != null)
				{
					this.GetIncludedSet(xmlSchemaExternal.Schema, includesList);
				}
			}
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x0006FBB4 File Offset: 0x0006EBB4
		internal static XmlSchema GetParentSchema(XmlSchemaObject currentSchemaObject)
		{
			XmlSchema xmlSchema = null;
			while (xmlSchema == null && currentSchemaObject != null)
			{
				currentSchemaObject = currentSchemaObject.Parent;
				xmlSchema = currentSchemaObject as XmlSchema;
			}
			return xmlSchema;
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x0006FBDC File Offset: 0x0006EBDC
		private void SetSchemaDefaults(XmlSchema schema)
		{
			if (schema.BlockDefault == XmlSchemaDerivationMethod.All)
			{
				this.blockDefault = XmlSchemaDerivationMethod.All;
			}
			else if (schema.BlockDefault == XmlSchemaDerivationMethod.None)
			{
				this.blockDefault = XmlSchemaDerivationMethod.Empty;
			}
			else
			{
				if ((schema.BlockDefault & ~(XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction)) != XmlSchemaDerivationMethod.Empty)
				{
					base.SendValidationEvent("Sch_InvalidBlockDefaultValue", schema);
				}
				this.blockDefault = schema.BlockDefault & (XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction);
			}
			if (schema.FinalDefault == XmlSchemaDerivationMethod.All)
			{
				this.finalDefault = XmlSchemaDerivationMethod.All;
			}
			else if (schema.FinalDefault == XmlSchemaDerivationMethod.None)
			{
				this.finalDefault = XmlSchemaDerivationMethod.Empty;
			}
			else
			{
				if ((schema.FinalDefault & ~(XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union)) != XmlSchemaDerivationMethod.Empty)
				{
					base.SendValidationEvent("Sch_InvalidFinalDefaultValue", schema);
				}
				this.finalDefault = schema.FinalDefault & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union);
			}
			this.elementFormDefault = schema.ElementFormDefault;
			if (this.elementFormDefault == XmlSchemaForm.None)
			{
				this.elementFormDefault = XmlSchemaForm.Unqualified;
			}
			this.attributeFormDefault = schema.AttributeFormDefault;
			if (this.attributeFormDefault == XmlSchemaForm.None)
			{
				this.attributeFormDefault = XmlSchemaForm.Unqualified;
			}
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x0006FCCC File Offset: 0x0006ECCC
		private int CountGroupSelfReference(XmlSchemaObjectCollection items, XmlQualifiedName name, XmlSchemaGroup redefined)
		{
			int num = 0;
			foreach (XmlSchemaObject xmlSchemaObject in items)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				if (xmlSchemaParticle is XmlSchemaGroupRef)
				{
					XmlSchemaGroupRef xmlSchemaGroupRef = (XmlSchemaGroupRef)xmlSchemaParticle;
					if (xmlSchemaGroupRef.RefName == name)
					{
						xmlSchemaGroupRef.Redefined = redefined;
						if (xmlSchemaGroupRef.MinOccurs != 1m || xmlSchemaGroupRef.MaxOccurs != 1m)
						{
							base.SendValidationEvent("Sch_MinMaxGroupRedefine", xmlSchemaGroupRef);
						}
						num++;
					}
				}
				else if (xmlSchemaParticle is XmlSchemaGroupBase)
				{
					num += this.CountGroupSelfReference(((XmlSchemaGroupBase)xmlSchemaParticle).Items, name, redefined);
				}
				if (num > 1)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x0006FDA4 File Offset: 0x0006EDA4
		private void CheckRefinedGroup(XmlSchemaGroup group)
		{
			int num = 0;
			if (group.Particle != null)
			{
				num = this.CountGroupSelfReference(group.Particle.Items, group.QualifiedName, group.Redefined);
			}
			if (num > 1)
			{
				base.SendValidationEvent("Sch_MultipleGroupSelfRef", group);
			}
			group.SelfReferenceCount = num;
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x0006FDF0 File Offset: 0x0006EDF0
		private void CheckRefinedAttributeGroup(XmlSchemaAttributeGroup attributeGroup)
		{
			int num = 0;
			foreach (object obj in attributeGroup.Attributes)
			{
				if (obj is XmlSchemaAttributeGroupRef && ((XmlSchemaAttributeGroupRef)obj).RefName == attributeGroup.QualifiedName)
				{
					num++;
				}
			}
			if (num > 1)
			{
				base.SendValidationEvent("Sch_MultipleAttrGroupSelfRef", attributeGroup);
			}
			attributeGroup.SelfReferenceCount = num;
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x0006FE7C File Offset: 0x0006EE7C
		private void CheckRefinedSimpleType(XmlSchemaSimpleType stype)
		{
			if (stype.Content != null && stype.Content is XmlSchemaSimpleTypeRestriction)
			{
				XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = (XmlSchemaSimpleTypeRestriction)stype.Content;
				if (xmlSchemaSimpleTypeRestriction.BaseTypeName == stype.QualifiedName)
				{
					return;
				}
			}
			base.SendValidationEvent("Sch_InvalidTypeRedefine", stype);
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x0006FECC File Offset: 0x0006EECC
		private void CheckRefinedComplexType(XmlSchemaComplexType ctype)
		{
			if (ctype.ContentModel != null)
			{
				XmlQualifiedName xmlQualifiedName;
				if (ctype.ContentModel is XmlSchemaComplexContent)
				{
					XmlSchemaComplexContent xmlSchemaComplexContent = (XmlSchemaComplexContent)ctype.ContentModel;
					if (xmlSchemaComplexContent.Content is XmlSchemaComplexContentRestriction)
					{
						xmlQualifiedName = ((XmlSchemaComplexContentRestriction)xmlSchemaComplexContent.Content).BaseTypeName;
					}
					else
					{
						xmlQualifiedName = ((XmlSchemaComplexContentExtension)xmlSchemaComplexContent.Content).BaseTypeName;
					}
				}
				else
				{
					XmlSchemaSimpleContent xmlSchemaSimpleContent = (XmlSchemaSimpleContent)ctype.ContentModel;
					if (xmlSchemaSimpleContent.Content is XmlSchemaSimpleContentRestriction)
					{
						xmlQualifiedName = ((XmlSchemaSimpleContentRestriction)xmlSchemaSimpleContent.Content).BaseTypeName;
					}
					else
					{
						xmlQualifiedName = ((XmlSchemaSimpleContentExtension)xmlSchemaSimpleContent.Content).BaseTypeName;
					}
				}
				if (xmlQualifiedName == ctype.QualifiedName)
				{
					return;
				}
			}
			base.SendValidationEvent("Sch_InvalidTypeRedefine", ctype);
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x0006FF88 File Offset: 0x0006EF88
		private void PreprocessAttribute(XmlSchemaAttribute attribute)
		{
			if (attribute.Name != null)
			{
				this.ValidateNameAttribute(attribute);
				attribute.SetQualifiedName(new XmlQualifiedName(attribute.Name, this.targetNamespace));
			}
			else
			{
				base.SendValidationEvent("Sch_MissRequiredAttribute", "name", attribute);
			}
			if (attribute.Use != XmlSchemaUse.None)
			{
				base.SendValidationEvent("Sch_ForbiddenAttribute", "use", attribute);
			}
			if (attribute.Form != XmlSchemaForm.None)
			{
				base.SendValidationEvent("Sch_ForbiddenAttribute", "form", attribute);
			}
			this.PreprocessAttributeContent(attribute);
			this.ValidateIdAttribute(attribute);
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x00070010 File Offset: 0x0006F010
		private void PreprocessLocalAttribute(XmlSchemaAttribute attribute)
		{
			if (attribute.Name != null)
			{
				this.ValidateNameAttribute(attribute);
				this.PreprocessAttributeContent(attribute);
				attribute.SetQualifiedName(new XmlQualifiedName(attribute.Name, (attribute.Form == XmlSchemaForm.Qualified || (attribute.Form == XmlSchemaForm.None && this.attributeFormDefault == XmlSchemaForm.Qualified)) ? this.targetNamespace : null));
			}
			else
			{
				this.PreprocessAnnotation(attribute);
				if (attribute.RefName.IsEmpty)
				{
					base.SendValidationEvent("Sch_AttributeNameRef", "???", attribute);
				}
				else
				{
					this.ValidateQNameAttribute(attribute, "ref", attribute.RefName);
				}
				if (!attribute.SchemaTypeName.IsEmpty || attribute.SchemaType != null || attribute.Form != XmlSchemaForm.None)
				{
					base.SendValidationEvent("Sch_InvalidAttributeRef", attribute);
				}
				attribute.SetQualifiedName(attribute.RefName);
			}
			this.ValidateIdAttribute(attribute);
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x000700E0 File Offset: 0x0006F0E0
		private void PreprocessAttributeContent(XmlSchemaAttribute attribute)
		{
			this.PreprocessAnnotation(attribute);
			if (Ref.Equal(this.currentSchema.TargetNamespace, this.NsXsi))
			{
				base.SendValidationEvent("Sch_TargetNamespaceXsi", attribute);
			}
			if (!attribute.RefName.IsEmpty)
			{
				base.SendValidationEvent("Sch_ForbiddenAttribute", "ref", attribute);
			}
			if (attribute.DefaultValue != null && attribute.FixedValue != null)
			{
				base.SendValidationEvent("Sch_DefaultFixedAttributes", attribute);
			}
			if (attribute.DefaultValue != null && attribute.Use != XmlSchemaUse.Optional && attribute.Use != XmlSchemaUse.None)
			{
				base.SendValidationEvent("Sch_OptionalDefaultAttribute", attribute);
			}
			if (attribute.Name == this.Xmlns)
			{
				base.SendValidationEvent("Sch_XmlNsAttribute", attribute);
			}
			if (attribute.SchemaType != null)
			{
				this.SetParent(attribute.SchemaType, attribute);
				if (!attribute.SchemaTypeName.IsEmpty)
				{
					base.SendValidationEvent("Sch_TypeMutualExclusive", attribute);
				}
				this.PreprocessSimpleType(attribute.SchemaType, true);
			}
			if (!attribute.SchemaTypeName.IsEmpty)
			{
				this.ValidateQNameAttribute(attribute, "type", attribute.SchemaTypeName);
			}
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x000701F0 File Offset: 0x0006F1F0
		private void PreprocessAttributeGroup(XmlSchemaAttributeGroup attributeGroup)
		{
			if (attributeGroup.Name != null)
			{
				this.ValidateNameAttribute(attributeGroup);
				attributeGroup.SetQualifiedName(new XmlQualifiedName(attributeGroup.Name, this.targetNamespace));
			}
			else
			{
				base.SendValidationEvent("Sch_MissRequiredAttribute", "name", attributeGroup);
			}
			this.PreprocessAttributes(attributeGroup.Attributes, attributeGroup.AnyAttribute, attributeGroup);
			this.PreprocessAnnotation(attributeGroup);
			this.ValidateIdAttribute(attributeGroup);
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x00070258 File Offset: 0x0006F258
		private void PreprocessElement(XmlSchemaElement element)
		{
			if (element.Name != null)
			{
				this.ValidateNameAttribute(element);
				element.SetQualifiedName(new XmlQualifiedName(element.Name, this.targetNamespace));
			}
			else
			{
				base.SendValidationEvent("Sch_MissRequiredAttribute", "name", element);
			}
			this.PreprocessElementContent(element);
			if (element.Final == XmlSchemaDerivationMethod.All)
			{
				element.SetFinalResolved(XmlSchemaDerivationMethod.All);
			}
			else if (element.Final == XmlSchemaDerivationMethod.None)
			{
				if (this.finalDefault == XmlSchemaDerivationMethod.All)
				{
					element.SetFinalResolved(XmlSchemaDerivationMethod.All);
				}
				else
				{
					element.SetFinalResolved(this.finalDefault & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
				}
			}
			else
			{
				if ((element.Final & ~(XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction)) != XmlSchemaDerivationMethod.Empty)
				{
					base.SendValidationEvent("Sch_InvalidElementFinalValue", element);
				}
				element.SetFinalResolved(element.Final & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
			}
			if (element.Form != XmlSchemaForm.None)
			{
				base.SendValidationEvent("Sch_ForbiddenAttribute", "form", element);
			}
			if (element.MinOccursString != null)
			{
				base.SendValidationEvent("Sch_ForbiddenAttribute", "minOccurs", element);
			}
			if (element.MaxOccursString != null)
			{
				base.SendValidationEvent("Sch_ForbiddenAttribute", "maxOccurs", element);
			}
			if (!element.SubstitutionGroup.IsEmpty)
			{
				this.ValidateQNameAttribute(element, "type", element.SubstitutionGroup);
			}
			this.ValidateIdAttribute(element);
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0007038C File Offset: 0x0006F38C
		private void PreprocessLocalElement(XmlSchemaElement element)
		{
			if (element.Name != null)
			{
				this.ValidateNameAttribute(element);
				this.PreprocessElementContent(element);
				element.SetQualifiedName(new XmlQualifiedName(element.Name, (element.Form == XmlSchemaForm.Qualified || (element.Form == XmlSchemaForm.None && this.elementFormDefault == XmlSchemaForm.Qualified)) ? this.targetNamespace : null));
			}
			else
			{
				this.PreprocessAnnotation(element);
				if (element.RefName.IsEmpty)
				{
					base.SendValidationEvent("Sch_ElementNameRef", element);
				}
				else
				{
					this.ValidateQNameAttribute(element, "ref", element.RefName);
				}
				if (!element.SchemaTypeName.IsEmpty || element.HasAbstractAttribute || element.Block != XmlSchemaDerivationMethod.None || element.SchemaType != null || element.HasConstraints || element.DefaultValue != null || element.Form != XmlSchemaForm.None || element.FixedValue != null || element.HasNillableAttribute)
				{
					base.SendValidationEvent("Sch_InvalidElementRef", element);
				}
				if (element.DefaultValue != null && element.FixedValue != null)
				{
					base.SendValidationEvent("Sch_DefaultFixedAttributes", element);
				}
				element.SetQualifiedName(element.RefName);
			}
			if (element.MinOccurs > element.MaxOccurs)
			{
				element.MinOccurs = 0m;
				base.SendValidationEvent("Sch_MinGtMax", element);
			}
			if (element.HasAbstractAttribute)
			{
				base.SendValidationEvent("Sch_ForbiddenAttribute", "abstract", element);
			}
			if (element.Final != XmlSchemaDerivationMethod.None)
			{
				base.SendValidationEvent("Sch_ForbiddenAttribute", "final", element);
			}
			if (!element.SubstitutionGroup.IsEmpty)
			{
				base.SendValidationEvent("Sch_ForbiddenAttribute", "substitutionGroup", element);
			}
			this.ValidateIdAttribute(element);
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x0007052C File Offset: 0x0006F52C
		private void PreprocessElementContent(XmlSchemaElement element)
		{
			this.PreprocessAnnotation(element);
			if (!element.RefName.IsEmpty)
			{
				base.SendValidationEvent("Sch_ForbiddenAttribute", "ref", element);
			}
			if (element.Block == XmlSchemaDerivationMethod.All)
			{
				element.SetBlockResolved(XmlSchemaDerivationMethod.All);
			}
			else if (element.Block == XmlSchemaDerivationMethod.None)
			{
				if (this.blockDefault == XmlSchemaDerivationMethod.All)
				{
					element.SetBlockResolved(XmlSchemaDerivationMethod.All);
				}
				else
				{
					element.SetBlockResolved(this.blockDefault & (XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
				}
			}
			else
			{
				if ((element.Block & ~(XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction)) != XmlSchemaDerivationMethod.Empty)
				{
					base.SendValidationEvent("Sch_InvalidElementBlockValue", element);
				}
				element.SetBlockResolved(element.Block & (XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
			}
			if (element.SchemaType != null)
			{
				this.SetParent(element.SchemaType, element);
				if (!element.SchemaTypeName.IsEmpty)
				{
					base.SendValidationEvent("Sch_TypeMutualExclusive", element);
				}
				if (element.SchemaType is XmlSchemaComplexType)
				{
					this.PreprocessComplexType((XmlSchemaComplexType)element.SchemaType, true);
				}
				else
				{
					this.PreprocessSimpleType((XmlSchemaSimpleType)element.SchemaType, true);
				}
			}
			if (!element.SchemaTypeName.IsEmpty)
			{
				this.ValidateQNameAttribute(element, "type", element.SchemaTypeName);
			}
			if (element.DefaultValue != null && element.FixedValue != null)
			{
				base.SendValidationEvent("Sch_DefaultFixedAttributes", element);
			}
			foreach (XmlSchemaObject xmlSchemaObject in element.Constraints)
			{
				XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint = (XmlSchemaIdentityConstraint)xmlSchemaObject;
				this.SetParent(xmlSchemaIdentityConstraint, element);
				this.PreprocessIdentityConstraint(xmlSchemaIdentityConstraint);
			}
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x000706C4 File Offset: 0x0006F6C4
		private void PreprocessIdentityConstraint(XmlSchemaIdentityConstraint constraint)
		{
			bool flag = true;
			this.PreprocessAnnotation(constraint);
			if (constraint.Name != null)
			{
				this.ValidateNameAttribute(constraint);
				constraint.SetQualifiedName(new XmlQualifiedName(constraint.Name, this.targetNamespace));
			}
			else
			{
				base.SendValidationEvent("Sch_MissRequiredAttribute", "name", constraint);
				flag = false;
			}
			if (this.rootSchema.IdentityConstraints[constraint.QualifiedName] != null)
			{
				base.SendValidationEvent("Sch_DupIdentityConstraint", constraint.QualifiedName.ToString(), constraint);
				flag = false;
			}
			else
			{
				this.rootSchema.IdentityConstraints.Add(constraint.QualifiedName, constraint);
			}
			if (constraint.Selector == null)
			{
				base.SendValidationEvent("Sch_IdConstraintNoSelector", constraint);
				flag = false;
			}
			if (constraint.Fields.Count == 0)
			{
				base.SendValidationEvent("Sch_IdConstraintNoFields", constraint);
				flag = false;
			}
			if (constraint is XmlSchemaKeyref)
			{
				XmlSchemaKeyref xmlSchemaKeyref = (XmlSchemaKeyref)constraint;
				if (xmlSchemaKeyref.Refer.IsEmpty)
				{
					base.SendValidationEvent("Sch_IdConstraintNoRefer", constraint);
					flag = false;
				}
				else
				{
					this.ValidateQNameAttribute(xmlSchemaKeyref, "refer", xmlSchemaKeyref.Refer);
				}
			}
			if (flag)
			{
				this.ValidateIdAttribute(constraint);
				this.ValidateIdAttribute(constraint.Selector);
				this.SetParent(constraint.Selector, constraint);
				foreach (XmlSchemaObject xmlSchemaObject in constraint.Fields)
				{
					XmlSchemaXPath xmlSchemaXPath = (XmlSchemaXPath)xmlSchemaObject;
					this.SetParent(xmlSchemaXPath, constraint);
					this.ValidateIdAttribute(xmlSchemaXPath);
				}
			}
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x00070848 File Offset: 0x0006F848
		private void PreprocessSimpleType(XmlSchemaSimpleType simpleType, bool local)
		{
			if (local)
			{
				if (simpleType.Name != null)
				{
					base.SendValidationEvent("Sch_ForbiddenAttribute", "name", simpleType);
				}
			}
			else
			{
				if (simpleType.Name != null)
				{
					this.ValidateNameAttribute(simpleType);
					simpleType.SetQualifiedName(new XmlQualifiedName(simpleType.Name, this.targetNamespace));
				}
				else
				{
					base.SendValidationEvent("Sch_MissRequiredAttribute", "name", simpleType);
				}
				if (simpleType.Final == XmlSchemaDerivationMethod.All)
				{
					simpleType.SetFinalResolved(XmlSchemaDerivationMethod.All);
				}
				else if (simpleType.Final == XmlSchemaDerivationMethod.None)
				{
					if (this.finalDefault == XmlSchemaDerivationMethod.All)
					{
						simpleType.SetFinalResolved(XmlSchemaDerivationMethod.All);
					}
					else
					{
						simpleType.SetFinalResolved(this.finalDefault & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union));
					}
				}
				else
				{
					if ((simpleType.Final & ~(XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union)) != XmlSchemaDerivationMethod.Empty)
					{
						base.SendValidationEvent("Sch_InvalidSimpleTypeFinalValue", simpleType);
					}
					simpleType.SetFinalResolved(simpleType.Final & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union));
				}
			}
			if (simpleType.Content == null)
			{
				base.SendValidationEvent("Sch_NoSimpleTypeContent", simpleType);
			}
			else if (simpleType.Content is XmlSchemaSimpleTypeRestriction)
			{
				XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = (XmlSchemaSimpleTypeRestriction)simpleType.Content;
				this.SetParent(xmlSchemaSimpleTypeRestriction, simpleType);
				foreach (XmlSchemaObject xmlSchemaObject in xmlSchemaSimpleTypeRestriction.Facets)
				{
					this.SetParent(xmlSchemaObject, xmlSchemaSimpleTypeRestriction);
				}
				if (xmlSchemaSimpleTypeRestriction.BaseType != null)
				{
					if (!xmlSchemaSimpleTypeRestriction.BaseTypeName.IsEmpty)
					{
						base.SendValidationEvent("Sch_SimpleTypeRestRefBase", xmlSchemaSimpleTypeRestriction);
					}
					this.PreprocessSimpleType(xmlSchemaSimpleTypeRestriction.BaseType, true);
				}
				else if (xmlSchemaSimpleTypeRestriction.BaseTypeName.IsEmpty)
				{
					base.SendValidationEvent("Sch_SimpleTypeRestRefBaseNone", xmlSchemaSimpleTypeRestriction);
				}
				else
				{
					this.ValidateQNameAttribute(xmlSchemaSimpleTypeRestriction, "base", xmlSchemaSimpleTypeRestriction.BaseTypeName);
				}
				this.PreprocessAnnotation(xmlSchemaSimpleTypeRestriction);
				this.ValidateIdAttribute(xmlSchemaSimpleTypeRestriction);
			}
			else if (simpleType.Content is XmlSchemaSimpleTypeList)
			{
				XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = (XmlSchemaSimpleTypeList)simpleType.Content;
				this.SetParent(xmlSchemaSimpleTypeList, simpleType);
				if (xmlSchemaSimpleTypeList.ItemType != null)
				{
					if (!xmlSchemaSimpleTypeList.ItemTypeName.IsEmpty)
					{
						base.SendValidationEvent("Sch_SimpleTypeListRefBase", xmlSchemaSimpleTypeList);
					}
					this.SetParent(xmlSchemaSimpleTypeList.ItemType, xmlSchemaSimpleTypeList);
					this.PreprocessSimpleType(xmlSchemaSimpleTypeList.ItemType, true);
				}
				else if (xmlSchemaSimpleTypeList.ItemTypeName.IsEmpty)
				{
					base.SendValidationEvent("Sch_SimpleTypeListRefBaseNone", xmlSchemaSimpleTypeList);
				}
				else
				{
					this.ValidateQNameAttribute(xmlSchemaSimpleTypeList, "itemType", xmlSchemaSimpleTypeList.ItemTypeName);
				}
				this.PreprocessAnnotation(xmlSchemaSimpleTypeList);
				this.ValidateIdAttribute(xmlSchemaSimpleTypeList);
			}
			else
			{
				XmlSchemaSimpleTypeUnion xmlSchemaSimpleTypeUnion = (XmlSchemaSimpleTypeUnion)simpleType.Content;
				this.SetParent(xmlSchemaSimpleTypeUnion, simpleType);
				int num = xmlSchemaSimpleTypeUnion.BaseTypes.Count;
				if (xmlSchemaSimpleTypeUnion.MemberTypes != null)
				{
					num += xmlSchemaSimpleTypeUnion.MemberTypes.Length;
					foreach (XmlQualifiedName xmlQualifiedName in xmlSchemaSimpleTypeUnion.MemberTypes)
					{
						this.ValidateQNameAttribute(xmlSchemaSimpleTypeUnion, "memberTypes", xmlQualifiedName);
					}
				}
				if (num == 0)
				{
					base.SendValidationEvent("Sch_SimpleTypeUnionNoBase", xmlSchemaSimpleTypeUnion);
				}
				foreach (XmlSchemaObject xmlSchemaObject2 in xmlSchemaSimpleTypeUnion.BaseTypes)
				{
					XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)xmlSchemaObject2;
					this.SetParent(xmlSchemaSimpleType, xmlSchemaSimpleTypeUnion);
					this.PreprocessSimpleType(xmlSchemaSimpleType, true);
				}
				this.PreprocessAnnotation(xmlSchemaSimpleTypeUnion);
				this.ValidateIdAttribute(xmlSchemaSimpleTypeUnion);
			}
			this.ValidateIdAttribute(simpleType);
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x00070BB0 File Offset: 0x0006FBB0
		private void PreprocessComplexType(XmlSchemaComplexType complexType, bool local)
		{
			if (local)
			{
				if (complexType.Name != null)
				{
					base.SendValidationEvent("Sch_ForbiddenAttribute", "name", complexType);
				}
			}
			else
			{
				if (complexType.Name != null)
				{
					this.ValidateNameAttribute(complexType);
					complexType.SetQualifiedName(new XmlQualifiedName(complexType.Name, this.targetNamespace));
				}
				else
				{
					base.SendValidationEvent("Sch_MissRequiredAttribute", "name", complexType);
				}
				if (complexType.Block == XmlSchemaDerivationMethod.All)
				{
					complexType.SetBlockResolved(XmlSchemaDerivationMethod.All);
				}
				else if (complexType.Block == XmlSchemaDerivationMethod.None)
				{
					complexType.SetBlockResolved(this.blockDefault & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
				}
				else
				{
					if ((complexType.Block & ~(XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction)) != XmlSchemaDerivationMethod.Empty)
					{
						base.SendValidationEvent("Sch_InvalidComplexTypeBlockValue", complexType);
					}
					complexType.SetBlockResolved(complexType.Block & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
				}
				if (complexType.Final == XmlSchemaDerivationMethod.All)
				{
					complexType.SetFinalResolved(XmlSchemaDerivationMethod.All);
				}
				else if (complexType.Final == XmlSchemaDerivationMethod.None)
				{
					if (this.finalDefault == XmlSchemaDerivationMethod.All)
					{
						complexType.SetFinalResolved(XmlSchemaDerivationMethod.All);
					}
					else
					{
						complexType.SetFinalResolved(this.finalDefault & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
					}
				}
				else
				{
					if ((complexType.Final & ~(XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction)) != XmlSchemaDerivationMethod.Empty)
					{
						base.SendValidationEvent("Sch_InvalidComplexTypeFinalValue", complexType);
					}
					complexType.SetFinalResolved(complexType.Final & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
				}
			}
			if (complexType.ContentModel != null)
			{
				this.SetParent(complexType.ContentModel, complexType);
				this.PreprocessAnnotation(complexType.ContentModel);
				if (complexType.Particle == null)
				{
					XmlSchemaObjectCollection attributes = complexType.Attributes;
				}
				if (complexType.ContentModel is XmlSchemaSimpleContent)
				{
					XmlSchemaSimpleContent xmlSchemaSimpleContent = (XmlSchemaSimpleContent)complexType.ContentModel;
					if (xmlSchemaSimpleContent.Content == null)
					{
						if (complexType.QualifiedName == XmlQualifiedName.Empty)
						{
							base.SendValidationEvent("Sch_NoRestOrExt", complexType);
						}
						else
						{
							base.SendValidationEvent("Sch_NoRestOrExtQName", complexType.QualifiedName.Name, complexType.QualifiedName.Namespace, complexType);
						}
					}
					else
					{
						this.SetParent(xmlSchemaSimpleContent.Content, xmlSchemaSimpleContent);
						this.PreprocessAnnotation(xmlSchemaSimpleContent.Content);
						if (xmlSchemaSimpleContent.Content is XmlSchemaSimpleContentExtension)
						{
							XmlSchemaSimpleContentExtension xmlSchemaSimpleContentExtension = (XmlSchemaSimpleContentExtension)xmlSchemaSimpleContent.Content;
							if (xmlSchemaSimpleContentExtension.BaseTypeName.IsEmpty)
							{
								base.SendValidationEvent("Sch_MissAttribute", "base", xmlSchemaSimpleContentExtension);
							}
							else
							{
								this.ValidateQNameAttribute(xmlSchemaSimpleContentExtension, "base", xmlSchemaSimpleContentExtension.BaseTypeName);
							}
							this.PreprocessAttributes(xmlSchemaSimpleContentExtension.Attributes, xmlSchemaSimpleContentExtension.AnyAttribute, xmlSchemaSimpleContentExtension);
							this.ValidateIdAttribute(xmlSchemaSimpleContentExtension);
						}
						else
						{
							XmlSchemaSimpleContentRestriction xmlSchemaSimpleContentRestriction = (XmlSchemaSimpleContentRestriction)xmlSchemaSimpleContent.Content;
							if (xmlSchemaSimpleContentRestriction.BaseTypeName.IsEmpty)
							{
								base.SendValidationEvent("Sch_MissAttribute", "base", xmlSchemaSimpleContentRestriction);
							}
							else
							{
								this.ValidateQNameAttribute(xmlSchemaSimpleContentRestriction, "base", xmlSchemaSimpleContentRestriction.BaseTypeName);
							}
							if (xmlSchemaSimpleContentRestriction.BaseType != null)
							{
								this.SetParent(xmlSchemaSimpleContentRestriction.BaseType, xmlSchemaSimpleContentRestriction);
								this.PreprocessSimpleType(xmlSchemaSimpleContentRestriction.BaseType, true);
							}
							this.PreprocessAttributes(xmlSchemaSimpleContentRestriction.Attributes, xmlSchemaSimpleContentRestriction.AnyAttribute, xmlSchemaSimpleContentRestriction);
							this.ValidateIdAttribute(xmlSchemaSimpleContentRestriction);
						}
					}
					this.ValidateIdAttribute(xmlSchemaSimpleContent);
				}
				else
				{
					XmlSchemaComplexContent xmlSchemaComplexContent = (XmlSchemaComplexContent)complexType.ContentModel;
					if (xmlSchemaComplexContent.Content == null)
					{
						if (complexType.QualifiedName == XmlQualifiedName.Empty)
						{
							base.SendValidationEvent("Sch_NoRestOrExt", complexType);
						}
						else
						{
							base.SendValidationEvent("Sch_NoRestOrExtQName", complexType.QualifiedName.Name, complexType.QualifiedName.Namespace, complexType);
						}
					}
					else
					{
						if (!xmlSchemaComplexContent.HasMixedAttribute && complexType.IsMixed)
						{
							xmlSchemaComplexContent.IsMixed = true;
						}
						this.SetParent(xmlSchemaComplexContent.Content, xmlSchemaComplexContent);
						this.PreprocessAnnotation(xmlSchemaComplexContent.Content);
						if (xmlSchemaComplexContent.Content is XmlSchemaComplexContentExtension)
						{
							XmlSchemaComplexContentExtension xmlSchemaComplexContentExtension = (XmlSchemaComplexContentExtension)xmlSchemaComplexContent.Content;
							if (xmlSchemaComplexContentExtension.BaseTypeName.IsEmpty)
							{
								base.SendValidationEvent("Sch_MissAttribute", "base", xmlSchemaComplexContentExtension);
							}
							else
							{
								this.ValidateQNameAttribute(xmlSchemaComplexContentExtension, "base", xmlSchemaComplexContentExtension.BaseTypeName);
							}
							if (xmlSchemaComplexContentExtension.Particle != null)
							{
								this.SetParent(xmlSchemaComplexContentExtension.Particle, xmlSchemaComplexContentExtension);
								this.PreprocessParticle(xmlSchemaComplexContentExtension.Particle);
							}
							this.PreprocessAttributes(xmlSchemaComplexContentExtension.Attributes, xmlSchemaComplexContentExtension.AnyAttribute, xmlSchemaComplexContentExtension);
							this.ValidateIdAttribute(xmlSchemaComplexContentExtension);
						}
						else
						{
							XmlSchemaComplexContentRestriction xmlSchemaComplexContentRestriction = (XmlSchemaComplexContentRestriction)xmlSchemaComplexContent.Content;
							if (xmlSchemaComplexContentRestriction.BaseTypeName.IsEmpty)
							{
								base.SendValidationEvent("Sch_MissAttribute", "base", xmlSchemaComplexContentRestriction);
							}
							else
							{
								this.ValidateQNameAttribute(xmlSchemaComplexContentRestriction, "base", xmlSchemaComplexContentRestriction.BaseTypeName);
							}
							if (xmlSchemaComplexContentRestriction.Particle != null)
							{
								this.SetParent(xmlSchemaComplexContentRestriction.Particle, xmlSchemaComplexContentRestriction);
								this.PreprocessParticle(xmlSchemaComplexContentRestriction.Particle);
							}
							this.PreprocessAttributes(xmlSchemaComplexContentRestriction.Attributes, xmlSchemaComplexContentRestriction.AnyAttribute, xmlSchemaComplexContentRestriction);
							this.ValidateIdAttribute(xmlSchemaComplexContentRestriction);
						}
						this.ValidateIdAttribute(xmlSchemaComplexContent);
					}
				}
			}
			else
			{
				if (complexType.Particle != null)
				{
					this.SetParent(complexType.Particle, complexType);
					this.PreprocessParticle(complexType.Particle);
				}
				this.PreprocessAttributes(complexType.Attributes, complexType.AnyAttribute, complexType);
			}
			this.ValidateIdAttribute(complexType);
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x00071088 File Offset: 0x00070088
		private void PreprocessGroup(XmlSchemaGroup group)
		{
			if (group.Name != null)
			{
				this.ValidateNameAttribute(group);
				group.SetQualifiedName(new XmlQualifiedName(group.Name, this.targetNamespace));
			}
			else
			{
				base.SendValidationEvent("Sch_MissRequiredAttribute", "name", group);
			}
			if (group.Particle == null)
			{
				base.SendValidationEvent("Sch_NoGroupParticle", group);
				return;
			}
			if (group.Particle.MinOccursString != null)
			{
				base.SendValidationEvent("Sch_ForbiddenAttribute", "minOccurs", group.Particle);
			}
			if (group.Particle.MaxOccursString != null)
			{
				base.SendValidationEvent("Sch_ForbiddenAttribute", "maxOccurs", group.Particle);
			}
			this.PreprocessParticle(group.Particle);
			this.PreprocessAnnotation(group);
			this.ValidateIdAttribute(group);
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x00071144 File Offset: 0x00070144
		private void PreprocessNotation(XmlSchemaNotation notation)
		{
			if (notation.Name != null)
			{
				this.ValidateNameAttribute(notation);
				notation.QualifiedName = new XmlQualifiedName(notation.Name, this.targetNamespace);
			}
			else
			{
				base.SendValidationEvent("Sch_MissRequiredAttribute", "name", notation);
			}
			if (notation.Public == null && notation.System == null)
			{
				base.SendValidationEvent("Sch_MissingPublicSystemAttribute", notation);
			}
			else
			{
				if (notation.Public != null)
				{
					try
					{
						XmlConvert.VerifyTOKEN(notation.Public);
					}
					catch (XmlException ex)
					{
						base.SendValidationEvent("Sch_InvalidPublicAttribute", new string[] { notation.Public }, ex, notation);
					}
				}
				if (notation.System != null)
				{
					this.ParseUri(notation.System, "Sch_InvalidSystemAttribute", notation);
				}
			}
			this.PreprocessAnnotation(notation);
			this.ValidateIdAttribute(notation);
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x00071218 File Offset: 0x00070218
		private void PreprocessParticle(XmlSchemaParticle particle)
		{
			if (particle is XmlSchemaAll)
			{
				if (particle.MinOccurs != 0m && particle.MinOccurs != 1m)
				{
					particle.MinOccurs = 1m;
					base.SendValidationEvent("Sch_InvalidAllMin", particle);
				}
				if (particle.MaxOccurs != 1m)
				{
					particle.MaxOccurs = 1m;
					base.SendValidationEvent("Sch_InvalidAllMax", particle);
				}
				using (XmlSchemaObjectEnumerator enumerator = ((XmlSchemaAll)particle).Items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						XmlSchemaObject xmlSchemaObject = enumerator.Current;
						XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)xmlSchemaObject;
						if (xmlSchemaElement.MaxOccurs != 0m && xmlSchemaElement.MaxOccurs != 1m)
						{
							xmlSchemaElement.MaxOccurs = 1m;
							base.SendValidationEvent("Sch_InvalidAllElementMax", xmlSchemaElement);
						}
						this.SetParent(xmlSchemaElement, particle);
						this.PreprocessLocalElement(xmlSchemaElement);
					}
					goto IL_0297;
				}
			}
			if (particle.MinOccurs > particle.MaxOccurs)
			{
				particle.MinOccurs = particle.MaxOccurs;
				base.SendValidationEvent("Sch_MinGtMax", particle);
			}
			if (particle is XmlSchemaChoice)
			{
				using (XmlSchemaObjectEnumerator enumerator2 = ((XmlSchemaChoice)particle).Items.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						XmlSchemaObject xmlSchemaObject2 = enumerator2.Current;
						this.SetParent(xmlSchemaObject2, particle);
						if (xmlSchemaObject2 is XmlSchemaElement)
						{
							this.PreprocessLocalElement((XmlSchemaElement)xmlSchemaObject2);
						}
						else
						{
							this.PreprocessParticle((XmlSchemaParticle)xmlSchemaObject2);
						}
					}
					goto IL_0297;
				}
			}
			if (particle is XmlSchemaSequence)
			{
				using (XmlSchemaObjectEnumerator enumerator3 = ((XmlSchemaSequence)particle).Items.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						XmlSchemaObject xmlSchemaObject3 = enumerator3.Current;
						this.SetParent(xmlSchemaObject3, particle);
						if (xmlSchemaObject3 is XmlSchemaElement)
						{
							this.PreprocessLocalElement((XmlSchemaElement)xmlSchemaObject3);
						}
						else
						{
							this.PreprocessParticle((XmlSchemaParticle)xmlSchemaObject3);
						}
					}
					goto IL_0297;
				}
			}
			if (particle is XmlSchemaGroupRef)
			{
				XmlSchemaGroupRef xmlSchemaGroupRef = (XmlSchemaGroupRef)particle;
				if (xmlSchemaGroupRef.RefName.IsEmpty)
				{
					base.SendValidationEvent("Sch_MissAttribute", "ref", xmlSchemaGroupRef);
				}
				else
				{
					this.ValidateQNameAttribute(xmlSchemaGroupRef, "ref", xmlSchemaGroupRef.RefName);
				}
			}
			else if (particle is XmlSchemaAny)
			{
				try
				{
					((XmlSchemaAny)particle).BuildNamespaceList(this.targetNamespace);
				}
				catch (FormatException ex)
				{
					base.SendValidationEvent("Sch_InvalidAnyDetailed", new string[] { ex.Message }, ex, particle);
				}
			}
			IL_0297:
			this.PreprocessAnnotation(particle);
			this.ValidateIdAttribute(particle);
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x00071500 File Offset: 0x00070500
		private void PreprocessAttributes(XmlSchemaObjectCollection attributes, XmlSchemaAnyAttribute anyAttribute, XmlSchemaObject parent)
		{
			foreach (XmlSchemaObject xmlSchemaObject in attributes)
			{
				XmlSchemaAnnotated xmlSchemaAnnotated = (XmlSchemaAnnotated)xmlSchemaObject;
				this.SetParent(xmlSchemaAnnotated, parent);
				if (xmlSchemaAnnotated is XmlSchemaAttribute)
				{
					this.PreprocessLocalAttribute((XmlSchemaAttribute)xmlSchemaAnnotated);
				}
				else
				{
					XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = (XmlSchemaAttributeGroupRef)xmlSchemaAnnotated;
					if (xmlSchemaAttributeGroupRef.RefName.IsEmpty)
					{
						base.SendValidationEvent("Sch_MissAttribute", "ref", xmlSchemaAttributeGroupRef);
					}
					else
					{
						this.ValidateQNameAttribute(xmlSchemaAttributeGroupRef, "ref", xmlSchemaAttributeGroupRef.RefName);
					}
					this.PreprocessAnnotation(xmlSchemaAnnotated);
					this.ValidateIdAttribute(xmlSchemaAnnotated);
				}
			}
			if (anyAttribute != null)
			{
				try
				{
					this.SetParent(anyAttribute, parent);
					this.PreprocessAnnotation(anyAttribute);
					anyAttribute.BuildNamespaceList(this.targetNamespace);
				}
				catch (FormatException ex)
				{
					base.SendValidationEvent("Sch_InvalidAnyDetailed", new string[] { ex.Message }, ex, anyAttribute);
				}
				this.ValidateIdAttribute(anyAttribute);
			}
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x0007160C File Offset: 0x0007060C
		private void ValidateIdAttribute(XmlSchemaObject xso)
		{
			if (xso.IdAttribute != null)
			{
				try
				{
					xso.IdAttribute = base.NameTable.Add(XmlConvert.VerifyNCName(xso.IdAttribute));
				}
				catch (XmlException ex)
				{
					base.SendValidationEvent("Sch_InvalidIdAttribute", new string[] { ex.Message }, ex, xso);
					return;
				}
				catch (ArgumentNullException)
				{
					base.SendValidationEvent("Sch_InvalidIdAttribute", Res.GetString("Sch_NullValue"), xso);
					return;
				}
				try
				{
					this.currentSchema.Ids.Add(xso.IdAttribute, xso);
				}
				catch (ArgumentException)
				{
					base.SendValidationEvent("Sch_DupIdAttribute", xso);
				}
			}
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x000716CC File Offset: 0x000706CC
		private void ValidateNameAttribute(XmlSchemaObject xso)
		{
			string text = xso.NameAttribute;
			if (text == null || text.Length == 0)
			{
				base.SendValidationEvent("Sch_InvalidNameAttributeEx", null, Res.GetString("Sch_NullValue"), xso);
			}
			text = XmlComplianceUtil.NonCDataNormalize(text);
			int num = ValidateNames.ParseNCName(text, 0);
			if (num != text.Length)
			{
				string @string = Res.GetString("Xml_BadNameCharWithPos", new object[]
				{
					XmlException.BuildCharExceptionStr(text[num])[0],
					XmlException.BuildCharExceptionStr(text[num])[1],
					num
				});
				base.SendValidationEvent("Sch_InvalidNameAttributeEx", text, @string, xso);
				return;
			}
			xso.NameAttribute = base.NameTable.Add(text);
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x0007177C File Offset: 0x0007077C
		private void ValidateQNameAttribute(XmlSchemaObject xso, string attributeName, XmlQualifiedName value)
		{
			try
			{
				value.Verify();
				value.Atomize(base.NameTable);
				if (this.currentSchema.IsChameleon && value.Namespace.Length == 0)
				{
					value.SetNamespace(this.currentSchema.TargetNamespace);
				}
				if (this.referenceNamespaces[value.Namespace] == null)
				{
					base.SendValidationEvent("Sch_UnrefNS", value.Namespace, xso, XmlSeverityType.Warning);
				}
			}
			catch (FormatException ex)
			{
				base.SendValidationEvent("Sch_InvalidAttribute", new string[] { attributeName, ex.Message }, ex, xso);
			}
			catch (XmlException ex2)
			{
				base.SendValidationEvent("Sch_InvalidAttribute", new string[] { attributeName, ex2.Message }, ex2, xso);
			}
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x00071854 File Offset: 0x00070854
		private Uri ResolveSchemaLocationUri(XmlSchema enclosingSchema, string location)
		{
			if (location.Length == 0)
			{
				return null;
			}
			return this.xmlResolver.ResolveUri(enclosingSchema.BaseUri, location);
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x00071872 File Offset: 0x00070872
		private object GetSchemaEntity(Uri ruri)
		{
			return this.xmlResolver.GetEntity(ruri, null, null);
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x00071884 File Offset: 0x00070884
		private XmlSchema GetChameleonSchema(string targetNamespace, XmlSchema schema)
		{
			ChameleonKey chameleonKey = new ChameleonKey(targetNamespace, schema.BaseUri);
			XmlSchema xmlSchema = (XmlSchema)this.chameleonSchemas[chameleonKey];
			if (xmlSchema == null)
			{
				xmlSchema = schema.DeepClone();
				xmlSchema.IsChameleon = true;
				xmlSchema.TargetNamespace = targetNamespace;
				this.chameleonSchemas.Add(chameleonKey, xmlSchema);
				schema.IsProcessing = false;
			}
			return xmlSchema;
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x000718DD File Offset: 0x000708DD
		private void SetParent(XmlSchemaObject child, XmlSchemaObject parent)
		{
			child.Parent = parent;
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x000718E8 File Offset: 0x000708E8
		private void PreprocessAnnotation(XmlSchemaObject schemaObject)
		{
			if (schemaObject is XmlSchemaAnnotated)
			{
				XmlSchemaAnnotated xmlSchemaAnnotated = schemaObject as XmlSchemaAnnotated;
				XmlSchemaAnnotation annotation = xmlSchemaAnnotated.Annotation;
				if (annotation != null)
				{
					this.PreprocessAnnotation(annotation);
					annotation.Parent = schemaObject;
				}
			}
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x0007191C File Offset: 0x0007091C
		private void PreprocessAnnotation(XmlSchemaAnnotation annotation)
		{
			this.ValidateIdAttribute(annotation);
			foreach (XmlSchemaObject xmlSchemaObject in annotation.Items)
			{
				xmlSchemaObject.Parent = annotation;
			}
		}

		// Token: 0x04000E72 RID: 3698
		private const XmlSchemaDerivationMethod schemaBlockDefaultAllowed = XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

		// Token: 0x04000E73 RID: 3699
		private const XmlSchemaDerivationMethod schemaFinalDefaultAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union;

		// Token: 0x04000E74 RID: 3700
		private const XmlSchemaDerivationMethod elementBlockAllowed = XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

		// Token: 0x04000E75 RID: 3701
		private const XmlSchemaDerivationMethod elementFinalAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

		// Token: 0x04000E76 RID: 3702
		private const XmlSchemaDerivationMethod simpleTypeFinalAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union;

		// Token: 0x04000E77 RID: 3703
		private const XmlSchemaDerivationMethod complexTypeBlockAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

		// Token: 0x04000E78 RID: 3704
		private const XmlSchemaDerivationMethod complexTypeFinalAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

		// Token: 0x04000E79 RID: 3705
		private string Xmlns;

		// Token: 0x04000E7A RID: 3706
		private string NsXsi;

		// Token: 0x04000E7B RID: 3707
		private string targetNamespace;

		// Token: 0x04000E7C RID: 3708
		private XmlSchema rootSchema;

		// Token: 0x04000E7D RID: 3709
		private XmlSchema currentSchema;

		// Token: 0x04000E7E RID: 3710
		private XmlSchemaForm elementFormDefault;

		// Token: 0x04000E7F RID: 3711
		private XmlSchemaForm attributeFormDefault;

		// Token: 0x04000E80 RID: 3712
		private XmlSchemaDerivationMethod blockDefault;

		// Token: 0x04000E81 RID: 3713
		private XmlSchemaDerivationMethod finalDefault;

		// Token: 0x04000E82 RID: 3714
		private Hashtable schemaLocations;

		// Token: 0x04000E83 RID: 3715
		private Hashtable chameleonSchemas;

		// Token: 0x04000E84 RID: 3716
		private Hashtable referenceNamespaces;

		// Token: 0x04000E85 RID: 3717
		private Hashtable processedExternals;

		// Token: 0x04000E86 RID: 3718
		private SortedList lockList;

		// Token: 0x04000E87 RID: 3719
		private XmlReaderSettings readerSettings;

		// Token: 0x04000E88 RID: 3720
		private XmlSchema rootSchemaForRedefine;

		// Token: 0x04000E89 RID: 3721
		private ArrayList redefinedList;

		// Token: 0x04000E8A RID: 3722
		private static XmlSchema builtInSchemaForXmlNS;

		// Token: 0x04000E8B RID: 3723
		private XmlResolver xmlResolver;
	}
}
