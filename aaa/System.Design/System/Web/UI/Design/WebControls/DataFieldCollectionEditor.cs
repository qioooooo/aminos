using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000439 RID: 1081
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal class DataFieldCollectionEditor : StringCollectionEditor
	{
		// Token: 0x06002727 RID: 10023 RVA: 0x000D524F File Offset: 0x000D424F
		public DataFieldCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06002728 RID: 10024 RVA: 0x000D5258 File Offset: 0x000D4258
		private bool HasSchema
		{
			get
			{
				ITypeDescriptorContext context = base.Context;
				bool flag = false;
				if (context != null && context.Instance != null)
				{
					Control control = context.Instance as Control;
					if (control != null)
					{
						ISite site = control.Site;
						if (site != null)
						{
							IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
							if (designerHost != null)
							{
								IDesigner designer = designerHost.GetDesigner(control);
								DataBoundControlDesigner dataBoundControlDesigner = designer as DataBoundControlDesigner;
								if (dataBoundControlDesigner != null)
								{
									DesignerDataSourceView designerView = dataBoundControlDesigner.DesignerView;
									if (designerView != null)
									{
										IDataSourceViewSchema dataSourceViewSchema = null;
										try
										{
											dataSourceViewSchema = designerView.Schema;
										}
										catch (Exception ex)
										{
											IComponentDesignerDebugService componentDesignerDebugService = (IComponentDesignerDebugService)site.GetService(typeof(IComponentDesignerDebugService));
											if (componentDesignerDebugService != null)
											{
												componentDesignerDebugService.Fail(SR.GetString("DataSource_DebugService_FailedCall", new object[] { "DesignerDataSourceView.Schema", ex.Message }));
											}
										}
										if (dataSourceViewSchema != null)
										{
											flag = true;
										}
									}
								}
							}
						}
					}
					else
					{
						IDataSourceViewSchemaAccessor dataSourceViewSchemaAccessor = context.Instance as IDataSourceViewSchemaAccessor;
						if (dataSourceViewSchemaAccessor != null && dataSourceViewSchemaAccessor.DataSourceViewSchema != null)
						{
							flag = true;
						}
					}
				}
				return flag;
			}
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000D5378 File Offset: 0x000D4378
		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			if (this.HasSchema)
			{
				ITypeDescriptorContext context = base.Context;
				if (context != null && context.Instance != null)
				{
					Control control = context.Instance as Control;
					if (control != null)
					{
						ISite site = control.Site;
						return new DataFieldCollectionEditor.DataFieldCollectionForm(site, this);
					}
				}
			}
			return base.CreateCollectionForm();
		}

		// Token: 0x04001AD5 RID: 6869
		private const int SC_CONTEXTHELP = 61824;

		// Token: 0x04001AD6 RID: 6870
		private const int WM_SYSCOMMAND = 274;

		// Token: 0x0200043A RID: 1082
		private class DataFieldCollectionForm : CollectionEditor.CollectionForm
		{
			// Token: 0x0600272A RID: 10026 RVA: 0x000D53C4 File Offset: 0x000D43C4
			public DataFieldCollectionForm(IServiceProvider serviceProvider, CollectionEditor editor)
				: base(editor)
			{
				this.editor = (DataFieldCollectionEditor)editor;
				this._serviceProvider = serviceProvider;
				string @string = SR.GetString("RTL");
				if (!string.Equals(@string, "RTL_False", StringComparison.Ordinal))
				{
					this.RightToLeft = RightToLeft.Yes;
					this.RightToLeftLayout = true;
				}
				this.InitializeComponent();
				this._dataFields = this.GetControlDataFieldNames();
			}

			// Token: 0x0600272B RID: 10027 RVA: 0x000D54B4 File Offset: 0x000D44B4
			private void AddFieldToSelectedList()
			{
				int selectedIndex = this.fieldsList.SelectedIndex;
				object selectedItem = this.fieldsList.SelectedItem;
				if (selectedIndex >= 0)
				{
					this.fieldsList.Items.RemoveAt(selectedIndex);
					this.selectedFieldsList.SelectedIndex = this.selectedFieldsList.Items.Add(selectedItem);
					if (this.fieldsList.Items.Count > 0)
					{
						this.fieldsList.SelectedIndex = ((this.fieldsList.Items.Count > selectedIndex) ? selectedIndex : (this.fieldsList.Items.Count - 1));
					}
				}
			}

			// Token: 0x0600272C RID: 10028 RVA: 0x000D5550 File Offset: 0x000D4550
			private string[] GetControlDataFieldNames()
			{
				if (this._dataFields == null)
				{
					ITypeDescriptorContext context = this.editor.Context;
					IDataSourceViewSchema dataSourceViewSchema = null;
					if (context != null && context.Instance != null)
					{
						Control control = context.Instance as Control;
						if (control != null)
						{
							ISite site = control.Site;
							if (site == null)
							{
								goto IL_010A;
							}
							IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
							if (designerHost == null)
							{
								goto IL_010A;
							}
							IDesigner designer = designerHost.GetDesigner(control);
							DataBoundControlDesigner dataBoundControlDesigner = designer as DataBoundControlDesigner;
							if (dataBoundControlDesigner == null)
							{
								goto IL_010A;
							}
							DesignerDataSourceView designerView = dataBoundControlDesigner.DesignerView;
							if (designerView == null)
							{
								goto IL_010A;
							}
							try
							{
								dataSourceViewSchema = designerView.Schema;
								goto IL_010A;
							}
							catch (Exception ex)
							{
								IComponentDesignerDebugService componentDesignerDebugService = (IComponentDesignerDebugService)site.GetService(typeof(IComponentDesignerDebugService));
								if (componentDesignerDebugService != null)
								{
									componentDesignerDebugService.Fail(SR.GetString("DataSource_DebugService_FailedCall", new object[] { "DesignerDataSourceView.Schema", ex.Message }));
								}
								goto IL_010A;
							}
						}
						IDataSourceViewSchemaAccessor dataSourceViewSchemaAccessor = context.Instance as IDataSourceViewSchemaAccessor;
						if (dataSourceViewSchemaAccessor != null)
						{
							dataSourceViewSchema = dataSourceViewSchemaAccessor.DataSourceViewSchema as IDataSourceViewSchema;
						}
					}
					IL_010A:
					if (dataSourceViewSchema != null)
					{
						IDataSourceFieldSchema[] array = dataSourceViewSchema.GetFields();
						if (array != null)
						{
							int num = array.Length;
							this._dataFields = new string[num];
							for (int i = 0; i < num; i++)
							{
								this._dataFields[i] = array[i].Name;
							}
						}
					}
				}
				return this._dataFields;
			}

			// Token: 0x0600272D RID: 10029 RVA: 0x000D56C0 File Offset: 0x000D46C0
			private void InitializeComponent()
			{
				int num = 217;
				int num2 = 364;
				base.SuspendLayout();
				this.fieldLabel.AutoSize = true;
				this.fieldLabel.TabStop = false;
				this.fieldLabel.TabIndex = 0;
				this.fieldLabel.Text = SR.GetString("DataFieldCollectionAvailableFields");
				this.fieldLabel.MinimumSize = new Size(135, 15);
				this.fieldLabel.MaximumSize = new Size(135, 30);
				this.fieldLabel.SetBounds(0, 0, 135, 15);
				this.selectedFieldsLabel.AutoSize = true;
				this.selectedFieldsLabel.TabStop = false;
				this.selectedFieldsLabel.Text = SR.GetString("DataFieldCollectionSelectedFields");
				this.selectedFieldsLabel.MinimumSize = new Size(135, 15);
				this.selectedFieldsLabel.MaximumSize = new Size(135, 30);
				this.selectedFieldsLabel.SetBounds(173, 0, 135, 15);
				this.fieldsList.TabIndex = 1;
				this.fieldsList.AllowDrop = false;
				this.fieldsList.SelectedIndexChanged += this.OnFieldsSelectedIndexChanged;
				this.fieldsList.MouseDoubleClick += this.OnDoubleClickField;
				this.fieldsList.KeyPress += this.OnKeyPressField;
				this.fieldsList.SetBounds(0, 0, 135, 130);
				this.selectedFieldsList.TabIndex = 3;
				this.selectedFieldsList.AllowDrop = false;
				this.selectedFieldsList.SelectedIndexChanged += this.OnSelectedFieldsSelectedIndexChanged;
				this.selectedFieldsList.MouseDoubleClick += this.OnDoubleClickSelectedField;
				this.selectedFieldsList.KeyPress += this.OnKeyPressSelectedField;
				this.selectedFieldsList.SetBounds(0, 0, 135, 130);
				this.moveRight.TabIndex = 100;
				this.moveRight.Text = ">";
				this.moveRight.AccessibleName = SR.GetString("DataFieldCollection_MoveRight");
				this.moveRight.AccessibleDescription = SR.GetString("DataFieldCollection_MoveRightDesc");
				this.moveRight.Click += this.OnMoveRight;
				this.moveRight.Location = new Point(0, 42);
				this.moveRight.Size = new Size(26, 23);
				this.moveLeft.TabIndex = 101;
				this.moveLeft.Text = "<";
				this.moveLeft.AccessibleName = SR.GetString("DataFieldCollection_MoveLeft");
				this.moveLeft.AccessibleDescription = SR.GetString("DataFieldCollection_MoveLeftDesc");
				this.moveLeft.Click += this.OnMoveLeft;
				this.moveLeft.Location = new Point(0, 65);
				this.moveLeft.Size = new Size(26, 23);
				this.moveLeftRightPanel.TabIndex = 2;
				this.moveLeftRightPanel.Location = new Point(6, 0);
				this.moveLeftRightPanel.Size = new Size(28, 130);
				this.moveLeftRightPanel.Controls.Add(this.moveLeft);
				this.moveLeftRightPanel.Controls.Add(this.moveRight);
				this.moveUp.TabIndex = 200;
				Bitmap bitmap = new Icon(base.GetType(), "SortUp.ico").ToBitmap();
				bitmap.MakeTransparent();
				this.moveUp.Image = bitmap;
				this.moveUp.AccessibleName = SR.GetString("DataFieldCollection_MoveUp");
				this.moveUp.AccessibleDescription = SR.GetString("DataFieldCollection_MoveUpDesc");
				this.moveUp.Click += this.OnMoveUp;
				this.moveUp.Location = new Point(0, 0);
				this.moveUp.Size = new Size(26, 23);
				this.moveDown.TabIndex = 201;
				Bitmap bitmap2 = new Icon(base.GetType(), "SortDown.ico").ToBitmap();
				bitmap2.MakeTransparent();
				this.moveDown.Image = bitmap2;
				this.moveDown.AccessibleName = SR.GetString("DataFieldCollection_MoveDown");
				this.moveDown.AccessibleDescription = SR.GetString("DataFieldCollection_MoveDownDesc");
				this.moveDown.Click += this.OnMoveDown;
				this.moveDown.Location = new Point(0, 24);
				this.moveDown.Size = new Size(26, 23);
				this.moveUpDownPanel.TabIndex = 4;
				this.moveUpDownPanel.Location = new Point(6, 0);
				this.moveUpDownPanel.Size = new Size(26, 47);
				this.moveUpDownPanel.Controls.Add(this.moveUp);
				this.moveUpDownPanel.Controls.Add(this.moveDown);
				this.okButton.TabIndex = 5;
				this.okButton.Text = SR.GetString("OKCaption");
				this.okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
				this.okButton.DialogResult = DialogResult.OK;
				this.okButton.Click += this.OKButton_click;
				this.okButton.SetBounds(num2 - 12 - 150 - 6, num - 12 - 23, 75, 23);
				this.cancelButton.TabIndex = 6;
				this.cancelButton.Text = SR.GetString("CancelCaption");
				this.cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
				this.cancelButton.DialogResult = DialogResult.Cancel;
				this.cancelButton.SetBounds(num2 - 12 - 75, num - 12 - 23, 75, 23);
				this.layoutPanel.AutoSize = true;
				this.layoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				this.layoutPanel.ColumnCount = 4;
				this.layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 135f));
				this.layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 38f));
				this.layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 135f));
				this.layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 32f));
				this.layoutPanel.Location = new Point(12, 12);
				this.layoutPanel.Size = new Size(340, 147);
				this.layoutPanel.RowCount = 2;
				this.layoutPanel.RowStyles.Add(new RowStyle());
				this.layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 130f));
				this.layoutPanel.Controls.Add(this.fieldLabel, 0, 0);
				this.layoutPanel.Controls.Add(this.selectedFieldsLabel, 2, 0);
				this.layoutPanel.Controls.Add(this.fieldsList, 0, 1);
				this.layoutPanel.Controls.Add(this.selectedFieldsList, 2, 1);
				this.layoutPanel.Controls.Add(this.moveLeftRightPanel, 1, 1);
				this.layoutPanel.Controls.Add(this.moveUpDownPanel, 3, 1);
				Font dialogFont = UIServiceHelper.GetDialogFont(this._serviceProvider);
				if (dialogFont != null)
				{
					this.Font = dialogFont;
				}
				this.Text = SR.GetString("DataFieldCollectionEditorTitle");
				base.AcceptButton = this.okButton;
				this.AutoScaleBaseSize = new Size(5, 14);
				base.CancelButton = this.cancelButton;
				base.ClientSize = new Size(num2, num);
				base.FormBorderStyle = FormBorderStyle.FixedDialog;
				base.HelpButton = true;
				base.MaximizeBox = false;
				base.MinimizeBox = false;
				string @string = SR.GetString("RTL");
				if (!string.Equals(@string, "RTL_False", StringComparison.Ordinal))
				{
					this.RightToLeft = RightToLeft.Yes;
					this.RightToLeftLayout = true;
				}
				base.ShowIcon = false;
				base.ShowInTaskbar = false;
				base.StartPosition = FormStartPosition.CenterParent;
				base.Controls.Clear();
				base.Controls.AddRange(new Control[] { this.layoutPanel, this.okButton, this.cancelButton });
				base.ResumeLayout(false);
				base.PerformLayout();
			}

			// Token: 0x0600272E RID: 10030 RVA: 0x000D5F18 File Offset: 0x000D4F18
			private void OKButton_click(object sender, EventArgs e)
			{
				int count = this.selectedFieldsList.Items.Count;
				object[] array = new object[count];
				this.selectedFieldsList.Items.CopyTo(array, 0);
				base.Items = array;
			}

			// Token: 0x0600272F RID: 10031 RVA: 0x000D5F56 File Offset: 0x000D4F56
			private void OnDoubleClickField(object sender, MouseEventArgs e)
			{
				if (this.fieldsList.IndexFromPoint(e.Location) != -1 && e.Button == MouseButtons.Left)
				{
					this.AddFieldToSelectedList();
				}
			}

			// Token: 0x06002730 RID: 10032 RVA: 0x000D5F7F File Offset: 0x000D4F7F
			private void OnDoubleClickSelectedField(object sender, MouseEventArgs e)
			{
				if (this.selectedFieldsList.IndexFromPoint(e.Location) != -1 && e.Button == MouseButtons.Left)
				{
					this.RemoveFieldFromSelectedList();
				}
			}

			// Token: 0x06002731 RID: 10033 RVA: 0x000D5FA8 File Offset: 0x000D4FA8
			protected override void OnEditValueChanged()
			{
				this.fields = null;
				this.fieldsList.Items.Clear();
				this.selectedFieldsList.Items.Clear();
				this.fields = new ArrayList();
				foreach (string text in this.GetControlDataFieldNames())
				{
					this.fields.Add(text);
					if (Array.IndexOf(base.Items, text) < 0)
					{
						this.fieldsList.Items.Add(text);
					}
				}
				foreach (string text2 in base.Items)
				{
					this.selectedFieldsList.Items.Add(text2);
				}
				if (this.fieldsList.Items.Count > 0)
				{
					this.fieldsList.SelectedIndex = 0;
				}
				this.SetButtonsEnabled();
			}

			// Token: 0x06002732 RID: 10034 RVA: 0x000D608B File Offset: 0x000D508B
			private void OnFieldsSelectedIndexChanged(object sender, EventArgs e)
			{
				if (this.fieldsList.SelectedIndex > -1)
				{
					this.selectedFieldsList.SelectedIndex = -1;
				}
				this.SetButtonsEnabled();
			}

			// Token: 0x06002733 RID: 10035 RVA: 0x000D60AD File Offset: 0x000D50AD
			private void OnKeyPressField(object sender, KeyPressEventArgs e)
			{
				if (e.KeyChar == '\r')
				{
					this.AddFieldToSelectedList();
					e.Handled = true;
				}
			}

			// Token: 0x06002734 RID: 10036 RVA: 0x000D60C6 File Offset: 0x000D50C6
			private void OnKeyPressSelectedField(object sender, KeyPressEventArgs e)
			{
				if (e.KeyChar == '\r')
				{
					this.RemoveFieldFromSelectedList();
					e.Handled = true;
				}
			}

			// Token: 0x06002735 RID: 10037 RVA: 0x000D60E0 File Offset: 0x000D50E0
			private void OnMoveDown(object sender, EventArgs e)
			{
				int selectedIndex = this.selectedFieldsList.SelectedIndex;
				object selectedItem = this.selectedFieldsList.SelectedItem;
				this.selectedFieldsList.Items.RemoveAt(selectedIndex);
				this.selectedFieldsList.Items.Insert(selectedIndex + 1, selectedItem);
				this.selectedFieldsList.SelectedIndex = selectedIndex + 1;
			}

			// Token: 0x06002736 RID: 10038 RVA: 0x000D6138 File Offset: 0x000D5138
			private void OnMoveLeft(object sender, EventArgs e)
			{
				this.RemoveFieldFromSelectedList();
			}

			// Token: 0x06002737 RID: 10039 RVA: 0x000D6140 File Offset: 0x000D5140
			private void OnMoveRight(object sender, EventArgs e)
			{
				this.AddFieldToSelectedList();
			}

			// Token: 0x06002738 RID: 10040 RVA: 0x000D6148 File Offset: 0x000D5148
			private void OnMoveUp(object sender, EventArgs e)
			{
				int selectedIndex = this.selectedFieldsList.SelectedIndex;
				object selectedItem = this.selectedFieldsList.SelectedItem;
				this.selectedFieldsList.Items.RemoveAt(selectedIndex);
				this.selectedFieldsList.Items.Insert(selectedIndex - 1, selectedItem);
				this.selectedFieldsList.SelectedIndex = selectedIndex - 1;
			}

			// Token: 0x06002739 RID: 10041 RVA: 0x000D61A0 File Offset: 0x000D51A0
			private void OnSelectedFieldsSelectedIndexChanged(object sender, EventArgs e)
			{
				if (this.selectedFieldsList.SelectedIndex > -1)
				{
					this.fieldsList.SelectedIndex = -1;
				}
				this.SetButtonsEnabled();
			}

			// Token: 0x0600273A RID: 10042 RVA: 0x000D61C4 File Offset: 0x000D51C4
			private void RemoveFieldFromSelectedList()
			{
				int selectedIndex = this.selectedFieldsList.SelectedIndex;
				int num = 0;
				if (selectedIndex >= 0)
				{
					string text = this.selectedFieldsList.SelectedItem.ToString();
					int num2 = this.fields.IndexOf(text);
					int num3 = 0;
					while (num3 < this.fieldsList.Items.Count && this.fields.IndexOf(this.fieldsList.Items[num3]) <= num2)
					{
						num++;
						num3++;
					}
					this.fieldsList.Items.Insert(num, text);
					this.selectedFieldsList.Items.RemoveAt(selectedIndex);
					this.fieldsList.SelectedIndex = num;
					if (this.selectedFieldsList.Items.Count > 0)
					{
						this.selectedFieldsList.SelectedIndex = ((this.selectedFieldsList.Items.Count > selectedIndex) ? selectedIndex : (this.selectedFieldsList.Items.Count - 1));
					}
				}
			}

			// Token: 0x0600273B RID: 10043 RVA: 0x000D62C0 File Offset: 0x000D52C0
			private void SetButtonsEnabled()
			{
				int count = this.selectedFieldsList.Items.Count;
				int selectedIndex = this.selectedFieldsList.SelectedIndex;
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				if (this.fieldsList.SelectedIndex > -1)
				{
					flag3 = true;
				}
				if (selectedIndex > -1)
				{
					flag4 = true;
					if (count > 0)
					{
						if (selectedIndex > 0)
						{
							flag = true;
						}
						if (selectedIndex < count - 1)
						{
							flag2 = true;
						}
					}
				}
				this.moveRight.Enabled = flag3;
				this.moveLeft.Enabled = flag4;
				this.moveUp.Enabled = flag;
				this.moveDown.Enabled = flag2;
			}

			// Token: 0x0600273C RID: 10044 RVA: 0x000D6350 File Offset: 0x000D5350
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == 274 && (int)m.WParam == 61824)
				{
					if (this._serviceProvider != null)
					{
						IHelpService helpService = (IHelpService)this._serviceProvider.GetService(typeof(IHelpService));
						if (helpService != null)
						{
							helpService.ShowHelpFromKeyword("net.Asp.DataFieldCollectionEditor");
							return;
						}
					}
				}
				else
				{
					base.WndProc(ref m);
				}
			}

			// Token: 0x04001AD7 RID: 6871
			private Label fieldLabel = new Label();

			// Token: 0x04001AD8 RID: 6872
			private DataFieldCollectionEditor.DataFieldCollectionForm.ListBoxWithEnter fieldsList = new DataFieldCollectionEditor.DataFieldCollectionForm.ListBoxWithEnter();

			// Token: 0x04001AD9 RID: 6873
			private Label selectedFieldsLabel = new Label();

			// Token: 0x04001ADA RID: 6874
			private DataFieldCollectionEditor.DataFieldCollectionForm.ListBoxWithEnter selectedFieldsList = new DataFieldCollectionEditor.DataFieldCollectionForm.ListBoxWithEnter();

			// Token: 0x04001ADB RID: 6875
			private Button moveLeft = new Button();

			// Token: 0x04001ADC RID: 6876
			private Button moveRight = new Button();

			// Token: 0x04001ADD RID: 6877
			private Button moveUp = new Button();

			// Token: 0x04001ADE RID: 6878
			private Button moveDown = new Button();

			// Token: 0x04001ADF RID: 6879
			private Button okButton = new Button();

			// Token: 0x04001AE0 RID: 6880
			private Button cancelButton = new Button();

			// Token: 0x04001AE1 RID: 6881
			private TableLayoutPanel layoutPanel = new TableLayoutPanel();

			// Token: 0x04001AE2 RID: 6882
			private Panel moveUpDownPanel = new Panel();

			// Token: 0x04001AE3 RID: 6883
			private Panel moveLeftRightPanel = new Panel();

			// Token: 0x04001AE4 RID: 6884
			private DataFieldCollectionEditor editor;

			// Token: 0x04001AE5 RID: 6885
			private ArrayList fields;

			// Token: 0x04001AE6 RID: 6886
			private string[] _dataFields;

			// Token: 0x04001AE7 RID: 6887
			private IServiceProvider _serviceProvider;

			// Token: 0x0200043B RID: 1083
			private class ListBoxWithEnter : ListBox
			{
				// Token: 0x0600273D RID: 10045 RVA: 0x000D63B5 File Offset: 0x000D53B5
				protected override bool IsInputKey(Keys keyData)
				{
					return keyData == Keys.Return || base.IsInputKey(keyData);
				}
			}
		}
	}
}
