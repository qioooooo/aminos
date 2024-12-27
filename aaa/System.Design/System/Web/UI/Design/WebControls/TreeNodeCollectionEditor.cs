using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004ED RID: 1261
	public class TreeNodeCollectionEditor : UITypeEditor
	{
		// Token: 0x06002D18 RID: 11544 RVA: 0x000FE960 File Offset: 0x000FD960
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IDesignerHost designerHost = (IDesignerHost)context.GetService(typeof(IDesignerHost));
			TreeView treeView = (TreeView)context.Instance;
			TreeViewDesigner treeViewDesigner = (TreeViewDesigner)designerHost.GetDesigner(treeView);
			treeViewDesigner.InvokeTreeNodeCollectionEditor();
			return value;
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x000FE9A3 File Offset: 0x000FD9A3
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
