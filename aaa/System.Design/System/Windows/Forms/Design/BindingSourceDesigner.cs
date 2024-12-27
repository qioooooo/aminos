using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Data;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001A5 RID: 421
	internal class BindingSourceDesigner : ComponentDesigner
	{
		// Token: 0x170002A5 RID: 677
		// (set) Token: 0x0600102B RID: 4139 RVA: 0x000496F6 File Offset: 0x000486F6
		public bool BindingUpdatedByUser
		{
			set
			{
				this.bindingUpdatedByUser = value;
			}
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x00049700 File Offset: 0x00048700
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

		// Token: 0x0600102D RID: 4141 RVA: 0x00049754 File Offset: 0x00048754
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

		// Token: 0x0600102E RID: 4142 RVA: 0x000497A8 File Offset: 0x000487A8
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

		// Token: 0x0600102F RID: 4143 RVA: 0x00049830 File Offset: 0x00048830
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

		// Token: 0x04001043 RID: 4163
		private bool bindingUpdatedByUser;
	}
}
