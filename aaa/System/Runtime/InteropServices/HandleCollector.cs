using System;
using System.Threading;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000215 RID: 533
	public sealed class HandleCollector
	{
		// Token: 0x06001208 RID: 4616 RVA: 0x0003CCE8 File Offset: 0x0003BCE8
		public HandleCollector(string name, int initialThreshold)
			: this(name, initialThreshold, int.MaxValue)
		{
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x0003CCF8 File Offset: 0x0003BCF8
		public HandleCollector(string name, int initialThreshold, int maximumThreshold)
		{
			if (initialThreshold < 0)
			{
				throw new ArgumentOutOfRangeException("initialThreshold", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (maximumThreshold < 0)
			{
				throw new ArgumentOutOfRangeException("maximumThreshold", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (initialThreshold > maximumThreshold)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidThreshold"));
			}
			if (name != null)
			{
				this.name = name;
			}
			else
			{
				this.name = string.Empty;
			}
			this.initialThreshold = initialThreshold;
			this.maximumThreshold = maximumThreshold;
			this.threshold = initialThreshold;
			this.handleCount = 0;
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x0003CD90 File Offset: 0x0003BD90
		public int Count
		{
			get
			{
				return this.handleCount;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x0600120B RID: 4619 RVA: 0x0003CD98 File Offset: 0x0003BD98
		public int InitialThreshold
		{
			get
			{
				return this.initialThreshold;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x0600120C RID: 4620 RVA: 0x0003CDA0 File Offset: 0x0003BDA0
		public int MaximumThreshold
		{
			get
			{
				return this.maximumThreshold;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x0003CDA8 File Offset: 0x0003BDA8
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0003CDB0 File Offset: 0x0003BDB0
		public void Add()
		{
			int num = -1;
			Interlocked.Increment(ref this.handleCount);
			if (this.handleCount < 0)
			{
				throw new InvalidOperationException(SR.GetString("InvalidOperation_HCCountOverflow"));
			}
			if (this.handleCount > this.threshold)
			{
				lock (this)
				{
					this.threshold = this.handleCount + this.handleCount / 10;
					num = this.gc_gen;
					if (this.gc_gen < 2)
					{
						this.gc_gen++;
					}
				}
			}
			if (num >= 0 && (num == 0 || this.gc_counts[num] == GC.CollectionCount(num)))
			{
				GC.Collect(num);
				Thread.Sleep(10 * num);
			}
			for (int i = 1; i < 3; i++)
			{
				this.gc_counts[i] = GC.CollectionCount(i);
			}
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0003CE88 File Offset: 0x0003BE88
		public void Remove()
		{
			Interlocked.Decrement(ref this.handleCount);
			if (this.handleCount < 0)
			{
				throw new InvalidOperationException(SR.GetString("InvalidOperation_HCCountOverflow"));
			}
			int num = this.handleCount + this.handleCount / 10;
			if (num < this.threshold - this.threshold / 10)
			{
				lock (this)
				{
					if (num > this.initialThreshold)
					{
						this.threshold = num;
					}
					else
					{
						this.threshold = this.initialThreshold;
					}
					this.gc_gen = 0;
				}
			}
			for (int i = 1; i < 3; i++)
			{
				this.gc_counts[i] = GC.CollectionCount(i);
			}
		}

		// Token: 0x0400107A RID: 4218
		private const int deltaPercent = 10;

		// Token: 0x0400107B RID: 4219
		private string name;

		// Token: 0x0400107C RID: 4220
		private int initialThreshold;

		// Token: 0x0400107D RID: 4221
		private int maximumThreshold;

		// Token: 0x0400107E RID: 4222
		private int threshold;

		// Token: 0x0400107F RID: 4223
		private int handleCount;

		// Token: 0x04001080 RID: 4224
		private int[] gc_counts = new int[3];

		// Token: 0x04001081 RID: 4225
		private int gc_gen;
	}
}
