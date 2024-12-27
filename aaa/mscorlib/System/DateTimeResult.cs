using System;
using System.Globalization;

namespace System
{
	// Token: 0x0200038D RID: 909
	internal struct DateTimeResult
	{
		// Token: 0x060024C5 RID: 9413 RVA: 0x000650FC File Offset: 0x000640FC
		internal void Init()
		{
			this.Year = -1;
			this.Month = -1;
			this.Day = -1;
			this.fraction = -1.0;
			this.era = -1;
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x00065129 File Offset: 0x00064129
		internal void SetDate(int year, int month, int day)
		{
			this.Year = year;
			this.Month = month;
			this.Day = day;
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x00065140 File Offset: 0x00064140
		internal void SetFailure(ParseFailureKind failure, string failureMessageID, object failureMessageFormatArgument)
		{
			this.failure = failure;
			this.failureMessageID = failureMessageID;
			this.failureMessageFormatArgument = failureMessageFormatArgument;
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x00065157 File Offset: 0x00064157
		internal void SetFailure(ParseFailureKind failure, string failureMessageID, object failureMessageFormatArgument, string failureArgumentName)
		{
			this.failure = failure;
			this.failureMessageID = failureMessageID;
			this.failureMessageFormatArgument = failureMessageFormatArgument;
			this.failureArgumentName = failureArgumentName;
		}

		// Token: 0x04000FD9 RID: 4057
		internal int Year;

		// Token: 0x04000FDA RID: 4058
		internal int Month;

		// Token: 0x04000FDB RID: 4059
		internal int Day;

		// Token: 0x04000FDC RID: 4060
		internal int Hour;

		// Token: 0x04000FDD RID: 4061
		internal int Minute;

		// Token: 0x04000FDE RID: 4062
		internal int Second;

		// Token: 0x04000FDF RID: 4063
		internal double fraction;

		// Token: 0x04000FE0 RID: 4064
		internal int era;

		// Token: 0x04000FE1 RID: 4065
		internal ParseFlags flags;

		// Token: 0x04000FE2 RID: 4066
		internal TimeSpan timeZoneOffset;

		// Token: 0x04000FE3 RID: 4067
		internal Calendar calendar;

		// Token: 0x04000FE4 RID: 4068
		internal DateTime parsedDate;

		// Token: 0x04000FE5 RID: 4069
		internal ParseFailureKind failure;

		// Token: 0x04000FE6 RID: 4070
		internal string failureMessageID;

		// Token: 0x04000FE7 RID: 4071
		internal object failureMessageFormatArgument;

		// Token: 0x04000FE8 RID: 4072
		internal string failureArgumentName;
	}
}
