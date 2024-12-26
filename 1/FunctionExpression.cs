using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000083 RID: 131
	public sealed class FunctionExpression : AST
	{
		// Token: 0x060005D0 RID: 1488 RVA: 0x000292E0 File Offset: 0x000282E0
		internal FunctionExpression(Context context, AST id, ParameterDeclaration[] formal_parameters, TypeExpression return_type, Block body, FunctionScope own_scope, FieldAttributes attributes)
			: base(context)
		{
			if (attributes != FieldAttributes.PrivateScope)
			{
				this.context.HandleError(JSError.SyntaxError);
				attributes = FieldAttributes.PrivateScope;
			}
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			this.name = id.ToString();
			if (this.name.Length == 0)
			{
				this.name = "anonymous " + FunctionExpression.uniqueNumber++.ToString(CultureInfo.InvariantCulture);
			}
			else
			{
				this.AddNameTo(scriptObject);
			}
			this.func = new FunctionObject(this.name, formal_parameters, return_type, body, own_scope, scriptObject, this.context, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0002938C File Offset: 0x0002838C
		private void AddNameTo(ScriptObject enclosingScope)
		{
			while (enclosingScope is WithObject)
			{
				enclosingScope = enclosingScope.GetParent();
			}
			FieldInfo fieldInfo = ((IActivationObject)enclosingScope).GetLocalField(this.name);
			if (fieldInfo != null)
			{
				return;
			}
			if (enclosingScope is ActivationObject)
			{
				if (enclosingScope is FunctionScope)
				{
					fieldInfo = ((ActivationObject)enclosingScope).AddNewField(this.name, null, FieldAttributes.Public);
				}
				else
				{
					fieldInfo = ((ActivationObject)enclosingScope).AddNewField(this.name, null, FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static);
				}
			}
			else
			{
				fieldInfo = ((StackFrame)enclosingScope).AddNewField(this.name, null, FieldAttributes.Public);
			}
			JSLocalField jslocalField = fieldInfo as JSLocalField;
			if (jslocalField != null)
			{
				jslocalField.debugOn = this.context.document.debugOn;
				jslocalField.isDefined = true;
			}
			this.field = (JSVariableField)fieldInfo;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00029448 File Offset: 0x00028448
		internal override object Evaluate()
		{
			if (VsaEngine.executeForJSEE)
			{
				throw new JScriptException(JSError.NonSupportedInDebugger);
			}
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			this.func.own_scope.SetParent(scriptObject);
			Closure closure = new Closure(this.func);
			if (this.field != null)
			{
				this.field.value = closure;
			}
			return closure;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x000294AA File Offset: 0x000284AA
		internal override IReflect InferType(JSField inference_target)
		{
			return Typeob.ScriptFunction;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000294B4 File Offset: 0x000284B4
		public static FunctionObject JScriptFunctionExpression(RuntimeTypeHandle handle, string name, string method_name, string[] formal_params, JSLocalField[] fields, bool must_save_stack_locals, bool hasArgumentsObject, string text, VsaEngine engine)
		{
			Type typeFromHandle = Type.GetTypeFromHandle(handle);
			return new FunctionObject(typeFromHandle, name, method_name, formal_params, fields, must_save_stack_locals, hasArgumentsObject, text, engine);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x000294E0 File Offset: 0x000284E0
		internal override AST PartiallyEvaluate()
		{
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			if (ClassScope.ScopeOfClassMemberInitializer(scriptObject) != null)
			{
				this.context.HandleError(JSError.MemberInitializerCannotContainFuncExpr);
				return this;
			}
			ScriptObject scriptObject2 = scriptObject;
			while (scriptObject2 is WithObject || scriptObject2 is BlockScope)
			{
				scriptObject2 = scriptObject2.GetParent();
			}
			FunctionScope functionScope = scriptObject2 as FunctionScope;
			if (functionScope != null)
			{
				functionScope.closuresMightEscape = true;
			}
			if (scriptObject2 != scriptObject)
			{
				this.func.own_scope.SetParent(new WithObject(new JSObject(), this.func.own_scope.GetGlobalScope()));
			}
			this.func.PartiallyEvaluate();
			return this;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00029580 File Offset: 0x00028580
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if (rtype == Typeob.Void)
			{
				return;
			}
			il.Emit(OpCodes.Ldloc, this.func_local);
			il.Emit(OpCodes.Newobj, CompilerGlobals.closureConstructor);
			Convert.Emit(this, il, Typeob.Closure, rtype);
			if (this.field != null)
			{
				il.Emit(OpCodes.Dup);
				object metaData = this.field.GetMetaData();
				if (metaData is LocalBuilder)
				{
					il.Emit(OpCodes.Stloc, (LocalBuilder)metaData);
					return;
				}
				il.Emit(OpCodes.Stsfld, (FieldInfo)metaData);
			}
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00029610 File Offset: 0x00028610
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.func.TranslateToIL(base.compilerGlobals);
			this.func_local = il.DeclareLocal(Typeob.FunctionObject);
			il.Emit(OpCodes.Ldtoken, this.func.classwriter);
			il.Emit(OpCodes.Ldstr, this.name);
			il.Emit(OpCodes.Ldstr, this.func.GetName());
			int num = this.func.formal_parameters.Length;
			ConstantWrapper.TranslateToILInt(il, num);
			il.Emit(OpCodes.Newarr, Typeob.String);
			for (int i = 0; i < num; i++)
			{
				il.Emit(OpCodes.Dup);
				ConstantWrapper.TranslateToILInt(il, i);
				il.Emit(OpCodes.Ldstr, this.func.formal_parameters[i]);
				il.Emit(OpCodes.Stelem_Ref);
			}
			num = this.func.fields.Length;
			ConstantWrapper.TranslateToILInt(il, num);
			il.Emit(OpCodes.Newarr, Typeob.JSLocalField);
			for (int j = 0; j < num; j++)
			{
				JSLocalField jslocalField = this.func.fields[j];
				il.Emit(OpCodes.Dup);
				ConstantWrapper.TranslateToILInt(il, j);
				il.Emit(OpCodes.Ldstr, jslocalField.Name);
				il.Emit(OpCodes.Ldtoken, jslocalField.FieldType);
				ConstantWrapper.TranslateToILInt(il, jslocalField.slotNumber);
				il.Emit(OpCodes.Newobj, CompilerGlobals.jsLocalFieldConstructor);
				il.Emit(OpCodes.Stelem_Ref);
			}
			if (this.func.must_save_stack_locals)
			{
				il.Emit(OpCodes.Ldc_I4_1);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}
			if (this.func.hasArgumentsObject)
			{
				il.Emit(OpCodes.Ldc_I4_1);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}
			il.Emit(OpCodes.Ldstr, this.func.ToString());
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.jScriptFunctionExpressionMethod);
			il.Emit(OpCodes.Stloc, this.func_local);
		}

		// Token: 0x0400028B RID: 651
		private FunctionObject func;

		// Token: 0x0400028C RID: 652
		private string name;

		// Token: 0x0400028D RID: 653
		private JSVariableField field;

		// Token: 0x0400028E RID: 654
		private LocalBuilder func_local;

		// Token: 0x0400028F RID: 655
		private static int uniqueNumber;
	}
}
