using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E3 RID: 1251
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	public sealed class InAttribute : Attribute
	{
		// Token: 0x06003128 RID: 12584 RVA: 0x000A9087 File Offset: 0x000A8087
		internal static Attribute GetCustomAttribute(ParameterInfo parameter)
		{
			if (!parameter.IsIn)
			{
				return null;
			}
			return new InAttribute();
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x000A9098 File Offset: 0x000A8098
		internal static bool IsDefined(ParameterInfo parameter)
		{
			return parameter.IsIn;
		}
	}
}
