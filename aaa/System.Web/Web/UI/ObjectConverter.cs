using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000430 RID: 1072
	[Obsolete("The recommended alternative is System.Convert and String.Format. http://go.microsoft.com/fwlink/?linkid=14202")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ObjectConverter
	{
		// Token: 0x06003365 RID: 13157 RVA: 0x000DEFB4 File Offset: 0x000DDFB4
		public static object ConvertValue(object value, Type toType, string formatString)
		{
			if (value == null || Convert.IsDBNull(value))
			{
				return value;
			}
			Type type = value.GetType();
			if (toType.IsAssignableFrom(type))
			{
				return value;
			}
			if (typeof(string).IsAssignableFrom(type))
			{
				if (typeof(int).IsAssignableFrom(toType))
				{
					return Convert.ToInt32((string)value, CultureInfo.InvariantCulture);
				}
				if (typeof(bool).IsAssignableFrom(toType))
				{
					return Convert.ToBoolean((string)value, CultureInfo.InvariantCulture);
				}
				if (typeof(DateTime).IsAssignableFrom(toType))
				{
					return Convert.ToDateTime((string)value, CultureInfo.InvariantCulture);
				}
				if (typeof(decimal).IsAssignableFrom(toType))
				{
					TypeConverter typeConverter = new DecimalConverter();
					return typeConverter.ConvertFromInvariantString((string)value);
				}
				if (typeof(double).IsAssignableFrom(toType))
				{
					return Convert.ToDouble((string)value, CultureInfo.InvariantCulture);
				}
				if (typeof(short).IsAssignableFrom(toType))
				{
					return Convert.ToInt16((short)value, CultureInfo.InvariantCulture);
				}
				throw new ArgumentException(SR.GetString("Cannot_convert_from_to", new object[]
				{
					type.ToString(),
					toType.ToString()
				}));
			}
			else
			{
				if (!typeof(string).IsAssignableFrom(toType))
				{
					throw new ArgumentException(SR.GetString("Cannot_convert_from_to", new object[]
					{
						type.ToString(),
						toType.ToString()
					}));
				}
				if (typeof(int).IsAssignableFrom(type))
				{
					return ((int)value).ToString(formatString, CultureInfo.InvariantCulture);
				}
				if (typeof(bool).IsAssignableFrom(type))
				{
					string[] array = null;
					if (formatString != null)
					{
						array = formatString.Split(ObjectConverter.formatSeparator);
						if (array.Length != 2)
						{
							array = null;
						}
					}
					if ((bool)value)
					{
						if (array != null)
						{
							return array[0];
						}
						return "true";
					}
					else
					{
						if (array != null)
						{
							return array[1];
						}
						return "false";
					}
				}
				else
				{
					if (typeof(DateTime).IsAssignableFrom(type))
					{
						return ((DateTime)value).ToString(formatString, CultureInfo.InvariantCulture);
					}
					if (typeof(decimal).IsAssignableFrom(type))
					{
						return ((decimal)value).ToString(formatString, CultureInfo.InvariantCulture);
					}
					if (typeof(double).IsAssignableFrom(type))
					{
						return ((double)value).ToString(formatString, CultureInfo.InvariantCulture);
					}
					if (typeof(float).IsAssignableFrom(type))
					{
						return ((float)value).ToString(formatString, CultureInfo.InvariantCulture);
					}
					if (typeof(short).IsAssignableFrom(type))
					{
						return ((short)value).ToString(formatString, CultureInfo.InvariantCulture);
					}
					throw new ArgumentException(SR.GetString("Cannot_convert_from_to", new object[]
					{
						type.ToString(),
						toType.ToString()
					}));
				}
			}
		}

		// Token: 0x040023FC RID: 9212
		internal static readonly char[] formatSeparator = new char[] { ',' };
	}
}
