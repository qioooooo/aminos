using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x0200000B RID: 11
	public abstract class AST
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00004570 File Offset: 0x00003570
		internal AST(Context context)
		{
			this.context = context;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000457F File Offset: 0x0000357F
		internal virtual void CheckIfOKToUseInSuperConstructorCall()
		{
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00004581 File Offset: 0x00003581
		internal CompilerGlobals compilerGlobals
		{
			get
			{
				return this.context.document.compilerGlobals;
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004593 File Offset: 0x00003593
		internal virtual bool Delete()
		{
			return true;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004598 File Offset: 0x00003598
		internal void EmitILToLoadEngine(ILGenerator il)
		{
			ScriptObject scriptObject = this.Engine.ScriptObjectStackTop();
			while (scriptObject != null && (scriptObject is WithObject || scriptObject is BlockScope))
			{
				scriptObject = scriptObject.GetParent();
			}
			if (scriptObject is FunctionScope)
			{
				((FunctionScope)scriptObject).owner.TranslateToILToLoadEngine(il);
				return;
			}
			if (!(scriptObject is ClassScope))
			{
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Ldfld, CompilerGlobals.engineField);
				return;
			}
			if (this.Engine.doCRS)
			{
				il.Emit(OpCodes.Ldsfld, CompilerGlobals.contextEngineField);
				return;
			}
			if (this.context.document.engine.PEFileKind == PEFileKinds.Dll)
			{
				il.Emit(OpCodes.Ldtoken, ((ClassScope)scriptObject).GetTypeBuilder());
				il.Emit(OpCodes.Call, CompilerGlobals.createVsaEngineWithType);
				return;
			}
			il.Emit(OpCodes.Call, CompilerGlobals.createVsaEngine);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00004678 File Offset: 0x00003678
		internal VsaEngine Engine
		{
			get
			{
				return this.context.document.engine;
			}
		}

		// Token: 0x0600007F RID: 127
		internal abstract object Evaluate();

		// Token: 0x06000080 RID: 128 RVA: 0x0000468A File Offset: 0x0000368A
		internal virtual LateBinding EvaluateAsLateBinding()
		{
			return new LateBinding(null, this.Evaluate(), VsaEngine.executeForJSEE);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000469D File Offset: 0x0000369D
		internal virtual WrappedNamespace EvaluateAsWrappedNamespace(bool giveErrorIfNameInUse)
		{
			throw new JScriptException(JSError.InternalError, this.context);
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000046AC File Offset: 0x000036AC
		internal Globals Globals
		{
			get
			{
				return this.context.document.engine.Globals;
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000046C3 File Offset: 0x000036C3
		internal virtual bool HasReturn()
		{
			return false;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000046C6 File Offset: 0x000036C6
		internal virtual IReflect InferType(JSField inference_target)
		{
			return Typeob.Object;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000046CD File Offset: 0x000036CD
		internal virtual void InvalidateInferredTypes()
		{
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000046CF File Offset: 0x000036CF
		internal virtual bool OkToUseAsType()
		{
			return false;
		}

		// Token: 0x06000087 RID: 135
		internal abstract AST PartiallyEvaluate();

		// Token: 0x06000088 RID: 136 RVA: 0x000046D2 File Offset: 0x000036D2
		internal virtual AST PartiallyEvaluateAsCallable()
		{
			return new CallableExpression(this.PartiallyEvaluate());
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000046DF File Offset: 0x000036DF
		internal virtual AST PartiallyEvaluateAsReference()
		{
			return this.PartiallyEvaluate();
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000046E7 File Offset: 0x000036E7
		internal virtual void ResolveCall(ASTList args, IReflect[] argIRs, bool constructor, bool brackets)
		{
			throw new JScriptException(JSError.InternalError, this.context);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000046F6 File Offset: 0x000036F6
		internal virtual object ResolveCustomAttribute(ASTList args, IReflect[] argIRs, AST target)
		{
			throw new JScriptException(JSError.InternalError, this.context);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004705 File Offset: 0x00003705
		internal virtual void SetPartialValue(AST partial_value)
		{
			this.context.HandleError(JSError.IllegalAssignment);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004717 File Offset: 0x00003717
		internal virtual void SetValue(object value)
		{
			this.context.HandleError(JSError.IllegalAssignment);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000472C File Offset: 0x0000372C
		internal virtual void TranslateToConditionalBranch(ILGenerator il, bool branchIfTrue, Label label, bool shortForm)
		{
			IReflect reflect = this.InferType(null);
			if (reflect != Typeob.Object && reflect is Type)
			{
				string text = (branchIfTrue ? "op_True" : "op_False");
				MethodInfo method = reflect.GetMethod(text, BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { (Type)reflect }, null);
				if (method != null)
				{
					this.TranslateToIL(il, (Type)reflect);
					il.Emit(OpCodes.Call, method);
					il.Emit(OpCodes.Brtrue, label);
					return;
				}
			}
			Type type = Convert.ToType(reflect);
			this.TranslateToIL(il, type);
			Convert.Emit(this, il, type, Typeob.Boolean, true);
			if (branchIfTrue)
			{
				il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
				return;
			}
			il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
		}

		// Token: 0x0600008F RID: 143
		internal abstract void TranslateToIL(ILGenerator il, Type rtype);

		// Token: 0x06000090 RID: 144 RVA: 0x000047FC File Offset: 0x000037FC
		internal virtual void TranslateToILCall(ILGenerator il, Type rtype, ASTList args, bool construct, bool brackets)
		{
			throw new JScriptException(JSError.InternalError, this.context);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000480B File Offset: 0x0000380B
		internal virtual void TranslateToILDelete(ILGenerator il, Type rtype)
		{
			if (rtype != Typeob.Void)
			{
				il.Emit(OpCodes.Ldc_I4_1);
				Convert.Emit(this, il, Typeob.Boolean, rtype);
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000482D File Offset: 0x0000382D
		internal virtual void TranslateToILInitializer(ILGenerator il)
		{
			throw new JScriptException(JSError.InternalError, this.context);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000483C File Offset: 0x0000383C
		internal virtual void TranslateToILPreSet(ILGenerator il)
		{
			throw new JScriptException(JSError.InternalError, this.context);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000484B File Offset: 0x0000384B
		internal virtual void TranslateToILPreSet(ILGenerator il, ASTList args)
		{
			this.TranslateToIL(il, Typeob.Object);
			args.TranslateToIL(il, Typeob.ArrayOfObject);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004865 File Offset: 0x00003865
		internal virtual void TranslateToILPreSetPlusGet(ILGenerator il)
		{
			throw new JScriptException(JSError.InternalError, this.context);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004874 File Offset: 0x00003874
		internal virtual void TranslateToILPreSetPlusGet(ILGenerator il, ASTList args, bool inBrackets)
		{
			il.Emit(OpCodes.Ldnull);
			this.TranslateToIL(il, Typeob.Object);
			il.Emit(OpCodes.Dup);
			LocalBuilder localBuilder = il.DeclareLocal(Typeob.Object);
			il.Emit(OpCodes.Stloc, localBuilder);
			args.TranslateToIL(il, Typeob.ArrayOfObject);
			il.Emit(OpCodes.Dup);
			LocalBuilder localBuilder2 = il.DeclareLocal(Typeob.ArrayOfObject);
			il.Emit(OpCodes.Stloc, localBuilder2);
			il.Emit(OpCodes.Ldc_I4_0);
			if (inBrackets)
			{
				il.Emit(OpCodes.Ldc_I4_1);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}
			this.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.callValueMethod);
			LocalBuilder localBuilder3 = il.DeclareLocal(Typeob.Object);
			il.Emit(OpCodes.Stloc, localBuilder3);
			il.Emit(OpCodes.Ldloc, localBuilder);
			il.Emit(OpCodes.Ldloc, localBuilder2);
			il.Emit(OpCodes.Ldloc, localBuilder3);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004963 File Offset: 0x00003963
		internal void TranslateToILSet(ILGenerator il)
		{
			this.TranslateToILSet(il, null);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000496D File Offset: 0x0000396D
		internal virtual void TranslateToILSet(ILGenerator il, AST rhvalue)
		{
			if (rhvalue != null)
			{
				rhvalue.TranslateToIL(il, Typeob.Object);
			}
			il.Emit(OpCodes.Call, CompilerGlobals.setIndexedPropertyValueStaticMethod);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004990 File Offset: 0x00003990
		internal virtual object TranslateToILReference(ILGenerator il, Type rtype)
		{
			this.TranslateToIL(il, rtype);
			LocalBuilder localBuilder = il.DeclareLocal(rtype);
			il.Emit(OpCodes.Stloc, localBuilder);
			il.Emit(OpCodes.Ldloca, localBuilder);
			return localBuilder;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000049C6 File Offset: 0x000039C6
		internal virtual Context GetFirstExecutableContext()
		{
			return this.context;
		}

		// Token: 0x04000022 RID: 34
		internal Context context;
	}
}
