using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002EC RID: 748
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebErrorEvent : WebBaseErrorEvent
	{
		// Token: 0x06002592 RID: 9618 RVA: 0x000A161B File Offset: 0x000A061B
		private void Init(Exception e)
		{
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000A161D File Offset: 0x000A061D
		internal override void PreProcessEventInit()
		{
			base.PreProcessEventInit();
			this.InitRequestInformation();
			this.InitThreadInformation();
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x000A1631 File Offset: 0x000A0631
		protected internal WebErrorEvent(string message, object eventSource, int eventCode, Exception exception)
			: base(message, eventSource, eventCode, exception)
		{
			this.Init(exception);
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x000A1646 File Offset: 0x000A0646
		protected internal WebErrorEvent(string message, object eventSource, int eventCode, int eventDetailCode, Exception exception)
			: base(message, eventSource, eventCode, eventDetailCode, exception)
		{
			this.Init(exception);
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x000A165D File Offset: 0x000A065D
		internal WebErrorEvent()
		{
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x000A1665 File Offset: 0x000A0665
		private void InitRequestInformation()
		{
			if (this._requestInfo == null)
			{
				this._requestInfo = new WebRequestInformation();
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06002598 RID: 9624 RVA: 0x000A167A File Offset: 0x000A067A
		public WebRequestInformation RequestInformation
		{
			get
			{
				this.InitRequestInformation();
				return this._requestInfo;
			}
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x000A1688 File Offset: 0x000A0688
		private void InitThreadInformation()
		{
			if (this._threadInfo == null)
			{
				this._threadInfo = new WebThreadInformation(base.ErrorException);
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x0600259A RID: 9626 RVA: 0x000A16A3 File Offset: 0x000A06A3
		public WebThreadInformation ThreadInformation
		{
			get
			{
				this.InitThreadInformation();
				return this._threadInfo;
			}
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x000A16B4 File Offset: 0x000A06B4
		internal override void FormatToString(WebEventFormatter formatter, bool includeAppInfo)
		{
			base.FormatToString(formatter, includeAppInfo);
			formatter.AppendLine(string.Empty);
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_request_information"));
			formatter.IndentationLevel++;
			this.RequestInformation.FormatToString(formatter);
			formatter.IndentationLevel--;
			formatter.AppendLine(string.Empty);
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_thread_information"));
			formatter.IndentationLevel++;
			this.ThreadInformation.FormatToString(formatter);
			formatter.IndentationLevel--;
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x000A1750 File Offset: 0x000A0750
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
			fields.Add(new WebEventFieldData("ThreadID", this.ThreadInformation.ThreadID.ToString(CultureInfo.InstalledUICulture), WebEventFieldType.Int));
			fields.Add(new WebEventFieldData("ThreadAccountName", this.ThreadInformation.ThreadAccountName, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("StackTrace", this.ThreadInformation.StackTrace, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("IsImpersonating", this.ThreadInformation.IsImpersonating.ToString(), WebEventFieldType.Bool));
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x000A18D3 File Offset: 0x000A08D3
		protected internal override void IncrementPerfCounters()
		{
			base.IncrementPerfCounters();
			PerfCounters.IncrementCounter(AppPerfCounter.EVENTS_HTTP_INFRA_ERROR);
			PerfCounters.IncrementGlobalCounter(GlobalPerfCounter.GLOBAL_EVENTS_HTTP_INFRA_ERROR);
		}

		// Token: 0x04001D67 RID: 7527
		private WebRequestInformation _requestInfo;

		// Token: 0x04001D68 RID: 7528
		private WebThreadInformation _threadInfo;
	}
}
