using System;
using System.Data.Common;
using System.Globalization;
using System.IO;

namespace System.Data.ProviderBase
{
	// Token: 0x0200009F RID: 159
	internal class DbMetaDataFactory
	{
		// Token: 0x06000863 RID: 2147 RVA: 0x000752A8 File Offset: 0x000746A8
		public DbMetaDataFactory(Stream xmlStream, string serverVersion, string normalizedServerVersion)
		{
			ADP.CheckArgumentNull(xmlStream, "xmlStream");
			ADP.CheckArgumentNull(serverVersion, "serverVersion");
			ADP.CheckArgumentNull(normalizedServerVersion, "normalizedServerVersion");
			this.LoadDataSetFromXml(xmlStream);
			this._serverVersionString = serverVersion;
			this._normalizedServerVersion = normalizedServerVersion;
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x000752F4 File Offset: 0x000746F4
		protected DataTable CloneAndFilterCollection(string collectionName, string[] hiddenColumnNames)
		{
			DataTable dataTable = this._metaDataCollectionsDataSet.Tables[collectionName];
			if (dataTable == null || collectionName != dataTable.TableName)
			{
				throw ADP.DataTableDoesNotExist(collectionName);
			}
			DataTable dataTable2 = new DataTable(collectionName);
			dataTable2.Locale = CultureInfo.InvariantCulture;
			DataColumnCollection columns = dataTable2.Columns;
			DataColumn[] array = this.FilterColumns(dataTable, hiddenColumnNames, columns);
			foreach (object obj in dataTable.Rows)
			{
				DataRow dataRow = (DataRow)obj;
				if (this.SupportedByCurrentVersion(dataRow))
				{
					DataRow dataRow2 = dataTable2.NewRow();
					for (int i = 0; i < columns.Count; i++)
					{
						dataRow2[columns[i]] = dataRow[array[i], DataRowVersion.Current];
					}
					dataTable2.Rows.Add(dataRow2);
					dataRow2.AcceptChanges();
				}
			}
			return dataTable2;
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x00075400 File Offset: 0x00074800
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00075414 File Offset: 0x00074814
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._normalizedServerVersion = null;
				this._serverVersionString = null;
				this._metaDataCollectionsDataSet.Dispose();
			}
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00075440 File Offset: 0x00074840
		private DataTable ExecuteCommand(DataRow requestedCollectionRow, string[] restrictions, DbConnection connection)
		{
			DataTable dataTable = this._metaDataCollectionsDataSet.Tables[DbMetaDataCollectionNames.MetaDataCollections];
			DataColumn dataColumn = dataTable.Columns["PopulationString"];
			DataColumn dataColumn2 = dataTable.Columns["NumberOfRestrictions"];
			DataColumn dataColumn3 = dataTable.Columns["CollectionName"];
			DataTable dataTable2 = null;
			string text = requestedCollectionRow[dataColumn, DataRowVersion.Current] as string;
			int num = (int)requestedCollectionRow[dataColumn2, DataRowVersion.Current];
			string text2 = requestedCollectionRow[dataColumn3, DataRowVersion.Current] as string;
			if (restrictions != null && restrictions.Length > num)
			{
				throw ADP.TooManyRestrictions(text2);
			}
			DbCommand dbCommand = connection.CreateCommand();
			dbCommand.CommandText = text;
			dbCommand.CommandTimeout = Math.Max(dbCommand.CommandTimeout, 180);
			for (int i = 0; i < num; i++)
			{
				DbParameter dbParameter = dbCommand.CreateParameter();
				if (restrictions != null && restrictions.Length > i && restrictions[i] != null)
				{
					dbParameter.Value = restrictions[i];
				}
				else
				{
					dbParameter.Value = DBNull.Value;
				}
				dbParameter.ParameterName = this.GetParameterName(text2, i + 1);
				dbParameter.Direction = ParameterDirection.Input;
				dbCommand.Parameters.Add(dbParameter);
			}
			DbDataReader dbDataReader = null;
			try
			{
				try
				{
					dbDataReader = dbCommand.ExecuteReader();
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableExceptionType(ex))
					{
						throw;
					}
					throw ADP.QueryFailed(text2, ex);
				}
				dataTable2 = new DataTable(text2);
				dataTable2.Locale = CultureInfo.InvariantCulture;
				DataTable schemaTable = dbDataReader.GetSchemaTable();
				foreach (object obj in schemaTable.Rows)
				{
					DataRow dataRow = (DataRow)obj;
					dataTable2.Columns.Add(dataRow["ColumnName"] as string, (Type)dataRow["DataType"]);
				}
				object[] array = new object[dataTable2.Columns.Count];
				while (dbDataReader.Read())
				{
					dbDataReader.GetValues(array);
					dataTable2.Rows.Add(array);
				}
			}
			finally
			{
				if (dbDataReader != null)
				{
					dbDataReader.Dispose();
					dbDataReader = null;
				}
			}
			return dataTable2;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x000756B4 File Offset: 0x00074AB4
		private DataColumn[] FilterColumns(DataTable sourceTable, string[] hiddenColumnNames, DataColumnCollection destinationColumns)
		{
			DataColumn[] array = null;
			int num = 0;
			foreach (object obj in sourceTable.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				if (this.IncludeThisColumn(dataColumn, hiddenColumnNames))
				{
					num++;
				}
			}
			if (num == 0)
			{
				throw ADP.NoColumns();
			}
			int num2 = 0;
			array = new DataColumn[num];
			foreach (object obj2 in sourceTable.Columns)
			{
				DataColumn dataColumn2 = (DataColumn)obj2;
				if (this.IncludeThisColumn(dataColumn2, hiddenColumnNames))
				{
					DataColumn dataColumn3 = new DataColumn(dataColumn2.ColumnName, dataColumn2.DataType);
					destinationColumns.Add(dataColumn3);
					array[num2] = dataColumn2;
					num2++;
				}
			}
			return array;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x000757C4 File Offset: 0x00074BC4
		internal DataRow FindMetaDataCollectionRow(string collectionName)
		{
			DataTable dataTable = this._metaDataCollectionsDataSet.Tables[DbMetaDataCollectionNames.MetaDataCollections];
			if (dataTable == null)
			{
				throw ADP.InvalidXml();
			}
			DataColumn dataColumn = dataTable.Columns[DbMetaDataColumnNames.CollectionName];
			if (dataColumn == null || typeof(string) != dataColumn.DataType)
			{
				throw ADP.InvalidXmlMissingColumn(DbMetaDataCollectionNames.MetaDataCollections, DbMetaDataColumnNames.CollectionName);
			}
			DataRow dataRow = null;
			string text = null;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			foreach (object obj in dataTable.Rows)
			{
				DataRow dataRow2 = (DataRow)obj;
				string text2 = dataRow2[dataColumn, DataRowVersion.Current] as string;
				if (ADP.IsEmpty(text2))
				{
					throw ADP.InvalidXmlInvalidValue(DbMetaDataCollectionNames.MetaDataCollections, DbMetaDataColumnNames.CollectionName);
				}
				if (ADP.CompareInsensitiveInvariant(text2, collectionName))
				{
					if (!this.SupportedByCurrentVersion(dataRow2))
					{
						flag = true;
					}
					else if (collectionName == text2)
					{
						if (flag2)
						{
							throw ADP.CollectionNameIsNotUnique(collectionName);
						}
						dataRow = dataRow2;
						text = text2;
						flag2 = true;
					}
					else
					{
						if (text != null)
						{
							flag3 = true;
						}
						dataRow = dataRow2;
						text = text2;
					}
				}
			}
			if (dataRow == null)
			{
				if (!flag)
				{
					throw ADP.UndefinedCollection(collectionName);
				}
				throw ADP.UnsupportedVersion(collectionName);
			}
			else
			{
				if (!flag2 && flag3)
				{
					throw ADP.AmbigousCollectionName(collectionName);
				}
				return dataRow;
			}
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00075924 File Offset: 0x00074D24
		private void FixUpVersion(DataTable dataSourceInfoTable)
		{
			DataColumn dataColumn = dataSourceInfoTable.Columns["DataSourceProductVersion"];
			DataColumn dataColumn2 = dataSourceInfoTable.Columns["DataSourceProductVersionNormalized"];
			if (dataColumn == null || dataColumn2 == null)
			{
				throw ADP.MissingDataSourceInformationColumn();
			}
			if (dataSourceInfoTable.Rows.Count != 1)
			{
				throw ADP.IncorrectNumberOfDataSourceInformationRows();
			}
			DataRow dataRow = dataSourceInfoTable.Rows[0];
			dataRow[dataColumn] = this._serverVersionString;
			dataRow[dataColumn2] = this._normalizedServerVersion;
			dataRow.AcceptChanges();
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x000759A0 File Offset: 0x00074DA0
		private string GetParameterName(string neededCollectionName, int neededRestrictionNumber)
		{
			DataColumn dataColumn = null;
			DataColumn dataColumn2 = null;
			DataColumn dataColumn3 = null;
			DataColumn dataColumn4 = null;
			string text = null;
			DataTable dataTable = this._metaDataCollectionsDataSet.Tables[DbMetaDataCollectionNames.Restrictions];
			if (dataTable != null)
			{
				DataColumnCollection columns = dataTable.Columns;
				if (columns != null)
				{
					dataColumn = columns["CollectionName"];
					dataColumn2 = columns["ParameterName"];
					dataColumn3 = columns["RestrictionName"];
					dataColumn4 = columns["RestrictionNumber"];
				}
			}
			if (dataColumn2 == null || dataColumn == null || dataColumn3 == null || dataColumn4 == null)
			{
				throw ADP.MissingRestrictionColumn();
			}
			foreach (object obj in dataTable.Rows)
			{
				DataRow dataRow = (DataRow)obj;
				if ((string)dataRow[dataColumn] == neededCollectionName && (int)dataRow[dataColumn4] == neededRestrictionNumber && this.SupportedByCurrentVersion(dataRow))
				{
					text = (string)dataRow[dataColumn2];
					break;
				}
			}
			if (text == null)
			{
				throw ADP.MissingRestrictionRow();
			}
			return text;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00075ACC File Offset: 0x00074ECC
		public virtual DataTable GetSchema(DbConnection connection, string collectionName, string[] restrictions)
		{
			DataTable dataTable = this._metaDataCollectionsDataSet.Tables[DbMetaDataCollectionNames.MetaDataCollections];
			DataColumn dataColumn = dataTable.Columns["PopulationMechanism"];
			DataColumn dataColumn2 = dataTable.Columns[DbMetaDataColumnNames.CollectionName];
			DataRow dataRow = this.FindMetaDataCollectionRow(collectionName);
			string text = dataRow[dataColumn2, DataRowVersion.Current] as string;
			if (!ADP.IsEmptyArray(restrictions))
			{
				for (int i = 0; i < restrictions.Length; i++)
				{
					if (restrictions[i] != null && restrictions[i].Length > 4096)
					{
						throw ADP.NotSupported();
					}
				}
			}
			string text2 = dataRow[dataColumn, DataRowVersion.Current] as string;
			string text3;
			if ((text3 = text2) != null)
			{
				DataTable dataTable2;
				if (!(text3 == "DataTable"))
				{
					if (!(text3 == "SQLCommand"))
					{
						if (!(text3 == "PrepareCollection"))
						{
							goto IL_014B;
						}
						dataTable2 = this.PrepareCollection(text, restrictions, connection);
					}
					else
					{
						dataTable2 = this.ExecuteCommand(dataRow, restrictions, connection);
					}
				}
				else
				{
					string[] array;
					if (text == DbMetaDataCollectionNames.MetaDataCollections)
					{
						array = new string[] { "PopulationMechanism", "PopulationString" };
					}
					else
					{
						array = null;
					}
					if (!ADP.IsEmptyArray(restrictions))
					{
						throw ADP.TooManyRestrictions(text);
					}
					dataTable2 = this.CloneAndFilterCollection(text, array);
					if (text == DbMetaDataCollectionNames.DataSourceInformation)
					{
						this.FixUpVersion(dataTable2);
					}
				}
				return dataTable2;
			}
			IL_014B:
			throw ADP.UndefinedPopulationMechanism(text2);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00075C30 File Offset: 0x00075030
		private bool IncludeThisColumn(DataColumn sourceColumn, string[] hiddenColumnNames)
		{
			bool flag = true;
			string columnName = sourceColumn.ColumnName;
			string text;
			if ((text = columnName) != null && (text == "MinimumVersion" || text == "MaximumVersion"))
			{
				flag = false;
			}
			else if (hiddenColumnNames != null)
			{
				for (int i = 0; i < hiddenColumnNames.Length; i++)
				{
					if (hiddenColumnNames[i] == columnName)
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00075C8C File Offset: 0x0007508C
		private void LoadDataSetFromXml(Stream XmlStream)
		{
			this._metaDataCollectionsDataSet = new DataSet();
			this._metaDataCollectionsDataSet.Locale = CultureInfo.InvariantCulture;
			this._metaDataCollectionsDataSet.ReadXml(XmlStream);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00075CC4 File Offset: 0x000750C4
		protected virtual DataTable PrepareCollection(string collectionName, string[] restrictions, DbConnection connection)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00075CD8 File Offset: 0x000750D8
		private bool SupportedByCurrentVersion(DataRow requestedCollectionRow)
		{
			bool flag = true;
			DataColumnCollection columns = requestedCollectionRow.Table.Columns;
			DataColumn dataColumn = columns["MinimumVersion"];
			if (dataColumn != null)
			{
				object obj = requestedCollectionRow[dataColumn];
				if (obj != null && obj != DBNull.Value && 0 > string.Compare(this._normalizedServerVersion, (string)obj, StringComparison.OrdinalIgnoreCase))
				{
					flag = false;
				}
			}
			if (flag)
			{
				dataColumn = columns["MaximumVersion"];
				if (dataColumn != null)
				{
					object obj = requestedCollectionRow[dataColumn];
					if (obj != null && obj != DBNull.Value && 0 < string.Compare(this._normalizedServerVersion, (string)obj, StringComparison.OrdinalIgnoreCase))
					{
						flag = false;
					}
				}
			}
			return flag;
		}

		// Token: 0x04000568 RID: 1384
		private const string _collectionName = "CollectionName";

		// Token: 0x04000569 RID: 1385
		private const string _populationMechanism = "PopulationMechanism";

		// Token: 0x0400056A RID: 1386
		private const string _populationString = "PopulationString";

		// Token: 0x0400056B RID: 1387
		private const string _maximumVersion = "MaximumVersion";

		// Token: 0x0400056C RID: 1388
		private const string _minimumVersion = "MinimumVersion";

		// Token: 0x0400056D RID: 1389
		private const string _dataSourceProductVersionNormalized = "DataSourceProductVersionNormalized";

		// Token: 0x0400056E RID: 1390
		private const string _dataSourceProductVersion = "DataSourceProductVersion";

		// Token: 0x0400056F RID: 1391
		private const string _restrictionDefault = "RestrictionDefault";

		// Token: 0x04000570 RID: 1392
		private const string _restrictionNumber = "RestrictionNumber";

		// Token: 0x04000571 RID: 1393
		private const string _numberOfRestrictions = "NumberOfRestrictions";

		// Token: 0x04000572 RID: 1394
		private const string _restrictionName = "RestrictionName";

		// Token: 0x04000573 RID: 1395
		private const string _parameterName = "ParameterName";

		// Token: 0x04000574 RID: 1396
		private const string _dataTable = "DataTable";

		// Token: 0x04000575 RID: 1397
		private const string _sqlCommand = "SQLCommand";

		// Token: 0x04000576 RID: 1398
		private const string _prepareCollection = "PrepareCollection";

		// Token: 0x04000577 RID: 1399
		private DataSet _metaDataCollectionsDataSet;

		// Token: 0x04000578 RID: 1400
		private string _normalizedServerVersion;

		// Token: 0x04000579 RID: 1401
		private string _serverVersionString;
	}
}
