using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x0200041B RID: 1051
	internal class HttpProtocolUtils
	{
		// Token: 0x060020DC RID: 8412 RVA: 0x0008132D File Offset: 0x0008032D
		private HttpProtocolUtils()
		{
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x00081338 File Offset: 0x00080338
		internal static DateTime string2date(string S)
		{
			DateTime dateTime;
			if (HttpDateParse.ParseHttpDate(S, out dateTime))
			{
				return dateTime;
			}
			throw new ProtocolViolationException(SR.GetString("net_baddate"));
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x00081360 File Offset: 0x00080360
		internal static string date2string(DateTime D)
		{
			DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
			return D.ToUniversalTime().ToString("R", dateTimeFormatInfo);
		}
	}
}
