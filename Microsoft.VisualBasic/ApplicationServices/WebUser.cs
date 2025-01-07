using System;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web;

namespace Microsoft.VisualBasic.ApplicationServices
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class WebUser : User
	{
		protected override IPrincipal InternalPrincipal
		{
			get
			{
				return HttpContext.Current.User;
			}
			set
			{
				HttpContext.Current.User = value;
			}
		}
	}
}
