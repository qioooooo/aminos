using System;

namespace GeFanuc.iFixToolkit.Adapter
{
	// Token: 0x02000002 RID: 2
	public struct FIX_SYSTEMTIME
	{
		// Token: 0x04000001 RID: 1
		public short wYear;

		// Token: 0x04000002 RID: 2
		public short wMonth;

		// Token: 0x04000003 RID: 3
		public short wDayOfWeek;

		// Token: 0x04000004 RID: 4
		public short wDay;

		// Token: 0x04000005 RID: 5
		public short wHour;

		// Token: 0x04000006 RID: 6
		public short wMinute;

		// Token: 0x04000007 RID: 7
		public short wSecond;

		// Token: 0x04000008 RID: 8
		public short wMilliseconds;

		// Token: 0x04000009 RID: 9
		public int ulReserved1;

		// Token: 0x0400000A RID: 10
		public int ulReserved2;
	}
}
