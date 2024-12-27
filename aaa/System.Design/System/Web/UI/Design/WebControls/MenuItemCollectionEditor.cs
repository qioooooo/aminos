using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200047D RID: 1149
	public class MenuItemCollectionEditor : UITypeEditor
	{
		// Token: 0x060029B2 RID: 10674 RVA: 0x000E43E0 File Offset: 0x000E33E0
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IDesignerHost designerHost = (IDesignerHost)context.GetService(typeof(IDesignerHost));
			Menu menu = (Menu)context.Instance;
			MenuDesigner menuDesigner = (MenuDesigner)designerHost.GetDesigner(menu);
			menuDesigner.InvokeMenuItemCollectionEditor();
			return value;
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x000E4423 File Offset: 0x000E3423
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
