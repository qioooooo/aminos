using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000155 RID: 341
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class UInt64Converter : BaseNumberConverter
	{
		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x00027D69 File Offset: 0x00026D69
		internal override Type TargetType
		{
			get
			{
				return typeof(ulong);
			}
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00027D75 File Offset: 0x00026D75
		internal override object FromString(string value, int radix)
		{
			return Convert.ToUInt64(value, radix);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00027D83 File Offset: 0x00026D83
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return ulong.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00027D92 File Offset: 0x00026D92
		internal override object FromString(string value, CultureInfo culture)
		{
			return ulong.Parse(value, culture);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00027DA0 File Offset: 0x00026DA0
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((ulong)value).ToString("G", formatInfo);
		}
	}
}
