using System;
using System.CodeDom;

namespace System.Data.Design
{
	// Token: 0x020000D3 RID: 211
	internal sealed class TypedRowHandler
	{
		// Token: 0x060008E0 RID: 2272 RVA: 0x0001D67A File Offset: 0x0001C67A
		internal TypedRowHandler(TypedDataSourceCodeGenerator codeGenerator, DesignTableCollection tables)
		{
			this.codeGenerator = codeGenerator;
			this.tables = tables;
			this.rowGenerator = new TypedRowGenerator(codeGenerator);
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x0001D69C File Offset: 0x0001C69C
		internal TypedRowGenerator RowGenerator
		{
			get
			{
				return this.rowGenerator;
			}
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0001D6A4 File Offset: 0x0001C6A4
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

		// Token: 0x060008E3 RID: 2275 RVA: 0x0001D736 File Offset: 0x0001C736
		internal void AddTypedRows(CodeTypeDeclaration dataSourceClass)
		{
			this.rowGenerator.GenerateRows(dataSourceClass);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0001D744 File Offset: 0x0001C744
		internal void AddTypedRowEventHandlers(CodeTypeDeclaration dataSourceClass)
		{
			this.rowGenerator.GenerateTypedRowEventHandlers(dataSourceClass);
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0001D752 File Offset: 0x0001C752
		internal void AddTypedRowEventArgs(CodeTypeDeclaration dataSourceClass)
		{
			this.rowGenerator.GenerateTypedRowEventArgs(dataSourceClass);
		}

		// Token: 0x04000CAC RID: 3244
		private TypedDataSourceCodeGenerator codeGenerator;

		// Token: 0x04000CAD RID: 3245
		private DesignTableCollection tables;

		// Token: 0x04000CAE RID: 3246
		private TypedRowGenerator rowGenerator;
	}
}
