using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x0200001F RID: 31
	public sealed class CommaDelimitedStringCollectionConverter : ConfigurationConverterBase
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x0000C5E0 File Offset: 0x0000B5E0
		public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
		{
			base.ValidateType(value, typeof(CommaDelimitedStringCollection));
			CommaDelimitedStringCollection commaDelimitedStringCollection = value as CommaDelimitedStringCollection;
			if (commaDelimitedStringCollection != null)
			{
				return commaDelimitedStringCollection.ToString();
			}
			return null;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000C610 File Offset: 0x0000B610
		public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
		{
			CommaDelimitedStringCollection commaDelimitedStringCollection = new CommaDelimitedStringCollection();
			commaDelimitedStringCollection.FromString((string)data);
			return commaDelimitedStringCollection;
		}
	}
}
