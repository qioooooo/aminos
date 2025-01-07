using System;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class BaseValidatorDesigner : PreviewControlDesigner
	{
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
