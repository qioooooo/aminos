using System;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000051 RID: 81
	internal sealed class Continue : AST
	{
		// Token: 0x060003D1 RID: 977 RVA: 0x00018309 File Offset: 0x00017309
		internal Continue(Context context, int count, bool leavesFinally)
			: base(context)
		{
			this.completion = new Completion();
			this.completion.Continue = count;
			this.leavesFinally = leavesFinally;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00018330 File Offset: 0x00017330
		internal override object Evaluate()
		{
			return this.completion;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00018338 File Offset: 0x00017338
		internal override AST PartiallyEvaluate()
		{
			if (this.leavesFinally)
			{
				this.context.HandleError(JSError.BadWayToLeaveFinally);
			}
			return this;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00018354 File Offset: 0x00017354
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Label label = (Label)base.compilerGlobals.ContinueLabelStack.Peek(this.completion.Continue - 1);
			this.context.EmitLineInfo(il);
			if (this.leavesFinally)
			{
				ConstantWrapper.TranslateToILInt(il, base.compilerGlobals.ContinueLabelStack.Size() - this.completion.Continue);
				il.Emit(OpCodes.Newobj, CompilerGlobals.continueOutOfFinallyConstructor);
				il.Emit(OpCodes.Throw);
				return;
			}
			if (base.compilerGlobals.InsideProtectedRegion)
			{
				il.Emit(OpCodes.Leave, label);
				return;
			}
			il.Emit(OpCodes.Br, label);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x000183FC File Offset: 0x000173FC
		internal override void TranslateToILInitializer(ILGenerator il)
		{
		}

		// Token: 0x040001EC RID: 492
		private Completion completion;

		// Token: 0x040001ED RID: 493
		private bool leavesFinally;
	}
}
