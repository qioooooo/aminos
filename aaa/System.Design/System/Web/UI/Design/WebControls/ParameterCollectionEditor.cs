using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing.Design;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200048E RID: 1166
	public class ParameterCollectionEditor : UITypeEditor
	{
		// Token: 0x06002A50 RID: 10832 RVA: 0x000E8BE8 File Offset: 0x000E7BE8
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

		// Token: 0x06002A51 RID: 10833 RVA: 0x000E8C7A File Offset: 0x000E7C7A
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
