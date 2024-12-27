using System;
using System.Globalization;
using System.Threading;

namespace System.Web.Compilation
{
	// Token: 0x02000166 RID: 358
	internal static class CompilationLock
	{
		// Token: 0x0600101A RID: 4122 RVA: 0x00047BAC File Offset: 0x00046BAC
		internal static void GetLock(ref bool gotLock)
		{
			try
			{
			}
			finally
			{
				Monitor.Enter(BuildManager.TheBuildManager);
				CompilationLock._mutex.WaitOne();
				gotLock = true;
			}
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00047BE4 File Offset: 0x00046BE4
		internal static void ReleaseLock()
		{
			CompilationLock._mutex.ReleaseMutex();
			Monitor.Exit(BuildManager.TheBuildManager);
		}

		// Token: 0x0400163F RID: 5695
		private static CompilationMutex _mutex = new CompilationMutex("CL" + ("CompilationLock" + HttpRuntime.AppDomainAppIdInternal.ToLower(CultureInfo.InvariantCulture)).GetHashCode().ToString("x", CultureInfo.InvariantCulture), "CompilationLock for " + HttpRuntime.AppDomainAppVirtualPath);
	}
}
