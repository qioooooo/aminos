using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x0200049A RID: 1178
	[ControlBuilder(typeof(HtmlEmptyTagControlBuilder))]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlImage : HtmlControl
	{
		// Token: 0x060036FB RID: 14075 RVA: 0x000ED00A File Offset: 0x000EC00A
		public HtmlImage()
			: base("img")
		{
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x060036FC RID: 14076 RVA: 0x000ED018 File Offset: 0x000EC018
		// (set) Token: 0x060036FD RID: 14077 RVA: 0x000ED040 File Offset: 0x000EC040
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Appearance")]
		[Localizable(true)]
		public string Alt
		{
			get
			{
				string text = base.Attributes["alt"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["alt"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x060036FE RID: 14078 RVA: 0x000ED058 File Offset: 0x000EC058
		// (set) Token: 0x060036FF RID: 14079 RVA: 0x000ED080 File Offset: 0x000EC080
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Appearance")]
		public string Align
		{
			get
			{
				string text = base.Attributes["align"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["align"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06003700 RID: 14080 RVA: 0x000ED098 File Offset: 0x000EC098
		// (set) Token: 0x06003701 RID: 14081 RVA: 0x000ED0C6 File Offset: 0x000EC0C6
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Appearance")]
		[DefaultValue(0)]
		public int Border
		{
			get
			{
				string text = base.Attributes["border"];
				if (text == null)
				{
					return -1;
				}
				return int.Parse(text, CultureInfo.InvariantCulture);
			}
			set
			{
				base.Attributes["border"] = HtmlControl.MapIntegerAttributeToString(value);
			}
		}

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06003702 RID: 14082 RVA: 0x000ED0E0 File Offset: 0x000EC0E0
		// (set) Token: 0x06003703 RID: 14083 RVA: 0x000ED10E File Offset: 0x000EC10E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(100)]
		[WebCategory("Layout")]
		public int Height
		{
			get
			{
				string text = base.Attributes["height"];
				if (text == null)
				{
					return -1;
				}
				return int.Parse(text, CultureInfo.InvariantCulture);
			}
			set
			{
				base.Attributes["height"] = HtmlControl.MapIntegerAttributeToString(value);
			}
		}

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06003704 RID: 14084 RVA: 0x000ED128 File Offset: 0x000EC128
		// (set) Token: 0x06003705 RID: 14085 RVA: 0x000ED150 File Offset: 0x000EC150
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[UrlProperty]
		[WebCategory("Behavior")]
		[DefaultValue("")]
		public string Src
		{
			get
			{
				string text = base.Attributes["src"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["src"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06003706 RID: 14086 RVA: 0x000ED168 File Offset: 0x000EC168
		// (set) Token: 0x06003707 RID: 14087 RVA: 0x000ED196 File Offset: 0x000EC196
		[WebCategory("Layout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(100)]
		public int Width
		{
			get
			{
				string text = base.Attributes["width"];
				if (text == null)
				{
					return -1;
				}
				return int.Parse(text, CultureInfo.InvariantCulture);
			}
			set
			{
				base.Attributes["width"] = HtmlControl.MapIntegerAttributeToString(value);
			}
		}

		// Token: 0x06003708 RID: 14088 RVA: 0x000ED1AE File Offset: 0x000EC1AE
		protected override void RenderAttributes(HtmlTextWriter writer)
		{
			base.PreProcessRelativeReferenceAttribute(writer, "src");
			base.RenderAttributes(writer);
			writer.Write(" /");
		}
	}
}
