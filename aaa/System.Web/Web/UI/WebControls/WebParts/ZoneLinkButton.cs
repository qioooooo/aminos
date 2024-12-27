using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200074E RID: 1870
	[SupportsEventValidation]
	internal sealed class ZoneLinkButton : LinkButton
	{
		// Token: 0x06005AD6 RID: 23254 RVA: 0x0016E818 File Offset: 0x0016D818
		public ZoneLinkButton(WebZone owner, string eventArgument)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this._owner = owner;
			this._eventArgument = eventArgument;
		}

		// Token: 0x1700178C RID: 6028
		// (get) Token: 0x06005AD7 RID: 23255 RVA: 0x0016E83C File Offset: 0x0016D83C
		// (set) Token: 0x06005AD8 RID: 23256 RVA: 0x0016E852 File Offset: 0x0016D852
		public string ImageUrl
		{
			get
			{
				if (this._imageUrl == null)
				{
					return string.Empty;
				}
				return this._imageUrl;
			}
			set
			{
				this._imageUrl = value;
			}
		}

		// Token: 0x06005AD9 RID: 23257 RVA: 0x0016E85C File Offset: 0x0016D85C
		protected override PostBackOptions GetPostBackOptions()
		{
			if (!string.IsNullOrEmpty(this._eventArgument) && this._owner.Page != null)
			{
				return new PostBackOptions(this._owner, this._eventArgument)
				{
					RequiresJavaScriptProtocol = true
				};
			}
			return base.GetPostBackOptions();
		}

		// Token: 0x06005ADA RID: 23258 RVA: 0x0016E8A4 File Offset: 0x0016D8A4
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
			string imageUrl = this.ImageUrl;
			if (!string.IsNullOrEmpty(imageUrl))
			{
				Image image = new Image();
				image.ImageUrl = base.ResolveClientUrl(imageUrl);
				string toolTip = this.ToolTip;
				if (!string.IsNullOrEmpty(toolTip))
				{
					image.ToolTip = toolTip;
				}
				string text = this.Text;
				if (!string.IsNullOrEmpty(text))
				{
					image.AlternateText = text;
				}
				image.Page = this.Page;
				image.RenderControl(writer);
				return;
			}
			base.RenderContents(writer);
		}

		// Token: 0x040030CC RID: 12492
		private WebZone _owner;

		// Token: 0x040030CD RID: 12493
		private string _eventArgument;

		// Token: 0x040030CE RID: 12494
		private string _imageUrl;
	}
}
