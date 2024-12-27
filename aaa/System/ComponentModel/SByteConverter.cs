using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200013A RID: 314
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class SByteConverter : BaseNumberConverter
	{
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x00023D94 File Offset: 0x00022D94
		internal override Type TargetType
		{
			get
			{
				return typeof(sbyte);
			}
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00023DA0 File Offset: 0x00022DA0
		internal override object FromString(string value, int radix)
		{
			return Convert.ToSByte(value, radix);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00023DAE File Offset: 0x00022DAE
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return sbyte.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00023DBD File Offset: 0x00022DBD
		internal override object FromString(string value, CultureInfo culture)
		{
			return sbyte.Parse(value, culture);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00023DCC File Offset: 0x00022DCC
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((sbyte)value).ToString("G", formatInfo);
		}
	}
}
