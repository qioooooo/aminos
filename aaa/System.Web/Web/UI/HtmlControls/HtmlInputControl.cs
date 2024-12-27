using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x0200049B RID: 1179
	[ControlBuilder(typeof(HtmlEmptyTagControlBuilder))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class HtmlInputControl : HtmlControl
	{
		// Token: 0x06003709 RID: 14089 RVA: 0x000ED1CE File Offset: 0x000EC1CE
		protected HtmlInputControl(string type)
			: base("input")
		{
			this._type = type;
			base.Attributes["type"] = type;
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x0600370A RID: 14090 RVA: 0x000ED1F3 File Offset: 0x000EC1F3
		// (set) Token: 0x0600370B RID: 14091 RVA: 0x000ED1FB File Offset: 0x000EC1FB
		[DefaultValue("")]
		[WebCategory("Behavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string Name
		{
			get
			{
				return this.UniqueID;
			}
			set
			{
			}
		}

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x0600370C RID: 14092 RVA: 0x000ED1FD File Offset: 0x000EC1FD
		internal virtual string RenderedNameAttribute
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x0600370D RID: 14093 RVA: 0x000ED208 File Offset: 0x000EC208
		// (set) Token: 0x0600370E RID: 14094 RVA: 0x000ED230 File Offset: 0x000EC230
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue("")]
		[WebCategory("Appearance")]
		public virtual string Value
		{
			get
			{
				string text = base.Attributes["value"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["value"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x0600370F RID: 14095 RVA: 0x000ED248 File Offset: 0x000EC248
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Behavior")]
		[DefaultValue("")]
		public string Type
		{
			get
			{
				string text = base.Attributes["type"];
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
				if (this._type == null)
				{
					return string.Empty;
				}
				return this._type;
			}
		}

		// Token: 0x06003710 RID: 14096 RVA: 0x000ED284 File Offset: 0x000EC284
		protected override void RenderAttributes(HtmlTextWriter writer)
		{
			writer.WriteAttribute("name", this.RenderedNameAttribute);
			base.Attributes.Remove("name");
			bool flag = false;
			string type = this.Type;
			if (!string.IsNullOrEmpty(type))
			{
				writer.WriteAttribute("type", type);
				base.Attributes.Remove("type");
				flag = true;
			}
			base.RenderAttributes(writer);
			if (flag && base.DesignMode)
			{
				base.Attributes.Add("type", type);
			}
			writer.Write(" /");
		}

		// Token: 0x040025C2 RID: 9666
		private string _type;
	}
}
