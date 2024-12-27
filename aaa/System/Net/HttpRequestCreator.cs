using System;

namespace System.Net
{
	// Token: 0x0200040F RID: 1039
	internal class HttpRequestCreator : IWebRequestCreate
	{
		// Token: 0x060020C3 RID: 8387 RVA: 0x000810D6 File Offset: 0x000800D6
		public WebRequest Create(Uri Uri)
		{
			return new HttpWebRequest(Uri, null);
		}
	}
}
