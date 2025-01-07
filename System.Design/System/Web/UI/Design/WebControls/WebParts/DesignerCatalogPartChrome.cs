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
	internal class DesignerCatalogPartChrome : CatalogPartChrome
	{
		public DesignerCatalogPartChrome(CatalogZone zone)
			: base(zone)
		{
		}

		public ViewRendering GetViewRendering(Control control)
		{
			CatalogPart catalogPart = control as CatalogPart;
			if (catalogPart == null)
			{
				string text = ControlDesigner.CreateErrorDesignTimeHtml(SR.GetString("CatalogZoneDesigner_OnlyCatalogParts"), null, control);
				return new ViewRendering(text, new DesignerRegionCollection());
			}
			DesignerRegionCollection designerRegionCollection;
			string text2;
			try
			{
				IDictionary dictionary = new HybridDictionary(1);
				dictionary["Zone"] = base.Zone;
				((IControlDesignerAccessor)catalogPart).SetDesignModeState(dictionary);
				this._partViewRendering = ControlDesigner.GetViewRendering(catalogPart);
				designerRegionCollection = this._partViewRendering.Regions;
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				this.RenderCatalogPart(new DesignTimeHtmlTextWriter(stringWriter), (CatalogPart)PartDesigner.GetViewControl(catalogPart));
				text2 = stringWriter.ToString();
			}
			catch (Exception ex)
			{
				text2 = ControlDesigner.CreateErrorDesignTimeHtml(SR.GetString("ControlDesigner_UnhandledException"), ex, control);
				designerRegionCollection = new DesignerRegionCollection();
			}
			return new ViewRendering(text2, designerRegionCollection);
		}

		protected override void RenderPartContents(HtmlTextWriter writer, CatalogPart catalogPart)
		{
			writer.Write(this._partViewRendering.Content);
		}

		private ViewRendering _partViewRendering;
	}
}
