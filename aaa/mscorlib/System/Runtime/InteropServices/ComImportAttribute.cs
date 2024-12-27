using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E0 RID: 1248
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
	public sealed class ComImportAttribute : Attribute
	{
		// Token: 0x06003120 RID: 12576 RVA: 0x000A900A File Offset: 0x000A800A
		internal static Attribute GetCustomAttribute(RuntimeType type)
		{
			if ((type.Attributes & TypeAttributes.Import) == TypeAttributes.NotPublic)
			{
				return null;
			}
			return new ComImportAttribute();
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x000A9021 File Offset: 0x000A8021
		internal static bool IsDefined(RuntimeType type)
		{
			return (type.Attributes & TypeAttributes.Import) != TypeAttributes.NotPublic;
		}
	}
}
