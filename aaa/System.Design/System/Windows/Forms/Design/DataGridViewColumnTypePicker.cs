using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001EB RID: 491
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	internal class DataGridViewColumnTypePicker : ContainerControl
	{
		// Token: 0x060012EA RID: 4842 RVA: 0x00060884 File Offset: 0x0005F884
		public DataGridViewColumnTypePicker()
		{
			this.typesListBox = new ListBox();
			base.Size = this.typesListBox.Size;
			this.typesListBox.Dock = DockStyle.Fill;
			this.typesListBox.Sorted = true;
			this.typesListBox.HorizontalScrollbar = true;
			this.typesListBox.SelectedIndexChanged += this.typesListBox_SelectedIndexChanged;
			base.Controls.Add(this.typesListBox);
			this.BackColor = SystemColors.Control;
			base.ActiveControl = this.typesListBox;
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x060012EB RID: 4843 RVA: 0x00060916 File Offset: 0x0005F916
		public Type SelectedType
		{
			get
			{
				return this.selectedType;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x060012EC RID: 4844 RVA: 0x00060920 File Offset: 0x0005F920
		private int PreferredWidth
		{
			get
			{
				int num = 0;
				Graphics graphics = this.typesListBox.CreateGraphics();
				try
				{
					for (int i = 0; i < this.typesListBox.Items.Count; i++)
					{
						DataGridViewColumnTypePicker.ListBoxItem listBoxItem = (DataGridViewColumnTypePicker.ListBoxItem)this.typesListBox.Items[i];
						num = Math.Max(num, Size.Ceiling(graphics.MeasureString(listBoxItem.ToString(), this.typesListBox.Font)).Width);
					}
				}
				finally
				{
					graphics.Dispose();
				}
				return num;
			}
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x000609B4 File Offset: 0x0005F9B4
		private void CloseDropDown()
		{
			if (this.edSvc != null)
			{
				this.edSvc.CloseDropDown();
			}
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x000609C9 File Offset: 0x0005F9C9
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((BoundsSpecified.Width & specified) == BoundsSpecified.Width)
			{
				width = Math.Max(width, 100);
			}
			if ((BoundsSpecified.Height & specified) == BoundsSpecified.Height)
			{
				height = Math.Max(height, 90);
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x000609FC File Offset: 0x0005F9FC
		public void Start(IWindowsFormsEditorService edSvc, ITypeDiscoveryService discoveryService, Type defaultType)
		{
			this.edSvc = edSvc;
			this.typesListBox.Items.Clear();
			ICollection collection = DesignerUtils.FilterGenericTypes(discoveryService.GetTypes(DataGridViewColumnTypePicker.dataGridViewColumnType, false));
			foreach (object obj in collection)
			{
				Type type = (Type)obj;
				if (type != DataGridViewColumnTypePicker.dataGridViewColumnType && !type.IsAbstract && (type.IsPublic || type.IsNestedPublic))
				{
					DataGridViewColumnDesignTimeVisibleAttribute dataGridViewColumnDesignTimeVisibleAttribute = TypeDescriptor.GetAttributes(type)[typeof(DataGridViewColumnDesignTimeVisibleAttribute)] as DataGridViewColumnDesignTimeVisibleAttribute;
					if (dataGridViewColumnDesignTimeVisibleAttribute == null || dataGridViewColumnDesignTimeVisibleAttribute.Visible)
					{
						this.typesListBox.Items.Add(new DataGridViewColumnTypePicker.ListBoxItem(type));
					}
				}
			}
			this.typesListBox.SelectedIndex = this.TypeToSelectedIndex(defaultType);
			this.selectedType = null;
			base.Width = Math.Max(base.Width, this.PreferredWidth + SystemInformation.VerticalScrollBarWidth * 2);
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x00060B0C File Offset: 0x0005FB0C
		private void typesListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.selectedType = ((DataGridViewColumnTypePicker.ListBoxItem)this.typesListBox.SelectedItem).ColumnType;
			this.edSvc.CloseDropDown();
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x00060B34 File Offset: 0x0005FB34
		private int TypeToSelectedIndex(Type type)
		{
			for (int i = 0; i < this.typesListBox.Items.Count; i++)
			{
				if (type == ((DataGridViewColumnTypePicker.ListBoxItem)this.typesListBox.Items[i]).ColumnType)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x04001184 RID: 4484
		private const int MinimumHeight = 90;

		// Token: 0x04001185 RID: 4485
		private const int MinimumWidth = 100;

		// Token: 0x04001186 RID: 4486
		private ListBox typesListBox;

		// Token: 0x04001187 RID: 4487
		private Type selectedType;

		// Token: 0x04001188 RID: 4488
		private IWindowsFormsEditorService edSvc;

		// Token: 0x04001189 RID: 4489
		private static Type dataGridViewColumnType = typeof(DataGridViewColumn);

		// Token: 0x020001EC RID: 492
		private class ListBoxItem
		{
			// Token: 0x060012F3 RID: 4851 RVA: 0x00060B8E File Offset: 0x0005FB8E
			public ListBoxItem(Type columnType)
			{
				this.columnType = columnType;
			}

			// Token: 0x060012F4 RID: 4852 RVA: 0x00060B9D File Offset: 0x0005FB9D
			public override string ToString()
			{
				return this.columnType.Name;
			}

			// Token: 0x17000301 RID: 769
			// (get) Token: 0x060012F5 RID: 4853 RVA: 0x00060BAA File Offset: 0x0005FBAA
			public Type ColumnType
			{
				get
				{
					return this.columnType;
				}
			}

			// Token: 0x0400118A RID: 4490
			private Type columnType;
		}
	}
}
