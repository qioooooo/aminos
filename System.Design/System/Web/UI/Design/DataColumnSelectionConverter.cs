using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataColumnSelectionConverter : TypeConverter
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
			ArrayList arrayList = new ArrayList();
			if (context != null)
			{
				IComponent component = context.Instance as IComponent;
				if (component != null)
				{
					GridView gridView = component as GridView;
					if (gridView != null)
					{
						if (gridView.AutoGenerateColumns)
						{
							DataFieldConverter dataFieldConverter = new DataFieldConverter();
							TypeConverter.StandardValuesCollection standardValues = dataFieldConverter.GetStandardValues(context);
							foreach (object obj in standardValues)
							{
								arrayList.Add(obj);
							}
						}
						DataControlFieldCollection columns = gridView.Columns;
						foreach (object obj2 in columns)
						{
							DataControlField dataControlField = (DataControlField)obj2;
							BoundField boundField = dataControlField as BoundField;
							if (boundField != null)
							{
								string dataField = boundField.DataField;
								if (!arrayList.Contains(dataField))
								{
									arrayList.Add(dataField);
								}
							}
						}
						arrayList.Sort();
						array = new string[arrayList.Count];
						arrayList.CopyTo(array, 0);
					}
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
			return context != null && context.Instance is IComponent;
		}
	}
}
