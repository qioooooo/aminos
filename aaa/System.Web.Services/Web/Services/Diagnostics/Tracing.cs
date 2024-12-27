using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace System.Web.Services.Diagnostics
{
	// Token: 0x02000131 RID: 305
	internal static class Tracing
	{
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x00044F4C File Offset: 0x00043F4C
		private static object InternalSyncObject
		{
			get
			{
				if (Tracing.internalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref Tracing.internalSyncObject, obj, null);
				}
				return Tracing.internalSyncObject;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x00044F78 File Offset: 0x00043F78
		internal static bool On
		{
			get
			{
				if (!Tracing.tracingInitialized)
				{
					Tracing.InitializeLogging();
				}
				return Tracing.tracingEnabled;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x00044F8B File Offset: 0x00043F8B
		internal static bool IsVerbose
		{
			get
			{
				return Tracing.ValidateSettings(Tracing.Asmx, TraceEventType.Verbose);
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x00044F99 File Offset: 0x00043F99
		internal static TraceSource Asmx
		{
			get
			{
				if (!Tracing.tracingInitialized)
				{
					Tracing.InitializeLogging();
				}
				if (!Tracing.tracingEnabled)
				{
					return null;
				}
				return Tracing.asmxTraceSource;
			}
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00044FB8 File Offset: 0x00043FB8
		private static void InitializeLogging()
		{
			lock (Tracing.InternalSyncObject)
			{
				if (!Tracing.tracingInitialized)
				{
					bool flag = false;
					Tracing.asmxTraceSource = new TraceSource("System.Web.Services.Asmx");
					if (Tracing.asmxTraceSource.Switch.ShouldTrace(TraceEventType.Critical))
					{
						flag = true;
						AppDomain currentDomain = AppDomain.CurrentDomain;
						currentDomain.UnhandledException += Tracing.UnhandledExceptionHandler;
						currentDomain.DomainUnload += Tracing.AppDomainUnloadEvent;
						currentDomain.ProcessExit += Tracing.ProcessExitEvent;
					}
					Tracing.tracingEnabled = flag;
					Tracing.tracingInitialized = true;
				}
			}
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x00045060 File Offset: 0x00044060
		private static void Close()
		{
			if (Tracing.asmxTraceSource != null)
			{
				Tracing.asmxTraceSource.Close();
			}
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x00045074 File Offset: 0x00044074
		private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception ex = (Exception)args.ExceptionObject;
			Tracing.ExceptionCatch(TraceEventType.Error, sender, "UnhandledExceptionHandler", ex);
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0004509B File Offset: 0x0004409B
		private static void ProcessExitEvent(object sender, EventArgs e)
		{
			Tracing.Close();
			Tracing.appDomainShutdown = true;
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x000450A8 File Offset: 0x000440A8
		private static void AppDomainUnloadEvent(object sender, EventArgs e)
		{
			Tracing.Close();
			Tracing.appDomainShutdown = true;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x000450B5 File Offset: 0x000440B5
		private static bool ValidateSettings(TraceSource traceSource, TraceEventType traceLevel)
		{
			if (!Tracing.tracingEnabled)
			{
				return false;
			}
			if (!Tracing.tracingInitialized)
			{
				Tracing.InitializeLogging();
			}
			return traceSource != null && traceSource.Switch.ShouldTrace(traceLevel) && !Tracing.appDomainShutdown;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x000450E9 File Offset: 0x000440E9
		internal static void Information(string format, params object[] args)
		{
			if (!Tracing.ValidateSettings(Tracing.Asmx, TraceEventType.Information))
			{
				return;
			}
			Tracing.TraceEvent(TraceEventType.Information, Res.GetString(format, args));
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x00045106 File Offset: 0x00044106
		private static void TraceEvent(TraceEventType eventType, string format)
		{
			Tracing.Asmx.TraceEvent(eventType, 0, format);
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00045115 File Offset: 0x00044115
		internal static Exception ExceptionThrow(TraceMethod method, Exception e)
		{
			return Tracing.ExceptionThrow(TraceEventType.Error, method, e);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x00045120 File Offset: 0x00044120
		internal static Exception ExceptionThrow(TraceEventType eventType, TraceMethod method, Exception e)
		{
			if (!Tracing.ValidateSettings(Tracing.Asmx, eventType))
			{
				return e;
			}
			Tracing.TraceEvent(eventType, Res.GetString("TraceExceptionThrown", new object[]
			{
				method.ToString(),
				e.GetType(),
				e.Message
			}));
			Tracing.StackTrace(eventType, e);
			return e;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00045177 File Offset: 0x00044177
		internal static Exception ExceptionCatch(TraceMethod method, Exception e)
		{
			return Tracing.ExceptionCatch(TraceEventType.Error, method, e);
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00045184 File Offset: 0x00044184
		internal static Exception ExceptionCatch(TraceEventType eventType, TraceMethod method, Exception e)
		{
			if (!Tracing.ValidateSettings(Tracing.Asmx, eventType))
			{
				return e;
			}
			Tracing.TraceEvent(eventType, Res.GetString("TraceExceptionCought", new object[]
			{
				method,
				e.GetType(),
				e.Message
			}));
			Tracing.StackTrace(eventType, e);
			return e;
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x000451D8 File Offset: 0x000441D8
		internal static Exception ExceptionCatch(TraceEventType eventType, object target, string method, Exception e)
		{
			if (!Tracing.ValidateSettings(Tracing.Asmx, eventType))
			{
				return e;
			}
			Tracing.TraceEvent(eventType, Res.GetString("TraceExceptionCought", new object[]
			{
				TraceMethod.MethodId(target, method),
				e.GetType(),
				e.Message
			}));
			Tracing.StackTrace(eventType, e);
			return e;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x00045230 File Offset: 0x00044230
		internal static Exception ExceptionIgnore(TraceEventType eventType, TraceMethod method, Exception e)
		{
			if (!Tracing.ValidateSettings(Tracing.Asmx, eventType))
			{
				return e;
			}
			Tracing.TraceEvent(eventType, Res.GetString("TraceExceptionIgnored", new object[]
			{
				method,
				e.GetType(),
				e.Message
			}));
			Tracing.StackTrace(eventType, e);
			return e;
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00045284 File Offset: 0x00044284
		private static void StackTrace(TraceEventType eventType, Exception e)
		{
			if (Tracing.IsVerbose && !string.IsNullOrEmpty(e.StackTrace))
			{
				Tracing.TraceEvent(eventType, Res.GetString("TraceExceptionDetails", new object[] { e.ToString() }));
			}
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x000452C6 File Offset: 0x000442C6
		internal static string TraceId(string id)
		{
			return Res.GetString(id);
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x000452D0 File Offset: 0x000442D0
		private static string GetHostByAddress(string ipAddress)
		{
			string text;
			try
			{
				text = Dns.GetHostByAddress(ipAddress).HostName;
			}
			catch
			{
				text = null;
			}
			return text;
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00045304 File Offset: 0x00044304
		internal static List<string> Details(HttpRequest request)
		{
			if (request == null)
			{
				return null;
			}
			List<string> list = new List<string>();
			list.Add(Res.GetString("TraceUserHostAddress", new object[] { request.UserHostAddress }));
			string text = ((request.UserHostAddress == request.UserHostName) ? Tracing.GetHostByAddress(request.UserHostAddress) : request.UserHostName);
			if (!string.IsNullOrEmpty(text))
			{
				list.Add(Res.GetString("TraceUserHostName", new object[] { text }));
			}
			list.Add(Res.GetString("TraceUrl", new object[] { request.HttpMethod, request.Url }));
			if (request.UrlReferrer != null)
			{
				list.Add(Res.GetString("TraceUrlReferrer", new object[] { request.UrlReferrer }));
			}
			return list;
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x000453EB File Offset: 0x000443EB
		internal static void Enter(string callId, TraceMethod caller)
		{
			Tracing.Enter(callId, caller, null, null);
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x000453F6 File Offset: 0x000443F6
		internal static void Enter(string callId, TraceMethod caller, List<string> details)
		{
			Tracing.Enter(callId, caller, null, details);
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00045401 File Offset: 0x00044401
		internal static void Enter(string callId, TraceMethod caller, TraceMethod callDetails)
		{
			Tracing.Enter(callId, caller, callDetails, null);
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0004540C File Offset: 0x0004440C
		internal static void Enter(string callId, TraceMethod caller, TraceMethod callDetails, List<string> details)
		{
			if (!Tracing.ValidateSettings(Tracing.Asmx, TraceEventType.Information))
			{
				return;
			}
			string text = ((callDetails == null) ? Res.GetString("TraceCallEnter", new object[] { callId, caller }) : Res.GetString("TraceCallEnterDetails", new object[] { callId, caller, callDetails }));
			if (details != null && details.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder(text);
				foreach (string text2 in details)
				{
					stringBuilder.Append(Environment.NewLine);
					stringBuilder.Append("    ");
					stringBuilder.Append(text2);
				}
				text = stringBuilder.ToString();
			}
			Tracing.TraceEvent(TraceEventType.Information, text);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x000454E8 File Offset: 0x000444E8
		internal static XmlDeserializationEvents GetDeserializationEvents()
		{
			return new XmlDeserializationEvents
			{
				OnUnknownElement = new XmlElementEventHandler(Tracing.OnUnknownElement),
				OnUnknownAttribute = new XmlAttributeEventHandler(Tracing.OnUnknownAttribute)
			};
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00045524 File Offset: 0x00044524
		internal static void Exit(string callId, TraceMethod caller)
		{
			if (!Tracing.ValidateSettings(Tracing.Asmx, TraceEventType.Information))
			{
				return;
			}
			Tracing.TraceEvent(TraceEventType.Information, Res.GetString("TraceCallExit", new object[] { callId, caller }));
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00045560 File Offset: 0x00044560
		internal static void OnUnknownElement(object sender, XmlElementEventArgs e)
		{
			if (!Tracing.ValidateSettings(Tracing.Asmx, TraceEventType.Warning))
			{
				return;
			}
			if (e.Element == null)
			{
				return;
			}
			string text = RuntimeUtils.ElementString(e.Element);
			string text2 = ((e.ExpectedElements == null) ? "WebUnknownElement" : ((e.ExpectedElements.Length == 0) ? "WebUnknownElement1" : "WebUnknownElement2"));
			Tracing.TraceEvent(TraceEventType.Warning, Res.GetString(text2, new object[] { text, e.ExpectedElements }));
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x000455DC File Offset: 0x000445DC
		internal static void OnUnknownAttribute(object sender, XmlAttributeEventArgs e)
		{
			if (!Tracing.ValidateSettings(Tracing.Asmx, TraceEventType.Warning))
			{
				return;
			}
			if (e.Attr == null)
			{
				return;
			}
			if (RuntimeUtils.IsKnownNamespace(e.Attr.NamespaceURI))
			{
				return;
			}
			string text = ((e.ExpectedAttributes == null) ? "WebUnknownAttribute" : ((e.ExpectedAttributes.Length == 0) ? "WebUnknownAttribute2" : "WebUnknownAttribute3"));
			Tracing.TraceEvent(TraceEventType.Warning, Res.GetString(text, new object[]
			{
				e.Attr.Name,
				e.Attr.Value,
				e.ExpectedAttributes
			}));
		}

		// Token: 0x040005FE RID: 1534
		private const string TraceSourceAsmx = "System.Web.Services.Asmx";

		// Token: 0x040005FF RID: 1535
		private static bool tracingEnabled = true;

		// Token: 0x04000600 RID: 1536
		private static bool tracingInitialized;

		// Token: 0x04000601 RID: 1537
		private static bool appDomainShutdown;

		// Token: 0x04000602 RID: 1538
		private static TraceSource asmxTraceSource;

		// Token: 0x04000603 RID: 1539
		private static object internalSyncObject;
	}
}
