using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System
{
	// Token: 0x020000E2 RID: 226
	[Serializable]
	internal class OleAutBinder : DefaultBinder
	{
		// Token: 0x06000C67 RID: 3175 RVA: 0x000253B4 File Offset: 0x000243B4
		public override object ChangeType(object value, Type type, CultureInfo cultureInfo)
		{
			Variant variant = new Variant(value);
			if (cultureInfo == null)
			{
				cultureInfo = CultureInfo.CurrentCulture;
			}
			if (type.IsByRef)
			{
				type = type.GetElementType();
			}
			if (!type.IsPrimitive && type.IsInstanceOfType(value))
			{
				return value;
			}
			Type type2 = value.GetType();
			if (type.IsEnum && type2.IsPrimitive)
			{
				return Enum.Parse(type, value.ToString());
			}
			if (type2 == typeof(DBNull))
			{
				if (type == typeof(DBNull))
				{
					return value;
				}
				if ((type.IsClass && type != typeof(object)) || type.IsInterface)
				{
					return null;
				}
			}
			object obj2;
			try
			{
				object obj = OAVariantLib.ChangeType(variant, type, 16, cultureInfo).ToObject();
				obj2 = obj;
			}
			catch (NotSupportedException)
			{
				throw new COMException(Environment.GetResourceString("Interop.COM_TypeMismatch"), -2147352571);
			}
			return obj2;
		}
	}
}
