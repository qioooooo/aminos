using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Data.OleDb
{
	// Token: 0x02000222 RID: 546
	public sealed class OleDbDataReader : DbDataReader
	{
		// Token: 0x06001F26 RID: 7974 RVA: 0x0025AE4C File Offset: 0x0025A24C
		internal OleDbDataReader(OleDbConnection connection, OleDbCommand command, int depth, CommandBehavior commandBehavior)
		{
			this._connection = connection;
			this._command = command;
			this._commandBehavior = commandBehavior;
			if (command != null && this._depth == 0)
			{
				this._parameterBindings = command.TakeBindingOwnerShip();
			}
			this._depth = depth;
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x0025AEBC File Offset: 0x0025A2BC
		private void Initialize()
		{
			CommandBehavior commandBehavior = this._commandBehavior;
			this._useIColumnsRowset = CommandBehavior.Default != (CommandBehavior.KeyInfo & commandBehavior);
			this._sequentialAccess = CommandBehavior.Default != (CommandBehavior.SequentialAccess & commandBehavior);
			if (this._depth == 0)
			{
				this._singleRow = CommandBehavior.Default != (CommandBehavior.SingleRow & commandBehavior);
			}
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x0025AF08 File Offset: 0x0025A308
		internal void InitializeIMultipleResults(object result)
		{
			this.Initialize();
			this._imultipleResults = (UnsafeNativeMethods.IMultipleResults)result;
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x0025AF28 File Offset: 0x0025A328
		internal void InitializeIRowset(object result, ChapterHandle chapterHandle, IntPtr recordsAffected)
		{
			if (this._connection == null || ChapterHandle.DB_NULL_HCHAPTER != chapterHandle)
			{
				this._rowHandleFetchCount = new IntPtr(1);
			}
			this.Initialize();
			this._recordsAffected = recordsAffected;
			this._irowset = (UnsafeNativeMethods.IRowset)result;
			this._chapterHandle = chapterHandle;
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x0025AF74 File Offset: 0x0025A374
		internal void InitializeIRow(object result, IntPtr recordsAffected)
		{
			this.Initialize();
			this._singleRow = true;
			this._recordsAffected = recordsAffected;
			this._irow = (UnsafeNativeMethods.IRow)result;
			this._hasRows = null != this._irow;
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001F2B RID: 7979 RVA: 0x0025AFB4 File Offset: 0x0025A3B4
		internal OleDbCommand Command
		{
			get
			{
				return this._command;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001F2C RID: 7980 RVA: 0x0025AFC8 File Offset: 0x0025A3C8
		public override int Depth
		{
			get
			{
				Bid.Trace("<oledb.OleDbDataReader.get_Depth|API> %d#\n", this.ObjectID);
				if (this.IsClosed)
				{
					throw ADP.DataReaderClosed("Depth");
				}
				return this._depth;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001F2D RID: 7981 RVA: 0x0025B000 File Offset: 0x0025A400
		public override int FieldCount
		{
			get
			{
				Bid.Trace("<oledb.OleDbDataReader.get_FieldCount|API> %d#\n", this.ObjectID);
				if (this.IsClosed)
				{
					throw ADP.DataReaderClosed("FieldCount");
				}
				MetaData[] metaData = this.MetaData;
				if (metaData == null)
				{
					return 0;
				}
				return metaData.Length;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001F2E RID: 7982 RVA: 0x0025B040 File Offset: 0x0025A440
		public override bool HasRows
		{
			get
			{
				Bid.Trace("<oledb.OleDbDataReader.get_HasRows|API> %d#\n", this.ObjectID);
				if (this.IsClosed)
				{
					throw ADP.DataReaderClosed("HasRows");
				}
				return this._hasRows;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06001F2F RID: 7983 RVA: 0x0025B078 File Offset: 0x0025A478
		public override bool IsClosed
		{
			get
			{
				Bid.Trace("<oledb.OleDbDataReader.get_IsClosed|API> %d#\n", this.ObjectID);
				return this._isClosed;
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06001F30 RID: 7984 RVA: 0x0025B09C File Offset: 0x0025A49C
		private MetaData[] MetaData
		{
			get
			{
				return this._metadata;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001F31 RID: 7985 RVA: 0x0025B0B0 File Offset: 0x0025A4B0
		public override int RecordsAffected
		{
			get
			{
				Bid.Trace("<oledb.OleDbDataReader.get_RecordsAffected|API> %d#\n", this.ObjectID);
				return ADP.IntPtrToInt32(this._recordsAffected);
			}
		}

		// Token: 0x17000446 RID: 1094
		public override object this[int index]
		{
			get
			{
				return this.GetValue(index);
			}
		}

		// Token: 0x17000447 RID: 1095
		public override object this[string name]
		{
			get
			{
				int ordinal = this.GetOrdinal(name);
				return this.GetValue(ordinal);
			}
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x0025B108 File Offset: 0x0025A508
		private UnsafeNativeMethods.IAccessor IAccessor()
		{
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|rowset> %d#, IAccessor\n", this.ObjectID);
			return (UnsafeNativeMethods.IAccessor)this.IRowset();
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x0025B130 File Offset: 0x0025A530
		private UnsafeNativeMethods.IRowsetInfo IRowsetInfo()
		{
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|rowset> %d#, IRowsetInfo\n", this.ObjectID);
			return (UnsafeNativeMethods.IRowsetInfo)this.IRowset();
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x0025B158 File Offset: 0x0025A558
		private UnsafeNativeMethods.IRowset IRowset()
		{
			UnsafeNativeMethods.IRowset irowset = this._irowset;
			if (irowset == null)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			return irowset;
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x0025B184 File Offset: 0x0025A584
		private UnsafeNativeMethods.IRow IRow()
		{
			UnsafeNativeMethods.IRow irow = this._irow;
			if (irow == null)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			return irow;
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x0025B1B0 File Offset: 0x0025A5B0
		public override DataTable GetSchemaTable()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbDataReader.GetSchemaTable|API> %d#\n", this.ObjectID);
			DataTable dataTable2;
			try
			{
				DataTable dataTable = this._dbSchemaTable;
				if (dataTable == null)
				{
					MetaData[] metaData = this.MetaData;
					if (metaData != null && 0 < metaData.Length)
					{
						if (0 < metaData.Length && this._useIColumnsRowset && this._connection != null)
						{
							this.AppendSchemaInfo();
						}
						dataTable = this.BuildSchemaTable(metaData);
					}
					else if (this.IsClosed)
					{
						throw ADP.DataReaderClosed("GetSchemaTable");
					}
				}
				dataTable2 = dataTable;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataTable2;
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x0025B24C File Offset: 0x0025A64C
		internal void BuildMetaInfo()
		{
			if (this._irowset != null)
			{
				if (this._useIColumnsRowset)
				{
					this.BuildSchemaTableRowset(this._irowset);
				}
				else
				{
					this.BuildSchemaTableInfo(this._irowset, false, false);
				}
				if (this._metadata != null && 0 < this._metadata.Length)
				{
					this.CreateAccessors(true);
				}
			}
			else if (this._irow != null)
			{
				this.BuildSchemaTableInfo(this._irow, false, false);
				if (this._metadata != null && 0 < this._metadata.Length)
				{
					this.CreateBindingsFromMetaData(true);
				}
			}
			if (this._metadata == null)
			{
				this._hasRows = false;
				this._visibleFieldCount = 0;
				this._metadata = new MetaData[0];
			}
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x0025B2F4 File Offset: 0x0025A6F4
		private DataTable BuildSchemaTable(MetaData[] metadata)
		{
			DataTable dataTable = new DataTable("SchemaTable");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.MinimumCapacity = metadata.Length;
			DataColumn dataColumn = new DataColumn("ColumnName", typeof(string));
			DataColumn dataColumn2 = new DataColumn("ColumnOrdinal", typeof(int));
			DataColumn dataColumn3 = new DataColumn("ColumnSize", typeof(int));
			DataColumn dataColumn4 = new DataColumn("NumericPrecision", typeof(short));
			DataColumn dataColumn5 = new DataColumn("NumericScale", typeof(short));
			DataColumn dataColumn6 = new DataColumn("DataType", typeof(Type));
			DataColumn dataColumn7 = new DataColumn("ProviderType", typeof(int));
			DataColumn dataColumn8 = new DataColumn("IsLong", typeof(bool));
			DataColumn dataColumn9 = new DataColumn("AllowDBNull", typeof(bool));
			DataColumn dataColumn10 = new DataColumn("IsReadOnly", typeof(bool));
			DataColumn dataColumn11 = new DataColumn("IsRowVersion", typeof(bool));
			DataColumn dataColumn12 = new DataColumn("IsUnique", typeof(bool));
			DataColumn dataColumn13 = new DataColumn("IsKey", typeof(bool));
			DataColumn dataColumn14 = new DataColumn("IsAutoIncrement", typeof(bool));
			DataColumn dataColumn15 = new DataColumn("IsHidden", typeof(bool));
			DataColumn dataColumn16 = new DataColumn("BaseSchemaName", typeof(string));
			DataColumn dataColumn17 = new DataColumn("BaseCatalogName", typeof(string));
			DataColumn dataColumn18 = new DataColumn("BaseTableName", typeof(string));
			DataColumn dataColumn19 = new DataColumn("BaseColumnName", typeof(string));
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
			if (this._visibleFieldCount < metadata.Length)
			{
				columns.Add(dataColumn15);
			}
			columns.Add(dataColumn16);
			columns.Add(dataColumn17);
			columns.Add(dataColumn18);
			columns.Add(dataColumn19);
			for (int i = 0; i < metadata.Length; i++)
			{
				MetaData metaData = metadata[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow[dataColumn] = metaData.columnName;
				dataRow[dataColumn2] = i;
				dataRow[dataColumn3] = ((metaData.type.enumOleDbType != OleDbType.BSTR) ? metaData.size : (-1));
				dataRow[dataColumn4] = metaData.precision;
				dataRow[dataColumn5] = metaData.scale;
				dataRow[dataColumn6] = metaData.type.dataType;
				dataRow[dataColumn7] = metaData.type.enumOleDbType;
				dataRow[dataColumn8] = OleDbDataReader.IsLong(metaData.flags);
				if (metaData.isKeyColumn)
				{
					dataRow[dataColumn9] = OleDbDataReader.AllowDBNull(metaData.flags);
				}
				else
				{
					dataRow[dataColumn9] = OleDbDataReader.AllowDBNullMaybeNull(metaData.flags);
				}
				dataRow[dataColumn10] = OleDbDataReader.IsReadOnly(metaData.flags);
				dataRow[dataColumn11] = OleDbDataReader.IsRowVersion(metaData.flags);
				dataRow[dataColumn12] = metaData.isUnique;
				dataRow[dataColumn13] = metaData.isKeyColumn;
				dataRow[dataColumn14] = metaData.isAutoIncrement;
				if (this._visibleFieldCount < metadata.Length)
				{
					dataRow[dataColumn15] = metaData.isHidden;
				}
				if (metaData.baseSchemaName != null)
				{
					dataRow[dataColumn16] = metaData.baseSchemaName;
				}
				if (metaData.baseCatalogName != null)
				{
					dataRow[dataColumn17] = metaData.baseCatalogName;
				}
				if (metaData.baseTableName != null)
				{
					dataRow[dataColumn18] = metaData.baseTableName;
				}
				if (metaData.baseColumnName != null)
				{
					dataRow[dataColumn19] = metaData.baseColumnName;
				}
				dataTable.Rows.Add(dataRow);
				dataRow.AcceptChanges();
			}
			int count = columns.Count;
			for (int j = 0; j < count; j++)
			{
				columns[j].ReadOnly = true;
			}
			this._dbSchemaTable = dataTable;
			return dataTable;
		}

		// Token: 0x06001F3B RID: 7995 RVA: 0x0025B7B0 File Offset: 0x0025ABB0
		private void BuildSchemaTableInfo(object handle, bool filterITypeInfo, bool filterChapters)
		{
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|rowset_row> %d#, IColumnsInfo\n", this.ObjectID);
			UnsafeNativeMethods.IColumnsInfo columnsInfo = handle as UnsafeNativeMethods.IColumnsInfo;
			if (columnsInfo == null)
			{
				Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|RET> %08X{HRESULT}\n", OleDbHResult.E_NOINTERFACE);
				this._dbSchemaTable = null;
				return;
			}
			IntPtr ptrZero = ADP.PtrZero;
			IntPtr ptrZero2 = ADP.PtrZero;
			OleDbHResult oleDbHResult;
			using (new DualCoTaskMem(columnsInfo, out ptrZero, out ptrZero2, out oleDbHResult))
			{
				if (oleDbHResult < OleDbHResult.S_OK)
				{
					this.ProcessResults(oleDbHResult);
				}
				if (0 < (int)ptrZero)
				{
					this.BuildSchemaTableInfoTable(ptrZero.ToInt32(), ptrZero2, filterITypeInfo, filterChapters);
				}
			}
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x0025B858 File Offset: 0x0025AC58
		private void BuildSchemaTableInfoTable(int columnCount, IntPtr columnInfos, bool filterITypeInfo, bool filterChapters)
		{
			int num = 0;
			MetaData[] array = new MetaData[columnCount];
			tagDBCOLUMNINFO tagDBCOLUMNINFO = new tagDBCOLUMNINFO();
			int i = 0;
			int num2 = 0;
			while (i < columnCount)
			{
				Marshal.PtrToStructure(ADP.IntPtrOffset(columnInfos, num2), tagDBCOLUMNINFO);
				if (0 < (int)tagDBCOLUMNINFO.iOrdinal && !OleDbDataReader.DoColumnDropFilter(tagDBCOLUMNINFO.dwFlags))
				{
					if (tagDBCOLUMNINFO.pwszName == null)
					{
						tagDBCOLUMNINFO.pwszName = "";
					}
					if ((!filterITypeInfo || !("DBCOLUMN_TYPEINFO" == tagDBCOLUMNINFO.pwszName)) && (!filterChapters || 136 != tagDBCOLUMNINFO.wType))
					{
						bool flag = OleDbDataReader.IsLong(tagDBCOLUMNINFO.dwFlags);
						bool flag2 = OleDbDataReader.IsFixed(tagDBCOLUMNINFO.dwFlags);
						NativeDBType nativeDBType = NativeDBType.FromDBType(tagDBCOLUMNINFO.wType, flag, flag2);
						MetaData metaData = new MetaData();
						metaData.columnName = tagDBCOLUMNINFO.pwszName;
						metaData.type = nativeDBType;
						metaData.ordinal = tagDBCOLUMNINFO.iOrdinal;
						metaData.size = (int)tagDBCOLUMNINFO.ulColumnSize;
						metaData.flags = tagDBCOLUMNINFO.dwFlags;
						metaData.precision = tagDBCOLUMNINFO.bPrecision;
						metaData.scale = tagDBCOLUMNINFO.bScale;
						metaData.kind = tagDBCOLUMNINFO.columnid.eKind;
						int eKind = tagDBCOLUMNINFO.columnid.eKind;
						switch (eKind)
						{
						case 0:
						case 1:
							goto IL_0141;
						default:
							if (eKind == 6)
							{
								goto IL_0141;
							}
							metaData.guid = Guid.Empty;
							break;
						}
						IL_015F:
						switch (tagDBCOLUMNINFO.columnid.eKind)
						{
						case 0:
						case 2:
							if (ADP.PtrZero != tagDBCOLUMNINFO.columnid.ulPropid)
							{
								metaData.idname = Marshal.PtrToStringUni(tagDBCOLUMNINFO.columnid.ulPropid);
							}
							else
							{
								metaData.idname = null;
							}
							break;
						case 1:
						case 5:
							metaData.propid = tagDBCOLUMNINFO.columnid.ulPropid;
							break;
						case 3:
						case 4:
							goto IL_01D8;
						default:
							goto IL_01D8;
						}
						IL_01E3:
						array[num] = metaData;
						num++;
						goto IL_01EC;
						IL_01D8:
						metaData.propid = ADP.PtrZero;
						goto IL_01E3;
						IL_0141:
						metaData.guid = tagDBCOLUMNINFO.columnid.uGuid;
						goto IL_015F;
					}
				}
				IL_01EC:
				i++;
				num2 += ODB.SizeOf_tagDBCOLUMNINFO;
			}
			if (num < columnCount)
			{
				MetaData[] array2 = new MetaData[num];
				for (int j = 0; j < num; j++)
				{
					array2[j] = array[j];
				}
				array = array2;
			}
			this._visibleFieldCount = num;
			this._metadata = array;
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x0025BA9C File Offset: 0x0025AE9C
		private void BuildSchemaTableRowset(object handle)
		{
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|rowset_row> %d, IColumnsRowset\n", this.ObjectID);
			UnsafeNativeMethods.IColumnsRowset columnsRowset = handle as UnsafeNativeMethods.IColumnsRowset;
			if (columnsRowset != null)
			{
				UnsafeNativeMethods.IRowset rowset = null;
				IntPtr intPtr;
				OleDbHResult columnsRowset2;
				using (DualCoTaskMem dualCoTaskMem = new DualCoTaskMem(columnsRowset, out intPtr, out columnsRowset2))
				{
					Bid.Trace("<oledb.IColumnsRowset.GetColumnsRowset|API|OLEDB> %d#, IID_IRowset\n", this.ObjectID);
					columnsRowset2 = columnsRowset.GetColumnsRowset(ADP.PtrZero, intPtr, dualCoTaskMem, ref ODB.IID_IRowset, 0, ADP.PtrZero, out rowset);
					Bid.Trace("<oledb.IColumnsRowset.GetColumnsRowset|API|OLEDB|RET> %08X{HRESULT}\n", columnsRowset2);
				}
				if (columnsRowset2 < OleDbHResult.S_OK)
				{
					this.ProcessResults(columnsRowset2);
				}
				this.DumpToSchemaTable(rowset);
				return;
			}
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|RET> %08X{HRESULT}\n", OleDbHResult.E_NOINTERFACE);
			this._useIColumnsRowset = false;
			this.BuildSchemaTableInfo(handle, false, false);
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x0025BB64 File Offset: 0x0025AF64
		public override void Close()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbDataReader.Close|API> %d#\n", this.ObjectID);
			try
			{
				OleDbConnection connection = this._connection;
				OleDbCommand command = this._command;
				Bindings bindings = this._parameterBindings;
				this._connection = null;
				this._command = null;
				this._parameterBindings = null;
				this._isClosed = true;
				this.DisposeOpenResults();
				this._hasRows = false;
				if (command != null && command.canceling)
				{
					this.DisposeNativeMultipleResults();
					if (bindings != null)
					{
						bindings.CloseFromConnection();
						bindings = null;
					}
				}
				else
				{
					UnsafeNativeMethods.IMultipleResults imultipleResults = this._imultipleResults;
					this._imultipleResults = null;
					if (imultipleResults != null)
					{
						try
						{
							if (command != null && !command.canceling)
							{
								IntPtr zero = IntPtr.Zero;
								OleDbException ex = OleDbDataReader.NextResults(imultipleResults, null, command, out zero);
								this._recordsAffected = OleDbDataReader.AddRecordsAffected(this._recordsAffected, zero);
								if (ex != null)
								{
									throw ex;
								}
							}
						}
						finally
						{
							if (imultipleResults != null)
							{
								Marshal.ReleaseComObject(imultipleResults);
							}
						}
					}
				}
				if (command != null && this._depth == 0)
				{
					command.CloseFromDataReader(bindings);
				}
				if (connection != null)
				{
					connection.RemoveWeakReference(this);
					if (this.IsCommandBehavior(CommandBehavior.CloseConnection))
					{
						connection.Close();
					}
				}
				RowHandleBuffer rowHandleNativeBuffer = this._rowHandleNativeBuffer;
				this._rowHandleNativeBuffer = null;
				if (rowHandleNativeBuffer != null)
				{
					rowHandleNativeBuffer.Dispose();
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x0025BCBC File Offset: 0x0025B0BC
		internal void CloseReaderFromConnection(bool canceling)
		{
			if (this._command != null)
			{
				this._command.canceling = canceling;
			}
			this._connection = null;
			this.Close();
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x0025BCEC File Offset: 0x0025B0EC
		private void DisposeManagedRowset()
		{
			this._isRead = false;
			this._hasRowsReadCheck = false;
			this._nextAccessorForRetrieval = 0;
			this._nextValueForRetrieval = 0;
			Bindings[] bindings = this._bindings;
			this._bindings = null;
			if (bindings != null)
			{
				for (int i = 0; i < bindings.Length; i++)
				{
					if (bindings[i] != null)
					{
						bindings[i].Dispose();
					}
				}
			}
			this._currentRow = 0;
			this._rowFetchedCount = IntPtr.Zero;
			this._dbSchemaTable = null;
			this._visibleFieldCount = 0;
			this._metadata = null;
			this._fieldNameLookup = null;
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x0025BD70 File Offset: 0x0025B170
		private void DisposeNativeMultipleResults()
		{
			UnsafeNativeMethods.IMultipleResults imultipleResults = this._imultipleResults;
			this._imultipleResults = null;
			if (imultipleResults != null)
			{
				Marshal.ReleaseComObject(imultipleResults);
			}
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x0025BD98 File Offset: 0x0025B198
		private void DisposeNativeRowset()
		{
			UnsafeNativeMethods.IRowset irowset = this._irowset;
			this._irowset = null;
			ChapterHandle chapterHandle = this._chapterHandle;
			this._chapterHandle = ChapterHandle.DB_NULL_HCHAPTER;
			if (ChapterHandle.DB_NULL_HCHAPTER != chapterHandle)
			{
				chapterHandle.Dispose();
			}
			if (irowset != null)
			{
				Marshal.ReleaseComObject(irowset);
			}
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x0025BDE0 File Offset: 0x0025B1E0
		private void DisposeNativeRow()
		{
			UnsafeNativeMethods.IRow irow = this._irow;
			this._irow = null;
			if (irow != null)
			{
				Marshal.ReleaseComObject(irow);
			}
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x0025BE08 File Offset: 0x0025B208
		private void DisposeOpenResults()
		{
			this.DisposeManagedRowset();
			this.DisposeNativeRow();
			this.DisposeNativeRowset();
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x0025BE28 File Offset: 0x0025B228
		public override bool GetBoolean(int ordinal)
		{
			ColumnBinding columnBinding = this.GetColumnBinding(ordinal);
			return columnBinding.ValueBoolean();
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x0025BE44 File Offset: 0x0025B244
		public override byte GetByte(int ordinal)
		{
			ColumnBinding columnBinding = this.GetColumnBinding(ordinal);
			return columnBinding.ValueByte();
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x0025BE60 File Offset: 0x0025B260
		private ColumnBinding DoSequentialCheck(int ordinal, long dataIndex, string method)
		{
			ColumnBinding columnBinding = this.GetColumnBinding(ordinal);
			if (dataIndex > 2147483647L)
			{
				throw ADP.InvalidSourceBufferIndex(0, dataIndex, "dataIndex");
			}
			if (this._sequentialOrdinal != ordinal)
			{
				this._sequentialOrdinal = ordinal;
				this._sequentialBytesRead = 0L;
			}
			else if (this._sequentialAccess && this._sequentialBytesRead < dataIndex)
			{
				throw ADP.NonSeqByteAccess(dataIndex, this._sequentialBytesRead, method);
			}
			return columnBinding;
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x0025BEC8 File Offset: 0x0025B2C8
		public override long GetBytes(int ordinal, long dataIndex, byte[] buffer, int bufferIndex, int length)
		{
			ColumnBinding columnBinding = this.DoSequentialCheck(ordinal, dataIndex, "GetBytes");
			byte[] array = columnBinding.ValueByteArray();
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
			if (bufferIndex < 0 || bufferIndex >= buffer.Length)
			{
				throw ADP.InvalidDestinationBufferIndex(buffer.Length, bufferIndex, "bufferIndex");
			}
			if (0 < num2)
			{
				Buffer.BlockCopy(array, num, buffer, bufferIndex, num2);
				this._sequentialBytesRead = (long)(num + num2);
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

		// Token: 0x06001F49 RID: 8009 RVA: 0x0025BF60 File Offset: 0x0025B360
		public override long GetChars(int ordinal, long dataIndex, char[] buffer, int bufferIndex, int length)
		{
			ColumnBinding columnBinding = this.DoSequentialCheck(ordinal, dataIndex, "GetChars");
			string text = columnBinding.ValueString();
			if (buffer == null)
			{
				return (long)text.Length;
			}
			int num = (int)dataIndex;
			int num2 = Math.Min(text.Length - num, length);
			if (num < 0)
			{
				throw ADP.InvalidSourceBufferIndex(text.Length, (long)num, "dataIndex");
			}
			if (bufferIndex < 0 || bufferIndex >= buffer.Length)
			{
				throw ADP.InvalidDestinationBufferIndex(buffer.Length, bufferIndex, "bufferIndex");
			}
			if (0 < num2)
			{
				text.CopyTo(num, buffer, bufferIndex, num2);
				this._sequentialBytesRead = (long)(num + num2);
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

		// Token: 0x06001F4A RID: 8010 RVA: 0x0025C000 File Offset: 0x0025B400
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override char GetChar(int ordinal)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x0025C014 File Offset: 0x0025B414
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new OleDbDataReader GetData(int ordinal)
		{
			ColumnBinding columnBinding = this.GetColumnBinding(ordinal);
			return columnBinding.ValueChapter();
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x0025C030 File Offset: 0x0025B430
		protected override DbDataReader GetDbDataReader(int ordinal)
		{
			return this.GetData(ordinal);
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x0025C044 File Offset: 0x0025B444
		internal OleDbDataReader ResetChapter(int bindingIndex, int index, RowBinding rowbinding, int valueOffset)
		{
			return this.GetDataForReader(this._metadata[bindingIndex + index].ordinal, rowbinding, valueOffset);
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x0025C06C File Offset: 0x0025B46C
		private OleDbDataReader GetDataForReader(IntPtr ordinal, RowBinding rowbinding, int valueOffset)
		{
			UnsafeNativeMethods.IRowsetInfo rowsetInfo = this.IRowsetInfo();
			Bid.Trace("<oledb.IRowsetInfo.GetReferencedRowset|API|OLEDB> %d#, ColumnOrdinal=%Id\n", this.ObjectID, ordinal);
			UnsafeNativeMethods.IRowset rowset;
			OleDbHResult referencedRowset = rowsetInfo.GetReferencedRowset(ordinal, ref ODB.IID_IRowset, out rowset);
			Bid.Trace("<oledb.IRowsetInfo.GetReferencedRowset|API|OLEDB|RET> %08X{HRESULT}\n", referencedRowset);
			this.ProcessResults(referencedRowset);
			OleDbDataReader oleDbDataReader = null;
			if (rowset != null)
			{
				ChapterHandle chapterHandle = ChapterHandle.CreateChapterHandle(rowset, rowbinding, valueOffset);
				oleDbDataReader = new OleDbDataReader(this._connection, this._command, 1 + this.Depth, this._commandBehavior & ~CommandBehavior.CloseConnection);
				oleDbDataReader.InitializeIRowset(rowset, chapterHandle, ADP.RecordsUnaffected);
				oleDbDataReader.BuildMetaInfo();
				oleDbDataReader.HasRowsRead();
				if (this._connection != null)
				{
					this._connection.AddWeakReference(oleDbDataReader, 2);
				}
			}
			return oleDbDataReader;
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x0025C114 File Offset: 0x0025B514
		public override string GetDataTypeName(int index)
		{
			if (this._metadata != null)
			{
				return this._metadata[index].type.dataSourceType;
			}
			throw ADP.DataReaderNoData();
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x0025C144 File Offset: 0x0025B544
		public override DateTime GetDateTime(int ordinal)
		{
			ColumnBinding columnBinding = this.GetColumnBinding(ordinal);
			return columnBinding.ValueDateTime();
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x0025C160 File Offset: 0x0025B560
		public override decimal GetDecimal(int ordinal)
		{
			ColumnBinding columnBinding = this.GetColumnBinding(ordinal);
			return columnBinding.ValueDecimal();
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x0025C17C File Offset: 0x0025B57C
		public override double GetDouble(int ordinal)
		{
			ColumnBinding columnBinding = this.GetColumnBinding(ordinal);
			return columnBinding.ValueDouble();
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x0025C198 File Offset: 0x0025B598
		public override IEnumerator GetEnumerator()
		{
			return new DbEnumerator(this, this.IsCommandBehavior(CommandBehavior.CloseConnection));
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x0025C1B4 File Offset: 0x0025B5B4
		public override Type GetFieldType(int index)
		{
			if (this._metadata != null)
			{
				return this._metadata[index].type.dataType;
			}
			throw ADP.DataReaderNoData();
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x0025C1E4 File Offset: 0x0025B5E4
		public override float GetFloat(int ordinal)
		{
			ColumnBinding columnBinding = this.GetColumnBinding(ordinal);
			return columnBinding.ValueSingle();
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x0025C200 File Offset: 0x0025B600
		public override Guid GetGuid(int ordinal)
		{
			ColumnBinding columnBinding = this.GetColumnBinding(ordinal);
			return columnBinding.ValueGuid();
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x0025C21C File Offset: 0x0025B61C
		public override short GetInt16(int ordinal)
		{
			ColumnBinding columnBinding = this.GetColumnBinding(ordinal);
			return columnBinding.ValueInt16();
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x0025C238 File Offset: 0x0025B638
		public override int GetInt32(int ordinal)
		{
			ColumnBinding columnBinding = this.GetColumnBinding(ordinal);
			return columnBinding.ValueInt32();
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0025C254 File Offset: 0x0025B654
		public override long GetInt64(int ordinal)
		{
			ColumnBinding columnBinding = this.GetColumnBinding(ordinal);
			return columnBinding.ValueInt64();
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x0025C270 File Offset: 0x0025B670
		public override string GetName(int index)
		{
			if (this._metadata != null)
			{
				return this._metadata[index].columnName;
			}
			throw ADP.DataReaderNoData();
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x0025C298 File Offset: 0x0025B698
		public override int GetOrdinal(string name)
		{
			if (this._fieldNameLookup == null)
			{
				if (this._metadata == null)
				{
					throw ADP.DataReaderNoData();
				}
				this._fieldNameLookup = new FieldNameLookup(this, -1);
			}
			return this._fieldNameLookup.GetOrdinal(name);
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x0025C2D4 File Offset: 0x0025B6D4
		public override string GetString(int ordinal)
		{
			ColumnBinding columnBinding = this.GetColumnBinding(ordinal);
			return columnBinding.ValueString();
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x0025C2F0 File Offset: 0x0025B6F0
		public TimeSpan GetTimeSpan(int ordinal)
		{
			return (TimeSpan)this.GetValue(ordinal);
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x0025C30C File Offset: 0x0025B70C
		private MetaData DoValueCheck(int ordinal)
		{
			if (!this._isRead)
			{
				throw ADP.DataReaderNoData();
			}
			if (this._sequentialAccess && ordinal < this._nextValueForRetrieval)
			{
				throw ADP.NonSequentialColumnAccess(ordinal, this._nextValueForRetrieval);
			}
			return this._metadata[ordinal];
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x0025C350 File Offset: 0x0025B750
		private ColumnBinding GetColumnBinding(int ordinal)
		{
			MetaData metaData = this.DoValueCheck(ordinal);
			return this.GetValueBinding(metaData);
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x0025C36C File Offset: 0x0025B76C
		private ColumnBinding GetValueBinding(MetaData info)
		{
			ColumnBinding columnBinding = info.columnBinding;
			for (int i = this._nextAccessorForRetrieval; i <= columnBinding.IndexForAccessor; i++)
			{
				if (this._sequentialAccess)
				{
					if (this._nextValueForRetrieval != columnBinding.Index)
					{
						this._metadata[this._nextValueForRetrieval].columnBinding.ResetValue();
					}
					this._nextAccessorForRetrieval = columnBinding.IndexForAccessor;
				}
				if (this._irowset != null)
				{
					this.GetRowDataFromHandle();
				}
				else
				{
					if (this._irow == null)
					{
						throw ADP.DataReaderNoData();
					}
					this.GetRowValue();
				}
			}
			this._nextValueForRetrieval = columnBinding.Index;
			return columnBinding;
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x0025C404 File Offset: 0x0025B804
		public override object GetValue(int ordinal)
		{
			ColumnBinding columnBinding = this.GetColumnBinding(ordinal);
			return columnBinding.Value();
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x0025C424 File Offset: 0x0025B824
		public override int GetValues(object[] values)
		{
			if (values == null)
			{
				throw ADP.ArgumentNull("values");
			}
			this.DoValueCheck(0);
			int num = Math.Min(values.Length, this._visibleFieldCount);
			int num2 = 0;
			while (num2 < this._metadata.Length && num2 < num)
			{
				ColumnBinding valueBinding = this.GetValueBinding(this._metadata[num2]);
				values[num2] = valueBinding.Value();
				num2++;
			}
			return num;
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x0025C488 File Offset: 0x0025B888
		private bool IsCommandBehavior(CommandBehavior condition)
		{
			return condition == (condition & this._commandBehavior);
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x0025C4A0 File Offset: 0x0025B8A0
		public override bool IsDBNull(int ordinal)
		{
			ColumnBinding columnBinding = this.GetColumnBinding(ordinal);
			return columnBinding.IsValueNull();
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x0025C4BC File Offset: 0x0025B8BC
		private void ProcessResults(OleDbHResult hr)
		{
			Exception ex;
			if (this._command != null)
			{
				ex = OleDbConnection.ProcessResults(hr, this._connection, this._command);
			}
			else
			{
				ex = OleDbConnection.ProcessResults(hr, this._connection, this._connection);
			}
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x0025C500 File Offset: 0x0025B900
		private static IntPtr AddRecordsAffected(IntPtr recordsAffected, IntPtr affected)
		{
			if (0 > (int)affected)
			{
				return recordsAffected;
			}
			if (0 <= (int)recordsAffected)
			{
				return (IntPtr)((int)recordsAffected + (int)affected);
			}
			return affected;
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001F67 RID: 8039 RVA: 0x0025C538 File Offset: 0x0025B938
		public override int VisibleFieldCount
		{
			get
			{
				Bid.Trace("<oledb.OleDbDataReader.get_VisibleFieldCount|API> %d#\n", this.ObjectID);
				if (this.IsClosed)
				{
					throw ADP.DataReaderClosed("VisibleFieldCount");
				}
				return this._visibleFieldCount;
			}
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x0025C570 File Offset: 0x0025B970
		internal void HasRowsRead()
		{
			bool flag = this.Read();
			this._hasRows = flag;
			this._hasRowsReadCheck = true;
			this._isRead = false;
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x0025C59C File Offset: 0x0025B99C
		internal static OleDbException NextResults(UnsafeNativeMethods.IMultipleResults imultipleResults, OleDbConnection connection, OleDbCommand command, out IntPtr recordsAffected)
		{
			recordsAffected = ADP.RecordsUnaffected;
			List<OleDbException> list = null;
			if (imultipleResults != null)
			{
				int num = 0;
				while (command == null || !command.canceling)
				{
					Bid.Trace("<oledb.IMultipleResults.GetResult|API|OLEDB> %d#, DBRESULTFLAG_DEFAULT, IID_NULL\n");
					IntPtr intPtr;
					object obj;
					OleDbHResult result = imultipleResults.GetResult(ADP.PtrZero, ODB.DBRESULTFLAG_DEFAULT, ref ODB.IID_NULL, out intPtr, out obj);
					Bid.Trace("<oledb.IMultipleResults.GetResult|API|OLEDB|RET> %08X{HRESULT}, RecordAffected=%Id\n", result, intPtr);
					if (OleDbHResult.DB_S_NORESULT == result || OleDbHResult.E_NOINTERFACE == result)
					{
						break;
					}
					if (connection != null)
					{
						Exception ex = OleDbConnection.ProcessResults(result, connection, command);
						if (ex != null)
						{
							OleDbException ex2 = ex as OleDbException;
							if (ex2 == null)
							{
								throw ex;
							}
							if (list == null)
							{
								list = new List<OleDbException>();
							}
							list.Add(ex2);
						}
					}
					else if (result < OleDbHResult.S_OK)
					{
						SafeNativeMethods.Wrapper.ClearErrorInfo();
						break;
					}
					recordsAffected = OleDbDataReader.AddRecordsAffected(recordsAffected, intPtr);
					if ((int)intPtr != 0)
					{
						num = 0;
					}
					else if (2000 <= num)
					{
						OleDbDataReader.NextResultsInfinite();
						break;
					}
					num++;
				}
			}
			if (list != null)
			{
				return OleDbException.CombineExceptions(list);
			}
			return null;
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x0025C690 File Offset: 0x0025BA90
		private static void NextResultsInfinite()
		{
			Bid.Trace("<oledb.OleDbDataReader.NextResultsInfinite|INFO> System.Data.OleDb.OleDbDataReader: 2000 IMultipleResult.GetResult(NULL, DBRESULTFLAG_DEFAULT, IID_NULL, NULL, NULL) iterations with 0 records affected. Stopping suspect infinite loop. To work-around try using ExecuteReader() and iterating through results with NextResult().\n");
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x0025C6A8 File Offset: 0x0025BAA8
		public override bool NextResult()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbDataReader.NextResult|API> %d#\n", this.ObjectID);
			bool flag2;
			try
			{
				bool flag = false;
				if (this.IsClosed)
				{
					throw ADP.DataReaderClosed("NextResult");
				}
				this._fieldNameLookup = null;
				OleDbCommand command = this._command;
				UnsafeNativeMethods.IMultipleResults imultipleResults = this._imultipleResults;
				if (imultipleResults != null)
				{
					this.DisposeOpenResults();
					this._hasRows = false;
					for (;;)
					{
						object obj = null;
						if (command != null && command.canceling)
						{
							break;
						}
						Bid.Trace("<oledb.IMultipleResults.GetResult|API|OLEDB> %d#, IID_IRowset\n", this.ObjectID);
						IntPtr intPtr2;
						OleDbHResult result = imultipleResults.GetResult(ADP.PtrZero, ODB.DBRESULTFLAG_DEFAULT, ref ODB.IID_IRowset, out intPtr2, out obj);
						Bid.Trace("<oledb.IMultipleResults.GetResult|API|OLEDB|RET> %08X{HRESULT}, RecordAffected=%Id\n", result, intPtr2);
						if (OleDbHResult.S_OK <= result && obj != null)
						{
							Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|RowSet> %d#, IRowset\n", this.ObjectID);
							this._irowset = (UnsafeNativeMethods.IRowset)obj;
						}
						this._recordsAffected = OleDbDataReader.AddRecordsAffected(this._recordsAffected, intPtr2);
						if (OleDbHResult.DB_S_NORESULT == result)
						{
							goto Block_9;
						}
						this.ProcessResults(result);
						if (this._irowset != null)
						{
							goto Block_10;
						}
					}
					this.Close();
					goto IL_0116;
					Block_9:
					this.DisposeNativeMultipleResults();
					goto IL_0116;
					Block_10:
					this.BuildMetaInfo();
					this.HasRowsRead();
					flag = true;
				}
				else
				{
					this.DisposeOpenResults();
					this._hasRows = false;
				}
				IL_0116:
				flag2 = flag;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return flag2;
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x0025C7F8 File Offset: 0x0025BBF8
		public override bool Read()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbDataReader.Read|API> %d#\n", this.ObjectID);
			bool flag2;
			try
			{
				bool flag = false;
				OleDbCommand command = this._command;
				if (command != null && command.canceling)
				{
					this.DisposeOpenResults();
				}
				else if (this._irowset != null)
				{
					if (this._hasRowsReadCheck)
					{
						flag = (this._isRead = this._hasRows);
						this._hasRowsReadCheck = false;
					}
					else if (this._singleRow && this._isRead)
					{
						this.DisposeOpenResults();
					}
					else
					{
						flag = this.ReadRowset();
					}
				}
				else if (this._irow != null)
				{
					flag = this.ReadRow();
				}
				else if (this.IsClosed)
				{
					throw ADP.DataReaderClosed("Read");
				}
				flag2 = flag;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return flag2;
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x0025C8C8 File Offset: 0x0025BCC8
		private bool ReadRow()
		{
			if (this._isRead)
			{
				this._isRead = false;
				this.DisposeNativeRow();
				this._sequentialOrdinal = -1;
				return false;
			}
			this._isRead = true;
			return 0 < this._metadata.Length;
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x0025C908 File Offset: 0x0025BD08
		private bool ReadRowset()
		{
			this.ReleaseCurrentRow();
			this._sequentialOrdinal = -1;
			if (IntPtr.Zero == this._rowFetchedCount)
			{
				this.GetRowHandles();
			}
			return this._currentRow <= (int)this._rowFetchedCount && this._isRead;
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x0025C958 File Offset: 0x0025BD58
		private void ReleaseCurrentRow()
		{
			if (0 < (int)this._rowFetchedCount)
			{
				Bindings[] bindings = this._bindings;
				int num = 0;
				while (num < bindings.Length && num < this._nextAccessorForRetrieval)
				{
					bindings[num].CleanupBindings();
					num++;
				}
				this._nextAccessorForRetrieval = 0;
				this._nextValueForRetrieval = 0;
				this._currentRow++;
				if (this._currentRow == (int)this._rowFetchedCount)
				{
					this.ReleaseRowHandles();
				}
			}
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x0025C9D0 File Offset: 0x0025BDD0
		private void CreateAccessors(bool allowMultipleAccessor)
		{
			Bindings[] array = this.CreateBindingsFromMetaData(allowMultipleAccessor);
			UnsafeNativeMethods.IAccessor accessor = this.IAccessor();
			for (int i = 0; i < array.Length; i++)
			{
				OleDbHResult oleDbHResult = array[i].CreateAccessor(accessor, 2);
				if (oleDbHResult < OleDbHResult.S_OK)
				{
					this.ProcessResults(oleDbHResult);
				}
			}
			if (IntPtr.Zero == this._rowHandleFetchCount)
			{
				this._rowHandleFetchCount = new IntPtr(1);
				object propertyValue = this.GetPropertyValue(73);
				if (propertyValue is int)
				{
					this._rowHandleFetchCount = new IntPtr((int)propertyValue);
					if (ADP.PtrZero == this._rowHandleFetchCount || 20 <= (int)this._rowHandleFetchCount)
					{
						this._rowHandleFetchCount = new IntPtr(20);
					}
				}
				else if (propertyValue is long)
				{
					this._rowHandleFetchCount = new IntPtr((long)propertyValue);
					if (ADP.PtrZero == this._rowHandleFetchCount || 20L <= (long)this._rowHandleFetchCount)
					{
						this._rowHandleFetchCount = new IntPtr(20);
					}
				}
			}
			if (this._rowHandleNativeBuffer == null)
			{
				this._rowHandleNativeBuffer = new RowHandleBuffer(this._rowHandleFetchCount);
			}
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x0025CAE8 File Offset: 0x0025BEE8
		private Bindings[] CreateBindingsFromMetaData(bool allowMultipleAccessor)
		{
			int num = 0;
			int num2 = 0;
			MetaData[] metadata = this._metadata;
			int[] array = new int[metadata.Length];
			int[] array2 = new int[metadata.Length];
			if (allowMultipleAccessor)
			{
				if (this._irowset != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = num;
						array2[i] = num2;
						num2++;
					}
					if (0 < num2)
					{
						num++;
					}
				}
				else if (this._irow != null)
				{
					for (int j = 0; j < array.Length; j++)
					{
						array[j] = j;
						array2[j] = 0;
					}
					num = metadata.Length;
				}
			}
			else
			{
				for (int k = 0; k < array.Length; k++)
				{
					array[k] = 0;
					array2[k] = k;
				}
				num = 1;
			}
			Bindings[] array3 = new Bindings[num];
			for (int l = 0; l < metadata.Length; l++)
			{
				Bindings bindings = array3[array[l]];
				if (bindings == null)
				{
					num = 0;
					int num3 = l;
					while (num3 < metadata.Length && num == array2[num3])
					{
						num++;
						num3++;
					}
					bindings = (array3[array[l]] = new Bindings(this, null != this._irowset, num));
				}
				MetaData metaData = metadata[l];
				int num4 = metaData.type.fixlen;
				short num5 = metaData.type.wType;
				if (-1 != metaData.size)
				{
					if (metaData.type.islong)
					{
						num4 = ADP.PtrSize;
						num5 = (short)((ushort)num5 | 16384);
					}
					else if (-1 == num4)
					{
						if (8192 < metaData.size)
						{
							num4 = ADP.PtrSize;
							num5 = (short)((ushort)num5 | 16384);
						}
						else if (130 == num5 && -1 != metaData.size)
						{
							num4 = metaData.size * 2 + 2;
						}
						else
						{
							num4 = metaData.size;
						}
					}
				}
				else if (num4 < 0)
				{
					num4 = ADP.PtrSize;
					num5 = (short)((ushort)num5 | 16384);
				}
				num2 = array2[l];
				bindings.CurrentIndex = num2;
				bindings.Ordinal = metaData.ordinal;
				bindings.Part = metaData.type.dbPart;
				bindings.Precision = metaData.precision;
				bindings.Scale = metaData.scale;
				bindings.DbType = (int)num5;
				bindings.MaxLen = num4;
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<oledb.struct.tagDBBINDING|INFO|ADV> index=%d, columnName='%ls'\n", l, metaData.columnName);
				}
			}
			int num6 = 0;
			int num7 = 0;
			for (int m = 0; m < array3.Length; m++)
			{
				num7 = array3[m].AllocateForAccessor(this, num7, m);
				ColumnBinding[] array4 = array3[m].ColumnBindings();
				for (int n = 0; n < array4.Length; n++)
				{
					metadata[num6].columnBinding = array4[n];
					metadata[num6].bindings = array3[m];
					num6++;
				}
			}
			this._bindings = array3;
			return array3;
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x0025CDA4 File Offset: 0x0025C1A4
		private void GetRowHandles()
		{
			OleDbHResult oleDbHResult = OleDbHResult.S_OK;
			RowHandleBuffer rowHandleNativeBuffer = this._rowHandleNativeBuffer;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				rowHandleNativeBuffer.DangerousAddRef(ref flag);
				IntPtr intPtr = rowHandleNativeBuffer.DangerousGetHandle();
				UnsafeNativeMethods.IRowset rowset = this.IRowset();
				try
				{
					Bid.Trace("<oledb.IRowset.GetNextRows|API|OLEDB> %d#, Chapter=%Id, RowsRequested=%Id\n", this.ObjectID, this._chapterHandle.HChapter, this._rowHandleFetchCount);
					oleDbHResult = rowset.GetNextRows(this._chapterHandle.HChapter, IntPtr.Zero, this._rowHandleFetchCount, out this._rowFetchedCount, ref intPtr);
					Bid.Trace("<oledb.IRowset.GetNextRows|API|OLEDB|RET> %08X{HRESULT}, RowsObtained=%Id\n", oleDbHResult, this._rowFetchedCount);
				}
				catch (InvalidCastException ex)
				{
					throw ODB.ThreadApartmentState(ex);
				}
			}
			finally
			{
				if (flag)
				{
					rowHandleNativeBuffer.DangerousRelease();
				}
			}
			if (oleDbHResult < OleDbHResult.S_OK)
			{
				this.ProcessResults(oleDbHResult);
			}
			this._isRead = OleDbHResult.DB_S_ENDOFROWSET != oleDbHResult || 0 < (int)this._rowFetchedCount;
			this._rowFetchedCount = (IntPtr)Math.Max((int)this._rowFetchedCount, 0);
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x0025CEC4 File Offset: 0x0025C2C4
		private void GetRowDataFromHandle()
		{
			OleDbHResult oleDbHResult = OleDbHResult.S_OK;
			UnsafeNativeMethods.IRowset rowset = this.IRowset();
			IntPtr rowHandle = this._rowHandleNativeBuffer.GetRowHandle(this._currentRow);
			RowBinding rowBinding = this._bindings[this._nextAccessorForRetrieval].RowBinding();
			IntPtr intPtr = rowBinding.DangerousGetAccessorHandle();
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				rowBinding.DangerousAddRef(ref flag);
				rowBinding.StartDataBlock();
				IntPtr intPtr2 = rowBinding.DangerousGetDataPtr();
				Bid.Trace("<oledb.IRowset.GetData|API|OLEDB> %d#, RowHandle=%Id, AccessorHandle=%Id\n", this.ObjectID, rowHandle, intPtr);
				oleDbHResult = rowset.GetData(rowHandle, intPtr, intPtr2);
				Bid.Trace("<oledb.IRowset.GetData|API|OLEDB|RET> %08X{HRESULT}\n", oleDbHResult);
			}
			finally
			{
				if (flag)
				{
					rowBinding.DangerousRelease();
				}
			}
			this._nextAccessorForRetrieval++;
			if (oleDbHResult < OleDbHResult.S_OK)
			{
				this.ProcessResults(oleDbHResult);
			}
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x0025CF90 File Offset: 0x0025C390
		private void ReleaseRowHandles()
		{
			UnsafeNativeMethods.IRowset rowset = this.IRowset();
			Bid.Trace("<oledb.IRowset.ReleaseRows|API|OLEDB> %d#, Request=%Id\n", this.ObjectID, this._rowFetchedCount);
			OleDbHResult oleDbHResult = rowset.ReleaseRows(this._rowFetchedCount, this._rowHandleNativeBuffer, ADP.PtrZero, ADP.PtrZero, ADP.PtrZero);
			Bid.Trace("<oledb.IRowset.ReleaseRows|API|OLEDB|RET> %08X{HRESULT}\n", oleDbHResult);
			if (oleDbHResult < OleDbHResult.S_OK)
			{
				SafeNativeMethods.Wrapper.ClearErrorInfo();
			}
			this._rowFetchedCount = IntPtr.Zero;
			this._currentRow = 0;
			this._isRead = false;
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x0025D00C File Offset: 0x0025C40C
		private object GetPropertyValue(int propertyId)
		{
			if (this._irowset != null)
			{
				return this.GetPropertyOnRowset(OleDbPropertySetGuid.Rowset, propertyId);
			}
			if (this._command != null)
			{
				return this._command.GetPropertyValue(OleDbPropertySetGuid.Rowset, propertyId);
			}
			return OleDbPropertyStatus.NotSupported;
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x0025D050 File Offset: 0x0025C450
		private object GetPropertyOnRowset(Guid propertySet, int propertyID)
		{
			UnsafeNativeMethods.IRowsetInfo rowsetInfo = this.IRowsetInfo();
			tagDBPROP[] propertySet2;
			using (PropertyIDSet propertyIDSet = new PropertyIDSet(propertySet, propertyID))
			{
				OleDbHResult oleDbHResult;
				using (DBPropSet dbpropSet = new DBPropSet(rowsetInfo, propertyIDSet, out oleDbHResult))
				{
					if (oleDbHResult < OleDbHResult.S_OK)
					{
						SafeNativeMethods.Wrapper.ClearErrorInfo();
					}
					propertySet2 = dbpropSet.GetPropertySet(0, out propertySet);
				}
			}
			if (propertySet2[0].dwStatus == OleDbPropertyStatus.Ok)
			{
				return propertySet2[0].vValue;
			}
			return propertySet2[0].dwStatus;
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x0025D0F8 File Offset: 0x0025C4F8
		private void GetRowValue()
		{
			Bindings bindings = this._bindings[this._nextAccessorForRetrieval];
			ColumnBinding[] array = bindings.ColumnBindings();
			RowBinding rowBinding = bindings.RowBinding();
			bool flag = false;
			bool[] array2 = new bool[array.Length];
			StringMemHandle[] array3 = new StringMemHandle[array.Length];
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				for (int i = 0; i < array.Length; i++)
				{
					bindings.CurrentIndex = i;
					array3[i] = null;
					MetaData metaData = this._metadata[array[i].Index];
					if (metaData.kind == 0 || 2 == metaData.kind)
					{
						array3[i] = new StringMemHandle(metaData.idname);
						array[i]._sptr = array3[i];
					}
					array3[i].DangerousAddRef(ref array2[i]);
					IntPtr intPtr = ((array3[i] != null) ? array3[i].DangerousGetHandle() : metaData.propid);
					bindings.GuidKindName(metaData.guid, metaData.kind, intPtr);
				}
				tagDBCOLUMNACCESS[] dbcolumnAccess = bindings.DBColumnAccess;
				rowBinding.DangerousAddRef(ref flag);
				rowBinding.StartDataBlock();
				UnsafeNativeMethods.IRow row = this.IRow();
				Bid.Trace("<oledb.IRow.GetColumns|API|OLEDB> %d#\n", this.ObjectID);
				OleDbHResult columns = row.GetColumns((IntPtr)dbcolumnAccess.Length, dbcolumnAccess);
				Bid.Trace("<oledb.IRow.GetColumns|API|OLEDB|RET> %08X{HRESULT}\n", columns);
			}
			finally
			{
				if (flag)
				{
					rowBinding.DangerousRelease();
				}
				for (int j = 0; j < array2.Length; j++)
				{
					if (array2[j])
					{
						array3[j].DangerousRelease();
					}
				}
			}
			this._nextAccessorForRetrieval++;
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x0025D284 File Offset: 0x0025C684
		private int IndexOf(Hashtable hash, string name)
		{
			object obj = hash[name];
			if (obj != null)
			{
				return (int)obj;
			}
			string text = name.ToLower(CultureInfo.InvariantCulture);
			obj = hash[text];
			if (obj == null)
			{
				return -1;
			}
			return (int)obj;
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x0025D2C4 File Offset: 0x0025C6C4
		private void AppendSchemaInfo()
		{
			if (this._metadata.Length <= 0)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < this._metadata.Length; i++)
			{
				if (this._metadata[i].isKeyColumn && !this._metadata[i].isHidden)
				{
					num++;
				}
			}
			if (num != 0)
			{
				return;
			}
			string text = null;
			string text2 = null;
			string text3 = null;
			for (int j = 0; j < this._metadata.Length; j++)
			{
				MetaData metaData = this._metadata[j];
				if (metaData.baseTableName != null && 0 < metaData.baseTableName.Length)
				{
					string text4 = ((metaData.baseCatalogName != null) ? metaData.baseCatalogName : "");
					string text5 = ((metaData.baseSchemaName != null) ? metaData.baseSchemaName : "");
					if (text3 == null)
					{
						text = text5;
						text2 = text4;
						text3 = metaData.baseTableName;
					}
					else if (ADP.SrcCompare(text3, metaData.baseTableName) != 0 || ADP.SrcCompare(text2, text4) != 0 || ADP.SrcCompare(text, text5) != 0)
					{
						text3 = null;
						break;
					}
				}
			}
			if (text3 == null)
			{
				return;
			}
			text2 = (ADP.IsEmpty(text2) ? null : text2);
			text = (ADP.IsEmpty(text) ? null : text);
			if (this._connection != null && 4 == this._connection.QuotedIdentifierCase())
			{
				string text6 = null;
				string text7 = null;
				this._connection.GetLiteralQuotes("GetSchemaTable", out text7, out text6);
				if (text7 == null)
				{
					text7 = "";
				}
				if (text6 == null)
				{
					text6 = "";
				}
				text3 = text7 + text3 + text6;
			}
			Hashtable hashtable = new Hashtable(this._metadata.Length * 2);
			int num2 = this._metadata.Length - 1;
			while (0 <= num2)
			{
				string baseColumnName = this._metadata[num2].baseColumnName;
				if (!ADP.IsEmpty(baseColumnName))
				{
					hashtable[baseColumnName] = num2;
				}
				num2--;
			}
			for (int k = 0; k < this._metadata.Length; k++)
			{
				string text8 = this._metadata[k].baseColumnName;
				if (!ADP.IsEmpty(text8))
				{
					text8 = text8.ToLower(CultureInfo.InvariantCulture);
					if (!hashtable.Contains(text8))
					{
						hashtable[text8] = k;
					}
				}
			}
			if (this._connection.SupportSchemaRowset(OleDbSchemaGuid.Primary_Keys))
			{
				object[] array = new object[] { text2, text, text3 };
				num = this.AppendSchemaPrimaryKey(hashtable, array);
			}
			if (num != 0)
			{
				return;
			}
			if (this._connection.SupportSchemaRowset(OleDbSchemaGuid.Indexes))
			{
				object[] array2 = new object[] { text2, text, null, null, text3 };
				this.AppendSchemaUniqueIndexAsKey(hashtable, array2);
			}
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x0025D560 File Offset: 0x0025C960
		private int AppendSchemaPrimaryKey(Hashtable baseColumnNames, object[] restrictions)
		{
			int num = 0;
			bool flag = false;
			DataTable dataTable = null;
			try
			{
				dataTable = this._connection.GetSchemaRowset(OleDbSchemaGuid.Primary_Keys, restrictions);
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				ADP.TraceExceptionWithoutRethrow(ex);
			}
			if (dataTable != null)
			{
				DataColumnCollection columns = dataTable.Columns;
				int num2 = columns.IndexOf("COLUMN_NAME");
				if (-1 != num2)
				{
					DataColumn dataColumn = columns[num2];
					foreach (object obj in dataTable.Rows)
					{
						DataRow dataRow = (DataRow)obj;
						string text = (string)dataRow[dataColumn, DataRowVersion.Default];
						int num3 = this.IndexOf(baseColumnNames, text);
						if (0 > num3)
						{
							flag = true;
							break;
						}
						MetaData metaData = this._metadata[num3];
						metaData.isKeyColumn = true;
						metaData.flags &= -33;
						num++;
					}
				}
			}
			if (flag)
			{
				for (int i = 0; i < this._metadata.Length; i++)
				{
					this._metadata[i].isKeyColumn = false;
				}
				return -1;
			}
			return num;
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x0025D6B8 File Offset: 0x0025CAB8
		private void AppendSchemaUniqueIndexAsKey(Hashtable baseColumnNames, object[] restrictions)
		{
			bool flag = false;
			DataTable dataTable = null;
			try
			{
				dataTable = this._connection.GetSchemaRowset(OleDbSchemaGuid.Indexes, restrictions);
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				ADP.TraceExceptionWithoutRethrow(ex);
			}
			if (dataTable != null)
			{
				DataColumnCollection columns = dataTable.Columns;
				int num = columns.IndexOf("INDEX_NAME");
				int num2 = columns.IndexOf("PRIMARY_KEY");
				int num3 = columns.IndexOf("UNIQUE");
				int num4 = columns.IndexOf("COLUMN_NAME");
				int num5 = columns.IndexOf("NULLS");
				if (-1 != num && -1 != num2 && -1 != num3 && -1 != num4)
				{
					DataColumn dataColumn = columns[num];
					DataColumn dataColumn2 = columns[num2];
					DataColumn dataColumn3 = columns[num3];
					DataColumn dataColumn4 = columns[num4];
					DataColumn dataColumn5 = ((-1 != num5) ? columns[num5] : null);
					bool[] array = new bool[this._metadata.Length];
					bool[] array2 = new bool[this._metadata.Length];
					string text = null;
					foreach (object obj in dataTable.Rows)
					{
						DataRow dataRow = (DataRow)obj;
						bool flag2 = !dataRow.IsNull(dataColumn2, DataRowVersion.Default) && (bool)dataRow[dataColumn2, DataRowVersion.Default];
						bool flag3 = !dataRow.IsNull(dataColumn3, DataRowVersion.Default) && (bool)dataRow[dataColumn3, DataRowVersion.Default];
						if (dataColumn5 != null && !dataRow.IsNull(dataColumn5, DataRowVersion.Default))
						{
							Convert.ToInt32(dataRow[dataColumn5, DataRowVersion.Default], CultureInfo.InvariantCulture);
						}
						if (flag2 || flag3)
						{
							string text2 = (string)dataRow[dataColumn4, DataRowVersion.Default];
							int num6 = this.IndexOf(baseColumnNames, text2);
							if (0 <= num6)
							{
								if (flag2)
								{
									array[num6] = true;
								}
								if (flag3 && array2 != null)
								{
									array2[num6] = true;
									string text3 = (string)dataRow[dataColumn, DataRowVersion.Default];
									if (text == null)
									{
										text = text3;
									}
									else if (text3 != text)
									{
										array2 = null;
									}
								}
							}
							else
							{
								if (flag2)
								{
									flag = true;
									break;
								}
								if (text != null)
								{
									string text4 = (string)dataRow[dataColumn, DataRowVersion.Default];
									if (text4 != text)
									{
										array2 = null;
									}
								}
							}
						}
					}
					if (flag)
					{
						for (int i = 0; i < this._metadata.Length; i++)
						{
							this._metadata[i].isKeyColumn = false;
						}
						return;
					}
					if (array2 != null)
					{
						for (int j = 0; j < this._metadata.Length; j++)
						{
							this._metadata[j].isKeyColumn = array2[j];
						}
					}
				}
			}
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x0025D9A0 File Offset: 0x0025CDA0
		private MetaData FindMetaData(string name)
		{
			int num = this._fieldNameLookup.IndexOfName(name);
			if (-1 == num)
			{
				return null;
			}
			return this._metadata[num];
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x0025D9C8 File Offset: 0x0025CDC8
		internal void DumpToSchemaTable(UnsafeNativeMethods.IRowset rowset)
		{
			List<MetaData> list = new List<MetaData>();
			object obj = null;
			using (OleDbDataReader oleDbDataReader = new OleDbDataReader(this._connection, this._command, int.MinValue, CommandBehavior.Default))
			{
				oleDbDataReader.InitializeIRowset(rowset, ChapterHandle.DB_NULL_HCHAPTER, IntPtr.Zero);
				oleDbDataReader.BuildSchemaTableInfo(rowset, true, false);
				obj = this.GetPropertyValue(258);
				if (oleDbDataReader.FieldCount == 0)
				{
					return;
				}
				FieldNameLookup fieldNameLookup = new FieldNameLookup(oleDbDataReader, -1);
				oleDbDataReader._fieldNameLookup = fieldNameLookup;
				MetaData metaData = oleDbDataReader.FindMetaData("DBCOLUMN_IDNAME");
				MetaData metaData2 = oleDbDataReader.FindMetaData("DBCOLUMN_GUID");
				MetaData metaData3 = oleDbDataReader.FindMetaData("DBCOLUMN_PROPID");
				MetaData metaData4 = oleDbDataReader.FindMetaData("DBCOLUMN_NAME");
				MetaData metaData5 = oleDbDataReader.FindMetaData("DBCOLUMN_NUMBER");
				MetaData metaData6 = oleDbDataReader.FindMetaData("DBCOLUMN_TYPE");
				MetaData metaData7 = oleDbDataReader.FindMetaData("DBCOLUMN_COLUMNSIZE");
				MetaData metaData8 = oleDbDataReader.FindMetaData("DBCOLUMN_PRECISION");
				MetaData metaData9 = oleDbDataReader.FindMetaData("DBCOLUMN_SCALE");
				MetaData metaData10 = oleDbDataReader.FindMetaData("DBCOLUMN_FLAGS");
				MetaData metaData11 = oleDbDataReader.FindMetaData("DBCOLUMN_BASESCHEMANAME");
				MetaData metaData12 = oleDbDataReader.FindMetaData("DBCOLUMN_BASECATALOGNAME");
				MetaData metaData13 = oleDbDataReader.FindMetaData("DBCOLUMN_BASETABLENAME");
				MetaData metaData14 = oleDbDataReader.FindMetaData("DBCOLUMN_BASECOLUMNNAME");
				MetaData metaData15 = oleDbDataReader.FindMetaData("DBCOLUMN_ISAUTOINCREMENT");
				MetaData metaData16 = oleDbDataReader.FindMetaData("DBCOLUMN_ISUNIQUE");
				MetaData metaData17 = oleDbDataReader.FindMetaData("DBCOLUMN_KEYCOLUMN");
				oleDbDataReader.CreateAccessors(false);
				while (oleDbDataReader.ReadRowset())
				{
					oleDbDataReader.GetRowDataFromHandle();
					MetaData metaData18 = new MetaData();
					ColumnBinding columnBinding = metaData.columnBinding;
					if (!columnBinding.IsValueNull())
					{
						metaData18.idname = (string)columnBinding.Value();
						metaData18.kind = 2;
					}
					columnBinding = metaData2.columnBinding;
					if (!columnBinding.IsValueNull())
					{
						metaData18.guid = columnBinding.Value_GUID();
						metaData18.kind = ((2 == metaData18.kind) ? 0 : 6);
					}
					columnBinding = metaData3.columnBinding;
					if (!columnBinding.IsValueNull())
					{
						metaData18.propid = new IntPtr((long)((ulong)columnBinding.Value_UI4()));
						metaData18.kind = ((6 == metaData18.kind) ? 1 : 5);
					}
					columnBinding = metaData4.columnBinding;
					if (!columnBinding.IsValueNull())
					{
						metaData18.columnName = (string)columnBinding.Value();
					}
					else
					{
						metaData18.columnName = "";
					}
					if (4 == ADP.PtrSize)
					{
						metaData18.ordinal = (IntPtr)((long)((ulong)metaData5.columnBinding.Value_UI4()));
					}
					else
					{
						metaData18.ordinal = (IntPtr)((long)metaData5.columnBinding.Value_UI8());
					}
					short num = (short)metaData6.columnBinding.Value_UI2();
					if (4 == ADP.PtrSize)
					{
						metaData18.size = (int)metaData7.columnBinding.Value_UI4();
					}
					else
					{
						metaData18.size = ADP.IntPtrToInt32((IntPtr)((long)metaData7.columnBinding.Value_UI8()));
					}
					columnBinding = metaData8.columnBinding;
					if (!columnBinding.IsValueNull())
					{
						metaData18.precision = (byte)columnBinding.Value_UI2();
					}
					columnBinding = metaData9.columnBinding;
					if (!columnBinding.IsValueNull())
					{
						metaData18.scale = (byte)columnBinding.Value_I2();
					}
					metaData18.flags = (int)metaData10.columnBinding.Value_UI4();
					bool flag = OleDbDataReader.IsLong(metaData18.flags);
					bool flag2 = OleDbDataReader.IsFixed(metaData18.flags);
					NativeDBType nativeDBType = NativeDBType.FromDBType(num, flag, flag2);
					metaData18.type = nativeDBType;
					if (metaData15 != null)
					{
						columnBinding = metaData15.columnBinding;
						if (!columnBinding.IsValueNull())
						{
							metaData18.isAutoIncrement = columnBinding.Value_BOOL();
						}
					}
					if (metaData16 != null)
					{
						columnBinding = metaData16.columnBinding;
						if (!columnBinding.IsValueNull())
						{
							metaData18.isUnique = columnBinding.Value_BOOL();
						}
					}
					if (metaData17 != null)
					{
						columnBinding = metaData17.columnBinding;
						if (!columnBinding.IsValueNull())
						{
							metaData18.isKeyColumn = columnBinding.Value_BOOL();
						}
					}
					if (metaData11 != null)
					{
						columnBinding = metaData11.columnBinding;
						if (!columnBinding.IsValueNull())
						{
							metaData18.baseSchemaName = columnBinding.ValueString();
						}
					}
					if (metaData12 != null)
					{
						columnBinding = metaData12.columnBinding;
						if (!columnBinding.IsValueNull())
						{
							metaData18.baseCatalogName = columnBinding.ValueString();
						}
					}
					if (metaData13 != null)
					{
						columnBinding = metaData13.columnBinding;
						if (!columnBinding.IsValueNull())
						{
							metaData18.baseTableName = columnBinding.ValueString();
						}
					}
					if (metaData14 != null)
					{
						columnBinding = metaData14.columnBinding;
						if (!columnBinding.IsValueNull())
						{
							metaData18.baseColumnName = columnBinding.ValueString();
						}
					}
					list.Add(metaData18);
				}
			}
			int i = list.Count;
			if (obj is int)
			{
				i -= (int)obj;
			}
			int num2 = list.Count - 1;
			while (i <= num2)
			{
				MetaData metaData19 = list[num2];
				metaData19.isHidden = true;
				if (metaData19.guid.Equals(ODB.DBCOL_SPECIALCOL))
				{
					metaData19.isKeyColumn = false;
				}
				num2--;
			}
			int num3 = i - 1;
			while (0 <= num3)
			{
				MetaData metaData20 = list[num3];
				if (metaData20.guid.Equals(ODB.DBCOL_SPECIALCOL))
				{
					metaData20.isHidden = true;
					i--;
				}
				else if (0 >= (int)metaData20.ordinal)
				{
					metaData20.isHidden = true;
					i--;
				}
				else if (OleDbDataReader.DoColumnDropFilter(metaData20.flags))
				{
					metaData20.isHidden = true;
					i--;
				}
				num3--;
			}
			list.Sort();
			this._visibleFieldCount = i;
			this._metadata = list.ToArray();
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x0025DF00 File Offset: 0x0025D300
		internal static void GenerateSchemaTable(OleDbDataReader dataReader, object handle, CommandBehavior behavior)
		{
			if ((CommandBehavior.KeyInfo & behavior) != CommandBehavior.Default)
			{
				dataReader.BuildSchemaTableRowset(handle);
				dataReader.AppendSchemaInfo();
			}
			else
			{
				dataReader.BuildSchemaTableInfo(handle, false, false);
			}
			MetaData[] metaData = dataReader.MetaData;
			if (metaData != null && 0 < metaData.Length)
			{
				dataReader.BuildSchemaTable(metaData);
			}
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x0025DF44 File Offset: 0x0025D344
		private static bool DoColumnDropFilter(int flags)
		{
			return 0 != (1 & flags);
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x0025DF5C File Offset: 0x0025D35C
		private static bool IsLong(int flags)
		{
			return 0 != (128 & flags);
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x0025DF78 File Offset: 0x0025D378
		private static bool IsFixed(int flags)
		{
			return 0 != (16 & flags);
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x0025DF90 File Offset: 0x0025D390
		private static bool IsRowVersion(int flags)
		{
			return 0 != (768 & flags);
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x0025DFAC File Offset: 0x0025D3AC
		private static bool AllowDBNull(int flags)
		{
			return 0 != (32 & flags);
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x0025DFC4 File Offset: 0x0025D3C4
		private static bool AllowDBNullMaybeNull(int flags)
		{
			return 0 != (96 & flags);
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x0025DFDC File Offset: 0x0025D3DC
		private static bool IsReadOnly(int flags)
		{
			return 0 == (12 & flags);
		}

		// Token: 0x040012AC RID: 4780
		private CommandBehavior _commandBehavior;

		// Token: 0x040012AD RID: 4781
		private static int _objectTypeCount;

		// Token: 0x040012AE RID: 4782
		internal readonly int ObjectID = Interlocked.Increment(ref OleDbDataReader._objectTypeCount);

		// Token: 0x040012AF RID: 4783
		private OleDbConnection _connection;

		// Token: 0x040012B0 RID: 4784
		private OleDbCommand _command;

		// Token: 0x040012B1 RID: 4785
		private Bindings _parameterBindings;

		// Token: 0x040012B2 RID: 4786
		private UnsafeNativeMethods.IMultipleResults _imultipleResults;

		// Token: 0x040012B3 RID: 4787
		private UnsafeNativeMethods.IRowset _irowset;

		// Token: 0x040012B4 RID: 4788
		private UnsafeNativeMethods.IRow _irow;

		// Token: 0x040012B5 RID: 4789
		private ChapterHandle _chapterHandle = ChapterHandle.DB_NULL_HCHAPTER;

		// Token: 0x040012B6 RID: 4790
		private int _depth;

		// Token: 0x040012B7 RID: 4791
		private bool _isClosed;

		// Token: 0x040012B8 RID: 4792
		private bool _isRead;

		// Token: 0x040012B9 RID: 4793
		private bool _hasRows;

		// Token: 0x040012BA RID: 4794
		private bool _hasRowsReadCheck;

		// Token: 0x040012BB RID: 4795
		private long _sequentialBytesRead;

		// Token: 0x040012BC RID: 4796
		private int _sequentialOrdinal;

		// Token: 0x040012BD RID: 4797
		private Bindings[] _bindings;

		// Token: 0x040012BE RID: 4798
		private int _nextAccessorForRetrieval;

		// Token: 0x040012BF RID: 4799
		private int _nextValueForRetrieval;

		// Token: 0x040012C0 RID: 4800
		private IntPtr _recordsAffected = ADP.RecordsUnaffected;

		// Token: 0x040012C1 RID: 4801
		private bool _useIColumnsRowset;

		// Token: 0x040012C2 RID: 4802
		private bool _sequentialAccess;

		// Token: 0x040012C3 RID: 4803
		private bool _singleRow;

		// Token: 0x040012C4 RID: 4804
		private IntPtr _rowHandleFetchCount;

		// Token: 0x040012C5 RID: 4805
		private RowHandleBuffer _rowHandleNativeBuffer;

		// Token: 0x040012C6 RID: 4806
		private IntPtr _rowFetchedCount;

		// Token: 0x040012C7 RID: 4807
		private int _currentRow;

		// Token: 0x040012C8 RID: 4808
		private DataTable _dbSchemaTable;

		// Token: 0x040012C9 RID: 4809
		private int _visibleFieldCount;

		// Token: 0x040012CA RID: 4810
		private MetaData[] _metadata;

		// Token: 0x040012CB RID: 4811
		private FieldNameLookup _fieldNameLookup;
	}
}
