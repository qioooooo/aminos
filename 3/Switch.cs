using System;
using System.Collections;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200011C RID: 284
	internal sealed class Switch : AST
	{
		// Token: 0x06000BAB RID: 2987 RVA: 0x00058C88 File Offset: 0x00057C88
		internal Switch(Context context, AST expression, ASTList cases)
			: base(context)
		{
			this.expression = expression;
			this.cases = cases;
			this.default_case = -1;
			int i = 0;
			int count = this.cases.count;
			while (i < count)
			{
				if (((SwitchCase)this.cases[i]).IsDefault())
				{
					this.default_case = i;
					break;
				}
				i++;
			}
			this.completion = new Completion();
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00058CF8 File Offset: 0x00057CF8
		internal override object Evaluate()
		{
			this.completion.Continue = 0;
			this.completion.Exit = 0;
			this.completion.value = null;
			object obj = this.expression.Evaluate();
			Completion completion = null;
			int count = this.cases.count;
			int i;
			for (i = 0; i < count; i++)
			{
				if (i != this.default_case)
				{
					completion = ((SwitchCase)this.cases[i]).Evaluate(obj);
					if (completion != null)
					{
						break;
					}
				}
			}
			if (completion == null)
			{
				if (this.default_case < 0)
				{
					return this.completion;
				}
				i = this.default_case;
				completion = (Completion)((SwitchCase)this.cases[i]).Evaluate();
			}
			for (;;)
			{
				if (completion.value != null)
				{
					this.completion.value = completion.value;
				}
				if (completion.Continue > 0)
				{
					break;
				}
				if (completion.Exit > 0)
				{
					goto Block_6;
				}
				if (completion.Return)
				{
					return completion;
				}
				if (i >= count - 1)
				{
					goto Block_8;
				}
				completion = (Completion)((SwitchCase)this.cases[++i]).Evaluate();
			}
			this.completion.Continue = completion.Continue - 1;
			goto IL_0137;
			Block_6:
			this.completion.Exit = completion.Exit - 1;
			goto IL_0137;
			Block_8:
			return this.completion;
			IL_0137:
			return this.completion;
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x00058E44 File Offset: 0x00057E44
		internal override AST PartiallyEvaluate()
		{
			this.expression = this.expression.PartiallyEvaluate();
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (scriptObject is WithObject)
			{
				scriptObject = scriptObject.GetParent();
			}
			if (scriptObject is FunctionScope)
			{
				FunctionScope functionScope = (FunctionScope)scriptObject;
				BitArray definedFlags = functionScope.DefinedFlags;
				int i = 0;
				int count = this.cases.count;
				while (i < count)
				{
					this.cases[i] = this.cases[i].PartiallyEvaluate();
					functionScope.DefinedFlags = definedFlags;
					i++;
				}
			}
			else
			{
				int j = 0;
				int count2 = this.cases.count;
				while (j < count2)
				{
					this.cases[j] = this.cases[j].PartiallyEvaluate();
					j++;
				}
			}
			return this;
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x00058F18 File Offset: 0x00057F18
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Type type = Convert.ToType(this.expression.InferType(null));
			this.expression.context.EmitLineInfo(il);
			this.expression.TranslateToIL(il, type);
			LocalBuilder localBuilder = il.DeclareLocal(type);
			il.Emit(OpCodes.Stloc, localBuilder);
			int count = this.cases.count;
			Label[] array = new Label[this.cases.count];
			for (int i = 0; i < count; i++)
			{
				array[i] = il.DefineLabel();
				if (i != this.default_case)
				{
					il.Emit(OpCodes.Ldloc, localBuilder);
					((SwitchCase)this.cases[i]).TranslateToConditionalBranch(il, type, true, array[i], false);
				}
			}
			Label label = il.DefineLabel();
			if (this.default_case >= 0)
			{
				il.Emit(OpCodes.Br, array[this.default_case]);
			}
			else
			{
				il.Emit(OpCodes.Br, label);
			}
			base.compilerGlobals.BreakLabelStack.Push(label);
			base.compilerGlobals.ContinueLabelStack.Push(label);
			for (int j = 0; j < count; j++)
			{
				il.MarkLabel(array[j]);
				this.cases[j].TranslateToIL(il, Typeob.Void);
			}
			il.MarkLabel(label);
			base.compilerGlobals.BreakLabelStack.Pop();
			base.compilerGlobals.ContinueLabelStack.Pop();
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x000590B8 File Offset: 0x000580B8
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.expression.TranslateToILInitializer(il);
			int i = 0;
			int count = this.cases.count;
			while (i < count)
			{
				this.cases[i].TranslateToILInitializer(il);
				i++;
			}
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x000590FB File Offset: 0x000580FB
		internal override Context GetFirstExecutableContext()
		{
			return this.expression.context;
		}

		// Token: 0x040006FD RID: 1789
		private AST expression;

		// Token: 0x040006FE RID: 1790
		private ASTList cases;

		// Token: 0x040006FF RID: 1791
		private int default_case;

		// Token: 0x04000700 RID: 1792
		private Completion completion;
	}
}
