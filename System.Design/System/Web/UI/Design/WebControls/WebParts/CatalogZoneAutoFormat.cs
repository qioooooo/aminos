using System;
using System.Data;
using System.Design;
using System.Globalization;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	internal sealed class CatalogZoneAutoFormat : BaseAutoFormat
	{
		public CatalogZoneAutoFormat(DataRow schemeData)
			: base(schemeData)
		{
			base.Style.Width = 300;
		}

		public override Control GetPreviewControl(Control runtimeControl)
		{
			CatalogZone catalogZone = (CatalogZone)base.GetPreviewControl(runtimeControl);
			if (catalogZone != null && catalogZone.CatalogParts.Count == 0)
			{
				catalogZone.ZoneTemplate = new CatalogZoneAutoFormat.AutoFormatTemplate();
			}
			catalogZone.ID = "AutoFormatPreviewControl";
			return catalogZone;
		}

		internal const string PreviewControlID = "AutoFormatPreviewControl";

		private sealed class AutoFormatTemplate : ITemplate
		{
			public void InstantiateIn(Control container)
			{
				DeclarativeCatalogPart declarativeCatalogPart = new DeclarativeCatalogPart();
				declarativeCatalogPart.WebPartsTemplate = new CatalogZoneAutoFormat.AutoFormatTemplate.SampleCatalogPartTemplate();
				declarativeCatalogPart.ID = "SampleCatalogPart";
				container.Controls.Add(declarativeCatalogPart);
			}

			private sealed class SampleCatalogPartTemplate : ITemplate
			{
				public void InstantiateIn(Control container)
				{
					CatalogZoneAutoFormat.AutoFormatTemplate.SampleCatalogPartTemplate.SampleWebPart sampleWebPart = new CatalogZoneAutoFormat.AutoFormatTemplate.SampleCatalogPartTemplate.SampleWebPart();
					sampleWebPart.ID = "SampleWebPart1";
					sampleWebPart.Title = string.Format(CultureInfo.CurrentCulture, SR.GetString("CatalogZone_SampleWebPartTitle"), new object[] { "1" });
					container.Controls.Add(sampleWebPart);
					sampleWebPart = new CatalogZoneAutoFormat.AutoFormatTemplate.SampleCatalogPartTemplate.SampleWebPart();
					sampleWebPart.ID = "SampleWebPart2";
					sampleWebPart.Title = string.Format(CultureInfo.CurrentCulture, SR.GetString("CatalogZone_SampleWebPartTitle"), new object[] { "2" });
					container.Controls.Add(sampleWebPart);
					sampleWebPart = new CatalogZoneAutoFormat.AutoFormatTemplate.SampleCatalogPartTemplate.SampleWebPart();
					sampleWebPart.ID = "SampleWebPart3";
					sampleWebPart.Title = string.Format(CultureInfo.CurrentCulture, SR.GetString("CatalogZone_SampleWebPartTitle"), new object[] { "3" });
					container.Controls.Add(sampleWebPart);
				}

				private sealed class SampleWebPart : WebPart
				{
				}
			}
		}
	}
}
