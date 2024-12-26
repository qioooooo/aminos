using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000018 RID: 24
	internal sealed class Assign : AST
	{
		// Token: 0x06000118 RID: 280 RVA: 0x00006CB7 File Offset: 0x00005CB7
		internal Assign(Context context, AST lhside, AST rhside)
			: base(context)
		{
			this.lhside = lhside;
			this.rhside = rhside;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00006CD0 File Offset: 0x00005CD0
		internal override object Evaluate()
		{
			object obj2;
			try
			{
				if (this.lhside is Call)
				{
					((Call)this.lhside).EvaluateIndices();
				}
				object obj = this.rhside.Evaluate();
				this.lhside.SetValue(obj);
				obj2 = obj;
			}
			catch (JScriptException ex)
			{
				if (ex.context == null)
				{
					ex.context = this.context;
				}
				throw ex;
			}
			catch (Exception ex2)
			{
				throw new JScriptException(ex2, this.context);
			}
			catch
			{
				throw new JScriptException(JSError.NonClsException, this.context);
			}
			return obj2;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00006D78 File Offset: 0x00005D78
		internal override IReflect InferType(JSField inference_target)
		{
			return this.rhside.InferType(inference_target);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00006D88 File Offset: 0x00005D88
		internal override AST PartiallyEvaluate()
		{
			AST ast = this.lhside.PartiallyEvaluateAsReference();
			this.lhside = ast;
			this.rhside = this.rhside.PartiallyEvaluate();
			ast.SetPartialValue(this.rhside);
			return this;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00006DC8 File Offset: 0x00005DC8
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Type type = Convert.ToType(this.lhside.InferType(null));
			this.lhside.TranslateToILPreSet(il);
			if (rtype != Typeob.Void)
			{
				Type type2 = Convert.ToType(this.rhside.InferType(null));
				this.rhside.TranslateToIL(il, type2);
				LocalBuilder localBuilder = il.DeclareLocal(type2);
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Stloc, localBuilder);
				Convert.Emit(this, il, type2, type);
				this.lhside.TranslateToILSet(il);
				il.Emit(OpCodes.Ldloc, localBuilder);
				Convert.Emit(this, il, type2, rtype);
				return;
			}
			this.lhside.TranslateToILSet(il, this.rhside);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00006E76 File Offset: 0x00005E76
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.lhside.TranslateToILInitializer(il);
			this.rhside.TranslateToILInitializer(il);
		}

		// Token: 0x04000040 RID: 64
		internal AST lhside;

		// Token: 0x04000041 RID: 65
		internal AST rhside;
	}
}
