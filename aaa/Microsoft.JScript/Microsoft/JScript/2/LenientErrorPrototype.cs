using System;

namespace Microsoft.JScript
{
	// Token: 0x020000CF RID: 207
	public sealed class LenientErrorPrototype : ErrorPrototype
	{
		// Token: 0x06000948 RID: 2376 RVA: 0x00048E84 File Offset: 0x00047E84
		internal LenientErrorPrototype(LenientFunctionPrototype funcprot, ScriptObject parent, string name)
			: base(parent, name)
		{
			this.noExpando = false;
			this.name = name;
			Type typeFromHandle = typeof(ErrorPrototype);
			this.toString = new BuiltinFunction("toString", this, typeFromHandle.GetMethod("toString"), funcprot);
		}

		// Token: 0x040005AA RID: 1450
		public new object constructor;

		// Token: 0x040005AB RID: 1451
		public new object name;

		// Token: 0x040005AC RID: 1452
		public new object toString;
	}
}
