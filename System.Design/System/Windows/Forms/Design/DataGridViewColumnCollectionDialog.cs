using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;

namespace System.Windows.Forms.Design
{
	internal partial class DataGridViewColumnCollectionDialog : Form
	{
		internal DataGridViewColumnCollectionDialog()
		{
			this.InitializeComponent();
			this.dataGridViewPrivateCopy = new DataGridView();
			this.columnsPrivateCopy = this.dataGridViewPrivateCopy.Columns;
			this.columnsPrivateCopy.CollectionChanged += this.columnsPrivateCopy_CollectionChanged;
		}

		private Bitmap SelectedColumnsItemBitmap
		{
			get
			{
				if (DataGridViewColumnCollectionDialog.selectedColumnsItemBitmap == null)
				{
					DataGridViewColumnCollectionDialog.selectedColumnsItemBitmap = new Bitmap(typeof(DataGridViewColumnCollectionDialog), "DataGridViewColumnsDialog.selectedColumns.bmp");
					DataGridViewColumnCollectionDialog.selectedColumnsItemBitmap.MakeTransparent(Color.Red);
				}
				return DataGridViewColumnCollectionDialog.selectedColumnsItemBitmap;
			}
		}

		private void columnsPrivateCopy_CollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			if (this.columnCollectionChanging)
			{
				return;
			}
			this.PopulateSelectedColumns();
			if (e.Action == CollectionChangeAction.Add)
			{
				this.selectedColumns.SelectedIndex = this.columnsPrivateCopy.IndexOf((DataGridViewColumn)e.Element);
				DataGridViewColumnCollectionDialog.ListBoxItem listBoxItem = this.selectedColumns.SelectedItem as DataGridViewColumnCollectionDialog.ListBoxItem;
				this.userAddedColumns[listBoxItem.DataGridViewColumn] = true;
				this.columnsNames[listBoxItem.DataGridViewColumn] = listBoxItem.DataGridViewColumn.Name;
			}
			this.formIsDirty = true;
		}

		private void ColumnTypeChanged(DataGridViewColumnCollectionDialog.ListBoxItem item, Type newType)
		{
			DataGridViewColumn dataGridViewColumn = item.DataGridViewColumn;
			DataGridViewColumn dataGridViewColumn2 = Activator.CreateInstance(newType) as DataGridViewColumn;
			ITypeResolutionService typeResolutionService = this.liveDataGridView.Site.GetService(DataGridViewColumnCollectionDialog.iTypeResolutionServiceType) as ITypeResolutionService;
			ComponentDesigner componentDesignerForType = DataGridViewAddColumnDialog.GetComponentDesignerForType(typeResolutionService, newType);
			DataGridViewColumnCollectionDialog.CopyDataGridViewColumnProperties(dataGridViewColumn, dataGridViewColumn2);
			DataGridViewColumnCollectionDialog.CopyDataGridViewColumnState(dataGridViewColumn, dataGridViewColumn2);
			this.columnCollectionChanging = true;
			int selectedIndex = this.selectedColumns.SelectedIndex;
			this.selectedColumns.Focus();
			base.ActiveControl = this.selectedColumns;
			try
			{
				DataGridViewColumnCollectionDialog.ListBoxItem listBoxItem = (DataGridViewColumnCollectionDialog.ListBoxItem)this.selectedColumns.SelectedItem;
				bool flag = (bool)this.userAddedColumns[listBoxItem.DataGridViewColumn];
				string text = string.Empty;
				if (this.columnsNames.Contains(listBoxItem.DataGridViewColumn))
				{
					text = (string)this.columnsNames[listBoxItem.DataGridViewColumn];
					this.columnsNames.Remove(listBoxItem.DataGridViewColumn);
				}
				if (this.userAddedColumns.Contains(listBoxItem.DataGridViewColumn))
				{
					this.userAddedColumns.Remove(listBoxItem.DataGridViewColumn);
				}
				if (listBoxItem.DataGridViewColumnDesigner != null)
				{
					TypeDescriptor.RemoveAssociation(listBoxItem.DataGridViewColumn, listBoxItem.DataGridViewColumnDesigner);
				}
				this.selectedColumns.Items.RemoveAt(selectedIndex);
				this.selectedColumns.Items.Insert(selectedIndex, new DataGridViewColumnCollectionDialog.ListBoxItem(dataGridViewColumn2, this, componentDesignerForType));
				this.columnsPrivateCopy.RemoveAt(selectedIndex);
				dataGridViewColumn2.DisplayIndex = -1;
				this.columnsPrivateCopy.Insert(selectedIndex, dataGridViewColumn2);
				if (!string.IsNullOrEmpty(text))
				{
					this.columnsNames[dataGridViewColumn2] = text;
				}
				this.userAddedColumns[dataGridViewColumn2] = flag;
				this.FixColumnCollectionDisplayIndices();
				this.selectedColumns.SelectedIndex = selectedIndex;
				this.propertyGrid1.SelectedObject = this.selectedColumns.SelectedItem;
			}
			finally
			{
				this.columnCollectionChanging = false;
			}
		}

