using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x02000785 RID: 1925
	internal class SynchronizedServerContextSink : InternalSink, IMessageSink
	{
		// Token: 0x06004519 RID: 17689 RVA: 0x000EC070 File Offset: 0x000EB070
		internal SynchronizedServerContextSink(SynchronizationAttribute prop, IMessageSink nextSink)
		{
			this._property = prop;
			this._nextSink = nextSink;
		}

		// Token: 0x0600451A RID: 17690 RVA: 0x000EC088 File Offset: 0x000EB088
		~SynchronizedServerContextSink()
		{
			this._property.Dispose();
		}

		// Token: 0x0600451B RID: 17691 RVA: 0x000EC0BC File Offset: 0x000EB0BC
		public virtual IMessage SyncProcessMessage(IMessage reqMsg)
		{
			WorkItem workItem = new WorkItem(reqMsg, this._nextSink, null);
			this._property.HandleWorkRequest(workItem);
			return workItem.ReplyMessage;
		}

		// Token: 0x0600451C RID: 17692 RVA: 0x000EC0EC File Offset: 0x000EB0EC
		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			WorkItem workItem = new WorkItem(reqMsg, this._nextSink, replySink);
			workItem.SetAsync();
			this._property.HandleWorkRequest(workItem);
			return null;
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x0600451D RID: 17693 RVA: 0x000EC11A File Offset: 0x000EB11A
		public IMessageSink NextSink
		{
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x0400222E RID: 8750
		internal IMessageSink _nextSink;

		// Token: 0x0400222F RID: 8751
		internal SynchronizationAttribute _property;
	}
}
