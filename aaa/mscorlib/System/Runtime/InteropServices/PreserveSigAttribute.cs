using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E2 RID: 1250
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	public sealed class PreserveSigAttribute : Attribute
	{
		// Token: 0x06003125 RID: 12581 RVA: 0x000A9054 File Offset: 0x000A8054
		internal static Attribute GetCustomAttribute(RuntimeMethodInfo method)
		{
			if ((method.GetMethodImplementationFlags() & MethodImplAttributes.PreserveSig) == MethodImplAttributes.IL)
			{
				return null;
			}
			return new PreserveSigAttribute();
		}

		// Token: 0x06003126 RID: 12582 RVA: 0x000A906B File Offset: 0x000A806B
		internal static bool IsDefined(RuntimeMethodInfo method)
		{
			return (method.GetMethodImplementationFlags() & MethodImplAttributes.PreserveSig) != MethodImplAttributes.IL;
		}
	}
}
