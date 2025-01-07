using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	public class PanelContainerDesigner : ContainerControlDesigner
	{
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

		public override string FrameCaption
		{
			get
			{
				return ((Panel)base.Component).GroupingText;
			}
		}

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

		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(Panel));
			base.Initialize(component);
		}

		private const string PanelWithCaptionDesignTimeHtml = "<div style=\"{0}{2}{3}{4}{6}{10}\" class=\"{11}\">\r\n    <fieldset>\r\n        <legend>{5}</legend>\r\n        <div {7}=0></div>\r\n    </fieldset>\r\n</div>";

		private const string PanelNoCaptionDesignTimeHtml = "<div style=\"{0}{2}{3}{4}{6}{10}\" class=\"{11}\" {7}=0></div>";
	}
}
