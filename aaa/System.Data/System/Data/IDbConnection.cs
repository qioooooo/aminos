using System;

namespace System.Data
{
	// Token: 0x020000BD RID: 189
	public interface IDbConnection : IDisposable
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000C98 RID: 3224
		// (set) Token: 0x06000C99 RID: 3225
		string ConnectionString { get; set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000C9A RID: 3226
		int ConnectionTimeout { get; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000C9B RID: 3227
		string Database { get; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000C9C RID: 3228
		ConnectionState State { get; }

		// Token: 0x06000C9D RID: 3229
		IDbTransaction BeginTransaction();

		// Token: 0x06000C9E RID: 3230
		IDbTransaction BeginTransaction(IsolationLevel il);

		// Token: 0x06000C9F RID: 3231
		void Close();

		// Token: 0x06000CA0 RID: 3232
		void ChangeDatabase(string databaseName);

		// Token: 0x06000CA1 RID: 3233
		IDbCommand CreateCommand();

		// Token: 0x06000CA2 RID: 3234
		void Open();
	}
}
