using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000036 RID: 54
	internal sealed class CallableExpression : Binding
	{
		// Token: 0x06000223 RID: 547 RVA: 0x00010E98 File Offset: 0x0000FE98
		internal CallableExpression(AST expression)
			: base(expression.context, "")
		{
			this.expression = expression;
			JSLocalField jslocalField = new JSLocalField("", null, 0, Missing.Value);
			this.expressionInferredType = expression.InferType(jslocalField);
			jslocalField.inferred_type = this.expressionInferredType;
			this.member = jslocalField;
			this.members = new MemberInfo[] { jslocalField };
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00010F01 File Offset: 0x0000FF01
		internal override LateBinding EvaluateAsLateBinding()
		{
			return new LateBinding(null, this.expression.Evaluate(), VsaEngine.executeForJSEE);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00010F19 File Offset: 0x0000FF19
		protected override object GetObject()
		{
			return this.GetObject2();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00010F24 File Offset: 0x0000FF24
		internal object GetObject2()
		{
			Call call = this.expression as Call;
			if (call == null || !call.inBrackets)
			{
				return Convert.ToObject(this.expression.Evaluate(), base.Engine);
			}
			return Convert.ToObject(call.func.Evaluate(), base.Engine);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00010F75 File Offset: 0x0000FF75
		protected override void HandleNoSuchMemberError()
		{
			throw new JScriptException(JSError.InternalError, this.context);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00010F84 File Offset: 0x0000FF84
		internal override AST PartiallyEvaluate()
		{
			return this;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00010F87 File Offset: 0x0000FF87
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.expression.TranslateToIL(il, rtype);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00010F98 File Offset: 0x0000FF98
		internal override void TranslateToILCall(ILGenerator il, Type rtype, ASTList argList, bool construct, bool brackets)
		{
			if (this.defaultMember != null && construct && brackets)
			{
				base.TranslateToILCall(il, rtype, argList, construct, brackets);
				return;
			}
			JSGlobalField jsglobalField = this.member as JSGlobalField;
			if (jsglobalField != null && jsglobalField.IsLiteral && argList.count == 1)
			{
				Type type = Convert.ToType((IReflect)jsglobalField.value);
				argList[0].TranslateToIL(il, type);
				Convert.Emit(this, il, type, rtype);
				return;
			}
			this.TranslateToILWithDupOfThisOb(il);
			argList.TranslateToIL(il, Typeob.ArrayOfObject);
			if (construct)
			{
				il.Emit(OpCodes.Ldc_I4_1);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}
			if (brackets)
			{
				il.Emit(OpCodes.Ldc_I4_1);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.callValueMethod);
			Convert.Emit(this, il, Typeob.Object, rtype);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0001107B File Offset: 0x0001007B
		protected override void TranslateToILObject(ILGenerator il, Type obType, bool noValue)
		{
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
			il.Emit(OpCodes.Castclass, Typeob.IActivationObject);
			il.Emit(OpCodes.Callvirt, CompilerGlobals.getGlobalScopeMethod);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000110B4 File Offset: 0x000100B4
		protected override void TranslateToILWithDupOfThisOb(ILGenerator il)
		{
			Call call = this.expression as Call;
			if (call == null || !call.inBrackets)
			{
				this.TranslateToILObject(il, null, false);
			}
			else
			{
				if (call.isConstructor && call.inBrackets)
				{
					call.TranslateToIL(il, Typeob.Object);
					il.Emit(OpCodes.Dup);
					return;
				}
				call.func.TranslateToIL(il, Typeob.Object);
			}
			this.expression.TranslateToIL(il, Typeob.Object);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00011130 File Offset: 0x00010130
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.expression.TranslateToILInitializer(il);
			if (!this.expressionInferredType.Equals(this.expression.InferType(null)))
			{
				MemberInfo[] members = this.members;
				base.InvalidateBinding();
				this.members = members;
			}
		}

		// Token: 0x0400014A RID: 330
		internal AST expression;

		// Token: 0x0400014B RID: 331
		private IReflect expressionInferredType;
	}
}
