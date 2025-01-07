using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	internal class DataSourceConverter : ReferenceConverter
	{
		public DataSourceConverter()
			: base(typeof(IListSource))
		{
		}

		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			ArrayList arrayList = new ArrayList(base.GetStandardValues(context));
			TypeConverter.StandardValuesCollection standardValues = this.listConverter.GetStandardValues(context);
			ArrayList arrayList2 = new ArrayList();
			BindingSource bindingSource = context.Instance as BindingSource;
			foreach (object obj in arrayList)
			{
				if (obj != null)
				{
					ListBindableAttribute listBindableAttribute = (ListBindableAttribute)TypeDescriptor.GetAttributes(obj)[typeof(ListBindableAttribute)];
					if ((listBindableAttribute == null || listBindableAttribute.ListBindable) && (bindingSource == null || bindingSource != obj))
					{
						DataTable dataTable = obj as DataTable;
						if (dataTable == null || !arrayList.Contains(dataTable.DataSet))
						{
							arrayList2.Add(obj);
						}
					}
				}
			}
			foreach (object obj2 in standardValues)
			{
				if (obj2 != null)
				{
					ListBindableAttribute listBindableAttribute2 = (ListBindableAttribute)TypeDescriptor.GetAttributes(obj2)[typeof(ListBindableAttribute)];
					if ((listBindableAttribute2 == null || listBindableAttribute2.ListBindable) && (bindingSource == null || bindingSource != obj2))
					{
						arrayList2.Add(obj2);
					}
				}
			}
			arrayList2.Add(null);
			return new TypeConverter.StandardValuesCollection(arrayList2);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is Type)
			{
				return value.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		private ReferenceConverter listConverter = new ReferenceConverter(typeof(IList));
	}
}
