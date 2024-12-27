using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000531 RID: 1329
	[DefaultProperty("ImageUrl")]
	[Designer("System.Web.UI.Design.WebControls.PreviewControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class Image : WebControl
	{
		// Token: 0x06004152 RID: 16722 RVA: 0x0010F06A File Offset: 0x0010E06A
		public Image()
			: base(HtmlTextWriterTag.Img)
		{
		}

		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x06004153 RID: 16723 RVA: 0x0010F074 File Offset: 0x0010E074
		// (set) Token: 0x06004154 RID: 16724 RVA: 0x0010F0A1 File Offset: 0x0010E0A1
		[WebSysDescription("Image_AlternateText")]
		[Localizable(true)]
		[Bindable(true)]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		public virtual string AlternateText
		{
			get
			{
				string text = (string)this.ViewState["AlternateText"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["AlternateText"] = value;
			}
		}

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x06004155 RID: 16725 RVA: 0x0010F0B4 File Offset: 0x0010E0B4
		// (set) Token: 0x06004156 RID: 16726 RVA: 0x0010F0E1 File Offset: 0x0010E0E1
		[UrlProperty]
		[WebSysDescription("Image_DescriptionUrl")]
		[WebCategory("Accessibility")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual string DescriptionUrl
		{
			get
			{
				string text = (string)this.ViewState["DescriptionUrl"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["DescriptionUrl"] = value;
			}
		}

		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x06004157 RID: 16727 RVA: 0x0010F0F4 File Offset: 0x0010E0F4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override FontInfo Font
		{
			get
			{
				return base.Font;
			}
		}

		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x06004158 RID: 16728 RVA: 0x0010F0FC File Offset: 0x0010E0FC
		// (set) Token: 0x06004159 RID: 16729 RVA: 0x0010F104 File Offset: 0x0010E104
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
			}
		}

		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x0600415A RID: 16730 RVA: 0x0010F110 File Offset: 0x0010E110
		// (set) Token: 0x0600415B RID: 16731 RVA: 0x0010F139 File Offset: 0x0010E139
		[WebCategory("Accessibility")]
		[DefaultValue(false)]
		[WebSysDescription("Image_GenerateEmptyAlternateText")]
		public virtual bool GenerateEmptyAlternateText
		{
			get
			{
				object obj = this.ViewState["GenerateEmptyAlternateText"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["GenerateEmptyAlternateText"] = value;
			}
		}

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x0600415C RID: 16732 RVA: 0x0010F154 File Offset: 0x0010E154
		// (set) Token: 0x0600415D RID: 16733 RVA: 0x0010F17D File Offset: 0x0010E17D
		[DefaultValue(ImageAlign.NotSet)]
		[WebSysDescription("Image_ImageAlign")]
		[WebCategory("Layout")]
		public virtual ImageAlign ImageAlign
		{
			get
			{
				object obj = this.ViewState["ImageAlign"];
				if (obj != null)
				{
					return (ImageAlign)obj;
				}
				return ImageAlign.NotSet;
			}
			set
			{
				if (value < ImageAlign.NotSet || value > ImageAlign.TextTop)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["ImageAlign"] = value;
			}
		}

		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x0600415E RID: 16734 RVA: 0x0010F1AC File Offset: 0x0010E1AC
		// (set) Token: 0x0600415F RID: 16735 RVA: 0x0010F1D9 File Offset: 0x0010E1D9
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		[WebSysDescription("Image_ImageUrl")]
		[Bindable(true)]
		[WebCategory("Appearance")]
		public virtual string ImageUrl
		{
			get
			{
				string text = (string)this.ViewState["ImageUrl"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ImageUrl"] = value;
			}
		}

		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x06004160 RID: 16736 RVA: 0x0010F1EC File Offset: 0x0010E1EC
		// (set) Token: 0x06004161 RID: 16737 RVA: 0x0010F1F4 File Offset: 0x0010E1F4
		internal bool UrlResolved
		{
			get
			{
				return this._urlResolved;
			}
			set
			{
				this._urlResolved = value;
			}
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x0010F200 File Offset: 0x0010E200
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
			string text = this.ImageUrl;
			if (!this.UrlResolved)
			{
				text = base.ResolveClientUrl(text);
			}
			if (text.Length > 0 || !base.EnableLegacyRendering)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Src, text);
			}
			text = this.DescriptionUrl;
			if (text.Length != 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Longdesc, base.ResolveClientUrl(text));
			}
			text = this.AlternateText;
			if (text.Length > 0 || this.GenerateEmptyAlternateText)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Alt, text);
			}
			ImageAlign imageAlign = this.ImageAlign;
			if (imageAlign != ImageAlign.NotSet)
			{
				string text2;
				switch (imageAlign)
				{
				case ImageAlign.Left:
					text2 = "left";
					break;
				case ImageAlign.Right:
					text2 = "right";
					break;
				case ImageAlign.Baseline:
					text2 = "baseline";
					break;
				case ImageAlign.Top:
					text2 = "top";
					break;
				case ImageAlign.Middle:
					text2 = "middle";
					break;
				case ImageAlign.Bottom:
					text2 = "bottom";
					break;
				case ImageAlign.AbsBottom:
					text2 = "absbottom";
					break;
				case ImageAlign.AbsMiddle:
					text2 = "absmiddle";
					break;
				default:
					text2 = "texttop";
					break;
				}
				writer.AddAttribute(HtmlTextWriterAttribute.Align, text2);
			}
			if (this.BorderWidth.IsEmpty)
			{
				if (base.EnableLegacyRendering)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Border, "0", false);
					return;
				}
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "0px");
			}
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x0010F33A File Offset: 0x0010E33A
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
		}

		// Token: 0x040028AF RID: 10415
		private bool _urlResolved;
	}
}
