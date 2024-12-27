using System;
using System.Collections;
using System.Data.Common;
using System.Globalization;

namespace System.Data
{
	// Token: 0x020000A3 RID: 163
	public sealed class DataTableReader : DbDataReader
	{
		// Token: 0x06000AE0 RID: 2784 RVA: 0x001F4660 File Offset: 0x001F3A60
		public DataTableReader(DataTable dataTable)
		{
			if (dataTable == null)
			{
				throw ExceptionBuilder.ArgumentNull("DataTable");
			}
			this.tables = new DataTable[] { dataTable };
			this.init();
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x001F46B8 File Offset: 0x001F3AB8
		public DataTableReader(DataTable[] dataTables)
		{
			if (dataTables == null)
			{
				throw ExceptionBuilder.ArgumentNull("DataTable");
			}
			if (dataTables.Length == 0)
			{
				throw ExceptionBuilder.DataTableReaderArgumentIsEmpty();
			}
			this.tables = new DataTable[dataTables.Length];
			for (int i = 0; i < dataTables.Length; i++)
			{
				if (dataTables[i] == null)
				{
					throw ExceptionBuilder.ArgumentNull("DataTable");
				}
				this.tables[i] = dataTables[i];
			}
			this.init();
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x001F4740 File Offset: 0x001F3B40
		// (set) Token: 0x06000AE3 RID: 2787 RVA: 0x001F4754 File Offset: 0x001F3B54
		private bool ReaderIsInvalid
		{
			get
			{
				return this.readerIsInvalid;
			}
			set
			{
				if (this.readerIsInvalid == value)
				{
					return;
				}
				this.readerIsInvalid = value;
				if (this.readerIsInvalid && this.listener != null)
				{
					this.listener.CleanUp();
				}
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x001F4790 File Offset: 0x001F3B90
		// (set) Token: 0x06000AE5 RID: 2789 RVA: 0x001F47A4 File Offset: 0x001F3BA4
		private bool IsSchemaChanged
		{
			get
			{
				return this.schemaIsChanged;
			}
			set
			{
				if (!value || this.schemaIsChanged == value)
				{
					return;
				}
				this.schemaIsChanged = value;
				if (this.listener != null)
				{
					this.listener.CleanUp();
				}
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x001F47D8 File Offset: 0x001F3BD8
		internal DataTable CurrentDataTable
		{
			get
			{
				return this.currentDataTable;
			}
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x001F47EC File Offset: 0x001F3BEC
		private void init()
		{
			this.tableCounter = 0;
			this.reachEORows = false;
			this.schemaIsChanged = false;
			this.currentDataTable = this.tables[this.tableCounter];
			this.hasRows = this.currentDataTable.Rows.Count > 0;
			this.ReaderIsInvalid = false;
			this.listener = new DataTableReaderListener(this);
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x001F4850 File Offset: 0x001F3C50
		public override void Close()
		{
			if (!this.isOpen)
			{
				return;
			}
			if (this.listener != null)
			{
				this.listener.CleanUp();
			}
			this.listener = null;
			this.schemaTable = null;
			this.isOpen = false;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x001F4890 File Offset: 0x001F3C90
		public override DataTable GetSchemaTable()
		{
			this.ValidateOpen("GetSchemaTable");
			this.ValidateReader();
			if (this.schemaTable == null)
			{
				this.schemaTable = DataTableReader.GetSchemaTableFromDataTable(this.currentDataTable);
			}
			return this.schemaTable;
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x001F48D0 File Offset: 0x001F3CD0
		public override bool NextResult()
		{
			this.ValidateOpen("NextResult");
			if (this.tableCounter == this.tables.Length - 1)
			{
				return false;
			}
			this.currentDataTable = this.tables[++this.tableCounter];
			if (this.listener != null)
			{
				this.listener.UpdataTable(this.currentDataTable);
			}
			this.schemaTable = null;
			this.rowCounter = -1;
			this.currentRowRemoved = false;
			this.reachEORows = false;
			this.schemaIsChanged = false;
			this.started = false;
			this.ReaderIsInvalid = false;
			this.tableCleared = false;
			this.hasRows = this.currentDataTable.Rows.Count > 0;
			return true;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x001F4988 File Offset: 0x001F3D88
		public override bool Read()
		{
			if (!this.started)
			{
				this.started = true;
			}
			this.ValidateOpen("Read");
			this.ValidateReader();
			if (this.reachEORows)
			{
				return false;
			}
			if (this.rowCounter >= this.currentDataTable.Rows.Count - 1)
			{
				this.reachEORows = true;
				if (this.listener != null)
				{
					this.listener.CleanUp();
				}
				return false;
			}
			this.rowCounter++;
			this.ValidateRow(this.rowCounter);
			this.currentDataRow = this.currentDataTable.Rows[this.rowCounter];
			while (this.currentDataRow.RowState == DataRowState.Deleted)
			{
				this.rowCounter++;
				if (this.rowCounter == this.currentDataTable.Rows.Count)
				{
					this.reachEORows = true;
					if (this.listener != null)
					{
						this.listener.CleanUp();
					}
					return false;
				}
				this.ValidateRow(this.rowCounter);
				this.currentDataRow = this.currentDataTable.Rows[this.rowCounter];
			}
			if (this.currentRowRemoved)
			{
				this.currentRowRemoved = false;
			}
			return true;
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x001F4AB8 File Offset: 0x001F3EB8
		public override int Depth
		{
			get
			{
				this.ValidateOpen("Depth");
				this.ValidateReader();
				return 0;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x001F4AD8 File Offset: 0x001F3ED8
		public override bool IsClosed
		{
			get
			{
				return !this.isOpen;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x001F4AF0 File Offset: 0x001F3EF0
		public override int RecordsAffected
		{
			get
			{
				this.ValidateReader();
				return 0;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x001F4B04 File Offset: 0x001F3F04
		public override bool HasRows
		{
			get
			{
				this.ValidateOpen("HasRows");
				this.ValidateReader();
				return this.hasRows;
			}
		}

		// Token: 0x1700016C RID: 364
		public override object this[int ordinal]
		{
			get
			{
				this.ValidateOpen("Item");
				this.ValidateReader();
				if (this.currentDataRow == null || this.currentDataRow.RowState == DataRowState.Deleted)
				{
					this.ReaderIsInvalid = true;
					throw ExceptionBuilder.InvalidDataTableReader(this.currentDataTable.TableName);
				}
				object obj;
				try
				{
					obj = this.currentDataRow[ordinal];
				}
				catch (IndexOutOfRangeException ex)
				{
					ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
					throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
				}
				return obj;
			}
		}

		// Token: 0x1700016D RID: 365
		public override object this[string name]
		{
			get
			{
				this.ValidateOpen("Item");
				this.ValidateReader();
				if (this.currentDataRow == null || this.currentDataRow.RowState == DataRowState.Deleted)
				{
					this.ReaderIsInvalid = true;
					throw ExceptionBuilder.InvalidDataTableReader(this.currentDataTable.TableName);
				}
				return this.currentDataRow[name];
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x001F4C10 File Offset: 0x001F4010
		public override int FieldCount
		{
			get
			{
				this.ValidateOpen("FieldCount");
				this.ValidateReader();
				return this.currentDataTable.Columns.Count;
			}
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x001F4C40 File Offset: 0x001F4040
		public override Type GetProviderSpecificFieldType(int ordinal)
		{
			this.ValidateOpen("GetProviderSpecificFieldType");
			this.ValidateReader();
			return this.GetFieldType(ordinal);
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x001F4C68 File Offset: 0x001F4068
		public override object GetProviderSpecificValue(int ordinal)
		{
			this.ValidateOpen("GetProviderSpecificValue");
			this.ValidateReader();
			return this.GetValue(ordinal);
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x001F4C90 File Offset: 0x001F4090
		public override int GetProviderSpecificValues(object[] values)
		{
			this.ValidateOpen("GetProviderSpecificValues");
			this.ValidateReader();
			return this.GetValues(values);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x001F4CB8 File Offset: 0x001F40B8
		public override bool GetBoolean(int ordinal)
		{
			this.ValidateState("GetBoolean");
			this.ValidateReader();
			bool flag;
			try
			{
				flag = (bool)this.currentDataRow[ordinal];
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return flag;
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x001F4D1C File Offset: 0x001F411C
		public override byte GetByte(int ordinal)
		{
			this.ValidateState("GetByte");
			this.ValidateReader();
			byte b;
			try
			{
				b = (byte)this.currentDataRow[ordinal];
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return b;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x001F4D80 File Offset: 0x001F4180
		public override long GetBytes(int ordinal, long dataIndex, byte[] buffer, int bufferIndex, int length)
		{
			this.ValidateState("GetBytes");
			this.ValidateReader();
			byte[] array;
			try
			{
				array = (byte[])this.currentDataRow[ordinal];
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			if (buffer == null)
			{
				return (long)array.Length;
			}
			int num = (int)dataIndex;
			int num2 = Math.Min(array.Length - num, length);
			if (num < 0)
			{
				throw ADP.InvalidSourceBufferIndex(array.Length, (long)num, "dataIndex");
			}
			if (bufferIndex < 0 || (bufferIndex > 0 && bufferIndex >= buffer.Length))
			{
				throw ADP.InvalidDestinationBufferIndex(buffer.Length, bufferIndex, "bufferIndex");
			}
			if (0 < num2)
			{
				Array.Copy(array, dataIndex, buffer, (long)bufferIndex, (long)num2);
			}
			else
			{
				if (length < 0)
				{
					throw ADP.InvalidDataLength((long)length);
				}
				num2 = 0;
			}
			return (long)num2;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x001F4E54 File Offset: 0x001F4254
		public override char GetChar(int ordinal)
		{
			this.ValidateState("GetChar");
			this.ValidateReader();
			char c;
			try
			{
				c = (char)this.currentDataRow[ordinal];
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return c;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x001F4EB8 File Offset: 0x001F42B8
		public override long GetChars(int ordinal, long dataIndex, char[] buffer, int bufferIndex, int length)
		{
			this.ValidateState("GetChars");
			this.ValidateReader();
			char[] array;
			try
			{
				array = (char[])this.currentDataRow[ordinal];
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			if (buffer == null)
			{
				return (long)array.Length;
			}
			int num = (int)dataIndex;
			int num2 = Math.Min(array.Length - num, length);
			if (num < 0)
			{
				throw ADP.InvalidSourceBufferIndex(array.Length, (long)num, "dataIndex");
			}
			if (bufferIndex < 0 || (bufferIndex > 0 && bufferIndex >= buffer.Length))
			{
				throw ADP.InvalidDestinationBufferIndex(buffer.Length, bufferIndex, "bufferIndex");
			}
			if (0 < num2)
			{
				Array.Copy(array, dataIndex, buffer, (long)bufferIndex, (long)num2);
			}
			else
			{
				if (length < 0)
				{
					throw ADP.InvalidDataLength((long)length);
				}
				num2 = 0;
			}
			return (long)num2;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x001F4F8C File Offset: 0x001F438C
		public override string GetDataTypeName(int ordinal)
		{
			this.ValidateOpen("GetDataTypeName");
			this.ValidateReader();
			return this.GetFieldType(ordinal).Name;
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x001F4FB8 File Offset: 0x001F43B8
		public override DateTime GetDateTime(int ordinal)
		{
			this.ValidateState("GetDateTime");
			this.ValidateReader();
			DateTime dateTime;
			try
			{
				dateTime = (DateTime)this.currentDataRow[ordinal];
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return dateTime;
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x001F501C File Offset: 0x001F441C
		public override decimal GetDecimal(int ordinal)
		{
			this.ValidateState("GetDecimal");
			this.ValidateReader();
			decimal num;
			try
			{
				num = (decimal)this.currentDataRow[ordinal];
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return num;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x001F5080 File Offset: 0x001F4480
		public override double GetDouble(int ordinal)
		{
			this.ValidateState("GetDouble");
			this.ValidateReader();
			double num;
			try
			{
				num = (double)this.currentDataRow[ordinal];
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return num;
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x001F50E4 File Offset: 0x001F44E4
		public override Type GetFieldType(int ordinal)
		{
			this.ValidateOpen("GetFieldType");
			this.ValidateReader();
			Type dataType;
			try
			{
				dataType = this.currentDataTable.Columns[ordinal].DataType;
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return dataType;
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x001F514C File Offset: 0x001F454C
		public override float GetFloat(int ordinal)
		{
			this.ValidateState("GetFloat");
			this.ValidateReader();
			float num;
			try
			{
				num = (float)this.currentDataRow[ordinal];
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return num;
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x001F51B0 File Offset: 0x001F45B0
		public override Guid GetGuid(int ordinal)
		{
			this.ValidateState("GetGuid");
			this.ValidateReader();
			Guid guid;
			try
			{
				guid = (Guid)this.currentDataRow[ordinal];
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return guid;
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x001F5214 File Offset: 0x001F4614
		public override short GetInt16(int ordinal)
		{
			this.ValidateState("GetInt16");
			this.ValidateReader();
			short num;
			try
			{
				num = (short)this.currentDataRow[ordinal];
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return num;
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x001F5278 File Offset: 0x001F4678
		public override int GetInt32(int ordinal)
		{
			this.ValidateState("GetInt32");
			this.ValidateReader();
			int num;
			try
			{
				num = (int)this.currentDataRow[ordinal];
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return num;
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x001F52DC File Offset: 0x001F46DC
		public override long GetInt64(int ordinal)
		{
			this.ValidateState("GetInt64");
			this.ValidateReader();
			long num;
			try
			{
				num = (long)this.currentDataRow[ordinal];
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return num;
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x001F5340 File Offset: 0x001F4740
		public override string GetName(int ordinal)
		{
			this.ValidateOpen("GetName");
			this.ValidateReader();
			string columnName;
			try
			{
				columnName = this.currentDataTable.Columns[ordinal].ColumnName;
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return columnName;
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x001F53A8 File Offset: 0x001F47A8
		public override int GetOrdinal(string name)
		{
			this.ValidateOpen("GetOrdinal");
			this.ValidateReader();
			DataColumn dataColumn = this.currentDataTable.Columns[name];
			if (dataColumn != null)
			{
				return dataColumn.Ordinal;
			}
			throw ExceptionBuilder.ColumnNotInTheTable(name, this.currentDataTable.TableName);
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x001F53F4 File Offset: 0x001F47F4
		public override string GetString(int ordinal)
		{
			this.ValidateState("GetString");
			this.ValidateReader();
			string text;
			try
			{
				text = (string)this.currentDataRow[ordinal];
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return text;
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x001F5458 File Offset: 0x001F4858
		public override object GetValue(int ordinal)
		{
			this.ValidateState("GetValue");
			this.ValidateReader();
			object obj;
			try
			{
				obj = this.currentDataRow[ordinal];
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return obj;
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x001F54B8 File Offset: 0x001F48B8
		public override int GetValues(object[] values)
		{
			this.ValidateState("GetValues");
			this.ValidateReader();
			if (values == null)
			{
				throw ExceptionBuilder.ArgumentNull("values");
			}
			Array.Copy(this.currentDataRow.ItemArray, values, (this.currentDataRow.ItemArray.Length > values.Length) ? values.Length : this.currentDataRow.ItemArray.Length);
			if (this.currentDataRow.ItemArray.Length <= values.Length)
			{
				return this.currentDataRow.ItemArray.Length;
			}
			return values.Length;
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x001F553C File Offset: 0x001F493C
		public override bool IsDBNull(int ordinal)
		{
			this.ValidateState("IsDBNull");
			this.ValidateReader();
			bool flag;
			try
			{
				flag = this.currentDataRow.IsNull(ordinal);
			}
			catch (IndexOutOfRangeException ex)
			{
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				throw ExceptionBuilder.ArgumentOutOfRange("ordinal");
			}
			return flag;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x001F559C File Offset: 0x001F499C
		public override IEnumerator GetEnumerator()
		{
			this.ValidateOpen("GetEnumerator");
			return new DbEnumerator(this);
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x001F55BC File Offset: 0x001F49BC
		internal static DataTable GetSchemaTableFromDataTable(DataTable table)
		{
			if (table == null)
			{
				throw ExceptionBuilder.ArgumentNull("DataTable");
			}
			DataTable dataTable = new DataTable("SchemaTable");
			dataTable.Locale = CultureInfo.InvariantCulture;
			DataColumn dataColumn = new DataColumn(SchemaTableColumn.ColumnName, typeof(string));
			DataColumn dataColumn2 = new DataColumn(SchemaTableColumn.ColumnOrdinal, typeof(int));
			DataColumn dataColumn3 = new DataColumn(SchemaTableColumn.ColumnSize, typeof(int));
			DataColumn dataColumn4 = new DataColumn(SchemaTableColumn.NumericPrecision, typeof(short));
			DataColumn dataColumn5 = new DataColumn(SchemaTableColumn.NumericScale, typeof(short));
			DataColumn dataColumn6 = new DataColumn(SchemaTableColumn.DataType, typeof(Type));
			DataColumn dataColumn7 = new DataColumn(SchemaTableColumn.ProviderType, typeof(int));
			DataColumn dataColumn8 = new DataColumn(SchemaTableColumn.IsLong, typeof(bool));
			DataColumn dataColumn9 = new DataColumn(SchemaTableColumn.AllowDBNull, typeof(bool));
			DataColumn dataColumn10 = new DataColumn(SchemaTableOptionalColumn.IsReadOnly, typeof(bool));
			DataColumn dataColumn11 = new DataColumn(SchemaTableOptionalColumn.IsRowVersion, typeof(bool));
			DataColumn dataColumn12 = new DataColumn(SchemaTableColumn.IsUnique, typeof(bool));
			DataColumn dataColumn13 = new DataColumn(SchemaTableColumn.IsKey, typeof(bool));
			DataColumn dataColumn14 = new DataColumn(SchemaTableOptionalColumn.IsAutoIncrement, typeof(bool));
			DataColumn dataColumn15 = new DataColumn(SchemaTableColumn.BaseSchemaName, typeof(string));
			DataColumn dataColumn16 = new DataColumn(SchemaTableOptionalColumn.BaseCatalogName, typeof(string));
			DataColumn dataColumn17 = new DataColumn(SchemaTableColumn.BaseTableName, typeof(string));
			DataColumn dataColumn18 = new DataColumn(SchemaTableColumn.BaseColumnName, typeof(string));
			DataColumn dataColumn19 = new DataColumn(SchemaTableOptionalColumn.AutoIncrementSeed, typeof(long));
			DataColumn dataColumn20 = new DataColumn(SchemaTableOptionalColumn.AutoIncrementStep, typeof(long));
			DataColumn dataColumn21 = new DataColumn(SchemaTableOptionalColumn.DefaultValue, typeof(object));
			DataColumn dataColumn22 = new DataColumn(SchemaTableOptionalColumn.Expression, typeof(string));
			DataColumn dataColumn23 = new DataColumn(SchemaTableOptionalColumn.ColumnMapping, typeof(MappingType));
			DataColumn dataColumn24 = new DataColumn(SchemaTableOptionalColumn.BaseTableNamespace, typeof(string));
			DataColumn dataColumn25 = new DataColumn(SchemaTableOptionalColumn.BaseColumnNamespace, typeof(string));
			dataColumn3.DefaultValue = -1;
			if (table.DataSet != null)
			{
				dataColumn16.DefaultValue = table.DataSet.DataSetName;
			}
			dataColumn17.DefaultValue = table.TableName;
			dataColumn24.DefaultValue = table.Namespace;
			dataColumn11.DefaultValue = false;
			dataColumn8.DefaultValue = false;
			dataColumn10.DefaultValue = false;
			dataColumn13.DefaultValue = false;
			dataColumn14.DefaultValue = false;
			dataColumn19.DefaultValue = 0;
			dataColumn20.DefaultValue = 1;
			dataTable.Columns.Add(dataColumn);
			dataTable.Columns.Add(dataColumn2);
			dataTable.Columns.Add(dataColumn3);
			dataTable.Columns.Add(dataColumn4);
			dataTable.Columns.Add(dataColumn5);
			dataTable.Columns.Add(dataColumn6);
			dataTable.Columns.Add(dataColumn7);
			dataTable.Columns.Add(dataColumn8);
			dataTable.Columns.Add(dataColumn9);
			dataTable.Columns.Add(dataColumn10);
			dataTable.Columns.Add(dataColumn11);
			dataTable.Columns.Add(dataColumn12);
			dataTable.Columns.Add(dataColumn13);
			dataTable.Columns.Add(dataColumn14);
			dataTable.Columns.Add(dataColumn16);
			dataTable.Columns.Add(dataColumn15);
			dataTable.Columns.Add(dataColumn17);
			dataTable.Columns.Add(dataColumn18);
			dataTable.Columns.Add(dataColumn19);
			dataTable.Columns.Add(dataColumn20);
			dataTable.Columns.Add(dataColumn21);
			dataTable.Columns.Add(dataColumn22);
			dataTable.Columns.Add(dataColumn23);
			dataTable.Columns.Add(dataColumn24);
			dataTable.Columns.Add(dataColumn25);
			foreach (object obj in table.Columns)
			{
				DataColumn dataColumn26 = (DataColumn)obj;
				DataRow dataRow = dataTable.NewRow();
				dataRow[dataColumn] = dataColumn26.ColumnName;
				dataRow[dataColumn2] = dataColumn26.Ordinal;
				dataRow[dataColumn6] = dataColumn26.DataType;
				if (dataColumn26.DataType == typeof(string))
				{
					dataRow[dataColumn3] = dataColumn26.MaxLength;
				}
				dataRow[dataColumn9] = dataColumn26.AllowDBNull;
				dataRow[dataColumn10] = dataColumn26.ReadOnly;
				dataRow[dataColumn12] = dataColumn26.Unique;
				if (dataColumn26.AutoIncrement)
				{
					dataRow[dataColumn14] = true;
					dataRow[dataColumn19] = dataColumn26.AutoIncrementSeed;
					dataRow[dataColumn20] = dataColumn26.AutoIncrementStep;
				}
				if (dataColumn26.DefaultValue != DBNull.Value)
				{
					dataRow[dataColumn21] = dataColumn26.DefaultValue;
				}
				if (dataColumn26.Expression.Length != 0)
				{
					bool flag = false;
					DataColumn[] dependency = dataColumn26.DataExpression.GetDependency();
					for (int i = 0; i < dependency.Length; i++)
					{
						if (dependency[i].Table != table)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						dataRow[dataColumn22] = dataColumn26.Expression;
					}
				}
				dataRow[dataColumn23] = dataColumn26.ColumnMapping;
				dataRow[dataColumn18] = dataColumn26.ColumnName;
				dataRow[dataColumn25] = dataColumn26.Namespace;
				dataTable.Rows.Add(dataRow);
			}
			foreach (DataColumn dataColumn27 in table.PrimaryKey)
			{
				dataTable.Rows[dataColumn27.Ordinal][dataColumn13] = true;
			}
			dataTable.AcceptChanges();
			return dataTable;
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x001F5C10 File Offset: 0x001F5010
		private void ValidateOpen(string caller)
		{
			if (!this.isOpen)
			{
				throw ADP.DataReaderClosed(caller);
			}
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x001F5C2C File Offset: 0x001F502C
		private void ValidateReader()
		{
			if (this.ReaderIsInvalid)
			{
				throw ExceptionBuilder.InvalidDataTableReader(this.currentDataTable.TableName);
			}
			if (this.IsSchemaChanged)
			{
				throw ExceptionBuilder.DataTableReaderSchemaIsInvalid(this.currentDataTable.TableName);
			}
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x001F5C6C File Offset: 0x001F506C
		private void ValidateState(string caller)
		{
			this.ValidateOpen(caller);
			if (this.tableCleared)
			{
				throw ExceptionBuilder.EmptyDataTableReader(this.currentDataTable.TableName);
			}
			if (this.currentDataRow == null || this.currentDataTable == null)
			{
				this.ReaderIsInvalid = true;
				throw ExceptionBuilder.InvalidDataTableReader(this.currentDataTable.TableName);
			}
			if (this.currentDataRow.RowState == DataRowState.Deleted || this.currentDataRow.RowState == DataRowState.Detached || this.currentRowRemoved)
			{
				throw ExceptionBuilder.InvalidCurrentRowInDataTableReader();
			}
			if (0 > this.rowCounter || this.currentDataTable.Rows.Count <= this.rowCounter)
			{
				this.ReaderIsInvalid = true;
				throw ExceptionBuilder.InvalidDataTableReader(this.currentDataTable.TableName);
			}
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x001F5D24 File Offset: 0x001F5124
		private void ValidateRow(int rowPosition)
		{
			if (this.ReaderIsInvalid)
			{
				throw ExceptionBuilder.InvalidDataTableReader(this.currentDataTable.TableName);
			}
			if (0 > rowPosition || this.currentDataTable.Rows.Count <= rowPosition)
			{
				this.ReaderIsInvalid = true;
				throw ExceptionBuilder.InvalidDataTableReader(this.currentDataTable.TableName);
			}
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x001F5D7C File Offset: 0x001F517C
		internal void SchemaChanged()
		{
			this.IsSchemaChanged = true;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x001F5D90 File Offset: 0x001F5190
		internal void DataTableCleared()
		{
			if (!this.started)
			{
				return;
			}
			this.rowCounter = -1;
			if (!this.reachEORows)
			{
				this.currentRowRemoved = true;
			}
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x001F5DBC File Offset: 0x001F51BC
		internal void DataChanged(DataRowChangeEventArgs args)
		{
			if (!this.started || (this.rowCounter == -1 && !this.tableCleared))
			{
				return;
			}
			DataRowAction action = args.Action;
			if (action <= DataRowAction.Rollback)
			{
				if (action != DataRowAction.Delete && action != DataRowAction.Rollback)
				{
					return;
				}
			}
			else if (action != DataRowAction.Commit)
			{
				if (action != DataRowAction.Add)
				{
					return;
				}
				this.ValidateRow(this.rowCounter + 1);
				if (this.currentDataRow == this.currentDataTable.Rows[this.rowCounter + 1])
				{
					this.rowCounter++;
					return;
				}
				return;
			}
			if (args.Row.RowState == DataRowState.Detached)
			{
				if (args.Row != this.currentDataRow)
				{
					if (this.rowCounter == 0)
					{
						return;
					}
					this.ValidateRow(this.rowCounter - 1);
					if (this.currentDataRow == this.currentDataTable.Rows[this.rowCounter - 1])
					{
						this.rowCounter--;
						return;
					}
				}
				else
				{
					this.currentRowRemoved = true;
					if (this.rowCounter > 0)
					{
						this.rowCounter--;
						this.currentDataRow = this.currentDataTable.Rows[this.rowCounter];
						return;
					}
					this.rowCounter = -1;
					this.currentDataRow = null;
				}
			}
		}

		// Token: 0x04000818 RID: 2072
		private readonly DataTable[] tables;

		// Token: 0x04000819 RID: 2073
		private bool isOpen = true;

		// Token: 0x0400081A RID: 2074
		private DataTable schemaTable;

		// Token: 0x0400081B RID: 2075
		private int tableCounter = -1;

		// Token: 0x0400081C RID: 2076
		private int rowCounter = -1;

		// Token: 0x0400081D RID: 2077
		private DataTable currentDataTable;

		// Token: 0x0400081E RID: 2078
		private DataRow currentDataRow;

		// Token: 0x0400081F RID: 2079
		private bool hasRows = true;

		// Token: 0x04000820 RID: 2080
		private bool reachEORows;

		// Token: 0x04000821 RID: 2081
		private bool currentRowRemoved;

		// Token: 0x04000822 RID: 2082
		private bool schemaIsChanged;

		// Token: 0x04000823 RID: 2083
		private bool started;

		// Token: 0x04000824 RID: 2084
		private bool readerIsInvalid;

		// Token: 0x04000825 RID: 2085
		private DataTableReaderListener listener;

		// Token: 0x04000826 RID: 2086
		private bool tableCleared;
	}
}
