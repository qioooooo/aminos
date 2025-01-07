using System;
using System.Collections;
using System.IO;

namespace System.Xml.Schema
{
	internal sealed class SchemaCollectionPreprocessor : BaseProcessor
	{
		public SchemaCollectionPreprocessor(XmlNameTable nameTable, SchemaNames schemaNames, ValidationEventHandler eventHandler)
			: base(nameTable, schemaNames, eventHandler)
		{
		}

		public bool Execute(XmlSchema schema, string targetNamespace, bool loadExternals, XmlSchemaCollection xsc)
		{
			this.schema = schema;
			this.Xmlns = base.NameTable.Add("xmlns");
			this.Cleanup(schema);
			if (loadExternals && this.xmlResolver != null)
			{
				this.schemaLocations = new Hashtable();
				if (schema.BaseUri != null)
				{
					this.schemaLocations.Add(schema.BaseUri, schema.BaseUri);
				}
				this.LoadExternals(schema, xsc);
			}
			this.ValidateIdAttribute(schema);
			this.Preprocess(schema, targetNamespace, SchemaCollectionPreprocessor.Compositor.Root);
			if (!base.HasErrors)
			{
				schema.IsPreprocessed = true;
				foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
				{
					XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
					if (xmlSchemaExternal.Schema != null)
					{
						xmlSchemaExternal.Schema.IsPreprocessed = true;
					}
				}
			}
			return !base.HasErrors;
		}

		private void Cleanup(XmlSchema schema)
		{
			if (schema.IsProcessing)
			{
				return;
			}
			schema.IsProcessing = true;
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				if (xmlSchemaExternal.Schema != null)
				{
					this.Cleanup(xmlSchemaExternal.Schema);
				}
				if (xmlSchemaExternal is XmlSchemaRedefine)
				{
					XmlSchemaRedefine xmlSchemaRedefine = xmlSchemaExternal as XmlSchemaRedefine;
					xmlSchemaRedefine.AttributeGroups.Clear();
					xmlSchemaRedefine.Groups.Clear();
					xmlSchemaRedefine.SchemaTypes.Clear();
				}
			}
			schema.Attributes.Clear();
			schema.AttributeGroups.Clear();
			schema.SchemaTypes.Clear();
			schema.Elements.Clear();
			schema.Groups.Clear();
			schema.Notations.Clear();
			schema.Ids.Clear();
			schema.IdentityConstraints.Clear();
			schema.IsProcessing = false;
		}

		internal XmlResolver XmlResolver
		{
			set
			{
				this.xmlResolver = value;
			}
		}

