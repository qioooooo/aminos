using System;

namespace System.Net
{
	// Token: 0x020003C2 RID: 962
	internal class FtpWebRequestCreator : IWebRequestCreate
	{
		// Token: 0x06001E48 RID: 7752 RVA: 0x000742CE File Offset: 0x000732CE
		internal FtpWebRequestCreator()
		{
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000742D6 File Offset: 0x000732D6
		public WebRequest Create(Uri uri)
		{
			return new FtpWebRequest(uri);
		}
	}
}
