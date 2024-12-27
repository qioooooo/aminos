using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Reflection
{
	// Token: 0x02000330 RID: 816
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_MethodBase))]
	[ComVisible(true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public abstract class MethodBase : MemberInfo, _MethodBase
	{
		// Token: 0x06001FB6 RID: 8118 RVA: 0x0004FE98 File Offset: 0x0004EE98
		public static MethodBase GetMethodFromHandle(RuntimeMethodHandle handle)
		{
			if (handle.IsNullHandle())
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidHandle"));
			}
			MethodBase methodBase = RuntimeType.GetMethodBase(handle);
			if (methodBase.DeclaringType != null && methodBase.DeclaringType.IsGenericType)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_MethodDeclaringTypeGeneric"), new object[]
				{
					methodBase,
					methodBase.DeclaringType.GetGenericTypeDefinition()
				}));
			}
			return methodBase;
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x0004FF0F File Offset: 0x0004EF0F
		[ComVisible(false)]
		public static MethodBase GetMethodFromHandle(RuntimeMethodHandle handle, RuntimeTypeHandle declaringType)
		{
			if (handle.IsNullHandle())
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidHandle"));
			}
			return RuntimeType.GetMethodBase(declaringType, handle);
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x0004FF34 File Offset: 0x0004EF34
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static MethodBase GetCurrentMethod()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return RuntimeMethodInfo.InternalGetCurrentMethod(ref stackCrawlMark);
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001FBA RID: 8122 RVA: 0x0004FF52 File Offset: 0x0004EF52
		internal virtual bool IsOverloaded
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_Method"));
			}
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x0004FF63 File Offset: 0x0004EF63
		internal virtual RuntimeMethodHandle GetMethodHandle()
		{
			return this.MethodHandle;
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x0004FF6B File Offset: 0x0004EF6B
		internal virtual Type GetReturnType()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x0004FF72 File Offset: 0x0004EF72
		internal virtual ParameterInfo[] GetParametersNoCopy()
		{
			return this.GetParameters();
		}

		// Token: 0x06001FBE RID: 8126
		public abstract ParameterInfo[] GetParameters();

		// Token: 0x06001FBF RID: 8127
		public abstract MethodImplAttributes GetMethodImplementationFlags();

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001FC0 RID: 8128
		public abstract RuntimeMethodHandle MethodHandle { get; }

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001FC1 RID: 8129
		public abstract MethodAttributes Attributes { get; }

		// Token: 0x06001FC2 RID: 8130
		public abstract object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001FC3 RID: 8131 RVA: 0x0004FF7A File Offset: 0x0004EF7A
		public virtual CallingConventions CallingConvention
		{
			get
			{
				return CallingConventions.Standard;
			}
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x0004FF7D File Offset: 0x0004EF7D
		[ComVisible(true)]
		public virtual Type[] GetGenericArguments()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001FC5 RID: 8133 RVA: 0x0004FF8E File Offset: 0x0004EF8E
		public virtual bool IsGenericMethodDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001FC6 RID: 8134 RVA: 0x0004FF91 File Offset: 0x0004EF91
		public virtual bool ContainsGenericParameters
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001FC7 RID: 8135 RVA: 0x0004FF94 File Offset: 0x0004EF94
		public virtual bool IsGenericMethod
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x0004FF97 File Offset: 0x0004EF97
		Type _MethodBase.GetType()
		{
			return base.GetType();
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001FC9 RID: 8137 RVA: 0x0004FF9F File Offset: 0x0004EF9F
		bool _MethodBase.IsPublic
		{
			get
			{
				return this.IsPublic;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001FCA RID: 8138 RVA: 0x0004FFA7 File Offset: 0x0004EFA7
		bool _MethodBase.IsPrivate
		{
			get
			{
				return this.IsPrivate;
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001FCB RID: 8139 RVA: 0x0004FFAF File Offset: 0x0004EFAF
		bool _MethodBase.IsFamily
		{
			get
			{
				return this.IsFamily;
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001FCC RID: 8140 RVA: 0x0004FFB7 File Offset: 0x0004EFB7
		bool _MethodBase.IsAssembly
		{
			get
			{
				return this.IsAssembly;
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001FCD RID: 8141 RVA: 0x0004FFBF File Offset: 0x0004EFBF
		bool _MethodBase.IsFamilyAndAssembly
		{
			get
			{
				return this.IsFamilyAndAssembly;
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001FCE RID: 8142 RVA: 0x0004FFC7 File Offset: 0x0004EFC7
		bool _MethodBase.IsFamilyOrAssembly
		{
			get
			{
				return this.IsFamilyOrAssembly;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001FCF RID: 8143 RVA: 0x0004FFCF File Offset: 0x0004EFCF
		bool _MethodBase.IsStatic
		{
			get
			{
				return this.IsStatic;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x0004FFD7 File Offset: 0x0004EFD7
		bool _MethodBase.IsFinal
		{
			get
			{
				return this.IsFinal;
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001FD1 RID: 8145 RVA: 0x0004FFDF File Offset: 0x0004EFDF
		bool _MethodBase.IsVirtual
		{
			get
			{
				return this.IsVirtual;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001FD2 RID: 8146 RVA: 0x0004FFE7 File Offset: 0x0004EFE7
		bool _MethodBase.IsHideBySig
		{
			get
			{
				return this.IsHideBySig;
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001FD3 RID: 8147 RVA: 0x0004FFEF File Offset: 0x0004EFEF
		bool _MethodBase.IsAbstract
		{
			get
			{
				return this.IsAbstract;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001FD4 RID: 8148 RVA: 0x0004FFF7 File Offset: 0x0004EFF7
		bool _MethodBase.IsSpecialName
		{
			get
			{
				return this.IsSpecialName;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001FD5 RID: 8149 RVA: 0x0004FFFF File Offset: 0x0004EFFF
		bool _MethodBase.IsConstructor
		{
			get
			{
				return this.IsConstructor;
			}
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x00050007 File Offset: 0x0004F007
		[DebuggerHidden]
		[DebuggerStepThrough]
		public object Invoke(object obj, object[] parameters)
		{
			return this.Invoke(obj, BindingFlags.Default, null, parameters, null);
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001FD7 RID: 8151 RVA: 0x00050014 File Offset: 0x0004F014
		public bool IsPublic
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x00050021 File Offset: 0x0004F021
		public bool IsPrivate
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001FD9 RID: 8153 RVA: 0x0005002E File Offset: 0x0004F02E
		public bool IsFamily
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Family;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001FDA RID: 8154 RVA: 0x0005003B File Offset: 0x0004F03B
		public bool IsAssembly
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Assembly;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001FDB RID: 8155 RVA: 0x00050048 File Offset: 0x0004F048
		public bool IsFamilyAndAssembly
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamANDAssem;
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001FDC RID: 8156 RVA: 0x00050055 File Offset: 0x0004F055
		public bool IsFamilyOrAssembly
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamORAssem;
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001FDD RID: 8157 RVA: 0x00050062 File Offset: 0x0004F062
		public bool IsStatic
		{
			get
			{
				return (this.Attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001FDE RID: 8158 RVA: 0x00050073 File Offset: 0x0004F073
		public bool IsFinal
		{
			get
			{
				return (this.Attributes & MethodAttributes.Final) != MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001FDF RID: 8159 RVA: 0x00050084 File Offset: 0x0004F084
		public bool IsVirtual
		{
			get
			{
				return (this.Attributes & MethodAttributes.Virtual) != MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001FE0 RID: 8160 RVA: 0x00050095 File Offset: 0x0004F095
		public bool IsHideBySig
		{
			get
			{
				return (this.Attributes & MethodAttributes.HideBySig) != MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001FE1 RID: 8161 RVA: 0x000500A9 File Offset: 0x0004F0A9
		public bool IsAbstract
		{
			get
			{
				return (this.Attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001FE2 RID: 8162 RVA: 0x000500BD File Offset: 0x0004F0BD
		public bool IsSpecialName
		{
			get
			{
				return (this.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001FE3 RID: 8163 RVA: 0x000500D1 File Offset: 0x0004F0D1
		[ComVisible(true)]
		public bool IsConstructor
		{
			get
			{
				return (this.Attributes & MethodAttributes.RTSpecialName) != MethodAttributes.PrivateScope && this.Name.Equals(ConstructorInfo.ConstructorName);
			}
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000500F3 File Offset: 0x0004F0F3
		[ReflectionPermission(SecurityAction.Demand, Flags = ReflectionPermissionFlag.MemberAccess)]
		public virtual MethodBody GetMethodBody()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06001FE5 RID: 8165
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint GetSpecialSecurityFlags(RuntimeMethodHandle method);

		// Token: 0x06001FE6 RID: 8166
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void PerformSecurityCheck(object obj, RuntimeMethodHandle method, IntPtr parent, uint invocationFlags);

		// Token: 0x06001FE7 RID: 8167 RVA: 0x000500FC File Offset: 0x0004F0FC
		internal virtual Type[] GetParameterTypes()
		{
			ParameterInfo[] parametersNoCopy = this.GetParametersNoCopy();
			Type[] array = new Type[parametersNoCopy.Length];
			for (int i = 0; i < parametersNoCopy.Length; i++)
			{
				array[i] = parametersNoCopy[i].ParameterType;
			}
			return array;
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x00050138 File Offset: 0x0004F138
		internal virtual uint GetOneTimeFlags()
		{
			RuntimeMethodHandle methodHandle = this.MethodHandle;
			uint num = 0U;
			Type declaringType = this.DeclaringType;
			if (this.ContainsGenericParameters || (declaringType != null && declaringType.ContainsGenericParameters) || (this.CallingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs || (this.Attributes & MethodAttributes.RequireSecObject) == MethodAttributes.RequireSecObject)
			{
				num |= 2U;
			}
			else
			{
				AssemblyBuilderData assemblyData = this.Module.Assembly.m_assemblyData;
				if (assemblyData != null && (assemblyData.m_access & AssemblyBuilderAccess.Run) == (AssemblyBuilderAccess)0)
				{
					num |= 2U;
				}
			}
			if (num == 0U)
			{
				num |= MethodBase.GetSpecialSecurityFlags(methodHandle);
				if ((num & 4U) == 0U)
				{
					if ((this.Attributes & MethodAttributes.MemberAccessMask) != MethodAttributes.Public || (declaringType != null && !declaringType.IsVisible))
					{
						num |= 4U;
					}
					else if (this.IsGenericMethod)
					{
						Type[] genericArguments = this.GetGenericArguments();
						for (int i = 0; i < genericArguments.Length; i++)
						{
							if (!genericArguments[i].IsVisible)
							{
								num |= 4U;
								break;
							}
						}
					}
				}
			}
			num |= this.GetOneTimeSpecificFlags();
			return num | 1U;
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x0005021E File Offset: 0x0004F21E
		internal virtual uint GetOneTimeSpecificFlags()
		{
			return 0U;
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x00050224 File Offset: 0x0004F224
		internal object[] CheckArguments(object[] parameters, Binder binder, BindingFlags invokeAttr, CultureInfo culture, Signature sig)
		{
			int num = ((parameters != null) ? parameters.Length : 0);
			object[] array = new object[num];
			ParameterInfo[] array2 = null;
			for (int i = 0; i < num; i++)
			{
				object obj = parameters[i];
				RuntimeTypeHandle runtimeTypeHandle = sig.Arguments[i];
				if (obj == Type.Missing)
				{
					if (array2 == null)
					{
						array2 = this.GetParametersNoCopy();
					}
					if (array2[i].DefaultValue == DBNull.Value)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_VarMissNull"), "parameters");
					}
					obj = array2[i].DefaultValue;
				}
				if (runtimeTypeHandle.IsInstanceOfType(obj))
				{
					array[i] = obj;
				}
				else
				{
					array[i] = runtimeTypeHandle.GetRuntimeType().CheckValue(obj, binder, culture, invokeAttr);
				}
			}
			return array;
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x000502D8 File Offset: 0x0004F2D8
		void _MethodBase.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x000502DF File Offset: 0x0004F2DF
		void _MethodBase.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x000502E6 File Offset: 0x0004F2E6
		void _MethodBase.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x000502ED File Offset: 0x0004F2ED
		void _MethodBase.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}
	}
}
