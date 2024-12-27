using System;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000411 RID: 1041
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IPostBackDataHandler
	{
		// Token: 0x060032B0 RID: 12976
		bool LoadPostData(string postDataKey, NameValueCollection postCollection);

		// Token: 0x060032B1 RID: 12977
		void RaisePostDataChangedEvent();
	}
}
