using System;

namespace System.Web.Util
{
	// Token: 0x02000759 RID: 1881
	internal static class CodePageUtils
	{
		// Token: 0x06005BAD RID: 23469 RVA: 0x001703B4 File Offset: 0x0016F3B4
		internal static bool IsAsciiCompatibleCodePage(int codepage)
		{
			int i = 0;
			int num = 78;
			while (i <= num)
			{
				int num2 = i + num >> 1;
				int num3 = CodePageUtils._asciiCompatCodePages[num2] - codepage;
				if (num3 == 0)
				{
					return true;
				}
				if (num3 < 0)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2 - 1;
				}
			}
			return false;
		}

		// Token: 0x04003116 RID: 12566
		internal const int CodePageUT8 = 65001;

		// Token: 0x04003117 RID: 12567
		private static int[] _asciiCompatCodePages = new int[]
		{
			437, 708, 720, 737, 775, 850, 852, 855, 857, 858,
			860, 861, 862, 863, 864, 865, 866, 869, 874, 932,
			936, 949, 950, 1250, 1251, 1252, 1253, 1254, 1255, 1256,
			1257, 1258, 1361, 10000, 10001, 10002, 10003, 10004, 10005, 10006,
			10007, 10008, 10010, 10017, 10029, 10079, 10081, 10082, 20000, 20001,
			20002, 20003, 20004, 20005, 20127, 20866, 20932, 20936, 20949, 21866,
			28591, 28592, 28593, 28594, 28595, 28596, 28597, 28598, 28599, 28605,
			38598, 50220, 50221, 50222, 50225, 50227, 51932, 51949, 65001
		};
	}
}
