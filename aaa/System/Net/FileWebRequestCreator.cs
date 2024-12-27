using System;

namespace System.Net
{
	// Token: 0x020003B3 RID: 947
	internal class FileWebRequestCreator : IWebRequestCreate
	{
		// Token: 0x06001DC3 RID: 7619 RVA: 0x000711FD File Offset: 0x000701FD
		internal FileWebRequestCreator()
		{
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x00071205 File Offset: 0x00070205
		public WebRequest Create(Uri uri)
		{
			return new FileWebRequest(uri);
		}
	}
}
