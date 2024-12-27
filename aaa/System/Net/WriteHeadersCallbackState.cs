using System;

namespace System.Net
{
	// Token: 0x020004CE RID: 1230
	internal struct WriteHeadersCallbackState
	{
		// Token: 0x0600260D RID: 9741 RVA: 0x00099AB8 File Offset: 0x00098AB8
		internal WriteHeadersCallbackState(HttpWebRequest request, ConnectStream stream)
		{
			this.request = request;
			this.stream = stream;
		}

		// Token: 0x040025BE RID: 9662
		internal HttpWebRequest request;

		// Token: 0x040025BF RID: 9663
		internal ConnectStream stream;
	}
}
