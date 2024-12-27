using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x02000442 RID: 1090
	public class ImageKeyConverter : StringConverter
	{
		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x06004160 RID: 16736 RVA: 0x000EA4FD File Offset: 0x000E94FD
		protected virtual bool IncludeNoneAsStandardValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06004161 RID: 16737 RVA: 0x000EA500 File Offset: 0x000E9500
		// (set) Token: 0x06004162 RID: 16738 RVA: 0x000EA508 File Offset: 0x000E9508
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

		// Token: 0x06004163 RID: 16739 RVA: 0x000EA511 File Offset: 0x000E9511
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x000EA52A File Offset: 0x000E952A
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return (string)value;
			}
			if (value == null)
			{
				return "";
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x000EA550 File Offset: 0x000E9550
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value != null && value is string && ((string)value).Length == 0)
			{
				return SR.GetString("toStringNone");
			}
			if (destinationType == typeof(string) && value == null)
			{
				return SR.GetString("toStringNone");
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x000EA5C4 File Offset: 0x000E95C4
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
							array[count] = "";
						}
						else
						{
							array = new object[count];
						}
						StringCollection keys = imageList.Images.Keys;
						for (int i = 0; i < keys.Count; i++)
						{
							if (keys[i] != null && keys[i].Length != 0)
							{
								array[i] = keys[i];
							}
						}
						return new TypeConverter.StandardValuesCollection(array);
					}
				}
			}
			if (this.IncludeNoneAsStandardValue)
			{
				return new TypeConverter.StandardValuesCollection(new object[] { "" });
			}
			return new TypeConverter.StandardValuesCollection(new object[0]);
		}

		// Token: 0x06004167 RID: 16743 RVA: 0x000EA760 File Offset: 0x000E9760
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06004168 RID: 16744 RVA: 0x000EA763 File Offset: 0x000E9763
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x04001F7C RID: 8060
		private string parentImageListProperty = "Parent";
	}
}
