using System;
using System.Runtime.CompilerServices;

namespace System.Management
{
	// Token: 0x0200007C RID: 124
	internal class ValueTypeSafety
	{
		// Token: 0x0600036A RID: 874 RVA: 0x0000DFF8 File Offset: 0x0000CFF8
		public static object GetSafeObject(object theValue)
		{
			if (theValue == null)
			{
				return null;
			}
			if (theValue.GetType().IsPrimitive)
			{
				return ((IConvertible)theValue).ToType(typeof(object), null);
			}
			return RuntimeHelpers.GetObjectValue(theValue);
		}
	}
}
