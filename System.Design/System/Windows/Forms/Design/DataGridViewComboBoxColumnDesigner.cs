using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	internal class DataGridViewComboBoxColumnDesigner : DataGridViewColumnDesigner
	{
		private string ValueMember
		{
			get
			{
				DataGridViewComboBoxColumn dataGridViewComboBoxColumn = (DataGridViewComboBoxColumn)base.Component;
				return dataGridViewComboBoxColumn.ValueMember;
			}
			set
			{
				DataGridViewComboBoxColumn dataGridViewComboBoxColumn = (DataGridViewComboBoxColumn)base.Component;
				if (dataGridViewComboBoxColumn.DataSource == null)
				{
					return;
				}
				if (DataGridViewComboBoxColumnDesigner.ValidDataMember(dataGridViewComboBoxColumn.DataSource, value))
				{
					dataGridViewComboBoxColumn.ValueMember = value;
				}
			}
		}

		private string DisplayMember
		{
			get
			{
				DataGridViewComboBoxColumn dataGridViewComboBoxColumn = (DataGridViewComboBoxColumn)base.Component;
				return dataGridViewComboBoxColumn.DisplayMember;
			}
			set
			{
				DataGridViewComboBoxColumn dataGridViewComboBoxColumn = (DataGridViewComboBoxColumn)base.Component;
				if (dataGridViewComboBoxColumn.DataSource == null)
				{
					return;
				}
				if (DataGridViewComboBoxColumnDesigner.ValidDataMember(dataGridViewComboBoxColumn.DataSource, value))
				{
					dataGridViewComboBoxColumn.DisplayMember = value;
				}
			}
		}

		private bool ShouldSerializeDisplayMember()
		{
			DataGridViewComboBoxColumn dataGridViewComboBoxColumn = (DataGridViewComboBoxColumn)base.Component;
			return !string.IsNullOrEmpty(dataGridViewComboBoxColumn.DisplayMember);
		}

		private bool ShouldSerializeValueMember()
		{
			DataGridViewComboBoxColumn dataGridViewComboBoxColumn = (DataGridViewComboBoxColumn)base.Component;
			return !string.IsNullOrEmpty(dataGridViewComboBoxColumn.ValueMember);
		}

		private static bool ValidDataMember(object dataSource, string dataMember)
		{
			if (string.IsNullOrEmpty(dataMember))
			{
				return true;
			}
			if (DataGridViewComboBoxColumnDesigner.bc == null)
			{
				DataGridViewComboBoxColumnDesigner.bc = new BindingContext();
			}
			int count = ((ICollection)DataGridViewComboBoxColumnDesigner.bc).Count;
			BindingMemberInfo bindingMemberInfo = new BindingMemberInfo(dataMember);
			BindingManagerBase bindingManagerBase;
			try
			{
				bindingManagerBase = DataGridViewComboBoxColumnDesigner.bc[dataSource, bindingMemberInfo.BindingPath];
			}
			catch (ArgumentException)
			{
				return false;
			}
			if (bindingManagerBase == null)
			{
				return false;
			}
			PropertyDescriptorCollection itemProperties = bindingManagerBase.GetItemProperties();
			return itemProperties != null && itemProperties[bindingMemberInfo.BindingField] != null;
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["ValueMember"];
			if (propertyDescriptor != null)
			{
				properties["ValueMember"] = TypeDescriptor.CreateProperty(typeof(DataGridViewComboBoxColumnDesigner), propertyDescriptor, new Attribute[0]);
			}
			propertyDescriptor = (PropertyDescriptor)properties["DisplayMember"];
			if (propertyDescriptor != null)
			{
				properties["DisplayMember"] = TypeDescriptor.CreateProperty(typeof(DataGridViewComboBoxColumnDesigner), propertyDescriptor, new Attribute[0]);
			}
		}

		private static BindingContext bc;
	}
}
