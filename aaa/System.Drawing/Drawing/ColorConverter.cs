using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Drawing
{
	// Token: 0x0200003A RID: 58
	public class ColorConverter : TypeConverter
	{
		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000AEA8 File Offset: 0x00009EA8
		private static Hashtable Colors
		{
			get
			{
				if (ColorConverter.colorConstants == null)
				{
					lock (ColorConverter.ColorConstantsLock)
					{
						if (ColorConverter.colorConstants == null)
						{
							Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
							ColorConverter.FillConstants(hashtable, typeof(Color));
							ColorConverter.colorConstants = hashtable;
						}
					}
				}
				return ColorConverter.colorConstants;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000AF10 File Offset: 0x00009F10
		private static Hashtable SystemColors
		{
			get
			{
				if (ColorConverter.systemColorConstants == null)
				{
					lock (ColorConverter.SystemColorConstantsLock)
					{
						if (ColorConverter.systemColorConstants == null)
						{
							Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
							ColorConverter.FillConstants(hashtable, typeof(SystemColors));
							ColorConverter.systemColorConstants = hashtable;
						}
					}
				}
				return ColorConverter.systemColorConstants;
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000AF78 File Offset: 0x00009F78
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000AF91 File Offset: 0x00009F91
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000AFAC File Offset: 0x00009FAC
		internal static object GetNamedColor(string name)
		{
			object obj = ColorConverter.Colors[name];
			if (obj != null)
			{
				return obj;
			}
			return ColorConverter.SystemColors[name];
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000AFDC File Offset: 0x00009FDC
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				object obj = null;
				string text2 = text.Trim();
				if (text2.Length == 0)
				{
					obj = Color.Empty;
				}
				else
				{
					obj = ColorConverter.GetNamedColor(text2);
					if (obj == null)
					{
						if (culture == null)
						{
							culture = CultureInfo.CurrentCulture;
						}
						char c = culture.TextInfo.ListSeparator[0];
						bool flag = true;
						TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
						if (text2.IndexOf(c) == -1)
						{
							if (text2.Length >= 2 && (text2[0] == '\'' || text2[0] == '"') && text2[0] == text2[text2.Length - 1])
							{
								string text3 = text2.Substring(1, text2.Length - 2);
								obj = Color.FromName(text3);
								flag = false;
							}
							else if ((text2.Length == 7 && text2[0] == '#') || (text2.Length == 8 && (text2.StartsWith("0x") || text2.StartsWith("0X"))) || (text2.Length == 8 && (text2.StartsWith("&h") || text2.StartsWith("&H"))))
							{
								obj = Color.FromArgb(-16777216 | (int)converter.ConvertFromString(context, culture, text2));
							}
						}
						if (obj == null)
						{
							string[] array = text2.Split(new char[] { c });
							int[] array2 = new int[array.Length];
							for (int i = 0; i < array2.Length; i++)
							{
								array2[i] = (int)converter.ConvertFromString(context, culture, array[i]);
							}
							switch (array2.Length)
							{
							case 1:
								obj = Color.FromArgb(array2[0]);
								break;
							case 3:
								obj = Color.FromArgb(array2[0], array2[1], array2[2]);
								break;
							case 4:
								obj = Color.FromArgb(array2[0], array2[1], array2[2], array2[3]);
								break;
							}
							flag = true;
						}
						if (obj != null && flag)
						{
							int num = ((Color)obj).ToArgb();
							foreach (object obj2 in ColorConverter.Colors.Values)
							{
								Color color = (Color)obj2;
								if (color.ToArgb() == num)
								{
									obj = color;
									break;
								}
							}
						}
					}
					if (obj == null)
					{
						throw new ArgumentException(SR.GetString("InvalidColor", new object[] { text2 }));
					}
				}
				return obj;
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000B29C File Offset: 0x0000A29C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is Color)
			{
				if (destinationType == typeof(string))
				{
					Color color = (Color)value;
					if (color == Color.Empty)
					{
						return string.Empty;
					}
					if (color.IsKnownColor)
					{
						return color.Name;
					}
					if (color.IsNamedColor)
					{
						return "'" + color.Name + "'";
					}
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					string text = culture.TextInfo.ListSeparator + " ";
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
					int num = 0;
					string[] array;
					if (color.A < 255)
					{
						array = new string[4];
						array[num++] = converter.ConvertToString(context, culture, color.A);
					}
					else
					{
						array = new string[3];
					}
					array[num++] = converter.ConvertToString(context, culture, color.R);
					array[num++] = converter.ConvertToString(context, culture, color.G);
					array[num++] = converter.ConvertToString(context, culture, color.B);
					return string.Join(text, array);
				}
				else if (destinationType == typeof(InstanceDescriptor))
				{
					object[] array2 = null;
					Color color2 = (Color)value;
					MemberInfo memberInfo;
					if (color2.IsEmpty)
					{
						memberInfo = typeof(Color).GetField("Empty");
					}
					else if (color2.IsSystemColor)
					{
						memberInfo = typeof(SystemColors).GetProperty(color2.Name);
					}
					else if (color2.IsKnownColor)
					{
						memberInfo = typeof(Color).GetProperty(color2.Name);
					}
					else if (color2.A != 255)
					{
						memberInfo = typeof(Color).GetMethod("FromArgb", new Type[]
						{
							typeof(int),
							typeof(int),
							typeof(int),
							typeof(int)
						});
						array2 = new object[] { color2.A, color2.R, color2.G, color2.B };
					}
					else if (color2.IsNamedColor)
					{
						memberInfo = typeof(Color).GetMethod("FromName", new Type[] { typeof(string) });
						array2 = new object[] { color2.Name };
					}
					else
					{
						memberInfo = typeof(Color).GetMethod("FromArgb", new Type[]
						{
							typeof(int),
							typeof(int),
							typeof(int)
						});
						array2 = new object[] { color2.R, color2.G, color2.B };
					}
					if (memberInfo != null)
					{
						return new InstanceDescriptor(memberInfo, array2);
					}
					return null;
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000B62C File Offset: 0x0000A62C
		private static void FillConstants(Hashtable hash, Type enumType)
		{
			MethodAttributes methodAttributes = MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static;
			foreach (PropertyInfo propertyInfo in enumType.GetProperties())
			{
				if (propertyInfo.PropertyType == typeof(Color))
				{
					MethodInfo getMethod = propertyInfo.GetGetMethod();
					if (getMethod != null && (getMethod.Attributes & methodAttributes) == methodAttributes)
					{
						object[] array = null;
						hash[propertyInfo.Name] = propertyInfo.GetValue(null, array);
					}
				}
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000B698 File Offset: 0x0000A698
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (ColorConverter.values == null)
			{
				lock (ColorConverter.ValuesLock)
				{
					if (ColorConverter.values == null)
					{
						ArrayList arrayList = new ArrayList();
						arrayList.AddRange(ColorConverter.Colors.Values);
						arrayList.AddRange(ColorConverter.SystemColors.Values);
						int num = arrayList.Count;
						for (int i = 0; i < num - 1; i++)
						{
							for (int j = i + 1; j < num; j++)
							{
								if (arrayList[i].Equals(arrayList[j]))
								{
									arrayList.RemoveAt(j);
									num--;
									j--;
								}
							}
						}
						arrayList.Sort(0, arrayList.Count, new ColorConverter.ColorComparer());
						ColorConverter.values = new TypeConverter.StandardValuesCollection(arrayList.ToArray());
					}
				}
			}
			return ColorConverter.values;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000B774 File Offset: 0x0000A774
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x04000279 RID: 633
		private static string ColorConstantsLock = "colorConstants";

		// Token: 0x0400027A RID: 634
		private static Hashtable colorConstants;

		// Token: 0x0400027B RID: 635
		private static string SystemColorConstantsLock = "systemColorConstants";

		// Token: 0x0400027C RID: 636
		private static Hashtable systemColorConstants;

		// Token: 0x0400027D RID: 637
		private static string ValuesLock = "values";

		// Token: 0x0400027E RID: 638
		private static TypeConverter.StandardValuesCollection values;

		// Token: 0x0200003B RID: 59
		private class ColorComparer : IComparer
		{
			// Token: 0x06000311 RID: 785 RVA: 0x0000B798 File Offset: 0x0000A798
			public int Compare(object left, object right)
			{
				Color color = (Color)left;
				Color color2 = (Color)right;
				return string.Compare(color.Name, color2.Name, false, CultureInfo.InvariantCulture);
			}
		}
	}
}
