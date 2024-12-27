using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace System
{
	// Token: 0x020007A0 RID: 1952
	internal static class SecurityUtils
	{
		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06003C13 RID: 15379 RVA: 0x00100E30 File Offset: 0x000FFE30
		private static bool HasReflectionPermission
		{
			get
			{
				try
				{
					new ReflectionPermission(PermissionState.Unrestricted).Demand();
					return true;
				}
				catch (SecurityException)
				{
				}
				return false;
			}
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x00100E64 File Offset: 0x000FFE64
		internal static object SecureCreateInstance(Type type)
		{
			return SecurityUtils.SecureCreateInstance(type, null);
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x00100E70 File Offset: 0x000FFE70
		internal static object SecureCreateInstance(Type type, object[] args)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type.Assembly == typeof(SecurityUtils).Assembly && !type.IsPublic && !type.IsNestedPublic)
			{
				new ReflectionPermission(PermissionState.Unrestricted).Demand();
			}
			return Activator.CreateInstance(type, args);
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x00100EC4 File Offset: 0x000FFEC4
		internal static object SecureCreateInstance(Type type, object[] args, bool allowNonPublic)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
			if (type.Assembly == typeof(SecurityUtils).Assembly)
			{
				if (!type.IsPublic && !type.IsNestedPublic)
				{
					new ReflectionPermission(PermissionState.Unrestricted).Demand();
				}
				else if (allowNonPublic && !SecurityUtils.HasReflectionPermission)
				{
					allowNonPublic = false;
				}
			}
			if (allowNonPublic)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			return Activator.CreateInstance(type, bindingFlags, null, args, null);
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x00100F38 File Offset: 0x000FFF38
		internal static object SecureConstructorInvoke(Type type, Type[] argTypes, object[] args, bool allowNonPublic)
		{
			return SecurityUtils.SecureConstructorInvoke(type, argTypes, args, allowNonPublic, BindingFlags.Default);
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x00100F44 File Offset: 0x000FFF44
		internal static object SecureConstructorInvoke(Type type, Type[] argTypes, object[] args, bool allowNonPublic, BindingFlags extraFlags)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | extraFlags;
			if (type.Assembly == typeof(SecurityUtils).Assembly)
			{
				if (!type.IsPublic && !type.IsNestedPublic)
				{
					new ReflectionPermission(PermissionState.Unrestricted).Demand();
				}
				else if (allowNonPublic && !SecurityUtils.HasReflectionPermission)
				{
					allowNonPublic = false;
				}
			}
			if (allowNonPublic)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			ConstructorInfo constructor = type.GetConstructor(bindingFlags, null, argTypes, null);
			if (constructor != null)
			{
				return constructor.Invoke(args);
			}
			return null;
		}
	}
}
