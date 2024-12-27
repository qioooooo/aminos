using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004B6 RID: 1206
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class AccessDataSourceView : SqlDataSourceView
	{
		// Token: 0x06003929 RID: 14633 RVA: 0x000F287B File Offset: 0x000F187B
		public AccessDataSourceView(AccessDataSource owner, string name, HttpContext context)
			: base(owner, name, context)
		{
			this._owner = owner;
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x000F2890 File Offset: 0x000F1890
		protected internal override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
		{
			if (string.IsNullOrEmpty(this._owner.DataFile))
			{
				throw new InvalidOperationException(SR.GetString("AccessDataSourceView_SelectRequiresDataFile", new object[] { this._owner.ID }));
			}
			return base.ExecuteSelect(arguments);
		}

		// Token: 0x0400260D RID: 9741
		private AccessDataSource _owner;
	}
}
