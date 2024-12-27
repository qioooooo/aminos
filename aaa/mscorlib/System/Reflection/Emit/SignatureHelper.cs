using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Reflection.Emit
{
	// Token: 0x02000831 RID: 2097
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_SignatureHelper))]
	[ComVisible(true)]
	public sealed class SignatureHelper : _SignatureHelper
	{
		// Token: 0x06004BD7 RID: 19415 RVA: 0x0010A5B0 File Offset: 0x001095B0
		public static SignatureHelper GetMethodSigHelper(Module mod, Type returnType, Type[] parameterTypes)
		{
			return SignatureHelper.GetMethodSigHelper(mod, CallingConventions.Standard, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x0010A5C0 File Offset: 0x001095C0
		internal static SignatureHelper GetMethodSigHelper(Module mod, CallingConventions callingConvention, Type returnType, int cGenericParam)
		{
			return SignatureHelper.GetMethodSigHelper(mod, callingConvention, cGenericParam, returnType, null, null, null, null, null);
		}

		// Token: 0x06004BD9 RID: 19417 RVA: 0x0010A5DB File Offset: 0x001095DB
		public static SignatureHelper GetMethodSigHelper(Module mod, CallingConventions callingConvention, Type returnType)
		{
			return SignatureHelper.GetMethodSigHelper(mod, callingConvention, returnType, null, null, null, null, null);
		}

		// Token: 0x06004BDA RID: 19418 RVA: 0x0010A5EC File Offset: 0x001095EC
		internal static SignatureHelper GetMethodSpecSigHelper(Module scope, Type[] inst)
		{
			SignatureHelper signatureHelper = new SignatureHelper(scope, 10);
			signatureHelper.AddData(inst.Length);
			foreach (Type type in inst)
			{
				signatureHelper.AddArgument(type);
			}
			return signatureHelper;
		}

		// Token: 0x06004BDB RID: 19419 RVA: 0x0010A628 File Offset: 0x00109628
		internal static SignatureHelper GetMethodSigHelper(Module scope, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			return SignatureHelper.GetMethodSigHelper(scope, callingConvention, 0, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
		}

		// Token: 0x06004BDC RID: 19420 RVA: 0x0010A648 File Offset: 0x00109648
		internal static SignatureHelper GetMethodSigHelper(Module scope, CallingConventions callingConvention, int cGenericParam, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			if (returnType == null)
			{
				returnType = typeof(void);
			}
			int num = 0;
			if ((callingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs)
			{
				num = 5;
			}
			if (cGenericParam > 0)
			{
				num |= 16;
			}
			if ((callingConvention & CallingConventions.HasThis) == CallingConventions.HasThis)
			{
				num |= 32;
			}
			SignatureHelper signatureHelper = new SignatureHelper(scope, num, cGenericParam, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers);
			signatureHelper.AddArguments(parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
			return signatureHelper;
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x0010A6A0 File Offset: 0x001096A0
		public static SignatureHelper GetMethodSigHelper(Module mod, CallingConvention unmanagedCallConv, Type returnType)
		{
			if (returnType == null)
			{
				returnType = typeof(void);
			}
			int num;
			if (unmanagedCallConv == CallingConvention.Cdecl)
			{
				num = 1;
			}
			else if (unmanagedCallConv == CallingConvention.StdCall || unmanagedCallConv == CallingConvention.Winapi)
			{
				num = 2;
			}
			else if (unmanagedCallConv == CallingConvention.ThisCall)
			{
				num = 3;
			}
			else
			{
				if (unmanagedCallConv != CallingConvention.FastCall)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_UnknownUnmanagedCallConv"), "unmanagedCallConv");
				}
				num = 4;
			}
			return new SignatureHelper(mod, num, returnType, null, null);
		}

		// Token: 0x06004BDE RID: 19422 RVA: 0x0010A701 File Offset: 0x00109701
		public static SignatureHelper GetLocalVarSigHelper()
		{
			return SignatureHelper.GetLocalVarSigHelper(null);
		}

		// Token: 0x06004BDF RID: 19423 RVA: 0x0010A709 File Offset: 0x00109709
		public static SignatureHelper GetMethodSigHelper(CallingConventions callingConvention, Type returnType)
		{
			return SignatureHelper.GetMethodSigHelper(null, callingConvention, returnType);
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x0010A713 File Offset: 0x00109713
		public static SignatureHelper GetMethodSigHelper(CallingConvention unmanagedCallingConvention, Type returnType)
		{
			return SignatureHelper.GetMethodSigHelper(null, unmanagedCallingConvention, returnType);
		}

		// Token: 0x06004BE1 RID: 19425 RVA: 0x0010A71D File Offset: 0x0010971D
		public static SignatureHelper GetLocalVarSigHelper(Module mod)
		{
			return new SignatureHelper(mod, 7);
		}

		// Token: 0x06004BE2 RID: 19426 RVA: 0x0010A726 File Offset: 0x00109726
		public static SignatureHelper GetFieldSigHelper(Module mod)
		{
			return new SignatureHelper(mod, 6);
		}

		// Token: 0x06004BE3 RID: 19427 RVA: 0x0010A72F File Offset: 0x0010972F
		public static SignatureHelper GetPropertySigHelper(Module mod, Type returnType, Type[] parameterTypes)
		{
			return SignatureHelper.GetPropertySigHelper(mod, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x06004BE4 RID: 19428 RVA: 0x0010A73D File Offset: 0x0010973D
		public static SignatureHelper GetPropertySigHelper(Module mod, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			return SignatureHelper.GetPropertySigHelper(mod, (CallingConventions)0, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
		}

		// Token: 0x06004BE5 RID: 19429 RVA: 0x0010A750 File Offset: 0x00109750
		public static SignatureHelper GetPropertySigHelper(Module mod, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			if (returnType == null)
			{
				returnType = typeof(void);
			}
			int num = 8;
			if ((callingConvention & CallingConventions.HasThis) == CallingConventions.HasThis)
			{
				num |= 32;
			}
			SignatureHelper signatureHelper = new SignatureHelper(mod, num, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers);
			signatureHelper.AddArguments(parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
			return signatureHelper;
		}

		// Token: 0x06004BE6 RID: 19430 RVA: 0x0010A794 File Offset: 0x00109794
		internal static SignatureHelper GetTypeSigToken(Module mod, Type type)
		{
			if (mod == null)
			{
				throw new ArgumentNullException("module");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return new SignatureHelper(mod, type);
		}

		// Token: 0x06004BE7 RID: 19431 RVA: 0x0010A7B9 File Offset: 0x001097B9
		private SignatureHelper(Module mod, int callingConvention)
		{
			this.Init(mod, callingConvention);
		}

		// Token: 0x06004BE8 RID: 19432 RVA: 0x0010A7C9 File Offset: 0x001097C9
		private SignatureHelper(Module mod, int callingConvention, int cGenericParameters, Type returnType, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers)
		{
			this.Init(mod, callingConvention, cGenericParameters);
			if (callingConvention == 6)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadFieldSig"));
			}
			this.AddOneArgTypeHelper(returnType, requiredCustomModifiers, optionalCustomModifiers);
		}

		// Token: 0x06004BE9 RID: 19433 RVA: 0x0010A7FA File Offset: 0x001097FA
		private SignatureHelper(Module mod, int callingConvention, Type returnType, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers)
			: this(mod, callingConvention, 0, returnType, requiredCustomModifiers, optionalCustomModifiers)
		{
		}

		// Token: 0x06004BEA RID: 19434 RVA: 0x0010A80A File Offset: 0x0010980A
		private SignatureHelper(Module mod, Type type)
		{
			this.Init(mod);
			this.AddOneArgTypeHelper(type);
		}

		// Token: 0x06004BEB RID: 19435 RVA: 0x0010A820 File Offset: 0x00109820
		private void Init(Module mod)
		{
			this.m_signature = new byte[32];
			this.m_currSig = 0;
			this.m_module = mod as ModuleBuilder;
			this.m_argCount = 0;
			this.m_sigDone = false;
			this.m_sizeLoc = -1;
			if (this.m_module == null && mod != null)
			{
				throw new ArgumentException(Environment.GetResourceString("NotSupported_MustBeModuleBuilder"));
			}
		}

		// Token: 0x06004BEC RID: 19436 RVA: 0x0010A87D File Offset: 0x0010987D
		private void Init(Module mod, int callingConvention)
		{
			this.Init(mod, callingConvention, 0);
		}

		// Token: 0x06004BED RID: 19437 RVA: 0x0010A888 File Offset: 0x00109888
		private void Init(Module mod, int callingConvention, int cGenericParam)
		{
			this.Init(mod);
			this.AddData(callingConvention);
			if (callingConvention == 6 || callingConvention == 10)
			{
				this.m_sizeLoc = -1;
				return;
			}
			if (cGenericParam > 0)
			{
				this.AddData(cGenericParam);
			}
			this.m_sizeLoc = this.m_currSig++;
		}

		// Token: 0x06004BEE RID: 19438 RVA: 0x0010A8D6 File Offset: 0x001098D6
		private void AddOneArgTypeHelper(Type argument, bool pinned)
		{
			if (pinned)
			{
				this.AddElementType(69);
			}
			this.AddOneArgTypeHelper(argument);
		}

		// Token: 0x06004BEF RID: 19439 RVA: 0x0010A8EC File Offset: 0x001098EC
		private void AddOneArgTypeHelper(Type clsArgument, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers)
		{
			if (optionalCustomModifiers != null)
			{
				for (int i = 0; i < optionalCustomModifiers.Length; i++)
				{
					this.AddElementType(32);
					this.AddToken(this.m_module.GetTypeToken(optionalCustomModifiers[i]).Token);
				}
			}
			if (requiredCustomModifiers != null)
			{
				for (int j = 0; j < requiredCustomModifiers.Length; j++)
				{
					this.AddElementType(31);
					this.AddToken(this.m_module.GetTypeToken(requiredCustomModifiers[j]).Token);
				}
			}
			this.AddOneArgTypeHelper(clsArgument);
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x0010A96A File Offset: 0x0010996A
		private void AddOneArgTypeHelper(Type clsArgument)
		{
			this.AddOneArgTypeHelperWorker(clsArgument, false);
		}

		// Token: 0x06004BF1 RID: 19441 RVA: 0x0010A974 File Offset: 0x00109974
		private void AddOneArgTypeHelperWorker(Type clsArgument, bool lastWasGenericInst)
		{
			if (clsArgument.IsGenericParameter)
			{
				if (clsArgument.DeclaringMethod != null)
				{
					this.AddData(30);
				}
				else
				{
					this.AddData(19);
				}
				this.AddData(clsArgument.GenericParameterPosition);
				return;
			}
			if (clsArgument.IsGenericType && (!clsArgument.IsGenericTypeDefinition || !lastWasGenericInst))
			{
				this.AddElementType(21);
				this.AddOneArgTypeHelperWorker(clsArgument.GetGenericTypeDefinition(), true);
				Type[] genericArguments = clsArgument.GetGenericArguments();
				this.AddData(genericArguments.Length);
				foreach (Type type in genericArguments)
				{
					this.AddOneArgTypeHelper(type);
				}
				return;
			}
			if (clsArgument is TypeBuilder)
			{
				TypeBuilder typeBuilder = (TypeBuilder)clsArgument;
				TypeToken typeToken;
				if (typeBuilder.Module.Equals(this.m_module))
				{
					typeToken = typeBuilder.TypeToken;
				}
				else
				{
					typeToken = this.m_module.GetTypeToken(clsArgument);
				}
				if (clsArgument.IsValueType)
				{
					this.InternalAddTypeToken(typeToken, 17);
					return;
				}
				this.InternalAddTypeToken(typeToken, 18);
				return;
			}
			else if (clsArgument is EnumBuilder)
			{
				TypeBuilder typeBuilder2 = ((EnumBuilder)clsArgument).m_typeBuilder;
				TypeToken typeToken2;
				if (typeBuilder2.Module.Equals(this.m_module))
				{
					typeToken2 = typeBuilder2.TypeToken;
				}
				else
				{
					typeToken2 = this.m_module.GetTypeToken(clsArgument);
				}
				if (clsArgument.IsValueType)
				{
					this.InternalAddTypeToken(typeToken2, 17);
					return;
				}
				this.InternalAddTypeToken(typeToken2, 18);
				return;
			}
			else
			{
				if (clsArgument.IsByRef)
				{
					this.AddElementType(16);
					clsArgument = clsArgument.GetElementType();
					this.AddOneArgTypeHelper(clsArgument);
					return;
				}
				if (clsArgument.IsPointer)
				{
					this.AddElementType(15);
					this.AddOneArgTypeHelper(clsArgument.GetElementType());
					return;
				}
				if (clsArgument.IsArray)
				{
					if (clsArgument.IsSzArray)
					{
						this.AddElementType(29);
						this.AddOneArgTypeHelper(clsArgument.GetElementType());
						return;
					}
					this.AddElementType(20);
					this.AddOneArgTypeHelper(clsArgument.GetElementType());
					this.AddData(clsArgument.GetArrayRank());
					this.AddData(0);
					this.AddData(0);
					return;
				}
				else
				{
					RuntimeType runtimeType = clsArgument as RuntimeType;
					int num = ((runtimeType != null) ? SignatureHelper.GetCorElementTypeFromClass(runtimeType) : 34);
					if (SignatureHelper.IsSimpleType(num))
					{
						this.AddElementType(num);
						return;
					}
					if (clsArgument == typeof(object))
					{
						this.AddElementType(28);
						return;
					}
					if (clsArgument == typeof(string))
					{
						this.AddElementType(14);
						return;
					}
					if (this.m_module == null)
					{
						this.InternalAddRuntimeType(runtimeType);
						return;
					}
					if (clsArgument.IsValueType)
					{
						this.InternalAddTypeToken(this.m_module.GetTypeToken(clsArgument), 17);
						return;
					}
					this.InternalAddTypeToken(this.m_module.GetTypeToken(clsArgument), 18);
					return;
				}
			}
		}

		// Token: 0x06004BF2 RID: 19442 RVA: 0x0010ABEC File Offset: 0x00109BEC
		private void AddData(int data)
		{
			if (this.m_currSig + 4 >= this.m_signature.Length)
			{
				this.m_signature = this.ExpandArray(this.m_signature);
			}
			if (data <= 127)
			{
				this.m_signature[this.m_currSig++] = (byte)(data & 255);
				return;
			}
			if (data <= 16383)
			{
				this.m_signature[this.m_currSig++] = (byte)((data >> 8) | 128);
				this.m_signature[this.m_currSig++] = (byte)(data & 255);
				return;
			}
			if (data <= 536870911)
			{
				this.m_signature[this.m_currSig++] = (byte)((data >> 24) | 192);
				this.m_signature[this.m_currSig++] = (byte)((data >> 16) & 255);
				this.m_signature[this.m_currSig++] = (byte)((data >> 8) & 255);
				this.m_signature[this.m_currSig++] = (byte)(data & 255);
				return;
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_LargeInteger"));
		}

		// Token: 0x06004BF3 RID: 19443 RVA: 0x0010AD38 File Offset: 0x00109D38
		private void AddData(uint data)
		{
			if (this.m_currSig + 4 >= this.m_signature.Length)
			{
				this.m_signature = this.ExpandArray(this.m_signature);
			}
			this.m_signature[this.m_currSig++] = (byte)(data & 255U);
			this.m_signature[this.m_currSig++] = (byte)((data >> 8) & 255U);
			this.m_signature[this.m_currSig++] = (byte)((data >> 16) & 255U);
			this.m_signature[this.m_currSig++] = (byte)((data >> 24) & 255U);
		}

		// Token: 0x06004BF4 RID: 19444 RVA: 0x0010ADF4 File Offset: 0x00109DF4
		private void AddData(ulong data)
		{
			if (this.m_currSig + 8 >= this.m_signature.Length)
			{
				this.m_signature = this.ExpandArray(this.m_signature);
			}
			this.m_signature[this.m_currSig++] = (byte)(data & 255UL);
			this.m_signature[this.m_currSig++] = (byte)((data >> 8) & 255UL);
			this.m_signature[this.m_currSig++] = (byte)((data >> 16) & 255UL);
			this.m_signature[this.m_currSig++] = (byte)((data >> 24) & 255UL);
			this.m_signature[this.m_currSig++] = (byte)((data >> 32) & 255UL);
			this.m_signature[this.m_currSig++] = (byte)((data >> 40) & 255UL);
			this.m_signature[this.m_currSig++] = (byte)((data >> 48) & 255UL);
			this.m_signature[this.m_currSig++] = (byte)((data >> 56) & 255UL);
		}

		// Token: 0x06004BF5 RID: 19445 RVA: 0x0010AF4C File Offset: 0x00109F4C
		private void AddElementType(int cvt)
		{
			if (this.m_currSig + 1 >= this.m_signature.Length)
			{
				this.m_signature = this.ExpandArray(this.m_signature);
			}
			this.m_signature[this.m_currSig++] = (byte)cvt;
		}

		// Token: 0x06004BF6 RID: 19446 RVA: 0x0010AF98 File Offset: 0x00109F98
		private void AddToken(int token)
		{
			int num = token & 16777215;
			int num2 = token & -16777216;
			if (num > 67108863)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_LargeInteger"));
			}
			num <<= 2;
			if (num2 == 16777216)
			{
				num |= 1;
			}
			else if (num2 == 553648128)
			{
				num |= 2;
			}
			this.AddData(num);
		}

		// Token: 0x06004BF7 RID: 19447 RVA: 0x0010AFF2 File Offset: 0x00109FF2
		private void InternalAddTypeToken(TypeToken clsToken, int CorType)
		{
			this.AddElementType(CorType);
			this.AddToken(clsToken.Token);
		}

		// Token: 0x06004BF8 RID: 19448 RVA: 0x0010B008 File Offset: 0x0010A008
		private unsafe void InternalAddRuntimeType(Type type)
		{
			this.AddElementType(33);
			void* ptr = (void*)type.GetTypeHandleInternal().Value;
			if (sizeof(void*) == 4)
			{
				this.AddData(ptr);
				return;
			}
			this.AddData(ptr);
		}

		// Token: 0x06004BF9 RID: 19449 RVA: 0x0010B04B File Offset: 0x0010A04B
		private byte[] ExpandArray(byte[] inArray)
		{
			return this.ExpandArray(inArray, inArray.Length * 2);
		}

		// Token: 0x06004BFA RID: 19450 RVA: 0x0010B05C File Offset: 0x0010A05C
		private byte[] ExpandArray(byte[] inArray, int requiredLength)
		{
			if (requiredLength < inArray.Length)
			{
				requiredLength = inArray.Length * 2;
			}
			byte[] array = new byte[requiredLength];
			Array.Copy(inArray, array, inArray.Length);
			return array;
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x0010B088 File Offset: 0x0010A088
		private void IncrementArgCounts()
		{
			if (this.m_sizeLoc == -1)
			{
				return;
			}
			this.m_argCount++;
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x0010B0A4 File Offset: 0x0010A0A4
		private void SetNumberOfSignatureElements(bool forceCopy)
		{
			int currSig = this.m_currSig;
			if (this.m_sizeLoc == -1)
			{
				return;
			}
			if (this.m_argCount < 128 && !forceCopy)
			{
				this.m_signature[this.m_sizeLoc] = (byte)this.m_argCount;
				return;
			}
			int num;
			if (this.m_argCount < 127)
			{
				num = 1;
			}
			else if (this.m_argCount < 16383)
			{
				num = 2;
			}
			else
			{
				num = 4;
			}
			byte[] array = new byte[this.m_currSig + num - 1];
			array[0] = this.m_signature[0];
			Array.Copy(this.m_signature, this.m_sizeLoc + 1, array, this.m_sizeLoc + num, currSig - (this.m_sizeLoc + 1));
			this.m_signature = array;
			this.m_currSig = this.m_sizeLoc;
			this.AddData(this.m_argCount);
			this.m_currSig = currSig + (num - 1);
		}

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x06004BFD RID: 19453 RVA: 0x0010B173 File Offset: 0x0010A173
		internal int ArgumentCount
		{
			get
			{
				return this.m_argCount;
			}
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x0010B17B File Offset: 0x0010A17B
		internal static bool IsSimpleType(int type)
		{
			return type <= 14 || (type == 22 || type == 24 || type == 25 || type == 28);
		}

		// Token: 0x06004BFF RID: 19455 RVA: 0x0010B19B File Offset: 0x0010A19B
		internal byte[] InternalGetSignature(out int length)
		{
			if (!this.m_sigDone)
			{
				this.m_sigDone = true;
				this.SetNumberOfSignatureElements(false);
			}
			length = this.m_currSig;
			return this.m_signature;
		}

		// Token: 0x06004C00 RID: 19456 RVA: 0x0010B1C4 File Offset: 0x0010A1C4
		internal byte[] InternalGetSignatureArray()
		{
			int argCount = this.m_argCount;
			int currSig = this.m_currSig;
			int num = currSig;
			if (argCount < 127)
			{
				num++;
			}
			else if (argCount < 16383)
			{
				num += 2;
			}
			else
			{
				num += 4;
			}
			byte[] array = new byte[num];
			int num2 = 0;
			array[num2++] = this.m_signature[0];
			if (argCount <= 127)
			{
				array[num2++] = (byte)(argCount & 255);
			}
			else if (argCount <= 16383)
			{
				array[num2++] = (byte)((argCount >> 8) | 128);
				array[num2++] = (byte)(argCount & 255);
			}
			else
			{
				if (argCount > 536870911)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_LargeInteger"));
				}
				array[num2++] = (byte)((argCount >> 24) | 192);
				array[num2++] = (byte)((argCount >> 16) & 255);
				array[num2++] = (byte)((argCount >> 8) & 255);
				array[num2++] = (byte)(argCount & 255);
			}
			Array.Copy(this.m_signature, 2, array, num2, currSig - 2);
			array[num - 1] = 0;
			return array;
		}

		// Token: 0x06004C01 RID: 19457 RVA: 0x0010B2E1 File Offset: 0x0010A2E1
		public void AddArgument(Type clsArgument)
		{
			this.AddArgument(clsArgument, null, null);
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x0010B2EC File Offset: 0x0010A2EC
		public void AddArgument(Type argument, bool pinned)
		{
			if (argument == null)
			{
				throw new ArgumentNullException("argument");
			}
			this.IncrementArgCounts();
			this.AddOneArgTypeHelper(argument, pinned);
		}

		// Token: 0x06004C03 RID: 19459 RVA: 0x0010B30C File Offset: 0x0010A30C
		public void AddArguments(Type[] arguments, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
		{
			if (requiredCustomModifiers != null && (arguments == null || requiredCustomModifiers.Length != arguments.Length))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MismatchedArrays", new object[] { "requiredCustomModifiers", "arguments" }));
			}
			if (optionalCustomModifiers != null && (arguments == null || optionalCustomModifiers.Length != arguments.Length))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MismatchedArrays", new object[] { "optionalCustomModifiers", "arguments" }));
			}
			if (arguments != null)
			{
				for (int i = 0; i < arguments.Length; i++)
				{
					this.AddArgument(arguments[i], (requiredCustomModifiers == null) ? null : requiredCustomModifiers[i], (optionalCustomModifiers == null) ? null : optionalCustomModifiers[i]);
				}
			}
		}

		// Token: 0x06004C04 RID: 19460 RVA: 0x0010B3B4 File Offset: 0x0010A3B4
		public void AddArgument(Type argument, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers)
		{
			if (this.m_sigDone)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_SigIsFinalized"));
			}
			if (argument == null)
			{
				throw new ArgumentNullException("argument");
			}
			if (requiredCustomModifiers != null)
			{
				foreach (Type type in requiredCustomModifiers)
				{
					if (type == null)
					{
						throw new ArgumentNullException("requiredCustomModifiers");
					}
					if (type.HasElementType)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_ArraysInvalid"), "requiredCustomModifiers");
					}
					if (type.ContainsGenericParameters)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_GenericsInvalid"), "requiredCustomModifiers");
					}
				}
			}
			if (optionalCustomModifiers != null)
			{
				foreach (Type type2 in optionalCustomModifiers)
				{
					if (type2 == null)
					{
						throw new ArgumentNullException("optionalCustomModifiers");
					}
					if (type2.HasElementType)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_ArraysInvalid"), "optionalCustomModifiers");
					}
					if (type2.ContainsGenericParameters)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_GenericsInvalid"), "optionalCustomModifiers");
					}
				}
			}
			this.IncrementArgCounts();
			this.AddOneArgTypeHelper(argument, requiredCustomModifiers, optionalCustomModifiers);
		}

		// Token: 0x06004C05 RID: 19461 RVA: 0x0010B4B0 File Offset: 0x0010A4B0
		public void AddSentinel()
		{
			this.AddElementType(65);
		}

		// Token: 0x06004C06 RID: 19462 RVA: 0x0010B4BC File Offset: 0x0010A4BC
		public override bool Equals(object obj)
		{
			if (!(obj is SignatureHelper))
			{
				return false;
			}
			SignatureHelper signatureHelper = (SignatureHelper)obj;
			if (!signatureHelper.m_module.Equals(this.m_module) || signatureHelper.m_currSig != this.m_currSig || signatureHelper.m_sizeLoc != this.m_sizeLoc || signatureHelper.m_sigDone != this.m_sigDone)
			{
				return false;
			}
			for (int i = 0; i < this.m_currSig; i++)
			{
				if (this.m_signature[i] != signatureHelper.m_signature[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004C07 RID: 19463 RVA: 0x0010B540 File Offset: 0x0010A540
		public override int GetHashCode()
		{
			int num = this.m_module.GetHashCode() + this.m_currSig + this.m_sizeLoc;
			if (this.m_sigDone)
			{
				num++;
			}
			for (int i = 0; i < this.m_currSig; i++)
			{
				num += this.m_signature[i].GetHashCode();
			}
			return num;
		}

		// Token: 0x06004C08 RID: 19464 RVA: 0x0010B599 File Offset: 0x0010A599
		public byte[] GetSignature()
		{
			return this.GetSignature(false);
		}

		// Token: 0x06004C09 RID: 19465 RVA: 0x0010B5A4 File Offset: 0x0010A5A4
		internal byte[] GetSignature(bool appendEndOfSig)
		{
			if (!this.m_sigDone)
			{
				if (appendEndOfSig)
				{
					this.AddElementType(0);
				}
				this.SetNumberOfSignatureElements(true);
				this.m_sigDone = true;
			}
			if (this.m_signature.Length > this.m_currSig)
			{
				byte[] array = new byte[this.m_currSig];
				Array.Copy(this.m_signature, array, this.m_currSig);
				this.m_signature = array;
			}
			return this.m_signature;
		}

		// Token: 0x06004C0A RID: 19466 RVA: 0x0010B60C File Offset: 0x0010A60C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Length: " + this.m_currSig + Environment.NewLine);
			if (this.m_sizeLoc != -1)
			{
				stringBuilder.Append("Arguments: " + this.m_signature[this.m_sizeLoc] + Environment.NewLine);
			}
			else
			{
				stringBuilder.Append("Field Signature" + Environment.NewLine);
			}
			stringBuilder.Append("Signature: " + Environment.NewLine);
			for (int i = 0; i <= this.m_currSig; i++)
			{
				stringBuilder.Append(this.m_signature[i] + "  ");
			}
			stringBuilder.Append(Environment.NewLine);
			return stringBuilder.ToString();
		}

		// Token: 0x06004C0B RID: 19467
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetCorElementTypeFromClass(RuntimeType cls);

		// Token: 0x06004C0C RID: 19468 RVA: 0x0010B6E0 File Offset: 0x0010A6E0
		void _SignatureHelper.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C0D RID: 19469 RVA: 0x0010B6E7 File Offset: 0x0010A6E7
		void _SignatureHelper.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C0E RID: 19470 RVA: 0x0010B6EE File Offset: 0x0010A6EE
		void _SignatureHelper.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C0F RID: 19471 RVA: 0x0010B6F5 File Offset: 0x0010A6F5
		void _SignatureHelper.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400278C RID: 10124
		internal const int mdtTypeRef = 16777216;

		// Token: 0x0400278D RID: 10125
		internal const int mdtTypeDef = 33554432;

		// Token: 0x0400278E RID: 10126
		internal const int mdtTypeSpec = 553648128;

		// Token: 0x0400278F RID: 10127
		internal const byte ELEMENT_TYPE_END = 0;

		// Token: 0x04002790 RID: 10128
		internal const byte ELEMENT_TYPE_VOID = 1;

		// Token: 0x04002791 RID: 10129
		internal const byte ELEMENT_TYPE_BOOLEAN = 2;

		// Token: 0x04002792 RID: 10130
		internal const byte ELEMENT_TYPE_CHAR = 3;

		// Token: 0x04002793 RID: 10131
		internal const byte ELEMENT_TYPE_I1 = 4;

		// Token: 0x04002794 RID: 10132
		internal const byte ELEMENT_TYPE_U1 = 5;

		// Token: 0x04002795 RID: 10133
		internal const byte ELEMENT_TYPE_I2 = 6;

		// Token: 0x04002796 RID: 10134
		internal const byte ELEMENT_TYPE_U2 = 7;

		// Token: 0x04002797 RID: 10135
		internal const byte ELEMENT_TYPE_I4 = 8;

		// Token: 0x04002798 RID: 10136
		internal const byte ELEMENT_TYPE_U4 = 9;

		// Token: 0x04002799 RID: 10137
		internal const byte ELEMENT_TYPE_I8 = 10;

		// Token: 0x0400279A RID: 10138
		internal const byte ELEMENT_TYPE_U8 = 11;

		// Token: 0x0400279B RID: 10139
		internal const byte ELEMENT_TYPE_R4 = 12;

		// Token: 0x0400279C RID: 10140
		internal const byte ELEMENT_TYPE_R8 = 13;

		// Token: 0x0400279D RID: 10141
		internal const byte ELEMENT_TYPE_STRING = 14;

		// Token: 0x0400279E RID: 10142
		internal const byte ELEMENT_TYPE_PTR = 15;

		// Token: 0x0400279F RID: 10143
		internal const byte ELEMENT_TYPE_BYREF = 16;

		// Token: 0x040027A0 RID: 10144
		internal const byte ELEMENT_TYPE_VALUETYPE = 17;

		// Token: 0x040027A1 RID: 10145
		internal const byte ELEMENT_TYPE_CLASS = 18;

		// Token: 0x040027A2 RID: 10146
		internal const byte ELEMENT_TYPE_VAR = 19;

		// Token: 0x040027A3 RID: 10147
		internal const byte ELEMENT_TYPE_ARRAY = 20;

		// Token: 0x040027A4 RID: 10148
		internal const byte ELEMENT_TYPE_GENERICINST = 21;

		// Token: 0x040027A5 RID: 10149
		internal const byte ELEMENT_TYPE_TYPEDBYREF = 22;

		// Token: 0x040027A6 RID: 10150
		internal const byte ELEMENT_TYPE_I = 24;

		// Token: 0x040027A7 RID: 10151
		internal const byte ELEMENT_TYPE_U = 25;

		// Token: 0x040027A8 RID: 10152
		internal const byte ELEMENT_TYPE_FNPTR = 27;

		// Token: 0x040027A9 RID: 10153
		internal const byte ELEMENT_TYPE_OBJECT = 28;

		// Token: 0x040027AA RID: 10154
		internal const byte ELEMENT_TYPE_SZARRAY = 29;

		// Token: 0x040027AB RID: 10155
		internal const byte ELEMENT_TYPE_MVAR = 30;

		// Token: 0x040027AC RID: 10156
		internal const byte ELEMENT_TYPE_CMOD_REQD = 31;

		// Token: 0x040027AD RID: 10157
		internal const byte ELEMENT_TYPE_CMOD_OPT = 32;

		// Token: 0x040027AE RID: 10158
		internal const byte ELEMENT_TYPE_INTERNAL = 33;

		// Token: 0x040027AF RID: 10159
		internal const byte ELEMENT_TYPE_MAX = 34;

		// Token: 0x040027B0 RID: 10160
		internal const byte ELEMENT_TYPE_SENTINEL = 65;

		// Token: 0x040027B1 RID: 10161
		internal const byte ELEMENT_TYPE_PINNED = 69;

		// Token: 0x040027B2 RID: 10162
		internal const int IMAGE_CEE_UNMANAGED_CALLCONV_C = 1;

		// Token: 0x040027B3 RID: 10163
		internal const int IMAGE_CEE_UNMANAGED_CALLCONV_STDCALL = 2;

		// Token: 0x040027B4 RID: 10164
		internal const int IMAGE_CEE_UNMANAGED_CALLCONV_THISCALL = 3;

		// Token: 0x040027B5 RID: 10165
		internal const int IMAGE_CEE_UNMANAGED_CALLCONV_FASTCALL = 4;

		// Token: 0x040027B6 RID: 10166
		internal const int IMAGE_CEE_CS_CALLCONV_DEFAULT = 0;

		// Token: 0x040027B7 RID: 10167
		internal const int IMAGE_CEE_CS_CALLCONV_VARARG = 5;

		// Token: 0x040027B8 RID: 10168
		internal const int IMAGE_CEE_CS_CALLCONV_FIELD = 6;

		// Token: 0x040027B9 RID: 10169
		internal const int IMAGE_CEE_CS_CALLCONV_LOCAL_SIG = 7;

		// Token: 0x040027BA RID: 10170
		internal const int IMAGE_CEE_CS_CALLCONV_PROPERTY = 8;

		// Token: 0x040027BB RID: 10171
		internal const int IMAGE_CEE_CS_CALLCONV_UNMGD = 9;

		// Token: 0x040027BC RID: 10172
		internal const int IMAGE_CEE_CS_CALLCONV_GENERICINST = 10;

		// Token: 0x040027BD RID: 10173
		internal const int IMAGE_CEE_CS_CALLCONV_MAX = 11;

		// Token: 0x040027BE RID: 10174
		internal const int IMAGE_CEE_CS_CALLCONV_MASK = 15;

		// Token: 0x040027BF RID: 10175
		internal const int IMAGE_CEE_CS_CALLCONV_GENERIC = 16;

		// Token: 0x040027C0 RID: 10176
		internal const int IMAGE_CEE_CS_CALLCONV_HASTHIS = 32;

		// Token: 0x040027C1 RID: 10177
		internal const int IMAGE_CEE_CS_CALLCONV_RETPARAM = 64;

		// Token: 0x040027C2 RID: 10178
		internal const int NO_SIZE_IN_SIG = -1;

		// Token: 0x040027C3 RID: 10179
		private byte[] m_signature;

		// Token: 0x040027C4 RID: 10180
		private int m_currSig;

		// Token: 0x040027C5 RID: 10181
		private int m_sizeLoc;

		// Token: 0x040027C6 RID: 10182
		private ModuleBuilder m_module;

		// Token: 0x040027C7 RID: 10183
		private bool m_sigDone;

		// Token: 0x040027C8 RID: 10184
		private int m_argCount;
	}
}
