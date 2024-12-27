using System;
using System.CodeDom;
using System.Collections;
using System.ComponentModel;

namespace System.Data.Design
{
	// Token: 0x020000D5 RID: 213
	internal sealed class TypedTableHandler
	{
		// Token: 0x060008EC RID: 2284 RVA: 0x0001DA96 File Offset: 0x0001CA96
		internal TypedTableHandler(TypedDataSourceCodeGenerator codeGenerator, DesignTableCollection tables)
		{
			this.codeGenerator = codeGenerator;
			this.tables = tables;
			this.tableGenerator = new TypedTableGenerator(codeGenerator);
			this.SetColumnHandlers();
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x0001DABE File Offset: 0x0001CABE
		internal DesignTableCollection Tables
		{
			get
			{
				return this.tables;
			}
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0001DAC6 File Offset: 0x0001CAC6
		internal TypedColumnHandler GetColumnHandler(string tableName)
		{
			if (tableName == null)
			{
				return null;
			}
			return (TypedColumnHandler)this.columnHandlers[tableName];
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0001DAE0 File Offset: 0x0001CAE0
		internal void AddPrivateVars(CodeTypeDeclaration dataSourceClass)
		{
			if (this.tables == null)
			{
				return;
			}
			foreach (object obj in this.tables)
			{
				DesignTable designTable = (DesignTable)obj;
				string generatorTableClassName = designTable.GeneratorTableClassName;
				string generatorTableVarName = designTable.GeneratorTableVarName;
				dataSourceClass.Members.Add(CodeGenHelper.FieldDecl(CodeGenHelper.Type(generatorTableClassName), generatorTableVarName));
			}
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0001DB64 File Offset: 0x0001CB64
		internal void AddTableProperties(CodeTypeDeclaration dataSourceClass)
		{
			if (this.tables == null)
			{
				return;
			}
			foreach (object obj in this.tables)
			{
				DesignTable designTable = (DesignTable)obj;
				string generatorTableClassName = designTable.GeneratorTableClassName;
				string generatorTablePropName = designTable.GeneratorTablePropName;
				string generatorTableVarName = designTable.GeneratorTableVarName;
				CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.Type(generatorTableClassName), generatorTablePropName, (MemberAttributes)24578);
				codeMemberProperty.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.Browsable", CodeGenHelper.Primitive(false)));
				codeMemberProperty.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.DesignerSerializationVisibility", CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DesignerSerializationVisibility)), "Content")));
				codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Field(CodeGenHelper.This(), generatorTableVarName)));
				dataSourceClass.Members.Add(codeMemberProperty);
			}
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0001DC74 File Offset: 0x0001CC74
		internal void AddTableClasses(CodeTypeDeclaration dataSourceClass)
		{
			this.tableGenerator.GenerateTables(dataSourceClass);
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0001DC84 File Offset: 0x0001CC84
		private void SetColumnHandlers()
		{
			this.columnHandlers = new Hashtable();
			foreach (object obj in this.tables)
			{
				DesignTable designTable = (DesignTable)obj;
				this.columnHandlers.Add(designTable.Name, new TypedColumnHandler(designTable, this.codeGenerator));
			}
		}

		// Token: 0x04000CB1 RID: 3249
		private TypedDataSourceCodeGenerator codeGenerator;

		// Token: 0x04000CB2 RID: 3250
		private TypedTableGenerator tableGenerator;

		// Token: 0x04000CB3 RID: 3251
		private DesignTableCollection tables;

		// Token: 0x04000CB4 RID: 3252
		private Hashtable columnHandlers;
	}
}
