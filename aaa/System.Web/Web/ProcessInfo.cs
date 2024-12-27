using System;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x020000BF RID: 191
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ProcessInfo
	{
		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x00028044 File Offset: 0x00027044
		public DateTime StartTime
		{
			get
			{
				return this._StartTime;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x0002804C File Offset: 0x0002704C
		public TimeSpan Age
		{
			get
			{
				return this._Age;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x00028054 File Offset: 0x00027054
		public int ProcessID
		{
			get
			{
				return this._ProcessID;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x0002805C File Offset: 0x0002705C
		public int RequestCount
		{
			get
			{
				return this._RequestCount;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x00028064 File Offset: 0x00027064
		public ProcessStatus Status
		{
			get
			{
				return this._Status;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0002806C File Offset: 0x0002706C
		public ProcessShutdownReason ShutdownReason
		{
			get
			{
				return this._ShutdownReason;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x00028074 File Offset: 0x00027074
		public int PeakMemoryUsed
		{
			get
			{
				return this._PeakMemoryUsed;
			}
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0002807C File Offset: 0x0002707C
		public void SetAll(DateTime startTime, TimeSpan age, int processID, int requestCount, ProcessStatus status, ProcessShutdownReason shutdownReason, int peakMemoryUsed)
		{
			this._StartTime = startTime;
			this._Age = age;
			this._ProcessID = processID;
			this._RequestCount = requestCount;
			this._Status = status;
			this._ShutdownReason = shutdownReason;
			this._PeakMemoryUsed = peakMemoryUsed;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x000280B3 File Offset: 0x000270B3
		public ProcessInfo(DateTime startTime, TimeSpan age, int processID, int requestCount, ProcessStatus status, ProcessShutdownReason shutdownReason, int peakMemoryUsed)
		{
			this._StartTime = startTime;
			this._Age = age;
			this._ProcessID = processID;
			this._RequestCount = requestCount;
			this._Status = status;
			this._ShutdownReason = shutdownReason;
			this._PeakMemoryUsed = peakMemoryUsed;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x000280F0 File Offset: 0x000270F0
		public ProcessInfo()
		{
		}

		// Token: 0x040011F9 RID: 4601
		private DateTime _StartTime;

		// Token: 0x040011FA RID: 4602
		private TimeSpan _Age;

		// Token: 0x040011FB RID: 4603
		private int _ProcessID;

		// Token: 0x040011FC RID: 4604
		private int _RequestCount;

		// Token: 0x040011FD RID: 4605
		private ProcessStatus _Status;

		// Token: 0x040011FE RID: 4606
		private ProcessShutdownReason _ShutdownReason;

		// Token: 0x040011FF RID: 4607
		private int _PeakMemoryUsed;
	}
}
