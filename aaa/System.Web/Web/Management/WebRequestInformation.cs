using System;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Web.Management
{
	// Token: 0x020002F6 RID: 758
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebRequestInformation
	{
		// Token: 0x060025DC RID: 9692 RVA: 0x000A22A0 File Offset: 0x000A12A0
		internal WebRequestInformation()
		{
			InternalSecurityPermissions.ControlPrincipal.Assert();
			HttpContext httpContext = HttpContext.Current;
			HttpRequest httpRequest = null;
			if (httpContext != null)
			{
				bool hideRequestResponse = httpContext.HideRequestResponse;
				httpContext.HideRequestResponse = false;
				httpRequest = httpContext.Request;
				httpContext.HideRequestResponse = hideRequestResponse;
				this._iprincipal = httpContext.User;
			}
			else
			{
				this._iprincipal = null;
			}
			if (httpRequest == null)
			{
				this._requestUrl = string.Empty;
				this._requestPath = string.Empty;
				this._userHostAddress = string.Empty;
			}
			else
			{
				this._requestUrl = httpRequest.UrlInternal;
				this._requestPath = httpRequest.Path;
				this._userHostAddress = httpRequest.UserHostAddress;
			}
			this._accountName = WindowsIdentity.GetCurrent().Name;
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x060025DD RID: 9693 RVA: 0x000A2353 File Offset: 0x000A1353
		public string RequestUrl
		{
			get
			{
				return this._requestUrl;
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x060025DE RID: 9694 RVA: 0x000A235B File Offset: 0x000A135B
		public string RequestPath
		{
			get
			{
				return this._requestPath;
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x060025DF RID: 9695 RVA: 0x000A2363 File Offset: 0x000A1363
		public IPrincipal Principal
		{
			get
			{
				return this._iprincipal;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x060025E0 RID: 9696 RVA: 0x000A236B File Offset: 0x000A136B
		public string UserHostAddress
		{
			get
			{
				return this._userHostAddress;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x060025E1 RID: 9697 RVA: 0x000A2373 File Offset: 0x000A1373
		public string ThreadAccountName
		{
			get
			{
				return this._accountName;
			}
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x000A237C File Offset: 0x000A137C
		public void FormatToString(WebEventFormatter formatter)
		{
			string text;
			string text2;
			bool flag;
			if (this.Principal == null)
			{
				text = string.Empty;
				text2 = string.Empty;
				flag = false;
			}
			else
			{
				IIdentity identity = this.Principal.Identity;
				text = identity.Name;
				flag = identity.IsAuthenticated;
				text2 = identity.AuthenticationType;
			}
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_request_url", this.RequestUrl));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_request_path", this.RequestPath));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_user_host_address", this.UserHostAddress));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_user", text));
			if (flag)
			{
				formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_is_authenticated"));
			}
			else
			{
				formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_is_not_authenticated"));
			}
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_authentication_type", text2));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_thread_account_name", this.ThreadAccountName));
		}

		// Token: 0x04001D77 RID: 7543
		private string _requestUrl;

		// Token: 0x04001D78 RID: 7544
		private string _requestPath;

		// Token: 0x04001D79 RID: 7545
		private IPrincipal _iprincipal;

		// Token: 0x04001D7A RID: 7546
		private string _userHostAddress;

		// Token: 0x04001D7B RID: 7547
		private string _accountName;
	}
}
