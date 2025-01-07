using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Diagnostics.Design
{
	internal class CategoryValueConverter : TypeConverter
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

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		private TypeConverter.StandardValuesCollection values;

		private string previousMachineName;
	}
}
