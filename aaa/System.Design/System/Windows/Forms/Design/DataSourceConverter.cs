using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001F7 RID: 503
	internal class DataSourceConverter : ReferenceConverter
	{
		// Token: 0x06001344 RID: 4932 RVA: 0x00062690 File Offset: 0x00061690
		public DataSourceConverter()
			: base(typeof(IListSource))
		{
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x000626B8 File Offset: 0x000616B8
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

		// Token: 0x06001346 RID: 4934 RVA: 0x0006281C File Offset: 0x0006181C
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0006281F File Offset: 0x0006181F
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x00062822 File Offset: 0x00061822
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is Type)
			{
				return value.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x0400119C RID: 4508
		private ReferenceConverter listConverter = new ReferenceConverter(typeof(IList));
	}
}
