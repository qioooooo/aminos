using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Text;

namespace System.Windows.Forms.Design
{
	internal class ToolStripCollectionEditor : CollectionEditor
	{
		public ToolStripCollectionEditor()
			: base(typeof(ToolStripItemCollection))
		{
		}

		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			return new ToolStripCollectionEditor.ToolStripItemEditorForm(this);
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.ComponentModel.ToolStripCollectionEditor";
			}
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			ToolStripDesigner toolStripDesigner = null;
			if (provider != null)
			{
				ISelectionService selectionService = (ISelectionService)provider.GetService(typeof(ISelectionService));
				if (selectionService != null)
				{
					object obj = selectionService.PrimarySelection;
					if (obj is ToolStripDropDownItem)
					{
						obj = ((ToolStripDropDownItem)obj).Owner;
					}
					if (obj is ToolStrip)
					{
						IDesignerHost designerHost = (IDesignerHost)provider.GetService(typeof(IDesignerHost));
						if (designerHost != null)
						{
							toolStripDesigner = designerHost.GetDesigner((IComponent)obj) as ToolStripDesigner;
						}
					}
				}
			}
			object obj2;
			try
			{
				if (toolStripDesigner != null)
				{
					toolStripDesigner.EditingCollection = true;
				}
				obj2 = base.EditValue(context, provider, value);
			}
			finally
			{
				if (toolStripDesigner != null)
				{
					toolStripDesigner.EditingCollection = false;
				}
			}
			return obj2;
		}

		protected class ToolStripItemEditorForm : CollectionEditor.CollectionForm
		{
			internal ToolStripItemEditorForm(CollectionEditor parent)
				: base(parent)
			{
				this.editor = (ToolStripCollectionEditor)parent;
				this.InitializeComponent();
				base.ActiveControl = this.listBoxItems;
				this._originalText = this.Text;
				base.SetStyle(ControlStyles.ResizeRedraw, true);
			}

			internal ToolStripItemCollection Collection
			{
				set
				{
					if (value != this._targetToolStripCollection)
					{
						if (this._itemList != null)
						{
							this._itemList.Clear();
						}
						if (value != null)
						{
							if (base.Context != null)
							{
								this._itemList = new ToolStripCollectionEditor.ToolStripItemEditorForm.EditorItemCollection(this, this.listBoxItems.Items, value);
								ToolStrip toolStrip = ToolStripCollectionEditor.ToolStripItemEditorForm.ToolStripFromObject(base.Context.Instance);
								this._itemList.Add(toolStrip);
								ToolStripItem toolStripItem = base.Context.Instance as ToolStripItem;
								if (toolStripItem != null && toolStripItem.Site != null)
								{
									this.Text = string.Concat(new string[]
									{
										this._originalText,
										" (",
										toolStripItem.Site.Name,
										".",
										base.Context.PropertyDescriptor.Name,
										")"
									});
								}
								foreach (object obj in value)
								{
									ToolStripItem toolStripItem2 = (ToolStripItem)obj;
									if (!(toolStripItem2 is DesignerToolStripControlHost))
									{
										this._itemList.Add(toolStripItem2);
									}
								}
								IComponentChangeService componentChangeService = (IComponentChangeService)base.Context.GetService(typeof(IComponentChangeService));
								if (componentChangeService != null)
								{
									componentChangeService.ComponentChanged += this.OnComponentChanged;
								}
								this.selectedItemProps.Site = new CollectionEditor.PropertyGridSite(base.Context, this.selectedItemProps);
							}
						}
						else
						{
							if (this._componentChangeSvc != null)
							{
								this._componentChangeSvc.ComponentChanged -= this.OnComponentChanged;
							}
							this._componentChangeSvc = null;
							this.selectedItemProps.Site = null;
						}
						this._targetToolStripCollection = value;
					}
				}
			}

			private IComponentChangeService ComponentChangeService
			{
				get
				{
					if (this._componentChangeSvc == null && base.Context != null)
					{
						this._componentChangeSvc = (IComponentChangeService)base.Context.GetService(typeof(IComponentChangeService));
					}
					return this._componentChangeSvc;
				}
			}

			private void InitializeComponent()
			{
				ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ToolStripCollectionEditor.ToolStripItemEditorForm));
				this.btnCancel = new Button();
				this.btnOK = new Button();
				this.tableLayoutPanel = new TableLayoutPanel();
				this.addTableLayoutPanel = new TableLayoutPanel();
				this.btnAddNew = new Button();
				this.newItemTypes = new ToolStripCollectionEditor.ToolStripItemEditorForm.ImageComboBox();
				this.okCancelTableLayoutPanel = new TableLayoutPanel();
				this.lblItems = new Label();
				this.selectedItemName = new Label();
				this.selectedItemProps = new VsPropertyGrid(base.Context);
				this.lblMembers = new Label();
				this.listBoxItems = new CollectionEditor.FilterListBox();
				this.btnMoveUp = new Button();
				this.btnMoveDown = new Button();
				this.btnRemove = new Button();
				this.tableLayoutPanel.SuspendLayout();
				this.addTableLayoutPanel.SuspendLayout();
				this.okCancelTableLayoutPanel.SuspendLayout();
				base.SuspendLayout();
				componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
				this.btnCancel.DialogResult = DialogResult.Cancel;
				this.btnCancel.Margin = new Padding(3, 0, 0, 0);
				this.btnCancel.Name = "btnCancel";
				componentResourceManager.ApplyResources(this.btnOK, "btnOK");
				this.btnOK.Margin = new Padding(0, 0, 3, 0);
				this.btnOK.Name = "btnOK";
				componentResourceManager.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
				this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 274f));
				this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
				this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
				this.tableLayoutPanel.Controls.Add(this.addTableLayoutPanel, 0, 1);
				this.tableLayoutPanel.Controls.Add(this.okCancelTableLayoutPanel, 0, 6);
				this.tableLayoutPanel.Controls.Add(this.lblItems, 0, 0);
				this.tableLayoutPanel.Controls.Add(this.selectedItemName, 2, 0);
				this.tableLayoutPanel.Controls.Add(this.selectedItemProps, 2, 1);
				this.tableLayoutPanel.Controls.Add(this.lblMembers, 0, 2);
				this.tableLayoutPanel.Controls.Add(this.listBoxItems, 0, 3);
				this.tableLayoutPanel.Controls.Add(this.btnMoveUp, 1, 3);
				this.tableLayoutPanel.Controls.Add(this.btnMoveDown, 1, 4);
				this.tableLayoutPanel.Controls.Add(this.btnRemove, 1, 5);
				this.tableLayoutPanel.Name = "tableLayoutPanel";
				this.tableLayoutPanel.RowStyles.Add(new RowStyle());
				this.tableLayoutPanel.RowStyles.Add(new RowStyle());
				this.tableLayoutPanel.RowStyles.Add(new RowStyle());
				this.tableLayoutPanel.RowStyles.Add(new RowStyle());
				this.tableLayoutPanel.RowStyles.Add(new RowStyle());
				this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
				this.tableLayoutPanel.RowStyles.Add(new RowStyle());
				componentResourceManager.ApplyResources(this.addTableLayoutPanel, "addTableLayoutPanel");
				this.addTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
				this.addTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
				this.addTableLayoutPanel.Controls.Add(this.btnAddNew, 1, 0);
				this.addTableLayoutPanel.Controls.Add(this.newItemTypes, 0, 0);
				this.addTableLayoutPanel.Margin = new Padding(0, 3, 3, 3);
				this.addTableLayoutPanel.Name = "addTableLayoutPanel";
				this.addTableLayoutPanel.RowStyles.Add(new RowStyle());
				componentResourceManager.ApplyResources(this.btnAddNew, "btnAddNew");
				this.btnAddNew.Margin = new Padding(3, 0, 0, 0);
				this.btnAddNew.Name = "btnAddNew";
				componentResourceManager.ApplyResources(this.newItemTypes, "newItemTypes");
				this.newItemTypes.DropDownStyle = ComboBoxStyle.DropDownList;
				this.newItemTypes.FormattingEnabled = true;
				this.newItemTypes.Margin = new Padding(0, 0, 3, 0);
				this.newItemTypes.Name = "newItemTypes";
				this.newItemTypes.DrawMode = DrawMode.OwnerDrawVariable;
				componentResourceManager.ApplyResources(this.okCancelTableLayoutPanel, "okCancelTableLayoutPanel");
				this.tableLayoutPanel.SetColumnSpan(this.okCancelTableLayoutPanel, 3);
				this.okCancelTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				this.okCancelTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				this.okCancelTableLayoutPanel.Controls.Add(this.btnOK, 0, 0);
				this.okCancelTableLayoutPanel.Controls.Add(this.btnCancel, 1, 0);
				this.okCancelTableLayoutPanel.Margin = new Padding(3, 6, 0, 0);
				this.okCancelTableLayoutPanel.Name = "okCancelTableLayoutPanel";
				this.okCancelTableLayoutPanel.RowStyles.Add(new RowStyle());
				componentResourceManager.ApplyResources(this.lblItems, "lblItems");
				this.lblItems.Margin = new Padding(0, 3, 3, 0);
				this.lblItems.Name = "lblItems";
				componentResourceManager.ApplyResources(this.selectedItemName, "selectedItemName");
				this.selectedItemName.Margin = new Padding(3, 3, 3, 0);
				this.selectedItemName.Name = "selectedItemName";
				this.selectedItemProps.CommandsVisibleIfAvailable = false;
				componentResourceManager.ApplyResources(this.selectedItemProps, "selectedItemProps");
				this.selectedItemProps.Margin = new Padding(3, 3, 0, 3);
				this.selectedItemProps.Name = "selectedItemProps";
				this.tableLayoutPanel.SetRowSpan(this.selectedItemProps, 5);
				componentResourceManager.ApplyResources(this.lblMembers, "lblMembers");
				this.lblMembers.Margin = new Padding(0, 3, 3, 0);
				this.lblMembers.Name = "lblMembers";
				componentResourceManager.ApplyResources(this.listBoxItems, "listBoxItems");
				this.listBoxItems.DrawMode = DrawMode.OwnerDrawVariable;
				this.listBoxItems.FormattingEnabled = true;
				this.listBoxItems.Margin = new Padding(0, 3, 3, 3);
				this.listBoxItems.Name = "listBoxItems";
				this.tableLayoutPanel.SetRowSpan(this.listBoxItems, 3);
				this.listBoxItems.SelectionMode = SelectionMode.MultiExtended;
				componentResourceManager.ApplyResources(this.btnMoveUp, "btnMoveUp");
				this.btnMoveUp.Margin = new Padding(3, 3, 18, 0);
				this.btnMoveUp.Name = "btnMoveUp";
				componentResourceManager.ApplyResources(this.btnMoveDown, "btnMoveDown");
				this.btnMoveDown.Margin = new Padding(3, 1, 18, 3);
				this.btnMoveDown.Name = "btnMoveDown";
				componentResourceManager.ApplyResources(this.btnRemove, "btnRemove");
				this.btnRemove.Margin = new Padding(3, 3, 18, 3);
				this.btnRemove.Name = "btnRemove";
				base.AutoScaleMode = AutoScaleMode.Font;
				base.AcceptButton = this.btnOK;
				componentResourceManager.ApplyResources(this, "$this");
				base.CancelButton = this.btnCancel;
				base.Controls.Add(this.tableLayoutPanel);
				base.HelpButton = true;
				base.MaximizeBox = false;
				base.MinimizeBox = false;
				base.Name = "ToolStripCollectionEditor";
				base.Padding = new Padding(9);
				base.ShowIcon = false;
				base.ShowInTaskbar = false;
				base.SizeGripStyle = SizeGripStyle.Show;
				this.tableLayoutPanel.ResumeLayout(false);
				this.tableLayoutPanel.PerformLayout();
				this.addTableLayoutPanel.ResumeLayout(false);
				this.addTableLayoutPanel.PerformLayout();
				this.okCancelTableLayoutPanel.ResumeLayout(false);
				this.okCancelTableLayoutPanel.PerformLayout();
				base.ResumeLayout(false);
				base.HelpButtonClicked += this.ToolStripCollectionEditor_HelpButtonClicked;
				this.newItemTypes.DropDown += this.OnnewItemTypes_DropDown;
				this.newItemTypes.HandleCreated += this.OnComboHandleCreated;
				this.newItemTypes.SelectedIndexChanged += this.OnnewItemTypes_SelectedIndexChanged;
				this.btnAddNew.Click += this.OnnewItemTypes_SelectionChangeCommitted;
				this.btnMoveUp.Click += this.OnbtnMoveUp_Click;
				this.btnMoveDown.Click += this.OnbtnMoveDown_Click;
				this.btnRemove.Click += this.OnbtnRemove_Click;
				this.btnOK.Click += this.OnbtnOK_Click;
				this.selectedItemName.Paint += this.OnselectedItemName_Paint;
				this.listBoxItems.SelectedIndexChanged += this.OnlistBoxItems_SelectedIndexChanged;
				this.listBoxItems.DrawItem += this.OnlistBoxItems_DrawItem;
				this.listBoxItems.MeasureItem += this.OnlistBoxItems_MeasureItem;
				this.selectedItemProps.PropertyValueChanged += this.PropertyGrid_propertyValueChanged;
				base.Load += this.OnFormLoad;
			}

			private void OnComboHandleCreated(object sender, EventArgs e)
			{
				this.newItemTypes.HandleCreated -= this.OnComboHandleCreated;
				this.newItemTypes.MeasureItem += this.OnlistBoxItems_MeasureItem;
				this.newItemTypes.DrawItem += this.OnlistBoxItems_DrawItem;
			}

			private void AddItem(ToolStripItem newItem, int index)
			{
				if (index == -1)
				{
					this._itemList.Add(newItem);
				}
				else
				{
					if (index < 0 || index >= this._itemList.Count)
					{
						throw new IndexOutOfRangeException();
					}
					this._itemList.Insert(index, newItem);
				}
				ToolStrip toolStrip = ((base.Context != null) ? ToolStripCollectionEditor.ToolStripItemEditorForm.ToolStripFromObject(base.Context.Instance) : null);
				if (toolStrip != null)
				{
					toolStrip.Items.Add(newItem);
				}
				this.listBoxItems.ClearSelected();
				this.listBoxItems.SelectedItem = newItem;
			}

			private void MoveItem(int fromIndex, int toIndex)
			{
				this._itemList.Move(fromIndex, toIndex);
			}

			private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
			{
				if (e.Component is ToolStripItem && e.Member is PropertyDescriptor && e.Member.Name == "Name")
				{
					this.lblItems.Invalidate();
				}
			}

			protected override void OnEditValueChanged()
			{
				this.selectedItemProps.SelectedObjects = null;
				this.Collection = (ToolStripItemCollection)base.EditValue;
			}

			private void OnFormLoad(object sender, EventArgs e)
			{
				this.newItemTypes.ItemHeight = Math.Max(16, this.Font.Height);
				Component component = base.Context.Instance as Component;
				if (component != null)
				{
					Type[] array = ToolStripDesignerUtils.GetStandardItemTypes(component);
					this.newItemTypes.Items.Clear();
					foreach (Type type in array)
					{
						this.newItemTypes.Items.Add(new ToolStripCollectionEditor.ToolStripItemEditorForm.TypeListItem(type));
					}
					this.newItemTypes.SelectedIndex = 0;
					this.customItemIndex = -1;
					array = ToolStripDesignerUtils.GetCustomItemTypes(component, component.Site);
					if (array.Length > 0)
					{
						this.customItemIndex = this.newItemTypes.Items.Count;
						foreach (Type type2 in array)
						{
							this.newItemTypes.Items.Add(new ToolStripCollectionEditor.ToolStripItemEditorForm.TypeListItem(type2));
						}
					}
					if (this.listBoxItems.Items.Count > 0)
					{
						this.listBoxItems.SelectedIndex = 0;
					}
				}
			}

			private void OnbtnOK_Click(object sender, EventArgs e)
			{
				base.DialogResult = DialogResult.OK;
			}

			private void ToolStripCollectionEditor_HelpButtonClicked(object sender, CancelEventArgs e)
			{
				e.Cancel = true;
				this.editor.ShowHelp();
			}

			private void OnbtnRemove_Click(object sender, EventArgs e)
			{
				ToolStripItem[] array = new ToolStripItem[this.listBoxItems.SelectedItems.Count];
				this.listBoxItems.SelectedItems.CopyTo(array, 0);
				for (int i = 0; i < array.Length; i++)
				{
					this.RemoveItem(array[i]);
				}
			}

			private void OnbtnMoveDown_Click(object sender, EventArgs e)
			{
				ToolStripItem toolStripItem = (ToolStripItem)this.listBoxItems.SelectedItem;
				int num = this.listBoxItems.Items.IndexOf(toolStripItem);
				this.MoveItem(num, ++num);
				this.listBoxItems.SelectedIndex = num;
			}

			private void OnbtnMoveUp_Click(object sender, EventArgs e)
			{
				ToolStripItem toolStripItem = (ToolStripItem)this.listBoxItems.SelectedItem;
				int num = this.listBoxItems.Items.IndexOf(toolStripItem);
				if (num > 1)
				{
					this.MoveItem(num, --num);
					this.listBoxItems.SelectedIndex = num;
				}
			}

			private void OnnewItemTypes_DropDown(object sender, EventArgs e)
			{
				if (this.newItemTypes.Tag == null || !(bool)this.newItemTypes.Tag)
				{
					int num = this.newItemTypes.ItemHeight;
					int num2 = 0;
					using (Graphics graphics = this.newItemTypes.CreateGraphics())
					{
						foreach (object obj in this.newItemTypes.Items)
						{
							ToolStripCollectionEditor.ToolStripItemEditorForm.TypeListItem typeListItem = (ToolStripCollectionEditor.ToolStripItemEditorForm.TypeListItem)obj;
							num = (int)Math.Max((float)num, (float)(this.newItemTypes.ItemHeight + 1) + graphics.MeasureString(typeListItem.Type.Name, this.newItemTypes.Font).Width + 5f);
							num2 += this.Font.Height + 4 + 2;
						}
					}
					this.newItemTypes.DropDownWidth = num;
					this.newItemTypes.DropDownHeight = num2;
					this.newItemTypes.Tag = true;
				}
			}

			private void OnnewItemTypes_SelectionChangeCommitted(object sender, EventArgs e)
			{
				ToolStripCollectionEditor.ToolStripItemEditorForm.TypeListItem typeListItem = this.newItemTypes.SelectedItem as ToolStripCollectionEditor.ToolStripItemEditorForm.TypeListItem;
				if (typeListItem != null)
				{
					ToolStripItem toolStripItem = (ToolStripItem)base.CreateInstance(typeListItem.Type);
					if (toolStripItem is ToolStripButton || toolStripItem is ToolStripSplitButton || toolStripItem is ToolStripDropDownButton)
					{
						Image image = null;
						try
						{
							image = new Bitmap(typeof(ToolStripButton), "blank.bmp");
						}
						catch (Exception ex)
						{
							if (ClientUtils.IsCriticalException(ex))
							{
								throw;
							}
						}
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(toolStripItem)["Image"];
						if (propertyDescriptor != null && image != null)
						{
							propertyDescriptor.SetValue(toolStripItem, image);
						}
						PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(toolStripItem)["DisplayStyle"];
						if (propertyDescriptor2 != null)
						{
							propertyDescriptor2.SetValue(toolStripItem, ToolStripItemDisplayStyle.Image);
						}
						PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(toolStripItem)["ImageTransparentColor"];
						if (propertyDescriptor3 != null)
						{
							propertyDescriptor3.SetValue(toolStripItem, Color.Magenta);
						}
					}
					this.AddItem(toolStripItem, -1);
					this.listBoxItems.Focus();
				}
			}

			private void OnnewItemTypes_SelectedIndexChanged(object sender, EventArgs e)
			{
				this.newItemTypes.Invalidate();
			}

			private void OnlistBoxItems_MeasureItem(object sender, MeasureItemEventArgs e)
			{
				int num = 0;
				if (sender is ComboBox)
				{
					bool flag = e.Index == this.customItemIndex;
					if (e.Index >= 0 && flag)
					{
						num = 4;
					}
				}
				Font font = this.Font;
				e.ItemHeight = Math.Max(16 + num, font.Height + num) + 2;
			}

			private void OnlistBoxItems_DrawItem(object sender, DrawItemEventArgs e)
			{
				if (e.Index == -1)
				{
					return;
				}
				bool flag = false;
				bool flag2 = false;
				bool flag3 = (e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;
				Type type;
				string text;
				if (sender is ListBox)
				{
					ListBox listBox = sender as ListBox;
					Component component = listBox.Items[e.Index] as Component;
					if (component == null)
					{
						return;
					}
					if (component is ToolStripItem)
					{
						flag = true;
					}
					type = component.GetType();
					text = ((component.Site != null) ? component.Site.Name : type.Name);
				}
				else
				{
					if (!(sender is ComboBox))
					{
						return;
					}
					flag2 = e.Index == this.customItemIndex && !flag3;
					ToolStripCollectionEditor.ToolStripItemEditorForm.TypeListItem typeListItem = ((ComboBox)sender).Items[e.Index] as ToolStripCollectionEditor.ToolStripItemEditorForm.TypeListItem;
					if (typeListItem == null)
					{
						return;
					}
					type = typeListItem.Type;
					text = typeListItem.ToString();
				}
				if (type != null)
				{
					Color color = Color.Empty;
					if (flag2)
					{
						e.Graphics.DrawLine(SystemPens.ControlDark, e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Right - 2, e.Bounds.Y + 2);
					}
					Rectangle bounds = e.Bounds;
					bounds.Size = new Size(16, 16);
					int num = (flag3 ? 0 : 2);
					bounds.Offset(num, 1);
					if (flag2)
					{
						bounds.Offset(0, 4);
					}
					if (flag)
					{
						bounds.X += 20;
					}
					if (!flag3)
					{
						bounds.Intersect(e.Bounds);
					}
					Bitmap toolboxBitmap = ToolStripDesignerUtils.GetToolboxBitmap(type);
					if (toolboxBitmap != null)
					{
						if (flag3)
						{
							e.Graphics.DrawImage(toolboxBitmap, e.Bounds.X, e.Bounds.Y, 16, 16);
						}
						else
						{
							e.Graphics.FillRectangle(SystemBrushes.Window, bounds);
							e.Graphics.DrawImage(toolboxBitmap, bounds);
						}
					}
					Rectangle bounds2 = e.Bounds;
					bounds2.X = bounds.Right + 6;
					bounds2.Y = bounds.Top - 1;
					if (!flag3)
					{
						bounds2.Y += 2;
					}
					bounds2.Intersect(e.Bounds);
					Rectangle bounds3 = e.Bounds;
					bounds3.X = bounds2.X - 2;
					if (flag2)
					{
						bounds3.Y += 4;
						bounds3.Height -= 4;
					}
					if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
					{
						color = SystemColors.HighlightText;
						e.Graphics.FillRectangle(SystemBrushes.Highlight, bounds3);
					}
					else
					{
						color = SystemColors.WindowText;
						e.Graphics.FillRectangle(SystemBrushes.Window, bounds3);
					}
					if (!string.IsNullOrEmpty(text))
					{
						TextFormatFlags textFormatFlags = TextFormatFlags.Default;
						TextRenderer.DrawText(e.Graphics, text, this.Font, bounds2, color, textFormatFlags);
					}
					if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
					{
						bounds3.Width--;
						ControlPaint.DrawFocusRectangle(e.Graphics, bounds3, e.ForeColor, e.BackColor);
					}
				}
			}

			private void OnlistBoxItems_SelectedIndexChanged(object sender, EventArgs e)
			{
				object[] array = new object[this.listBoxItems.SelectedItems.Count];
				if (array.Length > 0)
				{
					this.listBoxItems.SelectedItems.CopyTo(array, 0);
				}
				if (array.Length == 1 && array[0] is ToolStrip)
				{
					ToolStrip toolStrip = array[0] as ToolStrip;
					if (toolStrip != null && toolStrip.Site != null)
					{
						if (this.toolStripCustomTypeDescriptor == null)
						{
							this.toolStripCustomTypeDescriptor = new ToolStripCustomTypeDescriptor((ToolStrip)array[0]);
						}
						this.selectedItemProps.SelectedObjects = new object[] { this.toolStripCustomTypeDescriptor };
					}
					else
					{
						this.selectedItemProps.SelectedObjects = null;
					}
				}
				else
				{
					this.selectedItemProps.SelectedObjects = array;
				}
				this.btnMoveUp.Enabled = this.listBoxItems.SelectedItems.Count == 1 && this.listBoxItems.SelectedIndex > 1;
				this.btnMoveDown.Enabled = this.listBoxItems.SelectedItems.Count == 1 && this.listBoxItems.SelectedIndex < this.listBoxItems.Items.Count - 1;
				this.btnRemove.Enabled = array.Length > 0;
				foreach (object obj in this.listBoxItems.SelectedItems)
				{
					if (obj is ToolStrip)
					{
						this.btnRemove.Enabled = (this.btnMoveUp.Enabled = (this.btnMoveDown.Enabled = false));
						break;
					}
				}
				this.listBoxItems.Invalidate();
				this.selectedItemName.Invalidate();
			}

			private void PropertyGrid_propertyValueChanged(object sender, PropertyValueChangedEventArgs e)
			{
				this.listBoxItems.Invalidate();
				this.selectedItemName.Invalidate();
			}

			private void OnselectedItemName_Paint(object sender, PaintEventArgs e)
			{
				using (Font font = new Font(this.selectedItemName.Font, FontStyle.Bold))
				{
					Label label = sender as Label;
					Rectangle clientRectangle = label.ClientRectangle;
					bool flag = label.RightToLeft == RightToLeft.Yes;
					StringFormat stringFormat;
					if (flag)
					{
						stringFormat = new StringFormat(StringFormatFlags.DirectionRightToLeft);
					}
					else
					{
						stringFormat = new StringFormat();
					}
					stringFormat.HotkeyPrefix = HotkeyPrefix.Show;
					switch (this.listBoxItems.SelectedItems.Count)
					{
					case 0:
						e.Graphics.FillRectangle(SystemBrushes.Control, clientRectangle);
						if (label != null)
						{
							label.Text = SR.GetString("ToolStripItemCollectionEditorLabelNone");
						}
						e.Graphics.DrawString(SR.GetString("ToolStripItemCollectionEditorLabelNone"), font, SystemBrushes.WindowText, clientRectangle, stringFormat);
						break;
					case 1:
					{
						Component component;
						if (this.listBoxItems.SelectedItem is ToolStrip)
						{
							component = (ToolStrip)this.listBoxItems.SelectedItem;
						}
						else
						{
							component = (ToolStripItem)this.listBoxItems.SelectedItem;
						}
						string text = "&" + component.GetType().Name;
						if (component.Site != null)
						{
							e.Graphics.FillRectangle(SystemBrushes.Control, clientRectangle);
							string name = component.Site.Name;
							if (label != null)
							{
								label.Text = text + name;
							}
							int num = (int)e.Graphics.MeasureString(text, font).Width;
							e.Graphics.DrawString(text, font, SystemBrushes.WindowText, clientRectangle, stringFormat);
							int num2 = (int)e.Graphics.MeasureString(name, this.selectedItemName.Font).Width;
							Rectangle rectangle = new Rectangle(num + 5, 0, clientRectangle.Width - (num + 5), clientRectangle.Height);
							if (num2 > rectangle.Width)
							{
								label.AutoEllipsis = true;
							}
							else
							{
								label.AutoEllipsis = false;
							}
							TextFormatFlags textFormatFlags = TextFormatFlags.EndEllipsis;
							if (flag)
							{
								textFormatFlags |= TextFormatFlags.RightToLeft;
							}
							TextRenderer.DrawText(e.Graphics, name, this.selectedItemName.Font, rectangle, SystemColors.WindowText, textFormatFlags);
						}
						break;
					}
					default:
						e.Graphics.FillRectangle(SystemBrushes.Control, clientRectangle);
						if (label != null)
						{
							label.Text = SR.GetString("ToolStripItemCollectionEditorLabelMultipleItems");
						}
						e.Graphics.DrawString(SR.GetString("ToolStripItemCollectionEditorLabelMultipleItems"), font, SystemBrushes.WindowText, clientRectangle, stringFormat);
						break;
					}
				}
			}

			private void RemoveItem(ToolStripItem item)
			{
				int num;
				try
				{
					num = this._itemList.IndexOf(item);
					this._itemList.Remove(item);
				}
				finally
				{
					item.Dispose();
				}
				if (this._itemList.Count > 0)
				{
					this.listBoxItems.ClearSelected();
					num = Math.Max(0, Math.Min(num, this.listBoxItems.Items.Count - 1));
					this.listBoxItems.SelectedIndex = num;
				}
			}

			internal static ToolStrip ToolStripFromObject(object instance)
			{
				ToolStrip toolStrip = null;
				if (instance != null)
				{
					if (instance is ToolStripDropDownItem)
					{
						toolStrip = ((ToolStripDropDownItem)instance).DropDown;
					}
					else
					{
						toolStrip = instance as ToolStrip;
					}
				}
				return toolStrip;
			}

			private const int ICON_HEIGHT = 16;

			private const int SEPARATOR_HEIGHT = 4;

			private const int TEXT_IMAGE_SPACING = 6;

			private const int INDENT_SPACING = 4;

			private const int IMAGE_PADDING = 1;

			private const int GdiPlusFudge = 5;

			private ToolStripCollectionEditor editor;

			private ToolStripCustomTypeDescriptor toolStripCustomTypeDescriptor;

			private ToolStripItemCollection _targetToolStripCollection;

			private ToolStripCollectionEditor.ToolStripItemEditorForm.EditorItemCollection _itemList;

			private int customItemIndex = -1;

			private TableLayoutPanel tableLayoutPanel;

			private TableLayoutPanel addTableLayoutPanel;

			private TableLayoutPanel okCancelTableLayoutPanel;

			private Button btnCancel;

			private Button btnOK;

			private Button btnMoveUp;

			private Button btnMoveDown;

			private Label lblItems;

			private ComboBox newItemTypes;

			private Button btnAddNew;

			private CollectionEditor.FilterListBox listBoxItems;

			private Label selectedItemName;

			private Button btnRemove;

			private VsPropertyGrid selectedItemProps;

			private Label lblMembers;

			private IComponentChangeService _componentChangeSvc;

			private string _originalText;

			private class ImageComboBox : ComboBox
			{
				private Rectangle ImageRect
				{
					get
					{
						if (this.RightToLeft == RightToLeft.Yes)
						{
							return new Rectangle(4 + SystemInformation.HorizontalScrollBarThumbWidth, 3, 16, 16);
						}
						return new Rectangle(3, 3, 16, 16);
					}
				}

				protected override void OnDropDownClosed(EventArgs e)
				{
					base.OnDropDownClosed(e);
					base.Invalidate(this.ImageRect);
				}

				protected override void OnSelectedIndexChanged(EventArgs e)
				{
					base.OnSelectedIndexChanged(e);
					base.Invalidate(this.ImageRect);
				}

				protected override void WndProc(ref Message m)
				{
					base.WndProc(ref m);
					switch (m.Msg)
					{
					case 7:
					case 8:
						base.Invalidate(this.ImageRect);
						return;
					default:
						return;
					}
				}
			}

			private class EditorItemCollection : CollectionBase
			{
				internal EditorItemCollection(ToolStripCollectionEditor.ToolStripItemEditorForm owner, IList displayList, IList componentList)
				{
					this._owner = owner;
					this._listBoxList = displayList;
					this._targetCollectionList = componentList;
				}

				public void Add(object item)
				{
					base.List.Add(new ToolStripCollectionEditor.ToolStripItemEditorForm.EditorItemCollection.EditorItem(item));
				}

				public int IndexOf(ToolStripItem item)
				{
					for (int i = 0; i < base.List.Count; i++)
					{
						ToolStripCollectionEditor.ToolStripItemEditorForm.EditorItemCollection.EditorItem editorItem = (ToolStripCollectionEditor.ToolStripItemEditorForm.EditorItemCollection.EditorItem)base.List[i];
						if (editorItem.Component == item)
						{
							return i;
						}
					}
					return -1;
				}

				public void Insert(int index, ToolStripItem item)
				{
					base.List.Insert(index, new ToolStripCollectionEditor.ToolStripItemEditorForm.EditorItemCollection.EditorItem(item));
				}

				public void Move(int fromIndex, int toIndex)
				{
					if (toIndex == fromIndex)
					{
						return;
					}
					ToolStripCollectionEditor.ToolStripItemEditorForm.EditorItemCollection.EditorItem editorItem = (ToolStripCollectionEditor.ToolStripItemEditorForm.EditorItemCollection.EditorItem)base.List[fromIndex];
					if (editorItem.Host != null)
					{
						return;
					}
					try
					{
						this._owner.Context.OnComponentChanging();
						this._listBoxList.Remove(editorItem.Component);
						this._targetCollectionList.Remove(editorItem.Component);
						base.InnerList.Remove(editorItem);
						this._listBoxList.Insert(toIndex, editorItem.Component);
						this._targetCollectionList.Insert(toIndex - 1, editorItem.Component);
						base.InnerList.Insert(toIndex, editorItem);
					}
					finally
					{
						this._owner.Context.OnComponentChanged();
					}
				}

				protected override void OnClear()
				{
					this._listBoxList.Clear();
					foreach (object obj in base.List)
					{
						ToolStripCollectionEditor.ToolStripItemEditorForm.EditorItemCollection.EditorItem editorItem = (ToolStripCollectionEditor.ToolStripItemEditorForm.EditorItemCollection.EditorItem)obj;
						editorItem.Dispose();
					}
					base.OnClear();
				}

				protected override void OnInsertComplete(int index, object value)
				{
					ToolStripCollectionEditor.ToolStripItemEditorForm.EditorItemCollection.EditorItem editorItem = (ToolStripCollectionEditor.ToolStripItemEditorForm.EditorItemCollection.EditorItem)value;
					if (editorItem.Host != null)
					{
						this._listBoxList.Insert(index, editorItem.Host);
						base.OnInsertComplete(index, value);
						return;
					}
					if (!this._targetCollectionList.Contains(editorItem.Component))
					{
						try
						{
							this._owner.Context.OnComponentChanging();
							this._targetCollectionList.Insert(index - 1, editorItem.Component);
						}
						finally
						{
							this._owner.Context.OnComponentChanged();
						}
					}
					this._listBoxList.Insert(index, editorItem.Component);
					base.OnInsertComplete(index, value);
				}

				protected override void OnRemove(int index, object value)
				{
					ToolStripCollectionEditor.ToolStripItemEditorForm.EditorItemCollection.EditorItem editorItem = (ToolStripCollectionEditor.ToolStripItemEditorForm.EditorItemCollection.EditorItem)base.List[index];
					this._listBoxList.RemoveAt(index);
					try
					{
						this._owner.Context.OnComponentChanging();
						this._targetCollectionList.RemoveAt(index - 1);
					}
					finally
					{
						this._owner.Context.OnComponentChanged();
					}
					editorItem.Dispose();
					base.OnRemove(index, value);
				}

				public void Remove(ToolStripItem item)
				{
					int num = this.IndexOf(item);
					base.List.RemoveAt(num);
				}

				private IList _listBoxList;

				private IList _targetCollectionList;

				private ToolStripCollectionEditor.ToolStripItemEditorForm _owner;

				private class EditorItem
				{
					internal EditorItem(object componentItem)
					{
						if (componentItem is ToolStrip)
						{
							this._host = (ToolStrip)componentItem;
							return;
						}
						this._component = (ToolStripItem)componentItem;
					}

					public ToolStripItem Component
					{
						get
						{
							return this._component;
						}
					}

					public ToolStrip Host
					{
						get
						{
							return this._host;
						}
					}

					public void Dispose()
					{
						GC.SuppressFinalize(this);
						this._component = null;
					}

					public ToolStripItem _component;

					public ToolStrip _host;
				}
			}

			private class TypeListItem
			{
				public TypeListItem(Type t)
				{
					this.Type = t;
				}

				public override string ToString()
				{
					return ToolStripDesignerUtils.GetToolboxDescription(this.Type);
				}

				public readonly Type Type;
			}
		}
	}
}
