using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000527 RID: 1319
	[Browsable(false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CreateUserWizardStep : TemplatedWizardStep
	{
		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x06004111 RID: 16657 RVA: 0x0010E7F3 File Offset: 0x0010D7F3
		// (set) Token: 0x06004112 RID: 16658 RVA: 0x0010E7FB File Offset: 0x0010D7FB
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool AllowReturn
		{
			get
			{
				return this.AllowReturnInternal;
			}
			set
			{
				throw new InvalidOperationException(SR.GetString("CreateUserWizardStep_AllowReturnCannotBeSet"));
			}
		}

		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x06004113 RID: 16659 RVA: 0x0010E80C File Offset: 0x0010D80C
		// (set) Token: 0x06004114 RID: 16660 RVA: 0x0010E835 File Offset: 0x0010D835
		internal bool AllowReturnInternal
		{
			get
			{
				object obj = this.ViewState["AllowReturnInternal"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["AllowReturnInternal"] = value;
			}
		}

		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x06004115 RID: 16661 RVA: 0x0010E84D File Offset: 0x0010D84D
		// (set) Token: 0x06004116 RID: 16662 RVA: 0x0010E855 File Offset: 0x0010D855
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
				throw new HttpException(SR.GetString("CreateUserWizardStep_OnlyAllowedInCreateUserWizard"));
			}
		}

		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x06004117 RID: 16663 RVA: 0x0010E87C File Offset: 0x0010D87C
		// (set) Token: 0x06004118 RID: 16664 RVA: 0x0010E89F File Offset: 0x0010D89F
		[Localizable(true)]
		[WebSysDefaultValue("CreateUserWizard_DefaultCreateUserTitleText")]
		public override string Title
		{
			get
			{
				string titleInternal = base.TitleInternal;
				if (titleInternal == null)
				{
					return SR.GetString("CreateUserWizard_DefaultCreateUserTitleText");
				}
				return titleInternal;
			}
			set
			{
				base.Title = value;
			}
		}

		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x06004119 RID: 16665 RVA: 0x0010E8A8 File Offset: 0x0010D8A8
		// (set) Token: 0x0600411A RID: 16666 RVA: 0x0010E8B0 File Offset: 0x0010D8B0
		[Browsable(false)]
		[Themeable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Filterable(false)]
		public override WizardStepType StepType
		{
			get
			{
				return base.StepType;
			}
			set
			{
				throw new InvalidOperationException(SR.GetString("CreateUserWizardStep_StepTypeCannotBeSet"));
			}
		}
	}
}
