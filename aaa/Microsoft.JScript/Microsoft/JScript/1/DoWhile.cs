using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200006B RID: 107
	internal sealed class DoWhile : AST
	{
		// Token: 0x0600053B RID: 1339 RVA: 0x00025528 File Offset: 0x00024528
		internal DoWhile(Context context, AST body, AST condition)
			: base(context)
		{
			this.body = body;
			this.condition = condition;
			this.completion = new Completion();
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0002554C File Offset: 0x0002454C
		internal override object Evaluate()
		{
			this.completion.Continue = 0;
			this.completion.Exit = 0;
			this.completion.value = null;
			Completion completion;
			for (;;)
			{
				completion = (Completion)this.body.Evaluate();
				if (completion.value != null)
				{
					this.completion.value = completion.value;
				}
				if (completion.Continue > 1)
				{
					break;
				}
				if (completion.Exit > 0)
				{
					goto Block_3;
				}
				if (completion.Return)
				{
					return completion;
				}
				if (!Convert.ToBoolean(this.condition.Evaluate()))
				{
					goto IL_00A9;
				}
			}
			this.completion.Continue = completion.Continue - 1;
			goto IL_00A9;
			Block_3:
			this.completion.Exit = completion.Exit - 1;
			IL_00A9:
			return this.completion;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00025608 File Offset: 0x00024608
		internal override AST PartiallyEvaluate()
		{
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (scriptObject is WithObject)
			{
				scriptObject = scriptObject.GetParent();
			}
			if (scriptObject is FunctionScope)
			{
				FunctionScope functionScope = (FunctionScope)scriptObject;
				BitArray definedFlags = functionScope.DefinedFlags;
				this.body = this.body.PartiallyEvaluate();
				functionScope.DefinedFlags = definedFlags;
				this.condition = this.condition.PartiallyEvaluate();
				functionScope.DefinedFlags = definedFlags;
			}
			else
			{
				this.body = this.body.PartiallyEvaluate();
				this.condition = this.condition.PartiallyEvaluate();
			}
			IReflect reflect = this.condition.InferType(null);
			if (reflect is FunctionPrototype || reflect == Typeob.ScriptFunction)
			{
				this.context.HandleError(JSError.SuspectLoopCondition);
			}
			return this;
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x000256D0 File Offset: 0x000246D0
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			Label label3 = il.DefineLabel();
			base.compilerGlobals.BreakLabelStack.Push(label3);
			base.compilerGlobals.ContinueLabelStack.Push(label2);
			il.MarkLabel(label);
			this.body.TranslateToIL(il, Typeob.Void);
			il.MarkLabel(label2);
			this.context.EmitLineInfo(il);
			this.condition.TranslateToConditionalBranch(il, true, label, false);
			il.MarkLabel(label3);
			base.compilerGlobals.BreakLabelStack.Pop();
			base.compilerGlobals.ContinueLabelStack.Pop();
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00025781 File Offset: 0x00024781
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.body.TranslateToILInitializer(il);
			this.condition.TranslateToILInitializer(il);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0002579B File Offset: 0x0002479B
		internal override Context GetFirstExecutableContext()
		{
			return this.body.GetFirstExecutableContext();
		}

		// Token: 0x04000243 RID: 579
		private AST body;

		// Token: 0x04000244 RID: 580
		private AST condition;

		// Token: 0x04000245 RID: 581
		private Completion completion;
	}
}
