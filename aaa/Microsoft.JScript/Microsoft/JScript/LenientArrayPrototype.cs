using System;

namespace Microsoft.JScript
{
	// Token: 0x020000CA RID: 202
	public sealed class LenientArrayPrototype : ArrayPrototype
	{
		// Token: 0x06000943 RID: 2371 RVA: 0x00048654 File Offset: 0x00047654
		internal LenientArrayPrototype(FunctionPrototype funcprot, ObjectPrototype parent)
			: base(parent)
		{
			this.noExpando = false;
			Type typeFromHandle = typeof(ArrayPrototype);
			this.concat = new BuiltinFunction("concat", this, typeFromHandle.GetMethod("concat"), funcprot);
			this.join = new BuiltinFunction("join", this, typeFromHandle.GetMethod("join"), funcprot);
			this.pop = new BuiltinFunction("pop", this, typeFromHandle.GetMethod("pop"), funcprot);
			this.push = new BuiltinFunction("push", this, typeFromHandle.GetMethod("push"), funcprot);
			this.reverse = new BuiltinFunction("reverse", this, typeFromHandle.GetMethod("reverse"), funcprot);
			this.shift = new BuiltinFunction("shift", this, typeFromHandle.GetMethod("shift"), funcprot);
			this.slice = new BuiltinFunction("slice", this, typeFromHandle.GetMethod("slice"), funcprot);
			this.sort = new BuiltinFunction("sort", this, typeFromHandle.GetMethod("sort"), funcprot);
			this.splice = new BuiltinFunction("splice", this, typeFromHandle.GetMethod("splice"), funcprot);
			this.unshift = new BuiltinFunction("unshift", this, typeFromHandle.GetMethod("unshift"), funcprot);
			this.toLocaleString = new BuiltinFunction("toLocaleString", this, typeFromHandle.GetMethod("toLocaleString"), funcprot);
			this.toString = new BuiltinFunction("toString", this, typeFromHandle.GetMethod("toString"), funcprot);
		}

		// Token: 0x04000565 RID: 1381
		public new object constructor;

		// Token: 0x04000566 RID: 1382
		public new object concat;

		// Token: 0x04000567 RID: 1383
		public new object join;

		// Token: 0x04000568 RID: 1384
		public new object pop;

		// Token: 0x04000569 RID: 1385
		public new object push;

		// Token: 0x0400056A RID: 1386
		public new object reverse;

		// Token: 0x0400056B RID: 1387
		public new object shift;

		// Token: 0x0400056C RID: 1388
		public new object slice;

		// Token: 0x0400056D RID: 1389
		public new object sort;

		// Token: 0x0400056E RID: 1390
		public new object splice;

		// Token: 0x0400056F RID: 1391
		public new object unshift;

		// Token: 0x04000570 RID: 1392
		public new object toLocaleString;

		// Token: 0x04000571 RID: 1393
		public new object toString;
	}
}
