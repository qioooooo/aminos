using System;
using System.Collections;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200004B RID: 75
	internal sealed class Conditional : AST
	{
		// Token: 0x0600039F RID: 927 RVA: 0x00016855 File Offset: 0x00015855
		internal Conditional(Context context, AST condition, AST operand1, AST operand2)
			: base(context)
		{
			this.condition = condition;
			this.operand1 = operand1;
			this.operand2 = operand2;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00016874 File Offset: 0x00015874
		internal override object Evaluate()
		{
			if (Convert.ToBoolean(this.condition.Evaluate()))
			{
				return this.operand1.Evaluate();
			}
			return this.operand2.Evaluate();
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x000168A0 File Offset: 0x000158A0
		internal override AST PartiallyEvaluate()
		{
			this.condition = this.condition.PartiallyEvaluate();
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (scriptObject is WithObject)
			{
				scriptObject = scriptObject.GetParent();
			}
			if (scriptObject is FunctionScope)
			{
				FunctionScope functionScope = (FunctionScope)scriptObject;
				BitArray definedFlags = functionScope.DefinedFlags;
				this.operand1 = this.operand1.PartiallyEvaluate();
				BitArray definedFlags2 = functionScope.DefinedFlags;
				functionScope.DefinedFlags = definedFlags;
				this.operand2 = this.operand2.PartiallyEvaluate();
				BitArray definedFlags3 = functionScope.DefinedFlags;
				int length = definedFlags2.Length;
				int length2 = definedFlags3.Length;
				if (length < length2)
				{
					definedFlags2.Length = length2;
				}
				if (length2 < length)
				{
					definedFlags3.Length = length;
				}
				functionScope.DefinedFlags = definedFlags2.And(definedFlags3);
			}
			else
			{
				this.operand1 = this.operand1.PartiallyEvaluate();
				this.operand2 = this.operand2.PartiallyEvaluate();
			}
			return this;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00016994 File Offset: 0x00015994
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			this.condition.TranslateToConditionalBranch(il, false, label, false);
			this.operand1.TranslateToIL(il, rtype);
			il.Emit(OpCodes.Br, label2);
			il.MarkLabel(label);
			this.operand2.TranslateToIL(il, rtype);
			il.MarkLabel(label2);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x000169F2 File Offset: 0x000159F2
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.condition.TranslateToILInitializer(il);
			this.operand1.TranslateToILInitializer(il);
			this.operand2.TranslateToILInitializer(il);
		}

		// Token: 0x040001D0 RID: 464
		private AST condition;

		// Token: 0x040001D1 RID: 465
		private AST operand1;

		// Token: 0x040001D2 RID: 466
		private AST operand2;
	}
}
