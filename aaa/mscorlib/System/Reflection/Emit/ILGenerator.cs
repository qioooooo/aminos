using System;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000800 RID: 2048
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_ILGenerator))]
	[ComVisible(true)]
	public class ILGenerator : _ILGenerator
	{
		// Token: 0x060048E6 RID: 18662 RVA: 0x000FD808 File Offset: 0x000FC808
		internal static int[] EnlargeArray(int[] incoming)
		{
			int[] array = new int[incoming.Length * 2];
			Array.Copy(incoming, array, incoming.Length);
			return array;
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x000FD82C File Offset: 0x000FC82C
		internal static byte[] EnlargeArray(byte[] incoming)
		{
			byte[] array = new byte[incoming.Length * 2];
			Array.Copy(incoming, array, incoming.Length);
			return array;
		}

		// Token: 0x060048E8 RID: 18664 RVA: 0x000FD850 File Offset: 0x000FC850
		internal static byte[] EnlargeArray(byte[] incoming, int requiredSize)
		{
			byte[] array = new byte[requiredSize];
			Array.Copy(incoming, array, incoming.Length);
			return array;
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x000FD870 File Offset: 0x000FC870
		internal static __FixupData[] EnlargeArray(__FixupData[] incoming)
		{
			__FixupData[] array = new __FixupData[incoming.Length * 2];
			Array.Copy(incoming, array, incoming.Length);
			return array;
		}

		// Token: 0x060048EA RID: 18666 RVA: 0x000FD894 File Offset: 0x000FC894
		internal static Type[] EnlargeArray(Type[] incoming)
		{
			Type[] array = new Type[incoming.Length * 2];
			Array.Copy(incoming, array, incoming.Length);
			return array;
		}

		// Token: 0x060048EB RID: 18667 RVA: 0x000FD8B8 File Offset: 0x000FC8B8
		internal static __ExceptionInfo[] EnlargeArray(__ExceptionInfo[] incoming)
		{
			__ExceptionInfo[] array = new __ExceptionInfo[incoming.Length * 2];
			Array.Copy(incoming, array, incoming.Length);
			return array;
		}

		// Token: 0x060048EC RID: 18668 RVA: 0x000FD8DC File Offset: 0x000FC8DC
		internal static int CalculateNumberOfExceptions(__ExceptionInfo[] excp)
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

		// Token: 0x060048ED RID: 18669 RVA: 0x000FD90A File Offset: 0x000FC90A
		internal ILGenerator(MethodInfo methodBuilder)
			: this(methodBuilder, 64)
		{
		}

		// Token: 0x060048EE RID: 18670 RVA: 0x000FD918 File Offset: 0x000FC918
		internal ILGenerator(MethodInfo methodBuilder, int size)
		{
			if (size < 16)
			{
				this.m_ILStream = new byte[16];
			}
			else
			{
				this.m_ILStream = new byte[size];
			}
			this.m_length = 0;
			this.m_labelCount = 0;
			this.m_fixupCount = 0;
			this.m_labelList = null;
			this.m_fixupData = null;
			this.m_exceptions = null;
			this.m_exceptionCount = 0;
			this.m_currExcStack = null;
			this.m_currExcStackCount = 0;
			this.m_RelocFixupList = new int[64];
			this.m_RelocFixupCount = 0;
			this.m_RVAFixupList = new int[64];
			this.m_RVAFixupCount = 0;
			this.m_ScopeTree = new ScopeTree();
			this.m_LineNumberInfo = new LineNumberInfo();
			this.m_methodBuilder = methodBuilder;
			this.m_localCount = 0;
			MethodBuilder methodBuilder2 = this.m_methodBuilder as MethodBuilder;
			if (methodBuilder2 == null)
			{
				this.m_localSignature = SignatureHelper.GetLocalVarSigHelper(null);
				return;
			}
			this.m_localSignature = SignatureHelper.GetLocalVarSigHelper(methodBuilder2.GetTypeBuilder().Module);
		}

		// Token: 0x060048EF RID: 18671 RVA: 0x000FDA08 File Offset: 0x000FCA08
		internal ILGenerator(int size)
		{
			if (size < 16)
			{
				this.m_ILStream = new byte[16];
			}
			else
			{
				this.m_ILStream = new byte[size];
			}
			this.m_length = 0;
			this.m_labelCount = 0;
			this.m_fixupCount = 0;
			this.m_labelList = null;
			this.m_fixupData = null;
			this.m_exceptions = null;
			this.m_exceptionCount = 0;
			this.m_currExcStack = null;
			this.m_currExcStackCount = 0;
			this.m_RelocFixupList = new int[64];
			this.m_RelocFixupCount = 0;
			this.m_RVAFixupList = new int[64];
			this.m_RVAFixupCount = 0;
			this.m_ScopeTree = new ScopeTree();
			this.m_LineNumberInfo = new LineNumberInfo();
			this.m_methodBuilder = null;
			this.m_localCount = 0;
			this.m_localSignature = SignatureHelper.GetLocalVarSigHelper(null);
		}

		// Token: 0x060048F0 RID: 18672 RVA: 0x000FDAD4 File Offset: 0x000FCAD4
		private void RecordTokenFixup()
		{
			if (this.m_RelocFixupCount >= this.m_RelocFixupList.Length)
			{
				this.m_RelocFixupList = ILGenerator.EnlargeArray(this.m_RelocFixupList);
			}
			this.m_RelocFixupList[this.m_RelocFixupCount++] = this.m_length;
		}

		// Token: 0x060048F1 RID: 18673 RVA: 0x000FDB20 File Offset: 0x000FCB20
		internal void InternalEmit(OpCode opcode)
		{
			if (opcode.m_size == 1)
			{
				this.m_ILStream[this.m_length++] = opcode.m_s2;
			}
			else
			{
				this.m_ILStream[this.m_length++] = opcode.m_s1;
				this.m_ILStream[this.m_length++] = opcode.m_s2;
			}
			this.UpdateStackSize(opcode, opcode.StackChange());
		}

		// Token: 0x060048F2 RID: 18674 RVA: 0x000FDBA4 File Offset: 0x000FCBA4
		internal void UpdateStackSize(OpCode opcode, int stackchange)
		{
			this.m_maxMidStackCur += stackchange;
			if (this.m_maxMidStackCur > this.m_maxMidStack)
			{
				this.m_maxMidStack = this.m_maxMidStackCur;
			}
			else if (this.m_maxMidStackCur < 0)
			{
				this.m_maxMidStackCur = 0;
			}
			if (opcode.EndsUncondJmpBlk())
			{
				this.m_maxStackSize += this.m_maxMidStack;
				this.m_maxMidStack = 0;
				this.m_maxMidStackCur = 0;
			}
		}

		// Token: 0x060048F3 RID: 18675 RVA: 0x000FDC18 File Offset: 0x000FCC18
		internal virtual int GetMethodToken(MethodBase method, Type[] optionalParameterTypes)
		{
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
			int num;
			if (method.IsGenericMethod)
			{
				MethodInfo methodInfo = method as MethodInfo;
				if (!method.IsGenericMethodDefinition && method is MethodInfo)
				{
					methodInfo = ((MethodInfo)method).GetGenericMethodDefinition();
				}
				if (!methodInfo.Module.Equals(this.m_methodBuilder.Module) || (methodInfo.DeclaringType != null && methodInfo.DeclaringType.IsGenericType))
				{
					num = this.GetMemberRefToken(methodInfo, null);
				}
				else
				{
					num = moduleBuilder.GetMethodTokenInternal(methodInfo).Token;
				}
				int num2;
				byte[] array = SignatureHelper.GetMethodSpecSigHelper(moduleBuilder, method.GetGenericArguments()).InternalGetSignature(out num2);
				num = TypeBuilder.InternalDefineMethodSpec(num, array, num2, moduleBuilder);
			}
			else if ((method.CallingConvention & CallingConventions.VarArgs) == (CallingConventions)0 && (method.DeclaringType == null || !method.DeclaringType.IsGenericType))
			{
				if (method is MethodInfo)
				{
					num = moduleBuilder.GetMethodTokenInternal(method as MethodInfo).Token;
				}
				else
				{
					num = moduleBuilder.GetConstructorToken(method as ConstructorInfo).Token;
				}
			}
			else
			{
				num = this.GetMemberRefToken(method, optionalParameterTypes);
			}
			return num;
		}

		// Token: 0x060048F4 RID: 18676 RVA: 0x000FDD32 File Offset: 0x000FCD32
		internal virtual int GetMemberRefToken(MethodBase method, Type[] optionalParameterTypes)
		{
			return ((ModuleBuilder)this.m_methodBuilder.Module).GetMemberRefToken(method, optionalParameterTypes);
		}

		// Token: 0x060048F5 RID: 18677 RVA: 0x000FDD4B File Offset: 0x000FCD4B
		internal virtual SignatureHelper GetMemberRefSignature(CallingConventions call, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			return this.GetMemberRefSignature(call, returnType, parameterTypes, optionalParameterTypes, 0);
		}

		// Token: 0x060048F6 RID: 18678 RVA: 0x000FDD59 File Offset: 0x000FCD59
		internal virtual SignatureHelper GetMemberRefSignature(CallingConventions call, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes, int cGenericParameters)
		{
			return ((ModuleBuilder)this.m_methodBuilder.Module).GetMemberRefSignature(call, returnType, parameterTypes, optionalParameterTypes, cGenericParameters);
		}

		// Token: 0x060048F7 RID: 18679 RVA: 0x000FDD78 File Offset: 0x000FCD78
		internal virtual byte[] BakeByteArray()
		{
			if (this.m_currExcStackCount != 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_UnclosedExceptionBlock"));
			}
			if (this.m_length == 0)
			{
				return null;
			}
			int length = this.m_length;
			byte[] array = new byte[length];
			Array.Copy(this.m_ILStream, array, length);
			for (int i = 0; i < this.m_fixupCount; i++)
			{
				int num = this.GetLabelPos(this.m_fixupData[i].m_fixupLabel) - (this.m_fixupData[i].m_fixupPos + this.m_fixupData[i].m_fixupInstSize);
				if (this.m_fixupData[i].m_fixupInstSize == 1)
				{
					if (num < -128 || num > 127)
					{
						throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("NotSupported_IllegalOneByteBranch"), new object[]
						{
							this.m_fixupData[i].m_fixupPos,
							num
						}));
					}
					if (num < 0)
					{
						array[this.m_fixupData[i].m_fixupPos] = (byte)(256 + num);
					}
					else
					{
						array[this.m_fixupData[i].m_fixupPos] = (byte)num;
					}
				}
				else
				{
					this.PutInteger4(num, this.m_fixupData[i].m_fixupPos, array);
				}
			}
			return array;
		}

		// Token: 0x060048F8 RID: 18680 RVA: 0x000FDED4 File Offset: 0x000FCED4
		internal virtual __ExceptionInfo[] GetExceptions()
		{
			if (this.m_currExcStackCount != 0)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_UnclosedExceptionBlock"));
			}
			if (this.m_exceptionCount == 0)
			{
				return null;
			}
			__ExceptionInfo[] array = new __ExceptionInfo[this.m_exceptionCount];
			Array.Copy(this.m_exceptions, array, this.m_exceptionCount);
			this.SortExceptions(array);
			return array;
		}

		// Token: 0x060048F9 RID: 18681 RVA: 0x000FDF2C File Offset: 0x000FCF2C
		internal virtual void EnsureCapacity(int size)
		{
			if (this.m_length + size >= this.m_ILStream.Length)
			{
				if (this.m_length + size >= 2 * this.m_ILStream.Length)
				{
					this.m_ILStream = ILGenerator.EnlargeArray(this.m_ILStream, this.m_length + size);
					return;
				}
				this.m_ILStream = ILGenerator.EnlargeArray(this.m_ILStream);
			}
		}

		// Token: 0x060048FA RID: 18682 RVA: 0x000FDF8A File Offset: 0x000FCF8A
		internal virtual int PutInteger4(int value, int startPos, byte[] array)
		{
			array[startPos++] = (byte)value;
			array[startPos++] = (byte)(value >> 8);
			array[startPos++] = (byte)(value >> 16);
			array[startPos++] = (byte)(value >> 24);
			return startPos;
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x000FDFC0 File Offset: 0x000FCFC0
		internal virtual int GetLabelPos(Label lbl)
		{
			int labelValue = lbl.GetLabelValue();
			if (labelValue < 0 || labelValue >= this.m_labelCount)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadLabel"));
			}
			if (this.m_labelList[labelValue] < 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadLabelContent"));
			}
			return this.m_labelList[labelValue];
		}

		// Token: 0x060048FC RID: 18684 RVA: 0x000FE018 File Offset: 0x000FD018
		internal virtual void AddFixup(Label lbl, int pos, int instSize)
		{
			if (this.m_fixupData == null)
			{
				this.m_fixupData = new __FixupData[64];
			}
			if (this.m_fixupCount >= this.m_fixupData.Length)
			{
				this.m_fixupData = ILGenerator.EnlargeArray(this.m_fixupData);
			}
			this.m_fixupData[this.m_fixupCount].m_fixupPos = pos;
			this.m_fixupData[this.m_fixupCount].m_fixupLabel = lbl;
			this.m_fixupData[this.m_fixupCount].m_fixupInstSize = instSize;
			this.m_fixupCount++;
		}

		// Token: 0x060048FD RID: 18685 RVA: 0x000FE0B0 File Offset: 0x000FD0B0
		internal virtual int GetMaxStackSize()
		{
			MethodBuilder methodBuilder = this.m_methodBuilder as MethodBuilder;
			if (methodBuilder == null)
			{
				throw new NotSupportedException();
			}
			return this.m_maxStackSize + methodBuilder.GetNumberOfExceptions();
		}

		// Token: 0x060048FE RID: 18686 RVA: 0x000FE0E0 File Offset: 0x000FD0E0
		internal virtual void SortExceptions(__ExceptionInfo[] exceptions)
		{
			int num = exceptions.Length;
			for (int i = 0; i < num; i++)
			{
				int num2 = i;
				for (int j = i + 1; j < num; j++)
				{
					if (exceptions[num2].IsInner(exceptions[j]))
					{
						num2 = j;
					}
				}
				__ExceptionInfo _ExceptionInfo = exceptions[i];
				exceptions[i] = exceptions[num2];
				exceptions[num2] = _ExceptionInfo;
			}
		}

		// Token: 0x060048FF RID: 18687 RVA: 0x000FE130 File Offset: 0x000FD130
		internal virtual int[] GetTokenFixups()
		{
			int[] array = new int[this.m_RelocFixupCount];
			Array.Copy(this.m_RelocFixupList, array, this.m_RelocFixupCount);
			return array;
		}

		// Token: 0x06004900 RID: 18688 RVA: 0x000FE15C File Offset: 0x000FD15C
		internal virtual int[] GetRVAFixups()
		{
			int[] array = new int[this.m_RVAFixupCount];
			Array.Copy(this.m_RVAFixupList, array, this.m_RVAFixupCount);
			return array;
		}

		// Token: 0x06004901 RID: 18689 RVA: 0x000FE188 File Offset: 0x000FD188
		public virtual void Emit(OpCode opcode)
		{
			this.EnsureCapacity(3);
			this.InternalEmit(opcode);
		}

		// Token: 0x06004902 RID: 18690 RVA: 0x000FE198 File Offset: 0x000FD198
		public virtual void Emit(OpCode opcode, byte arg)
		{
			this.EnsureCapacity(4);
			this.InternalEmit(opcode);
			this.m_ILStream[this.m_length++] = arg;
		}

		// Token: 0x06004903 RID: 18691 RVA: 0x000FE1CC File Offset: 0x000FD1CC
		[CLSCompliant(false)]
		public void Emit(OpCode opcode, sbyte arg)
		{
			this.EnsureCapacity(4);
			this.InternalEmit(opcode);
			if (arg < 0)
			{
				this.m_ILStream[this.m_length++] = (byte)(256 + (int)arg);
				return;
			}
			this.m_ILStream[this.m_length++] = (byte)arg;
		}

		// Token: 0x06004904 RID: 18692 RVA: 0x000FE228 File Offset: 0x000FD228
		public virtual void Emit(OpCode opcode, short arg)
		{
			this.EnsureCapacity(5);
			this.InternalEmit(opcode);
			this.m_ILStream[this.m_length++] = (byte)arg;
			this.m_ILStream[this.m_length++] = (byte)(arg >> 8);
		}

		// Token: 0x06004905 RID: 18693 RVA: 0x000FE279 File Offset: 0x000FD279
		public virtual void Emit(OpCode opcode, int arg)
		{
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			this.m_length = this.PutInteger4(arg, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004906 RID: 18694 RVA: 0x000FE2A4 File Offset: 0x000FD2A4
		public virtual void Emit(OpCode opcode, MethodInfo meth)
		{
			if (opcode.Equals(OpCodes.Call) || opcode.Equals(OpCodes.Callvirt) || opcode.Equals(OpCodes.Newobj))
			{
				this.EmitCall(opcode, meth, null);
				return;
			}
			int num = 0;
			if (meth == null)
			{
				throw new ArgumentNullException("meth");
			}
			int methodToken = this.GetMethodToken(meth, null);
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			this.UpdateStackSize(opcode, num);
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(methodToken, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004907 RID: 18695 RVA: 0x000FE334 File Offset: 0x000FD334
		public virtual void EmitCalli(OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			int num = 0;
			if (optionalParameterTypes != null && (callingConvention & CallingConventions.VarArgs) == (CallingConventions)0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAVarArgCallingConvention"));
			}
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
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
			this.UpdateStackSize(opcode, num);
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(moduleBuilder.GetSignatureToken(memberRefSignature).Token, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004908 RID: 18696 RVA: 0x000FE3F8 File Offset: 0x000FD3F8
		public virtual void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			int num = 0;
			int num2 = 0;
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
			if (parameterTypes != null)
			{
				num2 = parameterTypes.Length;
			}
			SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(moduleBuilder, unmanagedCallConv, returnType);
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
			this.UpdateStackSize(opcode, num);
			this.EnsureCapacity(7);
			this.Emit(OpCodes.Calli);
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(moduleBuilder.GetSignatureToken(methodSigHelper).Token, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004909 RID: 18697 RVA: 0x000FE4B0 File Offset: 0x000FD4B0
		public virtual void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			int num = 0;
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			int methodToken = this.GetMethodToken(methodInfo, optionalParameterTypes);
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			if (methodInfo.GetReturnType() != typeof(void))
			{
				num++;
			}
			if (methodInfo.GetParameterTypes() != null)
			{
				num -= methodInfo.GetParameterTypes().Length;
			}
			if (!(methodInfo is SymbolMethod) && !methodInfo.IsStatic && !opcode.Equals(OpCodes.Newobj))
			{
				num--;
			}
			if (optionalParameterTypes != null)
			{
				num -= optionalParameterTypes.Length;
			}
			this.UpdateStackSize(opcode, num);
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(methodToken, this.m_length, this.m_ILStream);
		}

		// Token: 0x0600490A RID: 18698 RVA: 0x000FE560 File Offset: 0x000FD560
		public virtual void Emit(OpCode opcode, SignatureHelper signature)
		{
			int num = 0;
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
			if (signature == null)
			{
				throw new ArgumentNullException("signature");
			}
			int token = moduleBuilder.GetSignatureToken(signature).Token;
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			if (opcode.m_pop == StackBehaviour.Varpop)
			{
				num -= signature.ArgumentCount;
				num--;
				this.UpdateStackSize(opcode, num);
			}
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(token, this.m_length, this.m_ILStream);
		}

		// Token: 0x0600490B RID: 18699 RVA: 0x000FE5EC File Offset: 0x000FD5EC
		[ComVisible(true)]
		public virtual void Emit(OpCode opcode, ConstructorInfo con)
		{
			int num = 0;
			int methodToken = this.GetMethodToken(con, null);
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			if (opcode.m_push == StackBehaviour.Varpush)
			{
				num++;
			}
			if (opcode.m_pop == StackBehaviour.Varpop && con.GetParameterTypes() != null)
			{
				num -= con.GetParameterTypes().Length;
			}
			this.UpdateStackSize(opcode, num);
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(methodToken, this.m_length, this.m_ILStream);
		}

		// Token: 0x0600490C RID: 18700 RVA: 0x000FE668 File Offset: 0x000FD668
		public virtual void Emit(OpCode opcode, Type cls)
		{
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
			int num;
			if (opcode == OpCodes.Ldtoken && cls != null && cls.IsGenericTypeDefinition)
			{
				num = moduleBuilder.GetTypeToken(cls).Token;
			}
			else
			{
				num = moduleBuilder.GetTypeTokenInternal(cls).Token;
			}
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(num, this.m_length, this.m_ILStream);
		}

		// Token: 0x0600490D RID: 18701 RVA: 0x000FE6F0 File Offset: 0x000FD6F0
		public virtual void Emit(OpCode opcode, long arg)
		{
			this.EnsureCapacity(11);
			this.InternalEmit(opcode);
			this.m_ILStream[this.m_length++] = (byte)arg;
			this.m_ILStream[this.m_length++] = (byte)(arg >> 8);
			this.m_ILStream[this.m_length++] = (byte)(arg >> 16);
			this.m_ILStream[this.m_length++] = (byte)(arg >> 24);
			this.m_ILStream[this.m_length++] = (byte)(arg >> 32);
			this.m_ILStream[this.m_length++] = (byte)(arg >> 40);
			this.m_ILStream[this.m_length++] = (byte)(arg >> 48);
			this.m_ILStream[this.m_length++] = (byte)(arg >> 56);
		}

		// Token: 0x0600490E RID: 18702 RVA: 0x000FE7F8 File Offset: 0x000FD7F8
		public unsafe virtual void Emit(OpCode opcode, float arg)
		{
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			uint num = *(uint*)(&arg);
			this.m_ILStream[this.m_length++] = (byte)num;
			this.m_ILStream[this.m_length++] = (byte)(num >> 8);
			this.m_ILStream[this.m_length++] = (byte)(num >> 16);
			this.m_ILStream[this.m_length++] = (byte)(num >> 24);
		}

		// Token: 0x0600490F RID: 18703 RVA: 0x000FE88C File Offset: 0x000FD88C
		public unsafe virtual void Emit(OpCode opcode, double arg)
		{
			this.EnsureCapacity(11);
			this.InternalEmit(opcode);
			ulong num = (ulong)(*(long*)(&arg));
			this.m_ILStream[this.m_length++] = (byte)num;
			this.m_ILStream[this.m_length++] = (byte)(num >> 8);
			this.m_ILStream[this.m_length++] = (byte)(num >> 16);
			this.m_ILStream[this.m_length++] = (byte)(num >> 24);
			this.m_ILStream[this.m_length++] = (byte)(num >> 32);
			this.m_ILStream[this.m_length++] = (byte)(num >> 40);
			this.m_ILStream[this.m_length++] = (byte)(num >> 48);
			this.m_ILStream[this.m_length++] = (byte)(num >> 56);
		}

		// Token: 0x06004910 RID: 18704 RVA: 0x000FE99C File Offset: 0x000FD99C
		public virtual void Emit(OpCode opcode, Label label)
		{
			label.GetLabelValue();
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			if (OpCodes.TakesSingleByteArgument(opcode))
			{
				this.AddFixup(label, this.m_length, 1);
				this.m_length++;
				return;
			}
			this.AddFixup(label, this.m_length, 4);
			this.m_length += 4;
		}

		// Token: 0x06004911 RID: 18705 RVA: 0x000FEA00 File Offset: 0x000FDA00
		public virtual void Emit(OpCode opcode, Label[] labels)
		{
			int num = labels.Length;
			this.EnsureCapacity(num * 4 + 7);
			this.InternalEmit(opcode);
			this.m_length = this.PutInteger4(num, this.m_length, this.m_ILStream);
			int i = num * 4;
			int num2 = 0;
			while (i > 0)
			{
				this.AddFixup(labels[num2], this.m_length, i);
				this.m_length += 4;
				i -= 4;
				num2++;
			}
		}

		// Token: 0x06004912 RID: 18706 RVA: 0x000FEA78 File Offset: 0x000FDA78
		public virtual void Emit(OpCode opcode, FieldInfo field)
		{
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
			int token = moduleBuilder.GetFieldToken(field).Token;
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(token, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004913 RID: 18707 RVA: 0x000FEAD4 File Offset: 0x000FDAD4
		public virtual void Emit(OpCode opcode, string str)
		{
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
			int token = moduleBuilder.GetStringConstant(str).Token;
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			this.m_length = this.PutInteger4(token, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004914 RID: 18708 RVA: 0x000FEB2C File Offset: 0x000FDB2C
		public virtual void Emit(OpCode opcode, LocalBuilder local)
		{
			if (local == null)
			{
				throw new ArgumentNullException("local");
			}
			int localIndex = local.GetLocalIndex();
			if (local.GetMethodBuilder() != this.m_methodBuilder)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_UnmatchedMethodForLocal"), "local");
			}
			if (opcode.Equals(OpCodes.Ldloc))
			{
				switch (localIndex)
				{
				case 0:
					opcode = OpCodes.Ldloc_0;
					break;
				case 1:
					opcode = OpCodes.Ldloc_1;
					break;
				case 2:
					opcode = OpCodes.Ldloc_2;
					break;
				case 3:
					opcode = OpCodes.Ldloc_3;
					break;
				default:
					if (localIndex <= 255)
					{
						opcode = OpCodes.Ldloc_S;
					}
					break;
				}
			}
			else if (opcode.Equals(OpCodes.Stloc))
			{
				switch (localIndex)
				{
				case 0:
					opcode = OpCodes.Stloc_0;
					break;
				case 1:
					opcode = OpCodes.Stloc_1;
					break;
				case 2:
					opcode = OpCodes.Stloc_2;
					break;
				case 3:
					opcode = OpCodes.Stloc_3;
					break;
				default:
					if (localIndex <= 255)
					{
						opcode = OpCodes.Stloc_S;
					}
					break;
				}
			}
			else if (opcode.Equals(OpCodes.Ldloca) && localIndex <= 255)
			{
				opcode = OpCodes.Ldloca_S;
			}
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			if (opcode.OperandType == OperandType.InlineNone)
			{
				return;
			}
			if (!OpCodes.TakesSingleByteArgument(opcode))
			{
				this.m_ILStream[this.m_length++] = (byte)localIndex;
				this.m_ILStream[this.m_length++] = (byte)(localIndex >> 8);
				return;
			}
			if (localIndex > 255)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadInstructionOrIndexOutOfBound"));
			}
			this.m_ILStream[this.m_length++] = (byte)localIndex;
		}

		// Token: 0x06004915 RID: 18709 RVA: 0x000FECE8 File Offset: 0x000FDCE8
		public virtual Label BeginExceptionBlock()
		{
			if (this.m_exceptions == null)
			{
				this.m_exceptions = new __ExceptionInfo[8];
			}
			if (this.m_currExcStack == null)
			{
				this.m_currExcStack = new __ExceptionInfo[8];
			}
			if (this.m_exceptionCount >= this.m_exceptions.Length)
			{
				this.m_exceptions = ILGenerator.EnlargeArray(this.m_exceptions);
			}
			if (this.m_currExcStackCount >= this.m_currExcStack.Length)
			{
				this.m_currExcStack = ILGenerator.EnlargeArray(this.m_currExcStack);
			}
			Label label = this.DefineLabel();
			__ExceptionInfo _ExceptionInfo = new __ExceptionInfo(this.m_length, label);
			this.m_exceptions[this.m_exceptionCount++] = _ExceptionInfo;
			this.m_currExcStack[this.m_currExcStackCount++] = _ExceptionInfo;
			return label;
		}

		// Token: 0x06004916 RID: 18710 RVA: 0x000FEDA8 File Offset: 0x000FDDA8
		public virtual void EndExceptionBlock()
		{
			if (this.m_currExcStackCount == 0)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
			}
			__ExceptionInfo _ExceptionInfo = this.m_currExcStack[this.m_currExcStackCount - 1];
			this.m_currExcStack[this.m_currExcStackCount - 1] = null;
			this.m_currExcStackCount--;
			Label endLabel = _ExceptionInfo.GetEndLabel();
			int currentState = _ExceptionInfo.GetCurrentState();
			if (currentState == 1 || currentState == 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Argument_BadExceptionCodeGen"));
			}
			if (currentState == 2)
			{
				this.Emit(OpCodes.Leave, endLabel);
			}
			else if (currentState == 3 || currentState == 4)
			{
				this.Emit(OpCodes.Endfinally);
			}
			if (this.m_labelList[endLabel.GetLabelValue()] == -1)
			{
				this.MarkLabel(endLabel);
			}
			else
			{
				this.MarkLabel(_ExceptionInfo.GetFinallyEndLabel());
			}
			_ExceptionInfo.Done(this.m_length);
		}

		// Token: 0x06004917 RID: 18711 RVA: 0x000FEE78 File Offset: 0x000FDE78
		public virtual void BeginExceptFilterBlock()
		{
			if (this.m_currExcStackCount == 0)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
			}
			__ExceptionInfo _ExceptionInfo = this.m_currExcStack[this.m_currExcStackCount - 1];
			Label endLabel = _ExceptionInfo.GetEndLabel();
			this.Emit(OpCodes.Leave, endLabel);
			_ExceptionInfo.MarkFilterAddr(this.m_length);
		}

		// Token: 0x06004918 RID: 18712 RVA: 0x000FEECC File Offset: 0x000FDECC
		public virtual void BeginCatchBlock(Type exceptionType)
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
				Label endLabel = _ExceptionInfo.GetEndLabel();
				this.Emit(OpCodes.Leave, endLabel);
			}
			_ExceptionInfo.MarkCatchAddr(this.m_length, exceptionType);
		}

		// Token: 0x06004919 RID: 18713 RVA: 0x000FEF58 File Offset: 0x000FDF58
		public virtual void BeginFaultBlock()
		{
			if (this.m_currExcStackCount == 0)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
			}
			__ExceptionInfo _ExceptionInfo = this.m_currExcStack[this.m_currExcStackCount - 1];
			Label endLabel = _ExceptionInfo.GetEndLabel();
			this.Emit(OpCodes.Leave, endLabel);
			_ExceptionInfo.MarkFaultAddr(this.m_length);
		}

		// Token: 0x0600491A RID: 18714 RVA: 0x000FEFAC File Offset: 0x000FDFAC
		public virtual void BeginFinallyBlock()
		{
			if (this.m_currExcStackCount == 0)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
			}
			__ExceptionInfo _ExceptionInfo = this.m_currExcStack[this.m_currExcStackCount - 1];
			int currentState = _ExceptionInfo.GetCurrentState();
			Label endLabel = _ExceptionInfo.GetEndLabel();
			int num = 0;
			if (currentState != 0)
			{
				this.Emit(OpCodes.Leave, endLabel);
				num = this.m_length;
			}
			this.MarkLabel(endLabel);
			Label label = this.DefineLabel();
			_ExceptionInfo.SetFinallyEndLabel(label);
			this.Emit(OpCodes.Leave, label);
			if (num == 0)
			{
				num = this.m_length;
			}
			_ExceptionInfo.MarkFinallyAddr(this.m_length, num);
		}

		// Token: 0x0600491B RID: 18715 RVA: 0x000FF044 File Offset: 0x000FE044
		public virtual Label DefineLabel()
		{
			if (this.m_labelList == null)
			{
				this.m_labelList = new int[16];
			}
			if (this.m_labelCount >= this.m_labelList.Length)
			{
				this.m_labelList = ILGenerator.EnlargeArray(this.m_labelList);
			}
			this.m_labelList[this.m_labelCount] = -1;
			return new Label(this.m_labelCount++);
		}

		// Token: 0x0600491C RID: 18716 RVA: 0x000FF0AC File Offset: 0x000FE0AC
		public virtual void MarkLabel(Label loc)
		{
			int labelValue = loc.GetLabelValue();
			if (labelValue < 0 || labelValue >= this.m_labelList.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidLabel"));
			}
			if (this.m_labelList[labelValue] != -1)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_RedefinedLabel"));
			}
			this.m_labelList[labelValue] = this.m_length;
		}

		// Token: 0x0600491D RID: 18717 RVA: 0x000FF10C File Offset: 0x000FE10C
		public virtual void ThrowException(Type excType)
		{
			if (excType == null)
			{
				throw new ArgumentNullException("excType");
			}
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
			if (!excType.IsSubclassOf(typeof(Exception)) && excType != typeof(Exception))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotExceptionType"));
			}
			ConstructorInfo constructor = excType.GetConstructor(Type.EmptyTypes);
			if (constructor == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MissingDefaultConstructor"));
			}
			this.Emit(OpCodes.Newobj, constructor);
			this.Emit(OpCodes.Throw);
		}

		// Token: 0x0600491E RID: 18718 RVA: 0x000FF1A0 File Offset: 0x000FE1A0
		public virtual void EmitWriteLine(string value)
		{
			this.Emit(OpCodes.Ldstr, value);
			Type[] array = new Type[] { typeof(string) };
			MethodInfo method = typeof(Console).GetMethod("WriteLine", array);
			this.Emit(OpCodes.Call, method);
		}

		// Token: 0x0600491F RID: 18719 RVA: 0x000FF1F0 File Offset: 0x000FE1F0
		public virtual void EmitWriteLine(LocalBuilder localBuilder)
		{
			if (this.m_methodBuilder == null)
			{
				throw new ArgumentException(Environment.GetResourceString("InvalidOperation_BadILGeneratorUsage"));
			}
			MethodInfo method = typeof(Console).GetMethod("get_Out");
			this.Emit(OpCodes.Call, method);
			this.Emit(OpCodes.Ldloc, localBuilder);
			Type[] array = new Type[1];
			object localType = localBuilder.LocalType;
			if (localType is TypeBuilder || localType is EnumBuilder)
			{
				throw new ArgumentException(Environment.GetResourceString("NotSupported_OutputStreamUsingTypeBuilder"));
			}
			array[0] = (Type)localType;
			MethodInfo method2 = typeof(TextWriter).GetMethod("WriteLine", array);
			if (method2 == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmitWriteLineType"), "localBuilder");
			}
			this.Emit(OpCodes.Callvirt, method2);
		}

		// Token: 0x06004920 RID: 18720 RVA: 0x000FF2B4 File Offset: 0x000FE2B4
		public virtual void EmitWriteLine(FieldInfo fld)
		{
			if (fld == null)
			{
				throw new ArgumentNullException("fld");
			}
			MethodInfo method = typeof(Console).GetMethod("get_Out");
			this.Emit(OpCodes.Call, method);
			if ((fld.Attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope)
			{
				this.Emit(OpCodes.Ldsfld, fld);
			}
			else
			{
				this.Emit(OpCodes.Ldarg, 0);
				this.Emit(OpCodes.Ldfld, fld);
			}
			Type[] array = new Type[1];
			object fieldType = fld.FieldType;
			if (fieldType is TypeBuilder || fieldType is EnumBuilder)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_OutputStreamUsingTypeBuilder"));
			}
			array[0] = (Type)fieldType;
			MethodInfo method2 = typeof(TextWriter).GetMethod("WriteLine", array);
			if (method2 == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmitWriteLineType"), "fld");
			}
			this.Emit(OpCodes.Callvirt, method2);
		}

		// Token: 0x06004921 RID: 18721 RVA: 0x000FF392 File Offset: 0x000FE392
		public virtual LocalBuilder DeclareLocal(Type localType)
		{
			return this.DeclareLocal(localType, false);
		}

		// Token: 0x06004922 RID: 18722 RVA: 0x000FF39C File Offset: 0x000FE39C
		public virtual LocalBuilder DeclareLocal(Type localType, bool pinned)
		{
			MethodBuilder methodBuilder = this.m_methodBuilder as MethodBuilder;
			if (methodBuilder == null)
			{
				throw new NotSupportedException();
			}
			if (methodBuilder.IsTypeCreated())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TypeHasBeenCreated"));
			}
			if (localType == null)
			{
				throw new ArgumentNullException("localType");
			}
			if (methodBuilder.m_bIsBaked)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MethodBaked"));
			}
			this.m_localSignature.AddArgument(localType, pinned);
			LocalBuilder localBuilder = new LocalBuilder(this.m_localCount, localType, methodBuilder, pinned);
			this.m_localCount++;
			return localBuilder;
		}

		// Token: 0x06004923 RID: 18723 RVA: 0x000FF428 File Offset: 0x000FE428
		public virtual void UsingNamespace(string usingNamespace)
		{
			if (usingNamespace == null)
			{
				throw new ArgumentNullException("usingNamespace");
			}
			if (usingNamespace.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "usingNamespace");
			}
			MethodBuilder methodBuilder = this.m_methodBuilder as MethodBuilder;
			if (methodBuilder == null)
			{
				throw new NotSupportedException();
			}
			int currentActiveScopeIndex = methodBuilder.GetILGenerator().m_ScopeTree.GetCurrentActiveScopeIndex();
			if (currentActiveScopeIndex == -1)
			{
				methodBuilder.m_localSymInfo.AddUsingNamespace(usingNamespace);
				return;
			}
			this.m_ScopeTree.AddUsingNamespaceToCurrentScope(usingNamespace);
		}

		// Token: 0x06004924 RID: 18724 RVA: 0x000FF4A3 File Offset: 0x000FE4A3
		public virtual void MarkSequencePoint(ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
		{
			if (startLine == 0 || startLine < 0 || endLine == 0 || endLine < 0)
			{
				throw new ArgumentOutOfRangeException("startLine");
			}
			this.m_LineNumberInfo.AddLineNumberInfo(document, this.m_length, startLine, startColumn, endLine, endColumn);
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x000FF4D8 File Offset: 0x000FE4D8
		public virtual void BeginScope()
		{
			this.m_ScopeTree.AddScopeInfo(ScopeAction.Open, this.m_length);
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x000FF4EC File Offset: 0x000FE4EC
		public virtual void EndScope()
		{
			this.m_ScopeTree.AddScopeInfo(ScopeAction.Close, this.m_length);
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x000FF500 File Offset: 0x000FE500
		void _ILGenerator.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004928 RID: 18728 RVA: 0x000FF507 File Offset: 0x000FE507
		void _ILGenerator.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004929 RID: 18729 RVA: 0x000FF50E File Offset: 0x000FE50E
		void _ILGenerator.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600492A RID: 18730 RVA: 0x000FF515 File Offset: 0x000FE515
		void _ILGenerator.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04002541 RID: 9537
		internal const byte PrefixInstruction = 255;

		// Token: 0x04002542 RID: 9538
		internal const int defaultSize = 16;

		// Token: 0x04002543 RID: 9539
		internal const int DefaultFixupArraySize = 64;

		// Token: 0x04002544 RID: 9540
		internal const int DefaultLabelArraySize = 16;

		// Token: 0x04002545 RID: 9541
		internal const int DefaultExceptionArraySize = 8;

		// Token: 0x04002546 RID: 9542
		internal int m_length;

		// Token: 0x04002547 RID: 9543
		internal byte[] m_ILStream;

		// Token: 0x04002548 RID: 9544
		internal int[] m_labelList;

		// Token: 0x04002549 RID: 9545
		internal int m_labelCount;

		// Token: 0x0400254A RID: 9546
		internal __FixupData[] m_fixupData;

		// Token: 0x0400254B RID: 9547
		internal int m_fixupCount;

		// Token: 0x0400254C RID: 9548
		internal int[] m_RVAFixupList;

		// Token: 0x0400254D RID: 9549
		internal int m_RVAFixupCount;

		// Token: 0x0400254E RID: 9550
		internal int[] m_RelocFixupList;

		// Token: 0x0400254F RID: 9551
		internal int m_RelocFixupCount;

		// Token: 0x04002550 RID: 9552
		internal int m_exceptionCount;

		// Token: 0x04002551 RID: 9553
		internal int m_currExcStackCount;

		// Token: 0x04002552 RID: 9554
		internal __ExceptionInfo[] m_exceptions;

		// Token: 0x04002553 RID: 9555
		internal __ExceptionInfo[] m_currExcStack;

		// Token: 0x04002554 RID: 9556
		internal ScopeTree m_ScopeTree;

		// Token: 0x04002555 RID: 9557
		internal LineNumberInfo m_LineNumberInfo;

		// Token: 0x04002556 RID: 9558
		internal MethodInfo m_methodBuilder;

		// Token: 0x04002557 RID: 9559
		internal int m_localCount;

		// Token: 0x04002558 RID: 9560
		internal SignatureHelper m_localSignature;

		// Token: 0x04002559 RID: 9561
		internal int m_maxStackSize;

		// Token: 0x0400255A RID: 9562
		internal int m_maxMidStack;

		// Token: 0x0400255B RID: 9563
		internal int m_maxMidStackCur;
	}
}
