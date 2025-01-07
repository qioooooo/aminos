using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Data;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	[DesignTimeVisible(false)]
	[ToolboxItem(false)]
	internal class DesignBindingPicker : ContainerControl
	{
		public DesignBindingPicker()
		{
			base.SuspendLayout();
			this.treeViewCtrl = new DesignBindingPicker.BindingPickerTree();
			this.treeViewCtrl.HotTracking = true;
			this.treeViewCtrl.BackColor = SystemColors.Window;
			this.treeViewCtrl.ForeColor = SystemColors.WindowText;
			this.treeViewCtrl.BorderStyle = BorderStyle.None;
			Size size = this.treeViewCtrl.Size;
			this.treeViewCtrl.Dock = DockStyle.Fill;
			this.treeViewCtrl.MouseMove += this.treeViewCtrl_MouseMove;
			this.treeViewCtrl.MouseLeave += this.treeViewCtrl_MouseLeave;
			this.treeViewCtrl.AfterExpand += this.treeViewCtrl_AfterExpand;
			this.treeViewCtrl.AccessibleName = SR.GetString("DesignBindingPickerTreeViewAccessibleName");
			Label label = new Label();
			label.Height = 1;
			label.BackColor = SystemColors.ControlDark;
			label.Dock = DockStyle.Top;
			this.addNewCtrl = new DesignBindingPicker.BindingPickerLink();
			this.addNewCtrl.Text = SR.GetString("DesignBindingPickerAddProjDataSourceLabel");
			this.addNewCtrl.TextAlign = ContentAlignment.MiddleLeft;
			this.addNewCtrl.BackColor = SystemColors.Window;
			this.addNewCtrl.ForeColor = SystemColors.WindowText;
			this.addNewCtrl.LinkBehavior = LinkBehavior.HoverUnderline;
			int height = this.addNewCtrl.Height;
			this.addNewCtrl.Dock = DockStyle.Fill;
			this.addNewCtrl.LinkClicked += this.addNewCtrl_Click;
			Bitmap bitmap = new Bitmap(typeof(DesignBindingPicker), "AddNewDataSource.bmp");
			bitmap.MakeTransparent(Color.Magenta);
			PictureBox pictureBox = new PictureBox();
			pictureBox.Image = bitmap;
			pictureBox.BackColor = SystemColors.Window;
			pictureBox.ForeColor = SystemColors.WindowText;
			pictureBox.Width = height;
			pictureBox.Height = height;
			pictureBox.Dock = DockStyle.Left;
			pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
			pictureBox.AccessibleRole = AccessibleRole.Graphic;
			this.addNewPanel = new Panel();
			this.addNewPanel.Controls.Add(this.addNewCtrl);
			this.addNewPanel.Controls.Add(pictureBox);
			this.addNewPanel.Controls.Add(label);
			this.addNewPanel.Height = height + 1;
			this.addNewPanel.Dock = DockStyle.Bottom;
			Label label2 = new Label();
			label2.Height = 1;
			label2.BackColor = SystemColors.ControlDark;
			label2.Dock = DockStyle.Top;
			this.helpTextCtrl = new DesignBindingPicker.HelpTextLabel();
			this.helpTextCtrl.TextAlign = ContentAlignment.TopLeft;
			this.helpTextCtrl.BackColor = SystemColors.Window;
			this.helpTextCtrl.ForeColor = SystemColors.WindowText;
			this.helpTextCtrl.Height *= 2;
			int height2 = this.helpTextCtrl.Height;
			this.helpTextCtrl.Dock = DockStyle.Fill;
			this.helpTextPanel = new Panel();
			this.helpTextPanel.Controls.Add(this.helpTextCtrl);
			this.helpTextPanel.Controls.Add(label2);
			this.helpTextPanel.Height = height2 + 1;
			this.helpTextPanel.Dock = DockStyle.Bottom;
			base.Controls.Add(this.treeViewCtrl);
			base.Controls.Add(this.addNewPanel);
			base.Controls.Add(this.helpTextPanel);
			base.ResumeLayout(false);
			base.Size = size;
			this.BackColor = SystemColors.Control;
			base.ActiveControl = this.treeViewCtrl;
			base.AccessibleName = SR.GetString("DesignBindingPickerAccessibleName");
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
		}

		public DesignBinding Pick(ITypeDescriptorContext context, IServiceProvider provider, bool showDataSources, bool showDataMembers, bool selectListMembers, object rootDataSource, string rootDataMember, DesignBinding initialSelectedItem)
		{
			this.serviceProvider = provider;
			this.edSvc = (IWindowsFormsEditorService)this.serviceProvider.GetService(typeof(IWindowsFormsEditorService));
			this.dspSvc = (DataSourceProviderService)this.serviceProvider.GetService(typeof(DataSourceProviderService));
			this.typeSvc = (ITypeResolutionService)this.serviceProvider.GetService(typeof(ITypeResolutionService));
			this.hostSvc = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
			if (this.edSvc == null)
			{
				return null;
			}
			this.context = context;
			this.showDataSources = showDataSources;
			this.showDataMembers = showDataMembers;
			this.selectListMembers = !showDataMembers || selectListMembers;
			this.rootDataSource = rootDataSource;
			this.rootDataMember = rootDataMember;
			IUIService iuiservice = this.serviceProvider.GetService(typeof(IUIService)) as IUIService;
			if (iuiservice != null)
			{
				if (iuiservice.Styles["VsColorPanelHyperLink"] is Color)
				{
					this.addNewCtrl.LinkColor = (Color)iuiservice.Styles["VsColorPanelHyperLink"];
				}
				if (iuiservice.Styles["VsColorPanelHyperLinkPressed"] is Color)
				{
					this.addNewCtrl.ActiveLinkColor = (Color)iuiservice.Styles["VsColorPanelHyperLinkPressed"];
				}
			}
			this.FillTree(initialSelectedItem);
			this.addNewPanel.Visible = showDataSources && this.dspSvc != null && this.dspSvc.SupportsAddNewDataSource;
			this.helpTextPanel.Visible = showDataSources;
			this.UpdateHelpText(null);
			this.edSvc.DropDownControl(this);
			DesignBinding designBinding = this.selectedItem;
			this.selectedItem = null;
			this.EmptyTree();
			this.serviceProvider = null;
			this.edSvc = null;
			this.dspSvc = null;
			this.hostSvc = null;
			context = null;
			return designBinding;
		}

		private void CloseDropDown()
		{
			if (this.context.Instance is BindingSource && this.hostSvc != null)
			{
				BindingSourceDesigner bindingSourceDesigner = this.hostSvc.GetDesigner(this.context.Instance as IComponent) as BindingSourceDesigner;
				if (bindingSourceDesigner != null)
				{
					bindingSourceDesigner.BindingUpdatedByUser = true;
				}
			}
			if (this.edSvc != null)
			{
				this.edSvc.CloseDropDown();
			}
		}

		private void EmptyTree()
		{
			this.noneNode = null;
			this.otherNode = null;
			this.projectNode = null;
			this.instancesNode = null;
			this.selectedNode = null;
			this.treeViewCtrl.Nodes.Clear();
		}

		private void FillTree(DesignBinding initialSelectedItem)
		{
			this.selectedItem = initialSelectedItem;
			this.EmptyTree();
			this.noneNode = new DesignBindingPicker.NoneNode();
			this.otherNode = new DesignBindingPicker.OtherNode();
			this.projectNode = new DesignBindingPicker.ProjectNode(this);
			if (this.hostSvc != null && this.hostSvc.RootComponent != null && this.hostSvc.RootComponent.Site != null)
			{
				this.instancesNode = new DesignBindingPicker.InstancesNode(this.hostSvc.RootComponent.Site.Name);
			}
			else
			{
				this.instancesNode = new DesignBindingPicker.InstancesNode(string.Empty);
			}
			this.treeViewCtrl.Nodes.Add(this.noneNode);
			if (this.showDataSources)
			{
				this.AddFormDataSources();
				this.AddProjectDataSources();
				if (this.projectNode.Nodes.Count > 0)
				{
					this.otherNode.Nodes.Add(this.projectNode);
				}
				if (this.instancesNode.Nodes.Count > 0)
				{
					this.otherNode.Nodes.Add(this.instancesNode);
				}
				if (this.otherNode.Nodes.Count > 0)
				{
					this.treeViewCtrl.Nodes.Add(this.otherNode);
				}
			}
			else
			{
				this.AddDataSourceContents(this.treeViewCtrl.Nodes, this.rootDataSource, this.rootDataMember, null);
			}
			if (this.selectedNode == null)
			{
				this.selectedNode = this.noneNode;
			}
			this.selectedItem = null;
			base.Width = Math.Max(base.Width, this.treeViewCtrl.PreferredWidth + SystemInformation.VerticalScrollBarWidth * 2);
		}

		private void AddFormDataSources()
		{
			IContainer container = null;
			if (this.context != null)
			{
				container = this.context.Container;
			}
			if (container == null && this.hostSvc != null)
			{
				container = this.hostSvc.Container;
			}
			if (container == null)
			{
				return;
			}
			container = DesignerUtils.CheckForNestedContainer(container);
			ComponentCollection components = container.Components;
			foreach (object obj in components)
			{
				IComponent component = (IComponent)obj;
				if (component != this.context.Instance && (!(component is DataTable) || !this.FindComponent(components, (component as DataTable).DataSet)))
				{
					if (component is BindingSource)
					{
						this.AddDataSource(this.treeViewCtrl.Nodes, component, null);
					}
					else
					{
						this.AddDataSource(this.instancesNode.Nodes, component, null);
					}
				}
			}
		}

		private void AddDataSource(TreeNodeCollection nodes, IComponent dataSource, string dataMember)
		{
			if (!this.showDataSources)
			{
				return;
			}
			if (!this.IsBindableDataSource(dataSource))
			{
				return;
			}
			string text = null;
			PropertyDescriptorCollection propertyDescriptorCollection = null;
			try
			{
				propertyDescriptorCollection = this.GetItemProperties(dataSource, dataMember);
				if (propertyDescriptorCollection == null)
				{
					return;
				}
			}
			catch (ArgumentException ex)
			{
				text = ex.Message;
			}
			if (!this.showDataMembers || propertyDescriptorCollection.Count != 0)
			{
				DesignBindingPicker.DataSourceNode dataSourceNode = new DesignBindingPicker.DataSourceNode(this, dataSource, dataSource.Site.Name);
				nodes.Add(dataSourceNode);
				if (this.selectedItem != null && this.selectedItem.Equals(dataSource, ""))
				{
					this.selectedNode = dataSourceNode;
				}
				if (text == null)
				{
					this.AddDataSourceContents(dataSourceNode.Nodes, dataSource, dataMember, propertyDescriptorCollection);
					dataSourceNode.SubNodesFilled = true;
					return;
				}
				dataSourceNode.Error = text;
				dataSourceNode.ForeColor = SystemColors.GrayText;
				return;
			}
		}

		private void AddDataSourceContents(TreeNodeCollection nodes, object dataSource, string dataMember, PropertyDescriptorCollection properties)
		{
			if (!this.showDataMembers && !(dataSource is BindingSource))
			{
				return;
			}
			if (dataSource is Type)
			{
				try
				{
					dataSource = new BindingSource
					{
						DataSource = dataSource
					}.List;
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
				}
				catch
				{
				}
			}
			if (!this.IsBindableDataSource(dataSource))
			{
				return;
			}
			if (properties == null)
			{
				properties = this.GetItemProperties(dataSource, dataMember);
				if (properties == null)
				{
					return;
				}
			}
			for (int i = 0; i < properties.Count; i++)
			{
				PropertyDescriptor propertyDescriptor = properties[i];
				if (this.IsBindableDataMember(propertyDescriptor))
				{
					string text = (string.IsNullOrEmpty(dataMember) ? propertyDescriptor.Name : (dataMember + "." + propertyDescriptor.Name));
					this.AddDataMember(nodes, dataSource, text, propertyDescriptor.Name, this.IsListMember(propertyDescriptor));
				}
			}
		}

		private void AddDataMember(TreeNodeCollection nodes, object dataSource, string dataMember, string propertyName, bool isList)
		{
			bool flag = isList && dataSource is BindingSource;
			bool flag2 = this.showDataMembers && !this.selectListMembers;
			bool flag3 = flag && flag2;
			bool flag4 = (flag && !flag2) || this.context.Instance is BindingSource;
			if (flag3)
			{
				return;
			}
			if (this.selectListMembers && !isList)
			{
				return;
			}
			DesignBindingPicker.DataMemberNode dataMemberNode = new DesignBindingPicker.DataMemberNode(this, dataSource, dataMember, propertyName, isList);
			nodes.Add(dataMemberNode);
			if (this.selectedItem != null && this.selectedItem.Equals(dataSource, dataMember) && dataMemberNode != null)
			{
				this.selectedNode = dataMemberNode;
			}
			if (!flag4)
			{
				this.AddDataMemberContents(dataMemberNode);
			}
		}

		private void AddDataMemberContents(TreeNodeCollection nodes, object dataSource, string dataMember, bool isList)
		{
			if (!isList)
			{
				return;
			}
			PropertyDescriptorCollection itemProperties = this.GetItemProperties(dataSource, dataMember);
			if (itemProperties == null)
			{
				return;
			}
			for (int i = 0; i < itemProperties.Count; i++)
			{
				PropertyDescriptor propertyDescriptor = itemProperties[i];
				if (this.IsBindableDataMember(propertyDescriptor))
				{
					bool flag = this.IsListMember(propertyDescriptor);
					if (!this.selectListMembers || flag)
					{
						DesignBindingPicker.DataMemberNode dataMemberNode = new DesignBindingPicker.DataMemberNode(this, dataSource, dataMember + "." + propertyDescriptor.Name, propertyDescriptor.Name, flag);
						nodes.Add(dataMemberNode);
						if (this.selectedItem != null && this.selectedItem.DataSource == dataMemberNode.DataSource)
						{
							if (this.selectedItem.Equals(dataSource, dataMemberNode.DataMember))
							{
								this.selectedNode = dataMemberNode;
							}
							else if (!string.IsNullOrEmpty(this.selectedItem.DataMember) && this.selectedItem.DataMember.IndexOf(dataMemberNode.DataMember) == 0)
							{
								this.AddDataMemberContents(dataMemberNode);
							}
						}
					}
				}
			}
		}

		private void AddDataMemberContents(TreeNodeCollection nodes, DesignBindingPicker.DataMemberNode dataMemberNode)
		{
			this.AddDataMemberContents(nodes, dataMemberNode.DataSource, dataMemberNode.DataMember, dataMemberNode.IsList);
		}

		private void AddDataMemberContents(DesignBindingPicker.DataMemberNode dataMemberNode)
		{
			this.AddDataMemberContents(dataMemberNode.Nodes, dataMemberNode);
		}

		private void AddProjectDataSources()
		{
			if (this.dspSvc == null)
			{
				return;
			}
			DataSourceGroupCollection dataSources = this.dspSvc.GetDataSources();
			if (dataSources == null)
			{
				return;
			}
			bool flag = this.selectedItem != null && this.selectedItem.DataSource is DataSourceDescriptor;
			foreach (object obj in dataSources)
			{
				DataSourceGroup dataSourceGroup = (DataSourceGroup)obj;
				if (dataSourceGroup != null)
				{
					if (dataSourceGroup.IsDefault)
					{
						this.AddProjectGroupContents(this.projectNode.Nodes, dataSourceGroup);
					}
					else
					{
						this.AddProjectGroup(this.projectNode.Nodes, dataSourceGroup, flag);
					}
				}
			}
			if (flag)
			{
				this.projectNode.FillSubNodes();
			}
		}

		private void AddProjectGroup(TreeNodeCollection nodes, DataSourceGroup group, bool addMembers)
		{
			DesignBindingPicker.ProjectGroupNode projectGroupNode = new DesignBindingPicker.ProjectGroupNode(this, group.Name, group.Image);
			this.AddProjectGroupContents(projectGroupNode.Nodes, group);
			nodes.Add(projectGroupNode);
			if (addMembers)
			{
				projectGroupNode.FillSubNodes();
			}
		}

		private void AddProjectGroupContents(TreeNodeCollection nodes, DataSourceGroup group)
		{
			DataSourceDescriptorCollection dataSources = group.DataSources;
			if (dataSources == null)
			{
				return;
			}
			foreach (object obj in dataSources)
			{
				DataSourceDescriptor dataSourceDescriptor = (DataSourceDescriptor)obj;
				if (dataSourceDescriptor != null)
				{
					this.AddProjectDataSource(nodes, dataSourceDescriptor);
				}
			}
		}

		private void AddProjectDataSource(TreeNodeCollection nodes, DataSourceDescriptor dsd)
		{
			Type type = this.GetType(dsd.TypeName, true, true);
			if (type != null && type.GetType() != DesignBindingPicker.runtimeType)
			{
				return;
			}
			DesignBindingPicker.ProjectDataSourceNode projectDataSourceNode = new DesignBindingPicker.ProjectDataSourceNode(this, dsd, dsd.Name, dsd.Image);
			nodes.Add(projectDataSourceNode);
			if (this.selectedItem != null && string.IsNullOrEmpty(this.selectedItem.DataMember))
			{
				if (this.selectedItem.DataSource is DataSourceDescriptor && string.Equals(dsd.Name, (this.selectedItem.DataSource as DataSourceDescriptor).Name, StringComparison.OrdinalIgnoreCase))
				{
					this.selectedNode = projectDataSourceNode;
					return;
				}
				if (this.selectedItem.DataSource is Type && string.Equals(dsd.TypeName, (this.selectedItem.DataSource as Type).FullName, StringComparison.OrdinalIgnoreCase))
				{
					this.selectedNode = projectDataSourceNode;
				}
			}
		}

		private void AddProjectDataSourceContents(TreeNodeCollection nodes, DesignBindingPicker.DataSourceNode projectDataSourceNode)
		{
			DataSourceDescriptor dataSourceDescriptor = projectDataSourceNode.DataSource as DataSourceDescriptor;
			if (dataSourceDescriptor == null)
			{
				return;
			}
			Type type = this.GetType(dataSourceDescriptor.TypeName, false, false);
			if (type == null)
			{
				return;
			}
			object obj = type;
			try
			{
				obj = Activator.CreateInstance(type);
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsCriticalException(ex))
				{
					throw;
				}
			}
			catch
			{
			}
			bool flag = obj is IListSource && (obj as IListSource).ContainsListCollection;
			if (flag && this.context.Instance is BindingSource)
			{
				return;
			}
			PropertyDescriptorCollection listItemProperties = ListBindingHelper.GetListItemProperties(obj);
			if (listItemProperties == null)
			{
				return;
			}
			foreach (object obj2 in listItemProperties)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj2;
				if (this.IsBindableDataMember(propertyDescriptor) && propertyDescriptor.IsBrowsable)
				{
					bool flag2 = this.IsListMember(propertyDescriptor);
					if ((!this.selectListMembers || flag2) && (flag || !flag2))
					{
						this.AddProjectDataMember(nodes, dataSourceDescriptor, propertyDescriptor, obj, flag2);
					}
				}
			}
		}

		private void AddProjectDataSourceContents(DesignBindingPicker.DataSourceNode projectDataSourceNode)
		{
			this.AddProjectDataSourceContents(projectDataSourceNode.Nodes, projectDataSourceNode);
		}

		private void AddProjectDataMember(TreeNodeCollection nodes, DataSourceDescriptor dsd, PropertyDescriptor pd, object dataSourceInstance, bool isList)
		{
			Type type = this.GetType(dsd.TypeName, true, true);
			if (type != null && type.GetType() != DesignBindingPicker.runtimeType)
			{
				return;
			}
			DesignBindingPicker.DataMemberNode dataMemberNode = new DesignBindingPicker.ProjectDataMemberNode(this, dsd, pd.Name, pd.Name, isList);
			nodes.Add(dataMemberNode);
			this.AddProjectDataMemberContents(dataMemberNode, dsd, pd, dataSourceInstance);
		}

		private void AddProjectDataMemberContents(TreeNodeCollection nodes, DesignBindingPicker.DataMemberNode projectDataMemberNode, DataSourceDescriptor dsd, PropertyDescriptor propDesc, object dataSourceInstance)
		{
			if (this.selectListMembers)
			{
				return;
			}
			if (!projectDataMemberNode.IsList)
			{
				return;
			}
			if (dataSourceInstance == null)
			{
				return;
			}
			PropertyDescriptorCollection listItemProperties = ListBindingHelper.GetListItemProperties(dataSourceInstance, new PropertyDescriptor[] { propDesc });
			if (listItemProperties == null)
			{
				return;
			}
			foreach (object obj in listItemProperties)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
				if (this.IsBindableDataMember(propertyDescriptor) && propertyDescriptor.IsBrowsable)
				{
					bool flag = this.IsListMember(propertyDescriptor);
					if (!flag)
					{
						this.AddProjectDataMember(nodes, dsd, propertyDescriptor, dataSourceInstance, flag);
					}
				}
			}
		}

		private void AddProjectDataMemberContents(DesignBindingPicker.DataMemberNode projectDataMemberNode, DataSourceDescriptor dsd, PropertyDescriptor pd, object dataSourceInstance)
		{
			this.AddProjectDataMemberContents(projectDataMemberNode.Nodes, projectDataMemberNode, dsd, pd, dataSourceInstance);
		}

		private BindingSource CreateNewBindingSource(object dataSource, string dataMember)
		{
			if (this.hostSvc == null || this.dspSvc == null)
			{
				return null;
			}
			BindingSource bindingSource = new BindingSource();
			try
			{
				bindingSource.DataSource = dataSource;
				bindingSource.DataMember = dataMember;
			}
			catch (Exception ex)
			{
				IUIService iuiservice = this.serviceProvider.GetService(typeof(IUIService)) as IUIService;
				DataGridViewDesigner.ShowErrorDialog(iuiservice, ex, this);
				return null;
			}
			string text = this.GetBindingSourceNamePrefix(dataSource, dataMember);
			if (this.serviceProvider != null)
			{
				text = ToolStripDesigner.NameFromText(text, bindingSource.GetType(), this.serviceProvider);
			}
			else
			{
				text += bindingSource.GetType().Name;
			}
			string uniqueSiteName = DesignerUtils.GetUniqueSiteName(this.hostSvc, text);
			DesignerTransaction designerTransaction = this.hostSvc.CreateTransaction(SR.GetString("DesignerBatchCreateTool", new object[] { uniqueSiteName }));
			try
			{
				try
				{
					this.hostSvc.Container.Add(bindingSource, uniqueSiteName);
				}
				catch (InvalidOperationException ex2)
				{
					if (designerTransaction != null)
					{
						designerTransaction.Cancel();
					}
					IUIService iuiservice2 = this.serviceProvider.GetService(typeof(IUIService)) as IUIService;
					DataGridViewDesigner.ShowErrorDialog(iuiservice2, ex2, this);
					return null;
				}
				catch (CheckoutException ex3)
				{
					if (designerTransaction != null)
					{
						designerTransaction.Cancel();
					}
					IUIService iuiservice3 = this.serviceProvider.GetService(typeof(IUIService)) as IUIService;
					DataGridViewDesigner.ShowErrorDialog(iuiservice3, ex3, this);
					return null;
				}
				this.dspSvc.NotifyDataSourceComponentAdded(bindingSource);
				if (designerTransaction != null)
				{
					designerTransaction.Commit();
					designerTransaction = null;
				}
			}
			finally
			{
				if (designerTransaction != null)
				{
					designerTransaction.Cancel();
				}
			}
			return bindingSource;
		}

		private BindingSource CreateNewBindingSource(DataSourceDescriptor dataSourceDescriptor, string dataMember)
		{
			if (this.hostSvc == null || this.dspSvc == null)
			{
				return null;
			}
			object projectDataSourceInstance = this.GetProjectDataSourceInstance(dataSourceDescriptor);
			if (projectDataSourceInstance == null)
			{
				return null;
			}
			return this.CreateNewBindingSource(projectDataSourceInstance, dataMember);
		}

		private string GetBindingSourceNamePrefix(object dataSource, string dataMember)
		{
			if (!string.IsNullOrEmpty(dataMember))
			{
				return dataMember;
			}
			if (dataSource == null)
			{
				return "";
			}
			Type type = dataSource as Type;
			if (type != null)
			{
				return type.Name;
			}
			IComponent component = dataSource as IComponent;
			if (component != null)
			{
				ISite site = component.Site;
				if (site != null && !string.IsNullOrEmpty(site.Name))
				{
					return site.Name;
				}
			}
			return dataSource.GetType().Name;
		}

		private Type GetType(string name, bool throwOnError, bool ignoreCase)
		{
			if (this.typeSvc != null)
			{
				return this.typeSvc.GetType(name, throwOnError, ignoreCase);
			}
			return Type.GetType(name, throwOnError, ignoreCase);
		}

		private object GetProjectDataSourceInstance(DataSourceDescriptor dataSourceDescriptor)
		{
			Type type = this.GetType(dataSourceDescriptor.TypeName, true, true);
			if (!dataSourceDescriptor.IsDesignable)
			{
				return type;
			}
			foreach (object obj in this.hostSvc.Container.Components)
			{
				IComponent component = (IComponent)obj;
				if (type.Equals(component.GetType()))
				{
					return component;
				}
			}
			object obj2;
			try
			{
				obj2 = this.dspSvc.AddDataSourceInstance(this.hostSvc, dataSourceDescriptor);
			}
			catch (InvalidOperationException ex)
			{
				IUIService iuiservice = this.serviceProvider.GetService(typeof(IUIService)) as IUIService;
				DataGridViewDesigner.ShowErrorDialog(iuiservice, ex, this);
				obj2 = null;
			}
			catch (CheckoutException ex2)
			{
				IUIService iuiservice2 = this.serviceProvider.GetService(typeof(IUIService)) as IUIService;
				DataGridViewDesigner.ShowErrorDialog(iuiservice2, ex2, this);
				obj2 = null;
			}
			return obj2;
		}

		private bool FindComponent(ComponentCollection components, IComponent targetComponent)
		{
			foreach (object obj in components)
			{
				IComponent component = (IComponent)obj;
				if (component == targetComponent)
				{
					return true;
				}
			}
			return false;
		}

		private bool IsBindableDataSource(object dataSource)
		{
			if (!(dataSource is IListSource) && !(dataSource is IList) && !(dataSource is Array))
			{
				return false;
			}
			ListBindableAttribute listBindableAttribute = (ListBindableAttribute)TypeDescriptor.GetAttributes(dataSource)[typeof(ListBindableAttribute)];
			return listBindableAttribute == null || listBindableAttribute.ListBindable;
		}

		private bool IsBindableDataMember(PropertyDescriptor property)
		{
			if (typeof(byte[]).IsAssignableFrom(property.PropertyType))
			{
				return true;
			}
			ListBindableAttribute listBindableAttribute = (ListBindableAttribute)property.Attributes[typeof(ListBindableAttribute)];
			return listBindableAttribute == null || listBindableAttribute.ListBindable;
		}

		private bool IsListMember(PropertyDescriptor property)
		{
			return !typeof(byte[]).IsAssignableFrom(property.PropertyType) && typeof(IList).IsAssignableFrom(property.PropertyType);
		}

		private PropertyDescriptorCollection GetItemProperties(object dataSource, string dataMember)
		{
			CurrencyManager currencyManager = (CurrencyManager)this.bindingContext[dataSource, dataMember];
			if (currencyManager != null)
			{
				return currencyManager.GetItemProperties();
			}
			return null;
		}

		private void UpdateHelpText(DesignBindingPicker.BindingPickerNode mouseNode)
		{
			string text = ((mouseNode == null) ? null : mouseNode.HelpText);
			string text2 = ((mouseNode == null) ? null : mouseNode.Error);
			if (text != null || text2 != null)
			{
				this.helpTextCtrl.BackColor = SystemColors.Info;
				this.helpTextCtrl.ForeColor = SystemColors.InfoText;
			}
			else
			{
				this.helpTextCtrl.BackColor = SystemColors.Window;
				this.helpTextCtrl.ForeColor = SystemColors.WindowText;
			}
			if (text2 != null)
			{
				this.helpTextCtrl.Text = text2;
				return;
			}
			if (text != null)
			{
				this.helpTextCtrl.Text = text;
				return;
			}
			if (this.selectedNode != null && this.selectedNode != this.noneNode)
			{
				this.helpTextCtrl.Text = string.Format(CultureInfo.CurrentCulture, SR.GetString("DesignBindingPickerHelpGenCurrentBinding"), new object[] { this.selectedNode.Text });
				return;
			}
			if (!this.showDataSources)
			{
				this.helpTextCtrl.Text = ((this.treeViewCtrl.Nodes.Count > 1) ? SR.GetString("DesignBindingPickerHelpGenPickMember") : "");
				return;
			}
			if (this.treeViewCtrl.Nodes.Count > 1 && this.treeViewCtrl.Nodes[1] is DesignBindingPicker.DataSourceNode)
			{
				this.helpTextCtrl.Text = SR.GetString("DesignBindingPickerHelpGenPickBindSrc");
				return;
			}
			if (this.instancesNode.Nodes.Count > 0 || this.projectNode.Nodes.Count > 0)
			{
				this.helpTextCtrl.Text = SR.GetString("DesignBindingPickerHelpGenPickDataSrc");
				return;
			}
			if (this.addNewPanel.Visible)
			{
				this.helpTextCtrl.Text = SR.GetString("DesignBindingPickerHelpGenAddDataSrc");
				return;
			}
			this.helpTextCtrl.Text = "";
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			this.treeViewCtrl.Focus();
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (base.Visible)
			{
				this.ShowSelectedNode();
			}
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.Width) == BoundsSpecified.Width)
			{
				width = Math.Max(width, 250);
			}
			if ((specified & BoundsSpecified.Height) == BoundsSpecified.Height)
			{
				height = Math.Max(height, 250);
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		private void addNewCtrl_Click(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (this.dspSvc == null || !this.dspSvc.SupportsAddNewDataSource)
			{
				return;
			}
			DataSourceGroup dataSourceGroup = this.dspSvc.InvokeAddNewDataSource(this, FormStartPosition.CenterScreen);
			if (dataSourceGroup == null || dataSourceGroup.DataSources.Count == 0)
			{
				return;
			}
			DataSourceDescriptor dataSourceDescriptor = dataSourceGroup.DataSources[0];
			this.FillTree(new DesignBinding(dataSourceDescriptor, ""));
			if (this.selectedNode == null)
			{
				return;
			}
			int count = this.selectedNode.Nodes.Count;
			if (this.context.Instance is BindingSource)
			{
				this.treeViewCtrl.SetSelectedItem(this.selectedNode);
			}
			if (count == 0 || this.context.Instance is BindingSource)
			{
				this.treeViewCtrl.SetSelectedItem(this.selectedNode);
				return;
			}
			if (count == 1)
			{
				this.treeViewCtrl.SetSelectedItem(this.selectedNode.Nodes[0]);
				return;
			}
			this.ShowSelectedNode();
			this.selectedNode.Expand();
			this.selectedNode = null;
			this.UpdateHelpText(null);
		}

		private void treeViewCtrl_MouseMove(object sender, MouseEventArgs e)
		{
			Point point = new Point(e.X, e.Y);
			TreeNode treeNode = this.treeViewCtrl.GetNodeAt(point);
			if (treeNode != null && !treeNode.Bounds.Contains(point))
			{
				treeNode = null;
			}
			this.UpdateHelpText(treeNode as DesignBindingPicker.BindingPickerNode);
		}

		private void treeViewCtrl_MouseLeave(object sender, EventArgs e)
		{
			this.UpdateHelpText(null);
		}

		private void treeViewCtrl_AfterExpand(object sender, TreeViewEventArgs tvcevent)
		{
			if (this.inSelectNode || !base.Visible)
			{
				return;
			}
			(tvcevent.Node as DesignBindingPicker.BindingPickerNode).OnExpand();
		}

		private void ShowSelectedNode()
		{
			this.PostSelectTreeNode(this.selectedNode);
		}

		private void SelectTreeNode(TreeNode node)
		{
			if (this.inSelectNode)
			{
				return;
			}
			try
			{
				this.inSelectNode = true;
				this.treeViewCtrl.BeginUpdate();
				this.treeViewCtrl.SelectedNode = node;
				this.treeViewCtrl.EndUpdate();
			}
			finally
			{
				this.inSelectNode = false;
			}
		}

		private void PostSelectTreeNodeCallback(TreeNode node)
		{
			this.SelectTreeNode(null);
			this.SelectTreeNode(node);
		}

		private void PostSelectTreeNode(TreeNode node)
		{
			if (node != null && base.IsHandleCreated)
			{
				base.BeginInvoke(new DesignBindingPicker.PostSelectTreeNodeDelegate(this.PostSelectTreeNodeCallback), new object[] { node });
			}
		}

		private const int minimumHeight = 250;

		private const int minimumWidth = 250;

		private DesignBindingPicker.BindingPickerTree treeViewCtrl;

		private DesignBindingPicker.BindingPickerLink addNewCtrl;

		private Panel addNewPanel;

		private DesignBindingPicker.HelpTextLabel helpTextCtrl;

		private Panel helpTextPanel;

		private IServiceProvider serviceProvider;

		private IWindowsFormsEditorService edSvc;

		private DataSourceProviderService dspSvc;

		private ITypeResolutionService typeSvc;

		private IDesignerHost hostSvc;

		private bool showDataSources;

		private bool showDataMembers;

		private bool selectListMembers;

		private object rootDataSource;

		private string rootDataMember;

		private DesignBinding selectedItem;

		private TreeNode selectedNode;

		private bool inSelectNode;

		private DesignBindingPicker.NoneNode noneNode;

		private DesignBindingPicker.OtherNode otherNode;

		private DesignBindingPicker.ProjectNode projectNode;

		private DesignBindingPicker.InstancesNode instancesNode;

		private ITypeDescriptorContext context;

		private BindingContext bindingContext = new BindingContext();

		private static Type runtimeType = typeof(object).GetType().GetType();

		private delegate void PostSelectTreeNodeDelegate(TreeNode node);

		internal class HelpTextLabel : Label
		{
			protected override void OnPaint(PaintEventArgs e)
			{
				TextFormatFlags textFormatFlags = TextFormatFlags.EndEllipsis | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak;
				Rectangle rectangle = new Rectangle(base.ClientRectangle.Location, base.ClientRectangle.Size);
				rectangle.Inflate(-2, -2);
				TextRenderer.DrawText(e.Graphics, this.Text, this.Font, rectangle, this.ForeColor, textFormatFlags);
			}
		}

		internal class BindingPickerLink : LinkLabel
		{
			protected override bool IsInputKey(Keys key)
			{
				return key == Keys.Return || base.IsInputKey(key);
			}
		}

		internal class BindingPickerTree : TreeView
		{
			internal BindingPickerTree()
			{
				Image image = new Bitmap(typeof(DesignBindingPicker), "DataPickerImages.bmp");
				ImageList imageList = new ImageList();
				imageList.TransparentColor = Color.Magenta;
				imageList.Images.AddStrip(image);
				imageList.ColorDepth = ColorDepth.Depth24Bit;
				base.ImageList = imageList;
			}

			internal int PreferredWidth
			{
				get
				{
					return this.GetMaxItemWidth(base.Nodes);
				}
			}

			private int GetMaxItemWidth(TreeNodeCollection nodes)
			{
				int num = 0;
				foreach (object obj in nodes)
				{
					TreeNode treeNode = (TreeNode)obj;
					Rectangle bounds = treeNode.Bounds;
					int num2 = bounds.Left + bounds.Width;
					num = Math.Max(num2, num);
					if (treeNode.IsExpanded)
					{
						num = Math.Max(num, this.GetMaxItemWidth(treeNode.Nodes));
					}
				}
				return num;
			}

			public void SetSelectedItem(TreeNode node)
			{
				DesignBindingPicker designBindingPicker = base.Parent as DesignBindingPicker;
				if (designBindingPicker == null)
				{
					return;
				}
				DesignBindingPicker.BindingPickerNode bindingPickerNode = node as DesignBindingPicker.BindingPickerNode;
				designBindingPicker.selectedItem = ((bindingPickerNode.CanSelect && bindingPickerNode.Error == null) ? bindingPickerNode.OnSelect() : null);
				if (designBindingPicker.selectedItem != null)
				{
					designBindingPicker.CloseDropDown();
				}
			}

			protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
			{
				TreeViewHitTestInfo treeViewHitTestInfo = base.HitTest(new Point(e.X, e.Y));
				if (treeViewHitTestInfo.Node == e.Node && (treeViewHitTestInfo.Location == TreeViewHitTestLocations.Image || treeViewHitTestInfo.Location == TreeViewHitTestLocations.Label))
				{
					this.SetSelectedItem(e.Node);
				}
				base.OnNodeMouseClick(e);
			}

			protected override void OnKeyUp(KeyEventArgs e)
			{
				base.OnKeyUp(e);
				if (e.KeyData == Keys.Return && base.SelectedNode != null)
				{
					this.SetSelectedItem(base.SelectedNode);
				}
			}

			protected override bool IsInputKey(Keys key)
			{
				return key == Keys.Return || base.IsInputKey(key);
			}
		}

		internal class BindingPickerNode : TreeNode
		{
			public BindingPickerNode(DesignBindingPicker picker, string nodeName)
				: base(nodeName)
			{
				this.picker = picker;
			}

			public BindingPickerNode(DesignBindingPicker picker, string nodeName, DesignBindingPicker.BindingPickerNode.BindingImage index)
				: base(nodeName)
			{
				this.picker = picker;
				this.BindingImageIndex = (int)index;
			}

			public static DesignBindingPicker.BindingPickerNode.BindingImage BindingImageIndexForDataSource(object dataSource)
			{
				if (dataSource is BindingSource)
				{
					return DesignBindingPicker.BindingPickerNode.BindingImage.BindingSource;
				}
				IListSource listSource = dataSource as IListSource;
				if (listSource != null)
				{
					if (listSource.ContainsListCollection)
					{
						return DesignBindingPicker.BindingPickerNode.BindingImage.DataSource;
					}
					return DesignBindingPicker.BindingPickerNode.BindingImage.ListMember;
				}
				else
				{
					if (dataSource is IList)
					{
						return DesignBindingPicker.BindingPickerNode.BindingImage.ListMember;
					}
					return DesignBindingPicker.BindingPickerNode.BindingImage.FieldMember;
				}
			}

			public virtual void OnExpand()
			{
				this.FillSubNodes();
			}

			public virtual void FillSubNodes()
			{
				if (this.SubNodesFilled)
				{
					return;
				}
				foreach (object obj in base.Nodes)
				{
					DesignBindingPicker.BindingPickerNode bindingPickerNode = (DesignBindingPicker.BindingPickerNode)obj;
					bindingPickerNode.Fill();
				}
				this.SubNodesFilled = true;
			}

			public virtual void Fill()
			{
			}

			public virtual DesignBinding OnSelect()
			{
				return null;
			}

			public virtual bool CanSelect
			{
				get
				{
					return false;
				}
			}

			public virtual string Error
			{
				get
				{
					return this.error;
				}
				set
				{
					this.error = value;
				}
			}

			public virtual string HelpText
			{
				get
				{
					return null;
				}
			}

			public int BindingImageIndex
			{
				set
				{
					base.ImageIndex = value;
					base.SelectedImageIndex = value;
				}
			}

			public Image CustomBindingImage
			{
				set
				{
					try
					{
						ImageList.ImageCollection images = this.picker.treeViewCtrl.ImageList.Images;
						images.Add(value, Color.Transparent);
						this.BindingImageIndex = images.Count - 1;
					}
					catch (Exception)
					{
					}
					catch
					{
					}
				}
			}

			public bool SubNodesFilled
			{
				get
				{
					return this.subNodesFilled;
				}
				set
				{
					this.subNodesFilled = true;
				}
			}

			private string error;

			private bool subNodesFilled;

			protected DesignBindingPicker picker;

			public enum BindingImage
			{
				None,
				Other,
				Project,
				Instances,
				BindingSource,
				ListMember,
				FieldMember,
				DataSource
			}
		}

		internal class DataSourceNode : DesignBindingPicker.BindingPickerNode
		{
			public DataSourceNode(DesignBindingPicker picker, object dataSource, string nodeName)
				: base(picker, nodeName)
			{
				this.dataSource = dataSource;
				base.BindingImageIndex = (int)DesignBindingPicker.BindingPickerNode.BindingImageIndexForDataSource(dataSource);
			}

			public object DataSource
			{
				get
				{
					return this.dataSource;
				}
			}

			public override DesignBinding OnSelect()
			{
				return new DesignBinding(this.DataSource, "");
			}

			public override bool CanSelect
			{
				get
				{
					return !this.picker.showDataMembers;
				}
			}

			public override string HelpText
			{
				get
				{
					string text;
					if (this.DataSource is DataSourceDescriptor)
					{
						text = "Project";
					}
					else if (this.DataSource is BindingSource)
					{
						text = "BindSrc";
					}
					else
					{
						text = "FormInst";
					}
					string text2;
					if (!(this is DesignBindingPicker.DataMemberNode))
					{
						text2 = "DS";
					}
					else if ((this as DesignBindingPicker.DataMemberNode).IsList)
					{
						text2 = "LM";
					}
					else
					{
						text2 = "DM";
					}
					string text4;
					try
					{
						string text3 = string.Format(CultureInfo.CurrentCulture, "DesignBindingPickerHelpNode{0}{1}{2}", new object[]
						{
							text,
							text2,
							this.CanSelect ? "1" : "0"
						});
						text4 = SR.GetString(text3);
					}
					catch
					{
						text4 = "";
					}
					return text4;
				}
			}

			private object dataSource;
		}

		internal class DataMemberNode : DesignBindingPicker.DataSourceNode
		{
			public DataMemberNode(DesignBindingPicker picker, object dataSource, string dataMember, string dataField, bool isList)
				: base(picker, dataSource, dataField)
			{
				this.dataMember = dataMember;
				this.isList = isList;
				base.BindingImageIndex = (isList ? 5 : 6);
			}

			public string DataMember
			{
				get
				{
					return this.dataMember;
				}
			}

			public bool IsList
			{
				get
				{
					return this.isList;
				}
			}

			public override void Fill()
			{
				this.picker.AddDataMemberContents(this);
			}

			public override DesignBinding OnSelect()
			{
				if (this.picker.showDataMembers)
				{
					return new DesignBinding(base.DataSource, this.DataMember);
				}
				BindingSource bindingSource = this.picker.CreateNewBindingSource(base.DataSource, this.DataMember);
				if (bindingSource != null)
				{
					return new DesignBinding(bindingSource, "");
				}
				return null;
			}

			public override bool CanSelect
			{
				get
				{
					return this.picker.selectListMembers == this.IsList;
				}
			}

			private bool isList;

			private string dataMember;
		}

		internal class NoneNode : DesignBindingPicker.BindingPickerNode
		{
			public NoneNode()
				: base(null, SR.GetString("DesignBindingPickerNodeNone"), DesignBindingPicker.BindingPickerNode.BindingImage.None)
			{
			}

			public override DesignBinding OnSelect()
			{
				return DesignBinding.Null;
			}

			public override bool CanSelect
			{
				get
				{
					return true;
				}
			}

			public override string HelpText
			{
				get
				{
					return SR.GetString("DesignBindingPickerHelpNodeNone");
				}
			}
		}

		internal class OtherNode : DesignBindingPicker.BindingPickerNode
		{
			public OtherNode()
				: base(null, SR.GetString("DesignBindingPickerNodeOther"), DesignBindingPicker.BindingPickerNode.BindingImage.Other)
			{
			}

			public override string HelpText
			{
				get
				{
					return SR.GetString("DesignBindingPickerHelpNodeOther");
				}
			}
		}

		internal class InstancesNode : DesignBindingPicker.BindingPickerNode
		{
			public InstancesNode(string rootComponentName)
				: base(null, string.Format(CultureInfo.CurrentCulture, SR.GetString("DesignBindingPickerNodeInstances"), new object[] { rootComponentName }), DesignBindingPicker.BindingPickerNode.BindingImage.Instances)
			{
			}

			public override string HelpText
			{
				get
				{
					return SR.GetString("DesignBindingPickerHelpNodeInstances");
				}
			}
		}

		internal class ProjectNode : DesignBindingPicker.BindingPickerNode
		{
			public ProjectNode(DesignBindingPicker picker)
				: base(picker, SR.GetString("DesignBindingPickerNodeProject"), DesignBindingPicker.BindingPickerNode.BindingImage.Project)
			{
			}

			public override string HelpText
			{
				get
				{
					return SR.GetString("DesignBindingPickerHelpNodeProject");
				}
			}
		}

		internal class ProjectGroupNode : DesignBindingPicker.BindingPickerNode
		{
			public ProjectGroupNode(DesignBindingPicker picker, string nodeName, Image image)
				: base(picker, nodeName, DesignBindingPicker.BindingPickerNode.BindingImage.Project)
			{
				if (image != null)
				{
					base.CustomBindingImage = image;
				}
			}

			public override string HelpText
			{
				get
				{
					return SR.GetString("DesignBindingPickerHelpNodeProjectGroup");
				}
			}
		}

		internal class ProjectDataSourceNode : DesignBindingPicker.DataSourceNode
		{
			public ProjectDataSourceNode(DesignBindingPicker picker, object dataSource, string nodeName, Image image)
				: base(picker, dataSource, nodeName)
			{
				if (image != null)
				{
					base.CustomBindingImage = image;
				}
			}

			public override void OnExpand()
			{
			}

			public override void Fill()
			{
				this.picker.AddProjectDataSourceContents(this);
			}

			public override DesignBinding OnSelect()
			{
				DataSourceDescriptor dataSourceDescriptor = (DataSourceDescriptor)base.DataSource;
				if (this.picker.context.Instance is BindingSource)
				{
					object projectDataSourceInstance = this.picker.GetProjectDataSourceInstance(dataSourceDescriptor);
					if (projectDataSourceInstance != null)
					{
						return new DesignBinding(projectDataSourceInstance, "");
					}
					return null;
				}
				else
				{
					BindingSource bindingSource = this.picker.CreateNewBindingSource(dataSourceDescriptor, "");
					if (bindingSource != null)
					{
						return new DesignBinding(bindingSource, "");
					}
					return null;
				}
			}
		}

		internal class ProjectDataMemberNode : DesignBindingPicker.DataMemberNode
		{
			public ProjectDataMemberNode(DesignBindingPicker picker, object dataSource, string dataMember, string dataField, bool isList)
				: base(picker, dataSource, dataMember, dataField, isList)
			{
			}

			public override void OnExpand()
			{
			}

			public override DesignBinding OnSelect()
			{
				DesignBindingPicker.ProjectDataMemberNode projectDataMemberNode = base.Parent as DesignBindingPicker.ProjectDataMemberNode;
				string text;
				string text2;
				if (projectDataMemberNode != null)
				{
					text = projectDataMemberNode.DataMember;
					text2 = base.DataMember;
				}
				else if (base.IsList)
				{
					text = base.DataMember;
					text2 = "";
				}
				else
				{
					text = "";
					text2 = base.DataMember;
				}
				DataSourceDescriptor dataSourceDescriptor = (DataSourceDescriptor)base.DataSource;
				BindingSource bindingSource = this.picker.CreateNewBindingSource(dataSourceDescriptor, text);
				if (bindingSource != null)
				{
					return new DesignBinding(bindingSource, text2);
				}
				return null;
			}
		}
	}
}
