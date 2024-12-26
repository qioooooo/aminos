using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200000D RID: 13
	internal sealed class AddressOf : UnaryOp
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00004A82 File Offset: 0x00003A82
		internal AddressOf(Context context, AST operand)
			: base(context, operand)
		{
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004A8C File Offset: 0x00003A8C
		internal override object Evaluate()
		{
			return this.operand.Evaluate();
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004A9C File Offset: 0x00003A9C
		internal FieldInfo GetField()
		{
			if (!(this.operand is Binding))
			{
				return null;
			}
			MemberInfo member = ((Binding)this.operand).member;
			if (member is FieldInfo)
			{
				return (FieldInfo)member;
			}
			return null;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004AD9 File Offset: 0x00003AD9
		internal override IReflect InferType(JSField inference_target)
		{
			return this.operand.InferType(inference_target);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004AE8 File Offset: 0x00003AE8
		internal override AST PartiallyEvaluate()
		{
			this.operand = this.operand.PartiallyEvaluate();
			if (!(this.operand is Binding) || !((Binding)this.operand).RefersToMemoryLocation())
			{
				this.context.HandleError(JSError.DoesNotHaveAnAddress);
			}
			return this;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004B36 File Offset: 0x00003B36
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.operand.TranslateToIL(il, rtype);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004B45 File Offset: 0x00003B45
		internal override void TranslateToILPreSet(ILGenerator il)
		{
			this.operand.TranslateToILPreSet(il);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004B53 File Offset: 0x00003B53
		internal override object TranslateToILReference(ILGenerator il, Type rtype)
		{
			return this.operand.TranslateToILReference(il, rtype);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004B62 File Offset: 0x00003B62
		internal override void TranslateToILSet(ILGenerator il, AST rhvalue)
		{
			this.operand.TranslateToILSet(il, rhvalue);
		}
	}
}
