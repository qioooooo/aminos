using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class DataGridViewColumnCollectionEditor : UITypeEditor
	{
		private DataGridViewColumnCollectionEditor()
		{
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null && context.Instance != null)
				{
					IDesignerHost designerHost = (IDesignerHost)provider.GetService(typeof(IDesignerHost));
					if (designerHost == null)
					{
						return value;
					}
					if (this.dataGridViewColumnCollectionDialog == null)
					{
						this.dataGridViewColumnCollectionDialog = new DataGridViewColumnCollectionDialog();
					}
					this.dataGridViewColumnCollectionDialog.SetLiveDataGridView((DataGridView)context.Instance);
					using (DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString("DataGridViewColumnCollectionTransaction")))
					{
						if (windowsFormsEditorService.ShowDialog(this.dataGridViewColumnCollectionDialog) == DialogResult.OK)
						{
							designerTransaction.Commit();
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

		private DataGridViewColumnCollectionDialog dataGridViewColumnCollectionDialog;
	}
}
