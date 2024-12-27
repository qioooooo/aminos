using System;
using System.Collections.Generic;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002F3 RID: 755
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebAuthenticationSuccessAuditEvent : WebSuccessAuditEvent
	{
		// Token: 0x060025C7 RID: 9671 RVA: 0x000A1FD1 File Offset: 0x000A0FD1
		private void Init(string name)
		{
			this._nameToAuthenticate = name;
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x000A1FDA File Offset: 0x000A0FDA
		protected internal WebAuthenticationSuccessAuditEvent(string message, object eventSource, int eventCode, string nameToAuthenticate)
			: base(message, eventSource, eventCode)
		{
			this.Init(nameToAuthenticate);
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x000A1FED File Offset: 0x000A0FED
		protected internal WebAuthenticationSuccessAuditEvent(string message, object eventSource, int eventCode, int eventDetailCode, string nameToAuthenticate)
			: base(message, eventSource, eventCode, eventDetailCode)
		{
			this.Init(nameToAuthenticate);
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x000A2002 File Offset: 0x000A1002
		internal WebAuthenticationSuccessAuditEvent()
		{
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x060025CB RID: 9675 RVA: 0x000A200A File Offset: 0x000A100A
		public string NameToAuthenticate
		{
			get
			{
				return this._nameToAuthenticate;
			}
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x000A2012 File Offset: 0x000A1012
		internal override void GenerateFieldsForMarshal(List<WebEventFieldData> fields)
		{
			base.GenerateFieldsForMarshal(fields);
			fields.Add(new WebEventFieldData("NameToAuthenticate", this.NameToAuthenticate, WebEventFieldType.String));
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000A2032 File Offset: 0x000A1032
		internal override void FormatToString(WebEventFormatter formatter, bool includeAppInfo)
		{
			base.FormatToString(formatter, includeAppInfo);
			formatter.AppendLine(string.Empty);
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_name_to_authenticate", this._nameToAuthenticate));
		}

		// Token: 0x04001D6E RID: 7534
		private string _nameToAuthenticate;
	}
}
