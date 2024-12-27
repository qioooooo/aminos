using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x020006F4 RID: 1780
	internal class LeaseSink : IMessageSink
	{
		// Token: 0x06003FBF RID: 16319 RVA: 0x000D9EFA File Offset: 0x000D8EFA
		public LeaseSink(Lease lease, IMessageSink nextSink)
		{
			this.lease = lease;
			this.nextSink = nextSink;
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x000D9F10 File Offset: 0x000D8F10
		public IMessage SyncProcessMessage(IMessage msg)
		{
			this.lease.RenewOnCall();
			return this.nextSink.SyncProcessMessage(msg);
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x000D9F29 File Offset: 0x000D8F29
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			this.lease.RenewOnCall();
			return this.nextSink.AsyncProcessMessage(msg, replySink);
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06003FC2 RID: 16322 RVA: 0x000D9F43 File Offset: 0x000D8F43
		public IMessageSink NextSink
		{
			get
			{
				return this.nextSink;
			}
		}

		// Token: 0x04002005 RID: 8197
		private Lease lease;

		// Token: 0x04002006 RID: 8198
		private IMessageSink nextSink;
	}
}
