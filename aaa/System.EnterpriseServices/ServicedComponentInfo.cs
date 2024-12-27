using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200007C RID: 124
	internal static class ServicedComponentInfo
	{
		// Token: 0x060002BC RID: 700 RVA: 0x000075D3 File Offset: 0x000065D3
		static ServicedComponentInfo()
		{
			ServicedComponentInfo.AddExecuteMethodValidTypes();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000075F8 File Offset: 0x000065F8
		private static bool IsTypeServicedComponent2(Type t)
		{
			return t.IsSubclassOf(typeof(ServicedComponent));
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000760C File Offset: 0x0000660C
		private static bool IsTypeJITActivated2(Type t)
		{
			object[] customAttributes = t.GetCustomAttributes(true);
			object[] array = customAttributes;
			int i = 0;
			while (i < array.Length)
			{
				object obj = array[i];
				if (!(obj is JustInTimeActivationAttribute))
				{
					if (obj is TransactionAttribute)
					{
						int value = (int)((TransactionAttribute)obj).Value;
						if (value >= 2)
						{
							return true;
						}
					}
					i++;
					continue;
				}
				return ((JustInTimeActivationAttribute)obj).Value;
			}
			return false;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00007674 File Offset: 0x00006674
		private static bool IsTypeEventSource2(Type t)
		{
			object[] customAttributes = t.GetCustomAttributes(true);
			foreach (object obj in customAttributes)
			{
				if (obj is EventClassAttribute)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000076B1 File Offset: 0x000066B1
		public static bool IsTypeEventSource(Type t)
		{
			return (ServicedComponentInfo.SCICachedLookup(t) & 4) != 0;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x000076C1 File Offset: 0x000066C1
		public static bool IsTypeJITActivated(Type t)
		{
			return (ServicedComponentInfo.SCICachedLookup(t) & 8) != 0;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x000076D1 File Offset: 0x000066D1
		public static bool IsTypeServicedComponent(Type t)
		{
			return (ServicedComponentInfo.SCICachedLookup(t) & 2) != 0;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x000076E1 File Offset: 0x000066E1
		public static bool IsTypeObjectPooled(Type t)
		{
			return (ServicedComponentInfo.SCICachedLookup(t) & 16) != 0;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000076F2 File Offset: 0x000066F2
		internal static bool AreMethodsSecure(Type t)
		{
			return (ServicedComponentInfo.SCICachedLookup(t) & 32) != 0;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00007704 File Offset: 0x00006704
		internal static int SCICachedLookup(Type t)
		{
			object obj = ServicedComponentInfo._SCICache.Get(t);
			if (obj != null)
			{
				return (int)obj;
			}
			int num = 0;
			if (ServicedComponentInfo.IsTypeServicedComponent2(t))
			{
				num |= 2;
				if (ServicedComponentInfo.IsTypeEventSource2(t))
				{
					num |= 4;
				}
				if (ServicedComponentInfo.IsTypeJITActivated2(t))
				{
					num |= 8;
				}
				if (ServicedComponentInfo.IsTypeObjectPooled2(t))
				{
					num |= 16;
				}
			}
			if (ServicedComponentInfo.AreMethodsSecure2(t))
			{
				num |= 32;
			}
			if (ServicedComponentInfo.HasClassInterface2(t))
			{
				num |= 64;
			}
			ServicedComponentInfo._SCICache.Put(t, num);
			return num;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00007788 File Offset: 0x00006788
		private static bool IsTypeObjectPooled2(Type t)
		{
			object[] customAttributes = t.GetCustomAttributes(typeof(ObjectPoolingAttribute), true);
			return customAttributes != null && customAttributes.Length > 0 && ((ObjectPoolingAttribute)customAttributes[0]).Enabled;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x000077BF File Offset: 0x000067BF
		public static bool IsMethodAutoDone(MemberInfo m)
		{
			return (ServicedComponentInfo.MICachedLookup(m) & 2) != 0;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x000077CF File Offset: 0x000067CF
		public static bool HasSpecialMethodAttributes(MemberInfo m)
		{
			return (ServicedComponentInfo.MICachedLookup(m) & 4) != 0;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x000077E0 File Offset: 0x000067E0
		internal static int MICachedLookup(MemberInfo m)
		{
			object obj = ServicedComponentInfo._MICache.Get(m);
			if (obj != null)
			{
				return (int)obj;
			}
			int num = 0;
			if (ServicedComponentInfo.IsMethodAutoDone2(m))
			{
				num |= 2;
			}
			if (ServicedComponentInfo.HasSpecialMethodAttributes2(m))
			{
				num |= 4;
			}
			if (ServicedComponentInfo.IsExecuteMessageValid2(m))
			{
				num |= 8;
			}
			ServicedComponentInfo._MICache.Put(m, num);
			return num;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000783C File Offset: 0x0000683C
		private static bool IsExecuteMessageValid2(MemberInfo m)
		{
			if (ReflectionCache.ConvertToInterfaceMI(m) == null)
			{
				return false;
			}
			MethodInfo methodInfo = m as MethodInfo;
			if (methodInfo == null)
			{
				return false;
			}
			foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
			{
				if (!ServicedComponentInfo.IsTypeExecuteMethodValid(parameterInfo.ParameterType))
				{
					return false;
				}
			}
			return ServicedComponentInfo.IsTypeExecuteMethodValid(methodInfo.ReturnType);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x000078A8 File Offset: 0x000068A8
		private static bool IsTypeExecuteMethodValid(Type t)
		{
			if (t.IsEnum)
			{
				return true;
			}
			Type elementType = t.GetElementType();
			if (elementType != null && (t.IsByRef || t.IsArray))
			{
				if (ServicedComponentInfo._ExecuteMessageCache[elementType] == null)
				{
					return false;
				}
			}
			else if (ServicedComponentInfo._ExecuteMessageCache[t] == null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x000078F8 File Offset: 0x000068F8
		private static void AddExecuteMethodValidTypes()
		{
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(bool), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(byte), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(char), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(DateTime), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(decimal), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(double), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(Guid), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(short), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(int), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(long), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(IntPtr), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(sbyte), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(float), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(string), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(TimeSpan), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(ushort), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(uint), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(ulong), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(UIntPtr), true);
			ServicedComponentInfo._ExecuteMessageCache.Add(typeof(void), true);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00007B10 File Offset: 0x00006B10
		private static bool IsMethodAutoDone2(MemberInfo m)
		{
			object[] customAttributes = m.GetCustomAttributes(typeof(AutoCompleteAttribute), true);
			object[] array = customAttributes;
			int num = 0;
			if (num >= array.Length)
			{
				return false;
			}
			object obj = array[num];
			return ((AutoCompleteAttribute)obj).Value;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00007B54 File Offset: 0x00006B54
		private static bool HasSpecialMethodAttributes2(MemberInfo m)
		{
			object[] customAttributes = m.GetCustomAttributes(true);
			foreach (object obj in customAttributes)
			{
				if (obj is IConfigurationAttribute && !(obj is AutoCompleteAttribute))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00007B9C File Offset: 0x00006B9C
		private static bool AreMethodsSecure2(Type t)
		{
			object[] customAttributes = t.GetCustomAttributes(typeof(SecureMethodAttribute), true);
			return customAttributes != null && customAttributes.Length > 0;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00007BC8 File Offset: 0x00006BC8
		private static bool HasClassInterface2(Type t)
		{
			object[] array = t.GetCustomAttributes(typeof(ClassInterfaceAttribute), false);
			if (array != null && array.Length > 0)
			{
				ClassInterfaceAttribute classInterfaceAttribute = (ClassInterfaceAttribute)array[0];
				if (classInterfaceAttribute.Value == ClassInterfaceType.AutoDual || classInterfaceAttribute.Value == ClassInterfaceType.AutoDispatch)
				{
					return true;
				}
			}
			array = t.Assembly.GetCustomAttributes(typeof(ClassInterfaceAttribute), true);
			if (array != null && array.Length > 0)
			{
				ClassInterfaceAttribute classInterfaceAttribute2 = (ClassInterfaceAttribute)array[0];
				if (classInterfaceAttribute2.Value == ClassInterfaceType.AutoDual || classInterfaceAttribute2.Value == ClassInterfaceType.AutoDispatch)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00007C4C File Offset: 0x00006C4C
		internal static ClassInterfaceType GetClassInterfaceType(Type t)
		{
			object[] array = t.GetCustomAttributes(typeof(ClassInterfaceAttribute), false);
			if (array == null || array.Length == 0)
			{
				array = t.Assembly.GetCustomAttributes(typeof(ClassInterfaceAttribute), true);
				if (array == null || array.Length == 0)
				{
					return ClassInterfaceType.None;
				}
			}
			return ((ClassInterfaceAttribute)array[0]).Value;
		}

		// Token: 0x0400010E RID: 270
		internal const int SCI_PRESENT = 1;

		// Token: 0x0400010F RID: 271
		internal const int SCI_SERVICEDCOMPONENT = 2;

		// Token: 0x04000110 RID: 272
		internal const int SCI_EVENTSOURCE = 4;

		// Token: 0x04000111 RID: 273
		internal const int SCI_JIT = 8;

		// Token: 0x04000112 RID: 274
		internal const int SCI_OBJECTPOOLED = 16;

		// Token: 0x04000113 RID: 275
		internal const int SCI_METHODSSECURE = 32;

		// Token: 0x04000114 RID: 276
		internal const int SCI_CLASSINTERFACE = 64;

		// Token: 0x04000115 RID: 277
		internal const int MI_PRESENT = 1;

		// Token: 0x04000116 RID: 278
		internal const int MI_AUTODONE = 2;

		// Token: 0x04000117 RID: 279
		internal const int MI_HASSPECIALATTRIBUTES = 4;

		// Token: 0x04000118 RID: 280
		internal const int MI_EXECUTEMESSAGEVALID = 8;

		// Token: 0x04000119 RID: 281
		private static RWHashTable _SCICache = new RWHashTable();

		// Token: 0x0400011A RID: 282
		private static RWHashTable _MICache = new RWHashTable();

		// Token: 0x0400011B RID: 283
		private static Hashtable _ExecuteMessageCache = new Hashtable();
	}
}
