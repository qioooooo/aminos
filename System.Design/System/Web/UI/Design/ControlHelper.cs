using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace System.Web.UI.Design
{
	internal static class ControlHelper
	{
		internal static Control FindControl(IServiceProvider serviceProvider, Control control, string controlIdToFind)
		{
			if (string.IsNullOrEmpty(controlIdToFind))
			{
				throw new ArgumentNullException("controlIdToFind");
			}
			while (control != null)
			{
				if (control.Site == null || control.Site.Container == null)
				{
					return null;
				}
				IComponent component = control.Site.Container.Components[controlIdToFind];
				if (component != null)
				{
					return component as Control;
				}
				IDesignerHost designerHost = (IDesignerHost)control.Site.GetService(typeof(IDesignerHost));
				if (designerHost == null)
				{
					return null;
				}
				ControlDesigner controlDesigner = designerHost.GetDesigner(control) as ControlDesigner;
				if (controlDesigner == null || controlDesigner.View == null || controlDesigner.View.NamingContainerDesigner == null)
				{
					return null;
				}
				control = controlDesigner.View.NamingContainerDesigner.Component as Control;
			}
			if (serviceProvider != null)
			{
				IDesignerHost designerHost2 = (IDesignerHost)serviceProvider.GetService(typeof(IDesignerHost));
				if (designerHost2 != null)
				{
					IContainer container = designerHost2.Container;
					if (container != null)
					{
						return container.Components[controlIdToFind] as Control;
					}
				}
			}
			return null;
		}

		internal static IList<IComponent> GetAllComponents(IComponent component, ControlHelper.IsValidComponentDelegate componentFilter)
		{
			List<IComponent> list = new List<IComponent>();
			while (component != null)
			{
				IList<IComponent> componentsInContainer = ControlHelper.GetComponentsInContainer(component, componentFilter);
				list.AddRange(componentsInContainer);
				IDesignerHost designerHost = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
				ControlDesigner controlDesigner = designerHost.GetDesigner(component) as ControlDesigner;
				component = null;
				if (controlDesigner != null && controlDesigner.View != null && controlDesigner.View.NamingContainerDesigner != null)
				{
					component = controlDesigner.View.NamingContainerDesigner.Component;
				}
			}
			return list;
		}

		private static IList<IComponent> GetComponentsInContainer(IComponent component, ControlHelper.IsValidComponentDelegate componentFilter)
		{
			List<IComponent> list = new List<IComponent>();
			if (component.Site != null && component.Site.Container != null)
			{
				foreach (object obj in component.Site.Container.Components)
				{
					IComponent component2 = (IComponent)obj;
					if (componentFilter(component2) && !Marshal.IsComObject(component2))
					{
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component2)["Modifiers"];
						if (propertyDescriptor != null)
						{
							MemberAttributes memberAttributes = (MemberAttributes)propertyDescriptor.GetValue(component2);
							if ((memberAttributes & MemberAttributes.AccessMask) == MemberAttributes.Private)
							{
								continue;
							}
						}
						list.Add(component2);
					}
				}
			}
			return list;
		}

		internal delegate bool IsValidComponentDelegate(IComponent component);
	}
}
