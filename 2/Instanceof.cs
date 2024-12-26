using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000092 RID: 146
	public sealed class Instanceof : BinaryOp
	{
		// Token: 0x0600069F RID: 1695 RVA: 0x0002EABC File Offset: 0x0002DABC
		internal Instanceof(Context context, AST operand1, AST operand2)
			: base(context, operand1, operand2)
		{
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0002EAC8 File Offset: 0x0002DAC8
		internal override object Evaluate()
		{
			object obj = this.operand1.Evaluate();
			object obj2 = this.operand2.Evaluate();
			object obj3;
			try
			{
				obj3 = Instanceof.JScriptInstanceof(obj, obj2);
			}
			catch (JScriptException ex)
			{
				if (ex.context == null)
				{
					ex.context = this.operand2.context;
				}
				throw ex;
			}
			return obj3;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0002EB2C File Offset: 0x0002DB2C
		internal override IReflect InferType(JSField inference_target)
		{
			return Typeob.Boolean;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0002EB34 File Offset: 0x0002DB34
		public static bool JScriptInstanceof(object v1, object v2)
		{
			if (v2 is ClassScope)
			{
				return ((ClassScope)v2).HasInstance(v1);
			}
			if (v2 is ScriptFunction)
			{
				return ((ScriptFunction)v2).HasInstance(v1);
			}
			if (v1 == null)
			{
				return false;
			}
			if (v2 is Type)
			{
				Type type = v1.GetType();
				if (v1 is IConvertible)
				{
					try
					{
						Convert.CoerceT(v1, (Type)v2);
						return true;
					}
					catch (JScriptException)
					{
						return false;
					}
				}
				return ((Type)v2).IsAssignableFrom(type);
			}
			if (v2 is IDebugType)
			{
				return ((IDebugType)v2).HasInstance(v1);
			}
			throw new JScriptException(JSError.NeedType);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0002EBDC File Offset: 0x0002DBDC
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.operand1.TranslateToIL(il, Typeob.Object);
			object obj = null;
			if (this.operand2 is ConstantWrapper && (obj = this.operand2.Evaluate()) is Type && !((Type)obj).IsValueType)
			{
				il.Emit(OpCodes.Isinst, (Type)obj);
				il.Emit(OpCodes.Ldnull);
				il.Emit(OpCodes.Cgt_Un);
			}
			else if (obj is ClassScope)
			{
				il.Emit(OpCodes.Isinst, ((ClassScope)obj).GetTypeBuilderOrEnumBuilder());
				il.Emit(OpCodes.Ldnull);
				il.Emit(OpCodes.Cgt_Un);
			}
			else
			{
				this.operand2.TranslateToIL(il, Typeob.Object);
				il.Emit(OpCodes.Call, CompilerGlobals.jScriptInstanceofMethod);
			}
			Convert.Emit(this, il, Typeob.Boolean, rtype);
		}
	}
}
