using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003B9 RID: 953
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum NumberStyles
	{
		// Token: 0x040011F2 RID: 4594
		None = 0,
		// Token: 0x040011F3 RID: 4595
		AllowLeadingWhite = 1,
		// Token: 0x040011F4 RID: 4596
		AllowTrailingWhite = 2,
		// Token: 0x040011F5 RID: 4597
		AllowLeadingSign = 4,
		// Token: 0x040011F6 RID: 4598
		AllowTrailingSign = 8,
		// Token: 0x040011F7 RID: 4599
		AllowParentheses = 16,
		// Token: 0x040011F8 RID: 4600
		AllowDecimalPoint = 32,
		// Token: 0x040011F9 RID: 4601
		AllowThousands = 64,
		// Token: 0x040011FA RID: 4602
		AllowExponent = 128,
		// Token: 0x040011FB RID: 4603
		AllowCurrencySymbol = 256,
		// Token: 0x040011FC RID: 4604
		AllowHexSpecifier = 512,
		// Token: 0x040011FD RID: 4605
		Integer = 7,
		// Token: 0x040011FE RID: 4606
		HexNumber = 515,
		// Token: 0x040011FF RID: 4607
		Number = 111,
		// Token: 0x04001200 RID: 4608
		Float = 167,
		// Token: 0x04001201 RID: 4609
		Currency = 383,
		// Token: 0x04001202 RID: 4610
		Any = 511
	}
}
