using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000453 RID: 1107
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class HiddenFieldDesigner : ControlDesigner
	{
		// Token: 0x06002882 RID: 10370 RVA: 0x000DEDEA File Offset: 0x000DDDEA
		public override string GetDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml();
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x000DEDF2 File Offset: 0x000DDDF2
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(HiddenField));
			base.Initialize(component);
		}
	}
}
