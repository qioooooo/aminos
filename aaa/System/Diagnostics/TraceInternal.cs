using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020001D8 RID: 472
	internal static class TraceInternal
	{
		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x0002E4F0 File Offset: 0x0002D4F0
		public static TraceListenerCollection Listeners
		{
			get
			{
				TraceInternal.InitializeSettings();
				if (TraceInternal.listeners == null)
				{
					lock (TraceInternal.critSec)
					{
						if (TraceInternal.listeners == null)
						{
							SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.SystemDiagnosticsSection;
							if (systemDiagnosticsSection != null)
							{
								TraceInternal.listeners = systemDiagnosticsSection.Trace.Listeners.GetRuntimeObject();
							}
							else
							{
								TraceInternal.listeners = new TraceListenerCollection();
								TraceListener traceListener = new DefaultTraceListener();
								traceListener.IndentLevel = TraceInternal.indentLevel;
								traceListener.IndentSize = TraceInternal.indentSize;
								TraceInternal.listeners.Add(traceListener);
							}
						}
					}
				}
				return TraceInternal.listeners;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000EC8 RID: 3784 RVA: 0x0002E58C File Offset: 0x0002D58C
		internal static string AppName
		{
			get
			{
				if (TraceInternal.appName == null)
				{
					new EnvironmentPermission(EnvironmentPermissionAccess.Read, "Path").Assert();
					TraceInternal.appName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
				}
				return TraceInternal.appName;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x0002E5BB File Offset: 0x0002D5BB
		// (set) Token: 0x06000ECA RID: 3786 RVA: 0x0002E5C7 File Offset: 0x0002D5C7
		public static bool AutoFlush
		{
			get
			{
				TraceInternal.InitializeSettings();
				return TraceInternal.autoFlush;
			}
			set
			{
				TraceInternal.InitializeSettings();
				TraceInternal.autoFlush = value;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x0002E5D4 File Offset: 0x0002D5D4
		// (set) Token: 0x06000ECC RID: 3788 RVA: 0x0002E5E0 File Offset: 0x0002D5E0
		public static bool UseGlobalLock
		{
			get
			{
				TraceInternal.InitializeSettings();
				return TraceInternal.useGlobalLock;
			}
			set
			{
				TraceInternal.InitializeSettings();
				TraceInternal.useGlobalLock = value;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x0002E5ED File Offset: 0x0002D5ED
		internal static TraceEventCache EventCache
		{
			get
			{
				if (TraceInternal.eventCache == null)
				{
					TraceInternal.eventCache = new TraceEventCache();
				}
				return TraceInternal.eventCache;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000ECE RID: 3790 RVA: 0x0002E605 File Offset: 0x0002D605
		// (set) Token: 0x06000ECF RID: 3791 RVA: 0x0002E60C File Offset: 0x0002D60C
		public static int IndentLevel
		{
			get
			{
				return TraceInternal.indentLevel;
			}
			set
			{
				lock (TraceInternal.critSec)
				{
					if (value < 0)
					{
						value = 0;
					}
					TraceInternal.indentLevel = value;
					if (TraceInternal.listeners != null)
					{
						foreach (object obj2 in TraceInternal.Listeners)
						{
							TraceListener traceListener = (TraceListener)obj2;
							traceListener.IndentLevel = TraceInternal.indentLevel;
						}
					}
				}
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x0002E6A0 File Offset: 0x0002D6A0
		// (set) Token: 0x06000ED1 RID: 3793 RVA: 0x0002E6AC File Offset: 0x0002D6AC
		public static int IndentSize
		{
			get
			{
				TraceInternal.InitializeSettings();
				return TraceInternal.indentSize;
			}
			set
			{
				TraceInternal.InitializeSettings();
				TraceInternal.SetIndentSize(value);
			}
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0002E6BC File Offset: 0x0002D6BC
		private static void SetIndentSize(int value)
		{
			lock (TraceInternal.critSec)
			{
				if (value < 0)
				{
					value = 0;
				}
				TraceInternal.indentSize = value;
				if (TraceInternal.listeners != null)
				{
					foreach (object obj2 in TraceInternal.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj2;
						traceListener.IndentSize = TraceInternal.indentSize;
					}
				}
			}
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0002E750 File Offset: 0x0002D750
		public static void Indent()
		{
			lock (TraceInternal.critSec)
			{
				TraceInternal.InitializeSettings();
				if (TraceInternal.indentLevel < 2147483647)
				{
					TraceInternal.indentLevel++;
				}
				foreach (object obj2 in TraceInternal.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj2;
					traceListener.IndentLevel = TraceInternal.indentLevel;
				}
			}
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0002E7EC File Offset: 0x0002D7EC
		public static void Unindent()
		{
			lock (TraceInternal.critSec)
			{
				TraceInternal.InitializeSettings();
				if (TraceInternal.indentLevel > 0)
				{
					TraceInternal.indentLevel--;
				}
				foreach (object obj2 in TraceInternal.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj2;
					traceListener.IndentLevel = TraceInternal.indentLevel;
				}
			}
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0002E884 File Offset: 0x0002D884
		public static void Flush()
		{
			if (TraceInternal.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					lock (TraceInternal.critSec)
					{
						foreach (object obj2 in TraceInternal.Listeners)
						{
							TraceListener traceListener = (TraceListener)obj2;
							traceListener.Flush();
						}
						return;
					}
				}
				foreach (object obj3 in TraceInternal.Listeners)
				{
					TraceListener traceListener2 = (TraceListener)obj3;
					if (!traceListener2.IsThreadSafe)
					{
						lock (traceListener2)
						{
							traceListener2.Flush();
							continue;
						}
					}
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0002E98C File Offset: 0x0002D98C
		public static void Close()
		{
			if (TraceInternal.listeners != null)
			{
				lock (TraceInternal.critSec)
				{
					foreach (object obj2 in TraceInternal.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj2;
						traceListener.Close();
					}
				}
			}
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0002EA0C File Offset: 0x0002DA0C
		public static void Assert(bool condition)
		{
			if (condition)
			{
				return;
			}
			TraceInternal.Fail(string.Empty);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0002EA1C File Offset: 0x0002DA1C
		public static void Assert(bool condition, string message)
		{
			if (condition)
			{
				return;
			}
			TraceInternal.Fail(message);
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0002EA28 File Offset: 0x0002DA28
		public static void Assert(bool condition, string message, string detailMessage)
		{
			if (condition)
			{
				return;
			}
			TraceInternal.Fail(message, detailMessage);
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0002EA38 File Offset: 0x0002DA38
		public static void Fail(string message)
		{
			if (TraceInternal.UseGlobalLock)
			{
				lock (TraceInternal.critSec)
				{
					foreach (object obj2 in TraceInternal.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj2;
						traceListener.Fail(message);
						if (TraceInternal.AutoFlush)
						{
							traceListener.Flush();
						}
					}
					return;
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					lock (traceListener2)
					{
						traceListener2.Fail(message);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.Fail(message);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0002EB64 File Offset: 0x0002DB64
		public static void Fail(string message, string detailMessage)
		{
			if (TraceInternal.UseGlobalLock)
			{
				lock (TraceInternal.critSec)
				{
					foreach (object obj2 in TraceInternal.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj2;
						traceListener.Fail(message, detailMessage);
						if (TraceInternal.AutoFlush)
						{
							traceListener.Flush();
						}
					}
					return;
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					lock (traceListener2)
					{
						traceListener2.Fail(message, detailMessage);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.Fail(message, detailMessage);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0002EC90 File Offset: 0x0002DC90
		private static void InitializeSettings()
		{
			if (!TraceInternal.settingsInitialized || (TraceInternal.defaultInitialized && DiagnosticsConfiguration.IsInitialized()))
			{
				TraceInternal.defaultInitialized = DiagnosticsConfiguration.IsInitializing();
				TraceInternal.SetIndentSize(DiagnosticsConfiguration.IndentSize);
				TraceInternal.autoFlush = DiagnosticsConfiguration.AutoFlush;
				TraceInternal.useGlobalLock = DiagnosticsConfiguration.UseGlobalLock;
				TraceInternal.settingsInitialized = true;
			}
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0002ECE0 File Offset: 0x0002DCE0
		public static void TraceEvent(TraceEventType eventType, int id, string format, params object[] args)
		{
			if (TraceInternal.UseGlobalLock)
			{
				lock (TraceInternal.critSec)
				{
					if (args == null)
					{
						using (IEnumerator enumerator = TraceInternal.Listeners.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj2 = enumerator.Current;
								TraceListener traceListener = (TraceListener)obj2;
								traceListener.TraceEvent(TraceInternal.EventCache, TraceInternal.AppName, eventType, id, format);
								if (TraceInternal.AutoFlush)
								{
									traceListener.Flush();
								}
							}
							goto IL_00D1;
						}
					}
					foreach (object obj3 in TraceInternal.Listeners)
					{
						TraceListener traceListener2 = (TraceListener)obj3;
						traceListener2.TraceEvent(TraceInternal.EventCache, TraceInternal.AppName, eventType, id, format, args);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
					}
					IL_00D1:
					goto IL_0215;
				}
			}
			if (args == null)
			{
				using (IEnumerator enumerator3 = TraceInternal.Listeners.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						object obj4 = enumerator3.Current;
						TraceListener traceListener3 = (TraceListener)obj4;
						if (!traceListener3.IsThreadSafe)
						{
							lock (traceListener3)
							{
								traceListener3.TraceEvent(TraceInternal.EventCache, TraceInternal.AppName, eventType, id, format);
								if (TraceInternal.AutoFlush)
								{
									traceListener3.Flush();
								}
								continue;
							}
						}
						traceListener3.TraceEvent(TraceInternal.EventCache, TraceInternal.AppName, eventType, id, format);
						if (TraceInternal.AutoFlush)
						{
							traceListener3.Flush();
						}
					}
					goto IL_0215;
				}
			}
			foreach (object obj5 in TraceInternal.Listeners)
			{
				TraceListener traceListener5 = (TraceListener)obj5;
				if (!traceListener5.IsThreadSafe)
				{
					lock (traceListener5)
					{
						traceListener5.TraceEvent(TraceInternal.EventCache, TraceInternal.AppName, eventType, id, format, args);
						if (TraceInternal.AutoFlush)
						{
							traceListener5.Flush();
						}
						continue;
					}
				}
				traceListener5.TraceEvent(TraceInternal.EventCache, TraceInternal.AppName, eventType, id, format, args);
				if (TraceInternal.AutoFlush)
				{
					traceListener5.Flush();
				}
			}
			IL_0215:
			TraceInternal.EventCache.Clear();
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0002EF64 File Offset: 0x0002DF64
		public static void Write(string message)
		{
			if (TraceInternal.UseGlobalLock)
			{
				lock (TraceInternal.critSec)
				{
					foreach (object obj2 in TraceInternal.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj2;
						traceListener.Write(message);
						if (TraceInternal.AutoFlush)
						{
							traceListener.Flush();
						}
					}
					return;
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					lock (traceListener2)
					{
						traceListener2.Write(message);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.Write(message);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0002F090 File Offset: 0x0002E090
		public static void Write(object value)
		{
			if (TraceInternal.UseGlobalLock)
			{
				lock (TraceInternal.critSec)
				{
					foreach (object obj2 in TraceInternal.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj2;
						traceListener.Write(value);
						if (TraceInternal.AutoFlush)
						{
							traceListener.Flush();
						}
					}
					return;
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					lock (traceListener2)
					{
						traceListener2.Write(value);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.Write(value);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0002F1BC File Offset: 0x0002E1BC
		public static void Write(string message, string category)
		{
			if (TraceInternal.UseGlobalLock)
			{
				lock (TraceInternal.critSec)
				{
					foreach (object obj2 in TraceInternal.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj2;
						traceListener.Write(message, category);
						if (TraceInternal.AutoFlush)
						{
							traceListener.Flush();
						}
					}
					return;
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					lock (traceListener2)
					{
						traceListener2.Write(message, category);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.Write(message, category);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0002F2E8 File Offset: 0x0002E2E8
		public static void Write(object value, string category)
		{
			if (TraceInternal.UseGlobalLock)
			{
				lock (TraceInternal.critSec)
				{
					foreach (object obj2 in TraceInternal.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj2;
						traceListener.Write(value, category);
						if (TraceInternal.AutoFlush)
						{
							traceListener.Flush();
						}
					}
					return;
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					lock (traceListener2)
					{
						traceListener2.Write(value, category);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.Write(value, category);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0002F414 File Offset: 0x0002E414
		public static void WriteLine(string message)
		{
			if (TraceInternal.UseGlobalLock)
			{
				lock (TraceInternal.critSec)
				{
					foreach (object obj2 in TraceInternal.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj2;
						traceListener.WriteLine(message);
						if (TraceInternal.AutoFlush)
						{
							traceListener.Flush();
						}
					}
					return;
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					lock (traceListener2)
					{
						traceListener2.WriteLine(message);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.WriteLine(message);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0002F540 File Offset: 0x0002E540
		public static void WriteLine(object value)
		{
			if (TraceInternal.UseGlobalLock)
			{
				lock (TraceInternal.critSec)
				{
					foreach (object obj2 in TraceInternal.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj2;
						traceListener.WriteLine(value);
						if (TraceInternal.AutoFlush)
						{
							traceListener.Flush();
						}
					}
					return;
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					lock (traceListener2)
					{
						traceListener2.WriteLine(value);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.WriteLine(value);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0002F66C File Offset: 0x0002E66C
		public static void WriteLine(string message, string category)
		{
			if (TraceInternal.UseGlobalLock)
			{
				lock (TraceInternal.critSec)
				{
					foreach (object obj2 in TraceInternal.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj2;
						traceListener.WriteLine(message, category);
						if (TraceInternal.AutoFlush)
						{
							traceListener.Flush();
						}
					}
					return;
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					lock (traceListener2)
					{
						traceListener2.WriteLine(message, category);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.WriteLine(message, category);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0002F798 File Offset: 0x0002E798
		public static void WriteLine(object value, string category)
		{
			if (TraceInternal.UseGlobalLock)
			{
				lock (TraceInternal.critSec)
				{
					foreach (object obj2 in TraceInternal.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj2;
						traceListener.WriteLine(value, category);
						if (TraceInternal.AutoFlush)
						{
							traceListener.Flush();
						}
					}
					return;
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					lock (traceListener2)
					{
						traceListener2.WriteLine(value, category);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.WriteLine(value, category);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0002F8C4 File Offset: 0x0002E8C4
		public static void WriteIf(bool condition, string message)
		{
			if (condition)
			{
				TraceInternal.Write(message);
			}
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0002F8CF File Offset: 0x0002E8CF
		public static void WriteIf(bool condition, object value)
		{
			if (condition)
			{
				TraceInternal.Write(value);
			}
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0002F8DA File Offset: 0x0002E8DA
		public static void WriteIf(bool condition, string message, string category)
		{
			if (condition)
			{
				TraceInternal.Write(message, category);
			}
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0002F8E6 File Offset: 0x0002E8E6
		public static void WriteIf(bool condition, object value, string category)
		{
			if (condition)
			{
				TraceInternal.Write(value, category);
			}
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0002F8F2 File Offset: 0x0002E8F2
		public static void WriteLineIf(bool condition, string message)
		{
			if (condition)
			{
				TraceInternal.WriteLine(message);
			}
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0002F8FD File Offset: 0x0002E8FD
		public static void WriteLineIf(bool condition, object value)
		{
			if (condition)
			{
				TraceInternal.WriteLine(value);
			}
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0002F908 File Offset: 0x0002E908
		public static void WriteLineIf(bool condition, string message, string category)
		{
			if (condition)
			{
				TraceInternal.WriteLine(message, category);
			}
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0002F914 File Offset: 0x0002E914
		public static void WriteLineIf(bool condition, object value, string category)
		{
			if (condition)
			{
				TraceInternal.WriteLine(value, category);
			}
		}

		// Token: 0x04000F24 RID: 3876
		private static TraceEventCache eventCache = null;

		// Token: 0x04000F25 RID: 3877
		private static string appName = null;

		// Token: 0x04000F26 RID: 3878
		private static TraceListenerCollection listeners;

		// Token: 0x04000F27 RID: 3879
		private static bool autoFlush;

		// Token: 0x04000F28 RID: 3880
		private static bool useGlobalLock;

		// Token: 0x04000F29 RID: 3881
		[ThreadStatic]
		private static int indentLevel;

		// Token: 0x04000F2A RID: 3882
		private static int indentSize;

		// Token: 0x04000F2B RID: 3883
		private static bool settingsInitialized;

		// Token: 0x04000F2C RID: 3884
		private static bool defaultInitialized;

		// Token: 0x04000F2D RID: 3885
		internal static readonly object critSec = new object();
	}
}
