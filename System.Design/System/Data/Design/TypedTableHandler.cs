using System;
using System.CodeDom;
using System.Collections;
using System.ComponentModel;

namespace System.Data.Design
{
	internal sealed class TypedTableHandler
	{
		internal TypedTableHandler(TypedDataSourceCodeGenerator codeGenerator, DesignTableCollection tables)
		{
			this.codeGenerator = codeGenerator;
			this.tables = tables;
			this.tableGenerator = new TypedTableGenerator(codeGenerator);
			this.SetColumnHandlers();
		}

		internal DesignTableCollection Tables
		{
			get
			{
				return this.tables;
			}
		}

		internal TypedColumnHandler GetColumnHandler(string tableName)
		{
			if (tableName == null)
			{
				return null;
			}
			return (TypedColumnHandler)this.columnHandlers[tableName];
		}

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

		internal void AddTableClasses(CodeTypeDeclaration dataSourceClass)
		{
			this.tableGenerator.GenerateTables(dataSourceClass);
		}

		private void SetColumnHandlers()
		{
			this.columnHandlers = new Hashtable();
			foreach (object obj in this.tables)
			{
				DesignTable designTable = (DesignTable)obj;
				this.columnHandlers.Add(designTable.Name, new TypedColumnHandler(designTable, this.codeGenerator));
			}
		}

		private TypedDataSourceCodeGenerator codeGenerator;

		private TypedTableGenerator tableGenerator;

		private DesignTableCollection tables;

		private Hashtable columnHandlers;
	}
}
