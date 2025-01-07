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
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed partial class CreateDataSourceDialog : TaskForm
	{
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

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.DataBoundControl.CreateDataSourceDialog";
			}
		}

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

		private void CreatePanel()
		{
			base.SuspendLayout();
			this.CreatePanelControls();
			this.InitializePanelControls();
			base.InitializeForm();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

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

		private IComponent GetComponent()
		{
			if (this._controlDesigner != null)
			{
				return this._controlDesigner.Component;
			}
			return null;
		}

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

		private ISite GetSite()
		{
			IComponent component = this.GetComponent();
			if (component != null)
			{
				return component.Site;
			}
			return null;
		}

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

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.UpdateFonts();
		}

		private void OnIDChanged(object sender, EventArgs e)
		{
			this.UpdateOKButtonEnabled();
		}

		private void OnListViewDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
		}

		private void UpdateFonts()
		{
			this._selectLabel.Font = new Font(this.Font, FontStyle.Bold);
		}

		private void UpdateOKButtonEnabled()
		{
			if (this._idTextBox.Text.Length > 0 && this._dataSourceTypesListView.SelectedItems.Count > 0)
			{
				base.OKButton.Enabled = true;
				return;
			}
			base.OKButton.Enabled = false;
		}

		private ControlDesigner _controlDesigner;

		private string _controlID;

		private Type _dataSourceType;

		private CreateDataSourceDialog.DisplayNameComparer _displayNameComparer;

		private string _dataSourceID;

		private bool _configure;

		private Label _selectLabel;

		private ListView _dataSourceTypesListView;

		private TextBox _descriptionBox;

		private Label _idLabel;

		private TextBox _idTextBox;

		private class DataSourceListViewItem : ListViewItem
		{
			public DataSourceListViewItem(Type dataSourceType)
			{
				this._dataSourceType = dataSourceType;
				base.Text = this.GetDisplayName();
				base.ImageKey = this._dataSourceType.FullName;
			}

			public Type DataSourceType
			{
				get
				{
					return this._dataSourceType;
				}
			}

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

			private Type _dataSourceType;

			private string _displayName;
		}

		private class DisplayNameComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				if (!(x is CreateDataSourceDialog.DataSourceListViewItem) || !(y is CreateDataSourceDialog.DataSourceListViewItem))
				{
					return 0;
				}
				return this.Compare((CreateDataSourceDialog.DataSourceListViewItem)x, (CreateDataSourceDialog.DataSourceListViewItem)y);
			}

			private int Compare(CreateDataSourceDialog.DataSourceListViewItem x, CreateDataSourceDialog.DataSourceListViewItem y)
			{
				StringComparer stringComparer = StringComparer.Create(CultureInfo.CurrentCulture, true);
				return stringComparer.Compare(x.GetDisplayName(), y.GetDisplayName());
			}
		}
	}
}
