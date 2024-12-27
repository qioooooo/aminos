using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003CA RID: 970
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct DTFIUserOverrideValues
	{
		// Token: 0x0400132A RID: 4906
		internal string shortDatePattern;

		// Token: 0x0400132B RID: 4907
		internal string longDatePattern;

		// Token: 0x0400132C RID: 4908
		internal string yearMonthPattern;

		// Token: 0x0400132D RID: 4909
		internal string amDesignator;

		// Token: 0x0400132E RID: 4910
		internal string pmDesignator;

		// Token: 0x0400132F RID: 4911
		internal string longTimePattern;

		// Token: 0x04001330 RID: 4912
		internal int firstDayOfWeek;

		// Token: 0x04001331 RID: 4913
		internal int padding1;

		// Token: 0x04001332 RID: 4914
		internal int calendarWeekRule;

		// Token: 0x04001333 RID: 4915
		internal int padding2;
	}
}
