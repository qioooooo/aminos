using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000103 RID: 259
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class SystemTime
	{
		// Token: 0x0400063D RID: 1597
		public ushort wYear;

		// Token: 0x0400063E RID: 1598
		public ushort wMonth;

		// Token: 0x0400063F RID: 1599
		public ushort wDayOfWeek;

		// Token: 0x04000640 RID: 1600
		public ushort wDay;

		// Token: 0x04000641 RID: 1601
		public ushort wHour;

		// Token: 0x04000642 RID: 1602
		public ushort wMinute;

		// Token: 0x04000643 RID: 1603
		public ushort wSecond;

		// Token: 0x04000644 RID: 1604
		public ushort wMilliseconds;
	}
}
