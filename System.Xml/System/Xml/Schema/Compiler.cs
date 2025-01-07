using System;
using System.Collections;
using System.Globalization;

namespace System.Xml.Schema
{
	internal sealed class Compiler : BaseProcessor
	{
		public Compiler(XmlNameTable nameTable, ValidationEventHandler eventHandler, XmlSchema schemaForSchema, XmlSchemaCompilationSettings compilationSettings)
			: base(nameTable, null, eventHandler, compilationSettings)
		{
			this.schemaForSchema = schemaForSchema;
		}

		public bool Execute(XmlSchemaSet schemaSet, SchemaInfo schemaCompiledInfo)
		{
			this.Compile();
			if (!base.HasErrors)
			{
				this.Output(schemaCompiledInfo);
				schemaSet.elements = this.elements;
				schemaSet.attributes = this.attributes;
				schemaSet.schemaTypes = this.schemaTypes;
				schemaSet.substitutionGroups = this.examplars;
			}
			return !base.HasErrors;
		}

		internal void Prepare(XmlSchema schema, bool cleanup)
		{
			if (this.schemasToCompile[schema] != null)
			{
				return;
			}
			this.schemasToCompile.Add(schema, schema);
			foreach (object obj in schema.Elements.Values)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj;
				if (cleanup)
				{
					this.CleanupElement(xmlSchemaElement);
				}
				base.AddToTable(this.elements, xmlSchemaElement.QualifiedName, xmlSchemaElement);
			}
			foreach (object obj2 in schema.Attributes.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj2;
				if (cleanup)
				{
					this.CleanupAttribute(xmlSchemaAttribute);
				}
				base.AddToTable(this.attributes, xmlSchemaAttribute.QualifiedName, xmlSchemaAttribute);
			}
			foreach (object obj3 in schema.Groups.Values)
			{
				XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup)obj3;
				if (cleanup)
				{
					this.CleanupGroup(xmlSchemaGroup);
				}
				base.AddToTable(this.groups, xmlSchemaGroup.QualifiedName, xmlSchemaGroup);
			}
			foreach (object obj4 in schema.AttributeGroups.Values)
			{
				XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)obj4;
				if (cleanup)
				{
					this.CleanupAttributeGroup(xmlSchemaAttributeGroup);
				}
				base.AddToTable(this.attributeGroups, xmlSchemaAttributeGroup.QualifiedName, xmlSchemaAttributeGroup);
			}
			foreach (object obj5 in schema.SchemaTypes.Values)
			{
				XmlSchemaType xmlSchemaType = (XmlSchemaType)obj5;
				if (cleanup)
				{
					XmlSchemaComplexType xmlSchemaComplexType = xmlSchemaType as XmlSchemaComplexType;
					if (xmlSchemaComplexType != null)
					{
						this.CleanupComplexType(xmlSchemaComplexType);
					}
					else
					{
						this.CleanupSimpleType(xmlSchemaType as XmlSchemaSimpleType);
					}
				}
				base.AddToTable(this.schemaTypes, xmlSchemaType.QualifiedName, xmlSchemaType);
			}
			foreach (object obj6 in schema.Notations.Values)
			{
				XmlSchemaNotation xmlSchemaNotation = (XmlSchemaNotation)obj6;
				base.AddToTable(this.notations, xmlSchemaNotation.QualifiedName, xmlSchemaNotation);
			}
			foreach (object obj7 in schema.IdentityConstraints.Values)
			{
				XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint = (XmlSchemaIdentityConstraint)obj7;
				base.AddToTable(this.identityConstraints, xmlSchemaIdentityConstraint.QualifiedName, xmlSchemaIdentityConstraint);
			}
		}

		private void UpdateSForSSimpleTypes()
		{
			XmlSchemaSimpleType[] builtInTypes = DatatypeImplementation.GetBuiltInTypes();
			int num = builtInTypes.Length - 3;
			for (int i = 12; i < num; i++)
			{
				XmlSchemaSimpleType xmlSchemaSimpleType = builtInTypes[i];
				this.schemaForSchema.SchemaTypes.Replace(xmlSchemaSimpleType.QualifiedName, xmlSchemaSimpleType);
				this.schemaTypes.Replace(xmlSchemaSimpleType.QualifiedName, xmlSchemaSimpleType);
			}
		}

		private void Output(SchemaInfo schemaInfo)
		{
			foreach (object obj in this.schemasToCompile.Values)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				string text = xmlSchema.TargetNamespace;
				if (text == null)
				{
					text = string.Empty;
				}
				schemaInfo.TargetNamespaces[text] = text;
			}
			foreach (object obj2 in this.elements.Values)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj2;
				schemaInfo.ElementDecls.Add(xmlSchemaElement.QualifiedName, xmlSchemaElement.ElementDecl);
			}
			foreach (object obj3 in this.attributes.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj3;
				schemaInfo.AttributeDecls.Add(xmlSchemaAttribute.QualifiedName, xmlSchemaAttribute.AttDef);
			}
			foreach (object obj4 in this.schemaTypes.Values)
			{
				XmlSchemaType xmlSchemaType = (XmlSchemaType)obj4;
				schemaInfo.ElementDeclsByType.Add(xmlSchemaType.QualifiedName, xmlSchemaType.ElementDecl);
			}
			foreach (object obj5 in this.notations.Values)
			{
				XmlSchemaNotation xmlSchemaNotation = (XmlSchemaNotation)obj5;
				SchemaNotation schemaNotation = new SchemaNotation(xmlSchemaNotation.QualifiedName);
				schemaNotation.SystemLiteral = xmlSchemaNotation.System;
				schemaNotation.Pubid = xmlSchemaNotation.Public;
				if (schemaInfo.Notations[schemaNotation.Name.Name] == null)
				{
					schemaInfo.Notations.Add(schemaNotation.Name.Name, schemaNotation);
				}
			}
		}

		internal void ImportAllCompiledSchemas(XmlSchemaSet schemaSet)
		{
			SortedList sortedSchemas = schemaSet.SortedSchemas;
			for (int i = 0; i < sortedSchemas.Count; i++)
			{
				XmlSchema xmlSchema = (XmlSchema)sortedSchemas.GetByIndex(i);
				if (xmlSchema.IsCompiledBySet)
				{
					this.Prepare(xmlSchema, false);
				}
			}
		}

		internal bool Compile()
		{
			this.schemaTypes.Insert(DatatypeImplementation.QnAnyType, XmlSchemaComplexType.AnyType);
			if (this.schemaForSchema != null)
			{
				this.schemaForSchema.SchemaTypes.Replace(DatatypeImplementation.QnAnyType, XmlSchemaComplexType.AnyType);
				this.UpdateSForSSimpleTypes();
			}
			foreach (object obj in this.groups.Values)
			{
				XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup)obj;
				this.CompileGroup(xmlSchemaGroup);
			}
			foreach (object obj2 in this.attributeGroups.Values)
			{
				XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)obj2;
				this.CompileAttributeGroup(xmlSchemaAttributeGroup);
			}
			foreach (object obj3 in this.schemaTypes.Values)
			{
				XmlSchemaType xmlSchemaType = (XmlSchemaType)obj3;
				XmlSchemaComplexType xmlSchemaComplexType = xmlSchemaType as XmlSchemaComplexType;
				if (xmlSchemaComplexType != null)
				{
					this.CompileComplexType(xmlSchemaComplexType);
				}
				else
				{
					this.CompileSimpleType((XmlSchemaSimpleType)xmlSchemaType);
				}
			}
			foreach (object obj4 in this.elements.Values)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj4;
				if (xmlSchemaElement.ElementDecl == null)
				{
					this.CompileElement(xmlSchemaElement);
				}
			}
			foreach (object obj5 in this.attributes.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj5;
				if (xmlSchemaAttribute.AttDef == null)
				{
					this.CompileAttribute(xmlSchemaAttribute);
				}
			}
			using (IEnumerator enumerator6 = this.identityConstraints.Values.GetEnumerator())
			{
				while (enumerator6.MoveNext())
				{
					object obj6 = enumerator6.Current;
					XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint = (XmlSchemaIdentityConstraint)obj6;
					if (xmlSchemaIdentityConstraint.CompiledConstraint == null)
					{
						this.CompileIdentityConstraint(xmlSchemaIdentityConstraint);
					}
				}
				goto IL_0240;
			}
			IL_0226:
			XmlSchemaComplexType xmlSchemaComplexType2 = (XmlSchemaComplexType)this.complexTypeStack.Pop();
			this.CompileComplexTypeElements(xmlSchemaComplexType2);
			IL_0240:
			if (this.complexTypeStack.Count <= 0)
			{
				this.ProcessSubstitutionGroups();
				foreach (object obj7 in this.schemaTypes.Values)
				{
					XmlSchemaType xmlSchemaType2 = (XmlSchemaType)obj7;
					XmlSchemaComplexType xmlSchemaComplexType3 = xmlSchemaType2 as XmlSchemaComplexType;
					if (xmlSchemaComplexType3 != null)
					{
						this.CheckParticleDerivation(xmlSchemaComplexType3);
					}
				}
				foreach (object obj8 in this.elements.Values)
				{
					XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)obj8;
					XmlSchemaComplexType xmlSchemaComplexType4 = xmlSchemaElement2.ElementSchemaType as XmlSchemaComplexType;
					if (xmlSchemaComplexType4 != null && xmlSchemaElement2.SchemaTypeName == XmlQualifiedName.Empty)
					{
						this.CheckParticleDerivation(xmlSchemaComplexType4);
					}
				}
				foreach (object obj9 in this.groups.Values)
				{
					XmlSchemaGroup xmlSchemaGroup2 = (XmlSchemaGroup)obj9;
					XmlSchemaGroup redefined = xmlSchemaGroup2.Redefined;
					if (redefined != null)
					{
						this.RecursivelyCheckRedefinedGroups(xmlSchemaGroup2, redefined);
					}
				}
				foreach (object obj10 in this.attributeGroups.Values)
				{
					XmlSchemaAttributeGroup xmlSchemaAttributeGroup2 = (XmlSchemaAttributeGroup)obj10;
					XmlSchemaAttributeGroup redefined2 = xmlSchemaAttributeGroup2.Redefined;
					if (redefined2 != null)
					{
						this.RecursivelyCheckRedefinedAttributeGroups(xmlSchemaAttributeGroup2, redefined2);
					}
				}
				return !base.HasErrors;
			}
			goto IL_0226;
		}

		private void CleanupAttribute(XmlSchemaAttribute attribute)
		{
			if (attribute.SchemaType != null)
			{
				this.CleanupSimpleType(attribute.SchemaType);
			}
			attribute.AttDef = null;
		}

		private void CleanupAttributeGroup(XmlSchemaAttributeGroup attributeGroup)
		{
			this.CleanupAttributes(attributeGroup.Attributes);
			attributeGroup.AttributeUses.Clear();
			attributeGroup.AttributeWildcard = null;
			if (attributeGroup.Redefined != null)
			{
				this.CleanupAttributeGroup(attributeGroup.Redefined);
			}
		}

		private void CleanupComplexType(XmlSchemaComplexType complexType)
		{
			if (complexType.QualifiedName == DatatypeImplementation.QnAnyType)
			{
				return;
			}
			if (complexType.ContentModel != null)
			{
				if (complexType.ContentModel is XmlSchemaSimpleContent)
				{
					XmlSchemaSimpleContent xmlSchemaSimpleContent = (XmlSchemaSimpleContent)complexType.ContentModel;
					if (xmlSchemaSimpleContent.Content is XmlSchemaSimpleContentExtension)
					{
						XmlSchemaSimpleContentExtension xmlSchemaSimpleContentExtension = (XmlSchemaSimpleContentExtension)xmlSchemaSimpleContent.Content;
						this.CleanupAttributes(xmlSchemaSimpleContentExtension.Attributes);
					}
					else
					{
						XmlSchemaSimpleContentRestriction xmlSchemaSimpleContentRestriction = (XmlSchemaSimpleContentRestriction)xmlSchemaSimpleContent.Content;
						this.CleanupAttributes(xmlSchemaSimpleContentRestriction.Attributes);
					}
				}
				else
				{
					XmlSchemaComplexContent xmlSchemaComplexContent = (XmlSchemaComplexContent)complexType.ContentModel;
					if (xmlSchemaComplexContent.Content is XmlSchemaComplexContentExtension)
					{
						XmlSchemaComplexContentExtension xmlSchemaComplexContentExtension = (XmlSchemaComplexContentExtension)xmlSchemaComplexContent.Content;
						this.CleanupParticle(xmlSchemaComplexContentExtension.Particle);
						this.CleanupAttributes(xmlSchemaComplexContentExtension.Attributes);
					}
					else
					{
						XmlSchemaComplexContentRestriction xmlSchemaComplexContentRestriction = (XmlSchemaComplexContentRestriction)xmlSchemaComplexContent.Content;
						this.CleanupParticle(xmlSchemaComplexContentRestriction.Particle);
						this.CleanupAttributes(xmlSchemaComplexContentRestriction.Attributes);
					}
				}
			}
			else
			{
				this.CleanupParticle(complexType.Particle);
				this.CleanupAttributes(complexType.Attributes);
			}
			complexType.LocalElements.Clear();
			complexType.AttributeUses.Clear();
			complexType.SetAttributeWildcard(null);
			complexType.SetContentTypeParticle(XmlSchemaParticle.Empty);
			complexType.ElementDecl = null;
			complexType.HasDuplicateDecls = false;
			complexType.HasWildCard = false;
			if (complexType.Redefined != null)
			{
				this.CleanupComplexType(complexType.Redefined as XmlSchemaComplexType);
			}
		}

		private void CleanupSimpleType(XmlSchemaSimpleType simpleType)
		{
			if (simpleType == XmlSchemaType.GetBuiltInSimpleType(simpleType.TypeCode))
			{
				return;
			}
			simpleType.ElementDecl = null;
			if (simpleType.Redefined != null)
			{
				this.CleanupSimpleType(simpleType.Redefined as XmlSchemaSimpleType);
			}
		}

		private void CleanupElement(XmlSchemaElement element)
		{
			if (element.SchemaType != null)
			{
				XmlSchemaComplexType xmlSchemaComplexType = element.SchemaType as XmlSchemaComplexType;
				if (xmlSchemaComplexType != null)
				{
					this.CleanupComplexType(xmlSchemaComplexType);
				}
				else
				{
					this.CleanupSimpleType((XmlSchemaSimpleType)element.SchemaType);
				}
			}
			foreach (XmlSchemaObject xmlSchemaObject in element.Constraints)
			{
				XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint = (XmlSchemaIdentityConstraint)xmlSchemaObject;
				xmlSchemaIdentityConstraint.CompiledConstraint = null;
			}
			element.ElementDecl = null;
			element.IsLocalTypeDerivationChecked = false;
		}

		private void CleanupAttributes(XmlSchemaObjectCollection attributes)
		{
			foreach (XmlSchemaObject xmlSchemaObject in attributes)
			{
				if (xmlSchemaObject is XmlSchemaAttribute)
				{
					this.CleanupAttribute((XmlSchemaAttribute)xmlSchemaObject);
				}
			}
		}

		private void CleanupGroup(XmlSchemaGroup group)
		{
			this.CleanupParticle(group.Particle);
			group.CanonicalParticle = null;
			if (group.Redefined != null)
			{
				this.CleanupGroup(group.Redefined);
			}
		}

		private void CleanupParticle(XmlSchemaParticle particle)
		{
			if (particle is XmlSchemaElement)
			{
				this.CleanupElement((XmlSchemaElement)particle);
				return;
			}
			if (particle is XmlSchemaGroupBase)
			{
				foreach (XmlSchemaObject xmlSchemaObject in ((XmlSchemaGroupBase)particle).Items)
				{
					XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
					this.CleanupParticle(xmlSchemaParticle);
				}
			}
		}

		private void ProcessSubstitutionGroups()
		{
			foreach (object obj in this.elements.Values)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj;
				if (!xmlSchemaElement.SubstitutionGroup.IsEmpty)
				{
					XmlSchemaElement xmlSchemaElement2 = this.elements[xmlSchemaElement.SubstitutionGroup] as XmlSchemaElement;
					if (xmlSchemaElement2 == null)
					{
						base.SendValidationEvent("Sch_NoExamplar", xmlSchemaElement);
					}
					else
					{
						if (!XmlSchemaType.IsDerivedFrom(xmlSchemaElement.ElementSchemaType, xmlSchemaElement2.ElementSchemaType, xmlSchemaElement2.FinalResolved))
						{
							base.SendValidationEvent("Sch_InvalidSubstitutionMember", xmlSchemaElement.QualifiedName.ToString(), xmlSchemaElement2.QualifiedName.ToString(), xmlSchemaElement);
						}
						if ((xmlSchemaElement2.BlockResolved & XmlSchemaDerivationMethod.Substitution) == XmlSchemaDerivationMethod.Empty)
						{
							XmlSchemaSubstitutionGroup xmlSchemaSubstitutionGroup = (XmlSchemaSubstitutionGroup)this.examplars[xmlSchemaElement.SubstitutionGroup];
							if (xmlSchemaSubstitutionGroup == null)
							{
								xmlSchemaSubstitutionGroup = new XmlSchemaSubstitutionGroup();
								xmlSchemaSubstitutionGroup.Examplar = xmlSchemaElement.SubstitutionGroup;
								this.examplars.Add(xmlSchemaElement.SubstitutionGroup, xmlSchemaSubstitutionGroup);
							}
							ArrayList members = xmlSchemaSubstitutionGroup.Members;
							if (!members.Contains(xmlSchemaElement))
							{
								members.Add(xmlSchemaElement);
							}
						}
					}
				}
			}
			foreach (object obj2 in this.examplars.Values)
			{
				XmlSchemaSubstitutionGroup xmlSchemaSubstitutionGroup2 = (XmlSchemaSubstitutionGroup)obj2;
				this.CompileSubstitutionGroup(xmlSchemaSubstitutionGroup2);
			}
		}

		private void CompileSubstitutionGroup(XmlSchemaSubstitutionGroup substitutionGroup)
		{
			if (substitutionGroup.IsProcessing)
			{
				using (IEnumerator enumerator = substitutionGroup.Members.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)enumerator.Current;
						base.SendValidationEvent("Sch_SubstitutionCircularRef", xmlSchemaElement);
						return;
					}
				}
			}
			XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)this.elements[substitutionGroup.Examplar];
			if (!substitutionGroup.Members.Contains(xmlSchemaElement2))
			{
				substitutionGroup.IsProcessing = true;
				try
				{
					if (xmlSchemaElement2.FinalResolved == XmlSchemaDerivationMethod.All)
					{
						base.SendValidationEvent("Sch_InvalidExamplar", xmlSchemaElement2);
					}
					ArrayList arrayList = null;
					foreach (object obj in substitutionGroup.Members)
					{
						XmlSchemaElement xmlSchemaElement3 = (XmlSchemaElement)obj;
						XmlSchemaSubstitutionGroup xmlSchemaSubstitutionGroup = (XmlSchemaSubstitutionGroup)this.examplars[xmlSchemaElement3.QualifiedName];
						if (xmlSchemaSubstitutionGroup != null)
						{
							this.CompileSubstitutionGroup(xmlSchemaSubstitutionGroup);
							foreach (object obj2 in xmlSchemaSubstitutionGroup.Members)
							{
								XmlSchemaElement xmlSchemaElement4 = (XmlSchemaElement)obj2;
								if (xmlSchemaElement4 != xmlSchemaElement3)
								{
									if (arrayList == null)
									{
										arrayList = new ArrayList();
									}
									arrayList.Add(xmlSchemaElement4);
								}
							}
						}
					}
					if (arrayList != null)
					{
						foreach (object obj3 in arrayList)
						{
							XmlSchemaElement xmlSchemaElement5 = (XmlSchemaElement)obj3;
							substitutionGroup.Members.Add(xmlSchemaElement5);
						}
					}
					substitutionGroup.Members.Add(xmlSchemaElement2);
				}
				finally
				{
					substitutionGroup.IsProcessing = false;
				}
				return;
			}
		}

		private void RecursivelyCheckRedefinedGroups(XmlSchemaGroup redefinedGroup, XmlSchemaGroup baseGroup)
		{
			if (baseGroup.Redefined != null)
			{
				this.RecursivelyCheckRedefinedGroups(baseGroup, baseGroup.Redefined);
			}
			if (redefinedGroup.SelfReferenceCount == 0)
			{
				if (baseGroup.CanonicalParticle == null)
				{
					baseGroup.CanonicalParticle = this.CannonicalizeParticle(baseGroup.Particle, true);
				}
				if (redefinedGroup.CanonicalParticle == null)
				{
					redefinedGroup.CanonicalParticle = this.CannonicalizeParticle(redefinedGroup.Particle, true);
				}
				this.CompileParticleElements(redefinedGroup.CanonicalParticle);
				this.CompileParticleElements(baseGroup.CanonicalParticle);
				this.CheckParticleDerivation(redefinedGroup.CanonicalParticle, baseGroup.CanonicalParticle);
			}
		}

		private void RecursivelyCheckRedefinedAttributeGroups(XmlSchemaAttributeGroup attributeGroup, XmlSchemaAttributeGroup baseAttributeGroup)
		{
			if (baseAttributeGroup.Redefined != null)
			{
				this.RecursivelyCheckRedefinedAttributeGroups(baseAttributeGroup, baseAttributeGroup.Redefined);
			}
			if (attributeGroup.SelfReferenceCount == 0)
			{
				this.CompileAttributeGroup(baseAttributeGroup);
				this.CompileAttributeGroup(attributeGroup);
				this.CheckAtrributeGroupRestriction(baseAttributeGroup, attributeGroup);
			}
		}

		private void CompileGroup(XmlSchemaGroup group)
		{
			if (group.IsProcessing)
			{
				base.SendValidationEvent("Sch_GroupCircularRef", group);
				group.CanonicalParticle = XmlSchemaParticle.Empty;
				return;
			}
			group.IsProcessing = true;
			if (group.CanonicalParticle == null)
			{
				group.CanonicalParticle = this.CannonicalizeParticle(group.Particle, true);
			}
			group.IsProcessing = false;
		}

		private void CompileSimpleType(XmlSchemaSimpleType simpleType)
		{
			if (simpleType.IsProcessing)
			{
				throw new XmlSchemaException("Sch_TypeCircularRef", simpleType);
			}
			if (simpleType.ElementDecl != null)
			{
				return;
			}
			simpleType.IsProcessing = true;
			try
			{
				if (simpleType.Content is XmlSchemaSimpleTypeList)
				{
					XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = (XmlSchemaSimpleTypeList)simpleType.Content;
					simpleType.SetBaseSchemaType(DatatypeImplementation.AnySimpleType);
					XmlSchemaDatatype xmlSchemaDatatype;
					if (xmlSchemaSimpleTypeList.ItemTypeName.IsEmpty)
					{
						this.CompileSimpleType(xmlSchemaSimpleTypeList.ItemType);
						xmlSchemaSimpleTypeList.BaseItemType = xmlSchemaSimpleTypeList.ItemType;
						xmlSchemaDatatype = xmlSchemaSimpleTypeList.ItemType.Datatype;
					}
					else
					{
						XmlSchemaSimpleType simpleType2 = this.GetSimpleType(xmlSchemaSimpleTypeList.ItemTypeName);
						if (simpleType2 == null)
						{
							throw new XmlSchemaException("Sch_UndeclaredSimpleType", xmlSchemaSimpleTypeList.ItemTypeName.ToString(), xmlSchemaSimpleTypeList);
						}
						if ((simpleType2.FinalResolved & XmlSchemaDerivationMethod.List) != XmlSchemaDerivationMethod.Empty)
						{
							base.SendValidationEvent("Sch_BaseFinalList", simpleType);
						}
						xmlSchemaSimpleTypeList.BaseItemType = simpleType2;
						xmlSchemaDatatype = simpleType2.Datatype;
					}
					simpleType.SetDatatype(xmlSchemaDatatype.DeriveByList(simpleType));
					simpleType.SetDerivedBy(XmlSchemaDerivationMethod.List);
				}
				else if (simpleType.Content is XmlSchemaSimpleTypeRestriction)
				{
					XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = (XmlSchemaSimpleTypeRestriction)simpleType.Content;
					XmlSchemaDatatype xmlSchemaDatatype2;
					if (xmlSchemaSimpleTypeRestriction.BaseTypeName.IsEmpty)
					{
						this.CompileSimpleType(xmlSchemaSimpleTypeRestriction.BaseType);
						simpleType.SetBaseSchemaType(xmlSchemaSimpleTypeRestriction.BaseType);
						xmlSchemaDatatype2 = xmlSchemaSimpleTypeRestriction.BaseType.Datatype;
					}
					else if (simpleType.Redefined != null && xmlSchemaSimpleTypeRestriction.BaseTypeName == simpleType.Redefined.QualifiedName)
					{
						this.CompileSimpleType((XmlSchemaSimpleType)simpleType.Redefined);
						simpleType.SetBaseSchemaType(simpleType.Redefined.BaseXmlSchemaType);
						xmlSchemaDatatype2 = simpleType.Redefined.Datatype;
					}
					else
					{
						if (xmlSchemaSimpleTypeRestriction.BaseTypeName.Equals(DatatypeImplementation.QnAnySimpleType))
						{
							XmlSchema parentSchema = Preprocessor.GetParentSchema(simpleType);
							if (parentSchema.TargetNamespace != "http://www.w3.org/2001/XMLSchema")
							{
								throw new XmlSchemaException("Sch_InvalidSimpleTypeRestriction", xmlSchemaSimpleTypeRestriction.BaseTypeName.ToString(), simpleType);
							}
						}
						XmlSchemaSimpleType simpleType3 = this.GetSimpleType(xmlSchemaSimpleTypeRestriction.BaseTypeName);
						if (simpleType3 == null)
						{
							throw new XmlSchemaException("Sch_UndeclaredSimpleType", xmlSchemaSimpleTypeRestriction.BaseTypeName.ToString(), xmlSchemaSimpleTypeRestriction);
						}
						if ((simpleType3.FinalResolved & XmlSchemaDerivationMethod.Restriction) != XmlSchemaDerivationMethod.Empty)
						{
							base.SendValidationEvent("Sch_BaseFinalRestriction", simpleType);
						}
						simpleType.SetBaseSchemaType(simpleType3);
						xmlSchemaDatatype2 = simpleType3.Datatype;
					}
					simpleType.SetDatatype(xmlSchemaDatatype2.DeriveByRestriction(xmlSchemaSimpleTypeRestriction.Facets, base.NameTable, simpleType));
					simpleType.SetDerivedBy(XmlSchemaDerivationMethod.Restriction);
				}
				else
				{
					XmlSchemaSimpleType[] array = this.CompileBaseMemberTypes(simpleType);
					simpleType.SetBaseSchemaType(DatatypeImplementation.AnySimpleType);
					simpleType.SetDatatype(XmlSchemaDatatype.DeriveByUnion(array, simpleType));
					simpleType.SetDerivedBy(XmlSchemaDerivationMethod.Union);
				}
			}
			catch (XmlSchemaException ex)
			{
				if (ex.SourceSchemaObject == null)
				{
					ex.SetSource(simpleType);
				}
				base.SendValidationEvent(ex);
				simpleType.SetDatatype(DatatypeImplementation.AnySimpleType.Datatype);
			}
			finally
			{
				simpleType.ElementDecl = new SchemaElementDecl
				{
					ContentValidator = ContentValidator.TextOnly,
					SchemaType = simpleType,
					Datatype = simpleType.Datatype
				};
				simpleType.IsProcessing = false;
			}
		}

		private XmlSchemaSimpleType[] CompileBaseMemberTypes(XmlSchemaSimpleType simpleType)
		{
			ArrayList arrayList = new ArrayList();
			XmlSchemaSimpleTypeUnion xmlSchemaSimpleTypeUnion = (XmlSchemaSimpleTypeUnion)simpleType.Content;
			Array memberTypes = xmlSchemaSimpleTypeUnion.MemberTypes;
			if (memberTypes != null)
			{
				foreach (object obj in memberTypes)
				{
					XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)obj;
					XmlSchemaSimpleType simpleType2 = this.GetSimpleType(xmlQualifiedName);
					if (simpleType2 == null)
					{
						throw new XmlSchemaException("Sch_UndeclaredSimpleType", xmlQualifiedName.ToString(), xmlSchemaSimpleTypeUnion);
					}
					if (simpleType2.Datatype.Variety == XmlSchemaDatatypeVariety.Union)
					{
						this.CheckUnionType(simpleType2, arrayList, simpleType);
					}
					else
					{
						arrayList.Add(simpleType2);
					}
					if ((simpleType2.FinalResolved & XmlSchemaDerivationMethod.Union) != XmlSchemaDerivationMethod.Empty)
					{
						base.SendValidationEvent("Sch_BaseFinalUnion", simpleType);
					}
				}
			}
			XmlSchemaObjectCollection baseTypes = xmlSchemaSimpleTypeUnion.BaseTypes;
			if (baseTypes != null)
			{
				foreach (XmlSchemaObject xmlSchemaObject in baseTypes)
				{
					XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)xmlSchemaObject;
					this.CompileSimpleType(xmlSchemaSimpleType);
					if (xmlSchemaSimpleType.Datatype.Variety == XmlSchemaDatatypeVariety.Union)
					{
						this.CheckUnionType(xmlSchemaSimpleType, arrayList, simpleType);
					}
					else
					{
						arrayList.Add(xmlSchemaSimpleType);
					}
				}
			}
			xmlSchemaSimpleTypeUnion.SetBaseMemberTypes(arrayList.ToArray(typeof(XmlSchemaSimpleType)) as XmlSchemaSimpleType[]);
			return xmlSchemaSimpleTypeUnion.BaseMemberTypes;
		}

		private void CheckUnionType(XmlSchemaSimpleType unionMember, ArrayList memberTypeDefinitions, XmlSchemaSimpleType parentType)
		{
			XmlSchemaDatatype datatype = unionMember.Datatype;
			if (unionMember.DerivedBy == XmlSchemaDerivationMethod.Restriction && (datatype.HasLexicalFacets || datatype.HasValueFacets))
			{
				base.SendValidationEvent("Sch_UnionFromUnion", parentType);
				return;
			}
			Datatype_union datatype_union = unionMember.Datatype as Datatype_union;
			memberTypeDefinitions.AddRange(datatype_union.BaseMemberTypes);
		}

		private void CompileComplexType(XmlSchemaComplexType complexType)
		{
			if (complexType.ElementDecl != null)
			{
				return;
			}
			if (complexType.IsProcessing)
			{
				base.SendValidationEvent("Sch_TypeCircularRef", complexType);
				return;
			}
			complexType.IsProcessing = true;
			try
			{
				if (complexType.ContentModel != null)
				{
					if (complexType.ContentModel is XmlSchemaSimpleContent)
					{
						XmlSchemaSimpleContent xmlSchemaSimpleContent = (XmlSchemaSimpleContent)complexType.ContentModel;
						complexType.SetContentType(XmlSchemaContentType.TextOnly);
						if (xmlSchemaSimpleContent.Content is XmlSchemaSimpleContentExtension)
						{
							this.CompileSimpleContentExtension(complexType, (XmlSchemaSimpleContentExtension)xmlSchemaSimpleContent.Content);
						}
						else
						{
							this.CompileSimpleContentRestriction(complexType, (XmlSchemaSimpleContentRestriction)xmlSchemaSimpleContent.Content);
						}
					}
					else
					{
						XmlSchemaComplexContent xmlSchemaComplexContent = (XmlSchemaComplexContent)complexType.ContentModel;
						if (xmlSchemaComplexContent.Content is XmlSchemaComplexContentExtension)
						{
							this.CompileComplexContentExtension(complexType, xmlSchemaComplexContent, (XmlSchemaComplexContentExtension)xmlSchemaComplexContent.Content);
						}
						else
						{
							this.CompileComplexContentRestriction(complexType, xmlSchemaComplexContent, (XmlSchemaComplexContentRestriction)xmlSchemaComplexContent.Content);
						}
					}
				}
				else
				{
					complexType.SetBaseSchemaType(XmlSchemaComplexType.AnyType);
					this.CompileLocalAttributes(XmlSchemaComplexType.AnyType, complexType, complexType.Attributes, complexType.AnyAttribute, XmlSchemaDerivationMethod.Restriction);
					complexType.SetDerivedBy(XmlSchemaDerivationMethod.Restriction);
					complexType.SetContentTypeParticle(this.CompileContentTypeParticle(complexType.Particle));
					complexType.SetContentType(this.GetSchemaContentType(complexType, null, complexType.ContentTypeParticle));
				}
				if (complexType.ContainsIdAttribute(true))
				{
					base.SendValidationEvent("Sch_TwoIdAttrUses", complexType);
				}
				SchemaElementDecl schemaElementDecl = new SchemaElementDecl();
				schemaElementDecl.ContentValidator = this.CompileComplexContent(complexType);
				schemaElementDecl.SchemaType = complexType;
				schemaElementDecl.IsAbstract = complexType.IsAbstract;
				schemaElementDecl.Datatype = complexType.Datatype;
				schemaElementDecl.Block = complexType.BlockResolved;
				schemaElementDecl.AnyAttribute = complexType.AttributeWildcard;
				foreach (object obj in complexType.AttributeUses.Values)
				{
					XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj;
					if (xmlSchemaAttribute.Use == XmlSchemaUse.Prohibited)
					{
						if (schemaElementDecl.ProhibitedAttributes[xmlSchemaAttribute.QualifiedName] == null)
						{
							schemaElementDecl.ProhibitedAttributes.Add(xmlSchemaAttribute.QualifiedName, xmlSchemaAttribute.QualifiedName);
						}
					}
					else if (schemaElementDecl.AttDefs[xmlSchemaAttribute.QualifiedName] == null && xmlSchemaAttribute.AttDef != null && xmlSchemaAttribute.AttDef.Name != XmlQualifiedName.Empty && xmlSchemaAttribute.AttDef != SchemaAttDef.Empty)
					{
						schemaElementDecl.AddAttDef(xmlSchemaAttribute.AttDef);
					}
				}
				schemaElementDecl.EndAddAttDef();
				complexType.ElementDecl = schemaElementDecl;
			}
			finally
			{
				complexType.IsProcessing = false;
			}
		}

		private void CompileSimpleContentExtension(XmlSchemaComplexType complexType, XmlSchemaSimpleContentExtension simpleExtension)
		{
			XmlSchemaComplexType xmlSchemaComplexType;
			if (complexType.Redefined != null && simpleExtension.BaseTypeName == complexType.Redefined.QualifiedName)
			{
				xmlSchemaComplexType = (XmlSchemaComplexType)complexType.Redefined;
				this.CompileComplexType(xmlSchemaComplexType);
				complexType.SetBaseSchemaType(xmlSchemaComplexType);
				complexType.SetDatatype(xmlSchemaComplexType.Datatype);
			}
			else
			{
				XmlSchemaType anySchemaType = this.GetAnySchemaType(simpleExtension.BaseTypeName);
				if (anySchemaType == null)
				{
					base.SendValidationEvent("Sch_UndeclaredType", simpleExtension.BaseTypeName.ToString(), simpleExtension);
				}
				else
				{
					complexType.SetBaseSchemaType(anySchemaType);
					complexType.SetDatatype(anySchemaType.Datatype);
				}
				xmlSchemaComplexType = anySchemaType as XmlSchemaComplexType;
			}
			if (xmlSchemaComplexType != null)
			{
				if ((xmlSchemaComplexType.FinalResolved & XmlSchemaDerivationMethod.Extension) != XmlSchemaDerivationMethod.Empty)
				{
					base.SendValidationEvent("Sch_BaseFinalExtension", complexType);
				}
				if (xmlSchemaComplexType.ContentType != XmlSchemaContentType.TextOnly)
				{
					base.SendValidationEvent("Sch_NotSimpleContent", complexType);
				}
			}
			complexType.SetDerivedBy(XmlSchemaDerivationMethod.Extension);
			this.CompileLocalAttributes(xmlSchemaComplexType, complexType, simpleExtension.Attributes, simpleExtension.AnyAttribute, XmlSchemaDerivationMethod.Extension);
		}

		private void CompileSimpleContentRestriction(XmlSchemaComplexType complexType, XmlSchemaSimpleContentRestriction simpleRestriction)
		{
			XmlSchemaComplexType xmlSchemaComplexType = null;
			XmlSchemaDatatype xmlSchemaDatatype = null;
			if (complexType.Redefined != null && simpleRestriction.BaseTypeName == complexType.Redefined.QualifiedName)
			{
				xmlSchemaComplexType = (XmlSchemaComplexType)complexType.Redefined;
				this.CompileComplexType(xmlSchemaComplexType);
				xmlSchemaDatatype = xmlSchemaComplexType.Datatype;
			}
			else
			{
				xmlSchemaComplexType = this.GetComplexType(simpleRestriction.BaseTypeName);
				if (xmlSchemaComplexType == null)
				{
					base.SendValidationEvent("Sch_UndefBaseRestriction", simpleRestriction.BaseTypeName.ToString(), simpleRestriction);
					return;
				}
				if (xmlSchemaComplexType.ContentType == XmlSchemaContentType.TextOnly)
				{
					if (simpleRestriction.BaseType == null)
					{
						xmlSchemaDatatype = xmlSchemaComplexType.Datatype;
					}
					else
					{
						this.CompileSimpleType(simpleRestriction.BaseType);
						if (!XmlSchemaType.IsDerivedFromDatatype(simpleRestriction.BaseType.Datatype, xmlSchemaComplexType.Datatype, XmlSchemaDerivationMethod.None))
						{
							base.SendValidationEvent("Sch_DerivedNotFromBase", simpleRestriction);
						}
						xmlSchemaDatatype = simpleRestriction.BaseType.Datatype;
					}
				}
				else if (xmlSchemaComplexType.ContentType == XmlSchemaContentType.Mixed && xmlSchemaComplexType.ElementDecl.ContentValidator.IsEmptiable)
				{
					if (simpleRestriction.BaseType != null)
					{
						this.CompileSimpleType(simpleRestriction.BaseType);
						complexType.SetBaseSchemaType(simpleRestriction.BaseType);
						xmlSchemaDatatype = simpleRestriction.BaseType.Datatype;
					}
					else
					{
						base.SendValidationEvent("Sch_NeedSimpleTypeChild", simpleRestriction);
					}
				}
				else
				{
					base.SendValidationEvent("Sch_NotSimpleContent", complexType);
				}
			}
			if (xmlSchemaComplexType != null && xmlSchemaComplexType.ElementDecl != null && (xmlSchemaComplexType.FinalResolved & XmlSchemaDerivationMethod.Restriction) != XmlSchemaDerivationMethod.Empty)
			{
				base.SendValidationEvent("Sch_BaseFinalRestriction", complexType);
			}
			if (xmlSchemaComplexType != null)
			{
				complexType.SetBaseSchemaType(xmlSchemaComplexType);
			}
			if (xmlSchemaDatatype != null)
			{
				try
				{
					complexType.SetDatatype(xmlSchemaDatatype.DeriveByRestriction(simpleRestriction.Facets, base.NameTable, complexType));
				}
				catch (XmlSchemaException ex)
				{
					if (ex.SourceSchemaObject == null)
					{
						ex.SetSource(complexType);
					}
					base.SendValidationEvent(ex);
					complexType.SetDatatype(DatatypeImplementation.AnySimpleType.Datatype);
				}
			}
			complexType.SetDerivedBy(XmlSchemaDerivationMethod.Restriction);
			this.CompileLocalAttributes(xmlSchemaComplexType, complexType, simpleRestriction.Attributes, simpleRestriction.AnyAttribute, XmlSchemaDerivationMethod.Restriction);
		}

		private void CompileComplexContentExtension(XmlSchemaComplexType complexType, XmlSchemaComplexContent complexContent, XmlSchemaComplexContentExtension complexExtension)
		{
			XmlSchemaComplexType xmlSchemaComplexType;
			if (complexType.Redefined != null && complexExtension.BaseTypeName == complexType.Redefined.QualifiedName)
			{
				xmlSchemaComplexType = (XmlSchemaComplexType)complexType.Redefined;
				this.CompileComplexType(xmlSchemaComplexType);
			}
			else
			{
				xmlSchemaComplexType = this.GetComplexType(complexExtension.BaseTypeName);
				if (xmlSchemaComplexType == null)
				{
					base.SendValidationEvent("Sch_UndefBaseExtension", complexExtension.BaseTypeName.ToString(), complexExtension);
					return;
				}
			}
			if ((xmlSchemaComplexType.FinalResolved & XmlSchemaDerivationMethod.Extension) != XmlSchemaDerivationMethod.Empty)
			{
				base.SendValidationEvent("Sch_BaseFinalExtension", complexType);
			}
			this.CompileLocalAttributes(xmlSchemaComplexType, complexType, complexExtension.Attributes, complexExtension.AnyAttribute, XmlSchemaDerivationMethod.Extension);
			XmlSchemaParticle contentTypeParticle = xmlSchemaComplexType.ContentTypeParticle;
			XmlSchemaParticle xmlSchemaParticle = this.CannonicalizeParticle(complexExtension.Particle, true);
			if (contentTypeParticle != XmlSchemaParticle.Empty)
			{
				if (xmlSchemaParticle != XmlSchemaParticle.Empty)
				{
					complexType.SetContentTypeParticle(this.CompileContentTypeParticle(new XmlSchemaSequence
					{
						Items = { contentTypeParticle, xmlSchemaParticle }
					}));
				}
				else
				{
					complexType.SetContentTypeParticle(contentTypeParticle);
				}
			}
			else
			{
				complexType.SetContentTypeParticle(xmlSchemaParticle);
			}
			XmlSchemaContentType xmlSchemaContentType = this.GetSchemaContentType(complexType, complexContent, xmlSchemaParticle);
			if (xmlSchemaContentType == XmlSchemaContentType.Empty)
			{
				xmlSchemaContentType = xmlSchemaComplexType.ContentType;
			}
			complexType.SetContentType(xmlSchemaContentType);
			if (xmlSchemaComplexType.ContentType != XmlSchemaContentType.Empty && complexType.ContentType != xmlSchemaComplexType.ContentType)
			{
				base.SendValidationEvent("Sch_DifContentType", complexType);
				return;
			}
			complexType.SetBaseSchemaType(xmlSchemaComplexType);
			complexType.SetDerivedBy(XmlSchemaDerivationMethod.Extension);
		}

		private void CompileComplexContentRestriction(XmlSchemaComplexType complexType, XmlSchemaComplexContent complexContent, XmlSchemaComplexContentRestriction complexRestriction)
		{
			XmlSchemaComplexType xmlSchemaComplexType;
			if (complexType.Redefined != null && complexRestriction.BaseTypeName == complexType.Redefined.QualifiedName)
			{
				xmlSchemaComplexType = (XmlSchemaComplexType)complexType.Redefined;
				this.CompileComplexType(xmlSchemaComplexType);
			}
			else
			{
				xmlSchemaComplexType = this.GetComplexType(complexRestriction.BaseTypeName);
				if (xmlSchemaComplexType == null)
				{
					base.SendValidationEvent("Sch_UndefBaseRestriction", complexRestriction.BaseTypeName.ToString(), complexRestriction);
					return;
				}
			}
			complexType.SetBaseSchemaType(xmlSchemaComplexType);
			if ((xmlSchemaComplexType.FinalResolved & XmlSchemaDerivationMethod.Restriction) != XmlSchemaDerivationMethod.Empty)
			{
				base.SendValidationEvent("Sch_BaseFinalRestriction", complexType);
			}
			this.CompileLocalAttributes(xmlSchemaComplexType, complexType, complexRestriction.Attributes, complexRestriction.AnyAttribute, XmlSchemaDerivationMethod.Restriction);
			complexType.SetContentTypeParticle(this.CompileContentTypeParticle(complexRestriction.Particle));
			XmlSchemaContentType schemaContentType = this.GetSchemaContentType(complexType, complexContent, complexType.ContentTypeParticle);
			complexType.SetContentType(schemaContentType);
			switch (schemaContentType)
			{
			case XmlSchemaContentType.Empty:
				if (xmlSchemaComplexType.ElementDecl != null && !xmlSchemaComplexType.ElementDecl.ContentValidator.IsEmptiable)
				{
					base.SendValidationEvent("Sch_InvalidContentRestrictionDetailed", Res.GetString("Sch_InvalidBaseToEmpty"), complexType);
				}
				break;
			case XmlSchemaContentType.Mixed:
				if (xmlSchemaComplexType.ContentType != XmlSchemaContentType.Mixed)
				{
					base.SendValidationEvent("Sch_InvalidContentRestrictionDetailed", Res.GetString("Sch_InvalidBaseToMixed"), complexType);
				}
				break;
			}
			complexType.SetDerivedBy(XmlSchemaDerivationMethod.Restriction);
		}

		private void CheckParticleDerivation(XmlSchemaComplexType complexType)
		{
			XmlSchemaComplexType xmlSchemaComplexType = complexType.BaseXmlSchemaType as XmlSchemaComplexType;
			this.restrictionErrorMsg = null;
			if (xmlSchemaComplexType != null && xmlSchemaComplexType != XmlSchemaComplexType.AnyType && complexType.DerivedBy == XmlSchemaDerivationMethod.Restriction)
			{
				XmlSchemaParticle xmlSchemaParticle = this.CannonicalizePointlessRoot(complexType.ContentTypeParticle);
				XmlSchemaParticle xmlSchemaParticle2 = this.CannonicalizePointlessRoot(xmlSchemaComplexType.ContentTypeParticle);
				if (!this.IsValidRestriction(xmlSchemaParticle, xmlSchemaParticle2))
				{
					if (this.restrictionErrorMsg != null)
					{
						base.SendValidationEvent("Sch_InvalidParticleRestrictionDetailed", this.restrictionErrorMsg, complexType);
						return;
					}
					base.SendValidationEvent("Sch_InvalidParticleRestriction", complexType);
					return;
				}
			}
			else if (xmlSchemaComplexType == XmlSchemaComplexType.AnyType)
			{
				foreach (object obj in complexType.LocalElements.Values)
				{
					XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj;
					if (!xmlSchemaElement.IsLocalTypeDerivationChecked)
					{
						XmlSchemaComplexType xmlSchemaComplexType2 = xmlSchemaElement.ElementSchemaType as XmlSchemaComplexType;
						if (xmlSchemaComplexType2 != null && xmlSchemaElement.SchemaTypeName == XmlQualifiedName.Empty && xmlSchemaElement.RefName == XmlQualifiedName.Empty)
						{
							xmlSchemaElement.IsLocalTypeDerivationChecked = true;
							this.CheckParticleDerivation(xmlSchemaComplexType2);
						}
					}
				}
			}
		}

		private void CheckParticleDerivation(XmlSchemaParticle derivedParticle, XmlSchemaParticle baseParticle)
		{
			this.restrictionErrorMsg = null;
			derivedParticle = this.CannonicalizePointlessRoot(derivedParticle);
			baseParticle = this.CannonicalizePointlessRoot(baseParticle);
			if (!this.IsValidRestriction(derivedParticle, baseParticle))
			{
				if (this.restrictionErrorMsg != null)
				{
					base.SendValidationEvent("Sch_InvalidParticleRestrictionDetailed", this.restrictionErrorMsg, derivedParticle);
					return;
				}
				base.SendValidationEvent("Sch_InvalidParticleRestriction", derivedParticle);
			}
		}

		private XmlSchemaParticle CompileContentTypeParticle(XmlSchemaParticle particle)
		{
			XmlSchemaParticle xmlSchemaParticle = this.CannonicalizeParticle(particle, true);
			XmlSchemaChoice xmlSchemaChoice = xmlSchemaParticle as XmlSchemaChoice;
			if (xmlSchemaChoice != null && xmlSchemaChoice.Items.Count == 0)
			{
				if (xmlSchemaChoice.MinOccurs != 0m)
				{
					base.SendValidationEvent("Sch_EmptyChoice", xmlSchemaChoice, XmlSeverityType.Warning);
				}
				return XmlSchemaParticle.Empty;
			}
			return xmlSchemaParticle;
		}

		private XmlSchemaParticle CannonicalizeParticle(XmlSchemaParticle particle, bool root)
		{
			if (particle == null || particle.IsEmpty)
			{
				return XmlSchemaParticle.Empty;
			}
			if (particle is XmlSchemaElement)
			{
				return particle;
			}
			if (particle is XmlSchemaGroupRef)
			{
				return this.CannonicalizeGroupRef((XmlSchemaGroupRef)particle, root);
			}
			if (particle is XmlSchemaAll)
			{
				return this.CannonicalizeAll((XmlSchemaAll)particle, root);
			}
			if (particle is XmlSchemaChoice)
			{
				return this.CannonicalizeChoice((XmlSchemaChoice)particle, root);
			}
			if (particle is XmlSchemaSequence)
			{
				return this.CannonicalizeSequence((XmlSchemaSequence)particle, root);
			}
			return particle;
		}

		private XmlSchemaParticle CannonicalizeElement(XmlSchemaElement element)
		{
			if (element.RefName.IsEmpty || (element.ElementDecl.Block & XmlSchemaDerivationMethod.Substitution) != XmlSchemaDerivationMethod.Empty)
			{
				return element;
			}
			XmlSchemaSubstitutionGroup xmlSchemaSubstitutionGroup = (XmlSchemaSubstitutionGroup)this.examplars[element.QualifiedName];
			if (xmlSchemaSubstitutionGroup == null)
			{
				return element;
			}
			XmlSchemaChoice xmlSchemaChoice = new XmlSchemaChoice();
			foreach (object obj in xmlSchemaSubstitutionGroup.Members)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj;
				xmlSchemaChoice.Items.Add(xmlSchemaElement);
			}
			xmlSchemaChoice.MinOccurs = element.MinOccurs;
			xmlSchemaChoice.MaxOccurs = element.MaxOccurs;
			this.CopyPosition(xmlSchemaChoice, element, false);
			return xmlSchemaChoice;
		}

		private XmlSchemaParticle CannonicalizeGroupRef(XmlSchemaGroupRef groupRef, bool root)
		{
			XmlSchemaGroup xmlSchemaGroup;
			if (groupRef.Redefined != null)
			{
				xmlSchemaGroup = groupRef.Redefined;
			}
			else
			{
				xmlSchemaGroup = (XmlSchemaGroup)this.groups[groupRef.RefName];
			}
			if (xmlSchemaGroup == null)
			{
				base.SendValidationEvent("Sch_UndefGroupRef", groupRef.RefName.ToString(), groupRef);
				return XmlSchemaParticle.Empty;
			}
			if (xmlSchemaGroup.CanonicalParticle == null)
			{
				this.CompileGroup(xmlSchemaGroup);
			}
			if (xmlSchemaGroup.CanonicalParticle == XmlSchemaParticle.Empty)
			{
				return XmlSchemaParticle.Empty;
			}
			XmlSchemaGroupBase xmlSchemaGroupBase = (XmlSchemaGroupBase)xmlSchemaGroup.CanonicalParticle;
			if (xmlSchemaGroupBase is XmlSchemaAll)
			{
				if (!root)
				{
					base.SendValidationEvent("Sch_AllRefNotRoot", "", groupRef);
					return XmlSchemaParticle.Empty;
				}
				if (groupRef.MinOccurs > 1m || groupRef.MaxOccurs != 1m)
				{
					base.SendValidationEvent("Sch_AllRefMinMax", groupRef);
					return XmlSchemaParticle.Empty;
				}
			}
			else if (xmlSchemaGroupBase is XmlSchemaChoice && xmlSchemaGroupBase.Items.Count == 0)
			{
				if (groupRef.MinOccurs != 0m)
				{
					base.SendValidationEvent("Sch_EmptyChoice", groupRef, XmlSeverityType.Warning);
				}
				return XmlSchemaParticle.Empty;
			}
			XmlSchemaGroupBase xmlSchemaGroupBase2 = ((xmlSchemaGroupBase is XmlSchemaSequence) ? new XmlSchemaSequence() : ((xmlSchemaGroupBase is XmlSchemaChoice) ? new XmlSchemaChoice() : new XmlSchemaAll()));
			xmlSchemaGroupBase2.MinOccurs = groupRef.MinOccurs;
			xmlSchemaGroupBase2.MaxOccurs = groupRef.MaxOccurs;
			xmlSchemaGroupBase2.LineNumber = groupRef.LineNumber;
			xmlSchemaGroupBase2.LinePosition = groupRef.LinePosition;
			foreach (XmlSchemaObject xmlSchemaObject in xmlSchemaGroupBase.Items)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				xmlSchemaGroupBase2.Items.Add(xmlSchemaParticle);
			}
			groupRef.SetParticle(xmlSchemaGroupBase2);
			return xmlSchemaGroupBase2;
		}

		private XmlSchemaParticle CannonicalizeAll(XmlSchemaAll all, bool root)
		{
			if (all.Items.Count > 0)
			{
				XmlSchemaAll xmlSchemaAll = new XmlSchemaAll();
				xmlSchemaAll.MinOccurs = all.MinOccurs;
				xmlSchemaAll.MaxOccurs = all.MaxOccurs;
				this.CopyPosition(xmlSchemaAll, all, true);
				foreach (XmlSchemaObject xmlSchemaObject in all.Items)
				{
					XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)xmlSchemaObject;
					XmlSchemaParticle xmlSchemaParticle = this.CannonicalizeParticle(xmlSchemaElement, false);
					if (xmlSchemaParticle != XmlSchemaParticle.Empty)
					{
						xmlSchemaAll.Items.Add(xmlSchemaParticle);
					}
				}
				all = xmlSchemaAll;
			}
			if (all.Items.Count == 0)
			{
				return XmlSchemaParticle.Empty;
			}
			if (!root)
			{
				base.SendValidationEvent("Sch_NotAllAlone", all);
				return XmlSchemaParticle.Empty;
			}
			return all;
		}

		private XmlSchemaParticle CannonicalizeChoice(XmlSchemaChoice choice, bool root)
		{
			XmlSchemaChoice xmlSchemaChoice = choice;
			if (choice.Items.Count > 0)
			{
				XmlSchemaChoice xmlSchemaChoice2 = new XmlSchemaChoice();
				xmlSchemaChoice2.MinOccurs = choice.MinOccurs;
				xmlSchemaChoice2.MaxOccurs = choice.MaxOccurs;
				this.CopyPosition(xmlSchemaChoice2, choice, true);
				foreach (XmlSchemaObject xmlSchemaObject in choice.Items)
				{
					XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
					XmlSchemaParticle xmlSchemaParticle2 = this.CannonicalizeParticle(xmlSchemaParticle, false);
					if (xmlSchemaParticle2 != XmlSchemaParticle.Empty)
					{
						if (xmlSchemaParticle2.MinOccurs == 1m && xmlSchemaParticle2.MaxOccurs == 1m && xmlSchemaParticle2 is XmlSchemaChoice)
						{
							using (XmlSchemaObjectEnumerator enumerator2 = ((XmlSchemaChoice)xmlSchemaParticle2).Items.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									XmlSchemaObject xmlSchemaObject2 = enumerator2.Current;
									XmlSchemaParticle xmlSchemaParticle3 = (XmlSchemaParticle)xmlSchemaObject2;
									xmlSchemaChoice2.Items.Add(xmlSchemaParticle3);
								}
								continue;
							}
						}
						xmlSchemaChoice2.Items.Add(xmlSchemaParticle2);
					}
				}
				choice = xmlSchemaChoice2;
			}
			if (!root && choice.Items.Count == 0)
			{
				if (choice.MinOccurs != 0m)
				{
					base.SendValidationEvent("Sch_EmptyChoice", xmlSchemaChoice, XmlSeverityType.Warning);
				}
				return XmlSchemaParticle.Empty;
			}
			if (!root && choice.Items.Count == 1 && choice.MinOccurs == 1m && choice.MaxOccurs == 1m)
			{
				return (XmlSchemaParticle)choice.Items[0];
			}
			return choice;
		}

		private XmlSchemaParticle CannonicalizeSequence(XmlSchemaSequence sequence, bool root)
		{
			if (sequence.Items.Count > 0)
			{
				XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
				xmlSchemaSequence.MinOccurs = sequence.MinOccurs;
				xmlSchemaSequence.MaxOccurs = sequence.MaxOccurs;
				this.CopyPosition(xmlSchemaSequence, sequence, true);
				foreach (XmlSchemaObject xmlSchemaObject in sequence.Items)
				{
					XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
					XmlSchemaParticle xmlSchemaParticle2 = this.CannonicalizeParticle(xmlSchemaParticle, false);
					if (xmlSchemaParticle2 != XmlSchemaParticle.Empty)
					{
						if (xmlSchemaParticle2.MinOccurs == 1m && xmlSchemaParticle2.MaxOccurs == 1m && xmlSchemaParticle2 is XmlSchemaSequence)
						{
							using (XmlSchemaObjectEnumerator enumerator2 = ((XmlSchemaSequence)xmlSchemaParticle2).Items.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									XmlSchemaObject xmlSchemaObject2 = enumerator2.Current;
									XmlSchemaParticle xmlSchemaParticle3 = (XmlSchemaParticle)xmlSchemaObject2;
									xmlSchemaSequence.Items.Add(xmlSchemaParticle3);
								}
								continue;
							}
						}
						xmlSchemaSequence.Items.Add(xmlSchemaParticle2);
					}
				}
				sequence = xmlSchemaSequence;
			}
			if (sequence.Items.Count == 0)
			{
				return XmlSchemaParticle.Empty;
			}
			if (!root && sequence.Items.Count == 1 && sequence.MinOccurs == 1m && sequence.MaxOccurs == 1m)
			{
				return (XmlSchemaParticle)sequence.Items[0];
			}
			return sequence;
		}

		private XmlSchemaParticle CannonicalizePointlessRoot(XmlSchemaParticle particle)
		{
			if (particle == null)
			{
				return null;
			}
			decimal num = 1m;
			XmlSchemaSequence xmlSchemaSequence;
			XmlSchemaChoice xmlSchemaChoice;
			XmlSchemaAll xmlSchemaAll;
			if ((xmlSchemaSequence = particle as XmlSchemaSequence) != null)
			{
				XmlSchemaObjectCollection items = xmlSchemaSequence.Items;
				int count = items.Count;
				if (count == 1 && xmlSchemaSequence.MinOccurs == num && xmlSchemaSequence.MaxOccurs == num)
				{
					return (XmlSchemaParticle)items[0];
				}
			}
			else if ((xmlSchemaChoice = particle as XmlSchemaChoice) != null)
			{
				XmlSchemaObjectCollection items2 = xmlSchemaChoice.Items;
				int count2 = items2.Count;
				if (count2 == 1)
				{
					if (xmlSchemaChoice.MinOccurs == num && xmlSchemaChoice.MaxOccurs == num)
					{
						return (XmlSchemaParticle)items2[0];
					}
				}
				else if (count2 == 0)
				{
					return XmlSchemaParticle.Empty;
				}
			}
			else if ((xmlSchemaAll = particle as XmlSchemaAll) != null)
			{
				XmlSchemaObjectCollection items3 = xmlSchemaAll.Items;
				int count3 = items3.Count;
				if (count3 == 1 && xmlSchemaAll.MinOccurs == num && xmlSchemaAll.MaxOccurs == num)
				{
					return (XmlSchemaParticle)items3[0];
				}
			}
			return particle;
		}

		private bool IsValidRestriction(XmlSchemaParticle derivedParticle, XmlSchemaParticle baseParticle)
		{
			if (derivedParticle == baseParticle)
			{
				return true;
			}
			if (derivedParticle == null || derivedParticle == XmlSchemaParticle.Empty)
			{
				return this.IsParticleEmptiable(baseParticle);
			}
			if (baseParticle == null || baseParticle == XmlSchemaParticle.Empty)
			{
				return false;
			}
			if (derivedParticle is XmlSchemaElement)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)derivedParticle;
				derivedParticle = this.CannonicalizeElement(xmlSchemaElement);
			}
			if (baseParticle is XmlSchemaElement)
			{
				XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)baseParticle;
				XmlSchemaParticle xmlSchemaParticle = this.CannonicalizeElement(xmlSchemaElement2);
				if (xmlSchemaParticle is XmlSchemaChoice)
				{
					return this.IsValidRestriction(derivedParticle, xmlSchemaParticle);
				}
				if (derivedParticle is XmlSchemaElement)
				{
					return this.IsElementFromElement((XmlSchemaElement)derivedParticle, xmlSchemaElement2);
				}
				this.restrictionErrorMsg = Res.GetString("Sch_ForbiddenDerivedParticleForElem");
				return false;
			}
			else if (baseParticle is XmlSchemaAny)
			{
				if (derivedParticle is XmlSchemaElement)
				{
					return this.IsElementFromAny((XmlSchemaElement)derivedParticle, (XmlSchemaAny)baseParticle);
				}
				if (derivedParticle is XmlSchemaAny)
				{
					return this.IsAnyFromAny((XmlSchemaAny)derivedParticle, (XmlSchemaAny)baseParticle);
				}
				return this.IsGroupBaseFromAny((XmlSchemaGroupBase)derivedParticle, (XmlSchemaAny)baseParticle);
			}
			else if (baseParticle is XmlSchemaAll)
			{
				if (derivedParticle is XmlSchemaElement)
				{
					return this.IsElementFromGroupBase((XmlSchemaElement)derivedParticle, (XmlSchemaGroupBase)baseParticle);
				}
				if (derivedParticle is XmlSchemaAll)
				{
					if (this.IsGroupBaseFromGroupBase((XmlSchemaGroupBase)derivedParticle, (XmlSchemaGroupBase)baseParticle, true))
					{
						return true;
					}
				}
				else if (derivedParticle is XmlSchemaSequence)
				{
					if (this.IsSequenceFromAll((XmlSchemaSequence)derivedParticle, (XmlSchemaAll)baseParticle))
					{
						return true;
					}
					this.restrictionErrorMsg = Res.GetString("Sch_SeqFromAll", new object[]
					{
						derivedParticle.LineNumber.ToString(NumberFormatInfo.InvariantInfo),
						derivedParticle.LinePosition.ToString(NumberFormatInfo.InvariantInfo),
						baseParticle.LineNumber.ToString(NumberFormatInfo.InvariantInfo),
						baseParticle.LinePosition.ToString(NumberFormatInfo.InvariantInfo)
					});
				}
				else if (derivedParticle is XmlSchemaChoice || derivedParticle is XmlSchemaAny)
				{
					this.restrictionErrorMsg = Res.GetString("Sch_ForbiddenDerivedParticleForAll");
				}
				return false;
			}
			else if (baseParticle is XmlSchemaChoice)
			{
				if (derivedParticle is XmlSchemaElement)
				{
					return this.IsElementFromGroupBase((XmlSchemaElement)derivedParticle, (XmlSchemaGroupBase)baseParticle);
				}
				if (derivedParticle is XmlSchemaChoice)
				{
					XmlSchemaChoice xmlSchemaChoice = baseParticle as XmlSchemaChoice;
					XmlSchemaChoice xmlSchemaChoice2 = derivedParticle as XmlSchemaChoice;
					if (xmlSchemaChoice.Parent == null || xmlSchemaChoice2.Parent == null)
					{
						return this.IsChoiceFromChoiceSubstGroup(xmlSchemaChoice2, xmlSchemaChoice);
					}
					if (this.IsGroupBaseFromGroupBase(xmlSchemaChoice2, xmlSchemaChoice, false))
					{
						return true;
					}
				}
				else if (derivedParticle is XmlSchemaSequence)
				{
					if (this.IsSequenceFromChoice((XmlSchemaSequence)derivedParticle, (XmlSchemaChoice)baseParticle))
					{
						return true;
					}
					this.restrictionErrorMsg = Res.GetString("Sch_SeqFromChoice", new object[]
					{
						derivedParticle.LineNumber.ToString(NumberFormatInfo.InvariantInfo),
						derivedParticle.LinePosition.ToString(NumberFormatInfo.InvariantInfo),
						baseParticle.LineNumber.ToString(NumberFormatInfo.InvariantInfo),
						baseParticle.LinePosition.ToString(NumberFormatInfo.InvariantInfo)
					});
				}
				else
				{
					this.restrictionErrorMsg = Res.GetString("Sch_ForbiddenDerivedParticleForChoice");
				}
				return false;
			}
			else
			{
				if (!(baseParticle is XmlSchemaSequence))
				{
					return false;
				}
				if (derivedParticle is XmlSchemaElement)
				{
					return this.IsElementFromGroupBase((XmlSchemaElement)derivedParticle, (XmlSchemaGroupBase)baseParticle);
				}
				if (derivedParticle is XmlSchemaSequence || (derivedParticle is XmlSchemaAll && ((XmlSchemaGroupBase)derivedParticle).Items.Count == 1))
				{
					if (this.IsGroupBaseFromGroupBase((XmlSchemaGroupBase)derivedParticle, (XmlSchemaGroupBase)baseParticle, true))
					{
						return true;
					}
				}
				else
				{
					this.restrictionErrorMsg = Res.GetString("Sch_ForbiddenDerivedParticleForSeq");
				}
				return false;
			}
		}

		private bool IsElementFromElement(XmlSchemaElement derivedElement, XmlSchemaElement baseElement)
		{
			if (!(derivedElement.QualifiedName == baseElement.QualifiedName) || (!baseElement.IsNillable && derivedElement.IsNillable) || (!this.IsValidOccurrenceRangeRestriction(derivedElement, baseElement) || (baseElement.FixedValue != null && !this.IsFixedEqual(baseElement.ElementDecl, derivedElement.ElementDecl))) || (derivedElement.ElementDecl.Block | baseElement.ElementDecl.Block) != derivedElement.ElementDecl.Block || derivedElement.ElementSchemaType == null || baseElement.ElementSchemaType == null || !XmlSchemaType.IsDerivedFrom(derivedElement.ElementSchemaType, baseElement.ElementSchemaType, ~(XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union)))
			{
				this.restrictionErrorMsg = Res.GetString("Sch_ElementFromElement", new object[] { derivedElement.QualifiedName, baseElement.QualifiedName });
				return false;
			}
			return true;
		}

		private bool IsElementFromAny(XmlSchemaElement derivedElement, XmlSchemaAny baseAny)
		{
			if (!baseAny.Allows(derivedElement.QualifiedName))
			{
				this.restrictionErrorMsg = Res.GetString("Sch_ElementFromAnyRule1", new object[] { derivedElement.QualifiedName.ToString() });
				return false;
			}
			if (!this.IsValidOccurrenceRangeRestriction(derivedElement, baseAny))
			{
				this.restrictionErrorMsg = Res.GetString("Sch_ElementFromAnyRule2", new object[] { derivedElement.QualifiedName.ToString() });
				return false;
			}
			return true;
		}

		private bool IsAnyFromAny(XmlSchemaAny derivedAny, XmlSchemaAny baseAny)
		{
			if (!this.IsValidOccurrenceRangeRestriction(derivedAny, baseAny))
			{
				this.restrictionErrorMsg = Res.GetString("Sch_AnyFromAnyRule1");
				return false;
			}
			if (!NamespaceList.IsSubset(derivedAny.NamespaceList, baseAny.NamespaceList))
			{
				this.restrictionErrorMsg = Res.GetString("Sch_AnyFromAnyRule2");
				return false;
			}
			if (derivedAny.ProcessContentsCorrect < baseAny.ProcessContentsCorrect)
			{
				this.restrictionErrorMsg = Res.GetString("Sch_AnyFromAnyRule3");
				return false;
			}
			return true;
		}

		private bool IsGroupBaseFromAny(XmlSchemaGroupBase derivedGroupBase, XmlSchemaAny baseAny)
		{
			decimal num;
			decimal num2;
			this.CalculateEffectiveTotalRange(derivedGroupBase, out num, out num2);
			if (!this.IsValidOccurrenceRangeRestriction(num, num2, baseAny.MinOccurs, baseAny.MaxOccurs))
			{
				this.restrictionErrorMsg = Res.GetString("Sch_GroupBaseFromAny2", new object[]
				{
					derivedGroupBase.LineNumber.ToString(NumberFormatInfo.InvariantInfo),
					derivedGroupBase.LinePosition.ToString(NumberFormatInfo.InvariantInfo),
					baseAny.LineNumber.ToString(NumberFormatInfo.InvariantInfo),
					baseAny.LinePosition.ToString(NumberFormatInfo.InvariantInfo)
				});
				return false;
			}
			string minOccursString = baseAny.MinOccursString;
			baseAny.MinOccurs = 0m;
			foreach (XmlSchemaObject xmlSchemaObject in derivedGroupBase.Items)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				if (!this.IsValidRestriction(xmlSchemaParticle, baseAny))
				{
					this.restrictionErrorMsg = Res.GetString("Sch_GroupBaseFromAny1");
					baseAny.MinOccursString = minOccursString;
					return false;
				}
			}
			baseAny.MinOccursString = minOccursString;
			return true;
		}

		private bool IsElementFromGroupBase(XmlSchemaElement derivedElement, XmlSchemaGroupBase baseGroupBase)
		{
			if (baseGroupBase is XmlSchemaSequence)
			{
				if (this.IsGroupBaseFromGroupBase(new XmlSchemaSequence
				{
					MinOccurs = 1m,
					MaxOccurs = 1m,
					Items = { derivedElement }
				}, baseGroupBase, true))
				{
					return true;
				}
				this.restrictionErrorMsg = Res.GetString("Sch_ElementFromGroupBase1", new object[]
				{
					derivedElement.QualifiedName.ToString(),
					derivedElement.LineNumber.ToString(NumberFormatInfo.InvariantInfo),
					derivedElement.LinePosition.ToString(NumberFormatInfo.InvariantInfo),
					baseGroupBase.LineNumber.ToString(NumberFormatInfo.InvariantInfo),
					baseGroupBase.LinePosition.ToString(NumberFormatInfo.InvariantInfo)
				});
			}
			else if (baseGroupBase is XmlSchemaChoice)
			{
				if (this.IsGroupBaseFromGroupBase(new XmlSchemaChoice
				{
					MinOccurs = 1m,
					MaxOccurs = 1m,
					Items = { derivedElement }
				}, baseGroupBase, false))
				{
					return true;
				}
				this.restrictionErrorMsg = Res.GetString("Sch_ElementFromGroupBase2", new object[]
				{
					derivedElement.QualifiedName.ToString(),
					derivedElement.LineNumber.ToString(NumberFormatInfo.InvariantInfo),
					derivedElement.LinePosition.ToString(NumberFormatInfo.InvariantInfo),
					baseGroupBase.LineNumber.ToString(NumberFormatInfo.InvariantInfo),
					baseGroupBase.LinePosition.ToString(NumberFormatInfo.InvariantInfo)
				});
			}
			else if (baseGroupBase is XmlSchemaAll)
			{
				if (this.IsGroupBaseFromGroupBase(new XmlSchemaAll
				{
					MinOccurs = 1m,
					MaxOccurs = 1m,
					Items = { derivedElement }
				}, baseGroupBase, true))
				{
					return true;
				}
				this.restrictionErrorMsg = Res.GetString("Sch_ElementFromGroupBase3", new object[]
				{
					derivedElement.QualifiedName.ToString(),
					derivedElement.LineNumber.ToString(NumberFormatInfo.InvariantInfo),
					derivedElement.LinePosition.ToString(NumberFormatInfo.InvariantInfo),
					baseGroupBase.LineNumber.ToString(NumberFormatInfo.InvariantInfo),
					baseGroupBase.LinePosition.ToString(NumberFormatInfo.InvariantInfo)
				});
			}
			return false;
		}

		private bool IsChoiceFromChoiceSubstGroup(XmlSchemaChoice derivedChoice, XmlSchemaChoice baseChoice)
		{
			if (!this.IsValidOccurrenceRangeRestriction(derivedChoice, baseChoice))
			{
				this.restrictionErrorMsg = Res.GetString("Sch_GroupBaseRestRangeInvalid");
				return false;
			}
			foreach (XmlSchemaObject xmlSchemaObject in derivedChoice.Items)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				if (this.GetMappingParticle(xmlSchemaParticle, baseChoice.Items) < 0)
				{
					return false;
				}
			}
			return true;
		}

		private bool IsGroupBaseFromGroupBase(XmlSchemaGroupBase derivedGroupBase, XmlSchemaGroupBase baseGroupBase, bool skipEmptableOnly)
		{
			if (!this.IsValidOccurrenceRangeRestriction(derivedGroupBase, baseGroupBase))
			{
				this.restrictionErrorMsg = Res.GetString("Sch_GroupBaseRestRangeInvalid");
				return false;
			}
			if (derivedGroupBase.Items.Count > baseGroupBase.Items.Count)
			{
				this.restrictionErrorMsg = Res.GetString("Sch_GroupBaseRestNoMap");
				return false;
			}
			int num = 0;
			foreach (XmlSchemaObject xmlSchemaObject in baseGroupBase.Items)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				if (num < derivedGroupBase.Items.Count && this.IsValidRestriction((XmlSchemaParticle)derivedGroupBase.Items[num], xmlSchemaParticle))
				{
					num++;
				}
				else if (skipEmptableOnly && !this.IsParticleEmptiable(xmlSchemaParticle))
				{
					if (this.restrictionErrorMsg == null)
					{
						this.restrictionErrorMsg = Res.GetString("Sch_GroupBaseRestNotEmptiable");
					}
					return false;
				}
			}
			return num >= derivedGroupBase.Items.Count;
		}

		private bool IsSequenceFromAll(XmlSchemaSequence derivedSequence, XmlSchemaAll baseAll)
		{
			if (!this.IsValidOccurrenceRangeRestriction(derivedSequence, baseAll) || derivedSequence.Items.Count > baseAll.Items.Count)
			{
				return false;
			}
			BitSet bitSet = new BitSet(baseAll.Items.Count);
			foreach (XmlSchemaObject xmlSchemaObject in derivedSequence.Items)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				int mappingParticle = this.GetMappingParticle(xmlSchemaParticle, baseAll.Items);
				if (mappingParticle < 0)
				{
					return false;
				}
				if (bitSet[mappingParticle])
				{
					return false;
				}
				bitSet.Set(mappingParticle);
			}
			for (int i = 0; i < baseAll.Items.Count; i++)
			{
				if (!bitSet[i] && !this.IsParticleEmptiable((XmlSchemaParticle)baseAll.Items[i]))
				{
					return false;
				}
			}
			return true;
		}

		private bool IsSequenceFromChoice(XmlSchemaSequence derivedSequence, XmlSchemaChoice baseChoice)
		{
			decimal num = derivedSequence.MinOccurs * derivedSequence.Items.Count;
			decimal num2;
			if (derivedSequence.MaxOccurs == 79228162514264337593543950335m)
			{
				num2 = decimal.MaxValue;
			}
			else
			{
				num2 = derivedSequence.MaxOccurs * derivedSequence.Items.Count;
			}
			if (!this.IsValidOccurrenceRangeRestriction(num, num2, baseChoice.MinOccurs, baseChoice.MaxOccurs) || derivedSequence.Items.Count > baseChoice.Items.Count)
			{
				return false;
			}
			foreach (XmlSchemaObject xmlSchemaObject in derivedSequence.Items)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				if (this.GetMappingParticle(xmlSchemaParticle, baseChoice.Items) < 0)
				{
					return false;
				}
			}
			return true;
		}

		private bool IsValidOccurrenceRangeRestriction(XmlSchemaParticle derivedParticle, XmlSchemaParticle baseParticle)
		{
			return this.IsValidOccurrenceRangeRestriction(derivedParticle.MinOccurs, derivedParticle.MaxOccurs, baseParticle.MinOccurs, baseParticle.MaxOccurs);
		}

		private bool IsValidOccurrenceRangeRestriction(decimal minOccurs, decimal maxOccurs, decimal baseMinOccurs, decimal baseMaxOccurs)
		{
			return baseMinOccurs <= minOccurs && maxOccurs <= baseMaxOccurs;
		}

		private int GetMappingParticle(XmlSchemaParticle particle, XmlSchemaObjectCollection collection)
		{
			for (int i = 0; i < collection.Count; i++)
			{
				if (this.IsValidRestriction(particle, (XmlSchemaParticle)collection[i]))
				{
					return i;
				}
			}
			return -1;
		}

		private bool IsParticleEmptiable(XmlSchemaParticle particle)
		{
			decimal num;
			decimal num2;
			this.CalculateEffectiveTotalRange(particle, out num, out num2);
			return num == 0m;
		}

		private void CalculateEffectiveTotalRange(XmlSchemaParticle particle, out decimal minOccurs, out decimal maxOccurs)
		{
			if (particle is XmlSchemaElement || particle is XmlSchemaAny)
			{
				minOccurs = particle.MinOccurs;
				maxOccurs = particle.MaxOccurs;
				return;
			}
			if (particle is XmlSchemaChoice)
			{
				if (((XmlSchemaChoice)particle).Items.Count == 0)
				{
					minOccurs = (maxOccurs = 0m);
					return;
				}
				minOccurs = decimal.MaxValue;
				maxOccurs = 0m;
				foreach (XmlSchemaObject xmlSchemaObject in ((XmlSchemaChoice)particle).Items)
				{
					XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
					decimal num;
					decimal num2;
					this.CalculateEffectiveTotalRange(xmlSchemaParticle, out num, out num2);
					if (num < minOccurs)
					{
						minOccurs = num;
					}
					if (num2 > maxOccurs)
					{
						maxOccurs = num2;
					}
				}
				minOccurs *= particle.MinOccurs;
				if (maxOccurs != 79228162514264337593543950335m)
				{
					if (particle.MaxOccurs == 79228162514264337593543950335m)
					{
						maxOccurs = decimal.MaxValue;
						return;
					}
					maxOccurs *= particle.MaxOccurs;
					return;
				}
			}
			else
			{
				XmlSchemaObjectCollection items = ((XmlSchemaGroupBase)particle).Items;
				if (items.Count == 0)
				{
					minOccurs = (maxOccurs = 0m);
					return;
				}
				minOccurs = 0m;
				maxOccurs = 0m;
				foreach (XmlSchemaObject xmlSchemaObject2 in items)
				{
					XmlSchemaParticle xmlSchemaParticle2 = (XmlSchemaParticle)xmlSchemaObject2;
					decimal num3;
					decimal num4;
					this.CalculateEffectiveTotalRange(xmlSchemaParticle2, out num3, out num4);
					minOccurs += num3;
					if (maxOccurs != 79228162514264337593543950335m)
					{
						if (num4 == 79228162514264337593543950335m)
						{
							maxOccurs = decimal.MaxValue;
						}
						else
						{
							maxOccurs += num4;
						}
					}
				}
				minOccurs *= particle.MinOccurs;
				if (maxOccurs != 79228162514264337593543950335m)
				{
					if (particle.MaxOccurs == 79228162514264337593543950335m)
					{
						maxOccurs = decimal.MaxValue;
						return;
					}
					maxOccurs *= particle.MaxOccurs;
				}
			}
		}

		private void PushComplexType(XmlSchemaComplexType complexType)
		{
			this.complexTypeStack.Push(complexType);
		}

		private XmlSchemaContentType GetSchemaContentType(XmlSchemaComplexType complexType, XmlSchemaComplexContent complexContent, XmlSchemaParticle particle)
		{
			if ((complexContent != null && complexContent.IsMixed) || (complexContent == null && complexType.IsMixed))
			{
				return XmlSchemaContentType.Mixed;
			}
			if (particle != null && !particle.IsEmpty)
			{
				return XmlSchemaContentType.ElementOnly;
			}
			return XmlSchemaContentType.Empty;
		}

		private void CompileAttributeGroup(XmlSchemaAttributeGroup attributeGroup)
		{
			if (attributeGroup.IsProcessing)
			{
				base.SendValidationEvent("Sch_AttributeGroupCircularRef", attributeGroup);
				return;
			}
			if (attributeGroup.AttributeUses.Count > 0)
			{
				return;
			}
			attributeGroup.IsProcessing = true;
			XmlSchemaAnyAttribute xmlSchemaAnyAttribute = attributeGroup.AnyAttribute;
			try
			{
				foreach (XmlSchemaObject xmlSchemaObject in attributeGroup.Attributes)
				{
					if (xmlSchemaObject is XmlSchemaAttribute)
					{
						XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)xmlSchemaObject;
						if (xmlSchemaAttribute.Use != XmlSchemaUse.Prohibited)
						{
							this.CompileAttribute(xmlSchemaAttribute);
							if (attributeGroup.AttributeUses[xmlSchemaAttribute.QualifiedName] == null)
							{
								attributeGroup.AttributeUses.Add(xmlSchemaAttribute.QualifiedName, xmlSchemaAttribute);
							}
							else
							{
								base.SendValidationEvent("Sch_DupAttributeUse", xmlSchemaAttribute.QualifiedName.ToString(), xmlSchemaAttribute);
							}
						}
					}
					else
					{
						XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = (XmlSchemaAttributeGroupRef)xmlSchemaObject;
						XmlSchemaAttributeGroup xmlSchemaAttributeGroup;
						if (attributeGroup.Redefined != null && xmlSchemaAttributeGroupRef.RefName == attributeGroup.Redefined.QualifiedName)
						{
							xmlSchemaAttributeGroup = attributeGroup.Redefined;
						}
						else
						{
							xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)this.attributeGroups[xmlSchemaAttributeGroupRef.RefName];
						}
						if (xmlSchemaAttributeGroup != null)
						{
							this.CompileAttributeGroup(xmlSchemaAttributeGroup);
							foreach (object obj in xmlSchemaAttributeGroup.AttributeUses.Values)
							{
								XmlSchemaAttribute xmlSchemaAttribute2 = (XmlSchemaAttribute)obj;
								if (attributeGroup.AttributeUses[xmlSchemaAttribute2.QualifiedName] == null)
								{
									attributeGroup.AttributeUses.Add(xmlSchemaAttribute2.QualifiedName, xmlSchemaAttribute2);
								}
								else
								{
									base.SendValidationEvent("Sch_DupAttributeUse", xmlSchemaAttribute2.QualifiedName.ToString(), xmlSchemaAttribute2);
								}
							}
							xmlSchemaAnyAttribute = this.CompileAnyAttributeIntersection(xmlSchemaAnyAttribute, xmlSchemaAttributeGroup.AttributeWildcard);
						}
						else
						{
							base.SendValidationEvent("Sch_UndefAttributeGroupRef", xmlSchemaAttributeGroupRef.RefName.ToString(), xmlSchemaAttributeGroupRef);
						}
					}
				}
				attributeGroup.AttributeWildcard = xmlSchemaAnyAttribute;
			}
			finally
			{
				attributeGroup.IsProcessing = false;
			}
		}

		private void CompileLocalAttributes(XmlSchemaComplexType baseType, XmlSchemaComplexType derivedType, XmlSchemaObjectCollection attributes, XmlSchemaAnyAttribute anyAttribute, XmlSchemaDerivationMethod derivedBy)
		{
			XmlSchemaAnyAttribute xmlSchemaAnyAttribute = ((baseType != null) ? baseType.AttributeWildcard : null);
			foreach (XmlSchemaObject xmlSchemaObject in attributes)
			{
				if (xmlSchemaObject is XmlSchemaAttribute)
				{
					XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)xmlSchemaObject;
					if (xmlSchemaAttribute.Use != XmlSchemaUse.Prohibited)
					{
						this.CompileAttribute(xmlSchemaAttribute);
					}
					if (xmlSchemaAttribute.Use != XmlSchemaUse.Prohibited || (xmlSchemaAttribute.Use == XmlSchemaUse.Prohibited && derivedBy == XmlSchemaDerivationMethod.Restriction && baseType != XmlSchemaComplexType.AnyType))
					{
						if (derivedType.AttributeUses[xmlSchemaAttribute.QualifiedName] == null)
						{
							derivedType.AttributeUses.Add(xmlSchemaAttribute.QualifiedName, xmlSchemaAttribute);
						}
						else
						{
							base.SendValidationEvent("Sch_DupAttributeUse", xmlSchemaAttribute.QualifiedName.ToString(), xmlSchemaAttribute);
						}
					}
					else
					{
						base.SendValidationEvent("Sch_AttributeIgnored", xmlSchemaAttribute.QualifiedName.ToString(), xmlSchemaAttribute, XmlSeverityType.Warning);
					}
				}
				else
				{
					XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = (XmlSchemaAttributeGroupRef)xmlSchemaObject;
					XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)this.attributeGroups[xmlSchemaAttributeGroupRef.RefName];
					if (xmlSchemaAttributeGroup != null)
					{
						this.CompileAttributeGroup(xmlSchemaAttributeGroup);
						foreach (object obj in xmlSchemaAttributeGroup.AttributeUses.Values)
						{
							XmlSchemaAttribute xmlSchemaAttribute2 = (XmlSchemaAttribute)obj;
							if (xmlSchemaAttribute2.Use != XmlSchemaUse.Prohibited || (xmlSchemaAttribute2.Use == XmlSchemaUse.Prohibited && derivedBy == XmlSchemaDerivationMethod.Restriction && baseType != XmlSchemaComplexType.AnyType))
							{
								if (derivedType.AttributeUses[xmlSchemaAttribute2.QualifiedName] == null)
								{
									derivedType.AttributeUses.Add(xmlSchemaAttribute2.QualifiedName, xmlSchemaAttribute2);
								}
								else
								{
									base.SendValidationEvent("Sch_DupAttributeUse", xmlSchemaAttribute2.QualifiedName.ToString(), xmlSchemaAttributeGroupRef);
								}
							}
							else
							{
								base.SendValidationEvent("Sch_AttributeIgnored", xmlSchemaAttribute2.QualifiedName.ToString(), xmlSchemaAttribute2, XmlSeverityType.Warning);
							}
						}
						anyAttribute = this.CompileAnyAttributeIntersection(anyAttribute, xmlSchemaAttributeGroup.AttributeWildcard);
					}
					else
					{
						base.SendValidationEvent("Sch_UndefAttributeGroupRef", xmlSchemaAttributeGroupRef.RefName.ToString(), xmlSchemaAttributeGroupRef);
					}
				}
			}
			if (baseType != null)
			{
				if (derivedBy == XmlSchemaDerivationMethod.Extension)
				{
					derivedType.SetAttributeWildcard(this.CompileAnyAttributeUnion(anyAttribute, xmlSchemaAnyAttribute));
					using (IEnumerator enumerator3 = baseType.AttributeUses.Values.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							object obj2 = enumerator3.Current;
							XmlSchemaAttribute xmlSchemaAttribute3 = (XmlSchemaAttribute)obj2;
							XmlSchemaAttribute xmlSchemaAttribute4 = (XmlSchemaAttribute)derivedType.AttributeUses[xmlSchemaAttribute3.QualifiedName];
							if (xmlSchemaAttribute4 == null)
							{
								derivedType.AttributeUses.Add(xmlSchemaAttribute3.QualifiedName, xmlSchemaAttribute3);
							}
							else if (xmlSchemaAttribute3.Use != XmlSchemaUse.Prohibited && xmlSchemaAttribute4.AttributeSchemaType != xmlSchemaAttribute3.AttributeSchemaType)
							{
								base.SendValidationEvent("Sch_InvalidAttributeExtension", xmlSchemaAttribute4);
							}
						}
						return;
					}
				}
				if (anyAttribute != null && (xmlSchemaAnyAttribute == null || !XmlSchemaAnyAttribute.IsSubset(anyAttribute, xmlSchemaAnyAttribute) || !this.IsProcessContentsRestricted(baseType, anyAttribute, xmlSchemaAnyAttribute)))
				{
					base.SendValidationEvent("Sch_InvalidAnyAttributeRestriction", derivedType);
				}
				else
				{
					derivedType.SetAttributeWildcard(anyAttribute);
				}
				foreach (object obj3 in baseType.AttributeUses.Values)
				{
					XmlSchemaAttribute xmlSchemaAttribute5 = (XmlSchemaAttribute)obj3;
					XmlSchemaAttribute xmlSchemaAttribute6 = (XmlSchemaAttribute)derivedType.AttributeUses[xmlSchemaAttribute5.QualifiedName];
					if (xmlSchemaAttribute6 == null)
					{
						derivedType.AttributeUses.Add(xmlSchemaAttribute5.QualifiedName, xmlSchemaAttribute5);
					}
					else if (xmlSchemaAttribute5.Use == XmlSchemaUse.Prohibited && xmlSchemaAttribute6.Use != XmlSchemaUse.Prohibited)
					{
						base.SendValidationEvent("Sch_AttributeRestrictionProhibited", xmlSchemaAttribute6);
					}
					else if (xmlSchemaAttribute5.Use == XmlSchemaUse.Required && xmlSchemaAttribute6.Use != XmlSchemaUse.Required)
					{
						base.SendValidationEvent("Sch_AttributeUseInvalid", xmlSchemaAttribute6);
					}
					else if (xmlSchemaAttribute6.Use != XmlSchemaUse.Prohibited)
					{
						if (xmlSchemaAttribute5.AttributeSchemaType == null || xmlSchemaAttribute6.AttributeSchemaType == null || !XmlSchemaType.IsDerivedFrom(xmlSchemaAttribute6.AttributeSchemaType, xmlSchemaAttribute5.AttributeSchemaType, XmlSchemaDerivationMethod.Empty))
						{
							base.SendValidationEvent("Sch_AttributeRestrictionInvalid", xmlSchemaAttribute6);
						}
						else if (!this.IsFixedEqual(xmlSchemaAttribute5.AttDef, xmlSchemaAttribute6.AttDef))
						{
							base.SendValidationEvent("Sch_AttributeFixedInvalid", xmlSchemaAttribute6);
						}
					}
				}
				using (IEnumerator enumerator5 = derivedType.AttributeUses.Values.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						object obj4 = enumerator5.Current;
						XmlSchemaAttribute xmlSchemaAttribute7 = (XmlSchemaAttribute)obj4;
						if ((XmlSchemaAttribute)baseType.AttributeUses[xmlSchemaAttribute7.QualifiedName] == null && (xmlSchemaAnyAttribute == null || !xmlSchemaAnyAttribute.Allows(xmlSchemaAttribute7.QualifiedName)))
						{
							base.SendValidationEvent("Sch_AttributeRestrictionInvalidFromWildcard", xmlSchemaAttribute7);
						}
					}
					return;
				}
			}
			derivedType.SetAttributeWildcard(anyAttribute);
		}

		private void CheckAtrributeGroupRestriction(XmlSchemaAttributeGroup baseAttributeGroup, XmlSchemaAttributeGroup derivedAttributeGroup)
		{
			XmlSchemaAnyAttribute attributeWildcard = baseAttributeGroup.AttributeWildcard;
			XmlSchemaAnyAttribute attributeWildcard2 = derivedAttributeGroup.AttributeWildcard;
			if (attributeWildcard2 != null && (attributeWildcard == null || !XmlSchemaAnyAttribute.IsSubset(attributeWildcard2, attributeWildcard) || !this.IsProcessContentsRestricted(null, attributeWildcard2, attributeWildcard)))
			{
				base.SendValidationEvent("Sch_InvalidAnyAttributeRestriction", derivedAttributeGroup);
			}
			foreach (object obj in baseAttributeGroup.AttributeUses.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj;
				XmlSchemaAttribute xmlSchemaAttribute2 = (XmlSchemaAttribute)derivedAttributeGroup.AttributeUses[xmlSchemaAttribute.QualifiedName];
				if (xmlSchemaAttribute2 != null)
				{
					if (xmlSchemaAttribute.Use == XmlSchemaUse.Prohibited && xmlSchemaAttribute2.Use != XmlSchemaUse.Prohibited)
					{
						base.SendValidationEvent("Sch_AttributeRestrictionProhibited", xmlSchemaAttribute2);
					}
					else if (xmlSchemaAttribute.Use == XmlSchemaUse.Required && xmlSchemaAttribute2.Use != XmlSchemaUse.Required)
					{
						base.SendValidationEvent("Sch_AttributeUseInvalid", xmlSchemaAttribute2);
					}
					else if (xmlSchemaAttribute2.Use != XmlSchemaUse.Prohibited)
					{
						if (xmlSchemaAttribute.AttributeSchemaType == null || xmlSchemaAttribute2.AttributeSchemaType == null || !XmlSchemaType.IsDerivedFrom(xmlSchemaAttribute2.AttributeSchemaType, xmlSchemaAttribute.AttributeSchemaType, XmlSchemaDerivationMethod.Empty))
						{
							base.SendValidationEvent("Sch_AttributeRestrictionInvalid", xmlSchemaAttribute2);
						}
						else if (!this.IsFixedEqual(xmlSchemaAttribute.AttDef, xmlSchemaAttribute2.AttDef))
						{
							base.SendValidationEvent("Sch_AttributeFixedInvalid", xmlSchemaAttribute2);
						}
					}
				}
				else if (xmlSchemaAttribute.Use == XmlSchemaUse.Required)
				{
					base.SendValidationEvent("Sch_NoDerivedAttribute", xmlSchemaAttribute.QualifiedName.ToString(), baseAttributeGroup.QualifiedName.ToString(), derivedAttributeGroup);
				}
			}
			foreach (object obj2 in derivedAttributeGroup.AttributeUses.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute3 = (XmlSchemaAttribute)obj2;
				if ((XmlSchemaAttribute)baseAttributeGroup.AttributeUses[xmlSchemaAttribute3.QualifiedName] == null && (attributeWildcard == null || !attributeWildcard.Allows(xmlSchemaAttribute3.QualifiedName)))
				{
					base.SendValidationEvent("Sch_AttributeRestrictionInvalidFromWildcard", xmlSchemaAttribute3);
				}
			}
		}

		private bool IsProcessContentsRestricted(XmlSchemaComplexType baseType, XmlSchemaAnyAttribute derivedAttributeWildcard, XmlSchemaAnyAttribute baseAttributeWildcard)
		{
			return baseType == XmlSchemaComplexType.AnyType || derivedAttributeWildcard.ProcessContentsCorrect >= baseAttributeWildcard.ProcessContentsCorrect;
		}

		private XmlSchemaAnyAttribute CompileAnyAttributeUnion(XmlSchemaAnyAttribute a, XmlSchemaAnyAttribute b)
		{
			if (a == null)
			{
				return b;
			}
			if (b == null)
			{
				return a;
			}
			XmlSchemaAnyAttribute xmlSchemaAnyAttribute = XmlSchemaAnyAttribute.Union(a, b, false);
			if (xmlSchemaAnyAttribute == null)
			{
				base.SendValidationEvent("Sch_UnexpressibleAnyAttribute", a);
			}
			return xmlSchemaAnyAttribute;
		}

		private XmlSchemaAnyAttribute CompileAnyAttributeIntersection(XmlSchemaAnyAttribute a, XmlSchemaAnyAttribute b)
		{
			if (a == null)
			{
				return b;
			}
			if (b == null)
			{
				return a;
			}
			XmlSchemaAnyAttribute xmlSchemaAnyAttribute = XmlSchemaAnyAttribute.Intersection(a, b, false);
			if (xmlSchemaAnyAttribute == null)
			{
				base.SendValidationEvent("Sch_UnexpressibleAnyAttribute", a);
			}
			return xmlSchemaAnyAttribute;
		}

		private void CompileAttribute(XmlSchemaAttribute xa)
		{
			if (xa.IsProcessing)
			{
				base.SendValidationEvent("Sch_AttributeCircularRef", xa);
				return;
			}
			if (xa.AttDef != null)
			{
				return;
			}
			xa.IsProcessing = true;
			try
			{
				SchemaAttDef schemaAttDef;
				if (!xa.RefName.IsEmpty)
				{
					XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)this.attributes[xa.RefName];
					if (xmlSchemaAttribute == null)
					{
						throw new XmlSchemaException("Sch_UndeclaredAttribute", xa.RefName.ToString(), xa);
					}
					this.CompileAttribute(xmlSchemaAttribute);
					if (xmlSchemaAttribute.AttDef == null)
					{
						throw new XmlSchemaException("Sch_RefInvalidAttribute", xa.RefName.ToString(), xa);
					}
					schemaAttDef = xmlSchemaAttribute.AttDef.Clone();
					XmlSchemaDatatype datatype = schemaAttDef.Datatype;
					if (datatype != null)
					{
						if (xmlSchemaAttribute.FixedValue == null && xmlSchemaAttribute.DefaultValue == null)
						{
							this.SetDefaultFixed(xa, schemaAttDef);
						}
						else if (xmlSchemaAttribute.FixedValue != null)
						{
							if (xa.DefaultValue != null)
							{
								throw new XmlSchemaException("Sch_FixedDefaultInRef", xa.RefName.ToString(), xa);
							}
							if (xa.FixedValue != null)
							{
								object obj = datatype.ParseValue(xa.FixedValue, base.NameTable, new SchemaNamespaceManager(xa), true);
								if (!datatype.IsEqual(schemaAttDef.DefaultValueTyped, obj))
								{
									throw new XmlSchemaException("Sch_FixedInRef", xa.RefName.ToString(), xa);
								}
							}
						}
					}
					xa.SetAttributeType(xmlSchemaAttribute.AttributeSchemaType);
				}
				else
				{
					schemaAttDef = new SchemaAttDef(xa.QualifiedName, xa.Prefix);
					if (xa.SchemaType != null)
					{
						this.CompileSimpleType(xa.SchemaType);
						xa.SetAttributeType(xa.SchemaType);
						schemaAttDef.SchemaType = xa.SchemaType;
						schemaAttDef.Datatype = xa.SchemaType.Datatype;
					}
					else if (!xa.SchemaTypeName.IsEmpty)
					{
						XmlSchemaSimpleType simpleType = this.GetSimpleType(xa.SchemaTypeName);
						if (simpleType == null)
						{
							throw new XmlSchemaException("Sch_UndeclaredSimpleType", xa.SchemaTypeName.ToString(), xa);
						}
						xa.SetAttributeType(simpleType);
						schemaAttDef.Datatype = simpleType.Datatype;
						schemaAttDef.SchemaType = simpleType;
					}
					else
					{
						schemaAttDef.SchemaType = DatatypeImplementation.AnySimpleType;
						schemaAttDef.Datatype = DatatypeImplementation.AnySimpleType.Datatype;
						xa.SetAttributeType(DatatypeImplementation.AnySimpleType);
					}
					if (schemaAttDef.Datatype != null)
					{
						schemaAttDef.Datatype.VerifySchemaValid(this.notations, xa);
					}
					this.SetDefaultFixed(xa, schemaAttDef);
				}
				schemaAttDef.SchemaAttribute = xa;
				xa.AttDef = schemaAttDef;
			}
			catch (XmlSchemaException ex)
			{
				if (ex.SourceSchemaObject == null)
				{
					ex.SetSource(xa);
				}
				base.SendValidationEvent(ex);
				xa.AttDef = SchemaAttDef.Empty;
			}
			finally
			{
				xa.IsProcessing = false;
			}
		}

		private void SetDefaultFixed(XmlSchemaAttribute xa, SchemaAttDef decl)
		{
			if (xa.DefaultValue != null || xa.FixedValue != null)
			{
				if (xa.DefaultValue != null)
				{
					decl.Presence = SchemaDeclBase.Use.Default;
					decl.DefaultValueRaw = (decl.DefaultValueExpanded = xa.DefaultValue);
				}
				else
				{
					if (xa.Use == XmlSchemaUse.Required)
					{
						decl.Presence = SchemaDeclBase.Use.RequiredFixed;
					}
					else
					{
						decl.Presence = SchemaDeclBase.Use.Fixed;
					}
					decl.DefaultValueRaw = (decl.DefaultValueExpanded = xa.FixedValue);
				}
				if (decl.Datatype != null)
				{
					if (decl.Datatype.TypeCode == XmlTypeCode.Id)
					{
						base.SendValidationEvent("Sch_DefaultIdValue", xa);
						return;
					}
					decl.DefaultValueTyped = decl.Datatype.ParseValue(decl.DefaultValueRaw, base.NameTable, new SchemaNamespaceManager(xa), true);
					return;
				}
			}
			else
			{
				switch (xa.Use)
				{
				case XmlSchemaUse.None:
				case XmlSchemaUse.Optional:
					decl.Presence = SchemaDeclBase.Use.Implied;
					return;
				case XmlSchemaUse.Prohibited:
					break;
				case XmlSchemaUse.Required:
					decl.Presence = SchemaDeclBase.Use.Required;
					break;
				default:
					return;
				}
			}
		}

		private void CompileIdentityConstraint(XmlSchemaIdentityConstraint xi)
		{
			if (xi.IsProcessing)
			{
				xi.CompiledConstraint = CompiledIdentityConstraint.Empty;
				base.SendValidationEvent("Sch_IdentityConstraintCircularRef", xi);
				return;
			}
			if (xi.CompiledConstraint != null)
			{
				return;
			}
			xi.IsProcessing = true;
			try
			{
				SchemaNamespaceManager schemaNamespaceManager = new SchemaNamespaceManager(xi);
				CompiledIdentityConstraint compiledIdentityConstraint = new CompiledIdentityConstraint(xi, schemaNamespaceManager);
				if (xi is XmlSchemaKeyref)
				{
					XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint = (XmlSchemaIdentityConstraint)this.identityConstraints[((XmlSchemaKeyref)xi).Refer];
					if (xmlSchemaIdentityConstraint == null)
					{
						throw new XmlSchemaException("Sch_UndeclaredIdentityConstraint", ((XmlSchemaKeyref)xi).Refer.ToString(), xi);
					}
					this.CompileIdentityConstraint(xmlSchemaIdentityConstraint);
					if (xmlSchemaIdentityConstraint.CompiledConstraint == null)
					{
						throw new XmlSchemaException("Sch_RefInvalidIdentityConstraint", ((XmlSchemaKeyref)xi).Refer.ToString(), xi);
					}
					if (xmlSchemaIdentityConstraint.Fields.Count != xi.Fields.Count)
					{
						throw new XmlSchemaException("Sch_RefInvalidCardin", xi.QualifiedName.ToString(), xi);
					}
					if (xmlSchemaIdentityConstraint.CompiledConstraint.Role == CompiledIdentityConstraint.ConstraintRole.Keyref)
					{
						throw new XmlSchemaException("Sch_ReftoKeyref", xi.QualifiedName.ToString(), xi);
					}
				}
				xi.CompiledConstraint = compiledIdentityConstraint;
			}
			catch (XmlSchemaException ex)
			{
				if (ex.SourceSchemaObject == null)
				{
					ex.SetSource(xi);
				}
				base.SendValidationEvent(ex);
				xi.CompiledConstraint = CompiledIdentityConstraint.Empty;
			}
			finally
			{
				xi.IsProcessing = false;
			}
		}

		private void CompileElement(XmlSchemaElement xe)
		{
			if (xe.IsProcessing)
			{
				base.SendValidationEvent("Sch_ElementCircularRef", xe);
				return;
			}
			if (xe.ElementDecl != null)
			{
				return;
			}
			xe.IsProcessing = true;
			SchemaElementDecl schemaElementDecl = null;
			try
			{
				if (!xe.RefName.IsEmpty)
				{
					XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)this.elements[xe.RefName];
					if (xmlSchemaElement == null)
					{
						throw new XmlSchemaException("Sch_UndeclaredElement", xe.RefName.ToString(), xe);
					}
					this.CompileElement(xmlSchemaElement);
					if (xmlSchemaElement.ElementDecl == null)
					{
						throw new XmlSchemaException("Sch_RefInvalidElement", xe.RefName.ToString(), xe);
					}
					xe.SetElementType(xmlSchemaElement.ElementSchemaType);
					schemaElementDecl = xmlSchemaElement.ElementDecl.Clone();
				}
				else
				{
					if (xe.SchemaType != null)
					{
						xe.SetElementType(xe.SchemaType);
					}
					else if (!xe.SchemaTypeName.IsEmpty)
					{
						xe.SetElementType(this.GetAnySchemaType(xe.SchemaTypeName));
						if (xe.ElementSchemaType == null)
						{
							throw new XmlSchemaException("Sch_UndeclaredType", xe.SchemaTypeName.ToString(), xe);
						}
					}
					else if (!xe.SubstitutionGroup.IsEmpty)
					{
						XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)this.elements[xe.SubstitutionGroup];
						if (xmlSchemaElement2 == null)
						{
							throw new XmlSchemaException("Sch_UndeclaredEquivClass", xe.SubstitutionGroup.Name.ToString(CultureInfo.InvariantCulture), xe);
						}
						if (xmlSchemaElement2.IsProcessing)
						{
							return;
						}
						this.CompileElement(xmlSchemaElement2);
						if (xmlSchemaElement2.ElementDecl == null)
						{
							xe.SetElementType(XmlSchemaComplexType.AnyType);
							schemaElementDecl = XmlSchemaComplexType.AnyType.ElementDecl.Clone();
						}
						else
						{
							xe.SetElementType(xmlSchemaElement2.ElementSchemaType);
							schemaElementDecl = xmlSchemaElement2.ElementDecl.Clone();
						}
					}
					else
					{
						xe.SetElementType(XmlSchemaComplexType.AnyType);
						schemaElementDecl = XmlSchemaComplexType.AnyType.ElementDecl.Clone();
					}
					if (schemaElementDecl == null)
					{
						if (xe.ElementSchemaType is XmlSchemaComplexType)
						{
							XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)xe.ElementSchemaType;
							this.CompileComplexType(xmlSchemaComplexType);
							if (xmlSchemaComplexType.ElementDecl != null)
							{
								schemaElementDecl = xmlSchemaComplexType.ElementDecl.Clone();
							}
						}
						else if (xe.ElementSchemaType is XmlSchemaSimpleType)
						{
							XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)xe.ElementSchemaType;
							this.CompileSimpleType(xmlSchemaSimpleType);
							if (xmlSchemaSimpleType.ElementDecl != null)
							{
								schemaElementDecl = xmlSchemaSimpleType.ElementDecl.Clone();
							}
						}
					}
					schemaElementDecl.Name = xe.QualifiedName;
					schemaElementDecl.IsAbstract = xe.IsAbstract;
					XmlSchemaComplexType xmlSchemaComplexType2 = xe.ElementSchemaType as XmlSchemaComplexType;
					if (xmlSchemaComplexType2 != null)
					{
						schemaElementDecl.IsAbstract |= xmlSchemaComplexType2.IsAbstract;
					}
					schemaElementDecl.IsNillable = xe.IsNillable;
					schemaElementDecl.Block |= xe.BlockResolved;
				}
				if (schemaElementDecl.Datatype != null)
				{
					schemaElementDecl.Datatype.VerifySchemaValid(this.notations, xe);
				}
				if ((xe.DefaultValue != null || xe.FixedValue != null) && schemaElementDecl.ContentValidator != null)
				{
					if (schemaElementDecl.ContentValidator.ContentType != XmlSchemaContentType.TextOnly && (schemaElementDecl.ContentValidator.ContentType != XmlSchemaContentType.Mixed || !schemaElementDecl.ContentValidator.IsEmptiable))
					{
						throw new XmlSchemaException("Sch_ElementCannotHaveValue", xe);
					}
					if (xe.DefaultValue != null)
					{
						schemaElementDecl.Presence = SchemaDeclBase.Use.Default;
						schemaElementDecl.DefaultValueRaw = xe.DefaultValue;
					}
					else
					{
						schemaElementDecl.Presence = SchemaDeclBase.Use.Fixed;
						schemaElementDecl.DefaultValueRaw = xe.FixedValue;
					}
					if (schemaElementDecl.Datatype != null)
					{
						if (schemaElementDecl.Datatype.TypeCode == XmlTypeCode.Id)
						{
							base.SendValidationEvent("Sch_DefaultIdValue", xe);
						}
						else
						{
							schemaElementDecl.DefaultValueTyped = schemaElementDecl.Datatype.ParseValue(schemaElementDecl.DefaultValueRaw, base.NameTable, new SchemaNamespaceManager(xe), true);
						}
					}
					else
					{
						schemaElementDecl.DefaultValueTyped = DatatypeImplementation.AnySimpleType.Datatype.ParseValue(schemaElementDecl.DefaultValueRaw, base.NameTable, new SchemaNamespaceManager(xe));
					}
				}
				if (xe.HasConstraints)
				{
					XmlSchemaObjectCollection constraints = xe.Constraints;
					CompiledIdentityConstraint[] array = new CompiledIdentityConstraint[constraints.Count];
					int num = 0;
					foreach (XmlSchemaObject xmlSchemaObject in constraints)
					{
						XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint = (XmlSchemaIdentityConstraint)xmlSchemaObject;
						this.CompileIdentityConstraint(xmlSchemaIdentityConstraint);
						array[num++] = xmlSchemaIdentityConstraint.CompiledConstraint;
					}
					schemaElementDecl.Constraints = array;
				}
				schemaElementDecl.SchemaElement = xe;
				xe.ElementDecl = schemaElementDecl;
			}
			catch (XmlSchemaException ex)
			{
				if (ex.SourceSchemaObject == null)
				{
					ex.SetSource(xe);
				}
				base.SendValidationEvent(ex);
				xe.ElementDecl = SchemaElementDecl.Empty;
			}
			finally
			{
				xe.IsProcessing = false;
			}
		}

		private ContentValidator CompileComplexContent(XmlSchemaComplexType complexType)
		{
			if (complexType.ContentType == XmlSchemaContentType.Empty)
			{
				return ContentValidator.Empty;
			}
			if (complexType.ContentType == XmlSchemaContentType.TextOnly)
			{
				return ContentValidator.TextOnly;
			}
			XmlSchemaParticle contentTypeParticle = complexType.ContentTypeParticle;
			if (contentTypeParticle == null || contentTypeParticle == XmlSchemaParticle.Empty)
			{
				if (complexType.ContentType == XmlSchemaContentType.ElementOnly)
				{
					return ContentValidator.Empty;
				}
				return ContentValidator.Mixed;
			}
			else
			{
				this.PushComplexType(complexType);
				if (contentTypeParticle is XmlSchemaAll)
				{
					XmlSchemaAll xmlSchemaAll = (XmlSchemaAll)contentTypeParticle;
					AllElementsContentValidator allElementsContentValidator = new AllElementsContentValidator(complexType.ContentType, xmlSchemaAll.Items.Count, xmlSchemaAll.MinOccurs == 0m);
					foreach (XmlSchemaObject xmlSchemaObject in xmlSchemaAll.Items)
					{
						XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)xmlSchemaObject;
						if (!allElementsContentValidator.AddElement(xmlSchemaElement.QualifiedName, xmlSchemaElement, xmlSchemaElement.MinOccurs == 0m))
						{
							base.SendValidationEvent("Sch_DupElement", xmlSchemaElement.QualifiedName.ToString(), xmlSchemaElement);
						}
					}
					return allElementsContentValidator;
				}
				ParticleContentValidator particleContentValidator = new ParticleContentValidator(complexType.ContentType, base.CompilationSettings.EnableUpaCheck);
				ContentValidator contentValidator;
				try
				{
					particleContentValidator.Start();
					complexType.HasWildCard = this.BuildParticleContentModel(particleContentValidator, contentTypeParticle);
					contentValidator = particleContentValidator.Finish(true);
				}
				catch (UpaException ex)
				{
					if (ex.Particle1 is XmlSchemaElement)
					{
						if (ex.Particle2 is XmlSchemaElement)
						{
							base.SendValidationEvent("Sch_NonDeterministic", ((XmlSchemaElement)ex.Particle1).QualifiedName.ToString(), (XmlSchemaElement)ex.Particle2);
						}
						else
						{
							base.SendValidationEvent("Sch_NonDeterministicAnyEx", ((XmlSchemaAny)ex.Particle2).ResolvedNamespace, ((XmlSchemaElement)ex.Particle1).QualifiedName.ToString(), (XmlSchemaAny)ex.Particle2);
						}
					}
					else if (ex.Particle2 is XmlSchemaElement)
					{
						base.SendValidationEvent("Sch_NonDeterministicAnyEx", ((XmlSchemaAny)ex.Particle1).ResolvedNamespace, ((XmlSchemaElement)ex.Particle2).QualifiedName.ToString(), (XmlSchemaElement)ex.Particle2);
					}
					else
					{
						base.SendValidationEvent("Sch_NonDeterministicAnyAny", ((XmlSchemaAny)ex.Particle1).ResolvedNamespace, ((XmlSchemaAny)ex.Particle2).ResolvedNamespace, (XmlSchemaAny)ex.Particle2);
					}
					contentValidator = XmlSchemaComplexType.AnyTypeContentValidator;
				}
				catch (NotSupportedException)
				{
					base.SendValidationEvent("Sch_ComplexContentModel", complexType, XmlSeverityType.Warning);
					contentValidator = XmlSchemaComplexType.AnyTypeContentValidator;
				}
				return contentValidator;
			}
		}

		private bool BuildParticleContentModel(ParticleContentValidator contentValidator, XmlSchemaParticle particle)
		{
			bool flag = false;
			if (particle is XmlSchemaElement)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)particle;
				contentValidator.AddName(xmlSchemaElement.QualifiedName, xmlSchemaElement);
			}
			else if (particle is XmlSchemaAny)
			{
				flag = true;
				XmlSchemaAny xmlSchemaAny = (XmlSchemaAny)particle;
				contentValidator.AddNamespaceList(xmlSchemaAny.NamespaceList, xmlSchemaAny);
			}
			else if (particle is XmlSchemaGroupBase)
			{
				XmlSchemaObjectCollection items = ((XmlSchemaGroupBase)particle).Items;
				bool flag2 = particle is XmlSchemaChoice;
				contentValidator.OpenGroup();
				bool flag3 = true;
				foreach (XmlSchemaObject xmlSchemaObject in items)
				{
					XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
					if (flag3)
					{
						flag3 = false;
					}
					else if (flag2)
					{
						contentValidator.AddChoice();
					}
					else
					{
						contentValidator.AddSequence();
					}
					flag = this.BuildParticleContentModel(contentValidator, xmlSchemaParticle);
				}
				contentValidator.CloseGroup();
			}
			if (!(particle.MinOccurs == 1m) || !(particle.MaxOccurs == 1m))
			{
				if (particle.MinOccurs == 0m && particle.MaxOccurs == 1m)
				{
					contentValidator.AddQMark();
				}
				else if (particle.MinOccurs == 0m && particle.MaxOccurs == 79228162514264337593543950335m)
				{
					contentValidator.AddStar();
				}
				else if (particle.MinOccurs == 1m && particle.MaxOccurs == 79228162514264337593543950335m)
				{
					contentValidator.AddPlus();
				}
				else
				{
					contentValidator.AddLeafRange(particle.MinOccurs, particle.MaxOccurs);
				}
			}
			return flag;
		}

		private void CompileParticleElements(XmlSchemaComplexType complexType, XmlSchemaParticle particle)
		{
			if (particle is XmlSchemaElement)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)particle;
				this.CompileElement(xmlSchemaElement);
				if (complexType.LocalElements[xmlSchemaElement.QualifiedName] == null)
				{
					complexType.LocalElements.Add(xmlSchemaElement.QualifiedName, xmlSchemaElement);
					return;
				}
				complexType.HasDuplicateDecls = true;
				XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)complexType.LocalElements[xmlSchemaElement.QualifiedName];
				if (xmlSchemaElement2.ElementSchemaType != xmlSchemaElement.ElementSchemaType)
				{
					base.SendValidationEvent("Sch_ElementTypeCollision", particle);
					return;
				}
			}
			else if (particle is XmlSchemaGroupBase)
			{
				XmlSchemaObjectCollection items = ((XmlSchemaGroupBase)particle).Items;
				foreach (XmlSchemaObject xmlSchemaObject in items)
				{
					XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
					this.CompileParticleElements(complexType, xmlSchemaParticle);
				}
			}
		}

		private void CompileParticleElements(XmlSchemaParticle particle)
		{
			if (particle is XmlSchemaElement)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)particle;
				this.CompileElement(xmlSchemaElement);
				return;
			}
			if (particle is XmlSchemaGroupBase)
			{
				XmlSchemaObjectCollection items = ((XmlSchemaGroupBase)particle).Items;
				foreach (XmlSchemaObject xmlSchemaObject in items)
				{
					XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
					this.CompileParticleElements(xmlSchemaParticle);
				}
			}
		}

		private void CompileComplexTypeElements(XmlSchemaComplexType complexType)
		{
			if (complexType.IsProcessing)
			{
				base.SendValidationEvent("Sch_TypeCircularRef", complexType);
				return;
			}
			complexType.IsProcessing = true;
			try
			{
				if (complexType.ContentTypeParticle != XmlSchemaParticle.Empty)
				{
					this.CompileParticleElements(complexType, complexType.ContentTypeParticle);
				}
			}
			finally
			{
				complexType.IsProcessing = false;
			}
		}

		private XmlSchemaSimpleType GetSimpleType(XmlQualifiedName name)
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = this.schemaTypes[name] as XmlSchemaSimpleType;
			if (xmlSchemaSimpleType != null)
			{
				this.CompileSimpleType(xmlSchemaSimpleType);
			}
			else
			{
				xmlSchemaSimpleType = DatatypeImplementation.GetSimpleTypeFromXsdType(name);
			}
			return xmlSchemaSimpleType;
		}

		private XmlSchemaComplexType GetComplexType(XmlQualifiedName name)
		{
			XmlSchemaComplexType xmlSchemaComplexType = this.schemaTypes[name] as XmlSchemaComplexType;
			if (xmlSchemaComplexType != null)
			{
				this.CompileComplexType(xmlSchemaComplexType);
			}
			return xmlSchemaComplexType;
		}

		private XmlSchemaType GetAnySchemaType(XmlQualifiedName name)
		{
			XmlSchemaType xmlSchemaType = (XmlSchemaType)this.schemaTypes[name];
			if (xmlSchemaType != null)
			{
				if (xmlSchemaType is XmlSchemaComplexType)
				{
					this.CompileComplexType((XmlSchemaComplexType)xmlSchemaType);
				}
				else
				{
					this.CompileSimpleType((XmlSchemaSimpleType)xmlSchemaType);
				}
				return xmlSchemaType;
			}
			return DatatypeImplementation.GetSimpleTypeFromXsdType(name);
		}

		private void CopyPosition(XmlSchemaObject to, XmlSchemaObject from, bool copyParent)
		{
			to.SourceUri = from.SourceUri;
			to.LinePosition = from.LinePosition;
			to.LineNumber = from.LineNumber;
			if (copyParent)
			{
				to.Parent = from.Parent;
			}
		}

		private bool IsFixedEqual(SchemaDeclBase baseDecl, SchemaDeclBase derivedDecl)
		{
			if (baseDecl.Presence == SchemaDeclBase.Use.Fixed || baseDecl.Presence == SchemaDeclBase.Use.RequiredFixed)
			{
				object defaultValueTyped = baseDecl.DefaultValueTyped;
				object defaultValueTyped2 = derivedDecl.DefaultValueTyped;
				if (derivedDecl.Presence != SchemaDeclBase.Use.Fixed && derivedDecl.Presence != SchemaDeclBase.Use.RequiredFixed)
				{
					return false;
				}
				XmlSchemaDatatype datatype = baseDecl.Datatype;
				XmlSchemaDatatype datatype2 = derivedDecl.Datatype;
				if (datatype.Variety == XmlSchemaDatatypeVariety.Union)
				{
					if (datatype2.Variety == XmlSchemaDatatypeVariety.Union)
					{
						if (!datatype2.IsEqual(defaultValueTyped, defaultValueTyped2))
						{
							return false;
						}
					}
					else
					{
						XsdSimpleValue xsdSimpleValue = baseDecl.DefaultValueTyped as XsdSimpleValue;
						XmlSchemaDatatype datatype3 = xsdSimpleValue.XmlType.Datatype;
						if (!datatype3.IsComparable(datatype2) || !datatype2.IsEqual(xsdSimpleValue.TypedValue, defaultValueTyped2))
						{
							return false;
						}
					}
				}
				else if (!datatype2.IsEqual(defaultValueTyped, defaultValueTyped2))
				{
					return false;
				}
			}
			return true;
		}

		private string restrictionErrorMsg;

		private XmlSchemaObjectTable attributes = new XmlSchemaObjectTable();

		private XmlSchemaObjectTable attributeGroups = new XmlSchemaObjectTable();

		private XmlSchemaObjectTable elements = new XmlSchemaObjectTable();

		private XmlSchemaObjectTable schemaTypes = new XmlSchemaObjectTable();

		private XmlSchemaObjectTable groups = new XmlSchemaObjectTable();

		private XmlSchemaObjectTable notations = new XmlSchemaObjectTable();

		private XmlSchemaObjectTable examplars = new XmlSchemaObjectTable();

		private XmlSchemaObjectTable identityConstraints = new XmlSchemaObjectTable();

		private Stack complexTypeStack = new Stack();

		private Hashtable schemasToCompile = new Hashtable();

		private Hashtable importedSchemas = new Hashtable();

		private XmlSchema schemaForSchema;
	}
}
