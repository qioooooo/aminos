using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200026F RID: 623
	[ComVisible(true)]
	public static class Nullable
	{
		// Token: 0x0600191D RID: 6429 RVA: 0x00041EAA File Offset: 0x00040EAA
		[ComVisible(true)]
		public static int Compare<T>(T? n1, T? n2) where T : struct
		{
			if (n1 != null)
			{
				if (n2 != null)
				{
					return Comparer<T>.Default.Compare(n1.value, n2.value);
				}
				return 1;
			}
			else
			{
				if (n2 != null)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x0600191E RID: 6430 RVA: 0x00041EE5 File Offset: 0x00040EE5
		[ComVisible(true)]
		public static bool Equals<T>(T? n1, T? n2) where T : struct
		{
			if (n1 != null)
			{
				return n2 != null && EqualityComparer<T>.Default.Equals(n1.value, n2.value);
			}
			return n2 == null;
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x00041F20 File Offset: 0x00040F20
		public static Type GetUnderlyingType(Type nullableType)
		{
			if (nullableType == null)
			{
				throw new ArgumentNullException("nullableType");
			}
			Type type = null;
			if (nullableType.IsGenericType && !nullableType.IsGenericTypeDefinition)
			{
				Type genericTypeDefinition = nullableType.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(Nullable<>))
				{
					type = nullableType.GetGenericArguments()[0];
				}
			}
			return type;
		}
	}
}
