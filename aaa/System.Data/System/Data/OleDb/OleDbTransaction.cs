using System;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Data.OleDb
{
	// Token: 0x0200024E RID: 590
	public sealed class OleDbTransaction : DbTransaction
	{
		// Token: 0x06002075 RID: 8309 RVA: 0x00262BEC File Offset: 0x00261FEC
		internal OleDbTransaction(OleDbConnection connection, OleDbTransaction transaction, IsolationLevel isolevel)
		{
			this._parentConnection = connection;
			this._parentTransaction = transaction;
			IsolationLevel isolationLevel = isolevel;
			if (isolationLevel <= IsolationLevel.ReadUncommitted)
			{
				if (isolationLevel == IsolationLevel.Unspecified)
				{
					isolevel = IsolationLevel.ReadCommitted;
					goto IL_007D;
				}
				if (isolationLevel == IsolationLevel.Chaos || isolationLevel == IsolationLevel.ReadUncommitted)
				{
					goto IL_007D;
				}
			}
			else if (isolationLevel <= IsolationLevel.RepeatableRead)
			{
				if (isolationLevel == IsolationLevel.ReadCommitted || isolationLevel == IsolationLevel.RepeatableRead)
				{
					goto IL_007D;
				}
			}
			else if (isolationLevel == IsolationLevel.Serializable || isolationLevel == IsolationLevel.Snapshot)
			{
				goto IL_007D;
			}
			throw ADP.InvalidIsolationLevel(isolevel);
			IL_007D:
			this._isolationLevel = isolevel;
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06002076 RID: 8310 RVA: 0x00262C80 File Offset: 0x00262080
		public new OleDbConnection Connection
		{
			get
			{
				return this._parentConnection;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06002077 RID: 8311 RVA: 0x00262C94 File Offset: 0x00262094
		protected override DbConnection DbConnection
		{
			get
			{
				return this.Connection;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06002078 RID: 8312 RVA: 0x00262CA8 File Offset: 0x002620A8
		public override IsolationLevel IsolationLevel
		{
			get
			{
				if (this._transaction == null)
				{
					throw ADP.TransactionZombied(this);
				}
				return this._isolationLevel;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06002079 RID: 8313 RVA: 0x00262CCC File Offset: 0x002620CC
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x0600207A RID: 8314 RVA: 0x00262CE0 File Offset: 0x002620E0
		internal OleDbTransaction Parent
		{
			get
			{
				return this._parentTransaction;
			}
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x00262CF4 File Offset: 0x002620F4
		public OleDbTransaction Begin(IsolationLevel isolevel)
		{
			OleDbConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbTransaction.Begin|API> %d#, isolevel=%d{IsolationLevel}", this.ObjectID, (int)isolevel);
			OleDbTransaction oleDbTransaction2;
			try
			{
				if (this._transaction == null)
				{
					throw ADP.TransactionZombied(this);
				}
				if (this._nestedTransaction != null && this._nestedTransaction.IsAlive)
				{
					throw ADP.ParallelTransactionsNotSupported(this.Connection);
				}
				OleDbTransaction oleDbTransaction = new OleDbTransaction(this._parentConnection, this, isolevel);
				this._nestedTransaction = new WeakReference(oleDbTransaction, false);
				UnsafeNativeMethods.ITransactionLocal transactionLocal = null;
				try
				{
					transactionLocal = (UnsafeNativeMethods.ITransactionLocal)this._transaction.ComWrapper();
					oleDbTransaction.BeginInternal(transactionLocal);
				}
				finally
				{
					if (transactionLocal != null)
					{
						Marshal.ReleaseComObject(transactionLocal);
					}
				}
				oleDbTransaction2 = oleDbTransaction;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return oleDbTransaction2;
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x00262DD4 File Offset: 0x002621D4
		public OleDbTransaction Begin()
		{
			return this.Begin(IsolationLevel.ReadCommitted);
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x00262DEC File Offset: 0x002621EC
		internal void BeginInternal(UnsafeNativeMethods.ITransactionLocal transaction)
		{
			OleDbHResult oleDbHResult;
			this._transaction = new OleDbTransaction.WrappedTransaction(transaction, (int)this._isolationLevel, out oleDbHResult);
			if (oleDbHResult < OleDbHResult.S_OK)
			{
				this._transaction.Dispose();
				this._transaction = null;
				this.ProcessResults(oleDbHResult);
			}
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x00262E2C File Offset: 0x0026222C
		public override void Commit()
		{
			OleDbConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbTransaction.Commit|API> %d#", this.ObjectID);
			try
			{
				if (this._transaction == null)
				{
					throw ADP.TransactionZombied(this);
				}
				this.CommitInternal();
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x00262E90 File Offset: 0x00262290
		private void CommitInternal()
		{
			if (this._transaction == null)
			{
				return;
			}
			if (this._nestedTransaction != null)
			{
				OleDbTransaction oleDbTransaction = (OleDbTransaction)this._nestedTransaction.Target;
				if (oleDbTransaction != null && this._nestedTransaction.IsAlive)
				{
					oleDbTransaction.CommitInternal();
				}
				this._nestedTransaction = null;
			}
			OleDbHResult oleDbHResult = this._transaction.Commit();
			if (!this._transaction.MustComplete)
			{
				this._transaction.Dispose();
				this._transaction = null;
				this.DisposeManaged();
			}
			if (oleDbHResult < OleDbHResult.S_OK)
			{
				this.ProcessResults(oleDbHResult);
			}
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x00262F18 File Offset: 0x00262318
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.DisposeManaged();
				this.RollbackInternal(false);
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x00262F40 File Offset: 0x00262340
		private void DisposeManaged()
		{
			if (this._parentTransaction != null)
			{
				this._parentTransaction._nestedTransaction = null;
			}
			else if (this._parentConnection != null)
			{
				this._parentConnection.LocalTransaction = null;
			}
			this._parentConnection = null;
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x00262F80 File Offset: 0x00262380
		private void ProcessResults(OleDbHResult hr)
		{
			Exception ex = OleDbConnection.ProcessResults(hr, this._parentConnection, this);
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x00262FA0 File Offset: 0x002623A0
		public override void Rollback()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbTransaction.Rollback|API> %d#", this.ObjectID);
			try
			{
				if (this._transaction == null)
				{
					throw ADP.TransactionZombied(this);
				}
				this.DisposeManaged();
				this.RollbackInternal(true);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x00263004 File Offset: 0x00262404
		internal OleDbHResult RollbackInternal(bool exceptionHandling)
		{
			OleDbHResult oleDbHResult = OleDbHResult.S_OK;
			if (this._transaction != null)
			{
				if (this._nestedTransaction != null)
				{
					OleDbTransaction oleDbTransaction = (OleDbTransaction)this._nestedTransaction.Target;
					if (oleDbTransaction != null && this._nestedTransaction.IsAlive)
					{
						oleDbHResult = oleDbTransaction.RollbackInternal(exceptionHandling);
						if (exceptionHandling && oleDbHResult < OleDbHResult.S_OK)
						{
							SafeNativeMethods.Wrapper.ClearErrorInfo();
							return oleDbHResult;
						}
					}
					this._nestedTransaction = null;
				}
				oleDbHResult = this._transaction.Abort();
				this._transaction.Dispose();
				this._transaction = null;
				if (oleDbHResult < OleDbHResult.S_OK)
				{
					if (exceptionHandling)
					{
						this.ProcessResults(oleDbHResult);
					}
					else
					{
						SafeNativeMethods.Wrapper.ClearErrorInfo();
					}
				}
			}
			return oleDbHResult;
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x00263098 File Offset: 0x00262498
		internal static OleDbTransaction TransactionLast(OleDbTransaction head)
		{
			if (head._nestedTransaction != null)
			{
				OleDbTransaction oleDbTransaction = (OleDbTransaction)head._nestedTransaction.Target;
				if (oleDbTransaction != null && head._nestedTransaction.IsAlive)
				{
					return OleDbTransaction.TransactionLast(oleDbTransaction);
				}
			}
			return head;
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x002630D8 File Offset: 0x002624D8
		internal static OleDbTransaction TransactionUpdate(OleDbTransaction transaction)
		{
			if (transaction != null && transaction._transaction == null)
			{
				return null;
			}
			return transaction;
		}

		// Token: 0x040014E9 RID: 5353
		private readonly OleDbTransaction _parentTransaction;

		// Token: 0x040014EA RID: 5354
		private readonly IsolationLevel _isolationLevel;

		// Token: 0x040014EB RID: 5355
		private WeakReference _nestedTransaction;

		// Token: 0x040014EC RID: 5356
		private OleDbTransaction.WrappedTransaction _transaction;

		// Token: 0x040014ED RID: 5357
		internal OleDbConnection _parentConnection;

		// Token: 0x040014EE RID: 5358
		private static int _objectTypeCount;

		// Token: 0x040014EF RID: 5359
		internal readonly int _objectID = Interlocked.Increment(ref OleDbTransaction._objectTypeCount);

		// Token: 0x02000250 RID: 592
		private sealed class WrappedTransaction : WrappedIUnknown
		{
			// Token: 0x0600208C RID: 8332 RVA: 0x00263204 File Offset: 0x00262604
			internal WrappedTransaction(UnsafeNativeMethods.ITransactionLocal transaction, int isolevel, out OleDbHResult hr)
				: base(transaction)
			{
				int num = 0;
				Bid.Trace("<oledb.ITransactionLocal.StartTransaction|API|OLEDB>\n");
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					hr = transaction.StartTransaction(isolevel, 0, IntPtr.Zero, out num);
					if (OleDbHResult.S_OK <= hr)
					{
						this._mustComplete = true;
					}
				}
				Bid.Trace("<oledb.ITransactionLocal.StartTransaction|API|OLEDB|RET> %08X{HRESULT}\n", hr);
			}

			// Token: 0x1700047C RID: 1148
			// (get) Token: 0x0600208D RID: 8333 RVA: 0x00263274 File Offset: 0x00262674
			internal bool MustComplete
			{
				get
				{
					return this._mustComplete;
				}
			}

			// Token: 0x0600208E RID: 8334 RVA: 0x00263288 File Offset: 0x00262688
			internal OleDbHResult Abort()
			{
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				OleDbHResult oleDbHResult;
				try
				{
					base.DangerousAddRef(ref flag);
					Bid.Trace("<oledb.ITransactionLocal.Abort|API|OLEDB> handle=%p\n", this.handle);
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
					}
					finally
					{
						oleDbHResult = NativeOledbWrapper.ITransactionAbort(base.DangerousGetHandle());
						this._mustComplete = false;
					}
					Bid.Trace("<oledb.ITransactionLocal.Abort|API|OLEDB|RET> %08X{HRESULT}\n", oleDbHResult);
				}
				finally
				{
					if (flag)
					{
						base.DangerousRelease();
					}
				}
				return oleDbHResult;
			}

			// Token: 0x0600208F RID: 8335 RVA: 0x0026331C File Offset: 0x0026271C
			internal OleDbHResult Commit()
			{
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				OleDbHResult oleDbHResult;
				try
				{
					base.DangerousAddRef(ref flag);
					Bid.Trace("<oledb.ITransactionLocal.Commit|API|OLEDB> handle=%p\n", this.handle);
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
					}
					finally
					{
						oleDbHResult = NativeOledbWrapper.ITransactionCommit(base.DangerousGetHandle());
						if (OleDbHResult.S_OK <= oleDbHResult || OleDbHResult.XACT_E_NOTRANSACTION == oleDbHResult)
						{
							this._mustComplete = false;
						}
					}
					Bid.Trace("<oledb.ITransactionLocal.Commit|API|OLEDB|RET> %08X{HRESULT}\n", oleDbHResult);
				}
				finally
				{
					if (flag)
					{
						base.DangerousRelease();
					}
				}
				return oleDbHResult;
			}

			// Token: 0x06002090 RID: 8336 RVA: 0x002633BC File Offset: 0x002627BC
			protected override bool ReleaseHandle()
			{
				if (this._mustComplete && IntPtr.Zero != this.handle)
				{
					Bid.Trace("<oledb.ITransactionLocal.Abort|API|OLEDB|INFO> handle=%p\n", this.handle);
					OleDbHResult oleDbHResult = NativeOledbWrapper.ITransactionAbort(this.handle);
					this._mustComplete = false;
					Bid.Trace("<oledb.ITransactionLocal.Abort|API|OLEDB|INFO|RET> %08X{HRESULT}\n", oleDbHResult);
				}
				return base.ReleaseHandle();
			}

			// Token: 0x040014F0 RID: 5360
			private bool _mustComplete;
		}
	}
}
