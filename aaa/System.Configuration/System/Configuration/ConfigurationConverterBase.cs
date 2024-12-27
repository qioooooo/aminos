using System;
using System.ComponentModel;

namespace System.Configuration
{
	// Token: 0x0200001E RID: 30
	public abstract class ConfigurationConverterBase : TypeConverter
	{
		// Token: 0x060001B0 RID: 432 RVA: 0x0000C57B File Offset: 0x0000B57B
		public override bool CanConvertTo(ITypeDescriptorContext ctx, Type type)
		{
			return type == typeof(string);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000C58A File Offset: 0x0000B58A
		public override bool CanConvertFrom(ITypeDescriptorContext ctx, Type type)
		{
			return type == typeof(string);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000C59C File Offset: 0x0000B59C
		internal void ValidateType(object value, Type expected)
		{
			if (value != null && value.GetType() != expected)
			{
				throw new ArgumentException(SR.GetString("Converter_unsupported_value_type", new object[] { expected.Name }));
			}
		}
	}
}
