using System;
using System.ComponentModel;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000622 RID: 1570
	public class PingCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x0600303E RID: 12350 RVA: 0x000D0264 File Offset: 0x000CF264
		internal PingCompletedEventArgs(PingReply reply, Exception error, bool cancelled, object userToken)
			: base(error, cancelled, userToken)
		{
			this.reply = reply;
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x0600303F RID: 12351 RVA: 0x000D0277 File Offset: 0x000CF277
		public PingReply Reply
		{
			get
			{
				return this.reply;
			}
		}

		// Token: 0x04002DF0 RID: 11760
		private PingReply reply;
	}
}
