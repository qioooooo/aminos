using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Diagnostics.Design
{
	internal class InstanceNameConverter : TypeConverter
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
				Array.Sort(instanceNames, Comparer.Default);
				return new TypeConverter.StandardValuesCollection(instanceNames);
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
