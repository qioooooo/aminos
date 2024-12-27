using System;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data.OleDb
{
	// Token: 0x02000213 RID: 531
	public sealed class OleDbCommandBuilder : DbCommandBuilder
	{
		// Token: 0x06001E15 RID: 7701 RVA: 0x00255A90 File Offset: 0x00254E90
		public OleDbCommandBuilder()
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x00255AAC File Offset: 0x00254EAC
		public OleDbCommandBuilder(OleDbDataAdapter adapter)
			: this()
		{
			this.DataAdapter = adapter;
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001E17 RID: 7703 RVA: 0x00255AC8 File Offset: 0x00254EC8
		// (set) Token: 0x06001E18 RID: 7704 RVA: 0x00255AE0 File Offset: 0x00254EE0
		[ResDescription("OleDbCommandBuilder_DataAdapter")]
		[DefaultValue(null)]
		[ResCategory("DataCategory_Update")]
		public new OleDbDataAdapter DataAdapter
		{
			get
			{
				return base.DataAdapter as OleDbDataAdapter;
			}
			set
			{
				base.DataAdapter = value;
			}
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x00255AF4 File Offset: 0x00254EF4
		private void OleDbRowUpdatingHandler(object sender, OleDbRowUpdatingEventArgs ruevent)
		{
			base.RowUpdatingHandler(ruevent);
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x00255B08 File Offset: 0x00254F08
		public new OleDbCommand GetInsertCommand()
		{
			return (OleDbCommand)base.GetInsertCommand();
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x00255B20 File Offset: 0x00254F20
		public new OleDbCommand GetInsertCommand(bool useColumnsForParameterNames)
		{
			return (OleDbCommand)base.GetInsertCommand(useColumnsForParameterNames);
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x00255B3C File Offset: 0x00254F3C
		public new OleDbCommand GetUpdateCommand()
		{
			return (OleDbCommand)base.GetUpdateCommand();
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x00255B54 File Offset: 0x00254F54
		public new OleDbCommand GetUpdateCommand(bool useColumnsForParameterNames)
		{
			return (OleDbCommand)base.GetUpdateCommand(useColumnsForParameterNames);
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x00255B70 File Offset: 0x00254F70
		public new OleDbCommand GetDeleteCommand()
		{
			return (OleDbCommand)base.GetDeleteCommand();
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x00255B88 File Offset: 0x00254F88
		public new OleDbCommand GetDeleteCommand(bool useColumnsForParameterNames)
		{
			return (OleDbCommand)base.GetDeleteCommand(useColumnsForParameterNames);
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x00255BA4 File Offset: 0x00254FA4
		protected override string GetParameterName(int parameterOrdinal)
		{
			return "p" + parameterOrdinal.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x00255BC8 File Offset: 0x00254FC8
		protected override string GetParameterName(string parameterName)
		{
			return parameterName;
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x00255BD8 File Offset: 0x00254FD8
		protected override string GetParameterPlaceholder(int parameterOrdinal)
		{
			return "?";
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x00255BEC File Offset: 0x00254FEC
		protected override void ApplyParameterInfo(DbParameter parameter, DataRow datarow, StatementType statementType, bool whereClause)
		{
			OleDbParameter oleDbParameter = (OleDbParameter)parameter;
			object obj = datarow[SchemaTableColumn.ProviderType];
			oleDbParameter.OleDbType = (OleDbType)obj;
			object obj2 = datarow[SchemaTableColumn.NumericPrecision];
			if (DBNull.Value != obj2)
			{
				byte b = (byte)((short)obj2);
				oleDbParameter.PrecisionInternal = ((byte.MaxValue != b) ? b : 0);
			}
			obj2 = datarow[SchemaTableColumn.NumericScale];
			if (DBNull.Value != obj2)
			{
				byte b2 = (byte)((short)obj2);
				oleDbParameter.ScaleInternal = ((byte.MaxValue != b2) ? b2 : 0);
			}
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x00255C78 File Offset: 0x00255078
		public static void DeriveParameters(OleDbCommand command)
		{
			OleDbConnection.ExecutePermission.Demand();
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
				OleDbConnection connection = command.Connection;
				if (connection == null)
				{
					throw ADP.ConnectionRequired("DeriveParameters");
				}
				ConnectionState state = connection.State;
				if (ConnectionState.Open != state)
				{
					throw ADP.OpenConnectionRequired("DeriveParameters", state);
				}
				OleDbParameter[] array = OleDbCommandBuilder.DeriveParametersFromStoredProcedure(connection, command);
				OleDbParameterCollection parameters = command.Parameters;
				parameters.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					parameters.Add(array[i]);
				}
				return;
			}
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x00255D48 File Offset: 0x00255148
		private static OleDbParameter[] DeriveParametersFromStoredProcedure(OleDbConnection connection, OleDbCommand command)
		{
			OleDbParameter[] array = new OleDbParameter[0];
			if (connection.SupportSchemaRowset(OleDbSchemaGuid.Procedure_Parameters))
			{
				string text;
				string text2;
				connection.GetLiteralQuotes("DeriveParameters", out text, out text2);
				object[] array2 = MultipartIdentifier.ParseMultipartIdentifier(command.CommandText, text, text2, '.', 4, true, "OLEDB_OLEDBCommandText", false);
				if (array2[3] == null)
				{
					throw ADP.NoStoredProcedureExists(command.CommandText);
				}
				object[] array3 = new object[4];
				Array.Copy(array2, 1, array3, 0, 3);
				DataTable dataTable = connection.GetSchemaRowset(OleDbSchemaGuid.Procedure_Parameters, array3);
				if (dataTable != null)
				{
					DataColumnCollection columns = dataTable.Columns;
					DataColumn dataColumn = null;
					DataColumn dataColumn2 = null;
					DataColumn dataColumn3 = null;
					DataColumn dataColumn4 = null;
					DataColumn dataColumn5 = null;
					DataColumn dataColumn6 = null;
					DataColumn dataColumn7 = null;
					int i = columns.IndexOf("PARAMETER_NAME");
					if (-1 != i)
					{
						dataColumn = columns[i];
					}
					i = columns.IndexOf("PARAMETER_TYPE");
					if (-1 != i)
					{
						dataColumn2 = columns[i];
					}
					i = columns.IndexOf("DATA_TYPE");
					if (-1 != i)
					{
						dataColumn3 = columns[i];
					}
					i = columns.IndexOf("CHARACTER_MAXIMUM_LENGTH");
					if (-1 != i)
					{
						dataColumn4 = columns[i];
					}
					i = columns.IndexOf("NUMERIC_PRECISION");
					if (-1 != i)
					{
						dataColumn5 = columns[i];
					}
					i = columns.IndexOf("NUMERIC_SCALE");
					if (-1 != i)
					{
						dataColumn6 = columns[i];
					}
					i = columns.IndexOf("TYPE_NAME");
					if (-1 != i)
					{
						dataColumn7 = columns[i];
					}
					DataRow[] array4 = dataTable.Select(null, "ORDINAL_POSITION ASC", DataViewRowState.CurrentRows);
					array = new OleDbParameter[array4.Length];
					i = 0;
					while (i < array4.Length)
					{
						DataRow dataRow = array4[i];
						OleDbParameter oleDbParameter = new OleDbParameter();
						if (dataColumn != null && !dataRow.IsNull(dataColumn, DataRowVersion.Default))
						{
							oleDbParameter.ParameterName = Convert.ToString(dataRow[dataColumn, DataRowVersion.Default], CultureInfo.InvariantCulture).TrimStart(new char[] { '@', ' ', ':' });
						}
						if (dataColumn2 != null && !dataRow.IsNull(dataColumn2, DataRowVersion.Default))
						{
							short num = Convert.ToInt16(dataRow[dataColumn2, DataRowVersion.Default], CultureInfo.InvariantCulture);
							oleDbParameter.Direction = OleDbCommandBuilder.ConvertToParameterDirection((int)num);
						}
						if (dataColumn3 != null && !dataRow.IsNull(dataColumn3, DataRowVersion.Default))
						{
							short num2 = Convert.ToInt16(dataRow[dataColumn3, DataRowVersion.Default], CultureInfo.InvariantCulture);
							oleDbParameter.OleDbType = NativeDBType.FromDBType(num2, false, false).enumOleDbType;
						}
						if (dataColumn4 != null && !dataRow.IsNull(dataColumn4, DataRowVersion.Default))
						{
							oleDbParameter.Size = Convert.ToInt32(dataRow[dataColumn4, DataRowVersion.Default], CultureInfo.InvariantCulture);
						}
						OleDbType oleDbType = oleDbParameter.OleDbType;
						if (oleDbType <= OleDbType.Numeric)
						{
							if (oleDbType == OleDbType.Decimal || oleDbType == OleDbType.Numeric)
							{
								goto IL_02BB;
							}
						}
						else
						{
							if (oleDbType == OleDbType.VarNumeric)
							{
								goto IL_02BB;
							}
							switch (oleDbType)
							{
							case OleDbType.VarChar:
							case OleDbType.VarWChar:
							case OleDbType.VarBinary:
							{
								object obj = dataRow[dataColumn7, DataRowVersion.Default];
								if (obj is string)
								{
									string text3 = ((string)obj).ToLower(CultureInfo.InvariantCulture);
									string text4;
									if ((text4 = text3) != null)
									{
										if (!(text4 == "binary"))
										{
											if (!(text4 == "image"))
											{
												if (!(text4 == "char"))
												{
													if (!(text4 == "text"))
													{
														if (!(text4 == "nchar"))
														{
															if (text4 == "ntext")
															{
																oleDbParameter.OleDbType = OleDbType.LongVarWChar;
															}
														}
														else
														{
															oleDbParameter.OleDbType = OleDbType.WChar;
														}
													}
													else
													{
														oleDbParameter.OleDbType = OleDbType.LongVarChar;
													}
												}
												else
												{
													oleDbParameter.OleDbType = OleDbType.Char;
												}
											}
											else
											{
												oleDbParameter.OleDbType = OleDbType.LongVarBinary;
											}
										}
										else
										{
											oleDbParameter.OleDbType = OleDbType.Binary;
										}
									}
								}
								break;
							}
							}
						}
						IL_03F8:
						array[i] = oleDbParameter;
						i++;
						continue;
						IL_02BB:
						if (dataColumn5 != null && !dataRow.IsNull(dataColumn5, DataRowVersion.Default))
						{
							oleDbParameter.PrecisionInternal = (byte)Convert.ToInt16(dataRow[dataColumn5], CultureInfo.InvariantCulture);
						}
						if (dataColumn6 != null && !dataRow.IsNull(dataColumn6, DataRowVersion.Default))
						{
							oleDbParameter.ScaleInternal = (byte)Convert.ToInt16(dataRow[dataColumn6], CultureInfo.InvariantCulture);
							goto IL_03F8;
						}
						goto IL_03F8;
					}
				}
				if (array.Length == 0 && connection.SupportSchemaRowset(OleDbSchemaGuid.Procedures))
				{
					object[] array5 = new object[4];
					array5[2] = command.CommandText;
					array3 = array5;
					dataTable = connection.GetSchemaRowset(OleDbSchemaGuid.Procedures, array3);
					if (dataTable.Rows.Count == 0)
					{
						throw ADP.NoStoredProcedureExists(command.CommandText);
					}
				}
				return array;
			}
			else
			{
				if (!connection.SupportSchemaRowset(OleDbSchemaGuid.Procedures))
				{
					throw ODB.NoProviderSupportForSProcResetParameters(connection.Provider);
				}
				object[] array6 = new object[4];
				array6[2] = command.CommandText;
				object[] array7 = array6;
				DataTable schemaRowset = connection.GetSchemaRowset(OleDbSchemaGuid.Procedures, array7);
				if (schemaRowset.Rows.Count == 0)
				{
					throw ADP.NoStoredProcedureExists(command.CommandText);
				}
				throw ODB.NoProviderSupportForSProcResetParameters(connection.Provider);
			}
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x00256220 File Offset: 0x00255620
		private static ParameterDirection ConvertToParameterDirection(int value)
		{
			switch (value)
			{
			case 1:
				return ParameterDirection.Input;
			case 2:
				return ParameterDirection.InputOutput;
			case 3:
				return ParameterDirection.Output;
			case 4:
				return ParameterDirection.ReturnValue;
			default:
				return ParameterDirection.Input;
			}
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x00256254 File Offset: 0x00255654
		public override string QuoteIdentifier(string unquotedIdentifier)
		{
			return this.QuoteIdentifier(unquotedIdentifier, null);
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x0025626C File Offset: 0x0025566C
		public string QuoteIdentifier(string unquotedIdentifier, OleDbConnection connection)
		{
			ADP.CheckArgumentNull(unquotedIdentifier, "unquotedIdentifier");
			string quotePrefix = this.QuotePrefix;
			string text = this.QuoteSuffix;
			if (ADP.IsEmpty(quotePrefix))
			{
				if (connection == null)
				{
					throw ADP.QuotePrefixNotSet("QuoteIdentifier");
				}
				connection.GetLiteralQuotes("QuoteIdentifier", out quotePrefix, out text);
				if (text == null)
				{
					text = quotePrefix;
				}
			}
			return ADP.BuildQuotedString(quotePrefix, text, unquotedIdentifier);
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x002562C4 File Offset: 0x002556C4
		protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
		{
			if (adapter == base.DataAdapter)
			{
				((OleDbDataAdapter)adapter).RowUpdating -= this.OleDbRowUpdatingHandler;
				return;
			}
			((OleDbDataAdapter)adapter).RowUpdating += this.OleDbRowUpdatingHandler;
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x0025630C File Offset: 0x0025570C
		public override string UnquoteIdentifier(string quotedIdentifier)
		{
			return this.UnquoteIdentifier(quotedIdentifier, null);
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x00256324 File Offset: 0x00255724
		public string UnquoteIdentifier(string quotedIdentifier, OleDbConnection connection)
		{
			ADP.CheckArgumentNull(quotedIdentifier, "quotedIdentifier");
			string quotePrefix = this.QuotePrefix;
			string text = this.QuoteSuffix;
			if (ADP.IsEmpty(quotePrefix))
			{
				if (connection == null)
				{
					throw ADP.QuotePrefixNotSet("UnquoteIdentifier");
				}
				connection.GetLiteralQuotes("UnquoteIdentifier", out quotePrefix, out text);
				if (text == null)
				{
					text = quotePrefix;
				}
			}
			string text2;
			ADP.RemoveStringQuotes(quotePrefix, text, quotedIdentifier, out text2);
			return text2;
		}
	}
}
