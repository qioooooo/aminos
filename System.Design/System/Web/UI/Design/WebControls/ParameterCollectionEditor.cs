using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing.Design;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	public class ParameterCollectionEditor : UITypeEditor
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			ParameterCollection parameterCollection = value as ParameterCollection;
			if (parameterCollection == null)
			{
				throw new ArgumentException(SR.GetString("ParameterCollectionEditor_InvalidParameters"), "value");
			}
			Control control = context.Instance as Control;
			ControlDesigner controlDesigner = null;
			if (control != null && control.Site != null)
			{
				IDesignerHost designerHost = (IDesignerHost)control.Site.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					controlDesigner = designerHost.GetDesigner(control) as ControlDesigner;
				}
			}
			ParameterCollectionEditorForm parameterCollectionEditorForm = new ParameterCollectionEditorForm(provider, parameterCollection, controlDesigner);
			DialogResult dialogResult = parameterCollectionEditorForm.ShowDialog();
			if (dialogResult == DialogResult.OK && context != null)
			{
				context.OnComponentChanged();
			}
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
