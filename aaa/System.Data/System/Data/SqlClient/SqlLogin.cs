using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000326 RID: 806
	internal sealed class SqlLogin
	{
		// Token: 0x04001B9A RID: 7066
		internal int timeout;

		// Token: 0x04001B9B RID: 7067
		internal bool userInstance;

		// Token: 0x04001B9C RID: 7068
		internal string hostName = "";

		// Token: 0x04001B9D RID: 7069
		internal string userName = "";

		// Token: 0x04001B9E RID: 7070
		internal string password = "";

		// Token: 0x04001B9F RID: 7071
		internal string applicationName = "";

		// Token: 0x04001BA0 RID: 7072
		internal string serverName = "";

		// Token: 0x04001BA1 RID: 7073
		internal string language = "";

		// Token: 0x04001BA2 RID: 7074
		internal string database = "";

		// Token: 0x04001BA3 RID: 7075
		internal string attachDBFilename = "";

		// Token: 0x04001BA4 RID: 7076
		internal string newPassword = "";

		// Token: 0x04001BA5 RID: 7077
		internal bool useReplication;

		// Token: 0x04001BA6 RID: 7078
		internal bool useSSPI;

		// Token: 0x04001BA7 RID: 7079
		internal int packetSize = 8000;

		// Token: 0x04001BA8 RID: 7080
		internal bool readOnlyIntent;
	}
}
