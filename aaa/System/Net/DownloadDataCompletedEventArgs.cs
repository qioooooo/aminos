using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x02000491 RID: 1169
	public class DownloadDataCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023E3 RID: 9187 RVA: 0x0008D129 File Offset: 0x0008C129
		internal DownloadDataCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x060023E4 RID: 9188 RVA: 0x0008D13C File Offset: 0x0008C13C
		public byte[] Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x0400245C RID: 9308
		private byte[] m_Result;
	}
}
