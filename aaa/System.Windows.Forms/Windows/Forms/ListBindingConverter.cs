using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x02000479 RID: 1145
	public class ListBindingConverter : TypeConverter
	{
		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x0600434A RID: 17226 RVA: 0x000F0C90 File Offset: 0x000EFC90
		private static Type[] ConstructorParamaterTypes
		{
			get
			{
				if (ListBindingConverter.ctorTypes == null)
				{
					ListBindingConverter.ctorTypes = new Type[]
					{
						typeof(string),
						typeof(object),
						typeof(string),
						typeof(bool),
						typeof(DataSourceUpdateMode),
						typeof(object),
						typeof(string),
						typeof(IFormatProvider)
					};
				}
				return ListBindingConverter.ctorTypes;
			}
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x0600434B RID: 17227 RVA: 0x000F0D20 File Offset: 0x000EFD20
		private static string[] ConstructorParameterProperties
		{
			get
			{
				if (ListBindingConverter.ctorParamProps == null)
				{
					ListBindingConverter.ctorParamProps = new string[] { null, null, null, "FormattingEnabled", "DataSourceUpdateMode", "NullValue", "FormatString", "FormatInfo" };
				}
				return ListBindingConverter.ctorParamProps;
			}
		}

		// Token: 0x0600434C RID: 17228 RVA: 0x000F0D6E File Offset: 0x000EFD6E
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600434D RID: 17229 RVA: 0x000F0D88 File Offset: 0x000EFD88
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is Binding)
			{
				Binding binding = (Binding)value;
				return this.GetInstanceDescriptorFromValues(binding);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x0600434E RID: 17230 RVA: 0x000F0DD4 File Offset: 0x000EFDD4
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			object obj;
			try
			{
				obj = new Binding((string)propertyValues["PropertyName"], propertyValues["DataSource"], (string)propertyValues["DataMember"]);
			}
			catch (InvalidCastException ex)
			{
				throw new ArgumentException(SR.GetString("PropertyValueInvalidEntry"), ex);
			}
			catch (NullReferenceException ex2)
			{
				throw new ArgumentException(SR.GetString("PropertyValueInvalidEntry"), ex2);
			}
			return obj;
		}

		// Token: 0x0600434F RID: 17231 RVA: 0x000F0E58 File Offset: 0x000EFE58
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06004350 RID: 17232 RVA: 0x000F0E5C File Offset: 0x000EFE5C
		private InstanceDescriptor GetInstanceDescriptorFromValues(Binding b)
		{
			b.FormattingEnabled = true;
			bool flag = true;
			int num = ListBindingConverter.ConstructorParameterProperties.Length - 1;
			while (num >= 0 && ListBindingConverter.ConstructorParameterProperties[num] != null)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(b)[ListBindingConverter.ConstructorParameterProperties[num]];
				if (propertyDescriptor != null && propertyDescriptor.ShouldSerializeValue(b))
				{
					break;
				}
				num--;
			}
			Type[] array = new Type[num + 1];
			Array.Copy(ListBindingConverter.ConstructorParamaterTypes, 0, array, 0, array.Length);
			ConstructorInfo constructorInfo = typeof(Binding).GetConstructor(array);
			if (constructorInfo == null)
			{
				flag = false;
				constructorInfo = typeof(Binding).GetConstructor(new Type[]
				{
					typeof(string),
					typeof(object),
					typeof(string)
				});
			}
			object[] array2 = new object[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				object obj;
				switch (i)
				{
				case 0:
					obj = b.PropertyName;
					break;
				case 1:
					obj = b.BindToObject.DataSource;
					break;
				case 2:
					obj = b.BindToObject.BindingMemberInfo.BindingMember;
					break;
				default:
					obj = TypeDescriptor.GetProperties(b)[ListBindingConverter.ConstructorParameterProperties[i]].GetValue(b);
					break;
				}
				array2[i] = obj;
			}
			return new InstanceDescriptor(constructorInfo, array2, flag);
		}

		// Token: 0x040020D8 RID: 8408
		private static Type[] ctorTypes;

		// Token: 0x040020D9 RID: 8409
		private static string[] ctorParamProps;
	}
}
