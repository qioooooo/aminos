using System;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000152 RID: 338
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class TypeListConverter : TypeConverter
	{
		// Token: 0x06000B22 RID: 2850 RVA: 0x00027B0E File Offset: 0x00026B0E
		protected TypeListConverter(Type[] types)
		{
			this.types = types;
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00027B1D File Offset: 0x00026B1D
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x00027B36 File Offset: 0x00026B36
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00027B50 File Offset: 0x00026B50
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				foreach (Type type in this.types)
				{
					if (value.Equals(type.FullName))
					{
						return type;
					}
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00027B9C File Offset: 0x00026B9C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType != typeof(string))
			{
				if (destinationType == typeof(InstanceDescriptor) && value is Type)
				{
					MethodInfo method = typeof(Type).GetMethod("GetType", new Type[] { typeof(string) });
					if (method != null)
					{
						return new InstanceDescriptor(method, new object[] { ((Type)value).AssemblyQualifiedName });
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
			if (value == null)
			{
				return SR.GetString("toStringNone");
			}
			return ((Type)value).FullName;
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00027C4C File Offset: 0x00026C4C
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.values == null)
			{
				object[] array;
				if (this.types != null)
				{
					array = new object[this.types.Length];
					Array.Copy(this.types, array, this.types.Length);
				}
				else
				{
					array = null;
				}
				this.values = new TypeConverter.StandardValuesCollection(array);
			}
			return this.values;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00027CA1 File Offset: 0x00026CA1
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00027CA4 File Offset: 0x00026CA4
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x04000A9A RID: 2714
		private Type[] types;

		// Token: 0x04000A9B RID: 2715
		private TypeConverter.StandardValuesCollection values;
	}
}
