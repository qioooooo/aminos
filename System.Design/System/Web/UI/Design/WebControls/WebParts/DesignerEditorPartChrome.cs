using System;
using System.Collections;
using System.Collections.Specialized;
using System.Design;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal class DesignerEditorPartChrome : EditorPartChrome
	{
		public DesignerEditorPartChrome(EditorZone zone)
			: base(zone)
		{
		}

		public ViewRendering GetViewRendering(Control control)
		{
			EditorPart editorPart = control as EditorPart;
			if (editorPart == null)
			{
				string text = ControlDesigner.CreateErrorDesignTimeHtml(SR.GetString("EditorZoneDesigner_OnlyEditorParts"), null, control);
				return new ViewRendering(text, new DesignerRegionCollection());
			}
			DesignerRegionCollection designerRegionCollection;
			string text2;
			try
			{
				IDictionary dictionary = new HybridDictionary(1);
				dictionary["Zone"] = base.Zone;
				((IControlDesignerAccessor)editorPart).SetDesignModeState(dictionary);
				this._partViewRendering = ControlDesigner.GetViewRendering(editorPart);
				designerRegionCollection = this._partViewRendering.Regions;
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				this.RenderEditorPart(new DesignTimeHtmlTextWriter(stringWriter), (EditorPart)PartDesigner.GetViewControl(editorPart));
				text2 = stringWriter.ToString();
			}
			catch (Exception ex)
			{
				text2 = ControlDesigner.CreateErrorDesignTimeHtml(SR.GetString("ControlDesigner_UnhandledException"), ex, control);
				designerRegionCollection = new DesignerRegionCollection();
			}
			return new ViewRendering(text2, designerRegionCollection);
		}

		protected override void RenderPartContents(HtmlTextWriter writer, EditorPart editorPart)
		{
			writer.Write(this._partViewRendering.Content);
		}

		private ViewRendering _partViewRendering;
	}
}
