using System;
using System.Data.Common;

namespace System.Data.Sql
{
	// Token: 0x02000285 RID: 645
	internal sealed class SqlGenericUtil
	{
		// Token: 0x060021C1 RID: 8641 RVA: 0x00269D8C File Offset: 0x0026918C
		private SqlGenericUtil()
		{
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x00269DA0 File Offset: 0x002691A0
		internal static Exception NullCommandText()
		{
			return ADP.Argument(Res.GetString("Sql_NullCommandText"));
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x00269DBC File Offset: 0x002691BC
		internal static Exception MismatchedMetaDataDirectionArrayLengths()
		{
			return ADP.Argument(Res.GetString("Sql_MismatchedMetaDataDirectionArrayLengths"));
		}
	}
}
