﻿using System;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Data.Odbc
{
	// Token: 0x020001F5 RID: 501
	internal class OdbcMetaDataFactory : DbMetaDataFactory
	{
		// Token: 0x06001BC0 RID: 7104 RVA: 0x002493F8 File Offset: 0x002487F8
		internal OdbcMetaDataFactory(Stream XMLStream, string serverVersion, string serverVersionNormalized, OdbcConnection connection)
			: base(XMLStream, serverVersion, serverVersionNormalized)
		{
			this._schemaMapping = new OdbcMetaDataFactory.SchemaFunctionName[]
			{
				new OdbcMetaDataFactory.SchemaFunctionName(DbMetaDataCollectionNames.DataTypes, ODBC32.SQL_API.SQLGETTYPEINFO),
				new OdbcMetaDataFactory.SchemaFunctionName(OdbcMetaDataCollectionNames.Columns, ODBC32.SQL_API.SQLCOLUMNS),
				new OdbcMetaDataFactory.SchemaFunctionName(OdbcMetaDataCollectionNames.Indexes, ODBC32.SQL_API.SQLSTATISTICS),
				new OdbcMetaDataFactory.SchemaFunctionName(OdbcMetaDataCollectionNames.Procedures, ODBC32.SQL_API.SQLPROCEDURES),
				new OdbcMetaDataFactory.SchemaFunctionName(OdbcMetaDataCollectionNames.ProcedureColumns, ODBC32.SQL_API.SQLPROCEDURECOLUMNS),
				new OdbcMetaDataFactory.SchemaFunctionName(OdbcMetaDataCollectionNames.ProcedureParameters, ODBC32.SQL_API.SQLPROCEDURECOLUMNS),
				new OdbcMetaDataFactory.SchemaFunctionName(OdbcMetaDataCollectionNames.Tables, ODBC32.SQL_API.SQLTABLES),
				new OdbcMetaDataFactory.SchemaFunctionName(OdbcMetaDataCollectionNames.Views, ODBC32.SQL_API.SQLTABLES)
			};
			DataTable dataTable = base.CollectionDataSet.Tables[DbMetaDataCollectionNames.MetaDataCollections];
			if (dataTable == null)
			{
				throw ADP.UnableToBuildCollection(DbMetaDataCollectionNames.MetaDataCollections);
			}
			dataTable = base.CloneAndFilterCollection(DbMetaDataCollectionNames.MetaDataCollections, null);
			DataTable dataTable2 = base.CollectionDataSet.Tables[DbMetaDataCollectionNames.Restrictions];
			if (dataTable2 != null)
			{
				dataTable2 = base.CloneAndFilterCollection(DbMetaDataCollectionNames.Restrictions, null);
			}
			DataColumn dataColumn = dataTable.Columns["PopulationMechanism"];
			DataColumn dataColumn2 = dataTable.Columns["CollectionName"];
			DataColumn dataColumn3 = null;
			if (dataTable2 != null)
			{
				dataColumn3 = dataTable2.Columns["CollectionName"];
			}
			foreach (object obj in dataTable.Rows)
			{
				DataRow dataRow = (DataRow)obj;
				if ((string)dataRow[dataColumn] == "PrepareCollection")
				{
					int num = -1;
					for (int i = 0; i < this._schemaMapping.Length; i++)
					{
						if (this._schemaMapping[i]._schemaName == (string)dataRow[dataColumn2])
						{
							num = i;
							break;
						}
					}
					if (num != -1 && !connection.SQLGetFunctions(this._schemaMapping[num]._odbcFunction))
					{
						if (dataTable2 != null)
						{
							foreach (object obj2 in dataTable2.Rows)
							{
								DataRow dataRow2 = (DataRow)obj2;
								if ((string)dataRow[dataColumn2] == (string)dataRow2[dataColumn3])
								{
									dataRow2.Delete();
								}
							}
							dataTable2.AcceptChanges();
						}
						dataRow.Delete();
					}
				}
			}
			dataTable.AcceptChanges();
			base.CollectionDataSet.Tables.Remove(base.CollectionDataSet.Tables[DbMetaDataCollectionNames.MetaDataCollections]);
			base.CollectionDataSet.Tables.Add(dataTable);
			if (dataTable2 != null)
			{
				base.CollectionDataSet.Tables.Remove(base.CollectionDataSet.Tables[DbMetaDataCollectionNames.Restrictions]);
				base.CollectionDataSet.Tables.Add(dataTable2);
			}
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x00249760 File Offset: 0x00248B60
		private object BooleanFromODBC(object odbcSource)
		{
			if (odbcSource == DBNull.Value)
			{
				return DBNull.Value;
			}
			if (Convert.ToInt32(odbcSource, null) == 0)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x00249794 File Offset: 0x00248B94
		private OdbcCommand GetCommand(OdbcConnection connection)
		{
			OdbcCommand odbcCommand = connection.CreateCommand();
			odbcCommand.Transaction = connection.LocalTransaction;
			return odbcCommand;
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x002497B8 File Offset: 0x00248BB8
		private DataTable DataTableFromDataReader(IDataReader reader, string tableName)
		{
			object[] array;
			DataTable dataTable = this.NewDataTableFromReader(reader, out array, tableName);
			while (reader.Read())
			{
				reader.GetValues(array);
				dataTable.Rows.Add(array);
			}
			return dataTable;
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x002497F0 File Offset: 0x00248BF0
		private void DataTableFromDataReaderDataTypes(DataTable dataTypesTable, OdbcDataReader dataReader, OdbcConnection connection)
		{
			DataTable schemaTable = dataReader.GetSchemaTable();
			object[] array = new object[schemaTable.Rows.Count];
			DataColumn dataColumn = dataTypesTable.Columns[DbMetaDataColumnNames.TypeName];
			DataColumn dataColumn2 = dataTypesTable.Columns[DbMetaDataColumnNames.ProviderDbType];
			DataColumn dataColumn3 = dataTypesTable.Columns[DbMetaDataColumnNames.ColumnSize];
			DataColumn dataColumn4 = dataTypesTable.Columns[DbMetaDataColumnNames.CreateParameters];
			DataColumn dataColumn5 = dataTypesTable.Columns[DbMetaDataColumnNames.DataType];
			DataColumn dataColumn6 = dataTypesTable.Columns[DbMetaDataColumnNames.IsAutoIncrementable];
			DataColumn dataColumn7 = dataTypesTable.Columns[DbMetaDataColumnNames.IsCaseSensitive];
			DataColumn dataColumn8 = dataTypesTable.Columns[DbMetaDataColumnNames.IsFixedLength];
			DataColumn dataColumn9 = dataTypesTable.Columns[DbMetaDataColumnNames.IsFixedPrecisionScale];
			DataColumn dataColumn10 = dataTypesTable.Columns[DbMetaDataColumnNames.IsLong];
			DataColumn dataColumn11 = dataTypesTable.Columns[DbMetaDataColumnNames.IsNullable];
			DataColumn dataColumn12 = dataTypesTable.Columns[DbMetaDataColumnNames.IsSearchable];
			DataColumn dataColumn13 = dataTypesTable.Columns[DbMetaDataColumnNames.IsSearchableWithLike];
			DataColumn dataColumn14 = dataTypesTable.Columns[DbMetaDataColumnNames.IsUnsigned];
			DataColumn dataColumn15 = dataTypesTable.Columns[DbMetaDataColumnNames.MaximumScale];
			DataColumn dataColumn16 = dataTypesTable.Columns[DbMetaDataColumnNames.MinimumScale];
			DataColumn dataColumn17 = dataTypesTable.Columns[DbMetaDataColumnNames.LiteralPrefix];
			DataColumn dataColumn18 = dataTypesTable.Columns[DbMetaDataColumnNames.LiteralSuffix];
			DataColumn dataColumn19 = dataTypesTable.Columns[OdbcMetaDataColumnNames.SQLType];
			while (dataReader.Read())
			{
				dataReader.GetValues(array);
				DataRow dataRow = dataTypesTable.NewRow();
				dataRow[dataColumn] = array[0];
				dataRow[dataColumn19] = array[1];
				ODBC32.SQL_TYPE sql_TYPE = (ODBC32.SQL_TYPE)((int)Convert.ChangeType(array[1], typeof(int), null));
				if (!connection.IsV3Driver)
				{
					if (sql_TYPE == (ODBC32.SQL_TYPE)9)
					{
						sql_TYPE = ODBC32.SQL_TYPE.TYPE_DATE;
					}
					else if (sql_TYPE == (ODBC32.SQL_TYPE)10)
					{
						sql_TYPE = ODBC32.SQL_TYPE.TYPE_TIME;
					}
				}
				TypeMap typeMap;
				try
				{
					typeMap = TypeMap.FromSqlType(sql_TYPE);
				}
				catch (ArgumentException)
				{
					typeMap = null;
				}
				if (typeMap != null)
				{
					dataRow[dataColumn2] = typeMap._odbcType;
					dataRow[dataColumn5] = typeMap._type.FullName;
					ODBC32.SQL_TYPE sql_TYPE2 = sql_TYPE;
					switch (sql_TYPE2)
					{
					case ODBC32.SQL_TYPE.SS_TIME_EX:
					case ODBC32.SQL_TYPE.SS_UTCDATETIME:
					case ODBC32.SQL_TYPE.SS_VARIANT:
						goto IL_02EE;
					case ODBC32.SQL_TYPE.SS_XML:
						break;
					case ODBC32.SQL_TYPE.SS_UDT:
						goto IL_030A;
					default:
						switch (sql_TYPE2)
						{
						case ODBC32.SQL_TYPE.GUID:
						case ODBC32.SQL_TYPE.WCHAR:
						case ODBC32.SQL_TYPE.BIT:
						case ODBC32.SQL_TYPE.TINYINT:
						case ODBC32.SQL_TYPE.BIGINT:
						case ODBC32.SQL_TYPE.BINARY:
						case ODBC32.SQL_TYPE.CHAR:
						case ODBC32.SQL_TYPE.NUMERIC:
						case ODBC32.SQL_TYPE.DECIMAL:
						case ODBC32.SQL_TYPE.INTEGER:
						case ODBC32.SQL_TYPE.SMALLINT:
						case ODBC32.SQL_TYPE.FLOAT:
						case ODBC32.SQL_TYPE.REAL:
						case ODBC32.SQL_TYPE.DOUBLE:
						case ODBC32.SQL_TYPE.TIMESTAMP:
							goto IL_02EE;
						case ODBC32.SQL_TYPE.WLONGVARCHAR:
						case ODBC32.SQL_TYPE.LONGVARBINARY:
						case ODBC32.SQL_TYPE.LONGVARCHAR:
							break;
						case ODBC32.SQL_TYPE.WVARCHAR:
						case ODBC32.SQL_TYPE.VARBINARY:
						case ODBC32.SQL_TYPE.VARCHAR:
							dataRow[dataColumn10] = false;
							dataRow[dataColumn8] = false;
							goto IL_030A;
						case (ODBC32.SQL_TYPE)0:
						case (ODBC32.SQL_TYPE)9:
						case (ODBC32.SQL_TYPE)10:
							goto IL_030A;
						default:
							switch (sql_TYPE2)
							{
							case ODBC32.SQL_TYPE.TYPE_DATE:
							case ODBC32.SQL_TYPE.TYPE_TIME:
							case ODBC32.SQL_TYPE.TYPE_TIMESTAMP:
								goto IL_02EE;
							default:
								goto IL_030A;
							}
							break;
						}
						break;
					}
					dataRow[dataColumn10] = true;
					dataRow[dataColumn8] = false;
					goto IL_030A;
					IL_02EE:
					dataRow[dataColumn10] = false;
					dataRow[dataColumn8] = true;
				}
				IL_030A:
				dataRow[dataColumn3] = array[2];
				dataRow[dataColumn4] = array[5];
				if (array[11] == DBNull.Value || Convert.ToInt16(array[11], null) == 0)
				{
					dataRow[dataColumn6] = false;
				}
				else
				{
					dataRow[dataColumn6] = true;
				}
				dataRow[dataColumn7] = this.BooleanFromODBC(array[7]);
				dataRow[dataColumn9] = this.BooleanFromODBC(array[10]);
				if (array[6] != DBNull.Value)
				{
					switch ((ushort)Convert.ToInt16(array[6], null))
					{
					case 0:
						dataRow[dataColumn11] = false;
						break;
					case 1:
						dataRow[dataColumn11] = true;
						break;
					case 2:
						dataRow[dataColumn11] = DBNull.Value;
						break;
					}
				}
				if (DBNull.Value != array[8])
				{
					switch (Convert.ToInt16(array[8], null))
					{
					case 0:
						dataRow[dataColumn12] = false;
						dataRow[dataColumn13] = false;
						break;
					case 1:
						dataRow[dataColumn12] = false;
						dataRow[dataColumn13] = true;
						break;
					case 2:
						dataRow[dataColumn12] = true;
						dataRow[dataColumn13] = false;
						break;
					case 3:
						dataRow[dataColumn12] = true;
						dataRow[dataColumn13] = true;
						break;
					}
				}
				dataRow[dataColumn14] = this.BooleanFromODBC(array[9]);
				if (array[14] != DBNull.Value)
				{
					dataRow[dataColumn15] = array[14];
				}
				if (array[13] != DBNull.Value)
				{
					dataRow[dataColumn16] = array[13];
				}
				if (array[3] != DBNull.Value)
				{
					dataRow[dataColumn17] = array[3];
				}
				if (array[4] != DBNull.Value)
				{
					dataRow[dataColumn18] = array[4];
				}
				dataTypesTable.Rows.Add(dataRow);
			}
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x00249D18 File Offset: 0x00249118
		private DataTable DataTableFromDataReaderIndex(IDataReader reader, string tableName, string restrictionIndexName)
		{
			object[] array;
			DataTable dataTable = this.NewDataTableFromReader(reader, out array, tableName);
			int num = 6;
			int num2 = 5;
			while (reader.Read())
			{
				reader.GetValues(array);
				if (this.IncludeIndexRow(array[num2], restrictionIndexName, Convert.ToInt16(array[num], null)))
				{
					dataTable.Rows.Add(array);
				}
			}
			return dataTable;
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x00249D6C File Offset: 0x0024916C
		private DataTable DataTableFromDataReaderProcedureColumns(IDataReader reader, string tableName, bool isColumn)
		{
			object[] array;
			DataTable dataTable = this.NewDataTableFromReader(reader, out array, tableName);
			int num = 4;
			while (reader.Read())
			{
				reader.GetValues(array);
				if (array[num].GetType() == typeof(short) && (((short)array[num] == 3 && isColumn) || ((short)array[num] != 3 && !isColumn)))
				{
					dataTable.Rows.Add(array);
				}
			}
			return dataTable;
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x00249DD8 File Offset: 0x002491D8
		private DataTable DataTableFromDataReaderProcedures(IDataReader reader, string tableName, short procedureType)
		{
			object[] array;
			DataTable dataTable = this.NewDataTableFromReader(reader, out array, tableName);
			int num = 7;
			while (reader.Read())
			{
				reader.GetValues(array);
				if (array[num].GetType() == typeof(short) && (short)array[num] == procedureType)
				{
					dataTable.Rows.Add(array);
				}
			}
			return dataTable;
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x00249E34 File Offset: 0x00249234
		private void FillOutRestrictions(int restrictionsCount, string[] restrictions, object[] allRestrictions, string collectionName)
		{
			int i = 0;
			if (restrictions != null)
			{
				if (restrictions.Length > restrictionsCount)
				{
					throw ADP.TooManyRestrictions(collectionName);
				}
				for (i = 0; i < restrictions.Length; i++)
				{
					if (restrictions[i] != null)
					{
						allRestrictions[i] = restrictions[i];
					}
				}
			}
			while (i < restrictionsCount)
			{
				allRestrictions[i] = null;
				i++;
			}
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x00249E7C File Offset: 0x0024927C
		private DataTable GetColumnsCollection(string[] restrictions, OdbcConnection connection)
		{
			OdbcCommand odbcCommand = null;
			OdbcDataReader odbcDataReader = null;
			DataTable dataTable = null;
			try
			{
				odbcCommand = this.GetCommand(connection);
				string[] array = new string[4];
				this.FillOutRestrictions(4, restrictions, array, OdbcMetaDataCollectionNames.Columns);
				odbcDataReader = odbcCommand.ExecuteReaderFromSQLMethod(array, ODBC32.SQL_API.SQLCOLUMNS);
				dataTable = this.DataTableFromDataReader(odbcDataReader, OdbcMetaDataCollectionNames.Columns);
			}
			finally
			{
				if (odbcDataReader != null)
				{
					odbcDataReader.Dispose();
				}
				if (odbcCommand != null)
				{
					odbcCommand.Dispose();
				}
			}
			return dataTable;
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x00249EF8 File Offset: 0x002492F8
		private DataTable GetDataSourceInformationCollection(string[] restrictions, OdbcConnection connection)
		{
			if (!ADP.IsEmptyArray(restrictions))
			{
				throw ADP.TooManyRestrictions(DbMetaDataCollectionNames.DataSourceInformation);
			}
			if (base.CollectionDataSet.Tables[DbMetaDataCollectionNames.DataSourceInformation] == null)
			{
				throw ADP.UnableToBuildCollection(DbMetaDataCollectionNames.DataSourceInformation);
			}
			DataTable dataTable = base.CloneAndFilterCollection(DbMetaDataCollectionNames.DataSourceInformation, null);
			if (dataTable.Rows.Count != 1)
			{
				throw ADP.IncorrectNumberOfDataSourceInformationRows();
			}
			DataRow dataRow = dataTable.Rows[0];
			string text = connection.GetInfoStringUnhandled(ODBC32.SQL_INFO.CATALOG_NAME_SEPARATOR);
			if (!ADP.IsEmpty(text))
			{
				StringBuilder stringBuilder = new StringBuilder();
				ADP.EscapeSpecialCharacters(text, stringBuilder);
				dataRow[DbMetaDataColumnNames.CompositeIdentifierSeparatorPattern] = stringBuilder.ToString();
			}
			text = connection.GetInfoStringUnhandled(ODBC32.SQL_INFO.DBMS_NAME);
			if (text != null)
			{
				dataRow[DbMetaDataColumnNames.DataSourceProductName] = text;
			}
			dataRow[DbMetaDataColumnNames.DataSourceProductVersion] = base.ServerVersion;
			dataRow[DbMetaDataColumnNames.DataSourceProductVersionNormalized] = base.ServerVersionNormalized;
			dataRow[DbMetaDataColumnNames.ParameterMarkerFormat] = "?";
			dataRow[DbMetaDataColumnNames.ParameterMarkerPattern] = "\\?";
			dataRow[DbMetaDataColumnNames.ParameterNameMaxLength] = 0;
			int num;
			ODBC32.RetCode retCode;
			if (connection.IsV3Driver)
			{
				retCode = connection.GetInfoInt32Unhandled(ODBC32.SQL_INFO.SQL_OJ_CAPABILITIES_30, out num);
			}
			else
			{
				retCode = connection.GetInfoInt32Unhandled(ODBC32.SQL_INFO.SQL_OJ_CAPABILITIES_20, out num);
			}
			if (retCode == ODBC32.RetCode.SUCCESS || retCode == ODBC32.RetCode.SUCCESS_WITH_INFO)
			{
				SupportedJoinOperators supportedJoinOperators = SupportedJoinOperators.None;
				if ((num & 1) != 0)
				{
					supportedJoinOperators |= SupportedJoinOperators.LeftOuter;
				}
				if ((num & 2) != 0)
				{
					supportedJoinOperators |= SupportedJoinOperators.RightOuter;
				}
				if ((num & 4) != 0)
				{
					supportedJoinOperators |= SupportedJoinOperators.FullOuter;
				}
				if ((num & 32) != 0)
				{
					supportedJoinOperators |= SupportedJoinOperators.Inner;
				}
				dataRow[DbMetaDataColumnNames.SupportedJoinOperators] = supportedJoinOperators;
			}
			short num2;
			retCode = connection.GetInfoInt16Unhandled(ODBC32.SQL_INFO.GROUP_BY, out num2);
			GroupByBehavior groupByBehavior = GroupByBehavior.Unknown;
			if (retCode == ODBC32.RetCode.SUCCESS || retCode == ODBC32.RetCode.SUCCESS_WITH_INFO)
			{
				switch (num2)
				{
				case 0:
					groupByBehavior = GroupByBehavior.NotSupported;
					break;
				case 1:
					groupByBehavior = GroupByBehavior.ExactMatch;
					break;
				case 2:
					groupByBehavior = GroupByBehavior.MustContainAll;
					break;
				case 3:
					groupByBehavior = GroupByBehavior.Unrelated;
					break;
				}
			}
			dataRow[DbMetaDataColumnNames.GroupByBehavior] = groupByBehavior;
			retCode = connection.GetInfoInt16Unhandled(ODBC32.SQL_INFO.IDENTIFIER_CASE, out num2);
			IdentifierCase identifierCase = IdentifierCase.Unknown;
			if (retCode == ODBC32.RetCode.SUCCESS || retCode == ODBC32.RetCode.SUCCESS_WITH_INFO)
			{
				switch (num2)
				{
				case 1:
				case 2:
				case 4:
					identifierCase = IdentifierCase.Insensitive;
					break;
				case 3:
					identifierCase = IdentifierCase.Sensitive;
					break;
				}
			}
			dataRow[DbMetaDataColumnNames.IdentifierCase] = identifierCase;
			text = connection.GetInfoStringUnhandled(ODBC32.SQL_INFO.ORDER_BY_COLUMNS_IN_SELECT);
			if (text != null)
			{
				if (text == "Y")
				{
					dataRow[DbMetaDataColumnNames.OrderByColumnsInSelect] = true;
				}
				else if (text == "N")
				{
					dataRow[DbMetaDataColumnNames.OrderByColumnsInSelect] = false;
				}
			}
			text = connection.QuoteChar("GetSchema");
			if (text != null && text != " " && text.Length == 1)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				ADP.EscapeSpecialCharacters(text, stringBuilder2);
				string text2 = stringBuilder2.ToString();
				stringBuilder2.Length = 0;
				ADP.EscapeSpecialCharacters(text, stringBuilder2);
				stringBuilder2.Append("(([^");
				stringBuilder2.Append(text2);
				stringBuilder2.Append("]|");
				stringBuilder2.Append(text2);
				stringBuilder2.Append(text2);
				stringBuilder2.Append(")*)");
				stringBuilder2.Append(text2);
				dataRow[DbMetaDataColumnNames.QuotedIdentifierPattern] = stringBuilder2.ToString();
			}
			retCode = connection.GetInfoInt16Unhandled(ODBC32.SQL_INFO.QUOTED_IDENTIFIER_CASE, out num2);
			IdentifierCase identifierCase2 = IdentifierCase.Unknown;
			if (retCode == ODBC32.RetCode.SUCCESS || retCode == ODBC32.RetCode.SUCCESS_WITH_INFO)
			{
				switch (num2)
				{
				case 1:
				case 2:
				case 4:
					identifierCase2 = IdentifierCase.Insensitive;
					break;
				case 3:
					identifierCase2 = IdentifierCase.Sensitive;
					break;
				}
			}
			dataRow[DbMetaDataColumnNames.QuotedIdentifierCase] = identifierCase2;
			dataTable.AcceptChanges();
			return dataTable;
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x0024A26C File Offset: 0x0024966C
		private DataTable GetDataTypesCollection(string[] restrictions, OdbcConnection connection)
		{
			if (!ADP.IsEmptyArray(restrictions))
			{
				throw ADP.TooManyRestrictions(DbMetaDataCollectionNames.DataTypes);
			}
			DataTable dataTable = base.CollectionDataSet.Tables[DbMetaDataCollectionNames.DataTypes];
			if (dataTable == null)
			{
				throw ADP.UnableToBuildCollection(DbMetaDataCollectionNames.DataTypes);
			}
			dataTable = base.CloneAndFilterCollection(DbMetaDataCollectionNames.DataTypes, null);
			OdbcCommand odbcCommand = null;
			OdbcDataReader odbcDataReader = null;
			object[] array = new object[] { 0 };
			try
			{
				odbcCommand = this.GetCommand(connection);
				odbcDataReader = odbcCommand.ExecuteReaderFromSQLMethod(array, ODBC32.SQL_API.SQLGETTYPEINFO);
				this.DataTableFromDataReaderDataTypes(dataTable, odbcDataReader, connection);
			}
			finally
			{
				if (odbcDataReader != null)
				{
					odbcDataReader.Dispose();
				}
				if (odbcCommand != null)
				{
					odbcCommand.Dispose();
				}
			}
			dataTable.AcceptChanges();
			return dataTable;
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x0024A324 File Offset: 0x00249724
		private DataTable GetIndexCollection(string[] restrictions, OdbcConnection connection)
		{
			OdbcCommand odbcCommand = null;
			OdbcDataReader odbcDataReader = null;
			DataTable dataTable = null;
			try
			{
				odbcCommand = this.GetCommand(connection);
				object[] array = new object[5];
				this.FillOutRestrictions(4, restrictions, array, OdbcMetaDataCollectionNames.Indexes);
				if (array[2] == null)
				{
					throw ODBC.GetSchemaRestrictionRequired();
				}
				array[3] = 1;
				array[4] = 1;
				odbcDataReader = odbcCommand.ExecuteReaderFromSQLMethod(array, ODBC32.SQL_API.SQLSTATISTICS);
				string text = null;
				if (restrictions != null && restrictions.Length >= 4)
				{
					text = restrictions[3];
				}
				dataTable = this.DataTableFromDataReaderIndex(odbcDataReader, OdbcMetaDataCollectionNames.Indexes, text);
			}
			finally
			{
				if (odbcDataReader != null)
				{
					odbcDataReader.Dispose();
				}
				if (odbcCommand != null)
				{
					odbcCommand.Dispose();
				}
			}
			return dataTable;
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x0024A3D0 File Offset: 0x002497D0
		private DataTable GetProcedureColumnsCollection(string[] restrictions, OdbcConnection connection, bool isColumns)
		{
			OdbcCommand odbcCommand = null;
			OdbcDataReader odbcDataReader = null;
			DataTable dataTable = null;
			try
			{
				odbcCommand = this.GetCommand(connection);
				string[] array = new string[4];
				this.FillOutRestrictions(4, restrictions, array, OdbcMetaDataCollectionNames.Columns);
				odbcDataReader = odbcCommand.ExecuteReaderFromSQLMethod(array, ODBC32.SQL_API.SQLPROCEDURECOLUMNS);
				string text;
				if (isColumns)
				{
					text = OdbcMetaDataCollectionNames.ProcedureColumns;
				}
				else
				{
					text = OdbcMetaDataCollectionNames.ProcedureParameters;
				}
				dataTable = this.DataTableFromDataReaderProcedureColumns(odbcDataReader, text, isColumns);
			}
			finally
			{
				if (odbcDataReader != null)
				{
					odbcDataReader.Dispose();
				}
				if (odbcCommand != null)
				{
					odbcCommand.Dispose();
				}
			}
			return dataTable;
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x0024A45C File Offset: 0x0024985C
		private DataTable GetProceduresCollection(string[] restrictions, OdbcConnection connection)
		{
			OdbcCommand odbcCommand = null;
			OdbcDataReader odbcDataReader = null;
			DataTable dataTable = null;
			try
			{
				odbcCommand = this.GetCommand(connection);
				string[] array = new string[4];
				this.FillOutRestrictions(4, restrictions, array, OdbcMetaDataCollectionNames.Procedures);
				odbcDataReader = odbcCommand.ExecuteReaderFromSQLMethod(array, ODBC32.SQL_API.SQLPROCEDURES);
				if (array[3] == null)
				{
					dataTable = this.DataTableFromDataReader(odbcDataReader, OdbcMetaDataCollectionNames.Procedures);
				}
				else
				{
					short num;
					if (restrictions[3] == "SQL_PT_UNKNOWN" || restrictions[3] == "0")
					{
						num = 0;
					}
					else if (restrictions[3] == "SQL_PT_PROCEDURE" || restrictions[3] == "1")
					{
						num = 1;
					}
					else
					{
						if (!(restrictions[3] == "SQL_PT_FUNCTION") && !(restrictions[3] == "2"))
						{
							throw ADP.InvalidRestrictionValue(OdbcMetaDataCollectionNames.Procedures, "PROCEDURE_TYPE", restrictions[3]);
						}
						num = 2;
					}
					dataTable = this.DataTableFromDataReaderProcedures(odbcDataReader, OdbcMetaDataCollectionNames.Procedures, num);
				}
			}
			finally
			{
				if (odbcDataReader != null)
				{
					odbcDataReader.Dispose();
				}
				if (odbcCommand != null)
				{
					odbcCommand.Dispose();
				}
			}
			return dataTable;
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x0024A56C File Offset: 0x0024996C
		private DataTable GetReservedWordsCollection(string[] restrictions, OdbcConnection connection)
		{
			if (!ADP.IsEmptyArray(restrictions))
			{
				throw ADP.TooManyRestrictions(DbMetaDataCollectionNames.ReservedWords);
			}
			if (base.CollectionDataSet.Tables[DbMetaDataCollectionNames.ReservedWords] == null)
			{
				throw ADP.UnableToBuildCollection(DbMetaDataCollectionNames.ReservedWords);
			}
			DataTable dataTable = base.CloneAndFilterCollection(DbMetaDataCollectionNames.ReservedWords, null);
			DataColumn dataColumn = dataTable.Columns[DbMetaDataColumnNames.ReservedWord];
			if (dataColumn == null)
			{
				throw ADP.UnableToBuildCollection(DbMetaDataCollectionNames.ReservedWords);
			}
			string infoStringUnhandled = connection.GetInfoStringUnhandled(ODBC32.SQL_INFO.KEYWORDS);
			if (infoStringUnhandled != null)
			{
				string[] array = infoStringUnhandled.Split(OdbcMetaDataFactory.KeywordSeparatorChar);
				for (int i = 0; i < array.Length; i++)
				{
					DataRow dataRow = dataTable.NewRow();
					dataRow[dataColumn] = array[i];
					dataTable.Rows.Add(dataRow);
					dataRow.AcceptChanges();
				}
			}
			return dataTable;
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x0024A62C File Offset: 0x00249A2C
		private DataTable GetTablesCollection(string[] restrictions, OdbcConnection connection, bool isTables)
		{
			OdbcCommand odbcCommand = null;
			OdbcDataReader odbcDataReader = null;
			DataTable dataTable = null;
			try
			{
				odbcCommand = this.GetCommand(connection);
				string[] array = new string[4];
				string text;
				string text2;
				if (isTables)
				{
					text = "TABLE,SYSTEM TABLE";
					text2 = OdbcMetaDataCollectionNames.Tables;
				}
				else
				{
					text = "VIEW";
					text2 = OdbcMetaDataCollectionNames.Views;
				}
				this.FillOutRestrictions(3, restrictions, array, text2);
				array[3] = text;
				odbcDataReader = odbcCommand.ExecuteReaderFromSQLMethod(array, ODBC32.SQL_API.SQLTABLES);
				dataTable = this.DataTableFromDataReader(odbcDataReader, text2);
			}
			finally
			{
				if (odbcDataReader != null)
				{
					odbcDataReader.Dispose();
				}
				if (odbcCommand != null)
				{
					odbcCommand.Dispose();
				}
			}
			return dataTable;
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x0024A6C4 File Offset: 0x00249AC4
		private bool IncludeIndexRow(object rowIndexName, string restrictionIndexName, short rowIndexType)
		{
			return rowIndexType != 0 && (restrictionIndexName == null || !(restrictionIndexName != (string)rowIndexName));
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x0024A6EC File Offset: 0x00249AEC
		private DataTable NewDataTableFromReader(IDataReader reader, out object[] values, string tableName)
		{
			DataTable dataTable = new DataTable(tableName);
			dataTable.Locale = CultureInfo.InvariantCulture;
			DataTable schemaTable = reader.GetSchemaTable();
			foreach (object obj in schemaTable.Rows)
			{
				DataRow dataRow = (DataRow)obj;
				dataTable.Columns.Add(dataRow["ColumnName"] as string, (Type)dataRow["DataType"]);
			}
			values = new object[dataTable.Columns.Count];
			return dataTable;
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x0024A7A4 File Offset: 0x00249BA4
		protected override DataTable PrepareCollection(string collectionName, string[] restrictions, DbConnection connection)
		{
			DataTable dataTable = null;
			OdbcConnection odbcConnection = (OdbcConnection)connection;
			if (collectionName == OdbcMetaDataCollectionNames.Tables)
			{
				dataTable = this.GetTablesCollection(restrictions, odbcConnection, true);
			}
			else if (collectionName == OdbcMetaDataCollectionNames.Views)
			{
				dataTable = this.GetTablesCollection(restrictions, odbcConnection, false);
			}
			else if (collectionName == OdbcMetaDataCollectionNames.Columns)
			{
				dataTable = this.GetColumnsCollection(restrictions, odbcConnection);
			}
			else if (collectionName == OdbcMetaDataCollectionNames.Procedures)
			{
				dataTable = this.GetProceduresCollection(restrictions, odbcConnection);
			}
			else if (collectionName == OdbcMetaDataCollectionNames.ProcedureColumns)
			{
				dataTable = this.GetProcedureColumnsCollection(restrictions, odbcConnection, true);
			}
			else if (collectionName == OdbcMetaDataCollectionNames.ProcedureParameters)
			{
				dataTable = this.GetProcedureColumnsCollection(restrictions, odbcConnection, false);
			}
			else if (collectionName == OdbcMetaDataCollectionNames.Indexes)
			{
				dataTable = this.GetIndexCollection(restrictions, odbcConnection);
			}
			else if (collectionName == DbMetaDataCollectionNames.DataTypes)
			{
				dataTable = this.GetDataTypesCollection(restrictions, odbcConnection);
			}
			else if (collectionName == DbMetaDataCollectionNames.DataSourceInformation)
			{
				dataTable = this.GetDataSourceInformationCollection(restrictions, odbcConnection);
			}
			else if (collectionName == DbMetaDataCollectionNames.ReservedWords)
			{
				dataTable = this.GetReservedWordsCollection(restrictions, odbcConnection);
			}
			if (dataTable == null)
			{
				throw ADP.UnableToBuildCollection(collectionName);
			}
			return dataTable;
		}

		// Token: 0x0400102C RID: 4140
		private const string _collectionName = "CollectionName";

		// Token: 0x0400102D RID: 4141
		private const string _populationMechanism = "PopulationMechanism";

		// Token: 0x0400102E RID: 4142
		private const string _prepareCollection = "PrepareCollection";

		// Token: 0x0400102F RID: 4143
		private readonly OdbcMetaDataFactory.SchemaFunctionName[] _schemaMapping;

		// Token: 0x04001030 RID: 4144
		internal static readonly char[] KeywordSeparatorChar = new char[] { ',' };

		// Token: 0x020001F6 RID: 502
		private struct SchemaFunctionName
		{
			// Token: 0x06001BD5 RID: 7125 RVA: 0x0024A8E8 File Offset: 0x00249CE8
			internal SchemaFunctionName(string schemaName, ODBC32.SQL_API odbcFunction)
			{
				this._schemaName = schemaName;
				this._odbcFunction = odbcFunction;
			}

			// Token: 0x04001031 RID: 4145
			internal readonly string _schemaName;

			// Token: 0x04001032 RID: 4146
			internal readonly ODBC32.SQL_API _odbcFunction;
		}
	}
}
