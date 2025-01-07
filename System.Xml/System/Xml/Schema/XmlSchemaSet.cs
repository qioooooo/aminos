using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace System.Xml.Schema
{
	public class XmlSchemaSet
	{
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

		public XmlSchemaSet()
			: this(new NameTable())
		{
		}

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

		public XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

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

		public bool IsCompiled
		{
			get
			{
				return this.isCompiled;
			}
		}

		public XmlResolver XmlResolver
		{
			set
			{
				this.readerSettings.XmlResolver = value;
			}
		}

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

		public int Count
		{
			get
			{
				return this.schemas.Count;
			}
		}

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

		internal Hashtable SchemaLocations
		{
			get
			{
				return this.schemaLocations;
			}
		}

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

		public XmlSchema Remove(XmlSchema schema)
		{
			return this.Remove(schema, true);
		}

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

		public bool Contains(string targetNamespace)
		{
			if (targetNamespace == null)
			{
				targetNamespace = string.Empty;
			}
			return this.targetNamespaces[targetNamespace] != null;
		}

		public bool Contains(XmlSchema schema)
		{
			if (schema == null)
			{
				throw new ArgumentNullException("schema");
			}
			return this.schemas.ContainsValue(schema);
		}

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

		public ICollection Schemas()
		{
			return this.schemas.Values;
		}

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

		private void ClearTables()
		{
			this.GlobalElements.Clear();
			this.GlobalAttributes.Clear();
			this.GlobalTypes.Clear();
			this.SubstitutionGroups.Clear();
			this.TypeExtensions.Clear();
		}

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

		internal SchemaInfo CompiledInfo
		{
			get
			{
				return this.cachedCompiledInfo;
			}
		}

		internal XmlReaderSettings ReaderSettings
		{
			get
			{
				return this.readerSettings;
			}
		}

		internal XmlResolver GetResolver()
		{
			return this.readerSettings.GetXmlResolver_CheckConfig();
		}

		internal ValidationEventHandler GetEventHandler()
		{
			return this.eventHandler;
		}

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

		internal string GetTargetNamespace(XmlSchema schema)
		{
			if (schema.TargetNamespace != null)
			{
				return schema.TargetNamespace;
			}
			return string.Empty;
		}

		internal SortedList SortedSchemas
		{
			get
			{
				return this.schemas;
			}
		}

		internal bool CompileAll
		{
			get
			{
				return this.compileAll;
			}
		}

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

		private void InternalValidationCallback(object sender, ValidationEventArgs e)
		{
			if (e.Severity == XmlSeverityType.Error)
			{
				throw e.Exception;
			}
		}

		private void SendValidationEvent(XmlSchemaException e, XmlSeverityType severity)
		{
			if (this.eventHandler != null)
			{
				this.eventHandler(this, new ValidationEventArgs(e, severity));
				return;
			}
			throw e;
		}

		private XmlNameTable nameTable;

		private SchemaNames schemaNames;

		private SortedList schemas;

		private ValidationEventHandler internalEventHandler;

		private ValidationEventHandler eventHandler;

		private bool isCompiled;

		private Hashtable schemaLocations;

		private Hashtable chameleonSchemas;

		private Hashtable targetNamespaces;

		private bool compileAll;

		private SchemaInfo cachedCompiledInfo;

		private XmlReaderSettings readerSettings;

		private XmlSchema schemaForSchema;

		private XmlSchemaCompilationSettings compilationSettings;

		internal XmlSchemaObjectTable elements;

		internal XmlSchemaObjectTable attributes;

		internal XmlSchemaObjectTable schemaTypes;

		internal XmlSchemaObjectTable substitutionGroups;

		private XmlSchemaObjectTable typeExtensions;

		private object internalSyncObject;
	}
}
