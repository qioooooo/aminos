using System;

namespace System.Data.SqlClient
{
	// Token: 0x0200032C RID: 812
	internal sealed class _SqlRPC
	{
		// Token: 0x04001BDA RID: 7130
		internal string rpcName;

		// Token: 0x04001BDB RID: 7131
		internal string databaseName;

		// Token: 0x04001BDC RID: 7132
		internal ushort ProcID;

		// Token: 0x04001BDD RID: 7133
		internal ushort options;

		// Token: 0x04001BDE RID: 7134
		internal SqlParameter[] parameters;

		// Token: 0x04001BDF RID: 7135
		internal byte[] paramoptions;

		// Token: 0x04001BE0 RID: 7136
		internal int? recordsAffected;

		// Token: 0x04001BE1 RID: 7137
		internal int cumulativeRecordsAffected;

		// Token: 0x04001BE2 RID: 7138
		internal int errorsIndexStart;

		// Token: 0x04001BE3 RID: 7139
		internal int errorsIndexEnd;

		// Token: 0x04001BE4 RID: 7140
		internal SqlErrorCollection errors;

		// Token: 0x04001BE5 RID: 7141
		internal int warningsIndexStart;

		// Token: 0x04001BE6 RID: 7142
		internal int warningsIndexEnd;

		// Token: 0x04001BE7 RID: 7143
		internal SqlErrorCollection warnings;
	}
}
