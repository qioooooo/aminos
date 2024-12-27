using System;

namespace System.Net
{
	// Token: 0x020003ED RID: 1005
	internal static class NclConstants
	{
		// Token: 0x04001FB3 RID: 8115
		internal static readonly object Sentinel = new object();

		// Token: 0x04001FB4 RID: 8116
		internal static readonly object[] EmptyObjectArray = new object[0];

		// Token: 0x04001FB5 RID: 8117
		internal static readonly Uri[] EmptyUriArray = new Uri[0];

		// Token: 0x04001FB6 RID: 8118
		internal static readonly byte[] CRLF = new byte[] { 13, 10 };

		// Token: 0x04001FB7 RID: 8119
		internal static readonly byte[] ChunkTerminator = new byte[] { 48, 13, 10, 13, 10 };
	}
}
