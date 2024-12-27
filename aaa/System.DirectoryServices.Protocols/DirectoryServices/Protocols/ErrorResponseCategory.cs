using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000063 RID: 99
	public enum ErrorResponseCategory
	{
		// Token: 0x040001F2 RID: 498
		NotAttempted,
		// Token: 0x040001F3 RID: 499
		CouldNotConnect,
		// Token: 0x040001F4 RID: 500
		ConnectionClosed,
		// Token: 0x040001F5 RID: 501
		MalformedRequest,
		// Token: 0x040001F6 RID: 502
		GatewayInternalError,
		// Token: 0x040001F7 RID: 503
		AuthenticationFailed,
		// Token: 0x040001F8 RID: 504
		UnresolvableUri,
		// Token: 0x040001F9 RID: 505
		Other
	}
}
