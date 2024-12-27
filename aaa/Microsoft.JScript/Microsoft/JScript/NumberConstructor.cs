using System;

namespace Microsoft.JScript
{
	// Token: 0x020000ED RID: 237
	public class NumberConstructor : ScriptFunction
	{
		// Token: 0x06000A6B RID: 2667 RVA: 0x0004F30F File Offset: 0x0004E30F
		internal NumberConstructor()
			: base(FunctionPrototype.ob, "Number", 1)
		{
			this.originalPrototype = NumberPrototype.ob;
			NumberPrototype._constructor = this;
			this.proto = NumberPrototype.ob;
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x0004F33E File Offset: 0x0004E33E
		internal NumberConstructor(LenientFunctionPrototype parent, LenientNumberPrototype prototypeProp)
			: base(parent, "Number", 1)
		{
			this.originalPrototype = prototypeProp;
			prototypeProp.constructor = this;
			this.proto = prototypeProp;
			this.noExpando = false;
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x0004F369 File Offset: 0x0004E369
		internal override object Call(object[] args, object thisob)
		{
			if (args.Length == 0)
			{
				return 0;
			}
			return Convert.ToNumber(args[0]);
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x0004F384 File Offset: 0x0004E384
		internal NumberObject Construct()
		{
			return new NumberObject(this.originalPrototype, 0.0, false);
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0004F3A0 File Offset: 0x0004E3A0
		internal override object Construct(object[] args)
		{
			return this.CreateInstance(args);
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x0004F3A9 File Offset: 0x0004E3A9
		internal NumberObject ConstructImplicitWrapper(object arg)
		{
			return new NumberObject(this.originalPrototype, arg, true);
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x0004F3B8 File Offset: 0x0004E3B8
		internal NumberObject ConstructWrapper(object arg)
		{
			return new NumberObject(this.originalPrototype, arg, false);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x0004F3C7 File Offset: 0x0004E3C7
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public new NumberObject CreateInstance(params object[] args)
		{
			if (args.Length == 0)
			{
				return new NumberObject(this.originalPrototype, 0.0, false);
			}
			return new NumberObject(this.originalPrototype, Convert.ToNumber(args[0]), false);
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0004F402 File Offset: 0x0004E402
		public double Invoke(object arg)
		{
			return Convert.ToNumber(arg);
		}

		// Token: 0x04000671 RID: 1649
		public const double MAX_VALUE = 1.7976931348623157E+308;

		// Token: 0x04000672 RID: 1650
		public const double MIN_VALUE = 5E-324;

		// Token: 0x04000673 RID: 1651
		public const double NaN = double.NaN;

		// Token: 0x04000674 RID: 1652
		public const double NEGATIVE_INFINITY = double.NegativeInfinity;

		// Token: 0x04000675 RID: 1653
		public const double POSITIVE_INFINITY = double.PositiveInfinity;

		// Token: 0x04000676 RID: 1654
		internal static readonly NumberConstructor ob = new NumberConstructor();

		// Token: 0x04000677 RID: 1655
		private NumberPrototype originalPrototype;
	}
}
