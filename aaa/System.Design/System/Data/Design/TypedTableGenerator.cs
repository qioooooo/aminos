﻿using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Reflection;
using System.Xml.Serialization;

namespace System.Data.Design
{
	// Token: 0x020000D4 RID: 212
	internal sealed class TypedTableGenerator
	{
		// Token: 0x060008E6 RID: 2278 RVA: 0x0001D760 File Offset: 0x0001C760
		internal TypedTableGenerator(TypedDataSourceCodeGenerator codeGenerator)
		{
			this.codeGenerator = codeGenerator;
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0001D770 File Offset: 0x0001C770
		internal void GenerateTables(CodeTypeDeclaration dataSourceClass)
		{
			if (dataSourceClass == null)
			{
				throw new InternalException("DataSource CodeTypeDeclaration should not be null.");
			}
			foreach (object obj in this.codeGenerator.TableHandler.Tables)
			{
				DesignTable designTable = (DesignTable)obj;
				dataSourceClass.Members.Add(this.GenerateTable(designTable, dataSourceClass));
			}
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0001D7F0 File Offset: 0x0001C7F0
		private CodeTypeDeclaration GenerateTable(DesignTable designTable, CodeTypeDeclaration dataSourceClass)
		{
			string generatorTableClassName = designTable.GeneratorTableClassName;
			TypedColumnHandler columnHandler = this.codeGenerator.TableHandler.GetColumnHandler(designTable.Name);
			CodeTypeDeclaration codeTypeDeclaration = CodeGenHelper.Class(generatorTableClassName, true, TypeAttributes.Public);
			if ((this.codeGenerator.GenerateOptions & TypedDataSetGenerator.GenerateOption.LinqOverTypedDatasets) == TypedDataSetGenerator.GenerateOption.LinqOverTypedDatasets)
			{
				codeTypeDeclaration.BaseTypes.Add(CodeGenHelper.GlobalGenericType(TypedTableGenerator.LINQOverTDSTableBaseClass, CodeGenHelper.Type(designTable.GeneratorRowClassName)));
			}
			else
			{
				codeTypeDeclaration.BaseTypes.Add(CodeGenHelper.GlobalType(typeof(DataTable)));
				codeTypeDeclaration.BaseTypes.Add(CodeGenHelper.GlobalType(typeof(IEnumerable)));
			}
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.Serializable"));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl(typeof(XmlSchemaProviderAttribute).FullName, CodeGenHelper.Primitive("GetTypedTableSchema")));
			codeTypeDeclaration.Comments.Add(CodeGenHelper.Comment("Represents the strongly named DataTable class.", true));
			columnHandler.AddPrivateVariables(codeTypeDeclaration);
			columnHandler.AddTableColumnProperties(codeTypeDeclaration);
			codeTypeDeclaration.Members.Add(this.CountProperty());
			if (this.codeGenerator.CodeProvider.Supports(GeneratorSupport.DeclareIndexerProperties))
			{
				codeTypeDeclaration.Members.Add(this.IndexProperty(designTable));
			}
			if (this.codeGenerator.CodeProvider.Supports(GeneratorSupport.DeclareEvents) && this.codeGenerator.CodeProvider.Supports(GeneratorSupport.DeclareDelegates))
			{
				this.codeGenerator.RowHandler.AddTypedRowEvents(codeTypeDeclaration, designTable.Name);
			}
			TableMethodGenerator tableMethodGenerator = new TableMethodGenerator(this.codeGenerator, designTable);
			tableMethodGenerator.AddMethods(codeTypeDeclaration);
			return codeTypeDeclaration;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0001D988 File Offset: 0x0001C988
		private CodeMemberProperty CountProperty()
		{
			CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.GlobalType(typeof(int)), "Count", (MemberAttributes)24578);
			codeMemberProperty.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.Browsable", CodeGenHelper.Primitive(false)));
			codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), "Rows"), "Count")));
			return codeMemberProperty;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0001DA00 File Offset: 0x0001CA00
		private CodeMemberProperty IndexProperty(DesignTable designTable)
		{
			string generatorRowClassName = designTable.GeneratorRowClassName;
			CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.Type(generatorRowClassName), "Item", (MemberAttributes)24578);
			codeMemberProperty.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(int)), "index"));
			codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Cast(CodeGenHelper.Type(generatorRowClassName), CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.This(), "Rows"), CodeGenHelper.Argument("index")))));
			return codeMemberProperty;
		}

		// Token: 0x04000CAF RID: 3247
		private TypedDataSourceCodeGenerator codeGenerator;

		// Token: 0x04000CB0 RID: 3248
		private static string LINQOverTDSTableBaseClass = "System.Data.TypedTableBase";
	}
}
