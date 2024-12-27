using System;

namespace System.Xml.Xsl.XPath
{
	// Token: 0x020000DF RID: 223
	internal enum LexKind
	{
		// Token: 0x040006BB RID: 1723
		Comma = 44,
		// Token: 0x040006BC RID: 1724
		Slash = 47,
		// Token: 0x040006BD RID: 1725
		At = 64,
		// Token: 0x040006BE RID: 1726
		Dot = 46,
		// Token: 0x040006BF RID: 1727
		LParens = 40,
		// Token: 0x040006C0 RID: 1728
		RParens,
		// Token: 0x040006C1 RID: 1729
		LBracket = 91,
		// Token: 0x040006C2 RID: 1730
		RBracket = 93,
		// Token: 0x040006C3 RID: 1731
		LBrace = 123,
		// Token: 0x040006C4 RID: 1732
		RBrace = 125,
		// Token: 0x040006C5 RID: 1733
		Star = 42,
		// Token: 0x040006C6 RID: 1734
		Plus,
		// Token: 0x040006C7 RID: 1735
		Minus = 45,
		// Token: 0x040006C8 RID: 1736
		Eq = 61,
		// Token: 0x040006C9 RID: 1737
		Lt = 60,
		// Token: 0x040006CA RID: 1738
		Gt = 62,
		// Token: 0x040006CB RID: 1739
		Bang = 33,
		// Token: 0x040006CC RID: 1740
		Dollar = 36,
		// Token: 0x040006CD RID: 1741
		Union = 124,
		// Token: 0x040006CE RID: 1742
		Ne = 78,
		// Token: 0x040006CF RID: 1743
		Le = 76,
		// Token: 0x040006D0 RID: 1744
		Ge = 71,
		// Token: 0x040006D1 RID: 1745
		DotDot = 68,
		// Token: 0x040006D2 RID: 1746
		SlashSlash = 83,
		// Token: 0x040006D3 RID: 1747
		Name = 110,
		// Token: 0x040006D4 RID: 1748
		String = 115,
		// Token: 0x040006D5 RID: 1749
		Number = 100,
		// Token: 0x040006D6 RID: 1750
		Axis = 97,
		// Token: 0x040006D7 RID: 1751
		Unknown = 85,
		// Token: 0x040006D8 RID: 1752
		Eof = 69
	}
}
