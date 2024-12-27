using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000126 RID: 294
	public sealed class Typeof : UnaryOp
	{
		// Token: 0x06000C81 RID: 3201 RVA: 0x0005B5F0 File Offset: 0x0005A5F0
		internal Typeof(Context context, AST operand)
			: base(context, operand)
		{
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0005B5FC File Offset: 0x0005A5FC
		internal override object Evaluate()
		{
			object obj;
			try
			{
				obj = Typeof.JScriptTypeof(this.operand.Evaluate(), VsaEngine.executeForJSEE);
			}
			catch (JScriptException ex)
			{
				if ((ex.Number & 65535) != 5009)
				{
					throw ex;
				}
				obj = "undefined";
			}
			return obj;
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x0005B650 File Offset: 0x0005A650
		internal override IReflect InferType(JSField inference_target)
		{
			return Typeob.String;
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x0005B657 File Offset: 0x0005A657
		public static string JScriptTypeof(object value)
		{
			return Typeof.JScriptTypeof(value, false);
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0005B660 File Offset: 0x0005A660
		internal static string JScriptTypeof(object value, bool checkForDebuggerObject)
		{
			switch (Convert.GetTypeCode(value))
			{
			case TypeCode.Empty:
				return "undefined";
			case TypeCode.Object:
				if (value is Missing || value is Missing)
				{
					return "undefined";
				}
				if (checkForDebuggerObject)
				{
					IDebuggerObject debuggerObject = value as IDebuggerObject;
					if (debuggerObject != null)
					{
						if (!debuggerObject.IsScriptFunction())
						{
							return "object";
						}
						return "function";
					}
				}
				if (!(value is ScriptFunction))
				{
					return "object";
				}
				return "function";
			case TypeCode.DBNull:
				return "object";
			case TypeCode.Boolean:
				return "boolean";
			case TypeCode.Char:
			case TypeCode.String:
				return "string";
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return "number";
			case TypeCode.DateTime:
				return "date";
			}
			return "unknown";
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x0005B73C File Offset: 0x0005A73C
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if (this.operand is Binding)
			{
				((Binding)this.operand).TranslateToIL(il, Typeob.Object, true);
			}
			else
			{
				this.operand.TranslateToIL(il, Typeob.Object);
			}
			il.Emit(OpCodes.Call, CompilerGlobals.jScriptTypeofMethod);
			Convert.Emit(this, il, Typeob.String, rtype);
		}
	}
}
