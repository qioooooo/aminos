using System;
using System.Configuration.Internal;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Security.Permissions;
using System.Web;

namespace System.Configuration
{
	// Token: 0x020000AB RID: 171
	internal static class TypeUtil
	{
		// Token: 0x0600066C RID: 1644 RVA: 0x0001D424 File Offset: 0x0001C424
		private static Type GetLegacyType(string typeString)
		{
			Type type = null;
			try
			{
				Assembly assembly = typeof(ConfigurationException).Assembly;
				type = assembly.GetType(typeString, false);
			}
			catch
			{
			}
			return type;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0001D464 File Offset: 0x0001C464
		private static Type GetTypeImpl(string typeString, bool throwOnError)
		{
			Type type = null;
			Exception ex = null;
			try
			{
				type = Type.GetType(typeString, throwOnError);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			if (type == null)
			{
				type = TypeUtil.GetLegacyType(typeString);
				if (type == null && ex != null)
				{
					throw ex;
				}
			}
			return type;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0001D4A8 File Offset: 0x0001C4A8
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.TypeInformation)]
		internal static Type GetTypeWithReflectionPermission(IInternalConfigHost host, string typeString, bool throwOnError)
		{
			Type type = null;
			Exception ex = null;
			try
			{
				type = host.GetConfigType(typeString, throwOnError);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			if (type == null)
			{
				type = TypeUtil.GetLegacyType(typeString);
				if (type == null && ex != null)
				{
					throw ex;
				}
			}
			return type;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0001D4EC File Offset: 0x0001C4EC
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.TypeInformation)]
		internal static Type GetTypeWithReflectionPermission(string typeString, bool throwOnError)
		{
			return TypeUtil.GetTypeImpl(typeString, throwOnError);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0001D4F5 File Offset: 0x0001C4F5
		internal static T CreateInstance<T>(string typeString)
		{
			return TypeUtil.CreateInstanceRestricted<T>(null, typeString);
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0001D500 File Offset: 0x0001C500
		internal static T CreateInstanceRestricted<T>(Type callingType, string typeString)
		{
			Type typeImpl = TypeUtil.GetTypeImpl(typeString, true);
			TypeUtil.VerifyAssignableType(typeof(T), typeImpl, true);
			return (T)((object)TypeUtil.CreateInstanceRestricted(callingType, typeImpl));
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0001D534 File Offset: 0x0001C534
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.TypeInformation | ReflectionPermissionFlag.MemberAccess)]
		internal static object CreateInstanceWithReflectionPermission(Type type)
		{
			return Activator.CreateInstance(type, true);
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001D54C File Offset: 0x0001C54C
		internal static object CreateInstanceRestricted(Type callingType, Type targetType)
		{
			if (TypeUtil.CallerHasMemberAccessOrAspNetPermission())
			{
				return TypeUtil.CreateInstanceWithReflectionPermission(targetType);
			}
			DynamicMethod dynamicMethod = TypeUtil.CreateDynamicMethod(callingType, typeof(object), new Type[] { typeof(Type) });
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldc_I4_1);
			ilgenerator.Emit(OpCodes.Call, typeof(Activator).GetMethod("CreateInstance", new Type[]
			{
				typeof(Type),
				typeof(bool)
			}));
			ilgenerator.Emit(OpCodes.Ret);
			TypeUtil.CreateInstanceInvoker createInstanceInvoker = (TypeUtil.CreateInstanceInvoker)dynamicMethod.CreateDelegate(typeof(TypeUtil.CreateInstanceInvoker));
			return createInstanceInvoker(targetType);
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001D618 File Offset: 0x0001C618
		internal static Delegate CreateDelegateRestricted(Type callingType, Type delegateType, MethodInfo targetMethod)
		{
			if (TypeUtil.CallerHasMemberAccessOrAspNetPermission())
			{
				return Delegate.CreateDelegate(delegateType, targetMethod);
			}
			DynamicMethod dynamicMethod = TypeUtil.CreateDynamicMethod(callingType, typeof(Delegate), new Type[]
			{
				typeof(Type),
				typeof(MethodInfo)
			});
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Call, typeof(Delegate).GetMethod("CreateDelegate", new Type[]
			{
				typeof(Type),
				typeof(MethodInfo)
			}));
			ilgenerator.Emit(OpCodes.Ret);
			TypeUtil.CreateDelegateInvoker createDelegateInvoker = (TypeUtil.CreateDelegateInvoker)dynamicMethod.CreateDelegate(typeof(TypeUtil.CreateDelegateInvoker));
			return createDelegateInvoker(delegateType, targetMethod);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001D6F2 File Offset: 0x0001C6F2
		private static DynamicMethod CreateDynamicMethod(Type owner, Type returnType, Type[] parameterTypes)
		{
			if (owner != null)
			{
				return TypeUtil.CreateDynamicMethodWithUnrestrictedPermission(owner, returnType, parameterTypes);
			}
			return new DynamicMethod("temp-dynamic-method", returnType, parameterTypes);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001D70C File Offset: 0x0001C70C
		[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
		private static DynamicMethod CreateDynamicMethodWithUnrestrictedPermission(Type owner, Type returnType, Type[] parameterTypes)
		{
			return new DynamicMethod("temp-dynamic-method", returnType, parameterTypes, owner);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001D71C File Offset: 0x0001C71C
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.TypeInformation)]
		internal static ConstructorInfo GetConstructorWithReflectionPermission(Type type, Type baseType, bool throwOnError)
		{
			type = TypeUtil.VerifyAssignableType(baseType, type, throwOnError);
			if (type == null)
			{
				return null;
			}
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			ConstructorInfo constructor = type.GetConstructor(bindingFlags, null, CallingConventions.HasThis, Type.EmptyTypes, null);
			if (constructor == null && throwOnError)
			{
				throw new TypeLoadException(SR.GetString("TypeNotPublic", new object[] { type.AssemblyQualifiedName }));
			}
			return constructor;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001D774 File Offset: 0x0001C774
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.TypeInformation | ReflectionPermissionFlag.MemberAccess)]
		internal static object InvokeCtorWithReflectionPermission(ConstructorInfo ctor)
		{
			return ctor.Invoke(null);
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001D780 File Offset: 0x0001C780
		internal static bool IsTypeFromTrustedAssemblyWithoutAptca(Type type)
		{
			Assembly assembly = type.Assembly;
			return assembly.GlobalAssemblyCache && !TypeUtil.HasAptcaBit(assembly);
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001D7A8 File Offset: 0x0001C7A8
		internal static Type VerifyAssignableType(Type baseType, Type type, bool throwOnError)
		{
			if (baseType.IsAssignableFrom(type))
			{
				return type;
			}
			if (throwOnError)
			{
				throw new TypeLoadException(SR.GetString("Config_type_doesnt_inherit_from_type", new object[] { type.FullName, baseType.FullName }));
			}
			return null;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001D7F0 File Offset: 0x0001C7F0
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.TypeInformation | ReflectionPermissionFlag.MemberAccess)]
		private static bool HasAptcaBit(Assembly assembly)
		{
			object[] customAttributes = assembly.GetCustomAttributes(typeof(AllowPartiallyTrustedCallersAttribute), false);
			return customAttributes != null && customAttributes.Length > 0;
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x0001D81C File Offset: 0x0001C81C
		internal static bool IsCallerFullTrust
		{
			get
			{
				bool flag = false;
				try
				{
					if (TypeUtil.s_fullTrustPermissionSet == null)
					{
						TypeUtil.s_fullTrustPermissionSet = new PermissionSet(PermissionState.Unrestricted);
					}
					TypeUtil.s_fullTrustPermissionSet.Demand();
					flag = true;
				}
				catch
				{
				}
				return flag;
			}
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001D860 File Offset: 0x0001C860
		private static bool CallerHasMemberAccessOrAspNetPermission()
		{
			try
			{
				TypeUtil.s_memberAccessPermission.Demand();
				return true;
			}
			catch (SecurityException)
			{
			}
			try
			{
				TypeUtil.s_aspNetHostingPermission.Demand();
				return true;
			}
			catch (SecurityException)
			{
			}
			return false;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0001D8B0 File Offset: 0x0001C8B0
		internal static bool IsTypeAllowedInConfig(Type t)
		{
			if (TypeUtil.IsCallerFullTrust)
			{
				return true;
			}
			Assembly assembly = t.Assembly;
			return !assembly.GlobalAssemblyCache || TypeUtil.HasAptcaBit(assembly);
		}

		// Token: 0x040003FA RID: 1018
		private static PermissionSet s_fullTrustPermissionSet;

		// Token: 0x040003FB RID: 1019
		private static readonly ReflectionPermission s_memberAccessPermission = new ReflectionPermission(ReflectionPermissionFlag.MemberAccess);

		// Token: 0x040003FC RID: 1020
		private static readonly AspNetHostingPermission s_aspNetHostingPermission = new AspNetHostingPermission(AspNetHostingPermissionLevel.Minimal);

		// Token: 0x020000AC RID: 172
		// (Invoke) Token: 0x06000681 RID: 1665
		private delegate object CreateInstanceInvoker(Type type);

		// Token: 0x020000AD RID: 173
		// (Invoke) Token: 0x06000685 RID: 1669
		private delegate Delegate CreateDelegateInvoker(Type type, MethodInfo method);
	}
}
