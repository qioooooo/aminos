using System;
using System.Web.Configuration;
using System.Web.Hosting;

namespace System.Web.Caching
{
	// Token: 0x0200010C RID: 268
	internal sealed class CacheMemoryPrivateBytesPressure : CacheMemoryPressure
	{
		// Token: 0x06000C7A RID: 3194 RVA: 0x000318E5 File Offset: 0x000308E5
		internal CacheMemoryPrivateBytesPressure()
		{
			this._pressureHigh = 99;
			this._pressureMiddle = 98;
			this._pressureLow = 97;
			this._startupTime = DateTime.UtcNow;
			base.InitHistory();
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000C7B RID: 3195 RVA: 0x00031918 File Offset: 0x00030918
		internal static long AutoPrivateBytesLimit
		{
			get
			{
				if (CacheMemoryPrivateBytesPressure.s_autoPrivateBytesLimit == -1L)
				{
					bool flag = IntPtr.Size == 8;
					long totalPhysical = CacheMemoryPressure.TotalPhysical;
					long totalVirtual = CacheMemoryPressure.TotalVirtual;
					if (totalPhysical != 0L)
					{
						long num;
						if (flag)
						{
							num = 1099511627776L;
						}
						else if (totalVirtual > (long)((ulong)(-2147483648)))
						{
							num = 1887436800L;
						}
						else
						{
							num = 838860800L;
						}
						long num2 = (HostingEnvironment.IsHosted ? (totalPhysical * 3L / 5L) : totalPhysical);
						CacheMemoryPrivateBytesPressure.s_autoPrivateBytesLimit = Math.Min(num2, num);
					}
					else
					{
						CacheMemoryPrivateBytesPressure.s_autoPrivateBytesLimit = (flag ? 1099511627776L : 838860800L);
					}
				}
				return CacheMemoryPrivateBytesPressure.s_autoPrivateBytesLimit;
			}
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x000319B0 File Offset: 0x000309B0
		internal override void ReadConfig(CacheSection cacheSection)
		{
			long privateBytesLimit = cacheSection.PrivateBytesLimit;
			if (UnsafeNativeMethods.GetModuleHandle("aspnet_wp.exe") != IntPtr.Zero)
			{
				this._memoryLimit = (long)UnsafeNativeMethods.PMGetMemoryLimitInMB() << 20;
			}
			else if (UnsafeNativeMethods.GetModuleHandle("w3wp.exe") != IntPtr.Zero)
			{
				IServerConfig instance = ServerConfig.GetInstance();
				this._memoryLimit = instance.GetW3WPMemoryLimitInKB() << 10;
			}
			if (privateBytesLimit == 0L && this._memoryLimit == 0L)
			{
				this._memoryLimit = CacheMemoryPrivateBytesPressure.AutoPrivateBytesLimit;
			}
			else if (privateBytesLimit != 0L && this._memoryLimit != 0L)
			{
				this._memoryLimit = Math.Min(this._memoryLimit, privateBytesLimit);
			}
			else if (privateBytesLimit != 0L)
			{
				this._memoryLimit = privateBytesLimit;
			}
			if (this._memoryLimit > 0L)
			{
				if (CacheMemoryPrivateBytesPressure.s_pid == 0U)
				{
					CacheMemoryPrivateBytesPressure.s_pid = (uint)SafeNativeMethods.GetCurrentProcessId();
				}
				if (this._memoryLimit >= 268435456L)
				{
					this._pressureHigh = (int)Math.Max(90L, (this._memoryLimit - 100663296L) * 100L / this._memoryLimit);
					this._pressureLow = (int)Math.Max(80L, (this._memoryLimit - 234881024L) * 100L / this._memoryLimit);
					this._pressureMiddle = (this._pressureHigh + this._pressureLow) / 2;
				}
				else
				{
					this._pressureHigh = 90;
					this._pressureMiddle = 85;
					this._pressureLow = 78;
				}
				this._pressureHighMemoryLimit = (long)this._pressureHigh * this._memoryLimit / 100L;
			}
			CacheMemoryPrivateBytesPressure.s_pollInterval = (int)Math.Min(cacheSection.PrivateBytesPollTime.TotalMilliseconds, 2147483647.0);
			PerfCounters.SetCounter(AppPerfCounter.CACHE_PERCENT_PROC_MEM_LIMIT_USED_BASE, (int)(this._pressureHighMemoryLimit >> 10));
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x00031B55 File Offset: 0x00030B55
		internal long MemoryLimit
		{
			get
			{
				return this._memoryLimit;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x00031B5D File Offset: 0x00030B5D
		internal static int PollInterval
		{
			get
			{
				return CacheMemoryPrivateBytesPressure.s_pollInterval;
			}
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00031B64 File Offset: 0x00030B64
		internal static long GetPrivateBytes(bool nocache)
		{
			int num;
			long num3;
			if (CacheMemoryPrivateBytesPressure.s_isIIS6)
			{
				long num2;
				num = UnsafeNativeMethods.GetPrivateBytesIIS6(out num2, nocache);
				num3 = num2;
			}
			else
			{
				uint num4 = 0U;
				uint num5;
				num = UnsafeNativeMethods.GetProcessMemoryInformation(CacheMemoryPrivateBytesPressure.s_pid, out num4, out num5, nocache);
				num3 = (long)((long)((ulong)num4) << 20);
			}
			if (num == 0)
			{
				CacheMemoryPrivateBytesPressure.s_lastReadPrivateBytes = num3;
				CacheMemoryPrivateBytesPressure.s_lastTimeReadPrivateBytes = DateTime.UtcNow;
			}
			return num3;
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00031BB8 File Offset: 0x00030BB8
		protected override int GetCurrentPressure()
		{
			if (this._memoryLimit == 0L)
			{
				return 0;
			}
			long privateBytes = CacheMemoryPrivateBytesPressure.GetPrivateBytes(false);
			if (this._pressureHighMemoryLimit != 0L)
			{
				PerfCounters.SetCounter(AppPerfCounter.CACHE_PERCENT_PROC_MEM_LIMIT_USED, (int)(privateBytes >> 10));
			}
			return (int)(privateBytes * 100L / this._memoryLimit);
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x00031BFD File Offset: 0x00030BFD
		internal long PressureHighMemoryLimit
		{
			get
			{
				return this._pressureHighMemoryLimit;
			}
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00031C05 File Offset: 0x00030C05
		internal bool HasLimit()
		{
			return this._memoryLimit != 0L;
		}

		// Token: 0x0400143B RID: 5179
		private const long PRIVATE_BYTES_LIMIT_2GB = 838860800L;

		// Token: 0x0400143C RID: 5180
		private const long PRIVATE_BYTES_LIMIT_3GB = 1887436800L;

		// Token: 0x0400143D RID: 5181
		private const long PRIVATE_BYTES_LIMIT_64BIT = 1099511627776L;

		// Token: 0x0400143E RID: 5182
		private long _memoryLimit;

		// Token: 0x0400143F RID: 5183
		private static bool s_isIIS6 = HostingEnvironment.IsUnderIIS6Process;

		// Token: 0x04001440 RID: 5184
		private static long s_autoPrivateBytesLimit = -1L;

		// Token: 0x04001441 RID: 5185
		private static long s_lastReadPrivateBytes;

		// Token: 0x04001442 RID: 5186
		private static DateTime s_lastTimeReadPrivateBytes = DateTime.MinValue;

		// Token: 0x04001443 RID: 5187
		private static uint s_pid = 0U;

		// Token: 0x04001444 RID: 5188
		private static int s_pollInterval;

		// Token: 0x04001445 RID: 5189
		private DateTime _startupTime;

		// Token: 0x04001446 RID: 5190
		private long _pressureHighMemoryLimit;
	}
}
