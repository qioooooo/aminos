using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design
{
	// Token: 0x0200036B RID: 875
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ExpressionsCollectionEditor : UITypeEditor
	{
		// Token: 0x060020D5 RID: 8405 RVA: 0x000B8864 File Offset: 0x000B7864
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
			DesignerTransaction designerTransaction = designerHost.CreateTransaction("(Expressions)");
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
					ExpressionBindingsDialog expressionBindingsDialog = new ExpressionBindingsDialog(serviceProvider, control);
					IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
					dialogResult = windowsFormsEditorService.ShowDialog(expressionBindingsDialog);
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

		// Token: 0x060020D6 RID: 8406 RVA: 0x000B898C File Offset: 0x000B798C
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}
