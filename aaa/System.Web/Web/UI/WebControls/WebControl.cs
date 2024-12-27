using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004BA RID: 1210
	[PersistChildren(false)]
	[ParseChildren(true)]
	[Themeable(true)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebControl : Control, IAttributeAccessor
	{
		// Token: 0x06003942 RID: 14658 RVA: 0x000F2B50 File Offset: 0x000F1B50
		protected WebControl()
			: this(HtmlTextWriterTag.Span)
		{
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x000F2B5A File Offset: 0x000F1B5A
		public WebControl(HtmlTextWriterTag tag)
		{
			this.tagKey = tag;
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x000F2B69 File Offset: 0x000F1B69
		protected WebControl(string tag)
		{
			this.tagKey = HtmlTextWriterTag.Unknown;
			this.tagName = tag;
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06003945 RID: 14661 RVA: 0x000F2B80 File Offset: 0x000F1B80
		// (set) Token: 0x06003946 RID: 14662 RVA: 0x000F2BBB File Offset: 0x000F1BBB
		[DefaultValue("")]
		[WebCategory("Accessibility")]
		[WebSysDescription("WebControl_AccessKey")]
		public virtual string AccessKey
		{
			get
			{
				if (this._webControlFlags[4])
				{
					string text = (string)this.ViewState["AccessKey"];
					if (text != null)
					{
						return text;
					}
				}
				return string.Empty;
			}
			set
			{
				if (value != null && value.Length > 1)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("WebControl_InvalidAccessKey"));
				}
				this.ViewState["AccessKey"] = value;
				this._webControlFlags.Set(4);
			}
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06003947 RID: 14663 RVA: 0x000F2BFC File Offset: 0x000F1BFC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebSysDescription("WebControl_Attributes")]
		[Browsable(false)]
		public AttributeCollection Attributes
		{
			get
			{
				if (this.attrColl == null)
				{
					if (this.attrState == null)
					{
						this.attrState = new StateBag(true);
						if (base.IsTrackingViewState)
						{
							this.attrState.TrackViewState();
						}
					}
					this.attrColl = new AttributeCollection(this.attrState);
				}
				return this.attrColl;
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06003948 RID: 14664 RVA: 0x000F2C4F File Offset: 0x000F1C4F
		// (set) Token: 0x06003949 RID: 14665 RVA: 0x000F2C6A File Offset: 0x000F1C6A
		[DefaultValue(typeof(Color), "")]
		[TypeConverter(typeof(WebColorConverter))]
		[WebSysDescription("WebControl_BackColor")]
		[WebCategory("Appearance")]
		public virtual Color BackColor
		{
			get
			{
				if (!this.ControlStyleCreated)
				{
					return Color.Empty;
				}
				return this.ControlStyle.BackColor;
			}
			set
			{
				this.ControlStyle.BackColor = value;
			}
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x0600394A RID: 14666 RVA: 0x000F2C78 File Offset: 0x000F1C78
		// (set) Token: 0x0600394B RID: 14667 RVA: 0x000F2C93 File Offset: 0x000F1C93
		[WebCategory("Appearance")]
		[DefaultValue(typeof(Color), "")]
		[WebSysDescription("WebControl_BorderColor")]
		[TypeConverter(typeof(WebColorConverter))]
		public virtual Color BorderColor
		{
			get
			{
				if (!this.ControlStyleCreated)
				{
					return Color.Empty;
				}
				return this.ControlStyle.BorderColor;
			}
			set
			{
				this.ControlStyle.BorderColor = value;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x0600394C RID: 14668 RVA: 0x000F2CA1 File Offset: 0x000F1CA1
		// (set) Token: 0x0600394D RID: 14669 RVA: 0x000F2CBC File Offset: 0x000F1CBC
		[WebSysDescription("WebControl_BorderWidth")]
		[DefaultValue(typeof(Unit), "")]
		[WebCategory("Appearance")]
		public virtual Unit BorderWidth
		{
			get
			{
				if (!this.ControlStyleCreated)
				{
					return Unit.Empty;
				}
				return this.ControlStyle.BorderWidth;
			}
			set
			{
				this.ControlStyle.BorderWidth = value;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x0600394E RID: 14670 RVA: 0x000F2CCA File Offset: 0x000F1CCA
		// (set) Token: 0x0600394F RID: 14671 RVA: 0x000F2CE1 File Offset: 0x000F1CE1
		[WebCategory("Appearance")]
		[DefaultValue(BorderStyle.NotSet)]
		[WebSysDescription("WebControl_BorderStyle")]
		public virtual BorderStyle BorderStyle
		{
			get
			{
				if (!this.ControlStyleCreated)
				{
					return BorderStyle.NotSet;
				}
				return this.ControlStyle.BorderStyle;
			}
			set
			{
				this.ControlStyle.BorderStyle = value;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06003950 RID: 14672 RVA: 0x000F2CF0 File Offset: 0x000F1CF0
		[WebSysDescription("WebControl_ControlStyle")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Style ControlStyle
		{
			get
			{
				if (this.controlStyle == null)
				{
					this.controlStyle = this.CreateControlStyle();
					if (base.IsTrackingViewState)
					{
						this.controlStyle.TrackViewState();
					}
					if (this._webControlFlags[1])
					{
						this._webControlFlags.Clear(1);
						this.controlStyle.LoadViewState(null);
					}
				}
				return this.controlStyle;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06003951 RID: 14673 RVA: 0x000F2D50 File Offset: 0x000F1D50
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[WebSysDescription("WebControl_ControlStyleCreated")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool ControlStyleCreated
		{
			get
			{
				return this.controlStyle != null;
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06003952 RID: 14674 RVA: 0x000F2D5E File Offset: 0x000F1D5E
		// (set) Token: 0x06003953 RID: 14675 RVA: 0x000F2D79 File Offset: 0x000F1D79
		[WebCategory("Appearance")]
		[CssClassProperty]
		[DefaultValue("")]
		[WebSysDescription("WebControl_CSSClassName")]
		public virtual string CssClass
		{
			get
			{
				if (!this.ControlStyleCreated)
				{
					return string.Empty;
				}
				return this.ControlStyle.CssClass;
			}
			set
			{
				this.ControlStyle.CssClass = value;
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06003954 RID: 14676 RVA: 0x000F2D87 File Offset: 0x000F1D87
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebSysDescription("WebControl_Style")]
		public CssStyleCollection Style
		{
			get
			{
				return this.Attributes.CssStyle;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x06003955 RID: 14677 RVA: 0x000F2D94 File Offset: 0x000F1D94
		// (set) Token: 0x06003956 RID: 14678 RVA: 0x000F2DAC File Offset: 0x000F1DAC
		[DefaultValue(true)]
		[WebCategory("Behavior")]
		[Themeable(false)]
		[Bindable(true)]
		[WebSysDescription("WebControl_Enabled")]
		public virtual bool Enabled
		{
			get
			{
				return !this.flags[524288];
			}
			set
			{
				bool flag = !this.flags[524288];
				if (flag != value)
				{
					if (!value)
					{
						this.flags.Set(524288);
					}
					else
					{
						this.flags.Clear(524288);
					}
					if (base.IsTrackingViewState)
					{
						this._webControlFlags.Set(2);
					}
				}
			}
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06003957 RID: 14679 RVA: 0x000F2E0A File Offset: 0x000F1E0A
		// (set) Token: 0x06003958 RID: 14680 RVA: 0x000F2E12 File Offset: 0x000F1E12
		[Browsable(true)]
		public override bool EnableTheming
		{
			get
			{
				return base.EnableTheming;
			}
			set
			{
				base.EnableTheming = value;
			}
		}

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06003959 RID: 14681 RVA: 0x000F2E1B File Offset: 0x000F1E1B
		[WebCategory("Appearance")]
		[WebSysDescription("WebControl_Font")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		public virtual FontInfo Font
		{
			get
			{
				return this.ControlStyle.Font;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x0600395A RID: 14682 RVA: 0x000F2E28 File Offset: 0x000F1E28
		// (set) Token: 0x0600395B RID: 14683 RVA: 0x000F2E43 File Offset: 0x000F1E43
		[TypeConverter(typeof(WebColorConverter))]
		[WebSysDescription("WebControl_ForeColor")]
		[WebCategory("Appearance")]
		[DefaultValue(typeof(Color), "")]
		public virtual Color ForeColor
		{
			get
			{
				if (!this.ControlStyleCreated)
				{
					return Color.Empty;
				}
				return this.ControlStyle.ForeColor;
			}
			set
			{
				this.ControlStyle.ForeColor = value;
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x0600395C RID: 14684 RVA: 0x000F2E51 File Offset: 0x000F1E51
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool HasAttributes
		{
			get
			{
				return (this.attrColl != null && this.attrColl.Count > 0) || (this.attrState != null && this.attrState.Count > 0);
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x0600395D RID: 14685 RVA: 0x000F2E83 File Offset: 0x000F1E83
		// (set) Token: 0x0600395E RID: 14686 RVA: 0x000F2E9E File Offset: 0x000F1E9E
		[WebCategory("Layout")]
		[DefaultValue(typeof(Unit), "")]
		[WebSysDescription("WebControl_Height")]
		public virtual Unit Height
		{
			get
			{
				if (!this.ControlStyleCreated)
				{
					return Unit.Empty;
				}
				return this.ControlStyle.Height;
			}
			set
			{
				this.ControlStyle.Height = value;
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x0600395F RID: 14687 RVA: 0x000F2EAC File Offset: 0x000F1EAC
		protected internal bool IsEnabled
		{
			get
			{
				for (Control control = this; control != null; control = control.Parent)
				{
					if (control.flags[524288])
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06003960 RID: 14688 RVA: 0x000F2EDC File Offset: 0x000F1EDC
		internal virtual bool RequiresLegacyRendering
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06003961 RID: 14689 RVA: 0x000F2EDF File Offset: 0x000F1EDF
		// (set) Token: 0x06003962 RID: 14690 RVA: 0x000F2EE7 File Offset: 0x000F1EE7
		[Browsable(true)]
		public override string SkinID
		{
			get
			{
				return base.SkinID;
			}
			set
			{
				base.SkinID = value;
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06003963 RID: 14691 RVA: 0x000F2EF0 File Offset: 0x000F1EF0
		// (set) Token: 0x06003964 RID: 14692 RVA: 0x000F2F28 File Offset: 0x000F1F28
		[DefaultValue(0)]
		[WebCategory("Accessibility")]
		[WebSysDescription("WebControl_TabIndex")]
		public virtual short TabIndex
		{
			get
			{
				if (this._webControlFlags[16])
				{
					object obj = this.ViewState["TabIndex"];
					if (obj != null)
					{
						return (short)obj;
					}
				}
				return 0;
			}
			set
			{
				this.ViewState["TabIndex"] = value;
				this._webControlFlags.Set(16);
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06003965 RID: 14693 RVA: 0x000F2F4D File Offset: 0x000F1F4D
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected virtual HtmlTextWriterTag TagKey
		{
			get
			{
				return this.tagKey;
			}
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06003966 RID: 14694 RVA: 0x000F2F58 File Offset: 0x000F1F58
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		protected virtual string TagName
		{
			get
			{
				if (this.tagName == null && this.TagKey != HtmlTextWriterTag.Unknown)
				{
					this.tagName = Enum.Format(typeof(HtmlTextWriterTag), this.TagKey, "G").ToLower(CultureInfo.InvariantCulture);
				}
				return this.tagName;
			}
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06003967 RID: 14695 RVA: 0x000F2FAC File Offset: 0x000F1FAC
		// (set) Token: 0x06003968 RID: 14696 RVA: 0x000F2FE7 File Offset: 0x000F1FE7
		[WebCategory("Behavior")]
		[WebSysDescription("WebControl_Tooltip")]
		[Localizable(true)]
		[DefaultValue("")]
		public virtual string ToolTip
		{
			get
			{
				if (this._webControlFlags[8])
				{
					string text = (string)this.ViewState["ToolTip"];
					if (text != null)
					{
						return text;
					}
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ToolTip"] = value;
				this._webControlFlags.Set(8);
			}
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06003969 RID: 14697 RVA: 0x000F3006 File Offset: 0x000F2006
		// (set) Token: 0x0600396A RID: 14698 RVA: 0x000F3021 File Offset: 0x000F2021
		[DefaultValue(typeof(Unit), "")]
		[WebCategory("Layout")]
		[WebSysDescription("WebControl_Width")]
		public virtual Unit Width
		{
			get
			{
				if (!this.ControlStyleCreated)
				{
					return Unit.Empty;
				}
				return this.ControlStyle.Width;
			}
			set
			{
				this.ControlStyle.Width = value;
			}
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x000F3030 File Offset: 0x000F2030
		protected virtual void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (this.ID != null)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
			}
			if (this._webControlFlags[4])
			{
				string accessKey = this.AccessKey;
				if (accessKey.Length > 0)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Accesskey, accessKey);
				}
			}
			if (!this.Enabled)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
			}
			if (this._webControlFlags[16])
			{
				int tabIndex = (int)this.TabIndex;
				if (tabIndex != 0)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Tabindex, tabIndex.ToString(NumberFormatInfo.InvariantInfo));
				}
			}
			if (this._webControlFlags[8])
			{
				string toolTip = this.ToolTip;
				if (toolTip.Length > 0)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Title, toolTip);
				}
			}
			if (this.TagKey == HtmlTextWriterTag.Span || this.TagKey == HtmlTextWriterTag.A)
			{
				this.AddDisplayInlineBlockIfNeeded(writer);
			}
			if (this.ControlStyleCreated && !this.ControlStyle.IsEmpty)
			{
				this.ControlStyle.AddAttributesToRender(writer, this);
			}
			if (this.attrState != null)
			{
				AttributeCollection attributes = this.Attributes;
				foreach (object obj in attributes.Keys)
				{
					string text = (string)obj;
					writer.AddAttribute(text, attributes[text]);
				}
			}
		}

		// Token: 0x0600396C RID: 14700 RVA: 0x000F3164 File Offset: 0x000F2164
		internal void AddDisplayInlineBlockIfNeeded(HtmlTextWriter writer)
		{
			if ((!this.RequiresLegacyRendering || !base.EnableLegacyRendering) && (this.BorderStyle != BorderStyle.NotSet || !this.BorderWidth.IsEmpty || !this.Height.IsEmpty || !this.Width.IsEmpty))
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "inline-block");
			}
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x000F31C6 File Offset: 0x000F21C6
		public void ApplyStyle(Style s)
		{
			if (s != null && !s.IsEmpty)
			{
				this.ControlStyle.CopyFrom(s);
			}
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x000F31E0 File Offset: 0x000F21E0
		public void CopyBaseAttributes(WebControl controlSrc)
		{
			if (controlSrc == null)
			{
				throw new ArgumentNullException("controlSrc");
			}
			if (controlSrc._webControlFlags[4])
			{
				this.AccessKey = controlSrc.AccessKey;
			}
			if (!controlSrc.Enabled)
			{
				this.Enabled = false;
			}
			if (controlSrc._webControlFlags[8])
			{
				this.ToolTip = controlSrc.ToolTip;
			}
			if (controlSrc._webControlFlags[16])
			{
				this.TabIndex = controlSrc.TabIndex;
			}
			if (controlSrc.HasAttributes)
			{
				foreach (object obj in controlSrc.Attributes.Keys)
				{
					string text = (string)obj;
					this.Attributes[text] = controlSrc.Attributes[text];
				}
			}
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x000F32C4 File Offset: 0x000F22C4
		protected virtual Style CreateControlStyle()
		{
			return new Style(this.ViewState);
		}

		// Token: 0x06003970 RID: 14704 RVA: 0x000F32D4 File Offset: 0x000F22D4
		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				Pair pair = (Pair)savedState;
				base.LoadViewState(pair.First);
				if (this.ControlStyleCreated || this.ViewState["_!SB"] != null)
				{
					this.ControlStyle.LoadViewState(null);
				}
				else
				{
					this._webControlFlags.Set(1);
				}
				if (pair.Second != null)
				{
					if (this.attrState == null)
					{
						this.attrState = new StateBag(true);
						this.attrState.TrackViewState();
					}
					this.attrState.LoadViewState(pair.Second);
				}
			}
			object obj = this.ViewState["Enabled"];
			if (obj != null)
			{
				if (!(bool)obj)
				{
					this.flags.Set(524288);
				}
				else
				{
					this.flags.Clear(524288);
				}
				this._webControlFlags.Set(2);
			}
			if (((IDictionary)this.ViewState).Contains("AccessKey"))
			{
				this._webControlFlags.Set(4);
			}
			if (((IDictionary)this.ViewState).Contains("TabIndex"))
			{
				this._webControlFlags.Set(16);
			}
			if (((IDictionary)this.ViewState).Contains("ToolTip"))
			{
				this._webControlFlags.Set(8);
			}
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x000F3408 File Offset: 0x000F2408
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this.ControlStyleCreated)
			{
				this.ControlStyle.TrackViewState();
			}
			if (this.attrState != null)
			{
				this.attrState.TrackViewState();
			}
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x000F3436 File Offset: 0x000F2436
		public void MergeStyle(Style s)
		{
			if (s != null && !s.IsEmpty)
			{
				this.ControlStyle.MergeWith(s);
			}
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x000F344F File Offset: 0x000F244F
		protected internal override void Render(HtmlTextWriter writer)
		{
			this.RenderBeginTag(writer);
			this.RenderContents(writer);
			this.RenderEndTag(writer);
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x000F3468 File Offset: 0x000F2468
		public virtual void RenderBeginTag(HtmlTextWriter writer)
		{
			this.AddAttributesToRender(writer);
			HtmlTextWriterTag htmlTextWriterTag = this.TagKey;
			if (htmlTextWriterTag != HtmlTextWriterTag.Unknown)
			{
				writer.RenderBeginTag(htmlTextWriterTag);
				return;
			}
			writer.RenderBeginTag(this.TagName);
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x000F349A File Offset: 0x000F249A
		public virtual void RenderEndTag(HtmlTextWriter writer)
		{
			writer.RenderEndTag();
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x000F34A2 File Offset: 0x000F24A2
		protected internal virtual void RenderContents(HtmlTextWriter writer)
		{
			base.Render(writer);
		}

		// Token: 0x06003977 RID: 14711 RVA: 0x000F34AC File Offset: 0x000F24AC
		protected override object SaveViewState()
		{
			Pair pair = null;
			if (this._webControlFlags[2])
			{
				this.ViewState["Enabled"] = !this.flags[524288];
			}
			if (this.ControlStyleCreated)
			{
				this.ControlStyle.SaveViewState();
			}
			object obj = base.SaveViewState();
			object obj2 = null;
			if (this.attrState != null)
			{
				obj2 = this.attrState.SaveViewState();
			}
			if (obj != null || obj2 != null)
			{
				pair = new Pair(obj, obj2);
			}
			return pair;
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x000F3531 File Offset: 0x000F2531
		string IAttributeAccessor.GetAttribute(string name)
		{
			if (this.attrState == null)
			{
				return null;
			}
			return (string)this.attrState[name];
		}

		// Token: 0x06003979 RID: 14713 RVA: 0x000F354E File Offset: 0x000F254E
		void IAttributeAccessor.SetAttribute(string name, string value)
		{
			this.Attributes[name] = value;
		}

		// Token: 0x0400261C RID: 9756
		private const int deferStyleLoadViewState = 1;

		// Token: 0x0400261D RID: 9757
		private const int disabledDirty = 2;

		// Token: 0x0400261E RID: 9758
		private const int accessKeySet = 4;

		// Token: 0x0400261F RID: 9759
		private const int toolTipSet = 8;

		// Token: 0x04002620 RID: 9760
		private const int tabIndexSet = 16;

		// Token: 0x04002621 RID: 9761
		private string tagName;

		// Token: 0x04002622 RID: 9762
		private HtmlTextWriterTag tagKey;

		// Token: 0x04002623 RID: 9763
		private AttributeCollection attrColl;

		// Token: 0x04002624 RID: 9764
		private StateBag attrState;

		// Token: 0x04002625 RID: 9765
		private Style controlStyle;

		// Token: 0x04002626 RID: 9766
		private SimpleBitVector32 _webControlFlags;
	}
}
