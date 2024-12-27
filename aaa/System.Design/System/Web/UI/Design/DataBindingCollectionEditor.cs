using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design
{
	// Token: 0x02000341 RID: 833
	[Obsolete("Use of this type is not recommended because DataBindings editing is launched via a DesignerActionList instead of the property grid. http://go.microsoft.com/fwlink/?linkid=14202")]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataBindingCollectionEditor : UITypeEditor
	{
		// Token: 0x06001F76 RID: 8054 RVA: 0x000B3030 File Offset: 0x000B2030
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			Control control = (Control)context.Instance;
			IServiceProvider serviceProvider = control.Site;
			if (serviceProvider == null)
			{
				if (control.Page != null)
				{
					serviceProvider = control.Page.Site;
				}
				if (serviceProvider == null)
				{
					serviceProvider = provider;
				}
			}
			if (serviceProvider == null)
			{
				return value;
			}
			IDesignerHost designerHost = (IDesignerHost)serviceProvider.GetService(typeof(IDesignerHost));
			DesignerTransaction designerTransaction = designerHost.CreateTransaction("(DataBindings)");
			try
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)serviceProvider.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					try
					{
						componentChangeService.OnComponentChanging(control, null);
					}
					catch (CheckoutException ex)
					{
						if (ex == CheckoutException.Canceled)
						{
							return value;
						}
						throw ex;
					}
				}
				DialogResult dialogResult = DialogResult.Cancel;
				try
				{
					DataBindingsDialog dataBindingsDialog = new DataBindingsDialog(serviceProvider, control);
					IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
					dialogResult = windowsFormsEditorService.ShowDialog(dataBindingsDialog);
				}
				finally
				{
					if (dialogResult == DialogResult.OK && componentChangeService != null)
					{
						try
						{
							componentChangeService.OnComponentChanged(control, null, null, null);
						}
						catch
						{
						}
					}
				}
			}
			finally
			{
				designerTransaction.Commit();
			}
			return value;
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x000B3158 File Offset: 0x000B2158
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
