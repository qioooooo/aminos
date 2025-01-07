using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Globalization;

namespace System.Web.UI.Design.WebControls
{
	public class DataSourceIDConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
			{
				return string.Empty;
			}
			if (value.GetType() == typeof(string))
			{
				return (string)value;
			}
			throw base.GetConvertFromException(value);
		}

		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			string[] array = null;
			if (context != null)
			{
				WebFormsRootDesigner webFormsRootDesigner = null;
				IDesignerHost designerHost = (IDesignerHost)context.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					IComponent rootComponent = designerHost.RootComponent;
					if (rootComponent != null)
					{
						webFormsRootDesigner = designerHost.GetDesigner(rootComponent) as WebFormsRootDesigner;
					}
				}
				if (webFormsRootDesigner != null && !webFormsRootDesigner.IsDesignerViewLocked)
				{
					IComponent component = context.Instance as IComponent;
					if (component == null)
					{
						DesignerActionList designerActionList = context.Instance as DesignerActionList;
						if (designerActionList != null)
						{
							component = designerActionList.Component;
						}
					}
					IList<IComponent> allComponents = ControlHelper.GetAllComponents(component, new ControlHelper.IsValidComponentDelegate(this.IsValidDataSource));
					List<string> list = new List<string>();
					foreach (IComponent component2 in allComponents)
					{
						Control control = component2 as Control;
						if (control != null && !string.IsNullOrEmpty(control.ID) && !list.Contains(control.ID))
						{
							list.Add(control.ID);
						}
					}
					list.Sort(StringComparer.OrdinalIgnoreCase);
					list.Insert(0, SR.GetString("DataSourceIDChromeConverter_NoDataSource"));
					list.Add(SR.GetString("DataSourceIDChromeConverter_NewDataSource"));
					array = list.ToArray();
				}
			}
			return new TypeConverter.StandardValuesCollection(array);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		protected virtual bool IsValidDataSource(IComponent component)
		{
			Control control = component as Control;
			return control != null && !string.IsNullOrEmpty(control.ID) && component is IDataSource;
		}
	}
}
