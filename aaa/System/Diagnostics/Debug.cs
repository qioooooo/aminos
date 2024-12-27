using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020001BE RID: 446
	public sealed class Debug
	{
		// Token: 0x06000DC4 RID: 3524 RVA: 0x0002BEAE File Offset: 0x0002AEAE
		private Debug()
		{
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x0002BEB6 File Offset: 0x0002AEB6
		public static TraceListenerCollection Listeners
		{
			[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return TraceInternal.Listeners;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x0002BEBD File Offset: 0x0002AEBD
		// (set) Token: 0x06000DC7 RID: 3527 RVA: 0x0002BEC4 File Offset: 0x0002AEC4
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

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x0002BECC File Offset: 0x0002AECC
		// (set) Token: 0x06000DC9 RID: 3529 RVA: 0x0002BED3 File Offset: 0x0002AED3
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

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x0002BEDB File Offset: 0x0002AEDB
		// (set) Token: 0x06000DCB RID: 3531 RVA: 0x0002BEE2 File Offset: 0x0002AEE2
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

		// Token: 0x06000DCC RID: 3532 RVA: 0x0002BEEA File Offset: 0x0002AEEA
		[Conditional("DEBUG")]
		public static void Flush()
		{
			TraceInternal.Flush();
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0002BEF1 File Offset: 0x0002AEF1
		[Conditional("DEBUG")]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Close()
		{
			TraceInternal.Close();
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0002BEF8 File Offset: 0x0002AEF8
		[Conditional("DEBUG")]
		public static void Assert(bool condition)
		{
			TraceInternal.Assert(condition);
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0002BF00 File Offset: 0x0002AF00
		[Conditional("DEBUG")]
		public static void Assert(bool condition, string message)
		{
			TraceInternal.Assert(condition, message);
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0002BF09 File Offset: 0x0002AF09
		[Conditional("DEBUG")]
		public static void Assert(bool condition, string message, string detailMessage)
		{
			TraceInternal.Assert(condition, message, detailMessage);
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0002BF13 File Offset: 0x0002AF13
		[Conditional("DEBUG")]
		public static void Fail(string message)
		{
			TraceInternal.Fail(message);
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0002BF1B File Offset: 0x0002AF1B
		[Conditional("DEBUG")]
		public static void Fail(string message, string detailMessage)
		{
			TraceInternal.Fail(message, detailMessage);
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0002BF24 File Offset: 0x0002AF24
		[Conditional("DEBUG")]
		public static void Print(string message)
		{
			TraceInternal.WriteLine(message);
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0002BF2C File Offset: 0x0002AF2C
		[Conditional("DEBUG")]
		public static void Print(string format, params object[] args)
		{
			TraceInternal.WriteLine(string.Format(CultureInfo.InvariantCulture, format, args));
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0002BF3F File Offset: 0x0002AF3F
		[Conditional("DEBUG")]
		public static void Write(string message)
		{
			TraceInternal.Write(message);
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0002BF47 File Offset: 0x0002AF47
		[Conditional("DEBUG")]
		public static void Write(object value)
		{
			TraceInternal.Write(value);
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0002BF4F File Offset: 0x0002AF4F
		[Conditional("DEBUG")]
		public static void Write(string message, string category)
		{
			TraceInternal.Write(message, category);
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0002BF58 File Offset: 0x0002AF58
		[Conditional("DEBUG")]
		public static void Write(object value, string category)
		{
			TraceInternal.Write(value, category);
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0002BF61 File Offset: 0x0002AF61
		[Conditional("DEBUG")]
		public static void WriteLine(string message)
		{
			TraceInternal.WriteLine(message);
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0002BF69 File Offset: 0x0002AF69
		[Conditional("DEBUG")]
		public static void WriteLine(object value)
		{
			TraceInternal.WriteLine(value);
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0002BF71 File Offset: 0x0002AF71
		[Conditional("DEBUG")]
		public static void WriteLine(string message, string category)
		{
			TraceInternal.WriteLine(message, category);
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0002BF7A File Offset: 0x0002AF7A
		[Conditional("DEBUG")]
		public static void WriteLine(object value, string category)
		{
			TraceInternal.WriteLine(value, category);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0002BF83 File Offset: 0x0002AF83
		[Conditional("DEBUG")]
		public static void WriteIf(bool condition, string message)
		{
			TraceInternal.WriteIf(condition, message);
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0002BF8C File Offset: 0x0002AF8C
		[Conditional("DEBUG")]
		public static void WriteIf(bool condition, object value)
		{
			TraceInternal.WriteIf(condition, value);
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0002BF95 File Offset: 0x0002AF95
		[Conditional("DEBUG")]
		public static void WriteIf(bool condition, string message, string category)
		{
			TraceInternal.WriteIf(condition, message, category);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0002BF9F File Offset: 0x0002AF9F
		[Conditional("DEBUG")]
		public static void WriteIf(bool condition, object value, string category)
		{
			TraceInternal.WriteIf(condition, value, category);
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0002BFA9 File Offset: 0x0002AFA9
		[Conditional("DEBUG")]
		public static void WriteLineIf(bool condition, string message)
		{
			TraceInternal.WriteLineIf(condition, message);
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0002BFB2 File Offset: 0x0002AFB2
		[Conditional("DEBUG")]
		public static void WriteLineIf(bool condition, object value)
		{
			TraceInternal.WriteLineIf(condition, value);
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0002BFBB File Offset: 0x0002AFBB
		[Conditional("DEBUG")]
		public static void WriteLineIf(bool condition, string message, string category)
		{
			TraceInternal.WriteLineIf(condition, message, category);
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0002BFC5 File Offset: 0x0002AFC5
		[Conditional("DEBUG")]
		public static void WriteLineIf(bool condition, object value, string category)
		{
			TraceInternal.WriteLineIf(condition, value, category);
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0002BFCF File Offset: 0x0002AFCF
		[Conditional("DEBUG")]
		public static void Indent()
		{
			TraceInternal.Indent();
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0002BFD6 File Offset: 0x0002AFD6
		[Conditional("DEBUG")]
		public static void Unindent()
		{
			TraceInternal.Unindent();
		}
	}
}
