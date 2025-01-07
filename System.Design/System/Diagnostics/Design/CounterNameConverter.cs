using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Diagnostics.Design
{
	internal class CounterNameConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return ((string)value).Trim();
			}
			return base.ConvertFrom(context, culture, value);
		}

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

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
