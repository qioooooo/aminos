using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Web.Configuration;
using System.Web.Security;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005D1 RID: 1489
	[DefaultEvent("Authenticate")]
	[Bindable(false)]
	[Designer("System.Web.UI.Design.WebControls.LoginDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class Login : CompositeControl
	{
		// Token: 0x170011DE RID: 4574
		// (get) Token: 0x06004887 RID: 18567 RVA: 0x00127888 File Offset: 0x00126888
		// (set) Token: 0x06004888 RID: 18568 RVA: 0x001278B1 File Offset: 0x001268B1
		[WebSysDescription("Login_BorderPadding")]
		[WebCategory("Appearance")]
		[DefaultValue(1)]
		public virtual int BorderPadding
		{
			get
			{
				object obj = this.ViewState["BorderPadding"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 1;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("Login_InvalidBorderPadding"));
				}
				this.ViewState["BorderPadding"] = value;
			}
		}

		// Token: 0x170011DF RID: 4575
		// (get) Token: 0x06004889 RID: 18569 RVA: 0x001278E2 File Offset: 0x001268E2
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebSysDescription("Login_CheckBoxStyle")]
		public TableItemStyle CheckBoxStyle
		{
			get
			{
				if (this._checkBoxStyle == null)
				{
					this._checkBoxStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._checkBoxStyle).TrackViewState();
					}
				}
				return this._checkBoxStyle;
			}
		}

		// Token: 0x170011E0 RID: 4576
		// (get) Token: 0x0600488A RID: 18570 RVA: 0x00127910 File Offset: 0x00126910
		// (set) Token: 0x0600488B RID: 18571 RVA: 0x0012793D File Offset: 0x0012693D
		[WebSysDescription("ChangePassword_CreateUserText")]
		[Localizable(true)]
		[WebCategory("Links")]
		[DefaultValue("")]
		public virtual string CreateUserText
		{
			get
			{
				object obj = this.ViewState["CreateUserText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["CreateUserText"] = value;
			}
		}

		// Token: 0x170011E1 RID: 4577
		// (get) Token: 0x0600488C RID: 18572 RVA: 0x00127950 File Offset: 0x00126950
		private bool ConvertingToTemplate
		{
			get
			{
				return base.DesignMode && this._convertingToTemplate;
			}
		}

		// Token: 0x170011E2 RID: 4578
		// (get) Token: 0x0600488D RID: 18573 RVA: 0x00127964 File Offset: 0x00126964
		// (set) Token: 0x0600488E RID: 18574 RVA: 0x00127991 File Offset: 0x00126991
		[WebSysDescription("Login_CreateUserUrl")]
		[WebCategory("Links")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		public virtual string CreateUserUrl
		{
			get
			{
				object obj = this.ViewState["CreateUserUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["CreateUserUrl"] = value;
			}
		}

		// Token: 0x170011E3 RID: 4579
		// (get) Token: 0x0600488F RID: 18575 RVA: 0x001279A4 File Offset: 0x001269A4
		// (set) Token: 0x06004890 RID: 18576 RVA: 0x001279D1 File Offset: 0x001269D1
		[WebCategory("Behavior")]
		[UrlProperty]
		[DefaultValue("")]
		[WebSysDescription("Login_DestinationPageUrl")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Themeable(false)]
		public virtual string DestinationPageUrl
		{
			get
			{
				object obj = this.ViewState["DestinationPageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["DestinationPageUrl"] = value;
			}
		}

		// Token: 0x170011E4 RID: 4580
		// (get) Token: 0x06004891 RID: 18577 RVA: 0x001279E4 File Offset: 0x001269E4
		// (set) Token: 0x06004892 RID: 18578 RVA: 0x00127A0D File Offset: 0x00126A0D
		[WebCategory("Behavior")]
		[WebSysDescription("Login_DisplayRememberMe")]
		[DefaultValue(true)]
		[Themeable(false)]
		public virtual bool DisplayRememberMe
		{
			get
			{
				object obj = this.ViewState["DisplayRememberMe"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["DisplayRememberMe"] = value;
			}
		}

		// Token: 0x170011E5 RID: 4581
		// (get) Token: 0x06004893 RID: 18579 RVA: 0x00127A28 File Offset: 0x00126A28
		// (set) Token: 0x06004894 RID: 18580 RVA: 0x00127A55 File Offset: 0x00126A55
		[WebCategory("Links")]
		[Localizable(true)]
		[WebSysDescription("ChangePassword_HelpPageText")]
		[DefaultValue("")]
		public virtual string HelpPageText
		{
			get
			{
				object obj = this.ViewState["HelpPageText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["HelpPageText"] = value;
			}
		}

		// Token: 0x170011E6 RID: 4582
		// (get) Token: 0x06004895 RID: 18581 RVA: 0x00127A68 File Offset: 0x00126A68
		// (set) Token: 0x06004896 RID: 18582 RVA: 0x00127A95 File Offset: 0x00126A95
		[UrlProperty]
		[WebCategory("Links")]
		[DefaultValue("")]
		[WebSysDescription("LoginControls_HelpPageUrl")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual string HelpPageUrl
		{
			get
			{
				object obj = this.ViewState["HelpPageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["HelpPageUrl"] = value;
			}
		}

		// Token: 0x170011E7 RID: 4583
		// (get) Token: 0x06004897 RID: 18583 RVA: 0x00127AA8 File Offset: 0x00126AA8
		// (set) Token: 0x06004898 RID: 18584 RVA: 0x00127AD5 File Offset: 0x00126AD5
		[WebCategory("Links")]
		[UrlProperty]
		[DefaultValue("")]
		[WebSysDescription("Login_CreateUserIconUrl")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual string CreateUserIconUrl
		{
			get
			{
				object obj = this.ViewState["CreateUserIconUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["CreateUserIconUrl"] = value;
			}
		}

		// Token: 0x170011E8 RID: 4584
		// (get) Token: 0x06004899 RID: 18585 RVA: 0x00127AE8 File Offset: 0x00126AE8
		// (set) Token: 0x0600489A RID: 18586 RVA: 0x00127B15 File Offset: 0x00126B15
		[WebSysDescription("Login_HelpPageIconUrl")]
		[WebCategory("Links")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		public virtual string HelpPageIconUrl
		{
			get
			{
				object obj = this.ViewState["HelpPageIconUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["HelpPageIconUrl"] = value;
			}
		}

		// Token: 0x170011E9 RID: 4585
		// (get) Token: 0x0600489B RID: 18587 RVA: 0x00127B28 File Offset: 0x00126B28
		[WebCategory("Styles")]
		[WebSysDescription("WebControl_HyperLinkStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public TableItemStyle HyperLinkStyle
		{
			get
			{
				if (this._hyperLinkStyle == null)
				{
					this._hyperLinkStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._hyperLinkStyle).TrackViewState();
					}
				}
				return this._hyperLinkStyle;
			}
		}

		// Token: 0x170011EA RID: 4586
		// (get) Token: 0x0600489C RID: 18588 RVA: 0x00127B58 File Offset: 0x00126B58
		// (set) Token: 0x0600489D RID: 18589 RVA: 0x00127B85 File Offset: 0x00126B85
		[WebCategory("Appearance")]
		[Localizable(true)]
		[DefaultValue("")]
		[WebSysDescription("WebControl_InstructionText")]
		public virtual string InstructionText
		{
			get
			{
				object obj = this.ViewState["InstructionText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["InstructionText"] = value;
			}
		}

		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x0600489E RID: 18590 RVA: 0x00127B98 File Offset: 0x00126B98
		[DefaultValue(null)]
		[WebCategory("Styles")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("WebControl_InstructionTextStyle")]
		public TableItemStyle InstructionTextStyle
		{
			get
			{
				if (this._instructionTextStyle == null)
				{
					this._instructionTextStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._instructionTextStyle).TrackViewState();
					}
				}
				return this._instructionTextStyle;
			}
		}

		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x0600489F RID: 18591 RVA: 0x00127BC6 File Offset: 0x00126BC6
		[WebSysDescription("LoginControls_LabelStyle")]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public TableItemStyle LabelStyle
		{
			get
			{
				if (this._labelStyle == null)
				{
					this._labelStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._labelStyle).TrackViewState();
					}
				}
				return this._labelStyle;
			}
		}

		// Token: 0x170011ED RID: 4589
		// (get) Token: 0x060048A0 RID: 18592 RVA: 0x00127BF4 File Offset: 0x00126BF4
		// (set) Token: 0x060048A1 RID: 18593 RVA: 0x00127BFC File Offset: 0x00126BFC
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(Login))]
		public virtual ITemplate LayoutTemplate
		{
			get
			{
				return this._loginTemplate;
			}
			set
			{
				this._loginTemplate = value;
				base.ChildControlsCreated = false;
			}
		}

		// Token: 0x170011EE RID: 4590
		// (get) Token: 0x060048A2 RID: 18594 RVA: 0x00127C0C File Offset: 0x00126C0C
		// (set) Token: 0x060048A3 RID: 18595 RVA: 0x00127C35 File Offset: 0x00126C35
		[DefaultValue(LoginFailureAction.Refresh)]
		[WebCategory("Behavior")]
		[Themeable(false)]
		[WebSysDescription("Login_FailureAction")]
		public virtual LoginFailureAction FailureAction
		{
			get
			{
				object obj = this.ViewState["FailureAction"];
				if (obj != null)
				{
					return (LoginFailureAction)obj;
				}
				return LoginFailureAction.Refresh;
			}
			set
			{
				if (value < LoginFailureAction.Refresh || value > LoginFailureAction.RedirectToLoginPage)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["FailureAction"] = value;
			}
		}

		// Token: 0x170011EF RID: 4591
		// (get) Token: 0x060048A4 RID: 18596 RVA: 0x00127C60 File Offset: 0x00126C60
		// (set) Token: 0x060048A5 RID: 18597 RVA: 0x00127C92 File Offset: 0x00126C92
		[WebSysDefaultValue("Login_DefaultFailureText")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		[WebSysDescription("Login_FailureText")]
		public virtual string FailureText
		{
			get
			{
				object obj = this.ViewState["FailureText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("Login_DefaultFailureText");
			}
			set
			{
				this.ViewState["FailureText"] = value;
			}
		}

		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x060048A6 RID: 18598 RVA: 0x00127CA5 File Offset: 0x00126CA5
		[WebCategory("Styles")]
		[WebSysDescription("WebControl_FailureTextStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public TableItemStyle FailureTextStyle
		{
			get
			{
				if (this._failureTextStyle == null)
				{
					this._failureTextStyle = new ErrorTableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._failureTextStyle).TrackViewState();
					}
				}
				return this._failureTextStyle;
			}
		}

		// Token: 0x170011F1 RID: 4593
		// (get) Token: 0x060048A7 RID: 18599 RVA: 0x00127CD4 File Offset: 0x00126CD4
		// (set) Token: 0x060048A8 RID: 18600 RVA: 0x00127D01 File Offset: 0x00126D01
		[WebCategory("Appearance")]
		[UrlProperty]
		[DefaultValue("")]
		[WebSysDescription("Login_LoginButtonImageUrl")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual string LoginButtonImageUrl
		{
			get
			{
				object obj = this.ViewState["LoginButtonImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["LoginButtonImageUrl"] = value;
			}
		}

		// Token: 0x170011F2 RID: 4594
		// (get) Token: 0x060048A9 RID: 18601 RVA: 0x00127D14 File Offset: 0x00126D14
		[WebCategory("Styles")]
		[WebSysDescription("Login_LoginButtonStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public Style LoginButtonStyle
		{
			get
			{
				if (this._loginButtonStyle == null)
				{
					this._loginButtonStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._loginButtonStyle).TrackViewState();
					}
				}
				return this._loginButtonStyle;
			}
		}

		// Token: 0x170011F3 RID: 4595
		// (get) Token: 0x060048AA RID: 18602 RVA: 0x00127D44 File Offset: 0x00126D44
		// (set) Token: 0x060048AB RID: 18603 RVA: 0x00127D76 File Offset: 0x00126D76
		[WebCategory("Appearance")]
		[Localizable(true)]
		[WebSysDefaultValue("Login_DefaultLoginButtonText")]
		[WebSysDescription("Login_LoginButtonText")]
		public virtual string LoginButtonText
		{
			get
			{
				object obj = this.ViewState["LoginButtonText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("Login_DefaultLoginButtonText");
			}
			set
			{
				this.ViewState["LoginButtonText"] = value;
			}
		}

		// Token: 0x170011F4 RID: 4596
		// (get) Token: 0x060048AC RID: 18604 RVA: 0x00127D8C File Offset: 0x00126D8C
		// (set) Token: 0x060048AD RID: 18605 RVA: 0x00127DB5 File Offset: 0x00126DB5
		[WebCategory("Appearance")]
		[DefaultValue(ButtonType.Button)]
		[WebSysDescription("Login_LoginButtonType")]
		public virtual ButtonType LoginButtonType
		{
			get
			{
				object obj = this.ViewState["LoginButtonType"];
				if (obj != null)
				{
					return (ButtonType)obj;
				}
				return ButtonType.Button;
			}
			set
			{
				if (value < ButtonType.Button || value > ButtonType.Link)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["LoginButtonType"] = value;
			}
		}

		// Token: 0x170011F5 RID: 4597
		// (get) Token: 0x060048AE RID: 18606 RVA: 0x00127DE0 File Offset: 0x00126DE0
		// (set) Token: 0x060048AF RID: 18607 RVA: 0x00127E09 File Offset: 0x00126E09
		[DefaultValue(Orientation.Vertical)]
		[WebSysDescription("Login_Orientation")]
		[WebCategory("Layout")]
		public virtual Orientation Orientation
		{
			get
			{
				object obj = this.ViewState["Orientation"];
				if (obj != null)
				{
					return (Orientation)obj;
				}
				return Orientation.Vertical;
			}
			set
			{
				if (value < Orientation.Horizontal || value > Orientation.Vertical)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["Orientation"] = value;
				base.ChildControlsCreated = false;
			}
		}

		// Token: 0x170011F6 RID: 4598
		// (get) Token: 0x060048B0 RID: 18608 RVA: 0x00127E3C File Offset: 0x00126E3C
		// (set) Token: 0x060048B1 RID: 18609 RVA: 0x00127E69 File Offset: 0x00126E69
		[WebCategory("Data")]
		[Themeable(false)]
		[WebSysDescription("MembershipProvider_Name")]
		[DefaultValue("")]
		public virtual string MembershipProvider
		{
			get
			{
				object obj = this.ViewState["MembershipProvider"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["MembershipProvider"] = value;
			}
		}

		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x060048B2 RID: 18610 RVA: 0x00127E7C File Offset: 0x00126E7C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual string Password
		{
			get
			{
				if (this._password != null)
				{
					return this._password;
				}
				return string.Empty;
			}
		}

		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x060048B3 RID: 18611 RVA: 0x00127E94 File Offset: 0x00126E94
		private string PasswordInternal
		{
			get
			{
				string password = this.Password;
				if (string.IsNullOrEmpty(password) && this._templateContainer != null)
				{
					ITextControl textControl = (ITextControl)this._templateContainer.PasswordTextBox;
					if (textControl != null && textControl.Text != null)
					{
						return textControl.Text;
					}
				}
				return password;
			}
		}

		// Token: 0x170011F9 RID: 4601
		// (get) Token: 0x060048B4 RID: 18612 RVA: 0x00127EDC File Offset: 0x00126EDC
		// (set) Token: 0x060048B5 RID: 18613 RVA: 0x00127F0E File Offset: 0x00126F0E
		[WebCategory("Appearance")]
		[Localizable(true)]
		[WebSysDefaultValue("LoginControls_DefaultPasswordLabelText")]
		[WebSysDescription("LoginControls_PasswordLabelText")]
		public virtual string PasswordLabelText
		{
			get
			{
				object obj = this.ViewState["PasswordLabelText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("LoginControls_DefaultPasswordLabelText");
			}
			set
			{
				this.ViewState["PasswordLabelText"] = value;
			}
		}

		// Token: 0x170011FA RID: 4602
		// (get) Token: 0x060048B6 RID: 18614 RVA: 0x00127F24 File Offset: 0x00126F24
		// (set) Token: 0x060048B7 RID: 18615 RVA: 0x00127F51 File Offset: 0x00126F51
		[DefaultValue("")]
		[WebCategory("Links")]
		[Localizable(true)]
		[WebSysDescription("ChangePassword_PasswordRecoveryText")]
		public virtual string PasswordRecoveryText
		{
			get
			{
				object obj = this.ViewState["PasswordRecoveryText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["PasswordRecoveryText"] = value;
			}
		}

		// Token: 0x170011FB RID: 4603
		// (get) Token: 0x060048B8 RID: 18616 RVA: 0x00127F64 File Offset: 0x00126F64
		// (set) Token: 0x060048B9 RID: 18617 RVA: 0x00127F91 File Offset: 0x00126F91
		[WebCategory("Links")]
		[DefaultValue("")]
		[WebSysDescription("Login_PasswordRecoveryUrl")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		public virtual string PasswordRecoveryUrl
		{
			get
			{
				object obj = this.ViewState["PasswordRecoveryUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["PasswordRecoveryUrl"] = value;
			}
		}

		// Token: 0x170011FC RID: 4604
		// (get) Token: 0x060048BA RID: 18618 RVA: 0x00127FA4 File Offset: 0x00126FA4
		// (set) Token: 0x060048BB RID: 18619 RVA: 0x00127FD1 File Offset: 0x00126FD1
		[WebSysDescription("Login_PasswordRecoveryIconUrl")]
		[WebCategory("Links")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		public virtual string PasswordRecoveryIconUrl
		{
			get
			{
				object obj = this.ViewState["PasswordRecoveryIconUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["PasswordRecoveryIconUrl"] = value;
			}
		}

		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x060048BC RID: 18620 RVA: 0x00127FE4 File Offset: 0x00126FE4
		// (set) Token: 0x060048BD RID: 18621 RVA: 0x00128016 File Offset: 0x00127016
		[WebSysDescription("Login_PasswordRequiredErrorMessage")]
		[Localizable(true)]
		[WebCategory("Validation")]
		[WebSysDefaultValue("Login_DefaultPasswordRequiredErrorMessage")]
		public virtual string PasswordRequiredErrorMessage
		{
			get
			{
				object obj = this.ViewState["PasswordRequiredErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("Login_DefaultPasswordRequiredErrorMessage");
			}
			set
			{
				this.ViewState["PasswordRequiredErrorMessage"] = value;
			}
		}

		// Token: 0x170011FE RID: 4606
		// (get) Token: 0x060048BE RID: 18622 RVA: 0x0012802C File Offset: 0x0012702C
		// (set) Token: 0x060048BF RID: 18623 RVA: 0x00128055 File Offset: 0x00127055
		[Themeable(false)]
		[WebCategory("Behavior")]
		[DefaultValue(false)]
		[WebSysDescription("Login_RememberMeSet")]
		public virtual bool RememberMeSet
		{
			get
			{
				object obj = this.ViewState["RememberMeSet"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["RememberMeSet"] = value;
			}
		}

		// Token: 0x170011FF RID: 4607
		// (get) Token: 0x060048C0 RID: 18624 RVA: 0x00128070 File Offset: 0x00127070
		// (set) Token: 0x060048C1 RID: 18625 RVA: 0x001280A2 File Offset: 0x001270A2
		[Localizable(true)]
		[WebSysDefaultValue("Login_DefaultRememberMeText")]
		[WebSysDescription("Login_RememberMeText")]
		[WebCategory("Appearance")]
		public virtual string RememberMeText
		{
			get
			{
				object obj = this.ViewState["RememberMeText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("Login_DefaultRememberMeText");
			}
			set
			{
				this.ViewState["RememberMeText"] = value;
			}
		}

		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x060048C2 RID: 18626 RVA: 0x001280B5 File Offset: 0x001270B5
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Table;
			}
		}

		// Token: 0x17001201 RID: 4609
		// (get) Token: 0x060048C3 RID: 18627 RVA: 0x001280B9 File Offset: 0x001270B9
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		private Login.LoginContainer TemplateContainer
		{
			get
			{
				this.EnsureChildControls();
				return this._templateContainer;
			}
		}

		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x060048C4 RID: 18628 RVA: 0x001280C7 File Offset: 0x001270C7
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebSysDescription("LoginControls_TextBoxStyle")]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public Style TextBoxStyle
		{
			get
			{
				if (this._textBoxStyle == null)
				{
					this._textBoxStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._textBoxStyle).TrackViewState();
					}
				}
				return this._textBoxStyle;
			}
		}

		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x060048C5 RID: 18629 RVA: 0x001280F8 File Offset: 0x001270F8
		// (set) Token: 0x060048C6 RID: 18630 RVA: 0x00128121 File Offset: 0x00127121
		[WebCategory("Layout")]
		[WebSysDescription("LoginControls_TextLayout")]
		[DefaultValue(LoginTextLayout.TextOnLeft)]
		public virtual LoginTextLayout TextLayout
		{
			get
			{
				object obj = this.ViewState["TextLayout"];
				if (obj != null)
				{
					return (LoginTextLayout)obj;
				}
				return LoginTextLayout.TextOnLeft;
			}
			set
			{
				if (value < LoginTextLayout.TextOnLeft || value > LoginTextLayout.TextOnTop)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["TextLayout"] = value;
				base.ChildControlsCreated = false;
			}
		}

		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x060048C7 RID: 18631 RVA: 0x00128154 File Offset: 0x00127154
		// (set) Token: 0x060048C8 RID: 18632 RVA: 0x00128186 File Offset: 0x00127186
		[Localizable(true)]
		[WebSysDescription("LoginControls_TitleText")]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("Login_DefaultTitleText")]
		public virtual string TitleText
		{
			get
			{
				object obj = this.ViewState["TitleText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("Login_DefaultTitleText");
			}
			set
			{
				this.ViewState["TitleText"] = value;
			}
		}

		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x060048C9 RID: 18633 RVA: 0x00128199 File Offset: 0x00127199
		[DefaultValue(null)]
		[WebCategory("Styles")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("LoginControls_TitleTextStyle")]
		public TableItemStyle TitleTextStyle
		{
			get
			{
				if (this._titleTextStyle == null)
				{
					this._titleTextStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._titleTextStyle).TrackViewState();
					}
				}
				return this._titleTextStyle;
			}
		}

		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x060048CA RID: 18634 RVA: 0x001281C8 File Offset: 0x001271C8
		// (set) Token: 0x060048CB RID: 18635 RVA: 0x001281F5 File Offset: 0x001271F5
		[DefaultValue("")]
		[WebCategory("Appearance")]
		[WebSysDescription("UserName_InitialValue")]
		public virtual string UserName
		{
			get
			{
				object obj = this.ViewState["UserName"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["UserName"] = value;
			}
		}

		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x060048CC RID: 18636 RVA: 0x00128208 File Offset: 0x00127208
		private string UserNameInternal
		{
			get
			{
				string userName = this.UserName;
				if (string.IsNullOrEmpty(userName) && this._templateContainer != null)
				{
					ITextControl textControl = (ITextControl)this._templateContainer.UserNameTextBox;
					if (textControl != null && textControl.Text != null)
					{
						return textControl.Text;
					}
				}
				return userName;
			}
		}

		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x060048CD RID: 18637 RVA: 0x00128250 File Offset: 0x00127250
		// (set) Token: 0x060048CE RID: 18638 RVA: 0x00128282 File Offset: 0x00127282
		[WebCategory("Appearance")]
		[Localizable(true)]
		[WebSysDescription("LoginControls_UserNameLabelText")]
		[WebSysDefaultValue("Login_DefaultUserNameLabelText")]
		public virtual string UserNameLabelText
		{
			get
			{
				object obj = this.ViewState["UserNameLabelText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("Login_DefaultUserNameLabelText");
			}
			set
			{
				this.ViewState["UserNameLabelText"] = value;
			}
		}

		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x060048CF RID: 18639 RVA: 0x00128298 File Offset: 0x00127298
		// (set) Token: 0x060048D0 RID: 18640 RVA: 0x001282CA File Offset: 0x001272CA
		[Localizable(true)]
		[WebSysDescription("ChangePassword_UserNameRequiredErrorMessage")]
		[WebCategory("Validation")]
		[WebSysDefaultValue("Login_DefaultUserNameRequiredErrorMessage")]
		public virtual string UserNameRequiredErrorMessage
		{
			get
			{
				object obj = this.ViewState["UserNameRequiredErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("Login_DefaultUserNameRequiredErrorMessage");
			}
			set
			{
				this.ViewState["UserNameRequiredErrorMessage"] = value;
			}
		}

		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x060048D1 RID: 18641 RVA: 0x001282DD File Offset: 0x001272DD
		[DefaultValue(null)]
		[WebSysDescription("Login_ValidatorTextStyle")]
		[WebCategory("Styles")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public Style ValidatorTextStyle
		{
			get
			{
				if (this._validatorTextStyle == null)
				{
					this._validatorTextStyle = new ErrorStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._validatorTextStyle).TrackViewState();
					}
				}
				return this._validatorTextStyle;
			}
		}

		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x060048D2 RID: 18642 RVA: 0x0012830C File Offset: 0x0012730C
		// (set) Token: 0x060048D3 RID: 18643 RVA: 0x00128335 File Offset: 0x00127335
		[WebCategory("Behavior")]
		[WebSysDescription("Login_VisibleWhenLoggedIn")]
		[DefaultValue(true)]
		[Themeable(false)]
		public virtual bool VisibleWhenLoggedIn
		{
			get
			{
				object obj = this.ViewState["VisibleWhenLoggedIn"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["VisibleWhenLoggedIn"] = value;
			}
		}

		// Token: 0x140000CE RID: 206
		// (add) Token: 0x060048D4 RID: 18644 RVA: 0x0012834D File Offset: 0x0012734D
		// (remove) Token: 0x060048D5 RID: 18645 RVA: 0x00128360 File Offset: 0x00127360
		[WebCategory("Action")]
		[WebSysDescription("Login_LoggedIn")]
		public event EventHandler LoggedIn
		{
			add
			{
				base.Events.AddHandler(Login.EventLoggedIn, value);
			}
			remove
			{
				base.Events.RemoveHandler(Login.EventLoggedIn, value);
			}
		}

		// Token: 0x140000CF RID: 207
		// (add) Token: 0x060048D6 RID: 18646 RVA: 0x00128373 File Offset: 0x00127373
		// (remove) Token: 0x060048D7 RID: 18647 RVA: 0x00128386 File Offset: 0x00127386
		[WebCategory("Action")]
		[WebSysDescription("Login_Authenticate")]
		public event AuthenticateEventHandler Authenticate
		{
			add
			{
				base.Events.AddHandler(Login.EventAuthenticate, value);
			}
			remove
			{
				base.Events.RemoveHandler(Login.EventAuthenticate, value);
			}
		}

		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x060048D8 RID: 18648 RVA: 0x00128399 File Offset: 0x00127399
		// (remove) Token: 0x060048D9 RID: 18649 RVA: 0x001283AC File Offset: 0x001273AC
		[WebCategory("Action")]
		[WebSysDescription("Login_LoggingIn")]
		public event LoginCancelEventHandler LoggingIn
		{
			add
			{
				base.Events.AddHandler(Login.EventLoggingIn, value);
			}
			remove
			{
				base.Events.RemoveHandler(Login.EventLoggingIn, value);
			}
		}

		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x060048DA RID: 18650 RVA: 0x001283BF File Offset: 0x001273BF
		// (remove) Token: 0x060048DB RID: 18651 RVA: 0x001283D2 File Offset: 0x001273D2
		[WebCategory("Action")]
		[WebSysDescription("Login_LoginError")]
		public event EventHandler LoginError
		{
			add
			{
				base.Events.AddHandler(Login.EventLoginError, value);
			}
			remove
			{
				base.Events.RemoveHandler(Login.EventLoginError, value);
			}
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x001283E8 File Offset: 0x001273E8
		private void AttemptLogin()
		{
			if (this.Page != null && !this.Page.IsValid)
			{
				return;
			}
			LoginCancelEventArgs loginCancelEventArgs = new LoginCancelEventArgs();
			this.OnLoggingIn(loginCancelEventArgs);
			if (loginCancelEventArgs.Cancel)
			{
				return;
			}
			AuthenticateEventArgs authenticateEventArgs = new AuthenticateEventArgs();
			this.OnAuthenticate(authenticateEventArgs);
			if (authenticateEventArgs.Authenticated)
			{
				FormsAuthentication.SetAuthCookie(this.UserNameInternal, this.RememberMeSet);
				this.OnLoggedIn(EventArgs.Empty);
				this.Page.Response.Redirect(this.GetRedirectUrl(), false);
				return;
			}
			this.OnLoginError(EventArgs.Empty);
			if (this.FailureAction == LoginFailureAction.RedirectToLoginPage)
			{
				FormsAuthentication.RedirectToLoginPage("loginfailure=1");
			}
			ITextControl textControl = (ITextControl)this.TemplateContainer.FailureTextLabel;
			if (textControl != null)
			{
				textControl.Text = this.FailureText;
			}
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x001284A8 File Offset: 0x001274A8
		private void AuthenticateUsingMembershipProvider(AuthenticateEventArgs e)
		{
			MembershipProvider provider = LoginUtil.GetProvider(this.MembershipProvider);
			e.Authenticated = provider.ValidateUser(this.UserNameInternal, this.PasswordInternal);
		}

		// Token: 0x060048DE RID: 18654 RVA: 0x001284DC File Offset: 0x001274DC
		protected internal override void CreateChildControls()
		{
			this.Controls.Clear();
			this._templateContainer = new Login.LoginContainer(this);
			this._templateContainer.RenderDesignerRegion = this._renderDesignerRegion;
			ITemplate template = this.LayoutTemplate;
			if (template == null)
			{
				this._templateContainer.EnableViewState = false;
				this._templateContainer.EnableTheming = false;
				template = new Login.LoginTemplate(this);
			}
			template.InstantiateIn(this._templateContainer);
			this._templateContainer.Visible = true;
			this.Controls.Add(this._templateContainer);
			this.SetEditableChildProperties();
			IEditableTextControl editableTextControl = this._templateContainer.UserNameTextBox as IEditableTextControl;
			if (editableTextControl != null)
			{
				editableTextControl.TextChanged += this.UserNameTextChanged;
			}
			IEditableTextControl editableTextControl2 = this._templateContainer.PasswordTextBox as IEditableTextControl;
			if (editableTextControl2 != null)
			{
				editableTextControl2.TextChanged += this.PasswordTextChanged;
			}
			ICheckBoxControl checkBoxControl = (ICheckBoxControl)this._templateContainer.RememberMeCheckBox;
			if (checkBoxControl != null)
			{
				checkBoxControl.CheckedChanged += this.RememberMeCheckedChanged;
			}
		}

		// Token: 0x060048DF RID: 18655 RVA: 0x001285DC File Offset: 0x001275DC
		private string GetRedirectUrl()
		{
			if (this.OnLoginPage())
			{
				string returnUrl = FormsAuthentication.GetReturnUrl(false);
				if (!string.IsNullOrEmpty(returnUrl))
				{
					return returnUrl;
				}
				string destinationPageUrl = this.DestinationPageUrl;
				if (!string.IsNullOrEmpty(destinationPageUrl))
				{
					return base.ResolveClientUrl(destinationPageUrl);
				}
				return FormsAuthentication.DefaultUrl;
			}
			else
			{
				string destinationPageUrl2 = this.DestinationPageUrl;
				if (!string.IsNullOrEmpty(destinationPageUrl2))
				{
					return base.ResolveClientUrl(destinationPageUrl2);
				}
				if (this.Page.Form != null && string.Equals(this.Page.Form.Method, "get", StringComparison.OrdinalIgnoreCase))
				{
					return this.Page.Request.Path;
				}
				return this.Page.Request.PathWithQueryString;
			}
		}

		// Token: 0x060048E0 RID: 18656 RVA: 0x00128684 File Offset: 0x00127684
		protected override void LoadViewState(object savedState)
		{
			if (savedState == null)
			{
				base.LoadViewState(null);
				return;
			}
			object[] array = (object[])savedState;
			if (array.Length != 10)
			{
				throw new ArgumentException(SR.GetString("ViewState_InvalidViewState"));
			}
			base.LoadViewState(array[0]);
			if (array[1] != null)
			{
				((IStateManager)this.LoginButtonStyle).LoadViewState(array[1]);
			}
			if (array[2] != null)
			{
				((IStateManager)this.LabelStyle).LoadViewState(array[2]);
			}
			if (array[3] != null)
			{
				((IStateManager)this.TextBoxStyle).LoadViewState(array[3]);
			}
			if (array[4] != null)
			{
				((IStateManager)this.HyperLinkStyle).LoadViewState(array[4]);
			}
			if (array[5] != null)
			{
				((IStateManager)this.InstructionTextStyle).LoadViewState(array[5]);
			}
			if (array[6] != null)
			{
				((IStateManager)this.TitleTextStyle).LoadViewState(array[6]);
			}
			if (array[7] != null)
			{
				((IStateManager)this.CheckBoxStyle).LoadViewState(array[7]);
			}
			if (array[8] != null)
			{
				((IStateManager)this.FailureTextStyle).LoadViewState(array[8]);
			}
			if (array[9] != null)
			{
				((IStateManager)this.ValidatorTextStyle).LoadViewState(array[9]);
			}
		}

		// Token: 0x060048E1 RID: 18657 RVA: 0x00128770 File Offset: 0x00127770
		protected virtual void OnLoggedIn(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Login.EventLoggedIn];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060048E2 RID: 18658 RVA: 0x001287A0 File Offset: 0x001277A0
		protected virtual void OnAuthenticate(AuthenticateEventArgs e)
		{
			AuthenticateEventHandler authenticateEventHandler = (AuthenticateEventHandler)base.Events[Login.EventAuthenticate];
			if (authenticateEventHandler != null)
			{
				authenticateEventHandler(this, e);
				return;
			}
			this.AuthenticateUsingMembershipProvider(e);
		}

		// Token: 0x060048E3 RID: 18659 RVA: 0x001287D8 File Offset: 0x001277D8
		protected virtual void OnLoggingIn(LoginCancelEventArgs e)
		{
			LoginCancelEventHandler loginCancelEventHandler = (LoginCancelEventHandler)base.Events[Login.EventLoggingIn];
			if (loginCancelEventHandler != null)
			{
				loginCancelEventHandler(this, e);
			}
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x00128808 File Offset: 0x00127808
		protected override bool OnBubbleEvent(object source, EventArgs e)
		{
			bool flag = false;
			if (e is CommandEventArgs)
			{
				CommandEventArgs commandEventArgs = (CommandEventArgs)e;
				if (string.Equals(commandEventArgs.CommandName, Login.LoginButtonCommandName, StringComparison.OrdinalIgnoreCase))
				{
					this.AttemptLogin();
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x00128844 File Offset: 0x00127844
		protected virtual void OnLoginError(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Login.EventLoginError];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060048E6 RID: 18662 RVA: 0x00128872 File Offset: 0x00127872
		private bool OnLoginPage()
		{
			return AuthenticationConfig.AccessingLoginPage(this.Context, FormsAuthentication.LoginUrl);
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x00128884 File Offset: 0x00127884
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			this.SetEditableChildProperties();
			this.TemplateContainer.Visible = this.VisibleWhenLoggedIn || !this.Page.Request.IsAuthenticated || this.OnLoginPage();
		}

		// Token: 0x060048E8 RID: 18664 RVA: 0x001288C1 File Offset: 0x001278C1
		private void PasswordTextChanged(object source, EventArgs e)
		{
			this._password = ((ITextControl)source).Text;
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x001288D4 File Offset: 0x001278D4
		private bool RedirectedFromFailedLogin()
		{
			return !base.DesignMode && this.Page != null && !this.Page.IsPostBack && this.Page.Request.QueryString["loginfailure"] != null;
		}

		// Token: 0x060048EA RID: 18666 RVA: 0x00128927 File Offset: 0x00127927
		private void RememberMeCheckedChanged(object source, EventArgs e)
		{
			this.RememberMeSet = ((ICheckBoxControl)source).Checked;
		}

		// Token: 0x060048EB RID: 18667 RVA: 0x0012893C File Offset: 0x0012793C
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			if (base.DesignMode)
			{
				base.ChildControlsCreated = false;
				this.EnsureChildControls();
			}
			if (this.TemplateContainer.Visible)
			{
				this.SetChildProperties();
				this.RenderContents(writer);
			}
		}

		// Token: 0x060048EC RID: 18668 RVA: 0x0012898C File Offset: 0x0012798C
		protected override object SaveViewState()
		{
			object[] array = new object[]
			{
				base.SaveViewState(),
				(this._loginButtonStyle != null) ? ((IStateManager)this._loginButtonStyle).SaveViewState() : null,
				(this._labelStyle != null) ? ((IStateManager)this._labelStyle).SaveViewState() : null,
				(this._textBoxStyle != null) ? ((IStateManager)this._textBoxStyle).SaveViewState() : null,
				(this._hyperLinkStyle != null) ? ((IStateManager)this._hyperLinkStyle).SaveViewState() : null,
				(this._instructionTextStyle != null) ? ((IStateManager)this._instructionTextStyle).SaveViewState() : null,
				(this._titleTextStyle != null) ? ((IStateManager)this._titleTextStyle).SaveViewState() : null,
				(this._checkBoxStyle != null) ? ((IStateManager)this._checkBoxStyle).SaveViewState() : null,
				(this._failureTextStyle != null) ? ((IStateManager)this._failureTextStyle).SaveViewState() : null,
				(this._validatorTextStyle != null) ? ((IStateManager)this._validatorTextStyle).SaveViewState() : null
			};
			for (int i = 0; i < 10; i++)
			{
				if (array[i] != null)
				{
					return array;
				}
			}
			return null;
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x00128AA1 File Offset: 0x00127AA1
		internal void SetChildProperties()
		{
			this.SetCommonChildProperties();
			if (this.LayoutTemplate == null)
			{
				this.SetDefaultTemplateChildProperties();
			}
		}

		// Token: 0x060048EE RID: 18670 RVA: 0x00128AB8 File Offset: 0x00127AB8
		private void SetCommonChildProperties()
		{
			Login.LoginContainer templateContainer = this.TemplateContainer;
			Util.CopyBaseAttributesToInnerControl(this, templateContainer);
			templateContainer.ApplyStyle(base.ControlStyle);
			ITextControl textControl = (ITextControl)templateContainer.FailureTextLabel;
			string failureText = this.FailureText;
			if (textControl != null && failureText.Length > 0 && this.RedirectedFromFailedLogin())
			{
				textControl.Text = failureText;
			}
		}

		// Token: 0x060048EF RID: 18671 RVA: 0x00128B10 File Offset: 0x00127B10
		private void SetDefaultTemplateChildProperties()
		{
			/*
An exception occurred when decompiling this method (060048EF)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Web.UI.WebControls.Login::SetDefaultTemplateChildProperties()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 1052
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body, HashSet`1 ehs) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 959
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 280
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x060048F0 RID: 18672 RVA: 0x00129180 File Offset: 0x00128180
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		protected override void SetDesignModeState(IDictionary data)
		{
			if (data != null)
			{
				object obj = data["ConvertToTemplate"];
				if (obj != null)
				{
					this._convertingToTemplate = (bool)obj;
				}
				obj = data["RegionEditing"];
				if (obj != null)
				{
					this._renderDesignerRegion = (bool)obj;
				}
			}
		}

		// Token: 0x060048F1 RID: 18673 RVA: 0x001291C8 File Offset: 0x001281C8
		private void SetEditableChildProperties()
		{
			Login.LoginContainer templateContainer = this.TemplateContainer;
			string userNameInternal = this.UserNameInternal;
			if (!string.IsNullOrEmpty(userNameInternal))
			{
				ITextControl textControl = (ITextControl)templateContainer.UserNameTextBox;
				if (textControl != null)
				{
					textControl.Text = userNameInternal;
				}
			}
			ICheckBoxControl checkBoxControl = (ICheckBoxControl)templateContainer.RememberMeCheckBox;
			if (checkBoxControl != null)
			{
				if (this.LayoutTemplate == null)
				{
					LoginUtil.SetTableCellVisible(templateContainer.RememberMeCheckBox, this.DisplayRememberMe);
				}
				checkBoxControl.Checked = this.RememberMeSet;
			}
		}

		// Token: 0x060048F2 RID: 18674 RVA: 0x00129238 File Offset: 0x00128238
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this._loginButtonStyle != null)
			{
				((IStateManager)this._loginButtonStyle).TrackViewState();
			}
			if (this._labelStyle != null)
			{
				((IStateManager)this._labelStyle).TrackViewState();
			}
			if (this._textBoxStyle != null)
			{
				((IStateManager)this._textBoxStyle).TrackViewState();
			}
			if (this._hyperLinkStyle != null)
			{
				((IStateManager)this._hyperLinkStyle).TrackViewState();
			}
			if (this._instructionTextStyle != null)
			{
				((IStateManager)this._instructionTextStyle).TrackViewState();
			}
			if (this._titleTextStyle != null)
			{
				((IStateManager)this._titleTextStyle).TrackViewState();
			}
			if (this._checkBoxStyle != null)
			{
				((IStateManager)this._checkBoxStyle).TrackViewState();
			}
			if (this._failureTextStyle != null)
			{
				((IStateManager)this._failureTextStyle).TrackViewState();
			}
			if (this._validatorTextStyle != null)
			{
				((IStateManager)this._validatorTextStyle).TrackViewState();
			}
		}

		// Token: 0x060048F3 RID: 18675 RVA: 0x001292F6 File Offset: 0x001282F6
		private void UserNameTextChanged(object source, EventArgs e)
		{
			this.UserName = ((ITextControl)source).Text;
		}

		// Token: 0x04002AE1 RID: 10977
		private const string _userNameID = "UserName";

		// Token: 0x04002AE2 RID: 10978
		private const string _passwordID = "Password";

		// Token: 0x04002AE3 RID: 10979
		private const string _rememberMeID = "RememberMe";

		// Token: 0x04002AE4 RID: 10980
		private const string _failureTextID = "FailureText";

		// Token: 0x04002AE5 RID: 10981
		private const string _userNameRequiredID = "UserNameRequired";

		// Token: 0x04002AE6 RID: 10982
		private const string _passwordRequiredID = "PasswordRequired";

		// Token: 0x04002AE7 RID: 10983
		private const string _pushButtonID = "LoginButton";

		// Token: 0x04002AE8 RID: 10984
		private const string _imageButtonID = "LoginImageButton";

		// Token: 0x04002AE9 RID: 10985
		private const string _linkButtonID = "LoginLinkButton";

		// Token: 0x04002AEA RID: 10986
		private const string _passwordRecoveryLinkID = "PasswordRecoveryLink";

		// Token: 0x04002AEB RID: 10987
		private const string _helpLinkID = "HelpLink";

		// Token: 0x04002AEC RID: 10988
		private const string _createUserLinkID = "CreateUserLink";

		// Token: 0x04002AED RID: 10989
		private const string _failureParameterName = "loginfailure";

		// Token: 0x04002AEE RID: 10990
		private const ValidatorDisplay _requiredFieldValidatorDisplay = ValidatorDisplay.Static;

		// Token: 0x04002AEF RID: 10991
		private const int _viewStateArrayLength = 10;

		// Token: 0x04002AF0 RID: 10992
		public static readonly string LoginButtonCommandName = "Login";

		// Token: 0x04002AF1 RID: 10993
		private ITemplate _loginTemplate;

		// Token: 0x04002AF2 RID: 10994
		private Login.LoginContainer _templateContainer;

		// Token: 0x04002AF3 RID: 10995
		private string _password;

		// Token: 0x04002AF4 RID: 10996
		private bool _convertingToTemplate;

		// Token: 0x04002AF5 RID: 10997
		private bool _renderDesignerRegion;

		// Token: 0x04002AF6 RID: 10998
		private Style _loginButtonStyle;

		// Token: 0x04002AF7 RID: 10999
		private TableItemStyle _labelStyle;

		// Token: 0x04002AF8 RID: 11000
		private Style _textBoxStyle;

		// Token: 0x04002AF9 RID: 11001
		private TableItemStyle _hyperLinkStyle;

		// Token: 0x04002AFA RID: 11002
		private TableItemStyle _instructionTextStyle;

		// Token: 0x04002AFB RID: 11003
		private TableItemStyle _titleTextStyle;

		// Token: 0x04002AFC RID: 11004
		private TableItemStyle _checkBoxStyle;

		// Token: 0x04002AFD RID: 11005
		private TableItemStyle _failureTextStyle;

		// Token: 0x04002AFE RID: 11006
		private Style _validatorTextStyle;

		// Token: 0x04002AFF RID: 11007
		private static readonly object EventLoggingIn = new object();

		// Token: 0x04002B00 RID: 11008
		private static readonly object EventAuthenticate = new object();

		// Token: 0x04002B01 RID: 11009
		private static readonly object EventLoggedIn = new object();

		// Token: 0x04002B02 RID: 11010
		private static readonly object EventLoginError = new object();

		// Token: 0x020005D2 RID: 1490
		private sealed class LoginTemplate : ITemplate
		{
			// Token: 0x060048F6 RID: 18678 RVA: 0x00129345 File Offset: 0x00128345
			public LoginTemplate(Login owner)
			{
				this._owner = owner;
			}

			// Token: 0x060048F7 RID: 18679 RVA: 0x00129354 File Offset: 0x00128354
			private void CreateControls(Login.LoginContainer loginContainer)
			{
				string uniqueID = this._owner.UniqueID;
				Literal literal = new Literal();
				loginContainer.Title = literal;
				Literal literal2 = new Literal();
				loginContainer.Instruction = literal2;
				TextBox textBox = new TextBox();
				textBox.ID = "UserName";
				loginContainer.UserNameTextBox = textBox;
				LabelLiteral labelLiteral = new LabelLiteral(textBox);
				loginContainer.UserNameLabel = labelLiteral;
				bool flag = true;
				loginContainer.UserNameRequired = new RequiredFieldValidator
				{
					ID = "UserNameRequired",
					ValidationGroup = uniqueID,
					ControlToValidate = textBox.ID,
					Display = ValidatorDisplay.Static,
					Text = SR.GetString("LoginControls_DefaultRequiredFieldValidatorText"),
					Enabled = flag,
					Visible = flag
				};
				TextBox textBox2 = new TextBox();
				textBox2.ID = "Password";
				textBox2.TextMode = TextBoxMode.Password;
				loginContainer.PasswordTextBox = textBox2;
				LabelLiteral labelLiteral2 = new LabelLiteral(textBox2);
				loginContainer.PasswordLabel = labelLiteral2;
				loginContainer.PasswordRequired = new RequiredFieldValidator
				{
					ID = "PasswordRequired",
					ValidationGroup = uniqueID,
					ControlToValidate = textBox2.ID,
					Display = ValidatorDisplay.Static,
					Text = SR.GetString("LoginControls_DefaultRequiredFieldValidatorText"),
					Enabled = flag,
					Visible = flag
				};
				loginContainer.RememberMeCheckBox = new CheckBox
				{
					ID = "RememberMe"
				};
				loginContainer.LinkButton = new LinkButton
				{
					ID = "LoginLinkButton",
					ValidationGroup = uniqueID,
					CommandName = Login.LoginButtonCommandName
				};
				loginContainer.ImageButton = new ImageButton
				{
					ID = "LoginImageButton",
					ValidationGroup = uniqueID,
					CommandName = Login.LoginButtonCommandName
				};
				loginContainer.PushButton = new Button
				{
					ID = "LoginButton",
					ValidationGroup = uniqueID,
					CommandName = Login.LoginButtonCommandName
				};
				HyperLink hyperLink = new HyperLink();
				loginContainer.PasswordRecoveryLink = hyperLink;
				LiteralControl literalControl = new LiteralControl();
				hyperLink.ID = "PasswordRecoveryLink";
				loginContainer.PasswordRecoveryLinkSeparator = literalControl;
				HyperLink hyperLink2 = new HyperLink();
				loginContainer.CreateUserLink = hyperLink2;
				hyperLink2.ID = "CreateUserLink";
				LiteralControl literalControl2 = new LiteralControl();
				loginContainer.CreateUserLinkSeparator = literalControl2;
				loginContainer.HelpPageLink = new HyperLink
				{
					ID = "HelpLink"
				};
				loginContainer.FailureTextLabel = new Literal
				{
					ID = "FailureText"
				};
				loginContainer.PasswordRecoveryIcon = new Image();
				loginContainer.HelpPageIcon = new Image();
				loginContainer.CreateUserIcon = new Image();
			}

			// Token: 0x060048F8 RID: 18680 RVA: 0x001295F0 File Offset: 0x001285F0
			private void LayoutControls(Login.LoginContainer loginContainer)
			{
				Orientation orientation = this._owner.Orientation;
				LoginTextLayout textLayout = this._owner.TextLayout;
				if (orientation == Orientation.Vertical && textLayout == LoginTextLayout.TextOnLeft)
				{
					this.LayoutVerticalTextOnLeft(loginContainer);
					return;
				}
				if (orientation == Orientation.Vertical && textLayout == LoginTextLayout.TextOnTop)
				{
					this.LayoutVerticalTextOnTop(loginContainer);
					return;
				}
				if (orientation == Orientation.Horizontal && textLayout == LoginTextLayout.TextOnLeft)
				{
					this.LayoutHorizontalTextOnLeft(loginContainer);
					return;
				}
				this.LayoutHorizontalTextOnTop(loginContainer);
			}

			// Token: 0x060048F9 RID: 18681 RVA: 0x0012964C File Offset: 0x0012864C
			private void LayoutHorizontalTextOnLeft(Login.LoginContainer loginContainer)
			{
				Table table = new Table();
				table.CellPadding = 0;
				TableRow tableRow = new LoginUtil.DisappearingTableRow();
				TableCell tableCell = new TableCell();
				tableCell.ColumnSpan = 6;
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.Title);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 6;
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.Instruction);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				if (this._owner.ConvertingToTemplate)
				{
					loginContainer.UserNameLabel.RenderAsLabel = true;
				}
				tableCell.Controls.Add(loginContainer.UserNameLabel);
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				tableCell.Controls.Add(loginContainer.UserNameTextBox);
				tableCell.Controls.Add(loginContainer.UserNameRequired);
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				if (this._owner.ConvertingToTemplate)
				{
					loginContainer.PasswordLabel.RenderAsLabel = true;
				}
				tableCell.Controls.Add(loginContainer.PasswordLabel);
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				tableCell.Controls.Add(loginContainer.PasswordTextBox);
				tableCell.Controls.Add(loginContainer.PasswordRequired);
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				tableCell.Controls.Add(loginContainer.RememberMeCheckBox);
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				tableCell.Controls.Add(loginContainer.LinkButton);
				tableCell.Controls.Add(loginContainer.ImageButton);
				tableCell.Controls.Add(loginContainer.PushButton);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 6;
				tableCell.Controls.Add(loginContainer.FailureTextLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 6;
				tableCell.Controls.Add(loginContainer.CreateUserIcon);
				tableCell.Controls.Add(loginContainer.CreateUserLink);
				loginContainer.CreateUserLinkSeparator.Text = " ";
				tableCell.Controls.Add(loginContainer.CreateUserLinkSeparator);
				tableCell.Controls.Add(loginContainer.PasswordRecoveryIcon);
				tableCell.Controls.Add(loginContainer.PasswordRecoveryLink);
				loginContainer.PasswordRecoveryLinkSeparator.Text = " ";
				tableCell.Controls.Add(loginContainer.PasswordRecoveryLinkSeparator);
				tableCell.Controls.Add(loginContainer.HelpPageIcon);
				tableCell.Controls.Add(loginContainer.HelpPageLink);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				Table table2 = LoginUtil.CreateChildTable(this._owner.ConvertingToTemplate);
				tableRow = new TableRow();
				tableCell = new TableCell();
				tableCell.Controls.Add(table);
				tableRow.Cells.Add(tableCell);
				table2.Rows.Add(tableRow);
				loginContainer.LayoutTable = table;
				loginContainer.BorderTable = table2;
				loginContainer.Controls.Add(table2);
			}

			// Token: 0x060048FA RID: 18682 RVA: 0x001299C4 File Offset: 0x001289C4
			private void LayoutHorizontalTextOnTop(Login.LoginContainer loginContainer)
			{
				Table table = new Table();
				table.CellPadding = 0;
				TableRow tableRow = new LoginUtil.DisappearingTableRow();
				TableCell tableCell = new TableCell();
				tableCell.ColumnSpan = 4;
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.Title);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 4;
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.Instruction);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				if (this._owner.ConvertingToTemplate)
				{
					loginContainer.UserNameLabel.RenderAsLabel = true;
				}
				tableCell.Controls.Add(loginContainer.UserNameLabel);
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				if (this._owner.ConvertingToTemplate)
				{
					loginContainer.PasswordLabel.RenderAsLabel = true;
				}
				tableCell.Controls.Add(loginContainer.PasswordLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.Controls.Add(loginContainer.UserNameTextBox);
				tableCell.Controls.Add(loginContainer.UserNameRequired);
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				tableCell.Controls.Add(loginContainer.PasswordTextBox);
				tableCell.Controls.Add(loginContainer.PasswordRequired);
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				tableCell.Controls.Add(loginContainer.RememberMeCheckBox);
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				tableCell.Controls.Add(loginContainer.LinkButton);
				tableCell.Controls.Add(loginContainer.ImageButton);
				tableCell.Controls.Add(loginContainer.PushButton);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 4;
				tableCell.Controls.Add(loginContainer.FailureTextLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 4;
				tableCell.Controls.Add(loginContainer.CreateUserIcon);
				tableCell.Controls.Add(loginContainer.CreateUserLink);
				loginContainer.CreateUserLinkSeparator.Text = " ";
				tableCell.Controls.Add(loginContainer.CreateUserLinkSeparator);
				tableCell.Controls.Add(loginContainer.PasswordRecoveryIcon);
				tableCell.Controls.Add(loginContainer.PasswordRecoveryLink);
				loginContainer.PasswordRecoveryLinkSeparator.Text = " ";
				tableCell.Controls.Add(loginContainer.PasswordRecoveryLinkSeparator);
				tableCell.Controls.Add(loginContainer.HelpPageIcon);
				tableCell.Controls.Add(loginContainer.HelpPageLink);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				Table table2 = LoginUtil.CreateChildTable(this._owner.ConvertingToTemplate);
				tableRow = new TableRow();
				tableCell = new TableCell();
				tableCell.Controls.Add(table);
				tableRow.Cells.Add(tableCell);
				table2.Rows.Add(tableRow);
				loginContainer.LayoutTable = table;
				loginContainer.BorderTable = table2;
				loginContainer.Controls.Add(table2);
			}

			// Token: 0x060048FB RID: 18683 RVA: 0x00129D54 File Offset: 0x00128D54
			private void LayoutVerticalTextOnLeft(Login.LoginContainer loginContainer)
			{
				Table table = new Table();
				table.CellPadding = 0;
				TableRow tableRow = new LoginUtil.DisappearingTableRow();
				TableCell tableCell = new TableCell();
				tableCell.ColumnSpan = 2;
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.Title);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 2;
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.Instruction);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				if (this._owner.ConvertingToTemplate)
				{
					loginContainer.UserNameLabel.RenderAsLabel = true;
				}
				tableCell.Controls.Add(loginContainer.UserNameLabel);
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				tableCell.Controls.Add(loginContainer.UserNameTextBox);
				tableCell.Controls.Add(loginContainer.UserNameRequired);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				if (this._owner.ConvertingToTemplate)
				{
					loginContainer.PasswordLabel.RenderAsLabel = true;
				}
				tableCell.Controls.Add(loginContainer.PasswordLabel);
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				tableCell.Controls.Add(loginContainer.PasswordTextBox);
				tableCell.Controls.Add(loginContainer.PasswordRequired);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 2;
				tableCell.Controls.Add(loginContainer.RememberMeCheckBox);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 2;
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.FailureTextLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 2;
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				tableCell.Controls.Add(loginContainer.LinkButton);
				tableCell.Controls.Add(loginContainer.ImageButton);
				tableCell.Controls.Add(loginContainer.PushButton);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 2;
				tableCell.Controls.Add(loginContainer.CreateUserIcon);
				tableCell.Controls.Add(loginContainer.CreateUserLink);
				tableCell.Controls.Add(loginContainer.CreateUserLinkSeparator);
				tableCell.Controls.Add(loginContainer.PasswordRecoveryIcon);
				tableCell.Controls.Add(loginContainer.PasswordRecoveryLink);
				loginContainer.PasswordRecoveryLinkSeparator.Text = "<br />";
				loginContainer.CreateUserLinkSeparator.Text = "<br />";
				tableCell.Controls.Add(loginContainer.PasswordRecoveryLinkSeparator);
				tableCell.Controls.Add(loginContainer.HelpPageIcon);
				tableCell.Controls.Add(loginContainer.HelpPageLink);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				Table table2 = LoginUtil.CreateChildTable(this._owner.ConvertingToTemplate);
				tableRow = new TableRow();
				tableCell = new TableCell();
				tableCell.Controls.Add(table);
				tableRow.Cells.Add(tableCell);
				table2.Rows.Add(tableRow);
				loginContainer.LayoutTable = table;
				loginContainer.BorderTable = table2;
				loginContainer.Controls.Add(table2);
			}

			// Token: 0x060048FC RID: 18684 RVA: 0x0012A12C File Offset: 0x0012912C
			private void LayoutVerticalTextOnTop(Login.LoginContainer loginContainer)
			{
				Table table = new Table();
				table.CellPadding = 0;
				TableRow tableRow = new LoginUtil.DisappearingTableRow();
				TableCell tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.Title);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.Instruction);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				if (this._owner.ConvertingToTemplate)
				{
					loginContainer.UserNameLabel.RenderAsLabel = true;
				}
				tableCell.Controls.Add(loginContainer.UserNameLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.Controls.Add(loginContainer.UserNameTextBox);
				tableCell.Controls.Add(loginContainer.UserNameRequired);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				if (this._owner.ConvertingToTemplate)
				{
					loginContainer.PasswordLabel.RenderAsLabel = true;
				}
				tableCell.Controls.Add(loginContainer.PasswordLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.Controls.Add(loginContainer.PasswordTextBox);
				tableCell.Controls.Add(loginContainer.PasswordRequired);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.Controls.Add(loginContainer.RememberMeCheckBox);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.FailureTextLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				tableCell.Controls.Add(loginContainer.LinkButton);
				tableCell.Controls.Add(loginContainer.ImageButton);
				tableCell.Controls.Add(loginContainer.PushButton);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.Controls.Add(loginContainer.CreateUserIcon);
				tableCell.Controls.Add(loginContainer.CreateUserLink);
				loginContainer.CreateUserLinkSeparator.Text = "<br />";
				tableCell.Controls.Add(loginContainer.CreateUserLinkSeparator);
				tableCell.Controls.Add(loginContainer.PasswordRecoveryIcon);
				tableCell.Controls.Add(loginContainer.PasswordRecoveryLink);
				loginContainer.PasswordRecoveryLinkSeparator.Text = "<br />";
				tableCell.Controls.Add(loginContainer.PasswordRecoveryLinkSeparator);
				tableCell.Controls.Add(loginContainer.HelpPageIcon);
				tableCell.Controls.Add(loginContainer.HelpPageLink);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				Table table2 = LoginUtil.CreateChildTable(this._owner.ConvertingToTemplate);
				tableRow = new TableRow();
				tableCell = new TableCell();
				tableCell.Controls.Add(table);
				tableRow.Cells.Add(tableCell);
				table2.Rows.Add(tableRow);
				loginContainer.LayoutTable = table;
				loginContainer.BorderTable = table2;
				loginContainer.Controls.Add(table2);
			}

			// Token: 0x060048FD RID: 18685 RVA: 0x0012A4F4 File Offset: 0x001294F4
			void ITemplate.InstantiateIn(Control container)
			{
				Login.LoginContainer loginContainer = (Login.LoginContainer)container;
				this.CreateControls(loginContainer);
				this.LayoutControls(loginContainer);
			}

			// Token: 0x04002B03 RID: 11011
			private Login _owner;
		}

		// Token: 0x020005D3 RID: 1491
		internal sealed class LoginContainer : LoginUtil.GenericContainer<Login>
		{
			// Token: 0x060048FE RID: 18686 RVA: 0x0012A516 File Offset: 0x00129516
			public LoginContainer(Login owner)
				: base(owner)
			{
			}

			// Token: 0x1700120C RID: 4620
			// (get) Token: 0x060048FF RID: 18687 RVA: 0x0012A51F File Offset: 0x0012951F
			protected override bool ConvertingToTemplate
			{
				get
				{
					return base.Owner.ConvertingToTemplate;
				}
			}

			// Token: 0x1700120D RID: 4621
			// (get) Token: 0x06004900 RID: 18688 RVA: 0x0012A52C File Offset: 0x0012952C
			// (set) Token: 0x06004901 RID: 18689 RVA: 0x0012A534 File Offset: 0x00129534
			internal HyperLink CreateUserLink
			{
				get
				{
					return this._createUserLink;
				}
				set
				{
					this._createUserLink = value;
				}
			}

			// Token: 0x1700120E RID: 4622
			// (get) Token: 0x06004902 RID: 18690 RVA: 0x0012A53D File Offset: 0x0012953D
			// (set) Token: 0x06004903 RID: 18691 RVA: 0x0012A545 File Offset: 0x00129545
			internal LiteralControl CreateUserLinkSeparator
			{
				get
				{
					return this._createUserLinkSeparator;
				}
				set
				{
					this._createUserLinkSeparator = value;
				}
			}

			// Token: 0x1700120F RID: 4623
			// (get) Token: 0x06004904 RID: 18692 RVA: 0x0012A54E File Offset: 0x0012954E
			// (set) Token: 0x06004905 RID: 18693 RVA: 0x0012A556 File Offset: 0x00129556
			internal Image PasswordRecoveryIcon
			{
				get
				{
					return this._passwordRecoveryIcon;
				}
				set
				{
					this._passwordRecoveryIcon = value;
				}
			}

			// Token: 0x17001210 RID: 4624
			// (get) Token: 0x06004906 RID: 18694 RVA: 0x0012A55F File Offset: 0x0012955F
			// (set) Token: 0x06004907 RID: 18695 RVA: 0x0012A567 File Offset: 0x00129567
			internal Image HelpPageIcon
			{
				get
				{
					return this._helpPageIcon;
				}
				set
				{
					this._helpPageIcon = value;
				}
			}

			// Token: 0x17001211 RID: 4625
			// (get) Token: 0x06004908 RID: 18696 RVA: 0x0012A570 File Offset: 0x00129570
			// (set) Token: 0x06004909 RID: 18697 RVA: 0x0012A578 File Offset: 0x00129578
			internal Image CreateUserIcon
			{
				get
				{
					return this._createUserIcon;
				}
				set
				{
					this._createUserIcon = value;
				}
			}

			// Token: 0x17001212 RID: 4626
			// (get) Token: 0x0600490A RID: 18698 RVA: 0x0012A581 File Offset: 0x00129581
			// (set) Token: 0x0600490B RID: 18699 RVA: 0x0012A59D File Offset: 0x0012959D
			internal Control FailureTextLabel
			{
				get
				{
					if (this._failureTextLabel != null)
					{
						return this._failureTextLabel;
					}
					return base.FindOptionalControl<ITextControl>("FailureText");
				}
				set
				{
					this._failureTextLabel = value;
				}
			}

			// Token: 0x17001213 RID: 4627
			// (get) Token: 0x0600490C RID: 18700 RVA: 0x0012A5A6 File Offset: 0x001295A6
			// (set) Token: 0x0600490D RID: 18701 RVA: 0x0012A5AE File Offset: 0x001295AE
			internal HyperLink HelpPageLink
			{
				get
				{
					return this._helpPageLink;
				}
				set
				{
					this._helpPageLink = value;
				}
			}

			// Token: 0x17001214 RID: 4628
			// (get) Token: 0x0600490E RID: 18702 RVA: 0x0012A5B7 File Offset: 0x001295B7
			// (set) Token: 0x0600490F RID: 18703 RVA: 0x0012A5BF File Offset: 0x001295BF
			internal ImageButton ImageButton
			{
				get
				{
					return this._imageButton;
				}
				set
				{
					this._imageButton = value;
				}
			}

			// Token: 0x17001215 RID: 4629
			// (get) Token: 0x06004910 RID: 18704 RVA: 0x0012A5C8 File Offset: 0x001295C8
			// (set) Token: 0x06004911 RID: 18705 RVA: 0x0012A5D0 File Offset: 0x001295D0
			internal Literal Instruction
			{
				get
				{
					return this._instruction;
				}
				set
				{
					this._instruction = value;
				}
			}

			// Token: 0x17001216 RID: 4630
			// (get) Token: 0x06004912 RID: 18706 RVA: 0x0012A5D9 File Offset: 0x001295D9
			// (set) Token: 0x06004913 RID: 18707 RVA: 0x0012A5E1 File Offset: 0x001295E1
			internal LinkButton LinkButton
			{
				get
				{
					return this._linkButton;
				}
				set
				{
					this._linkButton = value;
				}
			}

			// Token: 0x17001217 RID: 4631
			// (get) Token: 0x06004914 RID: 18708 RVA: 0x0012A5EA File Offset: 0x001295EA
			// (set) Token: 0x06004915 RID: 18709 RVA: 0x0012A5F2 File Offset: 0x001295F2
			internal LabelLiteral PasswordLabel
			{
				get
				{
					return this._passwordLabel;
				}
				set
				{
					this._passwordLabel = value;
				}
			}

			// Token: 0x17001218 RID: 4632
			// (get) Token: 0x06004916 RID: 18710 RVA: 0x0012A5FB File Offset: 0x001295FB
			// (set) Token: 0x06004917 RID: 18711 RVA: 0x0012A603 File Offset: 0x00129603
			internal HyperLink PasswordRecoveryLink
			{
				get
				{
					return this._passwordRecoveryLink;
				}
				set
				{
					this._passwordRecoveryLink = value;
				}
			}

			// Token: 0x17001219 RID: 4633
			// (get) Token: 0x06004918 RID: 18712 RVA: 0x0012A60C File Offset: 0x0012960C
			// (set) Token: 0x06004919 RID: 18713 RVA: 0x0012A614 File Offset: 0x00129614
			internal LiteralControl PasswordRecoveryLinkSeparator
			{
				get
				{
					return this._passwordRecoveryLinkSeparator;
				}
				set
				{
					this._passwordRecoveryLinkSeparator = value;
				}
			}

			// Token: 0x1700121A RID: 4634
			// (get) Token: 0x0600491A RID: 18714 RVA: 0x0012A61D File Offset: 0x0012961D
			// (set) Token: 0x0600491B RID: 18715 RVA: 0x0012A625 File Offset: 0x00129625
			internal RequiredFieldValidator PasswordRequired
			{
				get
				{
					return this._passwordRequired;
				}
				set
				{
					this._passwordRequired = value;
				}
			}

			// Token: 0x1700121B RID: 4635
			// (get) Token: 0x0600491C RID: 18716 RVA: 0x0012A62E File Offset: 0x0012962E
			// (set) Token: 0x0600491D RID: 18717 RVA: 0x0012A64F File Offset: 0x0012964F
			internal Control PasswordTextBox
			{
				get
				{
					if (this._passwordTextBox != null)
					{
						return this._passwordTextBox;
					}
					return base.FindRequiredControl<IEditableTextControl>("Password", "Login_NoPasswordTextBox");
				}
				set
				{
					this._passwordTextBox = value;
				}
			}

			// Token: 0x1700121C RID: 4636
			// (get) Token: 0x0600491E RID: 18718 RVA: 0x0012A658 File Offset: 0x00129658
			// (set) Token: 0x0600491F RID: 18719 RVA: 0x0012A660 File Offset: 0x00129660
			internal Button PushButton
			{
				get
				{
					return this._pushButton;
				}
				set
				{
					this._pushButton = value;
				}
			}

			// Token: 0x1700121D RID: 4637
			// (get) Token: 0x06004920 RID: 18720 RVA: 0x0012A669 File Offset: 0x00129669
			// (set) Token: 0x06004921 RID: 18721 RVA: 0x0012A685 File Offset: 0x00129685
			internal Control RememberMeCheckBox
			{
				get
				{
					if (this._rememberMeCheckBox != null)
					{
						return this._rememberMeCheckBox;
					}
					return base.FindOptionalControl<ICheckBoxControl>("RememberMe");
				}
				set
				{
					this._rememberMeCheckBox = value;
				}
			}

			// Token: 0x1700121E RID: 4638
			// (get) Token: 0x06004922 RID: 18722 RVA: 0x0012A68E File Offset: 0x0012968E
			// (set) Token: 0x06004923 RID: 18723 RVA: 0x0012A696 File Offset: 0x00129696
			internal Literal Title
			{
				get
				{
					return this._title;
				}
				set
				{
					this._title = value;
				}
			}

			// Token: 0x1700121F RID: 4639
			// (get) Token: 0x06004924 RID: 18724 RVA: 0x0012A69F File Offset: 0x0012969F
			// (set) Token: 0x06004925 RID: 18725 RVA: 0x0012A6A7 File Offset: 0x001296A7
			internal LabelLiteral UserNameLabel
			{
				get
				{
					return this._userNameLabel;
				}
				set
				{
					this._userNameLabel = value;
				}
			}

			// Token: 0x17001220 RID: 4640
			// (get) Token: 0x06004926 RID: 18726 RVA: 0x0012A6B0 File Offset: 0x001296B0
			// (set) Token: 0x06004927 RID: 18727 RVA: 0x0012A6B8 File Offset: 0x001296B8
			internal RequiredFieldValidator UserNameRequired
			{
				get
				{
					return this._userNameRequired;
				}
				set
				{
					this._userNameRequired = value;
				}
			}

			// Token: 0x17001221 RID: 4641
			// (get) Token: 0x06004928 RID: 18728 RVA: 0x0012A6C1 File Offset: 0x001296C1
			// (set) Token: 0x06004929 RID: 18729 RVA: 0x0012A6E2 File Offset: 0x001296E2
			internal Control UserNameTextBox
			{
				get
				{
					if (this._userNameTextBox != null)
					{
						return this._userNameTextBox;
					}
					return base.FindRequiredControl<IEditableTextControl>("UserName", "Login_NoUserNameTextBox");
				}
				set
				{
					this._userNameTextBox = value;
				}
			}

			// Token: 0x04002B04 RID: 11012
			private HyperLink _createUserLink;

			// Token: 0x04002B05 RID: 11013
			private LiteralControl _createUserLinkSeparator;

			// Token: 0x04002B06 RID: 11014
			private Control _failureTextLabel;

			// Token: 0x04002B07 RID: 11015
			private HyperLink _helpPageLink;

			// Token: 0x04002B08 RID: 11016
			private ImageButton _imageButton;

			// Token: 0x04002B09 RID: 11017
			private Literal _instruction;

			// Token: 0x04002B0A RID: 11018
			private LinkButton _linkButton;

			// Token: 0x04002B0B RID: 11019
			private LabelLiteral _passwordLabel;

			// Token: 0x04002B0C RID: 11020
			private HyperLink _passwordRecoveryLink;

			// Token: 0x04002B0D RID: 11021
			private LiteralControl _passwordRecoveryLinkSeparator;

			// Token: 0x04002B0E RID: 11022
			private RequiredFieldValidator _passwordRequired;

			// Token: 0x04002B0F RID: 11023
			private Control _passwordTextBox;

			// Token: 0x04002B10 RID: 11024
			private Button _pushButton;

			// Token: 0x04002B11 RID: 11025
			private Control _rememberMeCheckBox;

			// Token: 0x04002B12 RID: 11026
			private Literal _title;

			// Token: 0x04002B13 RID: 11027
			private LabelLiteral _userNameLabel;

			// Token: 0x04002B14 RID: 11028
			private RequiredFieldValidator _userNameRequired;

			// Token: 0x04002B15 RID: 11029
			private Control _userNameTextBox;

			// Token: 0x04002B16 RID: 11030
			private Image _createUserIcon;

			// Token: 0x04002B17 RID: 11031
			private Image _helpPageIcon;

			// Token: 0x04002B18 RID: 11032
			private Image _passwordRecoveryIcon;
		}
	}
}
