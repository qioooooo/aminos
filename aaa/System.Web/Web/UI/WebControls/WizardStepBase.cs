using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004FE RID: 1278
	[Bindable(false)]
	[ControlBuilder(typeof(WizardStepControlBuilder))]
	[ToolboxItem(false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class WizardStepBase : View
	{
		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06003E75 RID: 15989 RVA: 0x00104974 File Offset: 0x00103974
		// (set) Token: 0x06003E76 RID: 15990 RVA: 0x0010499D File Offset: 0x0010399D
		[WebCategory("Behavior")]
		[WebSysDescription("WizardStep_AllowReturn")]
		[Themeable(false)]
		[Filterable(false)]
		[DefaultValue(true)]
		public virtual bool AllowReturn
		{
			get
			{
				object obj = this.ViewState["AllowReturn"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["AllowReturn"] = value;
			}
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06003E77 RID: 15991 RVA: 0x001049B5 File Offset: 0x001039B5
		// (set) Token: 0x06003E78 RID: 15992 RVA: 0x001049BD File Offset: 0x001039BD
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

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x06003E79 RID: 15993 RVA: 0x001049C6 File Offset: 0x001039C6
		// (set) Token: 0x06003E7A RID: 15994 RVA: 0x001049D0 File Offset: 0x001039D0
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
				if (this.Owner != null && this.Owner.DesignMode)
				{
					if (!CodeGenerator.IsValidLanguageIndependentIdentifier(value))
					{
						throw new ArgumentException(SR.GetString("Invalid_identifier", new object[] { value }));
					}
					if (value != null && value.Equals(this.Owner.ID, StringComparison.OrdinalIgnoreCase))
					{
						throw new ArgumentException(SR.GetString("Id_already_used", new object[] { value }));
					}
					foreach (object obj in this.Owner.WizardSteps)
					{
						WizardStepBase wizardStepBase = (WizardStepBase)obj;
						if (wizardStepBase != this && wizardStepBase.ID != null && wizardStepBase.ID.Equals(value, StringComparison.OrdinalIgnoreCase))
						{
							throw new ArgumentException(SR.GetString("Id_already_used", new object[] { value }));
						}
					}
				}
				base.ID = value;
			}
		}

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x06003E7B RID: 15995 RVA: 0x00104AE0 File Offset: 0x00103AE0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Appearance")]
		[WebSysDescription("WizardStep_Name")]
		[Browsable(false)]
		public virtual string Name
		{
			get
			{
				if (!string.IsNullOrEmpty(this.Title))
				{
					return this.Title;
				}
				if (!string.IsNullOrEmpty(this.ID))
				{
					return this.ID;
				}
				return null;
			}
		}

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x06003E7C RID: 15996 RVA: 0x00104B0B File Offset: 0x00103B0B
		// (set) Token: 0x06003E7D RID: 15997 RVA: 0x00104B13 File Offset: 0x00103B13
		internal virtual Wizard Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				this._owner = value;
			}
		}

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x06003E7E RID: 15998 RVA: 0x00104B1C File Offset: 0x00103B1C
		// (set) Token: 0x06003E7F RID: 15999 RVA: 0x00104B48 File Offset: 0x00103B48
		[WebSysDescription("WizardStep_StepType")]
		[WebCategory("Behavior")]
		[DefaultValue(WizardStepType.Auto)]
		public virtual WizardStepType StepType
		{
			get
			{
				object obj = this.ViewState["StepType"];
				if (obj != null)
				{
					return (WizardStepType)obj;
				}
				return WizardStepType.Auto;
			}
			set
			{
				if (value < WizardStepType.Auto || value > WizardStepType.Step)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.StepType != value)
				{
					this.ViewState["StepType"] = value;
					if (this.Owner != null)
					{
						this.Owner.OnWizardStepsChanged();
					}
				}
			}
		}

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x06003E80 RID: 16000 RVA: 0x00104B9C File Offset: 0x00103B9C
		// (set) Token: 0x06003E81 RID: 16001 RVA: 0x00104BC9 File Offset: 0x00103BC9
		[Localizable(true)]
		[DefaultValue("")]
		[WebSysDescription("WizardStep_Title")]
		[WebCategory("Appearance")]
		public virtual string Title
		{
			get
			{
				string text = (string)this.ViewState["Title"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				if (this.Title != value)
				{
					this.ViewState["Title"] = value;
					if (this.Owner != null)
					{
						this.Owner.OnWizardStepsChanged();
					}
				}
			}
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x06003E82 RID: 16002 RVA: 0x00104BFD File Offset: 0x00103BFD
		internal string TitleInternal
		{
			get
			{
				return (string)this.ViewState["Title"];
			}
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06003E83 RID: 16003 RVA: 0x00104C14 File Offset: 0x00103C14
		[WebCategory("Appearance")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public Wizard Wizard
		{
			get
			{
				return this.Owner;
			}
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x00104C1C File Offset: 0x00103C1C
		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				base.LoadViewState(savedState);
				if (this.Owner != null && (this.ViewState["Title"] != null || this.ViewState["StepType"] != null))
				{
					this.Owner.OnWizardStepsChanged();
				}
			}
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x00104C6A File Offset: 0x00103C6A
		protected internal override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (this.Owner == null && !base.DesignMode)
			{
				throw new InvalidOperationException(SR.GetString("WizardStep_WrongContainment"));
			}
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x00104C93 File Offset: 0x00103C93
		protected internal override void RenderChildren(HtmlTextWriter writer)
		{
			if (!this.Owner.ShouldRenderChildControl)
			{
				return;
			}
			base.RenderChildren(writer);
		}

		// Token: 0x04002799 RID: 10137
		private Wizard _owner;
	}
}
