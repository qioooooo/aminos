using System;
using System.Threading;

namespace System.Net.Mime
{
	// Token: 0x020006B1 RID: 1713
	internal class MultiAsyncResult : LazyAsyncResult
	{
		// Token: 0x060034EC RID: 13548 RVA: 0x000E0DBB File Offset: 0x000DFDBB
		internal MultiAsyncResult(object context, AsyncCallback callback, object state)
			: base(context, state, callback)
		{
			this.context = context;
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x060034ED RID: 13549 RVA: 0x000E0DCD File Offset: 0x000DFDCD
		internal object Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x000E0DD5 File Offset: 0x000DFDD5
		internal void Enter()
		{
			this.Increment();
		}

		// Token: 0x060034EF RID: 13551 RVA: 0x000E0DDD File Offset: 0x000DFDDD
		internal void Leave()
		{
			this.Decrement();
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x000E0DE5 File Offset: 0x000DFDE5
		internal void Leave(object result)
		{
			base.Result = result;
			this.Decrement();
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x000E0DF4 File Offset: 0x000DFDF4
		private void Decrement()
		{
			if (Interlocked.Decrement(ref this.outstanding) == -1)
			{
				base.InvokeCallback(base.Result);
			}
		}

		// Token: 0x060034F2 RID: 13554 RVA: 0x000E0E10 File Offset: 0x000DFE10
		private void Increment()
		{
			Interlocked.Increment(ref this.outstanding);
		}

		// Token: 0x060034F3 RID: 13555 RVA: 0x000E0E1E File Offset: 0x000DFE1E
		internal void CompleteSequence()
		{
			this.Decrement();
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x000E0E28 File Offset: 0x000DFE28
		internal static object End(IAsyncResult result)
		{
			MultiAsyncResult multiAsyncResult = (MultiAsyncResult)result;
			multiAsyncResult.InternalWaitForCompletion();
			return multiAsyncResult.Result;
		}

		// Token: 0x04003093 RID: 12435
		private int outstanding;

		// Token: 0x04003094 RID: 12436
		private object context;
	}
}
