using System;
using System.Data.Common;

namespace System.Data.Odbc
{
	// Token: 0x020001EA RID: 490
	internal sealed class OdbcEnvironmentHandle : OdbcHandle
	{
		// Token: 0x06001B80 RID: 7040 RVA: 0x002481D8 File Offset: 0x002475D8
		internal OdbcEnvironmentHandle()
			: base(ODBC32.SQL_HANDLE.ENV, null)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLSetEnvAttr(this, ODBC32.SQL_ATTR.ODBC_VERSION, ODBC32.SQL_OV_ODBC3, ODBC32.SQL_IS.INTEGER);
			retCode = UnsafeNativeMethods.SQLSetEnvAttr(this, ODBC32.SQL_ATTR.CONNECTION_POOLING, ODBC32.SQL_CP_ONE_PER_HENV, ODBC32.SQL_IS.INTEGER);
			switch (retCode)
			{
			case ODBC32.RetCode.SUCCESS:
			case ODBC32.RetCode.SUCCESS_WITH_INFO:
				return;
			default:
				base.Dispose();
				throw ODBC.CantEnableConnectionpooling(retCode);
			}
		}
	}
}
