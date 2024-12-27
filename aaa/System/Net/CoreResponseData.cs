using System;
using System.IO;

namespace System.Net
{
	// Token: 0x02000410 RID: 1040
	internal class CoreResponseData
	{
		// Token: 0x060020C5 RID: 8389 RVA: 0x000810E8 File Offset: 0x000800E8
		internal CoreResponseData Clone()
		{
			return new CoreResponseData
			{
				m_StatusCode = this.m_StatusCode,
				m_StatusDescription = this.m_StatusDescription,
				m_IsVersionHttp11 = this.m_IsVersionHttp11,
				m_ContentLength = this.m_ContentLength,
				m_ResponseHeaders = this.m_ResponseHeaders,
				m_ConnectStream = this.m_ConnectStream
			};
		}

		// Token: 0x040020C8 RID: 8392
		public HttpStatusCode m_StatusCode;

		// Token: 0x040020C9 RID: 8393
		public string m_StatusDescription;

		// Token: 0x040020CA RID: 8394
		public bool m_IsVersionHttp11;

		// Token: 0x040020CB RID: 8395
		public long m_ContentLength;

		// Token: 0x040020CC RID: 8396
		public WebHeaderCollection m_ResponseHeaders;

		// Token: 0x040020CD RID: 8397
		public Stream m_ConnectStream;
	}
}
