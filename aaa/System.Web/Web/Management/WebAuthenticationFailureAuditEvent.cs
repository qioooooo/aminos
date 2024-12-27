using System;
using System.Collections.Generic;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002F0 RID: 752
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebAuthenticationFailureAuditEvent : WebFailureAuditEvent
	{
		// Token: 0x060025B6 RID: 9654 RVA: 0x000A1D96 File Offset: 0x000A0D96
		private void Init(string name)
		{
			this._nameToAuthenticate = name;
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x000A1D9F File Offset: 0x000A0D9F
		protected internal WebAuthenticationFailureAuditEvent(string message, object eventSource, int eventCode, string nameToAuthenticate)
			: base(message, eventSource, eventCode)
		{
			this.Init(nameToAuthenticate);
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x000A1DB2 File Offset: 0x000A0DB2
		protected internal WebAuthenticationFailureAuditEvent(string message, object eventSource, int eventCode, int eventDetailCode, string nameToAuthenticate)
			: base(message, eventSource, eventCode, eventDetailCode)
		{
			this.Init(nameToAuthenticate);
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x000A1DC7 File Offset: 0x000A0DC7
		internal WebAuthenticationFailureAuditEvent()
		{
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x060025BA RID: 9658 RVA: 0x000A1DCF File Offset: 0x000A0DCF
		public string NameToAuthenticate
		{
			get
			{
				return this._nameToAuthenticate;
			}
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x000A1DD7 File Offset: 0x000A0DD7
		internal override void GenerateFieldsForMarshal(List<WebEventFieldData> fields)
		{
			base.GenerateFieldsForMarshal(fields);
			fields.Add(new WebEventFieldData("NameToAuthenticate", this.NameToAuthenticate, WebEventFieldType.String));
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x000A1DF7 File Offset: 0x000A0DF7
		internal override void FormatToString(WebEventFormatter formatter, bool includeAppInfo)
		{
			base.FormatToString(formatter, includeAppInfo);
			formatter.AppendLine(string.Empty);
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_name_to_authenticate", this._nameToAuthenticate));
		}

		// Token: 0x04001D6C RID: 7532
		private string _nameToAuthenticate;
	}
}
