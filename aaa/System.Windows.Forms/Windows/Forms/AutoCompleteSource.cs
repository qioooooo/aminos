using System;

namespace System.Windows.Forms
{
	// Token: 0x0200021B RID: 539
	public enum AutoCompleteSource
	{
		// Token: 0x04001223 RID: 4643
		FileSystem = 1,
		// Token: 0x04001224 RID: 4644
		HistoryList,
		// Token: 0x04001225 RID: 4645
		RecentlyUsedList = 4,
		// Token: 0x04001226 RID: 4646
		AllUrl = 6,
		// Token: 0x04001227 RID: 4647
		AllSystemSources,
		// Token: 0x04001228 RID: 4648
		FileSystemDirectories = 32,
		// Token: 0x04001229 RID: 4649
		CustomSource = 64,
		// Token: 0x0400122A RID: 4650
		None = 128,
		// Token: 0x0400122B RID: 4651
		ListItems = 256
	}
}
