using System;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000799 RID: 1945
	public class Stopwatch
	{
		// Token: 0x06003BFB RID: 15355 RVA: 0x001009EC File Offset: 0x000FF9EC
		static Stopwatch()
		{
			if (!SafeNativeMethods.QueryPerformanceFrequency(out Stopwatch.Frequency))
			{
				Stopwatch.IsHighResolution = false;
				Stopwatch.Frequency = 10000000L;
				Stopwatch.tickFrequency = 1.0;
				return;
			}
			Stopwatch.IsHighResolution = true;
			Stopwatch.tickFrequency = 10000000.0;
			Stopwatch.tickFrequency /= (double)Stopwatch.Frequency;
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x00100A4C File Offset: 0x000FFA4C
		public Stopwatch()
		{
			this.Reset();
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x00100A5A File Offset: 0x000FFA5A
		public void Start()
		{
			if (!this.isRunning)
			{
				this.startTimeStamp = Stopwatch.GetTimestamp();
				this.isRunning = true;
			}
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x00100A78 File Offset: 0x000FFA78
		public static Stopwatch StartNew()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			return stopwatch;
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x00100A94 File Offset: 0x000FFA94
		public void Stop()
		{
			if (this.isRunning)
			{
				long timestamp = Stopwatch.GetTimestamp();
				long num = timestamp - this.startTimeStamp;
				this.elapsed += num;
				this.isRunning = false;
			}
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x00100ACD File Offset: 0x000FFACD
		public void Reset()
		{
			this.elapsed = 0L;
			this.isRunning = false;
			this.startTimeStamp = 0L;
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06003C01 RID: 15361 RVA: 0x00100AE6 File Offset: 0x000FFAE6
		public bool IsRunning
		{
			get
			{
				return this.isRunning;
			}
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06003C02 RID: 15362 RVA: 0x00100AEE File Offset: 0x000FFAEE
		public TimeSpan Elapsed
		{
			get
			{
				return new TimeSpan(this.GetElapsedDateTimeTicks());
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06003C03 RID: 15363 RVA: 0x00100AFB File Offset: 0x000FFAFB
		public long ElapsedMilliseconds
		{
			get
			{
				return this.GetElapsedDateTimeTicks() / 10000L;
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06003C04 RID: 15364 RVA: 0x00100B0A File Offset: 0x000FFB0A
		public long ElapsedTicks
		{
			get
			{
				return this.GetRawElapsedTicks();
			}
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x00100B14 File Offset: 0x000FFB14
		public static long GetTimestamp()
		{
			if (Stopwatch.IsHighResolution)
			{
				long num = 0L;
				SafeNativeMethods.QueryPerformanceCounter(out num);
				return num;
			}
			return DateTime.UtcNow.Ticks;
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x00100B44 File Offset: 0x000FFB44
		private long GetRawElapsedTicks()
		{
			long num = this.elapsed;
			if (this.isRunning)
			{
				long timestamp = Stopwatch.GetTimestamp();
				long num2 = timestamp - this.startTimeStamp;
				num += num2;
			}
			return num;
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x00100B74 File Offset: 0x000FFB74
		private long GetElapsedDateTimeTicks()
		{
			long rawElapsedTicks = this.GetRawElapsedTicks();
			if (Stopwatch.IsHighResolution)
			{
				double num = (double)rawElapsedTicks;
				num *= Stopwatch.tickFrequency;
				return (long)num;
			}
			return rawElapsedTicks;
		}

		// Token: 0x04003492 RID: 13458
		private const long TicksPerMillisecond = 10000L;

		// Token: 0x04003493 RID: 13459
		private const long TicksPerSecond = 10000000L;

		// Token: 0x04003494 RID: 13460
		private long elapsed;

		// Token: 0x04003495 RID: 13461
		private long startTimeStamp;

		// Token: 0x04003496 RID: 13462
		private bool isRunning;

		// Token: 0x04003497 RID: 13463
		public static readonly long Frequency;

		// Token: 0x04003498 RID: 13464
		public static readonly bool IsHighResolution;

		// Token: 0x04003499 RID: 13465
		private static readonly double tickFrequency;
	}
}
