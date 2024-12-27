using System;

namespace Microsoft.JScript
{
	// Token: 0x02000030 RID: 48
	public class BooleanPrototype : BooleanObject
	{
		// Token: 0x060001F4 RID: 500 RVA: 0x0000EFCD File Offset: 0x0000DFCD
		protected BooleanPrototype(ObjectPrototype parent, Type baseType)
			: base(parent, baseType)
		{
			this.noExpando = true;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000EFDE File Offset: 0x0000DFDE
		public static BooleanConstructor constructor
		{
			get
			{
				return BooleanPrototype._constructor;
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000EFE5 File Offset: 0x0000DFE5
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Boolean_toString)]
		public static string toString(object thisob)
		{
			if (thisob is BooleanObject)
			{
				return Convert.ToString(((BooleanObject)thisob).value);
			}
			if (Convert.GetTypeCode(thisob) == TypeCode.Boolean)
			{
				return Convert.ToString(thisob);
			}
			throw new JScriptException(JSError.BooleanExpected);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000F01A File Offset: 0x0000E01A
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Boolean_valueOf)]
		public static object valueOf(object thisob)
		{
			if (thisob is BooleanObject)
			{
				return ((BooleanObject)thisob).value;
			}
			if (Convert.GetTypeCode(thisob) == TypeCode.Boolean)
			{
				return thisob;
			}
			throw new JScriptException(JSError.BooleanExpected);
		}

		// Token: 0x0400009A RID: 154
		internal static readonly BooleanPrototype ob = new BooleanPrototype(ObjectPrototype.ob, typeof(BooleanPrototype));

		// Token: 0x0400009B RID: 155
		internal static BooleanConstructor _constructor;
	}
}
