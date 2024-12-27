using System;
using System.Data.Common;
using System.Globalization;

namespace System.Data.Odbc
{
	// Token: 0x020001BC RID: 444
	internal static class ODBC
	{
		// Token: 0x0600195C RID: 6492 RVA: 0x0023E964 File Offset: 0x0023DD64
		internal static Exception UnknownSQLType(ODBC32.SQL_TYPE sqltype)
		{
			return ADP.Argument(Res.GetString("Odbc_UnknownSQLType", new object[] { sqltype.ToString() }));
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x0023E998 File Offset: 0x0023DD98
		internal static Exception ConnectionStringTooLong()
		{
			return ADP.Argument(Res.GetString("OdbcConnection_ConnectionStringTooLong", new object[] { 1024 }));
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0023E9CC File Offset: 0x0023DDCC
		internal static ArgumentException GetSchemaRestrictionRequired()
		{
			return ADP.Argument(Res.GetString("ODBC_GetSchemaRestrictionRequired"));
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x0023E9E8 File Offset: 0x0023DDE8
		internal static ArgumentOutOfRangeException NotSupportedEnumerationValue(Type type, int value)
		{
			return ADP.ArgumentOutOfRange(Res.GetString("ODBC_NotSupportedEnumerationValue", new object[]
			{
				type.Name,
				value.ToString(CultureInfo.InvariantCulture)
			}), type.Name);
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0023EA2C File Offset: 0x0023DE2C
		internal static ArgumentOutOfRangeException NotSupportedCommandType(CommandType value)
		{
			return ODBC.NotSupportedEnumerationValue(typeof(CommandType), (int)value);
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x0023EA4C File Offset: 0x0023DE4C
		internal static ArgumentOutOfRangeException NotSupportedIsolationLevel(IsolationLevel value)
		{
			return ODBC.NotSupportedEnumerationValue(typeof(IsolationLevel), (int)value);
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x0023EA6C File Offset: 0x0023DE6C
		internal static InvalidOperationException NoMappingForSqlTransactionLevel(int value)
		{
			return ADP.DataAdapter(Res.GetString("Odbc_NoMappingForSqlTransactionLevel", new object[] { value.ToString(CultureInfo.InvariantCulture) }));
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0023EAA0 File Offset: 0x0023DEA0
		internal static Exception NegativeArgument()
		{
			return ADP.Argument(Res.GetString("Odbc_NegativeArgument"));
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0023EABC File Offset: 0x0023DEBC
		internal static Exception CantSetPropertyOnOpenConnection()
		{
			return ADP.InvalidOperation(Res.GetString("Odbc_CantSetPropertyOnOpenConnection"));
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0023EAD8 File Offset: 0x0023DED8
		internal static Exception CantEnableConnectionpooling(ODBC32.RetCode retcode)
		{
			return ADP.DataAdapter(Res.GetString("Odbc_CantEnableConnectionpooling", new object[] { ODBC32.RetcodeToString(retcode) }));
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0023EB08 File Offset: 0x0023DF08
		internal static Exception CantAllocateEnvironmentHandle(ODBC32.RetCode retcode)
		{
			return ADP.DataAdapter(Res.GetString("Odbc_CantAllocateEnvironmentHandle", new object[] { ODBC32.RetcodeToString(retcode) }));
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0023EB38 File Offset: 0x0023DF38
		internal static Exception FailedToGetDescriptorHandle(ODBC32.RetCode retcode)
		{
			return ADP.DataAdapter(Res.GetString("Odbc_FailedToGetDescriptorHandle", new object[] { ODBC32.RetcodeToString(retcode) }));
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x0023EB68 File Offset: 0x0023DF68
		internal static Exception NotInTransaction()
		{
			return ADP.InvalidOperation(Res.GetString("Odbc_NotInTransaction"));
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x0023EB84 File Offset: 0x0023DF84
		internal static Exception UnknownOdbcType(OdbcType odbctype)
		{
			return ADP.InvalidEnumerationValue(typeof(OdbcType), (int)odbctype);
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x0023EBA4 File Offset: 0x0023DFA4
		internal static void TraceODBC(int level, string method, ODBC32.RetCode retcode)
		{
			Bid.TraceSqlReturn("<odbc|API|ODBC|RET> %08X{SQLRETURN}, method=%ls\n", retcode, method);
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x0023EBC0 File Offset: 0x0023DFC0
		internal static void TraceODBC(int level, string method, string param, ODBC32.RetCode retcode)
		{
			Bid.TraceSqlReturn("<odbc|API|ODBC|RET> %08X{SQLRETURN}, method=%ls, param=%ls\n", retcode, method, param);
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x0023EBDC File Offset: 0x0023DFDC
		internal static short ShortStringLength(string inputString)
		{
			return checked((short)ADP.StringLength(inputString));
		}

		// Token: 0x04000E3A RID: 3642
		internal const string Pwd = "pwd";
	}
}
