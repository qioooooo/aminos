using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x02000352 RID: 850
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataSourceViewSchemaConverter : TypeConverter
	{
		// Token: 0x06001FE2 RID: 8162 RVA: 0x000B5C70 File Offset: 0x000B4C70
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x000B5C82 File Offset: 0x000B4C82
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

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000B5CAD File Offset: 0x000B4CAD
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return this.GetStandardValues(context, null);
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x000B5CB8 File Offset: 0x000B4CB8
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

		// Token: 0x06001FE6 RID: 8166 RVA: 0x000B5D64 File Offset: 0x000B4D64
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x000B5D67 File Offset: 0x000B4D67
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return context != null && context.Instance is IDataSourceViewSchemaAccessor;
		}
	}
}
