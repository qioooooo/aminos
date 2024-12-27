using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000154 RID: 340
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class UInt32Converter : BaseNumberConverter
	{
		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x00027D09 File Offset: 0x00026D09
		internal override Type TargetType
		{
			get
			{
				return typeof(uint);
			}
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00027D15 File Offset: 0x00026D15
		internal override object FromString(string value, int radix)
		{
			return Convert.ToUInt32(value, radix);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00027D23 File Offset: 0x00026D23
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return uint.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00027D32 File Offset: 0x00026D32
		internal override object FromString(string value, CultureInfo culture)
		{
			return uint.Parse(value, culture);
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00027D40 File Offset: 0x00026D40
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((uint)value).ToString("G", formatInfo);
		}
	}
}
