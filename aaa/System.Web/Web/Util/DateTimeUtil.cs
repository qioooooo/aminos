using System;

namespace System.Web.Util
{
	// Token: 0x0200075B RID: 1883
	internal sealed class DateTimeUtil
	{
		// Token: 0x06005BB2 RID: 23474 RVA: 0x00170585 File Offset: 0x0016F585
		private DateTimeUtil()
		{
		}

		// Token: 0x06005BB3 RID: 23475 RVA: 0x00170590 File Offset: 0x0016F590
		internal static DateTime FromFileTimeToUtc(long filetime)
		{
			long num = filetime + 504911232000000000L;
			return new DateTime(num);
		}

		// Token: 0x06005BB4 RID: 23476 RVA: 0x001705AF File Offset: 0x0016F5AF
		internal static DateTime ConvertToUniversalTime(DateTime localTime)
		{
			if (localTime < DateTimeUtil.MinValuePlusOneDay)
			{
				return DateTime.MinValue;
			}
			if (localTime > DateTimeUtil.MaxValueMinusOneDay)
			{
				return DateTime.MaxValue;
			}
			return localTime.ToUniversalTime();
		}

		// Token: 0x06005BB5 RID: 23477 RVA: 0x001705DE File Offset: 0x0016F5DE
		internal static DateTime ConvertToLocalTime(DateTime utcTime)
		{
			if (utcTime < DateTimeUtil.MinValuePlusOneDay)
			{
				return DateTime.MinValue;
			}
			if (utcTime > DateTimeUtil.MaxValueMinusOneDay)
			{
				return DateTime.MaxValue;
			}
			return utcTime.ToLocalTime();
		}

		// Token: 0x04003118 RID: 12568
		private const long FileTimeOffset = 504911232000000000L;

		// Token: 0x04003119 RID: 12569
		private static readonly DateTime MinValuePlusOneDay = DateTime.MinValue.AddDays(1.0);

		// Token: 0x0400311A RID: 12570
		private static readonly DateTime MaxValueMinusOneDay = DateTime.MaxValue.AddDays(-1.0);
	}
}
