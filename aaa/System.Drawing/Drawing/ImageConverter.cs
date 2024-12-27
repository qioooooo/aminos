using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Drawing
{
	// Token: 0x0200004B RID: 75
	public class ImageConverter : TypeConverter
	{
		// Token: 0x0600048A RID: 1162 RVA: 0x000125B5 File Offset: 0x000115B5
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == this.iconType || sourceType == typeof(byte[]) || (sourceType != typeof(InstanceDescriptor) && base.CanConvertFrom(context, sourceType));
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x000125E8 File Offset: 0x000115E8
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(byte[]) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00012604 File Offset: 0x00011604
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is Icon)
			{
				Icon icon = (Icon)value;
				return icon.ToBitmap();
			}
			byte[] array = value as byte[];
			if (array != null)
			{
				Stream stream = this.GetBitmapStream(array);
				if (stream == null)
				{
					stream = new MemoryStream(array);
				}
				return Image.FromStream(stream);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00012658 File Offset: 0x00011658
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string))
			{
				if (value != null)
				{
					Image image = (Image)value;
					return image.ToString();
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
				bool flag = false;
				MemoryStream memoryStream = null;
				Image image2 = null;
				try
				{
					memoryStream = new MemoryStream();
					image2 = (Image)value;
					if (image2.RawFormat.Equals(ImageFormat.Icon))
					{
						flag = true;
						image2 = new Bitmap(image2, image2.Width, image2.Height);
					}
					image2.Save(memoryStream);
				}
				finally
				{
					if (memoryStream != null)
					{
						memoryStream.Close();
					}
					if (flag && image2 != null)
					{
						image2.Dispose();
					}
				}
				if (memoryStream != null)
				{
					return memoryStream.ToArray();
				}
				return null;
			}
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00012738 File Offset: 0x00011738
		private unsafe Stream GetBitmapStream(byte[] rawData)
		{
			try
			{
				try
				{
					fixed (byte* ptr = rawData)
					{
						IntPtr intPtr = (IntPtr)((void*)ptr);
						if (intPtr == IntPtr.Zero)
						{
							return null;
						}
						if (rawData.Length <= sizeof(SafeNativeMethods.OBJECTHEADER) || Marshal.ReadInt16(intPtr) != 7189)
						{
							return null;
						}
						SafeNativeMethods.OBJECTHEADER objectheader = (SafeNativeMethods.OBJECTHEADER)Marshal.PtrToStructure(intPtr, typeof(SafeNativeMethods.OBJECTHEADER));
						if (rawData.Length <= (int)(objectheader.headersize + 18))
						{
							return null;
						}
						string @string = Encoding.ASCII.GetString(rawData, (int)(objectheader.headersize + 12), 6);
						if (@string != "PBrush")
						{
							return null;
						}
						byte[] bytes = Encoding.ASCII.GetBytes("BM");
						int num = (int)(objectheader.headersize + 18);
						while (num < (int)(objectheader.headersize + 510) && num + 1 < rawData.Length)
						{
							if (bytes[0] == ptr[num] && bytes[1] == ptr[num + 1])
							{
								return new MemoryStream(rawData, num, rawData.Length - num);
							}
							num++;
						}
					}
				}
				finally
				{
					byte* ptr = null;
				}
			}
			catch (OutOfMemoryException)
			{
			}
			catch (ArgumentException)
			{
			}
			return null;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000128BC File Offset: 0x000118BC
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(typeof(Image), attributes);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x000128CE File Offset: 0x000118CE
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x040002C7 RID: 711
		private Type iconType = typeof(Icon);
	}
}
