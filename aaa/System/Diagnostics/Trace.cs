using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020001D5 RID: 469
	public sealed class Trace
	{
		// Token: 0x06000E8F RID: 3727 RVA: 0x0002E1F4 File Offset: 0x0002D1F4
		private Trace()
		{
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000E90 RID: 3728 RVA: 0x0002E1FC File Offset: 0x0002D1FC
		public static TraceListenerCollection Listeners
		{
			[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
			get
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
				return TraceInternal.Listeners;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x0002E20E File Offset: 0x0002D20E
		// (set) Token: 0x06000E92 RID: 3730 RVA: 0x0002E215 File Offset: 0x0002D215
		public static bool AutoFlush
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return TraceInternal.AutoFlush;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				TraceInternal.AutoFlush = value;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x0002E21D File Offset: 0x0002D21D
		// (set) Token: 0x06000E94 RID: 3732 RVA: 0x0002E224 File Offset: 0x0002D224
		public static bool UseGlobalLock
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return TraceInternal.UseGlobalLock;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				TraceInternal.UseGlobalLock = value;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000E95 RID: 3733 RVA: 0x0002E22C File Offset: 0x0002D22C
		public static CorrelationManager CorrelationManager
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				if (Trace.correlationManager == null)
				{
					Trace.correlationManager = new CorrelationManager();
				}
				return Trace.correlationManager;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000E96 RID: 3734 RVA: 0x0002E244 File Offset: 0x0002D244
		// (set) Token: 0x06000E97 RID: 3735 RVA: 0x0002E24B File Offset: 0x0002D24B
		public static int IndentLevel
		{
			get
			{
				return TraceInternal.IndentLevel;
			}
			set
			{
				TraceInternal.IndentLevel = value;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x0002E253 File Offset: 0x0002D253
		// (set) Token: 0x06000E99 RID: 3737 RVA: 0x0002E25A File Offset: 0x0002D25A
		public static int IndentSize
		{
			get
			{
				return TraceInternal.IndentSize;
			}
			set
			{
				TraceInternal.IndentSize = value;
			}
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0002E262 File Offset: 0x0002D262
		[Conditional("TRACE")]
		public static void Flush()
		{
			TraceInternal.Flush();
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0002E269 File Offset: 0x0002D269
		[Conditional("TRACE")]
		public static void Close()
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			TraceInternal.Close();
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x0002E27B File Offset: 0x0002D27B
		[Conditional("TRACE")]
		public static void Assert(bool condition)
		{
			TraceInternal.Assert(condition);
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x0002E283 File Offset: 0x0002D283
		[Conditional("TRACE")]
		public static void Assert(bool condition, string message)
		{
			TraceInternal.Assert(condition, message);
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0002E28C File Offset: 0x0002D28C
		[Conditional("TRACE")]
		public static void Assert(bool condition, string message, string detailMessage)
		{
			TraceInternal.Assert(condition, message, detailMessage);
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0002E296 File Offset: 0x0002D296
		[Conditional("TRACE")]
		public static void Fail(string message)
		{
			TraceInternal.Fail(message);
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0002E29E File Offset: 0x0002D29E
		[Conditional("TRACE")]
		public static void Fail(string message, string detailMessage)
		{
			TraceInternal.Fail(message, detailMessage);
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0002E2A7 File Offset: 0x0002D2A7
		public static void Refresh()
		{
			DiagnosticsConfiguration.Refresh();
			Switch.RefreshAll();
			TraceSource.RefreshAll();
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x0002E2B8 File Offset: 0x0002D2B8
		[Conditional("TRACE")]
		public static void TraceInformation(string message)
		{
			TraceInternal.TraceEvent(TraceEventType.Information, 0, message, null);
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0002E2C3 File Offset: 0x0002D2C3
		[Conditional("TRACE")]
		public static void TraceInformation(string format, params object[] args)
		{
			TraceInternal.TraceEvent(TraceEventType.Information, 0, format, args);
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0002E2CE File Offset: 0x0002D2CE
		[Conditional("TRACE")]
		public static void TraceWarning(string message)
		{
			TraceInternal.TraceEvent(TraceEventType.Warning, 0, message, null);
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x0002E2D9 File Offset: 0x0002D2D9
		[Conditional("TRACE")]
		public static void TraceWarning(string format, params object[] args)
		{
			TraceInternal.TraceEvent(TraceEventType.Warning, 0, format, args);
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0002E2E4 File Offset: 0x0002D2E4
		[Conditional("TRACE")]
		public static void TraceError(string message)
		{
			TraceInternal.TraceEvent(TraceEventType.Error, 0, message, null);
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0002E2EF File Offset: 0x0002D2EF
		[Conditional("TRACE")]
		public static void TraceError(string format, params object[] args)
		{
			TraceInternal.TraceEvent(TraceEventType.Error, 0, format, args);
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0002E2FA File Offset: 0x0002D2FA
		[Conditional("TRACE")]
		public static void Write(string message)
		{
			TraceInternal.Write(message);
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0002E302 File Offset: 0x0002D302
		[Conditional("TRACE")]
		public static void Write(object value)
		{
			TraceInternal.Write(value);
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0002E30A File Offset: 0x0002D30A
		[Conditional("TRACE")]
		public static void Write(string message, string category)
		{
			TraceInternal.Write(message, category);
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0002E313 File Offset: 0x0002D313
		[Conditional("TRACE")]
		public static void Write(object value, string category)
		{
			TraceInternal.Write(value, category);
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0002E31C File Offset: 0x0002D31C
		[Conditional("TRACE")]
		public static void WriteLine(string message)
		{
			TraceInternal.WriteLine(message);
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0002E324 File Offset: 0x0002D324
		[Conditional("TRACE")]
		public static void WriteLine(object value)
		{
			TraceInternal.WriteLine(value);
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x0002E32C File Offset: 0x0002D32C
		[Conditional("TRACE")]
		public static void WriteLine(string message, string category)
		{
			TraceInternal.WriteLine(message, category);
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0002E335 File Offset: 0x0002D335
		[Conditional("TRACE")]
		public static void WriteLine(object value, string category)
		{
			TraceInternal.WriteLine(value, category);
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0002E33E File Offset: 0x0002D33E
		[Conditional("TRACE")]
		public static void WriteIf(bool condition, string message)
		{
			TraceInternal.WriteIf(condition, message);
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0002E347 File Offset: 0x0002D347
		[Conditional("TRACE")]
		public static void WriteIf(bool condition, object value)
		{
			TraceInternal.WriteIf(condition, value);
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0002E350 File Offset: 0x0002D350
		[Conditional("TRACE")]
		public static void WriteIf(bool condition, string message, string category)
		{
			TraceInternal.WriteIf(condition, message, category);
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0002E35A File Offset: 0x0002D35A
		[Conditional("TRACE")]
		public static void WriteIf(bool condition, object value, string category)
		{
			TraceInternal.WriteIf(condition, value, category);
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x0002E364 File Offset: 0x0002D364
		[Conditional("TRACE")]
		public static void WriteLineIf(bool condition, string message)
		{
			TraceInternal.WriteLineIf(condition, message);
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0002E36D File Offset: 0x0002D36D
		[Conditional("TRACE")]
		public static void WriteLineIf(bool condition, object value)
		{
			TraceInternal.WriteLineIf(condition, value);
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0002E376 File Offset: 0x0002D376
		[Conditional("TRACE")]
		public static void WriteLineIf(bool condition, string message, string category)
		{
			TraceInternal.WriteLineIf(condition, message, category);
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0002E380 File Offset: 0x0002D380
		[Conditional("TRACE")]
		public static void WriteLineIf(bool condition, object value, string category)
		{
			TraceInternal.WriteLineIf(condition, value, category);
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x0002E38A File Offset: 0x0002D38A
		[Conditional("TRACE")]
		public static void Indent()
		{
			TraceInternal.Indent();
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0002E391 File Offset: 0x0002D391
		[Conditional("TRACE")]
		public static void Unindent()
		{
			TraceInternal.Unindent();
		}

		// Token: 0x04000F13 RID: 3859
		private static CorrelationManager correlationManager;
	}
}
