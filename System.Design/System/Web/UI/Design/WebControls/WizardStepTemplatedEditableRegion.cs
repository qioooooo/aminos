using System;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	public class WizardStepTemplatedEditableRegion : TemplatedEditableDesignerRegion, IWizardStepEditableRegion
	{
		public WizardStepTemplatedEditableRegion(TemplateDefinition templateDefinition, WizardStepBase wizardStep)
			: base(templateDefinition)
		{
			this._wizardStep = wizardStep;
			base.EnsureSize = true;
		}

		public WizardStepBase Step
		{
			get
			{
				return this._wizardStep;
			}
		}

		private WizardStepBase _wizardStep;
	}
}
