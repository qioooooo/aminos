using System;

namespace Microsoft.JScript
{
	// Token: 0x020000D0 RID: 208
	public sealed class LenientFunctionPrototype : FunctionPrototype
	{
		// Token: 0x06000949 RID: 2377 RVA: 0x00048ED0 File Offset: 0x00047ED0
		internal LenientFunctionPrototype(ScriptObject parent)
			: base(parent)
		{
			this.noExpando = false;
			Type typeFromHandle = typeof(FunctionPrototype);
			this.apply = new BuiltinFunction("apply", this, typeFromHandle.GetMethod("apply"), this);
			this.call = new BuiltinFunction("call", this, typeFromHandle.GetMethod("call"), this);
			this.toString = new BuiltinFunction("toString", this, typeFromHandle.GetMethod("toString"), this);
		}

		// Token: 0x040005AD RID: 1453
		public new object constructor;

		// Token: 0x040005AE RID: 1454
		public new object apply;

		// Token: 0x040005AF RID: 1455
		public new object call;

		// Token: 0x040005B0 RID: 1456
		public new object toString;
	}
}
