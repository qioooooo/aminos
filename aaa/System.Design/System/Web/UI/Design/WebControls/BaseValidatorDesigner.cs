using System;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003F1 RID: 1009
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class BaseValidatorDesigner : PreviewControlDesigner
	{
		// Token: 0x06002546 RID: 9542 RVA: 0x000C8124 File Offset: 0x000C7124
		public override string GetDesignTimeHtml()
		{
			BaseValidator baseValidator = (BaseValidator)base.ViewControl;
			baseValidator.IsValid = false;
			string errorMessage = baseValidator.ErrorMessage;
			ValidatorDisplay display = baseValidator.Display;
			bool flag = display == ValidatorDisplay.None || (errorMessage.Trim().Length == 0 && baseValidator.Text.Trim().Length == 0);
			if (flag)
			{
				baseValidator.ErrorMessage = "[" + baseValidator.ID + "]";
				baseValidator.Display = ValidatorDisplay.Static;
			}
			string designTimeHtml = base.GetDesignTimeHtml();
			if (flag)
			{
				baseValidator.ErrorMessage = errorMessage;
				baseValidator.Display = display;
			}
			return designTimeHtml;
		}
	}
}
