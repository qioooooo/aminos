using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000121 RID: 289
	public sealed class Try : AST
	{
		// Token: 0x06000BD0 RID: 3024 RVA: 0x00059BB4 File Offset: 0x00058BB4
		internal Try(Context context, AST body, AST identifier, TypeExpression type, AST handler, AST finally_block, bool finallyHasControlFlowOutOfIt, Context tryEndContext)
			: base(context)
		{
			this.body = body;
			this.type = type;
			this.handler = handler;
			this.finally_block = finally_block;
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (scriptObject is WithObject)
			{
				scriptObject = scriptObject.GetParent();
			}
			this.handler_scope = null;
			this.field = null;
			if (identifier != null)
			{
				this.fieldName = identifier.ToString();
				this.field = scriptObject.GetField(this.fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
				if (this.field != null)
				{
					if (type == null && this.field is JSVariableField && this.field.IsStatic && ((JSVariableField)this.field).type == null && !this.field.IsLiteral && !this.field.IsInitOnly)
					{
						return;
					}
					if (((IActivationObject)scriptObject).GetLocalField(this.fieldName) != null)
					{
						identifier.context.HandleError(JSError.DuplicateName, false);
					}
				}
				this.handler_scope = new BlockScope(scriptObject);
				this.handler_scope.catchHanderScope = true;
				JSVariableField jsvariableField = this.handler_scope.AddNewField(identifier.ToString(), Missing.Value, FieldAttributes.Public);
				this.field = jsvariableField;
				jsvariableField.originalContext = identifier.context;
				if (identifier.context.document.debugOn && this.field is JSLocalField)
				{
					this.handler_scope.AddFieldForLocalScopeDebugInfo((JSLocalField)this.field);
				}
			}
			this.finallyHasControlFlowOutOfIt = finallyHasControlFlowOutOfIt;
			this.tryEndContext = tryEndContext;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00059D3C File Offset: 0x00058D3C
		internal override object Evaluate()
		{
			int num = base.Globals.ScopeStack.Size();
			int num2 = base.Globals.CallContextStack.Size();
			Completion completion = null;
			Completion completion2 = null;
			try
			{
				object obj = null;
				try
				{
					completion = (Completion)this.body.Evaluate();
				}
				catch (Exception ex)
				{
					if (this.handler == null)
					{
						throw;
					}
					obj = ex;
					if (this.type != null)
					{
						Type type = this.type.ToType();
						if (Typeob.Exception.IsAssignableFrom(type))
						{
							if (!type.IsInstanceOfType(ex))
							{
								throw;
							}
						}
						else if (!type.IsInstanceOfType(obj = Try.JScriptExceptionValue(ex, base.Engine)))
						{
							throw;
						}
					}
					else
					{
						obj = Try.JScriptExceptionValue(ex, base.Engine);
					}
				}
				catch
				{
					obj = new JScriptException(JSError.NonClsException);
				}
				if (obj != null)
				{
					base.Globals.ScopeStack.TrimToSize(num);
					base.Globals.CallContextStack.TrimToSize(num2);
					if (this.handler_scope != null)
					{
						this.handler_scope.SetParent(base.Globals.ScopeStack.Peek());
						base.Globals.ScopeStack.Push(this.handler_scope);
					}
					if (this.field != null)
					{
						this.field.SetValue(base.Globals.ScopeStack.Peek(), obj);
					}
					completion = (Completion)this.handler.Evaluate();
				}
			}
			finally
			{
				base.Globals.ScopeStack.TrimToSize(num);
				base.Globals.CallContextStack.TrimToSize(num2);
				if (this.finally_block != null)
				{
					completion2 = (Completion)this.finally_block.Evaluate();
				}
			}
			if (completion == null || (completion2 != null && (completion2.Exit > 0 || completion2.Continue > 0 || completion2.Return)))
			{
				completion = completion2;
			}
			else if (completion2 != null && completion2.value is Missing)
			{
				completion.value = completion2.value;
			}
			return new Completion
			{
				Continue = completion.Continue - 1,
				Exit = completion.Exit - 1,
				Return = completion.Return,
				value = completion.value
			};
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00059FA4 File Offset: 0x00058FA4
		internal override Context GetFirstExecutableContext()
		{
			return this.body.GetFirstExecutableContext();
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00059FB4 File Offset: 0x00058FB4
		public static object JScriptExceptionValue(object e, VsaEngine engine)
		{
			if (engine == null)
			{
				engine = new VsaEngine(true);
				engine.InitVsaEngine("JS7://Microsoft.JScript.Vsa.VsaEngine", new DefaultVsaSite());
			}
			ErrorConstructor originalError = engine.Globals.globalObject.originalError;
			if (e is JScriptException)
			{
				object value = ((JScriptException)e).value;
				if (value is Exception || value is Missing || (((JScriptException)e).Number & 65535) != 5022)
				{
					return originalError.Construct((Exception)e);
				}
				return value;
			}
			else
			{
				if (e is StackOverflowException)
				{
					return originalError.Construct(new JScriptException(JSError.OutOfStack));
				}
				if (e is OutOfMemoryException)
				{
					return originalError.Construct(new JScriptException(JSError.OutOfMemory));
				}
				return originalError.Construct(e);
			}
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0005A06C File Offset: 0x0005906C
		internal override AST PartiallyEvaluate()
		{
			if (this.type != null)
			{
				this.type.PartiallyEvaluate();
				((JSVariableField)this.field).type = this.type;
			}
			else if (this.field is JSLocalField)
			{
				((JSLocalField)this.field).SetInferredType(Typeob.Object, null);
			}
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (scriptObject is WithObject)
			{
				scriptObject = scriptObject.GetParent();
			}
			FunctionScope functionScope = null;
			BitArray bitArray = null;
			if (scriptObject is FunctionScope)
			{
				functionScope = (FunctionScope)scriptObject;
				bitArray = functionScope.DefinedFlags;
			}
			this.body = this.body.PartiallyEvaluate();
			if (this.handler != null)
			{
				if (this.handler_scope != null)
				{
					base.Globals.ScopeStack.Push(this.handler_scope);
				}
				if (this.field is JSLocalField)
				{
					((JSLocalField)this.field).isDefined = true;
				}
				this.handler = this.handler.PartiallyEvaluate();
				if (this.handler_scope != null)
				{
					base.Globals.ScopeStack.Pop();
				}
			}
			if (this.finally_block != null)
			{
				this.finally_block = this.finally_block.PartiallyEvaluate();
			}
			if (functionScope != null)
			{
				functionScope.DefinedFlags = bitArray;
			}
			return this;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x0005A1A7 File Offset: 0x000591A7
		public static void PushHandlerScope(VsaEngine engine, string id, int scopeId)
		{
			engine.PushScriptObject(new BlockScope(engine.ScriptObjectStackTop(), id, scopeId));
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x0005A1BC File Offset: 0x000591BC
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			bool insideProtectedRegion = base.compilerGlobals.InsideProtectedRegion;
			base.compilerGlobals.InsideProtectedRegion = true;
			base.compilerGlobals.BreakLabelStack.Push(base.compilerGlobals.BreakLabelStack.Peek(0));
			base.compilerGlobals.ContinueLabelStack.Push(base.compilerGlobals.ContinueLabelStack.Peek(0));
			il.BeginExceptionBlock();
			if (this.finally_block != null)
			{
				if (this.finallyHasControlFlowOutOfIt)
				{
					il.BeginExceptionBlock();
				}
				if (this.handler != null)
				{
					il.BeginExceptionBlock();
				}
			}
			this.body.TranslateToIL(il, Typeob.Void);
			if (this.tryEndContext != null)
			{
				this.tryEndContext.EmitLineInfo(il);
			}
			if (this.handler != null)
			{
				if (this.type == null)
				{
					il.BeginCatchBlock(Typeob.Exception);
					this.handler.context.EmitLineInfo(il);
					base.EmitILToLoadEngine(il);
					il.Emit(OpCodes.Call, CompilerGlobals.jScriptExceptionValueMethod);
				}
				else
				{
					Type type = this.type.ToType();
					if (Typeob.Exception.IsAssignableFrom(type))
					{
						il.BeginCatchBlock(type);
						this.handler.context.EmitLineInfo(il);
					}
					else
					{
						il.BeginExceptFilterBlock();
						this.handler.context.EmitLineInfo(il);
						base.EmitILToLoadEngine(il);
						il.Emit(OpCodes.Call, CompilerGlobals.jScriptExceptionValueMethod);
						il.Emit(OpCodes.Isinst, type);
						il.Emit(OpCodes.Ldnull);
						il.Emit(OpCodes.Cgt_Un);
						il.BeginCatchBlock(null);
						base.EmitILToLoadEngine(il);
						il.Emit(OpCodes.Call, CompilerGlobals.jScriptExceptionValueMethod);
						Convert.Emit(this, il, Typeob.Object, type);
					}
				}
				object obj = ((this.field is JSVariableField) ? ((JSVariableField)this.field).GetMetaData() : this.field);
				if (obj is LocalBuilder)
				{
					il.Emit(OpCodes.Stloc, (LocalBuilder)obj);
				}
				else if (obj is FieldInfo)
				{
					il.Emit(OpCodes.Stsfld, (FieldInfo)obj);
				}
				else
				{
					Convert.EmitLdarg(il, (short)obj);
				}
				if (this.handler_scope != null)
				{
					if (!this.handler_scope.isKnownAtCompileTime)
					{
						base.EmitILToLoadEngine(il);
						il.Emit(OpCodes.Ldstr, this.fieldName);
						ConstantWrapper.TranslateToILInt(il, this.handler_scope.scopeId);
						il.Emit(OpCodes.Call, Typeob.Try.GetMethod("PushHandlerScope"));
						base.Globals.ScopeStack.Push(this.handler_scope);
						il.BeginExceptionBlock();
					}
					il.BeginScope();
					if (this.context.document.debugOn)
					{
						this.handler_scope.EmitLocalInfoForFields(il);
					}
				}
				this.handler.TranslateToIL(il, Typeob.Void);
				if (this.handler_scope != null)
				{
					il.EndScope();
					if (!this.handler_scope.isKnownAtCompileTime)
					{
						il.BeginFinallyBlock();
						base.EmitILToLoadEngine(il);
						il.Emit(OpCodes.Call, CompilerGlobals.popScriptObjectMethod);
						il.Emit(OpCodes.Pop);
						base.Globals.ScopeStack.Pop();
						il.EndExceptionBlock();
					}
				}
				il.EndExceptionBlock();
			}
			if (this.finally_block != null)
			{
				bool insideFinally = base.compilerGlobals.InsideFinally;
				int finallyStackTop = base.compilerGlobals.FinallyStackTop;
				base.compilerGlobals.InsideFinally = true;
				base.compilerGlobals.FinallyStackTop = base.compilerGlobals.BreakLabelStack.Size();
				il.BeginFinallyBlock();
				this.finally_block.TranslateToIL(il, Typeob.Void);
				il.EndExceptionBlock();
				base.compilerGlobals.InsideFinally = insideFinally;
				base.compilerGlobals.FinallyStackTop = finallyStackTop;
				if (this.finallyHasControlFlowOutOfIt)
				{
					il.BeginCatchBlock(Typeob.BreakOutOfFinally);
					il.Emit(OpCodes.Ldfld, Typeob.BreakOutOfFinally.GetField("target"));
					int i = base.compilerGlobals.BreakLabelStack.Size() - 1;
					int num = i;
					while (i > 0)
					{
						il.Emit(OpCodes.Dup);
						ConstantWrapper.TranslateToILInt(il, i);
						Label label = il.DefineLabel();
						il.Emit(OpCodes.Blt_S, label);
						il.Emit(OpCodes.Pop);
						if (insideFinally && i < finallyStackTop)
						{
							il.Emit(OpCodes.Rethrow);
						}
						else
						{
							il.Emit(OpCodes.Leave, (Label)base.compilerGlobals.BreakLabelStack.Peek(num - i));
						}
						il.MarkLabel(label);
						i--;
					}
					il.Emit(OpCodes.Pop);
					il.BeginCatchBlock(Typeob.ContinueOutOfFinally);
					il.Emit(OpCodes.Ldfld, Typeob.ContinueOutOfFinally.GetField("target"));
					int j = base.compilerGlobals.ContinueLabelStack.Size() - 1;
					int num2 = j;
					while (j > 0)
					{
						il.Emit(OpCodes.Dup);
						ConstantWrapper.TranslateToILInt(il, j);
						Label label2 = il.DefineLabel();
						il.Emit(OpCodes.Blt_S, label2);
						il.Emit(OpCodes.Pop);
						if (insideFinally && j < finallyStackTop)
						{
							il.Emit(OpCodes.Rethrow);
						}
						else
						{
							il.Emit(OpCodes.Leave, (Label)base.compilerGlobals.ContinueLabelStack.Peek(num2 - j));
						}
						il.MarkLabel(label2);
						j--;
					}
					il.Emit(OpCodes.Pop);
					ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
					while (scriptObject != null && !(scriptObject is FunctionScope))
					{
						scriptObject = scriptObject.GetParent();
					}
					if (scriptObject != null && !insideFinally)
					{
						il.BeginCatchBlock(Typeob.ReturnOutOfFinally);
						il.Emit(OpCodes.Pop);
						il.Emit(OpCodes.Leave, ((FunctionScope)scriptObject).owner.returnLabel);
					}
					il.EndExceptionBlock();
				}
			}
			base.compilerGlobals.InsideProtectedRegion = insideProtectedRegion;
			base.compilerGlobals.BreakLabelStack.Pop();
			base.compilerGlobals.ContinueLabelStack.Pop();
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x0005A7A2 File Offset: 0x000597A2
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.body.TranslateToILInitializer(il);
			if (this.handler != null)
			{
				this.handler.TranslateToILInitializer(il);
			}
			if (this.finally_block != null)
			{
				this.finally_block.TranslateToILInitializer(il);
			}
		}

		// Token: 0x04000707 RID: 1799
		private AST body;

		// Token: 0x04000708 RID: 1800
		private TypeExpression type;

		// Token: 0x04000709 RID: 1801
		private AST handler;

		// Token: 0x0400070A RID: 1802
		private AST finally_block;

		// Token: 0x0400070B RID: 1803
		private BlockScope handler_scope;

		// Token: 0x0400070C RID: 1804
		private FieldInfo field;

		// Token: 0x0400070D RID: 1805
		private string fieldName;

		// Token: 0x0400070E RID: 1806
		private bool finallyHasControlFlowOutOfIt;

		// Token: 0x0400070F RID: 1807
		private Context tryEndContext;
	}
}
