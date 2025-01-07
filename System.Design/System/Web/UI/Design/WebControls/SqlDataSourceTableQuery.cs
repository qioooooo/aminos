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
	internal sealed class SqlDataSourceTableQuery
	{
		public SqlDataSourceTableQuery(DesignerDataConnection designerDataConnection, DesignerDataTableBase designerDataTable)
		{
			this._designerDataConnection = designerDataConnection;
			this._designerDataTable = designerDataTable;
		}

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

		public DesignerDataConnection DesignerDataConnection
		{
			get
			{
				return this._designerDataConnection;
			}
		}

		public DesignerDataTableBase DesignerDataTable
		{
			get
			{
				return this._designerDataTable;
			}
		}

		public bool Distinct
		{
			get
			{
				return this._distinct;
			}
			set
			{
				this._distinct = value;
			}
		}

		public IList<DesignerDataColumn> Fields
		{
			get
			{
				return this._fields;
			}
		}

		public IList<SqlDataSourceFilterClause> FilterClauses
		{
			get
			{
				return this._filterClauses;
			}
		}

		public IList<SqlDataSourceOrderClause> OrderClauses
		{
			get
			{
				return this._orderClauses;
			}
		}

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

		private bool CanAutoGenerateQueries()
		{
			return !this.Distinct && (this.AsteriskField || this._fields.Count != 0);
		}

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

		public SqlDataSourceQuery GetSelectQuery()
		{
			if (!this._asteriskField && this._fields.Count == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(2048);
			stringBuilder.Append("SELECT");
			if (this._distinct)
			{
				stringBuilder.Append(" DISTINCT");
			}
			if (this._asteriskField)
			{
				stringBuilder.Append(" ");
				SqlDataSourceColumnData sqlDataSourceColumnData = new SqlDataSourceColumnData(this.DesignerDataConnection, null);
				stringBuilder.Append(sqlDataSourceColumnData.SelectName);
			}
			if (this._fields.Count > 0)
			{
				stringBuilder.Append(" ");
				bool flag = true;
				List<SqlDataSourceColumnData> effectiveColumns = this.GetEffectiveColumns();
				foreach (SqlDataSourceColumnData sqlDataSourceColumnData2 in effectiveColumns)
				{
					if (!flag)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(sqlDataSourceColumnData2.SelectName);
					flag = false;
				}
			}
			stringBuilder.Append(" FROM");
			stringBuilder.Append(" " + this.GetTableName());
			List<Parameter> list = new List<Parameter>();
			if (this._filterClauses.Count > 0)
			{
				stringBuilder.Append(" WHERE ");
				if (this._filterClauses.Count > 1)
				{
					stringBuilder.Append("(");
				}
				bool flag2 = true;
				foreach (SqlDataSourceFilterClause sqlDataSourceFilterClause in this._filterClauses)
				{
					if (!flag2)
					{
						stringBuilder.Append(" AND ");
					}
					stringBuilder.Append("(" + sqlDataSourceFilterClause.ToString() + ")");
					flag2 = false;
					if (sqlDataSourceFilterClause.Parameter != null)
					{
						list.Add(sqlDataSourceFilterClause.Parameter);
					}
				}
				if (this._filterClauses.Count > 1)
				{
					stringBuilder.Append(")");
				}
			}
			if (this._orderClauses.Count > 0)
			{
				stringBuilder.Append(" ORDER BY ");
				bool flag3 = true;
				foreach (SqlDataSourceOrderClause sqlDataSourceOrderClause in this._orderClauses)
				{
					if (!flag3)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(sqlDataSourceOrderClause.ToString());
					flag3 = false;
				}
			}
			string text = stringBuilder.ToString();
			return new SqlDataSourceQuery(text, SqlDataSourceCommandType.Text, list.ToArray());
		}

		public string GetTableName()
		{
			return SqlDataSourceColumnData.EscapeObjectName(this.DesignerDataConnection, this.DesignerDataTable.Name);
		}

		public SqlDataSourceQuery GetUpdateQuery(string oldValuesFormatString, bool includeOldValues)
		{
			if (!this.CanAutoGenerateQueries())
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder("UPDATE ");
			stringBuilder.Append(this.GetTableName());
			stringBuilder.Append(" SET ");
			List<SqlDataSourceColumnData> effectiveColumns = this.GetEffectiveColumns();
			List<Parameter> list = new List<Parameter>();
			bool flag = true;
			foreach (SqlDataSourceColumnData sqlDataSourceColumnData in effectiveColumns)
			{
				if (!sqlDataSourceColumnData.Column.PrimaryKey)
				{
					if (!flag)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(sqlDataSourceColumnData.EscapedName);
					stringBuilder.Append(" = ");
					stringBuilder.Append(sqlDataSourceColumnData.ParameterPlaceholder);
					flag = false;
					DbProviderFactory dbProviderFactory = SqlDataSourceDesigner.GetDbProviderFactory(this.DesignerDataConnection.ProviderName);
					list.Add(SqlDataSourceDesigner.CreateParameter(dbProviderFactory, sqlDataSourceColumnData.WebParameterName, sqlDataSourceColumnData.Column.DataType));
				}
			}
			if (flag)
			{
				return null;
			}
			SqlDataSourceQuery whereClause = this.GetWhereClause(oldValuesFormatString, includeOldValues);
			if (whereClause == null)
			{
				return null;
			}
			stringBuilder.Append(whereClause.Command);
			foreach (object obj in whereClause.Parameters)
			{
				Parameter parameter = (Parameter)obj;
				list.Add(parameter);
			}
			return new SqlDataSourceQuery(stringBuilder.ToString(), SqlDataSourceCommandType.Text, list);
		}

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

		private DesignerDataConnection _designerDataConnection;

		private DesignerDataTableBase _designerDataTable;

		private IList<SqlDataSourceFilterClause> _filterClauses = new List<SqlDataSourceFilterClause>();

		private IList<SqlDataSourceOrderClause> _orderClauses = new List<SqlDataSourceOrderClause>();

		private bool _distinct;

		private bool _asteriskField;

		private IList<DesignerDataColumn> _fields = new List<DesignerDataColumn>();
	}
}
