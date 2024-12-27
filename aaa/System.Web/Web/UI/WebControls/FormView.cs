using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI.WebControls.Adapters;
using System.Web.Util;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000581 RID: 1409
	[ControlValueProperty("SelectedValue")]
	[SupportsEventValidation]
	[DefaultEvent("PageIndexChanging")]
	[Designer("System.Web.UI.Design.WebControls.FormViewDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FormView : CompositeDataBoundControl, IDataItemContainer, INamingContainer, IPostBackEventHandler, IPostBackContainer
	{
		// Token: 0x170010E0 RID: 4320
		// (get) Token: 0x0600450F RID: 17679 RVA: 0x0011B948 File Offset: 0x0011A948
		// (set) Token: 0x06004510 RID: 17680 RVA: 0x0011B974 File Offset: 0x0011A974
		[WebSysDescription("FormView_AllowPaging")]
		[WebCategory("Paging")]
		[DefaultValue(false)]
		public virtual bool AllowPaging
		{
			get
			{
				object obj = this.ViewState["AllowPaging"];
				return obj != null && (bool)obj;
			}
			set
			{
				bool allowPaging = this.AllowPaging;
				if (value != allowPaging)
				{
					this.ViewState["AllowPaging"] = value;
					if (base.Initialized)
					{
						base.RequiresDataBinding = true;
					}
				}
			}
		}

		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x06004511 RID: 17681 RVA: 0x0011B9B1 File Offset: 0x0011A9B1
		// (set) Token: 0x06004512 RID: 17682 RVA: 0x0011B9D1 File Offset: 0x0011A9D1
		[WebSysDescription("WebControl_BackImageUrl")]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		public virtual string BackImageUrl
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return string.Empty;
				}
				return ((TableStyle)base.ControlStyle).BackImageUrl;
			}
			set
			{
				((TableStyle)base.ControlStyle).BackImageUrl = value;
			}
		}

		// Token: 0x170010E2 RID: 4322
		// (get) Token: 0x06004513 RID: 17683 RVA: 0x0011B9E4 File Offset: 0x0011A9E4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual FormViewRow BottomPagerRow
		{
			get
			{
				if (this._bottomPagerRow == null)
				{
					this.EnsureChildControls();
				}
				return this._bottomPagerRow;
			}
		}

		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x06004514 RID: 17684 RVA: 0x0011B9FC File Offset: 0x0011A9FC
		private IOrderedDictionary BoundFieldValues
		{
			get
			{
				if (this._boundFieldValues == null)
				{
					int num = 25;
					this._boundFieldValues = new OrderedDictionary(num);
				}
				return this._boundFieldValues;
			}
		}

		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x06004515 RID: 17685 RVA: 0x0011BA28 File Offset: 0x0011AA28
		// (set) Token: 0x06004516 RID: 17686 RVA: 0x0011BA55 File Offset: 0x0011AA55
		[Localizable(true)]
		[DefaultValue("")]
		[WebCategory("Accessibility")]
		[WebSysDescription("DataControls_Caption")]
		public virtual string Caption
		{
			get
			{
				string text = (string)this.ViewState["Caption"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				this.ViewState["Caption"] = value;
			}
		}

		// Token: 0x170010E5 RID: 4325
		// (get) Token: 0x06004517 RID: 17687 RVA: 0x0011BA68 File Offset: 0x0011AA68
		// (set) Token: 0x06004518 RID: 17688 RVA: 0x0011BA91 File Offset: 0x0011AA91
		[DefaultValue(TableCaptionAlign.NotSet)]
		[WebCategory("Accessibility")]
		[WebSysDescription("WebControl_CaptionAlign")]
		public virtual TableCaptionAlign CaptionAlign
		{
			get
			{
				object obj = this.ViewState["CaptionAlign"];
				if (obj == null)
				{
					return TableCaptionAlign.NotSet;
				}
				return (TableCaptionAlign)obj;
			}
			set
			{
				if (value < TableCaptionAlign.NotSet || value > TableCaptionAlign.Right)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["CaptionAlign"] = value;
			}
		}

		// Token: 0x170010E6 RID: 4326
		// (get) Token: 0x06004519 RID: 17689 RVA: 0x0011BABC File Offset: 0x0011AABC
		// (set) Token: 0x0600451A RID: 17690 RVA: 0x0011BAD8 File Offset: 0x0011AAD8
		[DefaultValue(-1)]
		[WebCategory("Layout")]
		[WebSysDescription("FormView_CellPadding")]
		public virtual int CellPadding
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return -1;
				}
				return ((TableStyle)base.ControlStyle).CellPadding;
			}
			set
			{
				((TableStyle)base.ControlStyle).CellPadding = value;
			}
		}

		// Token: 0x170010E7 RID: 4327
		// (get) Token: 0x0600451B RID: 17691 RVA: 0x0011BAEB File Offset: 0x0011AAEB
		// (set) Token: 0x0600451C RID: 17692 RVA: 0x0011BB07 File Offset: 0x0011AB07
		[DefaultValue(0)]
		[WebCategory("Layout")]
		[WebSysDescription("FormView_CellSpacing")]
		public virtual int CellSpacing
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return 0;
				}
				return ((TableStyle)base.ControlStyle).CellSpacing;
			}
			set
			{
				((TableStyle)base.ControlStyle).CellSpacing = value;
			}
		}

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x0600451D RID: 17693 RVA: 0x0011BB1A File Offset: 0x0011AB1A
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public FormViewMode CurrentMode
		{
			get
			{
				return this.Mode;
			}
		}

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x0600451E RID: 17694 RVA: 0x0011BB22 File Offset: 0x0011AB22
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual object DataItem
		{
			get
			{
				if (this.CurrentMode == FormViewMode.Insert)
				{
					return null;
				}
				return this._dataItem;
			}
		}

		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x0600451F RID: 17695 RVA: 0x0011BB35 File Offset: 0x0011AB35
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int DataItemCount
		{
			get
			{
				return this.PageCount;
			}
		}

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x06004520 RID: 17696 RVA: 0x0011BB3D File Offset: 0x0011AB3D
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int DataItemIndex
		{
			get
			{
				if (this.CurrentMode == FormViewMode.Insert)
				{
					return -1;
				}
				return this._dataItemIndex;
			}
		}

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x06004521 RID: 17697 RVA: 0x0011BB50 File Offset: 0x0011AB50
		// (set) Token: 0x06004522 RID: 17698 RVA: 0x0011BB80 File Offset: 0x0011AB80
		[WebSysDescription("DataControls_DataKeyNames")]
		[DefaultValue(null)]
		[Editor("System.Web.UI.Design.WebControls.DataFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[TypeConverter(typeof(StringArrayConverter))]
		[WebCategory("Data")]
		public virtual string[] DataKeyNames
		{
			get
			{
				object dataKeyNames = this._dataKeyNames;
				if (dataKeyNames != null)
				{
					return (string[])((string[])dataKeyNames).Clone();
				}
				return new string[0];
			}
			set
			{
				if (!DataBoundControlHelper.CompareStringArrays(value, this.DataKeyNamesInternal))
				{
					if (value != null)
					{
						this._dataKeyNames = (string[])value.Clone();
					}
					else
					{
						this._dataKeyNames = null;
					}
					this._keyTable = null;
					if (base.Initialized)
					{
						base.RequiresDataBinding = true;
					}
				}
			}
		}

		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x06004523 RID: 17699 RVA: 0x0011BBD0 File Offset: 0x0011ABD0
		private string[] DataKeyNamesInternal
		{
			get
			{
				object dataKeyNames = this._dataKeyNames;
				if (dataKeyNames != null)
				{
					return (string[])dataKeyNames;
				}
				return new string[0];
			}
		}

		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x06004524 RID: 17700 RVA: 0x0011BBF4 File Offset: 0x0011ABF4
		[WebSysDescription("FormView_DataKey")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual DataKey DataKey
		{
			get
			{
				if (this._dataKey == null)
				{
					this._dataKey = new DataKey(this.KeyTable);
				}
				return this._dataKey;
			}
		}

		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x06004525 RID: 17701 RVA: 0x0011BC15 File Offset: 0x0011AC15
		// (set) Token: 0x06004526 RID: 17702 RVA: 0x0011BC1D File Offset: 0x0011AC1D
		[DefaultValue(FormViewMode.ReadOnly)]
		[WebCategory("Behavior")]
		[WebSysDescription("View_DefaultMode")]
		public virtual FormViewMode DefaultMode
		{
			get
			{
				return this._defaultMode;
			}
			set
			{
				if (value < FormViewMode.ReadOnly || value > FormViewMode.Insert)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._defaultMode = value;
			}
		}

		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x06004527 RID: 17703 RVA: 0x0011BC39 File Offset: 0x0011AC39
		// (set) Token: 0x06004528 RID: 17704 RVA: 0x0011BC41 File Offset: 0x0011AC41
		[Browsable(false)]
		[WebSysDescription("FormView_EditItemTemplate")]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(FormView), BindingDirection.TwoWay)]
		public virtual ITemplate EditItemTemplate
		{
			get
			{
				return this._editItemTemplate;
			}
			set
			{
				this._editItemTemplate = value;
			}
		}

		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x06004529 RID: 17705 RVA: 0x0011BC4A File Offset: 0x0011AC4A
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("View_EditRowStyle")]
		public TableItemStyle EditRowStyle
		{
			get
			{
				if (this._editRowStyle == null)
				{
					this._editRowStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._editRowStyle).TrackViewState();
					}
				}
				return this._editRowStyle;
			}
		}

		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x0600452A RID: 17706 RVA: 0x0011BC78 File Offset: 0x0011AC78
		[DefaultValue(null)]
		[WebCategory("Styles")]
		[WebSysDescription("View_EmptyDataRowStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public TableItemStyle EmptyDataRowStyle
		{
			get
			{
				if (this._emptyDataRowStyle == null)
				{
					this._emptyDataRowStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._emptyDataRowStyle).TrackViewState();
					}
				}
				return this._emptyDataRowStyle;
			}
		}

		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x0600452B RID: 17707 RVA: 0x0011BCA6 File Offset: 0x0011ACA6
		// (set) Token: 0x0600452C RID: 17708 RVA: 0x0011BCAE File Offset: 0x0011ACAE
		[WebSysDescription("View_EmptyDataTemplate")]
		[Browsable(false)]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(FormView))]
		public virtual ITemplate EmptyDataTemplate
		{
			get
			{
				return this._emptyDataTemplate;
			}
			set
			{
				this._emptyDataTemplate = value;
			}
		}

		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x0600452D RID: 17709 RVA: 0x0011BCB8 File Offset: 0x0011ACB8
		// (set) Token: 0x0600452E RID: 17710 RVA: 0x0011BCE5 File Offset: 0x0011ACE5
		[DefaultValue("")]
		[WebSysDescription("View_EmptyDataText")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		public virtual string EmptyDataText
		{
			get
			{
				object obj = this.ViewState["EmptyDataText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["EmptyDataText"] = value;
			}
		}

		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x0600452F RID: 17711 RVA: 0x0011BCF8 File Offset: 0x0011ACF8
		// (set) Token: 0x06004530 RID: 17712 RVA: 0x0011BD21 File Offset: 0x0011AD21
		[WebSysDescription("DataBoundControl_EnableModelValidation")]
		[WebCategory("Behavior")]
		[DefaultValue(false)]
		public virtual bool EnableModelValidation
		{
			get
			{
				object obj = this.ViewState["EnableModelValidation"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["EnableModelValidation"] = value;
			}
		}

		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x06004531 RID: 17713 RVA: 0x0011BD3C File Offset: 0x0011AD3C
		// (set) Token: 0x06004532 RID: 17714 RVA: 0x0011BD65 File Offset: 0x0011AD65
		private int FirstDisplayedPageIndex
		{
			get
			{
				object obj = this.ViewState["FirstDisplayedPageIndex"];
				if (obj != null)
				{
					return (int)obj;
				}
				return -1;
			}
			set
			{
				this.ViewState["FirstDisplayedPageIndex"] = value;
			}
		}

		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x06004533 RID: 17715 RVA: 0x0011BD7D File Offset: 0x0011AD7D
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual FormViewRow FooterRow
		{
			get
			{
				if (this._footerRow == null)
				{
					this.EnsureChildControls();
				}
				return this._footerRow;
			}
		}

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x06004534 RID: 17716 RVA: 0x0011BD93 File Offset: 0x0011AD93
		[DefaultValue(null)]
		[WebCategory("Styles")]
		[WebSysDescription("FormView_FooterStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public TableItemStyle FooterStyle
		{
			get
			{
				if (this._footerStyle == null)
				{
					this._footerStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._footerStyle).TrackViewState();
					}
				}
				return this._footerStyle;
			}
		}

		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x06004535 RID: 17717 RVA: 0x0011BDC1 File Offset: 0x0011ADC1
		// (set) Token: 0x06004536 RID: 17718 RVA: 0x0011BDC9 File Offset: 0x0011ADC9
		[DefaultValue(null)]
		[WebSysDescription("FormView_FooterTemplate")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(FormView))]
		public virtual ITemplate FooterTemplate
		{
			get
			{
				return this._footerTemplate;
			}
			set
			{
				this._footerTemplate = value;
			}
		}

		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x06004537 RID: 17719 RVA: 0x0011BDD4 File Offset: 0x0011ADD4
		// (set) Token: 0x06004538 RID: 17720 RVA: 0x0011BE01 File Offset: 0x0011AE01
		[WebSysDescription("View_FooterText")]
		[DefaultValue("")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		public virtual string FooterText
		{
			get
			{
				object obj = this.ViewState["FooterText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["FooterText"] = value;
			}
		}

		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x06004539 RID: 17721 RVA: 0x0011BE14 File Offset: 0x0011AE14
		// (set) Token: 0x0600453A RID: 17722 RVA: 0x0011BE30 File Offset: 0x0011AE30
		[WebSysDescription("DataControls_GridLines")]
		[DefaultValue(GridLines.None)]
		[WebCategory("Appearance")]
		public virtual GridLines GridLines
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return GridLines.None;
				}
				return ((TableStyle)base.ControlStyle).GridLines;
			}
			set
			{
				((TableStyle)base.ControlStyle).GridLines = value;
			}
		}

		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x0600453B RID: 17723 RVA: 0x0011BE43 File Offset: 0x0011AE43
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual FormViewRow HeaderRow
		{
			get
			{
				if (this._headerRow == null)
				{
					this.EnsureChildControls();
				}
				return this._headerRow;
			}
		}

		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x0600453C RID: 17724 RVA: 0x0011BE59 File Offset: 0x0011AE59
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebSysDescription("WebControl_HeaderStyle")]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public TableItemStyle HeaderStyle
		{
			get
			{
				if (this._headerStyle == null)
				{
					this._headerStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._headerStyle).TrackViewState();
					}
				}
				return this._headerStyle;
			}
		}

		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x0600453D RID: 17725 RVA: 0x0011BE87 File Offset: 0x0011AE87
		// (set) Token: 0x0600453E RID: 17726 RVA: 0x0011BE8F File Offset: 0x0011AE8F
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(FormView))]
		[WebSysDescription("WebControl_HeaderTemplate")]
		[DefaultValue(null)]
		public virtual ITemplate HeaderTemplate
		{
			get
			{
				return this._headerTemplate;
			}
			set
			{
				this._headerTemplate = value;
			}
		}

		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x0600453F RID: 17727 RVA: 0x0011BE98 File Offset: 0x0011AE98
		// (set) Token: 0x06004540 RID: 17728 RVA: 0x0011BEC5 File Offset: 0x0011AEC5
		[Localizable(true)]
		[WebSysDescription("View_HeaderText")]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		public virtual string HeaderText
		{
			get
			{
				object obj = this.ViewState["HeaderText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["HeaderText"] = value;
			}
		}

		// Token: 0x17001100 RID: 4352
		// (get) Token: 0x06004541 RID: 17729 RVA: 0x0011BED8 File Offset: 0x0011AED8
		// (set) Token: 0x06004542 RID: 17730 RVA: 0x0011BEF4 File Offset: 0x0011AEF4
		[WebSysDescription("WebControl_HorizontalAlign")]
		[Category("Layout")]
		[DefaultValue(HorizontalAlign.NotSet)]
		public virtual HorizontalAlign HorizontalAlign
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return HorizontalAlign.NotSet;
				}
				return ((TableStyle)base.ControlStyle).HorizontalAlign;
			}
			set
			{
				((TableStyle)base.ControlStyle).HorizontalAlign = value;
			}
		}

		// Token: 0x17001101 RID: 4353
		// (get) Token: 0x06004543 RID: 17731 RVA: 0x0011BF07 File Offset: 0x0011AF07
		// (set) Token: 0x06004544 RID: 17732 RVA: 0x0011BF0F File Offset: 0x0011AF0F
		[TemplateContainer(typeof(FormView), BindingDirection.TwoWay)]
		[Browsable(false)]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("FormView_InsertItemTemplate")]
		public virtual ITemplate InsertItemTemplate
		{
			get
			{
				return this._insertItemTemplate;
			}
			set
			{
				this._insertItemTemplate = value;
			}
		}

		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x06004545 RID: 17733 RVA: 0x0011BF18 File Offset: 0x0011AF18
		[WebCategory("Styles")]
		[WebSysDescription("View_InsertRowStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public TableItemStyle InsertRowStyle
		{
			get
			{
				if (this._insertRowStyle == null)
				{
					this._insertRowStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._insertRowStyle).TrackViewState();
					}
				}
				return this._insertRowStyle;
			}
		}

		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x06004546 RID: 17734 RVA: 0x0011BF46 File Offset: 0x0011AF46
		// (set) Token: 0x06004547 RID: 17735 RVA: 0x0011BF4E File Offset: 0x0011AF4E
		[WebSysDescription("View_InsertRowStyle")]
		[TemplateContainer(typeof(FormView), BindingDirection.TwoWay)]
		[Browsable(false)]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual ITemplate ItemTemplate
		{
			get
			{
				return this._itemTemplate;
			}
			set
			{
				this._itemTemplate = value;
			}
		}

		// Token: 0x17001104 RID: 4356
		// (get) Token: 0x06004548 RID: 17736 RVA: 0x0011BF57 File Offset: 0x0011AF57
		private OrderedDictionary KeyTable
		{
			get
			{
				if (this._keyTable == null)
				{
					this._keyTable = new OrderedDictionary(this.DataKeyNamesInternal.Length);
				}
				return this._keyTable;
			}
		}

		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x06004549 RID: 17737 RVA: 0x0011BF7A File Offset: 0x0011AF7A
		// (set) Token: 0x0600454A RID: 17738 RVA: 0x0011BFA5 File Offset: 0x0011AFA5
		private FormViewMode Mode
		{
			get
			{
				if (!this._modeSet || base.DesignMode)
				{
					this._mode = this.DefaultMode;
					this._modeSet = true;
				}
				return this._mode;
			}
			set
			{
				if (value < FormViewMode.ReadOnly || value > FormViewMode.Insert)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._modeSet = true;
				if (this._mode != value)
				{
					this._mode = value;
					if (base.Initialized)
					{
						base.RequiresDataBinding = true;
					}
				}
			}
		}

		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x0600454B RID: 17739 RVA: 0x0011BFE0 File Offset: 0x0011AFE0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int PageCount
		{
			get
			{
				return this._pageCount;
			}
		}

		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x0600454C RID: 17740 RVA: 0x0011BFE8 File Offset: 0x0011AFE8
		// (set) Token: 0x0600454D RID: 17741 RVA: 0x0011BFF0 File Offset: 0x0011AFF0
		private int PageIndexInternal
		{
			get
			{
				return this._pageIndex;
			}
			set
			{
				int pageIndexInternal = this.PageIndexInternal;
				if (value != pageIndexInternal)
				{
					this._pageIndex = value;
					if (base.Initialized)
					{
						base.RequiresDataBinding = true;
					}
				}
			}
		}

		// Token: 0x17001108 RID: 4360
		// (get) Token: 0x0600454E RID: 17742 RVA: 0x0011C01E File Offset: 0x0011B01E
		// (set) Token: 0x0600454F RID: 17743 RVA: 0x0011C039 File Offset: 0x0011B039
		[WebSysDescription("FormView_PageIndex")]
		[Bindable(true)]
		[DefaultValue(0)]
		[WebCategory("Data")]
		public virtual int PageIndex
		{
			get
			{
				if (this.Mode == FormViewMode.Insert && !base.DesignMode)
				{
					return -1;
				}
				return this.PageIndexInternal;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value >= 0)
				{
					this.PageIndexInternal = value;
				}
			}
		}

		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x06004550 RID: 17744 RVA: 0x0011C058 File Offset: 0x0011B058
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("GridView_PagerSettings")]
		[WebCategory("Paging")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		public virtual PagerSettings PagerSettings
		{
			get
			{
				if (this._pagerSettings == null)
				{
					this._pagerSettings = new PagerSettings();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._pagerSettings).TrackViewState();
					}
					this._pagerSettings.PropertyChanged += this.OnPagerPropertyChanged;
				}
				return this._pagerSettings;
			}
		}

		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x06004551 RID: 17745 RVA: 0x0011C0A8 File Offset: 0x0011B0A8
		[WebSysDescription("WebControl_PagerStyle")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		public TableItemStyle PagerStyle
		{
			get
			{
				if (this._pagerStyle == null)
				{
					this._pagerStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._pagerStyle).TrackViewState();
					}
				}
				return this._pagerStyle;
			}
		}

		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x06004552 RID: 17746 RVA: 0x0011C0D6 File Offset: 0x0011B0D6
		// (set) Token: 0x06004553 RID: 17747 RVA: 0x0011C0DE File Offset: 0x0011B0DE
		[DefaultValue(null)]
		[WebSysDescription("View_PagerTemplate")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[Browsable(false)]
		[TemplateContainer(typeof(FormView))]
		public virtual ITemplate PagerTemplate
		{
			get
			{
				return this._pagerTemplate;
			}
			set
			{
				this._pagerTemplate = value;
			}
		}

		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x06004554 RID: 17748 RVA: 0x0011C0E7 File Offset: 0x0011B0E7
		[WebSysDescription("FormView_Rows")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual FormViewRow Row
		{
			get
			{
				if (this._row == null)
				{
					this.EnsureChildControls();
				}
				return this._row;
			}
		}

		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x06004555 RID: 17749 RVA: 0x0011C0FD File Offset: 0x0011B0FD
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		[NotifyParentProperty(true)]
		[WebSysDescription("View_RowStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TableItemStyle RowStyle
		{
			get
			{
				if (this._rowStyle == null)
				{
					this._rowStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._rowStyle).TrackViewState();
					}
				}
				return this._rowStyle;
			}
		}

		// Token: 0x1700110E RID: 4366
		// (get) Token: 0x06004556 RID: 17750 RVA: 0x0011C12B File Offset: 0x0011B12B
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedValue
		{
			get
			{
				return this.DataKey.Value;
			}
		}

		// Token: 0x1700110F RID: 4367
		// (get) Token: 0x06004557 RID: 17751 RVA: 0x0011C138 File Offset: 0x0011B138
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Table;
			}
		}

		// Token: 0x17001110 RID: 4368
		// (get) Token: 0x06004558 RID: 17752 RVA: 0x0011C13C File Offset: 0x0011B13C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual FormViewRow TopPagerRow
		{
			get
			{
				if (this._topPagerRow == null)
				{
					this.EnsureChildControls();
				}
				return this._topPagerRow;
			}
		}

		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x06004559 RID: 17753 RVA: 0x0011C152 File Offset: 0x0011B152
		// (remove) Token: 0x0600455A RID: 17754 RVA: 0x0011C165 File Offset: 0x0011B165
		[WebSysDescription("FormView_OnPageIndexChanged")]
		[WebCategory("Action")]
		public event EventHandler PageIndexChanged
		{
			add
			{
				base.Events.AddHandler(FormView.EventPageIndexChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(FormView.EventPageIndexChanged, value);
			}
		}

		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x0600455B RID: 17755 RVA: 0x0011C178 File Offset: 0x0011B178
		// (remove) Token: 0x0600455C RID: 17756 RVA: 0x0011C18B File Offset: 0x0011B18B
		[WebCategory("Action")]
		[WebSysDescription("FormView_OnPageIndexChanging")]
		public event FormViewPageEventHandler PageIndexChanging
		{
			add
			{
				base.Events.AddHandler(FormView.EventPageIndexChanging, value);
			}
			remove
			{
				base.Events.RemoveHandler(FormView.EventPageIndexChanging, value);
			}
		}

		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x0600455D RID: 17757 RVA: 0x0011C19E File Offset: 0x0011B19E
		// (remove) Token: 0x0600455E RID: 17758 RVA: 0x0011C1B1 File Offset: 0x0011B1B1
		[WebSysDescription("FormView_OnItemCommand")]
		[WebCategory("Action")]
		public event FormViewCommandEventHandler ItemCommand
		{
			add
			{
				base.Events.AddHandler(FormView.EventItemCommand, value);
			}
			remove
			{
				base.Events.RemoveHandler(FormView.EventItemCommand, value);
			}
		}

		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x0600455F RID: 17759 RVA: 0x0011C1C4 File Offset: 0x0011B1C4
		// (remove) Token: 0x06004560 RID: 17760 RVA: 0x0011C1D7 File Offset: 0x0011B1D7
		[WebCategory("Behavior")]
		[WebSysDescription("FormView_OnItemCreated")]
		public event EventHandler ItemCreated
		{
			add
			{
				base.Events.AddHandler(FormView.EventItemCreated, value);
			}
			remove
			{
				base.Events.RemoveHandler(FormView.EventItemCreated, value);
			}
		}

		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x06004561 RID: 17761 RVA: 0x0011C1EA File Offset: 0x0011B1EA
		// (remove) Token: 0x06004562 RID: 17762 RVA: 0x0011C1FD File Offset: 0x0011B1FD
		[WebSysDescription("DataControls_OnItemDeleted")]
		[WebCategory("Action")]
		public event FormViewDeletedEventHandler ItemDeleted
		{
			add
			{
				base.Events.AddHandler(FormView.EventItemDeleted, value);
			}
			remove
			{
				base.Events.RemoveHandler(FormView.EventItemDeleted, value);
			}
		}

		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x06004563 RID: 17763 RVA: 0x0011C210 File Offset: 0x0011B210
		// (remove) Token: 0x06004564 RID: 17764 RVA: 0x0011C223 File Offset: 0x0011B223
		[WebSysDescription("DataControls_OnItemDeleting")]
		[WebCategory("Action")]
		public event FormViewDeleteEventHandler ItemDeleting
		{
			add
			{
				base.Events.AddHandler(FormView.EventItemDeleting, value);
			}
			remove
			{
				base.Events.RemoveHandler(FormView.EventItemDeleting, value);
			}
		}

		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x06004565 RID: 17765 RVA: 0x0011C236 File Offset: 0x0011B236
		// (remove) Token: 0x06004566 RID: 17766 RVA: 0x0011C249 File Offset: 0x0011B249
		[WebCategory("Action")]
		[WebSysDescription("DataControls_OnItemInserted")]
		public event FormViewInsertedEventHandler ItemInserted
		{
			add
			{
				base.Events.AddHandler(FormView.EventItemInserted, value);
			}
			remove
			{
				base.Events.RemoveHandler(FormView.EventItemInserted, value);
			}
		}

		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x06004567 RID: 17767 RVA: 0x0011C25C File Offset: 0x0011B25C
		// (remove) Token: 0x06004568 RID: 17768 RVA: 0x0011C26F File Offset: 0x0011B26F
		[WebSysDescription("DataControls_OnItemInserting")]
		[WebCategory("Action")]
		public event FormViewInsertEventHandler ItemInserting
		{
			add
			{
				base.Events.AddHandler(FormView.EventItemInserting, value);
			}
			remove
			{
				base.Events.RemoveHandler(FormView.EventItemInserting, value);
			}
		}

		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x06004569 RID: 17769 RVA: 0x0011C282 File Offset: 0x0011B282
		// (remove) Token: 0x0600456A RID: 17770 RVA: 0x0011C295 File Offset: 0x0011B295
		[WebSysDescription("DataControls_OnItemUpdated")]
		[WebCategory("Action")]
		public event FormViewUpdatedEventHandler ItemUpdated
		{
			add
			{
				base.Events.AddHandler(FormView.EventItemUpdated, value);
			}
			remove
			{
				base.Events.RemoveHandler(FormView.EventItemUpdated, value);
			}
		}

		// Token: 0x140000BA RID: 186
		// (add) Token: 0x0600456B RID: 17771 RVA: 0x0011C2A8 File Offset: 0x0011B2A8
		// (remove) Token: 0x0600456C RID: 17772 RVA: 0x0011C2BB File Offset: 0x0011B2BB
		[WebCategory("Action")]
		[WebSysDescription("DataControls_OnItemUpdating")]
		public event FormViewUpdateEventHandler ItemUpdating
		{
			add
			{
				base.Events.AddHandler(FormView.EventItemUpdating, value);
			}
			remove
			{
				base.Events.RemoveHandler(FormView.EventItemUpdating, value);
			}
		}

		// Token: 0x140000BB RID: 187
		// (add) Token: 0x0600456D RID: 17773 RVA: 0x0011C2CE File Offset: 0x0011B2CE
		// (remove) Token: 0x0600456E RID: 17774 RVA: 0x0011C2E1 File Offset: 0x0011B2E1
		[WebSysDescription("FormView_OnModeChanged")]
		[WebCategory("Action")]
		public event EventHandler ModeChanged
		{
			add
			{
				base.Events.AddHandler(FormView.EventModeChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(FormView.EventModeChanged, value);
			}
		}

		// Token: 0x140000BC RID: 188
		// (add) Token: 0x0600456F RID: 17775 RVA: 0x0011C2F4 File Offset: 0x0011B2F4
		// (remove) Token: 0x06004570 RID: 17776 RVA: 0x0011C307 File Offset: 0x0011B307
		[WebCategory("Action")]
		[WebSysDescription("FormView_OnModeChanging")]
		public event FormViewModeEventHandler ModeChanging
		{
			add
			{
				base.Events.AddHandler(FormView.EventModeChanging, value);
			}
			remove
			{
				base.Events.RemoveHandler(FormView.EventModeChanging, value);
			}
		}

		// Token: 0x06004571 RID: 17777 RVA: 0x0011C31A File Offset: 0x0011B31A
		public void ChangeMode(FormViewMode newMode)
		{
			this.Mode = newMode;
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x0011C324 File Offset: 0x0011B324
		protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
		{
			PagedDataSource pagedDataSource = null;
			int num = this.PageIndex;
			bool allowPaging = this.AllowPaging;
			int num2 = 0;
			FormViewMode mode = this.Mode;
			if (base.DesignMode && mode == FormViewMode.Insert)
			{
				num = -1;
			}
			if (dataBinding)
			{
				DataSourceView data = this.GetData();
				DataSourceSelectArguments selectArguments = base.SelectArguments;
				if (data == null)
				{
					throw new HttpException(SR.GetString("DataBoundControl_NullView", new object[] { this.ID }));
				}
				if (mode != FormViewMode.Insert)
				{
					if (allowPaging && !data.CanPage && dataSource != null && !(dataSource is ICollection))
					{
						selectArguments.StartRowIndex = num;
						selectArguments.MaximumRows = 1;
						data.Select(selectArguments, new DataSourceViewSelectCallback(this.SelectCallback));
					}
					if (this._useServerPaging)
					{
						if (data.CanRetrieveTotalRowCount)
						{
							pagedDataSource = this.CreateServerPagedDataSource(selectArguments.TotalRowCount);
						}
						else
						{
							ICollection collection = dataSource as ICollection;
							if (collection == null)
							{
								throw new HttpException(SR.GetString("DataBoundControl_NeedICollectionOrTotalRowCount", new object[] { base.GetType().Name }));
							}
							pagedDataSource = this.CreateServerPagedDataSource(checked(this.PageIndex + collection.Count));
						}
					}
					else
					{
						pagedDataSource = this.CreatePagedDataSource();
					}
				}
			}
			else
			{
				pagedDataSource = this.CreatePagedDataSource();
			}
			if (mode != FormViewMode.Insert)
			{
				pagedDataSource.DataSource = dataSource;
			}
			IEnumerator enumerator = null;
			OrderedDictionary keyTable = this.KeyTable;
			if (!dataBinding)
			{
				enumerator = dataSource.GetEnumerator();
				ICollection collection2 = dataSource as ICollection;
				if (collection2 == null)
				{
					throw new HttpException(SR.GetString("DataControls_DataSourceMustBeCollectionWhenNotDataBinding"));
				}
				num2 = collection2.Count;
			}
			else
			{
				keyTable.Clear();
				if (dataSource != null)
				{
					if (mode != FormViewMode.Insert)
					{
						ICollection collection3 = dataSource as ICollection;
						if (collection3 == null && pagedDataSource.IsPagingEnabled && !pagedDataSource.IsServerPagingEnabled)
						{
							throw new HttpException(SR.GetString("FormView_DataSourceMustBeCollection", new object[] { this.ID }));
						}
						if (pagedDataSource.IsPagingEnabled)
						{
							num2 = pagedDataSource.DataSourceCount;
						}
						else if (collection3 != null)
						{
							num2 = collection3.Count;
						}
					}
					enumerator = dataSource.GetEnumerator();
				}
			}
			Table table = this.CreateTable();
			TableRowCollection rows = table.Rows;
			bool flag = false;
			object obj = null;
			this.Controls.Add(table);
			if (enumerator != null)
			{
				flag = enumerator.MoveNext();
			}
			if (!flag && mode != FormViewMode.Insert)
			{
				if (this.EmptyDataText.Length > 0 || this._emptyDataTemplate != null)
				{
					this._row = this.CreateRow(0, DataControlRowType.EmptyDataRow, DataControlRowState.Normal, rows, null);
				}
				num2 = 0;
			}
			else
			{
				int i = 0;
				if (!this._useServerPaging)
				{
					while (i < num)
					{
						obj = enumerator.Current;
						flag = enumerator.MoveNext();
						if (!flag)
						{
							this._pageIndex = i;
							pagedDataSource.CurrentPageIndex = i;
							num = i;
							break;
						}
						i++;
					}
				}
				if (flag)
				{
					this._dataItem = enumerator.Current;
				}
				else
				{
					this._dataItem = obj;
				}
				if ((!this._useServerPaging && !(dataSource is ICollection)) || (this._useServerPaging && num2 < 0))
				{
					num2 = i;
					while (flag)
					{
						num2++;
						flag = enumerator.MoveNext();
					}
				}
				this._dataItemIndex = i;
				bool flag2 = num2 <= 1 && !this._useServerPaging;
				if (allowPaging && this.PagerSettings.Visible && this._pagerSettings.IsPagerOnTop && mode != FormViewMode.Insert && !flag2)
				{
					this._topPagerRow = this.CreateRow(num, DataControlRowType.Pager, DataControlRowState.Normal, rows, pagedDataSource);
				}
				this._headerRow = this.CreateRow(num, DataControlRowType.Header, DataControlRowState.Normal, rows, null);
				if (this._headerTemplate == null && this.HeaderText.Length == 0)
				{
					this._headerRow.Visible = false;
				}
				this._row = this.CreateDataRow(dataBinding, rows, this._dataItem);
				if (num >= 0)
				{
					string[] dataKeyNamesInternal = this.DataKeyNamesInternal;
					if (dataBinding && dataKeyNamesInternal.Length != 0)
					{
						foreach (string text in dataKeyNamesInternal)
						{
							object propertyValue = DataBinder.GetPropertyValue(this._dataItem, text);
							keyTable.Add(text, propertyValue);
						}
						this._dataKey = new DataKey(keyTable);
					}
				}
				this._footerRow = this.CreateRow(num, DataControlRowType.Footer, DataControlRowState.Normal, rows, null);
				if (this._footerTemplate == null && this.FooterText.Length == 0)
				{
					this._footerRow.Visible = false;
				}
				if (allowPaging && this.PagerSettings.Visible && this._pagerSettings.IsPagerOnBottom && mode != FormViewMode.Insert && !flag2)
				{
					this._bottomPagerRow = this.CreateRow(num, DataControlRowType.Pager, DataControlRowState.Normal, rows, pagedDataSource);
				}
			}
			this._pageCount = num2;
			this.OnItemCreated(EventArgs.Empty);
			if (dataBinding)
			{
				this.DataBind(false);
			}
			return num2;
		}

		// Token: 0x06004573 RID: 17779 RVA: 0x0011C78C File Offset: 0x0011B78C
		protected override Style CreateControlStyle()
		{
			return new TableStyle
			{
				CellSpacing = 0
			};
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x0011C7A8 File Offset: 0x0011B7A8
		private FormViewRow CreateDataRow(bool dataBinding, TableRowCollection rows, object dataItem)
		{
			ITemplate template = null;
			switch (this.Mode)
			{
			case FormViewMode.ReadOnly:
				template = this._itemTemplate;
				break;
			case FormViewMode.Edit:
				template = this._editItemTemplate;
				break;
			case FormViewMode.Insert:
				if (this._insertItemTemplate != null)
				{
					template = this._insertItemTemplate;
				}
				else
				{
					template = this._editItemTemplate;
				}
				break;
			}
			if (template != null)
			{
				return this.CreateDataRowFromTemplates(dataBinding, rows);
			}
			return null;
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x0011C80C File Offset: 0x0011B80C
		private FormViewRow CreateDataRowFromTemplates(bool dataBinding, TableRowCollection rows)
		{
			int pageIndex = this.PageIndex;
			FormViewMode mode = this.Mode;
			DataControlRowState dataControlRowState = DataControlRowState.Normal;
			if (mode == FormViewMode.Edit)
			{
				dataControlRowState |= DataControlRowState.Edit;
			}
			else if (mode == FormViewMode.Insert)
			{
				dataControlRowState |= DataControlRowState.Insert;
			}
			return this.CreateRow(this.PageIndex, DataControlRowType.DataRow, dataControlRowState, rows, null);
		}

		// Token: 0x06004576 RID: 17782 RVA: 0x0011C850 File Offset: 0x0011B850
		protected override DataSourceSelectArguments CreateDataSourceSelectArguments()
		{
			DataSourceSelectArguments dataSourceSelectArguments = new DataSourceSelectArguments();
			DataSourceView data = this.GetData();
			this._useServerPaging = this.AllowPaging && data.CanPage;
			if (this._useServerPaging)
			{
				dataSourceSelectArguments.StartRowIndex = this.PageIndex;
				if (data.CanRetrieveTotalRowCount)
				{
					dataSourceSelectArguments.RetrieveTotalRowCount = true;
					dataSourceSelectArguments.MaximumRows = 1;
				}
				else
				{
					dataSourceSelectArguments.MaximumRows = -1;
				}
			}
			return dataSourceSelectArguments;
		}

		// Token: 0x06004577 RID: 17783 RVA: 0x0011C8B8 File Offset: 0x0011B8B8
		private void CreateNextPrevPager(TableRow row, PagedDataSource pagedDataSource, bool addFirstLastPageButtons)
		{
			PagerSettings pagerSettings = this.PagerSettings;
			string previousPageImageUrl = pagerSettings.PreviousPageImageUrl;
			string nextPageImageUrl = pagerSettings.NextPageImageUrl;
			bool isFirstPage = pagedDataSource.IsFirstPage;
			bool isLastPage = pagedDataSource.IsLastPage;
			if (addFirstLastPageButtons && !isFirstPage)
			{
				string firstPageImageUrl = pagerSettings.FirstPageImageUrl;
				TableCell tableCell = new TableCell();
				row.Cells.Add(tableCell);
				IButtonControl buttonControl;
				if (firstPageImageUrl.Length > 0)
				{
					buttonControl = new DataControlImageButton(this);
					((ImageButton)buttonControl).ImageUrl = firstPageImageUrl;
					((ImageButton)buttonControl).AlternateText = HttpUtility.HtmlDecode(pagerSettings.FirstPageText);
				}
				else
				{
					buttonControl = new DataControlPagerLinkButton(this);
					((DataControlPagerLinkButton)buttonControl).Text = pagerSettings.FirstPageText;
				}
				buttonControl.CommandName = "Page";
				buttonControl.CommandArgument = "First";
				tableCell.Controls.Add((Control)buttonControl);
			}
			if (!isFirstPage)
			{
				TableCell tableCell2 = new TableCell();
				row.Cells.Add(tableCell2);
				IButtonControl buttonControl2;
				if (previousPageImageUrl.Length > 0)
				{
					buttonControl2 = new DataControlImageButton(this);
					((ImageButton)buttonControl2).ImageUrl = previousPageImageUrl;
					((ImageButton)buttonControl2).AlternateText = HttpUtility.HtmlDecode(pagerSettings.PreviousPageText);
				}
				else
				{
					buttonControl2 = new DataControlPagerLinkButton(this);
					((DataControlPagerLinkButton)buttonControl2).Text = pagerSettings.PreviousPageText;
				}
				buttonControl2.CommandName = "Page";
				buttonControl2.CommandArgument = "Prev";
				tableCell2.Controls.Add((Control)buttonControl2);
			}
			if (!isLastPage)
			{
				TableCell tableCell3 = new TableCell();
				row.Cells.Add(tableCell3);
				IButtonControl buttonControl3;
				if (nextPageImageUrl.Length > 0)
				{
					buttonControl3 = new DataControlImageButton(this);
					((ImageButton)buttonControl3).ImageUrl = nextPageImageUrl;
					((ImageButton)buttonControl3).AlternateText = HttpUtility.HtmlDecode(pagerSettings.NextPageText);
				}
				else
				{
					buttonControl3 = new DataControlPagerLinkButton(this);
					((DataControlPagerLinkButton)buttonControl3).Text = pagerSettings.NextPageText;
				}
				buttonControl3.CommandName = "Page";
				buttonControl3.CommandArgument = "Next";
				tableCell3.Controls.Add((Control)buttonControl3);
			}
			if (addFirstLastPageButtons && !isLastPage)
			{
				string lastPageImageUrl = pagerSettings.LastPageImageUrl;
				TableCell tableCell4 = new TableCell();
				row.Cells.Add(tableCell4);
				IButtonControl buttonControl4;
				if (lastPageImageUrl.Length > 0)
				{
					buttonControl4 = new DataControlImageButton(this);
					((ImageButton)buttonControl4).ImageUrl = lastPageImageUrl;
					((ImageButton)buttonControl4).AlternateText = HttpUtility.HtmlDecode(pagerSettings.LastPageText);
				}
				else
				{
					buttonControl4 = new DataControlPagerLinkButton(this);
					((DataControlPagerLinkButton)buttonControl4).Text = pagerSettings.LastPageText;
				}
				buttonControl4.CommandName = "Page";
				buttonControl4.CommandArgument = "Last";
				tableCell4.Controls.Add((Control)buttonControl4);
			}
		}

		// Token: 0x06004578 RID: 17784 RVA: 0x0011CB68 File Offset: 0x0011BB68
		private void CreateNumericPager(TableRow row, PagedDataSource pagedDataSource, bool addFirstLastPageButtons)
		{
			PagerSettings pagerSettings = this.PagerSettings;
			int pageCount = pagedDataSource.PageCount;
			int num = pagedDataSource.CurrentPageIndex + 1;
			int pageButtonCount = pagerSettings.PageButtonCount;
			int num2 = pageButtonCount;
			int num3 = this.FirstDisplayedPageIndex + 1;
			if (pageCount < num2)
			{
				num2 = pageCount;
			}
			int num4 = 1;
			int num5 = num2;
			if (num > num5)
			{
				int num6 = (num - 1) / pageButtonCount;
				bool flag = num - num3 >= 0 && num - num3 < pageButtonCount;
				if (num3 > 0 && flag)
				{
					num4 = num3;
				}
				else
				{
					num4 = num6 * pageButtonCount + 1;
				}
				num5 = num4 + pageButtonCount - 1;
				if (num5 > pageCount)
				{
					num5 = pageCount;
				}
				if (num5 - num4 + 1 < pageButtonCount)
				{
					num4 = Math.Max(1, num5 - pageButtonCount + 1);
				}
				this.FirstDisplayedPageIndex = num4 - 1;
			}
			if (addFirstLastPageButtons && num != 1 && num4 != 1)
			{
				string firstPageImageUrl = pagerSettings.FirstPageImageUrl;
				TableCell tableCell = new TableCell();
				row.Cells.Add(tableCell);
				IButtonControl buttonControl;
				if (firstPageImageUrl.Length > 0)
				{
					buttonControl = new DataControlImageButton(this);
					((ImageButton)buttonControl).ImageUrl = firstPageImageUrl;
					((ImageButton)buttonControl).AlternateText = HttpUtility.HtmlDecode(pagerSettings.FirstPageText);
				}
				else
				{
					buttonControl = new DataControlPagerLinkButton(this);
					((DataControlPagerLinkButton)buttonControl).Text = pagerSettings.FirstPageText;
				}
				buttonControl.CommandName = "Page";
				buttonControl.CommandArgument = "First";
				tableCell.Controls.Add((Control)buttonControl);
			}
			if (num4 != 1)
			{
				TableCell tableCell2 = new TableCell();
				row.Cells.Add(tableCell2);
				LinkButton linkButton = new DataControlPagerLinkButton(this);
				linkButton.Text = "...";
				linkButton.CommandName = "Page";
				linkButton.CommandArgument = (num4 - 1).ToString(NumberFormatInfo.InvariantInfo);
				tableCell2.Controls.Add(linkButton);
			}
			for (int i = num4; i <= num5; i++)
			{
				TableCell tableCell3 = new TableCell();
				row.Cells.Add(tableCell3);
				string text = i.ToString(NumberFormatInfo.InvariantInfo);
				if (i == num)
				{
					Label label = new Label();
					label.Text = text;
					tableCell3.Controls.Add(label);
				}
				else
				{
					LinkButton linkButton = new DataControlPagerLinkButton(this);
					linkButton.Text = text;
					linkButton.CommandName = "Page";
					linkButton.CommandArgument = text;
					tableCell3.Controls.Add(linkButton);
				}
			}
			if (pageCount > num5)
			{
				TableCell tableCell4 = new TableCell();
				row.Cells.Add(tableCell4);
				LinkButton linkButton = new DataControlPagerLinkButton(this);
				linkButton.Text = "...";
				linkButton.CommandName = "Page";
				linkButton.CommandArgument = (num5 + 1).ToString(NumberFormatInfo.InvariantInfo);
				tableCell4.Controls.Add(linkButton);
			}
			bool flag2 = num5 == pageCount;
			if (addFirstLastPageButtons && num != pageCount && !flag2)
			{
				string lastPageImageUrl = pagerSettings.LastPageImageUrl;
				TableCell tableCell5 = new TableCell();
				row.Cells.Add(tableCell5);
				IButtonControl buttonControl2;
				if (lastPageImageUrl.Length > 0)
				{
					buttonControl2 = new DataControlImageButton(this);
					((ImageButton)buttonControl2).ImageUrl = lastPageImageUrl;
					((ImageButton)buttonControl2).AlternateText = HttpUtility.HtmlDecode(pagerSettings.LastPageText);
				}
				else
				{
					buttonControl2 = new DataControlPagerLinkButton(this);
					((DataControlPagerLinkButton)buttonControl2).Text = pagerSettings.LastPageText;
				}
				buttonControl2.CommandName = "Page";
				buttonControl2.CommandArgument = "Last";
				tableCell5.Controls.Add((Control)buttonControl2);
			}
		}

		// Token: 0x06004579 RID: 17785 RVA: 0x0011CED4 File Offset: 0x0011BED4
		private PagedDataSource CreatePagedDataSource()
		{
			return new PagedDataSource
			{
				CurrentPageIndex = this.PageIndex,
				PageSize = 1,
				AllowPaging = this.AllowPaging,
				AllowCustomPaging = false,
				AllowServerPaging = false,
				VirtualCount = 0
			};
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x0011CF1C File Offset: 0x0011BF1C
		private PagedDataSource CreateServerPagedDataSource(int totalRowCount)
		{
			return new PagedDataSource
			{
				CurrentPageIndex = this.PageIndex,
				PageSize = 1,
				AllowPaging = this.AllowPaging,
				AllowCustomPaging = false,
				AllowServerPaging = true,
				VirtualCount = totalRowCount
			};
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x0011CF64 File Offset: 0x0011BF64
		private FormViewRow CreateRow(int itemIndex, DataControlRowType rowType, DataControlRowState rowState, TableRowCollection rows, PagedDataSource pagedDataSource)
		{
			FormViewRow formViewRow = this.CreateRow(itemIndex, rowType, rowState);
			rows.Add(formViewRow);
			if (rowType != DataControlRowType.Pager)
			{
				this.InitializeRow(formViewRow);
			}
			else
			{
				this.InitializePager(formViewRow, pagedDataSource);
			}
			return formViewRow;
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x0011CF9B File Offset: 0x0011BF9B
		protected virtual FormViewRow CreateRow(int itemIndex, DataControlRowType rowType, DataControlRowState rowState)
		{
			if (rowType == DataControlRowType.Pager)
			{
				return new FormViewPagerRow(itemIndex, rowType, rowState);
			}
			return new FormViewRow(itemIndex, rowType, rowState);
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x0011CFB2 File Offset: 0x0011BFB2
		protected virtual Table CreateTable()
		{
			return new ChildTable(string.IsNullOrEmpty(this.ID) ? null : this.ClientID);
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x0011CFCF File Offset: 0x0011BFCF
		public sealed override void DataBind()
		{
			base.DataBind();
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x0011CFD7 File Offset: 0x0011BFD7
		public virtual void DeleteItem()
		{
			this.HandleDelete(string.Empty);
		}

		// Token: 0x06004580 RID: 17792 RVA: 0x0011CFE4 File Offset: 0x0011BFE4
		protected override void EnsureDataBound()
		{
			if (base.RequiresDataBinding && this.Mode == FormViewMode.Insert)
			{
				this.OnDataBinding(EventArgs.Empty);
				base.RequiresDataBinding = false;
				base.MarkAsDataBound();
				if (this._adapter != null)
				{
					DataBoundControlAdapter dataBoundControlAdapter = this._adapter as DataBoundControlAdapter;
					if (dataBoundControlAdapter != null)
					{
						dataBoundControlAdapter.PerformDataBinding(null);
					}
					else
					{
						this.PerformDataBinding(null);
					}
				}
				else
				{
					this.PerformDataBinding(null);
				}
				this.OnDataBound(EventArgs.Empty);
				return;
			}
			base.EnsureDataBound();
		}

		// Token: 0x06004581 RID: 17793 RVA: 0x0011D05C File Offset: 0x0011C05C
		protected virtual void ExtractRowValues(IOrderedDictionary fieldValues, bool includeKeys)
		{
			if (fieldValues == null)
			{
				return;
			}
			DataBoundControlHelper.ExtractValuesFromBindableControls(fieldValues, this);
			IBindableTemplate bindableTemplate = null;
			if (this.Mode == FormViewMode.ReadOnly && this.ItemTemplate != null)
			{
				bindableTemplate = this.ItemTemplate as IBindableTemplate;
			}
			else if ((this.Mode == FormViewMode.Edit || (this.Mode == FormViewMode.Insert && this.InsertItemTemplate == null)) && this.EditItemTemplate != null)
			{
				bindableTemplate = this.EditItemTemplate as IBindableTemplate;
			}
			else if (this.Mode == FormViewMode.Insert && this.InsertItemTemplate != null)
			{
				bindableTemplate = this.InsertItemTemplate as IBindableTemplate;
			}
			string[] dataKeyNamesInternal = this.DataKeyNamesInternal;
			if (bindableTemplate != null && this != null && bindableTemplate != null)
			{
				foreach (object obj in bindableTemplate.ExtractValues(this))
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if (includeKeys || Array.IndexOf(dataKeyNamesInternal, dictionaryEntry.Key) == -1)
					{
						fieldValues[dictionaryEntry.Key] = dictionaryEntry.Value;
					}
				}
			}
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x0011D168 File Offset: 0x0011C168
		private void HandleCancel()
		{
			bool isBoundUsingDataSourceID = base.IsBoundUsingDataSourceID;
			FormViewModeEventArgs formViewModeEventArgs = new FormViewModeEventArgs(this.DefaultMode, true);
			this.OnModeChanging(formViewModeEventArgs);
			if (formViewModeEventArgs.Cancel)
			{
				return;
			}
			if (isBoundUsingDataSourceID)
			{
				this.Mode = formViewModeEventArgs.NewMode;
				this.OnModeChanged(EventArgs.Empty);
			}
			base.RequiresDataBinding = true;
		}

		// Token: 0x06004583 RID: 17795 RVA: 0x0011D1BC File Offset: 0x0011C1BC
		private void HandleDelete(string commandArg)
		{
			int pageIndex = this.PageIndex;
			if (pageIndex < 0)
			{
				return;
			}
			DataSourceView dataSourceView = null;
			int pageIndex2 = this.PageIndex;
			bool isBoundUsingDataSourceID = base.IsBoundUsingDataSourceID;
			if (isBoundUsingDataSourceID)
			{
				dataSourceView = this.GetData();
				if (dataSourceView == null)
				{
					throw new HttpException(SR.GetString("View_DataSourceReturnedNullView", new object[] { this.ID }));
				}
			}
			FormViewDeleteEventArgs formViewDeleteEventArgs = new FormViewDeleteEventArgs(pageIndex2);
			if (isBoundUsingDataSourceID)
			{
				this.ExtractRowValues(formViewDeleteEventArgs.Values, false);
				foreach (object obj in this.DataKey.Values)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					formViewDeleteEventArgs.Keys.Add(dictionaryEntry.Key, dictionaryEntry.Value);
					if (formViewDeleteEventArgs.Values.Contains(dictionaryEntry.Key))
					{
						formViewDeleteEventArgs.Values.Remove(dictionaryEntry.Key);
					}
				}
			}
			this.OnItemDeleting(formViewDeleteEventArgs);
			if (formViewDeleteEventArgs.Cancel)
			{
				return;
			}
			if (isBoundUsingDataSourceID)
			{
				this._deleteKeys = formViewDeleteEventArgs.Keys;
				this._deleteValues = formViewDeleteEventArgs.Values;
				dataSourceView.Delete(formViewDeleteEventArgs.Keys, formViewDeleteEventArgs.Values, new DataSourceViewOperationCallback(this.HandleDeleteCallback));
			}
		}

		// Token: 0x06004584 RID: 17796 RVA: 0x0011D318 File Offset: 0x0011C318
		private bool HandleDeleteCallback(int affectedRows, Exception ex)
		{
			int pageIndex = this.PageIndex;
			FormViewDeletedEventArgs formViewDeletedEventArgs = new FormViewDeletedEventArgs(affectedRows, ex);
			formViewDeletedEventArgs.SetKeys(this._deleteKeys);
			formViewDeletedEventArgs.SetValues(this._deleteValues);
			this.OnItemDeleted(formViewDeletedEventArgs);
			this._deleteKeys = null;
			this._deleteValues = null;
			if (ex != null && !formViewDeletedEventArgs.ExceptionHandled && this.PageIsValidAfterModelException())
			{
				return false;
			}
			if (pageIndex == this._pageCount - 1)
			{
				this.HandlePage(pageIndex - 1);
			}
			base.RequiresDataBinding = true;
			return true;
		}

		// Token: 0x06004585 RID: 17797 RVA: 0x0011D394 File Offset: 0x0011C394
		private void HandleEdit()
		{
			if (this.PageIndex < 0)
			{
				return;
			}
			FormViewModeEventArgs formViewModeEventArgs = new FormViewModeEventArgs(FormViewMode.Edit, false);
			this.OnModeChanging(formViewModeEventArgs);
			if (formViewModeEventArgs.Cancel)
			{
				return;
			}
			if (base.IsBoundUsingDataSourceID)
			{
				this.Mode = formViewModeEventArgs.NewMode;
				this.OnModeChanged(EventArgs.Empty);
			}
			base.RequiresDataBinding = true;
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x0011D3EC File Offset: 0x0011C3EC
		private bool HandleEvent(EventArgs e, bool causesValidation, string validationGroup)
		{
			bool flag = false;
			this._modelValidationGroup = null;
			if (causesValidation && this.Page != null)
			{
				this.Page.Validate(validationGroup);
				if (this.EnableModelValidation)
				{
					this._modelValidationGroup = validationGroup;
				}
			}
			FormViewCommandEventArgs formViewCommandEventArgs = e as FormViewCommandEventArgs;
			if (formViewCommandEventArgs != null)
			{
				this.OnItemCommand(formViewCommandEventArgs);
				flag = true;
				string commandName = formViewCommandEventArgs.CommandName;
				int num = this.PageIndex;
				if (StringUtil.EqualsIgnoreCase(commandName, "Page"))
				{
					string text = (string)formViewCommandEventArgs.CommandArgument;
					if (StringUtil.EqualsIgnoreCase(text, "Next"))
					{
						num++;
					}
					else if (StringUtil.EqualsIgnoreCase(text, "Prev"))
					{
						num--;
					}
					else if (StringUtil.EqualsIgnoreCase(text, "First"))
					{
						num = 0;
					}
					else if (StringUtil.EqualsIgnoreCase(text, "Last"))
					{
						num = this.PageCount - 1;
					}
					else
					{
						num = Convert.ToInt32(text, CultureInfo.InvariantCulture) - 1;
					}
					this.HandlePage(num);
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "Edit"))
				{
					this.HandleEdit();
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "Update"))
				{
					this.HandleUpdate((string)formViewCommandEventArgs.CommandArgument, causesValidation);
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "Cancel"))
				{
					this.HandleCancel();
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "Delete"))
				{
					this.HandleDelete((string)formViewCommandEventArgs.CommandArgument);
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "Insert"))
				{
					this.HandleInsert((string)formViewCommandEventArgs.CommandArgument, causesValidation);
				}
				else if (StringUtil.EqualsIgnoreCase(commandName, "New"))
				{
					this.HandleNew();
				}
				else
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x06004587 RID: 17799 RVA: 0x0011D57C File Offset: 0x0011C57C
		private void HandleInsert(string commandArg, bool causesValidation)
		{
			if (causesValidation && this.Page != null && !this.Page.IsValid)
			{
				return;
			}
			if (this.Mode != FormViewMode.Insert)
			{
				throw new HttpException(SR.GetString("DetailsViewFormView_ControlMustBeInInsertMode", new object[] { "FormView", this.ID }));
			}
			DataSourceView dataSourceView = null;
			bool isBoundUsingDataSourceID = base.IsBoundUsingDataSourceID;
			if (isBoundUsingDataSourceID)
			{
				dataSourceView = this.GetData();
				if (dataSourceView == null)
				{
					throw new HttpException(SR.GetString("View_DataSourceReturnedNullView", new object[] { this.ID }));
				}
			}
			FormViewInsertEventArgs formViewInsertEventArgs = new FormViewInsertEventArgs(commandArg);
			if (isBoundUsingDataSourceID)
			{
				this.ExtractRowValues(formViewInsertEventArgs.Values, true);
			}
			this.OnItemInserting(formViewInsertEventArgs);
			if (formViewInsertEventArgs.Cancel)
			{
				return;
			}
			if (isBoundUsingDataSourceID)
			{
				this._insertValues = formViewInsertEventArgs.Values;
				dataSourceView.Insert(formViewInsertEventArgs.Values, new DataSourceViewOperationCallback(this.HandleInsertCallback));
			}
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x0011D65C File Offset: 0x0011C65C
		private bool HandleInsertCallback(int affectedRows, Exception ex)
		{
			FormViewInsertedEventArgs formViewInsertedEventArgs = new FormViewInsertedEventArgs(affectedRows, ex);
			formViewInsertedEventArgs.SetValues(this._insertValues);
			this.OnItemInserted(formViewInsertedEventArgs);
			this._insertValues = null;
			if (ex != null && !formViewInsertedEventArgs.ExceptionHandled)
			{
				if (this.PageIsValidAfterModelException())
				{
					return false;
				}
				formViewInsertedEventArgs.KeepInInsertMode = true;
			}
			if (!formViewInsertedEventArgs.KeepInInsertMode)
			{
				FormViewModeEventArgs formViewModeEventArgs = new FormViewModeEventArgs(this.DefaultMode, false);
				this.OnModeChanging(formViewModeEventArgs);
				if (!formViewModeEventArgs.Cancel)
				{
					this.Mode = formViewModeEventArgs.NewMode;
					this.OnModeChanged(EventArgs.Empty);
					base.RequiresDataBinding = true;
				}
			}
			return true;
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x0011D6EC File Offset: 0x0011C6EC
		private void HandleNew()
		{
			FormViewModeEventArgs formViewModeEventArgs = new FormViewModeEventArgs(FormViewMode.Insert, false);
			this.OnModeChanging(formViewModeEventArgs);
			if (formViewModeEventArgs.Cancel)
			{
				return;
			}
			if (base.IsBoundUsingDataSourceID)
			{
				this.Mode = formViewModeEventArgs.NewMode;
				this.OnModeChanged(EventArgs.Empty);
			}
			base.RequiresDataBinding = true;
		}

		// Token: 0x0600458A RID: 17802 RVA: 0x0011D738 File Offset: 0x0011C738
		private void HandlePage(int newPage)
		{
			if (!this.AllowPaging)
			{
				return;
			}
			if (this.PageIndex < 0)
			{
				return;
			}
			bool isBoundUsingDataSourceID = base.IsBoundUsingDataSourceID;
			FormViewPageEventArgs formViewPageEventArgs = new FormViewPageEventArgs(newPage);
			this.OnPageIndexChanging(formViewPageEventArgs);
			if (formViewPageEventArgs.Cancel)
			{
				return;
			}
			if (isBoundUsingDataSourceID)
			{
				if (formViewPageEventArgs.NewPageIndex <= -1)
				{
					return;
				}
				if (formViewPageEventArgs.NewPageIndex >= this.PageCount && this._pageIndex == this.PageCount - 1)
				{
					return;
				}
				this._keyTable = null;
				this._pageIndex = formViewPageEventArgs.NewPageIndex;
			}
			this.OnPageIndexChanged(EventArgs.Empty);
			base.RequiresDataBinding = true;
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x0011D7CC File Offset: 0x0011C7CC
		private void HandleUpdate(string commandArg, bool causesValidation)
		{
			if (causesValidation && this.Page != null && !this.Page.IsValid)
			{
				return;
			}
			if (this.Mode != FormViewMode.Edit)
			{
				throw new HttpException(SR.GetString("DetailsViewFormView_ControlMustBeInEditMode", new object[] { "FormView", this.ID }));
			}
			if (this.PageIndex < 0)
			{
				return;
			}
			DataSourceView dataSourceView = null;
			bool isBoundUsingDataSourceID = base.IsBoundUsingDataSourceID;
			if (isBoundUsingDataSourceID)
			{
				dataSourceView = this.GetData();
				if (dataSourceView == null)
				{
					throw new HttpException(SR.GetString("View_DataSourceReturnedNullView", new object[] { this.ID }));
				}
			}
			FormViewUpdateEventArgs formViewUpdateEventArgs = new FormViewUpdateEventArgs(commandArg);
			if (isBoundUsingDataSourceID)
			{
				foreach (object obj in this.BoundFieldValues)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					formViewUpdateEventArgs.OldValues.Add(dictionaryEntry.Key, dictionaryEntry.Value);
				}
				this.ExtractRowValues(formViewUpdateEventArgs.NewValues, true);
				foreach (object obj2 in this.DataKey.Values)
				{
					DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
					formViewUpdateEventArgs.Keys.Add(dictionaryEntry2.Key, dictionaryEntry2.Value);
				}
			}
			this.OnItemUpdating(formViewUpdateEventArgs);
			if (formViewUpdateEventArgs.Cancel)
			{
				return;
			}
			if (isBoundUsingDataSourceID)
			{
				this._updateKeys = formViewUpdateEventArgs.Keys;
				this._updateOldValues = formViewUpdateEventArgs.OldValues;
				this._updateNewValues = formViewUpdateEventArgs.NewValues;
				dataSourceView.Update(formViewUpdateEventArgs.Keys, formViewUpdateEventArgs.NewValues, formViewUpdateEventArgs.OldValues, new DataSourceViewOperationCallback(this.HandleUpdateCallback));
			}
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x0011D9B0 File Offset: 0x0011C9B0
		private bool HandleUpdateCallback(int affectedRows, Exception ex)
		{
			FormViewUpdatedEventArgs formViewUpdatedEventArgs = new FormViewUpdatedEventArgs(affectedRows, ex);
			formViewUpdatedEventArgs.SetOldValues(this._updateOldValues);
			formViewUpdatedEventArgs.SetNewValues(this._updateNewValues);
			formViewUpdatedEventArgs.SetKeys(this._updateKeys);
			this.OnItemUpdated(formViewUpdatedEventArgs);
			this._updateKeys = null;
			this._updateOldValues = null;
			this._updateNewValues = null;
			if (ex != null && !formViewUpdatedEventArgs.ExceptionHandled)
			{
				if (this.PageIsValidAfterModelException())
				{
					return false;
				}
				formViewUpdatedEventArgs.KeepInEditMode = true;
			}
			if (!formViewUpdatedEventArgs.KeepInEditMode)
			{
				FormViewModeEventArgs formViewModeEventArgs = new FormViewModeEventArgs(this.DefaultMode, false);
				this.OnModeChanging(formViewModeEventArgs);
				if (!formViewModeEventArgs.Cancel)
				{
					this.Mode = formViewModeEventArgs.NewMode;
					this.OnModeChanged(EventArgs.Empty);
					base.RequiresDataBinding = true;
				}
			}
			return true;
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x0011DA64 File Offset: 0x0011CA64
		protected virtual void InitializePager(FormViewRow row, PagedDataSource pagedDataSource)
		{
			TableCell tableCell = new TableCell();
			PagerSettings pagerSettings = this.PagerSettings;
			if (this._pagerTemplate != null)
			{
				this._pagerTemplate.InstantiateIn(tableCell);
			}
			else
			{
				PagerTable pagerTable = new PagerTable();
				TableRow tableRow = new TableRow();
				tableCell.Controls.Add(pagerTable);
				pagerTable.Rows.Add(tableRow);
				switch (pagerSettings.Mode)
				{
				case PagerButtons.NextPrevious:
					this.CreateNextPrevPager(tableRow, pagedDataSource, false);
					break;
				case PagerButtons.Numeric:
					this.CreateNumericPager(tableRow, pagedDataSource, false);
					break;
				case PagerButtons.NextPreviousFirstLast:
					this.CreateNextPrevPager(tableRow, pagedDataSource, true);
					break;
				case PagerButtons.NumericFirstLast:
					this.CreateNumericPager(tableRow, pagedDataSource, true);
					break;
				}
			}
			tableCell.ColumnSpan = 2;
			row.Cells.Add(tableCell);
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x0011DB18 File Offset: 0x0011CB18
		protected virtual void InitializeRow(FormViewRow row)
		{
			TableCellCollection cells = row.Cells;
			TableCell tableCell = new TableCell();
			ITemplate template = this._itemTemplate;
			int itemIndex = row.ItemIndex;
			DataControlRowState rowState = row.RowState;
			switch (row.RowType)
			{
			case DataControlRowType.Header:
			{
				template = this._headerTemplate;
				tableCell.ColumnSpan = 2;
				string headerText = this.HeaderText;
				if (this._headerTemplate == null && headerText.Length > 0)
				{
					tableCell.Text = headerText;
				}
				break;
			}
			case DataControlRowType.Footer:
			{
				template = this._footerTemplate;
				tableCell.ColumnSpan = 2;
				string footerText = this.FooterText;
				if (this._footerTemplate == null && footerText.Length > 0)
				{
					tableCell.Text = footerText;
				}
				break;
			}
			case DataControlRowType.DataRow:
				tableCell.ColumnSpan = 2;
				if ((rowState & DataControlRowState.Edit) != DataControlRowState.Normal && this._editItemTemplate != null)
				{
					template = this._editItemTemplate;
				}
				if ((rowState & DataControlRowState.Insert) != DataControlRowState.Normal)
				{
					if (this._insertItemTemplate != null)
					{
						template = this._insertItemTemplate;
					}
					else
					{
						template = this._editItemTemplate;
					}
				}
				break;
			case DataControlRowType.EmptyDataRow:
			{
				template = this._emptyDataTemplate;
				string emptyDataText = this.EmptyDataText;
				if (this._emptyDataTemplate == null && emptyDataText.Length > 0)
				{
					tableCell.Text = emptyDataText;
				}
				break;
			}
			}
			if (template != null)
			{
				template.InstantiateIn(tableCell);
			}
			cells.Add(tableCell);
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x0011DC55 File Offset: 0x0011CC55
		public virtual void InsertItem(bool causesValidation)
		{
			this.HandleInsert(string.Empty, causesValidation);
		}

		// Token: 0x06004590 RID: 17808 RVA: 0x0011DC63 File Offset: 0x0011CC63
		public virtual bool IsBindableType(Type type)
		{
			return DataBoundControlHelper.IsBindableType(type);
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x0011DC6C File Offset: 0x0011CC6C
		protected internal override void LoadControlState(object savedState)
		{
			this._pageIndex = 0;
			this._defaultMode = FormViewMode.ReadOnly;
			this._dataKeyNames = new string[0];
			this._pageCount = 0;
			object[] array = savedState as object[];
			if (array != null)
			{
				base.LoadControlState(array[0]);
				if (array[1] != null)
				{
					this._pageIndex = (int)array[1];
				}
				if (array[2] != null)
				{
					this._defaultMode = (FormViewMode)array[2];
				}
				if (array[3] != null)
				{
					this.Mode = (FormViewMode)array[3];
				}
				if (array[4] != null)
				{
					this._dataKeyNames = (string[])array[4];
				}
				if (array[5] != null)
				{
					this.KeyTable.Clear();
					OrderedDictionaryStateHelper.LoadViewState(this.KeyTable, (ArrayList)array[5]);
				}
				if (array[6] != null)
				{
					this._pageCount = (int)array[6];
					return;
				}
			}
			else
			{
				base.LoadControlState(null);
			}
		}

		// Token: 0x06004592 RID: 17810 RVA: 0x0011DD3C File Offset: 0x0011CD3C
		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				object[] array = (object[])savedState;
				base.LoadViewState(array[0]);
				if (array[1] != null)
				{
					((IStateManager)this.PagerStyle).LoadViewState(array[1]);
				}
				if (array[2] != null)
				{
					((IStateManager)this.HeaderStyle).LoadViewState(array[2]);
				}
				if (array[3] != null)
				{
					((IStateManager)this.FooterStyle).LoadViewState(array[3]);
				}
				if (array[4] != null)
				{
					((IStateManager)this.RowStyle).LoadViewState(array[4]);
				}
				if (array[5] != null)
				{
					((IStateManager)this.EditRowStyle).LoadViewState(array[5]);
				}
				if (array[6] != null)
				{
					((IStateManager)this.InsertRowStyle).LoadViewState(array[6]);
				}
				if (array[7] != null)
				{
					OrderedDictionaryStateHelper.LoadViewState((OrderedDictionary)this.BoundFieldValues, (ArrayList)array[7]);
				}
				if (array[8] != null)
				{
					((IStateManager)this.PagerSettings).LoadViewState(array[8]);
				}
				if (array[9] != null)
				{
					((IStateManager)base.ControlStyle).LoadViewState(array[9]);
					return;
				}
			}
			else
			{
				base.LoadViewState(null);
			}
		}

		// Token: 0x06004593 RID: 17811 RVA: 0x0011DE20 File Offset: 0x0011CE20
		protected override bool OnBubbleEvent(object source, EventArgs e)
		{
			bool flag = false;
			string text = string.Empty;
			FormViewCommandEventArgs formViewCommandEventArgs = e as FormViewCommandEventArgs;
			if (formViewCommandEventArgs != null)
			{
				IButtonControl buttonControl = formViewCommandEventArgs.CommandSource as IButtonControl;
				if (buttonControl != null)
				{
					flag = buttonControl.CausesValidation;
					text = buttonControl.ValidationGroup;
				}
			}
			return this.HandleEvent(e, flag, text);
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x0011DE68 File Offset: 0x0011CE68
		protected virtual void OnPageIndexChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[FormView.EventPageIndexChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004595 RID: 17813 RVA: 0x0011DE98 File Offset: 0x0011CE98
		protected virtual void OnPageIndexChanging(FormViewPageEventArgs e)
		{
			bool isBoundUsingDataSourceID = base.IsBoundUsingDataSourceID;
			FormViewPageEventHandler formViewPageEventHandler = (FormViewPageEventHandler)base.Events[FormView.EventPageIndexChanging];
			if (formViewPageEventHandler != null)
			{
				formViewPageEventHandler(this, e);
				return;
			}
			if (!isBoundUsingDataSourceID && !e.Cancel)
			{
				throw new HttpException(SR.GetString("FormView_UnhandledEvent", new object[] { this.ID, "PageIndexChanging" }));
			}
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x0011DF02 File Offset: 0x0011CF02
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (this.Page != null)
			{
				if (this.DataKeyNames.Length > 0)
				{
					this.Page.RegisterRequiresViewStateEncryption();
				}
				this.Page.RegisterRequiresControlState(this);
			}
		}

		// Token: 0x06004597 RID: 17815 RVA: 0x0011DF38 File Offset: 0x0011CF38
		protected virtual void OnItemCommand(FormViewCommandEventArgs e)
		{
			FormViewCommandEventHandler formViewCommandEventHandler = (FormViewCommandEventHandler)base.Events[FormView.EventItemCommand];
			if (formViewCommandEventHandler != null)
			{
				formViewCommandEventHandler(this, e);
			}
		}

		// Token: 0x06004598 RID: 17816 RVA: 0x0011DF68 File Offset: 0x0011CF68
		protected virtual void OnItemCreated(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[FormView.EventItemCreated];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x0011DF98 File Offset: 0x0011CF98
		protected virtual void OnItemDeleted(FormViewDeletedEventArgs e)
		{
			FormViewDeletedEventHandler formViewDeletedEventHandler = (FormViewDeletedEventHandler)base.Events[FormView.EventItemDeleted];
			if (formViewDeletedEventHandler != null)
			{
				formViewDeletedEventHandler(this, e);
			}
		}

		// Token: 0x0600459A RID: 17818 RVA: 0x0011DFC8 File Offset: 0x0011CFC8
		protected virtual void OnItemDeleting(FormViewDeleteEventArgs e)
		{
			bool isBoundUsingDataSourceID = base.IsBoundUsingDataSourceID;
			FormViewDeleteEventHandler formViewDeleteEventHandler = (FormViewDeleteEventHandler)base.Events[FormView.EventItemDeleting];
			if (formViewDeleteEventHandler != null)
			{
				formViewDeleteEventHandler(this, e);
				return;
			}
			if (!isBoundUsingDataSourceID && !e.Cancel)
			{
				throw new HttpException(SR.GetString("FormView_UnhandledEvent", new object[] { this.ID, "ItemDeleting" }));
			}
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x0011E034 File Offset: 0x0011D034
		protected virtual void OnItemInserted(FormViewInsertedEventArgs e)
		{
			FormViewInsertedEventHandler formViewInsertedEventHandler = (FormViewInsertedEventHandler)base.Events[FormView.EventItemInserted];
			if (formViewInsertedEventHandler != null)
			{
				formViewInsertedEventHandler(this, e);
			}
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x0011E064 File Offset: 0x0011D064
		protected virtual void OnItemInserting(FormViewInsertEventArgs e)
		{
			bool isBoundUsingDataSourceID = base.IsBoundUsingDataSourceID;
			FormViewInsertEventHandler formViewInsertEventHandler = (FormViewInsertEventHandler)base.Events[FormView.EventItemInserting];
			if (formViewInsertEventHandler != null)
			{
				formViewInsertEventHandler(this, e);
				return;
			}
			if (!isBoundUsingDataSourceID && !e.Cancel)
			{
				throw new HttpException(SR.GetString("FormView_UnhandledEvent", new object[] { this.ID, "ItemInserting" }));
			}
		}

		// Token: 0x0600459D RID: 17821 RVA: 0x0011E0D0 File Offset: 0x0011D0D0
		protected virtual void OnItemUpdated(FormViewUpdatedEventArgs e)
		{
			FormViewUpdatedEventHandler formViewUpdatedEventHandler = (FormViewUpdatedEventHandler)base.Events[FormView.EventItemUpdated];
			if (formViewUpdatedEventHandler != null)
			{
				formViewUpdatedEventHandler(this, e);
			}
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x0011E100 File Offset: 0x0011D100
		protected virtual void OnItemUpdating(FormViewUpdateEventArgs e)
		{
			bool isBoundUsingDataSourceID = base.IsBoundUsingDataSourceID;
			FormViewUpdateEventHandler formViewUpdateEventHandler = (FormViewUpdateEventHandler)base.Events[FormView.EventItemUpdating];
			if (formViewUpdateEventHandler != null)
			{
				formViewUpdateEventHandler(this, e);
				return;
			}
			if (!isBoundUsingDataSourceID && !e.Cancel)
			{
				throw new HttpException(SR.GetString("FormView_UnhandledEvent", new object[] { this.ID, "ItemUpdating" }));
			}
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x0011E16C File Offset: 0x0011D16C
		protected virtual void OnModeChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[FormView.EventModeChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x0011E19C File Offset: 0x0011D19C
		protected virtual void OnModeChanging(FormViewModeEventArgs e)
		{
			bool isBoundUsingDataSourceID = base.IsBoundUsingDataSourceID;
			FormViewModeEventHandler formViewModeEventHandler = (FormViewModeEventHandler)base.Events[FormView.EventModeChanging];
			if (formViewModeEventHandler != null)
			{
				formViewModeEventHandler(this, e);
				return;
			}
			if (!isBoundUsingDataSourceID && !e.Cancel)
			{
				throw new HttpException(SR.GetString("FormView_UnhandledEvent", new object[] { this.ID, "ModeChanging" }));
			}
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x0011E206 File Offset: 0x0011D206
		private void OnPagerPropertyChanged(object sender, EventArgs e)
		{
			if (base.Initialized)
			{
				base.RequiresDataBinding = true;
			}
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x0011E217 File Offset: 0x0011D217
		private bool PageIsValidAfterModelException()
		{
			if (this._modelValidationGroup == null)
			{
				return true;
			}
			this.Page.Validate(this._modelValidationGroup);
			return this.Page.IsValid;
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x0011E23F File Offset: 0x0011D23F
		protected internal override void PerformDataBinding(IEnumerable data)
		{
			base.PerformDataBinding(data);
			if (base.IsBoundUsingDataSourceID && this.Mode == FormViewMode.Edit && base.IsViewStateEnabled)
			{
				this.ExtractRowValues(this.BoundFieldValues, false);
			}
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x0011E270 File Offset: 0x0011D270
		protected internal virtual void PrepareControlHierarchy()
		{
			if (this.Controls.Count < 1)
			{
				return;
			}
			Table table = (Table)this.Controls[0];
			table.CopyBaseAttributes(this);
			if (base.ControlStyleCreated && !base.ControlStyle.IsEmpty)
			{
				table.ApplyStyle(base.ControlStyle);
			}
			else
			{
				table.GridLines = GridLines.None;
				table.CellSpacing = 0;
			}
			table.Caption = this.Caption;
			table.CaptionAlign = this.CaptionAlign;
			TableRowCollection rows = table.Rows;
			foreach (object obj in rows)
			{
				FormViewRow formViewRow = (FormViewRow)obj;
				Style style = new TableItemStyle();
				DataControlRowState rowState = formViewRow.RowState;
				switch (formViewRow.RowType)
				{
				case DataControlRowType.Header:
					style = this._headerStyle;
					break;
				case DataControlRowType.Footer:
					style = this._footerStyle;
					break;
				case DataControlRowType.DataRow:
					style.CopyFrom(this._rowStyle);
					if ((rowState & DataControlRowState.Edit) != DataControlRowState.Normal)
					{
						style.CopyFrom(this._editRowStyle);
					}
					if ((rowState & DataControlRowState.Insert) != DataControlRowState.Normal)
					{
						if (this._insertRowStyle != null)
						{
							style.CopyFrom(this._insertRowStyle);
						}
						else
						{
							style.CopyFrom(this._editRowStyle);
						}
					}
					break;
				case DataControlRowType.Pager:
					style = this._pagerStyle;
					break;
				case DataControlRowType.EmptyDataRow:
					style = this._emptyDataRowStyle;
					break;
				}
				if (style != null && formViewRow.Visible)
				{
					formViewRow.MergeStyle(style);
				}
			}
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x0011E3FC File Offset: 0x0011D3FC
		protected virtual void RaisePostBackEvent(string eventArgument)
		{
			base.ValidateEvent(this.UniqueID, eventArgument);
			int num = eventArgument.IndexOf('$');
			if (num < 0)
			{
				return;
			}
			CommandEventArgs commandEventArgs = new CommandEventArgs(eventArgument.Substring(0, num), eventArgument.Substring(num + 1));
			FormViewCommandEventArgs formViewCommandEventArgs = new FormViewCommandEventArgs(this, commandEventArgs);
			this.HandleEvent(formViewCommandEventArgs, false, string.Empty);
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x0011E451 File Offset: 0x0011D451
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			this.PrepareControlHierarchy();
			this.RenderContents(writer);
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x0011E474 File Offset: 0x0011D474
		protected internal override object SaveControlState()
		{
			object obj = base.SaveControlState();
			if (obj != null || this._pageIndex != 0 || this._mode != this._defaultMode || this._defaultMode != FormViewMode.ReadOnly || (this._dataKeyNames != null && this._dataKeyNames.Length > 0) || (this._keyTable != null && this._keyTable.Count > 0) || this._pageCount != 0)
			{
				object[] array = new object[7];
				object obj2 = null;
				object obj3 = null;
				object obj4 = null;
				object obj5 = null;
				object obj6 = null;
				object obj7 = null;
				if (this._pageIndex != 0)
				{
					obj2 = this._pageIndex;
				}
				if (this._defaultMode != FormViewMode.ReadOnly)
				{
					obj4 = (int)this._defaultMode;
				}
				if (this._mode != this._defaultMode && this._modeSet)
				{
					obj3 = (int)this._mode;
				}
				if (this._dataKeyNames != null && this._dataKeyNames.Length > 0)
				{
					obj5 = this._dataKeyNames;
				}
				if (this._keyTable != null)
				{
					obj6 = OrderedDictionaryStateHelper.SaveViewState(this._keyTable);
				}
				if (this._pageCount != 0)
				{
					obj7 = this._pageCount;
				}
				array[0] = obj;
				array[1] = obj2;
				array[2] = obj4;
				array[3] = obj3;
				array[4] = obj5;
				array[5] = obj6;
				array[6] = obj7;
				return array;
			}
			return true;
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x0011E5AC File Offset: 0x0011D5AC
		protected override object SaveViewState()
		{
			object obj = base.SaveViewState();
			object obj2 = ((this._pagerStyle != null) ? ((IStateManager)this._pagerStyle).SaveViewState() : null);
			object obj3 = ((this._headerStyle != null) ? ((IStateManager)this._headerStyle).SaveViewState() : null);
			object obj4 = ((this._footerStyle != null) ? ((IStateManager)this._footerStyle).SaveViewState() : null);
			object obj5 = ((this._rowStyle != null) ? ((IStateManager)this._rowStyle).SaveViewState() : null);
			object obj6 = ((this._editRowStyle != null) ? ((IStateManager)this._editRowStyle).SaveViewState() : null);
			object obj7 = ((this._insertRowStyle != null) ? ((IStateManager)this._insertRowStyle).SaveViewState() : null);
			object obj8 = ((this._boundFieldValues != null) ? OrderedDictionaryStateHelper.SaveViewState(this._boundFieldValues) : null);
			object obj9 = ((this._pagerSettings != null) ? ((IStateManager)this._pagerSettings).SaveViewState() : null);
			object obj10 = (base.ControlStyleCreated ? ((IStateManager)base.ControlStyle).SaveViewState() : null);
			return new object[] { obj, obj2, obj3, obj4, obj5, obj6, obj7, obj8, obj9, obj10 };
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x0011E6D9 File Offset: 0x0011D6D9
		private void SelectCallback(IEnumerable data)
		{
			throw new HttpException(SR.GetString("DataBoundControl_DataSourceDoesntSupportPaging"));
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x0011E6EC File Offset: 0x0011D6EC
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this._pagerStyle != null)
			{
				((IStateManager)this._pagerStyle).TrackViewState();
			}
			if (this._headerStyle != null)
			{
				((IStateManager)this._headerStyle).TrackViewState();
			}
			if (this._footerStyle != null)
			{
				((IStateManager)this._footerStyle).TrackViewState();
			}
			if (this._rowStyle != null)
			{
				((IStateManager)this._rowStyle).TrackViewState();
			}
			if (this._editRowStyle != null)
			{
				((IStateManager)this._editRowStyle).TrackViewState();
			}
			if (this._insertRowStyle != null)
			{
				((IStateManager)this._insertRowStyle).TrackViewState();
			}
			if (this._pagerSettings != null)
			{
				((IStateManager)this._pagerSettings).TrackViewState();
			}
			if (base.ControlStyleCreated)
			{
				((IStateManager)base.ControlStyle).TrackViewState();
			}
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x0011E797 File Offset: 0x0011D797
		public virtual void UpdateItem(bool causesValidation)
		{
			this.HandleUpdate(string.Empty, causesValidation);
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x0011E7A8 File Offset: 0x0011D7A8
		PostBackOptions IPostBackContainer.GetPostBackOptions(IButtonControl buttonControl)
		{
			if (buttonControl == null)
			{
				throw new ArgumentNullException("buttonControl");
			}
			if (buttonControl.CausesValidation)
			{
				throw new InvalidOperationException(SR.GetString("CannotUseParentPostBackWhenValidating", new object[]
				{
					base.GetType().Name,
					this.ID
				}));
			}
			return new PostBackOptions(this, buttonControl.CommandName + "$" + buttonControl.CommandArgument)
			{
				RequiresJavaScriptProtocol = true
			};
		}

		// Token: 0x060045AD RID: 17837 RVA: 0x0011E81F File Offset: 0x0011D81F
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			this.RaisePostBackEvent(eventArgument);
		}

		// Token: 0x17001111 RID: 4369
		// (get) Token: 0x060045AE RID: 17838 RVA: 0x0011E828 File Offset: 0x0011D828
		int IDataItemContainer.DataItemIndex
		{
			get
			{
				return this.DataItemIndex;
			}
		}

		// Token: 0x17001112 RID: 4370
		// (get) Token: 0x060045AF RID: 17839 RVA: 0x0011E830 File Offset: 0x0011D830
		int IDataItemContainer.DisplayIndex
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x040029DA RID: 10714
		private static readonly object EventPageIndexChanged = new object();

		// Token: 0x040029DB RID: 10715
		private static readonly object EventPageIndexChanging = new object();

		// Token: 0x040029DC RID: 10716
		private static readonly object EventItemCommand = new object();

		// Token: 0x040029DD RID: 10717
		private static readonly object EventItemCreated = new object();

		// Token: 0x040029DE RID: 10718
		private static readonly object EventItemDeleted = new object();

		// Token: 0x040029DF RID: 10719
		private static readonly object EventItemDeleting = new object();

		// Token: 0x040029E0 RID: 10720
		private static readonly object EventItemInserting = new object();

		// Token: 0x040029E1 RID: 10721
		private static readonly object EventItemInserted = new object();

		// Token: 0x040029E2 RID: 10722
		private static readonly object EventItemUpdating = new object();

		// Token: 0x040029E3 RID: 10723
		private static readonly object EventItemUpdated = new object();

		// Token: 0x040029E4 RID: 10724
		private static readonly object EventModeChanged = new object();

		// Token: 0x040029E5 RID: 10725
		private static readonly object EventModeChanging = new object();

		// Token: 0x040029E6 RID: 10726
		private ITemplate _itemTemplate;

		// Token: 0x040029E7 RID: 10727
		private ITemplate _editItemTemplate;

		// Token: 0x040029E8 RID: 10728
		private ITemplate _insertItemTemplate;

		// Token: 0x040029E9 RID: 10729
		private ITemplate _headerTemplate;

		// Token: 0x040029EA RID: 10730
		private ITemplate _footerTemplate;

		// Token: 0x040029EB RID: 10731
		private ITemplate _pagerTemplate;

		// Token: 0x040029EC RID: 10732
		private ITemplate _emptyDataTemplate;

		// Token: 0x040029ED RID: 10733
		private TableItemStyle _rowStyle;

		// Token: 0x040029EE RID: 10734
		private TableItemStyle _headerStyle;

		// Token: 0x040029EF RID: 10735
		private TableItemStyle _footerStyle;

		// Token: 0x040029F0 RID: 10736
		private TableItemStyle _editRowStyle;

		// Token: 0x040029F1 RID: 10737
		private TableItemStyle _insertRowStyle;

		// Token: 0x040029F2 RID: 10738
		private TableItemStyle _emptyDataRowStyle;

		// Token: 0x040029F3 RID: 10739
		private FormViewRow _bottomPagerRow;

		// Token: 0x040029F4 RID: 10740
		private FormViewRow _footerRow;

		// Token: 0x040029F5 RID: 10741
		private FormViewRow _headerRow;

		// Token: 0x040029F6 RID: 10742
		private FormViewRow _topPagerRow;

		// Token: 0x040029F7 RID: 10743
		private FormViewRow _row;

		// Token: 0x040029F8 RID: 10744
		private TableItemStyle _pagerStyle;

		// Token: 0x040029F9 RID: 10745
		private PagerSettings _pagerSettings;

		// Token: 0x040029FA RID: 10746
		private int _pageCount;

		// Token: 0x040029FB RID: 10747
		private object _dataItem;

		// Token: 0x040029FC RID: 10748
		private int _dataItemIndex;

		// Token: 0x040029FD RID: 10749
		private OrderedDictionary _boundFieldValues;

		// Token: 0x040029FE RID: 10750
		private DataKey _dataKey;

		// Token: 0x040029FF RID: 10751
		private OrderedDictionary _keyTable;

		// Token: 0x04002A00 RID: 10752
		private string[] _dataKeyNames;

		// Token: 0x04002A01 RID: 10753
		private int _pageIndex;

		// Token: 0x04002A02 RID: 10754
		private FormViewMode _defaultMode;

		// Token: 0x04002A03 RID: 10755
		private FormViewMode _mode;

		// Token: 0x04002A04 RID: 10756
		private bool _modeSet;

		// Token: 0x04002A05 RID: 10757
		private bool _useServerPaging;

		// Token: 0x04002A06 RID: 10758
		private string _modelValidationGroup;

		// Token: 0x04002A07 RID: 10759
		private IOrderedDictionary _deleteKeys;

		// Token: 0x04002A08 RID: 10760
		private IOrderedDictionary _deleteValues;

		// Token: 0x04002A09 RID: 10761
		private IOrderedDictionary _insertValues;

		// Token: 0x04002A0A RID: 10762
		private IOrderedDictionary _updateKeys;

		// Token: 0x04002A0B RID: 10763
		private IOrderedDictionary _updateOldValues;

		// Token: 0x04002A0C RID: 10764
		private IOrderedDictionary _updateNewValues;
	}
}
