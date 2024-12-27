using System;
using System.IO;
using System.Net;
using System.Text;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200005A RID: 90
	internal class RequestState
	{
		// Token: 0x060001B0 RID: 432 RVA: 0x00007B88 File Offset: 0x00006B88
		public RequestState()
		{
			this.bufferRead = new byte[1024];
		}

		// Token: 0x040001A0 RID: 416
		public const int bufferSize = 1024;

		// Token: 0x040001A1 RID: 417
		public StringBuilder responseString = new StringBuilder(1024);

		// Token: 0x040001A2 RID: 418
		public string requestString;

		// Token: 0x040001A3 RID: 419
		public HttpWebRequest request;

		// Token: 0x040001A4 RID: 420
		public Stream requestStream;

		// Token: 0x040001A5 RID: 421
		public Stream responseStream;

		// Token: 0x040001A6 RID: 422
		public byte[] bufferRead;

		// Token: 0x040001A7 RID: 423
		public UTF8Encoding encoder = new UTF8Encoding();

		// Token: 0x040001A8 RID: 424
		public DsmlAsyncResult dsmlAsync;

		// Token: 0x040001A9 RID: 425
		internal bool abortCalled;

		// Token: 0x040001AA RID: 426
		internal Exception exception;
	}
}
