using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Text;

namespace System.Data.OracleClient
{
	// Token: 0x0200004F RID: 79
	public sealed class OracleCommandBuilder : DbCommandBuilder
	{
		// Token: 0x060002E2 RID: 738 RVA: 0x0005F3E0 File Offset: 0x0005E7E0
		public OracleCommandBuilder()
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0005F3FC File Offset: 0x0005E7FC
		public OracleCommandBuilder(OracleDataAdapter adapter)
			: this()
		{
			this.DataAdapter = adapter;
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0005F418 File Offset: 0x0005E818
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x0005F428 File Offset: 0x0005E828
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override CatalogLocation CatalogLocation
		{
			get
			{
				return CatalogLocation.End;
			}
			set
			{
				if (CatalogLocation.End != value)
				{
					throw ADP.NotSupported();
				}
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0005F440 File Offset: 0x0005E840
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x0005F454 File Offset: 0x0005E854
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string CatalogSeparator
		{
			get
			{
				return "@";
			}
			set
			{
				if ("@" != value)
				{
					throw ADP.NotSupported();
				}
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0005F474 File Offset: 0x0005E874
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x0005F48C File Offset: 0x0005E88C
		[ResDescription("OracleCommandBuilder_DataAdapter")]
		[DefaultValue(null)]
		[ResCategory("OracleCategory_Update")]
		public new OracleDataAdapter DataAdapter
		{
			get
			{
				return (OracleDataAdapter)base.DataAdapter;
			}
			set
			{
				base.DataAdapter = value;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0005F4A0 File Offset: 0x0005E8A0
		// (set) Token: 0x060002EB RID: 747 RVA: 0x0005F4B4 File Offset: 0x0005E8B4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override string SchemaSeparator
		{
			get
			{
				return ".";
			}
			set
			{
				if ("." != value)
				{
					throw ADP.NotSupported();
				}
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0005F4D4 File Offset: 0x0005E8D4
		protected override void ApplyParameterInfo(DbParameter parameter, DataRow datarow, StatementType statementType, bool whereClause)
		{
			OracleParameter oracleParameter = (OracleParameter)parameter;
			object obj = datarow["ProviderType", DataRowVersion.Default];
			OracleType oracleType = (OracleType)obj;
			OracleType oracleType2 = oracleType;
			if (oracleType2 == OracleType.LongVarChar)
			{
				oracleType = OracleType.VarChar;
			}
			oracleParameter.OracleType = oracleType;
			oracleParameter.Offset = 0;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0005F518 File Offset: 0x0005E918
		public static void DeriveParameters(OracleCommand command)
		{
			OracleConnection.ExecutePermission.Demand();
			if (command == null)
			{
				throw ADP.ArgumentNull("command");
			}
			CommandType commandType = command.CommandType;
			if (commandType != CommandType.Text)
			{
				if (commandType != CommandType.StoredProcedure)
				{
					if (commandType != CommandType.TableDirect)
					{
						throw ADP.InvalidCommandType(command.CommandType);
					}
				}
				else
				{
					if (ADP.IsEmpty(command.CommandText))
					{
						throw ADP.CommandTextRequired("DeriveParameters");
					}
					OracleConnection connection = command.Connection;
					if (connection == null)
					{
						throw ADP.ConnectionRequired("DeriveParameters");
					}
					ConnectionState state = connection.State;
					if (ConnectionState.Open != state)
					{
						throw ADP.OpenConnectionRequired("DeriveParameters", state);
					}
					ArrayList arrayList = OracleCommandBuilder.DeriveParametersFromStoredProcedure(connection, command);
					OracleParameterCollection parameters = command.Parameters;
					parameters.Clear();
					int count = arrayList.Count;
					for (int i = 0; i < count; i++)
					{
						parameters.Add((OracleParameter)arrayList[i]);
					}
					return;
				}
			}
			throw ADP.DeriveParametersNotSupported(command);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0005F5F0 File Offset: 0x0005E9F0
		private static ArrayList DeriveParametersFromStoredProcedure(OracleConnection connection, OracleCommand command)
		{
			ArrayList arrayList = new ArrayList();
			OracleCommand oracleCommand = connection.CreateCommand();
			oracleCommand.Transaction = command.Transaction;
			string text;
			string text2;
			string text3;
			string text4;
			if (OracleCommandBuilder.ResolveName(oracleCommand, command.CommandText, out text, out text2, out text3, out text4) != 0U)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(OracleCommandBuilder.DeriveParameterCommand_Part1);
				stringBuilder.Append(OracleCommandBuilder.QuoteIdentifier(text, "'", "''"));
				stringBuilder.Append(" and package_name");
				if (!ADP.IsNull(text2))
				{
					stringBuilder.Append(" = ");
					stringBuilder.Append(OracleCommandBuilder.QuoteIdentifier(text2, "'", "''"));
				}
				else
				{
					stringBuilder.Append(" is null");
				}
				stringBuilder.Append(" and object_name = ");
				stringBuilder.Append(OracleCommandBuilder.QuoteIdentifier(text3, "'", "''"));
				stringBuilder.Append("  order by overload, position");
				oracleCommand.Parameters.Clear();
				oracleCommand.CommandText = stringBuilder.ToString();
				using (OracleDataReader oracleDataReader = oracleCommand.ExecuteReader())
				{
					while (oracleDataReader.Read())
					{
						if (!ADP.IsNull(oracleDataReader.GetValue(0)))
						{
							throw ADP.CannotDeriveOverloaded();
						}
						string @string = oracleDataReader.GetString(1);
						ParameterDirection parameterDirection = (ParameterDirection)(int)oracleDataReader.GetDecimal(2);
						OracleType oracleType = (OracleType)(int)oracleDataReader.GetDecimal(3);
						int num = (int)oracleDataReader.GetDecimal(4);
						byte b = (byte)oracleDataReader.GetDecimal(5);
						int num2 = (int)oracleDataReader.GetDecimal(6);
						byte b2;
						if (num2 < 0)
						{
							b2 = 0;
						}
						else
						{
							b2 = (byte)num2;
						}
						OracleParameter oracleParameter = new OracleParameter(@string, oracleType, num, parameterDirection, true, b, b2, "", DataRowVersion.Current, null);
						arrayList.Add(oracleParameter);
					}
				}
			}
			return arrayList;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0005F7C4 File Offset: 0x0005EBC4
		public new OracleCommand GetInsertCommand()
		{
			return (OracleCommand)base.GetInsertCommand();
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0005F7DC File Offset: 0x0005EBDC
		public new OracleCommand GetInsertCommand(bool useColumnsForParameterNames)
		{
			return (OracleCommand)base.GetInsertCommand(useColumnsForParameterNames);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0005F7F8 File Offset: 0x0005EBF8
		public new OracleCommand GetUpdateCommand()
		{
			return (OracleCommand)base.GetUpdateCommand();
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0005F810 File Offset: 0x0005EC10
		public new OracleCommand GetUpdateCommand(bool useColumnsForParameterNames)
		{
			return (OracleCommand)base.GetUpdateCommand(useColumnsForParameterNames);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0005F82C File Offset: 0x0005EC2C
		public new OracleCommand GetDeleteCommand()
		{
			return (OracleCommand)base.GetDeleteCommand();
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0005F844 File Offset: 0x0005EC44
		public new OracleCommand GetDeleteCommand(bool useColumnsForParameterNames)
		{
			return (OracleCommand)base.GetDeleteCommand(useColumnsForParameterNames);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0005F860 File Offset: 0x0005EC60
		protected override string GetParameterName(int parameterOrdinal)
		{
			return "p" + parameterOrdinal.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0005F884 File Offset: 0x0005EC84
		protected override string GetParameterName(string parameterName)
		{
			return parameterName;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0005F894 File Offset: 0x0005EC94
		protected override string GetParameterPlaceholder(int parameterOrdinal)
		{
			return ":" + this.GetParameterName(parameterOrdinal);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0005F8B4 File Offset: 0x0005ECB4
		public override string QuoteIdentifier(string unquotedIdentifier)
		{
			return OracleCommandBuilder.QuoteIdentifier(unquotedIdentifier, "\"", "\"\"");
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0005F8D4 File Offset: 0x0005ECD4
		private static string QuoteIdentifier(string unquotedIdentifier, string quoteString, string quoteEscapeString)
		{
			ADP.CheckArgumentNull(unquotedIdentifier, "unquotedIdentifier");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(quoteString);
			stringBuilder.Append(unquotedIdentifier.Replace(quoteString, quoteEscapeString));
			stringBuilder.Append(quoteString);
			return stringBuilder.ToString();
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0005F918 File Offset: 0x0005ED18
		private static uint ResolveName(OracleCommand command, string nameToResolve, out string schema, out string packageName, out string objectName, out string dblink)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("begin dbms_utility.name_resolve(");
			stringBuilder.Append(OracleCommandBuilder.QuoteIdentifier(nameToResolve, "'", "''"));
			stringBuilder.Append(",1,:schema,:part1,:part2,:dblink,:part1type,:objectnum); end;");
			command.CommandText = stringBuilder.ToString();
			command.Parameters.Add(new OracleParameter("schema", OracleType.VarChar, 30)).Direction = ParameterDirection.Output;
			command.Parameters.Add(new OracleParameter("part1", OracleType.VarChar, 30)).Direction = ParameterDirection.Output;
			command.Parameters.Add(new OracleParameter("part2", OracleType.VarChar, 30)).Direction = ParameterDirection.Output;
			command.Parameters.Add(new OracleParameter("dblink", OracleType.VarChar, 128)).Direction = ParameterDirection.Output;
			command.Parameters.Add(new OracleParameter("part1type", OracleType.UInt32)).Direction = ParameterDirection.Output;
			command.Parameters.Add(new OracleParameter("objectnum", OracleType.UInt32)).Direction = ParameterDirection.Output;
			command.ExecuteNonQuery();
			object value = command.Parameters["objectnum"].Value;
			if (ADP.IsNull(value))
			{
				schema = string.Empty;
				packageName = string.Empty;
				objectName = string.Empty;
				dblink = string.Empty;
				return 0U;
			}
			schema = (ADP.IsNull(command.Parameters["schema"].Value) ? null : ((string)command.Parameters["schema"].Value));
			packageName = (ADP.IsNull(command.Parameters["part1"].Value) ? null : ((string)command.Parameters["part1"].Value));
			objectName = (ADP.IsNull(command.Parameters["part2"].Value) ? null : ((string)command.Parameters["part2"].Value));
			dblink = (ADP.IsNull(command.Parameters["dblink"].Value) ? null : ((string)command.Parameters["dblink"].Value));
			return (uint)command.Parameters["part1type"].Value;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0005FB6C File Offset: 0x0005EF6C
		private void RowUpdatingHandler(object sender, OracleRowUpdatingEventArgs ruevent)
		{
			base.RowUpdatingHandler(ruevent);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0005FB80 File Offset: 0x0005EF80
		protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
		{
			if (adapter == base.DataAdapter)
			{
				((OracleDataAdapter)adapter).RowUpdating -= this.RowUpdatingHandler;
				return;
			}
			((OracleDataAdapter)adapter).RowUpdating += this.RowUpdatingHandler;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0005FBC8 File Offset: 0x0005EFC8
		public override string UnquoteIdentifier(string quotedIdentifier)
		{
			ADP.CheckArgumentNull(quotedIdentifier, "quotedIdentifier");
			if (quotedIdentifier.Length < 2 || quotedIdentifier[0] != '"' || quotedIdentifier[quotedIdentifier.Length - 1] != '"')
			{
				throw ADP.IdentifierIsNotQuoted();
			}
			return quotedIdentifier.Substring(1, quotedIdentifier.Length - 2).Replace("\"\"", "\"");
		}

		// Token: 0x0400035B RID: 859
		private const char _doubleQuoteChar = '"';

		// Token: 0x0400035C RID: 860
		private const string _doubleQuoteString = "\"";

		// Token: 0x0400035D RID: 861
		private const string _doubleQuoteEscapeString = "\"\"";

		// Token: 0x0400035E RID: 862
		private const char _singleQuoteChar = '\'';

		// Token: 0x0400035F RID: 863
		private const string _singleQuoteString = "'";

		// Token: 0x04000360 RID: 864
		private const string _singleQuoteEscapeString = "''";

		// Token: 0x04000361 RID: 865
		private const string ResolveNameCommand_Part1 = "begin dbms_utility.name_resolve(";

		// Token: 0x04000362 RID: 866
		private const string ResolveNameCommand_Part2 = ",1,:schema,:part1,:part2,:dblink,:part1type,:objectnum); end;";

		// Token: 0x04000363 RID: 867
		private const string DeriveParameterCommand_Part2 = " and package_name";

		// Token: 0x04000364 RID: 868
		private const string DeriveParameterCommand_Part3 = " and object_name = ";

		// Token: 0x04000365 RID: 869
		private const string DeriveParameterCommand_Part4 = "  order by overload, position";

		// Token: 0x04000366 RID: 870
		private static readonly string DeriveParameterCommand_Part1 = string.Concat(new object[]
		{
			"select overload, decode(position,0,'RETURN_VALUE',nvl(argument_name,chr(0))) name, decode(in_out,'IN',1,'IN/OUT',3,'OUT',decode(argument_name,null,6,2),1) direction, decode(data_type, 'BFILE',",
			1.ToString(CultureInfo.CurrentCulture),
			", 'BLOB',",
			2.ToString(CultureInfo.CurrentCulture),
			", 'CHAR',",
			3.ToString(CultureInfo.CurrentCulture),
			", 'CLOB',",
			4.ToString(CultureInfo.CurrentCulture),
			", 'DATE',",
			6.ToString(CultureInfo.CurrentCulture),
			", 'FLOAT',",
			13.ToString(CultureInfo.CurrentCulture),
			", 'INTERVAL YEAR TO MONTH',",
			8.ToString(CultureInfo.CurrentCulture),
			", 'INTERVAL DAY TO SECOND',",
			7.ToString(CultureInfo.CurrentCulture),
			", 'LONG',",
			10.ToString(CultureInfo.CurrentCulture),
			", 'LONG RAW',",
			9.ToString(CultureInfo.CurrentCulture),
			", 'NCHAR',",
			11.ToString(CultureInfo.CurrentCulture),
			", 'NCLOB',",
			12.ToString(CultureInfo.CurrentCulture),
			", 'NUMBER',",
			13.ToString(CultureInfo.CurrentCulture),
			", 'NVARCHAR2',",
			14.ToString(CultureInfo.CurrentCulture),
			", 'RAW',",
			15.ToString(CultureInfo.CurrentCulture),
			", 'REF CURSOR',",
			5.ToString(CultureInfo.CurrentCulture),
			", 'ROWID',",
			16.ToString(CultureInfo.CurrentCulture),
			", 'TIMESTAMP',",
			18.ToString(CultureInfo.CurrentCulture),
			", 'TIMESTAMP WITH LOCAL TIME ZONE',",
			19.ToString(CultureInfo.CurrentCulture),
			", 'TIMESTAMP WITH TIME ZONE',",
			20.ToString(CultureInfo.CurrentCulture),
			", 'VARCHAR2',",
			22.ToString(CultureInfo.CurrentCulture),
			",",
			22.ToString(CultureInfo.CurrentCulture),
			") oracletype, decode(data_type, 'CHAR',",
			2000,
			", 'LONG',",
			int.MaxValue,
			", 'LONG RAW',",
			int.MaxValue,
			", 'NCHAR',",
			4000,
			", 'NVARCHAR2',",
			4000,
			", 'RAW',",
			2000,
			", 'VARCHAR2',",
			2000,
			",0) length, nvl(data_precision, 255) precision, nvl(data_scale, 255) scale from all_arguments where data_level = 0 and data_type is not null and owner = "
		});
	}
}
