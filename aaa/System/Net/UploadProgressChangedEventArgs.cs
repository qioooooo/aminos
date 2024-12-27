using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x0200049D RID: 1181
	public class UploadProgressChangedEventArgs : ProgressChangedEventArgs
	{
		// Token: 0x06002408 RID: 9224 RVA: 0x0008D1F7 File Offset: 0x0008C1F7
		internal UploadProgressChangedEventArgs(int progressPercentage, object userToken, long bytesSent, long totalBytesToSend, long bytesReceived, long totalBytesToReceive)
			: base(progressPercentage, userToken)
		{
			this.m_BytesReceived = bytesReceived;
			this.m_TotalBytesToReceive = totalBytesToReceive;
			this.m_BytesSent = bytesSent;
			this.m_TotalBytesToSend = totalBytesToSend;
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002409 RID: 9225 RVA: 0x0008D220 File Offset: 0x0008C220
		public long BytesReceived
		{
			get
			{
				return this.m_BytesReceived;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x0600240A RID: 9226 RVA: 0x0008D228 File Offset: 0x0008C228
		public long TotalBytesToReceive
		{
			get
			{
				return this.m_TotalBytesToReceive;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x0600240B RID: 9227 RVA: 0x0008D230 File Offset: 0x0008C230
		public long BytesSent
		{
			get
			{
				return this.m_BytesSent;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x0600240C RID: 9228 RVA: 0x0008D238 File Offset: 0x0008C238
		public long TotalBytesToSend
		{
			get
			{
				return this.m_TotalBytesToSend;
			}
		}

		// Token: 0x04002463 RID: 9315
		private long m_BytesReceived;

		// Token: 0x04002464 RID: 9316
		private long m_TotalBytesToReceive;

		// Token: 0x04002465 RID: 9317
		private long m_BytesSent;

		// Token: 0x04002466 RID: 9318
		private long m_TotalBytesToSend;
	}
}
