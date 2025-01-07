using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Data;

namespace System.Windows.Forms.Design
{
	internal class BindingSourceDesigner : ComponentDesigner
	{
		public bool BindingUpdatedByUser
		{
			set
			{
				this.bindingUpdatedByUser = value;
			}
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentChanged += this.OnComponentChanged;
				componentChangeService.ComponentRemoving += this.OnComponentRemoving;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentChanged -= this.OnComponentChanged;
					componentChangeService.ComponentRemoving -= this.OnComponentRemoving;
				}
			}
			base.Dispose(disposing);
		}

		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (this.bindingUpdatedByUser && e.Component == base.Component && e.Member != null && (e.Member.Name == "DataSource" || e.Member.Name == "DataMember"))
			{
				this.bindingUpdatedByUser = false;
				DataSourceProviderService dataSourceProviderService = (DataSourceProviderService)this.GetService(typeof(DataSourceProviderService));
				if (dataSourceProviderService != null)
				{
					dataSourceProviderService.NotifyDataSourceComponentAdded(base.Component);
				}
			}
		}

		private void OnComponentRemoving(object sender, ComponentEventArgs e)
		{
			BindingSource bindingSource = base.Component as BindingSource;
			if (bindingSource != null && bindingSource.DataSource == e.Component)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				string dataMember = bindingSource.DataMember;
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(bindingSource);
				PropertyDescriptor propertyDescriptor = ((properties != null) ? properties["DataMember"] : null);
				if (componentChangeService != null && propertyDescriptor != null)
				{
					componentChangeService.OnComponentChanging(bindingSource, propertyDescriptor);
				}
				bindingSource.DataSource = null;
				if (componentChangeService != null && propertyDescriptor != null)
				{
					componentChangeService.OnComponentChanged(bindingSource, propertyDescriptor, dataMember, "");
				}
			}
		}

		private bool bindingUpdatedByUser;
	}
}
