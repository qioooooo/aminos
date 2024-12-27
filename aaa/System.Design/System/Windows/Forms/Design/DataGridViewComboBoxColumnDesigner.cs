using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001ED RID: 493
	internal class DataGridViewComboBoxColumnDesigner : DataGridViewColumnDesigner
	{
		// Token: 0x17000302 RID: 770
		// (get) Token: 0x060012F6 RID: 4854 RVA: 0x00060BB4 File Offset: 0x0005FBB4
		// (set) Token: 0x060012F7 RID: 4855 RVA: 0x00060BD4 File Offset: 0x0005FBD4
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

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x060012F8 RID: 4856 RVA: 0x00060C0C File Offset: 0x0005FC0C
		// (set) Token: 0x060012F9 RID: 4857 RVA: 0x00060C2C File Offset: 0x0005FC2C
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

		// Token: 0x060012FA RID: 4858 RVA: 0x00060C64 File Offset: 0x0005FC64
		private bool ShouldSerializeDisplayMember()
		{
			DataGridViewComboBoxColumn dataGridViewComboBoxColumn = (DataGridViewComboBoxColumn)base.Component;
			return !string.IsNullOrEmpty(dataGridViewComboBoxColumn.DisplayMember);
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x00060C8C File Offset: 0x0005FC8C
		private bool ShouldSerializeValueMember()
		{
			DataGridViewComboBoxColumn dataGridViewComboBoxColumn = (DataGridViewComboBoxColumn)base.Component;
			return !string.IsNullOrEmpty(dataGridViewComboBoxColumn.ValueMember);
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x00060CB4 File Offset: 0x0005FCB4
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

		// Token: 0x060012FD RID: 4861 RVA: 0x00060D40 File Offset: 0x0005FD40
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

		// Token: 0x0400118B RID: 4491
		private static BindingContext bc;
	}
}
