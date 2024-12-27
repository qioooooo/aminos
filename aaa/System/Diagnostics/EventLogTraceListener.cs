using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Diagnostics
{
	// Token: 0x0200075A RID: 1882
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public sealed class EventLogTraceListener : TraceListener
	{
		// Token: 0x060039A0 RID: 14752 RVA: 0x000F462B File Offset: 0x000F362B
		public EventLogTraceListener()
		{
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x000F4633 File Offset: 0x000F3633
		public EventLogTraceListener(EventLog eventLog)
			: base((eventLog != null) ? eventLog.Source : string.Empty)
		{
			this.eventLog = eventLog;
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x000F4652 File Offset: 0x000F3652
		public EventLogTraceListener(string source)
		{
			this.eventLog = new EventLog();
			this.eventLog.Source = source;
		}

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x060039A3 RID: 14755 RVA: 0x000F4671 File Offset: 0x000F3671
		// (set) Token: 0x060039A4 RID: 14756 RVA: 0x000F4679 File Offset: 0x000F3679
		public EventLog EventLog
		{
			get
			{
				return this.eventLog;
			}
			set
			{
				this.eventLog = value;
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x060039A5 RID: 14757 RVA: 0x000F4682 File Offset: 0x000F3682
		// (set) Token: 0x060039A6 RID: 14758 RVA: 0x000F46B2 File Offset: 0x000F36B2
		public override string Name
		{
			get
			{
				if (!this.nameSet && this.eventLog != null)
				{
					this.nameSet = true;
					base.Name = this.eventLog.Source;
				}
				return base.Name;
			}
			set
			{
				this.nameSet = true;
				base.Name = value;
			}
		}

		// Token: 0x060039A7 RID: 14759 RVA: 0x000F46C2 File Offset: 0x000F36C2
		public override void Close()
		{
			if (this.eventLog != null)
			{
				this.eventLog.Close();
			}
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x000F46D7 File Offset: 0x000F36D7
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Close();
			}
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x000F46E2 File Offset: 0x000F36E2
		public override void Write(string message)
		{
			if (this.eventLog != null)
			{
				this.eventLog.WriteEntry(message);
			}
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x000F46F8 File Offset: 0x000F36F8
		public override void WriteLine(string message)
		{
			this.Write(message);
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x000F4704 File Offset: 0x000F3704
		[ComVisible(false)]
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string format, params object[] args)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, severity, id, format, args))
			{
				return;
			}
			EventInstance eventInstance = this.CreateEventInstance(severity, id);
			if (args == null)
			{
				this.eventLog.WriteEvent(eventInstance, new object[] { format });
				return;
			}
			if (string.IsNullOrEmpty(format))
			{
				string[] array = new string[args.Length];
				for (int i = 0; i < args.Length; i++)
				{
					array[i] = args[i].ToString();
				}
				this.eventLog.WriteEvent(eventInstance, array);
				return;
			}
			this.eventLog.WriteEvent(eventInstance, new object[] { string.Format(CultureInfo.InvariantCulture, format, args) });
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x000F47BC File Offset: 0x000F37BC
		[ComVisible(false)]
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string message)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, severity, id, message))
			{
				return;
			}
			EventInstance eventInstance = this.CreateEventInstance(severity, id);
			this.eventLog.WriteEvent(eventInstance, new object[] { message });
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x000F480C File Offset: 0x000F380C
		[ComVisible(false)]
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, object data)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, severity, id, null, null, data))
			{
				return;
			}
			EventInstance eventInstance = this.CreateEventInstance(severity, id);
			this.eventLog.WriteEvent(eventInstance, new object[] { data });
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x000F485C File Offset: 0x000F385C
		[ComVisible(false)]
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, params object[] data)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, severity, id, null, null, null, data))
			{
				return;
			}
			EventInstance eventInstance = this.CreateEventInstance(severity, id);
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
			this.eventLog.WriteEvent(eventInstance, new object[] { stringBuilder.ToString() });
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x000F48F0 File Offset: 0x000F38F0
		private EventInstance CreateEventInstance(TraceEventType severity, int id)
		{
			if (id > 65535)
			{
				id = 65535;
			}
			if (id < 0)
			{
				id = 0;
			}
			EventInstance eventInstance = new EventInstance((long)id, 0);
			if (severity == TraceEventType.Error || severity == TraceEventType.Critical)
			{
				eventInstance.EntryType = EventLogEntryType.Error;
			}
			else if (severity == TraceEventType.Warning)
			{
				eventInstance.EntryType = EventLogEntryType.Warning;
			}
			else
			{
				eventInstance.EntryType = EventLogEntryType.Information;
			}
			return eventInstance;
		}

		// Token: 0x040032C4 RID: 12996
		private EventLog eventLog;

		// Token: 0x040032C5 RID: 12997
		private bool nameSet;
	}
}
