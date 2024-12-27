using System;
using System.Security.Permissions;
using System.Web.Hosting;

namespace System.Web
{
	// Token: 0x020000C0 RID: 192
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ProcessModelInfo
	{
		// Token: 0x060008D7 RID: 2263 RVA: 0x000280F8 File Offset: 0x000270F8
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.High)]
		public static ProcessInfo GetCurrentProcessInfo()
		{
			HttpContext httpContext = HttpContext.Current;
			if (httpContext == null || httpContext.WorkerRequest == null || !(httpContext.WorkerRequest is ISAPIWorkerRequestOutOfProc))
			{
				throw new HttpException(SR.GetString("Process_information_not_available"));
			}
			int num = 0;
			int num2 = 0;
			long num3 = 0L;
			int num4 = 0;
			int num5 = 0;
			int num6 = UnsafeNativeMethods.PMGetCurrentProcessInfo(ref num, ref num2, ref num5, ref num3, ref num4);
			if (num6 < 0)
			{
				throw new HttpException(SR.GetString("Process_information_not_available"));
			}
			DateTime dateTime = DateTime.FromFileTime(num3);
			TimeSpan timeSpan = DateTime.Now.Subtract(dateTime);
			return new ProcessInfo(dateTime, timeSpan, num4, num, ProcessStatus.Alive, ProcessShutdownReason.None, num5);
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00028190 File Offset: 0x00027190
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.High)]
		public static ProcessInfo[] GetHistory(int numRecords)
		{
			HttpContext httpContext = HttpContext.Current;
			if (httpContext == null || httpContext.WorkerRequest == null || !(httpContext.WorkerRequest is ISAPIWorkerRequestOutOfProc))
			{
				throw new HttpException(SR.GetString("Process_information_not_available"));
			}
			if (numRecords < 1)
			{
				return null;
			}
			int[] array = new int[numRecords];
			int[] array2 = new int[numRecords];
			int[] array3 = new int[numRecords];
			int[] array4 = new int[numRecords];
			int[] array5 = new int[numRecords];
			long[] array6 = new long[numRecords];
			long[] array7 = new long[numRecords];
			int[] array8 = new int[numRecords];
			int num = UnsafeNativeMethods.PMGetHistoryTable(numRecords, array, array2, array4, array3, array5, array8, array6, array7);
			if (num < 0)
			{
				throw new HttpException(SR.GetString("Process_information_not_available"));
			}
			ProcessInfo[] array9 = new ProcessInfo[num];
			for (int i = 0; i < num; i++)
			{
				DateTime dateTime = DateTime.FromFileTime(array6[i]);
				TimeSpan timeSpan = DateTime.Now.Subtract(dateTime);
				ProcessStatus processStatus = ProcessStatus.Alive;
				ProcessShutdownReason processShutdownReason = ProcessShutdownReason.None;
				if (array5[i] != 0)
				{
					if (array7[i] > 0L)
					{
						timeSpan = DateTime.FromFileTime(array7[i]).Subtract(dateTime);
					}
					if ((array5[i] & 4) != 0)
					{
						processStatus = ProcessStatus.Terminated;
					}
					else if ((array5[i] & 2) != 0)
					{
						processStatus = ProcessStatus.ShutDown;
					}
					else
					{
						processStatus = ProcessStatus.ShuttingDown;
					}
					if ((64 & array5[i]) != 0)
					{
						processShutdownReason = ProcessShutdownReason.IdleTimeout;
					}
					else if ((128 & array5[i]) != 0)
					{
						processShutdownReason = ProcessShutdownReason.RequestsLimit;
					}
					else if ((256 & array5[i]) != 0)
					{
						processShutdownReason = ProcessShutdownReason.RequestQueueLimit;
					}
					else if ((32 & array5[i]) != 0)
					{
						processShutdownReason = ProcessShutdownReason.Timeout;
					}
					else if ((512 & array5[i]) != 0)
					{
						processShutdownReason = ProcessShutdownReason.MemoryLimitExceeded;
					}
					else if ((1024 & array5[i]) != 0)
					{
						processShutdownReason = ProcessShutdownReason.PingFailed;
					}
					else if ((2048 & array5[i]) != 0)
					{
						processShutdownReason = ProcessShutdownReason.DeadlockSuspected;
					}
					else
					{
						processShutdownReason = ProcessShutdownReason.Unexpected;
					}
				}
				array9[i] = new ProcessInfo(dateTime, timeSpan, array[i], array2[i], processStatus, processShutdownReason, array8[i]);
			}
			return array9;
		}
	}
}
