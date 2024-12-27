﻿using System;
using System.Data.Common;
using System.Data.ProviderBase;
using System.IO;
using System.Text;

namespace System.Data.SqlClient
{
	// Token: 0x02000300 RID: 768
	internal sealed class SqlMetaDataFactory : DbMetaDataFactory
	{
		// Token: 0x06002814 RID: 10260 RVA: 0x0028D3A8 File Offset: 0x0028C7A8
		public SqlMetaDataFactory(Stream XMLStream, string serverVersion, string serverVersionNormalized)
			: base(XMLStream, serverVersion, serverVersionNormalized)
		{
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x0028D3C0 File Offset: 0x0028C7C0
		private void addUDTsToDataTypesTable(DataTable dataTypesTable, SqlConnection connection, string ServerVersion)
		{
			if (0 > string.Compare(ServerVersion, "09.00.0000", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			SqlCommand sqlCommand = connection.CreateCommand();
			sqlCommand.CommandText = "select assemblies.name, types.assembly_class, ASSEMBLYPROPERTY(assemblies.name, 'VersionMajor') as version_major, ASSEMBLYPROPERTY(assemblies.name, 'VersionMinor') as version_minor, ASSEMBLYPROPERTY(assemblies.name, 'VersionBuild') as version_build, ASSEMBLYPROPERTY(assemblies.name, 'VersionRevision') as version_revision, ASSEMBLYPROPERTY(assemblies.name, 'CultureInfo') as culture_info, ASSEMBLYPROPERTY(assemblies.name, 'PublicKey') as public_key, is_nullable, is_fixed_length, max_length from sys.assemblies as assemblies  join sys.assembly_types as types on assemblies.assembly_id = types.assembly_id ";
			DataColumn dataColumn = dataTypesTable.Columns[DbMetaDataColumnNames.ProviderDbType];
			DataColumn dataColumn2 = dataTypesTable.Columns[DbMetaDataColumnNames.ColumnSize];
			DataColumn dataColumn3 = dataTypesTable.Columns[DbMetaDataColumnNames.IsFixedLength];
			DataColumn dataColumn4 = dataTypesTable.Columns[DbMetaDataColumnNames.IsSearchable];
			DataColumn dataColumn5 = dataTypesTable.Columns[DbMetaDataColumnNames.IsLiteralSupported];
			DataColumn dataColumn6 = dataTypesTable.Columns[DbMetaDataColumnNames.TypeName];
			DataColumn dataColumn7 = dataTypesTable.Columns[DbMetaDataColumnNames.IsNullable];
			if (dataColumn == null || dataColumn2 == null || dataColumn3 == null || dataColumn4 == null || dataColumn5 == null || dataColumn6 == null || dataColumn7 == null)
			{
				throw ADP.InvalidXml();
			}
			using (IDataReader dataReader = sqlCommand.ExecuteReader())
			{
				object[] array = new object[11];
				while (dataReader.Read())
				{
					dataReader.GetValues(array);
					DataRow dataRow = dataTypesTable.NewRow();
					dataRow[dataColumn] = SqlDbType.Udt;
					if (array[10] != DBNull.Value)
					{
						dataRow[dataColumn2] = array[10];
					}
					if (array[9] != DBNull.Value)
					{
						dataRow[dataColumn3] = array[9];
					}
					dataRow[dataColumn4] = true;
					dataRow[dataColumn5] = false;
					if (array[8] != DBNull.Value)
					{
						dataRow[dataColumn7] = array[8];
					}
					if (array[0] != DBNull.Value && array[1] != DBNull.Value && array[2] != DBNull.Value && array[3] != DBNull.Value && array[4] != DBNull.Value && array[5] != DBNull.Value)
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append(array[1].ToString());
						stringBuilder.Append(", ");
						stringBuilder.Append(array[0].ToString());
						stringBuilder.Append(", Version=");
						stringBuilder.Append(array[2].ToString());
						stringBuilder.Append(".");
						stringBuilder.Append(array[3].ToString());
						stringBuilder.Append(".");
						stringBuilder.Append(array[4].ToString());
						stringBuilder.Append(".");
						stringBuilder.Append(array[5].ToString());
						if (array[6] != DBNull.Value)
						{
							stringBuilder.Append(", Culture=");
							stringBuilder.Append(array[6].ToString());
						}
						if (array[7] != DBNull.Value)
						{
							stringBuilder.Append(", PublicKeyToken=");
							StringBuilder stringBuilder2 = new StringBuilder();
							byte[] array2 = (byte[])array[7];
							foreach (byte b in array2)
							{
								stringBuilder2.Append(string.Format(null, "{0,-2:x2}", new object[] { b }));
							}
							stringBuilder.Append(stringBuilder2.ToString());
						}
						dataRow[dataColumn6] = stringBuilder.ToString();
						dataTypesTable.Rows.Add(dataRow);
						dataRow.AcceptChanges();
					}
				}
			}
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x0028D704 File Offset: 0x0028CB04
		private void AddTVPsToDataTypesTable(DataTable dataTypesTable, SqlConnection connection, string ServerVersion)
		{
			if (0 > string.Compare(ServerVersion, "10.00.0000", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			SqlCommand sqlCommand = connection.CreateCommand();
			sqlCommand.CommandText = "select name, is_nullable, max_length from sys.types where is_table_type = 1";
			DataColumn dataColumn = dataTypesTable.Columns[DbMetaDataColumnNames.ProviderDbType];
			DataColumn dataColumn2 = dataTypesTable.Columns[DbMetaDataColumnNames.ColumnSize];
			DataColumn dataColumn3 = dataTypesTable.Columns[DbMetaDataColumnNames.IsSearchable];
			DataColumn dataColumn4 = dataTypesTable.Columns[DbMetaDataColumnNames.IsLiteralSupported];
			DataColumn dataColumn5 = dataTypesTable.Columns[DbMetaDataColumnNames.TypeName];
			DataColumn dataColumn6 = dataTypesTable.Columns[DbMetaDataColumnNames.IsNullable];
			if (dataColumn == null || dataColumn2 == null || dataColumn3 == null || dataColumn4 == null || dataColumn5 == null || dataColumn6 == null)
			{
				throw ADP.InvalidXml();
			}
			using (IDataReader dataReader = sqlCommand.ExecuteReader())
			{
				object[] array = new object[11];
				while (dataReader.Read())
				{
					dataReader.GetValues(array);
					DataRow dataRow = dataTypesTable.NewRow();
					dataRow[dataColumn] = SqlDbType.Structured;
					if (array[2] != DBNull.Value)
					{
						dataRow[dataColumn2] = array[2];
					}
					dataRow[dataColumn3] = false;
					dataRow[dataColumn4] = false;
					if (array[1] != DBNull.Value)
					{
						dataRow[dataColumn6] = array[1];
					}
					if (array[0] != DBNull.Value)
					{
						dataRow[dataColumn5] = array[0];
						dataTypesTable.Rows.Add(dataRow);
						dataRow.AcceptChanges();
					}
				}
			}
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x0028D894 File Offset: 0x0028CC94
		private DataTable GetDataTypesTable(SqlConnection connection)
		{
			if (base.CollectionDataSet.Tables[DbMetaDataCollectionNames.DataTypes] == null)
			{
				throw ADP.UnableToBuildCollection(DbMetaDataCollectionNames.DataTypes);
			}
			DataTable dataTable = base.CloneAndFilterCollection(DbMetaDataCollectionNames.DataTypes, null);
			this.addUDTsToDataTypesTable(dataTable, connection, base.ServerVersionNormalized);
			this.AddTVPsToDataTypesTable(dataTable, connection, base.ServerVersionNormalized);
			dataTable.AcceptChanges();
			return dataTable;
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x0028D8F8 File Offset: 0x0028CCF8
		protected override DataTable PrepareCollection(string collectionName, string[] restrictions, DbConnection connection)
		{
			SqlConnection sqlConnection = (SqlConnection)connection;
			DataTable dataTable = null;
			if (collectionName == DbMetaDataCollectionNames.DataTypes)
			{
				if (!ADP.IsEmptyArray(restrictions))
				{
					throw ADP.TooManyRestrictions(DbMetaDataCollectionNames.DataTypes);
				}
				dataTable = this.GetDataTypesTable(sqlConnection);
			}
			if (dataTable == null)
			{
				throw ADP.UnableToBuildCollection(collectionName);
			}
			return dataTable;
		}

		// Token: 0x04001925 RID: 6437
		private const string _serverVersionNormalized90 = "09.00.0000";

		// Token: 0x04001926 RID: 6438
		private const string _serverVersionNormalized90782 = "09.00.0782";

		// Token: 0x04001927 RID: 6439
		private const string _serverVersionNormalized10 = "10.00.0000";
	}
}
