using System;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200004F RID: 79
	internal class ConstructorCall : AST
	{
		// Token: 0x060003B5 RID: 949 RVA: 0x00017E1B File Offset: 0x00016E1B
		internal ConstructorCall(Context context, ASTList arguments, bool isSuperConstructorCall)
			: base(context)
		{
			this.isOK = false;
			this.isSuperConstructorCall = isSuperConstructorCall;
			if (arguments == null)
			{
				this.arguments = new ASTList(context);
				return;
			}
			this.arguments = arguments;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00017E49 File Offset: 0x00016E49
		internal override object Evaluate()
		{
			return new Completion();
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00017E50 File Offset: 0x00016E50
		internal override AST PartiallyEvaluate()
		{
			if (!this.isOK)
			{
				this.context.HandleError(JSError.NotOKToCallSuper);
				return this;
			}
			int i = 0;
			int count = this.arguments.count;
			while (i < count)
			{
				this.arguments[i] = this.arguments[i].PartiallyEvaluate();
				this.arguments[i].CheckIfOKToUseInSuperConstructorCall();
				i++;
			}
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			if (!(scriptObject is FunctionScope))
			{
				this.context.HandleError(JSError.NotOKToCallSuper);
				return this;
			}
			if (!((FunctionScope)scriptObject).owner.isConstructor)
			{
				this.context.HandleError(JSError.NotOKToCallSuper);
			}
			((FunctionScope)scriptObject).owner.superConstructorCall = this;
			return this;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00017F1C File Offset: 0x00016F1C
		internal override AST PartiallyEvaluateAsReference()
		{
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00017F25 File Offset: 0x00016F25
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00017F27 File Offset: 0x00016F27
		internal override void TranslateToILInitializer(ILGenerator il)
		{
		}

		// Token: 0x040001DF RID: 479
		internal bool isOK;

		// Token: 0x040001E0 RID: 480
		internal bool isSuperConstructorCall;

		// Token: 0x040001E1 RID: 481
		internal ASTList arguments;
	}
}
