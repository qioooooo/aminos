using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace System.Drawing
{
	// Token: 0x02000016 RID: 22
	internal static class SecurityUtils
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000064 RID: 100 RVA: 0x0000333C File Offset: 0x0000233C
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

		// Token: 0x06000065 RID: 101 RVA: 0x00003370 File Offset: 0x00002370
		internal static object SecureCreateInstance(Type type)
		{
			return SecurityUtils.SecureCreateInstance(type, null);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000337C File Offset: 0x0000237C
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

		// Token: 0x06000067 RID: 103 RVA: 0x000033D0 File Offset: 0x000023D0
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

		// Token: 0x06000068 RID: 104 RVA: 0x00003444 File Offset: 0x00002444
		internal static object SecureConstructorInvoke(Type type, Type[] argTypes, object[] args, bool allowNonPublic)
		{
			return SecurityUtils.SecureConstructorInvoke(type, argTypes, args, allowNonPublic, BindingFlags.Default);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003450 File Offset: 0x00002450
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
