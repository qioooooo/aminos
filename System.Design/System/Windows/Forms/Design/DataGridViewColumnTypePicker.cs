using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	internal class DataGridViewColumnTypePicker : ContainerControl
	{
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

		public Type SelectedType
		{
			get
			{
				return this.selectedType;
			}
		}

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

		private void CloseDropDown()
		{
			if (this.edSvc != null)
			{
				this.edSvc.CloseDropDown();
			}
		}

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

		private void typesListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.selectedType = ((DataGridViewColumnTypePicker.ListBoxItem)this.typesListBox.SelectedItem).ColumnType;
			this.edSvc.CloseDropDown();
		}

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

		private const int MinimumHeight = 90;

		private const int MinimumWidth = 100;

		private ListBox typesListBox;

		private Type selectedType;

		private IWindowsFormsEditorService edSvc;

		private static Type dataGridViewColumnType = typeof(DataGridViewColumn);

		private class ListBoxItem
		{
			public ListBoxItem(Type columnType)
			{
				this.columnType = columnType;
			}

			public override string ToString()
			{
				return this.columnType.Name;
			}

			public Type ColumnType
			{
				get
				{
					return this.columnType;
				}
			}

			private Type columnType;
		}
	}
}
