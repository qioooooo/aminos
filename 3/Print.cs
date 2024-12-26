using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x020000FC RID: 252
	internal sealed class Print : AST
	{
		// Token: 0x06000AE2 RID: 2786 RVA: 0x000544B5 File Offset: 0x000534B5
		internal Print(Context context, AST operand)
			: base(context)
		{
			this.operand = (ASTList)operand;
			this.completion = new Completion();
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x000544D8 File Offset: 0x000534D8
		internal override object Evaluate()
		{
			object[] array = this.operand.EvaluateAsArray();
			for (int i = 0; i < array.Length - 1; i++)
			{
				ScriptStream.Out.Write(Convert.ToString(array[i]));
			}
			if (array.Length > 0)
			{
				this.completion.value = Convert.ToString(array[array.Length - 1]);
				ScriptStream.Out.WriteLine(this.completion.value);
			}
			else
			{
				ScriptStream.Out.WriteLine("");
				this.completion.value = null;
			}
			return this.completion;
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00054568 File Offset: 0x00053568
		internal override AST PartiallyEvaluate()
		{
			this.operand = (ASTList)this.operand.PartiallyEvaluate();
			return this;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00054584 File Offset: 0x00053584
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if (this.context.document.debugOn)
			{
				il.Emit(OpCodes.Nop);
			}
			ASTList astlist = this.operand;
			int count = astlist.count;
			for (int i = 0; i < count; i++)
			{
				AST ast = astlist[i];
				IReflect reflect = ast.InferType(null);
				if (reflect == Typeob.String)
				{
					ast.TranslateToIL(il, Typeob.String);
				}
				else
				{
					ast.TranslateToIL(il, Typeob.Object);
					ConstantWrapper.TranslateToILInt(il, 1);
					il.Emit(OpCodes.Call, CompilerGlobals.toStringMethod);
				}
				if (i == count - 1)
				{
					il.Emit(OpCodes.Call, CompilerGlobals.writeLineMethod);
				}
				else
				{
					il.Emit(OpCodes.Call, CompilerGlobals.writeMethod);
				}
			}
			if (count == 0)
			{
				il.Emit(OpCodes.Ldstr, "");
				il.Emit(OpCodes.Call, CompilerGlobals.writeLineMethod);
			}
			if (rtype != Typeob.Void)
			{
				il.Emit(OpCodes.Ldsfld, CompilerGlobals.undefinedField);
				Convert.Emit(this, il, Typeob.Object, rtype);
			}
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00054684 File Offset: 0x00053684
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			ASTList astlist = this.operand;
			for (int i = 0; i < astlist.count; i++)
			{
				astlist[i].TranslateToILInitializer(il);
			}
		}

		// Token: 0x040006A8 RID: 1704
		private ASTList operand;

		// Token: 0x040006A9 RID: 1705
		private Completion completion;
	}
}
