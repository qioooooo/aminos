using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200060D RID: 1549
	internal struct Icmp6EchoReply
	{
		// Token: 0x04002DB8 RID: 11704
		internal Ipv6Address Address;

		// Token: 0x04002DB9 RID: 11705
		internal uint Status;

		// Token: 0x04002DBA RID: 11706
		internal uint RoundTripTime;

		// Token: 0x04002DBB RID: 11707
		internal IntPtr data;
	}
}
