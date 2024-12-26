using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000131 RID: 305
	internal sealed class VoidOp : UnaryOp
	{
		// Token: 0x06000DEB RID: 3563 RVA: 0x0005E668 File Offset: 0x0005D668
		internal VoidOp(Context context, AST operand)
			: base(context, operand)
		{
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0005E672 File Offset: 0x0005D672
		internal override object Evaluate()
		{
			this.operand.Evaluate();
			return null;
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0005E681 File Offset: 0x0005D681
		internal override IReflect InferType(JSField inference_target)
		{
			return Typeob.Empty;
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0005E688 File Offset: 0x0005D688
		internal override AST PartiallyEvaluate()
		{
			return new ConstantWrapper(null, this.context);
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0005E698 File Offset: 0x0005D698
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.operand.TranslateToIL(il, Typeob.Object);
			if (rtype != Typeob.Void)
			{
				il.Emit(OpCodes.Ldsfld, CompilerGlobals.undefinedField);
				Convert.Emit(this, il, Typeob.Object, rtype);
				return;
			}
			il.Emit(OpCodes.Pop);
		}
	}
}
