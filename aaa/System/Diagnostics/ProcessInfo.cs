using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x02000777 RID: 1911
	internal class ProcessInfo
	{
		// Token: 0x040033AF RID: 13231
		public ArrayList threadInfoList = new ArrayList();

		// Token: 0x040033B0 RID: 13232
		public int basePriority;

		// Token: 0x040033B1 RID: 13233
		public string processName;

		// Token: 0x040033B2 RID: 13234
		public int processId;

		// Token: 0x040033B3 RID: 13235
		public int handleCount;

		// Token: 0x040033B4 RID: 13236
		public long poolPagedBytes;

		// Token: 0x040033B5 RID: 13237
		public long poolNonpagedBytes;

		// Token: 0x040033B6 RID: 13238
		public long virtualBytes;

		// Token: 0x040033B7 RID: 13239
		public long virtualBytesPeak;

		// Token: 0x040033B8 RID: 13240
		public long workingSetPeak;

		// Token: 0x040033B9 RID: 13241
		public long workingSet;

		// Token: 0x040033BA RID: 13242
		public long pageFileBytesPeak;

		// Token: 0x040033BB RID: 13243
		public long pageFileBytes;

		// Token: 0x040033BC RID: 13244
		public long privateBytes;

		// Token: 0x040033BD RID: 13245
		public int mainModuleId;

		// Token: 0x040033BE RID: 13246
		public int sessionId;
	}
}
