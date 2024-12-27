using System;
using System.CodeDom.Compiler;
using System.Security.Permissions;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x020002F1 RID: 753
	public class SoapSchemaImporter : SchemaImporter
	{
		// Token: 0x0600233E RID: 9022 RVA: 0x000A6A07 File Offset: 0x000A5A07
		public SoapSchemaImporter(XmlSchemas schemas)
			: base(schemas, CodeGenerationOptions.GenerateProperties, null, new ImportContext())
		{
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x000A6A17 File Offset: 0x000A5A17
		public SoapSchemaImporter(XmlSchemas schemas, CodeIdentifiers typeIdentifiers)
			: base(schemas, CodeGenerationOptions.GenerateProperties, null, new ImportContext(typeIdentifiers, false))
		{
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x000A6A29 File Offset: 0x000A5A29
		public SoapSchemaImporter(XmlSchemas schemas, CodeIdentifiers typeIdentifiers, CodeGenerationOptions options)
			: base(schemas, options, null, new ImportContext(typeIdentifiers, false))
		{
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x000A6A3B File Offset: 0x000A5A3B
		public SoapSchemaImporter(XmlSchemas schemas, CodeGenerationOptions options, ImportContext context)
			: base(schemas, options, null, context)
		{
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x000A6A47 File Offset: 0x000A5A47
		public SoapSchemaImporter(XmlSchemas schemas, CodeGenerationOptions options, CodeDomProvider codeProvider, ImportContext context)
			: base(schemas, options, codeProvider, context)
		{
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x000A6A54 File Offset: 0x000A5A54
		public XmlTypeMapping ImportDerivedTypeMapping(XmlQualifiedName name, Type baseType, bool baseTypeCanBeIndirect)
		{
			TypeMapping typeMapping = this.ImportType(name, false);
			if (typeMapping is StructMapping)
			{
				base.MakeDerived((StructMapping)typeMapping, baseType, baseTypeCanBeIndirect);
			}
			else if (baseType != null)
			{
				throw new InvalidOperationException(Res.GetString("XmlPrimitiveBaseType", new object[] { name.Name, name.Namespace, baseType.FullName }));
			}
			ElementAccessor elementAccessor = new ElementAccessor();
			elementAccessor.IsSoap = true;
			elementAccessor.Name = name.Name;
			elementAccessor.Namespace = name.Namespace;
			elementAccessor.Mapping = typeMapping;
			elementAccessor.IsNullable = true;
			elementAccessor.Form = XmlSchemaForm.Qualified;
			return new XmlTypeMapping(base.Scope, elementAccessor);
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x000A6B00 File Offset: 0x000A5B00
		public XmlMembersMapping ImportMembersMapping(string name, string ns, SoapSchemaMember member)
		{
			TypeMapping typeMapping = this.ImportType(member.MemberType, true);
			if (!(typeMapping is StructMapping))
			{
				return this.ImportMembersMapping(name, ns, new SoapSchemaMember[] { member });
			}
			MembersMapping membersMapping = new MembersMapping();
			membersMapping.TypeDesc = base.Scope.GetTypeDesc(typeof(object[]));
			membersMapping.Members = ((StructMapping)typeMapping).Members;
			membersMapping.HasWrapperElement = true;
			ElementAccessor elementAccessor = new ElementAccessor();
			elementAccessor.IsSoap = true;
			elementAccessor.Name = name;
			elementAccessor.Namespace = ((typeMapping.Namespace != null) ? typeMapping.Namespace : ns);
			elementAccessor.Mapping = membersMapping;
			elementAccessor.IsNullable = false;
			elementAccessor.Form = XmlSchemaForm.Qualified;
			return new XmlMembersMapping(base.Scope, elementAccessor, XmlMappingAccess.Read | XmlMappingAccess.Write);
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x000A6BBE File Offset: 0x000A5BBE
		public XmlMembersMapping ImportMembersMapping(string name, string ns, SoapSchemaMember[] members)
		{
			return this.ImportMembersMapping(name, ns, members, true);
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x000A6BCA File Offset: 0x000A5BCA
		public XmlMembersMapping ImportMembersMapping(string name, string ns, SoapSchemaMember[] members, bool hasWrapperElement)
		{
			return this.ImportMembersMapping(name, ns, members, hasWrapperElement, null, false);
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x000A6BDC File Offset: 0x000A5BDC
		public XmlMembersMapping ImportMembersMapping(string name, string ns, SoapSchemaMember[] members, bool hasWrapperElement, Type baseType, bool baseTypeCanBeIndirect)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			foreach (SoapSchemaMember soapSchemaMember in members)
			{
				XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
				xmlSchemaElement.Name = soapSchemaMember.MemberName;
				xmlSchemaElement.SchemaTypeName = soapSchemaMember.MemberType;
				xmlSchemaSequence.Items.Add(xmlSchemaElement);
			}
			CodeIdentifiers codeIdentifiers = new CodeIdentifiers();
			codeIdentifiers.UseCamelCasing = true;
			MembersMapping membersMapping = new MembersMapping();
			membersMapping.TypeDesc = base.Scope.GetTypeDesc(typeof(object[]));
			membersMapping.Members = this.ImportTypeMembers(xmlSchemaComplexType, ns, codeIdentifiers);
			membersMapping.HasWrapperElement = hasWrapperElement;
			if (baseType != null)
			{
				for (int j = 0; j < membersMapping.Members.Length; j++)
				{
					MemberMapping memberMapping = membersMapping.Members[j];
					if (memberMapping.Accessor.Mapping is StructMapping)
					{
						base.MakeDerived((StructMapping)memberMapping.Accessor.Mapping, baseType, baseTypeCanBeIndirect);
					}
				}
			}
			ElementAccessor elementAccessor = new ElementAccessor();
			elementAccessor.IsSoap = true;
			elementAccessor.Name = name;
			elementAccessor.Namespace = ns;
			elementAccessor.Mapping = membersMapping;
			elementAccessor.IsNullable = false;
			elementAccessor.Form = XmlSchemaForm.Qualified;
			return new XmlMembersMapping(base.Scope, elementAccessor, XmlMappingAccess.Read | XmlMappingAccess.Write);
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x000A6D2C File Offset: 0x000A5D2C
		private ElementAccessor ImportElement(XmlSchemaElement element, string ns)
		{
			if (!element.RefName.IsEmpty)
			{
				throw new InvalidOperationException(Res.GetString("RefSyntaxNotSupportedForElements0", new object[]
				{
					element.RefName.Name,
					element.RefName.Namespace
				}));
			}
			if (element.Name.Length == 0)
			{
				XmlQualifiedName parentName = XmlSchemas.GetParentName(element);
				throw new InvalidOperationException(Res.GetString("XmlElementHasNoName", new object[] { parentName.Name, parentName.Namespace }));
			}
			TypeMapping typeMapping = this.ImportElementType(element, ns);
			return new ElementAccessor
			{
				IsSoap = true,
				Name = element.Name,
				Namespace = ns,
				Mapping = typeMapping,
				IsNullable = element.IsNillable,
				Form = XmlSchemaForm.None
			};
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x000A6E00 File Offset: 0x000A5E00
		private TypeMapping ImportElementType(XmlSchemaElement element, string ns)
		{
			TypeMapping typeMapping;
			if (!element.SchemaTypeName.IsEmpty)
			{
				typeMapping = this.ImportType(element.SchemaTypeName, false);
			}
			else if (element.SchemaType != null)
			{
				XmlQualifiedName parentName = XmlSchemas.GetParentName(element);
				if (!(element.SchemaType is XmlSchemaComplexType))
				{
					throw new InvalidOperationException(Res.GetString("XmlInvalidSchemaElementType", new object[] { parentName.Name, parentName.Namespace, element.Name }));
				}
				typeMapping = this.ImportType((XmlSchemaComplexType)element.SchemaType, ns, false);
				if (!(typeMapping is ArrayMapping))
				{
					throw new InvalidOperationException(Res.GetString("XmlInvalidSchemaElementType", new object[] { parentName.Name, parentName.Namespace, element.Name }));
				}
			}
			else
			{
				if (!element.SubstitutionGroup.IsEmpty)
				{
					XmlQualifiedName parentName2 = XmlSchemas.GetParentName(element);
					throw new InvalidOperationException(Res.GetString("XmlInvalidSubstitutionGroupUse", new object[] { parentName2.Name, parentName2.Namespace }));
				}
				XmlQualifiedName parentName3 = XmlSchemas.GetParentName(element);
				throw new InvalidOperationException(Res.GetString("XmlElementMissingType", new object[] { parentName3.Name, parentName3.Namespace, element.Name }));
			}
			typeMapping.ReferencedByElement = true;
			return typeMapping;
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x000A6F64 File Offset: 0x000A5F64
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		internal override void ImportDerivedTypes(XmlQualifiedName baseName)
		{
			foreach (object obj in base.Schemas)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (!base.Schemas.IsReference(xmlSchema) && !XmlSchemas.IsDataSet(xmlSchema))
				{
					XmlSchemas.Preprocess(xmlSchema);
					foreach (object obj2 in xmlSchema.SchemaTypes.Values)
					{
						if (obj2 is XmlSchemaType)
						{
							XmlSchemaType xmlSchemaType = (XmlSchemaType)obj2;
							if (xmlSchemaType.DerivedFrom == baseName)
							{
								this.ImportType(xmlSchemaType.QualifiedName, false);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x000A7050 File Offset: 0x000A6050
		private TypeMapping ImportType(XmlQualifiedName name, bool excludeFromImport)
		{
			if (name.Name == "anyType" && name.Namespace == "http://www.w3.org/2001/XMLSchema")
			{
				return base.ImportRootMapping();
			}
			object obj = this.FindType(name);
			TypeMapping typeMapping = (TypeMapping)base.ImportedMappings[obj];
			if (typeMapping == null)
			{
				if (obj is XmlSchemaComplexType)
				{
					typeMapping = this.ImportType((XmlSchemaComplexType)obj, name.Namespace, excludeFromImport);
				}
				else
				{
					if (!(obj is XmlSchemaSimpleType))
					{
						throw new InvalidOperationException(Res.GetString("XmlInternalError"));
					}
					typeMapping = this.ImportDataType((XmlSchemaSimpleType)obj, name.Namespace, name.Name, false);
				}
			}
			if (excludeFromImport)
			{
				typeMapping.IncludeInSchema = false;
			}
			return typeMapping;
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x000A7104 File Offset: 0x000A6104
		private TypeMapping ImportType(XmlSchemaComplexType type, string typeNs, bool excludeFromImport)
		{
			if (type.Redefined != null)
			{
				throw new NotSupportedException(Res.GetString("XmlUnsupportedRedefine", new object[] { type.Name, typeNs }));
			}
			TypeMapping typeMapping = this.ImportAnyType(type, typeNs);
			if (typeMapping == null)
			{
				typeMapping = this.ImportArrayMapping(type, typeNs);
			}
			if (typeMapping == null)
			{
				typeMapping = this.ImportStructType(type, typeNs, excludeFromImport);
			}
			return typeMapping;
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x000A7164 File Offset: 0x000A6164
		private TypeMapping ImportAnyType(XmlSchemaComplexType type, string typeNs)
		{
			if (type.Particle == null)
			{
				return null;
			}
			if (!(type.Particle is XmlSchemaAll) && !(type.Particle is XmlSchemaSequence))
			{
				return null;
			}
			XmlSchemaGroupBase xmlSchemaGroupBase = (XmlSchemaGroupBase)type.Particle;
			if (xmlSchemaGroupBase.Items.Count != 1 || !(xmlSchemaGroupBase.Items[0] is XmlSchemaAny))
			{
				return null;
			}
			return base.ImportRootMapping();
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x000A71CC File Offset: 0x000A61CC
		private StructMapping ImportStructType(XmlSchemaComplexType type, string typeNs, bool excludeFromImport)
		{
			if (type.Name == null)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)type.Parent;
				XmlQualifiedName parentName = XmlSchemas.GetParentName(xmlSchemaElement);
				throw new InvalidOperationException(Res.GetString("XmlInvalidSchemaElementType", new object[] { parentName.Name, parentName.Namespace, xmlSchemaElement.Name }));
			}
			TypeDesc typeDesc = null;
			Mapping mapping = null;
			if (!type.DerivedFrom.IsEmpty)
			{
				mapping = this.ImportType(type.DerivedFrom, excludeFromImport);
				if (mapping is StructMapping)
				{
					typeDesc = ((StructMapping)mapping).TypeDesc;
				}
				else
				{
					mapping = null;
				}
			}
			if (mapping == null)
			{
				mapping = base.GetRootMapping();
			}
			Mapping mapping2 = (Mapping)base.ImportedMappings[type];
			if (mapping2 != null)
			{
				return (StructMapping)mapping2;
			}
			string text = base.GenerateUniqueTypeName(Accessor.UnescapeName(type.Name));
			StructMapping structMapping = new StructMapping();
			structMapping.IsReference = base.Schemas.IsReference(type);
			TypeFlags typeFlags = TypeFlags.Reference;
			if (type.IsAbstract)
			{
				typeFlags |= TypeFlags.Abstract;
			}
			structMapping.TypeDesc = new TypeDesc(text, text, TypeKind.Struct, typeDesc, typeFlags);
			structMapping.Namespace = typeNs;
			structMapping.TypeName = type.Name;
			structMapping.BaseMapping = (StructMapping)mapping;
			base.ImportedMappings.Add(type, structMapping);
			if (excludeFromImport)
			{
				structMapping.IncludeInSchema = false;
			}
			CodeIdentifiers codeIdentifiers = new CodeIdentifiers();
			codeIdentifiers.AddReserved(text);
			base.AddReservedIdentifiersForDataBinding(codeIdentifiers);
			structMapping.Members = this.ImportTypeMembers(type, typeNs, codeIdentifiers);
			base.Scope.AddTypeMapping(structMapping);
			this.ImportDerivedTypes(new XmlQualifiedName(type.Name, typeNs));
			return structMapping;
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x000A7364 File Offset: 0x000A6364
		private MemberMapping[] ImportTypeMembers(XmlSchemaComplexType type, string typeNs, CodeIdentifiers members)
		{
			if (type.AnyAttribute != null)
			{
				throw new InvalidOperationException(Res.GetString("XmlInvalidAnyAttributeUse", new object[]
				{
					type.Name,
					type.QualifiedName.Namespace
				}));
			}
			XmlSchemaObjectCollection attributes = type.Attributes;
			for (int i = 0; i < attributes.Count; i++)
			{
				object obj = attributes[i];
				if (obj is XmlSchemaAttributeGroup)
				{
					throw new InvalidOperationException(Res.GetString("XmlSoapInvalidAttributeUse", new object[]
					{
						type.Name,
						type.QualifiedName.Namespace
					}));
				}
				if (obj is XmlSchemaAttribute)
				{
					XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj;
					if (xmlSchemaAttribute.Use != XmlSchemaUse.Prohibited)
					{
						throw new InvalidOperationException(Res.GetString("XmlSoapInvalidAttributeUse", new object[]
						{
							type.Name,
							type.QualifiedName.Namespace
						}));
					}
				}
			}
			if (type.Particle != null)
			{
				this.ImportGroup(type.Particle, members, typeNs);
			}
			else if (type.ContentModel != null && type.ContentModel is XmlSchemaComplexContent)
			{
				XmlSchemaComplexContent xmlSchemaComplexContent = (XmlSchemaComplexContent)type.ContentModel;
				if (xmlSchemaComplexContent.Content is XmlSchemaComplexContentExtension)
				{
					if (((XmlSchemaComplexContentExtension)xmlSchemaComplexContent.Content).Particle != null)
					{
						this.ImportGroup(((XmlSchemaComplexContentExtension)xmlSchemaComplexContent.Content).Particle, members, typeNs);
					}
				}
				else if (xmlSchemaComplexContent.Content is XmlSchemaComplexContentRestriction && ((XmlSchemaComplexContentRestriction)xmlSchemaComplexContent.Content).Particle != null)
				{
					this.ImportGroup(((XmlSchemaComplexContentRestriction)xmlSchemaComplexContent.Content).Particle, members, typeNs);
				}
			}
			return (MemberMapping[])members.ToArray(typeof(MemberMapping));
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x000A7528 File Offset: 0x000A6528
		private void ImportGroup(XmlSchemaParticle group, CodeIdentifiers members, string ns)
		{
			if (group is XmlSchemaChoice)
			{
				XmlQualifiedName parentName = XmlSchemas.GetParentName(group);
				throw new InvalidOperationException(Res.GetString("XmlSoapInvalidChoice", new object[] { parentName.Name, parentName.Namespace }));
			}
			this.ImportGroupMembers(group, members, ns);
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x000A7578 File Offset: 0x000A6578
		private void ImportGroupMembers(XmlSchemaParticle particle, CodeIdentifiers members, string ns)
		{
			XmlQualifiedName parentName = XmlSchemas.GetParentName(particle);
			if (particle is XmlSchemaGroupRef)
			{
				throw new InvalidOperationException(Res.GetString("XmlSoapUnsupportedGroupRef", new object[] { parentName.Name, parentName.Namespace }));
			}
			if (particle is XmlSchemaGroupBase)
			{
				XmlSchemaGroupBase xmlSchemaGroupBase = (XmlSchemaGroupBase)particle;
				if (xmlSchemaGroupBase.IsMultipleOccurrence)
				{
					throw new InvalidOperationException(Res.GetString("XmlSoapUnsupportedGroupRepeat", new object[] { parentName.Name, parentName.Namespace }));
				}
				for (int i = 0; i < xmlSchemaGroupBase.Items.Count; i++)
				{
					object obj = xmlSchemaGroupBase.Items[i];
					if (obj is XmlSchemaGroupBase || obj is XmlSchemaGroupRef)
					{
						throw new InvalidOperationException(Res.GetString("XmlSoapUnsupportedGroupNested", new object[] { parentName.Name, parentName.Namespace }));
					}
					if (obj is XmlSchemaElement)
					{
						this.ImportElementMember((XmlSchemaElement)obj, members, ns);
					}
					else if (obj is XmlSchemaAny)
					{
						throw new InvalidOperationException(Res.GetString("XmlSoapUnsupportedGroupAny", new object[] { parentName.Name, parentName.Namespace }));
					}
				}
			}
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x000A76C0 File Offset: 0x000A66C0
		private ElementAccessor ImportArray(XmlSchemaElement element, string ns)
		{
			if (element.SchemaType == null)
			{
				return null;
			}
			if (!element.IsMultipleOccurrence)
			{
				return null;
			}
			XmlSchemaType schemaType = element.SchemaType;
			ArrayMapping arrayMapping = this.ImportArrayMapping(schemaType, ns);
			if (arrayMapping == null)
			{
				return null;
			}
			return new ElementAccessor
			{
				IsSoap = true,
				Name = element.Name,
				Namespace = ns,
				Mapping = arrayMapping,
				IsNullable = false,
				Form = XmlSchemaForm.None
			};
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x000A772C File Offset: 0x000A672C
		private ArrayMapping ImportArrayMapping(XmlSchemaType type, string ns)
		{
			ArrayMapping arrayMapping;
			if (type.Name == "Array" && ns == "http://schemas.xmlsoap.org/soap/encoding/")
			{
				arrayMapping = new ArrayMapping();
				TypeMapping rootMapping = base.GetRootMapping();
				ElementAccessor elementAccessor = new ElementAccessor();
				elementAccessor.IsSoap = true;
				elementAccessor.Name = "anyType";
				elementAccessor.Namespace = ns;
				elementAccessor.Mapping = rootMapping;
				elementAccessor.IsNullable = true;
				elementAccessor.Form = XmlSchemaForm.None;
				arrayMapping.Elements = new ElementAccessor[] { elementAccessor };
				arrayMapping.TypeDesc = elementAccessor.Mapping.TypeDesc.CreateArrayTypeDesc();
				arrayMapping.TypeName = "ArrayOf" + CodeIdentifier.MakePascal(elementAccessor.Mapping.TypeName);
				return arrayMapping;
			}
			if (!(type.DerivedFrom.Name == "Array") || !(type.DerivedFrom.Namespace == "http://schemas.xmlsoap.org/soap/encoding/"))
			{
				return null;
			}
			XmlSchemaContentModel contentModel = ((XmlSchemaComplexType)type).ContentModel;
			if (!(contentModel.Content is XmlSchemaComplexContentRestriction))
			{
				return null;
			}
			arrayMapping = new ArrayMapping();
			XmlSchemaComplexContentRestriction xmlSchemaComplexContentRestriction = (XmlSchemaComplexContentRestriction)contentModel.Content;
			for (int i = 0; i < xmlSchemaComplexContentRestriction.Attributes.Count; i++)
			{
				XmlSchemaAttribute xmlSchemaAttribute = xmlSchemaComplexContentRestriction.Attributes[i] as XmlSchemaAttribute;
				if (xmlSchemaAttribute != null && xmlSchemaAttribute.RefName.Name == "arrayType" && xmlSchemaAttribute.RefName.Namespace == "http://schemas.xmlsoap.org/soap/encoding/")
				{
					string text = null;
					if (xmlSchemaAttribute.UnhandledAttributes != null)
					{
						foreach (XmlAttribute xmlAttribute in xmlSchemaAttribute.UnhandledAttributes)
						{
							if (xmlAttribute.LocalName == "arrayType" && xmlAttribute.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/")
							{
								text = xmlAttribute.Value;
								break;
							}
						}
					}
					if (text != null)
					{
						string text2;
						XmlQualifiedName xmlQualifiedName = TypeScope.ParseWsdlArrayType(text, out text2, xmlSchemaAttribute);
						TypeDesc typeDesc = base.Scope.GetTypeDesc(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
						TypeMapping typeMapping;
						if (typeDesc != null && typeDesc.IsPrimitive)
						{
							typeMapping = new PrimitiveMapping();
							typeMapping.TypeDesc = typeDesc;
							typeMapping.TypeName = typeDesc.DataType.Name;
						}
						else
						{
							typeMapping = this.ImportType(xmlQualifiedName, false);
						}
						ElementAccessor elementAccessor2 = new ElementAccessor();
						elementAccessor2.IsSoap = true;
						elementAccessor2.Name = xmlQualifiedName.Name;
						elementAccessor2.Namespace = ns;
						elementAccessor2.Mapping = typeMapping;
						elementAccessor2.IsNullable = true;
						elementAccessor2.Form = XmlSchemaForm.None;
						arrayMapping.Elements = new ElementAccessor[] { elementAccessor2 };
						arrayMapping.TypeDesc = elementAccessor2.Mapping.TypeDesc.CreateArrayTypeDesc();
						arrayMapping.TypeName = "ArrayOf" + CodeIdentifier.MakePascal(elementAccessor2.Mapping.TypeName);
						return arrayMapping;
					}
				}
			}
			XmlSchemaParticle particle = xmlSchemaComplexContentRestriction.Particle;
			if (!(particle is XmlSchemaAll) && !(particle is XmlSchemaSequence))
			{
				return null;
			}
			XmlSchemaGroupBase xmlSchemaGroupBase = (XmlSchemaGroupBase)particle;
			if (xmlSchemaGroupBase.Items.Count != 1 || !(xmlSchemaGroupBase.Items[0] is XmlSchemaElement))
			{
				return null;
			}
			XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)xmlSchemaGroupBase.Items[0];
			if (!xmlSchemaElement.IsMultipleOccurrence)
			{
				return null;
			}
			ElementAccessor elementAccessor3 = this.ImportElement(xmlSchemaElement, ns);
			arrayMapping.Elements = new ElementAccessor[] { elementAccessor3 };
			arrayMapping.TypeDesc = elementAccessor3.Mapping.TypeDesc.CreateArrayTypeDesc();
			return arrayMapping;
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x000A7AC0 File Offset: 0x000A6AC0
		private void ImportElementMember(XmlSchemaElement element, CodeIdentifiers members, string ns)
		{
			ElementAccessor elementAccessor = this.ImportArray(element, ns) ?? this.ImportElement(element, ns);
			MemberMapping memberMapping = new MemberMapping();
			memberMapping.Name = CodeIdentifier.MakeValid(Accessor.UnescapeName(elementAccessor.Name));
			memberMapping.Name = members.AddUnique(memberMapping.Name, memberMapping);
			if (memberMapping.Name.EndsWith("Specified", StringComparison.Ordinal))
			{
				string name = memberMapping.Name;
				memberMapping.Name = members.AddUnique(memberMapping.Name, memberMapping);
				members.Remove(name);
			}
			memberMapping.TypeDesc = elementAccessor.Mapping.TypeDesc;
			memberMapping.Elements = new ElementAccessor[] { elementAccessor };
			if (element.IsMultipleOccurrence)
			{
				memberMapping.TypeDesc = memberMapping.TypeDesc.CreateArrayTypeDesc();
			}
			if (element.MinOccurs == 0m && memberMapping.TypeDesc.IsValueType && !memberMapping.TypeDesc.HasIsEmpty)
			{
				memberMapping.CheckSpecified = SpecifiedAccessor.ReadWrite;
			}
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x000A7BB8 File Offset: 0x000A6BB8
		private TypeMapping ImportDataType(XmlSchemaSimpleType dataType, string typeNs, string identifier, bool isList)
		{
			TypeMapping typeMapping = this.ImportNonXsdPrimitiveDataType(dataType, typeNs);
			if (typeMapping != null)
			{
				return typeMapping;
			}
			if (dataType.Content is XmlSchemaSimpleTypeRestriction)
			{
				XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = (XmlSchemaSimpleTypeRestriction)dataType.Content;
				using (XmlSchemaObjectEnumerator enumerator = xmlSchemaSimpleTypeRestriction.Facets.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						if (obj is XmlSchemaEnumerationFacet)
						{
							return this.ImportEnumeratedDataType(dataType, typeNs, identifier, isList);
						}
					}
					goto IL_0104;
				}
			}
			if (dataType.Content is XmlSchemaSimpleTypeList || dataType.Content is XmlSchemaSimpleTypeUnion)
			{
				if (dataType.Content is XmlSchemaSimpleTypeList)
				{
					XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = (XmlSchemaSimpleTypeList)dataType.Content;
					if (xmlSchemaSimpleTypeList.ItemType != null)
					{
						typeMapping = this.ImportDataType(xmlSchemaSimpleTypeList.ItemType, typeNs, identifier, true);
						if (typeMapping != null)
						{
							return typeMapping;
						}
					}
				}
				typeMapping = new PrimitiveMapping();
				typeMapping.TypeDesc = base.Scope.GetTypeDesc(typeof(string));
				typeMapping.TypeName = typeMapping.TypeDesc.DataType.Name;
				return typeMapping;
			}
			IL_0104:
			return this.ImportPrimitiveDataType(dataType);
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x000A7CE4 File Offset: 0x000A6CE4
		private TypeMapping ImportEnumeratedDataType(XmlSchemaSimpleType dataType, string typeNs, string identifier, bool isList)
		{
			TypeMapping typeMapping = (TypeMapping)base.ImportedMappings[dataType];
			if (typeMapping != null)
			{
				return typeMapping;
			}
			XmlSchemaSimpleType xmlSchemaSimpleType = this.FindDataType(dataType.DerivedFrom);
			TypeDesc typeDesc = base.Scope.GetTypeDesc(xmlSchemaSimpleType);
			if (typeDesc != null && typeDesc != base.Scope.GetTypeDesc(typeof(string)))
			{
				return this.ImportPrimitiveDataType(dataType);
			}
			identifier = Accessor.UnescapeName(identifier);
			string text = base.GenerateUniqueTypeName(identifier);
			EnumMapping enumMapping = new EnumMapping();
			enumMapping.IsReference = base.Schemas.IsReference(dataType);
			enumMapping.TypeDesc = new TypeDesc(text, text, TypeKind.Enum, null, TypeFlags.None);
			enumMapping.TypeName = identifier;
			enumMapping.Namespace = typeNs;
			enumMapping.IsFlags = isList;
			CodeIdentifiers codeIdentifiers = new CodeIdentifiers();
			if (!(dataType.Content is XmlSchemaSimpleTypeRestriction))
			{
				throw new InvalidOperationException(Res.GetString("XmlInvalidEnumContent", new object[]
				{
					dataType.Content.GetType().Name,
					identifier
				}));
			}
			XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = (XmlSchemaSimpleTypeRestriction)dataType.Content;
			for (int i = 0; i < xmlSchemaSimpleTypeRestriction.Facets.Count; i++)
			{
				object obj = xmlSchemaSimpleTypeRestriction.Facets[i];
				if (obj is XmlSchemaEnumerationFacet)
				{
					XmlSchemaEnumerationFacet xmlSchemaEnumerationFacet = (XmlSchemaEnumerationFacet)obj;
					ConstantMapping constantMapping = new ConstantMapping();
					string text2 = CodeIdentifier.MakeValid(xmlSchemaEnumerationFacet.Value);
					constantMapping.Name = codeIdentifiers.AddUnique(text2, constantMapping);
					constantMapping.XmlName = xmlSchemaEnumerationFacet.Value;
					constantMapping.Value = (long)i;
				}
			}
			enumMapping.Constants = (ConstantMapping[])codeIdentifiers.ToArray(typeof(ConstantMapping));
			if (isList && enumMapping.Constants.Length > 63)
			{
				typeMapping = new PrimitiveMapping();
				typeMapping.TypeDesc = base.Scope.GetTypeDesc(typeof(string));
				typeMapping.TypeName = typeMapping.TypeDesc.DataType.Name;
				base.ImportedMappings.Add(dataType, typeMapping);
				return typeMapping;
			}
			base.ImportedMappings.Add(dataType, enumMapping);
			base.Scope.AddTypeMapping(enumMapping);
			return enumMapping;
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x000A7EFC File Offset: 0x000A6EFC
		private PrimitiveMapping ImportPrimitiveDataType(XmlSchemaSimpleType dataType)
		{
			TypeDesc dataTypeSource = this.GetDataTypeSource(dataType);
			return new PrimitiveMapping
			{
				TypeDesc = dataTypeSource,
				TypeName = dataTypeSource.DataType.Name
			};
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x000A7F30 File Offset: 0x000A6F30
		private PrimitiveMapping ImportNonXsdPrimitiveDataType(XmlSchemaSimpleType dataType, string ns)
		{
			PrimitiveMapping primitiveMapping = null;
			if (dataType.Name != null && dataType.Name.Length != 0)
			{
				TypeDesc typeDesc = base.Scope.GetTypeDesc(dataType.Name, ns);
				if (typeDesc != null)
				{
					primitiveMapping = new PrimitiveMapping();
					primitiveMapping.TypeDesc = typeDesc;
					primitiveMapping.TypeName = typeDesc.DataType.Name;
				}
			}
			return primitiveMapping;
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x000A7F8C File Offset: 0x000A6F8C
		private TypeDesc GetDataTypeSource(XmlSchemaSimpleType dataType)
		{
			if (dataType.Name != null && dataType.Name.Length != 0)
			{
				TypeDesc typeDesc = base.Scope.GetTypeDesc(dataType);
				if (typeDesc != null)
				{
					return typeDesc;
				}
			}
			if (!dataType.DerivedFrom.IsEmpty)
			{
				return this.GetDataTypeSource(this.FindDataType(dataType.DerivedFrom));
			}
			return base.Scope.GetTypeDesc(typeof(string));
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x000A7FF8 File Offset: 0x000A6FF8
		private XmlSchemaSimpleType FindDataType(XmlQualifiedName name)
		{
			TypeDesc typeDesc = base.Scope.GetTypeDesc(name.Name, name.Namespace);
			if (typeDesc != null && typeDesc.DataType is XmlSchemaSimpleType)
			{
				return (XmlSchemaSimpleType)typeDesc.DataType;
			}
			XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)base.Schemas.Find(name, typeof(XmlSchemaSimpleType));
			if (xmlSchemaSimpleType != null)
			{
				return xmlSchemaSimpleType;
			}
			if (name.Namespace == "http://www.w3.org/2001/XMLSchema")
			{
				return (XmlSchemaSimpleType)base.Scope.GetTypeDesc(typeof(string)).DataType;
			}
			throw new InvalidOperationException(Res.GetString("XmlMissingDataType", new object[] { name.ToString() }));
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x000A80AC File Offset: 0x000A70AC
		private object FindType(XmlQualifiedName name)
		{
			if (name != null && name.Namespace == "http://schemas.xmlsoap.org/soap/encoding/")
			{
				object obj = base.Schemas.Find(name, typeof(XmlSchemaComplexType));
				if (obj == null)
				{
					return this.FindDataType(name);
				}
				XmlSchemaType xmlSchemaType = (XmlSchemaType)obj;
				XmlQualifiedName derivedFrom = xmlSchemaType.DerivedFrom;
				if (!derivedFrom.IsEmpty)
				{
					return this.FindType(derivedFrom);
				}
				return xmlSchemaType;
			}
			else
			{
				object obj2 = base.Schemas.Find(name, typeof(XmlSchemaComplexType));
				if (obj2 != null)
				{
					return obj2;
				}
				return this.FindDataType(name);
			}
		}
	}
}
