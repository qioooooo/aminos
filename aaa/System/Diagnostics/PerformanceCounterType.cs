using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x02000773 RID: 1907
	[TypeConverter(typeof(AlphabeticalEnumConverter))]
	public enum PerformanceCounterType
	{
		// Token: 0x04003353 RID: 13139
		NumberOfItems32 = 65536,
		// Token: 0x04003354 RID: 13140
		NumberOfItems64 = 65792,
		// Token: 0x04003355 RID: 13141
		NumberOfItemsHEX32 = 0,
		// Token: 0x04003356 RID: 13142
		NumberOfItemsHEX64 = 256,
		// Token: 0x04003357 RID: 13143
		RateOfCountsPerSecond32 = 272696320,
		// Token: 0x04003358 RID: 13144
		RateOfCountsPerSecond64 = 272696576,
		// Token: 0x04003359 RID: 13145
		CountPerTimeInterval32 = 4523008,
		// Token: 0x0400335A RID: 13146
		CountPerTimeInterval64 = 4523264,
		// Token: 0x0400335B RID: 13147
		RawFraction = 537003008,
		// Token: 0x0400335C RID: 13148
		RawBase = 1073939459,
		// Token: 0x0400335D RID: 13149
		AverageTimer32 = 805438464,
		// Token: 0x0400335E RID: 13150
		AverageBase = 1073939458,
		// Token: 0x0400335F RID: 13151
		AverageCount64 = 1073874176,
		// Token: 0x04003360 RID: 13152
		SampleFraction = 549585920,
		// Token: 0x04003361 RID: 13153
		SampleCounter = 4260864,
		// Token: 0x04003362 RID: 13154
		SampleBase = 1073939457,
		// Token: 0x04003363 RID: 13155
		CounterTimer = 541132032,
		// Token: 0x04003364 RID: 13156
		CounterTimerInverse = 557909248,
		// Token: 0x04003365 RID: 13157
		Timer100Ns = 542180608,
		// Token: 0x04003366 RID: 13158
		Timer100NsInverse = 558957824,
		// Token: 0x04003367 RID: 13159
		ElapsedTime = 807666944,
		// Token: 0x04003368 RID: 13160
		CounterMultiTimer = 574686464,
		// Token: 0x04003369 RID: 13161
		CounterMultiTimerInverse = 591463680,
		// Token: 0x0400336A RID: 13162
		CounterMultiTimer100Ns = 575735040,
		// Token: 0x0400336B RID: 13163
		CounterMultiTimer100NsInverse = 592512256,
		// Token: 0x0400336C RID: 13164
		CounterMultiBase = 1107494144,
		// Token: 0x0400336D RID: 13165
		CounterDelta32 = 4195328,
		// Token: 0x0400336E RID: 13166
		CounterDelta64 = 4195584
	}
}
