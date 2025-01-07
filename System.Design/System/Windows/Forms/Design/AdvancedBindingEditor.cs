using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class AdvancedBindingEditor : UITypeEditor
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				IDesignerHost designerHost = provider.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (windowsFormsEditorService != null && designerHost != null)
				{
					if (this.bindingFormattingDialog == null)
					{
						this.bindingFormattingDialog = new BindingFormattingDialog();
					}
					this.bindingFormattingDialog.Context = context;
					this.bindingFormattingDialog.Bindings = (ControlBindingsCollection)value;
					this.bindingFormattingDialog.Host = designerHost;
					using (DesignerTransaction designerTransaction = designerHost.CreateTransaction())
					{
						windowsFormsEditorService.ShowDialog(this.bindingFormattingDialog);
						if (this.bindingFormattingDialog.Dirty)
						{
							TypeDescriptor.Refresh(((ControlBindingsCollection)context.Instance).BindableComponent);
							if (designerTransaction != null)
							{
								designerTransaction.Commit();
							}
						}
						else
						{
							designerTransaction.Cancel();
						}
					}
				}
			}
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		private BindingFormattingDialog bindingFormattingDialog;
	}
}
