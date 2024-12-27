using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002ED RID: 749
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebRequestErrorEvent : WebBaseErrorEvent
	{
		// Token: 0x0600259E RID: 9630 RVA: 0x000A18E9 File Offset: 0x000A08E9
		private void Init(Exception e)
		{
		}

		// Token: 0x0600259F RID: 9631 RVA: 0x000A18EB File Offset: 0x000A08EB
		internal override void PreProcessEventInit()
		{
			base.PreProcessEventInit();
			this.InitRequestInformation();
			this.InitThreadInformation();
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x000A18FF File Offset: 0x000A08FF
		protected internal WebRequestErrorEvent(string message, object eventSource, int eventCode, Exception exception)
			: base(message, eventSource, eventCode, exception)
		{
			this.Init(exception);
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x000A1914 File Offset: 0x000A0914
		protected internal WebRequestErrorEvent(string message, object eventSource, int eventCode, int eventDetailCode, Exception exception)
			: base(message, eventSource, eventCode, eventDetailCode, exception)
		{
			this.Init(exception);
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x000A192B File Offset: 0x000A092B
		internal WebRequestErrorEvent()
		{
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x000A1933 File Offset: 0x000A0933
		private void InitRequestInformation()
		{
			if (this._requestInfo == null)
			{
				this._requestInfo = new WebRequestInformation();
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x060025A4 RID: 9636 RVA: 0x000A1948 File Offset: 0x000A0948
		public WebRequestInformation RequestInformation
		{
			get
			{
				this.InitRequestInformation();
				return this._requestInfo;
			}
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x000A1956 File Offset: 0x000A0956
		private void InitThreadInformation()
		{
			if (this._threadInfo == null)
			{
				this._threadInfo = new WebThreadInformation(base.ErrorException);
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x060025A6 RID: 9638 RVA: 0x000A1971 File Offset: 0x000A0971
		public WebThreadInformation ThreadInformation
		{
			get
			{
				this.InitThreadInformation();
				return this._threadInfo;
			}
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x000A1980 File Offset: 0x000A0980
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

		// Token: 0x060025A8 RID: 9640 RVA: 0x000A1A1C File Offset: 0x000A0A1C
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

		// Token: 0x060025A9 RID: 9641 RVA: 0x000A1B9F File Offset: 0x000A0B9F
		protected internal override void IncrementPerfCounters()
		{
			base.IncrementPerfCounters();
			PerfCounters.IncrementCounter(AppPerfCounter.EVENTS_HTTP_REQ_ERROR);
			PerfCounters.IncrementGlobalCounter(GlobalPerfCounter.GLOBAL_EVENTS_HTTP_REQ_ERROR);
		}

		// Token: 0x04001D69 RID: 7529
		private WebRequestInformation _requestInfo;

		// Token: 0x04001D6A RID: 7530
		private WebThreadInformation _threadInfo;
	}
}
