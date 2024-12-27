using System;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Data.SqlTypes;
using System.Globalization;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x020002E0 RID: 736
	internal sealed class SqlDataReaderSmi : SqlDataReader
	{
		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x0600263C RID: 9788 RVA: 0x00281434 File Offset: 0x00280834
		public override int FieldCount
		{
			get
			{
				this.ThrowIfClosed("FieldCount");
				return this.InternalFieldCount;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x0600263D RID: 9789 RVA: 0x00281454 File Offset: 0x00280854
		public override int VisibleFieldCount
		{
			get
			{
				this.ThrowIfClosed("VisibleFieldCount");
				if (this.FNotInResults())
				{
					return 0;
				}
				return this._visibleColumnCount;
			}
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x0028147C File Offset: 0x0028087C
		public override string GetName(int ordinal)
		{
			this.EnsureCanGetMetaData("GetName");
			return this._currentMetaData[ordinal].Name;
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x002814A4 File Offset: 0x002808A4
		public override string GetDataTypeName(int ordinal)
		{
			this.EnsureCanGetMetaData("GetDataTypeName");
			SmiExtendedMetaData smiExtendedMetaData = this._currentMetaData[ordinal];
			if (SqlDbType.Udt == smiExtendedMetaData.SqlDbType)
			{
				return string.Concat(new string[] { smiExtendedMetaData.TypeSpecificNamePart1, ".", smiExtendedMetaData.TypeSpecificNamePart2, ".", smiExtendedMetaData.TypeSpecificNamePart3 });
			}
			return smiExtendedMetaData.TypeName;
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x00281510 File Offset: 0x00280910
		public override Type GetFieldType(int ordinal)
		{
			this.EnsureCanGetMetaData("GetFieldType");
			if (SqlDbType.Udt == this._currentMetaData[ordinal].SqlDbType)
			{
				return this._currentMetaData[ordinal].Type;
			}
			return MetaType.GetMetaTypeFromSqlDbType(this._currentMetaData[ordinal].SqlDbType, this._currentMetaData[ordinal].IsMultiValued).ClassType;
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x0028156C File Offset: 0x0028096C
		public override Type GetProviderSpecificFieldType(int ordinal)
		{
			this.EnsureCanGetMetaData("GetProviderSpecificFieldType");
			if (SqlDbType.Udt == this._currentMetaData[ordinal].SqlDbType)
			{
				return this._currentMetaData[ordinal].Type;
			}
			return MetaType.GetMetaTypeFromSqlDbType(this._currentMetaData[ordinal].SqlDbType, this._currentMetaData[ordinal].IsMultiValued).SqlType;
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06002642 RID: 9794 RVA: 0x002815C8 File Offset: 0x002809C8
		public override int Depth
		{
			get
			{
				this.ThrowIfClosed("Depth");
				return 0;
			}
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x002815E4 File Offset: 0x002809E4
		public override object GetValue(int ordinal)
		{
			this.EnsureCanGetCol("GetValue", ordinal);
			SmiQueryMetaData smiQueryMetaData = this._currentMetaData[ordinal];
			if (this._currentConnection.IsKatmaiOrNewer)
			{
				return ValueUtilsSmi.GetValue200(this._readerEventSink, (SmiTypedGetterSetter)this._currentColumnValuesV3, ordinal, smiQueryMetaData, this._currentConnection.InternalContext);
			}
			return ValueUtilsSmi.GetValue(this._readerEventSink, this._currentColumnValuesV3, ordinal, smiQueryMetaData, this._currentConnection.InternalContext);
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x00281658 File Offset: 0x00280A58
		public override int GetValues(object[] values)
		{
			this.EnsureCanGetCol("GetValues", 0);
			if (values == null)
			{
				throw ADP.ArgumentNull("values");
			}
			int num = ((values.Length < this._visibleColumnCount) ? values.Length : this._visibleColumnCount);
			for (int i = 0; i < num; i++)
			{
				values[this._indexMap[i]] = this.GetValue(i);
			}
			return num;
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x002816B4 File Offset: 0x00280AB4
		public override int GetOrdinal(string name)
		{
			this.EnsureCanGetMetaData("GetOrdinal");
			if (this._fieldNameLookup == null)
			{
				this._fieldNameLookup = new FieldNameLookup(this, -1);
			}
			return this._fieldNameLookup.GetOrdinal(name);
		}

		// Token: 0x17000607 RID: 1543
		public override object this[int ordinal]
		{
			get
			{
				return this.GetValue(ordinal);
			}
		}

		// Token: 0x17000608 RID: 1544
		public override object this[string strName]
		{
			get
			{
				return this.GetValue(this.GetOrdinal(strName));
			}
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x00281720 File Offset: 0x00280B20
		public override bool IsDBNull(int ordinal)
		{
			this.EnsureCanGetCol("IsDBNull", ordinal);
			return ValueUtilsSmi.IsDBNull(this._readerEventSink, this._currentColumnValuesV3, ordinal);
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x0028174C File Offset: 0x00280B4C
		public override bool GetBoolean(int ordinal)
		{
			this.EnsureCanGetCol("GetBoolean", ordinal);
			return ValueUtilsSmi.GetBoolean(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x00281780 File Offset: 0x00280B80
		public override byte GetByte(int ordinal)
		{
			this.EnsureCanGetCol("GetByte", ordinal);
			return ValueUtilsSmi.GetByte(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x002817B4 File Offset: 0x00280BB4
		public override long GetBytes(int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			this.EnsureCanGetCol("GetBytes", ordinal);
			return ValueUtilsSmi.GetBytes(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal], fieldOffset, buffer, bufferOffset, length, true);
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x002817F0 File Offset: 0x00280BF0
		internal override long GetBytesInternal(int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			this.EnsureCanGetCol("GetBytes", ordinal);
			return ValueUtilsSmi.GetBytesInternal(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal], fieldOffset, buffer, bufferOffset, length, false);
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x0028182C File Offset: 0x00280C2C
		public override char GetChar(int ordinal)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x00281840 File Offset: 0x00280C40
		public override long GetChars(int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			this.EnsureCanGetCol("GetChars", ordinal);
			SmiExtendedMetaData smiExtendedMetaData = this._currentMetaData[ordinal];
			if (base.IsCommandBehavior(CommandBehavior.SequentialAccess) && smiExtendedMetaData.SqlDbType == SqlDbType.Xml)
			{
				return base.GetStreamingXmlChars(ordinal, fieldOffset, buffer, bufferOffset, length);
			}
			return ValueUtilsSmi.GetChars(this._readerEventSink, this._currentColumnValuesV3, ordinal, smiExtendedMetaData, fieldOffset, buffer, bufferOffset, length);
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x002818A0 File Offset: 0x00280CA0
		public override Guid GetGuid(int ordinal)
		{
			this.EnsureCanGetCol("GetGuid", ordinal);
			return ValueUtilsSmi.GetGuid(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x002818D4 File Offset: 0x00280CD4
		public override short GetInt16(int ordinal)
		{
			this.EnsureCanGetCol("GetInt16", ordinal);
			return ValueUtilsSmi.GetInt16(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x00281908 File Offset: 0x00280D08
		public override int GetInt32(int ordinal)
		{
			this.EnsureCanGetCol("GetInt32", ordinal);
			return ValueUtilsSmi.GetInt32(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x0028193C File Offset: 0x00280D3C
		public override long GetInt64(int ordinal)
		{
			this.EnsureCanGetCol("GetInt64", ordinal);
			return ValueUtilsSmi.GetInt64(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x00281970 File Offset: 0x00280D70
		public override float GetFloat(int ordinal)
		{
			this.EnsureCanGetCol("GetFloat", ordinal);
			return ValueUtilsSmi.GetSingle(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x002819A4 File Offset: 0x00280DA4
		public override double GetDouble(int ordinal)
		{
			this.EnsureCanGetCol("GetDouble", ordinal);
			return ValueUtilsSmi.GetDouble(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x002819D8 File Offset: 0x00280DD8
		public override string GetString(int ordinal)
		{
			this.EnsureCanGetCol("GetString", ordinal);
			return ValueUtilsSmi.GetString(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x00281A0C File Offset: 0x00280E0C
		public override decimal GetDecimal(int ordinal)
		{
			this.EnsureCanGetCol("GetDecimal", ordinal);
			return ValueUtilsSmi.GetDecimal(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x00281A40 File Offset: 0x00280E40
		public override DateTime GetDateTime(int ordinal)
		{
			this.EnsureCanGetCol("GetDateTime", ordinal);
			return ValueUtilsSmi.GetDateTime(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06002658 RID: 9816 RVA: 0x00281A74 File Offset: 0x00280E74
		public override bool IsClosed
		{
			get
			{
				return this.IsReallyClosed();
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06002659 RID: 9817 RVA: 0x00281A88 File Offset: 0x00280E88
		public override int RecordsAffected
		{
			get
			{
				return base.Command.InternalRecordsAffected;
			}
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x00281AA0 File Offset: 0x00280EA0
		public override void Close()
		{
			bool flag = base.IsCommandBehavior(CommandBehavior.CloseConnection);
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlDataReaderSmi.Close|API> %d#", this.ObjectID);
			bool flag2 = true;
			try
			{
				if (!this.IsClosed)
				{
					this._hasRows = false;
					while (this._eventStream.HasEvents)
					{
						this._eventStream.ProcessEvent(this._readerEventSink);
						this._readerEventSink.ProcessMessagesAndThrow(true);
					}
				}
			}
			catch (Exception ex)
			{
				flag2 = ADP.IsCatchableExceptionType(ex);
				throw;
			}
			finally
			{
				if (flag2)
				{
					this._isOpen = false;
					if (flag)
					{
						if (base.Connection != null)
						{
							base.Connection.Close();
						}
						Bid.ScopeLeave(ref intPtr);
					}
				}
			}
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x00281B74 File Offset: 0x00280F74
		public override bool NextResult()
		{
			this.ThrowIfClosed("NextResult");
			return this.InternalNextResult(false);
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x00281B98 File Offset: 0x00280F98
		internal bool InternalNextResult(bool ignoreNonFatalMessages)
		{
			IntPtr zero = IntPtr.Zero;
			if (Bid.AdvancedOn)
			{
				Bid.ScopeEnter(out zero, "<sc.SqlDataReaderSmi.InternalNextResult|ADV> %d#", this.ObjectID);
			}
			bool flag;
			try
			{
				this._hasRows = false;
				if (SqlDataReaderSmi.PositionState.AfterResults != this._currentPosition)
				{
					while (this.InternalRead(ignoreNonFatalMessages))
					{
					}
					while (this._currentMetaData == null && this._eventStream.HasEvents)
					{
						this._eventStream.ProcessEvent(this._readerEventSink);
						this._readerEventSink.ProcessMessagesAndThrow(ignoreNonFatalMessages);
					}
				}
				flag = SqlDataReaderSmi.PositionState.AfterResults != this._currentPosition;
			}
			finally
			{
				if (Bid.AdvancedOn)
				{
					Bid.ScopeLeave(ref zero);
				}
			}
			return flag;
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x00281C50 File Offset: 0x00281050
		public override bool Read()
		{
			this.ThrowIfClosed("Read");
			return this.InternalRead(false);
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x00281C74 File Offset: 0x00281074
		internal bool InternalRead(bool ignoreNonFatalErrors)
		{
			IntPtr zero = IntPtr.Zero;
			if (Bid.AdvancedOn)
			{
				Bid.ScopeEnter(out zero, "<sc.SqlDataReaderSmi.InternalRead|ADV> %d#", this.ObjectID);
			}
			bool flag;
			try
			{
				if (this.FInResults())
				{
					this._currentColumnValues = null;
					this._currentColumnValuesV3 = null;
					while (this._currentColumnValues == null && this._currentColumnValuesV3 == null && this.FInResults() && SqlDataReaderSmi.PositionState.AfterRows != this._currentPosition && this._eventStream.HasEvents)
					{
						this._eventStream.ProcessEvent(this._readerEventSink);
						this._readerEventSink.ProcessMessagesAndThrow(ignoreNonFatalErrors);
					}
				}
				flag = SqlDataReaderSmi.PositionState.OnRow == this._currentPosition;
			}
			finally
			{
				if (Bid.AdvancedOn)
				{
					Bid.ScopeLeave(ref zero);
				}
			}
			return flag;
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x00281D3C File Offset: 0x0028113C
		public override DataTable GetSchemaTable()
		{
			this.ThrowIfClosed("GetSchemaTable");
			if (this._schemaTable == null && this.FInResults())
			{
				DataTable dataTable = new DataTable("SchemaTable");
				dataTable.Locale = CultureInfo.InvariantCulture;
				dataTable.MinimumCapacity = this.InternalFieldCount;
				DataColumn dataColumn = new DataColumn(SchemaTableColumn.ColumnName, typeof(string));
				DataColumn dataColumn2 = new DataColumn(SchemaTableColumn.ColumnOrdinal, typeof(int));
				DataColumn dataColumn3 = new DataColumn(SchemaTableColumn.ColumnSize, typeof(int));
				DataColumn dataColumn4 = new DataColumn(SchemaTableColumn.NumericPrecision, typeof(short));
				DataColumn dataColumn5 = new DataColumn(SchemaTableColumn.NumericScale, typeof(short));
				DataColumn dataColumn6 = new DataColumn(SchemaTableColumn.DataType, typeof(Type));
				DataColumn dataColumn7 = new DataColumn(SchemaTableOptionalColumn.ProviderSpecificDataType, typeof(Type));
				DataColumn dataColumn8 = new DataColumn(SchemaTableColumn.ProviderType, typeof(int));
				DataColumn dataColumn9 = new DataColumn(SchemaTableColumn.NonVersionedProviderType, typeof(int));
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
				columns.Add(dataColumn8);
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
				columns.Add(dataColumn9);
				columns.Add(dataColumn31);
				int i = 0;
				while (i < this.InternalFieldCount)
				{
					SmiQueryMetaData smiQueryMetaData = this._currentMetaData[i];
					long num = smiQueryMetaData.MaxLength;
					MetaType metaType = MetaType.GetMetaTypeFromSqlDbType(smiQueryMetaData.SqlDbType, smiQueryMetaData.IsMultiValued);
					if (-1L == num)
					{
						metaType = MetaType.GetMaxMetaTypeFromMetaType(metaType);
						num = ((metaType.IsSizeInCharacters && !metaType.IsPlp) ? 1073741823L : 2147483647L);
					}
					DataRow dataRow = dataTable.NewRow();
					if (SqlDbType.Decimal == smiQueryMetaData.SqlDbType)
					{
						num = 17L;
					}
					else if (SqlDbType.Variant == smiQueryMetaData.SqlDbType)
					{
						num = 8009L;
					}
					dataRow[dataColumn] = smiQueryMetaData.Name;
					dataRow[dataColumn2] = i;
					dataRow[dataColumn3] = num;
					dataRow[dataColumn8] = (int)smiQueryMetaData.SqlDbType;
					dataRow[dataColumn9] = (int)smiQueryMetaData.SqlDbType;
					if (smiQueryMetaData.SqlDbType != SqlDbType.Udt)
					{
						dataRow[dataColumn6] = metaType.ClassType;
						dataRow[dataColumn7] = metaType.SqlType;
					}
					else
					{
						dataRow[dataColumn27] = smiQueryMetaData.Type.AssemblyQualifiedName;
						dataRow[dataColumn6] = smiQueryMetaData.Type;
						dataRow[dataColumn7] = smiQueryMetaData.Type;
					}
					byte b;
					switch (smiQueryMetaData.SqlDbType)
					{
					case SqlDbType.BigInt:
					case SqlDbType.DateTime:
					case SqlDbType.Decimal:
					case SqlDbType.Int:
					case SqlDbType.Money:
					case SqlDbType.SmallDateTime:
					case SqlDbType.SmallInt:
					case SqlDbType.SmallMoney:
					case SqlDbType.TinyInt:
						b = smiQueryMetaData.Precision;
						break;
					case SqlDbType.Binary:
					case SqlDbType.Bit:
					case SqlDbType.Char:
					case SqlDbType.Image:
					case SqlDbType.NChar:
					case SqlDbType.NText:
					case SqlDbType.NVarChar:
					case SqlDbType.UniqueIdentifier:
					case SqlDbType.Text:
					case SqlDbType.Timestamp:
						goto IL_05B6;
					case SqlDbType.Float:
						b = 15;
						break;
					case SqlDbType.Real:
						b = 7;
						break;
					default:
						goto IL_05B6;
					}
					IL_05BD:
					dataRow[dataColumn4] = b;
					if (SqlDbType.Decimal == smiQueryMetaData.SqlDbType || SqlDbType.Time == smiQueryMetaData.SqlDbType || SqlDbType.DateTime2 == smiQueryMetaData.SqlDbType || SqlDbType.DateTimeOffset == smiQueryMetaData.SqlDbType)
					{
						dataRow[dataColumn5] = smiQueryMetaData.Scale;
					}
					else
					{
						dataRow[dataColumn5] = MetaType.GetMetaTypeFromSqlDbType(smiQueryMetaData.SqlDbType, smiQueryMetaData.IsMultiValued).Scale;
					}
					dataRow[dataColumn11] = smiQueryMetaData.AllowsDBNull;
					if (!smiQueryMetaData.IsAliased.IsNull)
					{
						dataRow[dataColumn23] = smiQueryMetaData.IsAliased.Value;
					}
					if (!smiQueryMetaData.IsKey.IsNull)
					{
						dataRow[dataColumn15] = smiQueryMetaData.IsKey.Value;
					}
					if (!smiQueryMetaData.IsHidden.IsNull)
					{
						dataRow[dataColumn17] = smiQueryMetaData.IsHidden.Value;
					}
					if (!smiQueryMetaData.IsExpression.IsNull)
					{
						dataRow[dataColumn24] = smiQueryMetaData.IsExpression.Value;
					}
					dataRow[dataColumn12] = smiQueryMetaData.IsReadOnly;
					dataRow[dataColumn25] = smiQueryMetaData.IsIdentity;
					dataRow[dataColumn31] = smiQueryMetaData.IsColumnSet;
					dataRow[dataColumn16] = smiQueryMetaData.IsIdentity;
					dataRow[dataColumn10] = metaType.IsLong;
					if (SqlDbType.Timestamp == smiQueryMetaData.SqlDbType)
					{
						dataRow[dataColumn14] = true;
						dataRow[dataColumn13] = true;
					}
					else
					{
						dataRow[dataColumn14] = false;
						dataRow[dataColumn13] = false;
					}
					if (!ADP.IsEmpty(smiQueryMetaData.ColumnName))
					{
						dataRow[dataColumn21] = smiQueryMetaData.ColumnName;
					}
					else if (!ADP.IsEmpty(smiQueryMetaData.Name))
					{
						dataRow[dataColumn21] = smiQueryMetaData.Name;
					}
					if (!ADP.IsEmpty(smiQueryMetaData.TableName))
					{
						dataRow[dataColumn20] = smiQueryMetaData.TableName;
					}
					if (!ADP.IsEmpty(smiQueryMetaData.SchemaName))
					{
						dataRow[dataColumn19] = smiQueryMetaData.SchemaName;
					}
					if (!ADP.IsEmpty(smiQueryMetaData.CatalogName))
					{
						dataRow[dataColumn18] = smiQueryMetaData.CatalogName;
					}
					if (!ADP.IsEmpty(smiQueryMetaData.ServerName))
					{
						dataRow[dataColumn22] = smiQueryMetaData.ServerName;
					}
					if (SqlDbType.Udt == smiQueryMetaData.SqlDbType)
					{
						dataRow[dataColumn26] = string.Concat(new string[] { smiQueryMetaData.TypeSpecificNamePart1, ".", smiQueryMetaData.TypeSpecificNamePart2, ".", smiQueryMetaData.TypeSpecificNamePart3 });
					}
					else
					{
						dataRow[dataColumn26] = metaType.TypeName;
					}
					if (SqlDbType.Xml == smiQueryMetaData.SqlDbType)
					{
						dataRow[dataColumn28] = smiQueryMetaData.TypeSpecificNamePart1;
						dataRow[dataColumn29] = smiQueryMetaData.TypeSpecificNamePart2;
						dataRow[dataColumn30] = smiQueryMetaData.TypeSpecificNamePart3;
					}
					dataTable.Rows.Add(dataRow);
					dataRow.AcceptChanges();
					i++;
					continue;
					IL_05B6:
					b = byte.MaxValue;
					goto IL_05BD;
				}
				foreach (object obj in columns)
				{
					DataColumn dataColumn32 = (DataColumn)obj;
					dataColumn32.ReadOnly = true;
				}
				this._schemaTable = dataTable;
			}
			return this._schemaTable;
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x002826A8 File Offset: 0x00281AA8
		public override SqlBinary GetSqlBinary(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlBinary", ordinal);
			return ValueUtilsSmi.GetSqlBinary(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x002826DC File Offset: 0x00281ADC
		public override SqlBoolean GetSqlBoolean(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlBoolean", ordinal);
			return ValueUtilsSmi.GetSqlBoolean(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x00282710 File Offset: 0x00281B10
		public override SqlByte GetSqlByte(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlByte", ordinal);
			return ValueUtilsSmi.GetSqlByte(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x00282744 File Offset: 0x00281B44
		public override SqlInt16 GetSqlInt16(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlInt16", ordinal);
			return ValueUtilsSmi.GetSqlInt16(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x00282778 File Offset: 0x00281B78
		public override SqlInt32 GetSqlInt32(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlInt32", ordinal);
			return ValueUtilsSmi.GetSqlInt32(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x002827AC File Offset: 0x00281BAC
		public override SqlInt64 GetSqlInt64(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlInt64", ordinal);
			return ValueUtilsSmi.GetSqlInt64(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x002827E0 File Offset: 0x00281BE0
		public override SqlSingle GetSqlSingle(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlSingle", ordinal);
			return ValueUtilsSmi.GetSqlSingle(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x00282814 File Offset: 0x00281C14
		public override SqlDouble GetSqlDouble(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlDouble", ordinal);
			return ValueUtilsSmi.GetSqlDouble(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x00282848 File Offset: 0x00281C48
		public override SqlMoney GetSqlMoney(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlMoney", ordinal);
			return ValueUtilsSmi.GetSqlMoney(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x0028287C File Offset: 0x00281C7C
		public override SqlDateTime GetSqlDateTime(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlDateTime", ordinal);
			return ValueUtilsSmi.GetSqlDateTime(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x002828B0 File Offset: 0x00281CB0
		public override SqlDecimal GetSqlDecimal(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlDecimal", ordinal);
			return ValueUtilsSmi.GetSqlDecimal(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x002828E4 File Offset: 0x00281CE4
		public override SqlString GetSqlString(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlString", ordinal);
			return ValueUtilsSmi.GetSqlString(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x00282918 File Offset: 0x00281D18
		public override SqlGuid GetSqlGuid(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlGuid", ordinal);
			return ValueUtilsSmi.GetSqlGuid(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal]);
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x0028294C File Offset: 0x00281D4C
		public override SqlChars GetSqlChars(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlChars", ordinal);
			return ValueUtilsSmi.GetSqlChars(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal], this._currentConnection.InternalContext);
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x0028298C File Offset: 0x00281D8C
		public override SqlBytes GetSqlBytes(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlBytes", ordinal);
			return ValueUtilsSmi.GetSqlBytes(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal], this._currentConnection.InternalContext);
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x002829CC File Offset: 0x00281DCC
		public override SqlXml GetSqlXml(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlXml", ordinal);
			return ValueUtilsSmi.GetSqlXml(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal], this._currentConnection.InternalContext);
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x00282A0C File Offset: 0x00281E0C
		public override TimeSpan GetTimeSpan(int ordinal)
		{
			this.EnsureCanGetCol("GetTimeSpan", ordinal);
			return ValueUtilsSmi.GetTimeSpan(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal], this._currentConnection.IsKatmaiOrNewer);
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x00282A4C File Offset: 0x00281E4C
		public override DateTimeOffset GetDateTimeOffset(int ordinal)
		{
			this.EnsureCanGetCol("GetDateTimeOffset", ordinal);
			return ValueUtilsSmi.GetDateTimeOffset(this._readerEventSink, this._currentColumnValuesV3, ordinal, this._currentMetaData[ordinal], this._currentConnection.IsKatmaiOrNewer);
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x00282A8C File Offset: 0x00281E8C
		public override object GetSqlValue(int ordinal)
		{
			this.EnsureCanGetCol("GetSqlValue", ordinal);
			SmiMetaData smiMetaData = this._currentMetaData[ordinal];
			if (this._currentConnection.IsKatmaiOrNewer)
			{
				return ValueUtilsSmi.GetSqlValue200(this._readerEventSink, (SmiTypedGetterSetter)this._currentColumnValuesV3, ordinal, smiMetaData, this._currentConnection.InternalContext);
			}
			return ValueUtilsSmi.GetSqlValue(this._readerEventSink, this._currentColumnValuesV3, ordinal, smiMetaData, this._currentConnection.InternalContext);
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x00282B00 File Offset: 0x00281F00
		public override int GetSqlValues(object[] values)
		{
			this.EnsureCanGetCol("GetSqlValues", 0);
			if (values == null)
			{
				throw ADP.ArgumentNull("values");
			}
			int num = ((values.Length < this._visibleColumnCount) ? values.Length : this._visibleColumnCount);
			for (int i = 0; i < num; i++)
			{
				values[this._indexMap[i]] = this.GetSqlValue(i);
			}
			return num;
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06002674 RID: 9844 RVA: 0x00282B5C File Offset: 0x00281F5C
		public override bool HasRows
		{
			get
			{
				return this._hasRows;
			}
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x00282B70 File Offset: 0x00281F70
		internal SqlDataReaderSmi(SmiEventStream eventStream, SqlCommand parent, CommandBehavior behavior, SqlInternalConnectionSmi connection, SmiEventSink parentSink)
			: base(parent, behavior)
		{
			this._eventStream = eventStream;
			this._currentConnection = connection;
			this._readerEventSink = new SqlDataReaderSmi.ReaderEventSink(this, parentSink);
			this._currentPosition = SqlDataReaderSmi.PositionState.BeforeResults;
			this._isOpen = true;
			this._indexMap = null;
			this._visibleColumnCount = 0;
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x00282BC0 File Offset: 0x00281FC0
		internal override SmiExtendedMetaData[] GetInternalSmiMetaData()
		{
			if (this._currentMetaData == null || this._visibleColumnCount == this.InternalFieldCount)
			{
				return this._currentMetaData;
			}
			SmiExtendedMetaData[] array = new SmiExtendedMetaData[this._visibleColumnCount];
			for (int i = 0; i < this._visibleColumnCount; i++)
			{
				array[i] = this._currentMetaData[this._indexMap[i]];
			}
			return array;
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x00282C1C File Offset: 0x0028201C
		internal override int GetLocaleId(int ordinal)
		{
			this.EnsureCanGetMetaData("GetLocaleId");
			return (int)this._currentMetaData[ordinal].LocaleId;
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06002678 RID: 9848 RVA: 0x00282C44 File Offset: 0x00282044
		private int InternalFieldCount
		{
			get
			{
				if (this.FNotInResults())
				{
					return 0;
				}
				return this._currentMetaData.Length;
			}
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x00282C64 File Offset: 0x00282064
		private bool IsReallyClosed()
		{
			return !this._isOpen;
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x00282C7C File Offset: 0x0028207C
		internal void ThrowIfClosed(string operationName)
		{
			if (this.IsClosed)
			{
				throw ADP.DataReaderClosed(operationName);
			}
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x00282C98 File Offset: 0x00282098
		private void EnsureCanGetCol(string operationName, int ordinal)
		{
			this.EnsureOnRow(operationName);
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x00282CAC File Offset: 0x002820AC
		internal void EnsureOnRow(string operationName)
		{
			this.ThrowIfClosed(operationName);
			if (this._currentPosition != SqlDataReaderSmi.PositionState.OnRow)
			{
				throw SQL.InvalidRead();
			}
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x00282CD0 File Offset: 0x002820D0
		internal void EnsureCanGetMetaData(string operationName)
		{
			this.ThrowIfClosed(operationName);
			if (this.FNotInResults())
			{
				throw SQL.InvalidRead();
			}
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x00282CF4 File Offset: 0x002820F4
		private bool FInResults()
		{
			return !this.FNotInResults();
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x00282D0C File Offset: 0x0028210C
		private bool FNotInResults()
		{
			return SqlDataReaderSmi.PositionState.AfterResults == this._currentPosition || SqlDataReaderSmi.PositionState.BeforeResults == this._currentPosition;
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x00282D30 File Offset: 0x00282130
		private void MetaDataAvailable(SmiQueryMetaData[] md, bool nextEventIsRow)
		{
			this._currentMetaData = md;
			this._hasRows = nextEventIsRow;
			this._fieldNameLookup = null;
			this._currentPosition = SqlDataReaderSmi.PositionState.BeforeRows;
			this._indexMap = new int[this._currentMetaData.Length];
			int num = 0;
			for (int i = 0; i < this._currentMetaData.Length; i++)
			{
				if (!this._currentMetaData[i].IsHidden.IsTrue)
				{
					this._indexMap[num] = i;
					num++;
				}
			}
			this._visibleColumnCount = num;
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x00282DAC File Offset: 0x002821AC
		private void RowAvailable(ITypedGetters row)
		{
			this._currentColumnValues = row;
			this._currentPosition = SqlDataReaderSmi.PositionState.OnRow;
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x00282DC8 File Offset: 0x002821C8
		private void RowAvailable(ITypedGettersV3 row)
		{
			this._currentColumnValuesV3 = row;
			this._currentPosition = SqlDataReaderSmi.PositionState.OnRow;
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x00282DE4 File Offset: 0x002821E4
		private void StatementCompleted()
		{
			this._currentMetaData = null;
			this._visibleColumnCount = 0;
			this._schemaTable = null;
			this._currentPosition = SqlDataReaderSmi.PositionState.AfterRows;
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x00282E10 File Offset: 0x00282210
		private void BatchCompleted()
		{
			this._currentPosition = SqlDataReaderSmi.PositionState.AfterResults;
			this._eventStream.Close(this._readerEventSink);
		}

		// Token: 0x0400182A RID: 6186
		private SqlDataReaderSmi.PositionState _currentPosition;

		// Token: 0x0400182B RID: 6187
		private bool _isOpen;

		// Token: 0x0400182C RID: 6188
		private SmiQueryMetaData[] _currentMetaData;

		// Token: 0x0400182D RID: 6189
		private int[] _indexMap;

		// Token: 0x0400182E RID: 6190
		private int _visibleColumnCount;

		// Token: 0x0400182F RID: 6191
		private DataTable _schemaTable;

		// Token: 0x04001830 RID: 6192
		private ITypedGetters _currentColumnValues;

		// Token: 0x04001831 RID: 6193
		private ITypedGettersV3 _currentColumnValuesV3;

		// Token: 0x04001832 RID: 6194
		private bool _hasRows;

		// Token: 0x04001833 RID: 6195
		private SmiEventStream _eventStream;

		// Token: 0x04001834 RID: 6196
		private SqlInternalConnectionSmi _currentConnection;

		// Token: 0x04001835 RID: 6197
		private SqlDataReaderSmi.ReaderEventSink _readerEventSink;

		// Token: 0x04001836 RID: 6198
		private FieldNameLookup _fieldNameLookup;

		// Token: 0x020002E1 RID: 737
		internal enum PositionState
		{
			// Token: 0x04001838 RID: 6200
			BeforeResults,
			// Token: 0x04001839 RID: 6201
			BeforeRows,
			// Token: 0x0400183A RID: 6202
			OnRow,
			// Token: 0x0400183B RID: 6203
			AfterRows,
			// Token: 0x0400183C RID: 6204
			AfterResults
		}

		// Token: 0x020002E2 RID: 738
		private sealed class ReaderEventSink : SmiEventSink_Default
		{
			// Token: 0x06002685 RID: 9861 RVA: 0x00282E38 File Offset: 0x00282238
			internal ReaderEventSink(SqlDataReaderSmi reader, SmiEventSink parent)
				: base(parent)
			{
				this.reader = reader;
			}

			// Token: 0x06002686 RID: 9862 RVA: 0x00282E54 File Offset: 0x00282254
			internal override void MetaDataAvailable(SmiQueryMetaData[] md, bool nextEventIsRow)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlDataReaderSmi.ReaderEventSink.MetaDataAvailable|ADV> %d#, md.Length=%d nextEventIsRow=%d.\n", this.reader.ObjectID, (md != null) ? md.Length : (-1), nextEventIsRow);
					if (md != null)
					{
						for (int i = 0; i < md.Length; i++)
						{
							Bid.Trace("<sc.SqlDataReaderSmi.ReaderEventSink.MetaDataAvailable|ADV> %d#, metaData[%d] is %s%s\n", this.reader.ObjectID, i, md[i].GetType().ToString(), md[i].TraceString());
						}
					}
				}
				this.reader.MetaDataAvailable(md, nextEventIsRow);
			}

			// Token: 0x06002687 RID: 9863 RVA: 0x00282ED0 File Offset: 0x002822D0
			internal override void RowAvailable(ITypedGetters row)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlDataReaderSmi.ReaderEventSink.RowAvailable|ADV> %d# (v2).\n", this.reader.ObjectID);
				}
				this.reader.RowAvailable(row);
			}

			// Token: 0x06002688 RID: 9864 RVA: 0x00282F08 File Offset: 0x00282308
			internal override void RowAvailable(ITypedGettersV3 row)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlDataReaderSmi.ReaderEventSink.RowAvailable|ADV> %d# (ITypedGettersV3).\n", this.reader.ObjectID);
				}
				this.reader.RowAvailable(row);
			}

			// Token: 0x06002689 RID: 9865 RVA: 0x00282F40 File Offset: 0x00282340
			internal override void RowAvailable(SmiTypedGetterSetter rowData)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlDataReaderSmi.ReaderEventSink.RowAvailable|ADV> %d# (SmiTypedGetterSetter).\n", this.reader.ObjectID);
				}
				this.reader.RowAvailable(rowData);
			}

			// Token: 0x0600268A RID: 9866 RVA: 0x00282F78 File Offset: 0x00282378
			internal override void StatementCompleted(int recordsAffected)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlDataReaderSmi.ReaderEventSink.StatementCompleted|ADV> %d# recordsAffected=%d.\n", this.reader.ObjectID, recordsAffected);
				}
				base.StatementCompleted(recordsAffected);
				this.reader.StatementCompleted();
			}

			// Token: 0x0600268B RID: 9867 RVA: 0x00282FB4 File Offset: 0x002823B4
			internal override void BatchCompleted()
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlDataReaderSmi.ReaderEventSink.BatchCompleted|ADV> %d#.\n", this.reader.ObjectID);
				}
				base.BatchCompleted();
				this.reader.BatchCompleted();
			}

			// Token: 0x0400183D RID: 6205
			private readonly SqlDataReaderSmi reader;
		}
	}
}
