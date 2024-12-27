using System;
using System.Diagnostics;
using System.Security.Permissions;
using System.Threading;

namespace System.EnterpriseServices
{
	// Token: 0x0200009E RID: 158
	internal static class DBG
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0000C0A8 File Offset: 0x0000B0A8
		public static TraceSwitch General
		{
			get
			{
				if (!DBG._initialized)
				{
					DBG.InitDBG();
				}
				return DBG._genSwitch;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0000C0BD File Offset: 0x0000B0BD
		public static TraceSwitch Registration
		{
			get
			{
				if (!DBG._initialized)
				{
					DBG.InitDBG();
				}
				return DBG._regSwitch;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x0000C0D2 File Offset: 0x0000B0D2
		public static TraceSwitch Pool
		{
			get
			{
				if (!DBG._initialized)
				{
					DBG.InitDBG();
				}
				return DBG._poolSwitch;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x0000C0E7 File Offset: 0x0000B0E7
		public static TraceSwitch Platform
		{
			get
			{
				if (!DBG._initialized)
				{
					DBG.InitDBG();
				}
				return DBG._platSwitch;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x0000C0FC File Offset: 0x0000B0FC
		public static TraceSwitch CRM
		{
			get
			{
				if (!DBG._initialized)
				{
					DBG.InitDBG();
				}
				return DBG._crmSwitch;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x0000C111 File Offset: 0x0000B111
		public static TraceSwitch Perf
		{
			get
			{
				if (!DBG._initialized)
				{
					DBG.InitDBG();
				}
				return DBG._perfSwitch;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060003BA RID: 954 RVA: 0x0000C126 File Offset: 0x0000B126
		public static TraceSwitch Thunk
		{
			get
			{
				if (!DBG._initialized)
				{
					DBG.InitDBG();
				}
				return DBG._thkSwitch;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0000C13B File Offset: 0x0000B13B
		public static TraceSwitch SC
		{
			get
			{
				if (!DBG._initialized)
				{
					DBG.InitDBG();
				}
				return DBG._scSwitch;
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000C150 File Offset: 0x0000B150
		public static void InitDBG()
		{
			if (DBG._initialized)
			{
				return;
			}
			lock (DBG.initializeLock)
			{
				if (!DBG._initialized)
				{
					RegistryPermission registryPermission = new RegistryPermission(PermissionState.Unrestricted);
					registryPermission.Assert();
					DBG._genSwitch = new TraceSwitch("General");
					DBG._platSwitch = new TraceSwitch("Platform");
					DBG._regSwitch = new TraceSwitch("Registration");
					DBG._crmSwitch = new TraceSwitch("CRM");
					DBG._perfSwitch = new TraceSwitch("PerfLog");
					DBG._poolSwitch = new TraceSwitch("ObjectPool");
					DBG._thkSwitch = new TraceSwitch("Thunk");
					DBG._scSwitch = new TraceSwitch("ServicedComponent");
					DBG._conSwitch = new BooleanSwitch("ConsoleOutput");
					DBG._dbgDisable = new BooleanSwitch("DisableDebugOutput");
					DBG._stackSwitch = new BooleanSwitch("PrintStacks");
					DBG._initialized = true;
				}
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000C254 File Offset: 0x0000B254
		private static int TID()
		{
			return Thread.CurrentThread.GetHashCode();
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000C260 File Offset: 0x0000B260
		[Conditional("_DEBUG")]
		public static void Trace(TraceLevel level, TraceSwitch sw, string msg)
		{
			if (!DBG._initialized)
			{
				DBG.InitDBG();
			}
			bool flag = sw.Level != 0 && sw.Level >= (int)level;
			if (flag)
			{
				string text = string.Concat(new object[]
				{
					DBG.TID(),
					": ",
					sw.Name,
					": ",
					msg
				});
				if (DBG._stackSwitch.Enabled)
				{
					text += new StackTrace(2).ToString();
				}
				if (DBG._conSwitch.Enabled)
				{
					Console.WriteLine(text);
				}
				if (!DBG._dbgDisable.Enabled)
				{
					Util.OutputDebugString(text + "\n");
				}
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000C31D File Offset: 0x0000B31D
		[Conditional("_DEBUG")]
		public static void Info(TraceSwitch sw, string msg)
		{
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000C31F File Offset: 0x0000B31F
		[Conditional("_DEBUG")]
		public static void Status(TraceSwitch sw, string msg)
		{
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000C321 File Offset: 0x0000B321
		[Conditional("_DEBUG")]
		public static void Warning(TraceSwitch sw, string msg)
		{
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000C323 File Offset: 0x0000B323
		[Conditional("_DEBUG")]
		public static void Error(TraceSwitch sw, string msg)
		{
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000C328 File Offset: 0x0000B328
		[Conditional("_DEBUG")]
		private static void DoAssert(string msg, string detail)
		{
			StackTrace stackTrace = new StackTrace();
			string text = string.Concat(new object[] { msg, "\n\n", detail, "\n", stackTrace, "\n\nPress RETRY to launch a debugger." });
			string text2 = "ALERT: System.EnterpriseServices,  TID=" + DBG.TID();
			Util.OutputDebugString(text2 + "\n\n" + text);
			if (!Debugger.IsAttached)
			{
				int num = Util.MessageBox(0, text, text2, Util.MB_ABORTRETRYIGNORE | Util.MB_ICONEXCLAMATION);
				if (num == 3)
				{
					Environment.Exit(1);
					return;
				}
				if (num == 4)
				{
					if (!Debugger.IsAttached)
					{
						Debugger.Launch();
						return;
					}
					Debugger.Break();
					return;
				}
				else if (num == 5)
				{
					return;
				}
			}
			else
			{
				Debugger.Break();
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000C3E3 File Offset: 0x0000B3E3
		[Conditional("_DEBUG")]
		public static void Assert(bool cond, string msg)
		{
			if (!DBG._initialized)
			{
				DBG.InitDBG();
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000C3F5 File Offset: 0x0000B3F5
		[Conditional("_DEBUG")]
		public static void Assert(bool cond, string msg, string detail)
		{
			if (!DBG._initialized)
			{
				DBG.InitDBG();
			}
		}

		// Token: 0x040001A8 RID: 424
		private static TraceSwitch _genSwitch;

		// Token: 0x040001A9 RID: 425
		private static TraceSwitch _regSwitch;

		// Token: 0x040001AA RID: 426
		private static TraceSwitch _platSwitch;

		// Token: 0x040001AB RID: 427
		private static TraceSwitch _crmSwitch;

		// Token: 0x040001AC RID: 428
		private static TraceSwitch _perfSwitch;

		// Token: 0x040001AD RID: 429
		private static TraceSwitch _poolSwitch;

		// Token: 0x040001AE RID: 430
		private static TraceSwitch _thkSwitch;

		// Token: 0x040001AF RID: 431
		private static TraceSwitch _scSwitch;

		// Token: 0x040001B0 RID: 432
		private static BooleanSwitch _conSwitch;

		// Token: 0x040001B1 RID: 433
		private static BooleanSwitch _dbgDisable;

		// Token: 0x040001B2 RID: 434
		private static BooleanSwitch _stackSwitch;

		// Token: 0x040001B3 RID: 435
		private static volatile bool _initialized;

		// Token: 0x040001B4 RID: 436
		private static object initializeLock = new object();
	}
}
