using System;

namespace System.Web
{
	// Token: 0x02000078 RID: 120
	internal class HttpInputStreamFilterSource : HttpInputStream
	{
		// Token: 0x06000529 RID: 1321 RVA: 0x000152C2 File Offset: 0x000142C2
		internal HttpInputStreamFilterSource()
			: base(null, 0, 0)
		{
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x000152CD File Offset: 0x000142CD
		internal void SetContent(HttpRawUploadedContent data)
		{
			if (data != null)
			{
				base.Init(data, 0, data.Length);
				return;
			}
			base.Uninit();
		}
	}
}
