using System;
using System.Collections;
using System.Globalization;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000017 RID: 23
	internal sealed class RegexCode
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x000064EC File Offset: 0x000054EC
		internal RegexCode(int[] codes, ArrayList stringlist, int trackcount, Hashtable caps, int capsize, RegexBoyerMoore bmPrefix, RegexPrefix fcPrefix, int anchors, bool rightToLeft)
		{
			this._codes = codes;
			this._strings = new string[stringlist.Count];
			this._trackcount = trackcount;
			this._caps = caps;
			this._capsize = capsize;
			this._bmPrefix = bmPrefix;
			this._fcPrefix = fcPrefix;
			this._anchors = anchors;
			this._rightToLeft = rightToLeft;
			stringlist.CopyTo(0, this._strings, 0, stringlist.Count);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00006564 File Offset: 0x00005564
		internal static bool OpcodeBacktracks(int Op)
		{
			Op &= 63;
			switch (Op)
			{
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 23:
			case 24:
			case 25:
			case 26:
			case 27:
			case 28:
			case 29:
			case 31:
			case 32:
			case 33:
			case 34:
			case 35:
			case 36:
			case 38:
				return true;
			}
			return false;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00006618 File Offset: 0x00005618
		internal static int OpcodeSize(int Opcode)
		{
			Opcode &= 63;
			switch (Opcode)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 28:
			case 29:
			case 32:
				return 3;
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 23:
			case 24:
			case 25:
			case 26:
			case 27:
			case 37:
			case 38:
			case 39:
				return 2;
			case 14:
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
			case 22:
			case 30:
			case 31:
			case 33:
			case 34:
			case 35:
			case 36:
			case 40:
			case 41:
			case 42:
				return 1;
			default:
				throw RegexCode.MakeException(SR.GetString("UnexpectedOpcode", new object[] { Opcode.ToString(CultureInfo.CurrentCulture) }));
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000670D File Offset: 0x0000570D
		internal static ArgumentException MakeException(string message)
		{
			return new ArgumentException(message);
		}

		// Token: 0x04000687 RID: 1671
		internal const int Onerep = 0;

		// Token: 0x04000688 RID: 1672
		internal const int Notonerep = 1;

		// Token: 0x04000689 RID: 1673
		internal const int Setrep = 2;

		// Token: 0x0400068A RID: 1674
		internal const int Oneloop = 3;

		// Token: 0x0400068B RID: 1675
		internal const int Notoneloop = 4;

		// Token: 0x0400068C RID: 1676
		internal const int Setloop = 5;

		// Token: 0x0400068D RID: 1677
		internal const int Onelazy = 6;

		// Token: 0x0400068E RID: 1678
		internal const int Notonelazy = 7;

		// Token: 0x0400068F RID: 1679
		internal const int Setlazy = 8;

		// Token: 0x04000690 RID: 1680
		internal const int One = 9;

		// Token: 0x04000691 RID: 1681
		internal const int Notone = 10;

		// Token: 0x04000692 RID: 1682
		internal const int Set = 11;

		// Token: 0x04000693 RID: 1683
		internal const int Multi = 12;

		// Token: 0x04000694 RID: 1684
		internal const int Ref = 13;

		// Token: 0x04000695 RID: 1685
		internal const int Bol = 14;

		// Token: 0x04000696 RID: 1686
		internal const int Eol = 15;

		// Token: 0x04000697 RID: 1687
		internal const int Boundary = 16;

		// Token: 0x04000698 RID: 1688
		internal const int Nonboundary = 17;

		// Token: 0x04000699 RID: 1689
		internal const int Beginning = 18;

		// Token: 0x0400069A RID: 1690
		internal const int Start = 19;

		// Token: 0x0400069B RID: 1691
		internal const int EndZ = 20;

		// Token: 0x0400069C RID: 1692
		internal const int End = 21;

		// Token: 0x0400069D RID: 1693
		internal const int Nothing = 22;

		// Token: 0x0400069E RID: 1694
		internal const int Lazybranch = 23;

		// Token: 0x0400069F RID: 1695
		internal const int Branchmark = 24;

		// Token: 0x040006A0 RID: 1696
		internal const int Lazybranchmark = 25;

		// Token: 0x040006A1 RID: 1697
		internal const int Nullcount = 26;

		// Token: 0x040006A2 RID: 1698
		internal const int Setcount = 27;

		// Token: 0x040006A3 RID: 1699
		internal const int Branchcount = 28;

		// Token: 0x040006A4 RID: 1700
		internal const int Lazybranchcount = 29;

		// Token: 0x040006A5 RID: 1701
		internal const int Nullmark = 30;

		// Token: 0x040006A6 RID: 1702
		internal const int Setmark = 31;

		// Token: 0x040006A7 RID: 1703
		internal const int Capturemark = 32;

		// Token: 0x040006A8 RID: 1704
		internal const int Getmark = 33;

		// Token: 0x040006A9 RID: 1705
		internal const int Setjump = 34;

		// Token: 0x040006AA RID: 1706
		internal const int Backjump = 35;

		// Token: 0x040006AB RID: 1707
		internal const int Forejump = 36;

		// Token: 0x040006AC RID: 1708
		internal const int Testref = 37;

		// Token: 0x040006AD RID: 1709
		internal const int Goto = 38;

		// Token: 0x040006AE RID: 1710
		internal const int Prune = 39;

		// Token: 0x040006AF RID: 1711
		internal const int Stop = 40;

		// Token: 0x040006B0 RID: 1712
		internal const int ECMABoundary = 41;

		// Token: 0x040006B1 RID: 1713
		internal const int NonECMABoundary = 42;

		// Token: 0x040006B2 RID: 1714
		internal const int Mask = 63;

		// Token: 0x040006B3 RID: 1715
		internal const int Rtl = 64;

		// Token: 0x040006B4 RID: 1716
		internal const int Back = 128;

		// Token: 0x040006B5 RID: 1717
		internal const int Back2 = 256;

		// Token: 0x040006B6 RID: 1718
		internal const int Ci = 512;

		// Token: 0x040006B7 RID: 1719
		internal int[] _codes;

		// Token: 0x040006B8 RID: 1720
		internal string[] _strings;

		// Token: 0x040006B9 RID: 1721
		internal int _trackcount;

		// Token: 0x040006BA RID: 1722
		internal Hashtable _caps;

		// Token: 0x040006BB RID: 1723
		internal int _capsize;

		// Token: 0x040006BC RID: 1724
		internal RegexPrefix _fcPrefix;

		// Token: 0x040006BD RID: 1725
		internal RegexBoyerMoore _bmPrefix;

		// Token: 0x040006BE RID: 1726
		internal int _anchors;

		// Token: 0x040006BF RID: 1727
		internal bool _rightToLeft;
	}
}
