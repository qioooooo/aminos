using System;
using System.Collections.Generic;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002EA RID: 746
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebRequestEvent : WebManagementEvent
	{
		// Token: 0x06002581 RID: 9601 RVA: 0x000A12EF File Offset: 0x000A02EF
		internal override void PreProcessEventInit()
		{
			base.PreProcessEventInit();
			this.InitRequestInformation();
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x000A12FD File Offset: 0x000A02FD
		protected internal WebRequestEvent(string message, object eventSource, int eventCode)
			: base(message, eventSource, eventCode)
		{
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x000A1308 File Offset: 0x000A0308
		protected internal WebRequestEvent(string message, object eventSource, int eventCode, int eventDetailCode)
			: base(message, eventSource, eventCode, eventDetailCode)
		{
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x000A1315 File Offset: 0x000A0315
		internal WebRequestEvent()
		{
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x000A131D File Offset: 0x000A031D
		private void InitRequestInformation()
		{
			if (this._requestInfo == null)
			{
				this._requestInfo = new WebRequestInformation();
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06002586 RID: 9606 RVA: 0x000A1332 File Offset: 0x000A0332
		public WebRequestInformation RequestInformation
		{
			get
			{
				this.InitRequestInformation();
				return this._requestInfo;
			}
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x000A1340 File Offset: 0x000A0340
		internal override void GenerateFieldsForMarshal(List<WebEventFieldData> fields)
		{
			base.GenerateFieldsForMarshal(fields);
			fields.Add(new WebEventFieldData("RequestUrl", this.RequestInformation.RequestUrl, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("RequestPath", this.RequestInformation.RequestPath, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("UserHostAddress", this.RequestInformation.UserHostAddress, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("UserName", this.RequestInformation.Principal.Identity.Name, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("UserAuthenticated", this.RequestInformation.Principal.Identity.IsAuthenticated.ToString(), WebEventFieldType.Bool));
			fields.Add(new WebEventFieldData("UserAuthenticationType", this.RequestInformation.Principal.Identity.AuthenticationType, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("RequestThreadAccountName", this.RequestInformation.ThreadAccountName, WebEventFieldType.String));
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x000A1440 File Offset: 0x000A0440
		internal override void FormatToString(WebEventFormatter formatter, bool includeAppInfo)
		{
			base.FormatToString(formatter, includeAppInfo);
			formatter.AppendLine(string.Empty);
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_request_information"));
			formatter.IndentationLevel++;
			this.RequestInformation.FormatToString(formatter);
			formatter.IndentationLevel--;
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x000A1498 File Offset: 0x000A0498
		protected internal override void IncrementPerfCounters()
		{
			base.IncrementPerfCounters();
			PerfCounters.IncrementCounter(AppPerfCounter.EVENTS_WEB_REQ);
		}

		// Token: 0x04001D65 RID: 7525
		private WebRequestInformation _requestInfo;
	}
}
