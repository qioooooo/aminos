using System;
using System.Data.Common;
using System.Threading;

namespace System.Data.Odbc
{
	// Token: 0x020001E9 RID: 489
	internal sealed class OdbcEnvironment
	{
		// Token: 0x06001B7C RID: 7036 RVA: 0x00248114 File Offset: 0x00247514
		private OdbcEnvironment()
		{
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x00248128 File Offset: 0x00247528
		internal static OdbcEnvironmentHandle GetGlobalEnvironmentHandle()
		{
			OdbcEnvironmentHandle odbcEnvironmentHandle = OdbcEnvironment._globalEnvironmentHandle as OdbcEnvironmentHandle;
			if (odbcEnvironmentHandle == null)
			{
				ADP.CheckVersionMDAC(true);
				lock (OdbcEnvironment._globalEnvironmentHandleLock)
				{
					odbcEnvironmentHandle = OdbcEnvironment._globalEnvironmentHandle as OdbcEnvironmentHandle;
					if (odbcEnvironmentHandle == null)
					{
						odbcEnvironmentHandle = new OdbcEnvironmentHandle();
						OdbcEnvironment._globalEnvironmentHandle = odbcEnvironmentHandle;
					}
				}
			}
			return odbcEnvironmentHandle;
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x00248198 File Offset: 0x00247598
		internal static void ReleaseObjectPool()
		{
			object obj = Interlocked.Exchange(ref OdbcEnvironment._globalEnvironmentHandle, null);
			if (obj != null)
			{
				(obj as OdbcEnvironmentHandle).Dispose();
			}
		}

		// Token: 0x04001005 RID: 4101
		private static object _globalEnvironmentHandle;

		// Token: 0x04001006 RID: 4102
		private static object _globalEnvironmentHandleLock = new object();
	}
}
