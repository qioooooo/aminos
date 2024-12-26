using System;

namespace Microsoft.JScript
{
	// Token: 0x020000DC RID: 220
	public class LenientStringConstructor : StringConstructor
	{
		// Token: 0x060009D0 RID: 2512 RVA: 0x0004ABF4 File Offset: 0x00049BF4
		internal LenientStringConstructor(LenientFunctionPrototype parent, LenientStringPrototype prototypeProp)
			: base(parent, prototypeProp)
		{
			this.noExpando = false;
			Type typeFromHandle = typeof(StringConstructor);
			this.fromCharCode = new BuiltinFunction("fromCharCode", this, typeFromHandle.GetMethod("fromCharCode"), parent);
		}

		// Token: 0x0400062B RID: 1579
		public new object fromCharCode;
	}
}
