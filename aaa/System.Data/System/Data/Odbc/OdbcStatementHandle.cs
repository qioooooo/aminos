using System;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace System.Data.Odbc
{
	// Token: 0x02000206 RID: 518
	internal sealed class OdbcStatementHandle : OdbcHandle
	{
		// Token: 0x06001C96 RID: 7318 RVA: 0x0024D6E4 File Offset: 0x0024CAE4
		internal OdbcStatementHandle(OdbcConnectionHandle connectionHandle)
			: base(ODBC32.SQL_HANDLE.STMT, connectionHandle)
		{
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x0024D6FC File Offset: 0x0024CAFC
		internal ODBC32.RetCode BindColumn2(int columnNumber, ODBC32.SQL_C targetType, HandleRef buffer, IntPtr length, IntPtr srLen_or_Ind)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLBindCol(this, checked((ushort)columnNumber), targetType, buffer, length, srLen_or_Ind);
			ODBC.TraceODBC(3, "SQLBindCol", retCode);
			return retCode;
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x0024D728 File Offset: 0x0024CB28
		internal ODBC32.RetCode BindColumn3(int columnNumber, ODBC32.SQL_C targetType, IntPtr srLen_or_Ind)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLBindCol(this, checked((ushort)columnNumber), targetType, ADP.PtrZero, ADP.PtrZero, srLen_or_Ind);
			ODBC.TraceODBC(3, "SQLBindCol", retCode);
			return retCode;
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x0024D758 File Offset: 0x0024CB58
		internal ODBC32.RetCode BindParameter(short ordinal, short parameterDirection, ODBC32.SQL_C sqlctype, ODBC32.SQL_TYPE sqltype, IntPtr cchSize, IntPtr scale, HandleRef buffer, IntPtr bufferLength, HandleRef intbuffer)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLBindParameter(this, checked((ushort)ordinal), parameterDirection, sqlctype, (short)sqltype, cchSize, scale, buffer, bufferLength, intbuffer);
			ODBC.TraceODBC(3, "SQLBindParameter", retCode);
			return retCode;
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x0024D78C File Offset: 0x0024CB8C
		internal ODBC32.RetCode Cancel()
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLCancel(this);
			ODBC.TraceODBC(3, "SQLCancel", retCode);
			return retCode;
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x0024D7B0 File Offset: 0x0024CBB0
		internal ODBC32.RetCode CloseCursor()
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLCloseCursor(this);
			ODBC.TraceODBC(3, "SQLCloseCursor", retCode);
			return retCode;
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x0024D7D4 File Offset: 0x0024CBD4
		internal ODBC32.RetCode ColumnAttribute(int columnNumber, short fieldIdentifier, CNativeBuffer characterAttribute, out short stringLength, out SQLLEN numericAttribute)
		{
			IntPtr intPtr;
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLColAttributeW(this, checked((short)columnNumber), fieldIdentifier, characterAttribute, characterAttribute.ShortLength, out stringLength, out intPtr);
			numericAttribute = new SQLLEN(intPtr);
			ODBC.TraceODBC(3, "SQLColAttributeW", retCode);
			return retCode;
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x0024D810 File Offset: 0x0024CC10
		internal ODBC32.RetCode Columns(string tableCatalog, string tableSchema, string tableName, string columnName)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLColumnsW(this, tableCatalog, ODBC.ShortStringLength(tableCatalog), tableSchema, ODBC.ShortStringLength(tableSchema), tableName, ODBC.ShortStringLength(tableName), columnName, ODBC.ShortStringLength(columnName));
			ODBC.TraceODBC(3, "SQLColumnsW", retCode);
			return retCode;
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x0024D850 File Offset: 0x0024CC50
		internal ODBC32.RetCode Execute()
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLExecute(this);
			ODBC.TraceODBC(3, "SQLExecute", retCode);
			return retCode;
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x0024D874 File Offset: 0x0024CC74
		internal ODBC32.RetCode ExecuteDirect(string commandText)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLExecDirectW(this, commandText, -3);
			ODBC.TraceODBC(3, "SQLExecDirectW", retCode);
			return retCode;
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x0024D898 File Offset: 0x0024CC98
		internal ODBC32.RetCode Fetch()
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLFetch(this);
			ODBC.TraceODBC(3, "SQLFetch", retCode);
			return retCode;
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x0024D8BC File Offset: 0x0024CCBC
		internal ODBC32.RetCode FreeStatement(ODBC32.STMT stmt)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLFreeStmt(this, stmt);
			ODBC.TraceODBC(3, "SQLFreeStmt", retCode);
			return retCode;
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x0024D8E0 File Offset: 0x0024CCE0
		internal ODBC32.RetCode GetData(int index, ODBC32.SQL_C sqlctype, CNativeBuffer buffer, int cb, out IntPtr cbActual)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLGetData(this, checked((ushort)index), sqlctype, buffer, new IntPtr(cb), out cbActual);
			ODBC.TraceODBC(3, "SQLGetData", retCode);
			return retCode;
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x0024D910 File Offset: 0x0024CD10
		internal ODBC32.RetCode GetStatementAttribute(ODBC32.SQL_ATTR attribute, out IntPtr value, out int stringLength)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLGetStmtAttrW(this, attribute, out value, ADP.PtrSize, out stringLength);
			ODBC.TraceODBC(3, "SQLGetStmtAttrW", retCode);
			return retCode;
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x0024D93C File Offset: 0x0024CD3C
		internal ODBC32.RetCode GetTypeInfo(short fSqlType)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLGetTypeInfo(this, fSqlType);
			ODBC.TraceODBC(3, "SQLGetTypeInfo", retCode);
			return retCode;
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x0024D960 File Offset: 0x0024CD60
		internal ODBC32.RetCode MoreResults()
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLMoreResults(this);
			ODBC.TraceODBC(3, "SQLMoreResults", retCode);
			return retCode;
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x0024D984 File Offset: 0x0024CD84
		internal ODBC32.RetCode NumberOfResultColumns(out short columnsAffected)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLNumResultCols(this, out columnsAffected);
			ODBC.TraceODBC(3, "SQLNumResultCols", retCode);
			return retCode;
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x0024D9A8 File Offset: 0x0024CDA8
		internal ODBC32.RetCode Prepare(string commandText)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLPrepareW(this, commandText, -3);
			ODBC.TraceODBC(3, "SQLPrepareW", retCode);
			return retCode;
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x0024D9CC File Offset: 0x0024CDCC
		internal ODBC32.RetCode PrimaryKeys(string catalogName, string schemaName, string tableName)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLPrimaryKeysW(this, catalogName, ODBC.ShortStringLength(catalogName), schemaName, ODBC.ShortStringLength(schemaName), tableName, ODBC.ShortStringLength(tableName));
			ODBC.TraceODBC(3, "SQLPrimaryKeysW", retCode);
			return retCode;
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x0024DA04 File Offset: 0x0024CE04
		internal ODBC32.RetCode Procedures(string procedureCatalog, string procedureSchema, string procedureName)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLProceduresW(this, procedureCatalog, ODBC.ShortStringLength(procedureCatalog), procedureSchema, ODBC.ShortStringLength(procedureSchema), procedureName, ODBC.ShortStringLength(procedureName));
			ODBC.TraceODBC(3, "SQLProceduresW", retCode);
			return retCode;
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x0024DA3C File Offset: 0x0024CE3C
		internal ODBC32.RetCode ProcedureColumns(string procedureCatalog, string procedureSchema, string procedureName, string columnName)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLProcedureColumnsW(this, procedureCatalog, ODBC.ShortStringLength(procedureCatalog), procedureSchema, ODBC.ShortStringLength(procedureSchema), procedureName, ODBC.ShortStringLength(procedureName), columnName, ODBC.ShortStringLength(columnName));
			ODBC.TraceODBC(3, "SQLProcedureColumnsW", retCode);
			return retCode;
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x0024DA7C File Offset: 0x0024CE7C
		internal ODBC32.RetCode RowCount(out SQLLEN rowCount)
		{
			IntPtr intPtr;
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLRowCount(this, out intPtr);
			rowCount._value = new SQLLEN(intPtr);
			ODBC.TraceODBC(3, "SQLRowCount", retCode);
			return retCode;
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x0024DAB0 File Offset: 0x0024CEB0
		internal ODBC32.RetCode SetStatementAttribute(ODBC32.SQL_ATTR attribute, IntPtr value, ODBC32.SQL_IS stringLength)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLSetStmtAttrW(this, (int)attribute, value, (int)stringLength);
			ODBC.TraceODBC(3, "SQLSetStmtAttrW", retCode);
			return retCode;
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x0024DAD4 File Offset: 0x0024CED4
		internal ODBC32.RetCode SpecialColumns(string quotedTable)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLSpecialColumnsW(this, ODBC32.SQL_SPECIALCOLS.ROWVER, null, 0, null, 0, quotedTable, ODBC.ShortStringLength(quotedTable), ODBC32.SQL_SCOPE.SESSION, ODBC32.SQL_NULLABILITY.NO_NULLS);
			ODBC.TraceODBC(3, "SQLSpecialColumnsW", retCode);
			return retCode;
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x0024DB04 File Offset: 0x0024CF04
		internal ODBC32.RetCode Statistics(string tableCatalog, string tableSchema, string tableName, short unique, short accuracy)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLStatisticsW(this, tableCatalog, ODBC.ShortStringLength(tableCatalog), tableSchema, ODBC.ShortStringLength(tableSchema), tableName, ODBC.ShortStringLength(tableName), unique, accuracy);
			ODBC.TraceODBC(3, "SQLStatisticsW", retCode);
			return retCode;
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x0024DB40 File Offset: 0x0024CF40
		internal ODBC32.RetCode Statistics(string tableName)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLStatisticsW(this, null, 0, null, 0, tableName, ODBC.ShortStringLength(tableName), 0, 1);
			ODBC.TraceODBC(3, "SQLStatisticsW", retCode);
			return retCode;
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x0024DB70 File Offset: 0x0024CF70
		internal ODBC32.RetCode Tables(string tableCatalog, string tableSchema, string tableName, string tableType)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLTablesW(this, tableCatalog, ODBC.ShortStringLength(tableCatalog), tableSchema, ODBC.ShortStringLength(tableSchema), tableName, ODBC.ShortStringLength(tableName), tableType, ODBC.ShortStringLength(tableType));
			ODBC.TraceODBC(3, "SQLTablesW", retCode);
			return retCode;
		}
	}
}
