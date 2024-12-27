using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004C7 RID: 1223
	[Designer("System.Web.UI.Design.WebControls.LabelDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolboxData("<{0}:Label runat=\"server\" Text=\"Label\"></{0}:Label>")]
	[ControlBuilder(typeof(LabelControlBuilder))]
	[ControlValueProperty("Text")]
	[DataBindingHandler("System.Web.UI.Design.TextDataBindingHandler, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("Text")]
	[ParseChildren(false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class Label : WebControl, ITextControl
	{
		// Token: 0x06003A55 RID: 14933 RVA: 0x000F65A2 File Offset: 0x000F55A2
		public Label()
		{
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x000F65AA File Offset: 0x000F55AA
		internal Label(HtmlTextWriterTag tag)
			: base(tag)
		{
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06003A57 RID: 14935 RVA: 0x000F65B4 File Offset: 0x000F55B4
		// (set) Token: 0x06003A58 RID: 14936 RVA: 0x000F65E1 File Offset: 0x000F55E1
		[IDReferenceProperty]
		[WebCategory("Accessibility")]
		[WebSysDescription("Label_AssociatedControlID")]
		[Themeable(false)]
		[DefaultValue("")]
		[TypeConverter(typeof(AssociatedControlConverter))]
		public virtual string AssociatedControlID
		{
			get
			{
				string text = (string)this.ViewState["AssociatedControlID"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["AssociatedControlID"] = value;
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06003A59 RID: 14937 RVA: 0x000F65F4 File Offset: 0x000F55F4
		// (set) Token: 0x06003A5A RID: 14938 RVA: 0x000F661D File Offset: 0x000F561D
		internal bool AssociatedControlInControlTree
		{
			get
			{
				object obj = this.ViewState["AssociatedControlNotInControlTree"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["AssociatedControlNotInControlTree"] = value;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06003A5B RID: 14939 RVA: 0x000F6635 File Offset: 0x000F5635
		internal override bool RequiresLegacyRendering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06003A5C RID: 14940 RVA: 0x000F6638 File Offset: 0x000F5638
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				if (this.AssociatedControlID.Length != 0)
				{
					return HtmlTextWriterTag.Label;
				}
				return base.TagKey;
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06003A5D RID: 14941 RVA: 0x000F6650 File Offset: 0x000F5650
		// (set) Token: 0x06003A5E RID: 14942 RVA: 0x000F667D File Offset: 0x000F567D
		[Localizable(true)]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[WebSysDescription("Label_Text")]
		[PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		[Bindable(true)]
		public virtual string Text
		{
			get
			{
				object obj = this.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (this.HasControls())
				{
					this.Controls.Clear();
				}
				this.ViewState["Text"] = value;
			}
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x000F66A4 File Offset: 0x000F56A4
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			string associatedControlID = this.AssociatedControlID;
			if (associatedControlID.Length != 0)
			{
				if (this.AssociatedControlInControlTree)
				{
					Control control = this.FindControl(associatedControlID);
					if (control == null)
					{
						if (!base.DesignMode)
						{
							throw new HttpException(SR.GetString("LabelForNotFound", new object[] { associatedControlID, this.ID }));
						}
					}
					else
					{
						writer.AddAttribute(HtmlTextWriterAttribute.For, control.ClientID);
					}
				}
				else
				{
					writer.AddAttribute(HtmlTextWriterAttribute.For, associatedControlID);
				}
			}
			base.AddAttributesToRender(writer);
		}

		// Token: 0x06003A60 RID: 14944 RVA: 0x000F6720 File Offset: 0x000F5720
		protected override void AddParsedSubObject(object obj)
		{
			if (this.HasControls())
			{
				base.AddParsedSubObject(obj);
				return;
			}
			if (!(obj is LiteralControl))
			{
				string text = this.Text;
				if (text.Length != 0)
				{
					this.Text = string.Empty;
					base.AddParsedSubObject(new LiteralControl(text));
				}
				base.AddParsedSubObject(obj);
				return;
			}
			if (base.DesignMode)
			{
				if (this._textSetByAddParsedSubObject)
				{
					this.Text += ((LiteralControl)obj).Text;
				}
				else
				{
					this.Text = ((LiteralControl)obj).Text;
				}
				this._textSetByAddParsedSubObject = true;
				return;
			}
			this.Text = ((LiteralControl)obj).Text;
		}

		// Token: 0x06003A61 RID: 14945 RVA: 0x000F67CC File Offset: 0x000F57CC
		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				base.LoadViewState(savedState);
				string text = (string)this.ViewState["Text"];
				if (text != null)
				{
					this.Text = text;
				}
			}
		}

		// Token: 0x06003A62 RID: 14946 RVA: 0x000F6803 File Offset: 0x000F5803
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
			if (base.HasRenderingData())
			{
				base.RenderContents(writer);
				return;
			}
			writer.Write(this.Text);
		}

		// Token: 0x04002683 RID: 9859
		private bool _textSetByAddParsedSubObject;
	}
}
