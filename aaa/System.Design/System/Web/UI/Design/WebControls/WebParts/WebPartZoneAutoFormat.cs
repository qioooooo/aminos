using System;
using System.Data;
using System.Design;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	// Token: 0x02000545 RID: 1349
	internal sealed class WebPartZoneAutoFormat : BaseAutoFormat
	{
		// Token: 0x06002F77 RID: 12151 RVA: 0x0010E708 File Offset: 0x0010D708
		public WebPartZoneAutoFormat(DataRow schemeData)
			: base(schemeData)
		{
			base.Style.Width = 250;
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x0010E728 File Offset: 0x0010D728
		public override Control GetPreviewControl(Control runtimeControl)
		{
			WebPartZone webPartZone = (WebPartZone)base.GetPreviewControl(runtimeControl);
			if (webPartZone != null && webPartZone.WebParts.Count == 0)
			{
				webPartZone.ZoneTemplate = new WebPartZoneAutoFormat.AutoFormatTemplate();
			}
			return webPartZone;
		}

		// Token: 0x02000546 RID: 1350
		private sealed class AutoFormatTemplate : ITemplate
		{
			// Token: 0x06002F79 RID: 12153 RVA: 0x0010E75E File Offset: 0x0010D75E
			public void InstantiateIn(Control container)
			{
				container.Controls.Add(new WebPartZoneAutoFormat.AutoFormatTemplate.SampleWebPart());
			}

			// Token: 0x02000547 RID: 1351
			private sealed class SampleWebPart : WebPart
			{
				// Token: 0x06002F7B RID: 12155 RVA: 0x0010E778 File Offset: 0x0010D778
				public SampleWebPart()
				{
					this.Title = SR.GetString("WebPartZoneAutoFormat_SampleWebPartTitle");
					this.ID = "SampleWebPart";
				}

				// Token: 0x06002F7C RID: 12156 RVA: 0x0010E79B File Offset: 0x0010D79B
				protected override void RenderContents(HtmlTextWriter writer)
				{
					writer.Write(SR.GetString("WebPartZoneAutoFormat_SampleWebPartContents"));
				}
			}
		}
	}
}
