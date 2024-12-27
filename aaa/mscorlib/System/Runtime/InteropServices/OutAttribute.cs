using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E4 RID: 1252
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	[ComVisible(true)]
	public sealed class OutAttribute : Attribute
	{
		// Token: 0x0600312B RID: 12587 RVA: 0x000A90A8 File Offset: 0x000A80A8
		internal static Attribute GetCustomAttribute(ParameterInfo parameter)
		{
			if (!parameter.IsOut)
			{
				return null;
			}
			return new OutAttribute();
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x000A90B9 File Offset: 0x000A80B9
		internal static bool IsDefined(ParameterInfo parameter)
		{
			return parameter.IsOut;
		}
	}
}
