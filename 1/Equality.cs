using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000075 RID: 117
	public class Equality : BinaryOp
	{
		// Token: 0x0600057E RID: 1406 RVA: 0x00026163 File Offset: 0x00025163
		internal Equality(Context context, AST operand1, AST operand2, JSToken operatorTok)
			: base(context, operand1, operand2, operatorTok)
		{
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00026170 File Offset: 0x00025170
		public Equality(int operatorTok)
			: base(null, null, null, (JSToken)operatorTok)
		{
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0002617C File Offset: 0x0002517C
		internal override object Evaluate()
		{
			bool flag = this.EvaluateEquality(this.operand1.Evaluate(), this.operand2.Evaluate(), VsaEngine.executeForJSEE);
			if (this.operatorTok == JSToken.Equal)
			{
				return flag;
			}
			return !flag;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x000261C5 File Offset: 0x000251C5
		[DebuggerStepThrough]
		[DebuggerHidden]
		public bool EvaluateEquality(object v1, object v2)
		{
			return this.EvaluateEquality(v1, v2, false);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x000261D0 File Offset: 0x000251D0
		[DebuggerHidden]
		[DebuggerStepThrough]
		private bool EvaluateEquality(object v1, object v2, bool checkForDebuggerObjects)
		{
			if (v1 is string && v2 is string)
			{
				return v1.Equals(v2);
			}
			if (v1 is int && v2 is int)
			{
				return (int)v1 == (int)v2;
			}
			if (v1 is double && v2 is double)
			{
				return (double)v1 == (double)v2;
			}
			if ((v2 == null || v2 is DBNull || v2 is Missing) && !checkForDebuggerObjects)
			{
				return v1 == null || v1 is DBNull || v1 is Missing;
			}
			IConvertible iconvertible = Convert.GetIConvertible(v1);
			IConvertible iconvertible2 = Convert.GetIConvertible(v2);
			TypeCode typeCode = Convert.GetTypeCode(v1, iconvertible);
			TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
			switch (typeCode)
			{
			case TypeCode.Empty:
			case TypeCode.DBNull:
				break;
			case TypeCode.Object:
				switch (typeCode2)
				{
				case TypeCode.Empty:
				case TypeCode.DBNull:
					break;
				default:
				{
					MethodInfo @operator = base.GetOperator(v1.GetType(), v2.GetType());
					if (@operator != null)
					{
						bool flag = (bool)@operator.Invoke(null, BindingFlags.Default, JSBinder.ob, new object[] { v1, v2 }, null);
						if (this.operatorTok == JSToken.NotEqual)
						{
							return !flag;
						}
						return flag;
					}
					break;
				}
				}
				break;
			default:
			{
				TypeCode typeCode3 = typeCode2;
				if (typeCode3 == TypeCode.Object)
				{
					MethodInfo operator2 = base.GetOperator(v1.GetType(), v2.GetType());
					if (operator2 != null)
					{
						bool flag2 = (bool)operator2.Invoke(null, BindingFlags.Default, JSBinder.ob, new object[] { v1, v2 }, null);
						if (this.operatorTok == JSToken.NotEqual)
						{
							return !flag2;
						}
						return flag2;
					}
				}
				break;
			}
			}
			return Equality.JScriptEquals(v1, v2, iconvertible, iconvertible2, typeCode, typeCode2, checkForDebuggerObjects);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00026370 File Offset: 0x00025370
		public static bool JScriptEquals(object v1, object v2)
		{
			if (v1 is string && v2 is string)
			{
				return v1.Equals(v2);
			}
			if (v1 is int && v2 is int)
			{
				return (int)v1 == (int)v2;
			}
			if (v1 is double && v2 is double)
			{
				return (double)v1 == (double)v2;
			}
			if (v2 == null || v2 is DBNull || v2 is Missing)
			{
				return v1 == null || v1 is DBNull || v1 is Missing;
			}
			IConvertible iconvertible = Convert.GetIConvertible(v1);
			IConvertible iconvertible2 = Convert.GetIConvertible(v2);
			TypeCode typeCode = Convert.GetTypeCode(v1, iconvertible);
			TypeCode typeCode2 = Convert.GetTypeCode(v2, iconvertible2);
			return Equality.JScriptEquals(v1, v2, iconvertible, iconvertible2, typeCode, typeCode2, false);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00026428 File Offset: 0x00025428
		private static bool JScriptEquals(object v1, object v2, IConvertible ic1, IConvertible ic2, TypeCode t1, TypeCode t2, bool checkForDebuggerObjects)
		{
			if (StrictEquality.JScriptStrictEquals(v1, v2, ic1, ic2, t1, t2, checkForDebuggerObjects))
			{
				return true;
			}
			if (t2 == TypeCode.Boolean)
			{
				v2 = (ic2.ToBoolean(null) ? 1 : 0);
				ic2 = Convert.GetIConvertible(v2);
				return Equality.JScriptEquals(v1, v2, ic1, ic2, t1, TypeCode.Int32, false);
			}
			switch (t1)
			{
			case TypeCode.Empty:
				return t2 == TypeCode.Empty || t2 == TypeCode.DBNull || (t2 == TypeCode.Object && v2 is Missing);
			case TypeCode.Object:
				switch (t2)
				{
				case TypeCode.Empty:
				case TypeCode.DBNull:
					return v1 is Missing;
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
				case TypeCode.Decimal:
				case TypeCode.String:
				{
					IConvertible convertible = ic1;
					object obj = Convert.ToPrimitive(v1, PreferredType.Either, ref convertible);
					return convertible != null && obj != v1 && Equality.JScriptEquals(obj, v2, convertible, ic2, convertible.GetTypeCode(), t2, false);
				}
				}
				return false;
			case TypeCode.DBNull:
				return t2 == TypeCode.DBNull || t2 == TypeCode.Empty || (t2 == TypeCode.Object && v2 is Missing);
			case TypeCode.Boolean:
				v1 = (ic1.ToBoolean(null) ? 1 : 0);
				ic1 = Convert.GetIConvertible(v1);
				return Equality.JScriptEquals(v1, v2, ic1, ic2, TypeCode.Int32, t2, false);
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
			case TypeCode.Decimal:
				if (t2 == TypeCode.Object)
				{
					IConvertible convertible2 = ic2;
					object obj2 = Convert.ToPrimitive(v2, PreferredType.Either, ref convertible2);
					return convertible2 != null && obj2 != v2 && Equality.JScriptEquals(v1, obj2, ic1, convertible2, t1, convertible2.GetTypeCode(), false);
				}
				if (t2 != TypeCode.String)
				{
					return false;
				}
				if (v1 is Enum)
				{
					return Convert.ToString(v1).Equals(ic2.ToString(null));
				}
				v2 = Convert.ToNumber(v2, ic2);
				ic2 = Convert.GetIConvertible(v2);
				return StrictEquality.JScriptStrictEquals(v1, v2, ic1, ic2, t1, TypeCode.Double, false);
			case TypeCode.DateTime:
				if (t2 == TypeCode.Object)
				{
					IConvertible convertible3 = ic2;
					object obj3 = Convert.ToPrimitive(v2, PreferredType.Either, ref convertible3);
					if (obj3 != null && obj3 != v2)
					{
						return StrictEquality.JScriptStrictEquals(v1, obj3, ic1, convertible3, t1, convertible3.GetTypeCode(), false);
					}
				}
				return false;
			case TypeCode.String:
				switch (t2)
				{
				case TypeCode.Object:
				{
					IConvertible convertible4 = ic2;
					object obj4 = Convert.ToPrimitive(v2, PreferredType.Either, ref convertible4);
					return convertible4 != null && obj4 != v2 && Equality.JScriptEquals(v1, obj4, ic1, convertible4, t1, convertible4.GetTypeCode(), false);
				}
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
				case TypeCode.Decimal:
					if (v2 is Enum)
					{
						return Convert.ToString(v2).Equals(ic1.ToString(null));
					}
					v1 = Convert.ToNumber(v1, ic1);
					ic1 = Convert.GetIConvertible(v1);
					return StrictEquality.JScriptStrictEquals(v1, v2, ic1, ic2, TypeCode.Double, t2, false);
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0002672A File Offset: 0x0002572A
		internal override IReflect InferType(JSField inference_target)
		{
			return Typeob.Boolean;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00026734 File Offset: 0x00025734
		internal override void TranslateToConditionalBranch(ILGenerator il, bool branchIfTrue, Label label, bool shortForm)
		{
			if (this.metaData == null)
			{
				Type type = this.type1;
				Type type2 = this.type2;
				Type type3 = Typeob.Object;
				bool flag = true;
				if (type.IsPrimitive && type2.IsPrimitive)
				{
					type3 = Typeob.Double;
					if (type == Typeob.Single || type2 == Typeob.Single)
					{
						type3 = Typeob.Single;
					}
					else if (Convert.IsPromotableTo(type, type2))
					{
						type3 = type2;
					}
					else if (Convert.IsPromotableTo(type2, type))
					{
						type3 = type;
					}
				}
				else if (type == Typeob.String && (type2 == Typeob.String || type2 == Typeob.Empty || type2 == Typeob.Null))
				{
					type3 = Typeob.String;
					if (type2 != Typeob.String)
					{
						flag = false;
						branchIfTrue = !branchIfTrue;
					}
				}
				else if ((type == Typeob.Empty || type == Typeob.Null) && type2 == Typeob.String)
				{
					type3 = Typeob.String;
					flag = false;
					branchIfTrue = !branchIfTrue;
				}
				if (type3 == Typeob.SByte || type3 == Typeob.Int16)
				{
					type3 = Typeob.Int32;
				}
				else if (type3 == Typeob.Byte || type3 == Typeob.UInt16)
				{
					type3 = Typeob.UInt32;
				}
				if (flag)
				{
					this.operand1.TranslateToIL(il, type3);
					this.operand2.TranslateToIL(il, type3);
					if (type3 == Typeob.Object)
					{
						il.Emit(OpCodes.Call, CompilerGlobals.jScriptEqualsMethod);
					}
					else if (type3 == Typeob.String)
					{
						il.Emit(OpCodes.Call, CompilerGlobals.stringEqualsMethod);
					}
				}
				else if (type == Typeob.String)
				{
					this.operand1.TranslateToIL(il, type3);
				}
				else if (type2 == Typeob.String)
				{
					this.operand2.TranslateToIL(il, type3);
				}
				if (branchIfTrue)
				{
					if (this.operatorTok == JSToken.Equal)
					{
						if (type3 == Typeob.String || type3 == Typeob.Object)
						{
							il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
							return;
						}
						il.Emit(shortForm ? OpCodes.Beq_S : OpCodes.Beq, label);
						return;
					}
					else
					{
						if (type3 == Typeob.String || type3 == Typeob.Object)
						{
							il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
							return;
						}
						il.Emit(shortForm ? OpCodes.Bne_Un_S : OpCodes.Bne_Un, label);
						return;
					}
				}
				else if (this.operatorTok == JSToken.Equal)
				{
					if (type3 == Typeob.String || type3 == Typeob.Object)
					{
						il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
						return;
					}
					il.Emit(shortForm ? OpCodes.Bne_Un_S : OpCodes.Bne_Un, label);
					return;
				}
				else
				{
					if (type3 == Typeob.String || type3 == Typeob.Object)
					{
						il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
						return;
					}
					il.Emit(shortForm ? OpCodes.Beq_S : OpCodes.Beq, label);
					return;
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
				il.Emit(OpCodes.Call, CompilerGlobals.evaluateEqualityMethod);
				if (branchIfTrue)
				{
					if (this.operatorTok == JSToken.Equal)
					{
						il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
						return;
					}
					il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
					return;
				}
				else
				{
					if (this.operatorTok == JSToken.Equal)
					{
						il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
						return;
					}
					il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
					return;
				}
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00026B1C File Offset: 0x00025B1C
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

		// Token: 0x06000588 RID: 1416 RVA: 0x00026B80 File Offset: 0x00025B80
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
			if (this.operand1 is ConstantWrapper)
			{
				object obj = this.operand1.Evaluate();
				if (obj == null)
				{
					this.type1 = Typeob.Empty;
				}
				else if (obj is DBNull)
				{
					this.type1 = Typeob.Null;
				}
			}
			if (this.operand2 is ConstantWrapper)
			{
				object obj2 = this.operand2.Evaluate();
				if (obj2 == null)
				{
					this.type2 = Typeob.Empty;
				}
				else if (obj2 is DBNull)
				{
					this.type2 = Typeob.Null;
				}
			}
			if (this.type1 == Typeob.Empty || this.type1 == Typeob.Null || this.type2 == Typeob.Empty || this.type2 == Typeob.Null)
			{
				return;
			}
			if ((this.type1.IsPrimitive || this.type1 == Typeob.String || Typeob.JSObject.IsAssignableFrom(this.type1)) && (this.type2.IsPrimitive || this.type2 == Typeob.String || Typeob.JSObject.IsAssignableFrom(this.type2)))
			{
				return;
			}
			this.metaData = il.DeclareLocal(Typeob.Equality);
			ConstantWrapper.TranslateToILInt(il, (int)this.operatorTok);
			il.Emit(OpCodes.Newobj, CompilerGlobals.equalityConstructor);
			il.Emit(OpCodes.Stloc, (LocalBuilder)this.metaData);
		}

		// Token: 0x04000254 RID: 596
		private object metaData;
	}
}
