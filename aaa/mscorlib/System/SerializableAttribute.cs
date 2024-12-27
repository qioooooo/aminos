using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000111 RID: 273
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate, Inherited = false)]
	[ComVisible(true)]
	public sealed class SerializableAttribute : Attribute
	{
		// Token: 0x06000FF7 RID: 4087 RVA: 0x0002D948 File Offset: 0x0002C948
		internal static Attribute GetCustomAttribute(Type type)
		{
			if ((type.Attributes & TypeAttributes.Serializable) != TypeAttributes.Serializable)
			{
				return null;
			}
			return new SerializableAttribute();
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0002D964 File Offset: 0x0002C964
		internal static bool IsDefined(Type type)
		{
			return type.IsSerializable;
		}
	}
}
