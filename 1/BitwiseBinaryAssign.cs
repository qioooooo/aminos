using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200002B RID: 43
	internal sealed class BitwiseBinaryAssign : BinaryOp
	{
		// Token: 0x060001C8 RID: 456 RVA: 0x0000DEC1 File Offset: 0x0000CEC1
		internal BitwiseBinaryAssign(Context context, AST operand1, AST operand2, JSToken operatorTok)
			: base(context, operand1, operand2, operatorTok)
		{
			this.binOp = new BitwiseBinary(context, operand1, operand2, operatorTok);
			this.metaData = null;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000DEE8 File Offset: 0x0000CEE8
		internal override object Evaluate()
		{
			object obj = this.operand1.Evaluate();
			object obj2 = this.operand2.Evaluate();
			object obj3 = this.binOp.EvaluateBitwiseBinary(obj, obj2);
			object obj4;
			try
			{
				this.operand1.SetValue(obj3);
				obj4 = obj3;
			}
			catch (JScriptException ex)
			{
				if (ex.context == null)
				{
					ex.context = this.context;
				}
				throw ex;
			}
			catch (Exception ex2)
			{
				throw new JScriptException(ex2, this.context);
			}
			catch
			{
				throw new JScriptException(JSError.NonClsException, this.context);
			}
			return obj4;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000DF90 File Offset: 0x0000CF90
		internal override IReflect InferType(JSField inference_target)
		{
			MethodInfo methodInfo;
			if (this.type1 == null)
			{
				methodInfo = base.GetOperator(this.operand1.InferType(inference_target), this.operand2.InferType(inference_target));
			}
			else
			{
				methodInfo = base.GetOperator(this.type1, this.type2);
			}
			if (methodInfo != null)
			{
				this.metaData = methodInfo;
				return methodInfo.ReturnType;
			}
			if ((this.type1.IsPrimitive || Typeob.JSObject.IsAssignableFrom(this.type1)) && (this.type2.IsPrimitive || Typeob.JSObject.IsAssignableFrom(this.type2)))
			{
				return Typeob.Int32;
			}
			return Typeob.Object;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000E034 File Offset: 0x0000D034
		internal override AST PartiallyEvaluate()
		{
			this.operand1 = this.operand1.PartiallyEvaluateAsReference();
			this.operand2 = this.operand2.PartiallyEvaluate();
			this.binOp = new BitwiseBinary(this.context, this.operand1, this.operand2, this.operatorTok);
			this.operand1.SetPartialValue(this.binOp);
			return this;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000E098 File Offset: 0x0000D098
		private void TranslateToILForNoOverloadCase(ILGenerator il, Type rtype)
		{
			Type type = Convert.ToType(this.operand1.InferType(null));
			Type type2 = Convert.ToType(this.operand2.InferType(null));
			Type type3 = BitwiseBinary.ResultType(type, type2, this.operatorTok);
			this.operand1.TranslateToILPreSetPlusGet(il);
			Convert.Emit(this, il, type, type3, true);
			this.operand2.TranslateToIL(il, type2);
			Convert.Emit(this, il, type2, BitwiseBinary.Operand2Type(this.operatorTok, type3), true);
			JSToken operatorTok = this.operatorTok;
			switch (operatorTok)
			{
			case JSToken.BitwiseOr:
				il.Emit(OpCodes.Or);
				break;
			case JSToken.BitwiseXor:
				il.Emit(OpCodes.Xor);
				break;
			case JSToken.BitwiseAnd:
				il.Emit(OpCodes.And);
				break;
			default:
				switch (operatorTok)
				{
				case JSToken.LeftShift:
					BitwiseBinary.TranslateToBitCountMask(il, type3, this.operand2);
					il.Emit(OpCodes.Shl);
					break;
				case JSToken.RightShift:
					BitwiseBinary.TranslateToBitCountMask(il, type3, this.operand2);
					il.Emit(OpCodes.Shr);
					break;
				case JSToken.UnsignedRightShift:
					BitwiseBinary.TranslateToBitCountMask(il, type3, this.operand2);
					il.Emit(OpCodes.Shr_Un);
					break;
				default:
					throw new JScriptException(JSError.InternalError, this.context);
				}
				break;
			}
			if (rtype != Typeob.Void)
			{
				LocalBuilder localBuilder = il.DeclareLocal(type3);
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Stloc, localBuilder);
				Convert.Emit(this, il, type3, type);
				this.operand1.TranslateToILSet(il);
				il.Emit(OpCodes.Ldloc, localBuilder);
				Convert.Emit(this, il, type3, rtype);
				return;
			}
			Convert.Emit(this, il, type3, type);
			this.operand1.TranslateToILSet(il);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000E230 File Offset: 0x0000D230
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if (this.metaData == null)
			{
				this.TranslateToILForNoOverloadCase(il, rtype);
				return;
			}
			if (this.metaData is MethodInfo)
			{
				object obj = null;
				MethodInfo methodInfo = (MethodInfo)this.metaData;
				Type type = Convert.ToType(this.operand1.InferType(null));
				ParameterInfo[] parameters = methodInfo.GetParameters();
				this.operand1.TranslateToILPreSetPlusGet(il);
				Convert.Emit(this, il, type, parameters[0].ParameterType);
				this.operand2.TranslateToIL(il, parameters[1].ParameterType);
				il.Emit(OpCodes.Call, methodInfo);
				if (rtype != Typeob.Void)
				{
					obj = il.DeclareLocal(rtype);
					il.Emit(OpCodes.Dup);
					Convert.Emit(this, il, type, rtype);
					il.Emit(OpCodes.Stloc, (LocalBuilder)obj);
				}
				Convert.Emit(this, il, methodInfo.ReturnType, type);
				this.operand1.TranslateToILSet(il);
				if (rtype != Typeob.Void)
				{
					il.Emit(OpCodes.Ldloc, (LocalBuilder)obj);
					return;
				}
			}
			else
			{
				Type type2 = Convert.ToType(this.operand1.InferType(null));
				LocalBuilder localBuilder = il.DeclareLocal(Typeob.Object);
				this.operand1.TranslateToILPreSetPlusGet(il);
				Convert.Emit(this, il, type2, Typeob.Object);
				il.Emit(OpCodes.Stloc, localBuilder);
				il.Emit(OpCodes.Ldloc, (LocalBuilder)this.metaData);
				il.Emit(OpCodes.Ldloc, localBuilder);
				this.operand2.TranslateToIL(il, Typeob.Object);
				il.Emit(OpCodes.Call, CompilerGlobals.evaluateBitwiseBinaryMethod);
				if (rtype != Typeob.Void)
				{
					il.Emit(OpCodes.Dup);
					il.Emit(OpCodes.Stloc, localBuilder);
				}
				Convert.Emit(this, il, Typeob.Object, type2);
				this.operand1.TranslateToILSet(il);
				if (rtype != Typeob.Void)
				{
					il.Emit(OpCodes.Ldloc, localBuilder);
					Convert.Emit(this, il, Typeob.Object, rtype);
				}
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000E414 File Offset: 0x0000D414
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			IReflect reflect = this.InferType(null);
			this.operand1.TranslateToILInitializer(il);
			this.operand2.TranslateToILInitializer(il);
			if (reflect != Typeob.Object)
			{
				return;
			}
			this.metaData = il.DeclareLocal(Typeob.BitwiseBinary);
			ConstantWrapper.TranslateToILInt(il, (int)this.operatorTok);
			il.Emit(OpCodes.Newobj, CompilerGlobals.bitwiseBinaryConstructor);
			il.Emit(OpCodes.Stloc, (LocalBuilder)this.metaData);
		}

		// Token: 0x0400008E RID: 142
		private BitwiseBinary binOp;

		// Token: 0x0400008F RID: 143
		private object metaData;
	}
}
