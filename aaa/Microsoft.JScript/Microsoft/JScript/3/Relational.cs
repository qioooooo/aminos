using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200010B RID: 267
	public class Relational : BinaryOp
	{
		// Token: 0x06000B42 RID: 2882 RVA: 0x00055BA7 File Offset: 0x00054BA7
		internal Relational(Context context, AST operand1, AST operand2, JSToken operatorTok)
			: base(context, operand1, operand2, operatorTok)
		{
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00055BB4 File Offset: 0x00054BB4
		public Relational(int operatorTok)
			: base(null, null, null, (JSToken)operatorTok)
		{
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00055BC0 File Offset: 0x00054BC0
		internal override object Evaluate()
		{
			object obj = this.operand1.Evaluate();
			object obj2 = this.operand2.Evaluate();
			double num = this.EvaluateRelational(obj, obj2);
			switch (this.operatorTok)
			{
			case JSToken.GreaterThan:
				return num > 0.0;
			case JSToken.LessThan:
				return num < 0.0;
			case JSToken.LessThanEqual:
				return num <= 0.0;
			case JSToken.GreaterThanEqual:
				return num >= 0.0;
			default:
				throw new JScriptException(JSError.InternalError, this.context);
			}
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00055C6C File Offset: 0x00054C6C
		[DebuggerHidden]
		[DebuggerStepThrough]
		public double EvaluateRelational(object v1, object v2)
		{
			if (v1 is int)
			{
				if (v2 is int)
				{
					return (double)((int)v1) - (double)((int)v2);
				}
				if (v2 is double)
				{
					return (double)((int)v1) - (double)v2;
				}
			}
			else if (v1 is double)
			{
				if (v2 is double)
				{
					double num = (double)v1;
					double num2 = (double)v2;
					if (num == num2)
					{
						return 0.0;
					}
					return num - num2;
				}
				else if (v2 is int)
				{
					return (double)v1 - (double)((int)v2);
				}
			}
			IConvertible iconvertible = Convert.GetIConvertible(v1);
			IConvertible iconvertible2 = Convert.GetIConvertible(v2);
			TypeCode typeCode = Convert.GetTypeCode(v1, iconvertible);
			TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
			if (typeCode == TypeCode.Object && typeCode2 == TypeCode.Object)
			{
				MethodInfo @operator = base.GetOperator(v1.GetType(), v2.GetType());
				if (@operator != null)
				{
					bool flag = Convert.ToBoolean(@operator.Invoke(null, BindingFlags.Default, JSBinder.ob, new object[] { v1, v2 }, null));
					switch (this.operatorTok)
					{
					case JSToken.GreaterThan:
					case JSToken.GreaterThanEqual:
						return (double)(flag ? 1 : (-1));
					case JSToken.LessThan:
					case JSToken.LessThanEqual:
						return (double)(flag ? (-1) : 1);
					default:
						throw new JScriptException(JSError.InternalError, this.context);
					}
				}
			}
			return Relational.JScriptCompare2(v1, v2, iconvertible, iconvertible2, typeCode, typeCode2);
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00055DBC File Offset: 0x00054DBC
		internal override IReflect InferType(JSField inference_target)
		{
			return Typeob.Boolean;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00055DC4 File Offset: 0x00054DC4
		public static double JScriptCompare(object v1, object v2)
		{
			if (v1 is int)
			{
				if (v2 is int)
				{
					return (double)((int)v1 - (int)v2);
				}
				if (v2 is double)
				{
					return (double)((int)v1) - (double)v2;
				}
			}
			else if (v1 is double)
			{
				if (v2 is double)
				{
					double num = (double)v1;
					double num2 = (double)v2;
					if (num == num2)
					{
						return 0.0;
					}
					return num - num2;
				}
				else if (v2 is int)
				{
					return (double)v1 - (double)((int)v2);
				}
			}
			IConvertible iconvertible = Convert.GetIConvertible(v1);
			IConvertible iconvertible2 = Convert.GetIConvertible(v2);
			TypeCode typeCode = Convert.GetTypeCode(v1, iconvertible);
			TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
			return Relational.JScriptCompare2(v1, v2, iconvertible, iconvertible2, typeCode, typeCode2);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00055E7C File Offset: 0x00054E7C
		private static double JScriptCompare2(object v1, object v2, IConvertible ic1, IConvertible ic2, TypeCode t1, TypeCode t2)
		{
			if (t1 == TypeCode.Object)
			{
				v1 = Convert.ToPrimitive(v1, PreferredType.Number, ref ic1);
				t1 = Convert.GetTypeCode(v1, ic1);
			}
			if (t2 == TypeCode.Object)
			{
				v2 = Convert.ToPrimitive(v2, PreferredType.Number, ref ic2);
				t2 = Convert.GetTypeCode(v2, ic2);
			}
			switch (t1)
			{
			case TypeCode.Char:
				if (t2 == TypeCode.String)
				{
					return (double)string.CompareOrdinal(Convert.ToString(v1, ic1), ic2.ToString(null));
				}
				break;
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
				break;
			case TypeCode.UInt64:
			{
				ulong num = ic1.ToUInt64(null);
				switch (t2)
				{
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				{
					long num2 = ic2.ToInt64(null);
					if (num2 < 0L)
					{
						return 1.0;
					}
					if (num == (ulong)num2)
					{
						return 0.0;
					}
					return -1.0;
				}
				case TypeCode.UInt64:
				{
					ulong num3 = ic2.ToUInt64(null);
					if (num < num3)
					{
						return -1.0;
					}
					if (num == num3)
					{
						return 0.0;
					}
					return 1.0;
				}
				case TypeCode.Single:
				case TypeCode.Double:
					return num - ic2.ToDouble(null);
				case TypeCode.Decimal:
					return (double)(new decimal(num) - ic2.ToDecimal(null));
				default:
				{
					object obj = Convert.ToNumber(v2, ic2);
					return Relational.JScriptCompare2(v1, obj, ic1, Convert.GetIConvertible(obj), t1, TypeCode.Double);
				}
				}
				break;
			}
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.DateTime:
			case (TypeCode)17:
				goto IL_0355;
			case TypeCode.Decimal:
			{
				decimal num4 = ic1.ToDecimal(null);
				switch (t2)
				{
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return (double)(num4 - new decimal(ic2.ToInt64(null)));
				case TypeCode.UInt64:
					return (double)(num4 - new decimal(ic2.ToUInt64(null)));
				case TypeCode.Single:
				case TypeCode.Double:
					return (double)(num4 - new decimal(ic2.ToDouble(null)));
				case TypeCode.Decimal:
					return (double)(num4 - ic2.ToDecimal(null));
				default:
					return (double)(num4 - new decimal(Convert.ToNumber(v2, ic2)));
				}
				break;
			}
			case TypeCode.String:
			{
				TypeCode typeCode = t2;
				if (typeCode == TypeCode.Char)
				{
					return (double)string.CompareOrdinal(ic1.ToString(null), Convert.ToString(v2, ic2));
				}
				if (typeCode == TypeCode.String)
				{
					return (double)string.CompareOrdinal(ic1.ToString(null), ic2.ToString(null));
				}
				goto IL_0355;
			}
			default:
				goto IL_0355;
			}
			long num5 = ic1.ToInt64(null);
			switch (t2)
			{
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
				return (double)(num5 - ic2.ToInt64(null));
			case TypeCode.UInt64:
			{
				if (num5 < 0L)
				{
					return -1.0;
				}
				ulong num6 = ic2.ToUInt64(null);
				if (num5 < (long)num6)
				{
					return -1.0;
				}
				if (num5 == (long)num6)
				{
					return 0.0;
				}
				return 1.0;
			}
			case TypeCode.Single:
			case TypeCode.Double:
				return (double)num5 - ic2.ToDouble(null);
			case TypeCode.Decimal:
				return (double)(new decimal(num5) - ic2.ToDecimal(null));
			default:
			{
				object obj2 = Convert.ToNumber(v2, ic2);
				return Relational.JScriptCompare2(v1, obj2, ic1, Convert.GetIConvertible(obj2), t1, TypeCode.Double);
			}
			}
			IL_0355:
			double num7 = Convert.ToNumber(v1, ic1);
			double num8 = Convert.ToNumber(v2, ic2);
			if (num7 == num8)
			{
				return 0.0;
			}
			return num7 - num8;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00056208 File Offset: 0x00055208
		internal override void TranslateToConditionalBranch(ILGenerator il, bool branchIfTrue, Label label, bool shortForm)
		{
			Type type = this.type1;
			Type type2 = this.type2;
			Type type3 = Typeob.Object;
			if (type.IsPrimitive && type2.IsPrimitive)
			{
				type3 = Typeob.Double;
				if (Convert.IsPromotableTo(type, type2))
				{
					type3 = type2;
				}
				else if (Convert.IsPromotableTo(type2, type))
				{
					type3 = type;
				}
				else if (type == Typeob.Int64 || type == Typeob.UInt64 || type2 == Typeob.Int64 || type2 == Typeob.UInt64)
				{
					type3 = Typeob.Object;
				}
			}
			if (type3 == Typeob.SByte || type3 == Typeob.Int16)
			{
				type3 = Typeob.Int32;
			}
			else if (type3 == Typeob.Byte || type3 == Typeob.UInt16)
			{
				type3 = Typeob.UInt32;
			}
			if (this.metaData == null)
			{
				this.operand1.TranslateToIL(il, type3);
				this.operand2.TranslateToIL(il, type3);
				if (type3 == Typeob.Object)
				{
					il.Emit(OpCodes.Call, CompilerGlobals.jScriptCompareMethod);
					il.Emit(OpCodes.Ldc_I4_0);
					il.Emit(OpCodes.Conv_R8);
					type3 = Typeob.Double;
				}
			}
			else if (this.metaData is MethodInfo)
			{
				MethodInfo methodInfo = (MethodInfo)this.metaData;
				ParameterInfo[] parameters = methodInfo.GetParameters();
				this.operand1.TranslateToIL(il, parameters[0].ParameterType);
				this.operand2.TranslateToIL(il, parameters[1].ParameterType);
				il.Emit(OpCodes.Call, methodInfo);
				if (branchIfTrue)
				{
					il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
					return;
				}
				il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
				return;
			}
			else
			{
				il.Emit(OpCodes.Ldloc, (LocalBuilder)this.metaData);
				this.operand1.TranslateToIL(il, Typeob.Object);
				this.operand2.TranslateToIL(il, Typeob.Object);
				il.Emit(OpCodes.Call, CompilerGlobals.evaluateRelationalMethod);
				il.Emit(OpCodes.Ldc_I4_0);
				il.Emit(OpCodes.Conv_R8);
				type3 = Typeob.Double;
			}
			if (branchIfTrue)
			{
				if (type3 == Typeob.UInt32 || type3 == Typeob.UInt64)
				{
					switch (this.operatorTok)
					{
					case JSToken.GreaterThan:
						il.Emit(shortForm ? OpCodes.Bgt_Un_S : OpCodes.Bgt_Un, label);
						return;
					case JSToken.LessThan:
						il.Emit(shortForm ? OpCodes.Blt_Un_S : OpCodes.Blt_Un, label);
						return;
					case JSToken.LessThanEqual:
						il.Emit(shortForm ? OpCodes.Ble_Un_S : OpCodes.Ble_Un, label);
						return;
					case JSToken.GreaterThanEqual:
						il.Emit(shortForm ? OpCodes.Bge_Un_S : OpCodes.Bge_Un, label);
						return;
					default:
						throw new JScriptException(JSError.InternalError, this.context);
					}
				}
				else
				{
					switch (this.operatorTok)
					{
					case JSToken.GreaterThan:
						il.Emit(shortForm ? OpCodes.Bgt_S : OpCodes.Bgt, label);
						return;
					case JSToken.LessThan:
						il.Emit(shortForm ? OpCodes.Blt_S : OpCodes.Blt, label);
						return;
					case JSToken.LessThanEqual:
						il.Emit(shortForm ? OpCodes.Ble_S : OpCodes.Ble, label);
						return;
					case JSToken.GreaterThanEqual:
						il.Emit(shortForm ? OpCodes.Bge_S : OpCodes.Bge, label);
						return;
					default:
						throw new JScriptException(JSError.InternalError, this.context);
					}
				}
			}
			else if (type3 == Typeob.Int32 || type3 == Typeob.Int64)
			{
				switch (this.operatorTok)
				{
				case JSToken.GreaterThan:
					il.Emit(shortForm ? OpCodes.Ble_S : OpCodes.Ble, label);
					return;
				case JSToken.LessThan:
					il.Emit(shortForm ? OpCodes.Bge_S : OpCodes.Bge, label);
					return;
				case JSToken.LessThanEqual:
					il.Emit(shortForm ? OpCodes.Bgt_S : OpCodes.Bgt, label);
					return;
				case JSToken.GreaterThanEqual:
					il.Emit(shortForm ? OpCodes.Blt_S : OpCodes.Blt, label);
					return;
				default:
					throw new JScriptException(JSError.InternalError, this.context);
				}
			}
			else
			{
				switch (this.operatorTok)
				{
				case JSToken.GreaterThan:
					il.Emit(shortForm ? OpCodes.Ble_Un_S : OpCodes.Ble_Un, label);
					return;
				case JSToken.LessThan:
					il.Emit(shortForm ? OpCodes.Bge_Un_S : OpCodes.Bge_Un, label);
					return;
				case JSToken.LessThanEqual:
					il.Emit(shortForm ? OpCodes.Bgt_Un_S : OpCodes.Bgt_Un, label);
					return;
				case JSToken.GreaterThanEqual:
					il.Emit(shortForm ? OpCodes.Blt_Un_S : OpCodes.Blt_Un, label);
					return;
				default:
					throw new JScriptException(JSError.InternalError, this.context);
				}
			}
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0005666C File Offset: 0x0005566C
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			this.TranslateToConditionalBranch(il, true, label, true);
			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Br_S, label2);
			il.MarkLabel(label);
			il.Emit(OpCodes.Ldc_I4_1);
			il.MarkLabel(label2);
			Convert.Emit(this, il, Typeob.Boolean, rtype);
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x000566D0 File Offset: 0x000556D0
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.operand1.TranslateToILInitializer(il);
			this.operand2.TranslateToILInitializer(il);
			MethodInfo @operator = base.GetOperator(this.operand1.InferType(null), this.operand2.InferType(null));
			if (@operator != null)
			{
				this.metaData = @operator;
				return;
			}
			if ((this.type1.IsPrimitive || Typeob.JSObject.IsAssignableFrom(this.type1)) && (this.type2.IsPrimitive || Typeob.JSObject.IsAssignableFrom(this.type2)))
			{
				return;
			}
			this.metaData = il.DeclareLocal(Typeob.Relational);
			ConstantWrapper.TranslateToILInt(il, (int)this.operatorTok);
			il.Emit(OpCodes.Newobj, CompilerGlobals.relationalConstructor);
			il.Emit(OpCodes.Stloc, (LocalBuilder)this.metaData);
		}

		// Token: 0x040006CE RID: 1742
		private object metaData;
	}
}
