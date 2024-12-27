using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000145 RID: 325
	internal sealed class While : AST
	{
		// Token: 0x06000EF5 RID: 3829 RVA: 0x00064B28 File Offset: 0x00063B28
		internal While(Context context, AST condition, AST body)
			: base(context)
		{
			this.condition = condition;
			this.body = body;
			this.completion = new Completion();
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00064B4C File Offset: 0x00063B4C
		internal override object Evaluate()
		{
			this.completion.Continue = 0;
			this.completion.Exit = 0;
			this.completion.value = null;
			while (Convert.ToBoolean(this.condition.Evaluate()))
			{
				Completion completion = (Completion)this.body.Evaluate();
				if (completion.value != null)
				{
					this.completion.value = completion.value;
				}
				if (completion.Continue > 1)
				{
					this.completion.Continue = completion.Continue - 1;
					break;
				}
				if (completion.Exit > 0)
				{
					this.completion.Exit = completion.Exit - 1;
					break;
				}
				if (completion.Return)
				{
					return completion;
				}
			}
			return this.completion;
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00064C0C File Offset: 0x00063C0C
		internal override AST PartiallyEvaluate()
		{
			this.condition = this.condition.PartiallyEvaluate();
			IReflect reflect = this.condition.InferType(null);
			if (reflect is FunctionPrototype || reflect == Typeob.ScriptFunction)
			{
				this.context.HandleError(JSError.SuspectLoopCondition);
			}
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
			}
			else
			{
				this.body = this.body.PartiallyEvaluate();
			}
			return this;
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00064CBC File Offset: 0x00063CBC
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			Label label3 = il.DefineLabel();
			base.compilerGlobals.BreakLabelStack.Push(label2);
			base.compilerGlobals.ContinueLabelStack.Push(label);
			il.Emit(OpCodes.Br, label);
			il.MarkLabel(label3);
			this.body.TranslateToIL(il, Typeob.Void);
			il.MarkLabel(label);
			this.context.EmitLineInfo(il);
			this.condition.TranslateToConditionalBranch(il, true, label3, false);
			il.MarkLabel(label2);
			base.compilerGlobals.BreakLabelStack.Pop();
			base.compilerGlobals.ContinueLabelStack.Pop();
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00064D79 File Offset: 0x00063D79
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.condition.TranslateToILInitializer(il);
			this.body.TranslateToILInitializer(il);
		}

		// Token: 0x040007EF RID: 2031
		private AST condition;

		// Token: 0x040007F0 RID: 2032
		private AST body;

		// Token: 0x040007F1 RID: 2033
		private Completion completion;
	}
}
