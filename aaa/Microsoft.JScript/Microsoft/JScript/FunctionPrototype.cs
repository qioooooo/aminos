using System;

namespace Microsoft.JScript
{
	// Token: 0x02000085 RID: 133
	public class FunctionPrototype : ScriptFunction
	{
		// Token: 0x060005FB RID: 1531 RVA: 0x0002C1B7 File Offset: 0x0002B1B7
		internal FunctionPrototype(ScriptObject parent)
			: base(parent)
		{
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0002C1C0 File Offset: 0x0002B1C0
		internal override object Call(object[] args, object thisob)
		{
			return null;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0002C1C4 File Offset: 0x0002B1C4
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Function_apply)]
		public static object apply(object thisob, object thisarg, object argArray)
		{
			if (!(thisob is ScriptFunction))
			{
				throw new JScriptException(JSError.FunctionExpected);
			}
			if (thisarg is Missing)
			{
				thisarg = ((IActivationObject)((ScriptFunction)thisob).engine.ScriptObjectStackTop()).GetDefaultThisObject();
			}
			if (argArray is Missing)
			{
				return ((ScriptFunction)thisob).Call(new object[0], thisarg);
			}
			if (argArray is ArgumentsObject)
			{
				return ((ScriptFunction)thisob).Call(((ArgumentsObject)argArray).ToArray(), thisarg);
			}
			if (argArray is ArrayObject)
			{
				return ((ScriptFunction)thisob).Call(((ArrayObject)argArray).ToArray(), thisarg);
			}
			throw new JScriptException(JSError.InvalidCall);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0002C26C File Offset: 0x0002B26C
		[JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.Function_call)]
		public static object call(object thisob, object thisarg, params object[] args)
		{
			if (!(thisob is ScriptFunction))
			{
				throw new JScriptException(JSError.FunctionExpected);
			}
			if (thisarg is Missing)
			{
				thisarg = ((IActivationObject)((ScriptFunction)thisob).engine.ScriptObjectStackTop()).GetDefaultThisObject();
			}
			return ((ScriptFunction)thisob).Call(args, thisarg);
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x0002C2BD File Offset: 0x0002B2BD
		public static FunctionConstructor constructor
		{
			get
			{
				return FunctionPrototype._constructor;
			}
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0002C2C4 File Offset: 0x0002B2C4
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Function_toString)]
		public static string toString(object thisob)
		{
			if (thisob is ScriptFunction)
			{
				return thisob.ToString();
			}
			throw new JScriptException(JSError.FunctionExpected);
		}

		// Token: 0x040002B3 RID: 691
		internal static readonly FunctionPrototype ob = new FunctionPrototype(ObjectPrototype.CommonInstance());

		// Token: 0x040002B4 RID: 692
		internal static FunctionConstructor _constructor;
	}
}
