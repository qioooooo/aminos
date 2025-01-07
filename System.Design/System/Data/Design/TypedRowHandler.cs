using System;
using System.CodeDom;

namespace System.Data.Design
{
	internal sealed class TypedRowHandler
	{
		internal TypedRowHandler(TypedDataSourceCodeGenerator codeGenerator, DesignTableCollection tables)
		{
			this.codeGenerator = codeGenerator;
			this.tables = tables;
			this.rowGenerator = new TypedRowGenerator(codeGenerator);
		}

		internal TypedRowGenerator RowGenerator
		{
			get
			{
				return this.rowGenerator;
			}
		}

		internal void AddTypedRowEvents(CodeTypeDeclaration dataTableClass, string tableName)
		{
			DesignTable designTable = this.codeGenerator.TableHandler.Tables[tableName];
			string generatorRowClassName = designTable.GeneratorRowClassName;
			string generatorRowEvHandlerName = designTable.GeneratorRowEvHandlerName;
			dataTableClass.Members.Add(CodeGenHelper.EventDecl(generatorRowEvHandlerName, designTable.GeneratorRowChangingName));
			dataTableClass.Members.Add(CodeGenHelper.EventDecl(generatorRowEvHandlerName, designTable.GeneratorRowChangedName));
			dataTableClass.Members.Add(CodeGenHelper.EventDecl(generatorRowEvHandlerName, designTable.GeneratorRowDeletingName));
			dataTableClass.Members.Add(CodeGenHelper.EventDecl(generatorRowEvHandlerName, designTable.GeneratorRowDeletedName));
		}

		internal void AddTypedRows(CodeTypeDeclaration dataSourceClass)
		{
			this.rowGenerator.GenerateRows(dataSourceClass);
		}

		internal void AddTypedRowEventHandlers(CodeTypeDeclaration dataSourceClass)
		{
			this.rowGenerator.GenerateTypedRowEventHandlers(dataSourceClass);
		}

		internal void AddTypedRowEventArgs(CodeTypeDeclaration dataSourceClass)
		{
			this.rowGenerator.GenerateTypedRowEventArgs(dataSourceClass);
		}

		private TypedDataSourceCodeGenerator codeGenerator;

		private DesignTableCollection tables;

		private TypedRowGenerator rowGenerator;
	}
}
