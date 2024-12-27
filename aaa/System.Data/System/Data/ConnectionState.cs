using System;

namespace System.Data
{
	// Token: 0x02000060 RID: 96
	[Flags]
	public enum ConnectionState
	{
		// Token: 0x040006BD RID: 1725
		Closed = 0,
		// Token: 0x040006BE RID: 1726
		Open = 1,
		// Token: 0x040006BF RID: 1727
		Connecting = 2,
		// Token: 0x040006C0 RID: 1728
		Executing = 4,
		// Token: 0x040006C1 RID: 1729
		Fetching = 8,
		// Token: 0x040006C2 RID: 1730
		Broken = 16
	}
}
