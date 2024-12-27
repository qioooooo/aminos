using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;
using System.Web.Security;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004E1 RID: 1249
	[Bindable(false)]
	[DefaultEvent("ChangedPassword")]
	[Designer("System.Web.UI.Design.WebControls.ChangePasswordDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ChangePassword : CompositeControl, INamingContainer
	{
		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x06003C28 RID: 15400 RVA: 0x000FD448 File Offset: 0x000FC448
		// (set) Token: 0x06003C29 RID: 15401 RVA: 0x000FD471 File Offset: 0x000FC471
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
					throw new ArgumentOutOfRangeException("value", SR.GetString("ChangePassword_InvalidBorderPadding"));
				}
				this.ViewState["BorderPadding"] = value;
			}
		}

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x06003C2A RID: 15402 RVA: 0x000FD4A4 File Offset: 0x000FC4A4
		// (set) Token: 0x06003C2B RID: 15403 RVA: 0x000FD4D1 File Offset: 0x000FC4D1
		[DefaultValue("")]
		[UrlProperty]
		[WebCategory("Appearance")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebSysDescription("ChangePassword_CancelButtonImageUrl")]
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

		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x06003C2C RID: 15404 RVA: 0x000FD4E4 File Offset: 0x000FC4E4
		[WebSysDescription("ChangePassword_CancelButtonStyle")]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
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

		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x06003C2D RID: 15405 RVA: 0x000FD514 File Offset: 0x000FC514
		// (set) Token: 0x06003C2E RID: 15406 RVA: 0x000FD546 File Offset: 0x000FC546
		[Localizable(true)]
		[WebSysDescription("ChangePassword_CancelButtonText")]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("ChangePassword_DefaultCancelButtonText")]
		public virtual string CancelButtonText
		{
			get
			{
				object obj = this.ViewState["CancelButtonText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("ChangePassword_DefaultCancelButtonText");
			}
			set
			{
				this.ViewState["CancelButtonText"] = value;
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x06003C2F RID: 15407 RVA: 0x000FD55C File Offset: 0x000FC55C
		// (set) Token: 0x06003C30 RID: 15408 RVA: 0x000FD585 File Offset: 0x000FC585
		[DefaultValue(ButtonType.Button)]
		[WebCategory("Appearance")]
		[WebSysDescription("ChangePassword_CancelButtonType")]
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
				if (value < ButtonType.Button || value > ButtonType.Link)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["CancelButtonType"] = value;
			}
		}

		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x06003C31 RID: 15409 RVA: 0x000FD5B0 File Offset: 0x000FC5B0
		// (set) Token: 0x06003C32 RID: 15410 RVA: 0x000FD5DD File Offset: 0x000FC5DD
		[WebCategory("Behavior")]
		[UrlProperty]
		[DefaultValue("")]
		[WebSysDescription("ChangePassword_CancelDestinationPageUrl")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Themeable(false)]
		public virtual string CancelDestinationPageUrl
		{
			get
			{
				object obj = this.ViewState["CancelDestinationPageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["CancelDestinationPageUrl"] = value;
			}
		}

		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x06003C33 RID: 15411 RVA: 0x000FD5F0 File Offset: 0x000FC5F0
		// (set) Token: 0x06003C34 RID: 15412 RVA: 0x000FD61D File Offset: 0x000FC61D
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[WebSysDescription("ChangePassword_ChangePasswordButtonImageUrl")]
		[UrlProperty]
		public virtual string ChangePasswordButtonImageUrl
		{
			get
			{
				object obj = this.ViewState["ChangePasswordButtonImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ChangePasswordButtonImageUrl"] = value;
			}
		}

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x06003C35 RID: 15413 RVA: 0x000FD630 File Offset: 0x000FC630
		[WebSysDescription("ChangePassword_ChangePasswordButtonStyle")]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public Style ChangePasswordButtonStyle
		{
			get
			{
				if (this._changePasswordButtonStyle == null)
				{
					this._changePasswordButtonStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._changePasswordButtonStyle).TrackViewState();
					}
				}
				return this._changePasswordButtonStyle;
			}
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x06003C36 RID: 15414 RVA: 0x000FD660 File Offset: 0x000FC660
		// (set) Token: 0x06003C37 RID: 15415 RVA: 0x000FD692 File Offset: 0x000FC692
		[WebCategory("Appearance")]
		[Localizable(true)]
		[WebSysDefaultValue("ChangePassword_DefaultChangePasswordButtonText")]
		[WebSysDescription("ChangePassword_ChangePasswordButtonText")]
		public virtual string ChangePasswordButtonText
		{
			get
			{
				object obj = this.ViewState["ChangePasswordButtonText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("ChangePassword_DefaultChangePasswordButtonText");
			}
			set
			{
				this.ViewState["ChangePasswordButtonText"] = value;
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06003C38 RID: 15416 RVA: 0x000FD6A8 File Offset: 0x000FC6A8
		// (set) Token: 0x06003C39 RID: 15417 RVA: 0x000FD6D1 File Offset: 0x000FC6D1
		[WebCategory("Appearance")]
		[WebSysDescription("ChangePassword_ChangePasswordButtonType")]
		[DefaultValue(ButtonType.Button)]
		public virtual ButtonType ChangePasswordButtonType
		{
			get
			{
				object obj = this.ViewState["ChangePasswordButtonType"];
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
				this.ViewState["ChangePasswordButtonType"] = value;
			}
		}

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06003C3A RID: 15418 RVA: 0x000FD6FC File Offset: 0x000FC6FC
		// (set) Token: 0x06003C3B RID: 15419 RVA: 0x000FD72E File Offset: 0x000FC72E
		[Localizable(true)]
		[WebSysDescription("ChangePassword_ChangePasswordFailureText")]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("ChangePassword_DefaultChangePasswordFailureText")]
		public virtual string ChangePasswordFailureText
		{
			get
			{
				object obj = this.ViewState["ChangePasswordFailureText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("ChangePassword_DefaultChangePasswordFailureText");
			}
			set
			{
				this.ViewState["ChangePasswordFailureText"] = value;
			}
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x06003C3C RID: 15420 RVA: 0x000FD741 File Offset: 0x000FC741
		// (set) Token: 0x06003C3D RID: 15421 RVA: 0x000FD749 File Offset: 0x000FC749
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(ChangePassword))]
		public virtual ITemplate ChangePasswordTemplate
		{
			get
			{
				return this._changePasswordTemplate;
			}
			set
			{
				this._changePasswordTemplate = value;
				base.ChildControlsCreated = false;
			}
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x06003C3E RID: 15422 RVA: 0x000FD759 File Offset: 0x000FC759
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Control ChangePasswordTemplateContainer
		{
			get
			{
				this.EnsureChildControls();
				return this._changePasswordContainer;
			}
		}

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x06003C3F RID: 15423 RVA: 0x000FD768 File Offset: 0x000FC768
		// (set) Token: 0x06003C40 RID: 15424 RVA: 0x000FD79A File Offset: 0x000FC79A
		[WebSysDefaultValue("ChangePassword_DefaultChangePasswordTitleText")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		[WebSysDescription("LoginControls_TitleText")]
		public virtual string ChangePasswordTitleText
		{
			get
			{
				object obj = this.ViewState["ChangePasswordTitleText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("ChangePassword_DefaultChangePasswordTitleText");
			}
			set
			{
				this.ViewState["ChangePasswordTitleText"] = value;
			}
		}

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x06003C41 RID: 15425 RVA: 0x000FD7AD File Offset: 0x000FC7AD
		[Themeable(false)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Filterable(false)]
		public virtual string ConfirmNewPassword
		{
			get
			{
				if (this._confirmNewPassword != null)
				{
					return this._confirmNewPassword;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x06003C42 RID: 15426 RVA: 0x000FD7C4 File Offset: 0x000FC7C4
		// (set) Token: 0x06003C43 RID: 15427 RVA: 0x000FD7F6 File Offset: 0x000FC7F6
		[Localizable(true)]
		[WebSysDescription("ChangePassword_ConfirmNewPasswordLabelText")]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("ChangePassword_DefaultConfirmNewPasswordLabelText")]
		public virtual string ConfirmNewPasswordLabelText
		{
			get
			{
				object obj = this.ViewState["ConfirmNewPasswordLabelText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("ChangePassword_DefaultConfirmNewPasswordLabelText");
			}
			set
			{
				this.ViewState["ConfirmNewPasswordLabelText"] = value;
			}
		}

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06003C44 RID: 15428 RVA: 0x000FD80C File Offset: 0x000FC80C
		// (set) Token: 0x06003C45 RID: 15429 RVA: 0x000FD83E File Offset: 0x000FC83E
		[WebCategory("Validation")]
		[WebSysDescription("ChangePassword_ConfirmPasswordCompareErrorMessage")]
		[Localizable(true)]
		[WebSysDefaultValue("ChangePassword_DefaultConfirmPasswordCompareErrorMessage")]
		public virtual string ConfirmPasswordCompareErrorMessage
		{
			get
			{
				object obj = this.ViewState["ConfirmPasswordCompareErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("ChangePassword_DefaultConfirmPasswordCompareErrorMessage");
			}
			set
			{
				this.ViewState["ConfirmPasswordCompareErrorMessage"] = value;
			}
		}

		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06003C46 RID: 15430 RVA: 0x000FD854 File Offset: 0x000FC854
		// (set) Token: 0x06003C47 RID: 15431 RVA: 0x000FD886 File Offset: 0x000FC886
		[WebSysDescription("LoginControls_ConfirmPasswordRequiredErrorMessage")]
		[Localizable(true)]
		[WebCategory("Validation")]
		[WebSysDefaultValue("ChangePassword_DefaultConfirmPasswordRequiredErrorMessage")]
		public virtual string ConfirmPasswordRequiredErrorMessage
		{
			get
			{
				object obj = this.ViewState["ConfirmPasswordRequiredErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("ChangePassword_DefaultConfirmPasswordRequiredErrorMessage");
			}
			set
			{
				this.ViewState["ConfirmPasswordRequiredErrorMessage"] = value;
			}
		}

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x06003C48 RID: 15432 RVA: 0x000FD89C File Offset: 0x000FC89C
		// (set) Token: 0x06003C49 RID: 15433 RVA: 0x000FD8C9 File Offset: 0x000FC8C9
		[DefaultValue("")]
		[UrlProperty]
		[WebCategory("Appearance")]
		[WebSysDescription("ChangePassword_ContinueButtonImageUrl")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual string ContinueButtonImageUrl
		{
			get
			{
				object obj = this.ViewState["ContinueButtonImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ContinueButtonImageUrl"] = value;
			}
		}

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x06003C4A RID: 15434 RVA: 0x000FD8DC File Offset: 0x000FC8DC
		[WebCategory("Styles")]
		[NotifyParentProperty(true)]
		[WebSysDescription("ChangePassword_ContinueButtonStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public Style ContinueButtonStyle
		{
			get
			{
				if (this._continueButtonStyle == null)
				{
					this._continueButtonStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._continueButtonStyle).TrackViewState();
					}
				}
				return this._continueButtonStyle;
			}
		}

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06003C4B RID: 15435 RVA: 0x000FD90C File Offset: 0x000FC90C
		// (set) Token: 0x06003C4C RID: 15436 RVA: 0x000FD93E File Offset: 0x000FC93E
		[WebSysDescription("ChangePassword_ContinueButtonText")]
		[WebSysDefaultValue("ChangePassword_DefaultContinueButtonText")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		public virtual string ContinueButtonText
		{
			get
			{
				object obj = this.ViewState["ContinueButtonText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("ChangePassword_DefaultContinueButtonText");
			}
			set
			{
				this.ViewState["ContinueButtonText"] = value;
			}
		}

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06003C4D RID: 15437 RVA: 0x000FD954 File Offset: 0x000FC954
		// (set) Token: 0x06003C4E RID: 15438 RVA: 0x000FD97D File Offset: 0x000FC97D
		[DefaultValue(ButtonType.Button)]
		[WebCategory("Appearance")]
		[WebSysDescription("ChangePassword_ContinueButtonType")]
		public virtual ButtonType ContinueButtonType
		{
			get
			{
				object obj = this.ViewState["ContinueButtonType"];
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
				this.ViewState["ContinueButtonType"] = value;
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x06003C4F RID: 15439 RVA: 0x000FD9A8 File Offset: 0x000FC9A8
		// (set) Token: 0x06003C50 RID: 15440 RVA: 0x000FD9D5 File Offset: 0x000FC9D5
		[WebCategory("Behavior")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		[WebSysDescription("LoginControls_ContinueDestinationPageUrl")]
		[Themeable(false)]
		[UrlProperty]
		public virtual string ContinueDestinationPageUrl
		{
			get
			{
				object obj = this.ViewState["ContinueDestinationPageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ContinueDestinationPageUrl"] = value;
			}
		}

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06003C51 RID: 15441 RVA: 0x000FD9E8 File Offset: 0x000FC9E8
		private bool ConvertingToTemplate
		{
			get
			{
				return base.DesignMode && this._convertingToTemplate;
			}
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x06003C52 RID: 15442 RVA: 0x000FD9FC File Offset: 0x000FC9FC
		// (set) Token: 0x06003C53 RID: 15443 RVA: 0x000FDA29 File Offset: 0x000FCA29
		[WebCategory("Links")]
		[DefaultValue("")]
		[WebSysDescription("ChangePassword_CreateUserIconUrl")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
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

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06003C54 RID: 15444 RVA: 0x000FDA3C File Offset: 0x000FCA3C
		// (set) Token: 0x06003C55 RID: 15445 RVA: 0x000FDA69 File Offset: 0x000FCA69
		[Localizable(true)]
		[WebCategory("Links")]
		[DefaultValue("")]
		[WebSysDescription("ChangePassword_CreateUserText")]
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

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x06003C56 RID: 15446 RVA: 0x000FDA7C File Offset: 0x000FCA7C
		// (set) Token: 0x06003C57 RID: 15447 RVA: 0x000FDAA9 File Offset: 0x000FCAA9
		[DefaultValue("")]
		[WebCategory("Links")]
		[WebSysDescription("ChangePassword_CreateUserUrl")]
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

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06003C58 RID: 15448 RVA: 0x000FDABC File Offset: 0x000FCABC
		[Browsable(false)]
		[Themeable(false)]
		[Filterable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string CurrentPassword
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

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06003C59 RID: 15449 RVA: 0x000FDAD4 File Offset: 0x000FCAD4
		private string CurrentPasswordInternal
		{
			get
			{
				string currentPassword = this.CurrentPassword;
				if (string.IsNullOrEmpty(currentPassword) && this._changePasswordContainer != null)
				{
					ITextControl textControl = (ITextControl)this._changePasswordContainer.CurrentPasswordTextBox;
					if (textControl != null)
					{
						return textControl.Text;
					}
				}
				return currentPassword;
			}
		}

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06003C5A RID: 15450 RVA: 0x000FDB14 File Offset: 0x000FCB14
		// (set) Token: 0x06003C5B RID: 15451 RVA: 0x000FDB1C File Offset: 0x000FCB1C
		internal ChangePassword.View CurrentView
		{
			get
			{
				return this._currentView;
			}
			set
			{
				if (value < ChangePassword.View.ChangePassword || value > ChangePassword.View.Success)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value != this.CurrentView)
				{
					this._currentView = value;
				}
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06003C5C RID: 15452 RVA: 0x000FDB44 File Offset: 0x000FCB44
		// (set) Token: 0x06003C5D RID: 15453 RVA: 0x000FDB6D File Offset: 0x000FCB6D
		[DefaultValue(false)]
		[WebCategory("Behavior")]
		[WebSysDescription("ChangePassword_DisplayUserName")]
		public virtual bool DisplayUserName
		{
			get
			{
				object obj = this.ViewState["DisplayUserName"];
				return obj != null && (bool)obj;
			}
			set
			{
				if (this.DisplayUserName != value)
				{
					this.ViewState["DisplayUserName"] = value;
					this.UpdateValidators();
				}
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06003C5E RID: 15454 RVA: 0x000FDB94 File Offset: 0x000FCB94
		// (set) Token: 0x06003C5F RID: 15455 RVA: 0x000FDBC1 File Offset: 0x000FCBC1
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		[WebSysDescription("LoginControls_EditProfileIconUrl")]
		[WebCategory("Links")]
		[UrlProperty]
		public virtual string EditProfileIconUrl
		{
			get
			{
				object obj = this.ViewState["EditProfileIconUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["EditProfileIconUrl"] = value;
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06003C60 RID: 15456 RVA: 0x000FDBD4 File Offset: 0x000FCBD4
		// (set) Token: 0x06003C61 RID: 15457 RVA: 0x000FDC01 File Offset: 0x000FCC01
		[Localizable(true)]
		[WebCategory("Links")]
		[DefaultValue("")]
		[WebSysDescription("ChangePassword_EditProfileText")]
		public virtual string EditProfileText
		{
			get
			{
				object obj = this.ViewState["EditProfileText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["EditProfileText"] = value;
			}
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06003C62 RID: 15458 RVA: 0x000FDC14 File Offset: 0x000FCC14
		// (set) Token: 0x06003C63 RID: 15459 RVA: 0x000FDC41 File Offset: 0x000FCC41
		[DefaultValue("")]
		[WebCategory("Links")]
		[WebSysDescription("ChangePassword_EditProfileUrl")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		public virtual string EditProfileUrl
		{
			get
			{
				object obj = this.ViewState["EditProfileUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["EditProfileUrl"] = value;
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06003C64 RID: 15460 RVA: 0x000FDC54 File Offset: 0x000FCC54
		[WebSysDescription("WebControl_FailureTextStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
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

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06003C65 RID: 15461 RVA: 0x000FDC84 File Offset: 0x000FCC84
		// (set) Token: 0x06003C66 RID: 15462 RVA: 0x000FDCB1 File Offset: 0x000FCCB1
		[DefaultValue("")]
		[WebCategory("Links")]
		[WebSysDescription("LoginControls_HelpPageIconUrl")]
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

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06003C67 RID: 15463 RVA: 0x000FDCC4 File Offset: 0x000FCCC4
		// (set) Token: 0x06003C68 RID: 15464 RVA: 0x000FDCF1 File Offset: 0x000FCCF1
		[Localizable(true)]
		[WebCategory("Links")]
		[DefaultValue("")]
		[WebSysDescription("ChangePassword_HelpPageText")]
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

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06003C69 RID: 15465 RVA: 0x000FDD04 File Offset: 0x000FCD04
		// (set) Token: 0x06003C6A RID: 15466 RVA: 0x000FDD31 File Offset: 0x000FCD31
		[WebCategory("Links")]
		[DefaultValue("")]
		[WebSysDescription("LoginControls_HelpPageUrl")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
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

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06003C6B RID: 15467 RVA: 0x000FDD44 File Offset: 0x000FCD44
		[WebSysDescription("WebControl_HyperLinkStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
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

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06003C6C RID: 15468 RVA: 0x000FDD74 File Offset: 0x000FCD74
		// (set) Token: 0x06003C6D RID: 15469 RVA: 0x000FDDA1 File Offset: 0x000FCDA1
		[DefaultValue("")]
		[WebCategory("Appearance")]
		[Localizable(true)]
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

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06003C6E RID: 15470 RVA: 0x000FDDB4 File Offset: 0x000FCDB4
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

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06003C6F RID: 15471 RVA: 0x000FDDE2 File Offset: 0x000FCDE2
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("LoginControls_LabelStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebCategory("Styles")]
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

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x06003C70 RID: 15472 RVA: 0x000FDE10 File Offset: 0x000FCE10
		// (set) Token: 0x06003C71 RID: 15473 RVA: 0x000FDE3D File Offset: 0x000FCE3D
		[Themeable(false)]
		[WebCategory("Data")]
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

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x06003C72 RID: 15474 RVA: 0x000FDE50 File Offset: 0x000FCE50
		[Filterable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Themeable(false)]
		[Browsable(false)]
		public virtual string NewPassword
		{
			get
			{
				if (this._newPassword != null)
				{
					return this._newPassword;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x06003C73 RID: 15475 RVA: 0x000FDE68 File Offset: 0x000FCE68
		private string NewPasswordInternal
		{
			get
			{
				string newPassword = this.NewPassword;
				if (string.IsNullOrEmpty(newPassword) && this._changePasswordContainer != null)
				{
					ITextControl textControl = (ITextControl)this._changePasswordContainer.NewPasswordTextBox;
					if (textControl != null)
					{
						return textControl.Text;
					}
				}
				return newPassword;
			}
		}

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06003C74 RID: 15476 RVA: 0x000FDEA8 File Offset: 0x000FCEA8
		// (set) Token: 0x06003C75 RID: 15477 RVA: 0x000FDEDA File Offset: 0x000FCEDA
		[WebSysDefaultValue("Password_InvalidPasswordErrorMessage")]
		[WebSysDescription("ChangePassword_NewPasswordRegularExpressionErrorMessage")]
		[WebCategory("Validation")]
		public virtual string NewPasswordRegularExpressionErrorMessage
		{
			get
			{
				object obj = this.ViewState["NewPasswordRegularExpressionErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("Password_InvalidPasswordErrorMessage");
			}
			set
			{
				this.ViewState["NewPasswordRegularExpressionErrorMessage"] = value;
			}
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06003C76 RID: 15478 RVA: 0x000FDEF0 File Offset: 0x000FCEF0
		// (set) Token: 0x06003C77 RID: 15479 RVA: 0x000FDF22 File Offset: 0x000FCF22
		[WebCategory("Appearance")]
		[WebSysDefaultValue("ChangePassword_DefaultNewPasswordLabelText")]
		[WebSysDescription("ChangePassword_NewPasswordLabelText")]
		[Localizable(true)]
		public virtual string NewPasswordLabelText
		{
			get
			{
				object obj = this.ViewState["NewPasswordLabelText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("ChangePassword_DefaultNewPasswordLabelText");
			}
			set
			{
				this.ViewState["NewPasswordLabelText"] = value;
			}
		}

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06003C78 RID: 15480 RVA: 0x000FDF38 File Offset: 0x000FCF38
		// (set) Token: 0x06003C79 RID: 15481 RVA: 0x000FDF65 File Offset: 0x000FCF65
		[WebSysDefaultValue("")]
		[WebCategory("Validation")]
		[WebSysDescription("ChangePassword_NewPasswordRegularExpression")]
		public virtual string NewPasswordRegularExpression
		{
			get
			{
				object obj = this.ViewState["NewPasswordRegularExpression"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (this.NewPasswordRegularExpression != value)
				{
					this.ViewState["NewPasswordRegularExpression"] = value;
					this.UpdateValidators();
				}
			}
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06003C7A RID: 15482 RVA: 0x000FDF8C File Offset: 0x000FCF8C
		// (set) Token: 0x06003C7B RID: 15483 RVA: 0x000FDFBE File Offset: 0x000FCFBE
		[WebCategory("Validation")]
		[WebSysDefaultValue("ChangePassword_DefaultNewPasswordRequiredErrorMessage")]
		[WebSysDescription("ChangePassword_NewPasswordRequiredErrorMessage")]
		[Localizable(true)]
		public virtual string NewPasswordRequiredErrorMessage
		{
			get
			{
				object obj = this.ViewState["NewPasswordRequiredErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("ChangePassword_DefaultNewPasswordRequiredErrorMessage");
			}
			set
			{
				this.ViewState["NewPasswordRequiredErrorMessage"] = value;
			}
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06003C7C RID: 15484 RVA: 0x000FDFD1 File Offset: 0x000FCFD1
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("ChangePassword_PasswordHintStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebCategory("Styles")]
		public TableItemStyle PasswordHintStyle
		{
			get
			{
				if (this._passwordHintStyle == null)
				{
					this._passwordHintStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._passwordHintStyle).TrackViewState();
					}
				}
				return this._passwordHintStyle;
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06003C7D RID: 15485 RVA: 0x000FE000 File Offset: 0x000FD000
		// (set) Token: 0x06003C7E RID: 15486 RVA: 0x000FE02D File Offset: 0x000FD02D
		[WebSysDescription("ChangePassword_PasswordHintText")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		public virtual string PasswordHintText
		{
			get
			{
				object obj = this.ViewState["PasswordHintText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["PasswordHintText"] = value;
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06003C7F RID: 15487 RVA: 0x000FE040 File Offset: 0x000FD040
		// (set) Token: 0x06003C80 RID: 15488 RVA: 0x000FE072 File Offset: 0x000FD072
		[WebSysDefaultValue("LoginControls_DefaultPasswordLabelText")]
		[WebSysDescription("LoginControls_PasswordLabelText")]
		[Localizable(true)]
		[WebCategory("Appearance")]
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

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06003C81 RID: 15489 RVA: 0x000FE088 File Offset: 0x000FD088
		// (set) Token: 0x06003C82 RID: 15490 RVA: 0x000FE0B5 File Offset: 0x000FD0B5
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		[WebCategory("Links")]
		[DefaultValue("")]
		[WebSysDescription("ChangePassword_PasswordRecoveryIconUrl")]
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

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06003C83 RID: 15491 RVA: 0x000FE0C8 File Offset: 0x000FD0C8
		// (set) Token: 0x06003C84 RID: 15492 RVA: 0x000FE0F5 File Offset: 0x000FD0F5
		[Localizable(true)]
		[DefaultValue("")]
		[WebSysDescription("ChangePassword_PasswordRecoveryText")]
		[WebCategory("Links")]
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

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x06003C85 RID: 15493 RVA: 0x000FE108 File Offset: 0x000FD108
		// (set) Token: 0x06003C86 RID: 15494 RVA: 0x000FE135 File Offset: 0x000FD135
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		[WebCategory("Links")]
		[DefaultValue("")]
		[WebSysDescription("ChangePassword_PasswordRecoveryUrl")]
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

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x06003C87 RID: 15495 RVA: 0x000FE148 File Offset: 0x000FD148
		// (set) Token: 0x06003C88 RID: 15496 RVA: 0x000FE17A File Offset: 0x000FD17A
		[WebSysDefaultValue("ChangePassword_DefaultPasswordRequiredErrorMessage")]
		[WebSysDescription("ChangePassword_PasswordRequiredErrorMessage")]
		[WebCategory("Validation")]
		[Localizable(true)]
		public virtual string PasswordRequiredErrorMessage
		{
			get
			{
				object obj = this.ViewState["PasswordRequiredErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("ChangePassword_DefaultPasswordRequiredErrorMessage");
			}
			set
			{
				this.ViewState["PasswordRequiredErrorMessage"] = value;
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x06003C89 RID: 15497 RVA: 0x000FE18D File Offset: 0x000FD18D
		private bool RegExpEnabled
		{
			get
			{
				return this.NewPasswordRegularExpression.Length > 0;
			}
		}

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06003C8A RID: 15498 RVA: 0x000FE19D File Offset: 0x000FD19D
		[Themeable(false)]
		[WebSysDescription("ChangePassword_MailDefinition")]
		[WebCategory("Behavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public MailDefinition MailDefinition
		{
			get
			{
				if (this._mailDefinition == null)
				{
					this._mailDefinition = new MailDefinition();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._mailDefinition).TrackViewState();
					}
				}
				return this._mailDefinition;
			}
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06003C8B RID: 15499 RVA: 0x000FE1CC File Offset: 0x000FD1CC
		// (set) Token: 0x06003C8C RID: 15500 RVA: 0x000FE1F9 File Offset: 0x000FD1F9
		[WebCategory("Behavior")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Themeable(false)]
		[DefaultValue("")]
		[WebSysDescription("LoginControls_SuccessPageUrl")]
		[UrlProperty]
		public virtual string SuccessPageUrl
		{
			get
			{
				object obj = this.ViewState["SuccessPageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["SuccessPageUrl"] = value;
			}
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06003C8D RID: 15501 RVA: 0x000FE20C File Offset: 0x000FD20C
		// (set) Token: 0x06003C8E RID: 15502 RVA: 0x000FE214 File Offset: 0x000FD214
		[TemplateContainer(typeof(ChangePassword))]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual ITemplate SuccessTemplate
		{
			get
			{
				return this._successTemplate;
			}
			set
			{
				this._successTemplate = value;
				base.ChildControlsCreated = false;
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06003C8F RID: 15503 RVA: 0x000FE224 File Offset: 0x000FD224
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Control SuccessTemplateContainer
		{
			get
			{
				this.EnsureChildControls();
				return this._successContainer;
			}
		}

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x06003C90 RID: 15504 RVA: 0x000FE234 File Offset: 0x000FD234
		// (set) Token: 0x06003C91 RID: 15505 RVA: 0x000FE266 File Offset: 0x000FD266
		[WebCategory("Appearance")]
		[WebSysDescription("ChangePassword_SuccessText")]
		[WebSysDefaultValue("ChangePassword_DefaultSuccessText")]
		[Localizable(true)]
		public virtual string SuccessText
		{
			get
			{
				object obj = this.ViewState["SuccessText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("ChangePassword_DefaultSuccessText");
			}
			set
			{
				this.ViewState["SuccessText"] = value;
			}
		}

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06003C92 RID: 15506 RVA: 0x000FE279 File Offset: 0x000FD279
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DefaultValue(null)]
		[WebSysDescription("ChangePassword_SuccessTextStyle")]
		[WebCategory("Styles")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		public TableItemStyle SuccessTextStyle
		{
			get
			{
				if (this._successTextStyle == null)
				{
					this._successTextStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._successTextStyle).TrackViewState();
					}
				}
				return this._successTextStyle;
			}
		}

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06003C93 RID: 15507 RVA: 0x000FE2A8 File Offset: 0x000FD2A8
		// (set) Token: 0x06003C94 RID: 15508 RVA: 0x000FE2DA File Offset: 0x000FD2DA
		[WebSysDescription("ChangePassword_SuccessTitleText")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("ChangePassword_DefaultSuccessTitleText")]
		public virtual string SuccessTitleText
		{
			get
			{
				object obj = this.ViewState["SuccessTitleText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("ChangePassword_DefaultSuccessTitleText");
			}
			set
			{
				this.ViewState["SuccessTitleText"] = value;
			}
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x06003C95 RID: 15509 RVA: 0x000FE2ED File Offset: 0x000FD2ED
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Table;
			}
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06003C96 RID: 15510 RVA: 0x000FE2F1 File Offset: 0x000FD2F1
		[WebCategory("Styles")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("LoginControls_TextBoxStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
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

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06003C97 RID: 15511 RVA: 0x000FE31F File Offset: 0x000FD31F
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[NotifyParentProperty(true)]
		[WebCategory("Styles")]
		[WebSysDescription("LoginControls_TitleTextStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
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

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x06003C98 RID: 15512 RVA: 0x000FE34D File Offset: 0x000FD34D
		// (set) Token: 0x06003C99 RID: 15513 RVA: 0x000FE363 File Offset: 0x000FD363
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[WebSysDescription("UserName_InitialValue")]
		public virtual string UserName
		{
			get
			{
				if (this._userName != null)
				{
					return this._userName;
				}
				return string.Empty;
			}
			set
			{
				this._userName = value;
			}
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x06003C9A RID: 15514 RVA: 0x000FE36C File Offset: 0x000FD36C
		private string UserNameInternal
		{
			get
			{
				string userName = this.UserName;
				if (string.IsNullOrEmpty(userName) && this._changePasswordContainer != null && this.DisplayUserName)
				{
					ITextControl textControl = (ITextControl)this._changePasswordContainer.UserNameTextBox;
					if (textControl != null)
					{
						return textControl.Text;
					}
				}
				return userName;
			}
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x06003C9B RID: 15515 RVA: 0x000FE3B4 File Offset: 0x000FD3B4
		// (set) Token: 0x06003C9C RID: 15516 RVA: 0x000FE3E6 File Offset: 0x000FD3E6
		[WebCategory("Appearance")]
		[Localizable(true)]
		[WebSysDescription("LoginControls_UserNameLabelText")]
		[WebSysDefaultValue("ChangePassword_DefaultUserNameLabelText")]
		public virtual string UserNameLabelText
		{
			get
			{
				object obj = this.ViewState["UserNameLabelText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("ChangePassword_DefaultUserNameLabelText");
			}
			set
			{
				this.ViewState["UserNameLabelText"] = value;
			}
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x06003C9D RID: 15517 RVA: 0x000FE3FC File Offset: 0x000FD3FC
		// (set) Token: 0x06003C9E RID: 15518 RVA: 0x000FE42E File Offset: 0x000FD42E
		[WebCategory("Validation")]
		[WebSysDefaultValue("ChangePassword_DefaultUserNameRequiredErrorMessage")]
		[WebSysDescription("ChangePassword_UserNameRequiredErrorMessage")]
		[Localizable(true)]
		public virtual string UserNameRequiredErrorMessage
		{
			get
			{
				object obj = this.ViewState["UserNameRequiredErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("ChangePassword_DefaultUserNameRequiredErrorMessage");
			}
			set
			{
				this.ViewState["UserNameRequiredErrorMessage"] = value;
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x06003C9F RID: 15519 RVA: 0x000FE441 File Offset: 0x000FD441
		// (set) Token: 0x06003CA0 RID: 15520 RVA: 0x000FE449 File Offset: 0x000FD449
		internal Control ValidatorRow
		{
			get
			{
				return this._validatorRow;
			}
			set
			{
				this._validatorRow = value;
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06003CA1 RID: 15521 RVA: 0x000FE452 File Offset: 0x000FD452
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("ChangePassword_ValidatorTextStyle")]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
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

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06003CA2 RID: 15522 RVA: 0x000FE480 File Offset: 0x000FD480
		// (remove) Token: 0x06003CA3 RID: 15523 RVA: 0x000FE493 File Offset: 0x000FD493
		[WebSysDescription("ChangePassword_CancelButtonClick")]
		[WebCategory("Action")]
		public event EventHandler CancelButtonClick
		{
			add
			{
				base.Events.AddHandler(ChangePassword.EventCancelButtonClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(ChangePassword.EventCancelButtonClick, value);
			}
		}

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06003CA4 RID: 15524 RVA: 0x000FE4A6 File Offset: 0x000FD4A6
		// (remove) Token: 0x06003CA5 RID: 15525 RVA: 0x000FE4B9 File Offset: 0x000FD4B9
		[WebCategory("Action")]
		[WebSysDescription("ChangePassword_ChangedPassword")]
		public event EventHandler ChangedPassword
		{
			add
			{
				base.Events.AddHandler(ChangePassword.EventChangedPassword, value);
			}
			remove
			{
				base.Events.RemoveHandler(ChangePassword.EventChangedPassword, value);
			}
		}

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x06003CA6 RID: 15526 RVA: 0x000FE4CC File Offset: 0x000FD4CC
		// (remove) Token: 0x06003CA7 RID: 15527 RVA: 0x000FE4DF File Offset: 0x000FD4DF
		[WebCategory("Action")]
		[WebSysDescription("ChangePassword_ChangePasswordError")]
		public event EventHandler ChangePasswordError
		{
			add
			{
				base.Events.AddHandler(ChangePassword.EventChangePasswordError, value);
			}
			remove
			{
				base.Events.RemoveHandler(ChangePassword.EventChangePasswordError, value);
			}
		}

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06003CA8 RID: 15528 RVA: 0x000FE4F2 File Offset: 0x000FD4F2
		// (remove) Token: 0x06003CA9 RID: 15529 RVA: 0x000FE505 File Offset: 0x000FD505
		[WebSysDescription("ChangePassword_ChangingPassword")]
		[WebCategory("Action")]
		public event LoginCancelEventHandler ChangingPassword
		{
			add
			{
				base.Events.AddHandler(ChangePassword.EventChangingPassword, value);
			}
			remove
			{
				base.Events.RemoveHandler(ChangePassword.EventChangingPassword, value);
			}
		}

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06003CAA RID: 15530 RVA: 0x000FE518 File Offset: 0x000FD518
		// (remove) Token: 0x06003CAB RID: 15531 RVA: 0x000FE52B File Offset: 0x000FD52B
		[WebCategory("Action")]
		[WebSysDescription("ChangePassword_ContinueButtonClick")]
		public event EventHandler ContinueButtonClick
		{
			add
			{
				base.Events.AddHandler(ChangePassword.EventContinueButtonClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(ChangePassword.EventContinueButtonClick, value);
			}
		}

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06003CAC RID: 15532 RVA: 0x000FE53E File Offset: 0x000FD53E
		// (remove) Token: 0x06003CAD RID: 15533 RVA: 0x000FE551 File Offset: 0x000FD551
		[WebCategory("Action")]
		[WebSysDescription("ChangePassword_SendingMail")]
		public event MailMessageEventHandler SendingMail
		{
			add
			{
				base.Events.AddHandler(ChangePassword.EventSendingMail, value);
			}
			remove
			{
				base.Events.RemoveHandler(ChangePassword.EventSendingMail, value);
			}
		}

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06003CAE RID: 15534 RVA: 0x000FE564 File Offset: 0x000FD564
		// (remove) Token: 0x06003CAF RID: 15535 RVA: 0x000FE577 File Offset: 0x000FD577
		[WebCategory("Action")]
		[WebSysDescription("ChangePassword_SendMailError")]
		public event SendMailErrorEventHandler SendMailError
		{
			add
			{
				base.Events.AddHandler(ChangePassword.EventSendMailError, value);
			}
			remove
			{
				base.Events.RemoveHandler(ChangePassword.EventSendMailError, value);
			}
		}

		// Token: 0x06003CB0 RID: 15536 RVA: 0x000FE58C File Offset: 0x000FD58C
		private void AttemptChangePassword()
		{
			if (this.Page != null && !this.Page.IsValid)
			{
				return;
			}
			LoginCancelEventArgs loginCancelEventArgs = new LoginCancelEventArgs();
			this.OnChangingPassword(loginCancelEventArgs);
			if (loginCancelEventArgs.Cancel)
			{
				return;
			}
			MembershipProvider provider = LoginUtil.GetProvider(this.MembershipProvider);
			MembershipUser user = provider.GetUser(this.UserNameInternal, false, false);
			string newPasswordInternal = this.NewPasswordInternal;
			if (user != null && user.ChangePassword(this.CurrentPasswordInternal, newPasswordInternal, false))
			{
				if (user.IsApproved && !user.IsLockedOut)
				{
					FormsAuthentication.SetAuthCookie(this.UserNameInternal, false);
				}
				this.OnChangedPassword(EventArgs.Empty);
				this.PerformSuccessAction(user.Email, user.UserName, newPasswordInternal);
				return;
			}
			this.OnChangePasswordError(EventArgs.Empty);
			string text = this.ChangePasswordFailureText;
			if (!string.IsNullOrEmpty(text))
			{
				text = string.Format(CultureInfo.CurrentCulture, text, new object[] { provider.MinRequiredPasswordLength, provider.MinRequiredNonAlphanumericCharacters });
			}
			this.SetFailureTextLabel(this._changePasswordContainer, text);
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x000FE695 File Offset: 0x000FD695
		private void ConfirmNewPasswordTextChanged(object source, EventArgs e)
		{
			this._confirmNewPassword = ((ITextControl)source).Text;
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x000FE6A8 File Offset: 0x000FD6A8
		private void CreateChangePasswordViewControls()
		{
			this._changePasswordContainer = new ChangePassword.ChangePasswordContainer(this);
			this._changePasswordContainer.ID = "ChangePasswordContainerID";
			this._changePasswordContainer.RenderDesignerRegion = this._renderDesignerRegion;
			ITemplate template = this.ChangePasswordTemplate;
			bool flag = template == null;
			if (flag)
			{
				this._changePasswordContainer.EnableViewState = false;
				this._changePasswordContainer.EnableTheming = false;
				template = new ChangePassword.DefaultChangePasswordTemplate(this);
			}
			template.InstantiateIn(this._changePasswordContainer);
			this.Controls.Add(this._changePasswordContainer);
			IEditableTextControl editableTextControl = this._changePasswordContainer.UserNameTextBox as IEditableTextControl;
			if (editableTextControl != null)
			{
				editableTextControl.TextChanged += this.UserNameTextChanged;
			}
			IEditableTextControl editableTextControl2 = this._changePasswordContainer.CurrentPasswordTextBox as IEditableTextControl;
			if (editableTextControl2 != null)
			{
				editableTextControl2.TextChanged += this.PasswordTextChanged;
			}
			IEditableTextControl editableTextControl3 = this._changePasswordContainer.NewPasswordTextBox as IEditableTextControl;
			if (editableTextControl3 != null)
			{
				editableTextControl3.TextChanged += this.NewPasswordTextChanged;
			}
			IEditableTextControl editableTextControl4 = this._changePasswordContainer.ConfirmNewPasswordTextBox as IEditableTextControl;
			if (editableTextControl4 != null)
			{
				editableTextControl4.TextChanged += this.ConfirmNewPasswordTextChanged;
			}
			this.SetEditableChildProperties();
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x000FE7D1 File Offset: 0x000FD7D1
		protected internal override void CreateChildControls()
		{
			this.Controls.Clear();
			this.CreateChangePasswordViewControls();
			this.CreateSuccessViewControls();
			this.UpdateValidators();
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x000FE7F0 File Offset: 0x000FD7F0
		private void CreateSuccessViewControls()
		{
			this._successContainer = new ChangePassword.SuccessContainer(this);
			this._successContainer.ID = "SuccessContainerID";
			this._successContainer.RenderDesignerRegion = this._renderDesignerRegion;
			ITemplate template;
			if (this.SuccessTemplate != null)
			{
				template = this.SuccessTemplate;
			}
			else
			{
				template = new ChangePassword.DefaultSuccessTemplate(this);
				this._successContainer.EnableViewState = false;
				this._successContainer.EnableTheming = false;
			}
			template.InstantiateIn(this._successContainer);
			this.Controls.Add(this._successContainer);
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x000FE87C File Offset: 0x000FD87C
		protected internal override void LoadControlState(object savedState)
		{
			if (savedState != null)
			{
				Triplet triplet = (Triplet)savedState;
				if (triplet.First != null)
				{
					base.LoadControlState(triplet.First);
				}
				if (triplet.Second != null)
				{
					this._currentView = (ChangePassword.View)((int)triplet.Second);
				}
				if (triplet.Third != null)
				{
					this._userName = (string)triplet.Third;
				}
			}
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x000FE8DC File Offset: 0x000FD8DC
		protected override void LoadViewState(object savedState)
		{
			if (savedState == null)
			{
				base.LoadViewState(null);
			}
			else
			{
				object[] array = (object[])savedState;
				if (array.Length != 14)
				{
					throw new ArgumentException(SR.GetString("ViewState_InvalidViewState"));
				}
				base.LoadViewState(array[0]);
				if (array[1] != null)
				{
					((IStateManager)this.ChangePasswordButtonStyle).LoadViewState(array[1]);
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
					((IStateManager)this.PasswordHintStyle).LoadViewState(array[7]);
				}
				if (array[8] != null)
				{
					((IStateManager)this.FailureTextStyle).LoadViewState(array[8]);
				}
				if (array[9] != null)
				{
					((IStateManager)this.MailDefinition).LoadViewState(array[9]);
				}
				if (array[10] != null)
				{
					((IStateManager)this.CancelButtonStyle).LoadViewState(array[10]);
				}
				if (array[11] != null)
				{
					((IStateManager)this.ContinueButtonStyle).LoadViewState(array[11]);
				}
				if (array[12] != null)
				{
					((IStateManager)this.SuccessTextStyle).LoadViewState(array[12]);
				}
				if (array[13] != null)
				{
					((IStateManager)this.ValidatorTextStyle).LoadViewState(array[13]);
				}
			}
			this.UpdateValidators();
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x000FEA26 File Offset: 0x000FDA26
		private void NewPasswordTextChanged(object source, EventArgs e)
		{
			this._newPassword = ((ITextControl)source).Text;
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x000FEA3C File Offset: 0x000FDA3C
		protected override bool OnBubbleEvent(object source, EventArgs e)
		{
			bool flag = false;
			if (e is CommandEventArgs)
			{
				CommandEventArgs commandEventArgs = (CommandEventArgs)e;
				if (commandEventArgs.CommandName.Equals(ChangePassword.ChangePasswordButtonCommandName, StringComparison.CurrentCultureIgnoreCase))
				{
					this.AttemptChangePassword();
					flag = true;
				}
				else if (commandEventArgs.CommandName.Equals(ChangePassword.CancelButtonCommandName, StringComparison.CurrentCultureIgnoreCase))
				{
					this.OnCancelButtonClick(commandEventArgs);
					flag = true;
				}
				else if (commandEventArgs.CommandName.Equals(ChangePassword.ContinueButtonCommandName, StringComparison.CurrentCultureIgnoreCase))
				{
					this.OnContinueButtonClick(commandEventArgs);
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x000FEAB4 File Offset: 0x000FDAB4
		protected virtual void OnCancelButtonClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ChangePassword.EventCancelButtonClick];
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

		// Token: 0x06003CBA RID: 15546 RVA: 0x000FEB0C File Offset: 0x000FDB0C
		protected virtual void OnChangedPassword(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ChangePassword.EventChangedPassword];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x000FEB3C File Offset: 0x000FDB3C
		protected virtual void OnChangePasswordError(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ChangePassword.EventChangePasswordError];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x000FEB6C File Offset: 0x000FDB6C
		protected virtual void OnChangingPassword(LoginCancelEventArgs e)
		{
			LoginCancelEventHandler loginCancelEventHandler = (LoginCancelEventHandler)base.Events[ChangePassword.EventChangingPassword];
			if (loginCancelEventHandler != null)
			{
				loginCancelEventHandler(this, e);
			}
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x000FEB9C File Offset: 0x000FDB9C
		protected virtual void OnContinueButtonClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ChangePassword.EventContinueButtonClick];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			string continueDestinationPageUrl = this.ContinueDestinationPageUrl;
			if (!string.IsNullOrEmpty(continueDestinationPageUrl))
			{
				this.Page.Response.Redirect(base.ResolveClientUrl(continueDestinationPageUrl), false);
			}
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x000FEBF4 File Offset: 0x000FDBF4
		protected internal override void OnInit(EventArgs e)
		{
			if (!base.DesignMode)
			{
				string userName = LoginUtil.GetUserName(this);
				if (!string.IsNullOrEmpty(userName))
				{
					this.UserName = userName;
				}
			}
			base.OnInit(e);
			this.Page.RegisterRequiresControlState(this);
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x000FEC34 File Offset: 0x000FDC34
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			ChangePassword.View currentView = this.CurrentView;
			if (currentView != ChangePassword.View.ChangePassword)
			{
				return;
			}
			this.SetEditableChildProperties();
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x000FEC5C File Offset: 0x000FDC5C
		protected virtual void OnSendingMail(MailMessageEventArgs e)
		{
			MailMessageEventHandler mailMessageEventHandler = (MailMessageEventHandler)base.Events[ChangePassword.EventSendingMail];
			if (mailMessageEventHandler != null)
			{
				mailMessageEventHandler(this, e);
			}
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x000FEC8C File Offset: 0x000FDC8C
		protected virtual void OnSendMailError(SendMailErrorEventArgs e)
		{
			SendMailErrorEventHandler sendMailErrorEventHandler = (SendMailErrorEventHandler)base.Events[ChangePassword.EventSendMailError];
			if (sendMailErrorEventHandler != null)
			{
				sendMailErrorEventHandler(this, e);
			}
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x000FECBA File Offset: 0x000FDCBA
		private void PasswordTextChanged(object source, EventArgs e)
		{
			this._password = ((ITextControl)source).Text;
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x000FECD0 File Offset: 0x000FDCD0
		private void PerformSuccessAction(string email, string userName, string newPassword)
		{
			if (this._mailDefinition != null && !string.IsNullOrEmpty(email))
			{
				LoginUtil.SendPasswordMail(email, userName, newPassword, this.MailDefinition, null, null, new LoginUtil.OnSendingMailDelegate(this.OnSendingMail), new LoginUtil.OnSendMailErrorDelegate(this.OnSendMailError), this);
			}
			string successPageUrl = this.SuccessPageUrl;
			if (!string.IsNullOrEmpty(successPageUrl))
			{
				this.Page.Response.Redirect(base.ResolveClientUrl(successPageUrl), false);
				return;
			}
			this.CurrentView = ChangePassword.View.Success;
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x000FED47 File Offset: 0x000FDD47
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			if (base.DesignMode)
			{
				base.ChildControlsCreated = false;
			}
			this.EnsureChildControls();
			this.SetChildProperties();
			this.RenderContents(writer);
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x000FED80 File Offset: 0x000FDD80
		protected internal override object SaveControlState()
		{
			object obj = base.SaveControlState();
			object obj2 = null;
			object obj3 = (int)this._currentView;
			if (this._userName != null && this._currentView != ChangePassword.View.Success)
			{
				obj2 = this._userName;
			}
			return new Triplet(obj, obj3, obj2);
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x000FEDC4 File Offset: 0x000FDDC4
		protected override object SaveViewState()
		{
			object[] array = new object[]
			{
				base.SaveViewState(),
				(this._changePasswordButtonStyle != null) ? ((IStateManager)this._changePasswordButtonStyle).SaveViewState() : null,
				(this._labelStyle != null) ? ((IStateManager)this._labelStyle).SaveViewState() : null,
				(this._textBoxStyle != null) ? ((IStateManager)this._textBoxStyle).SaveViewState() : null,
				(this._hyperLinkStyle != null) ? ((IStateManager)this._hyperLinkStyle).SaveViewState() : null,
				(this._instructionTextStyle != null) ? ((IStateManager)this._instructionTextStyle).SaveViewState() : null,
				(this._titleTextStyle != null) ? ((IStateManager)this._titleTextStyle).SaveViewState() : null,
				(this._passwordHintStyle != null) ? ((IStateManager)this._passwordHintStyle).SaveViewState() : null,
				(this._failureTextStyle != null) ? ((IStateManager)this._failureTextStyle).SaveViewState() : null,
				(this._mailDefinition != null) ? ((IStateManager)this._mailDefinition).SaveViewState() : null,
				(this._cancelButtonStyle != null) ? ((IStateManager)this._cancelButtonStyle).SaveViewState() : null,
				(this._continueButtonStyle != null) ? ((IStateManager)this._continueButtonStyle).SaveViewState() : null,
				(this._successTextStyle != null) ? ((IStateManager)this._successTextStyle).SaveViewState() : null,
				(this._validatorTextStyle != null) ? ((IStateManager)this._validatorTextStyle).SaveViewState() : null
			};
			for (int i = 0; i < 14; i++)
			{
				if (array[i] != null)
				{
					return array;
				}
			}
			return null;
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x000FEF44 File Offset: 0x000FDF44
		private void SetFailureTextLabel(ChangePassword.ChangePasswordContainer container, string failureText)
		{
			ITextControl textControl = (ITextControl)container.FailureTextLabel;
			if (textControl != null)
			{
				textControl.Text = failureText;
			}
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x000FEF68 File Offset: 0x000FDF68
		internal void SetChildProperties()
		{
			switch (this.CurrentView)
			{
			case ChangePassword.View.ChangePassword:
				this.SetCommonChangePasswordViewProperties();
				if (this.ChangePasswordTemplate == null)
				{
					this.SetDefaultChangePasswordViewProperties();
					return;
				}
				break;
			case ChangePassword.View.Success:
				this.SetCommonSuccessViewProperties();
				if (this.SuccessTemplate == null)
				{
					this.SetDefaultSuccessViewProperties();
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06003CC9 RID: 15561 RVA: 0x000FEFB4 File Offset: 0x000FDFB4
		private void SetCommonChangePasswordViewProperties()
		{
			Util.CopyBaseAttributesToInnerControl(this, this._changePasswordContainer);
			this._changePasswordContainer.ApplyStyle(base.ControlStyle);
			this._successContainer.Visible = false;
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x000FEFDF File Offset: 0x000FDFDF
		private void SetCommonSuccessViewProperties()
		{
			Util.CopyBaseAttributesToInnerControl(this, this._successContainer);
			this._successContainer.ApplyStyle(base.ControlStyle);
			this._changePasswordContainer.Visible = false;
		}

		// Token: 0x06003CCB RID: 15563 RVA: 0x000FF00C File Offset: 0x000FE00C
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		protected override void SetDesignModeState(IDictionary data)
		{
			if (data != null)
			{
				object obj = data["CurrentView"];
				if (obj != null)
				{
					this.CurrentView = (ChangePassword.View)obj;
				}
				obj = data["ConvertToTemplate"];
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

		// Token: 0x06003CCC RID: 15564 RVA: 0x000FF070 File Offset: 0x000FE070
		private void SetDefaultChangePasswordViewProperties()
		{
			ChangePassword.ChangePasswordContainer changePasswordContainer = this._changePasswordContainer;
			changePasswordContainer.BorderTable.CellPadding = this.BorderPadding;
			changePasswordContainer.BorderTable.CellSpacing = 0;
			LoginUtil.ApplyStyleToLiteral(changePasswordContainer.Title, this.ChangePasswordTitleText, this.TitleTextStyle, true);
			LoginUtil.ApplyStyleToLiteral(changePasswordContainer.Instruction, this.InstructionText, this.InstructionTextStyle, true);
			LoginUtil.ApplyStyleToLiteral(changePasswordContainer.UserNameLabel, this.UserNameLabelText, this.LabelStyle, false);
			LoginUtil.ApplyStyleToLiteral(changePasswordContainer.CurrentPasswordLabel, this.PasswordLabelText, this.LabelStyle, false);
			LoginUtil.ApplyStyleToLiteral(changePasswordContainer.NewPasswordLabel, this.NewPasswordLabelText, this.LabelStyle, false);
			LoginUtil.ApplyStyleToLiteral(changePasswordContainer.ConfirmNewPasswordLabel, this.ConfirmNewPasswordLabelText, this.LabelStyle, false);
			LoginUtil.ApplyStyleToLiteral(changePasswordContainer.PasswordHintLabel, this.PasswordHintText, this.PasswordHintStyle, false);
			if (this._textBoxStyle != null)
			{
				if (this.DisplayUserName)
				{
					((WebControl)changePasswordContainer.UserNameTextBox).ApplyStyle(this.TextBoxStyle);
				}
				((WebControl)changePasswordContainer.CurrentPasswordTextBox).ApplyStyle(this.TextBoxStyle);
				((WebControl)changePasswordContainer.NewPasswordTextBox).ApplyStyle(this.TextBoxStyle);
				((WebControl)changePasswordContainer.ConfirmNewPasswordTextBox).ApplyStyle(this.TextBoxStyle);
			}
			this._passwordHintTableRow.Visible = !string.IsNullOrEmpty(this.PasswordHintText);
			this._userNameTableRow.Visible = this.DisplayUserName;
			if (this.DisplayUserName)
			{
				((WebControl)changePasswordContainer.UserNameTextBox).TabIndex = this.TabIndex;
				((WebControl)changePasswordContainer.UserNameTextBox).AccessKey = this.AccessKey;
			}
			else
			{
				((WebControl)changePasswordContainer.CurrentPasswordTextBox).AccessKey = this.AccessKey;
			}
			((WebControl)changePasswordContainer.CurrentPasswordTextBox).TabIndex = this.TabIndex;
			((WebControl)changePasswordContainer.NewPasswordTextBox).TabIndex = this.TabIndex;
			((WebControl)changePasswordContainer.ConfirmNewPasswordTextBox).TabIndex = this.TabIndex;
			bool flag = true;
			this.ValidatorRow.Visible = flag;
			RequiredFieldValidator userNameRequired = changePasswordContainer.UserNameRequired;
			userNameRequired.ErrorMessage = this.UserNameRequiredErrorMessage;
			userNameRequired.ToolTip = this.UserNameRequiredErrorMessage;
			userNameRequired.Enabled = flag;
			userNameRequired.Visible = flag;
			if (this._validatorTextStyle != null)
			{
				userNameRequired.ApplyStyle(this._validatorTextStyle);
			}
			RequiredFieldValidator passwordRequired = changePasswordContainer.PasswordRequired;
			passwordRequired.ErrorMessage = this.PasswordRequiredErrorMessage;
			passwordRequired.ToolTip = this.PasswordRequiredErrorMessage;
			passwordRequired.Enabled = flag;
			passwordRequired.Visible = flag;
			RequiredFieldValidator newPasswordRequired = changePasswordContainer.NewPasswordRequired;
			newPasswordRequired.ErrorMessage = this.NewPasswordRequiredErrorMessage;
			newPasswordRequired.ToolTip = this.NewPasswordRequiredErrorMessage;
			newPasswordRequired.Enabled = flag;
			newPasswordRequired.Visible = flag;
			RequiredFieldValidator confirmNewPasswordRequired = changePasswordContainer.ConfirmNewPasswordRequired;
			confirmNewPasswordRequired.ErrorMessage = this.ConfirmPasswordRequiredErrorMessage;
			confirmNewPasswordRequired.ToolTip = this.ConfirmPasswordRequiredErrorMessage;
			confirmNewPasswordRequired.Enabled = flag;
			confirmNewPasswordRequired.Visible = flag;
			CompareValidator newPasswordCompareValidator = changePasswordContainer.NewPasswordCompareValidator;
			newPasswordCompareValidator.ErrorMessage = this.ConfirmPasswordCompareErrorMessage;
			newPasswordCompareValidator.Enabled = flag;
			newPasswordCompareValidator.Visible = flag;
			if (this._validatorTextStyle != null)
			{
				passwordRequired.ApplyStyle(this._validatorTextStyle);
				newPasswordRequired.ApplyStyle(this._validatorTextStyle);
				confirmNewPasswordRequired.ApplyStyle(this._validatorTextStyle);
				newPasswordCompareValidator.ApplyStyle(this._validatorTextStyle);
			}
			RegularExpressionValidator regExpValidator = changePasswordContainer.RegExpValidator;
			regExpValidator.ErrorMessage = this.NewPasswordRegularExpressionErrorMessage;
			regExpValidator.Enabled = flag;
			regExpValidator.Visible = flag;
			if (this._validatorTextStyle != null)
			{
				regExpValidator.ApplyStyle(this._validatorTextStyle);
			}
			LinkButton changePasswordLinkButton = changePasswordContainer.ChangePasswordLinkButton;
			LinkButton cancelLinkButton = changePasswordContainer.CancelLinkButton;
			ImageButton changePasswordImageButton = changePasswordContainer.ChangePasswordImageButton;
			ImageButton cancelImageButton = changePasswordContainer.CancelImageButton;
			Button changePasswordPushButton = changePasswordContainer.ChangePasswordPushButton;
			Button cancelPushButton = changePasswordContainer.CancelPushButton;
			WebControl webControl = null;
			WebControl webControl2 = null;
			switch (this.CancelButtonType)
			{
			case ButtonType.Button:
				cancelPushButton.Text = this.CancelButtonText;
				webControl2 = cancelPushButton;
				break;
			case ButtonType.Image:
				cancelImageButton.ImageUrl = this.CancelButtonImageUrl;
				cancelImageButton.AlternateText = this.CancelButtonText;
				webControl2 = cancelImageButton;
				break;
			case ButtonType.Link:
				cancelLinkButton.Text = this.CancelButtonText;
				webControl2 = cancelLinkButton;
				break;
			}
			switch (this.ChangePasswordButtonType)
			{
			case ButtonType.Button:
				changePasswordPushButton.Text = this.ChangePasswordButtonText;
				webControl = changePasswordPushButton;
				break;
			case ButtonType.Image:
				changePasswordImageButton.ImageUrl = this.ChangePasswordButtonImageUrl;
				changePasswordImageButton.AlternateText = this.ChangePasswordButtonText;
				webControl = changePasswordImageButton;
				break;
			case ButtonType.Link:
				changePasswordLinkButton.Text = this.ChangePasswordButtonText;
				webControl = changePasswordLinkButton;
				break;
			}
			changePasswordLinkButton.Visible = false;
			changePasswordImageButton.Visible = false;
			changePasswordPushButton.Visible = false;
			cancelLinkButton.Visible = false;
			cancelImageButton.Visible = false;
			cancelPushButton.Visible = false;
			webControl.Visible = true;
			webControl2.Visible = true;
			webControl2.TabIndex = this.TabIndex;
			webControl.TabIndex = this.TabIndex;
			if (this.CancelButtonStyle != null)
			{
				webControl2.ApplyStyle(this.CancelButtonStyle);
			}
			if (this.ChangePasswordButtonStyle != null)
			{
				webControl.ApplyStyle(this.ChangePasswordButtonStyle);
			}
			Image createUserIcon = changePasswordContainer.CreateUserIcon;
			HyperLink createUserLink = changePasswordContainer.CreateUserLink;
			LiteralControl createUserLinkSeparator = changePasswordContainer.CreateUserLinkSeparator;
			HyperLink passwordRecoveryLink = changePasswordContainer.PasswordRecoveryLink;
			Image passwordRecoveryIcon = changePasswordContainer.PasswordRecoveryIcon;
			HyperLink helpPageLink = changePasswordContainer.HelpPageLink;
			Image helpPageIcon = changePasswordContainer.HelpPageIcon;
			LiteralControl helpPageLinkSeparator = changePasswordContainer.HelpPageLinkSeparator;
			LiteralControl editProfileLinkSeparator = changePasswordContainer.EditProfileLinkSeparator;
			HyperLink editProfileLink = changePasswordContainer.EditProfileLink;
			Image editProfileIcon = changePasswordContainer.EditProfileIcon;
			string createUserText = this.CreateUserText;
			string createUserIconUrl = this.CreateUserIconUrl;
			string passwordRecoveryText = this.PasswordRecoveryText;
			string passwordRecoveryIconUrl = this.PasswordRecoveryIconUrl;
			string helpPageText = this.HelpPageText;
			string helpPageIconUrl = this.HelpPageIconUrl;
			string editProfileText = this.EditProfileText;
			string editProfileIconUrl = this.EditProfileIconUrl;
			bool flag2 = createUserText.Length > 0;
			bool flag3 = passwordRecoveryText.Length > 0;
			bool flag4 = helpPageText.Length > 0;
			bool flag5 = helpPageIconUrl.Length > 0;
			bool flag6 = createUserIconUrl.Length > 0;
			bool flag7 = passwordRecoveryIconUrl.Length > 0;
			bool flag8 = flag4 || flag5;
			bool flag9 = flag2 || flag6;
			bool flag10 = flag3 || flag7;
			bool flag11 = editProfileText.Length > 0;
			bool flag12 = editProfileIconUrl.Length > 0;
			bool flag13 = flag11 || flag12;
			helpPageLink.Visible = flag4;
			helpPageLinkSeparator.Visible = flag8 && (flag10 || flag9 || flag13);
			if (flag4)
			{
				helpPageLink.Text = helpPageText;
				helpPageLink.NavigateUrl = this.HelpPageUrl;
				helpPageLink.TabIndex = this.TabIndex;
			}
			helpPageIcon.Visible = flag5;
			if (flag5)
			{
				helpPageIcon.ImageUrl = helpPageIconUrl;
				helpPageIcon.AlternateText = this.HelpPageText;
			}
			createUserLink.Visible = flag2;
			createUserLinkSeparator.Visible = flag9 && (flag10 || flag13);
			if (flag2)
			{
				createUserLink.Text = createUserText;
				createUserLink.NavigateUrl = this.CreateUserUrl;
				createUserLink.TabIndex = this.TabIndex;
			}
			createUserIcon.Visible = flag6;
			if (flag6)
			{
				createUserIcon.ImageUrl = createUserIconUrl;
				createUserIcon.AlternateText = this.CreateUserText;
			}
			passwordRecoveryLink.Visible = flag3;
			if (flag3)
			{
				passwordRecoveryLink.Text = passwordRecoveryText;
				passwordRecoveryLink.NavigateUrl = this.PasswordRecoveryUrl;
				passwordRecoveryLink.TabIndex = this.TabIndex;
			}
			passwordRecoveryIcon.Visible = flag7;
			if (flag7)
			{
				passwordRecoveryIcon.ImageUrl = passwordRecoveryIconUrl;
				passwordRecoveryIcon.AlternateText = this.PasswordRecoveryText;
			}
			editProfileLinkSeparator.Visible = flag10 && flag13;
			editProfileLink.Visible = flag11;
			editProfileIcon.Visible = flag12;
			if (flag11)
			{
				editProfileLink.Text = editProfileText;
				editProfileLink.NavigateUrl = this.EditProfileUrl;
				editProfileLink.TabIndex = this.TabIndex;
			}
			if (flag12)
			{
				editProfileIcon.ImageUrl = editProfileIconUrl;
				editProfileIcon.AlternateText = this.EditProfileText;
			}
			if (flag9 || flag10 || flag8 || flag13)
			{
				if (this._hyperLinkStyle != null)
				{
					TableItemStyle tableItemStyle = new TableItemStyle();
					tableItemStyle.CopyFrom(this._hyperLinkStyle);
					tableItemStyle.Font.Reset();
					LoginUtil.SetTableCellStyle(createUserLink, tableItemStyle);
					createUserLink.Font.CopyFrom(this._hyperLinkStyle.Font);
					createUserLink.ForeColor = this._hyperLinkStyle.ForeColor;
					passwordRecoveryLink.Font.CopyFrom(this._hyperLinkStyle.Font);
					passwordRecoveryLink.ForeColor = this._hyperLinkStyle.ForeColor;
					helpPageLink.Font.CopyFrom(this._hyperLinkStyle.Font);
					helpPageLink.ForeColor = this._hyperLinkStyle.ForeColor;
					editProfileLink.Font.CopyFrom(this._hyperLinkStyle.Font);
					editProfileLink.ForeColor = this._hyperLinkStyle.ForeColor;
				}
				LoginUtil.SetTableCellVisible(helpPageLink, true);
			}
			else
			{
				LoginUtil.SetTableCellVisible(helpPageLink, false);
			}
			Control failureTextLabel = changePasswordContainer.FailureTextLabel;
			if (((ITextControl)failureTextLabel).Text.Length > 0)
			{
				LoginUtil.SetTableCellStyle(failureTextLabel, this.FailureTextStyle);
				LoginUtil.SetTableCellVisible(failureTextLabel, true);
				return;
			}
			LoginUtil.SetTableCellVisible(failureTextLabel, false);
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x000FF948 File Offset: 0x000FE948
		internal void SetDefaultSuccessViewProperties()
		{
			ChangePassword.SuccessContainer successContainer = this._successContainer;
			LinkButton continueLinkButton = successContainer.ContinueLinkButton;
			ImageButton continueImageButton = successContainer.ContinueImageButton;
			Button continuePushButton = successContainer.ContinuePushButton;
			successContainer.BorderTable.CellPadding = this.BorderPadding;
			successContainer.BorderTable.CellSpacing = 0;
			WebControl webControl = null;
			switch (this.ContinueButtonType)
			{
			case ButtonType.Button:
				continuePushButton.Text = this.ContinueButtonText;
				webControl = continuePushButton;
				break;
			case ButtonType.Image:
				continueImageButton.ImageUrl = this.ContinueButtonImageUrl;
				continueImageButton.AlternateText = this.ContinueButtonText;
				webControl = continueImageButton;
				break;
			case ButtonType.Link:
				continueLinkButton.Text = this.ContinueButtonText;
				webControl = continueLinkButton;
				break;
			}
			continueLinkButton.Visible = false;
			continueImageButton.Visible = false;
			continuePushButton.Visible = false;
			webControl.Visible = true;
			webControl.TabIndex = this.TabIndex;
			webControl.AccessKey = this.AccessKey;
			if (this.ContinueButtonStyle != null)
			{
				webControl.ApplyStyle(this.ContinueButtonStyle);
			}
			LoginUtil.ApplyStyleToLiteral(successContainer.Title, this.SuccessTitleText, this._titleTextStyle, true);
			LoginUtil.ApplyStyleToLiteral(successContainer.SuccessTextLabel, this.SuccessText, this._successTextStyle, true);
			string editProfileText = this.EditProfileText;
			string editProfileIconUrl = this.EditProfileIconUrl;
			bool flag = editProfileText.Length > 0;
			bool flag2 = editProfileIconUrl.Length > 0;
			HyperLink editProfileLink = successContainer.EditProfileLink;
			Image editProfileIcon = successContainer.EditProfileIcon;
			editProfileIcon.Visible = flag2;
			editProfileLink.Visible = flag;
			if (flag)
			{
				editProfileLink.Text = editProfileText;
				editProfileLink.NavigateUrl = this.EditProfileUrl;
				editProfileLink.TabIndex = this.TabIndex;
				if (this._hyperLinkStyle != null)
				{
					Style style = new TableItemStyle();
					style.CopyFrom(this._hyperLinkStyle);
					style.Font.Reset();
					LoginUtil.SetTableCellStyle(editProfileLink, style);
					editProfileLink.Font.CopyFrom(this._hyperLinkStyle.Font);
					editProfileLink.ForeColor = this._hyperLinkStyle.ForeColor;
				}
			}
			if (flag2)
			{
				editProfileIcon.ImageUrl = editProfileIconUrl;
				editProfileIcon.AlternateText = this.EditProfileText;
			}
			LoginUtil.SetTableCellVisible(editProfileLink, flag || flag2);
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x000FFB5C File Offset: 0x000FEB5C
		private void SetEditableChildProperties()
		{
			if (this.UserNameInternal.Length > 0 && this.DisplayUserName)
			{
				ITextControl textControl = (ITextControl)this._changePasswordContainer.UserNameTextBox;
				if (textControl != null)
				{
					textControl.Text = this.UserNameInternal;
				}
			}
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x000FFBA0 File Offset: 0x000FEBA0
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this._changePasswordButtonStyle != null)
			{
				((IStateManager)this._changePasswordButtonStyle).TrackViewState();
			}
			if (this._labelStyle != null)
			{
				((IStateManager)this._labelStyle).TrackViewState();
			}
			if (this._textBoxStyle != null)
			{
				((IStateManager)this._textBoxStyle).TrackViewState();
			}
			if (this._successTextStyle != null)
			{
				((IStateManager)this._successTextStyle).TrackViewState();
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
			if (this._passwordHintStyle != null)
			{
				((IStateManager)this._passwordHintStyle).TrackViewState();
			}
			if (this._failureTextStyle != null)
			{
				((IStateManager)this._failureTextStyle).TrackViewState();
			}
			if (this._mailDefinition != null)
			{
				((IStateManager)this._mailDefinition).TrackViewState();
			}
			if (this._cancelButtonStyle != null)
			{
				((IStateManager)this._cancelButtonStyle).TrackViewState();
			}
			if (this._continueButtonStyle != null)
			{
				((IStateManager)this._continueButtonStyle).TrackViewState();
			}
			if (this._validatorTextStyle != null)
			{
				((IStateManager)this._validatorTextStyle).TrackViewState();
			}
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x000FFCAC File Offset: 0x000FECAC
		private void UpdateValidators()
		{
			if (base.DesignMode)
			{
				return;
			}
			ChangePassword.ChangePasswordContainer changePasswordContainer = this._changePasswordContainer;
			if (changePasswordContainer != null)
			{
				bool displayUserName = this.DisplayUserName;
				RequiredFieldValidator userNameRequired = changePasswordContainer.UserNameRequired;
				if (userNameRequired != null)
				{
					userNameRequired.Enabled = displayUserName;
					userNameRequired.Visible = displayUserName;
				}
				bool regExpEnabled = this.RegExpEnabled;
				RegularExpressionValidator regExpValidator = changePasswordContainer.RegExpValidator;
				if (regExpValidator != null)
				{
					regExpValidator.Enabled = regExpEnabled;
					regExpValidator.Visible = regExpEnabled;
				}
			}
		}

		// Token: 0x06003CD1 RID: 15569 RVA: 0x000FFD10 File Offset: 0x000FED10
		private void UserNameTextChanged(object source, EventArgs e)
		{
			string text = ((ITextControl)source).Text;
			if (!string.IsNullOrEmpty(text))
			{
				this.UserName = text;
			}
		}

		// Token: 0x04002701 RID: 9985
		private const string _userNameID = "UserName";

		// Token: 0x04002702 RID: 9986
		private const string _currentPasswordID = "CurrentPassword";

		// Token: 0x04002703 RID: 9987
		private const string _newPasswordID = "NewPassword";

		// Token: 0x04002704 RID: 9988
		private const string _confirmNewPasswordID = "ConfirmNewPassword";

		// Token: 0x04002705 RID: 9989
		private const string _failureTextID = "FailureText";

		// Token: 0x04002706 RID: 9990
		private const string _userNameRequiredID = "UserNameRequired";

		// Token: 0x04002707 RID: 9991
		private const string _currentPasswordRequiredID = "CurrentPasswordRequired";

		// Token: 0x04002708 RID: 9992
		private const string _newPasswordRequiredID = "NewPasswordRequired";

		// Token: 0x04002709 RID: 9993
		private const string _confirmNewPasswordRequiredID = "ConfirmNewPasswordRequired";

		// Token: 0x0400270A RID: 9994
		private const string _newPasswordCompareID = "NewPasswordCompare";

		// Token: 0x0400270B RID: 9995
		private const string _newPasswordRegExpID = "NewPasswordRegExp";

		// Token: 0x0400270C RID: 9996
		private const string _changePasswordPushButtonID = "ChangePasswordPushButton";

		// Token: 0x0400270D RID: 9997
		private const string _changePasswordImageButtonID = "ChangePasswordImageButton";

		// Token: 0x0400270E RID: 9998
		private const string _changePasswordLinkButtonID = "ChangePasswordLinkButton";

		// Token: 0x0400270F RID: 9999
		private const string _cancelPushButtonID = "CancelPushButton";

		// Token: 0x04002710 RID: 10000
		private const string _cancelImageButtonID = "CancelImageButton";

		// Token: 0x04002711 RID: 10001
		private const string _cancelLinkButtonID = "CancelLinkButton";

		// Token: 0x04002712 RID: 10002
		private const string _continuePushButtonID = "ContinuePushButton";

		// Token: 0x04002713 RID: 10003
		private const string _continueImageButtonID = "ContinueImageButton";

		// Token: 0x04002714 RID: 10004
		private const string _continueLinkButtonID = "ContinueLinkButton";

		// Token: 0x04002715 RID: 10005
		private const string _passwordRecoveryLinkID = "PasswordRecoveryLink";

		// Token: 0x04002716 RID: 10006
		private const string _helpLinkID = "HelpLink";

		// Token: 0x04002717 RID: 10007
		private const string _createUserLinkID = "CreateUserLink";

		// Token: 0x04002718 RID: 10008
		private const string _editProfileLinkID = "EditProfileLink";

		// Token: 0x04002719 RID: 10009
		private const string _editProfileSuccessLinkID = "EditProfileLinkSuccess";

		// Token: 0x0400271A RID: 10010
		private const string _changePasswordViewContainerID = "ChangePasswordContainerID";

		// Token: 0x0400271B RID: 10011
		private const string _successViewContainerID = "SuccessContainerID";

		// Token: 0x0400271C RID: 10012
		private const ValidatorDisplay _requiredFieldValidatorDisplay = ValidatorDisplay.Static;

		// Token: 0x0400271D RID: 10013
		private const ValidatorDisplay _compareFieldValidatorDisplay = ValidatorDisplay.Dynamic;

		// Token: 0x0400271E RID: 10014
		private const ValidatorDisplay _regexpFieldValidatorDisplay = ValidatorDisplay.Dynamic;

		// Token: 0x0400271F RID: 10015
		private const string _userNameReplacementKey = "<%\\s*UserName\\s*%>";

		// Token: 0x04002720 RID: 10016
		private const string _passwordReplacementKey = "<%\\s*Password\\s*%>";

		// Token: 0x04002721 RID: 10017
		private const int _viewStateArrayLength = 14;

		// Token: 0x04002722 RID: 10018
		public static readonly string ChangePasswordButtonCommandName = "ChangePassword";

		// Token: 0x04002723 RID: 10019
		public static readonly string CancelButtonCommandName = "Cancel";

		// Token: 0x04002724 RID: 10020
		public static readonly string ContinueButtonCommandName = "Continue";

		// Token: 0x04002725 RID: 10021
		private ITemplate _changePasswordTemplate;

		// Token: 0x04002726 RID: 10022
		private ChangePassword.ChangePasswordContainer _changePasswordContainer;

		// Token: 0x04002727 RID: 10023
		private ITemplate _successTemplate;

		// Token: 0x04002728 RID: 10024
		private ChangePassword.SuccessContainer _successContainer;

		// Token: 0x04002729 RID: 10025
		private string _userName;

		// Token: 0x0400272A RID: 10026
		private string _password;

		// Token: 0x0400272B RID: 10027
		private string _newPassword;

		// Token: 0x0400272C RID: 10028
		private string _confirmNewPassword;

		// Token: 0x0400272D RID: 10029
		private bool _convertingToTemplate;

		// Token: 0x0400272E RID: 10030
		private bool _renderDesignerRegion;

		// Token: 0x0400272F RID: 10031
		private ChangePassword.View _currentView;

		// Token: 0x04002730 RID: 10032
		private Style _changePasswordButtonStyle;

		// Token: 0x04002731 RID: 10033
		private TableItemStyle _labelStyle;

		// Token: 0x04002732 RID: 10034
		private Style _textBoxStyle;

		// Token: 0x04002733 RID: 10035
		private TableItemStyle _hyperLinkStyle;

		// Token: 0x04002734 RID: 10036
		private TableItemStyle _instructionTextStyle;

		// Token: 0x04002735 RID: 10037
		private TableItemStyle _titleTextStyle;

		// Token: 0x04002736 RID: 10038
		private TableItemStyle _failureTextStyle;

		// Token: 0x04002737 RID: 10039
		private TableItemStyle _successTextStyle;

		// Token: 0x04002738 RID: 10040
		private TableItemStyle _passwordHintStyle;

		// Token: 0x04002739 RID: 10041
		private Style _cancelButtonStyle;

		// Token: 0x0400273A RID: 10042
		private Style _continueButtonStyle;

		// Token: 0x0400273B RID: 10043
		private Style _validatorTextStyle;

		// Token: 0x0400273C RID: 10044
		private MailDefinition _mailDefinition;

		// Token: 0x0400273D RID: 10045
		private Control _validatorRow;

		// Token: 0x0400273E RID: 10046
		private Control _passwordHintTableRow;

		// Token: 0x0400273F RID: 10047
		private Control _userNameTableRow;

		// Token: 0x04002740 RID: 10048
		private static readonly object EventChangePasswordError = new object();

		// Token: 0x04002741 RID: 10049
		private static readonly object EventCancelButtonClick = new object();

		// Token: 0x04002742 RID: 10050
		private static readonly object EventContinueButtonClick = new object();

		// Token: 0x04002743 RID: 10051
		private static readonly object EventChangingPassword = new object();

		// Token: 0x04002744 RID: 10052
		private static readonly object EventChangedPassword = new object();

		// Token: 0x04002745 RID: 10053
		private static readonly object EventSendingMail = new object();

		// Token: 0x04002746 RID: 10054
		private static readonly object EventSendMailError = new object();

		// Token: 0x020004E2 RID: 1250
		private sealed class DefaultSuccessTemplate : ITemplate
		{
			// Token: 0x06003CD4 RID: 15572 RVA: 0x000FFDB1 File Offset: 0x000FEDB1
			public DefaultSuccessTemplate(ChangePassword owner)
			{
				this._owner = owner;
			}

			// Token: 0x06003CD5 RID: 15573 RVA: 0x000FFDC0 File Offset: 0x000FEDC0
			private void CreateControls(ChangePassword.SuccessContainer successContainer)
			{
				successContainer.Title = new Literal();
				successContainer.SuccessTextLabel = new Literal();
				successContainer.EditProfileLink = new HyperLink();
				successContainer.EditProfileLink.ID = "EditProfileLinkSuccess";
				successContainer.EditProfileIcon = new Image();
				successContainer.ContinueLinkButton = new LinkButton
				{
					ID = "ContinueLinkButton",
					CommandName = ChangePassword.ContinueButtonCommandName,
					CausesValidation = false
				};
				successContainer.ContinueImageButton = new ImageButton
				{
					ID = "ContinueImageButton",
					CommandName = ChangePassword.ContinueButtonCommandName,
					CausesValidation = false
				};
				successContainer.ContinuePushButton = new Button
				{
					ID = "ContinuePushButton",
					CommandName = ChangePassword.ContinueButtonCommandName,
					CausesValidation = false
				};
			}

			// Token: 0x06003CD6 RID: 15574 RVA: 0x000FFE88 File Offset: 0x000FEE88
			private void LayoutControls(ChangePassword.SuccessContainer successContainer)
			{
				Table table = new Table();
				table.CellPadding = 0;
				TableRow tableRow = new LoginUtil.DisappearingTableRow();
				TableCell tableCell = new TableCell();
				tableCell.ColumnSpan = 2;
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(successContainer.Title);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.Controls.Add(successContainer.SuccessTextLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 2;
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				tableCell.Controls.Add(successContainer.ContinuePushButton);
				tableCell.Controls.Add(successContainer.ContinueLinkButton);
				tableCell.Controls.Add(successContainer.ContinueImageButton);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 2;
				tableCell.Controls.Add(successContainer.EditProfileIcon);
				tableCell.Controls.Add(successContainer.EditProfileLink);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				Table table2 = LoginUtil.CreateChildTable(this._owner.ConvertingToTemplate);
				tableRow = new TableRow();
				tableCell = new TableCell();
				tableCell.Controls.Add(table);
				tableRow.Cells.Add(tableCell);
				table2.Rows.Add(tableRow);
				successContainer.LayoutTable = table;
				successContainer.BorderTable = table2;
				successContainer.Controls.Add(table2);
			}

			// Token: 0x06003CD7 RID: 15575 RVA: 0x00100034 File Offset: 0x000FF034
			void ITemplate.InstantiateIn(Control container)
			{
				ChangePassword.SuccessContainer successContainer = (ChangePassword.SuccessContainer)container;
				this.CreateControls(successContainer);
				this.LayoutControls(successContainer);
			}

			// Token: 0x04002747 RID: 10055
			private ChangePassword _owner;
		}

		// Token: 0x020004EA RID: 1258
		internal sealed class SuccessContainer : LoginUtil.GenericContainer<ChangePassword>, INonBindingContainer, INamingContainer
		{
			// Token: 0x06003D0B RID: 15627 RVA: 0x00100879 File Offset: 0x000FF879
			public SuccessContainer(ChangePassword owner)
				: base(owner)
			{
			}

			// Token: 0x17000E35 RID: 3637
			// (get) Token: 0x06003D0C RID: 15628 RVA: 0x00100882 File Offset: 0x000FF882
			// (set) Token: 0x06003D0D RID: 15629 RVA: 0x0010088A File Offset: 0x000FF88A
			internal ImageButton ContinueImageButton
			{
				get
				{
					return this._continueImageButton;
				}
				set
				{
					this._continueImageButton = value;
				}
			}

			// Token: 0x17000E36 RID: 3638
			// (get) Token: 0x06003D0E RID: 15630 RVA: 0x00100893 File Offset: 0x000FF893
			// (set) Token: 0x06003D0F RID: 15631 RVA: 0x0010089B File Offset: 0x000FF89B
			internal LinkButton ContinueLinkButton
			{
				get
				{
					return this._continueLinkButton;
				}
				set
				{
					this._continueLinkButton = value;
				}
			}

			// Token: 0x17000E37 RID: 3639
			// (get) Token: 0x06003D10 RID: 15632 RVA: 0x001008A4 File Offset: 0x000FF8A4
			// (set) Token: 0x06003D11 RID: 15633 RVA: 0x001008AC File Offset: 0x000FF8AC
			internal Button ContinuePushButton
			{
				get
				{
					return this._continuePushButton;
				}
				set
				{
					this._continuePushButton = value;
				}
			}

			// Token: 0x17000E38 RID: 3640
			// (get) Token: 0x06003D12 RID: 15634 RVA: 0x001008B5 File Offset: 0x000FF8B5
			protected override bool ConvertingToTemplate
			{
				get
				{
					return base.Owner.ConvertingToTemplate;
				}
			}

			// Token: 0x17000E39 RID: 3641
			// (get) Token: 0x06003D13 RID: 15635 RVA: 0x001008C2 File Offset: 0x000FF8C2
			// (set) Token: 0x06003D14 RID: 15636 RVA: 0x001008CA File Offset: 0x000FF8CA
			internal Image EditProfileIcon
			{
				get
				{
					return this._editProfileIcon;
				}
				set
				{
					this._editProfileIcon = value;
				}
			}

			// Token: 0x17000E3A RID: 3642
			// (get) Token: 0x06003D15 RID: 15637 RVA: 0x001008D3 File Offset: 0x000FF8D3
			// (set) Token: 0x06003D16 RID: 15638 RVA: 0x001008DB File Offset: 0x000FF8DB
			internal HyperLink EditProfileLink
			{
				get
				{
					return this._editProfileLink;
				}
				set
				{
					this._editProfileLink = value;
				}
			}

			// Token: 0x17000E3B RID: 3643
			// (get) Token: 0x06003D17 RID: 15639 RVA: 0x001008E4 File Offset: 0x000FF8E4
			// (set) Token: 0x06003D18 RID: 15640 RVA: 0x001008EC File Offset: 0x000FF8EC
			public Literal SuccessTextLabel
			{
				get
				{
					return this._successTextLabel;
				}
				set
				{
					this._successTextLabel = value;
				}
			}

			// Token: 0x17000E3C RID: 3644
			// (get) Token: 0x06003D19 RID: 15641 RVA: 0x001008F5 File Offset: 0x000FF8F5
			// (set) Token: 0x06003D1A RID: 15642 RVA: 0x001008FD File Offset: 0x000FF8FD
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

			// Token: 0x04002750 RID: 10064
			private Literal _successTextLabel;

			// Token: 0x04002751 RID: 10065
			private Button _continuePushButton;

			// Token: 0x04002752 RID: 10066
			private LinkButton _continueLinkButton;

			// Token: 0x04002753 RID: 10067
			private ImageButton _continueImageButton;

			// Token: 0x04002754 RID: 10068
			private Image _editProfileIcon;

			// Token: 0x04002755 RID: 10069
			private HyperLink _editProfileLink;

			// Token: 0x04002756 RID: 10070
			private Literal _title;
		}

		// Token: 0x020004EB RID: 1259
		private sealed class DefaultChangePasswordTemplate : ITemplate
		{
			// Token: 0x06003D1B RID: 15643 RVA: 0x00100906 File Offset: 0x000FF906
			public DefaultChangePasswordTemplate(ChangePassword owner)
			{
				this._owner = owner;
			}

			// Token: 0x06003D1C RID: 15644 RVA: 0x00100918 File Offset: 0x000FF918
			private RequiredFieldValidator CreateRequiredFieldValidator(string id, TextBox textBox, string validationGroup, bool enableValidation)
			{
				return new RequiredFieldValidator
				{
					ID = id,
					ValidationGroup = validationGroup,
					ControlToValidate = textBox.ID,
					Display = ValidatorDisplay.Static,
					Text = SR.GetString("LoginControls_DefaultRequiredFieldValidatorText"),
					Enabled = enableValidation,
					Visible = enableValidation
				};
			}

			// Token: 0x06003D1D RID: 15645 RVA: 0x00100970 File Offset: 0x000FF970
			private void CreateControls(ChangePassword.ChangePasswordContainer container)
			{
				string uniqueID = this._owner.UniqueID;
				container.Title = new Literal();
				container.Instruction = new Literal();
				container.PasswordHintLabel = new Literal();
				TextBox textBox = new TextBox();
				textBox.ID = "UserName";
				container.UserNameTextBox = textBox;
				container.UserNameLabel = new LabelLiteral(textBox);
				bool flag = this._owner.CurrentView == ChangePassword.View.ChangePassword;
				container.UserNameRequired = this.CreateRequiredFieldValidator("UserNameRequired", textBox, uniqueID, flag);
				TextBox textBox2 = new TextBox();
				textBox2.ID = "CurrentPassword";
				textBox2.TextMode = TextBoxMode.Password;
				container.CurrentPasswordTextBox = textBox2;
				container.CurrentPasswordLabel = new LabelLiteral(textBox2);
				container.PasswordRequired = this.CreateRequiredFieldValidator("CurrentPasswordRequired", textBox2, uniqueID, flag);
				TextBox textBox3 = new TextBox();
				textBox3.ID = "NewPassword";
				textBox3.TextMode = TextBoxMode.Password;
				container.NewPasswordTextBox = textBox3;
				container.NewPasswordLabel = new LabelLiteral(textBox3);
				container.NewPasswordRequired = this.CreateRequiredFieldValidator("NewPasswordRequired", textBox3, uniqueID, flag);
				TextBox textBox4 = new TextBox();
				textBox4.ID = "ConfirmNewPassword";
				textBox4.TextMode = TextBoxMode.Password;
				container.ConfirmNewPasswordTextBox = textBox4;
				container.ConfirmNewPasswordLabel = new LabelLiteral(textBox4);
				container.ConfirmNewPasswordRequired = this.CreateRequiredFieldValidator("ConfirmNewPasswordRequired", textBox4, uniqueID, flag);
				container.NewPasswordCompareValidator = new CompareValidator
				{
					ID = "NewPasswordCompare",
					ValidationGroup = uniqueID,
					ControlToValidate = "ConfirmNewPassword",
					ControlToCompare = "NewPassword",
					Operator = ValidationCompareOperator.Equal,
					ErrorMessage = this._owner.ConfirmPasswordCompareErrorMessage,
					Display = ValidatorDisplay.Dynamic,
					Enabled = flag,
					Visible = flag
				};
				container.RegExpValidator = new RegularExpressionValidator
				{
					ID = "NewPasswordRegExp",
					ValidationGroup = uniqueID,
					ControlToValidate = "NewPassword",
					ErrorMessage = this._owner.NewPasswordRegularExpressionErrorMessage,
					ValidationExpression = this._owner.NewPasswordRegularExpression,
					Display = ValidatorDisplay.Dynamic,
					Enabled = flag,
					Visible = flag
				};
				container.ChangePasswordLinkButton = new LinkButton
				{
					ID = "ChangePasswordLinkButton",
					ValidationGroup = uniqueID,
					CommandName = ChangePassword.ChangePasswordButtonCommandName
				};
				container.CancelLinkButton = new LinkButton
				{
					ID = "CancelLinkButton",
					CausesValidation = false,
					CommandName = ChangePassword.CancelButtonCommandName
				};
				container.ChangePasswordImageButton = new ImageButton
				{
					ID = "ChangePasswordImageButton",
					ValidationGroup = uniqueID,
					CommandName = ChangePassword.ChangePasswordButtonCommandName
				};
				container.CancelImageButton = new ImageButton
				{
					ID = "CancelImageButton",
					CommandName = ChangePassword.CancelButtonCommandName,
					CausesValidation = false
				};
				container.ChangePasswordPushButton = new Button
				{
					ID = "ChangePasswordPushButton",
					ValidationGroup = uniqueID,
					CommandName = ChangePassword.ChangePasswordButtonCommandName
				};
				container.CancelPushButton = new Button
				{
					ID = "CancelPushButton",
					CommandName = ChangePassword.CancelButtonCommandName,
					CausesValidation = false
				};
				container.PasswordRecoveryIcon = new Image();
				container.PasswordRecoveryLink = new HyperLink();
				container.PasswordRecoveryLink.ID = "PasswordRecoveryLink";
				container.CreateUserIcon = new Image();
				container.CreateUserLink = new HyperLink();
				container.CreateUserLink.ID = "CreateUserLink";
				container.CreateUserLinkSeparator = new LiteralControl();
				container.HelpPageIcon = new Image();
				container.HelpPageLink = new HyperLink();
				container.HelpPageLink.ID = "HelpLink";
				container.HelpPageLinkSeparator = new LiteralControl();
				container.EditProfileLink = new HyperLink();
				container.EditProfileLink.ID = "EditProfileLink";
				container.EditProfileIcon = new Image();
				container.EditProfileLinkSeparator = new LiteralControl();
				container.FailureTextLabel = new Literal
				{
					ID = "FailureText"
				};
			}

			// Token: 0x06003D1E RID: 15646 RVA: 0x00100D80 File Offset: 0x000FFD80
			private void LayoutControls(ChangePassword.ChangePasswordContainer container)
			{
				Table table = new Table();
				table.CellPadding = 0;
				TableRow tableRow = new LoginUtil.DisappearingTableRow();
				TableCell tableCell = new TableCell();
				tableCell.ColumnSpan = 2;
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(container.Title);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 2;
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(container.Instruction);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				if (this._owner.ConvertingToTemplate)
				{
					container.UserNameLabel.RenderAsLabel = true;
				}
				tableCell.Controls.Add(container.UserNameLabel);
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				tableCell.Controls.Add(container.UserNameTextBox);
				tableCell.Controls.Add(container.UserNameRequired);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				this._owner._userNameTableRow = tableRow;
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				tableCell.Controls.Add(container.CurrentPasswordLabel);
				if (this._owner.ConvertingToTemplate)
				{
					container.CurrentPasswordLabel.RenderAsLabel = true;
				}
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				tableCell.Controls.Add(container.CurrentPasswordTextBox);
				tableCell.Controls.Add(container.PasswordRequired);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				tableCell.Controls.Add(container.NewPasswordLabel);
				if (this._owner.ConvertingToTemplate)
				{
					container.NewPasswordLabel.RenderAsLabel = true;
				}
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				tableCell.Controls.Add(container.NewPasswordTextBox);
				tableCell.Controls.Add(container.NewPasswordRequired);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				tableCell.Controls.Add(container.PasswordHintLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				this._owner._passwordHintTableRow = tableRow;
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				tableCell.Controls.Add(container.ConfirmNewPasswordLabel);
				if (this._owner.ConvertingToTemplate)
				{
					container.ConfirmNewPasswordLabel.RenderAsLabel = true;
				}
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				tableCell.Controls.Add(container.ConfirmNewPasswordTextBox);
				tableCell.Controls.Add(container.ConfirmNewPasswordRequired);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.ColumnSpan = 2;
				tableCell.Controls.Add(container.NewPasswordCompareValidator);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				if (this._owner.RegExpEnabled)
				{
					tableRow = new LoginUtil.DisappearingTableRow();
					tableCell = new TableCell();
					tableCell.HorizontalAlign = HorizontalAlign.Center;
					tableCell.ColumnSpan = 2;
					tableCell.Controls.Add(container.RegExpValidator);
					tableRow.Cells.Add(tableCell);
					table.Rows.Add(tableRow);
				}
				this._owner.ValidatorRow = tableRow;
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.ColumnSpan = 2;
				tableCell.Controls.Add(container.FailureTextLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				tableCell.Controls.Add(container.ChangePasswordLinkButton);
				tableCell.Controls.Add(container.ChangePasswordImageButton);
				tableCell.Controls.Add(container.ChangePasswordPushButton);
				tableRow.Cells.Add(tableCell);
				tableCell = new TableCell();
				tableCell.Controls.Add(container.CancelLinkButton);
				tableCell.Controls.Add(container.CancelImageButton);
				tableCell.Controls.Add(container.CancelPushButton);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new LoginUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 2;
				tableCell.Controls.Add(container.HelpPageIcon);
				tableCell.Controls.Add(container.HelpPageLink);
				tableCell.Controls.Add(container.HelpPageLinkSeparator);
				tableCell.Controls.Add(container.CreateUserIcon);
				tableCell.Controls.Add(container.CreateUserLink);
				container.HelpPageLinkSeparator.Text = "<br />";
				container.CreateUserLinkSeparator.Text = "<br />";
				container.EditProfileLinkSeparator.Text = "<br />";
				tableCell.Controls.Add(container.CreateUserLinkSeparator);
				tableCell.Controls.Add(container.PasswordRecoveryIcon);
				tableCell.Controls.Add(container.PasswordRecoveryLink);
				tableCell.Controls.Add(container.EditProfileLinkSeparator);
				tableCell.Controls.Add(container.EditProfileIcon);
				tableCell.Controls.Add(container.EditProfileLink);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				Table table2 = LoginUtil.CreateChildTable(this._owner.ConvertingToTemplate);
				tableRow = new TableRow();
				tableCell = new TableCell();
				tableCell.Controls.Add(table);
				tableRow.Cells.Add(tableCell);
				table2.Rows.Add(tableRow);
				container.LayoutTable = table;
				container.BorderTable = table2;
				container.Controls.Add(table2);
			}

			// Token: 0x06003D1F RID: 15647 RVA: 0x001013BC File Offset: 0x001003BC
			void ITemplate.InstantiateIn(Control container)
			{
				ChangePassword.ChangePasswordContainer changePasswordContainer = (ChangePassword.ChangePasswordContainer)container;
				this.CreateControls(changePasswordContainer);
				this.LayoutControls(changePasswordContainer);
			}

			// Token: 0x04002757 RID: 10071
			private ChangePassword _owner;
		}

		// Token: 0x020004EC RID: 1260
		internal sealed class ChangePasswordContainer : LoginUtil.GenericContainer<ChangePassword>, INonBindingContainer, INamingContainer
		{
			// Token: 0x06003D20 RID: 15648 RVA: 0x001013DE File Offset: 0x001003DE
			public ChangePasswordContainer(ChangePassword owner)
				: base(owner)
			{
			}

			// Token: 0x17000E3D RID: 3645
			// (get) Token: 0x06003D21 RID: 15649 RVA: 0x001013E7 File Offset: 0x001003E7
			// (set) Token: 0x06003D22 RID: 15650 RVA: 0x001013EF File Offset: 0x001003EF
			internal ImageButton CancelImageButton
			{
				get
				{
					return this._cancelImageButton;
				}
				set
				{
					this._cancelImageButton = value;
				}
			}

			// Token: 0x17000E3E RID: 3646
			// (get) Token: 0x06003D23 RID: 15651 RVA: 0x001013F8 File Offset: 0x001003F8
			// (set) Token: 0x06003D24 RID: 15652 RVA: 0x00101400 File Offset: 0x00100400
			internal LinkButton CancelLinkButton
			{
				get
				{
					return this._cancelLinkButton;
				}
				set
				{
					this._cancelLinkButton = value;
				}
			}

			// Token: 0x17000E3F RID: 3647
			// (get) Token: 0x06003D25 RID: 15653 RVA: 0x00101409 File Offset: 0x00100409
			// (set) Token: 0x06003D26 RID: 15654 RVA: 0x00101411 File Offset: 0x00100411
			internal Button CancelPushButton
			{
				get
				{
					return this._cancelPushButton;
				}
				set
				{
					this._cancelPushButton = value;
				}
			}

			// Token: 0x17000E40 RID: 3648
			// (get) Token: 0x06003D27 RID: 15655 RVA: 0x0010141A File Offset: 0x0010041A
			// (set) Token: 0x06003D28 RID: 15656 RVA: 0x00101422 File Offset: 0x00100422
			internal ImageButton ChangePasswordImageButton
			{
				get
				{
					return this._changePasswordImageButton;
				}
				set
				{
					this._changePasswordImageButton = value;
				}
			}

			// Token: 0x17000E41 RID: 3649
			// (get) Token: 0x06003D29 RID: 15657 RVA: 0x0010142B File Offset: 0x0010042B
			// (set) Token: 0x06003D2A RID: 15658 RVA: 0x00101433 File Offset: 0x00100433
			internal LinkButton ChangePasswordLinkButton
			{
				get
				{
					return this._changePasswordLinkButton;
				}
				set
				{
					this._changePasswordLinkButton = value;
				}
			}

			// Token: 0x17000E42 RID: 3650
			// (get) Token: 0x06003D2B RID: 15659 RVA: 0x0010143C File Offset: 0x0010043C
			// (set) Token: 0x06003D2C RID: 15660 RVA: 0x00101444 File Offset: 0x00100444
			internal Button ChangePasswordPushButton
			{
				get
				{
					return this._changePasswordPushButton;
				}
				set
				{
					this._changePasswordPushButton = value;
				}
			}

			// Token: 0x17000E43 RID: 3651
			// (get) Token: 0x06003D2D RID: 15661 RVA: 0x0010144D File Offset: 0x0010044D
			// (set) Token: 0x06003D2E RID: 15662 RVA: 0x00101455 File Offset: 0x00100455
			internal LabelLiteral ConfirmNewPasswordLabel
			{
				get
				{
					return this._confirmNewPasswordLabel;
				}
				set
				{
					this._confirmNewPasswordLabel = value;
				}
			}

			// Token: 0x17000E44 RID: 3652
			// (get) Token: 0x06003D2F RID: 15663 RVA: 0x0010145E File Offset: 0x0010045E
			// (set) Token: 0x06003D30 RID: 15664 RVA: 0x00101466 File Offset: 0x00100466
			internal RequiredFieldValidator ConfirmNewPasswordRequired
			{
				get
				{
					return this._confirmNewPasswordRequired;
				}
				set
				{
					this._confirmNewPasswordRequired = value;
				}
			}

			// Token: 0x17000E45 RID: 3653
			// (get) Token: 0x06003D31 RID: 15665 RVA: 0x0010146F File Offset: 0x0010046F
			// (set) Token: 0x06003D32 RID: 15666 RVA: 0x0010148B File Offset: 0x0010048B
			internal Control ConfirmNewPasswordTextBox
			{
				get
				{
					if (this._confirmNewPasswordTextBox != null)
					{
						return this._confirmNewPasswordTextBox;
					}
					return base.FindOptionalControl<IEditableTextControl>("ConfirmNewPassword");
				}
				set
				{
					this._confirmNewPasswordTextBox = value;
				}
			}

			// Token: 0x17000E46 RID: 3654
			// (get) Token: 0x06003D33 RID: 15667 RVA: 0x00101494 File Offset: 0x00100494
			protected override bool ConvertingToTemplate
			{
				get
				{
					return base.Owner.ConvertingToTemplate;
				}
			}

			// Token: 0x17000E47 RID: 3655
			// (get) Token: 0x06003D34 RID: 15668 RVA: 0x001014A1 File Offset: 0x001004A1
			// (set) Token: 0x06003D35 RID: 15669 RVA: 0x001014A9 File Offset: 0x001004A9
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

			// Token: 0x17000E48 RID: 3656
			// (get) Token: 0x06003D36 RID: 15670 RVA: 0x001014B2 File Offset: 0x001004B2
			// (set) Token: 0x06003D37 RID: 15671 RVA: 0x001014BA File Offset: 0x001004BA
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

			// Token: 0x17000E49 RID: 3657
			// (get) Token: 0x06003D38 RID: 15672 RVA: 0x001014C3 File Offset: 0x001004C3
			// (set) Token: 0x06003D39 RID: 15673 RVA: 0x001014CB File Offset: 0x001004CB
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

			// Token: 0x17000E4A RID: 3658
			// (get) Token: 0x06003D3A RID: 15674 RVA: 0x001014D4 File Offset: 0x001004D4
			// (set) Token: 0x06003D3B RID: 15675 RVA: 0x001014DC File Offset: 0x001004DC
			internal LabelLiteral CurrentPasswordLabel
			{
				get
				{
					return this._currentPasswordLabel;
				}
				set
				{
					this._currentPasswordLabel = value;
				}
			}

			// Token: 0x17000E4B RID: 3659
			// (get) Token: 0x06003D3C RID: 15676 RVA: 0x001014E5 File Offset: 0x001004E5
			// (set) Token: 0x06003D3D RID: 15677 RVA: 0x00101506 File Offset: 0x00100506
			internal Control CurrentPasswordTextBox
			{
				get
				{
					if (this._currentPasswordTextBox != null)
					{
						return this._currentPasswordTextBox;
					}
					return base.FindRequiredControl<IEditableTextControl>("CurrentPassword", "ChangePassword_NoCurrentPasswordTextBox");
				}
				set
				{
					this._currentPasswordTextBox = value;
				}
			}

			// Token: 0x17000E4C RID: 3660
			// (get) Token: 0x06003D3E RID: 15678 RVA: 0x0010150F File Offset: 0x0010050F
			// (set) Token: 0x06003D3F RID: 15679 RVA: 0x00101517 File Offset: 0x00100517
			internal Image EditProfileIcon
			{
				get
				{
					return this._editProfileIcon;
				}
				set
				{
					this._editProfileIcon = value;
				}
			}

			// Token: 0x17000E4D RID: 3661
			// (get) Token: 0x06003D40 RID: 15680 RVA: 0x00101520 File Offset: 0x00100520
			// (set) Token: 0x06003D41 RID: 15681 RVA: 0x00101528 File Offset: 0x00100528
			internal HyperLink EditProfileLink
			{
				get
				{
					return this._editProfileLink;
				}
				set
				{
					this._editProfileLink = value;
				}
			}

			// Token: 0x17000E4E RID: 3662
			// (get) Token: 0x06003D42 RID: 15682 RVA: 0x00101531 File Offset: 0x00100531
			// (set) Token: 0x06003D43 RID: 15683 RVA: 0x00101539 File Offset: 0x00100539
			internal LiteralControl EditProfileLinkSeparator
			{
				get
				{
					return this._editProfileLinkSeparator;
				}
				set
				{
					this._editProfileLinkSeparator = value;
				}
			}

			// Token: 0x17000E4F RID: 3663
			// (get) Token: 0x06003D44 RID: 15684 RVA: 0x00101542 File Offset: 0x00100542
			// (set) Token: 0x06003D45 RID: 15685 RVA: 0x0010155E File Offset: 0x0010055E
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

			// Token: 0x17000E50 RID: 3664
			// (get) Token: 0x06003D46 RID: 15686 RVA: 0x00101567 File Offset: 0x00100567
			// (set) Token: 0x06003D47 RID: 15687 RVA: 0x0010156F File Offset: 0x0010056F
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

			// Token: 0x17000E51 RID: 3665
			// (get) Token: 0x06003D48 RID: 15688 RVA: 0x00101578 File Offset: 0x00100578
			// (set) Token: 0x06003D49 RID: 15689 RVA: 0x00101580 File Offset: 0x00100580
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

			// Token: 0x17000E52 RID: 3666
			// (get) Token: 0x06003D4A RID: 15690 RVA: 0x00101589 File Offset: 0x00100589
			// (set) Token: 0x06003D4B RID: 15691 RVA: 0x00101591 File Offset: 0x00100591
			internal LiteralControl HelpPageLinkSeparator
			{
				get
				{
					return this._helpPageLinkSeparator;
				}
				set
				{
					this._helpPageLinkSeparator = value;
				}
			}

			// Token: 0x17000E53 RID: 3667
			// (get) Token: 0x06003D4C RID: 15692 RVA: 0x0010159A File Offset: 0x0010059A
			// (set) Token: 0x06003D4D RID: 15693 RVA: 0x001015A2 File Offset: 0x001005A2
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

			// Token: 0x17000E54 RID: 3668
			// (get) Token: 0x06003D4E RID: 15694 RVA: 0x001015AB File Offset: 0x001005AB
			// (set) Token: 0x06003D4F RID: 15695 RVA: 0x001015B3 File Offset: 0x001005B3
			internal CompareValidator NewPasswordCompareValidator
			{
				get
				{
					return this._newPasswordCompareValidator;
				}
				set
				{
					this._newPasswordCompareValidator = value;
				}
			}

			// Token: 0x17000E55 RID: 3669
			// (get) Token: 0x06003D50 RID: 15696 RVA: 0x001015BC File Offset: 0x001005BC
			// (set) Token: 0x06003D51 RID: 15697 RVA: 0x001015C4 File Offset: 0x001005C4
			internal LabelLiteral NewPasswordLabel
			{
				get
				{
					return this._newPasswordLabel;
				}
				set
				{
					this._newPasswordLabel = value;
				}
			}

			// Token: 0x17000E56 RID: 3670
			// (get) Token: 0x06003D52 RID: 15698 RVA: 0x001015CD File Offset: 0x001005CD
			// (set) Token: 0x06003D53 RID: 15699 RVA: 0x001015D5 File Offset: 0x001005D5
			internal RequiredFieldValidator NewPasswordRequired
			{
				get
				{
					return this._newPasswordRequired;
				}
				set
				{
					this._newPasswordRequired = value;
				}
			}

			// Token: 0x17000E57 RID: 3671
			// (get) Token: 0x06003D54 RID: 15700 RVA: 0x001015DE File Offset: 0x001005DE
			// (set) Token: 0x06003D55 RID: 15701 RVA: 0x001015FF File Offset: 0x001005FF
			internal Control NewPasswordTextBox
			{
				get
				{
					if (this._newPasswordTextBox != null)
					{
						return this._newPasswordTextBox;
					}
					return base.FindRequiredControl<IEditableTextControl>("NewPassword", "ChangePassword_NoNewPasswordTextBox");
				}
				set
				{
					this._newPasswordTextBox = value;
				}
			}

			// Token: 0x17000E58 RID: 3672
			// (get) Token: 0x06003D56 RID: 15702 RVA: 0x00101608 File Offset: 0x00100608
			// (set) Token: 0x06003D57 RID: 15703 RVA: 0x00101610 File Offset: 0x00100610
			internal Literal PasswordHintLabel
			{
				get
				{
					return this._passwordHintLabel;
				}
				set
				{
					this._passwordHintLabel = value;
				}
			}

			// Token: 0x17000E59 RID: 3673
			// (get) Token: 0x06003D58 RID: 15704 RVA: 0x00101619 File Offset: 0x00100619
			// (set) Token: 0x06003D59 RID: 15705 RVA: 0x00101621 File Offset: 0x00100621
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

			// Token: 0x17000E5A RID: 3674
			// (get) Token: 0x06003D5A RID: 15706 RVA: 0x0010162A File Offset: 0x0010062A
			// (set) Token: 0x06003D5B RID: 15707 RVA: 0x00101632 File Offset: 0x00100632
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

			// Token: 0x17000E5B RID: 3675
			// (get) Token: 0x06003D5C RID: 15708 RVA: 0x0010163B File Offset: 0x0010063B
			// (set) Token: 0x06003D5D RID: 15709 RVA: 0x00101643 File Offset: 0x00100643
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

			// Token: 0x17000E5C RID: 3676
			// (get) Token: 0x06003D5E RID: 15710 RVA: 0x0010164C File Offset: 0x0010064C
			// (set) Token: 0x06003D5F RID: 15711 RVA: 0x00101654 File Offset: 0x00100654
			internal RegularExpressionValidator RegExpValidator
			{
				get
				{
					return this._regExpValidator;
				}
				set
				{
					this._regExpValidator = value;
				}
			}

			// Token: 0x17000E5D RID: 3677
			// (get) Token: 0x06003D60 RID: 15712 RVA: 0x0010165D File Offset: 0x0010065D
			// (set) Token: 0x06003D61 RID: 15713 RVA: 0x00101665 File Offset: 0x00100665
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

			// Token: 0x17000E5E RID: 3678
			// (get) Token: 0x06003D62 RID: 15714 RVA: 0x0010166E File Offset: 0x0010066E
			// (set) Token: 0x06003D63 RID: 15715 RVA: 0x00101676 File Offset: 0x00100676
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

			// Token: 0x17000E5F RID: 3679
			// (get) Token: 0x06003D64 RID: 15716 RVA: 0x0010167F File Offset: 0x0010067F
			// (set) Token: 0x06003D65 RID: 15717 RVA: 0x00101687 File Offset: 0x00100687
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

			// Token: 0x17000E60 RID: 3680
			// (get) Token: 0x06003D66 RID: 15718 RVA: 0x00101690 File Offset: 0x00100690
			// (set) Token: 0x06003D67 RID: 15719 RVA: 0x001016D0 File Offset: 0x001006D0
			internal Control UserNameTextBox
			{
				get
				{
					if (this._userNameTextBox != null)
					{
						return this._userNameTextBox;
					}
					if (base.Owner.DisplayUserName)
					{
						return base.FindRequiredControl<IEditableTextControl>("UserName", "ChangePassword_NoUserNameTextBox");
					}
					base.VerifyControlNotPresent<IEditableTextControl>("UserName", "ChangePassword_UserNameTextBoxNotAllowed");
					return null;
				}
				set
				{
					this._userNameTextBox = value;
				}
			}

			// Token: 0x04002758 RID: 10072
			private LiteralControl _createUserLinkSeparator;

			// Token: 0x04002759 RID: 10073
			private LiteralControl _helpPageLinkSeparator;

			// Token: 0x0400275A RID: 10074
			private LiteralControl _editProfileLinkSeparator;

			// Token: 0x0400275B RID: 10075
			private Control _failureTextLabel;

			// Token: 0x0400275C RID: 10076
			private ImageButton _changePasswordImageButton;

			// Token: 0x0400275D RID: 10077
			private LinkButton _changePasswordLinkButton;

			// Token: 0x0400275E RID: 10078
			private Button _changePasswordPushButton;

			// Token: 0x0400275F RID: 10079
			private ImageButton _cancelImageButton;

			// Token: 0x04002760 RID: 10080
			private LinkButton _cancelLinkButton;

			// Token: 0x04002761 RID: 10081
			private Button _cancelPushButton;

			// Token: 0x04002762 RID: 10082
			private Image _createUserIcon;

			// Token: 0x04002763 RID: 10083
			private Image _helpPageIcon;

			// Token: 0x04002764 RID: 10084
			private Image _passwordRecoveryIcon;

			// Token: 0x04002765 RID: 10085
			private Image _editProfileIcon;

			// Token: 0x04002766 RID: 10086
			private RequiredFieldValidator _passwordRequired;

			// Token: 0x04002767 RID: 10087
			private RequiredFieldValidator _userNameRequired;

			// Token: 0x04002768 RID: 10088
			private RequiredFieldValidator _confirmNewPasswordRequired;

			// Token: 0x04002769 RID: 10089
			private RequiredFieldValidator _newPasswordRequired;

			// Token: 0x0400276A RID: 10090
			private CompareValidator _newPasswordCompareValidator;

			// Token: 0x0400276B RID: 10091
			private RegularExpressionValidator _regExpValidator;

			// Token: 0x0400276C RID: 10092
			private Literal _title;

			// Token: 0x0400276D RID: 10093
			private Literal _instruction;

			// Token: 0x0400276E RID: 10094
			private LabelLiteral _userNameLabel;

			// Token: 0x0400276F RID: 10095
			private LabelLiteral _currentPasswordLabel;

			// Token: 0x04002770 RID: 10096
			private LabelLiteral _newPasswordLabel;

			// Token: 0x04002771 RID: 10097
			private LabelLiteral _confirmNewPasswordLabel;

			// Token: 0x04002772 RID: 10098
			private Literal _passwordHintLabel;

			// Token: 0x04002773 RID: 10099
			private Control _userNameTextBox;

			// Token: 0x04002774 RID: 10100
			private Control _currentPasswordTextBox;

			// Token: 0x04002775 RID: 10101
			private Control _newPasswordTextBox;

			// Token: 0x04002776 RID: 10102
			private Control _confirmNewPasswordTextBox;

			// Token: 0x04002777 RID: 10103
			private HyperLink _helpPageLink;

			// Token: 0x04002778 RID: 10104
			private HyperLink _passwordRecoveryLink;

			// Token: 0x04002779 RID: 10105
			private HyperLink _createUserLink;

			// Token: 0x0400277A RID: 10106
			private HyperLink _editProfileLink;
		}

		// Token: 0x020004ED RID: 1261
		internal enum View
		{
			// Token: 0x0400277C RID: 10108
			ChangePassword,
			// Token: 0x0400277D RID: 10109
			Success
		}
	}
}
