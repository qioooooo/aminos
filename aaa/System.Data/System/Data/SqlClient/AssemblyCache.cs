using System;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x020002A2 RID: 674
	internal sealed class AssemblyCache
	{
		// Token: 0x060022B2 RID: 8882 RVA: 0x0026E034 File Offset: 0x0026D434
		private AssemblyCache()
		{
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x0026E048 File Offset: 0x0026D448
		internal static int GetLength(object inst)
		{
			return SerializationHelperSql9.SizeInBytes(inst);
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x0026E05C File Offset: 0x0026D45C
		internal static SqlUdtInfo GetInfoFromType(Type t)
		{
			Type type = t;
			SqlUdtInfo sqlUdtInfo;
			for (;;)
			{
				sqlUdtInfo = SqlUdtInfo.TryGetFromType(t);
				if (sqlUdtInfo != null)
				{
					break;
				}
				t = t.BaseType;
				if (t == null)
				{
					goto Block_2;
				}
			}
			return sqlUdtInfo;
			Block_2:
			throw SQL.UDTInvalidSqlType(type.AssemblyQualifiedName);
		}
	}
}
