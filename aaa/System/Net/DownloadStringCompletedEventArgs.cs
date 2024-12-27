using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x0200048F RID: 1167
	public class DownloadStringCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023DD RID: 9181 RVA: 0x0008D108 File Offset: 0x0008C108
		internal DownloadStringCompletedEventArgs(string result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x0008D11B File Offset: 0x0008C11B
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x0400245B RID: 9307
		private string m_Result;
	}
}
