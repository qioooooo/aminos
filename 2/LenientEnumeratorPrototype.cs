using System;

namespace Microsoft.JScript
{
	// Token: 0x020000CE RID: 206
	public sealed class LenientEnumeratorPrototype : EnumeratorPrototype
	{
		// Token: 0x06000947 RID: 2375 RVA: 0x00048DE8 File Offset: 0x00047DE8
		internal LenientEnumeratorPrototype(LenientFunctionPrototype funcprot, LenientObjectPrototype parent)
			: base(parent)
		{
			this.noExpando = false;
			Type typeFromHandle = typeof(EnumeratorPrototype);
			this.atEnd = new BuiltinFunction("atEnd", this, typeFromHandle.GetMethod("atEnd"), funcprot);
			this.item = new BuiltinFunction("item", this, typeFromHandle.GetMethod("item"), funcprot);
			this.moveFirst = new BuiltinFunction("moveFirst", this, typeFromHandle.GetMethod("moveFirst"), funcprot);
			this.moveNext = new BuiltinFunction("moveNext", this, typeFromHandle.GetMethod("moveNext"), funcprot);
		}

		// Token: 0x040005A5 RID: 1445
		public new object constructor;

		// Token: 0x040005A6 RID: 1446
		public new object atEnd;

		// Token: 0x040005A7 RID: 1447
		public new object item;

		// Token: 0x040005A8 RID: 1448
		public new object moveFirst;

		// Token: 0x040005A9 RID: 1449
		public new object moveNext;
	}
}
