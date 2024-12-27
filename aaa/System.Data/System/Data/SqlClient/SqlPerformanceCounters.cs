using System;
using System.Data.ProviderBase;
using System.Diagnostics;
using System.Security.Permissions;

namespace System.Data.SqlClient
{
	// Token: 0x020002D1 RID: 721
	internal sealed class SqlPerformanceCounters : DbConnectionPoolCounters
	{
		// Token: 0x06002509 RID: 9481 RVA: 0x0027A3B4 File Offset: 0x002797B4
		[PerformanceCounterPermission(SecurityAction.Assert, PermissionAccess = PerformanceCounterPermissionAccess.Write, MachineName = ".", CategoryName = ".NET Data Provider for SqlServer")]
		private SqlPerformanceCounters()
			: base(".NET Data Provider for SqlServer", "Counters for System.Data.SqlClient")
		{
		}

		// Token: 0x04001782 RID: 6018
		private const string CategoryName = ".NET Data Provider for SqlServer";

		// Token: 0x04001783 RID: 6019
		private const string CategoryHelp = "Counters for System.Data.SqlClient";

		// Token: 0x04001784 RID: 6020
		public static readonly SqlPerformanceCounters SingletonInstance = new SqlPerformanceCounters();
	}
}
