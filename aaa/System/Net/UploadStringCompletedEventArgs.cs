using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x02000493 RID: 1171
	public class UploadStringCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023E9 RID: 9193 RVA: 0x0008D14A File Offset: 0x0008C14A
		internal UploadStringCompletedEventArgs(string result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060023EA RID: 9194 RVA: 0x0008D15D File Offset: 0x0008C15D
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x0400245D RID: 9309
		private string m_Result;
	}
}
