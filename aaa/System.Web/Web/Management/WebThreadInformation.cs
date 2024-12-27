using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Web.Management
{
	// Token: 0x020002F8 RID: 760
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebThreadInformation
	{
		// Token: 0x060025F0 RID: 9712 RVA: 0x000A2778 File Offset: 0x000A1778
		internal WebThreadInformation(Exception exception)
		{
			this._threadId = Thread.CurrentThread.ManagedThreadId;
			this._accountName = WindowsIdentity.GetCurrent().Name;
			if (exception != null)
			{
				this._stackTrace = new StackTrace(exception, true).ToString();
			}
			else
			{
				this._stackTrace = string.Empty;
			}
			this._isImpersonating = exception.Data.Contains("ASPIMPERSONATING");
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x060025F1 RID: 9713 RVA: 0x000A27E3 File Offset: 0x000A17E3
		public int ThreadID
		{
			get
			{
				return this._threadId;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x060025F2 RID: 9714 RVA: 0x000A27EB File Offset: 0x000A17EB
		public string ThreadAccountName
		{
			get
			{
				return this._accountName;
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x060025F3 RID: 9715 RVA: 0x000A27F3 File Offset: 0x000A17F3
		public string StackTrace
		{
			get
			{
				return this._stackTrace;
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x060025F4 RID: 9716 RVA: 0x000A27FB File Offset: 0x000A17FB
		public bool IsImpersonating
		{
			get
			{
				return this._isImpersonating;
			}
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x000A2804 File Offset: 0x000A1804
		public void FormatToString(WebEventFormatter formatter)
		{
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_thread_id", this.ThreadID.ToString(CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_thread_account_name", this.ThreadAccountName));
			if (this.IsImpersonating)
			{
				formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_is_impersonating"));
			}
			else
			{
				formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_is_not_impersonating"));
			}
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_stack_trace", this.StackTrace));
		}

		// Token: 0x04001D89 RID: 7561
		internal const string IsImpersonatingKey = "ASPIMPERSONATING";

		// Token: 0x04001D8A RID: 7562
		private int _threadId;

		// Token: 0x04001D8B RID: 7563
		private string _accountName;

		// Token: 0x04001D8C RID: 7564
		private string _stackTrace;

		// Token: 0x04001D8D RID: 7565
		private bool _isImpersonating;
	}
}
