using System;

namespace System.Data.Common
{
	// Token: 0x02000147 RID: 327
	public abstract class DbTransaction : MarshalByRefObject, IDbTransaction, IDisposable
	{
		// Token: 0x17000312 RID: 786
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x00229700 File Offset: 0x00228B00
		public DbConnection Connection
		{
			get
			{
				return this.DbConnection;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x0600152D RID: 5421 RVA: 0x00229714 File Offset: 0x00228B14
		IDbConnection IDbTransaction.Connection
		{
			get
			{
				return this.DbConnection;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x0600152E RID: 5422
		protected abstract DbConnection DbConnection { get; }

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x0600152F RID: 5423
		public abstract IsolationLevel IsolationLevel { get; }

		// Token: 0x06001530 RID: 5424
		public abstract void Commit();

		// Token: 0x06001531 RID: 5425 RVA: 0x00229728 File Offset: 0x00228B28
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x0022973C File Offset: 0x00228B3C
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x06001533 RID: 5427
		public abstract void Rollback();
	}
}
