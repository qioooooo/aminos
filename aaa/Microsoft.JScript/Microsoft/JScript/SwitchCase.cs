using System;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200011D RID: 285
	internal sealed class SwitchCase : AST
	{
		// Token: 0x06000BB1 RID: 2993 RVA: 0x00059108 File Offset: 0x00058108
		internal SwitchCase(Context context, AST statements)
			: this(context, null, statements)
		{
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00059113 File Offset: 0x00058113
		internal SwitchCase(Context context, AST case_value, AST statements)
			: base(context)
		{
			this.case_value = case_value;
			this.statements = statements;
			this.completion = new Completion();
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00059135 File Offset: 0x00058135
		internal override object Evaluate()
		{
			return this.statements.Evaluate();
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00059142 File Offset: 0x00058142
		internal Completion Evaluate(object expression)
		{
			if (StrictEquality.JScriptStrictEquals(this.case_value.Evaluate(), expression))
			{
				return (Completion)this.statements.Evaluate();
			}
			return null;
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00059169 File Offset: 0x00058169
		internal bool IsDefault()
		{
			return this.case_value == null;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00059174 File Offset: 0x00058174
		internal override AST PartiallyEvaluate()
		{
			if (this.case_value != null)
			{
				this.case_value = this.case_value.PartiallyEvaluate();
			}
			this.statements = this.statements.PartiallyEvaluate();
			return this;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x000591A4 File Offset: 0x000581A4
		internal void TranslateToConditionalBranch(ILGenerator il, Type etype, bool branchIfTrue, Label label, bool shortForm)
		{
			Type type = etype;
			Type type2 = Convert.ToType(this.case_value.InferType(null));
			if (type != type2 && type.IsPrimitive && type2.IsPrimitive)
			{
				if (type == Typeob.Single && type2 == Typeob.Double)
				{
					type2 = Typeob.Single;
				}
				else if (Convert.IsPromotableTo(type2, type))
				{
					type2 = type;
				}
				else if (Convert.IsPromotableTo(type, type2))
				{
					type = type2;
				}
			}
			bool flag = true;
			if (type == type2 && type != Typeob.Object)
			{
				Convert.Emit(this, il, etype, type);
				if (!type.IsPrimitive && type.IsValueType)
				{
					il.Emit(OpCodes.Box, type);
				}
				this.case_value.context.EmitLineInfo(il);
				this.case_value.TranslateToIL(il, type);
				if (type == Typeob.String)
				{
					il.Emit(OpCodes.Call, CompilerGlobals.stringEqualsMethod);
				}
				else if (!type.IsPrimitive)
				{
					if (type.IsValueType)
					{
						il.Emit(OpCodes.Box, type);
					}
					il.Emit(OpCodes.Callvirt, CompilerGlobals.equalsMethod);
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				Convert.Emit(this, il, etype, Typeob.Object);
				this.case_value.context.EmitLineInfo(il);
				this.case_value.TranslateToIL(il, Typeob.Object);
				il.Emit(OpCodes.Call, CompilerGlobals.jScriptStrictEqualsMethod);
			}
			if (branchIfTrue)
			{
				if (flag)
				{
					il.Emit(shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue, label);
					return;
				}
				il.Emit(shortForm ? OpCodes.Beq_S : OpCodes.Beq, label);
				return;
			}
			else
			{
				if (flag)
				{
					il.Emit(shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse, label);
					return;
				}
				il.Emit(shortForm ? OpCodes.Bne_Un_S : OpCodes.Bne_Un, label);
				return;
			}
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00059357 File Offset: 0x00058357
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.statements.TranslateToIL(il, Typeob.Void);
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0005936A File Offset: 0x0005836A
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			if (this.case_value != null)
			{
				this.case_value.TranslateToILInitializer(il);
			}
			this.statements.TranslateToILInitializer(il);
		}

		// Token: 0x04000701 RID: 1793
		private AST case_value;

		// Token: 0x04000702 RID: 1794
		private AST statements;

		// Token: 0x04000703 RID: 1795
		private Completion completion;
	}
}
