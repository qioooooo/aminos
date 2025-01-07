using System;
using System.Data;
using System.Design;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	internal sealed class WebPartZoneAutoFormat : BaseAutoFormat
	{
		public WebPartZoneAutoFormat(DataRow schemeData)
			: base(schemeData)
		{
			base.Style.Width = 250;
		}

		public override Control GetPreviewControl(Control runtimeControl)
		{
			WebPartZone webPartZone = (WebPartZone)base.GetPreviewControl(runtimeControl);
			if (webPartZone != null && webPartZone.WebParts.Count == 0)
			{
				webPartZone.ZoneTemplate = new WebPartZoneAutoFormat.AutoFormatTemplate();
			}
			return webPartZone;
		}

		private sealed class AutoFormatTemplate : ITemplate
		{
			public void InstantiateIn(Control container)
			{
				container.Controls.Add(new WebPartZoneAutoFormat.AutoFormatTemplate.SampleWebPart());
			}

			private sealed class SampleWebPart : WebPart
			{
				public SampleWebPart()
				{
					this.Title = SR.GetString("WebPartZoneAutoFormat_SampleWebPartTitle");
					this.ID = "SampleWebPart";
				}

				protected override void RenderContents(HtmlTextWriter writer)
				{
					writer.Write(SR.GetString("WebPartZoneAutoFormat_SampleWebPartContents"));
				}
			}
		}
	}
}
