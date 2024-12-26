using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000047 RID: 71
	internal sealed class Comma : BinaryOp
	{
		// Token: 0x060002E9 RID: 745 RVA: 0x00015A78 File Offset: 0x00014A78
		internal Comma(Context context, AST operand1, AST operand2)
			: base(context, operand1, operand2)
		{
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00015A83 File Offset: 0x00014A83
		internal override object Evaluate()
		{
			this.operand1.Evaluate();
			return this.operand2.Evaluate();
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00015A9C File Offset: 0x00014A9C
		internal override IReflect InferType(JSField inference_target)
		{
			return this.operand2.InferType(inference_target);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00015AAA File Offset: 0x00014AAA
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.operand1.TranslateToIL(il, Typeob.Void);
			this.operand2.TranslateToIL(il, rtype);
		}
	}
}
