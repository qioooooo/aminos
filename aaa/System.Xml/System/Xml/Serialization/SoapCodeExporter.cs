using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Security.Permissions;

namespace System.Xml.Serialization
{
	// Token: 0x020002EA RID: 746
	public class SoapCodeExporter : CodeExporter
	{
		// Token: 0x060022DE RID: 8926 RVA: 0x000A3D26 File Offset: 0x000A2D26
		public SoapCodeExporter(CodeNamespace codeNamespace)
			: base(codeNamespace, null, null, CodeGenerationOptions.GenerateProperties, null)
		{
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x000A3D33 File Offset: 0x000A2D33
		public SoapCodeExporter(CodeNamespace codeNamespace, CodeCompileUnit codeCompileUnit)
			: base(codeNamespace, codeCompileUnit, null, CodeGenerationOptions.GenerateProperties, null)
		{
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x000A3D40 File Offset: 0x000A2D40
		public SoapCodeExporter(CodeNamespace codeNamespace, CodeCompileUnit codeCompileUnit, CodeGenerationOptions options)
			: base(codeNamespace, codeCompileUnit, null, CodeGenerationOptions.GenerateProperties, null)
		{
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x000A3D4D File Offset: 0x000A2D4D
		public SoapCodeExporter(CodeNamespace codeNamespace, CodeCompileUnit codeCompileUnit, CodeGenerationOptions options, Hashtable mappings)
			: base(codeNamespace, codeCompileUnit, null, options, mappings)
		{
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x000A3D5B File Offset: 0x000A2D5B
		public SoapCodeExporter(CodeNamespace codeNamespace, CodeCompileUnit codeCompileUnit, CodeDomProvider codeProvider, CodeGenerationOptions options, Hashtable mappings)
			: base(codeNamespace, codeCompileUnit, codeProvider, options, mappings)
		{
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x000A3D6A File Offset: 0x000A2D6A
		public void ExportTypeMapping(XmlTypeMapping xmlTypeMapping)
		{
			xmlTypeMapping.CheckShallow();
			base.CheckScope(xmlTypeMapping.Scope);
			this.ExportElement(xmlTypeMapping.Accessor);
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x000A3D8C File Offset: 0x000A2D8C
		public void ExportMembersMapping(XmlMembersMapping xmlMembersMapping)
		{
			xmlMembersMapping.CheckShallow();
			base.CheckScope(xmlMembersMapping.Scope);
			for (int i = 0; i < xmlMembersMapping.Count; i++)
			{
				this.ExportElement((ElementAccessor)xmlMembersMapping[i].Accessor);
			}
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x000A3DD3 File Offset: 0x000A2DD3
		private void ExportElement(ElementAccessor element)
		{
			this.ExportType(element.Mapping);
		}

		// Token: 0x060022E6 RID: 8934 RVA: 0x000A3DE4 File Offset: 0x000A2DE4
		private void ExportType(TypeMapping mapping)
		{
			if (mapping.IsReference)
			{
				return;
			}
			if (base.ExportedMappings[mapping] == null)
			{
				CodeTypeDeclaration codeTypeDeclaration = null;
				base.ExportedMappings.Add(mapping, mapping);
				if (mapping is EnumMapping)
				{
					codeTypeDeclaration = base.ExportEnum((EnumMapping)mapping, typeof(SoapEnumAttribute));
				}
				else if (mapping is StructMapping)
				{
					codeTypeDeclaration = this.ExportStruct((StructMapping)mapping);
				}
				else if (mapping is ArrayMapping)
				{
					this.EnsureTypesExported(((ArrayMapping)mapping).Elements, null);
				}
				if (codeTypeDeclaration != null)
				{
					codeTypeDeclaration.CustomAttributes.Add(base.GeneratedCodeAttribute);
					codeTypeDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(SerializableAttribute).FullName));
					if (!codeTypeDeclaration.IsEnum)
					{
						codeTypeDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(DebuggerStepThroughAttribute).FullName));
						codeTypeDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(DesignerCategoryAttribute).FullName, new CodeAttributeArgument[]
						{
							new CodeAttributeArgument(new CodePrimitiveExpression("code"))
						}));
					}
					base.AddTypeMetadata(codeTypeDeclaration.CustomAttributes, typeof(SoapTypeAttribute), mapping.TypeDesc.Name, Accessor.UnescapeName(mapping.TypeName), mapping.Namespace, mapping.IncludeInSchema);
					base.ExportedClasses.Add(mapping, codeTypeDeclaration);
				}
			}
		}

		// Token: 0x060022E7 RID: 8935 RVA: 0x000A3F4C File Offset: 0x000A2F4C
		private CodeTypeDeclaration ExportStruct(StructMapping mapping)
		{
			if (mapping.TypeDesc.IsRoot)
			{
				base.ExportRoot(mapping, typeof(SoapIncludeAttribute));
				return null;
			}
			if (!mapping.IncludeInSchema)
			{
				return null;
			}
			string name = mapping.TypeDesc.Name;
			string text = ((mapping.TypeDesc.BaseTypeDesc == null) ? string.Empty : mapping.TypeDesc.BaseTypeDesc.Name);
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(name);
			codeTypeDeclaration.IsPartial = base.CodeProvider.Supports(GeneratorSupport.PartialTypes);
			codeTypeDeclaration.Comments.Add(new CodeCommentStatement(Res.GetString("XmlRemarks"), true));
			base.CodeNamespace.Types.Add(codeTypeDeclaration);
			if (text != null && text.Length > 0)
			{
				codeTypeDeclaration.BaseTypes.Add(text);
			}
			else
			{
				base.AddPropertyChangedNotifier(codeTypeDeclaration);
			}
			codeTypeDeclaration.TypeAttributes |= TypeAttributes.Public;
			if (mapping.TypeDesc.IsAbstract)
			{
				codeTypeDeclaration.TypeAttributes |= TypeAttributes.Abstract;
			}
			CodeExporter.AddIncludeMetadata(codeTypeDeclaration.CustomAttributes, mapping, typeof(SoapIncludeAttribute));
			if (base.GenerateProperties)
			{
				for (int i = 0; i < mapping.Members.Length; i++)
				{
					this.ExportProperty(codeTypeDeclaration, mapping.Members[i], mapping.Scope);
				}
			}
			else
			{
				for (int j = 0; j < mapping.Members.Length; j++)
				{
					this.ExportMember(codeTypeDeclaration, mapping.Members[j]);
				}
			}
			for (int k = 0; k < mapping.Members.Length; k++)
			{
				this.EnsureTypesExported(mapping.Members[k].Elements, null);
			}
			if (mapping.BaseMapping != null)
			{
				this.ExportType(mapping.BaseMapping);
			}
			this.ExportDerivedStructs(mapping);
			CodeGenerator.ValidateIdentifiers(codeTypeDeclaration);
			return codeTypeDeclaration;
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x000A410C File Offset: 0x000A310C
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		internal override void ExportDerivedStructs(StructMapping mapping)
		{
			for (StructMapping structMapping = mapping.DerivedMappings; structMapping != null; structMapping = structMapping.NextDerivedMapping)
			{
				this.ExportType(structMapping);
			}
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x000A4133 File Offset: 0x000A3133
		public void AddMappingMetadata(CodeAttributeDeclarationCollection metadata, XmlMemberMapping member, bool forceUseMemberName)
		{
			this.AddMemberMetadata(metadata, member.Mapping, forceUseMemberName);
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x000A4143 File Offset: 0x000A3143
		public void AddMappingMetadata(CodeAttributeDeclarationCollection metadata, XmlMemberMapping member)
		{
			this.AddMemberMetadata(metadata, member.Mapping, false);
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x000A4154 File Offset: 0x000A3154
		private void AddElementMetadata(CodeAttributeDeclarationCollection metadata, string elementName, TypeDesc typeDesc, bool isNullable)
		{
			CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(typeof(SoapElementAttribute).FullName);
			if (elementName != null)
			{
				codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(elementName)));
			}
			if (typeDesc != null && typeDesc.IsAmbiguousDataType)
			{
				codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("DataType", new CodePrimitiveExpression(typeDesc.DataType.Name)));
			}
			if (isNullable)
			{
				codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("IsNullable", new CodePrimitiveExpression(true)));
			}
			metadata.Add(codeAttributeDeclaration);
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x000A41F0 File Offset: 0x000A31F0
		private void AddMemberMetadata(CodeAttributeDeclarationCollection metadata, MemberMapping member, bool forceUseMemberName)
		{
			if (member.Elements.Length == 0)
			{
				return;
			}
			ElementAccessor elementAccessor = member.Elements[0];
			TypeMapping mapping = elementAccessor.Mapping;
			string text = Accessor.UnescapeName(elementAccessor.Name);
			bool flag = text == member.Name && !forceUseMemberName;
			if (!flag || mapping.TypeDesc.IsAmbiguousDataType || elementAccessor.IsNullable)
			{
				this.AddElementMetadata(metadata, flag ? null : text, mapping.TypeDesc.IsAmbiguousDataType ? mapping.TypeDesc : null, elementAccessor.IsNullable);
			}
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x000A427C File Offset: 0x000A327C
		private void ExportMember(CodeTypeDeclaration codeClass, MemberMapping member)
		{
			string typeName = member.GetTypeName(base.CodeProvider);
			CodeMemberField codeMemberField = new CodeMemberField(typeName, member.Name);
			codeMemberField.Attributes = (codeMemberField.Attributes & (MemberAttributes)(-61441)) | MemberAttributes.Public;
			codeMemberField.Comments.Add(new CodeCommentStatement(Res.GetString("XmlRemarks"), true));
			codeClass.Members.Add(codeMemberField);
			this.AddMemberMetadata(codeMemberField.CustomAttributes, member, false);
			if (member.CheckSpecified != SpecifiedAccessor.None)
			{
				codeMemberField = new CodeMemberField(typeof(bool).FullName, member.Name + "Specified");
				codeMemberField.Attributes = (codeMemberField.Attributes & (MemberAttributes)(-61441)) | MemberAttributes.Public;
				codeMemberField.Comments.Add(new CodeCommentStatement(Res.GetString("XmlRemarks"), true));
				CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(typeof(SoapIgnoreAttribute).FullName);
				codeMemberField.CustomAttributes.Add(codeAttributeDeclaration);
				codeClass.Members.Add(codeMemberField);
			}
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x000A4388 File Offset: 0x000A3388
		private void ExportProperty(CodeTypeDeclaration codeClass, MemberMapping member, CodeIdentifiers memberScope)
		{
			string text = memberScope.AddUnique(CodeExporter.MakeFieldName(member.Name), member);
			string typeName = member.GetTypeName(base.CodeProvider);
			CodeMemberField codeMemberField = new CodeMemberField(typeName, text);
			codeMemberField.Attributes = MemberAttributes.Private;
			codeClass.Members.Add(codeMemberField);
			CodeMemberProperty codeMemberProperty = base.CreatePropertyDeclaration(codeMemberField, member.Name, typeName);
			codeMemberProperty.Comments.Add(new CodeCommentStatement(Res.GetString("XmlRemarks"), true));
			this.AddMemberMetadata(codeMemberProperty.CustomAttributes, member, false);
			codeClass.Members.Add(codeMemberProperty);
			if (member.CheckSpecified != SpecifiedAccessor.None)
			{
				codeMemberField = new CodeMemberField(typeof(bool).FullName, text + "Specified");
				codeMemberField.Attributes = MemberAttributes.Private;
				codeClass.Members.Add(codeMemberField);
				codeMemberProperty = base.CreatePropertyDeclaration(codeMemberField, member.Name + "Specified", typeof(bool).FullName);
				codeMemberProperty.Comments.Add(new CodeCommentStatement(Res.GetString("XmlRemarks"), true));
				CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(typeof(SoapIgnoreAttribute).FullName);
				codeMemberProperty.CustomAttributes.Add(codeAttributeDeclaration);
				codeClass.Members.Add(codeMemberProperty);
			}
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x000A44D4 File Offset: 0x000A34D4
		internal override void EnsureTypesExported(Accessor[] accessors, string ns)
		{
			if (accessors == null)
			{
				return;
			}
			for (int i = 0; i < accessors.Length; i++)
			{
				this.ExportType(accessors[i].Mapping);
			}
		}
	}
}
