using System;
using System.CodeDom;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200015B RID: 347
	internal class EnumCodeDomSerializer : CodeDomSerializer
	{
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x00033E4A File Offset: 0x00032E4A
		internal new static EnumCodeDomSerializer Default
		{
			get
			{
				if (EnumCodeDomSerializer.defaultSerializer == null)
				{
					EnumCodeDomSerializer.defaultSerializer = new EnumCodeDomSerializer();
				}
				return EnumCodeDomSerializer.defaultSerializer;
			}
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00033E64 File Offset: 0x00032E64
		public override object Serialize(IDesignerSerializationManager manager, object value)
		{
			CodeExpression codeExpression = null;
			using (CodeDomSerializerBase.TraceScope("EnumCodeDomSerializer::Serialize"))
			{
				if (value is Enum)
				{
					TypeConverter converter = TypeDescriptor.GetConverter(value);
					Enum[] array;
					bool flag;
					if (converter != null && converter.CanConvertTo(typeof(Enum[])))
					{
						array = (Enum[])converter.ConvertTo(value, typeof(Enum[]));
						flag = array.Length > 1;
					}
					else
					{
						array = new Enum[] { (Enum)value };
						flag = true;
					}
					CodeTypeReferenceExpression codeTypeReferenceExpression = new CodeTypeReferenceExpression(value.GetType());
					TypeConverter typeConverter = new EnumConverter(value.GetType());
					foreach (Enum @enum in array)
					{
						string text = ((typeConverter != null) ? typeConverter.ConvertToString(@enum) : null);
						CodeExpression codeExpression2 = ((!string.IsNullOrEmpty(text)) ? new CodeFieldReferenceExpression(codeTypeReferenceExpression, text) : null);
						if (codeExpression2 != null)
						{
							if (codeExpression == null)
							{
								codeExpression = codeExpression2;
							}
							else
							{
								codeExpression = new CodeBinaryOperatorExpression(codeExpression, CodeBinaryOperatorType.BitwiseOr, codeExpression2);
							}
						}
					}
					if (codeExpression != null && flag)
					{
						codeExpression = new CodeCastExpression(value.GetType(), codeExpression);
					}
				}
			}
			return codeExpression;
		}

		// Token: 0x04000EEB RID: 3819
		private static EnumCodeDomSerializer defaultSerializer;
	}
}
