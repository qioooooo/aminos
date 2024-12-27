using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Data.Common
{
	// Token: 0x02000127 RID: 295
	public abstract class DbCommandBuilder : Component
	{
		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06001318 RID: 4888 RVA: 0x002209E8 File Offset: 0x0021FDE8
		// (set) Token: 0x06001319 RID: 4889 RVA: 0x002209FC File Offset: 0x0021FDFC
		[DefaultValue(ConflictOption.CompareAllSearchableValues)]
		[ResCategory("DataCategory_Update")]
		[ResDescription("DbCommandBuilder_ConflictOption")]
		public virtual ConflictOption ConflictOption
		{
			get
			{
				return this._conflictDetection;
			}
			set
			{
				switch (value)
				{
				case ConflictOption.CompareAllSearchableValues:
				case ConflictOption.CompareRowVersion:
				case ConflictOption.OverwriteChanges:
					this._conflictDetection = value;
					return;
				default:
					throw ADP.InvalidConflictOptions(value);
				}
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x0600131A RID: 4890 RVA: 0x00220A30 File Offset: 0x0021FE30
		// (set) Token: 0x0600131B RID: 4891 RVA: 0x00220A44 File Offset: 0x0021FE44
		[ResCategory("DataCategory_Schema")]
		[DefaultValue(CatalogLocation.Start)]
		[ResDescription("DbCommandBuilder_CatalogLocation")]
		public virtual CatalogLocation CatalogLocation
		{
			get
			{
				return this._catalogLocation;
			}
			set
			{
				if (this._dbSchemaTable != null)
				{
					throw ADP.NoQuoteChange();
				}
				switch (value)
				{
				case CatalogLocation.Start:
				case CatalogLocation.End:
					this._catalogLocation = value;
					return;
				default:
					throw ADP.InvalidCatalogLocation(value);
				}
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x0600131C RID: 4892 RVA: 0x00220A84 File Offset: 0x0021FE84
		// (set) Token: 0x0600131D RID: 4893 RVA: 0x00220AAC File Offset: 0x0021FEAC
		[DefaultValue(".")]
		[ResCategory("DataCategory_Schema")]
		[ResDescription("DbCommandBuilder_CatalogSeparator")]
		public virtual string CatalogSeparator
		{
			get
			{
				string catalogSeparator = this._catalogSeparator;
				if (catalogSeparator == null || 0 >= catalogSeparator.Length)
				{
					return ".";
				}
				return catalogSeparator;
			}
			set
			{
				if (this._dbSchemaTable != null)
				{
					throw ADP.NoQuoteChange();
				}
				this._catalogSeparator = value;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x0600131E RID: 4894 RVA: 0x00220AD0 File Offset: 0x0021FED0
		// (set) Token: 0x0600131F RID: 4895 RVA: 0x00220AE4 File Offset: 0x0021FEE4
		[Browsable(false)]
		[ResDescription("DbCommandBuilder_DataAdapter")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DbDataAdapter DataAdapter
		{
			get
			{
				return this._dataAdapter;
			}
			set
			{
				if (this._dataAdapter != value)
				{
					this.RefreshSchema();
					if (this._dataAdapter != null)
					{
						this.SetRowUpdatingHandler(this._dataAdapter);
						this._dataAdapter = null;
					}
					if (value != null)
					{
						this.SetRowUpdatingHandler(value);
						this._dataAdapter = value;
					}
				}
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06001320 RID: 4896 RVA: 0x00220B2C File Offset: 0x0021FF2C
		internal int ParameterNameMaxLength
		{
			get
			{
				return this._parameterNameMaxLength;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06001321 RID: 4897 RVA: 0x00220B40 File Offset: 0x0021FF40
		internal string ParameterNamePattern
		{
			get
			{
				return this._parameterNamePattern;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06001322 RID: 4898 RVA: 0x00220B54 File Offset: 0x0021FF54
		private string QuotedBaseTableName
		{
			get
			{
				return this._quotedBaseTableName;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06001323 RID: 4899 RVA: 0x00220B68 File Offset: 0x0021FF68
		// (set) Token: 0x06001324 RID: 4900 RVA: 0x00220B88 File Offset: 0x0021FF88
		[ResDescription("DbCommandBuilder_QuotePrefix")]
		[DefaultValue("")]
		[ResCategory("DataCategory_Schema")]
		public virtual string QuotePrefix
		{
			get
			{
				string quotePrefix = this._quotePrefix;
				if (quotePrefix == null)
				{
					return ADP.StrEmpty;
				}
				return quotePrefix;
			}
			set
			{
				if (this._dbSchemaTable != null)
				{
					throw ADP.NoQuoteChange();
				}
				this._quotePrefix = value;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06001325 RID: 4901 RVA: 0x00220BAC File Offset: 0x0021FFAC
		// (set) Token: 0x06001326 RID: 4902 RVA: 0x00220BCC File Offset: 0x0021FFCC
		[ResCategory("DataCategory_Schema")]
		[DefaultValue("")]
		[ResDescription("DbCommandBuilder_QuoteSuffix")]
		public virtual string QuoteSuffix
		{
			get
			{
				string quoteSuffix = this._quoteSuffix;
				if (quoteSuffix == null)
				{
					return ADP.StrEmpty;
				}
				return quoteSuffix;
			}
			set
			{
				if (this._dbSchemaTable != null)
				{
					throw ADP.NoQuoteChange();
				}
				this._quoteSuffix = value;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06001327 RID: 4903 RVA: 0x00220BF0 File Offset: 0x0021FFF0
		// (set) Token: 0x06001328 RID: 4904 RVA: 0x00220C18 File Offset: 0x00220018
		[DefaultValue(".")]
		[ResCategory("DataCategory_Schema")]
		[ResDescription("DbCommandBuilder_SchemaSeparator")]
		public virtual string SchemaSeparator
		{
			get
			{
				string schemaSeparator = this._schemaSeparator;
				if (schemaSeparator == null || 0 >= schemaSeparator.Length)
				{
					return ".";
				}
				return schemaSeparator;
			}
			set
			{
				if (this._dbSchemaTable != null)
				{
					throw ADP.NoQuoteChange();
				}
				this._schemaSeparator = value;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06001329 RID: 4905 RVA: 0x00220C3C File Offset: 0x0022003C
		// (set) Token: 0x0600132A RID: 4906 RVA: 0x00220C50 File Offset: 0x00220050
		[ResDescription("DbCommandBuilder_SetAllValues")]
		[DefaultValue(false)]
		[ResCategory("DataCategory_Schema")]
		public bool SetAllValues
		{
			get
			{
				return this._setAllValues;
			}
			set
			{
				this._setAllValues = value;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x0600132B RID: 4907 RVA: 0x00220C64 File Offset: 0x00220064
		// (set) Token: 0x0600132C RID: 4908 RVA: 0x00220C78 File Offset: 0x00220078
		private DbCommand InsertCommand
		{
			get
			{
				return this._insertCommand;
			}
			set
			{
				this._insertCommand = value;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x0600132D RID: 4909 RVA: 0x00220C8C File Offset: 0x0022008C
		// (set) Token: 0x0600132E RID: 4910 RVA: 0x00220CA0 File Offset: 0x002200A0
		private DbCommand UpdateCommand
		{
			get
			{
				return this._updateCommand;
			}
			set
			{
				this._updateCommand = value;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x0600132F RID: 4911 RVA: 0x00220CB4 File Offset: 0x002200B4
		// (set) Token: 0x06001330 RID: 4912 RVA: 0x00220CC8 File Offset: 0x002200C8
		private DbCommand DeleteCommand
		{
			get
			{
				return this._deleteCommand;
			}
			set
			{
				this._deleteCommand = value;
			}
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x00220CDC File Offset: 0x002200DC
		private void BuildCache(bool closeConnection, DataRow dataRow, bool useColumnsForParameterNames)
		{
			if (this._dbSchemaTable != null && (!useColumnsForParameterNames || this._parameterNames != null))
			{
				return;
			}
			DataTable dataTable = null;
			DbCommand selectCommand = this.GetSelectCommand();
			DbConnection connection = selectCommand.Connection;
			if (connection == null)
			{
				throw ADP.MissingSourceCommandConnection();
			}
			try
			{
				if ((ConnectionState.Open & connection.State) == ConnectionState.Closed)
				{
					connection.Open();
				}
				else
				{
					closeConnection = false;
				}
				if (useColumnsForParameterNames)
				{
					DataTable schema = connection.GetSchema(DbMetaDataCollectionNames.DataSourceInformation);
					if (schema.Rows.Count == 1)
					{
						this._parameterNamePattern = schema.Rows[0][DbMetaDataColumnNames.ParameterNamePattern] as string;
						this._parameterMarkerFormat = schema.Rows[0][DbMetaDataColumnNames.ParameterMarkerFormat] as string;
						object obj = schema.Rows[0][DbMetaDataColumnNames.ParameterNameMaxLength];
						this._parameterNameMaxLength = ((obj is int) ? ((int)obj) : 0);
						if (this._parameterNameMaxLength == 0 || this._parameterNamePattern == null || this._parameterMarkerFormat == null)
						{
							useColumnsForParameterNames = false;
						}
					}
					else
					{
						useColumnsForParameterNames = false;
					}
				}
				dataTable = this.GetSchemaTable(selectCommand);
			}
			finally
			{
				if (closeConnection)
				{
					connection.Close();
				}
			}
			if (dataTable == null)
			{
				throw ADP.DynamicSQLNoTableInfo();
			}
			this.BuildInformation(dataTable);
			this._dbSchemaTable = dataTable;
			DbSchemaRow[] dbSchemaRows = this._dbSchemaRows;
			string[] array = new string[dbSchemaRows.Length];
			for (int i = 0; i < dbSchemaRows.Length; i++)
			{
				if (dbSchemaRows[i] != null)
				{
					array[i] = dbSchemaRows[i].ColumnName;
				}
			}
			this._sourceColumnNames = array;
			if (useColumnsForParameterNames)
			{
				this._parameterNames = new DbCommandBuilder.ParameterNames(this, dbSchemaRows);
			}
			ADP.BuildSchemaTableInfoTableNames(array);
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x00220E80 File Offset: 0x00220280
		protected virtual DataTable GetSchemaTable(DbCommand sourceCommand)
		{
			DataTable schemaTable;
			using (IDataReader dataReader = sourceCommand.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo))
			{
				schemaTable = dataReader.GetSchemaTable();
			}
			return schemaTable;
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x00220EC8 File Offset: 0x002202C8
		private void BuildInformation(DataTable schemaTable)
		{
			DbSchemaRow[] sortedSchemaRows = DbSchemaRow.GetSortedSchemaRows(schemaTable, false);
			if (sortedSchemaRows == null || sortedSchemaRows.Length == 0)
			{
				throw ADP.DynamicSQLNoTableInfo();
			}
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = null;
			for (int i = 0; i < sortedSchemaRows.Length; i++)
			{
				DbSchemaRow dbSchemaRow = sortedSchemaRows[i];
				string baseTableName = dbSchemaRow.BaseTableName;
				if (baseTableName == null || baseTableName.Length == 0)
				{
					sortedSchemaRows[i] = null;
				}
				else
				{
					string text5 = dbSchemaRow.BaseServerName;
					string text6 = dbSchemaRow.BaseCatalogName;
					string text7 = dbSchemaRow.BaseSchemaName;
					if (text5 == null)
					{
						text5 = "";
					}
					if (text6 == null)
					{
						text6 = "";
					}
					if (text7 == null)
					{
						text7 = "";
					}
					if (text4 == null)
					{
						text = text5;
						text2 = text6;
						text3 = text7;
						text4 = baseTableName;
					}
					else if (ADP.SrcCompare(text4, baseTableName) != 0 || ADP.SrcCompare(text3, text7) != 0 || ADP.SrcCompare(text2, text6) != 0 || ADP.SrcCompare(text, text5) != 0)
					{
						throw ADP.DynamicSQLJoinUnsupported();
					}
				}
			}
			if (text.Length == 0)
			{
				text = null;
			}
			if (text2.Length == 0)
			{
				text = null;
				text2 = null;
			}
			if (text3.Length == 0)
			{
				text = null;
				text2 = null;
				text3 = null;
			}
			if (text4 == null || text4.Length == 0)
			{
				throw ADP.DynamicSQLNoTableInfo();
			}
			CatalogLocation catalogLocation = this.CatalogLocation;
			string catalogSeparator = this.CatalogSeparator;
			string schemaSeparator = this.SchemaSeparator;
			string quotePrefix = this.QuotePrefix;
			string quoteSuffix = this.QuoteSuffix;
			if (!ADP.IsEmpty(quotePrefix) && -1 != text4.IndexOf(quotePrefix, StringComparison.Ordinal))
			{
				throw ADP.DynamicSQLNestedQuote(text4, quotePrefix);
			}
			if (!ADP.IsEmpty(quoteSuffix) && -1 != text4.IndexOf(quoteSuffix, StringComparison.Ordinal))
			{
				throw ADP.DynamicSQLNestedQuote(text4, quoteSuffix);
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (CatalogLocation.Start == catalogLocation)
			{
				if (text != null)
				{
					stringBuilder.Append(ADP.BuildQuotedString(quotePrefix, quoteSuffix, text));
					stringBuilder.Append(catalogSeparator);
				}
				if (text2 != null)
				{
					stringBuilder.Append(ADP.BuildQuotedString(quotePrefix, quoteSuffix, text2));
					stringBuilder.Append(catalogSeparator);
				}
			}
			if (text3 != null)
			{
				stringBuilder.Append(ADP.BuildQuotedString(quotePrefix, quoteSuffix, text3));
				stringBuilder.Append(schemaSeparator);
			}
			stringBuilder.Append(ADP.BuildQuotedString(quotePrefix, quoteSuffix, text4));
			if (CatalogLocation.End == catalogLocation)
			{
				if (text != null)
				{
					stringBuilder.Append(catalogSeparator);
					stringBuilder.Append(ADP.BuildQuotedString(quotePrefix, quoteSuffix, text));
				}
				if (text2 != null)
				{
					stringBuilder.Append(catalogSeparator);
					stringBuilder.Append(ADP.BuildQuotedString(quotePrefix, quoteSuffix, text2));
				}
			}
			this._quotedBaseTableName = stringBuilder.ToString();
			this._hasPartialPrimaryKey = false;
			foreach (DbSchemaRow dbSchemaRow2 in sortedSchemaRows)
			{
				if (dbSchemaRow2 != null && (dbSchemaRow2.IsKey || dbSchemaRow2.IsUnique) && !dbSchemaRow2.IsLong && !dbSchemaRow2.IsRowVersion && dbSchemaRow2.IsHidden)
				{
					this._hasPartialPrimaryKey = true;
					break;
				}
			}
			this._dbSchemaRows = sortedSchemaRows;
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x00221184 File Offset: 0x00220584
		private DbCommand BuildDeleteCommand(DataTableMapping mappings, DataRow dataRow)
		{
			DbCommand dbCommand = this.InitializeCommand(this.DeleteCommand);
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			stringBuilder.Append("DELETE FROM ");
			stringBuilder.Append(this.QuotedBaseTableName);
			num = this.BuildWhereClause(mappings, dataRow, stringBuilder, dbCommand, num, false);
			dbCommand.CommandText = stringBuilder.ToString();
			DbCommandBuilder.RemoveExtraParameters(dbCommand, num);
			this.DeleteCommand = dbCommand;
			return dbCommand;
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x002211E8 File Offset: 0x002205E8
		private DbCommand BuildInsertCommand(DataTableMapping mappings, DataRow dataRow)
		{
			DbCommand dbCommand = this.InitializeCommand(this.InsertCommand);
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			string text = " (";
			stringBuilder.Append("INSERT INTO ");
			stringBuilder.Append(this.QuotedBaseTableName);
			DbSchemaRow[] dbSchemaRows = this._dbSchemaRows;
			string[] array = new string[dbSchemaRows.Length];
			for (int i = 0; i < dbSchemaRows.Length; i++)
			{
				DbSchemaRow dbSchemaRow = dbSchemaRows[i];
				if (dbSchemaRow != null && dbSchemaRow.BaseColumnName.Length != 0 && this.IncludeInInsertValues(dbSchemaRow))
				{
					object obj = null;
					string text2 = this._sourceColumnNames[i];
					if (mappings != null && dataRow != null)
					{
						DataColumn dataColumn = this.GetDataColumn(text2, mappings, dataRow);
						if (dataColumn == null || (dbSchemaRow.IsReadOnly && dataColumn.ReadOnly))
						{
							goto IL_011A;
						}
						obj = this.GetColumnValue(dataRow, dataColumn, DataRowVersion.Current);
						if (!dbSchemaRow.AllowDBNull && (obj == null || Convert.IsDBNull(obj)))
						{
							goto IL_011A;
						}
					}
					stringBuilder.Append(text);
					text = ", ";
					stringBuilder.Append(this.QuotedColumn(dbSchemaRow.BaseColumnName));
					array[num] = this.CreateParameterForValue(dbCommand, this.GetBaseParameterName(i), text2, DataRowVersion.Current, num, obj, dbSchemaRow, StatementType.Insert, false);
					num++;
				}
				IL_011A:;
			}
			if (num == 0)
			{
				stringBuilder.Append(" DEFAULT VALUES");
			}
			else
			{
				stringBuilder.Append(")");
				stringBuilder.Append(" VALUES ");
				stringBuilder.Append("(");
				stringBuilder.Append(array[0]);
				for (int j = 1; j < num; j++)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(array[j]);
				}
				stringBuilder.Append(")");
			}
			dbCommand.CommandText = stringBuilder.ToString();
			DbCommandBuilder.RemoveExtraParameters(dbCommand, num);
			this.InsertCommand = dbCommand;
			return dbCommand;
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x002213B0 File Offset: 0x002207B0
		private DbCommand BuildUpdateCommand(DataTableMapping mappings, DataRow dataRow)
		{
			DbCommand dbCommand = this.InitializeCommand(this.UpdateCommand);
			StringBuilder stringBuilder = new StringBuilder();
			string text = " SET ";
			int num = 0;
			stringBuilder.Append("UPDATE ");
			stringBuilder.Append(this.QuotedBaseTableName);
			DbSchemaRow[] dbSchemaRows = this._dbSchemaRows;
			for (int i = 0; i < dbSchemaRows.Length; i++)
			{
				DbSchemaRow dbSchemaRow = dbSchemaRows[i];
				if (dbSchemaRow != null && dbSchemaRow.BaseColumnName.Length != 0 && this.IncludeInUpdateSet(dbSchemaRow))
				{
					object obj = null;
					string text2 = this._sourceColumnNames[i];
					if (mappings != null && dataRow != null)
					{
						DataColumn dataColumn = this.GetDataColumn(text2, mappings, dataRow);
						if (dataColumn == null || (dbSchemaRow.IsReadOnly && dataColumn.ReadOnly))
						{
							goto IL_0139;
						}
						obj = this.GetColumnValue(dataRow, dataColumn, DataRowVersion.Current);
						if (!this.SetAllValues)
						{
							object columnValue = this.GetColumnValue(dataRow, dataColumn, DataRowVersion.Original);
							if (columnValue == obj || (columnValue != null && columnValue.Equals(obj)))
							{
								goto IL_0139;
							}
						}
					}
					stringBuilder.Append(text);
					text = ", ";
					stringBuilder.Append(this.QuotedColumn(dbSchemaRow.BaseColumnName));
					stringBuilder.Append(" = ");
					stringBuilder.Append(this.CreateParameterForValue(dbCommand, this.GetBaseParameterName(i), text2, DataRowVersion.Current, num, obj, dbSchemaRow, StatementType.Update, false));
					num++;
				}
				IL_0139:;
			}
			bool flag = 0 == num;
			num = this.BuildWhereClause(mappings, dataRow, stringBuilder, dbCommand, num, true);
			dbCommand.CommandText = stringBuilder.ToString();
			DbCommandBuilder.RemoveExtraParameters(dbCommand, num);
			this.UpdateCommand = dbCommand;
			if (!flag)
			{
				return dbCommand;
			}
			return null;
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x00221540 File Offset: 0x00220940
		private int BuildWhereClause(DataTableMapping mappings, DataRow dataRow, StringBuilder builder, DbCommand command, int parameterCount, bool isUpdate)
		{
			string text = string.Empty;
			int num = 0;
			builder.Append(" WHERE ");
			builder.Append("(");
			DbSchemaRow[] dbSchemaRows = this._dbSchemaRows;
			for (int i = 0; i < dbSchemaRows.Length; i++)
			{
				DbSchemaRow dbSchemaRow = dbSchemaRows[i];
				if (dbSchemaRow != null && dbSchemaRow.BaseColumnName.Length != 0 && this.IncludeInWhereClause(dbSchemaRow, isUpdate))
				{
					builder.Append(text);
					text = " AND ";
					object obj = null;
					string text2 = this._sourceColumnNames[i];
					string text3 = this.QuotedColumn(dbSchemaRow.BaseColumnName);
					if (mappings != null && dataRow != null)
					{
						obj = this.GetColumnValue(dataRow, text2, mappings, DataRowVersion.Original);
					}
					if (!dbSchemaRow.AllowDBNull)
					{
						builder.Append("(");
						builder.Append(text3);
						builder.Append(" = ");
						builder.Append(this.CreateParameterForValue(command, this.GetOriginalParameterName(i), text2, DataRowVersion.Original, parameterCount, obj, dbSchemaRow, isUpdate ? StatementType.Update : StatementType.Delete, true));
						parameterCount++;
						builder.Append(")");
					}
					else
					{
						builder.Append("(");
						builder.Append("(");
						builder.Append(this.CreateParameterForNullTest(command, this.GetNullParameterName(i), text2, DataRowVersion.Original, parameterCount, obj, dbSchemaRow, isUpdate ? StatementType.Update : StatementType.Delete, true));
						parameterCount++;
						builder.Append(" = 1");
						builder.Append(" AND ");
						builder.Append(text3);
						builder.Append(" IS NULL");
						builder.Append(")");
						builder.Append(" OR ");
						builder.Append("(");
						builder.Append(text3);
						builder.Append(" = ");
						builder.Append(this.CreateParameterForValue(command, this.GetOriginalParameterName(i), text2, DataRowVersion.Original, parameterCount, obj, dbSchemaRow, isUpdate ? StatementType.Update : StatementType.Delete, true));
						parameterCount++;
						builder.Append(")");
						builder.Append(")");
					}
					if (this.IncrementWhereCount(dbSchemaRow))
					{
						num++;
					}
				}
			}
			builder.Append(")");
			if (num != 0)
			{
				return parameterCount;
			}
			if (isUpdate)
			{
				if (ConflictOption.CompareRowVersion == this.ConflictOption)
				{
					throw ADP.DynamicSQLNoKeyInfoRowVersionUpdate();
				}
				throw ADP.DynamicSQLNoKeyInfoUpdate();
			}
			else
			{
				if (ConflictOption.CompareRowVersion == this.ConflictOption)
				{
					throw ADP.DynamicSQLNoKeyInfoRowVersionDelete();
				}
				throw ADP.DynamicSQLNoKeyInfoDelete();
			}
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x002217A0 File Offset: 0x00220BA0
		private string CreateParameterForNullTest(DbCommand command, string parameterName, string sourceColumn, DataRowVersion version, int parameterCount, object value, DbSchemaRow row, StatementType statementType, bool whereClause)
		{
			DbParameter nextParameter = DbCommandBuilder.GetNextParameter(command, parameterCount);
			if (parameterName == null)
			{
				nextParameter.ParameterName = this.GetParameterName(1 + parameterCount);
			}
			else
			{
				nextParameter.ParameterName = parameterName;
			}
			nextParameter.Direction = ParameterDirection.Input;
			nextParameter.SourceColumn = sourceColumn;
			nextParameter.SourceVersion = version;
			nextParameter.SourceColumnNullMapping = true;
			nextParameter.Value = value;
			nextParameter.Size = 0;
			this.ApplyParameterInfo(nextParameter, row.DataRow, statementType, whereClause);
			nextParameter.DbType = DbType.Int32;
			nextParameter.Value = (ADP.IsNull(value) ? DbDataAdapter.ParameterValueNullValue : DbDataAdapter.ParameterValueNonNullValue);
			if (!command.Parameters.Contains(nextParameter))
			{
				command.Parameters.Add(nextParameter);
			}
			if (parameterName == null)
			{
				return this.GetParameterPlaceholder(1 + parameterCount);
			}
			return string.Format(CultureInfo.InvariantCulture, this._parameterMarkerFormat, new object[] { parameterName });
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00221878 File Offset: 0x00220C78
		private string CreateParameterForValue(DbCommand command, string parameterName, string sourceColumn, DataRowVersion version, int parameterCount, object value, DbSchemaRow row, StatementType statementType, bool whereClause)
		{
			DbParameter nextParameter = DbCommandBuilder.GetNextParameter(command, parameterCount);
			if (parameterName == null)
			{
				nextParameter.ParameterName = this.GetParameterName(1 + parameterCount);
			}
			else
			{
				nextParameter.ParameterName = parameterName;
			}
			nextParameter.Direction = ParameterDirection.Input;
			nextParameter.SourceColumn = sourceColumn;
			nextParameter.SourceVersion = version;
			nextParameter.SourceColumnNullMapping = false;
			nextParameter.Value = value;
			nextParameter.Size = 0;
			this.ApplyParameterInfo(nextParameter, row.DataRow, statementType, whereClause);
			if (!command.Parameters.Contains(nextParameter))
			{
				command.Parameters.Add(nextParameter);
			}
			if (parameterName == null)
			{
				return this.GetParameterPlaceholder(1 + parameterCount);
			}
			return string.Format(CultureInfo.InvariantCulture, this._parameterMarkerFormat, new object[] { parameterName });
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00221930 File Offset: 0x00220D30
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.DataAdapter = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x00221950 File Offset: 0x00220D50
		private DataTableMapping GetTableMapping(DataRow dataRow)
		{
			DataTableMapping dataTableMapping = null;
			if (dataRow != null)
			{
				DataTable table = dataRow.Table;
				if (table != null)
				{
					DbDataAdapter dataAdapter = this.DataAdapter;
					if (dataAdapter != null)
					{
						dataTableMapping = dataAdapter.GetTableMapping(table);
					}
					else
					{
						string tableName = table.TableName;
						dataTableMapping = new DataTableMapping(tableName, tableName);
					}
				}
			}
			return dataTableMapping;
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00221990 File Offset: 0x00220D90
		private string GetBaseParameterName(int index)
		{
			if (this._parameterNames != null)
			{
				return this._parameterNames.GetBaseParameterName(index);
			}
			return null;
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x002219B4 File Offset: 0x00220DB4
		private string GetOriginalParameterName(int index)
		{
			if (this._parameterNames != null)
			{
				return this._parameterNames.GetOriginalParameterName(index);
			}
			return null;
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x002219D8 File Offset: 0x00220DD8
		private string GetNullParameterName(int index)
		{
			if (this._parameterNames != null)
			{
				return this._parameterNames.GetNullParameterName(index);
			}
			return null;
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x002219FC File Offset: 0x00220DFC
		private DbCommand GetSelectCommand()
		{
			DbCommand dbCommand = null;
			DbDataAdapter dataAdapter = this.DataAdapter;
			if (dataAdapter != null)
			{
				if (this._missingMappingAction == (MissingMappingAction)0)
				{
					this._missingMappingAction = dataAdapter.MissingMappingAction;
				}
				dbCommand = dataAdapter.SelectCommand;
			}
			if (dbCommand == null)
			{
				throw ADP.MissingSourceCommand();
			}
			return dbCommand;
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x00221A3C File Offset: 0x00220E3C
		public DbCommand GetInsertCommand()
		{
			return this.GetInsertCommand(null, false);
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x00221A54 File Offset: 0x00220E54
		public DbCommand GetInsertCommand(bool useColumnsForParameterNames)
		{
			return this.GetInsertCommand(null, useColumnsForParameterNames);
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x00221A6C File Offset: 0x00220E6C
		internal DbCommand GetInsertCommand(DataRow dataRow, bool useColumnsForParameterNames)
		{
			this.BuildCache(true, dataRow, useColumnsForParameterNames);
			this.BuildInsertCommand(this.GetTableMapping(dataRow), dataRow);
			return this.InsertCommand;
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x00221A98 File Offset: 0x00220E98
		public DbCommand GetUpdateCommand()
		{
			return this.GetUpdateCommand(null, false);
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x00221AB0 File Offset: 0x00220EB0
		public DbCommand GetUpdateCommand(bool useColumnsForParameterNames)
		{
			return this.GetUpdateCommand(null, useColumnsForParameterNames);
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x00221AC8 File Offset: 0x00220EC8
		internal DbCommand GetUpdateCommand(DataRow dataRow, bool useColumnsForParameterNames)
		{
			this.BuildCache(true, dataRow, useColumnsForParameterNames);
			this.BuildUpdateCommand(this.GetTableMapping(dataRow), dataRow);
			return this.UpdateCommand;
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x00221AF4 File Offset: 0x00220EF4
		public DbCommand GetDeleteCommand()
		{
			return this.GetDeleteCommand(null, false);
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x00221B0C File Offset: 0x00220F0C
		public DbCommand GetDeleteCommand(bool useColumnsForParameterNames)
		{
			return this.GetDeleteCommand(null, useColumnsForParameterNames);
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x00221B24 File Offset: 0x00220F24
		internal DbCommand GetDeleteCommand(DataRow dataRow, bool useColumnsForParameterNames)
		{
			this.BuildCache(true, dataRow, useColumnsForParameterNames);
			this.BuildDeleteCommand(this.GetTableMapping(dataRow), dataRow);
			return this.DeleteCommand;
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x00221B50 File Offset: 0x00220F50
		private object GetColumnValue(DataRow row, string columnName, DataTableMapping mappings, DataRowVersion version)
		{
			return this.GetColumnValue(row, this.GetDataColumn(columnName, mappings, row), version);
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00221B70 File Offset: 0x00220F70
		private object GetColumnValue(DataRow row, DataColumn column, DataRowVersion version)
		{
			object obj = null;
			if (column != null)
			{
				obj = row[column, version];
			}
			return obj;
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00221B8C File Offset: 0x00220F8C
		private DataColumn GetDataColumn(string columnName, DataTableMapping tablemapping, DataRow row)
		{
			DataColumn dataColumn = null;
			if (!ADP.IsEmpty(columnName))
			{
				dataColumn = tablemapping.GetDataColumn(columnName, null, row.Table, this._missingMappingAction, MissingSchemaAction.Error);
			}
			return dataColumn;
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x00221BBC File Offset: 0x00220FBC
		private static DbParameter GetNextParameter(DbCommand command, int pcount)
		{
			DbParameter dbParameter;
			if (pcount < command.Parameters.Count)
			{
				dbParameter = command.Parameters[pcount];
			}
			else
			{
				dbParameter = command.CreateParameter();
			}
			return dbParameter;
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x00221BF0 File Offset: 0x00220FF0
		private bool IncludeInInsertValues(DbSchemaRow row)
		{
			return !row.IsAutoIncrement && !row.IsHidden && !row.IsExpression && !row.IsRowVersion;
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00221C20 File Offset: 0x00221020
		private bool IncludeInUpdateSet(DbSchemaRow row)
		{
			return !row.IsAutoIncrement && !row.IsRowVersion && !row.IsHidden;
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x00221C48 File Offset: 0x00221048
		private bool IncludeInWhereClause(DbSchemaRow row, bool isUpdate)
		{
			bool flag = this.IncrementWhereCount(row);
			if (!flag || !row.IsHidden)
			{
				if (!flag && ConflictOption.CompareAllSearchableValues == this.ConflictOption)
				{
					flag = !row.IsLong && !row.IsRowVersion && !row.IsHidden;
				}
				return flag;
			}
			if (ConflictOption.CompareRowVersion == this.ConflictOption)
			{
				throw ADP.DynamicSQLNoKeyInfoRowVersionUpdate();
			}
			throw ADP.DynamicSQLNoKeyInfoUpdate();
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x00221CA8 File Offset: 0x002210A8
		private bool IncrementWhereCount(DbSchemaRow row)
		{
			ConflictOption conflictOption = this.ConflictOption;
			switch (conflictOption)
			{
			case ConflictOption.CompareAllSearchableValues:
			case ConflictOption.OverwriteChanges:
				return (row.IsKey || row.IsUnique) && !row.IsLong && !row.IsRowVersion;
			case ConflictOption.CompareRowVersion:
				return (((row.IsKey || row.IsUnique) && !this._hasPartialPrimaryKey) || row.IsRowVersion) && !row.IsLong;
			default:
				throw ADP.InvalidConflictOptions(conflictOption);
			}
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x00221D2C File Offset: 0x0022112C
		protected virtual DbCommand InitializeCommand(DbCommand command)
		{
			if (command == null)
			{
				DbCommand selectCommand = this.GetSelectCommand();
				command = selectCommand.Connection.CreateCommand();
				command.CommandTimeout = selectCommand.CommandTimeout;
				command.Transaction = selectCommand.Transaction;
			}
			command.CommandType = CommandType.Text;
			command.UpdatedRowSource = UpdateRowSource.None;
			return command;
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x00221D78 File Offset: 0x00221178
		private string QuotedColumn(string column)
		{
			return ADP.BuildQuotedString(this.QuotePrefix, this.QuoteSuffix, column);
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x00221D98 File Offset: 0x00221198
		public virtual string QuoteIdentifier(string unquotedIdentifier)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00221DAC File Offset: 0x002211AC
		public virtual void RefreshSchema()
		{
			this._dbSchemaTable = null;
			this._dbSchemaRows = null;
			this._sourceColumnNames = null;
			this._quotedBaseTableName = null;
			DbDataAdapter dataAdapter = this.DataAdapter;
			if (dataAdapter != null)
			{
				if (this.InsertCommand == dataAdapter.InsertCommand)
				{
					dataAdapter.InsertCommand = null;
				}
				if (this.UpdateCommand == dataAdapter.UpdateCommand)
				{
					dataAdapter.UpdateCommand = null;
				}
				if (this.DeleteCommand == dataAdapter.DeleteCommand)
				{
					dataAdapter.DeleteCommand = null;
				}
			}
			DbCommand dbCommand;
			if ((dbCommand = this.InsertCommand) != null)
			{
				dbCommand.Dispose();
			}
			if ((dbCommand = this.UpdateCommand) != null)
			{
				dbCommand.Dispose();
			}
			if ((dbCommand = this.DeleteCommand) != null)
			{
				dbCommand.Dispose();
			}
			this.InsertCommand = null;
			this.UpdateCommand = null;
			this.DeleteCommand = null;
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x00221E64 File Offset: 0x00221264
		private static void RemoveExtraParameters(DbCommand command, int usedParameterCount)
		{
			for (int i = command.Parameters.Count - 1; i >= usedParameterCount; i--)
			{
				command.Parameters.RemoveAt(i);
			}
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x00221E98 File Offset: 0x00221298
		protected void RowUpdatingHandler(RowUpdatingEventArgs rowUpdatingEvent)
		{
			if (rowUpdatingEvent == null)
			{
				throw ADP.ArgumentNull("rowUpdatingEvent");
			}
			try
			{
				if (rowUpdatingEvent.Status == UpdateStatus.Continue)
				{
					StatementType statementType = rowUpdatingEvent.StatementType;
					DbCommand dbCommand = (DbCommand)rowUpdatingEvent.Command;
					if (dbCommand != null)
					{
						switch (statementType)
						{
						case StatementType.Select:
							return;
						case StatementType.Insert:
							dbCommand = this.InsertCommand;
							break;
						case StatementType.Update:
							dbCommand = this.UpdateCommand;
							break;
						case StatementType.Delete:
							dbCommand = this.DeleteCommand;
							break;
						default:
							throw ADP.InvalidStatementType(statementType);
						}
						if (dbCommand != rowUpdatingEvent.Command)
						{
							dbCommand = (DbCommand)rowUpdatingEvent.Command;
							if (dbCommand != null && dbCommand.Connection == null)
							{
								DbDataAdapter dataAdapter = this.DataAdapter;
								DbCommand dbCommand2 = ((dataAdapter != null) ? dataAdapter.SelectCommand : null);
								if (dbCommand2 != null)
								{
									dbCommand.Connection = dbCommand2.Connection;
								}
							}
						}
						else
						{
							dbCommand = null;
						}
					}
					if (dbCommand == null)
					{
						this.RowUpdatingHandlerBuilder(rowUpdatingEvent);
					}
				}
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				ADP.TraceExceptionForCapture(ex);
				rowUpdatingEvent.Status = UpdateStatus.ErrorsOccurred;
				rowUpdatingEvent.Errors = ex;
			}
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x00221FB0 File Offset: 0x002213B0
		private void RowUpdatingHandlerBuilder(RowUpdatingEventArgs rowUpdatingEvent)
		{
			DataRow row = rowUpdatingEvent.Row;
			this.BuildCache(false, row, false);
			DbCommand dbCommand;
			switch (rowUpdatingEvent.StatementType)
			{
			case StatementType.Insert:
				dbCommand = this.BuildInsertCommand(rowUpdatingEvent.TableMapping, row);
				break;
			case StatementType.Update:
				dbCommand = this.BuildUpdateCommand(rowUpdatingEvent.TableMapping, row);
				break;
			case StatementType.Delete:
				dbCommand = this.BuildDeleteCommand(rowUpdatingEvent.TableMapping, row);
				break;
			default:
				throw ADP.InvalidStatementType(rowUpdatingEvent.StatementType);
			}
			if (dbCommand == null)
			{
				if (row != null)
				{
					row.AcceptChanges();
				}
				rowUpdatingEvent.Status = UpdateStatus.SkipCurrentRow;
			}
			rowUpdatingEvent.Command = dbCommand;
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x00222040 File Offset: 0x00221440
		public virtual string UnquoteIdentifier(string quotedIdentifier)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06001359 RID: 4953
		protected abstract void ApplyParameterInfo(DbParameter parameter, DataRow row, StatementType statementType, bool whereClause);

		// Token: 0x0600135A RID: 4954
		protected abstract string GetParameterName(int parameterOrdinal);

		// Token: 0x0600135B RID: 4955
		protected abstract string GetParameterName(string parameterName);

		// Token: 0x0600135C RID: 4956
		protected abstract string GetParameterPlaceholder(int parameterOrdinal);

		// Token: 0x0600135D RID: 4957
		protected abstract void SetRowUpdatingHandler(DbDataAdapter adapter);

		// Token: 0x0600135E RID: 4958 RVA: 0x00222054 File Offset: 0x00221454
		internal static string[] ParseProcedureName(string name, string quotePrefix, string quoteSuffix)
		{
			string[] array = new string[4];
			if (!ADP.IsEmpty(name))
			{
				bool flag = !ADP.IsEmpty(quotePrefix) && !ADP.IsEmpty(quoteSuffix);
				int i = 0;
				int num = 0;
				while (num < array.Length && i < name.Length)
				{
					int num2 = i;
					if (flag && name.IndexOf(quotePrefix, i, quotePrefix.Length, StringComparison.Ordinal) == i)
					{
						for (i += quotePrefix.Length; i < name.Length; i += quoteSuffix.Length)
						{
							i = name.IndexOf(quoteSuffix, i, StringComparison.Ordinal);
							if (i < 0)
							{
								i = name.Length;
								break;
							}
							i += quoteSuffix.Length;
							if (i >= name.Length || name.IndexOf(quoteSuffix, i, quoteSuffix.Length, StringComparison.Ordinal) != i)
							{
								break;
							}
						}
					}
					if (i < name.Length)
					{
						i = name.IndexOf(".", i, StringComparison.Ordinal);
						if (i < 0 || num == array.Length - 1)
						{
							i = name.Length;
						}
					}
					array[num] = name.Substring(num2, i - num2);
					i += ".".Length;
					num++;
				}
				int num3 = array.Length - 1;
				while (0 <= num3)
				{
					array[num3] = ((0 < num) ? array[--num] : null);
					num3--;
				}
			}
			return array;
		}

		// Token: 0x04000BC0 RID: 3008
		private const string DeleteFrom = "DELETE FROM ";

		// Token: 0x04000BC1 RID: 3009
		private const string InsertInto = "INSERT INTO ";

		// Token: 0x04000BC2 RID: 3010
		private const string DefaultValues = " DEFAULT VALUES";

		// Token: 0x04000BC3 RID: 3011
		private const string Values = " VALUES ";

		// Token: 0x04000BC4 RID: 3012
		private const string Update = "UPDATE ";

		// Token: 0x04000BC5 RID: 3013
		private const string Set = " SET ";

		// Token: 0x04000BC6 RID: 3014
		private const string Where = " WHERE ";

		// Token: 0x04000BC7 RID: 3015
		private const string SpaceLeftParenthesis = " (";

		// Token: 0x04000BC8 RID: 3016
		private const string Comma = ", ";

		// Token: 0x04000BC9 RID: 3017
		private const string Equal = " = ";

		// Token: 0x04000BCA RID: 3018
		private const string LeftParenthesis = "(";

		// Token: 0x04000BCB RID: 3019
		private const string RightParenthesis = ")";

		// Token: 0x04000BCC RID: 3020
		private const string NameSeparator = ".";

		// Token: 0x04000BCD RID: 3021
		private const string IsNull = " IS NULL";

		// Token: 0x04000BCE RID: 3022
		private const string EqualOne = " = 1";

		// Token: 0x04000BCF RID: 3023
		private const string And = " AND ";

		// Token: 0x04000BD0 RID: 3024
		private const string Or = " OR ";

		// Token: 0x04000BD1 RID: 3025
		private DbDataAdapter _dataAdapter;

		// Token: 0x04000BD2 RID: 3026
		private DbCommand _insertCommand;

		// Token: 0x04000BD3 RID: 3027
		private DbCommand _updateCommand;

		// Token: 0x04000BD4 RID: 3028
		private DbCommand _deleteCommand;

		// Token: 0x04000BD5 RID: 3029
		private MissingMappingAction _missingMappingAction;

		// Token: 0x04000BD6 RID: 3030
		private ConflictOption _conflictDetection = ConflictOption.CompareAllSearchableValues;

		// Token: 0x04000BD7 RID: 3031
		private bool _setAllValues;

		// Token: 0x04000BD8 RID: 3032
		private bool _hasPartialPrimaryKey;

		// Token: 0x04000BD9 RID: 3033
		private DataTable _dbSchemaTable;

		// Token: 0x04000BDA RID: 3034
		private DbSchemaRow[] _dbSchemaRows;

		// Token: 0x04000BDB RID: 3035
		private string[] _sourceColumnNames;

		// Token: 0x04000BDC RID: 3036
		private DbCommandBuilder.ParameterNames _parameterNames;

		// Token: 0x04000BDD RID: 3037
		private string _quotedBaseTableName;

		// Token: 0x04000BDE RID: 3038
		private CatalogLocation _catalogLocation = CatalogLocation.Start;

		// Token: 0x04000BDF RID: 3039
		private string _catalogSeparator = ".";

		// Token: 0x04000BE0 RID: 3040
		private string _schemaSeparator = ".";

		// Token: 0x04000BE1 RID: 3041
		private string _quotePrefix = "";

		// Token: 0x04000BE2 RID: 3042
		private string _quoteSuffix = "";

		// Token: 0x04000BE3 RID: 3043
		private string _parameterNamePattern;

		// Token: 0x04000BE4 RID: 3044
		private string _parameterMarkerFormat;

		// Token: 0x04000BE5 RID: 3045
		private int _parameterNameMaxLength;

		// Token: 0x02000128 RID: 296
		private class ParameterNames
		{
			// Token: 0x0600135F RID: 4959 RVA: 0x00222184 File Offset: 0x00221584
			internal ParameterNames(DbCommandBuilder dbCommandBuilder, DbSchemaRow[] schemaRows)
			{
				this._dbCommandBuilder = dbCommandBuilder;
				this._baseParameterNames = new string[schemaRows.Length];
				this._originalParameterNames = new string[schemaRows.Length];
				this._nullParameterNames = new string[schemaRows.Length];
				this._isMutatedName = new bool[schemaRows.Length];
				this._count = schemaRows.Length;
				this._parameterNameParser = new Regex(this._dbCommandBuilder.ParameterNamePattern, RegexOptions.ExplicitCapture | RegexOptions.Singleline);
				this.SetAndValidateNamePrefixes();
				this._adjustedParameterNameMaxLength = this.GetAdjustedParameterNameMaxLength();
				for (int i = 0; i < schemaRows.Length; i++)
				{
					if (schemaRows[i] != null)
					{
						bool flag = false;
						string text = schemaRows[i].ColumnName;
						if ((this._originalPrefix == null || !text.StartsWith(this._originalPrefix, StringComparison.OrdinalIgnoreCase)) && (this._isNullPrefix == null || !text.StartsWith(this._isNullPrefix, StringComparison.OrdinalIgnoreCase)))
						{
							if (text.IndexOf(' ') >= 0)
							{
								text = text.Replace(' ', '_');
								flag = true;
							}
							if (this._parameterNameParser.IsMatch(text) && text.Length <= this._adjustedParameterNameMaxLength)
							{
								this._baseParameterNames[i] = text;
								this._isMutatedName[i] = flag;
							}
						}
					}
				}
				this.EliminateConflictingNames();
				for (int j = 0; j < schemaRows.Length; j++)
				{
					if (this._baseParameterNames[j] != null)
					{
						if (this._originalPrefix != null)
						{
							this._originalParameterNames[j] = this._originalPrefix + this._baseParameterNames[j];
						}
						if (this._isNullPrefix != null && schemaRows[j].AllowDBNull)
						{
							this._nullParameterNames[j] = this._isNullPrefix + this._baseParameterNames[j];
						}
					}
				}
				this.ApplyProviderSpecificFormat();
				this.GenerateMissingNames(schemaRows);
			}

			// Token: 0x06001360 RID: 4960 RVA: 0x00222320 File Offset: 0x00221720
			private void SetAndValidateNamePrefixes()
			{
				if (this._parameterNameParser.IsMatch("IsNull_"))
				{
					this._isNullPrefix = "IsNull_";
				}
				else if (this._parameterNameParser.IsMatch("isnull"))
				{
					this._isNullPrefix = "isnull";
				}
				else if (this._parameterNameParser.IsMatch("ISNULL"))
				{
					this._isNullPrefix = "ISNULL";
				}
				else
				{
					this._isNullPrefix = null;
				}
				if (this._parameterNameParser.IsMatch("Original_"))
				{
					this._originalPrefix = "Original_";
					return;
				}
				if (this._parameterNameParser.IsMatch("original"))
				{
					this._originalPrefix = "original";
					return;
				}
				if (this._parameterNameParser.IsMatch("ORIGINAL"))
				{
					this._originalPrefix = "ORIGINAL";
					return;
				}
				this._originalPrefix = null;
			}

			// Token: 0x06001361 RID: 4961 RVA: 0x002223F4 File Offset: 0x002217F4
			private void ApplyProviderSpecificFormat()
			{
				for (int i = 0; i < this._baseParameterNames.Length; i++)
				{
					if (this._baseParameterNames[i] != null)
					{
						this._baseParameterNames[i] = this._dbCommandBuilder.GetParameterName(this._baseParameterNames[i]);
					}
					if (this._originalParameterNames[i] != null)
					{
						this._originalParameterNames[i] = this._dbCommandBuilder.GetParameterName(this._originalParameterNames[i]);
					}
					if (this._nullParameterNames[i] != null)
					{
						this._nullParameterNames[i] = this._dbCommandBuilder.GetParameterName(this._nullParameterNames[i]);
					}
				}
			}

			// Token: 0x06001362 RID: 4962 RVA: 0x00222484 File Offset: 0x00221884
			private void EliminateConflictingNames()
			{
				for (int i = 0; i < this._count - 1; i++)
				{
					string text = this._baseParameterNames[i];
					if (text != null)
					{
						for (int j = i + 1; j < this._count; j++)
						{
							if (ADP.CompareInsensitiveInvariant(text, this._baseParameterNames[j]))
							{
								int num = (this._isMutatedName[j] ? j : i);
								this._baseParameterNames[num] = null;
							}
						}
					}
				}
			}

			// Token: 0x06001363 RID: 4963 RVA: 0x002224EC File Offset: 0x002218EC
			internal void GenerateMissingNames(DbSchemaRow[] schemaRows)
			{
				for (int i = 0; i < this._baseParameterNames.Length; i++)
				{
					if (this._baseParameterNames[i] == null)
					{
						this._baseParameterNames[i] = this.GetNextGenericParameterName();
						this._originalParameterNames[i] = this.GetNextGenericParameterName();
						if (schemaRows[i] != null && schemaRows[i].AllowDBNull)
						{
							this._nullParameterNames[i] = this.GetNextGenericParameterName();
						}
					}
				}
			}

			// Token: 0x06001364 RID: 4964 RVA: 0x00222554 File Offset: 0x00221954
			private int GetAdjustedParameterNameMaxLength()
			{
				int num = Math.Max((this._isNullPrefix != null) ? this._isNullPrefix.Length : 0, (this._originalPrefix != null) ? this._originalPrefix.Length : 0) + this._dbCommandBuilder.GetParameterName("").Length;
				return this._dbCommandBuilder.ParameterNameMaxLength - num;
			}

			// Token: 0x06001365 RID: 4965 RVA: 0x002225B8 File Offset: 0x002219B8
			private string GetNextGenericParameterName()
			{
				bool flag;
				string parameterName;
				do
				{
					flag = false;
					this._genericParameterCount++;
					parameterName = this._dbCommandBuilder.GetParameterName(this._genericParameterCount);
					for (int i = 0; i < this._baseParameterNames.Length; i++)
					{
						if (ADP.CompareInsensitiveInvariant(this._baseParameterNames[i], parameterName))
						{
							flag = true;
							break;
						}
					}
				}
				while (flag);
				return parameterName;
			}

			// Token: 0x06001366 RID: 4966 RVA: 0x00222614 File Offset: 0x00221A14
			internal string GetBaseParameterName(int index)
			{
				return this._baseParameterNames[index];
			}

			// Token: 0x06001367 RID: 4967 RVA: 0x0022262C File Offset: 0x00221A2C
			internal string GetOriginalParameterName(int index)
			{
				return this._originalParameterNames[index];
			}

			// Token: 0x06001368 RID: 4968 RVA: 0x00222644 File Offset: 0x00221A44
			internal string GetNullParameterName(int index)
			{
				return this._nullParameterNames[index];
			}

			// Token: 0x04000BE6 RID: 3046
			private const string DefaultOriginalPrefix = "Original_";

			// Token: 0x04000BE7 RID: 3047
			private const string DefaultIsNullPrefix = "IsNull_";

			// Token: 0x04000BE8 RID: 3048
			private const string AlternativeOriginalPrefix = "original";

			// Token: 0x04000BE9 RID: 3049
			private const string AlternativeIsNullPrefix = "isnull";

			// Token: 0x04000BEA RID: 3050
			private const string AlternativeOriginalPrefix2 = "ORIGINAL";

			// Token: 0x04000BEB RID: 3051
			private const string AlternativeIsNullPrefix2 = "ISNULL";

			// Token: 0x04000BEC RID: 3052
			private string _originalPrefix;

			// Token: 0x04000BED RID: 3053
			private string _isNullPrefix;

			// Token: 0x04000BEE RID: 3054
			private Regex _parameterNameParser;

			// Token: 0x04000BEF RID: 3055
			private DbCommandBuilder _dbCommandBuilder;

			// Token: 0x04000BF0 RID: 3056
			private string[] _baseParameterNames;

			// Token: 0x04000BF1 RID: 3057
			private string[] _originalParameterNames;

			// Token: 0x04000BF2 RID: 3058
			private string[] _nullParameterNames;

			// Token: 0x04000BF3 RID: 3059
			private bool[] _isMutatedName;

			// Token: 0x04000BF4 RID: 3060
			private int _count;

			// Token: 0x04000BF5 RID: 3061
			private int _genericParameterCount;

			// Token: 0x04000BF6 RID: 3062
			private int _adjustedParameterNameMaxLength;
		}
	}
}
