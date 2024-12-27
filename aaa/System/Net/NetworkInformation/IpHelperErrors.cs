using System;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005ED RID: 1517
	internal class IpHelperErrors
	{
		// Token: 0x06002FCC RID: 12236 RVA: 0x000CF16E File Offset: 0x000CE16E
		internal static void CheckFamilyUnspecified(AddressFamily family)
		{
			if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6 && family != AddressFamily.Unspecified)
			{
				throw new ArgumentException(SR.GetString("net_invalidversion"), "family");
			}
		}

		// Token: 0x04002CBB RID: 11451
		internal const uint Success = 0U;

		// Token: 0x04002CBC RID: 11452
		internal const uint ErrorInvalidFunction = 1U;

		// Token: 0x04002CBD RID: 11453
		internal const uint ErrorNoSuchDevice = 2U;

		// Token: 0x04002CBE RID: 11454
		internal const uint ErrorInvalidData = 13U;

		// Token: 0x04002CBF RID: 11455
		internal const uint ErrorInvalidParameter = 87U;

		// Token: 0x04002CC0 RID: 11456
		internal const uint ErrorBufferOverflow = 111U;

		// Token: 0x04002CC1 RID: 11457
		internal const uint ErrorInsufficientBuffer = 122U;

		// Token: 0x04002CC2 RID: 11458
		internal const uint ErrorNoData = 232U;

		// Token: 0x04002CC3 RID: 11459
		internal const uint Pending = 997U;

		// Token: 0x04002CC4 RID: 11460
		internal const uint ErrorNotFound = 1168U;
	}
}
