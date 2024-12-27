using System;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002F2 RID: 754
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebSuccessAuditEvent : WebAuditEvent
	{
		// Token: 0x060025C3 RID: 9667 RVA: 0x000A1F9B File Offset: 0x000A0F9B
		protected internal WebSuccessAuditEvent(string message, object eventSource, int eventCode)
			: base(message, eventSource, eventCode)
		{
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x000A1FA6 File Offset: 0x000A0FA6
		protected internal WebSuccessAuditEvent(string message, object eventSource, int eventCode, int eventDetailCode)
			: base(message, eventSource, eventCode, eventDetailCode)
		{
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x000A1FB3 File Offset: 0x000A0FB3
		internal WebSuccessAuditEvent()
		{
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x000A1FBB File Offset: 0x000A0FBB
		protected internal override void IncrementPerfCounters()
		{
			base.IncrementPerfCounters();
			PerfCounters.IncrementCounter(AppPerfCounter.AUDIT_SUCCESS);
			PerfCounters.IncrementGlobalCounter(GlobalPerfCounter.GLOBAL_AUDIT_SUCCESS);
		}
	}
}
