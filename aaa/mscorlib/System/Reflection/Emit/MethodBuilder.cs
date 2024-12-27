using System;
using System.Collections;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Reflection.Emit
{
	// Token: 0x02000819 RID: 2073
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_MethodBuilder))]
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class MethodBuilder : MethodInfo, _MethodBuilder
	{
		// Token: 0x06004A3E RID: 19006 RVA: 0x00102A74 File Offset: 0x00101A74
		internal MethodBuilder(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Module mod, TypeBuilder type, bool bIsGlobalMethod)
		{
			this.Init(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null, mod, type, bIsGlobalMethod);
		}

		// Token: 0x06004A3F RID: 19007 RVA: 0x00102AA0 File Offset: 0x00101AA0
		internal MethodBuilder(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers, Module mod, TypeBuilder type, bool bIsGlobalMethod)
		{
			this.Init(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, mod, type, bIsGlobalMethod);
		}

		// Token: 0x06004A40 RID: 19008 RVA: 0x00102AD0 File Offset: 0x00101AD0
		private void Init(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers, Module mod, TypeBuilder type, bool bIsGlobalMethod)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if (name[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IllegalName"), "name");
			}
			if (mod == null)
			{
				throw new ArgumentNullException("mod");
			}
			if (parameterTypes != null)
			{
				for (int i = 0; i < parameterTypes.Length; i++)
				{
					if (parameterTypes[i] == null)
					{
						throw new ArgumentNullException("parameterTypes");
					}
				}
			}
			this.m_link = type.m_currentMethod;
			type.m_currentMethod = this;
			this.m_strName = name;
			this.m_module = mod;
			this.m_containingType = type;
			this.m_localSignature = SignatureHelper.GetLocalVarSigHelper(mod);
			this.m_returnType = returnType;
			if ((attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
			{
				callingConvention |= CallingConventions.HasThis;
			}
			else if ((attributes & MethodAttributes.Virtual) != MethodAttributes.PrivateScope)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_NoStaticVirtual"));
			}
			if ((attributes & MethodAttributes.SpecialName) != MethodAttributes.SpecialName && (type.Attributes & TypeAttributes.ClassSemanticsMask) == TypeAttributes.ClassSemanticsMask && (attributes & (MethodAttributes.Virtual | MethodAttributes.Abstract)) != (MethodAttributes.Virtual | MethodAttributes.Abstract) && (attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadAttributeOnInterfaceMethod"));
			}
			this.m_callingConvention = callingConvention;
			if (parameterTypes != null)
			{
				this.m_parameterTypes = new Type[parameterTypes.Length];
				Array.Copy(parameterTypes, this.m_parameterTypes, parameterTypes.Length);
			}
			else
			{
				this.m_parameterTypes = null;
			}
			this.m_returnTypeRequiredCustomModifiers = returnTypeRequiredCustomModifiers;
			this.m_returnTypeOptionalCustomModifiers = returnTypeOptionalCustomModifiers;
			this.m_parameterTypeRequiredCustomModifiers = parameterTypeRequiredCustomModifiers;
			this.m_parameterTypeOptionalCustomModifiers = parameterTypeOptionalCustomModifiers;
			this.m_iAttributes = attributes;
			this.m_bIsGlobalMethod = bIsGlobalMethod;
			this.m_bIsBaked = false;
			this.m_fInitLocals = true;
			this.m_localSymInfo = new LocalSymInfo();
			this.m_ubBody = null;
			this.m_ilGenerator = null;
			this.m_dwMethodImplFlags = MethodImplAttributes.IL;
		}

		// Token: 0x06004A41 RID: 19009 RVA: 0x00102C92 File Offset: 0x00101C92
		internal void CheckContext(params Type[][] typess)
		{
			((AssemblyBuilder)this.Module.Assembly).CheckContext(typess);
		}

		// Token: 0x06004A42 RID: 19010 RVA: 0x00102CAA File Offset: 0x00101CAA
		internal void CheckContext(params Type[] types)
		{
			((AssemblyBuilder)this.Module.Assembly).CheckContext(types);
		}

		// Token: 0x06004A43 RID: 19011 RVA: 0x00102CC4 File Offset: 0x00101CC4
		internal void CreateMethodBodyHelper(ILGenerator il)
		{
			int num = 0;
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_module;
			this.m_containingType.ThrowIfCreated();
			if (this.m_bIsBaked)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MethodHasBody"));
			}
			if (il == null)
			{
				throw new ArgumentNullException("il");
			}
			if (il.m_methodBuilder != this && il.m_methodBuilder != null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadILGeneratorUsage"));
			}
			this.ThrowIfShouldNotHaveBody();
			if (il.m_ScopeTree.m_iOpenScopeCount != 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_OpenLocalVariableScope"));
			}
			this.m_ubBody = il.BakeByteArray();
			this.m_RVAFixups = il.GetRVAFixups();
			this.m_mdMethodFixups = il.GetTokenFixups();
			__ExceptionInfo[] exceptions = il.GetExceptions();
			this.m_numExceptions = this.CalculateNumberOfExceptions(exceptions);
			if (this.m_numExceptions > 0)
			{
				this.m_exceptions = new __ExceptionInstance[this.m_numExceptions];
				for (int i = 0; i < exceptions.Length; i++)
				{
					int[] filterAddresses = exceptions[i].GetFilterAddresses();
					int[] catchAddresses = exceptions[i].GetCatchAddresses();
					int[] catchEndAddresses = exceptions[i].GetCatchEndAddresses();
					Type[] catchClass = exceptions[i].GetCatchClass();
					for (int j = 0; j < catchClass.Length; j++)
					{
						if (catchClass[j] != null)
						{
							moduleBuilder.GetTypeTokenInternal(catchClass[j]);
						}
					}
					int numberOfCatches = exceptions[i].GetNumberOfCatches();
					int startAddress = exceptions[i].GetStartAddress();
					int endAddress = exceptions[i].GetEndAddress();
					int[] exceptionTypes = exceptions[i].GetExceptionTypes();
					for (int k = 0; k < numberOfCatches; k++)
					{
						int num2 = 0;
						if (catchClass[k] != null)
						{
							num2 = moduleBuilder.GetTypeTokenInternal(catchClass[k]).Token;
						}
						switch (exceptionTypes[k])
						{
						case 0:
						case 1:
						case 4:
							this.m_exceptions[num++] = new __ExceptionInstance(startAddress, endAddress, filterAddresses[k], catchAddresses[k], catchEndAddresses[k], exceptionTypes[k], num2);
							break;
						case 2:
							this.m_exceptions[num++] = new __ExceptionInstance(startAddress, exceptions[i].GetFinallyEndAddress(), filterAddresses[k], catchAddresses[k], catchEndAddresses[k], exceptionTypes[k], num2);
							break;
						}
					}
				}
			}
			this.m_bIsBaked = true;
			if (moduleBuilder.GetSymWriter() != null)
			{
				SymbolToken symbolToken = new SymbolToken(this.MetadataTokenInternal);
				ISymbolWriter symWriter = moduleBuilder.GetSymWriter();
				symWriter.OpenMethod(symbolToken);
				symWriter.OpenScope(0);
				if (this.m_symCustomAttrs != null)
				{
					foreach (object obj in this.m_symCustomAttrs)
					{
						MethodBuilder.SymCustomAttr symCustomAttr = (MethodBuilder.SymCustomAttr)obj;
						moduleBuilder.GetSymWriter().SetSymAttribute(new SymbolToken(this.MetadataTokenInternal), symCustomAttr.m_name, symCustomAttr.m_data);
					}
				}
				if (this.m_localSymInfo != null)
				{
					this.m_localSymInfo.EmitLocalSymInfo(symWriter);
				}
				il.m_ScopeTree.EmitScopeTree(symWriter);
				il.m_LineNumberInfo.EmitLineNumberInfo(symWriter);
				symWriter.CloseScope(il.m_length);
				symWriter.CloseMethod();
			}
		}

		// Token: 0x06004A44 RID: 19012 RVA: 0x00103000 File Offset: 0x00102000
		internal void ReleaseBakedStructures()
		{
			if (!this.m_bIsBaked)
			{
				return;
			}
			this.m_ubBody = null;
			this.m_localSymInfo = null;
			this.m_RVAFixups = null;
			this.m_mdMethodFixups = null;
			this.m_exceptions = null;
		}

		// Token: 0x06004A45 RID: 19013 RVA: 0x0010302E File Offset: 0x0010202E
		internal override Type[] GetParameterTypes()
		{
			if (this.m_parameterTypes == null)
			{
				this.m_parameterTypes = new Type[0];
			}
			return this.m_parameterTypes;
		}

		// Token: 0x06004A46 RID: 19014 RVA: 0x0010304A File Offset: 0x0010204A
		internal void SetToken(MethodToken token)
		{
			this.m_tkMethod = token;
		}

		// Token: 0x06004A47 RID: 19015 RVA: 0x00103053 File Offset: 0x00102053
		internal byte[] GetBody()
		{
			return this.m_ubBody;
		}

		// Token: 0x06004A48 RID: 19016 RVA: 0x0010305B File Offset: 0x0010205B
		internal int[] GetTokenFixups()
		{
			return this.m_mdMethodFixups;
		}

		// Token: 0x06004A49 RID: 19017 RVA: 0x00103063 File Offset: 0x00102063
		internal int[] GetRVAFixups()
		{
			return this.m_RVAFixups;
		}

		// Token: 0x06004A4A RID: 19018 RVA: 0x0010306C File Offset: 0x0010206C
		internal SignatureHelper GetMethodSignature()
		{
			if (this.m_parameterTypes == null)
			{
				this.m_parameterTypes = new Type[0];
			}
			this.m_signature = SignatureHelper.GetMethodSigHelper(this.m_module, this.m_callingConvention, (this.m_inst != null) ? this.m_inst.Length : 0, (this.m_returnType == null) ? typeof(void) : this.m_returnType, this.m_returnTypeRequiredCustomModifiers, this.m_returnTypeOptionalCustomModifiers, this.m_parameterTypes, this.m_parameterTypeRequiredCustomModifiers, this.m_parameterTypeOptionalCustomModifiers);
			return this.m_signature;
		}

		// Token: 0x06004A4B RID: 19019 RVA: 0x001030F5 File Offset: 0x001020F5
		internal SignatureHelper GetLocalsSignature()
		{
			if (this.m_ilGenerator != null && this.m_ilGenerator.m_localCount != 0)
			{
				return this.m_ilGenerator.m_localSignature;
			}
			return this.m_localSignature;
		}

		// Token: 0x06004A4C RID: 19020 RVA: 0x0010311E File Offset: 0x0010211E
		internal int GetNumberOfExceptions()
		{
			return this.m_numExceptions;
		}

		// Token: 0x06004A4D RID: 19021 RVA: 0x00103126 File Offset: 0x00102126
		internal __ExceptionInstance[] GetExceptionInstances()
		{
			return this.m_exceptions;
		}

		// Token: 0x06004A4E RID: 19022 RVA: 0x00103130 File Offset: 0x00102130
		internal int CalculateNumberOfExceptions(__ExceptionInfo[] excp)
		{
			int num = 0;
			if (excp == null)
			{
				return 0;
			}
			for (int i = 0; i < excp.Length; i++)
			{
				num += excp[i].GetNumberOfCatches();
			}
			return num;
		}

		// Token: 0x06004A4F RID: 19023 RVA: 0x0010315E File Offset: 0x0010215E
		internal bool IsTypeCreated()
		{
			return this.m_containingType != null && this.m_containingType.m_hasBeenCreated;
		}

		// Token: 0x06004A50 RID: 19024 RVA: 0x00103175 File Offset: 0x00102175
		internal TypeBuilder GetTypeBuilder()
		{
			return this.m_containingType;
		}

		// Token: 0x06004A51 RID: 19025 RVA: 0x00103180 File Offset: 0x00102180
		public override bool Equals(object obj)
		{
			if (!(obj is MethodBuilder))
			{
				return false;
			}
			if (!this.m_strName.Equals(((MethodBuilder)obj).m_strName))
			{
				return false;
			}
			if (this.m_iAttributes != ((MethodBuilder)obj).m_iAttributes)
			{
				return false;
			}
			SignatureHelper methodSignature = ((MethodBuilder)obj).GetMethodSignature();
			return methodSignature.Equals(this.GetMethodSignature());
		}

		// Token: 0x06004A52 RID: 19026 RVA: 0x001031E3 File Offset: 0x001021E3
		public override int GetHashCode()
		{
			return this.m_strName.GetHashCode();
		}

		// Token: 0x06004A53 RID: 19027 RVA: 0x001031F0 File Offset: 0x001021F0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(1000);
			stringBuilder.Append("Name: " + this.m_strName + " " + Environment.NewLine);
			stringBuilder.Append("Attributes: " + (int)this.m_iAttributes + Environment.NewLine);
			stringBuilder.Append("Method Signature: " + this.GetMethodSignature() + Environment.NewLine);
			stringBuilder.Append(Environment.NewLine);
			return stringBuilder.ToString();
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06004A54 RID: 19028 RVA: 0x00103278 File Offset: 0x00102278
		public override string Name
		{
			get
			{
				return this.m_strName;
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06004A55 RID: 19029 RVA: 0x00103280 File Offset: 0x00102280
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.GetToken().Token;
			}
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06004A56 RID: 19030 RVA: 0x0010329B File Offset: 0x0010229B
		public override Module Module
		{
			get
			{
				return this.m_containingType.Module;
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x06004A57 RID: 19031 RVA: 0x001032A8 File Offset: 0x001022A8
		public override Type DeclaringType
		{
			get
			{
				if (this.m_containingType.m_isHiddenGlobalType)
				{
					return null;
				}
				return this.m_containingType;
			}
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06004A58 RID: 19032 RVA: 0x001032BF File Offset: 0x001022BF
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x06004A59 RID: 19033 RVA: 0x001032C2 File Offset: 0x001022C2
		public override Type ReflectedType
		{
			get
			{
				if (this.m_containingType.m_isHiddenGlobalType)
				{
					return null;
				}
				return this.m_containingType;
			}
		}

		// Token: 0x06004A5A RID: 19034 RVA: 0x001032D9 File Offset: 0x001022D9
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x001032EA File Offset: 0x001022EA
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.m_dwMethodImplFlags;
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06004A5C RID: 19036 RVA: 0x001032F2 File Offset: 0x001022F2
		public override MethodAttributes Attributes
		{
			get
			{
				return this.m_iAttributes;
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x06004A5D RID: 19037 RVA: 0x001032FA File Offset: 0x001022FA
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.m_callingConvention;
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x06004A5E RID: 19038 RVA: 0x00103302 File Offset: 0x00102302
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
		}

		// Token: 0x06004A5F RID: 19039 RVA: 0x00103313 File Offset: 0x00102313
		public override MethodInfo GetBaseDefinition()
		{
			return this;
		}

		// Token: 0x06004A60 RID: 19040 RVA: 0x00103316 File Offset: 0x00102316
		internal override Type GetReturnType()
		{
			return this.m_returnType;
		}

		// Token: 0x06004A61 RID: 19041 RVA: 0x00103320 File Offset: 0x00102320
		public override ParameterInfo[] GetParameters()
		{
			if (!this.m_bIsBaked)
			{
				throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_TypeNotCreated"));
			}
			Type runtimeType = this.m_containingType.m_runtimeType;
			MethodInfo method = runtimeType.GetMethod(this.m_strName, this.m_parameterTypes);
			return method.GetParameters();
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x06004A62 RID: 19042 RVA: 0x0010336C File Offset: 0x0010236C
		public override ParameterInfo ReturnParameter
		{
			get
			{
				if (!this.m_bIsBaked)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TypeNotCreated"));
				}
				Type runtimeType = this.m_containingType.m_runtimeType;
				MethodInfo method = runtimeType.GetMethod(this.m_strName, this.m_parameterTypes);
				return method.ReturnParameter;
			}
		}

		// Token: 0x06004A63 RID: 19043 RVA: 0x001033B6 File Offset: 0x001023B6
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004A64 RID: 19044 RVA: 0x001033C7 File Offset: 0x001023C7
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004A65 RID: 19045 RVA: 0x001033D8 File Offset: 0x001023D8
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x06004A66 RID: 19046 RVA: 0x001033E9 File Offset: 0x001023E9
		public override bool IsGenericMethodDefinition
		{
			get
			{
				return this.m_bIsGenMethDef;
			}
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06004A67 RID: 19047 RVA: 0x001033F1 File Offset: 0x001023F1
		public override bool ContainsGenericParameters
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x001033F8 File Offset: 0x001023F8
		public override MethodInfo GetGenericMethodDefinition()
		{
			if (!this.IsGenericMethod)
			{
				throw new InvalidOperationException();
			}
			return this;
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x06004A69 RID: 19049 RVA: 0x00103409 File Offset: 0x00102409
		public override bool IsGenericMethod
		{
			get
			{
				return this.m_inst != null;
			}
		}

		// Token: 0x06004A6A RID: 19050 RVA: 0x00103417 File Offset: 0x00102417
		public override Type[] GetGenericArguments()
		{
			return this.m_inst;
		}

		// Token: 0x06004A6B RID: 19051 RVA: 0x0010341F File Offset: 0x0010241F
		public override MethodInfo MakeGenericMethod(params Type[] typeArguments)
		{
			return new MethodBuilderInstantiation(this, typeArguments);
		}

		// Token: 0x06004A6C RID: 19052 RVA: 0x00103428 File Offset: 0x00102428
		public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
		{
			if (this.m_inst != null)
			{
				throw new InvalidOperationException();
			}
			if (names == null)
			{
				throw new ArgumentNullException("names");
			}
			for (int i = 0; i < names.Length; i++)
			{
				if (names[i] == null)
				{
					throw new ArgumentNullException("names");
				}
			}
			if (this.m_tkMethod.Token != 0)
			{
				throw new InvalidOperationException();
			}
			if (names.Length == 0)
			{
				throw new ArgumentException();
			}
			this.m_bIsGenMethDef = true;
			this.m_inst = new GenericTypeParameterBuilder[names.Length];
			for (int j = 0; j < names.Length; j++)
			{
				this.m_inst[j] = new GenericTypeParameterBuilder(new TypeBuilder(names[j], j, this));
			}
			return this.m_inst;
		}

		// Token: 0x06004A6D RID: 19053 RVA: 0x001034CD File Offset: 0x001024CD
		internal void ThrowIfGeneric()
		{
			if (this.IsGenericMethod && !this.IsGenericMethodDefinition)
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06004A6E RID: 19054 RVA: 0x001034E8 File Offset: 0x001024E8
		public MethodToken GetToken()
		{
			if (this.m_tkMethod.Token == 0)
			{
				if (this.m_link != null)
				{
					this.m_link.GetToken();
				}
				int num;
				byte[] array = this.GetMethodSignature().InternalGetSignature(out num);
				this.m_tkMethod = new MethodToken(TypeBuilder.InternalDefineMethod(this.m_containingType.MetadataTokenInternal, this.m_strName, array, num, this.Attributes, this.m_module));
				if (this.m_inst != null)
				{
					foreach (GenericTypeParameterBuilder genericTypeParameterBuilder in this.m_inst)
					{
						if (!genericTypeParameterBuilder.m_type.IsCreated())
						{
							genericTypeParameterBuilder.m_type.CreateType();
						}
					}
				}
				TypeBuilder.InternalSetMethodImpl(this.m_module, this.MetadataTokenInternal, this.m_dwMethodImplFlags);
			}
			return this.m_tkMethod;
		}

		// Token: 0x06004A6F RID: 19055 RVA: 0x001035B2 File Offset: 0x001025B2
		public void SetParameters(params Type[] parameterTypes)
		{
			this.CheckContext(parameterTypes);
			this.SetSignature(null, null, null, parameterTypes, null, null);
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x001035C8 File Offset: 0x001025C8
		public void SetReturnType(Type returnType)
		{
			this.CheckContext(new Type[] { returnType });
			this.SetSignature(returnType, null, null, null, null, null);
		}

		// Token: 0x06004A71 RID: 19057 RVA: 0x001035F4 File Offset: 0x001025F4
		public void SetSignature(Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			this.CheckContext(new Type[] { returnType });
			this.CheckContext(new Type[][] { returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes });
			this.CheckContext(parameterTypeRequiredCustomModifiers);
			this.CheckContext(parameterTypeOptionalCustomModifiers);
			this.ThrowIfGeneric();
			if (returnType != null)
			{
				this.m_returnType = returnType;
			}
			if (parameterTypes != null)
			{
				this.m_parameterTypes = new Type[parameterTypes.Length];
				Array.Copy(parameterTypes, this.m_parameterTypes, parameterTypes.Length);
			}
			this.m_returnTypeRequiredCustomModifiers = returnTypeRequiredCustomModifiers;
			this.m_returnTypeOptionalCustomModifiers = returnTypeOptionalCustomModifiers;
			this.m_parameterTypeRequiredCustomModifiers = parameterTypeRequiredCustomModifiers;
			this.m_parameterTypeOptionalCustomModifiers = parameterTypeOptionalCustomModifiers;
		}

		// Token: 0x06004A72 RID: 19058 RVA: 0x00103690 File Offset: 0x00102690
		public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string strParamName)
		{
			this.ThrowIfGeneric();
			this.m_containingType.ThrowIfCreated();
			if (position < 0)
			{
				throw new ArgumentOutOfRangeException(Environment.GetResourceString("ArgumentOutOfRange_ParamSequence"));
			}
			if (position > 0 && (this.m_parameterTypes == null || position > this.m_parameterTypes.Length))
			{
				throw new ArgumentOutOfRangeException(Environment.GetResourceString("ArgumentOutOfRange_ParamSequence"));
			}
			attributes &= ~ParameterAttributes.ReservedMask;
			return new ParameterBuilder(this, position, attributes, strParamName);
		}

		// Token: 0x06004A73 RID: 19059 RVA: 0x001036FB File Offset: 0x001026FB
		[Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public void SetMarshal(UnmanagedMarshal unmanagedMarshal)
		{
			this.ThrowIfGeneric();
			this.m_containingType.ThrowIfCreated();
			if (this.m_retParam == null)
			{
				this.m_retParam = new ParameterBuilder(this, 0, ParameterAttributes.None, null);
			}
			this.m_retParam.SetMarshal(unmanagedMarshal);
		}

		// Token: 0x06004A74 RID: 19060 RVA: 0x00103734 File Offset: 0x00102734
		public void SetSymCustomAttribute(string name, byte[] data)
		{
			this.ThrowIfGeneric();
			this.m_containingType.ThrowIfCreated();
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_module;
			if (moduleBuilder.GetSymWriter() == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotADebugModule"));
			}
			if (this.m_symCustomAttrs == null)
			{
				this.m_symCustomAttrs = new ArrayList();
			}
			this.m_symCustomAttrs.Add(new MethodBuilder.SymCustomAttr(name, data));
		}

		// Token: 0x06004A75 RID: 19061 RVA: 0x001037A4 File Offset: 0x001027A4
		public void AddDeclarativeSecurity(SecurityAction action, PermissionSet pset)
		{
			this.ThrowIfGeneric();
			if (pset == null)
			{
				throw new ArgumentNullException("pset");
			}
			if (!Enum.IsDefined(typeof(SecurityAction), action) || action == SecurityAction.RequestMinimum || action == SecurityAction.RequestOptional || action == SecurityAction.RequestRefuse)
			{
				throw new ArgumentOutOfRangeException("action");
			}
			this.m_containingType.ThrowIfCreated();
			byte[] array = null;
			if (!pset.IsEmpty())
			{
				array = pset.EncodeXml();
			}
			TypeBuilder.InternalAddDeclarativeSecurity(this.m_module, this.MetadataTokenInternal, action, array);
		}

		// Token: 0x06004A76 RID: 19062 RVA: 0x00103824 File Offset: 0x00102824
		public void CreateMethodBody(byte[] il, int count)
		{
			this.ThrowIfGeneric();
			if (this.m_bIsBaked)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MethodBaked"));
			}
			this.m_containingType.ThrowIfCreated();
			if (il != null && (count < 0 || count > il.Length))
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (il == null)
			{
				this.m_ubBody = null;
				return;
			}
			this.m_ubBody = new byte[count];
			Array.Copy(il, this.m_ubBody, count);
			this.m_bIsBaked = true;
		}

		// Token: 0x06004A77 RID: 19063 RVA: 0x001038A7 File Offset: 0x001028A7
		public void SetImplementationFlags(MethodImplAttributes attributes)
		{
			this.ThrowIfGeneric();
			this.m_containingType.ThrowIfCreated();
			this.m_dwMethodImplFlags = attributes;
			this.m_canBeRuntimeImpl = true;
			TypeBuilder.InternalSetMethodImpl(this.m_module, this.MetadataTokenInternal, attributes);
		}

		// Token: 0x06004A78 RID: 19064 RVA: 0x001038DA File Offset: 0x001028DA
		public ILGenerator GetILGenerator()
		{
			this.ThrowIfGeneric();
			this.ThrowIfShouldNotHaveBody();
			if (this.m_ilGenerator == null)
			{
				this.m_ilGenerator = new ILGenerator(this);
			}
			return this.m_ilGenerator;
		}

		// Token: 0x06004A79 RID: 19065 RVA: 0x00103902 File Offset: 0x00102902
		public ILGenerator GetILGenerator(int size)
		{
			this.ThrowIfGeneric();
			this.ThrowIfShouldNotHaveBody();
			if (this.m_ilGenerator == null)
			{
				this.m_ilGenerator = new ILGenerator(this, size);
			}
			return this.m_ilGenerator;
		}

		// Token: 0x06004A7A RID: 19066 RVA: 0x0010392B File Offset: 0x0010292B
		private void ThrowIfShouldNotHaveBody()
		{
			if ((this.m_dwMethodImplFlags & MethodImplAttributes.CodeTypeMask) != MethodImplAttributes.IL || (this.m_dwMethodImplFlags & MethodImplAttributes.ManagedMask) != MethodImplAttributes.IL || (this.m_iAttributes & MethodAttributes.PinvokeImpl) != MethodAttributes.PrivateScope || this.m_isDllImport)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ShouldNotHaveMethodBody"));
			}
		}

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x06004A7B RID: 19067 RVA: 0x00103967 File Offset: 0x00102967
		// (set) Token: 0x06004A7C RID: 19068 RVA: 0x00103975 File Offset: 0x00102975
		public bool InitLocals
		{
			get
			{
				this.ThrowIfGeneric();
				return this.m_fInitLocals;
			}
			set
			{
				this.ThrowIfGeneric();
				this.m_fInitLocals = value;
			}
		}

		// Token: 0x06004A7D RID: 19069 RVA: 0x00103984 File Offset: 0x00102984
		public Module GetModule()
		{
			return this.m_module;
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x06004A7E RID: 19070 RVA: 0x0010398C File Offset: 0x0010298C
		public string Signature
		{
			get
			{
				return this.GetMethodSignature().ToString();
			}
		}

		// Token: 0x06004A7F RID: 19071 RVA: 0x0010399C File Offset: 0x0010299C
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			this.ThrowIfGeneric();
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			TypeBuilder.InternalCreateCustomAttribute(this.MetadataTokenInternal, ((ModuleBuilder)this.m_module).GetConstructorToken(con).Token, binaryAttribute, this.m_module, false);
			if (this.IsKnownCA(con))
			{
				this.ParseCA(con, binaryAttribute);
			}
		}

		// Token: 0x06004A80 RID: 19072 RVA: 0x00103A08 File Offset: 0x00102A08
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			this.ThrowIfGeneric();
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			customBuilder.CreateCustomAttribute((ModuleBuilder)this.m_module, this.MetadataTokenInternal);
			if (this.IsKnownCA(customBuilder.m_con))
			{
				this.ParseCA(customBuilder.m_con, customBuilder.m_blob);
			}
		}

		// Token: 0x06004A81 RID: 19073 RVA: 0x00103A60 File Offset: 0x00102A60
		private bool IsKnownCA(ConstructorInfo con)
		{
			Type declaringType = con.DeclaringType;
			return declaringType == typeof(MethodImplAttribute) || declaringType == typeof(DllImportAttribute);
		}

		// Token: 0x06004A82 RID: 19074 RVA: 0x00103A94 File Offset: 0x00102A94
		private void ParseCA(ConstructorInfo con, byte[] blob)
		{
			Type declaringType = con.DeclaringType;
			if (declaringType == typeof(MethodImplAttribute))
			{
				this.m_canBeRuntimeImpl = true;
				return;
			}
			if (declaringType == typeof(DllImportAttribute))
			{
				this.m_canBeRuntimeImpl = true;
				this.m_isDllImport = true;
			}
		}

		// Token: 0x06004A83 RID: 19075 RVA: 0x00103AD8 File Offset: 0x00102AD8
		void _MethodBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004A84 RID: 19076 RVA: 0x00103ADF File Offset: 0x00102ADF
		void _MethodBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004A85 RID: 19077 RVA: 0x00103AE6 File Offset: 0x00102AE6
		void _MethodBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004A86 RID: 19078 RVA: 0x00103AED File Offset: 0x00102AED
		void _MethodBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040025D4 RID: 9684
		internal string m_strName;

		// Token: 0x040025D5 RID: 9685
		private MethodToken m_tkMethod;

		// Token: 0x040025D6 RID: 9686
		internal Module m_module;

		// Token: 0x040025D7 RID: 9687
		internal TypeBuilder m_containingType;

		// Token: 0x040025D8 RID: 9688
		private MethodBuilder m_link;

		// Token: 0x040025D9 RID: 9689
		private int[] m_RVAFixups;

		// Token: 0x040025DA RID: 9690
		private int[] m_mdMethodFixups;

		// Token: 0x040025DB RID: 9691
		private SignatureHelper m_localSignature;

		// Token: 0x040025DC RID: 9692
		internal LocalSymInfo m_localSymInfo;

		// Token: 0x040025DD RID: 9693
		internal ILGenerator m_ilGenerator;

		// Token: 0x040025DE RID: 9694
		private byte[] m_ubBody;

		// Token: 0x040025DF RID: 9695
		private int m_numExceptions;

		// Token: 0x040025E0 RID: 9696
		private __ExceptionInstance[] m_exceptions;

		// Token: 0x040025E1 RID: 9697
		internal bool m_bIsBaked;

		// Token: 0x040025E2 RID: 9698
		private bool m_bIsGlobalMethod;

		// Token: 0x040025E3 RID: 9699
		private bool m_fInitLocals;

		// Token: 0x040025E4 RID: 9700
		private MethodAttributes m_iAttributes;

		// Token: 0x040025E5 RID: 9701
		private CallingConventions m_callingConvention;

		// Token: 0x040025E6 RID: 9702
		private MethodImplAttributes m_dwMethodImplFlags;

		// Token: 0x040025E7 RID: 9703
		private SignatureHelper m_signature;

		// Token: 0x040025E8 RID: 9704
		internal Type[] m_parameterTypes;

		// Token: 0x040025E9 RID: 9705
		private ParameterBuilder m_retParam;

		// Token: 0x040025EA RID: 9706
		internal Type m_returnType;

		// Token: 0x040025EB RID: 9707
		private Type[] m_returnTypeRequiredCustomModifiers;

		// Token: 0x040025EC RID: 9708
		private Type[] m_returnTypeOptionalCustomModifiers;

		// Token: 0x040025ED RID: 9709
		private Type[][] m_parameterTypeRequiredCustomModifiers;

		// Token: 0x040025EE RID: 9710
		private Type[][] m_parameterTypeOptionalCustomModifiers;

		// Token: 0x040025EF RID: 9711
		private GenericTypeParameterBuilder[] m_inst;

		// Token: 0x040025F0 RID: 9712
		private bool m_bIsGenMethDef;

		// Token: 0x040025F1 RID: 9713
		private ArrayList m_symCustomAttrs;

		// Token: 0x040025F2 RID: 9714
		internal bool m_canBeRuntimeImpl;

		// Token: 0x040025F3 RID: 9715
		internal bool m_isDllImport;

		// Token: 0x0200081A RID: 2074
		private struct SymCustomAttr
		{
			// Token: 0x06004A87 RID: 19079 RVA: 0x00103AF4 File Offset: 0x00102AF4
			public SymCustomAttr(string name, byte[] data)
			{
				this.m_name = name;
				this.m_data = data;
			}

			// Token: 0x040025F4 RID: 9716
			public string m_name;

			// Token: 0x040025F5 RID: 9717
			public byte[] m_data;
		}
	}
}
