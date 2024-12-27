using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000410 RID: 1040
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed partial class CreateDataSourceDialog : TaskForm
	{
		// Token: 0x060025FB RID: 9723 RVA: 0x000CC4E8 File Offset: 0x000CB4E8
		public CreateDataSourceDialog(ControlDesigner controlDesigner, Type dataSourceType, bool configure)
			: base(controlDesigner.Component.Site)
		{
			this._controlDesigner = controlDesigner;
			this._controlID = ((Control)controlDesigner.Component).ID;
			this._dataSourceType = dataSourceType;
			this._configure = configure;
			this._displayNameComparer = new CreateDataSourceDialog.DisplayNameComparer();
			base.Glyph = new Bitmap(base.GetType(), "datasourcewizard.bmp");
			this.CreatePanel();
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060025FC RID: 9724 RVA: 0x000CC558 File Offset: 0x000CB558
		public string DataSourceID
		{
			get
			{
				if (this._dataSourceID == null)
				{
					return string.Empty;
				}
				return this._dataSourceID;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060025FD RID: 9725 RVA: 0x000CC56E File Offset: 0x000CB56E
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.DataBoundControl.CreateDataSourceDialog";
			}
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000CC578 File Offset: 0x000CB578
		private string CreateNewDataSource(Type dataSourceType)
		{
			string text = this._idTextBox.Text;
			string text2 = string.Empty;
			if (dataSourceType != null)
			{
				object obj = Activator.CreateInstance(dataSourceType);
				if (obj != null)
				{
					Control control = obj as Control;
					if (control != null)
					{
						control.ID = text;
						ISite site = this.GetSite();
						if (site != null)
						{
							INameCreationService nameCreationService = (INameCreationService)site.GetService(typeof(INameCreationService));
							if (nameCreationService != null)
							{
								try
								{
									nameCreationService.ValidateName(text);
								}
								catch (Exception ex)
								{
									UIServiceHelper.ShowError(site, SR.GetString("CreateDataSource_NameNotValid", new object[] { ex.Message }));
									this._idTextBox.Focus();
									return text2;
								}
								IContainer container = site.Container;
								if (container == null)
								{
									goto IL_00F0;
								}
								ComponentCollection components = container.Components;
								if (components != null && components[text] != null)
								{
									UIServiceHelper.ShowError(site, SR.GetString("CreateDataSource_NameNotUnique"));
									this._idTextBox.Focus();
									return text2;
								}
							}
							IL_00F0:
							IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
							if (designerHost != null)
							{
								IComponent rootComponent = designerHost.RootComponent;
								if (rootComponent != null)
								{
									WebFormsRootDesigner webFormsRootDesigner = designerHost.GetDesigner(rootComponent) as WebFormsRootDesigner;
									if (webFormsRootDesigner != null)
									{
										Control control2 = this.GetComponent() as Control;
										text2 = webFormsRootDesigner.AddControlToDocument(control, control2, ControlLocation.After);
										IDesigner designer = designerHost.GetDesigner(control);
										IDataSourceDesigner dataSourceDesigner = designer as IDataSourceDesigner;
										if (dataSourceDesigner != null)
										{
											if (dataSourceDesigner.CanConfigure && this._configure)
											{
												dataSourceDesigner.Configure();
											}
										}
										else
										{
											IHierarchicalDataSourceDesigner hierarchicalDataSourceDesigner = designer as IHierarchicalDataSourceDesigner;
											if (hierarchicalDataSourceDesigner != null && hierarchicalDataSourceDesigner.CanConfigure && this._configure)
											{
												hierarchicalDataSourceDesigner.Configure();
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return text2;
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000CC73C File Offset: 0x000CB73C
		private void CreatePanel()
		{
			base.SuspendLayout();
			this.CreatePanelControls();
			this.InitializePanelControls();
			base.InitializeForm();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x000CC764 File Offset: 0x000CB764
		private void CreatePanelControls()
		{
			this._selectLabel = new Label();
			this._dataSourceTypesListView = new ListView();
			this._descriptionBox = new TextBox();
			this._idLabel = new Label();
			this._idTextBox = new TextBox();
			this._selectLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._selectLabel.Location = new Point(0, 0);
			this._selectLabel.Name = "_selectLabel";
			this._selectLabel.Size = new Size(544, 16);
			this._selectLabel.TabIndex = 0;
			this._dataSourceTypesListView.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._dataSourceTypesListView.Location = new Point(0, 18);
			this._dataSourceTypesListView.Name = "_dataSourceTypesListView";
			this._dataSourceTypesListView.Size = new Size(544, 90);
			this._dataSourceTypesListView.TabIndex = 1;
			this._dataSourceTypesListView.SelectedIndexChanged += this.OnDataSourceTypeChosen;
			this._dataSourceTypesListView.Alignment = ListViewAlignment.Left;
			this._dataSourceTypesListView.LabelWrap = true;
			this._dataSourceTypesListView.MultiSelect = false;
			this._dataSourceTypesListView.HideSelection = false;
			this._dataSourceTypesListView.ListViewItemSorter = this._displayNameComparer;
			this._dataSourceTypesListView.Sorting = SortOrder.Ascending;
			this._dataSourceTypesListView.MouseDoubleClick += this.OnListViewDoubleClick;
			this._descriptionBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._descriptionBox.Location = new Point(0, 112);
			this._descriptionBox.Name = "_descriptionBox";
			this._descriptionBox.Size = new Size(544, 55);
			this._descriptionBox.TabIndex = 2;
			this._descriptionBox.ReadOnly = true;
			this._descriptionBox.Multiline = true;
			this._descriptionBox.TabStop = false;
			this._descriptionBox.BackColor = SystemColors.Control;
			this._descriptionBox.Multiline = true;
			this._idLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._idLabel.Location = new Point(0, 176);
			this._idLabel.Name = "_idLabel";
			this._idLabel.Size = new Size(544, 16);
			this._idLabel.TabIndex = 3;
			this._idTextBox.Location = new Point(0, 194);
			this._idTextBox.Name = "_idTextBox";
			this._idTextBox.Size = new Size(220, 20);
			this._idTextBox.TabIndex = 4;
			this._idTextBox.TextChanged += this.OnIDChanged;
			base.TaskPanel.Controls.Add(this._idTextBox);
			base.TaskPanel.Controls.Add(this._idLabel);
			base.TaskPanel.Controls.Add(this._descriptionBox);
			base.TaskPanel.Controls.Add(this._dataSourceTypesListView);
			base.TaskPanel.Controls.Add(this._selectLabel);
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x000CCA7F File Offset: 0x000CBA7F
		private IComponent GetComponent()
		{
			if (this._controlDesigner != null)
			{
				return this._controlDesigner.Component;
			}
			return null;
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x000CCA98 File Offset: 0x000CBA98
		private string GetNewDataSourceName(Type dataSourceType)
		{
			if (dataSourceType != null)
			{
				ISite site = this.GetSite();
				if (site != null)
				{
					INameCreationService nameCreationService = (INameCreationService)site.GetService(typeof(INameCreationService));
					if (nameCreationService != null)
					{
						return nameCreationService.CreateName(site.Container, dataSourceType);
					}
					return site.Name + "_DataSource";
				}
			}
			return string.Empty;
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x000CCAF0 File Offset: 0x000CBAF0
		private ISite GetSite()
		{
			IComponent component = this.GetComponent();
			if (component != null)
			{
				return component.Site;
			}
			return null;
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x000CCB10 File Offset: 0x000CBB10
		private void InitializePanelControls()
		{
			this._selectLabel.Text = SR.GetString("CreateDataSource_SelectType");
			this._idLabel.Text = SR.GetString("CreateDataSource_ID");
			base.OKButton.Enabled = false;
			this.Text = SR.GetString("CreateDataSource_Title");
			this._descriptionBox.Text = SR.GetString("CreateDataSource_SelectTypeDesc");
			base.AccessibleDescription = SR.GetString("CreateDataSource_Description");
			base.CaptionLabel.Text = SR.GetString("CreateDataSource_Caption");
			this.UpdateFonts();
			ISite site = this.GetSite();
			if (site != null)
			{
				IComponentDiscoveryService componentDiscoveryService = (IComponentDiscoveryService)site.GetService(typeof(IComponentDiscoveryService));
				IDesignerHost designerHost = null;
				if (componentDiscoveryService != null)
				{
					ICollection componentTypes = componentDiscoveryService.GetComponentTypes(designerHost, this._dataSourceType);
					if (componentTypes != null)
					{
						ImageList imageList = new ImageList();
						imageList.ColorDepth = ColorDepth.Depth32Bit;
						Type[] array = new Type[componentTypes.Count];
						componentTypes.CopyTo(array, 0);
						foreach (Type type in array)
						{
							AttributeCollection attributes = TypeDescriptor.GetAttributes(type);
							Bitmap bitmap = null;
							if (attributes != null)
							{
								ToolboxBitmapAttribute toolboxBitmapAttribute = attributes[typeof(ToolboxBitmapAttribute)] as ToolboxBitmapAttribute;
								if (toolboxBitmapAttribute != null && !toolboxBitmapAttribute.Equals(ToolboxBitmapAttribute.Default))
								{
									bitmap = toolboxBitmapAttribute.GetImage(type, true) as Bitmap;
								}
							}
							if (bitmap == null)
							{
								bitmap = new Bitmap(base.GetType(), "CustomDataSource.bmp");
							}
							imageList.ImageSize = new Size(32, 32);
							imageList.Images.Add(type.FullName, bitmap);
							this._dataSourceTypesListView.Items.Add(new CreateDataSourceDialog.DataSourceListViewItem(type));
						}
						this._dataSourceTypesListView.Sort();
						this._dataSourceTypesListView.LargeImageList = imageList;
					}
				}
			}
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000CCCE4 File Offset: 0x000CBCE4
		protected override void OnClosing(CancelEventArgs e)
		{
			if (base.DialogResult == DialogResult.OK && this._dataSourceTypesListView.SelectedItems.Count > 0)
			{
				CreateDataSourceDialog.DataSourceListViewItem dataSourceListViewItem = this._dataSourceTypesListView.SelectedItems[0] as CreateDataSourceDialog.DataSourceListViewItem;
				Type dataSourceType = dataSourceListViewItem.DataSourceType;
				string text = this.CreateNewDataSource(dataSourceType);
				if (text.Length > 0)
				{
					this._dataSourceID = text;
				}
				else
				{
					e.Cancel = true;
				}
				TypeDescriptor.Refresh(this.GetComponent());
			}
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x000CCD58 File Offset: 0x000CBD58
		private void OnDataSourceTypeChosen(object sender, EventArgs e)
		{
			if (this._dataSourceTypesListView.SelectedItems.Count > 0)
			{
				CreateDataSourceDialog.DataSourceListViewItem dataSourceListViewItem = this._dataSourceTypesListView.SelectedItems[0] as CreateDataSourceDialog.DataSourceListViewItem;
				Type dataSourceType = dataSourceListViewItem.DataSourceType;
				this._idTextBox.Text = this.GetNewDataSourceName(dataSourceType);
				this._descriptionBox.Text = dataSourceListViewItem.GetDescriptionText();
			}
			this.UpdateOKButtonEnabled();
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x000CCDBF File Offset: 0x000CBDBF
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.UpdateFonts();
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x000CCDCE File Offset: 0x000CBDCE
		private void OnIDChanged(object sender, EventArgs e)
		{
			this.UpdateOKButtonEnabled();
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x000CCDD6 File Offset: 0x000CBDD6
		private void OnListViewDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x000CCDF2 File Offset: 0x000CBDF2
		private void UpdateFonts()
		{
			this._selectLabel.Font = new Font(this.Font, FontStyle.Bold);
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x000CCE0C File Offset: 0x000CBE0C
		private void UpdateOKButtonEnabled()
		{
			if (this._idTextBox.Text.Length > 0 && this._dataSourceTypesListView.SelectedItems.Count > 0)
			{
				base.OKButton.Enabled = true;
				return;
			}
			base.OKButton.Enabled = false;
		}

		// Token: 0x040019FB RID: 6651
		private ControlDesigner _controlDesigner;

		// Token: 0x040019FC RID: 6652
		private string _controlID;

		// Token: 0x040019FD RID: 6653
		private Type _dataSourceType;

		// Token: 0x040019FE RID: 6654
		private CreateDataSourceDialog.DisplayNameComparer _displayNameComparer;

		// Token: 0x040019FF RID: 6655
		private string _dataSourceID;

		// Token: 0x04001A00 RID: 6656
		private bool _configure;

		// Token: 0x04001A01 RID: 6657
		private Label _selectLabel;

		// Token: 0x04001A02 RID: 6658
		private ListView _dataSourceTypesListView;

		// Token: 0x04001A03 RID: 6659
		private TextBox _descriptionBox;

		// Token: 0x04001A04 RID: 6660
		private Label _idLabel;

		// Token: 0x04001A05 RID: 6661
		private TextBox _idTextBox;

		// Token: 0x02000411 RID: 1041
		private class DataSourceListViewItem : ListViewItem
		{
			// Token: 0x0600260C RID: 9740 RVA: 0x000CCE58 File Offset: 0x000CBE58
			public DataSourceListViewItem(Type dataSourceType)
			{
				this._dataSourceType = dataSourceType;
				base.Text = this.GetDisplayName();
				base.ImageKey = this._dataSourceType.FullName;
			}

			// Token: 0x17000713 RID: 1811
			// (get) Token: 0x0600260D RID: 9741 RVA: 0x000CCE84 File Offset: 0x000CBE84
			public Type DataSourceType
			{
				get
				{
					return this._dataSourceType;
				}
			}

			// Token: 0x0600260E RID: 9742 RVA: 0x000CCE8C File Offset: 0x000CBE8C
			public string GetDescriptionText()
			{
				AttributeCollection attributes = TypeDescriptor.GetAttributes(this._dataSourceType);
				if (attributes != null)
				{
					DescriptionAttribute descriptionAttribute = attributes[typeof(DescriptionAttribute)] as DescriptionAttribute;
					if (descriptionAttribute != null)
					{
						return descriptionAttribute.Description;
					}
				}
				return string.Empty;
			}

			// Token: 0x0600260F RID: 9743 RVA: 0x000CCED0 File Offset: 0x000CBED0
			public string GetDisplayName()
			{
				if (this._displayName == null)
				{
					AttributeCollection attributes = TypeDescriptor.GetAttributes(this._dataSourceType);
					this._displayName = string.Empty;
					if (attributes != null)
					{
						DisplayNameAttribute displayNameAttribute = attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
						if (displayNameAttribute != null)
						{
							this._displayName = displayNameAttribute.DisplayName;
						}
					}
					if (string.IsNullOrEmpty(this._displayName))
					{
						this._displayName = this._dataSourceType.Name;
					}
				}
				return this._displayName;
			}

			// Token: 0x04001A06 RID: 6662
			private Type _dataSourceType;

			// Token: 0x04001A07 RID: 6663
			private string _displayName;
		}

		// Token: 0x02000412 RID: 1042
		private class DisplayNameComparer : IComparer
		{
			// Token: 0x06002610 RID: 9744 RVA: 0x000CCF48 File Offset: 0x000CBF48
			public int Compare(object x, object y)
			{
				if (!(x is CreateDataSourceDialog.DataSourceListViewItem) || !(y is CreateDataSourceDialog.DataSourceListViewItem))
				{
					return 0;
				}
				return this.Compare((CreateDataSourceDialog.DataSourceListViewItem)x, (CreateDataSourceDialog.DataSourceListViewItem)y);
			}

			// Token: 0x06002611 RID: 9745 RVA: 0x000CCF70 File Offset: 0x000CBF70
			private int Compare(CreateDataSourceDialog.DataSourceListViewItem x, CreateDataSourceDialog.DataSourceListViewItem y)
			{
				StringComparer stringComparer = StringComparer.Create(CultureInfo.CurrentCulture, true);
				return stringComparer.Compare(x.GetDisplayName(), y.GetDisplayName());
			}
		}
	}
}
