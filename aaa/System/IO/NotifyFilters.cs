using System;

namespace System.IO
{
	// Token: 0x02000725 RID: 1829
	[Flags]
	public enum NotifyFilters
	{
		// Token: 0x040031E5 RID: 12773
		FileName = 1,
		// Token: 0x040031E6 RID: 12774
		DirectoryName = 2,
		// Token: 0x040031E7 RID: 12775
		Attributes = 4,
		// Token: 0x040031E8 RID: 12776
		Size = 8,
		// Token: 0x040031E9 RID: 12777
		LastWrite = 16,
		// Token: 0x040031EA RID: 12778
		LastAccess = 32,
		// Token: 0x040031EB RID: 12779
		CreationTime = 64,
		// Token: 0x040031EC RID: 12780
		Security = 256
	}
}
