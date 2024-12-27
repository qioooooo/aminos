using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x02000497 RID: 1175
	public class UploadFileCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023F5 RID: 9205 RVA: 0x0008D18C File Offset: 0x0008C18C
		internal UploadFileCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x060023F6 RID: 9206 RVA: 0x0008D19F File Offset: 0x0008C19F
		public byte[] Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x0400245F RID: 9311
		private byte[] m_Result;
	}
}
