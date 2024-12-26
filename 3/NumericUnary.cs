using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x020000F0 RID: 240
	public sealed class NumericUnary : UnaryOp
	{
		// Token: 0x06000A8B RID: 2699 RVA: 0x00050B19 File Offset: 0x0004FB19
		internal NumericUnary(Context context, AST operand, JSToken operatorTok)
			: base(context, operand)
		{
			this.operatorTok = operatorTok;
			this.operatorMeth = null;
			this.type = null;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00050B38 File Offset: 0x0004FB38
		public NumericUnary(int operatorTok)
			: this(null, null, (JSToken)operatorTok)
		{
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x00050B43 File Offset: 0x0004FB43
		internal override object Evaluate()
		{
			return this.EvaluateUnary(this.operand.Evaluate());
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00050B58 File Offset: 0x0004FB58
		[DebuggerHidden]
		[DebuggerStepThrough]
		public object EvaluateUnary(object v)
		{
			IConvertible iconvertible = Convert.GetIConvertible(v);
			switch (Convert.GetTypeCode(v, iconvertible))
			{
			case TypeCode.Empty:
				return this.EvaluateUnary(double.NaN);
			case TypeCode.DBNull:
				return this.EvaluateUnary(0);
			case TypeCode.Boolean:
				return this.EvaluateUnary(iconvertible.ToBoolean(null) ? 1 : 0);
			case TypeCode.Char:
				return this.EvaluateUnary((int)iconvertible.ToChar(null));
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			{
				int num = iconvertible.ToInt32(null);
				JSToken jstoken = this.operatorTok;
				switch (jstoken)
				{
				case JSToken.FirstOp:
					return num == 0;
				case JSToken.BitwiseNot:
					return ~num;
				default:
					switch (jstoken)
					{
					case JSToken.FirstBinaryOp:
						return num;
					case JSToken.Minus:
						if (num == 0)
						{
							return -(double)num;
						}
						if (num == -2147483648)
						{
							return (ulong)(-(ulong)((double)num));
						}
						return -num;
					default:
						throw new JScriptException(JSError.InternalError, this.context);
					}
					break;
				}
				break;
			}
			case TypeCode.UInt32:
			{
				uint num2 = iconvertible.ToUInt32(null);
				JSToken jstoken2 = this.operatorTok;
				switch (jstoken2)
				{
				case JSToken.FirstOp:
					return num2 == 0U;
				case JSToken.BitwiseNot:
					return ~num2;
				default:
					switch (jstoken2)
					{
					case JSToken.FirstBinaryOp:
						return num2;
					case JSToken.Minus:
						if (num2 != 0U && num2 <= 2147483647U)
						{
							return (int)(-(int)num2);
						}
						return -num2;
					default:
						throw new JScriptException(JSError.InternalError, this.context);
					}
					break;
				}
				break;
			}
			case TypeCode.Int64:
			{
				long num3 = iconvertible.ToInt64(null);
				JSToken jstoken3 = this.operatorTok;
				switch (jstoken3)
				{
				case JSToken.FirstOp:
					return num3 == 0L;
				case JSToken.BitwiseNot:
					return ~num3;
				default:
					switch (jstoken3)
					{
					case JSToken.FirstBinaryOp:
						return num3;
					case JSToken.Minus:
						if (num3 == 0L || num3 == -9223372036854775808L)
						{
							return -(double)num3;
						}
						return -num3;
					default:
						throw new JScriptException(JSError.InternalError, this.context);
					}
					break;
				}
				break;
			}
			case TypeCode.UInt64:
			{
				ulong num4 = iconvertible.ToUInt64(null);
				JSToken jstoken4 = this.operatorTok;
				switch (jstoken4)
				{
				case JSToken.FirstOp:
					return num4 == 0UL;
				case JSToken.BitwiseNot:
					return ~num4;
				default:
					switch (jstoken4)
					{
					case JSToken.FirstBinaryOp:
						return num4;
					case JSToken.Minus:
						if (num4 != 0UL && num4 <= 9223372036854775807UL)
						{
							return (long)(-(long)num4);
						}
						return -num4;
					default:
						throw new JScriptException(JSError.InternalError, this.context);
					}
					break;
				}
				break;
			}
			case TypeCode.Single:
			case TypeCode.Double:
			{
				double num5 = iconvertible.ToDouble(null);
				JSToken jstoken5 = this.operatorTok;
				switch (jstoken5)
				{
				case JSToken.FirstOp:
					return !Convert.ToBoolean(num5);
				case JSToken.BitwiseNot:
					return ~(int)Runtime.DoubleToInt64(num5);
				default:
					switch (jstoken5)
					{
					case JSToken.FirstBinaryOp:
						return num5;
					case JSToken.Minus:
						return -num5;
					default:
						throw new JScriptException(JSError.InternalError, this.context);
					}
					break;
				}
				break;
			}
			case TypeCode.String:
				goto IL_0362;
			}
			MethodInfo @operator = this.GetOperator(v.GetType());
			if (@operator != null)
			{
				return @operator.Invoke(null, BindingFlags.Default, JSBinder.ob, new object[] { v }, null);
			}
			IL_0362:
			JSToken jstoken6 = this.operatorTok;
			switch (jstoken6)
			{
			case JSToken.FirstOp:
				return !Convert.ToBoolean(v, iconvertible);
			case JSToken.BitwiseNot:
				return ~Convert.ToInt32(v, iconvertible);
			default:
				switch (jstoken6)
				{
				case JSToken.FirstBinaryOp:
					return Convert.ToNumber(v, iconvertible);
				case JSToken.Minus:
					return -Convert.ToNumber(v, iconvertible);
				default:
					throw new JScriptException(JSError.InternalError, this.context);
				}
				break;
			}
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00050F3C File Offset: 0x0004FF3C
		private MethodInfo GetOperator(IReflect ir)
		{
			Type type = ((ir is Type) ? ((Type)ir) : Typeob.Object);
			if (this.type == type)
			{
				return this.operatorMeth;
			}
			this.type = type;
			if (Convert.IsPrimitiveNumericType(type) || Typeob.JSObject.IsAssignableFrom(type))
			{
				this.operatorMeth = null;
				return null;
			}
			JSToken jstoken = this.operatorTok;
			switch (jstoken)
			{
			case JSToken.FirstOp:
				this.operatorMeth = type.GetMethod("op_LogicalNot", BindingFlags.Static | BindingFlags.Public, JSBinder.ob, new Type[] { type }, null);
				break;
			case JSToken.BitwiseNot:
				this.operatorMeth = type.GetMethod("op_OnesComplement", BindingFlags.Static | BindingFlags.Public, JSBinder.ob, new Type[] { type }, null);
				break;
			default:
				switch (jstoken)
				{
				case JSToken.FirstBinaryOp:
					this.operatorMeth = type.GetMethod("op_UnaryPlus", BindingFlags.Static | BindingFlags.Public, JSBinder.ob, new Type[] { type }, null);
					break;
				case JSToken.Minus:
					this.operatorMeth = type.GetMethod("op_UnaryNegation", BindingFlags.Static | BindingFlags.Public, JSBinder.ob, new Type[] { type }, null);
					break;
				default:
					throw new JScriptException(JSError.InternalError, this.context);
				}
				break;
			}
			if (this.operatorMeth == null || (this.operatorMeth.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope || this.operatorMeth.GetParameters().Length != 1)
			{
				this.operatorMeth = null;
			}
			if (this.operatorMeth != null)
			{
				this.operatorMeth = new JSMethodInfo(this.operatorMeth);
			}
			return this.operatorMeth;
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x000510C8 File Offset: 0x000500C8
		internal override IReflect InferType(JSField inference_target)
		{
			MethodInfo methodInfo;
			if (this.type == null || inference_target != null)
			{
				methodInfo = this.GetOperator(this.operand.InferType(inference_target));
			}
			else
			{
				methodInfo = this.GetOperator(this.type);
			}
			if (methodInfo != null)
			{
				this.metaData = methodInfo;
				return methodInfo.ReturnType;
			}
			if (this.operatorTok == JSToken.FirstOp)
			{
				return Typeob.Boolean;
			}
			switch (Type.GetTypeCode(this.type))
			{
			case TypeCode.Empty:
				if (this.operatorTok != JSToken.BitwiseNot)
				{
					return Typeob.Double;
				}
				return Typeob.Int32;
			case TypeCode.Object:
				return Typeob.Object;
			case TypeCode.DBNull:
				return Typeob.Int32;
			case TypeCode.Boolean:
				return Typeob.Int32;
			case TypeCode.Char:
				return Typeob.Int32;
			case TypeCode.SByte:
				if (this.operatorTok != JSToken.BitwiseNot)
				{
					return Typeob.Int32;
				}
				return Typeob.SByte;
			case TypeCode.Byte:
				if (this.operatorTok != JSToken.BitwiseNot)
				{
					return Typeob.Int32;
				}
				return Typeob.Byte;
			case TypeCode.Int16:
				if (this.operatorTok != JSToken.BitwiseNot)
				{
					return Typeob.Int32;
				}
				return Typeob.Int16;
			case TypeCode.UInt16:
				if (this.operatorTok != JSToken.BitwiseNot)
				{
					return Typeob.Int32;
				}
				return Typeob.UInt16;
			case TypeCode.Int32:
				return Typeob.Int32;
			case TypeCode.UInt32:
				if (this.operatorTok != JSToken.Minus)
				{
					return Typeob.UInt32;
				}
				return Typeob.Double;
			case TypeCode.Int64:
				return Typeob.Int64;
			case TypeCode.UInt64:
				if (this.operatorTok != JSToken.Minus)
				{
					return Typeob.UInt64;
				}
				return Typeob.Double;
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.String:
				if (this.operatorTok != JSToken.BitwiseNot)
				{
					return Typeob.Double;
				}
				return Typeob.Int32;
			}
			if (Typeob.JSObject.IsAssignableFrom(this.type))
			{
				return Typeob.Double;
			}
			return Typeob.Object;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x00051277 File Offset: 0x00050277
		internal override void TranslateToConditionalBranch(ILGenerator il, bool branchIfTrue, Label label, bool shortForm)
		{
			if (this.operatorTok == JSToken.FirstOp)
			{
				this.operand.TranslateToConditionalBranch(il, !branchIfTrue, label, shortForm);
				return;
			}
			base.TranslateToConditionalBranch(il, branchIfTrue, label, shortForm);
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x000512A4 File Offset: 0x000502A4
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if (this.metaData == null)
			{
				Type type = ((this.operatorTok == JSToken.FirstOp) ? Typeob.Boolean : Typeob.Double);
				if (Convert.IsPrimitiveNumericType(rtype) && Convert.IsPromotableTo(this.type, rtype))
				{
					type = rtype;
				}
				if (this.operatorTok == JSToken.BitwiseNot && !Convert.IsPrimitiveIntegerType(type))
				{
					type = this.type;
					if (!Convert.IsPrimitiveIntegerType(type))
					{
						type = Typeob.Int32;
					}
				}
				this.operand.TranslateToIL(il, this.type);
				Convert.Emit(this, il, this.type, type, true);
				JSToken jstoken = this.operatorTok;
				switch (jstoken)
				{
				case JSToken.FirstOp:
					Convert.Emit(this, il, type, Typeob.Boolean, true);
					type = Typeob.Boolean;
					il.Emit(OpCodes.Ldc_I4_0);
					il.Emit(OpCodes.Ceq);
					break;
				case JSToken.BitwiseNot:
					il.Emit(OpCodes.Not);
					break;
				default:
					switch (jstoken)
					{
					case JSToken.FirstBinaryOp:
						break;
					case JSToken.Minus:
						il.Emit(OpCodes.Neg);
						break;
					default:
						throw new JScriptException(JSError.InternalError, this.context);
					}
					break;
				}
				Convert.Emit(this, il, type, rtype);
				return;
			}
			if (this.metaData is MethodInfo)
			{
				MethodInfo methodInfo = (MethodInfo)this.metaData;
				ParameterInfo[] parameters = methodInfo.GetParameters();
				this.operand.TranslateToIL(il, parameters[0].ParameterType);
				il.Emit(OpCodes.Call, methodInfo);
				Convert.Emit(this, il, methodInfo.ReturnType, rtype);
				return;
			}
			il.Emit(OpCodes.Ldloc, (LocalBuilder)this.metaData);
			this.operand.TranslateToIL(il, Typeob.Object);
			il.Emit(OpCodes.Call, CompilerGlobals.evaluateUnaryMethod);
			Convert.Emit(this, il, Typeob.Object, rtype);
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x00051450 File Offset: 0x00050450
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			IReflect reflect = this.InferType(null);
			this.operand.TranslateToILInitializer(il);
			if (reflect != Typeob.Object)
			{
				return;
			}
			this.metaData = il.DeclareLocal(Typeob.NumericUnary);
			ConstantWrapper.TranslateToILInt(il, (int)this.operatorTok);
			il.Emit(OpCodes.Newobj, CompilerGlobals.numericUnaryConstructor);
			il.Emit(OpCodes.Stloc, (LocalBuilder)this.metaData);
		}

		// Token: 0x0400067B RID: 1659
		private object metaData;

		// Token: 0x0400067C RID: 1660
		private JSToken operatorTok;

		// Token: 0x0400067D RID: 1661
		private MethodInfo operatorMeth;

		// Token: 0x0400067E RID: 1662
		private Type type;
	}
}
