using System;
using System.ComponentModel.Design;
using System.Text;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	internal class WizardStepBaseTemplateDefinition : TemplateDefinition
	{
		public WizardStepBaseTemplateDefinition(WizardDesigner designer, WizardStepBase step, string name, Style style)
			: base(designer, name, step, name, style)
		{
			this._step = step;
		}

		public override string Content
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (object obj in this._step.Controls)
				{
					Control control = (Control)obj;
					stringBuilder.Append(ControlPersister.PersistControl(control));
				}
				return stringBuilder.ToString();
			}
			set
			{
				this._step.Controls.Clear();
				if (value == null)
				{
					return;
				}
				IDesignerHost designerHost = (IDesignerHost)base.GetService(typeof(IDesignerHost));
				Control[] array = ControlParser.ParseControls(designerHost, value);
				foreach (Control control in array)
				{
					this._step.Controls.Add(control);
				}
			}
		}

		private WizardStepBase _step;
	}
}
