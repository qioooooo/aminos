using System;

namespace Microsoft.JScript
{
	// Token: 0x0200012D RID: 301
	public sealed class VBArrayConstructor : ScriptFunction
	{
		// Token: 0x06000DDC RID: 3548 RVA: 0x0005E422 File Offset: 0x0005D422
		internal VBArrayConstructor()
			: base(FunctionPrototype.ob, "VBArray", 1)
		{
			this.originalPrototype = VBArrayPrototype.ob;
			VBArrayPrototype._constructor = this;
			this.proto = VBArrayPrototype.ob;
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0005E451 File Offset: 0x0005D451
		internal VBArrayConstructor(LenientFunctionPrototype parent, LenientVBArrayPrototype prototypeProp)
			: base(parent, "VBArray", 1)
		{
			this.originalPrototype = prototypeProp;
			prototypeProp.constructor = this;
			this.proto = prototypeProp;
			this.noExpando = false;
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0005E47C File Offset: 0x0005D47C
		internal override object Call(object[] args, object thisob)
		{
			return null;
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0005E47F File Offset: 0x0005D47F
		internal override object Construct(object[] args)
		{
			return this.CreateInstance(args);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0005E488 File Offset: 0x0005D488
		internal VBArrayObject Construct()
		{
			return new VBArrayObject(this.originalPrototype, null);
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0005E496 File Offset: 0x0005D496
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public new object CreateInstance(params object[] args)
		{
			if (args.Length < 1 || !typeof(Array).IsAssignableFrom(args[0].GetType()))
			{
				throw new JScriptException(JSError.VBArrayExpected);
			}
			return new VBArrayObject(this.originalPrototype, (Array)args[0]);
		}

		// Token: 0x04000784 RID: 1924
		internal static readonly VBArrayConstructor ob = new VBArrayConstructor();

		// Token: 0x04000785 RID: 1925
		private VBArrayPrototype originalPrototype;
	}
}
