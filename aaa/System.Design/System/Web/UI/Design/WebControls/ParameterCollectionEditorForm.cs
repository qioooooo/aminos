using System;
using System.Collections;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200048F RID: 1167
	internal partial class ParameterCollectionEditorForm : DesignerForm
	{
		// Token: 0x06002A53 RID: 10835 RVA: 0x000E8C88 File Offset: 0x000E7C88
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

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06002A54 RID: 10836 RVA: 0x000E8D48 File Offset: 0x000E7D48
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.Parameter.CollectionEditor";
			}
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x000E8EF0 File Offset: 0x000E7EF0
		private void InitializeUI()
		{
			this._okButton.Text = SR.GetString("OK");
			this._cancelButton.Text = SR.GetString("Cancel");
			this.Text = SR.GetString("ParameterCollectionEditorForm_Caption");
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x000E8F2C File Offset: 0x000E7F2C
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

		// Token: 0x06002A58 RID: 10840 RVA: 0x000E8F7E File Offset: 0x000E7F7E
		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x04001CDE RID: 7390
		private ParameterCollection _parameters;
	}
}
