using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000426 RID: 1062
	internal class Logging
	{
		// Token: 0x0600211E RID: 8478 RVA: 0x000829B1 File Offset: 0x000819B1
		private Logging()
		{
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x0600211F RID: 8479 RVA: 0x000829BC File Offset: 0x000819BC
		private static object InternalSyncObject
		{
			get
			{
				if (Logging.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref Logging.s_InternalSyncObject, obj, null);
				}
				return Logging.s_InternalSyncObject;
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06002120 RID: 8480 RVA: 0x000829E8 File Offset: 0x000819E8
		internal static bool On
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				return Logging.s_LoggingEnabled;
			}
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x000829FB File Offset: 0x000819FB
		internal static bool IsVerbose(TraceSource traceSource)
		{
			return Logging.ValidateSettings(traceSource, TraceEventType.Verbose);
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06002122 RID: 8482 RVA: 0x00082A05 File Offset: 0x00081A05
		internal static TraceSource Web
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				if (!Logging.s_LoggingEnabled)
				{
					return null;
				}
				return Logging.s_WebTraceSource;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06002123 RID: 8483 RVA: 0x00082A21 File Offset: 0x00081A21
		internal static TraceSource HttpListener
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				if (!Logging.s_LoggingEnabled)
				{
					return null;
				}
				return Logging.s_HttpListenerTraceSource;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06002124 RID: 8484 RVA: 0x00082A3D File Offset: 0x00081A3D
		internal static TraceSource Sockets
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				if (!Logging.s_LoggingEnabled)
				{
					return null;
				}
				return Logging.s_SocketsTraceSource;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06002125 RID: 8485 RVA: 0x00082A59 File Offset: 0x00081A59
		internal static TraceSource RequestCache
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				if (!Logging.s_LoggingEnabled)
				{
					return null;
				}
				return Logging.s_CacheTraceSource;
			}
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x00082A78 File Offset: 0x00081A78
		private static bool GetUseProtocolTextSetting(TraceSource traceSource)
		{
			bool flag = false;
			if (traceSource.Attributes["tracemode"] == "protocolonly")
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x00082AA8 File Offset: 0x00081AA8
		private static int GetMaxDumpSizeSetting(TraceSource traceSource)
		{
			int num = 1024;
			if (traceSource.Attributes.ContainsKey("maxdatasize"))
			{
				try
				{
					num = int.Parse(traceSource.Attributes["maxdatasize"], NumberFormatInfo.InvariantInfo);
				}
				catch (Exception ex)
				{
					if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
					{
						throw;
					}
					traceSource.Attributes["maxdatasize"] = num.ToString(NumberFormatInfo.InvariantInfo);
				}
			}
			return num;
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x00082B34 File Offset: 0x00081B34
		private static void InitializeLogging()
		{
			lock (Logging.InternalSyncObject)
			{
				if (!Logging.s_LoggingInitialized)
				{
					bool flag = false;
					Logging.s_WebTraceSource = new Logging.NclTraceSource("System.Net");
					Logging.s_HttpListenerTraceSource = new Logging.NclTraceSource("System.Net.HttpListener");
					Logging.s_SocketsTraceSource = new Logging.NclTraceSource("System.Net.Sockets");
					Logging.s_CacheTraceSource = new Logging.NclTraceSource("System.Net.Cache");
					if (Logging.s_WebTraceSource.Switch.ShouldTrace(TraceEventType.Critical) || Logging.s_HttpListenerTraceSource.Switch.ShouldTrace(TraceEventType.Critical) || Logging.s_SocketsTraceSource.Switch.ShouldTrace(TraceEventType.Critical) || Logging.s_CacheTraceSource.Switch.ShouldTrace(TraceEventType.Critical))
					{
						flag = true;
						AppDomain currentDomain = AppDomain.CurrentDomain;
						currentDomain.UnhandledException += Logging.UnhandledExceptionHandler;
						currentDomain.DomainUnload += Logging.AppDomainUnloadEvent;
						currentDomain.ProcessExit += Logging.ProcessExitEvent;
					}
					Logging.s_LoggingEnabled = flag;
					Logging.s_LoggingInitialized = true;
				}
			}
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x00082C40 File Offset: 0x00081C40
		private static void Close()
		{
			if (Logging.s_WebTraceSource != null)
			{
				Logging.s_WebTraceSource.Close();
			}
			if (Logging.s_HttpListenerTraceSource != null)
			{
				Logging.s_HttpListenerTraceSource.Close();
			}
			if (Logging.s_SocketsTraceSource != null)
			{
				Logging.s_SocketsTraceSource.Close();
			}
			if (Logging.s_CacheTraceSource != null)
			{
				Logging.s_CacheTraceSource.Close();
			}
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x00082C94 File Offset: 0x00081C94
		private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception ex = (Exception)args.ExceptionObject;
			Logging.Exception(Logging.Web, sender, "UnhandledExceptionHandler", ex);
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x00082CBE File Offset: 0x00081CBE
		private static void ProcessExitEvent(object sender, EventArgs e)
		{
			Logging.Close();
			Logging.s_AppDomainShutdown = true;
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x00082CCB File Offset: 0x00081CCB
		private static void AppDomainUnloadEvent(object sender, EventArgs e)
		{
			Logging.Close();
			Logging.s_AppDomainShutdown = true;
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x00082CD8 File Offset: 0x00081CD8
		private static bool ValidateSettings(TraceSource traceSource, TraceEventType traceLevel)
		{
			if (!Logging.s_LoggingEnabled)
			{
				return false;
			}
			if (!Logging.s_LoggingInitialized)
			{
				Logging.InitializeLogging();
			}
			return traceSource != null && traceSource.Switch.ShouldTrace(traceLevel) && !Logging.s_AppDomainShutdown;
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x00082D0C File Offset: 0x00081D0C
		private static string GetObjectName(object obj)
		{
			string text = obj.ToString();
			string text2;
			try
			{
				if (!(obj is Uri))
				{
					int num = text.LastIndexOf('.') + 1;
					text2 = text.Substring(num, text.Length - num);
				}
				else
				{
					text2 = text;
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				text2 = text;
			}
			return text2;
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x00082D7C File Offset: 0x00081D7C
		internal static uint GetThreadId()
		{
			uint num = UnsafeNclNativeMethods.GetCurrentThreadId();
			if (num == 0U)
			{
				num = (uint)Thread.CurrentThread.GetHashCode();
			}
			return num;
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x00082DA0 File Offset: 0x00081DA0
		internal static void PrintLine(TraceSource traceSource, TraceEventType eventType, int id, string msg)
		{
			string text = "[" + Logging.GetThreadId().ToString("d4", CultureInfo.InvariantCulture) + "] ";
			traceSource.TraceEvent(eventType, id, text + msg);
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x00082DE4 File Offset: 0x00081DE4
		internal static void Associate(TraceSource traceSource, object objA, object objB)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			string text = Logging.GetObjectName(objA) + "#" + ValidationHelper.HashString(objA);
			string text2 = Logging.GetObjectName(objB) + "#" + ValidationHelper.HashString(objB);
			Logging.PrintLine(traceSource, TraceEventType.Information, 0, "Associating " + text + " with " + text2);
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x00082E42 File Offset: 0x00081E42
		internal static void Enter(TraceSource traceSource, object obj, string method, string param)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Enter(traceSource, Logging.GetObjectName(obj) + "#" + ValidationHelper.HashString(obj), method, param);
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x00082E6C File Offset: 0x00081E6C
		internal static void Enter(TraceSource traceSource, object obj, string method, object paramObject)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Enter(traceSource, Logging.GetObjectName(obj) + "#" + ValidationHelper.HashString(obj), method, paramObject);
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x00082E98 File Offset: 0x00081E98
		internal static void Enter(TraceSource traceSource, string obj, string method, string param)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Enter(traceSource, string.Concat(new string[] { obj, "::", method, "(", param, ")" }));
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x00082EE8 File Offset: 0x00081EE8
		internal static void Enter(TraceSource traceSource, string obj, string method, object paramObject)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			string text = "";
			if (paramObject != null)
			{
				text = Logging.GetObjectName(paramObject) + "#" + ValidationHelper.HashString(paramObject);
			}
			Logging.Enter(traceSource, string.Concat(new string[] { obj, "::", method, "(", text, ")" }));
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x00082F56 File Offset: 0x00081F56
		internal static void Enter(TraceSource traceSource, string method, string parameters)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Enter(traceSource, method + "(" + parameters + ")");
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x00082F79 File Offset: 0x00081F79
		internal static void Enter(TraceSource traceSource, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, msg);
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x00082F90 File Offset: 0x00081F90
		internal static void Exit(TraceSource traceSource, object obj, string method, object retObject)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			string text = "";
			if (retObject != null)
			{
				text = Logging.GetObjectName(retObject) + "#" + ValidationHelper.HashString(retObject);
			}
			Logging.Exit(traceSource, obj, method, text);
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x00082FD0 File Offset: 0x00081FD0
		internal static void Exit(TraceSource traceSource, string obj, string method, object retObject)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			string text = "";
			if (retObject != null)
			{
				text = Logging.GetObjectName(retObject) + "#" + ValidationHelper.HashString(retObject);
			}
			Logging.Exit(traceSource, obj, method, text);
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x00083010 File Offset: 0x00082010
		internal static void Exit(TraceSource traceSource, object obj, string method, string retValue)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Exit(traceSource, Logging.GetObjectName(obj) + "#" + ValidationHelper.HashString(obj), method, retValue);
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x0008303C File Offset: 0x0008203C
		internal static void Exit(TraceSource traceSource, string obj, string method, string retValue)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			if (!ValidationHelper.IsBlankString(retValue))
			{
				retValue = "\t-> " + retValue;
			}
			Logging.Exit(traceSource, string.Concat(new string[] { obj, "::", method, "() ", retValue }));
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x00083097 File Offset: 0x00082097
		internal static void Exit(TraceSource traceSource, string method, string parameters)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Exit(traceSource, method + "() " + parameters);
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x000830B5 File Offset: 0x000820B5
		internal static void Exit(TraceSource traceSource, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, "Exiting " + msg);
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x000830D8 File Offset: 0x000820D8
		internal static void Exception(TraceSource traceSource, object obj, string method, Exception e)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Error))
			{
				return;
			}
			string text = string.Concat(new string[]
			{
				"Exception in the ",
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				"::",
				method,
				" - "
			});
			Logging.PrintLine(traceSource, TraceEventType.Error, 0, text + e.Message);
			if (!ValidationHelper.IsBlankString(e.StackTrace))
			{
				Logging.PrintLine(traceSource, TraceEventType.Error, 0, e.StackTrace);
			}
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x00083162 File Offset: 0x00082162
		internal static void PrintInfo(TraceSource traceSource, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Information, 0, msg);
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x00083178 File Offset: 0x00082178
		internal static void PrintInfo(TraceSource traceSource, object obj, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Information, 0, string.Concat(new string[]
			{
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				" - ",
				msg
			}));
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x000831CC File Offset: 0x000821CC
		internal static void PrintInfo(TraceSource traceSource, object obj, string method, string param)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Information, 0, string.Concat(new string[]
			{
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				"::",
				method,
				"(",
				param,
				")"
			}));
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x00083232 File Offset: 0x00082232
		internal static void PrintWarning(TraceSource traceSource, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Warning))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Warning, 0, msg);
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x00083248 File Offset: 0x00082248
		internal static void PrintWarning(TraceSource traceSource, object obj, string method, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Warning))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Warning, 0, string.Concat(new string[]
			{
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				"::",
				method,
				"() - ",
				msg
			}));
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x000832A6 File Offset: 0x000822A6
		internal static void PrintError(TraceSource traceSource, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Error))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Error, 0, msg);
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x000832BC File Offset: 0x000822BC
		internal static void PrintError(TraceSource traceSource, object obj, string method, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Error))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Error, 0, string.Concat(new string[]
			{
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				"::",
				method,
				"() - ",
				msg
			}));
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x0008331C File Offset: 0x0008231C
		internal static void Dump(TraceSource traceSource, object obj, string method, IntPtr bufferPtr, int length)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Verbose) || bufferPtr == IntPtr.Zero || length < 0)
			{
				return;
			}
			byte[] array = new byte[length];
			Marshal.Copy(bufferPtr, array, 0, length);
			Logging.Dump(traceSource, obj, method, array, 0, length);
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x00083364 File Offset: 0x00082364
		internal static void Dump(TraceSource traceSource, object obj, string method, byte[] buffer, int offset, int length)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Verbose))
			{
				return;
			}
			if (buffer == null)
			{
				Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, "(null)");
				return;
			}
			if (offset > buffer.Length)
			{
				Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, "(offset out of range)");
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, string.Concat(new string[]
			{
				"Data from ",
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				"::",
				method
			}));
			int maxDumpSizeSetting = Logging.GetMaxDumpSizeSetting(traceSource);
			if (length > maxDumpSizeSetting)
			{
				Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, string.Concat(new string[]
				{
					"(printing ",
					maxDumpSizeSetting.ToString(NumberFormatInfo.InvariantInfo),
					" out of ",
					length.ToString(NumberFormatInfo.InvariantInfo),
					")"
				}));
				length = maxDumpSizeSetting;
			}
			if (length < 0 || length > buffer.Length - offset)
			{
				length = buffer.Length - offset;
			}
			if (Logging.GetUseProtocolTextSetting(traceSource))
			{
				string text = "<<" + WebHeaderCollection.HeaderEncoding.GetString(buffer, offset, length) + ">>";
				Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, text);
				return;
			}
			do
			{
				int num = Math.Min(length, 16);
				string text2 = string.Format(CultureInfo.CurrentCulture, "{0:X8} : ", new object[] { offset });
				for (int i = 0; i < num; i++)
				{
					text2 = text2 + string.Format(CultureInfo.CurrentCulture, "{0:X2}", new object[] { buffer[offset + i] }) + ((i == 7) ? '-' : ' ');
				}
				for (int j = num; j < 16; j++)
				{
					text2 += "   ";
				}
				text2 += ": ";
				for (int k = 0; k < num; k++)
				{
					text2 += (char)((buffer[offset + k] < 32 || buffer[offset + k] > 126) ? 46 : buffer[offset + k]);
				}
				Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, text2);
				offset += num;
				length -= num;
			}
			while (length > 0);
		}

		// Token: 0x04002160 RID: 8544
		private const int DefaultMaxDumpSize = 1024;

		// Token: 0x04002161 RID: 8545
		private const bool DefaultUseProtocolTextOnly = false;

		// Token: 0x04002162 RID: 8546
		private const string AttributeNameMaxSize = "maxdatasize";

		// Token: 0x04002163 RID: 8547
		private const string AttributeNameTraceMode = "tracemode";

		// Token: 0x04002164 RID: 8548
		private const string AttributeValueProtocolOnly = "protocolonly";

		// Token: 0x04002165 RID: 8549
		private const string TraceSourceWebName = "System.Net";

		// Token: 0x04002166 RID: 8550
		private const string TraceSourceHttpListenerName = "System.Net.HttpListener";

		// Token: 0x04002167 RID: 8551
		private const string TraceSourceSocketsName = "System.Net.Sockets";

		// Token: 0x04002168 RID: 8552
		private const string TraceSourceCacheName = "System.Net.Cache";

		// Token: 0x04002169 RID: 8553
		private static bool s_LoggingEnabled = true;

		// Token: 0x0400216A RID: 8554
		private static bool s_LoggingInitialized;

		// Token: 0x0400216B RID: 8555
		private static bool s_AppDomainShutdown;

		// Token: 0x0400216C RID: 8556
		private static readonly string[] SupportedAttributes = new string[] { "maxdatasize", "tracemode" };

		// Token: 0x0400216D RID: 8557
		private static TraceSource s_WebTraceSource;

		// Token: 0x0400216E RID: 8558
		private static TraceSource s_HttpListenerTraceSource;

		// Token: 0x0400216F RID: 8559
		private static TraceSource s_SocketsTraceSource;

		// Token: 0x04002170 RID: 8560
		private static TraceSource s_CacheTraceSource;

		// Token: 0x04002171 RID: 8561
		private static object s_InternalSyncObject;

		// Token: 0x02000427 RID: 1063
		private class NclTraceSource : TraceSource
		{
			// Token: 0x06002149 RID: 8521 RVA: 0x000835CC File Offset: 0x000825CC
			internal NclTraceSource(string name)
				: base(name)
			{
			}

			// Token: 0x0600214A RID: 8522 RVA: 0x000835D5 File Offset: 0x000825D5
			protected internal override string[] GetSupportedAttributes()
			{
				return Logging.SupportedAttributes;
			}
		}
	}
}
