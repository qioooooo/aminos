using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000054 RID: 84
	internal class ADExtenMatchFilter
	{
		// Token: 0x060001A2 RID: 418 RVA: 0x00007A55 File Offset: 0x00006A55
		public ADExtenMatchFilter()
		{
			this.Value = null;
			this.DNAttributes = false;
		}

		// Token: 0x0400018B RID: 395
		public string Name;

		// Token: 0x0400018C RID: 396
		public ADValue Value;

		// Token: 0x0400018D RID: 397
		public bool DNAttributes;

		// Token: 0x0400018E RID: 398
		public string MatchingRule;
	}
}
