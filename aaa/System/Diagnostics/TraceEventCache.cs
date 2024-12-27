using System;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x020001D6 RID: 470
	public class TraceEventCache
	{
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000EBA RID: 3770 RVA: 0x0002E398 File Offset: 0x0002D398
		internal Guid ActivityId
		{
			get
			{
				return Trace.CorrelationManager.ActivityId;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000EBB RID: 3771 RVA: 0x0002E3A4 File Offset: 0x0002D3A4
		public string Callstack
		{
			get
			{
				if (this.stackTrace == null)
				{
					this.stackTrace = Environment.StackTrace;
				}
				else
				{
					new EnvironmentPermission(PermissionState.Unrestricted).Demand();
				}
				return this.stackTrace;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x0002E3CC File Offset: 0x0002D3CC
		public Stack LogicalOperationStack
		{
			get
			{
				return Trace.CorrelationManager.LogicalOperationStack;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000EBD RID: 3773 RVA: 0x0002E3D8 File Offset: 0x0002D3D8
		public DateTime DateTime
		{
			get
			{
				if (this.dateTime == DateTime.MinValue)
				{
					this.dateTime = DateTime.UtcNow;
				}
				return this.dateTime;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000EBE RID: 3774 RVA: 0x0002E3FD File Offset: 0x0002D3FD
		public int ProcessId
		{
			get
			{
				return TraceEventCache.GetProcessId();
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000EBF RID: 3775 RVA: 0x0002E404 File Offset: 0x0002D404
		public string ThreadId
		{
			get
			{
				return TraceEventCache.GetThreadId().ToString(CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x0002E423 File Offset: 0x0002D423
		public long Timestamp
		{
			get
			{
				if (this.timeStamp == -1L)
				{
					this.timeStamp = Stopwatch.GetTimestamp();
				}
				return this.timeStamp;
			}
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0002E440 File Offset: 0x0002D440
		internal void Clear()
		{
			this.timeStamp = -1L;
			this.dateTime = DateTime.MinValue;
			this.stackTrace = null;
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0002E45C File Offset: 0x0002D45C
		private static void InitProcessInfo()
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			if (TraceEventCache.processName == null)
			{
				Process currentProcess = Process.GetCurrentProcess();
				try
				{
					TraceEventCache.processId = currentProcess.Id;
					TraceEventCache.processName = currentProcess.ProcessName;
				}
				finally
				{
					currentProcess.Dispose();
				}
			}
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0002E4B0 File Offset: 0x0002D4B0
		internal static int GetProcessId()
		{
			TraceEventCache.InitProcessInfo();
			return TraceEventCache.processId;
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0002E4BC File Offset: 0x0002D4BC
		internal static string GetProcessName()
		{
			TraceEventCache.InitProcessInfo();
			return TraceEventCache.processName;
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0002E4C8 File Offset: 0x0002D4C8
		internal static int GetThreadId()
		{
			return Thread.CurrentThread.ManagedThreadId;
		}

		// Token: 0x04000F14 RID: 3860
		private static int processId;

		// Token: 0x04000F15 RID: 3861
		private static string processName;

		// Token: 0x04000F16 RID: 3862
		private long timeStamp = -1L;

		// Token: 0x04000F17 RID: 3863
		private DateTime dateTime = DateTime.MinValue;

		// Token: 0x04000F18 RID: 3864
		private string stackTrace;
	}
}
