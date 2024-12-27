using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200057F RID: 1407
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FontUnitConverter : TypeConverter
	{
		// Token: 0x060044FE RID: 17662 RVA: 0x0011B5A1 File Offset: 0x0011A5A1
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x0011B5BC File Offset: 0x0011A5BC
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
			{
				return null;
			}
			string text = value as string;
			if (text == null)
			{
				return base.ConvertFrom(context, culture, value);
			}
			string text2 = text.Trim();
			if (text2.Length == 0)
			{
				return FontUnit.Empty;
			}
			return FontUnit.Parse(text2, culture);
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x0011B608 File Offset: 0x0011A608
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x0011B630 File Offset: 0x0011A630
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				if (value == null || ((FontUnit)value).Type == FontSize.NotSet)
				{
					return string.Empty;
				}
				return ((FontUnit)value).ToString(culture);
			}
			else
			{
				if (destinationType != typeof(InstanceDescriptor) || value == null)
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				FontUnit fontUnit = (FontUnit)value;
				MemberInfo memberInfo = null;
				object[] array = null;
				if (fontUnit.IsEmpty)
				{
					memberInfo = typeof(FontUnit).GetField("Empty");
				}
				else if (fontUnit.Type != FontSize.AsUnit)
				{
					string text = null;
					switch (fontUnit.Type)
					{
					case FontSize.Smaller:
						text = "Smaller";
						break;
					case FontSize.Larger:
						text = "Larger";
						break;
					case FontSize.XXSmall:
						text = "XXSmall";
						break;
					case FontSize.XSmall:
						text = "XSmall";
						break;
					case FontSize.Small:
						text = "Small";
						break;
					case FontSize.Medium:
						text = "Medium";
						break;
					case FontSize.Large:
						text = "Large";
						break;
					case FontSize.XLarge:
						text = "XLarge";
						break;
					case FontSize.XXLarge:
						text = "XXLarge";
						break;
					}
					if (text != null)
					{
						memberInfo = typeof(FontUnit).GetField(text);
					}
				}
				else
				{
					memberInfo = typeof(FontUnit).GetConstructor(new Type[] { typeof(Unit) });
					array = new object[] { fontUnit.Unit };
				}
				if (memberInfo != null)
				{
					return new InstanceDescriptor(memberInfo, array);
				}
				return null;
			}
		}

		// Token: 0x06004502 RID: 17666 RVA: 0x0011B7BC File Offset: 0x0011A7BC
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.values == null)
			{
				object[] array = new object[]
				{
					FontUnit.Smaller,
					FontUnit.Larger,
					FontUnit.XXSmall,
					FontUnit.XSmall,
					FontUnit.Small,
					FontUnit.Medium,
					FontUnit.Large,
					FontUnit.XLarge,
					FontUnit.XXLarge
				};
				this.values = new TypeConverter.StandardValuesCollection(array);
			}
			return this.values;
		}

		// Token: 0x06004503 RID: 17667 RVA: 0x0011B865 File Offset: 0x0011A865
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x0011B868 File Offset: 0x0011A868
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x040029D9 RID: 10713
		private TypeConverter.StandardValuesCollection values;
	}
}
