using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x02000495 RID: 1173
	public class UploadDataCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023EF RID: 9199 RVA: 0x0008D16B File Offset: 0x0008C16B
		internal UploadDataCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060023F0 RID: 9200 RVA: 0x0008D17E File Offset: 0x0008C17E
		public byte[] Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x0400245E RID: 9310
		private byte[] m_Result;
	}
}