		private void LoadExternals(XmlSchema schema, XmlSchemaCollection xsc)
		{
			if (schema.IsProcessing)
			{
				return;
			}
			schema.IsProcessing = true;
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				if (xmlSchemaExternal.Schema != null)
				{
					if (xmlSchemaExternal is XmlSchemaImport && ((XmlSchemaImport)xmlSchemaExternal).Namespace == "http://www.w3.org/XML/1998/namespace")
					{
						this.buildinIncluded = true;
					}
					else
					{
						Uri baseUri = xmlSchemaExternal.BaseUri;
						if (baseUri != null && this.schemaLocations[baseUri] == null)
						{
							this.schemaLocations.Add(baseUri, baseUri);
						}
						this.LoadExternals(xmlSchemaExternal.Schema, xsc);
					}
				}
				else
				{
					if (xsc != null && xmlSchemaExternal is XmlSchemaImport)
					{
						XmlSchemaImport xmlSchemaImport = (XmlSchemaImport)xmlSchemaExternal;
						string text = ((xmlSchemaImport.Namespace != null) ? xmlSchemaImport.Namespace : string.Empty);
						xmlSchemaExternal.Schema = xsc[text];
						if (xmlSchemaExternal.Schema != null)
						{
							xmlSchemaExternal.Schema = xmlSchemaExternal.Schema.Clone();
							if (xmlSchemaExternal.Schema.BaseUri != null && this.schemaLocations[xmlSchemaExternal.Schema.BaseUri] == null)
							{
								this.schemaLocations.Add(xmlSchemaExternal.Schema.BaseUri, xmlSchemaExternal.Schema.BaseUri);
							}
							using (XmlSchemaObjectEnumerator enumerator2 = xmlSchemaExternal.Schema.Includes.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									XmlSchemaObject xmlSchemaObject2 = enumerator2.Current;
									XmlSchemaExternal xmlSchemaExternal2 = (XmlSchemaExternal)xmlSchemaObject2;
									if (xmlSchemaExternal2 is XmlSchemaImport)
									{
										XmlSchemaImport xmlSchemaImport2 = (XmlSchemaImport)xmlSchemaExternal2;
										Uri uri = ((xmlSchemaImport2.BaseUri != null) ? xmlSchemaImport2.BaseUri : ((xmlSchemaImport2.Schema != null && xmlSchemaImport2.Schema.BaseUri != null) ? xmlSchemaImport2.Schema.BaseUri : null));
										if (uri != null)
										{
											if (this.schemaLocations[uri] != null)
											{
												xmlSchemaImport2.Schema = null;
											}
											else
											{
												this.schemaLocations.Add(uri, uri);
											}
										}
									}
								}
								continue;
							}
						}
					}
					if (xmlSchemaExternal is XmlSchemaImport && ((XmlSchemaImport)xmlSchemaExternal).Namespace == "http://www.w3.org/XML/1998/namespace")
					{
						if (!this.buildinIncluded)
						{
							this.buildinIncluded = true;
							xmlSchemaExternal.Schema = Preprocessor.GetBuildInSchema();
						}
					}
					else
					{
						string schemaLocation = xmlSchemaExternal.SchemaLocation;
						if (schemaLocation != null)
						{
							Uri uri2 = this.ResolveSchemaLocationUri(schema, schemaLocation);
							if (uri2 != null && this.schemaLocations[uri2] == null)
							{
								Stream schemaEntity = this.GetSchemaEntity(uri2);
								if (schemaEntity != null)
								{
									xmlSchemaExternal.BaseUri = uri2;
									this.schemaLocations.Add(uri2, uri2);
									XmlTextReader xmlTextReader = new XmlTextReader(uri2.ToString(), schemaEntity, base.NameTable);
									xmlTextReader.XmlResolver = this.xmlResolver;
									try
									{
										try
										{
											Parser parser = new Parser(SchemaType.XSD, base.NameTable, base.SchemaNames, base.EventHandler);
											parser.Parse(xmlTextReader, null);
											while (xmlTextReader.Read())
											{
											}
											xmlSchemaExternal.Schema = parser.XmlSchema;
											this.LoadExternals(xmlSchemaExternal.Schema, xsc);
										}
										catch (XmlSchemaException ex)
										{
											base.SendValidationEventNoThrow(new XmlSchemaException("Sch_CannotLoadSchema", new string[] { schemaLocation, ex.Message }, ex.SourceUri, ex.LineNumber, ex.LinePosition), XmlSeverityType.Error);
										}
										catch (Exception)
										{
											base.SendValidationEvent("Sch_InvalidIncludeLocation", xmlSchemaExternal, XmlSeverityType.Warning);
										}
										continue;
									}
									finally
									{
										xmlTextReader.Close();
									}
								}
								base.SendValidationEvent("Sch_InvalidIncludeLocation", xmlSchemaExternal, XmlSeverityType.Warning);
							}
						}
					}
				}
			}
			schema.IsProcessing = false;
		}

		private void BuildRefNamespaces(XmlSchema schema)
		{
			this.referenceNamespaces = new Hashtable();
			this.referenceNamespaces.Add("http://www.w3.org/2001/XMLSchema", "http://www.w3.org/2001/XMLSchema");
			this.referenceNamespaces.Add(string.Empty, string.Empty);
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				if (xmlSchemaExternal is XmlSchemaImport)
				{
					XmlSchemaImport xmlSchemaImport = xmlSchemaExternal as XmlSchemaImport;
					string @namespace = xmlSchemaImport.Namespace;
					if (@namespace != null && this.referenceNamespaces[@namespace] == null)
					{
						this.referenceNamespaces.Add(@namespace, @namespace);
					}
				}
			}
			if (schema.TargetNamespace != null && this.referenceNamespaces[schema.TargetNamespace] == null)
			{
				this.referenceNamespaces.Add(schema.TargetNamespace, schema.TargetNamespace);
			}
		}

		private void Preprocess(XmlSchema schema, string targetNamespace, SchemaCollectionPreprocessor.Compositor compositor)
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
					try
					{
						XmlConvert.ToUri(text);
					}
					catch
					{
						base.SendValidationEvent("Sch_InvalidNamespace", schema.TargetNamespace, schema);
					}
				}
			}
			if (schema.Version != null)
			{
				try
				{
					XmlConvert.VerifyTOKEN(schema.Version);
				}
				catch (Exception)
				{
					base.SendValidationEvent("Sch_AttributeValueDataType", "version", schema);
				}
			}
			switch (compositor)
			{
			case SchemaCollectionPreprocessor.Compositor.Root:
				if (targetNamespace == null && schema.TargetNamespace != null)
				{
					targetNamespace = schema.TargetNamespace;
				}
				else if (schema.TargetNamespace == null && targetNamespace != null && targetNamespace.Length == 0)
				{
					targetNamespace = null;
				}
				if (targetNamespace != schema.TargetNamespace)
				{
					base.SendValidationEvent("Sch_MismatchTargetNamespaceEx", targetNamespace, schema.TargetNamespace, schema);
				}
				break;
			case SchemaCollectionPreprocessor.Compositor.Include:
				if (schema.TargetNamespace != null && targetNamespace != schema.TargetNamespace)
				{
					base.SendValidationEvent("Sch_MismatchTargetNamespaceInclude", targetNamespace, schema.TargetNamespace, schema);
				}
				break;
			case SchemaCollectionPreprocessor.Compositor.Import:
				if (targetNamespace != schema.TargetNamespace)
				{
					base.SendValidationEvent("Sch_MismatchTargetNamespaceImport", targetNamespace, schema.TargetNamespace, schema);
				}
				break;
			}
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				this.SetParent(xmlSchemaExternal, schema);
				this.PreprocessAnnotation(xmlSchemaExternal);
				string schemaLocation = xmlSchemaExternal.SchemaLocation;
				if (schemaLocation != null)
				{
					try
					{
						XmlConvert.ToUri(schemaLocation);
						goto IL_01BB;
					}
					catch
					{
						base.SendValidationEvent("Sch_InvalidSchemaLocation", schemaLocation, xmlSchemaExternal);
						goto IL_01BB;
					}
					goto IL_0192;
				}
				goto IL_0192;
				IL_01BB:
				if (xmlSchemaExternal.Schema != null)
				{
					if (xmlSchemaExternal is XmlSchemaRedefine)
					{
						this.Preprocess(xmlSchemaExternal.Schema, schema.TargetNamespace, SchemaCollectionPreprocessor.Compositor.Include);
						continue;
					}
					if (xmlSchemaExternal is XmlSchemaImport)
					{
						if (((XmlSchemaImport)xmlSchemaExternal).Namespace == null && schema.TargetNamespace == null)
						{
							base.SendValidationEvent("Sch_ImportTargetNamespaceNull", xmlSchemaExternal);
						}
						else if (((XmlSchemaImport)xmlSchemaExternal).Namespace == schema.TargetNamespace)
						{
							base.SendValidationEvent("Sch_ImportTargetNamespace", xmlSchemaExternal);
						}
						this.Preprocess(xmlSchemaExternal.Schema, ((XmlSchemaImport)xmlSchemaExternal).Namespace, SchemaCollectionPreprocessor.Compositor.Import);
						continue;
					}
					this.Preprocess(xmlSchemaExternal.Schema, schema.TargetNamespace, SchemaCollectionPreprocessor.Compositor.Include);
					continue;
				}
				else
				{
					if (!(xmlSchemaExternal is XmlSchemaImport))
					{
						continue;
					}
					string @namespace = ((XmlSchemaImport)xmlSchemaExternal).Namespace;
					if (@namespace == null)
					{
						continue;
					}
					if (@namespace.Length == 0)
					{
						base.SendValidationEvent("Sch_InvalidNamespaceAttribute", @namespace, xmlSchemaExternal);
						continue;
					}
					try
					{
						XmlConvert.ToUri(@namespace);
					}
					catch (FormatException)
					{
						base.SendValidationEvent("Sch_InvalidNamespace", @namespace, xmlSchemaExternal);
					}
					continue;
				}
				IL_0192:
				if ((xmlSchemaExternal is XmlSchemaRedefine || xmlSchemaExternal is XmlSchemaInclude) && xmlSchemaExternal.Schema == null)
				{
					base.SendValidationEvent("Sch_MissRequiredAttribute", "schemaLocation", xmlSchemaExternal);
					goto IL_01BB;
				}
				goto IL_01BB;
			}
			this.BuildRefNamespaces(schema);
			this.targetNamespace = ((targetNamespace == null) ? string.Empty : targetNamespace);
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
			foreach (XmlSchemaObject xmlSchemaObject2 in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal2 = (XmlSchemaExternal)xmlSchemaObject2;
				if (xmlSchemaExternal2 is XmlSchemaRedefine)
				{
					XmlSchemaRedefine xmlSchemaRedefine = (XmlSchemaRedefine)xmlSchemaExternal2;
					if (xmlSchemaExternal2.Schema != null)
					{
						this.PreprocessRedefine(xmlSchemaRedefine);
					}
					else
					{
						foreach (XmlSchemaObject xmlSchemaObject3 in xmlSchemaRedefine.Items)
						{
							if (!(xmlSchemaObject3 is XmlSchemaAnnotation))
							{
								base.SendValidationEvent("Sch_RedefineNoSchema", xmlSchemaRedefine);
								break;
							}
						}
					}
				}
				XmlSchema xmlSchema = xmlSchemaExternal2.Schema;
				if (xmlSchema != null)
				{
					foreach (object obj in xmlSchema.Elements.Values)
					{
						XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj;
						base.AddToTable(schema.Elements, xmlSchemaElement.QualifiedName, xmlSchemaElement);
					}
					foreach (object obj2 in xmlSchema.Attributes.Values)
					{
						XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj2;
						base.AddToTable(schema.Attributes, xmlSchemaAttribute.QualifiedName, xmlSchemaAttribute);
					}
					foreach (object obj3 in xmlSchema.Groups.Values)
					{
						XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup)obj3;
						base.AddToTable(schema.Groups, xmlSchemaGroup.QualifiedName, xmlSchemaGroup);
					}
					foreach (object obj4 in xmlSchema.AttributeGroups.Values)
					{
						XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)obj4;
						base.AddToTable(schema.AttributeGroups, xmlSchemaAttributeGroup.QualifiedName, xmlSchemaAttributeGroup);
					}
					foreach (object obj5 in xmlSchema.SchemaTypes.Values)
					{
						XmlSchemaType xmlSchemaType = (XmlSchemaType)obj5;
						base.AddToTable(schema.SchemaTypes, xmlSchemaType.QualifiedName, xmlSchemaType);
					}
					foreach (object obj6 in xmlSchema.Notations.Values)
					{
						XmlSchemaNotation xmlSchemaNotation = (XmlSchemaNotation)obj6;
						base.AddToTable(schema.Notations, xmlSchemaNotation.QualifiedName, xmlSchemaNotation);
					}
				}
				this.ValidateIdAttribute(xmlSchemaExternal2);
			}
			ArrayList arrayList = new ArrayList();
			foreach (XmlSchemaObject xmlSchemaObject4 in schema.Items)
			{
				this.SetParent(xmlSchemaObject4, schema);
				if (xmlSchemaObject4 is XmlSchemaAttribute)
				{
					XmlSchemaAttribute xmlSchemaAttribute2 = (XmlSchemaAttribute)xmlSchemaObject4;
					this.PreprocessAttribute(xmlSchemaAttribute2);
					base.AddToTable(schema.Attributes, xmlSchemaAttribute2.QualifiedName, xmlSchemaAttribute2);
				}
				else if (xmlSchemaObject4 is XmlSchemaAttributeGroup)
				{
					XmlSchemaAttributeGroup xmlSchemaAttributeGroup2 = (XmlSchemaAttributeGroup)xmlSchemaObject4;
					this.PreprocessAttributeGroup(xmlSchemaAttributeGroup2);
					base.AddToTable(schema.AttributeGroups, xmlSchemaAttributeGroup2.QualifiedName, xmlSchemaAttributeGroup2);
				}
				else if (xmlSchemaObject4 is XmlSchemaComplexType)
				{
					XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)xmlSchemaObject4;
					this.PreprocessComplexType(xmlSchemaComplexType, false);
					base.AddToTable(schema.SchemaTypes, xmlSchemaComplexType.QualifiedName, xmlSchemaComplexType);
				}
				else if (xmlSchemaObject4 is XmlSchemaSimpleType)
				{
					XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)xmlSchemaObject4;
					this.PreprocessSimpleType(xmlSchemaSimpleType, false);
					base.AddToTable(schema.SchemaTypes, xmlSchemaSimpleType.QualifiedName, xmlSchemaSimpleType);
				}
				else if (xmlSchemaObject4 is XmlSchemaElement)
				{
					XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)xmlSchemaObject4;
					this.PreprocessElement(xmlSchemaElement2);
					base.AddToTable(schema.Elements, xmlSchemaElement2.QualifiedName, xmlSchemaElement2);
				}
				else if (xmlSchemaObject4 is XmlSchemaGroup)
				{
					XmlSchemaGroup xmlSchemaGroup2 = (XmlSchemaGroup)xmlSchemaObject4;
					this.PreprocessGroup(xmlSchemaGroup2);
					base.AddToTable(schema.Groups, xmlSchemaGroup2.QualifiedName, xmlSchemaGroup2);
				}
				else if (xmlSchemaObject4 is XmlSchemaNotation)
				{
					XmlSchemaNotation xmlSchemaNotation2 = (XmlSchemaNotation)xmlSchemaObject4;
					this.PreprocessNotation(xmlSchemaNotation2);
					base.AddToTable(schema.Notations, xmlSchemaNotation2.QualifiedName, xmlSchemaNotation2);
				}
				else if (!(xmlSchemaObject4 is XmlSchemaAnnotation))
				{
					base.SendValidationEvent("Sch_InvalidCollection", xmlSchemaObject4);
					arrayList.Add(xmlSchemaObject4);
				}
			}
			foreach (object obj7 in arrayList)
			{
				XmlSchemaObject xmlSchemaObject5 = (XmlSchemaObject)obj7;
				schema.Items.Remove(xmlSchemaObject5);
			}
			schema.IsProcessing = false;
		}

		private void PreprocessRedefine(XmlSchemaRedefine redefine)
		{
			foreach (XmlSchemaObject xmlSchemaObject in redefine.Items)
			{
				this.SetParent(xmlSchemaObject, redefine);
				if (xmlSchemaObject is XmlSchemaGroup)
				{
					XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup)xmlSchemaObject;
					this.PreprocessGroup(xmlSchemaGroup);
					if (redefine.Groups[xmlSchemaGroup.QualifiedName] != null)
					{
						base.SendValidationEvent("Sch_GroupDoubleRedefine", xmlSchemaGroup);
					}
					else
					{
						base.AddToTable(redefine.Groups, xmlSchemaGroup.QualifiedName, xmlSchemaGroup);
						xmlSchemaGroup.Redefined = (XmlSchemaGroup)redefine.Schema.Groups[xmlSchemaGroup.QualifiedName];
						if (xmlSchemaGroup.Redefined != null)
						{
							this.CheckRefinedGroup(xmlSchemaGroup);
						}
						else
						{
							base.SendValidationEvent("Sch_GroupRedefineNotFound", xmlSchemaGroup);
						}
					}
				}
				else if (xmlSchemaObject is XmlSchemaAttributeGroup)
				{
					XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)xmlSchemaObject;
					this.PreprocessAttributeGroup(xmlSchemaAttributeGroup);
					if (redefine.AttributeGroups[xmlSchemaAttributeGroup.QualifiedName] != null)
					{
						base.SendValidationEvent("Sch_AttrGroupDoubleRedefine", xmlSchemaAttributeGroup);
					}
					else
					{
						base.AddToTable(redefine.AttributeGroups, xmlSchemaAttributeGroup.QualifiedName, xmlSchemaAttributeGroup);
						xmlSchemaAttributeGroup.Redefined = (XmlSchemaAttributeGroup)redefine.Schema.AttributeGroups[xmlSchemaAttributeGroup.QualifiedName];
						if (xmlSchemaAttributeGroup.Redefined != null)
						{
							this.CheckRefinedAttributeGroup(xmlSchemaAttributeGroup);
						}
						else
						{
							base.SendValidationEvent("Sch_AttrGroupRedefineNotFound", xmlSchemaAttributeGroup);
						}
					}
				}
				else if (xmlSchemaObject is XmlSchemaComplexType)
				{
					XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)xmlSchemaObject;
					this.PreprocessComplexType(xmlSchemaComplexType, false);
					if (redefine.SchemaTypes[xmlSchemaComplexType.QualifiedName] != null)
					{
						base.SendValidationEvent("Sch_ComplexTypeDoubleRedefine", xmlSchemaComplexType);
					}
					else
					{
						base.AddToTable(redefine.SchemaTypes, xmlSchemaComplexType.QualifiedName, xmlSchemaComplexType);
						XmlSchemaType xmlSchemaType = (XmlSchemaType)redefine.Schema.SchemaTypes[xmlSchemaComplexType.QualifiedName];
						if (xmlSchemaType != null)
						{
							if (xmlSchemaType is XmlSchemaComplexType)
							{
								xmlSchemaComplexType.Redefined = xmlSchemaType;
								this.CheckRefinedComplexType(xmlSchemaComplexType);
							}
							else
							{
								base.SendValidationEvent("Sch_SimpleToComplexTypeRedefine", xmlSchemaComplexType);
							}
						}
						else
						{
							base.SendValidationEvent("Sch_ComplexTypeRedefineNotFound", xmlSchemaComplexType);
						}
					}
				}
				else if (xmlSchemaObject is XmlSchemaSimpleType)
				{
					XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)xmlSchemaObject;
					this.PreprocessSimpleType(xmlSchemaSimpleType, false);
					if (redefine.SchemaTypes[xmlSchemaSimpleType.QualifiedName] != null)
					{
						base.SendValidationEvent("Sch_SimpleTypeDoubleRedefine", xmlSchemaSimpleType);
					}
					else
					{
						base.AddToTable(redefine.SchemaTypes, xmlSchemaSimpleType.QualifiedName, xmlSchemaSimpleType);
						XmlSchemaType xmlSchemaType2 = (XmlSchemaType)redefine.Schema.SchemaTypes[xmlSchemaSimpleType.QualifiedName];
						if (xmlSchemaType2 != null)
						{
							if (xmlSchemaType2 is XmlSchemaSimpleType)
							{
								xmlSchemaSimpleType.Redefined = xmlSchemaType2;
								this.CheckRefinedSimpleType(xmlSchemaSimpleType);
							}
							else
							{
								base.SendValidationEvent("Sch_ComplexToSimpleTypeRedefine", xmlSchemaSimpleType);
							}
						}
						else
						{
							base.SendValidationEvent("Sch_SimpleTypeRedefineNotFound", xmlSchemaSimpleType);
						}
					}
				}
			}
			foreach (object obj in redefine.Groups)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				redefine.Schema.Groups.Insert((XmlQualifiedName)dictionaryEntry.Key, (XmlSchemaObject)dictionaryEntry.Value);
			}
			foreach (object obj2 in redefine.AttributeGroups)
			{
				DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
				redefine.Schema.AttributeGroups.Insert((XmlQualifiedName)dictionaryEntry2.Key, (XmlSchemaObject)dictionaryEntry2.Value);
			}
			foreach (object obj3 in redefine.SchemaTypes)
			{
				DictionaryEntry dictionaryEntry3 = (DictionaryEntry)obj3;
				redefine.Schema.SchemaTypes.Insert((XmlQualifiedName)dictionaryEntry3.Key, (XmlSchemaObject)dictionaryEntry3.Value);
			}
		}

		private int CountGroupSelfReference(XmlSchemaObjectCollection items, XmlQualifiedName name)
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
						if (xmlSchemaGroupRef.MinOccurs != 1m || xmlSchemaGroupRef.MaxOccurs != 1m)
						{
							base.SendValidationEvent("Sch_MinMaxGroupRedefine", xmlSchemaGroupRef);
						}
						num++;
					}
				}
				else if (xmlSchemaParticle is XmlSchemaGroupBase)
				{
					num += this.CountGroupSelfReference(((XmlSchemaGroupBase)xmlSchemaParticle).Items, name);
				}
				if (num > 1)
				{
					break;
				}
			}
			return num;
		}

		private void CheckRefinedGroup(XmlSchemaGroup group)
		{
			int num = 0;
			if (group.Particle != null)
			{
				num = this.CountGroupSelfReference(group.Particle.Items, group.QualifiedName);
			}
			if (num > 1)
			{
				base.SendValidationEvent("Sch_MultipleGroupSelfRef", group);
			}
		}

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
		}

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

		private void PreprocessAttributeContent(XmlSchemaAttribute attribute)
		{
			this.PreprocessAnnotation(attribute);
			if (this.schema.TargetNamespace == "http://www.w3.org/2001/XMLSchema-instance")
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
				if (!element.SchemaTypeName.IsEmpty || element.IsAbstract || element.Block != XmlSchemaDerivationMethod.None || element.SchemaType != null || element.HasConstraints || element.DefaultValue != null || element.Form != XmlSchemaForm.None || element.FixedValue != null || element.HasNillableAttribute)
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
			if (element.IsAbstract)
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
			if (this.schema.IdentityConstraints[constraint.QualifiedName] != null)
			{
				base.SendValidationEvent("Sch_DupIdentityConstraint", constraint.QualifiedName.ToString(), constraint);
				flag = false;
			}
			else
			{
				this.schema.IdentityConstraints.Add(constraint.QualifiedName, constraint);
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
						simpleType.SetFinalResolved(this.finalDefault & (XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union));
					}
				}
				else
				{
					if ((simpleType.Final & ~(XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union)) != XmlSchemaDerivationMethod.Empty)
					{
						base.SendValidationEvent("Sch_InvalidSimpleTypeFinalValue", simpleType);
					}
					simpleType.SetFinalResolved(simpleType.Final & (XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union));
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
			if (notation.Public != null)
			{
				try
				{
					XmlConvert.ToUri(notation.Public);
					goto IL_0075;
				}
				catch
				{
					base.SendValidationEvent("Sch_InvalidPublicAttribute", notation.Public, notation);
					goto IL_0075;
				}
			}
			base.SendValidationEvent("Sch_MissRequiredAttribute", "public", notation);
			IL_0075:
			if (notation.System != null)
			{
				try
				{
					XmlConvert.ToUri(notation.System);
				}
				catch
				{
					base.SendValidationEvent("Sch_InvalidSystemAttribute", notation.System, notation);
				}
			}
			this.PreprocessAnnotation(notation);
			this.ValidateIdAttribute(notation);
		}

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
					goto IL_027F;
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
					goto IL_027F;
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
					goto IL_027F;
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
					((XmlSchemaAny)particle).BuildNamespaceListV1Compat(this.targetNamespace);
				}
				catch
				{
					base.SendValidationEvent("Sch_InvalidAny", particle);
				}
			}
			IL_027F:
			this.PreprocessAnnotation(particle);
			this.ValidateIdAttribute(particle);
		}

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
					anyAttribute.BuildNamespaceListV1Compat(this.targetNamespace);
				}
				catch
				{
					base.SendValidationEvent("Sch_InvalidAnyAttribute", anyAttribute);
				}
				this.ValidateIdAttribute(anyAttribute);
			}
		}

		private void ValidateIdAttribute(XmlSchemaObject xso)
		{
			if (xso.IdAttribute != null)
			{
				try
				{
					xso.IdAttribute = base.NameTable.Add(XmlConvert.VerifyNCName(xso.IdAttribute));
					if (this.schema.Ids[xso.IdAttribute] != null)
					{
						base.SendValidationEvent("Sch_DupIdAttribute", xso);
					}
					else
					{
						this.schema.Ids.Add(xso.IdAttribute, xso);
					}
				}
				catch (Exception ex)
				{
					base.SendValidationEvent("Sch_InvalidIdAttribute", ex.Message, xso);
				}
			}
		}

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

		private void ValidateQNameAttribute(XmlSchemaObject xso, string attributeName, XmlQualifiedName value)
		{
			try
			{
				value.Verify();
				value.Atomize(base.NameTable);
				if (this.referenceNamespaces[value.Namespace] == null)
				{
					base.SendValidationEvent("Sch_UnrefNS", value.Namespace, xso, XmlSeverityType.Warning);
				}
			}
			catch (Exception ex)
			{
				base.SendValidationEvent("Sch_InvalidAttribute", attributeName, ex.Message, xso);
			}
		}

		private void SetParent(XmlSchemaObject child, XmlSchemaObject parent)
		{
			child.Parent = parent;
		}

		private void PreprocessAnnotation(XmlSchemaObject schemaObject)
		{
			if (schemaObject is XmlSchemaAnnotated)
			{
				XmlSchemaAnnotated xmlSchemaAnnotated = schemaObject as XmlSchemaAnnotated;
				if (xmlSchemaAnnotated.Annotation != null)
				{
					xmlSchemaAnnotated.Annotation.Parent = schemaObject;
					foreach (XmlSchemaObject xmlSchemaObject in xmlSchemaAnnotated.Annotation.Items)
					{
						xmlSchemaObject.Parent = xmlSchemaAnnotated.Annotation;
					}
				}
			}
		}

		private Uri ResolveSchemaLocationUri(XmlSchema enclosingSchema, string location)
		{
			Uri uri;
			try
			{
				uri = this.xmlResolver.ResolveUri(enclosingSchema.BaseUri, location);
			}
			catch
			{
				uri = null;
			}
			return uri;
		}

		private Stream GetSchemaEntity(Uri ruri)
		{
			Stream stream;
			try
			{
				stream = (Stream)this.xmlResolver.GetEntity(ruri, null, null);
			}
			catch
			{
				stream = null;
			}
			return stream;
		}

		private const XmlSchemaDerivationMethod schemaBlockDefaultAllowed = XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

		private const XmlSchemaDerivationMethod schemaFinalDefaultAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union;

		private const XmlSchemaDerivationMethod elementBlockAllowed = XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

		private const XmlSchemaDerivationMethod elementFinalAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

		private const XmlSchemaDerivationMethod simpleTypeFinalAllowed = XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union;

		private const XmlSchemaDerivationMethod complexTypeBlockAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

		private const XmlSchemaDerivationMethod complexTypeFinalAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

		private XmlSchema schema;

		private string targetNamespace;

		private bool buildinIncluded;

		private XmlSchemaForm elementFormDefault;

		private XmlSchemaForm attributeFormDefault;

		private XmlSchemaDerivationMethod blockDefault;

		private XmlSchemaDerivationMethod finalDefault;

		private Hashtable schemaLocations;

		private Hashtable referenceNamespaces;

		private string Xmlns;

		private XmlResolver xmlResolver;

		private enum Compositor
		{
			Root,
			Include,
			Import
		}
	}
}
