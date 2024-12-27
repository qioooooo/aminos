using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003DC RID: 988
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IDataSource
	{
		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06003001 RID: 12289
		// (remove) Token: 0x06003002 RID: 12290
		event EventHandler DataSourceChanged;

		// Token: 0x06003003 RID: 12291
		DataSourceView GetView(string viewName);

		// Token: 0x06003004 RID: 12292
		ICollection GetViewNames();
	}
}
