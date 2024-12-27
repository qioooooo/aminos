using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x02000499 RID: 1177
	public class UploadValuesCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023FB RID: 9211 RVA: 0x0008D1AD File Offset: 0x0008C1AD
		internal UploadValuesCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x060023FC RID: 9212 RVA: 0x0008D1C0 File Offset: 0x0008C1C0
		public byte[] Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04002460 RID: 9312
		private byte[] m_Result;
	}
}
