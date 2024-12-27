using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E5 RID: 1253
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	public sealed class OptionalAttribute : Attribute
	{
		// Token: 0x0600312E RID: 12590 RVA: 0x000A90C9 File Offset: 0x000A80C9
		internal static Attribute GetCustomAttribute(ParameterInfo parameter)
		{
			if (!parameter.IsOptional)
			{
				return null;
			}
			return new OptionalAttribute();
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x000A90DA File Offset: 0x000A80DA
		internal static bool IsDefined(ParameterInfo parameter)
		{
			return parameter.IsOptional;
		}
	}
}
