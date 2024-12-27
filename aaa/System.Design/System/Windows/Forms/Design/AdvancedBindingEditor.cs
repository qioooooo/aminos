using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000173 RID: 371
	internal class AdvancedBindingEditor : UITypeEditor
	{
		// Token: 0x06000D99 RID: 3481 RVA: 0x00037B20 File Offset: 0x00036B20
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

		// Token: 0x06000D9A RID: 3482 RVA: 0x00037C0C File Offset: 0x00036C0C
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		// Token: 0x04000F24 RID: 3876
		private BindingFormattingDialog bindingFormattingDialog;
	}
}
