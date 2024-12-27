using System;

namespace System.Data
{
	// Token: 0x0200005D RID: 93
	[Flags]
	public enum CommandBehavior
	{
		// Token: 0x040006AD RID: 1709
		Default = 0,
		// Token: 0x040006AE RID: 1710
		SingleResult = 1,
		// Token: 0x040006AF RID: 1711
		SchemaOnly = 2,
		// Token: 0x040006B0 RID: 1712
		KeyInfo = 4,
		// Token: 0x040006B1 RID: 1713
		SingleRow = 8,
		// Token: 0x040006B2 RID: 1714
		SequentialAccess = 16,
		// Token: 0x040006B3 RID: 1715
		CloseConnection = 32
	}
}
