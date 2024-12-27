using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x02000441 RID: 1089
	public class ImageIndexConverter : Int32Converter
	{
		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06004157 RID: 16727 RVA: 0x000EA2D7 File Offset: 0x000E92D7
		protected virtual bool IncludeNoneAsStandardValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x06004158 RID: 16728 RVA: 0x000EA2DA File Offset: 0x000E92DA
		// (set) Token: 0x06004159 RID: 16729 RVA: 0x000EA2E2 File Offset: 0x000E92E2
		internal string ParentImageListProperty
		{
			get
			{
				return this.parentImageListProperty;
			}
			set
			{
				this.parentImageListProperty = value;
			}
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x000EA2EC File Offset: 0x000E92EC
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null && string.Compare(text, SR.GetString("toStringNone"), true, culture) == 0)
			{
				return -1;
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x000EA328 File Offset: 0x000E9328
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value is int && (int)value == -1)
			{
				return SR.GetString("toStringNone");
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x000EA37C File Offset: 0x000E937C
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (context != null && context.Instance != null)
			{
				object obj = context.Instance;
				PropertyDescriptor propertyDescriptor = ImageListUtils.GetImageListProperty(context.PropertyDescriptor, ref obj);
				while (obj != null && propertyDescriptor == null)
				{
					PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
					foreach (object obj2 in properties)
					{
						PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)obj2;
						if (typeof(ImageList).IsAssignableFrom(propertyDescriptor2.PropertyType))
						{
							propertyDescriptor = propertyDescriptor2;
							break;
						}
					}
					if (propertyDescriptor == null)
					{
						PropertyDescriptor propertyDescriptor3 = properties[this.ParentImageListProperty];
						if (propertyDescriptor3 != null)
						{
							obj = propertyDescriptor3.GetValue(obj);
						}
						else
						{
							obj = null;
						}
					}
				}
				if (propertyDescriptor != null)
				{
					ImageList imageList = (ImageList)propertyDescriptor.GetValue(obj);
					if (imageList != null)
					{
						int count = imageList.Images.Count;
						object[] array;
						if (this.IncludeNoneAsStandardValue)
						{
							array = new object[count + 1];
							array[count] = -1;
						}
						else
						{
							array = new object[count];
						}
						for (int i = 0; i < count; i++)
						{
							array[i] = i;
						}
						return new TypeConverter.StandardValuesCollection(array);
					}
				}
			}
			if (this.IncludeNoneAsStandardValue)
			{
				return new TypeConverter.StandardValuesCollection(new object[] { -1 });
			}
			return new TypeConverter.StandardValuesCollection(new object[0]);
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x000EA4E4 File Offset: 0x000E94E4
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x000EA4E7 File Offset: 0x000E94E7
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x04001F7B RID: 8059
		private string parentImageListProperty = "Parent";
	}
}
