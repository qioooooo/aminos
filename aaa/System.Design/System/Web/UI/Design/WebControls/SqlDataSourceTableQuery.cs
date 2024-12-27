using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design.Data;
using System.Data.Common;
using System.Text;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004E1 RID: 1249
	internal sealed class SqlDataSourceTableQuery
	{
		// Token: 0x06002CCC RID: 11468 RVA: 0x000FD579 File Offset: 0x000FC579
		public SqlDataSourceTableQuery(DesignerDataConnection designerDataConnection, DesignerDataTableBase designerDataTable)
		{
			this._designerDataConnection = designerDataConnection;
			this._designerDataTable = designerDataTable;
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002CCD RID: 11469 RVA: 0x000FD5B0 File Offset: 0x000FC5B0
		// (set) Token: 0x06002CCE RID: 11470 RVA: 0x000FD5B8 File Offset: 0x000FC5B8
		public bool AsteriskField
		{
			get
			{
				return this._asteriskField;
			}
			set
			{
				this._asteriskField = value;
				if (value)
				{
					this.Fields.Clear();
				}
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002CCF RID: 11471 RVA: 0x000FD5CF File Offset: 0x000FC5CF
		public DesignerDataConnection DesignerDataConnection
		{
			get
			{
				return this._designerDataConnection;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002CD0 RID: 11472 RVA: 0x000FD5D7 File Offset: 0x000FC5D7
		public DesignerDataTableBase DesignerDataTable
		{
			get
			{
				return this._designerDataTable;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002CD1 RID: 11473 RVA: 0x000FD5DF File Offset: 0x000FC5DF
		// (set) Token: 0x06002CD2 RID: 11474 RVA: 0x000FD5E7 File Offset: 0x000FC5E7
		public bool Distinct
		{
			get
			{
				return this._distinct;
			}
			set
			{
				/*
An exception occurred when decompiling this method (06002CD2)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Web.UI.Design.WebControls.SqlDataSourceTableQuery::set_Distinct(System.Boolean)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILInlining.InlineAllInBlock(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILInlining.cs:line 153
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 332
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06002CD3 RID: 11475 RVA: 0x000FD5F0 File Offset: 0x000FC5F0
		public IList<DesignerDataColumn> Fields
		{
			get
			{
				/*
An exception occurred when decompiling this method (06002CD3)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Collections.Generic.IList`1<System.ComponentModel.Design.Data.DesignerDataColumn> System.Web.UI.Design.WebControls.SqlDataSourceTableQuery::get_Fields()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at dnSpy.Contracts.Decompiler.MethodDebugInfoBuilder..ctor(Int32 decompilerSettingsVersion, StateMachineKind stateMachineKind, MethodDef method, MethodDef kickoffMethod) in D:\a\dnSpy\dnSpy\dnSpy\dnSpy.Contracts.Logic\Decompiler\MethodDebugInfoBuilder.cs:line 79
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 103
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06002CD4 RID: 11476 RVA: 0x000FD5F8 File Offset: 0x000FC5F8
		public IList<SqlDataSourceFilterClause> FilterClauses
		{
			get
			{
				return this._filterClauses;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06002CD5 RID: 11477 RVA: 0x000FD600 File Offset: 0x000FC600
		public IList<SqlDataSourceOrderClause> OrderClauses
		{
			get
			{
				return this._orderClauses;
			}
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000FD608 File Offset: 0x000FC608
		private void AppendWhereClauseParameter(StringBuilder commandText, SqlDataSourceColumnData columnData, string oldValuesFormatString)
		{
			string escapedName = columnData.EscapedName;
			string oldValueParameterPlaceHolder = columnData.GetOldValueParameterPlaceHolder(oldValuesFormatString);
			if (columnData.Column.Nullable)
			{
				commandText.Append("((");
				commandText.Append(escapedName);
				commandText.Append(" = ");
				commandText.Append(oldValueParameterPlaceHolder);
				commandText.Append(") OR (");
				commandText.Append(escapedName);
				commandText.Append(" IS NULL AND ");
				commandText.Append(oldValueParameterPlaceHolder);
				commandText.Append(" IS NULL))");
				return;
			}
			commandText.Append(escapedName);
			commandText.Append(" = ");
			commandText.Append(oldValueParameterPlaceHolder);
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000FD6AA File Offset: 0x000FC6AA
		private bool CanAutoGenerateQueries()
		{
			return !this.Distinct && (this.AsteriskField || this._fields.Count != 0);
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000FD6D0 File Offset: 0x000FC6D0
		public SqlDataSourceTableQuery Clone()
		{
			SqlDataSourceTableQuery sqlDataSourceTableQuery = new SqlDataSourceTableQuery(this.DesignerDataConnection, this.DesignerDataTable);
			sqlDataSourceTableQuery.Distinct = this.Distinct;
			sqlDataSourceTableQuery.AsteriskField = this.AsteriskField;
			foreach (DesignerDataColumn designerDataColumn in this.Fields)
			{
				sqlDataSourceTableQuery.Fields.Add(designerDataColumn);
			}
			foreach (SqlDataSourceFilterClause sqlDataSourceFilterClause in this.FilterClauses)
			{
				sqlDataSourceTableQuery.FilterClauses.Add(sqlDataSourceFilterClause);
			}
			foreach (SqlDataSourceOrderClause sqlDataSourceOrderClause in this.OrderClauses)
			{
				sqlDataSourceTableQuery.OrderClauses.Add(sqlDataSourceOrderClause);
			}
			return sqlDataSourceTableQuery;
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x000FD7E0 File Offset: 0x000FC7E0
		public SqlDataSourceQuery GetDeleteQuery(string oldValuesFormatString, bool includeOldValues)
		{
			if (!this.CanAutoGenerateQueries())
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder("DELETE FROM ");
			stringBuilder.Append(this.GetTableName());
			SqlDataSourceQuery whereClause = this.GetWhereClause(oldValuesFormatString, includeOldValues);
			if (whereClause == null)
			{
				return null;
			}
			stringBuilder.Append(whereClause.Command);
			return new SqlDataSourceQuery(stringBuilder.ToString(), SqlDataSourceCommandType.Text, whereClause.Parameters);
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000FD83C File Offset: 0x000FC83C
		public SqlDataSourceQuery GetInsertQuery()
		{
			if (!this.CanAutoGenerateQueries())
			{
				return null;
			}
			List<Parameter> list = new List<Parameter>();
			StringBuilder stringBuilder = new StringBuilder("INSERT INTO ");
			stringBuilder.Append(this.GetTableName());
			List<SqlDataSourceColumnData> effectiveColumns = this.GetEffectiveColumns();
			StringBuilder stringBuilder2 = new StringBuilder();
			StringBuilder stringBuilder3 = new StringBuilder();
			bool flag = true;
			foreach (SqlDataSourceColumnData sqlDataSourceColumnData in effectiveColumns)
			{
				if (!sqlDataSourceColumnData.Column.Identity)
				{
					if (!flag)
					{
						stringBuilder2.Append(", ");
						stringBuilder3.Append(", ");
					}
					stringBuilder2.Append(sqlDataSourceColumnData.EscapedName);
					stringBuilder3.Append(sqlDataSourceColumnData.ParameterPlaceholder);
					DbProviderFactory dbProviderFactory = SqlDataSourceDesigner.GetDbProviderFactory(this.DesignerDataConnection.ProviderName);
					list.Add(SqlDataSourceDesigner.CreateParameter(dbProviderFactory, sqlDataSourceColumnData.WebParameterName, sqlDataSourceColumnData.Column.DataType));
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			stringBuilder.Append(" (");
			stringBuilder.Append(stringBuilder2.ToString());
			stringBuilder.Append(") VALUES (");
			stringBuilder.Append(stringBuilder3.ToString());
			stringBuilder.Append(")");
			return new SqlDataSourceQuery(stringBuilder.ToString(), SqlDataSourceCommandType.Text, list);
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x000FD99C File Offset: 0x000FC99C
		private List<SqlDataSourceColumnData> GetEffectiveColumns()
		{
			StringCollection stringCollection = new StringCollection();
			List<SqlDataSourceColumnData> list = new List<SqlDataSourceColumnData>();
			if (this.AsteriskField)
			{
				using (IEnumerator enumerator = this._designerDataTable.Columns.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DesignerDataColumn designerDataColumn = (DesignerDataColumn)obj;
						list.Add(new SqlDataSourceColumnData(this.DesignerDataConnection, designerDataColumn, stringCollection));
					}
					return list;
				}
			}
			foreach (DesignerDataColumn designerDataColumn2 in this._fields)
			{
				list.Add(new SqlDataSourceColumnData(this.DesignerDataConnection, designerDataColumn2, stringCollection));
			}
			return list;
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x000FDA70 File Offset: 0x000FCA70
		public SqlDataSourceQuery GetSelectQuery()
		{
			/*
An exception occurred when decompiling this method (06002CDC)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Web.UI.Design.WebControls.SqlDataSourceQuery System.Web.UI.Design.WebControls.SqlDataSourceTableQuery::GetSelectQuery()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 1052
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body, HashSet`1 ehs) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 959
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body, HashSet`1 ehs) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 897
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 280
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000FDCFC File Offset: 0x000FCCFC
		public string GetTableName()
		{
			return SqlDataSourceColumnData.EscapeObjectName(this.DesignerDataConnection, this.DesignerDataTable.Name);
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x000FDD14 File Offset: 0x000FCD14
		public SqlDataSourceQuery GetUpdateQuery(string oldValuesFormatString, bool includeOldValues)
		{
			/*
An exception occurred when decompiling this method (06002CDE)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Web.UI.Design.WebControls.SqlDataSourceQuery System.Web.UI.Design.WebControls.SqlDataSourceTableQuery::GetUpdateQuery(System.String,System.Boolean)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at System.Collections.Generic.List`1.set_Capacity(Int32 value)
   at System.Collections.Generic.List`1.AddWithResize(T item)
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.SplitToBasicBlocks(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 1748
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 260
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000FDE9C File Offset: 0x000FCE9C
		private SqlDataSourceQuery GetWhereClause(string oldValuesFormatString, bool includeOldValues)
		{
			List<SqlDataSourceColumnData> effectiveColumns = this.GetEffectiveColumns();
			List<Parameter> list = new List<Parameter>();
			if (effectiveColumns.Count == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" WHERE ");
			int num = 0;
			DbProviderFactory dbProviderFactory = SqlDataSourceDesigner.GetDbProviderFactory(this.DesignerDataConnection.ProviderName);
			foreach (SqlDataSourceColumnData sqlDataSourceColumnData in effectiveColumns)
			{
				if (sqlDataSourceColumnData.Column.PrimaryKey)
				{
					if (num > 0)
					{
						stringBuilder.Append(" AND ");
					}
					num++;
					this.AppendWhereClauseParameter(stringBuilder, sqlDataSourceColumnData, oldValuesFormatString);
					list.Add(SqlDataSourceDesigner.CreateParameter(dbProviderFactory, sqlDataSourceColumnData.GetOldValueWebParameterName(oldValuesFormatString), sqlDataSourceColumnData.Column.DataType));
				}
			}
			if (num == 0)
			{
				return null;
			}
			if (includeOldValues)
			{
				foreach (SqlDataSourceColumnData sqlDataSourceColumnData2 in effectiveColumns)
				{
					if (!sqlDataSourceColumnData2.Column.PrimaryKey)
					{
						stringBuilder.Append(" AND ");
						num++;
						this.AppendWhereClauseParameter(stringBuilder, sqlDataSourceColumnData2, oldValuesFormatString);
						list.Add(SqlDataSourceDesigner.CreateParameter(dbProviderFactory, sqlDataSourceColumnData2.GetOldValueWebParameterName(oldValuesFormatString), sqlDataSourceColumnData2.Column.DataType));
					}
				}
			}
			return new SqlDataSourceQuery(stringBuilder.ToString(), SqlDataSourceCommandType.Text, list);
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x000FE008 File Offset: 0x000FD008
		public bool IsPrimaryKeySelected()
		{
			List<SqlDataSourceColumnData> effectiveColumns = this.GetEffectiveColumns();
			if (effectiveColumns.Count == 0)
			{
				return false;
			}
			int num = 0;
			foreach (object obj in this._designerDataTable.Columns)
			{
				DesignerDataColumn designerDataColumn = (DesignerDataColumn)obj;
				if (designerDataColumn.PrimaryKey)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return false;
			}
			int num2 = 0;
			foreach (SqlDataSourceColumnData sqlDataSourceColumnData in effectiveColumns)
			{
				if (sqlDataSourceColumnData.Column.PrimaryKey)
				{
					num2++;
				}
			}
			return num == num2;
		}

		// Token: 0x04001E9A RID: 7834
		private DesignerDataConnection _designerDataConnection;

		// Token: 0x04001E9B RID: 7835
		private DesignerDataTableBase _designerDataTable;

		// Token: 0x04001E9C RID: 7836
		private IList<SqlDataSourceFilterClause> _filterClauses = new List<SqlDataSourceFilterClause>();

		// Token: 0x04001E9D RID: 7837
		private IList<SqlDataSourceOrderClause> _orderClauses = new List<SqlDataSourceOrderClause>();

		// Token: 0x04001E9E RID: 7838
		private bool _distinct;

		// Token: 0x04001E9F RID: 7839
		private bool _asteriskField;

		// Token: 0x04001EA0 RID: 7840
		private IList<DesignerDataColumn> _fields = new List<DesignerDataColumn>();
	}
}
