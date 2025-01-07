using System;
using System.Collections;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	internal partial class ParameterCollectionEditorForm : DesignerForm
	{
		public ParameterCollectionEditorForm(IServiceProvider serviceProvider, ParameterCollection parameters, ControlDesigner designer)
			: base(serviceProvider)
		{
			this._parameters = parameters;
			if (designer != null)
			{
				this._control = designer.Component as Control;
			}
			this.InitializeComponent();
			this.InitializeUI();
			ArrayList arrayList = new ArrayList();
			foreach (object obj in parameters)
			{
				ICloneable cloneable = (ICloneable)obj;
				object obj2 = cloneable.Clone();
				if (designer != null)
				{
					designer.RegisterClone(cloneable, obj2);
				}
				arrayList.Add(obj2);
			}
			this._parameterEditorUserControl.AddParameters((Parameter[])arrayList.ToArray(typeof(Parameter)));
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.Parameter.CollectionEditor";
			}
		}

		private void InitializeUI()
		{
			this._okButton.Text = SR.GetString("OK");
			this._cancelButton.Text = SR.GetString("Cancel");
			this.Text = SR.GetString("ParameterCollectionEditorForm_Caption");
		}

		private void OnOkButtonClick(object sender, EventArgs e)
		{
			Parameter[] parameters = this._parameterEditorUserControl.GetParameters();
			this._parameters.Clear();
			foreach (Parameter parameter in parameters)
			{
				this._parameters.Add(parameter);
			}
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		private ParameterCollection _parameters;
	}
}
