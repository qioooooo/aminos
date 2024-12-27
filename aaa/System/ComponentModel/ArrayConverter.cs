using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000094 RID: 148
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ArrayConverter : CollectionConverter
	{
		// Token: 0x06000566 RID: 1382 RVA: 0x00016BB4 File Offset: 0x00015BB4
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value is Array)
			{
				return SR.GetString("ArrayConverterText", new object[] { value.GetType().Name });
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00016C14 File Offset: 0x00015C14
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptor[] array = null;
			if (value.GetType().IsArray)
			{
				Array array2 = (Array)value;
				int length = array2.GetLength(0);
				array = new PropertyDescriptor[length];
				Type type = value.GetType();
				Type elementType = type.GetElementType();
				for (int i = 0; i < length; i++)
				{
					array[i] = new ArrayConverter.ArrayPropertyDescriptor(type, elementType, i);
				}
			}
			return new PropertyDescriptorCollection(array);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00016C79 File Offset: 0x00015C79
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x02000095 RID: 149
		private class ArrayPropertyDescriptor : TypeConverter.SimplePropertyDescriptor
		{
			// Token: 0x0600056A RID: 1386 RVA: 0x00016C84 File Offset: 0x00015C84
			public ArrayPropertyDescriptor(Type arrayType, Type elementType, int index)
				: base(arrayType, "[" + index + "]", elementType, null)
			{
				this.index = index;
			}

			// Token: 0x0600056B RID: 1387 RVA: 0x00016CAC File Offset: 0x00015CAC
			public override object GetValue(object instance)
			{
				if (instance is Array)
				{
					Array array = (Array)instance;
					if (array.GetLength(0) > this.index)
					{
						return array.GetValue(this.index);
					}
				}
				return null;
			}

			// Token: 0x0600056C RID: 1388 RVA: 0x00016CE8 File Offset: 0x00015CE8
			public override void SetValue(object instance, object value)
			{
				if (instance is Array)
				{
					Array array = (Array)instance;
					if (array.GetLength(0) > this.index)
					{
						array.SetValue(value, this.index);
					}
					this.OnValueChanged(instance, EventArgs.Empty);
				}
			}

			// Token: 0x040008CA RID: 2250
			private int index;
		}
	}
}
