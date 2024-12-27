using System;
using System.Collections;
using System.Data.Common;
using System.Globalization;
using System.Text;

namespace System.Data.OracleClient
{
	// Token: 0x0200007A RID: 122
	internal sealed class OracleSqlParser : DbSqlParser
	{
		// Token: 0x0600068D RID: 1677 RVA: 0x0006E99C File Offset: 0x0006DD9C
		internal OracleSqlParser()
			: base(OracleSqlParser._quoteCharacter, OracleSqlParser._quoteCharacter, OracleSqlParser._regexPattern)
		{
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0006E9C0 File Offset: 0x0006DDC0
		internal static string CatalogCase(string value)
		{
			if (ADP.IsEmpty(value))
			{
				return string.Empty;
			}
			if ('"' == value[0])
			{
				return value.Substring(1, value.Length - 2);
			}
			return value.ToUpper(CultureInfo.CurrentCulture);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0006EA04 File Offset: 0x0006DE04
		protected override bool CatalogMatch(string valueA, string valueB)
		{
			if (ADP.IsEmpty(valueA) && ADP.IsEmpty(valueB))
			{
				return true;
			}
			if (ADP.IsEmpty(valueA) || ADP.IsEmpty(valueB))
			{
				return false;
			}
			bool flag = '"' == valueA[0];
			int num = 0;
			int num2 = valueA.Length;
			bool flag2 = '"' == valueB[0];
			int num3 = 0;
			int num4 = valueB.Length;
			if (flag)
			{
				num++;
				num2 -= 2;
			}
			if (flag2)
			{
				num3++;
				num4 -= 2;
			}
			CompareOptions compareOptions = CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;
			if (!flag || !flag2)
			{
				compareOptions |= CompareOptions.IgnoreCase;
			}
			int num5 = CultureInfo.CurrentCulture.CompareInfo.Compare(valueA, num, num2, valueB, num3, num4, compareOptions);
			return 0 == num5;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0006EAAC File Offset: 0x0006DEAC
		private DbSqlParserColumn FindConstraintColumn(string schemaName, string tableName, string columnName)
		{
			DbSqlParserColumnCollection columns = base.Columns;
			int count = columns.Count;
			for (int i = 0; i < count; i++)
			{
				DbSqlParserColumn dbSqlParserColumn = columns[i];
				if (this.CatalogMatch(dbSqlParserColumn.SchemaName, schemaName) && this.CatalogMatch(dbSqlParserColumn.TableName, tableName) && this.CatalogMatch(dbSqlParserColumn.ColumnName, columnName))
				{
					return dbSqlParserColumn;
				}
			}
			return null;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0006EB0C File Offset: 0x0006DF0C
		protected override void GatherKeyColumns(DbSqlParserTable table)
		{
			using (OracleCommand oracleCommand = this._connection.CreateCommand())
			{
				oracleCommand.Transaction = this._connection.Transaction;
				string text = OracleSqlParser.CatalogCase(table.SchemaName);
				string text2 = OracleSqlParser.CatalogCase(table.TableName);
				string text3 = text;
				string text4 = text2;
				oracleCommand.CommandText = this.GetSynonymQueryStatement(text, text2);
				using (OracleDataReader oracleDataReader = oracleCommand.ExecuteReader())
				{
					if (oracleDataReader.Read())
					{
						text3 = oracleDataReader.GetString(0);
						text4 = oracleDataReader.GetString(1);
					}
				}
				StringBuilder stringBuilder = new StringBuilder(OracleSqlParser.ConstraintQuery1a);
				StringBuilder stringBuilder2 = new StringBuilder(OracleSqlParser.ConstraintQuery2a);
				if (ADP.IsEmpty(text3))
				{
					stringBuilder.Append(OracleSqlParser.ConstraintQuery1b_ownerDefault);
					stringBuilder2.Append(OracleSqlParser.ConstraintQuery2b_ownerDefault);
				}
				else
				{
					oracleCommand.Parameters.Add(new OracleParameter(OracleSqlParser.ConstraintOwnerParameterName, DbType.String)).Value = text3;
					stringBuilder.Append(OracleSqlParser.ConstraintQuery1b_ownerIsKnown);
					stringBuilder2.Append(OracleSqlParser.ConstraintQuery2b_ownerIsKnown);
				}
				oracleCommand.Parameters.Add(new OracleParameter(OracleSqlParser.ConstraintTableParameterName, DbType.String)).Value = text4;
				stringBuilder.Append(OracleSqlParser.ConstraintQuery1c);
				stringBuilder2.Append(OracleSqlParser.ConstraintQuery2c);
				string[] array = new string[]
				{
					stringBuilder.ToString(),
					stringBuilder2.ToString()
				};
				foreach (string text5 in array)
				{
					oracleCommand.CommandText = text5;
					using (OracleDataReader oracleDataReader2 = oracleCommand.ExecuteReader())
					{
						ArrayList arrayList = new ArrayList();
						bool flag = oracleDataReader2.Read();
						bool flag2 = false;
						while (flag)
						{
							arrayList.Clear();
							string @string = oracleDataReader2.GetString(0);
							do
							{
								arrayList.Add(new OracleSqlParser.ConstraintColumn
								{
									columnName = oracleDataReader2.GetString(1),
									constraintType = (DbSqlParserColumn.ConstraintType)(int)oracleDataReader2.GetDecimal(2),
									parsedColumn = null
								});
								flag = oracleDataReader2.Read();
							}
							while (flag && @string == oracleDataReader2.GetString(0));
							flag2 = true;
							for (int j = 0; j < arrayList.Count; j++)
							{
								OracleSqlParser.ConstraintColumn constraintColumn = (OracleSqlParser.ConstraintColumn)arrayList[j];
								constraintColumn.parsedColumn = this.FindConstraintColumn(text, text2, constraintColumn.columnName);
								if (constraintColumn.parsedColumn == null)
								{
									flag2 = false;
									break;
								}
							}
							if (flag2)
							{
								for (int k = 0; k < arrayList.Count; k++)
								{
									OracleSqlParser.ConstraintColumn constraintColumn = (OracleSqlParser.ConstraintColumn)arrayList[k];
									constraintColumn.parsedColumn.SetConstraint(constraintColumn.constraintType);
								}
								break;
							}
						}
						if (flag2)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0006EE10 File Offset: 0x0006E210
		protected override DbSqlParserColumnCollection GatherTableColumns(DbSqlParserTable table)
		{
			OciStatementHandle ociStatementHandle = new OciStatementHandle(this._connection.ServiceContextHandle);
			OciErrorHandle errorHandle = this._connection.ErrorHandle;
			StringBuilder stringBuilder = new StringBuilder();
			string schemaName = table.SchemaName;
			string tableName = table.TableName;
			DbSqlParserColumnCollection dbSqlParserColumnCollection = new DbSqlParserColumnCollection();
			stringBuilder.Append("select * from ");
			if (!ADP.IsEmpty(schemaName))
			{
				stringBuilder.Append(schemaName);
				stringBuilder.Append(".");
			}
			stringBuilder.Append(tableName);
			string text = stringBuilder.ToString();
			if (TracedNativeMethods.OCIStmtPrepare(ociStatementHandle, errorHandle, text, OCI.SYNTAX.OCI_NTV_SYNTAX, OCI.MODE.OCI_DEFAULT, this._connection) == 0 && TracedNativeMethods.OCIStmtExecute(this._connection.ServiceContextHandle, ociStatementHandle, errorHandle, 0, OCI.MODE.OCI_SHARED) == 0)
			{
				int num;
				ociStatementHandle.GetAttribute(OCI.ATTR.OCI_ATTR_PARAM_COUNT, out num, errorHandle);
				for (int i = 0; i < num; i++)
				{
					OciParameterDescriptor descriptor = ociStatementHandle.GetDescriptor(i, errorHandle);
					string text2;
					descriptor.GetAttribute(OCI.ATTR.OCI_ATTR_SQLCODE, out text2, errorHandle, this._connection);
					OciHandle.SafeDispose(ref descriptor);
					text2 = this.QuotePrefixCharacter + text2 + this.QuoteSuffixCharacter;
					dbSqlParserColumnCollection.Add(null, schemaName, tableName, text2, null);
				}
			}
			OciHandle.SafeDispose(ref ociStatementHandle);
			return dbSqlParserColumnCollection;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0006EF30 File Offset: 0x0006E330
		private string GetSynonymQueryStatement(string schemaName, string tableName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select table_owner, table_name from all_synonyms where");
			if (ADP.IsEmpty(schemaName))
			{
				stringBuilder.Append(" owner in ('PUBLIC', user)");
			}
			else
			{
				stringBuilder.Append(" owner = '");
				stringBuilder.Append(schemaName);
				stringBuilder.Append("'");
			}
			stringBuilder.Append(" and synonym_name = '");
			stringBuilder.Append(tableName);
			stringBuilder.Append("' order by decode(owner, 'PUBLIC', 2, 1)");
			return stringBuilder.ToString();
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0006EFAC File Offset: 0x0006E3AC
		internal void Parse(string statementText, OracleConnection connection)
		{
			this._connection = connection;
			base.Parse(statementText);
		}

		// Token: 0x040004AE RID: 1198
		private const string SynonymQueryBegin = "select table_owner, table_name from all_synonyms where";

		// Token: 0x040004AF RID: 1199
		private const string SynonymQueryNoSchema = " owner in ('PUBLIC', user)";

		// Token: 0x040004B0 RID: 1200
		private const string SynonymQuerySchema = " owner = '";

		// Token: 0x040004B1 RID: 1201
		private const string SynonymQueryTable = " and synonym_name = '";

		// Token: 0x040004B2 RID: 1202
		private const string SynonymQueryEnd = "' order by decode(owner, 'PUBLIC', 2, 1)";

		// Token: 0x040004B3 RID: 1203
		private static readonly string ConstraintOwnerParameterName = "OwnerName";

		// Token: 0x040004B4 RID: 1204
		private static readonly string ConstraintTableParameterName = "TableName";

		// Token: 0x040004B5 RID: 1205
		private static readonly string ConstraintQuery1a = "select ac.constraint_name key_name, acc.column_name key_col," + 1.ToString(CultureInfo.InvariantCulture) + " from all_cons_columns acc, all_constraints ac where acc.owner = ac.owner and acc.constraint_name = ac.constraint_name and acc.table_name = ac.table_name and ac.constraint_type = 'P'";

		// Token: 0x040004B6 RID: 1206
		private static readonly string ConstraintQuery1b_ownerDefault = " and ac.owner = user";

		// Token: 0x040004B7 RID: 1207
		private static readonly string ConstraintQuery1b_ownerIsKnown = " and ac.owner = :OwnerName";

		// Token: 0x040004B8 RID: 1208
		private static readonly string ConstraintQuery1c = " and ac.table_name = :TableName order by acc.constraint_name";

		// Token: 0x040004B9 RID: 1209
		private static readonly string ConstraintQuery2a = "select aic.index_name key_name, aic.column_name key_col," + 3.ToString(CultureInfo.InvariantCulture) + " from all_ind_columns aic, all_indexes ai where aic.table_owner = ai.table_owner and aic.table_name = ai.table_name and aic.index_name = ai.index_name and ai.uniqueness = 'UNIQUE'";

		// Token: 0x040004BA RID: 1210
		private static readonly string ConstraintQuery2b_ownerDefault = " and ai.owner = user";

		// Token: 0x040004BB RID: 1211
		private static readonly string ConstraintQuery2b_ownerIsKnown = " and ai.owner = :OwnerName";

		// Token: 0x040004BC RID: 1212
		private static readonly string ConstraintQuery2c = " and ai.table_name = :TableName order by aic.index_name";

		// Token: 0x040004BD RID: 1213
		private OracleConnection _connection;

		// Token: 0x040004BE RID: 1214
		private static readonly string _quoteCharacter = "\"";

		// Token: 0x040004BF RID: 1215
		private static readonly string _regexPattern = DbSqlParser.CreateRegexPattern("[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\uff3f_#$]", "[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\uff3f_#$]", OracleSqlParser._quoteCharacter, "([^\"]|\"\")*", OracleSqlParser._quoteCharacter, "('([^']|'')*')");

		// Token: 0x0200007B RID: 123
		private sealed class ConstraintColumn
		{
			// Token: 0x040004C0 RID: 1216
			internal string columnName;

			// Token: 0x040004C1 RID: 1217
			internal DbSqlParserColumn.ConstraintType constraintType;

			// Token: 0x040004C2 RID: 1218
			internal DbSqlParserColumn parsedColumn;
		}
	}
}
