using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200040B RID: 1035
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataGridColumnCollectionEditor : UITypeEditor
	{
		// Token: 0x060025D9 RID: 9689 RVA: 0x000CBEB8 File Offset: 0x000CAEB8
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IDesignerHost designerHost = (IDesignerHost)context.GetService(typeof(IDesignerHost));
			DataGrid dataGrid = (DataGrid)context.Instance;
			BaseDataListDesigner baseDataListDesigner = (BaseDataListDesigner)designerHost.GetDesigner(dataGrid);
			baseDataListDesigner.InvokePropertyBuilder(DataGridComponentEditor.IDX_COLUMNS);
			return value;
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x000CBF00 File Offset: 0x000CAF00
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
