using System;
using System.Collections;
using System.Globalization;

namespace System.Xml.Schema
{
	// Token: 0x0200020D RID: 525
	internal sealed class SchemaCollectionCompiler : BaseProcessor
	{
		// Token: 0x060018E6 RID: 6374 RVA: 0x00071CDD File Offset: 0x00070CDD
		public SchemaCollectionCompiler(XmlNameTable nameTable, ValidationEventHandler eventHandler)
			: base(nameTable, null, eventHandler)
		{
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x00071CFE File Offset: 0x00070CFE
		public bool Execute(XmlSchema schema, SchemaInfo schemaInfo, bool compileContentModel)
		{
			this.compileContentModel = compileContentModel;
			this.schema = schema;
			this.Prepare();
			this.Cleanup();
			this.Compile();
			if (!base.HasErrors)
			{
				this.Output(schemaInfo);
			}
			return !base.HasErrors;
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x00071D38 File Offset: 0x00070D38
		private void Prepare()
		{
			foreach (object obj in this.schema.Elements.Values)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj;
				if (!xmlSchemaElement.SubstitutionGroup.IsEmpty)
				{
					XmlSchemaSubstitutionGroup xmlSchemaSubstitutionGroup = (XmlSchemaSubstitutionGroup)this.examplars[xmlSchemaElement.SubstitutionGroup];
					if (xmlSchemaSubstitutionGroup == null)
					{
						xmlSchemaSubstitutionGroup = new XmlSchemaSubstitutionGroupV1Compat();
						xmlSchemaSubstitutionGroup.Examplar = xmlSchemaElement.SubstitutionGroup;
						this.examplars.Add(xmlSchemaElement.SubstitutionGroup, xmlSchemaSubstitutionGroup);
					}
					ArrayList members = xmlSchemaSubstitutionGroup.Members;
					members.Add(xmlSchemaElement);
				}
			}
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x00071DF4 File Offset: 0x00070DF4
		private void Cleanup()
		{
			foreach (object obj in this.schema.Groups.Values)
			{
				XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup)obj;
				SchemaCollectionCompiler.CleanupGroup(xmlSchemaGroup);
			}
			foreach (object obj2 in this.schema.AttributeGroups.Values)
			{
				XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)obj2;
				SchemaCollectionCompiler.CleanupAttributeGroup(xmlSchemaAttributeGroup);
			}
			foreach (object obj3 in this.schema.SchemaTypes.Values)
			{
				XmlSchemaType xmlSchemaType = (XmlSchemaType)obj3;
				if (xmlSchemaType is XmlSchemaComplexType)
				{
					SchemaCollectionCompiler.CleanupComplexType((XmlSchemaComplexType)xmlSchemaType);
				}
				else
				{
					SchemaCollectionCompiler.CleanupSimpleType((XmlSchemaSimpleType)xmlSchemaType);
				}
			}
			foreach (object obj4 in this.schema.Elements.Values)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj4;
				SchemaCollectionCompiler.CleanupElement(xmlSchemaElement);
			}
			foreach (object obj5 in this.schema.Attributes.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj5;
				SchemaCollectionCompiler.CleanupAttribute(xmlSchemaAttribute);
			}
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x00071FDC File Offset: 0x00070FDC
		internal static void Cleanup(XmlSchema schema)
		{
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				if (xmlSchemaExternal.Schema != null)
				{
					SchemaCollectionCompiler.Cleanup(xmlSchemaExternal.Schema);
				}
				if (xmlSchemaExternal is XmlSchemaRedefine)
				{
					XmlSchemaRedefine xmlSchemaRedefine = xmlSchemaExternal as XmlSchemaRedefine;
					xmlSchemaRedefine.AttributeGroups.Clear();
					xmlSchemaRedefine.Groups.Clear();
					xmlSchemaRedefine.SchemaTypes.Clear();
					foreach (object obj in xmlSchemaRedefine.Items)
					{
						if (obj is XmlSchemaAttribute)
						{
							SchemaCollectionCompiler.CleanupAttribute((XmlSchemaAttribute)obj);
						}
						else if (obj is XmlSchemaAttributeGroup)
						{
							SchemaCollectionCompiler.CleanupAttributeGroup((XmlSchemaAttributeGroup)obj);
						}
						else if (obj is XmlSchemaComplexType)
						{
							SchemaCollectionCompiler.CleanupComplexType((XmlSchemaComplexType)obj);
						}
						else if (obj is XmlSchemaSimpleType)
						{
							SchemaCollectionCompiler.CleanupSimpleType((XmlSchemaSimpleType)obj);
						}
						else if (obj is XmlSchemaElement)
						{
							SchemaCollectionCompiler.CleanupElement((XmlSchemaElement)obj);
						}
						else if (obj is XmlSchemaGroup)
						{
							SchemaCollectionCompiler.CleanupGroup((XmlSchemaGroup)obj);
						}
					}
				}
			}
			foreach (object obj2 in schema.Items)
			{
				if (obj2 is XmlSchemaAttribute)
				{
					SchemaCollectionCompiler.CleanupAttribute((XmlSchemaAttribute)obj2);
				}
				else if (obj2 is XmlSchemaAttributeGroup)
				{
					SchemaCollectionCompiler.CleanupAttributeGroup((XmlSchemaAttributeGroup)obj2);
				}
				else if (obj2 is XmlSchemaComplexType)
				{
					SchemaCollectionCompiler.CleanupComplexType((XmlSchemaComplexType)obj2);
				}
				else if (obj2 is XmlSchemaSimpleType)
				{
					SchemaCollectionCompiler.CleanupSimpleType((XmlSchemaSimpleType)obj2);
				}
				else if (obj2 is XmlSchemaElement)
				{
					SchemaCollectionCompiler.CleanupElement((XmlSchemaElement)obj2);
				}
				else if (obj2 is XmlSchemaGroup)
				{
					SchemaCollectionCompiler.CleanupGroup((XmlSchemaGroup)obj2);
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
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x00072288 File Offset: 0x00071288
		private void Compile()
		{
			this.schema.SchemaTypes.Insert(DatatypeImplementation.QnAnyType, XmlSchemaComplexType.AnyType);
			foreach (object obj in this.examplars.Values)
			{
				XmlSchemaSubstitutionGroupV1Compat xmlSchemaSubstitutionGroupV1Compat = (XmlSchemaSubstitutionGroupV1Compat)obj;
				this.CompileSubstitutionGroup(xmlSchemaSubstitutionGroupV1Compat);
			}
			foreach (object obj2 in this.schema.Groups.Values)
			{
				XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup)obj2;
				this.CompileGroup(xmlSchemaGroup);
			}
			foreach (object obj3 in this.schema.AttributeGroups.Values)
			{
				XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)obj3;
				this.CompileAttributeGroup(xmlSchemaAttributeGroup);
			}
			foreach (object obj4 in this.schema.SchemaTypes.Values)
			{
				XmlSchemaType xmlSchemaType = (XmlSchemaType)obj4;
				if (xmlSchemaType is XmlSchemaComplexType)
				{
					this.CompileComplexType((XmlSchemaComplexType)xmlSchemaType);
				}
				else
				{
					this.CompileSimpleType((XmlSchemaSimpleType)xmlSchemaType);
				}
			}
			foreach (object obj5 in this.schema.Elements.Values)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj5;
				if (xmlSchemaElement.ElementDecl == null)
				{
					this.CompileElement(xmlSchemaElement);
				}
			}
			foreach (object obj6 in this.schema.Attributes.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj6;
				if (xmlSchemaAttribute.AttDef == null)
				{
					this.CompileAttribute(xmlSchemaAttribute);
				}
			}
			using (IEnumerator enumerator7 = this.schema.IdentityConstraints.Values.GetEnumerator())
			{
				while (enumerator7.MoveNext())
				{
					object obj7 = enumerator7.Current;
					XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint = (XmlSchemaIdentityConstraint)obj7;
					if (xmlSchemaIdentityConstraint.CompiledConstraint == null)
					{
						this.CompileIdentityConstraint(xmlSchemaIdentityConstraint);
					}
				}
				goto IL_0286;
			}
			IL_026C:
			XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)this.complexTypeStack.Pop();
			this.CompileCompexTypeElements(xmlSchemaComplexType);
			IL_0286:
			if (this.complexTypeStack.Count <= 0)
			{
				foreach (object obj8 in this.schema.SchemaTypes.Values)
				{
					XmlSchemaType xmlSchemaType2 = (XmlSchemaType)obj8;
					if (xmlSchemaType2 is XmlSchemaComplexType)
					{
						this.CheckParticleDerivation((XmlSchemaComplexType)xmlSchemaType2);
					}
				}
				foreach (object obj9 in this.schema.Elements.Values)
				{
					XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)obj9;
					if (xmlSchemaElement2.ElementSchemaType is XmlSchemaComplexType && xmlSchemaElement2.SchemaTypeName == XmlQualifiedName.Empty)
					{
						this.CheckParticleDerivation((XmlSchemaComplexType)xmlSchemaElement2.ElementSchemaType);
					}
				}
				foreach (object obj10 in this.examplars.Values)
				{
					XmlSchemaSubstitutionGroup xmlSchemaSubstitutionGroup = (XmlSchemaSubstitutionGroup)obj10;
					this.CheckSubstitutionGroup(xmlSchemaSubstitutionGroup);
				}
				this.schema.SchemaTypes.Remove(DatatypeImplementation.QnAnyType);
				return;
			}
			goto IL_026C;
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x000726DC File Offset: 0x000716DC
		private void Output(SchemaInfo schemaInfo)
		{
			foreach (object obj in this.schema.Elements.Values)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj;
				schemaInfo.TargetNamespaces[xmlSchemaElement.QualifiedName.Namespace] = true;
				schemaInfo.ElementDecls.Add(xmlSchemaElement.QualifiedName, xmlSchemaElement.ElementDecl);
			}
			foreach (object obj2 in this.schema.Attributes.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj2;
				schemaInfo.TargetNamespaces[xmlSchemaAttribute.QualifiedName.Namespace] = true;
				schemaInfo.AttributeDecls.Add(xmlSchemaAttribute.QualifiedName, xmlSchemaAttribute.AttDef);
			}
			foreach (object obj3 in this.schema.SchemaTypes.Values)
			{
				XmlSchemaType xmlSchemaType = (XmlSchemaType)obj3;
				schemaInfo.TargetNamespaces[xmlSchemaType.QualifiedName.Namespace] = true;
				XmlSchemaComplexType xmlSchemaComplexType = xmlSchemaType as XmlSchemaComplexType;
				if (xmlSchemaComplexType == null || (!xmlSchemaComplexType.IsAbstract && xmlSchemaType != XmlSchemaComplexType.AnyType))
				{
					schemaInfo.ElementDeclsByType.Add(xmlSchemaType.QualifiedName, xmlSchemaType.ElementDecl);
				}
			}
			foreach (object obj4 in this.schema.Notations.Values)
			{
				XmlSchemaNotation xmlSchemaNotation = (XmlSchemaNotation)obj4;
				schemaInfo.TargetNamespaces[xmlSchemaNotation.QualifiedName.Namespace] = true;
				SchemaNotation schemaNotation = new SchemaNotation(xmlSchemaNotation.QualifiedName);
				schemaNotation.SystemLiteral = xmlSchemaNotation.System;
				schemaNotation.Pubid = xmlSchemaNotation.Public;
				if (schemaInfo.Notations[schemaNotation.Name.Name] == null)
				{
					schemaInfo.Notations.Add(schemaNotation.Name.Name, schemaNotation);
				}
			}
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x00072968 File Offset: 0x00071968
		private static void CleanupAttribute(XmlSchemaAttribute attribute)
		{
			if (attribute.SchemaType != null)
			{
				SchemaCollectionCompiler.CleanupSimpleType(attribute.SchemaType);
			}
			attribute.AttDef = null;
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x00072984 File Offset: 0x00071984
		private static void CleanupAttributeGroup(XmlSchemaAttributeGroup attributeGroup)
		{
			SchemaCollectionCompiler.CleanupAttributes(attributeGroup.Attributes);
			attributeGroup.AttributeUses.Clear();
			attributeGroup.AttributeWildcard = null;
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x000729A4 File Offset: 0x000719A4
		private static void CleanupComplexType(XmlSchemaComplexType complexType)
		{
			if (complexType.ContentModel != null)
			{
				if (complexType.ContentModel is XmlSchemaSimpleContent)
				{
					XmlSchemaSimpleContent xmlSchemaSimpleContent = (XmlSchemaSimpleContent)complexType.ContentModel;
					if (xmlSchemaSimpleContent.Content is XmlSchemaSimpleContentExtension)
					{
						XmlSchemaSimpleContentExtension xmlSchemaSimpleContentExtension = (XmlSchemaSimpleContentExtension)xmlSchemaSimpleContent.Content;
						SchemaCollectionCompiler.CleanupAttributes(xmlSchemaSimpleContentExtension.Attributes);
					}
					else
					{
						XmlSchemaSimpleContentRestriction xmlSchemaSimpleContentRestriction = (XmlSchemaSimpleContentRestriction)xmlSchemaSimpleContent.Content;
						SchemaCollectionCompiler.CleanupAttributes(xmlSchemaSimpleContentRestriction.Attributes);
					}
				}
				else
				{
					XmlSchemaComplexContent xmlSchemaComplexContent = (XmlSchemaComplexContent)complexType.ContentModel;
					if (xmlSchemaComplexContent.Content is XmlSchemaComplexContentExtension)
					{
						XmlSchemaComplexContentExtension xmlSchemaComplexContentExtension = (XmlSchemaComplexContentExtension)xmlSchemaComplexContent.Content;
						SchemaCollectionCompiler.CleanupParticle(xmlSchemaComplexContentExtension.Particle);
						SchemaCollectionCompiler.CleanupAttributes(xmlSchemaComplexContentExtension.Attributes);
					}
					else
					{
						XmlSchemaComplexContentRestriction xmlSchemaComplexContentRestriction = (XmlSchemaComplexContentRestriction)xmlSchemaComplexContent.Content;
						SchemaCollectionCompiler.CleanupParticle(xmlSchemaComplexContentRestriction.Particle);
						SchemaCollectionCompiler.CleanupAttributes(xmlSchemaComplexContentRestriction.Attributes);
					}
				}
			}
			else
			{
				SchemaCollectionCompiler.CleanupParticle(complexType.Particle);
				SchemaCollectionCompiler.CleanupAttributes(complexType.Attributes);
			}
			complexType.LocalElements.Clear();
			complexType.AttributeUses.Clear();
			complexType.SetAttributeWildcard(null);
			complexType.SetContentTypeParticle(XmlSchemaParticle.Empty);
			complexType.ElementDecl = null;
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x00072AC3 File Offset: 0x00071AC3
		private static void CleanupSimpleType(XmlSchemaSimpleType simpleType)
		{
			simpleType.ElementDecl = null;
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x00072ACC File Offset: 0x00071ACC
		private static void CleanupElement(XmlSchemaElement element)
		{
			if (element.SchemaType != null)
			{
				XmlSchemaComplexType xmlSchemaComplexType = element.SchemaType as XmlSchemaComplexType;
				if (xmlSchemaComplexType != null)
				{
					SchemaCollectionCompiler.CleanupComplexType(xmlSchemaComplexType);
				}
				else
				{
					SchemaCollectionCompiler.CleanupSimpleType((XmlSchemaSimpleType)element.SchemaType);
				}
			}
			foreach (XmlSchemaObject xmlSchemaObject in element.Constraints)
			{
				XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint = (XmlSchemaIdentityConstraint)xmlSchemaObject;
				xmlSchemaIdentityConstraint.CompiledConstraint = null;
			}
			element.ElementDecl = null;
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x00072B5C File Offset: 0x00071B5C
		private static void CleanupAttributes(XmlSchemaObjectCollection attributes)
		{
			foreach (XmlSchemaObject xmlSchemaObject in attributes)
			{
				if (xmlSchemaObject is XmlSchemaAttribute)
				{
					SchemaCollectionCompiler.CleanupAttribute((XmlSchemaAttribute)xmlSchemaObject);
				}
			}
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x00072BB8 File Offset: 0x00071BB8
		private static void CleanupGroup(XmlSchemaGroup group)
		{
			SchemaCollectionCompiler.CleanupParticle(group.Particle);
			group.CanonicalParticle = null;
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x00072BCC File Offset: 0x00071BCC
		private static void CleanupParticle(XmlSchemaParticle particle)
		{
			if (particle is XmlSchemaElement)
			{
				SchemaCollectionCompiler.CleanupElement((XmlSchemaElement)particle);
				return;
			}
			if (particle is XmlSchemaGroupBase)
			{
				foreach (XmlSchemaObject xmlSchemaObject in ((XmlSchemaGroupBase)particle).Items)
				{
					XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
					SchemaCollectionCompiler.CleanupParticle(xmlSchemaParticle);
				}
			}
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x00072C48 File Offset: 0x00071C48
		private void CompileSubstitutionGroup(XmlSchemaSubstitutionGroupV1Compat substitutionGroup)
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
			XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)this.schema.Elements[substitutionGroup.Examplar];
			if (!substitutionGroup.Members.Contains(xmlSchemaElement2))
			{
				substitutionGroup.IsProcessing = true;
				if (xmlSchemaElement2 != null)
				{
					if (xmlSchemaElement2.FinalResolved == XmlSchemaDerivationMethod.All)
					{
						base.SendValidationEvent("Sch_InvalidExamplar", xmlSchemaElement2);
					}
					foreach (object obj in substitutionGroup.Members)
					{
						XmlSchemaElement xmlSchemaElement3 = (XmlSchemaElement)obj;
						XmlSchemaSubstitutionGroupV1Compat xmlSchemaSubstitutionGroupV1Compat = (XmlSchemaSubstitutionGroupV1Compat)this.examplars[xmlSchemaElement3.QualifiedName];
						if (xmlSchemaSubstitutionGroupV1Compat != null)
						{
							this.CompileSubstitutionGroup(xmlSchemaSubstitutionGroupV1Compat);
							using (XmlSchemaObjectEnumerator enumerator3 = xmlSchemaSubstitutionGroupV1Compat.Choice.Items.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									XmlSchemaObject xmlSchemaObject = enumerator3.Current;
									XmlSchemaElement xmlSchemaElement4 = (XmlSchemaElement)xmlSchemaObject;
									substitutionGroup.Choice.Items.Add(xmlSchemaElement4);
								}
								continue;
							}
						}
						substitutionGroup.Choice.Items.Add(xmlSchemaElement3);
					}
					substitutionGroup.Choice.Items.Add(xmlSchemaElement2);
					substitutionGroup.Members.Add(xmlSchemaElement2);
				}
				else
				{
					using (IEnumerator enumerator4 = substitutionGroup.Members.GetEnumerator())
					{
						if (enumerator4.MoveNext())
						{
							XmlSchemaElement xmlSchemaElement5 = (XmlSchemaElement)enumerator4.Current;
							base.SendValidationEvent("Sch_NoExamplar", xmlSchemaElement5);
						}
					}
				}
				substitutionGroup.IsProcessing = false;
				return;
			}
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x00072E6C File Offset: 0x00071E6C
		private void CheckSubstitutionGroup(XmlSchemaSubstitutionGroup substitutionGroup)
		{
			XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)this.schema.Elements[substitutionGroup.Examplar];
			if (xmlSchemaElement != null)
			{
				foreach (object obj in substitutionGroup.Members)
				{
					XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)obj;
					if (xmlSchemaElement2 != xmlSchemaElement && !XmlSchemaType.IsDerivedFrom(xmlSchemaElement2.ElementSchemaType, xmlSchemaElement.ElementSchemaType, xmlSchemaElement.FinalResolved))
					{
						base.SendValidationEvent("Sch_InvalidSubstitutionMember", xmlSchemaElement2.QualifiedName.ToString(), xmlSchemaElement.QualifiedName.ToString(), xmlSchemaElement2);
					}
				}
			}
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x00072F1C File Offset: 0x00071F1C
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
				group.CanonicalParticle = this.CannonicalizeParticle(group.Particle, true, true);
			}
			group.IsProcessing = false;
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x00072F74 File Offset: 0x00071F74
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
							throw new XmlSchemaException("Sch_UndeclaredSimpleType", xmlSchemaSimpleTypeList.ItemTypeName.ToString(), simpleType);
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
							throw new XmlSchemaException("Sch_InvalidSimpleTypeRestriction", xmlSchemaSimpleTypeRestriction.BaseTypeName.ToString(), simpleType);
						}
						XmlSchemaSimpleType simpleType3 = this.GetSimpleType(xmlSchemaSimpleTypeRestriction.BaseTypeName);
						if (simpleType3 == null)
						{
							throw new XmlSchemaException("Sch_UndeclaredSimpleType", xmlSchemaSimpleTypeRestriction.BaseTypeName.ToString(), simpleType);
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

		// Token: 0x060018F9 RID: 6393 RVA: 0x00073270 File Offset: 0x00072270
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
						throw new XmlSchemaException("Sch_UndeclaredSimpleType", xmlQualifiedName.ToString(), simpleType);
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

		// Token: 0x060018FA RID: 6394 RVA: 0x000733DC File Offset: 0x000723DC
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

		// Token: 0x060018FB RID: 6395 RVA: 0x00073430 File Offset: 0x00072430
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
				complexType.SetContentTypeParticle(this.CompileContentTypeParticle(complexType.Particle, true));
				complexType.SetContentType(this.GetSchemaContentType(complexType, null, complexType.ContentTypeParticle));
			}
			bool flag = false;
			foreach (object obj in complexType.AttributeUses.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj;
				if (xmlSchemaAttribute.Use != XmlSchemaUse.Prohibited)
				{
					XmlSchemaDatatype datatype = xmlSchemaAttribute.Datatype;
					if (datatype != null && datatype.TokenizedType == XmlTokenizedType.ID)
					{
						if (flag)
						{
							base.SendValidationEvent("Sch_TwoIdAttrUses", complexType);
						}
						else
						{
							flag = true;
						}
					}
				}
			}
			SchemaElementDecl schemaElementDecl = new SchemaElementDecl();
			schemaElementDecl.ContentValidator = this.CompileComplexContent(complexType);
			schemaElementDecl.SchemaType = complexType;
			schemaElementDecl.IsAbstract = complexType.IsAbstract;
			schemaElementDecl.Datatype = complexType.Datatype;
			schemaElementDecl.Block = complexType.BlockResolved;
			schemaElementDecl.AnyAttribute = complexType.AttributeWildcard;
			foreach (object obj2 in complexType.AttributeUses.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute2 = (XmlSchemaAttribute)obj2;
				if (xmlSchemaAttribute2.Use == XmlSchemaUse.Prohibited)
				{
					if (schemaElementDecl.ProhibitedAttributes[xmlSchemaAttribute2.QualifiedName] == null)
					{
						schemaElementDecl.ProhibitedAttributes.Add(xmlSchemaAttribute2.QualifiedName, xmlSchemaAttribute2.QualifiedName);
					}
				}
				else if (schemaElementDecl.AttDefs[xmlSchemaAttribute2.QualifiedName] == null && xmlSchemaAttribute2.AttDef != null && xmlSchemaAttribute2.AttDef.Name != XmlQualifiedName.Empty && xmlSchemaAttribute2.AttDef != SchemaAttDef.Empty)
				{
					schemaElementDecl.AddAttDef(xmlSchemaAttribute2.AttDef);
				}
			}
			schemaElementDecl.EndAddAttDef();
			complexType.ElementDecl = schemaElementDecl;
			complexType.IsProcessing = false;
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x0007372C File Offset: 0x0007272C
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
					base.SendValidationEvent("Sch_UndeclaredType", simpleExtension.BaseTypeName.ToString(), complexType);
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

		// Token: 0x060018FD RID: 6397 RVA: 0x00073810 File Offset: 0x00072810
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

		// Token: 0x060018FE RID: 6398 RVA: 0x000739E8 File Offset: 0x000729E8
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
			if (xmlSchemaComplexType != null && xmlSchemaComplexType.ElementDecl != null && xmlSchemaComplexType.ContentType == XmlSchemaContentType.TextOnly)
			{
				base.SendValidationEvent("Sch_NotComplexContent", complexType);
				return;
			}
			complexType.SetBaseSchemaType(xmlSchemaComplexType);
			if ((xmlSchemaComplexType.FinalResolved & XmlSchemaDerivationMethod.Extension) != XmlSchemaDerivationMethod.Empty)
			{
				base.SendValidationEvent("Sch_BaseFinalExtension", complexType);
			}
			this.CompileLocalAttributes(xmlSchemaComplexType, complexType, complexExtension.Attributes, complexExtension.AnyAttribute, XmlSchemaDerivationMethod.Extension);
			XmlSchemaParticle contentTypeParticle = xmlSchemaComplexType.ContentTypeParticle;
			XmlSchemaParticle xmlSchemaParticle = this.CannonicalizeParticle(complexExtension.Particle, true, true);
			if (contentTypeParticle != XmlSchemaParticle.Empty)
			{
				if (xmlSchemaParticle != XmlSchemaParticle.Empty)
				{
					complexType.SetContentTypeParticle(this.CompileContentTypeParticle(new XmlSchemaSequence
					{
						Items = { contentTypeParticle, xmlSchemaParticle }
					}, false));
				}
				else
				{
					complexType.SetContentTypeParticle(contentTypeParticle);
				}
				XmlSchemaContentType xmlSchemaContentType = this.GetSchemaContentType(complexType, complexContent, xmlSchemaParticle);
				if (xmlSchemaContentType == XmlSchemaContentType.Empty)
				{
					xmlSchemaContentType = xmlSchemaComplexType.ContentType;
				}
				complexType.SetContentType(xmlSchemaContentType);
				if (complexType.ContentType != xmlSchemaComplexType.ContentType)
				{
					base.SendValidationEvent("Sch_DifContentType", complexType);
				}
			}
			else
			{
				complexType.SetContentTypeParticle(xmlSchemaParticle);
				complexType.SetContentType(this.GetSchemaContentType(complexType, complexContent, complexType.ContentTypeParticle));
			}
			complexType.SetDerivedBy(XmlSchemaDerivationMethod.Extension);
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x00073B64 File Offset: 0x00072B64
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
			if (xmlSchemaComplexType != null && xmlSchemaComplexType.ElementDecl != null && xmlSchemaComplexType.ContentType == XmlSchemaContentType.TextOnly)
			{
				base.SendValidationEvent("Sch_NotComplexContent", complexType);
				return;
			}
			complexType.SetBaseSchemaType(xmlSchemaComplexType);
			if ((xmlSchemaComplexType.FinalResolved & XmlSchemaDerivationMethod.Restriction) != XmlSchemaDerivationMethod.Empty)
			{
				base.SendValidationEvent("Sch_BaseFinalRestriction", complexType);
			}
			this.CompileLocalAttributes(xmlSchemaComplexType, complexType, complexRestriction.Attributes, complexRestriction.AnyAttribute, XmlSchemaDerivationMethod.Restriction);
			complexType.SetContentTypeParticle(this.CompileContentTypeParticle(complexRestriction.Particle, true));
			complexType.SetContentType(this.GetSchemaContentType(complexType, complexContent, complexType.ContentTypeParticle));
			if (complexType.ContentType == XmlSchemaContentType.Empty)
			{
				SchemaElementDecl elementDecl = xmlSchemaComplexType.ElementDecl;
				if (xmlSchemaComplexType.ElementDecl != null && !xmlSchemaComplexType.ElementDecl.ContentValidator.IsEmptiable)
				{
					base.SendValidationEvent("Sch_InvalidContentRestriction", complexType);
				}
			}
			complexType.SetDerivedBy(XmlSchemaDerivationMethod.Restriction);
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x00073C88 File Offset: 0x00072C88
		private void CheckParticleDerivation(XmlSchemaComplexType complexType)
		{
			XmlSchemaComplexType xmlSchemaComplexType = complexType.BaseXmlSchemaType as XmlSchemaComplexType;
			if (xmlSchemaComplexType != null && xmlSchemaComplexType != XmlSchemaComplexType.AnyType && complexType.DerivedBy == XmlSchemaDerivationMethod.Restriction && !this.IsValidRestriction(complexType.ContentTypeParticle, xmlSchemaComplexType.ContentTypeParticle))
			{
				base.SendValidationEvent("Sch_InvalidParticleRestriction", complexType);
			}
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x00073CD8 File Offset: 0x00072CD8
		private XmlSchemaParticle CompileContentTypeParticle(XmlSchemaParticle particle, bool substitution)
		{
			XmlSchemaParticle xmlSchemaParticle = this.CannonicalizeParticle(particle, true, substitution);
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

		// Token: 0x06001902 RID: 6402 RVA: 0x00073D30 File Offset: 0x00072D30
		private XmlSchemaParticle CannonicalizeParticle(XmlSchemaParticle particle, bool root, bool substitution)
		{
			if (particle == null || particle.IsEmpty)
			{
				return XmlSchemaParticle.Empty;
			}
			if (particle is XmlSchemaElement)
			{
				return this.CannonicalizeElement((XmlSchemaElement)particle, substitution);
			}
			if (particle is XmlSchemaGroupRef)
			{
				return this.CannonicalizeGroupRef((XmlSchemaGroupRef)particle, root, substitution);
			}
			if (particle is XmlSchemaAll)
			{
				return this.CannonicalizeAll((XmlSchemaAll)particle, root, substitution);
			}
			if (particle is XmlSchemaChoice)
			{
				return this.CannonicalizeChoice((XmlSchemaChoice)particle, root, substitution);
			}
			if (particle is XmlSchemaSequence)
			{
				return this.CannonicalizeSequence((XmlSchemaSequence)particle, root, substitution);
			}
			return particle;
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x00073DC4 File Offset: 0x00072DC4
		private XmlSchemaParticle CannonicalizeElement(XmlSchemaElement element, bool substitution)
		{
			if (element.RefName.IsEmpty || !substitution || (element.BlockResolved & XmlSchemaDerivationMethod.Substitution) != XmlSchemaDerivationMethod.Empty)
			{
				return element;
			}
			XmlSchemaSubstitutionGroupV1Compat xmlSchemaSubstitutionGroupV1Compat = (XmlSchemaSubstitutionGroupV1Compat)this.examplars[element.QualifiedName];
			if (xmlSchemaSubstitutionGroupV1Compat == null)
			{
				return element;
			}
			XmlSchemaChoice xmlSchemaChoice = (XmlSchemaChoice)xmlSchemaSubstitutionGroupV1Compat.Choice.Clone();
			xmlSchemaChoice.MinOccurs = element.MinOccurs;
			xmlSchemaChoice.MaxOccurs = element.MaxOccurs;
			return xmlSchemaChoice;
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x00073E34 File Offset: 0x00072E34
		private XmlSchemaParticle CannonicalizeGroupRef(XmlSchemaGroupRef groupRef, bool root, bool substitution)
		{
			XmlSchemaGroup xmlSchemaGroup;
			if (groupRef.Redefined != null)
			{
				xmlSchemaGroup = groupRef.Redefined;
			}
			else
			{
				xmlSchemaGroup = (XmlSchemaGroup)this.schema.Groups[groupRef.RefName];
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
				if (groupRef.MinOccurs != 1m || groupRef.MaxOccurs != 1m)
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
			foreach (XmlSchemaObject xmlSchemaObject in xmlSchemaGroupBase.Items)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				xmlSchemaGroupBase2.Items.Add(xmlSchemaParticle);
			}
			groupRef.SetParticle(xmlSchemaGroupBase2);
			return xmlSchemaGroupBase2;
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x00073FF0 File Offset: 0x00072FF0
		private XmlSchemaParticle CannonicalizeAll(XmlSchemaAll all, bool root, bool substitution)
		{
			if (all.Items.Count > 0)
			{
				XmlSchemaAll xmlSchemaAll = new XmlSchemaAll();
				xmlSchemaAll.MinOccurs = all.MinOccurs;
				xmlSchemaAll.MaxOccurs = all.MaxOccurs;
				xmlSchemaAll.SourceUri = all.SourceUri;
				xmlSchemaAll.LineNumber = all.LineNumber;
				xmlSchemaAll.LinePosition = all.LinePosition;
				foreach (XmlSchemaObject xmlSchemaObject in all.Items)
				{
					XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)xmlSchemaObject;
					XmlSchemaParticle xmlSchemaParticle = this.CannonicalizeParticle(xmlSchemaElement, false, substitution);
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
			if (root && all.Items.Count == 1)
			{
				return new XmlSchemaSequence
				{
					MinOccurs = all.MinOccurs,
					MaxOccurs = all.MaxOccurs,
					Items = { (XmlSchemaParticle)all.Items[0] }
				};
			}
			if (!root && all.Items.Count == 1 && all.MinOccurs == 1m && all.MaxOccurs == 1m)
			{
				return (XmlSchemaParticle)all.Items[0];
			}
			if (!root)
			{
				base.SendValidationEvent("Sch_NotAllAlone", all);
				return XmlSchemaParticle.Empty;
			}
			return all;
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x00074180 File Offset: 0x00073180
		private XmlSchemaParticle CannonicalizeChoice(XmlSchemaChoice choice, bool root, bool substitution)
		{
			XmlSchemaChoice xmlSchemaChoice = choice;
			if (choice.Items.Count > 0)
			{
				XmlSchemaChoice xmlSchemaChoice2 = new XmlSchemaChoice();
				xmlSchemaChoice2.MinOccurs = choice.MinOccurs;
				xmlSchemaChoice2.MaxOccurs = choice.MaxOccurs;
				foreach (XmlSchemaObject xmlSchemaObject in choice.Items)
				{
					XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
					XmlSchemaParticle xmlSchemaParticle2 = this.CannonicalizeParticle(xmlSchemaParticle, false, substitution);
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

		// Token: 0x06001907 RID: 6407 RVA: 0x00074340 File Offset: 0x00073340
		private XmlSchemaParticle CannonicalizeSequence(XmlSchemaSequence sequence, bool root, bool substitution)
		{
			if (sequence.Items.Count > 0)
			{
				XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
				xmlSchemaSequence.MinOccurs = sequence.MinOccurs;
				xmlSchemaSequence.MaxOccurs = sequence.MaxOccurs;
				foreach (XmlSchemaObject xmlSchemaObject in sequence.Items)
				{
					XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
					XmlSchemaParticle xmlSchemaParticle2 = this.CannonicalizeParticle(xmlSchemaParticle, false, substitution);
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

		// Token: 0x06001908 RID: 6408 RVA: 0x000744D8 File Offset: 0x000734D8
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
			if (baseParticle is XmlSchemaElement)
			{
				return derivedParticle is XmlSchemaElement && this.IsElementFromElement((XmlSchemaElement)derivedParticle, (XmlSchemaElement)baseParticle);
			}
			if (!(baseParticle is XmlSchemaAny))
			{
				if (baseParticle is XmlSchemaAll)
				{
					if (derivedParticle is XmlSchemaElement)
					{
						return this.IsElementFromGroupBase((XmlSchemaElement)derivedParticle, (XmlSchemaGroupBase)baseParticle, true);
					}
					if (derivedParticle is XmlSchemaAll)
					{
						return this.IsGroupBaseFromGroupBase((XmlSchemaGroupBase)derivedParticle, (XmlSchemaGroupBase)baseParticle, true);
					}
					if (derivedParticle is XmlSchemaSequence)
					{
						return this.IsSequenceFromAll((XmlSchemaSequence)derivedParticle, (XmlSchemaAll)baseParticle);
					}
				}
				else if (baseParticle is XmlSchemaChoice)
				{
					if (derivedParticle is XmlSchemaElement)
					{
						return this.IsElementFromGroupBase((XmlSchemaElement)derivedParticle, (XmlSchemaGroupBase)baseParticle, false);
					}
					if (derivedParticle is XmlSchemaChoice)
					{
						return this.IsGroupBaseFromGroupBase((XmlSchemaGroupBase)derivedParticle, (XmlSchemaGroupBase)baseParticle, false);
					}
					if (derivedParticle is XmlSchemaSequence)
					{
						return this.IsSequenceFromChoice((XmlSchemaSequence)derivedParticle, (XmlSchemaChoice)baseParticle);
					}
				}
				else if (baseParticle is XmlSchemaSequence)
				{
					if (derivedParticle is XmlSchemaElement)
					{
						return this.IsElementFromGroupBase((XmlSchemaElement)derivedParticle, (XmlSchemaGroupBase)baseParticle, true);
					}
					if (derivedParticle is XmlSchemaSequence)
					{
						return this.IsGroupBaseFromGroupBase((XmlSchemaGroupBase)derivedParticle, (XmlSchemaGroupBase)baseParticle, true);
					}
				}
				return false;
			}
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

		// Token: 0x06001909 RID: 6409 RVA: 0x0007467C File Offset: 0x0007367C
		private bool IsElementFromElement(XmlSchemaElement derivedElement, XmlSchemaElement baseElement)
		{
			return derivedElement.QualifiedName == baseElement.QualifiedName && derivedElement.IsNillable == baseElement.IsNillable && this.IsValidOccurrenceRangeRestriction(derivedElement, baseElement) && (baseElement.FixedValue == null || baseElement.FixedValue == derivedElement.FixedValue) && (derivedElement.BlockResolved | baseElement.BlockResolved) == derivedElement.BlockResolved && derivedElement.ElementSchemaType != null && baseElement.ElementSchemaType != null && XmlSchemaType.IsDerivedFrom(derivedElement.ElementSchemaType, baseElement.ElementSchemaType, ~XmlSchemaDerivationMethod.Restriction);
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x00074709 File Offset: 0x00073709
		private bool IsElementFromAny(XmlSchemaElement derivedElement, XmlSchemaAny baseAny)
		{
			return baseAny.Allows(derivedElement.QualifiedName) && this.IsValidOccurrenceRangeRestriction(derivedElement, baseAny);
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x00074723 File Offset: 0x00073723
		private bool IsAnyFromAny(XmlSchemaAny derivedAny, XmlSchemaAny baseAny)
		{
			return this.IsValidOccurrenceRangeRestriction(derivedAny, baseAny) && NamespaceList.IsSubset(derivedAny.NamespaceList, baseAny.NamespaceList);
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x00074744 File Offset: 0x00073744
		private bool IsGroupBaseFromAny(XmlSchemaGroupBase derivedGroupBase, XmlSchemaAny baseAny)
		{
			decimal num;
			decimal num2;
			this.CalculateEffectiveTotalRange(derivedGroupBase, out num, out num2);
			if (!this.IsValidOccurrenceRangeRestriction(num, num2, baseAny.MinOccurs, baseAny.MaxOccurs))
			{
				return false;
			}
			string minOccursString = baseAny.MinOccursString;
			baseAny.MinOccurs = 0m;
			foreach (XmlSchemaObject xmlSchemaObject in derivedGroupBase.Items)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				if (!this.IsValidRestriction(xmlSchemaParticle, baseAny))
				{
					baseAny.MinOccursString = minOccursString;
					return false;
				}
			}
			baseAny.MinOccursString = minOccursString;
			return true;
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x000747F4 File Offset: 0x000737F4
		private bool IsElementFromGroupBase(XmlSchemaElement derivedElement, XmlSchemaGroupBase baseGroupBase, bool skipEmptableOnly)
		{
			bool flag = false;
			foreach (XmlSchemaObject xmlSchemaObject in baseGroupBase.Items)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				if (!flag)
				{
					string minOccursString = xmlSchemaParticle.MinOccursString;
					string maxOccursString = xmlSchemaParticle.MaxOccursString;
					xmlSchemaParticle.MinOccurs *= baseGroupBase.MinOccurs;
					if (xmlSchemaParticle.MaxOccurs != 79228162514264337593543950335m)
					{
						if (baseGroupBase.MaxOccurs == 79228162514264337593543950335m)
						{
							xmlSchemaParticle.MaxOccurs = decimal.MaxValue;
						}
						else
						{
							xmlSchemaParticle.MaxOccurs *= baseGroupBase.MaxOccurs;
						}
					}
					flag = this.IsValidRestriction(derivedElement, xmlSchemaParticle);
					xmlSchemaParticle.MinOccursString = minOccursString;
					xmlSchemaParticle.MaxOccursString = maxOccursString;
				}
				else if (skipEmptableOnly && !this.IsParticleEmptiable(xmlSchemaParticle))
				{
					return false;
				}
			}
			return flag;
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x00074908 File Offset: 0x00073908
		private bool IsGroupBaseFromGroupBase(XmlSchemaGroupBase derivedGroupBase, XmlSchemaGroupBase baseGroupBase, bool skipEmptableOnly)
		{
			if (!this.IsValidOccurrenceRangeRestriction(derivedGroupBase, baseGroupBase) || derivedGroupBase.Items.Count > baseGroupBase.Items.Count)
			{
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
					return false;
				}
			}
			return num >= derivedGroupBase.Items.Count;
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x000749D4 File Offset: 0x000739D4
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

		// Token: 0x06001910 RID: 6416 RVA: 0x00074AD0 File Offset: 0x00073AD0
		private bool IsSequenceFromChoice(XmlSchemaSequence derivedSequence, XmlSchemaChoice baseChoice)
		{
			decimal num;
			decimal num2;
			this.CalculateSequenceRange(derivedSequence, out num, out num2);
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

		// Token: 0x06001911 RID: 6417 RVA: 0x00074B7C File Offset: 0x00073B7C
		private void CalculateSequenceRange(XmlSchemaSequence sequence, out decimal minOccurs, out decimal maxOccurs)
		{
			minOccurs = 0m;
			maxOccurs = 0m;
			foreach (XmlSchemaObject xmlSchemaObject in sequence.Items)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				minOccurs += xmlSchemaParticle.MinOccurs;
				if (xmlSchemaParticle.MaxOccurs == 79228162514264337593543950335m)
				{
					maxOccurs = decimal.MaxValue;
				}
				else if (maxOccurs != 79228162514264337593543950335m)
				{
					maxOccurs += xmlSchemaParticle.MaxOccurs;
				}
			}
			minOccurs *= sequence.MinOccurs;
			if (sequence.MaxOccurs == 79228162514264337593543950335m)
			{
				maxOccurs = decimal.MaxValue;
				return;
			}
			if (maxOccurs != 79228162514264337593543950335m)
			{
				maxOccurs *= sequence.MaxOccurs;
			}
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x00074CC4 File Offset: 0x00073CC4
		private bool IsValidOccurrenceRangeRestriction(XmlSchemaParticle derivedParticle, XmlSchemaParticle baseParticle)
		{
			return this.IsValidOccurrenceRangeRestriction(derivedParticle.MinOccurs, derivedParticle.MaxOccurs, baseParticle.MinOccurs, baseParticle.MaxOccurs);
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x00074CE4 File Offset: 0x00073CE4
		private bool IsValidOccurrenceRangeRestriction(decimal minOccurs, decimal maxOccurs, decimal baseMinOccurs, decimal baseMaxOccurs)
		{
			return baseMinOccurs <= minOccurs && maxOccurs <= baseMaxOccurs;
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x00074CFC File Offset: 0x00073CFC
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

		// Token: 0x06001915 RID: 6421 RVA: 0x00074D34 File Offset: 0x00073D34
		private bool IsParticleEmptiable(XmlSchemaParticle particle)
		{
			decimal num;
			decimal num2;
			this.CalculateEffectiveTotalRange(particle, out num, out num2);
			return num == 0m;
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x00074D58 File Offset: 0x00073D58
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

		// Token: 0x06001917 RID: 6423 RVA: 0x0007503C File Offset: 0x0007403C
		private void PushComplexType(XmlSchemaComplexType complexType)
		{
			this.complexTypeStack.Push(complexType);
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x0007504A File Offset: 0x0007404A
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

		// Token: 0x06001919 RID: 6425 RVA: 0x00075074 File Offset: 0x00074074
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
			foreach (XmlSchemaObject xmlSchemaObject in attributeGroup.Attributes)
			{
				if (xmlSchemaObject is XmlSchemaAttribute)
				{
					XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)xmlSchemaObject;
					if (xmlSchemaAttribute.Use != XmlSchemaUse.Prohibited)
					{
						this.CompileAttribute(xmlSchemaAttribute);
					}
					if (attributeGroup.AttributeUses[xmlSchemaAttribute.QualifiedName] == null)
					{
						attributeGroup.AttributeUses.Add(xmlSchemaAttribute.QualifiedName, xmlSchemaAttribute);
					}
					else
					{
						base.SendValidationEvent("Sch_DupAttributeUse", xmlSchemaAttribute.QualifiedName.ToString(), xmlSchemaAttribute);
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
						xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)this.schema.AttributeGroups[xmlSchemaAttributeGroupRef.RefName];
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
			attributeGroup.IsProcessing = false;
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x000752A4 File Offset: 0x000742A4
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
					XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)this.schema.AttributeGroups[xmlSchemaAttributeGroupRef.RefName];
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
							if (xmlSchemaAttribute4 != null)
							{
								if (xmlSchemaAttribute4.AttributeSchemaType != xmlSchemaAttribute3.AttributeSchemaType || xmlSchemaAttribute3.Use == XmlSchemaUse.Prohibited)
								{
									base.SendValidationEvent("Sch_InvalidAttributeExtension", xmlSchemaAttribute4);
								}
							}
							else
							{
								derivedType.AttributeUses.Add(xmlSchemaAttribute3.QualifiedName, xmlSchemaAttribute3);
							}
						}
						return;
					}
				}
				if (anyAttribute != null && (xmlSchemaAnyAttribute == null || !XmlSchemaAnyAttribute.IsSubset(anyAttribute, xmlSchemaAnyAttribute)))
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
					else if (xmlSchemaAttribute6.Use != XmlSchemaUse.Prohibited && (xmlSchemaAttribute5.AttributeSchemaType == null || xmlSchemaAttribute6.AttributeSchemaType == null || !XmlSchemaType.IsDerivedFrom(xmlSchemaAttribute6.AttributeSchemaType, xmlSchemaAttribute5.AttributeSchemaType, XmlSchemaDerivationMethod.Empty)))
					{
						base.SendValidationEvent("Sch_AttributeRestrictionInvalid", xmlSchemaAttribute6);
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

		// Token: 0x0600191B RID: 6427 RVA: 0x00075784 File Offset: 0x00074784
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
			XmlSchemaAnyAttribute xmlSchemaAnyAttribute = XmlSchemaAnyAttribute.Union(a, b, true);
			if (xmlSchemaAnyAttribute == null)
			{
				base.SendValidationEvent("Sch_UnexpressibleAnyAttribute", a);
			}
			return xmlSchemaAnyAttribute;
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x000757B4 File Offset: 0x000747B4
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
			XmlSchemaAnyAttribute xmlSchemaAnyAttribute = XmlSchemaAnyAttribute.Intersection(a, b, true);
			if (xmlSchemaAnyAttribute == null)
			{
				base.SendValidationEvent("Sch_UnexpressibleAnyAttribute", a);
			}
			return xmlSchemaAnyAttribute;
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x000757E4 File Offset: 0x000747E4
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
					XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)this.schema.Attributes[xa.RefName];
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
					if (schemaAttDef.Datatype != null)
					{
						if (xmlSchemaAttribute.FixedValue != null)
						{
							if (xa.DefaultValue != null)
							{
								throw new XmlSchemaException("Sch_FixedDefaultInRef", xa.RefName.ToString(), xa);
							}
							if (xa.FixedValue != null)
							{
								if (xa.FixedValue != xmlSchemaAttribute.FixedValue)
								{
									throw new XmlSchemaException("Sch_FixedInRef", xa.RefName.ToString(), xa);
								}
							}
							else
							{
								schemaAttDef.Presence = SchemaDeclBase.Use.Fixed;
								schemaAttDef.DefaultValueRaw = (schemaAttDef.DefaultValueExpanded = xmlSchemaAttribute.FixedValue);
								schemaAttDef.DefaultValueTyped = schemaAttDef.Datatype.ParseValue(schemaAttDef.DefaultValueRaw, base.NameTable, new SchemaNamespaceManager(xa), true);
							}
						}
						else if (xmlSchemaAttribute.DefaultValue != null && xa.DefaultValue == null && xa.FixedValue == null)
						{
							schemaAttDef.Presence = SchemaDeclBase.Use.Default;
							schemaAttDef.DefaultValueRaw = (schemaAttDef.DefaultValueExpanded = xmlSchemaAttribute.DefaultValue);
							schemaAttDef.DefaultValueTyped = schemaAttDef.Datatype.ParseValue(schemaAttDef.DefaultValueRaw, base.NameTable, new SchemaNamespaceManager(xa), true);
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
				}
				if (schemaAttDef.Datatype != null)
				{
					schemaAttDef.Datatype.VerifySchemaValid(this.schema.Notations, xa);
				}
				if (xa.DefaultValue != null || xa.FixedValue != null)
				{
					if (xa.DefaultValue != null)
					{
						schemaAttDef.Presence = SchemaDeclBase.Use.Default;
						schemaAttDef.DefaultValueRaw = (schemaAttDef.DefaultValueExpanded = xa.DefaultValue);
					}
					else
					{
						schemaAttDef.Presence = SchemaDeclBase.Use.Fixed;
						schemaAttDef.DefaultValueRaw = (schemaAttDef.DefaultValueExpanded = xa.FixedValue);
					}
					if (schemaAttDef.Datatype != null)
					{
						schemaAttDef.DefaultValueTyped = schemaAttDef.Datatype.ParseValue(schemaAttDef.DefaultValueRaw, base.NameTable, new SchemaNamespaceManager(xa), true);
					}
				}
				else
				{
					switch (xa.Use)
					{
					case XmlSchemaUse.None:
					case XmlSchemaUse.Optional:
						schemaAttDef.Presence = SchemaDeclBase.Use.Implied;
						break;
					case XmlSchemaUse.Required:
						schemaAttDef.Presence = SchemaDeclBase.Use.Required;
						break;
					}
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

		// Token: 0x0600191E RID: 6430 RVA: 0x00075BB8 File Offset: 0x00074BB8
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
					XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint = (XmlSchemaIdentityConstraint)this.schema.IdentityConstraints[((XmlSchemaKeyref)xi).Refer];
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

		// Token: 0x0600191F RID: 6431 RVA: 0x00075D3C File Offset: 0x00074D3C
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
					XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)this.schema.Elements[xe.RefName];
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
						XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)this.schema.Elements[xe.SubstitutionGroup];
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
					schemaElementDecl.Datatype.VerifySchemaValid(this.schema.Notations, xe);
				}
				if ((xe.DefaultValue != null || xe.FixedValue != null) && schemaElementDecl.ContentValidator != null)
				{
					if (schemaElementDecl.ContentValidator.ContentType == XmlSchemaContentType.TextOnly)
					{
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
							schemaElementDecl.DefaultValueTyped = schemaElementDecl.Datatype.ParseValue(schemaElementDecl.DefaultValueRaw, base.NameTable, new SchemaNamespaceManager(xe), true);
						}
					}
					else if (schemaElementDecl.ContentValidator.ContentType != XmlSchemaContentType.Mixed || !schemaElementDecl.ContentValidator.IsEmptiable)
					{
						throw new XmlSchemaException("Sch_ElementCannotHaveValue", xe);
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

		// Token: 0x06001920 RID: 6432 RVA: 0x000761B4 File Offset: 0x000751B4
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
				ParticleContentValidator particleContentValidator = new ParticleContentValidator(complexType.ContentType);
				ContentValidator contentValidator;
				try
				{
					particleContentValidator.Start();
					this.BuildParticleContentModel(particleContentValidator, contentTypeParticle);
					contentValidator = particleContentValidator.Finish(this.compileContentModel);
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
							base.SendValidationEvent("Sch_NonDeterministicAnyEx", ((XmlSchemaAny)ex.Particle2).NamespaceList.ToString(), ((XmlSchemaElement)ex.Particle1).QualifiedName.ToString(), (XmlSchemaAny)ex.Particle2);
						}
					}
					else if (ex.Particle2 is XmlSchemaElement)
					{
						base.SendValidationEvent("Sch_NonDeterministicAnyEx", ((XmlSchemaAny)ex.Particle1).NamespaceList.ToString(), ((XmlSchemaElement)ex.Particle2).QualifiedName.ToString(), (XmlSchemaAny)ex.Particle1);
					}
					else
					{
						base.SendValidationEvent("Sch_NonDeterministicAnyAny", ((XmlSchemaAny)ex.Particle1).NamespaceList.ToString(), ((XmlSchemaAny)ex.Particle2).NamespaceList.ToString(), (XmlSchemaAny)ex.Particle1);
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

		// Token: 0x06001921 RID: 6433 RVA: 0x0007648C File Offset: 0x0007548C
		private void BuildParticleContentModel(ParticleContentValidator contentValidator, XmlSchemaParticle particle)
		{
			if (particle is XmlSchemaElement)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)particle;
				contentValidator.AddName(xmlSchemaElement.QualifiedName, xmlSchemaElement);
			}
			else if (particle is XmlSchemaAny)
			{
				XmlSchemaAny xmlSchemaAny = (XmlSchemaAny)particle;
				contentValidator.AddNamespaceList(xmlSchemaAny.NamespaceList, xmlSchemaAny);
			}
			else if (particle is XmlSchemaGroupBase)
			{
				XmlSchemaObjectCollection items = ((XmlSchemaGroupBase)particle).Items;
				bool flag = particle is XmlSchemaChoice;
				contentValidator.OpenGroup();
				bool flag2 = true;
				foreach (XmlSchemaObject xmlSchemaObject in items)
				{
					XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
					if (flag2)
					{
						flag2 = false;
					}
					else if (flag)
					{
						contentValidator.AddChoice();
					}
					else
					{
						contentValidator.AddSequence();
					}
					this.BuildParticleContentModel(contentValidator, xmlSchemaParticle);
				}
				contentValidator.CloseGroup();
			}
			if (particle.MinOccurs == 1m && particle.MaxOccurs == 1m)
			{
				return;
			}
			if (particle.MinOccurs == 0m && particle.MaxOccurs == 1m)
			{
				contentValidator.AddQMark();
				return;
			}
			if (particle.MinOccurs == 0m && particle.MaxOccurs == 79228162514264337593543950335m)
			{
				contentValidator.AddStar();
				return;
			}
			if (particle.MinOccurs == 1m && particle.MaxOccurs == 79228162514264337593543950335m)
			{
				contentValidator.AddPlus();
				return;
			}
			contentValidator.AddLeafRange(particle.MinOccurs, particle.MaxOccurs);
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x00076640 File Offset: 0x00075640
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

		// Token: 0x06001923 RID: 6435 RVA: 0x00076720 File Offset: 0x00075720
		private void CompileCompexTypeElements(XmlSchemaComplexType complexType)
		{
			if (complexType.IsProcessing)
			{
				base.SendValidationEvent("Sch_TypeCircularRef", complexType);
				return;
			}
			complexType.IsProcessing = true;
			if (complexType.ContentTypeParticle != XmlSchemaParticle.Empty)
			{
				this.CompileParticleElements(complexType, complexType.ContentTypeParticle);
			}
			complexType.IsProcessing = false;
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x00076760 File Offset: 0x00075760
		private XmlSchemaSimpleType GetSimpleType(XmlQualifiedName name)
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = this.schema.SchemaTypes[name] as XmlSchemaSimpleType;
			if (xmlSchemaSimpleType != null)
			{
				this.CompileSimpleType(xmlSchemaSimpleType);
			}
			else
			{
				xmlSchemaSimpleType = DatatypeImplementation.GetSimpleTypeFromXsdType(name);
				if (xmlSchemaSimpleType != null)
				{
					if (xmlSchemaSimpleType.TypeCode == XmlTypeCode.NormalizedString)
					{
						xmlSchemaSimpleType = DatatypeImplementation.GetNormalizedStringTypeV1Compat();
					}
					else if (xmlSchemaSimpleType.TypeCode == XmlTypeCode.Token)
					{
						xmlSchemaSimpleType = DatatypeImplementation.GetTokenTypeV1Compat();
					}
				}
			}
			return xmlSchemaSimpleType;
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x000767C0 File Offset: 0x000757C0
		private XmlSchemaComplexType GetComplexType(XmlQualifiedName name)
		{
			XmlSchemaComplexType xmlSchemaComplexType = this.schema.SchemaTypes[name] as XmlSchemaComplexType;
			if (xmlSchemaComplexType != null)
			{
				this.CompileComplexType(xmlSchemaComplexType);
			}
			return xmlSchemaComplexType;
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x000767F0 File Offset: 0x000757F0
		private XmlSchemaType GetAnySchemaType(XmlQualifiedName name)
		{
			XmlSchemaType xmlSchemaType = (XmlSchemaType)this.schema.SchemaTypes[name];
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

		// Token: 0x04000EAB RID: 3755
		private bool compileContentModel;

		// Token: 0x04000EAC RID: 3756
		private XmlSchemaObjectTable examplars = new XmlSchemaObjectTable();

		// Token: 0x04000EAD RID: 3757
		private Stack complexTypeStack = new Stack();

		// Token: 0x04000EAE RID: 3758
		private XmlSchema schema;
	}
}
