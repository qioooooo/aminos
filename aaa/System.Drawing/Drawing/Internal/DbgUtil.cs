using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.Drawing.Internal
{
	// Token: 0x02000017 RID: 23
	[UIPermission(SecurityAction.Assert, Unrestricted = true)]
	[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
	[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
	[ReflectionPermission(SecurityAction.Assert, TypeInformation = true, MemberAccess = true)]
	internal sealed class DbgUtil
	{
		// Token: 0x0600006A RID: 106
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetUserDefaultLCID();

		// Token: 0x0600006B RID: 107
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int FormatMessage(int dwFlags, HandleRef lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, HandleRef arguments);

		// Token: 0x0600006C RID: 108 RVA: 0x000034D1 File Offset: 0x000024D1
		[Conditional("DEBUG")]
		public static void AssertFinalization(object obj, bool disposing)
		{
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000034D3 File Offset: 0x000024D3
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string message)
		{
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000034D5 File Offset: 0x000024D5
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1)
		{
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000034D7 File Offset: 0x000024D7
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2)
		{
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000034D9 File Offset: 0x000024D9
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2, object arg3)
		{
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000034DB File Offset: 0x000024DB
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2, object arg3, object arg4)
		{
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000034DD File Offset: 0x000024DD
		[Conditional("DEBUG")]
		public static void AssertWin32(bool expression, string format, object arg1, object arg2, object arg3, object arg4, object arg5)
		{
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000034DF File Offset: 0x000024DF
		[Conditional("DEBUG")]
		private static void AssertWin32Impl(bool expression, string format, object[] args)
		{
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000034E4 File Offset: 0x000024E4
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

		// Token: 0x06000075 RID: 117 RVA: 0x0000359C File Offset: 0x0000259C
		private static bool IsCriticalException(Exception ex)
		{
			return ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000076 RID: 118 RVA: 0x000035B9 File Offset: 0x000025B9
		public static string StackTrace
		{
			get
			{
				return Environment.StackTrace;
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000035C0 File Offset: 0x000025C0
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

		// Token: 0x06000078 RID: 120 RVA: 0x00003788 File Offset: 0x00002788
		public static string StackFramesToStr()
		{
			return DbgUtil.StackFramesToStr(DbgUtil.gdipInitMaxFrameCount);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003794 File Offset: 0x00002794
		public static string StackTraceToStr(string message, int frameCount)
		{
			return string.Format(CultureInfo.CurrentCulture, "{0}\r\nTop Stack Trace:\r\n{1}", new object[]
			{
				message,
				DbgUtil.StackFramesToStr(frameCount)
			});
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000037C5 File Offset: 0x000027C5
		public static string StackTraceToStr(string message)
		{
			return DbgUtil.StackTraceToStr(message, DbgUtil.gdipInitMaxFrameCount);
		}

		// Token: 0x040000D5 RID: 213
		public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;

		// Token: 0x040000D6 RID: 214
		public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x040000D7 RID: 215
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x040000D8 RID: 216
		public const int FORMAT_MESSAGE_DEFAULT = 4608;

		// Token: 0x040000D9 RID: 217
		public static int gdipInitMaxFrameCount = 8;

		// Token: 0x040000DA RID: 218
		public static int gdiUseMaxFrameCount = 8;

		// Token: 0x040000DB RID: 219
		public static int finalizeMaxFrameCount = 5;
	}
}
