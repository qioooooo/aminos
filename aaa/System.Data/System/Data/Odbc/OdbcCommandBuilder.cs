using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data.Odbc
{
	// Token: 0x020001D5 RID: 469
	public sealed class OdbcCommandBuilder : DbCommandBuilder
	{
		// Token: 0x060019C3 RID: 6595 RVA: 0x00240918 File Offset: 0x0023FD18
		public OdbcCommandBuilder()
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x00240934 File Offset: 0x0023FD34
		public OdbcCommandBuilder(OdbcDataAdapter adapter)
			: this()
		{
			this.DataAdapter = adapter;
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060019C5 RID: 6597 RVA: 0x00240950 File Offset: 0x0023FD50
		// (set) Token: 0x060019C6 RID: 6598 RVA: 0x00240968 File Offset: 0x0023FD68
		[ResCategory("DataCategory_Update")]
		[DefaultValue(null)]
		[ResDescription("OdbcCommandBuilder_DataAdapter")]
		public new OdbcDataAdapter DataAdapter
		{
			get
			{
				return base.DataAdapter as OdbcDataAdapter;
			}
			set
			{
				base.DataAdapter = value;
			}
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x0024097C File Offset: 0x0023FD7C
		private void OdbcRowUpdatingHandler(object sender, OdbcRowUpdatingEventArgs ruevent)
		{
			base.RowUpdatingHandler(ruevent);
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x00240990 File Offset: 0x0023FD90
		public new OdbcCommand GetInsertCommand()
		{
			return (OdbcCommand)base.GetInsertCommand();
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x002409A8 File Offset: 0x0023FDA8
		public new OdbcCommand GetInsertCommand(bool useColumnsForParameterNames)
		{
			return (OdbcCommand)base.GetInsertCommand(useColumnsForParameterNames);
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x002409C4 File Offset: 0x0023FDC4
		public new OdbcCommand GetUpdateCommand()
		{
			return (OdbcCommand)base.GetUpdateCommand();
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x002409DC File Offset: 0x0023FDDC
		public new OdbcCommand GetUpdateCommand(bool useColumnsForParameterNames)
		{
			return (OdbcCommand)base.GetUpdateCommand(useColumnsForParameterNames);
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x002409F8 File Offset: 0x0023FDF8
		public new OdbcCommand GetDeleteCommand()
		{
			return (OdbcCommand)base.GetDeleteCommand();
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x00240A10 File Offset: 0x0023FE10
		public new OdbcCommand GetDeleteCommand(bool useColumnsForParameterNames)
		{
			return (OdbcCommand)base.GetDeleteCommand(useColumnsForParameterNames);
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x00240A2C File Offset: 0x0023FE2C
		protected override string GetParameterName(int parameterOrdinal)
		{
			return "p" + parameterOrdinal.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x00240A50 File Offset: 0x0023FE50
		protected override string GetParameterName(string parameterName)
		{
			return parameterName;
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x00240A60 File Offset: 0x0023FE60
		protected override string GetParameterPlaceholder(int parameterOrdinal)
		{
			return "?";
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x00240A74 File Offset: 0x0023FE74
		protected override void ApplyParameterInfo(DbParameter parameter, DataRow datarow, StatementType statementType, bool whereClause)
		{
			OdbcParameter odbcParameter = (OdbcParameter)parameter;
			object obj = datarow[SchemaTableColumn.ProviderType];
			odbcParameter.OdbcType = (OdbcType)obj;
			object obj2 = datarow[SchemaTableColumn.NumericPrecision];
			if (DBNull.Value != obj2)
			{
				byte b = (byte)((short)obj2);
				odbcParameter.PrecisionInternal = ((byte.MaxValue != b) ? b : 0);
			}
			obj2 = datarow[SchemaTableColumn.NumericScale];
			if (DBNull.Value != obj2)
			{
				byte b2 = (byte)((short)obj2);
				odbcParameter.ScaleInternal = ((byte.MaxValue != b2) ? b2 : 0);
			}
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x00240B00 File Offset: 0x0023FF00
		public static void DeriveParameters(OdbcCommand command)
		{
			OdbcConnection.ExecutePermission.Demand();
			if (command == null)
			{
				throw ADP.ArgumentNull("command");
			}
			CommandType commandType = command.CommandType;
			if (commandType == CommandType.Text)
			{
				throw ADP.DeriveParametersNotSupported(command);
			}
			if (commandType != CommandType.StoredProcedure)
			{
				if (commandType != CommandType.TableDirect)
				{
					throw ADP.InvalidCommandType(command.CommandType);
				}
				throw ADP.DeriveParametersNotSupported(command);
			}
			else
			{
				if (ADP.IsEmpty(command.CommandText))
				{
					throw ADP.CommandTextRequired("DeriveParameters");
				}
				OdbcConnection connection = command.Connection;
				if (connection == null)
				{
					throw ADP.ConnectionRequired("DeriveParameters");
				}
				ConnectionState state = connection.State;
				if (ConnectionState.Open != state)
				{
					throw ADP.OpenConnectionRequired("DeriveParameters", state);
				}
				OdbcParameter[] array = OdbcCommandBuilder.DeriveParametersFromStoredProcedure(connection, command);
				OdbcParameterCollection parameters = command.Parameters;
				parameters.Clear();
				int num = array.Length;
				if (0 < num)
				{
					for (int i = 0; i < array.Length; i++)
					{
						parameters.Add(array[i]);
					}
				}
				return;
			}
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x00240BDC File Offset: 0x0023FFDC
		private static OdbcParameter[] DeriveParametersFromStoredProcedure(OdbcConnection connection, OdbcCommand command)
		{
			List<OdbcParameter> list = new List<OdbcParameter>();
			CMDWrapper statementHandle = command.GetStatementHandle();
			OdbcStatementHandle statementHandle2 = statementHandle.StatementHandle;
			string text = connection.QuoteChar("DeriveParameters");
			string[] array = MultipartIdentifier.ParseMultipartIdentifier(command.CommandText, text, text, '.', 4, true, "ODBC_ODBCCommandText", false);
			if (array[3] == null)
			{
				array[3] = command.CommandText;
			}
			ODBC32.RetCode retCode = statementHandle2.ProcedureColumns(array[1], array[2], array[3], null);
			if (retCode != ODBC32.RetCode.SUCCESS)
			{
				connection.HandleError(statementHandle2, retCode);
			}
			using (OdbcDataReader odbcDataReader = new OdbcDataReader(command, statementHandle, CommandBehavior.Default))
			{
				odbcDataReader.FirstResult();
				int fieldCount = odbcDataReader.FieldCount;
				while (odbcDataReader.Read())
				{
					OdbcParameter odbcParameter = new OdbcParameter();
					odbcParameter.ParameterName = odbcDataReader.GetString(3);
					switch (odbcDataReader.GetInt16(4))
					{
					case 1:
						odbcParameter.Direction = ParameterDirection.Input;
						break;
					case 2:
						odbcParameter.Direction = ParameterDirection.InputOutput;
						break;
					case 4:
						odbcParameter.Direction = ParameterDirection.Output;
						break;
					case 5:
						odbcParameter.Direction = ParameterDirection.ReturnValue;
						break;
					}
					odbcParameter.OdbcType = TypeMap.FromSqlType((ODBC32.SQL_TYPE)odbcDataReader.GetInt16(5))._odbcType;
					odbcParameter.Size = odbcDataReader.GetInt32(7);
					switch (odbcParameter.OdbcType)
					{
					case OdbcType.Decimal:
					case OdbcType.Numeric:
						odbcParameter.ScaleInternal = (byte)odbcDataReader.GetInt16(9);
						odbcParameter.PrecisionInternal = (byte)odbcDataReader.GetInt16(10);
						break;
					}
					list.Add(odbcParameter);
				}
			}
			retCode = statementHandle2.CloseCursor();
			return list.ToArray();
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x00240D74 File Offset: 0x00240174
		public override string QuoteIdentifier(string unquotedIdentifier)
		{
			return this.QuoteIdentifier(unquotedIdentifier, null);
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x00240D8C File Offset: 0x0024018C
		public string QuoteIdentifier(string unquotedIdentifier, OdbcConnection connection)
		{
			ADP.CheckArgumentNull(unquotedIdentifier, "unquotedIdentifier");
			string text = this.QuotePrefix;
			string text2 = this.QuoteSuffix;
			if (ADP.IsEmpty(text))
			{
				if (connection == null)
				{
					throw ADP.QuotePrefixNotSet("QuoteIdentifier");
				}
				text = connection.QuoteChar("QuoteIdentifier");
				text2 = text;
			}
			if (!ADP.IsEmpty(text) && text != " ")
			{
				return ADP.BuildQuotedString(text, text2, unquotedIdentifier);
			}
			return unquotedIdentifier;
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x00240DF8 File Offset: 0x002401F8
		protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
		{
			if (adapter == base.DataAdapter)
			{
				((OdbcDataAdapter)adapter).RowUpdating -= this.OdbcRowUpdatingHandler;
				return;
			}
			((OdbcDataAdapter)adapter).RowUpdating += this.OdbcRowUpdatingHandler;
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x00240E40 File Offset: 0x00240240
		public override string UnquoteIdentifier(string quotedIdentifier)
		{
			return this.UnquoteIdentifier(quotedIdentifier, null);
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x00240E58 File Offset: 0x00240258
		public string UnquoteIdentifier(string quotedIdentifier, OdbcConnection connection)
		{
			ADP.CheckArgumentNull(quotedIdentifier, "quotedIdentifier");
			string text = this.QuotePrefix;
			string text2 = this.QuoteSuffix;
			if (ADP.IsEmpty(text))
			{
				if (connection == null)
				{
					throw ADP.QuotePrefixNotSet("UnquoteIdentifier");
				}
				text = connection.QuoteChar("UnquoteIdentifier");
				text2 = text;
			}
			string text3;
			if (!ADP.IsEmpty(text) || text != " ")
			{
				ADP.RemoveStringQuotes(text, text2, quotedIdentifier, out text3);
			}
			else
			{
				text3 = quotedIdentifier;
			}
			return text3;
		}
	}
}
