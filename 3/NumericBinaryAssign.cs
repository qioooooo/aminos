using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x020000EF RID: 239
	internal class NumericBinaryAssign : BinaryOp
	{
		// Token: 0x06000A84 RID: 2692 RVA: 0x00050341 File Offset: 0x0004F341
		internal NumericBinaryAssign(Context context, AST operand1, AST operand2, JSToken operatorTok)
			: base(context, operand1, operand2, operatorTok)
		{
			this.binOp = new NumericBinary(context, operand1, operand2, operatorTok);
			this.metaData = null;
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00050368 File Offset: 0x0004F368
		internal override object Evaluate()
		{
			object obj = this.operand1.Evaluate();
			object obj2 = this.operand2.Evaluate();
			object obj3 = this.binOp.EvaluateNumericBinary(obj, obj2);
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

		// Token: 0x06000A86 RID: 2694 RVA: 0x00050410 File Offset: 0x0004F410
		internal override IReflect InferType(JSField inference_target)
		{
			MethodInfo methodInfo;
			if (this.type1 == null || inference_target != null)
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
			if (this.type1 == Typeob.Char && this.operatorTok == JSToken.Minus)
			{
				TypeCode typeCode = Type.GetTypeCode(this.type2);
				if (Convert.IsPrimitiveNumericTypeCode(typeCode) || typeCode == TypeCode.Boolean)
				{
					return Typeob.Char;
				}
				if (typeCode == TypeCode.Char)
				{
					return Typeob.Int32;
				}
			}
			if (Convert.IsPrimitiveNumericType(this.type1))
			{
				if (Convert.IsPromotableTo(this.type2, this.type1) || (this.operand2 is ConstantWrapper && ((ConstantWrapper)this.operand2).IsAssignableTo(this.type1)))
				{
					return this.type1;
				}
				if (Convert.IsPrimitiveNumericType(this.type1) && Convert.IsPrimitiveNumericTypeFitForDouble(this.type2))
				{
					return Typeob.Double;
				}
			}
			return Typeob.Object;
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00050520 File Offset: 0x0004F520
		internal override AST PartiallyEvaluate()
		{
			this.operand1 = this.operand1.PartiallyEvaluateAsReference();
			this.operand2 = this.operand2.PartiallyEvaluate();
			this.binOp = new NumericBinary(this.context, this.operand1, this.operand2, this.operatorTok);
			this.operand1.SetPartialValue(this.binOp);
			return this;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00050584 File Offset: 0x0004F584
		private void TranslateToILForNoOverloadCase(ILGenerator il, Type rtype)
		{
			Type type = Convert.ToType(this.operand1.InferType(null));
			Type type2 = Convert.ToType(this.operand2.InferType(null));
			Type type3 = Typeob.Double;
			if (this.operatorTok != JSToken.Divide && (rtype == Typeob.Void || rtype == type || Convert.IsPrimitiveNumericType(type)) && (Convert.IsPromotableTo(type2, type) || (this.operand2 is ConstantWrapper && ((ConstantWrapper)this.operand2).IsAssignableTo(type))))
			{
				type3 = type;
			}
			if (type3 == Typeob.SByte || type3 == Typeob.Int16)
			{
				type3 = Typeob.Int32;
			}
			else if (type3 == Typeob.Byte || type3 == Typeob.UInt16 || type3 == Typeob.Char)
			{
				type3 = Typeob.UInt32;
			}
			if (this.operand2 is ConstantWrapper)
			{
				if (!((ConstantWrapper)this.operand2).IsAssignableTo(type3))
				{
					type3 = Typeob.Object;
				}
			}
			else if ((Convert.IsPrimitiveSignedNumericType(type2) && Convert.IsPrimitiveUnsignedIntegerType(type)) || (Convert.IsPrimitiveUnsignedIntegerType(type2) && Convert.IsPrimitiveSignedIntegerType(type)))
			{
				type3 = Typeob.Object;
			}
			this.operand1.TranslateToILPreSetPlusGet(il);
			Convert.Emit(this, il, type, type3);
			this.operand2.TranslateToIL(il, type3);
			if (type3 == Typeob.Object)
			{
				il.Emit(OpCodes.Ldc_I4, (int)this.operatorTok);
				il.Emit(OpCodes.Call, CompilerGlobals.numericbinaryDoOpMethod);
			}
			else if (type3 == Typeob.Double || type3 == Typeob.Single)
			{
				JSToken operatorTok = this.operatorTok;
				if (operatorTok != JSToken.Minus)
				{
					switch (operatorTok)
					{
					case JSToken.Multiply:
						il.Emit(OpCodes.Mul);
						break;
					case JSToken.Divide:
						il.Emit(OpCodes.Div);
						break;
					case JSToken.Modulo:
						il.Emit(OpCodes.Rem);
						break;
					default:
						throw new JScriptException(JSError.InternalError, this.context);
					}
				}
				else
				{
					il.Emit(OpCodes.Sub);
				}
			}
			else if (type3 == Typeob.Int32 || type3 == Typeob.Int64 || type3 == Typeob.Int16 || type3 == Typeob.SByte)
			{
				JSToken operatorTok2 = this.operatorTok;
				if (operatorTok2 != JSToken.Minus)
				{
					switch (operatorTok2)
					{
					case JSToken.Multiply:
						il.Emit(OpCodes.Mul_Ovf);
						break;
					case JSToken.Divide:
						il.Emit(OpCodes.Div);
						break;
					case JSToken.Modulo:
						il.Emit(OpCodes.Rem);
						break;
					default:
						throw new JScriptException(JSError.InternalError, this.context);
					}
				}
				else
				{
					il.Emit(OpCodes.Sub_Ovf);
				}
			}
			else
			{
				JSToken operatorTok3 = this.operatorTok;
				if (operatorTok3 != JSToken.Minus)
				{
					switch (operatorTok3)
					{
					case JSToken.Multiply:
						il.Emit(OpCodes.Mul_Ovf_Un);
						break;
					case JSToken.Divide:
						il.Emit(OpCodes.Div);
						break;
					case JSToken.Modulo:
						il.Emit(OpCodes.Rem);
						break;
					default:
						throw new JScriptException(JSError.InternalError, this.context);
					}
				}
				else
				{
					il.Emit(OpCodes.Sub_Ovf_Un);
				}
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

		// Token: 0x06000A89 RID: 2697 RVA: 0x000508BC File Offset: 0x0004F8BC
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
				il.Emit(OpCodes.Call, CompilerGlobals.evaluateNumericBinaryMethod);
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

		// Token: 0x06000A8A RID: 2698 RVA: 0x00050AA0 File Offset: 0x0004FAA0
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			IReflect reflect = this.InferType(null);
			this.operand1.TranslateToILInitializer(il);
			this.operand2.TranslateToILInitializer(il);
			if (reflect != Typeob.Object)
			{
				return;
			}
			this.metaData = il.DeclareLocal(Typeob.NumericBinary);
			ConstantWrapper.TranslateToILInt(il, (int)this.operatorTok);
			il.Emit(OpCodes.Newobj, CompilerGlobals.numericBinaryConstructor);
			il.Emit(OpCodes.Stloc, (LocalBuilder)this.metaData);
		}

		// Token: 0x04000679 RID: 1657
		private NumericBinary binOp;

		// Token: 0x0400067A RID: 1658
		private object metaData;
	}
}
