using System;
using System.Security.Permissions;
using System.Web.Configuration;

namespace System.Web.SessionState
{
	// Token: 0x0200037D RID: 893
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class StateRuntime : IStateRuntime
	{
		// Token: 0x06002B55 RID: 11093 RVA: 0x000C0FD8 File Offset: 0x000BFFD8
		static StateRuntime()
		{
			WebConfigurationFileMap webConfigurationFileMap = new WebConfigurationFileMap();
			UserMapPath userMapPath = new UserMapPath(webConfigurationFileMap);
			HttpConfigurationSystem.EnsureInit(userMapPath, false, true);
			StateApplication stateApplication = new StateApplication();
			HttpApplicationFactory.SetCustomApplication(stateApplication);
			PerfCounters.OpenStateCounters();
			StateRuntime.ResetStateServerCounters();
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x000C1010 File Offset: 0x000C0010
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public StateRuntime()
		{
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x000C1018 File Offset: 0x000C0018
		public void StopProcessing()
		{
			StateRuntime.ResetStateServerCounters();
			HttpRuntime.Close();
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x000C1024 File Offset: 0x000C0024
		private static void ResetStateServerCounters()
		{
			PerfCounters.SetStateServiceCounter(StateServicePerfCounter.STATE_SERVICE_SESSIONS_TOTAL, 0);
			PerfCounters.SetStateServiceCounter(StateServicePerfCounter.STATE_SERVICE_SESSIONS_ACTIVE, 0);
			PerfCounters.SetStateServiceCounter(StateServicePerfCounter.STATE_SERVICE_SESSIONS_TIMED_OUT, 0);
			PerfCounters.SetStateServiceCounter(StateServicePerfCounter.STATE_SERVICE_SESSIONS_ABANDONED, 0);
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x000C1048 File Offset: 0x000C0048
		public void ProcessRequest(IntPtr tracker, int verb, string uri, int exclusive, int timeout, int lockCookieExists, int lockCookie, int contentLength, IntPtr content)
		{
			this.ProcessRequest(tracker, verb, uri, exclusive, 0, timeout, lockCookieExists, lockCookie, contentLength, content);
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x000C106C File Offset: 0x000C006C
		public void ProcessRequest(IntPtr tracker, int verb, string uri, int exclusive, int extraFlags, int timeout, int lockCookieExists, int lockCookie, int contentLength, IntPtr content)
		{
			StateHttpWorkerRequest stateHttpWorkerRequest = new StateHttpWorkerRequest(tracker, (UnsafeNativeMethods.StateProtocolVerb)verb, uri, (UnsafeNativeMethods.StateProtocolExclusive)exclusive, extraFlags, timeout, lockCookieExists, lockCookie, contentLength, content);
			HttpRuntime.ProcessRequest(stateHttpWorkerRequest);
		}
	}
}
