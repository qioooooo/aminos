using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x020000EE RID: 238
	public sealed class NumericBinary : BinaryOp
	{
		// Token: 0x06000A75 RID: 2677 RVA: 0x0004F416 File Offset: 0x0004E416
		internal NumericBinary(Context context, AST operand1, AST operand2, JSToken operatorTok)
			: base(context, operand1, operand2, operatorTok)
		{
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0004F423 File Offset: 0x0004E423
		public NumericBinary(int operatorTok)
			: base(null, null, null, (JSToken)operatorTok)
		{
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0004F42F File Offset: 0x0004E42F
		internal override object Evaluate()
		{
			return this.EvaluateNumericBinary(this.operand1.Evaluate(), this.operand2.Evaluate());
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0004F450 File Offset: 0x0004E450
		[DebuggerHidden]
		[DebuggerStepThrough]
		public object EvaluateNumericBinary(object v1, object v2)
		{
			if (v1 is int && v2 is int)
			{
				return NumericBinary.DoOp((int)v1, (int)v2, this.operatorTok);
			}
			if (v1 is double && v2 is double)
			{
				return NumericBinary.DoOp((double)v1, (double)v2, this.operatorTok);
			}
			return this.EvaluateNumericBinary(v1, v2, this.operatorTok);
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0004F4BC File Offset: 0x0004E4BC
		[DebuggerStepThrough]
		[DebuggerHidden]
		private object EvaluateNumericBinary(object v1, object v2, JSToken operatorTok)
		{
			IConvertible iconvertible = Convert.GetIConvertible(v1);
			IConvertible iconvertible2 = Convert.GetIConvertible(v2);
			TypeCode typeCode = Convert.GetTypeCode(v1, iconvertible);
			TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
			switch (typeCode)
			{
			case TypeCode.Empty:
				return double.NaN;
			case TypeCode.DBNull:
				return this.EvaluateNumericBinary(0, v2, operatorTok);
			case TypeCode.Boolean:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			{
				int num = iconvertible.ToInt32(null);
				switch (typeCode2)
				{
				case TypeCode.Empty:
					return double.NaN;
				case TypeCode.DBNull:
					return NumericBinary.DoOp(num, 0, operatorTok);
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
					return NumericBinary.DoOp(num, iconvertible2.ToInt32(null), operatorTok);
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return NumericBinary.DoOp((long)num, iconvertible2.ToInt64(null), operatorTok);
				case TypeCode.UInt64:
					if (num >= 0)
					{
						return NumericBinary.DoOp((ulong)((long)num), iconvertible2.ToUInt64(null), operatorTok);
					}
					return NumericBinary.DoOp((double)num, iconvertible2.ToDouble(null), operatorTok);
				case TypeCode.Single:
				case TypeCode.Double:
					return NumericBinary.DoOp((double)num, iconvertible2.ToDouble(null), operatorTok);
				}
				break;
			}
			case TypeCode.Char:
			{
				int num2 = iconvertible.ToInt32(null);
				object obj;
				switch (typeCode2)
				{
				case TypeCode.Empty:
					return double.NaN;
				case TypeCode.DBNull:
					return NumericBinary.DoOp(num2, 0, operatorTok);
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
					obj = NumericBinary.DoOp(num2, iconvertible2.ToInt32(null), operatorTok);
					goto IL_017F;
				case TypeCode.UInt32:
				case TypeCode.Int64:
					obj = NumericBinary.DoOp((long)num2, iconvertible2.ToInt64(null), operatorTok);
					goto IL_017F;
				case TypeCode.UInt64:
					obj = NumericBinary.DoOp((double)num2, iconvertible2.ToDouble(null), operatorTok);
					goto IL_017F;
				case TypeCode.Single:
				case TypeCode.Double:
					obj = NumericBinary.DoOp((double)iconvertible.ToInt32(null), iconvertible2.ToDouble(null), operatorTok);
					goto IL_017F;
				case TypeCode.String:
					obj = NumericBinary.DoOp((double)num2, Convert.ToNumber(v2, iconvertible2), operatorTok);
					goto IL_017F;
				}
				obj = null;
				IL_017F:
				if (this.operatorTok == JSToken.Minus && obj != null && typeCode2 != TypeCode.Char)
				{
					return Convert.Coerce2(obj, TypeCode.Char, false);
				}
				if (obj != null)
				{
					return obj;
				}
				break;
			}
			case TypeCode.UInt32:
			{
				uint num3 = iconvertible.ToUInt32(null);
				switch (typeCode2)
				{
				case TypeCode.Empty:
					return double.NaN;
				case TypeCode.DBNull:
					return NumericBinary.DoOp(num3, 0U, operatorTok);
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
					return NumericBinary.DoOp(num3, iconvertible2.ToUInt32(null), operatorTok);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.Int32:
				{
					int num4 = iconvertible2.ToInt32(null);
					if (num4 >= 0)
					{
						return NumericBinary.DoOp(num3, (uint)num4, operatorTok);
					}
					return NumericBinary.DoOp((long)((ulong)num3), (long)num4, operatorTok);
				}
				case TypeCode.Int64:
					return NumericBinary.DoOp((long)((ulong)num3), iconvertible2.ToInt64(null), operatorTok);
				case TypeCode.UInt64:
					return NumericBinary.DoOp((ulong)num3, iconvertible2.ToUInt64(null), operatorTok);
				case TypeCode.Single:
				case TypeCode.Double:
					return NumericBinary.DoOp(num3, iconvertible2.ToDouble(null), operatorTok);
				}
				break;
			}
			case TypeCode.Int64:
			{
				long num5 = iconvertible.ToInt64(null);
				switch (typeCode2)
				{
				case TypeCode.Empty:
					return double.NaN;
				case TypeCode.DBNull:
					return NumericBinary.DoOp(num5, 0L, operatorTok);
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return NumericBinary.DoOp(num5, iconvertible2.ToInt64(null), operatorTok);
				case TypeCode.UInt64:
					if (num5 >= 0L)
					{
						return NumericBinary.DoOp((ulong)num5, iconvertible2.ToUInt64(null), operatorTok);
					}
					return NumericBinary.DoOp((double)num5, iconvertible2.ToDouble(null), operatorTok);
				case TypeCode.Single:
				case TypeCode.Double:
					return NumericBinary.DoOp((double)num5, iconvertible2.ToDouble(null), operatorTok);
				}
				break;
			}
			case TypeCode.UInt64:
			{
				ulong num6 = iconvertible.ToUInt64(null);
				switch (typeCode2)
				{
				case TypeCode.Empty:
					return double.NaN;
				case TypeCode.DBNull:
					return NumericBinary.DoOp(num6, 0UL, operatorTok);
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
					return NumericBinary.DoOp(num6, iconvertible2.ToUInt64(null), operatorTok);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				{
					long num7 = iconvertible2.ToInt64(null);
					if (num7 >= 0L)
					{
						return NumericBinary.DoOp(num6, (ulong)num7, operatorTok);
					}
					return NumericBinary.DoOp(num6, (double)num7, operatorTok);
				}
				case TypeCode.Single:
				case TypeCode.Double:
					return NumericBinary.DoOp(num6, iconvertible2.ToDouble(null), operatorTok);
				}
				break;
			}
			case TypeCode.Single:
			case TypeCode.Double:
			{
				double num8 = iconvertible.ToDouble(null);
				switch (typeCode2)
				{
				case TypeCode.Empty:
					return double.NaN;
				case TypeCode.DBNull:
					return NumericBinary.DoOp(num8, 0.0, operatorTok);
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
					return NumericBinary.DoOp(num8, (double)iconvertible2.ToInt32(null), operatorTok);
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
					return NumericBinary.DoOp(num8, iconvertible2.ToDouble(null), operatorTok);
				}
				break;
			}
			}
			if (v2 == null)
			{
				return double.NaN;
			}
			MethodInfo @operator = base.GetOperator(v1.GetType(), v2.GetType());
			if (@operator != null)
			{
				return @operator.Invoke(null, BindingFlags.Default, JSBinder.ob, new object[] { v1, v2 }, null);
			}
			return NumericBinary.DoOp(v1, v2, iconvertible, iconvertible2, operatorTok);
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0004FAB4 File Offset: 0x0004EAB4
		public static object DoOp(object v1, object v2, JSToken operatorTok)
		{
			return NumericBinary.DoOp(v1, v2, Convert.GetIConvertible(v1), Convert.GetIConvertible(v2), operatorTok);
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0004FACC File Offset: 0x0004EACC
		private static object DoOp(object v1, object v2, IConvertible ic1, IConvertible ic2, JSToken operatorTok)
		{
			if (operatorTok == JSToken.Minus)
			{
				IConvertible convertible = ic1;
				object obj = Convert.ToPrimitive(v1, PreferredType.Either, ref convertible);
				TypeCode typeCode = Convert.GetTypeCode(obj, convertible);
				if (typeCode == TypeCode.Char)
				{
					IConvertible convertible2 = ic2;
					object obj2 = Convert.ToPrimitive(v2, PreferredType.Either, ref convertible2);
					TypeCode typeCode2 = Convert.GetTypeCode(obj2, convertible2);
					if (typeCode2 == TypeCode.String)
					{
						string text = convertible2.ToString(null);
						if (text.Length == 1)
						{
							typeCode2 = TypeCode.Char;
							obj2 = text[0];
							convertible2 = Convert.GetIConvertible(obj2);
						}
					}
					object obj3 = NumericBinary.DoOp(Convert.ToNumber(obj, convertible), Convert.ToNumber(obj2, convertible2), operatorTok);
					if (typeCode2 != TypeCode.Char)
					{
						obj3 = Convert.Coerce2(obj3, TypeCode.Char, false);
					}
					return obj3;
				}
			}
			return NumericBinary.DoOp(Convert.ToNumber(v1, ic1), Convert.ToNumber(v2, ic2), operatorTok);
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0004FB84 File Offset: 0x0004EB84
		private static object DoOp(int x, int y, JSToken operatorTok)
		{
			if (operatorTok != JSToken.Minus)
			{
				switch (operatorTok)
				{
				case JSToken.Multiply:
					if (x == 0 || y == 0)
					{
						return (double)x * (double)y;
					}
					try
					{
						return checked(x * y);
					}
					catch (OverflowException)
					{
						return (double)x * (double)y;
					}
					break;
				case JSToken.Divide:
					return (double)x / (double)y;
				case JSToken.Modulo:
					if (x <= 0 || y <= 0)
					{
						return (double)x % (double)y;
					}
					return x % y;
				}
				throw new JScriptException(JSError.InternalError);
			}
			int num = x - y;
			if (num < x == y > 0)
			{
				return num;
			}
			return (double)x - (double)y;
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0004FC3C File Offset: 0x0004EC3C
		private static object DoOp(uint x, uint y, JSToken operatorTok)
		{
			if (operatorTok != JSToken.Minus)
			{
				switch (operatorTok)
				{
				case JSToken.Multiply:
					try
					{
						return checked(x * y);
					}
					catch (OverflowException)
					{
						return x * y;
					}
					break;
				case JSToken.Divide:
					return x / y;
				case JSToken.Modulo:
					if (y == 0U)
					{
						return double.NaN;
					}
					return x % y;
				}
				throw new JScriptException(JSError.InternalError);
			}
			uint num = x - y;
			if (num <= x)
			{
				return num;
			}
			return x - y;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0004FCE0 File Offset: 0x0004ECE0
		private static object DoOp(long x, long y, JSToken operatorTok)
		{
			if (operatorTok != JSToken.Minus)
			{
				switch (operatorTok)
				{
				case JSToken.Multiply:
					if (x == 0L || y == 0L)
					{
						return (double)x * (double)y;
					}
					try
					{
						return checked(x * y);
					}
					catch (OverflowException)
					{
						return (double)x * (double)y;
					}
					break;
				case JSToken.Divide:
					return (double)x / (double)y;
				case JSToken.Modulo:
				{
					if (y == 0L)
					{
						return double.NaN;
					}
					long num = x % y;
					if (num != 0L)
					{
						return num;
					}
					if (x < 0L)
					{
						if (y < 0L)
						{
							return 0;
						}
						return -0.0;
					}
					else
					{
						if (y < 0L)
						{
							return -0.0;
						}
						return 0;
					}
					break;
				}
				}
				throw new JScriptException(JSError.InternalError);
			}
			long num2 = x - y;
			if (num2 < x == y > 0L)
			{
				return num2;
			}
			return (double)x - (double)y;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0004FDE0 File Offset: 0x0004EDE0
		private static object DoOp(ulong x, ulong y, JSToken operatorTok)
		{
			if (operatorTok != JSToken.Minus)
			{
				switch (operatorTok)
				{
				case JSToken.Multiply:
					try
					{
						return checked(x * y);
					}
					catch (OverflowException)
					{
						return x * y;
					}
					break;
				case JSToken.Divide:
					return x / y;
				case JSToken.Modulo:
					if (y == 0UL)
					{
						return double.NaN;
					}
					return x % y;
				}
				throw new JScriptException(JSError.InternalError);
			}
			ulong num = x - y;
			if (num <= x)
			{
				return num;
			}
			return x - y;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x0004FE88 File Offset: 0x0004EE88
		private static object DoOp(double x, double y, JSToken operatorTok)
		{
			if (operatorTok == JSToken.Minus)
			{
				return x - y;
			}
			switch (operatorTok)
			{
			case JSToken.Multiply:
				return x * y;
			case JSToken.Divide:
				return x / y;
			case JSToken.Modulo:
				return x % y;
			default:
				throw new JScriptException(JSError.InternalError);
			}
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0004FEE0 File Offset: 0x0004EEE0
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
			if ((Convert.IsPrimitiveNumericTypeFitForDouble(this.type1) || Typeob.JSObject.IsAssignableFrom(this.type1)) && (Convert.IsPrimitiveNumericTypeFitForDouble(this.type2) || Typeob.JSObject.IsAssignableFrom(this.type2)))
			{
				return Typeob.Double;
			}
			return Typeob.Object;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0004FFC8 File Offset: 0x0004EFC8
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if (this.metaData == null)
			{
				Type type = Typeob.Double;
				if (Convert.IsPrimitiveNumericType(rtype) && Convert.IsPromotableTo(this.type1, rtype) && Convert.IsPromotableTo(this.type2, rtype))
				{
					type = rtype;
				}
				if (this.operatorTok == JSToken.Divide)
				{
					type = Typeob.Double;
				}
				else if (type == Typeob.SByte || type == Typeob.Int16)
				{
					type = Typeob.Int32;
				}
				else if (type == Typeob.Byte || type == Typeob.UInt16 || type == Typeob.Char)
				{
					type = Typeob.UInt32;
				}
				this.operand1.TranslateToIL(il, type);
				this.operand2.TranslateToIL(il, type);
				if (type == Typeob.Double || type == Typeob.Single)
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
				else if (type == Typeob.Int32 || type == Typeob.Int64)
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
				if (Convert.ToType(this.InferType(null)) == Typeob.Char)
				{
					Convert.Emit(this, il, type, Typeob.Char);
					Convert.Emit(this, il, Typeob.Char, rtype);
					return;
				}
				Convert.Emit(this, il, type, rtype);
				return;
			}
			else
			{
				if (this.metaData is MethodInfo)
				{
					MethodInfo methodInfo = (MethodInfo)this.metaData;
					ParameterInfo[] parameters = methodInfo.GetParameters();
					this.operand1.TranslateToIL(il, parameters[0].ParameterType);
					this.operand2.TranslateToIL(il, parameters[1].ParameterType);
					il.Emit(OpCodes.Call, methodInfo);
					Convert.Emit(this, il, methodInfo.ReturnType, rtype);
					return;
				}
				il.Emit(OpCodes.Ldloc, (LocalBuilder)this.metaData);
				this.operand1.TranslateToIL(il, Typeob.Object);
				this.operand2.TranslateToIL(il, Typeob.Object);
				il.Emit(OpCodes.Call, CompilerGlobals.evaluateNumericBinaryMethod);
				Convert.Emit(this, il, Typeob.Object, rtype);
				return;
			}
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x000502C8 File Offset: 0x0004F2C8
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

		// Token: 0x04000678 RID: 1656
		private object metaData;
	}
}
