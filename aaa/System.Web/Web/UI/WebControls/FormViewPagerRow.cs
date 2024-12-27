using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000592 RID: 1426
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FormViewPagerRow : FormViewRow, INonBindingContainer, INamingContainer
	{
		// Token: 0x060045F5 RID: 17909 RVA: 0x0011EB0D File Offset: 0x0011DB0D
		public FormViewPagerRow(int rowIndex, DataControlRowType rowType, DataControlRowState rowState)
			: base(rowIndex, rowType, rowState)
		{
		}
	}
}
