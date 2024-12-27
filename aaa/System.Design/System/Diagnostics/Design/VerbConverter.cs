using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Diagnostics.Design
{
	// Token: 0x020000F1 RID: 241
	internal class VerbConverter : TypeConverter
	{
		// Token: 0x060009F9 RID: 2553 RVA: 0x0002603B File Offset: 0x0002503B
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00026054 File Offset: 0x00025054
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return ((string)value).Trim();
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x00026080 File Offset: 0x00025080
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

		// Token: 0x060009FC RID: 2556 RVA: 0x000260B3 File Offset: 0x000250B3
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x000260B6 File Offset: 0x000250B6
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x04000D4C RID: 3404
		private const string DefaultVerb = "VerbEditorDefault";
	}
}
