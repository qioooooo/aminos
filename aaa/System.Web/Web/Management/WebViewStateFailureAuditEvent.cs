using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Web.UI;

namespace System.Web.Management
{
	// Token: 0x020002F1 RID: 753
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebViewStateFailureAuditEvent : WebFailureAuditEvent
	{
		// Token: 0x060025BD RID: 9661 RVA: 0x000A1E22 File Offset: 0x000A0E22
		protected internal WebViewStateFailureAuditEvent(string message, object eventSource, int eventCode, ViewStateException viewStateException)
			: base(message, eventSource, eventCode)
		{
			this._viewStateException = viewStateException;
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x000A1E35 File Offset: 0x000A0E35
		protected internal WebViewStateFailureAuditEvent(string message, object eventSource, int eventCode, int eventDetailCode, ViewStateException viewStateException)
			: base(message, eventSource, eventCode, eventDetailCode)
		{
			this._viewStateException = viewStateException;
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x000A1E4A File Offset: 0x000A0E4A
		internal WebViewStateFailureAuditEvent()
		{
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x060025C0 RID: 9664 RVA: 0x000A1E52 File Offset: 0x000A0E52
		public ViewStateException ViewStateException
		{
			get
			{
				return this._viewStateException;
			}
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x000A1E5C File Offset: 0x000A0E5C
		internal override void GenerateFieldsForMarshal(List<WebEventFieldData> fields)
		{
			base.GenerateFieldsForMarshal(fields);
			fields.Add(new WebEventFieldData("ViewStateExceptionMessage", this.ViewStateException.Message, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("RemoteAddress", this.ViewStateException.RemoteAddress, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("RemotePort", this.ViewStateException.RemotePort, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("UserAgent", this.ViewStateException.UserAgent, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("PersistedState", this.ViewStateException.PersistedState, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("Path", this.ViewStateException.Path, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("Referer", this.ViewStateException.Referer, WebEventFieldType.String));
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x000A1F34 File Offset: 0x000A0F34
		internal override void FormatToString(WebEventFormatter formatter, bool includeAppInfo)
		{
			base.FormatToString(formatter, includeAppInfo);
			formatter.AppendLine(string.Empty);
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_ViewStateException_information"));
			formatter.IndentationLevel++;
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_exception_message", this._viewStateException.Message));
			formatter.IndentationLevel--;
		}

		// Token: 0x04001D6D RID: 7533
		private ViewStateException _viewStateException;
	}
}
