using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Net.Mime
{
	// Token: 0x02000681 RID: 1665
	internal abstract class BaseWriter
	{
		// Token: 0x06003389 RID: 13193
		internal abstract IAsyncResult BeginGetContentStream(AsyncCallback callback, object state);

		// Token: 0x0600338A RID: 13194
		internal abstract Stream EndGetContentStream(IAsyncResult result);

		// Token: 0x0600338B RID: 13195
		internal abstract Stream GetContentStream();

		// Token: 0x0600338C RID: 13196
		internal abstract void WriteHeader(string name, string value);

		// Token: 0x0600338D RID: 13197
		internal abstract void WriteHeaders(NameValueCollection headers);

		// Token: 0x0600338E RID: 13198
		internal abstract void Close();
	}
}
