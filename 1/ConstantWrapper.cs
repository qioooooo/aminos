using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200004E RID: 78
	internal class ConstantWrapper : AST
	{
		// Token: 0x060003AB RID: 939 RVA: 0x00016F9A File Offset: 0x00015F9A
		internal ConstantWrapper(object value, Context context)
			: base(context)
		{
			if (value is ConcatString)
			{
				value = value.ToString();
			}
			this.value = value;
			this.isNumericLiteral = false;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00016FC1 File Offset: 0x00015FC1
		internal override object Evaluate()
		{
			return this.value;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00016FCC File Offset: 0x00015FCC
		internal override IReflect InferType(JSField inference_target)
		{
			if (this.value == null || this.value is DBNull)
			{
				return Typeob.Object;
			}
			if (this.value is ClassScope || this.value is TypedArray)
			{
				return Typeob.Type;
			}
			if (this.value is EnumWrapper)
			{
				return ((EnumWrapper)this.value).classScopeOrType;
			}
			return Globals.TypeRefs.ToReferenceContext(this.value.GetType());
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00017048 File Offset: 0x00016048
		internal bool IsAssignableTo(Type rtype)
		{
			bool flag;
			try
			{
				Convert.CoerceT(this.value, rtype, false);
				flag = true;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00017080 File Offset: 0x00016080
		internal override AST PartiallyEvaluate()
		{
			return this;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00017083 File Offset: 0x00016083
		public override string ToString()
		{
			return this.value.ToString();
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00017090 File Offset: 0x00016090
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if (rtype == Typeob.Void)
			{
				return;
			}
			object obj = this.value;
			if (obj is EnumWrapper && rtype != Typeob.Object && rtype != Typeob.String)
			{
				obj = ((EnumWrapper)obj).value;
			}
			if (this.isNumericLiteral && (rtype == Typeob.Decimal || rtype == Typeob.Int64 || rtype == Typeob.UInt64 || rtype == Typeob.Single))
			{
				obj = this.context.GetCode();
			}
			if (!(rtype is TypeBuilder))
			{
				try
				{
					obj = Convert.CoerceT(obj, rtype);
				}
				catch
				{
				}
			}
			this.TranslateToIL(il, obj, rtype);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00017134 File Offset: 0x00016134
		private void TranslateToIL(ILGenerator il, object val, Type rtype)
		{
			IConvertible iconvertible = Convert.GetIConvertible(val);
			switch (Convert.GetTypeCode(val, iconvertible))
			{
			case TypeCode.Empty:
				il.Emit(OpCodes.Ldnull);
				if (rtype.IsValueType)
				{
					Convert.Emit(this, il, Typeob.Object, rtype);
				}
				return;
			case TypeCode.DBNull:
				il.Emit(OpCodes.Ldsfld, Typeob.Null.GetField("Value"));
				Convert.Emit(this, il, Typeob.Null, rtype);
				return;
			case TypeCode.Boolean:
				ConstantWrapper.TranslateToILInt(il, iconvertible.ToInt32(null));
				Convert.Emit(this, il, Typeob.Boolean, rtype);
				return;
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
				ConstantWrapper.TranslateToILInt(il, iconvertible.ToInt32(null));
				if (rtype.IsEnum)
				{
					return;
				}
				if (val is EnumWrapper)
				{
					Convert.Emit(this, il, ((EnumWrapper)val).type, rtype);
					return;
				}
				Convert.Emit(this, il, Globals.TypeRefs.ToReferenceContext(val.GetType()), rtype);
				return;
			case TypeCode.UInt32:
				ConstantWrapper.TranslateToILInt(il, (int)iconvertible.ToUInt32(null));
				if (rtype.IsEnum)
				{
					return;
				}
				if (val is EnumWrapper)
				{
					Convert.Emit(this, il, ((EnumWrapper)val).type, rtype);
					return;
				}
				Convert.Emit(this, il, Typeob.UInt32, rtype);
				return;
			case TypeCode.Int64:
			{
				long num = iconvertible.ToInt64(null);
				if (-2147483648L <= num && num <= 2147483647L)
				{
					ConstantWrapper.TranslateToILInt(il, (int)num);
					il.Emit(OpCodes.Conv_I8);
				}
				else
				{
					il.Emit(OpCodes.Ldc_I8, num);
				}
				if (rtype.IsEnum)
				{
					return;
				}
				if (val is EnumWrapper)
				{
					Convert.Emit(this, il, ((EnumWrapper)val).type, rtype);
					return;
				}
				Convert.Emit(this, il, Typeob.Int64, rtype);
				return;
			}
			case TypeCode.UInt64:
			{
				ulong num2 = iconvertible.ToUInt64(null);
				if (num2 <= 2147483647UL)
				{
					ConstantWrapper.TranslateToILInt(il, (int)num2);
					il.Emit(OpCodes.Conv_I8);
				}
				else
				{
					il.Emit(OpCodes.Ldc_I8, (long)num2);
				}
				if (rtype.IsEnum)
				{
					return;
				}
				if (val is EnumWrapper)
				{
					Convert.Emit(this, il, ((EnumWrapper)val).type, rtype);
					return;
				}
				Convert.Emit(this, il, Typeob.UInt64, rtype);
				return;
			}
			case TypeCode.Single:
			{
				float num3 = iconvertible.ToSingle(null);
				if (num3 == num3 && (num3 != 0f || !float.IsNegativeInfinity(1f / num3)))
				{
					int num4 = (int)Runtime.DoubleToInt64((double)num3);
					if (-128 <= num4 && num4 <= 127 && num3 == (float)num4)
					{
						ConstantWrapper.TranslateToILInt(il, num4);
						il.Emit(OpCodes.Conv_R4);
					}
					else
					{
						il.Emit(OpCodes.Ldc_R4, num3);
					}
				}
				else
				{
					il.Emit(OpCodes.Ldc_R4, num3);
				}
				Convert.Emit(this, il, Typeob.Single, rtype);
				return;
			}
			case TypeCode.Double:
			{
				double num5 = iconvertible.ToDouble(null);
				if (num5 == num5 && (num5 != 0.0 || !double.IsNegativeInfinity(1.0 / num5)))
				{
					int num6 = (int)Runtime.DoubleToInt64(num5);
					if (-128 <= num6 && num6 <= 127 && num5 == (double)num6)
					{
						ConstantWrapper.TranslateToILInt(il, num6);
						il.Emit(OpCodes.Conv_R8);
					}
					else
					{
						il.Emit(OpCodes.Ldc_R8, num5);
					}
				}
				else
				{
					il.Emit(OpCodes.Ldc_R8, num5);
				}
				Convert.Emit(this, il, Typeob.Double, rtype);
				return;
			}
			case TypeCode.Decimal:
			{
				int[] bits = decimal.GetBits(iconvertible.ToDecimal(null));
				ConstantWrapper.TranslateToILInt(il, bits[0]);
				ConstantWrapper.TranslateToILInt(il, bits[1]);
				ConstantWrapper.TranslateToILInt(il, bits[2]);
				il.Emit((bits[3] < 0) ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
				ConstantWrapper.TranslateToILInt(il, (bits[3] & int.MaxValue) >> 16);
				il.Emit(OpCodes.Newobj, CompilerGlobals.decimalConstructor);
				Convert.Emit(this, il, Typeob.Decimal, rtype);
				return;
			}
			case TypeCode.DateTime:
			{
				long num = iconvertible.ToDateTime(null).Ticks;
				il.Emit(OpCodes.Ldc_I8, num);
				Convert.Emit(this, il, Typeob.Int64, rtype);
				return;
			}
			case TypeCode.String:
			{
				string text = iconvertible.ToString(null);
				if (rtype == Typeob.Char && text.Length == 1)
				{
					ConstantWrapper.TranslateToILInt(il, (int)text[0]);
					return;
				}
				il.Emit(OpCodes.Ldstr, text);
				Convert.Emit(this, il, Typeob.String, rtype);
				return;
			}
			}
			if (val is Enum)
			{
				if (rtype == Typeob.String)
				{
					this.TranslateToIL(il, val.ToString(), rtype);
					return;
				}
				if (rtype.IsPrimitive)
				{
					this.TranslateToIL(il, Convert.ChangeType(val, Enum.GetUnderlyingType(Globals.TypeRefs.ToReferenceContext(val.GetType())), CultureInfo.InvariantCulture), rtype);
					return;
				}
				Type type = Globals.TypeRefs.ToReferenceContext(val.GetType());
				Type underlyingType = Enum.GetUnderlyingType(type);
				this.TranslateToIL(il, Convert.ChangeType(val, underlyingType, CultureInfo.InvariantCulture), underlyingType);
				il.Emit(OpCodes.Box, type);
				Convert.Emit(this, il, Typeob.Object, rtype);
				return;
			}
			else if (val is EnumWrapper)
			{
				if (rtype == Typeob.String)
				{
					this.TranslateToIL(il, val.ToString(), rtype);
					return;
				}
				if (rtype.IsPrimitive)
				{
					this.TranslateToIL(il, ((EnumWrapper)val).ToNumericValue(), rtype);
					return;
				}
				Type type2 = ((EnumWrapper)val).type;
				Type type3 = Globals.TypeRefs.ToReferenceContext(((EnumWrapper)val).value.GetType());
				this.TranslateToIL(il, ((EnumWrapper)val).value, type3);
				il.Emit(OpCodes.Box, type2);
				Convert.Emit(this, il, Typeob.Object, rtype);
				return;
			}
			else
			{
				if (val is Type)
				{
					il.Emit(OpCodes.Ldtoken, (Type)val);
					il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
					Convert.Emit(this, il, Typeob.Type, rtype);
					return;
				}
				if (val is Namespace)
				{
					il.Emit(OpCodes.Ldstr, ((Namespace)val).Name);
					base.EmitILToLoadEngine(il);
					il.Emit(OpCodes.Call, CompilerGlobals.getNamespaceMethod);
					Convert.Emit(this, il, Typeob.Namespace, rtype);
					return;
				}
				if (val is ClassScope)
				{
					il.Emit(OpCodes.Ldtoken, ((ClassScope)val).GetTypeBuilderOrEnumBuilder());
					il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
					Convert.Emit(this, il, Typeob.Type, rtype);
					return;
				}
				if (val is TypedArray)
				{
					il.Emit(OpCodes.Ldtoken, Convert.ToType((TypedArray)val));
					il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
					Convert.Emit(this, il, Typeob.Type, rtype);
					return;
				}
				if (val is NumberObject)
				{
					this.TranslateToIL(il, ((NumberObject)val).value, Typeob.Object);
					base.EmitILToLoadEngine(il);
					il.Emit(OpCodes.Call, CompilerGlobals.toObjectMethod);
					Convert.Emit(this, il, Typeob.NumberObject, rtype);
					return;
				}
				if (val is StringObject)
				{
					il.Emit(OpCodes.Ldstr, ((StringObject)val).value);
					base.EmitILToLoadEngine(il);
					il.Emit(OpCodes.Call, CompilerGlobals.toObjectMethod);
					Convert.Emit(this, il, Typeob.StringObject, rtype);
					return;
				}
				if (val is BooleanObject)
				{
					il.Emit(((BooleanObject)val).value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
					il.Emit(OpCodes.Box, Typeob.Boolean);
					base.EmitILToLoadEngine(il);
					il.Emit(OpCodes.Call, CompilerGlobals.toObjectMethod);
					Convert.Emit(this, il, Typeob.BooleanObject, rtype);
					return;
				}
				if (val is ActiveXObjectConstructor)
				{
					il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("ActiveXObject").GetGetMethod());
					Convert.Emit(this, il, Typeob.ScriptFunction, rtype);
					return;
				}
				if (val is ArrayConstructor)
				{
					il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Array").GetGetMethod());
					Convert.Emit(this, il, Typeob.ScriptFunction, rtype);
					return;
				}
				if (val is BooleanConstructor)
				{
					il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Boolean").GetGetMethod());
					Convert.Emit(this, il, Typeob.ScriptFunction, rtype);
					return;
				}
				if (val is DateConstructor)
				{
					il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Date").GetGetMethod());
					Convert.Emit(this, il, Typeob.ScriptFunction, rtype);
					return;
				}
				if (val is EnumeratorConstructor)
				{
					il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Enumerator").GetGetMethod());
					Convert.Emit(this, il, Typeob.ScriptFunction, rtype);
					return;
				}
				if (val is ErrorConstructor)
				{
					ErrorConstructor errorConstructor = (ErrorConstructor)val;
					if (errorConstructor == ErrorConstructor.evalOb)
					{
						il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("EvalError").GetGetMethod());
					}
					else if (errorConstructor == ErrorConstructor.rangeOb)
					{
						il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("RangeError").GetGetMethod());
					}
					else if (errorConstructor == ErrorConstructor.referenceOb)
					{
						il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("ReferenceError").GetGetMethod());
					}
					else if (errorConstructor == ErrorConstructor.syntaxOb)
					{
						il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("SyntaxError").GetGetMethod());
					}
					else if (errorConstructor == ErrorConstructor.typeOb)
					{
						il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("TypeError").GetGetMethod());
					}
					else if (errorConstructor == ErrorConstructor.uriOb)
					{
						il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("URIError").GetGetMethod());
					}
					else
					{
						il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Error").GetGetMethod());
					}
					Convert.Emit(this, il, Typeob.ScriptFunction, rtype);
					return;
				}
				if (val is FunctionConstructor)
				{
					il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Function").GetGetMethod());
					Convert.Emit(this, il, Typeob.ScriptFunction, rtype);
					return;
				}
				if (val is MathObject)
				{
					il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Math").GetGetMethod());
					Convert.Emit(this, il, Typeob.JSObject, rtype);
					return;
				}
				if (val is NumberConstructor)
				{
					il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Number").GetGetMethod());
					Convert.Emit(this, il, Typeob.ScriptFunction, rtype);
					return;
				}
				if (val is ObjectConstructor)
				{
					il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("Object").GetGetMethod());
					Convert.Emit(this, il, Typeob.ScriptFunction, rtype);
					return;
				}
				if (val is RegExpConstructor)
				{
					il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("RegExp").GetGetMethod());
					Convert.Emit(this, il, Typeob.ScriptFunction, rtype);
					return;
				}
				if (val is StringConstructor)
				{
					il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("String").GetGetMethod());
					Convert.Emit(this, il, Typeob.ScriptFunction, rtype);
					return;
				}
				if (val is VBArrayConstructor)
				{
					il.Emit(OpCodes.Call, Typeob.GlobalObject.GetProperty("VBArray").GetGetMethod());
					Convert.Emit(this, il, Typeob.ScriptFunction, rtype);
					return;
				}
				if (val is IntPtr)
				{
					il.Emit(OpCodes.Ldc_I8, (long)((IntPtr)val));
					il.Emit(OpCodes.Conv_I);
					Convert.Emit(this, il, Typeob.IntPtr, rtype);
					return;
				}
				if (val is UIntPtr)
				{
					il.Emit(OpCodes.Ldc_I8, (long)(ulong)((UIntPtr)val));
					il.Emit(OpCodes.Conv_U);
					Convert.Emit(this, il, Typeob.UIntPtr, rtype);
					return;
				}
				if (val is Missing)
				{
					il.Emit(OpCodes.Ldsfld, CompilerGlobals.missingField);
					Convert.Emit(this, il, Typeob.Object, rtype);
					return;
				}
				if (val is Missing)
				{
					if (rtype.IsPrimitive)
					{
						this.TranslateToIL(il, double.NaN, rtype);
						return;
					}
					if (rtype != Typeob.Object && !rtype.IsValueType)
					{
						il.Emit(OpCodes.Ldnull);
						return;
					}
					il.Emit(OpCodes.Ldsfld, CompilerGlobals.systemReflectionMissingField);
					Convert.Emit(this, il, Typeob.Object, rtype);
					return;
				}
				else
				{
					if (val != this.value)
					{
						this.TranslateToIL(il, this.value, rtype);
						return;
					}
					throw new JScriptException(JSError.InternalError, this.context);
				}
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00017D3C File Offset: 0x00016D3C
		internal static void TranslateToILInt(ILGenerator il, int i)
		{
			switch (i)
			{
			case -1:
				il.Emit(OpCodes.Ldc_I4_M1);
				return;
			case 0:
				il.Emit(OpCodes.Ldc_I4_0);
				return;
			case 1:
				il.Emit(OpCodes.Ldc_I4_1);
				return;
			case 2:
				il.Emit(OpCodes.Ldc_I4_2);
				return;
			case 3:
				il.Emit(OpCodes.Ldc_I4_3);
				return;
			case 4:
				il.Emit(OpCodes.Ldc_I4_4);
				return;
			case 5:
				il.Emit(OpCodes.Ldc_I4_5);
				return;
			case 6:
				il.Emit(OpCodes.Ldc_I4_6);
				return;
			case 7:
				il.Emit(OpCodes.Ldc_I4_7);
				return;
			case 8:
				il.Emit(OpCodes.Ldc_I4_8);
				return;
			default:
				if (-128 <= i && i <= 127)
				{
					il.Emit(OpCodes.Ldc_I4_S, (sbyte)i);
					return;
				}
				il.Emit(OpCodes.Ldc_I4, i);
				return;
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00017E19 File Offset: 0x00016E19
		internal override void TranslateToILInitializer(ILGenerator il)
		{
		}

		// Token: 0x040001DD RID: 477
		internal object value;

		// Token: 0x040001DE RID: 478
		internal bool isNumericLiteral;
	}
}
