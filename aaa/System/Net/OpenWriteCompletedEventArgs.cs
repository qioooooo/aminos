using System;
using System.ComponentModel;
using System.IO;

namespace System.Net
{
	// Token: 0x0200048D RID: 1165
	public class OpenWriteCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023D7 RID: 9175 RVA: 0x0008D0E7 File Offset: 0x0008C0E7
		internal OpenWriteCompletedEventArgs(Stream result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x060023D8 RID: 9176 RVA: 0x0008D0FA File Offset: 0x0008C0FA
		public Stream Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x0400245A RID: 9306
		private Stream m_Result;
	}
}
