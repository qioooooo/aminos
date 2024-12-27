using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000080 RID: 128
	public sealed class ForIn : AST
	{
		// Token: 0x060005B9 RID: 1465 RVA: 0x00027F38 File Offset: 0x00026F38
		internal ForIn(Context context, AST var, AST initializer, AST collection, AST body)
			: base(context)
		{
			if (var != null)
			{
				this.var = var;
				this.inExpressionContext = this.var.context.Clone();
			}
			else
			{
				VariableDeclaration variableDeclaration = (VariableDeclaration)initializer;
				this.var = variableDeclaration.identifier;
				if (variableDeclaration.initializer == null)
				{
					variableDeclaration.initializer = new ConstantWrapper(null, null);
				}
				this.inExpressionContext = initializer.context.Clone();
			}
			this.initializer = initializer;
			this.collection = collection;
			this.inExpressionContext.UpdateWith(this.collection.context);
			this.body = body;
			this.completion = new Completion();
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00027FE0 File Offset: 0x00026FE0
		internal override object Evaluate()
		{
			AST ast = this.var;
			if (this.initializer != null)
			{
				this.initializer.Evaluate();
			}
			this.completion.Continue = 0;
			this.completion.Exit = 0;
			this.completion.value = null;
			object obj = Convert.ToForInObject(this.collection.Evaluate(), base.Engine);
			IEnumerator enumerator = null;
			try
			{
				enumerator = ForIn.JScriptGetEnumerator(obj);
				goto IL_00F4;
			}
			catch (JScriptException ex)
			{
				ex.context = this.collection.context;
				throw ex;
			}
			IL_0078:
			ast.SetValue(enumerator.Current);
			Completion completion = (Completion)this.body.Evaluate();
			this.completion.value = completion.value;
			if (completion.Continue > 1)
			{
				this.completion.Continue = completion.Continue - 1;
				goto IL_00FF;
			}
			if (completion.Exit > 0)
			{
				this.completion.Exit = completion.Exit - 1;
				goto IL_00FF;
			}
			if (completion.Return)
			{
				return completion;
			}
			IL_00F4:
			if (enumerator.MoveNext())
			{
				goto IL_0078;
			}
			IL_00FF:
			return this.completion;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00028104 File Offset: 0x00027104
		public static IEnumerator JScriptGetEnumerator(object coll)
		{
			if (coll is IEnumerator)
			{
				return (IEnumerator)coll;
			}
			if (coll is ScriptObject)
			{
				return new ScriptObjectPropertyEnumerator((ScriptObject)coll);
			}
			if (coll is Array)
			{
				Array array = (Array)coll;
				return new RangeEnumerator(array.GetLowerBound(0), array.GetUpperBound(0));
			}
			if (!(coll is IEnumerable))
			{
				throw new JScriptException(JSError.NotCollection);
			}
			IEnumerator enumerator = ((IEnumerable)coll).GetEnumerator();
			if (enumerator != null)
			{
				return enumerator;
			}
			return new ScriptObjectPropertyEnumerator(new JSObject());
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00028188 File Offset: 0x00027188
		internal override AST PartiallyEvaluate()
		{
			this.var = this.var.PartiallyEvaluateAsReference();
			this.var.SetPartialValue(new ConstantWrapper(null, null));
			if (this.initializer != null)
			{
				this.initializer = this.initializer.PartiallyEvaluate();
			}
			this.collection = this.collection.PartiallyEvaluate();
			IReflect reflect = this.collection.InferType(null);
			if ((reflect is ClassScope && ((ClassScope)reflect).noExpando && !((ClassScope)reflect).ImplementsInterface(Typeob.IEnumerable)) || (reflect != Typeob.Object && reflect is Type && !Typeob.ScriptObject.IsAssignableFrom((Type)reflect) && !Typeob.IEnumerable.IsAssignableFrom((Type)reflect) && !Typeob.IConvertible.IsAssignableFrom((Type)reflect) && !Typeob.IEnumerator.IsAssignableFrom((Type)reflect)))
			{
				this.collection.context.HandleError(JSError.NotCollection);
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

		// Token: 0x060005BD RID: 1469 RVA: 0x000282E8 File Offset: 0x000272E8
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			Label label3 = il.DefineLabel();
			base.compilerGlobals.BreakLabelStack.Push(label2);
			base.compilerGlobals.ContinueLabelStack.Push(label);
			if (this.initializer != null)
			{
				this.initializer.TranslateToIL(il, Typeob.Void);
			}
			this.inExpressionContext.EmitLineInfo(il);
			this.collection.TranslateToIL(il, Typeob.Object);
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.toForInObjectMethod);
			il.Emit(OpCodes.Call, CompilerGlobals.jScriptGetEnumeratorMethod);
			LocalBuilder localBuilder = il.DeclareLocal(Typeob.IEnumerator);
			il.Emit(OpCodes.Stloc, localBuilder);
			il.Emit(OpCodes.Br, label);
			il.MarkLabel(label3);
			this.body.TranslateToIL(il, Typeob.Void);
			il.MarkLabel(label);
			this.context.EmitLineInfo(il);
			il.Emit(OpCodes.Ldloc, localBuilder);
			il.Emit(OpCodes.Callvirt, CompilerGlobals.moveNextMethod);
			il.Emit(OpCodes.Brfalse, label2);
			il.Emit(OpCodes.Ldloc, localBuilder);
			il.Emit(OpCodes.Callvirt, CompilerGlobals.getCurrentMethod);
			Type type = Convert.ToType(this.var.InferType(null));
			LocalBuilder localBuilder2 = il.DeclareLocal(type);
			Convert.Emit(this, il, Typeob.Object, type);
			il.Emit(OpCodes.Stloc, localBuilder2);
			this.var.TranslateToILPreSet(il);
			il.Emit(OpCodes.Ldloc, localBuilder2);
			this.var.TranslateToILSet(il);
			il.Emit(OpCodes.Br, label3);
			il.MarkLabel(label2);
			base.compilerGlobals.BreakLabelStack.Pop();
			base.compilerGlobals.ContinueLabelStack.Pop();
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000284B8 File Offset: 0x000274B8
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.var.TranslateToILInitializer(il);
			if (this.initializer != null)
			{
				this.initializer.TranslateToILInitializer(il);
			}
			this.collection.TranslateToILInitializer(il);
			this.body.TranslateToILInitializer(il);
		}

		// Token: 0x0400027A RID: 634
		private AST var;

		// Token: 0x0400027B RID: 635
		private AST initializer;

		// Token: 0x0400027C RID: 636
		private AST collection;

		// Token: 0x0400027D RID: 637
		private AST body;

		// Token: 0x0400027E RID: 638
		private Completion completion;

		// Token: 0x0400027F RID: 639
		private Context inExpressionContext;
	}
}
