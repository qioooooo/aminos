using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI.Design.Util;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design.WebControls
{
	internal sealed class WindowsFormsEditorServiceHelper : IWindowsFormsEditorService, IServiceProvider
	{
		public WindowsFormsEditorServiceHelper(ComponentDesigner componentDesigner)
		{
			this._componentDesigner = componentDesigner;
		}

		void IWindowsFormsEditorService.CloseDropDown()
		{
		}

		void IWindowsFormsEditorService.DropDownControl(Control control)
		{
		}

		DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
		{
			return UIServiceHelper.ShowDialog(this, dialog);
		}

		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == typeof(IWindowsFormsEditorService))
			{
				return this;
			}
			IComponent component = this._componentDesigner.Component;
			if (component != null)
			{
				ISite site = this._componentDesigner.Component.Site;
				if (site != null)
				{
					return site.GetService(serviceType);
				}
			}
			return null;
		}

		private ComponentDesigner _componentDesigner;
	}
}
