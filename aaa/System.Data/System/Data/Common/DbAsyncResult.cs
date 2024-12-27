using System;
using System.Data.ProviderBase;
using System.Threading;

namespace System.Data.Common
{
	// Token: 0x02000125 RID: 293
	internal sealed class DbAsyncResult : IAsyncResult
	{
		// Token: 0x060012E2 RID: 4834 RVA: 0x00220630 File Offset: 0x0021FA30
		internal DbAsyncResult(object owner, string endMethodName, AsyncCallback callback, object stateObject, ExecutionContext execContext)
		{
			this._owner = owner;
			this._endMethodName = endMethodName;
			this._callback = callback;
			this._stateObject = stateObject;
			this._manualResetEvent = new ManualResetEvent(false);
			this._execContext = execContext;
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060012E3 RID: 4835 RVA: 0x00220674 File Offset: 0x0021FA74
		object IAsyncResult.AsyncState
		{
			get
			{
				return this._stateObject;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060012E4 RID: 4836 RVA: 0x00220688 File Offset: 0x0021FA88
		WaitHandle IAsyncResult.AsyncWaitHandle
		{
			get
			{
				return this._manualResetEvent;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x0022069C File Offset: 0x0021FA9C
		bool IAsyncResult.CompletedSynchronously
		{
			get
			{
				return this._fCompletedSynchronously;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x002206B0 File Offset: 0x0021FAB0
		// (set) Token: 0x060012E7 RID: 4839 RVA: 0x002206C4 File Offset: 0x0021FAC4
		internal DbConnectionInternal ConnectionInternal
		{
			get
			{
				return this._connectionInternal;
			}
			set
			{
				this._connectionInternal = value;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060012E8 RID: 4840 RVA: 0x002206D8 File Offset: 0x0021FAD8
		bool IAsyncResult.IsCompleted
		{
			get
			{
				return this._fCompleted;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x002206EC File Offset: 0x0021FAEC
		internal string EndMethodName
		{
			get
			{
				return this._endMethodName;
			}
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x00220700 File Offset: 0x0021FB00
		internal void CompareExchangeOwner(object owner, string method)
		{
			object obj = Interlocked.CompareExchange(ref this._owner, null, owner);
			if (obj == owner)
			{
				return;
			}
			if (obj != null)
			{
				throw ADP.IncorrectAsyncResult();
			}
			throw ADP.MethodCalledTwice(method);
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00220730 File Offset: 0x0021FB30
		internal void Reset()
		{
			this._fCompleted = false;
			this._fCompletedSynchronously = false;
			this._manualResetEvent.Reset();
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x00220758 File Offset: 0x0021FB58
		internal void SetCompleted()
		{
			this._fCompleted = true;
			this._manualResetEvent.Set();
			if (this._callback != null)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.ExecuteCallback), this);
			}
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x00220794 File Offset: 0x0021FB94
		internal void SetCompletedSynchronously()
		{
			this._fCompletedSynchronously = true;
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x002207A8 File Offset: 0x0021FBA8
		private static void AsyncCallback_Context(object state)
		{
			DbAsyncResult dbAsyncResult = (DbAsyncResult)state;
			if (dbAsyncResult._callback != null)
			{
				dbAsyncResult._callback(dbAsyncResult);
			}
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x002207D0 File Offset: 0x0021FBD0
		private void ExecuteCallback(object asyncResult)
		{
			DbAsyncResult dbAsyncResult = (DbAsyncResult)asyncResult;
			if (dbAsyncResult._callback != null)
			{
				if (dbAsyncResult._execContext != null)
				{
					ExecutionContext.Run(dbAsyncResult._execContext, DbAsyncResult._contextCallback, dbAsyncResult);
					return;
				}
				dbAsyncResult._callback(this);
			}
		}

		// Token: 0x04000BB6 RID: 2998
		private readonly AsyncCallback _callback;

		// Token: 0x04000BB7 RID: 2999
		private bool _fCompleted;

		// Token: 0x04000BB8 RID: 3000
		private bool _fCompletedSynchronously;

		// Token: 0x04000BB9 RID: 3001
		private readonly ManualResetEvent _manualResetEvent;

		// Token: 0x04000BBA RID: 3002
		private object _owner;

		// Token: 0x04000BBB RID: 3003
		private readonly object _stateObject;

		// Token: 0x04000BBC RID: 3004
		private readonly string _endMethodName;

		// Token: 0x04000BBD RID: 3005
		private ExecutionContext _execContext;

		// Token: 0x04000BBE RID: 3006
		private static ContextCallback _contextCallback = new ContextCallback(DbAsyncResult.AsyncCallback_Context);

		// Token: 0x04000BBF RID: 3007
		private DbConnectionInternal _connectionInternal;
	}
}
