using System;

namespace Microsoft.JScript
{
	// Token: 0x020000CB RID: 203
	public sealed class LenientBooleanPrototype : BooleanPrototype
	{
		// Token: 0x06000944 RID: 2372 RVA: 0x000487D8 File Offset: 0x000477D8
		internal LenientBooleanPrototype(LenientFunctionPrototype funcprot, LenientObjectPrototype parent)
			: base(parent, typeof(LenientBooleanPrototype))
		{
			this.noExpando = false;
			Type typeFromHandle = typeof(BooleanPrototype);
			this.toString = new BuiltinFunction("toString", this, typeFromHandle.GetMethod("toString"), funcprot);
			this.valueOf = new BuiltinFunction("valueOf", this, typeFromHandle.GetMethod("valueOf"), funcprot);
		}

		// Token: 0x04000572 RID: 1394
		public new object constructor;

		// Token: 0x04000573 RID: 1395
		public new object toString;

		// Token: 0x04000574 RID: 1396
		public new object valueOf;
	}
}
