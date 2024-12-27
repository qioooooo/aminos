using System;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200003C RID: 60
	internal abstract class SmiLink
	{
		// Token: 0x06000225 RID: 549
		internal abstract ulong NegotiateVersion(ulong requestedVersion);

		// Token: 0x06000226 RID: 550
		internal abstract object GetCurrentContext(SmiEventSink eventSink);

		// Token: 0x0400058F RID: 1423
		internal const ulong InterfaceVersion = 210UL;
	}
}
