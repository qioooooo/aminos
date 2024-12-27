using System;

namespace System
{
	// Token: 0x0200038A RID: 906
	internal struct DateTimeRawInfo
	{
		// Token: 0x060024C2 RID: 9410 RVA: 0x00065088 File Offset: 0x00064088
		internal unsafe void Init(int* numberBuffer)
		{
			this.month = -1;
			this.year = -1;
			this.dayOfWeek = -1;
			this.era = -1;
			this.timeMark = DateTimeParse.TM.NotSet;
			this.fraction = -1.0;
			this.num = numberBuffer;
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x000650C4 File Offset: 0x000640C4
		internal unsafe void AddNumber(int value)
		{
			this.num[(IntPtr)(this.numCount++) * 4] = value;
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x000650EE File Offset: 0x000640EE
		internal unsafe int GetNumber(int index)
		{
			return this.num[index];
		}

		// Token: 0x04000FBA RID: 4026
		private unsafe int* num;

		// Token: 0x04000FBB RID: 4027
		internal int numCount;

		// Token: 0x04000FBC RID: 4028
		internal int month;

		// Token: 0x04000FBD RID: 4029
		internal int year;

		// Token: 0x04000FBE RID: 4030
		internal int dayOfWeek;

		// Token: 0x04000FBF RID: 4031
		internal int era;

		// Token: 0x04000FC0 RID: 4032
		internal DateTimeParse.TM timeMark;

		// Token: 0x04000FC1 RID: 4033
		internal double fraction;

		// Token: 0x04000FC2 RID: 4034
		internal bool timeZone;
	}
}
