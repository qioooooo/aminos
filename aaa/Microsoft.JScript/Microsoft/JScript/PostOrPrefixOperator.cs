using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x020000FB RID: 251
	public class PostOrPrefixOperator : UnaryOp
	{
		// Token: 0x06000AD2 RID: 2770 RVA: 0x00053721 File Offset: 0x00052721
		internal PostOrPrefixOperator(Context context, AST operand)
			: base(context, operand)
		{
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0005372B File Offset: 0x0005272B
		internal PostOrPrefixOperator(Context context, AST operand, PostOrPrefix operatorTok)
			: base(context, operand)
		{
			this.operatorMeth = null;
			this.operatorTok = operatorTok;
			this.metaData = null;
			this.type = null;
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00053751 File Offset: 0x00052751
		public PostOrPrefixOperator(int operatorTok)
			: this(null, null, (PostOrPrefix)operatorTok)
		{
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0005375C File Offset: 0x0005275C
		private object DoOp(int i)
		{
			switch (this.operatorTok)
			{
			case PostOrPrefix.PostfixIncrement:
			case PostOrPrefix.PrefixIncrement:
				if (i == 2147483647)
				{
					return 2147483648.0;
				}
				return i + 1;
			}
			if (i == -2147483648)
			{
				return -2147483649.0;
			}
			return i - 1;
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x000537C8 File Offset: 0x000527C8
		private object DoOp(uint i)
		{
			switch (this.operatorTok)
			{
			case PostOrPrefix.PostfixIncrement:
			case PostOrPrefix.PrefixIncrement:
				if (i == 4294967295U)
				{
					return 4294967296.0;
				}
				return i + 1U;
			}
			if (i == 0U)
			{
				return -1.0;
			}
			return i - 1U;
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x00053828 File Offset: 0x00052828
		private object DoOp(long i)
		{
			switch (this.operatorTok)
			{
			case PostOrPrefix.PostfixIncrement:
			case PostOrPrefix.PrefixIncrement:
				if (i == 9223372036854775807L)
				{
					return 9.223372036854776E+18;
				}
				return i + 1L;
			}
			if (i == -9223372036854775808L)
			{
				return -9.223372036854776E+18;
			}
			return i - 1L;
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0005389C File Offset: 0x0005289C
		private object DoOp(ulong i)
		{
			switch (this.operatorTok)
			{
			case PostOrPrefix.PostfixIncrement:
			case PostOrPrefix.PrefixIncrement:
				if (i == 18446744073709551615UL)
				{
					return 1.8446744073709552E+19;
				}
				return i + 1UL;
			}
			if (i == 0UL)
			{
				return -1.0;
			}
			return i - 1UL;
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00053904 File Offset: 0x00052904
		private object DoOp(double d)
		{
			switch (this.operatorTok)
			{
			case PostOrPrefix.PostfixIncrement:
			case PostOrPrefix.PrefixIncrement:
				return d + 1.0;
			}
			return d - 1.0;
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00053950 File Offset: 0x00052950
		internal override object Evaluate()
		{
			object obj3;
			try
			{
				object obj = this.operand.Evaluate();
				object obj2 = this.EvaluatePostOrPrefix(ref obj);
				this.operand.SetValue(obj2);
				switch (this.operatorTok)
				{
				case PostOrPrefix.PostfixDecrement:
				case PostOrPrefix.PostfixIncrement:
					obj3 = obj;
					break;
				case PostOrPrefix.PrefixDecrement:
				case PostOrPrefix.PrefixIncrement:
					obj3 = obj2;
					break;
				default:
					throw new JScriptException(JSError.InternalError, this.context);
				}
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
			return obj3;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x00053A18 File Offset: 0x00052A18
		[DebuggerHidden]
		[DebuggerStepThrough]
		public object EvaluatePostOrPrefix(ref object v)
		{
			IConvertible iconvertible = Convert.GetIConvertible(v);
			double num5;
			switch (Convert.GetTypeCode(v, iconvertible))
			{
			case TypeCode.Empty:
				v = double.NaN;
				return v;
			case TypeCode.DBNull:
				v = 0;
				return this.DoOp(0);
			case TypeCode.Boolean:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			{
				int num;
				v = (num = iconvertible.ToInt32(null));
				return this.DoOp(num);
			}
			case TypeCode.Char:
			{
				int num = iconvertible.ToInt32(null);
				return ((IConvertible)this.DoOp(num)).ToChar(null);
			}
			case TypeCode.UInt32:
			{
				uint num2;
				v = (num2 = iconvertible.ToUInt32(null));
				return this.DoOp(num2);
			}
			case TypeCode.Int64:
			{
				long num3;
				v = (num3 = iconvertible.ToInt64(null));
				return this.DoOp(num3);
			}
			case TypeCode.UInt64:
			{
				ulong num4;
				v = (num4 = iconvertible.ToUInt64(null));
				return this.DoOp(num4);
			}
			case TypeCode.Single:
			case TypeCode.Double:
				v = (num5 = iconvertible.ToDouble(null));
				return this.DoOp(num5);
			}
			MethodInfo @operator = this.GetOperator(v.GetType());
			if (@operator != null)
			{
				return @operator.Invoke(null, BindingFlags.Default, JSBinder.ob, new object[] { v }, null);
			}
			v = (num5 = Convert.ToNumber(v, iconvertible));
			return this.DoOp(num5);
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x00053B94 File Offset: 0x00052B94
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
			switch (this.operatorTok)
			{
			case PostOrPrefix.PostfixDecrement:
			case PostOrPrefix.PrefixDecrement:
				this.operatorMeth = type.GetMethod("op_Decrement", BindingFlags.Static | BindingFlags.Public, JSBinder.ob, new Type[] { type }, null);
				break;
			case PostOrPrefix.PostfixIncrement:
			case PostOrPrefix.PrefixIncrement:
				this.operatorMeth = type.GetMethod("op_Increment", BindingFlags.Static | BindingFlags.Public, JSBinder.ob, new Type[] { type }, null);
				break;
			default:
				throw new JScriptException(JSError.InternalError, this.context);
			}
			if (this.operatorMeth != null && (!this.operatorMeth.IsStatic || (this.operatorMeth.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope || this.operatorMeth.GetParameters().Length != 1))
			{
				this.operatorMeth = null;
			}
			if (this.operatorMeth != null)
			{
				this.operatorMeth = new JSMethodInfo(this.operatorMeth);
			}
			return this.operatorMeth;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x00053CC8 File Offset: 0x00052CC8
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
			if (Convert.IsPrimitiveNumericType(this.type))
			{
				return this.type;
			}
			if (this.type == Typeob.Char)
			{
				return this.type;
			}
			if (Typeob.JSObject.IsAssignableFrom(this.type))
			{
				return Typeob.Double;
			}
			return Typeob.Object;
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x00053D58 File Offset: 0x00052D58
		internal override AST PartiallyEvaluate()
		{
			this.operand = this.operand.PartiallyEvaluateAsReference();
			this.operand.SetPartialValue(this);
			return this;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x00053D78 File Offset: 0x00052D78
		private void TranslateToILForNoOverloadCase(ILGenerator il, Type rtype)
		{
			Type type = Convert.ToType(this.operand.InferType(null));
			this.operand.TranslateToILPreSetPlusGet(il);
			if (rtype == Typeob.Void)
			{
				Type type2 = Typeob.Double;
				if (Convert.IsPrimitiveNumericType(type))
				{
					if (type == Typeob.SByte || type == Typeob.Int16)
					{
						type2 = Typeob.Int32;
					}
					else if (type == Typeob.Byte || type == Typeob.UInt16 || type == Typeob.Char)
					{
						type2 = Typeob.UInt32;
					}
					else
					{
						type2 = type;
					}
				}
				Convert.Emit(this, il, type, type2);
				il.Emit(OpCodes.Ldc_I4_1);
				Convert.Emit(this, il, Typeob.Int32, type2);
				if (type2 == Typeob.Double || type2 == Typeob.Single)
				{
					if (this.operatorTok == PostOrPrefix.PostfixDecrement || this.operatorTok == PostOrPrefix.PrefixDecrement)
					{
						il.Emit(OpCodes.Sub);
					}
					else
					{
						il.Emit(OpCodes.Add);
					}
				}
				else if (type2 == Typeob.Int32 || type2 == Typeob.Int64)
				{
					if (this.operatorTok == PostOrPrefix.PostfixDecrement || this.operatorTok == PostOrPrefix.PrefixDecrement)
					{
						il.Emit(OpCodes.Sub_Ovf);
					}
					else
					{
						il.Emit(OpCodes.Add_Ovf);
					}
				}
				else if (this.operatorTok == PostOrPrefix.PostfixDecrement || this.operatorTok == PostOrPrefix.PrefixDecrement)
				{
					il.Emit(OpCodes.Sub_Ovf_Un);
				}
				else
				{
					il.Emit(OpCodes.Add_Ovf_Un);
				}
				Convert.Emit(this, il, type2, type);
				this.operand.TranslateToILSet(il);
				return;
			}
			Type type3 = Typeob.Double;
			if (Convert.IsPrimitiveNumericType(rtype) && Convert.IsPromotableTo(type, rtype))
			{
				type3 = rtype;
			}
			else if (Convert.IsPrimitiveNumericType(type) && Convert.IsPromotableTo(rtype, type))
			{
				type3 = type;
			}
			if (type3 == Typeob.SByte || type3 == Typeob.Int16)
			{
				type3 = Typeob.Int32;
			}
			else if (type3 == Typeob.Byte || type3 == Typeob.UInt16 || type3 == Typeob.Char)
			{
				type3 = Typeob.UInt32;
			}
			LocalBuilder localBuilder = il.DeclareLocal(rtype);
			Convert.Emit(this, il, type, type3);
			if (this.operatorTok == PostOrPrefix.PostfixDecrement)
			{
				il.Emit(OpCodes.Dup);
				if (type == Typeob.Char)
				{
					Convert.Emit(this, il, type3, Typeob.Char);
					Convert.Emit(this, il, Typeob.Char, rtype);
				}
				else
				{
					Convert.Emit(this, il, type3, rtype);
				}
				il.Emit(OpCodes.Stloc, localBuilder);
				il.Emit(OpCodes.Ldc_I4_1);
				Convert.Emit(this, il, Typeob.Int32, type3);
				if (type3 == Typeob.Double || type3 == Typeob.Single)
				{
					il.Emit(OpCodes.Sub);
				}
				else if (type3 == Typeob.Int32 || type3 == Typeob.Int64)
				{
					il.Emit(OpCodes.Sub_Ovf);
				}
				else
				{
					il.Emit(OpCodes.Sub_Ovf_Un);
				}
			}
			else if (this.operatorTok == PostOrPrefix.PostfixIncrement)
			{
				il.Emit(OpCodes.Dup);
				if (type == Typeob.Char)
				{
					Convert.Emit(this, il, type3, Typeob.Char);
					Convert.Emit(this, il, Typeob.Char, rtype);
				}
				else
				{
					Convert.Emit(this, il, type3, rtype);
				}
				il.Emit(OpCodes.Stloc, localBuilder);
				il.Emit(OpCodes.Ldc_I4_1);
				Convert.Emit(this, il, Typeob.Int32, type3);
				if (type3 == Typeob.Double || type3 == Typeob.Single)
				{
					il.Emit(OpCodes.Add);
				}
				else if (type3 == Typeob.Int32 || type3 == Typeob.Int64)
				{
					il.Emit(OpCodes.Add_Ovf);
				}
				else
				{
					il.Emit(OpCodes.Add_Ovf_Un);
				}
			}
			else if (this.operatorTok == PostOrPrefix.PrefixDecrement)
			{
				il.Emit(OpCodes.Ldc_I4_1);
				Convert.Emit(this, il, Typeob.Int32, type3);
				if (type3 == Typeob.Double || type3 == Typeob.Single)
				{
					il.Emit(OpCodes.Sub);
				}
				else if (type3 == Typeob.Int32 || type3 == Typeob.Int64)
				{
					il.Emit(OpCodes.Sub_Ovf);
				}
				else
				{
					il.Emit(OpCodes.Sub_Ovf_Un);
				}
				il.Emit(OpCodes.Dup);
				if (type == Typeob.Char)
				{
					Convert.Emit(this, il, type3, Typeob.Char);
					Convert.Emit(this, il, Typeob.Char, rtype);
				}
				else
				{
					Convert.Emit(this, il, type3, rtype);
				}
				il.Emit(OpCodes.Stloc, localBuilder);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4_1);
				Convert.Emit(this, il, Typeob.Int32, type3);
				if (type3 == Typeob.Double || type3 == Typeob.Single)
				{
					il.Emit(OpCodes.Add);
				}
				else if (type3 == Typeob.Int32 || type3 == Typeob.Int64)
				{
					il.Emit(OpCodes.Add_Ovf);
				}
				else
				{
					il.Emit(OpCodes.Add_Ovf_Un);
				}
				il.Emit(OpCodes.Dup);
				if (type == Typeob.Char)
				{
					Convert.Emit(this, il, type3, Typeob.Char);
					Convert.Emit(this, il, Typeob.Char, rtype);
				}
				else
				{
					Convert.Emit(this, il, type3, rtype);
				}
				il.Emit(OpCodes.Stloc, localBuilder);
			}
			Convert.Emit(this, il, type3, type);
			this.operand.TranslateToILSet(il);
			il.Emit(OpCodes.Ldloc, localBuilder);
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x00054224 File Offset: 0x00053224
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
				Type type = Convert.ToType(this.operand.InferType(null));
				this.operand.TranslateToILPreSetPlusGet(il);
				if (rtype != Typeob.Void)
				{
					obj = il.DeclareLocal(rtype);
					if (this.operatorTok == PostOrPrefix.PostfixDecrement || this.operatorTok == PostOrPrefix.PostfixIncrement)
					{
						il.Emit(OpCodes.Dup);
						Convert.Emit(this, il, type, rtype);
						il.Emit(OpCodes.Stloc, (LocalBuilder)obj);
					}
				}
				MethodInfo methodInfo = (MethodInfo)this.metaData;
				ParameterInfo[] parameters = methodInfo.GetParameters();
				Convert.Emit(this, il, type, parameters[0].ParameterType);
				il.Emit(OpCodes.Call, methodInfo);
				if (rtype != Typeob.Void && (this.operatorTok == PostOrPrefix.PrefixDecrement || this.operatorTok == PostOrPrefix.PrefixIncrement))
				{
					il.Emit(OpCodes.Dup);
					Convert.Emit(this, il, type, rtype);
					il.Emit(OpCodes.Stloc, (LocalBuilder)obj);
				}
				Convert.Emit(this, il, methodInfo.ReturnType, type);
				this.operand.TranslateToILSet(il);
				if (rtype != Typeob.Void)
				{
					il.Emit(OpCodes.Ldloc, (LocalBuilder)obj);
					return;
				}
			}
			else
			{
				Type type2 = Convert.ToType(this.operand.InferType(null));
				LocalBuilder localBuilder = il.DeclareLocal(Typeob.Object);
				this.operand.TranslateToILPreSetPlusGet(il);
				Convert.Emit(this, il, type2, Typeob.Object);
				il.Emit(OpCodes.Stloc, localBuilder);
				il.Emit(OpCodes.Ldloc, (LocalBuilder)this.metaData);
				il.Emit(OpCodes.Ldloca, localBuilder);
				il.Emit(OpCodes.Call, CompilerGlobals.evaluatePostOrPrefixOperatorMethod);
				if (rtype != Typeob.Void && (this.operatorTok == PostOrPrefix.PrefixDecrement || this.operatorTok == PostOrPrefix.PrefixIncrement))
				{
					il.Emit(OpCodes.Dup);
					il.Emit(OpCodes.Stloc, localBuilder);
				}
				Convert.Emit(this, il, Typeob.Object, type2);
				this.operand.TranslateToILSet(il);
				if (rtype != Typeob.Void)
				{
					il.Emit(OpCodes.Ldloc, localBuilder);
					Convert.Emit(this, il, Typeob.Object, rtype);
				}
			}
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00054448 File Offset: 0x00053448
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			IReflect reflect = this.InferType(null);
			this.operand.TranslateToILInitializer(il);
			if (reflect != Typeob.Object)
			{
				return;
			}
			this.metaData = il.DeclareLocal(Typeob.PostOrPrefixOperator);
			ConstantWrapper.TranslateToILInt(il, (int)this.operatorTok);
			il.Emit(OpCodes.Newobj, CompilerGlobals.postOrPrefixConstructor);
			il.Emit(OpCodes.Stloc, (LocalBuilder)this.metaData);
		}

		// Token: 0x040006A4 RID: 1700
		private MethodInfo operatorMeth;

		// Token: 0x040006A5 RID: 1701
		private PostOrPrefix operatorTok;

		// Token: 0x040006A6 RID: 1702
		private object metaData;

		// Token: 0x040006A7 RID: 1703
		private Type type;
	}
}
