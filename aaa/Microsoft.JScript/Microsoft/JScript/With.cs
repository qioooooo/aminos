using System;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000146 RID: 326
	public sealed class With : AST
	{
		// Token: 0x06000EFA RID: 3834 RVA: 0x00064D94 File Offset: 0x00063D94
		internal With(Context context, AST obj, AST block)
			: base(context)
		{
			this.obj = obj;
			this.block = block;
			this.completion = new Completion();
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			if (scriptObject is FunctionScope)
			{
				this.enclosing_function = (FunctionScope)scriptObject;
				return;
			}
			this.enclosing_function = null;
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00064DF0 File Offset: 0x00063DF0
		internal override object Evaluate()
		{
			try
			{
				With.JScriptWith(this.obj.Evaluate(), base.Engine);
			}
			catch (JScriptException ex)
			{
				ex.context = this.obj.context;
				throw ex;
			}
			Completion completion = null;
			try
			{
				completion = (Completion)this.block.Evaluate();
			}
			finally
			{
				base.Globals.ScopeStack.Pop();
			}
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

		// Token: 0x06000EFC RID: 3836 RVA: 0x00064ED4 File Offset: 0x00063ED4
		public static object JScriptWith(object withOb, VsaEngine engine)
		{
			object obj = Convert.ToObject(withOb, engine);
			if (obj == null)
			{
				throw new JScriptException(JSError.ObjectExpected);
			}
			Globals globals = engine.Globals;
			globals.ScopeStack.GuardedPush(new WithObject(globals.ScopeStack.Peek(), obj));
			return obj;
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00064F1C File Offset: 0x00063F1C
		internal override AST PartiallyEvaluate()
		{
			this.obj = this.obj.PartiallyEvaluate();
			WithObject withObject;
			if (this.obj is ConstantWrapper)
			{
				object obj = Convert.ToObject(this.obj.Evaluate(), base.Engine);
				withObject = new WithObject(base.Globals.ScopeStack.Peek(), obj);
				if (obj is JSObject && ((JSObject)obj).noExpando)
				{
					withObject.isKnownAtCompileTime = true;
				}
			}
			else
			{
				withObject = new WithObject(base.Globals.ScopeStack.Peek(), new JSObject(null, false));
			}
			base.Globals.ScopeStack.Push(withObject);
			try
			{
				this.block = this.block.PartiallyEvaluate();
			}
			finally
			{
				base.Globals.ScopeStack.Pop();
			}
			return this;
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00064FF8 File Offset: 0x00063FF8
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.context.EmitLineInfo(il);
			base.Globals.ScopeStack.Push(new WithObject(base.Globals.ScopeStack.Peek(), new JSObject(null, false)));
			bool insideProtectedRegion = base.compilerGlobals.InsideProtectedRegion;
			base.compilerGlobals.InsideProtectedRegion = true;
			Label label = il.DefineLabel();
			base.compilerGlobals.BreakLabelStack.Push(label);
			base.compilerGlobals.ContinueLabelStack.Push(label);
			this.obj.TranslateToIL(il, Typeob.Object);
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.jScriptWithMethod);
			LocalBuilder localBuilder = null;
			if (this.context.document.debugOn)
			{
				il.BeginScope();
				localBuilder = il.DeclareLocal(Typeob.Object);
				localBuilder.SetLocalSymInfo("with()");
				il.Emit(OpCodes.Stloc, localBuilder);
			}
			else
			{
				il.Emit(OpCodes.Pop);
			}
			il.BeginExceptionBlock();
			this.block.TranslateToILInitializer(il);
			this.block.TranslateToIL(il, Typeob.Void);
			il.BeginFinallyBlock();
			if (this.context.document.debugOn)
			{
				il.Emit(OpCodes.Ldnull);
				il.Emit(OpCodes.Stloc, localBuilder);
			}
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.popScriptObjectMethod);
			il.Emit(OpCodes.Pop);
			il.EndExceptionBlock();
			if (this.context.document.debugOn)
			{
				il.EndScope();
			}
			il.MarkLabel(label);
			base.compilerGlobals.BreakLabelStack.Pop();
			base.compilerGlobals.ContinueLabelStack.Pop();
			base.compilerGlobals.InsideProtectedRegion = insideProtectedRegion;
			base.Globals.ScopeStack.Pop();
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x000651D3 File Offset: 0x000641D3
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.obj.TranslateToILInitializer(il);
		}

		// Token: 0x040007F2 RID: 2034
		private AST obj;

		// Token: 0x040007F3 RID: 2035
		private AST block;

		// Token: 0x040007F4 RID: 2036
		private Completion completion;

		// Token: 0x040007F5 RID: 2037
		private FunctionScope enclosing_function;
	}
}
