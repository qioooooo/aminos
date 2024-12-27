using System;
using System.Globalization;
using System.Threading;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000057 RID: 87
	internal class ScalarFormatter
	{
		// Token: 0x060001ED RID: 493 RVA: 0x00009ABD File Offset: 0x00008ABD
		private ScalarFormatter()
		{
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00009AC5 File Offset: 0x00008AC5
		internal static string ToString(object value)
		{
			if (value == null)
			{
				return string.Empty;
			}
			if (value is string)
			{
				return (string)value;
			}
			if (value.GetType().IsEnum)
			{
				return ScalarFormatter.EnumToString(value);
			}
			return Convert.ToString(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00009B00 File Offset: 0x00008B00
		internal static object FromString(string value, Type type)
		{
			object obj;
			try
			{
				if (type == typeof(string))
				{
					obj = value;
				}
				else if (type.IsEnum)
				{
					obj = ScalarFormatter.EnumFromString(value, type);
				}
				else
				{
					obj = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new ArgumentException(Res.GetString("WebChangeTypeFailed", new object[] { value, type.FullName }), "type", ex);
			}
			catch
			{
				throw new ArgumentException(Res.GetString("WebChangeTypeFailed", new object[] { value, type.FullName }), "type", null);
			}
			return obj;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00009BD4 File Offset: 0x00008BD4
		private static object EnumFromString(string value, Type type)
		{
			return Enum.Parse(type, value);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00009BDD File Offset: 0x00008BDD
		private static string EnumToString(object value)
		{
			return Enum.Format(value.GetType(), value, "G");
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00009BF0 File Offset: 0x00008BF0
		internal static bool IsTypeSupported(Type type)
		{
			return type.IsEnum || type == typeof(int) || type == typeof(string) || type == typeof(long) || type == typeof(byte) || type == typeof(sbyte) || type == typeof(short) || type == typeof(bool) || type == typeof(char) || type == typeof(float) || type == typeof(decimal) || type == typeof(DateTime) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong) || type == typeof(double);
		}
	}
}
