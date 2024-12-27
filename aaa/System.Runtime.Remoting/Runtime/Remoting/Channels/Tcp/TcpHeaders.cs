using System;

namespace System.Runtime.Remoting.Channels.Tcp
{
	// Token: 0x0200004B RID: 75
	internal static class TcpHeaders
	{
		// Token: 0x040001BB RID: 443
		internal const ushort EndOfHeaders = 0;

		// Token: 0x040001BC RID: 444
		internal const ushort Custom = 1;

		// Token: 0x040001BD RID: 445
		internal const ushort StatusCode = 2;

		// Token: 0x040001BE RID: 446
		internal const ushort StatusPhrase = 3;

		// Token: 0x040001BF RID: 447
		internal const ushort RequestUri = 4;

		// Token: 0x040001C0 RID: 448
		internal const ushort CloseConnection = 5;

		// Token: 0x040001C1 RID: 449
		internal const ushort ContentType = 6;
	}
}
