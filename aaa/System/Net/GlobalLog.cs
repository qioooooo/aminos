using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.ConstrainedExecution;

namespace System.Net
{
	// Token: 0x020004EC RID: 1260
	internal static class GlobalLog
	{
		// Token: 0x06002736 RID: 10038 RVA: 0x000A23A6 File Offset: 0x000A13A6
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		private static BaseLoggingObject LoggingInitialize()
		{
			return new BaseLoggingObject();
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06002737 RID: 10039 RVA: 0x000A23AD File Offset: 0x000A13AD
		internal static ThreadKinds CurrentThreadKind
		{
			get
			{
				return ThreadKinds.Unknown;
			}
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x000A23B0 File Offset: 0x000A13B0
		[Conditional("DEBUG")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		internal static void SetThreadSource(ThreadKinds source)
		{
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x000A23B2 File Offset: 0x000A13B2
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		[Conditional("DEBUG")]
		internal static void ThreadContract(ThreadKinds kind, string errorMsg)
		{
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x000A23B4 File Offset: 0x000A13B4
		[Conditional("DEBUG")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		internal static void ThreadContract(ThreadKinds kind, ThreadKinds allowedSources, string errorMsg)
		{
			if ((kind & ThreadKinds.SourceMask) != ThreadKinds.Unknown || (allowedSources & ThreadKinds.SourceMask) != allowedSources)
			{
				throw new InternalException();
			}
			ThreadKinds currentThreadKind = GlobalLog.CurrentThreadKind;
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x000A23D5 File Offset: 0x000A13D5
		[Conditional("TRAVE")]
		public static void AddToArray(string msg)
		{
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x000A23D7 File Offset: 0x000A13D7
		[Conditional("TRAVE")]
		public static void Ignore(object msg)
		{
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x000A23D9 File Offset: 0x000A13D9
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		[Conditional("TRAVE")]
		public static void Print(string msg)
		{
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x000A23DB File Offset: 0x000A13DB
		[Conditional("TRAVE")]
		public static void PrintHex(string msg, object value)
		{
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x000A23DD File Offset: 0x000A13DD
		[Conditional("TRAVE")]
		public static void Enter(string func)
		{
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x000A23DF File Offset: 0x000A13DF
		[Conditional("TRAVE")]
		public static void Enter(string func, string parms)
		{
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x000A23E4 File Offset: 0x000A13E4
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		[Conditional("DEBUG")]
		[Conditional("_FORCE_ASSERTS")]
		public static void Assert(bool condition, string messageFormat, params object[] data)
		{
			if (!condition)
			{
				string text = string.Format(CultureInfo.InvariantCulture, messageFormat, data);
				int num = text.IndexOf('|');
				if (num == -1)
				{
					return;
				}
				int length = text.Length;
			}
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x000A2416 File Offset: 0x000A1416
		[Conditional("_FORCE_ASSERTS")]
		[Conditional("DEBUG")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		public static void Assert(string message)
		{
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x000A2418 File Offset: 0x000A1418
		[Conditional("DEBUG")]
		[Conditional("_FORCE_ASSERTS")]
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.None)]
		public static void Assert(string message, string detailMessage)
		{
			try
			{
				GlobalLog.Logobject.DumpArray(false);
			}
			finally
			{
				UnsafeNclNativeMethods.DebugBreak();
				Debugger.Break();
			}
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x000A2450 File Offset: 0x000A1450
		[Conditional("TRAVE")]
		public static void LeaveException(string func, Exception exception)
		{
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x000A2452 File Offset: 0x000A1452
		[Conditional("TRAVE")]
		public static void Leave(string func)
		{
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x000A2454 File Offset: 0x000A1454
		[Conditional("TRAVE")]
		public static void Leave(string func, string result)
		{
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x000A2456 File Offset: 0x000A1456
		[Conditional("TRAVE")]
		public static void Leave(string func, int returnval)
		{
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x000A2458 File Offset: 0x000A1458
		[Conditional("TRAVE")]
		public static void Leave(string func, bool returnval)
		{
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x000A245A File Offset: 0x000A145A
		[Conditional("TRAVE")]
		public static void DumpArray()
		{
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x000A245C File Offset: 0x000A145C
		[Conditional("TRAVE")]
		public static void Dump(byte[] buffer)
		{
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x000A245E File Offset: 0x000A145E
		[Conditional("TRAVE")]
		public static void Dump(byte[] buffer, int length)
		{
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x000A2460 File Offset: 0x000A1460
		[Conditional("TRAVE")]
		public static void Dump(byte[] buffer, int offset, int length)
		{
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x000A2462 File Offset: 0x000A1462
		[Conditional("TRAVE")]
		public static void Dump(IntPtr buffer, int offset, int length)
		{
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x000A2464 File Offset: 0x000A1464
		[Conditional("DEBUG")]
		internal static void DebugAddRequest(HttpWebRequest request, Connection connection, int flags)
		{
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x000A2466 File Offset: 0x000A1466
		[Conditional("DEBUG")]
		internal static void DebugRemoveRequest(HttpWebRequest request)
		{
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x000A2468 File Offset: 0x000A1468
		[Conditional("DEBUG")]
		internal static void DebugUpdateRequest(HttpWebRequest request, Connection connection, int flags)
		{
		}

		// Token: 0x040026B4 RID: 9908
		private static BaseLoggingObject Logobject = GlobalLog.LoggingInitialize();
	}
}
