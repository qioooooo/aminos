using System;

namespace System.IO
{
	// Token: 0x02000733 RID: 1843
	[Flags]
	public enum WatcherChangeTypes
	{
		// Token: 0x04003225 RID: 12837
		Created = 1,
		// Token: 0x04003226 RID: 12838
		Deleted = 2,
		// Token: 0x04003227 RID: 12839
		Changed = 4,
		// Token: 0x04003228 RID: 12840
		Renamed = 8,
		// Token: 0x04003229 RID: 12841
		All = 15
	}
}
