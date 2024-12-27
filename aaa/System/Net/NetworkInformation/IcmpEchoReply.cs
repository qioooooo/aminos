using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200060B RID: 1547
	internal struct IcmpEchoReply
	{
		// Token: 0x04002DAE RID: 11694
		internal uint address;

		// Token: 0x04002DAF RID: 11695
		internal uint status;

		// Token: 0x04002DB0 RID: 11696
		internal uint roundTripTime;

		// Token: 0x04002DB1 RID: 11697
		internal ushort dataSize;

		// Token: 0x04002DB2 RID: 11698
		internal ushort reserved;

		// Token: 0x04002DB3 RID: 11699
		internal IntPtr data;

		// Token: 0x04002DB4 RID: 11700
		internal IPOptions options;
	}
}
