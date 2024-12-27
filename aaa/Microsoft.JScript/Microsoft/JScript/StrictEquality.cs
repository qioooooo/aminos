using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x0200011A RID: 282
	public class StrictEquality : BinaryOp
	{
		// Token: 0x06000B9F RID: 2975 RVA: 0x00057E3C File Offset: 0x00056E3C
		internal StrictEquality(Context context, AST operand1, AST operand2, JSToken operatorTok)
			: base(context, operand1, operand2, operatorTok)
		{
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x00057E4C File Offset: 0x00056E4C
		internal override object Evaluate()
		{
			bool flag = StrictEquality.JScriptStrictEquals(this.operand1.Evaluate(), this.operand2.Evaluate(), VsaEngine.executeForJSEE);
			if (this.operatorTok == JSToken.StrictEqual)
			{
				return flag;
			}
			return !flag;
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x00057E94 File Offset: 0x00056E94
		public static bool JScriptStrictEquals(object v1, object v2)
		{
			return StrictEquality.JScriptStrictEquals(v1, v2, false);
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x00057EA0 File Offset: 0x00056EA0
		internal static bool JScriptStrictEquals(object v1, object v2, bool checkForDebuggerObjects)
		{
			IConvertible iconvertible = Convert.GetIConvertible(v1);
			IConvertible iconvertible2 = Convert.GetIConvertible(v2);
			TypeCode typeCode = Convert.GetTypeCode(v1, iconvertible);
			TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
			return StrictEquality.JScriptStrictEquals(v1, v2, iconvertible, iconvertible2, typeCode, typeCode2, checkForDebuggerObjects);
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00057ED8 File Offset: 0x00056ED8
		internal static bool JScriptStrictEquals(object v1, object v2, IConvertible ic1, IConvertible ic2, TypeCode t1, TypeCode t2, bool checkForDebuggerObjects)
		{
			switch (t1)
			{
			case TypeCode.Empty:
				return t2 == TypeCode.Empty;
			case TypeCode.Object:
				if (v1 == v2)
				{
					return true;
				}
				if (v1 is Missing || v1 is Missing)
				{
					v1 = null;
				}
				if (v1 == v2)
				{
					return true;
				}
				if (v2 is Missing || v2 is Missing)
				{
					v2 = null;
				}
				if (checkForDebuggerObjects)
				{
					IDebuggerObject debuggerObject = v1 as IDebuggerObject;
					if (debuggerObject != null)
					{
						IDebuggerObject debuggerObject2 = v2 as IDebuggerObject;
						if (debuggerObject2 != null)
						{
							return debuggerObject.IsEqual(debuggerObject2);
						}
					}
				}
				return v1 == v2;
			case TypeCode.DBNull:
				return t2 == TypeCode.DBNull;
			case TypeCode.Boolean:
				return t2 == TypeCode.Boolean && ic1.ToBoolean(null) == ic2.ToBoolean(null);
			case TypeCode.Char:
			{
				char c = ic1.ToChar(null);
				switch (t2)
				{
				case TypeCode.Char:
					return c == ic2.ToChar(null);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return (ulong)c == (ulong)ic2.ToInt64(null);
				case TypeCode.UInt64:
					return (ulong)c == ic2.ToUInt64(null);
				case TypeCode.Single:
				case TypeCode.Double:
					return (double)c == ic2.ToDouble(null);
				case TypeCode.Decimal:
					return (int)c == ic2.ToDecimal(null);
				case TypeCode.String:
				{
					string text = ic2.ToString(null);
					return text.Length == 1 && c == text[0];
				}
				}
				return false;
			}
			case TypeCode.SByte:
			{
				sbyte b = ic1.ToSByte(null);
				switch (t2)
				{
				case TypeCode.Char:
					return (char)b == ic2.ToChar(null);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return (long)b == ic2.ToInt64(null);
				case TypeCode.UInt64:
					return b >= 0 && (long)b == (long)ic2.ToUInt64(null);
				case TypeCode.Single:
					return (float)b == ic2.ToSingle(null);
				case TypeCode.Double:
					return (double)b == ic2.ToDouble(null);
				case TypeCode.Decimal:
					return b == ic2.ToDecimal(null);
				default:
					return false;
				}
				break;
			}
			case TypeCode.Byte:
			{
				byte b2 = ic1.ToByte(null);
				switch (t2)
				{
				case TypeCode.Char:
					return (char)b2 == ic2.ToChar(null);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return (ulong)b2 == (ulong)ic2.ToInt64(null);
				case TypeCode.UInt64:
					return (ulong)b2 == ic2.ToUInt64(null);
				case TypeCode.Single:
					return (float)b2 == ic2.ToSingle(null);
				case TypeCode.Double:
					return (double)b2 == ic2.ToDouble(null);
				case TypeCode.Decimal:
					return b2 == ic2.ToDecimal(null);
				default:
					return false;
				}
				break;
			}
			case TypeCode.Int16:
			{
				short num = ic1.ToInt16(null);
				switch (t2)
				{
				case TypeCode.Char:
					return num == (short)ic2.ToChar(null);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return (long)num == ic2.ToInt64(null);
				case TypeCode.UInt64:
					return num >= 0 && (long)num == (long)ic2.ToUInt64(null);
				case TypeCode.Single:
					return (float)num == ic2.ToSingle(null);
				case TypeCode.Double:
					return (double)num == ic2.ToDouble(null);
				case TypeCode.Decimal:
					return num == ic2.ToDecimal(null);
				default:
					return false;
				}
				break;
			}
			case TypeCode.UInt16:
			{
				ushort num2 = ic1.ToUInt16(null);
				switch (t2)
				{
				case TypeCode.Char:
					return num2 == (ushort)ic2.ToChar(null);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return (ulong)num2 == (ulong)ic2.ToInt64(null);
				case TypeCode.UInt64:
					return (ulong)num2 == ic2.ToUInt64(null);
				case TypeCode.Single:
					return (float)num2 == ic2.ToSingle(null);
				case TypeCode.Double:
					return (double)num2 == ic2.ToDouble(null);
				case TypeCode.Decimal:
					return num2 == ic2.ToDecimal(null);
				default:
					return false;
				}
				break;
			}
			case TypeCode.Int32:
			{
				int num3 = ic1.ToInt32(null);
				switch (t2)
				{
				case TypeCode.Char:
					return num3 == (int)ic2.ToChar(null);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return (long)num3 == ic2.ToInt64(null);
				case TypeCode.UInt64:
					return num3 >= 0 && (long)num3 == (long)ic2.ToUInt64(null);
				case TypeCode.Single:
					return (float)num3 == ic2.ToSingle(null);
				case TypeCode.Double:
					return (double)num3 == ic2.ToDouble(null);
				case TypeCode.Decimal:
					return num3 == ic2.ToDecimal(null);
				default:
					return false;
				}
				break;
			}
			case TypeCode.UInt32:
			{
				uint num4 = ic1.ToUInt32(null);
				switch (t2)
				{
				case TypeCode.Char:
					return num4 == (uint)ic2.ToChar(null);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return (ulong)num4 == (ulong)ic2.ToInt64(null);
				case TypeCode.UInt64:
					return (ulong)num4 == ic2.ToUInt64(null);
				case TypeCode.Single:
					return num4 == ic2.ToSingle(null);
				case TypeCode.Double:
					return num4 == ic2.ToDouble(null);
				case TypeCode.Decimal:
					return num4 == ic2.ToDecimal(null);
				default:
					return false;
				}
				break;
			}
			case TypeCode.Int64:
			{
				long num5 = ic1.ToInt64(null);
				switch (t2)
				{
				case TypeCode.Char:
					return num5 == (long)((ulong)ic2.ToChar(null));
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return num5 == ic2.ToInt64(null);
				case TypeCode.UInt64:
					return num5 >= 0L && num5 == (long)ic2.ToUInt64(null);
				case TypeCode.Single:
					return (float)num5 == ic2.ToSingle(null);
				case TypeCode.Double:
					return (double)num5 == ic2.ToDouble(null);
				case TypeCode.Decimal:
					return num5 == ic2.ToDecimal(null);
				default:
					return false;
				}
				break;
			}
			case TypeCode.UInt64:
			{
				ulong num6 = ic1.ToUInt64(null);
				switch (t2)
				{
				case TypeCode.Char:
					return num6 == (ulong)ic2.ToChar(null);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				{
					long num5 = ic2.ToInt64(null);
					return num5 >= 0L && num6 == (ulong)num5;
				}
				case TypeCode.UInt64:
					return num6 == ic2.ToUInt64(null);
				case TypeCode.Single:
					return num6 == ic2.ToSingle(null);
				case TypeCode.Double:
					return num6 == ic2.ToDouble(null);
				case TypeCode.Decimal:
					return num6 == ic2.ToDecimal(null);
				default:
					return false;
				}
				break;
			}
			case TypeCode.Single:
			{
				float num7 = ic1.ToSingle(null);
				switch (t2)
				{
				case TypeCode.Char:
					return num7 == (float)ic2.ToChar(null);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return num7 == (float)ic2.ToInt64(null);
				case TypeCode.UInt64:
					return num7 == ic2.ToUInt64(null);
				case TypeCode.Single:
					return num7 == ic2.ToSingle(null);
				case TypeCode.Double:
					return num7 == ic2.ToSingle(null);
				case TypeCode.Decimal:
					return (decimal)num7 == ic2.ToDecimal(null);
				default:
					return false;
				}
				break;
			}
			case TypeCode.Double:
			{
				double num8 = ic1.ToDouble(null);
				switch (t2)
				{
				case TypeCode.Char:
					return num8 == (double)ic2.ToChar(null);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return num8 == (double)ic2.ToInt64(null);
				case TypeCode.UInt64:
					return num8 == ic2.ToUInt64(null);
				case TypeCode.Single:
					return (float)num8 == ic2.ToSingle(null);
				case TypeCode.Double:
					return num8 == ic2.ToDouble(null);
				case TypeCode.Decimal:
					return (decimal)num8 == ic2.ToDecimal(null);
				default:
					return false;
				}
				break;
			}
			case TypeCode.Decimal:
			{
				decimal num9 = ic1.ToDecimal(null);
				switch (t2)
				{
				case TypeCode.Char:
					return num9 == (int)ic2.ToChar(null);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
					return num9 == ic2.ToInt64(null);
				case TypeCode.UInt64:
					return num9 == ic2.ToUInt64(null);
				case TypeCode.Single:
					return num9 == (decimal)ic2.ToSingle(null);
				case TypeCode.Double:
					return num9 == (decimal)ic2.ToDouble(null);
				case TypeCode.Decimal:
					return num9 == ic2.ToDecimal(null);
				default:
					return false;
				}
				break;
			}
			case TypeCode.DateTime:
				return t2 == TypeCode.DateTime && ic1.ToDateTime(null) == ic2.ToDateTime(null);
			case TypeCode.String:
				if (t2 == TypeCode.Char)
				{
					string text2 = ic1.ToString(null);
					return text2.Length == 1 && text2[0] == ic2.ToChar(null);
				}
				return t2 == TypeCode.String && (v1 == v2 || ic1.ToString(null).Equals(ic2.ToString(null)));
			}
			return false;
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x000587F5 File Offset: 0x000577F5
		internal override IReflect InferType(JSField inference_target)
		{
			return Typeob.Boolean;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x000587FC File Offset: 0x000577FC
		internal override void TranslateToConditionalBranch(ILGenerator il, bool branchIfTrue, Label label, bool shortForm)
		{
			Type type = Convert.ToType(this.operand1.InferType(null));
			Type type2 = Convert.ToType(this.operand2.InferType(null));
			if (this.operand1 is ConstantWrapper && this.operand1.Evaluate() == null)
			{
				type = Typeob.Empty;
			}
			if (this.operand2 is ConstantWrapper && this.operand2.Evaluate() == null)
			{
				type2 = Typeob.Empty;
			}
			if (type != type2 && type.IsPrimitive && type2.IsPrimitive)
			{
				if (type == Typeob.Single)
				{
					type2 = type;
				}
				else if (type2 == Typeob.Single)
				{
					type = type2;
				}
				else if (Convert.IsPromotableTo(type2, type))
				{
					type2 = type;
				}
				else if (Convert.IsPromotableTo(type, type2))
				{
					type = type2;
				}
			}
			bool flag = true;
			if (type == type2 && type != Typeob.Object)
			{
				Type type3 = type;
				if (!type.IsPrimitive)
				{
					type3 = Typeob.Object;
				}
				this.operand1.TranslateToIL(il, type3);
				this.operand2.TranslateToIL(il, type3);
				if (type == Typeob.String)
				{
					il.Emit(OpCodes.Call, CompilerGlobals.stringEqualsMethod);
				}
				else if (!type.IsPrimitive)
				{
					il.Emit(OpCodes.Callvirt, CompilerGlobals.equalsMethod);
				}
				else
				{
					flag = false;
				}
			}
			else if (type == Typeob.Empty)
			{
				this.operand2.TranslateToIL(il, Typeob.Object);
				branchIfTrue = !branchIfTrue;
			}
			else if (type2 == Typeob.Empty)
			{
				this.operand1.TranslateToIL(il, Typeob.Object);
				branchIfTrue = !branchIfTrue;
			}
			else
			{
				this.operand1.TranslateToIL(il, Typeob.Object);
				this.operand2.TranslateToIL(il, Typeob.Object);
				il.Emit(OpCodes.Call, CompilerGlobals.jScriptStrictEqualsMethod);
			}
			if (branchIfTrue)
			{
				if (this.operatorTok == JSToken.StrictEqual)
				{
					if (flag)
					{
						il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
						return;
					}
					il.Emit(shortForm ? OpCodes.Beq_S : OpCodes.Beq, label);
					return;
				}
				else
				{
					if (flag)
					{
						il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
						return;
					}
					il.Emit(shortForm ? OpCodes.Bne_Un_S : OpCodes.Bne_Un, label);
					return;
				}
			}
			else if (this.operatorTok == JSToken.StrictEqual)
			{
				if (flag)
				{
					il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
					return;
				}
				il.Emit(shortForm ? OpCodes.Bne_Un_S : OpCodes.Bne_Un, label);
				return;
			}
			else
			{
				if (flag)
				{
					il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
					return;
				}
				il.Emit(shortForm ? OpCodes.Beq_S : OpCodes.Beq, label);
				return;
			}
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00058A7C File Offset: 0x00057A7C
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
	}
}
