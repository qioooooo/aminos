using System;

namespace System.Net
{
	// Token: 0x0200039E RID: 926
	internal class SystemNetworkCredential : NetworkCredential
	{
		// Token: 0x06001CE8 RID: 7400 RVA: 0x0006E2E2 File Offset: 0x0006D2E2
		private SystemNetworkCredential()
			: base(string.Empty, string.Empty, string.Empty)
		{
		}

		// Token: 0x04001D4B RID: 7499
		internal static readonly SystemNetworkCredential defaultCredential = new SystemNetworkCredential();
	}
}
