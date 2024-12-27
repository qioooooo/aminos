using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x020004A7 RID: 1191
	[ControlBuilder(typeof(HtmlEmptyTagControlBuilder))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlMeta : HtmlControl
	{
		// Token: 0x06003798 RID: 14232 RVA: 0x000EE382 File Offset: 0x000ED382
		public HtmlMeta()
			: base("meta")
		{
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x06003799 RID: 14233 RVA: 0x000EE390 File Offset: 0x000ED390
		// (set) Token: 0x0600379A RID: 14234 RVA: 0x000EE3B8 File Offset: 0x000ED3B8
		[WebCategory("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue("")]
		public virtual string Content
		{
			get
			{
				string text = base.Attributes["content"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["content"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x0600379B RID: 14235 RVA: 0x000EE3D0 File Offset: 0x000ED3D0
		// (set) Token: 0x0600379C RID: 14236 RVA: 0x000EE3F8 File Offset: 0x000ED3F8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		public virtual string HttpEquiv
		{
			get
			{
				string text = base.Attributes["http-equiv"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["http-equiv"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x0600379D RID: 14237 RVA: 0x000EE410 File Offset: 0x000ED410
		// (set) Token: 0x0600379E RID: 14238 RVA: 0x000EE438 File Offset: 0x000ED438
		[DefaultValue("")]
		[WebCategory("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string Name
		{
			get
			{
				string text = base.Attributes["name"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["name"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x0600379F RID: 14239 RVA: 0x000EE450 File Offset: 0x000ED450
		// (set) Token: 0x060037A0 RID: 14240 RVA: 0x000EE478 File Offset: 0x000ED478
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		public virtual string Scheme
		{
			get
			{
				string text = base.Attributes["scheme"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["scheme"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x060037A1 RID: 14241 RVA: 0x000EE490 File Offset: 0x000ED490
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (base.EnableLegacyRendering)
			{
				base.Render(writer);
				return;
			}
			writer.WriteBeginTag(this.TagName);
			this.RenderAttributes(writer);
			writer.Write(" />");
		}
	}
}
