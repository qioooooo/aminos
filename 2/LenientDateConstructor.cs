using System;

namespace Microsoft.JScript
{
	// Token: 0x020000CC RID: 204
	public sealed class LenientDateConstructor : DateConstructor
	{
		// Token: 0x06000945 RID: 2373 RVA: 0x00048844 File Offset: 0x00047844
		internal LenientDateConstructor(LenientFunctionPrototype parent, LenientDatePrototype prototypeProp)
			: base(parent, prototypeProp)
		{
			this.noExpando = false;
			Type typeFromHandle = typeof(DateConstructor);
			this.parse = new BuiltinFunction("parse", this, typeFromHandle.GetMethod("parse"), parent);
			this.UTC = new BuiltinFunction("UTC", this, typeFromHandle.GetMethod("UTC"), parent);
		}

		// Token: 0x04000575 RID: 1397
		public new object parse;

		// Token: 0x04000576 RID: 1398
		public new object UTC;
	}
}
