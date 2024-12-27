using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;

namespace System.Diagnostics
{
	// Token: 0x020002AC RID: 684
	internal static class Log
	{
		// Token: 0x06001AF9 RID: 6905 RVA: 0x00046D8C File Offset: 0x00045D8C
		static Log()
		{
			Log.GlobalSwitch.MinimumLevel = LoggingLevels.ErrorLevel;
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x00046DE8 File Offset: 0x00045DE8
		public static void AddOnLogMessage(LogMessageEventHandler handler)
		{
			lock (Log.locker)
			{
				Log._LogMessageEventHandler = (LogMessageEventHandler)Delegate.Combine(Log._LogMessageEventHandler, handler);
			}
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x00046E30 File Offset: 0x00045E30
		public static void RemoveOnLogMessage(LogMessageEventHandler handler)
		{
			lock (Log.locker)
			{
				Log._LogMessageEventHandler = (LogMessageEventHandler)Delegate.Remove(Log._LogMessageEventHandler, handler);
			}
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x00046E78 File Offset: 0x00045E78
		public static void AddOnLogSwitchLevel(LogSwitchLevelHandler handler)
		{
			lock (Log.locker)
			{
				Log._LogSwitchLevelHandler = (LogSwitchLevelHandler)Delegate.Combine(Log._LogSwitchLevelHandler, handler);
			}
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x00046EC0 File Offset: 0x00045EC0
		public static void RemoveOnLogSwitchLevel(LogSwitchLevelHandler handler)
		{
			lock (Log.locker)
			{
				Log._LogSwitchLevelHandler = (LogSwitchLevelHandler)Delegate.Remove(Log._LogSwitchLevelHandler, handler);
			}
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x00046F08 File Offset: 0x00045F08
		internal static void InvokeLogSwitchLevelHandlers(LogSwitch ls, LoggingLevels newLevel)
		{
			LogSwitchLevelHandler logSwitchLevelHandler = Log._LogSwitchLevelHandler;
			if (logSwitchLevelHandler != null)
			{
				logSwitchLevelHandler(ls, newLevel);
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001AFF RID: 6911 RVA: 0x00046F26 File Offset: 0x00045F26
		// (set) Token: 0x06001B00 RID: 6912 RVA: 0x00046F2D File Offset: 0x00045F2D
		public static bool IsConsoleEnabled
		{
			get
			{
				return Log.m_fConsoleDeviceEnabled;
			}
			set
			{
				Log.m_fConsoleDeviceEnabled = value;
			}
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x00046F38 File Offset: 0x00045F38
		public static void AddStream(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (Log.m_iStreamArraySize <= Log.m_iNumOfStreamDevices)
			{
				Stream[] array = new Stream[Log.m_iStreamArraySize + 4];
				if (Log.m_iNumOfStreamDevices > 0)
				{
					Array.Copy(Log.m_rgStream, array, Log.m_iNumOfStreamDevices);
				}
				Log.m_iStreamArraySize += 4;
				Log.m_rgStream = array;
			}
			Log.m_rgStream[Log.m_iNumOfStreamDevices++] = stream;
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x00046FAA File Offset: 0x00045FAA
		public static void LogMessage(LoggingLevels level, string message)
		{
			Log.LogMessage(level, Log.GlobalSwitch, message);
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x00046FB8 File Offset: 0x00045FB8
		public static void LogMessage(LoggingLevels level, LogSwitch logswitch, string message)
		{
			if (logswitch == null)
			{
				throw new ArgumentNullException("LogSwitch");
			}
			if (level < LoggingLevels.TraceLevel0)
			{
				throw new ArgumentOutOfRangeException("level", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (logswitch.CheckLevel(level))
			{
				Debugger.Log((int)level, logswitch.strName, message);
				if (Log.m_fConsoleDeviceEnabled)
				{
					Console.Write(message);
				}
				for (int i = 0; i < Log.m_iNumOfStreamDevices; i++)
				{
					StreamWriter streamWriter = new StreamWriter(Log.m_rgStream[i]);
					streamWriter.Write(message);
					streamWriter.Flush();
				}
			}
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x00047039 File Offset: 0x00046039
		public static void Trace(LogSwitch logswitch, string message)
		{
			Log.LogMessage(LoggingLevels.TraceLevel0, logswitch, message);
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x00047044 File Offset: 0x00046044
		public static void Trace(string switchname, string message)
		{
			LogSwitch @switch = LogSwitch.GetSwitch(switchname);
			Log.LogMessage(LoggingLevels.TraceLevel0, @switch, message);
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x00047060 File Offset: 0x00046060
		public static void Trace(string message)
		{
			Log.LogMessage(LoggingLevels.TraceLevel0, Log.GlobalSwitch, message);
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x0004706E File Offset: 0x0004606E
		public static void Status(LogSwitch logswitch, string message)
		{
			Log.LogMessage(LoggingLevels.StatusLevel0, logswitch, message);
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x0004707C File Offset: 0x0004607C
		public static void Status(string switchname, string message)
		{
			LogSwitch @switch = LogSwitch.GetSwitch(switchname);
			Log.LogMessage(LoggingLevels.StatusLevel0, @switch, message);
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x00047099 File Offset: 0x00046099
		public static void Status(string message)
		{
			Log.LogMessage(LoggingLevels.StatusLevel0, Log.GlobalSwitch, message);
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x000470A8 File Offset: 0x000460A8
		public static void Warning(LogSwitch logswitch, string message)
		{
			Log.LogMessage(LoggingLevels.WarningLevel, logswitch, message);
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x000470B4 File Offset: 0x000460B4
		public static void Warning(string switchname, string message)
		{
			LogSwitch @switch = LogSwitch.GetSwitch(switchname);
			Log.LogMessage(LoggingLevels.WarningLevel, @switch, message);
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x000470D1 File Offset: 0x000460D1
		public static void Warning(string message)
		{
			Log.LogMessage(LoggingLevels.WarningLevel, Log.GlobalSwitch, message);
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x000470E0 File Offset: 0x000460E0
		public static void Error(LogSwitch logswitch, string message)
		{
			Log.LogMessage(LoggingLevels.ErrorLevel, logswitch, message);
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x000470EC File Offset: 0x000460EC
		public static void Error(string switchname, string message)
		{
			LogSwitch @switch = LogSwitch.GetSwitch(switchname);
			Log.LogMessage(LoggingLevels.ErrorLevel, @switch, message);
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x00047109 File Offset: 0x00046109
		public static void Error(string message)
		{
			Log.LogMessage(LoggingLevels.ErrorLevel, Log.GlobalSwitch, message);
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x00047118 File Offset: 0x00046118
		public static void Panic(string message)
		{
			Log.LogMessage(LoggingLevels.PanicLevel, Log.GlobalSwitch, message);
		}

		// Token: 0x06001B11 RID: 6929
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddLogSwitch(LogSwitch logSwitch);

		// Token: 0x06001B12 RID: 6930
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ModifyLogSwitch(int iNewLevel, string strSwitchName, string strParentName);

		// Token: 0x04000A1B RID: 2587
		internal static Hashtable m_Hashtable = new Hashtable();

		// Token: 0x04000A1C RID: 2588
		private static bool m_fConsoleDeviceEnabled = false;

		// Token: 0x04000A1D RID: 2589
		private static Stream[] m_rgStream = null;

		// Token: 0x04000A1E RID: 2590
		private static int m_iNumOfStreamDevices = 0;

		// Token: 0x04000A1F RID: 2591
		private static int m_iStreamArraySize = 0;

		// Token: 0x04000A20 RID: 2592
		internal static int iNumOfSwitches;

		// Token: 0x04000A21 RID: 2593
		private static LogMessageEventHandler _LogMessageEventHandler;

		// Token: 0x04000A22 RID: 2594
		private static LogSwitchLevelHandler _LogSwitchLevelHandler;

		// Token: 0x04000A23 RID: 2595
		private static object locker = new object();

		// Token: 0x04000A24 RID: 2596
		public static readonly LogSwitch GlobalSwitch = new LogSwitch("Global", "Global Switch for this log");
	}
}
