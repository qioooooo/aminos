using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004EF RID: 1263
	[Designer("System.Web.UI.Design.WebControls.CheckBoxDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SupportsEventValidation]
	[DefaultProperty("Text")]
	[DefaultEvent("CheckedChanged")]
	[ControlValueProperty("Checked")]
	[DataBindingHandler("System.Web.UI.Design.TextDataBindingHandler, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CheckBox : WebControl, IPostBackDataHandler, ICheckBoxControl
	{
		// Token: 0x06003D6C RID: 15724 RVA: 0x001016D9 File Offset: 0x001006D9
		public CheckBox()
			: base(HtmlTextWriterTag.Input)
		{
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06003D6D RID: 15725 RVA: 0x001016E4 File Offset: 0x001006E4
		// (set) Token: 0x06003D6E RID: 15726 RVA: 0x0010170D File Offset: 0x0010070D
		[DefaultValue(false)]
		[WebSysDescription("CheckBox_AutoPostBack")]
		[Themeable(false)]
		[WebCategory("Behavior")]
		public virtual bool AutoPostBack
		{
			get
			{
				object obj = this.ViewState["AutoPostBack"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["AutoPostBack"] = value;
			}
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06003D6F RID: 15727 RVA: 0x00101728 File Offset: 0x00100728
		// (set) Token: 0x06003D70 RID: 15728 RVA: 0x00101751 File Offset: 0x00100751
		[WebSysDescription("AutoPostBackControl_CausesValidation")]
		[DefaultValue(false)]
		[WebCategory("Behavior")]
		[Themeable(false)]
		public virtual bool CausesValidation
		{
			get
			{
				object obj = this.ViewState["CausesValidation"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["CausesValidation"] = value;
			}
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06003D71 RID: 15729 RVA: 0x0010176C File Offset: 0x0010076C
		// (set) Token: 0x06003D72 RID: 15730 RVA: 0x00101795 File Offset: 0x00100795
		[DefaultValue(false)]
		[Bindable(true, BindingDirection.TwoWay)]
		[Themeable(false)]
		[WebSysDescription("CheckBox_Checked")]
		public virtual bool Checked
		{
			get
			{
				object obj = this.ViewState["Checked"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["Checked"] = value;
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06003D73 RID: 15731 RVA: 0x001017B0 File Offset: 0x001007B0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebSysDescription("CheckBox_InputAttributes")]
		public AttributeCollection InputAttributes
		{
			get
			{
				if (this._inputAttributes == null)
				{
					if (this._inputAttributesState == null)
					{
						this._inputAttributesState = new StateBag(true);
						if (base.IsTrackingViewState)
						{
							this._inputAttributesState.TrackViewState();
						}
					}
					this._inputAttributes = new AttributeCollection(this._inputAttributesState);
				}
				return this._inputAttributes;
			}
		}

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06003D74 RID: 15732 RVA: 0x00101804 File Offset: 0x00100804
		[WebSysDescription("CheckBox_LabelAttributes")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public AttributeCollection LabelAttributes
		{
			get
			{
				if (this._labelAttributes == null)
				{
					if (this._labelAttributesState == null)
					{
						this._labelAttributesState = new StateBag(true);
						if (base.IsTrackingViewState)
						{
							this._labelAttributesState.TrackViewState();
						}
					}
					this._labelAttributes = new AttributeCollection(this._labelAttributesState);
				}
				return this._labelAttributes;
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06003D75 RID: 15733 RVA: 0x00101857 File Offset: 0x00100857
		internal override bool RequiresLegacyRendering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x0010185C File Offset: 0x0010085C
		private bool SaveCheckedViewState(bool autoPostBack)
		{
			if (base.Events[CheckBox.EventCheckedChanged] != null || !base.IsEnabled || !this.Visible || (autoPostBack && this.Page != null && !this.Page.ClientSupportsJavaScript))
			{
				return true;
			}
			Type type = base.GetType();
			return type != typeof(CheckBox) && type != typeof(RadioButton);
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x06003D77 RID: 15735 RVA: 0x001018CC File Offset: 0x001008CC
		// (set) Token: 0x06003D78 RID: 15736 RVA: 0x001018F9 File Offset: 0x001008F9
		[Localizable(true)]
		[Bindable(true)]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[WebSysDescription("CheckBox_Text")]
		public virtual string Text
		{
			get
			{
				string text = (string)this.ViewState["Text"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["Text"] = value;
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x06003D79 RID: 15737 RVA: 0x0010190C File Offset: 0x0010090C
		// (set) Token: 0x06003D7A RID: 15738 RVA: 0x00101935 File Offset: 0x00100935
		[WebCategory("Appearance")]
		[WebSysDescription("WebControl_TextAlign")]
		[DefaultValue(TextAlign.Right)]
		public virtual TextAlign TextAlign
		{
			get
			{
				object obj = this.ViewState["TextAlign"];
				if (obj != null)
				{
					return (TextAlign)obj;
				}
				return TextAlign.Right;
			}
			set
			{
				if (value < TextAlign.Left || value > TextAlign.Right)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["TextAlign"] = value;
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06003D7B RID: 15739 RVA: 0x00101960 File Offset: 0x00100960
		// (set) Token: 0x06003D7C RID: 15740 RVA: 0x0010198D File Offset: 0x0010098D
		[Themeable(false)]
		[DefaultValue("")]
		[WebSysDescription("PostBackControl_ValidationGroup")]
		[WebCategory("Behavior")]
		public virtual string ValidationGroup
		{
			get
			{
				string text = (string)this.ViewState["ValidationGroup"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ValidationGroup"] = value;
			}
		}

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x06003D7D RID: 15741 RVA: 0x001019A0 File Offset: 0x001009A0
		// (remove) Token: 0x06003D7E RID: 15742 RVA: 0x001019B3 File Offset: 0x001009B3
		[WebSysDescription("Control_OnServerCheckChanged")]
		[WebCategory("Action")]
		public event EventHandler CheckedChanged
		{
			add
			{
				base.Events.AddHandler(CheckBox.EventCheckedChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(CheckBox.EventCheckedChanged, value);
			}
		}

		// Token: 0x06003D7F RID: 15743 RVA: 0x001019C6 File Offset: 0x001009C6
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddDisplayInlineBlockIfNeeded(writer);
		}

		// Token: 0x06003D80 RID: 15744 RVA: 0x001019D0 File Offset: 0x001009D0
		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				Triplet triplet = (Triplet)savedState;
				base.LoadViewState(triplet.First);
				if (triplet.Second != null)
				{
					if (this._inputAttributesState == null)
					{
						this._inputAttributesState = new StateBag();
						this._inputAttributesState.TrackViewState();
					}
					this._inputAttributesState.LoadViewState(triplet.Second);
				}
				if (triplet.Third != null)
				{
					if (this._labelAttributesState == null)
					{
						this._labelAttributesState = new StateBag();
						this._labelAttributesState.TrackViewState();
					}
					this._labelAttributesState.LoadViewState(triplet.Second);
				}
			}
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x00101A64 File Offset: 0x00100A64
		protected virtual void OnCheckedChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[CheckBox.EventCheckedChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x00101A94 File Offset: 0x00100A94
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			bool autoPostBack = this.AutoPostBack;
			if (this.Page != null && base.IsEnabled)
			{
				this.Page.RegisterRequiresPostBack(this);
				if (autoPostBack)
				{
					this.Page.RegisterPostBackScript();
					this.Page.RegisterFocusScript();
					if (this.CausesValidation && this.Page.GetValidators(this.ValidationGroup).Count > 0)
					{
						this.Page.RegisterWebFormsScript();
					}
				}
			}
			if (!this.SaveCheckedViewState(autoPostBack))
			{
				this.ViewState.SetItemDirty("Checked", false);
				if (this.Page != null && base.IsEnabled)
				{
					this.Page.RegisterEnabledControl(this);
				}
			}
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x00101B48 File Offset: 0x00100B48
		protected override object SaveViewState()
		{
			object obj = base.SaveViewState();
			object obj2 = null;
			object obj3 = null;
			object obj4 = null;
			if (this._inputAttributesState != null)
			{
				obj2 = this._inputAttributesState.SaveViewState();
			}
			if (this._labelAttributesState != null)
			{
				obj3 = this._labelAttributesState.SaveViewState();
			}
			if (obj != null || obj2 != null || obj3 != null)
			{
				obj4 = new Triplet(obj, obj2, obj3);
			}
			return obj4;
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x00101B9D File Offset: 0x00100B9D
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this._inputAttributesState != null)
			{
				this._inputAttributesState.TrackViewState();
			}
			if (this._labelAttributesState != null)
			{
				this._labelAttributesState.TrackViewState();
			}
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x00101BCC File Offset: 0x00100BCC
		protected internal override void Render(HtmlTextWriter writer)
		{
			this.AddAttributesToRender(writer);
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			bool flag = false;
			if (base.ControlStyleCreated)
			{
				Style controlStyle = base.ControlStyle;
				if (!controlStyle.IsEmpty)
				{
					controlStyle.AddAttributesToRender(writer, this);
					flag = true;
				}
			}
			if (!base.IsEnabled)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
				flag = true;
			}
			string toolTip = this.ToolTip;
			if (toolTip.Length > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Title, toolTip);
				flag = true;
			}
			string text = null;
			if (base.HasAttributes)
			{
				AttributeCollection attributes = base.Attributes;
				string text2 = attributes["value"];
				if (text2 != null)
				{
					attributes.Remove("value");
				}
				text = attributes["onclick"];
				if (text != null)
				{
					text = Util.EnsureEndWithSemiColon(text);
					attributes.Remove("onclick");
				}
				if (attributes.Count != 0)
				{
					attributes.AddAttributes(writer);
					flag = true;
				}
				if (text2 != null)
				{
					attributes["value"] = text2;
				}
			}
			if (flag)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Span);
			}
			string text3 = this.Text;
			string clientID = this.ClientID;
			if (text3.Length != 0)
			{
				if (this.TextAlign == TextAlign.Left)
				{
					this.RenderLabel(writer, text3, clientID);
					this.RenderInputTag(writer, clientID, text);
				}
				else
				{
					this.RenderInputTag(writer, clientID, text);
					this.RenderLabel(writer, text3, clientID);
				}
			}
			else
			{
				this.RenderInputTag(writer, clientID, text);
			}
			if (flag)
			{
				writer.RenderEndTag();
			}
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x00101D30 File Offset: 0x00100D30
		private void RenderLabel(HtmlTextWriter writer, string text, string clientID)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.For, clientID);
			if (this._labelAttributes != null && this._labelAttributes.Count != 0)
			{
				this._labelAttributes.AddAttributes(writer);
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Label);
			writer.Write(text);
			writer.RenderEndTag();
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x00101D7C File Offset: 0x00100D7C
		internal virtual void RenderInputTag(HtmlTextWriter writer, string clientID, string onClick)
		{
			if (clientID != null)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, clientID);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
			if (this.UniqueID != null)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
			}
			if (this._valueAttribute != null)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Value, this._valueAttribute);
			}
			if (this.Checked)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
			}
			if (!base.IsEnabled)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
			}
			if (this.AutoPostBack && this.Page != null && this.Page.ClientSupportsJavaScript)
			{
				PostBackOptions postBackOptions = new PostBackOptions(this, string.Empty);
				if (this.CausesValidation && this.Page.GetValidators(this.ValidationGroup).Count > 0)
				{
					postBackOptions.PerformValidation = true;
					postBackOptions.ValidationGroup = this.ValidationGroup;
				}
				if (this.Page.Form != null)
				{
					postBackOptions.AutoPostBack = true;
				}
				onClick = Util.MergeScript(onClick, this.Page.ClientScript.GetPostBackEventReference(postBackOptions, true));
				writer.AddAttribute(HtmlTextWriterAttribute.Onclick, onClick);
				if (base.EnableLegacyRendering)
				{
					writer.AddAttribute("language", "javascript", false);
				}
			}
			else
			{
				if (this.Page != null)
				{
					this.Page.ClientScript.RegisterForEventValidation(this.UniqueID);
				}
				if (onClick != null)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Onclick, onClick);
				}
			}
			string accessKey = this.AccessKey;
			if (accessKey.Length > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Accesskey, accessKey);
			}
			int tabIndex = (int)this.TabIndex;
			if (tabIndex != 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Tabindex, tabIndex.ToString(NumberFormatInfo.InvariantInfo));
			}
			if (this._inputAttributes != null && this._inputAttributes.Count != 0)
			{
				this._inputAttributes.AddAttributes(writer);
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x00101F3F File Offset: 0x00100F3F
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			return this.LoadPostData(postDataKey, postCollection);
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x00101F4C File Offset: 0x00100F4C
		protected virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			string text = postCollection[postDataKey];
			bool flag = !string.IsNullOrEmpty(text);
			if (flag)
			{
				base.ValidateEvent(postDataKey);
			}
			bool flag2 = flag != this.Checked;
			this.Checked = flag;
			return flag2;
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x00101F8C File Offset: 0x00100F8C
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			this.RaisePostDataChangedEvent();
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x00101F94 File Offset: 0x00100F94
		protected virtual void RaisePostDataChangedEvent()
		{
			if (this.AutoPostBack && !this.Page.IsPostBackEventControlRegistered)
			{
				this.Page.AutoPostBackControl = this;
				if (this.CausesValidation)
				{
					this.Page.Validate(this.ValidationGroup);
				}
			}
			this.OnCheckedChanged(EventArgs.Empty);
		}

		// Token: 0x0400277E RID: 10110
		internal AttributeCollection _inputAttributes;

		// Token: 0x0400277F RID: 10111
		private StateBag _inputAttributesState;

		// Token: 0x04002780 RID: 10112
		private AttributeCollection _labelAttributes;

		// Token: 0x04002781 RID: 10113
		private StateBag _labelAttributesState;

		// Token: 0x04002782 RID: 10114
		private string _valueAttribute;

		// Token: 0x04002783 RID: 10115
		private static readonly object EventCheckedChanged = new object();
	}
}
