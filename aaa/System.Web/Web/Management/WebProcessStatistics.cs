using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Permissions;
using System.Web.Hosting;

namespace System.Web.Management
{
	// Token: 0x020002F7 RID: 759
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebProcessStatistics
	{
		// Token: 0x060025E3 RID: 9699 RVA: 0x000A2464 File Offset: 0x000A1464
		static WebProcessStatistics()
		{
			try
			{
				WebProcessStatistics.s_startTime = Process.GetCurrentProcess().StartTime;
			}
			catch
			{
				WebProcessStatistics.s_getCurrentProcFailed = true;
			}
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x000A24CC File Offset: 0x000A14CC
		private void Update()
		{
			DateTime now = DateTime.Now;
			if (now - WebProcessStatistics.s_lastUpdated < WebProcessStatistics.TS_ONE_SECOND)
			{
				return;
			}
			lock (WebProcessStatistics.s_lockObject)
			{
				if (!(now - WebProcessStatistics.s_lastUpdated < WebProcessStatistics.TS_ONE_SECOND))
				{
					if (!WebProcessStatistics.s_getCurrentProcFailed)
					{
						Process currentProcess = Process.GetCurrentProcess();
						WebProcessStatistics.s_threadCount = currentProcess.Threads.Count;
						WebProcessStatistics.s_workingSet = currentProcess.WorkingSet64;
						WebProcessStatistics.s_peakWorkingSet = currentProcess.PeakWorkingSet64;
					}
					WebProcessStatistics.s_managedHeapSize = GC.GetTotalMemory(false);
					WebProcessStatistics.s_appdomainCount = HostingEnvironment.AppDomainsCount;
					WebProcessStatistics.s_requestsExecuting = PerfCounters.GetGlobalCounter(GlobalPerfCounter.REQUESTS_CURRENT);
					WebProcessStatistics.s_requestsQueued = PerfCounters.GetGlobalCounter(GlobalPerfCounter.REQUESTS_QUEUED);
					WebProcessStatistics.s_requestsRejected = PerfCounters.GetGlobalCounter(GlobalPerfCounter.REQUESTS_REJECTED);
					WebProcessStatistics.s_lastUpdated = now;
				}
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x060025E5 RID: 9701 RVA: 0x000A25A8 File Offset: 0x000A15A8
		public DateTime ProcessStartTime
		{
			get
			{
				this.Update();
				return WebProcessStatistics.s_startTime;
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x060025E6 RID: 9702 RVA: 0x000A25B5 File Offset: 0x000A15B5
		public int ThreadCount
		{
			get
			{
				this.Update();
				return WebProcessStatistics.s_threadCount;
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x060025E7 RID: 9703 RVA: 0x000A25C2 File Offset: 0x000A15C2
		public long WorkingSet
		{
			get
			{
				this.Update();
				return WebProcessStatistics.s_workingSet;
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x060025E8 RID: 9704 RVA: 0x000A25CF File Offset: 0x000A15CF
		public long PeakWorkingSet
		{
			get
			{
				this.Update();
				return WebProcessStatistics.s_peakWorkingSet;
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x060025E9 RID: 9705 RVA: 0x000A25DC File Offset: 0x000A15DC
		public long ManagedHeapSize
		{
			get
			{
				this.Update();
				return WebProcessStatistics.s_managedHeapSize;
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x060025EA RID: 9706 RVA: 0x000A25E9 File Offset: 0x000A15E9
		public int AppDomainCount
		{
			get
			{
				this.Update();
				return WebProcessStatistics.s_appdomainCount;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x060025EB RID: 9707 RVA: 0x000A25F6 File Offset: 0x000A15F6
		public int RequestsExecuting
		{
			get
			{
				this.Update();
				return WebProcessStatistics.s_requestsExecuting;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x060025EC RID: 9708 RVA: 0x000A2603 File Offset: 0x000A1603
		public int RequestsQueued
		{
			get
			{
				this.Update();
				return WebProcessStatistics.s_requestsQueued;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x060025ED RID: 9709 RVA: 0x000A2610 File Offset: 0x000A1610
		public int RequestsRejected
		{
			get
			{
				this.Update();
				return WebProcessStatistics.s_requestsRejected;
			}
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000A2620 File Offset: 0x000A1620
		public virtual void FormatToString(WebEventFormatter formatter)
		{
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_process_start_time", this.ProcessStartTime.ToString(CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_thread_count", this.ThreadCount.ToString(CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_working_set", this.WorkingSet.ToString(CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_peak_working_set", this.PeakWorkingSet.ToString(CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_managed_heap_size", this.ManagedHeapSize.ToString(CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_application_domain_count", this.AppDomainCount.ToString(CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_requests_executing", this.RequestsExecuting.ToString(CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_request_queued", this.RequestsQueued.ToString(CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_request_rejected", this.RequestsRejected.ToString(CultureInfo.InstalledUICulture)));
		}

		// Token: 0x04001D7C RID: 7548
		private static DateTime s_startTime = DateTime.MinValue;

		// Token: 0x04001D7D RID: 7549
		private static DateTime s_lastUpdated = DateTime.MinValue;

		// Token: 0x04001D7E RID: 7550
		private static int s_threadCount;

		// Token: 0x04001D7F RID: 7551
		private static long s_workingSet;

		// Token: 0x04001D80 RID: 7552
		private static long s_peakWorkingSet;

		// Token: 0x04001D81 RID: 7553
		private static long s_managedHeapSize;

		// Token: 0x04001D82 RID: 7554
		private static int s_appdomainCount;

		// Token: 0x04001D83 RID: 7555
		private static int s_requestsExecuting;

		// Token: 0x04001D84 RID: 7556
		private static int s_requestsQueued;

		// Token: 0x04001D85 RID: 7557
		private static int s_requestsRejected;

		// Token: 0x04001D86 RID: 7558
		private static bool s_getCurrentProcFailed = false;

		// Token: 0x04001D87 RID: 7559
		private static object s_lockObject = new object();

		// Token: 0x04001D88 RID: 7560
		private static TimeSpan TS_ONE_SECOND = new TimeSpan(0, 0, 1);
	}
}
