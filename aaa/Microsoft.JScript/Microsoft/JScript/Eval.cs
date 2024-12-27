using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Permissions;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x0200007A RID: 122
	public sealed class Eval : AST
	{
		// Token: 0x0600059D RID: 1437 RVA: 0x00027304 File Offset: 0x00026304
		internal Eval(Context context, AST operand, AST unsafeOption)
			: base(context)
		{
			this.operand = operand;
			this.unsafeOption = unsafeOption;
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			((IActivationObject)scriptObject).GetGlobalScope().evilScript = true;
			if (scriptObject is ActivationObject)
			{
				((ActivationObject)scriptObject).isKnownAtCompileTime = base.Engine.doFast;
			}
			if (scriptObject is FunctionScope)
			{
				this.enclosingFunctionScope = (FunctionScope)scriptObject;
				this.enclosingFunctionScope.mustSaveStackLocals = true;
				for (ScriptObject scriptObject2 = this.enclosingFunctionScope.GetParent(); scriptObject2 != null; scriptObject2 = scriptObject2.GetParent())
				{
					FunctionScope functionScope = scriptObject2 as FunctionScope;
					if (functionScope != null)
					{
						functionScope.mustSaveStackLocals = true;
						functionScope.closuresMightEscape = true;
					}
				}
				return;
			}
			this.enclosingFunctionScope = null;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x000273BE File Offset: 0x000263BE
		internal override void CheckIfOKToUseInSuperConstructorCall()
		{
			this.context.HandleError(JSError.NotAllowedInSuperConstructorCall);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x000273D0 File Offset: 0x000263D0
		internal override object Evaluate()
		{
			if (VsaEngine.executeForJSEE)
			{
				throw new JScriptException(JSError.NonSupportedInDebugger);
			}
			object obj = this.operand.Evaluate();
			object obj2 = null;
			if (this.unsafeOption != null)
			{
				obj2 = this.unsafeOption.Evaluate();
			}
			base.Globals.CallContextStack.Push(new CallContext(this.context, null, new object[] { obj, obj2 }));
			object obj3;
			try
			{
				obj3 = Eval.JScriptEvaluate(obj, obj2, base.Engine);
			}
			catch (JScriptException ex)
			{
				if (ex.context == null)
				{
					ex.context = this.context;
				}
				throw ex;
			}
			catch (Exception ex2)
			{
				throw new JScriptException(ex2, this.context);
			}
			catch
			{
				throw new JScriptException(JSError.NonClsException, this.context);
			}
			finally
			{
				base.Globals.CallContextStack.Pop();
			}
			return obj3;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x000274D4 File Offset: 0x000264D4
		public static object JScriptEvaluate(object source, VsaEngine engine)
		{
			if (Convert.GetTypeCode(source) != TypeCode.String)
			{
				return source;
			}
			return Eval.DoEvaluate(source, engine, true);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x000274EC File Offset: 0x000264EC
		public static object JScriptEvaluate(object source, object unsafeOption, VsaEngine engine)
		{
			if (Convert.GetTypeCode(source) != TypeCode.String)
			{
				return source;
			}
			bool flag = false;
			if (Convert.GetTypeCode(unsafeOption) == TypeCode.String && ((IConvertible)unsafeOption).ToString() == "unsafe")
			{
				flag = true;
			}
			return Eval.DoEvaluate(source, engine, flag);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00027534 File Offset: 0x00026534
		private static object DoEvaluate(object source, VsaEngine engine, bool isUnsafe)
		{
			if (engine.doFast)
			{
				engine.PushScriptObject(new BlockScope(engine.ScriptObjectStackTop()));
			}
			object value;
			try
			{
				Context context = new Context(new DocumentContext("eval code", engine), ((IConvertible)source).ToString());
				JSParser jsparser = new JSParser(context);
				if (!isUnsafe)
				{
					new SecurityPermission(SecurityPermissionFlag.Execution).PermitOnly();
				}
				value = ((Completion)jsparser.ParseEvalBody().PartiallyEvaluate().Evaluate()).value;
			}
			finally
			{
				if (engine.doFast)
				{
					engine.PopScriptObject();
				}
			}
			return value;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x000275CC File Offset: 0x000265CC
		internal override AST PartiallyEvaluate()
		{
			VsaEngine engine = base.Engine;
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			ClassScope classScope = ClassScope.ScopeOfClassMemberInitializer(scriptObject);
			if (classScope != null)
			{
				if (classScope.inStaticInitializerCode)
				{
					classScope.staticInitializerUsesEval = true;
				}
				else
				{
					classScope.instanceInitializerUsesEval = true;
				}
			}
			if (engine.doFast)
			{
				engine.PushScriptObject(new BlockScope(scriptObject));
			}
			else
			{
				while (scriptObject is WithObject || scriptObject is BlockScope)
				{
					if (scriptObject is BlockScope)
					{
						((BlockScope)scriptObject).isKnownAtCompileTime = false;
					}
					scriptObject = scriptObject.GetParent();
				}
			}
			try
			{
				this.operand = this.operand.PartiallyEvaluate();
				if (this.unsafeOption != null)
				{
					this.unsafeOption = this.unsafeOption.PartiallyEvaluate();
				}
				if (this.enclosingFunctionScope != null && this.enclosingFunctionScope.owner == null)
				{
					this.context.HandleError(JSError.NotYetImplemented);
				}
			}
			finally
			{
				if (engine.doFast)
				{
					base.Engine.PopScriptObject();
				}
			}
			return this;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x000276D0 File Offset: 0x000266D0
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if (this.enclosingFunctionScope != null && this.enclosingFunctionScope.owner != null)
			{
				this.enclosingFunctionScope.owner.TranslateToILToSaveLocals(il);
			}
			this.operand.TranslateToIL(il, Typeob.Object);
			MethodInfo methodInfo = null;
			ConstantWrapper constantWrapper = this.unsafeOption as ConstantWrapper;
			if (constantWrapper != null)
			{
				string text = constantWrapper.value as string;
				if (text != null && text == "unsafe")
				{
					methodInfo = CompilerGlobals.jScriptEvaluateMethod1;
				}
			}
			if (methodInfo == null)
			{
				methodInfo = CompilerGlobals.jScriptEvaluateMethod2;
				if (this.unsafeOption == null)
				{
					il.Emit(OpCodes.Ldnull);
				}
				else
				{
					this.unsafeOption.TranslateToIL(il, Typeob.Object);
				}
			}
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, methodInfo);
			Convert.Emit(this, il, Typeob.Object, rtype);
			if (this.enclosingFunctionScope != null && this.enclosingFunctionScope.owner != null)
			{
				this.enclosingFunctionScope.owner.TranslateToILToRestoreLocals(il);
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x000277BC File Offset: 0x000267BC
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.operand.TranslateToILInitializer(il);
			if (this.unsafeOption != null)
			{
				this.unsafeOption.TranslateToILInitializer(il);
			}
		}

		// Token: 0x0400026E RID: 622
		private AST operand;

		// Token: 0x0400026F RID: 623
		private AST unsafeOption;

		// Token: 0x04000270 RID: 624
		private FunctionScope enclosingFunctionScope;
	}
}
