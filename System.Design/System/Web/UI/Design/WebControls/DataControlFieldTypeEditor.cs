using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	public class DataControlFieldTypeEditor : UITypeEditor
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			DataBoundControl dataBoundControl = context.Instance as DataBoundControl;
			if (dataBoundControl != null)
			{
				IDesignerHost designerHost = (IDesignerHost)provider.GetService(typeof(IDesignerHost));
				DataBoundControlDesigner dataBoundControlDesigner = (DataBoundControlDesigner)designerHost.GetDesigner(dataBoundControl);
				IComponentChangeService componentChangeService = (IComponentChangeService)provider.GetService(typeof(IComponentChangeService));
				DataControlFieldsEditor dataControlFieldsEditor = new DataControlFieldsEditor(dataBoundControlDesigner);
				DialogResult dialogResult = UIServiceHelper.ShowDialog(provider, dataControlFieldsEditor);
				if (dialogResult == DialogResult.OK && componentChangeService != null)
				{
					componentChangeService.OnComponentChanged(dataBoundControl, null, null, null);
				}
				return value;
			}
			return null;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
