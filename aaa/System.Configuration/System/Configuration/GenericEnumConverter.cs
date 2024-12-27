using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace System.Configuration
{
	// Token: 0x0200006B RID: 107
	public sealed class GenericEnumConverter : ConfigurationConverterBase
	{
		// Token: 0x06000414 RID: 1044 RVA: 0x00013C9E File Offset: 0x00012C9E
		public GenericEnumConverter(Type typeEnum)
		{
			if (typeEnum == null)
			{
				throw new ArgumentNullException("typeEnum");
			}
			this._enumType = typeEnum;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00013CBB File Offset: 0x00012CBB
		public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
		{
			return value.ToString();
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00013CC4 File Offset: 0x00012CC4
		public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
		{
			object obj = null;
			try
			{
				string text = (string)data;
				if (string.IsNullOrEmpty(text))
				{
					throw new Exception();
				}
				if (!string.IsNullOrEmpty(text) && (char.IsDigit(text[0]) || text[0] == '-' || text[0] == '+'))
				{
					throw new Exception();
				}
				if (text != text.Trim())
				{
					throw new Exception();
				}
				obj = Enum.Parse(this._enumType, text);
			}
			catch
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text2 in Enum.GetNames(this._enumType))
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(text2);
				}
				throw new ArgumentException(SR.GetString("Invalid_enum_value", new object[] { stringBuilder.ToString() }));
			}
			return obj;
		}

		// Token: 0x04000316 RID: 790
		private Type _enumType;
	}
}
