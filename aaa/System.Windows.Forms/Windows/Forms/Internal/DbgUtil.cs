using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.Windows.Forms.Internal
{
	// Token: 0x02000040 RID: 64
	[UIPermission(SecurityAction.Assert, Unrestricted = true)]
	[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
	[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
	[ReflectionPermission(SecurityAction.Assert, TypeInformation = true, MemberAccess = true)]
	internal sealed class DbgUtil
	{
		// Token: 0x060001CD RID: 461
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetUserDefaultLCID();

		// Token: 0x060001CE RID: 462
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int FormatMessage(int dwFlags, HandleRef lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, HandleRef arguments);

		// Token: 0x060001CF RID: 463 RVA: 0x00006D07 File Offset: 0x00005D07
		[Conditional("DEBUG")]
		public static void AssertFinalization(object obj, bool disposing)
		{
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00006D09 File Offset: 0x00005D09
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string message)
		{
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00006D0B File Offset: 0x00005D0B
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1)
		{
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00006D0D File Offset: 0x00005D0D
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2)
		{
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00006D0F File Offset: 0x00005D0F
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2, object arg3)
		{
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00006D11 File Offset: 0x00005D11
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2, object arg3, object arg4)
		{
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00006D13 File Offset: 0x00005D13
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2, object arg3, object arg4, object arg5)
		{
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00006D15 File Offset: 0x00005D15
		[Conditional("DEBUG")]
		private static void AssertWin32Impl(bool expression, string format, object[] args)
		{
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00006D18 File Offset: 0x00005D18
		public static string GetLastErrorStr()
		{
			int num = 255;
			StringBuilder stringBuilder = new StringBuilder(num);
			string text = string.Empty;
			int num2 = 0;
			try
			{
				num2 = Marshal.GetLastWin32Error();
				text = ((DbgUtil.FormatMessage(4608, new HandleRef(null, IntPtr.Zero), num2, DbgUtil.GetUserDefaultLCID(), stringBuilder, num, new HandleRef(null, IntPtr.Zero)) != 0) ? stringBuilder.ToString() : "<error returned>");
			}
			catch (Exception ex)
			{
				if (DbgUtil.IsCriticalException(ex))
				{
					throw;
				}
				text = ex.ToString();
			}
			return string.Format(CultureInfo.CurrentCulture, "0x{0:x8} - {1}", new object[] { num2, text });
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00006DD0 File Offset: 0x00005DD0
		private static bool IsCriticalException(Exception ex)
		{
			return ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException;
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00006DED File Offset: 0x00005DED
		public static string StackTrace
		{
			get
			{
				return Environment.StackTrace;
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00006DF4 File Offset: 0x00005DF4
		public static string StackFramesToStr(int maxFrameCount)
		{
			string text = string.Empty;
			try
			{
				StackTrace stackTrace = new StackTrace(true);
				int i;
				for (i = 0; i < stackTrace.FrameCount; i++)
				{
					StackFrame frame = stackTrace.GetFrame(i);
					if (frame == null || frame.GetMethod().DeclaringType != typeof(DbgUtil))
					{
						break;
					}
				}
				maxFrameCount += i;
				if (maxFrameCount > stackTrace.FrameCount)
				{
					maxFrameCount = stackTrace.FrameCount;
				}
				for (int j = i; j < maxFrameCount; j++)
				{
					StackFrame frame2 = stackTrace.GetFrame(j);
					if (frame2 != null)
					{
						MethodBase method = frame2.GetMethod();
						if (method != null)
						{
							string text2 = string.Empty;
							string text3 = frame2.GetFileName();
							int num = ((text3 == null) ? (-1) : text3.LastIndexOf('\\'));
							if (num != -1)
							{
								text3 = text3.Substring(num + 1, text3.Length - num - 1);
							}
							foreach (ParameterInfo parameterInfo in method.GetParameters())
							{
								text2 = text2 + parameterInfo.ParameterType.Name + ", ";
							}
							if (text2.Length > 0)
							{
								text2 = text2.Substring(0, text2.Length - 2);
							}
							text += string.Format(CultureInfo.CurrentCulture, "at {0}:{1} {2}.{3}({4})\r\n", new object[]
							{
								text3,
								frame2.GetFileLineNumber(),
								method.DeclaringType,
								method.Name,
								text2
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (DbgUtil.IsCriticalException(ex))
				{
					throw;
				}
				text += ex.ToString();
			}
			return text.ToString();
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00006FBC File Offset: 0x00005FBC
		public static string StackFramesToStr()
		{
			return DbgUtil.StackFramesToStr(DbgUtil.gdipInitMaxFrameCount);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00006FC8 File Offset: 0x00005FC8
		public static string StackTraceToStr(string message, int frameCount)
		{
			return string.Format(CultureInfo.CurrentCulture, "{0}\r\nTop Stack Trace:\r\n{1}", new object[]
			{
				message,
				DbgUtil.StackFramesToStr(frameCount)
			});
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00006FF9 File Offset: 0x00005FF9
		public static string StackTraceToStr(string message)
		{
			return DbgUtil.StackTraceToStr(message, DbgUtil.gdipInitMaxFrameCount);
		}

		// Token: 0x04000B66 RID: 2918
		public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;

		// Token: 0x04000B67 RID: 2919
		public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04000B68 RID: 2920
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04000B69 RID: 2921
		public const int FORMAT_MESSAGE_DEFAULT = 4608;

		// Token: 0x04000B6A RID: 2922
		public static int gdipInitMaxFrameCount = 8;

		// Token: 0x04000B6B RID: 2923
		public static int gdiUseMaxFrameCount = 8;

		// Token: 0x04000B6C RID: 2924
		public static int finalizeMaxFrameCount = 5;
	}
}
