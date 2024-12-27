using System;
using System.Collections.Generic;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002EE RID: 750
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebAuditEvent : WebManagementEvent
	{
		// Token: 0x060025AA RID: 9642 RVA: 0x000A1BB5 File Offset: 0x000A0BB5
		internal override void PreProcessEventInit()
		{
			base.PreProcessEventInit();
			this.InitRequestInformation();
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x000A1BC3 File Offset: 0x000A0BC3
		protected internal WebAuditEvent(string message, object eventSource, int eventCode)
			: base(message, eventSource, eventCode)
		{
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x000A1BCE File Offset: 0x000A0BCE
		protected internal WebAuditEvent(string message, object eventSource, int eventCode, int eventDetailCode)
			: base(message, eventSource, eventCode, eventDetailCode)
		{
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x000A1BDB File Offset: 0x000A0BDB
		internal WebAuditEvent()
		{
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x000A1BE3 File Offset: 0x000A0BE3
		private void InitRequestInformation()
		{
			if (this._requestInfo == null)
			{
				this._requestInfo = new WebRequestInformation();
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x060025AF RID: 9647 RVA: 0x000A1BF8 File Offset: 0x000A0BF8
		public WebRequestInformation RequestInformation
		{
			get
			{
				this.InitRequestInformation();
				return this._requestInfo;
			}
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x000A1C08 File Offset: 0x000A0C08
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

		// Token: 0x060025B1 RID: 9649 RVA: 0x000A1D08 File Offset: 0x000A0D08
		internal override void FormatToString(WebEventFormatter formatter, bool includeAppInfo)
		{
			base.FormatToString(formatter, includeAppInfo);
			formatter.AppendLine(string.Empty);
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_request_information"));
			formatter.IndentationLevel++;
			this.RequestInformation.FormatToString(formatter);
			formatter.IndentationLevel--;
		}

		// Token: 0x04001D6B RID: 7531
		private WebRequestInformation _requestInfo;
	}
}
