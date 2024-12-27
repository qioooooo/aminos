using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000473 RID: 1139
	public class MenuBindingsEditor : UITypeEditor
	{
		// Token: 0x0600294D RID: 10573 RVA: 0x000E2204 File Offset: 0x000E1204
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IDesignerHost designerHost = (IDesignerHost)context.GetService(typeof(IDesignerHost));
			Menu menu = (Menu)context.Instance;
			MenuDesigner menuDesigner = (MenuDesigner)designerHost.GetDesigner(menu);
			menuDesigner.InvokeMenuBindingsEditor();
			return value;
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x000E2247 File Offset: 0x000E1247
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
