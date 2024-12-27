using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000315 RID: 789
	internal sealed class SQLMessage
	{
		// Token: 0x060029B4 RID: 10676 RVA: 0x00293C14 File Offset: 0x00293014
		private SQLMessage()
		{
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x00293C28 File Offset: 0x00293028
		internal static string CultureIdError()
		{
			return Res.GetString("SQL_CultureIdError");
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x00293C40 File Offset: 0x00293040
		internal static string EncryptionNotSupportedByClient()
		{
			return Res.GetString("SQL_EncryptionNotSupportedByClient");
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x00293C58 File Offset: 0x00293058
		internal static string EncryptionNotSupportedByServer()
		{
			return Res.GetString("SQL_EncryptionNotSupportedByServer");
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x00293C70 File Offset: 0x00293070
		internal static string OperationCancelled()
		{
			return Res.GetString("SQL_OperationCancelled");
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x00293C88 File Offset: 0x00293088
		internal static string SevereError()
		{
			return Res.GetString("SQL_SevereError");
		}

		// Token: 0x060029BA RID: 10682 RVA: 0x00293CA0 File Offset: 0x002930A0
		internal static string SSPIInitializeError()
		{
			return Res.GetString("SQL_SSPIInitializeError");
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x00293CB8 File Offset: 0x002930B8
		internal static string SSPIGenerateError()
		{
			return Res.GetString("SQL_SSPIGenerateError");
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x00293CD0 File Offset: 0x002930D0
		internal static string Timeout()
		{
			return Res.GetString("SQL_Timeout");
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x00293CE8 File Offset: 0x002930E8
		internal static string UserInstanceFailure()
		{
			return Res.GetString("SQL_UserInstanceFailure");
		}
	}
}
