using System;
using System.Threading;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200007C RID: 124
	internal class PartialResultsRetriever
	{
		// Token: 0x060002A0 RID: 672 RVA: 0x0000DDE4 File Offset: 0x0000CDE4
		internal PartialResultsRetriever(ManualResetEvent eventHandle, LdapPartialResultsProcessor processor)
		{
			this.workThreadWaitHandle = eventHandle;
			this.processor = processor;
			this.oThread = new Thread(new ThreadStart(this.ThreadRoutine));
			this.oThread.IsBackground = true;
			this.oThread.Start();
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000DE34 File Offset: 0x0000CE34
		private void ThreadRoutine()
		{
			for (;;)
			{
				this.workThreadWaitHandle.WaitOne();
				try
				{
					this.processor.RetrievingSearchResults();
				}
				catch (Exception)
				{
				}
				catch
				{
				}
				Thread.Sleep(250);
			}
		}

		// Token: 0x04000273 RID: 627
		private ManualResetEvent workThreadWaitHandle;

		// Token: 0x04000274 RID: 628
		private Thread oThread;

		// Token: 0x04000275 RID: 629
		private LdapPartialResultsProcessor processor;
	}
}
