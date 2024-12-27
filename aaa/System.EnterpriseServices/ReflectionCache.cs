using System;
using System.Reflection;

namespace System.EnterpriseServices
{
	// Token: 0x02000040 RID: 64
	internal static class ReflectionCache
	{
		// Token: 0x06000133 RID: 307 RVA: 0x000057A8 File Offset: 0x000047A8
		public static MemberInfo ConvertToInterfaceMI(MemberInfo mi)
		{
			MemberInfo memberInfo = (MemberInfo)ReflectionCache.Cache.Get(mi);
			if (memberInfo != null)
			{
				return memberInfo;
			}
			MethodInfo methodInfo = mi as MethodInfo;
			if (methodInfo == null)
			{
				return null;
			}
			MethodInfo methodInfo2 = null;
			Type reflectedType = methodInfo.ReflectedType;
			if (reflectedType.IsInterface)
			{
				methodInfo2 = methodInfo;
			}
			else
			{
				Type[] interfaces = reflectedType.GetInterfaces();
				if (interfaces == null)
				{
					return null;
				}
				for (int i = 0; i < interfaces.Length; i++)
				{
					InterfaceMapping interfaceMap = reflectedType.GetInterfaceMap(interfaces[i]);
					if (interfaceMap.TargetMethods != null)
					{
						for (int j = 0; j < interfaceMap.TargetMethods.Length; j++)
						{
							if (interfaceMap.TargetMethods[j] == methodInfo)
							{
								methodInfo2 = interfaceMap.InterfaceMethods[j];
								break;
							}
						}
						if (methodInfo2 != null)
						{
							break;
						}
					}
				}
			}
			ReflectionCache.Cache.Reset(mi, methodInfo2);
			return methodInfo2;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005868 File Offset: 0x00004868
		public static MemberInfo ConvertToClassMI(Type t, MemberInfo mi)
		{
			Type reflectedType = mi.ReflectedType;
			if (!reflectedType.IsInterface)
			{
				return mi;
			}
			Cachetable cachetable = (Cachetable)ReflectionCache.Cache.Get(t);
			if (cachetable != null)
			{
				MemberInfo memberInfo = (MemberInfo)cachetable.Get(mi);
				if (memberInfo != null)
				{
					return memberInfo;
				}
			}
			MethodInfo methodInfo = (MethodInfo)mi;
			MethodInfo methodInfo2 = null;
			InterfaceMapping interfaceMap = t.GetInterfaceMap(reflectedType);
			if (interfaceMap.TargetMethods == null)
			{
				throw new InvalidCastException();
			}
			for (int i = 0; i < interfaceMap.TargetMethods.Length; i++)
			{
				if (interfaceMap.InterfaceMethods[i] == methodInfo)
				{
					methodInfo2 = interfaceMap.TargetMethods[i];
					break;
				}
			}
			if (cachetable == null)
			{
				cachetable = (Cachetable)ReflectionCache.Cache.Set(t, new Cachetable());
			}
			cachetable.Reset(mi, methodInfo2);
			return methodInfo2;
		}

		// Token: 0x0400008C RID: 140
		private static Cachetable Cache = new Cachetable();
	}
}
