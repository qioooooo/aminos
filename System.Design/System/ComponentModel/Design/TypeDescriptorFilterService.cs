using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	internal sealed class TypeDescriptorFilterService : ITypeDescriptorFilterService
	{
		internal TypeDescriptorFilterService()
		{
		}

		private IDesigner GetDesigner(IComponent component)
		{
			ISite site = component.Site;
			if (site != null)
			{
				IDesignerHost designerHost = site.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (designerHost != null)
				{
					return designerHost.GetDesigner(component);
				}
			}
			return null;
		}

		bool ITypeDescriptorFilterService.FilterAttributes(IComponent component, IDictionary attributes)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (attributes == null)
			{
				throw new ArgumentNullException("attributes");
			}
			IDesigner designer = this.GetDesigner(component);
			if (designer is IDesignerFilter)
			{
				((IDesignerFilter)designer).PreFilterAttributes(attributes);
				((IDesignerFilter)designer).PostFilterAttributes(attributes);
			}
			return designer != null;
		}

		bool ITypeDescriptorFilterService.FilterEvents(IComponent component, IDictionary events)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (events == null)
			{
				throw new ArgumentNullException("events");
			}
			IDesigner designer = this.GetDesigner(component);
			if (designer is IDesignerFilter)
			{
				((IDesignerFilter)designer).PreFilterEvents(events);
				((IDesignerFilter)designer).PostFilterEvents(events);
			}
			return designer != null;
		}

		bool ITypeDescriptorFilterService.FilterProperties(IComponent component, IDictionary properties)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}
			IDesigner designer = this.GetDesigner(component);
			if (designer is IDesignerFilter)
			{
				((IDesignerFilter)designer).PreFilterProperties(properties);
				((IDesignerFilter)designer).PostFilterProperties(properties);
			}
			return designer != null;
		}
	}
}
