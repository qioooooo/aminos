using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class DataGridViewColumnTypeEditor : UITypeEditor
	{
		private DataGridViewColumnTypeEditor()
		{
		}

		public override bool IsDropDownResizable
		{
			get
			{
				return true;
			}
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null && context.Instance != null)
				{
					if (this.columnTypePicker == null)
					{
						this.columnTypePicker = new DataGridViewColumnTypePicker();
					}
					DataGridViewColumnCollectionDialog.ListBoxItem listBoxItem = (DataGridViewColumnCollectionDialog.ListBoxItem)context.Instance;
					IDesignerHost designerHost = (IDesignerHost)provider.GetService(typeof(IDesignerHost));
					ITypeDiscoveryService typeDiscoveryService = null;
					if (designerHost != null)
					{
						typeDiscoveryService = (ITypeDiscoveryService)designerHost.GetService(typeof(ITypeDiscoveryService));
					}
					this.columnTypePicker.Start(windowsFormsEditorService, typeDiscoveryService, listBoxItem.DataGridViewColumn.GetType());
					windowsFormsEditorService.DropDownControl(this.columnTypePicker);
					if (this.columnTypePicker.SelectedType != null)
					{
						value = this.columnTypePicker.SelectedType;
					}
				}
			}
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		private DataGridViewColumnTypePicker columnTypePicker;
	}
}
