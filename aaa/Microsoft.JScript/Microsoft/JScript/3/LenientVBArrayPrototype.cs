using System;

namespace Microsoft.JScript
{
	// Token: 0x020000E1 RID: 225
	public sealed class LenientVBArrayPrototype : VBArrayPrototype
	{
		// Token: 0x06000A0B RID: 2571 RVA: 0x0004BE24 File Offset: 0x0004AE24
		internal LenientVBArrayPrototype(LenientFunctionPrototype funcprot, LenientObjectPrototype parent)
			: base(funcprot, parent)
		{
			this.noExpando = false;
			Type typeFromHandle = typeof(VBArrayPrototype);
			this.dimensions = new BuiltinFunction("dimensions", this, typeFromHandle.GetMethod("dimensions"), funcprot);
			this.getItem = new BuiltinFunction("getItem", this, typeFromHandle.GetMethod("getItem"), funcprot);
			this.lbound = new BuiltinFunction("lbound", this, typeFromHandle.GetMethod("lbound"), funcprot);
			this.toArray = new BuiltinFunction("toArray", this, typeFromHandle.GetMethod("toArray"), funcprot);
			this.ubound = new BuiltinFunction("ubound", this, typeFromHandle.GetMethod("ubound"), funcprot);
		}

		// Token: 0x04000653 RID: 1619
		public new object constructor;

		// Token: 0x04000654 RID: 1620
		public new object dimensions;

		// Token: 0x04000655 RID: 1621
		public new object getItem;

		// Token: 0x04000656 RID: 1622
		public new object lbound;

		// Token: 0x04000657 RID: 1623
		public new object toArray;

		// Token: 0x04000658 RID: 1624
		public new object ubound;
	}
}
