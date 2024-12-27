using System;
using System.ComponentModel;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000029 RID: 41
	public class InvokeCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x00003E26 File Offset: 0x00002E26
		internal InvokeCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00003E39 File Offset: 0x00002E39
		public object[] Results
		{
			get
			{
				return this.results;
			}
		}

		// Token: 0x0400025A RID: 602
		private object[] results;
	}
}
