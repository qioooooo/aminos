using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Data
{
	// Token: 0x020000CD RID: 205
	internal sealed class PrimaryKeyTypeConverter : ReferenceConverter
	{
		// Token: 0x06000CD8 RID: 3288 RVA: 0x001FC270 File Offset: 0x001FB670
		public PrimaryKeyTypeConverter()
			: base(typeof(DataColumn[]))
		{
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x001FC290 File Offset: 0x001FB690
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x001FC2A0 File Offset: 0x001FB6A0
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x001FC2C4 File Offset: 0x001FB6C4
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string))
			{
				return new DataColumn[0].GetType().Name;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
