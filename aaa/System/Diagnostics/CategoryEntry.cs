using System;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x0200076A RID: 1898
	internal class CategoryEntry
	{
		// Token: 0x06003A77 RID: 14967 RVA: 0x000F8704 File Offset: 0x000F7704
		internal CategoryEntry(NativeMethods.PERF_OBJECT_TYPE perfObject)
		{
			this.NameIndex = perfObject.ObjectNameTitleIndex;
			this.HelpIndex = perfObject.ObjectHelpTitleIndex;
			this.CounterIndexes = new int[perfObject.NumCounters];
			this.HelpIndexes = new int[perfObject.NumCounters];
		}

		// Token: 0x0400332E RID: 13102
		internal int NameIndex;

		// Token: 0x0400332F RID: 13103
		internal int HelpIndex;

		// Token: 0x04003330 RID: 13104
		internal int[] CounterIndexes;

		// Token: 0x04003331 RID: 13105
		internal int[] HelpIndexes;
	}
}
