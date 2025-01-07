using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataSourceViewSchemaConverter : TypeConverter
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
			return this.GetStandardValues(context, null);
		}

		public virtual TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context, Type typeFilter)
		{
			string[] array = null;
			if (context != null)
			{
				IDataSourceViewSchemaAccessor dataSourceViewSchemaAccessor = context.Instance as IDataSourceViewSchemaAccessor;
				if (dataSourceViewSchemaAccessor != null)
				{
					IDataSourceViewSchema dataSourceViewSchema = dataSourceViewSchemaAccessor.DataSourceViewSchema as IDataSourceViewSchema;
					if (dataSourceViewSchema != null)
					{
						IDataSourceFieldSchema[] fields = dataSourceViewSchema.GetFields();
						string[] array2 = new string[fields.Length];
						int num = 0;
						for (int i = 0; i < fields.Length; i++)
						{
							if ((typeFilter != null && fields[i].DataType == typeFilter) || typeFilter == null)
							{
								array2[num] = fields[i].Name;
								num++;
							}
						}
						array = new string[num];
						Array.Copy(array2, array, num);
					}
				}
				if (array == null)
				{
					array = new string[0];
				}
				Array.Sort(array, Comparer.Default);
			}
			return new TypeConverter.StandardValuesCollection(array);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return context != null && context.Instance is IDataSourceViewSchemaAccessor;
		}
	}
}
