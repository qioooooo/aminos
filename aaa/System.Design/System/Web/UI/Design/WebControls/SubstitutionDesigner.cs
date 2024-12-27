using System;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004E8 RID: 1256
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class SubstitutionDesigner : ControlDesigner
	{
		// Token: 0x06002D0B RID: 11531 RVA: 0x000FE72E File Offset: 0x000FD72E
		public override string GetDesignTimeHtml()
		{
			return this.GetEmptyDesignTimeHtml();
		}
	}
}
