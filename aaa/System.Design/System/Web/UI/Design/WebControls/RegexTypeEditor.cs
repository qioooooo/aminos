using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004A6 RID: 1190
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class RegexTypeEditor : UITypeEditor
	{
		// Token: 0x06002B11 RID: 11025 RVA: 0x000EE8D8 File Offset: 0x000ED8D8
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					ISite site = null;
					if (context.Instance is IComponent)
					{
						site = ((IComponent)context.Instance).Site;
					}
					else if (context.Instance is object[])
					{
						object[] array = (object[])context.Instance;
						if (array[0] is IComponent)
						{
							site = ((IComponent)array[0]).Site;
						}
					}
					RegexEditorDialog regexEditorDialog = new RegexEditorDialog(site);
					regexEditorDialog.RegularExpression = value.ToString();
					if (regexEditorDialog.ShowDialog() == DialogResult.OK)
					{
						value = regexEditorDialog.RegularExpression;
					}
				}
			}
			return value;
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x000EE97C File Offset: 0x000ED97C
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
