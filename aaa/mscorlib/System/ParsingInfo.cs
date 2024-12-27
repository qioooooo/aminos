using System;
using System.Globalization;

namespace System
{
	// Token: 0x0200038E RID: 910
	internal struct ParsingInfo
	{
		// Token: 0x060024C9 RID: 9417 RVA: 0x00065176 File Offset: 0x00064176
		internal void Init()
		{
			this.dayOfWeek = -1;
			this.timeMark = DateTimeParse.TM.NotSet;
		}

		// Token: 0x04000FE9 RID: 4073
		internal Calendar calendar;

		// Token: 0x04000FEA RID: 4074
		internal int dayOfWeek;

		// Token: 0x04000FEB RID: 4075
		internal DateTimeParse.TM timeMark;

		// Token: 0x04000FEC RID: 4076
		internal bool fUseHour12;

		// Token: 0x04000FED RID: 4077
		internal bool fUseTwoDigitYear;

		// Token: 0x04000FEE RID: 4078
		internal bool fAllowInnerWhite;

		// Token: 0x04000FEF RID: 4079
		internal bool fAllowTrailingWhite;

		// Token: 0x04000FF0 RID: 4080
		internal bool fCustomNumberParser;

		// Token: 0x04000FF1 RID: 4081
		internal DateTimeParse.MatchNumberDelegate parseNumberDelegate;
	}
}
