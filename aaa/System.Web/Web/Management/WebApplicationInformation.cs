using System;
using System.Security.Permissions;
using System.Threading;

namespace System.Web.Management
{
	// Token: 0x020002F5 RID: 757
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebApplicationInformation
	{
		// Token: 0x060025D3 RID: 9683 RVA: 0x000A2168 File Offset: 0x000A1168
		internal WebApplicationInformation()
		{
			this._appDomain = Thread.GetDomain().FriendlyName;
			this._trustLevel = HttpRuntime.TrustLevel;
			this._appUrl = HttpRuntime.AppDomainAppVirtualPath;
			try
			{
				this._appPath = HttpRuntime.AppDomainAppPath;
			}
			catch
			{
				this._appPath = null;
			}
			this._machineName = this.GetMachineNameWithAssert();
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x060025D4 RID: 9684 RVA: 0x000A21D4 File Offset: 0x000A11D4
		public string ApplicationDomain
		{
			get
			{
				return this._appDomain;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x060025D5 RID: 9685 RVA: 0x000A21DC File Offset: 0x000A11DC
		public string TrustLevel
		{
			get
			{
				return this._trustLevel;
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x060025D6 RID: 9686 RVA: 0x000A21E4 File Offset: 0x000A11E4
		public string ApplicationVirtualPath
		{
			get
			{
				return this._appUrl;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x060025D7 RID: 9687 RVA: 0x000A21EC File Offset: 0x000A11EC
		public string ApplicationPath
		{
			get
			{
				return this._appPath;
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x060025D8 RID: 9688 RVA: 0x000A21F4 File Offset: 0x000A11F4
		public string MachineName
		{
			get
			{
				return this._machineName;
			}
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x000A21FC File Offset: 0x000A11FC
		public void FormatToString(WebEventFormatter formatter)
		{
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_application_domain", this.ApplicationDomain));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_trust_level", this.TrustLevel));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_application_virtual_path", this.ApplicationVirtualPath));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_application_path", this.ApplicationPath));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_machine_name", this.MachineName));
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x000A2277 File Offset: 0x000A1277
		[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
		private string GetMachineNameWithAssert()
		{
			return Environment.MachineName;
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x000A2280 File Offset: 0x000A1280
		public override string ToString()
		{
			WebEventFormatter webEventFormatter = new WebEventFormatter();
			this.FormatToString(webEventFormatter);
			return webEventFormatter.ToString();
		}

		// Token: 0x04001D72 RID: 7538
		private string _appDomain;

		// Token: 0x04001D73 RID: 7539
		private string _trustLevel;

		// Token: 0x04001D74 RID: 7540
		private string _appUrl;

		// Token: 0x04001D75 RID: 7541
		private string _appPath;

		// Token: 0x04001D76 RID: 7542
		private string _machineName;
	}
}
