using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005F3 RID: 1523
	internal struct IPExtendedAddress
	{
		// Token: 0x06002FCE RID: 12238 RVA: 0x000CF199 File Offset: 0x000CE199
		internal IPExtendedAddress(IPAddress address, IPAddress mask)
		{
			this.address = address;
			this.mask = mask;
		}

		// Token: 0x04002CE5 RID: 11493
		internal IPAddress mask;

		// Token: 0x04002CE6 RID: 11494
		internal IPAddress address;
	}
}
