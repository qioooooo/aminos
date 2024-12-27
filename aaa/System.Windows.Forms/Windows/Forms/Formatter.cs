using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x0200040C RID: 1036
	internal class Formatter
	{
		// Token: 0x06003E33 RID: 15923 RVA: 0x000E2A00 File Offset: 0x000E1A00
		public static object FormatObject(object value, Type targetType, TypeConverter sourceConverter, TypeConverter targetConverter, string formatString, IFormatProvider formatInfo, object formattedNullValue, object dataSourceNullValue)
		{
			if (Formatter.IsNullData(value, dataSourceNullValue))
			{
				value = DBNull.Value;
			}
			Type type = targetType;
			targetType = Formatter.NullableUnwrap(targetType);
			sourceConverter = Formatter.NullableUnwrap(sourceConverter);
			targetConverter = Formatter.NullableUnwrap(targetConverter);
			bool flag = targetType != type;
			object obj = Formatter.FormatObjectInternal(value, targetType, sourceConverter, targetConverter, formatString, formatInfo, formattedNullValue);
			if (type.IsValueType && obj == null && !flag)
			{
				throw new FormatException(Formatter.GetCantConvertMessage(value, targetType));
			}
			return obj;
		}

		// Token: 0x06003E34 RID: 15924 RVA: 0x000E2A6C File Offset: 0x000E1A6C
		private static object FormatObjectInternal(object value, Type targetType, TypeConverter sourceConverter, TypeConverter targetConverter, string formatString, IFormatProvider formatInfo, object formattedNullValue)
		{
			if (value == DBNull.Value || value == null)
			{
				if (formattedNullValue != null)
				{
					return formattedNullValue;
				}
				if (targetType == Formatter.stringType)
				{
					return string.Empty;
				}
				if (targetType == Formatter.checkStateType)
				{
					return CheckState.Indeterminate;
				}
				return null;
			}
			else
			{
				if (targetType == Formatter.stringType && value is IFormattable && !string.IsNullOrEmpty(formatString))
				{
					return (value as IFormattable).ToString(formatString, formatInfo);
				}
				Type type = value.GetType();
				TypeConverter converter = TypeDescriptor.GetConverter(type);
				if (sourceConverter != null && sourceConverter != converter && sourceConverter.CanConvertTo(targetType))
				{
					return sourceConverter.ConvertTo(null, Formatter.GetFormatterCulture(formatInfo), value, targetType);
				}
				TypeConverter converter2 = TypeDescriptor.GetConverter(targetType);
				if (targetConverter != null && targetConverter != converter2 && targetConverter.CanConvertFrom(type))
				{
					return targetConverter.ConvertFrom(null, Formatter.GetFormatterCulture(formatInfo), value);
				}
				if (targetType == Formatter.checkStateType)
				{
					if (type == Formatter.booleanType)
					{
						return ((bool)value) ? CheckState.Checked : CheckState.Unchecked;
					}
					if (sourceConverter == null)
					{
						sourceConverter = converter;
					}
					if (sourceConverter != null && sourceConverter.CanConvertTo(Formatter.booleanType))
					{
						return ((bool)sourceConverter.ConvertTo(null, Formatter.GetFormatterCulture(formatInfo), value, Formatter.booleanType)) ? CheckState.Checked : CheckState.Unchecked;
					}
				}
				if (targetType.IsAssignableFrom(type))
				{
					return value;
				}
				if (sourceConverter == null)
				{
					sourceConverter = converter;
				}
				if (targetConverter == null)
				{
					targetConverter = converter2;
				}
				if (sourceConverter != null && sourceConverter.CanConvertTo(targetType))
				{
					return sourceConverter.ConvertTo(null, Formatter.GetFormatterCulture(formatInfo), value, targetType);
				}
				if (targetConverter != null && targetConverter.CanConvertFrom(type))
				{
					return targetConverter.ConvertFrom(null, Formatter.GetFormatterCulture(formatInfo), value);
				}
				if (value is IConvertible)
				{
					return Formatter.ChangeType(value, targetType, formatInfo);
				}
				throw new FormatException(Formatter.GetCantConvertMessage(value, targetType));
			}
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x000E2BF4 File Offset: 0x000E1BF4
		public static object ParseObject(object value, Type targetType, Type sourceType, TypeConverter targetConverter, TypeConverter sourceConverter, IFormatProvider formatInfo, object formattedNullValue, object dataSourceNullValue)
		{
			Type type = targetType;
			sourceType = Formatter.NullableUnwrap(sourceType);
			targetType = Formatter.NullableUnwrap(targetType);
			sourceConverter = Formatter.NullableUnwrap(sourceConverter);
			targetConverter = Formatter.NullableUnwrap(targetConverter);
			object obj = Formatter.ParseObjectInternal(value, targetType, sourceType, targetConverter, sourceConverter, formatInfo, formattedNullValue);
			if (obj == DBNull.Value)
			{
				return Formatter.NullData(type, dataSourceNullValue);
			}
			return obj;
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x000E2C48 File Offset: 0x000E1C48
		private static object ParseObjectInternal(object value, Type targetType, Type sourceType, TypeConverter targetConverter, TypeConverter sourceConverter, IFormatProvider formatInfo, object formattedNullValue)
		{
			if (Formatter.EqualsFormattedNullValue(value, formattedNullValue, formatInfo) || value == DBNull.Value)
			{
				return DBNull.Value;
			}
			TypeConverter converter = TypeDescriptor.GetConverter(targetType);
			if (targetConverter != null && converter != targetConverter && targetConverter.CanConvertFrom(sourceType))
			{
				return targetConverter.ConvertFrom(null, Formatter.GetFormatterCulture(formatInfo), value);
			}
			TypeConverter converter2 = TypeDescriptor.GetConverter(sourceType);
			if (sourceConverter != null && converter2 != sourceConverter && sourceConverter.CanConvertTo(targetType))
			{
				return sourceConverter.ConvertTo(null, Formatter.GetFormatterCulture(formatInfo), value, targetType);
			}
			if (value is string)
			{
				object obj = Formatter.InvokeStringParseMethod(value, targetType, formatInfo);
				if (obj != Formatter.parseMethodNotFound)
				{
					return obj;
				}
			}
			else if (value is CheckState)
			{
				CheckState checkState = (CheckState)value;
				if (checkState == CheckState.Indeterminate)
				{
					return DBNull.Value;
				}
				if (targetType == Formatter.booleanType)
				{
					return checkState == CheckState.Checked;
				}
				if (targetConverter == null)
				{
					targetConverter = converter;
				}
				if (targetConverter != null && targetConverter.CanConvertFrom(Formatter.booleanType))
				{
					return targetConverter.ConvertFrom(null, Formatter.GetFormatterCulture(formatInfo), checkState == CheckState.Checked);
				}
			}
			else if (value != null && targetType.IsAssignableFrom(value.GetType()))
			{
				return value;
			}
			if (targetConverter == null)
			{
				targetConverter = converter;
			}
			if (sourceConverter == null)
			{
				sourceConverter = converter2;
			}
			if (targetConverter != null && targetConverter.CanConvertFrom(sourceType))
			{
				return targetConverter.ConvertFrom(null, Formatter.GetFormatterCulture(formatInfo), value);
			}
			if (sourceConverter != null && sourceConverter.CanConvertTo(targetType))
			{
				return sourceConverter.ConvertTo(null, Formatter.GetFormatterCulture(formatInfo), value, targetType);
			}
			if (value is IConvertible)
			{
				return Formatter.ChangeType(value, targetType, formatInfo);
			}
			throw new FormatException(Formatter.GetCantConvertMessage(value, targetType));
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x000E2DB4 File Offset: 0x000E1DB4
		private static object ChangeType(object value, Type type, IFormatProvider formatInfo)
		{
			object obj;
			try
			{
				if (formatInfo == null)
				{
					formatInfo = CultureInfo.CurrentCulture;
				}
				obj = Convert.ChangeType(value, type, formatInfo);
			}
			catch (InvalidCastException ex)
			{
				throw new FormatException(ex.Message, ex);
			}
			return obj;
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x000E2DF8 File Offset: 0x000E1DF8
		private static bool EqualsFormattedNullValue(object value, object formattedNullValue, IFormatProvider formatInfo)
		{
			string text = formattedNullValue as string;
			string text2 = value as string;
			if (text != null && text2 != null)
			{
				return text.Length == text2.Length && string.Compare(text2, text, true, Formatter.GetFormatterCulture(formatInfo)) == 0;
			}
			return object.Equals(value, formattedNullValue);
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x000E2E44 File Offset: 0x000E1E44
		private static string GetCantConvertMessage(object value, Type targetType)
		{
			string text = ((value == null) ? "Formatter_CantConvertNull" : "Formatter_CantConvert");
			return string.Format(CultureInfo.CurrentCulture, SR.GetString(text), new object[] { value, targetType.Name });
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x000E2E86 File Offset: 0x000E1E86
		private static CultureInfo GetFormatterCulture(IFormatProvider formatInfo)
		{
			if (formatInfo is CultureInfo)
			{
				return formatInfo as CultureInfo;
			}
			return CultureInfo.CurrentCulture;
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x000E2E9C File Offset: 0x000E1E9C
		public static object InvokeStringParseMethod(object value, Type targetType, IFormatProvider formatInfo)
		{
			object obj;
			try
			{
				MethodInfo methodInfo = targetType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[]
				{
					Formatter.stringType,
					typeof(NumberStyles),
					typeof(IFormatProvider)
				}, null);
				if (methodInfo != null)
				{
					obj = methodInfo.Invoke(null, new object[]
					{
						(string)value,
						NumberStyles.Any,
						formatInfo
					});
				}
				else
				{
					methodInfo = targetType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[]
					{
						Formatter.stringType,
						typeof(IFormatProvider)
					}, null);
					if (methodInfo != null)
					{
						obj = methodInfo.Invoke(null, new object[]
						{
							(string)value,
							formatInfo
						});
					}
					else
					{
						methodInfo = targetType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[] { Formatter.stringType }, null);
						if (methodInfo != null)
						{
							obj = methodInfo.Invoke(null, new object[] { (string)value });
						}
						else
						{
							obj = Formatter.parseMethodNotFound;
						}
					}
				}
			}
			catch (TargetInvocationException ex)
			{
				throw new FormatException(ex.InnerException.Message, ex.InnerException);
			}
			return obj;
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x000E2FF4 File Offset: 0x000E1FF4
		public static bool IsNullData(object value, object dataSourceNullValue)
		{
			return value == null || value == DBNull.Value || object.Equals(value, Formatter.NullData(value.GetType(), dataSourceNullValue));
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x000E3015 File Offset: 0x000E2015
		public static object NullData(Type type, object dataSourceNullValue)
		{
			if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Nullable<>))
			{
				return dataSourceNullValue;
			}
			if (dataSourceNullValue == null || dataSourceNullValue == DBNull.Value)
			{
				return null;
			}
			return dataSourceNullValue;
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x000E3044 File Offset: 0x000E2044
		private static Type NullableUnwrap(Type type)
		{
			if (type == Formatter.stringType)
			{
				return Formatter.stringType;
			}
			Type underlyingType = Nullable.GetUnderlyingType(type);
			return underlyingType ?? type;
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x000E306C File Offset: 0x000E206C
		private static TypeConverter NullableUnwrap(TypeConverter typeConverter)
		{
			NullableConverter nullableConverter = typeConverter as NullableConverter;
			if (nullableConverter == null)
			{
				return typeConverter;
			}
			return nullableConverter.UnderlyingTypeConverter;
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x000E308B File Offset: 0x000E208B
		public static object GetDefaultDataSourceNullValue(Type type)
		{
			if (type == null || type.IsValueType)
			{
				return Formatter.defaultDataSourceNullValue;
			}
			return null;
		}

		// Token: 0x04001EA3 RID: 7843
		private static Type stringType = typeof(string);

		// Token: 0x04001EA4 RID: 7844
		private static Type booleanType = typeof(bool);

		// Token: 0x04001EA5 RID: 7845
		private static Type checkStateType = typeof(CheckState);

		// Token: 0x04001EA6 RID: 7846
		private static object parseMethodNotFound = new object();

		// Token: 0x04001EA7 RID: 7847
		private static object defaultDataSourceNullValue = DBNull.Value;
	}
}
