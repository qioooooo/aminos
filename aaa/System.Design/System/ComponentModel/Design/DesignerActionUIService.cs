using System;

namespace System.ComponentModel.Design
{
	// Token: 0x02000126 RID: 294
	public sealed class DesignerActionUIService : IDisposable
	{
		// Token: 0x06000BAD RID: 2989 RVA: 0x0002DE20 File Offset: 0x0002CE20
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

		// Token: 0x06000BAE RID: 2990 RVA: 0x0002DE88 File Offset: 0x0002CE88
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

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000BAF RID: 2991 RVA: 0x0002DECB File Offset: 0x0002CECB
		// (remove) Token: 0x06000BB0 RID: 2992 RVA: 0x0002DEE4 File Offset: 0x0002CEE4
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

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0002DEFD File Offset: 0x0002CEFD
		public void HideUI(IComponent component)
		{
			this.OnDesignerActionUIStateChange(new DesignerActionUIStateChangeEventArgs(component, DesignerActionUIStateChangeType.Hide));
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0002DF0C File Offset: 0x0002CF0C
		public void ShowUI(IComponent component)
		{
			this.OnDesignerActionUIStateChange(new DesignerActionUIStateChangeEventArgs(component, DesignerActionUIStateChangeType.Show));
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0002DF1B File Offset: 0x0002CF1B
		public void Refresh(IComponent component)
		{
			this.OnDesignerActionUIStateChange(new DesignerActionUIStateChangeEventArgs(component, DesignerActionUIStateChangeType.Refresh));
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0002DF2A File Offset: 0x0002CF2A
		private void OnDesignerActionUIStateChange(DesignerActionUIStateChangeEventArgs e)
		{
			if (this.designerActionUIStateChangedEventHandler != null)
			{
				this.designerActionUIStateChangedEventHandler(this, e);
			}
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0002DF44 File Offset: 0x0002CF44
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

		// Token: 0x04000E51 RID: 3665
		private DesignerActionUIStateChangeEventHandler designerActionUIStateChangedEventHandler;

		// Token: 0x04000E52 RID: 3666
		private IServiceProvider serviceProvider;

		// Token: 0x04000E53 RID: 3667
		private DesignerActionService designerActionService;
	}
}
