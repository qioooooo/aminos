using System;

namespace Microsoft.JScript
{
	// Token: 0x0200002E RID: 46
	public sealed class BooleanConstructor : ScriptFunction
	{
		// Token: 0x060001E5 RID: 485 RVA: 0x0000EDE0 File Offset: 0x0000DDE0
		internal BooleanConstructor()
			: base(FunctionPrototype.ob, "Boolean", 1)
		{
			this.originalPrototype = BooleanPrototype.ob;
			BooleanPrototype._constructor = this;
			this.proto = BooleanPrototype.ob;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000EE0F File Offset: 0x0000DE0F
		internal BooleanConstructor(LenientFunctionPrototype parent, LenientBooleanPrototype prototypeProp)
			: base(parent, "Boolean", 1)
		{
			this.originalPrototype = prototypeProp;
			prototypeProp.constructor = this;
			this.proto = prototypeProp;
			this.noExpando = false;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000EE3A File Offset: 0x0000DE3A
		internal override object Call(object[] args, object thisob)
		{
			if (args.Length == 0)
			{
				return false;
			}
			return Convert.ToBoolean(args[0]);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000EE55 File Offset: 0x0000DE55
		internal BooleanObject Construct()
		{
			return new BooleanObject(this.originalPrototype, false, false);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000EE64 File Offset: 0x0000DE64
		internal override object Construct(object[] args)
		{
			return this.CreateInstance(args);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000EE6D File Offset: 0x0000DE6D
		internal BooleanObject ConstructImplicitWrapper(bool arg)
		{
			return new BooleanObject(this.originalPrototype, arg, true);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000EE7C File Offset: 0x0000DE7C
		internal BooleanObject ConstructWrapper(bool arg)
		{
			return new BooleanObject(this.originalPrototype, arg, false);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000EE8B File Offset: 0x0000DE8B
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public new BooleanObject CreateInstance(params object[] args)
		{
			return new BooleanObject(this.originalPrototype, args.Length != 0 && Convert.ToBoolean(args[0]), false);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000EEA9 File Offset: 0x0000DEA9
		public bool Invoke(object arg)
		{
			return Convert.ToBoolean(arg);
		}

		// Token: 0x04000096 RID: 150
		internal static readonly BooleanConstructor ob = new BooleanConstructor();

		// Token: 0x04000097 RID: 151
		private BooleanPrototype originalPrototype;
	}
}
