using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000012 RID: 18
	[Flags]
	public enum SecurityMasks
	{
		// Token: 0x040000CB RID: 203
		None = 0,
		// Token: 0x040000CC RID: 204
		Owner = 1,
		// Token: 0x040000CD RID: 205
		Group = 2,
		// Token: 0x040000CE RID: 206
		Dacl = 4,
		// Token: 0x040000CF RID: 207
		Sacl = 8
	}
}
