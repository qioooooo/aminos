using System;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000019 RID: 25
	public sealed class ASTList : AST
	{
		// Token: 0x0600011E RID: 286 RVA: 0x00006E90 File Offset: 0x00005E90
		internal ASTList(Context context)
			: base(context)
		{
			this.count = 0;
			this.list = new AST[16];
			this.array = null;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006EB4 File Offset: 0x00005EB4
		internal ASTList Append(AST elem)
		{
			int num = this.count++;
			if (this.list.Length == num)
			{
				this.Grow();
			}
			this.list[num] = elem;
			this.context.UpdateWith(elem.context);
			return this;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006EFF File Offset: 0x00005EFF
		internal override object Evaluate()
		{
			return this.EvaluateAsArray();
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006F08 File Offset: 0x00005F08
		internal object[] EvaluateAsArray()
		{
			int num = this.count;
			object[] array = this.array;
			if (array == null)
			{
				array = (this.array = new object[num]);
			}
			AST[] array2 = this.list;
			for (int i = 0; i < num; i++)
			{
				array[i] = array2[i].Evaluate();
			}
			return array;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006F54 File Offset: 0x00005F54
		private void Grow()
		{
			AST[] array = this.list;
			int num = array.Length;
			AST[] array2 = (this.list = new AST[num + 16]);
			for (int i = 0; i < num; i++)
			{
				array2[i] = array[i];
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006F94 File Offset: 0x00005F94
		internal override AST PartiallyEvaluate()
		{
			AST[] array = this.list;
			int i = 0;
			int num = this.count;
			while (i < num)
			{
				array[i] = array[i].PartiallyEvaluate();
				i++;
			}
			return this;
		}

		// Token: 0x17000010 RID: 16
		internal AST this[int i]
		{
			get
			{
				return this.list[i];
			}
			set
			{
				this.list[i] = value;
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006FDC File Offset: 0x00005FDC
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Type elementType = rtype.GetElementType();
			int num = this.count;
			ConstantWrapper.TranslateToILInt(il, num);
			il.Emit(OpCodes.Newarr, elementType);
			bool flag = elementType.IsValueType && !elementType.IsPrimitive;
			AST[] array = this.list;
			for (int i = 0; i < num; i++)
			{
				il.Emit(OpCodes.Dup);
				ConstantWrapper.TranslateToILInt(il, i);
				array[i].TranslateToIL(il, elementType);
				if (flag)
				{
					il.Emit(OpCodes.Ldelema, elementType);
				}
				Binding.TranslateToStelem(il, elementType);
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000706C File Offset: 0x0000606C
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			AST[] array = this.list;
			int i = 0;
			int num = this.count;
			while (i < num)
			{
				array[i].TranslateToILInitializer(il);
				i++;
			}
		}

		// Token: 0x04000042 RID: 66
		internal int count;

		// Token: 0x04000043 RID: 67
		private AST[] list;

		// Token: 0x04000044 RID: 68
		private object[] array;
	}
}
