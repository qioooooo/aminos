using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web.Hosting;

namespace System.Web
{
	// Token: 0x020000A4 RID: 164
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public sealed class IisTraceListener : TraceListener
	{
		// Token: 0x06000849 RID: 2121 RVA: 0x000249B4 File Offset: 0x000239B4
		public IisTraceListener()
		{
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null && !HttpRuntime.UseIntegratedPipeline && !(httpContext.WorkerRequest is ISAPIWorkerRequestInProcForIIS7))
			{
				throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_7"));
			}
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x000249F4 File Offset: 0x000239F4
		public override void Write(string message)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(null, string.Empty, TraceEventType.Verbose, 0, message, null, null, null))
			{
				return;
			}
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				httpContext.WorkerRequest.RaiseTraceEvent(IntegratedTraceType.TraceWrite, message);
			}
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00024A3C File Offset: 0x00023A3C
		public override void Write(string message, string category)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(null, string.Empty, TraceEventType.Verbose, 0, message, null, null, null))
			{
				return;
			}
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				httpContext.WorkerRequest.RaiseTraceEvent(IntegratedTraceType.TraceWrite, message);
			}
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00024A84 File Offset: 0x00023A84
		public override void WriteLine(string message)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(null, string.Empty, TraceEventType.Verbose, 0, message, null, null, null))
			{
				return;
			}
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				httpContext.WorkerRequest.RaiseTraceEvent(IntegratedTraceType.TraceWrite, message);
			}
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00024ACC File Offset: 0x00023ACC
		public override void WriteLine(string message, string category)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(null, string.Empty, TraceEventType.Verbose, 0, message, null, null, null))
			{
				return;
			}
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				httpContext.WorkerRequest.RaiseTraceEvent(IntegratedTraceType.TraceWrite, message);
			}
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x00024B14 File Offset: 0x00023B14
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
			{
				return;
			}
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				string text = string.Empty;
				if (data != null)
				{
					text = data.ToString();
				}
				httpContext.WorkerRequest.RaiseTraceEvent(this.Convert(eventType), this.AppendTraceOptions(eventCache, text));
			}
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00024B78 File Offset: 0x00023B78
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
		{
			HttpContext httpContext = HttpContext.Current;
			if (httpContext == null)
			{
				return;
			}
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (data != null)
			{
				for (int i = 0; i < data.Length; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(", ");
					}
					if (data[i] != null)
					{
						stringBuilder.Append(data[i].ToString());
					}
				}
			}
			if (httpContext != null)
			{
				httpContext.WorkerRequest.RaiseTraceEvent(this.Convert(eventType), this.AppendTraceOptions(eventCache, stringBuilder.ToString()));
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00024C10 File Offset: 0x00023C10
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
			httpContext.WorkerRequest.RaiseTraceEvent(this.Convert(severity), this.AppendTraceOptions(eventCache, message));
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00024C62 File Offset: 0x00023C62
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string format, params object[] args)
		{
			this.TraceEvent(eventCache, source, severity, id, string.Format(CultureInfo.InvariantCulture, format, args));
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00024C80 File Offset: 0x00023C80
		private string AppendTraceOptions(TraceEventCache eventCache, string message)
		{
			if (eventCache == null || base.TraceOutputOptions == TraceOptions.None)
			{
				return message;
			}
			StringBuilder stringBuilder = new StringBuilder(message, 1024);
			if (this.IsEnabled(TraceOptions.ProcessId))
			{
				stringBuilder.Append("\r\nProcessId=");
				stringBuilder.Append(eventCache.ProcessId);
			}
			if (this.IsEnabled(TraceOptions.LogicalOperationStack))
			{
				stringBuilder.Append("\r\nLogicalOperationStack=");
				bool flag = true;
				foreach (object obj in eventCache.LogicalOperationStack)
				{
					if (!flag)
					{
						stringBuilder.Append(", ");
					}
					else
					{
						flag = false;
					}
					stringBuilder.Append(obj);
				}
			}
			if (this.IsEnabled(TraceOptions.ThreadId))
			{
				stringBuilder.Append("\r\nThreadId=");
				stringBuilder.Append(eventCache.ThreadId);
			}
			if (this.IsEnabled(TraceOptions.DateTime))
			{
				stringBuilder.Append("\r\nDateTime=");
				stringBuilder.Append(eventCache.DateTime.ToString("o", CultureInfo.InvariantCulture));
			}
			if (this.IsEnabled(TraceOptions.Timestamp))
			{
				stringBuilder.Append("\r\nTimestamp=");
				stringBuilder.Append(eventCache.Timestamp);
			}
			if (this.IsEnabled(TraceOptions.Callstack))
			{
				stringBuilder.Append("\r\nCallstack=");
				stringBuilder.Append(eventCache.Callstack);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00024DE0 File Offset: 0x00023DE0
		private bool IsEnabled(TraceOptions opts)
		{
			return (opts & base.TraceOutputOptions) != TraceOptions.None;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00024DF0 File Offset: 0x00023DF0
		private IntegratedTraceType Convert(TraceEventType tet)
		{
			if (tet <= TraceEventType.Start)
			{
				if (tet <= TraceEventType.Information)
				{
					switch (tet)
					{
					case TraceEventType.Critical:
						return IntegratedTraceType.DiagCritical;
					case TraceEventType.Error:
						return IntegratedTraceType.DiagError;
					case (TraceEventType)3:
						break;
					case TraceEventType.Warning:
						return IntegratedTraceType.DiagWarning;
					default:
						if (tet == TraceEventType.Information)
						{
							return IntegratedTraceType.DiagInfo;
						}
						break;
					}
				}
				else
				{
					if (tet == TraceEventType.Verbose)
					{
						return IntegratedTraceType.DiagVerbose;
					}
					if (tet == TraceEventType.Start)
					{
						return IntegratedTraceType.DiagStart;
					}
				}
			}
			else if (tet <= TraceEventType.Suspend)
			{
				if (tet == TraceEventType.Stop)
				{
					return IntegratedTraceType.DiagStop;
				}
				if (tet == TraceEventType.Suspend)
				{
					return IntegratedTraceType.DiagSuspend;
				}
			}
			else
			{
				if (tet == TraceEventType.Resume)
				{
					return IntegratedTraceType.DiagResume;
				}
				if (tet == TraceEventType.Transfer)
				{
					return IntegratedTraceType.DiagTransfer;
				}
			}
			return IntegratedTraceType.DiagVerbose;
		}
	}
}
