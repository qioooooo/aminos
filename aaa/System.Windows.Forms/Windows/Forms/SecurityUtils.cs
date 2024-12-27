using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x0200003E RID: 62
	internal static class SecurityUtils
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00006B10 File Offset: 0x00005B10
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

		// Token: 0x060001C5 RID: 453 RVA: 0x00006B44 File Offset: 0x00005B44
		internal static object SecureCreateInstance(Type type)
		{
			return SecurityUtils.SecureCreateInstance(type, null);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00006B50 File Offset: 0x00005B50
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

		// Token: 0x060001C7 RID: 455 RVA: 0x00006BA4 File Offset: 0x00005BA4
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

		// Token: 0x060001C8 RID: 456 RVA: 0x00006C18 File Offset: 0x00005C18
		internal static object SecureConstructorInvoke(Type type, Type[] argTypes, object[] args, bool allowNonPublic)
		{
			return SecurityUtils.SecureConstructorInvoke(type, argTypes, args, allowNonPublic, BindingFlags.Default);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00006C24 File Offset: 0x00005C24
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
