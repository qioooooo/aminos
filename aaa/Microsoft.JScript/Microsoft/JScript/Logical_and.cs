using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x020000E3 RID: 227
	internal sealed class Logical_and : BinaryOp
	{
		// Token: 0x06000A10 RID: 2576 RVA: 0x0004BF3B File Offset: 0x0004AF3B
		internal Logical_and(Context context, AST operand1, AST operand2)
			: base(context, operand1, operand2)
		{
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0004BF48 File Offset: 0x0004AF48
		internal override object Evaluate()
		{
			object obj = this.operand1.Evaluate();
			MethodInfo methodInfo = null;
			Type type = null;
			if (obj != null && !(obj is IConvertible))
			{
				type = obj.GetType();
				methodInfo = type.GetMethod("op_False", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { type }, null);
				if (methodInfo == null || (methodInfo.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope || methodInfo.ReturnType != Typeob.Boolean)
				{
					methodInfo = null;
				}
			}
			if (methodInfo == null)
			{
				if (!Convert.ToBoolean(obj))
				{
					return obj;
				}
				return this.operand2.Evaluate();
			}
			else
			{
				methodInfo = new JSMethodInfo(methodInfo);
				if ((bool)methodInfo.Invoke(null, BindingFlags.SuppressChangeType, null, new object[] { obj }, null))
				{
					return obj;
				}
				object obj2 = this.operand2.Evaluate();
				if (obj2 != null && !(obj2 is IConvertible))
				{
					Type type2 = obj2.GetType();
					if (type == type2)
					{
						MethodInfo methodInfo2 = type.GetMethod("op_BitwiseAnd", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { type, type }, null);
						if (methodInfo2 != null && (methodInfo2.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope)
						{
							methodInfo2 = new JSMethodInfo(methodInfo2);
							return methodInfo2.Invoke(null, BindingFlags.SuppressChangeType, null, new object[] { obj, obj2 }, null);
						}
					}
				}
				return obj2;
			}
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0004C090 File Offset: 0x0004B090
		internal override IReflect InferType(JSField inference_target)
		{
			IReflect reflect = this.operand1.InferType(inference_target);
			IReflect reflect2 = this.operand2.InferType(inference_target);
			if (reflect == reflect2)
			{
				return reflect;
			}
			return Typeob.Object;
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0004C0C4 File Offset: 0x0004B0C4
		internal override void TranslateToConditionalBranch(ILGenerator il, bool branchIfTrue, Label label, bool shortForm)
		{
			Label label2 = il.DefineLabel();
			if (branchIfTrue)
			{
				this.operand1.TranslateToConditionalBranch(il, false, label2, shortForm);
				this.operand2.TranslateToConditionalBranch(il, true, label, shortForm);
				il.MarkLabel(label2);
				return;
			}
			this.operand1.TranslateToConditionalBranch(il, false, label, shortForm);
			this.operand2.TranslateToConditionalBranch(il, false, label, shortForm);
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0004C124 File Offset: 0x0004B124
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Type type = Convert.ToType(this.operand1.InferType(null));
			Type type2 = Convert.ToType(this.operand2.InferType(null));
			if (type != type2)
			{
				type = Typeob.Object;
			}
			MethodInfo methodInfo = type.GetMethod("op_False", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { type }, null);
			if (methodInfo == null || (methodInfo.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope || methodInfo.ReturnType != Typeob.Boolean)
			{
				methodInfo = null;
			}
			MethodInfo methodInfo2 = null;
			if (methodInfo != null)
			{
				methodInfo2 = type.GetMethod("op_BitwiseAnd", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { type, type }, null);
			}
			if (methodInfo2 == null || (methodInfo2.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope)
			{
				methodInfo = null;
			}
			Label label = il.DefineLabel();
			this.operand1.TranslateToIL(il, type);
			il.Emit(OpCodes.Dup);
			if (methodInfo != null)
			{
				if (type.IsValueType)
				{
					Convert.EmitLdloca(il, type);
				}
				il.Emit(OpCodes.Call, methodInfo);
				il.Emit(OpCodes.Brtrue, label);
				this.operand2.TranslateToIL(il, type);
				il.Emit(OpCodes.Call, methodInfo2);
				il.MarkLabel(label);
				Convert.Emit(this, il, methodInfo2.ReturnType, rtype);
				return;
			}
			Convert.Emit(this, il, type, Typeob.Boolean, true);
			il.Emit(OpCodes.Brfalse, label);
			il.Emit(OpCodes.Pop);
			this.operand2.TranslateToIL(il, type);
			il.MarkLabel(label);
			Convert.Emit(this, il, type, rtype);
		}
	}
}
