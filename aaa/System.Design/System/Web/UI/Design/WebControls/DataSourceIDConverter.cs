using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Globalization;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000444 RID: 1092
	public class DataSourceIDConverter : TypeConverter
	{
		// Token: 0x06002787 RID: 10119 RVA: 0x000D84C0 File Offset: 0x000D74C0
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		// Token: 0x06002788 RID: 10120 RVA: 0x000D84D2 File Offset: 0x000D74D2
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

		// Token: 0x06002789 RID: 10121 RVA: 0x000D8500 File Offset: 0x000D7500
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

		// Token: 0x0600278A RID: 10122 RVA: 0x000D8654 File Offset: 0x000D7654
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x000D8657 File Offset: 0x000D7657
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x000D865C File Offset: 0x000D765C
		protected virtual bool IsValidDataSource(IComponent component)
		{
			Control control = component as Control;
			return control != null && !string.IsNullOrEmpty(control.ID) && component is IDataSource;
		}
	}
}
