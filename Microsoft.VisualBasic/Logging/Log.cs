using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Permissions;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic.Logging
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class Log
	{
		public Log()
		{
			this.m_TraceSource = new Log.DefaultTraceSource("DefaultSource");
			if (!this.m_TraceSource.HasBeenConfigured)
			{
				this.InitializeWithDefaultsSinceNoConfigExists();
			}
			AppDomain.CurrentDomain.ProcessExit += this.CloseOnProcessExit;
		}

		public Log(string name)
		{
			this.m_TraceSource = new Log.DefaultTraceSource(name);
			if (!this.m_TraceSource.HasBeenConfigured)
			{
				this.InitializeWithDefaultsSinceNoConfigExists();
			}
		}

		public void WriteEntry(string message)
		{
			this.WriteEntry(message, TraceEventType.Information, this.TraceEventTypeToId(TraceEventType.Information));
		}

		public void WriteEntry(string message, TraceEventType severity)
		{
			this.WriteEntry(message, severity, this.TraceEventTypeToId(severity));
		}

		public void WriteEntry(string message, TraceEventType severity, int id)
		{
			if (message == null)
			{
				message = "";
			}
			this.m_TraceSource.TraceEvent(severity, id, message);
		}

		public void WriteException(Exception ex)
		{
			this.WriteException(ex, TraceEventType.Error, "", this.TraceEventTypeToId(TraceEventType.Error));
		}

		public void WriteException(Exception ex, TraceEventType severity, string additionalInfo)
		{
			this.WriteException(ex, severity, additionalInfo, this.TraceEventTypeToId(severity));
		}

		public void WriteException(Exception ex, TraceEventType severity, string additionalInfo, int id)
		{
			if (ex == null)
			{
				throw ExceptionUtils.GetArgumentNullException("ex");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(ex.Message);
			if (Operators.CompareString(additionalInfo, "", false) != 0)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(additionalInfo);
			}
			this.m_TraceSource.TraceEvent(severity, id, stringBuilder.ToString());
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public TraceSource TraceSource
		{
			get
			{
				return this.m_TraceSource;
			}
		}

		public FileLogTraceListener DefaultFileLogWriter
		{
			get
			{
				return (FileLogTraceListener)this.TraceSource.Listeners["FileLog"];
			}
		}

		protected internal virtual void InitializeWithDefaultsSinceNoConfigExists()
		{
			this.m_TraceSource.Listeners.Add(new FileLogTraceListener("FileLog"));
			this.m_TraceSource.Switch.Level = SourceLevels.Information;
		}

		private void CloseOnProcessExit(object sender, EventArgs e)
		{
			AppDomain.CurrentDomain.ProcessExit -= this.CloseOnProcessExit;
			this.TraceSource.Close();
		}

		private static Dictionary<TraceEventType, int> InitializeIDHash()
		{
			Dictionary<TraceEventType, int> dictionary = new Dictionary<TraceEventType, int>(10);
			Dictionary<TraceEventType, int> dictionary2 = dictionary;
			dictionary2.Add(TraceEventType.Information, 0);
			dictionary2.Add(TraceEventType.Warning, 1);
			dictionary2.Add(TraceEventType.Error, 2);
			dictionary2.Add(TraceEventType.Critical, 3);
			dictionary2.Add(TraceEventType.Start, 4);
			dictionary2.Add(TraceEventType.Stop, 5);
			dictionary2.Add(TraceEventType.Suspend, 6);
			dictionary2.Add(TraceEventType.Resume, 7);
			dictionary2.Add(TraceEventType.Verbose, 8);
			dictionary2.Add(TraceEventType.Transfer, 9);
			return dictionary;
		}

		private int TraceEventTypeToId(TraceEventType traceEventValue)
		{
			if (Log.m_IdHash.ContainsKey(traceEventValue))
			{
				return Log.m_IdHash[traceEventValue];
			}
			return 0;
		}

		private Log.DefaultTraceSource m_TraceSource;

		private static Dictionary<TraceEventType, int> m_IdHash = Log.InitializeIDHash();

		private const string WINAPP_SOURCE_NAME = "DefaultSource";

		private const string DEFAULT_FILE_LOG_TRACE_LISTENER_NAME = "FileLog";

		internal sealed class DefaultTraceSource : TraceSource
		{
			public DefaultTraceSource(string name)
				: base(name)
			{
			}

			public bool HasBeenConfigured
			{
				get
				{
					if (this.listenerAttributes == null)
					{
						this.listenerAttributes = this.Attributes;
					}
					return this.m_HasBeenInitializedFromConfigFile;
				}
			}

			protected override string[] GetSupportedAttributes()
			{
				this.m_HasBeenInitializedFromConfigFile = true;
				return base.GetSupportedAttributes();
			}

			private bool m_HasBeenInitializedFromConfigFile;

			private StringDictionary listenerAttributes;
		}
	}
}
