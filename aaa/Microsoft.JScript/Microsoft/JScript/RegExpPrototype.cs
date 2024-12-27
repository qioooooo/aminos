using System;

namespace Microsoft.JScript
{
	// Token: 0x020000D9 RID: 217
	public class RegExpPrototype : JSObject
	{
		// Token: 0x060009BD RID: 2493 RVA: 0x0004A8F6 File Offset: 0x000498F6
		internal RegExpPrototype(ObjectPrototype parent)
			: base(parent)
		{
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0004A900 File Offset: 0x00049900
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.RegExp_compile)]
		public static RegExpObject compile(object thisob, object source, object flags)
		{
			RegExpObject regExpObject = thisob as RegExpObject;
			if (regExpObject == null)
			{
				throw new JScriptException(JSError.RegExpExpected);
			}
			return regExpObject.compile((source == null || source is Missing) ? "" : Convert.ToString(source), (flags == null || flags is Missing) ? "" : Convert.ToString(flags));
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x0004A958 File Offset: 0x00049958
		public static RegExpConstructor constructor
		{
			get
			{
				return RegExpPrototype._constructor;
			}
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0004A960 File Offset: 0x00049960
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.RegExp_exec)]
		public static object exec(object thisob, object input)
		{
			RegExpObject regExpObject = thisob as RegExpObject;
			if (regExpObject == null)
			{
				throw new JScriptException(JSError.RegExpExpected);
			}
			if (input is Missing && !regExpObject.regExpConst.noExpando)
			{
				input = regExpObject.regExpConst.input;
			}
			return regExpObject.exec(Convert.ToString(input));
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0004A9B0 File Offset: 0x000499B0
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.RegExp_test)]
		public static bool test(object thisob, object input)
		{
			RegExpObject regExpObject = thisob as RegExpObject;
			if (regExpObject == null)
			{
				throw new JScriptException(JSError.RegExpExpected);
			}
			if (input is Missing && !regExpObject.regExpConst.noExpando)
			{
				input = regExpObject.regExpConst.input;
			}
			return regExpObject.test(Convert.ToString(input));
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0004AA00 File Offset: 0x00049A00
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.RegExp_toString)]
		public static string toString(object thisob)
		{
			RegExpObject regExpObject = thisob as RegExpObject;
			if (regExpObject == null)
			{
				throw new JScriptException(JSError.RegExpExpected);
			}
			return regExpObject.ToString();
		}

		// Token: 0x04000622 RID: 1570
		internal static readonly RegExpPrototype ob = new RegExpPrototype(ObjectPrototype.ob);

		// Token: 0x04000623 RID: 1571
		internal static RegExpConstructor _constructor;
	}
}
