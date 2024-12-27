using System;

namespace System.Data.Design
{
	// Token: 0x02000068 RID: 104
	internal class ConnectionString
	{
		// Token: 0x06000496 RID: 1174 RVA: 0x00003CB2 File Offset: 0x00002CB2
		public ConnectionString(string providerName, string connectionString)
		{
			this.connectionString = connectionString;
			this.providerName = providerName;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00003CC8 File Offset: 0x00002CC8
		public string ToFullString()
		{
			return this.connectionString.ToString();
		}

		// Token: 0x04000A90 RID: 2704
		private string providerName;

		// Token: 0x04000A91 RID: 2705
		private string connectionString;
	}
}
