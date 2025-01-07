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
	public abstract class BaseDataBoundControlDesigner : ControlDesigner
	{
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

		protected abstract bool ConnectToDataSource();

		protected abstract void CreateDataSource();

		protected abstract void DataBind(BaseDataBoundControl dataBoundControl);

		protected abstract void DisconnectFromDataSource();

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

		protected override string GetEmptyDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml(null);
		}

		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Control_ErrorRenderingShort") + "<br/>" + e.Message);
		}

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

		private void OnAnyComponentChanged(object sender, ComponentChangedEventArgs ce)
		{
			Control control = ce.Component as Control;
			if (control != null && ce.Member != null && ce.Member.Name == "ID" && base.Component != null && ((string)ce.OldValue == this.DataSourceID || (string)ce.NewValue == this.DataSourceID))
			{
				this.OnDataSourceChanged(false);
			}
		}

		private void OnComponentAdded(object sender, ComponentEventArgs e)
		{
			Control control = e.Component as Control;
			if (control != null && control.ID == this.DataSourceID)
			{
				this.OnDataSourceChanged(false);
			}
		}

		private void OnComponentRemoving(object sender, ComponentEventArgs e)
		{
			Control control = e.Component as Control;
			if (control != null && base.Component != null && control.ID == this.DataSourceID)
			{
				this.DisconnectFromDataSource();
			}
		}

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

		protected virtual void OnDataSourceChanged(bool forceUpdateView)
		{
			if (this.ConnectToDataSource() || forceUpdateView)
			{
				this.UpdateDesignTimeHtml();
			}
		}

		private void OnDesignerLoadComplete(object sender, EventArgs e)
		{
			this.OnDataSourceChanged(false);
		}

		protected virtual void OnSchemaRefreshed()
		{
		}

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

		public static DialogResult ShowCreateDataSourceDialog(ControlDesigner controlDesigner, Type dataSourceType, bool configure, out string dataSourceID)
		{
			CreateDataSourceDialog createDataSourceDialog = new CreateDataSourceDialog(controlDesigner, dataSourceType, configure);
			DialogResult dialogResult = UIServiceHelper.ShowDialog(controlDesigner.Component.Site, createDataSourceDialog);
			dataSourceID = createDataSourceDialog.DataSourceID;
			return dialogResult;
		}

		private bool _keepDataSourceBrowsable;
	}
}
