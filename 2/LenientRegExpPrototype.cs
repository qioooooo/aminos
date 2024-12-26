using System;

namespace Microsoft.JScript
{
	// Token: 0x020000DA RID: 218
	public sealed class LenientRegExpPrototype : RegExpPrototype
	{
		// Token: 0x060009C4 RID: 2500 RVA: 0x0004AA3C File Offset: 0x00049A3C
		internal LenientRegExpPrototype(LenientFunctionPrototype funcprot, LenientObjectPrototype parent)
			: base(parent)
		{
			this.noExpando = false;
			Type typeFromHandle = typeof(RegExpPrototype);
			this.compile = new BuiltinFunction("compile", this, typeFromHandle.GetMethod("compile"), funcprot);
			this.exec = new BuiltinFunction("exec", this, typeFromHandle.GetMethod("exec"), funcprot);
			this.test = new BuiltinFunction("test", this, typeFromHandle.GetMethod("test"), funcprot);
			this.toString = new BuiltinFunction("toString", this, typeFromHandle.GetMethod("toString"), funcprot);
		}

		// Token: 0x04000624 RID: 1572
		public new object constructor;

		// Token: 0x04000625 RID: 1573
		public new object compile;

		// Token: 0x04000626 RID: 1574
		public new object exec;

		// Token: 0x04000627 RID: 1575
		public new object test;

		// Token: 0x04000628 RID: 1576
		public new object toString;
	}
}
