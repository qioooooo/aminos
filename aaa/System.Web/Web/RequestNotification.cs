using System;

namespace System.Web
{
	// Token: 0x020000C1 RID: 193
	[Flags]
	public enum RequestNotification
	{
		// Token: 0x04001201 RID: 4609
		BeginRequest = 1,
		// Token: 0x04001202 RID: 4610
		AuthenticateRequest = 2,
		// Token: 0x04001203 RID: 4611
		AuthorizeRequest = 4,
		// Token: 0x04001204 RID: 4612
		ResolveRequestCache = 8,
		// Token: 0x04001205 RID: 4613
		MapRequestHandler = 16,
		// Token: 0x04001206 RID: 4614
		AcquireRequestState = 32,
		// Token: 0x04001207 RID: 4615
		PreExecuteRequestHandler = 64,
		// Token: 0x04001208 RID: 4616
		ExecuteRequestHandler = 128,
		// Token: 0x04001209 RID: 4617
		ReleaseRequestState = 256,
		// Token: 0x0400120A RID: 4618
		UpdateRequestCache = 512,
		// Token: 0x0400120B RID: 4619
		LogRequest = 1024,
		// Token: 0x0400120C RID: 4620
		EndRequest = 2048,
		// Token: 0x0400120D RID: 4621
		SendResponse = 536870912
	}
}
