using System;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class AdRotatorDesigner : DataBoundControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			AdRotator adRotator = (AdRotator)base.ViewControl;
			StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
			DesignTimeHtmlTextWriter designTimeHtmlTextWriter = new DesignTimeHtmlTextWriter(stringWriter);
			HyperLink hyperLink = new HyperLink();
			hyperLink.ID = adRotator.ID;
			hyperLink.NavigateUrl = "";
			hyperLink.Target = adRotator.Target;
			hyperLink.AccessKey = adRotator.AccessKey;
			hyperLink.Enabled = adRotator.Enabled;
			hyperLink.TabIndex = adRotator.TabIndex;
			hyperLink.Style.Value = adRotator.Style.Value;
			hyperLink.RenderBeginTag(designTimeHtmlTextWriter);
			Image image = new Image();
			image.ApplyStyle(adRotator.ControlStyle);
			image.ImageUrl = "";
			image.AlternateText = adRotator.ID;
			image.ToolTip = adRotator.ToolTip;
			image.RenderControl(designTimeHtmlTextWriter);
			hyperLink.RenderEndTag(designTimeHtmlTextWriter);
			return stringWriter.ToString();
		}
	}
}
