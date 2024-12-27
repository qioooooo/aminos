using System;
using System.Data.Common;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Data.OleDb
{
	// Token: 0x02000211 RID: 529
	internal static class ODB
	{
		// Token: 0x06001D94 RID: 7572 RVA: 0x00252F00 File Offset: 0x00252300
		internal static void CommandParameterStatus(StringBuilder builder, int index, DBStatus status)
		{
			switch (status)
			{
			case DBStatus.S_OK:
			case DBStatus.S_ISNULL:
			case DBStatus.S_IGNORE:
				return;
			case DBStatus.E_BADACCESSOR:
				builder.Append(Res.GetString("OleDb_CommandParameterBadAccessor", new object[]
				{
					index.ToString(CultureInfo.InvariantCulture),
					""
				}));
				builder.Append(Environment.NewLine);
				return;
			case DBStatus.E_CANTCONVERTVALUE:
				builder.Append(Res.GetString("OleDb_CommandParameterCantConvertValue", new object[]
				{
					index.ToString(CultureInfo.InvariantCulture),
					""
				}));
				builder.Append(Environment.NewLine);
				return;
			case DBStatus.E_SIGNMISMATCH:
				builder.Append(Res.GetString("OleDb_CommandParameterSignMismatch", new object[]
				{
					index.ToString(CultureInfo.InvariantCulture),
					""
				}));
				builder.Append(Environment.NewLine);
				return;
			case DBStatus.E_DATAOVERFLOW:
				builder.Append(Res.GetString("OleDb_CommandParameterDataOverflow", new object[]
				{
					index.ToString(CultureInfo.InvariantCulture),
					""
				}));
				builder.Append(Environment.NewLine);
				return;
			case DBStatus.E_UNAVAILABLE:
				builder.Append(Res.GetString("OleDb_CommandParameterUnavailable", new object[]
				{
					index.ToString(CultureInfo.InvariantCulture),
					""
				}));
				builder.Append(Environment.NewLine);
				return;
			case DBStatus.S_DEFAULT:
				builder.Append(Res.GetString("OleDb_CommandParameterDefault", new object[]
				{
					index.ToString(CultureInfo.InvariantCulture),
					""
				}));
				builder.Append(Environment.NewLine);
				return;
			}
			builder.Append(Res.GetString("OleDb_CommandParameterError", new object[]
			{
				index.ToString(CultureInfo.InvariantCulture),
				status.ToString()
			}));
			builder.Append(Environment.NewLine);
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x00253118 File Offset: 0x00252518
		internal static Exception CommandParameterStatus(string value, Exception inner)
		{
			if (ADP.IsEmpty(value))
			{
				return inner;
			}
			return ADP.InvalidOperation(value, inner);
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x00253138 File Offset: 0x00252538
		internal static Exception UninitializedParameters(int index, OleDbType dbtype)
		{
			return ADP.InvalidOperation(Res.GetString("OleDb_UninitializedParameters", new object[]
			{
				index.ToString(CultureInfo.InvariantCulture),
				dbtype.ToString()
			}));
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x0025317C File Offset: 0x0025257C
		internal static Exception BadStatus_ParamAcc(int index, DBBindStatus status)
		{
			return ADP.DataAdapter(Res.GetString("OleDb_BadStatus_ParamAcc", new object[]
			{
				index.ToString(CultureInfo.InvariantCulture),
				status.ToString()
			}));
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x002531C0 File Offset: 0x002525C0
		internal static Exception NoProviderSupportForParameters(string provider, Exception inner)
		{
			return ADP.DataAdapter(Res.GetString("OleDb_NoProviderSupportForParameters", new object[] { provider }), inner);
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x002531EC File Offset: 0x002525EC
		internal static Exception NoProviderSupportForSProcResetParameters(string provider)
		{
			return ADP.DataAdapter(Res.GetString("OleDb_NoProviderSupportForSProcResetParameters", new object[] { provider }));
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x00253214 File Offset: 0x00252614
		internal static void PropsetSetFailure(StringBuilder builder, string description, OleDbPropertyStatus status)
		{
			if (status == OleDbPropertyStatus.Ok)
			{
				return;
			}
			switch (status)
			{
			case OleDbPropertyStatus.NotSupported:
				if (0 < builder.Length)
				{
					builder.Append(Environment.NewLine);
				}
				builder.Append(Res.GetString("OleDb_PropertyNotSupported", new object[] { description }));
				return;
			case OleDbPropertyStatus.BadValue:
				if (0 < builder.Length)
				{
					builder.Append(Environment.NewLine);
				}
				builder.Append(Res.GetString("OleDb_PropertyBadValue", new object[] { description }));
				return;
			case OleDbPropertyStatus.BadOption:
				if (0 < builder.Length)
				{
					builder.Append(Environment.NewLine);
				}
				builder.Append(Res.GetString("OleDb_PropertyBadOption", new object[] { description }));
				return;
			case OleDbPropertyStatus.BadColumn:
				if (0 < builder.Length)
				{
					builder.Append(Environment.NewLine);
				}
				builder.Append(Res.GetString("OleDb_PropertyBadColumn", new object[] { description }));
				return;
			case OleDbPropertyStatus.NotAllSettable:
				if (0 < builder.Length)
				{
					builder.Append(Environment.NewLine);
				}
				builder.Append(Res.GetString("OleDb_PropertyNotAllSettable", new object[] { description }));
				return;
			case OleDbPropertyStatus.NotSettable:
				if (0 < builder.Length)
				{
					builder.Append(Environment.NewLine);
				}
				builder.Append(Res.GetString("OleDb_PropertyNotSettable", new object[] { description }));
				return;
			case OleDbPropertyStatus.NotSet:
				if (0 < builder.Length)
				{
					builder.Append(Environment.NewLine);
				}
				builder.Append(Res.GetString("OleDb_PropertyNotSet", new object[] { description }));
				return;
			case OleDbPropertyStatus.Conflicting:
				if (0 < builder.Length)
				{
					builder.Append(Environment.NewLine);
				}
				builder.Append(Res.GetString("OleDb_PropertyConflicting", new object[] { description }));
				return;
			case OleDbPropertyStatus.NotAvailable:
				if (0 < builder.Length)
				{
					builder.Append(Environment.NewLine);
				}
				builder.Append(Res.GetString("OleDb_PropertyNotAvailable", new object[] { description }));
				return;
			default:
			{
				if (0 < builder.Length)
				{
					builder.Append(Environment.NewLine);
				}
				string text = "OleDb_PropertyStatusUnknown";
				object[] array = new object[1];
				object[] array2 = array;
				int num = 0;
				int num2 = (int)status;
				array2[num] = num2.ToString(CultureInfo.InvariantCulture);
				builder.Append(Res.GetString(text, array));
				return;
			}
			}
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x00253478 File Offset: 0x00252878
		internal static Exception PropsetSetFailure(string value, Exception inner)
		{
			if (ADP.IsEmpty(value))
			{
				return inner;
			}
			return ADP.InvalidOperation(value, inner);
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x00253498 File Offset: 0x00252898
		internal static ArgumentException SchemaRowsetsNotSupported(string provider)
		{
			return ADP.Argument(Res.GetString("OleDb_SchemaRowsetsNotSupported", new object[] { "IDBSchemaRowset", provider }));
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x002534C8 File Offset: 0x002528C8
		internal static OleDbException NoErrorInformation(string provider, OleDbHResult hr, Exception inner)
		{
			OleDbException ex;
			if (!ADP.IsEmpty(provider))
			{
				ex = new OleDbException(Res.GetString("OleDb_NoErrorInformation2", new object[]
				{
					provider,
					ODB.ELookup(hr)
				}), hr, inner);
			}
			else
			{
				ex = new OleDbException(Res.GetString("OleDb_NoErrorInformation", new object[] { ODB.ELookup(hr) }), hr, inner);
			}
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x00253530 File Offset: 0x00252930
		internal static InvalidOperationException MDACNotAvailable(Exception inner)
		{
			return ADP.DataAdapter(Res.GetString("OleDb_MDACNotAvailable"), inner);
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x00253550 File Offset: 0x00252950
		internal static ArgumentException MSDASQLNotSupported()
		{
			return ADP.Argument(Res.GetString("OleDb_MSDASQLNotSupported"));
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x0025356C File Offset: 0x0025296C
		internal static InvalidOperationException CommandTextNotSupported(string provider, Exception inner)
		{
			return ADP.DataAdapter(Res.GetString("OleDb_CommandTextNotSupported", new object[] { provider }), inner);
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x00253598 File Offset: 0x00252998
		internal static InvalidOperationException PossiblePromptNotUserInteractive()
		{
			return ADP.DataAdapter(Res.GetString("OleDb_PossiblePromptNotUserInteractive"));
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x002535B4 File Offset: 0x002529B4
		internal static InvalidOperationException ProviderUnavailable(string provider, Exception inner)
		{
			return ADP.DataAdapter(Res.GetString("OleDb_ProviderUnavailable", new object[] { provider }), inner);
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x002535E0 File Offset: 0x002529E0
		internal static InvalidOperationException TransactionsNotSupported(string provider, Exception inner)
		{
			return ADP.DataAdapter(Res.GetString("OleDb_TransactionsNotSupported", new object[] { provider }), inner);
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x0025360C File Offset: 0x00252A0C
		internal static ArgumentException AsynchronousNotSupported()
		{
			return ADP.Argument(Res.GetString("OleDb_AsynchronousNotSupported"));
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x00253628 File Offset: 0x00252A28
		internal static ArgumentException NoProviderSpecified()
		{
			return ADP.Argument(Res.GetString("OleDb_NoProviderSpecified"));
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x00253644 File Offset: 0x00252A44
		internal static ArgumentException InvalidProviderSpecified()
		{
			return ADP.Argument(Res.GetString("OleDb_InvalidProviderSpecified"));
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x00253660 File Offset: 0x00252A60
		internal static ArgumentException InvalidRestrictionsDbInfoKeywords(string parameter)
		{
			return ADP.Argument(Res.GetString("OleDb_InvalidRestrictionsDbInfoKeywords"), parameter);
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x00253680 File Offset: 0x00252A80
		internal static ArgumentException InvalidRestrictionsDbInfoLiteral(string parameter)
		{
			return ADP.Argument(Res.GetString("OleDb_InvalidRestrictionsDbInfoLiteral"), parameter);
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x002536A0 File Offset: 0x00252AA0
		internal static ArgumentException InvalidRestrictionsSchemaGuids(string parameter)
		{
			return ADP.Argument(Res.GetString("OleDb_InvalidRestrictionsSchemaGuids"), parameter);
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x002536C0 File Offset: 0x00252AC0
		internal static ArgumentException NotSupportedSchemaTable(Guid schema, OleDbConnection connection)
		{
			return ADP.Argument(Res.GetString("OleDb_NotSupportedSchemaTable", new object[]
			{
				OleDbSchemaGuid.GetTextFromValue(schema),
				connection.Provider
			}));
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x002536F8 File Offset: 0x00252AF8
		internal static Exception InvalidOleDbType(OleDbType value)
		{
			return ADP.InvalidEnumerationValue(typeof(OleDbType), (int)value);
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x00253718 File Offset: 0x00252B18
		internal static InvalidOperationException BadAccessor()
		{
			return ADP.DataAdapter(Res.GetString("OleDb_BadAccessor"));
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x00253734 File Offset: 0x00252B34
		internal static InvalidCastException ConversionRequired()
		{
			return ADP.InvalidCast();
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x00253748 File Offset: 0x00252B48
		internal static InvalidCastException CantConvertValue()
		{
			return ADP.InvalidCast(Res.GetString("OleDb_CantConvertValue"));
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x00253764 File Offset: 0x00252B64
		internal static InvalidOperationException SignMismatch(Type type)
		{
			return ADP.DataAdapter(Res.GetString("OleDb_SignMismatch", new object[] { type.Name }));
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x00253794 File Offset: 0x00252B94
		internal static InvalidOperationException DataOverflow(Type type)
		{
			return ADP.DataAdapter(Res.GetString("OleDb_DataOverflow", new object[] { type.Name }));
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x002537C4 File Offset: 0x00252BC4
		internal static InvalidOperationException CantCreate(Type type)
		{
			return ADP.DataAdapter(Res.GetString("OleDb_CantCreate", new object[] { type.Name }));
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x002537F4 File Offset: 0x00252BF4
		internal static InvalidOperationException Unavailable(Type type)
		{
			return ADP.DataAdapter(Res.GetString("OleDb_Unavailable", new object[] { type.Name }));
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x00253824 File Offset: 0x00252C24
		internal static InvalidOperationException UnexpectedStatusValue(DBStatus status)
		{
			return ADP.DataAdapter(Res.GetString("OleDb_UnexpectedStatusValue", new object[] { status.ToString() }));
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x00253858 File Offset: 0x00252C58
		internal static InvalidOperationException GVtUnknown(int wType)
		{
			return ADP.DataAdapter(Res.GetString("OleDb_GVtUnknown", new object[]
			{
				wType.ToString("X4", CultureInfo.InvariantCulture),
				wType.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x002538A0 File Offset: 0x00252CA0
		internal static InvalidOperationException SVtUnknown(int wType)
		{
			return ADP.DataAdapter(Res.GetString("OleDb_SVtUnknown", new object[]
			{
				wType.ToString("X4", CultureInfo.InvariantCulture),
				wType.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x002538E8 File Offset: 0x00252CE8
		internal static InvalidOperationException BadStatusRowAccessor(int i, DBBindStatus rowStatus)
		{
			return ADP.DataAdapter(Res.GetString("OleDb_BadStatusRowAccessor", new object[]
			{
				i.ToString(CultureInfo.InvariantCulture),
				rowStatus.ToString()
			}));
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x0025392C File Offset: 0x00252D2C
		internal static InvalidOperationException ThreadApartmentState(Exception innerException)
		{
			return ADP.InvalidOperation(Res.GetString("OleDb_ThreadApartmentState"), innerException);
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x0025394C File Offset: 0x00252D4C
		internal static ArgumentException Fill_NotADODB(string parameter)
		{
			return ADP.Argument(Res.GetString("OleDb_Fill_NotADODB"), parameter);
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x0025396C File Offset: 0x00252D6C
		internal static ArgumentException Fill_EmptyRecordSet(string parameter, Exception innerException)
		{
			return ADP.Argument(Res.GetString("OleDb_Fill_EmptyRecordSet", new object[] { "IRowset" }), parameter, innerException);
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x0025399C File Offset: 0x00252D9C
		internal static ArgumentException Fill_EmptyRecord(string parameter, Exception innerException)
		{
			return ADP.Argument(Res.GetString("OleDb_Fill_EmptyRecord"), parameter, innerException);
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x002539BC File Offset: 0x00252DBC
		internal static string NoErrorMessage(OleDbHResult errorcode)
		{
			return Res.GetString("OleDb_NoErrorMessage", new object[] { ODB.ELookup(errorcode) });
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x002539E4 File Offset: 0x00252DE4
		internal static string FailedGetDescription(OleDbHResult errorcode)
		{
			return Res.GetString("OleDb_FailedGetDescription", new object[] { ODB.ELookup(errorcode) });
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x00253A0C File Offset: 0x00252E0C
		internal static string FailedGetSource(OleDbHResult errorcode)
		{
			return Res.GetString("OleDb_FailedGetSource", new object[] { ODB.ELookup(errorcode) });
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x00253A34 File Offset: 0x00252E34
		internal static InvalidOperationException DBBindingGetVector()
		{
			return ADP.InvalidOperation(Res.GetString("OleDb_DBBindingGetVector"));
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x00253A50 File Offset: 0x00252E50
		internal static OleDbHResult GetErrorDescription(UnsafeNativeMethods.IErrorInfo errorInfo, OleDbHResult hresult, out string message)
		{
			Bid.Trace("<oledb.IErrorInfo.GetDescription|API|OS>\n");
			OleDbHResult description = errorInfo.GetDescription(out message);
			Bid.Trace("<oledb.IErrorInfo.GetDescription|API|OS|RET> %08X{HRESULT}, Message='%ls'\n", description, message);
			if (description < OleDbHResult.S_OK && ADP.IsEmpty(message))
			{
				message = ODB.FailedGetDescription(description) + Environment.NewLine + ODB.ELookup(hresult);
			}
			if (ADP.IsEmpty(message))
			{
				message = ODB.ELookup(hresult);
			}
			return description;
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x00253AB4 File Offset: 0x00252EB4
		internal static ArgumentException ISourcesRowsetNotSupported()
		{
			throw ADP.Argument("OleDb_ISourcesRowsetNotSupported");
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x00253ACC File Offset: 0x00252ECC
		internal static InvalidOperationException IDBInfoNotSupported()
		{
			return ADP.InvalidOperation(Res.GetString("OleDb_IDBInfoNotSupported"));
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x00253AE8 File Offset: 0x00252EE8
		internal static string ELookup(OleDbHResult hr)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(hr.ToString());
			if (0 < stringBuilder.Length && char.IsDigit(stringBuilder[0]))
			{
				stringBuilder.Length = 0;
			}
			stringBuilder.Append("(0x");
			StringBuilder stringBuilder2 = stringBuilder;
			int num = (int)hr;
			stringBuilder2.Append(num.ToString("X8", CultureInfo.InvariantCulture));
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x00253B64 File Offset: 0x00252F64
		// Note: this type is marked as 'beforefieldinit'.
		static ODB()
		{
			char[] array = new char[3];
			array[0] = '\r';
			array[1] = '\n';
			ODB.ErrorTrimCharacters = array;
		}

		// Token: 0x0400115A RID: 4442
		internal const int ADODB_AlreadyClosedError = -2146824584;

		// Token: 0x0400115B RID: 4443
		internal const int ADODB_NextResultError = -2146825037;

		// Token: 0x0400115C RID: 4444
		internal const int InternalStateExecuting = 5;

		// Token: 0x0400115D RID: 4445
		internal const int InternalStateFetching = 9;

		// Token: 0x0400115E RID: 4446
		internal const int InternalStateClosed = 0;

		// Token: 0x0400115F RID: 4447
		internal const int ExecutedIMultipleResults = 0;

		// Token: 0x04001160 RID: 4448
		internal const int ExecutedIRowset = 1;

		// Token: 0x04001161 RID: 4449
		internal const int ExecutedIRow = 2;

		// Token: 0x04001162 RID: 4450
		internal const int PrepareICommandText = 3;

		// Token: 0x04001163 RID: 4451
		internal const int InternalStateExecutingNot = -5;

		// Token: 0x04001164 RID: 4452
		internal const int InternalStateFetchingNot = -9;

		// Token: 0x04001165 RID: 4453
		internal const int InternalStateConnecting = 2;

		// Token: 0x04001166 RID: 4454
		internal const int InternalStateOpen = 1;

		// Token: 0x04001167 RID: 4455
		internal const int LargeDataSize = 8192;

		// Token: 0x04001168 RID: 4456
		internal const int CacheIncrement = 10;

		// Token: 0x04001169 RID: 4457
		internal const short VARIANT_TRUE = -1;

		// Token: 0x0400116A RID: 4458
		internal const short VARIANT_FALSE = 0;

		// Token: 0x0400116B RID: 4459
		internal const int CLSCTX_ALL = 23;

		// Token: 0x0400116C RID: 4460
		internal const int MaxProgIdLength = 255;

		// Token: 0x0400116D RID: 4461
		internal const int DBLITERAL_CATALOG_SEPARATOR = 3;

		// Token: 0x0400116E RID: 4462
		internal const int DBLITERAL_QUOTE_PREFIX = 15;

		// Token: 0x0400116F RID: 4463
		internal const int DBLITERAL_QUOTE_SUFFIX = 28;

		// Token: 0x04001170 RID: 4464
		internal const int DBLITERAL_SCHEMA_SEPARATOR = 27;

		// Token: 0x04001171 RID: 4465
		internal const int DBLITERAL_TABLE_NAME = 17;

		// Token: 0x04001172 RID: 4466
		internal const int DBPROP_ACCESSORDER = 231;

		// Token: 0x04001173 RID: 4467
		internal const int DBPROP_AUTH_CACHE_AUTHINFO = 5;

		// Token: 0x04001174 RID: 4468
		internal const int DBPROP_AUTH_ENCRYPT_PASSWORD = 6;

		// Token: 0x04001175 RID: 4469
		internal const int DBPROP_AUTH_INTEGRATED = 7;

		// Token: 0x04001176 RID: 4470
		internal const int DBPROP_AUTH_MASK_PASSWORD = 8;

		// Token: 0x04001177 RID: 4471
		internal const int DBPROP_AUTH_PASSWORD = 9;

		// Token: 0x04001178 RID: 4472
		internal const int DBPROP_AUTH_PERSIST_ENCRYPTED = 10;

		// Token: 0x04001179 RID: 4473
		internal const int DBPROP_AUTH_PERSIST_SENSITIVE_AUTHINFO = 11;

		// Token: 0x0400117A RID: 4474
		internal const int DBPROP_AUTH_USERID = 12;

		// Token: 0x0400117B RID: 4475
		internal const int DBPROP_CATALOGLOCATION = 22;

		// Token: 0x0400117C RID: 4476
		internal const int DBPROP_COMMANDTIMEOUT = 34;

		// Token: 0x0400117D RID: 4477
		internal const int DBPROP_CONNECTIONSTATUS = 244;

		// Token: 0x0400117E RID: 4478
		internal const int DBPROP_CURRENTCATALOG = 37;

		// Token: 0x0400117F RID: 4479
		internal const int DBPROP_DATASOURCENAME = 38;

		// Token: 0x04001180 RID: 4480
		internal const int DBPROP_DBMSNAME = 40;

		// Token: 0x04001181 RID: 4481
		internal const int DBPROP_DBMSVER = 41;

		// Token: 0x04001182 RID: 4482
		internal const int DBPROP_GROUPBY = 44;

		// Token: 0x04001183 RID: 4483
		internal const int DBPROP_HIDDENCOLUMNS = 258;

		// Token: 0x04001184 RID: 4484
		internal const int DBPROP_IColumnsRowset = 123;

		// Token: 0x04001185 RID: 4485
		internal const int DBPROP_IDENTIFIERCASE = 46;

		// Token: 0x04001186 RID: 4486
		internal const int DBPROP_INIT_ASYNCH = 200;

		// Token: 0x04001187 RID: 4487
		internal const int DBPROP_INIT_BINDFLAGS = 270;

		// Token: 0x04001188 RID: 4488
		internal const int DBPROP_INIT_CATALOG = 233;

		// Token: 0x04001189 RID: 4489
		internal const int DBPROP_INIT_DATASOURCE = 59;

		// Token: 0x0400118A RID: 4490
		internal const int DBPROP_INIT_GENERALTIMEOUT = 284;

		// Token: 0x0400118B RID: 4491
		internal const int DBPROP_INIT_HWND = 60;

		// Token: 0x0400118C RID: 4492
		internal const int DBPROP_INIT_IMPERSONATION_LEVEL = 61;

		// Token: 0x0400118D RID: 4493
		internal const int DBPROP_INIT_LCID = 186;

		// Token: 0x0400118E RID: 4494
		internal const int DBPROP_INIT_LOCATION = 62;

		// Token: 0x0400118F RID: 4495
		internal const int DBPROP_INIT_LOCKOWNER = 271;

		// Token: 0x04001190 RID: 4496
		internal const int DBPROP_INIT_MODE = 63;

		// Token: 0x04001191 RID: 4497
		internal const int DBPROP_INIT_OLEDBSERVICES = 248;

		// Token: 0x04001192 RID: 4498
		internal const int DBPROP_INIT_PROMPT = 64;

		// Token: 0x04001193 RID: 4499
		internal const int DBPROP_INIT_PROTECTION_LEVEL = 65;

		// Token: 0x04001194 RID: 4500
		internal const int DBPROP_INIT_PROVIDERSTRING = 160;

		// Token: 0x04001195 RID: 4501
		internal const int DBPROP_INIT_TIMEOUT = 66;

		// Token: 0x04001196 RID: 4502
		internal const int DBPROP_IRow = 263;

		// Token: 0x04001197 RID: 4503
		internal const int DBPROP_MAXROWS = 73;

		// Token: 0x04001198 RID: 4504
		internal const int DBPROP_MULTIPLERESULTS = 196;

		// Token: 0x04001199 RID: 4505
		internal const int DBPROP_ORDERBYCOLUNSINSELECT = 85;

		// Token: 0x0400119A RID: 4506
		internal const int DBPROP_PROVIDERFILENAME = 96;

		// Token: 0x0400119B RID: 4507
		internal const int DBPROP_QUOTEDIDENTIFIERCASE = 100;

		// Token: 0x0400119C RID: 4508
		internal const int DBPROP_RESETDATASOURCE = 247;

		// Token: 0x0400119D RID: 4509
		internal const int DBPROP_SQLSUPPORT = 109;

		// Token: 0x0400119E RID: 4510
		internal const int DBPROP_UNIQUEROWS = 238;

		// Token: 0x0400119F RID: 4511
		internal const int DBPROPSTATUS_OK = 0;

		// Token: 0x040011A0 RID: 4512
		internal const int DBPROPSTATUS_NOTSUPPORTED = 1;

		// Token: 0x040011A1 RID: 4513
		internal const int DBPROPSTATUS_BADVALUE = 2;

		// Token: 0x040011A2 RID: 4514
		internal const int DBPROPSTATUS_BADOPTION = 3;

		// Token: 0x040011A3 RID: 4515
		internal const int DBPROPSTATUS_BADCOLUMN = 4;

		// Token: 0x040011A4 RID: 4516
		internal const int DBPROPSTATUS_NOTALLSETTABLE = 5;

		// Token: 0x040011A5 RID: 4517
		internal const int DBPROPSTATUS_NOTSETTABLE = 6;

		// Token: 0x040011A6 RID: 4518
		internal const int DBPROPSTATUS_NOTSET = 7;

		// Token: 0x040011A7 RID: 4519
		internal const int DBPROPSTATUS_CONFLICTING = 8;

		// Token: 0x040011A8 RID: 4520
		internal const int DBPROPSTATUS_NOTAVAILABLE = 9;

		// Token: 0x040011A9 RID: 4521
		internal const int DBPROPOPTIONS_REQUIRED = 0;

		// Token: 0x040011AA RID: 4522
		internal const int DBPROPOPTIONS_OPTIONAL = 1;

		// Token: 0x040011AB RID: 4523
		internal const int DBPROPFLAGS_WRITE = 1024;

		// Token: 0x040011AC RID: 4524
		internal const int DBPROPFLAGS_SESSION = 4096;

		// Token: 0x040011AD RID: 4525
		internal const int DBPROPVAL_AO_RANDOM = 2;

		// Token: 0x040011AE RID: 4526
		internal const int DBPROPVAL_CL_END = 2;

		// Token: 0x040011AF RID: 4527
		internal const int DBPROPVAL_CL_START = 1;

		// Token: 0x040011B0 RID: 4528
		internal const int DBPROPVAL_CS_COMMUNICATIONFAILURE = 2;

		// Token: 0x040011B1 RID: 4529
		internal const int DBPROPVAL_CS_INITIALIZED = 1;

		// Token: 0x040011B2 RID: 4530
		internal const int DBPROPVAL_CS_UNINITIALIZED = 0;

		// Token: 0x040011B3 RID: 4531
		internal const int DBPROPVAL_GB_COLLATE = 16;

		// Token: 0x040011B4 RID: 4532
		internal const int DBPROPVAL_GB_CONTAINS_SELECT = 4;

		// Token: 0x040011B5 RID: 4533
		internal const int DBPROPVAL_GB_EQUALS_SELECT = 2;

		// Token: 0x040011B6 RID: 4534
		internal const int DBPROPVAL_GB_NO_RELATION = 8;

		// Token: 0x040011B7 RID: 4535
		internal const int DBPROPVAL_GB_NOT_SUPPORTED = 1;

		// Token: 0x040011B8 RID: 4536
		internal const int DBPROPVAL_IC_LOWER = 2;

		// Token: 0x040011B9 RID: 4537
		internal const int DBPROPVAL_IC_MIXED = 8;

		// Token: 0x040011BA RID: 4538
		internal const int DBPROPVAL_IC_SENSITIVE = 4;

		// Token: 0x040011BB RID: 4539
		internal const int DBPROPVAL_IC_UPPER = 1;

		// Token: 0x040011BC RID: 4540
		internal const int DBPROPVAL_IN_ALLOWNULL = 0;

		// Token: 0x040011BD RID: 4541
		internal const int DBPROPVAL_MR_NOTSUPPORTED = 0;

		// Token: 0x040011BE RID: 4542
		internal const int DBPROPVAL_RD_RESETALL = -1;

		// Token: 0x040011BF RID: 4543
		internal const int DBPROPVAL_OS_RESOURCEPOOLING = 1;

		// Token: 0x040011C0 RID: 4544
		internal const int DBPROPVAL_OS_TXNENLISTMENT = 2;

		// Token: 0x040011C1 RID: 4545
		internal const int DBPROPVAL_OS_CLIENTCURSOR = 4;

		// Token: 0x040011C2 RID: 4546
		internal const int DBPROPVAL_OS_AGR_AFTERSESSION = 8;

		// Token: 0x040011C3 RID: 4547
		internal const int DBPROPVAL_SQL_ODBC_MINIMUM = 1;

		// Token: 0x040011C4 RID: 4548
		internal const int DBPROPVAL_SQL_ESCAPECLAUSES = 256;

		// Token: 0x040011C5 RID: 4549
		internal const int DBKIND_GUID_NAME = 0;

		// Token: 0x040011C6 RID: 4550
		internal const int DBKIND_GUID_PROPID = 1;

		// Token: 0x040011C7 RID: 4551
		internal const int DBKIND_NAME = 2;

		// Token: 0x040011C8 RID: 4552
		internal const int DBKIND_PGUID_NAME = 3;

		// Token: 0x040011C9 RID: 4553
		internal const int DBKIND_PGUID_PROPID = 4;

		// Token: 0x040011CA RID: 4554
		internal const int DBKIND_PROPID = 5;

		// Token: 0x040011CB RID: 4555
		internal const int DBKIND_GUID = 6;

		// Token: 0x040011CC RID: 4556
		internal const int DBCOLUMNFLAGS_ISBOOKMARK = 1;

		// Token: 0x040011CD RID: 4557
		internal const int DBCOLUMNFLAGS_ISLONG = 128;

		// Token: 0x040011CE RID: 4558
		internal const int DBCOLUMNFLAGS_ISFIXEDLENGTH = 16;

		// Token: 0x040011CF RID: 4559
		internal const int DBCOLUMNFLAGS_ISNULLABLE = 32;

		// Token: 0x040011D0 RID: 4560
		internal const int DBCOLUMNFLAGS_ISROWSET = 1048576;

		// Token: 0x040011D1 RID: 4561
		internal const int DBCOLUMNFLAGS_ISROW = 2097152;

		// Token: 0x040011D2 RID: 4562
		internal const int DBCOLUMNFLAGS_ISROWSET_DBCOLUMNFLAGS_ISROW = 3145728;

		// Token: 0x040011D3 RID: 4563
		internal const int DBCOLUMNFLAGS_ISLONG_DBCOLUMNFLAGS_ISSTREAM = 524416;

		// Token: 0x040011D4 RID: 4564
		internal const int DBCOLUMNFLAGS_ISROWID_DBCOLUMNFLAGS_ISROWVER = 768;

		// Token: 0x040011D5 RID: 4565
		internal const int DBCOLUMNFLAGS_WRITE_DBCOLUMNFLAGS_WRITEUNKNOWN = 12;

		// Token: 0x040011D6 RID: 4566
		internal const int DBCOLUMNFLAGS_ISNULLABLE_DBCOLUMNFLAGS_MAYBENULL = 96;

		// Token: 0x040011D7 RID: 4567
		internal const int DBACCESSOR_ROWDATA = 2;

		// Token: 0x040011D8 RID: 4568
		internal const int DBACCESSOR_PARAMETERDATA = 4;

		// Token: 0x040011D9 RID: 4569
		internal const int DBPARAMTYPE_INPUT = 1;

		// Token: 0x040011DA RID: 4570
		internal const int DBPARAMTYPE_INPUTOUTPUT = 2;

		// Token: 0x040011DB RID: 4571
		internal const int DBPARAMTYPE_OUTPUT = 3;

		// Token: 0x040011DC RID: 4572
		internal const int DBPARAMTYPE_RETURNVALUE = 4;

		// Token: 0x040011DD RID: 4573
		internal const int ParameterDirectionFlag = 3;

		// Token: 0x040011DE RID: 4574
		internal const uint DB_UNSEARCHABLE = 1U;

		// Token: 0x040011DF RID: 4575
		internal const uint DB_LIKE_ONLY = 2U;

		// Token: 0x040011E0 RID: 4576
		internal const uint DB_ALL_EXCEPT_LIKE = 3U;

		// Token: 0x040011E1 RID: 4577
		internal const uint DB_SEARCHABLE = 4U;

		// Token: 0x040011E2 RID: 4578
		internal const string Asynchronous_Processing = "asynchronous processing";

		// Token: 0x040011E3 RID: 4579
		internal const string AttachDBFileName = "attachdbfilename";

		// Token: 0x040011E4 RID: 4580
		internal const string Connect_Timeout = "connect timeout";

		// Token: 0x040011E5 RID: 4581
		internal const string Data_Source = "data source";

		// Token: 0x040011E6 RID: 4582
		internal const string File_Name = "file name";

		// Token: 0x040011E7 RID: 4583
		internal const string Initial_Catalog = "initial catalog";

		// Token: 0x040011E8 RID: 4584
		internal const string Password = "password";

		// Token: 0x040011E9 RID: 4585
		internal const string Persist_Security_Info = "persist security info";

		// Token: 0x040011EA RID: 4586
		internal const string Provider = "provider";

		// Token: 0x040011EB RID: 4587
		internal const string Pwd = "pwd";

		// Token: 0x040011EC RID: 4588
		internal const string User_ID = "user id";

		// Token: 0x040011ED RID: 4589
		internal const string Current_Catalog = "current catalog";

		// Token: 0x040011EE RID: 4590
		internal const string DBMS_Version = "dbms version";

		// Token: 0x040011EF RID: 4591
		internal const string Properties = "Properties";

		// Token: 0x040011F0 RID: 4592
		internal const string DataLinks_CLSID = "CLSID\\{2206CDB2-19C1-11D1-89E0-00C04FD7A829}\\InprocServer32";

		// Token: 0x040011F1 RID: 4593
		internal const string OLEDB_SERVICES = "OLEDB_SERVICES";

		// Token: 0x040011F2 RID: 4594
		internal const string DefaultDescription_MSDASQL = "microsoft ole db provider for odbc drivers";

		// Token: 0x040011F3 RID: 4595
		internal const string MSDASQL = "msdasql";

		// Token: 0x040011F4 RID: 4596
		internal const string MSDASQLdot = "msdasql.";

		// Token: 0x040011F5 RID: 4597
		internal const string _Add = "add";

		// Token: 0x040011F6 RID: 4598
		internal const string _Keyword = "keyword";

		// Token: 0x040011F7 RID: 4599
		internal const string _Name = "name";

		// Token: 0x040011F8 RID: 4600
		internal const string _Value = "value";

		// Token: 0x040011F9 RID: 4601
		internal const string DBCOLUMN_BASECATALOGNAME = "DBCOLUMN_BASECATALOGNAME";

		// Token: 0x040011FA RID: 4602
		internal const string DBCOLUMN_BASECOLUMNNAME = "DBCOLUMN_BASECOLUMNNAME";

		// Token: 0x040011FB RID: 4603
		internal const string DBCOLUMN_BASESCHEMANAME = "DBCOLUMN_BASESCHEMANAME";

		// Token: 0x040011FC RID: 4604
		internal const string DBCOLUMN_BASETABLENAME = "DBCOLUMN_BASETABLENAME";

		// Token: 0x040011FD RID: 4605
		internal const string DBCOLUMN_COLUMNSIZE = "DBCOLUMN_COLUMNSIZE";

		// Token: 0x040011FE RID: 4606
		internal const string DBCOLUMN_FLAGS = "DBCOLUMN_FLAGS";

		// Token: 0x040011FF RID: 4607
		internal const string DBCOLUMN_GUID = "DBCOLUMN_GUID";

		// Token: 0x04001200 RID: 4608
		internal const string DBCOLUMN_IDNAME = "DBCOLUMN_IDNAME";

		// Token: 0x04001201 RID: 4609
		internal const string DBCOLUMN_ISAUTOINCREMENT = "DBCOLUMN_ISAUTOINCREMENT";

		// Token: 0x04001202 RID: 4610
		internal const string DBCOLUMN_ISUNIQUE = "DBCOLUMN_ISUNIQUE";

		// Token: 0x04001203 RID: 4611
		internal const string DBCOLUMN_KEYCOLUMN = "DBCOLUMN_KEYCOLUMN";

		// Token: 0x04001204 RID: 4612
		internal const string DBCOLUMN_NAME = "DBCOLUMN_NAME";

		// Token: 0x04001205 RID: 4613
		internal const string DBCOLUMN_NUMBER = "DBCOLUMN_NUMBER";

		// Token: 0x04001206 RID: 4614
		internal const string DBCOLUMN_PRECISION = "DBCOLUMN_PRECISION";

		// Token: 0x04001207 RID: 4615
		internal const string DBCOLUMN_PROPID = "DBCOLUMN_PROPID";

		// Token: 0x04001208 RID: 4616
		internal const string DBCOLUMN_SCALE = "DBCOLUMN_SCALE";

		// Token: 0x04001209 RID: 4617
		internal const string DBCOLUMN_TYPE = "DBCOLUMN_TYPE";

		// Token: 0x0400120A RID: 4618
		internal const string DBCOLUMN_TYPEINFO = "DBCOLUMN_TYPEINFO";

		// Token: 0x0400120B RID: 4619
		internal const string PRIMARY_KEY = "PRIMARY_KEY";

		// Token: 0x0400120C RID: 4620
		internal const string UNIQUE = "UNIQUE";

		// Token: 0x0400120D RID: 4621
		internal const string COLUMN_NAME = "COLUMN_NAME";

		// Token: 0x0400120E RID: 4622
		internal const string NULLS = "NULLS";

		// Token: 0x0400120F RID: 4623
		internal const string INDEX_NAME = "INDEX_NAME";

		// Token: 0x04001210 RID: 4624
		internal const string PARAMETER_NAME = "PARAMETER_NAME";

		// Token: 0x04001211 RID: 4625
		internal const string ORDINAL_POSITION = "ORDINAL_POSITION";

		// Token: 0x04001212 RID: 4626
		internal const string PARAMETER_TYPE = "PARAMETER_TYPE";

		// Token: 0x04001213 RID: 4627
		internal const string IS_NULLABLE = "IS_NULLABLE";

		// Token: 0x04001214 RID: 4628
		internal const string DATA_TYPE = "DATA_TYPE";

		// Token: 0x04001215 RID: 4629
		internal const string CHARACTER_MAXIMUM_LENGTH = "CHARACTER_MAXIMUM_LENGTH";

		// Token: 0x04001216 RID: 4630
		internal const string NUMERIC_PRECISION = "NUMERIC_PRECISION";

		// Token: 0x04001217 RID: 4631
		internal const string NUMERIC_SCALE = "NUMERIC_SCALE";

		// Token: 0x04001218 RID: 4632
		internal const string TYPE_NAME = "TYPE_NAME";

		// Token: 0x04001219 RID: 4633
		internal const string ORDINAL_POSITION_ASC = "ORDINAL_POSITION ASC";

		// Token: 0x0400121A RID: 4634
		internal const string SchemaGuids = "SchemaGuids";

		// Token: 0x0400121B RID: 4635
		internal const string Schema = "Schema";

		// Token: 0x0400121C RID: 4636
		internal const string RestrictionSupport = "RestrictionSupport";

		// Token: 0x0400121D RID: 4637
		internal const string DbInfoKeywords = "DbInfoKeywords";

		// Token: 0x0400121E RID: 4638
		internal const string Keyword = "Keyword";

		// Token: 0x0400121F RID: 4639
		internal static readonly IntPtr DBRESULTFLAG_DEFAULT = IntPtr.Zero;

		// Token: 0x04001220 RID: 4640
		internal static readonly IntPtr DB_INVALID_HACCESSOR = ADP.PtrZero;

		// Token: 0x04001221 RID: 4641
		internal static readonly IntPtr DB_NULL_HCHAPTER = ADP.PtrZero;

		// Token: 0x04001222 RID: 4642
		internal static readonly IntPtr DB_NULL_HROW = ADP.PtrZero;

		// Token: 0x04001223 RID: 4643
		internal static readonly int SizeOf_tagDBBINDING = Marshal.SizeOf(typeof(tagDBBINDING));

		// Token: 0x04001224 RID: 4644
		internal static readonly int SizeOf_tagDBCOLUMNINFO = Marshal.SizeOf(typeof(tagDBCOLUMNINFO));

		// Token: 0x04001225 RID: 4645
		internal static readonly int SizeOf_tagDBLITERALINFO = Marshal.SizeOf(typeof(tagDBLITERALINFO));

		// Token: 0x04001226 RID: 4646
		internal static readonly int SizeOf_tagDBPROPSET = Marshal.SizeOf(typeof(tagDBPROPSET));

		// Token: 0x04001227 RID: 4647
		internal static readonly int SizeOf_tagDBPROP = Marshal.SizeOf(typeof(tagDBPROP));

		// Token: 0x04001228 RID: 4648
		internal static readonly int SizeOf_tagDBPROPINFOSET = Marshal.SizeOf(typeof(tagDBPROPINFOSET));

		// Token: 0x04001229 RID: 4649
		internal static readonly int SizeOf_tagDBPROPINFO = Marshal.SizeOf(typeof(tagDBPROPINFO));

		// Token: 0x0400122A RID: 4650
		internal static readonly int SizeOf_tagDBPROPIDSET = Marshal.SizeOf(typeof(tagDBPROPIDSET));

		// Token: 0x0400122B RID: 4651
		internal static readonly int SizeOf_Guid = Marshal.SizeOf(typeof(Guid));

		// Token: 0x0400122C RID: 4652
		internal static readonly int SizeOf_Variant = 8 + 2 * ADP.PtrSize;

		// Token: 0x0400122D RID: 4653
		internal static readonly int OffsetOf_tagDBPROP_Status = Marshal.OffsetOf(typeof(tagDBPROP), "dwStatus").ToInt32();

		// Token: 0x0400122E RID: 4654
		internal static readonly int OffsetOf_tagDBPROP_Value = Marshal.OffsetOf(typeof(tagDBPROP), "vValue").ToInt32();

		// Token: 0x0400122F RID: 4655
		internal static readonly int OffsetOf_tagDBPROPSET_Properties = Marshal.OffsetOf(typeof(tagDBPROPSET), "rgProperties").ToInt32();

		// Token: 0x04001230 RID: 4656
		internal static readonly int OffsetOf_tagDBPROPINFO_Value = Marshal.OffsetOf(typeof(tagDBPROPINFO), "vValue").ToInt32();

		// Token: 0x04001231 RID: 4657
		internal static readonly int OffsetOf_tagDBPROPIDSET_PropertySet = Marshal.OffsetOf(typeof(tagDBPROPIDSET), "guidPropertySet").ToInt32();

		// Token: 0x04001232 RID: 4658
		internal static readonly int OffsetOf_tagDBLITERALINFO_it = Marshal.OffsetOf(typeof(tagDBLITERALINFO), "it").ToInt32();

		// Token: 0x04001233 RID: 4659
		internal static readonly int OffsetOf_tagDBBINDING_obValue = Marshal.OffsetOf(typeof(tagDBBINDING), "obValue").ToInt32();

		// Token: 0x04001234 RID: 4660
		internal static readonly int OffsetOf_tagDBBINDING_wType = Marshal.OffsetOf(typeof(tagDBBINDING), "wType").ToInt32();

		// Token: 0x04001235 RID: 4661
		internal static Guid IID_NULL = Guid.Empty;

		// Token: 0x04001236 RID: 4662
		internal static Guid IID_IUnknown = new Guid(0, 0, 0, 192, 0, 0, 0, 0, 0, 0, 70);

		// Token: 0x04001237 RID: 4663
		internal static Guid IID_IDBInitialize = new Guid(208878219, 10780, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001238 RID: 4664
		internal static Guid IID_IDBCreateSession = new Guid(208878173, 10780, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001239 RID: 4665
		internal static Guid IID_IDBCreateCommand = new Guid(208878109, 10780, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400123A RID: 4666
		internal static Guid IID_ICommandText = new Guid(208878119, 10780, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400123B RID: 4667
		internal static Guid IID_IMultipleResults = new Guid(208878224, 10780, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400123C RID: 4668
		internal static Guid IID_IRow = new Guid(208878260, 10780, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400123D RID: 4669
		internal static Guid IID_IRowset = new Guid(208878204, 10780, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400123E RID: 4670
		internal static Guid IID_ISQLErrorInfo = new Guid(208878196, 10780, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400123F RID: 4671
		internal static Guid CLSID_DataLinks = new Guid(570871218, 6593, 4561, 137, 224, 0, 192, 79, 215, 168, 41);

		// Token: 0x04001240 RID: 4672
		internal static Guid DBGUID_DEFAULT = new Guid(3367313915U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001241 RID: 4673
		internal static Guid DBGUID_ROWSET = new Guid(3367314166U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001242 RID: 4674
		internal static Guid DBGUID_ROW = new Guid(3367314167U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001243 RID: 4675
		internal static Guid DBGUID_ROWDEFAULTSTREAM = new Guid(208878263, 10780, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001244 RID: 4676
		internal static readonly Guid CLSID_MSDASQL = new Guid(3367314123U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001245 RID: 4677
		internal static readonly object DBCOL_SPECIALCOL = new Guid(3367313970U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001246 RID: 4678
		internal static readonly char[] ErrorTrimCharacters;
	}
}
