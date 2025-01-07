using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public sealed class ControlDesignerState
	{
		internal ControlDesignerState(IComponent component)
		{
			this._component = component;
		}

		public object this[string key]
		{
			get
			{
				if (this._designerState == null)
				{
					if (this._component != null && this._component.Site != null)
					{
						IComponentDesignerStateService componentDesignerStateService = (IComponentDesignerStateService)this._component.Site.GetService(typeof(IComponentDesignerStateService));
						if (componentDesignerStateService != null)
						{
							return componentDesignerStateService.GetState(this._component, key);
						}
					}
					this._designerState = new Hashtable();
				}
				return this._designerState[key];
			}
			set
			{
				if (this._designerState == null)
				{
					if (this._component != null && this._component.Site != null)
					{
						IComponentDesignerStateService componentDesignerStateService = (IComponentDesignerStateService)this._component.Site.GetService(typeof(IComponentDesignerStateService));
						if (componentDesignerStateService != null)
						{
							componentDesignerStateService.SetState(this._component, key, value);
							return;
						}
					}
					this._designerState = new Hashtable();
				}
				this._designerState[key] = value;
			}
		}

		private IDictionary _designerState;

		private IComponent _component;
	}
}
