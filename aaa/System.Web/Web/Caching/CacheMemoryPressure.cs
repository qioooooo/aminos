using System;
using System.Web.Configuration;

namespace System.Web.Caching
{
	// Token: 0x0200010A RID: 266
	internal abstract class CacheMemoryPressure
	{
		// Token: 0x06000C67 RID: 3175 RVA: 0x00031640 File Offset: 0x00030640
		internal static void GetMemoryStatusOnce()
		{
			UnsafeNativeMethods.MEMORYSTATUSEX memorystatusex = default(UnsafeNativeMethods.MEMORYSTATUSEX);
			memorystatusex.Init();
			if (UnsafeNativeMethods.GlobalMemoryStatusEx(ref memorystatusex) != 0)
			{
				CacheMemoryPressure.s_totalPhysical = memorystatusex.ullTotalPhys;
				CacheMemoryPressure.s_totalVirtual = memorystatusex.ullTotalVirtual;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x0003167D File Offset: 0x0003067D
		internal static long TotalPhysical
		{
			get
			{
				return CacheMemoryPressure.s_totalPhysical;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x00031684 File Offset: 0x00030684
		internal static long TotalVirtual
		{
			get
			{
				return CacheMemoryPressure.s_totalVirtual;
			}
		}

		// Token: 0x06000C6A RID: 3178
		protected abstract int GetCurrentPressure();

		// Token: 0x06000C6B RID: 3179 RVA: 0x0003168B File Offset: 0x0003068B
		internal virtual void ReadConfig(CacheSection cacheSection)
		{
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x00031690 File Offset: 0x00030690
		protected void InitHistory()
		{
			int currentPressure = this.GetCurrentPressure();
			this._pressureHist = new int[6];
			for (int i = 0; i < 6; i++)
			{
				this._pressureHist[i] = currentPressure;
				this._pressureTotal += currentPressure;
			}
			this._pressureAvg = currentPressure;
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x000316DC File Offset: 0x000306DC
		internal void Update()
		{
			int currentPressure = this.GetCurrentPressure();
			this._i0 = (this._i0 + 1) % 6;
			this._pressureTotal -= this._pressureHist[this._i0];
			this._pressureTotal += currentPressure;
			this._pressureHist[this._i0] = currentPressure;
			this._pressureAvg = this._pressureTotal / 6;
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x00031744 File Offset: 0x00030744
		internal int PressureLast
		{
			get
			{
				return this._pressureHist[this._i0];
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x00031753 File Offset: 0x00030753
		internal int PressureAvg
		{
			get
			{
				return this._pressureAvg;
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x0003175B File Offset: 0x0003075B
		internal int PressureHigh
		{
			get
			{
				return this._pressureHigh;
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x00031763 File Offset: 0x00030763
		internal int PressureLow
		{
			get
			{
				return this._pressureLow;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x0003176B File Offset: 0x0003076B
		internal int PressureMiddle
		{
			get
			{
				return this._pressureMiddle;
			}
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00031773 File Offset: 0x00030773
		internal bool IsAboveHighPressure()
		{
			return this.PressureLast >= this.PressureHigh;
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x00031786 File Offset: 0x00030786
		internal bool IsAboveMediumPressure()
		{
			return this.PressureLast > this.PressureMiddle;
		}

		// Token: 0x04001429 RID: 5161
		protected const int TERABYTE_SHIFT = 40;

		// Token: 0x0400142A RID: 5162
		protected const long TERABYTE = 1099511627776L;

		// Token: 0x0400142B RID: 5163
		protected const int GIGABYTE_SHIFT = 30;

		// Token: 0x0400142C RID: 5164
		protected const long GIGABYTE = 1073741824L;

		// Token: 0x0400142D RID: 5165
		protected const int MEGABYTE_SHIFT = 20;

		// Token: 0x0400142E RID: 5166
		protected const long MEGABYTE = 1048576L;

		// Token: 0x0400142F RID: 5167
		protected const int KILOBYTE_SHIFT = 10;

		// Token: 0x04001430 RID: 5168
		protected const long KILOBYTE = 1024L;

		// Token: 0x04001431 RID: 5169
		protected const int HISTORY_COUNT = 6;

		// Token: 0x04001432 RID: 5170
		protected int _pressureHigh;

		// Token: 0x04001433 RID: 5171
		protected int _pressureMiddle;

		// Token: 0x04001434 RID: 5172
		protected int _pressureLow;

		// Token: 0x04001435 RID: 5173
		protected int _i0;

		// Token: 0x04001436 RID: 5174
		protected int[] _pressureHist;

		// Token: 0x04001437 RID: 5175
		protected int _pressureTotal;

		// Token: 0x04001438 RID: 5176
		protected int _pressureAvg;

		// Token: 0x04001439 RID: 5177
		private static long s_totalPhysical;

		// Token: 0x0400143A RID: 5178
		private static long s_totalVirtual;
	}
}
