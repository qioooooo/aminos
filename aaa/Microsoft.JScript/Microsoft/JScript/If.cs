using System;
using System.Collections;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200008E RID: 142
	internal sealed class If : AST
	{
		// Token: 0x0600068C RID: 1676 RVA: 0x0002E383 File Offset: 0x0002D383
		internal If(Context context, AST condition, AST true_branch, AST false_branch)
			: base(context)
		{
			this.condition = condition;
			this.operand1 = true_branch;
			this.operand2 = false_branch;
			this.completion = new Completion();
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0002E3B0 File Offset: 0x0002D3B0
		internal override object Evaluate()
		{
			if (this.operand1 == null && this.operand2 == null)
			{
				return this.completion;
			}
			Completion completion;
			if (this.condition != null)
			{
				if (Convert.ToBoolean(this.condition.Evaluate()))
				{
					completion = (Completion)this.operand1.Evaluate();
				}
				else if (this.operand2 != null)
				{
					completion = (Completion)this.operand2.Evaluate();
				}
				else
				{
					completion = new Completion();
				}
			}
			else if (this.operand1 != null)
			{
				completion = (Completion)this.operand1.Evaluate();
			}
			else
			{
				completion = (Completion)this.operand2.Evaluate();
			}
			this.completion.value = completion.value;
			if (completion.Continue > 1)
			{
				this.completion.Continue = completion.Continue - 1;
			}
			else
			{
				this.completion.Continue = 0;
			}
			if (completion.Exit > 0)
			{
				this.completion.Exit = completion.Exit - 1;
			}
			else
			{
				this.completion.Exit = 0;
			}
			if (completion.Return)
			{
				return completion;
			}
			return this.completion;
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0002E4C8 File Offset: 0x0002D4C8
		internal override bool HasReturn()
		{
			if (this.operand1 != null)
			{
				return this.operand1.HasReturn() && this.operand2 != null && this.operand2.HasReturn();
			}
			return this.operand2 != null && this.operand2.HasReturn();
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0002E518 File Offset: 0x0002D518
		internal override AST PartiallyEvaluate()
		{
			this.condition = this.condition.PartiallyEvaluate();
			if (this.condition is ConstantWrapper)
			{
				if (Convert.ToBoolean(this.condition.Evaluate()))
				{
					this.operand2 = null;
				}
				else
				{
					this.operand1 = null;
				}
				this.condition = null;
			}
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (scriptObject is WithObject)
			{
				scriptObject = scriptObject.GetParent();
			}
			if (scriptObject is FunctionScope)
			{
				FunctionScope functionScope = (FunctionScope)scriptObject;
				BitArray bitArray = functionScope.DefinedFlags;
				BitArray bitArray2 = bitArray;
				if (this.operand1 != null)
				{
					this.operand1 = this.operand1.PartiallyEvaluate();
					bitArray2 = functionScope.DefinedFlags;
					functionScope.DefinedFlags = bitArray;
				}
				if (this.operand2 != null)
				{
					this.operand2 = this.operand2.PartiallyEvaluate();
					BitArray definedFlags = functionScope.DefinedFlags;
					int length = bitArray2.Length;
					int length2 = definedFlags.Length;
					if (length < length2)
					{
						bitArray2.Length = length2;
					}
					if (length2 < length)
					{
						definedFlags.Length = length;
					}
					bitArray = bitArray2.And(definedFlags);
				}
				functionScope.DefinedFlags = bitArray;
			}
			else
			{
				if (this.operand1 != null)
				{
					this.operand1 = this.operand1.PartiallyEvaluate();
				}
				if (this.operand2 != null)
				{
					this.operand2 = this.operand2.PartiallyEvaluate();
				}
			}
			return this;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0002E664 File Offset: 0x0002D664
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if (this.operand1 == null && this.operand2 == null)
			{
				return;
			}
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			base.compilerGlobals.BreakLabelStack.Push(label2);
			base.compilerGlobals.ContinueLabelStack.Push(label2);
			if (this.condition != null)
			{
				this.context.EmitLineInfo(il);
				if (this.operand2 != null)
				{
					this.condition.TranslateToConditionalBranch(il, false, label, false);
				}
				else
				{
					this.condition.TranslateToConditionalBranch(il, false, label2, false);
				}
				if (this.operand1 != null)
				{
					this.operand1.TranslateToIL(il, Typeob.Void);
				}
				if (this.operand2 != null)
				{
					if (this.operand1 != null && !this.operand1.HasReturn())
					{
						il.Emit(OpCodes.Br, label2);
					}
					il.MarkLabel(label);
					this.operand2.TranslateToIL(il, Typeob.Void);
				}
			}
			else if (this.operand1 != null)
			{
				this.operand1.TranslateToIL(il, Typeob.Void);
			}
			else
			{
				this.operand2.TranslateToIL(il, Typeob.Void);
			}
			il.MarkLabel(label2);
			base.compilerGlobals.BreakLabelStack.Pop();
			base.compilerGlobals.ContinueLabelStack.Pop();
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0002E7AC File Offset: 0x0002D7AC
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			if (this.condition != null)
			{
				this.condition.TranslateToILInitializer(il);
			}
			if (this.operand1 != null)
			{
				this.operand1.TranslateToILInitializer(il);
			}
			if (this.operand2 != null)
			{
				this.operand2.TranslateToILInitializer(il);
			}
		}

		// Token: 0x04000300 RID: 768
		private AST condition;

		// Token: 0x04000301 RID: 769
		private AST operand1;

		// Token: 0x04000302 RID: 770
		private AST operand2;

		// Token: 0x04000303 RID: 771
		private Completion completion;
	}
}
