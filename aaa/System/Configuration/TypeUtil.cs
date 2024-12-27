using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x0200071D RID: 1821
	internal static class TypeUtil
	{
		// Token: 0x060037AB RID: 14251 RVA: 0x000EBE20 File Offset: 0x000EAE20
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.TypeInformation | ReflectionPermissionFlag.MemberAccess)]
		internal static object CreateInstanceWithReflectionPermission(string typeString)
		{
			Type type = Type.GetType(typeString, true);
			return Activator.CreateInstance(type, true);
		}
	}
}
