using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Microsoft.Win32;

namespace System.Drawing
{
	// Token: 0x0200003F RID: 63
	public class FontConverter : TypeConverter
	{
		// Token: 0x06000313 RID: 787 RVA: 0x0000B7D4 File Offset: 0x0000A7D4
		~FontConverter()
		{
			if (this.fontNameConverter != null)
			{
				((IDisposable)this.fontNameConverter).Dispose();
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000B810 File Offset: 0x0000A810
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000B829 File Offset: 0x0000A829
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000B844 File Offset: 0x0000A844
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text == null)
			{
				return base.ConvertFrom(context, culture, value);
			}
			string text2 = text.Trim();
			if (text2.Length == 0)
			{
				return null;
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			char c = culture.TextInfo.ListSeparator[0];
			string text3 = text2;
			string text4 = null;
			float num = 8.25f;
			FontStyle fontStyle = FontStyle.Regular;
			GraphicsUnit graphicsUnit = GraphicsUnit.Point;
			int num2 = text2.IndexOf(c);
			if (num2 > 0)
			{
				text3 = text2.Substring(0, num2);
				if (num2 < text2.Length - 1)
				{
					int num3 = text2.IndexOf("style=");
					string text5;
					if (num3 != -1)
					{
						text4 = text2.Substring(num3, text2.Length - num3);
						if (!text4.StartsWith("style="))
						{
							throw this.GetFormatException(text2, c);
						}
						text5 = text2.Substring(num2 + 1, num3 - num2 - 1);
					}
					else
					{
						text5 = text2.Substring(num2 + 1, text2.Length - num2 - 1);
					}
					string[] array = this.ParseSizeTokens(text5, c);
					if (array[0] != null)
					{
						try
						{
							num = (float)TypeDescriptor.GetConverter(typeof(float)).ConvertFromString(context, culture, array[0]);
						}
						catch
						{
							throw this.GetFormatException(text2, c);
						}
					}
					if (array[1] != null)
					{
						graphicsUnit = this.ParseGraphicsUnits(array[1]);
					}
					if (text4 != null)
					{
						int num4 = text4.IndexOf("=");
						text4 = text4.Substring(num4 + 1, text4.Length - "style=".Length);
						foreach (string text6 in text4.Split(new char[] { c }))
						{
							text6 = text6.Trim();
							try
							{
								fontStyle |= (FontStyle)Enum.Parse(typeof(FontStyle), text6, true);
							}
							catch (Exception ex)
							{
								if (ex is InvalidEnumArgumentException)
								{
									throw;
								}
								throw this.GetFormatException(text2, c);
							}
							FontStyle fontStyle2 = FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout;
							if ((fontStyle | fontStyle2) != fontStyle2)
							{
								throw new InvalidEnumArgumentException("style", (int)fontStyle, typeof(FontStyle));
							}
						}
					}
				}
			}
			if (this.fontNameConverter == null)
			{
				this.fontNameConverter = new FontConverter.FontNameConverter();
			}
			text3 = (string)this.fontNameConverter.ConvertFrom(context, culture, text3);
			return new Font(text3, num, fontStyle, graphicsUnit);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000BAA4 File Offset: 0x0000AAA4
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType != typeof(string))
			{
				if (destinationType == typeof(InstanceDescriptor) && value is Font)
				{
					Font font = (Font)value;
					int num = 2;
					if (font.GdiVerticalFont)
					{
						num = 6;
					}
					else if (font.GdiCharSet != 1)
					{
						num = 5;
					}
					else if (font.Unit != GraphicsUnit.Point)
					{
						num = 4;
					}
					else if (font.Style != FontStyle.Regular)
					{
						num++;
					}
					object[] array = new object[num];
					Type[] array2 = new Type[num];
					array[0] = font.Name;
					array2[0] = typeof(string);
					array[1] = font.Size;
					array2[1] = typeof(float);
					if (num > 2)
					{
						array[2] = font.Style;
						array2[2] = typeof(FontStyle);
					}
					if (num > 3)
					{
						array[3] = font.Unit;
						array2[3] = typeof(GraphicsUnit);
					}
					if (num > 4)
					{
						array[4] = font.GdiCharSet;
						array2[4] = typeof(byte);
					}
					if (num > 5)
					{
						array[5] = font.GdiVerticalFont;
						array2[5] = typeof(bool);
					}
					MemberInfo constructor = typeof(Font).GetConstructor(array2);
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, array);
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
			Font font2 = value as Font;
			if (font2 == null)
			{
				return SR.GetString("toStringNone");
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			string text = culture.TextInfo.ListSeparator + " ";
			int num2 = 2;
			if (font2.Style != FontStyle.Regular)
			{
				num2++;
			}
			string[] array3 = new string[num2];
			int num3 = 0;
			array3[num3++] = font2.Name;
			array3[num3++] = TypeDescriptor.GetConverter(font2.Size).ConvertToString(context, culture, font2.Size) + this.GetGraphicsUnitText(font2.Unit);
			if (font2.Style != FontStyle.Regular)
			{
				array3[num3++] = "style=" + font2.Style.ToString("G");
			}
			return string.Join(text, array3);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000BD04 File Offset: 0x0000AD04
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			if (propertyValues == null)
			{
				throw new ArgumentNullException("propertyValues");
			}
			object obj = propertyValues["Name"];
			object obj2 = propertyValues["Size"];
			object obj3 = propertyValues["Unit"];
			object obj4 = propertyValues["Bold"];
			object obj5 = propertyValues["Italic"];
			object obj6 = propertyValues["Strikeout"];
			object obj7 = propertyValues["Underline"];
			object obj8 = propertyValues["GdiCharSet"];
			object obj9 = propertyValues["GdiVerticalFont"];
			if (obj == null)
			{
				obj = "Tahoma";
			}
			if (obj2 == null)
			{
				obj2 = 8f;
			}
			if (obj3 == null)
			{
				obj3 = GraphicsUnit.Point;
			}
			if (obj4 == null)
			{
				obj4 = false;
			}
			if (obj5 == null)
			{
				obj5 = false;
			}
			if (obj6 == null)
			{
				obj6 = false;
			}
			if (obj7 == null)
			{
				obj7 = false;
			}
			if (obj8 == null)
			{
				obj8 = 0;
			}
			if (obj9 == null)
			{
				obj9 = false;
			}
			if (!(obj is string) || !(obj2 is float) || !(obj8 is byte) || !(obj3 is GraphicsUnit) || !(obj4 is bool) || !(obj5 is bool) || !(obj6 is bool) || !(obj7 is bool) || !(obj9 is bool))
			{
				throw new ArgumentException(SR.GetString("PropertyValueInvalidEntry"));
			}
			FontStyle fontStyle = FontStyle.Regular;
			if (obj4 != null && (bool)obj4)
			{
				fontStyle |= FontStyle.Bold;
			}
			if (obj5 != null && (bool)obj5)
			{
				fontStyle |= FontStyle.Italic;
			}
			if (obj6 != null && (bool)obj6)
			{
				fontStyle |= FontStyle.Strikeout;
			}
			if (obj7 != null && (bool)obj7)
			{
				fontStyle |= FontStyle.Underline;
			}
			return new Font((string)obj, (float)obj2, fontStyle, (GraphicsUnit)obj3, (byte)obj8, (bool)obj9);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000BEC8 File Offset: 0x0000AEC8
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000BECC File Offset: 0x0000AECC
		private ArgumentException GetFormatException(string text, char separator)
		{
			string text2 = string.Format(CultureInfo.CurrentCulture, "name{0} size[units[{0} style=style1[{0} style2{0} ...]]]", new object[] { separator });
			return new ArgumentException(SR.GetString("TextParseFailedFormat", new object[] { text, text2 }));
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000BF1C File Offset: 0x0000AF1C
		private string GetGraphicsUnitText(GraphicsUnit units)
		{
			string text = "";
			for (int i = 0; i < FontConverter.UnitName.names.Length; i++)
			{
				if (FontConverter.UnitName.names[i].unit == units)
				{
					text = FontConverter.UnitName.names[i].name;
					break;
				}
			}
			return text;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000BF60 File Offset: 0x0000AF60
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Font), attributes);
			return properties.Sort(new string[] { "Name", "Size", "Unit", "Weight" });
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000BFAC File Offset: 0x0000AFAC
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000BFB0 File Offset: 0x0000AFB0
		private string[] ParseSizeTokens(string text, char separator)
		{
			string text2 = null;
			string text3 = null;
			text = text.Trim();
			int length = text.Length;
			if (length > 0)
			{
				int num = 0;
				while (num < length && !char.IsLetter(text[num]))
				{
					num++;
				}
				char[] array = new char[] { separator, ' ' };
				if (num > 0)
				{
					text2 = text.Substring(0, num);
					text2 = text2.Trim(array);
				}
				if (num < length)
				{
					text3 = text.Substring(num);
					text3 = text3.TrimEnd(array);
				}
			}
			return new string[] { text2, text3 };
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000C044 File Offset: 0x0000B044
		private GraphicsUnit ParseGraphicsUnits(string units)
		{
			FontConverter.UnitName unitName = null;
			for (int i = 0; i < FontConverter.UnitName.names.Length; i++)
			{
				if (string.Equals(FontConverter.UnitName.names[i].name, units, StringComparison.OrdinalIgnoreCase))
				{
					unitName = FontConverter.UnitName.names[i];
					break;
				}
			}
			if (unitName == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[] { "units", units }));
			}
			return unitName.unit;
		}

		// Token: 0x0400029F RID: 671
		private const string styleHdr = "style=";

		// Token: 0x040002A0 RID: 672
		private FontConverter.FontNameConverter fontNameConverter;

		// Token: 0x02000040 RID: 64
		internal class UnitName
		{
			// Token: 0x06000321 RID: 801 RVA: 0x0000C0B9 File Offset: 0x0000B0B9
			internal UnitName(string name, GraphicsUnit unit)
			{
				this.name = name;
				this.unit = unit;
			}

			// Token: 0x040002A1 RID: 673
			internal string name;

			// Token: 0x040002A2 RID: 674
			internal GraphicsUnit unit;

			// Token: 0x040002A3 RID: 675
			internal static readonly FontConverter.UnitName[] names = new FontConverter.UnitName[]
			{
				new FontConverter.UnitName("world", GraphicsUnit.World),
				new FontConverter.UnitName("display", GraphicsUnit.Display),
				new FontConverter.UnitName("px", GraphicsUnit.Pixel),
				new FontConverter.UnitName("pt", GraphicsUnit.Point),
				new FontConverter.UnitName("in", GraphicsUnit.Inch),
				new FontConverter.UnitName("doc", GraphicsUnit.Document),
				new FontConverter.UnitName("mm", GraphicsUnit.Millimeter)
			};
		}

		// Token: 0x02000041 RID: 65
		public sealed class FontNameConverter : TypeConverter, IDisposable
		{
			// Token: 0x06000323 RID: 803 RVA: 0x0000C14C File Offset: 0x0000B14C
			public FontNameConverter()
			{
				SystemEvents.InstalledFontsChanged += this.OnInstalledFontsChanged;
			}

			// Token: 0x06000324 RID: 804 RVA: 0x0000C165 File Offset: 0x0000B165
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x06000325 RID: 805 RVA: 0x0000C17E File Offset: 0x0000B17E
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				if (value is string)
				{
					return this.MatchFontName((string)value, context);
				}
				return base.ConvertFrom(context, culture, value);
			}

			// Token: 0x06000326 RID: 806 RVA: 0x0000C19F File Offset: 0x0000B19F
			void IDisposable.Dispose()
			{
				SystemEvents.InstalledFontsChanged -= this.OnInstalledFontsChanged;
			}

			// Token: 0x06000327 RID: 807 RVA: 0x0000C1B4 File Offset: 0x0000B1B4
			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				if (this.values == null)
				{
					FontFamily[] families = FontFamily.Families;
					Hashtable hashtable = new Hashtable();
					for (int i = 0; i < families.Length; i++)
					{
						string name = families[i].Name;
						hashtable[name.ToLower(CultureInfo.InvariantCulture)] = name;
					}
					object[] array = new object[hashtable.Values.Count];
					hashtable.Values.CopyTo(array, 0);
					Array.Sort(array, Comparer.Default);
					this.values = new TypeConverter.StandardValuesCollection(array);
				}
				return this.values;
			}

			// Token: 0x06000328 RID: 808 RVA: 0x0000C23D File Offset: 0x0000B23D
			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return false;
			}

