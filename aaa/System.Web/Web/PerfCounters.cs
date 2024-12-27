using System;

namespace System.Web
{
	// Token: 0x020000BB RID: 187
	internal sealed class PerfCounters
	{
		// Token: 0x060008B6 RID: 2230 RVA: 0x00027B71 File Offset: 0x00026B71
		private PerfCounters()
		{
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00027B79 File Offset: 0x00026B79
		internal static void Open(string appName)
		{
			PerfCounters.OpenCounter(appName);
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00027B81 File Offset: 0x00026B81
		internal static void OpenStateCounters()
		{
			PerfCounters.OpenCounter(null);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x00027B8C File Offset: 0x00026B8C
		private static void OpenCounter(string appName)
		{
			try
			{
				if (HttpRuntime.IsEngineLoaded)
				{
					if (PerfCounters._global == IntPtr.Zero)
					{
						PerfCounters._global = UnsafeNativeMethods.PerfOpenGlobalCounters();
					}
					if (appName == null)
					{
						if (PerfCounters._stateService == IntPtr.Zero)
						{
							PerfCounters._stateService = UnsafeNativeMethods.PerfOpenStateCounters();
						}
					}
					else if (appName != null)
					{
						PerfCounters._instance = UnsafeNativeMethods.PerfOpenAppCounters(appName);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00027C00 File Offset: 0x00026C00
		internal static void IncrementCounter(AppPerfCounter counter)
		{
			if (PerfCounters._instance != null)
			{
				UnsafeNativeMethods.PerfIncrementCounter(PerfCounters._instance.UnsafeHandle, (int)counter);
			}
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x00027C19 File Offset: 0x00026C19
		internal static void DecrementCounter(AppPerfCounter counter)
		{
			if (PerfCounters._instance != null)
			{
				UnsafeNativeMethods.PerfDecrementCounter(PerfCounters._instance.UnsafeHandle, (int)counter);
			}
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00027C32 File Offset: 0x00026C32
		internal static void IncrementCounterEx(AppPerfCounter counter, int delta)
		{
			if (PerfCounters._instance != null)
			{
				UnsafeNativeMethods.PerfIncrementCounterEx(PerfCounters._instance.UnsafeHandle, (int)counter, delta);
			}
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00027C4C File Offset: 0x00026C4C
		internal static void SetCounter(AppPerfCounter counter, int value)
		{
			if (PerfCounters._instance != null)
			{
				UnsafeNativeMethods.PerfSetCounter(PerfCounters._instance.UnsafeHandle, (int)counter, value);
			}
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00027C66 File Offset: 0x00026C66
		internal static int GetGlobalCounter(GlobalPerfCounter counter)
		{
			if (PerfCounters._global != IntPtr.Zero)
			{
				return UnsafeNativeMethods.PerfGetCounter(PerfCounters._global, (int)counter);
			}
			return -1;
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00027C86 File Offset: 0x00026C86
		internal static void IncrementGlobalCounter(GlobalPerfCounter counter)
		{
			if (PerfCounters._global != IntPtr.Zero)
			{
				UnsafeNativeMethods.PerfIncrementCounter(PerfCounters._global, (int)counter);
			}
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x00027CA4 File Offset: 0x00026CA4
		internal static void DecrementGlobalCounter(GlobalPerfCounter counter)
		{
			if (PerfCounters._global != IntPtr.Zero)
			{
				UnsafeNativeMethods.PerfDecrementCounter(PerfCounters._global, (int)counter);
			}
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00027CC2 File Offset: 0x00026CC2
		internal static void SetGlobalCounter(GlobalPerfCounter counter, int value)
		{
			if (PerfCounters._global != IntPtr.Zero)
			{
				UnsafeNativeMethods.PerfSetCounter(PerfCounters._global, (int)counter, value);
			}
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x00027CE4 File Offset: 0x00026CE4
		internal static void IncrementStateServiceCounter(StateServicePerfCounter counter)
		{
			if (PerfCounters._stateService == IntPtr.Zero)
			{
				return;
			}
			UnsafeNativeMethods.PerfIncrementCounter(PerfCounters._stateService, (int)counter);
			switch (counter)
			{
			case StateServicePerfCounter.STATE_SERVICE_SESSIONS_ACTIVE:
				PerfCounters.IncrementGlobalCounter(GlobalPerfCounter.STATE_SERVER_SESSIONS_ACTIVE);
				return;
			case StateServicePerfCounter.STATE_SERVICE_SESSIONS_ABANDONED:
				PerfCounters.IncrementGlobalCounter(GlobalPerfCounter.STATE_SERVER_SESSIONS_ABANDONED);
				return;
			case StateServicePerfCounter.STATE_SERVICE_SESSIONS_TIMED_OUT:
				PerfCounters.IncrementGlobalCounter(GlobalPerfCounter.STATE_SERVER_SESSIONS_TIMED_OUT);
				return;
			case StateServicePerfCounter.STATE_SERVICE_SESSIONS_TOTAL:
				PerfCounters.IncrementGlobalCounter(GlobalPerfCounter.STATE_SERVER_SESSIONS_TOTAL);
				return;
			default:
				return;
			}
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00027D4C File Offset: 0x00026D4C
		internal static void DecrementStateServiceCounter(StateServicePerfCounter counter)
		{
			if (PerfCounters._stateService == IntPtr.Zero)
			{
				return;
			}
			UnsafeNativeMethods.PerfDecrementCounter(PerfCounters._stateService, (int)counter);
			switch (counter)
			{
			case StateServicePerfCounter.STATE_SERVICE_SESSIONS_ACTIVE:
				PerfCounters.DecrementGlobalCounter(GlobalPerfCounter.STATE_SERVER_SESSIONS_ACTIVE);
				return;
			case StateServicePerfCounter.STATE_SERVICE_SESSIONS_ABANDONED:
				PerfCounters.DecrementGlobalCounter(GlobalPerfCounter.STATE_SERVER_SESSIONS_ABANDONED);
				return;
			case StateServicePerfCounter.STATE_SERVICE_SESSIONS_TIMED_OUT:
				PerfCounters.DecrementGlobalCounter(GlobalPerfCounter.STATE_SERVER_SESSIONS_TIMED_OUT);
				return;
			case StateServicePerfCounter.STATE_SERVICE_SESSIONS_TOTAL:
				PerfCounters.DecrementGlobalCounter(GlobalPerfCounter.STATE_SERVER_SESSIONS_TOTAL);
				return;
			default:
				return;
			}
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00027DB4 File Offset: 0x00026DB4
		internal static void SetStateServiceCounter(StateServicePerfCounter counter, int value)
		{
			if (PerfCounters._stateService == IntPtr.Zero)
			{
				return;
			}
			UnsafeNativeMethods.PerfSetCounter(PerfCounters._stateService, (int)counter, value);
			switch (counter)
			{
			case StateServicePerfCounter.STATE_SERVICE_SESSIONS_ACTIVE:
				PerfCounters.SetGlobalCounter(GlobalPerfCounter.STATE_SERVER_SESSIONS_ACTIVE, value);
				return;
			case StateServicePerfCounter.STATE_SERVICE_SESSIONS_ABANDONED:
				PerfCounters.SetGlobalCounter(GlobalPerfCounter.STATE_SERVER_SESSIONS_ABANDONED, value);
				return;
			case StateServicePerfCounter.STATE_SERVICE_SESSIONS_TIMED_OUT:
				PerfCounters.SetGlobalCounter(GlobalPerfCounter.STATE_SERVER_SESSIONS_TIMED_OUT, value);
				return;
			case StateServicePerfCounter.STATE_SERVICE_SESSIONS_TOTAL:
				PerfCounters.SetGlobalCounter(GlobalPerfCounter.STATE_SERVER_SESSIONS_TOTAL, value);
				return;
			default:
				return;
			}
		}

		// Token: 0x040011E5 RID: 4581
		private static PerfInstanceDataHandle _instance = null;

		// Token: 0x040011E6 RID: 4582
		private static IntPtr _global = IntPtr.Zero;

		// Token: 0x040011E7 RID: 4583
		private static IntPtr _stateService = IntPtr.Zero;
	}
}
