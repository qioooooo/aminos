using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x0200049B RID: 1179
	public class DownloadProgressChangedEventArgs : ProgressChangedEventArgs
	{
		// Token: 0x06002401 RID: 9217 RVA: 0x0008D1CE File Offset: 0x0008C1CE
		internal DownloadProgressChangedEventArgs(int progressPercentage, object userToken, long bytesReceived, long totalBytesToReceive)
			: base(progressPercentage, userToken)
		{
			this.m_BytesReceived = bytesReceived;
			this.m_TotalBytesToReceive = totalBytesToReceive;
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002402 RID: 9218 RVA: 0x0008D1E7 File Offset: 0x0008C1E7
		public long BytesReceived
		{
			get
			{
				return this.m_BytesReceived;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002403 RID: 9219 RVA: 0x0008D1EF File Offset: 0x0008C1EF
		public long TotalBytesToReceive
		{
			get
			{
				return this.m_TotalBytesToReceive;
			}
		}

		// Token: 0x04002461 RID: 9313
		private long m_BytesReceived;

		// Token: 0x04002462 RID: 9314
		private long m_TotalBytesToReceive;
	}
}
