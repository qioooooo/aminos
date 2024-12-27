using System;
using System.Collections;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Xml;

namespace System.Data.SqlClient
{
	// Token: 0x020002B4 RID: 692
	public sealed class SqlBulkCopy : IDisposable
	{
		// Token: 0x06002317 RID: 8983 RVA: 0x0026FA94 File Offset: 0x0026EE94
		public SqlBulkCopy(SqlConnection connection)
		{
			if (connection == null)
			{
				throw ADP.ArgumentNull("connection");
			}
			this._connection = connection;
			this._columnMappings = new SqlBulkCopyColumnMappingCollection();
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x0026FAE0 File Offset: 0x0026EEE0
		public SqlBulkCopy(SqlConnection connection, SqlBulkCopyOptions copyOptions, SqlTransaction externalTransaction)
			: this(connection)
		{
			this._copyOptions = copyOptions;
			if (externalTransaction != null && this.IsCopyOption(SqlBulkCopyOptions.UseInternalTransaction))
			{
				throw SQL.BulkLoadConflictingTransactionOption();
			}
			if (!this.IsCopyOption(SqlBulkCopyOptions.UseInternalTransaction))
			{
				this._externalTransaction = externalTransaction;
			}
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x0026FB20 File Offset: 0x0026EF20
		public SqlBulkCopy(string connectionString)
			: this(new SqlConnection(connectionString))
		{
			if (connectionString == null)
			{
				throw ADP.ArgumentNull("connectionString");
			}
			this._connection = new SqlConnection(connectionString);
			this._columnMappings = new SqlBulkCopyColumnMappingCollection();
			this._ownConnection = true;
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x0026FB68 File Offset: 0x0026EF68
		public SqlBulkCopy(string connectionString, SqlBulkCopyOptions copyOptions)
			: this(connectionString)
		{
			this._copyOptions = copyOptions;
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x0600231B RID: 8987 RVA: 0x0026FB84 File Offset: 0x0026EF84
		// (set) Token: 0x0600231C RID: 8988 RVA: 0x0026FB98 File Offset: 0x0026EF98
		public int BatchSize
		{
			get
			{
				return this._batchSize;
			}
			set
			{
				if (value >= 0)
				{
					this._batchSize = value;
					return;
				}
				throw ADP.ArgumentOutOfRange("BatchSize");
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x0600231D RID: 8989 RVA: 0x0026FBBC File Offset: 0x0026EFBC
		// (set) Token: 0x0600231E RID: 8990 RVA: 0x0026FBD0 File Offset: 0x0026EFD0
		public int BulkCopyTimeout
		{
			get
			{
				return this._timeout;
			}
			set
			{
				if (value < 0)
				{
					throw SQL.BulkLoadInvalidTimeout(value);
				}
				this._timeout = value;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x0600231F RID: 8991 RVA: 0x0026FBF0 File Offset: 0x0026EFF0
		public SqlBulkCopyColumnMappingCollection ColumnMappings
		{
			get
			{
				return this._columnMappings;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06002320 RID: 8992 RVA: 0x0026FC04 File Offset: 0x0026F004
		// (set) Token: 0x06002321 RID: 8993 RVA: 0x0026FC18 File Offset: 0x0026F018
		public string DestinationTableName
		{
			get
			{
				return this._destinationTableName;
			}
			set
			{
				if (value == null)
				{
					throw ADP.ArgumentNull("DestinationTableName");
				}
				if (value.Length == 0)
				{
					throw ADP.ArgumentOutOfRange("DestinationTableName");
				}
				this._destinationTableName = value;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06002322 RID: 8994 RVA: 0x0026FC50 File Offset: 0x0026F050
		// (set) Token: 0x06002323 RID: 8995 RVA: 0x0026FC64 File Offset: 0x0026F064
		public int NotifyAfter
		{
			get
			{
				return this._notifyAfter;
			}
			set
			{
				if (value >= 0)
				{
					this._notifyAfter = value;
					return;
				}
				throw ADP.ArgumentOutOfRange("NotifyAfter");
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06002324 RID: 8996 RVA: 0x0026FC88 File Offset: 0x0026F088
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06002325 RID: 8997 RVA: 0x0026FC9C File Offset: 0x0026F09C
		// (remove) Token: 0x06002326 RID: 8998 RVA: 0x0026FCC0 File Offset: 0x0026F0C0
		public event SqlRowsCopiedEventHandler SqlRowsCopied
		{
			add
			{
				this._rowsCopiedEventHandler = (SqlRowsCopiedEventHandler)Delegate.Combine(this._rowsCopiedEventHandler, value);
			}
			remove
			{
				this._rowsCopiedEventHandler = (SqlRowsCopiedEventHandler)Delegate.Remove(this._rowsCopiedEventHandler, value);
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06002327 RID: 8999 RVA: 0x0026FCE4 File Offset: 0x0026F0E4
		internal SqlStatistics Statistics
		{
			get
			{
				if (this._connection != null && this._connection.StatisticsEnabled)
				{
					return this._connection.Statistics;
				}
				return null;
			}
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x0026FD14 File Offset: 0x0026F114
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x0026FD30 File Offset: 0x0026F130
		private bool IsCopyOption(SqlBulkCopyOptions copyOption)
		{
			return (this._copyOptions & copyOption) == copyOption;
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x0026FD48 File Offset: 0x0026F148
		private BulkCopySimpleResultSet CreateAndExecuteInitialQuery()
		{
			string[] array;
			try
			{
				array = MultipartIdentifier.ParseMultipartIdentifier(this.DestinationTableName, "[\"", "]\"", "SQL_BulkCopyDestinationTableName", true);
			}
			catch (Exception ex)
			{
				throw SQL.BulkLoadInvalidDestinationTable(this.DestinationTableName, ex);
			}
			if (ADP.IsEmpty(array[3]))
			{
				throw SQL.BulkLoadInvalidDestinationTable(this.DestinationTableName, null);
			}
			BulkCopySimpleResultSet bulkCopySimpleResultSet = new BulkCopySimpleResultSet();
			string text = "select @@trancount; SET FMTONLY ON select * from " + this.DestinationTableName + " SET FMTONLY OFF ";
			if (this._connection.IsShiloh)
			{
				string text2;
				if (this._connection.IsKatmaiOrNewer)
				{
					text2 = "sp_tablecollations_100";
				}
				else if (this._connection.IsYukonOrNewer)
				{
					text2 = "sp_tablecollations_90";
				}
				else
				{
					text2 = "sp_tablecollations";
				}
				string text3 = array[3].Replace("'", "''");
				string text4 = array[2];
				if (text4 != null)
				{
					text4 = text4.Replace("'", "''");
				}
				string text5 = array[1];
				if (text3.Length > 0 && '#' == text3[0] && ADP.IsEmpty(text5))
				{
					text += string.Format(null, "exec tempdb..{0} N'{1}.{2}'", new object[] { text2, text4, text3 });
				}
				else
				{
					text += string.Format(null, "exec {0}..{1} N'{2}.{3}'", new object[] { text5, text2, text4, text3 });
				}
			}
			Bid.Trace("<sc.SqlBulkCopy.CreateAndExecuteInitialQuery|INFO> Initial Query: '%ls' \n", text);
			this._parser.TdsExecuteSQLBatch(text, this.BulkCopyTimeout, null, this._stateObj);
			this._parser.Run(RunBehavior.UntilDone, null, null, bulkCopySimpleResultSet, this._stateObj);
			return bulkCopySimpleResultSet;
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x0026FF04 File Offset: 0x0026F304
		private string AnalyzeTargetAndCreateUpdateBulkCommand(BulkCopySimpleResultSet internalResults)
		{
			this._sortedColumnMappings = new ArrayList();
			StringBuilder stringBuilder = new StringBuilder();
			if (this._connection.IsShiloh && internalResults[2].Count == 0)
			{
				throw SQL.BulkLoadNoCollation();
			}
			stringBuilder.Append("insert bulk " + this.DestinationTableName + " (");
			int num = 0;
			int num2 = 0;
			bool flag;
			if (this._parser.IsYukonOrNewer)
			{
				flag = this._connection.HasLocalTransaction;
			}
			else
			{
				flag = (bool)(0 < (SqlInt32)internalResults[0][0][0]);
			}
			if (flag && this._externalTransaction == null && this._internalTransaction == null && this._connection.Parser != null && this._connection.Parser.CurrentTransaction != null && this._connection.Parser.CurrentTransaction.IsLocal)
			{
				throw SQL.BulkLoadExistingTransaction();
			}
			for (int i = 0; i < internalResults[1].MetaData.Length; i++)
			{
				_SqlMetaData sqlMetaData = internalResults[1].MetaData[i];
				bool flag2 = false;
				if (sqlMetaData.type == SqlDbType.Timestamp || (sqlMetaData.isIdentity && !this.IsCopyOption(SqlBulkCopyOptions.KeepIdentity)))
				{
					internalResults[1].MetaData[i] = null;
					flag2 = true;
				}
				int j = 0;
				while (j < this._localColumnMappings.Count)
				{
					if (this._localColumnMappings[j]._destinationColumnOrdinal == sqlMetaData.ordinal || this.UnquotedName(this._localColumnMappings[j]._destinationColumnName) == sqlMetaData.column)
					{
						if (flag2)
						{
							num2++;
							break;
						}
						this._sortedColumnMappings.Add(new _ColumnMapping(this._localColumnMappings[j]._internalSourceColumnOrdinal, sqlMetaData));
						num++;
						if (num > 1)
						{
							stringBuilder.Append(", ");
						}
						if (sqlMetaData.type == SqlDbType.Variant)
						{
							this.AppendColumnNameAndTypeName(stringBuilder, sqlMetaData.column, "sql_variant");
						}
						else if (sqlMetaData.type == SqlDbType.Udt)
						{
							this.AppendColumnNameAndTypeName(stringBuilder, sqlMetaData.column, "varbinary");
						}
						else
						{
							this.AppendColumnNameAndTypeName(stringBuilder, sqlMetaData.column, sqlMetaData.type.ToString());
						}
						byte nullableType = sqlMetaData.metaType.NullableType;
						switch (nullableType)
						{
						case 41:
						case 42:
						case 43:
							stringBuilder.Append("(" + sqlMetaData.scale.ToString(null) + ")");
							break;
						default:
							switch (nullableType)
							{
							case 106:
							case 108:
								stringBuilder.Append(string.Concat(new string[]
								{
									"(",
									sqlMetaData.precision.ToString(null),
									",",
									sqlMetaData.scale.ToString(null),
									")"
								}));
								goto IL_03BE;
							case 107:
								break;
							default:
								if (nullableType == 240)
								{
									if (sqlMetaData.IsLargeUdt)
									{
										stringBuilder.Append("(max)");
										goto IL_03BE;
									}
									int length = sqlMetaData.length;
									stringBuilder.Append("(" + length.ToString(null) + ")");
									goto IL_03BE;
								}
								break;
							}
							if (!sqlMetaData.metaType.IsFixed && !sqlMetaData.metaType.IsLong)
							{
								int num3 = sqlMetaData.length;
								byte nullableType2 = sqlMetaData.metaType.NullableType;
								if (nullableType2 == 99 || nullableType2 == 231 || nullableType2 == 239)
								{
									num3 /= 2;
								}
								stringBuilder.Append("(" + num3.ToString(null) + ")");
							}
							else if (sqlMetaData.metaType.IsPlp && sqlMetaData.metaType.SqlDbType != SqlDbType.Xml)
							{
								stringBuilder.Append("(max)");
							}
							break;
						}
						IL_03BE:
						if (!this._connection.IsShiloh)
						{
							break;
						}
						Result result = internalResults[2];
						object obj = result[i][3];
						if (obj == null)
						{
							break;
						}
						SqlString sqlString = (SqlString)obj;
						if (sqlString.IsNull)
						{
							break;
						}
						stringBuilder.Append(" COLLATE " + sqlString.ToString());
						if (this._SqlDataReaderRowSource == null)
						{
							break;
						}
						int internalSourceColumnOrdinal = this._localColumnMappings[j]._internalSourceColumnOrdinal;
						int lcid = internalResults[1].MetaData[i].collation.LCID;
						int localeId = this._SqlDataReaderRowSource.GetLocaleId(internalSourceColumnOrdinal);
						if (localeId != lcid)
						{
							throw SQL.BulkLoadLcidMismatch(localeId, this._SqlDataReaderRowSource.GetName(internalSourceColumnOrdinal), lcid, sqlMetaData.column);
						}
						break;
					}
					else
					{
						j++;
					}
				}
				if (j == this._localColumnMappings.Count)
				{
					internalResults[1].MetaData[i] = null;
				}
			}
			if (num + num2 != this._localColumnMappings.Count)
			{
				throw SQL.BulkLoadNonMatchingColumnMapping();
			}
			stringBuilder.Append(")");
			if ((this._copyOptions & (SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.KeepNulls | SqlBulkCopyOptions.FireTriggers)) != SqlBulkCopyOptions.Default)
			{
				bool flag3 = false;
				stringBuilder.Append(" with (");
				if (this.IsCopyOption(SqlBulkCopyOptions.KeepNulls))
				{
					stringBuilder.Append("KEEP_NULLS");
					flag3 = true;
				}
				if (this.IsCopyOption(SqlBulkCopyOptions.TableLock))
				{
					stringBuilder.Append((flag3 ? ", " : "") + "TABLOCK");
					flag3 = true;
				}
				if (this.IsCopyOption(SqlBulkCopyOptions.CheckConstraints))
				{
					stringBuilder.Append((flag3 ? ", " : "") + "CHECK_CONSTRAINTS");
					flag3 = true;
				}
				if (this.IsCopyOption(SqlBulkCopyOptions.FireTriggers))
				{
					stringBuilder.Append((flag3 ? ", " : "") + "FIRE_TRIGGERS");
				}
				stringBuilder.Append(")");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x002704E4 File Offset: 0x0026F8E4
		private void SubmitUpdateBulkCommand(BulkCopySimpleResultSet internalResults, string TDSCommand)
		{
			this._parser.TdsExecuteSQLBatch(TDSCommand, this.BulkCopyTimeout, null, this._stateObj);
			this._parser.Run(RunBehavior.UntilDone, null, null, null, this._stateObj);
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x00270520 File Offset: 0x0026F920
		private void WriteMetaData(BulkCopySimpleResultSet internalResults)
		{
			this._stateObj.SetTimeoutSeconds(this.BulkCopyTimeout);
			_SqlMetaDataSet metaData = internalResults[1].MetaData;
			this._stateObj._outputMessageType = 7;
			this._parser.WriteBulkCopyMetaData(metaData, this._sortedColumnMappings.Count, this._stateObj);
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x00270574 File Offset: 0x0026F974
		public void Close()
		{
			if (this._insideRowsCopiedEvent)
			{
				throw SQL.InvalidOperationInsideEvent();
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x0027059C File Offset: 0x0026F99C
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._columnMappings = null;
				this._parser = null;
				try
				{
					if (this._internalTransaction != null)
					{
						this._internalTransaction.Rollback();
						this._internalTransaction.Dispose();
						this._internalTransaction = null;
					}
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableExceptionType(ex))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(ex);
				}
				finally
				{
					if (this._connection != null)
					{
						if (this._ownConnection)
						{
							this._connection.Dispose();
						}
						this._connection = null;
					}
				}
			}
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x00270650 File Offset: 0x0026FA50
		private object GetValueFromSourceRow(int columnOrdinal, _SqlMetaData metadata, int[] UseSqlValue, int destRowIndex)
		{
			if (UseSqlValue[destRowIndex] == 0)
			{
				UseSqlValue[destRowIndex] = -1;
				if (metadata.metaType.NullableType == 106 || metadata.metaType.NullableType == 108)
				{
					Type type = null;
					switch (this._rowSourceType)
					{
					case SqlBulkCopy.ValueSourceType.IDataReader:
						if (this._SqlDataReaderRowSource != null)
						{
							type = this._SqlDataReaderRowSource.GetFieldType(columnOrdinal);
						}
						break;
					case SqlBulkCopy.ValueSourceType.DataTable:
					case SqlBulkCopy.ValueSourceType.RowArray:
						type = this._currentRow.Table.Columns[columnOrdinal].DataType;
						break;
					}
					if (typeof(SqlDecimal) == type || typeof(decimal) == type)
					{
						UseSqlValue[destRowIndex] = 4;
					}
					else if (typeof(SqlDouble) == type || typeof(double) == type)
					{
						UseSqlValue[destRowIndex] = 5;
					}
					else if (typeof(SqlSingle) == type || typeof(float) == type)
					{
						UseSqlValue[destRowIndex] = 10;
					}
				}
			}
			switch (this._rowSourceType)
			{
			case SqlBulkCopy.ValueSourceType.IDataReader:
			{
				if (this._SqlDataReaderRowSource == null)
				{
					return ((IDataReader)this._rowSource).GetValue(columnOrdinal);
				}
				int num = UseSqlValue[destRowIndex];
				switch (num)
				{
				case 4:
					return this._SqlDataReaderRowSource.GetSqlDecimal(columnOrdinal);
				case 5:
					return new SqlDecimal(this._SqlDataReaderRowSource.GetSqlDouble(columnOrdinal).Value);
				default:
					if (num != 10)
					{
						return this._SqlDataReaderRowSource.GetValue(columnOrdinal);
					}
					return new SqlDecimal((double)this._SqlDataReaderRowSource.GetSqlSingle(columnOrdinal).Value);
				}
				break;
			}
			case SqlBulkCopy.ValueSourceType.DataTable:
			case SqlBulkCopy.ValueSourceType.RowArray:
			{
				object obj = this._currentRow[columnOrdinal];
				if (obj != null && DBNull.Value != obj && (10 == UseSqlValue[destRowIndex] || 5 == UseSqlValue[destRowIndex] || 4 == UseSqlValue[destRowIndex]))
				{
					INullable nullable = obj as INullable;
					if (nullable == null || !nullable.IsNull)
					{
						SqlBuffer.StorageType storageType = (SqlBuffer.StorageType)UseSqlValue[destRowIndex];
						switch (storageType)
						{
						case SqlBuffer.StorageType.Decimal:
							if (nullable != null)
							{
								return (SqlDecimal)obj;
							}
							return new SqlDecimal((decimal)obj);
						case SqlBuffer.StorageType.Double:
						{
							if (nullable != null)
							{
								return new SqlDecimal(((SqlDouble)obj).Value);
							}
							double num2 = (double)obj;
							if (!double.IsNaN(num2))
							{
								return new SqlDecimal(num2);
							}
							break;
						}
						default:
							if (storageType == SqlBuffer.StorageType.Single)
							{
								if (nullable != null)
								{
									return new SqlDecimal((double)((SqlSingle)obj).Value);
								}
								float num3 = (float)obj;
								if (!float.IsNaN(num3))
								{
									return new SqlDecimal((double)num3);
								}
							}
							break;
						}
					}
				}
				return obj;
			}
			default:
				throw ADP.NotSupported();
			}
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x00270908 File Offset: 0x0026FD08
		private bool ReadFromRowSource()
		{
			switch (this._rowSourceType)
			{
			case SqlBulkCopy.ValueSourceType.IDataReader:
				return ((IDataReader)this._rowSource).Read();
			case SqlBulkCopy.ValueSourceType.DataTable:
			case SqlBulkCopy.ValueSourceType.RowArray:
				while (this._rowEnumerator.MoveNext())
				{
					this._currentRow = (DataRow)this._rowEnumerator.Current;
					if ((this._currentRow.RowState & DataRowState.Deleted) == (DataRowState)0 && (this._rowState == (DataRowState)0 || (this._currentRow.RowState & this._rowState) != (DataRowState)0))
					{
						this._currentRowLength = this._currentRow.ItemArray.Length;
						return true;
					}
				}
				return false;
			default:
				throw ADP.NotSupported();
			}
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x002709B0 File Offset: 0x0026FDB0
		private void CreateOrValidateConnection(string method)
		{
			if (this._connection == null)
			{
				throw ADP.ConnectionRequired(method);
			}
			if (this._connection.IsContextConnection)
			{
				throw SQL.NotAvailableOnContextConnection();
			}
			if (this._ownConnection && this._connection.State != ConnectionState.Open)
			{
				this._connection.Open();
			}
			this._connection.ValidateConnectionForExecute(method, null);
			if (this._externalTransaction != null && this._connection != this._externalTransaction.Connection)
			{
				throw ADP.TransactionConnectionMismatch();
			}
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x00270A30 File Offset: 0x0026FE30
		private void AppendColumnNameAndTypeName(StringBuilder query, string columnName, string typeName)
		{
			query.Append('[');
			query.Append(columnName.Replace("]", "]]"));
			query.Append("] ");
			query.Append(typeName);
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x00270A74 File Offset: 0x0026FE74
		private string UnquotedName(string name)
		{
			if (ADP.IsEmpty(name))
			{
				return null;
			}
			if (name[0] == '[')
			{
				int length = name.Length;
				name = name.Substring(1, length - 2);
			}
			return name;
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x00270AAC File Offset: 0x0026FEAC
		private object ValidateBulkCopyVariant(object value)
		{
			MetaType metaTypeFromValue = MetaType.GetMetaTypeFromValue(value);
			byte tdstype = metaTypeFromValue.TDSType;
			if (tdstype <= 108)
			{
				switch (tdstype)
				{
				case 36:
				case 40:
				case 41:
				case 42:
				case 43:
					break;
				case 37:
				case 38:
				case 39:
					goto IL_00C1;
				default:
					switch (tdstype)
					{
					case 48:
					case 50:
					case 52:
					case 56:
					case 59:
					case 60:
					case 61:
					case 62:
						break;
					case 49:
					case 51:
					case 53:
					case 54:
					case 55:
					case 57:
					case 58:
						goto IL_00C1;
					default:
						if (tdstype != 108)
						{
							goto IL_00C1;
						}
						break;
					}
					break;
				}
			}
			else if (tdstype != 127)
			{
				switch (tdstype)
				{
				case 165:
				case 167:
					break;
				case 166:
					goto IL_00C1;
				default:
					if (tdstype != 231)
					{
						goto IL_00C1;
					}
					break;
				}
			}
			if (value is INullable)
			{
				return MetaType.GetComValueFromSqlVariant(value);
			}
			return value;
			IL_00C1:
			throw SQL.BulkLoadInvalidVariantValue();
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x00270B80 File Offset: 0x0026FF80
		private object ConvertValue(object value, _SqlMetaData metadata)
		{
			if (!ADP.IsNull(value))
			{
				MetaType metaType = metadata.metaType;
				object obj;
				try
				{
					byte nullableType = metaType.NullableType;
					MetaType metaType2;
					if (nullableType <= 111)
					{
						switch (nullableType)
						{
						case 34:
						case 35:
						case 36:
						case 38:
						case 40:
						case 41:
						case 42:
						case 43:
						case 50:
							break;
						case 37:
						case 39:
						case 44:
						case 45:
						case 46:
						case 47:
						case 48:
						case 49:
							goto IL_0278;
						default:
							switch (nullableType)
							{
							case 58:
							case 59:
							case 61:
							case 62:
								break;
							case 60:
								goto IL_0278;
							default:
								switch (nullableType)
								{
								case 98:
									value = this.ValidateBulkCopyVariant(value);
									goto IL_028B;
								case 99:
									goto IL_01E1;
								case 100:
								case 101:
								case 102:
								case 103:
								case 105:
								case 107:
									goto IL_0278;
								case 104:
								case 109:
								case 110:
								case 111:
									break;
								case 106:
								case 108:
								{
									metaType2 = MetaType.GetMetaTypeFromSqlDbType(metaType.SqlDbType, false);
									value = SqlParameter.CoerceValue(value, metaType2);
									SqlDecimal sqlDecimal;
									if (value is SqlDecimal)
									{
										sqlDecimal = (SqlDecimal)value;
									}
									else
									{
										sqlDecimal = new SqlDecimal((decimal)value);
									}
									if (sqlDecimal.Scale != metadata.scale)
									{
										sqlDecimal = TdsParser.AdjustSqlDecimalScale(sqlDecimal, (int)metadata.scale);
										value = sqlDecimal;
									}
									if (sqlDecimal.Precision > metadata.precision)
									{
										throw SQL.BulkLoadCannotConvertValue(value.GetType(), metaType2, ADP.ParameterValueOutOfRange(sqlDecimal));
									}
									goto IL_028B;
								}
								default:
									goto IL_0278;
								}
								break;
							}
							break;
						}
					}
					else if (nullableType <= 175)
					{
						switch (nullableType)
						{
						case 165:
						case 167:
							break;
						case 166:
							goto IL_0278;
						default:
							switch (nullableType)
							{
							case 173:
							case 175:
								break;
							case 174:
								goto IL_0278;
							default:
								goto IL_0278;
							}
							break;
						}
					}
					else
					{
						if (nullableType == 231)
						{
							goto IL_01E1;
						}
						switch (nullableType)
						{
						case 239:
							goto IL_01E1;
						case 240:
							if (value.GetType() != typeof(byte[]))
							{
								value = this._connection.GetBytes(value);
								goto IL_028B;
							}
							goto IL_028B;
						case 241:
							if (value is XmlReader)
							{
								value = MetaType.GetStringFromXml((XmlReader)value);
								goto IL_028B;
							}
							goto IL_028B;
						default:
							goto IL_0278;
						}
					}
					metaType2 = MetaType.GetMetaTypeFromSqlDbType(metaType.SqlDbType, false);
					value = SqlParameter.CoerceValue(value, metaType2);
					goto IL_028B;
					IL_01E1:
					metaType2 = MetaType.GetMetaTypeFromSqlDbType(metaType.SqlDbType, false);
					value = SqlParameter.CoerceValue(value, metaType2);
					int num = ((value is string) ? ((string)value).Length : ((SqlString)value).Value.Length);
					if (num > metadata.length / 2)
					{
						throw SQL.BulkLoadStringTooLong();
					}
					goto IL_028B;
					IL_0278:
					throw SQL.BulkLoadCannotConvertValue(value.GetType(), metadata.metaType, null);
					IL_028B:
					obj = value;
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableExceptionType(ex))
					{
						throw;
					}
					throw SQL.BulkLoadCannotConvertValue(value.GetType(), metadata.metaType, ex);
				}
				return obj;
			}
			if (!metadata.isNullable)
			{
				throw SQL.BulkLoadBulkLoadNotAllowDBNull(metadata.column);
			}
			return value;
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x00270E60 File Offset: 0x00270260
		public void WriteToServer(IDataReader reader)
		{
			SqlConnection.ExecutePermission.Demand();
			SqlStatistics sqlStatistics = this.Statistics;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				if (reader == null)
				{
					throw new ArgumentNullException("reader");
				}
				this._rowSource = reader;
				this._SqlDataReaderRowSource = this._rowSource as SqlDataReader;
				this._rowSourceType = SqlBulkCopy.ValueSourceType.IDataReader;
				this.WriteRowSourceToServer(reader.FieldCount);
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x00270EE8 File Offset: 0x002702E8
		public void WriteToServer(DataTable table)
		{
			this.WriteToServer(table, (DataRowState)0);
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x00270F00 File Offset: 0x00270300
		public void WriteToServer(DataTable table, DataRowState rowState)
		{
			SqlConnection.ExecutePermission.Demand();
			SqlStatistics sqlStatistics = this.Statistics;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				if (table == null)
				{
					throw new ArgumentNullException("table");
				}
				this._rowState = rowState & ~DataRowState.Deleted;
				this._rowSource = table;
				this._SqlDataReaderRowSource = null;
				this._rowSourceType = SqlBulkCopy.ValueSourceType.DataTable;
				this._rowEnumerator = table.Rows.GetEnumerator();
				this.WriteRowSourceToServer(table.Columns.Count);
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x00270FA0 File Offset: 0x002703A0
		public void WriteToServer(DataRow[] rows)
		{
			SqlConnection.ExecutePermission.Demand();
			SqlStatistics sqlStatistics = this.Statistics;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				if (rows == null)
				{
					throw new ArgumentNullException("rows");
				}
				if (rows.Length != 0)
				{
					DataTable table = rows[0].Table;
					this._rowState = (DataRowState)0;
					this._rowSource = rows;
					this._SqlDataReaderRowSource = null;
					this._rowSourceType = SqlBulkCopy.ValueSourceType.RowArray;
					this._rowEnumerator = rows.GetEnumerator();
					this.WriteRowSourceToServer(table.Columns.Count);
				}
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x00271048 File Offset: 0x00270448
		private void WriteRowSourceToServer(int columnCount)
		{
			this.CreateOrValidateConnection("WriteToServer");
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			SNIHandle snihandle = null;
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._connection);
				this._columnMappings.ReadOnly = true;
				this._localColumnMappings = this._columnMappings;
				if (this._localColumnMappings.Count > 0)
				{
					this._localColumnMappings.ValidateCollection();
					using (IEnumerator enumerator = this._localColumnMappings.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							SqlBulkCopyColumnMapping sqlBulkCopyColumnMapping = (SqlBulkCopyColumnMapping)obj;
							if (sqlBulkCopyColumnMapping._internalSourceColumnOrdinal == -1)
							{
								flag = true;
								break;
							}
						}
						goto IL_00B7;
					}
				}
				this._localColumnMappings = new SqlBulkCopyColumnMappingCollection();
				this._localColumnMappings.CreateDefaultMapping(columnCount);
				IL_00B7:
				if (flag)
				{
					int num = -1;
					flag = false;
					if (this._localColumnMappings.Count > 0)
					{
						foreach (object obj2 in this._localColumnMappings)
						{
							SqlBulkCopyColumnMapping sqlBulkCopyColumnMapping2 = (SqlBulkCopyColumnMapping)obj2;
							if (sqlBulkCopyColumnMapping2._internalSourceColumnOrdinal == -1)
							{
								string text = this.UnquotedName(sqlBulkCopyColumnMapping2.SourceColumn);
								switch (this._rowSourceType)
								{
								case SqlBulkCopy.ValueSourceType.IDataReader:
									try
									{
										num = ((IDataRecord)this._rowSource).GetOrdinal(text);
									}
									catch (IndexOutOfRangeException ex)
									{
										throw SQL.BulkLoadNonMatchingColumnName(text, ex);
									}
									break;
								case SqlBulkCopy.ValueSourceType.DataTable:
									num = ((DataTable)this._rowSource).Columns.IndexOf(text);
									break;
								case SqlBulkCopy.ValueSourceType.RowArray:
									num = ((DataRow[])this._rowSource)[0].Table.Columns.IndexOf(text);
									break;
								}
								if (num == -1)
								{
									throw SQL.BulkLoadNonMatchingColumnName(text);
								}
								sqlBulkCopyColumnMapping2._internalSourceColumnOrdinal = num;
							}
						}
					}
				}
				this.WriteToServerInternal();
			}
			catch (OutOfMemoryException ex2)
			{
				this._connection.Abort(ex2);
				throw;
			}
			catch (StackOverflowException ex3)
			{
				this._connection.Abort(ex3);
				throw;
			}
			catch (ThreadAbortException ex4)
			{
				this._connection.Abort(ex4);
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
			finally
			{
				this._columnMappings.ReadOnly = false;
			}
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x00271310 File Offset: 0x00270710
		private void WriteToServerInternal()
		{
			string text = null;
			bool flag = false;
			bool flag2 = false;
			int[] array = null;
			int batchSize = this._batchSize;
			bool flag3 = false;
			if (this._batchSize > 0)
			{
				flag3 = true;
			}
			Exception ex = null;
			this._rowsCopied = 0;
			if (this._destinationTableName == null)
			{
				throw SQL.BulkLoadMissingDestinationTable();
			}
			if (!this.ReadFromRowSource())
			{
				return;
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				bool flag4 = true;
				this._parser = this._connection.Parser;
				this._stateObj = this._parser.GetSession(this);
				this._stateObj._bulkCopyOpperationInProgress = true;
				try
				{
					this._stateObj.StartSession(this.ObjectID);
					BulkCopySimpleResultSet bulkCopySimpleResultSet;
					try
					{
						bulkCopySimpleResultSet = this.CreateAndExecuteInitialQuery();
					}
					catch (SqlException ex2)
					{
						throw SQL.BulkLoadInvalidDestinationTable(this._destinationTableName, ex2);
					}
					this._rowsUntilNotification = this._notifyAfter;
					text = this.AnalyzeTargetAndCreateUpdateBulkCommand(bulkCopySimpleResultSet);
					if (this._sortedColumnMappings.Count != 0)
					{
						this._stateObj.SniContext = SniContext.Snix_SendRows;
						for (;;)
						{
							if (this.IsCopyOption(SqlBulkCopyOptions.UseInternalTransaction))
							{
								this._internalTransaction = this._connection.BeginTransaction();
							}
							this.SubmitUpdateBulkCommand(bulkCopySimpleResultSet, text);
							try
							{
								this.WriteMetaData(bulkCopySimpleResultSet);
								object[] array2 = new object[this._sortedColumnMappings.Count];
								if (array == null)
								{
									array = new int[array2.Length];
								}
								int num = batchSize;
								do
								{
									for (int i = 0; i < array2.Length; i++)
									{
										_ColumnMapping columnMapping = (_ColumnMapping)this._sortedColumnMappings[i];
										_SqlMetaData metadata = columnMapping._metadata;
										object valueFromSourceRow = this.GetValueFromSourceRow(columnMapping._sourceColumnOrdinal, metadata, array, i);
										array2[i] = this.ConvertValue(valueFromSourceRow, metadata);
									}
									this._parser.WriteByte(209, this._stateObj);
									for (int j = 0; j < array2.Length; j++)
									{
										_ColumnMapping columnMapping2 = (_ColumnMapping)this._sortedColumnMappings[j];
										_SqlMetaData metadata2 = columnMapping2._metadata;
										if (metadata2.type != SqlDbType.Variant)
										{
											this._parser.WriteBulkCopyValue(array2[j], metadata2, this._stateObj);
										}
										else
										{
											this._parser.WriteSqlVariantDataRowValue(array2[j], this._stateObj);
										}
									}
									this._rowsCopied++;
									if (this._notifyAfter > 0 && this._rowsUntilNotification > 0 && --this._rowsUntilNotification == 0)
									{
										try
										{
											this._stateObj.BcpLock = true;
											flag2 = this.FireRowsCopiedEvent((long)this._rowsCopied);
											Bid.Trace("<sc.SqlBulkCopy.WriteToServerInternal|INFO> \n");
											if (ConnectionState.Open != this._connection.State)
											{
												break;
											}
										}
										catch (Exception ex3)
										{
											if (!ADP.IsCatchableExceptionType(ex3))
											{
												throw;
											}
											ex = OperationAbortedException.Aborted(ex3);
											break;
										}
										finally
										{
											this._stateObj.BcpLock = false;
										}
										if (flag2)
										{
											break;
										}
										this._rowsUntilNotification = this._notifyAfter;
									}
									if (this._rowsUntilNotification > this._notifyAfter)
									{
										this._rowsUntilNotification = this._notifyAfter;
									}
									flag = this.ReadFromRowSource();
									if (flag3)
									{
										num--;
										if (num == 0)
										{
											break;
										}
									}
								}
								while (flag);
							}
							catch (Exception ex4)
							{
								if (ADP.IsCatchableExceptionType(ex4))
								{
									this._stateObj.CancelRequest();
								}
								throw;
							}
							if (ConnectionState.Open != this._connection.State)
							{
								break;
							}
							this._parser.WriteBulkCopyDone(this._stateObj);
							this._parser.Run(RunBehavior.UntilDone, null, null, null, this._stateObj);
							if (flag2 || ex != null)
							{
								goto IL_033A;
							}
							if (this._internalTransaction != null)
							{
								this._internalTransaction.Commit();
								this._internalTransaction = null;
							}
							if (!flag)
							{
								goto Block_16;
							}
						}
						throw ADP.OpenConnectionRequired("WriteToServer", this._connection.State);
						IL_033A:
						throw OperationAbortedException.Aborted(ex);
						Block_16:
						this._localColumnMappings = null;
					}
				}
				catch (Exception ex5)
				{
					flag4 = ADP.IsCatchableExceptionType(ex5);
					if (flag4)
					{
						this._stateObj._internalTimeout = false;
						if (this._internalTransaction != null)
						{
							if (!this._internalTransaction.IsZombied)
							{
								this._internalTransaction.Rollback();
							}
							this._internalTransaction = null;
						}
					}
					throw;
				}
				finally
				{
					if (flag4 && this._stateObj != null)
					{
						this._stateObj.CloseSession();
					}
				}
			}
			finally
			{
				if (this._stateObj != null)
				{
					this._stateObj._bulkCopyOpperationInProgress = false;
					this._stateObj = null;
				}
			}
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x002717B4 File Offset: 0x00270BB4
		private void OnRowsCopied(SqlRowsCopiedEventArgs value)
		{
			SqlRowsCopiedEventHandler rowsCopiedEventHandler = this._rowsCopiedEventHandler;
			if (rowsCopiedEventHandler != null)
			{
				rowsCopiedEventHandler(this, value);
			}
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x002717D4 File Offset: 0x00270BD4
		private bool FireRowsCopiedEvent(long rowsCopied)
		{
			SqlRowsCopiedEventArgs sqlRowsCopiedEventArgs = new SqlRowsCopiedEventArgs(rowsCopied);
			try
			{
				this._insideRowsCopiedEvent = true;
				this.OnRowsCopied(sqlRowsCopiedEventArgs);
			}
			finally
			{
				this._insideRowsCopiedEvent = false;
			}
			return sqlRowsCopiedEventArgs.Abort;
		}

		// Token: 0x040016BA RID: 5818
		private const int TranCountResultId = 0;

		// Token: 0x040016BB RID: 5819
		private const int TranCountRowId = 0;

		// Token: 0x040016BC RID: 5820
		private const int TranCountValueId = 0;

		// Token: 0x040016BD RID: 5821
		private const int MetaDataResultId = 1;

		// Token: 0x040016BE RID: 5822
		private const int CollationResultId = 2;

		// Token: 0x040016BF RID: 5823
		private const int ColIdId = 0;

		// Token: 0x040016C0 RID: 5824
		private const int NameId = 1;

		// Token: 0x040016C1 RID: 5825
		private const int Tds_CollationId = 2;

		// Token: 0x040016C2 RID: 5826
		private const int CollationId = 3;

		// Token: 0x040016C3 RID: 5827
		private const int DefaultCommandTimeout = 30;

		// Token: 0x040016C4 RID: 5828
		private int _batchSize;

		// Token: 0x040016C5 RID: 5829
		private bool _ownConnection;

		// Token: 0x040016C6 RID: 5830
		private SqlBulkCopyOptions _copyOptions;

		// Token: 0x040016C7 RID: 5831
		private int _timeout = 30;

		// Token: 0x040016C8 RID: 5832
		private string _destinationTableName;

		// Token: 0x040016C9 RID: 5833
		private int _rowsCopied;

		// Token: 0x040016CA RID: 5834
		private int _notifyAfter;

		// Token: 0x040016CB RID: 5835
		private int _rowsUntilNotification;

		// Token: 0x040016CC RID: 5836
		private bool _insideRowsCopiedEvent;

		// Token: 0x040016CD RID: 5837
		private object _rowSource;

		// Token: 0x040016CE RID: 5838
		private SqlDataReader _SqlDataReaderRowSource;

		// Token: 0x040016CF RID: 5839
		private SqlBulkCopyColumnMappingCollection _columnMappings;

		// Token: 0x040016D0 RID: 5840
		private SqlBulkCopyColumnMappingCollection _localColumnMappings;

		// Token: 0x040016D1 RID: 5841
		private SqlConnection _connection;

		// Token: 0x040016D2 RID: 5842
		private SqlTransaction _internalTransaction;

		// Token: 0x040016D3 RID: 5843
		private SqlTransaction _externalTransaction;

		// Token: 0x040016D4 RID: 5844
		private SqlBulkCopy.ValueSourceType _rowSourceType;

		// Token: 0x040016D5 RID: 5845
		private DataRow _currentRow;

		// Token: 0x040016D6 RID: 5846
		private int _currentRowLength;

		// Token: 0x040016D7 RID: 5847
		private DataRowState _rowState;

		// Token: 0x040016D8 RID: 5848
		private IEnumerator _rowEnumerator;

		// Token: 0x040016D9 RID: 5849
		private TdsParser _parser;

		// Token: 0x040016DA RID: 5850
		private TdsParserStateObject _stateObj;

		// Token: 0x040016DB RID: 5851
		private ArrayList _sortedColumnMappings;

		// Token: 0x040016DC RID: 5852
		private SqlRowsCopiedEventHandler _rowsCopiedEventHandler;

		// Token: 0x040016DD RID: 5853
		private static int _objectTypeCount;

		// Token: 0x040016DE RID: 5854
		internal readonly int _objectID = Interlocked.Increment(ref SqlBulkCopy._objectTypeCount);

		// Token: 0x020002B5 RID: 693
		private enum ValueSourceType
		{
			// Token: 0x040016E0 RID: 5856
			Unspecified,
			// Token: 0x040016E1 RID: 5857
			IDataReader,
			// Token: 0x040016E2 RID: 5858
			DataTable,
			// Token: 0x040016E3 RID: 5859
			RowArray
		}
	}
}
