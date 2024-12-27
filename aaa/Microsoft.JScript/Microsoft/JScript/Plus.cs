using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x020000F8 RID: 248
	public sealed class Plus : BinaryOp
	{
		// Token: 0x06000ABA RID: 2746 RVA: 0x00051EBB File Offset: 0x00050EBB
		internal Plus(Context context, AST operand1, AST operand2)
			: base(context, operand1, operand2, JSToken.FirstBinaryOp)
		{
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00051EC8 File Offset: 0x00050EC8
		public Plus()
			: base(null, null, null, JSToken.FirstBinaryOp)
		{
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00051ED5 File Offset: 0x00050ED5
		internal override object Evaluate()
		{
			return this.EvaluatePlus(this.operand1.Evaluate(), this.operand2.Evaluate());
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00051EF4 File Offset: 0x00050EF4
		[DebuggerStepThrough]
		[DebuggerHidden]
		public object EvaluatePlus(object v1, object v2)
		{
			if (v1 is int && v2 is int)
			{
				return Plus.DoOp((int)v1, (int)v2);
			}
			if (v1 is double && v2 is double)
			{
				return Plus.DoOp((double)v1, (double)v2);
			}
			return this.EvaluatePlus2(v1, v2);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x00051F50 File Offset: 0x00050F50
		[DebuggerHidden]
		[DebuggerStepThrough]
		private object EvaluatePlus2(object v1, object v2)
		{
			IConvertible iconvertible = Convert.GetIConvertible(v1);
			IConvertible iconvertible2 = Convert.GetIConvertible(v2);
			TypeCode typeCode = Convert.GetTypeCode(v1, iconvertible);
			TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
			switch (typeCode)
			{
			case TypeCode.Empty:
				return Plus.DoOp(v1, v2);
			case TypeCode.DBNull:
				switch (typeCode2)
				{
				case TypeCode.Empty:
					return double.NaN;
				case TypeCode.DBNull:
					return 0;
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
					return iconvertible2.ToInt32(null);
				case TypeCode.UInt32:
					return iconvertible2.ToUInt32(null);
				case TypeCode.Int64:
					return iconvertible2.ToInt64(null);
				case TypeCode.UInt64:
					return iconvertible2.ToUInt64(null);
				case TypeCode.Single:
				case TypeCode.Double:
					return iconvertible2.ToDouble(null);
				case TypeCode.String:
					return "null" + iconvertible2.ToString(null);
				}
				break;
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
					return num;
				case TypeCode.Boolean:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
					return Plus.DoOp(num, iconvertible2.ToInt32(null));
				case TypeCode.Char:
					return ((IConvertible)Plus.DoOp(num, iconvertible2.ToInt32(null))).ToChar(null);
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return Plus.DoOp((long)num, iconvertible2.ToInt64(null));
				case TypeCode.UInt64:
					if (num >= 0)
					{
						return Plus.DoOp((ulong)((long)num), iconvertible2.ToUInt64(null));
					}
					return Plus.DoOp((double)num, iconvertible2.ToDouble(null));
				case TypeCode.Single:
				case TypeCode.Double:
					return Plus.DoOp((double)num, iconvertible2.ToDouble(null));
				case TypeCode.String:
					return Convert.ToString(v1) + iconvertible2.ToString(null);
				}
				break;
			}
			case TypeCode.Char:
			{
				int num2 = iconvertible.ToInt32(null);
				switch (typeCode2)
				{
				case TypeCode.Empty:
					return double.NaN;
				case TypeCode.Object:
				case TypeCode.Decimal:
				case TypeCode.DateTime:
					return Plus.DoOp(v1, v2);
				case TypeCode.DBNull:
					return num2;
				case TypeCode.Boolean:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
					return ((IConvertible)Plus.DoOp(num2, iconvertible2.ToInt32(null))).ToChar(null);
				case TypeCode.Char:
				case TypeCode.String:
					return iconvertible.ToString(null) + iconvertible2.ToString(null);
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return ((IConvertible)Plus.DoOp((long)num2, iconvertible2.ToInt64(null))).ToChar(null);
				case TypeCode.UInt64:
					return ((IConvertible)Plus.DoOp((ulong)((long)num2), iconvertible2.ToUInt64(null))).ToChar(null);
				case TypeCode.Single:
				case TypeCode.Double:
					return checked((char)((int)Convert.CheckIfDoubleIsInteger((double)Plus.DoOp((double)num2, iconvertible2.ToDouble(null)))));
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
					return num3;
				case TypeCode.Boolean:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
					return Plus.DoOp(num3, iconvertible2.ToUInt32(null));
				case TypeCode.Char:
					return ((IConvertible)Plus.DoOp(num3, iconvertible2.ToUInt32(null))).ToChar(null);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.Int32:
				{
					int num4 = iconvertible2.ToInt32(null);
					if (num4 >= 0)
					{
						return Plus.DoOp(num3, (uint)num4);
					}
					return Plus.DoOp((long)((ulong)num3), (long)num4);
				}
				case TypeCode.Int64:
					return Plus.DoOp((long)((ulong)num3), iconvertible2.ToInt64(null));
				case TypeCode.UInt64:
					return Plus.DoOp((ulong)num3, iconvertible2.ToUInt64(null));
				case TypeCode.Single:
				case TypeCode.Double:
					return Plus.DoOp(num3, iconvertible2.ToDouble(null));
				case TypeCode.String:
					return Convert.ToString(v1) + iconvertible2.ToString(null);
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
					return num5;
				case TypeCode.Boolean:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return Plus.DoOp(num5, iconvertible2.ToInt64(null));
				case TypeCode.Char:
					return ((IConvertible)Plus.DoOp(num5, iconvertible2.ToInt64(null))).ToChar(null);
				case TypeCode.UInt64:
					if (num5 >= 0L)
					{
						return Plus.DoOp((ulong)num5, iconvertible2.ToUInt64(null));
					}
					return Plus.DoOp((double)num5, iconvertible2.ToDouble(null));
				case TypeCode.Single:
				case TypeCode.Double:
					return Plus.DoOp((double)num5, iconvertible2.ToDouble(null));
				case TypeCode.String:
					return Convert.ToString(v1) + iconvertible2.ToString(null);
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
					return num6;
				case TypeCode.Boolean:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
					return Plus.DoOp(num6, iconvertible2.ToUInt64(null));
				case TypeCode.Char:
					return ((IConvertible)Plus.DoOp(num6, iconvertible2.ToUInt64(null))).ToChar(null);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				{
					long num7 = iconvertible2.ToInt64(null);
					if (num7 >= 0L)
					{
						return Plus.DoOp(num6, (ulong)num7);
					}
					return Plus.DoOp(num6, (double)num7);
				}
				case TypeCode.Single:
				case TypeCode.Double:
					return Plus.DoOp(num6, iconvertible2.ToDouble(null));
				case TypeCode.String:
					return Convert.ToString(v1) + iconvertible2.ToString(null);
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
					return iconvertible.ToDouble(null);
				case TypeCode.Boolean:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
					return num8 + (double)iconvertible2.ToInt32(null);
				case TypeCode.Char:
					return Convert.ToChar(Convert.ToInt32(num8 + (double)iconvertible2.ToInt32(null)));
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
					return num8 + iconvertible2.ToDouble(null);
				case TypeCode.String:
					return new ConcatString(Convert.ToString(num8), iconvertible2.ToString(null));
				}
				break;
			}
			case TypeCode.String:
			{
				TypeCode typeCode3 = typeCode2;
				if (typeCode3 != TypeCode.Object)
				{
					if (typeCode3 == TypeCode.String)
					{
						if (v1 is ConcatString)
						{
							return new ConcatString((ConcatString)v1, iconvertible2.ToString(null));
						}
						return new ConcatString(iconvertible.ToString(null), iconvertible2.ToString(null));
					}
					else
					{
						if (v1 is ConcatString)
						{
							return new ConcatString((ConcatString)v1, Convert.ToString(v2));
						}
						return new ConcatString(iconvertible.ToString(null), Convert.ToString(v2));
					}
				}
				break;
			}
			}
			MethodInfo @operator = this.GetOperator((v1 == null) ? Typeob.Empty : v1.GetType(), (v2 == null) ? Typeob.Empty : v2.GetType());
			if (@operator != null)
			{
				return @operator.Invoke(null, BindingFlags.Default, JSBinder.ob, new object[] { v1, v2 }, null);
			}
			return Plus.DoOp(v1, v2);
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00052740 File Offset: 0x00051740
		private new MethodInfo GetOperator(IReflect ir1, IReflect ir2)
		{
			Type type = ((ir1 is Type) ? ((Type)ir1) : Typeob.Object);
			Type type2 = ((ir2 is Type) ? ((Type)ir2) : Typeob.Object);
			if (this.type1 == type && this.type2 == type2)
			{
				return this.operatorMeth;
			}
			if (type == Typeob.String || type2 == Typeob.String || ((Convert.IsPrimitiveNumericType(type) || Typeob.JSObject.IsAssignableFrom(type)) && (Convert.IsPrimitiveNumericType(type2) || Typeob.JSObject.IsAssignableFrom(type2))))
			{
				this.operatorMeth = null;
				this.type1 = type;
				this.type2 = type2;
				return null;
			}
			return base.GetOperator(type, type2);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x000527EB File Offset: 0x000517EB
		private static object DoOp(double x, double y)
		{
			return x + y;
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x000527F8 File Offset: 0x000517F8
		private static object DoOp(int x, int y)
		{
			int num = x + y;
			if (num < x == y < 0)
			{
				return num;
			}
			return (double)x + (double)y;
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00052824 File Offset: 0x00051824
		private static object DoOp(long x, long y)
		{
			long num = x + y;
			if (num < x == y < 0L)
			{
				return num;
			}
			return (double)x + (double)y;
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00052854 File Offset: 0x00051854
		private static object DoOp(uint x, uint y)
		{
			uint num = x + y;
			if (num >= x)
			{
				return num;
			}
			return x + y;
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0005287C File Offset: 0x0005187C
		private static object DoOp(ulong x, ulong y)
		{
			ulong num = x + y;
			if (num >= x)
			{
				return num;
			}
			return x + y;
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x000528A4 File Offset: 0x000518A4
		public static object DoOp(object v1, object v2)
		{
			IConvertible iconvertible = Convert.GetIConvertible(v1);
			IConvertible iconvertible2 = Convert.GetIConvertible(v2);
			v1 = Convert.ToPrimitive(v1, PreferredType.Either, ref iconvertible);
			v2 = Convert.ToPrimitive(v2, PreferredType.Either, ref iconvertible2);
			TypeCode typeCode = Convert.GetTypeCode(v1, iconvertible);
			TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
			if (typeCode == TypeCode.String)
			{
				if (v1 is ConcatString)
				{
					return new ConcatString((ConcatString)v1, Convert.ToString(v2, iconvertible2));
				}
				return new ConcatString(iconvertible.ToString(null), Convert.ToString(v2, iconvertible2));
			}
			else
			{
				if (typeCode2 == TypeCode.String)
				{
					return Convert.ToString(v1, iconvertible) + iconvertible2.ToString(null);
				}
				if (typeCode == TypeCode.Char && typeCode2 == TypeCode.Char)
				{
					return iconvertible.ToString(null) + iconvertible2.ToString(null);
				}
				if ((typeCode == TypeCode.Char && (Convert.IsPrimitiveNumericTypeCode(typeCode2) || typeCode2 == TypeCode.Boolean)) || (typeCode2 == TypeCode.Char && (Convert.IsPrimitiveNumericTypeCode(typeCode) || typeCode == TypeCode.Boolean)))
				{
					return (char)((int)Runtime.DoubleToInt64(Convert.ToNumber(v1, iconvertible) + Convert.ToNumber(v2, iconvertible2)));
				}
				return Convert.ToNumber(v1, iconvertible) + Convert.ToNumber(v2, iconvertible2);
			}
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x000529A0 File Offset: 0x000519A0
		internal override IReflect InferType(JSField inference_target)
		{
			MethodInfo methodInfo;
			if (this.type1 == null || inference_target != null)
			{
				methodInfo = this.GetOperator(this.operand1.InferType(inference_target), this.operand2.InferType(inference_target));
			}
			else
			{
				methodInfo = this.GetOperator(this.type1, this.type2);
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
			if (this.type1 == Typeob.Char && this.type2 == Typeob.Char)
			{
				return Typeob.String;
			}
			if (Convert.IsPrimitiveNumericTypeFitForDouble(this.type1))
			{
				if (this.type2 == Typeob.Char)
				{
					return Typeob.Char;
				}
				if (Convert.IsPrimitiveNumericTypeFitForDouble(this.type2))
				{
					return Typeob.Double;
				}
				return Typeob.Object;
			}
			else if (Convert.IsPrimitiveNumericTypeFitForDouble(this.type2))
			{
				if (this.type1 == Typeob.Char)
				{
					return Typeob.Char;
				}
				if (Convert.IsPrimitiveNumericTypeFitForDouble(this.type1))
				{
					return Typeob.Double;
				}
				return Typeob.Object;
			}
			else
			{
				if (this.type1 == Typeob.Boolean && this.type2 == Typeob.Char)
				{
					return Typeob.Char;
				}
				if (this.type1 == Typeob.Char && this.type2 == Typeob.Boolean)
				{
					return Typeob.Char;
				}
				return Typeob.Object;
			}
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00052AF4 File Offset: 0x00051AF4
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Type type = Convert.ToType(this.InferType(null));
			if (this.metaData == null)
			{
				Type type2 = Typeob.Object;
				if (rtype == Typeob.Double)
				{
					type2 = rtype;
				}
				else if (this.type1 == Typeob.Char && this.type2 == Typeob.Char)
				{
					type2 = Typeob.String;
				}
				else if (Convert.IsPrimitiveNumericType(rtype) && Convert.IsPromotableTo(this.type1, rtype) && Convert.IsPromotableTo(this.type2, rtype))
				{
					type2 = rtype;
				}
				else if (this.type1 != Typeob.String && this.type2 != Typeob.String)
				{
					type2 = Typeob.Double;
				}
				else
				{
					type2 = Typeob.String;
				}
				if (type2 == Typeob.SByte || type2 == Typeob.Int16)
				{
					type2 = Typeob.Int32;
				}
				else if (type2 == Typeob.Byte || type2 == Typeob.UInt16 || type2 == Typeob.Char)
				{
					type2 = Typeob.UInt32;
				}
				if (type2 == Typeob.String)
				{
					if (!(this.operand1 is Plus) || this.type1 != type2)
					{
						Plus.TranslateToStringWithSpecialCaseForNull(il, this.operand1);
						Plus.TranslateToStringWithSpecialCaseForNull(il, this.operand2);
						il.Emit(OpCodes.Call, CompilerGlobals.stringConcat2Method);
						Convert.Emit(this, il, type2, rtype);
						return;
					}
					Plus plus = (Plus)this.operand1;
					if (!(plus.operand1 is Plus) || plus.type1 != type2)
					{
						Plus.TranslateToStringWithSpecialCaseForNull(il, plus.operand1);
						Plus.TranslateToStringWithSpecialCaseForNull(il, plus.operand2);
						Plus.TranslateToStringWithSpecialCaseForNull(il, this.operand2);
						il.Emit(OpCodes.Call, CompilerGlobals.stringConcat3Method);
						Convert.Emit(this, il, type2, rtype);
						return;
					}
					Plus plus2 = (Plus)plus.operand1;
					if (plus2.operand1 is Plus && plus2.type1 == type2)
					{
						int num = plus.TranslateToILArrayOfStrings(il, 1);
						il.Emit(OpCodes.Dup);
						ConstantWrapper.TranslateToILInt(il, num - 1);
						this.operand2.TranslateToIL(il, type2);
						il.Emit(OpCodes.Stelem_Ref);
						il.Emit(OpCodes.Call, CompilerGlobals.stringConcatArrMethod);
						Convert.Emit(this, il, type2, rtype);
						return;
					}
					Plus.TranslateToStringWithSpecialCaseForNull(il, plus2.operand1);
					Plus.TranslateToStringWithSpecialCaseForNull(il, plus2.operand2);
					Plus.TranslateToStringWithSpecialCaseForNull(il, plus.operand2);
					Plus.TranslateToStringWithSpecialCaseForNull(il, this.operand2);
					il.Emit(OpCodes.Call, CompilerGlobals.stringConcat4Method);
					Convert.Emit(this, il, type2, rtype);
					return;
				}
				else
				{
					this.operand1.TranslateToIL(il, type2);
					this.operand2.TranslateToIL(il, type2);
					if (type2 == Typeob.Object)
					{
						il.Emit(OpCodes.Call, CompilerGlobals.plusDoOpMethod);
					}
					else if (type2 == Typeob.Double || type2 == Typeob.Single)
					{
						il.Emit(OpCodes.Add);
					}
					else if (type2 == Typeob.Int32 || type2 == Typeob.Int64)
					{
						il.Emit(OpCodes.Add_Ovf);
					}
					else
					{
						il.Emit(OpCodes.Add_Ovf_Un);
					}
					if (type == Typeob.Char)
					{
						Convert.Emit(this, il, type2, Typeob.Char);
						Convert.Emit(this, il, Typeob.Char, rtype);
						return;
					}
					Convert.Emit(this, il, type2, rtype);
					return;
				}
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
				il.Emit(OpCodes.Callvirt, CompilerGlobals.evaluatePlusMethod);
				Convert.Emit(this, il, Typeob.Object, rtype);
				return;
			}
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00052EB4 File Offset: 0x00051EB4
		private int TranslateToILArrayOfStrings(ILGenerator il, int n)
		{
			int num = n + 2;
			if (this.operand1 is Plus && this.type1 == Typeob.String)
			{
				num = ((Plus)this.operand1).TranslateToILArrayOfStrings(il, n + 1);
			}
			else
			{
				ConstantWrapper.TranslateToILInt(il, num);
				il.Emit(OpCodes.Newarr, Typeob.String);
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Ldc_I4_0);
				Plus.TranslateToStringWithSpecialCaseForNull(il, this.operand1);
				il.Emit(OpCodes.Stelem_Ref);
			}
			il.Emit(OpCodes.Dup);
			ConstantWrapper.TranslateToILInt(il, num - 1 - n);
			Plus.TranslateToStringWithSpecialCaseForNull(il, this.operand2);
			il.Emit(OpCodes.Stelem_Ref);
			return num;
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00052F68 File Offset: 0x00051F68
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

		// Token: 0x06000ACA RID: 2762 RVA: 0x00052FD8 File Offset: 0x00051FD8
		private static void TranslateToStringWithSpecialCaseForNull(ILGenerator il, AST operand)
		{
			ConstantWrapper constantWrapper = operand as ConstantWrapper;
			if (constantWrapper == null)
			{
				operand.TranslateToIL(il, Typeob.String);
				return;
			}
			if (constantWrapper.value is DBNull)
			{
				il.Emit(OpCodes.Ldstr, "null");
				return;
			}
			if (constantWrapper.value == Empty.Value)
			{
				il.Emit(OpCodes.Ldstr, "undefined");
				return;
			}
			constantWrapper.TranslateToIL(il, Typeob.String);
		}

		// Token: 0x0400069C RID: 1692
		private object metaData;
	}
}
