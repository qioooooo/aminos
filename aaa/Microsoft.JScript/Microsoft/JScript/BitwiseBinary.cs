using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200002A RID: 42
	public sealed class BitwiseBinary : BinaryOp
	{
		// Token: 0x060001BC RID: 444 RVA: 0x0000D1A1 File Offset: 0x0000C1A1
		internal BitwiseBinary(Context context, AST operand1, AST operand2, JSToken operatorTok)
			: base(context, operand1, operand2, operatorTok)
		{
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000D1AE File Offset: 0x0000C1AE
		public BitwiseBinary(int operatorTok)
			: base(null, null, null, (JSToken)operatorTok)
		{
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000D1BA File Offset: 0x0000C1BA
		internal override object Evaluate()
		{
			return this.EvaluateBitwiseBinary(this.operand1.Evaluate(), this.operand2.Evaluate());
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000D1D8 File Offset: 0x0000C1D8
		[DebuggerStepThrough]
		[DebuggerHidden]
		public object EvaluateBitwiseBinary(object v1, object v2)
		{
			if (v1 is int && v2 is int)
			{
				return BitwiseBinary.DoOp((int)v1, (int)v2, this.operatorTok);
			}
			return this.EvaluateBitwiseBinary(v1, v2, this.operatorTok);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000D210 File Offset: 0x0000C210
		[DebuggerStepThrough]
		[DebuggerHidden]
		private object EvaluateBitwiseBinary(object v1, object v2, JSToken operatorTok)
		{
			IConvertible iconvertible = Convert.GetIConvertible(v1);
			IConvertible iconvertible2 = Convert.GetIConvertible(v2);
			TypeCode typeCode = Convert.GetTypeCode(v1, iconvertible);
			TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
			switch (typeCode)
			{
			case TypeCode.Empty:
			case TypeCode.DBNull:
				return this.EvaluateBitwiseBinary(0, v2, operatorTok);
			case TypeCode.Boolean:
			case TypeCode.Char:
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
				case TypeCode.DBNull:
					return BitwiseBinary.DoOp(num, 0, operatorTok);
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
					return BitwiseBinary.DoOp(num, iconvertible2.ToInt32(null), operatorTok);
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
					return BitwiseBinary.DoOp(num, (int)Runtime.DoubleToInt64(iconvertible2.ToDouble(null)), operatorTok);
				}
				break;
			}
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			{
				int num = (int)Runtime.DoubleToInt64(iconvertible.ToDouble(null));
				switch (typeCode2)
				{
				case TypeCode.Empty:
				case TypeCode.DBNull:
					return BitwiseBinary.DoOp(num, 0, operatorTok);
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
					return BitwiseBinary.DoOp(num, iconvertible2.ToInt32(null), operatorTok);
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
					return BitwiseBinary.DoOp(num, (int)Runtime.DoubleToInt64(iconvertible2.ToDouble(null)), operatorTok);
				}
				break;
			}
			}
			if (v2 == null)
			{
				return BitwiseBinary.DoOp(Convert.ToInt32(v1), 0, this.operatorTok);
			}
			MethodInfo @operator = base.GetOperator(v1.GetType(), v2.GetType());
			if (@operator != null)
			{
				return @operator.Invoke(null, BindingFlags.Default, JSBinder.ob, new object[] { v1, v2 }, null);
			}
			return BitwiseBinary.DoOp(Convert.ToInt32(v1), Convert.ToInt32(v2), this.operatorTok);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000D43C File Offset: 0x0000C43C
		internal static object DoOp(int i, int j, JSToken operatorTok)
		{
			switch (operatorTok)
			{
			case JSToken.BitwiseOr:
				return i | j;
			case JSToken.BitwiseXor:
				return i ^ j;
			case JSToken.BitwiseAnd:
				return i & j;
			default:
				switch (operatorTok)
				{
				case JSToken.LeftShift:
					return i << j;
				case JSToken.RightShift:
					return i >> j;
				case JSToken.UnsignedRightShift:
					return (uint)i >> j;
				default:
					throw new JScriptException(JSError.InternalError);
				}
				break;
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000D4C0 File Offset: 0x0000C4C0
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
			return BitwiseBinary.ResultType(this.type1, this.type2, this.operatorTok);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000D534 File Offset: 0x0000C534
		internal static Type Operand2Type(JSToken operatorTok, Type bbrType)
		{
			switch (operatorTok)
			{
			case JSToken.LeftShift:
			case JSToken.RightShift:
			case JSToken.UnsignedRightShift:
				return Typeob.Int32;
			default:
				return bbrType;
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000D564 File Offset: 0x0000C564
		internal static Type ResultType(Type type1, Type type2, JSToken operatorTok)
		{
			switch (operatorTok)
			{
			case JSToken.LeftShift:
			case JSToken.RightShift:
				if (Convert.IsPrimitiveIntegerType(type1))
				{
					return type1;
				}
				if (Typeob.JSObject.IsAssignableFrom(type1))
				{
					return Typeob.Int32;
				}
				return Typeob.Object;
			case JSToken.UnsignedRightShift:
				switch (Type.GetTypeCode(type1))
				{
				case TypeCode.SByte:
				case TypeCode.Byte:
					return Typeob.Byte;
				case TypeCode.Int16:
				case TypeCode.UInt16:
					return Typeob.UInt16;
				case TypeCode.Int32:
				case TypeCode.UInt32:
					return Typeob.UInt32;
				case TypeCode.Int64:
				case TypeCode.UInt64:
					return Typeob.UInt64;
				default:
					if (Typeob.JSObject.IsAssignableFrom(type1))
					{
						return Typeob.Int32;
					}
					return Typeob.Object;
				}
				break;
			default:
			{
				TypeCode typeCode = Type.GetTypeCode(type1);
				TypeCode typeCode2 = Type.GetTypeCode(type2);
				switch (typeCode)
				{
				case TypeCode.Empty:
				case TypeCode.DBNull:
				case TypeCode.Boolean:
					switch (typeCode2)
					{
					case TypeCode.Empty:
					case TypeCode.DBNull:
					case TypeCode.Boolean:
					case TypeCode.Int32:
					case TypeCode.Single:
					case TypeCode.Double:
						return Typeob.Int32;
					case TypeCode.Object:
						if (Typeob.JSObject.IsAssignableFrom(type2))
						{
							return Typeob.Int32;
						}
						break;
					case TypeCode.Char:
					case TypeCode.UInt16:
						return Typeob.UInt16;
					case TypeCode.SByte:
						return Typeob.SByte;
					case TypeCode.Byte:
						return Typeob.Byte;
					case TypeCode.Int16:
						return Typeob.Int16;
					case TypeCode.UInt32:
						return Typeob.UInt32;
					case TypeCode.Int64:
						return Typeob.Int64;
					case TypeCode.UInt64:
						return Typeob.UInt64;
					}
					break;
				case TypeCode.Object:
					if (Typeob.JSObject.IsAssignableFrom(type1))
					{
						return Typeob.Int32;
					}
					break;
				case TypeCode.Char:
				case TypeCode.UInt16:
					switch (typeCode2)
					{
					case TypeCode.Empty:
					case TypeCode.DBNull:
					case TypeCode.Boolean:
					case TypeCode.Char:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
						return Typeob.UInt16;
					case TypeCode.Object:
						if (Typeob.JSObject.IsAssignableFrom(type2))
						{
							return Typeob.UInt32;
						}
						break;
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Single:
					case TypeCode.Double:
						return Typeob.UInt32;
					case TypeCode.Int64:
					case TypeCode.UInt64:
						return Typeob.UInt64;
					}
					break;
				case TypeCode.SByte:
					switch (typeCode2)
					{
					case TypeCode.Empty:
					case TypeCode.DBNull:
					case TypeCode.Boolean:
					case TypeCode.SByte:
						return Typeob.SByte;
					case TypeCode.Object:
						if (Typeob.JSObject.IsAssignableFrom(type2))
						{
							return Typeob.Int32;
						}
						break;
					case TypeCode.Char:
					case TypeCode.Int16:
						return Typeob.Int16;
					case TypeCode.Byte:
						return Typeob.Byte;
					case TypeCode.UInt16:
						return Typeob.UInt16;
					case TypeCode.Int32:
					case TypeCode.Single:
					case TypeCode.Double:
						return Typeob.Int32;
					case TypeCode.UInt32:
						return Typeob.UInt32;
					case TypeCode.Int64:
						return Typeob.Int64;
					case TypeCode.UInt64:
						return Typeob.UInt64;
					}
					break;
				case TypeCode.Byte:
					switch (typeCode2)
					{
					case TypeCode.Empty:
					case TypeCode.DBNull:
					case TypeCode.Boolean:
					case TypeCode.SByte:
					case TypeCode.Byte:
						return Typeob.Byte;
					case TypeCode.Object:
						if (Typeob.JSObject.IsAssignableFrom(type2))
						{
							return Typeob.UInt32;
						}
						break;
					case TypeCode.Char:
					case TypeCode.Int16:
					case TypeCode.UInt16:
						return Typeob.UInt16;
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Single:
					case TypeCode.Double:
						return Typeob.UInt32;
					case TypeCode.Int64:
					case TypeCode.UInt64:
						return Typeob.UInt64;
					}
					break;
				case TypeCode.Int16:
					switch (typeCode2)
					{
					case TypeCode.Empty:
					case TypeCode.DBNull:
					case TypeCode.Boolean:
					case TypeCode.SByte:
					case TypeCode.Int16:
						return Typeob.Int16;
					case TypeCode.Object:
						if (Typeob.JSObject.IsAssignableFrom(type2))
						{
							return Typeob.Int32;
						}
						break;
					case TypeCode.Char:
					case TypeCode.Byte:
					case TypeCode.UInt16:
						return Typeob.UInt16;
					case TypeCode.Int32:
					case TypeCode.Single:
					case TypeCode.Double:
						return Typeob.Int32;
					case TypeCode.UInt32:
						return Typeob.UInt32;
					case TypeCode.Int64:
						return Typeob.Int64;
					case TypeCode.UInt64:
						return Typeob.UInt64;
					}
					break;
				case TypeCode.Int32:
				case TypeCode.Single:
				case TypeCode.Double:
					switch (typeCode2)
					{
					case TypeCode.Empty:
					case TypeCode.DBNull:
					case TypeCode.Boolean:
					case TypeCode.SByte:
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Single:
					case TypeCode.Double:
						return Typeob.Int32;
					case TypeCode.Object:
						if (Typeob.JSObject.IsAssignableFrom(type2))
						{
							return Typeob.Int32;
						}
						break;
					case TypeCode.Char:
					case TypeCode.Byte:
					case TypeCode.UInt16:
					case TypeCode.UInt32:
						return Typeob.UInt32;
					case TypeCode.Int64:
						return Typeob.Int64;
					case TypeCode.UInt64:
						return Typeob.UInt64;
					}
					break;
				case TypeCode.UInt32:
					switch (typeCode2)
					{
					case TypeCode.Empty:
					case TypeCode.DBNull:
					case TypeCode.Boolean:
					case TypeCode.Char:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Single:
					case TypeCode.Double:
						return Typeob.UInt32;
					case TypeCode.Object:
						if (Typeob.JSObject.IsAssignableFrom(type2))
						{
							return Typeob.UInt32;
						}
						break;
					case TypeCode.Int64:
					case TypeCode.UInt64:
						return Typeob.UInt64;
					}
					break;
				case TypeCode.Int64:
					switch (typeCode2)
					{
					case TypeCode.Empty:
					case TypeCode.DBNull:
					case TypeCode.Boolean:
					case TypeCode.SByte:
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.Double:
						return Typeob.Int64;
					case TypeCode.Object:
						if (Typeob.JSObject.IsAssignableFrom(type2))
						{
							return Typeob.Int64;
						}
						break;
					case TypeCode.Char:
					case TypeCode.Byte:
					case TypeCode.UInt16:
					case TypeCode.UInt32:
					case TypeCode.UInt64:
						return Typeob.UInt64;
					}
					break;
				case TypeCode.UInt64:
					switch (typeCode2)
					{
					case TypeCode.Empty:
					case TypeCode.DBNull:
					case TypeCode.Boolean:
					case TypeCode.Char:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
						return Typeob.UInt64;
					case TypeCode.Object:
						if (Typeob.JSObject.IsAssignableFrom(type2))
						{
							return Typeob.UInt64;
						}
						break;
					}
					break;
				}
				return Typeob.Object;
			}
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000DB6C File Offset: 0x0000CB6C
		internal static void TranslateToBitCountMask(ILGenerator il, Type type, AST operand2)
		{
			int num = 0;
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.SByte:
			case TypeCode.Byte:
				num = 7;
				break;
			case TypeCode.Int16:
			case TypeCode.UInt16:
				num = 15;
				break;
			case TypeCode.Int32:
			case TypeCode.UInt32:
				num = 31;
				break;
			case TypeCode.Int64:
			case TypeCode.UInt64:
				num = 63;
				break;
			}
			ConstantWrapper constantWrapper = operand2 as ConstantWrapper;
			if (constantWrapper != null)
			{
				int num2 = Convert.ToInt32(constantWrapper.value);
				if (num2 <= num)
				{
					return;
				}
			}
			il.Emit(OpCodes.Ldc_I4_S, num);
			il.Emit(OpCodes.And);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000DBF0 File Offset: 0x0000CBF0
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if (this.metaData == null)
			{
				Type type = BitwiseBinary.ResultType(this.type1, this.type2, this.operatorTok);
				if (Convert.IsPrimitiveNumericType(this.type1))
				{
					this.operand1.TranslateToIL(il, this.type1);
					Convert.Emit(this, il, this.type1, type, true);
				}
				else
				{
					this.operand1.TranslateToIL(il, Typeob.Double);
					Convert.Emit(this, il, Typeob.Double, type, true);
				}
				Type type2 = BitwiseBinary.Operand2Type(this.operatorTok, type);
				if (Convert.IsPrimitiveNumericType(this.type2))
				{
					this.operand2.TranslateToIL(il, this.type2);
					Convert.Emit(this, il, this.type2, type2, true);
				}
				else
				{
					this.operand2.TranslateToIL(il, Typeob.Double);
					Convert.Emit(this, il, Typeob.Double, type2, true);
				}
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
						BitwiseBinary.TranslateToBitCountMask(il, type, this.operand2);
						il.Emit(OpCodes.Shl);
						break;
					case JSToken.RightShift:
						BitwiseBinary.TranslateToBitCountMask(il, type, this.operand2);
						il.Emit(OpCodes.Shr);
						break;
					case JSToken.UnsignedRightShift:
						BitwiseBinary.TranslateToBitCountMask(il, type, this.operand2);
						il.Emit(OpCodes.Shr_Un);
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
				this.operand1.TranslateToIL(il, parameters[0].ParameterType);
				this.operand2.TranslateToIL(il, parameters[1].ParameterType);
				il.Emit(OpCodes.Call, methodInfo);
				Convert.Emit(this, il, methodInfo.ReturnType, rtype);
				return;
			}
			il.Emit(OpCodes.Ldloc, (LocalBuilder)this.metaData);
			this.operand1.TranslateToIL(il, Typeob.Object);
			this.operand2.TranslateToIL(il, Typeob.Object);
			il.Emit(OpCodes.Call, CompilerGlobals.evaluateBitwiseBinaryMethod);
			Convert.Emit(this, il, Typeob.Object, rtype);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000DE48 File Offset: 0x0000CE48
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

		// Token: 0x0400008D RID: 141
		private object metaData;
	}
}
