using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000438 RID: 1080
	public class DataControlFieldTypeEditor : UITypeEditor
	{
		// Token: 0x06002724 RID: 10020 RVA: 0x000D51C8 File Offset: 0x000D41C8
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

		// Token: 0x06002725 RID: 10021 RVA: 0x000D5244 File Offset: 0x000D4244
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
