using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System
{
	// Token: 0x020000F4 RID: 244
	[Serializable]
	internal class RuntimeType : Type, ISerializable, ICloneable
	{
		// Token: 0x06000DE6 RID: 3558
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void PrepareMemberInfoCache(RuntimeTypeHandle rt);

		// Token: 0x06000DE7 RID: 3559 RVA: 0x00026F51 File Offset: 0x00025F51
		internal static MethodBase GetMethodBase(ModuleHandle scope, int typeMetadataToken)
		{
			return RuntimeType.GetMethodBase(scope.ResolveMethodHandle(typeMetadataToken));
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00026F60 File Offset: 0x00025F60
		internal static MethodBase GetMethodBase(Module scope, int typeMetadataToken)
		{
			return RuntimeType.GetMethodBase(scope.GetModuleHandle(), typeMetadataToken);
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00026F6E File Offset: 0x00025F6E
		internal static MethodBase GetMethodBase(RuntimeMethodHandle methodHandle)
		{
			return RuntimeType.GetMethodBase(RuntimeTypeHandle.EmptyHandle, methodHandle);
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00026F7C File Offset: 0x00025F7C
		internal static MethodBase GetMethodBase(RuntimeTypeHandle reflectedTypeHandle, RuntimeMethodHandle methodHandle)
		{
			if (methodHandle.IsDynamicMethod())
			{
				Resolver resolver = methodHandle.GetResolver();
				if (resolver != null)
				{
					return resolver.GetDynamicMethod();
				}
				return null;
			}
			else
			{
				Type type = methodHandle.GetDeclaringType().GetRuntimeType();
				RuntimeType runtimeType = reflectedTypeHandle.GetRuntimeType();
				RuntimeTypeHandle[] array = null;
				bool flag = false;
				if (runtimeType == null)
				{
					runtimeType = type as RuntimeType;
				}
				if (runtimeType.IsArray)
				{
					MethodBase[] array2 = runtimeType.GetMember(methodHandle.GetName(), MemberTypes.Constructor | MemberTypes.Method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) as MethodBase[];
					bool flag2 = false;
					for (int i = 0; i < array2.Length; i++)
					{
						if (array2[i].GetMethodHandle() == methodHandle)
						{
							flag2 = true;
						}
					}
					if (!flag2)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveMethodHandle"), new object[]
						{
							runtimeType.ToString(),
							type.ToString()
						}));
					}
					type = runtimeType;
				}
				else if (!type.IsAssignableFrom(runtimeType))
				{
					if (!type.IsGenericType)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveMethodHandle"), new object[]
						{
							runtimeType.ToString(),
							type.ToString()
						}));
					}
					Type genericTypeDefinition = type.GetGenericTypeDefinition();
					Type type2;
					for (type2 = runtimeType; type2 != null; type2 = type2.BaseType)
					{
						Type type3 = type2;
						if (type3.IsGenericType && !type2.IsGenericTypeDefinition)
						{
							type3 = type3.GetGenericTypeDefinition();
						}
						if (type3.Equals(genericTypeDefinition))
						{
							break;
						}
					}
					if (type2 == null)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveMethodHandle"), new object[]
						{
							runtimeType.ToString(),
							type.ToString()
						}));
					}
					type = type2;
					array = methodHandle.GetMethodInstantiation();
					bool flag3 = methodHandle.IsGenericMethodDefinition();
					methodHandle = methodHandle.GetMethodFromCanonical(type.GetTypeHandleInternal());
					if (!flag3)
					{
						flag = true;
					}
				}
				if (type.IsValueType)
				{
					methodHandle = methodHandle.GetUnboxingStub();
				}
				if (flag || (type.GetTypeHandleInternal().HasInstantiation() && !type.GetTypeHandleInternal().IsGenericTypeDefinition() && !methodHandle.HasMethodInstantiation()))
				{
					methodHandle = methodHandle.GetInstantiatingStub(type.GetTypeHandleInternal(), array);
				}
				if (methodHandle.IsConstructor())
				{
					return runtimeType.Cache.GetConstructor(type.GetTypeHandleInternal(), methodHandle);
				}
				if (methodHandle.HasMethodInstantiation() && !methodHandle.IsGenericMethodDefinition())
				{
					return runtimeType.Cache.GetGenericMethodInfo(methodHandle);
				}
				return runtimeType.Cache.GetMethod(type.GetTypeHandleInternal(), methodHandle);
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000DEB RID: 3563 RVA: 0x000271F2 File Offset: 0x000261F2
		// (set) Token: 0x06000DEC RID: 3564 RVA: 0x000271FF File Offset: 0x000261FF
		internal bool DomainInitialized
		{
			get
			{
				return this.Cache.DomainInitialized;
			}
			set
			{
				this.Cache.DomainInitialized = value;
			}
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0002720D File Offset: 0x0002620D
		internal static FieldInfo GetFieldInfo(RuntimeFieldHandle fieldHandle)
		{
			return RuntimeType.GetFieldInfo(fieldHandle.GetApproxDeclaringType(), fieldHandle);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0002721C File Offset: 0x0002621C
		internal static FieldInfo GetFieldInfo(RuntimeTypeHandle reflectedTypeHandle, RuntimeFieldHandle fieldHandle)
		{
			if (reflectedTypeHandle.IsNullHandle())
			{
				reflectedTypeHandle = fieldHandle.GetApproxDeclaringType();
			}
			else
			{
				RuntimeTypeHandle approxDeclaringType = fieldHandle.GetApproxDeclaringType();
				if (!reflectedTypeHandle.Equals(approxDeclaringType) && (!fieldHandle.AcquiresContextFromThis() || !approxDeclaringType.GetCanonicalHandle().Equals(reflectedTypeHandle.GetCanonicalHandle())))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveFieldHandle"), new object[]
					{
						reflectedTypeHandle.GetRuntimeType().ToString(),
						approxDeclaringType.GetRuntimeType().ToString()
					}));
				}
			}
			return reflectedTypeHandle.GetRuntimeType().Cache.GetField(fieldHandle);
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x000272C4 File Offset: 0x000262C4
		internal static PropertyInfo GetPropertyInfo(RuntimeTypeHandle reflectedTypeHandle, int tkProperty)
		{
			CerArrayList<RuntimePropertyInfo> propertyList = reflectedTypeHandle.GetRuntimeType().Cache.GetPropertyList(MemberListType.All, null);
			for (int i = 0; i < propertyList.Count; i++)
			{
				RuntimePropertyInfo runtimePropertyInfo = propertyList[i];
				if (runtimePropertyInfo.MetadataToken == tkProperty)
				{
					return runtimePropertyInfo;
				}
			}
			throw new SystemException();
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x00027310 File Offset: 0x00026310
		private static void ThrowIfTypeNeverValidGenericArgument(Type type)
		{
			if (type.IsPointer || type.IsByRef || type == typeof(void))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_NeverValidGenericArgument"), new object[] { type.ToString() }));
			}
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00027368 File Offset: 0x00026368
		internal static void SanityCheckGenericArguments(Type[] genericArguments, Type[] genericParamters)
		{
			if (genericArguments == null)
			{
				throw new ArgumentNullException();
			}
			for (int i = 0; i < genericArguments.Length; i++)
			{
				if (genericArguments[i] == null)
				{
					throw new ArgumentNullException();
				}
				RuntimeType.ThrowIfTypeNeverValidGenericArgument(genericArguments[i]);
			}
			if (genericArguments.Length != genericParamters.Length)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_NotEnoughGenArguments", new object[] { genericArguments.Length, genericParamters.Length }), new object[0]));
			}
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x000273E8 File Offset: 0x000263E8
		internal static void ValidateGenericArguments(MemberInfo definition, Type[] genericArguments, Exception e)
		{
			RuntimeTypeHandle[] array = null;
			RuntimeTypeHandle[] array2 = null;
			Type[] array3;
			if (definition is Type)
			{
				Type type = (Type)definition;
				array3 = type.GetGenericArguments();
				array = new RuntimeTypeHandle[genericArguments.Length];
				for (int i = 0; i < genericArguments.Length; i++)
				{
					array[i] = genericArguments[i].GetTypeHandleInternal();
				}
			}
			else
			{
				MethodInfo methodInfo = (MethodInfo)definition;
				array3 = methodInfo.GetGenericArguments();
				array2 = new RuntimeTypeHandle[genericArguments.Length];
				for (int j = 0; j < genericArguments.Length; j++)
				{
					array2[j] = genericArguments[j].GetTypeHandleInternal();
				}
				Type declaringType = methodInfo.DeclaringType;
				if (declaringType != null)
				{
					array = declaringType.GetTypeHandleInternal().GetInstantiation();
				}
			}
			for (int k = 0; k < genericArguments.Length; k++)
			{
				Type type2 = genericArguments[k];
				Type type3 = array3[k];
				if (!type3.GetTypeHandleInternal().SatisfiesConstraints(array, array2, type2.GetTypeHandleInternal()))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_GenConstraintViolation"), new object[]
					{
						k.ToString(CultureInfo.CurrentCulture),
						type2.ToString(),
						definition.ToString(),
						type3.ToString()
					}), e);
				}
			}
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00027538 File Offset: 0x00026538
		private static void SplitName(string fullname, out string name, out string ns)
		{
			name = null;
			ns = null;
			if (fullname == null)
			{
				return;
			}
			int num = fullname.LastIndexOf(".", StringComparison.Ordinal);
			if (num == -1)
			{
				name = fullname;
				return;
			}
			ns = fullname.Substring(0, num);
			int num2 = fullname.Length - ns.Length - 1;
			if (num2 != 0)
			{
				name = fullname.Substring(num + 1, num2);
				return;
			}
			name = "";
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00027598 File Offset: 0x00026598
		internal static BindingFlags FilterPreCalculate(bool isPublic, bool isInherited, bool isStatic)
		{
			BindingFlags bindingFlags = (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			if (isInherited)
			{
				bindingFlags |= BindingFlags.DeclaredOnly;
				if (isStatic)
				{
					bindingFlags |= BindingFlags.Static | BindingFlags.FlattenHierarchy;
				}
				else
				{
					bindingFlags |= BindingFlags.Instance;
				}
			}
			else if (isStatic)
			{
				bindingFlags |= BindingFlags.Static;
			}
			else
			{
				bindingFlags |= BindingFlags.Instance;
			}
			return bindingFlags;
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x000275D4 File Offset: 0x000265D4
		private static void FilterHelper(BindingFlags bindingFlags, ref string name, bool allowPrefixLookup, out bool prefixLookup, out bool ignoreCase, out MemberListType listType)
		{
			prefixLookup = false;
			ignoreCase = false;
			if (name != null)
			{
				if ((bindingFlags & BindingFlags.IgnoreCase) != BindingFlags.Default)
				{
					name = name.ToLower(CultureInfo.InvariantCulture);
					ignoreCase = true;
					listType = MemberListType.CaseInsensitive;
				}
				else
				{
					listType = MemberListType.CaseSensitive;
				}
				if (allowPrefixLookup && name.EndsWith("*", StringComparison.Ordinal))
				{
					name = name.Substring(0, name.Length - 1);
					prefixLookup = true;
					listType = MemberListType.All;
					return;
				}
			}
			else
			{
				listType = MemberListType.All;
			}
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00027640 File Offset: 0x00026640
		private static void FilterHelper(BindingFlags bindingFlags, ref string name, out bool ignoreCase, out MemberListType listType)
		{
			bool flag;
			RuntimeType.FilterHelper(bindingFlags, ref name, false, out flag, out ignoreCase, out listType);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00027659 File Offset: 0x00026659
		private static bool FilterApplyPrefixLookup(MemberInfo memberInfo, string name, bool ignoreCase)
		{
			if (ignoreCase)
			{
				if (!memberInfo.Name.ToLower(CultureInfo.InvariantCulture).StartsWith(name, StringComparison.Ordinal))
				{
					return false;
				}
			}
			else if (!memberInfo.Name.StartsWith(name, StringComparison.Ordinal))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0002768C File Offset: 0x0002668C
		private static bool FilterApplyBase(MemberInfo memberInfo, BindingFlags bindingFlags, bool isPublic, bool isNonProtectedInternal, bool isStatic, string name, bool prefixLookup)
		{
			if (isPublic)
			{
				if ((bindingFlags & BindingFlags.Public) == BindingFlags.Default)
				{
					return false;
				}
			}
			else if ((bindingFlags & BindingFlags.NonPublic) == BindingFlags.Default)
			{
				return false;
			}
			bool flag = memberInfo.DeclaringType != memberInfo.ReflectedType;
			if ((bindingFlags & BindingFlags.DeclaredOnly) != BindingFlags.Default && flag)
			{
				return false;
			}
			if (memberInfo.MemberType != MemberTypes.TypeInfo && memberInfo.MemberType != MemberTypes.NestedType)
			{
				if (isStatic)
				{
					if ((bindingFlags & BindingFlags.FlattenHierarchy) == BindingFlags.Default && flag)
					{
						return false;
					}
					if ((bindingFlags & BindingFlags.Static) == BindingFlags.Default)
					{
						return false;
					}
				}
				else if ((bindingFlags & BindingFlags.Instance) == BindingFlags.Default)
				{
					return false;
				}
			}
			if (prefixLookup && !RuntimeType.FilterApplyPrefixLookup(memberInfo, name, (bindingFlags & BindingFlags.IgnoreCase) != BindingFlags.Default))
			{
				return false;
			}
			if ((bindingFlags & BindingFlags.DeclaredOnly) == BindingFlags.Default && flag && isNonProtectedInternal && (bindingFlags & BindingFlags.NonPublic) != BindingFlags.Default && !isStatic && (bindingFlags & BindingFlags.Instance) != BindingFlags.Default)
			{
				MethodInfo methodInfo = memberInfo as MethodInfo;
				if (methodInfo == null)
				{
					return false;
				}
				if (!methodInfo.IsVirtual && !methodInfo.IsAbstract)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00027750 File Offset: 0x00026750
		private static bool FilterApplyType(Type type, BindingFlags bindingFlags, string name, bool prefixLookup, string ns)
		{
			bool flag = type.IsNestedPublic || type.IsPublic;
			bool flag2 = false;
			return RuntimeType.FilterApplyBase(type, bindingFlags, flag, type.IsNestedAssembly, flag2, name, prefixLookup) && (ns == null || type.Namespace.Equals(ns));
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0002779C File Offset: 0x0002679C
		private static bool FilterApplyMethodBaseInfo(MethodBase methodBase, BindingFlags bindingFlags, string name, CallingConventions callConv, Type[] argumentTypes, bool prefixLookup)
		{
			bindingFlags ^= BindingFlags.DeclaredOnly;
			RuntimeMethodInfo runtimeMethodInfo = methodBase as RuntimeMethodInfo;
			BindingFlags bindingFlags2;
			if (runtimeMethodInfo == null)
			{
				RuntimeConstructorInfo runtimeConstructorInfo = methodBase as RuntimeConstructorInfo;
				bindingFlags2 = runtimeConstructorInfo.BindingFlags;
			}
			else
			{
				bindingFlags2 = runtimeMethodInfo.BindingFlags;
			}
			return (bindingFlags & bindingFlags2) == bindingFlags2 && (!prefixLookup || RuntimeType.FilterApplyPrefixLookup(methodBase, name, (bindingFlags & BindingFlags.IgnoreCase) != BindingFlags.Default)) && RuntimeType.FilterApplyMethodBaseInfo(methodBase, bindingFlags, callConv, argumentTypes);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x000277F8 File Offset: 0x000267F8
		private static bool FilterApplyMethodBaseInfo(MethodBase methodBase, BindingFlags bindingFlags, CallingConventions callConv, Type[] argumentTypes)
		{
			if ((callConv & CallingConventions.Any) == (CallingConventions)0)
			{
				if ((callConv & CallingConventions.VarArgs) != (CallingConventions)0 && (methodBase.CallingConvention & CallingConventions.VarArgs) == (CallingConventions)0)
				{
					return false;
				}
				if ((callConv & CallingConventions.Standard) != (CallingConventions)0 && (methodBase.CallingConvention & CallingConventions.Standard) == (CallingConventions)0)
				{
					return false;
				}
			}
			if (argumentTypes != null)
			{
				ParameterInfo[] parametersNoCopy = methodBase.GetParametersNoCopy();
				if (argumentTypes.Length != parametersNoCopy.Length)
				{
					if ((bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.CreateInstance | BindingFlags.GetProperty | BindingFlags.SetProperty)) == BindingFlags.Default)
					{
						return false;
					}
					bool flag = false;
					bool flag2 = argumentTypes.Length > parametersNoCopy.Length;
					if (flag2)
					{
						if ((methodBase.CallingConvention & CallingConventions.VarArgs) == (CallingConventions)0)
						{
							flag = true;
						}
					}
					else if ((bindingFlags & BindingFlags.OptionalParamBinding) == BindingFlags.Default)
					{
						flag = true;
					}
					else if (!parametersNoCopy[argumentTypes.Length].IsOptional)
					{
						flag = true;
					}
					if (flag)
					{
						if (parametersNoCopy.Length == 0)
						{
							return false;
						}
						bool flag3 = argumentTypes.Length < parametersNoCopy.Length - 1;
						if (flag3)
						{
							return false;
						}
						ParameterInfo parameterInfo = parametersNoCopy[parametersNoCopy.Length - 1];
						if (!parameterInfo.ParameterType.IsArray)
						{
							return false;
						}
						if (!parameterInfo.IsDefined(typeof(ParamArrayAttribute), false))
						{
							return false;
						}
					}
				}
				else if ((bindingFlags & BindingFlags.ExactBinding) != BindingFlags.Default && (bindingFlags & BindingFlags.InvokeMethod) == BindingFlags.Default)
				{
					for (int i = 0; i < parametersNoCopy.Length; i++)
					{
						if (argumentTypes[i] != null && parametersNoCopy[i].ParameterType != argumentTypes[i])
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0002790E File Offset: 0x0002690E
		private RuntimeType(RuntimeTypeHandle typeHandle)
		{
			this.m_handle = typeHandle;
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0002791D File Offset: 0x0002691D
		internal RuntimeType()
		{
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00027928 File Offset: 0x00026928
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override bool CacheEquals(object o)
		{
			RuntimeType runtimeType = o as RuntimeType;
			return runtimeType != null && runtimeType.m_handle.Equals(this.m_handle);
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000DFF RID: 3583 RVA: 0x00027954 File Offset: 0x00026954
		private new RuntimeType.RuntimeTypeCache Cache
		{
			get
			{
				if (this.m_cache.IsNull())
				{
					IntPtr gchandle = this.m_handle.GetGCHandle(GCHandleType.WeakTrackResurrection);
					if (!Interlocked.CompareExchange(ref this.m_cache, gchandle, (IntPtr)0).IsNull())
					{
						this.m_handle.FreeGCHandle(gchandle);
					}
				}
				RuntimeType.RuntimeTypeCache runtimeTypeCache = GCHandle.InternalGet(this.m_cache) as RuntimeType.RuntimeTypeCache;
				if (runtimeTypeCache == null)
				{
					runtimeTypeCache = new RuntimeType.RuntimeTypeCache(this);
					RuntimeType.RuntimeTypeCache runtimeTypeCache2 = GCHandle.InternalCompareExchange(this.m_cache, runtimeTypeCache, null, false) as RuntimeType.RuntimeTypeCache;
					if (runtimeTypeCache2 != null)
					{
						runtimeTypeCache = runtimeTypeCache2;
					}
					if (RuntimeType.s_typeCache == null)
					{
						RuntimeType.s_typeCache = new RuntimeType.TypeCacheQueue();
					}
				}
				return runtimeTypeCache;
			}
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x000279EC File Offset: 0x000269EC
		private MethodInfo[] GetMethodCandidates(string name, BindingFlags bindingAttr, CallingConventions callConv, Type[] types, bool allowPrefixLookup)
		{
			bool flag;
			bool flag2;
			MemberListType memberListType;
			RuntimeType.FilterHelper(bindingAttr, ref name, allowPrefixLookup, out flag, out flag2, out memberListType);
			List<MethodInfo> list = new List<MethodInfo>();
			CerArrayList<RuntimeMethodInfo> methodList = this.Cache.GetMethodList(memberListType, name);
			bindingAttr ^= BindingFlags.DeclaredOnly;
			for (int i = 0; i < methodList.Count; i++)
			{
				RuntimeMethodInfo runtimeMethodInfo = methodList[i];
				if ((bindingAttr & runtimeMethodInfo.BindingFlags) == runtimeMethodInfo.BindingFlags && RuntimeType.FilterApplyMethodBaseInfo(runtimeMethodInfo, bindingAttr, callConv, types) && (!flag || RuntimeType.FilterApplyPrefixLookup(runtimeMethodInfo, name, flag2)))
				{
					list.Add(runtimeMethodInfo);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00027A80 File Offset: 0x00026A80
		private ConstructorInfo[] GetConstructorCandidates(string name, BindingFlags bindingAttr, CallingConventions callConv, Type[] types, bool allowPrefixLookup)
		{
			bool flag;
			bool flag2;
			MemberListType memberListType;
			RuntimeType.FilterHelper(bindingAttr, ref name, allowPrefixLookup, out flag, out flag2, out memberListType);
			List<ConstructorInfo> list = new List<ConstructorInfo>();
			CerArrayList<RuntimeConstructorInfo> constructorList = this.Cache.GetConstructorList(memberListType, name);
			bindingAttr ^= BindingFlags.DeclaredOnly;
			for (int i = 0; i < constructorList.Count; i++)
			{
				RuntimeConstructorInfo runtimeConstructorInfo = constructorList[i];
				if ((bindingAttr & runtimeConstructorInfo.BindingFlags) == runtimeConstructorInfo.BindingFlags && RuntimeType.FilterApplyMethodBaseInfo(runtimeConstructorInfo, bindingAttr, callConv, types) && (!flag || RuntimeType.FilterApplyPrefixLookup(runtimeConstructorInfo, name, flag2)))
				{
					list.Add(runtimeConstructorInfo);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00027B14 File Offset: 0x00026B14
		private PropertyInfo[] GetPropertyCandidates(string name, BindingFlags bindingAttr, Type[] types, bool allowPrefixLookup)
		{
			bool flag;
			bool flag2;
			MemberListType memberListType;
			RuntimeType.FilterHelper(bindingAttr, ref name, allowPrefixLookup, out flag, out flag2, out memberListType);
			List<PropertyInfo> list = new List<PropertyInfo>();
			CerArrayList<RuntimePropertyInfo> propertyList = this.Cache.GetPropertyList(memberListType, name);
			bindingAttr ^= BindingFlags.DeclaredOnly;
			for (int i = 0; i < propertyList.Count; i++)
			{
				RuntimePropertyInfo runtimePropertyInfo = propertyList[i];
				if ((bindingAttr & runtimePropertyInfo.BindingFlags) == runtimePropertyInfo.BindingFlags && (!flag || RuntimeType.FilterApplyPrefixLookup(runtimePropertyInfo, name, flag2)) && (types == null || runtimePropertyInfo.GetIndexParameters().Length == types.Length))
				{
					list.Add(runtimePropertyInfo);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00027BAC File Offset: 0x00026BAC
		private EventInfo[] GetEventCandidates(string name, BindingFlags bindingAttr, bool allowPrefixLookup)
		{
			bool flag;
			bool flag2;
			MemberListType memberListType;
			RuntimeType.FilterHelper(bindingAttr, ref name, allowPrefixLookup, out flag, out flag2, out memberListType);
			List<EventInfo> list = new List<EventInfo>();
			CerArrayList<RuntimeEventInfo> eventList = this.Cache.GetEventList(memberListType, name);
			bindingAttr ^= BindingFlags.DeclaredOnly;
			for (int i = 0; i < eventList.Count; i++)
			{
				RuntimeEventInfo runtimeEventInfo = eventList[i];
				if ((bindingAttr & runtimeEventInfo.BindingFlags) == runtimeEventInfo.BindingFlags && (!flag || RuntimeType.FilterApplyPrefixLookup(runtimeEventInfo, name, flag2)))
				{
					list.Add(runtimeEventInfo);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00027C34 File Offset: 0x00026C34
		private FieldInfo[] GetFieldCandidates(string name, BindingFlags bindingAttr, bool allowPrefixLookup)
		{
			bool flag;
			bool flag2;
			MemberListType memberListType;
			RuntimeType.FilterHelper(bindingAttr, ref name, allowPrefixLookup, out flag, out flag2, out memberListType);
			List<FieldInfo> list = new List<FieldInfo>();
			CerArrayList<RuntimeFieldInfo> fieldList = this.Cache.GetFieldList(memberListType, name);
			bindingAttr ^= BindingFlags.DeclaredOnly;
			for (int i = 0; i < fieldList.Count; i++)
			{
				RuntimeFieldInfo runtimeFieldInfo = fieldList[i];
				if ((bindingAttr & runtimeFieldInfo.BindingFlags) == runtimeFieldInfo.BindingFlags && (!flag || RuntimeType.FilterApplyPrefixLookup(runtimeFieldInfo, name, flag2)))
				{
					list.Add(runtimeFieldInfo);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00027CBC File Offset: 0x00026CBC
		private Type[] GetNestedTypeCandidates(string fullname, BindingFlags bindingAttr, bool allowPrefixLookup)
		{
			bindingAttr &= ~BindingFlags.Static;
			string text;
			string text2;
			RuntimeType.SplitName(fullname, out text, out text2);
			bool flag;
			bool flag2;
			MemberListType memberListType;
			RuntimeType.FilterHelper(bindingAttr, ref text, allowPrefixLookup, out flag, out flag2, out memberListType);
			List<Type> list = new List<Type>();
			CerArrayList<RuntimeType> nestedTypeList = this.Cache.GetNestedTypeList(memberListType, text);
			for (int i = 0; i < nestedTypeList.Count; i++)
			{
				RuntimeType runtimeType = nestedTypeList[i];
				if (RuntimeType.FilterApplyType(runtimeType, bindingAttr, text, flag, text2))
				{
					list.Add(runtimeType);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00027D3D File Offset: 0x00026D3D
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return this.GetMethodCandidates(null, bindingAttr, CallingConventions.Any, null, false);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00027D4A File Offset: 0x00026D4A
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			return this.GetConstructorCandidates(null, bindingAttr, CallingConventions.Any, null, false);
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00027D57 File Offset: 0x00026D57
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			return this.GetPropertyCandidates(null, bindingAttr, null, false);
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x00027D63 File Offset: 0x00026D63
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			return this.GetEventCandidates(null, bindingAttr, false);
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00027D6E File Offset: 0x00026D6E
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			return this.GetFieldCandidates(null, bindingAttr, false);
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00027D7C File Offset: 0x00026D7C
		public override Type[] GetInterfaces()
		{
			CerArrayList<RuntimeType> interfaceList = this.Cache.GetInterfaceList(MemberListType.All, null);
			Type[] array = new Type[interfaceList.Count];
			for (int i = 0; i < interfaceList.Count; i++)
			{
				JitHelpers.UnsafeSetArrayElement(array, i, interfaceList[i]);
			}
			return array;
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x00027DC3 File Offset: 0x00026DC3
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			return this.GetNestedTypeCandidates(null, bindingAttr, false);
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00027DD0 File Offset: 0x00026DD0
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			MethodInfo[] methodCandidates = this.GetMethodCandidates(null, bindingAttr, CallingConventions.Any, null, false);
			ConstructorInfo[] constructorCandidates = this.GetConstructorCandidates(null, bindingAttr, CallingConventions.Any, null, false);
			PropertyInfo[] propertyCandidates = this.GetPropertyCandidates(null, bindingAttr, null, false);
			EventInfo[] eventCandidates = this.GetEventCandidates(null, bindingAttr, false);
			FieldInfo[] fieldCandidates = this.GetFieldCandidates(null, bindingAttr, false);
			Type[] nestedTypeCandidates = this.GetNestedTypeCandidates(null, bindingAttr, false);
			MemberInfo[] array = new MemberInfo[methodCandidates.Length + constructorCandidates.Length + propertyCandidates.Length + eventCandidates.Length + fieldCandidates.Length + nestedTypeCandidates.Length];
			int num = 0;
			Array.Copy(methodCandidates, 0, array, num, methodCandidates.Length);
			num += methodCandidates.Length;
			Array.Copy(constructorCandidates, 0, array, num, constructorCandidates.Length);
			num += constructorCandidates.Length;
			Array.Copy(propertyCandidates, 0, array, num, propertyCandidates.Length);
			num += propertyCandidates.Length;
			Array.Copy(eventCandidates, 0, array, num, eventCandidates.Length);
			num += eventCandidates.Length;
			Array.Copy(fieldCandidates, 0, array, num, fieldCandidates.Length);
			num += fieldCandidates.Length;
			Array.Copy(nestedTypeCandidates, 0, array, num, nestedTypeCandidates.Length);
			num += nestedTypeCandidates.Length;
			return array;
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00027ED0 File Offset: 0x00026ED0
		public override InterfaceMapping GetInterfaceMap(Type ifaceType)
		{
			if (this.IsGenericParameter)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_GenericParameter"));
			}
			if (ifaceType == null)
			{
				throw new ArgumentNullException("ifaceType");
			}
			if (!(ifaceType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"), "ifaceType");
			}
			RuntimeType runtimeType = ifaceType as RuntimeType;
			RuntimeTypeHandle typeHandleInternal = runtimeType.GetTypeHandleInternal();
			int firstSlotForInterface = this.GetTypeHandleInternal().GetFirstSlotForInterface(runtimeType.GetTypeHandleInternal());
			int interfaceMethodSlots = typeHandleInternal.GetInterfaceMethodSlots();
			int num = 0;
			for (int i = 0; i < interfaceMethodSlots; i++)
			{
				if ((typeHandleInternal.GetMethodAt(i).GetAttributes() & MethodAttributes.Static) != MethodAttributes.PrivateScope)
				{
					num++;
				}
			}
			int num2 = interfaceMethodSlots - num;
			InterfaceMapping interfaceMapping;
			interfaceMapping.InterfaceType = ifaceType;
			interfaceMapping.TargetType = this;
			interfaceMapping.InterfaceMethods = new MethodInfo[num2];
			interfaceMapping.TargetMethods = new MethodInfo[num2];
			for (int j = 0; j < interfaceMethodSlots; j++)
			{
				RuntimeMethodHandle runtimeMethodHandle = typeHandleInternal.GetMethodAt(j);
				if ((typeHandleInternal.GetMethodAt(j).GetAttributes() & MethodAttributes.Static) == MethodAttributes.PrivateScope)
				{
					bool flag = typeHandleInternal.HasInstantiation() && !typeHandleInternal.IsGenericTypeDefinition();
					if (flag)
					{
						runtimeMethodHandle = runtimeMethodHandle.GetInstantiatingStubIfNeeded(typeHandleInternal);
					}
					MethodBase methodBase = RuntimeType.GetMethodBase(typeHandleInternal, runtimeMethodHandle);
					interfaceMapping.InterfaceMethods[j] = (MethodInfo)methodBase;
					int num3;
					if (firstSlotForInterface == -1)
					{
						num3 = this.GetTypeHandleInternal().GetInterfaceMethodImplementationSlot(typeHandleInternal, runtimeMethodHandle);
					}
					else
					{
						num3 = firstSlotForInterface + j;
					}
					if (num3 != -1)
					{
						RuntimeTypeHandle typeHandleInternal2 = this.GetTypeHandleInternal();
						RuntimeMethodHandle runtimeMethodHandle2 = typeHandleInternal2.GetMethodAt(num3);
						flag = typeHandleInternal2.HasInstantiation() && !typeHandleInternal2.IsGenericTypeDefinition();
						if (flag)
						{
							runtimeMethodHandle2 = runtimeMethodHandle2.GetInstantiatingStubIfNeeded(typeHandleInternal2);
						}
						MethodBase methodBase2 = RuntimeType.GetMethodBase(typeHandleInternal2, runtimeMethodHandle2);
						interfaceMapping.TargetMethods[j] = (MethodInfo)methodBase2;
					}
				}
			}
			return interfaceMapping;
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x000280A8 File Offset: 0x000270A8
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConv, Type[] types, ParameterModifier[] modifiers)
		{
			MethodInfo[] methodCandidates = this.GetMethodCandidates(name, bindingAttr, callConv, types, false);
			if (methodCandidates.Length == 0)
			{
				return null;
			}
			if (types == null || types.Length == 0)
			{
				if (methodCandidates.Length == 1)
				{
					return methodCandidates[0];
				}
				if (types == null)
				{
					for (int i = 1; i < methodCandidates.Length; i++)
					{
						MethodInfo methodInfo = methodCandidates[i];
						if (!global::System.DefaultBinder.CompareMethodSigAndName(methodInfo, methodCandidates[0]))
						{
							throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
						}
					}
					return global::System.DefaultBinder.FindMostDerivedNewSlotMeth(methodCandidates, methodCandidates.Length) as MethodInfo;
				}
			}
			if (binder == null)
			{
				binder = Type.DefaultBinder;
			}
			return binder.SelectMethod(bindingAttr, methodCandidates, types, modifiers) as MethodInfo;
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0002813C File Offset: 0x0002713C
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			ConstructorInfo[] constructorCandidates = this.GetConstructorCandidates(null, bindingAttr, CallingConventions.Any, types, false);
			if (binder == null)
			{
				binder = Type.DefaultBinder;
			}
			if (constructorCandidates.Length == 0)
			{
				return null;
			}
			if (types.Length == 0 && constructorCandidates.Length == 1)
			{
				ParameterInfo[] parametersNoCopy = constructorCandidates[0].GetParametersNoCopy();
				if (parametersNoCopy == null || parametersNoCopy.Length == 0)
				{
					return constructorCandidates[0];
				}
			}
			if ((bindingAttr & BindingFlags.ExactBinding) != BindingFlags.Default)
			{
				return global::System.DefaultBinder.ExactBinding(constructorCandidates, types, modifiers) as ConstructorInfo;
			}
			return binder.SelectMethod(bindingAttr, constructorCandidates, types, modifiers) as ConstructorInfo;
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x000281B4 File Offset: 0x000271B4
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			if (name == null)
			{
				throw new ArgumentNullException();
			}
			PropertyInfo[] propertyCandidates = this.GetPropertyCandidates(name, bindingAttr, types, false);
			if (binder == null)
			{
				binder = Type.DefaultBinder;
			}
			if (propertyCandidates.Length == 0)
			{
				return null;
			}
			if (types == null || types.Length == 0)
			{
				if (propertyCandidates.Length == 1)
				{
					if (returnType != null && returnType != propertyCandidates[0].PropertyType)
					{
						return null;
					}
					return propertyCandidates[0];
				}
				else if (returnType == null)
				{
					throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
				}
			}
			if ((bindingAttr & BindingFlags.ExactBinding) != BindingFlags.Default)
			{
				return global::System.DefaultBinder.ExactPropertyBinding(propertyCandidates, returnType, types, modifiers);
			}
			return binder.SelectProperty(bindingAttr, propertyCandidates, returnType, types, modifiers);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00028248 File Offset: 0x00027248
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			if (name == null)
			{
				throw new ArgumentNullException();
			}
			bool flag;
			MemberListType memberListType;
			RuntimeType.FilterHelper(bindingAttr, ref name, out flag, out memberListType);
			CerArrayList<RuntimeEventInfo> eventList = this.Cache.GetEventList(memberListType, name);
			EventInfo eventInfo = null;
			bindingAttr ^= BindingFlags.DeclaredOnly;
			for (int i = 0; i < eventList.Count; i++)
			{
				RuntimeEventInfo runtimeEventInfo = eventList[i];
				if ((bindingAttr & runtimeEventInfo.BindingFlags) == runtimeEventInfo.BindingFlags)
				{
					if (eventInfo != null)
					{
						throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
					}
					eventInfo = runtimeEventInfo;
				}
			}
			return eventInfo;
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x000282C8 File Offset: 0x000272C8
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			if (name == null)
			{
				throw new ArgumentNullException();
			}
			bool flag;
			MemberListType memberListType;
			RuntimeType.FilterHelper(bindingAttr, ref name, out flag, out memberListType);
			CerArrayList<RuntimeFieldInfo> fieldList = this.Cache.GetFieldList(memberListType, name);
			FieldInfo fieldInfo = null;
			bindingAttr ^= BindingFlags.DeclaredOnly;
			bool flag2 = false;
			for (int i = 0; i < fieldList.Count; i++)
			{
				RuntimeFieldInfo runtimeFieldInfo = fieldList[i];
				if ((bindingAttr & runtimeFieldInfo.BindingFlags) == runtimeFieldInfo.BindingFlags)
				{
					if (fieldInfo != null)
					{
						if (runtimeFieldInfo.DeclaringType == fieldInfo.DeclaringType)
						{
							throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
						}
						if (fieldInfo.DeclaringType.IsInterface && runtimeFieldInfo.DeclaringType.IsInterface)
						{
							flag2 = true;
						}
					}
					if (fieldInfo == null || runtimeFieldInfo.DeclaringType.IsSubclassOf(fieldInfo.DeclaringType) || fieldInfo.DeclaringType.IsInterface)
					{
						fieldInfo = runtimeFieldInfo;
					}
				}
			}
			if (flag2 && fieldInfo.DeclaringType.IsInterface)
			{
				throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
			}
			return fieldInfo;
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x000283C4 File Offset: 0x000273C4
		public override Type GetInterface(string fullname, bool ignoreCase)
		{
			if (fullname == null)
			{
				throw new ArgumentNullException();
			}
			BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic;
			bindingFlags &= ~BindingFlags.Static;
			if (ignoreCase)
			{
				bindingFlags |= BindingFlags.IgnoreCase;
			}
			string text;
			string text2;
			RuntimeType.SplitName(fullname, out text, out text2);
			MemberListType memberListType;
			RuntimeType.FilterHelper(bindingFlags, ref text, out ignoreCase, out memberListType);
			CerArrayList<RuntimeType> interfaceList = this.Cache.GetInterfaceList(memberListType, text);
			RuntimeType runtimeType = null;
			for (int i = 0; i < interfaceList.Count; i++)
			{
				RuntimeType runtimeType2 = interfaceList[i];
				if (RuntimeType.FilterApplyType(runtimeType2, bindingFlags, text, false, text2))
				{
					if (runtimeType != null)
					{
						throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
					}
					runtimeType = runtimeType2;
				}
			}
			return runtimeType;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0002845C File Offset: 0x0002745C
		public override Type GetNestedType(string fullname, BindingFlags bindingAttr)
		{
			if (fullname == null)
			{
				throw new ArgumentNullException();
			}
			bindingAttr &= ~BindingFlags.Static;
			string text;
			string text2;
			RuntimeType.SplitName(fullname, out text, out text2);
			bool flag;
			MemberListType memberListType;
			RuntimeType.FilterHelper(bindingAttr, ref text, out flag, out memberListType);
			CerArrayList<RuntimeType> nestedTypeList = this.Cache.GetNestedTypeList(memberListType, text);
			RuntimeType runtimeType = null;
			for (int i = 0; i < nestedTypeList.Count; i++)
			{
				RuntimeType runtimeType2 = nestedTypeList[i];
				if (RuntimeType.FilterApplyType(runtimeType2, bindingAttr, text, false, text2))
				{
					if (runtimeType != null)
					{
						throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
					}
					runtimeType = runtimeType2;
				}
			}
			return runtimeType;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x000284E8 File Offset: 0x000274E8
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			if (name == null)
			{
				throw new ArgumentNullException();
			}
			MethodInfo[] array = new MethodInfo[0];
			ConstructorInfo[] array2 = new ConstructorInfo[0];
			PropertyInfo[] array3 = new PropertyInfo[0];
			EventInfo[] array4 = new EventInfo[0];
			FieldInfo[] array5 = new FieldInfo[0];
			Type[] array6 = new Type[0];
			if ((type & MemberTypes.Method) != (MemberTypes)0)
			{
				array = this.GetMethodCandidates(name, bindingAttr, CallingConventions.Any, null, true);
			}
			if ((type & MemberTypes.Constructor) != (MemberTypes)0)
			{
				array2 = this.GetConstructorCandidates(name, bindingAttr, CallingConventions.Any, null, true);
			}
			if ((type & MemberTypes.Property) != (MemberTypes)0)
			{
				array3 = this.GetPropertyCandidates(name, bindingAttr, null, true);
			}
			if ((type & MemberTypes.Event) != (MemberTypes)0)
			{
				array4 = this.GetEventCandidates(name, bindingAttr, true);
			}
			if ((type & MemberTypes.Field) != (MemberTypes)0)
			{
				array5 = this.GetFieldCandidates(name, bindingAttr, true);
			}
			if ((type & (MemberTypes.TypeInfo | MemberTypes.NestedType)) != (MemberTypes)0)
			{
				array6 = this.GetNestedTypeCandidates(name, bindingAttr, true);
			}
			if (type <= MemberTypes.Property)
			{
				switch (type)
				{
				case MemberTypes.Constructor:
					return array2;
				case MemberTypes.Event:
					return array4;
				case MemberTypes.Constructor | MemberTypes.Event:
				case MemberTypes.Constructor | MemberTypes.Field:
				case MemberTypes.Event | MemberTypes.Field:
				case MemberTypes.Constructor | MemberTypes.Event | MemberTypes.Field:
					break;
				case MemberTypes.Field:
					return array5;
				case MemberTypes.Method:
					return array;
				case MemberTypes.Constructor | MemberTypes.Method:
				{
					MethodBase[] array7 = new MethodBase[array.Length + array2.Length];
					Array.Copy(array, array7, array.Length);
					Array.Copy(array2, 0, array7, array.Length, array2.Length);
					return array7;
				}
				default:
					if (type == MemberTypes.Property)
					{
						return array3;
					}
					break;
				}
			}
			else
			{
				if (type == MemberTypes.TypeInfo)
				{
					return array6;
				}
				if (type == MemberTypes.NestedType)
				{
					return array6;
				}
			}
			MemberInfo[] array8 = new MemberInfo[array.Length + array2.Length + array3.Length + array4.Length + array5.Length + array6.Length];
			int num = 0;
			if (array.Length > 0)
			{
				Array.Copy(array, 0, array8, num, array.Length);
			}
			num += array.Length;
			if (array2.Length > 0)
			{
				Array.Copy(array2, 0, array8, num, array2.Length);
			}
			num += array2.Length;
			if (array3.Length > 0)
			{
				Array.Copy(array3, 0, array8, num, array3.Length);
			}
			num += array3.Length;
			if (array4.Length > 0)
			{
				Array.Copy(array4, 0, array8, num, array4.Length);
			}
			num += array4.Length;
			if (array5.Length > 0)
			{
				Array.Copy(array5, 0, array8, num, array5.Length);
			}
			num += array5.Length;
			if (array6.Length > 0)
			{
				Array.Copy(array6, 0, array8, num, array6.Length);
			}
			num += array6.Length;
			return array8;
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000E17 RID: 3607 RVA: 0x000286F0 File Offset: 0x000276F0
		public override Module Module
		{
			get
			{
				return this.GetTypeHandleInternal().GetModuleHandle().GetModule();
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x00028714 File Offset: 0x00027714
		public override Assembly Assembly
		{
			get
			{
				return this.GetTypeHandleInternal().GetAssemblyHandle().GetAssembly();
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000E19 RID: 3609 RVA: 0x00028737 File Offset: 0x00027737
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				return this.m_handle;
			}
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0002873F File Offset: 0x0002773F
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override RuntimeTypeHandle GetTypeHandleInternal()
		{
			return this.m_handle;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00028748 File Offset: 0x00027748
		internal override TypeCode GetTypeCodeInternal()
		{
			TypeCode typeCode = this.Cache.TypeCode;
			if (typeCode != TypeCode.Empty)
			{
				return typeCode;
			}
			switch (this.GetTypeHandleInternal().GetCorElementType())
			{
			case CorElementType.Boolean:
				typeCode = TypeCode.Boolean;
				goto IL_0113;
			case CorElementType.Char:
				typeCode = TypeCode.Char;
				goto IL_0113;
			case CorElementType.I1:
				typeCode = TypeCode.SByte;
				goto IL_0113;
			case CorElementType.U1:
				typeCode = TypeCode.Byte;
				goto IL_0113;
			case CorElementType.I2:
				typeCode = TypeCode.Int16;
				goto IL_0113;
			case CorElementType.U2:
				typeCode = TypeCode.UInt16;
				goto IL_0113;
			case CorElementType.I4:
				typeCode = TypeCode.Int32;
				goto IL_0113;
			case CorElementType.U4:
				typeCode = TypeCode.UInt32;
				goto IL_0113;
			case CorElementType.I8:
				typeCode = TypeCode.Int64;
				goto IL_0113;
			case CorElementType.U8:
				typeCode = TypeCode.UInt64;
				goto IL_0113;
			case CorElementType.R4:
				typeCode = TypeCode.Single;
				goto IL_0113;
			case CorElementType.R8:
				typeCode = TypeCode.Double;
				goto IL_0113;
			case CorElementType.String:
				typeCode = TypeCode.String;
				goto IL_0113;
			case CorElementType.ValueType:
				if (this == Convert.ConvertTypes[15])
				{
					typeCode = TypeCode.Decimal;
					goto IL_0113;
				}
				if (this == Convert.ConvertTypes[16])
				{
					typeCode = TypeCode.DateTime;
					goto IL_0113;
				}
				if (base.IsEnum)
				{
					typeCode = Type.GetTypeCode(Enum.GetUnderlyingType(this));
					goto IL_0113;
				}
				typeCode = TypeCode.Object;
				goto IL_0113;
			}
			if (this == Convert.ConvertTypes[2])
			{
				typeCode = TypeCode.DBNull;
			}
			else if (this == Convert.ConvertTypes[18])
			{
				typeCode = TypeCode.String;
			}
			else
			{
				typeCode = TypeCode.Object;
			}
			IL_0113:
			this.Cache.TypeCode = typeCode;
			return typeCode;
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x00028878 File Offset: 0x00027878
		public override MethodBase DeclaringMethod
		{
			get
			{
				if (!this.IsGenericParameter)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericParameter"));
				}
				RuntimeMethodHandle declaringMethod = this.GetTypeHandleInternal().GetDeclaringMethod();
				if (declaringMethod.IsNullHandle())
				{
					return null;
				}
				return RuntimeType.GetMethodBase(declaringMethod.GetDeclaringType(), declaringMethod);
			}
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x000288C4 File Offset: 0x000278C4
		public override bool IsInstanceOfType(object o)
		{
			return this.GetTypeHandleInternal().IsInstanceOfType(o);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x000288E0 File Offset: 0x000278E0
		[ComVisible(true)]
		public override bool IsSubclassOf(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			for (Type type2 = this.BaseType; type2 != null; type2 = type2.BaseType)
			{
				if (type2 == type)
				{
					return true;
				}
			}
			return type == typeof(object) && type != this;
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000E1F RID: 3615 RVA: 0x00028928 File Offset: 0x00027928
		public override Type BaseType
		{
			get
			{
				if (base.IsInterface)
				{
					return null;
				}
				if (this.m_handle.IsGenericVariable())
				{
					Type[] genericParameterConstraints = this.GetGenericParameterConstraints();
					Type type = typeof(object);
					foreach (Type type2 in genericParameterConstraints)
					{
						if (!type2.IsInterface)
						{
							if (type2.IsGenericParameter)
							{
								GenericParameterAttributes genericParameterAttributes = type2.GenericParameterAttributes & GenericParameterAttributes.SpecialConstraintMask;
								if ((genericParameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint) == GenericParameterAttributes.None && (genericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) == GenericParameterAttributes.None)
								{
									goto IL_005A;
								}
							}
							type = type2;
						}
						IL_005A:;
					}
					if (type == typeof(object))
					{
						GenericParameterAttributes genericParameterAttributes2 = this.GenericParameterAttributes & GenericParameterAttributes.SpecialConstraintMask;
						if ((genericParameterAttributes2 & GenericParameterAttributes.NotNullableValueTypeConstraint) != GenericParameterAttributes.None)
						{
							type = typeof(ValueType);
						}
					}
					return type;
				}
				return this.m_handle.GetBaseTypeHandle().GetRuntimeType();
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x000289D8 File Offset: 0x000279D8
		public override Type UnderlyingSystemType
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000E21 RID: 3617 RVA: 0x000289DB File Offset: 0x000279DB
		public override string FullName
		{
			get
			{
				return this.Cache.GetFullName();
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x000289E8 File Offset: 0x000279E8
		public override string AssemblyQualifiedName
		{
			get
			{
				if (!this.IsGenericTypeDefinition && this.ContainsGenericParameters)
				{
					return null;
				}
				return Assembly.CreateQualifiedName(this.Assembly.FullName, this.FullName);
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000E23 RID: 3619 RVA: 0x00028A14 File Offset: 0x00027A14
		public override string Namespace
		{
			get
			{
				string nameSpace = this.Cache.GetNameSpace();
				if (nameSpace == null || nameSpace.Length == 0)
				{
					return null;
				}
				return nameSpace;
			}
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x00028A3B File Offset: 0x00027A3B
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.m_handle.GetAttributes();
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000E25 RID: 3621 RVA: 0x00028A48 File Offset: 0x00027A48
		public override Guid GUID
		{
			get
			{
				Guid guid = default(Guid);
				this.GetGUID(ref guid);
				return guid;
			}
		}

		// Token: 0x06000E26 RID: 3622
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetGUID(ref Guid result);

		// Token: 0x06000E27 RID: 3623 RVA: 0x00028A68 File Offset: 0x00027A68
		protected override bool IsContextfulImpl()
		{
			return this.GetTypeHandleInternal().IsContextful();
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00028A84 File Offset: 0x00027A84
		protected override bool IsByRefImpl()
		{
			CorElementType corElementType = this.GetTypeHandleInternal().GetCorElementType();
			return corElementType == CorElementType.ByRef;
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00028AA8 File Offset: 0x00027AA8
		protected override bool IsPrimitiveImpl()
		{
			CorElementType corElementType = this.GetTypeHandleInternal().GetCorElementType();
			return (corElementType >= CorElementType.Boolean && corElementType <= CorElementType.R8) || corElementType == CorElementType.I || corElementType == CorElementType.U;
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00028ADC File Offset: 0x00027ADC
		protected override bool IsPointerImpl()
		{
			CorElementType corElementType = this.GetTypeHandleInternal().GetCorElementType();
			return corElementType == CorElementType.Ptr;
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00028B00 File Offset: 0x00027B00
		protected override bool IsCOMObjectImpl()
		{
			return this.GetTypeHandleInternal().IsComObject(false);
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00028B1C File Offset: 0x00027B1C
		internal override bool HasProxyAttributeImpl()
		{
			return this.GetTypeHandleInternal().HasProxyAttribute();
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00028B37 File Offset: 0x00027B37
		protected override bool HasElementTypeImpl()
		{
			return base.IsArray || base.IsPointer || base.IsByRef;
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x00028B54 File Offset: 0x00027B54
		public override GenericParameterAttributes GenericParameterAttributes
		{
			get
			{
				if (!this.IsGenericParameter)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericParameter"));
				}
				GenericParameterAttributes genericParameterAttributes;
				this.GetTypeHandleInternal().GetModuleHandle().GetMetadataImport()
					.GetGenericParamProps(this.MetadataToken, out genericParameterAttributes);
				return genericParameterAttributes;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000E2F RID: 3631 RVA: 0x00028BA0 File Offset: 0x00027BA0
		internal override bool IsSzArray
		{
			get
			{
				CorElementType corElementType = this.GetTypeHandleInternal().GetCorElementType();
				return corElementType == CorElementType.SzArray;
			}
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00028BC4 File Offset: 0x00027BC4
		protected override bool IsArrayImpl()
		{
			CorElementType corElementType = this.GetTypeHandleInternal().GetCorElementType();
			return corElementType == CorElementType.Array || corElementType == CorElementType.SzArray;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00028BEC File Offset: 0x00027BEC
		public override int GetArrayRank()
		{
			if (!this.IsArrayImpl())
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_HasToBeArrayClass"));
			}
			return this.GetTypeHandleInternal().GetArrayRank();
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00028C20 File Offset: 0x00027C20
		public override Type GetElementType()
		{
			return this.GetTypeHandleInternal().GetElementType().GetRuntimeType();
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00028C44 File Offset: 0x00027C44
		public override Type[] GetGenericArguments()
		{
			RuntimeTypeHandle[] instantiation = this.GetRootElementType().GetTypeHandleInternal().GetInstantiation();
			Type[] array;
			if (instantiation != null)
			{
				array = new Type[instantiation.Length];
				for (int i = 0; i < instantiation.Length; i++)
				{
					array[i] = instantiation[i].GetRuntimeType();
				}
			}
			else
			{
				array = new Type[0];
			}
			return array;
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00028C9C File Offset: 0x00027C9C
		public override Type MakeGenericType(Type[] instantiation)
		{
			if (instantiation == null)
			{
				throw new ArgumentNullException("instantiation");
			}
			Type[] array = new Type[instantiation.Length];
			for (int i = 0; i < instantiation.Length; i++)
			{
				array[i] = instantiation[i];
			}
			instantiation = array;
			if (!this.IsGenericTypeDefinition)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_NotGenericTypeDefinition"), new object[] { this }));
			}
			for (int j = 0; j < instantiation.Length; j++)
			{
				if (instantiation[j] == null)
				{
					throw new ArgumentNullException();
				}
				if (!(instantiation[j] is RuntimeType))
				{
					return new TypeBuilderInstantiation(this, instantiation);
				}
			}
			Type[] genericArguments = this.GetGenericArguments();
			RuntimeType.SanityCheckGenericArguments(instantiation, genericArguments);
			RuntimeTypeHandle[] array2 = new RuntimeTypeHandle[instantiation.Length];
			for (int k = 0; k < instantiation.Length; k++)
			{
				array2[k] = instantiation[k].GetTypeHandleInternal();
			}
			Type type = null;
			try
			{
				type = this.m_handle.Instantiate(array2).GetRuntimeType();
			}
			catch (TypeLoadException ex)
			{
				RuntimeType.ValidateGenericArguments(this, instantiation, ex);
				throw ex;
			}
			return type;
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000E35 RID: 3637 RVA: 0x00028DB4 File Offset: 0x00027DB4
		public override bool IsGenericTypeDefinition
		{
			get
			{
				return this.m_handle.IsGenericTypeDefinition();
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x00028DC1 File Offset: 0x00027DC1
		public override bool IsGenericParameter
		{
			get
			{
				return this.m_handle.IsGenericVariable();
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000E37 RID: 3639 RVA: 0x00028DCE File Offset: 0x00027DCE
		public override int GenericParameterPosition
		{
			get
			{
				if (!this.IsGenericParameter)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericParameter"));
				}
				return this.m_handle.GetGenericVariableIndex();
			}
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00028DF4 File Offset: 0x00027DF4
		public override Type GetGenericTypeDefinition()
		{
			if (!this.IsGenericType)
			{
				throw new InvalidOperationException();
			}
			return this.m_handle.GetGenericTypeDefinition().GetRuntimeType();
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000E39 RID: 3641 RVA: 0x00028E24 File Offset: 0x00027E24
		public override bool IsGenericType
		{
			get
			{
				return !base.HasElementType && this.GetTypeHandleInternal().HasInstantiation();
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x00028E4C File Offset: 0x00027E4C
		public override bool ContainsGenericParameters
		{
			get
			{
				return this.GetRootElementType().GetTypeHandleInternal().ContainsGenericVariables();
			}
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00028E6C File Offset: 0x00027E6C
		public override Type[] GetGenericParameterConstraints()
		{
			if (!this.IsGenericParameter)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericParameter"));
			}
			RuntimeTypeHandle[] constraints = this.m_handle.GetConstraints();
			Type[] array = new Type[constraints.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = constraints[i].GetRuntimeType();
			}
			return array;
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00028EC4 File Offset: 0x00027EC4
		public override Type MakePointerType()
		{
			return this.m_handle.MakePointer().GetRuntimeType();
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00028EE4 File Offset: 0x00027EE4
		public override Type MakeByRefType()
		{
			return this.m_handle.MakeByRef().GetRuntimeType();
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00028F04 File Offset: 0x00027F04
		public override Type MakeArrayType()
		{
			return this.m_handle.MakeSZArray().GetRuntimeType();
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00028F24 File Offset: 0x00027F24
		public override Type MakeArrayType(int rank)
		{
			if (rank <= 0)
			{
				throw new IndexOutOfRangeException();
			}
			return this.m_handle.MakeArray(rank).GetRuntimeType();
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x00028F4F File Offset: 0x00027F4F
		public override StructLayoutAttribute StructLayoutAttribute
		{
			get
			{
				return (StructLayoutAttribute)StructLayoutAttribute.GetCustomAttribute(this);
			}
		}

		// Token: 0x06000E41 RID: 3649
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CanValueSpecialCast(IntPtr valueType, IntPtr targetType);

		// Token: 0x06000E42 RID: 3650
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object AllocateObjectForByRef(RuntimeTypeHandle type, object value);

		// Token: 0x06000E43 RID: 3651
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ForceEnUSLcidComInvoking();

		// Token: 0x06000E44 RID: 3652 RVA: 0x00028F5C File Offset: 0x00027F5C
		internal object CheckValue(object value, Binder binder, CultureInfo culture, BindingFlags invokeAttr)
		{
			if (this.IsInstanceOfType(value))
			{
				return value;
			}
			bool isByRef = base.IsByRef;
			if (isByRef)
			{
				Type elementType = this.GetElementType();
				if (elementType.IsInstanceOfType(value) || value == null)
				{
					return RuntimeType.AllocateObjectForByRef(elementType.TypeHandle, value);
				}
			}
			else
			{
				if (value == null)
				{
					return value;
				}
				if (this == RuntimeType.s_typedRef)
				{
					return value;
				}
			}
			bool flag = base.IsPointer || base.IsEnum || base.IsPrimitive;
			if (flag)
			{
				Pointer pointer = value as Pointer;
				Type type;
				if (pointer != null)
				{
					type = pointer.GetPointerType();
				}
				else
				{
					type = value.GetType();
				}
				if (RuntimeType.CanValueSpecialCast(type.TypeHandle.Value, this.TypeHandle.Value))
				{
					if (pointer != null)
					{
						return pointer.GetPointerValue();
					}
					return value;
				}
			}
			if ((invokeAttr & BindingFlags.ExactBinding) == BindingFlags.ExactBinding)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Arg_ObjObjEx"), new object[]
				{
					value.GetType(),
					this
				}));
			}
			if (binder != null && binder != Type.DefaultBinder)
			{
				value = binder.ChangeType(value, this, culture);
				if (this.IsInstanceOfType(value))
				{
					return value;
				}
				if (isByRef)
				{
					Type elementType2 = this.GetElementType();
					if (elementType2.IsInstanceOfType(value) || value == null)
					{
						return RuntimeType.AllocateObjectForByRef(elementType2.TypeHandle, value);
					}
				}
				else if (value == null)
				{
					return value;
				}
				if (flag)
				{
					Pointer pointer2 = value as Pointer;
					Type type2;
					if (pointer2 != null)
					{
						type2 = pointer2.GetPointerType();
					}
					else
					{
						type2 = value.GetType();
					}
					if (RuntimeType.CanValueSpecialCast(type2.TypeHandle.Value, this.TypeHandle.Value))
					{
						if (pointer2 != null)
						{
							return pointer2.GetPointerValue();
						}
						return value;
					}
				}
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Arg_ObjObjEx"), new object[]
			{
				value.GetType(),
				this
			}));
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00029138 File Offset: 0x00028138
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override object InvokeMember(string name, BindingFlags bindingFlags, Binder binder, object target, object[] providedArgs, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParams)
		{
			if (this.IsGenericParameter)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_GenericParameter"));
			}
			if ((bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.CreateInstance | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty)) == BindingFlags.Default)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_NoAccessSpec"), "bindingFlags");
			}
			if ((bindingFlags & (BindingFlags)255) == BindingFlags.Default)
			{
				bindingFlags |= BindingFlags.Instance | BindingFlags.Public;
				if ((bindingFlags & BindingFlags.CreateInstance) == BindingFlags.Default)
				{
					bindingFlags |= BindingFlags.Static;
				}
			}
			if (namedParams != null)
			{
				if (providedArgs != null)
				{
					if (namedParams.Length > providedArgs.Length)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_NamedParamTooBig"), "namedParams");
					}
				}
				else if (namedParams.Length != 0)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_NamedParamTooBig"), "namedParams");
				}
			}
			if (target != null && target.GetType().IsCOMObject)
			{
				if ((bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty)) == BindingFlags.Default)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_COMAccess"), "bindingFlags");
				}
				if ((bindingFlags & BindingFlags.GetProperty) != BindingFlags.Default && (bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty) & ~(BindingFlags.InvokeMethod | BindingFlags.GetProperty)) != BindingFlags.Default)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PropSetGet"), "bindingFlags");
				}
				if ((bindingFlags & BindingFlags.InvokeMethod) != BindingFlags.Default && (bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty) & ~(BindingFlags.InvokeMethod | BindingFlags.GetProperty)) != BindingFlags.Default)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PropSetInvoke"), "bindingFlags");
				}
				if ((bindingFlags & BindingFlags.SetProperty) != BindingFlags.Default && (bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty) & ~BindingFlags.SetProperty) != BindingFlags.Default)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_COMPropSetPut"), "bindingFlags");
				}
				if ((bindingFlags & BindingFlags.PutDispProperty) != BindingFlags.Default && (bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty) & ~BindingFlags.PutDispProperty) != BindingFlags.Default)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_COMPropSetPut"), "bindingFlags");
				}
				if ((bindingFlags & BindingFlags.PutRefDispProperty) != BindingFlags.Default && (bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty) & ~BindingFlags.PutRefDispProperty) != BindingFlags.Default)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_COMPropSetPut"), "bindingFlags");
				}
				if (RemotingServices.IsTransparentProxy(target))
				{
					return ((MarshalByRefObject)target).InvokeMember(name, bindingFlags, binder, providedArgs, modifiers, culture, namedParams);
				}
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				bool[] array = ((modifiers == null) ? null : modifiers[0].IsByRefArray);
				int num;
				if (culture == null)
				{
					num = (RuntimeType.forceInvokingWithEnUS ? 1033 : Thread.CurrentThread.CurrentCulture.LCID);
				}
				else
				{
					num = culture.LCID;
				}
				return this.InvokeDispMethod(name, bindingFlags, target, providedArgs, array, num, namedParams);
			}
			else
			{
				if (namedParams != null && Array.IndexOf<string>(namedParams, null) != -1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_NamedParamNull"), "namedParams");
				}
				int num2 = ((providedArgs != null) ? providedArgs.Length : 0);
				if (binder == null)
				{
					binder = Type.DefaultBinder;
				}
				Binder defaultBinder = Type.DefaultBinder;
				if ((bindingFlags & BindingFlags.CreateInstance) != BindingFlags.Default)
				{
					if ((bindingFlags & BindingFlags.CreateInstance) != BindingFlags.Default && (bindingFlags & (BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty)) != BindingFlags.Default)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_CreatInstAccess"), "bindingFlags");
					}
					return Activator.CreateInstance(this, bindingFlags, binder, providedArgs, culture);
				}
				else
				{
					if ((bindingFlags & (BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty)) != BindingFlags.Default)
					{
						bindingFlags |= BindingFlags.SetProperty;
					}
					if (name == null)
					{
						throw new ArgumentNullException("name");
					}
					if (name.Length == 0 || name.Equals("[DISPID=0]"))
					{
						name = this.GetDefaultMemberName();
						if (name == null)
						{
							name = "ToString";
						}
					}
					bool flag = (bindingFlags & BindingFlags.GetField) != BindingFlags.Default;
					bool flag2 = (bindingFlags & BindingFlags.SetField) != BindingFlags.Default;
					if (flag || flag2)
					{
						if (flag)
						{
							if (flag2)
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_FldSetGet"), "bindingFlags");
							}
							if ((bindingFlags & BindingFlags.SetProperty) != BindingFlags.Default)
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_FldGetPropSet"), "bindingFlags");
							}
						}
						else
						{
							if (providedArgs == null)
							{
								throw new ArgumentNullException("providedArgs");
							}
							if ((bindingFlags & BindingFlags.GetProperty) != BindingFlags.Default)
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_FldSetPropGet"), "bindingFlags");
							}
							if ((bindingFlags & BindingFlags.InvokeMethod) != BindingFlags.Default)
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_FldSetInvoke"), "bindingFlags");
							}
						}
						FieldInfo fieldInfo = null;
						FieldInfo[] array2 = this.GetMember(name, MemberTypes.Field, bindingFlags) as FieldInfo[];
						if (array2.Length == 1)
						{
							fieldInfo = array2[0];
						}
						else if (array2.Length > 0)
						{
							fieldInfo = binder.BindToField(bindingFlags, array2, flag ? Empty.Value : providedArgs[0], culture);
						}
						if (fieldInfo != null)
						{
							if (fieldInfo.FieldType.IsArray || fieldInfo.FieldType == typeof(Array))
							{
								int num3;
								if ((bindingFlags & BindingFlags.GetField) != BindingFlags.Default)
								{
									num3 = num2;
								}
								else
								{
									num3 = num2 - 1;
								}
								if (num3 > 0)
								{
									int[] array3 = new int[num3];
									for (int i = 0; i < num3; i++)
									{
										try
										{
											array3[i] = ((IConvertible)providedArgs[i]).ToInt32(null);
										}
										catch (InvalidCastException)
										{
											throw new ArgumentException(Environment.GetResourceString("Arg_IndexMustBeInt"));
										}
									}
									Array array4 = (Array)fieldInfo.GetValue(target);
									if ((bindingFlags & BindingFlags.GetField) != BindingFlags.Default)
									{
										return array4.GetValue(array3);
									}
									array4.SetValue(providedArgs[num3], array3);
									return null;
								}
							}
							if (flag)
							{
								if (num2 != 0)
								{
									throw new ArgumentException(Environment.GetResourceString("Arg_FldGetArgErr"), "bindingFlags");
								}
								return fieldInfo.GetValue(target);
							}
							else
							{
								if (num2 != 1)
								{
									throw new ArgumentException(Environment.GetResourceString("Arg_FldSetArgErr"), "bindingFlags");
								}
								fieldInfo.SetValue(target, providedArgs[0], bindingFlags, binder, culture);
								return null;
							}
						}
						else if ((bindingFlags & (BindingFlags)16773888) == BindingFlags.Default)
						{
							throw new MissingFieldException(this.FullName, name);
						}
					}
					bool flag3 = (bindingFlags & BindingFlags.GetProperty) != BindingFlags.Default;
					bool flag4 = (bindingFlags & BindingFlags.SetProperty) != BindingFlags.Default;
					if (flag3 || flag4)
					{
						if (flag3)
						{
							if (flag4)
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_PropSetGet"), "bindingFlags");
							}
						}
						else if ((bindingFlags & BindingFlags.InvokeMethod) != BindingFlags.Default)
						{
							throw new ArgumentException(Environment.GetResourceString("Arg_PropSetInvoke"), "bindingFlags");
						}
					}
					MethodInfo[] array5 = null;
					MethodInfo methodInfo = null;
					if ((bindingFlags & BindingFlags.InvokeMethod) != BindingFlags.Default)
					{
						MethodInfo[] array6 = this.GetMember(name, MemberTypes.Method, bindingFlags) as MethodInfo[];
						ArrayList arrayList = null;
						foreach (MethodInfo methodInfo2 in array6)
						{
							if (RuntimeType.FilterApplyMethodBaseInfo(methodInfo2, bindingFlags, null, CallingConventions.Any, new Type[num2], false))
							{
								if (methodInfo == null)
								{
									methodInfo = methodInfo2;
								}
								else
								{
									if (arrayList == null)
									{
										arrayList = new ArrayList(array6.Length);
										arrayList.Add(methodInfo);
									}
									arrayList.Add(methodInfo2);
								}
							}
						}
						if (arrayList != null)
						{
							array5 = new MethodInfo[arrayList.Count];
							arrayList.CopyTo(array5);
						}
					}
					if ((methodInfo == null && flag3) || flag4)
					{
						PropertyInfo[] array7 = this.GetMember(name, MemberTypes.Property, bindingFlags) as PropertyInfo[];
						ArrayList arrayList2 = null;
						for (int k = 0; k < array7.Length; k++)
						{
							MethodInfo methodInfo3;
							if (flag4)
							{
								methodInfo3 = array7[k].GetSetMethod(true);
							}
							else
							{
								methodInfo3 = array7[k].GetGetMethod(true);
							}
							if (methodInfo3 != null && RuntimeType.FilterApplyMethodBaseInfo(methodInfo3, bindingFlags, null, CallingConventions.Any, new Type[num2], false))
							{
								if (methodInfo == null)
								{
									methodInfo = methodInfo3;
								}
								else
								{
									if (arrayList2 == null)
									{
										arrayList2 = new ArrayList(array7.Length);
										arrayList2.Add(methodInfo);
									}
									arrayList2.Add(methodInfo3);
								}
							}
						}
						if (arrayList2 != null)
						{
							array5 = new MethodInfo[arrayList2.Count];
							arrayList2.CopyTo(array5);
						}
					}
					if (methodInfo == null)
					{
						throw new MissingMethodException(this.FullName, name);
					}
					if (array5 == null && num2 == 0 && methodInfo.GetParametersNoCopy().Length == 0 && (bindingFlags & BindingFlags.OptionalParamBinding) == BindingFlags.Default)
					{
						return methodInfo.Invoke(target, bindingFlags, binder, providedArgs, culture);
					}
					if (array5 == null)
					{
						array5 = new MethodInfo[] { methodInfo };
					}
					if (providedArgs == null)
					{
						providedArgs = new object[0];
					}
					object obj = null;
					MethodBase methodBase = null;
					try
					{
						methodBase = binder.BindToMethod(bindingFlags, array5, ref providedArgs, modifiers, culture, namedParams, out obj);
					}
					catch (MissingMethodException)
					{
					}
					if (methodBase == null)
					{
						throw new MissingMethodException(this.FullName, name);
					}
					object obj2 = ((MethodInfo)methodBase).Invoke(target, bindingFlags, binder, providedArgs, culture);
					if (obj != null)
					{
						binder.ReorderArgumentArray(ref providedArgs, obj);
					}
					return obj2;
				}
			}
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x000298CC File Offset: 0x000288CC
		public override bool Equals(object obj)
		{
			return obj == this;
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x000298D4 File Offset: 0x000288D4
		public override int GetHashCode()
		{
			return (int)this.GetTypeHandleInternal().Value;
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x000298F4 File Offset: 0x000288F4
		public override string ToString()
		{
			return this.Cache.GetToString();
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x00029901 File Offset: 0x00028901
		public object Clone()
		{
			return this;
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00029904 File Offset: 0x00028904
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			UnitySerializationHolder.GetUnitySerializationInfo(info, this);
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0002991B File Offset: 0x0002891B
		public override object[] GetCustomAttributes(bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType, inherit);
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00029934 File Offset: 0x00028934
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.GetCustomAttributes(this, runtimeType, inherit);
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0002997C File Offset: 0x0002897C
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.IsDefined(this, runtimeType, inherit);
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000E4E RID: 3662 RVA: 0x000299C3 File Offset: 0x000289C3
		public override string Name
		{
			get
			{
				return this.Cache.GetName();
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x000299D0 File Offset: 0x000289D0
		public override MemberTypes MemberType
		{
			get
			{
				if (base.IsPublic || base.IsNotPublic)
				{
					return MemberTypes.TypeInfo;
				}
				return MemberTypes.NestedType;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x000299EA File Offset: 0x000289EA
		public override Type DeclaringType
		{
			get
			{
				return this.Cache.GetEnclosingType();
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x000299F7 File Offset: 0x000289F7
		public override Type ReflectedType
		{
			get
			{
				return this.DeclaringType;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x000299FF File Offset: 0x000289FF
		public override int MetadataToken
		{
			get
			{
				return this.m_handle.GetToken();
			}
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00029A0C File Offset: 0x00028A0C
		internal void CreateInstanceCheckThis()
		{
			if (this is ReflectionOnlyType)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ReflectionOnlyInvoke"));
			}
			if (this.ContainsGenericParameters)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Acc_CreateGenericEx"), new object[] { this }));
			}
			Type rootElementType = this.GetRootElementType();
			if (rootElementType == typeof(ArgIterator))
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Acc_CreateArgIterator"), new object[0]));
			}
			if (rootElementType == typeof(void))
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Acc_CreateVoid"), new object[0]));
			}
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x00029AC0 File Offset: 0x00028AC0
		internal object CreateInstanceImpl(BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			this.CreateInstanceCheckThis();
			object obj = null;
			try
			{
				try
				{
					if (activationAttributes != null)
					{
						ActivationServices.PushActivationAttributes(this, activationAttributes);
					}
					if (args == null)
					{
						args = new object[0];
					}
					int num = args.Length;
					if (binder == null)
					{
						binder = Type.DefaultBinder;
					}
					if (num == 0 && (bindingAttr & BindingFlags.Public) != BindingFlags.Default && (bindingAttr & BindingFlags.Instance) != BindingFlags.Default && (this.IsGenericCOMObjectImpl() || this.IsSubclassOf(typeof(ValueType))))
					{
						obj = this.CreateInstanceImpl((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default);
					}
					else
					{
						MethodBase[] constructors = this.GetConstructors(bindingAttr);
						ArrayList arrayList = new ArrayList(constructors.Length);
						Type[] array = new Type[num];
						for (int i = 0; i < num; i++)
						{
							if (args[i] != null)
							{
								array[i] = args[i].GetType();
							}
						}
						for (int j = 0; j < constructors.Length; j++)
						{
							MethodBase methodBase = constructors[j];
							if (RuntimeType.FilterApplyMethodBaseInfo(constructors[j], bindingAttr, null, CallingConventions.Any, array, false))
							{
								arrayList.Add(constructors[j]);
							}
						}
						MethodBase[] array2 = new MethodBase[arrayList.Count];
						arrayList.CopyTo(array2);
						if (array2 != null && array2.Length == 0)
						{
							array2 = null;
						}
						if (array2 == null)
						{
							if (activationAttributes != null)
							{
								ActivationServices.PopActivationAttributes(this);
								activationAttributes = null;
							}
							throw new MissingMethodException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingConstructor_Name"), new object[] { this.FullName }));
						}
						if (num == 0 && array2.Length == 1 && (bindingAttr & BindingFlags.OptionalParamBinding) == BindingFlags.Default)
						{
							obj = Activator.CreateInstance(this, true);
						}
						else
						{
							object obj2 = null;
							MethodBase methodBase2;
							try
							{
								methodBase2 = binder.BindToMethod(bindingAttr, array2, ref args, null, culture, null, out obj2);
							}
							catch (MissingMethodException)
							{
								methodBase2 = null;
							}
							if (methodBase2 == null)
							{
								if (activationAttributes != null)
								{
									ActivationServices.PopActivationAttributes(this);
									activationAttributes = null;
								}
								throw new MissingMethodException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingConstructor_Name"), new object[] { this.FullName }));
							}
							if (typeof(Delegate).IsAssignableFrom(methodBase2.DeclaringType))
							{
								new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
							}
							obj = ((ConstructorInfo)methodBase2).Invoke(bindingAttr, binder, args, culture);
							if (obj2 != null)
							{
								binder.ReorderArgumentArray(ref args, obj2);
							}
						}
					}
				}
				finally
				{
					if (activationAttributes != null)
					{
						ActivationServices.PopActivationAttributes(this);
						activationAttributes = null;
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return obj;
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00029D2C File Offset: 0x00028D2C
		private object CreateInstanceSlow(bool publicOnly, bool fillCache)
		{
			RuntimeMethodHandle emptyHandle = RuntimeMethodHandle.EmptyHandle;
			bool flag = true;
			bool flag2 = false;
			bool flag3 = false;
			this.CreateInstanceCheckThis();
			if (!fillCache)
			{
				flag3 = true;
			}
			object obj = RuntimeTypeHandle.CreateInstance(this, publicOnly, flag3, ref flag2, ref emptyHandle, ref flag);
			if (flag2 && fillCache)
			{
				RuntimeType.ActivatorCache activatorCache = RuntimeType.s_ActivatorCache;
				if (activatorCache == null)
				{
					activatorCache = new RuntimeType.ActivatorCache();
					Thread.MemoryBarrier();
					RuntimeType.s_ActivatorCache = activatorCache;
				}
				RuntimeType.ActivatorCacheEntry activatorCacheEntry = new RuntimeType.ActivatorCacheEntry(this, emptyHandle, flag);
				Thread.MemoryBarrier();
				activatorCache.SetEntry(activatorCacheEntry);
			}
			return obj;
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00029D9E File Offset: 0x00028D9E
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal object CreateInstanceImpl(bool publicOnly)
		{
			return this.CreateInstanceImpl(publicOnly, false, true);
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00029DAC File Offset: 0x00028DAC
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal object CreateInstanceImpl(bool publicOnly, bool skipVisibilityChecks, bool fillCache)
		{
			RuntimeTypeHandle typeHandle = this.TypeHandle;
			RuntimeType.ActivatorCache activatorCache = RuntimeType.s_ActivatorCache;
			if (activatorCache != null)
			{
				RuntimeType.ActivatorCacheEntry entry = activatorCache.GetEntry(this);
				if (entry != null)
				{
					if (publicOnly && entry.m_ctor != null && (entry.m_hCtorMethodHandle.GetAttributes() & MethodAttributes.MemberAccessMask) != MethodAttributes.Public)
					{
						throw new MissingMethodException(Environment.GetResourceString("Arg_NoDefCTor"));
					}
					object obj = typeHandle.Allocate();
					if (entry.m_ctor != null)
					{
						if (!skipVisibilityChecks && entry.m_bNeedSecurityCheck)
						{
							MethodBase.PerformSecurityCheck(obj, entry.m_hCtorMethodHandle, this.TypeHandle.Value, 268435456U);
						}
						try
						{
							entry.m_ctor(obj);
						}
						catch (Exception ex)
						{
							throw new TargetInvocationException(ex);
						}
					}
					return obj;
				}
			}
			return this.CreateInstanceSlow(publicOnly, fillCache);
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00029E74 File Offset: 0x00028E74
		internal bool SupportsInterface(object o)
		{
			return this.TypeHandle.SupportsInterface(o);
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00029E90 File Offset: 0x00028E90
		internal void InvalidateCachedNestedType()
		{
			this.Cache.InvalidateCachedNestedType();
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x00029E9D File Offset: 0x00028E9D
		internal bool IsGenericCOMObjectImpl()
		{
			return this.m_handle.IsComObject(true);
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00029EAC File Offset: 0x00028EAC
		internal static bool CanCastTo(RuntimeType fromType, RuntimeType toType)
		{
			return fromType.GetTypeHandleInternal().CanCastTo(toType.GetTypeHandleInternal());
		}

		// Token: 0x06000E5C RID: 3676
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object _CreateEnum(IntPtr enumType, long value);

		// Token: 0x06000E5D RID: 3677 RVA: 0x00029ECD File Offset: 0x00028ECD
		internal static object CreateEnum(RuntimeTypeHandle enumType, long value)
		{
			return RuntimeType._CreateEnum(enumType.Value, value);
		}

		// Token: 0x06000E5E RID: 3678
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object InvokeDispMethod(string name, BindingFlags invokeAttr, object target, object[] args, bool[] byrefModifiers, int culture, string[] namedParameters);

		// Token: 0x06000E5F RID: 3679
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Type GetTypeFromProgIDImpl(string progID, string server, bool throwOnError);

		// Token: 0x06000E60 RID: 3680
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Type GetTypeFromCLSIDImpl(Guid clsid, string server, bool throwOnError);

		// Token: 0x06000E61 RID: 3681 RVA: 0x00029EDC File Offset: 0x00028EDC
		internal static Type PrivateGetType(string typeName, bool throwOnError, bool ignoreCase, ref StackCrawlMark stackMark)
		{
			return RuntimeType.PrivateGetType(typeName, throwOnError, ignoreCase, false, ref stackMark);
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x00029EE8 File Offset: 0x00028EE8
		internal static Type PrivateGetType(string typeName, bool throwOnError, bool ignoreCase, bool reflectionOnly, ref StackCrawlMark stackMark)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("TypeName");
			}
			return RuntimeTypeHandle.GetTypeByName(typeName, throwOnError, ignoreCase, reflectionOnly, ref stackMark).GetRuntimeType();
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x00029F18 File Offset: 0x00028F18
		private object ForwardCallToInvokeMember(string memberName, BindingFlags flags, object target, int[] aWrapperTypes, ref MessageData msgData)
		{
			ParameterModifier[] array = null;
			object obj = null;
			Message message = new Message();
			message.InitFields(msgData);
			MethodInfo methodInfo = (MethodInfo)message.GetMethodBase();
			object[] args = message.Args;
			int num = args.Length;
			ParameterInfo[] parametersNoCopy = methodInfo.GetParametersNoCopy();
			if (num > 0)
			{
				ParameterModifier parameterModifier = new ParameterModifier(num);
				for (int i = 0; i < num; i++)
				{
					if (parametersNoCopy[i].ParameterType.IsByRef)
					{
						parameterModifier[i] = true;
					}
				}
				array = new ParameterModifier[] { parameterModifier };
				if (aWrapperTypes != null)
				{
					this.WrapArgsForInvokeCall(args, aWrapperTypes);
				}
			}
			if (methodInfo.ReturnType == typeof(void))
			{
				flags |= BindingFlags.IgnoreReturn;
			}
			try
			{
				obj = this.InvokeMember(memberName, flags, null, target, args, array, null, null);
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
			for (int j = 0; j < num; j++)
			{
				if (array[0][j] && args[j] != null)
				{
					Type elementType = parametersNoCopy[j].ParameterType.GetElementType();
					if (elementType != args[j].GetType())
					{
						args[j] = this.ForwardCallBinder.ChangeType(args[j], elementType, null);
					}
				}
			}
			if (obj != null)
			{
				Type returnType = methodInfo.ReturnType;
				if (returnType != obj.GetType())
				{
					obj = this.ForwardCallBinder.ChangeType(obj, returnType, null);
				}
			}
			RealProxy.PropagateOutParameters(message, args, obj);
			return obj;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0002A090 File Offset: 0x00029090
		private void WrapArgsForInvokeCall(object[] aArgs, int[] aWrapperTypes)
		{
			int num = aArgs.Length;
			for (int i = 0; i < num; i++)
			{
				if (aWrapperTypes[i] != 0)
				{
					if ((aWrapperTypes[i] & 65536) != 0)
					{
						Type type = null;
						bool flag = false;
						RuntimeType.DispatchWrapperType dispatchWrapperType = (RuntimeType.DispatchWrapperType)(aWrapperTypes[i] & -65537);
						if (dispatchWrapperType <= RuntimeType.DispatchWrapperType.Error)
						{
							switch (dispatchWrapperType)
							{
							case RuntimeType.DispatchWrapperType.Unknown:
								type = typeof(UnknownWrapper);
								break;
							case RuntimeType.DispatchWrapperType.Dispatch:
								type = typeof(DispatchWrapper);
								break;
							default:
								if (dispatchWrapperType == RuntimeType.DispatchWrapperType.Error)
								{
									type = typeof(ErrorWrapper);
								}
								break;
							}
						}
						else if (dispatchWrapperType != RuntimeType.DispatchWrapperType.Currency)
						{
							if (dispatchWrapperType == RuntimeType.DispatchWrapperType.BStr)
							{
								type = typeof(BStrWrapper);
								flag = true;
							}
						}
						else
						{
							type = typeof(CurrencyWrapper);
						}
						Array array = (Array)aArgs[i];
						int length = array.Length;
						object[] array2 = (object[])Array.CreateInstance(type, length);
						ConstructorInfo constructorInfo;
						if (flag)
						{
							constructorInfo = type.GetConstructor(new Type[] { typeof(string) });
						}
						else
						{
							constructorInfo = type.GetConstructor(new Type[] { typeof(object) });
						}
						for (int j = 0; j < length; j++)
						{
							if (flag)
							{
								array2[j] = constructorInfo.Invoke(new object[] { (string)array.GetValue(j) });
							}
							else
							{
								array2[j] = constructorInfo.Invoke(new object[] { array.GetValue(j) });
							}
						}
						aArgs[i] = array2;
					}
					else
					{
						RuntimeType.DispatchWrapperType dispatchWrapperType2 = (RuntimeType.DispatchWrapperType)aWrapperTypes[i];
						if (dispatchWrapperType2 <= RuntimeType.DispatchWrapperType.Error)
						{
							switch (dispatchWrapperType2)
							{
							case RuntimeType.DispatchWrapperType.Unknown:
								aArgs[i] = new UnknownWrapper(aArgs[i]);
								break;
							case RuntimeType.DispatchWrapperType.Dispatch:
								aArgs[i] = new DispatchWrapper(aArgs[i]);
								break;
							default:
								if (dispatchWrapperType2 == RuntimeType.DispatchWrapperType.Error)
								{
									aArgs[i] = new ErrorWrapper(aArgs[i]);
								}
								break;
							}
						}
						else if (dispatchWrapperType2 != RuntimeType.DispatchWrapperType.Currency)
						{
							if (dispatchWrapperType2 == RuntimeType.DispatchWrapperType.BStr)
							{
								aArgs[i] = new BStrWrapper((string)aArgs[i]);
							}
						}
						else
						{
							aArgs[i] = new CurrencyWrapper(aArgs[i]);
						}
					}
				}
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x0002A287 File Offset: 0x00029287
		private OleAutBinder ForwardCallBinder
		{
			get
			{
				if (RuntimeType.s_ForwardCallBinder == null)
				{
					RuntimeType.s_ForwardCallBinder = new OleAutBinder();
				}
				return RuntimeType.s_ForwardCallBinder;
			}
		}

		// Token: 0x0400049F RID: 1183
		private const BindingFlags MemberBindingMask = (BindingFlags)255;

		// Token: 0x040004A0 RID: 1184
		private const BindingFlags InvocationMask = BindingFlags.InvokeMethod | BindingFlags.CreateInstance | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty;

		// Token: 0x040004A1 RID: 1185
		private const BindingFlags BinderNonCreateInstance = BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

		// Token: 0x040004A2 RID: 1186
		private const BindingFlags BinderGetSetProperty = BindingFlags.GetProperty | BindingFlags.SetProperty;

		// Token: 0x040004A3 RID: 1187
		private const BindingFlags BinderSetInvokeProperty = BindingFlags.InvokeMethod | BindingFlags.SetProperty;

		// Token: 0x040004A4 RID: 1188
		private const BindingFlags BinderGetSetField = BindingFlags.GetField | BindingFlags.SetField;

		// Token: 0x040004A5 RID: 1189
		private const BindingFlags BinderSetInvokeField = BindingFlags.InvokeMethod | BindingFlags.SetField;

		// Token: 0x040004A6 RID: 1190
		private const BindingFlags BinderNonFieldGetSet = (BindingFlags)16773888;

		// Token: 0x040004A7 RID: 1191
		private const BindingFlags ClassicBindingMask = BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty;

		// Token: 0x040004A8 RID: 1192
		private IntPtr m_cache;

		// Token: 0x040004A9 RID: 1193
		private RuntimeTypeHandle m_handle;

		// Token: 0x040004AA RID: 1194
		private static RuntimeType.TypeCacheQueue s_typeCache = null;

		// Token: 0x040004AB RID: 1195
		private static Type s_typedRef = typeof(TypedReference);

		// Token: 0x040004AC RID: 1196
		private static bool forceInvokingWithEnUS = RuntimeType.ForceEnUSLcidComInvoking();

		// Token: 0x040004AD RID: 1197
		private static RuntimeType.ActivatorCache s_ActivatorCache;

		// Token: 0x040004AE RID: 1198
		private static OleAutBinder s_ForwardCallBinder;

		// Token: 0x020000F5 RID: 245
		[Serializable]
		internal class RuntimeTypeCache
		{
			// Token: 0x06000E67 RID: 3687 RVA: 0x0002A2C0 File Offset: 0x000292C0
			internal static void Prejitinit_HACK()
			{
				if (!RuntimeType.RuntimeTypeCache.s_dontrunhack)
				{
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
					}
					finally
					{
						RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeMethodInfo> memberInfoCache = new RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeMethodInfo>(null);
						CerArrayList<RuntimeMethodInfo> cerArrayList = null;
						memberInfoCache.Insert(ref cerArrayList, "dummy", MemberListType.All);
						RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeConstructorInfo> memberInfoCache2 = new RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeConstructorInfo>(null);
						CerArrayList<RuntimeConstructorInfo> cerArrayList2 = null;
						memberInfoCache2.Insert(ref cerArrayList2, "dummy", MemberListType.All);
						RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeFieldInfo> memberInfoCache3 = new RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeFieldInfo>(null);
						CerArrayList<RuntimeFieldInfo> cerArrayList3 = null;
						memberInfoCache3.Insert(ref cerArrayList3, "dummy", MemberListType.All);
						RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeType> memberInfoCache4 = new RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeType>(null);
						CerArrayList<RuntimeType> cerArrayList4 = null;
						memberInfoCache4.Insert(ref cerArrayList4, "dummy", MemberListType.All);
						RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimePropertyInfo> memberInfoCache5 = new RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimePropertyInfo>(null);
						CerArrayList<RuntimePropertyInfo> cerArrayList5 = null;
						memberInfoCache5.Insert(ref cerArrayList5, "dummy", MemberListType.All);
						RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeEventInfo> memberInfoCache6 = new RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeEventInfo>(null);
						CerArrayList<RuntimeEventInfo> cerArrayList6 = null;
						memberInfoCache6.Insert(ref cerArrayList6, "dummy", MemberListType.All);
					}
				}
			}

			// Token: 0x06000E68 RID: 3688 RVA: 0x0002A388 File Offset: 0x00029388
			internal RuntimeTypeCache(RuntimeType runtimeType)
			{
				this.m_typeCode = TypeCode.Empty;
				this.m_runtimeType = runtimeType;
				this.m_runtimeTypeHandle = runtimeType.GetTypeHandleInternal();
				this.m_isGlobal = this.m_runtimeTypeHandle.GetModuleHandle().GetModuleTypeHandle().Equals(this.m_runtimeTypeHandle);
				RuntimeType.RuntimeTypeCache.s_dontrunhack = true;
				RuntimeType.RuntimeTypeCache.Prejitinit_HACK();
			}

			// Token: 0x06000E69 RID: 3689 RVA: 0x0002A3E8 File Offset: 0x000293E8
			private string ConstructName(ref string name, bool nameSpace, bool fullinst, bool assembly)
			{
				if (name == null)
				{
					name = this.RuntimeTypeHandle.ConstructName(nameSpace, fullinst, assembly);
				}
				return name;
			}

			// Token: 0x06000E6A RID: 3690 RVA: 0x0002A410 File Offset: 0x00029410
			private CerArrayList<T> GetMemberList<T>(ref RuntimeType.RuntimeTypeCache.MemberInfoCache<T> m_cache, MemberListType listType, string name, RuntimeType.RuntimeTypeCache.CacheType cacheType) where T : MemberInfo
			{
				RuntimeType.RuntimeTypeCache.MemberInfoCache<T> memberCache = this.GetMemberCache<T>(ref m_cache);
				return memberCache.GetMemberList(listType, name, cacheType);
			}

			// Token: 0x06000E6B RID: 3691 RVA: 0x0002A430 File Offset: 0x00029430
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<T> GetMemberCache<T>(ref RuntimeType.RuntimeTypeCache.MemberInfoCache<T> m_cache) where T : MemberInfo
			{
				RuntimeType.RuntimeTypeCache.MemberInfoCache<T> memberInfoCache = m_cache;
				if (memberInfoCache == null)
				{
					RuntimeType.RuntimeTypeCache.MemberInfoCache<T> memberInfoCache2 = new RuntimeType.RuntimeTypeCache.MemberInfoCache<T>(this);
					memberInfoCache = Interlocked.CompareExchange<RuntimeType.RuntimeTypeCache.MemberInfoCache<T>>(ref m_cache, memberInfoCache2, null);
					if (memberInfoCache == null)
					{
						memberInfoCache = memberInfoCache2;
					}
				}
				return memberInfoCache;
			}

			// Token: 0x170001CC RID: 460
			// (get) Token: 0x06000E6C RID: 3692 RVA: 0x0002A459 File Offset: 0x00029459
			// (set) Token: 0x06000E6D RID: 3693 RVA: 0x0002A461 File Offset: 0x00029461
			internal bool DomainInitialized
			{
				get
				{
					return this.m_bIsDomainInitialized;
				}
				set
				{
					this.m_bIsDomainInitialized = value;
				}
			}

			// Token: 0x06000E6E RID: 3694 RVA: 0x0002A46A File Offset: 0x0002946A
			internal string GetName()
			{
				return this.ConstructName(ref this.m_name, false, false, false);
			}

			// Token: 0x06000E6F RID: 3695 RVA: 0x0002A47C File Offset: 0x0002947C
			internal string GetNameSpace()
			{
				if (this.m_namespace == null)
				{
					Type type = this.m_runtimeType;
					type = type.GetRootElementType();
					while (type.IsNested)
					{
						type = type.DeclaringType;
					}
					this.m_namespace = type.GetTypeHandleInternal().GetModuleHandle().GetMetadataImport()
						.GetNamespace(type.MetadataToken)
						.ToString();
				}
				return this.m_namespace;
			}

			// Token: 0x06000E70 RID: 3696 RVA: 0x0002A4EF File Offset: 0x000294EF
			internal string GetToString()
			{
				return this.ConstructName(ref this.m_toString, true, false, false);
			}

			// Token: 0x06000E71 RID: 3697 RVA: 0x0002A500 File Offset: 0x00029500
			internal string GetFullName()
			{
				if (!this.m_runtimeType.IsGenericTypeDefinition && this.m_runtimeType.ContainsGenericParameters)
				{
					return null;
				}
				return this.ConstructName(ref this.m_fullname, true, true, false);
			}

			// Token: 0x170001CD RID: 461
			// (get) Token: 0x06000E72 RID: 3698 RVA: 0x0002A52D File Offset: 0x0002952D
			// (set) Token: 0x06000E73 RID: 3699 RVA: 0x0002A535 File Offset: 0x00029535
			internal TypeCode TypeCode
			{
				get
				{
					return this.m_typeCode;
				}
				set
				{
					this.m_typeCode = value;
				}
			}

			// Token: 0x06000E74 RID: 3700 RVA: 0x0002A540 File Offset: 0x00029540
			internal RuntimeType GetEnclosingType()
			{
				if ((this.m_whatsCached & RuntimeType.RuntimeTypeCache.WhatsCached.EnclosingType) == RuntimeType.RuntimeTypeCache.WhatsCached.Nothing)
				{
					this.m_enclosingType = this.RuntimeTypeHandle.GetDeclaringType().GetRuntimeType();
					this.m_whatsCached |= RuntimeType.RuntimeTypeCache.WhatsCached.EnclosingType;
				}
				return this.m_enclosingType;
			}

			// Token: 0x170001CE RID: 462
			// (get) Token: 0x06000E75 RID: 3701 RVA: 0x0002A587 File Offset: 0x00029587
			internal bool IsGlobal
			{
				[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
				get
				{
					return this.m_isGlobal;
				}
			}

			// Token: 0x170001CF RID: 463
			// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0002A58F File Offset: 0x0002958F
			internal RuntimeType RuntimeType
			{
				get
				{
					return this.m_runtimeType;
				}
			}

			// Token: 0x170001D0 RID: 464
			// (get) Token: 0x06000E77 RID: 3703 RVA: 0x0002A597 File Offset: 0x00029597
			internal RuntimeTypeHandle RuntimeTypeHandle
			{
				get
				{
					return this.m_runtimeTypeHandle;
				}
			}

			// Token: 0x06000E78 RID: 3704 RVA: 0x0002A59F File Offset: 0x0002959F
			internal void InvalidateCachedNestedType()
			{
				this.m_nestedClassesCache = null;
			}

			// Token: 0x06000E79 RID: 3705 RVA: 0x0002A5A8 File Offset: 0x000295A8
			internal MethodInfo GetGenericMethodInfo(RuntimeMethodHandle genericMethod)
			{
				if (RuntimeType.RuntimeTypeCache.s_methodInstantiations == null)
				{
					Interlocked.CompareExchange<CerHashtable<RuntimeMethodInfo, RuntimeMethodInfo>>(ref RuntimeType.RuntimeTypeCache.s_methodInstantiations, new CerHashtable<RuntimeMethodInfo, RuntimeMethodInfo>(), null);
				}
				RuntimeMethodInfo runtimeMethodInfo = new RuntimeMethodInfo(genericMethod, genericMethod.GetDeclaringType(), this, genericMethod.GetAttributes(), (BindingFlags)(-1));
				RuntimeMethodInfo runtimeMethodInfo2 = RuntimeType.RuntimeTypeCache.s_methodInstantiations[runtimeMethodInfo];
				if (runtimeMethodInfo2 != null)
				{
					return runtimeMethodInfo2;
				}
				bool flag = false;
				bool flag2 = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.ReliableEnter(RuntimeType.RuntimeTypeCache.s_methodInstantiations, ref flag);
					runtimeMethodInfo2 = RuntimeType.RuntimeTypeCache.s_methodInstantiations[runtimeMethodInfo];
					if (runtimeMethodInfo2 != null)
					{
						return runtimeMethodInfo2;
					}
					RuntimeType.RuntimeTypeCache.s_methodInstantiations.Preallocate(1);
					flag2 = true;
				}
				finally
				{
					if (flag2)
					{
						RuntimeType.RuntimeTypeCache.s_methodInstantiations[runtimeMethodInfo] = runtimeMethodInfo;
					}
					if (flag)
					{
						Monitor.Exit(RuntimeType.RuntimeTypeCache.s_methodInstantiations);
					}
				}
				return runtimeMethodInfo;
			}

			// Token: 0x06000E7A RID: 3706 RVA: 0x0002A660 File Offset: 0x00029660
			internal CerArrayList<RuntimeMethodInfo> GetMethodList(MemberListType listType, string name)
			{
				return this.GetMemberList<RuntimeMethodInfo>(ref this.m_methodInfoCache, listType, name, RuntimeType.RuntimeTypeCache.CacheType.Method);
			}

			// Token: 0x06000E7B RID: 3707 RVA: 0x0002A671 File Offset: 0x00029671
			internal CerArrayList<RuntimeConstructorInfo> GetConstructorList(MemberListType listType, string name)
			{
				return this.GetMemberList<RuntimeConstructorInfo>(ref this.m_constructorInfoCache, listType, name, RuntimeType.RuntimeTypeCache.CacheType.Constructor);
			}

			// Token: 0x06000E7C RID: 3708 RVA: 0x0002A682 File Offset: 0x00029682
			internal CerArrayList<RuntimePropertyInfo> GetPropertyList(MemberListType listType, string name)
			{
				return this.GetMemberList<RuntimePropertyInfo>(ref this.m_propertyInfoCache, listType, name, RuntimeType.RuntimeTypeCache.CacheType.Property);
			}

			// Token: 0x06000E7D RID: 3709 RVA: 0x0002A693 File Offset: 0x00029693
			internal CerArrayList<RuntimeEventInfo> GetEventList(MemberListType listType, string name)
			{
				return this.GetMemberList<RuntimeEventInfo>(ref this.m_eventInfoCache, listType, name, RuntimeType.RuntimeTypeCache.CacheType.Event);
			}

			// Token: 0x06000E7E RID: 3710 RVA: 0x0002A6A4 File Offset: 0x000296A4
			internal CerArrayList<RuntimeFieldInfo> GetFieldList(MemberListType listType, string name)
			{
				return this.GetMemberList<RuntimeFieldInfo>(ref this.m_fieldInfoCache, listType, name, RuntimeType.RuntimeTypeCache.CacheType.Field);
			}

			// Token: 0x06000E7F RID: 3711 RVA: 0x0002A6B5 File Offset: 0x000296B5
			internal CerArrayList<RuntimeType> GetInterfaceList(MemberListType listType, string name)
			{
				return this.GetMemberList<RuntimeType>(ref this.m_interfaceCache, listType, name, RuntimeType.RuntimeTypeCache.CacheType.Interface);
			}

			// Token: 0x06000E80 RID: 3712 RVA: 0x0002A6C6 File Offset: 0x000296C6
			internal CerArrayList<RuntimeType> GetNestedTypeList(MemberListType listType, string name)
			{
				return this.GetMemberList<RuntimeType>(ref this.m_nestedClassesCache, listType, name, RuntimeType.RuntimeTypeCache.CacheType.NestedType);
			}

			// Token: 0x06000E81 RID: 3713 RVA: 0x0002A6D7 File Offset: 0x000296D7
			internal MethodBase GetMethod(RuntimeTypeHandle declaringType, RuntimeMethodHandle method)
			{
				this.GetMemberCache<RuntimeMethodInfo>(ref this.m_methodInfoCache);
				return this.m_methodInfoCache.AddMethod(declaringType, method, RuntimeType.RuntimeTypeCache.CacheType.Method);
			}

			// Token: 0x06000E82 RID: 3714 RVA: 0x0002A6F4 File Offset: 0x000296F4
			internal MethodBase GetConstructor(RuntimeTypeHandle declaringType, RuntimeMethodHandle constructor)
			{
				this.GetMemberCache<RuntimeConstructorInfo>(ref this.m_constructorInfoCache);
				return this.m_constructorInfoCache.AddMethod(declaringType, constructor, RuntimeType.RuntimeTypeCache.CacheType.Constructor);
			}

			// Token: 0x06000E83 RID: 3715 RVA: 0x0002A711 File Offset: 0x00029711
			internal FieldInfo GetField(RuntimeFieldHandle field)
			{
				this.GetMemberCache<RuntimeFieldInfo>(ref this.m_fieldInfoCache);
				return this.m_fieldInfoCache.AddField(field);
			}

			// Token: 0x040004AF RID: 1199
			private RuntimeType.RuntimeTypeCache.WhatsCached m_whatsCached;

			// Token: 0x040004B0 RID: 1200
			private RuntimeTypeHandle m_runtimeTypeHandle;

			// Token: 0x040004B1 RID: 1201
			private RuntimeType m_runtimeType;

			// Token: 0x040004B2 RID: 1202
			private RuntimeType m_enclosingType;

			// Token: 0x040004B3 RID: 1203
			private TypeCode m_typeCode;

			// Token: 0x040004B4 RID: 1204
			private string m_name;

			// Token: 0x040004B5 RID: 1205
			private string m_fullname;

			// Token: 0x040004B6 RID: 1206
			private string m_toString;

			// Token: 0x040004B7 RID: 1207
			private string m_namespace;

			// Token: 0x040004B8 RID: 1208
			private bool m_isGlobal;

			// Token: 0x040004B9 RID: 1209
			private bool m_bIsDomainInitialized;

			// Token: 0x040004BA RID: 1210
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeMethodInfo> m_methodInfoCache;

			// Token: 0x040004BB RID: 1211
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeConstructorInfo> m_constructorInfoCache;

			// Token: 0x040004BC RID: 1212
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeFieldInfo> m_fieldInfoCache;

			// Token: 0x040004BD RID: 1213
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeType> m_interfaceCache;

			// Token: 0x040004BE RID: 1214
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeType> m_nestedClassesCache;

			// Token: 0x040004BF RID: 1215
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimePropertyInfo> m_propertyInfoCache;

			// Token: 0x040004C0 RID: 1216
			private RuntimeType.RuntimeTypeCache.MemberInfoCache<RuntimeEventInfo> m_eventInfoCache;

			// Token: 0x040004C1 RID: 1217
			private static CerHashtable<RuntimeMethodInfo, RuntimeMethodInfo> s_methodInstantiations;

			// Token: 0x040004C2 RID: 1218
			private static bool s_dontrunhack;

			// Token: 0x020000F6 RID: 246
			internal enum WhatsCached
			{
				// Token: 0x040004C4 RID: 1220
				Nothing,
				// Token: 0x040004C5 RID: 1221
				EnclosingType
			}

			// Token: 0x020000F7 RID: 247
			internal enum CacheType
			{
				// Token: 0x040004C7 RID: 1223
				Method,
				// Token: 0x040004C8 RID: 1224
				Constructor,
				// Token: 0x040004C9 RID: 1225
				Field,
				// Token: 0x040004CA RID: 1226
				Property,
				// Token: 0x040004CB RID: 1227
				Event,
				// Token: 0x040004CC RID: 1228
				Interface,
				// Token: 0x040004CD RID: 1229
				NestedType
			}

			// Token: 0x020000F8 RID: 248
			private struct Filter
			{
				// Token: 0x06000E84 RID: 3716 RVA: 0x0002A72C File Offset: 0x0002972C
				public unsafe Filter(byte* pUtf8Name, int cUtf8Name, MemberListType listType)
				{
					this.m_name = new Utf8String((void*)pUtf8Name, cUtf8Name);
					this.m_listType = listType;
				}

				// Token: 0x06000E85 RID: 3717 RVA: 0x0002A742 File Offset: 0x00029742
				public bool Match(Utf8String name)
				{
					if (this.m_listType == MemberListType.CaseSensitive)
					{
						return this.m_name.Equals(name);
					}
					return this.m_listType != MemberListType.CaseInsensitive || this.m_name.EqualsCaseInsensitive(name);
				}

				// Token: 0x040004CE RID: 1230
				private Utf8String m_name;

				// Token: 0x040004CF RID: 1231
				private MemberListType m_listType;
			}

			// Token: 0x020000F9 RID: 249
			[Serializable]
			private class MemberInfoCache<T> where T : MemberInfo
			{
				// Token: 0x06000E86 RID: 3718 RVA: 0x0002A771 File Offset: 0x00029771
				static MemberInfoCache()
				{
					RuntimeType.PrepareMemberInfoCache(typeof(RuntimeType.RuntimeTypeCache.MemberInfoCache<T>).TypeHandle);
				}

				// Token: 0x06000E87 RID: 3719 RVA: 0x0002A787 File Offset: 0x00029787
				internal MemberInfoCache(RuntimeType.RuntimeTypeCache runtimeTypeCache)
				{
					Mda.MemberInfoCacheCreation();
					this.m_runtimeTypeCache = runtimeTypeCache;
					this.m_cacheComplete = false;
				}

				// Token: 0x06000E88 RID: 3720 RVA: 0x0002A7A4 File Offset: 0x000297A4
				internal MethodBase AddMethod(RuntimeTypeHandle declaringType, RuntimeMethodHandle method, RuntimeType.RuntimeTypeCache.CacheType cacheType)
				{
					object obj = null;
					MethodAttributes attributes = method.GetAttributes();
					bool flag = (attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;
					bool flag2 = (attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope;
					bool flag3 = declaringType.Value != this.ReflectedTypeHandle.Value;
					BindingFlags bindingFlags = RuntimeType.FilterPreCalculate(flag, flag3, flag2);
					switch (cacheType)
					{
					case RuntimeType.RuntimeTypeCache.CacheType.Method:
						obj = new List<RuntimeMethodInfo>(1)
						{
							new RuntimeMethodInfo(method, declaringType, this.m_runtimeTypeCache, attributes, bindingFlags)
						};
						break;
					case RuntimeType.RuntimeTypeCache.CacheType.Constructor:
						obj = new List<RuntimeConstructorInfo>(1)
						{
							new RuntimeConstructorInfo(method, declaringType, this.m_runtimeTypeCache, attributes, bindingFlags)
						};
						break;
					}
					CerArrayList<T> cerArrayList = new CerArrayList<T>((List<T>)obj);
					this.Insert(ref cerArrayList, null, MemberListType.HandleToInfo);
					return (MethodBase)((object)cerArrayList[0]);
				}

				// Token: 0x06000E89 RID: 3721 RVA: 0x0002A878 File Offset: 0x00029878
				internal FieldInfo AddField(RuntimeFieldHandle field)
				{
					List<RuntimeFieldInfo> list = new List<RuntimeFieldInfo>(1);
					FieldAttributes attributes = field.GetAttributes();
					bool flag = (attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public;
					bool flag2 = (attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope;
					bool flag3 = field.GetApproxDeclaringType().Value != this.ReflectedTypeHandle.Value;
					BindingFlags bindingFlags = RuntimeType.FilterPreCalculate(flag, flag3, flag2);
					list.Add(new RtFieldInfo(field, this.ReflectedType, this.m_runtimeTypeCache, bindingFlags));
					CerArrayList<T> cerArrayList = new CerArrayList<T>((List<T>)list);
					this.Insert(ref cerArrayList, null, MemberListType.HandleToInfo);
					return (FieldInfo)((object)cerArrayList[0]);
				}

				// Token: 0x06000E8A RID: 3722 RVA: 0x0002A91C File Offset: 0x0002991C
				private unsafe CerArrayList<T> Populate(string name, MemberListType listType, RuntimeType.RuntimeTypeCache.CacheType cacheType)
				{
					if (name == null || name.Length == 0 || (cacheType == RuntimeType.RuntimeTypeCache.CacheType.Constructor && name.FirstChar != '.' && name.FirstChar != '*'))
					{
						RuntimeType.RuntimeTypeCache.Filter filter = new RuntimeType.RuntimeTypeCache.Filter(null, 0, listType);
						List<T> list = null;
						switch (cacheType)
						{
						case RuntimeType.RuntimeTypeCache.CacheType.Method:
							list = this.PopulateMethods(filter) as List<T>;
							break;
						case RuntimeType.RuntimeTypeCache.CacheType.Constructor:
							list = this.PopulateConstructors(filter) as List<T>;
							break;
						case RuntimeType.RuntimeTypeCache.CacheType.Field:
							list = this.PopulateFields(filter) as List<T>;
							break;
						case RuntimeType.RuntimeTypeCache.CacheType.Property:
							list = this.PopulateProperties(filter) as List<T>;
							break;
						case RuntimeType.RuntimeTypeCache.CacheType.Event:
							list = this.PopulateEvents(filter) as List<T>;
							break;
						case RuntimeType.RuntimeTypeCache.CacheType.Interface:
							list = this.PopulateInterfaces(filter) as List<T>;
							break;
						case RuntimeType.RuntimeTypeCache.CacheType.NestedType:
							list = this.PopulateNestedClasses(filter) as List<T>;
							break;
						}
						CerArrayList<T> cerArrayList = new CerArrayList<T>(list);
						this.Insert(ref cerArrayList, name, listType);
						return cerArrayList;
					}
					IntPtr intPtr2;
					IntPtr intPtr = (intPtr2 = name);
					if (intPtr != 0)
					{
						intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
					}
					char* ptr = intPtr2;
					int byteCount = Encoding.UTF8.GetByteCount(ptr, name.Length);
					byte* ptr2 = stackalloc byte[1 * byteCount];
					Encoding.UTF8.GetBytes(ptr, name.Length, ptr2, byteCount);
					RuntimeType.RuntimeTypeCache.Filter filter2 = new RuntimeType.RuntimeTypeCache.Filter(ptr2, byteCount, listType);
					List<T> list2 = null;
					switch (cacheType)
					{
					case RuntimeType.RuntimeTypeCache.CacheType.Method:
						list2 = this.PopulateMethods(filter2) as List<T>;
						break;
					case RuntimeType.RuntimeTypeCache.CacheType.Constructor:
						list2 = this.PopulateConstructors(filter2) as List<T>;
						break;
					case RuntimeType.RuntimeTypeCache.CacheType.Field:
						list2 = this.PopulateFields(filter2) as List<T>;
						break;
					case RuntimeType.RuntimeTypeCache.CacheType.Property:
						list2 = this.PopulateProperties(filter2) as List<T>;
						break;
					case RuntimeType.RuntimeTypeCache.CacheType.Event:
						list2 = this.PopulateEvents(filter2) as List<T>;
						break;
					case RuntimeType.RuntimeTypeCache.CacheType.Interface:
						list2 = this.PopulateInterfaces(filter2) as List<T>;
						break;
					case RuntimeType.RuntimeTypeCache.CacheType.NestedType:
						list2 = this.PopulateNestedClasses(filter2) as List<T>;
						break;
					}
					CerArrayList<T> cerArrayList2 = new CerArrayList<T>(list2);
					this.Insert(ref cerArrayList2, name, listType);
					return cerArrayList2;
				}

				// Token: 0x06000E8B RID: 3723 RVA: 0x0002AB0C File Offset: 0x00029B0C
				[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
				internal void Insert(ref CerArrayList<T> list, string name, MemberListType listType)
				{
					bool flag = false;
					bool flag2 = false;
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						Monitor.ReliableEnter(this, ref flag);
						if (listType == MemberListType.CaseSensitive)
						{
							if (this.m_csMemberInfos == null)
							{
								this.m_csMemberInfos = new CerHashtable<string, CerArrayList<T>>();
							}
							else
							{
								this.m_csMemberInfos.Preallocate(1);
							}
						}
						else if (listType == MemberListType.CaseInsensitive)
						{
							if (this.m_cisMemberInfos == null)
							{
								this.m_cisMemberInfos = new CerHashtable<string, CerArrayList<T>>();
							}
							else
							{
								this.m_cisMemberInfos.Preallocate(1);
							}
						}
						if (this.m_root == null)
						{
							this.m_root = new CerArrayList<T>(list.Count);
						}
						else
						{
							this.m_root.Preallocate(list.Count);
						}
						flag2 = true;
					}
					finally
					{
						try
						{
							if (flag2)
							{
								if (listType == MemberListType.CaseSensitive)
								{
									CerArrayList<T> cerArrayList = this.m_csMemberInfos[name];
									if (cerArrayList == null)
									{
										this.MergeWithGlobalList(list);
										this.m_csMemberInfos[name] = list;
									}
									else
									{
										list = cerArrayList;
									}
								}
								else if (listType == MemberListType.CaseInsensitive)
								{
									CerArrayList<T> cerArrayList2 = this.m_cisMemberInfos[name];
									if (cerArrayList2 == null)
									{
										this.MergeWithGlobalList(list);
										this.m_cisMemberInfos[name] = list;
									}
									else
									{
										list = cerArrayList2;
									}
								}
								else
								{
									this.MergeWithGlobalList(list);
								}
								if (listType == MemberListType.All)
								{
									this.m_cacheComplete = true;
								}
							}
						}
						finally
						{
							if (flag)
							{
								Monitor.Exit(this);
							}
						}
					}
				}

				// Token: 0x06000E8C RID: 3724 RVA: 0x0002AC4C File Offset: 0x00029C4C
				[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
				private void MergeWithGlobalList(CerArrayList<T> list)
				{
					int count = this.m_root.Count;
					for (int i = 0; i < list.Count; i++)
					{
						T t = list[i];
						T t2 = default(T);
						for (int j = 0; j < count; j++)
						{
							t2 = this.m_root[j];
							if (t.CacheEquals(t2))
							{
								list.Replace(i, t2);
								break;
							}
						}
						if (list[i] != t2)
						{
							this.m_root.Add(t);
						}
					}
				}

				// Token: 0x06000E8D RID: 3725 RVA: 0x0002ACE4 File Offset: 0x00029CE4
				private unsafe List<RuntimeMethodInfo> PopulateMethods(RuntimeType.RuntimeTypeCache.Filter filter)
				{
					List<RuntimeMethodInfo> list = new List<RuntimeMethodInfo>();
					RuntimeTypeHandle runtimeTypeHandle = this.ReflectedTypeHandle;
					bool flag = (runtimeTypeHandle.GetAttributes() & TypeAttributes.ClassSemanticsMask) == TypeAttributes.ClassSemanticsMask;
					if (flag)
					{
						bool flag2 = runtimeTypeHandle.HasInstantiation() && !runtimeTypeHandle.IsGenericTypeDefinition();
						foreach (RuntimeMethodHandle runtimeMethodHandle in runtimeTypeHandle.IntroducedMethods)
						{
							if (filter.Match(runtimeMethodHandle.GetUtf8Name()))
							{
								MethodAttributes attributes = runtimeMethodHandle.GetAttributes();
								bool flag3 = (attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;
								bool flag4 = (attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope;
								bool flag5 = false;
								BindingFlags bindingFlags = RuntimeType.FilterPreCalculate(flag3, flag5, flag4);
								if ((attributes & MethodAttributes.RTSpecialName) == MethodAttributes.PrivateScope && !runtimeMethodHandle.IsILStub())
								{
									RuntimeMethodHandle runtimeMethodHandle2 = (flag2 ? runtimeMethodHandle.GetInstantiatingStubIfNeeded(runtimeTypeHandle) : runtimeMethodHandle);
									RuntimeMethodInfo runtimeMethodInfo = new RuntimeMethodInfo(runtimeMethodHandle2, runtimeTypeHandle, this.m_runtimeTypeCache, attributes, bindingFlags);
									list.Add(runtimeMethodInfo);
								}
							}
						}
					}
					else
					{
						while (runtimeTypeHandle.IsGenericVariable())
						{
							runtimeTypeHandle = runtimeTypeHandle.GetRuntimeType().BaseType.GetTypeHandleInternal();
						}
						bool* ptr = stackalloc bool[1 * runtimeTypeHandle.GetNumVirtuals()];
						bool isValueType = runtimeTypeHandle.GetRuntimeType().IsValueType;
						while (!runtimeTypeHandle.IsNullHandle())
						{
							bool flag6 = runtimeTypeHandle.HasInstantiation() && !runtimeTypeHandle.IsGenericTypeDefinition();
							int numVirtuals = runtimeTypeHandle.GetNumVirtuals();
							foreach (RuntimeMethodHandle runtimeMethodHandle3 in runtimeTypeHandle.IntroducedMethods)
							{
								if (filter.Match(runtimeMethodHandle3.GetUtf8Name()))
								{
									MethodAttributes attributes2 = runtimeMethodHandle3.GetAttributes();
									MethodAttributes methodAttributes = attributes2 & MethodAttributes.MemberAccessMask;
									if ((attributes2 & MethodAttributes.RTSpecialName) == MethodAttributes.PrivateScope && !runtimeMethodHandle3.IsILStub())
									{
										bool flag7 = false;
										int num = 0;
										if ((attributes2 & MethodAttributes.Virtual) != MethodAttributes.PrivateScope)
										{
											num = runtimeMethodHandle3.GetSlot();
											flag7 = num < numVirtuals;
										}
										bool flag8 = methodAttributes == MethodAttributes.Private;
										bool flag9 = flag7 && flag8;
										bool flag10 = runtimeTypeHandle.Value != this.ReflectedTypeHandle.Value;
										if (!flag10 || !flag8 || flag9)
										{
											if (flag7)
											{
												if (ptr[num])
												{
													continue;
												}
												ptr[num] = true;
											}
											else if (isValueType && (attributes2 & (MethodAttributes.Virtual | MethodAttributes.Abstract)) != MethodAttributes.PrivateScope)
											{
												continue;
											}
											bool flag11 = methodAttributes == MethodAttributes.Public;
											bool flag12 = (attributes2 & MethodAttributes.Static) != MethodAttributes.PrivateScope;
											BindingFlags bindingFlags2 = RuntimeType.FilterPreCalculate(flag11, flag10, flag12);
											RuntimeMethodHandle runtimeMethodHandle4 = (flag6 ? runtimeMethodHandle3.GetInstantiatingStubIfNeeded(runtimeTypeHandle) : runtimeMethodHandle3);
											RuntimeMethodInfo runtimeMethodInfo2 = new RuntimeMethodInfo(runtimeMethodHandle4, runtimeTypeHandle, this.m_runtimeTypeCache, attributes2, bindingFlags2);
											list.Add(runtimeMethodInfo2);
										}
									}
								}
							}
							runtimeTypeHandle = runtimeTypeHandle.GetBaseTypeHandle();
						}
					}
					return list;
				}

				// Token: 0x06000E8E RID: 3726 RVA: 0x0002AF7C File Offset: 0x00029F7C
				private List<RuntimeConstructorInfo> PopulateConstructors(RuntimeType.RuntimeTypeCache.Filter filter)
				{
					List<RuntimeConstructorInfo> list = new List<RuntimeConstructorInfo>();
					if (this.ReflectedType.IsGenericParameter)
					{
						return list;
					}
					RuntimeTypeHandle reflectedTypeHandle = this.ReflectedTypeHandle;
					bool flag = reflectedTypeHandle.HasInstantiation() && !reflectedTypeHandle.IsGenericTypeDefinition();
					foreach (RuntimeMethodHandle runtimeMethodHandle in reflectedTypeHandle.IntroducedMethods)
					{
						if (filter.Match(runtimeMethodHandle.GetUtf8Name()))
						{
							MethodAttributes attributes = runtimeMethodHandle.GetAttributes();
							if ((attributes & MethodAttributes.RTSpecialName) != MethodAttributes.PrivateScope && !runtimeMethodHandle.IsILStub())
							{
								bool flag2 = (attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;
								bool flag3 = (attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope;
								bool flag4 = false;
								BindingFlags bindingFlags = RuntimeType.FilterPreCalculate(flag2, flag4, flag3);
								RuntimeMethodHandle runtimeMethodHandle2 = (flag ? runtimeMethodHandle.GetInstantiatingStubIfNeeded(reflectedTypeHandle) : runtimeMethodHandle);
								RuntimeConstructorInfo runtimeConstructorInfo = new RuntimeConstructorInfo(runtimeMethodHandle2, this.ReflectedTypeHandle, this.m_runtimeTypeCache, attributes, bindingFlags);
								list.Add(runtimeConstructorInfo);
							}
						}
					}
					return list;
				}

				// Token: 0x06000E8F RID: 3727 RVA: 0x0002B06C File Offset: 0x0002A06C
				private List<RuntimeFieldInfo> PopulateFields(RuntimeType.RuntimeTypeCache.Filter filter)
				{
					List<RuntimeFieldInfo> list = new List<RuntimeFieldInfo>();
					RuntimeTypeHandle runtimeTypeHandle = this.ReflectedTypeHandle;
					while (runtimeTypeHandle.IsGenericVariable())
					{
						runtimeTypeHandle = runtimeTypeHandle.GetRuntimeType().BaseType.GetTypeHandleInternal();
					}
					while (!runtimeTypeHandle.IsNullHandle())
					{
						this.PopulateRtFields(filter, runtimeTypeHandle, list);
						this.PopulateLiteralFields(filter, runtimeTypeHandle, list);
						runtimeTypeHandle = runtimeTypeHandle.GetBaseTypeHandle();
					}
					if (this.ReflectedType.IsGenericParameter)
					{
						Type[] interfaces = this.ReflectedTypeHandle.GetRuntimeType().BaseType.GetInterfaces();
						for (int i = 0; i < interfaces.Length; i++)
						{
							this.PopulateLiteralFields(filter, interfaces[i].GetTypeHandleInternal(), list);
							this.PopulateRtFields(filter, interfaces[i].GetTypeHandleInternal(), list);
						}
					}
					else
					{
						RuntimeTypeHandle[] interfaces2 = this.ReflectedTypeHandle.GetInterfaces();
						if (interfaces2 != null)
						{
							for (int j = 0; j < interfaces2.Length; j++)
							{
								this.PopulateLiteralFields(filter, interfaces2[j], list);
								this.PopulateRtFields(filter, interfaces2[j], list);
							}
						}
					}
					return list;
				}

				// Token: 0x06000E90 RID: 3728 RVA: 0x0002B174 File Offset: 0x0002A174
				private unsafe void PopulateRtFields(RuntimeType.RuntimeTypeCache.Filter filter, RuntimeTypeHandle declaringTypeHandle, List<RuntimeFieldInfo> list)
				{
					int** ptr = stackalloc int*[sizeof(int*) * 64];
					int num = 64;
					if (!declaringTypeHandle.GetFields(ptr, &num))
					{
						fixed (int** ptr2 = new int*[num])
						{
							declaringTypeHandle.GetFields(ptr2, &num);
							this.PopulateRtFields(filter, ptr2, num, declaringTypeHandle, list);
						}
						return;
					}
					if (num > 0)
					{
						this.PopulateRtFields(filter, ptr, num, declaringTypeHandle, list);
					}
				}

				// Token: 0x06000E91 RID: 3729 RVA: 0x0002B1E8 File Offset: 0x0002A1E8
				private unsafe void PopulateRtFields(RuntimeType.RuntimeTypeCache.Filter filter, int** ppFieldHandles, int count, RuntimeTypeHandle declaringTypeHandle, List<RuntimeFieldInfo> list)
				{
					bool flag = declaringTypeHandle.HasInstantiation() && !declaringTypeHandle.ContainsGenericVariables();
					bool flag2 = !declaringTypeHandle.Equals(this.ReflectedTypeHandle);
					for (int i = 0; i < count; i++)
					{
						RuntimeFieldHandle staticFieldForGenericType = new RuntimeFieldHandle(*(IntPtr*)(ppFieldHandles + (IntPtr)i * (IntPtr)sizeof(int*) / (IntPtr)sizeof(int*)));
						if (filter.Match(staticFieldForGenericType.GetUtf8Name()))
						{
							FieldAttributes attributes = staticFieldForGenericType.GetAttributes();
							FieldAttributes fieldAttributes = attributes & FieldAttributes.FieldAccessMask;
							if (!flag2 || fieldAttributes != FieldAttributes.Private)
							{
								bool flag3 = fieldAttributes == FieldAttributes.Public;
								bool flag4 = (attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope;
								BindingFlags bindingFlags = RuntimeType.FilterPreCalculate(flag3, flag2, flag4);
								if (flag && flag4)
								{
									staticFieldForGenericType = staticFieldForGenericType.GetStaticFieldForGenericType(declaringTypeHandle);
								}
								RuntimeFieldInfo runtimeFieldInfo = new RtFieldInfo(staticFieldForGenericType, declaringTypeHandle.GetRuntimeType(), this.m_runtimeTypeCache, bindingFlags);
								list.Add(runtimeFieldInfo);
							}
						}
					}
				}

				// Token: 0x06000E92 RID: 3730 RVA: 0x0002B2BC File Offset: 0x0002A2BC
				private unsafe void PopulateLiteralFields(RuntimeType.RuntimeTypeCache.Filter filter, RuntimeTypeHandle declaringTypeHandle, List<RuntimeFieldInfo> list)
				{
					int token = declaringTypeHandle.GetToken();
					if (global::System.Reflection.MetadataToken.IsNullToken(token))
					{
						return;
					}
					MetadataImport metadataImport = declaringTypeHandle.GetModuleHandle().GetMetadataImport();
					int num = metadataImport.EnumFieldsCount(token);
					int* ptr = stackalloc int[4 * num];
					metadataImport.EnumFields(token, ptr, num);
					for (int i = 0; i < num; i++)
					{
						int num2 = ptr[i];
						Utf8String name = metadataImport.GetName(num2);
						if (filter.Match(name))
						{
							FieldAttributes fieldAttributes;
							metadataImport.GetFieldDefProps(num2, out fieldAttributes);
							FieldAttributes fieldAttributes2 = fieldAttributes & FieldAttributes.FieldAccessMask;
							if ((fieldAttributes & FieldAttributes.Literal) != FieldAttributes.PrivateScope)
							{
								bool flag = !declaringTypeHandle.Equals(this.ReflectedTypeHandle);
								if (!flag || fieldAttributes2 != FieldAttributes.Private)
								{
									bool flag2 = fieldAttributes2 == FieldAttributes.Public;
									bool flag3 = (fieldAttributes & FieldAttributes.Static) != FieldAttributes.PrivateScope;
									BindingFlags bindingFlags = RuntimeType.FilterPreCalculate(flag2, flag, flag3);
									RuntimeFieldInfo runtimeFieldInfo = new MdFieldInfo(num2, fieldAttributes, declaringTypeHandle, this.m_runtimeTypeCache, bindingFlags);
									list.Add(runtimeFieldInfo);
								}
							}
						}
					}
				}

				// Token: 0x06000E93 RID: 3731 RVA: 0x0002B3B0 File Offset: 0x0002A3B0
				private static void AddElementTypes(Type template, IList<Type> types)
				{
					if (!template.HasElementType)
					{
						return;
					}
					RuntimeType.RuntimeTypeCache.MemberInfoCache<T>.AddElementTypes(template.GetElementType(), types);
					for (int i = 0; i < types.Count; i++)
					{
						if (template.IsArray)
						{
							if (template.IsSzArray)
							{
								types[i] = types[i].MakeArrayType();
							}
							else
							{
								types[i] = types[i].MakeArrayType(template.GetArrayRank());
							}
						}
						else if (template.IsPointer)
						{
							types[i] = types[i].MakePointerType();
						}
					}
				}

				// Token: 0x06000E94 RID: 3732 RVA: 0x0002B440 File Offset: 0x0002A440
				private List<RuntimeType> PopulateInterfaces(RuntimeType.RuntimeTypeCache.Filter filter)
				{
					List<RuntimeType> list = new List<RuntimeType>();
					RuntimeTypeHandle reflectedTypeHandle = this.ReflectedTypeHandle;
					if (!reflectedTypeHandle.IsGenericVariable())
					{
						RuntimeTypeHandle[] interfaces = this.ReflectedTypeHandle.GetInterfaces();
						if (interfaces != null)
						{
							for (int i = 0; i < interfaces.Length; i++)
							{
								RuntimeType runtimeType = interfaces[i].GetRuntimeType();
								if (filter.Match(runtimeType.GetTypeHandleInternal().GetUtf8Name()))
								{
									list.Add(runtimeType);
								}
							}
						}
						if (this.ReflectedType.IsSzArray)
						{
							Type elementType = this.ReflectedType.GetElementType();
							if (!elementType.IsPointer)
							{
								Type type = typeof(IList<>).MakeGenericType(new Type[] { elementType });
								if (type.IsAssignableFrom(this.ReflectedType))
								{
									if (filter.Match(type.GetTypeHandleInternal().GetUtf8Name()))
									{
										list.Add(type as RuntimeType);
									}
									Type[] interfaces2 = type.GetInterfaces();
									for (int j = 0; j < interfaces2.Length; j++)
									{
										Type type2 = interfaces2[j];
										if (type2.IsGenericType && filter.Match(type2.GetTypeHandleInternal().GetUtf8Name()))
										{
											list.Add(interfaces2[j] as RuntimeType);
										}
									}
								}
							}
						}
					}
					else
					{
						List<RuntimeType> list2 = new List<RuntimeType>();
						foreach (Type type3 in reflectedTypeHandle.GetRuntimeType().GetGenericParameterConstraints())
						{
							if (type3.IsInterface)
							{
								list2.Add(type3 as RuntimeType);
							}
							Type[] interfaces3 = type3.GetInterfaces();
							for (int l = 0; l < interfaces3.Length; l++)
							{
								list2.Add(interfaces3[l] as RuntimeType);
							}
						}
						Hashtable hashtable = new Hashtable();
						for (int m = 0; m < list2.Count; m++)
						{
							Type type4 = list2[m];
							if (!hashtable.Contains(type4))
							{
								hashtable[type4] = type4;
							}
						}
						Type[] array = new Type[hashtable.Values.Count];
						hashtable.Values.CopyTo(array, 0);
						for (int n = 0; n < array.Length; n++)
						{
							if (filter.Match(array[n].GetTypeHandleInternal().GetUtf8Name()))
							{
								list.Add(array[n] as RuntimeType);
							}
						}
					}
					return list;
				}

				// Token: 0x06000E95 RID: 3733 RVA: 0x0002B6A8 File Offset: 0x0002A6A8
				private unsafe List<RuntimeType> PopulateNestedClasses(RuntimeType.RuntimeTypeCache.Filter filter)
				{
					List<RuntimeType> list = new List<RuntimeType>();
					RuntimeTypeHandle runtimeTypeHandle = this.ReflectedTypeHandle;
					if (runtimeTypeHandle.IsGenericVariable())
					{
						while (runtimeTypeHandle.IsGenericVariable())
						{
							runtimeTypeHandle = runtimeTypeHandle.GetRuntimeType().BaseType.GetTypeHandleInternal();
						}
					}
					int token = runtimeTypeHandle.GetToken();
					if (global::System.Reflection.MetadataToken.IsNullToken(token))
					{
						return list;
					}
					ModuleHandle moduleHandle = runtimeTypeHandle.GetModuleHandle();
					MetadataImport metadataImport = moduleHandle.GetMetadataImport();
					int num = metadataImport.EnumNestedTypesCount(token);
					int* ptr = stackalloc int[4 * num];
					metadataImport.EnumNestedTypes(token, ptr, num);
					int i = 0;
					while (i < num)
					{
						RuntimeTypeHandle runtimeTypeHandle2 = default(RuntimeTypeHandle);
						try
						{
							runtimeTypeHandle2 = moduleHandle.ResolveTypeHandle(ptr[i]);
						}
						catch (TypeLoadException)
						{
							goto IL_00C3;
						}
						goto IL_0098;
						IL_00C3:
						i++;
						continue;
						IL_0098:
						if (filter.Match(runtimeTypeHandle2.GetRuntimeType().GetTypeHandleInternal().GetUtf8Name()))
						{
							list.Add(runtimeTypeHandle2.GetRuntimeType());
							goto IL_00C3;
						}
						goto IL_00C3;
					}
					return list;
				}

				// Token: 0x06000E96 RID: 3734 RVA: 0x0002B798 File Offset: 0x0002A798
				private List<RuntimeEventInfo> PopulateEvents(RuntimeType.RuntimeTypeCache.Filter filter)
				{
					Hashtable hashtable = new Hashtable();
					RuntimeTypeHandle runtimeTypeHandle = this.ReflectedTypeHandle;
					List<RuntimeEventInfo> list = new List<RuntimeEventInfo>();
					if ((runtimeTypeHandle.GetAttributes() & TypeAttributes.ClassSemanticsMask) != TypeAttributes.ClassSemanticsMask)
					{
						while (runtimeTypeHandle.IsGenericVariable())
						{
							runtimeTypeHandle = runtimeTypeHandle.GetRuntimeType().BaseType.GetTypeHandleInternal();
						}
						while (!runtimeTypeHandle.IsNullHandle())
						{
							this.PopulateEvents(filter, runtimeTypeHandle, hashtable, list);
							runtimeTypeHandle = runtimeTypeHandle.GetBaseTypeHandle();
						}
					}
					else
					{
						this.PopulateEvents(filter, runtimeTypeHandle, hashtable, list);
					}
					return list;
				}

				// Token: 0x06000E97 RID: 3735 RVA: 0x0002B814 File Offset: 0x0002A814
				private unsafe void PopulateEvents(RuntimeType.RuntimeTypeCache.Filter filter, RuntimeTypeHandle declaringTypeHandle, Hashtable csEventInfos, List<RuntimeEventInfo> list)
				{
					int token = declaringTypeHandle.GetToken();
					if (global::System.Reflection.MetadataToken.IsNullToken(token))
					{
						return;
					}
					MetadataImport metadataImport = declaringTypeHandle.GetModuleHandle().GetMetadataImport();
					int num = metadataImport.EnumEventsCount(token);
					int* ptr = stackalloc int[4 * num];
					metadataImport.EnumEvents(token, ptr, num);
					this.PopulateEvents(filter, declaringTypeHandle, metadataImport, ptr, num, csEventInfos, list);
				}

				// Token: 0x06000E98 RID: 3736 RVA: 0x0002B86C File Offset: 0x0002A86C
				private unsafe void PopulateEvents(RuntimeType.RuntimeTypeCache.Filter filter, RuntimeTypeHandle declaringTypeHandle, MetadataImport scope, int* tkAssociates, int cAssociates, Hashtable csEventInfos, List<RuntimeEventInfo> list)
				{
					for (int i = 0; i < cAssociates; i++)
					{
						int num = tkAssociates[i];
						Utf8String name = scope.GetName(num);
						if (filter.Match(name))
						{
							bool flag;
							RuntimeEventInfo runtimeEventInfo = new RuntimeEventInfo(num, declaringTypeHandle.GetRuntimeType(), this.m_runtimeTypeCache, out flag);
							if ((declaringTypeHandle.Equals(this.m_runtimeTypeCache.RuntimeTypeHandle) || !flag) && csEventInfos[runtimeEventInfo.Name] == null)
							{
								csEventInfos[runtimeEventInfo.Name] = runtimeEventInfo;
								list.Add(runtimeEventInfo);
							}
						}
					}
				}

				// Token: 0x06000E99 RID: 3737 RVA: 0x0002B8FC File Offset: 0x0002A8FC
				private List<RuntimePropertyInfo> PopulateProperties(RuntimeType.RuntimeTypeCache.Filter filter)
				{
					Hashtable hashtable = new Hashtable();
					RuntimeTypeHandle runtimeTypeHandle = this.ReflectedTypeHandle;
					List<RuntimePropertyInfo> list = new List<RuntimePropertyInfo>();
					if ((runtimeTypeHandle.GetAttributes() & TypeAttributes.ClassSemanticsMask) != TypeAttributes.ClassSemanticsMask)
					{
						while (runtimeTypeHandle.IsGenericVariable())
						{
							runtimeTypeHandle = runtimeTypeHandle.GetRuntimeType().BaseType.GetTypeHandleInternal();
						}
						while (!runtimeTypeHandle.IsNullHandle())
						{
							this.PopulateProperties(filter, runtimeTypeHandle, hashtable, list);
							runtimeTypeHandle = runtimeTypeHandle.GetBaseTypeHandle();
						}
					}
					else
					{
						this.PopulateProperties(filter, runtimeTypeHandle, hashtable, list);
					}
					return list;
				}

				// Token: 0x06000E9A RID: 3738 RVA: 0x0002B978 File Offset: 0x0002A978
				private unsafe void PopulateProperties(RuntimeType.RuntimeTypeCache.Filter filter, RuntimeTypeHandle declaringTypeHandle, Hashtable csPropertyInfos, List<RuntimePropertyInfo> list)
				{
					int token = declaringTypeHandle.GetToken();
					if (global::System.Reflection.MetadataToken.IsNullToken(token))
					{
						return;
					}
					MetadataImport metadataImport = declaringTypeHandle.GetModuleHandle().GetMetadataImport();
					int num = metadataImport.EnumPropertiesCount(token);
					int* ptr = stackalloc int[4 * num];
					metadataImport.EnumProperties(token, ptr, num);
					this.PopulateProperties(filter, declaringTypeHandle, ptr, num, csPropertyInfos, list);
				}

				// Token: 0x06000E9B RID: 3739 RVA: 0x0002B9D0 File Offset: 0x0002A9D0
				private unsafe void PopulateProperties(RuntimeType.RuntimeTypeCache.Filter filter, RuntimeTypeHandle declaringTypeHandle, int* tkAssociates, int cProperties, Hashtable csPropertyInfos, List<RuntimePropertyInfo> list)
				{
					for (int i = 0; i < cProperties; i++)
					{
						int num = tkAssociates[i];
						Utf8String name = declaringTypeHandle.GetRuntimeType().Module.MetadataImport.GetName(num);
						if (filter.Match(name))
						{
							bool flag;
							RuntimePropertyInfo runtimePropertyInfo = new RuntimePropertyInfo(num, declaringTypeHandle.GetRuntimeType(), this.m_runtimeTypeCache, out flag);
							if (declaringTypeHandle.Equals(this.m_runtimeTypeCache.RuntimeTypeHandle) || !flag)
							{
								List<RuntimePropertyInfo> list2 = csPropertyInfos[runtimePropertyInfo.Name] as List<RuntimePropertyInfo>;
								if (list2 == null)
								{
									list2 = new List<RuntimePropertyInfo>();
									csPropertyInfos[runtimePropertyInfo.Name] = list2;
								}
								else
								{
									for (int j = 0; j < list2.Count; j++)
									{
										if (runtimePropertyInfo.EqualsSig(list2[j]))
										{
											list2 = null;
											break;
										}
									}
								}
								if (list2 != null)
								{
									list2.Add(runtimePropertyInfo);
									list.Add(runtimePropertyInfo);
								}
							}
						}
					}
				}

				// Token: 0x06000E9C RID: 3740 RVA: 0x0002BAC4 File Offset: 0x0002AAC4
				internal CerArrayList<T> GetMemberList(MemberListType listType, string name, RuntimeType.RuntimeTypeCache.CacheType cacheType)
				{
					switch (listType)
					{
					case MemberListType.All:
						if (this.m_cacheComplete)
						{
							return this.m_root;
						}
						return this.Populate(null, listType, cacheType);
					case MemberListType.CaseSensitive:
					{
						if (this.m_csMemberInfos == null)
						{
							return this.Populate(name, listType, cacheType);
						}
						CerArrayList<T> cerArrayList = this.m_csMemberInfos[name];
						if (cerArrayList == null)
						{
							return this.Populate(name, listType, cacheType);
						}
						return cerArrayList;
					}
					default:
					{
						if (this.m_cisMemberInfos == null)
						{
							return this.Populate(name, listType, cacheType);
						}
						CerArrayList<T> cerArrayList = this.m_cisMemberInfos[name];
						if (cerArrayList == null)
						{
							return this.Populate(name, listType, cacheType);
						}
						return cerArrayList;
					}
					}
				}

				// Token: 0x170001D1 RID: 465
				// (get) Token: 0x06000E9D RID: 3741 RVA: 0x0002BB59 File Offset: 0x0002AB59
				internal RuntimeTypeHandle ReflectedTypeHandle
				{
					get
					{
						return this.m_runtimeTypeCache.RuntimeTypeHandle;
					}
				}

				// Token: 0x170001D2 RID: 466
				// (get) Token: 0x06000E9E RID: 3742 RVA: 0x0002BB68 File Offset: 0x0002AB68
				internal RuntimeType ReflectedType
				{
					get
					{
						return this.ReflectedTypeHandle.GetRuntimeType();
					}
				}

				// Token: 0x040004D0 RID: 1232
				private CerHashtable<string, CerArrayList<T>> m_csMemberInfos;

				// Token: 0x040004D1 RID: 1233
				private CerHashtable<string, CerArrayList<T>> m_cisMemberInfos;

				// Token: 0x040004D2 RID: 1234
				private CerArrayList<T> m_root;

				// Token: 0x040004D3 RID: 1235
				private bool m_cacheComplete;

				// Token: 0x040004D4 RID: 1236
				private RuntimeType.RuntimeTypeCache m_runtimeTypeCache;
			}
		}

		// Token: 0x020000FA RID: 250
		private class TypeCacheQueue
		{
			// Token: 0x06000E9F RID: 3743 RVA: 0x0002BB83 File Offset: 0x0002AB83
			internal TypeCacheQueue()
			{
				this.liveCache = new object[4];
			}

			// Token: 0x040004D5 RID: 1237
			private const int QUEUE_SIZE = 4;

			// Token: 0x040004D6 RID: 1238
			private object[] liveCache;
		}

		// Token: 0x020000FB RID: 251
		private class ActivatorCacheEntry
		{
			// Token: 0x06000EA0 RID: 3744 RVA: 0x0002BB97 File Offset: 0x0002AB97
			internal ActivatorCacheEntry(Type t, RuntimeMethodHandle rmh, bool bNeedSecurityCheck)
			{
				this.m_type = t;
				this.m_bNeedSecurityCheck = bNeedSecurityCheck;
				this.m_hCtorMethodHandle = rmh;
			}

			// Token: 0x040004D7 RID: 1239
			internal Type m_type;

			// Token: 0x040004D8 RID: 1240
			internal CtorDelegate m_ctor;

			// Token: 0x040004D9 RID: 1241
			internal RuntimeMethodHandle m_hCtorMethodHandle;

			// Token: 0x040004DA RID: 1242
			internal bool m_bNeedSecurityCheck;

			// Token: 0x040004DB RID: 1243
			internal bool m_bFullyInitialized;
		}

		// Token: 0x020000FC RID: 252
		private class ActivatorCache
		{
			// Token: 0x06000EA1 RID: 3745 RVA: 0x0002BBB4 File Offset: 0x0002ABB4
			private void InitializeDelegateCreator()
			{
				PermissionSet permissionSet = new PermissionSet(PermissionState.None);
				permissionSet.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));
				permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
				Thread.MemoryBarrier();
				this.delegateCreatePermissions = permissionSet;
				ConstructorInfo constructor = typeof(CtorDelegate).GetConstructor(new Type[]
				{
					typeof(object),
					typeof(IntPtr)
				});
				Thread.MemoryBarrier();
				this.delegateCtorInfo = constructor;
			}

			// Token: 0x06000EA2 RID: 3746 RVA: 0x0002BC2C File Offset: 0x0002AC2C
			private void InitializeCacheEntry(RuntimeType.ActivatorCacheEntry ace)
			{
				if (!ace.m_type.IsValueType)
				{
					if (this.delegateCtorInfo == null)
					{
						this.InitializeDelegateCreator();
					}
					this.delegateCreatePermissions.Assert();
					CtorDelegate ctorDelegate = (CtorDelegate)this.delegateCtorInfo.Invoke(new object[]
					{
						null,
						ace.m_hCtorMethodHandle.GetFunctionPointer()
					});
					Thread.MemoryBarrier();
					ace.m_ctor = ctorDelegate;
				}
				ace.m_bFullyInitialized = true;
			}

			// Token: 0x06000EA3 RID: 3747 RVA: 0x0002BCA0 File Offset: 0x0002ACA0
			internal RuntimeType.ActivatorCacheEntry GetEntry(Type t)
			{
				int num = this.hash_counter;
				for (int i = 0; i < 16; i++)
				{
					RuntimeType.ActivatorCacheEntry activatorCacheEntry = this.cache[num];
					if (activatorCacheEntry != null && activatorCacheEntry.m_type == t)
					{
						if (!activatorCacheEntry.m_bFullyInitialized)
						{
							this.InitializeCacheEntry(activatorCacheEntry);
						}
						return activatorCacheEntry;
					}
					num = (num + 1) & 15;
				}
				return null;
			}

			// Token: 0x06000EA4 RID: 3748 RVA: 0x0002BCF0 File Offset: 0x0002ACF0
			internal void SetEntry(RuntimeType.ActivatorCacheEntry ace)
			{
				int num = (this.hash_counter - 1) & 15;
				this.hash_counter = num;
				this.cache[num] = ace;
			}

			// Token: 0x040004DC RID: 1244
			private const int CACHE_SIZE = 16;

			// Token: 0x040004DD RID: 1245
			private int hash_counter;

			// Token: 0x040004DE RID: 1246
			private RuntimeType.ActivatorCacheEntry[] cache = new RuntimeType.ActivatorCacheEntry[16];

			// Token: 0x040004DF RID: 1247
			private ConstructorInfo delegateCtorInfo;

			// Token: 0x040004E0 RID: 1248
			private PermissionSet delegateCreatePermissions;
		}

		// Token: 0x020000FD RID: 253
		[Flags]
		private enum DispatchWrapperType
		{
			// Token: 0x040004E2 RID: 1250
			Unknown = 1,
			// Token: 0x040004E3 RID: 1251
			Dispatch = 2,
			// Token: 0x040004E4 RID: 1252
			Record = 4,
			// Token: 0x040004E5 RID: 1253
			Error = 8,
			// Token: 0x040004E6 RID: 1254
			Currency = 16,
			// Token: 0x040004E7 RID: 1255
			BStr = 32,
			// Token: 0x040004E8 RID: 1256
			SafeArray = 65536
		}
	}
}
