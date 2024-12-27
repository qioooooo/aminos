using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200013C RID: 316
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class SingleConverter : BaseNumberConverter
	{
		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x00023E59 File Offset: 0x00022E59
		internal override bool AllowHex
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x00023E5C File Offset: 0x00022E5C
		internal override Type TargetType
		{
			get
			{
				return typeof(float);
			}
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00023E68 File Offset: 0x00022E68
		internal override object FromString(string value, int radix)
		{
			return Convert.ToSingle(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00023E7A File Offset: 0x00022E7A
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return float.Parse(value, NumberStyles.Float, formatInfo);
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00023E8D File Offset: 0x00022E8D
		internal override object FromString(string value, CultureInfo culture)
		{
			return float.Parse(value, culture);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00023E9C File Offset: 0x00022E9C
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((float)value).ToString("R", formatInfo);
		}
	}
}
