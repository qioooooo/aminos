using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x020000F9 RID: 249
	internal sealed class PlusAssign : BinaryOp
	{
		// Token: 0x06000ACB RID: 2763 RVA: 0x00053044 File Offset: 0x00052044
		internal PlusAssign(Context context, AST operand1, AST operand2)
			: base(context, operand1, operand2, JSToken.FirstBinaryOp)
		{
			this.binOp = new Plus(context, operand1, operand2);
			this.metaData = null;
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x00053068 File Offset: 0x00052068
		internal override object Evaluate()
		{
			object obj = this.operand1.Evaluate();
			object obj2 = this.operand2.Evaluate();
			object obj3 = this.binOp.EvaluatePlus(obj, obj2);
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

		// Token: 0x06000ACD RID: 2765 RVA: 0x00053110 File Offset: 0x00052110
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
			if (this.type1 == Typeob.String || this.type2 == Typeob.String)
			{
				return Typeob.String;
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

		// Token: 0x06000ACE RID: 2766 RVA: 0x00053200 File Offset: 0x00052200
		internal override AST PartiallyEvaluate()
		{
			this.operand1 = this.operand1.PartiallyEvaluateAsReference();
			this.operand2 = this.operand2.PartiallyEvaluate();
			this.binOp = new Plus(this.context, this.operand1, this.operand2);
			this.operand1.SetPartialValue(this.binOp);
			if (base.Engine.doFast)
			{
				Binding binding = this.operand1 as Binding;
				if (binding != null && binding.member is JSVariableField)
				{
					TypeExpression type = ((JSVariableField)binding.member).type;
					if (type != null && type.InferType(null) == Typeob.String)
					{
						this.operand1.context.HandleError(JSError.StringConcatIsSlow);
					}
				}
			}
			return this;
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x000532C0 File Offset: 0x000522C0
		private void TranslateToILForNoOverloadCase(ILGenerator il, Type rtype)
		{
			Type type = Convert.ToType(this.operand1.InferType(null));
			Type type2 = Convert.ToType(this.operand2.InferType(null));
			Type type3 = Typeob.Object;
			if (type == Typeob.String || type2 == Typeob.String)
			{
				type3 = Typeob.String;
			}
			else if (rtype == Typeob.Void || rtype == type || (Convert.IsPrimitiveNumericType(type) && (Convert.IsPromotableTo(type2, type) || (this.operand2 is ConstantWrapper && ((ConstantWrapper)this.operand2).IsAssignableTo(type)))))
			{
				type3 = type;
			}
			if (type3 == Typeob.SByte || type3 == Typeob.Int16)
			{
				type3 = Typeob.Int32;
			}
			else if (type3 == Typeob.Byte || type3 == Typeob.UInt16)
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
			if (type3 == Typeob.Object || type3 == Typeob.String)
			{
				il.Emit(OpCodes.Call, CompilerGlobals.plusDoOpMethod);
				type3 = Typeob.Object;
			}
			else if (type3 == Typeob.Double || type3 == Typeob.Single)
			{
				il.Emit(OpCodes.Add);
			}
			else if (type3 == Typeob.Int32 || type3 == Typeob.Int64 || type3 == Typeob.Int16 || type3 == Typeob.SByte)
			{
				il.Emit(OpCodes.Add_Ovf);
			}
			else
			{
				il.Emit(OpCodes.Add_Ovf_Un);
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

		// Token: 0x06000AD0 RID: 2768 RVA: 0x000534D0 File Offset: 0x000524D0
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
				il.Emit(OpCodes.Call, CompilerGlobals.evaluatePlusMethod);
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

		// Token: 0x06000AD1 RID: 2769 RVA: 0x000536B4 File Offset: 0x000526B4
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			IReflect reflect = this.InferType(null);
			this.operand1.TranslateToILInitializer(il);
			this.operand2.TranslateToILInitializer(il);
			if (reflect != Typeob.Object)
			{
				return;
			}
			this.metaData = il.DeclareLocal(Typeob.Plus);
			il.Emit(OpCodes.Newobj, CompilerGlobals.plusConstructor);
			il.Emit(OpCodes.Stloc, (LocalBuilder)this.metaData);
		}

		// Token: 0x0400069D RID: 1693
		private Plus binOp;

		// Token: 0x0400069E RID: 1694
		private object metaData;
	}
}
