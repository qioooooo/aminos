using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace System.Xml.Schema
{
	// Token: 0x02000273 RID: 627
	public class XmlSchemaSet
	{
		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x00083D78 File Offset: 0x00082D78
		internal object InternalSyncObject
		{
			get
			{
				if (this.internalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref this.internalSyncObject, obj, null);
				}
				return this.internalSyncObject;
			}
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x00083DA7 File Offset: 0x00082DA7
		public XmlSchemaSet()
			: this(new NameTable())
		{
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x00083DB4 File Offset: 0x00082DB4
		public XmlSchemaSet(XmlNameTable nameTable)
		{
			if (nameTable == null)
			{
				throw new ArgumentNullException("nameTable");
			}
			this.nameTable = nameTable;
			this.schemas = new SortedList();
			this.schemaLocations = new Hashtable();
			this.chameleonSchemas = new Hashtable();
			this.targetNamespaces = new Hashtable();
			this.internalEventHandler = new ValidationEventHandler(this.InternalValidationCallback);
			this.eventHandler = this.internalEventHandler;
			this.readerSettings = new XmlReaderSettings();
			this.readerSettings.NameTable = nameTable;
			this.readerSettings.ProhibitDtd = true;
			this.compilationSettings = new XmlSchemaCompilationSettings();
			this.cachedCompiledInfo = new SchemaInfo();
			this.compileAll = true;
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x00083E66 File Offset: 0x00082E66
		public XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06001CF4 RID: 7412 RVA: 0x00083E70 File Offset: 0x00082E70
		// (remove) Token: 0x06001CF5 RID: 7413 RVA: 0x00083EC4 File Offset: 0x00082EC4
		public event ValidationEventHandler ValidationEventHandler
		{
			add
			{
				this.eventHandler = (ValidationEventHandler)Delegate.Remove(this.eventHandler, this.internalEventHandler);
				this.eventHandler = (ValidationEventHandler)Delegate.Combine(this.eventHandler, value);
				if (this.eventHandler == null)
				{
					this.eventHandler = this.internalEventHandler;
				}
			}
			remove
			{
				this.eventHandler = (ValidationEventHandler)Delegate.Remove(this.eventHandler, value);
				if (this.eventHandler == null)
				{
					this.eventHandler = this.internalEventHandler;
				}
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x00083EF1 File Offset: 0x00082EF1
		public bool IsCompiled
		{
			get
			{
				return this.isCompiled;
			}
		}

		// Token: 0x1700077F RID: 1919
		// (set) Token: 0x06001CF7 RID: 7415 RVA: 0x00083EF9 File Offset: 0x00082EF9
		public XmlResolver XmlResolver
		{
			set
			{
				this.readerSettings.XmlResolver = value;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x00083F07 File Offset: 0x00082F07
		// (set) Token: 0x06001CF9 RID: 7417 RVA: 0x00083F0F File Offset: 0x00082F0F
		public XmlSchemaCompilationSettings CompilationSettings
		{
			get
			{
				return this.compilationSettings;
			}
			set
			{
				this.compilationSettings = value;
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06001CFA RID: 7418 RVA: 0x00083F18 File Offset: 0x00082F18
		public int Count
		{
			get
			{
				return this.schemas.Count;
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06001CFB RID: 7419 RVA: 0x00083F25 File Offset: 0x00082F25
		public XmlSchemaObjectTable GlobalElements
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

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06001CFC RID: 7420 RVA: 0x00083F40 File Offset: 0x00082F40
		public XmlSchemaObjectTable GlobalAttributes
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

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06001CFD RID: 7421 RVA: 0x00083F5B File Offset: 0x00082F5B
		public XmlSchemaObjectTable GlobalTypes
		{
			get
			{
				if (this.schemaTypes == null)
				{
					this.schemaTypes = new XmlSchemaObjectTable();
				}
				return this.schemaTypes;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06001CFE RID: 7422 RVA: 0x00083F76 File Offset: 0x00082F76
		internal XmlSchemaObjectTable SubstitutionGroups
		{
			get
			{
				if (this.substitutionGroups == null)
				{
					this.substitutionGroups = new XmlSchemaObjectTable();
				}
				return this.substitutionGroups;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06001CFF RID: 7423 RVA: 0x00083F91 File Offset: 0x00082F91
		internal Hashtable SchemaLocations
		{
			get
			{
				return this.schemaLocations;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06001D00 RID: 7424 RVA: 0x00083F99 File Offset: 0x00082F99
		internal XmlSchemaObjectTable TypeExtensions
		{
			get
			{
				if (this.typeExtensions == null)
				{
					this.typeExtensions = new XmlSchemaObjectTable();
				}
				return this.typeExtensions;
			}
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x00083FB4 File Offset: 0x00082FB4
		public XmlSchema Add(string targetNamespace, string schemaUri)
		{
			if (schemaUri == null || schemaUri.Length == 0)
			{
				throw new ArgumentNullException("schemaUri");
			}
			if (targetNamespace != null)
			{
				targetNamespace = XmlComplianceUtil.CDataNormalize(targetNamespace);
			}
			XmlSchema xmlSchema = null;
			lock (this.InternalSyncObject)
			{
				XmlResolver xmlResolver = this.readerSettings.GetXmlResolver();
				if (xmlResolver == null)
				{
					xmlResolver = new XmlUrlResolver();
				}
				Uri uri = xmlResolver.ResolveUri(null, schemaUri);
				if (this.IsSchemaLoaded(uri, targetNamespace, out xmlSchema))
				{
					return xmlSchema;
				}
				XmlReader xmlReader = XmlReader.Create(schemaUri, this.readerSettings);
				try
				{
					xmlSchema = this.Add(targetNamespace, this.ParseSchema(targetNamespace, xmlReader));
					while (xmlReader.Read())
					{
					}
				}
				finally
				{
					xmlReader.Close();
				}
			}
			return xmlSchema;
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x0008407C File Offset: 0x0008307C
		public XmlSchema Add(string targetNamespace, XmlReader schemaDocument)
		{
			if (schemaDocument == null)
			{
				throw new ArgumentNullException("schemaDocument");
			}
			if (targetNamespace != null)
			{
				targetNamespace = XmlComplianceUtil.CDataNormalize(targetNamespace);
			}
			XmlSchema xmlSchema2;
			lock (this.InternalSyncObject)
			{
				XmlSchema xmlSchema = null;
				Uri uri = new Uri(schemaDocument.BaseURI, UriKind.RelativeOrAbsolute);
				if (this.IsSchemaLoaded(uri, targetNamespace, out xmlSchema))
				{
					xmlSchema2 = xmlSchema;
				}
				else
				{
					bool prohibitDtd = this.readerSettings.ProhibitDtd;
					this.SetProhibitDtd(schemaDocument);
					xmlSchema = this.Add(targetNamespace, this.ParseSchema(targetNamespace, schemaDocument));
					this.readerSettings.ProhibitDtd = prohibitDtd;
					xmlSchema2 = xmlSchema;
				}
			}
			return xmlSchema2;
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x0008411C File Offset: 0x0008311C
		public void Add(XmlSchemaSet schemas)
		{
			if (schemas == null)
			{
				throw new ArgumentNullException("schemas");
			}
			if (this == schemas)
			{
				return;
			}
			bool flag = false;
			try
			{
				for (;;)
				{
					if (Monitor.TryEnter(this.InternalSyncObject))
					{
						if (Monitor.TryEnter(schemas.InternalSyncObject))
						{
							break;
						}
						Monitor.Exit(this.InternalSyncObject);
					}
				}
				flag = true;
				if (schemas.IsCompiled)
				{
					this.CopyFromCompiledSet(schemas);
				}
				else
				{
					bool flag2 = false;
					foreach (object obj in schemas.SortedSchemas.Values)
					{
						XmlSchema xmlSchema = (XmlSchema)obj;
						string text = xmlSchema.TargetNamespace;
						if (text == null)
						{
							text = string.Empty;
						}
						if (!this.schemas.ContainsKey(xmlSchema.SchemaId) && this.FindSchemaByNSAndUrl(xmlSchema.BaseUri, text, null) == null && this.Add(xmlSchema.TargetNamespace, xmlSchema) == null)
						{
							flag2 = true;
							break;
						}
					}
					if (flag2)
					{
						foreach (object obj2 in schemas.SortedSchemas.Values)
						{
							XmlSchema xmlSchema2 = (XmlSchema)obj2;
							this.schemas.Remove(xmlSchema2.SchemaId);
							this.schemaLocations.Remove(xmlSchema2.BaseUri);
						}
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this.InternalSyncObject);
					Monitor.Exit(schemas.InternalSyncObject);
				}
			}
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x000842EC File Offset: 0x000832EC
		public XmlSchema Add(XmlSchema schema)
		{
			if (schema == null)
			{
				throw new ArgumentNullException("schema");
			}
			XmlSchema xmlSchema;
			lock (this.InternalSyncObject)
			{
				if (this.schemas.ContainsKey(schema.SchemaId))
				{
					xmlSchema = schema;
				}
				else
				{
					xmlSchema = this.Add(schema.TargetNamespace, schema);
				}
			}
			return xmlSchema;
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x00084358 File Offset: 0x00083358
		public XmlSchema Remove(XmlSchema schema)
		{
			return this.Remove(schema, true);
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x00084364 File Offset: 0x00083364
		public bool RemoveRecursive(XmlSchema schemaToRemove)
		{
			if (schemaToRemove == null)
			{
				throw new ArgumentNullException("schemaToRemove");
			}
			if (!this.schemas.ContainsKey(schemaToRemove.SchemaId))
			{
				return false;
			}
			lock (this.InternalSyncObject)
			{
				if (this.schemas.ContainsKey(schemaToRemove.SchemaId))
				{
					Hashtable hashtable = new Hashtable();
					hashtable.Add(this.GetTargetNamespace(schemaToRemove), schemaToRemove);
					for (int i = 0; i < schemaToRemove.ImportedNamespaces.Count; i++)
					{
						string text = (string)schemaToRemove.ImportedNamespaces[i];
						if (hashtable[text] == null)
						{
							hashtable.Add(text, text);
						}
					}
					ArrayList arrayList = new ArrayList();
					for (int j = 0; j < this.schemas.Count; j++)
					{
						XmlSchema xmlSchema = (XmlSchema)this.schemas.GetByIndex(j);
						if (xmlSchema != schemaToRemove && !schemaToRemove.ImportedSchemas.Contains(xmlSchema))
						{
							arrayList.Add(xmlSchema);
						}
					}
					for (int k = 0; k < arrayList.Count; k++)
					{
						XmlSchema xmlSchema = (XmlSchema)arrayList[k];
						if (xmlSchema.ImportedNamespaces.Count > 0)
						{
							foreach (object obj2 in hashtable.Keys)
							{
								string text2 = (string)obj2;
								if (xmlSchema.ImportedNamespaces.Contains(text2))
								{
									this.SendValidationEvent(new XmlSchemaException("Sch_SchemaNotRemoved", string.Empty), XmlSeverityType.Warning);
									return false;
								}
							}
						}
					}
					this.RemoveSchemaFromGlobalTables(schemaToRemove);
					this.Remove(schemaToRemove, false);
					foreach (object obj3 in schemaToRemove.ImportedSchemas)
					{
						XmlSchema xmlSchema2 = (XmlSchema)obj3;
						this.RemoveSchemaFromGlobalTables(xmlSchema2);
						this.Remove(xmlSchema2, false);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x000845C8 File Offset: 0x000835C8
		public bool Contains(string targetNamespace)
		{
			if (targetNamespace == null)
			{
				targetNamespace = string.Empty;
			}
			return this.targetNamespaces[targetNamespace] != null;
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x000845E6 File Offset: 0x000835E6
		public bool Contains(XmlSchema schema)
		{
			if (schema == null)
			{
				throw new ArgumentNullException("schema");
			}
			return this.schemas.ContainsValue(schema);
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x00084604 File Offset: 0x00083604
		public void Compile()
		{
			if (this.schemas.Count == 0)
			{
				this.ClearTables();
				return;
			}
			if (this.isCompiled)
			{
				return;
			}
			lock (this.InternalSyncObject)
			{
				if (!this.isCompiled)
				{
					Compiler compiler = new Compiler(this.nameTable, this.eventHandler, this.schemaForSchema, this.compilationSettings);
					SchemaInfo schemaInfo = new SchemaInfo();
					int i = 0;
					if (!this.compileAll)
					{
						compiler.ImportAllCompiledSchemas(this);
					}
					try
					{
						XmlSchema buildInSchema = Preprocessor.GetBuildInSchema();
						i = 0;
						while (i < this.schemas.Count)
						{
							XmlSchema xmlSchema = (XmlSchema)this.schemas.GetByIndex(i);
							Monitor.Enter(xmlSchema);
							if (!xmlSchema.IsPreprocessed)
							{
								this.SendValidationEvent(new XmlSchemaException("Sch_SchemaNotPreprocessed", string.Empty), XmlSeverityType.Error);
								this.isCompiled = false;
								return;
							}
							if (!xmlSchema.IsCompiledBySet)
							{
								goto IL_00D7;
							}
							if (this.compileAll)
							{
								if (xmlSchema != buildInSchema)
								{
									goto IL_00D7;
								}
								compiler.Prepare(xmlSchema, false);
							}
							IL_00DF:
							i++;
							continue;
							IL_00D7:
							compiler.Prepare(xmlSchema, true);
							goto IL_00DF;
						}
						this.isCompiled = compiler.Execute(this, schemaInfo);
						if (this.isCompiled)
						{
							this.compileAll = false;
							schemaInfo.Add(this.cachedCompiledInfo, this.eventHandler);
							this.cachedCompiledInfo = schemaInfo;
						}
					}
					finally
					{
						if (i == this.schemas.Count)
						{
							i--;
						}
						for (int j = i; j >= 0; j--)
						{
							XmlSchema xmlSchema2 = (XmlSchema)this.schemas.GetByIndex(j);
							if (xmlSchema2 == Preprocessor.GetBuildInSchema())
							{
								Monitor.Exit(xmlSchema2);
							}
							else
							{
								xmlSchema2.IsCompiledBySet = this.isCompiled;
								Monitor.Exit(xmlSchema2);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x000847D8 File Offset: 0x000837D8
		public XmlSchema Reprocess(XmlSchema schema)
		{
			if (schema == null)
			{
				throw new ArgumentNullException("schema");
			}
			if (!this.schemas.ContainsKey(schema.SchemaId))
			{
				throw new ArgumentException(Res.GetString("Sch_SchemaDoesNotExist"), "schema");
			}
			lock (this.InternalSyncObject)
			{
				this.RemoveSchemaFromCaches(schema);
				this.PreprocessSchema(ref schema, schema.TargetNamespace);
				foreach (object obj2 in schema.ImportedSchemas)
				{
					XmlSchema xmlSchema = (XmlSchema)obj2;
					if (!this.schemas.ContainsKey(xmlSchema.SchemaId))
					{
						this.schemas.Add(xmlSchema.SchemaId, xmlSchema);
					}
					string targetNamespace = this.GetTargetNamespace(xmlSchema);
					if (this.targetNamespaces[targetNamespace] == null)
					{
						this.targetNamespaces.Add(targetNamespace, targetNamespace);
					}
				}
				this.isCompiled = false;
			}
			return schema;
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x000848FC File Offset: 0x000838FC
		public void CopyTo(XmlSchema[] schemas, int index)
		{
			if (schemas == null)
			{
				throw new ArgumentNullException("schemas");
			}
			if (index < 0 || index > schemas.Length - 1)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.schemas.Values.CopyTo(schemas, index);
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x00084935 File Offset: 0x00083935
		public ICollection Schemas()
		{
			return this.schemas.Values;
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x00084944 File Offset: 0x00083944
		public ICollection Schemas(string targetNamespace)
		{
			ArrayList arrayList = new ArrayList();
			if (targetNamespace == null)
			{
				targetNamespace = string.Empty;
			}
			for (int i = 0; i < this.schemas.Count; i++)
			{
				XmlSchema xmlSchema = (XmlSchema)this.schemas.GetByIndex(i);
				if (this.GetTargetNamespace(xmlSchema) == targetNamespace)
				{
					arrayList.Add(xmlSchema);
				}
			}
			return arrayList;
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x000849A1 File Offset: 0x000839A1
		internal XmlSchema Add(string targetNamespace, XmlSchema schema)
		{
			if (schema == null || schema.ErrorCount != 0)
			{
				return null;
			}
			if (this.PreprocessSchema(ref schema, targetNamespace))
			{
				this.AddSchemaToSet(schema);
				this.isCompiled = false;
				return schema;
			}
			return null;
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x000849CC File Offset: 0x000839CC
		internal void Add(string targetNamespace, XmlReader reader, Hashtable validatedNamespaces)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (targetNamespace == null)
			{
				targetNamespace = string.Empty;
			}
			if (validatedNamespaces[targetNamespace] != null)
			{
				if (this.FindSchemaByNSAndUrl(new Uri(reader.BaseURI, UriKind.RelativeOrAbsolute), targetNamespace, null) != null)
				{
					return;
				}
				throw new XmlSchemaException("Sch_ComponentAlreadySeenForNS", targetNamespace);
			}
			else
			{
				XmlSchema xmlSchema;
				if (this.IsSchemaLoaded(new Uri(reader.BaseURI, UriKind.RelativeOrAbsolute), targetNamespace, out xmlSchema))
				{
					return;
				}
				xmlSchema = this.ParseSchema(targetNamespace, reader);
				DictionaryEntry[] array = new DictionaryEntry[this.schemaLocations.Count];
				this.schemaLocations.CopyTo(array, 0);
				this.Add(targetNamespace, xmlSchema);
				if (xmlSchema.ImportedSchemas.Count > 0)
				{
					foreach (object obj in xmlSchema.ImportedSchemas)
					{
						XmlSchema xmlSchema2 = (XmlSchema)obj;
						string text = xmlSchema2.TargetNamespace;
						if (text == null)
						{
							text = string.Empty;
						}
						if (validatedNamespaces[text] != null && this.FindSchemaByNSAndUrl(xmlSchema2.BaseUri, text, array) == null)
						{
							this.RemoveRecursive(xmlSchema);
							throw new XmlSchemaException("Sch_ComponentAlreadySeenForNS", text);
						}
					}
				}
				return;
			}
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x00084AFC File Offset: 0x00083AFC
		internal XmlSchema FindSchemaByNSAndUrl(Uri schemaUri, string ns, DictionaryEntry[] locationsTable)
		{
			if (schemaUri == null || schemaUri.OriginalString.Length == 0)
			{
				return null;
			}
			XmlSchema xmlSchema = null;
			if (locationsTable == null)
			{
				xmlSchema = (XmlSchema)this.schemaLocations[schemaUri];
			}
			else
			{
				for (int i = 0; i < locationsTable.Length; i++)
				{
					if (schemaUri.Equals(locationsTable[i].Key))
					{
						xmlSchema = (XmlSchema)locationsTable[i].Value;
						break;
					}
				}
			}
			if (xmlSchema != null)
			{
				string text = ((xmlSchema.TargetNamespace == null) ? string.Empty : xmlSchema.TargetNamespace);
				if (text == ns)
				{
					return xmlSchema;
				}
				if (text == string.Empty)
				{
					ChameleonKey chameleonKey = new ChameleonKey(ns, schemaUri);
					xmlSchema = (XmlSchema)this.chameleonSchemas[chameleonKey];
				}
				else
				{
					xmlSchema = null;
				}
			}
			return xmlSchema;
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x00084BC0 File Offset: 0x00083BC0
		private void SetProhibitDtd(XmlReader reader)
		{
			if (reader.Settings != null)
			{
				this.readerSettings.ProhibitDtd = reader.Settings.ProhibitDtd;
				return;
			}
			XmlTextReader xmlTextReader = reader as XmlTextReader;
			if (xmlTextReader != null)
			{
				this.readerSettings.ProhibitDtd = xmlTextReader.ProhibitDtd;
			}
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x00084C08 File Offset: 0x00083C08
		private void AddSchemaToSet(XmlSchema schema)
		{
			this.schemas.Add(schema.SchemaId, schema);
			string text = this.GetTargetNamespace(schema);
			if (this.targetNamespaces[text] == null)
			{
				this.targetNamespaces.Add(text, text);
			}
			if (this.schemaForSchema == null && text == "http://www.w3.org/2001/XMLSchema" && schema.SchemaTypes[DatatypeImplementation.QnAnyType] != null)
			{
				this.schemaForSchema = schema;
			}
			foreach (object obj in schema.ImportedSchemas)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (!this.schemas.ContainsKey(xmlSchema.SchemaId))
				{
					this.schemas.Add(xmlSchema.SchemaId, xmlSchema);
				}
				text = this.GetTargetNamespace(xmlSchema);
				if (this.targetNamespaces[text] == null)
				{
					this.targetNamespaces.Add(text, text);
				}
				if (this.schemaForSchema == null && text == "http://www.w3.org/2001/XMLSchema" && schema.SchemaTypes[DatatypeImplementation.QnAnyType] != null)
				{
					this.schemaForSchema = schema;
				}
			}
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x00084D48 File Offset: 0x00083D48
		private void ProcessNewSubstitutionGroups(XmlSchemaObjectTable substitutionGroupsTable, bool resolve)
		{
			foreach (object obj in substitutionGroupsTable.Values)
			{
				XmlSchemaSubstitutionGroup xmlSchemaSubstitutionGroup = (XmlSchemaSubstitutionGroup)obj;
				if (resolve)
				{
					this.ResolveSubstitutionGroup(xmlSchemaSubstitutionGroup, substitutionGroupsTable);
				}
				XmlQualifiedName examplar = xmlSchemaSubstitutionGroup.Examplar;
				XmlSchemaSubstitutionGroup xmlSchemaSubstitutionGroup2 = (XmlSchemaSubstitutionGroup)this.substitutionGroups[examplar];
				if (xmlSchemaSubstitutionGroup2 != null)
				{
					using (IEnumerator enumerator2 = xmlSchemaSubstitutionGroup.Members.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj2;
							if (!xmlSchemaSubstitutionGroup2.Members.Contains(xmlSchemaElement))
							{
								xmlSchemaSubstitutionGroup2.Members.Add(xmlSchemaElement);
							}
						}
						continue;
					}
				}
				this.AddToTable(this.substitutionGroups, examplar, xmlSchemaSubstitutionGroup);
			}
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x00084E40 File Offset: 0x00083E40
		private void ResolveSubstitutionGroup(XmlSchemaSubstitutionGroup substitutionGroup, XmlSchemaObjectTable substTable)
		{
			ArrayList arrayList = null;
			XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)this.elements[substitutionGroup.Examplar];
			if (substitutionGroup.Members.Contains(xmlSchemaElement))
			{
				return;
			}
			foreach (object obj in substitutionGroup.Members)
			{
				XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)obj;
				XmlSchemaSubstitutionGroup xmlSchemaSubstitutionGroup = (XmlSchemaSubstitutionGroup)substTable[xmlSchemaElement2.QualifiedName];
				if (xmlSchemaSubstitutionGroup != null)
				{
					this.ResolveSubstitutionGroup(xmlSchemaSubstitutionGroup, substTable);
					foreach (object obj2 in xmlSchemaSubstitutionGroup.Members)
					{
						XmlSchemaElement xmlSchemaElement3 = (XmlSchemaElement)obj2;
						if (xmlSchemaElement3 != xmlSchemaElement2)
						{
							if (arrayList == null)
							{
								arrayList = new ArrayList();
							}
							arrayList.Add(xmlSchemaElement3);
						}
					}
				}
			}
			if (arrayList != null)
			{
				foreach (object obj3 in arrayList)
				{
					XmlSchemaElement xmlSchemaElement4 = (XmlSchemaElement)obj3;
					substitutionGroup.Members.Add(xmlSchemaElement4);
				}
			}
			substitutionGroup.Members.Add(xmlSchemaElement);
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x00084FA4 File Offset: 0x00083FA4
		internal XmlSchema Remove(XmlSchema schema, bool forceCompile)
		{
			if (schema == null)
			{
				throw new ArgumentNullException("schema");
			}
			lock (this.InternalSyncObject)
			{
				if (this.schemas.ContainsKey(schema.SchemaId))
				{
					this.schemas.Remove(schema.SchemaId);
					if (schema.BaseUri != null)
					{
						this.schemaLocations.Remove(schema.BaseUri);
					}
					string targetNamespace = this.GetTargetNamespace(schema);
					if (this.Schemas(targetNamespace).Count == 0)
					{
						this.targetNamespaces.Remove(targetNamespace);
					}
					if (forceCompile)
					{
						this.isCompiled = false;
						this.compileAll = true;
					}
					return schema;
				}
			}
			return null;
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x0008506C File Offset: 0x0008406C
		private void ClearTables()
		{
			this.GlobalElements.Clear();
			this.GlobalAttributes.Clear();
			this.GlobalTypes.Clear();
			this.SubstitutionGroups.Clear();
			this.TypeExtensions.Clear();
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x000850A8 File Offset: 0x000840A8
		internal bool PreprocessSchema(ref XmlSchema schema, string targetNamespace)
		{
			Preprocessor preprocessor = new Preprocessor(this.nameTable, this.GetSchemaNames(this.nameTable), this.eventHandler, this.compilationSettings);
			preprocessor.XmlResolver = this.readerSettings.GetXmlResolver_CheckConfig();
			preprocessor.ReaderSettings = this.readerSettings;
			preprocessor.SchemaLocations = this.schemaLocations;
			preprocessor.ChameleonSchemas = this.chameleonSchemas;
			bool flag = preprocessor.Execute(schema, targetNamespace, true);
			schema = preprocessor.RootSchema;
			return flag;
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x00085124 File Offset: 0x00084124
		internal XmlSchema ParseSchema(string targetNamespace, XmlReader reader)
		{
			XmlNameTable xmlNameTable = reader.NameTable;
			SchemaNames schemaNames = this.GetSchemaNames(xmlNameTable);
			Parser parser = new Parser(SchemaType.XSD, xmlNameTable, schemaNames, this.eventHandler);
			parser.XmlResolver = this.readerSettings.GetXmlResolver_CheckConfig();
			try
			{
				parser.Parse(reader, targetNamespace);
			}
			catch (XmlSchemaException ex)
			{
				this.SendValidationEvent(ex, XmlSeverityType.Error);
				return null;
			}
			return parser.XmlSchema;
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x00085194 File Offset: 0x00084194
		internal void CopyFromCompiledSet(XmlSchemaSet otherSet)
		{
			SortedList sortedSchemas = otherSet.SortedSchemas;
			bool flag = this.schemas.Count == 0;
			ArrayList arrayList = new ArrayList();
			SchemaInfo schemaInfo = new SchemaInfo();
			for (int i = 0; i < sortedSchemas.Count; i++)
			{
				XmlSchema xmlSchema = (XmlSchema)sortedSchemas.GetByIndex(i);
				Uri baseUri = xmlSchema.BaseUri;
				if (this.schemas.ContainsKey(xmlSchema.SchemaId) || (baseUri != null && baseUri.OriginalString.Length != 0 && this.schemaLocations[baseUri] != null))
				{
					arrayList.Add(xmlSchema);
				}
				else
				{
					this.schemas.Add(xmlSchema.SchemaId, xmlSchema);
					if (baseUri != null && baseUri.OriginalString.Length != 0)
					{
						this.schemaLocations.Add(baseUri, xmlSchema);
					}
					string targetNamespace = this.GetTargetNamespace(xmlSchema);
					if (this.targetNamespaces[targetNamespace] == null)
					{
						this.targetNamespaces.Add(targetNamespace, targetNamespace);
					}
				}
			}
			this.VerifyTables();
			foreach (object obj in otherSet.GlobalElements.Values)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj;
				if (!this.AddToTable(this.elements, xmlSchemaElement.QualifiedName, xmlSchemaElement))
				{
					goto IL_026E;
				}
			}
			foreach (object obj2 in otherSet.GlobalAttributes.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj2;
				if (!this.AddToTable(this.attributes, xmlSchemaAttribute.QualifiedName, xmlSchemaAttribute))
				{
					goto IL_026E;
				}
			}
			foreach (object obj3 in otherSet.GlobalTypes.Values)
			{
				XmlSchemaType xmlSchemaType = (XmlSchemaType)obj3;
				if (!this.AddToTable(this.schemaTypes, xmlSchemaType.QualifiedName, xmlSchemaType))
				{
					goto IL_026E;
				}
			}
			this.ProcessNewSubstitutionGroups(otherSet.SubstitutionGroups, false);
			schemaInfo.Add(this.cachedCompiledInfo, this.eventHandler);
			schemaInfo.Add(otherSet.CompiledInfo, this.eventHandler);
			this.cachedCompiledInfo = schemaInfo;
			if (flag)
			{
				this.isCompiled = true;
				this.compileAll = false;
			}
			return;
			IL_026E:
			foreach (object obj4 in sortedSchemas.Values)
			{
				XmlSchema xmlSchema2 = (XmlSchema)obj4;
				if (!arrayList.Contains(xmlSchema2))
				{
					this.Remove(xmlSchema2, false);
				}
			}
			foreach (object obj5 in otherSet.GlobalElements.Values)
			{
				XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)obj5;
				if (!arrayList.Contains((XmlSchema)xmlSchemaElement2.Parent))
				{
					this.elements.Remove(xmlSchemaElement2.QualifiedName);
				}
			}
			foreach (object obj6 in otherSet.GlobalAttributes.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute2 = (XmlSchemaAttribute)obj6;
				if (!arrayList.Contains((XmlSchema)xmlSchemaAttribute2.Parent))
				{
					this.attributes.Remove(xmlSchemaAttribute2.QualifiedName);
				}
			}
			foreach (object obj7 in otherSet.GlobalTypes.Values)
			{
				XmlSchemaType xmlSchemaType2 = (XmlSchemaType)obj7;
				if (!arrayList.Contains((XmlSchema)xmlSchemaType2.Parent))
				{
					this.schemaTypes.Remove(xmlSchemaType2.QualifiedName);
				}
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06001D1A RID: 7450 RVA: 0x000855F0 File Offset: 0x000845F0
		internal SchemaInfo CompiledInfo
		{
			get
			{
				return this.cachedCompiledInfo;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06001D1B RID: 7451 RVA: 0x000855F8 File Offset: 0x000845F8
		internal XmlReaderSettings ReaderSettings
		{
			get
			{
				return this.readerSettings;
			}
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x00085600 File Offset: 0x00084600
		internal XmlResolver GetResolver()
		{
			return this.readerSettings.GetXmlResolver_CheckConfig();
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x0008560D File Offset: 0x0008460D
		internal ValidationEventHandler GetEventHandler()
		{
			return this.eventHandler;
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x00085615 File Offset: 0x00084615
		internal SchemaNames GetSchemaNames(XmlNameTable nt)
		{
			if (this.nameTable != nt)
			{
				return new SchemaNames(nt);
			}
			if (this.schemaNames == null)
			{
				this.schemaNames = new SchemaNames(this.nameTable);
			}
			return this.schemaNames;
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x00085648 File Offset: 0x00084648
		internal bool IsSchemaLoaded(Uri schemaUri, string targetNamespace, out XmlSchema schema)
		{
			schema = null;
			if (targetNamespace == null)
			{
				targetNamespace = string.Empty;
			}
			if (this.GetSchemaByUri(schemaUri, out schema))
			{
				if (!this.schemas.ContainsKey(schema.SchemaId) || (targetNamespace.Length != 0 && !(targetNamespace == schema.TargetNamespace)))
				{
					if (schema.TargetNamespace == null)
					{
						XmlSchema xmlSchema = this.FindSchemaByNSAndUrl(schemaUri, targetNamespace, null);
						if (xmlSchema != null && this.schemas.ContainsKey(xmlSchema.SchemaId))
						{
							schema = xmlSchema;
						}
						else
						{
							schema = this.Add(targetNamespace, schema);
						}
					}
					else if (targetNamespace.Length != 0 && targetNamespace != schema.TargetNamespace)
					{
						this.SendValidationEvent(new XmlSchemaException("Sch_MismatchTargetNamespaceEx", new string[] { targetNamespace, schema.TargetNamespace }), XmlSeverityType.Error);
						schema = null;
					}
					else
					{
						this.AddSchemaToSet(schema);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x00085733 File Offset: 0x00084733
		internal bool GetSchemaByUri(Uri schemaUri, out XmlSchema schema)
		{
			schema = null;
			if (schemaUri == null || schemaUri.OriginalString.Length == 0)
			{
				return false;
			}
			schema = (XmlSchema)this.schemaLocations[schemaUri];
			return schema != null;
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x0008576A File Offset: 0x0008476A
		internal string GetTargetNamespace(XmlSchema schema)
		{
			if (schema.TargetNamespace != null)
			{
				return schema.TargetNamespace;
			}
			return string.Empty;
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06001D22 RID: 7458 RVA: 0x00085780 File Offset: 0x00084780
		internal SortedList SortedSchemas
		{
			get
			{
				return this.schemas;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06001D23 RID: 7459 RVA: 0x00085788 File Offset: 0x00084788
		internal bool CompileAll
		{
			get
			{
				return this.compileAll;
			}
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x00085790 File Offset: 0x00084790
		private void RemoveSchemaFromCaches(XmlSchema schema)
		{
			List<XmlSchema> list = new List<XmlSchema>();
			schema.GetExternalSchemasList(list, schema);
			foreach (XmlSchema xmlSchema in list)
			{
				if (xmlSchema.BaseUri != null && xmlSchema.BaseUri.OriginalString.Length != 0)
				{
					this.schemaLocations.Remove(xmlSchema.BaseUri);
				}
				ICollection keys = this.chameleonSchemas.Keys;
				ArrayList arrayList = new ArrayList();
				foreach (object obj in keys)
				{
					ChameleonKey chameleonKey = (ChameleonKey)obj;
					if (chameleonKey.chameleonLocation.Equals(xmlSchema.BaseUri))
					{
						arrayList.Add(chameleonKey);
					}
				}
				foreach (object obj2 in arrayList)
				{
					ChameleonKey chameleonKey2 = (ChameleonKey)obj2;
					this.chameleonSchemas.Remove(chameleonKey2);
				}
			}
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x000858E4 File Offset: 0x000848E4
		private void RemoveSchemaFromGlobalTables(XmlSchema schema)
		{
			if (this.schemas.Count == 0)
			{
				return;
			}
			this.VerifyTables();
			foreach (object obj in schema.Elements.Values)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj;
				XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)this.elements[xmlSchemaElement.QualifiedName];
				if (xmlSchemaElement2 == xmlSchemaElement)
				{
					this.elements.Remove(xmlSchemaElement.QualifiedName);
				}
			}
			foreach (object obj2 in schema.Attributes.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj2;
				XmlSchemaAttribute xmlSchemaAttribute2 = (XmlSchemaAttribute)this.attributes[xmlSchemaAttribute.QualifiedName];
				if (xmlSchemaAttribute2 == xmlSchemaAttribute)
				{
					this.attributes.Remove(xmlSchemaAttribute.QualifiedName);
				}
			}
			foreach (object obj3 in schema.SchemaTypes.Values)
			{
				XmlSchemaType xmlSchemaType = (XmlSchemaType)obj3;
				XmlSchemaType xmlSchemaType2 = (XmlSchemaType)this.schemaTypes[xmlSchemaType.QualifiedName];
				if (xmlSchemaType2 == xmlSchemaType)
				{
					this.schemaTypes.Remove(xmlSchemaType.QualifiedName);
				}
			}
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x00085A7C File Offset: 0x00084A7C
		private bool AddToTable(XmlSchemaObjectTable table, XmlQualifiedName qname, XmlSchemaObject item)
		{
			if (qname.Name.Length == 0)
			{
				return true;
			}
			XmlSchemaObject xmlSchemaObject = table[qname];
			if (xmlSchemaObject == null)
			{
				table.Add(qname, item);
				return true;
			}
			if (xmlSchemaObject == item || xmlSchemaObject.SourceUri == item.SourceUri)
			{
				return true;
			}
			string text = string.Empty;
			if (item is XmlSchemaComplexType)
			{
				text = "Sch_DupComplexType";
			}
			else if (item is XmlSchemaSimpleType)
			{
				text = "Sch_DupSimpleType";
			}
			else if (item is XmlSchemaElement)
			{
				text = "Sch_DupGlobalElement";
			}
			else if (item is XmlSchemaAttribute)
			{
				if (qname.Namespace == "http://www.w3.org/XML/1998/namespace")
				{
					XmlSchema buildInSchema = Preprocessor.GetBuildInSchema();
					XmlSchemaObject xmlSchemaObject2 = buildInSchema.Attributes[qname];
					if (xmlSchemaObject == xmlSchemaObject2)
					{
						table.Insert(qname, item);
						return true;
					}
					if (item == xmlSchemaObject2)
					{
						return true;
					}
				}
				text = "Sch_DupGlobalAttribute";
			}
			this.SendValidationEvent(new XmlSchemaException(text, qname.ToString()), XmlSeverityType.Error);
			return false;
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x00085B5C File Offset: 0x00084B5C
		private void VerifyTables()
		{
			if (this.elements == null)
			{
				this.elements = new XmlSchemaObjectTable();
			}
			if (this.attributes == null)
			{
				this.attributes = new XmlSchemaObjectTable();
			}
			if (this.schemaTypes == null)
			{
				this.schemaTypes = new XmlSchemaObjectTable();
			}
			if (this.substitutionGroups == null)
			{
				this.substitutionGroups = new XmlSchemaObjectTable();
			}
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x00085BB5 File Offset: 0x00084BB5
		private void InternalValidationCallback(object sender, ValidationEventArgs e)
		{
			if (e.Severity == XmlSeverityType.Error)
			{
				throw e.Exception;
			}
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x00085BC6 File Offset: 0x00084BC6
		private void SendValidationEvent(XmlSchemaException e, XmlSeverityType severity)
		{
			if (this.eventHandler != null)
			{
				this.eventHandler(this, new ValidationEventArgs(e, severity));
				return;
			}
			throw e;
		}

		// Token: 0x040011BA RID: 4538
		private XmlNameTable nameTable;

		// Token: 0x040011BB RID: 4539
		private SchemaNames schemaNames;

		// Token: 0x040011BC RID: 4540
		private SortedList schemas;

		// Token: 0x040011BD RID: 4541
		private ValidationEventHandler internalEventHandler;

		// Token: 0x040011BE RID: 4542
		private ValidationEventHandler eventHandler;

		// Token: 0x040011BF RID: 4543
		private bool isCompiled;

		// Token: 0x040011C0 RID: 4544
		private Hashtable schemaLocations;

		// Token: 0x040011C1 RID: 4545
		private Hashtable chameleonSchemas;

		// Token: 0x040011C2 RID: 4546
		private Hashtable targetNamespaces;

		// Token: 0x040011C3 RID: 4547
		private bool compileAll;

		// Token: 0x040011C4 RID: 4548
		private SchemaInfo cachedCompiledInfo;

		// Token: 0x040011C5 RID: 4549
		private XmlReaderSettings readerSettings;

		// Token: 0x040011C6 RID: 4550
		private XmlSchema schemaForSchema;

		// Token: 0x040011C7 RID: 4551
		private XmlSchemaCompilationSettings compilationSettings;

		// Token: 0x040011C8 RID: 4552
		internal XmlSchemaObjectTable elements;

		// Token: 0x040011C9 RID: 4553
		internal XmlSchemaObjectTable attributes;

		// Token: 0x040011CA RID: 4554
		internal XmlSchemaObjectTable schemaTypes;

		// Token: 0x040011CB RID: 4555
		internal XmlSchemaObjectTable substitutionGroups;

		// Token: 0x040011CC RID: 4556
		private XmlSchemaObjectTable typeExtensions;

		// Token: 0x040011CD RID: 4557
		private object internalSyncObject;
	}
}
