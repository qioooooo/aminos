using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Management
{
	// Token: 0x02000105 RID: 261
	internal class MTAHelper
	{
		// Token: 0x06000656 RID: 1622 RVA: 0x00027334 File Offset: 0x00026334
		private static void InitWorkerThread()
		{
			Thread thread = new Thread(new ThreadStart(MTAHelper.WorkerThread));
			thread.SetApartmentState(ApartmentState.MTA);
			thread.IsBackground = true;
			thread.Start();
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00027368 File Offset: 0x00026368
		public static object CreateInMTA(Type type)
		{
			if (MTAHelper.IsNoContextMTA())
			{
				return Activator.CreateInstance(type);
			}
			MTAHelper.MTARequest mtarequest = new MTAHelper.MTARequest(type);
			lock (MTAHelper.critSec)
			{
				if (!MTAHelper.workerThreadInitialized)
				{
					MTAHelper.InitWorkerThread();
					MTAHelper.workerThreadInitialized = true;
				}
				int num = MTAHelper.reqList.Add(mtarequest);
				if (!MTAHelper.evtGo.Set())
				{
					MTAHelper.reqList.RemoveAt(num);
					throw new ManagementException(RC.GetString("WORKER_THREAD_WAKEUP_FAILED"));
				}
			}
			mtarequest.evtDone.WaitOne();
			if (mtarequest.exception != null)
			{
				throw mtarequest.exception;
			}
			return mtarequest.createdObject;
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00027418 File Offset: 0x00026418
		private static void WorkerThread()
		{
			for (;;)
			{
				MTAHelper.evtGo.WaitOne();
				for (;;)
				{
					MTAHelper.MTARequest mtarequest = null;
					lock (MTAHelper.critSec)
					{
						if (MTAHelper.reqList.Count <= 0)
						{
							break;
						}
						mtarequest = (MTAHelper.MTARequest)MTAHelper.reqList[0];
						MTAHelper.reqList.RemoveAt(0);
					}
					try
					{
						mtarequest.createdObject = Activator.CreateInstance(mtarequest.typeToCreate);
					}
					catch (Exception ex)
					{
						mtarequest.exception = ex;
					}
					finally
					{
						mtarequest.evtDone.Set();
					}
				}
			}
		}

		// Token: 0x06000659 RID: 1625
		[SuppressUnmanagedCodeSecurity]
		[DllImport("ole32.dll")]
		private static extern int CoGetObjectContext([In] ref Guid riid, out IntPtr pUnk);

		// Token: 0x0600065A RID: 1626 RVA: 0x000274C8 File Offset: 0x000264C8
		public static bool IsNoContextMTA()
		{
			if (Thread.CurrentThread.GetApartmentState() != ApartmentState.MTA)
			{
				return false;
			}
			if (!MTAHelper.CanCallCoGetObjectContext)
			{
				return true;
			}
			IntPtr zero = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			try
			{
				if (MTAHelper.CoGetObjectContext(ref MTAHelper.IID_IComThreadingInfo, out zero) != 0)
				{
					return false;
				}
				WmiNetUtilsHelper.APTTYPE apttype;
				if (WmiNetUtilsHelper.GetCurrentApartmentType_f(3, zero, out apttype) != 0)
				{
					return false;
				}
				if (apttype != WmiNetUtilsHelper.APTTYPE.APTTYPE_MTA)
				{
					return false;
				}
				if (Marshal.QueryInterface(zero, ref MTAHelper.IID_IObjectContext, out zero2) == 0)
				{
					return false;
				}
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Marshal.Release(zero);
				}
				if (zero2 != IntPtr.Zero)
				{
					Marshal.Release(zero2);
				}
			}
			return true;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00027578 File Offset: 0x00026578
		private static bool IsWindows2000OrHigher()
		{
			OperatingSystem osversion = Environment.OSVersion;
			return osversion.Platform == PlatformID.Win32NT && osversion.Version >= new Version(5, 0);
		}

		// Token: 0x04000521 RID: 1313
		private static ArrayList reqList = new ArrayList(3);

		// Token: 0x04000522 RID: 1314
		private static object critSec = new object();

		// Token: 0x04000523 RID: 1315
		private static AutoResetEvent evtGo = new AutoResetEvent(false);

		// Token: 0x04000524 RID: 1316
		private static bool workerThreadInitialized = false;

		// Token: 0x04000525 RID: 1317
		private static Guid IID_IObjectContext = new Guid("51372AE0-CAE7-11CF-BE81-00AA00A2FA25");

		// Token: 0x04000526 RID: 1318
		private static Guid IID_IComThreadingInfo = new Guid("000001ce-0000-0000-C000-000000000046");

		// Token: 0x04000527 RID: 1319
		private static bool CanCallCoGetObjectContext = MTAHelper.IsWindows2000OrHigher();

		// Token: 0x02000106 RID: 262
		private class MTARequest
		{
			// Token: 0x0600065E RID: 1630 RVA: 0x0002760F File Offset: 0x0002660F
			public MTARequest(Type typeToCreate)
			{
				this.typeToCreate = typeToCreate;
			}

			// Token: 0x04000528 RID: 1320
			public AutoResetEvent evtDone = new AutoResetEvent(false);

			// Token: 0x04000529 RID: 1321
			public Type typeToCreate;

			// Token: 0x0400052A RID: 1322
			public object createdObject;

			// Token: 0x0400052B RID: 1323
			public Exception exception;
		}
	}
}
