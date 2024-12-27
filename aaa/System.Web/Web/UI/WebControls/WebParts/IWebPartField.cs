using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006CE RID: 1742
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IWebPartField
	{
		// Token: 0x17001608 RID: 5640
		// (get) Token: 0x060055A3 RID: 21923
		PropertyDescriptor Schema { get; }

		// Token: 0x060055A4 RID: 21924
		void GetFieldValue(FieldCallback callback);
	}
}
