using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Diagnostics.Design
{
	// Token: 0x020000DE RID: 222
	internal class InstanceNameConverter : TypeConverter
	{
		// Token: 0x06000914 RID: 2324 RVA: 0x0001EC0B File Offset: 0x0001DC0B
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0001EC24 File Offset: 0x0001DC24
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return ((string)value).Trim();
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0001EC50 File Offset: 0x0001DC50
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

		// Token: 0x06000917 RID: 2327 RVA: 0x0001ECCC File Offset: 0x0001DCCC
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
