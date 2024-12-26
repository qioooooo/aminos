using System;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x020000F2 RID: 242
	internal sealed class ObjectLiteral : AST
	{
		// Token: 0x06000A9C RID: 2716 RVA: 0x00051610 File Offset: 0x00050610
		internal ObjectLiteral(Context context, ASTList propertyList)
			: base(context)
		{
			int count = propertyList.count;
			this.keys = new AST[count];
			this.values = new AST[count];
			for (int i = 0; i < count; i++)
			{
				ASTList astlist = (ASTList)propertyList[i];
				this.keys[i] = astlist[0];
				this.values[i] = astlist[1];
			}
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0005167C File Offset: 0x0005067C
		internal override void CheckIfOKToUseInSuperConstructorCall()
		{
			int i = 0;
			int num = this.values.Length;
			while (i < num)
			{
				this.values[i].CheckIfOKToUseInSuperConstructorCall();
				i++;
			}
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x000516AC File Offset: 0x000506AC
		internal override object Evaluate()
		{
			JSObject jsobject = base.Engine.GetOriginalObjectConstructor().ConstructObject();
			int i = 0;
			int num = this.keys.Length;
			while (i < num)
			{
				jsobject.SetMemberValue(this.keys[i].Evaluate().ToString(), this.values[i].Evaluate());
				i++;
			}
			return jsobject;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00051708 File Offset: 0x00050708
		internal override AST PartiallyEvaluate()
		{
			int num = this.keys.Length;
			for (int i = 0; i < num; i++)
			{
				this.keys[i] = this.keys[i].PartiallyEvaluate();
				this.values[i] = this.values[i].PartiallyEvaluate();
			}
			return this;
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00051758 File Offset: 0x00050758
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			int num = this.keys.Length;
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.getOriginalObjectConstructorMethod);
			il.Emit(OpCodes.Call, CompilerGlobals.constructObjectMethod);
			for (int i = 0; i < num; i++)
			{
				il.Emit(OpCodes.Dup);
				this.keys[i].TranslateToIL(il, Typeob.String);
				this.values[i].TranslateToIL(il, Typeob.Object);
				il.Emit(OpCodes.Call, CompilerGlobals.setMemberValue2Method);
			}
			Convert.Emit(this, il, Typeob.Object, rtype);
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x000517F0 File Offset: 0x000507F0
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			int i = 0;
			int num = this.keys.Length;
			while (i < num)
			{
				this.keys[i].TranslateToILInitializer(il);
				this.values[i].TranslateToILInitializer(il);
				i++;
			}
		}

		// Token: 0x04000681 RID: 1665
		internal AST[] keys;

		// Token: 0x04000682 RID: 1666
		internal AST[] values;
	}
}
