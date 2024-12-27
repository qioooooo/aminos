using System;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace System.Data.Odbc
{
	// Token: 0x020001EF RID: 495
	internal sealed class OdbcDescriptorHandle : OdbcHandle
	{
		// Token: 0x06001BA1 RID: 7073 RVA: 0x00248710 File Offset: 0x00247B10
		internal OdbcDescriptorHandle(OdbcStatementHandle statementHandle, ODBC32.SQL_ATTR attribute)
			: base(statementHandle, attribute)
		{
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x00248728 File Offset: 0x00247B28
		internal ODBC32.RetCode GetDescriptionField(int i, ODBC32.SQL_DESC attribute, CNativeBuffer buffer, out int numericAttribute)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLGetDescFieldW(this, checked((short)i), attribute, buffer, (int)buffer.ShortLength, out numericAttribute);
			ODBC.TraceODBC(3, "SQLGetDescFieldW", retCode);
			return retCode;
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x00248758 File Offset: 0x00247B58
		internal ODBC32.RetCode SetDescriptionField1(short ordinal, ODBC32.SQL_DESC type, IntPtr value)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLSetDescFieldW(this, ordinal, type, value, 0);
			ODBC.TraceODBC(3, "SQLSetDescFieldW", retCode);
			return retCode;
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x00248780 File Offset: 0x00247B80
		internal ODBC32.RetCode SetDescriptionField2(short ordinal, ODBC32.SQL_DESC type, HandleRef value)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLSetDescFieldW(this, ordinal, type, value, 0);
			ODBC.TraceODBC(3, "SQLSetDescFieldW", retCode);
			return retCode;
		}
	}
}
