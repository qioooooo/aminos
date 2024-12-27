using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000D9 RID: 217
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	[ComVisible(true)]
	public sealed class NonSerializedAttribute : Attribute
	{
		// Token: 0x06000C19 RID: 3097 RVA: 0x000240F6 File Offset: 0x000230F6
		internal static Attribute GetCustomAttribute(RuntimeFieldInfo field)
		{
			if ((field.Attributes & FieldAttributes.NotSerialized) == FieldAttributes.PrivateScope)
			{
				return null;
			}
			return new NonSerializedAttribute();
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0002410D File Offset: 0x0002310D
		internal static bool IsDefined(RuntimeFieldInfo field)
		{
			return (field.Attributes & FieldAttributes.NotSerialized) != FieldAttributes.PrivateScope;
		}
	}
}
