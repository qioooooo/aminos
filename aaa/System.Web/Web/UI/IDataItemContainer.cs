using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000409 RID: 1033
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IDataItemContainer : INamingContainer
	{
		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x0600329B RID: 12955
		object DataItem { get; }

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x0600329C RID: 12956
		int DataItemIndex { get; }

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x0600329D RID: 12957
		int DisplayIndex { get; }
	}
}
