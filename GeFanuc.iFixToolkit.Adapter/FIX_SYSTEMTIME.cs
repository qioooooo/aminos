using System;

namespace GeFanuc.iFixToolkit.Adapter
{
	public struct FIX_SYSTEMTIME
	{
		public short wYear;

		public short wMonth;

		public short wDayOfWeek;

		public short wDay;

		public short wHour;

		public short wMinute;

		public short wSecond;

		public short wMilliseconds;

		public int ulReserved1;

		public int ulReserved2;
	}
}
