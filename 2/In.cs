using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices.Expando;

namespace Microsoft.JScript
{
	// Token: 0x02000090 RID: 144
	public sealed class In : BinaryOp
	{
		// Token: 0x06000698 RID: 1688 RVA: 0x0002E8BD File Offset: 0x0002D8BD
		internal In(Context context, AST operand1, AST operand2)
			: base(context, operand1, operand2)
		{
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0002E8C8 File Offset: 0x0002D8C8
		internal override object Evaluate()
		{
			object obj = this.operand1.Evaluate();
			object obj2 = this.operand2.Evaluate();
			object obj3;
			try
			{
				obj3 = In.JScriptIn(obj, obj2);
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

		// Token: 0x0600069A RID: 1690 RVA: 0x0002E92C File Offset: 0x0002D92C
		internal override IReflect InferType(JSField inference_target)
		{
			return Typeob.Boolean;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0002E934 File Offset: 0x0002D934
		public static bool JScriptIn(object v1, object v2)
		{
			bool flag = false;
			if (v2 is ScriptObject)
			{
				return !(((ScriptObject)v2).GetMemberValue(Convert.ToString(v1)) is Missing);
			}
			if (v2 is Array)
			{
				Array array = (Array)v2;
				double num = Convert.ToNumber(v1);
				int num2 = (int)num;
				return num == (double)num2 && array.GetLowerBound(0) <= num2 && num2 <= array.GetUpperBound(0);
			}
			if (v2 is IEnumerable)
			{
				if (v1 == null)
				{
					return false;
				}
				if (v2 is IDictionary)
				{
					return ((IDictionary)v2).Contains(v1);
				}
				if (v2 is IExpando)
				{
					MemberInfo[] member = ((IReflect)v2).GetMember(Convert.ToString(v1), BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
					return member.Length > 0;
				}
				IEnumerator enumerator = ((IEnumerable)v2).GetEnumerator();
				while (!flag)
				{
					if (!enumerator.MoveNext())
					{
						break;
					}
					if (v1.Equals(enumerator.Current))
					{
						return true;
					}
				}
			}
			else if (v2 is IEnumerator)
			{
				if (v1 == null)
				{
					return false;
				}
				IEnumerator enumerator2 = (IEnumerator)v2;
				while (!flag)
				{
					if (!enumerator2.MoveNext())
					{
						break;
					}
					if (v1.Equals(enumerator2.Current))
					{
						return true;
					}
				}
			}
			else if (v2 is IDebuggerObject)
			{
				return ((IDebuggerObject)v2).HasEnumerableMember(Convert.ToString(v1));
			}
			throw new JScriptException(JSError.ObjectExpected);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0002EA70 File Offset: 0x0002DA70
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.operand1.TranslateToIL(il, Typeob.Object);
			this.operand2.TranslateToIL(il, Typeob.Object);
			il.Emit(OpCodes.Call, CompilerGlobals.jScriptInMethod);
			Convert.Emit(this, il, Typeob.Boolean, rtype);
		}
	}
}
