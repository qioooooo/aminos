using System;
using System.Globalization;
using System.Security.Permissions;
using System.Text;

namespace System.Web.Management
{
	// Token: 0x020002F4 RID: 756
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebProcessInformation
	{
		// Token: 0x060025CE RID: 9678 RVA: 0x000A2060 File Offset: 0x000A1060
		internal WebProcessInformation()
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			if (UnsafeNativeMethods.GetModuleFileName(IntPtr.Zero, stringBuilder, 256) == 0)
			{
				this._processName = string.Empty;
			}
			else
			{
				this._processName = stringBuilder.ToString();
				int num = this._processName.LastIndexOf('\\');
				if (num != -1)
				{
					this._processName = this._processName.Substring(num + 1);
				}
			}
			this._processId = SafeNativeMethods.GetCurrentProcessId();
			this._accountName = HttpRuntime.WpUserId;
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x060025CF RID: 9679 RVA: 0x000A20E5 File Offset: 0x000A10E5
		public int ProcessID
		{
			get
			{
				return this._processId;
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x060025D0 RID: 9680 RVA: 0x000A20ED File Offset: 0x000A10ED
		public string ProcessName
		{
			get
			{
				return this._processName;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x060025D1 RID: 9681 RVA: 0x000A20F5 File Offset: 0x000A10F5
		public string AccountName
		{
			get
			{
				if (this._accountName == null)
				{
					return string.Empty;
				}
				return this._accountName;
			}
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x000A210C File Offset: 0x000A110C
		public void FormatToString(WebEventFormatter formatter)
		{
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_process_id", this.ProcessID.ToString(CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_process_name", this.ProcessName));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_account_name", this.AccountName));
		}

		// Token: 0x04001D6F RID: 7535
		private int _processId;

		// Token: 0x04001D70 RID: 7536
		private string _processName;

		// Token: 0x04001D71 RID: 7537
		private string _accountName;
	}
}
