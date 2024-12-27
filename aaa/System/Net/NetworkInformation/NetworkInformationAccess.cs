using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000619 RID: 1561
	[Flags]
	public enum NetworkInformationAccess
	{
		// Token: 0x04002DD4 RID: 11732
		None = 0,
		// Token: 0x04002DD5 RID: 11733
		Read = 1,
		// Token: 0x04002DD6 RID: 11734
		Ping = 4
	}
}
