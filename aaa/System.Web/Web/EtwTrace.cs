using System;
using System.Web.Hosting;

namespace System.Web
{
	// Token: 0x0200002B RID: 43
	internal static class EtwTrace
	{
		// Token: 0x060000DF RID: 223 RVA: 0x000055E8 File Offset: 0x000045E8
		internal static int InferVerbosity(IntegratedTraceType traceType)
		{
			int num;
			switch (traceType)
			{
			case IntegratedTraceType.TraceWrite:
				num = 5;
				break;
			case IntegratedTraceType.TraceWarn:
				num = 3;
				break;
			case IntegratedTraceType.DiagCritical:
				num = 1;
				break;
			case IntegratedTraceType.DiagError:
				num = 2;
				break;
			case IntegratedTraceType.DiagWarning:
				num = 3;
				break;
			case IntegratedTraceType.DiagInfo:
				num = 4;
				break;
			case IntegratedTraceType.DiagVerbose:
				num = 5;
				break;
			case IntegratedTraceType.DiagStart:
				num = 0;
				break;
			case IntegratedTraceType.DiagStop:
				num = 0;
				break;
			case IntegratedTraceType.DiagSuspend:
				num = 0;
				break;
			case IntegratedTraceType.DiagResume:
				num = 0;
				break;
			case IntegratedTraceType.DiagTransfer:
				num = 0;
				break;
			default:
				num = 5;
				break;
			}
			return num;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005664 File Offset: 0x00004664
		internal static bool IsTraceEnabled(int level, int flag)
		{
			return level < EtwTrace._traceLevel && (flag & EtwTrace._traceFlags) != 0;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000567A File Offset: 0x0000467A
		private static void ResolveWorkerRequestType(HttpWorkerRequest workerRequest)
		{
			if (workerRequest is IIS7WorkerRequest)
			{
				EtwTrace.s_WrType = EtwWorkerRequestType.IIS7Integrated;
				return;
			}
			if (workerRequest is ISAPIWorkerRequestInProc)
			{
				EtwTrace.s_WrType = EtwWorkerRequestType.InProc;
				return;
			}
			if (workerRequest is ISAPIWorkerRequestOutOfProc)
			{
				EtwTrace.s_WrType = EtwWorkerRequestType.OutOfProc;
				return;
			}
			EtwTrace.s_WrType = EtwWorkerRequestType.Unknown;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000056B4 File Offset: 0x000046B4
		internal static void TraceEnableCheck(EtwTraceConfigType configType, IntPtr p)
		{
			if (!HttpRuntime.IsEngineLoaded)
			{
				return;
			}
			switch (configType)
			{
			case EtwTraceConfigType.DOWNLEVEL:
				UnsafeNativeMethods.GetEtwValues(out EtwTrace._traceLevel, out EtwTrace._traceFlags);
				return;
			case EtwTraceConfigType.IIS7_ISAPI:
			{
				int[] array = new int[3];
				UnsafeNativeMethods.EcbGetTraceFlags(p, array);
				EtwTrace._traceFlags = array[0];
				EtwTrace._traceLevel = array[1];
				return;
			}
			case EtwTraceConfigType.IIS7_INTEGRATED:
			{
				bool flag;
				UnsafeIISMethods.MgdEtwGetTraceConfig(p, out flag, out EtwTrace._traceFlags, out EtwTrace._traceLevel);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005721 File Offset: 0x00004721
		internal static void Trace(EtwTraceType traceType, HttpWorkerRequest workerRequest)
		{
			EtwTrace.Trace(traceType, workerRequest, null, null);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000572C File Offset: 0x0000472C
		internal static void Trace(EtwTraceType traceType, HttpWorkerRequest workerRequest, string data1)
		{
			EtwTrace.Trace(traceType, workerRequest, data1, null, null, null);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005739 File Offset: 0x00004739
		internal static void Trace(EtwTraceType traceType, HttpWorkerRequest workerRequest, string data1, string data2)
		{
			EtwTrace.Trace(traceType, workerRequest, data1, data2, null, null);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005748 File Offset: 0x00004748
		internal static void Trace(EtwTraceType traceType, HttpWorkerRequest workerRequest, string data1, string data2, string data3, string data4)
		{
			if (EtwTrace.s_WrType == EtwWorkerRequestType.Undefined)
			{
				EtwTrace.ResolveWorkerRequestType(workerRequest);
			}
			if (EtwTrace.s_WrType == EtwWorkerRequestType.Unknown)
			{
				return;
			}
			if (workerRequest == null)
			{
				return;
			}
			if (EtwTrace.s_WrType == EtwWorkerRequestType.IIS7Integrated)
			{
				UnsafeNativeMethods.TraceRaiseEventMgdHandler((int)traceType, ((IIS7WorkerRequest)workerRequest).RequestContext, data1, data2, data3, data4);
				return;
			}
			if (EtwTrace.s_WrType == EtwWorkerRequestType.InProc)
			{
				UnsafeNativeMethods.TraceRaiseEventWithEcb((int)traceType, ((ISAPIWorkerRequest)workerRequest).Ecb, data1, data2, data3, data4);
				return;
			}
			if (EtwTrace.s_WrType == EtwWorkerRequestType.OutOfProc)
			{
				UnsafeNativeMethods.PMTraceRaiseEvent((int)traceType, ((ISAPIWorkerRequest)workerRequest).Ecb, data1, data2, data3, data4);
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000057D2 File Offset: 0x000047D2
		internal static void Trace(EtwTraceType traceType, IntPtr ecb, string data1, string data2, bool inProc)
		{
			if (inProc)
			{
				UnsafeNativeMethods.TraceRaiseEventWithEcb((int)traceType, ecb, data1, data2, null, null);
				return;
			}
			UnsafeNativeMethods.PMTraceRaiseEvent((int)traceType, ecb, data1, data2, null, null);
		}

		// Token: 0x04000D9B RID: 3483
		private static int _traceLevel = 0;

		// Token: 0x04000D9C RID: 3484
		private static int _traceFlags = 0;

		// Token: 0x04000D9D RID: 3485
		private static EtwWorkerRequestType s_WrType = EtwWorkerRequestType.Undefined;
	}
}
