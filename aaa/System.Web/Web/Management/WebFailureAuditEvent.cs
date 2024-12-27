using System;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002EF RID: 751
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebFailureAuditEvent : WebAuditEvent
	{
		// Token: 0x060025B2 RID: 9650 RVA: 0x000A1D60 File Offset: 0x000A0D60
		protected internal WebFailureAuditEvent(string message, object eventSource, int eventCode)
			: base(message, eventSource, eventCode)
		{
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x000A1D6B File Offset: 0x000A0D6B
		protected internal WebFailureAuditEvent(string message, object eventSource, int eventCode, int eventDetailCode)
			: base(message, eventSource, eventCode, eventDetailCode)
		{
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x000A1D78 File Offset: 0x000A0D78
		internal WebFailureAuditEvent()
		{
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x000A1D80 File Offset: 0x000A0D80
		protected internal override void IncrementPerfCounters()
		{
			base.IncrementPerfCounters();
			PerfCounters.IncrementCounter(AppPerfCounter.AUDIT_FAIL);
			PerfCounters.IncrementGlobalCounter(GlobalPerfCounter.GLOBAL_AUDIT_FAIL);
		}
	}
}
