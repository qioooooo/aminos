using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x020000EB RID: 235
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebPageTraceListener : TraceListener
	{
		// Token: 0x06000B09 RID: 2825 RVA: 0x0002C238 File Offset: 0x0002B238
		public override void Write(string message)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(null, string.Empty, TraceEventType.Verbose, 0, message, null, null, null))
			{
				return;
			}
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				httpContext.Trace.WriteInternal(message, false);
			}
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0002C280 File Offset: 0x0002B280
		public override void Write(string message, string category)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(null, string.Empty, TraceEventType.Verbose, 0, message, null, null, null))
			{
				return;
			}
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				httpContext.Trace.WriteInternal(category, message, false);
			}
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x0002C2C8 File Offset: 0x0002B2C8
		public override void WriteLine(string message)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(null, string.Empty, TraceEventType.Verbose, 0, message, null, null, null))
			{
				return;
			}
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				httpContext.Trace.WriteInternal(message, false);
			}
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0002C310 File Offset: 0x0002B310
		public override void WriteLine(string message, string category)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(null, string.Empty, TraceEventType.Verbose, 0, message, null, null, null))
			{
				return;
			}
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				httpContext.Trace.WriteInternal(category, message, false);
			}
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0002C358 File Offset: 0x0002B358
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string message)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, severity, id, message, null, null, null))
			{
				return;
			}
			HttpContext httpContext = HttpContext.Current;
			if (httpContext == null)
			{
				return;
			}
			string text = string.Concat(new object[]
			{
				SR.GetString("WebPageTraceListener_Event"),
				" ",
				id,
				": ",
				message
			});
			if (severity <= TraceEventType.Warning)
			{
				httpContext.Trace.WarnInternal(source, text, false);
				return;
			}
			httpContext.Trace.WriteInternal(source, text, false);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0002C3EA File Offset: 0x0002B3EA
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string format, params object[] args)
		{
			this.TraceEvent(eventCache, source, severity, id, string.Format(CultureInfo.InvariantCulture, format, args));
		}
	}
}
