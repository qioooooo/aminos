using System;
using System.Threading;

namespace System.Transactions
{
	// Token: 0x0200006A RID: 106
	internal class CheapUnfairReaderWriterLock
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x00031D90 File Offset: 0x00031190
		private object SyncRoot
		{
			get
			{
				if (this.syncRoot == null)
				{
					Interlocked.CompareExchange(ref this.syncRoot, new object(), null);
				}
				return this.syncRoot;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00031DC0 File Offset: 0x000311C0
		private bool ReadersPresent
		{
			get
			{
				return this.readersIn != this.readersOut;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00031DE0 File Offset: 0x000311E0
		private ManualResetEvent WriterFinishedEvent
		{
			get
			{
				if (this.writerFinishedEvent == null)
				{
					Interlocked.CompareExchange(ref this.writerFinishedEvent, new ManualResetEvent(true), null);
				}
				return (ManualResetEvent)this.writerFinishedEvent;
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00031E14 File Offset: 0x00031214
		public int AcquireReaderLock()
		{
			int num;
			for (;;)
			{
				if (this.writerPresent)
				{
					this.WriterFinishedEvent.WaitOne();
				}
				num = Interlocked.Increment(ref this.readersIn);
				if (!this.writerPresent)
				{
					break;
				}
				Interlocked.Decrement(ref this.readersIn);
			}
			return num;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00031E5C File Offset: 0x0003125C
		public void AcquireWriterLock()
		{
			Monitor.Enter(this.SyncRoot);
			this.writerPresent = true;
			this.WriterFinishedEvent.Reset();
			do
			{
				int num = 0;
				while (this.ReadersPresent && num < 100)
				{
					Thread.Sleep(0);
					num++;
				}
				if (this.ReadersPresent)
				{
					Thread.Sleep(500);
				}
			}
			while (this.ReadersPresent);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00031EBC File Offset: 0x000312BC
		public void ReleaseReaderLock()
		{
			Interlocked.Increment(ref this.readersOut);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00031ED8 File Offset: 0x000312D8
		public void ReleaseWriterLock()
		{
			try
			{
				this.writerPresent = false;
				this.WriterFinishedEvent.Set();
			}
			finally
			{
				Monitor.Exit(this.SyncRoot);
			}
		}

		// Token: 0x04000127 RID: 295
		private const int MAX_SPIN_COUNT = 100;

		// Token: 0x04000128 RID: 296
		private const int SLEEP_TIME = 500;

		// Token: 0x04000129 RID: 297
		private object writerFinishedEvent;

		// Token: 0x0400012A RID: 298
		private int readersIn;

		// Token: 0x0400012B RID: 299
		private int readersOut;

		// Token: 0x0400012C RID: 300
		private bool writerPresent;

		// Token: 0x0400012D RID: 301
		private object syncRoot;
	}
}
