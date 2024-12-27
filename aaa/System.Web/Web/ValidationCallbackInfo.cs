using System;

namespace System.Web
{
	// Token: 0x0200005D RID: 93
	internal sealed class ValidationCallbackInfo
	{
		// Token: 0x0600033A RID: 826 RVA: 0x0000F4C4 File Offset: 0x0000E4C4
		internal ValidationCallbackInfo(HttpCacheValidateHandler handler, object data)
		{
			this.handler = handler;
			this.data = data;
		}

		// Token: 0x04000F6D RID: 3949
		internal readonly HttpCacheValidateHandler handler;

		// Token: 0x04000F6E RID: 3950
		internal readonly object data;
	}
}
