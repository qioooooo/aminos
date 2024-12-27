using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;
using System.Web.Security;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200051F RID: 1311
	[Designer("System.Web.UI.Design.WebControls.CreateUserWizardDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Bindable(false)]
	[ToolboxData("<{0}:CreateUserWizard runat=\"server\"> <WizardSteps> <asp:CreateUserWizardStep runat=\"server\"/> <asp:CompleteWizardStep runat=\"server\"/> </WizardSteps> </{0}:CreateUserWizard>")]
	[DefaultEvent("CreatedUser")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CreateUserWizard : Wizard
	{
		// Token: 0x06003FF7 RID: 16375 RVA: 0x00109DE8 File Offset: 0x00108DE8
		public CreateUserWizard()
		{
			this._displaySideBarDefault = false;
			this._displaySideBar = this._displaySideBarDefault;
		}

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x06003FF8 RID: 16376 RVA: 0x00109E03 File Offset: 0x00108E03
		// (set) Token: 0x06003FF9 RID: 16377 RVA: 0x00109E0B File Offset: 0x00108E0B
		[DefaultValue(0)]
		public override int ActiveStepIndex
		{
			get
			{
				return base.ActiveStepIndex;
			}
			set
			{
				base.ActiveStepIndex = value;
			}
		}

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x06003FFA RID: 16378 RVA: 0x00109E14 File Offset: 0x00108E14
		// (set) Token: 0x06003FFB RID: 16379 RVA: 0x00109E2A File Offset: 0x00108E2A
		[Themeable(false)]
		[DefaultValue("")]
		[Localizable(true)]
		[WebSysDescription("CreateUserWizard_Answer")]
		[WebCategory("Appearance")]
		public virtual string Answer
		{
			get
			{
				if (this._answer != null)
				{
					return this._answer;
				}
				return string.Empty;
			}
			set
			{
				this._answer = value;
			}
		}

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x06003FFC RID: 16380 RVA: 0x00109E34 File Offset: 0x00108E34
		private string AnswerInternal
		{
			get
			{
				string text = this.Answer;
				if (string.IsNullOrEmpty(this.Answer) && this._createUserStepContainer != null)
				{
					ITextControl textControl = (ITextControl)this._createUserStepContainer.AnswerTextBox;
					if (textControl != null)
					{
						text = textControl.Text;
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					text = null;
				}
				return text;
			}
		}

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x06003FFD RID: 16381 RVA: 0x00109E84 File Offset: 0x00108E84
		// (set) Token: 0x06003FFE RID: 16382 RVA: 0x00109EB6 File Offset: 0x00108EB6
		[WebSysDefaultValue("CreateUserWizard_DefaultAnswerLabelText")]
		[WebCategory("Appearance")]
		[Localizable(true)]
		[WebSysDescription("CreateUserWizard_AnswerLabelText")]
		public virtual string AnswerLabelText
		{
			get
			{
				object obj = this.ViewState["AnswerLabelText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultAnswerLabelText");
			}
			set
			{
				this.ViewState["AnswerLabelText"] = value;
			}
		}

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x06003FFF RID: 16383 RVA: 0x00109ECC File Offset: 0x00108ECC
		// (set) Token: 0x06004000 RID: 16384 RVA: 0x00109EFE File Offset: 0x00108EFE
		[Localizable(true)]
		[WebCategory("Validation")]
		[WebSysDefaultValue("CreateUserWizard_DefaultAnswerRequiredErrorMessage")]
		[WebSysDescription("LoginControls_AnswerRequiredErrorMessage")]
		public virtual string AnswerRequiredErrorMessage
		{
			get
			{
				object obj = this.ViewState["AnswerRequiredErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultAnswerRequiredErrorMessage");
			}
			set
			{
				this.ViewState["AnswerRequiredErrorMessage"] = value;
			}
		}

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x06004001 RID: 16385 RVA: 0x00109F14 File Offset: 0x00108F14
		// (set) Token: 0x06004002 RID: 16386 RVA: 0x00109F3D File Offset: 0x00108F3D
		[WebCategory("Behavior")]
		[WebSysDescription("CreateUserWizard_AutoGeneratePassword")]
		[DefaultValue(false)]
		[Themeable(false)]
		public virtual bool AutoGeneratePassword
		{
			get
			{
				object obj = this.ViewState["AutoGeneratePassword"];
				return obj != null && (bool)obj;
			}
			set
			{
				if (this.AutoGeneratePassword != value)
				{
					this.ViewState["AutoGeneratePassword"] = value;
					base.RequiresControlsRecreation();
				}
			}
		}

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x06004003 RID: 16387 RVA: 0x00109F64 File Offset: 0x00108F64
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[WebSysDescription("CreateUserWizard_CompleteStep")]
		[WebCategory("Appearance")]
		public CompleteWizardStep CompleteStep
		{
			get
			{
				this.EnsureChildControls();
				return this._completeStep;
			}
		}

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x06004004 RID: 16388 RVA: 0x00109F74 File Offset: 0x00108F74
		// (set) Token: 0x06004005 RID: 16389 RVA: 0x00109FA6 File Offset: 0x00108FA6
		[Localizable(true)]
		[WebSysDescription("CreateUserWizard_CompleteSuccessText")]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("CreateUserWizard_DefaultCompleteSuccessText")]
		public virtual string CompleteSuccessText
		{
			get
			{
				object obj = this.ViewState["CompleteSuccessText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultCompleteSuccessText");
			}
			set
			{
				this.ViewState["CompleteSuccessText"] = value;
			}
		}

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x06004006 RID: 16390 RVA: 0x00109FB9 File Offset: 0x00108FB9
		[WebCategory("Styles")]
		[NotifyParentProperty(true)]
		[WebSysDescription("CreateUserWizard_CompleteSuccessTextStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public TableItemStyle CompleteSuccessTextStyle
		{
			get
			{
				if (this._completeSuccessTextStyle == null)
				{
					this._completeSuccessTextStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._completeSuccessTextStyle).TrackViewState();
					}
				}
				return this._completeSuccessTextStyle;
			}
		}

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x06004007 RID: 16391 RVA: 0x00109FE7 File Offset: 0x00108FE7
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual string ConfirmPassword
		{
			get
			{
				if (this._confirmPassword != null)
				{
					return this._confirmPassword;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x06004008 RID: 16392 RVA: 0x0010A000 File Offset: 0x00109000
		// (set) Token: 0x06004009 RID: 16393 RVA: 0x0010A032 File Offset: 0x00109032
		[WebCategory("Validation")]
		[WebSysDescription("ChangePassword_ConfirmPasswordCompareErrorMessage")]
		[Localizable(true)]
		[WebSysDefaultValue("CreateUserWizard_DefaultConfirmPasswordCompareErrorMessage")]
		public virtual string ConfirmPasswordCompareErrorMessage
		{
			get
			{
				object obj = this.ViewState["ConfirmPasswordCompareErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultConfirmPasswordCompareErrorMessage");
			}
			set
			{
				this.ViewState["ConfirmPasswordCompareErrorMessage"] = value;
			}
		}

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x0600400A RID: 16394 RVA: 0x0010A048 File Offset: 0x00109048
		// (set) Token: 0x0600400B RID: 16395 RVA: 0x0010A07A File Offset: 0x0010907A
		[WebCategory("Appearance")]
		[WebSysDescription("CreateUserWizard_ConfirmPasswordLabelText")]
		[WebSysDefaultValue("CreateUserWizard_DefaultConfirmPasswordLabelText")]
		[Localizable(true)]
		public virtual string ConfirmPasswordLabelText
		{
			get
			{
				object obj = this.ViewState["ConfirmPasswordLabelText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultConfirmPasswordLabelText");
			}
			set
			{
				this.ViewState["ConfirmPasswordLabelText"] = value;
			}
		}

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x0600400C RID: 16396 RVA: 0x0010A090 File Offset: 0x00109090
		// (set) Token: 0x0600400D RID: 16397 RVA: 0x0010A0C2 File Offset: 0x001090C2
		[Localizable(true)]
		[WebCategory("Validation")]
		[WebSysDefaultValue("CreateUserWizard_DefaultConfirmPasswordRequiredErrorMessage")]
		[WebSysDescription("LoginControls_ConfirmPasswordRequiredErrorMessage")]
		public virtual string ConfirmPasswordRequiredErrorMessage
		{
			get
			{
				object obj = this.ViewState["ConfirmPasswordRequiredErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultConfirmPasswordRequiredErrorMessage");
			}
			set
			{
				this.ViewState["ConfirmPasswordRequiredErrorMessage"] = value;
			}
		}

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x0600400E RID: 16398 RVA: 0x0010A0D8 File Offset: 0x001090D8
		// (set) Token: 0x0600400F RID: 16399 RVA: 0x0010A105 File Offset: 0x00109105
		[WebSysDescription("ChangePassword_ContinueButtonImageUrl")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		[WebCategory("Appearance")]
		[DefaultValue("")]
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

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06004010 RID: 16400 RVA: 0x0010A118 File Offset: 0x00109118
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("CreateUserWizard_ContinueButtonStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebCategory("Styles")]
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

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06004011 RID: 16401 RVA: 0x0010A148 File Offset: 0x00109148
		// (set) Token: 0x06004012 RID: 16402 RVA: 0x0010A17A File Offset: 0x0010917A
		[WebSysDefaultValue("CreateUserWizard_DefaultContinueButtonText")]
		[WebCategory("Appearance")]
		[WebSysDescription("CreateUserWizard_ContinueButtonText")]
		[Localizable(true)]
		public virtual string ContinueButtonText
		{
			get
			{
				object obj = this.ViewState["ContinueButtonText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultContinueButtonText");
			}
			set
			{
				this.ViewState["ContinueButtonText"] = value;
			}
		}

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x06004013 RID: 16403 RVA: 0x0010A190 File Offset: 0x00109190
		// (set) Token: 0x06004014 RID: 16404 RVA: 0x0010A1B9 File Offset: 0x001091B9
		[WebCategory("Appearance")]
		[WebSysDescription("CreateUserWizard_ContinueButtonType")]
		[DefaultValue(ButtonType.Button)]
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
				if (value != this.ContinueButtonType)
				{
					this.ViewState["ContinueButtonType"] = value;
				}
			}
		}

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x06004015 RID: 16405 RVA: 0x0010A1F0 File Offset: 0x001091F0
		// (set) Token: 0x06004016 RID: 16406 RVA: 0x0010A21D File Offset: 0x0010921D
		[WebCategory("Behavior")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Themeable(false)]
		[UrlProperty]
		[WebSysDescription("LoginControls_ContinueDestinationPageUrl")]
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

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x06004017 RID: 16407 RVA: 0x0010A230 File Offset: 0x00109230
		private bool ConvertingToTemplate
		{
			get
			{
				return base.DesignMode && this._convertingToTemplate;
			}
		}

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x06004018 RID: 16408 RVA: 0x0010A242 File Offset: 0x00109242
		[WebCategory("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebSysDescription("CreateUserWizard_CreateUserStep")]
		[Browsable(false)]
		public CreateUserWizardStep CreateUserStep
		{
			get
			{
				this.EnsureChildControls();
				return this._createUserStep;
			}
		}

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x06004019 RID: 16409 RVA: 0x0010A250 File Offset: 0x00109250
		// (set) Token: 0x0600401A RID: 16410 RVA: 0x0010A27D File Offset: 0x0010927D
		[DefaultValue("")]
		[UrlProperty]
		[WebCategory("Appearance")]
		[WebSysDescription("CreateUserWizard_CreateUserButtonImageUrl")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual string CreateUserButtonImageUrl
		{
			get
			{
				object obj = this.ViewState["CreateUserButtonImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["CreateUserButtonImageUrl"] = value;
			}
		}

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x0600401B RID: 16411 RVA: 0x0010A290 File Offset: 0x00109290
		[WebSysDescription("CreateUserWizard_CreateUserButtonStyle")]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public Style CreateUserButtonStyle
		{
			get
			{
				if (this._createUserButtonStyle == null)
				{
					this._createUserButtonStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._createUserButtonStyle).TrackViewState();
					}
				}
				return this._createUserButtonStyle;
			}
		}

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x0600401C RID: 16412 RVA: 0x0010A2C0 File Offset: 0x001092C0
		// (set) Token: 0x0600401D RID: 16413 RVA: 0x0010A2F2 File Offset: 0x001092F2
		[WebSysDefaultValue("CreateUserWizard_DefaultCreateUserButtonText")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		[WebSysDescription("CreateUserWizard_CreateUserButtonText")]
		public virtual string CreateUserButtonText
		{
			get
			{
				object obj = this.ViewState["CreateUserButtonText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultCreateUserButtonText");
			}
			set
			{
				this.ViewState["CreateUserButtonText"] = value;
			}
		}

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x0600401E RID: 16414 RVA: 0x0010A308 File Offset: 0x00109308
		// (set) Token: 0x0600401F RID: 16415 RVA: 0x0010A331 File Offset: 0x00109331
		[DefaultValue(ButtonType.Button)]
		[WebCategory("Appearance")]
		[WebSysDescription("CreateUserWizard_CreateUserButtonType")]
		public virtual ButtonType CreateUserButtonType
		{
			get
			{
				object obj = this.ViewState["CreateUserButtonType"];
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
				if (value != this.CreateUserButtonType)
				{
					this.ViewState["CreateUserButtonType"] = value;
				}
			}
		}

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x06004020 RID: 16416 RVA: 0x0010A368 File Offset: 0x00109368
		private bool DefaultCreateUserStep
		{
			get
			{
				CreateUserWizardStep createUserStep = this.CreateUserStep;
				return createUserStep != null && createUserStep.ContentTemplate == null;
			}
		}

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x06004021 RID: 16417 RVA: 0x0010A38C File Offset: 0x0010938C
		private bool DefaultCompleteStep
		{
			get
			{
				CompleteWizardStep completeStep = this.CompleteStep;
				return completeStep != null && completeStep.ContentTemplate == null;
			}
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x06004022 RID: 16418 RVA: 0x0010A3B0 File Offset: 0x001093B0
		// (set) Token: 0x06004023 RID: 16419 RVA: 0x0010A3D9 File Offset: 0x001093D9
		[WebCategory("Behavior")]
		[DefaultValue(false)]
		[Themeable(false)]
		[WebSysDescription("CreateUserWizard_DisableCreatedUser")]
		public virtual bool DisableCreatedUser
		{
			get
			{
				object obj = this.ViewState["DisableCreatedUser"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["DisableCreatedUser"] = value;
			}
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x06004024 RID: 16420 RVA: 0x0010A3F1 File Offset: 0x001093F1
		// (set) Token: 0x06004025 RID: 16421 RVA: 0x0010A3F9 File Offset: 0x001093F9
		[DefaultValue(false)]
		public override bool DisplaySideBar
		{
			get
			{
				return base.DisplaySideBar;
			}
			set
			{
				base.DisplaySideBar = value;
			}
		}

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x06004026 RID: 16422 RVA: 0x0010A404 File Offset: 0x00109404
		// (set) Token: 0x06004027 RID: 16423 RVA: 0x0010A436 File Offset: 0x00109436
		[Localizable(true)]
		[WebSysDescription("CreateUserWizard_DuplicateEmailErrorMessage")]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("CreateUserWizard_DefaultDuplicateEmailErrorMessage")]
		public virtual string DuplicateEmailErrorMessage
		{
			get
			{
				object obj = this.ViewState["DuplicateEmailErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultDuplicateEmailErrorMessage");
			}
			set
			{
				this.ViewState["DuplicateEmailErrorMessage"] = value;
			}
		}

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x06004028 RID: 16424 RVA: 0x0010A44C File Offset: 0x0010944C
		// (set) Token: 0x06004029 RID: 16425 RVA: 0x0010A47E File Offset: 0x0010947E
		[Localizable(true)]
		[WebSysDefaultValue("CreateUserWizard_DefaultDuplicateUserNameErrorMessage")]
		[WebSysDescription("CreateUserWizard_DuplicateUserNameErrorMessage")]
		[WebCategory("Appearance")]
		public virtual string DuplicateUserNameErrorMessage
		{
			get
			{
				object obj = this.ViewState["DuplicateUserNameErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultDuplicateUserNameErrorMessage");
			}
			set
			{
				this.ViewState["DuplicateUserNameErrorMessage"] = value;
			}
		}

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x0600402A RID: 16426 RVA: 0x0010A494 File Offset: 0x00109494
		// (set) Token: 0x0600402B RID: 16427 RVA: 0x0010A4C1 File Offset: 0x001094C1
		[UrlProperty]
		[WebCategory("Links")]
		[DefaultValue("")]
		[WebSysDescription("LoginControls_EditProfileIconUrl")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
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

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x0600402C RID: 16428 RVA: 0x0010A4D4 File Offset: 0x001094D4
		// (set) Token: 0x0600402D RID: 16429 RVA: 0x0010A501 File Offset: 0x00109501
		[Localizable(true)]
		[WebSysDescription("CreateUserWizard_EditProfileText")]
		[WebCategory("Links")]
		[DefaultValue("")]
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

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x0600402E RID: 16430 RVA: 0x0010A514 File Offset: 0x00109514
		// (set) Token: 0x0600402F RID: 16431 RVA: 0x0010A541 File Offset: 0x00109541
		[WebCategory("Links")]
		[WebSysDescription("CreateUserWizard_EditProfileUrl")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
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

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x06004030 RID: 16432 RVA: 0x0010A554 File Offset: 0x00109554
		// (set) Token: 0x06004031 RID: 16433 RVA: 0x0010A581 File Offset: 0x00109581
		[WebSysDescription("CreateUserWizard_Email")]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		public virtual string Email
		{
			get
			{
				object obj = this.ViewState["Email"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["Email"] = value;
			}
		}

		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x06004032 RID: 16434 RVA: 0x0010A594 File Offset: 0x00109594
		private string EmailInternal
		{
			get
			{
				string email = this.Email;
				if (string.IsNullOrEmpty(email) && this._createUserStepContainer != null)
				{
					ITextControl textControl = (ITextControl)this._createUserStepContainer.EmailTextBox;
					if (textControl != null)
					{
						return textControl.Text;
					}
				}
				return email;
			}
		}

		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x06004033 RID: 16435 RVA: 0x0010A5D4 File Offset: 0x001095D4
		// (set) Token: 0x06004034 RID: 16436 RVA: 0x0010A606 File Offset: 0x00109606
		[WebCategory("Appearance")]
		[WebSysDescription("CreateUserWizard_EmailLabelText")]
		[WebSysDefaultValue("CreateUserWizard_DefaultEmailLabelText")]
		[Localizable(true)]
		public virtual string EmailLabelText
		{
			get
			{
				object obj = this.ViewState["EmailLabelText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultEmailLabelText");
			}
			set
			{
				this.ViewState["EmailLabelText"] = value;
			}
		}

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x06004035 RID: 16437 RVA: 0x0010A61C File Offset: 0x0010961C
		// (set) Token: 0x06004036 RID: 16438 RVA: 0x0010A649 File Offset: 0x00109649
		[WebSysDescription("CreateUserWizard_EmailRegularExpression")]
		[WebSysDefaultValue("")]
		[WebCategory("Validation")]
		public virtual string EmailRegularExpression
		{
			get
			{
				object obj = this.ViewState["EmailRegularExpression"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["EmailRegularExpression"] = value;
			}
		}

		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x06004037 RID: 16439 RVA: 0x0010A65C File Offset: 0x0010965C
		// (set) Token: 0x06004038 RID: 16440 RVA: 0x0010A68E File Offset: 0x0010968E
		[WebCategory("Validation")]
		[WebSysDescription("CreateUserWizard_EmailRegularExpressionErrorMessage")]
		[WebSysDefaultValue("CreateUserWizard_DefaultEmailRegularExpressionErrorMessage")]
		public virtual string EmailRegularExpressionErrorMessage
		{
			get
			{
				object obj = this.ViewState["EmailRegularExpressionErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultEmailRegularExpressionErrorMessage");
			}
			set
			{
				this.ViewState["EmailRegularExpressionErrorMessage"] = value;
			}
		}

		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x06004039 RID: 16441 RVA: 0x0010A6A4 File Offset: 0x001096A4
		// (set) Token: 0x0600403A RID: 16442 RVA: 0x0010A6D6 File Offset: 0x001096D6
		[WebSysDescription("CreateUserWizard_EmailRequiredErrorMessage")]
		[Localizable(true)]
		[WebCategory("Validation")]
		[WebSysDefaultValue("CreateUserWizard_DefaultEmailRequiredErrorMessage")]
		public virtual string EmailRequiredErrorMessage
		{
			get
			{
				object obj = this.ViewState["EmailRequiredErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultEmailRequiredErrorMessage");
			}
			set
			{
				this.ViewState["EmailRequiredErrorMessage"] = value;
			}
		}

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x0600403B RID: 16443 RVA: 0x0010A6EC File Offset: 0x001096EC
		// (set) Token: 0x0600403C RID: 16444 RVA: 0x0010A71E File Offset: 0x0010971E
		[WebSysDescription("CreateUserWizard_UnknownErrorMessage")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("CreateUserWizard_DefaultUnknownErrorMessage")]
		public virtual string UnknownErrorMessage
		{
			get
			{
				object obj = this.ViewState["UnknownErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultUnknownErrorMessage");
			}
			set
			{
				this.ViewState["UnknownErrorMessage"] = value;
			}
		}

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x0600403D RID: 16445 RVA: 0x0010A731 File Offset: 0x00109731
		[DefaultValue(null)]
		[WebSysDescription("CreateUserWizard_ErrorMessageStyle")]
		[WebCategory("Styles")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public TableItemStyle ErrorMessageStyle
		{
			get
			{
				if (this._errorMessageStyle == null)
				{
					this._errorMessageStyle = new ErrorTableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._errorMessageStyle).TrackViewState();
					}
				}
				return this._errorMessageStyle;
			}
		}

		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x0600403E RID: 16446 RVA: 0x0010A760 File Offset: 0x00109760
		// (set) Token: 0x0600403F RID: 16447 RVA: 0x0010A78D File Offset: 0x0010978D
		[WebSysDescription("LoginControls_HelpPageIconUrl")]
		[UrlProperty]
		[WebCategory("Links")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
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

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x06004040 RID: 16448 RVA: 0x0010A7A0 File Offset: 0x001097A0
		// (set) Token: 0x06004041 RID: 16449 RVA: 0x0010A7CD File Offset: 0x001097CD
		[Localizable(true)]
		[DefaultValue("")]
		[WebSysDescription("ChangePassword_HelpPageText")]
		[WebCategory("Links")]
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

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x06004042 RID: 16450 RVA: 0x0010A7E0 File Offset: 0x001097E0
		// (set) Token: 0x06004043 RID: 16451 RVA: 0x0010A80D File Offset: 0x0010980D
		[UrlProperty]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebCategory("Links")]
		[DefaultValue("")]
		[WebSysDescription("LoginControls_HelpPageUrl")]
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

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x06004044 RID: 16452 RVA: 0x0010A820 File Offset: 0x00109820
		[WebCategory("Styles")]
		[NotifyParentProperty(true)]
		[WebSysDescription("WebControl_HyperLinkStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
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

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06004045 RID: 16453 RVA: 0x0010A850 File Offset: 0x00109850
		// (set) Token: 0x06004046 RID: 16454 RVA: 0x0010A87D File Offset: 0x0010987D
		[WebSysDescription("WebControl_InstructionText")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		[DefaultValue("")]
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

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06004047 RID: 16455 RVA: 0x0010A890 File Offset: 0x00109890
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

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x06004048 RID: 16456 RVA: 0x0010A8C0 File Offset: 0x001098C0
		// (set) Token: 0x06004049 RID: 16457 RVA: 0x0010A8F2 File Offset: 0x001098F2
		[Localizable(true)]
		[WebSysDescription("CreateUserWizard_InvalidAnswerErrorMessage")]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("CreateUserWizard_DefaultInvalidAnswerErrorMessage")]
		public virtual string InvalidAnswerErrorMessage
		{
			get
			{
				object obj = this.ViewState["InvalidAnswerErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultInvalidAnswerErrorMessage");
			}
			set
			{
				this.ViewState["InvalidAnswerErrorMessage"] = value;
			}
		}

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x0600404A RID: 16458 RVA: 0x0010A908 File Offset: 0x00109908
		// (set) Token: 0x0600404B RID: 16459 RVA: 0x0010A93A File Offset: 0x0010993A
		[WebSysDescription("CreateUserWizard_InvalidEmailErrorMessage")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("CreateUserWizard_DefaultInvalidEmailErrorMessage")]
		public virtual string InvalidEmailErrorMessage
		{
			get
			{
				object obj = this.ViewState["InvalidEmailErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultInvalidEmailErrorMessage");
			}
			set
			{
				this.ViewState["InvalidEmailErrorMessage"] = value;
			}
		}

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x0600404C RID: 16460 RVA: 0x0010A950 File Offset: 0x00109950
		// (set) Token: 0x0600404D RID: 16461 RVA: 0x0010A982 File Offset: 0x00109982
		[WebSysDefaultValue("CreateUserWizard_DefaultInvalidPasswordErrorMessage")]
		[Localizable(true)]
		[WebSysDescription("CreateUserWizard_InvalidPasswordErrorMessage")]
		[WebCategory("Appearance")]
		public virtual string InvalidPasswordErrorMessage
		{
			get
			{
				object obj = this.ViewState["InvalidPasswordErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultInvalidPasswordErrorMessage");
			}
			set
			{
				this.ViewState["InvalidPasswordErrorMessage"] = value;
			}
		}

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x0600404E RID: 16462 RVA: 0x0010A998 File Offset: 0x00109998
		// (set) Token: 0x0600404F RID: 16463 RVA: 0x0010A9CA File Offset: 0x001099CA
		[Localizable(true)]
		[WebSysDefaultValue("CreateUserWizard_DefaultInvalidQuestionErrorMessage")]
		[WebSysDescription("CreateUserWizard_InvalidQuestionErrorMessage")]
		[WebCategory("Appearance")]
		public virtual string InvalidQuestionErrorMessage
		{
			get
			{
				object obj = this.ViewState["InvalidQuestionErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultInvalidQuestionErrorMessage");
			}
			set
			{
				this.ViewState["InvalidQuestionErrorMessage"] = value;
			}
		}

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x06004050 RID: 16464 RVA: 0x0010A9DD File Offset: 0x001099DD
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("LoginControls_LabelStyle")]
		[WebCategory("Styles")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
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

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x06004051 RID: 16465 RVA: 0x0010AA0C File Offset: 0x00109A0C
		// (set) Token: 0x06004052 RID: 16466 RVA: 0x0010AA35 File Offset: 0x00109A35
		[WebSysDescription("CreateUserWizard_LoginCreatedUser")]
		[Themeable(false)]
		[WebCategory("Behavior")]
		[DefaultValue(true)]
		public virtual bool LoginCreatedUser
		{
			get
			{
				object obj = this.ViewState["LoginCreatedUser"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["LoginCreatedUser"] = value;
			}
		}

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06004053 RID: 16467 RVA: 0x0010AA4D File Offset: 0x00109A4D
		[Themeable(false)]
		[WebCategory("Behavior")]
		[WebSysDescription("CreateUserWizard_MailDefinition")]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
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

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06004054 RID: 16468 RVA: 0x0010AA7C File Offset: 0x00109A7C
		// (set) Token: 0x06004055 RID: 16469 RVA: 0x0010AAA9 File Offset: 0x00109AA9
		[WebSysDescription("MembershipProvider_Name")]
		[WebCategory("Data")]
		[DefaultValue("")]
		[Themeable(false)]
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
				if (this.MembershipProvider != value)
				{
					this.ViewState["MembershipProvider"] = value;
					base.RequiresControlsRecreation();
				}
			}
		}

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x06004056 RID: 16470 RVA: 0x0010AAD0 File Offset: 0x00109AD0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x06004057 RID: 16471 RVA: 0x0010AAE8 File Offset: 0x00109AE8
		private string PasswordInternal
		{
			get
			{
				string password = this.Password;
				if (string.IsNullOrEmpty(password) && !this.AutoGeneratePassword && this._createUserStepContainer != null)
				{
					ITextControl textControl = (ITextControl)this._createUserStepContainer.PasswordTextBox;
					if (textControl != null)
					{
						return textControl.Text;
					}
				}
				return password;
			}
		}

		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x06004058 RID: 16472 RVA: 0x0010AB30 File Offset: 0x00109B30
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[WebSysDescription("CreateUserWizard_PasswordHintStyle")]
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

		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x06004059 RID: 16473 RVA: 0x0010AB60 File Offset: 0x00109B60
		// (set) Token: 0x0600405A RID: 16474 RVA: 0x0010AB8D File Offset: 0x00109B8D
		[WebSysDescription("ChangePassword_PasswordHintText")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("")]
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

		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x0600405B RID: 16475 RVA: 0x0010ABA0 File Offset: 0x00109BA0
		// (set) Token: 0x0600405C RID: 16476 RVA: 0x0010ABD2 File Offset: 0x00109BD2
		[WebSysDefaultValue("LoginControls_DefaultPasswordLabelText")]
		[WebCategory("Appearance")]
		[WebSysDescription("LoginControls_PasswordLabelText")]
		[Localizable(true)]
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

		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x0600405D RID: 16477 RVA: 0x0010ABE8 File Offset: 0x00109BE8
		// (set) Token: 0x0600405E RID: 16478 RVA: 0x0010AC15 File Offset: 0x00109C15
		[WebSysDefaultValue("")]
		[WebCategory("Validation")]
		[WebSysDescription("CreateUserWizard_PasswordRegularExpression")]
		public virtual string PasswordRegularExpression
		{
			get
			{
				object obj = this.ViewState["PasswordRegularExpression"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["PasswordRegularExpression"] = value;
			}
		}

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x0600405F RID: 16479 RVA: 0x0010AC28 File Offset: 0x00109C28
		// (set) Token: 0x06004060 RID: 16480 RVA: 0x0010AC5A File Offset: 0x00109C5A
		[WebSysDescription("CreateUserWizard_PasswordRegularExpressionErrorMessage")]
		[WebCategory("Validation")]
		[WebSysDefaultValue("Password_InvalidPasswordErrorMessage")]
		public virtual string PasswordRegularExpressionErrorMessage
		{
			get
			{
				object obj = this.ViewState["PasswordRegularExpressionErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("Password_InvalidPasswordErrorMessage");
			}
			set
			{
				this.ViewState["PasswordRegularExpressionErrorMessage"] = value;
			}
		}

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x06004061 RID: 16481 RVA: 0x0010AC70 File Offset: 0x00109C70
		// (set) Token: 0x06004062 RID: 16482 RVA: 0x0010ACA2 File Offset: 0x00109CA2
		[WebSysDefaultValue("CreateUserWizard_DefaultPasswordRequiredErrorMessage")]
		[Localizable(true)]
		[WebCategory("Validation")]
		[WebSysDescription("CreateUserWizard_PasswordRequiredErrorMessage")]
		public virtual string PasswordRequiredErrorMessage
		{
			get
			{
				object obj = this.ViewState["PasswordRequiredErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultPasswordRequiredErrorMessage");
			}
			set
			{
				this.ViewState["PasswordRequiredErrorMessage"] = value;
			}
		}

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x06004063 RID: 16483 RVA: 0x0010ACB8 File Offset: 0x00109CB8
		// (set) Token: 0x06004064 RID: 16484 RVA: 0x0010ACE5 File Offset: 0x00109CE5
		[WebCategory("Appearance")]
		[Localizable(true)]
		[Themeable(false)]
		[DefaultValue("")]
		[WebSysDescription("CreateUserWizard_Question")]
		public virtual string Question
		{
			get
			{
				object obj = this.ViewState["Question"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["Question"] = value;
			}
		}

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x06004065 RID: 16485 RVA: 0x0010ACF8 File Offset: 0x00109CF8
		private string QuestionInternal
		{
			get
			{
				string text = this.Question;
				if (string.IsNullOrEmpty(text) && this._createUserStepContainer != null)
				{
					ITextControl textControl = (ITextControl)this._createUserStepContainer.QuestionTextBox;
					if (textControl != null)
					{
						text = textControl.Text;
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					text = null;
				}
				return text;
			}
		}

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x06004066 RID: 16486 RVA: 0x0010AD42 File Offset: 0x00109D42
		[WebSysDescription("CreateUserWizard_QuestionAndAnswerRequired")]
		[WebCategory("Validation")]
		[DefaultValue(true)]
		protected internal bool QuestionAndAnswerRequired
		{
			get
			{
				if (base.DesignMode)
				{
					return this.CreateUserStep == null || this.CreateUserStep.ContentTemplate == null;
				}
				return LoginUtil.GetProvider(this.MembershipProvider).RequiresQuestionAndAnswer;
			}
		}

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x06004067 RID: 16487 RVA: 0x0010AD78 File Offset: 0x00109D78
		// (set) Token: 0x06004068 RID: 16488 RVA: 0x0010ADAA File Offset: 0x00109DAA
		[WebSysDescription("CreateUserWizard_QuestionLabelText")]
		[WebCategory("Appearance")]
		[WebSysDefaultValue("CreateUserWizard_DefaultQuestionLabelText")]
		[Localizable(true)]
		public virtual string QuestionLabelText
		{
			get
			{
				object obj = this.ViewState["QuestionLabelText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultQuestionLabelText");
			}
			set
			{
				this.ViewState["QuestionLabelText"] = value;
			}
		}

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x06004069 RID: 16489 RVA: 0x0010ADC0 File Offset: 0x00109DC0
		// (set) Token: 0x0600406A RID: 16490 RVA: 0x0010ADF2 File Offset: 0x00109DF2
		[WebSysDefaultValue("CreateUserWizard_DefaultQuestionRequiredErrorMessage")]
		[WebSysDescription("CreateUserWizard_QuestionRequiredErrorMessage")]
		[WebCategory("Validation")]
		[Localizable(true)]
		public virtual string QuestionRequiredErrorMessage
		{
			get
			{
				object obj = this.ViewState["QuestionRequiredErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultQuestionRequiredErrorMessage");
			}
			set
			{
				this.ViewState["QuestionRequiredErrorMessage"] = value;
			}
		}

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x0600406B RID: 16491 RVA: 0x0010AE08 File Offset: 0x00109E08
		// (set) Token: 0x0600406C RID: 16492 RVA: 0x0010AE31 File Offset: 0x00109E31
		[WebSysDescription("CreateUserWizard_RequireEmail")]
		[WebCategory("Behavior")]
		[DefaultValue(true)]
		[Themeable(false)]
		public virtual bool RequireEmail
		{
			get
			{
				object obj = this.ViewState["RequireEmail"];
				return obj == null || (bool)obj;
			}
			set
			{
				if (this.RequireEmail != value)
				{
					this.ViewState["RequireEmail"] = value;
				}
			}
		}

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x0600406D RID: 16493 RVA: 0x0010AE52 File Offset: 0x00109E52
		internal override bool ShowCustomNavigationTemplate
		{
			get
			{
				return base.ShowCustomNavigationTemplate || base.ActiveStep == this.CreateUserStep;
			}
		}

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x0600406E RID: 16494 RVA: 0x0010AE6C File Offset: 0x00109E6C
		// (set) Token: 0x0600406F RID: 16495 RVA: 0x0010AE8A File Offset: 0x00109E8A
		[DefaultValue("")]
		public override string SkipLinkText
		{
			get
			{
				string skipLinkTextInternal = base.SkipLinkTextInternal;
				if (skipLinkTextInternal != null)
				{
					return skipLinkTextInternal;
				}
				return string.Empty;
			}
			set
			{
				base.SkipLinkText = value;
			}
		}

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x06004070 RID: 16496 RVA: 0x0010AE93 File Offset: 0x00109E93
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

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x06004071 RID: 16497 RVA: 0x0010AEC1 File Offset: 0x00109EC1
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebCategory("Styles")]
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

		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x06004072 RID: 16498 RVA: 0x0010AEF0 File Offset: 0x00109EF0
		// (set) Token: 0x06004073 RID: 16499 RVA: 0x0010AF1D File Offset: 0x00109F1D
		[DefaultValue("")]
		[WebSysDescription("UserName_InitialValue")]
		[WebCategory("Appearance")]
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

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x06004074 RID: 16500 RVA: 0x0010AF30 File Offset: 0x00109F30
		private string UserNameInternal
		{
			get
			{
				string userName = this.UserName;
				if (string.IsNullOrEmpty(userName) && this._createUserStepContainer != null)
				{
					ITextControl textControl = (ITextControl)this._createUserStepContainer.UserNameTextBox;
					if (textControl != null)
					{
						return textControl.Text;
					}
				}
				return userName;
			}
		}

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x06004075 RID: 16501 RVA: 0x0010AF70 File Offset: 0x00109F70
		// (set) Token: 0x06004076 RID: 16502 RVA: 0x0010AFA2 File Offset: 0x00109FA2
		[WebSysDefaultValue("CreateUserWizard_DefaultUserNameLabelText")]
		[WebCategory("Appearance")]
		[Localizable(true)]
		[WebSysDescription("LoginControls_UserNameLabelText")]
		public virtual string UserNameLabelText
		{
			get
			{
				object obj = this.ViewState["UserNameLabelText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultUserNameLabelText");
			}
			set
			{
				this.ViewState["UserNameLabelText"] = value;
			}
		}

		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x06004077 RID: 16503 RVA: 0x0010AFB8 File Offset: 0x00109FB8
		// (set) Token: 0x06004078 RID: 16504 RVA: 0x0010AFEA File Offset: 0x00109FEA
		[WebSysDescription("ChangePassword_UserNameRequiredErrorMessage")]
		[Localizable(true)]
		[WebCategory("Validation")]
		[WebSysDefaultValue("CreateUserWizard_DefaultUserNameRequiredErrorMessage")]
		public virtual string UserNameRequiredErrorMessage
		{
			get
			{
				object obj = this.ViewState["UserNameRequiredErrorMessage"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("CreateUserWizard_DefaultUserNameRequiredErrorMessage");
			}
			set
			{
				this.ViewState["UserNameRequiredErrorMessage"] = value;
			}
		}

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x06004079 RID: 16505 RVA: 0x0010AFFD File Offset: 0x00109FFD
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebSysDescription("CreateUserWizard_ValidatorTextStyle")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		[WebCategory("Styles")]
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

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x0600407A RID: 16506 RVA: 0x0010B02B File Offset: 0x0010A02B
		internal string ValidationGroup
		{
			get
			{
				if (this._validationGroup == null)
				{
					base.EnsureID();
					this._validationGroup = this.ID;
				}
				return this._validationGroup;
			}
		}

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x0600407B RID: 16507 RVA: 0x0010B04D File Offset: 0x0010A04D
		[Editor("System.Web.UI.Design.WebControls.CreateUserWizardStepCollectionEditor,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public override WizardStepCollection WizardSteps
		{
			get
			{
				return base.WizardSteps;
			}
		}

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x0600407C RID: 16508 RVA: 0x0010B055 File Offset: 0x0010A055
		// (remove) Token: 0x0600407D RID: 16509 RVA: 0x0010B068 File Offset: 0x0010A068
		[WebSysDescription("CreateUserWizard_ContinueButtonClick")]
		[WebCategory("Action")]
		public event EventHandler ContinueButtonClick
		{
			add
			{
				base.Events.AddHandler(CreateUserWizard.EventButtonContinueClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(CreateUserWizard.EventButtonContinueClick, value);
			}
		}

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x0600407E RID: 16510 RVA: 0x0010B07B File Offset: 0x0010A07B
		// (remove) Token: 0x0600407F RID: 16511 RVA: 0x0010B08E File Offset: 0x0010A08E
		[WebCategory("Action")]
		[WebSysDescription("CreateUserWizard_CreatingUser")]
		public event LoginCancelEventHandler CreatingUser
		{
			add
			{
				base.Events.AddHandler(CreateUserWizard.EventCreatingUser, value);
			}
			remove
			{
				base.Events.RemoveHandler(CreateUserWizard.EventCreatingUser, value);
			}
		}

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06004080 RID: 16512 RVA: 0x0010B0A1 File Offset: 0x0010A0A1
		// (remove) Token: 0x06004081 RID: 16513 RVA: 0x0010B0B4 File Offset: 0x0010A0B4
		[WebSysDescription("CreateUserWizard_CreatedUser")]
		[WebCategory("Action")]
		public event EventHandler CreatedUser
		{
			add
			{
				base.Events.AddHandler(CreateUserWizard.EventCreatedUser, value);
			}
			remove
			{
				base.Events.RemoveHandler(CreateUserWizard.EventCreatedUser, value);
			}
		}

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x06004082 RID: 16514 RVA: 0x0010B0C7 File Offset: 0x0010A0C7
		// (remove) Token: 0x06004083 RID: 16515 RVA: 0x0010B0DA File Offset: 0x0010A0DA
		[WebCategory("Action")]
		[WebSysDescription("CreateUserWizard_CreateUserError")]
		public event CreateUserErrorEventHandler CreateUserError
		{
			add
			{
				base.Events.AddHandler(CreateUserWizard.EventCreateUserError, value);
			}
			remove
			{
				base.Events.RemoveHandler(CreateUserWizard.EventCreateUserError, value);
			}
		}

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x06004084 RID: 16516 RVA: 0x0010B0ED File Offset: 0x0010A0ED
		// (remove) Token: 0x06004085 RID: 16517 RVA: 0x0010B100 File Offset: 0x0010A100
		[WebSysDescription("ChangePassword_SendingMail")]
		[WebCategory("Action")]
		public event MailMessageEventHandler SendingMail
		{
			add
			{
				base.Events.AddHandler(CreateUserWizard.EventSendingMail, value);
			}
			remove
			{
				base.Events.RemoveHandler(CreateUserWizard.EventSendingMail, value);
			}
		}

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x06004086 RID: 16518 RVA: 0x0010B113 File Offset: 0x0010A113
		// (remove) Token: 0x06004087 RID: 16519 RVA: 0x0010B126 File Offset: 0x0010A126
		[WebSysDescription("CreateUserWizard_SendMailError")]
		[WebCategory("Action")]
		public event SendMailErrorEventHandler SendMailError
		{
			add
			{
				base.Events.AddHandler(CreateUserWizard.EventSendMailError, value);
			}
			remove
			{
				base.Events.RemoveHandler(CreateUserWizard.EventSendMailError, value);
			}
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x0010B139 File Offset: 0x0010A139
		private void AnswerTextChanged(object source, EventArgs e)
		{
			this.Answer = ((ITextControl)source).Text;
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x0010B14C File Offset: 0x0010A14C
		private void ApplyCommonCreateUserValues()
		{
			if (!string.IsNullOrEmpty(this.UserNameInternal))
			{
				ITextControl textControl = (ITextControl)this._createUserStepContainer.UserNameTextBox;
				if (textControl != null)
				{
					textControl.Text = this.UserNameInternal;
				}
			}
			if (!string.IsNullOrEmpty(this.EmailInternal))
			{
				ITextControl textControl2 = (ITextControl)this._createUserStepContainer.EmailTextBox;
				if (textControl2 != null)
				{
					textControl2.Text = this.EmailInternal;
				}
			}
			if (!string.IsNullOrEmpty(this.QuestionInternal))
			{
				ITextControl textControl3 = (ITextControl)this._createUserStepContainer.QuestionTextBox;
				if (textControl3 != null)
				{
					textControl3.Text = this.QuestionInternal;
				}
			}
			if (!string.IsNullOrEmpty(this.AnswerInternal))
			{
				ITextControl textControl4 = (ITextControl)this._createUserStepContainer.AnswerTextBox;
				if (textControl4 != null)
				{
					textControl4.Text = this.AnswerInternal;
				}
			}
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x0010B20D File Offset: 0x0010A20D
		internal override void ApplyControlProperties()
		{
			this.SetChildProperties();
			if (this.CreateUserStep.CustomNavigationTemplate == null)
			{
				this.SetDefaultCreateUserNavigationTemplateProperties();
			}
			base.ApplyControlProperties();
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x0010B230 File Offset: 0x0010A230
		private void ApplyDefaultCreateUserValues()
		{
			this._createUserStepContainer.UserNameLabel.Text = this.UserNameLabelText;
			WebControl webControl = (WebControl)this._createUserStepContainer.UserNameTextBox;
			webControl.TabIndex = this.TabIndex;
			webControl.AccessKey = this.AccessKey;
			this._createUserStepContainer.PasswordLabel.Text = this.PasswordLabelText;
			WebControl webControl2 = (WebControl)this._createUserStepContainer.PasswordTextBox;
			webControl2.TabIndex = this.TabIndex;
			this._createUserStepContainer.ConfirmPasswordLabel.Text = this.ConfirmPasswordLabelText;
			WebControl webControl3 = (WebControl)this._createUserStepContainer.ConfirmPasswordTextBox;
			webControl3.TabIndex = this.TabIndex;
			if (this._textBoxStyle != null)
			{
				webControl.ApplyStyle(this._textBoxStyle);
				webControl2.ApplyStyle(this._textBoxStyle);
				webControl3.ApplyStyle(this._textBoxStyle);
			}
			LoginUtil.ApplyStyleToLiteral(this._createUserStepContainer.Title, this.CreateUserStep.Title, this.TitleTextStyle, true);
			LoginUtil.ApplyStyleToLiteral(this._createUserStepContainer.InstructionLabel, this.InstructionText, this.InstructionTextStyle, true);
			LoginUtil.ApplyStyleToLiteral(this._createUserStepContainer.UserNameLabel, this.UserNameLabelText, this.LabelStyle, false);
			LoginUtil.ApplyStyleToLiteral(this._createUserStepContainer.PasswordLabel, this.PasswordLabelText, this.LabelStyle, false);
			LoginUtil.ApplyStyleToLiteral(this._createUserStepContainer.ConfirmPasswordLabel, this.ConfirmPasswordLabelText, this.LabelStyle, false);
			if (!string.IsNullOrEmpty(this.PasswordHintText) && !this.AutoGeneratePassword)
			{
				LoginUtil.ApplyStyleToLiteral(this._createUserStepContainer.PasswordHintLabel, this.PasswordHintText, this.PasswordHintStyle, false);
			}
			else
			{
				this._passwordHintTableRow.Visible = false;
			}
			bool flag = true;
			if (this.RequireEmail)
			{
				LoginUtil.ApplyStyleToLiteral(this._createUserStepContainer.EmailLabel, this.EmailLabelText, this.LabelStyle, false);
				WebControl webControl4 = (WebControl)this._createUserStepContainer.EmailTextBox;
				((ITextControl)webControl4).Text = this.Email;
				RequiredFieldValidator emailRequired = this._createUserStepContainer.EmailRequired;
				emailRequired.ToolTip = this.EmailRequiredErrorMessage;
				emailRequired.ErrorMessage = this.EmailRequiredErrorMessage;
				emailRequired.Enabled = flag;
				emailRequired.Visible = flag;
				if (this._validatorTextStyle != null)
				{
					emailRequired.ApplyStyle(this._validatorTextStyle);
				}
				webControl4.TabIndex = this.TabIndex;
				if (this._textBoxStyle != null)
				{
					webControl4.ApplyStyle(this._textBoxStyle);
				}
			}
			else
			{
				this._emailRow.Visible = false;
			}
			RequiredFieldValidator questionRequired = this._createUserStepContainer.QuestionRequired;
			RequiredFieldValidator answerRequired = this._createUserStepContainer.AnswerRequired;
			bool flag2 = flag && this.QuestionAndAnswerRequired;
			questionRequired.Enabled = flag2;
			questionRequired.Visible = flag2;
			answerRequired.Enabled = flag2;
			answerRequired.Visible = flag2;
			if (this.QuestionAndAnswerRequired)
			{
				LoginUtil.ApplyStyleToLiteral(this._createUserStepContainer.QuestionLabel, this.QuestionLabelText, this.LabelStyle, false);
				WebControl webControl5 = (WebControl)this._createUserStepContainer.QuestionTextBox;
				((ITextControl)webControl5).Text = this.Question;
				webControl5.TabIndex = this.TabIndex;
				LoginUtil.ApplyStyleToLiteral(this._createUserStepContainer.AnswerLabel, this.AnswerLabelText, this.LabelStyle, false);
				WebControl webControl6 = (WebControl)this._createUserStepContainer.AnswerTextBox;
				((ITextControl)webControl6).Text = this.Answer;
				webControl6.TabIndex = this.TabIndex;
				if (this._textBoxStyle != null)
				{
					webControl5.ApplyStyle(this._textBoxStyle);
					webControl6.ApplyStyle(this._textBoxStyle);
				}
				questionRequired.ToolTip = this.QuestionRequiredErrorMessage;
				questionRequired.ErrorMessage = this.QuestionRequiredErrorMessage;
				answerRequired.ToolTip = this.AnswerRequiredErrorMessage;
				answerRequired.ErrorMessage = this.AnswerRequiredErrorMessage;
				if (this._validatorTextStyle != null)
				{
					questionRequired.ApplyStyle(this._validatorTextStyle);
					answerRequired.ApplyStyle(this._validatorTextStyle);
				}
			}
			else
			{
				this._questionRow.Visible = false;
				this._answerRow.Visible = false;
			}
			if (this._defaultCreateUserNavigationTemplate != null)
			{
				((Wizard.BaseNavigationTemplateContainer)this.CreateUserStep.CustomNavigationTemplateContainer).NextButton = this._defaultCreateUserNavigationTemplate.CreateUserButton;
				((Wizard.BaseNavigationTemplateContainer)this.CreateUserStep.CustomNavigationTemplateContainer).CancelButton = this._defaultCreateUserNavigationTemplate.CancelButton;
			}
			RequiredFieldValidator passwordRequired = this._createUserStepContainer.PasswordRequired;
			RequiredFieldValidator confirmPasswordRequired = this._createUserStepContainer.ConfirmPasswordRequired;
			CompareValidator passwordCompareValidator = this._createUserStepContainer.PasswordCompareValidator;
			RegularExpressionValidator passwordRegExpValidator = this._createUserStepContainer.PasswordRegExpValidator;
			bool flag3 = flag && !this.AutoGeneratePassword;
			passwordRequired.Enabled = flag3;
			passwordRequired.Visible = flag3;
			confirmPasswordRequired.Enabled = flag3;
			confirmPasswordRequired.Visible = flag3;
			passwordCompareValidator.Enabled = flag3;
			passwordCompareValidator.Visible = flag3;
			bool flag4 = flag3 && this.PasswordRegularExpression.Length > 0;
			passwordRegExpValidator.Enabled = flag4;
			passwordRegExpValidator.Visible = flag4;
			if (!flag)
			{
				this._passwordRegExpRow.Visible = false;
				this._passwordCompareRow.Visible = false;
				this._emailRegExpRow.Visible = false;
			}
			if (this.AutoGeneratePassword)
			{
				this._passwordTableRow.Visible = false;
				this._confirmPasswordTableRow.Visible = false;
				this._passwordRegExpRow.Visible = false;
				this._passwordCompareRow.Visible = false;
			}
			else
			{
				passwordRequired.ErrorMessage = this.PasswordRequiredErrorMessage;
				passwordRequired.ToolTip = this.PasswordRequiredErrorMessage;
				confirmPasswordRequired.ErrorMessage = this.ConfirmPasswordRequiredErrorMessage;
				confirmPasswordRequired.ToolTip = this.ConfirmPasswordRequiredErrorMessage;
				passwordCompareValidator.ErrorMessage = this.ConfirmPasswordCompareErrorMessage;
				if (this._validatorTextStyle != null)
				{
					passwordRequired.ApplyStyle(this._validatorTextStyle);
					confirmPasswordRequired.ApplyStyle(this._validatorTextStyle);
					passwordCompareValidator.ApplyStyle(this._validatorTextStyle);
				}
				if (flag4)
				{
					passwordRegExpValidator.ValidationExpression = this.PasswordRegularExpression;
					passwordRegExpValidator.ErrorMessage = this.PasswordRegularExpressionErrorMessage;
					if (this._validatorTextStyle != null)
					{
						passwordRegExpValidator.ApplyStyle(this._validatorTextStyle);
					}
				}
				else
				{
					this._passwordRegExpRow.Visible = false;
				}
			}
			RequiredFieldValidator userNameRequired = this._createUserStepContainer.UserNameRequired;
			userNameRequired.ErrorMessage = this.UserNameRequiredErrorMessage;
			userNameRequired.ToolTip = this.UserNameRequiredErrorMessage;
			userNameRequired.Enabled = flag;
			userNameRequired.Visible = flag;
			if (this._validatorTextStyle != null)
			{
				userNameRequired.ApplyStyle(this._validatorTextStyle);
			}
			bool flag5 = flag && this.EmailRegularExpression.Length > 0 && this.RequireEmail;
			RegularExpressionValidator emailRegExpValidator = this._createUserStepContainer.EmailRegExpValidator;
			emailRegExpValidator.Enabled = flag5;
			emailRegExpValidator.Visible = flag5;
			if (this.EmailRegularExpression.Length > 0 && this.RequireEmail)
			{
				emailRegExpValidator.ValidationExpression = this.EmailRegularExpression;
				emailRegExpValidator.ErrorMessage = this.EmailRegularExpressionErrorMessage;
				if (this._validatorTextStyle != null)
				{
					emailRegExpValidator.ApplyStyle(this._validatorTextStyle);
				}
			}
			else
			{
				this._emailRegExpRow.Visible = false;
			}
			string helpPageText = this.HelpPageText;
			bool flag6 = helpPageText.Length > 0;
			HyperLink helpPageLink = this._createUserStepContainer.HelpPageLink;
			Image helpPageIcon = this._createUserStepContainer.HelpPageIcon;
			helpPageLink.Visible = flag6;
			if (flag6)
			{
				helpPageLink.Text = helpPageText;
				helpPageLink.NavigateUrl = this.HelpPageUrl;
				helpPageLink.TabIndex = this.TabIndex;
			}
			string helpPageIconUrl = this.HelpPageIconUrl;
			bool flag7 = helpPageIconUrl.Length > 0;
			helpPageIcon.Visible = flag7;
			if (flag7)
			{
				helpPageIcon.ImageUrl = helpPageIconUrl;
				helpPageIcon.AlternateText = helpPageText;
			}
			LoginUtil.SetTableCellVisible(helpPageLink, flag6 || flag7);
			if (this._hyperLinkStyle != null && (flag6 || flag7))
			{
				TableItemStyle tableItemStyle = new TableItemStyle();
				tableItemStyle.CopyFrom(this._hyperLinkStyle);
				tableItemStyle.Font.Reset();
				LoginUtil.SetTableCellStyle(helpPageLink, tableItemStyle);
				helpPageLink.Font.CopyFrom(this._hyperLinkStyle.Font);
				helpPageLink.ForeColor = this._hyperLinkStyle.ForeColor;
			}
			Control errorMessageLabel = this._createUserStepContainer.ErrorMessageLabel;
			if (errorMessageLabel != null)
			{
				if (this._failure && !string.IsNullOrEmpty(this._unknownErrorMessage))
				{
					((ITextControl)errorMessageLabel).Text = this._unknownErrorMessage;
					LoginUtil.SetTableCellStyle(errorMessageLabel, this.ErrorMessageStyle);
					LoginUtil.SetTableCellVisible(errorMessageLabel, true);
					return;
				}
				LoginUtil.SetTableCellVisible(errorMessageLabel, false);
			}
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x0010BA70 File Offset: 0x0010AA70
		private void ApplyCompleteValues()
		{
			LoginUtil.ApplyStyleToLiteral(this._completeStepContainer.SuccessTextLabel, this.CompleteSuccessText, this._completeSuccessTextStyle, true);
			switch (this.ContinueButtonType)
			{
			case ButtonType.Button:
				this._completeStepContainer.ContinueLinkButton.Visible = false;
				this._completeStepContainer.ContinueImageButton.Visible = false;
				this._completeStepContainer.ContinuePushButton.Text = this.ContinueButtonText;
				this._completeStepContainer.ContinuePushButton.ValidationGroup = this.ValidationGroup;
				this._completeStepContainer.ContinuePushButton.TabIndex = this.TabIndex;
				this._completeStepContainer.ContinuePushButton.AccessKey = this.AccessKey;
				break;
			case ButtonType.Image:
				this._completeStepContainer.ContinueLinkButton.Visible = false;
				this._completeStepContainer.ContinuePushButton.Visible = false;
				this._completeStepContainer.ContinueImageButton.ImageUrl = this.ContinueButtonImageUrl;
				this._completeStepContainer.ContinueImageButton.AlternateText = this.ContinueButtonText;
				this._completeStepContainer.ContinueImageButton.ValidationGroup = this.ValidationGroup;
				this._completeStepContainer.ContinueImageButton.TabIndex = this.TabIndex;
				this._completeStepContainer.ContinueImageButton.AccessKey = this.AccessKey;
				break;
			case ButtonType.Link:
				this._completeStepContainer.ContinuePushButton.Visible = false;
				this._completeStepContainer.ContinueImageButton.Visible = false;
				this._completeStepContainer.ContinueLinkButton.Text = this.ContinueButtonText;
				this._completeStepContainer.ContinueLinkButton.ValidationGroup = this.ValidationGroup;
				this._completeStepContainer.ContinueLinkButton.TabIndex = this.TabIndex;
				this._completeStepContainer.ContinueLinkButton.AccessKey = this.AccessKey;
				break;
			}
			if (!base.NavigationButtonStyle.IsEmpty)
			{
				this._completeStepContainer.ContinuePushButton.ApplyStyle(base.NavigationButtonStyle);
				this._completeStepContainer.ContinueImageButton.ApplyStyle(base.NavigationButtonStyle);
				this._completeStepContainer.ContinueLinkButton.ApplyStyle(base.NavigationButtonStyle);
			}
			if (this._continueButtonStyle != null)
			{
				this._completeStepContainer.ContinuePushButton.ApplyStyle(this._continueButtonStyle);
				this._completeStepContainer.ContinueImageButton.ApplyStyle(this._continueButtonStyle);
				this._completeStepContainer.ContinueLinkButton.ApplyStyle(this._continueButtonStyle);
			}
			LoginUtil.ApplyStyleToLiteral(this._completeStepContainer.Title, this.CompleteStep.Title, this._titleTextStyle, true);
			string editProfileText = this.EditProfileText;
			bool flag = editProfileText.Length > 0;
			HyperLink editProfileLink = this._completeStepContainer.EditProfileLink;
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
			string editProfileIconUrl = this.EditProfileIconUrl;
			bool flag2 = editProfileIconUrl.Length > 0;
			Image editProfileIcon = this._completeStepContainer.EditProfileIcon;
			editProfileIcon.Visible = flag2;
			if (flag2)
			{
				editProfileIcon.ImageUrl = editProfileIconUrl;
				editProfileIcon.AlternateText = this.EditProfileText;
			}
			LoginUtil.SetTableCellVisible(editProfileLink, flag || flag2);
			Table layoutTable = ((CreateUserWizard.CompleteStepContainer)this.CompleteStep.ContentTemplateContainer).LayoutTable;
			layoutTable.Height = this.Height;
			layoutTable.Width = this.Width;
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x0010BE20 File Offset: 0x0010AE20
		private bool AttemptCreateUser()
		{
			if (this.Page != null && !this.Page.IsValid)
			{
				return false;
			}
			LoginCancelEventArgs loginCancelEventArgs = new LoginCancelEventArgs();
			this.OnCreatingUser(loginCancelEventArgs);
			if (loginCancelEventArgs.Cancel)
			{
				return false;
			}
			MembershipProvider provider = LoginUtil.GetProvider(this.MembershipProvider);
			if (this.AutoGeneratePassword)
			{
				int num = Math.Max(10, Membership.MinRequiredPasswordLength);
				this._password = Membership.GeneratePassword(num, Membership.MinRequiredNonAlphanumericCharacters);
			}
			MembershipCreateStatus membershipCreateStatus;
			provider.CreateUser(this.UserNameInternal, this.PasswordInternal, this.EmailInternal, this.QuestionInternal, this.AnswerInternal, !this.DisableCreatedUser, null, out membershipCreateStatus);
			if (membershipCreateStatus == MembershipCreateStatus.Success)
			{
				this.OnCreatedUser(EventArgs.Empty);
				if (this._mailDefinition != null && !string.IsNullOrEmpty(this.EmailInternal))
				{
					LoginUtil.SendPasswordMail(this.EmailInternal, this.UserNameInternal, this.PasswordInternal, this.MailDefinition, null, null, new LoginUtil.OnSendingMailDelegate(this.OnSendingMail), new LoginUtil.OnSendMailErrorDelegate(this.OnSendMailError), this);
				}
				this.CreateUserStep.AllowReturnInternal = false;
				if (this.LoginCreatedUser)
				{
					this.AttemptLogin();
				}
				return true;
			}
			this.OnCreateUserError(new CreateUserErrorEventArgs(membershipCreateStatus));
			switch (membershipCreateStatus)
			{
			case MembershipCreateStatus.InvalidPassword:
			{
				string text = this.InvalidPasswordErrorMessage;
				if (!string.IsNullOrEmpty(text))
				{
					text = string.Format(CultureInfo.InvariantCulture, text, new object[] { provider.MinRequiredPasswordLength, provider.MinRequiredNonAlphanumericCharacters });
				}
				this._unknownErrorMessage = text;
				break;
			}
			case MembershipCreateStatus.InvalidQuestion:
				this._unknownErrorMessage = this.InvalidQuestionErrorMessage;
				break;
			case MembershipCreateStatus.InvalidAnswer:
				this._unknownErrorMessage = this.InvalidAnswerErrorMessage;
				break;
			case MembershipCreateStatus.InvalidEmail:
				this._unknownErrorMessage = this.InvalidEmailErrorMessage;
				break;
			case MembershipCreateStatus.DuplicateUserName:
				this._unknownErrorMessage = this.DuplicateUserNameErrorMessage;
				break;
			case MembershipCreateStatus.DuplicateEmail:
				this._unknownErrorMessage = this.DuplicateEmailErrorMessage;
				break;
			default:
				this._unknownErrorMessage = this.UnknownErrorMessage;
				break;
			}
			return false;
		}

		// Token: 0x0600408E RID: 16526 RVA: 0x0010C018 File Offset: 0x0010B018
		private void AttemptLogin()
		{
			MembershipProvider provider = LoginUtil.GetProvider(this.MembershipProvider);
			if (provider.ValidateUser(this.UserName, this.Password))
			{
				FormsAuthentication.SetAuthCookie(this.UserNameInternal, false);
			}
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x0010C051 File Offset: 0x0010B051
		private void ConfirmPasswordTextChanged(object source, EventArgs e)
		{
			if (!this.AutoGeneratePassword)
			{
				this._confirmPassword = ((ITextControl)source).Text;
			}
		}

		// Token: 0x06004090 RID: 16528 RVA: 0x0010C06C File Offset: 0x0010B06C
		protected internal override void CreateChildControls()
		{
			this._createUserStep = null;
			this._completeStep = null;
			base.CreateChildControls();
			this.UpdateValidators();
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x0010C088 File Offset: 0x0010B088
		protected override void CreateControlHierarchy()
		{
			this.EnsureCreateUserSteps();
			base.CreateControlHierarchy();
			IEditableTextControl editableTextControl = this._createUserStepContainer.UserNameTextBox as IEditableTextControl;
			if (editableTextControl != null)
			{
				editableTextControl.TextChanged += this.UserNameTextChanged;
			}
			IEditableTextControl editableTextControl2 = this._createUserStepContainer.EmailTextBox as IEditableTextControl;
			if (editableTextControl2 != null)
			{
				editableTextControl2.TextChanged += this.EmailTextChanged;
			}
			IEditableTextControl editableTextControl3 = this._createUserStepContainer.QuestionTextBox as IEditableTextControl;
			if (editableTextControl3 != null)
			{
				editableTextControl3.TextChanged += this.QuestionTextChanged;
			}
			IEditableTextControl editableTextControl4 = this._createUserStepContainer.AnswerTextBox as IEditableTextControl;
			if (editableTextControl4 != null)
			{
				editableTextControl4.TextChanged += this.AnswerTextChanged;
			}
			IEditableTextControl editableTextControl5 = this._createUserStepContainer.PasswordTextBox as IEditableTextControl;
			if (editableTextControl5 != null)
			{
				editableTextControl5.TextChanged += this.PasswordTextChanged;
			}
			editableTextControl5 = this._createUserStepContainer.ConfirmPasswordTextBox as IEditableTextControl;
			if (editableTextControl5 != null)
			{
				editableTextControl5.TextChanged += this.ConfirmPasswordTextChanged;
			}
			this.ApplyCommonCreateUserValues();
		}

		// Token: 0x06004092 RID: 16530 RVA: 0x0010C191 File Offset: 0x0010B191
		internal override ITemplate CreateDefaultSideBarTemplate()
		{
			return new CreateUserWizard.DefaultSideBarTemplate();
		}

		// Token: 0x06004093 RID: 16531 RVA: 0x0010C198 File Offset: 0x0010B198
		internal override ITemplate CreateDefaultDataListItemTemplate()
		{
			return new CreateUserWizard.DataListItemTemplate();
		}

		// Token: 0x06004094 RID: 16532 RVA: 0x0010C1A0 File Offset: 0x0010B1A0
		private static LabelLiteral CreateLabelLiteral(Control control)
		{
			LabelLiteral labelLiteral = new LabelLiteral(control);
			labelLiteral.PreventAutoID();
			return labelLiteral;
		}

		// Token: 0x06004095 RID: 16533 RVA: 0x0010C1BC File Offset: 0x0010B1BC
		private static Literal CreateLiteral()
		{
			Literal literal = new Literal();
			literal.PreventAutoID();
			return literal;
		}

		// Token: 0x06004096 RID: 16534 RVA: 0x0010C1D8 File Offset: 0x0010B1D8
		private static RequiredFieldValidator CreateRequiredFieldValidator(string id, string validationGroup, TextBox textBox, bool enableValidation)
		{
			return new RequiredFieldValidator
			{
				ID = id,
				ControlToValidate = textBox.ID,
				ValidationGroup = validationGroup,
				Display = ValidatorDisplay.Static,
				Text = SR.GetString("LoginControls_DefaultRequiredFieldValidatorText"),
				Enabled = enableValidation,
				Visible = enableValidation
			};
		}

		// Token: 0x06004097 RID: 16535 RVA: 0x0010C22C File Offset: 0x0010B22C
		private static Table CreateTable()
		{
			Table table = new Table();
			table.Width = Unit.Percentage(100.0);
			table.Height = Unit.Percentage(100.0);
			table.PreventAutoID();
			return table;
		}

		// Token: 0x06004098 RID: 16536 RVA: 0x0010C270 File Offset: 0x0010B270
		private static TableCell CreateTableCell()
		{
			TableCell tableCell = new TableCell();
			tableCell.PreventAutoID();
			return tableCell;
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x0010C28C File Offset: 0x0010B28C
		private static TableRow CreateTableRow()
		{
			TableRow tableRow = new LoginUtil.DisappearingTableRow();
			tableRow.PreventAutoID();
			return tableRow;
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x0010C2A8 File Offset: 0x0010B2A8
		internal override void CreateCustomNavigationTemplates()
		{
			for (int i = 0; i < this.WizardSteps.Count; i++)
			{
				TemplatedWizardStep templatedWizardStep = this.WizardSteps[i] as TemplatedWizardStep;
				if (templatedWizardStep != null)
				{
					string customContainerID = base.GetCustomContainerID(i);
					Wizard.BaseNavigationTemplateContainer baseNavigationTemplateContainer = base.CreateBaseNavigationTemplateContainer(customContainerID);
					if (templatedWizardStep.CustomNavigationTemplate != null)
					{
						templatedWizardStep.CustomNavigationTemplate.InstantiateIn(baseNavigationTemplateContainer);
						templatedWizardStep.CustomNavigationTemplateContainer = baseNavigationTemplateContainer;
						baseNavigationTemplateContainer.SetEnableTheming();
					}
					else if (templatedWizardStep == this.CreateUserStep)
					{
						ITemplate template = new CreateUserWizard.DefaultCreateUserNavigationTemplate(this);
						template.InstantiateIn(baseNavigationTemplateContainer);
						templatedWizardStep.CustomNavigationTemplateContainer = baseNavigationTemplateContainer;
						baseNavigationTemplateContainer.RegisterButtonCommandEvents();
					}
					base.CustomNavigationContainers[templatedWizardStep] = baseNavigationTemplateContainer;
				}
			}
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x0010C34C File Offset: 0x0010B34C
		internal override void DataListItemDataBound(object sender, DataListItemEventArgs e)
		{
			DataListItem item = e.Item;
			if (item.ItemType != ListItemType.Item && item.ItemType != ListItemType.AlternatingItem && item.ItemType != ListItemType.SelectedItem && item.ItemType != ListItemType.EditItem)
			{
				return;
			}
			IButtonControl buttonControl = item.FindControl(Wizard.SideBarButtonID) as IButtonControl;
			if (buttonControl != null)
			{
				base.DataListItemDataBound(sender, e);
				return;
			}
			Label label = item.FindControl("SideBarLabel") as Label;
			if (label != null)
			{
				label.MergeStyle(base.SideBarButtonStyle);
				WizardStepBase wizardStepBase = item.DataItem as WizardStepBase;
				if (wizardStepBase != null)
				{
					base.RegisterSideBarDataListForRender();
					if (wizardStepBase.Title.Length > 0)
					{
						label.Text = wizardStepBase.Title;
						return;
					}
					label.Text = wizardStepBase.ID;
				}
				return;
			}
			if (!base.DesignMode)
			{
				throw new InvalidOperationException(SR.GetString("CreateUserWizard_SideBar_Label_Not_Found", new object[]
				{
					Wizard.DataListID,
					"SideBarLabel"
				}));
			}
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x0010C433 File Offset: 0x0010B433
		private void EmailTextChanged(object source, EventArgs e)
		{
			this.Email = ((ITextControl)source).Text;
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x0010C448 File Offset: 0x0010B448
		private void EnsureCreateUserSteps()
		{
			bool flag = false;
			bool flag2 = false;
			foreach (object obj in this.WizardSteps)
			{
				WizardStepBase wizardStepBase = (WizardStepBase)obj;
				if (wizardStepBase is CreateUserWizardStep)
				{
					if (flag)
					{
						throw new HttpException(SR.GetString("CreateUserWizard_DuplicateCreateUserWizardStep"));
					}
					flag = true;
					this._createUserStep = (CreateUserWizardStep)wizardStepBase;
				}
				else if (wizardStepBase is CompleteWizardStep)
				{
					if (flag2)
					{
						throw new HttpException(SR.GetString("CreateUserWizard_DuplicateCompleteWizardStep"));
					}
					flag2 = true;
					this._completeStep = (CompleteWizardStep)wizardStepBase;
				}
			}
			if (!flag)
			{
				this._createUserStep = new CreateUserWizardStep();
				this._createUserStep.ApplyStyleSheetSkin(this.Page);
				this.WizardSteps.AddAt(0, this._createUserStep);
				this._createUserStep.Active = true;
			}
			if (!flag2)
			{
				this._completeStep = new CompleteWizardStep();
				this._completeStep.ApplyStyleSheetSkin(this.Page);
				this.WizardSteps.Add(this._completeStep);
			}
			if (this.ActiveStepIndex == -1)
			{
				this.ActiveStepIndex = 0;
			}
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x0010C574 File Offset: 0x0010B574
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		protected override IDictionary GetDesignModeState()
		{
			IDictionary designModeState = base.GetDesignModeState();
			WizardStepBase activeStep = base.ActiveStep;
			if (activeStep != null && activeStep == this.CreateUserStep)
			{
				designModeState["CustomNavigationControls"] = ((Wizard.BaseNavigationTemplateContainer)base.CustomNavigationContainers[base.ActiveStep]).Controls;
			}
			Control errorMessageLabel = this._createUserStepContainer.ErrorMessageLabel;
			if (errorMessageLabel != null)
			{
				LoginUtil.SetTableCellVisible(errorMessageLabel, true);
			}
			return designModeState;
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x0010C5D8 File Offset: 0x0010B5D8
		internal override void InstantiateStepContentTemplates()
		{
			foreach (object obj in this.WizardSteps)
			{
				WizardStepBase wizardStepBase = (WizardStepBase)obj;
				if (wizardStepBase == this.CreateUserStep)
				{
					wizardStepBase.Controls.Clear();
					this._createUserStepContainer = new CreateUserWizard.CreateUserStepContainer(this);
					this._createUserStepContainer.ID = "CreateUserStepContainer";
					ITemplate template = ((CreateUserWizardStep)wizardStepBase).ContentTemplate;
					if (template == null)
					{
						template = new CreateUserWizard.DefaultCreateUserContentTemplate(this);
					}
					else
					{
						this._createUserStepContainer.SetEnableTheming();
					}
					template.InstantiateIn(this._createUserStepContainer.InnerCell);
					((CreateUserWizardStep)wizardStepBase).ContentTemplateContainer = this._createUserStepContainer;
					wizardStepBase.Controls.Add(this._createUserStepContainer);
				}
				else if (wizardStepBase == this.CompleteStep)
				{
					wizardStepBase.Controls.Clear();
					this._completeStepContainer = new CreateUserWizard.CompleteStepContainer(this);
					this._completeStepContainer.ID = "CompleteStepContainer";
					ITemplate template2 = ((CompleteWizardStep)wizardStepBase).ContentTemplate;
					if (template2 == null)
					{
						template2 = new CreateUserWizard.DefaultCompleteStepContentTemplate(this);
					}
					else
					{
						this._completeStepContainer.SetEnableTheming();
					}
					template2.InstantiateIn(this._completeStepContainer.InnerCell);
					((CompleteWizardStep)wizardStepBase).ContentTemplateContainer = this._completeStepContainer;
					wizardStepBase.Controls.Add(this._completeStepContainer);
				}
				else if (wizardStepBase is TemplatedWizardStep)
				{
					base.InstantiateStepContentTemplate((TemplatedWizardStep)wizardStepBase);
				}
			}
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x0010C76C File Offset: 0x0010B76C
		protected override void LoadViewState(object savedState)
		{
			if (savedState == null)
			{
				base.LoadViewState(null);
			}
			else
			{
				object[] array = (object[])savedState;
				if (array.Length != 13)
				{
					throw new ArgumentException(SR.GetString("ViewState_InvalidViewState"));
				}
				base.LoadViewState(array[0]);
				if (array[1] != null)
				{
					((IStateManager)this.CreateUserButtonStyle).LoadViewState(array[1]);
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
					((IStateManager)this.ErrorMessageStyle).LoadViewState(array[7]);
				}
				if (array[8] != null)
				{
					((IStateManager)this.PasswordHintStyle).LoadViewState(array[8]);
				}
				if (array[9] != null)
				{
					((IStateManager)this.MailDefinition).LoadViewState(array[9]);
				}
				if (array[10] != null)
				{
					((IStateManager)this.ContinueButtonStyle).LoadViewState(array[10]);
				}
				if (array[11] != null)
				{
					((IStateManager)this.CompleteSuccessTextStyle).LoadViewState(array[11]);
				}
				if (array[12] != null)
				{
					((IStateManager)this.ValidatorTextStyle).LoadViewState(array[12]);
				}
			}
			this.UpdateValidators();
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x0010C8A4 File Offset: 0x0010B8A4
		private void UpdateValidators()
		{
			if (base.DesignMode)
			{
				return;
			}
			if (this.DefaultCreateUserStep && this._createUserStepContainer != null)
			{
				if (this.AutoGeneratePassword)
				{
					BaseValidator confirmPasswordRequired = this._createUserStepContainer.ConfirmPasswordRequired;
					if (confirmPasswordRequired != null)
					{
						this.Page.Validators.Remove(confirmPasswordRequired);
						confirmPasswordRequired.Enabled = false;
					}
					BaseValidator passwordRequired = this._createUserStepContainer.PasswordRequired;
					if (passwordRequired != null)
					{
						this.Page.Validators.Remove(passwordRequired);
						passwordRequired.Enabled = false;
					}
					BaseValidator passwordRegExpValidator = this._createUserStepContainer.PasswordRegExpValidator;
					if (passwordRegExpValidator != null)
					{
						this.Page.Validators.Remove(passwordRegExpValidator);
						passwordRegExpValidator.Enabled = false;
					}
				}
				else if (this.PasswordRegularExpression.Length <= 0)
				{
					BaseValidator passwordRegExpValidator2 = this._createUserStepContainer.PasswordRegExpValidator;
					if (passwordRegExpValidator2 != null)
					{
						if (this.Page != null)
						{
							this.Page.Validators.Remove(passwordRegExpValidator2);
						}
						passwordRegExpValidator2.Enabled = false;
					}
				}
				if (!this.RequireEmail)
				{
					BaseValidator emailRequired = this._createUserStepContainer.EmailRequired;
					if (emailRequired != null)
					{
						if (this.Page != null)
						{
							this.Page.Validators.Remove(emailRequired);
						}
						emailRequired.Enabled = false;
					}
					BaseValidator emailRegExpValidator = this._createUserStepContainer.EmailRegExpValidator;
					if (emailRegExpValidator != null)
					{
						if (this.Page != null)
						{
							this.Page.Validators.Remove(emailRegExpValidator);
						}
						emailRegExpValidator.Enabled = false;
					}
				}
				else if (this.EmailRegularExpression.Length <= 0)
				{
					BaseValidator emailRegExpValidator2 = this._createUserStepContainer.EmailRegExpValidator;
					if (emailRegExpValidator2 != null)
					{
						if (this.Page != null)
						{
							this.Page.Validators.Remove(emailRegExpValidator2);
						}
						emailRegExpValidator2.Enabled = false;
					}
				}
				if (!this.QuestionAndAnswerRequired)
				{
					BaseValidator questionRequired = this._createUserStepContainer.QuestionRequired;
					if (questionRequired != null)
					{
						if (this.Page != null)
						{
							this.Page.Validators.Remove(questionRequired);
						}
						questionRequired.Enabled = false;
					}
					BaseValidator answerRequired = this._createUserStepContainer.AnswerRequired;
					if (answerRequired != null)
					{
						if (this.Page != null)
						{
							this.Page.Validators.Remove(answerRequired);
						}
						answerRequired.Enabled = false;
					}
				}
			}
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x0010CAAC File Offset: 0x0010BAAC
		protected override bool OnBubbleEvent(object source, EventArgs e)
		{
			if (e is CommandEventArgs)
			{
				CommandEventArgs commandEventArgs = (CommandEventArgs)e;
				if (commandEventArgs.CommandName.Equals(CreateUserWizard.ContinueButtonCommandName, StringComparison.CurrentCultureIgnoreCase))
				{
					this.OnContinueButtonClick(EventArgs.Empty);
					return true;
				}
			}
			return base.OnBubbleEvent(source, e);
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x0010CAF0 File Offset: 0x0010BAF0
		protected virtual void OnContinueButtonClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[CreateUserWizard.EventButtonContinueClick];
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

		// Token: 0x060040A4 RID: 16548 RVA: 0x0010CB48 File Offset: 0x0010BB48
		protected virtual void OnCreatedUser(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[CreateUserWizard.EventCreatedUser];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x0010CB78 File Offset: 0x0010BB78
		protected virtual void OnCreateUserError(CreateUserErrorEventArgs e)
		{
			CreateUserErrorEventHandler createUserErrorEventHandler = (CreateUserErrorEventHandler)base.Events[CreateUserWizard.EventCreateUserError];
			if (createUserErrorEventHandler != null)
			{
				createUserErrorEventHandler(this, e);
			}
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x0010CBA8 File Offset: 0x0010BBA8
		protected virtual void OnCreatingUser(LoginCancelEventArgs e)
		{
			LoginCancelEventHandler loginCancelEventHandler = (LoginCancelEventHandler)base.Events[CreateUserWizard.EventCreatingUser];
			if (loginCancelEventHandler != null)
			{
				loginCancelEventHandler(this, e);
			}
		}

		// Token: 0x060040A7 RID: 16551 RVA: 0x0010CBD8 File Offset: 0x0010BBD8
		protected override void OnNextButtonClick(WizardNavigationEventArgs e)
		{
			if (this.WizardSteps[e.CurrentStepIndex] == this._createUserStep)
			{
				e.Cancel = this.Page != null && !this.Page.IsValid;
				if (!e.Cancel)
				{
					this._failure = !this.AttemptCreateUser();
					if (this._failure)
					{
						e.Cancel = true;
						ITextControl textControl = (ITextControl)this._createUserStepContainer.ErrorMessageLabel;
						if (textControl != null && !string.IsNullOrEmpty(this._unknownErrorMessage))
						{
							textControl.Text = this._unknownErrorMessage;
							if (textControl is Control)
							{
								((Control)textControl).Visible = true;
							}
						}
					}
				}
			}
			base.OnNextButtonClick(e);
		}

		// Token: 0x060040A8 RID: 16552 RVA: 0x0010CC90 File Offset: 0x0010BC90
		protected internal override void OnPreRender(EventArgs e)
		{
			this.EnsureCreateUserSteps();
			base.OnPreRender(e);
			string membershipProvider = this.MembershipProvider;
			if (!string.IsNullOrEmpty(membershipProvider) && Membership.Providers[membershipProvider] == null)
			{
				throw new HttpException(SR.GetString("WebControl_CantFindProvider"));
			}
		}

		// Token: 0x060040A9 RID: 16553 RVA: 0x0010CCD8 File Offset: 0x0010BCD8
		protected virtual void OnSendingMail(MailMessageEventArgs e)
		{
			MailMessageEventHandler mailMessageEventHandler = (MailMessageEventHandler)base.Events[CreateUserWizard.EventSendingMail];
			if (mailMessageEventHandler != null)
			{
				mailMessageEventHandler(this, e);
			}
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x0010CD08 File Offset: 0x0010BD08
		protected virtual void OnSendMailError(SendMailErrorEventArgs e)
		{
			SendMailErrorEventHandler sendMailErrorEventHandler = (SendMailErrorEventHandler)base.Events[CreateUserWizard.EventSendMailError];
			if (sendMailErrorEventHandler != null)
			{
				sendMailErrorEventHandler(this, e);
			}
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x0010CD36 File Offset: 0x0010BD36
		private void PasswordTextChanged(object source, EventArgs e)
		{
			if (!this.AutoGeneratePassword)
			{
				this._password = ((ITextControl)source).Text;
			}
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x0010CD51 File Offset: 0x0010BD51
		private void QuestionTextChanged(object source, EventArgs e)
		{
			this.Question = ((ITextControl)source).Text;
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x0010CD64 File Offset: 0x0010BD64
		protected override object SaveViewState()
		{
			object[] array = new object[]
			{
				base.SaveViewState(),
				(this._createUserButtonStyle != null) ? ((IStateManager)this._createUserButtonStyle).SaveViewState() : null,
				(this._labelStyle != null) ? ((IStateManager)this._labelStyle).SaveViewState() : null,
				(this._textBoxStyle != null) ? ((IStateManager)this._textBoxStyle).SaveViewState() : null,
				(this._hyperLinkStyle != null) ? ((IStateManager)this._hyperLinkStyle).SaveViewState() : null,
				(this._instructionTextStyle != null) ? ((IStateManager)this._instructionTextStyle).SaveViewState() : null,
				(this._titleTextStyle != null) ? ((IStateManager)this._titleTextStyle).SaveViewState() : null,
				(this._errorMessageStyle != null) ? ((IStateManager)this._errorMessageStyle).SaveViewState() : null,
				(this._passwordHintStyle != null) ? ((IStateManager)this._passwordHintStyle).SaveViewState() : null,
				(this._mailDefinition != null) ? ((IStateManager)this._mailDefinition).SaveViewState() : null,
				(this._continueButtonStyle != null) ? ((IStateManager)this._continueButtonStyle).SaveViewState() : null,
				(this._completeSuccessTextStyle != null) ? ((IStateManager)this._completeSuccessTextStyle).SaveViewState() : null,
				(this._validatorTextStyle != null) ? ((IStateManager)this._validatorTextStyle).SaveViewState() : null
			};
			for (int i = 0; i < 13; i++)
			{
				if (array[i] != null)
				{
					return array;
				}
			}
			return null;
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x0010CEC8 File Offset: 0x0010BEC8
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
			}
		}

		// Token: 0x060040AF RID: 16559 RVA: 0x0010CEF4 File Offset: 0x0010BEF4
		internal void SetChildProperties()
		{
			this.ApplyCommonCreateUserValues();
			if (this.DefaultCreateUserStep)
			{
				this.ApplyDefaultCreateUserValues();
			}
			if (this.DefaultCompleteStep)
			{
				this.ApplyCompleteValues();
			}
			Control errorMessageLabel = this._createUserStepContainer.ErrorMessageLabel;
			if (errorMessageLabel != null)
			{
				if (this._failure && !string.IsNullOrEmpty(this._unknownErrorMessage))
				{
					((ITextControl)errorMessageLabel).Text = this._unknownErrorMessage;
					errorMessageLabel.Visible = true;
					return;
				}
				errorMessageLabel.Visible = false;
			}
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x0010CF68 File Offset: 0x0010BF68
		private void SetDefaultCreateUserNavigationTemplateProperties()
		{
			WebControl webControl = (WebControl)this._defaultCreateUserNavigationTemplate.CreateUserButton;
			WebControl webControl2 = (WebControl)this._defaultCreateUserNavigationTemplate.PreviousButton;
			WebControl webControl3 = (WebControl)this._defaultCreateUserNavigationTemplate.CancelButton;
			this._defaultCreateUserNavigationTemplate.ApplyLayoutStyleToInnerCells(base.NavigationStyle);
			this.WizardSteps.IndexOf(this.CreateUserStep);
			((IButtonControl)webControl).CausesValidation = true;
			((IButtonControl)webControl).Text = this.CreateUserButtonText;
			((IButtonControl)webControl).ValidationGroup = this.ValidationGroup;
			((IButtonControl)webControl2).CausesValidation = false;
			((IButtonControl)webControl2).Text = this.StepPreviousButtonText;
			((IButtonControl)webControl3).Text = this.CancelButtonText;
			if (this._createUserButtonStyle != null)
			{
				webControl.ApplyStyle(this._createUserButtonStyle);
			}
			webControl.ControlStyle.MergeWith(base.NavigationButtonStyle);
			webControl.TabIndex = this.TabIndex;
			webControl.Visible = true;
			if (webControl is ImageButton)
			{
				((ImageButton)webControl).ImageUrl = this.CreateUserButtonImageUrl;
				((ImageButton)webControl).AlternateText = this.CreateUserButtonText;
			}
			webControl2.ApplyStyle(base.StepPreviousButtonStyle);
			webControl2.ControlStyle.MergeWith(base.NavigationButtonStyle);
			webControl2.TabIndex = this.TabIndex;
			int previousStepIndex = base.GetPreviousStepIndex(false);
			if (previousStepIndex != -1 && this.WizardSteps[previousStepIndex].AllowReturn)
			{
				webControl2.Visible = true;
			}
			else
			{
				webControl2.Parent.Visible = false;
			}
			if (webControl2 is ImageButton)
			{
				((ImageButton)webControl2).AlternateText = this.StepPreviousButtonText;
				((ImageButton)webControl2).ImageUrl = this.StepPreviousButtonImageUrl;
			}
			if (this.DisplayCancelButton)
			{
				webControl3.ApplyStyle(base.CancelButtonStyle);
				webControl3.ControlStyle.MergeWith(base.NavigationButtonStyle);
				webControl3.TabIndex = this.TabIndex;
				webControl3.Visible = true;
				if (webControl3 is ImageButton)
				{
					((ImageButton)webControl3).ImageUrl = this.CancelButtonImageUrl;
					((ImageButton)webControl3).AlternateText = this.CancelButtonText;
					return;
				}
			}
			else
			{
				webControl3.Parent.Visible = false;
			}
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x0010D180 File Offset: 0x0010C180
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this._createUserButtonStyle != null)
			{
				((IStateManager)this._createUserButtonStyle).TrackViewState();
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
			if (this._errorMessageStyle != null)
			{
				((IStateManager)this._errorMessageStyle).TrackViewState();
			}
			if (this._passwordHintStyle != null)
			{
				((IStateManager)this._passwordHintStyle).TrackViewState();
			}
			if (this._mailDefinition != null)
			{
				((IStateManager)this._mailDefinition).TrackViewState();
			}
			if (this._continueButtonStyle != null)
			{
				((IStateManager)this._continueButtonStyle).TrackViewState();
			}
			if (this._completeSuccessTextStyle != null)
			{
				((IStateManager)this._completeSuccessTextStyle).TrackViewState();
			}
			if (this._validatorTextStyle != null)
			{
				((IStateManager)this._validatorTextStyle).TrackViewState();
			}
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x0010D277 File Offset: 0x0010C277
		private void UserNameTextChanged(object source, EventArgs e)
		{
			this.UserName = ((ITextControl)source).Text;
		}

		// Token: 0x04002824 RID: 10276
		private const string _userNameReplacementKey = "<%\\s*UserName\\s*%>";

		// Token: 0x04002825 RID: 10277
		private const string _passwordReplacementKey = "<%\\s*Password\\s*%>";

		// Token: 0x04002826 RID: 10278
		private const int _viewStateArrayLength = 13;

		// Token: 0x04002827 RID: 10279
		private const string _createUserNavigationTemplateName = "CreateUserNavigationTemplate";

		// Token: 0x04002828 RID: 10280
		private const string _userNameID = "UserName";

		// Token: 0x04002829 RID: 10281
		private const string _passwordID = "Password";

		// Token: 0x0400282A RID: 10282
		private const string _confirmPasswordID = "ConfirmPassword";

		// Token: 0x0400282B RID: 10283
		private const string _errorMessageID = "ErrorMessage";

		// Token: 0x0400282C RID: 10284
		private const string _emailID = "Email";

		// Token: 0x0400282D RID: 10285
		private const string _questionID = "Question";

		// Token: 0x0400282E RID: 10286
		private const string _answerID = "Answer";

		// Token: 0x0400282F RID: 10287
		private const string _userNameRequiredID = "UserNameRequired";

		// Token: 0x04002830 RID: 10288
		private const string _passwordRequiredID = "PasswordRequired";

		// Token: 0x04002831 RID: 10289
		private const string _confirmPasswordRequiredID = "ConfirmPasswordRequired";

		// Token: 0x04002832 RID: 10290
		private const string _passwordRegExpID = "PasswordRegExp";

		// Token: 0x04002833 RID: 10291
		private const string _emailRegExpID = "EmailRegExp";

		// Token: 0x04002834 RID: 10292
		private const string _emailRequiredID = "EmailRequired";

		// Token: 0x04002835 RID: 10293
		private const string _questionRequiredID = "QuestionRequired";

		// Token: 0x04002836 RID: 10294
		private const string _answerRequiredID = "AnswerRequired";

		// Token: 0x04002837 RID: 10295
		private const string _passwordCompareID = "PasswordCompare";

		// Token: 0x04002838 RID: 10296
		private const string _continueButtonID = "ContinueButton";

		// Token: 0x04002839 RID: 10297
		private const string _helpLinkID = "HelpLink";

		// Token: 0x0400283A RID: 10298
		private const string _editProfileLinkID = "EditProfileLink";

		// Token: 0x0400283B RID: 10299
		private const string _createUserStepContainerID = "CreateUserStepContainer";

		// Token: 0x0400283C RID: 10300
		private const string _completeStepContainerID = "CompleteStepContainer";

		// Token: 0x0400283D RID: 10301
		private const string _sideBarLabelID = "SideBarLabel";

		// Token: 0x0400283E RID: 10302
		private const ValidatorDisplay _requiredFieldValidatorDisplay = ValidatorDisplay.Static;

		// Token: 0x0400283F RID: 10303
		private const ValidatorDisplay _compareFieldValidatorDisplay = ValidatorDisplay.Dynamic;

		// Token: 0x04002840 RID: 10304
		private const ValidatorDisplay _regexpFieldValidatorDisplay = ValidatorDisplay.Dynamic;

		// Token: 0x04002841 RID: 10305
		public static readonly string ContinueButtonCommandName = "Continue";

		// Token: 0x04002842 RID: 10306
		private string _password;

		// Token: 0x04002843 RID: 10307
		private string _confirmPassword;

		// Token: 0x04002844 RID: 10308
		private string _answer;

		// Token: 0x04002845 RID: 10309
		private string _unknownErrorMessage;

		// Token: 0x04002846 RID: 10310
		private string _validationGroup;

		// Token: 0x04002847 RID: 10311
		private CreateUserWizardStep _createUserStep;

		// Token: 0x04002848 RID: 10312
		private CompleteWizardStep _completeStep;

		// Token: 0x04002849 RID: 10313
		private CreateUserWizard.CreateUserStepContainer _createUserStepContainer;

		// Token: 0x0400284A RID: 10314
		private CreateUserWizard.CompleteStepContainer _completeStepContainer;

		// Token: 0x0400284B RID: 10315
		private bool _failure;

		// Token: 0x0400284C RID: 10316
		private bool _convertingToTemplate;

		// Token: 0x0400284D RID: 10317
		private CreateUserWizard.DefaultCreateUserNavigationTemplate _defaultCreateUserNavigationTemplate;

		// Token: 0x0400284E RID: 10318
		private Style _createUserButtonStyle;

		// Token: 0x0400284F RID: 10319
		private TableItemStyle _labelStyle;

		// Token: 0x04002850 RID: 10320
		private Style _textBoxStyle;

		// Token: 0x04002851 RID: 10321
		private TableItemStyle _hyperLinkStyle;

		// Token: 0x04002852 RID: 10322
		private TableItemStyle _instructionTextStyle;

		// Token: 0x04002853 RID: 10323
		private TableItemStyle _titleTextStyle;

		// Token: 0x04002854 RID: 10324
		private TableItemStyle _errorMessageStyle;

		// Token: 0x04002855 RID: 10325
		private TableItemStyle _passwordHintStyle;

		// Token: 0x04002856 RID: 10326
		private Style _continueButtonStyle;

		// Token: 0x04002857 RID: 10327
		private TableItemStyle _completeSuccessTextStyle;

		// Token: 0x04002858 RID: 10328
		private Style _validatorTextStyle;

		// Token: 0x04002859 RID: 10329
		private MailDefinition _mailDefinition;

		// Token: 0x0400285A RID: 10330
		private static readonly object EventCreatingUser = new object();

		// Token: 0x0400285B RID: 10331
		private static readonly object EventCreateUserError = new object();

		// Token: 0x0400285C RID: 10332
		private static readonly object EventCreatedUser = new object();

		// Token: 0x0400285D RID: 10333
		private static readonly object EventButtonContinueClick = new object();

		// Token: 0x0400285E RID: 10334
		private static readonly object EventCancelClick = new object();

		// Token: 0x0400285F RID: 10335
		private static readonly object EventSendingMail = new object();

		// Token: 0x04002860 RID: 10336
		private static readonly object EventSendMailError = new object();

		// Token: 0x04002861 RID: 10337
		private TableRow _passwordHintTableRow;

		// Token: 0x04002862 RID: 10338
		private TableRow _questionRow;

		// Token: 0x04002863 RID: 10339
		private TableRow _answerRow;

		// Token: 0x04002864 RID: 10340
		private TableRow _emailRow;

		// Token: 0x04002865 RID: 10341
		private TableRow _passwordCompareRow;

		// Token: 0x04002866 RID: 10342
		private TableRow _passwordRegExpRow;

		// Token: 0x04002867 RID: 10343
		private TableRow _emailRegExpRow;

		// Token: 0x04002868 RID: 10344
		private TableRow _passwordTableRow;

		// Token: 0x04002869 RID: 10345
		private TableRow _confirmPasswordTableRow;

		// Token: 0x02000520 RID: 1312
		internal sealed class DefaultCompleteStepContentTemplate : ITemplate
		{
			// Token: 0x060040B4 RID: 16564 RVA: 0x0010D2E9 File Offset: 0x0010C2E9
			internal DefaultCompleteStepContentTemplate(CreateUserWizard wizard)
			{
				this._wizard = wizard;
			}

			// Token: 0x060040B5 RID: 16565 RVA: 0x0010D2F8 File Offset: 0x0010C2F8
			private void ConstructControls(CreateUserWizard.CompleteStepContainer container)
			{
				container.Title = CreateUserWizard.CreateLiteral();
				container.SuccessTextLabel = CreateUserWizard.CreateLiteral();
				container.EditProfileLink = new HyperLink();
				container.EditProfileLink.ID = "EditProfileLink";
				container.EditProfileIcon = new Image();
				container.EditProfileIcon.PreventAutoID();
				LinkButton linkButton = new LinkButton();
				linkButton.ID = "ContinueButtonLinkButton";
				linkButton.CommandName = CreateUserWizard.ContinueButtonCommandName;
				linkButton.CausesValidation = false;
				ImageButton imageButton = new ImageButton();
				imageButton.ID = "ContinueButtonImageButton";
				imageButton.CommandName = CreateUserWizard.ContinueButtonCommandName;
				imageButton.CausesValidation = false;
				Button button = new Button();
				button.ID = "ContinueButtonButton";
				button.CommandName = CreateUserWizard.ContinueButtonCommandName;
				button.CausesValidation = false;
				container.ContinueLinkButton = linkButton;
				container.ContinuePushButton = button;
				container.ContinueImageButton = imageButton;
			}

			// Token: 0x060040B6 RID: 16566 RVA: 0x0010D3CC File Offset: 0x0010C3CC
			private void LayoutControls(CreateUserWizard.CompleteStepContainer container)
			{
				Table table = CreateUserWizard.CreateTable();
				table.EnableViewState = false;
				TableRow tableRow = CreateUserWizard.CreateTableRow();
				TableCell tableCell = CreateUserWizard.CreateTableCell();
				tableCell.ColumnSpan = 2;
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(container.Title);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.Controls.Add(container.SuccessTextLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.ColumnSpan = 2;
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				tableCell.Controls.Add(container.ContinuePushButton);
				tableCell.Controls.Add(container.ContinueLinkButton);
				tableCell.Controls.Add(container.ContinueImageButton);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.ColumnSpan = 2;
				tableCell.Controls.Add(container.EditProfileIcon);
				tableCell.Controls.Add(container.EditProfileLink);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				container.LayoutTable = table;
				container.InnerCell.Controls.Add(table);
			}

			// Token: 0x060040B7 RID: 16567 RVA: 0x0010D530 File Offset: 0x0010C530
			void ITemplate.InstantiateIn(Control container)
			{
				CreateUserWizard.CompleteStepContainer completeStepContainer = (CreateUserWizard.CompleteStepContainer)container.Parent.Parent.Parent;
				this.ConstructControls(completeStepContainer);
				this.LayoutControls(completeStepContainer);
			}

			// Token: 0x0400286A RID: 10346
			private CreateUserWizard _wizard;
		}

		// Token: 0x02000521 RID: 1313
		internal sealed class DefaultCreateUserContentTemplate : ITemplate
		{
			// Token: 0x060040B8 RID: 16568 RVA: 0x0010D561 File Offset: 0x0010C561
			internal DefaultCreateUserContentTemplate(CreateUserWizard wizard)
			{
				this._wizard = wizard;
			}

			// Token: 0x060040B9 RID: 16569 RVA: 0x0010D570 File Offset: 0x0010C570
			private void ConstructControls(CreateUserWizard.CreateUserStepContainer container)
			{
				string validationGroup = this._wizard.ValidationGroup;
				container.Title = CreateUserWizard.CreateLiteral();
				container.InstructionLabel = CreateUserWizard.CreateLiteral();
				container.PasswordHintLabel = CreateUserWizard.CreateLiteral();
				TextBox textBox = new TextBox();
				textBox.ID = "UserName";
				container.UserNameTextBox = textBox;
				TextBox textBox2 = new TextBox();
				textBox2.ID = "Password";
				textBox2.TextMode = TextBoxMode.Password;
				container.PasswordTextBox = textBox2;
				TextBox textBox3 = new TextBox();
				textBox3.ID = "ConfirmPassword";
				textBox3.TextMode = TextBoxMode.Password;
				container.ConfirmPasswordTextBox = textBox3;
				bool flag = true;
				container.UserNameRequired = CreateUserWizard.CreateRequiredFieldValidator("UserNameRequired", validationGroup, textBox, flag);
				container.UserNameLabel = CreateUserWizard.CreateLabelLiteral(textBox);
				container.PasswordLabel = CreateUserWizard.CreateLabelLiteral(textBox2);
				container.ConfirmPasswordLabel = CreateUserWizard.CreateLabelLiteral(textBox3);
				Image image = new Image();
				image.PreventAutoID();
				container.HelpPageIcon = image;
				container.HelpPageLink = new HyperLink
				{
					ID = "HelpLink"
				};
				container.ErrorMessageLabel = new Literal
				{
					ID = "ErrorMessage"
				};
				TextBox textBox4 = new TextBox();
				textBox4.ID = "Email";
				container.EmailRequired = CreateUserWizard.CreateRequiredFieldValidator("EmailRequired", validationGroup, textBox4, flag);
				container.EmailTextBox = textBox4;
				container.EmailLabel = CreateUserWizard.CreateLabelLiteral(textBox4);
				container.EmailRegExpValidator = new RegularExpressionValidator
				{
					ID = "EmailRegExp",
					ControlToValidate = "Email",
					ErrorMessage = this._wizard.EmailRegularExpressionErrorMessage,
					ValidationExpression = this._wizard.EmailRegularExpression,
					ValidationGroup = validationGroup,
					Display = ValidatorDisplay.Dynamic,
					Enabled = flag,
					Visible = flag
				};
				container.PasswordRequired = CreateUserWizard.CreateRequiredFieldValidator("PasswordRequired", validationGroup, textBox2, flag);
				container.ConfirmPasswordRequired = CreateUserWizard.CreateRequiredFieldValidator("ConfirmPasswordRequired", validationGroup, textBox3, flag);
				container.PasswordRegExpValidator = new RegularExpressionValidator
				{
					ID = "PasswordRegExp",
					ControlToValidate = "Password",
					ErrorMessage = this._wizard.PasswordRegularExpressionErrorMessage,
					ValidationExpression = this._wizard.PasswordRegularExpression,
					ValidationGroup = validationGroup,
					Display = ValidatorDisplay.Dynamic,
					Enabled = flag,
					Visible = flag
				};
				container.PasswordCompareValidator = new CompareValidator
				{
					ID = "PasswordCompare",
					ControlToValidate = "ConfirmPassword",
					ControlToCompare = "Password",
					Operator = ValidationCompareOperator.Equal,
					ErrorMessage = this._wizard.ConfirmPasswordCompareErrorMessage,
					ValidationGroup = validationGroup,
					Display = ValidatorDisplay.Dynamic,
					Enabled = flag,
					Visible = flag
				};
				TextBox textBox5 = new TextBox();
				textBox5.ID = "Question";
				container.QuestionTextBox = textBox5;
				TextBox textBox6 = new TextBox();
				textBox6.ID = "Answer";
				container.AnswerTextBox = textBox6;
				container.QuestionRequired = CreateUserWizard.CreateRequiredFieldValidator("QuestionRequired", validationGroup, textBox5, flag);
				container.AnswerRequired = CreateUserWizard.CreateRequiredFieldValidator("AnswerRequired", validationGroup, textBox6, flag);
				container.QuestionLabel = CreateUserWizard.CreateLabelLiteral(textBox5);
				container.AnswerLabel = CreateUserWizard.CreateLabelLiteral(textBox6);
			}

			// Token: 0x060040BA RID: 16570 RVA: 0x0010D8B0 File Offset: 0x0010C8B0
			private void LayoutControls(CreateUserWizard.CreateUserStepContainer container)
			{
				Table table = CreateUserWizard.CreateTable();
				table.EnableViewState = false;
				TableRow tableRow = CreateUserWizard.CreateTableRow();
				TableCell tableCell = CreateUserWizard.CreateTableCell();
				tableCell.ColumnSpan = 2;
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(container.Title);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				tableRow.PreventAutoID();
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.ColumnSpan = 2;
				tableCell.Controls.Add(container.InstructionLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				if (this._wizard.ConvertingToTemplate)
				{
					container.UserNameLabel.RenderAsLabel = true;
				}
				tableCell.Controls.Add(container.UserNameLabel);
				tableRow.Cells.Add(tableCell);
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.Controls.Add(container.UserNameTextBox);
				tableCell.Controls.Add(container.UserNameRequired);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				this._wizard._passwordTableRow = tableRow;
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				if (this._wizard.ConvertingToTemplate)
				{
					container.PasswordLabel.RenderAsLabel = true;
				}
				tableCell.Controls.Add(container.PasswordLabel);
				tableRow.Cells.Add(tableCell);
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.Controls.Add(container.PasswordTextBox);
				if (!this._wizard.AutoGeneratePassword)
				{
					tableCell.Controls.Add(container.PasswordRequired);
				}
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				this._wizard._passwordHintTableRow = tableRow;
				tableCell = CreateUserWizard.CreateTableCell();
				tableRow.Cells.Add(tableCell);
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.Controls.Add(container.PasswordHintLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				this._wizard._confirmPasswordTableRow = tableRow;
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				if (this._wizard.ConvertingToTemplate)
				{
					container.ConfirmPasswordLabel.RenderAsLabel = true;
				}
				tableCell.Controls.Add(container.ConfirmPasswordLabel);
				tableRow.Cells.Add(tableCell);
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.Controls.Add(container.ConfirmPasswordTextBox);
				if (!this._wizard.AutoGeneratePassword)
				{
					tableCell.Controls.Add(container.ConfirmPasswordRequired);
				}
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				this._wizard._emailRow = tableRow;
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				tableCell.Controls.Add(container.EmailLabel);
				if (this._wizard.ConvertingToTemplate)
				{
					container.EmailLabel.RenderAsLabel = true;
				}
				tableRow.Cells.Add(tableCell);
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.Controls.Add(container.EmailTextBox);
				tableCell.Controls.Add(container.EmailRequired);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				this._wizard._questionRow = tableRow;
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				tableCell.Controls.Add(container.QuestionLabel);
				if (this._wizard.ConvertingToTemplate)
				{
					container.QuestionLabel.RenderAsLabel = true;
				}
				tableRow.Cells.Add(tableCell);
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.Controls.Add(container.QuestionTextBox);
				tableCell.Controls.Add(container.QuestionRequired);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				this._wizard._answerRow = tableRow;
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				tableCell.Controls.Add(container.AnswerLabel);
				if (this._wizard.ConvertingToTemplate)
				{
					container.AnswerLabel.RenderAsLabel = true;
				}
				tableRow.Cells.Add(tableCell);
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.Controls.Add(container.AnswerTextBox);
				tableCell.Controls.Add(container.AnswerRequired);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				this._wizard._passwordCompareRow = tableRow;
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.ColumnSpan = 2;
				tableCell.Controls.Add(container.PasswordCompareValidator);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				this._wizard._passwordRegExpRow = tableRow;
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.ColumnSpan = 2;
				tableCell.Controls.Add(container.PasswordRegExpValidator);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				this._wizard._emailRegExpRow = tableRow;
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.ColumnSpan = 2;
				tableCell.Controls.Add(container.EmailRegExpValidator);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.ColumnSpan = 2;
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(container.ErrorMessageLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = CreateUserWizard.CreateTableRow();
				tableCell = CreateUserWizard.CreateTableCell();
				tableCell.ColumnSpan = 2;
				tableCell.Controls.Add(container.HelpPageIcon);
				tableCell.Controls.Add(container.HelpPageLink);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				container.InnerCell.Controls.Add(table);
			}

			// Token: 0x060040BB RID: 16571 RVA: 0x0010DEE8 File Offset: 0x0010CEE8
			void ITemplate.InstantiateIn(Control container)
			{
				CreateUserWizard.CreateUserStepContainer createUserStepContainer = (CreateUserWizard.CreateUserStepContainer)container.Parent.Parent.Parent;
				this.ConstructControls(createUserStepContainer);
				this.LayoutControls(createUserStepContainer);
			}

			// Token: 0x0400286B RID: 10347
			private CreateUserWizard _wizard;
		}

		// Token: 0x02000522 RID: 1314
		internal sealed class DefaultCreateUserNavigationTemplate : ITemplate
		{
			// Token: 0x060040BC RID: 16572 RVA: 0x0010DF19 File Offset: 0x0010CF19
			internal DefaultCreateUserNavigationTemplate(CreateUserWizard wizard)
			{
				this._wizard = wizard;
			}

			// Token: 0x060040BD RID: 16573 RVA: 0x0010DF28 File Offset: 0x0010CF28
			internal void ApplyLayoutStyleToInnerCells(TableItemStyle tableItemStyle)
			{
				for (int i = 0; i < this._innerCells.Length; i++)
				{
					if (tableItemStyle.IsSet(65536))
					{
						this._innerCells[i].HorizontalAlign = tableItemStyle.HorizontalAlign;
					}
					if (tableItemStyle.IsSet(131072))
					{
						this._innerCells[i].VerticalAlign = tableItemStyle.VerticalAlign;
					}
				}
			}

			// Token: 0x060040BE RID: 16574 RVA: 0x0010DF88 File Offset: 0x0010CF88
			void ITemplate.InstantiateIn(Control container)
			{
				this._wizard._defaultCreateUserNavigationTemplate = this;
				container.EnableViewState = false;
				Table table = CreateUserWizard.CreateTable();
				table.CellSpacing = 5;
				table.CellPadding = 5;
				container.Controls.Add(table);
				TableRow tableRow = new TableRow();
				this._row = tableRow;
				tableRow.PreventAutoID();
				tableRow.HorizontalAlign = HorizontalAlign.Right;
				table.Rows.Add(tableRow);
				this._buttons = new IButtonControl[3][];
				this._buttons[0] = new IButtonControl[3];
				this._buttons[1] = new IButtonControl[3];
				this._buttons[2] = new IButtonControl[3];
				this._innerCells = new TableCell[3];
				this._innerCells[0] = this.CreateButtonControl(this._buttons[0], this._wizard.ValidationGroup, Wizard.StepPreviousButtonID, false, Wizard.MovePreviousCommandName);
				this._innerCells[1] = this.CreateButtonControl(this._buttons[1], this._wizard.ValidationGroup, Wizard.StepNextButtonID, true, Wizard.MoveNextCommandName);
				this._innerCells[2] = this.CreateButtonControl(this._buttons[2], this._wizard.ValidationGroup, Wizard.CancelButtonID, false, Wizard.CancelCommandName);
			}

			// Token: 0x060040BF RID: 16575 RVA: 0x0010E0B5 File Offset: 0x0010D0B5
			private void OnPreRender(object source, EventArgs e)
			{
				((ImageButton)source).Visible = false;
			}

			// Token: 0x060040C0 RID: 16576 RVA: 0x0010E0C4 File Offset: 0x0010D0C4
			private TableCell CreateButtonControl(IButtonControl[] buttons, string validationGroup, string id, bool causesValidation, string commandName)
			{
				LinkButton linkButton = new LinkButton();
				linkButton.CausesValidation = causesValidation;
				linkButton.ID = id + "LinkButton";
				linkButton.Visible = false;
				linkButton.CommandName = commandName;
				linkButton.ValidationGroup = validationGroup;
				buttons[0] = linkButton;
				ImageButton imageButton = new ImageButton();
				imageButton.CausesValidation = causesValidation;
				imageButton.ID = id + "ImageButton";
				imageButton.Visible = !this._wizard.DesignMode;
				imageButton.CommandName = commandName;
				imageButton.ValidationGroup = validationGroup;
				imageButton.PreRender += this.OnPreRender;
				buttons[1] = imageButton;
				Button button = new Button();
				button.CausesValidation = causesValidation;
				button.ID = id + "Button";
				button.Visible = false;
				button.CommandName = commandName;
				button.ValidationGroup = validationGroup;
				buttons[2] = button;
				TableCell tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Right;
				this._row.Cells.Add(tableCell);
				tableCell.Controls.Add(linkButton);
				tableCell.Controls.Add(imageButton);
				tableCell.Controls.Add(button);
				return tableCell;
			}

			// Token: 0x17000F85 RID: 3973
			// (get) Token: 0x060040C1 RID: 16577 RVA: 0x0010E1DF File Offset: 0x0010D1DF
			internal IButtonControl PreviousButton
			{
				get
				{
					return this.GetButtonBasedOnType(0, this._wizard.StepPreviousButtonType);
				}
			}

			// Token: 0x17000F86 RID: 3974
			// (get) Token: 0x060040C2 RID: 16578 RVA: 0x0010E1F3 File Offset: 0x0010D1F3
			internal IButtonControl CreateUserButton
			{
				get
				{
					return this.GetButtonBasedOnType(1, this._wizard.CreateUserButtonType);
				}
			}

			// Token: 0x17000F87 RID: 3975
			// (get) Token: 0x060040C3 RID: 16579 RVA: 0x0010E207 File Offset: 0x0010D207
			internal IButtonControl CancelButton
			{
				get
				{
					return this.GetButtonBasedOnType(2, this._wizard.CancelButtonType);
				}
			}

			// Token: 0x060040C4 RID: 16580 RVA: 0x0010E21C File Offset: 0x0010D21C
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

			// Token: 0x0400286C RID: 10348
			private CreateUserWizard _wizard;

			// Token: 0x0400286D RID: 10349
			private TableRow _row;

			// Token: 0x0400286E RID: 10350
			private IButtonControl[][] _buttons;

			// Token: 0x0400286F RID: 10351
			private TableCell[] _innerCells;
		}

		// Token: 0x02000523 RID: 1315
		private sealed class DataListItemTemplate : ITemplate
		{
			// Token: 0x060040C5 RID: 16581 RVA: 0x0010E264 File Offset: 0x0010D264
			public void InstantiateIn(Control container)
			{
				Label label = new Label();
				label.PreventAutoID();
				label.ID = "SideBarLabel";
				container.Controls.Add(label);
			}
		}

		// Token: 0x02000524 RID: 1316
		private sealed class DefaultSideBarTemplate : ITemplate
		{
			// Token: 0x060040C7 RID: 16583 RVA: 0x0010E29C File Offset: 0x0010D29C
			public void InstantiateIn(Control container)
			{
				DataList dataList = new DataList();
				dataList.ID = Wizard.DataListID;
				container.Controls.Add(dataList);
				dataList.SelectedItemStyle.Font.Bold = true;
				dataList.ItemTemplate = new CreateUserWizard.DataListItemTemplate();
			}
		}

		// Token: 0x02000525 RID: 1317
		internal sealed class CreateUserStepContainer : Wizard.BaseContentTemplateContainer
		{
			// Token: 0x060040C9 RID: 16585 RVA: 0x0010E2EA File Offset: 0x0010D2EA
			internal CreateUserStepContainer(CreateUserWizard wizard)
				: base(wizard)
			{
				this._createUserWizard = wizard;
			}

			// Token: 0x17000F88 RID: 3976
			// (get) Token: 0x060040CA RID: 16586 RVA: 0x0010E2FA File Offset: 0x0010D2FA
			// (set) Token: 0x060040CB RID: 16587 RVA: 0x0010E302 File Offset: 0x0010D302
			internal LabelLiteral AnswerLabel
			{
				get
				{
					return this._answerLabel;
				}
				set
				{
					this._answerLabel = value;
				}
			}

			// Token: 0x17000F89 RID: 3977
			// (get) Token: 0x060040CC RID: 16588 RVA: 0x0010E30B File Offset: 0x0010D30B
			// (set) Token: 0x060040CD RID: 16589 RVA: 0x0010E313 File Offset: 0x0010D313
			internal RequiredFieldValidator AnswerRequired
			{
				get
				{
					return this._answerRequired;
				}
				set
				{
					this._answerRequired = value;
				}
			}

			// Token: 0x17000F8A RID: 3978
			// (get) Token: 0x060040CE RID: 16590 RVA: 0x0010E31C File Offset: 0x0010D31C
			// (set) Token: 0x060040CF RID: 16591 RVA: 0x0010E397 File Offset: 0x0010D397
			internal Control AnswerTextBox
			{
				get
				{
					if (this._answerTextBox != null)
					{
						return this._answerTextBox;
					}
					Control control = this.FindControl("Answer");
					if (control is IEditableTextControl)
					{
						return control;
					}
					if (!this._createUserWizard.DesignMode && this._createUserWizard.QuestionAndAnswerRequired)
					{
						throw new HttpException(SR.GetString("CreateUserWizard_NoAnswerTextBox", new object[]
						{
							this._createUserWizard.ID,
							"Answer"
						}));
					}
					return null;
				}
				set
				{
					this._answerTextBox = value;
				}
			}

			// Token: 0x17000F8B RID: 3979
			// (get) Token: 0x060040D0 RID: 16592 RVA: 0x0010E3A0 File Offset: 0x0010D3A0
			// (set) Token: 0x060040D1 RID: 16593 RVA: 0x0010E3A8 File Offset: 0x0010D3A8
			internal LabelLiteral ConfirmPasswordLabel
			{
				get
				{
					return this._confirmPasswordLabel;
				}
				set
				{
					this._confirmPasswordLabel = value;
				}
			}

			// Token: 0x17000F8C RID: 3980
			// (get) Token: 0x060040D2 RID: 16594 RVA: 0x0010E3B1 File Offset: 0x0010D3B1
			// (set) Token: 0x060040D3 RID: 16595 RVA: 0x0010E3B9 File Offset: 0x0010D3B9
			internal RequiredFieldValidator ConfirmPasswordRequired
			{
				get
				{
					return this._confirmPasswordRequired;
				}
				set
				{
					this._confirmPasswordRequired = value;
				}
			}

			// Token: 0x17000F8D RID: 3981
			// (get) Token: 0x060040D4 RID: 16596 RVA: 0x0010E3C4 File Offset: 0x0010D3C4
			// (set) Token: 0x060040D5 RID: 16597 RVA: 0x0010E3F7 File Offset: 0x0010D3F7
			internal Control ConfirmPasswordTextBox
			{
				get
				{
					if (this._confirmPasswordTextBox != null)
					{
						return this._confirmPasswordTextBox;
					}
					Control control = this.FindControl("ConfirmPassword");
					if (control is IEditableTextControl)
					{
						return control;
					}
					return null;
				}
				set
				{
					this._confirmPasswordTextBox = value;
				}
			}

			// Token: 0x17000F8E RID: 3982
			// (get) Token: 0x060040D6 RID: 16598 RVA: 0x0010E400 File Offset: 0x0010D400
			// (set) Token: 0x060040D7 RID: 16599 RVA: 0x0010E408 File Offset: 0x0010D408
			internal LabelLiteral EmailLabel
			{
				get
				{
					return this._emailLabel;
				}
				set
				{
					this._emailLabel = value;
				}
			}

			// Token: 0x17000F8F RID: 3983
			// (get) Token: 0x060040D8 RID: 16600 RVA: 0x0010E411 File Offset: 0x0010D411
			// (set) Token: 0x060040D9 RID: 16601 RVA: 0x0010E419 File Offset: 0x0010D419
			internal RegularExpressionValidator EmailRegExpValidator
			{
				get
				{
					return this._emailRegExpValidator;
				}
				set
				{
					this._emailRegExpValidator = value;
				}
			}

			// Token: 0x17000F90 RID: 3984
			// (get) Token: 0x060040DA RID: 16602 RVA: 0x0010E422 File Offset: 0x0010D422
			// (set) Token: 0x060040DB RID: 16603 RVA: 0x0010E42A File Offset: 0x0010D42A
			internal RequiredFieldValidator EmailRequired
			{
				get
				{
					return this._emailRequired;
				}
				set
				{
					this._emailRequired = value;
				}
			}

			// Token: 0x17000F91 RID: 3985
			// (get) Token: 0x060040DC RID: 16604 RVA: 0x0010E434 File Offset: 0x0010D434
			// (set) Token: 0x060040DD RID: 16605 RVA: 0x0010E4AF File Offset: 0x0010D4AF
			internal Control EmailTextBox
			{
				get
				{
					if (this._emailTextBox != null)
					{
						return this._emailTextBox;
					}
					Control control = this.FindControl("Email");
					if (control is IEditableTextControl)
					{
						return control;
					}
					if (!this._createUserWizard.DesignMode && this._createUserWizard.RequireEmail)
					{
						throw new HttpException(SR.GetString("CreateUserWizard_NoEmailTextBox", new object[]
						{
							this._createUserWizard.ID,
							"Email"
						}));
					}
					return null;
				}
				set
				{
					this._emailTextBox = value;
				}
			}

			// Token: 0x17000F92 RID: 3986
			// (get) Token: 0x060040DE RID: 16606 RVA: 0x0010E4B8 File Offset: 0x0010D4B8
			// (set) Token: 0x060040DF RID: 16607 RVA: 0x0010E4C0 File Offset: 0x0010D4C0
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

			// Token: 0x17000F93 RID: 3987
			// (get) Token: 0x060040E0 RID: 16608 RVA: 0x0010E4CC File Offset: 0x0010D4CC
			// (set) Token: 0x060040E1 RID: 16609 RVA: 0x0010E501 File Offset: 0x0010D501
			internal Control ErrorMessageLabel
			{
				get
				{
					if (this._unknownErrorMessageLabel != null)
					{
						return this._unknownErrorMessageLabel;
					}
					Control control = this.FindControl("ErrorMessage");
					if (!(control is ITextControl))
					{
						return null;
					}
					return control;
				}
				set
				{
					this._unknownErrorMessageLabel = value;
				}
			}

			// Token: 0x17000F94 RID: 3988
			// (get) Token: 0x060040E2 RID: 16610 RVA: 0x0010E50A File Offset: 0x0010D50A
			// (set) Token: 0x060040E3 RID: 16611 RVA: 0x0010E512 File Offset: 0x0010D512
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

			// Token: 0x17000F95 RID: 3989
			// (get) Token: 0x060040E4 RID: 16612 RVA: 0x0010E51B File Offset: 0x0010D51B
			// (set) Token: 0x060040E5 RID: 16613 RVA: 0x0010E523 File Offset: 0x0010D523
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

			// Token: 0x17000F96 RID: 3990
			// (get) Token: 0x060040E6 RID: 16614 RVA: 0x0010E52C File Offset: 0x0010D52C
			// (set) Token: 0x060040E7 RID: 16615 RVA: 0x0010E534 File Offset: 0x0010D534
			internal Literal InstructionLabel
			{
				get
				{
					return this._instructionLabel;
				}
				set
				{
					this._instructionLabel = value;
				}
			}

			// Token: 0x17000F97 RID: 3991
			// (get) Token: 0x060040E8 RID: 16616 RVA: 0x0010E53D File Offset: 0x0010D53D
			// (set) Token: 0x060040E9 RID: 16617 RVA: 0x0010E545 File Offset: 0x0010D545
			internal CompareValidator PasswordCompareValidator
			{
				get
				{
					return this._passwordCompareValidator;
				}
				set
				{
					this._passwordCompareValidator = value;
				}
			}

			// Token: 0x17000F98 RID: 3992
			// (get) Token: 0x060040EA RID: 16618 RVA: 0x0010E54E File Offset: 0x0010D54E
			// (set) Token: 0x060040EB RID: 16619 RVA: 0x0010E556 File Offset: 0x0010D556
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

			// Token: 0x17000F99 RID: 3993
			// (get) Token: 0x060040EC RID: 16620 RVA: 0x0010E55F File Offset: 0x0010D55F
			// (set) Token: 0x060040ED RID: 16621 RVA: 0x0010E567 File Offset: 0x0010D567
			internal RegularExpressionValidator PasswordRegExpValidator
			{
				get
				{
					return this._passwordRegExpValidator;
				}
				set
				{
					this._passwordRegExpValidator = value;
				}
			}

			// Token: 0x17000F9A RID: 3994
			// (get) Token: 0x060040EE RID: 16622 RVA: 0x0010E570 File Offset: 0x0010D570
			// (set) Token: 0x060040EF RID: 16623 RVA: 0x0010E578 File Offset: 0x0010D578
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

			// Token: 0x17000F9B RID: 3995
			// (get) Token: 0x060040F0 RID: 16624 RVA: 0x0010E584 File Offset: 0x0010D584
			// (set) Token: 0x060040F1 RID: 16625 RVA: 0x0010E5FF File Offset: 0x0010D5FF
			internal Control PasswordTextBox
			{
				get
				{
					if (this._passwordTextBox != null)
					{
						return this._passwordTextBox;
					}
					Control control = this.FindControl("Password");
					if (control is IEditableTextControl)
					{
						return control;
					}
					if (!this._createUserWizard.DesignMode && !this._createUserWizard.AutoGeneratePassword)
					{
						throw new HttpException(SR.GetString("CreateUserWizard_NoPasswordTextBox", new object[]
						{
							this._createUserWizard.ID,
							"Password"
						}));
					}
					return null;
				}
				set
				{
					this._passwordTextBox = value;
				}
			}

			// Token: 0x17000F9C RID: 3996
			// (get) Token: 0x060040F2 RID: 16626 RVA: 0x0010E608 File Offset: 0x0010D608
			// (set) Token: 0x060040F3 RID: 16627 RVA: 0x0010E610 File Offset: 0x0010D610
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

			// Token: 0x17000F9D RID: 3997
			// (get) Token: 0x060040F4 RID: 16628 RVA: 0x0010E619 File Offset: 0x0010D619
			// (set) Token: 0x060040F5 RID: 16629 RVA: 0x0010E621 File Offset: 0x0010D621
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

			// Token: 0x17000F9E RID: 3998
			// (get) Token: 0x060040F6 RID: 16630 RVA: 0x0010E62A File Offset: 0x0010D62A
			// (set) Token: 0x060040F7 RID: 16631 RVA: 0x0010E632 File Offset: 0x0010D632
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

			// Token: 0x17000F9F RID: 3999
			// (get) Token: 0x060040F8 RID: 16632 RVA: 0x0010E63B File Offset: 0x0010D63B
			// (set) Token: 0x060040F9 RID: 16633 RVA: 0x0010E643 File Offset: 0x0010D643
			internal LabelLiteral QuestionLabel
			{
				get
				{
					return this._questionLabel;
				}
				set
				{
					this._questionLabel = value;
				}
			}

			// Token: 0x17000FA0 RID: 4000
			// (get) Token: 0x060040FA RID: 16634 RVA: 0x0010E64C File Offset: 0x0010D64C
			// (set) Token: 0x060040FB RID: 16635 RVA: 0x0010E654 File Offset: 0x0010D654
			internal RequiredFieldValidator QuestionRequired
			{
				get
				{
					return this._questionRequired;
				}
				set
				{
					this._questionRequired = value;
				}
			}

			// Token: 0x17000FA1 RID: 4001
			// (get) Token: 0x060040FC RID: 16636 RVA: 0x0010E660 File Offset: 0x0010D660
			// (set) Token: 0x060040FD RID: 16637 RVA: 0x0010E6DB File Offset: 0x0010D6DB
			internal Control QuestionTextBox
			{
				get
				{
					if (this._questionTextBox != null)
					{
						return this._questionTextBox;
					}
					Control control = this.FindControl("Question");
					if (control is IEditableTextControl)
					{
						return control;
					}
					if (!this._createUserWizard.DesignMode && this._createUserWizard.QuestionAndAnswerRequired)
					{
						throw new HttpException(SR.GetString("CreateUserWizard_NoQuestionTextBox", new object[]
						{
							this._createUserWizard.ID,
							"Question"
						}));
					}
					return null;
				}
				set
				{
					this._questionTextBox = value;
				}
			}

			// Token: 0x17000FA2 RID: 4002
			// (get) Token: 0x060040FE RID: 16638 RVA: 0x0010E6E4 File Offset: 0x0010D6E4
			// (set) Token: 0x060040FF RID: 16639 RVA: 0x0010E752 File Offset: 0x0010D752
			internal Control UserNameTextBox
			{
				get
				{
					if (this._userNameTextBox != null)
					{
						return this._userNameTextBox;
					}
					Control control = this.FindControl("UserName");
					if (control is IEditableTextControl)
					{
						return control;
					}
					if (!this._createUserWizard.DesignMode)
					{
						throw new HttpException(SR.GetString("CreateUserWizard_NoUserNameTextBox", new object[]
						{
							this._createUserWizard.ID,
							"UserName"
						}));
					}
					return null;
				}
				set
				{
					this._userNameTextBox = value;
				}
			}

			// Token: 0x04002870 RID: 10352
			private CreateUserWizard _createUserWizard;

			// Token: 0x04002871 RID: 10353
			private Literal _title;

			// Token: 0x04002872 RID: 10354
			private Literal _instructionLabel;

			// Token: 0x04002873 RID: 10355
			private LabelLiteral _userNameLabel;

			// Token: 0x04002874 RID: 10356
			private LabelLiteral _passwordLabel;

			// Token: 0x04002875 RID: 10357
			private LabelLiteral _confirmPasswordLabel;

			// Token: 0x04002876 RID: 10358
			private LabelLiteral _emailLabel;

			// Token: 0x04002877 RID: 10359
			private LabelLiteral _questionLabel;

			// Token: 0x04002878 RID: 10360
			private LabelLiteral _answerLabel;

			// Token: 0x04002879 RID: 10361
			private Literal _passwordHintLabel;

			// Token: 0x0400287A RID: 10362
			private Control _userNameTextBox;

			// Token: 0x0400287B RID: 10363
			private Control _passwordTextBox;

			// Token: 0x0400287C RID: 10364
			private Control _confirmPasswordTextBox;

			// Token: 0x0400287D RID: 10365
			private Control _emailTextBox;

			// Token: 0x0400287E RID: 10366
			private Control _questionTextBox;

			// Token: 0x0400287F RID: 10367
			private Control _answerTextBox;

			// Token: 0x04002880 RID: 10368
			private Control _unknownErrorMessageLabel;

			// Token: 0x04002881 RID: 10369
			private RequiredFieldValidator _userNameRequired;

			// Token: 0x04002882 RID: 10370
			private RequiredFieldValidator _passwordRequired;

			// Token: 0x04002883 RID: 10371
			private RequiredFieldValidator _confirmPasswordRequired;

			// Token: 0x04002884 RID: 10372
			private RequiredFieldValidator _questionRequired;

			// Token: 0x04002885 RID: 10373
			private RequiredFieldValidator _answerRequired;

			// Token: 0x04002886 RID: 10374
			private RequiredFieldValidator _emailRequired;

			// Token: 0x04002887 RID: 10375
			private CompareValidator _passwordCompareValidator;

			// Token: 0x04002888 RID: 10376
			private RegularExpressionValidator _passwordRegExpValidator;

			// Token: 0x04002889 RID: 10377
			private RegularExpressionValidator _emailRegExpValidator;

			// Token: 0x0400288A RID: 10378
			private Image _helpPageIcon;

			// Token: 0x0400288B RID: 10379
			private HyperLink _helpPageLink;
		}

		// Token: 0x02000526 RID: 1318
		internal sealed class CompleteStepContainer : Wizard.BaseContentTemplateContainer
		{
			// Token: 0x06004100 RID: 16640 RVA: 0x0010E75B File Offset: 0x0010D75B
			internal CompleteStepContainer(CreateUserWizard wizard)
				: base(wizard)
			{
				this._createUserWizard = wizard;
			}

			// Token: 0x17000FA3 RID: 4003
			// (get) Token: 0x06004101 RID: 16641 RVA: 0x0010E76B File Offset: 0x0010D76B
			// (set) Token: 0x06004102 RID: 16642 RVA: 0x0010E773 File Offset: 0x0010D773
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

			// Token: 0x17000FA4 RID: 4004
			// (get) Token: 0x06004103 RID: 16643 RVA: 0x0010E77C File Offset: 0x0010D77C
			// (set) Token: 0x06004104 RID: 16644 RVA: 0x0010E784 File Offset: 0x0010D784
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

			// Token: 0x17000FA5 RID: 4005
			// (get) Token: 0x06004105 RID: 16645 RVA: 0x0010E78D File Offset: 0x0010D78D
			// (set) Token: 0x06004106 RID: 16646 RVA: 0x0010E795 File Offset: 0x0010D795
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

			// Token: 0x17000FA6 RID: 4006
			// (get) Token: 0x06004107 RID: 16647 RVA: 0x0010E79E File Offset: 0x0010D79E
			// (set) Token: 0x06004108 RID: 16648 RVA: 0x0010E7A6 File Offset: 0x0010D7A6
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

			// Token: 0x17000FA7 RID: 4007
			// (get) Token: 0x06004109 RID: 16649 RVA: 0x0010E7AF File Offset: 0x0010D7AF
			// (set) Token: 0x0600410A RID: 16650 RVA: 0x0010E7B7 File Offset: 0x0010D7B7
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

			// Token: 0x17000FA8 RID: 4008
			// (get) Token: 0x0600410B RID: 16651 RVA: 0x0010E7C0 File Offset: 0x0010D7C0
			// (set) Token: 0x0600410C RID: 16652 RVA: 0x0010E7C8 File Offset: 0x0010D7C8
			internal Table LayoutTable
			{
				get
				{
					return this._layoutTable;
				}
				set
				{
					this._layoutTable = value;
				}
			}

			// Token: 0x17000FA9 RID: 4009
			// (get) Token: 0x0600410D RID: 16653 RVA: 0x0010E7D1 File Offset: 0x0010D7D1
			// (set) Token: 0x0600410E RID: 16654 RVA: 0x0010E7D9 File Offset: 0x0010D7D9
			internal Literal SuccessTextLabel
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

			// Token: 0x17000FAA RID: 4010
			// (get) Token: 0x0600410F RID: 16655 RVA: 0x0010E7E2 File Offset: 0x0010D7E2
			// (set) Token: 0x06004110 RID: 16656 RVA: 0x0010E7EA File Offset: 0x0010D7EA
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

			// Token: 0x0400288C RID: 10380
			private CreateUserWizard _createUserWizard;

			// Token: 0x0400288D RID: 10381
			private Literal _title;

			// Token: 0x0400288E RID: 10382
			private Literal _successTextLabel;

			// Token: 0x0400288F RID: 10383
			private LinkButton _continueLinkButton;

			// Token: 0x04002890 RID: 10384
			private Button _continuePushButton;

			// Token: 0x04002891 RID: 10385
			private ImageButton _continueImageButton;

			// Token: 0x04002892 RID: 10386
			private Image _editProfileIcon;

			// Token: 0x04002893 RID: 10387
			private Table _layoutTable;

			// Token: 0x04002894 RID: 10388
			private HyperLink _editProfileLink;
		}
	}
}
