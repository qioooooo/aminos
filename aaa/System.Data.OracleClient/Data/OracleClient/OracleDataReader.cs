using System;
using System.Collections;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Data.OracleClient
{
	// Token: 0x02000062 RID: 98
	public sealed class OracleDataReader : DbDataReader
	{
		// Token: 0x06000424 RID: 1060 RVA: 0x0006474C File Offset: 0x00063B4C
		internal OracleDataReader(OracleCommand command, OciStatementHandle statementHandle, string statementText, CommandBehavior commandBehavior)
		{
			this._commandBehavior = commandBehavior;
			this._statementHandle = statementHandle;
			this._connection = command.Connection;
			this._connectionCloseCount = this._connection.CloseCount;
			this._columnInfo = null;
			if (OCI.STMT.OCI_STMT_SELECT == command.StatementType)
			{
				this.FillColumnInfo();
				this._recordsAffected = -1;
				if (this.IsCommandBehavior(CommandBehavior.SchemaOnly))
				{
					this._endOfData = true;
				}
			}
			else
			{
				this._statementHandle.GetAttribute(OCI.ATTR.OCI_ATTR_ROW_COUNT, out this._recordsAffected, this.ErrorHandle);
				this._endOfData = true;
				this._hasRows = 1;
			}
			this._statementText = statementText;
			this._closeConnectionToo = this.IsCommandBehavior(CommandBehavior.CloseConnection);
			if (CommandType.Text == command.CommandType)
			{
				this._keyInfoRequested = this.IsCommandBehavior(CommandBehavior.KeyInfo);
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0006481C File Offset: 0x00063C1C
		internal OracleDataReader(OracleConnection connection, OciStatementHandle statementHandle)
		{
			this._commandBehavior = CommandBehavior.Default;
			this._statementHandle = statementHandle;
			this._connection = connection;
			this._connectionCloseCount = this._connection.CloseCount;
			this._recordsAffected = -1;
			this.FillColumnInfo();
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00064874 File Offset: 0x00063C74
		internal OracleDataReader(OracleCommand command, ArrayList refCursorParameterOrdinals, string statementText, CommandBehavior commandBehavior)
		{
			this._commandBehavior = commandBehavior;
			this._statementText = statementText;
			this._closeConnectionToo = this.IsCommandBehavior(CommandBehavior.CloseConnection);
			if (CommandType.Text == command.CommandType)
			{
				this._keyInfoRequested = this.IsCommandBehavior(CommandBehavior.KeyInfo);
			}
			ArrayList arrayList = new ArrayList();
			int num = 0;
			for (int i = 0; i < refCursorParameterOrdinals.Count; i++)
			{
				int num2 = (int)refCursorParameterOrdinals[i];
				OracleParameter oracleParameter = command.Parameters[num2];
				if (OracleType.Cursor == oracleParameter.OracleType)
				{
					OracleDataReader oracleDataReader = (OracleDataReader)oracleParameter.Value;
					oracleDataReader._recordsAffected = num;
					arrayList.Add(oracleDataReader);
					oracleParameter.Value = DBNull.Value;
				}
				else
				{
					num += (int)oracleParameter.Value;
				}
			}
			this._refCursorDataReaders = new OracleDataReader[arrayList.Count];
			arrayList.CopyTo(this._refCursorDataReaders);
			this._nextRefCursor = 0;
			this.NextResultInternal();
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00064970 File Offset: 0x00063D70
		public override int Depth
		{
			get
			{
				this.AssertReaderIsOpen("Depth");
				return 0;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x0006498C File Offset: 0x00063D8C
		private OciErrorHandle ErrorHandle
		{
			get
			{
				return this._connection.ErrorHandle;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x000649A4 File Offset: 0x00063DA4
		public override int FieldCount
		{
			get
			{
				this.AssertReaderIsOpen();
				if (this._columnInfo == null)
				{
					return 0;
				}
				return this._columnInfo.Length;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x000649CC File Offset: 0x00063DCC
		public override bool HasRows
		{
			get
			{
				this.AssertReaderIsOpen();
				bool flag = 2 == this._hasRows;
				if (this._hasRows == 0)
				{
					flag = this.ReadInternal();
					if (this._buffer != null)
					{
						this._buffer.MovePrevious();
					}
					this._hasRows = (flag ? 2 : 1);
				}
				return flag;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x00064A1C File Offset: 0x00063E1C
		public override bool IsClosed
		{
			get
			{
				return this._statementHandle == null || this._connection == null || this._connectionCloseCount != this._connection.CloseCount;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00064A54 File Offset: 0x00063E54
		private bool IsValidRow
		{
			get
			{
				return !this._endOfData && this._buffer != null && this._buffer.CurrentPositionIsValid;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00064A80 File Offset: 0x00063E80
		public override int RecordsAffected
		{
			get
			{
				return this._recordsAffected;
			}
		}

		// Token: 0x170000CE RID: 206
		public override object this[int i]
		{
			get
			{
				return this.GetValue(i);
			}
		}

		// Token: 0x170000CF RID: 207
		public override object this[string name]
		{
			get
			{
				int ordinal = this.GetOrdinal(name);
				return this.GetValue(ordinal);
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00064AC4 File Offset: 0x00063EC4
		private void AssertReaderHasColumns()
		{
			if (0 >= this.FieldCount)
			{
				throw ADP.DataReaderNoData();
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00064AE0 File Offset: 0x00063EE0
		private void AssertReaderHasData()
		{
			if (!this.IsValidRow)
			{
				throw ADP.DataReaderNoData();
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00064AFC File Offset: 0x00063EFC
		private void AssertReaderIsOpen(string methodName)
		{
			if (this.IsClosed)
			{
				throw ADP.DataReaderClosed(methodName);
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00064B18 File Offset: 0x00063F18
		private void AssertReaderIsOpen()
		{
			if (this._connection != null && this._connectionCloseCount != this._connection.CloseCount)
			{
				this.Close();
			}
			if (this._statementHandle == null)
			{
				throw ADP.ClosedDataReaderError();
			}
			if (this._connection == null || ConnectionState.Open != this._connection.State)
			{
				throw ADP.ClosedConnectionError();
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00064B70 File Offset: 0x00063F70
		private object SetSchemaValue(string value)
		{
			if (ADP.IsEmpty(value))
			{
				return DBNull.Value;
			}
			return value;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00064B8C File Offset: 0x00063F8C
		private void Cleanup()
		{
			if (this._buffer != null)
			{
				this._buffer.Dispose();
				this._buffer = null;
			}
			if (this._columnInfo != null)
			{
				if (this._refCursorDataReaders == null)
				{
					int num = this._columnInfo.Length;
					while (--num >= 0)
					{
						if (this._columnInfo[num] != null)
						{
							this._columnInfo[num].Dispose();
							this._columnInfo[num] = null;
						}
					}
				}
				this._columnInfo = null;
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00064C00 File Offset: 0x00064000
		public override void Close()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ora.OracleDataReader.Close|API> %d#\n", this.ObjectID);
			try
			{
				OciHandle.SafeDispose(ref this._statementHandle);
				this.Cleanup();
				if (this._refCursorDataReaders != null)
				{
					int num = this._refCursorDataReaders.Length;
					while (--num >= 0)
					{
						OracleDataReader oracleDataReader = this._refCursorDataReaders[num];
						this._refCursorDataReaders[num] = null;
						if (oracleDataReader != null)
						{
							oracleDataReader.Dispose();
						}
					}
					this._refCursorDataReaders = null;
				}
				if (this._closeConnectionToo && this._connection != null)
				{
					this._connection.Close();
				}
				this._connection = null;
				this._fieldNameLookup = null;
				this._schemaTable = null;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00064CC4 File Offset: 0x000640C4
		private DataTable CreateSchemaTable(int columnCount)
		{
			DataTable dataTable = new DataTable("SchemaTable");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.MinimumCapacity = columnCount;
			DataColumn dataColumn = new DataColumn(SchemaTableColumn.ColumnName, typeof(string));
			DataColumn dataColumn2 = new DataColumn(SchemaTableColumn.ColumnOrdinal, typeof(int));
			DataColumn dataColumn3 = new DataColumn(SchemaTableColumn.ColumnSize, typeof(int));
			DataColumn dataColumn4 = new DataColumn(SchemaTableColumn.NumericPrecision, typeof(short));
			DataColumn dataColumn5 = new DataColumn(SchemaTableColumn.NumericScale, typeof(short));
			DataColumn dataColumn6 = new DataColumn(SchemaTableColumn.DataType, typeof(Type));
			DataColumn dataColumn7 = new DataColumn(SchemaTableColumn.ProviderType, typeof(int));
			DataColumn dataColumn8 = new DataColumn(SchemaTableColumn.IsLong, typeof(bool));
			DataColumn dataColumn9 = new DataColumn(SchemaTableColumn.AllowDBNull, typeof(bool));
			DataColumn dataColumn10 = new DataColumn(SchemaTableColumn.IsAliased, typeof(bool));
			DataColumn dataColumn11 = new DataColumn(SchemaTableColumn.IsExpression, typeof(bool));
			DataColumn dataColumn12 = new DataColumn(SchemaTableColumn.IsKey, typeof(bool));
			DataColumn dataColumn13 = new DataColumn(SchemaTableColumn.IsUnique, typeof(bool));
			DataColumn dataColumn14 = new DataColumn(SchemaTableColumn.BaseSchemaName, typeof(string));
			DataColumn dataColumn15 = new DataColumn(SchemaTableColumn.BaseTableName, typeof(string));
			DataColumn dataColumn16 = new DataColumn(SchemaTableColumn.BaseColumnName, typeof(string));
			dataColumn2.DefaultValue = 0;
			dataColumn8.DefaultValue = false;
			DataColumnCollection columns = dataTable.Columns;
			columns.Add(dataColumn);
			columns.Add(dataColumn2);
			columns.Add(dataColumn3);
			columns.Add(dataColumn4);
			columns.Add(dataColumn5);
			columns.Add(dataColumn6);
			columns.Add(dataColumn7);
			columns.Add(dataColumn8);
			columns.Add(dataColumn9);
			columns.Add(dataColumn10);
			columns.Add(dataColumn11);
			columns.Add(dataColumn12);
			columns.Add(dataColumn13);
			columns.Add(dataColumn14);
			columns.Add(dataColumn15);
			columns.Add(dataColumn16);
			for (int i = 0; i < columns.Count; i++)
			{
				columns[i].ReadOnly = true;
			}
			return dataTable;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00064F0C File Offset: 0x0006430C
		internal void FillColumnInfo()
		{
			bool flag = false;
			int num;
			this._statementHandle.GetAttribute(OCI.ATTR.OCI_ATTR_PARAM_COUNT, out num, this.ErrorHandle);
			this._columnInfo = new OracleColumn[num];
			this._rowBufferLength = 0;
			for (int i = 0; i < num; i++)
			{
				this._columnInfo[i] = new OracleColumn(this._statementHandle, i, this.ErrorHandle, this._connection);
				if (this._columnInfo[i].Describe(ref this._rowBufferLength, this._connection, this.ErrorHandle))
				{
					flag = true;
				}
			}
			if (flag || this._rowBufferLength == 0)
			{
				this._rowsToPrefetch = 1;
				return;
			}
			this._rowsToPrefetch = (65536 + this._rowBufferLength - 1) / this._rowBufferLength;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00064FC0 File Offset: 0x000643C0
		private void FillSchemaTable(DataTable schemaTable)
		{
			DataColumn dataColumn = new DataColumn(SchemaTableOptionalColumn.ProviderSpecificDataType, typeof(Type));
			schemaTable.Columns.Add(dataColumn);
			int fieldCount = this.FieldCount;
			DbSqlParserColumnCollection dbSqlParserColumnCollection = null;
			int num = 0;
			if (this._keyInfoRequested)
			{
				OracleSqlParser oracleSqlParser = new OracleSqlParser();
				oracleSqlParser.Parse(this._statementText, this._connection);
				dbSqlParserColumnCollection = oracleSqlParser.Columns;
				num = dbSqlParserColumnCollection.Count;
			}
			for (int i = 0; i < fieldCount; i++)
			{
				OracleColumn oracleColumn = this._columnInfo[i];
				DataRow dataRow = schemaTable.NewRow();
				dataRow[SchemaTableColumn.ColumnName] = oracleColumn.ColumnName;
				dataRow[SchemaTableColumn.ColumnOrdinal] = oracleColumn.Ordinal;
				if (oracleColumn.IsLong | oracleColumn.IsLob)
				{
					dataRow[SchemaTableColumn.ColumnSize] = int.MaxValue;
				}
				else
				{
					dataRow[SchemaTableColumn.ColumnSize] = oracleColumn.SchemaTableSize;
				}
				dataRow[SchemaTableColumn.NumericPrecision] = oracleColumn.Precision;
				dataRow[SchemaTableColumn.NumericScale] = oracleColumn.Scale;
				dataRow[SchemaTableColumn.DataType] = oracleColumn.GetFieldType();
				dataRow[dataColumn] = oracleColumn.GetFieldOracleType();
				dataRow[SchemaTableColumn.ProviderType] = oracleColumn.OracleType;
				dataRow[SchemaTableColumn.IsLong] = oracleColumn.IsLong | oracleColumn.IsLob;
				dataRow[SchemaTableColumn.AllowDBNull] = oracleColumn.IsNullable;
				if (this._keyInfoRequested && num == fieldCount)
				{
					DbSqlParserColumn dbSqlParserColumn = dbSqlParserColumnCollection[i];
					dataRow[SchemaTableColumn.IsAliased] = dbSqlParserColumn.IsAliased;
					dataRow[SchemaTableColumn.IsExpression] = dbSqlParserColumn.IsExpression;
					dataRow[SchemaTableColumn.IsKey] = dbSqlParserColumn.IsKey;
					dataRow[SchemaTableColumn.IsUnique] = dbSqlParserColumn.IsUnique;
					dataRow[SchemaTableColumn.BaseSchemaName] = this.SetSchemaValue(OracleSqlParser.CatalogCase(dbSqlParserColumn.SchemaName));
					dataRow[SchemaTableColumn.BaseTableName] = this.SetSchemaValue(OracleSqlParser.CatalogCase(dbSqlParserColumn.TableName));
					dataRow[SchemaTableColumn.BaseColumnName] = this.SetSchemaValue(OracleSqlParser.CatalogCase(dbSqlParserColumn.ColumnName));
				}
				else
				{
					dataRow[SchemaTableColumn.IsAliased] = DBNull.Value;
					dataRow[SchemaTableColumn.IsExpression] = DBNull.Value;
					dataRow[SchemaTableColumn.IsKey] = DBNull.Value;
					dataRow[SchemaTableColumn.IsUnique] = DBNull.Value;
					dataRow[SchemaTableColumn.BaseSchemaName] = DBNull.Value;
					dataRow[SchemaTableColumn.BaseTableName] = DBNull.Value;
					dataRow[SchemaTableColumn.BaseColumnName] = DBNull.Value;
				}
				schemaTable.Rows.Add(dataRow);
				dataRow.AcceptChanges();
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0006529C File Offset: 0x0006469C
		public override string GetDataTypeName(int i)
		{
			this.AssertReaderIsOpen();
			if (this._columnInfo == null)
			{
				throw ADP.NoData();
			}
			return this._columnInfo[i].GetDataTypeName();
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x000652CC File Offset: 0x000646CC
		public override Type GetProviderSpecificFieldType(int i)
		{
			if (this._columnInfo == null)
			{
				this.AssertReaderIsOpen();
				throw ADP.NoData();
			}
			return this._columnInfo[i].GetFieldOracleType();
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x000652FC File Offset: 0x000646FC
		public override IEnumerator GetEnumerator()
		{
			return new DbEnumerator(this, this.IsCommandBehavior(CommandBehavior.CloseConnection));
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00065318 File Offset: 0x00064718
		public override Type GetFieldType(int i)
		{
			if (this._columnInfo == null)
			{
				this.AssertReaderIsOpen();
				throw ADP.NoData();
			}
			return this._columnInfo[i].GetFieldType();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00065348 File Offset: 0x00064748
		public override string GetName(int i)
		{
			if (this._columnInfo == null)
			{
				this.AssertReaderIsOpen();
				throw ADP.NoData();
			}
			return this._columnInfo[i].ColumnName;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00065378 File Offset: 0x00064778
		public override int GetOrdinal(string name)
		{
			this.AssertReaderIsOpen("GetOrdinal");
			this.AssertReaderHasColumns();
			if (this._fieldNameLookup == null)
			{
				this._fieldNameLookup = new FieldNameLookup(this, -1);
			}
			return this._fieldNameLookup.GetOrdinal(name);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x000653B8 File Offset: 0x000647B8
		public override DataTable GetSchemaTable()
		{
			DataTable dataTable = this._schemaTable;
			if (dataTable == null)
			{
				this.AssertReaderIsOpen("GetSchemaTable");
				if (0 < this.FieldCount)
				{
					dataTable = this.CreateSchemaTable(this.FieldCount);
					this.FillSchemaTable(dataTable);
					this._schemaTable = dataTable;
				}
				else if (0 > this.FieldCount)
				{
					throw ADP.DataReaderNoData();
				}
			}
			return dataTable;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00065410 File Offset: 0x00064810
		public override object GetValue(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetValue(this._buffer);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00065440 File Offset: 0x00064840
		public override int GetValues(object[] values)
		{
			if (values == null)
			{
				throw ADP.ArgumentNull("values");
			}
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			int num = Math.Min(values.Length, this.FieldCount);
			for (int i = 0; i < num; i++)
			{
				values[i] = this._columnInfo[i].GetValue(this._buffer);
			}
			return num;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0006549C File Offset: 0x0006489C
		public override bool GetBoolean(int i)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x000654B0 File Offset: 0x000648B0
		public override byte GetByte(int i)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x000654C4 File Offset: 0x000648C4
		public override long GetBytes(int i, long fieldOffset, byte[] buffer2, int bufferoffset, int length)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetBytes(this._buffer, fieldOffset, buffer2, bufferoffset, length);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x000654F8 File Offset: 0x000648F8
		public override char GetChar(int i)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0006550C File Offset: 0x0006490C
		public override long GetChars(int i, long fieldOffset, char[] buffer2, int bufferoffset, int length)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetChars(this._buffer, fieldOffset, buffer2, bufferoffset, length);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00065540 File Offset: 0x00064940
		public override DateTime GetDateTime(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetDateTime(this._buffer);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0006556C File Offset: 0x0006496C
		public override decimal GetDecimal(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetDecimal(this._buffer);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00065598 File Offset: 0x00064998
		public override double GetDouble(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetDouble(this._buffer);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x000655C4 File Offset: 0x000649C4
		public override float GetFloat(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetFloat(this._buffer);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000655F0 File Offset: 0x000649F0
		public override Guid GetGuid(int i)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00065604 File Offset: 0x00064A04
		public override short GetInt16(int i)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00065618 File Offset: 0x00064A18
		public override int GetInt32(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetInt32(this._buffer);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00065644 File Offset: 0x00064A44
		public override long GetInt64(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetInt64(this._buffer);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00065670 File Offset: 0x00064A70
		public override object GetProviderSpecificValue(int i)
		{
			return this.GetOracleValue(i);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00065684 File Offset: 0x00064A84
		public override int GetProviderSpecificValues(object[] values)
		{
			return this.GetOracleValues(values);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00065698 File Offset: 0x00064A98
		public override string GetString(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetString(this._buffer);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x000656C4 File Offset: 0x00064AC4
		public TimeSpan GetTimeSpan(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetTimeSpan(this._buffer);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x000656F0 File Offset: 0x00064AF0
		public OracleBFile GetOracleBFile(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetOracleBFile(this._buffer);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0006571C File Offset: 0x00064B1C
		public OracleBinary GetOracleBinary(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetOracleBinary(this._buffer);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00065748 File Offset: 0x00064B48
		public OracleDateTime GetOracleDateTime(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetOracleDateTime(this._buffer);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00065774 File Offset: 0x00064B74
		public OracleLob GetOracleLob(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetOracleLob(this._buffer);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x000657A0 File Offset: 0x00064BA0
		public OracleMonthSpan GetOracleMonthSpan(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetOracleMonthSpan(this._buffer);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000657CC File Offset: 0x00064BCC
		public OracleNumber GetOracleNumber(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetOracleNumber(this._buffer);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000657F8 File Offset: 0x00064BF8
		public OracleString GetOracleString(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetOracleString(this._buffer);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00065824 File Offset: 0x00064C24
		public OracleTimeSpan GetOracleTimeSpan(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetOracleTimeSpan(this._buffer);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00065850 File Offset: 0x00064C50
		public object GetOracleValue(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].GetOracleValue(this._buffer);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0006587C File Offset: 0x00064C7C
		public int GetOracleValues(object[] values)
		{
			if (values == null)
			{
				throw ADP.ArgumentNull("values");
			}
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			int num = Math.Min(values.Length, this.FieldCount);
			for (int i = 0; i < num; i++)
			{
				values[i] = this.GetOracleValue(i);
			}
			return num;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000658CC File Offset: 0x00064CCC
		private bool IsCommandBehavior(CommandBehavior condition)
		{
			return condition == (condition & this._commandBehavior);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x000658E4 File Offset: 0x00064CE4
		public override bool IsDBNull(int i)
		{
			this.AssertReaderIsOpen();
			this.AssertReaderHasData();
			return this._columnInfo[i].IsDBNull(this._buffer);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00065910 File Offset: 0x00064D10
		public override bool NextResult()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ora.OracleDataReader.NextResult|API> %d#\n", this.ObjectID);
			bool flag;
			try
			{
				this.AssertReaderIsOpen("NextResult");
				this._fieldNameLookup = null;
				this._schemaTable = null;
				flag = this.NextResultInternal();
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return flag;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00065978 File Offset: 0x00064D78
		private bool NextResultInternal()
		{
			this.Cleanup();
			if (this._refCursorDataReaders == null || this._nextRefCursor >= this._refCursorDataReaders.Length)
			{
				this._endOfData = true;
				this._hasRows = 1;
				return false;
			}
			if (this._nextRefCursor > 0)
			{
				this._refCursorDataReaders[this._nextRefCursor - 1].Dispose();
				this._refCursorDataReaders[this._nextRefCursor - 1] = null;
			}
			OciStatementHandle statementHandle = this._statementHandle;
			this._statementHandle = this._refCursorDataReaders[this._nextRefCursor]._statementHandle;
			OciHandle.SafeDispose(ref statementHandle);
			this._connection = this._refCursorDataReaders[this._nextRefCursor]._connection;
			this._connectionCloseCount = this._refCursorDataReaders[this._nextRefCursor]._connectionCloseCount;
			this._hasRows = this._refCursorDataReaders[this._nextRefCursor]._hasRows;
			this._recordsAffected = this._refCursorDataReaders[this._nextRefCursor]._recordsAffected;
			this._columnInfo = this._refCursorDataReaders[this._nextRefCursor]._columnInfo;
			this._rowBufferLength = this._refCursorDataReaders[this._nextRefCursor]._rowBufferLength;
			this._rowsToPrefetch = this._refCursorDataReaders[this._nextRefCursor]._rowsToPrefetch;
			this._nextRefCursor++;
			this._endOfData = false;
			this._isLastBuffer = false;
			this._rowsTotal = 0;
			return true;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00065AD4 File Offset: 0x00064ED4
		public override bool Read()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ora.OracleDataReader.Read|API> %d#\n", this.ObjectID);
			bool flag2;
			try
			{
				this.AssertReaderIsOpen("Read");
				bool flag = this.ReadInternal();
				if (flag)
				{
					this._hasRows = 2;
				}
				flag2 = flag;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return flag2;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00065B38 File Offset: 0x00064F38
		private bool ReadInternal()
		{
			if (this._endOfData)
			{
				return false;
			}
			int num = this._columnInfo.Length;
			NativeBuffer_RowBuffer nativeBuffer_RowBuffer = this._buffer;
			bool flag = false;
			bool[] array = new bool[num];
			SafeHandle[] array2 = new SafeHandle[num];
			RuntimeHelpers.PrepareConstrainedRegions();
			bool flag2;
			try
			{
				if (nativeBuffer_RowBuffer == null)
				{
					int num2 = ((this._rowsToPrefetch > 1) ? this._rowBufferLength : 0);
					nativeBuffer_RowBuffer = new NativeBuffer_RowBuffer(this._rowBufferLength, this._rowsToPrefetch);
					nativeBuffer_RowBuffer.DangerousAddRef(ref flag);
					for (int i = 0; i < num; i++)
					{
						this._columnInfo[i].Bind(this._statementHandle, nativeBuffer_RowBuffer, this.ErrorHandle, num2);
					}
					this._buffer = nativeBuffer_RowBuffer;
				}
				else
				{
					nativeBuffer_RowBuffer.DangerousAddRef(ref flag);
				}
				if (nativeBuffer_RowBuffer.MoveNext())
				{
					flag2 = true;
				}
				else if (this._isLastBuffer)
				{
					this._endOfData = true;
					flag2 = false;
				}
				else
				{
					nativeBuffer_RowBuffer.MoveFirst();
					if (1 == this._rowsToPrefetch)
					{
						for (int i = 0; i < num; i++)
						{
							this._columnInfo[i].Rebind(this._connection, ref array[i], ref array2[i]);
						}
					}
					int num3 = TracedNativeMethods.OCIStmtFetch(this._statementHandle, this.ErrorHandle, this._rowsToPrefetch, OCI.FETCH.OCI_FETCH_NEXT, OCI.MODE.OCI_DEFAULT);
					int rowsTotal = this._rowsTotal;
					this._statementHandle.GetAttribute(OCI.ATTR.OCI_ATTR_ROW_COUNT, out this._rowsTotal, this.ErrorHandle);
					if (num3 == 0)
					{
						flag2 = true;
					}
					else if (1 == num3)
					{
						this._connection.CheckError(this.ErrorHandle, num3);
						flag2 = true;
					}
					else if (100 == num3)
					{
						int num4 = this._rowsTotal - rowsTotal;
						if (num4 == 0)
						{
							if (this._rowsTotal == 0)
							{
								this._hasRows = 1;
							}
							this._endOfData = true;
							flag2 = false;
						}
						else
						{
							nativeBuffer_RowBuffer.NumberOfRows = num4;
							this._isLastBuffer = true;
							flag2 = true;
						}
					}
					else
					{
						this._endOfData = true;
						this._connection.CheckError(this.ErrorHandle, num3);
						flag2 = false;
					}
				}
			}
			finally
			{
				if (1 == this._rowsToPrefetch)
				{
					for (int i = 0; i < num; i++)
					{
						if (array[i])
						{
							array2[i].DangerousRelease();
						}
					}
				}
				if (flag)
				{
					nativeBuffer_RowBuffer.DangerousRelease();
				}
			}
			return flag2;
		}

		// Token: 0x04000407 RID: 1031
		private const int _prefetchMemory = 65536;

		// Token: 0x04000408 RID: 1032
		private const byte x_hasRows_Unknown = 0;

		// Token: 0x04000409 RID: 1033
		private const byte x_hasRows_False = 1;

		// Token: 0x0400040A RID: 1034
		private const byte x_hasRows_True = 2;

		// Token: 0x0400040B RID: 1035
		private OracleConnection _connection;

		// Token: 0x0400040C RID: 1036
		private int _connectionCloseCount;

		// Token: 0x0400040D RID: 1037
		private OciStatementHandle _statementHandle;

		// Token: 0x0400040E RID: 1038
		private string _statementText;

		// Token: 0x0400040F RID: 1039
		private CommandBehavior _commandBehavior;

		// Token: 0x04000410 RID: 1040
		private OracleColumn[] _columnInfo;

		// Token: 0x04000411 RID: 1041
		private NativeBuffer_RowBuffer _buffer;

		// Token: 0x04000412 RID: 1042
		private int _rowBufferLength;

		// Token: 0x04000413 RID: 1043
		private int _rowsToPrefetch;

		// Token: 0x04000414 RID: 1044
		private int _rowsTotal;

		// Token: 0x04000415 RID: 1045
		private bool _isLastBuffer;

		// Token: 0x04000416 RID: 1046
		private bool _endOfData;

		// Token: 0x04000417 RID: 1047
		private bool _closeConnectionToo;

		// Token: 0x04000418 RID: 1048
		private bool _keyInfoRequested;

		// Token: 0x04000419 RID: 1049
		private byte _hasRows;

		// Token: 0x0400041A RID: 1050
		private static int _objectTypeCount;

		// Token: 0x0400041B RID: 1051
		internal readonly int ObjectID = Interlocked.Increment(ref OracleDataReader._objectTypeCount);

		// Token: 0x0400041C RID: 1052
		private FieldNameLookup _fieldNameLookup;

		// Token: 0x0400041D RID: 1053
		private DataTable _schemaTable;

		// Token: 0x0400041E RID: 1054
		private int _recordsAffected;

		// Token: 0x0400041F RID: 1055
		private OracleDataReader[] _refCursorDataReaders;

		// Token: 0x04000420 RID: 1056
		private int _nextRefCursor;
	}
}
