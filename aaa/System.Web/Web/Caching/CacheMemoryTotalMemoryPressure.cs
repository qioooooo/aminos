using System;
using System.Web.Configuration;

namespace System.Web.Caching
{
	// Token: 0x0200010B RID: 267
	internal sealed class CacheMemoryTotalMemoryPressure : CacheMemoryPressure
	{
		// Token: 0x06000C76 RID: 3190 RVA: 0x000317A0 File Offset: 0x000307A0
		internal CacheMemoryTotalMemoryPressure()
		{
			long totalPhysical = CacheMemoryPressure.TotalPhysical;
			if (totalPhysical >= 4294967296L)
			{
				this._pressureHigh = 99;
			}
			else if (totalPhysical >= (long)((ulong)(-2147483648)))
			{
				this._pressureHigh = 98;
			}
			else if (totalPhysical >= 1073741824L)
			{
				this._pressureHigh = 97;
			}
			else if (totalPhysical >= 805306368L)
			{
				this._pressureHigh = 96;
			}
			else
			{
				this._pressureHigh = 95;
			}
			this._pressureMiddle = this._pressureHigh - 2;
			this._pressureLow = this._pressureHigh - 9;
			base.InitHistory();
			PerfCounters.SetCounter(AppPerfCounter.CACHE_PERCENT_MACH_MEM_LIMIT_USED_BASE, this._pressureHigh);
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00031840 File Offset: 0x00030840
		internal override void ReadConfig(CacheSection cacheSection)
		{
			int percentagePhysicalMemoryUsedLimit = cacheSection.PercentagePhysicalMemoryUsedLimit;
			if (percentagePhysicalMemoryUsedLimit == 0)
			{
				return;
			}
			this._pressureHigh = Math.Max(3, percentagePhysicalMemoryUsedLimit);
			this._pressureMiddle = Math.Max(2, this._pressureHigh - 2);
			this._pressureLow = Math.Max(1, this._pressureHigh - 9);
			PerfCounters.SetCounter(AppPerfCounter.CACHE_PERCENT_MACH_MEM_LIMIT_USED_BASE, this._pressureHigh);
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x0003189C File Offset: 0x0003089C
		protected override int GetCurrentPressure()
		{
			UnsafeNativeMethods.MEMORYSTATUSEX memorystatusex = default(UnsafeNativeMethods.MEMORYSTATUSEX);
			memorystatusex.Init();
			if (UnsafeNativeMethods.GlobalMemoryStatusEx(ref memorystatusex) == 0)
			{
				return 0;
			}
			int dwMemoryLoad = memorystatusex.dwMemoryLoad;
			if (this._pressureHigh != 0)
			{
				PerfCounters.SetCounter(AppPerfCounter.CACHE_PERCENT_MACH_MEM_LIMIT_USED, dwMemoryLoad);
			}
			return dwMemoryLoad;
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x000318DC File Offset: 0x000308DC
		internal long MemoryLimit
		{
			get
			{
				return (long)this._pressureHigh;
			}
		}
	}
}
