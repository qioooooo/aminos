using System;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200078E RID: 1934
	internal class DisposeSink : IMessageSink
	{
		// Token: 0x06004550 RID: 17744 RVA: 0x000ECA7F File Offset: 0x000EBA7F
		internal DisposeSink(IDisposable iDis, IMessageSink replySink)
		{
			this._iDis = iDis;
			this._replySink = replySink;
		}

		// Token: 0x06004551 RID: 17745 RVA: 0x000ECA98 File Offset: 0x000EBA98
		public virtual IMessage SyncProcessMessage(IMessage reqMsg)
		{
			IMessage message = null;
			try
			{
				if (this._replySink != null)
				{
					message = this._replySink.SyncProcessMessage(reqMsg);
				}
			}
			finally
			{
				this._iDis.Dispose();
			}
			return message;
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x000ECADC File Offset: 0x000EBADC
		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x06004553 RID: 17747 RVA: 0x000ECAE3 File Offset: 0x000EBAE3
		public IMessageSink NextSink
		{
			get
			{
				return this._replySink;
			}
		}

		// Token: 0x0400224D RID: 8781
		private IDisposable _iDis;

		// Token: 0x0400224E RID: 8782
		private IMessageSink _replySink;
	}
}
