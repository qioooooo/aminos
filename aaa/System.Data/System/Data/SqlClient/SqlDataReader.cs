using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Data.SqlTypes;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x020002DE RID: 734
	public class SqlDataReader : DbDataReader, IDataReader, IDisposable, IDataRecord
	{
		// Token: 0x060025D6 RID: 9686 RVA: 0x0027D848 File Offset: 0x0027CC48
		internal SqlDataReader(SqlCommand command, CommandBehavior behavior)
		{
			this._command = command;
			this._commandBehavior = behavior;
			if (this._command != null)
			{
				this._timeoutSeconds = command.CommandTimeout;
				this._connection = command.Connection;
				if (this._connection != null)
				{
					this._statistics = this._connection.Statistics;
					this._typeSystem = this._connection.TypeSystem;
				}
			}
			this._dataReady = false;
			this._metaDataConsumed = false;
			this._hasRows = false;
			this._browseModeInfoConsumed = false;
		}

		// Token: 0x170005F4 RID: 1524
		// (set) Token: 0x060025D7 RID: 9687 RVA: 0x0027D8E8 File Offset: 0x0027CCE8
		internal bool BrowseModeInfoConsumed
		{
			set
			{
				this._browseModeInfoConsumed = value;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x060025D8 RID: 9688 RVA: 0x0027D8FC File Offset: 0x0027CCFC
		internal SqlCommand Command
		{
			get
			{
				return this._command;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x060025D9 RID: 9689 RVA: 0x0027D910 File Offset: 0x0027CD10
		protected SqlConnection Connection
		{
			get
			{
				return this._connection;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x060025DA RID: 9690 RVA: 0x0027D924 File Offset: 0x0027CD24
		public override int Depth
		{
			get
			{
				if (this.IsClosed)
				{
					throw ADP.DataReaderClosed("Depth");
				}
				return 0;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x060025DB RID: 9691 RVA: 0x0027D948 File Offset: 0x0027CD48
		public override int FieldCount
		{
			get
			{
				if (this.IsClosed)
				{
					throw ADP.DataReaderClosed("FieldCount");
				}
				if (this.MetaData == null)
				{
					return 0;
				}
				return this._metaData.Length;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x060025DC RID: 9692 RVA: 0x0027D980 File Offset: 0x0027CD80
		public override bool HasRows
		{
			get
			{
				if (this.IsClosed)
				{
					throw ADP.DataReaderClosed("HasRows");
				}
				return this._hasRows;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x060025DD RID: 9693 RVA: 0x0027D9A8 File Offset: 0x0027CDA8
		public override bool IsClosed
		{
			get
			{
				return this._isClosed;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060025DE RID: 9694 RVA: 0x0027D9BC File Offset: 0x0027CDBC
		// (set) Token: 0x060025DF RID: 9695 RVA: 0x0027D9D0 File Offset: 0x0027CDD0
		internal bool IsInitialized
		{
			get
			{
				return this._isInitialized;
			}
			set
			{
				this._isInitialized = value;
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060025E0 RID: 9696 RVA: 0x0027D9E4 File Offset: 0x0027CDE4
		internal _SqlMetaDataSet MetaData
		{
			get
			{
				if (this.IsClosed)
				{
					throw ADP.DataReaderClosed("MetaData");
				}
				if (this._metaData == null && !this._metaDataConsumed)
				{
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						this.ConsumeMetaData();
					}
					catch (OutOfMemoryException ex)
					{
						this._isClosed = true;
						if (this._connection != null)
						{
							this._connection.Abort(ex);
						}
						throw;
					}
					catch (StackOverflowException ex2)
					{
						this._isClosed = true;
						if (this._connection != null)
						{
							this._connection.Abort(ex2);
						}
						throw;
					}
					catch (ThreadAbortException ex3)
					{
						this._isClosed = true;
						if (this._connection != null)
						{
							this._connection.Abort(ex3);
						}
						throw;
					}
				}
				return this._metaData;
			}
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x0027DAD8 File Offset: 0x0027CED8
		internal virtual SmiExtendedMetaData[] GetInternalSmiMetaData()
		{
			SmiExtendedMetaData[] array = null;
			_SqlMetaDataSet metaData = this.MetaData;
			if (metaData != null && 0 < metaData.Length)
			{
				array = new SmiExtendedMetaData[metaData.visibleColumns];
				for (int i = 0; i < metaData.Length; i++)
				{
					_SqlMetaData sqlMetaData = metaData[i];
					if (!sqlMetaData.isHidden)
					{
						SqlCollation collation = sqlMetaData.collation;
						string text = null;
						string text2 = null;
						string text3 = null;
						if (SqlDbType.Xml == sqlMetaData.type)
						{
							text = sqlMetaData.xmlSchemaCollectionDatabase;
							text2 = sqlMetaData.xmlSchemaCollectionOwningSchema;
							text3 = sqlMetaData.xmlSchemaCollectionName;
						}
						else if (SqlDbType.Udt == sqlMetaData.type)
						{
							SqlConnection.CheckGetExtendedUDTInfo(sqlMetaData, true);
							text = sqlMetaData.udtDatabaseName;
							text2 = sqlMetaData.udtSchemaName;
							text3 = sqlMetaData.udtTypeName;
						}
						int num = sqlMetaData.length;
						if (num > 8000)
						{
							num = -1;
						}
						else if (SqlDbType.NChar == sqlMetaData.type || SqlDbType.NVarChar == sqlMetaData.type)
						{
							num /= ADP.CharSize;
						}
						array[i] = new SmiQueryMetaData(sqlMetaData.type, (long)num, sqlMetaData.precision, sqlMetaData.scale, (long)((collation != null) ? collation.LCID : this._defaultLCID), (collation != null) ? collation.SqlCompareOptions : SqlCompareOptions.None, sqlMetaData.udtType, false, null, null, sqlMetaData.column, text, text2, text3, sqlMetaData.isNullable, sqlMetaData.serverName, sqlMetaData.catalogName, sqlMetaData.schemaName, sqlMetaData.tableName, sqlMetaData.baseColumn, sqlMetaData.isKey, sqlMetaData.isIdentity, 0 == sqlMetaData.updatability, sqlMetaData.isExpression, sqlMetaData.isDifferentName, sqlMetaData.isHidden);
					}
				}
			}
			return array;
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x060025E2 RID: 9698 RVA: 0x0027DC7C File Offset: 0x0027D07C
		public override int RecordsAffected
		{
			get
			{
				if (this._command != null)
				{
					return this._command.InternalRecordsAffected;
				}
				return this._recordsAffected;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (set) Token: 0x060025E3 RID: 9699 RVA: 0x0027DCA4 File Offset: 0x0027D0A4
		internal string ResetOptionsString
		{
			set
			{
				this._resetOptionsString = value;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x060025E4 RID: 9700 RVA: 0x0027DCB8 File Offset: 0x0027D0B8
		private SqlStatistics Statistics
		{
			get
			{
				return this._statistics;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060025E5 RID: 9701 RVA: 0x0027DCCC File Offset: 0x0027D0CC
		// (set) Token: 0x060025E6 RID: 9702 RVA: 0x0027DCE0 File Offset: 0x0027D0E0
		internal MultiPartTableName[] TableNames
		{
			get
			{
				return this._tableNames;
			}
			set
			{
				this._tableNames = value;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060025E7 RID: 9703 RVA: 0x0027DCF4 File Offset: 0x0027D0F4
		public override int VisibleFieldCount
		{
			get
			{
				if (this.IsClosed)
				{
					throw ADP.DataReaderClosed("VisibleFieldCount");
				}
				if (this.MetaData == null)
				{
					return 0;
				}
				return this.MetaData.visibleColumns;
			}
		}

		// Token: 0x17000602 RID: 1538
		public override object this[int i]
		{
			get
			{
				return this.GetValue(i);
			}
		}

		// Token: 0x17000603 RID: 1539
		public override object this[string name]
		{
			get
			{
				return this.GetValue(this.GetOrdinal(name));
			}
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x0027DD5C File Offset: 0x0027D15C
		internal void Bind(TdsParserStateObject stateObj)
		{
			stateObj.Owner = this;
			this._stateObj = stateObj;
			this._parser = stateObj.Parser;
			this._defaultLCID = this._parser.DefaultLCID;
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x0027DD94 File Offset: 0x0027D194
		internal DataTable BuildSchemaTable()
		{
			_SqlMetaDataSet metaData = this.MetaData;
			DataTable dataTable = new DataTable("SchemaTable");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.MinimumCapacity = metaData.Length;
			DataColumn dataColumn = new DataColumn(SchemaTableColumn.ColumnName, typeof(string));
			DataColumn dataColumn2 = new DataColumn(SchemaTableColumn.ColumnOrdinal, typeof(int));
			DataColumn dataColumn3 = new DataColumn(SchemaTableColumn.ColumnSize, typeof(int));
			DataColumn dataColumn4 = new DataColumn(SchemaTableColumn.NumericPrecision, typeof(short));
			DataColumn dataColumn5 = new DataColumn(SchemaTableColumn.NumericScale, typeof(short));
			DataColumn dataColumn6 = new DataColumn(SchemaTableColumn.DataType, typeof(Type));
			DataColumn dataColumn7 = new DataColumn(SchemaTableOptionalColumn.ProviderSpecificDataType, typeof(Type));
			DataColumn dataColumn8 = new DataColumn(SchemaTableColumn.NonVersionedProviderType, typeof(int));
			DataColumn dataColumn9 = new DataColumn(SchemaTableColumn.ProviderType, typeof(int));
			DataColumn dataColumn10 = new DataColumn(SchemaTableColumn.IsLong, typeof(bool));
			DataColumn dataColumn11 = new DataColumn(SchemaTableColumn.AllowDBNull, typeof(bool));
			DataColumn dataColumn12 = new DataColumn(SchemaTableOptionalColumn.IsReadOnly, typeof(bool));
			DataColumn dataColumn13 = new DataColumn(SchemaTableOptionalColumn.IsRowVersion, typeof(bool));
			DataColumn dataColumn14 = new DataColumn(SchemaTableColumn.IsUnique, typeof(bool));
			DataColumn dataColumn15 = new DataColumn(SchemaTableColumn.IsKey, typeof(bool));
			DataColumn dataColumn16 = new DataColumn(SchemaTableOptionalColumn.IsAutoIncrement, typeof(bool));
			DataColumn dataColumn17 = new DataColumn(SchemaTableOptionalColumn.IsHidden, typeof(bool));
			DataColumn dataColumn18 = new DataColumn(SchemaTableOptionalColumn.BaseCatalogName, typeof(string));
			DataColumn dataColumn19 = new DataColumn(SchemaTableColumn.BaseSchemaName, typeof(string));
			DataColumn dataColumn20 = new DataColumn(SchemaTableColumn.BaseTableName, typeof(string));
			DataColumn dataColumn21 = new DataColumn(SchemaTableColumn.BaseColumnName, typeof(string));
			DataColumn dataColumn22 = new DataColumn(SchemaTableOptionalColumn.BaseServerName, typeof(string));
			DataColumn dataColumn23 = new DataColumn(SchemaTableColumn.IsAliased, typeof(bool));
			DataColumn dataColumn24 = new DataColumn(SchemaTableColumn.IsExpression, typeof(bool));
			DataColumn dataColumn25 = new DataColumn("IsIdentity", typeof(bool));
			DataColumn dataColumn26 = new DataColumn("DataTypeName", typeof(string));
			DataColumn dataColumn27 = new DataColumn("UdtAssemblyQualifiedName", typeof(string));
			DataColumn dataColumn28 = new DataColumn("XmlSchemaCollectionDatabase", typeof(string));
			DataColumn dataColumn29 = new DataColumn("XmlSchemaCollectionOwningSchema", typeof(string));
			DataColumn dataColumn30 = new DataColumn("XmlSchemaCollectionName", typeof(string));
			DataColumn dataColumn31 = new DataColumn("IsColumnSet", typeof(bool));
			dataColumn2.DefaultValue = 0;
			dataColumn10.DefaultValue = false;
			DataColumnCollection columns = dataTable.Columns;
			columns.Add(dataColumn);
			columns.Add(dataColumn2);
			columns.Add(dataColumn3);
			columns.Add(dataColumn4);
			columns.Add(dataColumn5);
			columns.Add(dataColumn14);
			columns.Add(dataColumn15);
			columns.Add(dataColumn22);
			columns.Add(dataColumn18);
			columns.Add(dataColumn21);
			columns.Add(dataColumn19);
			columns.Add(dataColumn20);
			columns.Add(dataColumn6);
			columns.Add(dataColumn11);
			columns.Add(dataColumn9);
			columns.Add(dataColumn23);
			columns.Add(dataColumn24);
			columns.Add(dataColumn25);
			columns.Add(dataColumn16);
			columns.Add(dataColumn13);
			columns.Add(dataColumn17);
			columns.Add(dataColumn10);
			columns.Add(dataColumn12);
			columns.Add(dataColumn7);
			columns.Add(dataColumn26);
			columns.Add(dataColumn28);
			columns.Add(dataColumn29);
			columns.Add(dataColumn30);
			columns.Add(dataColumn27);
			columns.Add(dataColumn8);
			columns.Add(dataColumn31);
			for (int i = 0; i < metaData.Length; i++)
			{
				_SqlMetaData sqlMetaData = metaData[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow[dataColumn] = sqlMetaData.column;
				dataRow[dataColumn2] = sqlMetaData.ordinal;
				dataRow[dataColumn3] = ((sqlMetaData.metaType.IsSizeInCharacters && sqlMetaData.length != int.MaxValue) ? (sqlMetaData.length / 2) : sqlMetaData.length);
				dataRow[dataColumn6] = this.GetFieldTypeInternal(sqlMetaData);
				dataRow[dataColumn7] = this.GetProviderSpecificFieldTypeInternal(sqlMetaData);
				dataRow[dataColumn8] = (int)sqlMetaData.type;
				dataRow[dataColumn26] = this.GetDataTypeNameInternal(sqlMetaData);
				if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && sqlMetaData.IsNewKatmaiDateTimeType)
				{
					dataRow[dataColumn9] = SqlDbType.NVarChar;
					switch (sqlMetaData.type)
					{
					case SqlDbType.Date:
						dataRow[dataColumn3] = 10;
						break;
					case SqlDbType.Time:
						dataRow[dataColumn3] = TdsEnums.WHIDBEY_TIME_LENGTH[(int)((byte.MaxValue != sqlMetaData.scale) ? sqlMetaData.scale : sqlMetaData.metaType.Scale)];
						break;
					case SqlDbType.DateTime2:
						dataRow[dataColumn3] = TdsEnums.WHIDBEY_DATETIME2_LENGTH[(int)((byte.MaxValue != sqlMetaData.scale) ? sqlMetaData.scale : sqlMetaData.metaType.Scale)];
						break;
					case SqlDbType.DateTimeOffset:
						dataRow[dataColumn3] = TdsEnums.WHIDBEY_DATETIMEOFFSET_LENGTH[(int)((byte.MaxValue != sqlMetaData.scale) ? sqlMetaData.scale : sqlMetaData.metaType.Scale)];
						break;
					}
				}
				else if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && sqlMetaData.IsLargeUdt)
				{
					if (this._typeSystem == SqlConnectionString.TypeSystem.SQLServer2005)
					{
						dataRow[dataColumn9] = SqlDbType.VarBinary;
					}
					else
					{
						dataRow[dataColumn9] = SqlDbType.Image;
					}
				}
				else if (this._typeSystem != SqlConnectionString.TypeSystem.SQLServer2000)
				{
					dataRow[dataColumn9] = (int)sqlMetaData.type;
					if (sqlMetaData.type == SqlDbType.Udt)
					{
						dataRow[dataColumn27] = sqlMetaData.udtAssemblyQualifiedName;
					}
					else if (sqlMetaData.type == SqlDbType.Xml)
					{
						dataRow[dataColumn28] = sqlMetaData.xmlSchemaCollectionDatabase;
						dataRow[dataColumn29] = sqlMetaData.xmlSchemaCollectionOwningSchema;
						dataRow[dataColumn30] = sqlMetaData.xmlSchemaCollectionName;
					}
				}
				else
				{
					dataRow[dataColumn9] = this.GetVersionedMetaType(sqlMetaData.metaType).SqlDbType;
				}
				if (255 != sqlMetaData.precision)
				{
					dataRow[dataColumn4] = sqlMetaData.precision;
				}
				else
				{
					dataRow[dataColumn4] = sqlMetaData.metaType.Precision;
				}
				if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && sqlMetaData.IsNewKatmaiDateTimeType)
				{
					dataRow[dataColumn5] = MetaType.MetaNVarChar.Scale;
				}
				else if (255 != sqlMetaData.scale)
				{
					dataRow[dataColumn5] = sqlMetaData.scale;
				}
				else
				{
					dataRow[dataColumn5] = sqlMetaData.metaType.Scale;
				}
				dataRow[dataColumn11] = sqlMetaData.isNullable;
				if (this._browseModeInfoConsumed)
				{
					dataRow[dataColumn23] = sqlMetaData.isDifferentName;
					dataRow[dataColumn15] = sqlMetaData.isKey;
					dataRow[dataColumn17] = sqlMetaData.isHidden;
					dataRow[dataColumn24] = sqlMetaData.isExpression;
				}
				dataRow[dataColumn25] = sqlMetaData.isIdentity;
				dataRow[dataColumn16] = sqlMetaData.isIdentity;
				dataRow[dataColumn10] = sqlMetaData.metaType.IsLong;
				if (SqlDbType.Timestamp == sqlMetaData.type)
				{
					dataRow[dataColumn14] = true;
					dataRow[dataColumn13] = true;
				}
				else
				{
					dataRow[dataColumn14] = false;
					dataRow[dataColumn13] = false;
				}
				dataRow[dataColumn12] = 0 == sqlMetaData.updatability;
				dataRow[dataColumn31] = sqlMetaData.isColumnSet;
				if (!ADP.IsEmpty(sqlMetaData.serverName))
				{
					dataRow[dataColumn22] = sqlMetaData.serverName;
				}
				if (!ADP.IsEmpty(sqlMetaData.catalogName))
				{
					dataRow[dataColumn18] = sqlMetaData.catalogName;
				}
				if (!ADP.IsEmpty(sqlMetaData.schemaName))
				{
					dataRow[dataColumn19] = sqlMetaData.schemaName;
				}
				if (!ADP.IsEmpty(sqlMetaData.tableName))
				{
					dataRow[dataColumn20] = sqlMetaData.tableName;
				}
				if (!ADP.IsEmpty(sqlMetaData.baseColumn))
				{
					dataRow[dataColumn21] = sqlMetaData.baseColumn;
				}
				else if (!ADP.IsEmpty(sqlMetaData.column))
				{
					dataRow[dataColumn21] = sqlMetaData.column;
				}
				dataTable.Rows.Add(dataRow);
				dataRow.AcceptChanges();
			}
			foreach (object obj in columns)
			{
				DataColumn dataColumn32 = (DataColumn)obj;
				dataColumn32.ReadOnly = true;
			}
			return dataTable;
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x0027E700 File Offset: 0x0027DB00
		internal void Cancel(int objectID)
		{
			TdsParserStateObject stateObj = this._stateObj;
			if (stateObj != null)
			{
				stateObj.Cancel(objectID);
			}
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x0027E720 File Offset: 0x0027DB20
		private void CleanPartialRead()
		{
			if (this._nextColumnHeaderToRead == 0)
			{
				this._stateObj.Parser.SkipRow(this._metaData, this._stateObj);
				return;
			}
			this.ResetBlobState();
			this._stateObj.Parser.SkipRow(this._metaData, this._nextColumnHeaderToRead, this._stateObj);
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x0027E77C File Offset: 0x0027DB7C
		public override void Close()
		{
			SqlStatistics sqlStatistics = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlDataReader.Close|API> %d#", this.ObjectID);
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				if (!this.IsClosed)
				{
					this.SetTimeout();
					this.CloseInternal(true);
				}
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x0027E7EC File Offset: 0x0027DBEC
		private void CloseInternal(bool closeReader)
		{
			TdsParser parser = this._parser;
			TdsParserStateObject stateObj = this._stateObj;
			bool flag = this.IsCommandBehavior(CommandBehavior.CloseConnection);
			this._parser = null;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				if (parser != null && stateObj != null && stateObj._pendingData && parser.State == TdsParserState.OpenLoggedIn)
				{
					if (this._altRowStatus == SqlDataReader.ALTROWSTATUS.AltRow)
					{
						this._dataReady = true;
					}
					if (this._dataReady)
					{
						this.CleanPartialRead();
					}
					parser.Run(RunBehavior.Clean, this._command, this, null, stateObj);
				}
				this.RestoreServerSettings(parser, stateObj);
			}
			catch (OutOfMemoryException ex)
			{
				this._isClosed = true;
				flag2 = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex);
				}
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this._isClosed = true;
				flag2 = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex2);
				}
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this._isClosed = true;
				flag2 = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex3);
				}
				throw;
			}
			finally
			{
				if (flag2)
				{
					this._isClosed = true;
					this._command = null;
					this._connection = null;
					this._statistics = null;
				}
				else if (closeReader)
				{
					this._stateObj = null;
					this._data = null;
					if (this.Connection != null)
					{
						this.Connection.RemoveWeakReference(this);
					}
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						if (this._command != null && stateObj != null)
						{
							stateObj.CloseSession();
						}
					}
					catch (OutOfMemoryException ex4)
					{
						this._isClosed = true;
						flag2 = true;
						if (this._connection != null)
						{
							this._connection.Abort(ex4);
						}
						throw;
					}
					catch (StackOverflowException ex5)
					{
						this._isClosed = true;
						flag2 = true;
						if (this._connection != null)
						{
							this._connection.Abort(ex5);
						}
						throw;
					}
					catch (ThreadAbortException ex6)
					{
						this._isClosed = true;
						flag2 = true;
						if (this._connection != null)
						{
							this._connection.Abort(ex6);
						}
						throw;
					}
					this.SetMetaData(null, false);
					this._dataReady = false;
					this._isClosed = true;
					this._fieldNameLookup = null;
					if (flag && this.Connection != null)
					{
						this.Connection.Close();
					}
					if (this._command != null)
					{
						this._recordsAffected = this._command.InternalRecordsAffected;
					}
					this._command = null;
					this._connection = null;
					this._statistics = null;
				}
			}
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x0027EAC0 File Offset: 0x0027DEC0
		internal void CloseReaderFromConnection()
		{
			this.Close();
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x0027EAD4 File Offset: 0x0027DED4
		private void ConsumeMetaData()
		{
			while (this._parser != null && this._stateObj != null && this._stateObj._pendingData && !this._metaDataConsumed)
			{
				if (this._parser.State == TdsParserState.Broken || this._parser.State == TdsParserState.Closed)
				{
					if (this._parser.Connection != null)
					{
						this._parser.Connection.DoomThisConnection();
					}
					throw SQL.ConnectionDoomed();
				}
				this._parser.Run(RunBehavior.ReturnImmediately, this._command, this, null, this._stateObj);
			}
			if (this._metaData != null)
			{
				this._metaData.visibleColumns = 0;
				int[] array = new int[this._metaData.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this._metaData.visibleColumns;
					if (!this._metaData[i].isHidden)
					{
						this._metaData.visibleColumns++;
					}
				}
				this._metaData.indexMap = array;
			}
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x0027EBD4 File Offset: 0x0027DFD4
		public override string GetDataTypeName(int i)
		{
			SqlStatistics sqlStatistics = null;
			string dataTypeNameInternal;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				if (this.MetaData == null)
				{
					throw SQL.InvalidRead();
				}
				dataTypeNameInternal = this.GetDataTypeNameInternal(this._metaData[i]);
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return dataTypeNameInternal;
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x0027EC38 File Offset: 0x0027E038
		private string GetDataTypeNameInternal(_SqlMetaData metaData)
		{
			string text;
			if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && metaData.IsNewKatmaiDateTimeType)
			{
				text = MetaType.MetaNVarChar.TypeName;
			}
			else if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && metaData.IsLargeUdt)
			{
				if (this._typeSystem == SqlConnectionString.TypeSystem.SQLServer2005)
				{
					text = MetaType.MetaMaxVarBinary.TypeName;
				}
				else
				{
					text = MetaType.MetaImage.TypeName;
				}
			}
			else if (this._typeSystem != SqlConnectionString.TypeSystem.SQLServer2000)
			{
				if (metaData.type == SqlDbType.Udt)
				{
					text = string.Concat(new string[] { metaData.udtDatabaseName, ".", metaData.udtSchemaName, ".", metaData.udtTypeName });
				}
				else
				{
					text = metaData.metaType.TypeName;
				}
			}
			else
			{
				text = this.GetVersionedMetaType(metaData.metaType).TypeName;
			}
			return text;
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x0027ED20 File Offset: 0x0027E120
		public override IEnumerator GetEnumerator()
		{
			return new DbEnumerator(this, this.IsCommandBehavior(CommandBehavior.CloseConnection));
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x0027ED3C File Offset: 0x0027E13C
		public override Type GetFieldType(int i)
		{
			SqlStatistics sqlStatistics = null;
			Type fieldTypeInternal;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				if (this.MetaData == null)
				{
					throw SQL.InvalidRead();
				}
				fieldTypeInternal = this.GetFieldTypeInternal(this._metaData[i]);
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return fieldTypeInternal;
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x0027EDA0 File Offset: 0x0027E1A0
		private Type GetFieldTypeInternal(_SqlMetaData metaData)
		{
			Type type;
			if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && metaData.IsNewKatmaiDateTimeType)
			{
				type = MetaType.MetaNVarChar.ClassType;
			}
			else if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && metaData.IsLargeUdt)
			{
				if (this._typeSystem == SqlConnectionString.TypeSystem.SQLServer2005)
				{
					type = MetaType.MetaMaxVarBinary.ClassType;
				}
				else
				{
					type = MetaType.MetaImage.ClassType;
				}
			}
			else if (this._typeSystem != SqlConnectionString.TypeSystem.SQLServer2000)
			{
				if (metaData.type == SqlDbType.Udt)
				{
					SqlConnection.CheckGetExtendedUDTInfo(metaData, false);
					type = metaData.udtType;
				}
				else
				{
					type = metaData.metaType.ClassType;
				}
			}
			else
			{
				type = this.GetVersionedMetaType(metaData.metaType).ClassType;
			}
			return type;
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x0027EE58 File Offset: 0x0027E258
		internal virtual int GetLocaleId(int i)
		{
			_SqlMetaData sqlMetaData = this.MetaData[i];
			int num;
			if (sqlMetaData.collation != null)
			{
				num = sqlMetaData.collation.LCID;
			}
			else
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x0027EE8C File Offset: 0x0027E28C
		public override string GetName(int i)
		{
			if (this.MetaData == null)
			{
				throw SQL.InvalidRead();
			}
			return this._metaData[i].column;
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x0027EEB8 File Offset: 0x0027E2B8
		public override Type GetProviderSpecificFieldType(int i)
		{
			SqlStatistics sqlStatistics = null;
			Type providerSpecificFieldTypeInternal;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				if (this.MetaData == null)
				{
					throw SQL.InvalidRead();
				}
				providerSpecificFieldTypeInternal = this.GetProviderSpecificFieldTypeInternal(this._metaData[i]);
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return providerSpecificFieldTypeInternal;
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x0027EF1C File Offset: 0x0027E31C
		private Type GetProviderSpecificFieldTypeInternal(_SqlMetaData metaData)
		{
			Type type;
			if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && metaData.IsNewKatmaiDateTimeType)
			{
				type = MetaType.MetaNVarChar.SqlType;
			}
			else if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && metaData.IsLargeUdt)
			{
				if (this._typeSystem == SqlConnectionString.TypeSystem.SQLServer2005)
				{
					type = MetaType.MetaMaxVarBinary.SqlType;
				}
				else
				{
					type = MetaType.MetaImage.SqlType;
				}
			}
			else if (this._typeSystem != SqlConnectionString.TypeSystem.SQLServer2000)
			{
				if (metaData.type == SqlDbType.Udt)
				{
					SqlConnection.CheckGetExtendedUDTInfo(metaData, false);
					type = metaData.udtType;
				}
				else
				{
					type = metaData.metaType.SqlType;
				}
			}
			else
			{
				type = this.GetVersionedMetaType(metaData.metaType).SqlType;
			}
			return type;
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x0027EFD4 File Offset: 0x0027E3D4
		public override int GetOrdinal(string name)
		{
			SqlStatistics sqlStatistics = null;
			int ordinal;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				if (this._fieldNameLookup == null)
				{
					if (this.MetaData == null)
					{
						throw SQL.InvalidRead();
					}
					this._fieldNameLookup = new FieldNameLookup(this, this._defaultLCID);
				}
				ordinal = this._fieldNameLookup.GetOrdinal(name);
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return ordinal;
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x0027F04C File Offset: 0x0027E44C
		public override object GetProviderSpecificValue(int i)
		{
			return this.GetSqlValue(i);
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x0027F060 File Offset: 0x0027E460
		public override int GetProviderSpecificValues(object[] values)
		{
			return this.GetSqlValues(values);
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x0027F074 File Offset: 0x0027E474
		public override DataTable GetSchemaTable()
		{
			SqlStatistics sqlStatistics = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlDataReader.GetSchemaTable|API> %d#", this.ObjectID);
			DataTable dataTable;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				if ((this._metaData == null || this._metaData.schemaTable == null) && this.MetaData != null)
				{
					this._metaData.schemaTable = this.BuildSchemaTable();
				}
				if (this._metaData != null)
				{
					dataTable = this._metaData.schemaTable;
				}
				else
				{
					dataTable = null;
				}
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
				Bid.ScopeLeave(ref intPtr);
			}
			return dataTable;
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x0027F114 File Offset: 0x0027E514
		public override bool GetBoolean(int i)
		{
			this.ReadColumn(i);
			return this._data[i].Boolean;
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x0027F138 File Offset: 0x0027E538
		public override byte GetByte(int i)
		{
			this.ReadColumn(i);
			return this._data[i].Byte;
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x0027F15C File Offset: 0x0027E55C
		public override long GetBytes(int i, long dataIndex, byte[] buffer, int bufferIndex, int length)
		{
			SqlStatistics sqlStatistics = null;
			long num = 0L;
			if (this.MetaData == null || !this._dataReady)
			{
				throw SQL.InvalidRead();
			}
			MetaType metaType = this._metaData[i].metaType;
			if ((!metaType.IsLong && !metaType.IsBinType) || SqlDbType.Xml == metaType.SqlDbType)
			{
				throw SQL.NonBlobColumn(this._metaData[i].column);
			}
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				this.SetTimeout();
				num = this.GetBytesInternal(i, dataIndex, buffer, bufferIndex, length);
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return num;
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x0027F210 File Offset: 0x0027E610
		internal virtual long GetBytesInternal(int i, long dataIndex, byte[] buffer, int bufferIndex, int length)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			long num2;
			try
			{
				int num = 0;
				if (this.IsCommandBehavior(CommandBehavior.SequentialAccess))
				{
					if (0 > i || i >= this._metaData.Length)
					{
						throw new IndexOutOfRangeException();
					}
					if (this._nextColumnDataToRead > i)
					{
						throw ADP.NonSequentialColumnAccess(i, this._nextColumnDataToRead);
					}
					if (this._nextColumnHeaderToRead <= i)
					{
						this.ReadColumnHeader(i);
					}
					if (this._data[i] != null && this._data[i].IsNull)
					{
						throw new SqlNullValueException();
					}
					if (0L == this._columnDataBytesRemaining)
					{
						num2 = 0L;
					}
					else if (buffer == null)
					{
						if (this._metaData[i].metaType.IsPlp)
						{
							num2 = (long)this._parser.PlpBytesTotalLength(this._stateObj);
						}
						else
						{
							num2 = this._columnDataBytesRemaining;
						}
					}
					else
					{
						if (dataIndex < 0L)
						{
							throw ADP.NegativeParameter("dataIndex");
						}
						if (dataIndex < this._columnDataBytesRead)
						{
							throw ADP.NonSeqByteAccess(dataIndex, this._columnDataBytesRead, "GetBytes");
						}
						long num3 = dataIndex - this._columnDataBytesRead;
						if (num3 > this._columnDataBytesRemaining && !this._metaData[i].metaType.IsPlp)
						{
							num2 = 0L;
						}
						else
						{
							if (bufferIndex < 0 || bufferIndex >= buffer.Length)
							{
								throw ADP.InvalidDestinationBufferIndex(buffer.Length, bufferIndex, "bufferIndex");
							}
							if (length + bufferIndex > buffer.Length)
							{
								throw ADP.InvalidBufferSizeOrIndex(length, bufferIndex);
							}
							if (length < 0)
							{
								throw ADP.InvalidDataLength((long)length);
							}
							if (this._metaData[i].metaType.IsPlp)
							{
								if (num3 > 0L)
								{
									num3 = (long)this._parser.SkipPlpValue((ulong)num3, this._stateObj);
									this._columnDataBytesRead += num3;
								}
								num3 = (long)this._stateObj.ReadPlpBytes(ref buffer, bufferIndex, length);
								this._columnDataBytesRead += num3;
								this._columnDataBytesRemaining = (long)this._parser.PlpBytesLeft(this._stateObj);
								num2 = num3;
							}
							else
							{
								if (num3 > 0L)
								{
									this._parser.SkipLongBytes((ulong)num3, this._stateObj);
									this._columnDataBytesRead += num3;
									this._columnDataBytesRemaining -= num3;
								}
								num3 = ((this._columnDataBytesRemaining < (long)length) ? this._columnDataBytesRemaining : ((long)length));
								this._stateObj.ReadByteArray(buffer, bufferIndex, (int)num3);
								this._columnDataBytesRead += num3;
								this._columnDataBytesRemaining -= num3;
								num2 = num3;
							}
						}
					}
				}
				else
				{
					if (dataIndex < 0L)
					{
						throw ADP.NegativeParameter("dataIndex");
					}
					if (dataIndex > 2147483647L)
					{
						throw ADP.InvalidSourceBufferIndex(num, dataIndex, "dataIndex");
					}
					int num4 = (int)dataIndex;
					byte[] array;
					if (this._metaData[i].metaType.IsBinType)
					{
						array = this.GetSqlBinary(i).Value;
					}
					else
					{
						SqlString sqlString = this.GetSqlString(i);
						if (this._metaData[i].metaType.IsNCharType)
						{
							array = sqlString.GetUnicodeBytes();
						}
						else
						{
							array = sqlString.GetNonUnicodeBytes();
						}
					}
					num = array.Length;
					if (buffer == null)
					{
						num2 = (long)num;
					}
					else if (num4 < 0 || num4 >= num)
					{
						num2 = 0L;
					}
					else
					{
						try
						{
							if (num4 < num)
							{
								if (num4 + length > num)
								{
									num -= num4;
								}
								else
								{
									num = length;
								}
							}
							Array.Copy(array, num4, buffer, bufferIndex, num);
						}
						catch (Exception ex)
						{
							if (!ADP.IsCatchableExceptionType(ex))
							{
								throw;
							}
							num = array.Length;
							if (length < 0)
							{
								throw ADP.InvalidDataLength((long)length);
							}
							if (bufferIndex < 0 || bufferIndex >= buffer.Length)
							{
								throw ADP.InvalidDestinationBufferIndex(buffer.Length, bufferIndex, "bufferIndex");
							}
							if (num + bufferIndex > buffer.Length)
							{
								throw ADP.InvalidBufferSizeOrIndex(num, bufferIndex);
							}
							throw;
						}
						num2 = (long)num;
					}
				}
			}
			catch (OutOfMemoryException ex2)
			{
				this._isClosed = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex2);
				}
				throw;
			}
			catch (StackOverflowException ex3)
			{
				this._isClosed = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex3);
				}
				throw;
			}
			catch (ThreadAbortException ex4)
			{
				this._isClosed = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex4);
				}
				throw;
			}
			return num2;
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x0027F664 File Offset: 0x0027EA64
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override char GetChar(int i)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x0027F678 File Offset: 0x0027EA78
		public override long GetChars(int i, long dataIndex, char[] buffer, int bufferIndex, int length)
		{
			SqlStatistics sqlStatistics = null;
			if (this.MetaData == null || !this._dataReady)
			{
				throw SQL.InvalidRead();
			}
			if (0 > i || i >= this._metaData.Length)
			{
				throw new IndexOutOfRangeException();
			}
			long num;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				this.SetTimeout();
				if (this._metaData[i].metaType.IsPlp && this.IsCommandBehavior(CommandBehavior.SequentialAccess))
				{
					if (length < 0)
					{
						throw ADP.InvalidDataLength((long)length);
					}
					if (bufferIndex < 0 || (buffer != null && bufferIndex >= buffer.Length))
					{
						throw ADP.InvalidDestinationBufferIndex(buffer.Length, bufferIndex, "bufferIndex");
					}
					if (buffer != null && length + bufferIndex > buffer.Length)
					{
						throw ADP.InvalidBufferSizeOrIndex(length, bufferIndex);
					}
					if (this._metaData[i].type == SqlDbType.Xml)
					{
						num = this.GetStreamingXmlChars(i, dataIndex, buffer, bufferIndex, length);
					}
					else
					{
						num = this.GetCharsFromPlpData(i, dataIndex, buffer, bufferIndex, length);
					}
				}
				else
				{
					if (this._nextColumnDataToRead == i + 1 && this._nextColumnHeaderToRead == i + 1 && this._columnDataChars != null)
					{
						if (this.IsCommandBehavior(CommandBehavior.SequentialAccess) && dataIndex < this._columnDataCharsRead)
						{
							throw ADP.NonSeqByteAccess(dataIndex, this._columnDataCharsRead, "GetChars");
						}
					}
					else
					{
						string value = this.GetSqlString(i).Value;
						this._columnDataChars = value.ToCharArray();
						this._columnDataCharsRead = 0L;
					}
					int num2 = this._columnDataChars.Length;
					if (dataIndex > 2147483647L)
					{
						throw ADP.InvalidSourceBufferIndex(num2, dataIndex, "dataIndex");
					}
					int num3 = (int)dataIndex;
					if (buffer == null)
					{
						num = (long)num2;
					}
					else if (num3 < 0 || num3 >= num2)
					{
						num = 0L;
					}
					else
					{
						try
						{
							if (num3 < num2)
							{
								if (num3 + length > num2)
								{
									num2 -= num3;
								}
								else
								{
									num2 = length;
								}
							}
							Array.Copy(this._columnDataChars, num3, buffer, bufferIndex, num2);
							this._columnDataCharsRead += (long)num2;
						}
						catch (Exception ex)
						{
							if (!ADP.IsCatchableExceptionType(ex))
							{
								throw;
							}
							num2 = this._columnDataChars.Length;
							if (length < 0)
							{
								throw ADP.InvalidDataLength((long)length);
							}
							if (bufferIndex < 0 || bufferIndex >= buffer.Length)
							{
								throw ADP.InvalidDestinationBufferIndex(buffer.Length, bufferIndex, "bufferIndex");
							}
							if (num2 + bufferIndex > buffer.Length)
							{
								throw ADP.InvalidBufferSizeOrIndex(num2, bufferIndex);
							}
							throw;
						}
						num = (long)num2;
					}
				}
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return num;
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x0027F8E4 File Offset: 0x0027ECE4
		private long GetCharsFromPlpData(int i, long dataIndex, char[] buffer, int bufferIndex, int length)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			long num;
			try
			{
				if (this.MetaData == null || !this._dataReady)
				{
					throw SQL.InvalidRead();
				}
				if (this._nextColumnDataToRead > i)
				{
					throw ADP.NonSequentialColumnAccess(i, this._nextColumnDataToRead);
				}
				if (!this._metaData[i].metaType.IsCharType)
				{
					throw SQL.NonCharColumn(this._metaData[i].column);
				}
				if (this._nextColumnHeaderToRead <= i)
				{
					this.ReadColumnHeader(i);
				}
				if (this._data[i] != null && this._data[i].IsNull)
				{
					throw new SqlNullValueException();
				}
				if (dataIndex < this._columnDataCharsRead)
				{
					throw ADP.NonSeqByteAccess(dataIndex, this._columnDataCharsRead, "GetChars");
				}
				bool isNCharType = this._metaData[i].metaType.IsNCharType;
				if (0L == this._columnDataBytesRemaining)
				{
					num = 0L;
				}
				else if (buffer == null)
				{
					long num2 = (long)this._parser.PlpBytesTotalLength(this._stateObj);
					num = ((isNCharType && num2 > 0L) ? (num2 >> 1) : num2);
				}
				else
				{
					long num2;
					if (dataIndex > this._columnDataCharsRead)
					{
						num2 = dataIndex - this._columnDataCharsRead;
						num2 = (isNCharType ? (num2 << 1) : num2);
						num2 = (long)this._parser.SkipPlpValue((ulong)num2, this._stateObj);
						this._columnDataBytesRead += num2;
						this._columnDataCharsRead += ((isNCharType && num2 > 0L) ? (num2 >> 1) : num2);
					}
					num2 = (long)length;
					if (isNCharType)
					{
						num2 = (long)this._parser.ReadPlpUnicodeChars(ref buffer, bufferIndex, length, this._stateObj);
						this._columnDataBytesRead += num2 << 1;
					}
					else
					{
						num2 = (long)this._parser.ReadPlpAnsiChars(ref buffer, bufferIndex, length, this._metaData[i], this._stateObj);
						this._columnDataBytesRead += num2 << 1;
					}
					this._columnDataCharsRead += num2;
					this._columnDataBytesRemaining = (long)this._parser.PlpBytesLeft(this._stateObj);
					num = num2;
				}
			}
			catch (OutOfMemoryException ex)
			{
				this._isClosed = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex);
				}
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this._isClosed = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex2);
				}
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this._isClosed = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex3);
				}
				throw;
			}
			return num;
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x0027FB88 File Offset: 0x0027EF88
		internal long GetStreamingXmlChars(int i, long dataIndex, char[] buffer, int bufferIndex, int length)
		{
			if (this._streamingXml != null && this._streamingXml.ColumnOrdinal != i)
			{
				this._streamingXml.Close();
				this._streamingXml = null;
			}
			SqlStreamingXml sqlStreamingXml;
			if (this._streamingXml == null)
			{
				sqlStreamingXml = new SqlStreamingXml(i, this);
			}
			else
			{
				sqlStreamingXml = this._streamingXml;
			}
			long chars = sqlStreamingXml.GetChars(dataIndex, buffer, bufferIndex, length);
			if (this._streamingXml == null)
			{
				this._streamingXml = sqlStreamingXml;
			}
			return chars;
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x0027FBF8 File Offset: 0x0027EFF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		IDataReader IDataRecord.GetData(int i)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x0027FC0C File Offset: 0x0027F00C
		public override DateTime GetDateTime(int i)
		{
			this.ReadColumn(i);
			DateTime dateTime = this._data[i].DateTime;
			if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && this._metaData[i].IsNewKatmaiDateTimeType)
			{
				object @string = this._data[i].String;
				dateTime = (DateTime)@string;
			}
			return dateTime;
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x0027FC64 File Offset: 0x0027F064
		public override decimal GetDecimal(int i)
		{
			this.ReadColumn(i);
			return this._data[i].Decimal;
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x0027FC88 File Offset: 0x0027F088
		public override double GetDouble(int i)
		{
			this.ReadColumn(i);
			return this._data[i].Double;
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x0027FCAC File Offset: 0x0027F0AC
		public override float GetFloat(int i)
		{
			this.ReadColumn(i);
			return this._data[i].Single;
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x0027FCD0 File Offset: 0x0027F0D0
		public override Guid GetGuid(int i)
		{
			this.ReadColumn(i);
			return this._data[i].SqlGuid.Value;
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x0027FCFC File Offset: 0x0027F0FC
		public override short GetInt16(int i)
		{
			this.ReadColumn(i);
			return this._data[i].Int16;
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x0027FD20 File Offset: 0x0027F120
		public override int GetInt32(int i)
		{
			this.ReadColumn(i);
			return this._data[i].Int32;
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x0027FD44 File Offset: 0x0027F144
		public override long GetInt64(int i)
		{
			this.ReadColumn(i);
			return this._data[i].Int64;
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x0027FD68 File Offset: 0x0027F168
		public virtual SqlBoolean GetSqlBoolean(int i)
		{
			this.ReadColumn(i);
			return this._data[i].SqlBoolean;
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x0027FD8C File Offset: 0x0027F18C
		public virtual SqlBinary GetSqlBinary(int i)
		{
			this.ReadColumn(i);
			return this._data[i].SqlBinary;
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x0027FDB0 File Offset: 0x0027F1B0
		public virtual SqlByte GetSqlByte(int i)
		{
			this.ReadColumn(i);
			return this._data[i].SqlByte;
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x0027FDD4 File Offset: 0x0027F1D4
		public virtual SqlBytes GetSqlBytes(int i)
		{
			if (this.MetaData == null)
			{
				throw SQL.InvalidRead();
			}
			this.ReadColumn(i);
			SqlBinary sqlBinary = this._data[i].SqlBinary;
			return new SqlBytes(sqlBinary);
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x0027FE0C File Offset: 0x0027F20C
		public virtual SqlChars GetSqlChars(int i)
		{
			this.ReadColumn(i);
			SqlString sqlString;
			if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && this._metaData[i].IsNewKatmaiDateTimeType)
			{
				sqlString = this._data[i].KatmaiDateTimeSqlString;
			}
			else
			{
				sqlString = this._data[i].SqlString;
			}
			return new SqlChars(sqlString);
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x0027FE64 File Offset: 0x0027F264
		public virtual SqlDateTime GetSqlDateTime(int i)
		{
			this.ReadColumn(i);
			return this._data[i].SqlDateTime;
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x0027FE88 File Offset: 0x0027F288
		public virtual SqlDecimal GetSqlDecimal(int i)
		{
			this.ReadColumn(i);
			return this._data[i].SqlDecimal;
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x0027FEAC File Offset: 0x0027F2AC
		public virtual SqlGuid GetSqlGuid(int i)
		{
			this.ReadColumn(i);
			return this._data[i].SqlGuid;
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x0027FED0 File Offset: 0x0027F2D0
		public virtual SqlDouble GetSqlDouble(int i)
		{
			this.ReadColumn(i);
			return this._data[i].SqlDouble;
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x0027FEF4 File Offset: 0x0027F2F4
		public virtual SqlInt16 GetSqlInt16(int i)
		{
			this.ReadColumn(i);
			return this._data[i].SqlInt16;
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x0027FF18 File Offset: 0x0027F318
		public virtual SqlInt32 GetSqlInt32(int i)
		{
			this.ReadColumn(i);
			return this._data[i].SqlInt32;
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x0027FF3C File Offset: 0x0027F33C
		public virtual SqlInt64 GetSqlInt64(int i)
		{
			this.ReadColumn(i);
			return this._data[i].SqlInt64;
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x0027FF60 File Offset: 0x0027F360
		public virtual SqlMoney GetSqlMoney(int i)
		{
			this.ReadColumn(i);
			return this._data[i].SqlMoney;
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x0027FF84 File Offset: 0x0027F384
		public virtual SqlSingle GetSqlSingle(int i)
		{
			this.ReadColumn(i);
			return this._data[i].SqlSingle;
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x0027FFA8 File Offset: 0x0027F3A8
		public virtual SqlString GetSqlString(int i)
		{
			this.ReadColumn(i);
			if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && this._metaData[i].IsNewKatmaiDateTimeType)
			{
				return this._data[i].KatmaiDateTimeSqlString;
			}
			return this._data[i].SqlString;
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x0027FFF8 File Offset: 0x0027F3F8
		public virtual SqlXml GetSqlXml(int i)
		{
			this.ReadColumn(i);
			SqlXml sqlXml;
			if (this._typeSystem != SqlConnectionString.TypeSystem.SQLServer2000)
			{
				sqlXml = (this._data[i].IsNull ? SqlXml.Null : this._data[i].SqlCachedBuffer.ToSqlXml());
			}
			else
			{
				SqlXml sqlXml2 = (this._data[i].IsNull ? SqlXml.Null : this._data[i].SqlCachedBuffer.ToSqlXml());
				object @string = this._data[i].String;
				sqlXml = (SqlXml)@string;
			}
			return sqlXml;
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x00280088 File Offset: 0x0027F488
		public virtual object GetSqlValue(int i)
		{
			SqlStatistics sqlStatistics = null;
			object obj;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				if (this.MetaData == null || !this._dataReady)
				{
					throw SQL.InvalidRead();
				}
				this.SetTimeout();
				object sqlValueInternal = this.GetSqlValueInternal(i);
				obj = sqlValueInternal;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return obj;
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x002800F0 File Offset: 0x0027F4F0
		private object GetSqlValueInternal(int i)
		{
			this.ReadColumn(i, false);
			if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && this._metaData[i].IsNewKatmaiDateTimeType)
			{
				return this._data[i].KatmaiDateTimeSqlString;
			}
			object obj;
			if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && this._metaData[i].IsLargeUdt)
			{
				obj = this._data[i].SqlValue;
			}
			else if (this._typeSystem != SqlConnectionString.TypeSystem.SQLServer2000)
			{
				if (this._metaData[i].type == SqlDbType.Udt)
				{
					SqlConnection.CheckGetExtendedUDTInfo(this._metaData[i], true);
					obj = this.Connection.GetUdtValue(this._data[i].Value, this._metaData[i], false);
				}
				else
				{
					obj = this._data[i].SqlValue;
				}
			}
			else if (this._metaData[i].type == SqlDbType.Xml)
			{
				obj = this._data[i].SqlString;
			}
			else
			{
				obj = this._data[i].SqlValue;
			}
			return obj;
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x00280210 File Offset: 0x0027F610
		public virtual int GetSqlValues(object[] values)
		{
			SqlStatistics sqlStatistics = null;
			int num2;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				if (this.MetaData == null || !this._dataReady)
				{
					throw SQL.InvalidRead();
				}
				if (values == null)
				{
					throw ADP.ArgumentNull("values");
				}
				this.SetTimeout();
				int num = ((values.Length < this._metaData.visibleColumns) ? values.Length : this._metaData.visibleColumns);
				for (int i = 0; i < num; i++)
				{
					values[this._metaData.indexMap[i]] = this.GetSqlValueInternal(i);
				}
				num2 = num;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return num2;
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x002802C0 File Offset: 0x0027F6C0
		public override string GetString(int i)
		{
			this.ReadColumn(i);
			if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && this._metaData[i].IsNewKatmaiDateTimeType)
			{
				return this._data[i].KatmaiDateTimeString;
			}
			return this._data[i].String;
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x00280310 File Offset: 0x0027F710
		public override object GetValue(int i)
		{
			SqlStatistics sqlStatistics = null;
			object obj;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				if (this.MetaData == null || !this._dataReady)
				{
					throw SQL.InvalidRead();
				}
				this.SetTimeout();
				object valueInternal = this.GetValueInternal(i);
				obj = valueInternal;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return obj;
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x00280378 File Offset: 0x0027F778
		public virtual TimeSpan GetTimeSpan(int i)
		{
			this.ReadColumn(i);
			TimeSpan timeSpan = this._data[i].Time;
			if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005)
			{
				object @string = this._data[i].String;
				timeSpan = (TimeSpan)@string;
			}
			return timeSpan;
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x002803C0 File Offset: 0x0027F7C0
		public virtual DateTimeOffset GetDateTimeOffset(int i)
		{
			this.ReadColumn(i);
			DateTimeOffset dateTimeOffset = this._data[i].DateTimeOffset;
			if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005)
			{
				object @string = this._data[i].String;
				dateTimeOffset = (DateTimeOffset)@string;
			}
			return dateTimeOffset;
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x00280408 File Offset: 0x0027F808
		private object GetValueInternal(int i)
		{
			this.ReadColumn(i, false);
			if (this._typeSystem > SqlConnectionString.TypeSystem.SQLServer2005 || !this._metaData[i].IsNewKatmaiDateTimeType)
			{
				object obj;
				if (this._typeSystem <= SqlConnectionString.TypeSystem.SQLServer2005 && this._metaData[i].IsLargeUdt)
				{
					obj = this._data[i].Value;
				}
				else if (this._typeSystem != SqlConnectionString.TypeSystem.SQLServer2000)
				{
					if (this._metaData[i].type != SqlDbType.Udt)
					{
						obj = this._data[i].Value;
					}
					else
					{
						SqlConnection.CheckGetExtendedUDTInfo(this._metaData[i], true);
						obj = this.Connection.GetUdtValue(this._data[i].Value, this._metaData[i], true);
					}
				}
				else
				{
					obj = this._data[i].Value;
				}
				return obj;
			}
			if (this._data[i].IsNull)
			{
				return DBNull.Value;
			}
			return this._data[i].KatmaiDateTimeString;
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x00280510 File Offset: 0x0027F910
		public override int GetValues(object[] values)
		{
			SqlStatistics sqlStatistics = null;
			int num2;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				if (this.MetaData == null || !this._dataReady)
				{
					throw SQL.InvalidRead();
				}
				if (values == null)
				{
					throw ADP.ArgumentNull("values");
				}
				int num = ((values.Length < this._metaData.visibleColumns) ? values.Length : this._metaData.visibleColumns);
				this.SetTimeout();
				for (int i = 0; i < num; i++)
				{
					values[this._metaData.indexMap[i]] = this.GetValueInternal(i);
				}
				if (this._rowException != null)
				{
					throw this._rowException;
				}
				num2 = num;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return num2;
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x002805D0 File Offset: 0x0027F9D0
		private MetaType GetVersionedMetaType(MetaType actualMetaType)
		{
			MetaType metaType;
			if (actualMetaType == MetaType.MetaUdt)
			{
				metaType = MetaType.MetaVarBinary;
			}
			else if (actualMetaType == MetaType.MetaXml)
			{
				metaType = MetaType.MetaNText;
			}
			else if (actualMetaType == MetaType.MetaMaxVarBinary)
			{
				metaType = MetaType.MetaImage;
			}
			else if (actualMetaType == MetaType.MetaMaxVarChar)
			{
				metaType = MetaType.MetaText;
			}
			else if (actualMetaType == MetaType.MetaMaxNVarChar)
			{
				metaType = MetaType.MetaNText;
			}
			else
			{
				metaType = actualMetaType;
			}
			return metaType;
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x00280634 File Offset: 0x0027FA34
		private bool HasMoreResults()
		{
			if (this._parser != null)
			{
				if (this.HasMoreRows())
				{
					return true;
				}
				while (this._stateObj._pendingData)
				{
					byte b = this._stateObj.PeekByte();
					byte b2 = b;
					if (b2 == 129)
					{
						return true;
					}
					switch (b2)
					{
					case 209:
						return true;
					case 210:
						break;
					case 211:
						if (this._altRowStatus == SqlDataReader.ALTROWSTATUS.Null)
						{
							this._altMetaDataSetCollection.metaDataSet = this._metaData;
							this._metaData = null;
						}
						this._altRowStatus = SqlDataReader.ALTROWSTATUS.AltRow;
						this._hasRows = true;
						return true;
					default:
						if (b2 == 253)
						{
							this._altRowStatus = SqlDataReader.ALTROWSTATUS.Null;
							this._metaData = null;
							this._altMetaDataSetCollection = null;
							return true;
						}
						break;
					}
					this._parser.Run(RunBehavior.ReturnImmediately, this._command, this, null, this._stateObj);
				}
			}
			return false;
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x00280708 File Offset: 0x0027FB08
		private bool HasMoreRows()
		{
			if (this._parser != null)
			{
				if (this._dataReady)
				{
					return true;
				}
				switch (this._altRowStatus)
				{
				case SqlDataReader.ALTROWSTATUS.AltRow:
					return true;
				case SqlDataReader.ALTROWSTATUS.Done:
					return false;
				default:
					if (this._stateObj._pendingData)
					{
						byte b = this._stateObj.PeekByte();
						bool flag = false;
						while (b == 253 || b == 254 || b == 255 || (!flag && b == 169) || (!flag && b == 170) || (!flag && b == 171))
						{
							if (b == 253 || b == 254 || b == 255)
							{
								flag = true;
							}
							this._parser.Run(RunBehavior.ReturnImmediately, this._command, this, null, this._stateObj);
							if (!this._stateObj._pendingData)
							{
								break;
							}
							b = this._stateObj.PeekByte();
						}
						if (209 == b)
						{
							return true;
						}
					}
					break;
				}
			}
			return false;
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x002807FC File Offset: 0x0027FBFC
		public override bool IsDBNull(int i)
		{
			this.SetTimeout();
			this.ReadColumnHeader(i);
			return this._data[i].IsNull;
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x00280824 File Offset: 0x0027FC24
		protected bool IsCommandBehavior(CommandBehavior condition)
		{
			return condition == (condition & this._commandBehavior);
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x0028083C File Offset: 0x0027FC3C
		public override bool NextResult()
		{
			SqlStatistics sqlStatistics = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlDataReader.NextResult|API> %d#", this.ObjectID);
			RuntimeHelpers.PrepareConstrainedRegions();
			bool flag2;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				this.SetTimeout();
				if (this.IsClosed)
				{
					throw ADP.DataReaderClosed("NextResult");
				}
				this._fieldNameLookup = null;
				bool flag = false;
				this._hasRows = false;
				if (this.IsCommandBehavior(CommandBehavior.SingleResult))
				{
					this.CloseInternal(false);
					this.ClearMetaData();
					flag2 = flag;
				}
				else
				{
					if (this._parser != null)
					{
						while (this.ReadInternal(false))
						{
						}
					}
					if (this._parser != null)
					{
						if (this.HasMoreResults())
						{
							this._metaDataConsumed = false;
							this._browseModeInfoConsumed = false;
							switch (this._altRowStatus)
							{
							case SqlDataReader.ALTROWSTATUS.AltRow:
							{
								int altRowId = this._parser.GetAltRowId(this._stateObj);
								_SqlMetaDataSet sqlMetaDataSet = this._altMetaDataSetCollection[altRowId];
								if (sqlMetaDataSet != null)
								{
									this._metaData = sqlMetaDataSet;
									this._metaData.indexMap = sqlMetaDataSet.indexMap;
								}
								break;
							}
							case SqlDataReader.ALTROWSTATUS.Done:
								this._metaData = this._altMetaDataSetCollection.metaDataSet;
								this._altRowStatus = SqlDataReader.ALTROWSTATUS.Null;
								break;
							default:
								this.ConsumeMetaData();
								if (this._metaData == null)
								{
									return false;
								}
								break;
							}
							flag = true;
						}
						else
						{
							this.CloseInternal(false);
							this.SetMetaData(null, false);
						}
					}
					else
					{
						this.ClearMetaData();
					}
					flag2 = flag;
				}
			}
			catch (OutOfMemoryException ex)
			{
				this._isClosed = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex);
				}
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this._isClosed = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex2);
				}
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this._isClosed = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex3);
				}
				throw;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
				Bid.ScopeLeave(ref intPtr);
			}
			return flag2;
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x00280A70 File Offset: 0x0027FE70
		public override bool Read()
		{
			return this.ReadInternal(true);
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x00280A84 File Offset: 0x0027FE84
		private bool ReadInternal(bool setTimeout)
		{
			SqlStatistics sqlStatistics = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlDataReader.Read|API> %d#", this.ObjectID);
			RuntimeHelpers.PrepareConstrainedRegions();
			bool flag;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				if (this._parser != null)
				{
					if (setTimeout)
					{
						this.SetTimeout();
					}
					if (this._dataReady)
					{
						this.CleanPartialRead();
					}
					this._dataReady = false;
					SqlBuffer.Clear(this._data);
					this._nextColumnHeaderToRead = 0;
					this._nextColumnDataToRead = 0;
					this._columnDataBytesRemaining = -1L;
					if (!this._haltRead)
					{
						if (this.HasMoreRows())
						{
							while (this._stateObj._pendingData)
							{
								if (this._altRowStatus == SqlDataReader.ALTROWSTATUS.AltRow)
								{
									this._altRowStatus = SqlDataReader.ALTROWSTATUS.Done;
									this._dataReady = true;
									break;
								}
								this._dataReady = this._parser.Run(RunBehavior.ReturnImmediately, this._command, this, null, this._stateObj);
								if (this._dataReady)
								{
									break;
								}
							}
							if (this._dataReady)
							{
								this._haltRead = this.IsCommandBehavior(CommandBehavior.SingleRow);
								return true;
							}
						}
						if (!this._stateObj._pendingData)
						{
							this.CloseInternal(false);
						}
					}
					else
					{
						while (this.HasMoreRows())
						{
							while (this._stateObj._pendingData && !this._dataReady)
							{
								this._dataReady = this._parser.Run(RunBehavior.ReturnImmediately, this._command, this, null, this._stateObj);
							}
							if (this._dataReady)
							{
								this.CleanPartialRead();
							}
							this._dataReady = false;
							SqlBuffer.Clear(this._data);
							this._nextColumnHeaderToRead = 0;
						}
						this._haltRead = false;
					}
				}
				else if (this.IsClosed)
				{
					throw ADP.DataReaderClosed("Read");
				}
				flag = false;
			}
			catch (OutOfMemoryException ex)
			{
				this._isClosed = true;
				SqlConnection connection = this._connection;
				if (connection != null)
				{
					connection.Abort(ex);
				}
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this._isClosed = true;
				SqlConnection connection2 = this._connection;
				if (connection2 != null)
				{
					connection2.Abort(ex2);
				}
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this._isClosed = true;
				SqlConnection connection3 = this._connection;
				if (connection3 != null)
				{
					connection3.Abort(ex3);
				}
				throw;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
				Bid.ScopeLeave(ref intPtr);
			}
			return flag;
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x00280CF8 File Offset: 0x002800F8
		private void ReadColumn(int i)
		{
			this.ReadColumn(i, true);
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x00280D10 File Offset: 0x00280110
		private void ReadColumn(int i, bool setTimeout)
		{
			if (this.MetaData == null || !this._dataReady)
			{
				throw SQL.InvalidRead();
			}
			if (0 > i || i >= this._metaData.Length)
			{
				throw new IndexOutOfRangeException();
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				if (setTimeout)
				{
					this.SetTimeout();
				}
				if (this._nextColumnHeaderToRead <= i)
				{
					this.ReadColumnHeader(i);
				}
				if (this._nextColumnDataToRead == i)
				{
					this.ReadColumnData();
				}
				else if (this._nextColumnDataToRead > i && this.IsCommandBehavior(CommandBehavior.SequentialAccess))
				{
					throw ADP.NonSequentialColumnAccess(i, this._nextColumnDataToRead);
				}
			}
			catch (OutOfMemoryException ex)
			{
				this._isClosed = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex);
				}
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this._isClosed = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex2);
				}
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this._isClosed = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex3);
				}
				throw;
			}
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x00280E48 File Offset: 0x00280248
		private void ReadColumnData()
		{
			if (!this._data[this._nextColumnDataToRead].IsNull)
			{
				_SqlMetaData sqlMetaData = this._metaData[this._nextColumnDataToRead];
				this._parser.ReadSqlValue(this._data[this._nextColumnDataToRead], sqlMetaData, (int)this._columnDataBytesRemaining, this._stateObj);
				this._columnDataBytesRemaining = 0L;
			}
			this._nextColumnDataToRead++;
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x00280EB8 File Offset: 0x002802B8
		private void ReadColumnHeader(int i)
		{
			if (!this._dataReady)
			{
				throw SQL.InvalidRead();
			}
			if (i < this._nextColumnDataToRead)
			{
				return;
			}
			bool flag = this.IsCommandBehavior(CommandBehavior.SequentialAccess);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				if (flag)
				{
					if (0 < this._nextColumnDataToRead)
					{
						this._data[this._nextColumnDataToRead - 1].Clear();
					}
				}
				else if (this._nextColumnDataToRead < this._nextColumnHeaderToRead)
				{
					this.ReadColumnData();
				}
				while (this._nextColumnHeaderToRead <= i)
				{
					this.ResetBlobState();
					if (flag)
					{
						flag = this._nextColumnHeaderToRead < i;
					}
					_SqlMetaData sqlMetaData = this._metaData[this._nextColumnHeaderToRead];
					if (flag && sqlMetaData.metaType.IsPlp)
					{
						this._parser.SkipPlpValue(ulong.MaxValue, this._stateObj);
						this._nextColumnDataToRead = this._nextColumnHeaderToRead;
						this._nextColumnHeaderToRead++;
						this._columnDataBytesRemaining = 0L;
					}
					else
					{
						bool flag2 = false;
						ulong num = this._parser.ProcessColumnHeader(sqlMetaData, this._stateObj, out flag2);
						this._nextColumnDataToRead = this._nextColumnHeaderToRead;
						this._nextColumnHeaderToRead++;
						if (flag)
						{
							this._parser.SkipLongBytes(num, this._stateObj);
							this._columnDataBytesRemaining = 0L;
						}
						else if (flag2)
						{
							this._parser.GetNullSqlValue(this._data[this._nextColumnDataToRead], sqlMetaData);
							this._columnDataBytesRemaining = 0L;
						}
						else
						{
							this._columnDataBytesRemaining = (long)num;
							if (i > this._nextColumnDataToRead)
							{
								this.ReadColumnData();
							}
						}
					}
				}
			}
			catch (OutOfMemoryException ex)
			{
				this._isClosed = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex);
				}
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this._isClosed = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex2);
				}
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this._isClosed = true;
				if (this._connection != null)
				{
					this._connection.Abort(ex3);
				}
				throw;
			}
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x002810EC File Offset: 0x002804EC
		private void ResetBlobState()
		{
			int num = this._nextColumnHeaderToRead - 1;
			if (num >= 0 && this._metaData[num].metaType.IsPlp)
			{
				if (this._stateObj._longlen != 0UL)
				{
					this._stateObj.Parser.SkipPlpValue(ulong.MaxValue, this._stateObj);
				}
				if (this._streamingXml != null)
				{
					SqlStreamingXml streamingXml = this._streamingXml;
					this._streamingXml = null;
					streamingXml.Close();
				}
			}
			else if (0L < this._columnDataBytesRemaining)
			{
				this._stateObj.Parser.SkipLongBytes((ulong)this._columnDataBytesRemaining, this._stateObj);
			}
			this._columnDataBytesRemaining = -1L;
			this._columnDataBytesRead = 0L;
			this._columnDataCharsRead = 0L;
			this._columnDataChars = null;
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x002811AC File Offset: 0x002805AC
		private void RestoreServerSettings(TdsParser parser, TdsParserStateObject stateObj)
		{
			if (parser != null && this._resetOptionsString != null)
			{
				if (parser.State == TdsParserState.OpenLoggedIn)
				{
					parser.TdsExecuteSQLBatch(this._resetOptionsString, (this._command != null) ? this._command.CommandTimeout : 0, null, stateObj);
					parser.Run(RunBehavior.UntilDone, this._command, this, null, stateObj);
				}
				this._resetOptionsString = null;
			}
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x0028120C File Offset: 0x0028060C
		internal void SetAltMetaDataSet(_SqlMetaDataSet metaDataSet, bool metaDataConsumed)
		{
			if (this._altMetaDataSetCollection == null)
			{
				this._altMetaDataSetCollection = new _SqlMetaDataSetCollection();
			}
			this._altMetaDataSetCollection.Add(metaDataSet);
			this._metaDataConsumed = metaDataConsumed;
			if (this._metaDataConsumed)
			{
				byte b = this._stateObj.PeekByte();
				if (169 == b)
				{
					this._parser.Run(RunBehavior.ReturnImmediately, this._command, this, null, this._stateObj);
					b = this._stateObj.PeekByte();
				}
				this._hasRows = 209 == b;
			}
			if (metaDataSet != null && (this._data == null || this._data.Length < metaDataSet.Length))
			{
				this._data = SqlBuffer.CreateBufferArray(metaDataSet.Length);
			}
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x002812BC File Offset: 0x002806BC
		private void ClearMetaData()
		{
			this._metaData = null;
			this._tableNames = null;
			this._fieldNameLookup = null;
			this._metaDataConsumed = false;
			this._browseModeInfoConsumed = false;
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x002812EC File Offset: 0x002806EC
		internal void SetMetaData(_SqlMetaDataSet metaData, bool moreInfo)
		{
			this._metaData = metaData;
			this._tableNames = null;
			if (this._metaData != null)
			{
				this._metaData.schemaTable = null;
				this._data = SqlBuffer.CreateBufferArray(metaData.Length);
			}
			this._fieldNameLookup = null;
			if (metaData != null)
			{
				if (!moreInfo)
				{
					this._metaDataConsumed = true;
					if (this._parser != null)
					{
						byte b = this._stateObj.PeekByte();
						if (b == 169)
						{
							this._parser.Run(RunBehavior.ReturnImmediately, null, null, null, this._stateObj);
							b = this._stateObj.PeekByte();
						}
						this._hasRows = 209 == b;
						if (136 == b)
						{
							this._metaDataConsumed = false;
						}
					}
				}
			}
			else
			{
				this._metaDataConsumed = false;
			}
			this._browseModeInfoConsumed = false;
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x002813AC File Offset: 0x002807AC
		private void SetTimeout()
		{
			TdsParserStateObject stateObj = this._stateObj;
			if (stateObj != null)
			{
				stateObj.SetTimeoutSeconds(this._timeoutSeconds);
			}
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x002813D0 File Offset: 0x002807D0
		internal object GetSqlValueWithNoConvert(int i)
		{
			if (this.MetaData == null || !this._dataReady)
			{
				throw SQL.InvalidRead();
			}
			this.ReadColumn(i, false);
			object obj;
			if (this._metaData[i].type == SqlDbType.Xml)
			{
				obj = this._data[i].SqlCachedBuffer;
			}
			else
			{
				obj = this._data[i].SqlValue;
			}
			return obj;
		}

		// Token: 0x04001804 RID: 6148
		private TdsParser _parser;

		// Token: 0x04001805 RID: 6149
		private TdsParserStateObject _stateObj;

		// Token: 0x04001806 RID: 6150
		private SqlCommand _command;

		// Token: 0x04001807 RID: 6151
		private SqlConnection _connection;

		// Token: 0x04001808 RID: 6152
		private int _defaultLCID;

		// Token: 0x04001809 RID: 6153
		private bool _dataReady;

		// Token: 0x0400180A RID: 6154
		private bool _haltRead;

		// Token: 0x0400180B RID: 6155
		private bool _metaDataConsumed;

		// Token: 0x0400180C RID: 6156
		private bool _browseModeInfoConsumed;

		// Token: 0x0400180D RID: 6157
		private bool _isClosed;

		// Token: 0x0400180E RID: 6158
		private bool _isInitialized;

		// Token: 0x0400180F RID: 6159
		private bool _hasRows;

		// Token: 0x04001810 RID: 6160
		private SqlDataReader.ALTROWSTATUS _altRowStatus;

		// Token: 0x04001811 RID: 6161
		private int _recordsAffected = -1;

		// Token: 0x04001812 RID: 6162
		private int _timeoutSeconds;

		// Token: 0x04001813 RID: 6163
		private SqlConnectionString.TypeSystem _typeSystem;

		// Token: 0x04001814 RID: 6164
		private SqlStatistics _statistics;

		// Token: 0x04001815 RID: 6165
		private SqlBuffer[] _data;

		// Token: 0x04001816 RID: 6166
		private SqlStreamingXml _streamingXml;

		// Token: 0x04001817 RID: 6167
		private _SqlMetaDataSet _metaData;

		// Token: 0x04001818 RID: 6168
		private _SqlMetaDataSetCollection _altMetaDataSetCollection;

		// Token: 0x04001819 RID: 6169
		private FieldNameLookup _fieldNameLookup;

		// Token: 0x0400181A RID: 6170
		private CommandBehavior _commandBehavior;

		// Token: 0x0400181B RID: 6171
		private static int _objectTypeCount;

		// Token: 0x0400181C RID: 6172
		internal readonly int ObjectID = Interlocked.Increment(ref SqlDataReader._objectTypeCount);

		// Token: 0x0400181D RID: 6173
		private MultiPartTableName[] _tableNames;

		// Token: 0x0400181E RID: 6174
		private string _resetOptionsString;

		// Token: 0x0400181F RID: 6175
		private int _nextColumnDataToRead;

		// Token: 0x04001820 RID: 6176
		private int _nextColumnHeaderToRead;

		// Token: 0x04001821 RID: 6177
		private long _columnDataBytesRead;

		// Token: 0x04001822 RID: 6178
		private long _columnDataBytesRemaining;

		// Token: 0x04001823 RID: 6179
		private long _columnDataCharsRead;

		// Token: 0x04001824 RID: 6180
		private char[] _columnDataChars;

		// Token: 0x04001825 RID: 6181
		private Exception _rowException;

		// Token: 0x020002DF RID: 735
		private enum ALTROWSTATUS
		{
			// Token: 0x04001827 RID: 6183
			Null,
			// Token: 0x04001828 RID: 6184
			AltRow,
			// Token: 0x04001829 RID: 6185
			Done
		}
	}
}
