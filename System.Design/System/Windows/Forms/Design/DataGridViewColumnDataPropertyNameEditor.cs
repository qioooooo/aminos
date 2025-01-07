using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class DataGridViewColumnDataPropertyNameEditor : UITypeEditor
	{
		private DataGridViewColumnDataPropertyNameEditor()
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
			if (provider != null && context.Instance != null)
			{
				DataGridViewColumnCollectionDialog.ListBoxItem listBoxItem = context.Instance as DataGridViewColumnCollectionDialog.ListBoxItem;
				DataGridView dataGridView;
				if (listBoxItem != null)
				{
					dataGridView = listBoxItem.DataGridViewColumn.DataGridView;
				}
				else
				{
					DataGridViewColumn dataGridViewColumn = context.Instance as DataGridViewColumn;
					if (dataGridViewColumn != null)
					{
						dataGridView = dataGridViewColumn.DataGridView;
					}
					else
					{
						dataGridView = null;
					}
				}
				if (dataGridView == null)
				{
					return value;
				}
				object dataSource = dataGridView.DataSource;
				string text = dataGridView.DataMember;
				string text2 = (string)value;
				string text3 = text + "." + text2;
				if (dataSource == null)
				{
					text = string.Empty;
					text3 = text2;
				}
				if (this.designBindingPicker == null)
				{
					this.designBindingPicker = new DesignBindingPicker();
				}
				DesignBinding designBinding = new DesignBinding(dataSource, text3);
				DesignBinding designBinding2 = this.designBindingPicker.Pick(context, provider, false, true, false, dataSource, text, designBinding);
				if (dataSource != null && designBinding2 != null)
				{
					value = designBinding2.DataField;
				}
			}
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		private DesignBindingPicker designBindingPicker;
	}
}
