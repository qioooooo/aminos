using System;

namespace Microsoft.JScript
{
	// Token: 0x020000D6 RID: 214
	public sealed class LenientNumberPrototype : NumberPrototype
	{
		// Token: 0x060009B0 RID: 2480 RVA: 0x0004A560 File Offset: 0x00049560
		internal LenientNumberPrototype(LenientFunctionPrototype funcprot, LenientObjectPrototype parent)
			: base(parent)
		{
			this.noExpando = false;
			Type typeFromHandle = typeof(NumberPrototype);
			this.toExponential = new BuiltinFunction("toExponential", this, typeFromHandle.GetMethod("toExponential"), funcprot);
			this.toFixed = new BuiltinFunction("toFixed", this, typeFromHandle.GetMethod("toFixed"), funcprot);
			this.toLocaleString = new BuiltinFunction("toLocaleString", this, typeFromHandle.GetMethod("toLocaleString"), funcprot);
			this.toPrecision = new BuiltinFunction("toPrecision", this, typeFromHandle.GetMethod("toPrecision"), funcprot);
			this.toString = new BuiltinFunction("toString", this, typeFromHandle.GetMethod("toString"), funcprot);
			this.valueOf = new BuiltinFunction("valueOf", this, typeFromHandle.GetMethod("valueOf"), funcprot);
		}

		// Token: 0x04000612 RID: 1554
		public new object constructor;

		// Token: 0x04000613 RID: 1555
		public new object toExponential;

		// Token: 0x04000614 RID: 1556
		public new object toFixed;

		// Token: 0x04000615 RID: 1557
		public new object toLocaleString;

		// Token: 0x04000616 RID: 1558
		public new object toPrecision;

		// Token: 0x04000617 RID: 1559
		public new object toString;

		// Token: 0x04000618 RID: 1560
		public new object valueOf;
	}
}
