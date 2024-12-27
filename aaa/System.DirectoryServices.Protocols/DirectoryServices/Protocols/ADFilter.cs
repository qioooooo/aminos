using System;
using System.Collections;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000051 RID: 81
	internal class ADFilter
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x00007A41 File Offset: 0x00006A41
		public ADFilter()
		{
			this.Filter = default(ADFilter.FilterContent);
		}

		// Token: 0x04000174 RID: 372
		public ADFilter.FilterType Type;

		// Token: 0x04000175 RID: 373
		public ADFilter.FilterContent Filter;

		// Token: 0x02000052 RID: 82
		public enum FilterType
		{
			// Token: 0x04000177 RID: 375
			And,
			// Token: 0x04000178 RID: 376
			Or,
			// Token: 0x04000179 RID: 377
			Not,
			// Token: 0x0400017A RID: 378
			EqualityMatch,
			// Token: 0x0400017B RID: 379
			Substrings,
			// Token: 0x0400017C RID: 380
			GreaterOrEqual,
			// Token: 0x0400017D RID: 381
			LessOrEqual,
			// Token: 0x0400017E RID: 382
			Present,
			// Token: 0x0400017F RID: 383
			ApproxMatch,
			// Token: 0x04000180 RID: 384
			ExtensibleMatch
		}

		// Token: 0x02000053 RID: 83
		public struct FilterContent
		{
			// Token: 0x04000181 RID: 385
			public ArrayList And;

			// Token: 0x04000182 RID: 386
			public ArrayList Or;

			// Token: 0x04000183 RID: 387
			public ADFilter Not;

			// Token: 0x04000184 RID: 388
			public ADAttribute EqualityMatch;

			// Token: 0x04000185 RID: 389
			public ADSubstringFilter Substrings;

			// Token: 0x04000186 RID: 390
			public ADAttribute GreaterOrEqual;

			// Token: 0x04000187 RID: 391
			public ADAttribute LessOrEqual;

			// Token: 0x04000188 RID: 392
			public string Present;

			// Token: 0x04000189 RID: 393
			public ADAttribute ApproxMatch;

			// Token: 0x0400018A RID: 394
			public ADExtenMatchFilter ExtensibleMatch;
		}
	}
}
