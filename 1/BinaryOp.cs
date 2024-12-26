using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000028 RID: 40
	public abstract class BinaryOp : AST
	{
		// Token: 0x06000177 RID: 375 RVA: 0x000080A7 File Offset: 0x000070A7
		internal BinaryOp(Context context, AST operand1, AST operand2)
			: this(context, operand1, operand2, JSToken.EndOfFile)
		{
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000080B3 File Offset: 0x000070B3
		internal BinaryOp(Context context, AST operand1, AST operand2, JSToken operatorTok)
			: base(context)
		{
			this.operand1 = operand1;
			this.operand2 = operand2;
			this.operatorTok = operatorTok;
			this.type1 = null;
			this.type2 = null;
			this.operatorMeth = null;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000080E7 File Offset: 0x000070E7
		internal override void CheckIfOKToUseInSuperConstructorCall()
		{
			this.operand1.CheckIfOKToUseInSuperConstructorCall();
			this.operand2.CheckIfOKToUseInSuperConstructorCall();
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00008100 File Offset: 0x00007100
		protected MethodInfo GetOperator(IReflect ir1, IReflect ir2)
		{
			if (ir1 is ClassScope)
			{
				ir1 = ((ClassScope)ir1).GetUnderlyingTypeIfEnum();
			}
			if (ir2 is ClassScope)
			{
				ir2 = ((ClassScope)ir2).GetUnderlyingTypeIfEnum();
			}
			Type type = ((ir1 is Type) ? ((Type)ir1) : Typeob.Object);
			Type type2 = ((ir2 is Type) ? ((Type)ir2) : Typeob.Object);
			if (this.type1 == type && this.type2 == type2)
			{
				return this.operatorMeth;
			}
			this.type1 = type;
			this.type2 = type2;
			this.operatorMeth = null;
			if (type == Typeob.String || Convert.IsPrimitiveNumericType(ir1) || Typeob.JSObject.IsAssignableFrom(type))
			{
				type = null;
			}
			if (type2 == Typeob.String || Convert.IsPrimitiveNumericType(ir2) || Typeob.JSObject.IsAssignableFrom(type2))
			{
				type2 = null;
			}
			if (type == null && type2 == null)
			{
				return null;
			}
			string text = "op_NoSuchOp";
			switch (this.operatorTok)
			{
			case JSToken.FirstBinaryOp:
				text = "op_Addition";
				break;
			case JSToken.Minus:
				text = "op_Subtraction";
				break;
			case JSToken.BitwiseOr:
				text = "op_BitwiseOr";
				break;
			case JSToken.BitwiseXor:
				text = "op_ExclusiveOr";
				break;
			case JSToken.BitwiseAnd:
				text = "op_BitwiseAnd";
				break;
			case JSToken.Equal:
				text = "op_Equality";
				break;
			case JSToken.NotEqual:
				text = "op_Inequality";
				break;
			case JSToken.GreaterThan:
				text = "op_GreaterThan";
				break;
			case JSToken.LessThan:
				text = "op_LessThan";
				break;
			case JSToken.LessThanEqual:
				text = "op_LessThanOrEqual";
				break;
			case JSToken.GreaterThanEqual:
				text = "op_GreaterThanOrEqual";
				break;
			case JSToken.LeftShift:
				text = "op_LeftShift";
				break;
			case JSToken.RightShift:
				text = "op_RightShift";
				break;
			case JSToken.Multiply:
				text = "op_Multiply";
				break;
			case JSToken.Divide:
				text = "op_Division";
				break;
			case JSToken.Modulo:
				text = "op_Modulus";
				break;
			}
			Type[] array = new Type[] { this.type1, this.type2 };
			if (type == type2)
			{
				MethodInfo method = type.GetMethod(text, BindingFlags.Static | BindingFlags.Public, JSBinder.ob, array, null);
				if (method != null && (method.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope && method.GetParameters().Length == 2)
				{
					this.operatorMeth = method;
				}
			}
			else
			{
				MethodInfo methodInfo = ((type == null) ? null : type.GetMethod(text, BindingFlags.Static | BindingFlags.Public, JSBinder.ob, array, null));
				MethodInfo methodInfo2 = ((type2 == null) ? null : type2.GetMethod(text, BindingFlags.Static | BindingFlags.Public, JSBinder.ob, array, null));
				this.operatorMeth = JSBinder.SelectOperator(methodInfo, methodInfo2, this.type1, this.type2);
			}
			if (this.operatorMeth != null)
			{
				this.operatorMeth = new JSMethodInfo(this.operatorMeth);
			}
			return this.operatorMeth;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000838C File Offset: 0x0000738C
		internal override AST PartiallyEvaluate()
		{
			this.operand1 = this.operand1.PartiallyEvaluate();
			this.operand2 = this.operand2.PartiallyEvaluate();
			try
			{
				if (this.operand1 is ConstantWrapper)
				{
					if (this.operand2 is ConstantWrapper)
					{
						return new ConstantWrapper(this.Evaluate(), this.context);
					}
					object value = ((ConstantWrapper)this.operand1).value;
					if (value is string && ((string)value).Length == 1 && this.operand2.InferType(null) == Typeob.Char)
					{
						((ConstantWrapper)this.operand1).value = ((string)value)[0];
					}
				}
				else if (this.operand2 is ConstantWrapper)
				{
					object value2 = ((ConstantWrapper)this.operand2).value;
					if (value2 is string && ((string)value2).Length == 1 && this.operand1.InferType(null) == Typeob.Char)
					{
						((ConstantWrapper)this.operand2).value = ((string)value2)[0];
					}
				}
			}
			catch (JScriptException ex)
			{
				this.context.HandleError((JSError)(ex.ErrorNumber & 65535));
			}
			catch
			{
				this.context.HandleError(JSError.TypeMismatch);
			}
			return this;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00008520 File Offset: 0x00007520
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.operand1.TranslateToILInitializer(il);
			this.operand2.TranslateToILInitializer(il);
		}

		// Token: 0x04000079 RID: 121
		protected AST operand1;

		// Token: 0x0400007A RID: 122
		protected AST operand2;

		// Token: 0x0400007B RID: 123
		protected JSToken operatorTok;

		// Token: 0x0400007C RID: 124
		protected Type type1;

		// Token: 0x0400007D RID: 125
		protected Type type2;

		// Token: 0x0400007E RID: 126
		protected MethodInfo operatorMeth;
	}
}
