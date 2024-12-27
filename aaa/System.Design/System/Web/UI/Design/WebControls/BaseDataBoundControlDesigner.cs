using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003E6 RID: 998
	public abstract class BaseDataBoundControlDesigner : ControlDesigner
	{
		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x060024D4 RID: 9428 RVA: 0x000C61B4 File Offset: 0x000C51B4
		// (set) Token: 0x060024D5 RID: 9429 RVA: 0x000C61E4 File Offset: 0x000C51E4
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
				this.OnDataSourceChanged(true);
				base.OnBindingsCollectionChangedInternal("DataSource");
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x060024D6 RID: 9430 RVA: 0x000C625F File Offset: 0x000C525F
		// (set) Token: 0x060024D7 RID: 9431 RVA: 0x000C6274 File Offset: 0x000C5274
		public string DataSourceID
		{
			get
			{
				return ((BaseDataBoundControl)base.Component).DataSourceID;
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
					TypeDescriptor.Refresh(base.Component);
					return;
				}
				if (value == SR.GetString("DataSourceIDChromeConverter_NoDataSource"))
				{
					value = string.Empty;
				}
				TypeDescriptor.Refresh(base.Component);
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(BaseDataBoundControl))["DataSourceID"];
				propertyDescriptor.SetValue(base.Component, value);
				this.OnDataSourceChanged(false);
				this.OnSchemaRefreshed();
			}
		}

		// Token: 0x060024D8 RID: 9432
		protected abstract bool ConnectToDataSource();

		// Token: 0x060024D9 RID: 9433
		protected abstract void CreateDataSource();

		// Token: 0x060024DA RID: 9434
		protected abstract void DataBind(BaseDataBoundControl dataBoundControl);

		// Token: 0x060024DB RID: 9435
		protected abstract void DisconnectFromDataSource();

		// Token: 0x060024DC RID: 9436 RVA: 0x000C630C File Offset: 0x000C530C
		protected override void Dispose(bool disposing)
		{
			if (disposing && base.Component != null && base.Component.Site != null)
			{
				this.DisconnectFromDataSource();
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
			base.Dispose(disposing);
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x000C63D4 File Offset: 0x000C53D4
		public override string GetDesignTimeHtml()
		{
			string text = string.Empty;
			try
			{
				this.DataBind((BaseDataBoundControl)base.ViewControl);
				text = base.GetDesignTimeHtml();
			}
			catch (Exception ex)
			{
				text = this.GetErrorDesignTimeHtml(ex);
			}
			return text;
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x000C6420 File Offset: 0x000C5420
		protected override string GetEmptyDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml(null);
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x000C6429 File Offset: 0x000C5429
		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Control_ErrorRenderingShort") + "<br/>" + e.Message);
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x000C644C File Offset: 0x000C544C
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(BaseDataBoundControl));
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

		// Token: 0x060024E1 RID: 9441 RVA: 0x000C6518 File Offset: 0x000C5518
		private void OnAnyComponentChanged(object sender, ComponentChangedEventArgs ce)
		{
			Control control = ce.Component as Control;
			if (control != null && ce.Member != null && ce.Member.Name == "ID" && base.Component != null && ((string)ce.OldValue == this.DataSourceID || (string)ce.NewValue == this.DataSourceID))
			{
				this.OnDataSourceChanged(false);
			}
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x000C6594 File Offset: 0x000C5594
		private void OnComponentAdded(object sender, ComponentEventArgs e)
		{
			Control control = e.Component as Control;
			if (control != null && control.ID == this.DataSourceID)
			{
				this.OnDataSourceChanged(false);
			}
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x000C65CC File Offset: 0x000C55CC
		private void OnComponentRemoving(object sender, ComponentEventArgs e)
		{
			Control control = e.Component as Control;
			if (control != null && base.Component != null && control.ID == this.DataSourceID)
			{
				this.DisconnectFromDataSource();
			}
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x000C660C File Offset: 0x000C560C
		private void OnComponentRemoved(object sender, ComponentEventArgs e)
		{
			Control control = e.Component as Control;
			if (control != null && base.Component != null && control.ID == this.DataSourceID)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null && !designerHost.Loading)
				{
					this.OnDataSourceChanged(false);
				}
			}
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x000C666C File Offset: 0x000C566C
		protected virtual void OnDataSourceChanged(bool forceUpdateView)
		{
			if (this.ConnectToDataSource() || forceUpdateView)
			{
				this.UpdateDesignTimeHtml();
			}
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x000C668C File Offset: 0x000C568C
		private void OnDesignerLoadComplete(object sender, EventArgs e)
		{
			this.OnDataSourceChanged(false);
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x000C6695 File Offset: 0x000C5695
		protected virtual void OnSchemaRefreshed()
		{
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x000C6698 File Offset: 0x000C5698
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["DataSource"];
			AttributeCollection attributes = propertyDescriptor.Attributes;
			int num = -1;
			int count = attributes.Count;
			string dataSource = this.DataSource;
			bool flag = dataSource != null && dataSource.Length > 0;
			if (flag)
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
			if (num == -1 && !flag && !this._keepDataSourceBrowsable)
			{
				num2 = count + 1;
			}
			else
			{
				num2 = count;
			}
			Attribute[] array = new Attribute[num2];
			attributes.CopyTo(array, 0);
			if (!flag && !this._keepDataSourceBrowsable)
			{
				if (num == -1)
				{
					array[count] = BrowsableAttribute.No;
				}
				else
				{
					array[num] = BrowsableAttribute.No;
				}
			}
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), "DataSource", typeof(string), array);
			properties["DataSource"] = propertyDescriptor;
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000C6798 File Offset: 0x000C5798
		public static DialogResult ShowCreateDataSourceDialog(ControlDesigner controlDesigner, Type dataSourceType, bool configure, out string dataSourceID)
		{
			CreateDataSourceDialog createDataSourceDialog = new CreateDataSourceDialog(controlDesigner, dataSourceType, configure);
			DialogResult dialogResult = UIServiceHelper.ShowDialog(controlDesigner.Component.Site, createDataSourceDialog);
			dataSourceID = createDataSourceDialog.DataSourceID;
			return dialogResult;
		}

		// Token: 0x0400195B RID: 6491
		private bool _keepDataSourceBrowsable;
	}
}