			// Token: 0x06000329 RID: 809 RVA: 0x0000C240 File Offset: 0x0000B240
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}

			// Token: 0x0600032A RID: 810 RVA: 0x0000C244 File Offset: 0x0000B244
			private string MatchFontName(string name, ITypeDescriptorContext context)
			{
				string text = null;
				name = name.ToLower(CultureInfo.InvariantCulture);
				foreach (object obj in this.GetStandardValues(context))
				{
					string text2 = obj.ToString().ToLower(CultureInfo.InvariantCulture);
					IEnumerator enumerator;
					if (text2.Equals(name))
					{
						return enumerator.Current.ToString();
					}
					if (text2.StartsWith(name) && (text == null || text2.Length <= text.Length))
					{
						text = enumerator.Current.ToString();
					}
				}
				if (text == null)
				{
					text = name;
				}
				return text;
			}

			// Token: 0x0600032B RID: 811 RVA: 0x0000C2CE File Offset: 0x0000B2CE
			private void OnInstalledFontsChanged(object sender, EventArgs e)
			{
				this.values = null;
			}

			// Token: 0x040002A4 RID: 676
			private TypeConverter.StandardValuesCollection values;
		}

		// Token: 0x02000042 RID: 66
		public class FontUnitConverter : EnumConverter
		{
			// Token: 0x0600032C RID: 812 RVA: 0x0000C2D7 File Offset: 0x0000B2D7
			public FontUnitConverter()
				: base(typeof(GraphicsUnit))
			{
			}

			// Token: 0x0600032D RID: 813 RVA: 0x0000C2EC File Offset: 0x0000B2EC
			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				if (base.Values == null)
				{
					base.GetStandardValues(context);
					ArrayList arrayList = new ArrayList(base.Values);
					arrayList.Remove(GraphicsUnit.Display);
					base.Values = new TypeConverter.StandardValuesCollection(arrayList);
				}
				return base.Values;
			}
		}
	}
}
