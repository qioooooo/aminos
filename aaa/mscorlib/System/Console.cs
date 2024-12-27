using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System
{
	// Token: 0x0200008A RID: 138
	public static class Console
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x000184C8 File Offset: 0x000174C8
		private static object InternalSyncObject
		{
			get
			{
				if (Console.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref Console.s_InternalSyncObject, obj, null);
				}
				return Console.s_InternalSyncObject;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x000184F4 File Offset: 0x000174F4
		private static IntPtr ConsoleInputHandle
		{
			get
			{
				if (Console._consoleInputHandle == IntPtr.Zero)
				{
					Console._consoleInputHandle = Win32Native.GetStdHandle(-10);
				}
				return Console._consoleInputHandle;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x00018518 File Offset: 0x00017518
		private static IntPtr ConsoleOutputHandle
		{
			get
			{
				if (Console._consoleOutputHandle == IntPtr.Zero)
				{
					Console._consoleOutputHandle = Win32Native.GetStdHandle(-11);
				}
				return Console._consoleOutputHandle;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x0001853C File Offset: 0x0001753C
		public static TextWriter Error
		{
			[HostProtection(SecurityAction.LinkDemand, UI = true)]
			get
			{
				if (Console._error == null)
				{
					Console.InitializeStdOutError(false);
				}
				return Console._error;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x00018550 File Offset: 0x00017550
		public static TextReader In
		{
			[HostProtection(SecurityAction.LinkDemand, UI = true)]
			get
			{
				if (Console._in == null)
				{
					lock (Console.InternalSyncObject)
					{
						if (Console._in == null)
						{
							Stream stream = Console.OpenStandardInput(256);
							TextReader textReader;
							if (stream == Stream.Null)
							{
								textReader = StreamReader.Null;
							}
							else
							{
								Encoding encoding = Encoding.GetEncoding((int)Win32Native.GetConsoleCP());
								textReader = TextReader.Synchronized(new StreamReader(stream, encoding, false, 256, false));
							}
							Thread.MemoryBarrier();
							Console._in = textReader;
						}
					}
				}
				return Console._in;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x000185DC File Offset: 0x000175DC
		public static TextWriter Out
		{
			[HostProtection(SecurityAction.LinkDemand, UI = true)]
			get
			{
				if (Console._out == null)
				{
					Console.InitializeStdOutError(true);
				}
				return Console._out;
			}
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x000185F0 File Offset: 0x000175F0
		private static void InitializeStdOutError(bool stdout)
		{
			lock (Console.InternalSyncObject)
			{
				if (!stdout || Console._out == null)
				{
					if (stdout || Console._error == null)
					{
						Stream stream;
						if (stdout)
						{
							stream = Console.OpenStandardOutput(256);
						}
						else
						{
							stream = Console.OpenStandardError(256);
						}
						TextWriter textWriter;
						if (stream == Stream.Null)
						{
							textWriter = TextWriter.Synchronized(StreamWriter.Null);
						}
						else
						{
							int consoleOutputCP = (int)Win32Native.GetConsoleOutputCP();
							Encoding encoding = Encoding.GetEncoding(consoleOutputCP);
							textWriter = TextWriter.Synchronized(new StreamWriter(stream, encoding, 256, false)
							{
								HaveWrittenPreamble = true,
								AutoFlush = true
							});
						}
						if (stdout)
						{
							Console._out = textWriter;
						}
						else
						{
							Console._error = textWriter;
						}
					}
				}
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x000186BC File Offset: 0x000176BC
		private static Stream GetStandardFile(int stdHandleName, FileAccess access, int bufferSize)
		{
			IntPtr stdHandle = Win32Native.GetStdHandle(stdHandleName);
			SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, false);
			if (safeFileHandle.IsInvalid)
			{
				safeFileHandle.SetHandleAsInvalid();
				return Stream.Null;
			}
			if (stdHandleName != -10 && !Console.ConsoleHandleIsValid(safeFileHandle))
			{
				return Stream.Null;
			}
			return new __ConsoleStream(safeFileHandle, access);
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x00018708 File Offset: 0x00017708
		// (set) Token: 0x06000782 RID: 1922 RVA: 0x00018724 File Offset: 0x00017724
		public static Encoding InputEncoding
		{
			get
			{
				uint consoleCP = Win32Native.GetConsoleCP();
				return Encoding.GetEncoding((int)consoleCP);
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (Environment.IsWin9X())
				{
					throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
				}
				new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
				uint codePage = (uint)value.CodePage;
				lock (Console.InternalSyncObject)
				{
					if (!Win32Native.SetConsoleCP(codePage))
					{
						__Error.WinIOError();
					}
					Console._in = null;
				}
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x000187A4 File Offset: 0x000177A4
		// (set) Token: 0x06000784 RID: 1924 RVA: 0x000187C0 File Offset: 0x000177C0
		public static Encoding OutputEncoding
		{
			get
			{
				uint consoleOutputCP = Win32Native.GetConsoleOutputCP();
				return Encoding.GetEncoding((int)consoleOutputCP);
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (Environment.IsWin9X())
				{
					throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
				}
				new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
				lock (Console.InternalSyncObject)
				{
					if (Console._out != null && !Console._wasOutRedirected)
					{
						Console._out.Flush();
						Console._out = null;
					}
					if (Console._error != null && !Console._wasErrorRedirected)
					{
						Console._error.Flush();
						Console._error = null;
					}
					uint codePage = (uint)value.CodePage;
					if (!Win32Native.SetConsoleOutputCP(codePage))
					{
						__Error.WinIOError();
					}
				}
			}
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00018874 File Offset: 0x00017874
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Beep()
		{
			Console.Beep(800, 200);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00018888 File Offset: 0x00017888
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Beep(int frequency, int duration)
		{
			if (frequency < 37 || frequency > 32767)
			{
				throw new ArgumentOutOfRangeException("frequency", frequency, Environment.GetResourceString("ArgumentOutOfRange_BeepFrequency", new object[] { 37, 32767 }));
			}
			if (duration <= 0)
			{
				throw new ArgumentOutOfRangeException("duration", duration, Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			Win32Native.Beep(frequency, duration);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00018904 File Offset: 0x00017904
		public static void Clear()
		{
			Win32Native.COORD coord = default(Win32Native.COORD);
			IntPtr consoleOutputHandle = Console.ConsoleOutputHandle;
			if (consoleOutputHandle == Win32Native.INVALID_HANDLE_VALUE)
			{
				throw new IOException(Environment.GetResourceString("IO.IO_NoConsole"));
			}
			Win32Native.CONSOLE_SCREEN_BUFFER_INFO bufferInfo = Console.GetBufferInfo();
			int num = (int)(bufferInfo.dwSize.X * bufferInfo.dwSize.Y);
			int num2 = 0;
			if (!Win32Native.FillConsoleOutputCharacter(consoleOutputHandle, ' ', num, coord, out num2))
			{
				__Error.WinIOError();
			}
			num2 = 0;
			if (!Win32Native.FillConsoleOutputAttribute(consoleOutputHandle, bufferInfo.wAttributes, num, coord, out num2))
			{
				__Error.WinIOError();
			}
			if (!Win32Native.SetConsoleCursorPosition(consoleOutputHandle, coord))
			{
				__Error.WinIOError();
			}
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x000189A8 File Offset: 0x000179A8
		private static Win32Native.Color ConsoleColorToColorAttribute(ConsoleColor color, bool isBackground)
		{
			if ((color & (ConsoleColor)(-16)) != ConsoleColor.Black)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidConsoleColor"));
			}
			Win32Native.Color color2 = (Win32Native.Color)color;
			if (isBackground)
			{
				color2 <<= 4;
			}
			return color2;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x000189D7 File Offset: 0x000179D7
		private static ConsoleColor ColorAttributeToConsoleColor(Win32Native.Color c)
		{
			if ((short)(c & Win32Native.Color.BackgroundMask) != 0)
			{
				c >>= 4;
			}
			return (ConsoleColor)c;
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x000189EC File Offset: 0x000179EC
		// (set) Token: 0x0600078B RID: 1931 RVA: 0x00018A1C File Offset: 0x00017A1C
		public static ConsoleColor BackgroundColor
		{
			get
			{
				bool flag;
				Win32Native.CONSOLE_SCREEN_BUFFER_INFO bufferInfo = Console.GetBufferInfo(false, out flag);
				if (!flag)
				{
					return ConsoleColor.Black;
				}
				Win32Native.Color color = (Win32Native.Color)(bufferInfo.wAttributes & 240);
				return Console.ColorAttributeToConsoleColor(color);
			}
			set
			{
				new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
				Win32Native.Color color = Console.ConsoleColorToColorAttribute(value, true);
				bool flag;
				Win32Native.CONSOLE_SCREEN_BUFFER_INFO bufferInfo = Console.GetBufferInfo(false, out flag);
				if (!flag)
				{
					return;
				}
				short num = bufferInfo.wAttributes;
				num &= -241;
				num = (short)((ushort)num | (ushort)color);
				Win32Native.SetConsoleTextAttribute(Console.ConsoleOutputHandle, num);
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x00018A70 File Offset: 0x00017A70
		// (set) Token: 0x0600078D RID: 1933 RVA: 0x00018AA0 File Offset: 0x00017AA0
		public static ConsoleColor ForegroundColor
		{
			get
			{
				bool flag;
				Win32Native.CONSOLE_SCREEN_BUFFER_INFO bufferInfo = Console.GetBufferInfo(false, out flag);
				if (!flag)
				{
					return ConsoleColor.Gray;
				}
				Win32Native.Color color = (Win32Native.Color)(bufferInfo.wAttributes & 15);
				return Console.ColorAttributeToConsoleColor(color);
			}
			set
			{
				new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
				Win32Native.Color color = Console.ConsoleColorToColorAttribute(value, false);
				bool flag;
				Win32Native.CONSOLE_SCREEN_BUFFER_INFO bufferInfo = Console.GetBufferInfo(false, out flag);
				if (!flag)
				{
					return;
				}
				short num = bufferInfo.wAttributes;
				num &= -16;
				num = (short)((ushort)num | (ushort)color);
				Win32Native.SetConsoleTextAttribute(Console.ConsoleOutputHandle, num);
			}
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00018AF0 File Offset: 0x00017AF0
		public static void ResetColor()
		{
			new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
			bool flag;
			Console.GetBufferInfo(false, out flag);
			if (!flag)
			{
				return;
			}
			short num = (short)Console._defaultColors;
			Win32Native.SetConsoleTextAttribute(Console.ConsoleOutputHandle, num);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00018B28 File Offset: 0x00017B28
		public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
		{
			Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, ' ', ConsoleColor.Black, Console.BackgroundColor);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00018B4C File Offset: 0x00017B4C
		public unsafe static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
		{
			if (sourceForeColor < ConsoleColor.Black || sourceForeColor > ConsoleColor.White)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidConsoleColor"), "sourceForeColor");
			}
			if (sourceBackColor < ConsoleColor.Black || sourceBackColor > ConsoleColor.White)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidConsoleColor"), "sourceBackColor");
			}
			Win32Native.COORD dwSize = Console.GetBufferInfo().dwSize;
			if (sourceLeft < 0 || sourceLeft > (int)dwSize.X)
			{
				throw new ArgumentOutOfRangeException("sourceLeft", sourceLeft, Environment.GetResourceString("ArgumentOutOfRange_ConsoleBufferBoundaries"));
			}
			if (sourceTop < 0 || sourceTop > (int)dwSize.Y)
			{
				throw new ArgumentOutOfRangeException("sourceTop", sourceTop, Environment.GetResourceString("ArgumentOutOfRange_ConsoleBufferBoundaries"));
			}
			if (sourceWidth < 0 || sourceWidth > (int)dwSize.X - sourceLeft)
			{
				throw new ArgumentOutOfRangeException("sourceWidth", sourceWidth, Environment.GetResourceString("ArgumentOutOfRange_ConsoleBufferBoundaries"));
			}
			if (sourceHeight < 0 || sourceTop > (int)dwSize.Y - sourceHeight)
			{
				throw new ArgumentOutOfRangeException("sourceHeight", sourceHeight, Environment.GetResourceString("ArgumentOutOfRange_ConsoleBufferBoundaries"));
			}
			if (targetLeft < 0 || targetLeft > (int)dwSize.X)
			{
				throw new ArgumentOutOfRangeException("targetLeft", targetLeft, Environment.GetResourceString("ArgumentOutOfRange_ConsoleBufferBoundaries"));
			}
			if (targetTop < 0 || targetTop > (int)dwSize.Y)
			{
				throw new ArgumentOutOfRangeException("targetTop", targetTop, Environment.GetResourceString("ArgumentOutOfRange_ConsoleBufferBoundaries"));
			}
			if (sourceWidth == 0 || sourceHeight == 0)
			{
				return;
			}
			new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
			Win32Native.CHAR_INFO[] array = new Win32Native.CHAR_INFO[sourceWidth * sourceHeight];
			dwSize.X = (short)sourceWidth;
			dwSize.Y = (short)sourceHeight;
			Win32Native.COORD coord = default(Win32Native.COORD);
			Win32Native.SMALL_RECT small_RECT = default(Win32Native.SMALL_RECT);
			small_RECT.Left = (short)sourceLeft;
			small_RECT.Right = (short)(sourceLeft + sourceWidth - 1);
			small_RECT.Top = (short)sourceTop;
			small_RECT.Bottom = (short)(sourceTop + sourceHeight - 1);
			bool flag;
			fixed (Win32Native.CHAR_INFO* ptr = array)
			{
				flag = Win32Native.ReadConsoleOutput(Console.ConsoleOutputHandle, ptr, dwSize, coord, ref small_RECT);
			}
			if (!flag)
			{
				__Error.WinIOError();
			}
			Win32Native.COORD coord2 = default(Win32Native.COORD);
			coord2.X = (short)sourceLeft;
			Win32Native.Color color = Console.ConsoleColorToColorAttribute(sourceBackColor, true);
			color |= Console.ConsoleColorToColorAttribute(sourceForeColor, false);
			short num = (short)color;
			for (int i = sourceTop; i < sourceTop + sourceHeight; i++)
			{
				coord2.Y = (short)i;
				int num2;
				if (!Win32Native.FillConsoleOutputCharacter(Console.ConsoleOutputHandle, sourceChar, sourceWidth, coord2, out num2))
				{
					__Error.WinIOError();
				}
				if (!Win32Native.FillConsoleOutputAttribute(Console.ConsoleOutputHandle, num, sourceWidth, coord2, out num2))
				{
					__Error.WinIOError();
				}
			}
			Win32Native.SMALL_RECT small_RECT2 = default(Win32Native.SMALL_RECT);
			small_RECT2.Left = (short)targetLeft;
			small_RECT2.Right = (short)(targetLeft + sourceWidth);
			small_RECT2.Top = (short)targetTop;
			small_RECT2.Bottom = (short)(targetTop + sourceHeight);
			fixed (Win32Native.CHAR_INFO* ptr2 = array)
			{
				flag = Win32Native.WriteConsoleOutput(Console.ConsoleOutputHandle, ptr2, dwSize, coord, ref small_RECT2);
			}
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00018E34 File Offset: 0x00017E34
		private static Win32Native.CONSOLE_SCREEN_BUFFER_INFO GetBufferInfo()
		{
			bool flag;
			return Console.GetBufferInfo(true, out flag);
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00018E4C File Offset: 0x00017E4C
		private static Win32Native.CONSOLE_SCREEN_BUFFER_INFO GetBufferInfo(bool throwOnNoConsole, out bool succeeded)
		{
			succeeded = false;
			IntPtr consoleOutputHandle = Console.ConsoleOutputHandle;
			if (!(consoleOutputHandle == Win32Native.INVALID_HANDLE_VALUE))
			{
				Win32Native.CONSOLE_SCREEN_BUFFER_INFO console_SCREEN_BUFFER_INFO;
				if (!Win32Native.GetConsoleScreenBufferInfo(consoleOutputHandle, out console_SCREEN_BUFFER_INFO))
				{
					bool flag = Win32Native.GetConsoleScreenBufferInfo(Win32Native.GetStdHandle(-12), out console_SCREEN_BUFFER_INFO);
					if (!flag)
					{
						flag = Win32Native.GetConsoleScreenBufferInfo(Win32Native.GetStdHandle(-10), out console_SCREEN_BUFFER_INFO);
					}
					if (!flag)
					{
						int lastWin32Error = Marshal.GetLastWin32Error();
						if (lastWin32Error == 6 && !throwOnNoConsole)
						{
							return default(Win32Native.CONSOLE_SCREEN_BUFFER_INFO);
						}
						__Error.WinIOError(lastWin32Error, null);
					}
				}
				if (!Console._haveReadDefaultColors)
				{
					Console._defaultColors = (byte)(console_SCREEN_BUFFER_INFO.wAttributes & 255);
					Console._haveReadDefaultColors = true;
				}
				succeeded = true;
				return console_SCREEN_BUFFER_INFO;
			}
			if (!throwOnNoConsole)
			{
				return default(Win32Native.CONSOLE_SCREEN_BUFFER_INFO);
			}
			throw new IOException(Environment.GetResourceString("IO.IO_NoConsole"));
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x00018F00 File Offset: 0x00017F00
		// (set) Token: 0x06000794 RID: 1940 RVA: 0x00018F1F File Offset: 0x00017F1F
		public static int BufferHeight
		{
			get
			{
				return (int)Console.GetBufferInfo().dwSize.Y;
			}
			set
			{
				Console.SetBufferSize(Console.BufferWidth, value);
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x00018F2C File Offset: 0x00017F2C
		// (set) Token: 0x06000796 RID: 1942 RVA: 0x00018F4B File Offset: 0x00017F4B
		public static int BufferWidth
		{
			get
			{
				return (int)Console.GetBufferInfo().dwSize.X;
			}
			set
			{
				Console.SetBufferSize(value, Console.BufferHeight);
			}
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00018F58 File Offset: 0x00017F58
		public static void SetBufferSize(int width, int height)
		{
			new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
			Win32Native.SMALL_RECT srWindow = Console.GetBufferInfo().srWindow;
			if (width < (int)(srWindow.Right + 1) || width >= 32767)
			{
				throw new ArgumentOutOfRangeException("width", width, Environment.GetResourceString("ArgumentOutOfRange_ConsoleBufferLessThanWindowSize"));
			}
			if (height < (int)(srWindow.Bottom + 1) || height >= 32767)
			{
				throw new ArgumentOutOfRangeException("height", height, Environment.GetResourceString("ArgumentOutOfRange_ConsoleBufferLessThanWindowSize"));
			}
			Win32Native.COORD coord = default(Win32Native.COORD);
			coord.X = (short)width;
			coord.Y = (short)height;
			if (!Win32Native.SetConsoleScreenBufferSize(Console.ConsoleOutputHandle, coord))
			{
				__Error.WinIOError();
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x0001900C File Offset: 0x0001800C
		// (set) Token: 0x06000799 RID: 1945 RVA: 0x0001903A File Offset: 0x0001803A
		public static int WindowHeight
		{
			get
			{
				Win32Native.CONSOLE_SCREEN_BUFFER_INFO bufferInfo = Console.GetBufferInfo();
				return (int)(bufferInfo.srWindow.Bottom - bufferInfo.srWindow.Top + 1);
			}
			set
			{
				Console.SetWindowSize(Console.WindowWidth, value);
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x00019048 File Offset: 0x00018048
		// (set) Token: 0x0600079B RID: 1947 RVA: 0x00019076 File Offset: 0x00018076
		public static int WindowWidth
		{
			get
			{
				Win32Native.CONSOLE_SCREEN_BUFFER_INFO bufferInfo = Console.GetBufferInfo();
				return (int)(bufferInfo.srWindow.Right - bufferInfo.srWindow.Left + 1);
			}
			set
			{
				Console.SetWindowSize(value, Console.WindowHeight);
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00019084 File Offset: 0x00018084
		public unsafe static void SetWindowSize(int width, int height)
		{
			new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
			Win32Native.CONSOLE_SCREEN_BUFFER_INFO bufferInfo = Console.GetBufferInfo();
			if (width <= 0)
			{
				throw new ArgumentOutOfRangeException("width", width, Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			if (height <= 0)
			{
				throw new ArgumentOutOfRangeException("height", height, Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			bool flag = false;
			Win32Native.COORD coord = default(Win32Native.COORD);
			coord.X = bufferInfo.dwSize.X;
			coord.Y = bufferInfo.dwSize.Y;
			if ((int)bufferInfo.dwSize.X < (int)bufferInfo.srWindow.Left + width)
			{
				if ((int)bufferInfo.srWindow.Left >= 32767 - width)
				{
					throw new ArgumentOutOfRangeException("width", Environment.GetResourceString("ArgumentOutOfRange_ConsoleWindowBufferSize"));
				}
				coord.X = (short)((int)bufferInfo.srWindow.Left + width);
				flag = true;
			}
			if ((int)bufferInfo.dwSize.Y < (int)bufferInfo.srWindow.Top + height)
			{
				if ((int)bufferInfo.srWindow.Top >= 32767 - height)
				{
					throw new ArgumentOutOfRangeException("height", Environment.GetResourceString("ArgumentOutOfRange_ConsoleWindowBufferSize"));
				}
				coord.Y = (short)((int)bufferInfo.srWindow.Top + height);
				flag = true;
			}
			if (flag && !Win32Native.SetConsoleScreenBufferSize(Console.ConsoleOutputHandle, coord))
			{
				__Error.WinIOError();
			}
			Win32Native.SMALL_RECT srWindow = bufferInfo.srWindow;
			srWindow.Bottom = (short)((int)srWindow.Top + height - 1);
			srWindow.Right = (short)((int)srWindow.Left + width - 1);
			if (!Win32Native.SetConsoleWindowInfo(Console.ConsoleOutputHandle, true, &srWindow))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (flag)
				{
					Win32Native.SetConsoleScreenBufferSize(Console.ConsoleOutputHandle, bufferInfo.dwSize);
				}
				Win32Native.COORD largestConsoleWindowSize = Win32Native.GetLargestConsoleWindowSize(Console.ConsoleOutputHandle);
				if (width > (int)largestConsoleWindowSize.X)
				{
					throw new ArgumentOutOfRangeException("width", width, Environment.GetResourceString("ArgumentOutOfRange_ConsoleWindowSize_Size", new object[] { largestConsoleWindowSize.X }));
				}
				if (height > (int)largestConsoleWindowSize.Y)
				{
					throw new ArgumentOutOfRangeException("height", height, Environment.GetResourceString("ArgumentOutOfRange_ConsoleWindowSize_Size", new object[] { largestConsoleWindowSize.Y }));
				}
				__Error.WinIOError(lastWin32Error, string.Empty);
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x000192DC File Offset: 0x000182DC
		public static int LargestWindowWidth
		{
			get
			{
				return (int)Win32Native.GetLargestConsoleWindowSize(Console.ConsoleOutputHandle).X;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x000192FC File Offset: 0x000182FC
		public static int LargestWindowHeight
		{
			get
			{
				return (int)Win32Native.GetLargestConsoleWindowSize(Console.ConsoleOutputHandle).Y;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x0001931C File Offset: 0x0001831C
		// (set) Token: 0x060007A0 RID: 1952 RVA: 0x0001933B File Offset: 0x0001833B
		public static int WindowLeft
		{
			get
			{
				return (int)Console.GetBufferInfo().srWindow.Left;
			}
			set
			{
				Console.SetWindowPosition(value, Console.WindowTop);
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00019348 File Offset: 0x00018348
		// (set) Token: 0x060007A2 RID: 1954 RVA: 0x00019367 File Offset: 0x00018367
		public static int WindowTop
		{
			get
			{
				return (int)Console.GetBufferInfo().srWindow.Top;
			}
			set
			{
				Console.SetWindowPosition(Console.WindowLeft, value);
			}
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00019374 File Offset: 0x00018374
		public unsafe static void SetWindowPosition(int left, int top)
		{
			new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
			Win32Native.CONSOLE_SCREEN_BUFFER_INFO bufferInfo = Console.GetBufferInfo();
			Win32Native.SMALL_RECT srWindow = bufferInfo.srWindow;
			int num = left + (int)srWindow.Right - (int)srWindow.Left + 1;
			if (left < 0 || num > (int)bufferInfo.dwSize.X || num < 0)
			{
				throw new ArgumentOutOfRangeException("left", left, Environment.GetResourceString("ArgumentOutOfRange_ConsoleWindowPos"));
			}
			int num2 = top + (int)srWindow.Bottom - (int)srWindow.Top + 1;
			if (top < 0 || num2 > (int)bufferInfo.dwSize.Y || num2 < 0)
			{
				throw new ArgumentOutOfRangeException("top", top, Environment.GetResourceString("ArgumentOutOfRange_ConsoleWindowPos"));
			}
			srWindow.Bottom -= (short)((int)srWindow.Top - top);
			srWindow.Right -= (short)((int)srWindow.Left - left);
			srWindow.Left = (short)left;
			srWindow.Top = (short)top;
			if (!Win32Native.SetConsoleWindowInfo(Console.ConsoleOutputHandle, true, &srWindow))
			{
				__Error.WinIOError();
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x00019484 File Offset: 0x00018484
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x000194A3 File Offset: 0x000184A3
		public static int CursorLeft
		{
			get
			{
				return (int)Console.GetBufferInfo().dwCursorPosition.X;
			}
			set
			{
				Console.SetCursorPosition(value, Console.CursorTop);
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x000194B0 File Offset: 0x000184B0
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x000194CF File Offset: 0x000184CF
		public static int CursorTop
		{
			get
			{
				return (int)Console.GetBufferInfo().dwCursorPosition.Y;
			}
			set
			{
				Console.SetCursorPosition(Console.CursorLeft, value);
			}
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x000194DC File Offset: 0x000184DC
		public static void SetCursorPosition(int left, int top)
		{
			if (left < 0 || left >= 32767)
			{
				throw new ArgumentOutOfRangeException("left", left, Environment.GetResourceString("ArgumentOutOfRange_ConsoleBufferBoundaries"));
			}
			if (top < 0 || top >= 32767)
			{
				throw new ArgumentOutOfRangeException("top", top, Environment.GetResourceString("ArgumentOutOfRange_ConsoleBufferBoundaries"));
			}
			new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
			IntPtr consoleOutputHandle = Console.ConsoleOutputHandle;
			if (!Win32Native.SetConsoleCursorPosition(consoleOutputHandle, new Win32Native.COORD
			{
				X = (short)left,
				Y = (short)top
			}))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				Win32Native.CONSOLE_SCREEN_BUFFER_INFO bufferInfo = Console.GetBufferInfo();
				if (left < 0 || left >= (int)bufferInfo.dwSize.X)
				{
					throw new ArgumentOutOfRangeException("left", left, Environment.GetResourceString("ArgumentOutOfRange_ConsoleBufferBoundaries"));
				}
				if (top < 0 || top >= (int)bufferInfo.dwSize.Y)
				{
					throw new ArgumentOutOfRangeException("top", top, Environment.GetResourceString("ArgumentOutOfRange_ConsoleBufferBoundaries"));
				}
				__Error.WinIOError(lastWin32Error, string.Empty);
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x000195E4 File Offset: 0x000185E4
		// (set) Token: 0x060007AA RID: 1962 RVA: 0x00019610 File Offset: 0x00018610
		public static int CursorSize
		{
			get
			{
				IntPtr consoleOutputHandle = Console.ConsoleOutputHandle;
				Win32Native.CONSOLE_CURSOR_INFO console_CURSOR_INFO;
				if (!Win32Native.GetConsoleCursorInfo(consoleOutputHandle, out console_CURSOR_INFO))
				{
					__Error.WinIOError();
				}
				return console_CURSOR_INFO.dwSize;
			}
			set
			{
				if (value < 1 || value > 100)
				{
					throw new ArgumentOutOfRangeException("value", value, Environment.GetResourceString("ArgumentOutOfRange_CursorSize"));
				}
				new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
				if (value == 100 && (Environment.OSInfo & Environment.OSName.Win9x) != Environment.OSName.Invalid)
				{
					value = 99;
				}
				IntPtr consoleOutputHandle = Console.ConsoleOutputHandle;
				Win32Native.CONSOLE_CURSOR_INFO console_CURSOR_INFO;
				if (!Win32Native.GetConsoleCursorInfo(consoleOutputHandle, out console_CURSOR_INFO))
				{
					__Error.WinIOError();
				}
				console_CURSOR_INFO.dwSize = value;
				if (!Win32Native.SetConsoleCursorInfo(consoleOutputHandle, ref console_CURSOR_INFO))
				{
					__Error.WinIOError();
				}
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x00019690 File Offset: 0x00018690
		// (set) Token: 0x060007AC RID: 1964 RVA: 0x000196BC File Offset: 0x000186BC
		public static bool CursorVisible
		{
			get
			{
				IntPtr consoleOutputHandle = Console.ConsoleOutputHandle;
				Win32Native.CONSOLE_CURSOR_INFO console_CURSOR_INFO;
				if (!Win32Native.GetConsoleCursorInfo(consoleOutputHandle, out console_CURSOR_INFO))
				{
					__Error.WinIOError();
				}
				return console_CURSOR_INFO.bVisible;
			}
			set
			{
				new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
				IntPtr consoleOutputHandle = Console.ConsoleOutputHandle;
				Win32Native.CONSOLE_CURSOR_INFO console_CURSOR_INFO;
				if (!Win32Native.GetConsoleCursorInfo(consoleOutputHandle, out console_CURSOR_INFO))
				{
					__Error.WinIOError();
				}
				console_CURSOR_INFO.bVisible = value;
				if (!Win32Native.SetConsoleCursorInfo(consoleOutputHandle, ref console_CURSOR_INFO))
				{
					__Error.WinIOError();
				}
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x00019704 File Offset: 0x00018704
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x00019770 File Offset: 0x00018770
		public static string Title
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(24501);
				Win32Native.SetLastError(0);
				int consoleTitle = Win32Native.GetConsoleTitle(stringBuilder, stringBuilder.Capacity);
				if (consoleTitle == 0)
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error == 0)
					{
						stringBuilder.Length = 0;
					}
					else
					{
						__Error.WinIOError(lastWin32Error, string.Empty);
					}
				}
				else if (consoleTitle > 24500)
				{
					throw new InvalidOperationException(Environment.GetResourceString("ArgumentOutOfRange_ConsoleTitleTooLong"));
				}
				return stringBuilder.ToString();
			}
			set
			{
				new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length > 24500)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_ConsoleTitleTooLong"));
				}
				if (!Win32Native.SetConsoleTitle(value))
				{
					__Error.WinIOError();
				}
			}
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x000197C5 File Offset: 0x000187C5
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static ConsoleKeyInfo ReadKey()
		{
			return Console.ReadKey(false);
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x000197CD File Offset: 0x000187CD
		private static bool IsKeyDownEvent(Win32Native.InputRecord ir)
		{
			return ir.eventType == 1 && ir.keyEvent.keyDown;
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x000197E7 File Offset: 0x000187E7
		private static bool IsModKey(short keyCode)
		{
			return (keyCode >= 16 && keyCode <= 18) || keyCode == 20 || keyCode == 144 || keyCode == 145;
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0001980C File Offset: 0x0001880C
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static ConsoleKeyInfo ReadKey(bool intercept)
		{
			int num = -1;
			Win32Native.InputRecord cachedInputRecord;
			if (Console._cachedInputRecord.eventType == 1)
			{
				cachedInputRecord = Console._cachedInputRecord;
				if (Console._cachedInputRecord.keyEvent.repeatCount == 0)
				{
					Console._cachedInputRecord.eventType = -1;
				}
				else
				{
					Console._cachedInputRecord.keyEvent.repeatCount = Console._cachedInputRecord.keyEvent.repeatCount - 1;
				}
			}
			else
			{
				for (;;)
				{
					bool flag = Win32Native.ReadConsoleInput(Console.ConsoleInputHandle, out cachedInputRecord, 1, out num);
					if (!flag || num == 0)
					{
						break;
					}
					if (Console.IsKeyDownEvent(cachedInputRecord))
					{
						if (cachedInputRecord.keyEvent.uChar != '\0')
						{
							goto IL_00A5;
						}
						short virtualKeyCode = cachedInputRecord.keyEvent.virtualKeyCode;
						if (!Console.IsModKey(virtualKeyCode))
						{
							goto IL_00A5;
						}
					}
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ConsoleReadKeyOnFile"));
				IL_00A5:
				if (cachedInputRecord.keyEvent.repeatCount > 1)
				{
					cachedInputRecord.keyEvent.repeatCount = cachedInputRecord.keyEvent.repeatCount - 1;
					Console._cachedInputRecord = cachedInputRecord;
				}
			}
			Console.ControlKeyState controlKeyState = (Console.ControlKeyState)cachedInputRecord.keyEvent.controlKeyState;
			bool flag2 = (controlKeyState & Console.ControlKeyState.ShiftPressed) != (Console.ControlKeyState)0;
			bool flag3 = (controlKeyState & (Console.ControlKeyState.RightAltPressed | Console.ControlKeyState.LeftAltPressed)) != (Console.ControlKeyState)0;
			bool flag4 = (controlKeyState & (Console.ControlKeyState.RightCtrlPressed | Console.ControlKeyState.LeftCtrlPressed)) != (Console.ControlKeyState)0;
			ConsoleKeyInfo consoleKeyInfo = new ConsoleKeyInfo(cachedInputRecord.keyEvent.uChar, (ConsoleKey)cachedInputRecord.keyEvent.virtualKeyCode, flag2, flag3, flag4);
			if (!intercept)
			{
				Console.Write(cachedInputRecord.keyEvent.uChar);
			}
			return consoleKeyInfo;
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x00019958 File Offset: 0x00018958
		public static bool KeyAvailable
		{
			[HostProtection(SecurityAction.LinkDemand, UI = true)]
			get
			{
				if (Console._cachedInputRecord.eventType == 1)
				{
					return true;
				}
				Win32Native.InputRecord inputRecord = default(Win32Native.InputRecord);
				int num = 0;
				for (;;)
				{
					if (!Win32Native.PeekConsoleInput(Console.ConsoleInputHandle, out inputRecord, 1, out num))
					{
						int lastWin32Error = Marshal.GetLastWin32Error();
						if (lastWin32Error == 6)
						{
							break;
						}
						__Error.WinIOError(lastWin32Error, "stdin");
					}
					if (num == 0)
					{
						return false;
					}
					short virtualKeyCode = inputRecord.keyEvent.virtualKeyCode;
					if (Console.IsKeyDownEvent(inputRecord) && !Console.IsModKey(virtualKeyCode))
					{
						return true;
					}
					if (!Win32Native.ReadConsoleInput(Console.ConsoleInputHandle, out inputRecord, 1, out num))
					{
						__Error.WinIOError();
					}
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ConsoleKeyAvailableOnFile"));
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x000199F8 File Offset: 0x000189F8
		public static bool NumberLock
		{
			get
			{
				short keyState = Win32Native.GetKeyState(144);
				return (keyState & 1) == 1;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00019A18 File Offset: 0x00018A18
		public static bool CapsLock
		{
			get
			{
				short keyState = Win32Native.GetKeyState(20);
				return (keyState & 1) == 1;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x00019A34 File Offset: 0x00018A34
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x00019A80 File Offset: 0x00018A80
		public static bool TreatControlCAsInput
		{
			get
			{
				IntPtr consoleInputHandle = Console.ConsoleInputHandle;
				if (consoleInputHandle == Win32Native.INVALID_HANDLE_VALUE)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_NoConsole"));
				}
				int num = 0;
				if (!Win32Native.GetConsoleMode(consoleInputHandle, out num))
				{
					__Error.WinIOError();
				}
				return (num & 1) == 0;
			}
			set
			{
				new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
				IntPtr consoleInputHandle = Console.ConsoleInputHandle;
				if (consoleInputHandle == Win32Native.INVALID_HANDLE_VALUE)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_NoConsole"));
				}
				int num = 0;
				bool consoleMode = Win32Native.GetConsoleMode(consoleInputHandle, out num);
				if (value)
				{
					num &= -2;
				}
				else
				{
					num |= 1;
				}
				if (!Win32Native.SetConsoleMode(consoleInputHandle, num))
				{
					__Error.WinIOError();
				}
			}
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00019AE4 File Offset: 0x00018AE4
		private static bool BreakEvent(int controlType)
		{
			if (controlType != 0 && controlType != 1)
			{
				return false;
			}
			ConsoleCancelEventHandler cancelCallbacks = Console._cancelCallbacks;
			if (cancelCallbacks == null)
			{
				return false;
			}
			ConsoleSpecialKey consoleSpecialKey = ((controlType == 0) ? ConsoleSpecialKey.ControlC : ConsoleSpecialKey.ControlBreak);
			Console.ControlCDelegateData controlCDelegateData = new Console.ControlCDelegateData(consoleSpecialKey, cancelCallbacks);
			WaitCallback waitCallback = new WaitCallback(Console.ControlCDelegate);
			if (!ThreadPool.QueueUserWorkItem(waitCallback, controlCDelegateData))
			{
				return false;
			}
			TimeSpan timeSpan = new TimeSpan(0, 0, 30);
			controlCDelegateData.CompletionEvent.WaitOne(timeSpan, false);
			if (!controlCDelegateData.DelegateStarted)
			{
				return false;
			}
			controlCDelegateData.CompletionEvent.WaitOne();
			controlCDelegateData.CompletionEvent.Close();
			return controlCDelegateData.Cancel;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00019B70 File Offset: 0x00018B70
		private static void ControlCDelegate(object data)
		{
			Console.ControlCDelegateData controlCDelegateData = (Console.ControlCDelegateData)data;
			try
			{
				controlCDelegateData.DelegateStarted = true;
				ConsoleCancelEventArgs consoleCancelEventArgs = new ConsoleCancelEventArgs(controlCDelegateData.ControlKey);
				controlCDelegateData.CancelCallbacks(null, consoleCancelEventArgs);
				controlCDelegateData.Cancel = consoleCancelEventArgs.Cancel;
			}
			finally
			{
				controlCDelegateData.CompletionEvent.Set();
			}
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060007BA RID: 1978 RVA: 0x00019BD0 File Offset: 0x00018BD0
		// (remove) Token: 0x060007BB RID: 1979 RVA: 0x00019C40 File Offset: 0x00018C40
		public static event ConsoleCancelEventHandler CancelKeyPress
		{
			add
			{
				new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
				lock (Console.InternalSyncObject)
				{
					Console._cancelCallbacks = (ConsoleCancelEventHandler)Delegate.Combine(Console._cancelCallbacks, value);
					if (Console._hooker == null)
					{
						Console._hooker = new Console.ControlCHooker();
						Console._hooker.Hook();
					}
				}
			}
			remove
			{
				new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
				lock (Console.InternalSyncObject)
				{
					Console._cancelCallbacks = (ConsoleCancelEventHandler)Delegate.Remove(Console._cancelCallbacks, value);
					if (Console._hooker != null && Console._cancelCallbacks == null)
					{
						Console._hooker.Unhook();
					}
				}
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x00019CAC File Offset: 0x00018CAC
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static Stream OpenStandardError()
		{
			return Console.OpenStandardError(256);
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x00019CB8 File Offset: 0x00018CB8
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static Stream OpenStandardError(int bufferSize)
		{
			if (bufferSize < 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			return Console.GetStandardFile(-12, FileAccess.Write, bufferSize);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00019CDC File Offset: 0x00018CDC
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static Stream OpenStandardInput()
		{
			return Console.OpenStandardInput(256);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00019CE8 File Offset: 0x00018CE8
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static Stream OpenStandardInput(int bufferSize)
		{
			if (bufferSize < 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			return Console.GetStandardFile(-10, FileAccess.Read, bufferSize);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x00019D0C File Offset: 0x00018D0C
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static Stream OpenStandardOutput()
		{
			return Console.OpenStandardOutput(256);
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00019D18 File Offset: 0x00018D18
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static Stream OpenStandardOutput(int bufferSize)
		{
			if (bufferSize < 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			return Console.GetStandardFile(-11, FileAccess.Write, bufferSize);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00019D3C File Offset: 0x00018D3C
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void SetIn(TextReader newIn)
		{
			if (newIn == null)
			{
				throw new ArgumentNullException("newIn");
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			newIn = TextReader.Synchronized(newIn);
			lock (Console.InternalSyncObject)
			{
				Console._in = newIn;
			}
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00019D98 File Offset: 0x00018D98
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void SetOut(TextWriter newOut)
		{
			if (newOut == null)
			{
				throw new ArgumentNullException("newOut");
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			Console._wasOutRedirected = true;
			newOut = TextWriter.Synchronized(newOut);
			lock (Console.InternalSyncObject)
			{
				Console._out = newOut;
			}
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x00019DF8 File Offset: 0x00018DF8
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void SetError(TextWriter newError)
		{
			if (newError == null)
			{
				throw new ArgumentNullException("newError");
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			Console._wasErrorRedirected = true;
			newError = TextWriter.Synchronized(newError);
			lock (Console.InternalSyncObject)
			{
				Console._error = newError;
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00019E58 File Offset: 0x00018E58
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static int Read()
		{
			return Console.In.Read();
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00019E64 File Offset: 0x00018E64
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static string ReadLine()
		{
			return Console.In.ReadLine();
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00019E70 File Offset: 0x00018E70
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine()
		{
			Console.Out.WriteLine();
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00019E7C File Offset: 0x00018E7C
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(bool value)
		{
			Console.Out.WriteLine(value);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00019E89 File Offset: 0x00018E89
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(char value)
		{
			Console.Out.WriteLine(value);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00019E96 File Offset: 0x00018E96
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(char[] buffer)
		{
			Console.Out.WriteLine(buffer);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00019EA3 File Offset: 0x00018EA3
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(char[] buffer, int index, int count)
		{
			Console.Out.WriteLine(buffer, index, count);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00019EB2 File Offset: 0x00018EB2
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(decimal value)
		{
			Console.Out.WriteLine(value);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00019EBF File Offset: 0x00018EBF
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(double value)
		{
			Console.Out.WriteLine(value);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00019ECC File Offset: 0x00018ECC
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(float value)
		{
			Console.Out.WriteLine(value);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00019ED9 File Offset: 0x00018ED9
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(int value)
		{
			Console.Out.WriteLine(value);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00019EE6 File Offset: 0x00018EE6
		[CLSCompliant(false)]
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(uint value)
		{
			Console.Out.WriteLine(value);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00019EF3 File Offset: 0x00018EF3
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(long value)
		{
			Console.Out.WriteLine(value);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x00019F00 File Offset: 0x00018F00
		[CLSCompliant(false)]
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(ulong value)
		{
			Console.Out.WriteLine(value);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00019F0D File Offset: 0x00018F0D
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(object value)
		{
			Console.Out.WriteLine(value);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00019F1A File Offset: 0x00018F1A
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(string value)
		{
			Console.Out.WriteLine(value);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00019F27 File Offset: 0x00018F27
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(string format, object arg0)
		{
			Console.Out.WriteLine(format, arg0);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00019F35 File Offset: 0x00018F35
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(string format, object arg0, object arg1)
		{
			Console.Out.WriteLine(format, arg0, arg1);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00019F44 File Offset: 0x00018F44
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(string format, object arg0, object arg1, object arg2)
		{
			Console.Out.WriteLine(format, arg0, arg1, arg2);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00019F54 File Offset: 0x00018F54
		[CLSCompliant(false)]
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(string format, object arg0, object arg1, object arg2, object arg3, __arglist)
		{
			ArgIterator argIterator = new ArgIterator(__arglist);
			int num = argIterator.GetRemainingCount() + 4;
			object[] array = new object[num];
			array[0] = arg0;
			array[1] = arg1;
			array[2] = arg2;
			array[3] = arg3;
			for (int i = 4; i < num; i++)
			{
				array[i] = TypedReference.ToObject(argIterator.GetNextArg());
			}
			Console.Out.WriteLine(format, array);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00019FB3 File Offset: 0x00018FB3
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void WriteLine(string format, params object[] arg)
		{
			Console.Out.WriteLine(format, arg);
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00019FC1 File Offset: 0x00018FC1
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(string format, object arg0)
		{
			Console.Out.Write(format, arg0);
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00019FCF File Offset: 0x00018FCF
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(string format, object arg0, object arg1)
		{
			Console.Out.Write(format, arg0, arg1);
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00019FDE File Offset: 0x00018FDE
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(string format, object arg0, object arg1, object arg2)
		{
			Console.Out.Write(format, arg0, arg1, arg2);
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00019FF0 File Offset: 0x00018FF0
		[CLSCompliant(false)]
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(string format, object arg0, object arg1, object arg2, object arg3, __arglist)
		{
			ArgIterator argIterator = new ArgIterator(__arglist);
			int num = argIterator.GetRemainingCount() + 4;
			object[] array = new object[num];
			array[0] = arg0;
			array[1] = arg1;
			array[2] = arg2;
			array[3] = arg3;
			for (int i = 4; i < num; i++)
			{
				array[i] = TypedReference.ToObject(argIterator.GetNextArg());
			}
			Console.Out.Write(format, array);
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0001A04F File Offset: 0x0001904F
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(string format, params object[] arg)
		{
			Console.Out.Write(format, arg);
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0001A05D File Offset: 0x0001905D
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(bool value)
		{
			Console.Out.Write(value);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001A06A File Offset: 0x0001906A
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(char value)
		{
			Console.Out.Write(value);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0001A077 File Offset: 0x00019077
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(char[] buffer)
		{
			Console.Out.Write(buffer);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0001A084 File Offset: 0x00019084
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(char[] buffer, int index, int count)
		{
			Console.Out.Write(buffer, index, count);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0001A093 File Offset: 0x00019093
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(double value)
		{
			Console.Out.Write(value);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0001A0A0 File Offset: 0x000190A0
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(decimal value)
		{
			Console.Out.Write(value);
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0001A0AD File Offset: 0x000190AD
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(float value)
		{
			Console.Out.Write(value);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0001A0BA File Offset: 0x000190BA
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(int value)
		{
			Console.Out.Write(value);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0001A0C7 File Offset: 0x000190C7
		[CLSCompliant(false)]
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(uint value)
		{
			Console.Out.Write(value);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0001A0D4 File Offset: 0x000190D4
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(long value)
		{
			Console.Out.Write(value);
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0001A0E1 File Offset: 0x000190E1
		[CLSCompliant(false)]
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(ulong value)
		{
			Console.Out.Write(value);
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0001A0EE File Offset: 0x000190EE
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(object value)
		{
			Console.Out.Write(value);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001A0FB File Offset: 0x000190FB
		[HostProtection(SecurityAction.LinkDemand, UI = true)]
		public static void Write(string value)
		{
			Console.Out.Write(value);
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0001A108 File Offset: 0x00019108
		private unsafe static bool ConsoleHandleIsValid(SafeFileHandle handle)
		{
			if (handle.IsInvalid)
			{
				return false;
			}
			byte b = 65;
			int num2;
			int num = __ConsoleStream.WriteFile(handle, &b, 0, out num2, IntPtr.Zero);
			return num != 0;
		}

		// Token: 0x04000291 RID: 657
		private const int _DefaultConsoleBufferSize = 256;

		// Token: 0x04000292 RID: 658
		private const int NumberLockVKCode = 144;

		// Token: 0x04000293 RID: 659
		private const int CapsLockVKCode = 20;

		// Token: 0x04000294 RID: 660
		private const int MinBeepFrequency = 37;

		// Token: 0x04000295 RID: 661
		private const int MaxBeepFrequency = 32767;

		// Token: 0x04000296 RID: 662
		private const int MaxConsoleTitleLength = 24500;

		// Token: 0x04000297 RID: 663
		private static TextReader _in;

		// Token: 0x04000298 RID: 664
		private static TextWriter _out;

		// Token: 0x04000299 RID: 665
		private static TextWriter _error;

		// Token: 0x0400029A RID: 666
		private static ConsoleCancelEventHandler _cancelCallbacks;

		// Token: 0x0400029B RID: 667
		private static Console.ControlCHooker _hooker;

		// Token: 0x0400029C RID: 668
		private static Win32Native.InputRecord _cachedInputRecord;

		// Token: 0x0400029D RID: 669
		private static bool _haveReadDefaultColors;

		// Token: 0x0400029E RID: 670
		private static byte _defaultColors;

		// Token: 0x0400029F RID: 671
		private static bool _wasOutRedirected;

		// Token: 0x040002A0 RID: 672
		private static bool _wasErrorRedirected;

		// Token: 0x040002A1 RID: 673
		private static object s_InternalSyncObject;

		// Token: 0x040002A2 RID: 674
		private static IntPtr _consoleInputHandle;

		// Token: 0x040002A3 RID: 675
		private static IntPtr _consoleOutputHandle;

		// Token: 0x0200008B RID: 139
		[Flags]
		internal enum ControlKeyState
		{
			// Token: 0x040002A5 RID: 677
			RightAltPressed = 1,
			// Token: 0x040002A6 RID: 678
			LeftAltPressed = 2,
			// Token: 0x040002A7 RID: 679
			RightCtrlPressed = 4,
			// Token: 0x040002A8 RID: 680
			LeftCtrlPressed = 8,
			// Token: 0x040002A9 RID: 681
			ShiftPressed = 16,
			// Token: 0x040002AA RID: 682
			NumLockOn = 32,
			// Token: 0x040002AB RID: 683
			ScrollLockOn = 64,
			// Token: 0x040002AC RID: 684
			CapsLockOn = 128,
			// Token: 0x040002AD RID: 685
			EnhancedKey = 256
		}

		// Token: 0x0200008D RID: 141
		internal sealed class ControlCHooker : CriticalFinalizerObject
		{
			// Token: 0x060007EF RID: 2031 RVA: 0x0001A16C File Offset: 0x0001916C
			internal ControlCHooker()
			{
				this._handler = new Win32Native.ConsoleCtrlHandlerRoutine(Console.BreakEvent);
			}

			// Token: 0x060007F0 RID: 2032 RVA: 0x0001A188 File Offset: 0x00019188
			~ControlCHooker()
			{
				this.Unhook();
			}

			// Token: 0x060007F1 RID: 2033 RVA: 0x0001A1B4 File Offset: 0x000191B4
			internal void Hook()
			{
				if (!this._hooked)
				{
					if (!Win32Native.SetConsoleCtrlHandler(this._handler, true))
					{
						__Error.WinIOError();
					}
					this._hooked = true;
				}
			}

			// Token: 0x060007F2 RID: 2034 RVA: 0x0001A1E8 File Offset: 0x000191E8
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			internal void Unhook()
			{
				if (this._hooked)
				{
					if (!Win32Native.SetConsoleCtrlHandler(this._handler, false))
					{
						__Error.WinIOError();
					}
					this._hooked = false;
				}
			}

			// Token: 0x040002AE RID: 686
			private bool _hooked;

			// Token: 0x040002AF RID: 687
			private Win32Native.ConsoleCtrlHandlerRoutine _handler;
		}

		// Token: 0x0200008E RID: 142
		private sealed class ControlCDelegateData
		{
			// Token: 0x060007F3 RID: 2035 RVA: 0x0001A219 File Offset: 0x00019219
			internal ControlCDelegateData(ConsoleSpecialKey controlKey, ConsoleCancelEventHandler cancelCallbacks)
			{
				this.ControlKey = controlKey;
				this.CancelCallbacks = cancelCallbacks;
				this.CompletionEvent = new ManualResetEvent(false);
			}

			// Token: 0x040002B0 RID: 688
			internal ConsoleSpecialKey ControlKey;

			// Token: 0x040002B1 RID: 689
			internal bool Cancel;

			// Token: 0x040002B2 RID: 690
			internal bool DelegateStarted;

			// Token: 0x040002B3 RID: 691
			internal ManualResetEvent CompletionEvent;

			// Token: 0x040002B4 RID: 692
			internal ConsoleCancelEventHandler CancelCallbacks;
		}
	}
}
