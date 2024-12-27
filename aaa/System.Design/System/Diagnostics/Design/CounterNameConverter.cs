using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Diagnostics.Design
{
	// Token: 0x020000DD RID: 221
	internal class CounterNameConverter : TypeConverter
	{
		// Token: 0x0600090F RID: 2319 RVA: 0x0001EAF0 File Offset: 0x0001DAF0
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0001EB0C File Offset: 0x0001DB0C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return ((string)value).Trim();
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0001EB38 File Offset: 0x0001DB38
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			PerformanceCounter performanceCounter = ((context == null) ? null : (context.Instance as PerformanceCounter));
			string text = ".";
			string text2 = string.Empty;
			if (performanceCounter != null)
			{
				text = performanceCounter.MachineName;
				text2 = performanceCounter.CategoryName;
			}
			try
			{
				PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory(text2, text);
				string[] instanceNames = performanceCounterCategory.GetInstanceNames();
				PerformanceCounter[] array;
				if (instanceNames.Length == 0)
				{
					array = performanceCounterCategory.GetCounters();
				}
				else
				{
					array = performanceCounterCategory.GetCounters(instanceNames[0]);
				}
				string[] array2 = new string[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = array[i].CounterName;
				}
				Array.Sort(array2, Comparer.Default);
				return new TypeConverter.StandardValuesCollection(array2);
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0001EC00 File Offset: 0x0001DC00
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
