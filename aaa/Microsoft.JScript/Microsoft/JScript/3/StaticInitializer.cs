using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000119 RID: 281
	internal sealed class StaticInitializer : AST
	{
		// Token: 0x06000B9A RID: 2970 RVA: 0x00057D78 File Offset: 0x00056D78
		internal StaticInitializer(Context context, Block body, FunctionScope own_scope)
			: base(context)
		{
			this.func = new FunctionObject(null, new ParameterDeclaration[0], null, body, own_scope, base.Globals.ScopeStack.Peek(), context, MethodAttributes.Private | MethodAttributes.Static);
			this.func.isMethod = true;
			this.func.hasArgumentsObject = false;
			this.completion = new Completion();
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x00057DD7 File Offset: 0x00056DD7
		internal override object Evaluate()
		{
			this.func.Call(new object[0], ((IActivationObject)base.Globals.ScopeStack.Peek()).GetGlobalScope());
			return this.completion;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x00057E0B File Offset: 0x00056E0B
		internal override AST PartiallyEvaluate()
		{
			this.func.PartiallyEvaluate();
			return this;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00057E19 File Offset: 0x00056E19
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.func.TranslateBodyToIL(il, base.compilerGlobals);
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00057E2D File Offset: 0x00056E2D
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			throw new JScriptException(JSError.InternalError, this.context);
		}

		// Token: 0x040006F8 RID: 1784
		private FunctionObject func;

		// Token: 0x040006F9 RID: 1785
		private Completion completion;
	}
}
