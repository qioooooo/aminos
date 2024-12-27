using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Diagnostics.Design
{
	// Token: 0x020000DB RID: 219
	internal class CategoryValueConverter : TypeConverter
	{
		// Token: 0x06000907 RID: 2311 RVA: 0x0001E90D File Offset: 0x0001D90D
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0001E928 File Offset: 0x0001D928
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return ((string)value).Trim();
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0001E954 File Offset: 0x0001D954
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			PerformanceCounter performanceCounter = ((context == null) ? null : (context.Instance as PerformanceCounter));
			string text = ".";
			if (performanceCounter != null)
			{
				text = performanceCounter.MachineName;
			}
			if (text == this.previousMachineName)
			{
				return this.values;
			}
			this.previousMachineName = text;
			try
			{
				PerformanceCounter.CloseSharedResources();
				PerformanceCounterCategory[] categories = PerformanceCounterCategory.GetCategories(text);
				string[] array = new string[categories.Length];
				for (int i = 0; i < categories.Length; i++)
				{
					array[i] = categories[i].CategoryName;
				}
				Array.Sort(array, Comparer.Default);
				this.values = new TypeConverter.StandardValuesCollection(array);
			}
			catch (Exception)
			{
				this.values = null;
			}
			return this.values;
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0001EA0C File Offset: 0x0001DA0C
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x04000D07 RID: 3335
		private TypeConverter.StandardValuesCollection values;

		// Token: 0x04000D08 RID: 3336
		private string previousMachineName;
	}
}
