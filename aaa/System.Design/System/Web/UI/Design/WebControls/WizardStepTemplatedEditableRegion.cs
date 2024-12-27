using System;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000501 RID: 1281
	public class WizardStepTemplatedEditableRegion : TemplatedEditableDesignerRegion, IWizardStepEditableRegion
	{
		// Token: 0x06002DBB RID: 11707 RVA: 0x00103627 File Offset: 0x00102627
		public WizardStepTemplatedEditableRegion(TemplateDefinition templateDefinition, WizardStepBase wizardStep)
			: base(templateDefinition)
		{
			this._wizardStep = wizardStep;
			base.EnsureSize = true;
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06002DBC RID: 11708 RVA: 0x0010363E File Offset: 0x0010263E
		public WizardStepBase Step
		{
			get
			{
				return this._wizardStep;
			}
		}

		// Token: 0x04001F13 RID: 7955
		private WizardStepBase _wizardStep;
	}
}
