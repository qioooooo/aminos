using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002EB RID: 747
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebBaseErrorEvent : WebManagementEvent
	{
		// Token: 0x0600258A RID: 9610 RVA: 0x000A14A7 File Offset: 0x000A04A7
		private void Init(Exception e)
		{
			this._exception = e;
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x000A14B0 File Offset: 0x000A04B0
		protected internal WebBaseErrorEvent(string message, object eventSource, int eventCode, Exception e)
			: base(message, eventSource, eventCode)
		{
			this.Init(e);
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x000A14C3 File Offset: 0x000A04C3
		protected internal WebBaseErrorEvent(string message, object eventSource, int eventCode, int eventDetailCode, Exception e)
			: base(message, eventSource, eventCode, eventDetailCode)
		{
			this.Init(e);
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x000A14D8 File Offset: 0x000A04D8
		internal WebBaseErrorEvent()
		{
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x0600258E RID: 9614 RVA: 0x000A14E0 File Offset: 0x000A04E0
		public Exception ErrorException
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000A14E8 File Offset: 0x000A04E8
		internal override void FormatToString(WebEventFormatter formatter, bool includeAppInfo)
		{
			base.FormatToString(formatter, includeAppInfo);
			if (this._exception == null)
			{
				return;
			}
			Exception ex = this._exception;
			int num = 0;
			while (ex != null && num <= 2)
			{
				formatter.AppendLine(string.Empty);
				if (num == 0)
				{
					formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_exception_information"));
				}
				else
				{
					formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_inner_exception_information", num.ToString(CultureInfo.InstalledUICulture)));
				}
				formatter.IndentationLevel++;
				formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_exception_type", ex.GetType().ToString()));
				formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_exception_message", ex.Message));
				formatter.IndentationLevel--;
				ex = ex.InnerException;
				num++;
			}
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000A15B4 File Offset: 0x000A05B4
		internal override void GenerateFieldsForMarshal(List<WebEventFieldData> fields)
		{
			base.GenerateFieldsForMarshal(fields);
			fields.Add(new WebEventFieldData("ExceptionType", this.ErrorException.GetType().ToString(), WebEventFieldType.String));
			fields.Add(new WebEventFieldData("ExceptionMessage", this.ErrorException.Message, WebEventFieldType.String));
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000A1605 File Offset: 0x000A0605
		protected internal override void IncrementPerfCounters()
		{
			base.IncrementPerfCounters();
			PerfCounters.IncrementCounter(AppPerfCounter.EVENTS_ERROR);
			PerfCounters.IncrementGlobalCounter(GlobalPerfCounter.GLOBAL_EVENTS_ERROR);
		}

		// Token: 0x04001D66 RID: 7526
		private Exception _exception;
	}
}
