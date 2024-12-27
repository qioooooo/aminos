using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002E7 RID: 743
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	public class WebManagementEvent : WebBaseEvent
	{
		// Token: 0x06002570 RID: 9584 RVA: 0x000A1089 File Offset: 0x000A0089
		protected internal WebManagementEvent(string message, object eventSource, int eventCode)
			: base(message, eventSource, eventCode)
		{
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x000A1094 File Offset: 0x000A0094
		protected internal WebManagementEvent(string message, object eventSource, int eventCode, int eventDetailCode)
			: base(message, eventSource, eventCode, eventDetailCode)
		{
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x000A10A1 File Offset: 0x000A00A1
		internal WebManagementEvent()
		{
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06002573 RID: 9587 RVA: 0x000A10A9 File Offset: 0x000A00A9
		public WebProcessInformation ProcessInformation
		{
			get
			{
				return WebManagementEvent.s_processInfo;
			}
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x000A10B0 File Offset: 0x000A00B0
		internal override void GenerateFieldsForMarshal(List<WebEventFieldData> fields)
		{
			base.GenerateFieldsForMarshal(fields);
			fields.Add(new WebEventFieldData("AccountName", this.ProcessInformation.AccountName, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("ProcessName", this.ProcessInformation.ProcessName, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("ProcessID", this.ProcessInformation.ProcessID.ToString(CultureInfo.InstalledUICulture), WebEventFieldType.Int));
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x000A1128 File Offset: 0x000A0128
		internal override void FormatToString(WebEventFormatter formatter, bool includeAppInfo)
		{
			base.FormatToString(formatter, includeAppInfo);
			formatter.AppendLine(string.Empty);
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_process_information"));
			formatter.IndentationLevel++;
			this.ProcessInformation.FormatToString(formatter);
			formatter.IndentationLevel--;
		}

		// Token: 0x04001D63 RID: 7523
		private static WebProcessInformation s_processInfo = new WebProcessInformation();
	}
}
