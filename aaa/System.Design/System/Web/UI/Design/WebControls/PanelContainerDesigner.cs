using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200048C RID: 1164
	public class PanelContainerDesigner : ContainerControlDesigner
	{
		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06002A46 RID: 10822 RVA: 0x000E88E7 File Offset: 0x000E78E7
		internal override string DesignTimeHtml
		{
			get
			{
				if (this.FrameCaption.Length > 0)
				{
					return "<div style=\"{0}{2}{3}{4}{6}{10}\" class=\"{11}\">\r\n    <fieldset>\r\n        <legend>{5}</legend>\r\n        <div {7}=0></div>\r\n    </fieldset>\r\n</div>";
				}
				return "<div style=\"{0}{2}{3}{4}{6}{10}\" class=\"{11}\" {7}=0></div>";
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06002A47 RID: 10823 RVA: 0x000E8902 File Offset: 0x000E7902
		public override string FrameCaption
		{
			get
			{
				return ((Panel)base.Component).GroupingText;
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06002A48 RID: 10824 RVA: 0x000E8914 File Offset: 0x000E7914
		public override Style FrameStyle
		{
			get
			{
				if (((Panel)base.Component).GroupingText.Length == 0)
				{
					return new Style();
				}
				return base.FrameStyle;
			}
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x000E893C File Offset: 0x000E793C
		protected override void AddDesignTimeCssAttributes(IDictionary styleAttributes)
		{
			Panel panel = (Panel)base.Component;
			switch (panel.Direction)
			{
			case ContentDirection.LeftToRight:
				styleAttributes["direction"] = "ltr";
				break;
			case ContentDirection.RightToLeft:
				styleAttributes["direction"] = "rtl";
				break;
			}
			string text = panel.BackImageUrl;
			if (text.Trim().Length > 0)
			{
				IUrlResolutionService urlResolutionService = (IUrlResolutionService)this.GetService(typeof(IUrlResolutionService));
				if (urlResolutionService != null)
				{
					text = urlResolutionService.ResolveClientUrl(text);
					styleAttributes["background-image"] = "url(" + text + ")";
				}
			}
			switch (panel.ScrollBars)
			{
			case ScrollBars.Horizontal:
				styleAttributes["overflow-x"] = "scroll";
				break;
			case ScrollBars.Vertical:
				styleAttributes["overflow-y"] = "scroll";
				break;
			case ScrollBars.Both:
				styleAttributes["overflow"] = "scroll";
				break;
			case ScrollBars.Auto:
				styleAttributes["overflow"] = "auto";
				break;
			}
			HorizontalAlign horizontalAlign = panel.HorizontalAlign;
			if (horizontalAlign != HorizontalAlign.NotSet)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(HorizontalAlign));
				styleAttributes["text-align"] = converter.ConvertToInvariantString(horizontalAlign).ToLowerInvariant();
			}
			if (!panel.Wrap)
			{
				styleAttributes["white-space"] = "nowrap";
			}
			base.AddDesignTimeCssAttributes(styleAttributes);
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06002A4A RID: 10826 RVA: 0x000E8AA3 File Offset: 0x000E7AA3
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x000E8AA6 File Offset: 0x000E7AA6
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(Panel));
			base.Initialize(component);
		}

		// Token: 0x04001CDC RID: 7388
		private const string PanelWithCaptionDesignTimeHtml = "<div style=\"{0}{2}{3}{4}{6}{10}\" class=\"{11}\">\r\n    <fieldset>\r\n        <legend>{5}</legend>\r\n        <div {7}=0></div>\r\n    </fieldset>\r\n</div>";

		// Token: 0x04001CDD RID: 7389
		private const string PanelNoCaptionDesignTimeHtml = "<div style=\"{0}{2}{3}{4}{6}{10}\" class=\"{11}\" {7}=0></div>";
	}
}
