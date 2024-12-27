using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x020004B2 RID: 1202
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlTitle : HtmlControl
	{
		// Token: 0x06003855 RID: 14421 RVA: 0x000F0208 File Offset: 0x000EF208
		public HtmlTitle()
			: base("title")
		{
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06003856 RID: 14422 RVA: 0x000F0215 File Offset: 0x000EF215
		// (set) Token: 0x06003857 RID: 14423 RVA: 0x000F022B File Offset: 0x000EF22B
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizable(true)]
		[PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		[WebCategory("Appearance")]
		public virtual string Text
		{
			get
			{
				if (this._text == null)
				{
					return string.Empty;
				}
				return this._text;
			}
			set
			{
				this._text = value;
			}
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x000F0234 File Offset: 0x000EF234
		protected override void AddParsedSubObject(object obj)
		{
			if (obj is LiteralControl)
			{
				this._text = ((LiteralControl)obj).Text;
				return;
			}
			base.AddParsedSubObject(obj);
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x000F0257 File Offset: 0x000EF257
		protected override ControlCollection CreateControlCollection()
		{
			return new ControlCollection(this);
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x000F025F File Offset: 0x000EF25F
		protected internal override void Render(HtmlTextWriter writer)
		{
			writer.RenderBeginTag(HtmlTextWriterTag.Title);
			if (this.HasControls() || base.HasRenderDelegate())
			{
				this.RenderChildren(writer);
			}
			else if (this._text != null)
			{
				writer.Write(this._text);
			}
			writer.RenderEndTag();
		}

		// Token: 0x040025DD RID: 9693
		private string _text;
	}
}
