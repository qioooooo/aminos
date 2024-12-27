using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000124 RID: 292
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class NullableConverter : TypeConverter
	{
		// Token: 0x0600094C RID: 2380 RVA: 0x0001F2F0 File Offset: 0x0001E2F0
		public NullableConverter(Type type)
		{
			this.nullableType = type;
			this.simpleType = Nullable.GetUnderlyingType(type);
			if (this.simpleType == null)
			{
				throw new ArgumentException(SR.GetString("NullableConverterBadCtorArg"), "type");
			}
			this.simpleTypeConverter = TypeDescriptor.GetConverter(this.simpleType);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0001F344 File Offset: 0x0001E344
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == this.simpleType)
			{
				return true;
			}
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.CanConvertFrom(context, sourceType);
			}
			return base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0001F370 File Offset: 0x0001E370
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null || value.GetType() == this.simpleType)
			{
				return value;
			}
			if (value is string && string.IsNullOrEmpty(value as string))
			{
				return null;
			}
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.ConvertFrom(context, culture, value);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0001F3C9 File Offset: 0x0001E3C9
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == this.simpleType)
			{
				return true;
			}
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.CanConvertTo(context, destinationType);
			}
			return base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0001F404 File Offset: 0x0001E404
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == this.simpleType && this.nullableType.IsInstanceOfType(value))
			{
				return value;
			}
			if (destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo constructor = this.nullableType.GetConstructor(new Type[] { this.simpleType });
				return new InstanceDescriptor(constructor, new object[] { value }, true);
			}
			if (value == null)
			{
				if (destinationType == typeof(string))
				{
					return string.Empty;
				}
			}
			else if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.ConvertTo(context, culture, value, destinationType);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0001F4B4 File Offset: 0x0001E4B4
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.CreateInstance(context, propertyValues);
			}
			return base.CreateInstance(context, propertyValues);
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0001F4E1 File Offset: 0x0001E4E1
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.GetCreateInstanceSupported(context);
			}
			return base.GetCreateInstanceSupported(context);
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0001F500 File Offset: 0x0001E500
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.GetProperties(context, value, attributes);
			}
			return base.GetProperties(context, value, attributes);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0001F52F File Offset: 0x0001E52F
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.GetPropertiesSupported(context);
			}
			return base.GetPropertiesSupported(context);
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0001F550 File Offset: 0x0001E550
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.simpleTypeConverter != null)
			{
				TypeConverter.StandardValuesCollection standardValues = this.simpleTypeConverter.GetStandardValues(context);
				if (this.GetStandardValuesSupported(context) && standardValues != null)
				{
					object[] array = new object[standardValues.Count + 1];
					int num = 0;
					array[num++] = null;
					foreach (object obj in standardValues)
					{
						array[num++] = obj;
					}
					return new TypeConverter.StandardValuesCollection(array);
				}
			}
			return base.GetStandardValues(context);
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0001F5F0 File Offset: 0x0001E5F0
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.GetStandardValuesExclusive(context);
			}
			return base.GetStandardValuesExclusive(context);
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0001F60E File Offset: 0x0001E60E
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.GetStandardValuesSupported(context);
			}
			return base.GetStandardValuesSupported(context);
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0001F62C File Offset: 0x0001E62C
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			if (this.simpleTypeConverter != null)
			{
				return value == null || this.simpleTypeConverter.IsValid(context, value);
			}
			return base.IsValid(context, value);
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x0001F65E File Offset: 0x0001E65E
		public Type NullableType
		{
			get
			{
				return this.nullableType;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x0001F666 File Offset: 0x0001E666
		public Type UnderlyingType
		{
			get
			{
				return this.simpleType;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x0001F66E File Offset: 0x0001E66E
		public TypeConverter UnderlyingTypeConverter
		{
			get
			{
				return this.simpleTypeConverter;
			}
		}

		// Token: 0x04000A05 RID: 2565
		private Type nullableType;

		// Token: 0x04000A06 RID: 2566
		private Type simpleType;

		// Token: 0x04000A07 RID: 2567
		private TypeConverter simpleTypeConverter;
	}
}
