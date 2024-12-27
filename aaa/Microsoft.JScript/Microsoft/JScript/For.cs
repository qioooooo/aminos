using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200007F RID: 127
	internal sealed class For : AST
	{
		// Token: 0x060005B4 RID: 1460 RVA: 0x00027B67 File Offset: 0x00026B67
		internal For(Context context, AST initializer, AST condition, AST incrementer, AST body)
			: base(context)
		{
			this.initializer = initializer;
			this.condition = condition;
			this.incrementer = incrementer;
			this.body = body;
			this.completion = new Completion();
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00027B9C File Offset: 0x00026B9C
		internal override object Evaluate()
		{
			this.completion.Continue = 0;
			this.completion.Exit = 0;
			this.completion.value = null;
			this.initializer.Evaluate();
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
				this.incrementer.Evaluate();
			}
			return this.completion;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00027C74 File Offset: 0x00026C74
		internal override AST PartiallyEvaluate()
		{
			this.initializer = this.initializer.PartiallyEvaluate();
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (scriptObject is WithObject)
			{
				scriptObject = scriptObject.GetParent();
			}
			if (scriptObject is FunctionScope)
			{
				FunctionScope functionScope = (FunctionScope)scriptObject;
				BitArray definedFlags = functionScope.DefinedFlags;
				this.condition = this.condition.PartiallyEvaluate();
				this.body = this.body.PartiallyEvaluate();
				functionScope.DefinedFlags = definedFlags;
				this.incrementer = this.incrementer.PartiallyEvaluate();
				functionScope.DefinedFlags = definedFlags;
			}
			else
			{
				this.condition = this.condition.PartiallyEvaluate();
				this.body = this.body.PartiallyEvaluate();
				this.incrementer = this.incrementer.PartiallyEvaluate();
			}
			IReflect reflect = this.condition.InferType(null);
			if (reflect is FunctionPrototype || reflect == Typeob.ScriptFunction)
			{
				this.context.HandleError(JSError.SuspectLoopCondition);
			}
			return this;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00027D70 File Offset: 0x00026D70
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			il.DefineLabel();
			Label label3 = il.DefineLabel();
			bool flag = false;
			base.compilerGlobals.BreakLabelStack.Push(label3);
			base.compilerGlobals.ContinueLabelStack.Push(label2);
			if (!(this.initializer is EmptyLiteral))
			{
				this.initializer.context.EmitLineInfo(il);
				this.initializer.TranslateToIL(il, Typeob.Void);
			}
			il.MarkLabel(label);
			if (!(this.condition is ConstantWrapper) || !(this.condition.Evaluate() is bool) || !(bool)this.condition.Evaluate())
			{
				this.condition.context.EmitLineInfo(il);
				this.condition.TranslateToConditionalBranch(il, false, label3, false);
			}
			else if (this.condition.context.StartPosition + 1 == this.condition.context.EndPosition)
			{
				flag = true;
			}
			this.body.TranslateToIL(il, Typeob.Void);
			il.MarkLabel(label2);
			if (!(this.incrementer is EmptyLiteral))
			{
				this.incrementer.context.EmitLineInfo(il);
				this.incrementer.TranslateToIL(il, Typeob.Void);
			}
			else if (flag)
			{
				this.context.EmitLineInfo(il);
			}
			il.Emit(OpCodes.Br, label);
			il.MarkLabel(label3);
			base.compilerGlobals.BreakLabelStack.Pop();
			base.compilerGlobals.ContinueLabelStack.Pop();
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00027F03 File Offset: 0x00026F03
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.initializer.TranslateToILInitializer(il);
			this.condition.TranslateToILInitializer(il);
			this.incrementer.TranslateToILInitializer(il);
			this.body.TranslateToILInitializer(il);
		}

		// Token: 0x04000275 RID: 629
		private AST initializer;

		// Token: 0x04000276 RID: 630
		private AST condition;

		// Token: 0x04000277 RID: 631
		private AST incrementer;

		// Token: 0x04000278 RID: 632
		private AST body;

		// Token: 0x04000279 RID: 633
		private Completion completion;
	}
}
