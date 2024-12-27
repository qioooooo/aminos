using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003EF RID: 1007
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public abstract class BaseDataListDesigner : TemplatedControlDesigner, IDataBindingSchemaProvider, IDataSourceProvider
	{
		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002519 RID: 9497 RVA: 0x000C7544 File Offset: 0x000C6544
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new BaseDataListActionList(this, this.DataSourceDesigner));
				return designerActionListCollection;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x0600251A RID: 9498 RVA: 0x000C7577 File Offset: 0x000C6577
		// (set) Token: 0x0600251B RID: 9499 RVA: 0x000C7584 File Offset: 0x000C6584
		public string DataKeyField
		{
			get
			{
				return this.bdl.DataKeyField;
			}
			set
			{
				this.bdl.DataKeyField = value;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x0600251C RID: 9500 RVA: 0x000C7592 File Offset: 0x000C6592
		// (set) Token: 0x0600251D RID: 9501 RVA: 0x000C759F File Offset: 0x000C659F
		public string DataMember
		{
			get
			{
				return this.bdl.DataMember;
			}
			set
			{
				this.bdl.DataMember = value;
				this.OnDataSourceChanged();
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x0600251E RID: 9502 RVA: 0x000C75B4 File Offset: 0x000C65B4
		// (set) Token: 0x0600251F RID: 9503 RVA: 0x000C75E4 File Offset: 0x000C65E4
		public string DataSource
		{
			get
			{
				DataBinding dataBinding = base.DataBindings["DataSource"];
				if (dataBinding != null)
				{
					return dataBinding.Expression;
				}
				return string.Empty;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					base.DataBindings.Remove("DataSource");
				}
				else
				{
					DataBinding dataBinding = base.DataBindings["DataSource"];
					if (dataBinding == null)
					{
						dataBinding = new DataBinding("DataSource", typeof(IEnumerable), value);
					}
					else
					{
						dataBinding.Expression = value;
					}
					base.DataBindings.Add(dataBinding);
				}
				this.OnDataSourceChanged();
				base.OnBindingsCollectionChangedInternal("DataSource");
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002520 RID: 9504 RVA: 0x000C765E File Offset: 0x000C665E
		public IDataSourceDesigner DataSourceDesigner
		{
			get
			{
				return this._dataSourceDesigner;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002521 RID: 9505 RVA: 0x000C7666 File Offset: 0x000C6666
		// (set) Token: 0x06002522 RID: 9506 RVA: 0x000C7674 File Offset: 0x000C6674
		public string DataSourceID
		{
			get
			{
				return this.bdl.DataSourceID;
			}
			set
			{
				if (value == this.DataSourceID)
				{
					return;
				}
				if (value == SR.GetString("DataSourceIDChromeConverter_NewDataSource"))
				{
					this.CreateDataSource();
					return;
				}
				if (value == SR.GetString("DataSourceIDChromeConverter_NoDataSource"))
				{
					value = string.Empty;
				}
				this.bdl.DataSourceID = value;
				this.OnDataSourceChanged();
				this.OnSchemaRefreshed();
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002523 RID: 9507 RVA: 0x000C76DC File Offset: 0x000C66DC
		public DesignerDataSourceView DesignerView
		{
			get
			{
				DesignerDataSourceView designerDataSourceView = null;
				if (this.DataSourceDesigner != null)
				{
					designerDataSourceView = this.DataSourceDesigner.GetView(this.DataMember);
					if (designerDataSourceView == null && string.IsNullOrEmpty(this.DataMember))
					{
						string[] viewNames = this.DataSourceDesigner.GetViewNames();
						if (viewNames != null && viewNames.Length > 0)
						{
							designerDataSourceView = this.DataSourceDesigner.GetView(viewNames[0]);
						}
					}
				}
				return designerDataSourceView;
			}
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x000C773C File Offset: 0x000C673C
		private bool ConnectToDataSource()
		{
			IDataSourceDesigner dataSourceDesigner = this.GetDataSourceDesigner();
			if (this._dataSourceDesigner != dataSourceDesigner)
			{
				if (this._dataSourceDesigner != null)
				{
					this._dataSourceDesigner.DataSourceChanged -= this.DataSourceChanged;
					this._dataSourceDesigner.SchemaRefreshed -= this.SchemaRefreshed;
				}
				this._dataSourceDesigner = dataSourceDesigner;
				if (this._dataSourceDesigner != null)
				{
					this._dataSourceDesigner.DataSourceChanged += this.DataSourceChanged;
					this._dataSourceDesigner.SchemaRefreshed += this.SchemaRefreshed;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x000C77CF File Offset: 0x000C67CF
		private void CreateDataSource()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.CreateDataSourceCallback), null, SR.GetString("BaseDataBoundControl_CreateDataSourceTransaction"));
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x000C77F4 File Offset: 0x000C67F4
		private bool CreateDataSourceCallback(object context)
		{
			CreateDataSourceDialog createDataSourceDialog = new CreateDataSourceDialog(this, typeof(IDataSource), true);
			DialogResult dialogResult = UIServiceHelper.ShowDialog(base.Component.Site, createDataSourceDialog);
			string dataSourceID = createDataSourceDialog.DataSourceID;
			if (dataSourceID.Length > 0)
			{
				this.DataSourceID = dataSourceID;
			}
			return dialogResult == DialogResult.OK;
		}

		// Token: 0x06002527 RID: 9511 RVA: 0x000C7840 File Offset: 0x000C6840
		private void DataSourceChanged(object sender, EventArgs e)
		{
			this.designTimeDataTable = null;
			this.UpdateDesignTimeHtml();
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x000C7850 File Offset: 0x000C6850
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (base.Component != null && base.Component.Site != null)
				{
					if (base.RootDesigner != null)
					{
						base.RootDesigner.LoadComplete -= this.OnDesignerLoadComplete;
					}
					IComponentChangeService componentChangeService = (IComponentChangeService)base.Component.Site.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						componentChangeService.ComponentAdded -= this.OnComponentAdded;
						componentChangeService.ComponentRemoving -= this.OnComponentRemoving;
						componentChangeService.ComponentRemoved -= this.OnComponentRemoved;
						componentChangeService.ComponentChanged -= this.OnAnyComponentChanged;
					}
				}
				this.bdl = null;
				if (this._dataSourceDesigner != null)
				{
					this._dataSourceDesigner.DataSourceChanged -= this.DataSourceChanged;
					this._dataSourceDesigner.SchemaRefreshed -= this.SchemaRefreshed;
					this._dataSourceDesigner = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x000C7954 File Offset: 0x000C6954
		private IDataSourceDesigner GetDataSourceDesigner()
		{
			IDataSourceDesigner dataSourceDesigner = null;
			string dataSourceID = this.DataSourceID;
			if (!string.IsNullOrEmpty(dataSourceID))
			{
				Control control = ControlHelper.FindControl(base.Component.Site, (Control)base.Component, dataSourceID);
				if (control != null && control.Site != null)
				{
					IDesignerHost designerHost = (IDesignerHost)control.Site.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						dataSourceDesigner = designerHost.GetDesigner(control) as IDataSourceDesigner;
					}
				}
			}
			return dataSourceDesigner;
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x000C79C8 File Offset: 0x000C69C8
		protected IEnumerable GetDesignTimeDataSource(int minimumRows, out bool dummyDataSource)
		{
			IEnumerable resolvedSelectedDataSource = this.GetResolvedSelectedDataSource();
			return this.GetDesignTimeDataSource(resolvedSelectedDataSource, minimumRows, out dummyDataSource);
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x000C79E8 File Offset: 0x000C69E8
		protected IEnumerable GetDesignTimeDataSource(IEnumerable selectedDataSource, int minimumRows, out bool dummyDataSource)
		{
			DataTable dataTable = this.designTimeDataTable;
			dummyDataSource = false;
			if (dataTable == null)
			{
				if (selectedDataSource != null)
				{
					this.designTimeDataTable = DesignTimeData.CreateSampleDataTable(selectedDataSource);
					dataTable = this.designTimeDataTable;
				}
				if (dataTable == null)
				{
					if (this.dummyDataTable == null)
					{
						this.dummyDataTable = DesignTimeData.CreateDummyDataTable();
					}
					dataTable = this.dummyDataTable;
					dummyDataSource = true;
				}
			}
			return DesignTimeData.GetDesignTimeDataSource(dataTable, minimumRows);
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x000C7A44 File Offset: 0x000C6A44
		public IEnumerable GetResolvedSelectedDataSource()
		{
			IEnumerable enumerable = null;
			DataBinding dataBinding = base.DataBindings["DataSource"];
			if (dataBinding != null)
			{
				enumerable = DesignTimeData.GetSelectedDataSource(this.bdl, dataBinding.Expression, this.DataMember);
			}
			return enumerable;
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x000C7A80 File Offset: 0x000C6A80
		public object GetSelectedDataSource()
		{
			object obj = null;
			DataBinding dataBinding = base.DataBindings["DataSource"];
			if (dataBinding != null)
			{
				obj = DesignTimeData.GetSelectedDataSource(this.bdl, dataBinding.Expression);
			}
			return obj;
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x000C7AB6 File Offset: 0x000C6AB6
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public override IEnumerable GetTemplateContainerDataSource(string templateName)
		{
			return this.GetResolvedSelectedDataSource();
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x000C7AC0 File Offset: 0x000C6AC0
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(BaseDataList));
			this.bdl = (BaseDataList)component;
			base.Initialize(component);
			base.SetViewFlags(ViewFlags.DesignTimeHtmlRequiresLoadComplete, true);
			if (base.RootDesigner != null)
			{
				if (base.RootDesigner.IsLoading)
				{
					base.RootDesigner.LoadComplete += this.OnDesignerLoadComplete;
				}
				else
				{
					this.OnDesignerLoadComplete(null, EventArgs.Empty);
				}
			}
			IComponentChangeService componentChangeService = (IComponentChangeService)component.Site.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentAdded += this.OnComponentAdded;
				componentChangeService.ComponentRemoving += this.OnComponentRemoving;
				componentChangeService.ComponentRemoved += this.OnComponentRemoved;
				componentChangeService.ComponentChanged += this.OnAnyComponentChanged;
			}
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x000C7B98 File Offset: 0x000C6B98
		protected internal void InvokePropertyBuilder(int initialPage)
		{
			ComponentEditor componentEditor;
			if (this.bdl is global::System.Web.UI.WebControls.DataGrid)
			{
				componentEditor = new DataGridComponentEditor(initialPage);
			}
			else
			{
				componentEditor = new DataListComponentEditor(initialPage);
			}
			componentEditor.EditComponent(this.bdl);
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x000C7BD0 File Offset: 0x000C6BD0
		private void OnAnyComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (e.Member != null)
			{
				object component = e.Component;
				IDataSource dataSource = component as IDataSource;
				if (dataSource != null && dataSource is Control && e.Member.Name == "ID" && base.Component != null && ((string)e.OldValue == this.DataSourceID || (string)e.NewValue == this.DataSourceID))
				{
					this.ConnectToDataSource();
					this.UpdateDesignTimeHtml();
				}
			}
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x000C7C5A File Offset: 0x000C6C5A
		[Obsolete("Use of this method is not recommended because the AutoFormat dialog is launched by the designer host. The list of available AutoFormats is exposed on the ControlDesigner in the AutoFormats property. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected void OnAutoFormat(object sender, EventArgs e)
		{
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x000C7C5C File Offset: 0x000C6C5C
		public override void OnAutoFormatApplied(DesignerAutoFormat appliedAutoFormat)
		{
			this.OnStylesChanged();
			base.OnAutoFormatApplied(appliedAutoFormat);
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x000C7C6C File Offset: 0x000C6C6C
		private void OnComponentAdded(object sender, ComponentEventArgs e)
		{
			IComponent component = e.Component;
			IDataSource dataSource = component as IDataSource;
			if (dataSource != null && component is Control && ((Control)dataSource).ID == this.DataSourceID)
			{
				this.ConnectToDataSource();
				this.UpdateDesignTimeHtml();
			}
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x000C7CB8 File Offset: 0x000C6CB8
		public override void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (e.Member != null)
			{
				string name = e.Member.Name;
				if (name.Equals("DataSource") || name.Equals("DataMember"))
				{
					this.OnDataSourceChanged();
				}
				else if (name.Equals("ItemStyle") || name.Equals("AlternatingItemStyle") || name.Equals("SelectedItemStyle") || name.Equals("EditItemStyle") || name.Equals("HeaderStyle") || name.Equals("FooterStyle") || name.Equals("SeparatorStyle") || name.Equals("Font") || name.Equals("ForeColor") || name.Equals("BackColor"))
				{
					this.OnStylesChanged();
				}
			}
			base.OnComponentChanged(sender, e);
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x000C7D94 File Offset: 0x000C6D94
		private void OnComponentRemoving(object sender, ComponentEventArgs e)
		{
			IComponent component = e.Component;
			IDataSource dataSource = component as IDataSource;
			if (dataSource != null && dataSource is Control && base.Component != null && ((Control)dataSource).ID == this.DataSourceID && this._dataSourceDesigner != null)
			{
				this._dataSourceDesigner.DataSourceChanged -= this.DataSourceChanged;
				this._dataSourceDesigner.SchemaRefreshed -= this.SchemaRefreshed;
				this._dataSourceDesigner = null;
			}
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x000C7E18 File Offset: 0x000C6E18
		private void OnComponentRemoved(object sender, ComponentEventArgs e)
		{
			IComponent component = e.Component;
			IDataSource dataSource = component as IDataSource;
			if (dataSource != null && dataSource is Control && base.Component != null && ((Control)dataSource).ID == this.DataSourceID)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null && !designerHost.Loading)
				{
					this.ConnectToDataSource();
					this.UpdateDesignTimeHtml();
				}
			}
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x000C7E8C File Offset: 0x000C6E8C
		protected internal virtual void OnDataSourceChanged()
		{
			this.ConnectToDataSource();
			this.designTimeDataTable = null;
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x000C7E9C File Offset: 0x000C6E9C
		private void OnDesignerLoadComplete(object sender, EventArgs e)
		{
			this.ConnectToDataSource();
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000C7EA5 File Offset: 0x000C6EA5
		protected void OnPropertyBuilder(object sender, EventArgs e)
		{
			this.InvokePropertyBuilder(0);
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x000C7EAE File Offset: 0x000C6EAE
		protected virtual void OnSchemaRefreshed()
		{
			this.UpdateDesignTimeHtml();
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x000C7EB6 File Offset: 0x000C6EB6
		protected internal void OnStylesChanged()
		{
			this.OnTemplateEditingVerbsChanged();
		}

		// Token: 0x0600253D RID: 9533
		protected abstract void OnTemplateEditingVerbsChanged();

		// Token: 0x0600253E RID: 9534 RVA: 0x000C7EC0 File Offset: 0x000C6EC0
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["DataSource"];
			AttributeCollection attributes = propertyDescriptor.Attributes;
			int num = -1;
			int count = attributes.Count;
			string dataSource = this.DataSource;
			if (dataSource.Length > 0)
			{
				this._keepDataSourceBrowsable = true;
			}
			for (int i = 0; i < attributes.Count; i++)
			{
				if (attributes[i] is BrowsableAttribute)
				{
					num = i;
					break;
				}
			}
			int num2;
			if (num == -1 && dataSource.Length == 0 && !this._keepDataSourceBrowsable)
			{
				num2 = count + 2;
			}
			else
			{
				num2 = count + 1;
			}
			Attribute[] array = new Attribute[num2];
			attributes.CopyTo(array, 0);
			array[count] = new TypeConverterAttribute(typeof(DataSourceConverter));
			if (dataSource.Length == 0 && !this._keepDataSourceBrowsable)
			{
				if (num == -1)
				{
					array[count + 1] = BrowsableAttribute.No;
				}
				else
				{
					array[num] = BrowsableAttribute.No;
				}
			}
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), "DataSource", typeof(string), array);
			properties["DataSource"] = propertyDescriptor;
			propertyDescriptor = (PropertyDescriptor)properties["DataMember"];
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[]
			{
				new TypeConverterAttribute(typeof(DataMemberConverter))
			});
			properties["DataMember"] = propertyDescriptor;
			propertyDescriptor = (PropertyDescriptor)properties["DataKeyField"];
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[]
			{
				new TypeConverterAttribute(typeof(DataFieldConverter))
			});
			properties["DataKeyField"] = propertyDescriptor;
			propertyDescriptor = (PropertyDescriptor)properties["DataSourceID"];
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[]
			{
				new TypeConverterAttribute(typeof(DataSourceIDConverter))
			});
			properties["DataSourceID"] = propertyDescriptor;
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x000C80A8 File Offset: 0x000C70A8
		private void SchemaRefreshed(object sender, EventArgs e)
		{
			this.OnSchemaRefreshed();
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002540 RID: 9536 RVA: 0x000C80B0 File Offset: 0x000C70B0
		bool IDataBindingSchemaProvider.CanRefreshSchema
		{
			get
			{
				IDataSourceDesigner dataSourceDesigner = this.DataSourceDesigner;
				return dataSourceDesigner != null && dataSourceDesigner.CanRefreshSchema;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002541 RID: 9537 RVA: 0x000C80D0 File Offset: 0x000C70D0
		IDataSourceViewSchema IDataBindingSchemaProvider.Schema
		{
			get
			{
				DesignerDataSourceView designerView = this.DesignerView;
				if (designerView != null)
				{
					return designerView.Schema;
				}
				return null;
			}
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x000C80F0 File Offset: 0x000C70F0
		void IDataBindingSchemaProvider.RefreshSchema(bool preferSilent)
		{
			IDataSourceDesigner dataSourceDesigner = this.DataSourceDesigner;
			if (dataSourceDesigner != null)
			{
				dataSourceDesigner.RefreshSchema(preferSilent);
			}
		}

		// Token: 0x04001979 RID: 6521
		private BaseDataList bdl;

		// Token: 0x0400197A RID: 6522
		private DataTable dummyDataTable;

		// Token: 0x0400197B RID: 6523
		private DataTable designTimeDataTable;

		// Token: 0x0400197C RID: 6524
		private IDataSourceDesigner _dataSourceDesigner;

		// Token: 0x0400197D RID: 6525
		private bool _keepDataSourceBrowsable;
	}
}
