using System;

namespace System.Data
{
	// Token: 0x020000BC RID: 188
	public interface IDbCommand : IDisposable
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000C84 RID: 3204
		// (set) Token: 0x06000C85 RID: 3205
		IDbConnection Connection { get; set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000C86 RID: 3206
		// (set) Token: 0x06000C87 RID: 3207
		IDbTransaction Transaction { get; set; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000C88 RID: 3208
		// (set) Token: 0x06000C89 RID: 3209
		string CommandText { get; set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000C8A RID: 3210
		// (set) Token: 0x06000C8B RID: 3211
		int CommandTimeout { get; set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000C8C RID: 3212
		// (set) Token: 0x06000C8D RID: 3213
		CommandType CommandType { get; set; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000C8E RID: 3214
		IDataParameterCollection Parameters { get; }

		// Token: 0x06000C8F RID: 3215
		void Prepare();

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000C90 RID: 3216
		// (set) Token: 0x06000C91 RID: 3217
		UpdateRowSource UpdatedRowSource { get; set; }

		// Token: 0x06000C92 RID: 3218
		void Cancel();

		// Token: 0x06000C93 RID: 3219
		IDbDataParameter CreateParameter();

		// Token: 0x06000C94 RID: 3220
		int ExecuteNonQuery();

		// Token: 0x06000C95 RID: 3221
		IDataReader ExecuteReader();

		// Token: 0x06000C96 RID: 3222
		IDataReader ExecuteReader(CommandBehavior behavior);

		// Token: 0x06000C97 RID: 3223
		object ExecuteScalar();
	}
}
