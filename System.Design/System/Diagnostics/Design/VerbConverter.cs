using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Diagnostics.Design
{
	internal class VerbConverter : TypeConverter
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
			ProcessStartInfo processStartInfo = ((context == null) ? null : (context.Instance as ProcessStartInfo));
			TypeConverter.StandardValuesCollection standardValuesCollection;
			if (processStartInfo != null)
			{
				standardValuesCollection = new TypeConverter.StandardValuesCollection(processStartInfo.Verbs);
			}
			else
			{
				standardValuesCollection = null;
			}
			return standardValuesCollection;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		private const string DefaultVerb = "VerbEditorDefault";
	}
}