		private void CommitChanges()
		{
			if (this.formIsDirty)
			{
				try
				{
					IComponentChangeService componentChangeService = (IComponentChangeService)this.liveDataGridView.Site.GetService(DataGridViewColumnCollectionDialog.iComponentChangeServiceType);
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.liveDataGridView)["Columns"];
					IContainer container = ((this.liveDataGridView.Site != null) ? this.liveDataGridView.Site.Container : null);
					DataGridViewColumn[] array = new DataGridViewColumn[this.liveDataGridView.Columns.Count];
					this.liveDataGridView.Columns.CopyTo(array, 0);
					componentChangeService.OnComponentChanging(this.liveDataGridView, propertyDescriptor);
					this.liveDataGridView.Columns.Clear();
					componentChangeService.OnComponentChanged(this.liveDataGridView, propertyDescriptor, null, null);
					if (container != null)
					{
						for (int i = 0; i < array.Length; i++)
						{
							container.Remove(array[i]);
						}
					}
					DataGridViewColumn[] array2 = new DataGridViewColumn[this.columnsPrivateCopy.Count];
					bool[] array3 = new bool[this.columnsPrivateCopy.Count];
					string[] array4 = new string[this.columnsPrivateCopy.Count];
					for (int j = 0; j < this.columnsPrivateCopy.Count; j++)
					{
						DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this.columnsPrivateCopy[j].Clone();
						dataGridViewColumn.ContextMenuStrip = this.columnsPrivateCopy[j].ContextMenuStrip;
						array2[j] = dataGridViewColumn;
						array3[j] = (bool)this.userAddedColumns[this.columnsPrivateCopy[j]];
						array4[j] = (string)this.columnsNames[this.columnsPrivateCopy[j]];
					}
					if (container != null)
					{
						for (int k = 0; k < array2.Length; k++)
						{
							if (!string.IsNullOrEmpty(array4[k]) && DataGridViewColumnCollectionDialog.ValidateName(container, array4[k], array2[k]))
							{
								container.Add(array2[k], array4[k]);
							}
							else
							{
								container.Add(array2[k]);
							}
						}
					}
					componentChangeService.OnComponentChanging(this.liveDataGridView, propertyDescriptor);
					for (int l = 0; l < array2.Length; l++)
					{
						PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(array2[l])["DisplayIndex"];
						if (propertyDescriptor2 != null)
						{
							propertyDescriptor2.SetValue(array2[l], -1);
						}
						this.liveDataGridView.Columns.Add(array2[l]);
					}
					componentChangeService.OnComponentChanged(this.liveDataGridView, propertyDescriptor, null, null);
					for (int m = 0; m < array3.Length; m++)
					{
						PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(array2[m])["UserAddedColumn"];
						if (propertyDescriptor3 != null)
						{
							propertyDescriptor3.SetValue(array2[m], array3[m]);
						}
					}
				}
				catch (InvalidOperationException ex)
				{
					IUIService iuiservice = (IUIService)this.liveDataGridView.Site.GetService(typeof(IUIService));
					DataGridViewDesigner.ShowErrorDialog(iuiservice, ex, this.liveDataGridView);
					base.DialogResult = DialogResult.Cancel;
				}
			}
		}

		private void componentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (e.Component is DataGridViewColumnCollectionDialog.ListBoxItem && this.selectedColumns.Items.Contains(e.Component))
			{
				this.formIsDirty = true;
			}
		}

		private static void CopyDataGridViewColumnProperties(DataGridViewColumn srcColumn, DataGridViewColumn destColumn)
		{
			destColumn.AutoSizeMode = srcColumn.AutoSizeMode;
			destColumn.ContextMenuStrip = srcColumn.ContextMenuStrip;
			destColumn.DataPropertyName = srcColumn.DataPropertyName;
			if (srcColumn.HasDefaultCellStyle)
			{
				DataGridViewColumnCollectionDialog.CopyDefaultCellStyle(srcColumn, destColumn);
			}
			destColumn.DividerWidth = srcColumn.DividerWidth;
			destColumn.HeaderText = srcColumn.HeaderText;
			destColumn.MinimumWidth = srcColumn.MinimumWidth;
			destColumn.Name = srcColumn.Name;
			destColumn.SortMode = srcColumn.SortMode;
			destColumn.Tag = srcColumn.Tag;
			destColumn.ToolTipText = srcColumn.ToolTipText;
			destColumn.Width = srcColumn.Width;
			destColumn.FillWeight = srcColumn.FillWeight;
		}

		private static void CopyDataGridViewColumnState(DataGridViewColumn srcColumn, DataGridViewColumn destColumn)
		{
			destColumn.Frozen = srcColumn.Frozen;
			destColumn.Visible = srcColumn.Visible;
			destColumn.ReadOnly = srcColumn.ReadOnly;
			destColumn.Resizable = srcColumn.Resizable;
		}

		private static void CopyDefaultCellStyle(DataGridViewColumn srcColumn, DataGridViewColumn destColumn)
		{
			Type type = srcColumn.GetType();
			Type type2 = destColumn.GetType();
			if (type.IsAssignableFrom(type2) || type2.IsAssignableFrom(type))
			{
				destColumn.DefaultCellStyle = srcColumn.DefaultCellStyle;
				return;
			}
			DataGridViewColumn dataGridViewColumn = null;
			try
			{
				dataGridViewColumn = Activator.CreateInstance(type) as DataGridViewColumn;
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsCriticalException(ex))
				{
					throw;
				}
				dataGridViewColumn = null;
			}
			catch
			{
				dataGridViewColumn = null;
			}
			if (dataGridViewColumn == null || dataGridViewColumn.DefaultCellStyle.Alignment != srcColumn.DefaultCellStyle.Alignment)
			{
				destColumn.DefaultCellStyle.Alignment = srcColumn.DefaultCellStyle.Alignment;
			}
			if (dataGridViewColumn == null || !dataGridViewColumn.DefaultCellStyle.BackColor.Equals(srcColumn.DefaultCellStyle.BackColor))
			{
				destColumn.DefaultCellStyle.BackColor = srcColumn.DefaultCellStyle.BackColor;
			}
			if (dataGridViewColumn != null && srcColumn.DefaultCellStyle.Font != null && !srcColumn.DefaultCellStyle.Font.Equals(dataGridViewColumn.DefaultCellStyle.Font))
			{
				destColumn.DefaultCellStyle.Font = srcColumn.DefaultCellStyle.Font;
			}
			if (dataGridViewColumn == null || !dataGridViewColumn.DefaultCellStyle.ForeColor.Equals(srcColumn.DefaultCellStyle.ForeColor))
			{
				destColumn.DefaultCellStyle.ForeColor = srcColumn.DefaultCellStyle.ForeColor;
			}
			if (dataGridViewColumn == null || !dataGridViewColumn.DefaultCellStyle.Format.Equals(srcColumn.DefaultCellStyle.Format))
			{
				destColumn.DefaultCellStyle.Format = srcColumn.DefaultCellStyle.Format;
			}
			if (dataGridViewColumn == null || dataGridViewColumn.DefaultCellStyle.Padding != srcColumn.DefaultCellStyle.Padding)
			{
				destColumn.DefaultCellStyle.Padding = srcColumn.DefaultCellStyle.Padding;
			}
			if (dataGridViewColumn == null || !dataGridViewColumn.DefaultCellStyle.SelectionBackColor.Equals(srcColumn.DefaultCellStyle.SelectionBackColor))
			{
				destColumn.DefaultCellStyle.SelectionBackColor = srcColumn.DefaultCellStyle.SelectionBackColor;
			}
			if (dataGridViewColumn == null || !dataGridViewColumn.DefaultCellStyle.SelectionForeColor.Equals(srcColumn.DefaultCellStyle.SelectionForeColor))
			{
				destColumn.DefaultCellStyle.SelectionForeColor = srcColumn.DefaultCellStyle.SelectionForeColor;
			}
			if (dataGridViewColumn == null || dataGridViewColumn.DefaultCellStyle.WrapMode != srcColumn.DefaultCellStyle.WrapMode)
			{
				destColumn.DefaultCellStyle.WrapMode = srcColumn.DefaultCellStyle.WrapMode;
			}
			if (!srcColumn.DefaultCellStyle.IsNullValueDefault)
			{
				object nullValue = srcColumn.DefaultCellStyle.NullValue;
				object nullValue2 = destColumn.DefaultCellStyle.NullValue;
				if (nullValue != null && nullValue2 != null && nullValue.GetType() == nullValue2.GetType())
				{
					destColumn.DefaultCellStyle.NullValue = nullValue;
				}
			}
		}

		private void FixColumnCollectionDisplayIndices()
		{
			for (int i = 0; i < this.columnsPrivateCopy.Count; i++)
			{
				this.columnsPrivateCopy[i].DisplayIndex = i;
			}
		}

		private void HookComponentChangedEventHandler(IComponentChangeService componentChangeService)
		{
			if (componentChangeService != null)
			{
				componentChangeService.ComponentChanged += this.componentChanged;
			}
		}

		private static bool IsColumnAddedByUser(DataGridViewColumn col)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(col)["UserAddedColumn"];
			return propertyDescriptor != null && (bool)propertyDescriptor.GetValue(col);
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			this.CommitChanges();
		}

		private void moveDown_Click(object sender, EventArgs e)
		{
			int selectedIndex = this.selectedColumns.SelectedIndex;
			this.columnCollectionChanging = true;
			try
			{
				DataGridViewColumnCollectionDialog.ListBoxItem listBoxItem = (DataGridViewColumnCollectionDialog.ListBoxItem)this.selectedColumns.SelectedItem;
				this.selectedColumns.Items.RemoveAt(selectedIndex);
				this.selectedColumns.Items.Insert(selectedIndex + 1, listBoxItem);
				this.columnsPrivateCopy.RemoveAt(selectedIndex);
				if (listBoxItem.DataGridViewColumn.Frozen)
				{
					this.columnsPrivateCopy[selectedIndex].Frozen = true;
				}
				listBoxItem.DataGridViewColumn.DisplayIndex = -1;
				this.columnsPrivateCopy.Insert(selectedIndex + 1, listBoxItem.DataGridViewColumn);
				this.FixColumnCollectionDisplayIndices();
			}
			finally
			{
				this.columnCollectionChanging = false;
			}
			this.formIsDirty = true;
			this.selectedColumns.SelectedIndex = selectedIndex + 1;
			this.moveUp.Enabled = this.selectedColumns.SelectedIndex > 0;
			this.moveDown.Enabled = this.selectedColumns.SelectedIndex < this.selectedColumns.Items.Count - 1;
		}

		private void moveUp_Click(object sender, EventArgs e)
		{
			int selectedIndex = this.selectedColumns.SelectedIndex;
			this.columnCollectionChanging = true;
			try
			{
				DataGridViewColumnCollectionDialog.ListBoxItem listBoxItem = (DataGridViewColumnCollectionDialog.ListBoxItem)this.selectedColumns.Items[selectedIndex - 1];
				this.selectedColumns.Items.RemoveAt(selectedIndex - 1);
				this.selectedColumns.Items.Insert(selectedIndex, listBoxItem);
				this.columnsPrivateCopy.RemoveAt(selectedIndex - 1);
				if (listBoxItem.DataGridViewColumn.Frozen && !this.columnsPrivateCopy[selectedIndex - 1].Frozen)
				{
					listBoxItem.DataGridViewColumn.Frozen = false;
				}
				listBoxItem.DataGridViewColumn.DisplayIndex = -1;
				this.columnsPrivateCopy.Insert(selectedIndex, listBoxItem.DataGridViewColumn);
				this.FixColumnCollectionDisplayIndices();
			}
			finally
			{
				this.columnCollectionChanging = false;
			}
			this.formIsDirty = true;
			this.selectedColumns.SelectedIndex = selectedIndex - 1;
			this.moveUp.Enabled = this.selectedColumns.SelectedIndex > 0;
			this.moveDown.Enabled = this.selectedColumns.SelectedIndex < this.selectedColumns.Items.Count - 1;
			if (this.selectedColumns.SelectedIndex != -1 && this.selectedColumns.TopIndex > this.selectedColumns.SelectedIndex)
			{
				this.selectedColumns.TopIndex = this.selectedColumns.SelectedIndex;
			}
		}

		private void DataGridViewColumnCollectionDialog_Closed(object sender, EventArgs e)
		{
			for (int i = 0; i < this.selectedColumns.Items.Count; i++)
			{
				DataGridViewColumnCollectionDialog.ListBoxItem listBoxItem = this.selectedColumns.Items[i] as DataGridViewColumnCollectionDialog.ListBoxItem;
				if (listBoxItem.DataGridViewColumnDesigner != null)
				{
					TypeDescriptor.RemoveAssociation(listBoxItem.DataGridViewColumn, listBoxItem.DataGridViewColumnDesigner);
				}
			}
			this.columnsNames = null;
			this.userAddedColumns = null;
		}

		private void DataGridViewColumnCollectionDialog_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			this.DataGridViewColumnCollectionDialog_HelpRequestHandled();
		}

		private void DataGridViewColumnCollectionDialog_HelpRequested(object sender, HelpEventArgs e)
		{
			this.DataGridViewColumnCollectionDialog_HelpRequestHandled();
			e.Handled = true;
		}

		private void DataGridViewColumnCollectionDialog_HelpRequestHandled()
		{
			IHelpService helpService = this.liveDataGridView.Site.GetService(DataGridViewColumnCollectionDialog.iHelpServiceType) as IHelpService;
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword("vs.DataGridViewColumnCollectionDialog");
			}
		}

		private void DataGridViewColumnCollectionDialog_Load(object sender, EventArgs e)
		{
			Font font = Control.DefaultFont;
			IUIService iuiservice = (IUIService)this.liveDataGridView.Site.GetService(DataGridViewColumnCollectionDialog.iUIServiceType);
			if (iuiservice != null)
			{
				font = (Font)iuiservice.Styles["DialogFont"];
			}
			this.Font = font;
			this.selectedColumns.SelectedIndex = Math.Min(0, this.selectedColumns.Items.Count - 1);
			this.moveUp.Enabled = this.selectedColumns.SelectedIndex > 0;
			this.moveDown.Enabled = this.selectedColumns.SelectedIndex < this.selectedColumns.Items.Count - 1;
			this.deleteButton.Enabled = this.selectedColumns.Items.Count > 0 && this.selectedColumns.SelectedIndex != -1;
			this.propertyGrid1.SelectedObject = this.selectedColumns.SelectedItem;
			this.selectedColumns.ItemHeight = this.Font.Height + 4;
			base.ActiveControl = this.selectedColumns;
			this.SetSelectedColumnsHorizontalExtent();
			this.selectedColumns.Focus();
			this.formIsDirty = false;
		}

		private void deleteButton_Click(object sender, EventArgs e)
		{
			int selectedIndex = this.selectedColumns.SelectedIndex;
			this.columnsNames.Remove(this.columnsPrivateCopy[selectedIndex]);
			this.userAddedColumns.Remove(this.columnsPrivateCopy[selectedIndex]);
			this.columnsPrivateCopy.RemoveAt(selectedIndex);
			this.selectedColumns.SelectedIndex = Math.Min(this.selectedColumns.Items.Count - 1, selectedIndex);
			this.moveUp.Enabled = this.selectedColumns.SelectedIndex > 0;
			this.moveDown.Enabled = this.selectedColumns.SelectedIndex < this.selectedColumns.Items.Count - 1;
			this.deleteButton.Enabled = this.selectedColumns.Items.Count > 0 && this.selectedColumns.SelectedIndex != -1;
			this.propertyGrid1.SelectedObject = this.selectedColumns.SelectedItem;
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			int num;
			if (this.selectedColumns.SelectedIndex == -1)
			{
				num = this.selectedColumns.Items.Count;
			}
			else
			{
				num = this.selectedColumns.SelectedIndex + 1;
			}
			if (this.addColumnDialog == null)
			{
				this.addColumnDialog = new DataGridViewAddColumnDialog(this.columnsPrivateCopy, this.liveDataGridView);
				this.addColumnDialog.StartPosition = FormStartPosition.CenterParent;
			}
			this.addColumnDialog.Start(num, false);
			this.addColumnDialog.ShowDialog(this);
		}

		private void PopulateSelectedColumns()
		{
			int selectedIndex = this.selectedColumns.SelectedIndex;
			for (int i = 0; i < this.selectedColumns.Items.Count; i++)
			{
				DataGridViewColumnCollectionDialog.ListBoxItem listBoxItem = this.selectedColumns.Items[i] as DataGridViewColumnCollectionDialog.ListBoxItem;
				if (listBoxItem.DataGridViewColumnDesigner != null)
				{
					TypeDescriptor.RemoveAssociation(listBoxItem.DataGridViewColumn, listBoxItem.DataGridViewColumnDesigner);
				}
			}
			this.selectedColumns.Items.Clear();
			ITypeResolutionService typeResolutionService = (ITypeResolutionService)this.liveDataGridView.Site.GetService(DataGridViewColumnCollectionDialog.iTypeResolutionServiceType);
			for (int j = 0; j < this.columnsPrivateCopy.Count; j++)
			{
				ComponentDesigner componentDesignerForType = DataGridViewAddColumnDialog.GetComponentDesignerForType(typeResolutionService, this.columnsPrivateCopy[j].GetType());
				this.selectedColumns.Items.Add(new DataGridViewColumnCollectionDialog.ListBoxItem(this.columnsPrivateCopy[j], this, componentDesignerForType));
			}
			this.selectedColumns.SelectedIndex = Math.Min(selectedIndex, this.selectedColumns.Items.Count - 1);
			this.SetSelectedColumnsHorizontalExtent();
			if (this.selectedColumns.Items.Count == 0)
			{
				this.propertyGridLabel.Text = SR.GetString("DataGridViewProperties");
			}
		}

		private void propertyGrid1_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
		{
			if (!this.columnCollectionChanging)
			{
				this.formIsDirty = true;
				if (e.ChangedItem.PropertyDescriptor.Name.Equals("HeaderText"))
				{
					int selectedIndex = this.selectedColumns.SelectedIndex;
					Rectangle rectangle = new Rectangle(0, selectedIndex * this.selectedColumns.ItemHeight, this.selectedColumns.Width, this.selectedColumns.ItemHeight);
					this.columnCollectionChanging = true;
					try
					{
						this.selectedColumns.Items[selectedIndex] = this.selectedColumns.Items[selectedIndex];
					}
					finally
					{
						this.columnCollectionChanging = false;
					}
					this.selectedColumns.Invalidate(rectangle);
					this.SetSelectedColumnsHorizontalExtent();
					return;
				}
				if (e.ChangedItem.PropertyDescriptor.Name.Equals("DataPropertyName"))
				{
					DataGridViewColumn dataGridViewColumn = ((DataGridViewColumnCollectionDialog.ListBoxItem)this.selectedColumns.SelectedItem).DataGridViewColumn;
					if (string.IsNullOrEmpty(dataGridViewColumn.DataPropertyName))
					{
						this.propertyGridLabel.Text = SR.GetString("DataGridViewUnboundColumnProperties");
						return;
					}
					this.propertyGridLabel.Text = SR.GetString("DataGridViewBoundColumnProperties");
					return;
				}
				else if (e.ChangedItem.PropertyDescriptor.Name.Equals("Name"))
				{
					DataGridViewColumn dataGridViewColumn2 = ((DataGridViewColumnCollectionDialog.ListBoxItem)this.selectedColumns.SelectedItem).DataGridViewColumn;
					this.columnsNames[dataGridViewColumn2] = dataGridViewColumn2.Name;
				}
			}
		}

		private void selectedColumns_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0)
			{
				return;
			}
			DataGridViewColumnCollectionDialog.ListBoxItem listBoxItem = this.selectedColumns.Items[e.Index] as DataGridViewColumnCollectionDialog.ListBoxItem;
			e.Graphics.DrawImage(listBoxItem.ToolboxBitmap, e.Bounds.X + 2, e.Bounds.Y + 2, listBoxItem.ToolboxBitmap.Width, listBoxItem.ToolboxBitmap.Height);
			Rectangle bounds = e.Bounds;
			bounds.Width -= listBoxItem.ToolboxBitmap.Width + 4;
			bounds.X += listBoxItem.ToolboxBitmap.Width + 4;
			bounds.Y += 2;
			bounds.Height -= 4;
			Brush brush = new SolidBrush(e.BackColor);
			Brush brush2 = new SolidBrush(e.ForeColor);
			Brush brush3 = new SolidBrush(this.selectedColumns.BackColor);
			string text = ((DataGridViewColumnCollectionDialog.ListBoxItem)this.selectedColumns.Items[e.Index]).ToString();
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				int width = Size.Ceiling(e.Graphics.MeasureString(text, e.Font, new SizeF((float)bounds.Width, (float)bounds.Height))).Width;
				Rectangle rectangle = new Rectangle(bounds.X, e.Bounds.Y + 1, width + 3, e.Bounds.Height - 2);
				e.Graphics.FillRectangle(brush, rectangle);
				rectangle.Inflate(-1, -1);
				e.Graphics.DrawString(text, e.Font, brush2, rectangle);
				rectangle.Inflate(1, 1);
				if (this.selectedColumns.Focused)
				{
					ControlPaint.DrawFocusRectangle(e.Graphics, rectangle, e.ForeColor, e.BackColor);
				}
				e.Graphics.FillRectangle(brush3, new Rectangle(rectangle.Right + 1, e.Bounds.Y, e.Bounds.Width - rectangle.Right - 1, e.Bounds.Height));
			}
			else
			{
				e.Graphics.FillRectangle(brush3, new Rectangle(bounds.X, e.Bounds.Y, e.Bounds.Width - bounds.X, e.Bounds.Height));
				e.Graphics.DrawString(text, e.Font, brush2, bounds);
			}
			brush.Dispose();
			brush3.Dispose();
			brush2.Dispose();
		}

		private void selectedColumns_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Modifiers == Keys.None && e.KeyCode == Keys.F4)
			{
				this.propertyGrid1.Focus();
				e.Handled = true;
			}
		}

		private void selectedColumns_KeyPress(object sender, KeyPressEventArgs e)
		{
			Keys modifierKeys = Control.ModifierKeys;
			if ((modifierKeys & Keys.Control) != Keys.None)
			{
				e.Handled = true;
			}
		}

		private void selectedColumns_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.columnCollectionChanging)
			{
				return;
			}
			this.propertyGrid1.SelectedObject = this.selectedColumns.SelectedItem;
			this.moveDown.Enabled = this.selectedColumns.Items.Count > 0 && this.selectedColumns.SelectedIndex != this.selectedColumns.Items.Count - 1;
			this.moveUp.Enabled = this.selectedColumns.Items.Count > 0 && this.selectedColumns.SelectedIndex > 0;
			this.deleteButton.Enabled = this.selectedColumns.Items.Count > 0 && this.selectedColumns.SelectedIndex != -1;
			if (this.selectedColumns.SelectedItem == null)
			{
				this.propertyGridLabel.Text = SR.GetString("DataGridViewProperties");
				return;
			}
			DataGridViewColumn dataGridViewColumn = ((DataGridViewColumnCollectionDialog.ListBoxItem)this.selectedColumns.SelectedItem).DataGridViewColumn;
			if (string.IsNullOrEmpty(dataGridViewColumn.DataPropertyName))
			{
				this.propertyGridLabel.Text = SR.GetString("DataGridViewUnboundColumnProperties");
				return;
			}
			this.propertyGridLabel.Text = SR.GetString("DataGridViewBoundColumnProperties");
		}

		internal void SetLiveDataGridView(DataGridView dataGridView)
		{
			IComponentChangeService componentChangeService = null;
			if (dataGridView.Site != null)
			{
				componentChangeService = (IComponentChangeService)dataGridView.Site.GetService(DataGridViewColumnCollectionDialog.iComponentChangeServiceType);
			}
			if (componentChangeService != this.compChangeService)
			{
				this.UnhookComponentChangedEventHandler(this.compChangeService);
				this.compChangeService = componentChangeService;
				this.HookComponentChangedEventHandler(this.compChangeService);
			}
			this.liveDataGridView = dataGridView;
			this.dataGridViewPrivateCopy.Site = dataGridView.Site;
			this.dataGridViewPrivateCopy.AutoSizeColumnsMode = dataGridView.AutoSizeColumnsMode;
			this.dataGridViewPrivateCopy.DataSource = dataGridView.DataSource;
			this.dataGridViewPrivateCopy.DataMember = dataGridView.DataMember;
			this.columnsNames = new Hashtable(this.columnsPrivateCopy.Count);
			this.columnsPrivateCopy.Clear();
			this.userAddedColumns = new Hashtable(this.liveDataGridView.Columns.Count);
			this.columnCollectionChanging = true;
			try
			{
				for (int i = 0; i < this.liveDataGridView.Columns.Count; i++)
				{
					DataGridViewColumn dataGridViewColumn = this.liveDataGridView.Columns[i];
					DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn)dataGridViewColumn.Clone();
					dataGridViewColumn2.ContextMenuStrip = this.liveDataGridView.Columns[i].ContextMenuStrip;
					dataGridViewColumn2.DisplayIndex = -1;
					this.columnsPrivateCopy.Add(dataGridViewColumn2);
					if (dataGridViewColumn.Site != null)
					{
						this.columnsNames[dataGridViewColumn2] = dataGridViewColumn.Site.Name;
					}
					this.userAddedColumns[dataGridViewColumn2] = DataGridViewColumnCollectionDialog.IsColumnAddedByUser(this.liveDataGridView.Columns[i]);
				}
			}
			finally
			{
				this.columnCollectionChanging = false;
			}
			this.PopulateSelectedColumns();
			this.propertyGrid1.Site = new DataGridViewComponentPropertyGridSite(this.liveDataGridView.Site, this.liveDataGridView);
			this.propertyGrid1.SelectedObject = this.selectedColumns.SelectedItem;
		}

		private void SetSelectedColumnsHorizontalExtent()
		{
			int num = 0;
			for (int i = 0; i < this.selectedColumns.Items.Count; i++)
			{
				int width = TextRenderer.MeasureText(this.selectedColumns.Items[i].ToString(), this.selectedColumns.Font).Width;
				num = Math.Max(num, width);
			}
			this.selectedColumns.HorizontalExtent = this.SelectedColumnsItemBitmap.Width + 4 + num + 3;
		}

		private void UnhookComponentChangedEventHandler(IComponentChangeService componentChangeService)
		{
			if (componentChangeService != null)
			{
				componentChangeService.ComponentChanged -= this.componentChanged;
			}
		}

		private static bool ValidateName(IContainer container, string siteName, IComponent component)
		{
			ComponentCollection componentCollection = container.Components;
			if (componentCollection == null)
			{
				return true;
			}
			for (int i = 0; i < componentCollection.Count; i++)
			{
				IComponent component2 = componentCollection[i];
				if (component2 != null && component2.Site != null)
				{
					ISite site = component2.Site;
					if (site != null && site.Name != null && string.Equals(site.Name, siteName, StringComparison.OrdinalIgnoreCase) && site.Component != component)
					{
						return false;
					}
				}
			}
			return true;
		}

		private const int LISTBOXITEMHEIGHT = 17;

		private const int OWNERDRAWHORIZONTALBUFFER = 3;

		private const int OWNERDRAWVERTICALBUFFER = 4;

		private const int OWNERDRAWITEMIMAGEBUFFER = 2;

		private DataGridView liveDataGridView;

		private IComponentChangeService compChangeService;

		private DataGridView dataGridViewPrivateCopy;

		private DataGridViewColumnCollection columnsPrivateCopy;

		private Hashtable columnsNames;

		private DataGridViewAddColumnDialog addColumnDialog;

		private static Bitmap selectedColumnsItemBitmap;

		private static ColorMap[] colorMap = new ColorMap[]
		{
			new ColorMap()
		};

		private static Type iTypeResolutionServiceType = typeof(ITypeResolutionService);

		private static Type iTypeDiscoveryServiceType = typeof(ITypeDiscoveryService);

		private static Type iComponentChangeServiceType = typeof(IComponentChangeService);

		private static Type iHelpServiceType = typeof(IHelpService);

		private static Type iUIServiceType = typeof(IUIService);

		private static Type toolboxBitmapAttributeType = typeof(ToolboxBitmapAttribute);

		private bool columnCollectionChanging;

		private bool formIsDirty;

		private Hashtable userAddedColumns;

		internal class ListBoxItem : ICustomTypeDescriptor, IComponent, IDisposable
		{
			public ListBoxItem(DataGridViewColumn column, DataGridViewColumnCollectionDialog owner, ComponentDesigner compDesigner)
			{
				this.column = column;
				this.owner = owner;
				this.compDesigner = compDesigner;
				if (this.compDesigner != null)
				{
					this.compDesigner.Initialize(column);
					TypeDescriptor.CreateAssociation(this.column, this.compDesigner);
				}
				ToolboxBitmapAttribute toolboxBitmapAttribute = TypeDescriptor.GetAttributes(column)[DataGridViewColumnCollectionDialog.toolboxBitmapAttributeType] as ToolboxBitmapAttribute;
				if (toolboxBitmapAttribute != null)
				{
					this.toolboxBitmap = toolboxBitmapAttribute.GetImage(column, false);
				}
				else
				{
					this.toolboxBitmap = this.owner.SelectedColumnsItemBitmap;
				}
				DataGridViewColumnDesigner dataGridViewColumnDesigner = compDesigner as DataGridViewColumnDesigner;
				if (dataGridViewColumnDesigner != null)
				{
					dataGridViewColumnDesigner.LiveDataGridView = this.owner.liveDataGridView;
				}
			}

			public DataGridViewColumn DataGridViewColumn
			{
				get
				{
					return this.column;
				}
			}

			public ComponentDesigner DataGridViewColumnDesigner
			{
				get
				{
					return this.compDesigner;
				}
			}

			public DataGridViewColumnCollectionDialog Owner
			{
				get
				{
					return this.owner;
				}
			}

			public Image ToolboxBitmap
			{
				get
				{
					return this.toolboxBitmap;
				}
			}

			public override string ToString()
			{
				return this.column.HeaderText;
			}

			AttributeCollection ICustomTypeDescriptor.GetAttributes()
			{
				return TypeDescriptor.GetAttributes(this.column);
			}

			string ICustomTypeDescriptor.GetClassName()
			{
				return TypeDescriptor.GetClassName(this.column);
			}

			string ICustomTypeDescriptor.GetComponentName()
			{
				return TypeDescriptor.GetComponentName(this.column);
			}

			TypeConverter ICustomTypeDescriptor.GetConverter()
			{
				return TypeDescriptor.GetConverter(this.column);
			}

			EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
			{
				return TypeDescriptor.GetDefaultEvent(this.column);
			}

			PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
			{
				return TypeDescriptor.GetDefaultProperty(this.column);
			}

			object ICustomTypeDescriptor.GetEditor(Type type)
			{
				return TypeDescriptor.GetEditor(this.column, type);
			}

			EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
			{
				return TypeDescriptor.GetEvents(this.column);
			}

			EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attrs)
			{
				return TypeDescriptor.GetEvents(this.column, attrs);
			}

			PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
			{
				return ((ICustomTypeDescriptor)this).GetProperties(null);
			}

			PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attrs)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this.column);
				PropertyDescriptor[] array;
				if (this.compDesigner != null)
				{
					Hashtable hashtable = new Hashtable();
					for (int i = 0; i < properties.Count; i++)
					{
						hashtable.Add(properties[i].Name, properties[i]);
					}
					((IDesignerFilter)this.compDesigner).PreFilterProperties(hashtable);
					array = new PropertyDescriptor[hashtable.Count + 1];
					hashtable.Values.CopyTo(array, 0);
				}
				else
				{
					array = new PropertyDescriptor[properties.Count + 1];
					properties.CopyTo(array, 0);
				}
				array[array.Length - 1] = new DataGridViewColumnCollectionDialog.ColumnTypePropertyDescriptor();
				return new PropertyDescriptorCollection(array);
			}

			object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
			{
				if (pd == null)
				{
					return this.column;
				}
				if (pd is DataGridViewColumnCollectionDialog.ColumnTypePropertyDescriptor)
				{
					return this;
				}
				return this.column;
			}

			ISite IComponent.Site
			{
				get
				{
					return this.owner.liveDataGridView.Site;
				}
				set
				{
				}
			}

			event EventHandler IComponent.Disposed
			{
				add
				{
				}
				remove
				{
				}
			}

			void IDisposable.Dispose()
			{
			}

			private DataGridViewColumn column;

			private DataGridViewColumnCollectionDialog owner;

			private ComponentDesigner compDesigner;

			private Image toolboxBitmap;
		}

		private class ColumnTypePropertyDescriptor : PropertyDescriptor
		{
			public ColumnTypePropertyDescriptor()
				: base("ColumnType", null)
			{
			}

			public override AttributeCollection Attributes
			{
				get
				{
					EditorAttribute editorAttribute = new EditorAttribute("System.Windows.Forms.Design.DataGridViewColumnTypeEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor));
					DescriptionAttribute descriptionAttribute = new DescriptionAttribute(SR.GetString("DataGridViewColumnTypePropertyDescription"));
					CategoryAttribute design = CategoryAttribute.Design;
					Attribute[] array = new Attribute[] { editorAttribute, descriptionAttribute, design };
					return new AttributeCollection(array);
				}
			}

			public override Type ComponentType
			{
				get
				{
					return typeof(DataGridViewColumnCollectionDialog.ListBoxItem);
				}
			}

			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			public override Type PropertyType
			{
				get
				{
					return typeof(Type);
				}
			}

			public override bool CanResetValue(object component)
			{
				return false;
			}

			public override object GetValue(object component)
			{
				if (component == null)
				{
					return null;
				}
				DataGridViewColumnCollectionDialog.ListBoxItem listBoxItem = (DataGridViewColumnCollectionDialog.ListBoxItem)component;
				return listBoxItem.DataGridViewColumn.GetType().Name;
			}

			public override void ResetValue(object component)
			{
			}

			public override void SetValue(object component, object value)
			{
				DataGridViewColumnCollectionDialog.ListBoxItem listBoxItem = (DataGridViewColumnCollectionDialog.ListBoxItem)component;
				Type type = value as Type;
				if (listBoxItem.DataGridViewColumn.GetType() != type)
				{
					listBoxItem.Owner.ColumnTypeChanged(listBoxItem, type);
					this.OnValueChanged(component, EventArgs.Empty);
				}
			}

			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}
		}
	}
}
