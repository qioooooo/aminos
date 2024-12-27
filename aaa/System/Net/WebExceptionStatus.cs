using System;

namespace System.Net
{
	// Token: 0x020004A0 RID: 1184
	public enum WebExceptionStatus
	{
		// Token: 0x04002470 RID: 9328
		Success,
		// Token: 0x04002471 RID: 9329
		NameResolutionFailure,
		// Token: 0x04002472 RID: 9330
		ConnectFailure,
		// Token: 0x04002473 RID: 9331
		ReceiveFailure,
		// Token: 0x04002474 RID: 9332
		SendFailure,
		// Token: 0x04002475 RID: 9333
		PipelineFailure,
		// Token: 0x04002476 RID: 9334
		RequestCanceled,
		// Token: 0x04002477 RID: 9335
		ProtocolError,
		// Token: 0x04002478 RID: 9336
		ConnectionClosed,
		// Token: 0x04002479 RID: 9337
		TrustFailure,
		// Token: 0x0400247A RID: 9338
		SecureChannelFailure,
		// Token: 0x0400247B RID: 9339
		ServerProtocolViolation,
		// Token: 0x0400247C RID: 9340
		KeepAliveFailure,
		// Token: 0x0400247D RID: 9341
		Pending,
		// Token: 0x0400247E RID: 9342
		Timeout,
		// Token: 0x0400247F RID: 9343
		ProxyNameResolutionFailure,
		// Token: 0x04002480 RID: 9344
		UnknownError,
		// Token: 0x04002481 RID: 9345
		MessageLengthLimitExceeded,
		// Token: 0x04002482 RID: 9346
		CacheEntryNotFound,
		// Token: 0x04002483 RID: 9347
		RequestProhibitedByCachePolicy,
		// Token: 0x04002484 RID: 9348
		RequestProhibitedByProxy
	}
}
