using System;

namespace System.Diagnostics
{
	// Token: 0x02000744 RID: 1860
	public struct CounterSample
	{
		// Token: 0x060038B4 RID: 14516 RVA: 0x000EF3E5 File Offset: 0x000EE3E5
		public CounterSample(long rawValue, long baseValue, long counterFrequency, long systemFrequency, long timeStamp, long timeStamp100nSec, PerformanceCounterType counterType)
		{
			this.rawValue = rawValue;
			this.baseValue = baseValue;
			this.timeStamp = timeStamp;
			this.counterFrequency = counterFrequency;
			this.counterType = counterType;
			this.timeStamp100nSec = timeStamp100nSec;
			this.systemFrequency = systemFrequency;
			this.counterTimeStamp = 0L;
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x000EF424 File Offset: 0x000EE424
		public CounterSample(long rawValue, long baseValue, long counterFrequency, long systemFrequency, long timeStamp, long timeStamp100nSec, PerformanceCounterType counterType, long counterTimeStamp)
		{
			this.rawValue = rawValue;
			this.baseValue = baseValue;
			this.timeStamp = timeStamp;
			this.counterFrequency = counterFrequency;
			this.counterType = counterType;
			this.timeStamp100nSec = timeStamp100nSec;
			this.systemFrequency = systemFrequency;
			this.counterTimeStamp = counterTimeStamp;
		}

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x060038B6 RID: 14518 RVA: 0x000EF463 File Offset: 0x000EE463
		public long RawValue
		{
			get
			{
				return this.rawValue;
			}
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x060038B7 RID: 14519 RVA: 0x000EF46B File Offset: 0x000EE46B
		internal ulong UnsignedRawValue
		{
			get
			{
				return (ulong)this.rawValue;
			}
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x060038B8 RID: 14520 RVA: 0x000EF473 File Offset: 0x000EE473
		public long BaseValue
		{
			get
			{
				return this.baseValue;
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x060038B9 RID: 14521 RVA: 0x000EF47B File Offset: 0x000EE47B
		public long SystemFrequency
		{
			get
			{
				return this.systemFrequency;
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x060038BA RID: 14522 RVA: 0x000EF483 File Offset: 0x000EE483
		public long CounterFrequency
		{
			get
			{
				return this.counterFrequency;
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x060038BB RID: 14523 RVA: 0x000EF48B File Offset: 0x000EE48B
		public long CounterTimeStamp
		{
			get
			{
				return this.counterTimeStamp;
			}
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x060038BC RID: 14524 RVA: 0x000EF493 File Offset: 0x000EE493
		public long TimeStamp
		{
			get
			{
				return this.timeStamp;
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x060038BD RID: 14525 RVA: 0x000EF49B File Offset: 0x000EE49B
		public long TimeStamp100nSec
		{
			get
			{
				return this.timeStamp100nSec;
			}
		}

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x060038BE RID: 14526 RVA: 0x000EF4A3 File Offset: 0x000EE4A3
		public PerformanceCounterType CounterType
		{
			get
			{
				return this.counterType;
			}
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x000EF4AB File Offset: 0x000EE4AB
		public static float Calculate(CounterSample counterSample)
		{
			return CounterSampleCalculator.ComputeCounterValue(counterSample);
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x000EF4B3 File Offset: 0x000EE4B3
		public static float Calculate(CounterSample counterSample, CounterSample nextCounterSample)
		{
			return CounterSampleCalculator.ComputeCounterValue(counterSample, nextCounterSample);
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x000EF4BC File Offset: 0x000EE4BC
		public override bool Equals(object o)
		{
			return o is CounterSample && this.Equals((CounterSample)o);
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x000EF4D4 File Offset: 0x000EE4D4
		public bool Equals(CounterSample sample)
		{
			return this.rawValue == sample.rawValue && this.baseValue == sample.baseValue && this.timeStamp == sample.timeStamp && this.counterFrequency == sample.counterFrequency && this.counterType == sample.counterType && this.timeStamp100nSec == sample.timeStamp100nSec && this.systemFrequency == sample.systemFrequency && this.counterTimeStamp == sample.counterTimeStamp;
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x000EF55B File Offset: 0x000EE55B
		public override int GetHashCode()
		{
			return this.rawValue.GetHashCode();
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x000EF568 File Offset: 0x000EE568
		public static bool operator ==(CounterSample a, CounterSample b)
		{
			return a.Equals(b);
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x000EF572 File Offset: 0x000EE572
		public static bool operator !=(CounterSample a, CounterSample b)
		{
			return !a.Equals(b);
		}

		// Token: 0x0400325D RID: 12893
		private long rawValue;

		// Token: 0x0400325E RID: 12894
		private long baseValue;

		// Token: 0x0400325F RID: 12895
		private long timeStamp;

		// Token: 0x04003260 RID: 12896
		private long counterFrequency;

		// Token: 0x04003261 RID: 12897
		private PerformanceCounterType counterType;

		// Token: 0x04003262 RID: 12898
		private long timeStamp100nSec;

		// Token: 0x04003263 RID: 12899
		private long systemFrequency;

		// Token: 0x04003264 RID: 12900
		private long counterTimeStamp;

		// Token: 0x04003265 RID: 12901
		public static CounterSample Empty = new CounterSample(0L, 0L, 0L, 0L, 0L, 0L, PerformanceCounterType.NumberOfItems32);
	}
}
