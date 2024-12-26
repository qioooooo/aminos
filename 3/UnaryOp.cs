using System;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200000C RID: 12
	public abstract class UnaryOp : AST
	{
		// Token: 0x0600009B RID: 155 RVA: 0x000049CE File Offset: 0x000039CE
		internal UnaryOp(Context context, AST operand)
			: base(context)
		{
			this.operand = operand;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000049DE File Offset: 0x000039DE
		internal override void CheckIfOKToUseInSuperConstructorCall()
		{
			this.operand.CheckIfOKToUseInSuperConstructorCall();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000049EC File Offset: 0x000039EC
		internal override AST PartiallyEvaluate()
		{
			this.operand = this.operand.PartiallyEvaluate();
			if (this.operand is ConstantWrapper)
			{
				try
				{
					return new ConstantWrapper(this.Evaluate(), this.context);
				}
				catch (JScriptException ex)
				{
					this.context.HandleError((JSError)(ex.ErrorNumber & 65535));
				}
				catch
				{
					this.context.HandleError(JSError.TypeMismatch);
				}
				return this;
			}
			return this;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004A74 File Offset: 0x00003A74
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.operand.TranslateToILInitializer(il);
		}

		// Token: 0x04000023 RID: 35
		protected AST operand;
	}
}
