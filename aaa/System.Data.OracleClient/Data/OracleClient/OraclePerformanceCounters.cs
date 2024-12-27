using System;
using System.Data.ProviderBase;
using System.Diagnostics;
using System.Security.Permissions;

namespace System.Data.OracleClient
{
	// Token: 0x0200005A RID: 90
	internal sealed class OraclePerformanceCounters : DbConnectionPoolCounters
	{
		// Token: 0x0600039B RID: 923 RVA: 0x00062744 File Offset: 0x00061B44
		[PerformanceCounterPermission(SecurityAction.Assert, PermissionAccess = PerformanceCounterPermissionAccess.Write, MachineName = ".", CategoryName = ".NET Data Provider for Oracle")]
		private OraclePerformanceCounters()
			: base(".NET Data Provider for Oracle", "Counters for System.Data.OracleClient")
		{
		}

		// Token: 0x040003BB RID: 955
		private const string CategoryName = ".NET Data Provider for Oracle";

		// Token: 0x040003BC RID: 956
		private const string CategoryHelp = "Counters for System.Data.OracleClient";

		// Token: 0x040003BD RID: 957
		public static readonly OraclePerformanceCounters SingletonInstance = new OraclePerformanceCounters();
	}
}
