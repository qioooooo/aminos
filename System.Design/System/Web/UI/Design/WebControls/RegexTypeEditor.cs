using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class RegexTypeEditor : UITypeEditor
	{
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

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
