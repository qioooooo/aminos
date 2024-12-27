using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200050E RID: 1294
	[ToolboxData("<{0}:Wizard runat=\"server\"> <WizardSteps> <asp:WizardStep title=\"Step 1\" runat=\"server\"></asp:WizardStep> <asp:WizardStep title=\"Step 2\" runat=\"server\"></asp:WizardStep> </WizardSteps> </{0}:Wizard>")]
	[Bindable(false)]
	[DefaultEvent("FinishButtonClick")]
	[Designer("System.Web.UI.Design.WebControls.WizardDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class Wizard : CompositeControl
	{
		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x06003F0D RID: 16141 RVA: 0x00105EEA File Offset: 0x00104EEA
		[WebSysDescription("Wizard_ActiveStep")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public WizardStepBase ActiveStep
		{
			get
			{
				if (this.ActiveStepIndex < -1 || this.ActiveStepIndex >= this.WizardSteps.Count)
				{
					throw new InvalidOperationException(SR.GetString("Wizard_ActiveStepIndex_out_of_range"));
				}
				return this.MultiView.GetActiveView() as WizardStepBase;
			}
		}

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x06003F0E RID: 16142 RVA: 0x00105F28 File Offset: 0x00104F28
		// (set) Token: 0x06003F0F RID: 16143 RVA: 0x00105F38 File Offset: 0x00104F38
		[DefaultValue(-1)]
		[Themeable(false)]
		[WebCategory("Behavior")]
		[WebSysDescription("Wizard_ActiveStepIndex")]
		public virtual int ActiveStepIndex
		{
			get
			{
				return this.MultiView.ActiveViewIndex;
			}
			set
			{
				if (value < -1 || (value >= this.WizardSteps.Count && base.ControlState >= ControlState.FrameworkInitialized))
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("Wizard_ActiveStepIndex_out_of_range"));
				}
				if (this.MultiView.ActiveViewIndex != value)
				{
					this.MultiView.ActiveViewIndex = value;
					this._activeStepIndexSet = true;
					if (this._sideBarDataList != null && this.SideBarTemplate != null)
					{
						this._sideBarDataList.SelectedIndex = this.ActiveStepIndex;
						this._sideBarDataList.DataBind();
					}
				}
			}
		}

		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x06003F10 RID: 16144 RVA: 0x00105FC4 File Offset: 0x00104FC4
		// (set) Token: 0x06003F11 RID: 16145 RVA: 0x00105FF1 File Offset: 0x00104FF1
		[WebSysDescription("Wizard_CancelButtonImageUrl")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebCategory("Appearance")]
		[UrlProperty]
		public virtual string CancelButtonImageUrl
		{
			get
			{
				object obj = this.ViewState["CancelButtonImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["CancelButtonImageUrl"] = value;
			}
		}

		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x06003F12 RID: 16146 RVA: 0x00106004 File Offset: 0x00105004
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[DefaultValue(null)]
		[WebCategory("Styles")]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("Wizard_CancelButtonStyle")]
		public Style CancelButtonStyle
		{
			get
			{
				if (this._cancelButtonStyle == null)
				{
					this._cancelButtonStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._cancelButtonStyle).TrackViewState();
					}
				}
				return this._cancelButtonStyle;
			}
		}

		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x06003F13 RID: 16147 RVA: 0x00106034 File Offset: 0x00105034
		// (set) Token: 0x06003F14 RID: 16148 RVA: 0x00106066 File Offset: 0x00105066
		[Localizable(true)]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("Wizard_Default_CancelButtonText")]
		[WebSysDescription("Wizard_CancelButtonText")]
		public virtual string CancelButtonText
		{
			get
			{
				string text = this.ViewState["CancelButtonText"] as string;
				if (text != null)
				{
					return text;
				}
				return SR.GetString("Wizard_Default_CancelButtonText");
			}
			set
			{
				if (value != this.CancelButtonText)
				{
					this.ViewState["CancelButtonText"] = value;
				}
			}
		}

		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x06003F15 RID: 16149 RVA: 0x00106088 File Offset: 0x00105088
		// (set) Token: 0x06003F16 RID: 16150 RVA: 0x001060B1 File Offset: 0x001050B1
		[DefaultValue(ButtonType.Button)]
		[WebCategory("Appearance")]
		[WebSysDescription("Wizard_CancelButtonType")]
		public virtual ButtonType CancelButtonType
		{
			get
			{
				object obj = this.ViewState["CancelButtonType"];
				if (obj != null)
				{
					return (ButtonType)obj;
				}
				return ButtonType.Button;
			}
			set
			{
				this.ValidateButtonType(value);
				this.ViewState["CancelButtonType"] = value;
			}
		}

		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x06003F17 RID: 16151 RVA: 0x001060D0 File Offset: 0x001050D0
		// (set) Token: 0x06003F18 RID: 16152 RVA: 0x001060FD File Offset: 0x001050FD
		[WebSysDescription("Wizard_CancelDestinationPageUrl")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Themeable(false)]
		[WebCategory("Behavior")]
		[DefaultValue("")]
		[UrlProperty]
		public virtual string CancelDestinationPageUrl
		{
			get
			{
				string text = this.ViewState["CancelDestinationPageUrl"] as string;
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["CancelDestinationPageUrl"] = value;
			}
		}

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x06003F19 RID: 16153 RVA: 0x00106110 File Offset: 0x00105110
		// (set) Token: 0x06003F1A RID: 16154 RVA: 0x0010612C File Offset: 0x0010512C
		[WebCategory("Layout")]
		[DefaultValue(0)]
		[WebSysDescription("Wizard_CellPadding")]
		public virtual int CellPadding
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return 0;
				}
				return ((TableStyle)base.ControlStyle).CellPadding;
			}
			set
			{
				((TableStyle)base.ControlStyle).CellPadding = value;
			}
		}

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x06003F1B RID: 16155 RVA: 0x0010613F File Offset: 0x0010513F
		// (set) Token: 0x06003F1C RID: 16156 RVA: 0x0010615B File Offset: 0x0010515B
		[WebCategory("Layout")]
		[DefaultValue(0)]
		[WebSysDescription("Wizard_CellSpacing")]
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

		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x06003F1D RID: 16157 RVA: 0x0010616E File Offset: 0x0010516E
		internal IDictionary CustomNavigationContainers
		{
			get
			{
				if (this._customNavigationContainers == null)
				{
					this._customNavigationContainers = new Hashtable();
				}
				return this._customNavigationContainers;
			}
		}

		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x06003F1E RID: 16158 RVA: 0x00106189 File Offset: 0x00105189
		internal ITemplate CustomNavigationTemplate
		{
			get
			{
				if (this.ActiveStep == null || !(this.ActiveStep is TemplatedWizardStep))
				{
					return null;
				}
				return ((TemplatedWizardStep)this.ActiveStep).CustomNavigationTemplate;
			}
		}

		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x06003F1F RID: 16159 RVA: 0x001061B4 File Offset: 0x001051B4
		// (set) Token: 0x06003F20 RID: 16160 RVA: 0x001061DD File Offset: 0x001051DD
		[WebSysDescription("Wizard_DisplayCancelButton")]
		[DefaultValue(false)]
		[Themeable(false)]
		[WebCategory("Behavior")]
		public virtual bool DisplayCancelButton
		{
			get
			{
				object obj = this.ViewState["DisplayCancelButton"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["DisplayCancelButton"] = value;
			}
		}

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x06003F21 RID: 16161 RVA: 0x001061F5 File Offset: 0x001051F5
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebCategory("Styles")]
		[WebSysDescription("Wizard_FinishCompleteButtonStyle")]
		public Style FinishCompleteButtonStyle
		{
			get
			{
				if (this._finishCompleteButtonStyle == null)
				{
					this._finishCompleteButtonStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._finishCompleteButtonStyle).TrackViewState();
					}
				}
				return this._finishCompleteButtonStyle;
			}
		}

		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x06003F22 RID: 16162 RVA: 0x00106224 File Offset: 0x00105224
		// (set) Token: 0x06003F23 RID: 16163 RVA: 0x00106256 File Offset: 0x00105256
		[Localizable(true)]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("Wizard_Default_FinishButtonText")]
		[WebSysDescription("Wizard_FinishCompleteButtonText")]
		public virtual string FinishCompleteButtonText
		{
			get
			{
				string text = this.ViewState["FinishCompleteButtonText"] as string;
				if (text != null)
				{
					return text;
				}
				return SR.GetString("Wizard_Default_FinishButtonText");
			}
			set
			{
				this.ViewState["FinishCompleteButtonText"] = value;
			}
		}

		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x06003F24 RID: 16164 RVA: 0x0010626C File Offset: 0x0010526C
		// (set) Token: 0x06003F25 RID: 16165 RVA: 0x00106295 File Offset: 0x00105295
		[WebCategory("Appearance")]
		[DefaultValue(ButtonType.Button)]
		[WebSysDescription("Wizard_FinishCompleteButtonType")]
		public virtual ButtonType FinishCompleteButtonType
		{
			get
			{
				object obj = this.ViewState["FinishCompleteButtonType"];
				if (obj != null)
				{
					return (ButtonType)obj;
				}
				return ButtonType.Button;
			}
			set
			{
				this.ValidateButtonType(value);
				this.ViewState["FinishCompleteButtonType"] = value;
			}
		}

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x06003F26 RID: 16166 RVA: 0x001062B4 File Offset: 0x001052B4
		// (set) Token: 0x06003F27 RID: 16167 RVA: 0x001062E1 File Offset: 0x001052E1
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		[Themeable(false)]
		[WebCategory("Behavior")]
		[WebSysDescription("Wizard_FinishDestinationPageUrl")]
		[UrlProperty]
		public virtual string FinishDestinationPageUrl
		{
			get
			{
				object obj = this.ViewState["FinishDestinationPageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["FinishDestinationPageUrl"] = value;
			}
		}

		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x06003F28 RID: 16168 RVA: 0x001062F4 File Offset: 0x001052F4
		// (set) Token: 0x06003F29 RID: 16169 RVA: 0x00106321 File Offset: 0x00105321
		[WebSysDescription("Wizard_FinishCompleteButtonImageUrl")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebCategory("Appearance")]
		[UrlProperty]
		public virtual string FinishCompleteButtonImageUrl
		{
			get
			{
				object obj = this.ViewState["FinishCompleteButtonImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["FinishCompleteButtonImageUrl"] = value;
			}
		}

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x06003F2A RID: 16170 RVA: 0x00106334 File Offset: 0x00105334
		[DefaultValue(null)]
		[WebCategory("Styles")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebSysDescription("Wizard_FinishPreviousButtonStyle")]
		public Style FinishPreviousButtonStyle
		{
			get
			{
				if (this._finishPreviousButtonStyle == null)
				{
					this._finishPreviousButtonStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._finishPreviousButtonStyle).TrackViewState();
					}
				}
				return this._finishPreviousButtonStyle;
			}
		}

		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x06003F2B RID: 16171 RVA: 0x00106364 File Offset: 0x00105364
		// (set) Token: 0x06003F2C RID: 16172 RVA: 0x00106396 File Offset: 0x00105396
		[WebSysDescription("Wizard_FinishPreviousButtonText")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("Wizard_Default_StepPreviousButtonText")]
		public virtual string FinishPreviousButtonText
		{
			get
			{
				string text = this.ViewState["FinishPreviousButtonText"] as string;
				if (text != null)
				{
					return text;
				}
				return SR.GetString("Wizard_Default_StepPreviousButtonText");
			}
			set
			{
				this.ViewState["FinishPreviousButtonText"] = value;
			}
		}

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x06003F2D RID: 16173 RVA: 0x001063AC File Offset: 0x001053AC
		// (set) Token: 0x06003F2E RID: 16174 RVA: 0x001063D5 File Offset: 0x001053D5
		[WebCategory("Appearance")]
		[DefaultValue(ButtonType.Button)]
		[WebSysDescription("Wizard_FinishPreviousButtonType")]
		public virtual ButtonType FinishPreviousButtonType
		{
			get
			{
				object obj = this.ViewState["FinishPreviousButtonType"];
				if (obj != null)
				{
					return (ButtonType)obj;
				}
				return ButtonType.Button;
			}
			set
			{
				this.ValidateButtonType(value);
				this.ViewState["FinishPreviousButtonType"] = value;
			}
		}

		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x06003F2F RID: 16175 RVA: 0x001063F4 File Offset: 0x001053F4
		// (set) Token: 0x06003F30 RID: 16176 RVA: 0x00106421 File Offset: 0x00105421
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		[WebCategory("Appearance")]
		[WebSysDescription("Wizard_FinishPreviousButtonImageUrl")]
		[DefaultValue("")]
		public virtual string FinishPreviousButtonImageUrl
		{
			get
			{
				object obj = this.ViewState["FinishPreviousButtonImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["FinishPreviousButtonImageUrl"] = value;
			}
		}

		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x06003F31 RID: 16177 RVA: 0x00106434 File Offset: 0x00105434
		internal bool IsMacIE5
		{
			get
			{
				if (!this._isMacIESet && !base.DesignMode)
				{
					HttpBrowserCapabilities httpBrowserCapabilities = null;
					if (this.Page != null)
					{
						httpBrowserCapabilities = this.Page.Request.Browser;
					}
					else
					{
						HttpContext httpContext = HttpContext.Current;
						if (httpContext != null)
						{
							httpBrowserCapabilities = httpContext.Request.Browser;
						}
					}
					if (httpBrowserCapabilities != null)
					{
						this._isMacIE = httpBrowserCapabilities.Type == "IE5" && httpBrowserCapabilities.Platform == "MacPPC";
					}
					this._isMacIESet = true;
				}
				return this._isMacIE;
			}
		}

		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x06003F32 RID: 16178 RVA: 0x001064BE File Offset: 0x001054BE
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebCategory("Styles")]
		[WebSysDescription("Wizard_StartNextButtonStyle")]
		public Style StartNextButtonStyle
		{
			get
			{
				if (this._startNextButtonStyle == null)
				{
					this._startNextButtonStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._startNextButtonStyle).TrackViewState();
					}
				}
				return this._startNextButtonStyle;
			}
		}

		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x06003F33 RID: 16179 RVA: 0x001064EC File Offset: 0x001054EC
		// (set) Token: 0x06003F34 RID: 16180 RVA: 0x0010651E File Offset: 0x0010551E
		[WebCategory("Appearance")]
		[WebSysDefaultValue("Wizard_Default_StepNextButtonText")]
		[Localizable(true)]
		[WebSysDescription("Wizard_StartNextButtonText")]
		public virtual string StartNextButtonText
		{
			get
			{
				string text = this.ViewState["StartNextButtonText"] as string;
				if (text != null)
				{
					return text;
				}
				return SR.GetString("Wizard_Default_StepNextButtonText");
			}
			set
			{
				this.ViewState["StartNextButtonText"] = value;
			}
		}

		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x06003F35 RID: 16181 RVA: 0x00106534 File Offset: 0x00105534
		// (set) Token: 0x06003F36 RID: 16182 RVA: 0x0010655D File Offset: 0x0010555D
		[WebSysDescription("Wizard_StartNextButtonType")]
		[WebCategory("Appearance")]
		[DefaultValue(ButtonType.Button)]
		public virtual ButtonType StartNextButtonType
		{
			get
			{
				object obj = this.ViewState["StartNextButtonType"];
				if (obj != null)
				{
					return (ButtonType)obj;
				}
				return ButtonType.Button;
			}
			set
			{
				this.ValidateButtonType(value);
				this.ViewState["StartNextButtonType"] = value;
			}
		}

		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x06003F37 RID: 16183 RVA: 0x0010657C File Offset: 0x0010557C
		// (set) Token: 0x06003F38 RID: 16184 RVA: 0x001065A9 File Offset: 0x001055A9
		[WebCategory("Appearance")]
		[UrlProperty]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		[WebSysDescription("Wizard_StartNextButtonImageUrl")]
		public virtual string StartNextButtonImageUrl
		{
			get
			{
				object obj = this.ViewState["StartNextButtonImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["StartNextButtonImageUrl"] = value;
			}
		}

		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x06003F39 RID: 16185 RVA: 0x001065BC File Offset: 0x001055BC
		// (set) Token: 0x06003F3A RID: 16186 RVA: 0x001065C4 File Offset: 0x001055C4
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(Wizard))]
		[WebSysDescription("Wizard_FinishNavigationTemplate")]
		[DefaultValue(null)]
		public virtual ITemplate FinishNavigationTemplate
		{
			get
			{
				return this._finishNavigationTemplate;
			}
			set
			{
				this._finishNavigationTemplate = value;
				this.RequiresControlsRecreation();
			}
		}

		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x06003F3B RID: 16187 RVA: 0x001065D3 File Offset: 0x001055D3
		[NotifyParentProperty(true)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("Styles")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("WebControl_HeaderStyle")]
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

		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06003F3C RID: 16188 RVA: 0x00106601 File Offset: 0x00105601
		// (set) Token: 0x06003F3D RID: 16189 RVA: 0x00106609 File Offset: 0x00105609
		[Browsable(false)]
		[TemplateContainer(typeof(Wizard))]
		[WebSysDescription("WebControl_HeaderTemplate")]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual ITemplate HeaderTemplate
		{
			get
			{
				return this._headerTemplate;
			}
			set
			{
				this._headerTemplate = value;
				this.RequiresControlsRecreation();
			}
		}

		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06003F3E RID: 16190 RVA: 0x00106618 File Offset: 0x00105618
		// (set) Token: 0x06003F3F RID: 16191 RVA: 0x00106645 File Offset: 0x00105645
		[DefaultValue("")]
		[WebSysDescription("Wizard_HeaderText")]
		[WebCategory("Appearance")]
		[Localizable(true)]
		public virtual string HeaderText
		{
			get
			{
				string text = this.ViewState["HeaderText"] as string;
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["HeaderText"] = value;
			}
		}

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x06003F40 RID: 16192 RVA: 0x00106658 File Offset: 0x00105658
		private Stack History
		{
			get
			{
				if (this._historyStack == null)
				{
					this._historyStack = new Stack();
				}
				return this._historyStack;
			}
		}

		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x06003F41 RID: 16193 RVA: 0x00106674 File Offset: 0x00105674
		internal MultiView MultiView
		{
			get
			{
				if (this._multiView == null)
				{
					this._multiView = new MultiView();
					this._multiView.EnableTheming = true;
					this._multiView.ID = "WizardMultiView";
					this._multiView.ActiveViewChanged += this.MultiViewActiveViewChanged;
					this._multiView.IgnoreBubbleEvents();
				}
				return this._multiView;
			}
		}

		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x06003F42 RID: 16194 RVA: 0x001066D8 File Offset: 0x001056D8
		[NotifyParentProperty(true)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("Styles")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("Wizard_NavigationButtonStyle")]
		public Style NavigationButtonStyle
		{
			get
			{
				if (this._navigationButtonStyle == null)
				{
					this._navigationButtonStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._navigationButtonStyle).TrackViewState();
					}
				}
				return this._navigationButtonStyle;
			}
		}

		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x06003F43 RID: 16195 RVA: 0x00106706 File Offset: 0x00105706
		internal TableCell NavigationTableCell
		{
			get
			{
				if (this._navigationTableCell == null)
				{
					this._navigationTableCell = new TableCell();
				}
				return this._navigationTableCell;
			}
		}

		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x06003F44 RID: 16196 RVA: 0x00106721 File Offset: 0x00105721
		[NotifyParentProperty(true)]
		[WebSysDescription("Wizard_NavigationStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("Styles")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public TableItemStyle NavigationStyle
		{
			get
			{
				if (this._navigationStyle == null)
				{
					this._navigationStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._navigationStyle).TrackViewState();
					}
				}
				return this._navigationStyle;
			}
		}

		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x06003F45 RID: 16197 RVA: 0x0010674F File Offset: 0x0010574F
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("Wizard_StepNextButtonStyle")]
		public Style StepNextButtonStyle
		{
			get
			{
				if (this._stepNextButtonStyle == null)
				{
					this._stepNextButtonStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._stepNextButtonStyle).TrackViewState();
					}
				}
				return this._stepNextButtonStyle;
			}
		}

		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x06003F46 RID: 16198 RVA: 0x00106780 File Offset: 0x00105780
		// (set) Token: 0x06003F47 RID: 16199 RVA: 0x001067B2 File Offset: 0x001057B2
		[WebSysDescription("Wizard_StepNextButtonText")]
		[Localizable(true)]
		[WebSysDefaultValue("Wizard_Default_StepNextButtonText")]
		[WebCategory("Appearance")]
		public virtual string StepNextButtonText
		{
			get
			{
				string text = this.ViewState["StepNextButtonText"] as string;
				if (text != null)
				{
					return text;
				}
				return SR.GetString("Wizard_Default_StepNextButtonText");
			}
			set
			{
				this.ViewState["StepNextButtonText"] = value;
			}
		}

		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x06003F48 RID: 16200 RVA: 0x001067C8 File Offset: 0x001057C8
		// (set) Token: 0x06003F49 RID: 16201 RVA: 0x001067F1 File Offset: 0x001057F1
		[WebSysDescription("Wizard_StepNextButtonType")]
		[DefaultValue(ButtonType.Button)]
		[WebCategory("Appearance")]
		public virtual ButtonType StepNextButtonType
		{
			get
			{
				object obj = this.ViewState["StepNextButtonType"];
				if (obj != null)
				{
					return (ButtonType)obj;
				}
				return ButtonType.Button;
			}
			set
			{
				this.ValidateButtonType(value);
				this.ViewState["StepNextButtonType"] = value;
			}
		}

		// Token: 0x17000F0D RID: 3853
		// (get) Token: 0x06003F4A RID: 16202 RVA: 0x00106810 File Offset: 0x00105810
		// (set) Token: 0x06003F4B RID: 16203 RVA: 0x0010683D File Offset: 0x0010583D
		[WebSysDescription("Wizard_StepNextButtonImageUrl")]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[UrlProperty]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual string StepNextButtonImageUrl
		{
			get
			{
				object obj = this.ViewState["StepNextButtonImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["StepNextButtonImageUrl"] = value;
			}
		}

		// Token: 0x17000F0E RID: 3854
		// (get) Token: 0x06003F4C RID: 16204 RVA: 0x00106850 File Offset: 0x00105850
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebCategory("Styles")]
		[WebSysDescription("Wizard_StepPreviousButtonStyle")]
		[DefaultValue(null)]
		public Style StepPreviousButtonStyle
		{
			get
			{
				if (this._stepPreviousButtonStyle == null)
				{
					this._stepPreviousButtonStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._stepPreviousButtonStyle).TrackViewState();
					}
				}
				return this._stepPreviousButtonStyle;
			}
		}

		// Token: 0x17000F0F RID: 3855
		// (get) Token: 0x06003F4D RID: 16205 RVA: 0x00106880 File Offset: 0x00105880
		// (set) Token: 0x06003F4E RID: 16206 RVA: 0x001068B2 File Offset: 0x001058B2
		[Localizable(true)]
		[WebSysDefaultValue("Wizard_Default_StepPreviousButtonText")]
		[WebSysDescription("Wizard_StepPreviousButtonText")]
		[WebCategory("Appearance")]
		public virtual string StepPreviousButtonText
		{
			get
			{
				string text = this.ViewState["StepPreviousButtonText"] as string;
				if (text != null)
				{
					return text;
				}
				return SR.GetString("Wizard_Default_StepPreviousButtonText");
			}
			set
			{
				this.ViewState["StepPreviousButtonText"] = value;
			}
		}

		// Token: 0x17000F10 RID: 3856
		// (get) Token: 0x06003F4F RID: 16207 RVA: 0x001068C8 File Offset: 0x001058C8
		// (set) Token: 0x06003F50 RID: 16208 RVA: 0x001068F1 File Offset: 0x001058F1
		[WebCategory("Appearance")]
		[DefaultValue(ButtonType.Button)]
		[WebSysDescription("Wizard_StepPreviousButtonType")]
		public virtual ButtonType StepPreviousButtonType
		{
			get
			{
				object obj = this.ViewState["StepPreviousButtonType"];
				if (obj != null)
				{
					return (ButtonType)obj;
				}
				return ButtonType.Button;
			}
			set
			{
				this.ValidateButtonType(value);
				this.ViewState["StepPreviousButtonType"] = value;
			}
		}

		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x06003F51 RID: 16209 RVA: 0x00106910 File Offset: 0x00105910
		// (set) Token: 0x06003F52 RID: 16210 RVA: 0x0010693D File Offset: 0x0010593D
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[WebSysDescription("Wizard_StepPreviousButtonImageUrl")]
		[UrlProperty]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual string StepPreviousButtonImageUrl
		{
			get
			{
				object obj = this.ViewState["StepPreviousButtonImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["StepPreviousButtonImageUrl"] = value;
			}
		}

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x06003F53 RID: 16211 RVA: 0x00106950 File Offset: 0x00105950
		internal virtual bool ShowCustomNavigationTemplate
		{
			get
			{
				return this.CustomNavigationTemplate != null;
			}
		}

		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x06003F54 RID: 16212 RVA: 0x0010695E File Offset: 0x0010595E
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("Wizard_SideBarButtonStyle")]
		public Style SideBarButtonStyle
		{
			get
			{
				if (this._sideBarButtonStyle == null)
				{
					this._sideBarButtonStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._sideBarButtonStyle).TrackViewState();
					}
				}
				return this._sideBarButtonStyle;
			}
		}

		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x06003F55 RID: 16213 RVA: 0x0010698C File Offset: 0x0010598C
		internal DataList SideBarDataList
		{
			get
			{
				return this._sideBarDataList;
			}
		}

		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x06003F56 RID: 16214 RVA: 0x00106994 File Offset: 0x00105994
		// (set) Token: 0x06003F57 RID: 16215 RVA: 0x0010699C File Offset: 0x0010599C
		[DefaultValue(true)]
		[Themeable(false)]
		[WebCategory("Behavior")]
		[WebSysDescription("Wizard_DisplaySideBar")]
		public virtual bool DisplaySideBar
		{
			get
			{
				return this._displaySideBar;
			}
			set
			{
				if (value != this._displaySideBar)
				{
					this._displaySideBar = value;
					this._sideBarTableCell = null;
					this.RequiresControlsRecreation();
				}
			}
		}

		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x06003F58 RID: 16216 RVA: 0x001069BB File Offset: 0x001059BB
		internal bool SideBarEnabled
		{
			get
			{
				return this._sideBarDataList != null && this.DisplaySideBar;
			}
		}

		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x06003F59 RID: 16217 RVA: 0x001069CD File Offset: 0x001059CD
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebSysDescription("Wizard_SideBarStyle")]
		public TableItemStyle SideBarStyle
		{
			get
			{
				if (this._sideBarStyle == null)
				{
					this._sideBarStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._sideBarStyle).TrackViewState();
					}
				}
				return this._sideBarStyle;
			}
		}

		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x06003F5A RID: 16218 RVA: 0x001069FB File Offset: 0x001059FB
		// (set) Token: 0x06003F5B RID: 16219 RVA: 0x00106A03 File Offset: 0x00105A03
		[TemplateContainer(typeof(Wizard))]
		[Browsable(false)]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("Wizard_SideBarTemplate")]
		public virtual ITemplate SideBarTemplate
		{
			get
			{
				return this._sideBarTemplate;
			}
			set
			{
				this._sideBarTemplate = value;
				this._sideBarTableCell = null;
				this.RequiresControlsRecreation();
			}
		}

		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x06003F5C RID: 16220 RVA: 0x00106A1C File Offset: 0x00105A1C
		// (set) Token: 0x06003F5D RID: 16221 RVA: 0x00106A3F File Offset: 0x00105A3F
		[WebCategory("Appearance")]
		[Localizable(true)]
		[WebSysDescription("WebControl_SkipLinkText")]
		[WebSysDefaultValue("Wizard_Default_SkipToContentText")]
		public virtual string SkipLinkText
		{
			get
			{
				string skipLinkTextInternal = this.SkipLinkTextInternal;
				if (skipLinkTextInternal != null)
				{
					return skipLinkTextInternal;
				}
				return SR.GetString("Wizard_Default_SkipToContentText");
			}
			set
			{
				this.ViewState["SkipLinkText"] = value;
			}
		}

		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x06003F5E RID: 16222 RVA: 0x00106A52 File Offset: 0x00105A52
		internal string SkipLinkTextInternal
		{
			get
			{
				return this.ViewState["SkipLinkText"] as string;
			}
		}

		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x06003F5F RID: 16223 RVA: 0x00106A69 File Offset: 0x00105A69
		// (set) Token: 0x06003F60 RID: 16224 RVA: 0x00106A71 File Offset: 0x00105A71
		[WebSysDescription("Wizard_StartNavigationTemplate")]
		[Browsable(false)]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(Wizard))]
		public virtual ITemplate StartNavigationTemplate
		{
			get
			{
				return this._startNavigationTemplate;
			}
			set
			{
				this._startNavigationTemplate = value;
				this.RequiresControlsRecreation();
			}
		}

		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x06003F61 RID: 16225 RVA: 0x00106A80 File Offset: 0x00105A80
		// (set) Token: 0x06003F62 RID: 16226 RVA: 0x00106A88 File Offset: 0x00105A88
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("Wizard_StepNavigationTemplate")]
		[Browsable(false)]
		[DefaultValue(null)]
		[TemplateContainer(typeof(Wizard))]
		public virtual ITemplate StepNavigationTemplate
		{
			get
			{
				return this._stepNavigationTemplate;
			}
			set
			{
				this._stepNavigationTemplate = value;
				this.RequiresControlsRecreation();
			}
		}

		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x06003F63 RID: 16227 RVA: 0x00106A97 File Offset: 0x00105A97
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("Styles")]
		[WebSysDescription("Wizard_StepStyle")]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public TableItemStyle StepStyle
		{
			get
			{
				if (this._stepStyle == null)
				{
					this._stepStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._stepStyle).TrackViewState();
					}
				}
				return this._stepStyle;
			}
		}

		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x06003F64 RID: 16228 RVA: 0x00106AC5 File Offset: 0x00105AC5
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Table;
			}
		}

		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x06003F65 RID: 16229 RVA: 0x00106AC9 File Offset: 0x00105AC9
		internal ArrayList TemplatedSteps
		{
			get
			{
				if (this._templatedSteps == null)
				{
					this._templatedSteps = new ArrayList();
				}
				return this._templatedSteps;
			}
		}

		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x06003F66 RID: 16230 RVA: 0x00106AE4 File Offset: 0x00105AE4
		[Editor("System.Web.UI.Design.WebControls.WizardStepCollectionEditor,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Themeable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebSysDescription("Wizard_WizardSteps")]
		public virtual WizardStepCollection WizardSteps
		{
			get
			{
				if (this._wizardStepCollection == null)
				{
					this._wizardStepCollection = new WizardStepCollection(this);
				}
				return this._wizardStepCollection;
			}
		}

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06003F67 RID: 16231 RVA: 0x00106B00 File Offset: 0x00105B00
		// (remove) Token: 0x06003F68 RID: 16232 RVA: 0x00106B13 File Offset: 0x00105B13
		[WebSysDescription("Wizard_ActiveStepChanged")]
		[WebCategory("Action")]
		public event EventHandler ActiveStepChanged
		{
			add
			{
				base.Events.AddHandler(Wizard._eventActiveStepChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Wizard._eventActiveStepChanged, value);
			}
		}

		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06003F69 RID: 16233 RVA: 0x00106B26 File Offset: 0x00105B26
		// (remove) Token: 0x06003F6A RID: 16234 RVA: 0x00106B39 File Offset: 0x00105B39
		[WebSysDescription("Wizard_CancelButtonClick")]
		[WebCategory("Action")]
		public event EventHandler CancelButtonClick
		{
			add
			{
				base.Events.AddHandler(Wizard._eventCancelButtonClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(Wizard._eventCancelButtonClick, value);
			}
		}

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06003F6B RID: 16235 RVA: 0x00106B4C File Offset: 0x00105B4C
		// (remove) Token: 0x06003F6C RID: 16236 RVA: 0x00106B5F File Offset: 0x00105B5F
		[WebSysDescription("Wizard_FinishButtonClick")]
		[WebCategory("Action")]
		public event WizardNavigationEventHandler FinishButtonClick
		{
			add
			{
				base.Events.AddHandler(Wizard._eventFinishButtonClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(Wizard._eventFinishButtonClick, value);
			}
		}

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x06003F6D RID: 16237 RVA: 0x00106B72 File Offset: 0x00105B72
		// (remove) Token: 0x06003F6E RID: 16238 RVA: 0x00106B85 File Offset: 0x00105B85
		[WebSysDescription("Wizard_NextButtonClick")]
		[WebCategory("Action")]
		public event WizardNavigationEventHandler NextButtonClick
		{
			add
			{
				base.Events.AddHandler(Wizard._eventNextButtonClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(Wizard._eventNextButtonClick, value);
			}
		}

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x06003F6F RID: 16239 RVA: 0x00106B98 File Offset: 0x00105B98
		// (remove) Token: 0x06003F70 RID: 16240 RVA: 0x00106BAB File Offset: 0x00105BAB
		[WebCategory("Action")]
		[WebSysDescription("Wizard_PreviousButtonClick")]
		public event WizardNavigationEventHandler PreviousButtonClick
		{
			add
			{
				base.Events.AddHandler(Wizard._eventPreviousButtonClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(Wizard._eventPreviousButtonClick, value);
			}
		}

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06003F71 RID: 16241 RVA: 0x00106BBE File Offset: 0x00105BBE
		// (remove) Token: 0x06003F72 RID: 16242 RVA: 0x00106BD1 File Offset: 0x00105BD1
		[WebCategory("Action")]
		[WebSysDescription("Wizard_SideBarButtonClick")]
		public virtual event WizardNavigationEventHandler SideBarButtonClick
		{
			add
			{
				base.Events.AddHandler(Wizard._eventSideBarButtonClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(Wizard._eventSideBarButtonClick, value);
			}
		}

		// Token: 0x06003F73 RID: 16243 RVA: 0x00106BE4 File Offset: 0x00105BE4
		private void MultiViewActiveViewChanged(object source, EventArgs e)
		{
			this.OnActiveStepChanged(this, EventArgs.Empty);
		}

		// Token: 0x06003F74 RID: 16244 RVA: 0x00106BF2 File Offset: 0x00105BF2
		private void ApplyButtonProperties(ButtonType type, string text, string imageUrl, IButtonControl button)
		{
			this.ApplyButtonProperties(type, text, imageUrl, button, true);
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x00106C00 File Offset: 0x00105C00
		private void ApplyButtonProperties(ButtonType type, string text, string imageUrl, IButtonControl button, bool imageButtonVisible)
		{
			if (button == null)
			{
				return;
			}
			if (button is ImageButton)
			{
				ImageButton imageButton = (ImageButton)button;
				imageButton.ImageUrl = imageUrl;
				imageButton.AlternateText = text;
				if (button is Control)
				{
					((Control)button).Visible = imageButtonVisible;
					return;
				}
			}
			else
			{
				button.Text = text;
			}
		}

		// Token: 0x06003F76 RID: 16246 RVA: 0x00106C54 File Offset: 0x00105C54
		internal virtual void ApplyControlProperties()
		{
			if (!base.DesignMode && (this.ActiveStepIndex < 0 || this.ActiveStepIndex >= this.WizardSteps.Count || this.WizardSteps.Count == 0))
			{
				return;
			}
			if (this.SideBarEnabled && this._sideBarStyle != null)
			{
				this._sideBarTableCell.ApplyStyle(this._sideBarStyle);
			}
			if (this._headerTableRow != null)
			{
				if (this.HeaderTemplate == null && string.IsNullOrEmpty(this.HeaderText))
				{
					this._headerTableRow.Visible = false;
				}
				else
				{
					this._headerTableCell.ApplyStyle(this._headerStyle);
					if (this.HeaderTemplate != null)
					{
						if (this._titleLiteral != null)
						{
							this._titleLiteral.Visible = false;
						}
					}
					else if (this._titleLiteral != null)
					{
						this._titleLiteral.Text = this.HeaderText;
					}
				}
			}
			if (this._stepTableCell != null && this._stepStyle != null)
			{
				if (!base.DesignMode && this.IsMacIE5 && this._stepStyle.Height == Unit.Empty)
				{
					this._stepStyle.Height = Unit.Pixel(1);
				}
				this._stepTableCell.ApplyStyle(this._stepStyle);
			}
			this.ApplyNavigationTemplateProperties();
			foreach (object obj in this.CustomNavigationContainers.Values)
			{
				Control control = (Control)obj;
				control.Visible = false;
			}
			if (this._navigationTableCell != null)
			{
				this.NavigationTableCell.HorizontalAlign = HorizontalAlign.Right;
				if (this._navigationStyle != null)
				{
					if (!base.DesignMode && this.IsMacIE5 && this._navigationStyle.Height == Unit.Empty)
					{
						this._navigationStyle.Height = Unit.Pixel(1);
					}
					this._navigationTableCell.ApplyStyle(this._navigationStyle);
				}
			}
			if (this.ShowCustomNavigationTemplate)
			{
				Wizard.BaseNavigationTemplateContainer baseNavigationTemplateContainer = (Wizard.BaseNavigationTemplateContainer)this._customNavigationContainers[this.ActiveStep];
				baseNavigationTemplateContainer.Visible = true;
				this._startNavigationTemplateContainer.Visible = false;
				this._stepNavigationTemplateContainer.Visible = false;
				this._finishNavigationTemplateContainer.Visible = false;
				this._navigationRow.Visible = true;
			}
			if (this.SideBarEnabled)
			{
				this._sideBarDataList.DataSource = this.WizardSteps;
				this._sideBarDataList.SelectedIndex = this.ActiveStepIndex;
				this._sideBarDataList.DataBind();
				if (this.SideBarTemplate == null)
				{
					foreach (object obj2 in this._sideBarDataList.Items)
					{
						DataListItem dataListItem = (DataListItem)obj2;
						WebControl webControl = dataListItem.FindControl(Wizard.SideBarButtonID) as WebControl;
						if (webControl != null)
						{
							webControl.MergeStyle(this._sideBarButtonStyle);
						}
					}
				}
			}
			if (this._renderTable != null)
			{
				Util.CopyBaseAttributesToInnerControl(this, this._renderTable);
				if (base.ControlStyleCreated)
				{
					this._renderTable.ApplyStyle(base.ControlStyle);
				}
				else
				{
					this._renderTable.CellSpacing = 0;
					this._renderTable.CellPadding = 0;
				}
				if (!base.DesignMode && this.IsMacIE5 && (!base.ControlStyleCreated || base.ControlStyle.Height == Unit.Empty))
				{
					this._renderTable.ControlStyle.Height = Unit.Pixel(1);
				}
			}
			if (!base.DesignMode && this._navigationTableCell != null && this.IsMacIE5)
			{
				this._navigationTableCell.ControlStyle.Height = Unit.Pixel(1);
			}
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x00107004 File Offset: 0x00106004
		private void ApplyNavigationTemplateProperties()
		{
			if (this._finishNavigationTemplateContainer == null || this._startNavigationTemplateContainer == null || this._stepNavigationTemplateContainer == null)
			{
				return;
			}
			if (this.ActiveStepIndex >= this.WizardSteps.Count || this.ActiveStepIndex < 0)
			{
				return;
			}
			WizardStepType wizardStepType = this.SetActiveTemplates();
			bool flag = wizardStepType != WizardStepType.Finish || this.ActiveStepIndex != 0 || this.ActiveStep.StepType != WizardStepType.Auto;
			if (this.StartNavigationTemplate == null)
			{
				if (base.DesignMode)
				{
					this._defaultStartNavigationTemplate.ResetButtonsVisibility();
				}
				this._startNavigationTemplateContainer.NextButton = this._defaultStartNavigationTemplate.SecondButton;
				((Control)this._startNavigationTemplateContainer.NextButton).Visible = true;
				this._startNavigationTemplateContainer.CancelButton = this._defaultStartNavigationTemplate.CancelButton;
				this.ApplyButtonProperties(this.StartNextButtonType, this.StartNextButtonText, this.StartNextButtonImageUrl, this._startNavigationTemplateContainer.NextButton);
				this.ApplyButtonProperties(this.CancelButtonType, this.CancelButtonText, this.CancelButtonImageUrl, this._startNavigationTemplateContainer.CancelButton);
				this.SetCancelButtonVisibility(this._startNavigationTemplateContainer);
				this._startNavigationTemplateContainer.ApplyButtonStyle(this.FinishCompleteButtonStyle, this.StepPreviousButtonStyle, this.StartNextButtonStyle, this.CancelButtonStyle);
			}
			bool flag2 = true;
			int previousStepIndex = this.GetPreviousStepIndex(false);
			if (previousStepIndex >= 0)
			{
				flag2 = this.WizardSteps[previousStepIndex].AllowReturn;
			}
			if (this.FinishNavigationTemplate == null)
			{
				if (base.DesignMode)
				{
					this._defaultFinishNavigationTemplate.ResetButtonsVisibility();
				}
				this._finishNavigationTemplateContainer.PreviousButton = this._defaultFinishNavigationTemplate.FirstButton;
				((Control)this._finishNavigationTemplateContainer.PreviousButton).Visible = true;
				this._finishNavigationTemplateContainer.FinishButton = this._defaultFinishNavigationTemplate.SecondButton;
				((Control)this._finishNavigationTemplateContainer.FinishButton).Visible = true;
				this._finishNavigationTemplateContainer.CancelButton = this._defaultFinishNavigationTemplate.CancelButton;
				this._finishNavigationTemplateContainer.FinishButton.CommandName = Wizard.MoveCompleteCommandName;
				this.ApplyButtonProperties(this.FinishCompleteButtonType, this.FinishCompleteButtonText, this.FinishCompleteButtonImageUrl, this._finishNavigationTemplateContainer.FinishButton);
				this.ApplyButtonProperties(this.FinishPreviousButtonType, this.FinishPreviousButtonText, this.FinishPreviousButtonImageUrl, this._finishNavigationTemplateContainer.PreviousButton, flag2);
				this.ApplyButtonProperties(this.CancelButtonType, this.CancelButtonText, this.CancelButtonImageUrl, this._finishNavigationTemplateContainer.CancelButton);
				int previousStepIndex2 = this.GetPreviousStepIndex(false);
				if (previousStepIndex2 != -1 && !this.WizardSteps[previousStepIndex2].AllowReturn)
				{
					((Control)this._finishNavigationTemplateContainer.PreviousButton).Visible = false;
				}
				this.SetCancelButtonVisibility(this._finishNavigationTemplateContainer);
				this._finishNavigationTemplateContainer.ApplyButtonStyle(this.FinishCompleteButtonStyle, this.FinishPreviousButtonStyle, this.StepNextButtonStyle, this.CancelButtonStyle);
			}
			if (this.StepNavigationTemplate == null)
			{
				if (base.DesignMode)
				{
					this._defaultStepNavigationTemplate.ResetButtonsVisibility();
				}
				this._stepNavigationTemplateContainer.PreviousButton = this._defaultStepNavigationTemplate.FirstButton;
				((Control)this._stepNavigationTemplateContainer.PreviousButton).Visible = true;
				this._stepNavigationTemplateContainer.NextButton = this._defaultStepNavigationTemplate.SecondButton;
				((Control)this._stepNavigationTemplateContainer.NextButton).Visible = true;
				this._stepNavigationTemplateContainer.CancelButton = this._defaultStepNavigationTemplate.CancelButton;
				this.ApplyButtonProperties(this.StepNextButtonType, this.StepNextButtonText, this.StepNextButtonImageUrl, this._stepNavigationTemplateContainer.NextButton);
				this.ApplyButtonProperties(this.StepPreviousButtonType, this.StepPreviousButtonText, this.StepPreviousButtonImageUrl, this._stepNavigationTemplateContainer.PreviousButton, flag2);
				this.ApplyButtonProperties(this.CancelButtonType, this.CancelButtonText, this.CancelButtonImageUrl, this._stepNavigationTemplateContainer.CancelButton);
				int previousStepIndex3 = this.GetPreviousStepIndex(false);
				if (previousStepIndex3 != -1 && !this.WizardSteps[previousStepIndex3].AllowReturn)
				{
					((Control)this._stepNavigationTemplateContainer.PreviousButton).Visible = false;
				}
				this.SetCancelButtonVisibility(this._stepNavigationTemplateContainer);
				this._stepNavigationTemplateContainer.ApplyButtonStyle(this.FinishCompleteButtonStyle, this.StepPreviousButtonStyle, this.StepNextButtonStyle, this.CancelButtonStyle);
			}
			if (!flag)
			{
				Control control = this._finishNavigationTemplateContainer.PreviousButton as Control;
				if (control != null)
				{
					if (this.FinishNavigationTemplate == null)
					{
						control.Parent.Visible = false;
						return;
					}
					control.Visible = false;
				}
			}
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x0010746C File Offset: 0x0010646C
		internal Wizard.BaseNavigationTemplateContainer CreateBaseNavigationTemplateContainer(string id)
		{
			return new Wizard.BaseNavigationTemplateContainer(this)
			{
				ID = id
			};
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x00107488 File Offset: 0x00106488
		protected internal override void CreateChildControls()
		{
			using (new Wizard.WizardControlCollectionModifier(this))
			{
				this.Controls.Clear();
				this._customNavigationContainers = null;
				this._navigationTableCell = null;
			}
			this.CreateControlHierarchy();
			base.ClearChildViewState();
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x001074E0 File Offset: 0x001064E0
		protected override ControlCollection CreateControlCollection()
		{
			return new Wizard.WizardControlCollection(this);
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x001074E8 File Offset: 0x001064E8
		protected virtual void CreateControlHierarchy()
		{
			Table table = null;
			if (this.DisplaySideBar)
			{
				Table table2 = new Wizard.WizardChildTable(this);
				table2.EnableTheming = false;
				table = new WizardDefaultInnerTable();
				table.CellSpacing = 0;
				table.Height = Unit.Percentage(100.0);
				table.Width = Unit.Percentage(100.0);
				TableRow tableRow = new TableRow();
				table2.Controls.Add(tableRow);
				if (this._sideBarTableCell == null)
				{
					TableCell tableCell = new Wizard.AccessibleTableCell(this);
					tableCell.ID = "SideBarContainer";
					tableCell.Height = Unit.Percentage(100.0);
					this._sideBarTableCell = tableCell;
					tableRow.Controls.Add(tableCell);
					ITemplate template = this.SideBarTemplate;
					if (template == null)
					{
						this._sideBarTableCell.EnableViewState = false;
						template = this.CreateDefaultSideBarTemplate();
					}
					else
					{
						this._sideBarTableCell.EnableTheming = this.EnableTheming;
					}
					template.InstantiateIn(this._sideBarTableCell);
				}
				else
				{
					tableRow.Controls.Add(this._sideBarTableCell);
				}
				this._renderSideBarDataList = false;
				TableCell tableCell2 = new TableCell();
				tableCell2.Height = Unit.Percentage(100.0);
				tableRow.Controls.Add(tableCell2);
				tableCell2.Controls.Add(table);
				if (!base.DesignMode && this.IsMacIE5)
				{
					tableCell2.Height = Unit.Pixel(1);
				}
				using (new Wizard.WizardControlCollectionModifier(this))
				{
					this.Controls.Add(table2);
				}
				if (this._sideBarDataList != null)
				{
					this._sideBarDataList.ItemCommand -= this.DataListItemCommand;
					this._sideBarDataList.ItemDataBound -= this.DataListItemDataBound;
				}
				this._sideBarDataList = this._sideBarTableCell.FindControl(Wizard.DataListID) as DataList;
				if (this._sideBarDataList != null)
				{
					this._sideBarDataList.ItemCommand += this.DataListItemCommand;
					this._sideBarDataList.ItemDataBound += this.DataListItemDataBound;
					this._sideBarDataList.DataSource = this.WizardSteps;
					this._sideBarDataList.SelectedIndex = this.ActiveStepIndex;
					this._sideBarDataList.DataBind();
				}
				else if (!base.DesignMode)
				{
					throw new InvalidOperationException(SR.GetString("Wizard_DataList_Not_Found", new object[] { Wizard.DataListID }));
				}
				this._renderTable = table2;
			}
			else
			{
				table = new Wizard.WizardChildTable(this);
				table.EnableTheming = false;
				using (new Wizard.WizardControlCollectionModifier(this))
				{
					this.Controls.Add(table);
				}
				this._renderTable = table;
			}
			this._headerTableRow = new TableRow();
			table.Controls.Add(this._headerTableRow);
			this._headerTableCell = new Wizard.InternalTableCell(this);
			this._headerTableCell.ID = "HeaderContainer";
			if (this.HeaderTemplate != null)
			{
				this._headerTableCell.EnableTheming = this.EnableTheming;
				this.HeaderTemplate.InstantiateIn(this._headerTableCell);
			}
			else
			{
				this._titleLiteral = new LiteralControl();
				this._headerTableCell.Controls.Add(this._titleLiteral);
			}
			this._headerTableRow.Controls.Add(this._headerTableCell);
			TableRow tableRow2 = new TableRow();
			tableRow2.Height = Unit.Percentage(100.0);
			table.Controls.Add(tableRow2);
			this._stepTableCell = new TableCell();
			tableRow2.Controls.Add(this._stepTableCell);
			this._navigationRow = new TableRow();
			table.Controls.Add(this._navigationRow);
			this._navigationRow.Controls.Add(this.NavigationTableCell);
			this._stepTableCell.Controls.Add(this.MultiView);
			this.InstantiateStepContentTemplates();
			this.CreateNavigationControlHierarchy();
		}

		// Token: 0x06003F7C RID: 16252 RVA: 0x001078E0 File Offset: 0x001068E0
		internal virtual ITemplate CreateDefaultSideBarTemplate()
		{
			return new Wizard.DefaultSideBarTemplate(this);
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x001078E8 File Offset: 0x001068E8
		internal virtual ITemplate CreateDefaultDataListItemTemplate()
		{
			return new Wizard.DataListItemTemplate(this);
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x001078F0 File Offset: 0x001068F0
		private void CreateStartNavigationTemplate()
		{
			ITemplate template = this.StartNavigationTemplate;
			this._startNavigationTemplateContainer = new Wizard.StartNavigationTemplateContainer(this);
			this._startNavigationTemplateContainer.ID = "StartNavigationTemplateContainerID";
			if (template == null)
			{
				this._startNavigationTemplateContainer.EnableViewState = false;
				this._defaultStartNavigationTemplate = Wizard.NavigationTemplate.GetDefaultStartNavigationTemplate(this);
				template = this._defaultStartNavigationTemplate;
			}
			else
			{
				this._startNavigationTemplateContainer.SetEnableTheming();
			}
			template.InstantiateIn(this._startNavigationTemplateContainer);
			this.NavigationTableCell.Controls.Add(this._startNavigationTemplateContainer);
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x00107974 File Offset: 0x00106974
		private void CreateStepNavigationTemplate()
		{
			ITemplate template = this.StepNavigationTemplate;
			this._stepNavigationTemplateContainer = new Wizard.StepNavigationTemplateContainer(this);
			this._stepNavigationTemplateContainer.ID = "StepNavigationTemplateContainerID";
			if (template == null)
			{
				this._stepNavigationTemplateContainer.EnableViewState = false;
				this._defaultStepNavigationTemplate = Wizard.NavigationTemplate.GetDefaultStepNavigationTemplate(this);
				template = this._defaultStepNavigationTemplate;
			}
			else
			{
				this._stepNavigationTemplateContainer.SetEnableTheming();
			}
			template.InstantiateIn(this._stepNavigationTemplateContainer);
			this.NavigationTableCell.Controls.Add(this._stepNavigationTemplateContainer);
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x001079F8 File Offset: 0x001069F8
		private void CreateFinishNavigationTemplate()
		{
			ITemplate template = this.FinishNavigationTemplate;
			this._finishNavigationTemplateContainer = new Wizard.FinishNavigationTemplateContainer(this);
			this._finishNavigationTemplateContainer.ID = "FinishNavigationTemplateContainerID";
			if (template == null)
			{
				this._finishNavigationTemplateContainer.EnableViewState = false;
				this._defaultFinishNavigationTemplate = Wizard.NavigationTemplate.GetDefaultFinishNavigationTemplate(this);
				template = this._defaultFinishNavigationTemplate;
			}
			else
			{
				this._finishNavigationTemplateContainer.SetEnableTheming();
			}
			template.InstantiateIn(this._finishNavigationTemplateContainer);
			this.NavigationTableCell.Controls.Add(this._finishNavigationTemplateContainer);
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x00107A7C File Offset: 0x00106A7C
		protected override Style CreateControlStyle()
		{
			return new TableStyle
			{
				CellSpacing = 0,
				CellPadding = 0
			};
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x00107AA0 File Offset: 0x00106AA0
		internal virtual void CreateCustomNavigationTemplates()
		{
			for (int i = 0; i < this.WizardSteps.Count; i++)
			{
				TemplatedWizardStep templatedWizardStep = this.WizardSteps[i] as TemplatedWizardStep;
				if (templatedWizardStep != null)
				{
					this.RegisterCustomNavigationContainers(templatedWizardStep);
				}
			}
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x00107AE0 File Offset: 0x00106AE0
		internal void RegisterCustomNavigationContainers(TemplatedWizardStep step)
		{
			this.InstantiateStepContentTemplate(step);
			if (!this.CustomNavigationContainers.Contains(step))
			{
				string customContainerID = this.GetCustomContainerID(this.WizardSteps.IndexOf(step));
				Wizard.BaseNavigationTemplateContainer baseNavigationTemplateContainer;
				if (step.CustomNavigationTemplate != null)
				{
					baseNavigationTemplateContainer = this.CreateBaseNavigationTemplateContainer(customContainerID);
					step.CustomNavigationTemplate.InstantiateIn(baseNavigationTemplateContainer);
					step.CustomNavigationTemplateContainer = baseNavigationTemplateContainer;
					baseNavigationTemplateContainer.RegisterButtonCommandEvents();
				}
				else
				{
					baseNavigationTemplateContainer = this.CreateBaseNavigationTemplateContainer(customContainerID);
					baseNavigationTemplateContainer.RegisterButtonCommandEvents();
				}
				this.CustomNavigationContainers[step] = baseNavigationTemplateContainer;
			}
		}

		// Token: 0x06003F84 RID: 16260 RVA: 0x00107B60 File Offset: 0x00106B60
		internal void CreateNavigationControlHierarchy()
		{
			this.NavigationTableCell.Controls.Clear();
			this.CustomNavigationContainers.Clear();
			this.CreateCustomNavigationTemplates();
			foreach (object obj in this.CustomNavigationContainers.Values)
			{
				Control control = (Control)obj;
				this.NavigationTableCell.Controls.Add(control);
			}
			this.CreateStartNavigationTemplate();
			this.CreateFinishNavigationTemplate();
			this.CreateStepNavigationTemplate();
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x00107BFC File Offset: 0x00106BFC
		internal virtual void DataListItemDataBound(object sender, DataListItemEventArgs e)
		{
			DataListItem item = e.Item;
			if (item.ItemType != ListItemType.Item && item.ItemType != ListItemType.AlternatingItem && item.ItemType != ListItemType.SelectedItem && item.ItemType != ListItemType.EditItem)
			{
				return;
			}
			IButtonControl buttonControl = item.FindControl(Wizard.SideBarButtonID) as IButtonControl;
			if (buttonControl != null)
			{
				if (buttonControl is Button)
				{
					((Button)buttonControl).UseSubmitBehavior = false;
				}
				WebControl webControl = buttonControl as WebControl;
				if (webControl != null)
				{
					webControl.TabIndex = this.TabIndex;
				}
				WizardStepBase wizardStepBase = item.DataItem as WizardStepBase;
				if (wizardStepBase != null)
				{
					if (this.GetStepType(wizardStepBase) == WizardStepType.Complete && webControl != null)
					{
						webControl.Enabled = false;
					}
					this.RegisterSideBarDataListForRender();
					if (wizardStepBase.Title.Length > 0)
					{
						buttonControl.Text = wizardStepBase.Title;
					}
					else
					{
						buttonControl.Text = wizardStepBase.ID;
					}
					int num = this.WizardSteps.IndexOf(wizardStepBase);
					buttonControl.CommandName = Wizard.MoveToCommandName;
					buttonControl.CommandArgument = num.ToString(NumberFormatInfo.InvariantInfo);
					this.RegisterCommandEvents(buttonControl);
				}
				return;
			}
			if (!base.DesignMode)
			{
				throw new InvalidOperationException(SR.GetString("Wizard_SideBar_Button_Not_Found", new object[]
				{
					Wizard.DataListID,
					Wizard.SideBarButtonID
				}));
			}
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x00107D33 File Offset: 0x00106D33
		internal void RegisterSideBarDataListForRender()
		{
			this._renderSideBarDataList = true;
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x00107D3C File Offset: 0x00106D3C
		internal virtual void DataListItemCommand(object sender, DataListCommandEventArgs e)
		{
			DataListItem item = e.Item;
			if (!Wizard.MoveToCommandName.Equals(e.CommandName, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			int activeStepIndex = this.ActiveStepIndex;
			int num = int.Parse((string)e.CommandArgument, CultureInfo.InvariantCulture);
			WizardNavigationEventArgs wizardNavigationEventArgs = new WizardNavigationEventArgs(activeStepIndex, num);
			if (this._commandSender != null && !base.DesignMode && this.Page != null && !this.Page.IsValid)
			{
				wizardNavigationEventArgs.Cancel = true;
			}
			this._activeStepIndexSet = false;
			this.OnSideBarButtonClick(wizardNavigationEventArgs);
			if (!wizardNavigationEventArgs.Cancel)
			{
				if (!this._activeStepIndexSet && this.AllowNavigationToStep(num))
				{
					this.ActiveStepIndex = num;
					return;
				}
			}
			else
			{
				this.ActiveStepIndex = activeStepIndex;
			}
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x00107DEB File Offset: 0x00106DEB
		internal string GetCustomContainerID(int index)
		{
			return "__CustomNav" + index;
		}

		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x06003F89 RID: 16265 RVA: 0x00107E00 File Offset: 0x00106E00
		internal bool ShouldRenderChildControl
		{
			get
			{
				if (!base.DesignMode)
				{
					return true;
				}
				if (this._designModeState == null)
				{
					return true;
				}
				object obj = this._designModeState["ShouldRenderWizardSteps"];
				return obj == null || (bool)obj;
			}
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x00107E40 File Offset: 0x00106E40
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		protected override IDictionary GetDesignModeState()
		{
			IDictionary designModeState = base.GetDesignModeState();
			this._designModeState = designModeState;
			int activeStepIndex = this.ActiveStepIndex;
			try
			{
				if (activeStepIndex == -1 && this.WizardSteps.Count > 0)
				{
					this.ActiveStepIndex = 0;
				}
				this.RequiresControlsRecreation();
				this.EnsureChildControls();
				this.ApplyControlProperties();
				designModeState["StepTableCell"] = this._stepTableCell;
				if (this._startNavigationTemplateContainer != null)
				{
					designModeState[Wizard.StartNextButtonID] = this._startNavigationTemplateContainer.NextButton;
					designModeState[Wizard.CancelButtonID] = this._startNavigationTemplateContainer.CancelButton;
				}
				if (this._stepNavigationTemplateContainer != null)
				{
					designModeState[Wizard.StepNextButtonID] = this._stepNavigationTemplateContainer.NextButton;
					designModeState[Wizard.StepPreviousButtonID] = this._stepNavigationTemplateContainer.PreviousButton;
					designModeState[Wizard.CancelButtonID] = this._stepNavigationTemplateContainer.CancelButton;
				}
				if (this._finishNavigationTemplateContainer != null)
				{
					designModeState[Wizard.FinishPreviousButtonID] = this._finishNavigationTemplateContainer.PreviousButton;
					designModeState[Wizard.FinishButtonID] = this._finishNavigationTemplateContainer.FinishButton;
					designModeState[Wizard.CancelButtonID] = this._finishNavigationTemplateContainer.CancelButton;
				}
				if (this.ShowCustomNavigationTemplate)
				{
					Wizard.BaseNavigationTemplateContainer baseNavigationTemplateContainer = (Wizard.BaseNavigationTemplateContainer)this.CustomNavigationContainers[this.ActiveStep];
					designModeState[Wizard.CustomNextButtonID] = baseNavigationTemplateContainer.NextButton;
					designModeState[Wizard.CustomPreviousButtonID] = baseNavigationTemplateContainer.PreviousButton;
					designModeState[Wizard.CustomFinishButtonID] = baseNavigationTemplateContainer.PreviousButton;
					designModeState[Wizard.CancelButtonID] = baseNavigationTemplateContainer.CancelButton;
					designModeState["CustomNavigationControls"] = baseNavigationTemplateContainer.Controls;
				}
				if (this.SideBarTemplate == null && this._sideBarDataList != null)
				{
					this._sideBarDataList.ItemTemplate = this.CreateDefaultDataListItemTemplate();
				}
				designModeState[Wizard.DataListID] = this._sideBarDataList;
				designModeState["TemplatedWizardSteps"] = this.TemplatedSteps;
			}
			finally
			{
				this.ActiveStepIndex = activeStepIndex;
			}
			return designModeState;
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x00108044 File Offset: 0x00107044
		public ICollection GetHistory()
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.History)
			{
				int num = (int)obj;
				arrayList.Add(this.WizardSteps[num]);
			}
			return arrayList;
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x001080B0 File Offset: 0x001070B0
		internal int GetPreviousStepIndex(bool popStack)
		{
			int num = -1;
			int activeStepIndex = this.ActiveStepIndex;
			if (this._historyStack == null || this._historyStack.Count == 0)
			{
				return num;
			}
			if (popStack)
			{
				num = (int)this._historyStack.Pop();
				if (num == activeStepIndex && this._historyStack.Count > 0)
				{
					num = (int)this._historyStack.Pop();
				}
			}
			else
			{
				num = (int)this._historyStack.Peek();
				if (num == activeStepIndex && this._historyStack.Count > 1)
				{
					int num2 = (int)this._historyStack.Pop();
					num = (int)this._historyStack.Peek();
					this._historyStack.Push(num2);
				}
			}
			if (num == activeStepIndex)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x00108174 File Offset: 0x00107174
		internal WizardStepType GetStepType(int index)
		{
			WizardStepBase wizardStepBase = this.WizardSteps[index];
			return this.GetStepType(wizardStepBase, index);
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x00108198 File Offset: 0x00107198
		internal WizardStepType GetStepType(WizardStepBase step)
		{
			int num = this.WizardSteps.IndexOf(step);
			return this.GetStepType(step, num);
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x001081BC File Offset: 0x001071BC
		public WizardStepType GetStepType(WizardStepBase wizardStep, int index)
		{
			if (wizardStep.StepType != WizardStepType.Auto)
			{
				return wizardStep.StepType;
			}
			if (this.WizardSteps.Count == 1 || (index < this.WizardSteps.Count - 1 && this.WizardSteps[index + 1].StepType == WizardStepType.Complete))
			{
				return WizardStepType.Finish;
			}
			if (index == 0)
			{
				return WizardStepType.Start;
			}
			if (index == this.WizardSteps.Count - 1)
			{
				return WizardStepType.Finish;
			}
			return WizardStepType.Step;
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x00108228 File Offset: 0x00107228
		internal virtual void InstantiateStepContentTemplates()
		{
			foreach (object obj in this.TemplatedSteps)
			{
				TemplatedWizardStep templatedWizardStep = (TemplatedWizardStep)obj;
				TemplatedWizardStep templatedWizardStep2 = templatedWizardStep;
				this.InstantiateStepContentTemplate(templatedWizardStep2);
			}
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x00108284 File Offset: 0x00107284
		internal void InstantiateStepContentTemplate(TemplatedWizardStep step)
		{
			step.Controls.Clear();
			Wizard.BaseContentTemplateContainer baseContentTemplateContainer = new Wizard.BaseContentTemplateContainer(this);
			ITemplate contentTemplate = step.ContentTemplate;
			if (contentTemplate != null)
			{
				baseContentTemplateContainer.SetEnableTheming();
				contentTemplate.InstantiateIn(baseContentTemplateContainer.InnerCell);
			}
			step.ContentTemplateContainer = baseContentTemplateContainer;
			step.Controls.Add(baseContentTemplateContainer);
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x001082D4 File Offset: 0x001072D4
		protected internal override void LoadControlState(object state)
		{
			Triplet triplet = state as Triplet;
			if (triplet != null)
			{
				base.LoadControlState(triplet.First);
				Array array = triplet.Second as Array;
				if (array != null)
				{
					Array.Reverse(array);
					this._historyStack = new Stack(array);
				}
				this.ActiveStepIndex = (int)triplet.Third;
			}
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x0010832C File Offset: 0x0010732C
		protected override void LoadViewState(object savedState)
		{
			if (savedState == null)
			{
				base.LoadViewState(null);
				return;
			}
			object[] array = (object[])savedState;
			if (array.Length != 15)
			{
				throw new ArgumentException(SR.GetString("ViewState_InvalidViewState"));
			}
			base.LoadViewState(array[0]);
			if (array[1] != null)
			{
				((IStateManager)this.NavigationButtonStyle).LoadViewState(array[1]);
			}
			if (array[2] != null)
			{
				((IStateManager)this.SideBarButtonStyle).LoadViewState(array[2]);
			}
			if (array[3] != null)
			{
				((IStateManager)this.HeaderStyle).LoadViewState(array[3]);
			}
			if (array[4] != null)
			{
				((IStateManager)this.NavigationStyle).LoadViewState(array[4]);
			}
			if (array[5] != null)
			{
				((IStateManager)this.SideBarStyle).LoadViewState(array[5]);
			}
			if (array[6] != null)
			{
				((IStateManager)this.StepStyle).LoadViewState(array[6]);
			}
			if (array[7] != null)
			{
				((IStateManager)this.StartNextButtonStyle).LoadViewState(array[7]);
			}
			if (array[8] != null)
			{
				((IStateManager)this.StepPreviousButtonStyle).LoadViewState(array[8]);
			}
			if (array[9] != null)
			{
				((IStateManager)this.StepNextButtonStyle).LoadViewState(array[9]);
			}
			if (array[10] != null)
			{
				((IStateManager)this.FinishPreviousButtonStyle).LoadViewState(array[10]);
			}
			if (array[11] != null)
			{
				((IStateManager)this.FinishCompleteButtonStyle).LoadViewState(array[11]);
			}
			if (array[12] != null)
			{
				((IStateManager)this.CancelButtonStyle).LoadViewState(array[12]);
			}
			if (array[13] != null)
			{
				((IStateManager)base.ControlStyle).LoadViewState(array[13]);
			}
			if (array[14] != null)
			{
				this.DisplaySideBar = (bool)array[14];
			}
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x00108484 File Offset: 0x00107484
		public void MoveTo(WizardStepBase wizardStep)
		{
			if (wizardStep == null)
			{
				throw new ArgumentNullException("wizardStep");
			}
			int num = this.WizardSteps.IndexOf(wizardStep);
			if (num == -1)
			{
				throw new ArgumentException(SR.GetString("Wizard_Step_Not_In_Wizard"));
			}
			this.ActiveStepIndex = num;
		}

		// Token: 0x06003F95 RID: 16277 RVA: 0x001084C8 File Offset: 0x001074C8
		protected virtual void OnActiveStepChanged(object source, EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Wizard._eventActiveStepChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003F96 RID: 16278 RVA: 0x001084F8 File Offset: 0x001074F8
		protected override bool OnBubbleEvent(object source, EventArgs e)
		{
			bool flag = false;
			if (e is CommandEventArgs)
			{
				CommandEventArgs commandEventArgs = (CommandEventArgs)e;
				if (string.Equals(Wizard.CancelCommandName, commandEventArgs.CommandName, StringComparison.OrdinalIgnoreCase))
				{
					this.OnCancelButtonClick(EventArgs.Empty);
					return true;
				}
				int activeStepIndex = this.ActiveStepIndex;
				int num = activeStepIndex;
				bool flag2 = true;
				WizardStepType wizardStepType = WizardStepType.Auto;
				WizardStepBase wizardStepBase = this.WizardSteps[activeStepIndex];
				if (wizardStepBase is TemplatedWizardStep)
				{
					flag2 = false;
				}
				else
				{
					wizardStepType = this.GetStepType(wizardStepBase);
				}
				WizardNavigationEventArgs wizardNavigationEventArgs = new WizardNavigationEventArgs(activeStepIndex, num);
				if (this._commandSender != null && this.Page != null && !this.Page.IsValid)
				{
					wizardNavigationEventArgs.Cancel = true;
				}
				bool flag3 = false;
				this._activeStepIndexSet = false;
				if (string.Equals(Wizard.MoveNextCommandName, commandEventArgs.CommandName, StringComparison.OrdinalIgnoreCase))
				{
					if (flag2 && wizardStepType != WizardStepType.Start && wizardStepType != WizardStepType.Step)
					{
						throw new InvalidOperationException(SR.GetString("Wizard_InvalidBubbleEvent", new object[] { Wizard.MoveNextCommandName }));
					}
					if (activeStepIndex < this.WizardSteps.Count - 1)
					{
						wizardNavigationEventArgs.SetNextStepIndex(activeStepIndex + 1);
					}
					this.OnNextButtonClick(wizardNavigationEventArgs);
					flag = true;
				}
				else if (string.Equals(Wizard.MovePreviousCommandName, commandEventArgs.CommandName, StringComparison.OrdinalIgnoreCase))
				{
					if (flag2 && wizardStepType != WizardStepType.Step && wizardStepType != WizardStepType.Finish)
					{
						throw new InvalidOperationException(SR.GetString("Wizard_InvalidBubbleEvent", new object[] { Wizard.MovePreviousCommandName }));
					}
					flag3 = true;
					int previousStepIndex = this.GetPreviousStepIndex(false);
					if (previousStepIndex != -1)
					{
						wizardNavigationEventArgs.SetNextStepIndex(previousStepIndex);
					}
					this.OnPreviousButtonClick(wizardNavigationEventArgs);
					flag = true;
				}
				else if (string.Equals(Wizard.MoveCompleteCommandName, commandEventArgs.CommandName, StringComparison.OrdinalIgnoreCase))
				{
					if (flag2 && wizardStepType != WizardStepType.Finish)
					{
						throw new InvalidOperationException(SR.GetString("Wizard_InvalidBubbleEvent", new object[] { Wizard.MoveCompleteCommandName }));
					}
					if (activeStepIndex < this.WizardSteps.Count - 1)
					{
						wizardNavigationEventArgs.SetNextStepIndex(activeStepIndex + 1);
					}
					this.OnFinishButtonClick(wizardNavigationEventArgs);
					flag = true;
				}
				else if (string.Equals(Wizard.MoveToCommandName, commandEventArgs.CommandName, StringComparison.OrdinalIgnoreCase))
				{
					num = int.Parse((string)commandEventArgs.CommandArgument, CultureInfo.InvariantCulture);
					wizardNavigationEventArgs.SetNextStepIndex(num);
					flag = true;
				}
				if (flag)
				{
					if (!wizardNavigationEventArgs.Cancel)
					{
						if (!this._activeStepIndexSet && this.AllowNavigationToStep(wizardNavigationEventArgs.NextStepIndex))
						{
							if (flag3)
							{
								this.GetPreviousStepIndex(true);
							}
							this.ActiveStepIndex = wizardNavigationEventArgs.NextStepIndex;
						}
					}
					else
					{
						this.ActiveStepIndex = activeStepIndex;
					}
				}
			}
			return flag;
		}

		// Token: 0x06003F97 RID: 16279 RVA: 0x0010875E File Offset: 0x0010775E
		internal void OnWizardStepsChanged()
		{
			if (this._sideBarDataList != null)
			{
				this._sideBarDataList.DataSource = this.WizardSteps;
				this._sideBarDataList.SelectedIndex = this.ActiveStepIndex;
				this._sideBarDataList.DataBind();
			}
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x00108795 File Offset: 0x00107795
		protected virtual bool AllowNavigationToStep(int index)
		{
			return this._historyStack == null || !this._historyStack.Contains(index) || this.WizardSteps[index].AllowReturn;
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x001087C8 File Offset: 0x001077C8
		protected virtual void OnCancelButtonClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Wizard._eventCancelButtonClick];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			string cancelDestinationPageUrl = this.CancelDestinationPageUrl;
			if (!string.IsNullOrEmpty(cancelDestinationPageUrl))
			{
				this.Page.Response.Redirect(base.ResolveClientUrl(cancelDestinationPageUrl), false);
			}
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x0010881D File Offset: 0x0010781D
		private void OnCommand(object sender, CommandEventArgs e)
		{
			this._commandSender = sender as IButtonControl;
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x0010882C File Offset: 0x0010782C
		protected virtual void OnFinishButtonClick(WizardNavigationEventArgs e)
		{
			WizardNavigationEventHandler wizardNavigationEventHandler = (WizardNavigationEventHandler)base.Events[Wizard._eventFinishButtonClick];
			if (wizardNavigationEventHandler != null)
			{
				wizardNavigationEventHandler(this, e);
			}
			string finishDestinationPageUrl = this.FinishDestinationPageUrl;
			if (!string.IsNullOrEmpty(finishDestinationPageUrl))
			{
				this.Page.Response.Redirect(base.ResolveClientUrl(finishDestinationPageUrl), false);
			}
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x00108884 File Offset: 0x00107884
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (this.ActiveStepIndex == -1 && this.WizardSteps.Count > 0 && !base.DesignMode)
			{
				this.ActiveStepIndex = 0;
			}
			this.EnsureChildControls();
			if (this.Page != null)
			{
				this.Page.RegisterRequiresControlState(this);
			}
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x001088D8 File Offset: 0x001078D8
		protected virtual void OnNextButtonClick(WizardNavigationEventArgs e)
		{
			WizardNavigationEventHandler wizardNavigationEventHandler = (WizardNavigationEventHandler)base.Events[Wizard._eventNextButtonClick];
			if (wizardNavigationEventHandler != null)
			{
				wizardNavigationEventHandler(this, e);
			}
		}

		// Token: 0x06003F9E RID: 16286 RVA: 0x00108908 File Offset: 0x00107908
		protected virtual void OnPreviousButtonClick(WizardNavigationEventArgs e)
		{
			WizardNavigationEventHandler wizardNavigationEventHandler = (WizardNavigationEventHandler)base.Events[Wizard._eventPreviousButtonClick];
			if (wizardNavigationEventHandler != null)
			{
				wizardNavigationEventHandler(this, e);
			}
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x00108938 File Offset: 0x00107938
		protected virtual void OnSideBarButtonClick(WizardNavigationEventArgs e)
		{
			WizardNavigationEventHandler wizardNavigationEventHandler = (WizardNavigationEventHandler)base.Events[Wizard._eventSideBarButtonClick];
			if (wizardNavigationEventHandler != null)
			{
				wizardNavigationEventHandler(this, e);
			}
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x00108968 File Offset: 0x00107968
		internal void RequiresControlsRecreation()
		{
			if (base.ChildControlsCreated)
			{
				using (new Wizard.WizardControlCollectionModifier(this))
				{
					base.ChildControlsCreated = false;
				}
			}
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x001089A8 File Offset: 0x001079A8
		protected internal void RegisterCommandEvents(IButtonControl button)
		{
			if (button != null && button.CausesValidation)
			{
				button.Command += this.OnCommand;
			}
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x001089C7 File Offset: 0x001079C7
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			this.EnsureChildControls();
			this.ApplyControlProperties();
			if (this.ActiveStepIndex == -1 || this.WizardSteps.Count == 0)
			{
				return;
			}
			this.RenderContents(writer);
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x00108A08 File Offset: 0x00107A08
		protected internal override object SaveControlState()
		{
			int activeStepIndex = this.ActiveStepIndex;
			if (this._historyStack == null || this._historyStack.Count == 0 || (int)this._historyStack.Peek() != activeStepIndex)
			{
				this.History.Push(this.ActiveStepIndex);
			}
			object obj = base.SaveControlState();
			bool flag = this._historyStack != null && this._historyStack.Count > 0;
			if (obj != null || flag || activeStepIndex != -1)
			{
				object obj2 = (flag ? this._historyStack.ToArray() : null);
				return new Triplet(obj, obj2, activeStepIndex);
			}
			return null;
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x00108AA8 File Offset: 0x00107AA8
		protected override object SaveViewState()
		{
			object[] array = new object[15];
			array[0] = base.SaveViewState();
			array[1] = ((this._navigationButtonStyle != null) ? ((IStateManager)this._navigationButtonStyle).SaveViewState() : null);
			array[2] = ((this._sideBarButtonStyle != null) ? ((IStateManager)this._sideBarButtonStyle).SaveViewState() : null);
			array[3] = ((this._headerStyle != null) ? ((IStateManager)this._headerStyle).SaveViewState() : null);
			array[4] = ((this._navigationStyle != null) ? ((IStateManager)this._navigationStyle).SaveViewState() : null);
			array[5] = ((this._sideBarStyle != null) ? ((IStateManager)this._sideBarStyle).SaveViewState() : null);
			array[6] = ((this._stepStyle != null) ? ((IStateManager)this._stepStyle).SaveViewState() : null);
			array[7] = ((this._startNextButtonStyle != null) ? ((IStateManager)this._startNextButtonStyle).SaveViewState() : null);
			array[8] = ((this._stepNextButtonStyle != null) ? ((IStateManager)this._stepNextButtonStyle).SaveViewState() : null);
			array[9] = ((this._stepPreviousButtonStyle != null) ? ((IStateManager)this._stepPreviousButtonStyle).SaveViewState() : null);
			array[10] = ((this._finishPreviousButtonStyle != null) ? ((IStateManager)this._finishPreviousButtonStyle).SaveViewState() : null);
			array[11] = ((this._finishCompleteButtonStyle != null) ? ((IStateManager)this._finishCompleteButtonStyle).SaveViewState() : null);
			array[12] = ((this._cancelButtonStyle != null) ? ((IStateManager)this._cancelButtonStyle).SaveViewState() : null);
			array[13] = (base.ControlStyleCreated ? ((IStateManager)base.ControlStyle).SaveViewState() : null);
			if (this.DisplaySideBar != this._displaySideBarDefault)
			{
				array[14] = this.DisplaySideBar;
			}
			for (int i = 0; i < 15; i++)
			{
				if (array[i] != null)
				{
					return array;
				}
			}
			return null;
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x00108C44 File Offset: 0x00107C44
		private WizardStepType SetActiveTemplates()
		{
			WizardStepType stepType = this.GetStepType(this.ActiveStepIndex);
			if (stepType == WizardStepType.Complete)
			{
				if (this._headerTableRow != null)
				{
					this._headerTableRow.Visible = false;
				}
				if (this._sideBarTableCell != null)
				{
					this._sideBarTableCell.Visible = false;
				}
				this._navigationRow.Visible = false;
			}
			else if (this._sideBarTableCell != null)
			{
				this._sideBarTableCell.Visible = this.SideBarEnabled && this._renderSideBarDataList;
			}
			this._startNavigationTemplateContainer.Visible = stepType == WizardStepType.Start;
			this._stepNavigationTemplateContainer.Visible = stepType == WizardStepType.Step;
			this._finishNavigationTemplateContainer.Visible = stepType == WizardStepType.Finish;
			return stepType;
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x00108CEC File Offset: 0x00107CEC
		private void SetCancelButtonVisibility(Wizard.BaseNavigationTemplateContainer container)
		{
			Control control = container.CancelButton as Control;
			if (control != null)
			{
				Control parent = control.Parent;
				if (parent != null)
				{
					parent.Visible = this.DisplayCancelButton;
				}
				control.Visible = this.DisplayCancelButton;
			}
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x00108D2C File Offset: 0x00107D2C
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this._navigationButtonStyle != null)
			{
				((IStateManager)this._navigationButtonStyle).TrackViewState();
			}
			if (this._sideBarButtonStyle != null)
			{
				((IStateManager)this._sideBarButtonStyle).TrackViewState();
			}
			if (this._headerStyle != null)
			{
				((IStateManager)this._headerStyle).TrackViewState();
			}
			if (this._navigationStyle != null)
			{
				((IStateManager)this._navigationStyle).TrackViewState();
			}
			if (this._sideBarStyle != null)
			{
				((IStateManager)this._sideBarStyle).TrackViewState();
			}
			if (this._stepStyle != null)
			{
				((IStateManager)this._stepStyle).TrackViewState();
			}
			if (this._startNextButtonStyle != null)
			{
				((IStateManager)this._startNextButtonStyle).TrackViewState();
			}
			if (this._stepPreviousButtonStyle != null)
			{
				((IStateManager)this._stepPreviousButtonStyle).TrackViewState();
			}
			if (this._stepNextButtonStyle != null)
			{
				((IStateManager)this._stepNextButtonStyle).TrackViewState();
			}
			if (this._finishPreviousButtonStyle != null)
			{
				((IStateManager)this._finishPreviousButtonStyle).TrackViewState();
			}
			if (this._finishCompleteButtonStyle != null)
			{
				((IStateManager)this._finishCompleteButtonStyle).TrackViewState();
			}
			if (this._cancelButtonStyle != null)
			{
				((IStateManager)this._cancelButtonStyle).TrackViewState();
			}
			if (base.ControlStyleCreated)
			{
				((IStateManager)base.ControlStyle).TrackViewState();
			}
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x00108E36 File Offset: 0x00107E36
		private void ValidateButtonType(ButtonType value)
		{
			if (value < ButtonType.Button || value > ButtonType.Link)
			{
				throw new ArgumentOutOfRangeException("value");
			}
		}

		// Token: 0x040027AD RID: 10157
		private const string StepTableCellID = "StepTableCell";

		// Token: 0x040027AE RID: 10158
		private const string _multiViewID = "WizardMultiView";

		// Token: 0x040027AF RID: 10159
		private const string _stepNavigationTemplateName = "StepNavigationTemplate";

		// Token: 0x040027B0 RID: 10160
		private const string _finishNavigationTemplateName = "FinishNavigationTemplate";

		// Token: 0x040027B1 RID: 10161
		private const string _startNavigationTemplateName = "StartNavigationTemplate";

		// Token: 0x040027B2 RID: 10162
		private const string _sideBarTemplateName = "SideBarTemplate";

		// Token: 0x040027B3 RID: 10163
		internal const string _customNavigationControls = "CustomNavigationControls";

		// Token: 0x040027B4 RID: 10164
		internal const string _startNavigationTemplateContainerID = "StartNavigationTemplateContainerID";

		// Token: 0x040027B5 RID: 10165
		internal const string _stepNavigationTemplateContainerID = "StepNavigationTemplateContainerID";

		// Token: 0x040027B6 RID: 10166
		internal const string _finishNavigationTemplateContainerID = "FinishNavigationTemplateContainerID";

		// Token: 0x040027B7 RID: 10167
		internal const string _customNavigationContainerIdPrefix = "__CustomNav";

		// Token: 0x040027B8 RID: 10168
		internal const string _templatedStepsID = "TemplatedWizardSteps";

		// Token: 0x040027B9 RID: 10169
		private const string _wizardContentMark = "_SkipLink";

		// Token: 0x040027BA RID: 10170
		private const string _sideBarCellID = "SideBarContainer";

		// Token: 0x040027BB RID: 10171
		private const string _headerCellID = "HeaderContainer";

		// Token: 0x040027BC RID: 10172
		private const int _viewStateArrayLength = 15;

		// Token: 0x040027BD RID: 10173
		private ITemplate _finishNavigationTemplate;

		// Token: 0x040027BE RID: 10174
		private ITemplate _headerTemplate;

		// Token: 0x040027BF RID: 10175
		private ITemplate _startNavigationTemplate;

		// Token: 0x040027C0 RID: 10176
		private ITemplate _stepNavigationTemplate;

		// Token: 0x040027C1 RID: 10177
		private ITemplate _sideBarTemplate;

		// Token: 0x040027C2 RID: 10178
		private Wizard.NavigationTemplate _defaultStartNavigationTemplate;

		// Token: 0x040027C3 RID: 10179
		private Wizard.NavigationTemplate _defaultStepNavigationTemplate;

		// Token: 0x040027C4 RID: 10180
		private Wizard.NavigationTemplate _defaultFinishNavigationTemplate;

		// Token: 0x040027C5 RID: 10181
		private MultiView _multiView;

		// Token: 0x040027C6 RID: 10182
		private Wizard.FinishNavigationTemplateContainer _finishNavigationTemplateContainer;

		// Token: 0x040027C7 RID: 10183
		private Wizard.StartNavigationTemplateContainer _startNavigationTemplateContainer;

		// Token: 0x040027C8 RID: 10184
		private Wizard.StepNavigationTemplateContainer _stepNavigationTemplateContainer;

		// Token: 0x040027C9 RID: 10185
		private IDictionary _customNavigationContainers;

		// Token: 0x040027CA RID: 10186
		private ArrayList _templatedSteps;

		// Token: 0x040027CB RID: 10187
		private static readonly object _eventActiveStepChanged = new object();

		// Token: 0x040027CC RID: 10188
		private static readonly object _eventFinishButtonClick = new object();

		// Token: 0x040027CD RID: 10189
		private static readonly object _eventNextButtonClick = new object();

		// Token: 0x040027CE RID: 10190
		private static readonly object _eventPreviousButtonClick = new object();

		// Token: 0x040027CF RID: 10191
		private static readonly object _eventSideBarButtonClick = new object();

		// Token: 0x040027D0 RID: 10192
		private static readonly object _eventCancelButtonClick = new object();

		// Token: 0x040027D1 RID: 10193
		public static readonly string CancelCommandName = "Cancel";

		// Token: 0x040027D2 RID: 10194
		public static readonly string MoveNextCommandName = "MoveNext";

		// Token: 0x040027D3 RID: 10195
		public static readonly string MovePreviousCommandName = "MovePrevious";

		// Token: 0x040027D4 RID: 10196
		public static readonly string MoveToCommandName = "Move";

		// Token: 0x040027D5 RID: 10197
		public static readonly string MoveCompleteCommandName = "MoveComplete";

		// Token: 0x040027D6 RID: 10198
		protected static readonly string CancelButtonID = "CancelButton";

		// Token: 0x040027D7 RID: 10199
		protected static readonly string StartNextButtonID = "StartNextButton";

		// Token: 0x040027D8 RID: 10200
		protected static readonly string StepPreviousButtonID = "StepPreviousButton";

		// Token: 0x040027D9 RID: 10201
		protected static readonly string StepNextButtonID = "StepNextButton";

		// Token: 0x040027DA RID: 10202
		protected static readonly string FinishButtonID = "FinishButton";

		// Token: 0x040027DB RID: 10203
		protected static readonly string FinishPreviousButtonID = "FinishPreviousButton";

		// Token: 0x040027DC RID: 10204
		protected static readonly string CustomPreviousButtonID = "CustomPreviousButton";

		// Token: 0x040027DD RID: 10205
		protected static readonly string CustomNextButtonID = "CustomNextButton";

		// Token: 0x040027DE RID: 10206
		protected static readonly string CustomFinishButtonID = "CustomFinishButton";

		// Token: 0x040027DF RID: 10207
		protected static readonly string DataListID = "SideBarList";

		// Token: 0x040027E0 RID: 10208
		protected static readonly string SideBarButtonID = "SideBarButton";

		// Token: 0x040027E1 RID: 10209
		private TableRow _headerTableRow;

		// Token: 0x040027E2 RID: 10210
		private TableRow _navigationRow;

		// Token: 0x040027E3 RID: 10211
		private TableCell _sideBarTableCell;

		// Token: 0x040027E4 RID: 10212
		private TableCell _headerTableCell;

		// Token: 0x040027E5 RID: 10213
		private TableCell _stepTableCell;

		// Token: 0x040027E6 RID: 10214
		internal TableCell _navigationTableCell;

		// Token: 0x040027E7 RID: 10215
		private Table _renderTable;

		// Token: 0x040027E8 RID: 10216
		private Stack _historyStack;

		// Token: 0x040027E9 RID: 10217
		private DataList _sideBarDataList;

		// Token: 0x040027EA RID: 10218
		private bool _renderSideBarDataList;

		// Token: 0x040027EB RID: 10219
		private LiteralControl _titleLiteral;

		// Token: 0x040027EC RID: 10220
		private bool _activeStepIndexSet;

		// Token: 0x040027ED RID: 10221
		private WizardStepCollection _wizardStepCollection;

		// Token: 0x040027EE RID: 10222
		private IButtonControl _commandSender;

		// Token: 0x040027EF RID: 10223
		internal bool _displaySideBarDefault = true;

		// Token: 0x040027F0 RID: 10224
		internal bool _displaySideBar = true;

		// Token: 0x040027F1 RID: 10225
		private Style _cancelButtonStyle;

		// Token: 0x040027F2 RID: 10226
		private Style _navigationButtonStyle;

		// Token: 0x040027F3 RID: 10227
		private Style _sideBarButtonStyle;

		// Token: 0x040027F4 RID: 10228
		private Style _startNextButtonStyle;

		// Token: 0x040027F5 RID: 10229
		private Style _stepNextButtonStyle;

		// Token: 0x040027F6 RID: 10230
		private Style _stepPreviousButtonStyle;

		// Token: 0x040027F7 RID: 10231
		private Style _finishCompleteButtonStyle;

		// Token: 0x040027F8 RID: 10232
		private Style _finishPreviousButtonStyle;

		// Token: 0x040027F9 RID: 10233
		private TableItemStyle _headerStyle;

		// Token: 0x040027FA RID: 10234
		private TableItemStyle _navigationStyle;

		// Token: 0x040027FB RID: 10235
		private TableItemStyle _sideBarStyle;

		// Token: 0x040027FC RID: 10236
		private TableItemStyle _stepStyle;

		// Token: 0x040027FD RID: 10237
		private bool _isMacIESet;

		// Token: 0x040027FE RID: 10238
		private bool _isMacIE;

		// Token: 0x040027FF RID: 10239
		private IDictionary _designModeState;

		// Token: 0x0200050F RID: 1295
		internal class WizardControlCollection : ControlCollection
		{
			// Token: 0x06003FAA RID: 16298 RVA: 0x00108F35 File Offset: 0x00107F35
			public WizardControlCollection(Wizard wizard)
				: base(wizard)
			{
				if (!wizard.DesignMode)
				{
					base.SetCollectionReadOnly("Wizard_Cannot_Modify_ControlCollection");
				}
			}
		}

		// Token: 0x02000510 RID: 1296
		internal class WizardControlCollectionModifier : IDisposable
		{
			// Token: 0x06003FAB RID: 16299 RVA: 0x00108F52 File Offset: 0x00107F52
			public WizardControlCollectionModifier(Wizard wizard)
			{
				this._wizard = wizard;
				if (!this._wizard.DesignMode)
				{
					this._controls = this._wizard.Controls;
					this._originalError = this._controls.SetCollectionReadOnly(null);
				}
			}

			// Token: 0x06003FAC RID: 16300 RVA: 0x00108F91 File Offset: 0x00107F91
			void IDisposable.Dispose()
			{
				if (!this._wizard.DesignMode)
				{
					this._controls.SetCollectionReadOnly(this._originalError);
				}
			}

			// Token: 0x04002800 RID: 10240
			private Wizard _wizard;

			// Token: 0x04002801 RID: 10241
			private ControlCollection _controls;

			// Token: 0x04002802 RID: 10242
			private string _originalError;
		}

		// Token: 0x02000511 RID: 1297
		[SupportsEventValidation]
		private class WizardChildTable : ChildTable
		{
			// Token: 0x06003FAD RID: 16301 RVA: 0x00108FB2 File Offset: 0x00107FB2
			internal WizardChildTable(Wizard owner)
			{
				this._owner = owner;
			}

			// Token: 0x06003FAE RID: 16302 RVA: 0x00108FC1 File Offset: 0x00107FC1
			protected override bool OnBubbleEvent(object source, EventArgs args)
			{
				return this._owner.OnBubbleEvent(source, args);
			}

			// Token: 0x04002803 RID: 10243
			private Wizard _owner;
		}

		// Token: 0x02000512 RID: 1298
		private enum WizardTemplateType
		{
			// Token: 0x04002805 RID: 10245
			StartNavigationTemplate,
			// Token: 0x04002806 RID: 10246
			StepNavigationTemplate,
			// Token: 0x04002807 RID: 10247
			FinishNavigationTemplate
		}

		// Token: 0x02000513 RID: 1299
		private sealed class NavigationTemplate : ITemplate
		{
			// Token: 0x06003FAF RID: 16303 RVA: 0x00108FD0 File Offset: 0x00107FD0
			internal static Wizard.NavigationTemplate GetDefaultStartNavigationTemplate(Wizard wizard)
			{
				return new Wizard.NavigationTemplate(wizard, Wizard.WizardTemplateType.StartNavigationTemplate, true, null, "StartNext", "Cancel");
			}

			// Token: 0x06003FB0 RID: 16304 RVA: 0x00108FE5 File Offset: 0x00107FE5
			internal static Wizard.NavigationTemplate GetDefaultStepNavigationTemplate(Wizard wizard)
			{
				return new Wizard.NavigationTemplate(wizard, Wizard.WizardTemplateType.StepNavigationTemplate, false, "StepPrevious", "StepNext", "Cancel");
			}

			// Token: 0x06003FB1 RID: 16305 RVA: 0x00108FFE File Offset: 0x00107FFE
			internal static Wizard.NavigationTemplate GetDefaultFinishNavigationTemplate(Wizard wizard)
			{
				return new Wizard.NavigationTemplate(wizard, Wizard.WizardTemplateType.FinishNavigationTemplate, false, "FinishPrevious", "Finish", "Cancel");
			}

			// Token: 0x06003FB2 RID: 16306 RVA: 0x00109018 File Offset: 0x00108018
			internal void ResetButtonsVisibility()
			{
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						Control control = this._buttons[i][j] as Control;
						if (control != null)
						{
							control.Visible = false;
						}
					}
				}
			}

			// Token: 0x06003FB3 RID: 16307 RVA: 0x00109058 File Offset: 0x00108058
			private NavigationTemplate(Wizard wizard, Wizard.WizardTemplateType templateType, bool button1CausesValidation, string label1ID, string label2ID, string label3ID)
			{
				this._wizard = wizard;
				this._button1ID = label1ID;
				this._button2ID = label2ID;
				this._button3ID = label3ID;
				this._templateType = templateType;
				this._buttons = new IButtonControl[3][];
				this._buttons[0] = new IButtonControl[3];
				this._buttons[1] = new IButtonControl[3];
				this._buttons[2] = new IButtonControl[3];
				this._button1CausesValidation = button1CausesValidation;
			}

			// Token: 0x06003FB4 RID: 16308 RVA: 0x001090D0 File Offset: 0x001080D0
			void ITemplate.InstantiateIn(Control container)
			{
				Table table = new WizardDefaultInnerTable();
				table.CellSpacing = 5;
				table.CellPadding = 5;
				container.Controls.Add(table);
				this._row = new TableRow();
				table.Rows.Add(this._row);
				if (this._button1ID != null)
				{
					this.CreateButtonControl(this._buttons[0], this._button1ID, this._button1CausesValidation, Wizard.MovePreviousCommandName);
				}
				if (this._button2ID != null)
				{
					this.CreateButtonControl(this._buttons[1], this._button2ID, true, (this._templateType == Wizard.WizardTemplateType.FinishNavigationTemplate) ? Wizard.MoveCompleteCommandName : Wizard.MoveNextCommandName);
				}
				this.CreateButtonControl(this._buttons[2], this._button3ID, false, Wizard.CancelCommandName);
			}

			// Token: 0x06003FB5 RID: 16309 RVA: 0x0010918D File Offset: 0x0010818D
			private void OnPreRender(object source, EventArgs e)
			{
				((ImageButton)source).Visible = false;
			}

			// Token: 0x06003FB6 RID: 16310 RVA: 0x0010919C File Offset: 0x0010819C
			private void CreateButtonControl(IButtonControl[] buttons, string id, bool causesValidation, string commandName)
			{
				LinkButton linkButton = new LinkButton();
				linkButton.CausesValidation = causesValidation;
				linkButton.ID = id + "LinkButton";
				linkButton.Visible = false;
				linkButton.CommandName = commandName;
				linkButton.TabIndex = this._wizard.TabIndex;
				this._wizard.RegisterCommandEvents(linkButton);
				buttons[0] = linkButton;
				ImageButton imageButton = new ImageButton();
				imageButton.CausesValidation = causesValidation;
				imageButton.ID = id + "ImageButton";
				imageButton.Visible = true;
				imageButton.CommandName = commandName;
				imageButton.TabIndex = this._wizard.TabIndex;
				this._wizard.RegisterCommandEvents(imageButton);
				imageButton.PreRender += this.OnPreRender;
				buttons[1] = imageButton;
				Button button = new Button();
				button.CausesValidation = causesValidation;
				button.ID = id + "Button";
				button.Visible = false;
				button.CommandName = commandName;
				button.TabIndex = this._wizard.TabIndex;
				this._wizard.RegisterCommandEvents(button);
				buttons[2] = button;
				TableCell tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				this._row.Cells.Add(tableCell);
				tableCell.Controls.Add(linkButton);
				tableCell.Controls.Add(imageButton);
				tableCell.Controls.Add(button);
			}

			// Token: 0x17000F22 RID: 3874
			// (get) Token: 0x06003FB7 RID: 16311 RVA: 0x001092E8 File Offset: 0x001082E8
			internal IButtonControl FirstButton
			{
				get
				{
					ButtonType buttonType = ButtonType.Button;
					switch (this._templateType)
					{
					case Wizard.WizardTemplateType.StartNavigationTemplate:
						goto IL_0037;
					case Wizard.WizardTemplateType.StepNavigationTemplate:
						buttonType = this._wizard.StepPreviousButtonType;
						goto IL_0037;
					}
					buttonType = this._wizard.FinishPreviousButtonType;
					IL_0037:
					return this.GetButtonBasedOnType(0, buttonType);
				}
			}

			// Token: 0x17000F23 RID: 3875
			// (get) Token: 0x06003FB8 RID: 16312 RVA: 0x00109334 File Offset: 0x00108334
			internal IButtonControl SecondButton
			{
				get
				{
					ButtonType buttonType;
					switch (this._templateType)
					{
					case Wizard.WizardTemplateType.StartNavigationTemplate:
						buttonType = this._wizard.StartNextButtonType;
						goto IL_0045;
					case Wizard.WizardTemplateType.StepNavigationTemplate:
						buttonType = this._wizard.StepNextButtonType;
						goto IL_0045;
					}
					buttonType = this._wizard.FinishCompleteButtonType;
					IL_0045:
					return this.GetButtonBasedOnType(1, buttonType);
				}
			}

			// Token: 0x17000F24 RID: 3876
			// (get) Token: 0x06003FB9 RID: 16313 RVA: 0x00109390 File Offset: 0x00108390
			internal IButtonControl CancelButton
			{
				get
				{
					ButtonType cancelButtonType = this._wizard.CancelButtonType;
					return this.GetButtonBasedOnType(2, cancelButtonType);
				}
			}

			// Token: 0x06003FBA RID: 16314 RVA: 0x001093B4 File Offset: 0x001083B4
			private IButtonControl GetButtonBasedOnType(int pos, ButtonType type)
			{
				switch (type)
				{
				case ButtonType.Button:
					return this._buttons[pos][2];
				case ButtonType.Image:
					return this._buttons[pos][1];
				case ButtonType.Link:
					return this._buttons[pos][0];
				default:
					return null;
				}
			}

			// Token: 0x04002808 RID: 10248
			private const string _startNextButtonID = "StartNext";

			// Token: 0x04002809 RID: 10249
			private const string _stepNextButtonID = "StepNext";

			// Token: 0x0400280A RID: 10250
			private const string _stepPreviousButtonID = "StepPrevious";

			// Token: 0x0400280B RID: 10251
			private const string _finishPreviousButtonID = "FinishPrevious";

			// Token: 0x0400280C RID: 10252
			private const string _finishButtonID = "Finish";

			// Token: 0x0400280D RID: 10253
			private const string _cancelButtonID = "Cancel";

			// Token: 0x0400280E RID: 10254
			private Wizard _wizard;

			// Token: 0x0400280F RID: 10255
			private Wizard.WizardTemplateType _templateType;

			// Token: 0x04002810 RID: 10256
			private string _button1ID;

			// Token: 0x04002811 RID: 10257
			private string _button2ID;

			// Token: 0x04002812 RID: 10258
			private string _button3ID;

			// Token: 0x04002813 RID: 10259
			private TableRow _row;

			// Token: 0x04002814 RID: 10260
			private IButtonControl[][] _buttons;

			// Token: 0x04002815 RID: 10261
			private bool _button1CausesValidation;
		}

		// Token: 0x02000514 RID: 1300
		private class DataListItemTemplate : ITemplate
		{
			// Token: 0x06003FBB RID: 16315 RVA: 0x001093F9 File Offset: 0x001083F9
			internal DataListItemTemplate(Wizard owner)
			{
				this._owner = owner;
			}

			// Token: 0x06003FBC RID: 16316 RVA: 0x00109408 File Offset: 0x00108408
			public void InstantiateIn(Control container)
			{
				LinkButton linkButton = new LinkButton();
				container.Controls.Add(linkButton);
				linkButton.ID = Wizard.SideBarButtonID;
				if (this._owner.DesignMode)
				{
					linkButton.MergeStyle(this._owner.SideBarButtonStyle);
				}
			}

			// Token: 0x04002816 RID: 10262
			private Wizard _owner;
		}

		// Token: 0x02000515 RID: 1301
		private class DefaultSideBarTemplate : ITemplate
		{
			// Token: 0x06003FBD RID: 16317 RVA: 0x00109450 File Offset: 0x00108450
			internal DefaultSideBarTemplate(Wizard owner)
			{
				this._owner = owner;
			}

			// Token: 0x06003FBE RID: 16318 RVA: 0x00109460 File Offset: 0x00108460
			public void InstantiateIn(Control container)
			{
				DataList dataList;
				if (this._owner.SideBarDataList == null)
				{
					dataList = new DataList();
					dataList.ID = Wizard.DataListID;
					dataList.SelectedItemStyle.Font.Bold = true;
					dataList.ItemTemplate = this._owner.CreateDefaultDataListItemTemplate();
				}
				else
				{
					dataList = this._owner.SideBarDataList;
				}
				container.Controls.Add(dataList);
			}

			// Token: 0x04002817 RID: 10263
			private Wizard _owner;
		}

		// Token: 0x02000516 RID: 1302
		internal abstract class BlockControl : WebControl, INonBindingContainer, INamingContainer
		{
			// Token: 0x06003FBF RID: 16319 RVA: 0x001094CC File Offset: 0x001084CC
			internal BlockControl(Wizard owner)
			{
				this._owner = owner;
				this._table = new WizardDefaultInnerTable();
				this._table.EnableTheming = false;
				this.Controls.Add(this._table);
				TableRow tableRow = new TableRow();
				this._table.Controls.Add(tableRow);
				this._cell = new TableCell();
				this._cell.Height = Unit.Percentage(100.0);
				this._cell.Width = Unit.Percentage(100.0);
				tableRow.Controls.Add(this._cell);
				this.HandleMacIECellHeight();
				base.PreventAutoID();
			}

			// Token: 0x17000F25 RID: 3877
			// (get) Token: 0x06003FC0 RID: 16320 RVA: 0x0010957F File Offset: 0x0010857F
			protected Table Table
			{
				get
				{
					return this._table;
				}
			}

			// Token: 0x17000F26 RID: 3878
			// (get) Token: 0x06003FC1 RID: 16321 RVA: 0x00109587 File Offset: 0x00108587
			internal TableCell InnerCell
			{
				get
				{
					return this._cell;
				}
			}

			// Token: 0x06003FC2 RID: 16322 RVA: 0x0010958F File Offset: 0x0010858F
			protected override Style CreateControlStyle()
			{
				return new TableItemStyle(this.ViewState);
			}

			// Token: 0x06003FC3 RID: 16323 RVA: 0x0010959C File Offset: 0x0010859C
			public override void Focus()
			{
				throw new NotSupportedException(SR.GetString("NoFocusSupport", new object[] { base.GetType().Name }));
			}

			// Token: 0x06003FC4 RID: 16324 RVA: 0x001095CE File Offset: 0x001085CE
			internal void HandleMacIECellHeight()
			{
				if (!this._owner.DesignMode && this._owner.IsMacIE5)
				{
					this._cell.Height = Unit.Pixel(1);
				}
			}

			// Token: 0x06003FC5 RID: 16325 RVA: 0x001095FB File Offset: 0x001085FB
			protected internal override void Render(HtmlTextWriter writer)
			{
				this.RenderContents(writer);
			}

			// Token: 0x06003FC6 RID: 16326 RVA: 0x00109604 File Offset: 0x00108604
			internal void SetEnableTheming()
			{
				this._cell.EnableTheming = this._owner.EnableTheming;
			}

			// Token: 0x04002818 RID: 10264
			private Table _table;

			// Token: 0x04002819 RID: 10265
			internal TableCell _cell;

			// Token: 0x0400281A RID: 10266
			internal Wizard _owner;
		}

		// Token: 0x02000518 RID: 1304
		private class InternalTableCell : TableCell, INonBindingContainer, INamingContainer
		{
			// Token: 0x06003FDB RID: 16347 RVA: 0x001099E4 File Offset: 0x001089E4
			internal InternalTableCell(Wizard owner)
			{
				this._owner = owner;
			}

			// Token: 0x06003FDC RID: 16348 RVA: 0x001099F3 File Offset: 0x001089F3
			protected override void AddAttributesToRender(HtmlTextWriter writer)
			{
				if (base.ControlStyleCreated && !base.ControlStyle.IsEmpty)
				{
					base.ControlStyle.AddAttributesToRender(writer, this);
				}
			}

			// Token: 0x0400281C RID: 10268
			protected Wizard _owner;
		}

		// Token: 0x02000519 RID: 1305
		private class AccessibleTableCell : Wizard.InternalTableCell
		{
			// Token: 0x06003FDD RID: 16349 RVA: 0x00109A17 File Offset: 0x00108A17
			internal AccessibleTableCell(Wizard owner)
				: base(owner)
			{
			}

			// Token: 0x06003FDE RID: 16350 RVA: 0x00109A20 File Offset: 0x00108A20
			protected internal override void RenderChildren(HtmlTextWriter writer)
			{
				bool flag = !string.IsNullOrEmpty(this._owner.SkipLinkText) && !this._owner.DesignMode;
				string text = this._owner.ClientID + "_SkipLink";
				if (flag)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Href, "#" + text);
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.AddAttribute(HtmlTextWriterAttribute.Alt, this._owner.SkipLinkText);
					writer.AddAttribute(HtmlTextWriterAttribute.Height, "0");
					writer.AddAttribute(HtmlTextWriterAttribute.Width, "0");
					writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "0px");
					writer.AddAttribute(HtmlTextWriterAttribute.Src, base.SpacerImageUrl);
					writer.RenderBeginTag(HtmlTextWriterTag.Img);
					writer.RenderEndTag();
					writer.RenderEndTag();
				}
				base.RenderChildren(writer);
				if (flag)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Id, text);
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.RenderEndTag();
				}
			}
		}

		// Token: 0x0200051A RID: 1306
		internal class BaseContentTemplateContainer : Wizard.BlockControl
		{
			// Token: 0x06003FDF RID: 16351 RVA: 0x00109AFE File Offset: 0x00108AFE
			internal BaseContentTemplateContainer(Wizard owner)
				: base(owner)
			{
				base.Table.Width = Unit.Percentage(100.0);
				base.Table.Height = Unit.Percentage(100.0);
			}
		}

		// Token: 0x0200051B RID: 1307
		internal class BaseNavigationTemplateContainer : WebControl, INonBindingContainer, INamingContainer
		{
			// Token: 0x06003FE0 RID: 16352 RVA: 0x00109B39 File Offset: 0x00108B39
			internal BaseNavigationTemplateContainer(Wizard owner)
			{
				this._owner = owner;
			}

			// Token: 0x17000F2E RID: 3886
			// (get) Token: 0x06003FE1 RID: 16353 RVA: 0x00109B48 File Offset: 0x00108B48
			internal Wizard Owner
			{
				get
				{
					return this._owner;
				}
			}

			// Token: 0x06003FE2 RID: 16354 RVA: 0x00109B50 File Offset: 0x00108B50
			internal virtual void ApplyButtonStyle(Style finishStyle, Style prevStyle, Style nextStyle, Style cancelStyle)
			{
				if (this.FinishButton != null)
				{
					this.ApplyButtonStyleInternal(this.FinishButton, finishStyle);
				}
				if (this.PreviousButton != null)
				{
					this.ApplyButtonStyleInternal(this.PreviousButton, prevStyle);
				}
				if (this.NextButton != null)
				{
					this.ApplyButtonStyleInternal(this.NextButton, nextStyle);
				}
				if (this.CancelButton != null)
				{
					this.ApplyButtonStyleInternal(this.CancelButton, cancelStyle);
				}
			}

			// Token: 0x06003FE3 RID: 16355 RVA: 0x00109BB4 File Offset: 0x00108BB4
			protected void ApplyButtonStyleInternal(IButtonControl control, Style buttonStyle)
			{
				WebControl webControl = control as WebControl;
				if (webControl != null)
				{
					webControl.ApplyStyle(buttonStyle);
					webControl.ControlStyle.MergeWith(this.Owner.NavigationButtonStyle);
				}
			}

			// Token: 0x06003FE4 RID: 16356 RVA: 0x00109BE8 File Offset: 0x00108BE8
			public override void Focus()
			{
				throw new NotSupportedException(SR.GetString("NoFocusSupport", new object[] { base.GetType().Name }));
			}

			// Token: 0x06003FE5 RID: 16357 RVA: 0x00109C1C File Offset: 0x00108C1C
			internal virtual void RegisterButtonCommandEvents()
			{
				this.Owner.RegisterCommandEvents(this.NextButton);
				this.Owner.RegisterCommandEvents(this.FinishButton);
				this.Owner.RegisterCommandEvents(this.PreviousButton);
				this.Owner.RegisterCommandEvents(this.CancelButton);
			}

			// Token: 0x17000F2F RID: 3887
			// (get) Token: 0x06003FE6 RID: 16358 RVA: 0x00109C6D File Offset: 0x00108C6D
			// (set) Token: 0x06003FE7 RID: 16359 RVA: 0x00109C9A File Offset: 0x00108C9A
			internal virtual IButtonControl CancelButton
			{
				get
				{
					if (this._cancelButton != null)
					{
						return this._cancelButton;
					}
					this._cancelButton = this.FindControl(Wizard.CancelButtonID) as IButtonControl;
					return this._cancelButton;
				}
				set
				{
					this._cancelButton = value;
				}
			}

			// Token: 0x17000F30 RID: 3888
			// (get) Token: 0x06003FE8 RID: 16360 RVA: 0x00109CA3 File Offset: 0x00108CA3
			// (set) Token: 0x06003FE9 RID: 16361 RVA: 0x00109CD0 File Offset: 0x00108CD0
			internal virtual IButtonControl NextButton
			{
				get
				{
					if (this._nextButton != null)
					{
						return this._nextButton;
					}
					this._nextButton = this.FindControl(Wizard.StepNextButtonID) as IButtonControl;
					return this._nextButton;
				}
				set
				{
					this._nextButton = value;
				}
			}

			// Token: 0x17000F31 RID: 3889
			// (get) Token: 0x06003FEA RID: 16362 RVA: 0x00109CD9 File Offset: 0x00108CD9
			// (set) Token: 0x06003FEB RID: 16363 RVA: 0x00109D06 File Offset: 0x00108D06
			internal virtual IButtonControl PreviousButton
			{
				get
				{
					if (this._previousButton != null)
					{
						return this._previousButton;
					}
					this._previousButton = this.FindControl(Wizard.StepPreviousButtonID) as IButtonControl;
					return this._previousButton;
				}
				set
				{
					this._previousButton = value;
				}
			}

			// Token: 0x17000F32 RID: 3890
			// (get) Token: 0x06003FEC RID: 16364 RVA: 0x00109D0F File Offset: 0x00108D0F
			// (set) Token: 0x06003FED RID: 16365 RVA: 0x00109D3C File Offset: 0x00108D3C
			internal virtual IButtonControl FinishButton
			{
				get
				{
					if (this._finishButton != null)
					{
						return this._finishButton;
					}
					this._finishButton = this.FindControl(Wizard.FinishButtonID) as IButtonControl;
					return this._finishButton;
				}
				set
				{
					this._finishButton = value;
				}
			}

			// Token: 0x06003FEE RID: 16366 RVA: 0x00109D45 File Offset: 0x00108D45
			internal void SetEnableTheming()
			{
				this.EnableTheming = this._owner.EnableTheming;
			}

			// Token: 0x06003FEF RID: 16367 RVA: 0x00109D58 File Offset: 0x00108D58
			protected internal override void Render(HtmlTextWriter writer)
			{
				this.RenderContents(writer);
			}

			// Token: 0x0400281D RID: 10269
			private IButtonControl _finishButton;

			// Token: 0x0400281E RID: 10270
			private IButtonControl _previousButton;

			// Token: 0x0400281F RID: 10271
			private IButtonControl _nextButton;

			// Token: 0x04002820 RID: 10272
			private IButtonControl _cancelButton;

			// Token: 0x04002821 RID: 10273
			private Wizard _owner;
		}

		// Token: 0x0200051C RID: 1308
		internal class FinishNavigationTemplateContainer : Wizard.BaseNavigationTemplateContainer
		{
			// Token: 0x06003FF0 RID: 16368 RVA: 0x00109D61 File Offset: 0x00108D61
			internal FinishNavigationTemplateContainer(Wizard owner)
				: base(owner)
			{
			}

			// Token: 0x17000F33 RID: 3891
			// (get) Token: 0x06003FF1 RID: 16369 RVA: 0x00109D6A File Offset: 0x00108D6A
			// (set) Token: 0x06003FF2 RID: 16370 RVA: 0x00109D97 File Offset: 0x00108D97
			internal override IButtonControl PreviousButton
			{
				get
				{
					if (this._previousButton != null)
					{
						return this._previousButton;
					}
					this._previousButton = this.FindControl(Wizard.FinishPreviousButtonID) as IButtonControl;
					return this._previousButton;
				}
				set
				{
					this._previousButton = value;
				}
			}

			// Token: 0x04002822 RID: 10274
			private IButtonControl _previousButton;
		}

		// Token: 0x0200051D RID: 1309
		internal class StartNavigationTemplateContainer : Wizard.BaseNavigationTemplateContainer
		{
			// Token: 0x06003FF3 RID: 16371 RVA: 0x00109DA0 File Offset: 0x00108DA0
			internal StartNavigationTemplateContainer(Wizard owner)
				: base(owner)
			{
			}

			// Token: 0x17000F34 RID: 3892
			// (get) Token: 0x06003FF4 RID: 16372 RVA: 0x00109DA9 File Offset: 0x00108DA9
			// (set) Token: 0x06003FF5 RID: 16373 RVA: 0x00109DD6 File Offset: 0x00108DD6
			internal override IButtonControl NextButton
			{
				get
				{
					if (this._nextButton != null)
					{
						return this._nextButton;
					}
					this._nextButton = this.FindControl(Wizard.StartNextButtonID) as IButtonControl;
					return this._nextButton;
				}
				set
				{
					this._nextButton = value;
				}
			}

			// Token: 0x04002823 RID: 10275
			private IButtonControl _nextButton;
		}

		// Token: 0x0200051E RID: 1310
		internal class StepNavigationTemplateContainer : Wizard.BaseNavigationTemplateContainer
		{
			// Token: 0x06003FF6 RID: 16374 RVA: 0x00109DDF File Offset: 0x00108DDF
			internal StepNavigationTemplateContainer(Wizard owner)
				: base(owner)
			{
			}
		}
	}
}
