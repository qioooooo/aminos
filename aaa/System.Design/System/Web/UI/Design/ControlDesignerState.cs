using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x02000333 RID: 819
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public sealed class ControlDesignerState
	{
		// Token: 0x06001EFD RID: 7933 RVA: 0x000AEF34 File Offset: 0x000ADF34
		internal ControlDesignerState(IComponent component)
		{
			this._component = component;
		}

		// Token: 0x17000576 RID: 1398
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

		// Token: 0x04001760 RID: 5984
		private IDictionary _designerState;

		// Token: 0x04001761 RID: 5985
		private IComponent _component;
	}
}
