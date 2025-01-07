using System;
using System.Design;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal class DesignerWebPartChrome : WebPartChrome
	{
		public DesignerWebPartChrome(WebPartZoneBase zone)
			: base(zone, null)
		{
		}

		public ViewRendering GetViewRendering(Control control)
		{
			DesignerRegionCollection designerRegionCollection;
			string text;
			try
			{
				this._partViewRendering = ControlDesigner.GetViewRendering(control);
				designerRegionCollection = this._partViewRendering.Regions;
				WebPart webPart = control as WebPart;
				if (webPart == null)
				{
					webPart = new DesignerGenericWebPart(PartDesigner.GetViewControl(control));
				}
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				this.RenderWebPart(new DesignTimeHtmlTextWriter(stringWriter), (WebPart)PartDesigner.GetViewControl(webPart));
				text = stringWriter.ToString();
			}
			catch (Exception ex)
			{
				text = ControlDesigner.CreateErrorDesignTimeHtml(SR.GetString("ControlDesigner_UnhandledException"), ex, control);
				designerRegionCollection = new DesignerRegionCollection();
			}
			StringWriter stringWriter2 = new StringWriter(CultureInfo.InvariantCulture);
			DesignTimeHtmlTextWriter designTimeHtmlTextWriter = new DesignTimeHtmlTextWriter(stringWriter2);
			bool flag = base.Zone.LayoutOrientation == Orientation.Horizontal;
			if (flag)
			{
				designTimeHtmlTextWriter.AddStyleAttribute("display", "inline-block");
				designTimeHtmlTextWriter.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
				designTimeHtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Span);
			}
			designTimeHtmlTextWriter.Write(text);
			if (flag)
			{
				designTimeHtmlTextWriter.RenderEndTag();
			}
			return new ViewRendering(stringWriter2.ToString(), designerRegionCollection);
		}

		protected override void RenderPartContents(HtmlTextWriter writer, WebPart webPart)
		{
			writer.Write(this._partViewRendering.Content);
		}

		private ViewRendering _partViewRendering;
	}
}
