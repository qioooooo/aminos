using System;

namespace System.ComponentModel.Design
{
	public sealed class DesignerActionUIService : IDisposable
	{
		internal DesignerActionUIService(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
			if (serviceProvider != null)
			{
				this.serviceProvider = serviceProvider;
				IDesignerHost designerHost = (IDesignerHost)serviceProvider.GetService(typeof(IDesignerHost));
				designerHost.AddService(typeof(DesignerActionUIService), this);
				this.designerActionService = serviceProvider.GetService(typeof(DesignerActionService)) as DesignerActionService;
			}
		}

		public void Dispose()
		{
			if (this.serviceProvider != null)
			{
				IDesignerHost designerHost = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					designerHost.RemoveService(typeof(DesignerActionUIService));
				}
			}
		}

		public event DesignerActionUIStateChangeEventHandler DesignerActionUIStateChange
		{
			add
			{
				this.designerActionUIStateChangedEventHandler = (DesignerActionUIStateChangeEventHandler)Delegate.Combine(this.designerActionUIStateChangedEventHandler, value);
			}
			remove
			{
				this.designerActionUIStateChangedEventHandler = (DesignerActionUIStateChangeEventHandler)Delegate.Remove(this.designerActionUIStateChangedEventHandler, value);
			}
		}

		public void HideUI(IComponent component)
		{
			this.OnDesignerActionUIStateChange(new DesignerActionUIStateChangeEventArgs(component, DesignerActionUIStateChangeType.Hide));
		}

		public void ShowUI(IComponent component)
		{
			this.OnDesignerActionUIStateChange(new DesignerActionUIStateChangeEventArgs(component, DesignerActionUIStateChangeType.Show));
		}

		public void Refresh(IComponent component)
		{
			this.OnDesignerActionUIStateChange(new DesignerActionUIStateChangeEventArgs(component, DesignerActionUIStateChangeType.Refresh));
		}

		private void OnDesignerActionUIStateChange(DesignerActionUIStateChangeEventArgs e)
		{
			if (this.designerActionUIStateChangedEventHandler != null)
			{
				this.designerActionUIStateChangedEventHandler(this, e);
			}
		}

		public bool ShouldAutoShow(IComponent component)
		{
			if (this.serviceProvider != null)
			{
				DesignerOptionService designerOptionService = this.serviceProvider.GetService(typeof(DesignerOptionService)) as DesignerOptionService;
				if (designerOptionService != null)
				{
					PropertyDescriptor propertyDescriptor = designerOptionService.Options.Properties["ObjectBoundSmartTagAutoShow"];
					if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(bool) && !(bool)propertyDescriptor.GetValue(null))
					{
						return false;
					}
				}
			}
			if (this.designerActionService != null)
			{
				DesignerActionListCollection componentActions = this.designerActionService.GetComponentActions(component);
				if (componentActions != null && componentActions.Count > 0)
				{
					for (int i = 0; i < componentActions.Count; i++)
					{
						if (componentActions[i].AutoShow)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private DesignerActionUIStateChangeEventHandler designerActionUIStateChangedEventHandler;

		private IServiceProvider serviceProvider;

		private DesignerActionService designerActionService;
	}
}
