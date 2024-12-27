using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000500 RID: 1280
	[Browsable(false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CompleteWizardStep : TemplatedWizardStep
	{
		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06003E93 RID: 16019 RVA: 0x00104D47 File Offset: 0x00103D47
		// (set) Token: 0x06003E94 RID: 16020 RVA: 0x00104D4F File Offset: 0x00103D4F
		internal override Wizard Owner
		{
			get
			{
				return base.Owner;
			}
			set
			{
				if (value is CreateUserWizard || value == null)
				{
					base.Owner = value;
					return;
				}
				throw new HttpException(SR.GetString("CompleteWizardStep_OnlyAllowedInCreateUserWizard"));
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06003E95 RID: 16021 RVA: 0x00104D73 File Offset: 0x00103D73
		// (set) Token: 0x06003E96 RID: 16022 RVA: 0x00104D76 File Offset: 0x00103D76
		[Browsable(false)]
		[Themeable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Filterable(false)]
		public override WizardStepType StepType
		{
			get
			{
				return WizardStepType.Complete;
			}
			set
			{
				throw new InvalidOperationException(SR.GetString("CreateUserWizardStep_StepTypeCannotBeSet"));
			}
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x06003E97 RID: 16023 RVA: 0x00104D88 File Offset: 0x00103D88
		// (set) Token: 0x06003E98 RID: 16024 RVA: 0x00104DAB File Offset: 0x00103DAB
		[WebSysDefaultValue("CreateUserWizard_DefaultCompleteTitleText")]
		[Localizable(true)]
		public override string Title
		{
			get
			{
				string titleInternal = base.TitleInternal;
				if (titleInternal == null)
				{
					return SR.GetString("CreateUserWizard_DefaultCompleteTitleText");
				}
				return titleInternal;
			}
			set
			{
				base.Title = value;
			}
		}
	}
}
