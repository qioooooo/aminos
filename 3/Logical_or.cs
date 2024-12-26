using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x020000E4 RID: 228
	internal sealed class Logical_or : BinaryOp
	{
		// Token: 0x06000A15 RID: 2581 RVA: 0x0004C29F File Offset: 0x0004B29F
		internal Logical_or(Context context, AST operand1, AST operand2)
			: base(context, operand1, operand2)
		{
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0004C2AC File Offset: 0x0004B2AC
		internal override object Evaluate()
		{
			object obj = this.operand1.Evaluate();
			MethodInfo methodInfo = null;
			Type type = null;
			if (obj != null && !(obj is IConvertible))
			{
				type = obj.GetType();
				methodInfo = type.GetMethod("op_True", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { type }, null);
				if (methodInfo == null || (methodInfo.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope || methodInfo.ReturnType != Typeob.Boolean)
				{
					methodInfo = null;
				}
			}
			if (methodInfo == null)
			{
				if (Convert.ToBoolean(obj))
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
						MethodInfo methodInfo2 = type.GetMethod("op_BitwiseOr", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { type, type }, null);
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

		// Token: 0x06000A17 RID: 2583 RVA: 0x0004C3F4 File Offset: 0x0004B3F4
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

		// Token: 0x06000A18 RID: 2584 RVA: 0x0004C428 File Offset: 0x0004B428
		internal override void TranslateToConditionalBranch(ILGenerator il, bool branchIfTrue, Label label, bool shortForm)
		{
			Label label2 = il.DefineLabel();
			if (branchIfTrue)
			{
				this.operand1.TranslateToConditionalBranch(il, true, label, shortForm);
				this.operand2.TranslateToConditionalBranch(il, true, label, shortForm);
				return;
			}
			this.operand1.TranslateToConditionalBranch(il, true, label2, shortForm);
			this.operand2.TranslateToConditionalBranch(il, false, label, shortForm);
			il.MarkLabel(label2);
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0004C488 File Offset: 0x0004B488
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Type type = Convert.ToType(this.operand1.InferType(null));
			Type type2 = Convert.ToType(this.operand2.InferType(null));
			if (type != type2)
			{
				type = Typeob.Object;
			}
			MethodInfo methodInfo = type.GetMethod("op_True", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { type }, null);
			if (methodInfo == null || (methodInfo.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope || methodInfo.ReturnType != Typeob.Boolean)
			{
				methodInfo = null;
			}
			MethodInfo methodInfo2 = null;
			if (methodInfo != null)
			{
				methodInfo2 = type.GetMethod("op_BitwiseOr", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { type, type }, null);
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
			il.Emit(OpCodes.Brtrue, label);
			il.Emit(OpCodes.Pop);
			this.operand2.TranslateToIL(il, type);
			il.MarkLabel(label);
			Convert.Emit(this, il, type, rtype);
		}
	}
}
