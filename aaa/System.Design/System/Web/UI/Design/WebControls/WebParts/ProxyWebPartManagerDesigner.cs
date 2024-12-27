using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	// Token: 0x02000542 RID: 1346
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ProxyWebPartManagerDesigner : ControlDesigner
	{
		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06002F6D RID: 12141 RVA: 0x0010E68F File Offset: 0x0010D68F
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x0010E692 File Offset: 0x0010D692
		public override string GetDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml();
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x0010E69A File Offset: 0x0010D69A
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(ProxyWebPartManager));
			base.Initialize(component);
		}
	}
}
