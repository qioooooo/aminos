using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;

namespace System.Runtime
{
	// Token: 0x020005FA RID: 1530
	public static class GCSettings
	{
		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x060037CA RID: 14282 RVA: 0x000BBE88 File Offset: 0x000BAE88
		// (set) Token: 0x060037CB RID: 14283 RVA: 0x000BBE8F File Offset: 0x000BAE8F
		public static GCLatencyMode LatencyMode
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return (GCLatencyMode)GC.nativeGetGCLatencyMode();
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
			set
			{
				if (value < GCLatencyMode.Batch || value > GCLatencyMode.LowLatency)
				{
					throw new ArgumentOutOfRangeException(Environment.GetResourceString("ArgumentOutOfRange_Enum"));
				}
				GC.nativeSetGCLatencyMode((int)value);
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x060037CC RID: 14284 RVA: 0x000BBEAF File Offset: 0x000BAEAF
		public static bool IsServerGC
		{
			get
			{
				return GC.nativeIsServerGC();
			}
		}
	}
}
