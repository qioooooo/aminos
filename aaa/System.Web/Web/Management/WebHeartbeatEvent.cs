using System;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002E8 RID: 744
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebHeartbeatEvent : WebManagementEvent
	{
		// Token: 0x06002577 RID: 9591 RVA: 0x000A118C File Offset: 0x000A018C
		protected internal WebHeartbeatEvent(string message, int eventCode)
			: base(message, null, eventCode)
		{
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000A1197 File Offset: 0x000A0197
		internal WebHeartbeatEvent()
		{
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06002579 RID: 9593 RVA: 0x000A119F File Offset: 0x000A019F
		public WebProcessStatistics ProcessStatistics
		{
			get
			{
				return WebHeartbeatEvent.s_procStats;
			}
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x000A11A8 File Offset: 0x000A01A8
		internal override void FormatToString(WebEventFormatter formatter, bool includeAppInfo)
		{
			base.FormatToString(formatter, includeAppInfo);
			formatter.AppendLine(string.Empty);
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_process_statistics"));
			formatter.IndentationLevel++;
			WebHeartbeatEvent.s_procStats.FormatToString(formatter);
			formatter.IndentationLevel--;
		}

		// Token: 0x04001D64 RID: 7524
		private static WebProcessStatistics s_procStats = new WebProcessStatistics();
	}
}
