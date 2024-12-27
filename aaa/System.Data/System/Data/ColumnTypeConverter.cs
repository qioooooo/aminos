using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data.SqlTypes;
using System.Globalization;
using System.Reflection;

namespace System.Data
{
	// Token: 0x0200005C RID: 92
	internal sealed class ColumnTypeConverter : TypeConverter
	{
		// Token: 0x0600046B RID: 1131 RVA: 0x001D4A20 File Offset: 0x001D3E20
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x001D4A44 File Offset: 0x001D3E44
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string))
			{
				if (value == null)
				{
					return string.Empty;
				}
				value.ToString();
			}
			if (value != null && destinationType == typeof(InstanceDescriptor))
			{
				object obj = value;
				if (value is string)
				{
					for (int i = 0; i < ColumnTypeConverter.types.Length; i++)
					{
						if (ColumnTypeConverter.types[i].ToString().Equals(value))
						{
							obj = ColumnTypeConverter.types[i];
						}
					}
				}
				if (value is Type || value is string)
				{
					MethodInfo method = typeof(Type).GetMethod("GetType", new Type[] { typeof(string) });
					if (method != null)
					{
						return new InstanceDescriptor(method, new object[] { ((Type)obj).AssemblyQualifiedName });
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x001D4B38 File Offset: 0x001D3F38
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertTo(context, sourceType);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x001D4B5C File Offset: 0x001D3F5C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value != null && value.GetType() == typeof(string))
			{
				for (int i = 0; i < ColumnTypeConverter.types.Length; i++)
				{
					if (ColumnTypeConverter.types[i].ToString().Equals(value))
					{
						return ColumnTypeConverter.types[i];
					}
				}
				return typeof(string);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x001D4BC0 File Offset: 0x001D3FC0
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.values == null)
			{
				object[] array;
				if (ColumnTypeConverter.types != null)
				{
					array = new object[ColumnTypeConverter.types.Length];
					Array.Copy(ColumnTypeConverter.types, array, ColumnTypeConverter.types.Length);
				}
				else
				{
					array = null;
				}
				this.values = new TypeConverter.StandardValuesCollection(array);
			}
			return this.values;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x001D4C14 File Offset: 0x001D4014
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x001D4C24 File Offset: 0x001D4024
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x040006AA RID: 1706
		private static Type[] types = new Type[]
		{
			typeof(bool),
			typeof(byte),
			typeof(byte[]),
			typeof(char),
			typeof(DateTime),
			typeof(decimal),
			typeof(double),
			typeof(Guid),
			typeof(short),
			typeof(int),
			typeof(long),
			typeof(object),
			typeof(sbyte),
			typeof(float),
			typeof(string),
			typeof(TimeSpan),
			typeof(ushort),
			typeof(uint),
			typeof(ulong),
			typeof(SqlInt16),
			typeof(SqlInt32),
			typeof(SqlInt64),
			typeof(SqlDecimal),
			typeof(SqlSingle),
			typeof(SqlDouble),
			typeof(SqlString),
			typeof(SqlBoolean),
			typeof(SqlBinary),
			typeof(SqlByte),
			typeof(SqlDateTime),
			typeof(SqlGuid),
			typeof(SqlMoney),
			typeof(SqlBytes),
			typeof(SqlChars),
			typeof(SqlXml)
		};

		// Token: 0x040006AB RID: 1707
		private TypeConverter.StandardValuesCollection values;
	}
}
