using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Web.UI.Design.WebControls
{
	internal sealed class TypeDescriptorContext : ITypeDescriptorContext, IServiceProvider
	{
		public TypeDescriptorContext(IDesignerHost designerHost, PropertyDescriptor propDesc, object instance)
		{
			this._designerHost = designerHost;
			this._propDesc = propDesc;
			this._instance = instance;
		}

		private IComponentChangeService ComponentChangeService
		{
			get
			{
				return (IComponentChangeService)this._designerHost.GetService(typeof(IComponentChangeService));
			}
		}

		public IContainer Container
		{
			get
			{
				return (IContainer)this._designerHost.GetService(typeof(IContainer));
			}
		}

		public object Instance
		{
			get
			{
				return this._instance;
			}
		}

		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this._propDesc;
			}
		}

		public object GetService(Type serviceType)
		{
			return this._designerHost.GetService(serviceType);
		}

		public bool OnComponentChanging()
		{
			if (this.ComponentChangeService != null)
			{
				try
				{
					this.ComponentChangeService.OnComponentChanging(this._instance, this._propDesc);
				}
				catch (CheckoutException ex)
				{
					if (ex == CheckoutException.Canceled)
					{
						return false;
					}
					throw ex;
				}
				return true;
			}
			return true;
		}

		public void OnComponentChanged()
		{
			if (this.ComponentChangeService != null)
			{
				this.ComponentChangeService.OnComponentChanged(this._instance, this._propDesc, null, null);
			}
		}

		private IDesignerHost _designerHost;

		private PropertyDescriptor _propDesc;

		private object _instance;
	}
}
