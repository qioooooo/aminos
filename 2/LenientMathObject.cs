using System;

namespace Microsoft.JScript
{
	// Token: 0x020000D3 RID: 211
	public sealed class LenientMathObject : MathObject
	{
		// Token: 0x0600099F RID: 2463 RVA: 0x00049DF8 File Offset: 0x00048DF8
		internal LenientMathObject(ScriptObject parent, FunctionPrototype funcprot)
			: base(parent)
		{
			this.noExpando = false;
			Type typeFromHandle = typeof(MathObject);
			this.abs = new BuiltinFunction("abs", this, typeFromHandle.GetMethod("abs"), funcprot);
			this.acos = new BuiltinFunction("acos", this, typeFromHandle.GetMethod("acos"), funcprot);
			this.asin = new BuiltinFunction("asin", this, typeFromHandle.GetMethod("asin"), funcprot);
			this.atan = new BuiltinFunction("atan", this, typeFromHandle.GetMethod("atan"), funcprot);
			this.atan2 = new BuiltinFunction("atan2", this, typeFromHandle.GetMethod("atan2"), funcprot);
			this.ceil = new BuiltinFunction("ceil", this, typeFromHandle.GetMethod("ceil"), funcprot);
			this.cos = new BuiltinFunction("cos", this, typeFromHandle.GetMethod("cos"), funcprot);
			this.exp = new BuiltinFunction("exp", this, typeFromHandle.GetMethod("exp"), funcprot);
			this.floor = new BuiltinFunction("floor", this, typeFromHandle.GetMethod("floor"), funcprot);
			this.log = new BuiltinFunction("log", this, typeFromHandle.GetMethod("log"), funcprot);
			this.max = new BuiltinFunction("max", this, typeFromHandle.GetMethod("max"), funcprot);
			this.min = new BuiltinFunction("min", this, typeFromHandle.GetMethod("min"), funcprot);
			this.pow = new BuiltinFunction("pow", this, typeFromHandle.GetMethod("pow"), funcprot);
			this.random = new BuiltinFunction("random", this, typeFromHandle.GetMethod("random"), funcprot);
			this.round = new BuiltinFunction("round", this, typeFromHandle.GetMethod("round"), funcprot);
			this.sin = new BuiltinFunction("sin", this, typeFromHandle.GetMethod("sin"), funcprot);
			this.sqrt = new BuiltinFunction("sqrt", this, typeFromHandle.GetMethod("sqrt"), funcprot);
			this.tan = new BuiltinFunction("tan", this, typeFromHandle.GetMethod("tan"), funcprot);
		}

		// Token: 0x040005F3 RID: 1523
		public new const double E = 2.718281828459045;

		// Token: 0x040005F4 RID: 1524
		public new const double LN10 = 2.302585092994046;

		// Token: 0x040005F5 RID: 1525
		public new const double LN2 = 0.6931471805599453;

		// Token: 0x040005F6 RID: 1526
		public new const double LOG2E = 1.4426950408889634;

		// Token: 0x040005F7 RID: 1527
		public new const double LOG10E = 0.4342944819032518;

		// Token: 0x040005F8 RID: 1528
		public new const double PI = 3.141592653589793;

		// Token: 0x040005F9 RID: 1529
		public new const double SQRT1_2 = 0.7071067811865476;

		// Token: 0x040005FA RID: 1530
		public new const double SQRT2 = 1.4142135623730951;

		// Token: 0x040005FB RID: 1531
		public new object abs;

		// Token: 0x040005FC RID: 1532
		public new object acos;

		// Token: 0x040005FD RID: 1533
		public new object asin;

		// Token: 0x040005FE RID: 1534
		public new object atan;

		// Token: 0x040005FF RID: 1535
		public new object atan2;

		// Token: 0x04000600 RID: 1536
		public new object ceil;

		// Token: 0x04000601 RID: 1537
		public new object cos;

		// Token: 0x04000602 RID: 1538
		public new object exp;

		// Token: 0x04000603 RID: 1539
		public new object floor;

		// Token: 0x04000604 RID: 1540
		public new object log;

		// Token: 0x04000605 RID: 1541
		public new object max;

		// Token: 0x04000606 RID: 1542
		public new object min;

		// Token: 0x04000607 RID: 1543
		public new object pow;

		// Token: 0x04000608 RID: 1544
		public new object random;

		// Token: 0x04000609 RID: 1545
		public new object round;

		// Token: 0x0400060A RID: 1546
		public new object sin;

		// Token: 0x0400060B RID: 1547
		public new object sqrt;

		// Token: 0x0400060C RID: 1548
		public new object tan;
	}
}
