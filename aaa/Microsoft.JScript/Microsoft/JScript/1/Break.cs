using System;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000031 RID: 49
	internal sealed class Break : AST
	{
		// Token: 0x060001F9 RID: 505 RVA: 0x0000F065 File Offset: 0x0000E065
		internal Break(Context context, int count, bool leavesFinally)
			: base(context)
		{
			this.completion = new Completion();
			this.completion.Exit = count;
			this.leavesFinally = leavesFinally;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000F08C File Offset: 0x0000E08C
		internal override object Evaluate()
		{
			return this.completion;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000F094 File Offset: 0x0000E094
		internal override AST PartiallyEvaluate()
		{
			if (this.leavesFinally)
			{
				this.context.HandleError(JSError.BadWayToLeaveFinally);
			}
			return this;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000F0B0 File Offset: 0x0000E0B0
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Label label = (Label)base.compilerGlobals.BreakLabelStack.Peek(this.completion.Exit - 1);
			this.context.EmitLineInfo(il);
			if (this.leavesFinally)
			{
				ConstantWrapper.TranslateToILInt(il, base.compilerGlobals.BreakLabelStack.Size() - this.completion.Exit);
				il.Emit(OpCodes.Newobj, CompilerGlobals.breakOutOfFinallyConstructor);
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

		// Token: 0x060001FD RID: 509 RVA: 0x0000F158 File Offset: 0x0000E158
		internal override void TranslateToILInitializer(ILGenerator il)
		{
		}

		// Token: 0x0400009C RID: 156
		private Completion completion;

		// Token: 0x0400009D RID: 157
		private bool leavesFinally;
	}
}
