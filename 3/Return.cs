using System;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200010D RID: 269
	internal sealed class Return : AST
	{
		// Token: 0x06000B4E RID: 2894 RVA: 0x000568AC File Offset: 0x000558AC
		internal Return(Context context, AST operand, bool leavesFinally)
			: base(context)
		{
			this.completion = new Completion();
			this.completion.Return = true;
			this.operand = operand;
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (!(scriptObject is FunctionScope))
			{
				scriptObject = scriptObject.GetParent();
				if (scriptObject == null)
				{
					this.context.HandleError(JSError.BadReturn);
					scriptObject = new FunctionScope(null);
				}
			}
			this.enclosingFunctionScope = (FunctionScope)scriptObject;
			if (this.operand != null && this.enclosingFunctionScope.returnVar == null)
			{
				this.enclosingFunctionScope.AddReturnValueField();
			}
			this.leavesFinally = leavesFinally;
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0005694D File Offset: 0x0005594D
		internal override object Evaluate()
		{
			if (this.operand != null)
			{
				this.completion.value = this.operand.Evaluate();
			}
			return this.completion;
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00056973 File Offset: 0x00055973
		internal override bool HasReturn()
		{
			return true;
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00056978 File Offset: 0x00055978
		internal override AST PartiallyEvaluate()
		{
			if (this.leavesFinally)
			{
				this.context.HandleError(JSError.BadWayToLeaveFinally);
			}
			if (this.operand != null)
			{
				this.operand = this.operand.PartiallyEvaluate();
				if (this.enclosingFunctionScope.returnVar != null)
				{
					if (this.enclosingFunctionScope.returnVar.type == null)
					{
						this.enclosingFunctionScope.returnVar.SetInferredType(this.operand.InferType(this.enclosingFunctionScope.returnVar), this.operand);
					}
					else
					{
						Binding.AssignmentCompatible(this.enclosingFunctionScope.returnVar.type.ToIReflect(), this.operand, this.operand.InferType(null), true);
					}
				}
				else
				{
					this.context.HandleError(JSError.CannotReturnValueFromVoidFunction);
					this.operand = null;
				}
			}
			else if (this.enclosingFunctionScope.returnVar != null)
			{
				this.enclosingFunctionScope.returnVar.SetInferredType(Typeob.Object, null);
			}
			return this;
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00056A74 File Offset: 0x00055A74
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.context.EmitLineInfo(il);
			if (this.operand != null)
			{
				this.operand.TranslateToIL(il, this.enclosingFunctionScope.returnVar.FieldType);
			}
			else if (this.enclosingFunctionScope.returnVar != null)
			{
				il.Emit(OpCodes.Ldsfld, CompilerGlobals.undefinedField);
				Convert.Emit(this, il, Typeob.Object, this.enclosingFunctionScope.returnVar.FieldType);
			}
			if (this.enclosingFunctionScope.returnVar != null)
			{
				il.Emit(OpCodes.Stloc, (LocalBuilder)this.enclosingFunctionScope.returnVar.GetMetaData());
			}
			if (this.leavesFinally)
			{
				il.Emit(OpCodes.Newobj, CompilerGlobals.returnOutOfFinallyConstructor);
				il.Emit(OpCodes.Throw);
				return;
			}
			if (base.compilerGlobals.InsideProtectedRegion)
			{
				il.Emit(OpCodes.Leave, this.enclosingFunctionScope.owner.returnLabel);
				return;
			}
			il.Emit(OpCodes.Br, this.enclosingFunctionScope.owner.returnLabel);
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00056B81 File Offset: 0x00055B81
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			if (this.operand != null)
			{
				this.operand.TranslateToILInitializer(il);
			}
		}

		// Token: 0x040006D4 RID: 1748
		private Completion completion;

		// Token: 0x040006D5 RID: 1749
		private AST operand;

		// Token: 0x040006D6 RID: 1750
		private FunctionScope enclosingFunctionScope;

		// Token: 0x040006D7 RID: 1751
		private bool leavesFinally;
	}
}
