using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	// Token: 0x02000544 RID: 1348
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class WebPartManagerDesigner : ControlDesigner
	{
		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06002F73 RID: 12147 RVA: 0x0010E6DC File Offset: 0x0010D6DC
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x0010E6DF File Offset: 0x0010D6DF
		public override string GetDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml();
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x0010E6E7 File Offset: 0x0010D6E7
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(WebPartManager));
			base.Initialize(component);
		}
	}
}
