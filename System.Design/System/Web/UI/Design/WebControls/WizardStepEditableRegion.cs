using System;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	public class WizardStepEditableRegion : EditableDesignerRegion, IWizardStepEditableRegion
	{
		public WizardStepEditableRegion(WizardDesigner designer, WizardStepBase wizardStep)
			: base(designer, designer.GetRegionName(wizardStep), false)
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
