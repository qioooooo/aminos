using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x020004AC RID: 1196
	[ConstructorNeedsTag(true)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlTableCell : HtmlContainerControl
	{
		// Token: 0x060037FB RID: 14331 RVA: 0x000EF7D5 File Offset: 0x000EE7D5
		public HtmlTableCell()
			: base("td")
		{
		}

		// Token: 0x060037FC RID: 14332 RVA: 0x000EF7E2 File Offset: 0x000EE7E2
		public HtmlTableCell(string tagName)
			: base(tagName)
		{
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x060037FD RID: 14333 RVA: 0x000EF7EC File Offset: 0x000EE7EC
		// (set) Token: 0x060037FE RID: 14334 RVA: 0x000EF814 File Offset: 0x000EE814
		[DefaultValue("")]
		[WebCategory("Layout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x060037FF RID: 14335 RVA: 0x000EF82C File Offset: 0x000EE82C
		// (set) Token: 0x06003800 RID: 14336 RVA: 0x000EF854 File Offset: 0x000EE854
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		public string BgColor
		{
			get
			{
				string text = base.Attributes["bgcolor"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["bgcolor"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x06003801 RID: 14337 RVA: 0x000EF86C File Offset: 0x000EE86C
		// (set) Token: 0x06003802 RID: 14338 RVA: 0x000EF894 File Offset: 0x000EE894
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		public string BorderColor
		{
			get
			{
				string text = base.Attributes["bordercolor"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["bordercolor"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x06003803 RID: 14339 RVA: 0x000EF8AC File Offset: 0x000EE8AC
		// (set) Token: 0x06003804 RID: 14340 RVA: 0x000EF8DA File Offset: 0x000EE8DA
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue("")]
		[WebCategory("Layout")]
		public int ColSpan
		{
			get
			{
				string text = base.Attributes["colspan"];
				if (text == null)
				{
					return -1;
				}
				return int.Parse(text, CultureInfo.InvariantCulture);
			}
			set
			{
				base.Attributes["colspan"] = HtmlControl.MapIntegerAttributeToString(value);
			}
		}

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06003805 RID: 14341 RVA: 0x000EF8F4 File Offset: 0x000EE8F4
		// (set) Token: 0x06003806 RID: 14342 RVA: 0x000EF91C File Offset: 0x000EE91C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue("")]
		[WebCategory("Layout")]
		public string Height
		{
			get
			{
				string text = base.Attributes["height"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["height"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06003807 RID: 14343 RVA: 0x000EF934 File Offset: 0x000EE934
		// (set) Token: 0x06003808 RID: 14344 RVA: 0x000EF962 File Offset: 0x000EE962
		[DefaultValue("")]
		[TypeConverter(typeof(MinimizableAttributeTypeConverter))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Behavior")]
		public bool NoWrap
		{
			get
			{
				string text = base.Attributes["nowrap"];
				return text != null && text.Equals("nowrap");
			}
			set
			{
				if (value)
				{
					base.Attributes["nowrap"] = "nowrap";
					return;
				}
				base.Attributes["nowrap"] = null;
			}
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06003809 RID: 14345 RVA: 0x000EF990 File Offset: 0x000EE990
		// (set) Token: 0x0600380A RID: 14346 RVA: 0x000EF9BE File Offset: 0x000EE9BE
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Layout")]
		public int RowSpan
		{
			get
			{
				string text = base.Attributes["rowspan"];
				if (text == null)
				{
					return -1;
				}
				return int.Parse(text, CultureInfo.InvariantCulture);
			}
			set
			{
				base.Attributes["rowspan"] = HtmlControl.MapIntegerAttributeToString(value);
			}
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x0600380B RID: 14347 RVA: 0x000EF9D8 File Offset: 0x000EE9D8
		// (set) Token: 0x0600380C RID: 14348 RVA: 0x000EFA00 File Offset: 0x000EEA00
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Layout")]
		public string VAlign
		{
			get
			{
				string text = base.Attributes["valign"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["valign"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x0600380D RID: 14349 RVA: 0x000EFA18 File Offset: 0x000EEA18
		// (set) Token: 0x0600380E RID: 14350 RVA: 0x000EFA40 File Offset: 0x000EEA40
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Layout")]
		[DefaultValue("")]
		public string Width
		{
			get
			{
				string text = base.Attributes["width"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["width"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x0600380F RID: 14351 RVA: 0x000EFA58 File Offset: 0x000EEA58
		protected override void RenderEndTag(HtmlTextWriter writer)
		{
			base.RenderEndTag(writer);
			writer.WriteLine();
		}
	}
}
