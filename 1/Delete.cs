using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000069 RID: 105
	internal sealed class Delete : UnaryOp
	{
		// Token: 0x0600052B RID: 1323 RVA: 0x000250CC File Offset: 0x000240CC
		internal Delete(Context context, AST operand)
			: base(context, operand)
		{
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x000250D6 File Offset: 0x000240D6
		internal override void CheckIfOKToUseInSuperConstructorCall()
		{
			this.context.HandleError(JSError.NotAllowedInSuperConstructorCall);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x000250E8 File Offset: 0x000240E8
		internal override object Evaluate()
		{
			object obj;
			try
			{
				obj = this.operand.Delete();
			}
			catch (JScriptException)
			{
				obj = true;
			}
			return obj;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00025124 File Offset: 0x00024124
		internal override IReflect InferType(JSField inference_target)
		{
			return Typeob.Boolean;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0002512C File Offset: 0x0002412C
		internal override AST PartiallyEvaluate()
		{
			this.operand = this.operand.PartiallyEvaluate();
			if (this.operand is Binding)
			{
				((Binding)this.operand).CheckIfDeletable();
			}
			else if (this.operand is Call)
			{
				((Call)this.operand).MakeDeletable();
			}
			else
			{
				this.operand.context.HandleError(JSError.NotDeletable);
			}
			return this;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0002519E File Offset: 0x0002419E
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.operand.TranslateToILDelete(il, rtype);
		}
	}
}
