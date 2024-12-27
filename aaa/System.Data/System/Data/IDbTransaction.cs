using System;

namespace System.Data
{
	// Token: 0x020000C0 RID: 192
	public interface IDbTransaction : IDisposable
	{
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000CB1 RID: 3249
		IDbConnection Connection { get; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000CB2 RID: 3250
		IsolationLevel IsolationLevel { get; }

		// Token: 0x06000CB3 RID: 3251
		void Commit();

		// Token: 0x06000CB4 RID: 3252
		void Rollback();
	}
}
