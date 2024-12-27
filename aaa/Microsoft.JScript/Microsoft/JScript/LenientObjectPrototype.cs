using System;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000D8 RID: 216
	public class LenientObjectPrototype : ObjectPrototype
	{
		// Token: 0x060009BB RID: 2491 RVA: 0x0004A818 File Offset: 0x00049818
		internal LenientObjectPrototype(VsaEngine engine)
		{
			this.engine = engine;
			this.noExpando = false;
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0004A830 File Offset: 0x00049830
		internal void Initialize(LenientFunctionPrototype funcprot)
		{
			Type typeFromHandle = typeof(ObjectPrototype);
			this.hasOwnProperty = new BuiltinFunction("hasOwnProperty", this, typeFromHandle.GetMethod("hasOwnProperty"), funcprot);
			this.isPrototypeOf = new BuiltinFunction("isPrototypeOf", this, typeFromHandle.GetMethod("isPrototypeOf"), funcprot);
			this.propertyIsEnumerable = new BuiltinFunction("propertyIsEnumerable", this, typeFromHandle.GetMethod("propertyIsEnumerable"), funcprot);
			this.toLocaleString = new BuiltinFunction("toLocaleString", this, typeFromHandle.GetMethod("toLocaleString"), funcprot);
			this.toString = new BuiltinFunction("toString", this, typeFromHandle.GetMethod("toString"), funcprot);
			this.valueOf = new BuiltinFunction("valueOf", this, typeFromHandle.GetMethod("valueOf"), funcprot);
		}

		// Token: 0x0400061B RID: 1563
		public new object constructor;

		// Token: 0x0400061C RID: 1564
		public new object hasOwnProperty;

		// Token: 0x0400061D RID: 1565
		public new object isPrototypeOf;

		// Token: 0x0400061E RID: 1566
		public new object propertyIsEnumerable;

		// Token: 0x0400061F RID: 1567
		public new object toLocaleString;

		// Token: 0x04000620 RID: 1568
		public new object toString;

		// Token: 0x04000621 RID: 1569
		public new object valueOf;
	}
}
