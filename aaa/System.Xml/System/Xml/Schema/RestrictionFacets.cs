using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x020001AA RID: 426
	internal class RestrictionFacets
	{
		// Token: 0x04000D05 RID: 3333
		internal int Length;

		// Token: 0x04000D06 RID: 3334
		internal int MinLength;

		// Token: 0x04000D07 RID: 3335
		internal int MaxLength;

		// Token: 0x04000D08 RID: 3336
		internal ArrayList Patterns;

		// Token: 0x04000D09 RID: 3337
		internal ArrayList Enumeration;

		// Token: 0x04000D0A RID: 3338
		internal XmlSchemaWhiteSpace WhiteSpace;

		// Token: 0x04000D0B RID: 3339
		internal object MaxInclusive;

		// Token: 0x04000D0C RID: 3340
		internal object MaxExclusive;

		// Token: 0x04000D0D RID: 3341
		internal object MinInclusive;

		// Token: 0x04000D0E RID: 3342
		internal object MinExclusive;

		// Token: 0x04000D0F RID: 3343
		internal int TotalDigits;

		// Token: 0x04000D10 RID: 3344
		internal int FractionDigits;

		// Token: 0x04000D11 RID: 3345
		internal RestrictionFlags Flags;

		// Token: 0x04000D12 RID: 3346
		internal RestrictionFlags FixedFlags;
	}
}
