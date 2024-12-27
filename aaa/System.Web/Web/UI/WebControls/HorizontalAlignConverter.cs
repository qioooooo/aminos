using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005B5 RID: 1461
	internal class HorizontalAlignConverter : EnumConverter
	{
		// Token: 0x0600477F RID: 18303 RVA: 0x001244E8 File Offset: 0x001234E8
		static HorizontalAlignConverter()
		{
			HorizontalAlignConverter.stringValues[0] = "NotSet";
			HorizontalAlignConverter.stringValues[1] = "Left";
			HorizontalAlignConverter.stringValues[2] = "Center";
			HorizontalAlignConverter.stringValues[3] = "Right";
			HorizontalAlignConverter.stringValues[4] = "Justify";
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x0012453C File Offset: 0x0012353C
		public HorizontalAlignConverter()
			: base(typeof(HorizontalAlign))
		{
		}

		// Token: 0x06004781 RID: 18305 RVA: 0x0012454E File Offset: 0x0012354E
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06004782 RID: 18306 RVA: 0x00124568 File Offset: 0x00123568
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is string)
			{
				string text = ((string)value).Trim();
				if (text.Length == 0)
				{
					return HorizontalAlign.NotSet;
				}
				string text2;
				if ((text2 = text) != null)
				{
					if (text2 == "NotSet")
					{
						return HorizontalAlign.NotSet;
					}
					if (text2 == "Left")
					{
						return HorizontalAlign.Left;
					}
					if (text2 == "Center")
					{
						return HorizontalAlign.Center;
					}
					if (text2 == "Right")
					{
						return HorizontalAlign.Right;
					}
					if (text2 == "Justify")
					{
						return HorizontalAlign.Justify;
					}
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x00124614 File Offset: 0x00123614
		public override bool CanConvertTo(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertTo(context, sourceType);
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x0012462D File Offset: 0x0012362D
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && (int)value <= 4)
			{
				return HorizontalAlignConverter.stringValues[(int)value];
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x04002AA2 RID: 10914
		private static string[] stringValues = new string[5];
	}
}
