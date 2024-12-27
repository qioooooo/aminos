using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Reflection.Emit
{
	// Token: 0x0200080A RID: 2058
	[ComVisible(true)]
	public sealed class DynamicMethod : MethodInfo
	{
		// Token: 0x06004982 RID: 18818 RVA: 0x00100B80 File Offset: 0x000FFB80
		private DynamicMethod()
		{
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x00100B88 File Offset: 0x000FFB88
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes)
		{
			this.Init(name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, returnType, parameterTypes, null, null, false, true);
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x00100BAC File Offset: 0x000FFBAC
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, bool restrictedSkipVisibility)
		{
			this.Init(name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, returnType, parameterTypes, null, null, restrictedSkipVisibility, true);
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x00100BD0 File Offset: 0x000FFBD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Module m)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			DynamicMethod.PerformSecurityCheck(m, ref stackCrawlMark, false);
			this.Init(name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, returnType, parameterTypes, null, m, false, false);
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x00100C00 File Offset: 0x000FFC00
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Module m, bool skipVisibility)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			DynamicMethod.PerformSecurityCheck(m, ref stackCrawlMark, skipVisibility);
			this.Init(name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, returnType, parameterTypes, null, m, skipVisibility, false);
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x00100C34 File Offset: 0x000FFC34
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DynamicMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Module m, bool skipVisibility)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			DynamicMethod.PerformSecurityCheck(m, ref stackCrawlMark, skipVisibility);
			this.Init(name, attributes, callingConvention, returnType, parameterTypes, null, m, skipVisibility, false);
		}

		// Token: 0x06004988 RID: 18824 RVA: 0x00100C68 File Offset: 0x000FFC68
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			DynamicMethod.PerformSecurityCheck(owner, ref stackCrawlMark, false);
			this.Init(name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, returnType, parameterTypes, owner, null, false, false);
		}

		// Token: 0x06004989 RID: 18825 RVA: 0x00100C98 File Offset: 0x000FFC98
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner, bool skipVisibility)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			DynamicMethod.PerformSecurityCheck(owner, ref stackCrawlMark, skipVisibility);
			this.Init(name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, returnType, parameterTypes, owner, null, skipVisibility, false);
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x00100CCC File Offset: 0x000FFCCC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DynamicMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type owner, bool skipVisibility)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			DynamicMethod.PerformSecurityCheck(owner, ref stackCrawlMark, skipVisibility);
			this.Init(name, attributes, callingConvention, returnType, parameterTypes, owner, null, skipVisibility, false);
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x00100D00 File Offset: 0x000FFD00
		private static void CheckConsistency(MethodAttributes attributes, CallingConventions callingConvention)
		{
			if ((attributes & ~MethodAttributes.MemberAccessMask) != MethodAttributes.Static)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicMethodFlags"));
			}
			if ((attributes & MethodAttributes.MemberAccessMask) != MethodAttributes.Public)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicMethodFlags"));
			}
			if (callingConvention != CallingConventions.Standard && callingConvention != CallingConventions.VarArgs)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicMethodFlags"));
			}
			if (callingConvention == CallingConventions.VarArgs)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicMethodFlags"));
			}
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x00100D68 File Offset: 0x000FFD68
		private static Module GetDynamicMethodsModule()
		{
			if (DynamicMethod.s_anonymouslyHostedDynamicMethodsModule != null)
			{
				return DynamicMethod.s_anonymouslyHostedDynamicMethodsModule;
			}
			lock (DynamicMethod.s_anonymouslyHostedDynamicMethodsModuleLock)
			{
				if (DynamicMethod.s_anonymouslyHostedDynamicMethodsModule != null)
				{
					return DynamicMethod.s_anonymouslyHostedDynamicMethodsModule;
				}
				ConstructorInfo constructor = typeof(SecurityTransparentAttribute).GetConstructor(Type.EmptyTypes);
				CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(constructor, new object[0]);
				CustomAttributeBuilder[] array = new CustomAttributeBuilder[] { customAttributeBuilder };
				AssemblyName assemblyName = new AssemblyName("Anonymously Hosted DynamicMethods Assembly");
				AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run, array);
				AppDomain.CurrentDomain.PublishAnonymouslyHostedDynamicMethodsAssembly(assemblyBuilder.InternalAssembly);
				DynamicMethod.s_anonymouslyHostedDynamicMethodsModule = assemblyBuilder.ManifestModule;
			}
			return DynamicMethod.s_anonymouslyHostedDynamicMethodsModule;
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x00100E28 File Offset: 0x000FFE28
		private void Init(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] signature, Type owner, Module m, bool skipVisibility, bool transparentMethod)
		{
			DynamicMethod.CheckConsistency(attributes, callingConvention);
			if (signature != null)
			{
				this.m_parameterTypes = new RuntimeType[signature.Length];
				for (int i = 0; i < signature.Length; i++)
				{
					if (signature[i] == null)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_InvalidTypeInSignature"));
					}
					this.m_parameterTypes[i] = signature[i].UnderlyingSystemType as RuntimeType;
					if (this.m_parameterTypes[i] == null || this.m_parameterTypes[i] == typeof(void))
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_InvalidTypeInSignature"));
					}
				}
			}
			else
			{
				this.m_parameterTypes = new RuntimeType[0];
			}
			this.m_returnType = ((returnType == null) ? ((RuntimeType)typeof(void)) : (returnType.UnderlyingSystemType as RuntimeType));
			if (this.m_returnType == null || this.m_returnType.IsByRef)
			{
				throw new NotSupportedException(Environment.GetResourceString("Arg_InvalidTypeInRetType"));
			}
			if (transparentMethod)
			{
				this.m_module = DynamicMethod.GetDynamicMethodsModule().ModuleHandle;
				if (skipVisibility)
				{
					this.m_restrictedSkipVisibility = true;
				}
				this.m_creationContext = CompressedStack.Capture();
			}
			else
			{
				this.m_typeOwner = ((owner != null) ? (owner.UnderlyingSystemType as RuntimeType) : null);
				if (this.m_typeOwner != null && (this.m_typeOwner.HasElementType || this.m_typeOwner.ContainsGenericParameters || this.m_typeOwner.IsGenericParameter || this.m_typeOwner.IsInterface))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidTypeForDynamicMethod"));
				}
				this.m_module = ((m != null) ? m.ModuleHandle : this.m_typeOwner.Module.ModuleHandle);
				this.m_skipVisibility = skipVisibility;
			}
			this.m_ilGenerator = null;
			this.m_fInitLocals = true;
			this.m_method = new RuntimeMethodHandle(null);
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.m_dynMethod = new DynamicMethod.RTDynamicMethod(this, name, attributes, callingConvention);
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x0010100C File Offset: 0x0010000C
		private static void PerformSecurityCheck(Module m, ref StackCrawlMark stackMark, bool skipVisibility)
		{
			if (m == null)
			{
				throw new ArgumentNullException("m");
			}
			if (m.Equals(DynamicMethod.s_anonymouslyHostedDynamicMethodsModule))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidValue"), "m");
			}
			if (skipVisibility)
			{
				new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
			}
			RuntimeTypeHandle callerType = ModuleHandle.GetCallerType(ref stackMark);
			if (!m.Assembly.AssemblyHandle.Equals(callerType.GetAssemblyHandle()) || m == typeof(object).Module)
			{
				PermissionSet permissionSet;
				PermissionSet permissionSet2;
				m.Assembly.nGetGrantSet(out permissionSet, out permissionSet2);
				if (permissionSet == null)
				{
					permissionSet = new PermissionSet(PermissionState.Unrestricted);
				}
				CodeAccessSecurityEngine.ReflectionTargetDemandHelper(PermissionType.SecurityControlEvidence, permissionSet);
			}
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x001010B0 File Offset: 0x001000B0
		private static void PerformSecurityCheck(Type owner, ref StackCrawlMark stackMark, bool skipVisibility)
		{
			if (owner == null || (owner = owner.UnderlyingSystemType as RuntimeType) == null)
			{
				throw new ArgumentNullException("owner");
			}
			RuntimeTypeHandle callerType = ModuleHandle.GetCallerType(ref stackMark);
			if (skipVisibility)
			{
				new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
			}
			else if (!callerType.Equals(owner.TypeHandle))
			{
				new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
			}
			if (!owner.Assembly.AssemblyHandle.Equals(callerType.GetAssemblyHandle()) || owner.Module == typeof(object).Module)
			{
				PermissionSet permissionSet;
				PermissionSet permissionSet2;
				owner.Assembly.nGetGrantSet(out permissionSet, out permissionSet2);
				if (permissionSet == null)
				{
					permissionSet = new PermissionSet(PermissionState.Unrestricted);
				}
				CodeAccessSecurityEngine.ReflectionTargetDemandHelper(PermissionType.SecurityControlEvidence, permissionSet);
			}
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x00101160 File Offset: 0x00100160
		[ComVisible(true)]
		public Delegate CreateDelegate(Type delegateType)
		{
			if (this.m_restrictedSkipVisibility)
			{
				RuntimeHelpers._CompileMethod(this.GetMethodDescriptor().Value);
			}
			MulticastDelegate multicastDelegate = (MulticastDelegate)Delegate.CreateDelegate(delegateType, null, this.GetMethodDescriptor());
			multicastDelegate.StoreDynamicMethod(this.GetMethodInfo());
			return multicastDelegate;
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x001011A8 File Offset: 0x001001A8
		[ComVisible(true)]
		public Delegate CreateDelegate(Type delegateType, object target)
		{
			if (this.m_restrictedSkipVisibility)
			{
				RuntimeHelpers._CompileMethod(this.GetMethodDescriptor().Value);
			}
			MulticastDelegate multicastDelegate = (MulticastDelegate)Delegate.CreateDelegate(delegateType, target, this.GetMethodDescriptor());
			multicastDelegate.StoreDynamicMethod(this.GetMethodInfo());
			return multicastDelegate;
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x001011F0 File Offset: 0x001001F0
		internal RuntimeMethodHandle GetMethodDescriptor()
		{
			if (this.m_method.IsNullHandle())
			{
				lock (this)
				{
					if (this.m_method.IsNullHandle())
					{
						if (this.m_DynamicILInfo != null)
						{
							this.m_method = this.m_DynamicILInfo.GetCallableMethod(this.m_module.Value);
						}
						else
						{
							if (this.m_ilGenerator == null || this.m_ilGenerator.m_length == 0)
							{
								throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_BadEmptyMethodBody"), new object[] { this.Name }));
							}
							this.m_method = this.m_ilGenerator.GetCallableMethod(this.m_module.Value);
						}
					}
				}
			}
			return this.m_method;
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x001012C8 File Offset: 0x001002C8
		public override string ToString()
		{
			return this.m_dynMethod.ToString();
		}

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06004994 RID: 18836 RVA: 0x001012D5 File Offset: 0x001002D5
		public override string Name
		{
			get
			{
				return this.m_dynMethod.Name;
			}
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06004995 RID: 18837 RVA: 0x001012E2 File Offset: 0x001002E2
		public override Type DeclaringType
		{
			get
			{
				return this.m_dynMethod.DeclaringType;
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x06004996 RID: 18838 RVA: 0x001012EF File Offset: 0x001002EF
		public override Type ReflectedType
		{
			get
			{
				return this.m_dynMethod.ReflectedType;
			}
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x06004997 RID: 18839 RVA: 0x001012FC File Offset: 0x001002FC
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.m_dynMethod.MetadataTokenInternal;
			}
		}

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06004998 RID: 18840 RVA: 0x00101309 File Offset: 0x00100309
		public override Module Module
		{
			get
			{
				return this.m_dynMethod.Module;
			}
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06004999 RID: 18841 RVA: 0x00101316 File Offset: 0x00100316
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
			}
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x0600499A RID: 18842 RVA: 0x00101327 File Offset: 0x00100327
		public override MethodAttributes Attributes
		{
			get
			{
				return this.m_dynMethod.Attributes;
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x0600499B RID: 18843 RVA: 0x00101334 File Offset: 0x00100334
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.m_dynMethod.CallingConvention;
			}
		}

		// Token: 0x0600499C RID: 18844 RVA: 0x00101341 File Offset: 0x00100341
		public override MethodInfo GetBaseDefinition()
		{
			return this;
		}

		// Token: 0x0600499D RID: 18845 RVA: 0x00101344 File Offset: 0x00100344
		public override ParameterInfo[] GetParameters()
		{
			return this.m_dynMethod.GetParameters();
		}

		// Token: 0x0600499E RID: 18846 RVA: 0x00101351 File Offset: 0x00100351
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.m_dynMethod.GetMethodImplementationFlags();
		}

		// Token: 0x0600499F RID: 18847 RVA: 0x00101360 File Offset: 0x00100360
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			RuntimeMethodHandle methodDescriptor = this.GetMethodDescriptor();
			if ((this.CallingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_CallToVarArg"));
			}
			RuntimeTypeHandle[] array = new RuntimeTypeHandle[this.m_parameterTypes.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.m_parameterTypes[i].TypeHandle;
			}
			Signature signature = new Signature(methodDescriptor, array, this.m_returnType.TypeHandle, this.CallingConvention);
			int num = signature.Arguments.Length;
			int num2 = ((parameters != null) ? parameters.Length : 0);
			if (num != num2)
			{
				throw new TargetParameterCountException(Environment.GetResourceString("Arg_ParmCnt"));
			}
			object obj2;
			if (num2 > 0)
			{
				object[] array2 = base.CheckArguments(parameters, binder, invokeAttr, culture, signature);
				obj2 = methodDescriptor.InvokeMethodFast(null, array2, signature, this.Attributes, RuntimeTypeHandle.EmptyHandle);
				for (int j = 0; j < num2; j++)
				{
					parameters[j] = array2[j];
				}
			}
			else
			{
				obj2 = methodDescriptor.InvokeMethodFast(null, null, signature, this.Attributes, RuntimeTypeHandle.EmptyHandle);
			}
			GC.KeepAlive(this);
			return obj2;
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x00101475 File Offset: 0x00100475
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.m_dynMethod.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x00101484 File Offset: 0x00100484
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.m_dynMethod.GetCustomAttributes(inherit);
		}

		// Token: 0x060049A2 RID: 18850 RVA: 0x00101492 File Offset: 0x00100492
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.m_dynMethod.IsDefined(attributeType, inherit);
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x060049A3 RID: 18851 RVA: 0x001014A1 File Offset: 0x001004A1
		public override Type ReturnType
		{
			get
			{
				return this.m_dynMethod.ReturnType;
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x060049A4 RID: 18852 RVA: 0x001014AE File Offset: 0x001004AE
		public override ParameterInfo ReturnParameter
		{
			get
			{
				return this.m_dynMethod.ReturnParameter;
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x060049A5 RID: 18853 RVA: 0x001014BB File Offset: 0x001004BB
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				return this.m_dynMethod.ReturnTypeCustomAttributes;
			}
		}

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x060049A6 RID: 18854 RVA: 0x001014C8 File Offset: 0x001004C8
		internal override bool IsOverloaded
		{
			get
			{
				return this.m_dynMethod.IsOverloaded;
			}
		}

		// Token: 0x060049A7 RID: 18855 RVA: 0x001014D8 File Offset: 0x001004D8
		public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string parameterName)
		{
			if (position < 0 || position > this.m_parameterTypes.Length)
			{
				throw new ArgumentOutOfRangeException(Environment.GetResourceString("ArgumentOutOfRange_ParamSequence"));
			}
			position--;
			if (position >= 0)
			{
				ParameterInfo[] array = this.m_dynMethod.LoadParameters();
				array[position].SetName(parameterName);
				array[position].SetAttributes(attributes);
			}
			return null;
		}

		// Token: 0x060049A8 RID: 18856 RVA: 0x0010152C File Offset: 0x0010052C
		public DynamicILInfo GetDynamicILInfo()
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			if (this.m_DynamicILInfo != null)
			{
				return this.m_DynamicILInfo;
			}
			return this.GetDynamicILInfo(new DynamicScope());
		}

		// Token: 0x060049A9 RID: 18857 RVA: 0x00101554 File Offset: 0x00100554
		internal DynamicILInfo GetDynamicILInfo(DynamicScope scope)
		{
			if (this.m_DynamicILInfo == null)
			{
				byte[] signature = SignatureHelper.GetMethodSigHelper(null, this.CallingConvention, this.ReturnType, null, null, this.m_parameterTypes, null, null).GetSignature(true);
				this.m_DynamicILInfo = new DynamicILInfo(scope, this, signature);
			}
			return this.m_DynamicILInfo;
		}

		// Token: 0x060049AA RID: 18858 RVA: 0x001015A0 File Offset: 0x001005A0
		public ILGenerator GetILGenerator()
		{
			return this.GetILGenerator(64);
		}

		// Token: 0x060049AB RID: 18859 RVA: 0x001015AC File Offset: 0x001005AC
		public ILGenerator GetILGenerator(int streamSize)
		{
			if (this.m_ilGenerator == null)
			{
				byte[] signature = SignatureHelper.GetMethodSigHelper(null, this.CallingConvention, this.ReturnType, null, null, this.m_parameterTypes, null, null).GetSignature(true);
				this.m_ilGenerator = new DynamicILGenerator(this, signature, streamSize);
			}
			return this.m_ilGenerator;
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x060049AC RID: 18860 RVA: 0x001015F8 File Offset: 0x001005F8
		// (set) Token: 0x060049AD RID: 18861 RVA: 0x00101600 File Offset: 0x00100600
		public bool InitLocals
		{
			get
			{
				return this.m_fInitLocals;
			}
			set
			{
				this.m_fInitLocals = value;
			}
		}

		// Token: 0x060049AE RID: 18862 RVA: 0x00101609 File Offset: 0x00100609
		internal MethodInfo GetMethodInfo()
		{
			return this.m_dynMethod;
		}

		// Token: 0x0400257B RID: 9595
		private RuntimeType[] m_parameterTypes;

		// Token: 0x0400257C RID: 9596
		private RuntimeType m_returnType;

		// Token: 0x0400257D RID: 9597
		private DynamicILGenerator m_ilGenerator;

		// Token: 0x0400257E RID: 9598
		private DynamicILInfo m_DynamicILInfo;

		// Token: 0x0400257F RID: 9599
		private bool m_fInitLocals;

		// Token: 0x04002580 RID: 9600
		internal RuntimeMethodHandle m_method;

		// Token: 0x04002581 RID: 9601
		internal ModuleHandle m_module;

		// Token: 0x04002582 RID: 9602
		internal bool m_skipVisibility;

		// Token: 0x04002583 RID: 9603
		internal RuntimeType m_typeOwner;

		// Token: 0x04002584 RID: 9604
		private DynamicMethod.RTDynamicMethod m_dynMethod;

		// Token: 0x04002585 RID: 9605
		internal DynamicResolver m_resolver;

		// Token: 0x04002586 RID: 9606
		internal bool m_restrictedSkipVisibility;

		// Token: 0x04002587 RID: 9607
		internal CompressedStack m_creationContext;

		// Token: 0x04002588 RID: 9608
		private static Module s_anonymouslyHostedDynamicMethodsModule;

		// Token: 0x04002589 RID: 9609
		private static readonly object s_anonymouslyHostedDynamicMethodsModuleLock = new object();

		// Token: 0x0200080B RID: 2059
		internal class RTDynamicMethod : MethodInfo
		{
			// Token: 0x060049B0 RID: 18864 RVA: 0x0010161D File Offset: 0x0010061D
			private RTDynamicMethod()
			{
			}

			// Token: 0x060049B1 RID: 18865 RVA: 0x00101625 File Offset: 0x00100625
			internal RTDynamicMethod(DynamicMethod owner, string name, MethodAttributes attributes, CallingConventions callingConvention)
			{
				this.m_owner = owner;
				this.m_name = name;
				this.m_attributes = attributes;
				this.m_callingConvention = callingConvention;
			}

			// Token: 0x060049B2 RID: 18866 RVA: 0x0010164A File Offset: 0x0010064A
			public override string ToString()
			{
				return this.ReturnType.SigToString() + " " + RuntimeMethodInfo.ConstructName(this);
			}

			// Token: 0x17000CB5 RID: 3253
			// (get) Token: 0x060049B3 RID: 18867 RVA: 0x00101667 File Offset: 0x00100667
			public override string Name
			{
				get
				{
					return this.m_name;
				}
			}

			// Token: 0x17000CB6 RID: 3254
			// (get) Token: 0x060049B4 RID: 18868 RVA: 0x0010166F File Offset: 0x0010066F
			public override Type DeclaringType
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17000CB7 RID: 3255
			// (get) Token: 0x060049B5 RID: 18869 RVA: 0x00101672 File Offset: 0x00100672
			public override Type ReflectedType
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17000CB8 RID: 3256
			// (get) Token: 0x060049B6 RID: 18870 RVA: 0x00101675 File Offset: 0x00100675
			internal override int MetadataTokenInternal
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x17000CB9 RID: 3257
			// (get) Token: 0x060049B7 RID: 18871 RVA: 0x00101678 File Offset: 0x00100678
			public override Module Module
			{
				get
				{
					return this.m_owner.m_module.GetModule();
				}
			}

			// Token: 0x17000CBA RID: 3258
			// (get) Token: 0x060049B8 RID: 18872 RVA: 0x0010168A File Offset: 0x0010068A
			public override RuntimeMethodHandle MethodHandle
			{
				get
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
				}
			}

			// Token: 0x17000CBB RID: 3259
			// (get) Token: 0x060049B9 RID: 18873 RVA: 0x0010169B File Offset: 0x0010069B
			public override MethodAttributes Attributes
			{
				get
				{
					return this.m_attributes;
				}
			}

			// Token: 0x17000CBC RID: 3260
			// (get) Token: 0x060049BA RID: 18874 RVA: 0x001016A3 File Offset: 0x001006A3
			public override CallingConventions CallingConvention
			{
				get
				{
					return this.m_callingConvention;
				}
			}

			// Token: 0x060049BB RID: 18875 RVA: 0x001016AB File Offset: 0x001006AB
			public override MethodInfo GetBaseDefinition()
			{
				return this;
			}

			// Token: 0x060049BC RID: 18876 RVA: 0x001016B0 File Offset: 0x001006B0
			public override ParameterInfo[] GetParameters()
			{
				ParameterInfo[] array = this.LoadParameters();
				ParameterInfo[] array2 = new ParameterInfo[array.Length];
				Array.Copy(array, array2, array.Length);
				return array2;
			}

			// Token: 0x060049BD RID: 18877 RVA: 0x001016D8 File Offset: 0x001006D8
			public override MethodImplAttributes GetMethodImplementationFlags()
			{
				return MethodImplAttributes.NoInlining;
			}

			// Token: 0x060049BE RID: 18878 RVA: 0x001016DB File Offset: 0x001006DB
			public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeMethodInfo"), "this");
			}

			// Token: 0x060049BF RID: 18879 RVA: 0x001016F4 File Offset: 0x001006F4
			public override object[] GetCustomAttributes(Type attributeType, bool inherit)
			{
				if (attributeType == null)
				{
					throw new ArgumentNullException("attributeType");
				}
				if (attributeType.IsAssignableFrom(typeof(MethodImplAttribute)))
				{
					return new object[]
					{
						new MethodImplAttribute(this.GetMethodImplementationFlags())
					};
				}
				return new object[0];
			}

			// Token: 0x060049C0 RID: 18880 RVA: 0x00101740 File Offset: 0x00100740
			public override object[] GetCustomAttributes(bool inherit)
			{
				return new object[]
				{
					new MethodImplAttribute(this.GetMethodImplementationFlags())
				};
			}

			// Token: 0x060049C1 RID: 18881 RVA: 0x00101763 File Offset: 0x00100763
			public override bool IsDefined(Type attributeType, bool inherit)
			{
				if (attributeType == null)
				{
					throw new ArgumentNullException("attributeType");
				}
				return attributeType.IsAssignableFrom(typeof(MethodImplAttribute));
			}

			// Token: 0x060049C2 RID: 18882 RVA: 0x00101788 File Offset: 0x00100788
			internal override Type GetReturnType()
			{
				return this.m_owner.m_returnType;
			}

			// Token: 0x17000CBD RID: 3261
			// (get) Token: 0x060049C3 RID: 18883 RVA: 0x00101795 File Offset: 0x00100795
			public override ParameterInfo ReturnParameter
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17000CBE RID: 3262
			// (get) Token: 0x060049C4 RID: 18884 RVA: 0x00101798 File Offset: 0x00100798
			public override ICustomAttributeProvider ReturnTypeCustomAttributes
			{
				get
				{
					return this.GetEmptyCAHolder();
				}
			}

			// Token: 0x17000CBF RID: 3263
			// (get) Token: 0x060049C5 RID: 18885 RVA: 0x001017A0 File Offset: 0x001007A0
			internal override bool IsOverloaded
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060049C6 RID: 18886 RVA: 0x001017A4 File Offset: 0x001007A4
			internal ParameterInfo[] LoadParameters()
			{
				if (this.m_parameters == null)
				{
					RuntimeType[] parameterTypes = this.m_owner.m_parameterTypes;
					ParameterInfo[] array = new ParameterInfo[parameterTypes.Length];
					for (int i = 0; i < parameterTypes.Length; i++)
					{
						array[i] = new ParameterInfo(this, null, parameterTypes[i], i);
					}
					if (this.m_parameters == null)
					{
						this.m_parameters = array;
					}
				}
				return this.m_parameters;
			}

			// Token: 0x060049C7 RID: 18887 RVA: 0x001017FF File Offset: 0x001007FF
			private ICustomAttributeProvider GetEmptyCAHolder()
			{
				return new DynamicMethod.RTDynamicMethod.EmptyCAHolder();
			}

			// Token: 0x0400258A RID: 9610
			internal DynamicMethod m_owner;

			// Token: 0x0400258B RID: 9611
			private ParameterInfo[] m_parameters;

			// Token: 0x0400258C RID: 9612
			private string m_name;

			// Token: 0x0400258D RID: 9613
			private MethodAttributes m_attributes;

			// Token: 0x0400258E RID: 9614
			private CallingConventions m_callingConvention;

			// Token: 0x0200080C RID: 2060
			private class EmptyCAHolder : ICustomAttributeProvider
			{
				// Token: 0x060049C8 RID: 18888 RVA: 0x00101806 File Offset: 0x00100806
				internal EmptyCAHolder()
				{
				}

				// Token: 0x060049C9 RID: 18889 RVA: 0x0010180E File Offset: 0x0010080E
				object[] ICustomAttributeProvider.GetCustomAttributes(Type attributeType, bool inherit)
				{
					return new object[0];
				}

				// Token: 0x060049CA RID: 18890 RVA: 0x00101816 File Offset: 0x00100816
				object[] ICustomAttributeProvider.GetCustomAttributes(bool inherit)
				{
					return new object[0];
				}

				// Token: 0x060049CB RID: 18891 RVA: 0x0010181E File Offset: 0x0010081E
				bool ICustomAttributeProvider.IsDefined(Type attributeType, bool inherit)
				{
					return false;
				}
			}
		}
	}
}
