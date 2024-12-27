using System;

namespace System.Net
{
	// Token: 0x020004ED RID: 1261
	internal static class GlobalSSPI
	{
		// Token: 0x040026B5 RID: 9909
		internal static SSPIInterface SSPIAuth = new SSPIAuthType();

		// Token: 0x040026B6 RID: 9910
		internal static SSPIInterface SSPISecureChannel = new SSPISecureChannelType();
	}
}
