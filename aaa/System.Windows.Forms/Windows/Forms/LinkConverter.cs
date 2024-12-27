using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x0200046E RID: 1134
	public class LinkConverter : TypeConverter
	{
		// Token: 0x06004289 RID: 17033 RVA: 0x000ED84A File Offset: 0x000EC84A
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x000ED863 File Offset: 0x000EC863
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x000ED88C File Offset: 0x000EC88C
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
				return new LinkLabel.Link(array2[0], array2[1]);
			}
			throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[] { text, "Start, Length" }));
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x000ED974 File Offset: 0x000EC974
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is LinkLabel.Link)
			{
				if (destinationType == typeof(string))
				{
					LinkLabel.Link link = (LinkLabel.Link)value;
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					string text = culture.TextInfo.ListSeparator + " ";
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
					string[] array = new string[2];
					int num = 0;
					array[num++] = converter.ConvertToString(context, culture, link.Start);
					array[num++] = converter.ConvertToString(context, culture, link.Length);
					return string.Join(text, array);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					LinkLabel.Link link2 = (LinkLabel.Link)value;
					if (link2.LinkData == null)
					{
						MemberInfo memberInfo = typeof(LinkLabel.Link).GetConstructor(new Type[]
						{
							typeof(int),
							typeof(int)
						});
						if (memberInfo != null)
						{
							return new InstanceDescriptor(memberInfo, new object[] { link2.Start, link2.Length }, true);
						}
					}
					else
					{
						MemberInfo memberInfo = typeof(LinkLabel.Link).GetConstructor(new Type[]
						{
							typeof(int),
							typeof(int),
							typeof(object)
						});
						if (memberInfo != null)
						{
							return new InstanceDescriptor(memberInfo, new object[] { link2.Start, link2.Length, link2.LinkData }, true);
						}
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
