using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x02000469 RID: 1129
	[TypeConverter(typeof(LinkArea.LinkAreaConverter))]
	[Serializable]
	public struct LinkArea
	{
		// Token: 0x0600426F RID: 17007 RVA: 0x000ED431 File Offset: 0x000EC431
		public LinkArea(int start, int length)
		{
			this.start = start;
			this.length = length;
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06004270 RID: 17008 RVA: 0x000ED441 File Offset: 0x000EC441
		// (set) Token: 0x06004271 RID: 17009 RVA: 0x000ED449 File Offset: 0x000EC449
		public int Start
		{
			get
			{
				return this.start;
			}
			set
			{
				this.start = value;
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06004272 RID: 17010 RVA: 0x000ED452 File Offset: 0x000EC452
		// (set) Token: 0x06004273 RID: 17011 RVA: 0x000ED45A File Offset: 0x000EC45A
		public int Length
		{
			get
			{
				return this.length;
			}
			set
			{
				this.length = value;
			}
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06004274 RID: 17012 RVA: 0x000ED463 File Offset: 0x000EC463
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.length == this.start && this.start == 0;
			}
		}

		// Token: 0x06004275 RID: 17013 RVA: 0x000ED480 File Offset: 0x000EC480
		public override bool Equals(object o)
		{
			if (!(o is LinkArea))
			{
				return false;
			}
			LinkArea linkArea = (LinkArea)o;
			return this == linkArea;
		}

		// Token: 0x06004276 RID: 17014 RVA: 0x000ED4AC File Offset: 0x000EC4AC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{Start=",
				this.Start.ToString(CultureInfo.CurrentCulture),
				", Length=",
				this.Length.ToString(CultureInfo.CurrentCulture),
				"}"
			});
		}

		// Token: 0x06004277 RID: 17015 RVA: 0x000ED50A File Offset: 0x000EC50A
		public static bool operator ==(LinkArea linkArea1, LinkArea linkArea2)
		{
			return linkArea1.start == linkArea2.start && linkArea1.length == linkArea2.length;
		}

		// Token: 0x06004278 RID: 17016 RVA: 0x000ED52E File Offset: 0x000EC52E
		public static bool operator !=(LinkArea linkArea1, LinkArea linkArea2)
		{
			return !(linkArea1 == linkArea2);
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x000ED53A File Offset: 0x000EC53A
		public override int GetHashCode()
		{
			return (this.start << 4) | this.length;
		}

		// Token: 0x040020A0 RID: 8352
		private int start;

		// Token: 0x040020A1 RID: 8353
		private int length;

		// Token: 0x0200046A RID: 1130
		public class LinkAreaConverter : TypeConverter
		{
			// Token: 0x0600427A RID: 17018 RVA: 0x000ED54B File Offset: 0x000EC54B
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x0600427B RID: 17019 RVA: 0x000ED564 File Offset: 0x000EC564
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x0600427C RID: 17020 RVA: 0x000ED580 File Offset: 0x000EC580
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				if (!(value is string))
				{
					return base.ConvertFrom(context, culture, value);
				}
				string text = ((string)value).Trim();
				if (text.Length == 0)
				{
					return null;
				}
				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}
				char c = culture.TextInfo.ListSeparator[0];
				string[] array = text.Split(new char[] { c });
				int[] array2 = new int[array.Length];
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = (int)converter.ConvertFromString(context, culture, array[i]);
				}
				if (array2.Length == 2)
				{
					return new LinkArea(array2[0], array2[1]);
				}
				throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[] { text, "start, length" }));
			}

			// Token: 0x0600427D RID: 17021 RVA: 0x000ED66C File Offset: 0x000EC66C
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw new ArgumentNullException("destinationType");
				}
				if (destinationType == typeof(string) && value is LinkArea)
				{
					LinkArea linkArea = (LinkArea)value;
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					string text = culture.TextInfo.ListSeparator + " ";
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
					string[] array = new string[2];
					int num = 0;
					array[num++] = converter.ConvertToString(context, culture, linkArea.Start);
					array[num++] = converter.ConvertToString(context, culture, linkArea.Length);
					return string.Join(text, array);
				}
				if (destinationType == typeof(InstanceDescriptor) && value is LinkArea)
				{
					LinkArea linkArea2 = (LinkArea)value;
					ConstructorInfo constructor = typeof(LinkArea).GetConstructor(new Type[]
					{
						typeof(int),
						typeof(int)
					});
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, new object[] { linkArea2.Start, linkArea2.Length });
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			// Token: 0x0600427E RID: 17022 RVA: 0x000ED7BD File Offset: 0x000EC7BD
			public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
			{
				return new LinkArea((int)propertyValues["Start"], (int)propertyValues["Length"]);
			}

			// Token: 0x0600427F RID: 17023 RVA: 0x000ED7E9 File Offset: 0x000EC7E9
			public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
			{
				return true;
			}

			// Token: 0x06004280 RID: 17024 RVA: 0x000ED7EC File Offset: 0x000EC7EC
			public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(LinkArea), attributes);
				return properties.Sort(new string[] { "Start", "Length" });
			}

			// Token: 0x06004281 RID: 17025 RVA: 0x000ED828 File Offset: 0x000EC828
			public override bool GetPropertiesSupported(ITypeDescriptorContext context)
			{
				return true;
			}
		}
	}
}
