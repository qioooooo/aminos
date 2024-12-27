using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000153 RID: 339
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class UInt16Converter : BaseNumberConverter
	{
		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x00027CA7 File Offset: 0x00026CA7
		internal override Type TargetType
		{
			get
			{
				return typeof(ushort);
			}
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00027CB3 File Offset: 0x00026CB3
		internal override object FromString(string value, int radix)
		{
			return Convert.ToUInt16(value, radix);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00027CC1 File Offset: 0x00026CC1
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return ushort.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00027CD0 File Offset: 0x00026CD0
		internal override object FromString(string value, CultureInfo culture)
		{
			return ushort.Parse(value, culture);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00027CE0 File Offset: 0x00026CE0
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((ushort)value).ToString("G", formatInfo);
		}
	}
}
