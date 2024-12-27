using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.IO;

namespace System.Drawing
{
	// Token: 0x02000048 RID: 72
	public class IconConverter : ExpandableObjectConverter
	{
		// Token: 0x0600046F RID: 1135 RVA: 0x00011D5E File Offset: 0x00010D5E
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(byte[]) || (sourceType != typeof(InstanceDescriptor) && base.CanConvertFrom(context, sourceType));
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00011D86 File Offset: 0x00010D86
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(Image) || destinationType == typeof(Bitmap) || destinationType == typeof(byte[]) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00011DBC File Offset: 0x00010DBC
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is byte[])
			{
				MemoryStream memoryStream = new MemoryStream((byte[])value);
				return new Icon(memoryStream);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00011DF0 File Offset: 0x00010DF0
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(Image) || destinationType == typeof(Bitmap))
			{
				Icon icon = value as Icon;
				if (icon != null)
				{
					return icon.ToBitmap();
				}
			}
			if (destinationType == typeof(string))
			{
				if (value != null)
				{
					return value.ToString();
				}
				return SR.GetString("toStringNone");
			}
			else
			{
				if (destinationType != typeof(byte[]))
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				if (value == null)
				{
					return new byte[0];
				}
				MemoryStream memoryStream = null;
				try
				{
					memoryStream = new MemoryStream();
					Icon icon2 = value as Icon;
					if (icon2 != null)
					{
						icon2.Save(memoryStream);
					}
				}
				finally
				{
					if (memoryStream != null)
					{
						memoryStream.Close();
					}
				}
				if (memoryStream != null)
				{
					return memoryStream.ToArray();
				}
				return null;
			}
		}
	}
}
