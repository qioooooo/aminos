using System;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000801 RID: 2049
	internal class DynamicILGenerator : ILGenerator
	{
		// Token: 0x0600492B RID: 18731 RVA: 0x000FF51C File Offset: 0x000FE51C
		internal DynamicILGenerator(DynamicMethod method, byte[] methodSignature, int size)
			: base(method, size)
		{
			this.m_scope = new DynamicScope();
			this.m_methodSigToken = this.m_scope.GetTokenFor(methodSignature);
		}

		// Token: 0x0600492C RID: 18732 RVA: 0x000FF543 File Offset: 0x000FE543
		internal unsafe RuntimeMethodHandle GetCallableMethod(void* module)
		{
			return new RuntimeMethodHandle(ModuleHandle.GetDynamicMethod(module, this.m_methodBuilder.Name, (byte[])this.m_scope[this.m_methodSigToken], new DynamicResolver(this)));
		}

		// Token: 0x0600492D RID: 18733 RVA: 0x000FF578 File Offset: 0x000FE578
		public override LocalBuilder DeclareLocal(Type localType, bool pinned)
		{
			if (localType == null)
			{
				throw new ArgumentNullException("localType");
			}
			if (localType.GetType() != typeof(RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"));
			}
			LocalBuilder localBuilder = new LocalBuilder(this.m_localCount, localType, this.m_methodBuilder);
			this.m_localSignature.AddArgument(localType, pinned);
			this.m_localCount++;
			return localBuilder;
		}

		// Token: 0x0600492E RID: 18734 RVA: 0x000FF5E4 File Offset: 0x000FE5E4
		public override void Emit(OpCode opcode, MethodInfo meth)
		{
			if (meth == null)
			{
				throw new ArgumentNullException("meth");
			}
			int num = 0;
			DynamicMethod dynamicMethod = meth as DynamicMethod;
			int num2;
			if (dynamicMethod == null)
			{
				if (!(meth is RuntimeMethodInfo))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeMethodInfo"), "meth");
				}
				if (meth.DeclaringType != null && (meth.DeclaringType.IsGenericType || meth.DeclaringType.IsArray))
				{
					num2 = this.m_scope.GetTokenFor(meth.MethodHandle, meth.DeclaringType.TypeHandle);
				}
				else
				{
					num2 = this.m_scope.GetTokenFor(meth.MethodHandle);
				}
			}
			else
			{
				if (opcode.Equals(OpCodes.Ldtoken) || opcode.Equals(OpCodes.Ldftn) || opcode.Equals(OpCodes.Ldvirtftn))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOpCodeOnDynamicMethod"));
				}
				num2 = this.m_scope.GetTokenFor(dynamicMethod);
			}
			this.EnsureCapacity(7);
			base.InternalEmit(opcode);
			if (opcode.m_push == StackBehaviour.Varpush && meth.ReturnType != typeof(void))
			{
				num++;
			}
			if (opcode.m_pop == StackBehaviour.Varpop)
			{
				num -= meth.GetParametersNoCopy().Length;
			}
			if (!meth.IsStatic && !opcode.Equals(OpCodes.Newobj) && !opcode.Equals(OpCodes.Ldtoken) && !opcode.Equals(OpCodes.Ldftn))
			{
				num--;
			}
			base.UpdateStackSize(opcode, num);
			this.m_length = this.PutInteger4(num2, this.m_length, this.m_ILStream);
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x000FF764 File Offset: 0x000FE764
		[ComVisible(true)]
		public override void Emit(OpCode opcode, ConstructorInfo con)
		{
			if (con == null || !(con is RuntimeConstructorInfo))
			{
				throw new ArgumentNullException("con");
			}
			if (con.DeclaringType != null && (con.DeclaringType.IsGenericType || con.DeclaringType.IsArray))
			{
				this.Emit(opcode, con.MethodHandle, con.DeclaringType.TypeHandle);
				return;
			}
			this.Emit(opcode, con.MethodHandle);
		}

		// Token: 0x06004930 RID: 18736 RVA: 0x000FF7D0 File Offset: 0x000FE7D0
		public void Emit(OpCode opcode, RuntimeMethodHandle meth)
		{
			if (meth.IsNullHandle())
			{
				throw new ArgumentNullException("meth");
			}
			int tokenFor = this.m_scope.GetTokenFor(meth);
			this.EnsureCapacity(7);
			base.InternalEmit(opcode);
			base.UpdateStackSize(opcode, 1);
			this.m_length = this.PutInteger4(tokenFor, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004931 RID: 18737 RVA: 0x000FF830 File Offset: 0x000FE830
		public void Emit(OpCode opcode, RuntimeMethodHandle meth, RuntimeTypeHandle typeContext)
		{
			if (meth.IsNullHandle())
			{
				throw new ArgumentNullException("meth");
			}
			if (typeContext.IsNullHandle())
			{
				throw new ArgumentNullException("typeContext");
			}
			int tokenFor = this.m_scope.GetTokenFor(meth, typeContext);
			this.EnsureCapacity(7);
			base.InternalEmit(opcode);
			base.UpdateStackSize(opcode, 1);
			this.m_length = this.PutInteger4(tokenFor, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004932 RID: 18738 RVA: 0x000FF8A2 File Offset: 0x000FE8A2
		public override void Emit(OpCode opcode, Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.Emit(opcode, type.TypeHandle);
		}

		// Token: 0x06004933 RID: 18739 RVA: 0x000FF8C0 File Offset: 0x000FE8C0
		public void Emit(OpCode opcode, RuntimeTypeHandle typeHandle)
		{
			if (typeHandle.IsNullHandle())
			{
				throw new ArgumentNullException("typeHandle");
			}
			int tokenFor = this.m_scope.GetTokenFor(typeHandle);
			this.EnsureCapacity(7);
			base.InternalEmit(opcode);
			this.m_length = this.PutInteger4(tokenFor, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x000FF918 File Offset: 0x000FE918
		public override void Emit(OpCode opcode, FieldInfo field)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			if (!(field is RuntimeFieldInfo))
			{
				throw new ArgumentNullException("field");
			}
			if (field.DeclaringType == null)
			{
				this.Emit(opcode, field.FieldHandle);
				return;
			}
			this.Emit(opcode, field.FieldHandle, field.DeclaringType.GetTypeHandleInternal());
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x000FF974 File Offset: 0x000FE974
		public void Emit(OpCode opcode, RuntimeFieldHandle fieldHandle)
		{
			if (fieldHandle.IsNullHandle())
			{
				throw new ArgumentNullException("fieldHandle");
			}
			int tokenFor = this.m_scope.GetTokenFor(fieldHandle);
			this.EnsureCapacity(7);
			base.InternalEmit(opcode);
			this.m_length = this.PutInteger4(tokenFor, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x000FF9CC File Offset: 0x000FE9CC
		public void Emit(OpCode opcode, RuntimeFieldHandle fieldHandle, RuntimeTypeHandle typeContext)
		{
			if (fieldHandle.IsNullHandle())
			{
				throw new ArgumentNullException("fieldHandle");
			}
			int tokenFor = this.m_scope.GetTokenFor(fieldHandle, typeContext);
			this.EnsureCapacity(7);
			base.InternalEmit(opcode);
			this.m_length = this.PutInteger4(tokenFor, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x000FFA24 File Offset: 0x000FEA24
		public override void Emit(OpCode opcode, string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			int num = this.AddStringLiteral(str);
			num |= 1879048192;
			this.EnsureCapacity(7);
			base.InternalEmit(opcode);
			this.m_length = this.PutInteger4(num, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004938 RID: 18744 RVA: 0x000FFA78 File Offset: 0x000FEA78
		public override void EmitCalli(OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			int num = 0;
			if (optionalParameterTypes != null && (callingConvention & CallingConventions.VarArgs) == (CallingConventions)0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAVarArgCallingConvention"));
			}
			SignatureHelper memberRefSignature = this.GetMemberRefSignature(callingConvention, returnType, parameterTypes, optionalParameterTypes);
			this.EnsureCapacity(7);
			this.Emit(OpCodes.Calli);
			if (returnType != typeof(void))
			{
				num++;
			}
			if (parameterTypes != null)
			{
				num -= parameterTypes.Length;
			}
			if (optionalParameterTypes != null)
			{
				num -= optionalParameterTypes.Length;
			}
			if ((callingConvention & CallingConventions.HasThis) == CallingConventions.HasThis)
			{
				num--;
			}
			num--;
			base.UpdateStackSize(opcode, num);
			int num2 = this.AddSignature(memberRefSignature.GetSignature(true));
			this.m_length = this.PutInteger4(num2, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004939 RID: 18745 RVA: 0x000FFB28 File Offset: 0x000FEB28
		public override void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			int num = 0;
			int num2 = 0;
			if (parameterTypes != null)
			{
				num2 = parameterTypes.Length;
			}
			SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(unmanagedCallConv, returnType);
			if (parameterTypes != null)
			{
				for (int i = 0; i < num2; i++)
				{
					methodSigHelper.AddArgument(parameterTypes[i]);
				}
			}
			if (returnType != typeof(void))
			{
				num++;
			}
			if (parameterTypes != null)
			{
				num -= num2;
			}
			num--;
			base.UpdateStackSize(opcode, num);
			this.EnsureCapacity(7);
			this.Emit(OpCodes.Calli);
			int num3 = this.AddSignature(methodSigHelper.GetSignature(true));
			this.m_length = this.PutInteger4(num3, this.m_length, this.m_ILStream);
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x000FFBC4 File Offset: 0x000FEBC4
		public override void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			int num = 0;
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			if (methodInfo.ContainsGenericParameters)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_GenericsInvalid"), "methodInfo");
			}
			if (methodInfo.DeclaringType != null && methodInfo.DeclaringType.ContainsGenericParameters)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_GenericsInvalid"), "methodInfo");
			}
			int memberRefToken = this.GetMemberRefToken(methodInfo, optionalParameterTypes);
			this.EnsureCapacity(7);
			base.InternalEmit(opcode);
			if (methodInfo.ReturnType != typeof(void))
			{
				num++;
			}
			num -= methodInfo.GetParameterTypes().Length;
			if (!(methodInfo is SymbolMethod) && !methodInfo.IsStatic && !opcode.Equals(OpCodes.Newobj))
			{
				num--;
			}
			if (optionalParameterTypes != null)
			{
				num -= optionalParameterTypes.Length;
			}
			base.UpdateStackSize(opcode, num);
			this.m_length = this.PutInteger4(memberRefToken, this.m_length, this.m_ILStream);
		}

		// Token: 0x0600493B RID: 18747 RVA: 0x000FFCAC File Offset: 0x000FECAC
		public override void Emit(OpCode opcode, SignatureHelper signature)
		{
			int num = 0;
			if (signature == null)
			{
				throw new ArgumentNullException("signature");
			}
			this.EnsureCapacity(7);
			base.InternalEmit(opcode);
			if (opcode.m_pop == StackBehaviour.Varpop)
			{
				num -= signature.ArgumentCount;
				num--;
				base.UpdateStackSize(opcode, num);
			}
			int num2 = this.AddSignature(signature.GetSignature(true));
			this.m_length = this.PutInteger4(num2, this.m_length, this.m_ILStream);
		}

		// Token: 0x0600493C RID: 18748 RVA: 0x000FFD1E File Offset: 0x000FED1E
		public override Label BeginExceptionBlock()
		{
			return base.BeginExceptionBlock();
		}

		// Token: 0x0600493D RID: 18749 RVA: 0x000FFD26 File Offset: 0x000FED26
		public override void EndExceptionBlock()
		{
			base.EndExceptionBlock();
		}

		// Token: 0x0600493E RID: 18750 RVA: 0x000FFD2E File Offset: 0x000FED2E
		public override void BeginExceptFilterBlock()
		{
			throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x000FFD40 File Offset: 0x000FED40
		public override void BeginCatchBlock(Type exceptionType)
		{
			if (this.m_currExcStackCount == 0)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
			}
			__ExceptionInfo _ExceptionInfo = this.m_currExcStack[this.m_currExcStackCount - 1];
			if (_ExceptionInfo.GetCurrentState() == 1)
			{
				if (exceptionType != null)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_ShouldNotSpecifyExceptionType"));
				}
				this.Emit(OpCodes.Endfilter);
			}
			else
			{
				if (exceptionType == null)
				{
					throw new ArgumentNullException("exceptionType");
				}
				if (exceptionType.GetType() != typeof(RuntimeType))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"));
				}
				Label endLabel = _ExceptionInfo.GetEndLabel();
				this.Emit(OpCodes.Leave, endLabel);
				base.UpdateStackSize(OpCodes.Nop, 1);
			}
			_ExceptionInfo.MarkCatchAddr(this.m_length, exceptionType);
			_ExceptionInfo.m_filterAddr[_ExceptionInfo.m_currentCatch - 1] = this.m_scope.GetTokenFor(exceptionType.TypeHandle);
		}

		// Token: 0x06004940 RID: 18752 RVA: 0x000FFE1A File Offset: 0x000FEE1A
		public override void BeginFaultBlock()
		{
			throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x000FFE2B File Offset: 0x000FEE2B
		public override void BeginFinallyBlock()
		{
			base.BeginFinallyBlock();
		}

		// Token: 0x06004942 RID: 18754 RVA: 0x000FFE33 File Offset: 0x000FEE33
		public override void UsingNamespace(string ns)
		{
			throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
		}

		// Token: 0x06004943 RID: 18755 RVA: 0x000FFE44 File Offset: 0x000FEE44
		public override void MarkSequencePoint(ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
		{
			throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
		}

		// Token: 0x06004944 RID: 18756 RVA: 0x000FFE55 File Offset: 0x000FEE55
		public override void BeginScope()
		{
			throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
		}

		// Token: 0x06004945 RID: 18757 RVA: 0x000FFE66 File Offset: 0x000FEE66
		public override void EndScope()
		{
			throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x000FFE77 File Offset: 0x000FEE77
		internal override int GetMaxStackSize()
		{
			return this.m_maxStackSize;
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x000FFE80 File Offset: 0x000FEE80
		internal override int GetMemberRefToken(MethodBase methodInfo, Type[] optionalParameterTypes)
		{
			if (optionalParameterTypes != null && (methodInfo.CallingConvention & CallingConventions.VarArgs) == (CallingConventions)0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAVarArgCallingConvention"));
			}
			if (!(methodInfo is RuntimeMethodInfo) && !(methodInfo is DynamicMethod))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeMethodInfo"), "methodInfo");
			}
			ParameterInfo[] parametersNoCopy = methodInfo.GetParametersNoCopy();
			Type[] array;
			if (parametersNoCopy != null && parametersNoCopy.Length != 0)
			{
				array = new Type[parametersNoCopy.Length];
				for (int i = 0; i < parametersNoCopy.Length; i++)
				{
					array[i] = parametersNoCopy[i].ParameterType;
				}
			}
			else
			{
				array = null;
			}
			SignatureHelper memberRefSignature = this.GetMemberRefSignature(methodInfo.CallingConvention, methodInfo.GetReturnType(), array, optionalParameterTypes);
			return this.m_scope.GetTokenFor(new VarArgMethod(methodInfo as MethodInfo, memberRefSignature));
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x000FFF3C File Offset: 0x000FEF3C
		internal override SignatureHelper GetMemberRefSignature(CallingConventions call, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			int num;
			if (parameterTypes == null)
			{
				num = 0;
			}
			else
			{
				num = parameterTypes.Length;
			}
			SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(call, returnType);
			for (int i = 0; i < num; i++)
			{
				methodSigHelper.AddArgument(parameterTypes[i]);
			}
			if (optionalParameterTypes != null && optionalParameterTypes.Length != 0)
			{
				methodSigHelper.AddSentinel();
				for (int i = 0; i < optionalParameterTypes.Length; i++)
				{
					methodSigHelper.AddArgument(optionalParameterTypes[i]);
				}
			}
			return methodSigHelper;
		}

		// Token: 0x06004949 RID: 18761 RVA: 0x000FFF9C File Offset: 0x000FEF9C
		private int AddStringLiteral(string s)
		{
			int tokenFor = this.m_scope.GetTokenFor(s);
			return tokenFor | 1879048192;
		}

		// Token: 0x0600494A RID: 18762 RVA: 0x000FFFC0 File Offset: 0x000FEFC0
		private int AddSignature(byte[] sig)
		{
			int tokenFor = this.m_scope.GetTokenFor(sig);
			return tokenFor | 285212672;
		}

		// Token: 0x0400255C RID: 9564
		internal DynamicScope m_scope;

		// Token: 0x0400255D RID: 9565
		private int m_methodSigToken;
	}
}
