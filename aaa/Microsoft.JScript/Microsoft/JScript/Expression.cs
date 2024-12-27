using System;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200007D RID: 125
	internal sealed class Expression : AST
	{
		// Token: 0x060005A9 RID: 1449 RVA: 0x000277FA File Offset: 0x000267FA
		internal Expression(Context context, AST operand)
			: base(context)
		{
			this.operand = operand;
			this.completion = new Completion();
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00027815 File Offset: 0x00026815
		internal override object Evaluate()
		{
			this.completion.value = this.operand.Evaluate();
			return this.completion;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00027834 File Offset: 0x00026834
		internal override AST PartiallyEvaluate()
		{
			this.operand = this.operand.PartiallyEvaluate();
			if (this.operand is ConstantWrapper)
			{
				this.operand.context.HandleError(JSError.UselessExpression);
			}
			else if (this.operand is Binding)
			{
				((Binding)this.operand).CheckIfUseless();
			}
			return this;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00027894 File Offset: 0x00026894
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.context.EmitLineInfo(il);
			this.operand.TranslateToIL(il, rtype);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x000278AF File Offset: 0x000268AF
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.operand.TranslateToILInitializer(il);
		}

		// Token: 0x04000271 RID: 625
		internal AST operand;

		// Token: 0x04000272 RID: 626
		private Completion completion;
	}
}
