using System;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	internal class WizardSelectableRegion : DesignerRegion
	{
		internal WizardSelectableRegion(WizardDesigner designer, string name, WizardStepBase wizardStep)
			: base(designer, name, true)
		{
			this._wizardStep = wizardStep;
		}

		internal WizardStepBase Step
		{
			get
			{
				return this._wizardStep;
			}
		}

		private WizardStepBase _wizardStep;
	}
}
