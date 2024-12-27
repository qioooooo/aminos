using System;
using System.Globalization;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000073 RID: 115
	internal class DecimalFormat
	{
		// Token: 0x060006E4 RID: 1764 RVA: 0x00024CA0 File Offset: 0x00023CA0
		internal DecimalFormat(NumberFormatInfo info, char digit, char zeroDigit, char patternSeparator)
		{
			this.info = info;
			this.digit = digit;
			this.zeroDigit = zeroDigit;
			this.patternSeparator = patternSeparator;
		}

		// Token: 0x04000452 RID: 1106
		public NumberFormatInfo info;

		// Token: 0x04000453 RID: 1107
		public char digit;

		// Token: 0x04000454 RID: 1108
		public char zeroDigit;

		// Token: 0x04000455 RID: 1109
		public char patternSeparator;
	}
}
